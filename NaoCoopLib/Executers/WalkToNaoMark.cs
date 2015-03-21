using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using NaoCoopLib.Helpers;
using System.Collections;
using System.Threading;
using NaoCoopLib.Types;
using Aldebaran.Proxies;

namespace NaoCoopLib.Executers
{
    public class WalkToNaoMark : IDisposable
    {
        #region Members
        BackgroundWorker _walkWorker = new BackgroundWorker();
        NaoConnectionHelper _robotConnectionHelper = new NaoConnectionHelper();
        ArrayList _detectedMark = null;
        object _lockObject = new object();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the checkpoint mark information
        /// </summary>
        public WalkToLandMarkInfo MarkInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the start looking direction
        /// </summary>
        public FindNaoMark.LookDirection StartLookDirection
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns true if robot is busy
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _walkWorker.IsBusy;
            }
        }
        #endregion

        #region Events
        public event EventHandler<EventArgs> WalkCompleted;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="robotIP">Robot ip</param>
        /// <param name="robotPort">Robot port</param>
        /// <param name="markInfo">Checkpoint mark information</param>
        /// <param name="startLookDirection">Start look direction</param>
        public WalkToNaoMark(string robotIP, int robotPort, WalkToLandMarkInfo markInfo,
                             FindNaoMark.LookDirection startLookDirection = FindNaoMark.LookDirection.Left) :
            this(new NaoConnectionHelper(robotIP, robotPort), markInfo, startLookDirection)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection">Robot connection</param>
        /// <param name="markInfo">Checkpoint mark information</param>
        /// <param name="startLookDirection">Start look direction</param>
        public WalkToNaoMark(NaoConnectionHelper connection, WalkToLandMarkInfo markInfo,
                            FindNaoMark.LookDirection startLookDirection = FindNaoMark.LookDirection.Left)
        {
            this.MarkInfo = markInfo;
            this.StartLookDirection = startLookDirection;
            this._robotConnectionHelper = connection;

            if (!_robotConnectionHelper.TestConnection(string.Empty))
            {
                throw new ArgumentException("Could not connect to the robot!");
            }

            _walkWorker.DoWork += new DoWorkEventHandler(_walkWorker_DoWork);
            _walkWorker.WorkerSupportsCancellation = true;
            _walkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_walkWorker_RunWorkerCompleted);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Walk do work event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _walkWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // set the initial head yaw step and head pitch step amount to default but update it as it gets near to the mark
            var headYawStepAmount = FindNaoMark.DEFAULT_HEAD_YAW_STEP_AMOUNT;
            var headPitchStepAmount = FindNaoMark.DEFAULT_HEAD_PITCH_STEP_AMOUNT;

            while (!_walkWorker.CancellationPending)
            {
                // clear previous detected mark
                _detectedMark = null;

                // find mark
                FindNaoMark findMark = new FindNaoMark(this.MarkInfo.MarkID, this._robotConnectionHelper, headPitchStepAmount: headPitchStepAmount, headYawStepAmount: headYawStepAmount, startLookDirection: this.StartLookDirection);
                findMark.NaoMarkDetected += new EventHandler<FindMarkEventArgs>(findMark_NaoMarkDetected);
                findMark.StartSearching();

                // wait until mark is detected
                while (!_walkWorker.CancellationPending)
                {
                    lock (_lockObject)
                    {
                        if (_detectedMark != null)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(500);
                }

                // stop searching for mark
                findMark.StopSearching();
                findMark.Dispose();

                if (!_walkWorker.CancellationPending)
                {
                    // get camera transform
                    Transform cameraTransform = null;
                    using (var motionProxy = _robotConnectionHelper.GetProxy<MotionProxy>())
                    {
                        cameraTransform = new Transform(motionProxy.getTransform("CameraTop", 2, true) as List<float>);

                        // get mark position
                        var markPosition = LandMarkHelper.Instance.GetLandMarkPosition(_detectedMark, this.MarkInfo.MarkSize, cameraTransform);

                        // align with mark
                        AlignWithMark(motionProxy, _detectedMark);

                        // stop if close to mark
                        if (markPosition.x - 0.01 <= this.MarkInfo.StopDistance)
                        {
                            break;
                        }
                        else
                        {
                            if (markPosition.x <= 0.4)
                            {
                                headPitchStepAmount = FindNaoMark.DEFAULT_HEAD_PITCH_STEP_AMOUNT / 4;
                                headYawStepAmount = FindNaoMark.DEFAULT_HEAD_YAW_STEP_AMOUNT / 4;
                            }
                            else if (markPosition.x <= 0.6)
                            {
                                headPitchStepAmount = FindNaoMark.DEFAULT_HEAD_PITCH_STEP_AMOUNT / 2;
                                headYawStepAmount = FindNaoMark.DEFAULT_HEAD_YAW_STEP_AMOUNT / 2;
                            }
                            else if (markPosition.x <= 0.8)
                            {
                                headPitchStepAmount = 3 * FindNaoMark.DEFAULT_HEAD_PITCH_STEP_AMOUNT / 4;
                                headYawStepAmount = 3 * FindNaoMark.DEFAULT_HEAD_YAW_STEP_AMOUNT / 4;
                            }
                            // advance on x
                            var advance = markPosition.x - this.MarkInfo.AdvanceDistance <= this.MarkInfo.StopDistance ? markPosition.x - this.MarkInfo.StopDistance : this.MarkInfo.AdvanceDistance;
                            motionProxy.moveTo(advance, 0f, 0f);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Aligns the robot with the landmark
        /// </summary>
        /// <param name="motionProxy"></param>
        /// <param name="markInfo"></param>
        private void AlignWithMark(MotionProxy motionProxy, ArrayList markInfo)
        {
            // get mark shape infor
            var shapeInfo = LandMarkHelper.Instance.GetShapeInfo(markInfo);

            // get current yaw position
            var currentYawPosition = motionProxy.getAngles("HeadYaw", true);
            if (!currentYawPosition.IsNullOrEmpty() && shapeInfo[1] != null)
            {
                // move pitch to landmark
                MotionHelper.MoveHead(motionProxy, shapeInfo[2] as float?, null, false);
                // calculate robot rotation
                var rotation = currentYawPosition[0] + (float)shapeInfo[1];
                motionProxy.moveTo(0f, 0f, rotation);
                // reset yaw
                MotionHelper.MoveHead(motionProxy, null, 0f, true);
            }
        }

        /// <summary>
        /// NaoMark detected event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void findMark_NaoMarkDetected(object sender, FindMarkEventArgs e)
        {
            lock (_lockObject)
            {
                _detectedMark = e.MarkData;
            }
        }

        /// <summary>
        /// Walk completed event hanlder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _walkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.WalkCompleted != null)
            {
                this.WalkCompleted(this, null);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts robots execution
        /// </summary>
        public void StartWalking()
        {
            _walkWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Stops the thread until walking is completed
        /// </summary>
        public void WaitUntilWalkingIsComplete()
        {
            while (_walkWorker.IsBusy)
            {
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Stops robot execution
        /// </summary>
        public void StopWalking()
        {
            _walkWorker.CancelAsync();
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            if (_walkWorker.IsBusy)
            {
                _walkWorker.CancelAsync();
                _walkWorker.Dispose();
            }
        }
        #endregion
    }
}

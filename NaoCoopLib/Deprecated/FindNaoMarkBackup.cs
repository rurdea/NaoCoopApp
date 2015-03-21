using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aldebaran.Proxies;
using System.ComponentModel;
using System.Collections;
using NaoCoopLib.Helpers;
using System.Diagnostics;
using System.Threading;
using NaoCoopLib.Enums;

namespace NaoCoopLib.Executers
{
    #region FindMarkEventArgs
    public class FindMarkEventArgs : EventArgs
    {
        #region Properties
        public int MarkID
        {
            get;
            private set;
        }

        public ArrayList MarkData
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        public FindMarkEventArgs(int markID, ArrayList markData)
        {
            this.MarkID = markID;
            this.MarkData = markData;
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// TO DO: declare a parameter for rotation
    /// add property for looking on the floor or not
    /// </summary>
    public class FindNaoMarkBackup : IDisposable
    {
        #region Constants
        public const float DEFAULT_ROTATION_AMOUNT = (float)Math.PI / 2;
        public const float DEFAULT_MAX_ROTATION = (float)(Math.PI * 2);
        public const float DEFAULT_HEAD_PITCH_STEP_AMOUNT = 0.17f;
        public const float DEFAULT_HEAD_YAW_STEP_AMOUNT = (float)Math.PI / 4;
        public const float DEFAULT_MAX_HEAD_YAW = ((float)Math.PI / 4) + ((float)Math.PI / 8);
        private const string SUBSCRIBER_NAME = "FindNaoMark_Subscriber";
        #endregion


        #region Members
        BackgroundWorker _findMarkWorker;
        NaoConnectionHelper _connection;
        #endregion

        #region Properties
        public int MarkID
        {
            get;
            private set;
        }

        public float RotationAmount
        {
            get;
            set;
        }

        public float MaxRotation
        {
            get;
            set;
        }

        public float HeadPitchStepAmount
        {
            get;
            set;
        }

        public float HeadYawStepAmount
        {
            get;
            set;
        }

        public float MaxHeadYaw
        {
            get;
            set;
        }

        public LookDirection StartLookDirection
        {
            get;
            set;
        }
        #endregion

        #region Events
        public event EventHandler<FindMarkEventArgs> NaoMarkDetected;
        #endregion

        #region Constructors
        public FindNaoMarkBackup(int markID, string robotIP, int robotPort, 
                           float rotationAmount = DEFAULT_ROTATION_AMOUNT, 
                           float maxRotation = DEFAULT_MAX_ROTATION,
                           float headPitchStepAmount = DEFAULT_HEAD_PITCH_STEP_AMOUNT,
                           float headYawStepAmount = DEFAULT_HEAD_YAW_STEP_AMOUNT,
                           float maxHeadYaw = DEFAULT_MAX_HEAD_YAW,
                           LookDirection startLookDirection = LookDirection.Left)
            : this (markID, new NaoConnectionHelper(robotIP, robotPort), rotationAmount, maxRotation, headPitchStepAmount, headYawStepAmount, maxHeadYaw, startLookDirection)
        {
        }

        public FindNaoMarkBackup(int markID, NaoConnectionHelper connection,
                           float rotationAmount = DEFAULT_ROTATION_AMOUNT,
                           float maxRotation = DEFAULT_MAX_ROTATION,
                           float headPitchStepAmount = DEFAULT_HEAD_PITCH_STEP_AMOUNT,
                           float headYawStepAmount = DEFAULT_HEAD_YAW_STEP_AMOUNT,
                           float maxHeadYaw = DEFAULT_MAX_HEAD_YAW,
                           LookDirection startLookDirection = LookDirection.Left)
        {
            if (!connection.TestConnection(string.Empty))
            {
                throw new ArgumentException("Could not connect to the robot!");
            }

            this._connection = connection;
            this.RotationAmount = rotationAmount;
            this.MaxRotation = maxRotation;
            this.HeadPitchStepAmount = headPitchStepAmount;
            this.MarkID = markID;
            this.HeadYawStepAmount = headYawStepAmount;
            this.MaxHeadYaw = maxHeadYaw;
            this.StartLookDirection = startLookDirection;

            // initialize background worker
            _findMarkWorker = new BackgroundWorker();
            _findMarkWorker.DoWork += new DoWorkEventHandler(_findMarkWorker_DoWork);
            _findMarkWorker.WorkerSupportsCancellation = true;
            _findMarkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_findMarkWorker_RunWorkerCompleted);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Worked completed event handler.
        /// Will fire NaoMarkDetected event in case the robot found one of the mark he is looking for.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _findMarkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null && this.NaoMarkDetected!=null)
            {
                var result = e.Result as KeyValuePair<int, ArrayList>?;
                if (result != null)
                {
                    this.NaoMarkDetected(this, new FindMarkEventArgs(result.Value.Key, result.Value.Value));
                }
            }
        }

        /// <summary>
        /// Do work event handler.
        /// Turns until the robot sees a mark.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _findMarkWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // reset the actual rotation
            var actualRotation = 0f;
            
            //init robot
            InitRobotPosition();

            // create a LandMarkDetectionProxy to start looking for marks
            using (var landMarkDetectionProxy = _connection.GetProxy<LandMarkDetectionProxy>())
            {
                landMarkDetectionProxy.subscribe(SUBSCRIBER_NAME);

                // create a MemoryProxy to read landmark information
                using (var memProxy = _connection.GetProxy<MemoryProxy>())
                {
                    // use one motion proxy
                    using (var motionProxy = _connection.GetProxy<MotionProxy>())
                    {
                        do
                        {// turn the robot 360 degrees
                            // reset the yaw to 0
                            MotionHelper.MoveHead(motionProxy, null, 0f, true);
                            // initialize the look direction
                            var startLookDirection = this.StartLookDirection;
                            var currentLookDirection = this.StartLookDirection;
                            do
                            {// make the robot look left and right
                                // reset head pitch position to 0
                                MotionHelper.MoveHead(motionProxy, 0f, null, true);
                                do
                                { // make the robot look down
                                    var landMarks = this.TryGetLandMark(memProxy);
                                    if (landMarks != null && landMarks.ContainsKey(MarkID))
                                    {
                                        // align the robot to the landmark
                                        AlignWithMark(motionProxy, landMarks[MarkID]);

                                        // reload the mark information from memory because the robot moved
                                        landMarks = this.TryGetLandMark(memProxy);
                                        // should not be empty but just make sure
                                        if (landMarks != null && landMarks.ContainsKey(MarkID))
                                        {
                                            // save the result
                                            e.Result = new KeyValuePair<int, ArrayList>(MarkID, landMarks[MarkID]);
                                        }

                                    }
                                }
                                while (e.Result == null && !_findMarkWorker.CancellationPending && LookDown(motionProxy));
                            }
                            while (e.Result == null && !_findMarkWorker.CancellationPending &&
                                   LookAround(motionProxy, startLookDirection, ref currentLookDirection));
                        }
                        while (e.Result == null && !_findMarkWorker.CancellationPending && Rotate(motionProxy, ref actualRotation));
                    }
                }

                landMarkDetectionProxy.unsubscribe(SUBSCRIBER_NAME);
            }
            
        }

        private void InitRobotPosition()
        {
            // init robot
            using (var poseProxy = _connection.GetProxy<RobotPoseProxy>())
            {
                var curPositionAndTime = poseProxy.getActualPoseAndTime() as ArrayList;
                string curPositionStr = curPositionAndTime[0].ToString();
                NAOPositions curPosition;
                if (Enum.TryParse<NAOPositions>(curPositionStr, true, out curPosition))
                {
                    if (curPosition != NAOPositions.Stand)
                    {
                        using (var postureProxy = _connection.GetProxy<RobotPostureProxy>())
                        {
                            var ret = postureProxy.goToPosture(NAOPostures.Stand.ToString(), 0.5f);
                            if (ret == false)
                            {
                                throw new Exception("Could not initialize robot posture.");
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Method that reads the LandmarkDetected memory and extracts the landmarks info into a dictionary
        /// </summary>
        /// <param name="memProxy"></param>
        /// <returns></returns>
        private Dictionary<int, ArrayList> TryGetLandMark(MemoryProxy memProxy)
        {
            Dictionary<int, ArrayList> ret = null;
            try
            {
                // get the LandmarkDetected memory
                var landMarkMemory = memProxy.getData("LandmarkDetected") as ArrayList;

                if (landMarkMemory != null)
                {
                    var landMarks = LandMarkHelper.Instance.GetLandMarksInfo(landMarkMemory) as ArrayList;
                    if (landMarks!=null)
                    {
                        ret = new Dictionary<int, ArrayList>();
                        foreach (var landMark in landMarks)
                        {
                            var landMarkArr = landMark as ArrayList;
                            if (landMarkArr != null)
                            {
                                int? markId = LandMarkHelper.Instance.GetMarkID(landMarkArr) as int?;
                                if (markId.HasValue)
                                {
                                    ret.Add(markId.Value, landMarkArr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Coult get marks. " + ex.ToString());
            }
            return ret;
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

        public enum LookDirection
        {
            Left,
            Right
        }

        /// <summary>
        /// Makes the robot look left and right
        /// </summary>
        /// <param name="motionProxy"></param>
        /// <returns></returns>
        private bool LookAround(MotionProxy motionProxy,LookDirection startLookDirection, ref LookDirection currentLookDirection)
        {
            // get initial head yaw position
            var initialHeadYawPositionArray = motionProxy.getAngles("HeadYaw", true);

            if (initialHeadYawPositionArray.IsNullOrEmpty())
            {
                // should not happen
                return false;
            }

            var initialHeadYawPosition = initialHeadYawPositionArray[0];
            float maxHeadYaw, step;
            float marginOfError = 0.01f;
            var maxReached = false;
            var directionChanged = currentLookDirection != startLookDirection;

            if (currentLookDirection == LookDirection.Left)
            {
                maxHeadYaw = this.MaxHeadYaw;
                step = this.HeadYawStepAmount;
                // check if it's the last movement and ajust the step if it is
                if (step + initialHeadYawPosition >= maxHeadYaw)
                {
                    step = maxHeadYaw - initialHeadYawPosition;
                }
                // if it reached the maximum value we need to change the direction and update the step
                if (step - marginOfError <= 0)
                {
                    maxReached = true;
                    step = -this.HeadYawStepAmount;
                }
            }
            else
            {
                // if it's the right direction we need to have negative values for max and step angles
                maxHeadYaw = -this.MaxHeadYaw;
                step = -this.HeadYawStepAmount;
                // check if it's the last movement and ajust the step if it is
                if (step + initialHeadYawPosition <= maxHeadYaw)
                {
                    step = maxHeadYaw - initialHeadYawPosition;
                }
                // if it reached the maximum value we need to change the direction and update the step
                if (step + marginOfError >= 0)
                {
                    maxReached = true;
                    step = this.HeadYawStepAmount;
                }
            }

            if (maxReached && directionChanged)
            {
                // no more movement allowed
                // reset head yaw position to 0
                MotionHelper.MoveHead(motionProxy, null, 0f, true);
                return false;
            }
            else
            {
                MotionHelper.MoveHead(motionProxy, null, step, maxReached);
                if (maxReached)
                {
                    currentLookDirection = currentLookDirection == LookDirection.Left ? LookDirection.Right : LookDirection.Left;
                }
                return true;
            }
        }


        private bool LookDown(MotionProxy motionProxy)
        {
            // get initial head pitch position
            var initialHeadPitchPosition = motionProxy.getAngles("HeadPitch", true);
            
            MotionHelper.MoveHead(motionProxy, this.HeadPitchStepAmount, null, false);
            
            // get current pitch position
            var currentHeadPitchPosition = motionProxy.getAngles("HeadPitch", true);

            // return true if the current position changed
            if (!initialHeadPitchPosition.IsNullOrEmpty() && !currentHeadPitchPosition.IsNullOrEmpty() &&
                    initialHeadPitchPosition[0] != currentHeadPitchPosition[0])
            {
                return true;
            }
            else
            {
                // reset pitch position
                MotionHelper.MoveHead(motionProxy, 0f, null, true);
                return false;
            }
        }

        private bool Rotate(MotionProxy motionProxy, ref float actualRotation)
        {
            if (actualRotation > this.MaxRotation)
            {
                return false;
            }

            motionProxy.moveInit();
            motionProxy.moveTo(0f, 0f, this.RotationAmount);
            actualRotation += this.RotationAmount;

            return true;
        }
        #endregion

        #region Public Methods
        public void StartSearching()
        {
            _findMarkWorker.RunWorkerAsync();
        }

        public void StopSearching()
        {
            _findMarkWorker.CancelAsync();
        }

        public void Dispose()
        {
            if (_findMarkWorker.IsBusy)
            {
                _findMarkWorker.CancelAsync();
            }
            _findMarkWorker.Dispose();

            try
            {
                // make sure we unsubribe to the landmark detection
                using (var speechRecognitionProxy = _connection.GetProxy<SpeechRecognitionProxy>())
                {
                    speechRecognitionProxy.unsubscribe(SUBSCRIBER_NAME);
                }
            }
            catch { }
        }
        #endregion
    }
}

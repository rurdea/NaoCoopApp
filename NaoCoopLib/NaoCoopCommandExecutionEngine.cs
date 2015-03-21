using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aldebaran.Proxies;
using NaoCoopLib.Enums;
using NaoCoopLib.Executers;
using NaoCoopLib.Helpers;
using NaoCoopLib.Types;

namespace NaoCoopLib
{
    /// <summary>
    /// Internal class for executing commands
    /// </summary>
    internal class NaoCoopCommandExecutionEngine : IDisposable
    {
        #region Members
        RobotSynchronization _robotSynch;
        WalkToNaoMark _walkToNaoMark;
        ObjectHandlingExecuter _objectHandlingExecuter;
        NaoConnectionHelper _connection;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection">Connection object</param>
        public NaoCoopCommandExecutionEngine(NaoConnectionHelper connection)
        {
            this._connection = connection;
            this._robotSynch = new RobotSynchronization(connection);
            this._objectHandlingExecuter = new ObjectHandlingExecuter(connection);
        }

        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Executes the specified command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandParameters"></param>
        public void Execute(NaoCommand command, Dictionary<string, string> commandParameters)
        {
            switch (command)
            {
                case NaoCommand.GoToGrabLocation:
                    GoToGrabLocation();
                    break;
                case NaoCommand.GoToLiftPosition:
                    break;
                case NaoCommand.SynchRobot:
                    break;
                case NaoCommand.WalkToCheckpoint:
                    break;
                case NaoCommand.WalkWithObject:
                    break;
            }
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Pauses the specified command
        /// </summary>
        /// <param name="command"></param>
        public void Pause(NaoCommand command)
        {
            switch (command)
            {
                case NaoCommand.GoToGrabLocation:
                    if (this._walkToNaoMark != null)
                    {
                        this._walkToNaoMark.StopWalking();
                    }
                    break;
                case NaoCommand.WalkWithObject:
                    _objectHandlingExecuter.StopWalking();
                    break;
                case NaoCommand.SynchRobot:
                    this._robotSynch.CancelSynchronization();
                    break;
            }
        }

        /// <summary>
        /// Disposes the specified command
        /// </summary>
        public void Dispose()
        {
            if (_robotSynch != null)
            {
                _robotSynch.Dispose();
            }
            _connection = null;
            if (_objectHandlingExecuter != null)
            {
                _objectHandlingExecuter.Dispose();
            }
            if (_walkToNaoMark != null)
            {
                _walkToNaoMark.Dispose();
            }
        }
        #endregion

        #region Private
        private void GoToGrabLocation()
        {
            GrabLocationInfo locationInfo = new GrabLocationInfo(Enums.GrabLocation.A);
            this._objectHandlingExecuter.GoToGrabLocation(locationInfo);
        }

        private void WalkToMark()
        {
            WalkToLandMarkInfo walkToMark = new WalkToLandMarkInfo(0, 0);
            if (this._walkToNaoMark != null)
            {
                this._walkToNaoMark.StopWalking();
                this._walkToNaoMark.Dispose();
            }
            this._walkToNaoMark = new WalkToNaoMark(this._connection, walkToMark);
            this._walkToNaoMark.StartWalking();
            this._walkToNaoMark.WaitUntilWalkingIsComplete();
            this.Say("Checkpoint");
        }

        private void GoToLiftPosition()
        {
            this._objectHandlingExecuter.GoToLiftPosition();
        }

        private void SynchronizeRobot()
        {
            this._robotSynch.SynchronizeRobot();
        }

        private void WalkWithObject()
        {
            this._objectHandlingExecuter.WalkWithObject();
        }

        private void LiftObject()
        {
            if (this._robotSynch.SynchronizeRobot())
            {
                this._objectHandlingExecuter.LiftObject();
            }
        }

        private void Say(string word)
        {
            using (var tts = _connection.GetProxy<TextToSpeechProxy>())
            {
                tts.say(word);
            }
        }
        #endregion

        #endregion

        
    }
}

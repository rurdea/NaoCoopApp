using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using bbv.Common.StateMachine;
using log4net;
using NaoCoopLib.Enums;
using NaoCoopLib.Executers;
using NaoCoopLib.Helpers;
using NaoCoopLib.Types;
using Aldebaran.Proxies;

namespace NaoCoopLib.Deprecated
{
    public class NaoCoopRobot : IDisposable
    {
        #region Members
        RobotSynchronization _robotSynch;
        WalkToNaoMark _walkToNaoMark;
        ObjectHandlingExecuter _objectHandlingExecuter;
        NaoConnectionHelper _connection;
        ActiveStateMachine<NaoState, NaoCommand> _stateMachine;
        ILog _logger;
        WalkToLandMarkInfo _markInfo;
        GrabLocationInfo _grabLocationInfo;
        bool _forcedStop = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="ip">The robot ip</param>
        /// <param name="port">The robot port</param>
        /// <param name="markInfo">The checkpoint's mark information</param>
        /// <param name="grabLocation">The grab location information</param>
        /// <param name="logger">The logger</param>
        public NaoCoopRobot(string ip, int port, WalkToLandMarkInfo markInfo, GrabLocationInfo grabLocation, ILog logger)
        {
            this._logger = logger;
            this._connection = new NaoConnectionHelper(ip, port);
            this._markInfo = markInfo;
            this._grabLocationInfo = grabLocation;
            
            /*
            if (this._connection == null || !this._connection.TestConnection())
            {
                throw new Exception("Could not connect to the Robot.");
            }
            */
            this._objectHandlingExecuter = new ObjectHandlingExecuter(_connection);
            this._robotSynch = new RobotSynchronization(_connection);
            this.Initialize();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current state
        /// </summary>
        public NaoState CurrentState
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Starts the robot
        /// </summary>
        public void Start()
        {
            this.Start(NaoState.Initialized);
        }

        /// <summary>
        /// Starts the robot at the specified state
        /// </summary>
        /// <param name="startState"></param>
        public void Start(NaoState startState)
        {
            this.CurrentState = startState;
            this.Resume();
        }

        /// <summary>
        /// Pauses the robot
        /// </summary>
        public void Pause()
        {
            switch (CurrentState)
            {
                case NaoState.Initialized:
                    if (this._walkToNaoMark != null)
                    {
                        this._walkToNaoMark.StopWalking();
                    }
                    goto StopStateMachine;
                case NaoState.InWalkPosition:
                    _objectHandlingExecuter.StopWalking();
                    goto StopStateMachine;
                case NaoState.InLiftPosition:
                    this._robotSynch.CancelSynchronization();
                    goto StopStateMachine;
            }

        StopStateMachine:
            this._forcedStop = true;
            this._stateMachine.Stop();
        }

        /// <summary>
        /// Resumes the robot
        /// </summary>
        public void Resume()
        {
            this._forcedStop = false;
            this._stateMachine.Initialize(this.CurrentState);
            this._stateMachine.Start();

            // get the commands to be executed
            var commands = this.GetCommands(this.CurrentState);
            foreach (var command in commands)
            {
                _stateMachine.Fire(command);
            }
        }

        /// <summary>
        /// Stops the robot
        /// </summary>
        public void Stop()
        {
            this.Pause();
            this.CurrentState = NaoState.Terminated;
        }

        public void Dispose()
        {
        }
        #endregion

        #region Private
        private void Initialize()
        {
            _stateMachine = new ActiveStateMachine<NaoState, NaoCommand>();

            _stateMachine.In(NaoState.Initialized)
                         .On(NaoCommand.WalkToCheckpoint).Goto(NaoState.AtCheckpoint).Execute(WalkToMark)
                         .On(NaoCommand.Stop).Goto(NaoState.Initialized);
            _stateMachine.In(NaoState.AtCheckpoint)
                         .On(NaoCommand.GoToGrabLocation).Goto(NaoState.AtGrabLocation).Execute(GoToGrabLocation);
            _stateMachine.In(NaoState.AtGrabLocation)
                         .On(NaoCommand.GoToLiftPosition).Goto(NaoState.InLiftPosition).Execute(GoToLiftPosition);
            _stateMachine.In(NaoState.InLiftPosition)
                         .On(NaoCommand.LiftObject).Goto(NaoState.InWalkPosition).Execute(LiftObject)
                         .On(NaoCommand.Stop).Goto(NaoState.InLiftPosition).Execute(Stop);
            _stateMachine.In(NaoState.InWalkPosition)
                         .On(NaoCommand.WalkWithObject).Goto(NaoState.Terminated).Execute(WalkWithObject)
                         .On(NaoCommand.Stop).Goto(NaoState.InWalkPosition);

            _stateMachine.TransitionBegin += _stateMachine_TransitionBegin;
            _stateMachine.TransitionCompleted += _stateMachine_TransitionCompleted;
            _stateMachine.TransitionDeclined += _stateMachine_TransitionDeclined;
        }

        #region StateMachine Event Handlers
        void _stateMachine_TransitionDeclined(object sender, TransitionEventArgs<NaoState, NaoCommand> e)
        {
            this._logger.WarnFormat("Transition declined! Current state: {0}, command: {1}.", e.StateId.ToString(), e.EventId.ToString());
        }

        void _stateMachine_TransitionCompleted(object sender, TransitionCompletedEventArgs<NaoState, NaoCommand> e)
        {
            this._logger.InfoFormat("Transition completed. Current state: {0}, new state: {1}, command: {2}.", e.StateId.ToString(), e.NewStateId.ToString(), e.EventId.ToString());
            if (!this._forcedStop)
            {
                this.CurrentState = e.NewStateId;
            }
        }

        void _stateMachine_TransitionBegin(object sender, TransitionEventArgs<NaoState, NaoCommand> e)
        {
            this._logger.InfoFormat("Transition Begin. Current state: {0}, command: {1}.", e.StateId.ToString(), e.EventId.ToString());
        }
        #endregion

        #region Command Executers
        private void GoToGrabLocation()
        {
            this._objectHandlingExecuter.GoToGrabLocation(this._grabLocationInfo);
        }

        private void WalkToMark()
        {
            if (this._walkToNaoMark != null)
            {
                this._walkToNaoMark.StopWalking();
                this._walkToNaoMark.Dispose();
            }
            this._walkToNaoMark = new WalkToNaoMark(this._connection, this._markInfo);
            this._walkToNaoMark.StartWalking();
            this._walkToNaoMark.WaitUntilWalkingIsComplete();
            this.Say("Checkpoint");
        }

        private void GoToLiftPosition()
        {
            this._objectHandlingExecuter.GoToLiftPosition();
        }

        private void WalkWithObject()
        {
            //_robotSynch.SynchronizeRobot();
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

        #region Protected
        protected List<NaoCommand> GetCommands(NaoState state)
        {
            var commands = new List<NaoCommand>();

            if (state <= NaoState.Initialized)
            {
                commands.Add(NaoCommand.WalkToCheckpoint);
            }
            if (state <= NaoState.AtCheckpoint)
            {
                commands.Add(NaoCommand.GoToGrabLocation);
            }
            if (state <= NaoState.AtGrabLocation)
            {
                commands.Add(NaoCommand.GoToLiftPosition);
            }
            if (state <= NaoState.InLiftPosition)
            {
                commands.Add(NaoCommand.LiftObject);
            }
            if (state <= NaoState.InWalkPosition)
            {
                commands.Add(NaoCommand.WalkWithObject);
            }

            return commands;
        }

        
        #endregion
        #endregion
    }
}

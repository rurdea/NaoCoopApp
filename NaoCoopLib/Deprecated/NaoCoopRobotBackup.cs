using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaoCoopLib.Enums;
using NaoCoopLib.Executers;
using NaoCoopLib.Helpers;

namespace NaoCoopLib
{
    public class NaoCoopRobotBackup
    {
        #region Members
        SpeechRecognition _speechRecognition;
        WalkToNaoMark _walkToNaoMark;
        NaoConnectionHelper _connection;
        Dictionary<NaoStateTransition, NaoState> _transitions;
        #endregion

        #region Constructor
        public NaoCoopRobotBackup(string ip, int port)
        {
            this._connection = new NaoConnectionHelper(ip, port);
            if (this._connection == null || !this._connection.TestConnection())
            {
                throw new Exception("Could not connect to the Robot.");
            }
        }
        #endregion

        #region Properties
        public NaoState CurrentState
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        #region Public
        public void Initialize()
        {
            this.CurrentState = NaoState.Initialized;
            this._transitions = new Dictionary<NaoStateTransition, NaoState>
            {
                { new NaoStateTransition(NaoState.Initialized, NaoCommand.WalkToCheckpoint), NaoState.AtCheckpoint },
                { new NaoStateTransition(NaoState.Initialized, NaoCommand.Stop), NaoState.Initialized },
                { new NaoStateTransition(NaoState.AtCheckpoint, NaoCommand.GoToGrabLocation), NaoState.AtGrabLocation },
                { new NaoStateTransition(NaoState.AtGrabLocation, NaoCommand.GoToLiftPosition), NaoState.InLiftPosition },
                { new NaoStateTransition(NaoState.InLiftPosition, NaoCommand.LiftObject), NaoState.InWalkPosition },
                { new NaoStateTransition(NaoState.InWalkPosition, NaoCommand.WalkWithObject), NaoState.Terminated },
                { new NaoStateTransition(NaoState.InWalkPosition, NaoCommand.Stop), NaoState.InWalkPosition }
            };
        }

        public NaoState GetNext(NaoCommand command)
        {
            NaoStateTransition transition = new NaoStateTransition(CurrentState, command);
            NaoState nextState;
            if (!this._transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + this.CurrentState + " -> " + command);
            return nextState;
        }

        public NaoState MoveNext(NaoCommand command)
        {
            this.CurrentState = GetNext(command);
            return CurrentState;
        }
        #endregion

        #region Private
        protected void ChangeState(NaoState newState)
        {

        }

        protected void ExecuteCommand(NaoCommand command)
        {

        }
        #endregion
        #endregion


    }
}

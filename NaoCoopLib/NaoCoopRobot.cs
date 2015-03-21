using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aldebaran.Proxies;
using bbv.Common.StateMachine;
using log4net;
using NaoCoopLib.Enums;
using NaoCoopLib.Executers;
using NaoCoopLib.Helpers;
using NaoCoopLib.Types;

namespace NaoCoopLib
{
    /// <summary>
    /// Robot status enum
    /// </summary>
    public enum RobotStatus
    {
        Idle,
        Running,
        Paused,
        Finished
    }

    /// <summary>
    /// Class containing all robot functionality
    /// </summary>
    public class NaoCoopRobot : IDisposable
    {
        #region Members
        NaoConnectionHelper _connection;
        ActiveStateMachine<NaoState, NaoCommand> _stateMachine;
        ILog _logger;
        bool _forcedStop = false;
        NaoCoopCommandExecutionEngine _commandExecutionEngine;
        #endregion

        #region Events
        /// <summary>
        /// Triggered when the robot state changes
        /// </summary>
        public event EventHandler RobotStateChanged;

        /// <summary>
        /// Triggered when the robot executing command changes
        /// </summary>
        public event EventHandler RobotExecutingCommandChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the robot id
        /// </summary>
        public Guid ID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the robot states and the commands associated with each state
        /// </summary>
        public OrderedDictionary<NaoState, OrderedDictionary<NaoCommand, Dictionary<string, string>>> StateCommands
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current state
        /// </summary>
        public NaoState CurrentState
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current executing command. If null the robot is Idle.
        /// </summary>
        public NaoCommand? CurrentExecutingCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the robot status
        /// </summary>
        public RobotStatus Status
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="ip">the ip of the robot</param>
        /// <param name="port">the port of the robot</param>
        /// <param name="logger">applicatoin logger</param>
        /// <param name="id">robot id</param>
        public NaoCoopRobot(string ip, int port, ILog logger, Guid? id = null)
        {
            _logger = logger;
            _connection = new NaoConnectionHelper(ip, port);
            _commandExecutionEngine = new NaoCoopCommandExecutionEngine(_connection);
             /*
            if (this._connection == null || !this._connection.TestConnection())
            {
                throw new Exception("Could not connect to the Robot.");
            }
            */
            this.StateCommands = new OrderedDictionary<NaoState, OrderedDictionary<NaoCommand, Dictionary<string, string>>>();

            if (id == null)
            {
                id = Guid.NewGuid();
            }
            this.ID = id.Value;
            this.Status = RobotStatus.Idle;
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Starts the robot
        /// </summary>
        public void Start()
        {
            this.Start(StateCommands.GetItem(0).Key);
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
            if (CurrentExecutingCommand.HasValue)
            {
                _commandExecutionEngine.Pause(CurrentExecutingCommand.Value);
            }

            this._forcedStop = true;
            this.StopStateMachine();
            ChangeRobotStatus(RobotStatus.Paused);
        }

        /// <summary>
        /// Resumes the robot
        /// </summary>
        public void Resume()
        {
            this._forcedStop = false;
            this.StopStateMachine();
            this.InitializeStateMachine();
            this._stateMachine.Initialize(this.CurrentState);
            this._stateMachine.Start();
            ChangeRobotStatus(RobotStatus.Running);

            // get the commands to be executed
            var commands = this.GetCommands(this.CurrentState);
            foreach (var command in commands)
            {
                _stateMachine.Fire(command, new KeyValuePair<NaoCommand, Dictionary<string, string>>(command, new Dictionary<string, string>()));
            }
        }

        /// <summary>
        /// Stops the robot
        /// </summary>
        public void Stop()
        {
            this.Pause();
            this.CurrentState = NaoState.Terminated;
            // raise event
            if (this.RobotStateChanged != null)
            {
                this.RobotStateChanged(this, null);
            }
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            // dispose the robot
            _connection = null;
            _commandExecutionEngine.Dispose();
            _stateMachine.Stop();
            _stateMachine = null;
        }
        #endregion

        #region Protected
        /// <summary>
        /// Gets the commands to be executed based on the specified state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected List<NaoCommand> GetCommands(NaoState state)
        {
            var commands = new List<NaoCommand>();

            var index = StateCommands.IndexOf(state);
            if (index != -1)
            {
                for (int i = index; i < StateCommands.Count; i++)
                {
                    commands.AddRange(StateCommands[i].Keys);
                }
            }

            return commands;
        }

        /// <summary>
        /// Changes the robot status
        /// </summary>
        /// <param name="newStatus"></param>
        protected void ChangeRobotStatus(RobotStatus newStatus)
        {
            this.Status = newStatus;
            // raise event
            if (this.RobotStateChanged != null)
            {
                this.RobotStateChanged(this, null);
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// Initializes the state machine
        /// </summary>
        private void InitializeStateMachine()
        {
            _stateMachine = new ActiveStateMachine<NaoState, NaoCommand>();

            for (int i = 0; i < this.StateCommands.Count - 1; i++)
            {
                var state = this.StateCommands.GetItem(i);
                var nextState = this.StateCommands.GetItem(i + 1);

                if (state.Value.Count > 1)
                {// if a state contains multiple tasks then do not pass to the next state unless it's the last task in the list
                    for (int j = 0; j < state.Value.Count - 1; j++)
                    {
                        var command = state.Value.GetItem(j);
                        _stateMachine.In(state.Key).On(command.Key).Goto(state.Key).Execute<KeyValuePair<NaoCommand, Dictionary<string, string>>>(ExecuteCommand);
                    }
                }
                if (state.Value.Count > 0)
                {// move to the next state for the last task
                    var command = state.Value.GetItem(state.Value.Count - 1);
                    _stateMachine.In(state.Key).On(command.Key).Goto(nextState.Key).Execute<KeyValuePair<NaoCommand, Dictionary<string, string>>>(ExecuteCommand);
                }
            }

            _stateMachine.TransitionBegin += _stateMachine_TransitionBegin;
            _stateMachine.TransitionCompleted += _stateMachine_TransitionCompleted;
            _stateMachine.TransitionDeclined += _stateMachine_TransitionDeclined;
        }

        /// <summary>
        /// Stops the state machine
        /// </summary>
        private void StopStateMachine()
        {
            if (_stateMachine != null && _stateMachine.IsRunning)
            {
                _stateMachine.Stop();
            }
        }

        /// <summary>
        /// Executed the specified command with the specified settings
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteCommand(KeyValuePair<NaoCommand, Dictionary<string, string>> obj)
        {
            // execute method
            _commandExecutionEngine.Execute(obj.Key, obj.Value);
        }

        #region StateMachine Event Handlers
        /// <summary>
        /// State machine transaction declined event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _stateMachine_TransitionDeclined(object sender, TransitionEventArgs<NaoState, NaoCommand> e)
        {
            this._logger.WarnFormat("[Robot: {0}] - Transition declined! Current state: {1}, command: {2}.", _connection.IP, e.StateId.ToString(), e.EventId.ToString());
        }

        /// <summary>
        /// State machine transaction completed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _stateMachine_TransitionCompleted(object sender, TransitionCompletedEventArgs<NaoState, NaoCommand> e)
        {
            this._logger.InfoFormat("[Robot: {0}] - Transition completed. Current state: {1}, new state: {2}, command: {3}.", _connection.IP, e.StateId.ToString(), e.NewStateId.ToString(), e.EventId.ToString());
            if (!this._forcedStop)
            {
                this.CurrentState = e.NewStateId;
            }
            this.CurrentExecutingCommand = null;

            // check if the last state and set robot status to finished
            if (e.NewStateId == this.StateCommands.GetItem(this.StateCommands.Count - 1).Key)
            {
                this.Status = RobotStatus.Finished;
            }

            // raise event
            if (this.RobotStateChanged != null)
            {
                this.RobotStateChanged(this, null);
            }
        }

        /// <summary>
        /// State machine transaction begin handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _stateMachine_TransitionBegin(object sender, TransitionEventArgs<NaoState, NaoCommand> e)
        {
            this._logger.InfoFormat("[Robot: {0}] - Transition Begin. Current state: {1}, command: {2}.", _connection.IP, e.StateId.ToString(), e.EventId.ToString());
            this.CurrentExecutingCommand = e.EventId;

            // raise command changed event
            if (this.RobotExecutingCommandChanged != null)
            {
                this.RobotExecutingCommandChanged(this, null);
            }
        }
        #endregion
        #endregion

        #endregion
    }
}

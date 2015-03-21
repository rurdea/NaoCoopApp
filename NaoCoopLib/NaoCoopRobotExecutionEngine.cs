using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Repository.Hierarchy;
using NaoCoopLib.Enums;
using NaoCoopLib.Types;
using NaoCoopObjects.Classes;

namespace NaoCoopLib
{
    #region ExecutionRobotStateChangedEventArgs
    public class ExecutionRobotStateChangedEventArgs : EventArgs
    {
        public Guid ID
        {
            get;
            private set;
        }

        public string NewState
        {
            get;
            private set;
        }

        public string ExecutingCommand
        {
            get;
            private set;
        }

        public string RobotStatus
        {
            get;
            private set;
        }

        public ExecutionRobotStateChangedEventArgs(Guid id, string newState, string executingCommand, string robotStatus)
        {
            this.ID = id;
            this.NewState = newState;
            this.ExecutingCommand = executingCommand;
            this.RobotStatus = robotStatus;
        }
    }
    #endregion

    /// <summary>
    /// Class used to manipulate robot executions
    /// </summary>
    public class NaoCoopRobotExecutionEngine
    {
        #region Properties
        /// <summary>
        /// Gets the current execution list
        /// </summary>
        public Dictionary<Execution, List<NaoCoopRobot>> CurrentExecutionList
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the logger
        /// </summary>
        public ILog Logger
        {
            get;
            private set;
        }
        #endregion

        #region Events
        /// <summary>
        /// Triggered when a robot state changed
        /// </summary>
        public event EventHandler<ExecutionRobotStateChangedEventArgs> RobotStateChanged;
        /// <summary>
        /// Triggered when an execution status changed
        /// </summary>
        public event EventHandler ExecutionStatusChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">the application logger</param>
        public NaoCoopRobotExecutionEngine(ILog logger)
        {
            this.CurrentExecutionList = new Dictionary<Execution, List<NaoCoopRobot>>();
            this.Logger = logger;
        }
        #endregion

        #region Methods

        #region Validation
        /// <summary>
        /// Validates the NaoCoopObject Execution.
        /// Throws exception on errors.
        /// </summary>
        /// <param name="execution">The NaoCoopObject Execution object to be validated</param>
        public void ValidateExecution(Execution execution)
        {
            this.ValidateExecution(execution, null);
        }

        private void ValidateExecution(Execution execution, List<NaoCoopRobot> naoRobots)
        {
            if (execution == null || execution.Operation == null || execution.ExecutionRobots.Count == 0)
            {
                throw new ArgumentException("Invalid execution: missing execution, operation or execution robots!");
            }

            if (naoRobots == null)
            {
                naoRobots = new List<NaoCoopRobot>();
            }

            #region Validate Robots
            // validate the number of robots
            if (execution.Operation.OperationRobots.Count != execution.ExecutionRobots.Count)
            {
                throw new Exception("The number of operation robots does not match the number of execution robots!");
            }

            var validatedRobots = new Dictionary<ExecutionRobot, OperationRobot>();
            // validate operation robots with execution robots
            foreach (var operationRobot in execution.Operation.OperationRobots)
            {
                // check if operation robot version matches at lease one 
                foreach (var executionRobot in execution.ExecutionRobots.Except(validatedRobots.Keys))
                {
                    if (operationRobot.RobotVersion.Equals(executionRobot.Robot.RobotVersion))
                    {
                        validatedRobots.Add(executionRobot, operationRobot);
                        break;
                    }
                }
            }

            if (execution.ExecutionRobots.Except(validatedRobots.Keys).Count() != 0)
            {
                throw new Exception("The execution robots provided do not match the operation robots requirements!");
            }
            #endregion

            #region Validate States and Tasks
            foreach (var robot in validatedRobots)
            {
                NaoCoopRobot naoRobot = new NaoCoopRobot(robot.Key.Robot.IP, robot.Key.Robot.Port, this.Logger, robot.Key.ID);
                naoRobots.Add(naoRobot);
                foreach (var robotState in robot.Value.OperationRobotStates.OrderBy(s=>s.Order))
                {
                    NaoState naoState;
                    if (Enum.TryParse<NaoState>(robotState.State.Type, out naoState))
                    {
                        naoRobot.StateCommands.Add(naoState, new OrderedDictionary<NaoCommand, Dictionary<string, string>>());
                        foreach (var stateTask in robotState.State.StateTasks.OrderBy(t => t.Order))
                        {
                            NaoCommand naoCommand;
                            if (Enum.TryParse<NaoCommand>(stateTask.Task.Type, out naoCommand))
                            {
                                naoRobot.StateCommands[naoState].Add(naoCommand, new Dictionary<string, string>());
                                foreach (var setting in stateTask.Task.Settings)
                                {
                                    if (naoRobot.StateCommands[naoState][naoCommand].ContainsKey(setting.Name))
                                    {
                                        naoRobot.StateCommands[naoState][naoCommand][setting.Name] = setting.Value;
                                    }
                                    else
                                    {
                                        naoRobot.StateCommands[naoState][naoCommand].Add(setting.Name, setting.Value);
                                    }
                                }
                            }
                            else
                            {
                                // invalid task specified
                                throw new Exception(string.Format("The specified task '{0}' of type '{1}' is not supported. Supported tasks: {2}", stateTask.Task.Name, stateTask.Task.Type, string.Join(", ", Enum.GetNames(typeof(NaoCommand)))));
                            }
                        }
                    }
                    else
                    {
                        // invalid state specified
                        throw new Exception(string.Format("The specified state '{0}' of type '{1}' is not supported. Supported states: {2}", robotState.State.Name, robotState.State.Type, string.Join(", ", Enum.GetNames(typeof(NaoState)))));
                    }
                }
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// Starts an execution and all of its robots
        /// </summary>
        /// <param name="execution"></param>
        public void Start(Execution execution)
        {
            Start(execution, execution.ExecutionRobots.Select(r => r.ID).ToArray());
        }

        /// <summary>
        /// Stars the specified robots of the specified execution
        /// </summary>
        /// <param name="execution">The execution to start</param>
        /// <param name="robotIDs">The robots of the execution to start</param>
        public void Start(Execution execution, params Guid[] robotIDs)
        {
            // check if execution is already started
            var currentExecution = this.CurrentExecutionList.Keys.FirstOrDefault(e => e.ID.Equals(execution.ID));
            if (currentExecution != null && currentExecution.Status != ExecutionStatus.Started)
            {
                // invalid call, the start should be called on the new execution or on a started execution for a specified robot
                throw new Exception("Invalid call.");
            }

            if (currentExecution==null)
            {
                // call the validate method to generate the nao robot object
                var naoCoopRobots = new List<NaoCoopRobot>();
                // validate execution
                this.ValidateExecution(execution, naoCoopRobots);
                // subscribe to state changed for each robot
                foreach (var robot in naoCoopRobots)
                {
                    //robot.RobotStateChanged += robot_RobotStateChanged;
                    robot.RobotExecutingCommandChanged += robot_RobotStateChanged;
                }
                // add it to the list
                this.CurrentExecutionList.Add(execution, naoCoopRobots);
                // update status and start execution
                execution.Status = ExecutionStatus.Started;
                if (ExecutionStatusChanged != null)
                {
                    ExecutionStatusChanged(execution, null);
                }
                execution.DateStarted = DateTime.Now;
            }

            if (robotIDs != null)
            {
                foreach (var robotId in robotIDs)
                {
                    var robot = this.CurrentExecutionList[execution].FirstOrDefault(r => r.ID.Equals(robotId));
                    if (robot != null)
                    {
                        robot.Start();
                    }
                }
            }
        }

        void robot_RobotStateChanged(object sender, EventArgs ea)
        {
            var robot = sender as NaoCoopRobot;

            // find which execution this robot belongs to
            var execution = CurrentExecutionList.FirstOrDefault(e => e.Value.FirstOrDefault(r => r == robot) != null);

            if (!execution.Equals(default(KeyValuePair<Execution, List<NaoCoopRobot>>)))
            {
                // fire robot state changed event
                if (RobotStateChanged != null)
                {
                    RobotStateChanged(execution.Key, new ExecutionRobotStateChangedEventArgs(robot.ID, robot.CurrentState.ToString(), robot.CurrentExecutingCommand.ToString(), robot.Status.ToString()));
                }

                // check if all robots finished
                if (execution.Value.All(r => r.Status == RobotStatus.Finished))
                {
                    execution.Key.Status = ExecutionStatus.Completed;
                    execution.Key.DateEnded = DateTime.Now;

                    // remove the execution from the list and fire event
                    CurrentExecutionList.Remove(execution.Key);

                    if (ExecutionStatusChanged != null)
                    {
                        ExecutionStatusChanged(execution.Key, null);
                    }
                }
            }
        }

        /// <summary>
        /// Pauses an execution
        /// </summary>
        /// <param name="execution">The execution to pause</param>
        public void Pause(Execution execution)
        {
            Pause(execution, execution.ExecutionRobots.Select(r => r.ID).ToArray());
        }

        /// <summary>
        /// Pauses the specified robots of the specified execution
        /// </summary>
        /// <param name="execution">The execution to pause</param>
        /// <param name="robotIDs">The robots of the execution to pause</param>
        public void Pause(Execution execution, params Guid[] robotIDs)
        {
            // check if execution is already started
            var currentExecution = this.CurrentExecutionList.Keys.FirstOrDefault(e => e.ID.Equals(execution.ID));
            if (currentExecution == null || currentExecution.Status != ExecutionStatus.Started)
            {
                throw new Exception("Missing or invalid execution status!");
            }

            if (robotIDs != null)
            {
                foreach (var robotId in robotIDs)
                {
                    var robot = this.CurrentExecutionList[currentExecution].FirstOrDefault(r => r.ID.Equals(robotId));
                    if (robot != null)
                    {
                        robot.Pause();
                    }
                }
            }

            // change the state to paused only if all robots are paused
            if (this.CurrentExecutionList[currentExecution].All(r => r.Status == RobotStatus.Paused))
            {
                currentExecution.Status = ExecutionStatus.Paused;
                if (ExecutionStatusChanged != null)
                {
                    ExecutionStatusChanged(execution, null);
                }
            }
        }

        /// <summary>
        /// Resumes and execution and all of its robots
        /// </summary>
        /// <param name="execution">The execution to resume</param>
        public void Resume(Execution execution)
        {
            Resume(execution, execution.ExecutionRobots.Select(r => r.ID).ToArray());
        }

        /// <summary>
        /// Resumes the sepcified robots of the specified execution
        /// </summary>
        /// <param name="execution">The execution to resume</param>
        /// <param name="robotIDs">The robots of the execution to resume</param>
        public void Resume(Execution execution, params Guid[] robotIDs)
        {
            // check if execution is already started
            var currentExecution = this.CurrentExecutionList.Keys.FirstOrDefault(e => e.ID.Equals(execution.ID));
            if (currentExecution == null)
            {
                throw new Exception("Missing or invalid execution!");
            }

            if (robotIDs != null)
            {
                foreach (var robotId in robotIDs)
                {
                    var robot = this.CurrentExecutionList[currentExecution].FirstOrDefault(r => r.ID.Equals(robotId));
                    if (robot != null && robot.Status == RobotStatus.Paused)
                    {
                        robot.Resume();
                    }
                }
            }

            // change the state to resumed if there is at least one robot running
            if (this.CurrentExecutionList[currentExecution].Any(r => r.Status == RobotStatus.Running))
            {
                currentExecution.Status = ExecutionStatus.Started;
                if (ExecutionStatusChanged != null)
                {
                    ExecutionStatusChanged(execution, null);
                }
            }
        }

        /// <summary>
        /// Stops an execution and all of is robots
        /// </summary>
        /// <param name="execution">The execution to stop</param>
        public void Stop(Execution execution)
        {
            var currentExecution = this.CurrentExecutionList.Keys.FirstOrDefault(e => e.ID.Equals(execution.ID));
            if (currentExecution == null)
            {
                throw new Exception("Execution does not exist!");
            }

            // refresh the execution from the loaded variable
            currentExecution = execution;

            // stop and dispose each robot object
            foreach (var robot in this.CurrentExecutionList[execution])
            {
                robot.Stop();
                robot.Dispose();
            }

            // update date and status
            execution.Status = ExecutionStatus.Failed; // manually stopped
            if (ExecutionStatusChanged != null)
            {
                ExecutionStatusChanged(execution, null);
            }
            execution.DateEnded = DateTime.Now;
        }
        #endregion
    }
}

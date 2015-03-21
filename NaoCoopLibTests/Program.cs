using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using NaoCoopLib;
using NaoCoopObjects.Classes;

namespace NaoCoopLibTests
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog logger = LogManager.GetLogger(typeof(Program));
            XmlConfigurator.Configure();

            #region Objects Creation
            #region RobotVersions
            RobotVersion robotVersion35 = new RobotVersion();
            robotVersion35.Name = "35";

            RobotVersion robotVersion45 = new RobotVersion();
            robotVersion45.Name = "45";

            #endregion

            #region Robots
            Robot robot1 = new Robot();
            robot1.Enabled = true;
            robot1.IP = "10.0.0.1";
            robot1.Name = "robot1";
            robot1.Port = 9559;
            robot1.RobotVersion = robotVersion35;

            Robot robot2 = new Robot();
            robot2.Enabled = true;
            robot2.IP = "10.0.0.2";
            robot2.Name = "robot2";
            robot2.Port = 9559;
            robot2.RobotVersion = robotVersion35;

            Robot robot3 = new Robot();
            robot3.Enabled = true;
            robot3.IP = "10.0.0.3";
            robot3.Name = "robot3";
            robot3.Port = 9559;
            robot3.RobotVersion = robotVersion45;
            #endregion

            #region Tasks
            Task taskWalkToNaoMark = new Task(){Name = "WalkToCheckpoint"};
            taskWalkToNaoMark.Settings.Add(new Setting() { Name = "MarkID", Value = "64" });
            taskWalkToNaoMark.Settings.Add(new Setting() { Name = "MarkSize", Value = "0.095" });
            taskWalkToNaoMark.Settings.Add(new Setting() { Name = "AdvanceDistance", Value = "0.20" });
            taskWalkToNaoMark.Settings.Add(new Setting() { Name = "StopDistance", Value = "0.20" });

            Task taskGoToGrabLocation = new Task(){Name = "GoToGrabLocation"};
            taskGoToGrabLocation.Settings.Add(new Setting() { Name = "GrabLocation", Value = "A" });

            Task taskGoToLiftPosition = new Task(){Name = "GoToLiftPosition"};

            Task taskSynchRobot = new Task(){Name = "SynchRobot"};

            Task taskLiftObject = new Task(){Name = "LiftObject"};

            Task taskWalkWithObject = new Task(){Name = "WalkWithObject"};
            #endregion

            #region States
            State stateInitialized = new State() { Name = "Initialized" };
            State stateAtCheckpoint = new State() { Name = "AtCheckpoint" };
            State stateAtGrabLocation = new State() { Name = "AtGrabLocation" };
            State stateInLiftPosition = new State() { Name = "InLiftPosition" };
            State stateInWalkPosition = new State() { Name = "InWalkPosition" };
            State stateTerminated = new State() { Name = "Terminated" };
            #endregion

            #region StateTasks
            stateInitialized.StateTasks.Add(new StateTask() { Task = taskWalkToNaoMark, Order = 0 });

            stateAtCheckpoint.StateTasks.Add(new StateTask() { Task = taskGoToGrabLocation, Order = 0 });

            stateAtGrabLocation.StateTasks.Add(new StateTask() { Task = taskGoToLiftPosition, Order = 0 });

            stateInLiftPosition.StateTasks.Add(new StateTask() { Task = taskSynchRobot, Order = 0 });
            stateInLiftPosition.StateTasks.Add(new StateTask() { Task = taskLiftObject, Order = 1 });

            stateInWalkPosition.StateTasks.Add(new StateTask() { Task = taskWalkWithObject, Order = 0 });
            #endregion

            #region OperationRobots
            OperationRobot singleRobot1 = new OperationRobot() { Name = "singleRobot1", RobotVersion = robotVersion45 };

            OperationRobot synchRobot1 = new OperationRobot() { Name = "synchRobot1", RobotVersion = robotVersion35 };
            synchRobot1.OperationRobotStates.Add(new OperationRobotState() { State = stateInitialized, Order = 0 });
            synchRobot1.OperationRobotStates.Add(new OperationRobotState() { State = stateAtCheckpoint, Order = 1 });
            synchRobot1.OperationRobotStates.Add(new OperationRobotState() { State = stateAtGrabLocation, Order = 2 });
            synchRobot1.OperationRobotStates.Add(new OperationRobotState() { State = stateInLiftPosition, Order = 3 });
            synchRobot1.OperationRobotStates.Add(new OperationRobotState() { State = stateInWalkPosition, Order = 4 });
            synchRobot1.OperationRobotStates.Add(new OperationRobotState() { State = stateTerminated, Order = 5 });
            #endregion

            #region Operations
            Operation operation1 = new Operation() { Name = "SingleCarry" };
            operation1.OperationRobots.Add(new OperationRobot() { Name = "robot1", RobotVersion = robotVersion45 });

            Operation operation2 = new Operation() { Name = "SynchronizedCarry" };
            operation2.OperationRobots.Add(synchRobot1);
            //operation2.OperationRobots.Add(synchRobot1);
            #endregion

            #region Executions
            Execution execution1 = new Execution() { DateCreated = DateTime.Now, Operation = operation2, Status = ExecutionStatus.Pending };
            execution1.ExecutionRobots.Add(new ExecutionRobot() { Robot = robot1 });
            //execution1.ExecutionRobots.Add(new ExecutionRobot() { Robot = robot2 });
            #endregion

            #endregion

            NaoCoopRobotExecutionEngine executionEngine = new NaoCoopRobotExecutionEngine(logger);
            executionEngine.Start(execution1);

            Console.ReadLine();
        }
    }
}

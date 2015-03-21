using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using NaoCoopApp.Models;
using NaoCoopLib;

namespace NaoCoopApp.Views
{
    #region Robot Status Converter
    [ValueConversion(typeof(RobotStatus), typeof(bool))]
    public class RobotStatusVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && value!=null)
            {
                foreach (var state in parameter.ToString().Split(';'))
                {
                    if (value.ToString().Equals(state))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    #endregion

    /// <summary>
    /// Interaction logic for OperationExecutionView.xaml
    /// </summary>
    public partial class OperationExecutionView : ModernWindow
    {
        #region Properties
        public Execution CurrentExecution
        {
            get;
            private set;
        }

        private NaoCoopRobotExecutionEngine ExecutionEngine
        {
            get;
            set;
        }
        #endregion

        public OperationExecutionView(Operation operation, Robot[] robots)
        {
            InitializeComponent();
            CurrentExecution = GenerateExecution(operation, robots);
            ExecutionEngine = new NaoCoopRobotExecutionEngine(App.AppLogger);
            // subscribe to execution engine events
            ExecutionEngine.RobotStateChanged += ExecutionEngine_RobotStateChanged;
            ExecutionEngine.ExecutionStatusChanged += ExecutionEngine_ExecutionStatusChanged;
            this.DataContext = this;
            RefreshView();
        }

        private Execution GenerateExecution(Operation operation, Robot[] robots)
        {
            Execution execution = new Execution();
            execution.Operation = operation;
            execution.DateCreated = DateTime.Now;
            execution.Status = NaoCoopObjects.Classes.ExecutionStatus.Pending;

            if (robots != null)
            {
                foreach (var robot in robots)
                {
                    ExecutionRobot executionRobot = new ExecutionRobot();
                    executionRobot.Robot = robot;
                    executionRobot.Status = RobotStatus.Idle.ToString();
                    execution.ExecutionRobots.Add(executionRobot);
                }
            }

            return execution;
        }

        public void ValidateExecution()
        {
            ExecutionEngine.ValidateExecution(CurrentExecution.Data);
        }

        private void RefreshExecutionStatus(NaoCoopObjects.Classes.Execution execution)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(new Action<NaoCoopObjects.Classes.Execution>(p => RefreshExecutionStatus(p)), execution);
                return;
            }

            txtExecutionStatus.Text = execution.Status.ToString();
            RefreshView();
        }

        private void RefreshView()
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(new Action(RefreshView));
                return;
            }

            // disable all controls
            UpdateControlsEnabled(false, btnStartExecution, btnPauseExecution, btnResumeExecution, btnStopExecution);

            switch (CurrentExecution.Status)
            {
                case NaoCoopObjects.Classes.ExecutionStatus.Pending:
                    UpdateControlsEnabled(true, btnStartExecution);
                    break;
                case NaoCoopObjects.Classes.ExecutionStatus.Started:
                    UpdateControlsEnabled(true, btnPauseExecution, btnStopExecution);
                    break;
                case NaoCoopObjects.Classes.ExecutionStatus.Paused:
                    UpdateControlsEnabled(true, btnResumeExecution, btnStopExecution);
                    break;
                case NaoCoopObjects.Classes.ExecutionStatus.Failed:
                    break;
                case NaoCoopObjects.Classes.ExecutionStatus.Completed:
                    break;

            }
        }

        private void UpdateControlsEnabled(bool enabled, params Control[] controls)
        {
            if (controls != null)
            {
                foreach (var control in controls)
                {
                    control.IsEnabled = enabled;
                }
            }
        }

        #region Button Click Handlers
        private void btnStartExecution_Click(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Start(CurrentExecution.Data, null);
        }

        private void btnPauseExecution_Click(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Pause(CurrentExecution.Data);
        }

        private void btnResumeExecution_Click(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Resume(CurrentExecution.Data);
        }

        private void btnStopExecution_Click(object sender, RoutedEventArgs e)
        {
            ExecutionEngine.Stop(CurrentExecution.Data);
        }

        private void btnOpenLogFile_Click(object sender, RoutedEventArgs e)
        {
            var filename = App.LogFileName;
            Process.Start("notepad.exe", filename);
        }

        private void btnStartRobotExecution_Click(object sender, RoutedEventArgs e)
        {
            var btnSender = sender as Button;
            var executionRobot = (Guid)btnSender.CommandParameter;
            ExecutionEngine.Start(CurrentExecution.Data, executionRobot);
        }
        #endregion

        #region ExecutionEngine Event Handlers
        void ExecutionEngine_ExecutionStatusChanged(object sender, EventArgs e)
        {
            RefreshExecutionStatus(sender as NaoCoopObjects.Classes.Execution);
        }

        void ExecutionEngine_RobotStateChanged(object sender, ExecutionRobotStateChangedEventArgs e)
        {
            var robot = CurrentExecution.ExecutionRobots.FirstOrDefault(r => r.ID.Equals(e.ID));
            if (robot != null)
            {
                robot.CurrentCommand = e.ExecutingCommand;
                robot.CurrentState = e.NewState;
                robot.Status = e.RobotStatus;
            }
        }
        #endregion
    }
}

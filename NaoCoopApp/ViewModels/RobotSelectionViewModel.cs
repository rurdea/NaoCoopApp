using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NaoCoopApp.Helpers;
using NaoCoopApp.Models;

namespace NaoCoopApp.ViewModels
{
    public enum DialogResult
    {
        Ok,
        Cancel
    }

    public class RobotSelectionViewModel : RecordsViewModel<Robot, NaoCoopObjects.Classes.Robot>
    {
        #region Properties
        private ObservableCollection<RobotSelection> _availableExecutingRobots;
        public ObservableCollection<RobotSelection> AvailableRobots
        {
            get
            {
                return _availableExecutingRobots;
            }
            set
            {
                if (_availableExecutingRobots != value)
                {
                    _availableExecutingRobots = value;
                    OnPropertyChanged(() => AvailableRobots);
                }
            }
        }
        #endregion

        #region Commands
        public ICommand OkCommand { get { return new DelegateCommand(p => OnOkCommand((NaoCoopApp.Views.RobotSelectionView)p)); } }
        public ICommand CancelCommand { get { return new DelegateCommand(p => OnCancelCommand((NaoCoopApp.Views.RobotSelectionView)p)); } }
        #endregion

        #region Constructor
        public RobotSelectionViewModel()
        {
            AvailableRobots = new ObservableCollection<RobotSelection>();
            OnRefreshData();
        }
        #endregion

        #region Methods
        protected override void OnRefreshData()
        {
            base.OnRefreshData();

            AvailableRobots.Clear();
            foreach (var robot in RecordsCollection)
            {
                AvailableRobots.Add(new RobotSelection(robot.Data));
            }
        }

        protected void OnOkCommand(Views.RobotSelectionView robotSelectionView)
        {
            // set the selected robots
            foreach (var robot in AvailableRobots)
            {
                if (robot.Selected)
                {
                    robotSelectionView.SelectedRobots.Add(robot);
                }
            }
            robotSelectionView.DialogResult = DialogResult.Ok;
            robotSelectionView.Close();
        }

        protected void OnCancelCommand(Views.RobotSelectionView robotSelectionView)
        {
            robotSelectionView.DialogResult = DialogResult.Cancel;
            robotSelectionView.Close();
        }
        #endregion

        

    }
}

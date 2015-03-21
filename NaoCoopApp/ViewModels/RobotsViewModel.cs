using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NaoCoopApp.Helpers;
using NaoCoopApp.Models;

namespace NaoCoopApp.ViewModels
{
    public class RobotsViewModel : RecordsViewModel<Robot, NaoCoopObjects.Classes.Robot>
    {
        private ObservableCollection<RobotVersion> _availableRobotVersions;
        public ObservableCollection<RobotVersion> AvailableRobotVersions
        {
            get
            {
                return _availableRobotVersions;
            }
            set
            {
                if (_availableRobotVersions != value)
                {
                    _availableRobotVersions = value;
                    OnPropertyChanged(() => AvailableRobotVersions);
                }
            }
        }

        protected override void OnRefreshData()
        {
            base.OnRefreshData();

            // get robot versions from database
            var versions = DataAccessHelper.Instance.RobotVersionsManager.GetRecords();
            AvailableRobotVersions = base.InitializeRecordsCollection<RobotVersion, NaoCoopObjects.Classes.RobotVersion>(AvailableRobotVersions, versions);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Models;

namespace NaoCoopApp.ViewModels
{
    public class RequirementsViewModel : RecordsViewModel<Requirement, NaoCoopObjects.Classes.Requirement>
    {
        private ObservableCollection<string> _availableTypes;
        public ObservableCollection<string> AvailableTypes
        {
            get
            {
                return _availableTypes;
            }
            set
            {
                if (_availableTypes != value)
                {
                    _availableTypes = value;
                    OnPropertyChanged(() => AvailableTypes);
                }
            }
        }

        public RequirementsViewModel()
        {
            _availableTypes = new ObservableCollection<string>(Enum.GetNames(typeof(NaoCoopObjects.Classes.RequirementType)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using NaoCoopApp.Helpers;
using NaoCoopApp.Models;

namespace NaoCoopApp.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        #region Members
        List<LinkGroup> _menuGroups;
        #endregion

        public ApplicationViewModel()
        {
            // welcome group
            LinkGroup grpWelcome = new LinkGroup();
            grpWelcome.DisplayName = "Welcome";
            grpWelcome.Links.Add(new Link() { DisplayName = "Executions", Source = new Uri(@"/Views/ExecutionsView.xaml", UriKind.Relative) });
            

            // settings group
            LinkGroup grpSettings = new LinkGroup();
            grpSettings.DisplayName = "Settings";
            grpSettings.Links.Add(new Link() { DisplayName = "Robots", Source = new Uri(@"/Views/RobotsView.xaml", UriKind.Relative) });
            grpSettings.Links.Add(new Link() { DisplayName = "Robot Versions", Source = new Uri(@"/Views/RobotVersionsView.xaml", UriKind.Relative) });
            grpSettings.Links.Add(new Link() { DisplayName = "Requirements", Source = new Uri(@"/Views/RequirementsView.xaml", UriKind.Relative) });
            grpSettings.Links.Add(new Link() { DisplayName = "Operations", Source = new Uri(@"/Views/OperationsView.xaml", UriKind.Relative) });
            grpSettings.Links.Add(new Link() { DisplayName = "States", Source = new Uri(@"/Views/StatesView.xaml", UriKind.Relative) });
            grpSettings.Links.Add(new Link() { DisplayName = "Tasks", Source = new Uri(@"/Views/TasksView.xaml", UriKind.Relative) });

            // welcome group should be added for all users
            MenuGroups.Add(grpWelcome);

            if (App.CurrentUser.IsAdmin)
            {
                // add settings for administrators
                MenuGroups.Add(grpSettings);
            }
        }


        #region Properties
        public List<LinkGroup> MenuGroups
        {
            get
            {
                if (_menuGroups == null)
                    _menuGroups = new List<LinkGroup>();

                return _menuGroups;
            }
        }

        public NaoCoopObjects.Classes.User CurrentUser
        {
            get
            {
                return App.CurrentUser;
            }
        }
        #endregion
    }
}

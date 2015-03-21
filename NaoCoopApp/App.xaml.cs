using System.Windows;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;
using NaoCoopApp.ViewModels;
using System.Linq;
using NaoCoopApp.Views;

namespace NaoCoopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Members
        private int _attempts;
        private ApplicationWindow _appWindow = null;
        #endregion


        #region Properties
        private static ILog _appLogger;
        public static ILog AppLogger
        {
            get
            {
                if (_appLogger == null)
                {
                    _appLogger = LogManager.GetLogger(typeof(App));
                    XmlConfigurator.Configure();
                }
                return _appLogger;
            }
        }

        private static string _logFileName;
        public static string LogFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_logFileName))
                {
                    // force initializing logger
                    var logger = AppLogger;

                    var rootAppender = ((Hierarchy)LogManager.GetRepository()).Root.Appenders.OfType<FileAppender>().FirstOrDefault();
                    _logFileName = rootAppender != null ? rootAppender.File : string.Empty;
                }

                return _logFileName;
            }
        }

        public static NaoCoopObjects.Classes.User CurrentUser
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            ShowLogOn();
        }

        public void ShowLogOn()
        {
            var logon = new LogOn();
            logon.Attempts = _attempts;
            bool? res = logon.ShowDialog();
            if (!res ?? true)
            {
                Shutdown(1);
            }
            else
            {
                NaoCoopObjects.Classes.User user = Helpers.DataAccessHelper.Instance.UsersManager.ValidateUsernameAndPassword(logon.UserName, logon.Password);
                if (user!=null)
                {
                    StartUp(user);
                }
                else
                {
                    if (logon.Attempts > 2)
                    {
                        MessageBox.Show("Application is exiting due to invalid credentials", "Application Exit", MessageBoxButton.OK, MessageBoxImage.Error);
                        Shutdown(1);
                    }
                    else
                    {
                        _attempts += 1;
                        ShowLogOn();
                    }
                }
            }
        }

        public void StartUp(NaoCoopObjects.Classes.User user)
        {
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            CurrentUser = user;
            _appWindow = new ApplicationWindow();
            ApplicationViewModel context = new ApplicationViewModel();
            _appWindow.DataContext = context;
            _appWindow.InitializeMenu();
            _appWindow.Show();
        }

        public void LogOut()
        {
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            CurrentUser = null;
            if (_appWindow != null)
            {
                _appWindow.Close();
            }

            ShowLogOn();
        }
        #endregion
    }
}

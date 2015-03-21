using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;

namespace NaoCoopApp.Views
{
    /// <summary>
    /// Interaction logic for LogOn.xaml
    /// </summary>
    public partial class LogOn : ModernWindow, INotifyPropertyChanged {
        #region Private Fields

        private int _attempts;

        #endregion

        #region Public Properties

        public int Attempts
        {
            get { return _attempts; }
            set
            {
                if (value != _attempts)
                {
                    _attempts = value;
                    OnPropertyChanged("Attempts");
                }
            }
        }

        public Visibility ShowInvalidCredentials
        {
            get
            {
                if (_attempts > 0)
                {
                    return Visibility.Visible;
                }
                return Visibility.Hidden;
            }
        }

        public string UserName
        {
            get { return txtUsername.Text; }
        }

        public string Password
        {
            get { return txtPassword.Password; }
        }

        #endregion

        public LogOn()
        {
            InitializeComponent();
            DataContext = this;
            txtUsername.Focus();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged {
            add { propertyChangedEvent += value; }
            remove { propertyChangedEvent -= value; }
        }

        #endregion

         private void LogonClick(object sender, RoutedEventArgs e) {
            DialogResult = true;

            #if DEBUG
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                txtPassword.Password = "admin";
                txtUsername.Text = "admin";
            }
            #endif

            Close();
        }

        private void CredentialsFocussed(object sender, RoutedEventArgs e) {
            TextBoxBase tb = sender as TextBoxBase;
            if (tb == null) {
                PasswordBox pwb = sender as PasswordBox;
                pwb.SelectAll();
            }
            else {
                tb.SelectAll();
            }
        }

        private event PropertyChangedEventHandler propertyChangedEvent;

        protected void OnPropertyChanged(string prop) {
            if (propertyChangedEvent != null)
                propertyChangedEvent(this, new PropertyChangedEventArgs(prop));
        }
    }
}

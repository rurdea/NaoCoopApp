using System;
using System.Collections.Generic;
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
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using NaoCoopApp.ViewModels;

namespace NaoCoopApp
{
    /// <summary>
    /// Interaction logic for ApplicationWindow.xaml
    /// </summary>
    public partial class ApplicationWindow : ModernWindow
    {
        public ApplicationWindow()
        {
            InitializeComponent();
        }

        public void InitializeMenu()
        {
            var main = Application.Current.MainWindow as ModernWindow;

            if (main != null)
            {
                main.MenuLinkGroups.Clear();

                var viewModel = this.DataContext as ApplicationViewModel;

                foreach(var group in viewModel.MenuGroups)
                {
                    main.MenuLinkGroups.Add(group);
                }
            }
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NaoCoopApp.Models;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Views
{
    /// <summary>
    /// Interaction logic for OperationsView.xaml
    /// </summary>
    public partial class OperationsView : UserControl
    {
        public OperationsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check errors on removed item and select it back if errors are present
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // return if no removed items
            if (e.RemovedItems.Count == 0) return;
            // get datagrid
            var dg = sender as DataGrid;
            // get removed item
            var operation = e.RemovedItems[0] as ModelValidatorBase<NaoCoopObjects.Classes.NaoCoopObject>;
            if (operation != null)
            {
                // check if has errors
                var hasError = !string.IsNullOrEmpty(operation.Error);
                // enable/disable save button
                btnSave.IsEnabled = !hasError;
                // select back the item with error in the grid
                if (hasError)
                {
                    dg.SelectionChanged -= DataGrid_SelectionChanged;
                    dg.SelectedItem = e.RemovedItems[0];
                    dg.SelectionChanged += DataGrid_SelectionChanged;
                }
            }
        }
    }
}

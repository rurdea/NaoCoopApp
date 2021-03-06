﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NaoCoopApp.Models;

namespace NaoCoopApp.Views
{
    /// <summary>
    /// Interaction logic for TasksWindow.xaml
    /// </summary>
    public partial class TasksView : UserControl
    {
        public TasksView()
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
            var task = e.RemovedItems[0] as Task;
            if (task != null)
            {
                // check if has errors
                var hasError = !string.IsNullOrEmpty(task.Error);
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

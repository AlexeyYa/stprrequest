/*

Copyright 2019 Yamborisov Alexey

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*/

using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Data;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Globalization;

namespace requestCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private ObservableCollection<DataClass> dataCollection = new ObservableCollection<DataClass>();
        // private SharedData sharedData = new SharedData();
        private Variables variables = Variables.Instance;
        private ViewModel vm;
        //private string Tasks = string.Empty, Corrections = string.Empty;
        //private string User, Phone, Group;

        public MainWindow()
        {
            InitializeComponent();
            vm = new ViewModel();
            LoadSettings(); // Reloaded after closing settings window
            LoadConfigData();
            DataContext = vm;
        }

        // Init
        private void LoadSettings()
        {
            if (Properties.Settings.Default.User != null && Properties.Settings.Default.User != String.Empty)
            {
                vm.User = Properties.Settings.Default.User;
            }
            if (Properties.Settings.Default.Phone != null && Properties.Settings.Default.Phone != String.Empty)
            {
                vm.Phone = Properties.Settings.Default.Phone;
            }
            if (Properties.Settings.Default.Group != null && Properties.Settings.Default.Group != String.Empty)
            {
                vm.Group = Properties.Settings.Default.Group;
            }
        }

        private void LoadConfigData()
        {
            cmbObject.ItemsSource = variables.Objects;
            cmbPublish.ItemsSource = variables.PublishTypes;
            cmbSub.ItemsSource = variables.Subs;
        }

        // Datagrid logic
        // Drag'n'drop
        private void DGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);

                vm.AddData(paths);
            }
        }

        // Dialogs part

        //private void BtnFolder_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog folderBrowser = new OpenFileDialog
        //    {
        //        ValidateNames = false,
        //        CheckFileExists = false,
        //        CheckPathExists = true,
        //        FileName = "Выбор папки"
        //    };
        //    if (folderBrowser.ShowDialog() == true)
        //    {
        //        string pth = System.IO.Path.GetDirectoryName(folderBrowser.FileName);
        //        vm.SavePath = pth;
        //    }
        //}

        private void BtnTasks_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new TasksWindow
            {
                Owner = Application.Current.MainWindow
            };

            dlg.Tasks = vm.Tasks;
            dlg.ShowDialog();
            if (dlg.DialogResult == true)
            {
                vm.Tasks = dlg.Tasks;
            }
        }

        private void CorrectionsBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CorrectionsWindow
            {
                Owner = Application.Current.MainWindow
            };

            dlg.ShowDialog();
            if (dlg.DialogResult == true)
            {
                vm.Corrections = dlg.Corrections;
            }
        }

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SettingsWindow
            {
                Owner = Application.Current.MainWindow
            };
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                LoadSettings();
            }
        }

        // DataGrid actions

        private void EditSize(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "PDF (*.pdf)|*.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                DataClass d = ((FrameworkElement)sender).DataContext as DataClass;
                foreach (string filename in openFileDialog.FileNames)
                {
                    d.SizeAdd(PdfProcessing.ProcessFile(filename));
                }
            }
        }
        private void EditSizeCor(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "PDF (*.pdf)|*.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                DataClass d = ((FrameworkElement)sender).DataContext as DataClass;
                foreach (string filename in openFileDialog.FileNames)
                {
                    d.SizeCorAdd(PdfProcessing.ProcessFile(filename));
                }
            }
        }
        private void ClearSize(object sender, RoutedEventArgs e)
        {
            DataClass d = ((FrameworkElement)sender).DataContext as DataClass;
            d.Size = new PdfFormat();
        }
        private void ClearSizeCor(object sender, RoutedEventArgs e)
        {
            DataClass d = ((FrameworkElement)sender).DataContext as DataClass;
            d.SizeCor = new PdfFormat();
        }

        private void ShowHideDetails(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow row)
                {
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible
                        ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        // App closing

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private void btnAddFrom_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "My Title";
            if (((Button)sender).Name == "btnAddFolders") {
                dlg.IsFolderPicker = true;
            }
            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = true;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var paths = dlg.FileNames;
                vm.AddData(paths);
            }
        }


        public void lostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.Name == "tbOrig")
            {
                vm.NroCh = true;
            }
            else
            {
                vm.NrcCh = true;
            }
        }
    }

    public class BoolToStyleConverter : IValueConverter
    {
        /*Style invalidStyle = new Style();
        Style validStyle = new Style();
        BoolToStyleConverter(string value)
        {
            invalidStyle.Setters.Add(new Setter { Property = Control.BorderBrushProperty, Value = new SolidColorBrush(Colors.Red) });
            invalidStyle.Setters.Add(new Setter { Property = Control.BorderThicknessProperty, Value = 2 });
        }*/

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Style invalidStyle = new Style();
            Style validStyle = new Style();
            invalidStyle.Setters.Add(new Setter { Property = Control.BorderBrushProperty, Value = new SolidColorBrush(Colors.Red) });
            invalidStyle.Setters.Add(new Setter { Property = Control.BorderThicknessProperty, Value = new Thickness(2) });
            if ((bool)value)
                return validStyle;
            else
                return invalidStyle;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

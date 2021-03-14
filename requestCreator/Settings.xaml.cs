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
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Collections.Generic;

namespace requestCreator
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private ObservableCollection<string> groupsCollection = new ObservableCollection<string>();

        public SettingsWindow()
        {
            InitializeComponent();

            string[] groups = File.ReadAllLines(@".\cfg\groups.txt");
            foreach (string group in groups)
            {
                groupsCollection.Add(group);
            }
            cmbGroup.ItemsSource = groupsCollection;

            FillSettings();
        }

        private void FillSettings()
        {
            tbUser.Text = Properties.Settings.Default.User;
            tbPhone.Text = Properties.Settings.Default.Phone;
            cmbGroup.SelectedItem = Properties.Settings.Default.Group;
            tbSender.Text = Properties.Settings.Default.Sender;

            tbConfig.Text = Properties.Settings.Default.ConfigPath;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.User = tbUser.Text;
            Properties.Settings.Default.Phone = tbPhone.Text;
            Properties.Settings.Default.Group = cmbGroup.SelectedItem as string;
            Properties.Settings.Default.Path = tbPath.Text;
            Properties.Settings.Default.Sender = tbSender.Text;

            Properties.Settings.Default.ConfigPath = tbConfig.Text;
            Variables.Instance.LoadConfig();

            Properties.Settings.Default.Save();

            DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Выбор папки"
            };
            if (folderBrowser.ShowDialog() == true)
            {
                string pth = System.IO.Path.GetDirectoryName(folderBrowser.FileName);
                if (pth[pth.Length - 1] != '\\')
                {
                    pth += "\\";
                }
                tbPath.Text = pth;
            }
        }
    }
}

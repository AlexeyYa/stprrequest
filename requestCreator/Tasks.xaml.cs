/*

Copyright[yyyy][name of copyright owner]

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
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace requestCreator
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class TasksWindow : Window
    {
        public TasksWindow()
        {
            InitializeComponent();
        }

        public string Tasks
        {
            get
            {
                string st = "";
                foreach (var c in LogicalTreeHelper.GetChildren(TaskGrid))
                {
                    if (c is CheckBox)
                    {
                        var chkCast = c as CheckBox;
                        if (chkCast.IsChecked == true)
                        {
                            if (st == "")
                            {
                                st = chkCast.Content as string;
                            }
                            else
                            {
                                st += "," + chkCast.Content as string;
                            }
                        }
                    }
                }
                if (OtherText.Text != "Другое")
                {
                    st += OtherText.Text;
                }
                return st;
            }
            set
            {
                if (value != null)
                {
                    string[] strings = value.Split(',');

                    foreach (var c in LogicalTreeHelper.GetChildren(TaskGrid))
                    {
                        if (c is CheckBox)
                        {
                            var chkCast = c as CheckBox;
                            if (strings.Contains(chkCast.Content))
                            {
                                chkCast.IsChecked = true;
                            }
                        }
                    }
                }
            }
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

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

namespace requestCreator
{
    /// <summary>
    /// Interaction logic for Corrections.xaml
    /// </summary>
    public partial class CorrectionsWindow : Window
    {
        public CorrectionsWindow()
        {
            InitializeComponent();
        }

        public string Corrections
        {
            get
            {
                string st = "";
                foreach (var c in LogicalTreeHelper.GetChildren(CorGrid))
                {
                    if (c is CheckBox)
                    {
                        var chkCast = c as CheckBox;
                        if (chkCast.IsChecked == true)
                        {
                            int rowIndex = System.Windows.Controls.Grid.GetRow(c as UIElement);
                            int columnIndex = System.Windows.Controls.Grid.GetColumn(c as UIElement);
                            if (rowIndex == 9)
                            {
                                if (st == "")
                                {
                                    st += (rowIndex - 2);
                                }
                                else
                                {
                                    st += "," + (rowIndex - 2);
                                }
                            }
                            else
                            {
                                if (st == "")
                                {
                                    st += (rowIndex - 2) + "-" + (columnIndex - 1);
                                }
                                else
                                {
                                    st += "," + (rowIndex - 2) + "-" + (columnIndex - 1);
                                }
                            }
                        }
                    }
                }
                return st;
            }
            set
            {

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

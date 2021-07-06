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

using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace requestCreator
{
    /// <summary>
    /// Interaction logic for Corrections.xaml
    /// </summary>
    public partial class CorrectionsWindow : Window
    {
        /// <summary>
        /// Build window gui with checkbox table
        /// </summary>
        public CorrectionsWindow()
        {
            InitializeComponent();

            var grid = CorGrid;
            var cors = Variables.Instance.Corrections;

            int colN = cors.Max(kv => kv.Value.Count);
            for (int k = 1; k < colN; ++k)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            var columns = cors.First().Value.Values;
            AddHeader(columns, 0);
            
            // Add rows with label and checkboxes
            int row = 1;
            foreach (var cor in cors)
            {
                grid.RowDefinitions.Add(new RowDefinition());

                // Add row with column names if needed
                if (!cor.Value.Values.SequenceEqual(columns))
                {
                    columns = cor.Value.Values;
                    AddHeader(columns, row);
                    grid.RowDefinitions.Add(new RowDefinition());
                    row++;
                }
                
                // Add label to 0th column
                TextBlock cor_name_y = new TextBlock();
                cor_name_y.Margin = new Thickness(5);
                cor_name_y.TextWrapping = TextWrapping.Wrap;
                cor_name_y.Text = cor.Key.Item2;
                cor_name_y.HorizontalAlignment = HorizontalAlignment.Left;
                cor_name_y.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(cor_name_y, 0);
                Grid.SetRow(cor_name_y, row);
                grid.Children.Add(cor_name_y);

                // Add checkboxes for each correction entry
                for (int col = 1; col < columns.Count + 1; col++)
                {
                    CheckBox chbx = new CheckBox();
                    chbx.HorizontalAlignment = HorizontalAlignment.Center;
                    chbx.VerticalAlignment = VerticalAlignment.Center;
                    chbx.ToolTip = cor.Key.Item1.ToString() + '-' + col;
                    Grid.SetColumn(chbx, col);
                    Grid.SetRow(chbx, row);
                    grid.Children.Add(chbx);
                }
                row++;
            }
            int rc = grid.RowDefinitions.Count;
            int cc = grid.ColumnDefinitions.Count;
            Grid.SetColumn(OkBtn, cc - 2);
            Grid.SetRow(OkBtn, rc - 1);
            Grid.SetColumn(CancelBtn, cc - 1);
            Grid.SetRow(CancelBtn, rc - 1);
        }
        /// <summary>
        /// Cycle through all checkboxes and add tooltip from checked
        /// </summary>
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
                            
                            if (st == "")
                            {
                                st += chkCast.ToolTip;
                            }
                            else
                            {
                                st += "," + chkCast.ToolTip;
                            }
                        }
                    }
                }
                return st;
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

        private void AddHeader(System.Collections.Generic.Dictionary<int, string>.ValueCollection columns, int row)
        {
            int tmpj = 1;
            foreach (var c in columns)
            {
                TextBlock cor_name_x = new TextBlock();
                cor_name_x.Margin = new Thickness(5);
                cor_name_x.TextWrapping = TextWrapping.Wrap;
                cor_name_x.Text = c;
                cor_name_x.HorizontalAlignment = HorizontalAlignment.Center;
                cor_name_x.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(cor_name_x, tmpj);
                Grid.SetRow(cor_name_x, row);
                CorGrid.Children.Add(cor_name_x);
                tmpj++;
            }
        }
    }
}

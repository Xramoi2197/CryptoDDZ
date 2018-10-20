using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CryptoDDZ
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public void WriteResults(string result)
        {
            RezultBox.Text += result;
            RezultBox.Text += "\r\n";
        }

        readonly Algorythms _algorythms;
        private string _info;
        private Dictionary<string, string> _parameters;
        private List<DataGridRowEx> _rowExs;

        public MainWindow()
        {
            _algorythms = new Algorythms(WriteResults);
            InitializeComponent();
        }

        private void DoButton_OnClick(object sender, RoutedEventArgs e)
        {
            RezultBox.Text = string.Empty;
            if (AlgCombo.SelectedItem == null)
            {
                return;
            }
            foreach (DataGridRowEx dataGridItem in DataGrid.Items)
            {
                if (dataGridItem.Value == string.Empty)
                {
                    WriteResults("Недостаточно аргументов");
                    return;
                }
            }
            var text = AlgCombo.SelectedItem as ComboBoxItem;
            _algorythms.DoAlg(text?.Name, _parameters);
        }

        private void AlgCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }
            ChangeView(selectedItem.Name);
        }

        private void ChangeView(string name)
        {
            _parameters = new Dictionary<string, string>();
            _rowExs = new List<DataGridRowEx>();
            RezultBox.Text = string.Empty;
            
            switch (name) //Сюда необходимо добавть свой алгоритм!!!
            {
                case "Example":
                {
                    _info = "Example text info"; //Info надо будет положить в ресурсы!!!
                    _parameters.Add("x", string.Empty);
                    _parameters.Add("y", string.Empty);
                    DataGridRowEx newRow = new DataGridRowEx(1, "x", string.Empty);
                    _rowExs.Add(newRow);
                    newRow = new DataGridRowEx(2, "y", string.Empty);
                    _rowExs.Add(newRow);
                    _rowExs.Sort();
                    DataGrid.ItemsSource = _rowExs;
                    break;
                }
                case "Shenks":
                {
                    _info = Properties.Resources.Shenks;
                    _parameters.Add("p", string.Empty);
                    _parameters.Add("a", string.Empty);
                    _parameters.Add("b", string.Empty);
                    _parameters.Add("m", string.Empty);
                    _parameters.Add("k", string.Empty);
                    
                    DataGridRowEx newRow = new DataGridRowEx(1, "p", string.Empty);
                    _rowExs.Add(newRow);
                    newRow = new DataGridRowEx(2, "a", string.Empty);
                    _rowExs.Add(newRow);
                    newRow = new DataGridRowEx(3, "b", string.Empty);
                    _rowExs.Add(newRow);
                    newRow = new DataGridRowEx(4, "m", string.Empty);
                    _rowExs.Add(newRow);
                    newRow = new DataGridRowEx(5, "k", string.Empty);
                    _rowExs.Add(newRow);
                    _rowExs.Sort();
                    DataGrid.ItemsSource = _rowExs;
                    break;
                }
            }
            InfoBox.Text = _info;
        }

        private void DataGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {   
            
            var grid = sender as DataGrid;
            var text = e.EditingElement as TextBox;
            var item = grid?.SelectedItem as DataGridRowEx;
            string variable = item?.Variable;
            if (e.EditAction == DataGridEditAction.Cancel || item == null || text == null)
            {
                return;
            }
            if (e.EditAction == DataGridEditAction.Commit)
            {
                
                _parameters[variable] = text?.Text;
                int count = grid.Items.Count;
                if (e.Row.GetIndex() == count - 1)
                {
                    DoButton.Focus();
                }
            }
        }
    }

    public class DataGridRowEx : IComparable
    {
        private readonly int _num;
        public string Variable { get; set; }
        public string Value { get; set; }

        public DataGridRowEx(int num, string variable, string value)
        {
            _num = num;
            Variable = variable;
            Value = value;
        }

        public int CompareTo(object obj)
        {
            var sec = obj as DataGridRowEx;
            if (sec == null)
            {
                return -1;
            }
            if (_num == sec._num)
            {
                return 0;
            }
            if (_num > sec._num)
            {
                return 1;
            }
            return -1;
        }

    }
}

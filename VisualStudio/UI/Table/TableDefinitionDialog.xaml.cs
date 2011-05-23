using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VisualStudio.Data;

namespace VisualStudio.UI.Table
{
    public partial class TableDefinitionDialog : ITableDefinitionDialog
    {
        private Func<Data.Table, Maybe<Exception>> _createTable;
        private Func<Data.Table, Tuple<Maybe<Exception>, string>> _previewOperation;
        private readonly IData _data;
        private string _lastServerName;

        public TableDefinitionDialog(IData data)
        {
            InitializeComponent();
            _data = data;
        }

        private void CreateTable_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            var definition = (Data.Table)DataContext;
            var result = _createTable(definition);
            if (!result.HasValue)
            {
                System.Windows.MessageBox.Show("Successfully created table {0}.".FormatWith(definition.Name),
                                "Create Ultraviolet Catastrophe Table");
                DialogResult = true;
            }
            else System.Windows.MessageBox.Show("Error creating table {0}:\r\n\r\n{1}".FormatWith(definition.Name, result.Value.Message),
                                 "Create Ultraviolet Catastrophe Table");
        }

        private void Databases_DropDownOpened(object sender, EventArgs e)
        {
            if (_lastServerName == ServerName.Text) return;
            try
            {
                Databases.ItemsSource = _data.EnumerateDatabases(ServerName.Text).ToList();
            }
            catch (Exception exception)
            { System.Windows.MessageBox.Show("Error enumerating databases:\r\n\r\n" + exception.Message); }
                
            _lastServerName = ServerName.Text;
        }

        private void ServerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded) return;
            Databases.Items.Clear();
        }

        public Maybe<Data.Table> Display(string defaultTableName,
                                               Func<Data.Table, Maybe<Exception>> createTable,
                                               Func<Data.Table, Tuple<Maybe<Exception>, string>> previewOperation)
        {
            DataContext = new Data.Table { Server = "localhost", Name = defaultTableName };
            _createTable = createTable;
            _previewOperation = previewOperation;
            return ShowDialog() == true
                       ? Maybe.Just((Data.Table)DataContext)
                       : Maybe.Nothing<Data.Table>();
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            var result = _previewOperation((Data.Table)DataContext);
            new Preview().Display(this, result.Item1.HasValue ? result.Item1.Value.ToString() : result.Item2);
        }

        private bool Validate()
        {
            if (Databases.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("You need to select a database.", "Databases");
                return false;
            }
            return true;
        }
    }
}

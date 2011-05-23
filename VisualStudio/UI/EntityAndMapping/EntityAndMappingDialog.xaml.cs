using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VisualStudio.Data;
using VisualStudio.Workflows.EntityAndMapping;

namespace VisualStudio.UI.EntityAndMapping
{
    public partial class EntityAndMappingDialog : IEntityAndMappingDialog
    {
        private readonly IData _data;
        private Func<EntityDefinition, Maybe<Exception>> _createEntity;
        private Func<EntityDefinition, Maybe<Exception>> _createMapping;
        private Func<EntityDefinition, Tuple<Maybe<Exception>, string>> _previewEntityOperation;
        private Func<EntityDefinition, Tuple<Maybe<Exception>, string>> _previewMappingOperation;
        private string _lastServerName;
        private string _lastDatabaseName;

        public EntityAndMappingDialog(IData data)
        {
            InitializeComponent();
            _data = data;
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

            Tables.Items.Clear();
            _lastServerName = ServerName.Text;
        }

        private void Tables_DropDownOpened(object sender, EventArgs e)
        {
            if (_lastDatabaseName == Databases.Text) return;
            try
            {
                Tables.ItemsSource = _data.EnumerateTables(ServerName.Text, Databases.SelectedItem.ToString()).ToList();
            }
            catch (Exception exception)
            { System.Windows.MessageBox.Show("Error enumerating tables:\r\n\r\n" + exception.Message); }

            _lastDatabaseName = Databases.Text;
        }

        private void ServerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded) return;
            Databases.Items.Clear();
            Tables.Items.Clear();
        }

        public Maybe<EntityDefinition> Display(string defaultEntityName, 
                                        Func<EntityDefinition, Maybe<Exception>> createEntity,
                                        Func<EntityDefinition, Maybe<Exception>> createMapping,
                                        Func<EntityDefinition, Tuple<Maybe<Exception>, string>> previewEntityOperation,
                                        Func<EntityDefinition, Tuple<Maybe<Exception>, string>> previewMappingOperation)
        {
            _createEntity = createEntity;
            _createMapping = createMapping;
            _previewEntityOperation = previewEntityOperation;
            _previewMappingOperation = previewMappingOperation;
            DataContext = new EntityDefinition { Server = "localhost", EntityName = defaultEntityName };
            return ShowDialog() == true
                       ? Maybe.Just((EntityDefinition)DataContext)
                       : Maybe.Nothing<EntityDefinition>();
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            var entityResult = _previewEntityOperation((EntityDefinition)DataContext);
            var mappingResult = _previewMappingOperation((EntityDefinition)DataContext);
            new Preview().Display(this, (entityResult.Item1.HasValue ? entityResult.Item1.Value.ToString() : entityResult.Item2) +
                                        string.Format("\r\n\r\n{0}\r\n\r\n", new String('-', 50)) +
                                        (mappingResult.Item1.HasValue ? mappingResult.Item1.Value.ToString() : mappingResult.Item2));
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            var definition = (EntityDefinition)DataContext;
            var result = _createEntity(definition);
            if (!result.HasValue)
            {
                result = _createMapping(definition);
                if (!result.HasValue) DialogResult = true;
                else System.Windows.MessageBox.Show("Error creating mapping {0}:\r\n\r\n{1}".FormatWith(definition.Table, result.Value.Message),
                                                    "Create Ultraviolet Catastrophe Mapping");
            }
            else System.Windows.MessageBox.Show("Error creating entity {0}:\r\n\r\n{1}".FormatWith(definition.Table, result.Value.Message),
                                                "Create Ultraviolet Catastrophe Entity");
        }

        private bool Validate()
        {
            if (Databases.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("You need to select a database.", "Databases");
                return false;
            }
            if (Tables.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("You need to select a table.", "Tables");
                return false;
            }
            if (EntityName.Text.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("You need to enter an entity name.", "Entity Name");
                return false;
            }
            if (Namespace.Text.IsNullOrEmpty())
            {
                System.Windows.MessageBox.Show("You need to enter a namespace.", "Namespace");
                return false;
            }
            return true;
        }
    }
}

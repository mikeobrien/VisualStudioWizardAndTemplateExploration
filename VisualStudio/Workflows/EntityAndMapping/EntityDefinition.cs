using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VisualStudio.Workflows.EntityAndMapping
{
    public class EntityDefinition : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const string EntityNamespaceFormat = "Domain.{0}";
        private const string MappingNamespaceFormat = "Infrastructure.Persistence.Mapping.{0}";
        private const string EntityFullNameFormat = EntityNamespaceFormat + ".{1}";
        private const string MappingFullNameFormat = MappingNamespaceFormat + ".{1}Map";
        private const string MappingNameFormat = "{0}Map";

        private string _entityName;
        private string _namespace;

        public string Server { get; set; }
        public string Database { get; set; }
        public string Table { get; set; }

        public string EntityName
        {
            get { return _entityName; }
            set { PropertyHasChanged(() => _entityName = value, "EntityName", "EntityFullName", "MappingFullName"); }
        }
        public string EntityNamespace { get { return EntityNamespaceFormat.FormatWith(Namespace); } }

        public string MappingName { get { return MappingNameFormat.FormatWith(EntityName); } }
        public string MappingNamespace { get { return MappingNamespaceFormat.FormatWith(Namespace); } }

        public string Namespace
        {
            get { return _namespace; }
            set { PropertyHasChanged(() => _namespace = value, "Namespace", "EntityFullName", "MappingFullName"); }
        }

        public string EntityFullName { get { return EntityFullNameFormat.FormatWith(Namespace, EntityName); } }
        public string MappingFullName { get { return MappingFullNameFormat.FormatWith(Namespace, EntityName); } }

        public IDictionary<string, object> ToDictionary(string rootNamespace)
        {
            var parameters = new Dictionary<string, object>
                                 {
                                     {"entityName", EntityName},
                                     {"mappingName", MappingName},
                                     {"entityNamespace", "{0}.{1}".FormatWith(rootNamespace, EntityNamespace)},
                                     {"mappingNamespace", "{0}.{1}".FormatWith(rootNamespace, MappingNamespace)},
                                 };

            return parameters;
        }

        private void PropertyHasChanged(Action setValue, params string[] names)
        {
            setValue();
            if (PropertyChanged != null) 
                foreach (var name in names) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}

﻿using System;
using System.Collections.Generic;

namespace VisualStudio.Data
{
    public class Table
    {
        public Table()
        {
            Columns = new List<Column>();
        }

        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public IList<Column> Columns { get; set; }

        public IDictionary<string, object> ToDictionary()
        {
            var parameters = new Dictionary<string, object>
                                 {
                                     {"server", Server},
                                     {"database", Database},
                                     {"table", Name}
                                 };

            var columnNames = new List<string>();
            var columnSqlTypes = new List<string>();
            var columnClrTypes = new List<Type>();
            var columnDefaults = new List<string>();
            var columnLengths = new List<int>();
            var columnIsNullable = new List<bool>();
            var columnIsIdentity = new List<bool>();
            var columnIsAutoGenerated = new List<bool>();
            var columnIsPrimaryKey = new List<bool>();

            foreach (var column in Columns)
            {
                columnNames.Add(column.Name);
                columnSqlTypes.Add(column.SqlType.ToSqlTypeString());
                columnClrTypes.Add(column.ClrType);
                columnDefaults.Add(column.DefaultValue);
                columnLengths.Add(column.Length);
                columnIsNullable.Add(column.IsNullable);
                columnIsIdentity.Add(column.IsIdentity);
                columnIsAutoGenerated.Add(column.IsAutoGenerated);
                columnIsPrimaryKey.Add(column.IsPrimaryKey);
            }

            parameters.Add("columnNames", columnNames.ToArray());
            parameters.Add("columnSqlTypes", columnSqlTypes.ToArray());
            parameters.Add("columnClrTypes", columnClrTypes.ToArray());
            parameters.Add("columnDefaults", columnDefaults.ToArray());
            parameters.Add("columnLengths", columnLengths.ToArray());
            parameters.Add("columnIsNullable", columnIsNullable.ToArray());
            parameters.Add("columnIsIdentity", columnIsIdentity.ToArray());
            parameters.Add("columnIsAutoGenerated", columnIsAutoGenerated.ToArray());
            parameters.Add("columnIsPrimaryKey", columnIsPrimaryKey.ToArray());

            return parameters;
        }
    }
}
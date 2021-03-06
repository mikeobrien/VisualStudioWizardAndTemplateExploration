﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace VisualStudio.Data
{
    public class SqlServer : IData
    {
        public IEnumerable<string> EnumerateDatabases(string server)
        {
            return Execute(server, null, new SqlCommand("SELECT name FROM sys.databases WHERE name NOT IN ('tempdb', 'master', 'model', 'msdb') ORDER BY name"), x => (string)x[0]);
        }

        public IEnumerable<string> EnumerateTables(string server, string database)
        {
            return Execute(server, database, new SqlCommand("SELECT name FROM sys.tables WHERE type='U' ORDER BY name"), x => (string)x[0]);
        }

        public Table GetTableDefinition(string server, string database, string table)
        {
            var commandText = Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualStudio.Data.Columns.sql").ReadAllText();
            return new Table {
                                Server = server,
                                Database = database,
                                Name = table,
                                Columns = Execute(server, database,
                                                  new SqlCommand(string.Format(commandText, table)),
                                                  x => new Column
                                                            {
                                                                Name = (string) x[0],
                                                                SqlType = (SqlTypes) (byte) x[1],
                                                                Length = (short) x[2],
                                                                IsNullable = (bool) x[3],
                                                                IsIdentity = (bool) x[4],
                                                                IsAutoGenerated = (bool) x[5],
                                                                DefaultValue = x[6].AsString(),
                                                                IsPrimaryKey = (bool) x[7]
                                                            }).ToList()
                            };
        }

        public void Execute(string server, string database, string commandText)
        {
            Execute(server, database, new SqlCommand(commandText));
        }

        private static IEnumerable<T> Execute<T>(string server, string database, SqlCommand command, Func<IDataRecord, T> recordFactory)
        {
            using (var connection = new SqlConnection(GetConnectionString(server, database)))
            {
                connection.Open();
                command.Connection = connection;
                var reader = command.ExecuteReader();
                while (reader.Read()) yield return recordFactory(reader);
            }
        }

        private static void Execute(string server, string database, SqlCommand command)
        {
            using (var connection = new SqlConnection(GetConnectionString(server, database)))
            {
                connection.Open();
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }

        private static string GetConnectionString(string server, string database = null)
        {
            return string.Format("Trusted_Connection=True;server={0};", server) +
                (database == null ? string.Empty : "database=" + database);
        }
    }
}

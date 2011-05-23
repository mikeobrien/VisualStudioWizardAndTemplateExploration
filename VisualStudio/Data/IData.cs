using System.Collections.Generic;

namespace VisualStudio.Data
{
    public interface IData
    {
        IEnumerable<string> EnumerateDatabases(string server);
        IEnumerable<string> EnumerateTables(string server, string database);
        Table GetTableDefinition(string server, string database, string table);
        void Execute(string server, string database, string commandText);
    }
}

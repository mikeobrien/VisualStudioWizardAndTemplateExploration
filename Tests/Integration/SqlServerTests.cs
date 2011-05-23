using System.Linq;
using NUnit.Framework;
using Should;
using VisualStudio.Data;

namespace Tests.Integration
{
    [TestFixture]
    public class SqlServerTests
    { 
        [Test]
        public void Enumerate_Databases_On_A_Server()
        {
            var server = new SqlServer();
            var databases = server.EnumerateDatabases("localhost");
            databases.Count().ShouldNotEqual(0);
        }

        [Test]
        public void Enumerate_Tables_In_A_Database()
        {
            var server = new SqlServer();
            var databases = server.EnumerateTables("localhost", "master");
            databases.Count().ShouldNotEqual(0);
        }

        [Test]
        public void Execute_Arbitrary_Sql()
        {
            var server = new SqlServer();
            Assert.DoesNotThrow(() => server.Execute("localhost", "master", "SELECT COUNT(*) FROM sys.databases"));
        }

        [Test]
        public void Get_Table_Definition()
        {
            var server = new SqlServer();
            var table = server.GetTableDefinition("localhost", "master", "spt_fallback_db");
            table.ShouldNotBeNull();
            table.Columns.Count().ShouldNotEqual(0);
        }
    }
}

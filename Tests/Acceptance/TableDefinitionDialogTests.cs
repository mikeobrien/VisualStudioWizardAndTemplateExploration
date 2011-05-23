using System;
using NUnit.Framework;
using Should;
using VisualStudio;
using VisualStudio.Data;
using VisualStudio.UI.Table;

namespace Tests.Acceptance
{
    [TestFixture]
    public class TableDefinitionDialogTests
    {
        [Test]
        public void Should_Return_Ok_Result_When_Create_Table_Button_Is_Pressed()
        {
            var tableDefinition = new TableDefinitionDialog(new SqlServer());
            var result = tableDefinition.Display("DefaultTableName", 
                x => Maybe.Nothing<Exception>(),
                x => Tuple.Create(Maybe.Nothing<Exception>(), "SELECT * FROM Customers"));
            result.HasValue.ShouldBeTrue();
        }

        [Test]
        public void Should_Return_Cancel_Result_When_Cancel_Button_Is_Pressed()
        {
            var tableDefinition = new TableDefinitionDialog(new SqlServer());
            var result = tableDefinition.Display("DefaultTableName", 
                x => Maybe.Nothing<Exception>(),
                x => Tuple.Create(Maybe.Nothing<Exception>(), "SELECT * FROM Customers"));
            result.HasValue.ShouldBeFalse();
        }

        [Test]
        public void Should_Display_Error_Message_When_Create_Table_Button_Is_Pressed()
        {
            var tableDefinition = new TableDefinitionDialog(new SqlServer());
            var result = tableDefinition.Display("DefaultTableName", x => Maybe.Just(new Exception("Something really bad happended!")),
                x => Tuple.Create(Maybe.Just(new Exception("Something really bad happended!")), "SELECT * FROM Customers"));
            result.HasValue.ShouldBeFalse();
        }
    }
}

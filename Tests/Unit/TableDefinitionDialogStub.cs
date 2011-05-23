using System;
using VisualStudio;
using VisualStudio.Data;
using VisualStudio.UI.Table;

namespace Tests.Unit
{
    public class TableDefinitionDialogStub : ITableDefinitionDialog
    {
        private enum Operation { CreateTable, PreviewOperation }

        private readonly Operation _operation;

        private TableDefinitionDialogStub(Operation operation)
        {
            _operation = operation;
        }

        public static TableDefinitionDialogStub CreateTable()
        {
            return new TableDefinitionDialogStub(Operation.CreateTable);
        }

        public static TableDefinitionDialogStub PreviewOperation()
        {
            return new TableDefinitionDialogStub(Operation.PreviewOperation);
        }

        public Maybe<Table> Display(string defaultTableName, 
            Func<Table, Maybe<Exception>> createTable, 
            Func<Table, Tuple<Maybe<Exception>, string>> previewOperation)
        {
            var tableDefinition = new Table();
            switch (_operation)
            {
                case Operation.CreateTable: return createTable(tableDefinition).HasValue ?
                                                    Maybe.Just(tableDefinition) : Maybe.Nothing<Table>();
                case Operation.PreviewOperation: return previewOperation(tableDefinition).ToMaybe().Bind(x => x.Item1).HasValue ?
                                                    Maybe.Just(tableDefinition) : Maybe.Nothing<Table>();
            }
            return null;
        }
    }
}

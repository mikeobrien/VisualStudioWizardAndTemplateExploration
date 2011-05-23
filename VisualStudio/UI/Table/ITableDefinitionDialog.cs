using System;

namespace VisualStudio.UI.Table
{
    public interface ITableDefinitionDialog
    {
        Maybe<Data.Table> Display(string defaultTableName,
                                        Func<Data.Table, Maybe<Exception>> createTable,
                                        Func<Data.Table, Tuple<Maybe<Exception>, string>> previewOperation);
    }
}

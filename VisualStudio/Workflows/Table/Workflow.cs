using System;
using VisualStudio.Data;
using VisualStudio.Extensibility;
using VisualStudio.IO;
using VisualStudio.UI;
using VisualStudio.UI.Table;

namespace VisualStudio.Workflows.Table
{
    public class Workflow
    {
        public const string TableTemplateFilename = "Table.template";

        private readonly IData _data;
        private readonly IFileSystem _fileSystem;
        private readonly IMessageBox _messageBox;
        private readonly ITableDefinitionDialog _tableDefinitionDialog;
        private readonly ITemplateService _templateService;
        private readonly string _solutionPath;
        private string _templatePath;

        public Workflow(string solutionPath,
                             IData data, 
                             IFileSystem fileSystem, 
                             IMessageBox messageBox,
                             ITableDefinitionDialog tableDefinitionDialog, 
                             ITemplateService templateService)
        {
            _solutionPath = solutionPath;
            _data = data;
            _fileSystem = fileSystem;
            _messageBox = messageBox;
            _tableDefinitionDialog = tableDefinitionDialog;
            _templateService = templateService;
        }

        public void Run(string defaultTableName)
        {
            var result = _fileSystem.FindFirst(_solutionPath, TableTemplateFilename);
            if (result.HasValue)
            {
                _templatePath = result.Value;
                _tableDefinitionDialog.Display(defaultTableName, CreateTable, GenerateCommandText);
            }
            else _messageBox.DisplayMessage("Template Error", "Unable to find table creation template '{0}' under folder '{1}'.", TableTemplateFilename, _solutionPath);
        }

        private Maybe<Exception> CreateTable(Data.Table definition)
        {
            var templateResult = GenerateCommandText(definition);
            return templateResult.Item1.HasValue ? templateResult.Item1 : 
                _data.MaybeException(x => x.Execute(definition.Server, definition.Database, templateResult.Item2));
        }

        private Tuple<Maybe<Exception>, string> GenerateCommandText(Data.Table definition)
        {
            var result = _templateService.Render(_templatePath, definition.ToDictionary());
            return Tuple.Create(result.Success ? Maybe.Nothing<Exception>() :
                Maybe.Just<Exception>(CompositeException.Create(result.Errors)), result.Output);
        }
    }
}

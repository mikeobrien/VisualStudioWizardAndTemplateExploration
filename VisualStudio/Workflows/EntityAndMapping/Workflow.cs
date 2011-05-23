using System;
using System.IO;
using VisualStudio.Data;
using VisualStudio.Extensibility;
using VisualStudio.IO;
using VisualStudio.UI;
using VisualStudio.UI.EntityAndMapping;

namespace VisualStudio.Workflows.EntityAndMapping
{
    public class Workflow
    {
        public const string EntityTemplateFilename = "Entity.template";
        public const string MappingTemplateFilename = "EntityMapping.template";

        private readonly IData _data;
        private readonly IFileSystem _fileSystem;
        private readonly IMessageBox _messageBox;
        private readonly IEntityAndMappingDialog _entityAndMappingDialog;
        private readonly ITemplateService _templateService;
        private readonly string _solutionPath;
        private readonly string _projectPath;
        private string _entityTemplatePath;
        private string _mappingTemplatePath;
        private readonly string _defaultNamespace;

        public Workflow(string solutionPath, string projectPath, 
                        string defaultNamespace,
                        IData data, 
                        IFileSystem fileSystem, 
                        IMessageBox messageBox,
                        IEntityAndMappingDialog entityAndMappingDialog, 
                        ITemplateService templateService)
        {
            _solutionPath = solutionPath;
            _projectPath = projectPath;
            _defaultNamespace = defaultNamespace;
            _data = data;
            _fileSystem = fileSystem;
            _messageBox = messageBox;
            _entityAndMappingDialog = entityAndMappingDialog;
            _templateService = templateService;
        }

        public void Run(string defaultEntityName)
        {
            var result = _fileSystem.FindFirst(_solutionPath, EntityTemplateFilename);
            if (result.HasValue)
            {
                _entityTemplatePath = result.Value;
                result = _fileSystem.FindFirst(_solutionPath, MappingTemplateFilename);
                if (result.HasValue)
                {
                    _mappingTemplatePath = result.Value;
                    _entityAndMappingDialog.Display(defaultEntityName,
                                                    x => Create(x, _entityTemplatePath, x.EntityNamespace, x.EntityName),
                                                    x => Create(x, _mappingTemplatePath, x.MappingNamespace, x.MappingName),
                                                    x => Generate(x, _entityTemplatePath),
                                                    x => Generate(x, _mappingTemplatePath));
                }
                else _messageBox.DisplayMessage("Template Error", "Unable to find mapping creation template '{0}' under folder '{1}'.", MappingTemplateFilename, _solutionPath);
            }
            else _messageBox.DisplayMessage("Template Error", "Unable to find entity creation template '{0}' under folder '{1}'.", EntityTemplateFilename, _solutionPath);
        }

        private Maybe<Exception> Create(EntityDefinition definition, string templatePath, string @namespace, string filename)
        {
            var path = _projectPath.CombinePath(@namespace.Replace('.', Path.DirectorySeparatorChar)).CombinePath(filename).AppendExtension("cs");
            var table = _data.GetTableDefinition(definition.Server, definition.Database, definition.Table);
            var result = _templateService.Render(templatePath, path, definition.ToDictionary(_defaultNamespace).Concat(table.ToDictionary()));
            return result.Success ? 
                Maybe.Nothing<Exception>() : 
                Maybe.Just<Exception>(CompositeException.Create(result.Errors));
        }

        private Tuple<Maybe<Exception>, string> Generate(EntityDefinition definition, string templatePath)
        {
            var table = _data.GetTableDefinition(definition.Server, definition.Database, definition.Table);
            var result = _templateService.Render(templatePath, definition.ToDictionary(_defaultNamespace).Concat(table.ToDictionary()));
            return Tuple.Create(result.Success ? 
                Maybe.Nothing<Exception>() :
                Maybe.Just<Exception>(CompositeException.Create(result.Errors)), result.Output);
        }
    }
}

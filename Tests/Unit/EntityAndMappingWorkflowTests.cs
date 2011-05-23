using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using VisualStudio;
using VisualStudio.Data;
using VisualStudio.Extensibility;
using VisualStudio.IO;
using VisualStudio.UI;
using VisualStudio.UI.EntityAndMapping;
using VisualStudio.Workflows.EntityAndMapping;

namespace Tests.Unit
{
    [TestFixture]
    public class EntityAndMappingWorkflowTests
    {
        private const string SolutionPath = @"C:\Solution\";
        private const string ProjectPath = @"C:\Solution\Project\";

        [Test]
        public void Find_Template_Files_Are_Called_When_Run()
        {
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            fileSystem.Stub(x => x.FindFirst(null, null)).IgnoreArguments().Return(Maybe.Just(string.Empty));
            var workflow = new Workflow(SolutionPath, ProjectPath, string.Empty,
                                             MockRepository.GenerateStub<IData>(),
                                             fileSystem,
                                             MockRepository.GenerateStub<IMessageBox>(),
                                             MockRepository.GenerateStub<IEntityAndMappingDialog>(),
                                             MockRepository.GenerateStub<ITemplateService>());

            workflow.Run(null);

            fileSystem.AssertWasCalled(x => x.FindFirst(SolutionPath, Workflow.EntityTemplateFilename));
            fileSystem.AssertWasCalled(x => x.FindFirst(SolutionPath, Workflow.MappingTemplateFilename));
        }

        [Test]
        public void Dialog_Is_Displayed_When_Run_And_Template_File_Found()
        {
            var dialog = MockRepository.GenerateStub<IEntityAndMappingDialog>();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            fileSystem.Stub(x => x.FindFirst(null, null)).IgnoreArguments().Return(Maybe.Just(string.Empty));
            var workflow = new Workflow(null, null, string.Empty,
                                             MockRepository.GenerateStub<IData>(),
                                             fileSystem,
                                             MockRepository.GenerateStub<IMessageBox>(),
                                             dialog,
                                             MockRepository.GenerateStub<ITemplateService>());
            workflow.Run(null);

            dialog.AssertWasCalled(x => x.Display(null, null, null, null, null), x => x.IgnoreArguments());
        }

        [Test]
        public void Dialog_Is_Not_Displayed_When_Run_And_Template_File_Is_Not_Found()
        {
            var dialog = MockRepository.GenerateStub<IEntityAndMappingDialog>();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            fileSystem.Stub(x => x.FindFirst(null, null)).IgnoreArguments().Return(Maybe.Nothing<string>());
            var workflow = new Workflow(null, null, string.Empty,
                                             MockRepository.GenerateStub<IData>(),
                                             fileSystem,
                                             MockRepository.GenerateStub<IMessageBox>(),
                                             dialog,
                                             MockRepository.GenerateStub<ITemplateService>());
            workflow.Run(null);

            dialog.AssertWasNotCalled(x => x.Display(null, null, null, null, null), x => x.IgnoreArguments());
        }

        [Test]
        public void Render_Is_Called_When_Dialog_Runs_Preview_Operation()
        {
            var templateService = MockRepository.GenerateStub<ITemplateService>();
            templateService.Stub(x => x.Render(null, null)).IgnoreArguments().Return(new TransformResult(null, Enumerable.Empty<Exception>()));

            var databaseService = MockRepository.GenerateStub<IData>();
            databaseService.Stub(x => x.GetTableDefinition(null, null, null)).IgnoreArguments().Return(new Table());

            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            fileSystem.Stub(x => x.FindFirst(null, null)).IgnoreArguments().Return(Maybe.Just(string.Empty));

            var workflow = new Workflow(null, string.Empty, string.Empty,
                                             databaseService,
                                             fileSystem,
                                             MockRepository.GenerateStub<IMessageBox>(),
                                             EntityAndMappingDialogStub.PreviewOperation(),
                                             templateService);
            workflow.Run("default");

            templateService.AssertWasCalled(x => x.Render(null, null), x => x.IgnoreArguments());
        }

        [Test]
        public void Render_Is_Called_When_Dialog_Creates_Files()
        {
            var templateService = MockRepository.GenerateStub<ITemplateService>();
            templateService.Stub(x => x.Render(null, null, null)).IgnoreArguments().Return(new TransformResult(null, Enumerable.Empty<Exception>()));

            var databaseService = MockRepository.GenerateStub<IData>();
            databaseService.Stub(x => x.GetTableDefinition(null, null, null)).IgnoreArguments().Return(new Table());
            
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            fileSystem.Stub(x => x.FindFirst(null, null)).IgnoreArguments().Return(Maybe.Just(string.Empty));

            var workflow = new Workflow(null, string.Empty, string.Empty,
                                             databaseService,
                                             fileSystem,
                                             MockRepository.GenerateStub<IMessageBox>(),
                                             EntityAndMappingDialogStub.Create(),
                                             templateService);
            workflow.Run("default");

            templateService.AssertWasCalled(x => x.Render(null, null, null), x => x.IgnoreArguments());
        }

        [Test]
        public void Get_Columns_When_Dialog_Previews_Entity_And_Mapping()
        {
            var templateService = MockRepository.GenerateStub<ITemplateService>();
            templateService.Stub(x => x.Render(null, null)).IgnoreArguments().Return(new TransformResult(null, Enumerable.Empty<Exception>()));

            var databaseService = MockRepository.GenerateStub<IData>();
            databaseService.Stub(x => x.GetTableDefinition(null, null, null)).IgnoreArguments().Return(new Table());

            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            fileSystem.Stub(x => x.FindFirst(null, null)).IgnoreArguments().Return(Maybe.Just(string.Empty));

            var workflow = new Workflow(null, null, string.Empty,
                                             databaseService,
                                             fileSystem,
                                             MockRepository.GenerateStub<IMessageBox>(),
                                             EntityAndMappingDialogStub.PreviewOperation(),
                                             templateService);
            workflow.Run("default");

            databaseService.AssertWasCalled(x => x.GetTableDefinition(null, null, null), x => x.IgnoreArguments());
        }

        [Test]
        public void Get_Columns_When_Dialog_Creates_Entity_And_Mapping()
        {
            var templateService = MockRepository.GenerateStub<ITemplateService>();
            templateService.Stub(x => x.Render(null, null, null)).IgnoreArguments().Return(new TransformResult(null, Enumerable.Empty<Exception>()));

            var databaseService = MockRepository.GenerateStub<IData>();
            databaseService.Stub(x => x.GetTableDefinition(null, null, null)).IgnoreArguments().Return(new Table());

            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            fileSystem.Stub(x => x.FindFirst(null, null)).IgnoreArguments().Return(Maybe.Just(string.Empty));

            var workflow = new Workflow(null, string.Empty, string.Empty,
                                             databaseService,
                                             fileSystem,
                                             MockRepository.GenerateStub<IMessageBox>(),
                                             EntityAndMappingDialogStub.Create(),
                                             templateService);
            workflow.Run("default");

            databaseService.AssertWasCalled(x => x.GetTableDefinition(null, null, null), x => x.IgnoreArguments());
        }

        [Test]
        public void Dont_Create_Files_When_Template_Creation_Is_Not_Successful()
        {
            var templateService = MockRepository.GenerateStub<ITemplateService>();
            templateService.Stub(x => x.Render(null, null, null)).IgnoreArguments().Return(new TransformResult(null, Enumerable.Repeat(new Exception(), 1)));

            var databaseService = MockRepository.GenerateStub<IData>();
            databaseService.Stub(x => x.GetTableDefinition(null, null, null)).IgnoreArguments().Return(new Table());

            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            fileSystem.Stub(x => x.FindFirst(null, null)).IgnoreArguments().Return(Maybe.Just(string.Empty));

            var workflow = new Workflow(null, string.Empty, string.Empty,
                                             databaseService,
                                             fileSystem,
                                             MockRepository.GenerateStub<IMessageBox>(),
                                             EntityAndMappingDialogStub.Create(),
                                             templateService);
            workflow.Run("default");

            databaseService.AssertWasNotCalled(x => x.Execute(null, null, null), x => x.IgnoreArguments());
        }
    }
}

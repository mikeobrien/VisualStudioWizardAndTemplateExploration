using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using VisualStudio.Data;
using VisualStudio.IO;
using VisualStudio.UI;
using VisualStudio.UI.EntityAndMapping;
using VisualStudio.Workflows.EntityAndMapping;

namespace VisualStudio.Extensibility
{
    public class EntityAndMappingWizard : IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            var context = new Context(automationObject, replacementsDictionary);
            var workflow = new Workflow(context.SolutionPath, 
                                        context.ProjectPath, 
                                        context.DefaultNamespace,
                                        new SqlServer(), 
                                        new FileSystem(),
                                        new MessageBox(),
                                        new EntityAndMappingDialog(new SqlServer()),
                                        context.TemplateService);
            workflow.Run(context.RootName);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem) { }
        public void ProjectFinishedGenerating(EnvDTE.Project project) { }
        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem) { }
        public void RunFinished() { }
    }
}

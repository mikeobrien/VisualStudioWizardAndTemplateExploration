using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using VisualStudio.Data;
using VisualStudio.IO;
using VisualStudio.UI;
using VisualStudio.UI.Table;
using VisualStudio.Workflows.Table;

namespace VisualStudio.Extensibility
{
    public class TableWizard : IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            var context = new Context(automationObject, replacementsDictionary);
            var workflow = new Workflow(context.SolutionPath,
                                             new SqlServer(), 
                                             new FileSystem(),
                                             new MessageBox(),
                                             new TableDefinitionDialog(new SqlServer()),
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

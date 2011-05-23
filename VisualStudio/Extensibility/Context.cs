using System;
using System.Collections.Generic;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace VisualStudio.Extensibility
{
    public class Context
    {
        private readonly IDictionary<string, string> _replacementValues;
        private readonly DTE _environment;

        public Context(object automationObject, IDictionary<string, string> values)
        {
            _replacementValues = values;
            _environment = (DTE)automationObject;
            ServiceProvider = new ServiceProvider(_environment as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            TemplateService = new TemplateService(this);
        }

        public string SolutionPath { get { return Path.GetDirectoryName(_environment.Solution.FullName); } }
        public string ProjectPath { get { return Path.GetDirectoryName(ActiveProject.FullName); } }
        public Project ActiveProject { get { return _environment.ActiveSolutionProjects.GetValue(0) as Project; } }
        public string DefaultNamespace { get { return ActiveProject.Properties.Item("DefaultNamespace").Value; } }
        public IServiceProvider ServiceProvider { get; private set; }
        public ITemplateService TemplateService { get; private set; }
        public string Guid1 { get { return _replacementValues["$guid1$"]; } }
        public string Guid2 { get { return _replacementValues["$guid2$"]; } }
        public string Guid3 { get { return _replacementValues["$guid3$"]; } }
        public string Guid4 { get { return _replacementValues["$guid4$"]; } }
        public string Guid5 { get { return _replacementValues["$guid5$"]; } }
        public string Guid6 { get { return _replacementValues["$guid6$"]; } }
        public string Guid7 { get { return _replacementValues["$guid7$"]; } }
        public string Guid8 { get { return _replacementValues["$guid8$"]; } }
        public string Guid9 { get { return _replacementValues["$guid9$"]; } }
        public string Guid10 { get { return _replacementValues["$guid10$"]; } }
        public string Time { get { return _replacementValues["$time$"]; } }
        public string Year { get { return _replacementValues["$year$"]; } }
        public string Username { get { return _replacementValues["$username$"]; } }
        public string UserDomain { get { return _replacementValues["$userdomain$"]; } }
        public string MachineName { get { return _replacementValues["$machinename$"]; } }
        public string ClrVersion { get { return _replacementValues["$clrversion$"]; } }
        public string RegisteredOrganization { get { return _replacementValues["$registeredorganization$"]; } }
        public string RunSilent { get { return _replacementValues["$runsilent$"]; } }
        public string RootName { get { return _replacementValues["$rootname$"]; } }
        public string TargetFramework { get { return _replacementValues["$targetframeworkversion$"]; } }
    }
}

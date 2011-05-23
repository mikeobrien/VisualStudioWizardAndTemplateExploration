using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace VisualStudio.Extensibility
{
    public class TemplateService : ITemplateService
    {
        private readonly Context _context;

        public TemplateService(Context context)
        {
            _context = context;
        }

        public TransformResult Render(string templatePath, IDictionary<string, object> parameters)
        {
            try
            {
                if (!File.Exists(templatePath)) throw new FileNotFoundException("Template file not found.", templatePath);

                var serviceProvider = _context.ServiceProvider;

                var templating = serviceProvider.GetService(typeof(STextTemplating)) as ITextTemplating;
                var sessionHost = templating as ITextTemplatingSessionHost;
                
                sessionHost.Session = sessionHost.CreateSession();
                foreach (var parameter in parameters) sessionHost.Session[parameter.Key] = parameter.Value;

                var transformContext = new TransformContext();
                
                var output = templating.ProcessTemplate(templatePath, File.ReadAllText(templatePath), transformContext);

                return new TransformResult(output, transformContext.Errors.Select(x => new Exception(x)));
            }
            catch (Exception e)
            {
                return new TransformResult(null, new [] {e});
            }
        }

        public TransformResult Render(string templatePath, string outputPath, IDictionary<string, object> parameters)
        {
            var result = Render(templatePath, parameters);
            if (result.Success)
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                    File.WriteAllText(outputPath, result.Output);
                    _context.ActiveProject.ProjectItems.AddFromFile(outputPath);
                }
                catch (Exception e)
                {
                    result.Errors.Add(e);
                }
            }
            return result;
        }

        private class TransformContext : ITextTemplatingCallback
        {
            private readonly List<string> _errors = new List<string>();

            public IEnumerable<string> Errors { get { return _errors; } }
            public string FileExtension { get; set; }
            public Encoding OutputEncoding { get; set; }

            public void ErrorCallback(bool warning, string message, int line, int column) { _errors.Add(message); }
            public void SetFileExtension(string extension) { FileExtension = extension; }
            public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective) { OutputEncoding = encoding; }
        }
    }
}

using System.Collections.Generic;

namespace VisualStudio.Extensibility
{
    public interface ITemplateService
    {
        TransformResult Render(string templatePath, IDictionary<string, object> parameters);
        TransformResult Render(string templatePath, string outputPath, IDictionary<string, object> parameters);
    }
}

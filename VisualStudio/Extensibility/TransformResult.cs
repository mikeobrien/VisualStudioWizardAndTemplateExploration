using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualStudio.Extensibility
{
    public class TransformResult
    {
        public TransformResult(string output, IEnumerable<Exception> errors)
        {
            Output = output;
            Errors = errors.ToList();
        }

        public string Output { get; private set; }
        public bool Success { get { return !Errors.Any(); } }
        public IList<Exception> Errors { get; private set; }
    }
}

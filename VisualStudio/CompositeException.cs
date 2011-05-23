using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualStudio
{
    public class CompositeException : Exception
    {
        public CompositeException(IEnumerable<Exception> exceptions) :
            base(CreateCompositeMessage(exceptions))
        {
            Exceptions = exceptions;
        }

        public IEnumerable<Exception> Exceptions { get; private set; }

        public static CompositeException Create(IEnumerable<Exception> exceptions)
        {
            return new CompositeException(exceptions);
        }

        private static string CreateCompositeMessage(IEnumerable<Exception> exceptions)
        {
            return exceptions.Select(x => x.Message).Aggregate((a, x) => a + "\r\n\r\n" + x);
        }
    }
}

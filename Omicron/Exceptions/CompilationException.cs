using System;

namespace Omicron.Exceptions
{
    [Serializable]
    public class CompilationException : Exception
    {
        public CompilationException(string message, int lineNumber) : base(message)
        {
            
        }
    }
}

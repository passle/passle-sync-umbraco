using System;

namespace PassleSync.Core.Exceptions
{
    public class PassleException : Exception
    {
        public PassleException() { }

        public PassleException(Type type, PassleExceptionEnum code) : base(string.Format("Failed to get {0} from the API: Code {1}", type.Name, (int)code)) { }
    }
}

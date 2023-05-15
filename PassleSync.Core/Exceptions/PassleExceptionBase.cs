using System;

namespace PassleSync.Core.Exceptions
{
    public abstract class PassleExceptionBase : Exception
    {
        public PassleExceptionBase(Type type, PassleExceptionCode code)
            : base(string.Format("Failed to get {0} from the API: Code {1}", type.Name, code)) 
        { }
    }
}

using System;

namespace PassleSync.Core.Exceptions
{
    public class PassleUnknownException : PassleExceptionBase
    {
        public PassleUnknownException(Type type)
            : base(type, PassleExceptionCode.UNKNOWN) 
        { } 
    }
}

using System;

namespace PassleSync.Core.Exceptions
{
    public class PassleAPINullException : PassleExceptionBase
    {
        public PassleAPINullException(Type type)
            : base(type, PassleExceptionCode.NULL_FROM_API)
        { }
    }
}

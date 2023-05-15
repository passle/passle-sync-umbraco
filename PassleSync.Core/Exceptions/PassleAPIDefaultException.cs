using System;

namespace PassleSync.Core.Exceptions
{
    public class PassleAPIDefaultException : PassleExceptionBase
    {
        public PassleAPIDefaultException(Type type) 
            : base(type, PassleExceptionCode.DEFAULT_FROM_API)
        { }
    }
}

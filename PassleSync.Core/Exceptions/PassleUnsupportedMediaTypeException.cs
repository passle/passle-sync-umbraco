using System;

namespace PassleSync.Core.Exceptions
{
    public class PassleUnsupportedMediaTypeException : PassleExceptionBase
    {
        public PassleUnsupportedMediaTypeException(Type type)
            : base(type, PassleExceptionCode.UNSUPPORTED_MEDIA_TYPE) 
        { }
    }
}

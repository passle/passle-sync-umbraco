namespace PassleSync.Core.Exceptions
{
    public enum PassleExceptionEnum
    {
        UNKNOWN = 0,
        NULL_FROM_API = 1,          // Passle doesn't exist, etc
        DEFAULT_FROM_API = 2,
        UNSUPPORTED_MEDIA_TYPE = 3  // Unauthorized / endpoint doesn't exist
    }
}

using System;

namespace PassleSync.Core.Models.Content.PassleApi
{
    [Flags]
    public enum PostShareButtons
    {
        None = 0,
        LinkedIn = 1 << 0,
        Twitter = 1 << 1,
        Facebook = 1 << 2,
        [Obsolete]
        GooglePlus = 1 << 3,
        Xing = 1 << 4,
        Email = 1 << 5,
        DefaultShareButtons = LinkedIn | Twitter | Facebook
    }
}

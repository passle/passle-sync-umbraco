namespace PassleSync.Core.API.SyncHandlers
{
    public interface ISyncHandler
    {
        bool SyncOne(string Shortcode);
    }
}

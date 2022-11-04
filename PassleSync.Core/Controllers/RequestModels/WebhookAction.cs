namespace PassleSync.Core.Controllers.RequestModels
{
    public enum WebhookAction
    {
        SYNC_POST = 1,
        DELETE_POST = 2,
        SYNC_AUTHOR = 3,
        DELETE_AUTHOR = 4,
        UPDATE_FEATURED_POST = 5,
        PING = 6
    }
}

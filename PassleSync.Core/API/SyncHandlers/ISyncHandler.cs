using System.Collections.Generic;

namespace PassleSync.Core.API.SyncHandlers
{
    public interface ISyncHandler<T>
    {
        bool SyncOne(string Shortcode);
        bool SyncMany(string[] Shortcodes);
        bool SyncAll();
        void DeleteMany(string[] Shortcodes, int parentNodeId);
        void DeleteAll(int parentNodeId);
        void CreateOne(T post, int parentNodeId);
        void CreateMany(IEnumerable<T> posts, int parentNodeId);
    }
}

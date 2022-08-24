using PassleSync.Core.API.ViewModels;
using System.Collections.Generic;

namespace PassleSync.Core.API.SyncHandlers
{
    public interface ISyncHandler<T>
    {
        IPassleDashboardViewModel GetAll();
        bool SyncOne(string Shortcode);
        bool SyncMany(string[] Shortcodes);
        bool SyncAll();
        bool DeleteMany(string[] Shortcodes);
        void DeleteMany(string[] Shortcodes, int parentNodeId);
        void DeleteAll(int parentNodeId);
        bool DeleteAll();
        void CreateOne(T post, int parentNodeId);
        void CreateMany(IEnumerable<T> posts, int parentNodeId, string[] shortcodes);
        void CreateAll(IEnumerable<T> posts, int parentNodeId);
    }
}

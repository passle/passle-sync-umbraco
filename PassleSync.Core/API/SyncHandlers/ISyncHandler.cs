using PassleSync.Core.API.ViewModels;
using System.Collections.Generic;

namespace PassleSync.Core.API.SyncHandlers
{
    public interface ISyncHandler<T>
    {
        IPassleDashboardViewModel GetAll();
        bool SyncOne(string shortcode);
        bool SyncMany(string[] shortcodes);
        bool SyncAll();
        bool DeleteMany(string[] shortcodes);
        void DeleteMany(string[] shortcodes, int parentNodeId);
        void DeleteAll(int parentNodeId);
        bool DeleteAll();
        void CreateOne(T post, int parentNodeId);
        void CreateMany(IEnumerable<T> entities, int parentNodeId, string[] shortcodes);
        void CreateAll(IEnumerable<T> entities, int parentNodeId);
    }
}

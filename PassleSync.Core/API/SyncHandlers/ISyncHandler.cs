using PassleSync.Core.API.ViewModels;
using System.Collections.Generic;

namespace PassleSync.Core.API.SyncHandlers
{
    public interface ISyncHandler<T>
    {
        IPassleDashboardViewModel GetAll();
        string Shortcode(T item);
        void SyncOne(string shortcodes);
        void SyncMany(string[] shortcodes);
        void SyncAll();
        void DeleteOne(string shortcode);
        void DeleteMany(string[] shortcodes);
        void DeleteAll();
        void CreateOne(T post);
        void CreateMany(IEnumerable<T> posts, string[] shortcodes);
        void CreateAll(IEnumerable<T> posts);
    }
}

using PassleSync.Core.API.ViewModels;
using System.Collections.Generic;

namespace PassleSync.Core.API.SyncHandlers
{
    public interface ISyncHandler<T>
    {
        IPassleDashboardViewModel GetAll();
        IPassleDashboardViewModel GetExisting();
        string Shortcode(T entity);
        void SyncOne(string shortcode);
        void SyncMany(string[] shortcodes);
        void SyncAll();
        void DeleteOne(string shortcode);
        void DeleteMany(string[] shortcodes);
        void DeleteAll();
        void CreateOne(T entity);
        void CreateMany(IEnumerable<T> entities, string[] shortcodes);
        void CreateAll(IEnumerable<T> entities);
    }
}

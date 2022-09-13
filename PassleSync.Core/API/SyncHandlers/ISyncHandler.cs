using PassleSync.Core.API.ViewModels;
using PassleSync.Core.SyncHandlers;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace PassleSync.Core.API.SyncHandlers
{
    public interface ISyncHandler<T>
    {
        IPassleDashboardViewModel GetAll();
        IPassleDashboardViewModel GetExisting();
        string Shortcode(T entity);
        SyncTaskResult SyncOne(string shortcode);
        IEnumerable<SyncTaskResult> SyncMany(string[] shortcodes);
        IEnumerable<SyncTaskResult> SyncAll();
        SyncTaskResult UpdateOrCreateOne(T entity);
        SyncTaskResult DeleteOne(string shortcode);
        IEnumerable<SyncTaskResult> DeleteMany(string[] shortcodes);
        IEnumerable<SyncTaskResult> DeleteAll();
        SyncTaskResult CreateOne(T entity);
        IEnumerable<SyncTaskResult> CreateMany(IEnumerable<T> entities, string[] shortcodes);
        IEnumerable<SyncTaskResult> CreateAll(IEnumerable<T> entities);
    }
}

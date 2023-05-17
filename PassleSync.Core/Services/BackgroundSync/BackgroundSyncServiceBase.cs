using System.Collections.Generic;
using System.Linq;

namespace PassleSync.Core.Services.API
{
    public class BackgroundSyncServiceBase<T> where T : class
    {
        private IEnumerable<string> _itemsToSync = new List<string>();
        private IEnumerable<string> _itemsToDelete = new List<string>();

        public void AddItemsToSync(IEnumerable<string> itemsToAdd)
        {
            _itemsToSync = _itemsToSync.Union(itemsToAdd);
        }

        public void AddItemsToDelete(IEnumerable<string> itemsToAdd)
        {
            _itemsToDelete= _itemsToDelete.Union(itemsToAdd);
        }

        public void RemoveItemsToSync(IEnumerable<string> itemsToRemove)
        {
            _itemsToSync = _itemsToSync.Where(x => !itemsToRemove.Contains(x));
        }

        public void RemoveItemsToDelete(IEnumerable<string> itemsToRemove)
        {
            _itemsToDelete = _itemsToDelete.Where(x => !itemsToRemove.Contains(x));
        }

        public IEnumerable<string> GetItemsToSync()
        {
            return _itemsToSync;
        }

        public IEnumerable<string> GetItemsToDelete()
        {
            return _itemsToDelete;
        }
    }
}

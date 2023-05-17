using System.Collections.Generic;

namespace PassleSync.Core.Controllers.ResponseModels
{
    public class SyncStatusResponseModel
    {
        public IEnumerable<string> ToSync;
        public IEnumerable<string> ToDelete;

        public SyncStatusResponseModel(IEnumerable<string> toSync, IEnumerable<string> toDelete)
        {
            ToSync = toSync;
            ToDelete = toDelete;
        }
    }
}

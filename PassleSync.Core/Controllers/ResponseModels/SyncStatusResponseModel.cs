using PassleSync.Core.Constants;
using System.Collections.Generic;

namespace PassleSync.Core.Controllers.ResponseModels
{
    public class SyncStatusResponseModel
    {
        public List<string> ToSync;
        public List<string> ToDelete;

        public SyncStatusResponseModel(List<string> toSync, List<string> toDelete)
        {
            ToSync = toSync;
            ToDelete = toDelete;
        }
    }
}

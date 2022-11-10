using PassleSync.Core.API.ViewModels;
using System.Collections.Generic;

namespace PassleSync.Core.ViewModels.PassleDashboard
{
    public class PassleDashboardTagsViewModel : IPassleDashboardViewModel
    {
        public IEnumerable<PassleDashboardTagViewModel> Tags;

        public PassleDashboardTagsViewModel(IEnumerable<PassleDashboardTagViewModel> tags)
        {
            Tags = tags;
        }
    }
}

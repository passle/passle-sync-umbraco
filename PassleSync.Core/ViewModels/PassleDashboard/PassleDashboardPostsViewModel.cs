using PassleSync.Core.API.ViewModels;
using System.Collections.Generic;

namespace PassleSync.Core.ViewModels.PassleDashboard
{
    public class PassleDashboardPostsViewModel : IPassleDashboardViewModel
    {
        public IEnumerable<PassleDashboardPostViewModel> Posts;

        public PassleDashboardPostsViewModel(IEnumerable<PassleDashboardPostViewModel> posts)
        {
            Posts = posts;
        }
    }
}

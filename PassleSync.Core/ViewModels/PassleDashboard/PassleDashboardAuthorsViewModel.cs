using PassleSync.Core.API.ViewModels;
using System.Collections.Generic;

namespace PassleSync.Core.ViewModels.PassleDashboard
{
    public class PassleDashboardAuthorsViewModel : IPassleDashboardViewModel
    {
        public IEnumerable<PassleDashboardAuthorViewModel> Authors;

        public PassleDashboardAuthorsViewModel(IEnumerable<PassleDashboardAuthorViewModel> authors)
        {
            Authors = authors;
        }
    }
}

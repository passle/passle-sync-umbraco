using PassleSync.Core.API.ViewModels;
using System;

namespace PassleSync.Core.ViewModels.PassleDashboard
{
    public class PassleDashboardErrorViewModel : IPassleDashboardViewModel
    {
        public string ErrorMsg;

        public PassleDashboardErrorViewModel(Exception ex)
        {
            ErrorMsg = ex.Message;
        }
    }
}

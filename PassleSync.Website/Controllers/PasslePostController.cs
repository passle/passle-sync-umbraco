using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using PassleSync.Core.Models.Content.Umbraco;

namespace PassleSync.Website.Controllers
{
    public class PasslePostController : RenderMvcController
    {
        public override ActionResult Index(ContentModel model)
        {
            var viewModel = new PasslePost(model.Content);

            return CurrentTemplate(viewModel);
        }
    }
}

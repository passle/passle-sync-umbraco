using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Web;

namespace PassleDotCom.Partner.Core.Models
{
    public partial class MasterBasePage
    {
        private HomePage _homePage = null;

        public string MetaDisplayTitle
        {
            get
            {
                //var pageTitle = MetaTitle.IfNullOrWhiteSpace(Name);
                //var siteName = this.Site().OfType<HomePage>().SiteName;
                //if (!String.IsNullOrEmpty(siteName))
                //{
                //    pageTitle = pageTitle.EnsureStartsWith(" | ");
                //}

                //return String.Format("{0}{1}", siteName.IfNullOrWhiteSpace(String.Empty), pageTitle);

                throw new NotImplementedException();
            }
        }

        public HomePage SiteRoot
        {
            get
            {
                //if (_homePage == null)
                //    _homePage = this.AncestorOrSelf<HomePage>();
                //return _homePage;

                throw new NotImplementedException();
            }
        }

        public IEnumerable<NavigationItem> TopNavItems
        {
            get
            {
                //var navContainer = this.SiteRoot.Descendant<NavigationContainer>();
                //return navContainer.Children<NavigationItem>().Where(n => n.NavigationPicker != null);

                throw new NotImplementedException();
            }
        }
        public NavigationContainer NavigationRoot
        {
            get
            {
                //return this.SiteRoot.Descendant<NavigationContainer>();

                throw new NotImplementedException();
            }
        }

        public IEnumerable<NavigationItem> AllNavItems
        {
            get
            {
                //var navContainer = this.SiteRoot.Descendant<NavigationContainer>();
                //return navContainer.Descendants<NavigationItem>().Where(n => n.NavigationPicker != null);

                throw new NotImplementedException();
            }
        }
    }
}

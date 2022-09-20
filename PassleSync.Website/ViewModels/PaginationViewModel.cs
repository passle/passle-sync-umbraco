using PassleSync.Website.Extensions;
using System;
using System.Collections.Generic;

namespace PassleSync.Website.ViewModels
{
    public class PaginationViewModel
    {
        public PaginationViewModel(int currentPage, int totalPages, Uri url)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            Url = url;
            Links = new List<PaginationLink>();

            InitialiseLinks();
        }

        public int CurrentPage;
        public int TotalPages;
        public Uri Url;
        public readonly List<PaginationLink> Links;

        private void InitialiseLinks()
        {
            var endSize = 1;
            var midSize = 2;
            var dots = false;

            for (var i = 1; i <= TotalPages; i++)
            {
                if (i == CurrentPage ||
                    i <= endSize || (CurrentPage > 0 && i >= CurrentPage - midSize && i <= CurrentPage + midSize) || i > TotalPages - endSize)
                {
                    Links.Add(new PaginationLink()
                    {
                        Url = Url.SetParameter("page", i.ToString()).ToString(),
                        Label = i.ToString(),
                        IsCurrent = i == CurrentPage,
                    });

                    dots = true;
                }
                else if (dots)
                {
                    Links.Add(new PaginationLink()
                    {
                        Label = "…",
                    });

                    dots = false;
                }
            }
        }
    }

    public class PaginationLink
    {
        public string Url;
        public string Label;
        public bool IsCurrent;
    }
}

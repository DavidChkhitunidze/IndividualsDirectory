using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Models.ResourceControls
{
    public class PagedList<TViewModel> where TViewModel : class
    {
        public PagedList(int totalItems, int pageNumber, int pageSize, IQueryable<TViewModel> items)
        {
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Items = items;
        }

        public int TotalItems { get; private set; }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public IQueryable<TViewModel> Items { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public int NextPageNumber => HasNextPage ? PageNumber + 1 : TotalPages;

        public int PreviousPageNumber => HasPreviousPage ? PageNumber - 1 : 1;
    }
}

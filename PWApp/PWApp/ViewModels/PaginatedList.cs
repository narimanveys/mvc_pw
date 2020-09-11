using System;
using System.Collections.Generic;
using System.Linq;

namespace PWApp.ViewModels
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);

            AddRange(items);
        }

        public int PageIndex { get; }
        public int TotalPages { get; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginatedList<T> CreateAsync(IList<T> source, int pageIndex, int pageSize)
        {
            var queryableSource = source.AsQueryable();
            var count = queryableSource.Count();
            var items = queryableSource.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var result = new PaginatedList<T>(items, count, pageIndex, pageSize);
            return result;
        }
    }
}
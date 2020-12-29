using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StarWarsApi.Models
{
    public class PaginatedList<T> : List<T>
    {
        #region public properties

        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPrevousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }


        #endregion

        #region Constructor

        public PaginatedList(List<T> data, int totalRecords, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            this.AddRange(data);
        }

        #endregion

        #region Methods

        public static async Task<PaginatedList<T>> Create(IQueryable<T> source, int? pageIndex, int? pageSize)
        {
            var totalRecords = await source.CountAsync();
            var data = new List<T>();
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                data = await source
                .Skip(((int)pageIndex - 1) * (int)pageSize)
                .Take((int)pageSize)
                .ToListAsync();
            } else
            {
                data = await source.ToListAsync();
            }
            return new PaginatedList<T>(data, totalRecords, pageIndex ?? 1, pageSize ?? totalRecords);
        }

        #endregion

    }
}

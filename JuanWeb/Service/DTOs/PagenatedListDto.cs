using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class PagenatedListDto<T> : List<T>
    {
        public PagenatedListDto(List<T> items, int count, int pageIndex, int pageSize)
        {
            this.AddRange(items);
            PageIndex = pageIndex;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
        }

        public int TotalPage { get; set; }

        public int PageIndex { get; set; }

        public bool HasPrev
        {
            get => PageIndex > 1;
        }

        public bool HasNext
        {
            get => TotalPage > PageIndex;
        }

        public static PagenatedListDto<T> Save(IQueryable<T> query, int pageIndex, int pageSize)
        {
            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagenatedListDto<T>(items, query.Count(), pageIndex, pageSize);
        }
    }
}

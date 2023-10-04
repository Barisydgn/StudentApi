using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PageList<T>: List<T>
    {
        public MetaData MetaData { get; set; }

        public PageList(List<T> items,int count,int pageSize,int pageNumber)
        {
            MetaData = new MetaData()
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPage = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        public static PageList<T> ToPageList(IEnumerable<T> source,int pageSize,int pageNumber)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PageList<T>(items, count, pageSize, pageNumber);
        }

    }
}

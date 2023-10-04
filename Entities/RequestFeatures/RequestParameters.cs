using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class RequestParameters
    {
        public const int maxPageSize = 50;

        public int PageNumber { get; set; }

        private int PageSize;

        public int _PageSize
        {
            get { return PageSize; }
            set { PageSize = value > maxPageSize ? maxPageSize : value; }
        }
        //SIRALAMA İÇİN
        public String? OrderBy { get; set; }

        //VERİ ŞEKİLLEME İÇİN
        public String? Fields { get; set; }
    }
}

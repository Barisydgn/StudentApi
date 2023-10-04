using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class EmployeeParameters:RequestParameters
    {
        //BU ALTTAKİ 3 ŞEY FİLTRELEME İÇİN
        public uint MaxId { get; set; }
        public uint MinId { get; set; }
        public bool ValipIdRange => MaxId> MinId;


        //Arama İçin

        public string? Search { get; set; }

        //SIRALAMA İÇİN
        public EmployeeParameters()
        {
            OrderBy = "id";
        }


    }
}

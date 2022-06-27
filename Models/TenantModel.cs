using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PavilionsEF.Models
{
    public class TenantModel
    {
        public int id_tenant { get; set; }
        public string tenant_name { get; set; }
        public string telephone { get; set; }
        public string tenant_address { get; set; }
    }
}

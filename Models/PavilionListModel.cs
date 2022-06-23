using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PavilionsEF.Models
{
    public class PavilionListModel
    {
        public string id_pavilion { get; set; }
        public int id_shopping_center { get; set; }
        public string shopping_center_name { get; set; }
        public string shopping_center_status_name { get; set; }
        public int floor { get; set; }
        public int pavilion_status { get; set; }
        public string pavilion_status_name { get; set; }
        public decimal pavilion_square { get; set; }
        public decimal cost_per_square_meter { get; set; }
        public decimal value_added_factor { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PavilionsEF.Models
{
    public class ShoppingCenterModel
    {
        public int id_shopping_center { get; set; }
        public string shopping_center_name { get; set; }
        public int status_id { get; set; }
        public string status_name { get; set; }
        public int pavilions_quantity { get; set; }
        public string city { get; set; }
        public decimal cost { get; set; }
        public int number_of_storeys { get; set; }
        public decimal value_added_factor { get; set; }
        public byte[] shopping_center_image { get; set; }
    }
}

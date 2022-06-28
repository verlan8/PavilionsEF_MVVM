using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PavilionsEF.Models
{
    public class RentalListModel
    {
        public string shopping_center_name { get; set; }
        public string city { get; set; }
        public string id_pavilion { get; set; }
        public System.DateTime start_of_lease { get; set; }
        public System.DateTime end_of_lease { get; set; }
        public decimal rental_cost { get; set; }
        public string rent_status { get; set; }
    }
}

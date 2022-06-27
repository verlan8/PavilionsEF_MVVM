using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PavilionsEF.Models
{
    public class EmployeeModel
    {
        public int id_employee { get; set; }
        public string employee_surname { get; set; }
        public string employee_name { get; set; }
        public string employee_middlename { get; set; }
        public string employee_login { get; set; }
        public string employee_password { get; set; }
        public int id_role { get; set; }
        public string role_name { get; set; }
        public string telephone { get; set; }
        public string sex { get; set; }
        public byte[] employee_image { get; set; }
    }
}

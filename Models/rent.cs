//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PavilionsEF.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class rent
    {
        public int id_rent { get; set; }
        public int id_tenant { get; set; }
        public int id_shopping_center { get; set; }
        public int id_employee { get; set; }
        public string id_pavilion { get; set; }
        public int id_status { get; set; }
        public System.DateTime start_of_lease { get; set; }
        public System.DateTime end_of_lease { get; set; }
    
        public virtual employee employee { get; set; }
        public virtual pavilion pavilion { get; set; }
        public virtual tenant tenant { get; set; }
        public virtual rentStatus rentStatus { get; set; }
    }
}

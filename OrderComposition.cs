//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderComposition
    {
        public int IDorderComposition { get; set; }
        public int orderID { get; set; }
        public int sparePartID { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual SparePart SparePart { get; set; }
    }
}

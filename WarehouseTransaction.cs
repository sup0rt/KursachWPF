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
    
    public partial class WarehouseTransaction
    {
        public int TransactionID { get; set; }
        public Nullable<int> PartID { get; set; }
        public int Quantity { get; set; }
        public System.DateTime TransactionDateTime { get; set; }
    
        public virtual Part Part { get; set; }
    }
}

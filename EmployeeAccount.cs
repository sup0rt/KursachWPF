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
    
    public partial class EmployeeAccount
    {
        public int IDemployeeAccount { get; set; }
        public int employeeID { get; set; }
        public string login { get; set; }
        public string passwordHash { get; set; }
        public string salt { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }

        private static Entities _context;

        public static Entities GetContext()
        {
            if(_context == null)
            {
                _context = new Entities();
            }
            return _context;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Parts> Parts { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<Statuses> Statuses { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<WarehouseTransactions> WarehouseTransactions { get; set; }
    }
}

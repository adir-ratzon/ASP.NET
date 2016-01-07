using System;
using Store.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Store.DAL
{
    public class OrderDAL : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().ToTable("dbo.OrdersSet");
        }

        public DbSet<Order> Order { get; set; }
    }
}
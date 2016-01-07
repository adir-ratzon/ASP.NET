using Store.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Store.DAL
{
    public class CustomerDAL : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CustomerEntity>().ToTable("dbo.CustomerSet");
        }

        public DbSet<CustomerEntity> Customers { get; set; }
    }
}
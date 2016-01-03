using System;
using Store.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Store.DAL
{
    public class ProductDAL : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>().ToTable("dbo.ProductSet");
        }

        public DbSet<Products> Products { get; set; }
    }
}
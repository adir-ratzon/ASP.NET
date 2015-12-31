using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Store.DAL
{
    public class LoginDAL : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Login>().ToTable("dbo.LoginSet");
        }

        public DbSet<Login> Logins { get; set; }
    }
}
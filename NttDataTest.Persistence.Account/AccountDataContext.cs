using Microsoft.EntityFrameworkCore;
using NttDataTest.Domain.Account;
using System;

namespace NttDataTest.Persistence.Account
{
    public class AccountDataContext : DbContext
    {
        public AccountDataContext(DbContextOptions<AccountDataContext> options)
            : base(options)
        {

        }

        public DbSet<Cuenta> Cuentas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Cuenta>().Property(c => c.CuentaGuid).HasDefaultValueSql("NEWID()");
        }
    }
}

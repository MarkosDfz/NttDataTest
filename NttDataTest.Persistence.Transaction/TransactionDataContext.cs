using Microsoft.EntityFrameworkCore;
using NttDataTest.Domain.Transaction;
using System;

namespace NttDataTest.Persistence.Transaction
{
    public class TransactionDataContext : DbContext
    {
        public TransactionDataContext(DbContextOptions<TransactionDataContext> options)
            : base(options)
        {

        }

        public DbSet<Movimiento> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movimiento>().Property(m => m.MovimientoGuid).HasDefaultValueSql("NEWID()");
        }
    }
}

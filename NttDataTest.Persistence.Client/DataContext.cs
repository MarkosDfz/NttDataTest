using Microsoft.EntityFrameworkCore;
using NttDataTest.Domain.Client;

namespace NttDataTest.Persistence.Client
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}

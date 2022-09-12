using Microsoft.EntityFrameworkCore;
using NttDataTest.Persistence.Client;

namespace NttDataTest.UnitTest.Client.Config
{
    public static class DataContextInMemory
    {
        public static DataContext Get()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                                    .UseInMemoryDatabase(databaseName: $"Cliente.Db")
                                    .Options;

            return new DataContext(options);
        }

    }
}

using Ardalis.Specification.EntityFrameworkCore;
using NttDataTest.Services.Client.Interfaces;

namespace NttDataTest.Persistence.Client.Repository
{
    public class MyRepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T> where T : class
    {
        private readonly DataContext _context;

        public MyRepositoryAsync(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}

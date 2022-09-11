using Ardalis.Specification.EntityFrameworkCore;
using NttDataTest.Services.Account.Interfaces;

namespace NttDataTest.Persistence.Account.Repository
{
    public class MyRepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T> where T : class
    {
        private readonly AccountDataContext _context;

        public MyRepositoryAsync(AccountDataContext context) : base(context)
        {
            _context = context;
        }
    }
}

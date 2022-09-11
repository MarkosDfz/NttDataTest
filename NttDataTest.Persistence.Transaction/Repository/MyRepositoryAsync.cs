using Ardalis.Specification.EntityFrameworkCore;
using NttDataTest.Services.Transaction.Interfaces;

namespace NttDataTest.Persistence.Transaction.Repository
{
    public class MyRepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T> where T : class
    {
        private readonly TransactionDataContext _context;

        public MyRepositoryAsync(TransactionDataContext context) : base(context)
        {
            _context = context;
        }
    }
}

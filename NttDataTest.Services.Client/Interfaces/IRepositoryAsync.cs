using Ardalis.Specification;

namespace NttDataTest.Services.Client.Interfaces
{
    public interface IRepositoryAsync<T> : IRepositoryBase<T> where T : class
    {
    }

    public interface IReadRepositoryAsync<T> : IRepositoryBase<T> where T : class
    {

    }
}

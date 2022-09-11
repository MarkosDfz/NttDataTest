using Ardalis.Specification;
using NttDataTest.Domain.Client;

namespace NttDataTest.Services.Client.Specifications
{
    public class ClientPagedSpecification : Specification<Cliente>
    {
        public ClientPagedSpecification(int pageSize, int pageNumber, string nombre)
        {
            Query.Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize);

            if (!string.IsNullOrEmpty(nombre))
            {
                Query.Search(x => x.Nombre, "%" + nombre + "%");
            }
        }
    }
}

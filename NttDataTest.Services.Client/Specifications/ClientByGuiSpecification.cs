using Ardalis.Specification;
using NttDataTest.Domain.Client;
using System.Linq;

namespace NttDataTest.Services.Client.Specifications
{
    public class ClientByGuiSpecification : Specification<Cliente>, ISingleResultSpecification
    {
        public ClientByGuiSpecification(string ClienteGuid)
        {
            Query.Where(x => x.ClienteGuid == ClienteGuid);
        }
    }
}

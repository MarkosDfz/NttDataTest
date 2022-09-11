using Ardalis.Specification;
using NttDataTest.Domain.Transaction;
using System;

namespace NttDataTest.Services.Transaction.Specifications
{
    public class TransactionPagedSpecification : Specification<Movimiento>
    {
        public TransactionPagedSpecification(int pageSize, int pageNumber, DateTime fechaInicio, DateTime fechaFin, string cuentaGuid)
        {
            var dt_inicio = fechaInicio.Date;
            var dt_fin = fechaFin.Date;

            Query.Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize);

            Query.Search(x => x.CuentaGuid.ToString(), "%" + cuentaGuid + "%")
                 .Where(x => x.Fecha > dt_inicio && x.Fecha < dt_fin)
                 .OrderBy(x => x.Fecha);
        }
    }
}

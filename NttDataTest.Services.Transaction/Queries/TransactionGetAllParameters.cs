using NttDataTest.Services.Transaction.Parameters;

namespace NttDataTest.Services.Transaction.Queries
{
    public class TransactionGetAllParameters : RequestParameter
    {
        public string CuentaGuid { get; set; }

        public string FechaInicio { get; set; }

        public string FechaFin { get; set; }
    }
}

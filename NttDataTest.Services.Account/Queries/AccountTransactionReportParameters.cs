using NttDataTest.Services.Account.Parameters;

namespace NttDataTest.Services.Account.Queries
{
    public class AccountTransactionReportParameters : RequestParameter
    {
        public string CuentaGuid { get; set; }

        public string FechaInicio { get; set; }

        public string FechaFin { get; set; }
    }
}

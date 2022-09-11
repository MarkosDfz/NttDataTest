namespace NttDataTest.Services.Proxies.Transaction.Account.Commands
{
    public class AccountUpdateInitialBalanceCommand
    {
        public string CuentaGuid { get; set; }

        public decimal NuevoSaldo { get; set; }
    }
}

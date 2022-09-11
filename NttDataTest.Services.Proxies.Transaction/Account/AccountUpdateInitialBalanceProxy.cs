using Microsoft.Extensions.Options;
using NttDataTest.Services.Proxies.Transaction.Account.Commands;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NttDataTest.Services.Proxies.Transaction.Account
{
    public interface IAccountUpdateInitialBalanceProxy
    {
        Task<(bool procesoCorrecto, string Error)> UpdateCuentaSaldoInicialAsync(AccountUpdateInitialBalanceCommand command);
    }

    public class AccountUpdateInitialBalanceProxy : IAccountUpdateInitialBalanceProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public AccountUpdateInitialBalanceProxy(IOptions<ApiUrls> apiUrls, HttpClient httpClient)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
        }

        public async Task<(bool procesoCorrecto, string Error)> UpdateCuentaSaldoInicialAsync(AccountUpdateInitialBalanceCommand command)
        {
            var body = new StringContent
            (
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var request = await _httpClient.PutAsync($"{_apiUrls.CuentaApiUrl}cuentas/saldo/{command.CuentaGuid}", body);

                var content = await request.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<Response<string>>(content, options);

                if (result.Succeeded)
                    return (true, null);

                return (false, result.Message);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

using Microsoft.Extensions.Options;
using NttDataTest.Services.Proxies.Transaction.Account.Commands;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NttDataTest.Services.Proxies.Transaction.Account
{
    public interface IAccountProxy
    {
        Task<(bool procesoCorrecto, CuentaRemoteResponse cuenta, string Error)> GetCuentaAsync(AccountGetByIdCommand command);
    }

    public class AccountProxy : IAccountProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public AccountProxy(IOptions<ApiUrls> apiUrls, HttpClient httpClient)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
        }

        public async Task<(bool procesoCorrecto, CuentaRemoteResponse cuenta, string Error)> GetCuentaAsync(AccountGetByIdCommand command)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiUrls.CuentaApiUrl}cuentas/{command.CuentaGuid}");

                var content = await request.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<Response<CuentaRemoteResponse>>(content, options);

                if (result.Succeeded)
                    return (true, result.Data, null);

                return (false, null, result.Message);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}

using Microsoft.Extensions.Options;
using NttDataTest.Services.Proxies.Account.Transaction.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NttDataTest.Services.Proxies.Account.Transaction
{
    public interface ITransactionProxy
    {
        Task<(bool procesoCorrecto, List<MovimientoRemoteResponse> movimientos, string Error)> GetMovimientosAsync(TransactionGetAllCommand command);
    }

    public class TransactionProxy : ITransactionProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public TransactionProxy(IOptions<ApiUrls> apiUrls, HttpClient httpClient)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
        }

        public async Task<(bool procesoCorrecto, List<MovimientoRemoteResponse> movimientos, string Error)> GetMovimientosAsync(TransactionGetAllCommand command)
        {
            try
            {
                string url = $"{_apiUrls.MovimientoApiUrl}movimientos?CuentaGuid={command.CuentaGuid}&PageSize={command.PageSize}" +
                             $"&FechaInicio={command.FechaInicio}&FechaFin={command.FechaFin}";

                var request = await _httpClient.GetAsync(url);

                var content = await request.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<Response<List<MovimientoRemoteResponse>>>(content, options);

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

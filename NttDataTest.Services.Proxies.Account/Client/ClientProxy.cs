using Microsoft.Extensions.Options;
using NttDataTest.Services.Proxies.Account.Client.Commands;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NttDataTest.Services.Proxies.Account.Client
{
    public interface IClientProxy
    {
        Task<(bool procesoCorrecto, ClienteRemoteResponse cliente, string Error)> GetClienteAsync(ClientGetByIdCommand command);
    }

    public class ClientProxy : IClientProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public ClientProxy(IOptions<ApiUrls> apiUrls, HttpClient httpClient)
        {
            _apiUrls = apiUrls.Value;
            _httpClient = httpClient;
        }

        public async Task<(bool procesoCorrecto, ClienteRemoteResponse cliente, string Error)> GetClienteAsync(ClientGetByIdCommand command)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{_apiUrls.ClienteApiUrl}clientes/{command.ClienteGuid}");

                var content = await request.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<Response<ClienteRemoteResponse>>(content, options);

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

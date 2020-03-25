using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Dtos.TrueLayer;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FractalSolutions.Api.HttpClients
{
    public class TrueLayerDataClient : ITrueLayerDataClient
    {
        private readonly HttpClient httpClient;

        public TrueLayerDataClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }    

        public async Task<ResponseTL<AccountTL>> GetUserAccountsAsync()
        {
            var response = await httpClient.GetAsync($"accounts");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ResponseTL<AccountTL>>(await response.Content.ReadAsStringAsync());
        }      

        public async Task<ResponseTL<TransactionTL>> GetAccountTransactionsAsync(string accountId)
        {
            var response = await httpClient.GetAsync($"accounts/{accountId}/transactions");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ResponseTL<TransactionTL>>(await response.Content.ReadAsStringAsync());
        }
    }
}

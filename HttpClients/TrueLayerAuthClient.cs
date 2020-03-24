using FractalSolutions.Api.Configuration;
using FractalSolutions.Api.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FractalSolutions.Api.HttpClients
{
    public class TrueLayerAuthClient: ITrueLayerAuthClient
    {        
        private readonly HttpClient _httpClient;
        private readonly IClientSettings _clientSettings;

        public TrueLayerAuthClient(HttpClient httpClient, IClientSettings clientSettings)
        {
            _httpClient = httpClient;
            _clientSettings = clientSettings;
        }

        public async Task<TokenInfoTL> GetTokenAsync(string authCode)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("grant_type", "authorization_code");
            dict.Add("client_id", _clientSettings.ClientId);
            dict.Add("client_secret", _clientSettings.ClientSecret);
            dict.Add("redirect_uri", "https://localhost:3001/api/redirect");
            dict.Add("code", authCode);

            var request = new HttpRequestMessage(HttpMethod.Post, "connect/token") { Content = new FormUrlEncodedContent(dict) };
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();       

            return JsonConvert.DeserializeObject<TokenInfoTL>(await response.Content.ReadAsStringAsync());
        }     
    }
}

using FractalSolutions.Api.Dtos;
using System.Threading.Tasks;

namespace FractalSolutions.Api.HttpClients
{
    public interface ITrueLayerAuthClient
    {
        public Task<TokenInfoTL> GetTokenAsync(string authCode);        
    }
}

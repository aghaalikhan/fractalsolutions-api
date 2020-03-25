using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Dtos.TrueLayer;
using System.Threading.Tasks;

namespace FractalSolutions.Api.HttpClients
{
    public interface ITrueLayerDataClient
    {
        public Task<ResponseTL<AccountTL>> GetUserAccountsAsync();
        public Task<ResponseTL<TransactionTL>> GetAccountTransactionsAsync(string accountId);
    }
}

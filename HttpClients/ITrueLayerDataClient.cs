using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Dtos.TrueLayer;
using System.Threading.Tasks;

namespace FractalSolutions.Api.HttpClients
{
    public interface ITrueLayerDataClient
    {
        public Task<ResponseTL<AccountTL>> GetUserAccountsAsync();
        public Task<ResponseTL<AccountTL>> GetUserAccountAsync(string accountId);
        public Task<ResponseTL<TransactionTL>> GetAccountTransactions(string accountId);
    }
}

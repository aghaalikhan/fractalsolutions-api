using FractalSolutions.Api.Dtos.TrueLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services.Interfaces
{
    public interface IUserAccountsTransactionsSummaryService
    {
        Task<IDictionary<TransactionCategoryTL, decimal>> GetAccountsSummaryAsync();
    }
}
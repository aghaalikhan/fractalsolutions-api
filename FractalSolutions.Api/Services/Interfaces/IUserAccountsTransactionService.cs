using FractalSolutions.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services.Interfaces
{
    public interface IUserAccountsTransactionsService
    {
        Task<IList<AccountTransactions>> GetUserTransactionsAsync();
    }
}
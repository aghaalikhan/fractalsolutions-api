using FractalSolutions.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services
{
    public interface IAccountTransactionsService
    {
        Task<IList<Transaction>> GetAccountTransactionsAsync(string accountId);
    }
}
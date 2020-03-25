using FractalSolutions.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services.Interfaces
{
    public interface IAccountTransactionsService
    {
        Task<IList<Transaction>> GetAccountTransactionsAsync(string accountId);
    }
}
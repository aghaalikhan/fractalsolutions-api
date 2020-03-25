using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Infrastructure;
using FractalSolutions.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services
{
    public class UserAccountsTransactionsService : IUserAccountsTransactionsService
    {        
        private readonly IUserAccountsService _userAccountsService;
        private readonly IAccountTransactionsService _accountTransactionService;

        public UserAccountsTransactionsService(           
            IUserAccountsService userAccountsService,
            IAccountTransactionsService accountTransactionService)
        {            
            _userAccountsService = userAccountsService;
            _accountTransactionService = accountTransactionService;            
        }

        public async Task<IList<AccountTransactions>> GetUserTransactionsAsync()
        {
            var accounts = await _userAccountsService.GetUserAccountsAsync();           
     
            var getAccTransactions = accounts.Select(acc =>
                {
                    return GetAccountTransactionsAsync(acc.AccountId);
                });

            return await Task.WhenAll(getAccTransactions);         
        }

        private async Task<AccountTransactions> GetAccountTransactionsAsync(string accountId)
        {
            var transactions = await _accountTransactionService.GetAccountTransactionsAsync(accountId);            

            return new AccountTransactions
            {
                AccountId = accountId,
                Transactions = transactions                
            };
        }
    }
}

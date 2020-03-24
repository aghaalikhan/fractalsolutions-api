using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Dtos.TrueLayer;
using FractalSolutions.Api.HttpClients;
using FractalSolutions.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services
{
    public class UserAccountsTransactionsService : IUserAccountsTransactionsService
    {
        private readonly ICustomMemoryCache _customMemoryCache;        
        private readonly IUserAccountsService _userAccountsService;
        private readonly IAccountTransactionsService _accountTransactionService;

        public UserAccountsTransactionsService(
            ICustomMemoryCache cusomMemoryCache,
            IUserAccountsService userAccountsService,
            IAccountTransactionsService accountTransactionService)
        {            
            _userAccountsService = userAccountsService;
            _accountTransactionService = accountTransactionService;
            _customMemoryCache = cusomMemoryCache;
        }

        public async Task<IList<AccountTransactions>> GetUserTransactionsAsync()
        {
            var accounts = await _userAccountsService.GetUserAccountsAsync();           
     
            var getAccTransactions = accounts.Select(acc =>
                {
                    return GetAccountTransactions(acc.AccountId);
                });

            return await Task.WhenAll(getAccTransactions);         
        }

        private async Task<AccountTransactions> GetAccountTransactions(string accountId)
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

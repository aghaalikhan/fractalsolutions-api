using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Dtos.TrueLayer;
using FractalSolutions.Api.HttpClients;
using FractalSolutions.Api.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services
{
    public class UserAccountsService : IUserAccountsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomMemoryCache _customMemoryCache;
        private readonly ITrueLayerDataClient _trueLayerDataClient;

        public UserAccountsService(
            IHttpContextAccessor httpContextAccessor,
            ICustomMemoryCache customMemoryCache, 
            ITrueLayerDataClient trueLayerDataClient)
        {
            _trueLayerDataClient = trueLayerDataClient;
            _httpContextAccessor = httpContextAccessor;
            _customMemoryCache = customMemoryCache;
        }

        public async Task<IList<Account>> GetUserAccountsAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            var currentUserKey = $"{userId}_{CacheKeyConstants.UserAccountsKey}";

            IList<AccountTL> userAccounts; 

            userAccounts = _customMemoryCache.Get<IList<AccountTL>>(currentUserKey);

            if(userAccounts == null)
            {
                var userAccountsResults = await _trueLayerDataClient.GetUserAccountsAsync();

                if (userAccountsResults.Status != ResultStatusTL.Succeeded)
                {
                    throw new Exception("Unable to get user accounts");
                }

                userAccounts = userAccountsResults.Results;

                _customMemoryCache.Set(currentUserKey, userAccounts);
            }         

            return userAccounts.Select(acc =>
            {
                return new Account
                {
                    AccountId = acc.AccountId,
                    DisplayName = acc.DisplayName,
                    AccountType = acc.AccountType
                };
            }).ToList();
        }

        public async Task<Account> GetUserAccountAsync(string accountId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            var currenUserAccountsKey = $"{userId}_{CacheKeyConstants.UserAccountsKey}";
            
            var cachedAccounts = _customMemoryCache.Get<IList<AccountTL>>(currenUserAccountsKey);

            AccountTL userAccount = null;

            if (cachedAccounts == null || cachedAccounts.Any(acc => acc.AccountId != accountId))
            {
                var userAccountsResults = await _trueLayerDataClient.GetUserAccountAsync(accountId);

                if (userAccountsResults.Status != ResultStatusTL.Succeeded)
                {
                    throw new Exception("Unable to get user accounts");
                }

                userAccount = userAccountsResults.Results.First();
                cachedAccounts.Add(userAccount);
            }            

            return new Account
            {
                AccountId = userAccount.AccountId,
                DisplayName = userAccount.DisplayName,
                AccountType = userAccount.AccountType
            };
        }
    }
}

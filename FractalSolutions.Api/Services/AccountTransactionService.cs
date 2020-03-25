using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Dtos.TrueLayer;
using FractalSolutions.Api.HttpClients;
using FractalSolutions.Api.Infrastructure;
using FractalSolutions.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services
{
    public class AccountTransactionsService : IAccountTransactionsService
    {
        private readonly ICustomMemoryCache _customMemoryCache;
        private readonly ITrueLayerDataClient _trueLayerDataClient;        

        public AccountTransactionsService(
            ICustomMemoryCache cusomMemoryCache,
            ITrueLayerDataClient trueLayerDataClient)
        {
            _trueLayerDataClient = trueLayerDataClient;
            _customMemoryCache = cusomMemoryCache;
        }

        public async Task<IList<Transaction>> GetAccountTransactionsAsync(string accountId)
        {
            var cacheKey = $"{accountId}_{CacheKeyConstants.AccountTransactionsKey}";
            var transactions = _customMemoryCache.Get<IList<TransactionTL>>(cacheKey);

            if (transactions == null)
            {
                var transactionsResult = await _trueLayerDataClient.GetAccountTransactionsAsync(accountId);

                if (transactionsResult.Status != ResultStatusTL.Succeeded)
                {
                    throw new Exception("Unable to retrieve account transactions");
                }

                _customMemoryCache.Set(cacheKey, transactionsResult.Results);
                transactions = transactionsResult.Results;
            }

            return transactions.Select(trans =>
            {
                return new Transaction
                {
                    Timestamp = trans.Timestamp,
                    TransactionCategory = trans.TransactionCategory,
                    Amount = trans.Amount,
                    TransactionId = trans.TransactionId
                };
            }).ToList();
        }
    }
}

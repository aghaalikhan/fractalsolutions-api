using FractalSolutions.Api.Dtos.TrueLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services
{
    public class UserAccountsTransactionsSummaryService : IUserAccountsTransactionsSummaryService
    {
        private readonly IUserAccountsTransactionsService _userAccountsTransactionsService;
        private readonly IDateTimeService _dateTimeService;

        public UserAccountsTransactionsSummaryService(
            IUserAccountsTransactionsService userAccountsTransactionsService,
            IDateTimeService dateTimeService)
        {
            _userAccountsTransactionsService = userAccountsTransactionsService;
            _dateTimeService = dateTimeService;
        }

        public async Task<IDictionary<TransactionCategoryTL, decimal>> GetAccountsSummaryAsync()
        {
            var accountTransactions = await _userAccountsTransactionsService.GetUserTransactionsAsync();

            var transactions = accountTransactions.SelectMany(accTrans => accTrans.Transactions).ToList();

            var categorisedTotals = new Dictionary<TransactionCategoryTL, decimal>();

            foreach (TransactionCategoryTL category in Enum.GetValues(typeof(TransactionCategoryTL)))
            {
                categorisedTotals.Add(category, 0);
            }

            transactions.ForEach(transaction =>
            {
                var today = _dateTimeService.UtcNow.Date;
                var pastWeek = today.AddDays(-7);

                if (transaction.TimeStamp >= pastWeek && transaction.TimeStamp < today)
                {
                    categorisedTotals[transaction.TransactionCategory] += transaction.Amount;
                }
            });

            return categorisedTotals;
        }
    }
}

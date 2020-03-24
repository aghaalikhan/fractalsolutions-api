using System.Collections.Generic;

namespace FractalSolutions.Api.Dtos
{
    public class AccountTransactions
    {
        public string AccountId { get; set; }

        public IList<Transaction> Transactions;
    }
}

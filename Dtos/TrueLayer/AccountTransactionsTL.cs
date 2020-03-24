using FractalSolutions.Api.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Dtos.TrueLayer
{
    public class AccountTransactionsTL
    {
        public string AccountId;
        public IList<TransactionTL> Transactions { get; set; }
    }
}

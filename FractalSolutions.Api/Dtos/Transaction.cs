using FractalSolutions.Api.Dtos.TrueLayer;
using System;

namespace FractalSolutions.Api.Dtos
{
    public class Transaction
    {
        public DateTimeOffset TimeStamp { get; set; }
        public TransactionCategoryTL TransactionCategory { get; set; }
        public decimal Amount { get; set; }
    }
}

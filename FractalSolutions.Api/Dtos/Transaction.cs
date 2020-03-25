using FractalSolutions.Api.Dtos.TrueLayer;
using System;

namespace FractalSolutions.Api.Dtos
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public TransactionCategoryTL TransactionCategory { get; set; }
        public decimal Amount { get; set; }
    }
}

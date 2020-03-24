using FractalSolutions.Api.Dtos.TrueLayer;

namespace FractalSolutions.Api.Dtos
{
    public class CategorisedTotal
    {
        public TransactionCategoryTL TransactionCategory { get; set; }
        public decimal? TotalSpendLastWeek { get; set; }
    }
}

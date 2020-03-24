using System.Collections.Generic;

namespace FractalSolutions.Api.Dtos.TrueLayer
{
    public class ResponseTL<T>
    {
        public IList<T> Results { get; set; }
        public ResultStatusTL Status { get; set; }
    }
}

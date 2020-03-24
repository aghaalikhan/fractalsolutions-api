using System;

namespace FractalSolutions.Api.Services
{
    public interface IDateTimeService
    {
        public DateTimeOffset UtcNow { get; }
    }
}

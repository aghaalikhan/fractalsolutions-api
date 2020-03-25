using System;

namespace FractalSolutions.Api.Services.Interfaces
{
    public interface IDateTimeService
    {
        public DateTimeOffset UtcNow { get; }
    }
}

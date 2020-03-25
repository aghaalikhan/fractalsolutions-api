using FractalSolutions.Api.Services.Interfaces;
using System;

namespace FractalSolutions.Api.Services
{
    public class DateTimeService: IDateTimeService
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;    
    }
}

using System;

namespace FractalSolutions.Api.Dtos
{
    public class AccountTL
    {        
        public DateTimeOffset UpdatedTimeStamp { get; set; }        
        public string AccountId { get; set; }
        public string AccountType { get; set; }
        public string DisplayName { get; set; }        
    }
}

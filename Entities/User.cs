using System;

namespace FractalSolutions.Api.Entities
{
    public class UserEntity
    {
        public string UserId { get; set; }                  
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset TokenExpiry { get; set; }        
    }
}

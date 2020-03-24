using System.Text.Json.Serialization;

namespace FractalSolutions.Api.Dtos
{
    public class TokenInfoTL
    {
        public string AccessToken { get; set; }        
        public int ExpiresIn { get; set; }        
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
    }
}

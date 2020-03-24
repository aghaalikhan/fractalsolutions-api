using FractalSolutions.Api.HttpClients;
using FractalSolutions.Api.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services
{
    public class AuthorisationService : IAuthorisationService
    {
        private readonly ITrueLayerAuthClient _trueLayerAuthClient;
        private readonly IUserRepository _userRespository;        

        public AuthorisationService(ITrueLayerAuthClient trueLayerAuthClient, IUserRepository userRespository)
        {
            _trueLayerAuthClient = trueLayerAuthClient;
            _userRespository = userRespository;
        }

        public async Task<string> GetAccessTokenAsync(string code)
        {
            var tokenInfo = await _trueLayerAuthClient.GetTokenAsync(code);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenInfo.AccessToken);
            var subject = jwtToken.Subject;


            // Typically we can store some information about user form the token however in this case there isn't a whole lot
            // not terribly familiar with authorisation code flow, not sure if should store these but oh well.
            _userRespository.AddUser(new Entities.UserEntity
            {
                UserId = subject,
                AuthToken = tokenInfo.AccessToken,
                RefreshToken = tokenInfo.RefreshToken,
                TokenExpiry = DateTime.Now.Add(TimeSpan.FromSeconds(tokenInfo.ExpiresIn))
            });

            return tokenInfo.AccessToken;
        }
    }
}

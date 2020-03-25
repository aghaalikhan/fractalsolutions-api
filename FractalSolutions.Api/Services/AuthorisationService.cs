using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.HttpClients;
using FractalSolutions.Api.Repositories;
using FractalSolutions.Api.Services.Interfaces;
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

        public async Task<TokenInfoTL> GetAccessTokenAsync(string code)
        {
            var tokenInfo = await _trueLayerAuthClient.GetTokenAsync(code);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenInfo.AccessToken);
            var subject = jwtToken.Subject;

            // Typically we can store some information about user form the token however in this case 
            // there isn't a whole lot so we store the subject id, currently we are using using 
            // this subject for anyting, however we chould ensure that when a token comes it it maches the user id on our repo
            _userRespository.AddUser(new Entities.UserEntity
            {
                UserId = subject              
            });

            return tokenInfo;
        }
    }
}

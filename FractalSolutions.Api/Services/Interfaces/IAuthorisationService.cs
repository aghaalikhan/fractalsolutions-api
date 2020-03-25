using FractalSolutions.Api.Dtos;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services.Interfaces
{
    public interface IAuthorisationService
    {
        Task<TokenInfoTL> GetAccessTokenAsync(string code);
    }
}
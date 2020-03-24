using FractalSolutions.Api.Dtos;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services
{
    public interface IAuthorisationService
    {
        Task<string> GetAccessTokenAsync(string code);
    }
}
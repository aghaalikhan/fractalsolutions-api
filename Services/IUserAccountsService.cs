using FractalSolutions.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Services
{
    public interface IUserAccountsService
    {
        public Task<IList<Account>> GetUserAccountsAsync();        
    }
}
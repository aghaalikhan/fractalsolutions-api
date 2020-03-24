using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Repositories;
using FractalSolutions.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Controllers
{
    [Route("api/[controller]")]
    public class RedirectController : Controller
    {
        private readonly IAuthorisationService _authorisationService;
        private readonly IUserRepository _userRepository;

        public RedirectController(IAuthorisationService authorisationService, IUserRepository userRepository)
        {
            _authorisationService = authorisationService;
            _userRepository = userRepository;
        }
        
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Get([FromQuery]string code)
        {
            if(string.IsNullOrWhiteSpace(code))
            {
                return BadRequest();
            }

            return Ok(await _authorisationService.GetAccessTokenAsync(code));
        }     
    }
}

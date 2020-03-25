using FractalSolutions.Api.Dtos;
using FractalSolutions.Api.Repositories;
using FractalSolutions.Api.Services.Interfaces;
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

        public RedirectController(IAuthorisationService authorisationService)
        {
            _authorisationService = authorisationService;            
        }
        
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenInfoTL), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery]string code)
        {
            if(string.IsNullOrWhiteSpace(code))
            {
                return BadRequest();
            }

            return Ok(await _authorisationService.GetAccessTokenAsync(code));
        }     
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FractalSolutions.Api.Infrastructure
{
    public class AddTrueLayerTokenHandler: DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddTrueLayerTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            request.Headers.Add("Authorization", token.ToString());

            return base.SendAsync(request, cancellationToken);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Web;

namespace JbHiFi.Weather.Api
{

    public class AppIdRequirementFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<AuthenticationSettings> _settings;

        public AppIdRequirementFilter(IHttpContextAccessor httpContextAccessor, IOptions<AuthenticationSettings> settings)
        {
            _httpContextAccessor = httpContextAccessor;
            _settings = settings;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var parsed = HttpUtility.ParseQueryString(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            
            if (!_settings.Value.Clients.ToUpper().Split(",").Contains(parsed["appId"].ToUpper()))
            {
                context.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }
        }
    }

    public class AppIdRequirement : IAuthorizationRequirement
    {
    }
}

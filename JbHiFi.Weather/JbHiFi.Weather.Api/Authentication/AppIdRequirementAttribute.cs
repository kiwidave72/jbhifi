using Microsoft.AspNetCore.Mvc;

namespace JbHiFi.Weather.Api.Authentication;

public class AppIdRequirementAttribute : TypeFilterAttribute
{
    public AppIdRequirementAttribute() : base(typeof(AppIdRequirementFilter))
    {
    }
}
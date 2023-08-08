using Microsoft.AspNetCore.Mvc;

namespace JbHiFi.Weather.Api;

public class AppIdRequirementAttribute : TypeFilterAttribute
{
    public AppIdRequirementAttribute() : base(typeof(AppIdRequirementFilter))
    {
    }
}
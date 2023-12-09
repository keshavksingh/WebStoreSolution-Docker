using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace StoreWebAPIApplication.CustomActionFilters
{
    public class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IConfiguration config;
        public CustomAuthorizationAttribute(IConfiguration config)
        {
            this.config = config;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            List<string> authorizedIds = new List<string>();
            var authSPNs = config.GetSection("authorizedSPNs").GetChildren();
            foreach (var item in authSPNs)
            {
                authorizedIds.Add(item.Value);
            }

            ClaimsIdentity identity = (ClaimsIdentity)context.HttpContext.User.Identity;
            string[] claims = identity.FindFirst(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value.Split(' ');
            string appid = identity.FindFirst("appid")?.Value;
            if (appid != null && authorizedIds.Exists(element => element == appid))
            {
                return;
            }
            else if (claims != null && Array.Exists(claims, element => element == "Reader"))
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}

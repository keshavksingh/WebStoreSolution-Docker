using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace StoreWebAPIApplication.Authorization
{
    public class ServicePrincipalAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IConfiguration config;
        public ServicePrincipalAuthorizationHandler(IConfiguration config)
        {
            this.config = config;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            var clientIdClaim = context.User.FindFirst(c => c.Type == "appid");
            var clientId = clientIdClaim?.Value;

            if (requirement.Name == "Read" && IsReadClient(clientId))
            {
                // Authorization logic for READ operations
                context.Succeed(requirement);
            }
            else if (requirement.Name == "Write" && IsWriteClient(clientId))
            {
                // Authorization logic for WRITE operations
                context.Succeed(requirement);
            }
            else
            {
                // Authorization failed
                var httpContext = context.Resource as Microsoft.AspNetCore.Http.HttpContext;
                httpContext.Response.StatusCode = 401;
                context.Fail();
            }

            return Task.CompletedTask;
        }

        private bool IsReadClient(string clientId)
        {
            List<string> authorizedReadIds = new List<string>();
            var authSPNs = config.GetSection("authorizedReadSPNs").GetChildren();
            foreach (var item in authSPNs)
            {
                authorizedReadIds.Add(item.Value);
            }
            if (clientId != null && authorizedReadIds.Exists(element => element == clientId))
            {
                return true;
            }

            return false;
        }

        private bool IsWriteClient(string clientId)
        {
            List<string> authorizedReadWriteIds = new List<string>();
            var authSPNs = config.GetSection("authorizedReadWriteSPNs").GetChildren();
            foreach (var item in authSPNs)
            {
                authorizedReadWriteIds.Add(item.Value);
            }
            if (clientId != null && authorizedReadWriteIds.Exists(element => element == clientId))
            {
                return true;
            }

            return false;
        }
    }

}

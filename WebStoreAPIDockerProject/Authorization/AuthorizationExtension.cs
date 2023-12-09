using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace StoreWebAPIApplication.Authorization
{
    public static class AuthorizationExtension
    {
        public static void AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, ServicePrincipalAuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ReadPolicy", policy =>
                    policy.Requirements.Add(new OperationAuthorizationRequirement { Name = "Read" }));

                options.AddPolicy("WritePolicy", policy =>
                    policy.Requirements.Add(new OperationAuthorizationRequirement { Name = "Write" }));
            });
        }
    }
}

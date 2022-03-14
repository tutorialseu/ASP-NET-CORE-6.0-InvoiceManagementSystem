using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IdentityApp.Authorization
{
    public class InvoiceManagerAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, 
            Invoice invoice)
        {

            if (context.User == null || invoice == null)
                return Task.CompletedTask;

            if (requirement.Name != Constants.ApprovedOperationName &&
                requirement.Name != Constants.RejectedOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(Constants.InvoiceManagersRole))
                context.Succeed(requirement);

            return Task.CompletedTask;

        }
    }
}

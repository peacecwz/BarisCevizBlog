using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Attributes
{
    public class AdminRequirement : IAuthorizationRequirement
    {
        
    }

    public class AdminHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if (UserManager.CurrentUser == null)
            {
                context.Fail();
            }
            else
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}

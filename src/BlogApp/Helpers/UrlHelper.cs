using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Helpers
{
    public static class UrlHelper
    {
        public static string FullPathAction(this IUrlHelper helper, string action, string controller, object routeValues)
        {
            string basePath = helper.ActionContext.HttpContext.Request.Host.Value;
            return $"http://{basePath}{helper.Action(action, controller, routeValues)}";
        }
    }
}

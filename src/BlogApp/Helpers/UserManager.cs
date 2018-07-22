using BlogApp.Helpers;
using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp
{
    public class UserManager
    {
        public static AuthUser CurrentUser
        {
            get
            {
                return SessionHelper.Get<AuthUser>("User");
            }
            set
            {
                SessionHelper.Set("User", value);
            }
        }
    }
}

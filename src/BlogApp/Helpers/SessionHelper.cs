using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Helpers
{
    public class SessionHelper
    {
        #region Session Helper

        private static ISession SessionContext
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }

        public static T Get<T>(string key) where T :class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(SessionContext.GetString(key));
            }
            catch { return null; }
        }

        public static void Set(string key,object value)
        {
            SessionContext.SetString(key, JsonConvert.SerializeObject(value));
        }
        
        #endregion
    }
}

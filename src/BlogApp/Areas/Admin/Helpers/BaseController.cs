using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Admin
{
    public class BaseController : Controller
    {
        #region Message Helper
        public ActionResult Error(string message, object model = null)
        {
            ModelState.AddModelError("Error", message);
            return View(model);
        }

        public void Message(string message)
        {
            ModelState.AddModelError("Message", message);
        }

        public ActionResult Success(string message,object model = null)
        {
            ModelState.AddModelError("Success", message);
            return View(model);
        }
        #endregion
    }
}

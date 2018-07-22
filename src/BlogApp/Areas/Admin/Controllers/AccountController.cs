using BlogApp.Areas.Admin.Models;
using BlogApp.EF.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : BaseController
    {
        Repositories.Repository<Author> AuthorRepo = new Repositories.Repository<Author>();

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Author author = AuthorRepo.First(a => a.Username == model.Username.ToLower() && a.Password == model.Password.Hash());
                if (author == null)
                    return Error("Kullanıcı adı veya şifreniz yanlıştır!", model);
                UserManager.CurrentUser = new BlogApp.Models.AuthUser()
                {
                    FullName = author.FullName,
                    Id = author.Id,
                    Username = author.Username,
                    Role = ""
                };
                return RedirectToAction("Index", "Dashboard");
            }
            return View(model);
        }

        [Authorize(Policy = "Admin")]
        public IActionResult Logout()
        {
            UserManager.CurrentUser = null;
            return RedirectToAction("Login");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class DashboardController : Controller
    {
        Repositories.Repository<EF.Tables.Post> PostsRepo = new Repositories.Repository<EF.Tables.Post>();

        public IActionResult Index()
        {
            return View(new Models.DashboardModel()
            {
                PostCount = PostsRepo.Count(),
                CommentCount = 0,
                GuestCount = 0,
                Posts = PostsRepo.All().Select(p => new Models.PostListModel()
                {
                    Id = p.Id,
                    IsDraft = p.IsDraft,
                    Title = p.Title
                }).ToList()
            });
        }
    }
}

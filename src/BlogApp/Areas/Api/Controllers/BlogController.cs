using BlogApp.EF.Tables;
using BlogApp.Models;
using BlogApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Api.Controllers
{
    public class BlogController : Controller
    {
        Repository<Category> CategoryRepo = new Repository<Category>();
        Repository<Post> PostRepo = new Repository<Post>();

        [HttpGet]
        [Route("api/blog/categories")]
        public List<BlogApp.Models.CategoryBarModel> Categories()
        {
            return CategoryRepo.All().Select(c => new CategoryBarModel()
            {
                CategoryId = c.Id,
                Name = c.Name
            }).ToList();
        }

        [HttpGet]
        [Route("api/blog/posts")]
        public List<Models.PostModel> GetPosts()
        {
            return PostRepo.All().Select(post => new Models.PostModel()
            {
                ImageUrl = post.Image,
                PostUrl = $"http://{BlogApp.HttpContext.Current.Request.Host}/Post/{post.Url}",
                Title = post.Title
            }).ToList();
        }
    }
}

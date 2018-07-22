using BlogApp.EF.Tables;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogApp.Repositories;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        Repository<Post> PostRepo = new Repository<Post>();
        Repository<Category> CategoryRepo = new Repository<Category>();
        Repository<Page> PageRepo = new Repository<EF.Tables.Page>();

        public IActionResult Index()
        {
            List<Post> posts = PostRepo.Paging(p => p.CreatedDate, 1, 10, p => p.Author);
            PostListModel model = new Models.PostListModel();
            model.Posts = posts.OrderByDescending(p => p.CreatedDate).Where(p => !p.IsDraft).Select(post => new PostModel()
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                AuthorNameSurname = post.Author?.FullName,
                CreatedDate = post.CreatedDate,
                Description = post.Description,
                FriendlyUrl = post.Url,
                ImageUrl = post.Image,
                Title = post.Title
            }).ToList();
            model.CanNext = (PostRepo.Count() / 10) > 0 & (PostRepo.Count() % 10) > 0;
            model.CanPrevious = false;
            model.PageNumber = 1;
            return View(model);
        }

        public IActionResult Posts(Nullable<uint> Page)
        {
            Page = Page ?? 1;
            List<Post> posts = PostRepo.Paging(p => p.CreatedDate, (Page.HasValue) ? Page.Value : 1, 10, p => p.Author);
            PostListModel model = new Models.PostListModel();
            model.Posts = posts.OrderByDescending(p => p.CreatedDate).Where(p => !p.IsDraft).Select(post => new PostModel()
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                AuthorNameSurname = post.Author?.FullName,
                CreatedDate = post.CreatedDate,
                Description = post.Description,
                FriendlyUrl = post.Url,
                ImageUrl = post.Image,
                Title = post.Title
            }).ToList();
            model.CanNext = (PostRepo.Count() / 10) > 0 & (PostRepo.Count() % (Page.Value * 10)) > 0;
            model.CanPrevious = (Page.Value - 1) != 0;
            model.PageNumber = Page.Value;
            return View(model);
        }

        public IActionResult Post(string Id)
        {
            Guid PostId;
            Post post;
            if (Guid.TryParse(Id, out PostId))
                post = PostRepo.Single(p => !p.IsDraft && p.Id == PostId, p => p.Author);
            else
                post = PostRepo.Single(p => !p.IsDraft && p.Url == Id.ToLower(), p => p.Author);
            if (post == null) return NotFound();
            return View(new PostDetailModel()
            {
                PostId = post.Id,
                CategoryId = post.CategoryId,
                AuthorFullName = post.Author?.FullName,
                CategoryName = post.Category?.Name,
                Content = post.Content,
                Description = post.Description,
                FriendlyUrl = post.Url,
                ImageUrl = post.Image,
                Tags = post.Tags,
                Title = post.Title,
                CreatedDate = post.CreatedDate,
                AuthorId = post.AuthorId
            });
        }

        public IActionResult Page(string Id)
        {
            Guid PageId;
            Page page;
            if (Guid.TryParse(Id, out PageId))
                page = PageRepo.Single(p => !p.IsDraft && p.Id == PageId);
            else
                page = PageRepo.Single(p => !p.IsDraft && p.Url == Id.ToLower());
            if (page == null) return NotFound();
            return View(new PageModel()
            {
                PostId = page.Id,
                Content = page.Content,
                Description = page.Description,
                FriendlyUrl = page.Url,
                ImageUrl = page.Image,
                Tags = page.Tags,
                Title = page.Title,
                CreatedDate = page.CreatedDate
            });
        }

        public IActionResult Category(string Id)
        {
            BlogApp.EF.Tables.Category category;
            int CategoryId;
            if (int.TryParse(Id, out CategoryId))
                category = CategoryRepo.Single(c => c.Id == CategoryId);
            else
                category = CategoryRepo.Single(c => c.Url == Id);
            if (category == null) return NotFound();
            List<Post> posts = PostRepo.Where(p => !p.IsDraft && p.CategoryId == category.Id, p => p.Author);
            return View(posts.OrderByDescending(p => p.CreatedDate).Select(post => new PostModel()
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                AuthorNameSurname = post.Author?.FullName,
                CreatedDate = post.CreatedDate,
                Description = post.Description,
                FriendlyUrl = post.Url,
                ImageUrl = post.Image,
                Title = post.Title
            }).ToList());
        }

        public IActionResult Author(Nullable<Guid> Id)
        {
            if (!Id.HasValue) return NotFound();
            List<Post> posts = PostRepo.Where(p => !p.IsDraft && p.AuthorId == Id.Value, p => p.Author);
            return View(posts.OrderByDescending(p => p.CreatedDate).Select(post => new PostModel()
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                AuthorNameSurname = post.Author?.FullName,
                CreatedDate = post.CreatedDate,
                Description = post.Description,
                FriendlyUrl = post.Url,
                ImageUrl = post.Image,
                Title = post.Title
            }).ToList());
        }

        public IActionResult Projects()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Sitemap()
        {
            List<SitemapModel> sitemap = new List<SitemapModel>();
            sitemap.Add(new SitemapModel()
            {
                Title = "Barış Ceviz",
                Url = "http://barisceviz.com"
            });
            sitemap.Add(new SitemapModel()
            {
                Title = "Projelerim - Barış Ceviz",
                Url = "http://barisceviz.com/Projects"
            });
            sitemap.Add(new SitemapModel()
            {
                Title = "İletişim - Barış Ceviz",
                Url = "http://barisceviz.com/Contact"
            });
            PostRepo.Where(p => !p.IsDraft).OrderByDescending(p => p.CreatedDate).ToList().ForEach(post =>
            {
                sitemap.Add(new SitemapModel()
                {
                    Title = post.Title,
                    ImageUrl = post.Image,
                    Url = "http://barisceviz.com" + Url.Action("Post", "Home", new { id = post.Url })
                });
            });
            PageRepo.Where(p => !p.IsDraft).OrderByDescending(p => p.CreatedDate).ToList().ForEach(page =>
            {
                sitemap.Add(new SitemapModel()
                {
                    Title = page.Title,
                    ImageUrl = page.Image,
                    Url = "http://barisceviz.com" + Url.Action("Page", "Home", new { id = page.Url })
                });
            });
            Response.ContentType = "application/xml;application/xhtml";
            return View(sitemap);
        }
    }
}

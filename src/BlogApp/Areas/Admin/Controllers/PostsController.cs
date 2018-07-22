using BlogApp.Areas.Admin.Models;
using BlogApp.EF.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class PostsController : BaseController
    {
        Repositories.Repository<Post> PostRepo = new Repositories.Repository<Post>();
        Repositories.Repository<Category> CategoryRepo = new Repositories.Repository<Category>();

        public IActionResult Index()
        {
            List<Post> posts = PostRepo.All().OrderByDescending(p => p.CreatedDate).ToList();
            return View(posts.Select(post => new PostListModel()
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                IsDraft = post.IsDraft
            }).ToList());
        }

        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(CategoryRepo.All(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PostModel model)
        {
            Guid AuthorId = UserManager.CurrentUser.Id;
            ViewBag.Categories = new SelectList(CategoryRepo.All(), "Id", "Name");
            if (ModelState.IsValid)
            {
                string imageUrl;
                if (model.PostImage != null)
                    imageUrl = await StorageHelper.Instance.UploadFile(model.PostImage.OpenReadStream(), model.Title.FriendlyUrl());
                else
                    imageUrl = CategoryRepo.Single(c => c.Id == model.CategoryId).CategoryImage;
                var post = new EF.Tables.Post()
                {
                    Id = model.Id,
                    AuthorId = AuthorId,
                    Content = model.Content,
                    CreatedDate = DateTime.Now,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    Image = imageUrl,
                    Tags = model.Tags,
                    Title = model.Title,
                    IsDraft = true,
                    Url = model.Title.FriendlyUrl()
                };
                if (PostRepo.Add(post))
                    return Success("Yazınız başarılı bir şekilde eklendi.", model);
                else
                    return Error("Yazınız eklenirken hata oluştu.", model);
            }
            return View(model);
        }

        public IActionResult Edit(Nullable<Guid> Id)
        {
            if (!Id.HasValue)
                return RedirectToAction("Index");
            Post post = PostRepo.Single(p => p.Id == Id.Value);
            if (post == null)
                return RedirectToAction("Index");
            ViewBag.Categories = new SelectList(CategoryRepo.All(), "Id", "Name");
            return View(new PostModel()
            {
                CategoryId = post.CategoryId,
                Id = post.Id,
                Content = post.Content,
                Description = post.Description,
                Tags = post.Tags,
                Title = post.Title
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(PostModel model)
        {
            if (ModelState.IsValid)
            {
                var post = PostRepo.First(p => p.Id == model.Id);
                if (post == null)
                    return Error("Güncellediğiniz yazı bulunamadı.", model);
                ViewBag.Categories = new SelectList(CategoryRepo.All(), "Id", "Name");
                Guid AuthorId = UserManager.CurrentUser.Id;
                string imageUrl;
                if (model.PostImage != null)
                    imageUrl = await StorageHelper.Instance.UploadFile(model.PostImage.OpenReadStream(), model.Title.FriendlyUrl());
                else
                    imageUrl = CategoryRepo.Single(c => c.Id == model.CategoryId).CategoryImage;

                if (PostRepo.Update(new Post()
                {
                    AuthorId = AuthorId,
                    Content = model.Content,
                    Description = model.Description,
                    Image = imageUrl,
                    Tags = model.Tags,
                    Title = model.Title,
                    Url = model.Title.FriendlyUrl()
                }))
                    return Success("Yazınız güncellenmiştir", model);
                else
                    return Error("Yazınız güncellenirken hata olutşu.", model);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(Nullable<Guid> Id)
        {
            if (!Id.HasValue) return Json("Yazı bulunamadı.");
            Post post = PostRepo.Single(a => a.Id == Id.Value);
            if (post == null)
                return Json("Yazı Bulunamadı.");
            if (PostRepo.Delete(post))
                return Json("Yazı başarılı bir şekilde silindi.");
            else
                return Json("Yazı silinirken bir hata oluştu.");
        }

        [HttpPost]
        public JsonResult Publish(Nullable<Guid> Id)
        {
            if (!Id.HasValue) return Json("Yazı bulunamadı.");
            Post post = PostRepo.Single(a => a.Id == Id.Value);
            if (post == null)
                return Json("Yazı Bulunamadı.");
            if (post.IsDraft)
            {
                if (post.PublishedDate == null)
                    post.PublishedDate = DateTime.Now;
                post.IsDraft = false;
            }
            else
                post.IsDraft = true;
            if (PostRepo.Update(post))
                return Json("Yazı başarılı bir şekilde yayınlandı.");
            else
                return Json("Yazı yayınlanırken bir hata oluştu.");
        }

    }
}

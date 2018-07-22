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
    public class PagesController : BaseController
    {
        Repositories.Repository<Page> PageRepo = new Repositories.Repository<Page>();
        Repositories.Repository<Category> CategoryRepo = new Repositories.Repository<Category>();

        public IActionResult Index()
        {
            List<Page> pages = PageRepo.All().OrderByDescending(p => p.CreatedDate).ToList();
            return View(pages.Select(page => new PageListModel()
            {
                Id = page.Id,
                Title = page.Title,
                Description = page.Description,
                IsDraft = page.IsDraft,
                FriendlyUrl = page.Url
            }).ToList());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PageModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = "";
                if (model.PageImage != null)
                    imageUrl = await StorageHelper.Instance.UploadFile(model.PageImage.OpenReadStream(), model.Title.FriendlyUrl());
                if (PageRepo.Add(new EF.Tables.Page()
                {
                    Id = model.Id,
                    Content = model.Content,
                    CreatedDate = DateTime.Now,
                    Description = model.Description,
                    Image = imageUrl,
                    Tags = model.Tags,
                    Title = model.Title,
                    IsDraft = true,
                    Url = model.Title.FriendlyUrl()
                }))
                    return Success("Sayfanız başarılı bir şekilde eklendi.", model);
                else
                    return Error("Sayfanız eklenirken hata oluştu.", model);
            }
            return View(model);
        }

        public IActionResult Edit(Nullable<Guid> Id)
        {
            if (!Id.HasValue)
                return RedirectToAction("Index");
            Page page = PageRepo.Single(p => p.Id == Id.Value);
            if (page == null)
                return RedirectToAction("Index");
            return View(new PageModel()
            {
                Id = page.Id,
                Content = page.Content,
                Description = page.Description,
                Tags = page.Tags,
                Title = page.Title
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(PageModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = "";
                if (model.PageImage != null)
                    imageUrl = await StorageHelper.Instance.UploadFile(model.PageImage.OpenReadStream(), model.Title.FriendlyUrl());
                if (PageRepo.Update(new Page()
                {
                    Content = model.Content,
                    Description = model.Description,
                    Id = model.Id,
                    Image = imageUrl,
                    Tags = model.Tags,
                    Title = model.Title,
                    Url = model.Title.FriendlyUrl()
                }))
                    return Success("Sayfanız güncellenmiştir", model);
                else
                    return Error("Sayfanız güncellenirken hata olutşu.", model);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(Nullable<Guid> Id)
        {
            if (!Id.HasValue) return Json("Sayfa bulunamadı.");
            Page page = PageRepo.Single(a => a.Id == Id.Value);
            if (page == null)
                return Json("Sayfa Bulunamadı.");
            if (PageRepo.Delete(page))
                return Json("Sayfa başarılı bir şekilde silindi.");
            else
                return Json("Sayfa silinirken bir hata oluştu.");
        }

        [HttpPost]
        public JsonResult Publish(Nullable<Guid> Id)
        {
            if (!Id.HasValue) return Json("Sayfa bulunamadı.");
            Page page = PageRepo.Single(a => a.Id == Id.Value);
            if (page == null)
                return Json("Sayfa Bulunamadı.");
            page.IsDraft = false;
            if (PageRepo.Update(page))
                return Json("Sayfa başarılı bir şekilde yayınlandı.");
            else
                return Json("Sayfa yayınlanırken bir hata oluştu.");
        }

    }
}
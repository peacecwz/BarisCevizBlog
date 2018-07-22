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
    [Authorize(Policy = "Admin")]
    public class CategoriesController : BaseController
    {
        Repositories.Repository<Category> CategoryRepo = new Repositories.Repository<Category>();

        public IActionResult Index()
        {
            List<Category> categories = CategoryRepo.All();
            return View(categories.Select(category => new CategoryListModel()
            {
                Id = category.Id,
                Name = category.Name
            }).ToList());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = await StorageHelper.Instance.UploadFile(model.CategoryImage.OpenReadStream(), model.Name.FriendlyUrl());
                if (CategoryRepo.Add(new EF.Tables.Category()
                {
                    Name = model.Name,
                    Url = model.Name.FriendlyUrl(),
                    CategoryImage = imageUrl
                }))
                    return Success("Kategori başarılı bri şekilde eklendi.", model);
                else
                    return Error("Kategori eklenirken hata oluştu.", model);
            }
            return View(model);
        }

        public IActionResult Edit(Nullable<int> Id)
        {
            if (!Id.HasValue)
                return RedirectToAction("Index");
            Category category = CategoryRepo.Single(a => a.Id == Id.Value);
            if (category == null)
                return RedirectToAction("Index");
            return View(new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = await StorageHelper.Instance.UploadFile(model.CategoryImage.OpenReadStream(), model.Name.FriendlyUrl());
                if (CategoryRepo.Update(new EF.Tables.Category()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Url = model.Name.FriendlyUrl(),
                    CategoryImage = imageUrl
                }))
                    return Success("Kategori başarılı bir şekilde güncellendi.", model);
                else
                    return Error("Kategori güncellenirken hata oluştu.", model);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(Nullable<int> Id)
        {
            if (!Id.HasValue) return Json("Kategori bulunamadı.");
            Category category = CategoryRepo.Single(a => a.Id == Id.Value);
            if (category == null)
                return Json("Kategori Bulunamadı.");
            if (CategoryRepo.Delete(category))
                return Json("Kategori başarılı bir şekilde silindi.");
            else
                return Json("Kategori silinirken bir hata oluştu.");
        }

    }
}

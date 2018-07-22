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
    [Authorize(Policy = "Admin")]
    [Area("Admin")]
    public class AuthorsController : BaseController
    {
        Repositories.Repository<Author> AuthorRepo = new Repositories.Repository<Author>();

        public IActionResult Index()
        {
            var authorList = AuthorRepo.All();
            return View(authorList.Select(author => new Models.AuthorListModel()
            {
                Email = author.Email,
                FullName = author.FullName,
                ProfileImage = author.ProfileImage,
                Id = author.Id,
                Title = author.Title
            }).ToList());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AuthorModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                    return Error("Şifreleriniz eşleşmiyor!", model);
                Author author = AuthorRepo.Single(a => a.Username == model.Username.ToLower() | a.Email == model.Email.ToLower());

                if (author != null)
                    return Error("Bu yazar önceden kaydedilmiştir.", model);

                if (AuthorRepo.Add(new Author()
                {
                    Email = model.Email,
                    FullName = model.NameSurname,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Password = model.Password.Hash(),
                    Username = model.Username.ToLower(),
                    ProfileImage = "",
                    Title = model.Title,
                    SocialMediaJSON = new SocialMediaHelper(model.Facebook, model.Twitter, model.Github, model.LinkedIn, model.WebSite).ToJSON()
                }))
                    return Success("Yazar başarılı bir şekilde eklendi.");
                else
                    return Error("Yazar eklenirken hata oluştu");
            }
            return View(model);
        }

        public IActionResult Edit(Nullable<Guid> Id)
        {
            if (!Id.HasValue)
                return RedirectToAction("Index");
            Author author = AuthorRepo.Single(a => a.Id == Id.Value);
            if (author == null)
                return RedirectToAction("Index");
            SocialMediaHelper socialMedia = new Admin.SocialMediaHelper(author.SocialMediaJSON);
            return View(new AuthorModel()
            {
                Id = author.Id,
                NameSurname = author.FullName,
                Email = author.Email,
                Title = author.Title,
                Username = author.Username.ToLower(),
                Facebook = socialMedia.Get("facebook")?.Url,
                Twitter = socialMedia.Get("twitter")?.Url,
                Github = socialMedia.Get("github")?.Url,
                LinkedIn = socialMedia.Get("linkedin")?.Url
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                    return Error("Şifreler birbiri ile uyuşmamaktadır.", model);
                if (AuthorRepo.Update(new EF.Tables.Author()
                {
                    Id = model.Id,
                    Email = model.Email,
                    FullName = model.NameSurname,
                    Password = model.Password.Hash(),
                    SocialMediaJSON = new SocialMediaHelper(
                            model.Facebook,
                            model.Twitter,
                            model.Github,
                            model.LinkedIn
                        ).ToJSON(),
                    Title = model.Title,
                    Username = model.Username
                }))
                    return Success("Yazar başarılı bir şekilde güncellendi.", model);
                else
                    return Error("Yazar güncellenirken bir hata oluştu.", model);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(Nullable<Guid> Id)
        {
            if (!Id.HasValue) return Json("Yazar bulunamadı.");
            Author author = AuthorRepo.Single(a => a.Id == Id.Value);
            if (author == null)
                return Json("Yazar Bulunamadı.");
            if (AuthorRepo.Delete(author))
                return Json("Yazar başarılı bir şekilde silindi.");
            else
                return Json("Yazar silinirken bir hata oluştu.");
        }
    }
}

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
    public class EventsController : BaseController
    {
        Repositories.Repository<Event> EventRepo = new Repositories.Repository<Event>();

        public IActionResult Index()
        {
            List<Event> categories = EventRepo.All();
            return View(categories.Select(Event => new EventModel()
            {
                Id = Event.Id,
                EventName = Event.EventName,
                Description = Event.Description
            }).ToList());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EventModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = await StorageHelper.Instance.UploadFile(model.MainImage.OpenReadStream(), model.EventName.FriendlyUrl());
                if (EventRepo.Add(new EF.Tables.Event()
                {
                    Content = model.Content,
                    CreatedDate = DateTime.Now,
                    EventName = model.EventName,
                    Description = model.Description,
                    MainImage = imageUrl
                }))
                    return Success("Etkinlik başarılı bri şekilde eklendi.", model);
                else
                    return Error("Etkinlik eklenirken hata oluştu.", model);
            }
            return View(model);
        }

        public IActionResult Edit(Nullable<Guid> Id)
        {
            if (!Id.HasValue)
                return RedirectToAction("Index");
            Event Event = EventRepo.Single(a => a.Id == Id.Value);
            if (Event == null)
                return RedirectToAction("Index");
            return View(new EventModel()
            {
                Id = Event.Id,
                EventName = Event.EventName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = await StorageHelper.Instance.UploadFile(model.MainImage.OpenReadStream(), model.EventName.FriendlyUrl());
                if (EventRepo.Update(new EF.Tables.Event()
                {
                    Id = model.Id,
                    EventName = model.EventName,
                    Url = model.EventName.FriendlyUrl(),
                    Description = model.Description,
                    Content = model.Content,
                    MainImage = imageUrl
                }))
                    return Success("Etkinlik başarılı bir şekilde güncellendi.", model);
                else
                    return Error("Etkinlik güncellenirken hata oluştu.", model);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(Nullable<Guid> Id)
        {
            if (!Id.HasValue) return Json("Etkinlik bulunamadı.");
            Event Event = EventRepo.Single(a => a.Id == Id.Value);
            if (Event == null)
                return Json("Etkinlik Bulunamadı.");
            if (EventRepo.Delete(Event))
                return Json("Etkinlik başarılı bir şekilde silindi.");
            else
                return Json("Etkinlik silinirken bir hata oluştu.");
        }
    }
}

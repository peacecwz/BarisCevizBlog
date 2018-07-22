using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Admin.Models
{
    public class PageModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Başlık")]
        public string Title { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        [Display(Name = "İçerik")]
        public string Content { get; set; }
        [Display(Name = "Etiketler")]
        public string Tags { get; set; }
        [Display(Name = "Sayfa Resmi")]
        public IFormFile PageImage { get; set; }
    }
}

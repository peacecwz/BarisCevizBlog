using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Admin.Models
{
    public class AuthorModel
    {
        public Guid Id { get; set; }
        [Display(Name ="Adı Soyadı")]
        public string NameSurname { get; set; }
        [Display(Name ="Ünvan")]
        public string Title { get; set; }
        [Display(Name ="Kullanıcı Adı")]
        public string Username { get; set; }
        [Display(Name ="Şifre")]
        public string Password { get; set; }
        [Display(Name ="Şifre (Tekrar)")]
        public string ConfirmPassword { get; set; }
        [Display(Name ="E-Mail Adres")]
        public string Email { get; set; }
        [Display(Name ="Profil Resmi")]
        public IFormFile ProfileImage { get; set; }

        [Display(Name ="Facebook Url")]
        public string Facebook { get; set; }
        [Display(Name ="Twitter Url")]
        public string Twitter { get; set; }
        [Display(Name ="Github Url")]
        public string Github { get; set; }
        [Display(Name ="LinkedIn Url")]
        public string LinkedIn { get; set; }
        [Display(Name ="WebSite Url")]
        public string WebSite { get; set; }
    }
}

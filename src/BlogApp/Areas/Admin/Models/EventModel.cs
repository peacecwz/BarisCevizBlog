using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Admin.Models
{
    public class EventModel
    {
        public Guid Id { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string FriendlyUrl { get; set; }
        public IFormFile MainImage { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}

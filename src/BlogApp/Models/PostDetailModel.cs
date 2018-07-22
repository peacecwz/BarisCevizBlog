using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class PostDetailModel
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string FriendlyUrl { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorFullName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Tags { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.EF.Tables
{
    public class Author
    {

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public string SocialMediaJSON { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<Post> Posts { get; set; }
    }
}

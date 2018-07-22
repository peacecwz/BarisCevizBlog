﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorNameSurname { get; set; }
        public string ImageUrl { get; set; }
        public string FriendlyUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.EF.Tables
{
    public class Page
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }
        public bool IsDraft { get; set; } = false;
        public DateTime CreatedDate { get; set; }
    }
}

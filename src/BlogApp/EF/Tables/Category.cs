using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.EF.Tables
{
    public class Category
    { 

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string CategoryImage { get; set; }

        public List<Post> Posts { get; set; }

    }
}

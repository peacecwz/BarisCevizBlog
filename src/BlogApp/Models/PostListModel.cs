using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class PostListModel
    {
        public List<PostModel> Posts { get; set; }
        public bool CanNext { get; set; }
        public bool CanPrevious { get; set; }
        public uint PageNumber { get; set; }
    }
}

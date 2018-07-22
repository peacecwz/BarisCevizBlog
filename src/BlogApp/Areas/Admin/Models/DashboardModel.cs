using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Admin.Models
{
    public class DashboardModel
    {
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
        public int GuestCount { get; set; }
        public List<PostListModel> Posts { get; set; }
    }
}

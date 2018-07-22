using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.EF.Tables
{
    public class PostStatics
    {
        [Key]
        public int Id { get; set; }
        public Guid PostId { get; set; }
        public int Views { get; set; }

    }
}

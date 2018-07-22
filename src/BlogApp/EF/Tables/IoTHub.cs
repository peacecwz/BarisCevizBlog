using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.EF.Tables
{
    public class IoTHub
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
    }
}

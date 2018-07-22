using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.EF.Tables
{
    public class StepInsight
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public DateTime Day { get; set; }
        public int StepCount { get; set; }
    }
}

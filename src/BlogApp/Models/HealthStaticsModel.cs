using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class HealthStaticsModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<StepCount> Steps { get; set; }
    }

    public class StepCount
    {
        public string Date { get; set; }
        public string HealthHeight { get; set; }
        public string HealthValue { get; set; }
        public int Step { get; set; }

    }
}

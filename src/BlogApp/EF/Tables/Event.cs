using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.EF.Tables
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public string MainImage { get; set; }
        public string Images { get; set; }
        [NotMapped]
        public List<string> ImageList
        {
            get
            {
                List<string> imageList = new List<string>();
                string[] images = Images.Split(';');
                foreach(string image in images)
                {
                    if (string.IsNullOrEmpty(image)) continue;
                    imageList.Add(image);
                }
                return imageList;
            }
            set
            {
                value.ForEach(image =>
                {
                    Images += $"{image};";
                });
            }
        }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

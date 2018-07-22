using BlogApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Admin
{
    public class SocialMediaHelper
    {
        List<SocialMediaModel> SocialMedias = new List<SocialMediaModel>();

        public SocialMediaHelper(string json)
        {
            try
            {
                SocialMedias = JsonConvert.DeserializeObject<List<SocialMediaModel>>(json);
            }
            catch { }
        }

        public SocialMediaHelper(params string[] sites)
        {
            foreach (string site in sites)
            {
                if (site.ToLower().Contains("facebook"))
                    SocialMedias.Add(new BlogApp.Models.SocialMediaModel()
                    {
                        Postfix = "facebook",
                        Url = site
                    });
                else if (site.ToLower().Contains("twitter"))
                    SocialMedias.Add(new BlogApp.Models.SocialMediaModel()
                    {
                        Postfix = "twitter",
                        Url = site
                    });
                else if (site.ToLower().Contains("github"))
                    SocialMedias.Add(new BlogApp.Models.SocialMediaModel()
                    {
                        Postfix = "github",
                        Url = site
                    });
                else if (site.ToLower().Contains("instagram"))
                    SocialMedias.Add(new BlogApp.Models.SocialMediaModel()
                    {
                        Postfix = "instagram",
                        Url = site
                    });
                else if (site.ToLower().Contains("linkedin"))
                    SocialMedias.Add(new BlogApp.Models.SocialMediaModel()
                    {
                        Postfix = "linkedin",
                        Url = site
                    });
            }
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(SocialMedias);
        }

        public List<SocialMediaModel> ToModel()
        {
            return SocialMedias;
        }

        public SocialMediaModel Get(string key)
        {
            return SocialMedias.FirstOrDefault(s => s.Postfix == key.ToLower());
        }
    }
}

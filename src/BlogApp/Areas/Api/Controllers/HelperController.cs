using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlogApp.Areas.Api.Controllers
{
    public class HelperController : Controller
    {
        [HttpPost]
        [Route("api/helper/getgist")]
        public async Task<string> GetGist(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                string githubUsername = Startup.WebConfiguration.GetValue<string>("GithubUsername");
                var response = await client.GetAsync($"https://gist.github.com/{githubUsername}/{id}.js");
                if (response.IsSuccessStatusCode)
                    return (await response.Content?.ReadAsStringAsync()).Split('\n')[1].Replace(@"\""", @"""")
                    .Replace("document.write('", "")
                    .Replace(@"\n", "")
                    .Replace(@"\/", @"/")
                    .Replace("')", "");
            }
            return "";
        }
    }
}

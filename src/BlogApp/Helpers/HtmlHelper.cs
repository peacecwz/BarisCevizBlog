using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp
{
    public static class HtmlHelpers
    {
        public static HtmlString Image(this HtmlHelper helper, string source, string alt = "", int width = 0, int height = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<img source'{0}'", source));
            if (!string.IsNullOrEmpty(alt))
                builder.Append(String.Format(" alt='{0}'", alt));
            if (width != 0)
                builder.Append(String.Format(" width='{0}'", width));
            if (height != 0)
                builder.Append(String.Format(" height='{0}'", height));
            builder.Append(" />");
            return new HtmlString(builder.ToString());
        }

        public static HtmlString GithubGist(this HtmlHelper helper, string embedUrl)
        {
            string gistUrl = $"https://gist.github.com/{embedUrl}.js";
            string html = HttpHelper.Get(gistUrl);
            return new HtmlString(html.Split('\n')[1].Replace(@"\""", @"""")
                    .Replace("document.write('", "")
                    .Replace(@"\n", "")
                    .Replace(@"\/", @"/")
                    .Replace("')", ""));
        }

        public static IHtmlContent CreateTwitterMeta(this IHtmlHelper helper, string username, string title, string description, string imageurl)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($@"<meta name=""twitter:card"" content=""summary_large_image"">");
            builder.AppendLine($@"<meta name=""twitter:site"" content=""@{username}"">");
            builder.AppendLine($@"<meta name=""twitter:creator"" content=""@{username}"">");
            builder.AppendLine($@"<meta name=""twitter:title"" content=""{title}"">");
            builder.AppendLine($@"<meta name=""twitter:description"" content=""{description}"">");
            builder.AppendLine($@"<meta name=""twitter:image"" content=""{imageurl}"">");
            return new HtmlString(builder.ToString());
        }

        public static IHtmlContent CreateFacebookMeta(this IHtmlHelper helper, string url, string title, string description, string imageUrl)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($@"<meta property=""og:url"" content=""{url}"" />");
            builder.AppendLine($@"<meta property=""og:type"" content=""article"" />");
            builder.AppendLine($@"<meta property=""og:title"" content=""{title}"" />");
            builder.AppendLine($@"<meta property=""og:description"" content=""{description}"" />");
            builder.AppendLine($@"<meta property=""og:image"" content=""{imageUrl}"" />");
            return new HtmlString(builder.ToString());
        }

        public static string GetFullUrl(this IHtmlHelper helper, string action, string id)
        {
            string host = HttpContext.Current.Request.Host.Host;
            return $"http://{host}/{action}/{id}";
        }

        public static Models.HealthStaticsModel GetHealthStatics(this IHtmlHelper helper)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://{HttpContext.Current.Request.Host}");
                var responseTask = client.GetAsync("/api/health/get");
                responseTask.Wait();
                var response = responseTask.Result;
                if (!response.IsSuccessStatusCode) return new Models.HealthStaticsModel();

                var jsonTask = response.Content?.ReadAsStringAsync();
                jsonTask.Wait();
                string json = jsonTask.Result;
                return JsonConvert.DeserializeObject<Models.HealthStaticsModel>(json);
            }
        }
    }
}

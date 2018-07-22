using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlogApp
{
    public class HttpHelper
    {
        public static string Get(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var request = client.GetAsync(url);
                request.Wait();
                if (!request.Result.IsSuccessStatusCode) return "";
                var response = request.Result.Content?.ReadAsStringAsync();
                response.Wait();
                return response.Result;
            }
        }

        public static async Task<string> GetAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var request = await client.GetAsync(url);
                if (!request.IsSuccessStatusCode) return "";
                return await request.Content?.ReadAsStringAsync();
            }
        }
    }
}

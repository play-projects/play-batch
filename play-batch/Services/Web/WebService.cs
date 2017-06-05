using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace batch.Services.Web
{
    public class WebService
    {
        public static WebService Instance = new WebService();
        private WebService() { }

        private readonly int timeout = 10;

        public string GetContent(string url)
        {
			try
			{
				using (var client = new HttpClient())
				{
				    client.Timeout = TimeSpan.FromSeconds(timeout);
					var response = client.GetAsync(url).Result;
					if (!response.IsSuccessStatusCode) return string.Empty;

					var content = response.Content.ReadAsStringAsync().Result;
					return content;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return string.Empty;
			}
        }

        public string PostContent(string url, Dictionary<string, string> datas)
        {
			try 
			{
				using (var client = new HttpClient())
				{
				    client.Timeout = TimeSpan.FromSeconds(timeout);
                    var param = new FormUrlEncodedContent(datas);
					var response = client.PostAsync(url, param).Result;
					if (!response.IsSuccessStatusCode) return string.Empty;

					var content = response.Content.ReadAsByteArrayAsync().Result;
					return Encoding.UTF8.GetString(content);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return string.Empty;
			}
        }

        public string GetContent(string url, Dictionary<string, string> headers)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(timeout);
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    var reponse = client.GetAsync(url).Result;
                    if (!reponse.IsSuccessStatusCode) return string.Empty;

                    var content = reponse.Content.ReadAsStringAsync().Result;
                    return content;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
    }
}

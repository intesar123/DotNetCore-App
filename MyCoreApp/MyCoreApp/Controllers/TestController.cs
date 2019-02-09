using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyCoreApp.Controllers
{
    public class TestController : Controller
    {
        public async Task<JsonResult> Index()
        {
            string url = "https://localhost:44314/api/Login";
            var result = "" ;
            //var values = new Dictionary<string, string>
            //{
            //  {"Name", "Intesar"},
            //  {"Password", "1234"} //inputJson is an array of a complex type
            //};

            using (var client = new HttpClient())
            {
                try
                {
                    var httpContent = new HttpRequestMessage(HttpMethod.Post, url);

                    // NOTE: I think there is no need for this
                    //httpContent.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    //httpContent.Headers.Add("Accept-Encoding", "gzip, deflate");

                    // NOTE: Your server was returning 417 Expectation failed, this is set so the request client doesn't expect 100 and continue.
                    httpContent.Headers.ExpectContinue = false;
                    //httpContent.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var values = new List<KeyValuePair<string, string>>();
                    //values.Add(new KeyValuePair<string, string>("Name", "Intesar"));
                    //values.Add(new KeyValuePair<string, string>("Password", "1234"));

                    httpContent.Content = new StringContent("{ \"Email\": \"alam.kir@gmail.com\", \"Password\": \"Hello@123\" }" , Encoding.UTF8, "application/json");
                    //httpContent.

                    HttpResponseMessage response = await  client.SendAsync(httpContent);

                    // here is the hash as string
                     result = await response.Content.ReadAsStringAsync();
                  //  Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString()); //"Invalid URI: The Uri string is too long."
                }
                return  Json(result);

            }
            
        }
    }
}
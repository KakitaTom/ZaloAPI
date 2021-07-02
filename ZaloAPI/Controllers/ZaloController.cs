using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZaloAPI.Models;

namespace ZaloAPI.Controllers
{
    public class ZaloController : Controller
    {
        private Access ac;

        public ZaloController()
        {
            ac = new Access();
        }

        // GET: Zalo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var app_id = "1164505779227151149";
            var redirect_uri = "https://lamwebapp.somee.com/Quote/Random";
            var state = "random";

            string str = ("https://oauth.zaloapp.com/v3/permission?app_id=" + app_id + "&redirect_uri=" + redirect_uri + "&state=" + state);
            
            return Redirect(str);
        }

        [HttpGet]
        public ActionResult PostStatus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostStatus(News baipost)
        {
            var app_id = "1164505779227151149";
            var app_secret = "E9Z4L46d0478Gv7C8t7B";
            var code = "RKEnIi_M6pWKSwaaYvGhKbLDaZUvZLvP4qZaDfZP2oy2URLgxDW6VdPAW2AHgZvDTsRs8j-XTMTlEA9m_vqQ8JijdWsgopLKV1lU5wJV5cSE6w4idB1fEGzJmrpCZYSB3330FFNLPLi12Cniz9WjVYmbXGkkX3zKG2-KMVdYAJiK4jfQzCu4HHzAvaNZic8lA5R7QyNnOdLQ0y5en8TLSbnUYq2kmM05GZdbJu-0JtqvOgizzPyPMWr_l3djlN1jM7ZCFDEsItiGLAYg-CGYfoSNcVFHmn29D3BEqk_8GBzaKgKF6HnFNi6Cn0Tbx8nHipm79pQ9hbheMN8Q5vhD7U0W3aHncTKuc1nzAsgUqoXUMIOFJ1C1mj9NQm";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://oauth.zaloapp.com");
                var getAccess = client.GetAsync("/v3/access_token?app_id=" + app_id + "&app_secret=" + app_secret + "&code=" + code);
                getAccess.Wait();

                var result = getAccess.Result;
                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadAsStringAsync();
                    read.Wait();

                    ac = JsonConvert.DeserializeObject<Access>(read.Result);

                    baipost.access_token = ac.access_token;
                }
            }

//            return Content(baipost.access_token + " " + baipost.message + " " + baipost.link);
//            var str = "https://graph.zalo.me/v2.0/me/feed?access_token=" + news.access_token + "&message=" + news.message + "&link=" + news.link;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.zalo.me/");
                var postTask = client.PostAsJsonAsync<News>("v2.0/me/feed", baipost);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return Content(result.ToString());
                }
            }
            return Content("Lỗi");
        }

    }
}
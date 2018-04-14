using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class JsonController : Controller
    {        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Index(FormCollection model)
        {
            //ResponeVal responeVal = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/HE_SolarWS/api/solar/");
                //client.BaseAddress = new Uri("http://honestenergycorp.com/HESolarWS/api/solar/");
                //HTTP POST
                var postTask = await client.PostAsync("CreateProposal",
                    new FormUrlEncodedContent(model.AllKeys.ToDictionary(k => k, v => model[v])));
                //postTask.Wait();
                var result = postTask.Content.ReadAsAsync<ResponeVal>();
                
                var value = result.Result;
                if (postTask.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        status = true,
                        Data = value

                    });
                }


            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();
        }
    }
}
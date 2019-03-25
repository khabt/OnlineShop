using BotDetect.Web.Mvc;
using Facebook;
using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace OnlineShop.Controllers
{    
    public class UserController : Controller
    {

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[CaptchaValidation("CaptchaCode", "RegisterCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MvcCaptcha captcha = new MvcCaptcha("RegisterCaptcha");
                string userInput = HttpContext.Request.Form["CaptchaCode"];

                string validatingInstancedId = HttpContext.Request.Form[captcha.ValidatingInstanceKey];                

                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    if (captcha.Validate(userInput, validatingInstancedId))
                    {
                        // or you can use static Validate method of MvcCaptcha class                     
                        MvcCaptcha.ResetCaptcha("RegisterCaptcha");

                        var user = new User();
                        user.UserName = model.UserName;
                        user.Name = model.Name;
                        user.Password = Encryptor.MD5Hash(model.Password);
                        user.Phone = model.Phone;
                        user.Email = model.Email;
                        user.Address = model.Address;
                        user.CreatedDate = DateTime.Now;
                        user.Status = true;
                        if (!string.IsNullOrEmpty(model.ProvinceID))
                        {
                            user.ProvinceID = int.Parse(model.ProvinceID);
                        }
                        if (!string.IsNullOrEmpty(model.DistricID))
                        {
                            user.DistrictID = int.Parse(model.DistricID);
                        }
                        if (!string.IsNullOrEmpty(model.WardID))
                        {
                            user.WardID = int.Parse(model.WardID);
                        }
                        var result = dao.Insert(user);
                        if (result > 0)
                        {
                            ViewBag.Success = "Đăng ký thành công";
                            //model = new RegisterModel();
                            ModelState.Clear();
                            model = new RegisterModel();
                            
                        }
                        else
                        {
                            ModelState.AddModelError("", "Đăng ký không thành công");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mã xác nhận không đúng!");
                    }                    
                }
            }
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (Session[CommonConstants.USER_SESSION] != null)
            {
                return Redirect("/");
            }

            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserID = user.ID;
                    userSession.UserName = user.UserName;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tên đăng nhập không tồn tại");                   
                }
                else if (result == -1)
                {

                    ModelState.AddModelError("", "Tên đăng nhập bị khóa");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            Session[CommonConstants.CartSession] = null;
            return Redirect("/");
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.ConnectionStrings["FbAppId"].ToString(),
                client_secret = ConfigurationManager.ConnectionStrings["FbAppSecret"].ToString(),
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/Client/data/Provinces_Data.xml"));

            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");

            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);              
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }

        public JsonResult LoadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/Client/data/Provinces_Data.xml"));

            var xElement = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);

            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElement.Elements("Item").Where(x=>x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                list.Add(district);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }

        public JsonResult LoadWard(int districtID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/Client/data/Provinces_Data.xml"));
            var xElement = xmlDoc.Element("Root").Elements("Item").Elements("Item")
                .Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == districtID);

            var list = new List<WardModel>();
            WardModel ward = null;
            foreach (var item in xElement.Elements("Item").Where(x=>x.Attribute("type").Value == "precinct"))
            {
                ward = new WardModel();
                ward.ID = int.Parse(item.Attribute("id").Value);
                ward.Name = item.Attribute("value").Value;
                ward.DistrictID = int.Parse(xElement.Attribute("id").Value);
                list.Add(ward);
            }

            return Json(new
            {
                data = list,
                status = true
            });
        }

        public JsonResult LosdsdadWard(int districtID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/Client/data/Provinces_Data.xml"));
            var xElement = xmlDoc.Element("Root").Elements("Item").Elements("Item")
                .Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == districtID);

            var list = new List<WardModel>();
            WardModel ward = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "precinct"))
            {
                ward = new WardModel();
                ward.ID = int.Parse(item.Attribute("id").Value);
                ward.Name = item.Attribute("value").Value;
                ward.DistrictID = int.Parse(xElement.Attribute("id").Value);
                list.Add(ward);
            }

            return Json(new
            {
                data = list,
                status = true
            });
        }
    }
}
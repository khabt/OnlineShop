using Model.Dao;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password), true);
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserID = user.ID;
                    userSession.UserName = user.UserName;
                    userSession.GroupID = user.GroupID;
                    var listCredential = dao.GetListCredential(model.UserName);
                    Session.Add(CommonConstants.SESSION_CREDENTIAL, listCredential);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");                    
                }
                else if (result == -1)
                {

                    ModelState.AddModelError("", "Tài khoản bị khóa");
                }
                else if (result == -2)
                {

                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
                }
                else if (result == -3)
                {

                    ModelState.AddModelError("", "Tài khoản không có quyền đăng nhập");
                }
                else
                {
                    ModelState.AddModelError("", "Don't know");
                }
            }
            return View("Index");
        }
    }
}
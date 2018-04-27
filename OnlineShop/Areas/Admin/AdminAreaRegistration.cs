using System.Web.Mvc;

namespace OnlineShop.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
              name: "Category Create",
              url: "Admin/tao-danh-muc",
              defaults: new { controller = "Category", action = "Create", id = UrlParameter.Optional }
              );

            context.MapRoute(
              name: "Admin Fail",
              url: "Admin/dang-nhap-that-bai",
              defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
              );

            context.MapRoute(
               name: "Admin Login",
               url: "Admin/dang-nhap-admin",
               defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }                          
               );

            context.MapRoute(
               name: "Admin Home",
               url: "Admin/trang-chu",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
               );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                //namespaces: new[] { "OnlineShop.Areas.Admin.Controllers" }
            );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Common
{
    public class MergeData
    {
        public static string MergeTemplate(string pathTemplate, string shipName, string mobile, string email, string address, string total)
        {
            string content = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(pathTemplate));

            content = content.Replace("{{CustomerName}}", shipName)
                .Replace("{{Phone}}", mobile)
                .Replace("{{Email}}", email)
                .Replace("{{Address}}", address)
                .Replace("{{Total}}", total);
            return content;                
        }
    }
}
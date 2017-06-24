
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thayloilocnuoc.Models;

namespace Thayloilocnuoc.Controllers.Display
{
    public class DefaultController : Controller
    {
        ThayloilocnuocContext db = new ThayloilocnuocContext();
        //
        // GET: /Default/
        [OutputCache (Duration=2400)]
        public ActionResult Index()
        {
            tblConfig tblconfig = db.tblConfigs.First();
            ViewBag.Title = "<title>" + tblconfig.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblconfig.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblconfig.Keywords + "\" /> ";
            string chuoi="";
            chuoi+="<span class=\"gp\">"+tblconfig.Slogan+"</span>";
            chuoi+="<p>HOTLINE: <span>"+tblconfig.MobileIN+" </span> (Miền Bắc) |<span>"+tblconfig.MobileOUT+" </span>(Miền Nam)</p>";
            ViewBag.Hotline = chuoi;
            ViewBag.favicon=" <link href=\""+tblconfig.Favicon+"\" rel=\"icon\" type=\"image/x-icon\" />";
            ViewBag.content = tblconfig.Footer;
            if (Session["Register"]!="")
            {
                ViewBag.Register = Session["Register"];
               
            }
            Session["Register"] = "";
            return View();
        }
        public PartialViewResult ProductNewsHomes()
        {
            string chuoisp = "";
            string chuoinew = "";
            var listproduct = db.tblProducts.Where(p => p.Active == true && p.ViewHomes == true).OrderByDescending(p => p.DateCreate).Take(8).ToList();
            for (int i = 0; i < listproduct.Count;i++ )
            {
                chuoisp+="<div class=\"Tear_pdn\">";
                 chuoisp+="<div class=\"img\">";
                 chuoisp += "<a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\"><img src=\"" + listproduct[i].ImageLinkThumb + "\" title=\"" + listproduct[i].Name + "\" /></a>";
                 chuoisp+="</div>";
                 chuoisp += "<h2><a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\">" + listproduct[i].Name + "</a></h2>";
                chuoisp+="</div>";
            }
            ViewBag.chuoisp = chuoisp;
            var listnew = db.tblNews.Where(p => p.Active == true && p.idCate == 7).OrderByDescending(p => p.Ord).Take(5).ToList();
            for (int i = 0; i < listnew.Count;i++ )
            {
                chuoinew += "<div class=\"Tear_N\">";
                chuoinew += "<a href=\"/3/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\"><img src=\"" + listnew[i].ImageLinkThumb + "\" alt=\"" + listnew[i].Name + "\" /></a>";
                chuoinew += " <h3><a href=\"/3/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\" class=\"Name\">" + listnew[i].Name + " </a></h3>";
                chuoinew += "</div>";
            }
            ViewBag.chuoinew = chuoinew;
                return PartialView();
        }
	}
}
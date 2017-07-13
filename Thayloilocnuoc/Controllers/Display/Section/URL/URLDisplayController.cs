using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thayloilocnuoc.Models;
namespace Thayloilocnuoc.Controllers.Display.Section.URL
{
    public class URLDisplayController : Controller
    {
        public ThayloilocnuocContext db = new ThayloilocnuocContext();
        //
        // GET: /URLDisplay/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult URL(string tag)
        {
            int id=int.Parse(tag);
            string url = db.tblUrls.Find(id).Url;
            return Redirect(url);
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thayloilocnuoc.Models;

namespace Thayloilocnuoc.Controllers.Display.Section.Contact
{
    public class ContactsController : Controller
    {
        //
        // GET: /Contacts/
        private ThayloilocnuocContext db = new ThayloilocnuocContext();
        public ActionResult Index()
        {
            ViewBag.favicon = " <link href=\"" + db.tblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection colletion)
        {
            tblContact tblcontact = new tblContact();
            tblcontact.Name = colletion["txtName"];
            tblcontact.Mobile = colletion["txtMobile"];
            tblcontact.Address = colletion["txtAddress"];
            tblcontact.Email = colletion["txtEmail"];
            tblcontact.Content = colletion["txtDescription"];
            db.tblContacts.Add(tblcontact);
            db.SaveChanges();
               ViewBag.favicon = " <link href=\"" + db.tblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            ViewBag.status = "<script>$(document).ready(function(){ alert('Bạn đã liên hệ thành công. Vui lòng Check Mail để biết chi tiết !') });</script>";
            return View();
        }

	}
}
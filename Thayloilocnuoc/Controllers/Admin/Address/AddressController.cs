using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thayloilocnuoc.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Data;

namespace Thayloilocnuoc.Controllers.Admin.Address
{
    public class AddressController : Controller
    {
        // GET: Address
        private ThayloilocnuocContext db = new ThayloilocnuocContext();

        //
        // GET: /News/

        public ActionResult Index(int? page, string text, string idCate)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            string txtSearch = "";

            var listAddress = (from p in db.tblAddresses select p).OrderByDescending(p => p.Ord).ToList();
            if (txtSearch != null && txtSearch != "")
            {
                listAddress = db.tblAddresses.Where(p => p.Name.Contains(txtSearch)).OrderByDescending(p => p.Ord).ToList();
            }
            Session["txtSearch"] = null;
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
            if (idCate != "" && idCate != null)
            {
                int idcates = int.Parse(idCate);
                listAddress = db.tblAddresses.Where(p => p.idCate == idcates).OrderByDescending(p => p.Ord).ToList();
                ViewBag.idMenu = idCate;
            }
            var ship = new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                EllipsesFormat = "&#8230;",
                LinkToFirstPageFormat = "Trang đầu",
                LinkToPreviousPageFormat = "«",
                LinkToIndividualPageFormat = "{0}",
                LinkToNextPageFormat = "»",
                LinkToLastPageFormat = "Trang cuối",
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                FunctionToDisplayEachPageNumber = null,
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                ContainerDivClasses = new[] { "pagination-container" },
                UlElementClasses = new[] { "pagination" },
                LiElementClasses = Enumerable.Empty<string>()
            };
            ViewBag.ship = ship;
            #region[Load Menu]
            var pro = db.tblGroupAddresses.OrderByDescending(p => p.Ord).Take(1).ToList();
            var menuModel = db.tblGroupAddresses.OrderBy(m => m.Ord).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {

                lstMenu.Add(new SelectListItem { Text = menu.Name, Value = menu.id.ToString() });


            }
            if (idCate != "" || idCate != null)
            {
                ViewBag.idMenu = idCate;
                ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idCate);
            }
            else
                ViewBag.drMenu = lstMenu;



            if (Request.IsAjaxRequest())
            {
                int idCatelogy;
                if (text != null && text != "")
                {
                    var list = db.tblAddresses.Where(p => p.Name.ToUpper().Contains(text.ToUpper())).ToList();
                    return PartialView("addressPartialViews", list);
                }
                if (idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate); ViewBag.idMenu = idCate;
                    var list = db.tblAddresses.Where(p => p.idCate == idCatelogy).ToList(); ViewBag.idMenu = idCate;
                    return PartialView("addressPartialViews", list);
                }
                if (text != null && text != "" && idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate); ViewBag.idMenu = idCate;
                    var list = db.tblAddresses.Where(p => p.Name.ToUpper().Contains(text.ToUpper()) && p.idCate == (int.Parse(idCate))).ToList();
                    return PartialView("addressPartialViews", list);
                }

                else
                {
                    return PartialView("addressPartialViews", listAddress);
                }
            }

            return View(listAddress.ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult addressPartialViews()
        {
            return PartialView();
        }
        public ActionResult Create(string idCate)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblAddresses.OrderByDescending(p => p.Ord).Take(1).ToList();
            var GroupNews = db.tblGroupAddresses.OrderBy(m => m.Ord).ToList();
              var menuModel = db.tblGroupAddresses.OrderBy(m => m.Ord).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {

                lstMenu.Add(new SelectListItem { Text = menu.Name, Value = menu.id.ToString() });


            }
            if (idCate != "" || idCate != null)
            {

                ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idCate);
            }
            else
                ViewBag.drMenu = lstMenu;
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;


            return View();
        }

        //
        // POST: /News/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblAddress tbladdress, FormCollection Collection, string idCate)
        {
            if (ModelState.IsValid)
            {
                string nidCate = Collection["drMenu"];
                if (nidCate != "")
                {
                    tbladdress.idCate = int.Parse(nidCate);
                     string idUser = Request.Cookies["Username"].Values["UserID"];
                    tbladdress.idUser = int.Parse(idUser);
                    db.tblAddresses.Add(tbladdress);
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Add tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                    var listnew = db.tblNews.OrderByDescending(p => p.id).Take(1).ToList();
 
                }
                return Redirect("Index?idCate=" + nidCate);
            }

            return View(tbladdress);
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblAddress tbladdress = db.tblAddresses.Find(id);
            if (tbladdress == null)
            {
                return HttpNotFound();
            }
            
            var menuModel = db.tblGroupAddresses.OrderBy(m => m.Ord).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {

                lstMenu.Add(new SelectListItem { Text = menu.Name, Value = menu.id.ToString() });


            }



            int idGroups = int.Parse(tbladdress.idCate.ToString());
            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idGroups);
            return View(tbladdress);
        }

        //
        // POST: /News/Edit/5

        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Edit(tblAddress tbladdress, FormCollection collection, int id)
        {
            if (ModelState.IsValid)
            {
                int idCate = int.Parse(collection["drMenu"]);
                tbladdress.idCate = idCate;
                 string idUser = Request.Cookies["Username"].Values["UserID"];
                 db.Entry(tbladdress).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                 return Redirect("Index?idCate=" + idCate);
            }
            return View(tbladdress);
        }

        //
        // GET: /News/Delete/5


        public ActionResult DeleteConfirmed(int id)
        {
            tblAddress tbladdress = db.tblAddresses.Find(id);
             db.tblAddresses.Remove(tbladdress);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult AddressEditOrd(int txtSort, string ts)
        {
            var Product = db.tblAddresses.Find(txtSort);
            var result = string.Empty;
            Product.Ord = int.Parse(ts);
            //db.Entry(Product).State = System.Data.EntityState.Modified;
            result = "Ord Updated";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult AddressEditActive(string chk, string nchecked)
        {

            var Product = db.tblAddresses.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                Product.Active = false;
            }
            else
            { Product.Active = true; }

            //db.Entry(Product).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Active Updated.";
            return Json(new { result = result });
        }

        #region[Delete]

        public ActionResult DeleteAddress(int id)
        {

            tblAddress tbladdress = db.tblAddresses.Find(id);

            var result = string.Empty;
            db.tblAddresses.Remove(tbladdress);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete tbladdress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
             result = "Bạn đã xóa thành công.";
            return Json(new { result = result });

        }
       
        public ActionResult Command(FormCollection collection)
        {
            if (collection["btnDeleteAll"] != null)
            {
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chkitem+"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 8));
                            var Del = (from emp in db.tblAddresses where emp.id == id select emp).SingleOrDefault();
                            db.tblAddresses.Remove(Del);
                            db.SaveChanges();
                          
                         }
                    }
                }
                return RedirectToAction("Index");
            }

            
              
              
            return View();
        }

  
        #endregion
        #region[Search]
        public ActionResult Search(string Name, string idCate)
        {
            if (Name != null || idCate != null)
            {
                Session["txtSearch"] = Name;
                Session["idCate"] = idCate;

            }
            return RedirectToAction("Index");
        }
        #endregion
        #endregion

    }
}
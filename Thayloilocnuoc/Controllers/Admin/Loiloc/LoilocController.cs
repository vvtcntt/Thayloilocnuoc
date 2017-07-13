using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thayloilocnuoc.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Data;
namespace Thayloilocnuoc.Controllers.Admin.Loiloc
{
    public class LoilocController : Controller
    {
        private ThayloilocnuocContext db = new ThayloilocnuocContext();

        public ActionResult Index(int? page, string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var ListLoiloc = db.tblLoilocs.ToList();

            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
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
            return View(ListLoiloc.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblLoilocs.OrderByDescending(p => p.Ord).Take(1).ToList();
            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            else
                ViewBag.Ord = "0";
            return View();
        }

        //
        // POST: /Url/Create

        [HttpPost]
        public ActionResult Create(tblLoiloc tblloiloc)
        {


            db.tblLoilocs.Add(tblloiloc);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Add Loiloc", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");
        }

        //
        // GET: /Url/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblLoiloc tblloiloc = db.tblLoilocs.Find(id);
            if (tblloiloc == null)
            {
                return HttpNotFound();
            }
            return View(tblloiloc);
        }

        //
        // POST: /Url/Edit/5

        [HttpPost]
        public ActionResult Edit(tblLoiloc tblloiloc)
        {
            if (ModelState.IsValid)
            {


                db.Entry(tblloiloc).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit Loiloc", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return RedirectToAction("Index");
            }
            return View(tblloiloc);
        }

        //
        // GET: /Url/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblLoiloc tblloiloc = db.tblLoilocs.Find(id);
            if (tblloiloc == null)
            {
                return HttpNotFound();
            }
            return View(tblloiloc);
        }

        //
        // POST: /Url/Delete/5

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            tblLoiloc tblloiloc = db.tblLoilocs.Find(id);
            db.tblLoilocs.Remove(tblloiloc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult LoilocEditOrd(int txtSort, string ts)
        {
            var tblloiloc = db.tblLoilocs.Find(txtSort);
            var result = string.Empty;
            tblloiloc.Ord = int.Parse(ts);
            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified;
            result = "Update Ord.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord tblloiloc", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult LoilocEditActive(string chk, string nchecked)
        {

            var tblloiloc = db.tblLoilocs.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                tblloiloc.Active = false;
            }
            else
            { tblloiloc.Active = true; }

            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified; 
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active tblloiloc", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Updated Active.";
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
                            tblLoiloc tblloiloc = db.tblLoilocs.Find(id);
                            db.tblLoilocs.Remove(tblloiloc);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete tblloiloc", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                            return Redirect("Index");
                        }
                    }
                }
            }
            return RedirectToAction("Index");


        }
    }
}
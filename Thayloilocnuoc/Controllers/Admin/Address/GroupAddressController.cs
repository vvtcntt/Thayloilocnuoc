using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Data.Entity;
using System.Data;
using Thayloilocnuoc.Models;

namespace Thayloilocnuoc.Controllers.Admin.Address
{
    public class GroupAddressController : Controller
    {
        // GET: GroupAddress
        private ThayloilocnuocContext db = new ThayloilocnuocContext();

        //
        // GET: /Manufacturers/

        public ActionResult Index(int? page, string text, string idCate)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            string txtSearch = "";
            if (idCate != "" && idCate != null)
            {
                ViewBag.idMenu = idCate;
            }
            var listAddress = (from p in db.tblGroupAddresses select p).ToList();
            if (txtSearch != null && txtSearch != "")
            {
                listAddress = db.tblGroupAddresses.Where(p => p.Name.Contains(txtSearch)).ToList();
            }
            Session["txtSearch"] = null;
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
            if (Request.IsAjaxRequest())
            {

                if (text != null && text != "")
                {
                    var list = db.tblGroupAddresses.Where(p => p.Name.ToUpper().Contains(text.ToUpper())).ToList();
                    return PartialView("groupAddressPartial", list);
                }


                else
                {
                    return PartialView("groupAddressPartial", listAddress);
                }
            }

            return View(listAddress.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Manufacturers/Details/5
        public PartialViewResult groupAddressPartial()
        {
            return PartialView();
        }
        public ActionResult Create(string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var pro = db.tblGroupAddresses.OrderByDescending(p => p.Ord).Take(1).ToList();

            if (pro.Count > 0)
                ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }
        // POST: /Manufacturers/Create

        [HttpPost]
        public ActionResult Create(tblGroupAddress tblgroupAddress, FormCollection Collection)
        {
            if (ModelState.IsValid)
            {
                string nidCate = Collection["drMenu"];
                if (nidCate != "")
                {
                     tblgroupAddress.Tag = StringClass.NameToTag(tblgroupAddress.Name);
                    string idUser = Request.Cookies["Username"].Values["UserID"];
                     db.tblGroupAddresses.Add(tblgroupAddress);
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Add tblgroupAddress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion

                }
                var listmanufactures = db.tblGroupAddresses.OrderByDescending(p => p.id).Take(1).ToList();
                //clsSitemap.CreateSitemap("6/" + StringClass.NameToTag(tblManufacture.Name), listmanufactures[0].id.ToString(), "Manufactures");
                return RedirectToAction("Index");
            }

            return View(tblgroupAddress);
        }

        //
        // GET: /Manufacturers/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblGroupAddress tblgroupAddress = db.tblGroupAddresses.Find(id);
            if (tblgroupAddress == null)
            {
                return HttpNotFound();
            }




            return View(tblgroupAddress);
        }

        //
        // POST: /Manufacturers/Edit/5

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(tblGroupAddress tblgroupAddress, FormCollection collection, int id)
        {
            if (ModelState.IsValid)
            {

                 string idUser = Request.Cookies["Username"].Values["UserID"];
                tblgroupAddress.Tag = StringClass.NameToTag(tblgroupAddress.Name);
                db.Entry(tblgroupAddress).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tblgroupAddress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                //clsSitemap.UpdateSitemap("6/"+StringClass.NameToTag(tblManufacture.Name),id.ToString(), "Manufactures");
                return RedirectToAction("Index");
            }
            return View(tblgroupAddress);
        }

        //
        // GET: /Manufacturers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblGroupAddress tblgroupAddress = db.tblGroupAddresses.Find(id);
            if (tblgroupAddress == null)
            {
                return HttpNotFound();
            }
            return View(tblgroupAddress);
        }

        //
        // POST: /Manufacturers/Delete/5

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            tblGroupAddress tblgroupAddress = db.tblGroupAddresses.Find(id);
            db.tblGroupAddresses.Remove(tblgroupAddress);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult groupAddressEditOrd(int txtSort, string ts)
        {
            var Product = db.tblGroupAddresses.Find(txtSort);
            var result = string.Empty;
            Product.Ord = int.Parse(ts);
            //db.Entry(Product).State = System.Data.EntityState.Modified;
            result = "Ord Updated";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord tblgroupAddress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult groupAddressEditActive(string chk, string nchecked)
        {
            var Product = db.tblGroupAddresses.Find(int.Parse(chk));
            var result = string.Empty;
            if (nchecked == "true")
            {
                Product.Active = false;
            }
            else
            { Product.Active = true; }
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Active groupAddress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Active Updated.";
            return Json(new { result = result });
        }

        #region[Delete]

        public ActionResult DeleteGroupAddress(int id)
        {
            clsSitemap.DeteleSitemap(id.ToString(), "Manufactures");
            tblGroupAddress tblgroupAddress = db.tblGroupAddresses.Find(id);

            var result = string.Empty;
            db.tblGroupAddresses.Remove(tblgroupAddress);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete tblgroupAddress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Bạn đã xóa thành công.";
            return Json(new { result = result });

        }
        [HttpPost]
        public string CheckValue(string text)
        {
            string chuoi = "";
            var listProduct = db.tblGroupAddresses.Where(p => p.Name == text).ToList();
            if (listProduct.Count > 0)
            {
                chuoi = "Duplicate Name !";

            }
            Session["Check"] = listProduct.Count;
            return chuoi;
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
                            var Del = (from emp in db.tblGroupAddresses where emp.id == id select emp).SingleOrDefault();
                            db.tblGroupAddresses.Remove(Del);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete tblgroupAddress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                         }
                    }
                }
                return RedirectToAction("Index");
            }


            return View();
        }
 
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
    }
}
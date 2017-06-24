using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thayloilocnuoc.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
namespace Thayloilocnuoc.Controllers.Admin.Product
{
    public class GroupProductController : Controller
    {
        private ThayloilocnuocContext db = new ThayloilocnuocContext();

        //
        // GET: /GroupProduct/

        public ActionResult Index(int?page, string id)
        {
             if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var listProduct = db.tblGroupProducts.Where(p => p.Level.Length == 5).OrderBy(p=>p.Ord).ToList();
            if (id != "" && id != null)
            {
                int idmenu = int.Parse(id);
                var menucha = db.tblGroupProducts.Find(idmenu);
                string Lever = menucha.Level;
                int dodai = menucha.Level.Length;
                listProduct = db.tblGroupProducts.Where(p => p.Level.Length == (dodai + 5) && p.Level.Substring(0, dodai) == Lever).OrderBy(p=>p.Ord).ToList();
                ViewBag.Idcha = id;
            }
            else
            {
                ViewBag.Idcha = 0;
            }
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
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /GroupProduct/Details/5

     
        
        public ActionResult Create(int id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var menuName = db.tblGroupProducts.ToList();
            var pro = db.tblGroupProducts.OrderByDescending(p => p.Ord).Take(1).ToList();
            var GroupsProducts = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupsProducts.Select(t => new SelectListItem { Text = "" +  StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture)}));
            var menuModel = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {
                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.Id.ToString() });

            }

            var ListManufactures = db.tblManufactures.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            //var menuName = db.Menus.ToList();

            ViewBag.drManu = new SelectList(ListManufactures, "id", "Name");

            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);
            ViewBag.Ord = pro[0].Ord + 1;
            return View();
        }

        //
        // POST: /GroupProduct/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblGroupProduct tblgroupproduct, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                if ((Request.Cookies["Username"] == null))
                {
                    return RedirectToAction("LoginIndex", "Login");
                }
                string drMenu = collection["drMenu"];
                string drManu = collection["drManu"];
                string nLevel;

                if (drMenu == "")
                {
                    nLevel = "00000";
                }
                else
                {
                    var dbLeve = db.tblGroupProducts.Find(int.Parse(drMenu));
                    nLevel = dbLeve.Level + "00000";
                }
                if (drManu == "")
                {
                    tblgroupproduct.idManu = 0;
                }
                else
                {
                    tblgroupproduct.idManu = int.Parse(drManu);
                }
                tblgroupproduct.Level = nLevel;
                tblgroupproduct.DateCreate = DateTime.Now;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblgroupproduct.idUser = int.Parse(idUser);

                tblgroupproduct.Tag = StringClass.NameToTag(tblgroupproduct.Name);
                db.tblGroupProducts.Add(tblgroupproduct);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Add Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion

                var Groups = db.tblGroupProducts.Where(p => p.Active == true).OrderByDescending(p => p.Id).Take(1).ToList();
                string id = Groups[0].Id.ToString();
                clsSitemap.CreateSitemap("0/"+StringClass.NameToTag(tblgroupproduct.Name),id,"GroupProduct");
                return Redirect("Index?id=" + drMenu);
            }

            return View(tblgroupproduct);
        }

        //
        // GET: /GroupProduct/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            tblGroupProduct tblgroupproduct = db.tblGroupProducts.Find(id);
            if (tblgroupproduct == null)
            {
                return HttpNotFound();


            }
            var menuName = db.tblGroupProducts.ToList();
            var pro = db.tblGroupProducts.OrderByDescending(p => p.Ord).Take(1).ToList();
            var GroupsProducts = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupsProducts.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {
               
                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.Id.ToString() });

               
            }



            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);

            var ListManu = db.tblManufactures.ToList();

            ViewBag.drManu = new SelectList(ListManu, "id", "Name", tblgroupproduct.idManu);
            return View(tblgroupproduct);
        }

        //
        // POST: /GroupProduct/Edit/5
 [HttpPost]
        public ActionResult GroupProductEditOrd(int txtSort, string ts)
        {
            var MenuGroupsProduct = db.tblGroupProducts.Find(txtSort);
            var result = string.Empty;
            MenuGroupsProduct.Ord = int.Parse(ts);
            //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified;
            result = "Update Ord.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Update Ord GroupsProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result=result });
        }
       [HttpPost]
           public ActionResult GroupProductEditActive(string chk, string nchecked)
           {

               var MenuGroupsProduct = db.tblGroupProducts.Find(int.Parse(chk));
               var result = string.Empty;
               if (nchecked == "true")
               {
                   MenuGroupsProduct.Active = false;
               }
               else
               { MenuGroupsProduct.Active = true; }
              
               //db.Entry(MenuGroupsProduct).State = System.Data.EntityState.Modified; 
               db.SaveChanges();
               #region[Updatehistory]
               Updatehistoty.UpdateHistory("Update Active GroupsProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
               #endregion
               result = "Updated Active.";
               return Json(new { result = result });
           }
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tblGroupProduct tblgroupproduct, FormCollection collection, int id)
        {
            if (ModelState.IsValid)
            {
                clsSitemap.UpdateSitemap(StringClass.NameToTag("0/" + tblgroupproduct.Name), id.ToString(), "GroupProduct");
                string drMenu = collection["drMenu"];
                string drManu = collection["drManu"];
                string nLevel="";
                
                if (drMenu == "")
                {
                    nLevel = "00000"; tblgroupproduct.Level = nLevel;
                    tblgroupproduct.Tag = StringClass.NameToTag(tblgroupproduct.Name);
                    tblgroupproduct.DateCreate = DateTime.Now;
                    if (drManu == "")
                        tblgroupproduct.idManu = 0;
                    else
                        tblgroupproduct.idManu = int.Parse(drManu);
                    string idUser = Request.Cookies["Username"].Values["UserID"];
                    tblgroupproduct.idUser = int.Parse(idUser);
                    db.Entry(tblgroupproduct).State = EntityState.Modified;
                    db.SaveChanges();
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Edit GroupsProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                    return Redirect("Index?id=" + drMenu);
                }
                else
                {
                    int idMenu=int.Parse(drMenu);
                    var Listda = db.tblGroupProducts.First(p => p.Id == idMenu);
                    if (drMenu == id.ToString())
                    {
                        nLevel = Listda.Level; 
                        tblgroupproduct.Tag = StringClass.NameToTag(tblgroupproduct.Name);
                        tblgroupproduct.Level = nLevel;
                        tblgroupproduct.DateCreate = DateTime.Now;
                        if (drManu == "")
                            tblgroupproduct.idManu = 0;
                        else
                        tblgroupproduct.idManu = int.Parse(drManu);

                        string idUser = Request.Cookies["Username"].Values["UserID"];
                        tblgroupproduct.idUser = int.Parse(idUser);
                        db.Entry(Listda).CurrentValues.SetValues(tblgroupproduct); 
                        db.SaveChanges();
                        #region[Updatehistory]
                        Updatehistoty.UpdateHistory("Edit GroupsProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                        #endregion
                        if (nLevel.Length > 5)
                        {
                            var list = db.tblGroupProducts.First(p => p.Level == nLevel.Substring(0, nLevel.Length - 5));
                            return Redirect("Index?id=" + list.Id);
                        }
                        else
                            return Redirect("Index");
                    }
                    else
                    {
                        nLevel = Listda.Level + "00000"; tblgroupproduct.Level = nLevel;
                        tblgroupproduct.Tag = StringClass.NameToTag(tblgroupproduct.Name);
                        tblgroupproduct.DateCreate = DateTime.Now;
                        if (drManu == "")
                            tblgroupproduct.idManu = 0;
                        else
                            tblgroupproduct.idManu = int.Parse(drManu);

                        string idUser = Request.Cookies["Username"].Values["UserID"];
                        tblgroupproduct.idUser = int.Parse(idUser);
                        db.Entry(tblgroupproduct).State = EntityState.Modified;
                        db.SaveChanges();
                        #region[Updatehistory]
                        Updatehistoty.UpdateHistory("Edit GroupsProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                        #endregion
                        return Redirect("Index?id=" + drMenu);
                    }
                    
                }
               
               
                
            }
            return View(tblgroupproduct);
        }

        //
        // GET: /GroupProduct/Delete/5

        public ActionResult Delete(int id)
        {
            tblGroupProduct tblgroupproduct = db.tblGroupProducts.Find(id);
            if (tblgroupproduct == null)
            {
                return HttpNotFound();
            }
            clsSitemap.DeteleSitemap(id.ToString(), "GroupProduct");
            return RedirectToAction("Index");
        }

        //
        // POST: /GroupProduct/Delete/5


        public ActionResult DeleteAll(FormCollection collection)
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
                            var ListLevel = db.tblGroupProducts.First(p => p.Id == id);
                            string LevelParent = ListLevel.Level;
                            int Length = LevelParent.Length;
                            var ListGroupsProduct = db.tblGroupProducts.Where(p => p.Level.Length > Length && p.Level.Substring(0, Length) == LevelParent).ToList();
                            for (int i = 0; i < ListGroupsProduct.Count; i++)
                            {
                                var idChild = ListGroupsProduct[i].Id;
                                var ListChild = db.tblGroupProducts.First(p => p.Id == idChild);
                                var listProduct = db.tblProducts.Where(p => p.idCate == ListChild.Id).ToList();
                                for (int j = 0; j < listProduct.Count; j++)
                                {

                                    db.tblProducts.Remove(listProduct[i]);
                                }
                                db.tblGroupProducts.Remove(ListChild);

                            }
                            clsSitemap.DeteleSitemap(id.ToString(), "GroupProduct");

                            db.tblGroupProducts.Remove(ListLevel);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete GroupsProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                            return Redirect("Index");
                        }
                    }
                }
            }
            return RedirectToAction("Index");
             

        }
        public ActionResult DeleteConfirmed(int id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            var ListLevel = db.tblGroupProducts.First(p => p.Id == id);
            string LevelParent = ListLevel.Level;
            int Length = LevelParent.Length;
            var ListGroupsProduct = db.tblGroupProducts.Where(p => p.Level.Length > Length && p.Level.Substring(0, Length) == LevelParent).ToList();
            for (int i = 0; i < ListGroupsProduct.Count; i++)
            {
                var idChild = ListGroupsProduct[i].Id;
                var ListChild = db.tblGroupProducts.First(p => p.Id == idChild);
                var listProduct = db.tblProducts.Where(p => p.idCate == ListChild.Id).ToList();
                for (int j = 0; j < listProduct.Count; j++)
                {

                    db.tblProducts.Remove(listProduct[i]);
                }
                db.tblGroupProducts.Remove(ListChild);
                clsSitemap.DeteleSitemap(ListGroupsProduct[i].Id.ToString(), "GroupProduct");

            }
            clsSitemap.DeteleSitemap(id.ToString(), "GroupProduct");
            db.tblGroupProducts.Remove(ListLevel);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete GroupsProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Redirect("Index");



             
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
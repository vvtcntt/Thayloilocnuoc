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
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Xml;
namespace Thayloilocnuoc.Controllers.Admin.Productad
{
    public class ProductadController : Controller
    {
        private ThayloilocnuocContext db = new ThayloilocnuocContext();

        public ActionResult Index(int? page, string text, string idCate)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            string txtSearch = "";
            var listProduct = (from p in db.tblProducts orderby (p.Ord) select p).ToList();

            if (idCate != "" && idCate != null)
            {
                int idcates = int.Parse(idCate);
                listProduct = db.tblProducts.Where(p => p.idCate == idcates).OrderByDescending(p => p.Ord).ToList();
                ViewBag.idMenu = idCate;
            }

            if (txtSearch != null && txtSearch != "")
            {
                listProduct = db.tblProducts.Where(p => p.Name.Contains(txtSearch)).ToList();
            }
            Session["txtSearch"] = null;
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
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

            if (idCate != "")
            {

                ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idCate);
            }
            else
            {
                ViewBag.drMenu = lstMenu;
            }
            #endregion

            if (Request.IsAjaxRequest())
            {
                int idCatelogy;
                if (text != null && text != "")
                {
                    var list = db.tblProducts.Where(p => p.Name.ToUpper().Contains(text.ToUpper())).ToList();
                    return PartialView("PartialProductData", list);
                }
                if (idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate);
                    var list = db.tblProducts.Where(p => p.idCate == idCatelogy).ToList(); ViewBag.idMenu = idCate;
                    return PartialView("PartialProductData", list);
                }
                if (text != null && text != "" && idCate != null && idCate != "")
                {
                    idCatelogy = int.Parse(idCate);
                    ViewBag.idMenu = idCate;
                    var list = db.tblProducts.Where(p => p.Name.ToUpper().Contains(text.ToUpper()) && p.idCate == (int.Parse(idCate))).ToList();
                    return PartialView("PartialProductData", list);
                }

                else
                {
                    return PartialView("PartialProductData", listProduct);
                }
            }


            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UpdateImage(tblProduct tblproduct)
        {
            //var listProduct = db.tblNews.ToList();
            //for (int i = 0; i < listProduct.Count; i++)
            //{
            //    int id = int.Parse(listProduct[i].id.ToString());
            //    var Product = db.tblNews.First(p => p.id == id);
            //    Product.ImageLinkThumb = Product.ImageLinkThumb.Remove(0, 1).ToString();
            //    db.SaveChanges();
            //}
            var listProduct = db.tblGroupNews.ToList();
            for (int i = 0; i < listProduct.Count; i++)
            {
                int id = int.Parse(listProduct[i].id.ToString());
                var Product = db.tblGroupNews.First(p => p.id == id);
                string tags = Product.Name;
                Product.Tag = StringClass.NameToTag(tags);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        //
        // GET: /Product/Details/5
        public PartialViewResult PartialProductData()
        {

            return PartialView();

        }

        //
        // GET: /Product/Create

        public ActionResult Create(int? id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }

            var pro = db.tblProducts.OrderByDescending(p => p.Ord).Take(1).ToList();
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

            if ( id != null)
            {
                if (pro.Count > 0)
                {
                    ViewBag.Ord = pro[0].Ord + 1;

                     int idMenus = int.Parse(db.tblGroupProducts.First(p => p.Id == id).idManu.ToString());
                    var Menufactures = db.tblManufactures.First(p => p.id == idMenus);
                    ViewBag.chuoithu = "<span class=\"Aler_Manu\" style=\"color: #F00\">" + Menufactures.Name + "</span>";

                }
                ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", id);
            }
            else
                ViewBag.drMenu = lstMenu;
           
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblProduct tblproduct, FormCollection Collection, string id)
        {

            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            string nidCate = Collection["drMenu"];
            if (nidCate != "")
            {
                tblproduct.idCate = int.Parse(nidCate);
                tblproduct.DateCreate = DateTime.Now;
                tblproduct.Tag = StringClass.NameToTag(tblproduct.Name);
                tblproduct.DateCreate = DateTime.Now;
                tblproduct.Visit = 0;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblproduct.idUser = int.Parse(idUser);
                db.tblProducts.Add(tblproduct);
                db.SaveChanges();
                var listprro = db.tblProducts.OrderByDescending(p => p.id).Take(1).ToList();
                clsSitemap.CreateSitemap("1/" + StringClass.NameToTag(tblproduct.Name), listprro[0].id.ToString(), "Product");
            }
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Add Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion

           
            return Redirect("Index?idCate=" + id);

        }

        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            Int32 ids = Int32.Parse(id.ToString());
            tblProduct tblproduct = db.tblProducts.Find(ids);
            if (tblproduct == null)
            {
                return HttpNotFound();
            }
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

            int idGroups = int.Parse(tblproduct.idCate.ToString());
            ViewBag.drMenu = new SelectList(lstMenu, "Value", "Text", idGroups);


            int idmenu1 = int.Parse(tblproduct.idCate.ToString());
            int idManus = int.Parse(db.tblGroupProducts.First(p => p.Id == idmenu1).idManu.ToString());
            var Menufactures = db.tblManufactures.First(p => p.id == idManus);
            ViewBag.chuoithu = "<span class=\"Aler_Manu\" style=\"color: #F00\">" + Menufactures.Name + "</span>";
            return View(tblproduct);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tblProduct tblproduct, FormCollection collection,int id)
        {

            if (ModelState.IsValid)
            {
                if (collection["drMenu"] != "" || collection["drMenu"] != null)
                {
                    int idCate = int.Parse(collection["drMenu"]);
                    tblproduct.idCate = idCate;
                    tblproduct.DateCreate = DateTime.Now;
                    string tag = tblproduct.Tag;
                    string URL = collection["URL"];
                    tblproduct.Visit = tblproduct.Visit;
                    if (URL == "on")
                    {
                        tblproduct.Tag = StringClass.NameToTag(tblproduct.Name);
                        clsSitemap.UpdateSitemap("1/"+StringClass.NameToTag(tblproduct.Name), id.ToString(),"Product");
                    }
                    else
                    {
                        tblproduct.Tag = collection["NameURL"];
                        clsSitemap.UpdateSitemap("1/" + collection["NameURL"], id.ToString(), "Product");
                    }

                    string idUser = Request.Cookies["Username"].Values["UserID"];
                    tblproduct.idUser = int.Parse(idUser);
                    db.Entry(tblproduct).State = EntityState.Modified;
                    db.SaveChanges();
                   
                }
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                return Redirect("Index?idCate=" + int.Parse(collection["drMenu"]));
            }
            return View(tblproduct);
        }
        public ActionResult DeleteConfirmed(int id)
        {
            tblProduct tblproduct = db.tblProducts.Find(id);
            clsSitemap.DeteleSitemap(id.ToString(), "Product");
            db.tblProducts.Remove(tblproduct);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult ProductEditOrd(int txtSort, string ts)
        {
            var Product = db.tblProducts.Find(txtSort);
            var result = string.Empty;
            Product.Ord = int.Parse(ts);
            //db.Entry(Product).State = System.Data.EntityState.Modified;
            result = "Ord Updated.";
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Edit Ord Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return Json(new { result = result });
        }
        [HttpPost]
        public ActionResult ProductEditActive(string chk, string nchecked)
        {

            var Product = db.tblProducts.Find(int.Parse(chk));
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
            Updatehistoty.UpdateHistory("Edit  Active Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Active Updated.";
            return Json(new { result = result });
        }

        #region[Delete]

        public ActionResult DeleteProduct(int id)
        {
            tblProduct tblproduct = db.tblProducts.Find(id);
            clsSitemap.DeteleSitemap(id.ToString(), "Product");

            var result = string.Empty;
            db.tblProducts.Remove(tblproduct);
            db.SaveChanges();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Delete Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            result = "Bạn đã xóa thành công.";
            return Json(new { result = result });

        }
        [HttpPost]
        public string Manufactures(string idCate)
        {
            int idCates = int.Parse(idCate);
            string chuoi = "";
            var listProduct = db.tblGroupProducts.First(p => p.Id == idCates);
            if(listProduct!=null)
            {
            int idManu = int.Parse(listProduct.idManu.ToString());
            var ListManu = db.tblManufactures.First(p => p.id == idManu);
            if (ListManu != null ) {
                chuoi = ListManu.Name;
            }
            else
            {
                chuoi = "Chưa có nhà sản xuất";

            }
            }
            return chuoi;
        }
        [HttpPost]
        public string CheckValue(string text)
        {
            string chuoi = "";
            var listProduct = db.tblProducts.Where(p => p.Name == text).ToList();
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
                            clsSitemap.DeteleSitemap(id.ToString(), "Product");

                            var Del = (from emp in db.tblProducts where emp.id == id select emp).SingleOrDefault();
                            db.tblProducts.Remove(Del);
                            db.SaveChanges();
                            #region[Updatehistory]
                            Updatehistoty.UpdateHistory("Delete Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                            #endregion
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            if (collection["btnExport"] != null)
            {
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Export  Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                GridView gv = new GridView();

                var listid = 0;
                List<int> exceptionList = new List<int>();
                foreach (string key in Request.Form.Keys)
                {
                    var checkbox = "";
                    if (key.StartsWith("chkitem+"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            int id = Convert.ToInt32(key.Remove(0, 8));
                            exceptionList.Add(id);
                        }
                    }
                }


                gv.DataSource = db.tblProducts.Where(x => exceptionList.Contains(x.id)).ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Marklist.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Index");
            }
            if (collection["btlPrinter"] != null)
            {


                List<int> exceptionList = new List<int>();
                foreach (string key in Request.Form)
                {
                    var checkbox = "";

                    if (key.StartsWith("chkitem+"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            int idp = int.Parse(key.Remove(0, 8));
                            //  int iddd = int.Parse(idp);
                            //  var sp = db.tblProducts.Where(m => m.id == iddd).First();
                            //  chuoi+= "<tr><td>Tên sản phẩm</td><td>"+sp.Name+"</td></tr>";
                            //chuoi+="<tr> <td></td><td></td> </tr>";
                            // chuoi+=" <tr> <td>Mã sản phẩm</td><td>"+2017+"</td></tr>";
                            //chuoi = chuoi + "," + key.Remove(0, 8);
                            exceptionList.Add(idp);
                            //dem = dem + 1;

                        }
                    }
                }
                var list = db.tblProducts.Where(x => exceptionList.Contains(x.id)).ToList();
                return View("Printer", list);
            }
            return View();
        }

        [HttpPost]
        public ActionResult print(FormCollection a)
        {
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Printer Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            string chuoi = "";
            chuoi = "<script type=\"text/javascript\">$(document).ready(function() {window.print();});</script>";
            ViewBag.Print = chuoi;
            return View("Printer");
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
        #region[Export]
        [HttpPost]
        public ActionResult ExportData(FormCollection collection)
        {
            GridView gv = new GridView();

            var listid = 0;
            List<int> exceptionList = new List<int>();
            foreach (string key in Request.Form.Keys)
            {
                var checkbox = "";
                if (key.StartsWith("chkitem+"))
                {
                    checkbox = Request.Form["" + key];
                    if (checkbox != "false")
                    {
                        int id = Convert.ToInt32(key.Remove(0, 8));
                        exceptionList.Add(id);
                    }
                }
            }


            gv.DataSource = db.tblProducts.Where(x => exceptionList.Contains(x.id)).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Marklist.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Export Excel Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            return View();
        }
        #endregion


    }
}
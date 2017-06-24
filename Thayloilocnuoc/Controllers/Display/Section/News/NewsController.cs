using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thayloilocnuoc.Models;
using PagedList;
using PagedList.Mvc;
namespace Thayloilocnuoc.Controllers.Display.Section.News
{
    public class NewsController : Controller
    {
        //
        // GET: /News/
        ThayloilocnuocContext db = new ThayloilocnuocContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewsDetail(string tag)
        {
            tblNew dblnew = db.tblNews.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + dblnew.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + dblnew.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + dblnew.Keyword + "\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + dblnew.Title + "\" />";
            if(dblnew.Meta!=null && dblnew.Meta!="")
            {
                int phut = DateTime.Now.Minute * 2;
                ViewBag.refresh = "<meta http-equiv=\"refresh\" content=\"" + phut + "; url=" + dblnew.Meta + "\">"; 
            }
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + dblnew.Description + "\" />";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://thayloilocnuoc.com/3/" + StringClass.NameToTag(tag) + "\" />";

            meta += "<meta itemprop=\"name\" content=\"" + dblnew.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + dblnew.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://thayloilocnuoc.com" + dblnew.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + dblnew.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://thayloilocnuoc.com" + dblnew.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://thayloilocnuoc.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + dblnew.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta; 
            int iduser = int.Parse(dblnew.idUser.ToString());
            ViewBag.User = db.tblUsers.First(p => p.id == iduser).UserName;
            int idcate = int.Parse(dblnew.idCate.ToString());
            var GroupNews = db.tblGroupNews.First(p => p.id == idcate);
            ViewBag.name = GroupNews.Name;
            int dodai = GroupNews.Level.Length / 5;
            string nUrl = "";
            for (int i = 0; i < dodai; i++)
            {
                int leht = GroupNews.Level.Substring(0, (i + 1) * 5).Length;
                var NameGroups = db.tblGroupNews.First(p => p.Level.Substring(0, (i + 1) * 5) == GroupNews.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
                nUrl = nUrl + "  <a href=\"/2/" + NameGroups.Tag + "\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + " /</a>  ";
            }
            ViewBag.nUrl = " <a href=\"/\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"icon_Homes\"></span>Trang chủ /</a> " + nUrl + " ";

           string tab = dblnew.Tabs;
           string Tabsnews = "";
           if (tab != null)
           {  
               List<int> mangidPd = new List<int>();

               string[] mang = tab.Split(',');
               int id = int.Parse(dblnew.id.ToString());
               string chuoitab = "";
               Tabsnews="<div class=\"Tabs\">";
               Tabsnews += " <span>Tags</span>";
               for (int i = 0; i < mang.Length; i++)
               {

                   Tabsnews += "<h2> <a href=\"/TagNews/" + StringClass.NameToTag(mang[i]) + "\" title=\"" + mang[i] + "\">" + mang[i] + "</a></h2>";
                
                   string tabs = mang[i];
                   var lnews = db.tblNews.Where(p => p.Tabs.Contains(tabs) && p.Active==true).ToList();
                   for(int j=0;j<lnews.Count;j++)
                   {
                       int idn=int.Parse(lnews[j].id.ToString());
                       mangidPd.Add(idn);
                   }

               }
               Tabsnews+="</div>";
               ViewBag.tabsnews = Tabsnews;
               var listnews = db.tblNews.Where(p => mangidPd.Contains(p.id) && p.Active == true && p.id!=id).OrderByDescending(p => p.DateCreate).Take(2).ToList();
               if (listnews.Count > 0)
               {
                   chuoitab += "<div class=\"Tintuclienquan\">";
                   for (int j = 0; j < listnews.Count; j++)
                   {
                       chuoitab += " <h3><a href=\"/3/" + listnews[j].Tag + "\" title=\"" + listnews[j].Name + "\">› " + listnews[j].Name + "</a><h3>";

                   } chuoitab += " </div>";
               }
               ViewBag.chuoitab = chuoitab;
           }
           ViewBag.favicon = " <link href=\"" + db.tblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            return View(dblnew);
        }
        public PartialViewResult RightNews()
        {
            return PartialView();
        }
        public PartialViewResult Tintuclienquan(string tag)
        {
            tblNew tblnew = db.tblNews.First(p => p.Tag == tag);

            int idcate = int.Parse(tblnew.idCate.ToString());
            string chuoi = "";
            var listnew = db.tblNews.Where(p => p.idCate == idcate && p.Active == true && p.Tag != tag).OrderByDescending(p => p.DateCreate).Take(10).ToList();
            for (int i = 0; i < listnew.Count; i++)
            {
                chuoi += "    <h3><a href=\"/3/" + listnew[i].Tag + "\" rel=\"nofollow\" title=\"" + listnew[i].Name + "\">" + listnew[i].Name + " </a></h3> ";
            }
            ViewBag.chuoi = chuoi;
            return PartialView();
        }
        public ActionResult ListNews(string tag, int? page)
        {
            tblGroupNew groupnews = db.tblGroupNews.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + groupnews.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupnews.Keyword + "\" /> ";
            int idcate = int.Parse(groupnews.id.ToString());
            ViewBag.Name = groupnews.Name;
            var Listnews = db.tblNews.Where(p => p.idCate == idcate && p.Active==true).OrderByDescending(p => p.DateCreate).ToList();
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
            ViewBag.name = groupnews.Name;
            int dodai = groupnews.Level.Length / 5;
            string nUrl = "";
            for (int i = 0; i < dodai; i++)
            {
                int leht = groupnews.Level.Substring(0, (i + 1) * 5).Length;
                var NameGroups = db.tblGroupNews.First(p => p.Level.Substring(0, (i + 1) * 5) == groupnews.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
                nUrl = nUrl + "  <a href=\"/2/" + NameGroups.Tag + "\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + " /</a>  ";
            }
            ViewBag.nUrl = " <a href=\"/\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"icon_Homes\"></span>Trang chủ /</a> " + nUrl + " ";
            ViewBag.favicon = " <link href=\"" + db.tblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            return View(Listnews.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Tabsnews(string tag, int? page)
        {
            tag = StringClass.NameToTag(tag); ;
            string[] Mang1 = StringClass.COnvertToUnSign1(tag).Split('-');
            string chuoitag = "";
            for (int i = 0; i < Mang1.Length; i++)
            {
                if (i == 0)
                    chuoitag += Mang1[i];
                else
                    chuoitag += " " + Mang1[i];
            }
            int dem = 1;
            string name = "";
            List<tblNew> ListNew = (from c in db.tblNews where c.Active == true select c).ToList();
            List<tblNew> listnews = ListNew.FindAll(delegate(tblNew math)
            {
                if(math.Tabs!=null&& math.Tabs!="")
                {
                    if (StringClass.COnvertToUnSign1(math.Tabs.ToUpper()).Contains(chuoitag.ToUpper()))
                    {

                        string[] Manghienthi = math.Tabs.Split(',');
                        foreach (var item in Manghienthi)
                        {
                            if (dem == 1)
                            {
                                var kiemtra = StringClass.COnvertToUnSign1(item.ToUpper()).Contains(chuoitag.ToUpper());
                                if (kiemtra == true)
                                {
                                    name = item;
                                    dem = 0;
                                }
                            }
                        }

                        return true;
                    }
                    else
                        return false;
                }
              

                else
                    return false;
            }
            );
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
            ViewBag.name = name;

            ViewBag.Title = "<title>" + name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + name + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
            ViewBag.favicon = " <link href=\"" + db.tblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Thayloilocnuoc.com/TagNews/" + StringClass.NameToTag(chuoitag) + "\" />"; ;

            return View(listnews.ToPagedList(pageNumber, pageSize));
         }
        public PartialViewResult Boxfacebook()
        {
            tblConfig tblconfig = db.tblConfigs.First();

            return PartialView(tblconfig);
        }
	}
}
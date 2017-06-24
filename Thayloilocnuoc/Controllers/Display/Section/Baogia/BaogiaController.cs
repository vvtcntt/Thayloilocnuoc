using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thayloilocnuoc.Models;
namespace Thayloilocnuoc.Controllers.Display.Section.Baogia
{
    public class BaogiaController : Controller
    {
        //
        // GET: /Baogia/
        ThayloilocnuocContext db = new ThayloilocnuocContext();
        public PartialViewResult Homes_Baogia()
        {
            string chuoi = "";
            var listbaogia = db.tblGroupProducts.Where(p => p.Active == true && p.Baogia == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listbaogia.Count;i++ )
            {
                int idManu = int.Parse(listbaogia[i].idManu.ToString());
                var listManufacture = db.tblManufactures.First(p => p.id == idManu);

               chuoi+="<div class=\"Tear_Baogias\">";
               chuoi += "<a href=\"/Bao-gia/Bao-gia-" + listbaogia[i].Tag + "\" title=\"Báo giá " + listbaogia[i].Name + "\"><img src=\"" + listManufacture.Images + "\" alt=\"Báo giá " + listbaogia[i].Name + "\" /></a>";
               chuoi += "<a href=\"/Bao-gia/Bao-gia-" + listbaogia[i].Tag + "\" title=\"Báo giá " + listbaogia[i].Name + "\" class=\"Name\">Báo giá " + listbaogia[i].Name + "</a>";
             chuoi+="</div>";
            }
            ViewBag.chuoi = chuoi;
                return PartialView();
        }
        public ActionResult Index(string tag)
        {
            tblConfig tblcongif = db.tblConfigs.First();

            tblGroupProduct groupproduct = db.tblGroupProducts.First(p => p.Tag == tag.Substring(8,(tag.Length-1)));
            int idmenu=int.Parse(groupproduct.Id.ToString());
            int idManu = int.Parse(groupproduct.idManu.ToString());
            tblManufacture manufacture = db.tblManufactures.Find(idManu);
            ViewBag.Namemanu = manufacture.Name;
            ViewBag.Info = manufacture.Content;
            ViewBag.name = groupproduct.Name; ViewBag.favicon = " <link href=\"" + groupproduct.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            ViewBag.imagemanu = manufacture.Images;
            string moth = "";
            int moths = int.Parse(DateTime.Now.Month.ToString());
            if (moths <= 3)
            {
                moth = "tháng 1,2,3 ";
            }
            else if (moths > 3 && moths <= 6)
            {
                moth = "tháng 4,5,6 ";
            }
            else if (moths > 6 && moths <= 9)
            {
                moth = "tháng 7,8,9 ";
            }
            else if (moths >= 9 && moths <= 12)
            {
                moth = "tháng 10,11,12 ";
            }
            
            ViewBag.Title = "<title>Bảng báo giá " + groupproduct.Name + " " + moth + "năm " + DateTime.Now.Year + " rẻ nhất HN</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Bảng báo giá " + groupproduct.Name + ", dịch vụ thay lõi lọc nước " + manufacture.Name + " tại nhà . " + tblcongif.Name + " là nhà phân phối chính thức Lõi máy lọc nước " + manufacture.Name + "  .\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Bảng Báo giá sản phẩm "+groupproduct.Name+"\" /> ";
            string chuoi = "";
            var listproduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idmenu).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listproduct.Count;i++ )
            {

                chuoi += "<tr>";
                chuoi += "<td class=\"Ords\">" + (i + 1) + "</td>";
                chuoi += "<td class=\"Names\">";
                chuoi += "<a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\">" + listproduct[i].Name + "</a></span>";
                chuoi += "<span class=\"n2\">Chức năng : " + listproduct[i].Info + "</span>";
                chuoi += "<span class=\"n3\">Chính hãng "+ manufacture.Name + " </span>";
                chuoi += " </td>";
                chuoi += "<td class=\"Codes\"><a href=\"/Tabs/" + listproduct[i].Code + "\" title=\"" + listproduct[i].Code + "\">" + listproduct[i].Code + "</a></td>";
               
                    chuoi += "<td class=\"Wans\"><a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\"><img src=\"" + listproduct[i].ImageLinkThumb + "\" alt=\"" + listproduct[i].Name + "\" title=\"" + listproduct[i].Name + "\"/></a>" + listproduct[i].Time + "</td>";
              

                chuoi += "<td class=\"Prices\">" + string.Format("{0:#,#}", listproduct[i].PriceSale) + "đ  <span class=\"n4\">Miễn phí vận chuyển và lắp đặt toàn quốc</span></td>";
                //chuoi += "<td class=\"Qualitys\">01</td>";
                //chuoi += "<td class=\"SumPrices\">" + string.Format("{0:#,#}", listproduct[i].PriceSale) + "đ</td>";
                chuoi += "<td class=\"Images\"><a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\"><img src=\"" + listproduct[i].ImageLinkThumb + "\" alt=\"" + listproduct[i].Name + "\" title=\"" + listproduct[i].Name + "\"/></a></td>";
                chuoi += "</tr>";
            }
            ViewBag.chuoi = chuoi;
            return View(tblcongif);
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thayloilocnuoc.Models;
using System.Net;
namespace CMSCODE.Controllers.Display.Product
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        float tongtien = 0;
        //
        // GET: /Order/

        ThayloilocnuocContext db = new ThayloilocnuocContext();
        
        public ActionResult OrderIndex()
        {
            int id; int Ord; int idMenu;
            if (Session["idProduct"] != "" && Session["OrdProduct"] != "" && Session["idMenu"] != "")
            {
                id = int.Parse(Session["idProduct"].ToString());
                  Ord= int.Parse(Session["OrdProduct"].ToString());
                  idMenu = int.Parse(Session["idMenu"].ToString());
                  Session["idProduct"] = "";
                  Session["OrdProduct"] = "";
                  Session["idMenu"] = "";
                  int sl = 0;
                  var Sopping = (clsGiohang)Session["giohang"];
                  if (Sopping == null)
                  {
                      Sopping = new clsGiohang();
                  }
                  if (Kiemtra(id) == 1)
                  {
                      for (int i = 0; i < Sopping.CartItem.Count; i++)
                      {
                          if (Sopping.CartItem[i].id == id)
                          {
                              Sopping.CartItem[i].Ord = Sopping.CartItem[i].Ord + Ord;
                              Sopping.CartItem[i].SumPrice = Sopping.CartItem[i].Ord * Sopping.CartItem[i].Price;
                          }
                          tongtien += Sopping.CartItem[i].SumPrice;
                      }
                      Sopping.CartTotal = tongtien;
                  }
                  else
                  {
                      var Sanpham = new clsProduct();
                      Sanpham.id = id;
                      var Product = db.tblProducts.Find(id);
                      Sanpham.Price = float.Parse(Product.Price.ToString());
                      Sanpham.Ord = Ord;
                      Sanpham.Name = Product.Name;
                      Sanpham.idMenu = idMenu;
                      Sanpham.SumPrice = Sanpham.Price * Sanpham.Ord;
                      Sanpham.Tag = Product.Tag;
                      Sopping.CartItem.Add(Sanpham);
                      for (int i = 0; i < Sopping.CartItem.Count; i++)
                      {
                          tongtien += Sopping.CartItem[i].SumPrice;
                      }
                      Sopping.CartTotal = tongtien;

                  }

                  Session["giohang"] = Sopping;
                  sl = Sopping.CartItem.Count;
                  var s = (clsGiohang)Session["giohang"];


                  Session["soluong"] = sl;
            }
        
           
           
            var giohang = (clsGiohang)Session["giohang"];
            if (Session["Status"] != "")
            {
                ViewBag.Status = Session["Status"];
                Session["Status"] = "";
            }


            ViewBag.url = Session["Url"].ToString();
            ViewBag.Title = "<title>Giỏ hàng của bạn</title>";
            ViewBag.Description = "<meta name=\"description\" content=\" Giỏ hàng đặt hàng máy lọc nước Sơn Hà dành cho khách hàng mua sản phẩm\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Giỏ hàng của bạn\" /> ";
            return View(giohang);
        }

        [HttpPost]
        public ActionResult CreateOrd(string Name,string Address,string DateByy,string Mobile,string Email,string Description)
        {
            var Sopping = (clsGiohang)Session["giohang"];
            tblOrder order = new tblOrder();
            order.Name = Name;
            order.Address = Address;
            order.DateByy = DateTime.Parse(DateByy);
            order.Mobile = int.Parse(Mobile);
            order.Email = Email;
            order.Description = Description;
            order.Active = false;
            order.Tolar = Sopping.CartTotal;
            db.tblOrders.Add(order);
            db.SaveChanges();

            tblOrderDetail orderdetail = new tblOrderDetail();
            var MaxOrd = db.tblOrders.OrderByDescending(p => p.id).Take(1).ToList();
            int idOrder = MaxOrd[0].id;
            for (int i = 0; i < Sopping.CartItem.Count; i++)
            {
                orderdetail.idProduct = Sopping.CartItem[i].id;
                orderdetail.Name = Sopping.CartItem[i].Name;
                orderdetail.Price = Sopping.CartItem[i].Price;
                orderdetail.Quantily = Sopping.CartItem[i].Ord;
                orderdetail.SumPrice = Sopping.CartItem[i].SumPrice;
                orderdetail.idOrder = idOrder;
                db.tblOrderDetails.Add(orderdetail);
                db.SaveChanges();
            }
            //Send mail cho khách

            tblConfig config = db.tblConfigs.First();
            // Gmail Address from where you send the mail
            var fromAddress = config.UserEmail;
            // any address where the email will be sending
            var toAddress = config.Email;
            //Password of your gmail address
            string fromPassword = config.PassEmail;
            // Passing the values and make a email formate to display
            string subject = "Đơn hàng mới từ";
            string body = "From: " + Name + "\n";

            body += "Tên khách hàng: " + Name + "\n";
            body += "Địa chỉ: " + Address + "\n";
            body += "Điện thoại: " + Mobile + "\n";
            body += "Email: " + Email + "\n";
            body += "Nội dung: \n" + Description + "\n";
            body += "THÔNG TIN ĐƠN ĐẶT HÀNG \n";
            ////string OrderId = clsConnect.sqlselectOneString("select max(idOrder) as MaxID from [Order]");

            //insert vao bang OrderDetail

            for (int i = 0; i < Sopping.CartItem.Count; i++)
            {
                body += "Tên sản phẩm : " + Sopping.CartItem[i].Name + "\n";
                body += "Đơn giá  :" + Sopping.CartItem[i].Price.ToString().Replace(",", "") + "\n";
                body += "Số lượng : " + Sopping.CartItem[i].Ord + "\n";


            }
            body += "Tổng giá trị đơn hàng là " + Sopping.CartTotal;
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = config.Host;
                smtp.Port = int.Parse(config.Port.ToString());
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = int.Parse(config.Timeout.ToString());
            }
            smtp.Send(fromAddress, toAddress, subject, body);


        Session["Status"] = "<script>$(document).ready(function(){ alert('Bạn đã đặt hàng thành công !') });</script>";
        return RedirectToAction("OrderIndex");
        }
[HttpPost]      
        public ActionResult OrderAdd(int id, int Ord)
        {
            id = int.Parse(Session["idProduct"].ToString());
            Ord = int.Parse(Session["OrdProduct"].ToString());
            Session["idProduct"] = "";
            Session["OrdProduct"] = "";
            int sl = 0;
            var Sopping = (clsGiohang)Session["giohang"];
            if (Sopping == null)
            {
                Sopping = new clsGiohang();
            }
            if (Kiemtra(id) == 1)
            {
                for (int i = 0; i < Sopping.CartItem.Count; i++)
                {
                    if (Sopping.CartItem[i].id == id)
                    {
                        Sopping.CartItem[i].Ord = Sopping.CartItem[i].Ord + Ord;
                        Sopping.CartItem[i].SumPrice = Sopping.CartItem[i].Ord * Sopping.CartItem[i].Price;
                    }
                    tongtien += Sopping.CartItem[i].SumPrice;
                }
                Sopping.CartTotal = tongtien;
            }
            else
            {
                var Sanpham = new clsProduct();
                Sanpham.id = id;
                var Product = db.tblProducts.Find(id);
                Sanpham.Price = float.Parse(Product.Price.ToString());
                Sanpham.Ord = Ord;
                Sanpham.Name = Product.Name;
                Sanpham.SumPrice = Sanpham.Price * Sanpham.Ord;
                Sanpham.Tag = Product.Tag;
                Sopping.CartItem.Add(Sanpham);
                for (int i = 0; i < Sopping.CartItem.Count; i++)
                {
                    tongtien += Sopping.CartItem[i].SumPrice;
                }
                Sopping.CartTotal = tongtien;

            }

         Session["giohang"] = Sopping;
         sl = Sopping.CartItem.Count;
         var s = (clsGiohang)Session["giohang"];


         Session["soluong"] = sl;
         return RedirectToAction("OrderIndex", "Order");            
        }
        public int Kiemtra(int idProduct)
        {
            int so = 0;
            var Sopping =(clsGiohang) Session["giohang"];
            if (Sopping != null)
            {
                for (int i = 0; i < Sopping.CartItem.Count; i++)
                {
                    if (Sopping.CartItem[i].id == idProduct)
                    {
                        so = 1; break;
                    }



                }

            }
            return so;
        }
        public ActionResult UpdatOder(int id, int Ord)
        {
            float tt = 0;
            var tien ="";
            int sl=0;
            var s = (clsGiohang)Session["giohang"];
            if (s != null)
            {
                for (int i = 0; i < s.CartItem.Count; i++) 
                {
                    if (id == s.CartItem[i].id) {
                        s.CartItem[i].Ord = Ord;
                        s.CartItem[i].SumPrice = Ord * s.CartItem[i].Price;
                        tien = s.CartItem[i].SumPrice.ToString();
                    }

                    tt += s.CartItem[i].SumPrice;

                }

                s.CartTotal = tt;
                sl = s.CartItem.Count;
            }
            Session["giohang"] = s;
            return Json(new{gia =string.Format("{0:#,#}", tien) ,tt=string.Format("{0:#,#}",tt) ,sl=sl});
        }
        public ActionResult DeleteOrder(int id)
        {
            var s = (clsGiohang)Session["giohang"];
            int sl = 0;
            for (int i = 0; i < s.CartItem.Count; i++)
            {
                if (s.CartItem[i].id == id)
                    s.CartItem.Remove(s.CartItem[i]);
            }
            sl = s.CartItem.Count;

            for (int i = 0; i < s.CartItem.Count; i++)
            {
                tongtien += s.CartItem[i].SumPrice;
            }
            s.CartTotal = tongtien;

            Session["soluong"] = sl;
           
            return RedirectToAction("OrderIndex");
        }
        public PartialViewResult OrderPartial()
        {
            return PartialView();
        }
        public PartialViewResult InputOrder()
        {
            return PartialView();
        }
    }
}

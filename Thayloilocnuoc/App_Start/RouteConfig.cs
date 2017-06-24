using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Thayloilocnuoc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("listnews", "2/{Tag}/{*catchall}", new { controller = "News", action = "ListNews", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^ListNews$" });
            routes.MapRoute("ListManufacturers", "9/{Tag}/{*catchall}", new { controller = "MenufacturersDisplay", action = "MenufacturerList", tag = UrlParameter.Optional }, new { controller = "^M.*", action = "^MenufacturerList$" });
            routes.MapRoute("DetailManufacturers", "NhaPhanPhoi/{Tag}/{*catchall}", new { controller = "MenufacturersDisplay", action = "MenufacturerDetail", tag = UrlParameter.Optional }, new { controller = "^M.*", action = "^MenufacturerDetail$" });
            routes.MapRoute("ChitietNew", "3/{Tag}/{*catchall}", new { controller = "News", action = "NewsDetail", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^NewsDetail$" });
            routes.MapRoute("Chi_Tiet", "1/{Tag}/{*catchall}", new { controller = "Product", action = "ProductDetail", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ProductDetail$" });
            routes.MapRoute("Danh_Sach", "0/{Tag}/{*catchall}", new { controller = "Product", action = "ListProduct", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ListProduct$" });
            routes.MapRoute("Hang-san-xuat", "Hang-san-xuat/{Tag}/{*catchall}", new { controller = "Product", action = "ListProductAgency", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ListProductAgency$" });
            routes.MapRoute("Tabs", "Tabs/{Tag}/{*catchall}", new { controller = "Product", action = "ListTabs", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^ListTabs$" });
            routes.MapRoute("URL", "Websites/{Tag}/{*catchall}", new { controller = "URLDisplay", action = "URL", tag = UrlParameter.Optional }, new { controller = "^U.*", action = "^URL$" });

            routes.MapRoute("TabAgency", "TabAgency/{Tag}/{*catchall}", new { controller = "AgencyDisplay", action = "TabsAgency", tag = UrlParameter.Optional }, new { controller = "^A.*", action = "^TabsAgency$" });
            routes.MapRoute("Tabsnews", "TagNews/{Tag}/{*catchall}", new { controller = "News", action = "Tabsnews", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^Tabsnews$" });
            routes.MapRoute("Baogia", "Bao-gia/{Tag}/{*catchall}", new { controller = "Baogia", action = "Index", tag = UrlParameter.Optional }, new { controller = "^B.*", action = "^Index$" });
            routes.MapRoute("ListAgency", "4/{Tag}/{*catchall}", new { controller = "AgencyDisplay", action = "AgencyList", tag = UrlParameter.Optional }, new { controller = "^A.*", action = "^AgencyList$" });
            routes.MapRoute("DetailAgency", "5/{Tag}/{*catchall}", new { controller = "AgencyDisplay", action = "AgencyDetail", tag = UrlParameter.Optional }, new { controller = "^A.*", action = "^AgencyDetail$" });
            routes.MapRoute("Danh_Sach_manufactures", "6/{Manu}/{*catchall}", new { controller = "Product", action = "ListProductManufactures", manu = UrlParameter.Optional }, new { controller = "^P.*", action = "^ListProductManufactures$" });
             routes.MapRoute(name: "Contact", url: "Lien-he", defaults: new { controller = "Contacts", action = "Index" });
            routes.MapRoute(name: "SearchProduct", url: "SearchProduct", defaults: new { controller = "Products", action = "SearchProduct" });
            routes.MapRoute(name: "Order", url: "Order", defaults: new { controller = "Order", action = "OrderIndex" });
            routes.MapRoute(name: "Maps", url: "Ban-do", defaults: new { controller = "MapsDisplay", action = "Mapsdetail" });
            routes.MapRoute(name: "Dang-ky-thay-loi-loc-nuoc", url: "Dang-ky-thay-loi-loc-nuoc", defaults: new { controller = "Register", action = "Index" });
            routes.MapRoute(name: "Dai-ly-cua-VINCOM", url: "Dai-ly-cua-VINCOM", defaults: new { controller = "AgencyDisplay", action = "AgencyList" });

            routes.MapRoute(name: "Admin", url: "Admin", defaults: new { controller = "Login", action = "LoginIndex" });
            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}", defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
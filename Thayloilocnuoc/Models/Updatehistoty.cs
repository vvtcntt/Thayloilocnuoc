using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thayloilocnuoc.Models;
namespace Thayloilocnuoc.Models
{
    public class Updatehistoty
    {
        public ThayloilocnuocContext db = new ThayloilocnuocContext();
        public static void UpdateHistory(string task,string FullName,string UserID)
        {

            ThayloilocnuocContext db = new ThayloilocnuocContext();
            tblHistoryLogin tblhistorylogin = new tblHistoryLogin();
            tblhistorylogin.FullName = FullName;
            tblhistorylogin.Task = task;
            tblhistorylogin.idUser = int.Parse(UserID);
            tblhistorylogin.DateCreate = DateTime.Now;
            tblhistorylogin.Active = true;
            
            db.tblHistoryLogins.Add(tblhistorylogin);
            db.SaveChanges();
           
        }
    }
}
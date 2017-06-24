using System;
using System.Collections.Generic;
using System.Data; 
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.Entity.Validation;
using Thayloilocnuoc.Models;
using System.IO;
namespace Thayloilocnuoc.Controllers.Admin.Product
{
    public class InportExcelController : Controller
    {   private ThayloilocnuocContext db = new ThayloilocnuocContext();
        //
        // GET: /InportExcel/

        public ActionResult Index()
    {
        if ((Request.Cookies["Username"] == null))
        {
            return RedirectToAction("LoginIndex", "Login");
        }
            return View();
        }
        public PartialViewResult PartialEXCEL()
        {
            return PartialView();
            
        }
        #region// Check int, float, datetime
        bool CheckDateTime(string String)
        {
            DateTime Date;
            if (DateTime.TryParse(String, out Date))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool CheckInt(string String)
        {
            int Int;
            if (Int32.TryParse(String, out Int))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool CheckFloat(string String)
        {
            float Float;
            if (float.TryParse(String, out Float))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool CheckActive(string String)
        {
            bool Active;
            if (bool.TryParse(String, out Active) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OrderMultipleUp(HttpPostedFileBase fileExcel, FormCollection conllection)
        {

            if (fileExcel != null)
            {
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/UploadExcel"), System.IO.Path.GetFileName(fileExcel.FileName));
                fileExcel.SaveAs(path);
                //Declare variables to hold refernces to Excel objects.
                Workbook workBook;
                SharedStringTable sharedStrings;
                IEnumerable<Sheet> workSheets;
                WorksheetPart productSheet;

                //Declare helper variables.
                string keywordID;

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, true))
                {
                    //References to the workbook and Shared String Table.
                    workBook = document.WorkbookPart.Workbook;
                    workSheets = workBook.Descendants<Sheet>();
                    sharedStrings = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                    //Reference to Excel Worksheet with Product data.
                    keywordID = workSheets.First().Id;
                    productSheet = (WorksheetPart)document.WorkbookPart.GetPartById(keywordID);

                    //Load product data to business object.
                    SaveKeyword(productSheet.Worksheet, sharedStrings);

                    document.Close();

                    System.IO.File.Delete(path);


                }

            } return Redirect("/Product/Index");
        }
        private void SaveKeyword(Worksheet worksheet, SharedStringTable sharedString)
        {
            try
            {
                //Initialize the product list.
                List<tblProduct> result = new List<tblProduct>();

                //LINQ query to skip first row with column names.
                IEnumerable<Row> dataRows = from row in worksheet.Descendants<Row>()
                                            where row.RowIndex > 1
                                            select row;


                List<tblProduct> keyworddb = db.tblProducts.ToList();

                var Messegebox = "";
                foreach (Row row in dataRows)
                {
                    IEnumerable<String> textValues = from cell in row.Descendants<Cell>()
                                                     where cell.CellValue != null
                                                     select (cell.DataType != null && cell.DataType.HasValue && cell.DataType == CellValues.SharedString ? sharedString.ChildElements[int.Parse(cell.CellValue.InnerText)].InnerText : cell.CellValue.InnerText);


                    //Check to verify the row contained data.

                    if (textValues.Count() > 0)
                    {
                        //Create a product and add it to the list.

                        var textArray = textValues.ToArray();
                        tblProduct sp = new tblProduct();

                        int leng = textArray.Length;

                        string Codes = textArray[0];


                        var kt = db.tblProducts.Where(p => p.Code == Codes).ToList();
                        if (kt.Count() == 0)
                        {
                            int nRow = int.Parse(row.RowIndex.ToString()) - 1;
                            if (textArray[0] != null && textArray[0] != "0")
                            {
                                string Code = textArray[0];
                                sp.Code = Code.Trim(' ');
                            }
                            else
                            {
                                Messegebox = Messegebox + " Hàng " + nRow + " cột 1 Name bị lỗi dữ liệu  ";
                            }
                            string nName = textArray[1];
                            if (nName != null && nName != "0" && nName != " ")
                            {
                                string Name = nName;
                                sp.Name = Name.Trim(' ');

                            }
                            else
                            {
                                Messegebox = Messegebox + "Hàng " + nRow + " cột 2 Price bị lỗi dữ liệu ";
                            }
                            string nPrice = textArray[2];
                            if (nPrice != "1" && nPrice != "0" && nPrice != " ")
                            {
                                string Price = nPrice;
                                sp.Price = int.Parse(Price.Trim(' '));

                            }
                            else
                            {
                                Messegebox = Messegebox + "Hàng " + nRow + " cột 2 Price bị lỗi dữ liệu ";
                            }
                            string nPriceSale = textArray[3];
                            if (nPriceSale != "1" && nPriceSale != "0" && nPriceSale != " ")
                            {
                                string PriceSale = nPriceSale;
                                sp.PriceSale = int.Parse(PriceSale.Trim(' '));

                            }
                            else
                            {
                                Messegebox = Messegebox + "Hàng " + nRow + " cột 3 PriceSale bị lỗi dữ liệu ";
                            }
                            ///

                            sp.idCate = 0;
                            db.tblProducts.Add(sp);
                          db.SaveChanges();

                        }

                        
                    }
                    #region[Updatehistory]
                    Updatehistoty.UpdateHistory("Inport Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                    #endregion
                } Session["Thongbao"] = "<script LANGUAGE = \"Javascript\">$(document).ready(function(){ alert('" + Messegebox + "') });</script>";
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }



        }
    }
}

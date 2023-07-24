//using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms;
using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS
{
    public class ChequeOperations
    {
        public string CreateLog(ChequeParameters paramz)
        {
            //try
            //{
            //    tblChequeLog chequeLog = new tblChequeLog();
            //    chequeLog.PayAc = paramz.PayAc;
            //    chequeLog.PayeeAc = paramz.PayeeAc;
            //    chequeLog.Payee = paramz.Payee;
            //    chequeLog.AcPayeeOnly = paramz.AcPayeeOnly;
            //    chequeLog.Amount = paramz.Amount;
            //    chequeLog.ChequeDate = paramz.ChequeDate;
            //    chequeLog.CreateBy = paramz.CreateBy;
            //    chequeLog.CreateDate = paramz.CreateDate;

            //    voucherDetailBL voucherBL = new voucherDetailBL();;
            //    voucherBL.ChequeLog((RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"], chequeLog);
            //    return "ok";
            //}
            //catch(Exception ex)
            //{
            //    return ex.Message;
            //}

            return "OK";
        }
        public string Print(ChequeParameters chequeParamz)
        {
            try
            {
                // Variables
                Warning[] warnings = null;
                String[] streamids = null;
                string mimeType = null;
                string encoding = null;
                string FileName = chequeParamz.ChequeNo;
                String extension = "pdf";

                //The DeviceInfo settings should be changed based on the reportType
                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + extension + "</OutputFormat>" +
                //"  <PageWidth>6.7in</PageWidth>" +
                //"  <PageHeight>3.05in</PageHeight>" +
                //"  <MarginTop>0.0in</MarginTop>" +
                //"  <MarginLeft>0.0in</MarginLeft>" +
                //"  <MarginRight>0.0in</MarginRight>" +
                //"  <MarginBottom>0.0in</MarginBottom>" +
                "</DeviceInfo>";


                ReportViewer viewer = new ReportViewer();
                ////Data source
                //List<Sp_Cheque_DetailResult> data = new List<Sp_Cheque_DetailResult>();
               // data = new ChequePrintBL().ChequeDetail((RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);

                ////Report path
                string bankName = new GlCodeBL().GetByID(chequeParamz.PayAc, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]).gl_dsc;

                if (bankName.ToLower().Contains("allied bank") || bankName.ToLower().Contains("abl") || bankName.ToLower().Contains("allied"))
                {
                    viewer.LocalReport.ReportPath = "glsetup/rdlc/rptChequeABL.rdlc";
                }
                else if (bankName.ToLower().Contains("muslim commercial") || bankName.ToLower().Contains("mcb"))
                {
                    viewer.LocalReport.ReportPath = "glsetup/rdlc/rptChequeMCB.rdlc";
                }
                else if (bankName.ToLower().Contains("askari bank") || bankName.ToLower().Contains("askari"))
                {
                    viewer.LocalReport.ReportPath = "glsetup/rdlc/rptChequeAB.rdlc";
                }
                else if (bankName.ToLower().Contains("bank of punjab") || bankName.ToLower().Contains("bop") || bankName.ToLower().Contains("punjab"))
                {
                    viewer.LocalReport.ReportPath = "glsetup/rdlc/rptChequeBOP.rdlc";
                }
                else if (bankName.ToLower().Contains("nib bank") || bankName.ToLower().Contains("nib"))
                {
                    viewer.LocalReport.ReportPath = "glsetup/rdlc/rptChequeNIB.rdlc";

                }
                else if (bankName.ToLower().Contains("habib metropolitan") || bankName.ToLower().Contains("metropolitan") || bankName.ToLower().Contains("metro"))
                {
                    viewer.LocalReport.ReportPath = "glsetup/rdlc/rptChequeHMB.rdlc";
                }
                else if (bankName.ToLower().Contains("summit"))
                {
                    viewer.LocalReport.ReportPath = "glsetup/rdlc/rptChequeSB.rdlc";
                }
                else
                {
                    return "No cheque format found.";
                }

                ////Report Datasource
                ReportDataSource datasource = new ReportDataSource("DataSet1"," " /*data*/);
               
                ////Report parameters
                ReportParameter[] paramz = new ReportParameter[13];
                string payee = chequeParamz.Payee;
                decimal amount = chequeParamz.Amount;
                string amnut = Convert.ToString(amount);
                var dateAndTime = DateTime.Now;
                
                string payeeOnly = "False";
                if (chequeParamz.AcPayeeOnly == true)
                {
                    payeeOnly = "True";
                }
                paramz[0] = new ReportParameter("Payee", payee);

                paramz[1] = new ReportParameter("d1", chequeParamz.d1);
                paramz[2] = new ReportParameter("amnut", amnut);
                paramz[3] = new ReportParameter("amountWords1", chequeParamz.AmountWords1);
                paramz[4] = new ReportParameter("amountWords2", chequeParamz.AmountWords2);
                paramz[5] = new ReportParameter("payeeOnly", payeeOnly);
                paramz[6] = new ReportParameter("d2", chequeParamz.d2);
                paramz[7] = new ReportParameter("d3", chequeParamz.m1);
                paramz[8] = new ReportParameter("d4", chequeParamz.m2);
                paramz[9] = new ReportParameter("d5", chequeParamz.y1);
                paramz[10] = new ReportParameter("d6", chequeParamz.y2);
                paramz[11] = new ReportParameter("d7", chequeParamz.y3);
                paramz[12] = new ReportParameter("d8", chequeParamz.y4);
  
                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(paramz);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);

                //new ReportPrintDocument(viewer.LocalReport).Print();

                Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

                //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = mimeType;
                HttpContext.Current.Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
                HttpContext.Current.Response.BinaryWrite(bytes);
                //// create the file
                //// send it to the client to download
                HttpContext.Current.Response.Flush();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
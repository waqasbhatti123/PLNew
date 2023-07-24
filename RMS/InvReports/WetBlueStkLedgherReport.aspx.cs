using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using RMS.BL;

namespace RMS.GLSetup
{

    public partial class WetBlueStkLedgherReport : BasePage
    {

        #region DataMembers
        LedgerCardBL cty = new LedgerCardBL();

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"]= value;}
        }

        #endregion
        #region event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtFrom.Text = new DateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year, Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month, 1).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                txtTo.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtFromDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    this.txtToDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    this.txtFromDate.Format = Session["DateFullYearFormat"].ToString();
                    this.txtToDate.Format = Session["DateFullYearFormat"].ToString();

                }

                //if(Request.QueryString[""]
                PID = Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 455)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "StkLedger").ToString();
                }

                rdoWetBlue.Checked = true;
            }
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            if (rdoWetBlue.Checked==true)
            {
                CreatePDFforWetBlue("pdf", txtFrom.Text, txtTo.Text);
            }
            else if (rdoCrust.Checked == true)
            {
                CreatePDFforCrust("pdf", txtFrom.Text, txtTo.Text);
            }
            else if (rdoFinish.Checked == true)
            {
                CreatePDFforFinishGoods("pdf", txtFrom.Text, txtTo.Text);
            }
         

        }
        #endregion
        #region helpingmethod
        protected void CreatePDFforWetBlue(String extension, string dtStart, string dtTo)
        {
           
            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(dtStart, out dtFrm);
            DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(dtTo, out dt2);
            FIN_PERD fnObject = cty.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            if (dtFrm >= fnObject.Start_Date && dt2 <= fnObject.End_Date)
            { }
            else
            {
                dtFrm = Convert.ToDateTime(fnObject.Start_Date);
                dt2 = Convert.ToDateTime(fnObject.End_Date);
            }
               string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + extension + "</OutputFormat>" +
                "  <PageWidth>11.69in</PageWidth>" +
                "  <PageHeight>8.27in</PageHeight>" +
                "  <MarginTop>0.2in</MarginTop>" +
                "  <MarginLeft>0.2in</MarginLeft>" +
                "  <MarginRight>0.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";
           
            ReportViewer viewer = new ReportViewer();
            List<sp_StkLedgerResult> sal = cty.GetWetBlueStkLedgerBal(dtFrm, dt2,2, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            if (PID == 455)
            {
                viewer.LocalReport.ReportPath = "InvReports/rdlc/WetBlueStkLedger.rdlc";
            }
            //if(PID==334)
            //{
            //    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptTB.rdlc";
            //}
            ReportDataSource datasource = new ReportDataSource("sp_StkLedgerResult", sal);
            ReportParameter[] paramz = new ReportParameter[3];
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[1] = new ReportParameter("dateRange","Period From:  " + dtFrm.ToString("dd-MMM-yyyy") + "  To  " + dt2.ToString("dd-MMM-yyyy"));
            paramz[2] = new ReportParameter("ReportName", "Wet Blue Stock Movement Ledger");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
            viewer.LocalReport.SetParameters(paramz);
            ReportViewer1 = viewer;
            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename=" +"WetBlueStkMovementReport" + "_" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) + ".") + extension);
            Response.BinaryWrite(bytes);
            //// create the file
            //// send it to the client to download
            Response.Flush();
        }


        protected void CreatePDFforCrust(String extension, string dtStart, string dtTo)
        {

            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(dtStart, out dtFrm);
            DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(dtTo, out dt2);
            FIN_PERD fnObject = cty.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            if (dtFrm >= fnObject.Start_Date && dt2 <= fnObject.End_Date)
            { }
            else
            {
                dtFrm = Convert.ToDateTime(fnObject.Start_Date);
                dt2 = Convert.ToDateTime(fnObject.End_Date);
            }
            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + extension + "</OutputFormat>" +
                "  <PageWidth>11.69in</PageWidth>" +
                "  <PageHeight>8.27in</PageHeight>" +
                "  <MarginTop>0.2in</MarginTop>" +
                "  <MarginLeft>0.2in</MarginLeft>" +
                "  <MarginRight>0.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            ReportViewer viewer = new ReportViewer();
            List<sp_StkLedgerResult> sal = cty.GetWetBlueStkLedgerBal(dtFrm, dt2, 3, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            if (PID == 455)
            {
                viewer.LocalReport.ReportPath = "InvReports/rdlc/CrustStkLedger.rdlc";
            }
            ReportDataSource datasource = new ReportDataSource("sp_StkLedgerResult", sal);
            ReportParameter[] paramz = new ReportParameter[3];
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[1] = new ReportParameter("dateRange", "Period From:  " + dtFrm.ToString("dd-MMM-yyyy") + "  To  " + dt2.ToString("dd-MMM-yyyy"));
            paramz[2] = new ReportParameter("ReportName", "Crust Stock Movement Ledger");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
            viewer.LocalReport.SetParameters(paramz);
            ReportViewer1 = viewer;
            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename=" + "CrustStkMovementReport" + "_" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) + ".") + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
        }


        protected void CreatePDFforFinishGoods(String extension, string dtStart, string dtTo)
        {

            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(dtStart, out dtFrm);
            DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(dtTo, out dt2);
            FIN_PERD fnObject = cty.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            if (dtFrm >= fnObject.Start_Date && dt2 <= fnObject.End_Date)
            { }
            else
            {
                dtFrm = Convert.ToDateTime(fnObject.Start_Date);
                dt2 = Convert.ToDateTime(fnObject.End_Date);
            }
            string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + extension + "</OutputFormat>" +
                "  <PageWidth>11.69in</PageWidth>" +
                "  <PageHeight>8.27in</PageHeight>" +
                "  <MarginTop>0.2in</MarginTop>" +
                "  <MarginLeft>0.2in</MarginLeft>" +
                "  <MarginRight>0.2in</MarginRight>" +
                "  <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            ReportViewer viewer = new ReportViewer();
            List<sp_StkLedgerResult> sal = cty.GetWetBlueStkLedgerBal(dtFrm, dt2, 4, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            if (PID == 455)
            {
                viewer.LocalReport.ReportPath = "InvReports/rdlc/FinishGoodsStkLedger.rdlc";
            }
            ReportDataSource datasource = new ReportDataSource("sp_StkLedgerResult", sal);
            ReportParameter[] paramz = new ReportParameter[3];
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[1] = new ReportParameter("dateRange", "Period From:  " + dtFrm.ToString("dd-MMM-yyyy") + "  To  " + dt2.ToString("dd-MMM-yyyy"));
            paramz[2] = new ReportParameter("ReportName", "Finish Goods Stock Movement Ledger");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
            viewer.LocalReport.SetParameters(paramz);
            ReportViewer1 = viewer;
            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename=" + "FinishGoodsStkMovementReport" + "_" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) + ".") + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
        }

        #endregion
    }
}

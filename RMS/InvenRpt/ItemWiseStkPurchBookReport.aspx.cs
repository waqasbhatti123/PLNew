using System;
using RMS.BL;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using System.Web.Services;
using System.Web;
// 
namespace RMS.InvenRpt
{
    public partial class ItemWiseStkPurchBookReport : BasePage
    {
        #region DataMembers

        InvReports_BL rptBl = new InvReports_BL();

        public DateTime fromDate
        {
            get { return Convert.ToDateTime(ViewState["fromDate"]); }
            set { ViewState["fromDate"] = value; }
        }
        public DateTime toDate
        {
            get { return Convert.ToDateTime(ViewState["toDate"]); }
            set { ViewState["toDate"] = value; }
        }

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
        public int BrId
        {
            get { return Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }
        //public int GroupID
        //{
        //    get { return (ViewState["GroupID"] == null) ? 0 : Convert.ToInt32(ViewState["GroupID"]); }
        //    set { ViewState["GroupID"] = value; }
        //}


        #endregion

        #region Event
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"]);
            }
            if (!IsPostBack)
            {

                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                }
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender2.Format = Session["DateFormat"].ToString();
                }
                CalendarExtender1.SelectedDate = Convert.ToDateTime((Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year - 1).ToString() + "-07" + "-01").Date;
                CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                

                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 525 || PID == 882)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ItemWiseStockPurchRpt").ToString();
                    txtfromDt.Text = Convert.ToDateTime((Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year - 1).ToString() + "-07" + "-01").ToString();
                    txttoDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

                    FillDDLYear();
                    //rdbYear.Checked = true;
                    //ddlMonth.Enabled = false;

                    FillDropDownLoc();
                    FillDdlItemGroup();
                }
            }
        }
        
        
        //protected void rdbYear_Changed(object sender, EventArgs e)
        //{
        //    ddlMonth.Enabled = false;
        //    lblYear.Text = "Fin Year :";
        //}

        //protected void rdbMonth_Changed(object sender, EventArgs e)
        //{
        //    ddlMonth.Enabled = true;
        //    ddlYear.Enabled = true;
        //    lblYear.Text = "Year :";
        //}

        //protected void rdbMonthWiseYear_Changed(object sender, EventArgs e)
        //{
        //    ddlMonth.Enabled = false;
        //    lblYear.Text = "Fin Year :";
        //}

        //protected void rdbMonthly_Changed(object sender, EventArgs e)
        //{
        //    ddlMonth.Enabled = true;
        //    ddlYear.Enabled = true;
        //    lblYear.Text = "Year :";
        //}

        

        protected void btnSearch_Click(object sender, EventArgs e)
        {
             GetMonthlyReportDetail();
        }
       
        #endregion

        #region Helping Method

        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = rptBl.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
        }
        public void FillDdlItemGroup()
        {
            ddlItemGroup.DataValueField = "itm_cd";
            ddlItemGroup.DataTextField = "itm_dsc";
            ddlItemGroup.DataSource = rptBl.GetItemGroup(BrId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItemGroup.DataBind();
        }

        public void GetMonthlyReportDetail()
        {
            string month = "";
            string monthP = "";
            string year = "";
            string rptBy = "";

            DateTime fromDt = Convert.ToDateTime(txtfromDt.Text);
            DateTime toDt = Convert.ToDateTime(txttoDt.Text);
            year = ddlYear.SelectedItem.Text;
            month = ddlMonth.SelectedValue;
            monthP = "For " + ddlMonth.SelectedValue;
            rptBy = "Item-Wise Stock Purchase Report";

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/StockPurchaseReport.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
            string tp = " ";
            List<sp_Inv_ItemWise_Purch_RptResult> PurchBook = rptBl.GetStkPurchReport(BrId, ddlItemGroup.SelectedValue, Convert.ToDecimal(ddlLoc.SelectedValue), tp, txtcodefrom.Text, txtcodeto.Text, fromDt, toDt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ReportDataSource dataSource = new ReportDataSource("sp_Inv_ItemWise_Purch_RptResult", PurchBook);

            ReportParameter[] rpt = new ReportParameter[4];
            rpt[0] = new ReportParameter("ReportName", "Item-Wise Stock Purchase Report");
            rpt[1] = new ReportParameter("Month", monthP);
            rpt[2] = new ReportParameter("Year", year);
            rpt[3] = new ReportParameter("rptBy", rptBy);

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);
            GetReport(reportViewer);
        }

        //public void GetMonthWiseYearlyReport()
        //{
        //    string year = "";
        //    string prmYear = "";
        //    int yr = 0;
        //    string rptBy = "";
        //    year = ddlYear.SelectedItem.Text;
        //    yr = Convert.ToInt32(year) - 1;

        //    prmYear = "For the Period : July " + yr.ToString() + " - June " + year;
        //    rptBy = "Party-Wise Report By Month";

        //    ReportViewer reportViewer = new ReportViewer();

        //    reportViewer.LocalReport.ReportPath = "InvReports/rdlc/RawHidePurchBookMonthYearRpt.rdlc";
        //    reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //    reportViewer.LocalReport.Refresh();
        //    reportViewer.LocalReport.EnableExternalImages = true;
        //    reportViewer.LocalReport.Refresh();

        //    List<spRawHidePurchByYearResult> PurchBook = rptBl.GetRawHidesPurchBookByYear(year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    ReportDataSource dataSource = new ReportDataSource("spRawHidePurchByYearResult", PurchBook);


        //    ReportParameter[] rpt = new ReportParameter[3];
        //    rpt[0] = new ReportParameter("ReportName", "Raw Hide Purchase Book");
        //    rpt[1] = new ReportParameter("Year", prmYear);
        //    rpt[2] = new ReportParameter("rptBy", rptBy);

        //    reportViewer.LocalReport.SetParameters(rpt);
        //    reportViewer.LocalReport.DataSources.Clear();

        //    reportViewer.LocalReport.DataSources.Add(dataSource);
        //    GetReport(reportViewer);
        //}
//--------------------
        //public void GetMonthlyReport()
        //{
        //    string year = "";
        //    string prmYear = "";
        //    string month = ddlMonth.SelectedItem.Text;
        //    int yr = 0;
        //    string rptBy = "";
        //    year = ddlYear.SelectedItem.Text;
            

        //    prmYear = "For " + month+" " + year;
        //    rptBy = "Party-Wise Report By Month";

        //    ReportViewer reportViewer = new ReportViewer();

        //    reportViewer.LocalReport.ReportPath = "InvReports/rdlc/RawHidePurchBookMonthRpt.rdlc";
        //    reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //    reportViewer.LocalReport.Refresh();
        //    reportViewer.LocalReport.EnableExternalImages = true;
        //    reportViewer.LocalReport.Refresh();

        //    List<spRawHidePurchByMonthlyResult> PurchBook = rptBl.GetRawHidesPurchBookByMonthly(year, month, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    ReportDataSource dataSource = new ReportDataSource("spRawHidePurchByMonthlyResult", PurchBook);


        //    ReportParameter[] rpt = new ReportParameter[3];
        //    rpt[0] = new ReportParameter("ReportName", "Raw Hide Purchase Book");
        //    rpt[1] = new ReportParameter("Year", prmYear);
        //    rpt[2] = new ReportParameter("rptBy", rptBy);

        //    reportViewer.LocalReport.SetParameters(rpt);
        //    reportViewer.LocalReport.DataSources.Clear();

        //    reportViewer.LocalReport.DataSources.Add(dataSource);
        //    GetReport(reportViewer);
        //}


        //public void GetYearlyReport()
        //{
        //    string year = "";
        //    string prmYear = "";
        //    int yr = 0;
        //    string rptBy = "";
        //    year = ddlYear.SelectedItem.Text;
        //    yr = Convert.ToInt32(year) - 1;

        //    prmYear = "For the Period : July " + yr.ToString() + " - June " + year;
        //    rptBy = "Party-Wise Report";

        //    ReportViewer reportViewer = new ReportViewer();

        //    reportViewer.LocalReport.ReportPath = "InvReports/rdlc/RawHidePurchBookYearRpt.rdlc";
        //    reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //    reportViewer.LocalReport.Refresh();
        //    reportViewer.LocalReport.EnableExternalImages = true;
        //    reportViewer.LocalReport.Refresh();

        //    List<spRawHidePurchByYearResult> PurchBook = rptBl.GetRawHidesPurchBookByYear(year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    ReportDataSource dataSource = new ReportDataSource("spRawHidePurchByYearResult", PurchBook);


        //    ReportParameter[] rpt = new ReportParameter[3];
        //    rpt[0] = new ReportParameter("ReportName", "Raw Hide Purchase Book");
        //    rpt[1] = new ReportParameter("Year", prmYear);
        //    rpt[2] = new ReportParameter("rptBy", rptBy);

        //    reportViewer.LocalReport.SetParameters(rpt);
        //    reportViewer.LocalReport.DataSources.Clear();

        //    reportViewer.LocalReport.DataSources.Add(dataSource);
        //    GetReport(reportViewer);
        //}

       
        public void GetReport(ReportViewer reportViewer)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;

            string ext = "pdf", type = "PDF";
            if (ddlExtension.SelectedValue == "Excel")
            {
                ext = "xls";
                type = "Excel";
            }

            byte[] bytes = reportViewer.LocalReport.Render(
               type, null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            filename = string.Format("{0}.{1}", "StockPurchaseReport", ext);
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        public void FillDDLYear()
        {
            int year = 2000;
            List<string> str = new List<string>();
            for (int i = 0; i < 21; i++)
            {
                str.Add(year.ToString());
                year++;

            }
            
            ddlYear.DataSource = str;
            ddlYear.DataBind();
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<string> GetDetailAccount(string sname)
        {
            InvReports_BL cty = new InvReports_BL();
            return cty.GetDetailAccount(sname, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);

        }

        #endregion
    }
}
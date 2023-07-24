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
    public partial class PartyWiseStkPurchBookReport : BasePage
    {
        #region DataMembers

        InvReports_BL rptBl = new InvReports_BL();
        ReportViewer reportViewer = new ReportViewer();

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
        public int BrId
        {
            get { return Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }
        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
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
            reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(setSubDataSourceContInfo);
            reportViewer.LocalReport.Refresh();

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
                if (PID == 524 || PID == 881)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "RawHidePurchBookParty").ToString();

                    txtfromDt.Text = Convert.ToDateTime((Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year - 1).ToString() + "-07" + "-01").ToString();
                    txttoDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

                    FillDDLYear();
                   // rdbYear.Checked = true;
                    //ddlMonth.Enabled = false;
                    FillDropDownLoc();
                    FillDdlItemGroup();
                    FillDropDownParty();
                }
            }
        }

        /*^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*/
        public void setSubDataSourceContInfo(object sender, SubreportProcessingEventArgs e)
        {
            //ReportParameterInfoCollection prm = e.Parameters;

            int vrId = Convert.ToInt32(e.Parameters[0].Values[0]);

            List<spInv_PurchSubRptResult> InvSubReport = rptBl.getInventoryPurchSubRpt(vrId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            e.DataSources.Add(new ReportDataSource("spInv_PurchSubRptResult", InvSubReport));
        }
        //---------------------------------------------------
        public void FillDropDownParty()
        {
            ddlParty.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlParty.DataTextField = "gl_dsc";
            ddlParty.DataValueField = "gl_cd";
            ddlParty.DataBind();
        }

        
        protected void rdbYear_Changed(object sender, EventArgs e)
        {
            ddlMonth.Enabled = false;
            lblYear.Text = "Fin Year :";
        }

        protected void rdbMonth_Changed(object sender, EventArgs e)
        {
            ddlMonth.Enabled = true;
            ddlYear.Enabled = true;
            lblYear.Text = "Year :";
        }

        protected void rdbMonthWiseYear_Changed(object sender, EventArgs e)
        {
            ddlMonth.Enabled = false;
            lblYear.Text = "Fin Year :";
        }

        protected void rdbMonthly_Changed(object sender, EventArgs e)
        {
            ddlMonth.Enabled = true;
            ddlYear.Enabled = true;
            lblYear.Text = "Year :";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
           // GetMonthlyReportDetail();
           getInventoryPurchRpt();
                  
        }
       
        #endregion

        #region Helping Method

        //public void GetMonthlyReportDetail()
        //{
        //    string month = "";
        //    string monthP = "";
        //    string year = "";
        //    string rptBy = "";
        //    year = ddlYear.SelectedItem.Text;
        //    month = ddlMonth.SelectedValue;
        //    monthP = "For " + ddlMonth.SelectedValue;
        //    rptBy = "Party-Wise Stock Purchase Report";

            
        //    reportViewer.Visible = false;
        //    reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/PartyWiseStockPurchaseReport.rdlc";
        //    reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //    reportViewer.LocalReport.Refresh();
        //    reportViewer.LocalReport.EnableExternalImages = true;
        //    reportViewer.LocalReport.Refresh();
        //    char tp = ' ';
        //    List<spStkPurchRptResult> PurchBook = rptBl.GetStkPurchReport(year, month, tp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    ReportDataSource dataSource = new ReportDataSource("spStkPurchRptResult", PurchBook);

        //    ReportParameter[] rpt = new ReportParameter[4];
        //    rpt[0] = new ReportParameter("ReportName", "Party-Wise Stock Purchase Report");
        //    rpt[1] = new ReportParameter("Month", monthP);
        //    rpt[2] = new ReportParameter("Year", year);
        //    rpt[3] = new ReportParameter("rptBy", rptBy);

        //    reportViewer.LocalReport.SetParameters(rpt);
        //    reportViewer.LocalReport.DataSources.Clear();

        //    reportViewer.LocalReport.DataSources.Add(dataSource);
        //    GetReport(reportViewer);
        //}


        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = rptBl.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
        }

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

            filename = string.Format("{0}.{1}", "InvertoryPurchaseRpt", ext);
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
        //--------------Test InventoryPurchase Report------
        public void getInventoryPurchRpt()
        {
            DateTime fromDt = Convert.ToDateTime(txtfromDt.Text);
            DateTime toDt = Convert.ToDateTime(txttoDt.Text);
            string year = "";
            string month = "";
            // string rptBy = "";
            year = ddlYear.SelectedItem.Text;
            month = ddlMonth.SelectedValue;

            //rptBy = "Comparative Stock Status Report";

            //ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/InventoryPurchaseRpt.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
#pragma warning disable CS0219 // The variable 'tp' is assigned but its value is never used
            char tp = ' ';
#pragma warning restore CS0219 // The variable 'tp' is assigned but its value is never used
            List<sp_Inv_Purch_RptResult> InvPurch = rptBl.getInventoryPurchRpt(ddlParty.SelectedValue, BrId, ddlItemGroup.SelectedValue, Convert.ToDecimal(ddlLoc.SelectedValue), txtcodefrom.Text, txtcodeto.Text, fromDt, toDt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ReportDataSource dataSource = new ReportDataSource("sp_Inv_Purch_RptResult", InvPurch);

            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Party-Wise Purchase Book");
            rpt[1] = new ReportParameter("Year", year);
            rpt[2] = new ReportParameter("Month", month);
            //rpt[3] = new ReportParameter("rptBy", rptBy);

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);
            GetReport(reportViewer);
        }

        //-----------------------------------
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
        public void FillDdlItemGroup()
        {
            ddlItemGroup.DataValueField = "itm_cd";
            ddlItemGroup.DataTextField = "itm_dsc";
            ddlItemGroup.DataSource = rptBl.GetItemGroup(BrId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItemGroup.DataBind();
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
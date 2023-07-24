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
// 
namespace RMS
{
    public partial class BrokerWiseRawHidePurchBookReport : BasePage
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
        //public int GroupID
        //{
        //    get { return (ViewState["GroupID"] == null) ? 0 : Convert.ToInt32(ViewState["GroupID"]); }
        //    set { ViewState["GroupID"] = value; }
        //}


        #endregion

        #region Event
    
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 458)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "RawHidePurchBookBroker").ToString();
                    FillDDLYear();
                    rdbVendor.Checked = true;
                    ddlMonth.Enabled = false;
                    
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

        protected void rdbVendor_Changed(object sender, EventArgs e)
        {
            ddlYear.Enabled = true;
            ddlMonth.Enabled = false;
            lblYear.Text = "Fin Year :";
        }

        protected void rdbVendorYr_Changed(object sender, EventArgs e)
        {
            ddlYear.Enabled = true;
            ddlMonth.Enabled = false;
            lblYear.Text = "Fin Year :";
        }

        protected void rdbVendorMonth_Changed(object sender, EventArgs e)
        {
            ddlYear.Enabled = true;
            ddlMonth.Enabled = true;
            lblYear.Text = "Year :";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlYear.SelectedValue != "0")
            {


                if (rdbVendor.Checked == true)
                {
                    GetVendorWiseReport();


                }
                else if (rdbVendorYr.Checked == true)
                {
                    GetVendorYearWiseReport();

                }
                //--------------------
                else if (rdbVendorMonth.Checked == true)
                {
                    if (ddlMonth.SelectedValue != "0")
                    {

                        GetVendorMonthlyReport();
                    }
                    else
                    {
                        ucMessage.ShowMessage("Please select month...", RMS.BL.Enums.MessageType.Error);
                        ddlMonth.Focus();
                    }

                }
                //--------------
                else
                {
                }
            }// end of outer if
            else
            {
                ucMessage.ShowMessage("Please Select Year...", RMS.BL.Enums.MessageType.Error);
                ddlYear.Focus();
            }
        }
       
        #endregion

        #region Helping Method

        //public void GetMonthlyReport()
        //{
        //    string month = "";
        //    string monthP = "";
        //    string year = "";
        //    string rptBy = "";
        //    year = ddlYear.SelectedItem.Text;
        //    month = ddlMonth.SelectedValue;
        //    monthP = "For "+ddlMonth.SelectedValue;
        //    rptBy = "Party-Wise Report";

        //    ReportViewer reportViewer = new ReportViewer();
        //    reportViewer.Visible = false;
        //    reportViewer.LocalReport.ReportPath = "InvReports/rdlc/RawHidePurchBookRpt.rdlc";
        //    reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //    reportViewer.LocalReport.Refresh();
        //    reportViewer.LocalReport.EnableExternalImages = true;
        //    reportViewer.LocalReport.Refresh();
        //    List<spRawHidePurchByMonthResult> PurchBook = rptBl.GetRawHidesPurchBook(year, month, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    ReportDataSource dataSource = new ReportDataSource("spRawHidePurchByMonthResult", PurchBook);

        //    ReportParameter[] rpt = new ReportParameter[4];
        //    rpt[0] = new ReportParameter("ReportName", "Raw Hide Purchase Book");
        //    rpt[1] = new ReportParameter("Month", monthP);
        //    rpt[2] = new ReportParameter("Year", year);
        //    rpt[3] = new ReportParameter("rptBy", rptBy);

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

        public void GetVendorWiseReport()
        {
            string year = "";
            string prmYear = "";
            string rptBy = "";
            int yr = 0;
            year = ddlYear.SelectedItem.Text;
            yr = Convert.ToInt32(year) - 1;

            prmYear = "For the Period : July " + yr.ToString() + " - June " + year;
            rptBy = "Broker-Wise Report";

            ReportViewer reportViewer = new ReportViewer();

            reportViewer.LocalReport.ReportPath = "InvReports/rdlc/RawHidePurchBookYrVendor.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();

            List<spRawHidePurchByYrVendorResult> PurchBook = rptBl.GetRawHidesPurchBookByYrVendor(year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ReportDataSource dataSource = new ReportDataSource("spRawHidePurchByYrVendorResult", PurchBook);


            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Raw Hide Purchase Book");
            rpt[1] = new ReportParameter("Year", prmYear);
            rpt[2] = new ReportParameter("rptBy",rptBy);

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);
            GetReport(reportViewer);
        }

        public void GetVendorYearWiseReport()
        {
            string year = "";
            string prmYear = "";
            int yr = 0;
            string rptBy = "";
            year = ddlYear.SelectedItem.Text;
            yr = Convert.ToInt32(year) - 1;

            prmYear = "For the Period : July " + yr.ToString() + " - June " + year;
            rptBy = "Broker-Wise Report by month";

            ReportViewer reportViewer = new ReportViewer();
          
            reportViewer.LocalReport.ReportPath = "InvReports/rdlc/RawHidePurchBookVendorWholeYear.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();

            List<spRawHidePurchByVendorWholeYearResult> PurchBook = rptBl.GetRawHidesPurchBookByVendorWholeYear(year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ReportDataSource dataSource = new ReportDataSource("spRawHidePurchByVendorWholeYearResult", PurchBook);


            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Raw Hide Purchase Book");
            rpt[1] = new ReportParameter("Year", prmYear);
            rpt[2] = new ReportParameter("rptBy", rptBy);

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);
            GetReport(reportViewer);
        }

        //------------------------
        public void GetVendorMonthlyReport()
        {
            string year = "";
            string prmYear = "";
            string prmMonth = "";
            string rptBy = "";
            int yr = 0;
            year = ddlYear.SelectedItem.Text;
            yr = Convert.ToInt32(year) ;

            prmMonth = ddlMonth.SelectedItem.Text;

            prmYear = "For "+prmMonth.ToString()+" "+ yr.ToString() ;
            rptBy = "Broker-Wise Report";

            ReportViewer reportViewer = new ReportViewer();

            reportViewer.LocalReport.ReportPath = "InvReports/rdlc/RawHidePurchBookVendorMonth.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();

            List<spRawHidePurchaseByMonthVendorResult> PurchBook = rptBl.GetRawHidesPurchBookByMonthVendor(year,prmMonth, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ReportDataSource dataSource = new ReportDataSource("spRawHidePurchaseByMonthVendorResult", PurchBook);


            ReportParameter[] rpt = new ReportParameter[4];
            rpt[0] = new ReportParameter("ReportName", "Raw Hide Purchase Book");
            rpt[1] = new ReportParameter("Year", prmYear);
            rpt[2] = new ReportParameter("rptBy", rptBy);
            rpt[3] = new ReportParameter("Month", prmMonth);

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);
            GetReport(reportViewer);
        }
//-----------------------------

        public void GetReport(ReportViewer reportViewer)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;


            byte[] bytes = reportViewer.LocalReport.Render(
               "PDF", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            filename = string.Format("{0}.{1}", "RawHidePurchaseBook", "pdf");
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


        #endregion
    }
}
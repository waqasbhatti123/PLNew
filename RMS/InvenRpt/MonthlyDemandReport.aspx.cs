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
namespace RMS.InvenRpt
{
    public partial class MonthlyDemandReport : BasePage
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
                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 529)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "MonthlyDemand").ToString();
                    FillDDLYear();
                    //FillDropDownLoc();

                    rdbDeptWise.Checked = true;
                    
                }
                FillDdlItemGroup();
            }
        }

        
       

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlYear.SelectedValue != "0")
            {

                    if (ddlMonth.SelectedValue != "0")
                    {
                        if (rdbDeptWise.Checked == true)
                        {
                            getDeptWiseMonthlyDemandRpt();
                        }
                        else if (rdbItemWise.Checked == true)
                        {
                            getItemWiseMonthlyDemandRpt();
                        }
                    }
                    else
                    {
                        ucMessage.ShowMessage("Please select month...", RMS.BL.Enums.MessageType.Error);
                        ddlMonth.Focus();
                    }

                }

         
            else
            {
                ucMessage.ShowMessage("Please Select Year...", RMS.BL.Enums.MessageType.Error);
                ddlYear.Focus();
            }
        }
       
        #endregion

        #region Helping Method



        public void FillDdlItemGroup()
        {
            ddlItemGroup.DataValueField = "itm_cd";
            ddlItemGroup.DataTextField = "itm_dsc";
            ddlItemGroup.DataSource = rptBl.GetItemGroup(BrId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItemGroup.DataBind();
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

            filename = string.Format("{0}.{1}", "DeptWiseMonthlyDemandRpt", ext);
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
      
        public void getDeptWiseMonthlyDemandRpt()
        {

            string year = "";
            string month = "";
            // string rptBy = "";
            year = ddlYear.SelectedItem.Text;
            month = ddlMonth.SelectedValue;

            //rptBy = "Comparative Stock Status Report";

            //ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/DeptWiseMonthlyDemand.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
            
            List<spMonthlyDemandRptResult> MonthDemand = rptBl.getMonthlyDemand(ddlItemGroup.SelectedValue, year, month, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ReportDataSource dataSource = new ReportDataSource("spMonthlyDemandRptResult", MonthDemand);

            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Dept-Wise Monthly Demand Report");
            rpt[1] = new ReportParameter("Year", year);
            rpt[2] = new ReportParameter("Month", month);
           

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);
            GetReport(reportViewer);
        }


        //-----------------------------------
        public void getItemWiseMonthlyDemandRpt()
        {

            string year = "";
            string month = "";
            // string rptBy = "";
            year = ddlYear.SelectedItem.Text;
            month = ddlMonth.SelectedValue;

            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/ItemWiseMonthlyDemand.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
            
            List<spMonthlyDemandRptResult> MonthDemand = rptBl.getMonthlyDemand(ddlItemGroup.SelectedValue, year, month, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ReportDataSource dataSource = new ReportDataSource("spMonthlyDemandRptResult", MonthDemand);

            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Item-Wise Monthly Demand Report");
            rpt[1] = new ReportParameter("Year", year);
            rpt[2] = new ReportParameter("Month", month);
            
            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);
            GetReport(reportViewer);
        }
        //-------------------
        
        public void FillDDLYear()
        {
            int year = 2005;
            List<string> str = new List<string>();
            for (int i = 0; i < 16; i++)
            {
                str.Add(year.ToString());
                year++;

            }
            
            ddlYear.DataSource = str;
            ddlYear.DataBind();
        }


        public void FillDropDownLoc()
        {
            //ddlLoc.DataSource = rptBl.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlLoc.DataValueField = "LocId";
            //ddlLoc.DataTextField = "LocName";
            //ddlLoc.DataBind();
        }

        #endregion
    }
}
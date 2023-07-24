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
    public partial class ComparativeStockStatusReport : BasePage
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
                if (PID == 528)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ComparativeStkStatusRpt").ToString();
                    FillDDLYear();
                    //rdbYear.Checked = true;
                    //ddlMonth.Enabled = false;
                    
                }
            }
        }
        
 
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlYear.SelectedValue != "0")
            {
                 getComparativeStockStatusRpt();
                
                
            }

     
            else
            {
                ucMessage.ShowMessage("Please Select Year...", RMS.BL.Enums.MessageType.Error);
                ddlYear.Focus();
            }
        }
       
        #endregion

        #region Helping Method

        public void getComparativeStockStatusRpt()
        {
          
            string year = "";
           // string rptBy = "";
            year = ddlYear.SelectedItem.Text;
           
            
            //rptBy = "Comparative Stock Status Report";

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/ComparativeStockStatusRpt.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
#pragma warning disable CS0219 // The variable 'tp' is assigned but its value is never used
            char tp = ' ';
#pragma warning restore CS0219 // The variable 'tp' is assigned but its value is never used
            // List<spComparativeStkStatusRptResult> ComparativeStk = rptBl.getComparativeStockStatusRpt(year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ReportDataSource dataSource = new ReportDataSource("spComparativeStkStatusRptResult", ""); //ComparativeStk);

            ReportParameter[] rpt = new ReportParameter[2];
            rpt[0] = new ReportParameter("ReportName", "Comparative Stock Status Report");
            rpt[1] = new ReportParameter("Year", year);
            //rpt[3] = new ReportParameter("rptBy", rptBy);

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);
            GetReport(reportViewer);
        }

 
       
       
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

            filename = string.Format("{0}.{1}", "ComparativeStockStatusRpt", "pdf");
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
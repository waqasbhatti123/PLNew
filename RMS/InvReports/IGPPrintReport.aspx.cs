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
    public partial class IGPPrintReport : BasePage
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
                CalendarExtender1.SelectedDate = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01");
                CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                

                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 460)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "IGPReport").ToString();
                    
                    
                    txtfromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01").ToString();
                    txttoDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

                    
                }

            }
        }
    
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime fromDt = Convert.ToDateTime(txtfromDt.Text);
            DateTime toDt = Convert.ToDateTime(txttoDt.Text);
            String status = ddlStatus.SelectedValue;
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvReports/rdlc/IGPPrintRpt.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();



            List<spAllIGPViewResult> IGP = rptBl.GetIGPReport(fromDt, toDt, status, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            ReportDataSource dataSource = new ReportDataSource("spAllIGPViewResult", IGP);

            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Inward Gate Pass Report");
            rpt[1] = new ReportParameter("FromDate", fromDt.ToString());
            rpt[2] = new ReportParameter("ToDate", toDt.ToString());
            //rpt[3] = new ReportParameter("prmCriteria", criteria);
            //rpt[4] = new ReportParameter("rptPath", logopath);
            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);
            

            

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

            filename = string.Format("{0}.{1}", "IGPReport", "pdf");
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();


        }
       
        #endregion

        #region Helping Method

        #endregion
    }
}
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
    public partial class PartyWiseUnpaidReport : BasePage
    {
        #region DataMembers

        InvReports_BL rptBl = new InvReports_BL();

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        #endregion

        #region Event
    
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 535 || PID == 884)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "PWI").ToString();
                    FillDropDownVendor();
                    
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string ddlVal = ddlUnpaid.SelectedValue;
            int BrId = 0;
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

            getUnpaidInvoice(ddlVal,BrId);
             
     
            
        }
       
        #endregion

        #region Helping Method

        public void FillDropDownVendor()
        {
            ddlUnpaid.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlUnpaid.DataTextField = "gl_dsc";
            ddlUnpaid.DataValueField = "gl_cd";
            ddlUnpaid.DataBind();
        }
        public void getUnpaidInvoice(string val, int brid)
        {

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/PartyWiseUnpaidInvoiceRpt.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
            //List<spPartyWiseUnpaidInvoicesResult> recs = rptBl.getUnpaidInvoice(val, brid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ReportDataSource dataSource = new ReportDataSource("spPartyWiseUnpaidInvoicesResult", "");//recs);

            ReportParameter[] rpt = new ReportParameter[1];
            rpt[0] = new ReportParameter("ReportName", Session["PageTitle"].ToString());

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

            filename = string.Format("{0}.{1}", Session["PageTitle"].ToString(), ext);
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        #endregion
    }
}
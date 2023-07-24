using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using RMS.BL;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace RMS.report.rdlc
{
    public partial class SalaryReconReport : System.Web.UI.Page
    {
        BL.PlReportBL rptBL = new RMS.BL.PlReportBL();

        #region Properties
        public int CompId
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

        public int PayPerd
        {
            get { return (ViewState["PayPerd"] == null) ? 0 : Convert.ToInt32(ViewState["PayPerd"]); }
            set { ViewState["PayPerd"] = value; }
        }

        #endregion

        private void FillDropDownPayPeriod()
        {
            BL.SalaryBL SalBl = new RMS.BL.SalaryBL();

            this.ddlPayPerd.DataTextField = "PayPerd";
            ddlPayPerd.DataValueField = "PayPerd";
            ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlPayPerd.DataBind();
        }

        protected void Load_report()
        {
            object sal;
            sal = rptBL.rptSalaryRecon(CompId, PayPerd, 0,"", "", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ReportViewer viewer = new ReportViewer();
            
            viewer.LocalReport.ReportPath = "report/rdlc/rptSalaryRecon.rdlc";
            ReportDataSource datasource = new ReportDataSource("spSalaryReconTORResult", sal);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.Refresh();
            //ReportViewer1 = viewer;
            
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalReconRpt").ToString();

                int iCompid;
                if (Session["CompID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    } 
                    int.TryParse(Request.Cookies["uzr"]["CompID"], out iCompid);
                }
                else
                {
                    int.TryParse(Session["CompID"].ToString(), out iCompid);
                }
                CompId = iCompid;

                FillDropDownPayPeriod();
            }
        }


        protected void CreatePDF(String FileName, String extension)
        {
        // Variables
            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + extension + "</OutputFormat>" +
            "  <PageWidth>15in</PageWidth>" +
            "  <PageHeight>8.27in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

        // Setup the report viewer object and get the array of bytes
            ReportViewer viewer =new ReportViewer();
            object sal;
            sal = rptBL.rptSalaryRecon(CompId, PayPerd, 0,"", "", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "report/rdlc/rptSalaryRecon.rdlc";
            ReportDataSource datasource = new ReportDataSource("spSalaryReconTORResult", sal);

            ReportParameter prm = new ReportParameter("rpt_Prm_PayPeriod", ddlPayPerd.SelectedItem.Text);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
            viewer.LocalReport.SetParameters(new ReportParameter[] { prm });

            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
            Response.BinaryWrite(bytes);
            //// create the file
            //// send it to the client to download
            Response.Flush();
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            int iPeriod;
            int.TryParse(ddlPayPerd.SelectedValue, out iPeriod);
            PayPerd = iPeriod;

            CreatePDF("SalaryReconciliation", ddlExport.Text);
        }

    }
}

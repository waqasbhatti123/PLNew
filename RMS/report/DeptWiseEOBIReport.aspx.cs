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

namespace RMS.report.rdlc
{
    public partial class DeptWiseEOBIReport : System.Web.UI.Page
    {
        BL.SalaryBL SalBl = new RMS.BL.SalaryBL();

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
            this.ddlPayPerd.DataTextField = "PayPerd";
            ddlPayPerd.DataValueField = "PayPerd";
            ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlPayPerd.DataBind();
        }

        protected void Load_report()
        {
            IQueryable<spSalaryListResult> sal;
            sal = SalBl.rptEmpListEOBI(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ReportViewer viewer = new ReportViewer();

            viewer.LocalReport.ReportPath = "report/rdlc/rptSalaryEOBISummary.rdlc";
            ReportDataSource datasource = new ReportDataSource("spSalaryListResult", sal);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.Refresh();
            //ReportViewer1 = viewer;
            
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalaryEOBIDeptWiseReport").ToString();
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


        protected void CreatePDF(String FileName)
        {
        // Variables
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());


            //Warning[] warnings = null;
            //String[] streamids = null;
            //string mimeType = null;
            //string encoding = null;
            ////The DeviceInfo settings should be changed based on the reportType
            ////http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            //string deviceInfo =
            //"<DeviceInfo>" +
            //"  <OutputFormat>" + extension + "</OutputFormat>" +
            //"  <PageWidth>8.27in</PageWidth>" +
            //"  <PageHeight>11.69in</PageHeight>" +
            //"  <MarginTop>0.5in</MarginTop>" +
            //"  <MarginLeft>0.5in</MarginLeft>" +
            //"  <MarginRight>0.5in</MarginRight>" +
            //"  <MarginBottom>0.5in</MarginBottom>" +
            //"</DeviceInfo>";

        // Setup the report viewer object and get the array of bytes
            //ReportViewer viewer =new ReportViewer();
            string paypd = ddlPayPerd.SelectedItem.Text;
            string yr = paypd.Substring(0, 4);
            string mn = paypd.Substring(4, 2);
            DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);

            IQueryable<spSalaryListResult> sal;
            sal = SalBl.rptEmpListEOBI(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "report/rdlc/rptDeptWiseEOBI.rdlc";
            ReportDataSource datasource = new ReportDataSource("spSalaryListResult", sal);
          //  ReportParameter prm = new ReportParameter("rpt_Prm_PayPeriod", ddlPayPerd.SelectedItem.Text);

            ReportParameter[] paramz = new ReportParameter[3];
            paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            if (Session["CompName"] == null)
            {
                paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[2] = new ReportParameter("LogoPath", rptLogoPath);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);


            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);


            //Byte[] bytes = viewer.LocalReport.Render(extension=="XLS"?"EXCEL":extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            ////Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
            //Response.BinaryWrite(bytes);
            ////// create the file
            ////// send it to the client to download
            //Response.Flush();
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            int iPeriod;
            int.TryParse(ddlPayPerd.SelectedValue, out iPeriod);
            PayPerd = iPeriod;

            CreatePDF("DeptWiseEOBIReport");
        }

    }
}

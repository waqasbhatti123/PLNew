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

namespace RMS.report
{
    public partial class LeaveBalanceReport : System.Web.UI.Page
    {
        PlLeaveBL lvBl = new PlLeaveBL();

        #region Properties
      
        public int CompId
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpLeaveBalReport").ToString();

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

                cal1.Format = "dd-MMM-yyyy";
                cal1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
            }
        }

        protected void CreatePDF(String FileName)
        {
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
            //"  <PageWidth>11.69in</PageWidth>" +
            //"  <PageHeight>8.27in</PageHeight>" +
            //"  <MarginTop>0.5in</MarginTop>" +
            //"  <MarginLeft>0.1in</MarginLeft>" +
            //"  <MarginRight>0.1in</MarginRight>" +
            //"  <MarginBottom>0.5in</MarginBottom>" +
            //"</DeviceInfo>";

            // Setup the report viewer object and get the array of bytes
            //ReportViewer viewer = new ReportViewer();

            //IQueryable<spLeaveBalRptResult> lvz;
            //lvz = lvBl.GetLeaveBalanceData(CompId, Convert.ToDateTime(txtToDate.Text), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "report/rdlc/rptLeaveBalReport.rdlc";
            ReportDataSource datasource = new ReportDataSource("spLeaveBalRptResult", "");

            ReportParameter[] paramz = new ReportParameter[3];
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[1] = new ReportParameter("LogoPath", rptLogoPath);
            paramz[2] = new ReportParameter("ToDate", Convert.ToDateTime(txtToDate.Text).ToString("dd-MMM-yyyy"));

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            //ReportViewer1 = viewer;

            //Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

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
            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                ucMessage.ShowMessage("Enter correct date.", RMS.BL.Enums.MessageType.Error);
                return;
            }

            CreatePDF("LeaveBalanceReport");
        }

    }
}

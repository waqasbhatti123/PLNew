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

namespace RMS.report
{
    public partial class DeptWiseGratuityReport : System.Web.UI.Page
    {
        BL.SalaryBL SalBl = new RMS.BL.SalaryBL();

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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GratuityStatDeptWiseReport").ToString();
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

                calToDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                calToDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
            }
        }

        protected void CreatePDF(String FileName)
        {
        // Variables
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());


        //    Warning[] warnings = null;
        //    String[] streamids = null;
        //    string mimeType = null;
        //    string encoding = null;
        //    //The DeviceInfo settings should be changed based on the reportType
        //    //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
        //    string deviceInfo =
        //    "<DeviceInfo>" +
        //    "  <OutputFormat>" + extension + "</OutputFormat>" +
        //    "  <PageWidth>11.69in</PageWidth>" +
        //    "  <PageHeight>8.27in</PageHeight>" +
        //    "  <MarginTop>0.5in</MarginTop>" +
        //    "  <MarginLeft>0.5in</MarginLeft>" +
        //    "  <MarginRight>0.5in</MarginRight>" +
        //    "  <MarginBottom>0.5in</MarginBottom>" +
        //    "</DeviceInfo>";

        //// Setup the report viewer object and get the array of bytes
        //    ReportViewer viewer =new ReportViewer();

            DateTime toDate = Convert.ToDateTime(txtToDate.Text);

            List<spGratuityStatmentResult> gratuity;
            gratuity = SalBl.GetGratuityStatement(CompId, toDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "report/rdlc/rptDeptWiseGratuityStatement.rdlc";
            ReportDataSource datasource = new ReportDataSource("spGratuityStatmentResult", gratuity);
          
            ReportParameter[] paramz = new ReportParameter[3];
            paramz[0] = new ReportParameter("rpt_Prm_ToDate", toDate.ToString(), false);

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
            CreatePDF("GratuityStatement");
        }

    }
}

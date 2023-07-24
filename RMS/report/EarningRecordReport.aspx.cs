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
    public partial class EarningRecordReport : System.Web.UI.Page
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
            this.ddlFromPayPerd.DataTextField = "PayPerd";
            ddlFromPayPerd.DataValueField = "PayPerd";
            ddlFromPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFromPayPerd.DataBind();

            this.ddlToPayPerd.DataTextField = "PayPerd";
            ddlToPayPerd.DataValueField = "PayPerd";
            ddlToPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlToPayPerd.DataBind();
        }

        protected void Load_report()
        {
            //IQueryable<spSalaryTransferResult> sal;
            //sal = SalBl.RptSalTransferCash(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ReportViewer viewer = new ReportViewer();

            //viewer.LocalReport.ReportPath = "report/rdlc/rptSalaryTransfer.rdlc";
            //ReportDataSource datasource = new ReportDataSource("spSalaryTransferResult", sal);
            //viewer.LocalReport.DataSources.Clear();
            //viewer.LocalReport.DataSources.Add(datasource);

            //viewer.LocalReport.Refresh();
            ////ReportViewer1 = viewer;
            
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EarningRec").ToString();
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

        //    Warning[] warnings = null;
        //    String[] streamids = null;
        //    string mimeType = null;
        //    string encoding = null;
        //    //The DeviceInfo settings should be changed based on the reportType
        //    //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
        //    string deviceInfo =
        //    "<DeviceInfo>" +
        //    "  <OutputFormat>" + extension + "</OutputFormat>" +
        //    "  <PageWidth>8.5in</PageWidth>" +
        //    "  <PageHeight>11in</PageHeight>" +
        //    "  <MarginTop>0.5in</MarginTop>" +
        //    "  <MarginLeft>0.5in</MarginLeft>" +
        //    "  <MarginRight>0.5in</MarginRight>" +
        //    "  <MarginBottom>0.5in</MarginBottom>" +
        //    "</DeviceInfo>";

        //// Setup the report viewer object and get the array of bytes
        //    ReportViewer viewer =new ReportViewer();
            string frompaypd = ddlFromPayPerd.SelectedItem.Text;
            string topaypd = ddlToPayPerd.SelectedItem.Text;

            string yr = frompaypd.Substring(0, 4);
            string mn = frompaypd.Substring(4, 2);
            DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);

            string yr1 = topaypd.Substring(0, 4);
            string mn1 = topaypd.Substring(4, 2);
            DateTime ddto = new DateTime(Convert.ToInt32(yr1), Convert.ToInt32(mn1), 13);


                List<spEarningRecResult> earning;
                earning = SalBl.GetEarningRecords(CompId, Convert.ToInt32(frompaypd), Convert.ToInt32(topaypd), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "report/rdlc/EarnngRec.rdlc";
                ReportDataSource datasource = new ReportDataSource("spEarningRecResult", earning);
         //   ReportParameter prm = new ReportParameter("rpt_Prm_PayPeriod", ddlFromPayPerd.SelectedItem.Text);

            ReportParameter[] paramz = new ReportParameter[4];
            paramz[0] = new ReportParameter("rpt_Prm_Frm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);
            paramz[1] = new ReportParameter("rpt_Prm_To_PayPeriod", ddto.ToString("MMM-yyyy"), false);

            if (Session["CompName"] == null)
            {
                paramz[2] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[2] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[3] = new ReportParameter("LogoPath", rptLogoPath);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);


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
            int iPeriod;
            int.TryParse(ddlFromPayPerd.SelectedValue, out iPeriod);
            PayPerd = iPeriod;

            CreatePDF("EarningRecord");
        }

    }
}

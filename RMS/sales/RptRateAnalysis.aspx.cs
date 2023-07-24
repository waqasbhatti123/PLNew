using System;
using System.Web.UI;
using RMS.BL;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace RMS.sales
{
    public partial class RptRateAnalysis : System.Web.UI.Page
    {
        #region DataMember
        
        SalesRptBL slBL = new SalesRptBL();

        #endregion

        #region Properties

        public int CompId
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }

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

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "RateAnalysis").ToString();
                
                CompId = iCompid;
                
                calFromDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                txtFromDate.Text = Convert.ToDateTime( Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month + "-01-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year).
                                ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                calToDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                txtToDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).
                                ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                btnGenerat.Focus();
            }
        }
        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateReport("RatesAnalysis");
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion

        #region HelpingMethods

        protected void GenerateReport(string reportName)
        {
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            
            viewer.Visible = true;
            DateTime fromDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                fromDate = Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                ucMessage.ShowMessage("Please, enter valid from date", RMS.BL.Enums.MessageType.Error);
                return;
            }
            DateTime toDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                toDate = Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                ucMessage.ShowMessage("Please, enter valid to date", RMS.BL.Enums.MessageType.Error);
                return;
            }

            List<spRateAnanlysisResult> recs;
            recs = slBL.GetRatesAnalysis(fromDate, toDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            
            viewer.LocalReport.ReportPath = "sales/rdlc/RatesAnalysis.rdlc";

            ReportDataSource datasource = new ReportDataSource("spRateAnanlysisResult", recs);

            ReportParameter[] paramz = new ReportParameter[4];
            paramz[0] = new ReportParameter("rpt_Prm_FromDate", fromDate.ToString(), false);
            paramz[1] = new ReportParameter("rpt_Prm_ToDate", toDate.ToString(), false);

            if (Session["CompName"] == null)
            {
                paramz[2] = new ReportParameter("rpt_Prm_CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[2] = new ReportParameter("rpt_Prm_CompName", Session["CompName"].ToString(), false);
            }
            paramz[3] = new ReportParameter("rpt_Prm_LogoPath", rptLogoPath);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);


            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }

        #endregion
    }
}

using System;
using System.Web.UI;
using RMS.BL;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace RMS.sales
{
    public partial class RptStatementOfAc : System.Web.UI.Page
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
        public int BrId
        {
            get { return (ViewState["BrId"] == null) ? 0 : Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
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
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"].ToString());
            }
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "StatementOfAC").ToString();
                
                CompId = iCompid;
                
                calFromDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                txtFromDate.Text = Convert.ToDateTime( Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month + "-01-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year).
                                ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                calToDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                txtToDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).
                                ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                FillDdlVendor();
                btnGenerat.Focus();
            }
        }
        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateReport(Convert.ToString(Session["PageTitle"]));
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
            string filter = "";
            filter = "From Date: " + fromDate.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]) + "    " +
                     "To Date: " + toDate.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);




            List<spStatementOfAcResult> recs;
            recs = slBL.GetStatementOfAc(BrId, ddlVendor.SelectedValue, fromDate, toDate, ddlStatus.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "sales/rdlc/StatementOfAcRpt.rdlc";

            ReportDataSource datasource = new ReportDataSource("spStatementOfAcResult", recs);

            ReportParameter[] paramz = new ReportParameter[4];
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[1] = new ReportParameter("RptName", reportName, false);
            paramz[2] = new ReportParameter("Filter", filter, false);
            paramz[3] = new ReportParameter("LogoPath", rptLogoPath);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);


            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }
        private void FillDdlVendor()
        {
            ddlVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.DataTextField = "gl_dsc";
            ddlVendor.DataValueField = "gl_cd";
            ddlVendor.DataBind();
        }

        #endregion
    }
}

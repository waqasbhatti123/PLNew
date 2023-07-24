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
    public partial class TurnOverReport : System.Web.UI.Page
    {
        EmpBL empBl = new EmpBL();

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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "TurnOverRpt").ToString();
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

                calFromDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                txtFromDate.Text = "01"+"-01-"+ Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year;
                calFromDate.SelectedDate = Convert.ToDateTime(txtFromDate.Text).Date;
                
                calToDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                txtToDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date.ToString();
                calToDate.SelectedDate = Convert.ToDateTime(txtToDate.Text).Date;
            }
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateReport("TurnOverReport");
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void GenerateReport(string reportName)
        {
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            
            viewer.Visible = true;
            DateTime fromDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) ;
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

            List<spTurnOverResult> turnOver;
            turnOver = empBl.GetTurnOverRpt(fromDate, toDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "report/rdlc/rptTurnOver.rdlc";
            ReportDataSource datasource = new ReportDataSource("spTurnOverResult", turnOver);

            ReportParameter[] paramz = new ReportParameter[4];
            paramz[0] = new ReportParameter("rpt_Prm_FromDate", fromDate.ToString(), false);
            paramz[1] = new ReportParameter("rpt_Prm_ToDate", toDate.ToString(), false);

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
        }
    }
}

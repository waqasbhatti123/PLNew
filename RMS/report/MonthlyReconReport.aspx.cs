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
using System.Windows.Forms;
using System.IO;

namespace RMS.report
{
    public partial class MonthlyReconReport : System.Web.UI.Page
    {

        #region Data Members

        BL.PlReportBL rptBL = new RMS.BL.PlReportBL();
        BL.SalaryBL SalBl = new RMS.BL.SalaryBL();

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
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "MonthlyReconReport").ToString();
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

                try
                {
                    FillDropDownPayPeriod();
                }
                catch(Exception ex) 
                {
                    ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
                }
            }
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            CreateReport("MonthlyReconciliation");
        }

        #endregion

        #region Helping Method

        protected void CreateReport(String FileName)
        {
            // Variables
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            string paypd = ddlPayPerd.SelectedItem.Text;
            string yr = paypd.Substring(0, 4);
            string mn = paypd.Substring(4, 2);
            DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);
            
            string strPrevPayPerd="";
            DateTime ddto;
            if (Convert.ToInt32(mn) > 1)
            {
                strPrevPayPerd = yr + (Convert.ToInt32(mn) - 1).ToString().PadLeft(2,'0');
                ddto = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn)-1, 13);
            }
            else
            {
                strPrevPayPerd = (Convert.ToInt32(yr) - 1) + "12";
                ddto = new DateTime(Convert.ToInt32(yr) -1, 12, 12);
            }
            int prevPayPerd = Convert.ToInt32(strPrevPayPerd);

            int iPeriod;
            int.TryParse(ddlPayPerd.SelectedValue, out iPeriod);
            int PayPerd = iPeriod;

            object sal;
            sal = rptBL.GetMonthlyRecon(CompId, PayPerd, prevPayPerd , (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "report/rdlc/MontylyReconciliation.rdlc";
            ReportDataSource datasource = new ReportDataSource("spMontylyReconResult", sal);

            ReportParameter[] paramz = new ReportParameter[5];
            paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);
            paramz[1] = new ReportParameter("rpt_Prm_Prev_PayPeriod", ddto.ToString("MMM-yyyy"), false);

            if (Session["CompName"] == null)
            {
                paramz[2] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[2] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }

            if (Session["UserName"] == null)
            {
                paramz[3] = new ReportParameter("UserName", Request.Cookies["uzr"]["UserName"].ToString(), false);
            }
            else
            {
                paramz[3] = new ReportParameter("UserName", Session["UserName"].ToString(), false);
            }
            paramz[4] = new ReportParameter("LogoPath", rptLogoPath);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }

        private void FillDropDownPayPeriod()
        {
            this.ddlPayPerd.DataTextField = "PayPerd";
            ddlPayPerd.DataValueField = "PayPerd";
            ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlPayPerd.DataBind();
        }

        #endregion
    }
}

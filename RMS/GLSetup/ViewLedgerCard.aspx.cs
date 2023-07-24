using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using RMS.UserControl;
using Microsoft.Reporting.WebForms;

namespace RMS.GLSetup
{
    public partial class ViewLedgerCard : System.Web.UI.Page
    {
        #region DataMembers

        LedgerCardBL cty = new LedgerCardBL();

        #endregion

        #region Properties

#pragma warning disable CS0114 // 'ViewLedgerCard.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'ViewLedgerCard.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            set { ViewState["ID"] = value; }
            get { return Convert.ToInt32(ViewState["ID"]); }
        }

        public string GlCode
        {
            set { ViewState["GlCode"] = value; }
            get { return Convert.ToString(ViewState["GlCode"]); }
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
                else
                {
                    ID = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                }
            }
            else
            {
                ID = Convert.ToInt32(Session["UserID"].ToString());
            }

            if (!IsPostBack)
            {
                try
                {
                    GlCode = Request.QueryString["Code"];
                    FIN_PERD fnObject = cty.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                    DateTime currentDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    DateTime fromDate = Convert.ToDateTime(fnObject.Start_Date);
                    GetReport(fromDate.Date, currentDate.Date, GlCode, fnObject.Gl_Year);
                }
                catch (Exception ex)
                {
                    ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
                }
            }
        }

        #endregion

        #region HelpingMethods

        public void GetReport(DateTime frmDate, DateTime toDate, string glCode, decimal glYear)
        {
            List<sp_LedgerInfoResult> ledger = cty.GetLedgerBal1(frmDate, toDate, glCode, glYear, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], 'A');


            viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptLedgerView.rdlc";
            ReportDataSource datasource = new ReportDataSource("sp_LedgerResult", ledger);
            ReportParameter[] paramz = new ReportParameter[4];

            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[1] = new ReportParameter("dateRange", "Period From:  " + frmDate.ToString("dd-MMM-yyyy") + "  To  " + toDate.ToString("dd-MMM-yyyy"));
            paramz[2] = new ReportParameter("PageTitle", "Ledger Card");
            paramz[3] = new ReportParameter("GLCode", GlCode);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);


            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }

        #endregion
    }
}

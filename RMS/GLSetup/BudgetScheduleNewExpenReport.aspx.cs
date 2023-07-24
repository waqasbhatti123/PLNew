using Microsoft.Reporting.WebForms;
using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.GLSetup
{
    public partial class BudgetScheduleNewExpenReport : System.Web.UI.Page
    {
        NewExpenditure exp = new NewExpenditure();

#pragma warning disable CS0114 // 'BudgetScheduleNewExpenReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'BudgetScheduleNewExpenReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int ProID
        {
            get { return (ViewState["ProID"] == null) ? 0 : Convert.ToInt32(ViewState["ProID"]); }
            set { ViewState["ProID"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        public static decimal Financialyear
        {
            get; set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "schedule").ToString();
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
            }
        }

        protected void GeneReport_click(object sender, EventArgs e)
        {
            string glyear = SelectedYear.SelectedValue;
            string[] yr = glyear.Split('-');
            int year = Convert.ToInt32(yr[1]);

            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            IList<sp_SneBranchWiseResult> sne = exp.GetSNEDemand(year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptSneDemandReport.rdlc";
            ReportDataSource datasource = new ReportDataSource("DataSet1", sne);
            //ReportParameter prm = new ReportParameter("Loan_ReportResult");
            ReportParameter[] paramz = new ReportParameter[3];
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("ComName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("ComName", Session["CompName"].ToString(), false);
            }
            paramz[1] = new ReportParameter("SelectYear", glyear);
            paramz[2] = new ReportParameter("LogoPath", rptLogoPath);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }


    }
}
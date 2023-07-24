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
    public partial class DistGrntAmounAllCouReport : System.Web.UI.Page
    {
        NewExpenditure exp = new NewExpenditure();

#pragma warning disable CS0114 // 'DistGrntAmounAllCouReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'DistGrntAmounAllCouReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "DistGrantReport").ToString();
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
            string[] year = glyear.Split('-');
            int yr = Convert.ToInt32(year[1]);

            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            if (GrantSelect.SelectedValue == "0")
            {
                IList<sp_DistGrantAllCouncilResult> sne = exp.GistGrant(yr, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptDistGrantAmoungCouncil.rdlc";
                ReportDataSource dataSource = new ReportDataSource("DataSet1", sne);
                ReportParameter[] param = new ReportParameter[3];
                if (Session["CompName"] == null)
                {
                    param[0] = new ReportParameter("ComName", Request.Cookies["uzr"]["CompName"].ToString(), false);
                }
                else
                {
                    param[0] = new ReportParameter("ComName", Session["CompName"].ToString(), false);
                }
                param[1] = new ReportParameter("SelectYear", glyear);
                param[2] = new ReportParameter("LogoPath", rptLogoPath);

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(param);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(dataSource);
            }
            if (GrantSelect.SelectedValue == "1")
            {
                IList<sp_DistGrantAllCouncilResult> sne = exp.GistGrant(yr, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptDistGrantAmoungCouncilHalf.rdlc";
                ReportDataSource dataSource = new ReportDataSource("DataSet1", sne);
                ReportParameter[] param = new ReportParameter[3];
                if (Session["CompName"] == null)
                {
                    param[0] = new ReportParameter("ComName", Request.Cookies["uzr"]["CompName"].ToString(), false);
                }
                else
                {
                    param[0] = new ReportParameter("ComName", Session["CompName"].ToString(), false);
                }
                param[1] = new ReportParameter("SelectYear", glyear);
                param[2] = new ReportParameter("LogoPath", rptLogoPath);

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(param);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(dataSource);
            }
            if (GrantSelect.SelectedValue == "2")
            {
                IList<sp_DistGrantAllCouncilResult> sne = exp.GistGrant(yr, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptDistGrantAmoungCouncilQuarterly.rdlc";
                ReportDataSource dataSource = new ReportDataSource("DataSet1", sne);
                ReportParameter[] param = new ReportParameter[3];
                if (Session["CompName"] == null)
                {
                    param[0] = new ReportParameter("ComName", Request.Cookies["uzr"]["CompName"].ToString(), false);
                }
                else
                {
                    param[0] = new ReportParameter("ComName", Session["CompName"].ToString(), false);
                }
                param[1] = new ReportParameter("SelectYear", glyear);
                param[2] = new ReportParameter("LogoPath", rptLogoPath);

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(param);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(dataSource);
            }
        }
    }
}
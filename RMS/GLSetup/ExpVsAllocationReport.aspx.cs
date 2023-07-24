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
    public partial class ExpVsAllocationReport : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        NewExpenditure exp = new NewExpenditure();

#pragma warning disable CS0114 // 'ExpVsAllocationReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'ExpVsAllocationReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "expAll").ToString();
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                DropDownDivision();
            }
        }




        protected void Report_Click(object sender, EventArgs e)
        {
            int div = Convert.ToInt32(ddlDivisional.SelectedValue);
            string year = SelectedYear.SelectedValue;
            string[] yr = year.Split('-');
            int yearr = Convert.ToInt32(yr[1]);
            // Variables
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());



            IList<sp_ExpVsAllocationResult> expp =exp.GetExpAll(yearr,div, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToList();

            viewer.LocalReport.ReportPath = "glsetup/rdlc/rptExpAllocation.rdlc";
            ReportDataSource datasource = new ReportDataSource("DataSet1", expp);
            //ReportParameter prm = new ReportParameter("Loan_ReportResult");
            ReportParameter[] paramz = new ReportParameter[3];

            //paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }

            paramz[1] = new ReportParameter("PucarLogo", rptLogoPath);
            paramz[2] = new ReportParameter("SelectYear", SelectedYear.SelectedValue);
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }


        protected void DropDownDivision()
        {
            Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            ddlDivisional.DataValueField = "br_id";
            ddlDivisional.DataTextField = "br_nme";
            if (br.IsHead == true)
            {
                ddlDivisional.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                ddlDivisional.DataSource = db.Branches.Where(x => x.br_status == true && x.br_id == BranchID).ToList();
            }
            ddlDivisional.DataBind();
            ddlDivisional.Items.Insert(0, new ListItem("Select Division", "0"));
        }
    }
}
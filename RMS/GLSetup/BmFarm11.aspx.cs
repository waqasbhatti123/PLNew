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
    public partial class BmFarm11 : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        NewExpenditure exp = new NewExpenditure();
#pragma warning disable CS0114 // 'BmFarm11.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'BmFarm11.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "bmfarm11").ToString();
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }

                FillDivisionDropdown();
                ddlDivisional.SelectedValue = BranchID.ToString();
            }
        }



        protected void FillDivisionDropdown()
        {
            Branch Br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            ddlDivisional.DataTextField = "br_nme";
            ddlDivisional.DataValueField = "br_id";
            if (Br.IsHead == true)
            {
                ddlDivisional.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                ddlDivisional.DataSource = db.Branches.Where(x => x.br_id == BranchID).ToList();
            }
            ddlDivisional.DataBind();
            ddlDivisional.Items.Insert(0, new ListItem("Select Division", "0"));
        }

        protected void btnFrom10_Save(object sender, EventArgs e)
        {
             string gl = SelectedYear.SelectedValue;
            //DateTime dateTime = Convert.ToDateTime(txtfrom.Text);
            int Bran = Convert.ToInt32(ddlDivisional.SelectedValue);
            
            Branch br = db.Branches.Where(x => x.br_id == BranchID && x.br_status == true).FirstOrDefault();
            // Variables
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());



            IList<sp_BMFormthreeResult> bm = exp.GetFarm11Data(Bran, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToList();

            viewer.LocalReport.ReportPath = "report/rdlc/rptBmForm11.rdlc";
            ReportDataSource datasource = new ReportDataSource("DataSet1", bm);
            //ReportParameter prm = new ReportParameter("Loan_ReportResult");
            ReportParameter[] paramz = new ReportParameter[4];

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
            paramz[2] = new ReportParameter("DivName", br.br_nme);
            paramz[3] = new ReportParameter("glyear", gl.ToString());
            //paramz[4] = new ReportParameter("datete", dateTime.ToString("dd-MMM-yyyy"));
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }
    }
}
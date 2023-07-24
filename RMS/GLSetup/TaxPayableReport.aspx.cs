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
    public partial class TaxPayableReport : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        TaxpayableBL tex = new TaxpayableBL();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "textPayableReport").ToString();
                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                FillDropDownChq();
            }

        }



        private void FillDropDownChq()
        {
            AccountID.DataTextField = "vr_chq"; 
            AccountID.DataValueField = "vrid";
            AccountID.DataSource = (from a in db.Glmf_Datas
                                    join chq in db.Glmf_Data_chqs on a.vrid equals chq.vrid
                                    where a.vt_cd == 64 && a.vr_apr == "A" && a.br_id == BranchID
                                    orderby a.vr_no descending, a.vr_dt descending
                                    select chq).ToList();
            AccountID.DataBind();
        }


        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            int account =Convert.ToInt32(AccountID.SelectedValue);
            // Variables
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());


            
            var tax = tex.rptGenerateReportyTax("",account);

            viewer.LocalReport.ReportPath = "report/rdlc/rptTaxpay.rdlc";
            ReportDataSource datasource = new ReportDataSource("DataSet1", tax);
            //ReportParameter prm = new ReportParameter("Loan_ReportResult");
            ReportParameter[] paramz = new ReportParameter[1];

            //paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            //if (Session["CompName"] == null)
            //{
            //    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            //}
            //else
            //{
            //    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            //}

            paramz[0] = new ReportParameter("PucarLogo", rptLogoPath);
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }
    }
}
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
    public partial class BankWiseExpense : System.Web.UI.Page
    {
        NewExpenditure exp = new NewExpenditure();

#pragma warning disable CS0114 // 'BankWiseExpense.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'BankWiseExpense.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "bnkexp").ToString();
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
                FillBankName(BranchID);
            }
        }



        protected void btnProgress_Save(object sender, EventArgs e)
        {
            string glyear;
            decimal bank;
            if (SelectedYear.SelectedValue == "0")
            {
                ucMessage.ShowMessage("Please Select Financial Year", BL.Enums.MessageType.Error);
                return;
            }
            else
            {
                 glyear = SelectedYear.SelectedValue;
            }
            if (dllBank.SelectedValue == "")
            {
                ucMessage.ShowMessage("Please Select Bank", BL.Enums.MessageType.Error);
                return;
            }
            else
            {
                 bank = Convert.ToDecimal(dllBank.SelectedValue);
            }

            CreatePDF(glyear, bank);
        }

        public void CreatePDF(string glyears, decimal banks)
        {
            string glyear = SelectedYear.SelectedValue;
            string[] year = glyear.Split('-');
            int yr = Convert.ToInt32(year[1]);
            int bnk = Convert.ToInt32(dllBank.SelectedValue);
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            IList<sp_BankWiseExpenseResult> sne = exp.GetBankWiseExp(bnk,yr,BranchID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBankWiseExp.rdlc";
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

        private void FillBankName(int br)
        {
            using(RMSDataContext Data = new RMSDataContext())
            {
                var dat = (from gc in Data.Glmf_Codes
                           join v in  Data.glmf_ven_cus_dets on gc.gl_cd equals v.gl_cd
                           into gl
                           from v in gl.DefaultIfEmpty()
                           where (gc.ct_id == "D") && br == (v != null && v.br_id != null ? v.br_id : br)
                           && gc.cnt_gl_cd == "010101"
                           select gc).ToList();

                dllBank.DataTextField = "gl_dsc";
                dllBank.DataValueField = "gl_cd";
                dllBank.DataSource = dat.ToList();
                dllBank.DataBind();

            }

        }
    }
}
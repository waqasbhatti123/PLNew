using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using Microsoft.Reporting.WebForms;

namespace RMS.GLSetup
{
    public partial class frmCCAccount : System.Web.UI.Page
    {
        LedgerCardBL cty = new LedgerCardBL();
        PucarAccountReportsBL pucarBl = new PucarAccountReportsBL();
        CuurentYearBL cuurentYear = new CuurentYearBL();
        //CostCenterBL ccBl = new CostCenterBL();

        public int BrID
        {
            get { return Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrID = Convert.ToInt32(Session["BranchID"]);
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "CCAccount").ToString();


                ddlGlYear.SelectedValue = cty.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]).Gl_Year.ToString();

                //DateTime currentDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //calFromDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                //calToDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];

                txtFromDate.Text = cuurentYear.fromFinalYearDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                txtToDate.Text = cuurentYear.toFinalYearDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
               

                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BrID.ToString();
                FillHeadsDetail();
                FillCostCentre();
            }
        }

        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BrID).FirstOrDefault();

            this.searchBranchDropDown.DataTextField = "br_nme";
            searchBranchDropDown.DataValueField = "br_id";
            if (BranchObj.IsHead == true)
            {
                searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                List<Branch> BranchList = new List<Branch>();

                if (BranchObj != null)
                {
                    if (BranchObj.IsDisplay == true)
                    {
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BrID).ToList();
                        BranchList.Insert(0, BranchObj);
                    }
                    else
                    {
                        BranchList.Add(BranchObj);
                    }
                }
                searchBranchDropDown.DataSource = BranchList.ToList();
            }
            searchBranchDropDown.DataBind();

        }


        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                    //IsSearch = true;
                    BrID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }




        private void FillHeadsDetail()
        {
            RMSDataContext db = new RMSDataContext();

            this.codeDropDown.DataTextField = "gl_dsc";
            codeDropDown.DataValueField = "gl_cd";

            codeDropDown.DataSource = db.Glmf_Codes.Where(x => x.ct_id == "D").ToList();
            codeDropDown.DataBind();


        }

        private void FillCostCentre()
        {
            RMSDataContext db = new RMSDataContext();

            this.costCenterDropDown.DataTextField = "cc_nme";
            costCenterDropDown.DataValueField = "cc_cd";

            costCenterDropDown.DataSource = db.Cost_Centers.Where(x => x.cct_id == "D" && x.status == true).ToList();
            costCenterDropDown.DataBind();


        }
        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateReport();
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        private void GenerateReport()
        {
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                ucMessage.ShowMessage("Please, enter valid from date", RMS.BL.Enums.MessageType.Error);
                return;
            }
            
            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                ucMessage.ShowMessage("Please, enter valid to date", RMS.BL.Enums.MessageType.Error);
                return;
            }
            //if (!string.IsNullOrEmpty(txtFromCC.Text))
            //{
            //    ccfrom = txtFromCC.Text;
            //}
            //if (!string.IsNullOrEmpty(txtToCC.Text))
            //{
            //    ccto = txtToCC.Text;
            //}
            //if (!string.IsNullOrEmpty(txtFromAC.Text))
            //{
            //    frglcd = txtFromAC.Text;
            //}
            //if (!string.IsNullOrEmpty(txtToAC.Text))
            //{
            //    toglcd = txtToAC.Text;
            //}



            //List<sp_GL_Ledger_CCResult> cc;
            //cc = new CCBL().GetCCLedgher(BrID, Convert.ToDecimal(ddlGlYear.SelectedValue), Convert.ToDateTime(txtFromDate.Text),
            //    Convert.ToDateTime(txtToDate.Text), ccfrom, ccto, frglcd, toglcd, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);



            List<sp_GL_Ledger_CCResult> cc;
            cc = pucarBl.CostCentreReport(BrID, Convert.ToDecimal(ddlGlYear.SelectedValue), Convert.ToDateTime(txtFromDate.Text),
                Convert.ToDateTime(txtToDate.Text), costCenterDropDown.SelectedValue.Trim(), codeDropDown.SelectedValue.Trim(), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToList();



            viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptCCLedger.rdlc";
            ReportDataSource datasource = new ReportDataSource("sp_GL_Ledger_CCResult", cc);

            ReportParameter[] paramz = new ReportParameter[8];
            paramz[0] = new ReportParameter("rpt_Prm_FromDate", txtFromDate.Text, false);
            paramz[1] = new ReportParameter("rpt_Prm_ToDate", txtToDate.Text, false);

            if (Session["CompName"] == null)
            {
                paramz[2] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[2] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[3] = new ReportParameter("LogoPath", rptLogoPath);
            paramz[4] = new ReportParameter("CC", costCenterDropDown.SelectedItem.Text.Trim());
           // paramz[5] = new ReportParameter("ToCC", ccto == "999999999999" ? "999999" : txtToCC.Text);
            paramz[5] = new ReportParameter("GlYear", ddlGlYear.SelectedValue);
            paramz[6] = new ReportParameter("AC", codeDropDown.SelectedItem.Text.Trim());
            //paramz[8] = new ReportParameter("ToAC", toglcd == "999999999999" ? "999999999999" : txtToAC.Text);
            paramz[7] = new ReportParameter("RecType", ddlStatus.SelectedValue == "A" ? "" : "Provisional");


            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);


            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }

    }
}

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
    public partial class ExpStatMonthWise : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        NewExpenditure exp = new NewExpenditure();
        voucherDetailBL objVoucher = new voucherDetailBL();

        public int BrId
        {
            get { return (ViewState["BrId"] == null) ? 0 : Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }
        public decimal Financialyear
        {
            get { return (ViewState["Financialyear"] == null) ? 0 : Convert.ToInt32(ViewState["Financialyear"]); }
            set { ViewState["Financialyear"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = "monthly Expenditure Statement Report";

                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    BrId = Convert.ToInt32(Session["BranchID"]);
                }
                Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BrId.ToString();
                BrId = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            }
        }


        protected void btn_generateReprot(object sender, EventArgs e)
        {
            CreatePDF("rptMonthlyExpenditureStatement");
        }

        private void FillSearchBranchDropDown()
        {
            

            Branch BranchObj = db.Branches.Where(x => x.br_id == BrId).FirstOrDefault();

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
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BrId).ToList();
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

        protected void CreatePDF(String FileName)
        {
            string year = SelectedYear.SelectedValue;
            string[] yr = year.Split('-');
            decimal exactyear = Convert.ToDecimal(yr[1]);
            // Variables
            //int fin = Convert.ToInt32(Financialyear);
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            int br_id = Convert.ToInt32(searchBranchDropDown.SelectedValue);

            Branch brr = db.Branches.Where(x => x.br_id == br_id).FirstOrDefault();

            IList<sp_ExpenditureStatementMonthWiseResult> sal;
            sal = exp.GetMonExpenditureStatement(br_id, exactyear, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "glsetup/rdlc/rptGetExpStatementMonth.rdlc";
            ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
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
            //paramz[0] = new ReportParameter("selectedYear", year);
            paramz[1] = new ReportParameter("DiviName", brr.br_nme.ToString());
            paramz[2] = new ReportParameter("Fin", year);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

        }
    }
}
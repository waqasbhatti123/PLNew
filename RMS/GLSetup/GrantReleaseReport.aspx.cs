using Microsoft.Reporting.WebForms;
using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.report
{
    public partial class GrantReleaseReport : System.Web.UI.Page
    {
        BL.BudgetBL budgetRpt = new RMS.BL.BudgetBL();
        RMSDataContext db = new RMSDataContext();
        public int CompId
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        public int PayPerd
        {
            get { return (ViewState["PayPerd"] == null) ? 0 : Convert.ToInt32(ViewState["PayPerd"]); }
            set { ViewState["PayPerd"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

        protected void Load_report()
        {
           
            ReportViewer viewer = new ReportViewer();
           
            viewer.LocalReport.ReportPath = "report/rdlc/rptBudget.rdlc";
            ReportDataSource datasource = new ReportDataSource("BudgetDataSet");
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.Refresh();
            //ReportViewer1 = viewer;
            
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GrantReleaseReport").ToString();
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }


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

                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();


            }
        }




        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

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
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BranchID).ToList();
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
                
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
               

            }
            catch
            { }
        }





        protected void CreatePDF(String FileName)
        {
            string year = SelectedYear.SelectedValue;
            string[] yr = year.Split('-');
            decimal exactyear = Convert.ToDecimal(yr[1]);
            Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            string BranchName = br.br_nme;
            // Variables
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            var fin = db.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault();


            IQueryable<GrantRelaseReportResult> sal;
                sal = budgetRpt.GrantReport((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], exactyear, BranchID);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptReleaseGrant.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                //ReportParameter prm = new ReportParameter("Loan_ReportResult");
                ReportParameter[] paramz = new ReportParameter[2];
                paramz[0] = new ReportParameter("selectedYear", year);
                paramz[1] = new ReportParameter("BraName", BranchName);

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(paramz);

                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            

            //paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            //if (Session["CompName"] == null)
            //{
            //    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            //}
            //else
            //{
            //    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            //}
            //paramz[1] = new ReportParameter("LogoPath", rptLogoPath);

            

            
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {

            // int iPeriod;
            //int.TryParse(ddlPayPerd.SelectedValue, out iPeriod);
            // PayPerd = iPeriod;

            CreatePDF("GrantReleaseReport");
        }


    }
}
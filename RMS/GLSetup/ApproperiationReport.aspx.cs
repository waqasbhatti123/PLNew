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
    public partial class ApproperiationReport : System.Web.UI.Page
    {
        BL.BudgetBL budgetRpt = new RMS.BL.BudgetBL();
        PucarAccoutsNewReportBL pucarNew = new PucarAccoutsNewReportBL();
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "AppropReport").ToString();
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
           


            }
        }




        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            if(BranchObj != null)
            {
                if (BranchObj.IsHead == true)
                {
                    btnGenerat.Visible = true;
                }
                else
                {
                    btnGenerat.Visible = false;
                }
            }
           
          

        }


      



        protected void CreatePDF(String FileName)
        {
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            string year = SelectedYear.SelectedValue;
            string[] yr = year.Split('-');
            decimal exactyear = Convert.ToDecimal(yr[1]);
            int durationSelected = Convert.ToInt32(dropDownDuration.SelectedValue.Trim());

            var fin = db.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault();


            IQueryable<SP_AnnualGrantResult> sal;
           
                sal = pucarNew.AnnualGrantDivsionalReport(exactyear, durationSelected, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "glsetup/rdlc/rptAnnualGrant.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                //ReportParameter prm = new ReportParameter("Loan_ReportResult");
                ReportParameter[] paramz = new ReportParameter[3];


                paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
                paramz[1] = new ReportParameter("SelectedYear", year);
                paramz[2] = new ReportParameter("Quarterly", dropDownDuration.SelectedItem.Text.Trim());

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(paramz);

                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            


        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {

            // int iPeriod;
            //int.TryParse(ddlPayPerd.SelectedValue, out iPeriod);
            // PayPerd = iPeriod;

            CreatePDF("Annual_Grant_Release_Rport");
        }


    }
}
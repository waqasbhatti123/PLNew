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
    public partial class SalarySheetConsoledated : System.Web.UI.Page
    {
        SlalaryPacakageBL rptBL = new SlalaryPacakageBL();
        RMSDataContext db = new RMSDataContext();
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Consolidated").ToString();

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

                GetMonth();
            }
        }


        protected void Onclick_ReportGen(object sender, EventArgs e)
        {
            Branch brr = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            int JobTypeID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            string MonthName = MonthSelected.Text;
            TblSalaryMonth mon = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(MonthName) && x.BranchID == BranchID).FirstOrDefault();
            string slectedmonthyear = mon.MonthVal;
            int monthID = mon.MonthID;
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            ReportDataSource datasource;
           ReportParameter[] paramz;
            object sal;
           
                    sal = rptBL.ConsolidatedReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                    viewer.LocalReport.ReportPath = "report/rdlc/ConsolidatedReport.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);

            paramz = new ReportParameter[3];
            // paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString());
            }

            paramz[1] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));
            paramz[2] = new ReportParameter("Div", brr.br_nme);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
                

        }


        protected void GetMonth()
        {
            var month = db.TblSalaryMonths.Where(x => x.MonthIsActive == true && x.BranchID == BranchID).FirstOrDefault();
            this.MonthSelected.Text = month.MonthVal;
        }
    }
}
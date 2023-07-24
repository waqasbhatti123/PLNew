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
    public partial class IncomeTaxDeductionfromsalary : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        SalaryIncomeTax tax = new SalaryIncomeTax();
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }


        public int JobTypeID
        {
            get { return (ViewState["JobTypeID"] == null) ? 0 : Convert.ToInt32(ViewState["JobTypeID"]); }
            set { ViewState["JobTypeID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalIncomeRpt").ToString();
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
                FillddlJobType();
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                GetMonth();
            }
        }


        protected void FillddlJobType()
        {
            try
            {
                ddlJobType.DataTextField = "JobTypeName";
                ddlJobType.DataValueField = "JobTypeID";
                using (RMSDataContext dataContext = new RMSDataContext())
                {

                    ddlJobType.DataSource = dataContext.JobTypes.Where(x => x.IsActive == true).ToList();
                    ddlJobType.DataBind();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());


                }

            }
            catch
            { }
        }
        protected void ddlJobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlJobType.SelectedValue.Equals("0"))
                {
                    JobTypeID = Convert.ToInt32(ddlJobType.SelectedValue);

                }

            }
            catch
            { }
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

        protected void GetMonth()
        {
            var month = db.TblSalaryMonths.Where(x => x.MonthIsActive == true && x.BranchID == BranchID).FirstOrDefault();
            if (month.MonthVal != null || month.MonthVal == "")
            {
                this.MonthSelected.Text = month.MonthVal;
            }
            else
            {
                this.MonthSelected.Text = "";
            }

        }

        protected void btnReport_Gen(object sender, EventArgs e)
        {
            if (MonthSelected.Text.Trim() != null && MonthSelected.Text.Trim() != "")
            {
                try
                {
                    if (BranchID == 16)
                    {
                        string slectedMonth = MonthSelected.Text.Trim().ToLower().ToString();

                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonth) && x.BranchID == 1).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreatePDF("SalaryDeductionReport", selectedMonth, selectedMonthID);

                        }
                        else
                        {
                            ucMessage.ShowMessage("Salary transfer Month is not exist", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                    else
                    {
                        string slectedMonth = MonthSelected.Text.Trim().ToLower().ToString();

                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonth) && x.BranchID == BranchID).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreatePDF("SalaryIncomeTaxReport", selectedMonth, selectedMonthID);

                        }
                        else
                        {
                            ucMessage.ShowMessage("Salary transfer Month is not exist", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    ucMessage.ShowMessage(ex.Message.ToString(), RMS.BL.Enums.MessageType.Error);
                }
            }


        }

        protected void CreatePDF(String FileName, string MonthVal, int monthID)
        {
            string dedName = "";
            if (ddlDeductions.SelectedValue != "0")
            {
                dedName = ddlDeductions.SelectedValue;
            }
            else
            {
                dedName = ddlded.SelectedValue;
            }

            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            // ReportViewer viewer =new ReportViewer();
            
            if (ddlJobType.SelectedValue == "0")
            {
                
                string mon = MonthSelected.Text;
                int BrID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                Branch branch = db.Branches.Where(x => x.br_id == BrID).FirstOrDefault();
                //string all = ddlDeductions.SelectedValue;
                IList<sp_TaxDedFromSalaryResult> sal;
                sal = tax.GetTaxFromsalary(monthID, JobTypeID, BranchID,dedName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                viewer.LocalReport.ReportPath = "report/rdlc/rptTaxdedFromSalaryCopy.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);

                ReportParameter[] paramz = new ReportParameter[4];

                paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
                paramz[1] = new ReportParameter("Division", branch.br_nme);
                paramz[2] = new ReportParameter("mon", Convert.ToDateTime(mon).ToString("MMM-yyyy"));
                paramz[3] = new ReportParameter("dedName", dedName);

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(paramz);


                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else
            {
                string mon = MonthSelected.Text;
                int BrID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                int job = Convert.ToInt32(ddlJobType.SelectedValue);
                JobType jobty = db.JobTypes.Where(x => x.JobTypeID == job).FirstOrDefault();
                Branch branch = db.Branches.Where(x => x.br_id == BrID).FirstOrDefault();
                string all = ddlDeductions.SelectedValue;
                IList<sp_TaxDedFromSalaryResult> sal;
                sal = tax.GetTaxFromsalary(monthID, JobTypeID, BranchID, all,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                viewer.LocalReport.ReportPath = "report/rdlc/rptTaxdedFromSalary.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);

                ReportParameter[] paramz = new ReportParameter[5];

                paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
                paramz[1] = new ReportParameter("Division", branch.br_nme);
                paramz[2] = new ReportParameter("mon", Convert.ToDateTime(mon).ToString("MMM-yyyy"));
                paramz[3] = new ReportParameter("job", jobty.JobTypeName);
                paramz[4] = new ReportParameter("dedName", dedName);

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(paramz);


                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
           


        }

    }
}
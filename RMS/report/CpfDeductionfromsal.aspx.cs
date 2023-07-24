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
    public partial class CpfDeductionfromsal : System.Web.UI.Page
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalCPFRpt").ToString();
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

                FillSearchBranchDropDown();
                GetMonth();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
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
                    if (BranchID == 14 || BranchID == 15)
                    {
                        string slectedMonth = MonthSelected.Text.Trim().ToLower().ToString();

                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonth) && x.BranchID == 1).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreatePDF("SalarySymmaryEmpReport", selectedMonth, selectedMonthID);

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

            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            // ReportViewer viewer =new ReportViewer();
                  string mon = MonthSelected.Text;
                int BrID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                Branch branch = db.Branches.Where(x => x.br_id == BrID).FirstOrDefault();

                IList<sp_CpfDedFromSalaryResult> cpf;
                cpf = tax.GetCPFFromsalary(BranchID, monthID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                viewer.LocalReport.ReportPath = "report/rdlc/rptCpfAndAdvance.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", cpf);

                ReportParameter[] paramz = new ReportParameter[3];

                paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
                paramz[1] = new ReportParameter("Division", branch.br_nme);
                paramz[2] = new ReportParameter("mon", Convert.ToDateTime(mon).ToString("MMM-yyyy"));

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(paramz);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
                
        }
    }
}
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
    public partial class SalarySummaryReport : System.Web.UI.Page
    {

        RMSDataContext db = new RMSDataContext();
        SlalaryPacakageBL rptBL = new SlalaryPacakageBL();
        public int CompId
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

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

        public int EmpID
        {
            get { return (ViewState["EmpID"] == null) ? 0 : Convert.ToInt32(ViewState["EmpID"]); }
            set { ViewState["EmpID"] = value; }
        }

        public int ddt
        {
            get { return (ViewState["ddt"] == null) ? 0 : Convert.ToInt32(ViewState["ddt"]); }
            set { ViewState["ddt"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "salarysummaryreport").ToString();

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

                if (Session["Divi"] == null)
                {
                    ddt = Convert.ToInt32(Request.Cookies["uzr"]["Division"]);
                }
                else
                {
                    ddt = Convert.ToInt32(Session["Divi"].ToString());
                }

                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                FillddlJobType();
                FillEmployeeDropDown();
                GetMonth();
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
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                }

            }
            catch
            { }
        }

        protected void ddlEmploye_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                EmpID = Convert.ToInt32(ddlemploye.SelectedValue);
            }
            catch (Exception)
            {

                throw;
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

                ddlJobType.Items.Insert(0, new ListItem("Select", "0"));
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        protected void FillEmployeeDropDown()
        {
            ddlemploye.DataTextField = "FullName";
            ddlemploye.DataValueField = "EmpID";
            using (RMSDataContext dataContext = new RMSDataContext())
            {

                ddlemploye.DataSource = dataContext.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID && x.BranchID != 14 && x.BranchID != null).ToList();
                ddlemploye.DataBind();
            }

            ddlemploye.Items.Insert(0, new ListItem("All", "0"));
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

        protected void CreatePDF(String FileName, string MonthVal, int monthID)
        {
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            // ReportViewer viewer =new ReportViewer();
            object sal;
            if (BranchID == 14 || BranchID == 15 || BranchID == 16)
            {
                if (BranchID == 16)
                {
                    int bra = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                    var bran = db.Branches.Where(x => x.br_id == bra).FirstOrDefault();
                    string name = bran.br_nme;
                    
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryForCul.rdlc";
                    ReportDataSource datasource = new ReportDataSource("DataSet1", sal);

                    ReportParameter[] paramz = new ReportParameter[4];
                    paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
                    if (Session["CompName"] == null)
                    {
                        paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
                    }
                    else
                    {
                        paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString());
                    }

                    paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(MonthVal).ToString("MMM-yyyy"));
                    //paramz[3] = new ReportParameter("OfficeOfficial", jobty);
                    paramz[3] = new ReportParameter("DivName", name);

                    viewer.LocalReport.EnableExternalImages = true;
                    viewer.LocalReport.Refresh();
                    viewer.LocalReport.SetParameters(paramz);


                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(datasource);
                }
                else
                {
                    int bra = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                    var bran = db.Branches.Where(x => x.br_id == bra).FirstOrDefault();
                    string name = bran.br_nme;


                    //int month = slectedmonthyear.Month;
                    //int year = slectedmonthyear.Year;
                    //string paypd = ddlPayPerd.SelectedItem.Text;
                    //string paypd = "";
                    //string yr = paypd.Substring(0, 4);
                    //string mn = paypd.Substring(4, 2);


                    //DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);


                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID,EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryForSkp.rdlc";
                    ReportDataSource datasource = new ReportDataSource("DataSet1", sal);

                    ReportParameter[] paramz = new ReportParameter[4];
                    paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
                    if (Session["CompName"] == null)
                    {
                        paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
                    }
                    else
                    {
                        paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString());
                    }

                    paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(MonthVal).ToString("MMM-yyyy"));
                    //paramz[3] = new ReportParameter("OfficeOfficial", jobty);
                    paramz[3] = new ReportParameter("DivName", name);

                    viewer.LocalReport.EnableExternalImages = true;
                    viewer.LocalReport.Refresh();
                    viewer.LocalReport.SetParameters(paramz);


                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(datasource);
                }
               
            }
            else
            {
                string jobty;
                if (ddlJobType.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Seletc Select Type", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    int jo = Convert.ToInt16(ddlJobType.SelectedValue);
                    var job = db.JobTypes.Where(x => x.JobTypeID == jo).FirstOrDefault();
                     jobty = job.JobTypeName;
                }
                

                int bra = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                var bran = db.Branches.Where(x => x.br_id == bra).FirstOrDefault();
                string name = bran.br_nme;
                ReportDataSource datasource = null;

                //int month = slectedmonthyear.Month;
                //int year = slectedmonthyear.Year;
                //string paypd = ddlPayPerd.SelectedItem.Text;
                //string paypd = "";
                //string yr = paypd.Substring(0, 4);
                //string mn = paypd.Substring(4, 2);


                //DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);
                if (BranchID == 1)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID,EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummary.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }
                else if (BranchID == 3)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDivision.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }
                else if (BranchID == 4)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDivision.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }
                else if (BranchID == 5)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDivision.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }
                else if (BranchID == 6)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDivision.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }
                else if (BranchID == 7)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDivision.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }
                else if (BranchID == 8)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID,EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDivision.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }
                else if (BranchID == 9)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDivision.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }
                else if (BranchID == 10)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDivision.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }

                else if (BranchID == 11)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDistric.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }
                else if (BranchID == 13)
                {
                    sal = rptBL.SalarySummaryReport(monthID, BranchID, JobTypeID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSummaryDistric.rdlc";
                    datasource = new ReportDataSource("DataSet1", sal);
                }



                ReportParameter[] paramz = new ReportParameter[5];
                paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
                if (Session["CompName"] == null)
                {
                    paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
                }
                else
                {
                    paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString());
                }

                paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(MonthVal).ToString("MMM-yyyy"));
                paramz[3] = new ReportParameter("OfficeOfficial", jobty);
                paramz[4] = new ReportParameter("DivName", name);

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(paramz);


                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            } 


            


            //Warning[] warnings = null;
            //String[] streamids = null;
            //string mimeType = null;
            //string encoding = null;
            ////The DeviceInfo settings should be changed based on the reportType
            ////http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            //string deviceInfo =
            //"<DeviceInfo>" +
            //"  <OutputFormat>" + extension + "</OutputFormat>" +
            //"  <PageWidth>8.27in</PageWidth>" +
            //"  <PageHeight>11in</PageHeight>" +
            //"  <MarginTop>0.3in</MarginTop>" +
            //"  <MarginLeft>0.3in</MarginLeft>" +
            //"  <MarginRight>0.3in</MarginRight>" +
            //"  <MarginBottom>0.3in</MarginBottom>" +
            //"</DeviceInfo>";

            //Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            ////Response.Buffer = true;
            //Response.ClearHeaders();
            //Response.Clear();
            //Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
            //Response.ContentType = mimeType;
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {

            //int iPeriod;
            //int empID = Convert.ToInt32(ddlEmployee.SelectedValue);
            //PayPerd = iPeriod;


            if (MonthSelected.Text.Trim() != null && MonthSelected.Text.Trim() != "")
            {
                try
                {
                    if (BranchID == 14 || BranchID == 16)
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
                            CreatePDF("SalarySymmaryEmpReport", selectedMonth, selectedMonthID);

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

        protected void GetMonth()
        {
            var month = db.TblSalaryMonths.Where(x => x.MonthIsActive == true && x.BranchID == BranchID).FirstOrDefault();
            this.MonthSelected.Text = month.MonthVal;
        }
    }
}
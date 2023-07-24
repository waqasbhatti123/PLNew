using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using RMS.BL;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace RMS.report.rdlc
{
    public partial class SalarySlipReport : System.Web.UI.Page
    {
        SlalaryPacakageBL rptBL = new SlalaryPacakageBL();
        RMSDataContext db = new RMSDataContext();
      

        #region Properties
        public int? EmpID
        {
            get { return (ViewState["EmpID"] == null) ? 0 : Convert.ToInt32(ViewState["EmpID"]); }
            set { ViewState["EmpID"] = value; }
        }

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

        public int ddt
        {
            get { return (ViewState["ddt"] == null) ? 0 : Convert.ToInt32(ViewState["ddt"]); }
            set { ViewState["ddt"] = value; }
        }

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }


        public bool IsSearch
        {
            get { return (ViewState["IsSearch"] == null) ? false : Convert.ToBoolean(ViewState["IsSearch"]); }
            set { ViewState["IsSearch"] = value; }
        }

        #endregion

        //private void FillDropDownPayPeriod()
        //{
        //    ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    this.ddlPayPerd.DataTextField = "PayPerd";
        //    ddlPayPerd.DataValueField = "PayPerd";
        //    ddlPayPerd.DataBind();
        //}


       
        //protected void Load_report()
        //{
        //    IQueryable<spRptEmployeeListResult> sal;
        //    sal = SalBl.rptEmpSlip(CompId, PayPerd, "","", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ReportViewer viewer = new ReportViewer();

        //    viewer.LocalReport.ReportPath = "report/rdlc/rptSalarySlip.rdlc";
        //    ReportDataSource datasource = new ReportDataSource("spRptEmployeeListResult", sal);
        //    viewer.LocalReport.DataSources.Clear();
        //    viewer.LocalReport.DataSources.Add(datasource);

        //    viewer.LocalReport.Refresh();
        //    //ReportViewer1 = viewer;
            
        //}


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpMonthSal").ToString();

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

                CompId = iCompid;
                EmpID = 0;
                FillddlJobType();
                FillDropDownEmployee();
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                GetMonth();
                FillSalHeaddropDown();
            }
        }


        protected void FillddlJobType()
        {
            try
            {
                ddlJobType.DataTextField = "JobTypeName1";
                ddlJobType.DataValueField = "JobNameID";
                using (RMSDataContext dataContext = new RMSDataContext())
                {

                    ddlJobType.DataSource = dataContext.JobTypeNames.Where(x => x.IsActive == true).ToList();
                    ddlJobType.DataBind();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }


        protected void ddlJobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlJobType.SelectedValue.Equals("0"))
                {
                    int selectedJobType = Convert.ToInt32(ddlJobType.SelectedValue);
                    RMSDataContext db = new RMSDataContext();

                    this.ddlEmployee.DataTextField = "FullName";
                    ddlEmployee.DataValueField = "EmpID";

                    ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID && x.JobNameID == selectedJobType).ToList();
                    ddlEmployee.DataBind();
                    ddlEmployee.Items.Insert(0, new ListItem("All", "0"));




                }

            }
            catch
            { }
        }

        private void FillDropDownEmployee()
        {
            RMSDataContext db = new RMSDataContext();

            this.ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataValueField = "EmpID";

            ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("All", "0"));



        }


        private void FillSalHeaddropDown()
        {
            //ddlSelectHead.DataTextField = "Name";
            //ddlSelectHead.DataValueField = "Name";
            //ddlSelectHead.DataSource = db.SalaryContents.Where(x => x.IsActive == true).ToList();
            //ddlSelectHead.DataBind();
            //ddlSelectHead.Items.Insert(0, new ListItem("Select Head", ""));
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
                    IsSearch = true;
                    FillDropDownEmployee();
                }

            }
            catch
            { }
        }



        //protected override void OnLoadComplete(EventArgs e)
        //{
        //    if (EmpSrchUC.EmpBindGrid.Equals("Yes"))
        //    {
        //        ClearFields();
        //        EmpID = EmpSrchUC.EmpIDUC;
        //    }
        //    base.OnLoadComplete(e);
        //}

        private void ClearFields()
        {
            //txtCode_From.Text = "";
            //txtCode_To.Text = "";
            //txtCode_From.Enabled = false;
            //txtCode_To.Enabled = false;
            EmpID = 0;
            ddlCarAlloted.SelectedValue = "";
            txtSalaryday.Text = "";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlEmployee.SelectedValue = "0";

            Response.Redirect("~/report/rptEmpSalSlip.aspx?PID=35");

        }

        protected void CreatePDF(String FileName,string slectedmonthyear, int monthID, int empID)
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);

            Branch brr = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            string job = ddlJobType.SelectedValue;
           // string income = txtIncomeTax.Text.Trim();
            string cars = ddlCarAlloted.SelectedValue;
            int emId = Convert.ToInt32(ddlEmployee.SelectedValue);
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            // ReportViewer viewer =new ReportViewer();\
            int JobTyp = Convert.ToInt32(ddlJobType.SelectedValue);
            IList<sp_SalarySlipForWResult> sal;
            IList<sp_GetOneClickSalarySlipResult> sal1;
            IList<sp_GetAllowanceResult> sal2;
            if (emId == 0)
            {
                sal1 = rptBL.SalarySlipSReportListAllowance(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                sal2 = rptBL.SalarySlipSReportListAllwo(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                viewer.LocalReport.ReportPath = "report/rdlc/rptListSalSlipPucar.rdlc";
                ReportDataSource datasource = new ReportDataSource("spBasic", sal1);
                ReportDataSource datasourceq = new ReportDataSource("spAllowance", sal2);

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

                paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));
                

                paramz[3] = new ReportParameter("Div", brr.br_nme);

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(paramz);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
                viewer.LocalReport.DataSources.Add(datasourceq);
            }
            else
            {
                if (BranchID == 14 || BranchID == 15)
                {
                    sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                    viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSlipForSkp.rdlc";
                    ReportDataSource datasource = new ReportDataSource("DataSet1", sal);

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

                    paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                    foreach (var item in sal)
                    {
                        if (item.EmpID == emId)
                        {
                            if (cars != null || cars != "")
                            {
                                paramz[3] = new ReportParameter("Cars", cars);
                            }
                            else
                            {
                                paramz[3] = null;
                            }
                        }
                    }

                    foreach (var item in sal)
                    {
                        if (item.EmpID == emId)
                        {
                            if (txtSalaryday.Text != "")
                            {
                                paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                            }
                            else
                            {
                                paramz[4] = new ReportParameter("income", "");
                            }
                        }
                    }
                    viewer.LocalReport.EnableExternalImages = true;
                    viewer.LocalReport.Refresh();
                    viewer.LocalReport.SetParameters(paramz);


                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(datasource);
                }
                else
                {

                    if (BranchID == 1)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSlip.rdlc";
                        ReportDataSource datasource = new ReportDataSource("DataSet1", sal);

                        ReportParameter[] paramz = new ReportParameter[6];
                        paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
                        if (Session["CompName"] == null)
                        {
                            paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
                        }
                        else
                        {
                            paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString());
                        }

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                        foreach (var item in sal)
                        {
                            if (item.EmpID == emId)
                            {
                                if (cars != null || cars != "")
                                {
                                    paramz[3] = new ReportParameter("Cars", cars);
                                }
                                else
                                {
                                    paramz[3] = null;
                                }
                            }
                        }

                        foreach (var item in sal)
                        {
                            if (item.EmpID == emId)
                            {
                                if (txtSalaryday.Text != "")
                                {
                                    paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                                }
                                else
                                {
                                    paramz[4] = new ReportParameter("income", "");
                                }
                            }
                        }

                        paramz[5] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 3)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlipDivision.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (cars != null || cars != "")
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", cars);
                        //        }
                        //        else
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", "");
                        //        }
                        //    }
                        //}

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (txtSalaryday.Text != "")
                        //        {
                        //            paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                        //        }
                        //        else
                        //        {
                        //            paramz[4] = new ReportParameter("income", "");
                        //        }
                        //    }
                        //}

                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 4)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlipDivision.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (cars != null || cars != "")
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", cars);
                        //        }
                        //        else
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", "");
                        //        }
                        //    }
                        //}

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (txtSalaryday.Text != "")
                        //        {
                        //            paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                        //        }
                        //        else
                        //        {
                        //            paramz[4] = new ReportParameter("income", "");
                        //        }
                        //    }
                        //}

                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 5)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlipDivision.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (cars != null || cars != "")
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", cars);
                        //        }
                        //        else
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", "");
                        //        }
                        //    }
                        //}

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (txtSalaryday.Text != "")
                        //        {
                        //            paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                        //        }
                        //        else
                        //        {
                        //            paramz[4] = new ReportParameter("income", "");
                        //        }
                        //    }
                        //}

                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 6)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlipDivision.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (cars != null || cars != "")
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", cars);
                        //        }
                        //        else
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", "");
                        //        }
                        //    }
                        //}

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (txtSalaryday.Text != "")
                        //        {
                        //            paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                        //        }
                        //        else
                        //        {
                        //            paramz[4] = new ReportParameter("income", "");
                        //        }
                        //    }
                        //}

                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 7)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlipDivision.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (cars != null || cars != "")
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", cars);
                        //        }
                        //        else
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", "");
                        //        }
                        //    }
                        //}

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (txtSalaryday.Text != "")
                        //        {
                        //            paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                        //        }
                        //        else
                        //        {
                        //            paramz[4] = new ReportParameter("income", "");
                        //        }
                        //    }
                        //}

                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 8)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlipDivision.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (cars != null || cars != "")
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", cars);
                        //        }
                        //        else
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", "");
                        //        }
                        //    }
                        //}

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (txtSalaryday.Text != "")
                        //        {
                        //            paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                        //        }
                        //        else
                        //        {
                        //            paramz[4] = new ReportParameter("income", "");
                        //        }
                        //    }
                        //}

                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 9)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlipDivision.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (cars != null || cars != "")
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", cars);
                        //        }
                        //        else
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", "");
                        //        }
                        //    }
                        //}

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (txtSalaryday.Text != "")
                        //        {
                        //            paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                        //        }
                        //        else
                        //        {
                        //            paramz[4] = new ReportParameter("income", "");
                        //        }
                        //    }
                        //}

                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 10)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlipDivision.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (cars != null || cars != "")
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", cars);
                        //        }
                        //        else
                        //        {
                        //            paramz[3] = new ReportParameter("Cars", "");
                        //        }
                        //    }
                        //}

                        //foreach (var item in sal)
                        //{
                        //    if (item.EmpID == emId)
                        //    {
                        //        if (txtSalaryday.Text != "")
                        //        {
                        //            paramz[4] = new ReportParameter("income", txtSalaryday.Text);
                        //        }
                        //        else
                        //        {
                        //            paramz[4] = new ReportParameter("income", "");
                        //        }
                        //    }
                        //}

                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 11)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlFordistrict.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));



                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 12)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlFordistrict.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));


                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else if (BranchID == 13)
                    {
                        sal = rptBL.SalarySlipSReport(monthID, empID, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSlFordistrict.rdlc";
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

                        paramz[2] = new ReportParameter("SalaryMonth", Convert.ToDateTime(slectedmonthyear).ToString("MMM-yyyy"));


                        paramz[3] = new ReportParameter("Div", brr.br_nme);

                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }

                }

            }

        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {

                //int iPeriod;
                int empID = Convert.ToInt32(ddlEmployee.SelectedValue);
            //PayPerd = iPeriod;
            if(MonthSelected.Text.Trim() != null && MonthSelected.Text.Trim() != "")
            {
                try
                {
                    string slectedMonthYear = MonthSelected.Text.Trim().ToLower().ToString();

                    if ( BranchID == 14)
                    {
                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonthYear) && x.BranchID == 1).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreatePDF("SalarySlip", selectedMonth, selectedMonthID, empID);
                            ClearFields();
                        }
                        else
                        {
                            ucMessage.ShowMessage("Salary transfer Month is not exist", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                    else
                    {
                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonthYear) && x.BranchID == BranchID).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreatePDF("SalarySlip", selectedMonth, selectedMonthID, empID);
                            ClearFields();
                        }
                        else
                        {
                            ucMessage.ShowMessage("Salary transfer Month is not exist", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                    

                    
                }
                catch(Exception ex)
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

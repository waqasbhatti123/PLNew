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
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

namespace RMS.report.rdlc
{
    public partial class SalaryReconReport2 : System.Web.UI.Page
    {
        //BL.PlReportBL rptBL = new RMS.BL.PlReportBL();

        SlalaryPacakageBL rptBL = new SlalaryPacakageBL();
        RMSDataContext db = new RMSDataContext();

        #region Properties
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
        #endregion

        //private void FillDropDownPayPeriod()
        //{
        //    BL.SalaryBL SalBl = new RMS.BL.SalaryBL();

        //    this.ddlPayPerd.DataTextField = "PayPerd";
        //    ddlPayPerd.DataValueField = "PayPerd";
        //    ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlPayPerd.DataBind();
        //}

        //protected void Load_report()
        //{
        //    object sal;
        //    sal = rptBL.rptSalaryRecon(CompId, PayPerd, 0,"", "", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ReportViewer viewer = new ReportViewer();

        //    viewer.LocalReport.ReportPath = "report/rdlc/rptSalaryRecon2.rdlc";
        //    ReportDataSource datasource = new ReportDataSource("spSalaryReconTORResult", sal);
        //    viewer.LocalReport.DataSources.Clear();
        //    viewer.LocalReport.DataSources.Add(datasource);

        //    viewer.LocalReport.Refresh();
        //    //ReportViewer1 = viewer;

        //}
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalReconRpt2").ToString();

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
                GetMonth();
                // FillDropDownPayPeriod();
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
                ddlJobType.Items.Insert(0, new ListItem("All", "0"));
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
                    JobTypeID = Convert.ToInt32(ddlJobType.SelectedValue);
            }
            catch
            { }
        }


        protected void CreatePDF(String FileName, string slectedmonthyear, int monthID)
        {

            Branch brr = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            ReportDataSource datasource;
            ReportParameter[] paramz;
            object sal;
            if (JobTypeID == 0)
            {
                if (BranchID == 15 || BranchID == 14 || BranchID == 16)
                {
                    if (BranchID == 16)
                    {
                        sal = rptBL.SalaryPackageForLAC(BranchID, JobTypeID, 0, monthID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/SalarySheetForLAC.rdlc";
                        datasource = new ReportDataSource("DataSet1", sal);

                        paramz = new ReportParameter[2];
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


                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetforSkp.rdlc";
                        datasource = new ReportDataSource("DataSet1", sal);

                        paramz = new ReportParameter[2];
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


                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                }
                else
                {
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

            }
            else
            {
                if (BranchID == 15 || BranchID == 14 || BranchID == 16)
                {
                    if (BranchID == 16)
                    {
                        sal = rptBL.SalaryPackageForLAC(BranchID, JobTypeID, 0, monthID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/SalarySheetForLAC.rdlc";
                        datasource = new ReportDataSource("DataSet1", sal);

                        paramz = new ReportParameter[2];
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


                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                    else
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetforSkp.rdlc";
                        datasource = new ReportDataSource("DataSet1", sal);

                        paramz = new ReportParameter[2];
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


                        viewer.LocalReport.EnableExternalImages = true;
                        viewer.LocalReport.Refresh();
                        viewer.LocalReport.SetParameters(paramz);


                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(datasource);
                    }
                }
                else
                {

                    if (BranchID == 1)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheet.rdlc";
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
                    else if (BranchID == 3)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetForDivision.rdlc";
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
                    else if (BranchID == 4)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetForDivision.rdlc";
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
                    else if (BranchID == 5)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetForDivision.rdlc";
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
                    else if (BranchID == 6)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetForDivision.rdlc";
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
                    else if (BranchID == 7)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetForDivision.rdlc";
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
                    else if (BranchID == 8)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetForDivision.rdlc";
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
                    else if (BranchID == 9)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetForDivision.rdlc";
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
                    else if (BranchID == 10)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheetForDivision.rdlc";
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
                    else if (BranchID == 11)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSDistrict.rdlc";
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
                    else if (BranchID == 13)
                    {
                        sal = rptBL.SalaryPackageReport(monthID, 0, BranchID, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSDistrict.rdlc";
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

                }


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
                            CreatePDF("SalarySheet", selectedMonth, selectedMonthID);

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

                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonth) && x.BranchID == BranchID ).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreatePDF("SalarySheet", selectedMonth, selectedMonthID);

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

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
    public partial class EmployeeProfileDetailReport : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        EmpProfileBL pro = new EmpProfileBL();


        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

#pragma warning disable CS0114 // 'EmployeeProfileDetailReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmployeeProfileDetailReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                ID = Convert.ToInt32(Request.QueryString["ID"]);
                
            }
        }
        
        protected void GenReport_click(object sender, EventArgs e)
        {
            string profile = ddlProfile.SelectedValue;
            Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            if (profile == "0")
            {
                IList<ap_EmployeeBasicInfoResult> emp;
                emp = pro.getEmployeeBasicInfo(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                IList<sp_EmpEducationResult> edu;
                edu = pro.getEmpEdu(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                IList<sp_EmpPriorExperienceResult> exp;
                exp = pro.GetEmpPriorExp(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                IList<sp_EmpTenureResult> ten;
                ten = pro.GetEmpTenureExp(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                IList<sp_EmpAcrRecordResult> acr;
                acr = pro.GetEmpAcrRecord(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                IList<sp_EmployeeEnquiryResult> enq;
                enq = pro.GetEmpEnq(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                IList<sp_EmployeeLitigationResult> lit;
                lit = pro.GetEmpLitigation(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                IList<sp_EmpPermotionResult> per;
                per = pro.GetEmpPermotion(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rdlcEmpProfilecomp.rdlc";
                ReportDataSource source = new ReportDataSource("spEmpEducation", edu);
                ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
                ReportDataSource expsource = new ReportDataSource("spEmpPriorExp", exp);
                ReportDataSource tensource = new ReportDataSource("spEmpTenureExp", ten);
                ReportDataSource AcrSource = new ReportDataSource("spempAcr", acr);
                ReportDataSource enqsource = new ReportDataSource("spEmpEnquiry", enq);
                ReportDataSource litsource = new ReportDataSource("spEmpLitigation", lit);
                ReportDataSource persource = new ReportDataSource("spEmpPermotion", per);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(source);
                viewer.LocalReport.DataSources.Add(empsource);
                viewer.LocalReport.DataSources.Add(expsource);
                viewer.LocalReport.DataSources.Add(tensource);
                viewer.LocalReport.DataSources.Add(AcrSource);
                viewer.LocalReport.DataSources.Add(enqsource);
                viewer.LocalReport.DataSources.Add(litsource);
                viewer.LocalReport.DataSources.Add(persource);
            }
            if (profile == "1")
            {
                IList<ap_EmployeeBasicInfoResult> emp;
                emp = pro.getEmployeeBasicInfo(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                IList<sp_EmpEducationResult> edu;
                edu = pro.getEmpEdu(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpEducation.rdlc";
                ReportDataSource source = new ReportDataSource("rptEmpEducation", edu);
                ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(source);
                viewer.LocalReport.DataSources.Add(empsource);
            }
            if (profile == "2")
            {
                IList<ap_EmployeeBasicInfoResult> emp;
                emp = pro.getEmployeeBasicInfo(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                IList<sp_EmpPriorExperienceResult> exp;
                exp = pro.GetEmpPriorExp(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpPriorExp.rdlc";
                ReportDataSource source = new ReportDataSource("rptEmpPrior", exp);
                ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(source);
                viewer.LocalReport.DataSources.Add(empsource);
            }
            if (profile == "3")
            {
                IList<ap_EmployeeBasicInfoResult> emp;
                emp = pro.getEmployeeBasicInfo(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                IList<sp_EmpTenureResult> ten;
                ten = pro.GetEmpTenureExp(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpTenureExpe.rdlc";
                ReportDataSource source = new ReportDataSource("rptEmpTenure", ten);
                ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(source);
                viewer.LocalReport.DataSources.Add(empsource);
            }
            if (profile == "4")
            {
                IList<ap_EmployeeBasicInfoResult> emp;
                emp = pro.getEmployeeBasicInfo(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                IList<sp_EmpAcrRecordResult> acr;
                acr = pro.GetEmpAcrRecord(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpAcrRecord.rdlc";
                ReportDataSource source = new ReportDataSource("rptEmpAcr", acr);
                ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(source);
                viewer.LocalReport.DataSources.Add(empsource);
            }
            if (profile == "5")
            {
                IList<ap_EmployeeBasicInfoResult> emp;
                emp = pro.getEmployeeBasicInfo(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                IList<sp_EmployeeEnquiryResult> enq;
                enq = pro.GetEmpEnq(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpEnquiryReport.rdlc";
                ReportDataSource source = new ReportDataSource("rptEmpEnquiry", enq);
                ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(source);
                viewer.LocalReport.DataSources.Add(empsource);
            }
            if (profile == "6")
            {
                IList<ap_EmployeeBasicInfoResult> emp;
                emp = pro.getEmployeeBasicInfo(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                IList<sp_EmployeeLitigationResult> lit;
                lit = pro.GetEmpLitigation(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpLitigationReport.rdlc";
                ReportDataSource source = new ReportDataSource("rptEmpLitigation", lit);
                ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(source);
                viewer.LocalReport.DataSources.Add(empsource);
            }
            if (profile == "7")
            {
                IList<ap_EmployeeBasicInfoResult> emp;
                emp = pro.getEmployeeBasicInfo(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                IList<sp_EmpPermotionResult> per;
                per = pro.GetEmpPermotion(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpPermotion.rdlc";
                ReportDataSource source = new ReportDataSource("rptEmpPer", per);
                ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(source);
                viewer.LocalReport.DataSources.Add(empsource);
            }
            ReportParameter[] paramz = new ReportParameter[3];
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[1] = new ReportParameter("LogoPath", rptLogoPath);
            paramz[2] = new ReportParameter("Div", br.br_nme);
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);
        }
    }
}
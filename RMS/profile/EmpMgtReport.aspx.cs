using Microsoft.Reporting.WebForms;
using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.profile
{
    public partial class EmpMgtReport : System.Web.UI.Page
    {
        RMSDataContext Data = new RMSDataContext();
        EmpBL empManager = new EmpBL();
        EmpTransferBL empTBL = new EmpTransferBL();
        EmpProfRptBL empProfRptBL = new EmpProfRptBL();
        ListItem selList = new ListItem();
        ListItem selListSub = new ListItem();
        EmpProfileBL emppro = new EmpProfileBL();

#pragma warning disable CS0114 // 'EmpMgtReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpMgtReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompID"] == null)
                {
                    CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                }
                else
                {
                    CompID = Convert.ToByte(Session["CompID"].ToString());
                }
                ID = Convert.ToInt32(Request.QueryString["ID"]);
                PrintFunction(ID);
            }
        }


       protected void PrintFunction(int empid)
        {
            try
            {
                if (empid > 0)
                {


                    List<spEmpBasicInfoResult> result1 = emppro.GetEmpBasicInfo(empid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    List<spCurrentSalaryPackageResult> result2 = empProfRptBL.GetCurrentSalaryPackage(CompID, empid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    //ReportViewer reportViewer = new ReportViewer();
                    //reportViewer.Visible = false;
                    reportViewer.LocalReport.ReportPath = "report/rdlc/rptEmpBasicProfile.rdlc";
                    // reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    reportViewer.LocalReport.Refresh();
                    reportViewer.LocalReport.EnableExternalImages = true;
                    reportViewer.LocalReport.Refresh();

                    string passcoLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
                    string empImagePath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["EmpImage"].ToString().Trim());

                    ReportDataSource dataSource1 = new ReportDataSource("spEmpBasicInfoResult", result1);
                    ReportDataSource dataSource2 = new ReportDataSource("spCurrentSalaryPackageResult", result2);

                    ReportParameter[] rpt = new ReportParameter[4];
                    rpt[0] = new ReportParameter("LogoPath", passcoLogoPath);
                    rpt[1] = new ReportParameter("EmpImagePath", empImagePath + result1.Single().EmpPic);
                    rpt[2] = new ReportParameter("ReportName", "BRIEF EMPLOYEE REPORT");
                    if (Session["CompName"] == null)
                    {
                        rpt[3] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
                    }
                    else
                    {
                        rpt[3] = new ReportParameter("CompName", Session["CompName"].ToString());
                    }


                    reportViewer.LocalReport.SetParameters(rpt);

                    reportViewer.LocalReport.DataSources.Clear();
                    reportViewer.LocalReport.DataSources.Add(dataSource1);
                    reportViewer.LocalReport.DataSources.Add(dataSource2);

                    //Warning[] warnings;
                    //string[] streamids;
                    //string mimeType;
                    //string encoding;
                    //string extension;
                    //string filename;
                    //byte[] bytes = reportViewer.LocalReport.Render(
                    //   "PDF", null, out mimeType, out encoding,
                    //    out extension,
                    //   out streamids, out warnings);
                    //filename = string.Format("{0}.{1}", "Employee_Profile_Rpt_EmpID_" + result1.Single().EmpCode, "pdf");
                    //Response.ClearHeaders();
                    //Response.Clear();
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    //Response.ContentType = mimeType;
                    //Response.BinaryWrite(bytes);
                    //Response.Flush();
                    //Response.End();
                }
                else
                {
                    ucMessage.ShowMessage("Select an employee to print", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
    }
}
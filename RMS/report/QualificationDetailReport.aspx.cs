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
    public partial class QualificationDetailReport : System.Web.UI.Page
    {

        RMSDataContext db = new RMSDataContext();
        EmpProfileBL pro = new EmpProfileBL();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
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
                int ID = Convert.ToInt32(Request.QueryString["ID"]);
                PrintFunction(ID);
            }
        }

        protected void PrintFunction(int id)
        {
            Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            IList<sp_EmpEducationResult> edu;
            edu = pro.getEmpEdu(id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            IList<ap_EmployeeBasicInfoResult> info;
            info = pro.getEmployeeBasicInfo(id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpEducation.rdlc";
            ReportDataSource source = new ReportDataSource("rptEmpEducation", edu);
            ReportDataSource Empsource = new ReportDataSource("rptEmpBasic", info);

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
            paramz[2] = new ReportParameter("Div", br.br_nme.ToString());
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(source);
            viewer.LocalReport.DataSources.Add(Empsource);
           
        }
        
    }
}
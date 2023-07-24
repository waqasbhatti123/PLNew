﻿using Microsoft.Reporting.WebForms;
using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.report
{
    public partial class EmpPromotionDetailReport : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        EmpProfileBL pro = new EmpProfileBL();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
#pragma warning disable CS0114 // 'EmpPromotionDetailReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpPromotionDetailReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                    BranchID = Convert.ToInt32(Session["BranchID"]);
                }
                ID = Convert.ToInt32(Request.QueryString["ID"]);
                PrintFunction(ID);
            }
        }

        
        protected void PrintFunction(int empID)
        {
            Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            IList<ap_EmployeeBasicInfoResult> emp;
            emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            IList<sp_EmpPermotionResult> per;
            per = pro.GetEmpPermotion(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpPermotion.rdlc";
            ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            ReportDataSource perSource = new ReportDataSource("rptEmpPer", per);

            ReportParameter[] param = new ReportParameter[3];
            if (Session["CompName"] == null)
            {
                param[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                param[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            param[1] = new ReportParameter("LogoPath", rptLogoPath);
            param[2] = new ReportParameter("Div", br.br_nme);
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(param);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(empsource);
            viewer.LocalReport.DataSources.Add(perSource);
            ClearFields();
        }

        protected void ClearFields()
        {
            ID = 0;
        }
    }
}
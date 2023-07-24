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
    public partial class AttenReport : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        AttendanceBL att = new AttendanceBL();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "empattanreprt").ToString();
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
                FillJobTypeDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
            }
        }


        private void FillJobTypeDropDown()
        {
            this.ddlJobType.DataTextField = "JobTypeName1";
            this.ddlJobType.DataValueField = "JobNameID";
            this.ddlJobType.DataSource = db.JobTypeNames.Where(x => x.IsActive == true).ToList();
            this.ddlJobType.DataBind();
           // ddlJobType.Controls.Clear();
        }

        private void FillSearchBranchDropDown()
        {
            

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

        protected void Onclick_ReportGen(object sender, EventArgs e)
        {
            var month = txtMonth.Text.Trim();
            var branch = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            var type = Convert.ToInt32(ddlJobType.SelectedValue);
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            IQueryable<sp_AttendacePucarResult> lvz;
            lvz = att.Atten(month, type, branch, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "report/rdlc/AttenReport.rdlc";
            ReportDataSource datasource = new ReportDataSource("DataSet1", lvz);

            ReportParameter[] paramz = new ReportParameter[3];
            //if (Session["CompName"] == null)
            //{
            //    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            //}
            //else
            //{
            //    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            //}
            paramz[0] = new ReportParameter("ComName", rptLogoPath);
            paramz[1] = new ReportParameter("Month", month);
            paramz[2] = new ReportParameter("EmpType", ddlJobType.SelectedItem.Text.Trim());


          viewer.LocalReport.EnableExternalImages = true;
          viewer.LocalReport.Refresh();
          viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }
    }
}
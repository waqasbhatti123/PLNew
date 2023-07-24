using Microsoft.Reporting.WebForms;
using RMS.BL;
using RMS.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.report
{
    public partial class EmpPriorExpReport : System.Web.UI.Page
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "empPriorReport").ToString();
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
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            }
        }

        protected void grdExpEmps_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdExpEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdExperience.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdexp_rowbound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void PriorExpe_Click(object sender, EventArgs e)
        {
            BindGrid();            
        }
        
        protected void lnkPriorPrint_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            int cmd = Convert.ToInt32(link.CommandArgument);
            int _newtab = cmd;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
            string.Format("window.open('EmpPriorExpDetailReport.aspx?ID={0}');", _newtab), true);
            //int empID = Convert.ToInt32(link.CommandArgument);
            //Response.Redirect("EmpPriorExpDetailReport.aspx?ID=" + empID);
            //GenerateReport(empID,"rptEmpPriorExperience");
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
        public void GenerateReport(int empID,string Name)
        {
            //Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            
            //string rptLogoPath = "";
            //rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            //IList<ap_EmployeeBasicInfoResult> emp;
            //emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //IList<sp_EmpPriorExperienceResult> exp;
            //exp = pro.GetEmpPriorExp(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpPriorExp.rdlc";
            //ReportDataSource source = new ReportDataSource("rptEmpPrior", exp);
            //ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            //ReportParameter[] paramz = new ReportParameter[3];
            //if (Session["CompName"] == null)
            //{
            //    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            //}
            //else
            //{
            //    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            //}
            //paramz[1] = new ReportParameter("LogoPath", rptLogoPath);
            //paramz[2] = new ReportParameter("Div", br.br_nme);
            //viewer.LocalReport.EnableExternalImages = true;
            //viewer.LocalReport.Refresh();
            //viewer.LocalReport.SetParameters(paramz);
            //viewer.LocalReport.DataSources.Clear();
            //viewer.LocalReport.DataSources.Add(source);
            //viewer.LocalReport.DataSources.Add(empsource);
        }

        protected void BindGrid()
        {
            int type = Convert.ToInt32(ddlReportType.SelectedValue);
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            int fromm = Convert.ToInt32(ddlfromYear.SelectedValue);
            int to = Convert.ToInt32(ddlToYear.SelectedValue);
            if (type == 0)
            {
                List<sp_PriorExperieceYearPlusAllEmpResult> pro = db.sp_PriorExperieceYearPlusAllEmp(fromm, to, br).ToList();
                var list = new List<PriorExpeModel>();
                foreach (var item in pro)
                {
                    var pr = new PriorExpeModel();
                    pr.ID = item.EmpID;
                    pr.Name = item.FullName;
                    pr.Yoe = item.YearofExpe;
                    list.Add(pr);
                }
                grdExperience.DataSource = list.ToList();
            }
            else if(type == 1)
            {
                List<sp_PriorExperieceYearPlusResult> pro = db.sp_PriorExperieceYearPlus(fromm, to, br).ToList();
                var list = new List<PriorExpeModel>();
                foreach (var item in pro)
                {
                    var pr = new PriorExpeModel();
                    pr.ID = item.EmpID;
                    pr.Name = item.FullName;
                    pr.Yoe = item.YearofExpe;
                    list.Add(pr);
                }
                grdExperience.DataSource = list.ToList();
            }
            else
            {
                List<sp_PriorExperieceYearPlusRElievedResult> pro = db.sp_PriorExperieceYearPlusRElieved(fromm, to, br).ToList();
                var list = new List<PriorExpeModel>();
                foreach (var item in pro)
                {
                    var pr = new PriorExpeModel();
                    pr.ID = item.EmpID;
                    pr.Name = item.FullName;
                    pr.Yoe = item.YearofExpe;
                    list.Add(pr);
                }
                grdExperience.DataSource = list.ToList();
            }
            
            grdExperience.DataBind();
        }
        protected void BindGridRelieved()
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            int fromm = Convert.ToInt32(ddlfromYear.SelectedValue);
            int to = Convert.ToInt32(ddlToYear.SelectedValue);
            List<sp_PriorExperieceYearPlusRElievedResult> pro = db.sp_PriorExperieceYearPlusRElieved(fromm, to, br).ToList();
            var list = new List<PriorExpeModel>();
            foreach (var item in pro)
            {
                var pr = new PriorExpeModel();
                pr.ID = item.EmpID;
                pr.Name = item.FullName;
                pr.Yoe = item.YearofExpe;
                list.Add(pr);
            }
            grdExperience.DataSource = list.ToList();
            grdExperience.DataBind();
        }
    }
}
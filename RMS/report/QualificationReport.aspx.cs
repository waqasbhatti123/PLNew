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
    public partial class QualificationReport : System.Web.UI.Page
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "empQuali").ToString();

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
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            }
        }

        protected void grdEduEmps_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void GrdReli_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void AllEmp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void AllEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AllEmp.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void AllEmp_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text == "True")
                {
                    e.Row.Cells[5].Text = "Yes";
                }
                else
                {
                    e.Row.Cells[5].Text = "No";
                }
            }
        }
        protected void GrdReli_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdReli.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void GrdReli_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text == "True")
                {
                    e.Row.Cells[5].Text = "Yes";
                }
                else
                {
                    e.Row.Cells[5].Text = "No";
                }
            }
        }
        protected void grdEduEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEducation.PageIndex = e.NewPageIndex;
            BindGrid();
            //BBindGridRelived();
        }
        protected void grdEdu_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text == "True")
                {
                    e.Row.Cells[5].Text = "Yes";
                }
                else
                {
                    e.Row.Cells[5].Text = "No";
                }
            }
        }
        protected void eduSearch_Click(object sender, EventArgs e)
        {
            int type = Convert.ToInt32(ddlReportType.SelectedValue);
            if (type == 0)
            {
                BindGrid();
                AllEmp.Visible = true;
                GrdReli.Visible = false;
                grdEducation.Visible = false;
            }
            else if(type == 1)
            {
                BindGrid();
                grdEducation.Visible = true;
                AllEmp.Visible = false;
                GrdReli.Visible = false;
            }
            else
            {
                BindGrid();
                GrdReli.Visible = true;
                grdEducation.Visible = false;
                AllEmp.Visible = false;
            }
            
            
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            int cmd = Convert.ToInt32(link.CommandArgument);
            int _newtab = cmd;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
            string.Format("window.open('QualificationDetailReport.aspx?ID={0}');", _newtab), true);
            //Response.Redirect("QualificationDetailReport.aspx?ID=" + Convert.ToInt32(link.CommandArgument));
            //int empId = Convert.ToInt32(link.CommandArgument);
            //Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            //string rptLogoPath = "";
            //rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            //IList<sp_EmpEducationResult> edu;
            //edu = pro.getEmpEdu(empId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //IList<ap_EmployeeBasicInfoResult> info;
            //info = pro.getEmployeeBasicInfo(empId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpEducation.rdlc";
            //ReportDataSource source = new ReportDataSource("rptEmpEducation", edu);
            //ReportDataSource Empsource = new ReportDataSource("rptEmpBasic", info);

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
            //paramz[2] = new ReportParameter("Div", br.br_nme.ToString());
            //viewer.LocalReport.EnableExternalImages = true;
            //viewer.LocalReport.Refresh();
            //viewer.LocalReport.SetParameters(paramz);
            //viewer.LocalReport.DataSources.Clear();
            //viewer.LocalReport.DataSources.Add(source);
            //viewer.LocalReport.DataSources.Add(Empsource);
        }
        private void FillSearchBranchDropDown()
        {


            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            this.searchBranchDropDown.DataTextField = "br_nme";
            searchBranchDropDown.DataValueField = "br_id";
            //if (BranchID == 1)
            //{
            //    searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            //}
            //else
            //{
            //    searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true && x.br_id == BranchID).ToList();
            //}
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

        protected void BindGrid()
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            int type = Convert.ToInt32(ddlReportType.SelectedValue);
            if (type == 0)
            {
                List<tblPlEmpEdu> edu = new List<tblPlEmpEdu>();
                if (ddlDegreeType.SelectedValue != "0")
                {
                    AllEmp.DataSource = from ed in db.tblPlEmpEdus
                                              join emp in db.tblPlEmpDatas on ed.EmpID equals emp.EmpID
                                              join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                              where ed.Degreetype == ddlDegreeType.SelectedValue
                                              && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              select new
                                              {
                                                  emp.FullName,
                                                  des.CodeDesc,
                                                  ed.DegreeTitle,
                                                  ed.EmpEduID,
                                                  ed.EmpID,
                                                  ed.UniversityBoard,
                                                  ed.Year,
                                                  ed.Percente,
                                                  ed.Verified
                                              };
                    AllEmp.DataBind();
                }
            }
            else if(type == 1)
            {
                List<tblPlEmpEdu> edu = new List<tblPlEmpEdu>();
                if (ddlDegreeType.SelectedValue != "0")
                {
                    grdEducation.DataSource = from ed in db.tblPlEmpEdus
                                              join emp in db.tblPlEmpDatas on ed.EmpID equals emp.EmpID
                                              join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                              where ed.Degreetype == ddlDegreeType.SelectedValue
                                              && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              && emp.EmpStatus == 1
                                              select new
                                              {
                                                  emp.FullName,
                                                  des.CodeDesc,
                                                  ed.DegreeTitle,
                                                  ed.EmpEduID,
                                                  ed.EmpID,
                                                  ed.UniversityBoard,
                                                  ed.Year,
                                                  ed.Percente,
                                                  ed.Verified
                                              };
                    grdEducation.DataBind();
                }
            }
            else
            {
                List<tblPlEmpEdu> edu = new List<tblPlEmpEdu>();
                if (ddlDegreeType.SelectedValue != "0")
                {
                    GrdReli.DataSource = from ed in db.tblPlEmpEdus
                                              join emp in db.tblPlEmpDatas on ed.EmpID equals emp.EmpID
                                              join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                              where ed.Degreetype == ddlDegreeType.SelectedValue
                                              && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              && emp.EmpStatus != 1
                                              select new
                                              {
                                                  emp.FullName,
                                                  des.CodeDesc,
                                                  ed.DegreeTitle,
                                                  ed.EmpEduID,
                                                  ed.EmpID,
                                                  ed.UniversityBoard,
                                                  ed.Year,
                                                  ed.Percente,
                                                  ed.Verified
                                              };
                    GrdReli.DataBind();
                }
            }
           
           // grdEducation.DataBind();
        }

        protected void BindGridForAllEmp()
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            List<tblPlEmpEdu> edu = new List<tblPlEmpEdu>();
            if (ddlDegreeType.SelectedValue != "0")
            {
                grdEducation.DataSource = from ed in db.tblPlEmpEdus
                                          join emp in db.tblPlEmpDatas on ed.EmpID equals emp.EmpID
                                          join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                          where ed.Degreetype == ddlDegreeType.SelectedValue
                                          && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                          select new
                                          {
                                              emp.FullName,
                                              des.CodeDesc,
                                              ed.DegreeTitle,
                                              ed.EmpEduID,
                                              ed.EmpID,
                                              ed.UniversityBoard,
                                              ed.Year,
                                              ed.Percente,
                                              ed.Verified
                                          };
            }
            grdEducation.DataBind();
        }
        protected void BindGridRelived()
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            List<tblPlEmpEdu> edu = new List<tblPlEmpEdu>();
            if (ddlDegreeType.SelectedValue != "0")
            {
                grdEducation.DataSource = from ed in db.tblPlEmpEdus
                                          join emp in db.tblPlEmpDatas on ed.EmpID equals emp.EmpID
                                          join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                          where ed.Degreetype == ddlDegreeType.SelectedValue
                                          && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                          && emp.EmpStatus != 1
                                          select new
                                          {
                                              emp.FullName,
                                              des.CodeDesc,
                                              ed.DegreeTitle,
                                              ed.EmpEduID,
                                              ed.EmpID,
                                              ed.UniversityBoard,
                                              ed.Year,
                                              ed.Percente,
                                              ed.Verified
                                          };
            }
            grdEducation.DataBind();
        }
    }
}
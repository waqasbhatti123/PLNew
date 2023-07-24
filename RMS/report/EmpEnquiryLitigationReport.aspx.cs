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
    public partial class EmpEnquiryLitigationReport : System.Web.UI.Page
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "empLitiEenqTitle").ToString();
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



        protected void Types_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddltypes.SelectedValue == "0")
            {
                ddlenquiryAud.Items.Clear();
                ddlenquiryAud.Items.Insert(0, new ListItem("Select Type", "0"));
                ddlenquiryAud.DataTextField = "";
                ddlenquiryAud.DataValueField = "";
                ddlenquiryAud.DataBind();
                grdLiti.Visible = false;
                grdLitiEnq.Visible = false;
            }
            else if (ddltypes.SelectedValue == "1")
            {
                ddlenquiryAud.Items.Clear();
                ddlenquiryAud.Items.Insert(0, new ListItem("Select Type", "0"));
                ddlenquiryAud.DataTextField = "EnquiryName";
                ddlenquiryAud.DataValueField = "EnquiryName";
                ddlenquiryAud.DataSource = db.tblPlEmpEnquiryTypes.ToList();
                ddlenquiryAud.DataBind();
                grdLiti.Visible = false;
                grdLitiEnq.Visible = true;

            }
            else
            {
                ddlenquiryAud.Items.Clear();
                ddlenquiryAud.Items.Insert(0, new ListItem("Select Type", "0"));
                ddlenquiryAud.DataTextField = "LitiName";
                ddlenquiryAud.DataValueField = "LitiID";
                ddlenquiryAud.DataSource = db.tblPlLitis.ToList();
                ddlenquiryAud.DataBind();
                grdLitiEnq.Visible = false;
                grdLiti.Visible = true;
            }
        }

        protected void grdLitiEnq_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        protected void grdLitiEnq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLitiEnq.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdLitiEnq_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void grdLiti_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void grdLiti_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLiti.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdLiti_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void EnqLitiSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
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
        protected void lnkEnqPrint_Click(object sender, EventArgs e)
        {
           

            LinkButton link = (LinkButton)sender;
            int cmd = Convert.ToInt32(link.CommandArgument);
            int _newtab = cmd;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
            string.Format("window.open('EmpEnqLitiDetailReport.aspx?ID={0}');", _newtab), true);
            //int empID = Convert.ToInt32(link.CommandArgument);
            //Response.Redirect("EmpEnqLitiDetailReport.aspx?ID=" + empID);
            //Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            //string rptLogoPath = "";
            //rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            //IList<ap_EmployeeBasicInfoResult> emp;
            //emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //IList<sp_EmployeeEnquiryResult> enq;
            //enq = pro.GetEmpEnq(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpEnquiryReport.rdlc";

            //ReportDataSource enqsource = new ReportDataSource("rptEmpEnquiry", enq);
            //ReportDataSource empSource = new ReportDataSource("rptEmpBasic", emp);

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
            //viewer.LocalReport.DataSources.Add(enqsource);
            //viewer.LocalReport.DataSources.Add(empSource);
        }

        protected void lnkLitiPrint_Click(object sender, EventArgs e)
        {
            

            LinkButton link = (LinkButton)sender;
            int cmd = Convert.ToInt32(link.CommandArgument);
            int _newtab = cmd;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
            string.Format("window.open('EmpLitigationDetailReport.aspx?ID={0}');", _newtab), true);
            //int empID = Convert.ToInt32(link.CommandArgument);
            //Response.Redirect("EmpLitigationDetailReport.aspx?ID=" + empID);
            //Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            //string rptLogoPath = "";
            //rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            //IList<ap_EmployeeBasicInfoResult> emp;
            //emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //IList<sp_EmployeeLitigationResult> lit;
            //lit = pro.GetEmpLitigation(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpLitigationReport.rdlc";

            //ReportDataSource litsource = new ReportDataSource("rptEmpLitigation", lit);
            //ReportDataSource empSource = new ReportDataSource("rptEmpBasic", emp);

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
            //viewer.LocalReport.DataSources.Add(litsource);
            //viewer.LocalReport.DataSources.Add(empSource);
        }

        protected void BindGrid()
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
           
            if (ddltypes.SelectedValue == "1")
            {
                if (ddlenquiryAud.SelectedValue != "0" && ddlStatus.SelectedValue != "0")
                {
                    grdLitiEnq.DataSource = from enq in db.tblPlEmpEnqs
                                            join emp in db.tblPlEmpDatas on enq.EmpID equals emp.EmpID
                                            join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                            where enq.EnquiryAud == ddlenquiryAud.SelectedValue &&
                                            enq.Statuss == ddlStatus.SelectedValue && 
                                            emp.BranchID == br
                                            select new
                                            {
                                                emp.FullName,
                                                des.CodeDesc,
                                                enq.EnqTitle,
                                                enq.EnquiryDate,
                                                enq.Statuss,
                                                enq.EmpAcrID,
                                                enq.EmpID,
                                                enq.EnquiryAud
                                            };
                   
                }
                if (ddlenquiryAud.SelectedValue != "0" && ddlStatus.SelectedValue == "0")
                {
                    grdLitiEnq.DataSource = from enq in db.tblPlEmpEnqs
                                            join emp in db.tblPlEmpDatas on enq.EmpID equals emp.EmpID
                                            join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                            where enq.EnquiryAud == ddlenquiryAud.SelectedValue &&
                                            emp.BranchID == br
                                            select new
                                            {
                                                emp.FullName,
                                                des.CodeDesc,
                                                enq.EnqTitle,
                                                enq.EnquiryDate,
                                                enq.Statuss,
                                                enq.EmpAcrID,
                                                enq.EmpID,
                                                enq.EnquiryAud
                                            };
                }
                if (ddlenquiryAud.SelectedValue == "0" && ddlStatus.SelectedValue != "0")
                {
                    grdLitiEnq.DataSource = from enq in db.tblPlEmpEnqs
                                            join emp in db.tblPlEmpDatas on enq.EmpID equals emp.EmpID
                                            join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                            where enq.Statuss == ddlStatus.SelectedValue &&
                                            emp.BranchID == br
                                            select new
                                            {
                                                emp.FullName,
                                                des.CodeDesc,
                                                enq.EnqTitle,
                                                enq.EnquiryDate,
                                                enq.Statuss,
                                                enq.EmpAcrID,
                                                enq.EmpID,
                                                enq.EnquiryAud
                                            };
                }

                grdLitiEnq.DataBind();
            }
            else if (ddltypes.SelectedValue == "2")
            {
                if (ddlenquiryAud.SelectedValue != "0" && ddlStatus.SelectedValue != "0")
                {
                    grdLiti.DataSource = from lit in db.tblPlEmpLitigations
                                            join lity in  db.tblPlLitis on lit.LitiID equals lity.LitiID
                                            join emp in db.tblPlEmpDatas on lit.EmpID equals emp.EmpID
                                            join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                            where lit.LitiID == Convert.ToInt32(ddlenquiryAud.SelectedValue) &&
                                            lit.Status == ddlStatus.SelectedValue &&
                                            emp.BranchID == br
                                         select new
                                            {
                                                emp.FullName,
                                                des.CodeDesc,
                                                lit.LitiTitle,
                                                lit.LitiDate,
                                                lity.LitiName,
                                                lit.EmpAcrID,
                                                lit.EmpID,
                                                lit.Status
                                            };
                }
                if (ddlenquiryAud.SelectedValue != "0" && ddlStatus.SelectedValue == "0")
                {
                    grdLiti.DataSource = from lit in db.tblPlEmpLitigations
                                            join lity in db.tblPlLitis on lit.LitiID equals lity.LitiID
                                            join emp in db.tblPlEmpDatas on lit.EmpID equals emp.EmpID
                                            join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                            where lit.LitiID == Convert.ToInt32(ddlenquiryAud.SelectedValue)
                                             &&
                                            emp.BranchID == br
                                         select new
                                            {
                                                emp.FullName,
                                                des.CodeDesc,
                                                lit.LitiTitle,
                                                lit.LitiDate,
                                                lity.LitiName,
                                                lit.EmpAcrID,
                                                lit.EmpID,
                                                lit.Status
                                            };
                }
                if (ddlenquiryAud.SelectedValue == "0" && ddlStatus.SelectedValue != "0")
                {
                    grdLiti.DataSource = from lit in db.tblPlEmpLitigations
                                            join lity in db.tblPlLitis on lit.LitiID equals lity.LitiID
                                            join emp in db.tblPlEmpDatas on lit.EmpID equals emp.EmpID
                                            join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                            where lit.Status == ddlStatus.SelectedValue &&
                                            emp.BranchID == br
                                         select new
                                            {
                                                emp.FullName,
                                                des.CodeDesc,
                                                lit.LitiTitle,
                                                lit.LitiDate,
                                                lity.LitiName,
                                                lit.EmpAcrID,
                                                lit.EmpID,
                                                lit.Status
                                            };
                }

                grdLiti.DataBind();
            }

            
        }
    }
}
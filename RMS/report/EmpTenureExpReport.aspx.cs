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
    public partial class EmpTenureExpReport : System.Web.UI.Page
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpTenureReport").ToString();
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
                //FillDropdownScaleFrom();
                //FillDropdownScaleTo();
                FillDropDownTenurePostDsgn();
                FillDropdownEmployeee();
                FillDropdownPersonal();
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            }
        }

        protected void TenureGridShow_Click(object sender, EventArgs e)
        {
              BindGrid();
        }

        protected void grdTenEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        protected void grdTenEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gedTenure.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void grdten_rowbound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Cells[5].Text = Convert.ToDateTime(e.Row.Cells[5].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            //}
        }


        protected void ddlEmpDrpdown_change(object sender, EventArgs e)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
                int emppp = Convert.ToInt32(ddlEmpDrpdwn.SelectedValue);
                int empref = db.tblPlEmpDatas.Where(x => x.EmpID == emppp).FirstOrDefault().EmpID;
                ddlperson.SelectedValue = empref.ToString();
            }
        }

        protected void ddlPersonal_change(object sender, EventArgs e)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
                int emppp = Convert.ToInt32(ddlperson.SelectedValue);
                int empref = db.tblPlEmpDatas.Where(x => x.EmpID == emppp).FirstOrDefault().EmpID;
                ddlEmpDrpdwn.SelectedValue = empref.ToString();
            }
        }

        protected void lnkTenPrint_Click(object sender, EventArgs e)
        {
           
            LinkButton link = (LinkButton)sender;
            int cmd = Convert.ToInt32(link.CommandArgument);
            int _newtab = cmd;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
            string.Format("window.open('EmpTenureExpDetReport.aspx?ID={0}');", _newtab), true);
            //int empID = Convert.ToInt32(link.CommandArgument);
            // Response.Redirect("EmpTenureExpDetReport.aspx?ID=" + empID);
            //Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            //string rptLogoPath = "";
            //rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            //IList<ap_EmployeeBasicInfoResult> emp;
            //emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //IList<sp_EmpTenureResult> ten;
            //ten = pro.GetEmpTenureExp(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpTenureExpe.rdlc";

            //ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            //ReportDataSource TenSource = new ReportDataSource("rptEmpTenure", ten);

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
            //viewer.LocalReport.DataSources.Add(empsource);
            //viewer.LocalReport.DataSources.Add(TenSource);
        }

        //protected void FillDropdownScaleFrom()
        //{
        //    ddlTenureScale.DataTextField = "ScaleName";
        //    ddlTenureScale.DataValueField = "ScaleID";
        //    ddlTenureScale.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.Orderby);
        //    ddlTenureScale.DataBind();
        //}
        //protected void FillDropdownScaleTo()
        //{
        //    ddlTenureScaleTo.DataTextField = "ScaleName";
        //    ddlTenureScaleTo.DataValueField = "ScaleID";
        //    ddlTenureScaleTo.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.Orderby);
        //    ddlTenureScaleTo.DataBind();
        //}
        private void FillDropDownTenurePostDsgn()
        {
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            };
            this.ddlTenurePost.DataValueField = "CodeID";
            this.ddlTenurePost.DataTextField = "CodeDesc";
            this.ddlTenurePost.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 4 && x.CompID == _cmp && x.Enabled == true).ToList().OrderBy(x => x.sort);
            this.ddlTenurePost.DataBind();
        }

        protected void FillDropdownEmployeee()
        {
            RMSDataContext db = new RMSDataContext();
            ddlEmpDrpdwn.DataTextField = "FullName";
            ddlEmpDrpdwn.DataValueField = "EmpID";
            if (BranchID == 1)
            {
                ddlEmpDrpdwn.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID != 14 && x.BranchID != null).OrderBy(x => x.FullName).ToList();
            }
            else
            {
                ddlEmpDrpdwn.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
            }

            ddlEmpDrpdwn.DataBind();
            ddlEmpDrpdwn.Items.Insert(0, new ListItem("Select", "0"));
        }

        protected void FillDropdownPersonal()
        {
            RMSDataContext db = new RMSDataContext();
            ddlperson.DataTextField = "EmpCode";
            ddlperson.DataValueField = "EmpID";
            if (BranchID == 1)
            {
                ddlperson.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID != null && x.BranchID != 14).ToList();
            }
            else
            {
                ddlperson.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
            }

            ddlperson.DataBind();
            ddlperson.Items.Insert(0, new ListItem("Select", "0"));
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

        protected void BindGrid()
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            int postedas = Convert.ToInt32(ddlTenurePost.SelectedValue);
            //int fromscale = Convert.ToInt32(ddlTenureScale.SelectedValue);
            //int ToScale = Convert.ToInt32(ddlTenureScaleTo.SelectedValue);
            int type = Convert.ToInt32(ddlReportType.SelectedValue);
            int Emp = Convert.ToInt32(ddlEmpDrpdwn.SelectedValue);
            if (type == 0)
            {
                if (ddlTenurePost.SelectedValue != "0")
                {
                    gedTenure.DataSource = from  emp in db.tblPlEmpDatas
                                           join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                           join posting in db.Branches on emp.BranchID equals posting.br_id
                                           join sc in db.TblEmpScales on emp.ScaleID equals sc.ScaleID
                                           where emp.DesigID == postedas &&  emp.BranchID == (br == 0 ? emp.BranchID : br)
                                           select new
                                           {
                                               emp.EmpID,
                                               emp.EmpCode,
                                               emp.FullName,
                                               des.CodeDesc,
                                               posting.br_nme,
                                               sc.ScaleName,
                                               
                                           };

                }
                else
                {
                    gedTenure.DataSource = from emp in db.tblPlEmpDatas
                                           join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                           join posting in db.Branches on emp.BranchID equals posting.br_id
                                           join sc in db.TblEmpScales on emp.ScaleID equals sc.ScaleID
                                           where emp.EmpID == Emp && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                           select new
                                           {
                                               emp.EmpID,
                                               emp.EmpCode,
                                               emp.FullName,
                                               des.CodeDesc,
                                               posting.br_nme,
                                               sc.ScaleName,
                                           };
                }
                
            }
            else if (type == 1)
            {
                if (ddlTenurePost.SelectedValue != "0")
                {
                    gedTenure.DataSource = from emp in db.tblPlEmpDatas 
                                           join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                           join posting in db.Branches on emp.BranchID equals posting.br_id
                                           join sc in db.TblEmpScales on emp.ScaleID equals sc.ScaleID
                                           where emp.DesigID == postedas &&  emp.BranchID == (br == 0 ? emp.BranchID : br)
                                           && emp.EmpStatus == 1
                                           select new
                                           {
                                               emp.EmpID,
                                               emp.EmpCode,
                                               emp.FullName,
                                               des.CodeDesc,
                                               posting.br_nme,
                                               sc.ScaleName
                                           };

                }
                else
                {
                    gedTenure.DataSource = from emp in db.tblPlEmpDatas
                                           join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                           join posting in db.Branches on emp.BranchID equals posting.br_id
                                           join sc in db.TblEmpScales on emp.ScaleID equals sc.ScaleID
                                           where emp.EmpID == Emp && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                           && emp.EmpStatus == 1
                                           select new
                                           {
                                               emp.EmpID,
                                               emp.EmpCode,
                                               emp.FullName,
                                               des.CodeDesc,
                                               posting.br_nme,
                                               sc.ScaleName
                                           };
                }
            }
            else
            {
                if (ddlTenurePost.SelectedValue != "0")
                {
                    gedTenure.DataSource = from emp  in db.tblPlEmpDatas 
                                           join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                           join posting in db.Branches on emp.BranchID equals posting.br_id
                                           join sc in db.TblEmpScales on emp.ScaleID equals sc.ScaleID
                                           where emp.DesigID == postedas &&  emp.BranchID == (br == 0 ? emp.BranchID : br)
                                           && emp.EmpStatus != 1
                                           select new
                                           {

                                               emp.EmpID,
                                               emp.EmpCode,
                                               emp.FullName,
                                               des.CodeDesc,
                                               posting.br_nme,
                                               sc.ScaleName
                                           };

                }
                else
                {
                    gedTenure.DataSource = from  emp in db.tblPlEmpDatas
                                           join des in db.tblPlCodes on emp.DesigID equals des.CodeID
                                           join posting in db.Branches on emp.BranchID equals posting.br_id
                                           join sc in db.TblEmpScales on emp.ScaleID equals sc.ScaleID
                                           where emp.EmpID == Emp && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                           && emp.EmpStatus != 1
                                           select new
                                           {
                                               emp.EmpID,
                                               emp.EmpCode,
                                               emp.FullName,
                                               des.CodeDesc,
                                               posting.br_nme,
                                               sc.ScaleName
                                           };
                }
            }
            
            gedTenure.DataBind();
        }
        //protected void BindGridRelived()
        //{
        //    int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
        //    string postedas = ddlTenurePost.SelectedValue;
        //    int fromscale = Convert.ToInt32(ddlTenureScale.SelectedValue);
        //    int ToScale = Convert.ToInt32(ddlTenureScaleTo.SelectedValue);
        //    if (ddlTenurePost.SelectedValue != "0" && ddlTenureScale.SelectedValue != "0" && ddlTenureScaleTo.SelectedValue != "0")
        //    {
        //        gedTenure.DataSource = from ten in db.TenureExperiences
        //                               join emp in db.tblPlEmpDatas on ten.EmpID equals emp.EmpID
        //                               join des in db.tblPlCodes on emp.DesigID equals des.CodeID
        //                               join posting in db.Branches on ten.BranchID equals posting.br_id
        //                               join sc in db.TblEmpScales on ten.Scale equals sc.ScaleID
        //                               where ten.Postedas == postedas && (ten.Scale >= fromscale
        //                               && ten.Scale <= ToScale) && emp.BranchID == (br == 0 ? emp.BranchID : br)
        //                               && emp.EmpStatus != 1
        //                               select new
        //                               {
        //                                   emp.FullName,
        //                                   des.CodeDesc,
        //                                   posting.br_nme,
        //                                   ten.AddtionalCharge,
        //                                   sc.ScaleName,
        //                                   ten.joinDate,
        //                                   ten.EmpID,
        //                                   ten.TenID,
        //                                   ten.Postedas
        //                               };

        //    }
        //    if (ddlTenurePost.SelectedValue != "0" && ddlTenureScale.SelectedValue == "0" && ddlTenureScaleTo.SelectedValue == "0")
        //    {
        //        gedTenure.DataSource = from ten in db.TenureExperiences
        //                               join emp in db.tblPlEmpDatas on ten.EmpID equals emp.EmpID
        //                               join des in db.tblPlCodes on emp.DesigID equals des.CodeID
        //                               join posting in db.Branches on ten.BranchID equals posting.br_id
        //                               join sc in db.TblEmpScales on ten.Scale equals sc.ScaleID
        //                               where ten.Postedas == ddlTenurePost.SelectedValue && emp.BranchID == (br == 0 ? emp.BranchID : br)
        //                               && emp.EmpStatus != 1
        //                               select new
        //                               {
        //                                   emp.FullName,
        //                                   des.CodeDesc,
        //                                   posting.br_nme,
        //                                   ten.AddtionalCharge,
        //                                   sc.ScaleName,
        //                                   ten.joinDate,
        //                                   ten.EmpID,
        //                                   ten.TenID,
        //                                   ten.Postedas
        //                               };
        //    }
        //    if (ddlTenurePost.SelectedValue == "0" && ddlTenureScale.SelectedValue != "0" && ddlTenureScaleTo.SelectedValue != "0")
        //    {
        //        gedTenure.DataSource = from ten in db.TenureExperiences
        //                               join emp in db.tblPlEmpDatas on ten.EmpID equals emp.EmpID
        //                               join des in db.tblPlCodes on emp.DesigID equals des.CodeID
        //                               join posting in db.Branches on ten.BranchID equals posting.br_id
        //                               join sc in db.TblEmpScales on ten.Scale equals sc.ScaleID
        //                               where (ten.Scale >= fromscale && ten.Scale <= ToScale) && emp.BranchID == (br == 0 ? emp.BranchID : br)
        //                               && emp.EmpStatus != 1
        //                               select new
        //                               {
        //                                   emp.FullName,
        //                                   des.CodeDesc,
        //                                   posting.br_nme,
        //                                   ten.AddtionalCharge,
        //                                   sc.ScaleName,
        //                                   ten.joinDate,
        //                                   ten.EmpID,
        //                                   ten.TenID,
        //                                   ten.Postedas
        //                               };
        //    }
        //    if (ddlTenurePost.SelectedValue == "0" && ddlTenureScale.SelectedValue != "0" && ddlTenureScaleTo.SelectedValue == "0")
        //    {
        //        gedTenure.DataSource = from ten in db.TenureExperiences
        //                               join emp in db.tblPlEmpDatas on ten.EmpID equals emp.EmpID
        //                               join des in db.tblPlCodes on emp.DesigID equals des.CodeID
        //                               join posting in db.Branches on ten.BranchID equals posting.br_id
        //                               join sc in db.TblEmpScales on ten.Scale equals sc.ScaleID
        //                               where ten.Scale >= fromscale
        //                                && emp.BranchID == (br == 0 ? emp.BranchID : br) && emp.EmpStatus != 1
        //                               select new
        //                               {
        //                                   emp.FullName,
        //                                   des.CodeDesc,
        //                                   posting.br_nme,
        //                                   ten.AddtionalCharge,
        //                                   sc.ScaleName,
        //                                   ten.joinDate,
        //                                   ten.EmpID,
        //                                   ten.TenID,
        //                                   ten.Postedas
        //                               };
        //    }
        //    if (ddlTenurePost.SelectedValue == "0" && ddlTenureScale.SelectedValue == "0" && ddlTenureScaleTo.SelectedValue != "0")
        //    {
        //        gedTenure.DataSource = from ten in db.TenureExperiences
        //                               join emp in db.tblPlEmpDatas on ten.EmpID equals emp.EmpID
        //                               join des in db.tblPlCodes on emp.DesigID equals des.CodeID
        //                               join posting in db.Branches on ten.BranchID equals posting.br_id
        //                               join sc in db.TblEmpScales on ten.Scale equals sc.ScaleID
        //                               where ten.Scale >= ToScale
        //                                && emp.BranchID == (br == 0 ? emp.BranchID : br) && emp.EmpStatus != 1
        //                               select new
        //                               {
        //                                   emp.FullName,
        //                                   des.CodeDesc,
        //                                   posting.br_nme,
        //                                   ten.AddtionalCharge,
        //                                   sc.ScaleName,
        //                                   ten.joinDate,
        //                                   ten.EmpID,
        //                                   ten.TenID,
        //                                   ten.Postedas
        //                               };
        //    }
        //    gedTenure.DataBind();
        //}
    }
}
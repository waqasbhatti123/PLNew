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
    public partial class EmpPermotionReport : System.Web.UI.Page
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "empPermotionReport").ToString();
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
                if (Session["DateFormat"] == null)
                {
                    txtperfromCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtPerToCal.Format = Request.Cookies["uzr"]["DateFormat"];
                }
                else
                {
                    txtperfromCal.Format = Session["DateFormat"].ToString();
                    txtPerToCal.Format = Session["DateFormat"].ToString();
                }
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            }
        }


        protected void btnPermo_Click(object sender, EventArgs e)
        {
            BindGrid();   
        }

        protected void lnkPriorPrint_Click(object sender, EventArgs e)
        {
           
            LinkButton link = (LinkButton)sender;
            int cmd = Convert.ToInt32(link.CommandArgument);
            int _newtab = cmd;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
            string.Format("window.open('EmpPromotionDetailReport.aspx?ID={0}');", _newtab), true);
            //int empID = Convert.ToInt32(link.CommandArgument);
            //Response.Redirect("EmpPromotionDetailReport.aspx?ID=" + empID);
            //Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            //string rptLogoPath = "";
            //rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            //IList<ap_EmployeeBasicInfoResult> emp;
            //emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //IList<sp_EmpPermotionResult> per;
            //per = pro.GetEmpPermotion(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpPermotion.rdlc";
            //ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            //ReportDataSource perSource = new ReportDataSource("rptEmpPer", per);

            //ReportParameter[] param = new ReportParameter[3];
            //if (Session["CompName"] == null)
            //{
            //    param[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            //}
            //else
            //{
            //    param[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            //}
            //param[1] = new ReportParameter("LogoPath", rptLogoPath);
            //param[2] = new ReportParameter("Div", br.br_nme);
            //viewer.LocalReport.EnableExternalImages = true;
            //viewer.LocalReport.Refresh();
            //viewer.LocalReport.SetParameters(param);
            //viewer.LocalReport.DataSources.Clear();
            //viewer.LocalReport.DataSources.Add(empsource);
            //viewer.LocalReport.DataSources.Add(perSource);

        }

        protected void grdPerEmps_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void grdPerEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPermotion.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void grdPer_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
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
            int type = Convert.ToInt32(ddlReportType.SelectedValue);
            if (type == 0)
            {
                if (ddlpertypes.SelectedValue != "0" && txtperfrom.Text != "" && txtPerTo.Text != "")
                {
                    grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                              join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                              join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                              where per.pertype == ddlpertypes.SelectedValue &&
                                              per.FromDate >= Convert.ToDateTime(txtperfrom.Text) &&
                                              per.todate <= Convert.ToDateTime(txtPerTo.Text) &&
                                              emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              select new
                                              {
                                                  emp.FullName,
                                                  per.PerID,
                                                  per.empID,
                                                  per.pertype,
                                                  sc.ScaleName,
                                                  per.FromDate,
                                                  per.todate
                                              };
                }
                if (ddlpertypes.SelectedValue == "0" && txtperfrom.Text != "" && txtPerTo.Text != "")
                {
                    grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                              join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                              join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                              where per.FromDate >= Convert.ToDateTime(txtperfrom.Text) &&
                                              per.todate <= Convert.ToDateTime(txtPerTo.Text) &&
                                              emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              select new
                                              {
                                                  emp.FullName,
                                                  per.PerID,
                                                  per.empID,
                                                  per.pertype,
                                                  sc.ScaleName,
                                                  per.FromDate,
                                                  per.todate
                                              };
                }
                if (ddlpertypes.SelectedValue != "0" && txtperfrom.Text == "" && txtPerTo.Text == "")
                {
                    grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                              join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                              join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                              where per.pertype == ddlpertypes.SelectedValue &&
                                              emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              select new
                                              {
                                                  emp.FullName,
                                                  per.PerID,
                                                  per.empID,
                                                  per.pertype,
                                                  sc.ScaleName,
                                                  per.FromDate,
                                                  per.todate
                                              };
                }
            }
            else if(type == 1)
            {
                if (ddlpertypes.SelectedValue != "0" && txtperfrom.Text != "" && txtPerTo.Text != "")
                {
                    grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                              join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                              join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                              where per.pertype == ddlpertypes.SelectedValue &&
                                              per.FromDate >= Convert.ToDateTime(txtperfrom.Text) &&
                                              per.todate <= Convert.ToDateTime(txtPerTo.Text) &&
                                              emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              && emp.EmpStatus == 1
                                              select new
                                              {
                                                  emp.FullName,
                                                  per.PerID,
                                                  per.empID,
                                                  per.pertype,
                                                  sc.ScaleName,
                                                  per.FromDate,
                                                  per.todate
                                              };
                }
                if (ddlpertypes.SelectedValue == "0" && txtperfrom.Text != "" && txtPerTo.Text != "")
                {
                    grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                              join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                              join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                              where per.FromDate >= Convert.ToDateTime(txtperfrom.Text) &&
                                              per.todate <= Convert.ToDateTime(txtPerTo.Text) &&
                                              emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              && emp.EmpStatus == 1
                                              select new
                                              {
                                                  emp.FullName,
                                                  per.PerID,
                                                  per.empID,
                                                  per.pertype,
                                                  sc.ScaleName,
                                                  per.FromDate,
                                                  per.todate
                                              };
                }
                if (ddlpertypes.SelectedValue != "0" && txtperfrom.Text == "" && txtPerTo.Text == "")
                {
                    grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                              join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                              join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                              where per.pertype == ddlpertypes.SelectedValue &&
                                              emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              && emp.EmpStatus == 1
                                              select new
                                              {
                                                  emp.FullName,
                                                  per.PerID,
                                                  per.empID,
                                                  per.pertype,
                                                  sc.ScaleName,
                                                  per.FromDate,
                                                  per.todate
                                              };
                }
            }
            else
            {
                if (ddlpertypes.SelectedValue != "0" && txtperfrom.Text != "" && txtPerTo.Text != "")
                {
                    grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                              join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                              join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                              where per.pertype == ddlpertypes.SelectedValue &&
                                              per.FromDate >= Convert.ToDateTime(txtperfrom.Text) &&
                                              per.todate <= Convert.ToDateTime(txtPerTo.Text) &&
                                              emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              && emp.EmpStatus != 1
                                              select new
                                              {
                                                  emp.FullName,
                                                  per.PerID,
                                                  per.empID,
                                                  per.pertype,
                                                  sc.ScaleName,
                                                  per.FromDate,
                                                  per.todate
                                              };
                }
                if (ddlpertypes.SelectedValue == "0" && txtperfrom.Text != "" && txtPerTo.Text != "")
                {
                    grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                              join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                              join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                              where per.FromDate >= Convert.ToDateTime(txtperfrom.Text) &&
                                              per.todate <= Convert.ToDateTime(txtPerTo.Text) &&
                                              emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              && emp.EmpStatus != 1
                                              select new
                                              {
                                                  emp.FullName,
                                                  per.PerID,
                                                  per.empID,
                                                  per.pertype,
                                                  sc.ScaleName,
                                                  per.FromDate,
                                                  per.todate
                                              };
                }
                if (ddlpertypes.SelectedValue != "0" && txtperfrom.Text == "" && txtPerTo.Text == "")
                {
                    grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                              join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                              join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                              where per.pertype == ddlpertypes.SelectedValue &&
                                              emp.BranchID == (br == 0 ? emp.BranchID : br)
                                              && emp.EmpStatus != 1
                                              select new
                                              {
                                                  emp.FullName,
                                                  per.PerID,
                                                  per.empID,
                                                  per.pertype,
                                                  sc.ScaleName,
                                                  per.FromDate,
                                                  per.todate
                                              };
                }
            }
            

            grdPermotion.DataBind();
        }
        protected void BindGridRelieved()
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            if (ddlpertypes.SelectedValue != "0" && txtperfrom.Text != "" && txtPerTo.Text != "")
            {
                grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                          join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                          join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                          where per.pertype == ddlpertypes.SelectedValue &&
                                          per.FromDate >= Convert.ToDateTime(txtperfrom.Text) &&
                                          per.todate <= Convert.ToDateTime(txtPerTo.Text) &&
                                          emp.BranchID == (br == 0 ? emp.BranchID : br)
                                          && emp.EmpStatus != 1
                                          select new
                                          {
                                              emp.FullName,
                                              per.PerID,
                                              per.empID,
                                              per.pertype,
                                              sc.ScaleName,
                                              per.FromDate,
                                              per.todate
                                          };
            }
            if (ddlpertypes.SelectedValue == "0" && txtperfrom.Text != "" && txtPerTo.Text != "")
            {
                grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                          join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                          join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                          where per.FromDate >= Convert.ToDateTime(txtperfrom.Text) &&
                                          per.todate <= Convert.ToDateTime(txtPerTo.Text) &&
                                          emp.BranchID == (br == 0 ? emp.BranchID : br)
                                          && emp.EmpStatus != 1
                                          select new
                                          {
                                              emp.FullName,
                                              per.PerID,
                                              per.empID,
                                              per.pertype,
                                              sc.ScaleName,
                                              per.FromDate,
                                              per.todate
                                          };
            }
            if (ddlpertypes.SelectedValue != "0" && txtperfrom.Text == "" && txtPerTo.Text == "")
            {
                grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                          join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                          join sc in db.TblEmpScales on per.scale equals sc.ScaleID
                                          where per.pertype == ddlpertypes.SelectedValue &&
                                          emp.BranchID == (br == 0 ? emp.BranchID : br)
                                          && emp.EmpStatus != 1
                                          select new
                                          {
                                              emp.FullName,
                                              per.PerID,
                                              per.empID,
                                              per.pertype,
                                              sc.ScaleName,
                                              per.FromDate,
                                              per.todate
                                          };
            }

            grdPermotion.DataBind();
        }
    }
}
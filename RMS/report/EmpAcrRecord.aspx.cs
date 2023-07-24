using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.report
{
    public partial class EmpAcrRecord : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "empAcrReport").ToString();
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
                    
                    txtDateFromCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtDateToCal.Format = Request.Cookies["uzr"]["DateFormat"];

                }
                else
                {
                    txtDateFromCal.Format = Session["DateFormat"].ToString();
                    txtDateToCal.Format = Session["DateFormat"].ToString();
                }
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            }
        }
        protected void grdAcrEmps_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void grdAcrEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DateTime datefr = Convert.ToDateTime(txtDateFrom.Text);
            DateTime datet = Convert.ToDateTime(txtDateTo.Text);
            grdAcr.PageIndex = e.NewPageIndex;
            BindGrid(datefr, datet);
        }

        protected void grdAcr_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[5].Text = Convert.ToDateTime(e.Row.Cells[5].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                //e.Row.Cells[6].Text = Convert.ToDateTime(e.Row.Cells[6].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
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

        protected void Search_click(object sender, EventArgs e)
        {
            DateTime datefr = Convert.ToDateTime(txtDateFrom.Text);
            DateTime datet = Convert.ToDateTime(txtDateTo.Text);
            BindGrid(datefr, datet);
            
        }

        protected void lnkAcrPrint_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            int cmd = Convert.ToInt32(link.CommandArgument);
            int _newtab = cmd;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
            string.Format("window.open('EmpAcrRecordDetail.aspx?ID={0}');", _newtab), true);
            //int emp = Convert.ToInt32(link.CommandArgument);
            //Response.Redirect("EmpAcrRecordDetail.aspx?ID=" + emp);
        }

        protected void BindGrid(DateTime dat, DateTime dt)
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            int type = Convert.ToInt32(ddlReportType.SelectedValue);
            if (type == 0)
            {
                grdAcr.DataSource = from acr in db.tblPlEmpAcrs
                                    join emp in db.tblPlEmpDatas on acr.EmpID equals emp.EmpID
                                    join code in db.tblPlCodes on acr.CodeID equals code.CodeID
                                    where acr.DateFrom >= dat &&
                                    acr.DateTo <= dt && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                    select new
                                    {
                                        acr.EmpAcrID,
                                        acr.EmpID,
                                        emp.FullName,
                                        code.CodeDesc,
                                        acr.DateFrom,
                                        acr.DateTo,
                                        acr.ReportingOfficer,
                                        acr.RepOffDate
                                    };
            }
            else if(type == 1)
            {
                grdAcr.DataSource = from acr in db.tblPlEmpAcrs
                                    join emp in db.tblPlEmpDatas on acr.EmpID equals emp.EmpID
                                    join code in db.tblPlCodes on acr.CodeID equals code.CodeID
                                    where acr.DateFrom >= dat &&
                                    acr.DateTo <= dt && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                    && emp.EmpStatus == 1
                                    select new
                                    {
                                        acr.EmpAcrID,
                                        acr.EmpID,
                                        emp.FullName,
                                        code.CodeDesc,
                                        acr.DateFrom,
                                        acr.DateTo,
                                        acr.ReportingOfficer,
                                        acr.RepOffDate
                                    };
            }
            else
            {
                grdAcr.DataSource = from acr in db.tblPlEmpAcrs
                                    join emp in db.tblPlEmpDatas on acr.EmpID equals emp.EmpID
                                    join code in db.tblPlCodes on acr.CodeID equals code.CodeID
                                    where acr.DateFrom >= dat &&
                                    acr.DateTo <= dt && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                    && emp.EmpStatus != 1
                                    select new
                                    {
                                        acr.EmpAcrID,
                                        acr.EmpID,
                                        emp.FullName,
                                        code.CodeDesc,
                                        acr.DateFrom,
                                        acr.DateTo,
                                        acr.ReportingOfficer,
                                        acr.RepOffDate
                                    };
            }
            
            grdAcr.DataBind();
        }
        protected void BindGridRelived(DateTime dat, DateTime dt)
        {
            int br = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            grdAcr.DataSource = from acr in db.tblPlEmpAcrs
                                join emp in db.tblPlEmpDatas on acr.EmpID equals emp.EmpID
                                join code in db.tblPlCodes on acr.CodeID equals code.CodeID
                                where acr.DateFrom >= dat &&
                                acr.DateTo <= dt && emp.BranchID == (br == 0 ? emp.BranchID : br)
                                && emp.EmpStatus != 1
                                select new
                                {
                                    acr.EmpAcrID,
                                    acr.EmpID,
                                    emp.FullName,
                                    code.CodeDesc,
                                    acr.DateFrom,
                                    acr.DateTo,
                                    acr.ReportingOfficer,
                                    acr.RepOffDate
                                };
            grdAcr.DataBind();
        }

    }
}
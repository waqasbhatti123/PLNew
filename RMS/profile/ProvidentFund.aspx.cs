using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.profile
{
    public partial class ProvidentFund : System.Web.UI.Page
    {
        EmpBL empBL = new EmpBL();
        ProvidentBL proBL = new ProvidentBL();
        RMSDataContext db = new RMSDataContext();


#pragma warning disable CS0114 // 'ProvidentFund.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'ProvidentFund.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Providentfund").ToString();
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
                FillDropDownEmployeeSearch();
                FillDropDownYear();
            }

        }


        private void FillDropDownEmployeeSearch()
        {
            ddlEmployeeSearch.DataTextField = "FullName";
            ddlEmployeeSearch.DataValueField = "EmpID";
            ddlEmployeeSearch.DataSource = (from emp in db.tblPlEmpDatas
                                            where emp.EmpStatus == 1
                                            && emp.BranchID == BranchID
                                            select emp).ToList();
            ddlEmployeeSearch.DataBind();
        }

        private void FillDropDownYear()
        {
            ddlYear.DataTextField = "YearName";
            ddlYear.DataValueField = "YearName";
            ddlYear.DataSource = empBL.GetYearPro((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlYear.DataBind();
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static object BindGridJquery(int id, int emp)
        {
            ProvidentBL providentBL = new ProvidentBL();
           var pra = providentBL.GetByYear(id,emp,(RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return pra;
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static object BindGrid(int empId)
        {
            RMSDataContext Data = new RMSDataContext();
            int count = Data.tblPlProvidentFunds.Where(x => x.EmpID == empId).Count();
            return count;
        }
        //[WebMethod]
        //[System.Web.Script.Services.ScriptMethod]
        //public static void SaveBudget(tblPlProvidentFund pFund)
        //{
        //    ProvidentBL providentBL = new ProvidentBL();
        //    providentBL.SubmitPro(pFund, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);

        //}

        protected void Save_click(object sender, EventArgs e)
          {
            tblPlProvidentFund pfund = new tblPlProvidentFund();
            int emp = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);

            if (ID == 0)
            {
                if (ddlEmployeeSearch.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    pfund.EmpID = emp;
                }
                if (txtOpening.Text == "")
                {
                    ucMessage.ShowMessage("Please Insert Opening Balance", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    pfund.closeBlnc = Convert.ToDecimal(txtOpening.Text.Trim());
                    pfund.createdon = DateTime.Now;
                    pfund.createdBy = Session["LoginID"].ToString();
                    proBL.SubmitPro(pfund, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }

                ClearFields();
            }
           
          }

        public void ClearFields()
        {
            txtOpening.Text = "";
            ddlEmployeeSearch.SelectedValue = "0";
        }
    }
}
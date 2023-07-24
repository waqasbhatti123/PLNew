using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using Microsoft.Reporting.WebForms;
using System.Web.Services;

namespace RMS.GLSetup
{
    public partial class Budget : BasePage
    {
        BudgetBL budgetBL = new BudgetBL();
        voucherDetailBL objVoucher = new voucherDetailBL();

        public int BrId
        {
            get { return (ViewState["BrId"] == null) ? 0 : Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }
        public static decimal Financialyear
        {
            get; set;
        }

        /*1 for Proposed, 2 for Approved*/
        public static int BudgetTypeID
        {
            get;set;
        }

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

       

        #region event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"]);
            }

            PID = Convert.ToInt32(Request.QueryString["PID"]);
            if (PID == 371)
            {
                BudgetTypeID = 1;
            }
            
            Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (!IsPostBack)
            {
                
                if(BudgetTypeID == 1)
                {
                    Session["PageTitle"] = "Proposed Budget";
                }
               
                else { }
                    
                BindGrid();
                FillSearchBranchDropDown();
               // searchBranchDropDown.SelectedValue = BrId.ToString();

            }
        }


        protected void BtnSave_Click(object sender, EventArgs e)
        {
            
            //try
            //{
            //    Budget budget = new Budget();
            //    foreach (TextBox item in )
            //    {

            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            //    try
            //    {
            //        List<RMS.BL.Budget> budgets = new List<RMS.BL.Budget>();
            //        RMS.BL.Budget budget;

            //        for (int i = 0; i < grdBudget.Rows.Count; i++)
            //        {
            //            budget = new RMS.BL.Budget();
            //            budget.BudgetTypeID = BudgetTypeID;
            //            budget.GUID = null;
            //            budget.QuarterID = null;
            //            budget.GlYear = Financialyear;
            //            budget.Account = ((System.Web.UI.WebControls.Label)grdBudget.Rows[i].FindControl("txtcode")).Text.Trim();

            //            budget.Income = 0;
            //            try
            //            {
            //                budget.Income = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdBudget.Rows[i].FindControl("txtIncome")).Text.Trim());
            //            }
            //            catch { }

            //            budget.Grant = 0;
            //            try
            //            {
            //                budget.Grant = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdBudget.Rows[i].FindControl("txtGrant")).Text.Trim());
            //            }
            //            catch { }

            //            budget.Aid = 0;
            //            budget.IsActive = true;

            //            budgets.Add(budget);
            //        }

            //        budgetBL.Submit(Financialyear, BudgetTypeID, budgets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //        ucMessage.ShowMessage("Record saved successfully", BL.Enums.MessageType.Info);
            //    }
            //    catch(Exception ex)
            //    {
            //        ucMessage.ShowMessage(ex.Message, BL.Enums.MessageType.Error);
            //    }
        }

        #endregion

        //protected void btn_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("BudgetReport.aspx");
            
        //}

        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            //this.searchBranchDropDown.DataTextField = "br_nme";
            //this.searchBranchDropDown.DataValueField = "br_id";
            //this.searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_id == 15).FirstOrDefault();
            //this.searchBranchDropDown.DataBind();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BrId).FirstOrDefault();

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
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BrId).ToList();
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



        #region helpingmethods

        private void BindGrid()
        {
            //grdBudget.DataSource = budgetBL.GetBudget((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], BudgetTypeID, Financialyear);
            //grdBudget.DataBind();
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static object BindGridJquery(int brID)
        {
            return new BudgetBL().GetBudget((RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"], BudgetTypeID, Financialyear, brID);
        }


       

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string SaveBudget(List<RMS.BL.Budget> budget)
        {
            try
            {
                BudgetBL budgetBL = new BudgetBL();
                int brId = Convert.ToInt32(budget[0].br_id);
                string Chk = budgetBL.Submit(Financialyear, BudgetTypeID, brId, budget, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
                if (Chk == "OK")
                {
                    return "Data Save Successfully";
                }
                else
                {
                    return "Error" + Chk.ToString();
                }
            }
           catch(Exception ex)
            {
                return "Error: " + ex.Message.ToString();
            }

        }

        #endregion


    }
}

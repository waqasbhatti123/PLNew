using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace RMS.proc
{
    public partial class MonthEnd : BasePage
    {

        #region DataMembers

        PlUploadBL uploadmgr = new PlUploadBL();

        RMSDataContext db = new RMSDataContext();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'MonthEnd.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int? ID
#pragma warning restore CS0114 // 'MonthEnd.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        public int CompID
        {
            get {
                if (Session["CompID"] == null)
                {
                    try { return Convert.ToByte(Request.Cookies["uzr"]["CompID"]); }
                    catch { return 0; }
                }
                else
                {
                   return Convert.ToByte(Session["CompID"].ToString());
                }
                //return (Session["CompID"] == null) ? 0 : Convert.ToInt32(Session["CompID"]); 
            }
            set { Session["CompID"] = value; }
        }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "MonthEnd").ToString();
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                string salp = GetSalPeriod(BranchID);
                BindMonthGrd();

                if (salp.Equals(""))
                {
                    lblMonthSal.Visible = true;
                    lblMonthSal.Text = "There is not any active month yet ";

                }

                else
                {
                    lblMonthSal.Visible = true;

                    lblMonthSal.Text = "Currently Active Month is " + salp + ". Do you want to end it?";
                }
            }
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            BindMonthGrd();
            if (e.CommandName == "Cancel")

            {
                Clear();
                Response.Redirect("~/proc/monthend.aspx?PID=44");
            }

            else 
            {
                try
                {

                    //uploadmgr.StartSalCalc(CompID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    // uploadmgr.MonthEnd(CompID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    string currentSelectedMonth = txtSelectedMonth.Text.Trim().ToLower().ToString();
                    if (currentSelectedMonth == null || currentSelectedMonth == "")
                    {
                        ucMessage.ShowMessage("Please select month first.", RMS.BL.Enums.MessageType.Info);
                        return;
                    }
                    if(ID == 0)
                    {
                        List<TblSalaryMonth> monthAllList = db.TblSalaryMonths.Where(x => x.BranchID == BranchID).ToList();
                        foreach (var item in monthAllList)
                        {
                            string dbMonths = item.MonthVal.ToLower().ToString();
                            if (dbMonths.Equals(currentSelectedMonth))
                            {
                                ucMessage.ShowMessage("Month is already exist.", RMS.BL.Enums.MessageType.Info);
                                return;
                            }
                        }
                    }
                    else
                    {
                        //List<TblSalaryMonth> monthAllList = db.TblSalaryMonths.Where(x => x.MonthID != ID).ToList();
                        //foreach (var item in monthAllList)
                        //{
                        //    string dbMonths = item.MonthVal.ToLower().ToString();
                        //    if (dbMonths.Equals(currentSelectedMonth))
                        //    {
                        //        ucMessage.ShowMessage("Month is already exist.", RMS.BL.Enums.MessageType.Info);
                        //        return;
                        //    }
                        //}
                    }



                    List<TblSalaryMonth> monthActiveList = db.TblSalaryMonths.Where(x => x.MonthIsActive == true && x.BranchID == BranchID).ToList();
                    foreach(var items in monthActiveList)
                    {
                        items.MonthIsActive = false;
                        db.SubmitChanges();
                    }
                    if(ID == 0)
                    {
                        TblSalaryMonth tblSalaryMonth = new TblSalaryMonth();
                        tblSalaryMonth.MonthVal = txtSelectedMonth.Text.Trim();
                        tblSalaryMonth.MonthIsActive = true;
                        tblSalaryMonth.BranchID = BranchID;
                        db.TblSalaryMonths.InsertOnSubmit(tblSalaryMonth);
                        
                    }
                    else
                    {
                        TblSalaryMonth tblSalaryMonthUpdate = db.TblSalaryMonths.Where(x => x.MonthID == ID).FirstOrDefault();
                        //tblSalaryMonthUpdate.MonthVal = txtSelectedMonth.Text;
                        tblSalaryMonthUpdate.MonthIsActive = true;

                    }
                    db.SubmitChanges();
                    BindMonthGrd();
                    Clear();
                    string salp = GetSalPeriod(BranchID);
                    lblMonthSal.Visible = true;
                    lblMonthSal.Text = "Currently Active Month is " + salp + ". Do you want to end it?";
                    ucMessage.ShowMessage("Month " + salp + " is successfull actived", RMS.BL.Enums.MessageType.Info);
                }
                catch
                {
                    ucMessage.ShowMessage("Errors occurred", RMS.BL.Enums.MessageType.Error);
                }
            }
            
        }


        protected void BindMonthGrd()
        {
            this.grdMonth.DataSource = db.TblSalaryMonths.Where(x => x.BranchID == BranchID).OrderByDescending(x => x.MonthID).ToList();
            this.grdMonth.DataBind();
        }

        protected void grdMonth_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMonth.PageIndex = e.NewPageIndex;
            BindMonthGrd();
        }


        protected void grdMonth_PageIndexChanged(object sender, EventArgs e)
        {

            ID = Convert.ToInt32(grdMonth.SelectedDataKey.Values["MonthID"].ToString());
            
            this.OnupdateEvent();

        }


        string OnupdateEvent()
        {
           
            
            try
            {
                TblSalaryMonth salMonthObj = db.TblSalaryMonths.Where(x => x.MonthID == ID).FirstOrDefault(); ;
                if (salMonthObj != null)
                {

                    txtSelectedMonth.Text = salMonthObj.MonthVal;
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }


        protected void Clear()
        {
            ID = 0;
        }

        #endregion

        #region Helping Method

        protected string GetSalPeriod(int brID)
        {
            try
            {

                TblSalaryMonth salaryMonth = db.TblSalaryMonths.Where(x => x.BranchID == brID && x.MonthIsActive == true).FirstOrDefault();
                if(salaryMonth != null)
                {
                    string currMonth = salaryMonth.MonthVal.ToString();
                    txtSelectedMonth.Text = currMonth;
                    return currMonth;
                }
                else
                {
                    return "";
                }
                    
                    
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch(Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }
        }
        #endregion

    }
}
 
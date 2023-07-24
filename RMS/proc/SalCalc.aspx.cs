using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Text.RegularExpressions;
namespace RMS.Setup
{
    public partial class SalCalc: BasePage
    {

        #region DataMembers

        PlUploadBL uploadmgr = new PlUploadBL();
       
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'SalCalc.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public string ID
#pragma warning restore CS0114 // 'SalCalc.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? "" : ViewState["ID"].ToString(); }
            set { ViewState["ID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalCalc").ToString();

                string salp = GetSalPeriod();

                if(salp.Equals(""))
                {
                    lblMonthSalAgain.Visible = true;
                    lblMonthSal.Visible = false;
                    lblMonthSalAgain.Text = "Salary is calculated for month " + salp + ", do you want to re-process it?";
                }
                else
                {
                    lblMonthSal.Visible = true;
                    lblMonthSalAgain.Visible = false;
                    lblMonthSal.Text = "Salary calculation for month " + salp + " is ready to process, do you want to process it?";
                }
            }
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                Response.Redirect("~/proc/salcalc.aspx?PID=41");
            }
            else 
            {
                try
                {
                    uploadmgr.StartSalCalc(CompID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //uploadmgr.StartSalCalcXL(CompID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucMessage.ShowMessage("Salary calculation is done for month " + ID + " successfully", RMS.BL.Enums.MessageType.Info);
                }
                catch
                {
                    ucMessage.ShowMessage("Errors occurred", RMS.BL.Enums.MessageType.Error);
                }
            }
            
        }
        
        

        #endregion

        #region Helping Method

        private string GetSalPeriod()
        {
            try
            {
                if (Session["CompID"] == null)
                {
                    ID = uploadmgr.GetPerd(Convert.ToInt32(Request.Cookies["uzr"]["CompID"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                else
                {
                    ID = uploadmgr.GetPerd(Convert.ToInt32(Session["CompID"].ToString()), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }


                if (ID == "")
                {
                    return "";
                }
                else
                {
                    return ID;
                }

            }
            catch { }
            return "";
        }
        #endregion

        
    }
}
 
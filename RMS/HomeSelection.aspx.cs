using System;
using System.Web.UI;
using System.Web;
using RMS.BL;
using System.Web.UI.WebControls;
namespace RMS
{
    public partial class HomeSelection : System.Web.UI.Page
    {

        #region DataMembers


#pragma warning disable CS0169 // The field 'HomeSelection.psw' is never used
        tblAppUser psw;
#pragma warning restore CS0169 // The field 'HomeSelection.psw' is never used
        RMS.BL.UserBL clsBL = new RMS.BL.UserBL();

        #endregion

        #region Properties

#pragma warning disable CS0114 // 'HomeSelection.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'HomeSelection.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    ID = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                }
            }
            else
            {
                ID = Convert.ToInt32(Session["UserID"].ToString());
            }

            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "HomeSelection").ToString();
            this.Page.Title = Session["PageTitle"].ToString();
            if (!IsPostBack)
            {
                GetCompanyName();
                if (Session["CompID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    else
                    {
                        FillDropDownBranch(Request.Cookies["uzr"]["CompID"].ToString());
                    }
                }
                else
                {
                    FillDropDownBranch(Session["CompID"].ToString());
                }
                if (Session["UserName"] != null)
                {
                    lblNameWelcome.Text = "Welcome " + Session["UserName"].ToString();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                //ddlBranch.Visible = true;
                
                if (Session["BranchID"].ToString().Equals("0"))
                {
                    ddlBranch.Enabled = true;
                }
                else
                {
                    ddlBranch.SelectedValue = Session["BranchID"].ToString();
                    ddlBranch.Enabled = false;
                }
                //string url = Request.Url.AbsolutePath;
                //lblCompName.Text = url.Substring(11, url.Length - 11);
            }
        }

        //protected void SavePassword(object sender, CommandEventArgs e)
        //{
        //    //if (e.CommandName == "Save")
        //    if (txtNewPwd.Text.Trim().Equals(txtConfPwd.Text.Trim()))
        //    {
        //        psw = clsBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                
        //        if (txtOldPwd.Text.Equals(psw.Password))
        //        {

        //                psw.Password = this.txtNewPwd.Text.Trim();
        //                clsBL.Update(psw, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //                //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
        //        }
        //        else
        //        {
        //            // show msg here...
        //            //ucMessage.ShowMessage("Old password does not match.", RMS.BL.Enums.MessageType.Error);
        //        }
        //    }
        //    else
        //    {
        //        // show msg here...
        //        //ucMessage.ShowMessage("New password does not match with confirm password", RMS.BL.Enums.MessageType.Error);
        //    }

        //    txtConfPwd.Text = "";
        //    txtNewPwd.Text = "";
        //    txtOldPwd.Text = "";
        //    // in any case refresh homeselection page
        //    Response.Redirect("~/homeselection.aspx?PID=1");
            
        //}
        protected void btnPayroll_Click(object sender, EventArgs e)
        {
            
            if (Page.IsValid == true)
            {
                Response.Redirect("~/home/home.aspx?PID=2"); 
            }
        }

        protected void btnAdmin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                Response.Redirect("~/home/adminhome.aspx?PID=1");
            }
        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                Response.Redirect("~/home/glhome.aspx?PID=310");
            }
        }

        protected void btnSales_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                Response.Redirect("~/home/saleshome.aspx?PID=900");
            }
        }
        protected void btnPruchase_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                Response.Redirect("~/home/purchhome.aspx?PID=851");
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                Response.Redirect("~/setup/changepassword.aspx?PID=999");
            }
        }
        protected void btnPayable_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                Response.Redirect("~/home/invenhome.aspx?PID=501");
            }
        }

        protected void btnInquiry_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                Response.Redirect("~/home/inqhome.aspx?PID=751");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            RMSDB.closeConn((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            Session.RemoveAll();
            Session.Clear();
            ExpireCookies();
            Response.Redirect("~/login.aspx");
        }

        
        #endregion

        #region Helping Method

        public void ExpireCookies()
        {
            int count = Request.Cookies.Count;
            for (int i = 0; i < count; i++)
            {
                HttpCookie aCookie = Request.Cookies[i];
                aCookie.Expires = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).AddDays(-1);
                Response.Cookies.Add(aCookie);
            }
        }

        protected void GetCompanyName()
        {
            try
            {
                //if (Session["CompName"] == null)
                //{
                //    this.lblCompName.Text = Request.Cookies["uzr"]["CompName"];
                //}
                //else
                //{
                //    this.lblCompName.Text = Session["CompName"].ToString();
                //}
            }
            catch { }
        }

        protected void FillDropDownBranch(string compid)
        {
            try
            {
                ddlBranch.DataTextField = "br_nme";
                ddlBranch.DataValueField = "br_id";
                ddlBranch.DataSource = new BranchBL().GetAllCompBranchCombo(compid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ddlBranch.DataBind();
            }
            catch
            {
                Response.Redirect("~/login.aspx");
            }
        }
        
        #endregion

    }
}
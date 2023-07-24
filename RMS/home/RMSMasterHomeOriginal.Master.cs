using System;
using System.Web.UI.WebControls;
using System.Web.UI;

using RMS.BL;
using System.Web;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.SessionState;
namespace RMS
{
    public partial class RMSMasterHomeOriginal : System.Web.UI.MasterPage
    {
        #region DataMembers

        int userID, groupID;
        string pageTitle;
        AppMenuBL menuBL = new AppMenuBL();
        UserBL userBL = new UserBL();
        tblAppUser user = new tblAppUser();

        #endregion

        #region Properties

        public static int moduleID;

        #endregion

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    CreateLog();
                    if (Request.Cookies["uzr"]["UserID"] != null && Convert.ToInt32(Request.Cookies["uzr"]["UserID"]) > 0)
                    {
                        ClearAllAndGenerateSessionCookeisAgain(Convert.ToInt32(Request.Cookies["uzr"]["UserID"]));
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
            }
            catch { Response.Redirect("~/login.aspx"); }
            //if (IsSessionExpired())
            //{
            //    //Response.Redirect("~/login.aspx");
            //    ClearAllAndGenerateSessionCookeisAgain();
            //}
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblTime.Text = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

            ScriptManager.RegisterClientScriptInclude(this, typeof(Page), "ss", ResolveUrl("~/js/AC_RunActiveContent.js"));

            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            //ScriptManager.RegisterClientScriptInclude(this, typeof(Page), "ss", ResolveUrl("~/js/AC_RunActiveContent.js"));

            if (Session["UserID"] == null)
            {
                userID = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
            }
            else
            {
                userID = Convert.ToInt32(Session["UserID"].ToString());
            }
            if (Session["GroupID"] == null)
            {
                groupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
            }
            else
            {
                groupID = Convert.ToInt32(Session["GroupID"].ToString());
            }
            if (Session["PageTitle"] == null)
            {
                pageTitle = "";// Request.Cookies["uzr"]["PageTitle"];
            }
            else
            {
                pageTitle = Session["PageTitle"].ToString();
            }
            try
            {
                if (Session["AppName"] == null)
                {
                    ContentPlaceHolder1.Page.Title = pageTitle;// +" - " + Request.Cookies["uzr"]["AppName"];
                }
                else
                {
                    ContentPlaceHolder1.Page.Title = pageTitle;// +" - " + Session["AppName"].ToString();
                }

                //((Label)ContentPlaceHolder1.FindControl("lblTitle")).Text = pageTitle;
                lblTitle.Text = pageTitle;
            }
            catch (Exception ex)
            {
                throw ex;
                //Session["errors"] = ex.Message;
                //Response.Redirect("~/home/Error.aspx");
            }
            // setting menu to open 

            //BindUser();
            if (!IsPostBack)
            {
                int pid = 0;
                if (Request.QueryString["PID"] != null)
                {
                    PrivilageBL privilagebl = new PrivilageBL();
                    try
                    {
                        pid = Convert.ToInt32(Request.QueryString["PID"]);
                        if (!privilagebl.HasAccess(pid, groupID, Request.Url.AbsolutePath, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                            Server.Transfer("~/NotAutherized.htm");
                    }
                    catch
                    {
                        RMSDB.closeConn((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        Session.RemoveAll();
                        Session.Clear();
                        ExpireCookies();
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    RMSDB.closeConn((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    Session.RemoveAll();
                    Session.Clear();
                    ExpireCookies();
                    Response.Redirect("~/login.aspx");
                }
                try
                {
                    moduleID = menuBL.GetModuleID(userID, pid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                catch
                {
                }

                //try
                //{
                //   LoadMenu(pid);
                //}
                //catch
                //{
                //    Session.Add("menuError", "menuError");
                //    Response.Redirect("~/login.aspx");
                //}
                // chek its place whether to come here or above ....
                BindUser();

                // company name 
                GetCompanyName();
                ////if (moduleID > 0)
                ////    this.LoadMasterMenu(moduleID);
                //testing for URL substring for privilage check
                //string url = Request.Url.AbsolutePath;
                //lblWelcome.Text = url.Substring(11, url.Length - 11);
            }
            try
            {
                if (moduleID > 0)
                    this.LoadMasterMenu(moduleID);
            }
            catch
            {
                Session.Add("menuError", "menuError");
                Response.Redirect("~/login.aspx");
            }

        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            CreateLogoutLog();
            RMSDB.closeConnLogout((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            Session.RemoveAll();
            Session.Clear();
            ExpireCookies();
            
            Response.Redirect("~/login.aspx");
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Session.Add("PageTitle", "Home");
            Response.Redirect("~/home/home.aspx");
        }

        #endregion

        #region Helping Method

        public static bool IsSessionExpired()
        {
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session.IsNewSession)
                {
                    string CookieHeaders = HttpContext.Current.Request.Headers["Cookie"];

                    if ((null != CookieHeaders) && (CookieHeaders.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        // IsNewSession is true, but session cookie exists,
                        // so, ASP.NET session is expired
                        return true;
                    }
                }
            }

            // Session is not expired and function will return false,
            // could be new session, or existing active session
            return false;
        }
        protected void GetCompanyName()
        {
            try
            {
                if (Session["CompName"] == null)
                {
                    this.lblCompName.Text = Request.Cookies["uzr"]["CompName"];
                }
                else
                {
                    this.lblCompName.Text = Session["CompName"].ToString();
                }
            }
            catch { }
        }
        protected void BindUser()
        {
            if (userID > 0 && groupID > 0)
            {
                string userName, location = "";
                userName = userBL.GetWelcome(userID, ref location, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

                if (userName.Equals("relogin4301paspioh234892214@#$!$!$@$!@$!@$!@fsdg"))
                {
                    Response.Redirect("~/login.aspx");
                }

                if (Session["UserName"] == null)
                {
                    this.lblWelcome.Text = "Welcome " + Request.Cookies["uzr"]["UserName"];
                }
                else
                {
                    this.lblWelcome.Text = "Welcome " + Session["UserName"].ToString();
                }

                if (Session["GroupName"] == null)
                {
                    this.lblWelcome.Text = "Welcome " + Request.Cookies["uzr"]["UserName"] + " (" + Request.Cookies["uzr"]["GroupName"].ToString() + ")";
                }
                else
                {
                    this.lblWelcome.Text = "Welcome " + Session["UserName"] + ", ";// + " (" + Session["GroupName"] + ") ";
                }

                //if (Session["RegionName"] != null)
                //{
                //    this.lblWelcome0.Text = Session["RegionName"].ToString();
                //}
                //else
                //{
                this.lblWelcome0.Text = location;
                //}
                string mn = "";
                string yr = "";

                if (Session["CurPayPeriod"] == null)
                {
                    string dt = Request.Cookies["uzr"]["CurPayPeriod"].ToString();
                    yr = dt.Substring(0, 4);
                    mn = dt.Substring(4, 2);
                }
                else
                {
                    string dt = Session["CurPayPeriod"].ToString();
                    yr = dt.Substring(0, 4);
                    mn = dt.Substring(4, 2);
                }

                DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);
                if (moduleID.Equals(2))
                {
                    string moduleNamePayroll = menuBL.GetModuleName(2, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    lblModuleName.Text = moduleNamePayroll;
                    lblWelcomeDateTime.Text = " (Process Month" + " : " + ddfrom.ToString("MMM-yyyy") + ")";
                }
                else //if (moduleID.Equals(1) || moduleID.Equals(3) || moduleID.Equals(4) || moduleID.Equals(9))
                {
                    string moduleName = menuBL.GetModuleName(moduleID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    lblModuleName.Text = moduleName;
                    //lblWelcomeDateTime.Text = " (Date" + " : " + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yyyy") + ")";
                }

            }
        }
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
        protected void Accordion1_ItemDataBound(object sender, AjaxControlToolkit.AccordionItemEventArgs e)
        {
            if (e.ItemType == AjaxControlToolkit.AccordionItemType.Content)
            {
                GridView grd = (GridView)e.AccordionItem.FindControl("GridView1");
                grd.DataSource = menuBL.GetNestedMenu(userID, Convert.ToInt32(((HiddenField)e.AccordionItem.FindControl("txtMenuID")).Value), false, groupID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);//dashBL.GetChildMenu(Convert.ToInt32(((HiddenField)e.AccordionItem.FindControl("txtMenuID")).Value), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                grd.DataBind();
            }
        }
        private void LoadMasterMenu(int moduleid)
        {
            List<spMenuResult> menu = menuBL.GetMenu(moduleid, groupID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (menu.Count > 0)
            {
                int parentid = Convert.ToInt32(menu[0].AmIDParent), menuCount = 0;
                foreach (spMenuResult m in menu)
                {
                    HyperLink link = new HyperLink();
                    link.Text = m.AmName;
                    link.NavigateUrl = m.AmURL + "?PID=" + m.AmID;
                    HtmlGenericControl li = new HtmlGenericControl("li"); //Create html control <li>

                    li.Controls.Add(link); //add hyperlink to <li>

                    if (parentid == m.AmIDParent)
                    {
                        //Do nothing
                    }
                    else
                    {
                        parentid = Convert.ToInt32(m.AmIDParent);
                        menuCount++;
                    }

                    if (menuCount == 0)
                    {
                        li0.Visible = true;
                        a0.Name = m.PAmName;
                        a0.InnerHtml = m.PAmName;
                        ul0.Controls.Add(li);  //add <li> to <ul>
                    }
                    else if (menuCount == 1)
                    {
                        li1.Visible = true;
                        a1.InnerHtml = m.PAmName;
                        ul1.Controls.Add(li);  //add <li> to <ul>
                    }
                    else if (menuCount == 2)
                    {
                        li2.Visible = true;
                        a2.InnerHtml = m.PAmName;
                        ul2.Controls.Add(li);  //add <li> to <ul>
                    }
                    else if (menuCount == 3)
                    {
                        li3.Visible = true;
                        a3.InnerHtml = m.PAmName;
                        ul3.Controls.Add(li);  //add <li> to <ul>
                    }
                    else if (menuCount == 4)
                    {
                        li4.Visible = true;
                        a4.InnerHtml = m.PAmName;
                        ul4.Controls.Add(li);  //add <li> to <ul>
                    }
                    else if (menuCount == 5)
                    {
                        li5.Visible = true;
                        a5.InnerHtml = m.PAmName;
                        ul5.Controls.Add(li);  //add <li> to <ul>
                    }
                    else if (menuCount == 6)
                    {
                        li6.Visible = true;
                        a6.InnerHtml = m.PAmName;
                        ul6.Controls.Add(li);  //add <li> to <ul>
                    }
                    else if (menuCount == 7)
                    {
                        li7.Visible = true;
                        a7.InnerHtml = m.PAmName;
                        ul7.Controls.Add(li);  //add <li> to <ul>
                    }
                    else if (menuCount == 8)
                    {
                        li8.Visible = true;
                        a8.InnerHtml = m.PAmName;
                        ul8.Controls.Add(li);  //add <li> to <ul>
                    }
                    else if (menuCount == 9)
                    {
                        li9.Visible = true;
                        a9.InnerHtml = m.PAmName;
                        ul9.Controls.Add(li);  //add <li> to <ul>
                    }
                }
            }
        }
        public void CreateLog()
        {
            try{
            string LogPath = ConfigurationManager.AppSettings["ErrorLogPath"].ToString();
            string path = Server.MapPath(LogPath);

            StringBuilder msg = new StringBuilder();
            msg.Append("Session User ID: " + Convert.ToString(Session["UserID"]));
            msg.AppendLine();
            msg.Append("Session Branch ID: " + Convert.ToString(Session["BranchID"]));
            msg.AppendLine();
            msg.Append("Cookies User ID: " + Convert.ToString(Request.Cookies["uzr"]["UserID"]));
            msg.AppendLine();
            msg.Append("Cookies Branch ID: " + Convert.ToString(Request.Cookies["uzr"]["BranchID"]));
            msg.AppendLine();
            msg.Append("Session Timeout: " + Convert.ToString(Session.Timeout));

            new CreateLog().AddToLogFile(msg, path, "Master Page -->> Page_Init", RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]));
            }
            catch { }

        }
        public void CreateLogoutLog()
        {
            string LogPath = ConfigurationManager.AppSettings["ErrorLogPath"].ToString();
            string path = Server.MapPath(LogPath);

            StringBuilder msg = new StringBuilder();
            msg.Append("Session User ID: " + Convert.ToString(Session["UserID"]));
            msg.AppendLine();
            msg.Append("Cookies User ID: " + Convert.ToString(Request.Cookies["uzr"]["UserID"]));
            msg.AppendLine();
            msg.Append("Session Timeout: " + Convert.ToString(Session.Timeout));

            new CreateLog().AddToLogFile(msg, path, "Master Page -->> btnLogout_Click", RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]));

        }
        private void ClearAllAndGenerateSessionCookeisAgain(int _userid)
        {

            int userId = _userid;
            RMSDataContext Data = null;
            if (Data == null) { Data = RMSDB.GetOject(); }
            //try
            //{
            //    if (Session != null)
            //    {
            //        Session.RemoveAll();
            //        Session.Clear();
            //    }
            //    ExpireCookies();
            //}
            //catch { }

            Session.Timeout = 120;

            Session[userId + "rmsDBObj"] = Data;
            user = new UserBL().GetByID(userId, (RMSDataContext)Session[userId + "rmsDBObj"]);
            string compNme = new CompanyBL().GetFirstCompName((RMSDataContext)Session[userId + "rmsDBObj"]);
            //spPreferences_GetResult pref = clsBL.Preference().First();

            //delete cookies first if already exist.
            //try { ExpireCookies(); }
            //catch { }

            HttpCookie cookie = new HttpCookie("uzr");

            cookie.Expires = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).AddHours(2);

            cookie.Values["UserID"] = user.UserID.ToString();
            cookie.Values["LoginID"] = user.LoginID.ToString();
            cookie.Values["UserName"] = user.UserName;
            cookie.Values["CompID"] = user.CompID.ToString();
            cookie.Values["CompName"] = compNme;
            cookie.Values["GroupID"] = user.GroupID.ToString();
            cookie.Values["GroupName"] = user.tblAppGroup.GroupName;
            cookie.Values["DateTimeFormat"] = System.Configuration.ConfigurationManager.AppSettings["DateTimeFormat"].ToString();
            cookie.Values["DateFormat"] = System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString();
            cookie.Values["DateFullYearFormat"] = System.Configuration.ConfigurationManager.AppSettings["DateFullYearFormat"].ToString();
            cookie.Values["AppName"] = System.Configuration.ConfigurationManager.AppSettings["AppName"].ToString();
            cookie.Values["CurPayPeriod"] = user.tblCompany.CurPayPeriod.ToString();



            if (user.tblPlEmpData != null)
            {
                cookie.Values["EmpID"] = user.tblPlEmpData.EmpID.ToString();
                cookie.Values["EmpCode"] = user.tblPlEmpData.EmpCode.ToString();
            }
            else
            {
                cookie.Values["EmpID"] = "0";
                cookie.Values["EmpCode"] = "0";
            }
            string br_id = "", br_name = "";
            if (user.Branch != null)
            {
                cookie.Values["BranchID"] = user.Branch.br_id.ToString();
                cookie.Values["BranchName"] = user.Branch.br_nme;
                br_id = user.Branch.br_id.ToString();
                br_name = user.Branch.br_nme;
            }
            else
            {
                cookie.Values["BranchID"] = "0";
                cookie.Values["BranchName"] = "All";
                br_id = "0";
                br_name = "All";
            }
            if (user.vendor != null)
                cookie.Values["VendorID"] = user.vendor;
            else
                cookie.Values["VendorID"] = user.vendor;

            Response.Cookies.Add(cookie);

            CreateSession(user.UserID.ToString(), user.CompID.ToString(), user.UserName, user.GroupID.ToString(), user.tblAppGroup.GroupName, user.tblCompany.CurPayPeriod.ToString(), user.LoginID.ToString(), compNme, br_id, br_name, user.vendor);

        }
        private void CreateSession(string userid, string compid, string username,
           string groupid, string groupname, string curPayperiod, string loginId, string compName, string brid, string brNme, string vendor)
        {
            Session.Add("UserID", userid);
            Session.Add("LoginID", loginId);
            Session.Add("CompID", compid);
            Session.Add("CompName", compName);
            Session.Add("UserName", username);
            Session.Add("GroupID", groupid);
            Session.Add("GroupName", groupname);
            Session.Add("DateTimeFormat", System.Configuration.ConfigurationManager.AppSettings["DateTimeFormat"].ToString());
            Session.Add("DateFormat", System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString());
            Session.Add("DateFullYearFormat", System.Configuration.ConfigurationManager.AppSettings["DateFullYearFormat"].ToString());
            Session.Add("AppName", System.Configuration.ConfigurationManager.AppSettings["AppName"].ToString());

            Session.Add("CurPayPeriod", curPayperiod);

            if (user.tblPlEmpData != null)
            {
                Session.Add("EmpID", user.tblPlEmpData.EmpID.ToString());
                Session.Add("EmpCode", user.tblPlEmpData.EmpCode.ToString());
            }
            else
            {
                Session.Add("EmpID", "0");
                Session.Add("EmpCode", "0");
            }

            Session["BranchID"] = brid;
            Session["BranchName"] = brNme;

            if (!string.IsNullOrEmpty(vendor))
            {
                Session["VendorID"] = vendor;
            }
            else
            {
                Session["VendorID"] = "";
            }
        }

        #endregion
    }
}
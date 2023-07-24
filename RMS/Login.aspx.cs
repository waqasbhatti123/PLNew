
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.Mail;
//using System.Net;
//using System.Net.Mail; 

using RMS.BL;
using System.Web;
using System.Configuration;
using System.Text;

namespace RMS
{
    //
    public partial class Login : System.Web.UI.Page
    {

        #region DataMembers
        tblAppUser user = new tblAppUser();
        UserBL clsBL = new UserBL();
        RMSDataContext db = new RMSDataContext();

        #endregion

        #region Properties
        //public int LoginAttempt
        //{
        //    get 
        //    {
        //        if (Session["LoginAttempt"] == null)
        //        {
        //            Session.Add("LoginAttempt", 0);
        //            return 0;
        //        }
        //        else
        //        {
        //            return Convert.ToInt32(Session["LoginAttempt"]);
        //        }
        //    }
        //    set 
        //    {
        //        Session["LoginAttempt"] = value; 
        //    }
        //}

        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
          
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Login").ToString();
            this.Page.Title = Session["PageTitle"].ToString();

            if (!IsPostBack)
            {
              
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
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //this.lblStatus.Text = "";
            if (Page.IsValid == true)
            {
                string comp = this.txtUSERID.Text.Trim();
                //if (comp.Contains("@"))
                //{
                this.IsValidUser(comp, this.txtPassword.Text.Trim());
                //}
                //else
                //{
                //  //this.IsValidUser(comp + "@ksb.com.pk", this.txtPassword.Text.Trim());
                //}
                CreateLog();

                //Response.Redirect("~/home/home.aspx?PID=1");
                Response.Redirect("~/homeselection.aspx?PID=1");
            }
        }
        public void CreateLog()
        {
            try
            {
                string LogPath = ConfigurationManager.AppSettings["ErrorLogPath"].ToString();
                string path = Server.MapPath(LogPath);

                StringBuilder msg = new StringBuilder();
                msg.Append("Session User ID: " + Convert.ToString(Session["UserID"]));
                msg.AppendLine();
                msg.Append("Cookies User ID: " + Convert.ToString(Request.Cookies["uzr"]["UserID"]));
                msg.AppendLine();
                msg.Append("Session Timeout: " + Convert.ToString(Session.Timeout));

                new CreateLog().AddToLogFile(msg, path, "Login Page -->> btnLOGIN_Click", RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]));
            }
            catch { }
        }
        #endregion

        #region Helping Method

        protected void BindBranch(int userId)
        {
            //IQueryable<Branch> br;
            //br = this.clsBr.GetByUserID(userId);

            //if (br.Count() == 0)
            //{
            //    Session.RemoveAll();
            //}
            //else if (br.Count() == 1)
            //{
            //    Branch branch = br.First();
            //    Session.Add("BranchID", branch.br_id);
            //    Response.Redirect("~/home.aspx");
            //}
            //else
            //{
            //    this.ddlBranch.Items.Clear();
            //    this.ddlBranch.Items.Add(new ListItem("Select Branch", "0"));
            //    this.ddlBranch.DataTextField = "Name";
            //    this.ddlBranch.DataValueField = "br_id";
            //    this.ddlBranch.DataSource = br;
            //    this.ddlBranch.DataBind();

            //    Response.Redirect("~/home.aspx");
            //    //divBranch.Visible = true;
            //}
        }

        protected void IsValidUser(string Email, string Password)
        {

            int isValid = 0;
            Int32? userId = 0;

            RMSDataContext Data = null;
            isValid = clsBL.IsValid(Email, Password, ref userId, ref Data);

            if (isValid == 1)
            {
                

                Session.Timeout = 120;

                /////////////////////////////
                Session[userId + "rmsDBObj"] = Data;
                user = clsBL.GetByID(userId.Value, (RMSDataContext)Session[userId + "rmsDBObj"]);
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



                string bankName = "";

                if (user.tblPlEmpData != null)
                {
                    cookie.Values["EmpID"] = user.tblPlEmpData.EmpID.ToString();
                    cookie.Values["EmpCode"] = user.tblPlEmpData.EmpCode.ToString();

                    string bankCode = user.tblPlEmpData.Bank;
                    if(bankCode != "" && bankCode != null)
                    {
                        tblBank bank = db.tblBanks.Where(x => x.BankCode == bankCode).FirstOrDefault();
                        if (bank == null)
                        {
                            bankName = "";
                            cookie.Values["BankName"] ="";
                        }
                        else
                        {
                            bankName = bank.BankName.ToString();
                            cookie.Values["BankName"] = bank.BankName.ToString();
                        }
                        
                    }
                    
                }
                else
                {
                    cookie.Values["EmpID"] = "0";
                    cookie.Values["EmpCode"] = "0";

                    cookie.Values["BankName"] = bankName;
                

                 }

                
                string br_id = "", br_name = ""; string br_Address = ""; int ddt = 0 ; string br_prmimaryConact = "(xxx) xxxxxxx"; string br_secondaryContact = "(xxx) xxxxxxx"; 
                if (user.Branch != null)
                {
                    cookie.Values["BranchID"] = user.Branch.br_id.ToString();
                    cookie.Values["BranchName"] = user.Branch.br_nme;
                    cookie.Values["Br_Address"] = user.Branch.br_address;
                    cookie.Values["Br_PrimaryContact"] = user.Branch.br_tel;
                    cookie.Values["Br_SecondaryContact"] = user.Branch.br_contact;
                    cookie.Values["Division"] = user.Branch.DDt.ToString();
                    br_id = user.Branch.br_id.ToString();
                    br_name = user.Branch.br_nme;
                    ddt = Convert.ToInt32(user.Branch.DDt);
                    br_Address = user.Branch.br_address;
                    br_prmimaryConact = user.Branch.br_tel;
                    br_secondaryContact = user.Branch.br_contact;
                }
                else
                {
                    cookie.Values["BranchID"] = "0";
                    cookie.Values["BranchName"] = "All";
                    br_id = "0";
                    br_name = "All";

                    cookie.Values["Division"] = ddt.ToString();
                    cookie.Values["Br_Address"] = br_Address;

                    cookie.Values["Br_PrimaryContact"] = br_prmimaryConact;
                    cookie.Values["Br_SecondaryContact"] = br_secondaryContact;
                }
                if (user.vendor != null)
                    cookie.Values["VendorID"] = user.vendor;
                else
                    cookie.Values["VendorID"] = user.vendor;

                Response.Cookies.Add(cookie);

                CreateSession(user.UserID.ToString(), user.CompID.ToString(), user.UserName, user.GroupID.ToString(), user.tblAppGroup.GroupName, user.tblCompany.CurPayPeriod.ToString(), user.LoginID.ToString(), compNme, br_id, ddt , br_name,br_Address, br_prmimaryConact, br_secondaryContact, user.vendor, bankName);

                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "LoginSuccessful").ToString(), RMS.BL.Enums.MessageType.Info);

                

            }
            else if (isValid == 2)
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "LoginDisabled").ToString(), RMS.BL.Enums.MessageType.Error);
            }
            else
            {
                //LoginAttempt += 1;
                //if (LoginAttempt > 2)
                //{
                //    if (userId.Value > 0)
                //    {
                //        UserBL usrBL = new UserBL();
                //        usrBL.UserDisable(Email);
                //        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "LoginDisabled").ToString(), RMS.BL.Enums.MessageType.Error);
                //    }
                //    else
                //    {
                //        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "LoginFailed").ToString(), RMS.BL.Enums.MessageType.Error);
                //    }
                //    Response.Redirect("~/genericerror.aspx");
                //}
                //else
                //{
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "LoginFailed").ToString(), RMS.BL.Enums.MessageType.Error);
                //}

            }

        }
        private void CreateSession(string userid, string compid, string username,
            string groupid, string groupname, string curPayperiod, string loginId, string compName, string brid, int ddt, string brNme,string Address, string firstContact, string secondContact, string vendor, string bank)
        {
            Session.Add("UserID", userid);
            Session.Add("LoginID",loginId);
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
                Session.Add("EmpID",user.tblPlEmpData.EmpID.ToString());
                Session.Add("EmpCode", user.tblPlEmpData.EmpCode.ToString());
            }
            else
            {
                Session.Add("EmpID", "0");
                Session.Add("EmpCode", "0");
            }

            Session["BankName"] = bank;
            Session["Divi"] = ddt;
            Session["BranchID"] = brid;
            Session["BranchName"] = brNme;
            Session["Br_Address"] = Address;

            Session["Br_PrimaryContact"] = firstContact;
            Session["Br_SecondaryContact"] = secondContact;

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


        protected void btnForgotPassword_Click(object sender, EventArgs e)
        {
            //this.lblStatus.Text = "";

            //if (Page.IsValid == true)
            //{

            //    string userEmail = this.txtUSERID.Text.ToString();
            //    //clsBL.GetUserInfo(userEmail, ref userpass, ref blnUserExist);
            //    BL.Users_ResetPassResult userReset = clsBL.ResetPassword(userEmail); 

            //    if (userReset.password == "INVALID")
            //    {
            //        this.lblStatus.Text =  GetGlobalResourceObject("UserNotFound").ToString();
            //    }
            //    else if (userReset.password == "DISABLED")
            //    {
            //        this.lblStatus.Text = GetGlobalResourceObject("MainResource","LoginDisabled").ToString();
            //    }
            //    else
            //    {
            //        ////SmtpClient smtpClient = new SmtpClient("smtp.yourdomain.com");
            //        //SmtpClient smtpClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString());    //Domain Name
            //        ////smtpClient.Credentials = new System.Net.NetworkCredential("User Id", "password");
            //        //smtpClient.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["DomainUserID"].ToString(), System.Configuration.ConfigurationManager.AppSettings["DomainUserPass"].ToString());
            //        //MailMessage message = new MailMessage();

            //        //SmtpClient smtpClient = new SmtpClient("smtp.yourdomain.com");
            //        SmtpClient smtpClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString());    //Domain Name
            //        //smtpClient.Credentials = new System.Net.NetworkCredential("User Id", "password");
            //        if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ReqAuthentication"]))
            //        {
            //            smtpClient.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SMTPUserID"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SMTPPass"].ToString());
            //        }

            //        MailMessage message = new MailMessage();


            //        try
            //        {

            //            MailAddress fromAddress = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString(), System.Configuration.ConfigurationManager.AppSettings["DisplayName"].ToString());

            //            //smtpClient.Port = 25;
            //            smtpClient.Port = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString());
            //            //From address will be given as a MailAddress Object 
            //            message.From = fromAddress;
            //            // To address collection of MailAddress 
            //            message.To.Add(this.txtUSERID.Text);
            //            message.Subject = GetGlobalResourceObject("EmailSubject").ToString();//System.Configuration.ConfigurationManager.AppSettings["DomainUserID"].ToString();

            //            message.IsBodyHtml = false;

            //            // Message body content 

            //            //message.Body = "Your password for Car Rental login is '" + user.Password + "',Thank you";
            //            //UserNotFound

            //            message.Body = string.Format(GetGlobalResourceObject("EmailBody").ToString(), userReset.password);// "Your password for Car Rental login is '" + user.Password + "',Thank you";
            //            message.Body = message.Body.Replace("\\n", "\n");

            //            // Send SMTP mail 
            //            smtpClient.Send(message);
            //            this.lblStatus.Text = string.Format(GetGlobalResourceObject("SentEmailMsg").ToString(), this.txtUSERID.Text);
            //            //this.lblStatus.Text = "Email has been sent successfully at " + this.txtUSERID.Text;

            //        }

            //        catch (Exception ex)
            //        {

            //            //this.lblStatus.Text = "Send Email Failed." + ex.Message;
            //            this.lblStatus.Text = GetGlobalResourceObject("SentEmailFailed") + ex.Message;

            //        }

            //    }
            //}

        }

    }
}
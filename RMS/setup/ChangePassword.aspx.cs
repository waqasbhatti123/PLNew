using System;
using RMS.BL;

namespace RMS.Setup
{
    public partial class ChangePassword : BasePage
    {
        #region DataMembers
        tblAppUser psw;
        RMS.BL.UserBL clsBL = new RMS.BL.UserBL();
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'ChangePassword.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'ChangePassword.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            txtPswd_Old.Focus();
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Password").ToString();
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
            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                ClearFields();
            }
            else
            {
                this.Update(ID);

            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        #endregion

        #region Helping Method

        protected void Update(int Id)
        {
            psw = clsBL.GetByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (psw.Password.Equals(this.txtPswd_Old.Text))
            {
                psw.Password = this.txtPswd_New.Text;
                clsBL.Update(psw, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
            }
            else
                ucMessage.ShowMessage("Old password does not match.", RMS.BL.Enums.MessageType.Error);
        }

        private void ClearFields()
        {
            this.txtPswd_Old.Text = "";
            this.txtPswd_New.Text = "";
            this.txtPswd_retype.Text = "";
            //ID = 0;
        }

        #endregion

    }

}
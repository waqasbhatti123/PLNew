using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text;


namespace RMS.inquiry
{
    public partial class EmailMgt : BasePage
    {
        #region DataMembers

        //tblMailSetup mailSetup;
        MailBL designMail = new RMS.BL.MailBL();
        
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'EmailMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmailMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }

        }

        #endregion

        #region Events
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmailMgt").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                //txtDesignation.Focus();
            }
        }

        protected void grdDesignations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
#pragma warning disable CS0168 // The variable 'sid' is declared but never used
                int sid;
#pragma warning restore CS0168 // The variable 'sid' is declared but never used
                if (e.Row.Cells[3].Text == "False")
                {
                    e.Row.Cells[3].Text = "Disable";
                }
                else
                {
                    e.Row.Cells[3].Text = "Enabled";
                    e.Row.BackColor = System.Drawing.Color.Green;
                }
            }
        }

        protected void grdDesignations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdDesignations.SelectedDataKey.Value);
                //this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }

        protected void grdDesignations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDesignations.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtDesignation.Focus();
            }
            else if (e.CommandName == "Save")
            {
                if (ID == 0)
                {
                    //this.Insert();
                }
                else
                {
                    //this.Update();
                }
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    this.Delete();
                    pnlMain.Enabled = false;
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.None);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletionDependency").ToString(), RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        Session["errors"] = ex.Message;
                        Response.Redirect("~/home/Error.aspx");
                    }
                }

                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtDesignation.Focus();

            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();

            }
        }

        #endregion

        #region Helping Method

        protected void BindGrid()
        {
            this.grdDesignations.DataSource = designMail.GetAll4Grid((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdDesignations.DataBind();
        }

        //protected void GetByID()
        //{
        //    mailSetup = designMail.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    this.txtMailFrom.Text = mailSetup.MailFrom.Trim();
        //    this.txtMailHead.Text = mailSetup.MailHead.Trim();
        //    this.txtMailAddress.Text = mailSetup.MailAddress.Trim();
        //    this.txtMailPswd.Text = mailSetup.MailPassword;
        //    this.txtMailPort.Text = mailSetup.Port.ToString();
        //    this.txtMailHost.Text = mailSetup.Host;
        //    this.rblSSL.SelectedValue = mailSetup.EnableSSL == true ? "1" : "0";
        //    this.rblStatus.SelectedValue = mailSetup.Status == true ? "1" : "0";
        //}

        //protected void Update()
        //{
        //    mailSetup = designMail.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    //mailSetup = new tblMailSetup();
        //    mailSetup.MailFrom = this.txtMailFrom.Text.Trim();
        //    mailSetup.MailHead = this.txtMailHead.Text.Trim();
        //    mailSetup.MailAddress = this.txtMailAddress.Text.Trim();
        //    mailSetup.MailPassword = this.txtMailPswd.Text.Trim();
        //    mailSetup.Port = Convert.ToInt32(txtMailPort.Text.Trim());
        //    mailSetup.Host = this.txtMailHost.Text.Trim();
        //    mailSetup.EnableSSL = rblSSL.SelectedValue.Equals("1") ? true : false;
        //    if (rblStatus.SelectedValue.Equals("1"))
        //    {
        //        designMail.getMailStatus((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        mailSetup.Status = rblStatus.SelectedValue.Equals("1") ? true : false;
        //    }
        //    else
        //    {
        //        mailSetup.Status = rblStatus.SelectedValue.Equals("1") ? true : false;
        //    }
        //    designMail.Update(mailSetup, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
        //    BindGrid();
        //    ClearFields();
        //    //if (!desigBL.ISAlreadyExist(mailSetup, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
        //    //{
        //    //    desigBL.Update(mailSetup, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
        //    //    BindGrid();
        //    //    ClearFields();
        //    //}
        //    //else
        //    //{
        //    //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "designationAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
        //    //    pnlMain.Enabled = true;
        //    //}

        //}

        protected void Delete()
        {
            //codeBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        //protected void Insert()
        //{
        //    mailSetup = new tblMailSetup();
        //    mailSetup.MailFrom = this.txtMailFrom.Text;
        //    mailSetup.MailHead = this.txtMailHead.Text;
        //    mailSetup.MailAddress = this.txtMailAddress.Text;
        //    mailSetup.MailPassword = this.txtMailPswd.Text;
        //    mailSetup.Port = Convert.ToInt32(txtMailPort.Text);
        //    mailSetup.Host = this.txtMailHost.Text;
        //    mailSetup.EnableSSL = rblSSL.SelectedValue.Equals("1") ? true : false;
        //    if (rblStatus.SelectedValue.Equals("1"))
        //    {
        //        designMail.getMailStatus((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        mailSetup.Status = rblStatus.SelectedValue.Equals("1") ? true : false;
        //    }
        //    else
        //    {
        //        mailSetup.Status = rblStatus.SelectedValue.Equals("1") ? true : false;
        //    }

        //    designMail.Insert(mailSetup, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
        //    BindGrid();
        //    ClearFields();
            
        //    //if (Session["CompID"] == null)
        //    //{
        //    //    try
        //    //    {
        //    //        plCode.CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"].ToString());
        //    //    }
        //    //    catch
        //    //    {
        //    //        ucMessage.ShowMessage("Please login again, session is expired", RMS.BL.Enums.MessageType.Error);
        //    //        return;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    plCode.CompID = Convert.ToByte(Session["CompID"].ToString());
        //    //}

        //    //plCode.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
        //    //if (!desigBL.ISAlreadyExist(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
        //    //{
        //    //    desigBL.Insert(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
        //    //    BindGrid();
        //    //    ClearFields();
        //    //}
        //    //else
        //    //{
        //    //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "designationAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
        //    //    pnlMain.Enabled = true;
        //    //}
        //}

        private void ClearFields()
        {
            txtDesignation.Text = "";
            txtMailFrom.Text = "";
            txtMailHead.Text = "";
            txtMailHost.Text = "";
            txtMailPort.Text = "";
            txtMailPswd.Text = "";
            txtMailAddress.Text = "";
            ID = 0;
            rblStatus.SelectedValue = "1";
            rblSSL.SelectedValue = "1";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

            grdDesignations.SelectedIndex = -1;
            txtDesignation.Focus();

        }

        #endregion
    }
}
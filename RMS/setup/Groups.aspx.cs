using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class Groups : BasePage
    {
        #region DataMembers
        
        tblAppGroup grp;
        RMS.BL.GroupBL groupManager = new RMS.BL.GroupBL();
        
        #endregion

        #region Properties
        
#pragma warning disable CS0114 // 'Groups.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'Groups.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }

        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Groups").ToString();
            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                txtGroup.Focus();
            }
        }

        protected void grdGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdGroups.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtGroup.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdGroups.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                //pnlMain.Enabled = true;
                txtGroup.Focus();
            }
            else if (e.CommandName == "Save")
            {
                if (ID == 0)
                {
                    this.Insert();
                }
                else
                {
                    this.Update();
                }
            }
            else if (e.CommandName == "Delete")
            {
                if (groupManager.hasEnabledPrivilage(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletionDependency").ToString(), RMS.BL.Enums.MessageType.Error);
                    return;
                }
                groupManager.DeleteAllPrivilages(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                this.Delete(ID);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                //pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtGroup.Focus();
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
            this.grdGroups.DataSource = groupManager.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdGroups.DataBind();
        }

        protected void GetByID()
        {
            grp = groupManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtGroup.Text = grp.GroupName.ToString();
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update()
        {
            grp = groupManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grp.GroupName = txtGroup.Text.Trim();
            if (!groupManager.ISAlreadyExist(grp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                groupManager.Update((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "userGroupAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                //pnlMain.Enabled = true;
            }

        }

        protected void Delete(int Id)
        {
            groupManager.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            grp = new tblAppGroup();
            grp.GroupName = this.txtGroup.Text.Trim();
            grp.Enabled = true;
            if (Session["UserID"] == null)
            {
                grp.UpdatedBy = Convert.ToInt32(Request.Cookies["uzr"]["UserID"].ToString());
            }
            else
            {
                grp.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
            }

            grp.UpdatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (!groupManager.ISAlreadyExist(grp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                groupManager.Insert(grp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "userGroupAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                //pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtGroup.Text = "";
            grdGroups.SelectedIndex = -1;
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtGroup.Focus();

        }

        #endregion
    }
}

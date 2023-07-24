using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class PromotionArtifacts : BasePage
    {
        #region DataMembers

        tblSdPromArt artifact;
        RMS.BL.ArtifactsBL artifactBL = new RMS.BL.ArtifactsBL();

        #endregion

        #region Properties

#pragma warning disable CS0114 // 'PromotionArtifacts.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'PromotionArtifacts.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "PromotionArtifacts").ToString();
            if (!IsPostBack)
            {
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                BindGrid();
                BindDropDown();
                txtPromotionArtifacts.Focus();
            }
        }

        protected void grtArtifact_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grtArtifact.SelectedDataKey.Values["ArtId"]);
                ddlArtifactType.SelectedValue = Convert.ToString(grtArtifact.SelectedDataKey.Values["ArtTypeId"]);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtPromotionArtifacts.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grtArtifact_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grtArtifact.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtPromotionArtifacts.Focus();
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
            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtPromotionArtifacts.Focus();
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
            this.grtArtifact.DataSource = artifactBL.GetAllArtifactsWithType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grtArtifact.DataBind();
        }

        protected void BindDropDown()
        {
            ddlArtifactType.DataValueField = "ArtTypeId";
            ddlArtifactType.DataTextField = "Desc";
            //ddlArtifactType.DataSource = artifactBL.GetAllArtifactTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlArtifactType.DataBind();
        }

        protected void GetByID()
        {
            artifact = artifactBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtPromotionArtifacts.Text = artifact.Desc.ToString();
            this.chkEnabled.Checked = Convert.ToBoolean(artifact.Enabled);
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update()
        {
            artifact = artifactBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
           // artifact.ArtTypeId = Convert.ToInt32(ddlArtifactType.SelectedValue);
            artifact.Desc = txtPromotionArtifacts.Text.Trim();
            artifact.Enabled = chkEnabled.Checked;

            if (!artifactBL.ISAlreadyExist(artifact, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                artifactBL.Update((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Promotion artifact updated successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage("Promotion artifact already exists", RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

        }

        protected void Delete(int Id)
        {
            //artifactBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            artifact = new tblSdPromArt();
            //artifact.ArtTypeId = Convert.ToInt32(ddlArtifactType.SelectedValue);
            artifact.Desc = this.txtPromotionArtifacts.Text.Trim();
            artifact.Enabled = this.chkEnabled.Checked;
            artifact.CreatedBy = Convert.ToString(Session["UserID"]);
            artifact.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (!artifactBL.ISAlreadyExist(artifact, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                artifactBL.Insert(artifact, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Promotion artifact inserted successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage("Promotion artifact already exists", RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtPromotionArtifacts.Text = string.Empty;
            ddlArtifactType.SelectedIndex = 0;
            chkEnabled.Checked = true;
            grtArtifact.SelectedIndex = -1;
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtPromotionArtifacts.Focus();

        }

        #endregion
    }
}

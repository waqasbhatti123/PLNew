using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class ArtifactsType : BasePage
    {
        #region DataMembers

        //tblSdPromArtType artifactType;
        RMS.BL.ArtifactsBL artifactsBL = new RMS.BL.ArtifactsBL();

        #endregion

        #region Properties

#pragma warning disable CS0114 // 'ArtifactsType.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'ArtifactsType.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ArtifactsType").ToString();
            if (!IsPostBack)
            {
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                txtArtifactType.Focus();
            }
        }

        protected void grdArtifactsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdArtifactsType.SelectedDataKey.Value);
                //this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtArtifactType.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdArtifactsType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdArtifactsType.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtArtifactType.Focus();
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

            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtArtifactType.Focus();
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
            //this.grdArtifactsType.DataSource = artifactsBL.GetAllArtifactTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdArtifactsType.DataBind();
        }

        //protected void GetByID()
        //{
        //    artifactType = artifactsBL.GetArtifactTypeByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    this.txtArtifactType.Text = artifactType.Desc.ToString();
        //    this.chkEnabled.Checked = Convert.ToBoolean(artifactType.Enabled);
        //    ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        //}

        //protected void Update()
        //{
        //    artifactType = artifactsBL.GetArtifactTypeByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    artifactType.Desc = txtArtifactType.Text.Trim();
        //    artifactType.Enabled = Convert.ToBoolean(chkEnabled.Checked);
        //    if (!artifactsBL.ISAlreadyExistArtifactType(artifactType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
        //    {
        //        artifactsBL.Update((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        ucMessage.ShowMessage("Artifact type updated successfully", RMS.BL.Enums.MessageType.Info);
        //        BindGrid();
        //        ClearFields();
        //    }
        //    else
        //    {
        //        ucMessage.ShowMessage("Artifact type already exists", RMS.BL.Enums.MessageType.Error);
        //        pnlMain.Enabled = true;
        //    }

        //}

        protected void Delete(int Id)
        {
            //artifactsBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        //protected void Insert()
        //{
        //    artifactType = new tblSdPromArtType();
        //    artifactType.Desc = this.txtArtifactType.Text.Trim();
        //    artifactType.Enabled = this.chkEnabled.Checked;
        //    artifactType.CreatedBy = Convert.ToString(Session["UserID"]);
        //    artifactType.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    if (!artifactsBL.ISAlreadyExistArtifactType(artifactType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
        //    {
        //        artifactsBL.InsertArtifactType(artifactType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        ucMessage.ShowMessage("Artifact type inserted successfully", RMS.BL.Enums.MessageType.Info);
        //        BindGrid();
        //        ClearFields();
        //    }
        //    else
        //    {
        //        ucMessage.ShowMessage("Artifact type already exists", RMS.BL.Enums.MessageType.Error);
        //        pnlMain.Enabled = true;
        //    }
        //}

        private void ClearFields()
        {
            txtArtifactType.Text = string.Empty;
            grdArtifactsType.SelectedIndex = -1;
            chkEnabled.Checked = true;
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtArtifactType.Focus();

        }

        #endregion
    }
}

using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class ResponseType : BasePage
    {
        #region DataMembers

        tblSdPromRespType responseType;
        RMS.BL.ResponseBL responseBL = new RMS.BL.ResponseBL();

        #endregion

        #region Properties

#pragma warning disable CS0114 // 'ResponseType.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'ResponseType.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ResponseType").ToString();
            if (!IsPostBack)
            {
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                txtResponseType.Focus();
            }
        }

        protected void grdResponseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdResponseType.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtResponseType.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdResponseType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResponseType.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtResponseType.Focus();
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
                txtResponseType.Focus();
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
            this.grdResponseType.DataSource = responseBL.GetAllResponseTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdResponseType.DataBind();
        }

        protected void GetByID()
        {
            responseType = responseBL.GetResponseTypeByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtResponseType.Text = responseType.Desc.ToString();
            this.chkEnabled.Checked = Convert.ToBoolean(responseType.Enabled);
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update()
        {
            responseType = responseBL.GetResponseTypeByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            responseType.Desc = txtResponseType.Text.Trim();
            responseType.Enabled = Convert.ToBoolean(chkEnabled.Checked);
            if (!responseBL.ISAlreadyExistResponseType(responseType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                responseBL.Update((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Response type updated successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage("Response type already exists", RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

        }

        protected void Delete(int Id)
        {
            //responseBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            responseType = new tblSdPromRespType();
            responseType.Desc = this.txtResponseType.Text.Trim();
            responseType.Enabled = this.chkEnabled.Checked;
            responseType.CreatedBy = Convert.ToString(Session["UserID"]);
            responseType.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (!responseBL.ISAlreadyExistResponseType(responseType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                responseBL.InsertResponseType(responseType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Response type inserted successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage("Response type already exists", RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtResponseType.Text = "";
            grdResponseType.SelectedIndex = -1;
            chkEnabled.Checked = true;
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtResponseType.Focus();

        }

        #endregion
    }
}

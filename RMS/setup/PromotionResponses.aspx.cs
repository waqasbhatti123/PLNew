using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class PromotionResponses : BasePage
    {
        #region DataMembers

        tblSdPromResp response;
        RMS.BL.ResponseBL responseBL = new RMS.BL.ResponseBL();

        #endregion

        #region Properties

#pragma warning disable CS0114 // 'PromotionResponses.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'PromotionResponses.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "PromotionResponse").ToString();
            if (!IsPostBack)
            {
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                BindDropDown();
                txtResponse.Focus();
                
            }
        }

        protected void grdResponse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdResponse.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtResponse.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdResponse_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResponse.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtResponse.Focus();
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
                txtResponse.Focus();
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
            this.grdResponse.DataSource = responseBL.GetAllResponses((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdResponse.DataBind();
        }

        protected void BindDropDown()
        {
            this.ddlResponseType.DataValueField = "RespTypeId";
            this.ddlResponseType.DataTextField = "Desc";
            this.ddlResponseType.DataSource = responseBL.GetAllResponseTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlResponseType.DataBind();
        }

        protected void GetByID()
        {
            response = responseBL.GetResponseByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtResponse.Text = response.Desc.ToString();
            txtRespCode.Text = response.Response;
            ddlResponseType.SelectedValue = response.RespTypeId.ToString();
            this.chkCritical.Checked = Convert.ToBoolean(response.Critical);
            this.chkEnabled.Checked = Convert.ToBoolean(response.Enabled);
            
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update()
        {
            response = responseBL.GetResponseByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            response.Desc = txtResponse.Text.Trim();
            response.Response = txtRespCode.Text.Trim();
            response.RespTypeId = Convert.ToInt32(ddlResponseType.SelectedValue);
            response.Critical = Convert.ToBoolean(chkCritical.Checked);
            response.Enabled = Convert.ToBoolean(chkEnabled.Checked);
            

            if (!responseBL.ISAlreadyExistResponse(response, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                responseBL.Update((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Response updated successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage("Response already exists", RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

        }

        protected void Delete(int Id)
        {
            //responseBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            response = new tblSdPromResp();
            response.Desc = this.txtResponse.Text.Trim();
            response.RespTypeId = Convert.ToInt32(ddlResponseType.SelectedValue);
            response.Critical = Convert.ToBoolean(chkCritical.Checked);
            response.Enabled = this.chkEnabled.Checked;
            response.Response = txtRespCode.Text.Trim();
            response.CreatedBy = Convert.ToString(Session["UserID"]);
            response.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (!responseBL.ISAlreadyExistResponse(response, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                responseBL.InsertResponse(response, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Response inserted successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage("Response already exists", RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtResponse.Text = "";
            txtRespCode.Text = "";
            grdResponse.SelectedIndex = -1;
            ddlResponseType.SelectedValue = "0";
            chkEnabled.Checked = true;
            chkCritical.Checked = false;
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtResponse.Focus();

        }

        #endregion
    }
}

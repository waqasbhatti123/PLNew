using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using RMS.BL;
using System.Web.UI.WebControls;

namespace RMS.GL.Setup
{
    public partial class frmGl_Type : BasePage
    {

        #region DataMembers
        Gl_Type cty;
        GlTypeBL ctyBL = new RMS.BL.GlTypeBL();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'frmGl_Type.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'frmGl_Type.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }

        }

        public char Code
        {
            get { return Convert.ToChar(ViewState["code"]); }
            set { ViewState["code"] = value; }

        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            int BrId = 0;
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"]);
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GlType").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                this.txtCode.Focus();

            }

        }

        protected void grdtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = 1;
                Code = Convert.ToChar(grdtype.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                this.txtCode.Enabled = false;
                this.txtName.Focus();
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }

        protected void grdtype_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdtype.PageIndex = e.NewPageIndex;
            BindGrid();
        }


        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                //pnlMain.Enabled = true;
                this.txtCode.Focus();
            }
            else if (e.CommandName == "Save")
            {
                if (Code == 0)
                {
                    this.Insert();
                }
                else
                {
                    this.Update(Code);
                }
            }
            else if (e.CommandName == "Edit")
            {
                //pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                this.txtCode.Focus();

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
            this.grdtype.DataSource = ctyBL.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdtype.DataBind();
        }

        protected void GetByID()
        {
            cty = ctyBL.GetByID(Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            
            this.txtCode.Text = cty.gt_cd.ToString();
            this.txtName.Text = cty.gt_dsc.ToString();
            this.chkbalance.Checked = cty.gt_cf;

        }


        protected void Insert()
        {
            RMS.BL.Gl_Type ctyR = new RMS.BL.Gl_Type();
            ctyR.gt_cd = Convert.ToString(this.txtCode.Text.Trim());
            ctyR.gt_dsc = this.txtName.Text.Trim();
            ctyR.gt_cf = chkbalance.Checked;
            if (!ctyBL.ISAlreadyExist(ctyR, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                ctyBL.Insert(ctyR, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "cityAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                //pnlMain.Enabled = true;
            }
        }

        protected void Update(char code)
        {
            cty = ctyBL.GetByID(code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            cty.gt_dsc = this.txtName.Text.Trim();
            cty.gt_cf = this.chkbalance.Checked;
            if (!ctyBL.ISAlreadyExist(cty, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                ctyBL.Update(cty, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "cityAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                //pnlMain.Enabled = true;
            }

        }


        private void ClearFields()
        {
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.chkbalance.Checked=false;
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            grdtype.SelectedIndex = -1;
            this.txtCode.Enabled = true;
            this.txtCode.Focus();

        }

        #endregion

    }
}

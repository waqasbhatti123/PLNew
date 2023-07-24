using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class UOM : BasePage
    {
        #region DataMembers
        
        Item_Uom uom;
        RMS.BL.UomBL uomBL = new RMS.BL.UomBL();
        
        #endregion

        #region Properties
        
#pragma warning disable CS0114 // 'UOM.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'UOM.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "UOM").ToString();
            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                txtUOM.Focus();
            }
        }

        protected void grdUOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdUOM.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtUOM.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdUOM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUOM.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtUOM.Focus();
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
            //else if (e.CommandName == "Delete")
            //{
            //    if (uomBL.hasEnabledPrivilage(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //    {
            //        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletionDependency").ToString(), RMS.BL.Enums.MessageType.Error);
            //        return;
            //    }
            //    uomBL.DeleteAllPrivilages(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //    this.Delete(ID);
            //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
            //    BindGrid();
            //    ClearFields();

            //}
            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtUOM.Focus();
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
            this.grdUOM.DataSource = uomBL.GetAllUom((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdUOM.DataBind();
        }

        protected void GetByID()
        {
            uom = uomBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtUOM.Text = uom.uom_dsc.ToString();
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update()
        {
            uom = uomBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            uom.uom_dsc = txtUOM.Text.Trim();
            if (!uomBL.ISAlreadyExist(uom, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                uomBL.Update((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Unit of measure updated successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage("Unit of measure already exists", RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

        }

        protected void Delete(int Id)
        {
            //uomBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            uom = new Item_Uom();
            uom.uom_dsc = this.txtUOM.Text.Trim();
           

            if (!uomBL.ISAlreadyExist(uom, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                uomBL.Insert(uom, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Unit of measure inserted successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage("Unit of measure already exists", RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtUOM.Text = "";
            grdUOM.SelectedIndex = -1;
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtUOM.Focus();

        }

        #endregion
    }
}

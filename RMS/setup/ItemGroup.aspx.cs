using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class ItemGroup : BasePage
    {
        #region DataMembers
        ItemGroupBL blItmGroup = new ItemGroupBL();
        tblItem_Group gItem = new tblItem_Group();

        TaxBL blTax = new TaxBL();
        tblTaxRate taxRate = new tblTaxRate();
        
        #endregion

        #region Properties
       
         public int ItmID
        {
            get { return (ViewState["ItmID"] == null) ? 0 : Convert.ToInt32(ViewState["ItmID"]); }
            set { ViewState["ItmID"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "IG").ToString();
            if (!IsPostBack)
            {
                BindGrid();
                ClearFields();
                txtItemDesc.Focus();
            }
        }
        protected void grdItmGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text.Trim() == "True")
                {
                    e.Row.Cells[1].Text = "Active";
                }
                else
                {
                    e.Row.Cells[1].Text = "InActive";
                }
            }
        }
        protected void grdItmGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ItmID = Convert.ToInt32(grdItmGroup.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                 txtItemDesc.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdItmGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           grdItmGroup.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            
            if (e.CommandName == "New")
            {
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
            }
            else if (e.CommandName == "Save")
            {
                if (ItmID == 0)
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
                txtItemDesc.Focus();
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
            this.grdItmGroup.DataSource = blItmGroup.BindGrid((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdItmGroup.DataBind();
        }

        protected void GetByID()
        {
            gItem = blItmGroup.GetByID(ItmID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtItemDesc.Text = gItem.itm_grp_desc.ToString();
            this.ddlStatus.SelectedValue = gItem.status == true ? "0" : "1";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }
       
        protected void Update()
        {
            
                 gItem= blItmGroup.GetByID(ItmID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                 gItem.itm_grp_desc = txtItemDesc.Text.Trim();
                 gItem.status = ddlStatus.SelectedValue == "0" ? true : false;
                 if (Session["UserID"] == null)
                 {
                     gItem.updatedby = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                 }
                 else
                 {
                     gItem.updatedby = Convert.ToInt32(Session["UserID"].ToString());
                 }
                 gItem.updatedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                blItmGroup.UpdateItmGroup((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(" Updated successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
        }


        protected void Insert()
        {
                gItem.itm_grp_desc = txtItemDesc.Text;
                gItem.status = ddlStatus.SelectedValue == "0" ? true : false;
                if (Session["UserID"] == null)
                {
                    gItem.createdby = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                }
                else
                {
                    gItem.createdby = Convert.ToInt32(Session["UserID"]);
                }
                gItem.createdon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                blItmGroup.InsertOnSubmit(gItem, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Inserted successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
                
                
        }

        private void ClearFields()
        {
            txtItemDesc.Text = "";
            ddlStatus.SelectedIndex = -1;
            ItmID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

        }
       
        #endregion
    }
}

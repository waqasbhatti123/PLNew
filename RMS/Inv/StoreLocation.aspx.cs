using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.Linq;

namespace RMS.Inv
{
    public partial class StoreLocation : BasePage
    {
        #region DataMembers

        StoreLocBL stkBL = new StoreLocBL();

        #endregion

        #region Properties

       public int BrID
       {
           get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
           set { ViewState["BrID"] = value; }
       }

#pragma warning disable CS0114 // 'StoreLocation.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'StoreLocation.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public string Code
        {
            get { return ViewState["Code"].ToString(); }
            set { ViewState["Code"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }

            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrID = Convert.ToInt32(Session["BranchID"].ToString());
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "StoreLoc").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                ID = 0;
                this.txtLocID.Text = stkBL.GetLocID(BrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
                this.Code = "";
                
                this.txtLocCode.Focus();
            }
            this.BindGrid();
        }

        protected void grdStockLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdStockLoc.SelectedDataKey.Values[0].ToString().Trim());
                BrID = Convert.ToInt32(grdStockLoc.SelectedDataKey.Values[1].ToString().Trim());

                this.GetByID(BrID, ID);
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }

        protected void grdStockLoc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdStockLoc.PageIndex = e.NewPageIndex;
            this.BindGrid();
        }

        protected void grdStockLoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text.Equals("M"))
                {
                    e.Row.Cells[4].Text = "Main Store";
                }
                else if(e.Row.Cells[4].Text.Equals("S"))
                {
                    e.Row.Cells[4].Text = "Sub Store";
                }
                else if(e.Row.Cells[4].Text.Equals("P"))
                {
                    e.Row.Cells[4].Text = "Project Store";
                }

                if (e.Row.Cells[5].Text.Equals("A"))
                {
                    e.Row.Cells[5].Text = "Active";
                }
                else if (e.Row.Cells[5].Text.Equals("I"))
                {
                    e.Row.Cells[5].Text = "Inactive";
                }
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                this.txtLocCode.Focus();
            }
            else if (e.CommandName == "Save")
            {
                if (ID == 0)
                {
                    this.Insert();
                }
                else
                {
                    this.Update(BrID, ID);
                }
                this.BindGrid();
            }
            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                this.txtLocCode.Focus();


            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
                this.BindGrid();

            }
        }

        #endregion

        #region Helping Method

        protected void BindGrid()
        {
            this.grdStockLoc.DataSource = stkBL.GetAllStockLoc(BrID,
                txtSrchLocCode.Text, txtSrchLocName.Text,
                ddlSrchStoreCat.SelectedValue == "0" ? ' ' : Convert.ToChar(ddlSrchStoreCat.SelectedValue), 
                ddlSrchStatus.SelectedValue == "0" ? ' ' : Convert.ToChar(ddlSrchStatus.SelectedValue),
                (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdStockLoc.DataBind();
        }

        private void ClearFields()
        {
            this.Code = "";
            this.ID = 0;
            this.txtLocID.Text = stkBL.GetLocID(BrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
            
            this.txtLocCode.Text = "";
            this.txtLocName.Text = "";
            this.txtLocAdd.Text = "";

            this.ddlStoreCat.SelectedValue = "0";
            this.ddlStatus.SelectedValue = "0";

            this.grdStockLoc.SelectedIndex = -1;

            this.txtLocCode.Focus();
        }

        protected void GetByID(int brid, int id)
        {
            RMS.BL.tblStock_Loc stkLoc = stkBL.GetStockByID(brid, id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            this.txtLocID.Text = stkLoc.LocId.ToString();
            this.txtLocCode.Text = stkLoc.LocCode;
            this.txtLocName.Text = stkLoc.LocName;
            this.txtLocAdd.Text = stkLoc.LocAddress;

            this.ddlStoreCat.SelectedValue = stkLoc.LocCategory.ToString();
            this.ddlStatus.SelectedValue = stkLoc.Status.ToString();

        }

        protected void Insert()
        {
            try
            {
                RMS.BL.tblStock_Loc stkLoc = new RMS.BL.tblStock_Loc();
                stkLoc.br_id = BrID;
                stkLoc.LocId = Convert.ToInt16(this.txtLocID.Text.Trim());
                stkLoc.LocCode = this.txtLocCode.Text;
                stkLoc.LocName = this.txtLocName.Text.Trim();
                stkLoc.LocAddress = this.txtLocAdd.Text.Trim();
                stkLoc.LocCategory = Convert.ToString(this.ddlStoreCat.SelectedValue);
                stkLoc.Remarks = null;
                stkLoc.Status = Convert.ToString(this.ddlStatus.SelectedValue);
                stkLoc.Updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (Session["UserID"] == null)
                    stkLoc.UpdateBy = Request.Cookies["uzr"]["UserID"].ToString();
                else
                    stkLoc.UpdateBy = Session["UserID"].ToString();

                stkBL.InsertStockLoc(stkLoc, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ClearFields();
                ucMessage.ShowMessage("Store location inserted successfully", RMS.BL.Enums.MessageType.Info);
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void Update(int brid, int id)
        {
            try
            {
                RMS.BL.tblStock_Loc stkLoc = stkBL.GetStockByID(brid, id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //stkLoc.br_id = BrID;
                //stkLoc.LocId = Convert.ToInt16(this.txtLocID.Text.Trim());
                stkLoc.LocCode = this.txtLocCode.Text;
                stkLoc.LocName = this.txtLocName.Text.Trim();
                stkLoc.LocAddress = this.txtLocAdd.Text.Trim();
                stkLoc.LocCategory = Convert.ToString(this.ddlStoreCat.SelectedValue);
                stkLoc.Remarks = null;
                stkLoc.Status = Convert.ToString(this.ddlStatus.SelectedValue);
                stkLoc.Updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (Session["UserID"] == null)
                    stkLoc.UpdateBy = Request.Cookies["uzr"]["UserID"].ToString();
                else
                    stkLoc.UpdateBy = Session["UserID"].ToString();

                stkBL.UpdateStockLoc(stkLoc, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ClearFields();
                ucMessage.ShowMessage("Store location updated successfully", RMS.BL.Enums.MessageType.Info);
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        
        #endregion
    }
}

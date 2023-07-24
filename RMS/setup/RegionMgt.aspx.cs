using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text;


namespace RMS.Setup
{
    public partial class RegionMgt : BasePage
    {
        #region DataMembers
        
        tblPlCode plCode;
        PlCodeBL codeBL = new PlCodeBL();
        
        #endregion

        #region Properties
        
#pragma warning disable CS0114 // 'RegionMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'RegionMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Region").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                txtRegion.Focus();
            }
        }

        protected void grdRegs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdRegs.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtRegion.Focus();
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }

        protected void grdRegs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdRegs.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtRegion.Focus();
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
                txtRegion.Focus();

            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();

            }
        }

        protected void grdRegs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text.Equals("True"))
                {
                    e.Row.Cells[1].Text = "Enable";
                }
                else
                {
                    e.Row.Cells[1].Text = "Disable";
                }
            }
        }

        #endregion

        #region Helping Method

        protected void BindGrid()
        {
            this.grdRegs.DataSource = codeBL.GetAll4Grid(1,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdRegs.DataBind();
        }

        protected void GetByID()
        {
            plCode = codeBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtRegion.Text = plCode.CodeDesc.ToString();
            this.rblStatus.SelectedValue = plCode.Enabled == true ? "1" : "0";
        }

        protected void Update()
        {
            plCode = codeBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            plCode.CodeDesc = this.txtRegion.Text.Trim();
            plCode.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
            if (!codeBL.ISAlreadyExist(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                codeBL.Update(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "regionAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

        }

        protected void Delete()
        {
            //codeBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            plCode = new tblPlCode();
            plCode.CodeDesc = this.txtRegion.Text.Trim();
            plCode.CodeTypeID = 1;

            if (Session["CompID"] == null)
            {
                try
                {
                    plCode.CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"].ToString());
                }
                catch
                {
                    ucMessage.ShowMessage("Please login again, session is expired", RMS.BL.Enums.MessageType.Error); 
                    return; 
                }
            }
            else
            {
                plCode.CompID = Convert.ToByte(Session["CompID"].ToString());
            }

            plCode.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
            if (!codeBL.ISAlreadyExist(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                codeBL.Insert(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "regionAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtRegion.Text = "";
            ID = 0;
            rblStatus.SelectedValue = "1";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

            grdRegs.SelectedIndex = -1;
            txtRegion.Focus();

        }

        #endregion
    }
}
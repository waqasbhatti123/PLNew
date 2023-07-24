using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text;


namespace RMS.Setup
{
    public partial class DivisionMgt : BasePage
    {

        #region DataMembers
        
        tblPlCode plCode;
        PlCodeBL divBL = new RMS.BL.PlCodeBL();
        
        #endregion

        #region Properties

#pragma warning disable CS0114 // 'DivisionMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'DivisionMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Division").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                txtDivision.Focus();
            }
        }

        protected void grdDivisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdDivisions.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtDivision.Focus();
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }

        protected void grdDivisions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDivisions.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdDivisions_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtDivision.Focus();
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
                txtDivision.Focus();

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
            this.grdDivisions.DataSource = divBL.GetAll4Grid(2,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdDivisions.DataBind();
        }

        protected void GetByID()
        {
            plCode = divBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtDivision.Text = plCode.CodeDesc.ToString();
            this.rblStatus.SelectedValue = plCode.Enabled == true ? "1" : "0";
        }

        protected void Update()
        {
            plCode = divBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            plCode.CodeDesc = this.txtDivision.Text.Trim();
            plCode.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
            if (!divBL.ISAlreadyExist(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                divBL.Update(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "divisionAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
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
            plCode.CodeDesc = this.txtDivision.Text.Trim();
            plCode.CodeTypeID = 2;
            
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
            if (!divBL.ISAlreadyExist(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                divBL.Insert(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "divisionAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtDivision.Text = "";
            ID = 0;
            rblStatus.SelectedValue = "1";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

            grdDivisions.SelectedIndex = -1;
            txtDivision.Focus();
        }

        #endregion
    }
}
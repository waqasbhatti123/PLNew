using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text;


namespace RMS.Setup
{
    public partial class DesignationMgt : BasePage
    {
        #region DataMembers

        tblPlCode plCode;
        PlCodeBL desigBL = new RMS.BL.PlCodeBL();
        
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'DesignationMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'DesignationMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Designation").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                txtDesignation.Focus();
            }
        }

        protected void grdDesignations_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void grdDesignations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdDesignations.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtDesignation.Focus();
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }

        protected void grdDesignations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDesignations.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtDesignation.Focus();
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
                txtDesignation.Focus();

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
            this.grdDesignations.DataSource = desigBL.GetAll4Grid(4, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdDesignations.DataBind();
        }

        protected void GetByID()
        {
            plCode = desigBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtDesignation.Text = plCode.CodeDesc.ToString();
            txtsort.Text = plCode.sort.ToString();
            this.rblStatus.SelectedValue = plCode.Enabled == true ? "1" : "0";
        }

        protected void Update()
        {
            plCode = desigBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            plCode.CodeDesc = this.txtDesignation.Text.Trim();
            plCode.sort = Convert.ToInt32(txtsort.Text);
            plCode.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
            if (!desigBL.ISAlreadyExist(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                desigBL.Update(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "designationAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
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
            plCode.CodeDesc = this.txtDesignation.Text.Trim();
            
                plCode.sort = Convert.ToInt32(txtsort.Text);
            plCode.CodeTypeID = 4;
            
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
            if (!desigBL.ISAlreadyExist(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                desigBL.Insert(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "designationAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtDesignation.Text = "";
            ID = 0;
            rblStatus.SelectedValue = "1";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

            grdDesignations.SelectedIndex = -1;
            txtDesignation.Focus();
            txtsort.Text = "";
        }

        #endregion
    }
}
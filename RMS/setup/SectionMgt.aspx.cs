using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text;


namespace RMS.Setup
{
    public partial class SectionMgt : BasePage
    {
        #region DataMembers
        tblPlCode tblSecCode;
        //Country cntry = new Country();
        //RMS.BL.CountryBL cntryBL = new RMS.BL.CountryBL();
        
        PlCodeBL sectBL = new RMS.BL.PlCodeBL();
        
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'SectionMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'SectionMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }

        }

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Section").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                txtSection.Focus();

            }

        }


        protected void grdSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdSections.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtSection.Focus();
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }

        protected void grdSections_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void grdSections_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSections.PageIndex = e.NewPageIndex;
            BindGrid();
        }

      

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtSection.Focus();
            }
            else if (e.CommandName == "Save")
            {
                if (ID == 0)
                {
                    this.Insert();
                }
                else
                {
                    this.Update(ID);
                }
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    this.Delete(ID);
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

                        Session["errors"] = ex.Message;
                    Response.Redirect("~/home/Error.aspx");

                }

                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtSection.Focus();

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
            this.grdSections.DataSource = sectBL.GetAll4Grid(5, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdSections.DataBind();
        }

        protected void GetByID()
        {
            tblSecCode = sectBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //this.ddlCountry.SelectedValue = cty.CountryID.ToString();
            this.txtSection.Text = tblSecCode.CodeDesc.ToString();
            this.rblStatus.SelectedValue = tblSecCode.Enabled == true ? "1" : "0";
            //ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update(int Id)
        {
            tblSecCode = sectBL.GetByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            tblSecCode.CodeDesc = this.txtSection.Text.Trim();
            //cty.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
            tblSecCode.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
            if (!sectBL.ISAlreadyExist(tblSecCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                sectBL.Update(tblSecCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "SectionAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

        }

        protected void Delete(int Id)
        {
            //codeBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            tblSecCode = new RMS.BL.tblPlCode();
            tblSecCode.CodeDesc = this.txtSection.Text.Trim();
            tblSecCode.CodeTypeID = 5;
            tblSecCode.CompID = 1;
            //ctyR.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
            tblSecCode.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
            if (!sectBL.ISAlreadyExist(tblSecCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                sectBL.Insert(tblSecCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "SectionAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtSection.Text = "";
            ID = 0;
            rblStatus.SelectedValue = "1";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

            grdSections.SelectedIndex = -1;
            txtSection.Focus();

        }
        #endregion
    }
}
using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text;

namespace RMS.Setup
{
    public partial class CityMgt : BasePage
    {
        #region DataMembers
        tblCity cty;
        //Country cntry = new Country();
        //RMS.BL.CountryBL cntryBL = new RMS.BL.CountryBL();
        CityBL ctyBL = new RMS.BL.CityBL();
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'CityMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'CityMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "City").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                txtCity.Focus();

            }

        }


        protected void grdCitys_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdCitys.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtCity.Focus();
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }


        protected void grdCitys_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCitys.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdCitys_RowDataBound(object sender, GridViewRowEventArgs e)
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
                txtCity.Focus();
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
                txtCity.Focus();

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
            this.grdCitys.DataSource = ctyBL.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdCitys.DataBind();
        }

        protected void GetByID()
        {
            cty = ctyBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //this.ddlCountry.SelectedValue = cty.CountryID.ToString();
            this.txtCity.Text = cty.CityName.ToString();
            this.rblStatus.SelectedValue = cty.Enabled == true ? "1" : "0";
            //ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update(int Id)
        {
            cty = ctyBL.GetByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            cty.CityName = this.txtCity.Text.Trim();
            //cty.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
            cty.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
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
                pnlMain.Enabled = true;
            }

        }

        protected void Delete(int Id)
        {
            ctyBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            RMS.BL.tblCity ctyR = new RMS.BL.tblCity();
            ctyR.CityName = this.txtCity.Text.Trim();
            //ctyR.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
            ctyR.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
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
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtCity.Text = "";
            ID = 0;
            rblStatus.SelectedValue = "1";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

            grdCitys.SelectedIndex = -1;
            txtCity.Focus();

        }
        #endregion
    }
}
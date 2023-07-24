using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text;

namespace RMS.Setup
{
    public partial class LocationMgt : BasePage
    {
        #region DataMembers
        tblPlLocation loc;
        //Country cntry = new Country();
        //RMS.BL.CountryBL cntryBL = new RMS.BL.CountryBL();
        LocationBL locBL = new RMS.BL.LocationBL();
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'LocationMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'LocationMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Location").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                ddlCity.Focus();
                FillDropDownCity();

            }

        }


        protected void grdCitys_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdCitys.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                ddlCity.Focus();
                
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

        //protected void grdCitys_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (e.Row.Cells[1].Text.Equals("True"))
        //        {
        //            e.Row.Cells[1].Text = "Enable";
        //        }
        //        else
        //        {
        //            e.Row.Cells[1].Text = "Disable";
        //        }


        //    }


        //}

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                ddlCity.Focus();
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
                ddlCity.Focus();

            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();

            }
        }


        #endregion

        #region Helping Method
        protected void FillDropDownCity() 
        {
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityID";
            ddlCity.DataSource = locBL.GetAllCityCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCity.DataBind();
        }
        
        protected void BindGrid()
        {
            this.grdCitys.DataSource = locBL.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdCitys.DataBind();
        }

        protected void GetByID()
        {
            loc = locBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlCity.SelectedValue = loc.CityID.ToString();
            this.txtLocation.Text = loc.LocName.ToString();
           // this.rblStatus.SelectedValue = cty.Enabled == true ? "1" : "0";
            //ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update(int Id)
        {
            loc = locBL.GetByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            loc.LocName = this.txtLocation.Text.Trim();
            loc.CityID = Convert.ToInt32(ddlCity.SelectedValue);
            //cty.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
            if (!locBL.ISAlreadyExist(loc, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                locBL.Update(loc, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "locationAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

        }

        protected void Delete(int Id)
        {
            locBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            RMS.BL.tblPlLocation locL = new RMS.BL.tblPlLocation();
            locL.LocName = this.txtLocation.Text.Trim();
            locL.CityID = Convert.ToInt32(ddlCity.SelectedValue);
            if (!locBL.ISAlreadyExist(locL, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                locBL.Insert(locL, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "locationAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtLocation.Text = "";
            ID = 0;
            //rblStatus.SelectedValue = "1";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            ddlCity.SelectedValue = "0";
            grdCitys.SelectedIndex = -1;
            ddlCity.Focus();

        }
        #endregion
    }
}
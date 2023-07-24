using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.Linq;
using System.Collections.Generic;
namespace RMS.sales
{
    public partial class ItemRate : BasePage
    {
        #region DataMembers

        ItemRateBL rtBL = new ItemRateBL();
       
        #endregion

        #region Properties
       
#pragma warning disable CS0114 // 'ItemRate.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'ItemRate.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
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

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ItemRate").ToString();
                
                BindParties();
                this.BindGrid();
            }
        }
        protected void ddlParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void grdParty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSaleRates();
        }
        
        #endregion

        #region Helping Method

        private void BindGrid()
        {
            List<spItemRateResult> list = rtBL.GetItems(ddlParty.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdParty.DataSource = list;
            this.grdParty.DataBind();

            if (list != null && list.Count > 0 && list[0].EffDate != null && list[0].EffDate.ToString() != "1/1/1900 12:00:00 AM")
            {
                txtEffDate.Text = Convert.ToDateTime(list[0].EffDate).ToString("dd-MMM-yyyy");
            }
            else
            {
                txtEffDate.Text = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yyyy");
            }

        }
        private void BindParties()
        {
            ddlParty.DataTextField = "gl_dsc";
            ddlParty.DataValueField = "gl_cd";
            //ddlParty.DataSource = rtBL.GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlParty.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlParty.DataBind();
        }
        private void SaveSaleRates()
        {
            try
            {
                Convert.ToDateTime(txtEffDate.Text.Trim());
                if(Convert.ToDateTime(txtEffDate.Text.Trim() +" 23:59:59" ) < RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    ucMessage.ShowMessage("Effecitive date cannot be less than " + RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yyyy"), RMS.BL.Enums.MessageType.Error);
                    return;
                }
            }
            catch
            {
                ucMessage.ShowMessage("Invalid effective date", RMS.BL.Enums.MessageType.Error);
                return;
            }

            int userID=0;
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    userID = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                userID = Convert.ToInt32(Session["UserID"]);
            }

            EntitySet<tblSaleRate> enttySaleRate = new EntitySet<tblSaleRate>();
            tblSaleRate sRate;
            for (int i = 0; i < grdParty.Rows.Count; i++)
            {
                sRate = new tblSaleRate();

                sRate.PartyID = ddlParty.SelectedValue;
                sRate.ItemID = grdParty.DataKeys[i].Values[0].ToString();
                sRate.EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());
                sRate.SaleRate = Convert.ToDecimal(((TextBox)grdParty.Rows[i].FindControl("txtSaleRate")).Text.Trim());
                sRate.DiscountPC = 0;
                sRate.CreatedBy = userID;
                sRate.CreatedOn = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                sRate.Status = "OP";

                enttySaleRate.Add(sRate);
            }

            string msg = rtBL.Save(ddlParty.SelectedValue, enttySaleRate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (msg == "ok")
            {
                this.BindGrid();
                ucMessage.ShowMessage("Rates saved successfully", RMS.BL.Enums.MessageType.Info);
            }
            else
            {
                ucMessage.ShowMessage("Exception: "+ msg, RMS.BL.Enums.MessageType.Info);
            }
        }

        #endregion
    }
}

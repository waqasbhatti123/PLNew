using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class TaxRates : BasePage
    {
        #region DataMembers
        
#pragma warning disable CS0169 // The field 'TaxRates.uom' is never used
        Item_Uom uom;
#pragma warning restore CS0169 // The field 'TaxRates.uom' is never used
        RMS.BL.UomBL uomBL = new RMS.BL.UomBL();
        TaxBL blTax = new TaxBL();
        tblTaxRate taxRate = new tblTaxRate();
        
        #endregion

        #region Properties

        public string taxRateID
        {
            get { return (ViewState["taxRateID"] == null) ? null : (ViewState["taxRateID"]).ToString(); }
            set { ViewState["taxRateID"] = value; }
        }
         public string type
        {
            get { return (ViewState["type"] == null) ? null : (ViewState["type"]).ToString(); }
            set { ViewState["type"] = value; }
        }
        public string taxID
        {
            get { return (ViewState["taxID"] == null) ? null : (ViewState["taxID"]).ToString(); }
            set { ViewState["taxID"] = value; }
        }
        public string taxIdTaxType
        {
            get { return (ViewState["taxIdTaxType"] == null) ? null : (ViewState["taxIdTaxType"]).ToString(); }
            set { ViewState["taxIdTaxType"] = value; }
        }
        public DateTime efDate
        {
            get { return Convert.ToDateTime((ViewState["efDate"] == null) ? null : (ViewState["efDate"])); }
            set { ViewState["efDate"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "TR").ToString();
            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                
                if (Session["DateFormat"] == null)
                {
                    this.txtdtEffective.Format = Request.Cookies["uzr"]["DateFormat"];
                }
                else
                {
                    this.txtdtEffective.Format = Session["DateFormat"].ToString();
                }
                txtEffectiveDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(
                    System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGridTaxDesc();
                this.BindGridTaxRate();
                this.fillddlDescription();
                this.ClearFields();
                this.ClearTaxDescFields();
                //txtUOM.Focus();
            }
        }
        protected void grdTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                taxID = (grdTax.SelectedDataKey.Value).ToString();
                ddlTaxType.Enabled = false;
                this.GetByIDTaxDesc();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                ddlTaxType.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdTax_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           grdTax.PageIndex = e.NewPageIndex;
            BindGridTaxDesc();
        }
        protected void grdTaxRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTaxRate.PageIndex = e.NewPageIndex;
            BindGridTaxRate();
        }
        protected void Select_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                //Label lblVoucherTypeID = (Label)clickedRow.FindControl("lblVoucherTypeID");
                LinkButton btnlnk = (LinkButton)clickedRow.FindControl("btnEdit");
                 int index = clickedRow.RowIndex;
                 //string datakey = grdTaxRate.DataKeys[index].Value.ToString();
                 efDate = Convert.ToDateTime(grdTaxRate.DataKeys[index].Values[1]);
                 taxRateID = grdTaxRate.DataKeys[index].Values[0].ToString();
                 taxIdTaxType = grdTaxRate.DataKeys[index].Values[2].ToString();
                this.GetByIDTaxRate();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                //ddlTaxType.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void grdTaxRate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                DateTime dt = Convert.ToDateTime(grdTaxRate.DataKeys[e.Row.RowIndex].Values[1].ToString());
                e.Row.Cells[3].Text = e.Row.Cells[3].Text + "%";
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                DateTime dtCurent = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                if (dt >= dtCurent)
                {
                    btnEdit.Visible = true;
                }
                else
                {
                    btnEdit.Visible = false;
                }
            }

        }
        protected void ButtonCommand_click(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {
                string[] arr = ddlDesc.SelectedValue.Split(':');
                type = arr[1];
                try 
                {
                    Convert.ToDateTime(txtEffectiveDate.Text);
                }
                catch
                {
                    ucMessage.ShowMessage("Please Enter Correct Effective Date", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                
                if (efDate == Convert.ToDateTime("1-1-1900") && taxRateID == null)
                {
                    this.Insert();
                }
                else
                {
                    this.Update();
                }
            }
            else if (e.CommandName == "Cancel")
            {
                ClearTaxDescFields();
            }
        }
        public bool isCheck;
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            isCheck = true;
            if (e.CommandName == "New")
            {
                ClearTaxDescFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                //txtUOM.Focus();
            }
            else if (e.CommandName == "Save")
            {
                if (taxID == null)
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
                //txtUOM.Focus();
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }

        #endregion

        #region Helping Method

        protected void BindGridTaxDesc()
        {
            this.grdTax.DataSource = blTax.BindGridTaxDesc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdTax.DataBind();
        }

        protected void GetByIDTaxDesc()
        {
            tblTax tax = new tblTax();
            tax = blTax.GetByIDTaxDesc(taxID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtDesc.Text = tax.TaxDesc.ToString();
            this.ddlTaxType.SelectedValue = tax.Type;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        
        protected void GetByIDTaxRate()
        {
            tblTaxRate taxRate = new tblTaxRate();
            taxRate = blTax.GetByIDTaxRate(efDate, taxRateID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlStatus.SelectedValue = taxRate.Status;
            this.ddlDesc.SelectedValue = taxIdTaxType;
            this.txtRate.Text = (taxRate.TaxRate).ToString();
            this.txtEffectiveDate.Text = taxRate.EffDate.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
            
        }

        protected void Update()
        {
            if (isCheck)
            {
                tblTax tax = new tblTax();
                tax = blTax.GetByIDTaxDesc(taxID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                tax.TaxDesc = txtDesc.Text.Trim();
                tax.Type = ddlTaxType.SelectedValue;
                if (!blTax.IsAlreadyExist(tax, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                blTax.UpdateTaxDesc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(" updated successfully", RMS.BL.Enums.MessageType.Info);
                fillddlDescription();
                BindGridTaxDesc();
                BindGridTaxRate();
                ClearFields();
                }
                else
                {
                    ucMessage.ShowMessage("Tax Description already exists", RMS.BL.Enums.MessageType.Error);
                    pnlMain.Enabled = true;
                }

            }
            else
            {
                taxRate = blTax.GetByIDTaxRate(efDate,taxRateID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                taxRate.TaxRate = Convert.ToDecimal(txtRate.Text);
                taxRate.Status = ddlStatus.SelectedValue;
                taxRate.EffDate = Convert.ToDateTime(txtEffectiveDate.Text);
                blTax.UpdateTaxRate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(" updated successfully", RMS.BL.Enums.MessageType.Info);
                BindGridTaxRate();
                ClearTaxDescFields();
            }
        }

        protected void Delete(int Id)
        {
            //uomBL.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            if (isCheck)
            {
                tblTax tax = new tblTax();

                string tType = ddlTaxType.SelectedValue;
                string ob = blTax.GetByType(tType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();


                if (tType == "GST")
                {

                    tax.TaxID = "ST" + ob;
                }
                else
                {
                    tax.TaxID = "WHT" + ob;
                }
                tax.TaxDesc = txtDesc.Text;
                if (Session["UserID"] == null)
                {
                    tax.CreatedBy = Convert.ToInt32( Request.Cookies["uzr"]["UserID"]);
                }
                else
                {
                    tax.CreatedBy =Convert.ToInt32(Session["UserID"].ToString());
                }
                tax.CreatedBy = Convert.ToInt32(Session["UserID"].ToString());
                tax.Type = ddlTaxType.SelectedValue;
                tax.Status = "OP";
                tax.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (!blTax.IsAlreadyExist(tax, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                blTax.insertTaxDesc(tax, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("inserted successfully", RMS.BL.Enums.MessageType.Info);
                fillddlDescription();
                BindGridTaxDesc();
                ClearFields();
                }
                else
                {
                    ucMessage.ShowMessage("Tax Description already exists", RMS.BL.Enums.MessageType.Error);
                    pnlMain.Enabled = true;
                }
            }
            else 
            {
                string[] arr = ddlDesc.SelectedValue.Split(':');
                string tID = arr[0];
                
                bool maxEfDate = blTax.CheckEffectiveDate(type, Convert.ToDateTime(txtEffectiveDate.Text), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (maxEfDate)
                {
                    taxRate.TaxID = tID;
                    taxRate.TaxRate = Convert.ToDecimal(txtRate.Text);
                    taxRate.Status = ddlStatus.SelectedValue;
                    taxRate.CreatedBy = Convert.ToInt32(Session["UserID"].ToString());
                    taxRate.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    taxRate.EffDate = Convert.ToDateTime(txtEffectiveDate.Text);
                    blTax.insertTaxRate(taxRate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucMessage.ShowMessage("inserted successfully", RMS.BL.Enums.MessageType.Info);
                    BindGridTaxRate();
                    ClearTaxDescFields();
                }
                else
                {
                    ucMessage.ShowMessage("Effective Date Must be Greater Than Previous Effective Date",RMS.BL.Enums.MessageType.Error);
                }
            }
            isCheck = false;
        }

        private void ClearFields()
        {
            taxID = null;
            ddlTaxType.Enabled = true;
            txtDesc.Text = "";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

        }
        private void ClearTaxDescFields()
        {
           taxRateID = null;
           taxIdTaxType = null;
           type = null;
           ddlDesc.SelectedIndex = -1;
           efDate = Convert.ToDateTime("1-1-1900");
           txtEffectiveDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFormat"]);
           txtRate.Text = "";

        }

        /* ========= Shahbaz Work    ============ */

        public void fillddlDescription()
        {
            ddlDesc.Items.Clear();
            ListItem itm = new ListItem();
            itm.Value = "0";
            itm.Text = "Select Tax Description";
            ddlDesc.Items.Add(itm);
            var obj = new TaxBL().GetTaxDesc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlDesc.DataTextField = "TaxDesc";
            ddlDesc.DataValueField = "TaxIdTaxType";

            ddlDesc.DataSource = obj;
            ddlDesc.DataBind();
        }
        public void BindGridTaxRate()
        {
            grdTaxRate.DataSource = blTax.BindGridTaxRate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdTaxRate.DataBind();
        }
        #endregion
    }
}

using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Collections.Generic;
using System.Web;


namespace RMS.Setup
{
    public partial class DepartmentMgt : BasePage
    {

        #region DataMembers

        tblPlCode plCode;
        PlCodeBL departmentBL = new RMS.BL.PlCodeBL();
        voucherDetailBL objVoucher = new voucherDetailBL();
        
        #endregion

        #region Properties

#pragma warning disable CS0114 // 'DepartmentMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'DepartmentMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }

        }

        public int PID
        {
            get { return (ViewState["PID"] == null) ? 0 : Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }

        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Department").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                PID = Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 503 || PID == 904)//503 for Inventory, 904 for Sales
                {
                    pnlFields.Visible = false;
                }
                txtDepartment.Focus();
            }
        }

        protected void grdDepartments_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void grdDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdDepartments.SelectedDataKey.Value);
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtDepartment.Focus();
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }

        protected void grdDepartments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDepartments.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtDepartment.Focus();
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
                txtDepartment.Focus();

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
            this.grdDepartments.DataSource = departmentBL.GetAll4Grid(3, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdDepartments.DataBind();
        }

        protected void GetByID()
        {
            plCode = departmentBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.txtDepartment.Text = plCode.CodeDesc.ToString();
            this.rblStatus.SelectedValue = plCode.Enabled == true ? "1" : "0";
            try
            {
                hdnStaffSal.Value = plCode.SalaryExpAct;
                txtStaffSal.Text = plCode.SalaryExpAct +" - "+ objVoucher.GetGlmfCodeByID(plCode.SalaryExpAct, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
            }
            catch 
            {
                hdnStaffSal.Value = null;
                txtStaffSal.Text = ""; 
            }
            try
            {
                hdnSalPayable.Value = plCode.SalaryPayable;
                txtSalPayable.Text = plCode.SalaryPayable + " - " + objVoucher.GetGlmfCodeByID(plCode.SalaryPayable, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
            }
            catch
            {
                hdnSalPayable.Value = null;
                txtSalPayable.Text = "";
            }
            try
            {
                hdnITax.Value = plCode.TaxPayable;
                txtITax.Text = plCode.TaxPayable + " - " + objVoucher.GetGlmfCodeByID(plCode.TaxPayable, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
            }
            catch
            {
                hdnITax.Value = null;
                txtITax.Text = "";
            }
            try
            {
                hdnEOBI.Value = plCode.EOBIPayable;
                txtEOBI.Text = plCode.EOBIPayable + " - " + objVoucher.GetGlmfCodeByID(plCode.EOBIPayable, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
            }
            catch
            {
                hdnEOBI.Value = null;
                txtEOBI.Text = "";
            }
            try
            {
                hdnLoanAdv.Value = plCode.LoansPayable;
                txtLoanAdv.Text = plCode.LoansPayable + " - " + objVoucher.GetGlmfCodeByID(plCode.LoansPayable, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
            }
            catch
            {
                hdnLoanAdv.Value = null;
                txtLoanAdv.Text = "";
            }
            try
            {
                hdnMisc.Value = plCode.MiscPayable;
                txtMisc.Text = plCode.MiscPayable + " - " + objVoucher.GetGlmfCodeByID(plCode.MiscPayable, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
            }
            catch
            {
                hdnMisc.Value = null;
                txtMisc.Text = "";
            }

            try
            {
                hdnOtrDed.Value = plCode.OtrDed;
                txtOtrDed.Text = plCode.OtrDed + " - " + objVoucher.GetGlmfCodeByID(plCode.OtrDed, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
            }
            catch
            {
                hdnOtrDed.Value = null;
                txtOtrDed.Text = "";
            }
            try
            {
                hdnMiscDed.Value = plCode.MiscDed;
                txtMiscDed.Text = plCode.MiscDed + " - " + objVoucher.GetGlmfCodeByID(plCode.MiscDed, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
            }
            catch
            {
                hdnMiscDed.Value = null;
                txtMiscDed.Text = "";
            }
        }

        protected void Update()
        {
            plCode = departmentBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            plCode.CodeDesc = this.txtDepartment.Text.Trim();

            //==========================================================================
            plCode.Code = null;
            if (!string.IsNullOrEmpty(txtStaffSal.Text))
            {
                plCode.SalaryExpAct = hdnStaffSal.Value;
            }
            else
            {
                plCode.SalaryExpAct = null;
            }
            if (!string.IsNullOrEmpty(txtSalPayable.Text))
            {
                plCode.SalaryPayable = hdnSalPayable.Value;
            }
            else
            {
                plCode.SalaryPayable = null;
            }
            if (!string.IsNullOrEmpty(txtITax.Text))
            {
                plCode.TaxPayable = hdnITax.Value;
            }
            else
            {
                plCode.TaxPayable = null;
            }
            if (!string.IsNullOrEmpty(txtEOBI.Text))
            {
                plCode.EOBIPayable = hdnEOBI.Value;
            }
            else
            {
                plCode.EOBIPayable = null;
            }
            if (!string.IsNullOrEmpty(txtLoanAdv.Text))
            {
                plCode.LoansPayable = hdnLoanAdv.Value;
            }
            else
            {
                plCode.LoansPayable = null;
            }
            if (!string.IsNullOrEmpty(txtMisc.Text))
            {
                plCode.MiscPayable = hdnMisc.Value;
            }
            else
            {
                plCode.MiscPayable = null;
            }
            if (!string.IsNullOrEmpty(txtOtrDed.Text))
            {
                plCode.OtrDed = hdnOtrDed.Value;
            }
            else
            {
                plCode.OtrDed = null;
            }
            if (!string.IsNullOrEmpty(txtMiscDed.Text))
            {
                plCode.MiscDed = hdnMiscDed.Value;
            }
            else
            {
                plCode.MiscDed = null;
            }
            //==========================================================================

            plCode.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
            if (!departmentBL.ISAlreadyExist(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                departmentBL.Update(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "departmentAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
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
            plCode.CodeDesc = this.txtDepartment.Text.Trim();
            plCode.CodeTypeID = 3;

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

            //==========================================================================
            plCode.Code = null;
            if (!string.IsNullOrEmpty(txtStaffSal.Text))
            {
                plCode.SalaryExpAct = hdnStaffSal.Value;
            }
            else
            {
                plCode.SalaryExpAct = null;
            }
            if (!string.IsNullOrEmpty(txtSalPayable.Text))
            {
                plCode.SalaryPayable = hdnSalPayable.Value;
            }
            else
            {
                plCode.SalaryPayable = null;
            }
            if (!string.IsNullOrEmpty(txtITax.Text))
            {
                plCode.TaxPayable = hdnITax.Value;
            }
            else
            {
                plCode.TaxPayable = null;
            }
            if (!string.IsNullOrEmpty(txtEOBI.Text))
            {
                plCode.EOBIPayable = hdnEOBI.Value;
            }
            else
            {
                plCode.EOBIPayable = null;
            }
            if (!string.IsNullOrEmpty(txtLoanAdv.Text))
            {
                plCode.LoansPayable = hdnLoanAdv.Value;
            }
            else
            {
                plCode.LoansPayable = null;
            }
            if (!string.IsNullOrEmpty(txtMisc.Text))
            {
                plCode.MiscPayable = hdnMisc.Value;
            }
            else
            {
                plCode.MiscPayable = null;
            }
            if (!string.IsNullOrEmpty(txtOtrDed.Text))
            {
                plCode.OtrDed = hdnOtrDed.Value;
            }
            else
            {
                plCode.OtrDed = null;
            }
            if (!string.IsNullOrEmpty(txtMiscDed.Text))
            {
                plCode.MiscDed = hdnMiscDed.Value;
            }
            else
            {
                plCode.MiscDed = null;
            }
            //==========================================================================

            plCode.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;

            if (!departmentBL.ISAlreadyExist(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                departmentBL.Insert(plCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "departmentAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            txtDepartment.Text = "";
            txtStaffSal.Text = "";
            txtSalPayable.Text = "";
            txtITax.Text = "";
            txtEOBI.Text = "";
            txtLoanAdv.Text = "";
            txtMisc.Text = "";
            txtOtrDed.Text = "";
            txtMiscDed.Text = "";

            hdnStaffSal.Value = null;
            hdnSalPayable.Value = null;
            hdnITax.Value = null;
            hdnEOBI.Value = null;
            hdnLoanAdv.Value = null;
            hdnMisc.Value = null;
            hdnOtrDed.Value = null;
            hdnMiscDed.Value = null;

            ID = 0;
            rblStatus.SelectedValue = "1";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

            grdDepartments.SelectedIndex = -1;
            txtDepartment.Focus();
        }

        [WebMethod]
        public static List<spGetAllA_CResult> GetBranch(string code)
        {
            voucherDetailBL vrBl = new voucherDetailBL();
            List<spGetAllA_CResult> acc = vrBl.GetAllAcc(code, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return acc;
        }


        [WebMethod]
        public static List<spGetAllCtrlA_CResult> GetControlAccount(string code)
        {
            voucherDetailBL vrBl = new voucherDetailBL();
            List<spGetAllCtrlA_CResult> acc = vrBl.GetAllCtrlAcc(code, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return acc;
        }

        #endregion
    }
}
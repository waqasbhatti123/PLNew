using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace RMS.Setup
{
    public partial class SalarySummary : BasePage
    {

        #region DataMembers

        GlCodeBL glCodeBL = new GlCodeBL();
        SalarySummaryBL summaryBL = new SalarySummaryBL();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'SalarySummary.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'SalarySummary.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        
        public string Allowance
        {
            get { return (ViewState["Allowance"] == null) ? "" : Convert.ToString(ViewState["Allowance"]); }
            set { ViewState["Allowance"] = value; }
        }

        public string Deduction
        {
            get { return (ViewState["Deduction"] == null) ? "" : Convert.ToString(ViewState["Deduction"]); }
            set { ViewState["Deduction"] = value; }
        }

        #endregion

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {
            Allowance = "Allowance";
            Deduction = "Deduction";

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalarySummary").ToString();

                var period = Session["CurPayPeriod"].ToString();

                hTitle.InnerText = "Salary Summary Sheet for the month " + period.Substring(4,2) + "/" + period.Substring(0, 4);
                BindGrids();
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            List<SalarySummarySheet> allowances = new List<SalarySummarySheet>();
            SalarySummarySheet allowance;

            for (int i = 0; i < grdAllowances.Rows.Count; i++)
            {
                allowance = new SalarySummarySheet();
                allowance.Type = Allowance;
                allowance.SalPerd = Convert.ToDecimal(Session["CurPayPeriod"]);
                allowance.Account = ((System.Web.UI.WebControls.Label)grdAllowances.Rows[i].FindControl("txtcode")).Text.Trim();
                decimal value = 0;
                try
                {
                    value = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdAllowances.Rows[i].FindControl("txtAmount")).Text.Trim());
                }
                catch { }
                
                allowance.Amount = value;
                allowance.IsActive = true;
                allowance.CreatedBy = Convert.ToInt32(Session["UserID"]);
                allowance.CreatedOn = DateTime.Now;

                allowances.Add(allowance);
            }


            List<SalarySummarySheet> deductions = new List<SalarySummarySheet>();
            SalarySummarySheet deduction;

            for (int i = 0; i < grdDeductions.Rows.Count; i++)
            {
                deduction = new SalarySummarySheet();
                deduction.Type = Deduction;
                deduction.SalPerd = Convert.ToDecimal(Session["CurPayPeriod"]);
                deduction.Account = ((System.Web.UI.WebControls.Label)grdDeductions.Rows[i].FindControl("txtcode")).Text.Trim();
                decimal value = 0;
                try
                {
                    value = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdDeductions.Rows[i].FindControl("txtAmount")).Text.Trim());
                }
                catch { }

                deduction.Amount = value;
                deduction.IsActive = true;
                deduction.CreatedBy = Convert.ToInt32(Session["UserID"]);
                deduction.CreatedOn = DateTime.Now;

                deductions.Add(deduction);
            }

            summaryBL.Submit(Convert.ToDecimal(Session["CurPayPeriod"]), allowances, deductions, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) ;
        }

        #endregion

        #region Helping Method
        protected void BindGrids()
        {
            this.grdAllowances.DataSource = glCodeBL.GetSalarySummaryCodes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], Allowance, Convert.ToDecimal(Session["CurPayPeriod"]));
            this.grdAllowances.DataBind();

            this.grdDeductions.DataSource = glCodeBL.GetSalarySummaryCodes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], Deduction, Convert.ToDecimal(Session["CurPayPeriod"]));
            this.grdDeductions.DataBind();
        }
        private void FillDropDownLeaveType()
        {
            //ddlleaveType.DataTextField = "LeaveTypeDesc";
            //ddlleaveType.DataValueField = "leaveTypeID";
            //ddlleaveType.DataSource = mgtleave.GetAllLeaveTypeCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlleaveType.DataBind();
        }
        private void ClearFields()
        {
            ID = 0;
            //EffDateStr = "";
            //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            //txtStartDate.Text = "";
            //txtEndDate.Text = "";
            //ddlleaveType.SelectedIndex=0;
            //txtBasicPay.Text = "";
            //txtHouseRent.Text = "";
            //txtFuelAll.Text = "";
            //txtSplAll.Text = "";
            //txtUtilities.Text = "";
            //grdlev.SelectedIndex = -1;
            //EmpSrchUC.ClearFields();
            //EmpSrchUC.EditModeDataHide();
            //EmpSrchUC.Focus();
        }

        #endregion
    }
}

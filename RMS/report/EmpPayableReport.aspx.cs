using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Data.Linq;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace RMS.report
{
    public partial class EmpPayableReport : BasePage
    {

        #region DataMembers

        BL.SalaryBL SalBl = new RMS.BL.SalaryBL();
        BL.PlSalPayBL SalPayBl = new RMS.BL.PlSalPayBL();
        voucherDetailBL objVoucher = new voucherDetailBL();
        PreferenceBL prefBl = new PreferenceBL();

        #endregion

        #region Properties
        
#pragma warning disable CS0114 // 'EmpPayableReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpPayableReport.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public int MinSal
        {
            get { return (ViewState["MinSal"] == null) ? 0 : Convert.ToInt32(ViewState["MinSal"]); }
            set { ViewState["MinSal"] = value; }
        }
        
        public int MaxSal
        {
            get { return (ViewState["MaxSal"] == null) ? 0 : Convert.ToInt32(ViewState["MaxSal"]); }
            set { ViewState["MaxSal"] = value; }
        }

        public int BrID
        {
            get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
        }

        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        
        public string PayPeriod
        {
            get { return (ViewState["PayPeriod"] == null) ? "" : Convert.ToString(ViewState["PayPeriod"]); }
            set { ViewState["PayPeriod"] = value; }

        }
 

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "PayableReport").ToString();
        
                int iCompid;
                if (Session["CompID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    int.TryParse(Request.Cookies["uzr"]["CompID"], out iCompid);
                }
                else
                {
                    int.TryParse(Session["CompID"].ToString(), out iCompid);
                }
                CompID = iCompid;
                
                if (Session["BranchID"] == null)
                {
                    BrID = Convert.ToByte(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BrID = Convert.ToByte(Session["BranchID"].ToString());
                }

                FillDropDownPayPeriod();
                FillDropDownCodeDept();
                FillDDlBankBranch();

            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            PayPeriod = ddlPayPerd.SelectedValue;

            MinSal = 0;
            MaxSal = -1;
            try
            {

                if (!txtMinSal.Text.Equals(""))
                {
                    MinSal = Convert.ToInt32(txtMinSal.Text);
                }

                if (!txtMaxSal.Text.Equals(""))
                {
                    MaxSal = Convert.ToInt32(txtMaxSal.Text);
                }

                if (!MinSal.Equals(0) && MaxSal.Equals(-1))
                {
                    if (MaxSal < MinSal)
                    {
                        ucMessage.ShowMessage("Enter maximum salary", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                }

                if (!MinSal.Equals(0) && !MaxSal.Equals(-1))
                {
                    if (MaxSal < MinSal)
                    {
                        ucMessage.ShowMessage("Maximum salary cannot be less than minimum salary", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                }
                GetReport();
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
                return;
            }

        }

        #endregion

        #region Helping Method

        private void GetReport()
        {

            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            string payperiod = "";
            if(ddlPayPerd.SelectedValue.Equals(0))
                payperiod = "All";
            else
                payperiod = ddlPayPerd.SelectedValue;
            List<spSalaryPayableReportResult> empsSalPay = SalBl.GetSalPayable(CompID, Convert.ToInt32(ddlPayPerd.SelectedValue), Convert.ToInt32(ddlDept.SelectedValue), MinSal, MaxSal, ddlBank.SelectedValue, ddlJobType.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.EnableExternalImages = true;

            viewer.LocalReport.ReportPath = "report/rdlc/rptPayable.rdlc";
            ReportDataSource datasource = new ReportDataSource("spSalaryPayableReportResult", empsSalPay);

            ReportParameter[] paramz = new ReportParameter[3];
            paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", payperiod, false);

            if (Session["CompName"] == null)
            {
                paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[2] = new ReportParameter("LogoPath", rptLogoPath);
            
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.Focus();
        }

        private void FillDropDownCodeDept()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 3;

            this.ddlDept.DataTextField = "CodeDesc";
            ddlDept.DataValueField = "CodeID";
            ddlDept.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDept.DataBind();

        }

        private void FillDropDownPayPeriod()
        {
            this.ddlPayPerd.DataTextField = "PayPerd";
            ddlPayPerd.DataValueField = "PayPerd";
            ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlPayPerd.DataBind();
        }

        private void FillDDlBankBranch()
        {
            ddlBank.DataValueField = "BankCode";
            ddlBank.DataTextField = "BankBranchName";
            ddlBank.DataSource = SalPayBl.GetBankBranch((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlBank.DataBind();
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Script.Services;
using System.Web.Services;

namespace RMS.GL.Setup
{
    public partial class frmPreferences : BasePage
    {
        #region DataMembers
        Preference cty;
        PreferenceBL ctyBL = new RMS.BL.PreferenceBL();
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'frmPreferences.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'frmPreferences.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int InvID
        {
            get { return (ViewState["InvID"] == null) ? 0 : Convert.ToInt32(ViewState["InvID"]); }
            set { ViewState["InvID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Preference").ToString();
                this.fillControls();
                this.fillInvControls();
            }
        }


        protected void btnSaveCodes_Click(object sender, CommandEventArgs e)
        {
           
            this.GetByID();
            
            if (ID == 0)
            {
                this.Insert();
                //ClearFields();
                //DisableFields();
            }
            else
            {
                this.Update(ID);
                //DisableFields();
            }
            
        }

        protected void btnSaveInvent_Click(object sender, CommandEventArgs e)
        {
            
            this.GetByID();

            if (InvID == 0)
            {
                this.insertInventCode();
            }
            else
            {
                this.UpdateInventCode(InvID);
            }
        
        }
        #endregion

        #region Helping Method
        
        protected void GetByID()
        {
            cty = ctyBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (cty != null)
            {
                ID = cty.p_id;
                InvID = cty.p_id;
            }

        }


        protected void Insert()
        {
            RMS.BL.Preference ctyR = new RMS.BL.Preference();
            int Id = ctyBL.GetMaxID((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

             ctyR.p_id = Id;
             ctyR.ctrl_Cash = this.txtcashcode.Text;
             ctyR.ctrl_Bank = this.txtbankcode.Text;
             ctyR.ctrl_Vndr = this.txtvendor.Text;
             ctyR.ctrl_Dept = this.txtdepartment.Text;
             ctyR.ctrl_Cust = this.txtcustomer.Text;
             ctyR.Def_Bank_Acc = this.txtbankaccount.Text;
             ctyR.Def_Cash_Acc = this.txtcashaccount.Text;
            
             ctyR.vc_ReqAprv = this.chkapprove.Checked;
             ctyR.vc_AutoPrint = this.chkprint.Checked;
            cty.IncomeTax = txtIncomeTax.Text.Trim();
            cty.GSTTax = txtGSTTax.Text.Trim();
            cty.PRATax = txtPraTax.Text.Trim();
            cty.TempAccount = txtTempAcc.Text.Trim();

             ctyBL.Insert(ctyR, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
             ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
            
            
        }
        protected void insertInventCode()
        {
            Preference ctyInv = new Preference();
            ctyInv.InvCtrl_Amnt = this.txtAmntCode.Text;
            ctyInv.InvCtrl_CustDuty = this.txtCustCode.Text;
            ctyInv.InvCtrl_Freight = this.txtFreight.Text;
            ctyInv.InvCtrl_GST = this.txtgstCode.Text;
            ctyInv.InvCtrl_OtrCost = this.txtOtherCost.Text;
            ctyInv.InvCtrl_WHT = this.txtwhtCode.Text;
            ctyInv.InvCtrl_ImpFreight = this.txtImpFreight.Text;
            ctyBL.insertInventCode(ctyInv, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ucMessage1.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
        }
        protected void Update(int Id)
        {
            cty = ctyBL.GetByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            cty.ctrl_Cash = this.txtcashcode.Text;
            cty.ctrl_Bank = this.txtbankcode.Text;
            cty.ctrl_Vndr = this.txtvendor.Text;
            cty.ctrl_Dept = this.txtdepartment.Text;
            cty.ctrl_Cust = this.txtcustomer.Text;
            cty.Def_Bank_Acc = this.txtbankaccount.Text;
            cty.Def_Cash_Acc = this.txtcashaccount.Text;
            cty.vc_ReqAprv = this.chkapprove.Checked;
            cty.vc_AutoPrint = this.chkprint.Checked;
            cty.IncomeTax = txtIncomeTax.Text.Trim();
            cty.GSTTax = txtGSTTax.Text.Trim();
            cty.PRATax = txtPraTax.Text.Trim();
            cty.TempAccount = txtTempAcc.Text.Trim();
            ctyBL.Update(cty, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
        }
        public void UpdateInventCode(int invID)
        {
            cty = ctyBL.GetByID(invID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            cty.InvCtrl_Amnt = this.txtAmntCode.Text;
            cty.InvCtrl_CustDuty = this.txtCustCode.Text;
            cty.InvCtrl_Freight = this.txtFreight.Text;
            cty.InvCtrl_GST = this.txtgstCode.Text;
            cty.InvCtrl_OtrCost = this.txtOtherCost.Text;
            cty.InvCtrl_WHT = this.txtwhtCode.Text;
            cty.InvCtrl_ImpFreight = this.txtImpFreight.Text;
            ctyBL.UpdateInventCode(cty, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ucMessage1.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
        }

     //  [WebMethod(EnableSession = true)]
     //  [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public static List<string> GetControlAccount(string sname)
        {
            PreferenceBL cty = new PreferenceBL();
            return cty.GetControlAccount(sname, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);


        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static object GetDetailAccount1(string sname)
        {
            PreferenceBL cty = new PreferenceBL();
            return cty.GetDetailAccount1(sname, Convert.ToInt32(HttpContext.Current.Session["BranchID"]), (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            
        }

        


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<string> GetDetailAccount(string sname)
        {
            PreferenceBL cty = new PreferenceBL();
            return cty.GetDetailAccount(sname, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<string> GetCostCenter(string sname)
        {
            PreferenceBL cty = new PreferenceBL();
            return cty.GetCostCenter(sname, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
        }

        // **** Shahbaz Work ******

        public void fillControls()
        {
            Preference cty = ctyBL.GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (cty != null)
            {
                this.txtcashcode.Text = cty.ctrl_Cash;
                this.txtbankcode.Text = cty.ctrl_Bank;
                this.txtvendor.Text = cty.ctrl_Vndr;
                this.txtdepartment.Text = cty.ctrl_Dept;
                this.txtcustomer.Text = cty.ctrl_Cust;
                this.txtbankaccount.Text = cty.Def_Bank_Acc;
                this.txtcashaccount.Text = cty.Def_Cash_Acc;
                this.chkapprove.Checked = cty.vc_ReqAprv;
                this.chkprint.Checked = cty.vc_AutoPrint;
                this.txtIncomeTax.Text = cty.IncomeTax;
                this.txtGSTTax.Text = cty.GSTTax;
                this.txtPraTax.Text = cty.PRATax;
                this.txtTempAcc.Text = cty.TempAccount;
            }
        }

        public void fillInvControls()
        {
            Preference cty = ctyBL.GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (cty != null)
            {
                this.txtAmntCode.Text = cty.InvCtrl_Amnt;
                this.txtCustCode.Text = cty.InvCtrl_CustDuty;
                this.txtgstCode.Text = cty.InvCtrl_GST;
                this.txtFreight.Text = cty.InvCtrl_Freight;
                this.txtwhtCode.Text = cty.InvCtrl_WHT;
                this.txtOtherCost.Text = cty.InvCtrl_OtrCost;
                this.txtImpFreight.Text = cty.InvCtrl_ImpFreight;
            }
        }

        #endregion
    }
}

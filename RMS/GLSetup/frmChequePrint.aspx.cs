using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Script.Services;
using System.Web.Services;

namespace RMS.GL.Setup
{
    public partial class frmChequePrint : BasePage
    {
        #region DataMembers
        //Preference cty;

        //tblChequePrint cp;
       
        //PreferenceBL ctyBL = new RMS.BL.PreferenceBL();
        ChequePrintBL cpBL = new ChequePrintBL();
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'frmChequePrint.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'frmChequePrint.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ChequePrint").ToString();
                //this.fillControls();
                this.fillInvControls();
            }
        }


        protected void btnSaveCodes_Click(object sender, CommandEventArgs e)
        {
           
            this.GetByID();
            
            if (ID == 0)
            {
                //this.Insert();
                
                
                //ClearFields();
                //DisableFields();
            }
            else
            {
                //this.Update(ID);
                //DisableFields();
            }
            
        }

        protected void btnSaveInvent_Click(object sender, CommandEventArgs e)
        {
            
            this.GetByID();

            if (InvID == 0)
            {
                //this.insertInventCode();
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
            //cp = cpBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //if (cp != null)
            //{
            //    ID = cp.Id;
            //    InvID = cp.Id;
            //}

        }


        //protected void Insert()
        //{
        //    RMS.BL.tblChequePrint cp = new RMS.BL.tblChequePrint();
        //    int Id = cpBL.GetMaxID((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //     cp.Id = Id;
        //     cp.FontSize = Convert.ToInt16(txtFontSize.Text);
        //     cp.DateWordSpacing = Convert.ToInt16(txtDateWordSpacing.Text);
        //     cp.PayeeWordSpacing = Convert.ToInt16(txtPayeeWordSpacing.Text);
        //    //if (chkPayeeBold.Checked == true)
        //    //{
        //    //    cp.PayeeBold = Convert.ToBoolean(1);
        //    //}
        //    //else
        //    //{
        //        cp.PayeeBold = chkPayeeBold.Checked;
           
            
        //     cp.PayeePrefix = this.txtPayPrefix.Text;
        //     cp.PayeeSuffix = this.txtPaySuffix.Text;
        //     cp.DigitsPrefix= this.txtDigitsPrefix.Text;
            
        //     cp.DigitsSuffix = this.txtDigitsSuffix.Text;
        //     cp.AmoutPrefix = this.txtAmountPrefix.Text;
        //    cp.AmountSuffix = this.txtAmountSuffix.Text;
        //    cp.ChequeFeed = this.ddlfield.SelectedItem.ToString();
        //    cp.PixelsShiftLeft = Convert.ToInt16(txtPixelShiftLeft.Text);
        //    cp.PixelsShiftRight = Convert.ToInt16(txtPixelsRight.Text);
        //    cp.PixelsShiftUp = Convert.ToInt16(txtPixelsUp.Text);
        //    cp.PixelsShiftDown = Convert.ToInt16(txtPixelsDown.Text);
        //    cp.LineBreak = Convert.ToInt16(txtLineBreak.Text);

        //    cpBL.Insert(cp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //     ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
            
            
        //}
        //protected void insertInventCode()
        //{
        //    Preference ctyInv = new Preference();
        //    //ctyInv.InvCtrl_Amnt = this.txtAmntCode.Text;
        //    //ctyInv.InvCtrl_CustDuty = this.txtCustCode.Text;
        //    //ctyInv.InvCtrl_Freight = this.txtFreight.Text;
        //    //ctyInv.InvCtrl_GST = this.txtgstCode.Text;
        //    //ctyInv.InvCtrl_OtrCost = this.txtOtherCost.Text;
        //    //ctyInv.InvCtrl_WHT = this.txtwhtCode.Text;
        //    //ctyInv.InvCtrl_ImpFreight = this.txtImpFreight.Text;
        //    //ctyBL.insertInventCode(ctyInv, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
        //}
        //protected void Update(int Id)
        //{
        //    try {
        //        //RMS.BL.tblChequePrint cp = new RMS.BL.tblChequePrint();
        //        //int Id = cpBL.GetMaxID((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //        //cp.Id = Id;
        //        //string num = Number.ConvertNumberToWord(40004567);
        //        //char[] number = num.ToCharArray();


        //        //int count = 0;
        //        //string[] str1 = null;
        //        //for (int i = 0; i < number.Length; i++)
        //        //{
        //        //    if (i <= 45 )
        //        //    {
        //        //        if (number[i] == ' ')
        //        //        {
                            
        //        //            count=i;
        //        //        }
                       
        //        //             str1 =  num.Split(number);
        //        //            Response.Write(number[i]);
                        
                        
        //        //        //string[] subString1 =num.Split(number);
                        
        //        //    }
                   
        //        //    //else if(i>38 && i<=43 && number[i] != ' ')
        //        //    //{
        //        //    //    Response.Write("");
        //        //    //}

        //        //    else
        //        //    {
        //        //        //Response.Write(number[i]);
        //        //    }
        //        //}
       
        //        cp = cpBL.GetByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        cp.FontSize = Convert.ToInt16(txtFontSize.Text);
        //        cp.DateWordSpacing = Convert.ToInt16(txtDateWordSpacing.Text);
        //        cp.PayeeWordSpacing = Convert.ToInt16(txtPayeeWordSpacing.Text);
        //        //if (chkPayeeBold.Checked == true)
        //        //{
        //        //    cp.PayeeBold = Convert.ToBoolean(1);
        //        //}
        //        //else
        //        //{
        //        cp.PayeeBold = chkPayeeBold.Checked;


        //        cp.PayeePrefix = this.txtPayPrefix.Text;
        //        cp.PayeeSuffix = this.txtPaySuffix.Text;
        //        cp.DigitsPrefix = this.txtDigitsPrefix.Text;

        //        cp.DigitsSuffix = this.txtDigitsSuffix.Text;
        //        cp.AmoutPrefix = this.txtAmountPrefix.Text;
        //        cp.AmountSuffix = this.txtAmountSuffix.Text;
        //        cp.ChequeFeed = this.ddlfield.SelectedItem.Text;
        //        cp.PixelsShiftLeft = Convert.ToInt16(txtPixelShiftLeft.Text);
        //        cp.PixelsShiftRight = Convert.ToInt16(txtPixelsRight.Text);
        //        cp.PixelsShiftUp = Convert.ToInt16(txtPixelsUp.Text);
        //        cp.PixelsShiftDown = Convert.ToInt16(txtPixelsDown.Text);
        //        cp.LineBreak = Convert.ToInt16(txtLineBreak.Text);
        //        cpBL.Update(cp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
        //    }
        //    catch(Exception ex)
        //    {
        //        ucMessage.ShowMessage(ex.Message.ToString(), RMS.BL.Enums.MessageType.Info);
        //    }
        //}
      
        public void UpdateInventCode(int invID)
        {
            //cty = ctyBL.GetByID(invID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //cty.InvCtrl_Amnt = this.txtAmntCode.Text;
            //cty.InvCtrl_CustDuty = this.txtCustCode.Text;
            //cty.InvCtrl_Freight = this.txtFreight.Text;
            //cty.InvCtrl_GST = this.txtgstCode.Text;
            //cty.InvCtrl_OtrCost = this.txtOtherCost.Text;
            //cty.InvCtrl_WHT = this.txtwhtCode.Text;
            //cty.InvCtrl_ImpFreight = this.txtImpFreight.Text;
            //ctyBL.UpdateInventCode(cty, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
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

        //public void fillControls()
        //{
        //    tblChequePrint cp = cpBL.GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    if (cp != null)
        //    {
        //        this.txtFontSize.Text = Convert.ToString(cp.FontSize);
        //        this.txtDateWordSpacing.Text = Convert.ToString(cp.DateWordSpacing);
        //        this.txtPayeeWordSpacing.Text = Convert.ToString(cp.PayeeWordSpacing);
        //        int bold= Convert.ToInt32(cp.PayeeBold);
        //        if (bold == 1)
        //        {
        //            this.chkPayeeBold.Checked = true;
        //        }
        //        this.txtPayPrefix.Text = cp.PayeePrefix;
        //        this.txtPaySuffix.Text = cp.PayeeSuffix;
        //        this.txtDigitsPrefix.Text = cp.DigitsPrefix;
        //        //this.chkapprove.Checked = cty.vc_ReqAprv;
        //        this.txtDigitsSuffix.Text = cp.DigitsSuffix;
        //        this.txtAmountPrefix.Text = cp.AmoutPrefix;
        //        this.txtAmountSuffix.Text = cp.AmountSuffix;
        //        this.ddlfield.SelectedItem.Text = cp.ChequeFeed;
        //        this.txtPixelShiftLeft.Text = Convert.ToString(cp.PixelsShiftLeft);
        //        this.txtPixelsRight.Text = Convert.ToString(cp.PixelsShiftRight);
        //        this.txtPixelsUp.Text= Convert.ToString(cp.PixelsShiftUp);
        //        this.txtPixelsDown.Text= Convert.ToString(cp.PixelsShiftDown);
        //        this.txtLineBreak.Text= Convert.ToString(cp.LineBreak);

        //    }
        //}

        public void fillInvControls()
        {
            //Preference cty = ctyBL.GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //if (cty != null)
            //{
                //this.txtAmntCode.Text = cty.InvCtrl_Amnt;
                //this.txtCustCode.Text = cty.InvCtrl_CustDuty;
                //this.txtgstCode.Text = cty.InvCtrl_GST;
                //this.txtFreight.Text = cty.InvCtrl_Freight;
                //this.txtwhtCode.Text = cty.InvCtrl_WHT;
                //this.txtOtherCost.Text = cty.InvCtrl_OtrCost;
                //this.txtImpFreight.Text = cty.InvCtrl_ImpFreight;
           // }
        }

        #endregion
    }
}

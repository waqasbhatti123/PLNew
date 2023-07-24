using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Services;
using System.Collections.Generic;
using System.Web;

namespace RMS.Setup
{
    public partial class BankMgt : BasePage
    {
        #region DataMembers
       
        tblBank bnk;
        BankBL groupManager = new BankBL();
        voucherDetailBL vrDetBl = new voucherDetailBL();
        
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'BankMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public string ID
#pragma warning restore CS0114 // 'BankMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? "0" : ViewState["ID"].ToString(); }
            set { ViewState["ID"] = value; }

        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Banks").ToString();
            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                this.BindGrid();
                txtBankCode.Focus();
            }

        }





        protected void grdBanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ID = grdBanks.SelectedDataKey.Value.ToString();
                //this.Data_GetByID((int)this.grdBanks.SelectedDataKey.Value);
                GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtBankCode.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void grdBanks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBanks.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {

                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
               // txtBank.Focus();
            }
            else if (e.CommandName == "Save")
            {
                if (ID.Equals("0"))
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
                //if (groupManager.hasEnabledPrivilage(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                //{
                //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletionDependency").ToString(), RMS.BL.Enums.MessageType.Error);
                //    return;
                //}
                //groupManager.DeleteAllPrivilages(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //this.Delete(ID);
                //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
                //BindGrid();
                //ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
               // txtBank.Focus();
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
            IQueryable grps = groupManager.GetAll(BranchID,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdBanks.DataSource = grps;
            this.grdBanks.DataBind();
        }

        protected void GetByID()
        {
            bnk = groupManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
           // this.txtBank.Text = grp.BankName.ToString();
            this.txtBankCode.Text = bnk.BankCode.ToString();
            this.txtBankName.Text = bnk.BankName.ToString();
            this.txtBankAbbreviation.Text = bnk.BankABv.ToString();

            try
            {
                Glmf_Code glCode =  vrDetBl.GetGlmfCodeByID(bnk.GlAccCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                this.txtGlAccCode.Text = glCode.gl_cd + " - "+ glCode.gl_dsc;
            }
            catch 
            {
                this.txtGlAccCode.Text = bnk.GlAccCd;
            }
            hdnGlCode.Value = bnk.GlAccCd;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update()
        {
            bnk = groupManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //string oldBnkCode = bnk.BankCode;
            //bnk.BankCode = txtBankCode.Text.Trim();
            bnk.BankName = txtBankName.Text.Trim();
            bnk.BankABv = txtBankAbbreviation.Text.Trim();
            if (txtGlAccCode.Text != "")
                bnk.GlAccCd = hdnGlCode.Value;
            else
                bnk.GlAccCd = null;
           
            if (!groupManager.ISAlreadyExistUpd(bnk.BankCode,bnk.BankName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                groupManager.Update((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "userBankAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

        }

        //protected void Delete()
        //{
        //  //  groupManager.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //}

        protected void Insert()
        {
           bnk = new tblBank();
           
           // bnk.BankName = this.txtBank.Text.Trim();
           bnk.BankCode = this.txtBankCode.Text.Trim();
           bnk.BankName = this.txtBankName.Text.Trim();
           bnk.BankABv = this.txtBankAbbreviation.Text.Trim();
            bnk.brID = Convert.ToInt32(BranchID);
           if (txtGlAccCode.Text != "")
            bnk.GlAccCd = hdnGlCode.Value;
           else
               bnk.GlAccCd = null;
           

            if (!groupManager.ISAlreadyExist(bnk.BankCode,bnk.BankName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                groupManager.Insert(bnk, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "userBankAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
           // txtBank.Text = "";
            txtBankName.Text = "";
            txtBankCode.Text = "";
            txtBankAbbreviation.Text = "";
            txtGlAccCode.Text = "";
            hdnGlCode.Value = "";
            ID = "0";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            grdBanks.SelectedIndex = -1;
           txtBankCode.Focus();

        }


        [WebMethod]
        public static List<spGetBankA_CResult> GetBranch(string bank)
        {
            voucherDetailBL vrBl = new voucherDetailBL();
            List<spGetBankA_CResult> acc = vrBl.GetBranch(Convert.ToInt32(HttpContext.Current.Session["BranchID"]), bank, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return acc;
        }

        #endregion
    }
}

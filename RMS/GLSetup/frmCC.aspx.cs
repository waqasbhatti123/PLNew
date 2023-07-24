using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.Linq;

namespace RMS.GLSetup
{
    public partial class frmCC : BasePage
    {
        #region DataMembers

        CCBL ccBL = new CCBL();
        static tblCCTyp ccTyp;
        static Cost_Center ccCd;

        #endregion

        #region Properties

        public string Code
        {
            get { return ViewState["Code"].ToString(); }
            set { ViewState["Code"] = value; }
        }

        public bool IsInPage
        {
            get { return Convert.ToBoolean(ViewState["IsInPage"]); }
            set { ViewState["IsInPage"] = value; }
        }

        public int CodeLength
        {
            get { return (ViewState["CodeLength"] == null) ? 0 : Convert.ToInt32(ViewState["CodeLength"]); }
            set { ViewState["CodeLength"] = value; }
        }

        public string CodeType
        {
            get { return ViewState["CodeType"].ToString(); }
            set { ViewState["CodeType"] = value; }
        }

        public bool IsEdit 
        {
            get { return Convert.ToBoolean(ViewState["IsEdit"]); }
            set { ViewState["IsEdit"] = value; }
        }

        public string Description
        {
            get{return ViewState["Description"].ToString();}
            set{ViewState["Description"]=value;}
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            int BrId = 0;
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"]);
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "CostCenter").ToString();
                
                CodeType = "";
                Code = "";
                IsInPage = false;
                IsEdit = false;

                BindDdlCodeType();
                ddlCodeType_SelectedIndexChanged(null, null);
              

                BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text);
            }
        }
        protected void ddlCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                ccTyp = ccBL.GetCodeType(Convert.ToChar(ddlCodeType.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (ccTyp != null)
                {
                    CodeLength = Convert.ToInt32(ccTyp.cct_len);
                    txtCode.MaxLength = CodeLength;
                    CodeType = ccTyp.cct_typ;

                    ClearFields();
      
                    if (CodeType.Equals("Group"))
                    {
                        count = ccBL.GetCount(null, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (!count.Equals(-1))
                        {
                            txtCode.Text = Convert.ToString(count + 1).PadLeft(CodeLength, '0');
                        }
                        else
                        {
                            ucMessage.ShowMessage("Error occurred during code generation process.", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                        txtCode.Enabled = true;
                    }
               
                    BindDdlCodeHead();
                    ddlCodeHead_SelectedIndexChanged(null, null);

                    upnlMain.Update();

                    ddlCodeHead.Focus();
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        protected void ddlCodeHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int max = 0;
                string codestr = "";
                ccCd = ccBL.GetCode(ddlCodeHead.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (ccCd != null)
                {
                    max = ccBL.GetMax(ccCd.cc_cd, ccCd.cc_cd.Length, CodeLength - ccCd.cc_cd.Length, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    codestr = max.ToString().PadLeft(CodeLength- ccCd.cc_cd.Length, '0');
                    
                    txtCode.Text = ccCd.cc_cd+codestr;
                    //ddlgltype.SelectedValue = ccCd.gt_cd.ToString();
                    //ddlgltype.Enabled = false;

                    txtDescription.Focus();
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        protected void grdcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlCodeType.SelectedValue = Convert.ToString(grdcode.SelectedDataKey["cct_id"]);
                ccTyp = ccBL.GetCodeType(Convert.ToChar(ddlCodeType.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                Code = grdcode.SelectedDataKey["cc_cd"].ToString().Trim();
                CodeType = Convert.ToString(grdcode.SelectedDataKey["codetype"]);
                

                if (!CodeType.Equals("Group"))
                {
                    BindDdlCodeHead();
                    ddlCodeHead.SelectedValue = Convert.ToString(grdcode.SelectedDataKey["headgl_cd"]);
                }
                else
                {
                    ddlCodeHead.Items.Clear();
                }

                this.GetByID();
                IsEdit = true;

                grdcode.PageIndex = 0;
                IsInPage = false;
                BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text);
                txtCode.Enabled = false;
                
                int row = grdcode.SelectedRow.RowIndex;
                Description = grdcode.Rows[row].Cells[1].Text;

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        protected void grdcode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdcode.PageIndex = e.NewPageIndex;
            BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text);
        }
        protected void grdcode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text.Trim() == Code)
                {
                    IsInPage = true;
                    e.Row.Attributes.Add("style", "background-color:#ffffcc");

                }
            }
        }
        protected void grdcode_DataBound(object sender, EventArgs e)
        {
            if (IsInPage == false && Code != "")
            {
                this.grdcode.PageIndex = this.grdcode.PageIndex + 1;
                BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdcode.PageIndex = 0;
            BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsEdit)
            {
                if (!ccBL.CodeExits(txtCode.Text, txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    if (!ccBL.DescExits(txtCode.Text, txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        Save();
                    }
                    else
                    {
                        ucMessage.ShowMessage("Description already exists.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Code already exists.", RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                bool IsExist = false;
                if (txtDescription.Text != Description)
                {
                    IsExist = ccBL.DescExits(txtCode.Text, txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                }

                if (IsExist == false)
                {
                    Edit();
                }
                else
                {
                    ucMessage.ShowMessage("Description already exists.", RMS.BL.Enums.MessageType.Error);
                }
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
 
        #endregion

        #region Method

        private void BindDdlCodeType()
        {
            ddlCodeType.DataSource = ccBL.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCodeType.DataTextField = "cct_dsc";
            ddlCodeType.DataValueField = "cct_id";
            ddlCodeType.DataBind();

            ddlFltCodeType.DataSource = ccBL.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltCodeType.DataTextField = "cct_dsc";
            ddlFltCodeType.DataValueField = "cct_id";
            ddlFltCodeType.DataBind();
        }
        private void BindDdlCodeHead()
        {
            ddlCodeHead.DataSource = ccBL.GetCodeHead(ccTyp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCodeHead.DataTextField = "cc_nme";
            ddlCodeHead.DataValueField = "cc_cd";
            ddlCodeHead.DataBind();
        }
        private void GetByID()
        {
            try
            {
                Cost_Center rec = ccBL.GetRecByID(Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (rec != null)
                {
                    txtCode.Text = rec.cc_cd;
                    txtDescription.Text = rec.cc_nme;

                    ddlCodeType.Enabled = false;
                    ddlCodeHead.Enabled = false;
                    ddlCCType.SelectedValue = rec.typ;
                    if (rec.status)
                        ddlStatus.SelectedValue = "1";
                    else
                        ddlStatus.SelectedValue = "0";
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        public void BindGrid(string cd, string dsc, string typ)
        {
            this.grdcode.DataSource = ccBL.GetAllGlCodeFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], cd, dsc, typ);
            this.grdcode.DataBind();
        }
        private void ClearForm()
        {
            IsEdit = false;
            IsInPage = false;
            Code = "";

            txtCode.Text = "";
            txtDescription.Text = "";
            ddlCodeType.Enabled = true;
            ddlCodeHead.Enabled = true;

            ddlCodeType.SelectedIndex = 0;
            ddlCodeType_SelectedIndexChanged(null, null);

            ddlCCType.SelectedIndex = 0;
            ddlStatus.SelectedValue = "1";
        }
        private void ClearFields()
        {
            txtCode.Text = "";
            txtDescription.Text = "";
            ddlCCType.SelectedIndex = 0;
            ddlStatus.SelectedValue = "1";
            txtCode.Enabled = false;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object CodeVal(string selectedTypeId1, string selectedTypeId2)
        {
            CCBL ccBL = new CCBL();
            return ccBL.CodeVal(selectedTypeId1, selectedTypeId2, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
        }
        private void Save()
        {
            try
            {
                if (txtCode.Text.Length.Equals(CodeLength))
                {
                    string res = "";
                    Cost_Center cccd = new Cost_Center();

                    Code = txtCode.Text;

                    cccd.cc_cd = txtCode.Text;

                    cccd.cc_nme = txtDescription.Text;
                    if (!string.IsNullOrEmpty(ddlCodeHead.SelectedValue))
                    {
                        cccd.cnt_cc_cd = ddlCodeHead.SelectedValue;
                    }
                    cccd.cct_id = Convert.ToString(ddlCodeType.SelectedValue);
                    cccd.typ = ddlCCType.SelectedValue;
                    if (ddlStatus.SelectedValue == "1")
                        cccd.status = true;
                    else
                        cccd.status = false;
                    cccd.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    if (Session["UserName"] == null)
                        cccd.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        cccd.updateby = Session["UserName"].ToString();


                    res = ccBL.SaveItem(cccd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == "ok")
                    {
                        grdcode.PageIndex = 0;
                        BindGrid("", "", "All");
                        Code = "";
                        IsInPage = false;

                        ClearForm();

                        ucMessage.ShowMessage("Saved successfully.", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        cccd = null;
                        ucMessage.ShowMessage("Exception: " + res, RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Code should be greater than 0 and " + CodeLength + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        private void Edit()
        {
            try
            {
                string code = "";
                Cost_Center cccd = new Cost_Center();

                Code = txtCode.Text;
                IsInPage = false;

                code = txtCode.Text;
                cccd.cc_cd = txtCode.Text;
                cccd.cc_nme = txtDescription.Text;
                cccd.typ = ddlCCType.SelectedValue;
                if (ddlStatus.SelectedValue == "1")
                    cccd.status = true;
                else
                    cccd.status = false;

                if (ccBL.updateCode(cccd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    grdcode.PageIndex = 0;
                    BindGrid("", "", "All");
                    Code = "";
                    IsInPage = false;

                    ddlCodeType.Enabled = true;
                    ddlCodeHead.Enabled = true;
                    txtCode.Enabled = true;
                    ddlCodeType.SelectedIndex = 0;
                    ddlCodeType_SelectedIndexChanged(null, null);

                    IsEdit = false;

                    ClearForm();
                    ucMessage.ShowMessage("Updated successfully.", RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    cccd = null;
                    ucMessage.ShowMessage("Update was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion
    }
}

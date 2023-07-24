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

namespace RMS.sales
{
    public partial class AreaCode : BasePage
    {
        #region DataMembers

        AreaCodeBL GlCodeBl = new AreaCodeBL();
        static tblSlAreaCdTyp glCodeTyp;
        static tblSlAreaCd glCode;

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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "AreaCode").ToString();
                
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
                glCodeTyp = GlCodeBl.GetCodeType(Convert.ToChar(ddlCodeType.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (glCodeTyp != null)
                {
                    CodeLength = Convert.ToInt32(glCodeTyp.ar_len);
                    txtCode.MaxLength = CodeLength;
                    CodeType = glCodeTyp.ar_typ;

                    ClearFields();
      
                    if (CodeType.Equals("Group"))
                    {
                        count = GlCodeBl.GetCount(null, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                glCode = GlCodeBl.GetCode(ddlCodeHead.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (glCode != null)
                {
                    max = GlCodeBl.GetMax(glCode.ar_cd, glCode.ar_cd.Length, CodeLength - glCode.ar_cd.Length, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    codestr = max.ToString().PadLeft(CodeLength- glCode.ar_cd.Length, '0');
                    
                    txtCode.Text = glCode.ar_cd+codestr;

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
                ddlCodeType.SelectedValue = Convert.ToString(grdcode.SelectedDataKey["ct_id"]);
                glCodeTyp = GlCodeBl.GetCodeType(Convert.ToChar(ddlCodeType.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                Code = grdcode.SelectedDataKey["gl_cd"].ToString().Trim();
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
                
                int row= grdcode.SelectedRow.RowIndex;
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
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            grdcode.PageIndex = 0;
            BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text);
        }
        protected void lnkAddMore_ClearVendor(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsEdit)
            {
                if (!GlCodeBl.CodeExits(txtCode.Text, txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    if (!GlCodeBl.DescExits(txtCode.Text, txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
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
                  IsExist=  GlCodeBl.DescExits(txtCode.Text, txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    
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

        private void Save()
        {
            try
            {
                if (txtCode.Text.Length.Equals(CodeLength))
                {
                    string res = "";
                    tblSlAreaCd glcd = new tblSlAreaCd();

                    Code = txtCode.Text;

                    glcd.ar_cd = txtCode.Text;

                    glcd.ar_dsc = txtDescription.Text;
                    if (!string.IsNullOrEmpty(ddlCodeHead.SelectedValue))
                    {
                        glcd.cnt_ar_cd = ddlCodeHead.SelectedValue;
                    }
                    glcd.ar_id = Convert.ToString(ddlCodeType.SelectedValue);
                    glcd.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    if (Session["UserName"] == null)
                        glcd.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        glcd.updateby = Session["UserName"].ToString();


                    res = GlCodeBl.SaveItem(glcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                        glcd = null;
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
                tblSlAreaCd tblGlCode = new tblSlAreaCd();

                Code = txtCode.Text;
                IsInPage = false;

                code = txtCode.Text;
                tblGlCode.ar_cd = txtCode.Text;
                tblGlCode.ar_dsc = txtDescription.Text;

                if (GlCodeBl.updateCode(tblGlCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    grdcode.PageIndex = 0;
                    BindGrid("", "", "All");
                    Code = "";
                    IsInPage = false;

                    ddlCodeType.Enabled = true;
                    ddlCodeHead.Enabled = true;
                    //txtCode.Enabled = true;
                    ddlCodeType.SelectedIndex = 0;
                    ddlCodeType_SelectedIndexChanged(null, null);

                    IsEdit = false;

                    ClearForm();
                    ucMessage.ShowMessage("Updated successfully.", RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    tblGlCode = null;
                    ucMessage.ShowMessage("Update was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        private void GetByID()
        {
            try
            {
                tblSlAreaCd rec = GlCodeBl.GetRecByID(Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (rec != null)
                {
                    txtCode.Text = rec.ar_cd;
                    txtDescription.Text = rec.ar_dsc;
                
                    ddlCodeType.Enabled = false;
                    ddlCodeHead.Enabled = false;
                    //txtCode.Enabled = false;
                }
                 
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }       
        public void BindGrid(string cd, string dsc, string typ)
        {
            this.grdcode.DataSource = GlCodeBl.GetAllGlCodeFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], cd, dsc, typ);
            this.grdcode.DataBind();
        }
        private void BindDdlCodeHead()
        {
            ddlCodeHead.DataSource = GlCodeBl.GetCodeHead(glCodeTyp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCodeHead.DataTextField = "gl_dsc";
            ddlCodeHead.DataValueField = "gl_cd";
            ddlCodeHead.DataBind();
        }
        private void BindDdlCodeType()
        {
            ddlCodeType.DataSource = GlCodeBl.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCodeType.DataTextField = "ar_dsc";
            ddlCodeType.DataValueField = "ar_id";
            ddlCodeType.DataBind();

            ddlFltCodeType.DataSource = GlCodeBl.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltCodeType.DataTextField = "ar_dsc";
            ddlFltCodeType.DataValueField = "ar_id";
            ddlFltCodeType.DataBind();
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
            //txtCode.Enabled = true;

            ddlCodeType.SelectedIndex = 0;
            ddlCodeType_SelectedIndexChanged(null, null);
        }
        private void ClearFields()
        {
            txtCode.Text = "";
            txtDescription.Text = "";
            txtCode.Enabled = false;
        }
      
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object CodeVal(string selectedTypeId1, string selectedTypeId2)
        {
            AreaCodeBL GlCodeBl = new AreaCodeBL();
            return GlCodeBl.CodeVal(selectedTypeId1, selectedTypeId2, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
        }

        #endregion
    }
}

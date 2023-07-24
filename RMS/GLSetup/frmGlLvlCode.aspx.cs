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
    public partial class frmGlLvlCode : BasePage
    {
        #region DataMembers

        voucherDetailBL objVoucher = new voucherDetailBL();
        EntitySet<Glmf> enttySetGlmf = new EntitySet<Glmf>();
        GlCodeBL1 GlCodeBl = new GlCodeBL1();
        static Code_Type glCodeTyp;
        static Glmf_Code glCode;

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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GlCode").ToString();
                
                CodeType = "";
                Code = "";
                IsInPage = false;
                IsEdit = false;

                BindGlType();
                BindDdlCodeType();
                ddlCodeType_SelectedIndexChanged(null, null);

                BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text, Convert.ToChar(ddlFltGlType.SelectedValue));
            }
        }

        protected void ddlCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                glCodeTyp = GlCodeBl.GetCodeType(Convert.ToChar(ddlCodeType.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                CodeLength = Convert.ToInt32(glCodeTyp.ct_len);
                if (glCodeTyp != null)
                {
                    txtCode.MaxLength = CodeLength;
                    CodeType = glCodeTyp.ct_typ;
                    
                    ClearFields();
                }
                BindDdlCodeHead();
                ddlCodeHead_SelectedIndexChanged(null, null);
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
                glCode = GlCodeBl.GetCode(ddlCodeHead.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (glCode != null)
                {
                    txtCode.Text = glCode.gl_cd;
                    ddlgltype.SelectedValue = glCode.gt_cd.ToString();
                    ddlgltype.Enabled = false;
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
                ddlgltype.Enabled = false;
                ddlgltype.SelectedValue = Convert.ToString(grdcode.SelectedDataKey["gt_cd"]);

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
                BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text, Convert.ToChar(ddlFltGlType.SelectedValue));

                
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
            BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text, Convert.ToChar(ddlFltGlType.SelectedValue));
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
                BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text, Convert.ToChar(ddlFltGlType.SelectedValue));
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            grdcode.PageIndex = 0;
            BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text, Convert.ToChar(ddlFltGlType.SelectedValue));
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
                    EditCode();
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
                    Glmf_Code glcd = new Glmf_Code();

                    Code = txtCode.Text;

                    glcd.gl_cd = txtCode.Text;
                    
                    glcd.gl_dsc = txtDescription.Text;
                    if (!string.IsNullOrEmpty(ddlCodeHead.SelectedValue))
                    {
                        glcd.cnt_gl_cd = ddlCodeHead.SelectedValue;
                    }
                    glcd.ct_id = Convert.ToString(ddlCodeType.SelectedValue);
                    glcd.gt_cd = Convert.ToString(ddlgltype.SelectedValue);
                    glcd.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    if (Session["UserName"] == null)
                        glcd.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        glcd.updateby = Session["UserName"].ToString();


                    res = GlCodeBl.SaveItem(glcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == "ok")
                    {
                        EntitySet<Glmf> setGlmf = Get_Glmf_Data(Code);
                        GlCodeBl.InsertIntoGlmf(setGlmf, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        grdcode.PageIndex = 0;
                        BindGrid("", "", "All", Convert.ToChar("-"));
                        Code = "";
                        IsInPage = false;
                        
                        ClearForm();

                        ucMessage.ShowMessage("Saved successfully.", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        glcd = null;
                        ucMessage.ShowMessage("Exception: "+res, RMS.BL.Enums.MessageType.Error);
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

        private void EditCode()
        {
            try
            {
                Glmf_Code tblGlCode = new Glmf_Code();

                Code = txtCode.Text;
                IsInPage = false;

                tblGlCode.gl_cd = txtCode.Text;
                tblGlCode.gl_dsc = txtDescription.Text;

                if (GlCodeBl.updateCode(tblGlCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    grdcode.PageIndex = 0;
                    BindGrid("", "", "All", Convert.ToChar("-"));
                    Code = "";
                    IsInPage = false;

                    ddlCodeType.Enabled = true;
                    ddlCodeHead.Enabled = true;
                    txtCode.Enabled = true;
                    ddlCodeType.SelectedIndex = 0;
                    ddlCodeType_SelectedIndexChanged(null, null);

                    IsEdit = false;

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
                Glmf_Code rec = GlCodeBl.GetRecByID(Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (rec != null)
                {
                    txtCode.Text = rec.gl_cd;
                    txtDescription.Text = rec.gl_dsc;
                
                    ddlCodeType.Enabled = false;
                    ddlCodeHead.Enabled = false;
                    txtCode.Enabled = false;
                }
                 
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindGrid(string cd, string dsc, string typ, char glTyp)
        {
            this.grdcode.DataSource = GlCodeBl.GetAllGlCodeFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], cd, dsc, glTyp, typ);
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
            ddlCodeType.DataTextField = "ct_dsc";
            ddlCodeType.DataValueField = "ct_id";
            ddlCodeType.DataBind();

            ddlFltCodeType.DataSource = GlCodeBl.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltCodeType.DataTextField = "ct_dsc";
            ddlFltCodeType.DataValueField = "ct_id";
            ddlFltCodeType.DataBind();
        }

        private void BindGlType()
        {
            this.ddlgltype.DataSource = GlCodeBl.GetGlType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlgltype.DataValueField = "gt_cd";
            this.ddlgltype.DataTextField = "gt_dsc";
            this.ddlgltype.DataBind();

            this.ddlFltGlType.DataSource = GlCodeBl.GetGlType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlFltGlType.DataValueField = "gt_cd";
            this.ddlFltGlType.DataTextField = "gt_dsc";
            this.ddlFltGlType.DataBind();
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
            txtCode.Enabled = true;

            ddlCodeType.SelectedIndex = 0;
            ddlCodeType_SelectedIndexChanged(null, null);
        }

        private void ClearFields()
        {
            txtCode.Text = "";
            txtDescription.Text = "";
            ddlgltype.SelectedValue = "0";
            ddlgltype.Enabled = true;
        }

        public EntitySet<Glmf> Get_Glmf_Data(string glCode)
        {
            try
            {
                List<Branch> branches = GlCodeBl.GetBranches((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                foreach (var b in branches)
                {
                    Glmf obj = new Glmf();
                    obj.gl_cd = glCode;
                    obj.br_id = b.br_id;
                    obj.gl_op = 0;
                    obj.gl_db = 0;
                    obj.gl_cr = 0;
                    obj.gl_obc = 0;
                    obj.gl_not = 0;
                    obj.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    obj.updateby = "admin";
                    obj.gl_cl = 0;
                    obj.gl_year = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    enttySetGlmf.Add(obj);
                }
                return enttySetGlmf;
            }
            catch
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object CodeVal(string selectedTypeId1, string selectedTypeId2)
        {
            GlCodeBL1 GlCodeBl = new GlCodeBL1();
            return GlCodeBl.CodeVal(selectedTypeId1, selectedTypeId2, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
        }

        #endregion
    }
}

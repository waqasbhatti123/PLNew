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

namespace RMS.GL.Setup
{
    public partial class frmGl_Code : BasePage
    {
        #region DataMembers
        Glmf_Code cty;
        voucherDetailBL objVoucher = new voucherDetailBL(); 
        EntitySet<Glmf> enttySetGlmf = new EntitySet<Glmf>();
        //Country cntry = new Country();
        //RMS.BL.CountryBL cntryBL = new RMS.BL.CountryBL();
        GlCodeBL ctyBL = new RMS.BL.GlCodeBL();
        static GlCodeBL ctyBL1 = new GlCodeBL();
        ListItem itm = new ListItem();
#pragma warning disable CS0414 // The field 'frmGl_Code.status' is assigned but its value is never used
       static bool status = false;
#pragma warning restore CS0414 // The field 'frmGl_Code.status' is assigned but its value is never used
        #endregion

        #region Properties

#pragma warning disable CS0114 // 'frmGl_Code.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'frmGl_Code.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }

        }

        public string Code
        {
            get { return ViewState["ID"].ToString(); }
            set { ViewState["ID"] = value; }

        }

        public string glType
        {
            get { return ViewState["glType"].ToString(); }
            set { ViewState["glType"] = value; }

        }

        public int GlCodeLength
        {
            get { return (ViewState["glCodeLength"] == null) ? 0 : Convert.ToInt32(ViewState["glCodeLength"]); }
            set { ViewState["glCodeLength"] = value; }

        }

        public bool IsInPage
        {
            get { return Convert.ToBoolean(ViewState["IsInPage"]); }
            set { ViewState["IsInPage"] = value; }

        }

        public string GlCode
        {
            get { return ViewState["GlCode"].ToString(); }
            set { ViewState["GlCode"] = value; }

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
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                GlCode = "";
                IsInPage = false;
                this.BindGrid();
                
                this.txtglcode.Focus();
                BindCodeType();
                BindGlType();
               // ddlglheadcode.Enabled = false;
                this.txtglcode.MaxLength = GetCodeTypeLength(Convert.ToChar(this.ddlcodetype.SelectedValue));
                GlCodeLength = GetCodeTypeLength(Convert.ToChar(this.ddlcodetype.SelectedValue));
              //  IntialBindHeadCode();
                
                

            }

        }

        protected void grdcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GlCode = grdcode.SelectedRow.Cells[0].Text;
                

                Code = grdcode.SelectedDataKey.Value.ToString().Trim();
                this.GetByID();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                this.txtglcode.Enabled = false;
                this.txtdescription.Focus();
                this.DisableControl();

                grdcode.PageIndex = 0;
                IsInPage = false;
                BindGrid();
                
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }
     
        protected void grdcode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdcode.PageIndex = e.NewPageIndex;
            this.BindGrid();
        }

        protected void grdcode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text.Trim() == GlCode)
                {
                    IsInPage = true;
                    e.Row.Attributes.Add("style", "background-color:#ffffcc");
                }
            }
        }

        protected void grdcode_OnDataBound(object sender, EventArgs e)
        {
            if (IsInPage == false && GlCode != "")
            {
                this.grdcode.PageIndex = this.grdcode.PageIndex + 1;
                BindGrid();
            }
        }

        protected void ddlcodetype_SelectedIndexChanged(object sender, EventArgs e)
        {

            BindHeadCode(ddlcodetype.SelectedValue);
            this.txtglcode.MaxLength = GetCodeTypeLength(Convert.ToChar(this.ddlcodetype.SelectedValue));
            GlCodeLength = GetCodeTypeLength(Convert.ToChar(this.ddlcodetype.SelectedValue));
            ddlgltype.Enabled = false;

            GlCode = "";
            IsInPage = false;
        }

        protected void ddlglheadcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtglcode.Text = ctyBL.GetCodeHeadControl((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"],this.ddlglheadcode.SelectedValue);


            GlCode = "";
            IsInPage = false;
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            GlCode = "";
            IsInPage = false;

            grdcode.DataSource = ctyBL.GetAllGlCodeFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], txtFltGlCode.Text.Trim(), txtFltDesc.Text.Trim(), ddlFltGlType.SelectedValue, ddlFltCodeType.SelectedValue);
            grdcode.DataBind();
            if (grdcode.Rows.Count == 0)
            {
                ucMessage.ShowMessage("No record found", RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

       //     this.grdcode.DataSource = ctyBL.GetFilteredData1((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], txtglcode.Text,txtdescription.Text,ddlcodetype.SelectedItem.Text);

            //if (this.txtglcode.Text != "")
            //{
            //    this.grdcode.DataSource = ctyBL.GetFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], txtglcode.Text, 1);
            //}
            //else if (this.txtdescription.Text != "")
            //{
            //    this.grdcode.DataSource = ctyBL.GetFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], txtdescription.Text, 2);
            //}
            //else if(this.ddlgltype.SelectedIndex > -1)
            //{
            //    this.grdcode.DataSource = ctyBL.GetFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], ddlgltype.SelectedItem.Text, 3);
            //}
            //else
            //{
            //    this.grdcode.DataSource = ctyBL.GetFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], txtglcode.Text, 0);
            
            //}
            //    this.grdcode.DataBind();
            //    if (grdcode.Rows.Count == 0)
            //    {
            //        ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "NoRecord").ToString(), RMS.BL.Enums.MessageType.Error);
            //        pnlMain.Enabled = true;
            //    }
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                this.txtglcode.Focus();
            }
            else if (e.CommandName == "Save")
            {
                if (ID == 0)
                {
                    if (txtglcode.Text.Length.Equals(GlCodeLength))
                    {
                        this.Insert();
                        ClearFields();
                    }
                    else
                    {
                        ucMessage.ShowMessage("GL code length should be "+ GlCodeLength.ToString()+" digits long.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    this.Update(Code);
                    ClearFields();
                }
            }
            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                this.txtdescription.Focus();
               

            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
                BindGrid();

            }
        }

        #endregion

        #region Helping Method

        protected void BindGrid()
        {
            //this.grdcode.DataSource = ctyBL.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdcode.DataSource = ctyBL.GetAllGlCodeFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], txtFltGlCode.Text.Trim(), txtFltDesc.Text.Trim(), ddlFltGlType.SelectedValue, ddlFltCodeType.SelectedValue);
            this.grdcode.DataBind();
        }

        protected void BindCodeType()
        {
            this.ddlcodetype.DataSource = ctyBL.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlcodetype.DataTextField = "ct_dsc";
            this.ddlcodetype.DataValueField = "ct_id";
            this.ddlcodetype.DataBind();

            this.ddlFltCodeType.DataSource = ctyBL.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlFltCodeType.DataTextField = "ct_dsc";
            this.ddlFltCodeType.DataValueField = "ct_dsc";
            this.ddlFltCodeType.DataBind();
        
        }

        protected void BindGlType()
        {
            itm.Text = "-----";
            itm.Value = "0";
            this.ddlgltype.Items.Insert(0, itm);
            this.ddlgltype.DataSource = ctyBL.GetGlType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlgltype.DataValueField = "gt_cd";
            this.ddlgltype.DataTextField = "gt_dsc";
            this.ddlgltype.DataBind();

            this.ddlFltGlType.DataSource = ctyBL.GetGlType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlFltGlType.DataValueField = "gt_dsc";
            this.ddlFltGlType.DataTextField = "gt_dsc";
            this.ddlFltGlType.DataBind();
        }

        protected void BindHeadCode(string ct_id)
        {
            if (ctyBL.GetCodeHead((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], ct_id).Count > 0)
            {
             //   this.ddlglheadcode.Enabled = true;
                this.ddlglheadcode.DataSource = ctyBL.GetCodeHead((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], ct_id);
                this.ddlglheadcode.DataTextField = "gl_dsc";

                //this.ddlglheadcode.DataValueField = "gl_cd";
                this.ddlglheadcode.DataValueField = "cnt_gl_cd";
                this.ddlglheadcode.DataBind();
             //   this.ddlgltype.Enabled = false;
            }
            else
            {
             //   this.ddlglheadcode.Enabled = false;
            //    this.ddlgltype.Enabled = true;
                this.ddlglheadcode.Items.Clear();

            }
        
        }

        protected void BindHeadCodeNew(string ct_id)
        {
            if (ctyBL.GetCodeHead((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], ct_id).Count > 0)
            {
                //   this.ddlglheadcode.Enabled = true;
                this.ddlglheadcode.DataSource = ctyBL.GetCodeHead((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], ct_id);
                this.ddlglheadcode.DataTextField = "gl_dsc";

                this.ddlglheadcode.DataValueField = "gl_cd";
                //this.ddlglheadcode.DataValueField = "cnt_gl_cd";
                this.ddlglheadcode.DataBind();
                //   this.ddlgltype.Enabled = false;
            }
            else
            {
                //   this.ddlglheadcode.Enabled = false;
                //    this.ddlgltype.Enabled = true;
                this.ddlglheadcode.Items.Clear();

            }

        }

        protected string GetGLTypeCode(string gl_cd)
        {
            return ctyBL.GetGLTypeCode((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"],gl_cd);
           
        }

        protected int GetCodeTypeLength(char gt_cd)
        {

            return ctyBL.GetCodeTypeLength((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], gt_cd);
        }

        private void ClearFields()
        {
            this.grdcode.PageIndex = 0;

            this.ddlcodetype.SelectedIndex = -1;
            this.ddlglheadcode.SelectedIndex = -1;
            this.ddlgltype.SelectedIndex = -1;
            this.txtglcode.Text = "";
            this.txtdescription.Text = "";
            Code = null;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            //this.BindGrid();
            this.txtglcode.Enabled = true;
            this.txtglcode.Focus();
          //  this.ddlglheadcode.Enabled = false;
            this.ddlgltype.Enabled = true;
            this.ddlcodetype.Enabled = true;
            this.txtglcode.Enabled = true;
            this.ddlglheadcode.Items.Clear();

            IsInPage = false;
            GlCode = "";
        }

        protected void DisableControl()
        {
            this.txtglcode.Enabled = false;
         //   this.ddlglheadcode.Enabled = false;
          //  this.ddlgltype.Enabled = false;
            this.ddlcodetype.Enabled = false;
            
        
        
        
        }

        protected void GetByID()
        {
            cty = ctyBL.GetByID(Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //this.ddlCountry.SelectedValue = cty.CountryID.ToString();
            this.txtglcode.Text = cty.gl_cd.ToString();
            this.txtdescription.Text = cty.gl_dsc.ToString();
            this.ddlcodetype.SelectedValue = cty.ct_id.ToString();
            this.ddlgltype.SelectedValue = cty.gt_cd.ToString();

            if (cty.cnt_gl_cd != null)
            {
                BindHeadCodeNew(cty.Glmf_Code1.ct_id.ToString());
                this.ddlglheadcode.SelectedValue = cty.cnt_gl_cd.ToString();
                //this.ddlglheadcode.SelectedValue = cty.cnt_gl_cd.ToString();
            }
            else
            {
                ddlglheadcode.Items.Clear();
            }

        }

        protected void Insert()
        {
            RMS.BL.Glmf_Code ctyR = new RMS.BL.Glmf_Code();
            ctyR.gl_cd= this.txtglcode.Text.Trim();
            ctyR.gl_dsc = this.txtdescription.Text.Trim();
            ctyR.ct_id = Convert.ToString(this.ddlcodetype.SelectedItem.Value);

            if (this.ddlcodetype.SelectedIndex > 0)
            {
                if (this.ddlglheadcode.SelectedIndex > 0)
                {
                    ctyR.cnt_gl_cd = this.ddlglheadcode.SelectedItem.Value;
                }
                else
                {
                    ctyR.cnt_gl_cd = hidglHeadCode.Value;
                }
            }
            else
                ctyR.cnt_gl_cd = null;

            ctyR.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ctyR.updateby = "admin";

            if (this.ddlgltype.Enabled == true)
            {
                ctyR.gt_cd = Convert.ToString(this.ddlgltype.SelectedItem.Value);
            }
            else
            {
                //string code = ddlglheadcode.SelectedItem.Value;
                string code = "";
                if (this.ddlglheadcode.SelectedIndex > 0)
                {
                    code = this.ddlglheadcode.SelectedItem.Value;
                }
                else
                {
                    code = hidglHeadCode.Value;
                }
                ctyR.gt_cd = Convert.ToString(GetGLTypeCode(code));
            
            }
            //ctyR.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);

            EntitySet<Glmf> setGlmf = Get_Glmf_Data(ctyR);
            if (!ctyBL.ISAlreadyExist(ctyR, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                bool res = ctyBL.CodeExists(ctyR, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (res == false)
                {
                    
                    ctyBL.Insert(ctyR, setGlmf, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    grdcode.PageIndex = 0;
                    GlCode = ctyR.gl_cd;
                    IsInPage = false;
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                    BindGrid();
                    ClearFields();
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "RecordExist").ToString(), RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "RecordExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        public EntitySet<Glmf> Get_Glmf_Data(Glmf_Code ctyR)
        {
            try
            {
                List<Branch> branches = ctyBL.GetBranches((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                foreach (var b in branches)
                {
                    Glmf obj = new Glmf();
                    obj.gl_cd = ctyR.gl_cd;
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

        protected void Update(string code)
        {
            cty = ctyBL.GetByID(code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            cty.gl_dsc = this.txtdescription.Text.Trim();
          //  cty.ct_id = Convert.ToChar(this.ddlcodetype.DataValueField);
         //   cty.cnt_gl_cd = this.ddlglheadcode.DataValueField;
            cty.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            cty.updateby = "admin";
            string sType = this.ddlgltype.SelectedValue.ToString();
            cty.gt_cd = Convert.ToString(this.ddlgltype.SelectedValue);

            //cty.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
            if (!ctyBL.ISAlreadyExist(cty, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                ctyBL.Update(cty, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                grdcode.PageIndex = 0;
                GlCode = cty.gl_cd;
                IsInPage = false;

                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "RecordExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }

        }
        
       [WebMethod(EnableSession=true)]
       [ScriptMethod(UseHttpGet=false,ResponseFormat=ResponseFormat.Json)]
        public static object GLCodeVal(string selectedTypeId1, string selectedTypeId2)
        {
           GlCodeBL cty = new GlCodeBL();
           return cty.GLCodeVal(selectedTypeId1,selectedTypeId2,(RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            
        }

       [WebMethod(EnableSession = true)]
       [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
       public static object GLCodeHeads(string selectedTypeId1)
       {
           GlCodeBL cty = new GlCodeBL();
           return cty.GLCodeHeads(selectedTypeId1,(RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);

       }

       [WebMethod(EnableSession = true)]
       [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
       public static object GLCodeHeadsType(string selectedTypeId)
       {
           GlCodeBL cty = new GlCodeBL();
           return cty.GLCodeHeadsType(selectedTypeId, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);

       }
        
        #endregion
    }
}

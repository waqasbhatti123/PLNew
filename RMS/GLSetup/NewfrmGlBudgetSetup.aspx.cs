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
    public partial class NewfrmGlBudgetSetup : BasePage
    {
        #region DataMembers
        static tblBgtHead cty;
        voucherDetailBL objVoucher = new voucherDetailBL(); 
        EntitySet<Glmf> enttySetGlmf = new EntitySet<Glmf>();
        //Country cntry = new Country();
        //RMS.BL.CountryBL cntryBL = new RMS.BL.CountryBL();
        //GlCodeBL ctyBL = new RMS.BL.GlCodeBL();
        //static GlBudgetSetupBL glB1 = new GlBudgetSetupBL();

        GlBudgetSetupBL glB = new GlBudgetSetupBL();

        ListItem itm = new ListItem();

        static Code_Type BgtCodeTyp;
        static tblBgtHead BgtCode;

#pragma warning disable CS0414 // The field 'NewfrmGlBudgetSetup.status' is assigned but its value is never used
       static bool status = false;
#pragma warning restore CS0414 // The field 'NewfrmGlBudgetSetup.status' is assigned but its value is never used
        #endregion

        #region Properties

#pragma warning disable CS0114 // 'NewfrmGlBudgetSetup.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'NewfrmGlBudgetSetup.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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

        public bool Edit
        {
            get { return Convert.ToBoolean(ViewState["Edit"]); }
            set { ViewState["Edit"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GlBudgetCode").ToString();
               
                this.BindGrid();
                //this.txtBudgetcode.Focus();
                this.txtCode.Focus();
                BindCodeType();
           
               // ddlBudgetheadcode.Enabled = false;
                //this.txtBudgetcode.MaxLength = GetCodeTypeLength(Convert.ToChar(this.ddlcodetype.SelectedValue));
            
                ddlBudgetType.Enabled = false;
             
                txtAcFrom.Enabled = false;
                txtAcTo.Enabled = false;
                //txtBudgetcode1.Visible = false;
                RequiredFieldValidator1.Enabled = false;
                RequiredFieldValidator6.Enabled = false;

                //---------------
                ddlcodetype_SelectedIndexChanged(null, null);

                Edit = false;
            }
  
        }

        protected void grdcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Code = grdcode.SelectedDataKey.Value.ToString().Trim();
                this.GetByID();
                Edit = true;
               
                this.txtCode.Enabled = false;
                this.txtdescription.Focus();
                this.DisableControl();
            }
            catch (Exception ex)
            {
                //Session["errors"] = ex.Message;
                //Response.Redirect("~/home/Error.aspx");
                ucMessage.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
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
                if (e.Row.Cells[2].Text == "A")
                {
                    e.Row.Cells[2].Text = "Group";
                }
                else if (e.Row.Cells[2].Text == "B")
                {
                    e.Row.Cells[2].Text = "Control";
                }
                else if (e.Row.Cells[2].Text == "C")
                {
                    e.Row.Cells[2].Text = "Control";
                }
                else
                {
                    e.Row.Cells[2].Text = "Detail";
                }

                //---------------------
                if (e.Row.Cells[4].Text == "Y")
                {
                    e.Row.Cells[4].Text = "Yearly";
                }
                else if (e.Row.Cells[4].Text == "Q")
                {
                    e.Row.Cells[4].Text = "Quaterly";
                }
             
            }
        }

        protected void ddlcodetype_SelectedIndexChanged(object sender, EventArgs e)
        {           
            try
            {
                BgtCodeTyp = glB.GetCodeType(Convert.ToChar(ddlcodetype.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (BgtCodeTyp != null)
                {
                    CodeLength = Convert.ToInt32(BgtCodeTyp.ct_len);
                    txtCode.MaxLength = CodeLength;
                    CodeType = BgtCodeTyp.ct_typ;

                    ClearFields();

                    if (CodeType.Equals("Group"))
                    {
                        //count = GlCodeBl.GetCount(null, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //if (!count.Equals(-1))
                        //{
                        //    txtCode.Text = Convert.ToString(count + 1).PadLeft(CodeLength,'0');
                        //}
                        //else
                        //{
                        //    ucMessage.ShowMessage("Error occurred during code generation process.", RMS.BL.Enums.MessageType.Error);
                        //    return;
                        //}
                        txtAcFrom.Enabled = false;
                        txtAcTo.Enabled = false;
                        RequiredFieldValidator1.Enabled = false;
                        RequiredFieldValidator6.Enabled = false;

                        ddlBudgetType.Enabled = false;

                        txtCode.Enabled = true;
                    }
                    else if (CodeType.Equals("Detail"))
                    {
                        ddlBudgetType.Enabled = true;
                        txtAcFrom.Enabled = true;
                        txtAcTo.Enabled = true;
                        RequiredFieldValidator1.Enabled = true;
                        RequiredFieldValidator5.Enabled = true;
                        RequiredFieldValidator6.Enabled = true;
                    }
                    else
                    {
                        txtAcFrom.Enabled = false;
                        txtAcTo.Enabled = false;
                        RequiredFieldValidator1.Enabled = false;
                        RequiredFieldValidator6.Enabled = false;

                        ddlBudgetType.Enabled = false;
                    }
                    BindDdlCodeHead();
                    ddlBudgetheadcode_SelectedIndexChanged(null, null);

                    //upnlMain.Update();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
    
        }
        
        private void BindDdlCodeHead()
        {
            ddlBudgetheadcode.DataSource = glB.GetCodeHead4Budget(BgtCodeTyp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlBudgetheadcode.DataTextField = "Headg_Desc";
            ddlBudgetheadcode.DataValueField = "Bgt_Code";
            ddlBudgetheadcode.DataBind();
        }
        //---------------------------------------------------
        protected void ddlBudgetheadcode_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            //string [] cd = ddlBudgetheadcode.SelectedItem.Text.Split('-');
            //txtBudgetcode1.Text = cd[0];

            //txtBudgetcode.Focus();

            try
            {
                int count = 0;
                string codestr = "";
                BgtCode = glB.GetCode(ddlBudgetheadcode.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (BgtCode != null)
                {
                    count = glB.GetCount(BgtCode.Bgt_Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    codestr = Convert.ToString(count + 1).PadLeft(CodeLength - BgtCode.Bgt_Code.Length, '0');

                    txtCode.Text = BgtCode.Bgt_Code + codestr;
                    //ddlgltype.SelectedValue = BgtCode.gt_cd.ToString();
                   // ddlgltype.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            string bgCd=txtFltBudgetCode.Text;
            if (bgCd == "")
            {
                bgCd = "";
            }
            string heading= txtFltDesc.Text;
            if (heading == "") 
            {
                heading = "";
            }
            char bgType;
            char cdType=Convert.ToChar(ddlFltCodeType.SelectedValue);
            if (ddlFltCodeType.SelectedValue == "0")
            {
                cdType = Convert.ToChar("Z");
            }
            if (ddlFltBudgetType.Enabled == true)
            {
                 bgType = Convert.ToChar(ddlFltBudgetType.SelectedValue);
                if (ddlFltBudgetType.SelectedValue == "0")
                {
                    bgType = Convert.ToChar("A");
                }
            }
            else
            {
                bgType =Convert.ToChar("A");
            }

           // grdcode.DataSource = glB.GetAllGlCodeFilteredData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], txtFltBudgetCode.Text.Trim(), txtFltDesc.Text.Trim(), ddlFltBudgetType.SelectedValue, ddlFltCodeType.SelectedValue);
            grdcode.DataSource = glB.getAllBudgetCodeFiltered((RMSDataContext)Session[Session["UserID"]+"rmsDBobj"],bgCd,heading,cdType,bgType);
            grdcode.DataBind();
            if (grdcode.Rows.Count == 0)
            {
                ucMessage.ShowMessage("No record found", RMS.BL.Enums.MessageType.Error);
                //pnlMain.Enabled = true;
            }

            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (Edit == true)
            {
                this.Update(Code);
                ClearAllFields();
            }
            else
            {
                this.Insert();
                ClearAllFields();
            }
          
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            //ClearFields();

            Edit = false;
            Response.Redirect("~/GLSetup/NewfrmGlBudgetSetup.aspx?PID=317");
            //Response.Redirect("~/GLSetup/frmReportParameter.aspx?PID=611");
        }

        protected void txtAcFrom_TextChanged(object sender, EventArgs e)
        {
            txtAcFrom.Text = AcFr.Value;
        }

        protected void txtAcTo_TextChanged(object sender, EventArgs e)
        {
            txtAcTo.Text = AcTo.Value;
        }

        #endregion


        #region Helping Method
        protected void BindGrid()
        {
            this.grdcode.DataSource = glB.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdcode.DataBind();
        }

        protected void BindCodeType()
        {
            this.ddlcodetype.DataSource = glB.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlcodetype.DataTextField = "ct_dsc";
            this.ddlcodetype.DataValueField = "ct_id";
            this.ddlcodetype.DataBind();

            this.ddlFltCodeType.DataSource = glB.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.ddlFltCodeType.DataTextField = "ct_dsc";
            this.ddlFltCodeType.DataValueField = "ct_id";
            this.ddlFltCodeType.DataBind();
        
        }

  
        protected void BindHeadCode(string ct_id)
        {
            if (glB.GetCodeHead((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], ct_id)!=null)
            {
                this.ddlBudgetheadcode.DataSource = glB.GetCodeHead((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], ct_id);
                this.ddlBudgetheadcode.DataTextField = "Headg_Desc";

                this.ddlBudgetheadcode.DataValueField = "Bgt_Code";
                this.ddlBudgetheadcode.DataBind();
             }
            else
            {
              this.ddlBudgetheadcode.Items.Clear();
            }
        
        }

     
        protected char GetGLTypeCode(string gl_cd)
        {
            return glB.GetGLTypeCode((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], gl_cd);

        }
        

       

        protected int GetCodeTypeLength(char gt_cd)
        {
           
            return glB.GetCodeTypeLength((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], gt_cd);
        }

        private void ClearFields()
        {
            //this.ddlcodetype.SelectedIndex = 0;
            //this.ddlBudgetheadcode.SelectedIndex = -1;
            

            //this.ddlBudgetType.SelectedIndex = -1;
            //this.txtBudgetcode.Text = "";
            //this.txtdescription.Text = "";
            //Code = null;
           
            //this.BindGrid();
            //this.txtBudgetcode.Enabled = true;
            //this.txtBudgetcode.Focus();
        
            //this.ddlBudgetType.Enabled = false;
            //this.ddlcodetype.Enabled = true;
            //this.txtBudgetcode.Enabled = true;
            //this.ddlBudgetheadcode.Items.Clear();

            //this.txtAcFrom.Text = "";
            //this.txtAcTo.Text = "";
            //txtBudgetcode1.Text = "";
            txtCode.Text = "";
            txtdescription.Text = "";
            
            txtCode.Enabled = false;

        }

        private void ClearAllFields()
        {
            this.ddlcodetype.SelectedIndex = 0;
            ddlcodetype.Enabled = true;
            this.ddlBudgetheadcode.Items.Clear();
            ddlBudgetheadcode.Enabled = true;
            this.ddlBudgetType.Enabled = false;
            //this.ddlBudgetType.SelectedIndex = -1;
            //this.txtBudgetcode.Text = "";
            //this.txtdescription.Text = "";
            //Code = null;

            //this.BindGrid();
            //this.txtBudgetcode.Enabled = true;
            //this.txtBudgetcode.Focus();

            //this.ddlBudgetType.Enabled = false;
            //this.ddlcodetype.Enabled = true;
            //this.txtBudgetcode.Enabled = true;
            //this.ddlBudgetheadcode.Items.Clear();

            this.txtAcFrom.Text = "";
            this.txtAcTo.Text = "";
            //txtBudgetcode1.Text = "";
            txtCode.Text = "";
            txtdescription.Text = "";
            txtCode.Enabled = true;
            txtCode.Focus();

        }

        protected void DisableControl()
        {
            this.txtCode.Enabled = false;
        
            this.ddlcodetype.Enabled = false;
            this.ddlBudgetheadcode.Enabled = false;
        }
        protected void GetByID()
        {
            cty = glB.GetByID(Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

  
            
            this.txtdescription.Text = cty.Headg_Desc.ToString();
            this.ddlcodetype.SelectedValue = cty.Code_Type.ToString();

           
           
            if (!cty.Bgt_Type.Equals(' '))
            {
                this.ddlBudgetType.SelectedValue = cty.Bgt_Type.ToString();
            }
            else
            {
                //this.ddlBudgetType.Items.Clear();
            }
            if (cty.Code_Type == "D")
            {
                txtAcFrom.Enabled = true;
                txtAcTo.Enabled = true;
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                txtAcFrom.Text = cty.GL_AC_Fr;
                txtAcTo.Text = cty.GL_AC_To;
                ddlBudgetType.Enabled = true;
            }
            else
            {
                txtAcFrom.Enabled = false;
                txtAcTo.Enabled = false;
                RequiredFieldValidator1.Enabled = false;
                RequiredFieldValidator6.Enabled = false;
                txtAcFrom.Text = "";
                txtAcTo.Text = "";
                ddlBudgetType.Enabled = false;
            }

            if (cty.cnt_bgt_code != null)
            {
               
                string c = Convert.ToString(ddlcodetype.SelectedValue);
                if (c.ToString() != "A")
                {
                    BindHeadCode(getParentID(c).ToString());
                    ddlBudgetheadcode.SelectedValue = cty.cnt_bgt_code;
                }
            }
            else
            {
                ddlBudgetheadcode.Items.Clear();
            }

            //----------------------------
            if (ddlcodetype.SelectedValue.ToString() != "A")
            {
               // txtBudgetcode1.Visible = true;
               // txtBudgetcode1.Enabled = false;
               // string[] cd = ddlBudgetheadcode.SelectedItem.Text.Split('-');
               // txtBudgetcode1.Text = cd[0];

              //  string txt = txtBudgetcode1.Text;
               // int len = Convert.ToInt32(txt.Length);

               // txtBudgetcode.Text = cty.Bgt_Code.Substring(len);
                txtCode.Text = cty.Bgt_Code;

            }
            else
            {
                txtCode.Text = cty.Bgt_Code;
               // txtBudgetcode1.Visible = false;
               // txtBudgetcode.Text = cty.Bgt_Code;
            }
            //----------------------
            
        }

    
        protected void Insert()
        {
            RMS.BL.tblBgtHead ctyR = new RMS.BL.tblBgtHead();
            ctyR.Bgt_Code = txtCode.Text.Trim();//this.txtBudgetcode1.Text.Trim()+this.txtBudgetcode.Text.Trim();
            ctyR.br_id = 1;
            ctyR.Headg_Desc = this.txtdescription.Text.Trim();
            ctyR.Code_Type = Convert.ToString(this.ddlcodetype.SelectedItem.Value);

            if (this.ddlcodetype.SelectedIndex > 0)
            {
                //if (this.ddlBudgetheadcode.SelectedIndex > 0)
                //{
                    ctyR.cnt_bgt_code = this.ddlBudgetheadcode.SelectedItem.Value;
                //}
                //else
                //{
                //    ctyR.cnt_bgt_code = hidglHeadCode.Value;
                //}
            }
            else
                ctyR.cnt_bgt_code = null;

            ctyR.Created_On = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ctyR.Created_By = "admin";

            if (this.ddlBudgetType.Enabled == true)
            {
                ctyR.Bgt_Type = Convert.ToString(this.ddlBudgetType.SelectedItem.Value);
            }
            else
            {
                ctyR.Bgt_Type =" ";
                               
            }

                    ctyR.GL_AC_Fr = txtAcFrom.Text;
                    ctyR.GL_AC_To = txtAcTo.Text;
           
            if (!glB.ISAlreadyExist(ctyR, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                bool res = glB.CodeExists(ctyR, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (res == false)
                {
                   
                    glB.Insert(ctyR,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

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
                //pnlMain.Enabled = true;
            }
        }
        
        protected void Update(string code)
        {
            Edit = false;
           
            string desc = this.txtdescription.Text.Trim();
            string fr = "";
            string to = "";
      
            char bgType = ' ';


            if (ddlcodetype.SelectedValue == "D")
            {
                 fr = txtAcFrom.Text;
                 to = txtAcTo.Text;
                 bgType = Convert.ToChar(ddlBudgetType.SelectedValue);
               
            }
        
            DateTime upOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            string upBy = "admin";
        

                glB.Update(code, desc, fr, to, bgType, upOn, upBy, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
               if( glB.Update(code, desc, fr, to, bgType, upOn, upBy, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])==true)
               {
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
                }

            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "Can not Update").ToString(), RMS.BL.Enums.MessageType.Error);
             
            }

        }

        public char getParentID(string c)
        {
            
                return glB.getParent(c, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
           
        }





        //----------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object CodeVal(string selectedTypeId1, string selectedTypeId2)
        {
            GlBudgetSetupBL BdgBl = new GlBudgetSetupBL();
            return BdgBl.CodeVal(selectedTypeId1, selectedTypeId2, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
        }

     
        #endregion
    }
}

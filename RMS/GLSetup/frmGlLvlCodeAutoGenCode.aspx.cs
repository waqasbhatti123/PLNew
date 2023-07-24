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
    public partial class frmGlLvlCodeAutoGenCode : BasePage
    {
        #region DataMembers

        static glmf_ven_cus_det glmfVenDet;
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
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
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
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GlCode").ToString();
                
                CodeType = "";
                Code = "";
                IsInPage = false;
                IsEdit = false;
                
                BindDdlCity();
                BindGlType();
                BindDdlCodeType();
                BindDdlVendor();
                ddlCodeType_SelectedIndexChanged(null, null);
                FillSearchBranchDropDown();
                PnlDivision.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(PnlDivision.SelectedValue);
                BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text, Convert.ToChar(ddlFltGlType.SelectedValue));
            }
        }

        protected void ddlCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
#pragma warning disable CS0219 // The variable 'count' is assigned but its value is never used
                int count = 0;
#pragma warning restore CS0219 // The variable 'count' is assigned but its value is never used
                glCodeTyp = GlCodeBl.GetCodeType(Convert.ToChar(ddlCodeType.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (glCodeTyp != null)
                {
                    CodeLength = Convert.ToInt32(glCodeTyp.ct_len);
                    txtCode.MaxLength = CodeLength;
                    CodeType = glCodeTyp.ct_typ;

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
                    max = GlCodeBl.GetMax(glCode.gl_cd, glCode.gl_cd.Length, CodeLength - glCode.gl_cd.Length, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    codestr = max.ToString().PadLeft(CodeLength- glCode.gl_cd.Length, '0');
                    
                    txtCode.Text = glCode.gl_cd+codestr;
                    ddlgltype.SelectedValue = glCode.gt_cd.ToString();
                    ddlgltype.Enabled = false;

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
                txtCode.Enabled = false;
                GetVendorInfo();
                
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdcode.PageIndex = 0;
            BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text, Convert.ToChar(ddlFltGlType.SelectedValue));
        }

        protected void lnkAddMore_ClearVendor(object sender, EventArgs e)
        {

        }

        

        protected void btnSaveVendor_Click(object sender, EventArgs e)
        {
            try
            {
                glmfVenDet = new glmf_ven_cus_det();

                glmfVenDet.NTN = txtNTN.Text;
                glmfVenDet.STN = txtSTN.Text;
                glmfVenDet.address = txtAddress.Text;
                glmfVenDet.tel = txtTelNo.Text;
                glmfVenDet.email = txtEmail.Text;
                glmfVenDet.Cont_Person = txtContactPerson.Text;
                glmfVenDet.city = ddlCity.SelectedItem.Text;
                glmfVenDet.cell_no = txtCellNo.Text;
                glmfVenDet.fax_no = txtFaxNo.Text;
                glmfVenDet.vendor_type = ddlVendorType.SelectedValue;
                glmfVenDet.br_id = Convert.ToInt32(PnlDivision.SelectedValue);
                glmfVenDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                if (Session["UserName"] == null)
                    glmfVenDet.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                else
                    glmfVenDet.updateby = Session["UserName"].ToString();
 
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
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
                        BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text, Convert.ToChar(ddlFltGlType.SelectedValue));

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
                    BindGrid(txtFltCode.Text, txtFltDesc.Text, ddlFltCodeType.SelectedItem.Text, Convert.ToChar(ddlFltGlType.SelectedValue));

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            glmfVenDet = null;
            ClearVendor();
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
                    glcd.code = txtManualCode.Text == "" ? null : txtManualCode.Text;
                    
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
                        //GLMF Entry
                        EntitySet<Glmf> setGlmf = Get_Glmf_Data(Code);
                        GlCodeBl.InsertIntoGlmf(setGlmf, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        //glmf_ven_cus_det Entry
                        if (!string.IsNullOrEmpty(txtAddress.Text))
                        {
                            if (glmfVenDet != null)
                            {
                                glmfVenDet.gl_cd = txtCode.Text;
                                GlCodeBl.Insert_glmf_ven_cus_det(glmfVenDet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                        }
                      
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

        private void Edit()
        {
            try
            {
                string code = "";
                Glmf_Code tblGlCode = new Glmf_Code();

                Code = txtCode.Text;
                IsInPage = false;

                code = txtCode.Text;
                tblGlCode.gl_cd = txtCode.Text;
                tblGlCode.code = txtManualCode.Text == "" ? null : txtManualCode.Text;
                tblGlCode.gl_dsc = txtDescription.Text;

                if (GlCodeBl.updateCode(tblGlCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    grdcode.PageIndex = 0;
                    BindGrid("", "", "All", Convert.ToChar("-"));
                    Code = "";
                    IsInPage = false;

                    ddlCodeType.Enabled = true;
                    ddlCodeHead.Enabled = true;
                    //txtCode.Enabled = true;
                    ddlCodeType.SelectedIndex = 0;
                    ddlCodeType_SelectedIndexChanged(null, null);

                    IsEdit = false;

                    //glmf_ven_cus_det Update
                    if (!string.IsNullOrEmpty(txtAddress.Text))
                    {
                        if (glmfVenDet != null)
                        {
                            glmfVenDet.gl_cd = code;
                            glmf_ven_cus_det glmfVen = GlCodeBl.Get_glmf_ven_cus_det(code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (glmfVen != null)
                            {
                                glmfVen.gl_cd = glmfVenDet.gl_cd;
                                glmfVen.NTN = glmfVenDet.NTN;
                                glmfVen.STN = glmfVenDet.STN;
                                glmfVen.address = glmfVenDet.address;
                                glmfVen.tel = glmfVenDet.tel;
                                glmfVen.email = glmfVenDet.email;
                                glmfVen.Cont_Person = glmfVenDet.Cont_Person;
                                glmfVen.city = glmfVenDet.city;
                                glmfVen.cell_no = glmfVenDet.cell_no;
                                glmfVen.fax_no = glmfVenDet.fax_no;
                                glmfVen.vendor_type = glmfVenDet.vendor_type;
                                glmfVen.updateon = glmfVenDet.updateon;
                                glmfVen.updateby = glmfVenDet.updateby;
                                glmfVen.br_id = glmfVenDet.br_id;

                                GlCodeBl.Update_glmf_ven_cus_det(glmfVen, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(txtAddress.Text))
                                {
                                    GlCodeBl.Insert_glmf_ven_cus_det(glmfVenDet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                }
                            }
                        }
                    }
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
                Glmf_Code rec = GlCodeBl.GetRecByID(Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (rec != null)
                {
                    txtCode.Text = rec.gl_cd;
                    txtManualCode.Text = rec.code;
                    txtDescription.Text = rec.gl_dsc;
                
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

        public void GetVendorInfo()
        {
            ClearVendor();
            glmf_ven_cus_det glmfVen = GlCodeBl.Get_glmf_ven_cus_det(txtCode.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (glmfVen != null)
            {
                ddlVendorType.SelectedValue = glmfVen.vendor_type;
                txtNTN.Text = glmfVen.NTN;
                txtSTN.Text = glmfVen.STN;
                txtAddress.Text = glmfVen.address;
                txtTelNo.Text = glmfVen.tel;
                txtCellNo.Text = glmfVen.cell_no;
                txtFaxNo.Text = glmfVen.fax_no;
                ddlCity.SelectedValue = glmfVen.city;
                txtContactPerson.Text = glmfVen.Cont_Person;
                txtEmail.Text = glmfVen.email;
                PnlDivision.SelectedValue = glmfVen.br_id.ToString();
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

        private void BindDdlVendor()
        {
            ddlVendorType.DataSource = GlCodeBl.GetAccountTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendorType.DataTextField = "Description";
            ddlVendorType.DataValueField = "codetype";
            ddlVendorType.DataBind();
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

        private void BindDdlCity()
        {
            ddlCity.DataSource = GlCodeBl.GetCities((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCity.DataValueField = "CityName";
            ddlCity.DataTextField = "CityName";
            ddlCity.DataBind();
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

        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            this.PnlDivision.DataTextField = "br_nme";
            PnlDivision.DataValueField = "br_id";
            if (BranchObj.IsHead == true)
            {
                PnlDivision.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                List<Branch> BranchList = new List<Branch>();

                if (BranchObj != null)
                {
                    if (BranchObj.IsDisplay == true)
                    {
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BranchID).ToList();
                        BranchList.Insert(0, BranchObj);
                    }
                    else
                    {
                        BranchList.Add(BranchObj);
                    }
                }
                PnlDivision.DataSource = BranchList.ToList();
            }
            PnlDivision.DataBind();

        }

        private void ClearForm()
        {
            IsEdit = false;
            IsInPage = false;
            Code = "";

            txtCode.Text = "";
            txtManualCode.Text = "";
            txtDescription.Text = "";
            ddlCodeType.Enabled = true;
            ddlCodeHead.Enabled = true;
            //txtCode.Enabled = true;

            //ddlCodeType.SelectedIndex = 0;
            ddlCodeType_SelectedIndexChanged(null, null);

            ClearVendor();

            txtDescription.Focus();
        }

        private void ClearFields()
        {
            txtCode.Text = "";
            txtManualCode.Text = "";
            txtDescription.Text = "";
            ddlgltype.SelectedValue = "0";
            ddlgltype.Enabled = true;
            txtCode.Enabled = false;
        }

        private void ClearVendor()
        {
            txtNTN.Text="";
            txtSTN.Text="";
            txtAddress.Text="";
            txtTelNo.Text="";
            txtEmail.Text="";
            PnlDivision.SelectedValue = "0";
            txtContactPerson.Text="";
            ddlCity.SelectedValue="0";
            ddlVendorType.SelectedValue = "0";
            txtCellNo.Text="";
            txtFaxNo.Text="";

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

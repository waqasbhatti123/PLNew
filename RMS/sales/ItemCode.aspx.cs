using System;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.sales
{
    public partial class ItemCode : BasePage
    {
        #region DataMembers

        ItemCodeBL CodeBl = new ItemCodeBL();

        static ItemCode_Type itmCodeTyp;
        static tblItem_Code itmCode;

        #endregion

        #region Properties

        public string Item_Code
        {
            get { return ViewState["Item_Code"].ToString(); }
            set { ViewState["Item_Code"] = value; }
        }

        public bool IsInPage
        {
            get { return Convert.ToBoolean(ViewState["IsInPage"]); }
            set { ViewState["IsInPage"] = value; }
        }

        public int BranchId
        {
            get { return (ViewState["BranchId"] == null) ? 0 : Convert.ToInt32(ViewState["BranchId"]); }
            set { ViewState["BranchId"] = value; }
        }

        public int ItemCodeLength
        {
            get { return (ViewState["ItemCodeLength"] == null) ? 0 : Convert.ToInt32(ViewState["ItemCodeLength"]); }
            set { ViewState["ItemCodeLength"] = value; }
        }

        public string ItemCodeType
        {
            get { return ViewState["ItemCodeType"].ToString(); }
            set { ViewState["ItemCodeType"] = value; }
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
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ProductSetup").ToString();

                if (Session["BranchID"] == null)
                {
                    BranchId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchId = Convert.ToInt32(Session["BranchID"]);
                }
                ItemCodeBL.BranchId = BranchId;
                
                ItemCodeType = "";
                Item_Code = "";
                IsInPage = false;
                IsEdit = false;

                BindDdlCodeType();
                BindDDLUOM();
                BindDdlTax();
                ddlCodeType_SelectedIndexChanged(null, null);
                BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));
            }
        }

        protected void ddlCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                itmCodeTyp = CodeBl.GetItemCodeType(Convert.ToChar(ddlCodeType.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ItemCodeLength = Convert.ToInt32(itmCodeTyp.ct_len);
                if (itmCodeTyp != null)
                {
                    txtCode.MaxLength = ItemCodeLength;
                    ItemCodeType = itmCodeTyp.ct_typ;

                    if (ItemCodeType.Equals("Group") || ItemCodeType.Equals("Control"))
                    {
                        DisableFields();
                    }
                    else
                    {
                        EnableFields();
                    }
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
                itmCode = CodeBl.GetItemCode(ddlCodeHead.SelectedValue, BranchId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (itmCode != null)
                {
                    //txtCode.Text = itmCode.itm_cd;//Control Item Code
                    txtCode.Text = CodeBl.GetCode(itmCode.itm_cd, Convert.ToChar(ddlCodeType.SelectedValue), ItemCodeLength, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                if (ItemCodeType.Equals("Group"))
                {
                    txtCode.Text = CodeBl.GetGroupCode(Convert.ToChar(ddlCodeType.SelectedValue), ItemCodeLength, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                Item_Code = grdcode.SelectedDataKey["itm_cd"].ToString().Trim();
                ItemCodeType = Convert.ToString(grdcode.SelectedDataKey["ct_typ"]);
                ddlCodeType.SelectedValue = Convert.ToString(grdcode.SelectedDataKey["ct_id"]);
                ddlCodeType_SelectedIndexChanged(null, null);

                if (!ItemCodeType.Equals("Group"))
                {
                    ddlCodeHead.SelectedValue = Convert.ToString(grdcode.SelectedDataKey["hdcode"]);
                }

                this.GetByID();
                IsEdit = true;

                grdcode.PageIndex = 0;
                IsInPage = false;
                BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));

                //-----------
                int row= grdcode.SelectedRow.RowIndex;
                Description = grdcode.Rows[row].Cells[1].Text;
                //----------
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void grdcode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdcode.PageIndex = e.NewPageIndex;
            BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));
        }

        protected void grdcode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text.Trim() == Item_Code)
                {
                    IsInPage = true;
                    e.Row.Attributes.Add("style", "background-color:#ffffcc");

                }
            }
        }

        protected void grdcode_DataBound(object sender, EventArgs e)
        {
            if (IsInPage == false && Item_Code != "")
            {
                this.grdcode.PageIndex = this.grdcode.PageIndex + 1;
                BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            grdcode.PageIndex = 0;
            BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsEdit)
            {
                //place check here
                if (!CodeBl.codeDescriptionExits(txtCode.Text, txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {

                    if (ItemCodeType.Equals("Group"))
                    {
                        SaveGroup();

                    }
                    else if (ItemCodeType.Equals("Control"))
                    {
                        SaveControl();
                    }
                    else
                    {
                        SaveDetail();
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Code or Description already exist.", RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                //place check here
                bool IsExist = false;
                if (txtDescription.Text != Description)
                {
                  IsExist=  CodeBl.checkIfDescriptionExist(txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    
                }

                if (IsExist == false)
                {
                    if (ItemCodeType.Equals("Group") || ItemCodeType.Equals("Control"))
                    {
                        EditCode();
                    }
                    else
                    {
                        EditDetail();
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Description is already exist.", RMS.BL.Enums.MessageType.Error);
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        #endregion

        #region Method

        private void SaveGroup()
        {
            try
            {
                if (txtCode.Text.Length.Equals(ItemCodeLength))
                {
                    bool res = false;
                    tblItem_Code itm = new tblItem_Code();

                    Item_Code = txtCode.Text;

                    itm.itm_cd = txtCode.Text;
                    itm.br_id = BranchId;
                    itm.itm_dsc = txtDescription.Text;
                    itm.cnt_itm_cd = txtCode.Text;
                    itm.ItemAlias = txtCode.Text;
                    itm.ItemSpecs = "";
                    itm.DrawingNo = "";
                    itm.BarCode = "";
                    itm.AltItem = txtCode.Text;
                    itm.status = "A";
                    itm.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    itm.ct_id = Convert.ToString(ddlCodeType.SelectedValue);
                    itm.uom_cd = null;
                    itm.Imp_Itm = "";
                    itm.Catg_cd = "1";
                    itm.Main_Catg = "1";
                    if (Session["UserName"] == null)
                        itm.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itm.updateby = Session["UserName"].ToString();
                    itm.itm_typ = "FG";

                    res = CodeBl.SaveItem(itm, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res)
                    {
                        grdcode.PageIndex = 0;
                        BindGrid("", "", 'M');
                        Item_Code = "";
                        IsInPage = false;

                        ddlCodeType.SelectedIndex = 0;
                        ddlCodeType_SelectedIndexChanged(null, null);

                        ucMessage.ShowMessage("Saved successfully.", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        itm = null;
                        ucMessage.ShowMessage("Save was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Code should be greater than 0 and " + ItemCodeLength + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        private void SaveControl()
        {
            try
            {
                if (txtCode.Text.Length.Equals(ItemCodeLength))
                {
                    bool res = false;
                    tblItem_Code itm = new tblItem_Code();

                    Item_Code = txtCode.Text;

                    itm.itm_cd = txtCode.Text;
                    itm.br_id = BranchId;
                    itm.itm_dsc = txtDescription.Text;
                    itm.cnt_itm_cd = ddlCodeHead.SelectedValue;
                    itm.ItemAlias = txtCode.Text;
                    itm.ItemSpecs = "";
                    itm.DrawingNo = "";
                    itm.BarCode = "";
                    itm.AltItem = ddlCodeHead.SelectedValue;
                    itm.status = "A";
                    itm.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    itm.ct_id = Convert.ToString(ddlCodeType.SelectedValue);
                    itm.uom_cd = null;
                    itm.Imp_Itm = "";
                    itm.Catg_cd = "1";
                    itm.Main_Catg = "1";
                    if (Session["UserName"] == null)
                        itm.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itm.updateby = Session["UserName"].ToString();
                    itm.itm_typ = "FG";

                    res = CodeBl.SaveItem(itm, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == true)
                    {
                        grdcode.PageIndex = 0;
                        BindGrid("", "", 'M');
                        Item_Code = "";
                        IsInPage = false;

                        ddlCodeType.SelectedIndex = 0;
                        ddlCodeType_SelectedIndexChanged(null, null);

                        ucMessage.ShowMessage("Saved successfully.", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        itm = null;
                        ucMessage.ShowMessage("Save was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Code should be greater than 0 and " + ItemCodeLength + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        private void SaveDetail()
        {
            try
            {
                if (txtCode.Text.Length.Equals(ItemCodeLength))
                {
                    bool res = false;
                    tblItem_Code itm = new tblItem_Code();

                    Item_Code = txtCode.Text;

                    itm.itm_cd = txtCode.Text;
                    if (Session["BranchID"] == null)
                        itm.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    else
                        itm.br_id = Convert.ToInt32(Session["BranchID"]);
                    itm.itm_dsc = txtDescription.Text;
                    itm.cnt_itm_cd = ddlCodeHead.SelectedValue;
                    itm.ItemAlias = txtCode.Text;
                    itm.ItemSpecs = txtSpecs.Text;
                    itm.DrawingNo = txtDrawingNo.Text;
                    itm.BarCode = txtBarCode.Text;
                    itm.AltItem = txtAltrCode.Text;
                    itm.status = "A";
                    itm.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    itm.ct_id = Convert.ToString(ddlCodeType.SelectedValue);
                    itm.uom_cd = Convert.ToByte(ddlUOM.SelectedValue);
                    if (rdbTypeImp.Checked == true)
                        itm.Imp_Itm = "Imp";
                    else
                        itm.Imp_Itm = "Local";
                    itm.Catg_cd = "1";
                    itm.Main_Catg = "1";
                    if (Session["UserName"] == null)
                        itm.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itm.updateby = Session["UserName"].ToString();
                    itm.itm_typ = "FG";

                    if (txtExpDays.Text.Trim() != "")
                    {
                        try
                        {
                            itm.Exp_Days = Convert.ToInt32(txtExpDays.Text);
                        }
                        catch
                        {
                            ucMessage.ShowMessage("Invalid expiry days", RMS.BL.Enums.MessageType.Error);
                            itm = null;
                            return;
                        }
                    }
                    itm.Batch = chkBatch.Checked;

                    if (ddlTax.SelectedValue != "0")
                    {
                        itm.TaxID = ddlTax.SelectedValue;
                    }
                    else
                    {
                        itm.TaxID = null;
                    }

                    res = CodeBl.SaveItem(itm, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == true)
                    {
                        grdcode.PageIndex = 0;
                        BindGrid("", "", 'M');
                        Item_Code = "";
                        IsInPage = false;

                        ddlCodeType.SelectedIndex = 0;
                        ddlCodeType_SelectedIndexChanged(null, null);

                        ucMessage.ShowMessage("Saved successfully.", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        itm = null;
                        ucMessage.ShowMessage("Save was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Code should be greater than 0 and " + ItemCodeLength + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
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
                tblItem_Code tblItemCode = new tblItem_Code();

                Item_Code = txtCode.Text;
                IsInPage = false;

                tblItemCode.itm_cd = txtCode.Text;
                tblItemCode.br_id = BranchId;
                tblItemCode.itm_dsc = txtDescription.Text;
                tblItemCode.itm_typ = "FG";

                if (CodeBl.updateItemCode(tblItemCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    grdcode.PageIndex = 0;
                    BindGrid("", "", 'M');
                    Item_Code = "";
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
                    tblItemCode = null;
                    ucMessage.ShowMessage("Update was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        private void EditDetail()
        {
            try
            {
                tblItem_Code tblItemCode = new tblItem_Code();

                Item_Code = txtCode.Text;
                IsInPage = false;

                tblItemCode.itm_cd = txtCode.Text;
                tblItemCode.br_id = BranchId;
                tblItemCode.itm_dsc = txtDescription.Text;
                tblItemCode.AltItem = txtAltrCode.Text;
                tblItemCode.uom_cd = Convert.ToByte(ddlUOM.SelectedValue);
                tblItemCode.ItemSpecs = txtSpecs.Text;
                tblItemCode.BarCode = txtBarCode.Text;
                tblItemCode.DrawingNo = txtDrawingNo.Text;
                if (rdbTypeImp.Checked)
                {
                    tblItemCode.Imp_Itm = "Imp";
                }
                else
                {
                    tblItemCode.Imp_Itm = "Local";
                }
                if (txtExpDays.Text.Trim() != "")
                {
                    try
                    {
                        tblItemCode.Exp_Days = Convert.ToInt32(txtExpDays.Text);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid expiry days", RMS.BL.Enums.MessageType.Error);
                        tblItemCode = null;
                        return;
                    }
                }
                tblItemCode.Batch = chkBatch.Checked;
                if (ddlTax.SelectedValue != "0")
                {
                    tblItemCode.TaxID = ddlTax.SelectedValue;
                }
                else
                {
                    tblItemCode.TaxID = null;
                }
                tblItemCode.itm_typ = "FG";

                if (CodeBl.updateItemCodeDet(tblItemCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    grdcode.PageIndex = 0;
                    BindGrid("", "", 'M');
                    Item_Code = "";
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
                    tblItemCode = null;
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

                tblItem_Code rec = CodeBl.GetRecByID(Item_Code, BranchId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (rec != null)
                {
                    if (ItemCodeType.Equals("Group") || ItemCodeType.Equals("Control"))
                    {
                        txtCode.Text = rec.itm_cd;
                        txtDescription.Text = rec.itm_dsc;

                        ddlCodeType.Enabled = false;
                        ddlCodeHead.Enabled = false;
                        txtCode.Enabled = false;
                    }
                    else
                    {
                        txtCode.Text = rec.itm_cd;
                        txtDescription.Text = rec.itm_dsc;
                        txtAltrCode.Text = rec.AltItem;
                        txtSpecs.Text = rec.ItemSpecs;
                        txtBarCode.Text = rec.BarCode;
                        txtDrawingNo.Text = rec.DrawingNo;
                        ddlUOM.SelectedValue = rec.uom_cd.ToString();
                        if (rec.Imp_Itm == "Local")
                        {
                            rdbTypeImp.Checked = false;
                            rdbTypeLocal.Checked = true;
                        }
                        else if (rec.Imp_Itm == "Imp")
                        {
                            rdbTypeLocal.Checked = false;
                            rdbTypeImp.Checked = true;
                        }

                        ddlCodeType.Enabled = false;
                        ddlCodeHead.Enabled = false;
                        txtCode.Enabled = false;
                        if (rec.Batch != null)
                            chkBatch.Checked = rec.Batch.Value;
                        else
                            chkBatch.Checked = false;
                        if (rec.TaxID != null)
                        {
                            ddlTax.SelectedValue = rec.TaxID;
                        }
                        else
                        {
                            ddlTax.SelectedValue = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindGrid(string cd, string dsc, char typ)
        {
            this.grdcode.DataSource = CodeBl.GetCodeDetail4Grid("FG", cd, dsc, typ, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdcode.DataBind();
        }

        public void BindDDLUOM()
        {
            ddlUOM.Items.Clear();
            ddlUOM.DataSource = CodeBl.GetUOM((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlUOM.DataTextField = "uom_dsc";
            ddlUOM.DataValueField = "uom_cd";
            ddlUOM.DataBind();
        }

        private void BindDdlCodeHead()
        {
            ddlCodeHead.DataSource = CodeBl.GetCodeHead("FG", itmCodeTyp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCodeHead.DataTextField = "itm_dsc";
            ddlCodeHead.DataValueField = "itm_cd";
            ddlCodeHead.DataBind();
        }

        private void BindDdlCodeType()
        {
            ddlCodeType.DataSource = CodeBl.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCodeType.DataTextField = "ct_dsc";
            ddlCodeType.DataValueField = "ct_id";
            ddlCodeType.DataBind();

            ddlFltCodeType.DataSource = CodeBl.GetCodeType((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltCodeType.DataTextField = "ct_dsc";
            ddlFltCodeType.DataValueField = "ct_id";
            ddlFltCodeType.DataBind();
        }
        
        private void BindDdlTax()
        {
            ddlTax.DataSource =  new TaxBL().GetGSTTaxes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlTax.DataTextField = "TaxDesc";
            ddlTax.DataValueField = "TaxID";
            ddlTax.DataBind();
        }

        private void ClearFields()
        {
            IsEdit = false;
            IsInPage = false;
            Item_Code = "";

            txtCode.Text = "";
            txtDescription.Text = "";
            txtAltrCode.Text = "";
            txtSpecs.Text = "";
            rdbTypeImp.Checked = true;
            txtBarCode.Text = "";
            txtDrawingNo.Text = "";

            ddlCodeType.Enabled = true;
            ddlCodeHead.Enabled = true;
            txtCode.Enabled = true;

            txtExpDays.Text = "";
            chkBatch.Checked = false;

            ddlCodeType.SelectedIndex = 0;
            ddlCodeType_SelectedIndexChanged(null, null);
        }

        private void DisableFields()
        {
            txtCode.Text = "";
            txtDescription.Text = "";
            txtAltrCode.Text = "";
            txtSpecs.Text = "";
            rdbTypeImp.Checked = true;
            txtBarCode.Text = "";
            txtDrawingNo.Text = "";

            txtAltrCode.Enabled = false;
            txtSpecs.Enabled = false;
            ddlUOM.Enabled = false;
            rdbTypeImp.Enabled = false;
            rdbTypeLocal.Enabled = false;
            txtBarCode.Enabled = false;
            txtDrawingNo.Enabled = false;

            txtExpDays.Enabled = false;
            chkBatch.Checked = false;
            chkBatch.Enabled = false;

            ddlTax.SelectedValue = "0";
            ddlTax.Enabled = false;
        }

        private void EnableFields()
        {
            txtCode.Text = "";
            txtDescription.Text = "";
            txtAltrCode.Text = "";
            txtSpecs.Text = "";
            rdbTypeImp.Checked = true;
            txtBarCode.Text = "";
            txtDrawingNo.Text = "";

            txtAltrCode.Enabled = true;
            txtSpecs.Enabled = true;
            ddlUOM.Enabled = true;
            rdbTypeImp.Enabled = true;
            rdbTypeLocal.Enabled = true;
            txtBarCode.Enabled = true;
            txtDrawingNo.Enabled = true;

            txtExpDays.Enabled = true;
            chkBatch.Checked = false;
            chkBatch.Enabled = true;

            ddlTax.SelectedValue = "0";
            ddlTax.Enabled = true;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object CodeVal(string selectedTypeId1, string selectedTypeId2)
        {
            ItemCodeBL codebl = new ItemCodeBL();
            return codebl.CodeVal(selectedTypeId1, selectedTypeId2, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
        }

        #endregion
    }
}

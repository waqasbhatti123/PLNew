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

namespace RMS.Inv
{
    public partial class InvItem_Code : BasePage
    {
        #region DataMembers

        InvCode InvCode = new InvCode();
        static tblItem_Code rec;//= new tblItem_Code();

        #endregion

        #region Properties

        //public tblItem_Code rec
        //{
        //    get { return (tblItem_Code)ViewState["rec"]; }
        //    set { ViewState["rec"] = value; }

        //}

#pragma warning disable CS0114 // 'InvItem_Code.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'InvItem_Code.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }

        }

        public int GroupCodeLen
        {
            get { return (ViewState["GroupCodeLen"] == null) ? 0 : Convert.ToInt32(ViewState["GroupCodeLen"]); }
            set { ViewState["GroupCodeLen"] = value; }

        }

        public int ContCodeLen
        {
            get { return (ViewState["ContCodeLen"] == null) ? 0 : Convert.ToInt32(ViewState["ContCodeLen"]); }
            set { ViewState["ContCodeLen"] = value; }

        }

        public int ItemCodeLen
        {
            get { return (ViewState["ItemCodeLen"] == null) ? 0 : Convert.ToInt32(ViewState["ItemCodeLen"]); }
            set { ViewState["ItemCodeLen"] = value; }

        }

        public string Item_Code
        {
            get { return ViewState["Item_Code"].ToString(); }
            set { ViewState["Item_Code"] = value; }

        }

        public char Item_Type
        {
            get { return Convert.ToChar(ViewState["Item_Type"]); }
            set { ViewState["Item_Type"] = value; }

        }


        public bool res
        {
            get { return Convert.ToBoolean(ViewState["res"]); }
            set { ViewState["res"] = value; }
        }


        public bool SaveIt 
        {
            get { return Convert.ToBoolean(ViewState["SaveIt"]); }
            set { ViewState["SaveIt"] = value; }
        }

        public bool UpdateIt
        {
            get { return Convert.ToBoolean(ViewState["UpdateIt"]); }
            set { ViewState["UpdateIt"] = value; }
        }

        public bool IsInPage
        {
            get { return Convert.ToBoolean(ViewState["IsInPage"]); }
            set { ViewState["IsInPage"] = value; }

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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ItemMasterSetup").ToString();
                
               
                GroupCodeLen= GetCodeLength('A', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                txtGroupCode.MaxLength = GroupCodeLen;
                ContCodeLen = GetCodeLength('C', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                txtControlCode.MaxLength = ContCodeLen - GroupCodeLen;
                ItemCodeLen = GetCodeLength('D', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                txtItemCode.MaxLength = ItemCodeLen - ContCodeLen;
                txtAltrCode.MaxLength = ItemCodeLen;

                BindDDLGroup();
                BindDDLControl();
                BindDDLUOM();

                DdlType_SelectedIndexChanged(null, null);

                Item_Code = "";
                
                //BindGrid("", "",Convert.ToChar("M"));
                BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));
                rdbTypeImp.Checked = true;
                
                SaveIt = false;
                UpdateIt = false;
             
            }

            
        }

        protected void DdlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue == "A")
            {
                trGroup.Visible = true;
                trGroup1.Visible = true;
                trControl.Visible = false;
                trControl1.Visible = false;
                trControl2.Visible = false;
                trDetail.Visible = false;
                trDetail2.Visible = false;
                trDetail3.Visible = false;
                trDetail4.Visible = false;
                trDetail5.Visible = false;
                trDetail6.Visible = false;
                trDetail7.Visible = false;

                txtGroupCode.Focus();
            }
            else if(ddlType.SelectedValue == "C")
            {
                trGroup.Visible = false;
                trGroup1.Visible = false;
                trControl.Visible = true;
                trControl1.Visible = true;
                trControl2.Visible = true;
                trDetail.Visible = false;
                trDetail2.Visible = false;
                trDetail3.Visible = false;
                trDetail4.Visible = false;
                trDetail5.Visible = false;
                trDetail6.Visible = false;
                trDetail7.Visible = false;

                ddlGroup.Focus();
            }
            else if (ddlType.SelectedValue == "D")
            {
                trGroup.Visible = false;
                trGroup1.Visible = false;
                trControl.Visible = false;
                trControl1.Visible = false;
                trControl2.Visible = false;
                trDetail.Visible = true;
                trDetail2.Visible = true;
                trDetail3.Visible = true;
                trDetail4.Visible = true;
                trDetail5.Visible = true;
                trDetail6.Visible = true;
                trDetail7.Visible = true;
              
                ddlControl.Focus();
            }
            else
            {
                trGroup.Visible = false;
                trGroup1.Visible = false;
                trControl.Visible = false;
                trControl1.Visible = false;
                trControl2.Visible = false;
                trDetail.Visible = false;
                trDetail2.Visible = false;
                trDetail3.Visible = false;
                trDetail4.Visible = false;
                trDetail5.Visible = false;
                trDetail6.Visible = false;
                trDetail7.Visible = false;

                ddlType.Focus();
            }
            ddlGroup_SelectedIndexChanged(null, null);
            ddlControl_SelectedIndexChanged(null, null);
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtControlHead.Text = ddlGroup.SelectedValue;
            txtControlCode.Focus();
        }

        protected void ddlControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtItemHead.Text = ddlControl.SelectedValue;
            txtItemCode.Focus();
        }

        protected void txtGroupCode_TexChanged(object sender, EventArgs e)
        {
            try
            {
                bool exits = false;
                if (txtGroupCode.Text.Length != GroupCodeLen)
                {
                    SaveIt = false;
                    txtGroupCode.Text = "";
                    ucMessage.ShowMessage("Group code should be greater than 0 and " + GroupCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                    txtGroupCode.Focus();
                }
                else
                {
                    if (Convert.ToInt32(txtGroupCode.Text) > 0)
                    {
                        tblItem_Code itmCd = new tblItem_Code();
                        itmCd.itm_cd = txtGroupCode.Text;
                        itmCd.ct_id = "A";

                        exits = InvCode.CheckIfCodeExists(itmCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (exits == false)
                        {
                            SaveIt = true;
                            txtGroup.Focus();
                        }
                        else
                        {
                            SaveIt = false;
                            txtGroupCode.Text = "";
                            ucMessage.ShowMessage("Group code already exists.", RMS.BL.Enums.MessageType.Error);
                            txtGroupCode.Focus();
                        }
                    }
                    else
                    {
                        SaveIt = false;
                        txtGroupCode.Text = "";
                        ucMessage.ShowMessage("Group code should be greater than 0 and " + GroupCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                        txtGroupCode.Focus();
                    }
                    
                }
            }
            catch
            {
                SaveIt = false;
                txtGroupCode.Text = "";
                ucMessage.ShowMessage("Group code should be greater than 0 and " + GroupCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                txtGroupCode.Focus();
            }
        }

        protected void txtGroup_TextChanged(object sender, EventArgs e)
        {
                bool exits = false;
       
                tblItem_Code itmCd = new tblItem_Code();
                itmCd.itm_dsc = txtGroup.Text;
                itmCd.ct_id = "A";

                exits = InvCode.CheckIfDscExists(itmCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (exits == false)
                {
                    SaveIt = true;
                }
                else
                {
                    SaveIt = false;
                    txtGroup.Text = "";
                    ucMessage.ShowMessage("Group description already exists.", RMS.BL.Enums.MessageType.Error);
                    txtGroup.Focus();
                } 
        }

        protected void txtControlCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool exits = false;
                if (txtControlCode.Text.Length != ContCodeLen - GroupCodeLen)
                {
                    SaveIt = false;
                    txtControlCode.Text = "";
                    ucMessage.ShowMessage("Control code should be greater than 0 and " + ContCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                    txtControlCode.Focus();
                }
                else
                {
                    if (Convert.ToInt32(txtControlCode.Text) > 0)
                    {
                        tblItem_Code itmCd = new tblItem_Code();
                        itmCd.itm_cd = txtControlHead.Text + txtControlCode.Text;
                        itmCd.ct_id = "C";

                        exits = InvCode.CheckIfCodeExists(itmCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (exits == false)
                        {
                            SaveIt = true;
                            txtContolName.Focus();
                        }
                        else
                        {
                            SaveIt = false;
                            txtControlCode.Text = "";
                            ucMessage.ShowMessage("Control code already exists.", RMS.BL.Enums.MessageType.Error);
                            txtControlCode.Focus();
                        }
                    }
                    else
                    {
                        SaveIt = false;
                        txtControlCode.Text = "";
                        ucMessage.ShowMessage("Control code should be greater than 0 and " + ContCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                        txtControlCode.Focus();
                    }

                }
            }
            catch
            {
                SaveIt = false;
                txtControlCode.Text = "";
                ucMessage.ShowMessage("Control code should be greater than 0 and " + ContCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                txtControlCode.Focus();
            }
        }

        protected void txtContolName_TextChanged(object sender, EventArgs e)
        {
            bool exits = false;

            tblItem_Code itmCd = new tblItem_Code();
            itmCd.itm_dsc = txtContolName.Text;
            itmCd.ct_id = "C";

            exits = InvCode.CheckIfDscExists(itmCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (exits == false)
            {
                SaveIt = true;
            }
            else
            {
                SaveIt = false;
                txtContolName.Text = "";
                ucMessage.ShowMessage("Control description already exists.", RMS.BL.Enums.MessageType.Error);
                txtContolName.Focus();
            }
        }

        protected void txtItemCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool exits = false;
                if (txtItemCode.Text.Length != ItemCodeLen - ContCodeLen)
                {
                    SaveIt = false;
                    txtItemCode.Text = "";
                    ucMessage.ShowMessage("Item code should be greater than 0 and " + ItemCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                    txtItemCode.Focus();
                }
                else
                {
                    if (Convert.ToInt32(txtItemCode.Text) > 0)
                    {
                        tblItem_Code itmCd = new tblItem_Code();
                        itmCd.itm_cd = txtItemHead.Text + txtItemCode.Text;
                        itmCd.ct_id = "D";

                        exits = InvCode.CheckIfCodeExists(itmCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (exits == false)
                        {
                            SaveIt = true;
                            txtAltrCode.Focus();
                        }
                        else
                        {
                            SaveIt = false;
                            txtItemCode.Text = "";
                            ucMessage.ShowMessage("Item code already exists.", RMS.BL.Enums.MessageType.Error);
                            txtItemCode.Focus();
                        }
                    }
                    else
                    {
                        SaveIt = false;
                        txtItemCode.Text = "";
                        ucMessage.ShowMessage("Item code should be greater than 0 and " + ItemCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                        txtItemCode.Focus();
                    }

                }
            }
            catch
            {
                SaveIt = false;
                txtItemCode.Text = "";
                ucMessage.ShowMessage("Item code should be greater than 0 and " + ItemCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
                txtItemCode.Focus();
            }
        }

        protected void txtAltrCode_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (txtAltrCode.Text.Length != ItemCodeLen)
            //    {
            //        SaveIt = false;
            //        txtAltrCode.Text = "";
            //        ucMessage.ShowMessage("Alternate code should be greater than 0 and " + ItemCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
            //        txtAltrCode.Focus();
            //    }
            //    else
            //    {
            //        if (Convert.ToInt32(txtAltrCode.Text) > 0)
            //        {
            //            SaveIt = true;
            //        }
            //        else
            //        {
            //            SaveIt = false;
            //            txtAltrCode.Text = "";
            //            ucMessage.ShowMessage("Altr. item code should be greater than 0 and " + ItemCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
            //            txtAltrCode.Focus();
            //        }

            //    }
            //}
            //catch
            //{
            //    SaveIt = false;
            //    txtAltrCode.Text = "";
            //    ucMessage.ShowMessage("Alternate code should be greater than 0 and " + ItemCodeLen + " digit/digits long.", RMS.BL.Enums.MessageType.Error);
            //    txtAltrCode.Focus();
            //}
        }

        protected void txtItemName_TextChanged(object sender, EventArgs e)
        {
            bool exits = false;

            tblItem_Code itmCd = new tblItem_Code();
            itmCd.itm_dsc = txtItemName.Text;
            itmCd.ct_id = "D";

            exits = InvCode.CheckIfDscExists(itmCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (exits == false)
            {
                SaveIt = true;
                txtSpecs.Focus();
            }
            else
            {
                SaveIt = true;
                txtItemName.Text = "";
                ucMessage.ShowMessage("Item description already exists.", RMS.BL.Enums.MessageType.Error);
                txtItemName.Focus();
            }
        }

        protected void grdcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Item_Code = grdcode.SelectedDataKey["itm_cd"].ToString().Trim();
                Item_Type = Convert.ToChar(grdcode.SelectedDataKey["ct_id"]);
                this.GetByID();
                UpdateIt = true;

                grdcode.PageIndex = 0;
                IsInPage = false;
                //BindGrid("", "", Convert.ToChar("M"));
                BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " +  ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void grdcode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdcode.PageIndex = e.NewPageIndex;
            //BindGrid("", "", Convert.ToChar("M"));
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
               // BindGrid("", "", Convert.ToChar("M"));
                BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));
            }
            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Inv/InvItem_Code.aspx?PID=502");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["LoginID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }

            if (UpdateIt == false)
            {
                if (ddlType.SelectedValue == "A")
                {
                    if (SaveIt == true)
                    {
                        SaveGroup();
                    }
                    else
                    {
                        ucMessage.ShowMessage("Please correct the entered data.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else if (ddlType.SelectedValue == "C")
                {
                    if (SaveIt == true)
                    {
                        SaveControl();
                    }
                    else
                    {
                        ucMessage.ShowMessage("Please correct the entered data.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else if (ddlType.SelectedValue == "D")
                {
                    if (SaveIt == true)
                    {
                        SaveDetail();
                    }
                    else
                    {
                        ucMessage.ShowMessage("Please correct the entered data.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                }
            }
            else
            {
                UpdateIt = false;
                if (trGroup.Visible == true)
                {
                    string itmDesc = txtGroup.Text;
                    
                   //----------Comment
                    if (InvCode.updateGroup_Control(Item_Code, Item_Type, itmDesc, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage("Update Successfully", RMS.BL.Enums.MessageType.Info);
                    }
                    //rec = InvCode.GetRecByID(Item_Code, Item_Type, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //rec.itm_dsc = txtGroup.Text;
                    //UpdateItemRecG();
                    //--------------------
                }
                if (trControl.Visible == true)
                {
                    //rec = InvCode.GetRecByID(Item_Code, Item_Type, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //rec.itm_dsc = txtContolName.Text;
                    //UpdateItemRec();
                    string itmDesc = txtContolName.Text;
                    if (InvCode.updateGroup_Control(Item_Code, Item_Type, itmDesc, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage("Update Successfully", RMS.BL.Enums.MessageType.Info);
                    }

                }
                if (trDetail.Visible == true)
                {
                    //rec = InvCode.GetRecByID(Item_Code, Item_Type, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //rec.AltItem = txtAltrCode.Text;
                    //rec.itm_dsc = txtItemName.Text;
                    //rec.ItemSpecs = txtSpecs.Text;
                    //rec.BarCode = txtBarCode.Text;
                    //rec.uom_cd = Convert.ToByte(ddlUOM.SelectedValue);
                    //rec.DrawingNo = txtDrawingNo.Text;
                    //if(rdbTypeLocal.Checked == true)
                    //{
                    //    rec.Imp_Itm = "Local";
                    //}
                    //else
                    //{
                    //    rec.Imp_Itm = "Imp";
                    //}
                    //UpdateItemRec();

                    string altItem = txtAltrCode.Text;
                    string itmDesc = txtItemName.Text;
                    string itmSpecs = txtSpecs.Text;
                    string barCode = txtBarCode.Text;
                    byte uom_code = Convert.ToByte(ddlUOM.SelectedValue);
                    string DrawingNo = txtDrawingNo.Text;
                    string ImpItmSts ;
                    if (rdbTypeLocal.Checked == true)
                    {
                        ImpItmSts = "Local";
                    }
                    else
                    {
                        ImpItmSts = "Imp";
                    }

                    if (InvCode.updateItemD(Item_Code, Item_Type,altItem,itmDesc,itmSpecs,barCode,uom_code,DrawingNo,ImpItmSts, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage("Update Successfully", RMS.BL.Enums.MessageType.Info);
                    }

                }
            }


            //if (res == true)
            //{
            //    ClearFields();
            //    BindDDLGroup();
            //    BindDDLControl();
            //    BindGrid("", "", Convert.ToChar("M"));
            //    res = false;
            //}
              
                BindDDLGroup();
                BindDDLControl();
                ClearFields();
                grdcode.PageIndex = 0;
            
                //BindGrid("", "", Convert.ToChar("M"));
                BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));
                Item_Code = "";
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            BindGrid(txtFltCode.Text, txtFltDesc.Text, Convert.ToChar(ddlFltCodeType.SelectedValue));
        }

        #endregion

        #region Helping Method

        public void SaveGroup()
        {
            try
            {
                res = false;
                if (txtGroup.Text == "" || txtGroupCode.Text == "")
                {
                    ucMessage.ShowMessage("Please enter group code/description.", RMS.BL.Enums.MessageType.Error);
                }
                else
                {
                    tblItem_Code itm = new tblItem_Code();
                    //----------------
                    Item_Code = txtGroupCode.Text;
                    //---------------

                    itm.itm_cd = txtGroupCode.Text;
                    if (Session["BranchID"] == null)
                        itm.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    else
                        itm.br_id = Convert.ToInt32(Session["BranchID"]);
                    itm.itm_dsc = txtGroup.Text;
                    itm.cnt_itm_cd = txtGroupCode.Text;
                    itm.ItemAlias = txtGroupCode.Text;
                    itm.ItemSpecs = "";
                    itm.DrawingNo = "";
                    itm.BarCode = "";
                    itm.AltItem = txtGroupCode.Text;
                    itm.status = "A";
                    itm.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    itm.ct_id =Convert.ToString(ddlType.SelectedValue);
                    itm.uom_cd = 0;
                    itm.Imp_Itm = "";
                    itm.Catg_cd = "1";
                    itm.Main_Catg = "1";
                    if (Session["UserName"] == null)
                        itm.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itm.updateby = Session["UserName"].ToString();


                    res = InvCode.SaveItem(itm, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == true)
                    {
                        ucMessage.ShowMessage("Group saved successfully.", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        ucMessage.ShowMessage("Save was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void SaveControl()
        {
            try
            {
                res = false;
                if (txtControlCode.Text == "" || txtContolName.Text == "" || ddlGroup.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please enter control code/description and select group name.", RMS.BL.Enums.MessageType.Error);
                }
                else
                {
                    tblItem_Code itm = new tblItem_Code();

                    //-----------
                    Item_Code = txtControlHead.Text + txtControlCode.Text;
                    //------------

                    itm.itm_cd = txtControlHead.Text + txtControlCode.Text;
                    if (Session["BranchID"] == null)
                        itm.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    else
                        itm.br_id = Convert.ToInt32(Session["BranchID"]);
                    itm.itm_dsc = txtContolName.Text;
                    itm.cnt_itm_cd = ddlGroup.SelectedValue;
                    itm.ItemAlias = txtControlHead.Text +  txtControlCode.Text;
                    itm.ItemSpecs = "";
                    itm.DrawingNo = "";
                    itm.BarCode = "";
                    itm.AltItem = ddlGroup.SelectedValue;
                    itm.status = "A";
                    itm.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    itm.ct_id = Convert.ToString(ddlType.SelectedValue);
                    itm.uom_cd = 0;
                    itm.Imp_Itm = "";
                    itm.Catg_cd = "1";
                    itm.Main_Catg = "1";
                    if (Session["UserName"] == null)
                        itm.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itm.updateby = Session["UserName"].ToString();


                    res = InvCode.SaveItem(itm, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == true)
                    {
                        ucMessage.ShowMessage("Control saved successfully.", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        ucMessage.ShowMessage("Save was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void SaveDetail()
        {
            try
            {
                res = false;
                if (txtItemCode.Text == "" || txtItemName.Text == "" || ddlControl.SelectedValue == "0" )
                {
                    ucMessage.ShowMessage("Please enter Item code/description and select control name.", RMS.BL.Enums.MessageType.Error);
                }
                else
                {
                    tblItem_Code itm = new tblItem_Code();

                    //-----------
                    Item_Code = txtItemHead.Text + txtItemCode.Text;
                    //------------

                    itm.itm_cd = txtItemHead.Text + txtItemCode.Text;
                    if (Session["BranchID"] == null)
                        itm.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    else
                        itm.br_id = Convert.ToInt32(Session["BranchID"]);
                    itm.itm_dsc = txtItemName.Text;
                    itm.cnt_itm_cd = ddlControl.SelectedValue;
                    itm.ItemAlias = txtItemHead.Text + txtItemCode.Text;
                    itm.ItemSpecs = txtSpecs.Text;
                    itm.DrawingNo = txtDrawingNo.Text;
                    itm.BarCode = txtBarCode.Text;
                    itm.AltItem = txtAltrCode.Text;
                    itm.status = "A";
                    itm.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    itm.ct_id = Convert.ToString(ddlType.SelectedValue);
                    itm.uom_cd = Convert.ToByte(ddlUOM.SelectedValue);
                    if(rdbTypeImp.Checked == true)
                        itm.Imp_Itm = "Imp";
                    else
                        itm.Imp_Itm = "Local";
                    itm.Catg_cd = "1";
                    itm.Main_Catg = "1";
                    if (Session["UserName"] == null)
                        itm.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itm.updateby = Session["UserName"].ToString();



                    res = InvCode.SaveItem(itm, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == true)
                    {
                        ucMessage.ShowMessage("Item saved successfully.", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        ucMessage.ShowMessage("Save was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public int GetCodeLength(char id, RMSDataContext Data)
        {
            try
            {
                return InvCode.GetTypeCodeLength(id, Data);
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
                return 0;
            }
        }

        public void BindGrid(string cd, string dsc, char typ)
        {
            this.grdcode.DataSource = InvCode.GetCodeDetail4Grid(cd, dsc, typ, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdcode.DataBind();
        }

        public void BindDDLGroup()
        {
            ddlGroup.Items.Clear();
            ddlGroup.DataSource = InvCode.GetGroups('A',(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlGroup.DataTextField = "itm_dsc";
            ddlGroup.DataValueField = "itm_cd";
            ddlGroup.DataBind();
        }

        public void BindDDLControl()
        {
            ddlControl.Items.Clear();
            ddlControl.DataSource = InvCode.GetControlss('C', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlControl.DataTextField = "itm_dsc";
            ddlControl.DataValueField = "itm_cd";
            ddlControl.DataBind();
        }

        public void BindDDLUOM()
        {
            ddlUOM.Items.Clear();
            ddlUOM.DataSource = InvCode.GetUOM('C', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlUOM.DataTextField = "uom_dsc";
            ddlUOM.DataValueField = "uom_cd";
            ddlUOM.DataBind();
        }

        public void GetByID()
        {
            try
            {

                rec = InvCode.GetRecByID(Item_Code, Item_Type, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (rec != null)
                {
                    ddlType.Enabled = false;
                    ddlType.SelectedValue = rec.ct_id.ToString();
                    DdlType_SelectedIndexChanged(null, null);
                    if (trGroup.Visible == true)
                    {
                        txtGroupCode.Enabled = false;
                        txtGroupCode.Text = rec.itm_cd;
                        txtGroup.Text = rec.itm_dsc;
                    }
                    if (trControl.Visible == true)
                    {
                        ddlGroup.Enabled = false;
                        txtControlCode.Enabled = false;
                        ddlGroup.SelectedValue = rec.cnt_itm_cd;
                        ddlGroup_SelectedIndexChanged(null, null);
                        txtControlCode.Text = rec.itm_cd.Substring(GroupCodeLen);
                        txtContolName.Text = rec.itm_dsc;
                    }
                    if (trDetail.Visible == true)
                    {
                        ddlControl.Enabled = false;
                        txtItemCode.Enabled = false;
                        ddlControl.Text = rec.cnt_itm_cd;
                        ddlControl_SelectedIndexChanged(null, null);
                        txtAltrCode.Text = rec.AltItem;
                        txtItemCode.Text = rec.itm_cd.Substring(ContCodeLen);
                        txtItemName.Text = rec.itm_dsc;
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
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
            }
        }
        public void UpdateItemRec()
        {
            res = InvCode.UpdateItem(rec, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (res == true)
            {
                UpdateIt = false;
                ucMessage.ShowMessage("Updated successfully.", RMS.BL.Enums.MessageType.Info);
            }
            else
            {
                ucMessage.ShowMessage("Update was unsuccessful.", RMS.BL.Enums.MessageType.Error);
            }
        }

        public void ClearFields()
        {
            //ddlType.SelectedValue = "0";
            SaveIt = false;
            UpdateIt = false;

            //------------------
            
            IsInPage = false;
            //------------------
            if (trGroup.Visible == true)
            {
                txtGroupCode.Text = "";
                txtGroup.Text = "";
            }

            if (trControl.Visible == true)
            {
                //ddlGroup.SelectedValue = "0";
                
                txtControlHead.Text = "";
                txtControlCode.Text = "";
                txtContolName.Text = "";
                txtControlHead.Text = "";
                ddlGroup_SelectedIndexChanged(null,null);
            }

            if (trDetail.Visible == true)
            {
                //ddlControl.SelectedValue = "0";
                
                txtItemHead.Text = "";
                txtItemCode.Text = "";
                txtAltrCode.Text = "";
                txtItemName.Text = "";
                txtSpecs.Text = "";
                //rdbTypeLocal.Checked = true;
                txtBarCode.Text = "";
                txtDrawingNo.Text = "";

                ddlControl_SelectedIndexChanged(null,null);
            }
        }

        #endregion

    }
}

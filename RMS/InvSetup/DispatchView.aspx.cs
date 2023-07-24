using System;
using RMS.BL;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Text.RegularExpressions; 

namespace RMS
{
    public partial class DispatchView : BasePage
    {
        #region DataMembers
        
        EntitySet<tblItemDataDet> itmDetEnt = new EntitySet<tblItemDataDet>();
        List<tblItem> SelList = new List<tblItem>();
        InvMN_BL mN = new InvMN_BL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;

        #endregion

        #region Properties

        
        public DataTable CurrentTable
        {
            set { ViewState["CurrentTable"] = value; }
            get { return (DataTable)(ViewState["CurrentTable"] ?? 0); }
        }

        public string Item_Code
        {
            set { ViewState["Item_Code"] = value; }
            get { return Convert.ToString(ViewState["Item_Code"]); }
        }

        public string Des_Code
        {
            set { ViewState["Des_Code"] = value; }
            get { return Convert.ToString(ViewState["Des_Code"]); }
        }

        public int vrCode
        {
            set { ViewState["vrCode"] = value; }
            get { return Convert.ToInt32(ViewState["vrCode"] ?? 0); }
        }

        public int locID
        {
            set { ViewState["locID"] = value; }
            get { return Convert.ToInt32(ViewState["locID"] ?? 0); }
        }

        public int ToLocID
        {
            set { ViewState["ToLocID"] = value; }
            get { return Convert.ToInt32(ViewState["ToLocID"] ?? 0); }
        }

        public int SumHalfQty
        {
            set { ViewState["SumHalfQty"] = value; }
            get { return Convert.ToInt32(ViewState["SumHalfQty"] ?? 0); }
        }

        public int SumFullQty
        {
            set { ViewState["SumFullQty"] = value; }
            get { return Convert.ToInt32(ViewState["SumFullQty"] ?? 0); }
        }

        public int vrNum
        {
            set { ViewState["vrNum"] = value; }
            get { return Convert.ToInt32(ViewState["vrNum"] ?? 0); }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            vrCode = 28;
            locID = 4;
            ToLocID = 4;
            if (!IsPostBack)
            {
                int GroupID = 0;
                ClearFields();
                if (Session["GroupID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                }
                else
                {
                    GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                }

                if (Session["DateFormat"] == null)
                {
                    dateExtender.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    dateExtender.Format = Session["DateFormat"].ToString();
                }
                dateExtender.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                
                BindDDLItem();
                BindDDLDesign();
                BindDDLColor();
                BindDDLThickness();
                BindDDLCustomer();

                if (Convert.ToBoolean(Session["IfEdit"]) == false)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "DispatchMgt").ToString();

                    GetDispatchNo();
                    btnBack.Visible = false;
                    btnClear.Visible = true;
                    btnSave.Visible = false;
                    btnList.Visible = true;
                    rbdPending.Checked = true;
                }
                else
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", Session["PgTile"].ToString()).ToString();
                    BindEditFields();
                    BindEditTable();
                    ddlCustomer.Enabled = false;
                    ddlItem.Enabled = false;
                    ddlDesign.Enabled = false;
                    btnClear.Visible = false;
                    btnBack.Visible = false;
                    btnList.Visible = true;
                    btnSave.Visible = false;
                    pnlGrd.Visible = true;
                    pnlGrd.Enabled = false;
                }

            }
      
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearFields();
            Des_Code = ddlDesign.SelectedValue;
            Item_Code = ddlItem.SelectedValue;

            SelList = mN.GetSelectionByDesignId(Item_Code, Des_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (SelList.Count > 0)
            {
                pnlGrd.Visible = true;
                btnSave.Visible = true;
                GetDesignById();
                BindTable();
            }
            else
            {
                ClearFields();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void ddlDesign_SelectedIndexChanged(object sednder, EventArgs e)
        {
            
            ClearFields();
            Des_Code = ddlDesign.SelectedValue;
            Item_Code = ddlItem.SelectedValue;

            SelList = mN.GetSelectionByDesignId(Item_Code, Des_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            
            if (SelList.Count > 0)
            {
                pnlGrd.Visible = true;
                btnSave.Visible = true;
                GetDesignById();
                BindTable();
            }
            else
            {
                ClearFields();
            }

        }

        protected void txtQuantity_OnTextChanged(object sender, EventArgs e)
        {
            GridViewRow clickedrow = ((TextBox)sender).NamingContainer as GridViewRow;
            decimal qty = 0;
            decimal purqty = 0;

            if (((TextBox)clickedrow.FindControl("txtPurQuantity")).Text != "")
            purqty = Convert.ToDecimal(((TextBox)clickedrow.FindControl("txtPurQuantity")).Text);
            qty = Convert.ToDecimal(((TextBox)clickedrow.FindControl("txtQuantity")).Text);

            if (qty > purqty)
            {
                ucMessage.ShowMessage("Full pieces should be less than or equal to purchase pieces.", RMS.BL.Enums.MessageType.Error);
                ((TextBox)clickedrow.FindControl("txtQuantity")).Text = "";
                ((TextBox)clickedrow.FindControl("txtQuantity")).Focus();
            }
            else
            {
                ((TextBox)clickedrow.FindControl("txtHalfQuantity")).Focus();
            }
        }

        protected void txtHalQty_OnTextChanged(object sender, EventArgs e)
        {
            GridViewRow clickedrow = ((TextBox)sender).NamingContainer as GridViewRow;
            decimal halfqty = 0;
            decimal purhalfqty = 0;

            if (((TextBox)clickedrow.FindControl("txtHalfPurQuantity")).Text != "")
            purhalfqty = Convert.ToDecimal(((TextBox)clickedrow.FindControl("txtHalfPurQuantity")).Text);
            halfqty = Convert.ToDecimal(((TextBox)clickedrow.FindControl("txtHalfQuantity")).Text);

            if (halfqty > purhalfqty)
            {
                ucMessage.ShowMessage("Half pieces should be less than or equal to purchase pieces.", RMS.BL.Enums.MessageType.Error);
                ((TextBox)clickedrow.FindControl("txtHalfQuantity")).Text = "";
                ((TextBox)clickedrow.FindControl("txtHalfQuantity")).Focus();
            }
            else
            {
                ((TextBox)clickedrow.FindControl("txtArea")).Focus();
            }
        }

        protected void txtArea_OnTextChanged(object sender, EventArgs e)
        {
            GridViewRow clickedrow = ((TextBox)sender).NamingContainer as GridViewRow;
            decimal area = 0;
            decimal purarea= 0;
            
            if (((TextBox)clickedrow.FindControl("txtPurArea")).Text != "")
            purarea = Convert.ToDecimal(((TextBox)clickedrow.FindControl("txtPurArea")).Text);
            area = Convert.ToDecimal(((TextBox)clickedrow.FindControl("txtArea")).Text);

            if (area > purarea)
            {
                ucMessage.ShowMessage("Area should be less than or equal to purchase area.", RMS.BL.Enums.MessageType.Error);
                ((TextBox)clickedrow.FindControl("txtArea")).Text = "";
                ((TextBox)clickedrow.FindControl("txtArea")).Focus();
            }
            else
            {
                ((TextBox)GridView1.Rows[clickedrow.RowIndex+1].FindControl("txtQuantity")).Focus();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/dispatchmgt.aspx?PID=466");
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invSetup/dispatchrhome.aspx?PID=448");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["IfEdit"]) == false)
            {
                Save();
            }
            else
            {
                Edit();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["PrePgAddress"].ToString());
        }

        #endregion
        
        #region Helping Method

        public void Save()
        {
            try
            {
                //tblItemData============================================
                tblItemData itmData = new tblItemData();

                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);
                string no = txtTrnsfrNo.Text.Substring(5);
                string yrno = txtTrnsfrNo.Text.Substring(0, 4) + no;
                vrNum = Convert.ToInt32(yrno);

                itmData.vr_no = Convert.ToInt32(yrno);
                itmData.vr_dt = Convert.ToDateTime(txtDate.Text);
                itmData.vt_cd = Convert.ToInt16(vrCode);
                itmData.vr_nrtn = "";
                itmData.gl_cd = ddlCustomer.SelectedValue;
                itmData.post_2gl = true;
                itmData.LocId = Convert.ToInt16(locID);
                itmData.LotNo = 0;
                itmData.ToLocId = Convert.ToInt16(ToLocID);
                itmData.Freight = 0;
                itmData.Tax = 0;
                itmData.Commission = 0;
                itmData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                if (rbdApprove.Checked == true)
                {
                    itmData.vr_apr = "A";
                }
                else if (rbdPending.Checked == true)
                {
                    itmData.vr_apr = "P";
                }
                else
                {
                    itmData.vr_apr = "C";
                }

                if (rbdApprove.Checked == true || rbdCancelled.Checked == true)
                {
                    if (Session["UserName"] == null)
                        itmData.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itmData.updateby = Session["UserName"].ToString();
                }

                itmData.Status = "";
                itmData.DocRef = "";

                //tblItemDataDet=========================================
                int srCount = 1;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string fullQty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtQuantity")).Text;
                    string halfQty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtHalfQuantity")).Text;
                    string area = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtArea")).Text;

                    if ((fullQty != "" || halfQty != "") && area != "")
                    {

                        tblItemDataDet itmDet = new tblItemDataDet();
                        
                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = Convert.ToInt16(vrCode);
                        string nos = txtTrnsfrNo.Text.Substring(5);
                        string yrnos = txtTrnsfrNo.Text.Substring(0, 4) + no;
                        
                        itmDet.vr_no = Convert.ToInt32(yrnos);
                        itmDet.vr_seq = Convert.ToByte(srCount++);
                        itmDet.itm_cd = Item_Code;
                        itmDet.LocId = Convert.ToInt16(locID);
                        itmDet.KGSwt = 0;
                        itmDet.vr_val = 0;
                        itmDet.vr_rate = 0;
                        itmDet.Feetage = Math.Round(Convert.ToDecimal(area), 2);
                        itmDet.IGP_Ref = 0;
                        itmDet.vr_rmk = "";
                        itmDet.GradeId = "";
                        itmDet.LotRef = 0;
                        itmDet.SelectionId = "";
                        itmDet.DesignId = ddlDesign.SelectedValue;
                        itmDet.ColorId = ddlColor.SelectedValue;
                        itmDet.ThickId =Convert.ToInt16(ddlThickness.SelectedValue);
                        itmDet.GradeId = ((HiddenField)(GridView1.Rows[i].Cells[0].FindControl("hdnGrade"))).Value;
                        itmDet.SelectionId = ((HiddenField)(GridView1.Rows[i].Cells[0].FindControl("hdnSelection"))).Value;
                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        itmDet.DrumNo = 0;
                        if (fullQty != "")
                            itmDet.vr_qty = Convert.ToInt32(fullQty);
                        else
                            itmDet.vr_qty = 0;
                        if (halfQty != "")
                            itmDet.vr_half = Convert.ToInt32(halfQty);
                        else
                            itmDet.vr_half = 0;

                        itmDetEnt.Add(itmDet);
                    }
                }
                //Save===================================================
                bool res = false;

                if (itmDetEnt.Count() > 0)
                    res = mN.SaveItemData(itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                else
                    res = false;


                if (res == true)
                {
                    //Post to tblItem
                    if (rbdApprove.Checked == true)
                    {
                        Post2TblItem(vrNum);
                    }

                    ucMessage.ShowMessage("Saved successfully", RMS.BL.Enums.MessageType.Info);
                    Response.Redirect("~/invSetup/dispatchrhome.aspx?PID=448");
                }
                else
                {
                    ucMessage.ShowMessage("Save was unsuccessful, Please provide atleast one record.", RMS.BL.Enums.MessageType.Error);
                }
   
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void Edit()
        {
            try
            {
                //tblItemData============================================
                tblItemData itmData = new tblItemData();

                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);
                string no = txtTrnsfrNo.Text.Substring(5);
                string yrno = txtTrnsfrNo.Text.Substring(0, 4) + no;
                vrNum = Convert.ToInt32(yrno);

                itmData.vr_no = Convert.ToInt32(yrno);
                itmData.vr_dt = Convert.ToDateTime(txtDate.Text);
                itmData.vt_cd = Convert.ToInt16(vrCode);
                itmData.vr_nrtn = "";
                itmData.gl_cd = ddlCustomer.SelectedValue;
                itmData.post_2gl = true;
                itmData.LocId = Convert.ToInt16(locID);
                itmData.LotNo = 0;
                itmData.ToLocId = Convert.ToInt16(ToLocID);
                itmData.Freight = 0;
                itmData.Tax = 0;
                itmData.Commission = 0;
                itmData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                if (rbdApprove.Checked == true)
                {
                    itmData.vr_apr = "A";
                }
                else if (rbdPending.Checked == true)
                {
                    itmData.vr_apr = "P";
                }
                else
                {
                    itmData.vr_apr = "C";
                }

                if (rbdApprove.Checked == true || rbdCancelled.Checked == true)
                {
                    if (Session["UserName"] == null)
                        itmData.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itmData.updateby = Session["UserName"].ToString();
                }

                itmData.Status = "";
                itmData.DocRef = "";

                //tblItemDataDet=========================================
                int srCount = 1;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string fullQty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtQuantity")).Text;
                    string halfQty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtHalfQuantity")).Text;
                    string area = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtArea")).Text;

                    if ((fullQty != "" || halfQty != "") && area != "")
                    {

                        tblItemDataDet itmDet = new tblItemDataDet();

                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = Convert.ToInt16(vrCode);
                        string nos = txtTrnsfrNo.Text.Substring(5);
                        string yrnos = txtTrnsfrNo.Text.Substring(0, 4) + no;

                        itmDet.vr_no = Convert.ToInt32(yrnos);
                        itmDet.vr_seq = Convert.ToByte(srCount++);
                        itmDet.itm_cd = Item_Code;
                        itmDet.LocId = Convert.ToInt16(locID);
                        itmDet.KGSwt = 0;
                        itmDet.vr_val = 0;
                        itmDet.vr_rate = 0;
                        itmDet.Feetage = Math.Round(Convert.ToDecimal(area), 2);
                        itmDet.IGP_Ref = 0;
                        itmDet.vr_rmk = "";
                        itmDet.GradeId = "";
                        itmDet.LotRef = 0;
                        itmDet.SelectionId = "";
                        itmDet.DesignId = ddlDesign.SelectedValue;
                        itmDet.ColorId = ddlColor.SelectedValue;
                        itmDet.ThickId = Convert.ToInt16(ddlThickness.SelectedValue);
                        itmDet.GradeId = ((HiddenField)(GridView1.Rows[i].Cells[0].FindControl("hdnGrade"))).Value;
                        itmDet.SelectionId = ((HiddenField)(GridView1.Rows[i].Cells[0].FindControl("hdnSelection"))).Value;
                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        itmDet.DrumNo = 0;
                        if (fullQty != "")
                            itmDet.vr_qty = Convert.ToInt32(fullQty);
                        if (halfQty != "")
                            itmDet.vr_half = Convert.ToInt32(halfQty);

                        itmDetEnt.Add(itmDet);
                    }
                }
                //Edit===================================================
                bool res = false;
                string vrNo = Session["vrNo"].ToString();
                vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);
                int yr = Convert.ToInt32(vrNo.Substring(0, 4));

                if (itmDetEnt.Count() > 0)
                    res = mN.EditRecWetBlu(Convert.ToInt32(vrNo), yr, vrCode, itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                else
                    res = false;


                if (res == true)
                {
                    //Post to tblItem
                    if (rbdApprove.Checked == true)
                    {
                        Post2TblItem(vrNum);
                    }

                    ucMessage.ShowMessage("Updated successfully.", RMS.BL.Enums.MessageType.Info);
                    Response.Redirect("~/invSetup/dispatchrhome.aspx?PID=448");
                }
                else
                {
                    ucMessage.ShowMessage("Update was unsuccessful, please provide atleast one record.", RMS.BL.Enums.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void Post2TblItem(int vrNo)
        {
            if (vrCode == 0)
            {
                vrCode = 28;
                locID = 4;
                ToLocID = 4;
            }

            List<tblItemDataDet> lstDet = mN.GetItemDetUsingVrNo(vrNo, vrCode,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (lstDet.Count > 0)
            {
                foreach (tblItemDataDet tbl in lstDet)
                {
                    tblItem Item = mN.GetItemUsingSelectionDesgin(tbl.itm_cd, tbl.SelectionId, tbl.DesignId, tbl.LocId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    if (Item != null)
                    {
                        //Issuing finish goods                        
                        mN.GetUpdTblItemDispatch(tbl.LocId, tbl.itm_cd, tbl.SelectionId, tbl.DesignId, Convert.ToInt32(tbl.vr_qty), Convert.ToInt32(tbl.vr_half),Convert.ToDecimal(tbl.Feetage) ,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                }
            }
        }

        public void GetDispatchNo()
        {
            txtTrnsfrNo.Text = mN.GetSGNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        public void GetDesignById()
        {
            try
            {
                tblItemDesign design = mN.GetDesignByID(ddlDesign.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                ddlThickness.SelectedValue = Convert.ToString(design.ThickId);
                ddlColor.SelectedValue = design.ColorId;
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindDDLItem()
        {
            ddlItem.DataSource = mN.GetProduct((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataTextField = "itm_dsc";
            ddlItem.DataValueField = "itm_cd";
            ddlItem.DataBind();
        }

        public void BindDDLColor()
        {
            ddlColor.DataSource = mN.GetColor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlColor.DataTextField = "Color";
            ddlColor.DataValueField = "ColorId";
            ddlColor.DataBind();
        }

        public void BindDDLThickness()
        {
            ddlThickness.DataSource = mN.GetThickness((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlThickness.DataTextField = "Thick_Desc";
            ddlThickness.DataValueField = "ThickId";
            ddlThickness.DataBind();
        }

        public void BindDDLDesign()
        {
            ddlDesign.DataSource = mN.GetDesign((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDesign.DataTextField = "Design_Desc";
            ddlDesign.DataValueField = "DesignId";
            ddlDesign.DataBind();
        }

        public void BindDDLCustomer()
        {
            ddlCustomer.DataSource = mN.GetCustomer((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCustomer.DataTextField = "gl_dsc";
            ddlCustomer.DataValueField = "gl_cd";
            ddlCustomer.DataBind();
        }

        public void GetColumns()
        {
            d_table.Columns.Clear();
            d_table.Rows.Clear();

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Grade";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "HdnGrade";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "HdnSelection";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "PurFullQty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "PurHalfQty";
            d_table.Columns.Add(d_Col);


            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "PurArea";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "FullQty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "HalfQty";
            d_table.Columns.Add(d_Col);


            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Area";
            d_table.Columns.Add(d_Col);


        }

        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }

        public void BindTable()
        {
            GetColumns();
            
            decimal fullPur = 0;
            decimal halfPur = 0;
            decimal areaPur = 0;
            decimal fullPending = 0;
            decimal halfPending = 0;
            decimal areaPendign = 0;

            List<tblStkGradeDet> lstGrade = mN.GetGrades(Item_Code, ToLocID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
     
            for (int i = 0; i < lstGrade.Count(); i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Grade"] = lstGrade[i].SizeGrade_Desc;
                d_Row["HdnGrade"] = lstGrade[i].GradeId;
                d_Row["HdnSelection"] = lstGrade[i].SelectionId;
                foreach (tblItem itm in SelList)
                {
                    if (itm.SelectionId == lstGrade[i].SelectionId)
                    {
                        spGetDispatchValResult rec = mN.GetDispatchPendingQty(Item_Code, Des_Code, locID, itm.SelectionId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        if (rec != null)
                        {
                            fullPending = Convert.ToDecimal(rec.qtyfull);
                            halfPending = Convert.ToDecimal(rec.qtyhalf);
                            areaPendign = Convert.ToDecimal(rec.area);
                        }
                        else
                        {
                            fullPending = 0;
                            halfPending = 0;
                            areaPendign = 0;
                        }
                        
                        fullPur = Convert.ToDecimal(itm.itm_pur_qty) - fullPending;
                        halfPur = Convert.ToDecimal(itm.itm_pur_half) - halfPending;
                        areaPur = Convert.ToDecimal(itm.itm_pur_sqft) - areaPendign;

                        d_Row["PurFullQty"] = Convert.ToInt32(fullPur).ToString();
                        d_Row["PurHalfQty"] = Convert.ToInt32(halfPur).ToString();
                        d_Row["PurArea"] = Convert.ToInt32(areaPur).ToString("F");
                    }
                }
                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }

        public void BindEditFields()
        {
            int count = 0;
            string vrNo = Session["vrNo"].ToString();
            vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);
            int yr = Convert.ToInt32(vrNo.Substring(0, 4));

            tblItemData record = mN.GetRecWetBlueTrnsfr(Convert.ToInt32(vrNo), yr, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblItemDataDet> recordsDet = mN.GetRecDetWetBlueTrnsfr(Convert.ToInt32(vrNo), yr, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            txtTrnsfrNo.Text = record.vr_no.ToString().Substring(0, 4) + "/" + record.vr_no.ToString().Substring(4);
            dateExtender.SelectedDate = record.vr_dt;

            if (record.vr_apr == "A")
            {
                rbdApprove.Checked = true;
            }
            else if (record.vr_apr == "P")
            {
                rbdPending.Checked = true;
            }
            else
            {
                rbdCancelled.Checked = true;
            }
            foreach (var rec in recordsDet)
            {
                if (count == 0)
                {
                    ddlCustomer.SelectedValue = record.gl_cd;
                    ddlItem.SelectedValue = rec.itm_cd;
                    ddlDesign.SelectedValue = rec.DesignId;
                    ddlColor.SelectedValue = rec.ColorId;
                    ddlThickness.SelectedValue = Convert.ToString(rec.ThickId);

                    Item_Code = rec.itm_cd;
                    Des_Code = rec.DesignId;
                    count++;
                }
            }
        }

        public void BindEditTable()
        {
            try
            {
                GetColumns();

                string vrNo = Session["vrNo"].ToString();
                vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);
                int yr = Convert.ToInt32(vrNo.Substring(0, 4));

                decimal fullPur = 0;
                decimal halfPur = 0;
                decimal areaPur = 0;
                decimal fullPending = 0;
                decimal halfPending = 0;
                decimal areaPendign = 0;
                decimal vrfull = 0;
                decimal vrhalf = 0;
                decimal vrarea = 0;

                SelList = mN.GetSelectionByDesignId(Item_Code, Des_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                List<tblItemDataDet> recordsDet = mN.GetItemDetUsingVrNo(Convert.ToInt32(vrNo), vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                List<tblStkGradeDet> lstGrade = mN.GetGrades(Item_Code, ToLocID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                for (int j = 0; j < lstGrade.Count(); j++)
                {
                    d_Row = d_table.NewRow();

                    d_Row["Grade"] = lstGrade[j].SizeGrade_Desc;
                    d_Row["HdnGrade"] = lstGrade[j].GradeId;
                    d_Row["HdnSelection"] = lstGrade[j].SelectionId;

                    foreach (tblItem itm in SelList)
                    {
                        if (itm.SelectionId == lstGrade[j].SelectionId)
                        {

                            tblItemDataDet det = mN.GetDispatchCurrentRecDet(vrCode, Convert.ToInt32(vrNo), locID, Item_Code, Des_Code, itm.SelectionId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                            if (det != null)
                            {
                                vrfull = det.vr_qty;
                                vrhalf = Convert.ToDecimal(det.vr_half);
                                vrarea = Convert.ToDecimal(det.Feetage);
                            }
                            else
                            {
                                vrfull = 0;
                                vrhalf = 0;
                                vrarea = 0;
                            }


                            spGetDispatchValResult rec = mN.GetDispatchPendingQty(Item_Code, Des_Code, locID, itm.SelectionId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                            if (rec != null)
                            {
                                fullPending = Convert.ToDecimal(rec.qtyfull);
                                halfPending = Convert.ToDecimal(rec.qtyhalf);
                                areaPendign = Convert.ToDecimal(rec.area);
                            }
                            else
                            {
                                fullPending = 0;
                                halfPending = 0;
                                areaPendign = 0;
                            }

                            fullPur = Convert.ToDecimal(itm.itm_pur_qty) - fullPending  ;//+ vrfull;
                            halfPur = Convert.ToDecimal(itm.itm_pur_half) - halfPending ;//+ vrhalf;
                            areaPur = Convert.ToDecimal(itm.itm_pur_sqft) - areaPendign ;//+ vrarea;

                            d_Row["PurFullQty"] = Convert.ToInt32(fullPur).ToString();
                            d_Row["PurHalfQty"] = Convert.ToInt32(halfPur).ToString();
                            d_Row["PurArea"] = Convert.ToInt32(areaPur).ToString("F");
                        }
                    }

                    for (int k = 0; k < recordsDet.Count; k++)
                    {
                        if (lstGrade[j].GradeId == recordsDet[k].GradeId)
                        {
                            d_Row["FullQty"] = Convert.ToInt32(recordsDet[k].vr_qty);
                            d_Row["HalfQty"] = Convert.ToInt32(recordsDet[k].vr_half);
                            d_Row["Area"] = Math.Round(Convert.ToDecimal(recordsDet[k].Feetage), 2).ToString();
                        }
                    }

                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;
                BindGrid();
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }

        }

        public void ClearFields()
        {
            pnlGrd.Visible = false;
            btnSave.Visible = false;
            ddlColor.SelectedValue = "0";
            ddlThickness.SelectedValue = "0";
            //txtStkHalf.Text = "0";
            //txtStkFull.Text = "0";
            ddlCustomer.SelectedValue = "0";
            vrNum = 0;
            SumHalfQty = 0;
            SumFullQty = 0;
        }

        #endregion
    }
}
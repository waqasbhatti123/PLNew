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
    public partial class GradingCardMgt : BasePage
    {
        #region DataMembers

        //DashBoardBL dashBoardBL = new DashBoardBL();
        InvGP_BL gP = new InvGP_BL();
        InvSG_BL sG = new InvSG_BL();
        InvGC_BL gC = new InvGC_BL();
        #endregion

        #region Properties

        EntitySet<tblItemDataDet> itmDetEnt = new EntitySet<tblItemDataDet>();
        EntitySet<tblStkGPDet> stkGpDetEnt = new EntitySet<tblStkGPDet>();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        public DataTable CurrentTable
        {
            set { ViewState["CurrentTable"] = value; }
            get { return (DataTable)(ViewState["CurrentTable"] ?? 0); }
        }
        public int rowsCount
        {
            set { ViewState["rowsCount"] = value; }
            get { return Convert.ToInt32(ViewState["rowsCount"] ?? 0); }
        }

        public int ltNo
        {
            set { ViewState["ltNo"] = value; }
            get { return Convert.ToInt32(ViewState["ltNo"] ?? 0); }
        }

        public int vrNo
        {
            set { ViewState["vrNo"] = value; }
            get { return Convert.ToInt32(ViewState["vrNo"] ?? 0); }
        }

        public int vrQty
        {
            set { ViewState["vrQty"] = value; }
            get { return Convert.ToInt32(ViewState["vrQty"] ?? 0); }
        }

        public int grdQty
        {
            set { ViewState["grdQty"] = value; }
            get { return Convert.ToInt32(ViewState["grdQty"] ?? 0); }
        }

        public int mtQty
        {
            set { ViewState["mtQty"] = value; }
            get { return Convert.ToInt32(ViewState["mtQty"] ?? 0); }
        }

        public string Item_Code
        {
            set { ViewState["Item_Code"] = value; }
            get { return (ViewState["Item_Code"].ToString()); }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                
                int GroupID = 0;
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
                GridView1.Columns[2].Visible = false;
                lblGPNo.Text = "Card No:";
                lblLotNo.Text = "Lot No:";
                lblRemarks.Text = "Remarks:";
                lblGpDate.Text = "Date:";
                lblStatus.Text = "Status:";
                lblProduct.Text = "Product:";
                lblToStore.Text = "Store Loc:";

                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                }
                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (Convert.ToBoolean(Session["IfEdit"]) == false)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GradingCard").ToString();
                    //BindTable();
                    BindDDLLoc();
                    BindProduct();
                    //BindDDLVendor();
                    //BindDDLCity();
                    GetGradingNo();
                    btnBack.Visible = false;
                    btnClear.Visible = true;
                    btnList.Visible = true;
                    ddlStatus.SelectedValue = "P";
                }
                else
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", Session["PgTile"].ToString()).ToString();
                    BindProduct();
                    BindDDLLoc();
                    //BindDDLVendor();
                    //BindDDLCity();
                    BindFields();
                    BindTableEdit();
                    txtLotNo.ReadOnly = true;
                    btnClear.Visible = false;
                    btnBack.Visible = true;
                    btnList.Visible = false;
                    ddlLoc.Enabled = false;
                    imgFind.Visible = false;
                }

            }
        }
        protected void imgFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLotNo.Text != "")
                {
                    try
                    {
                        ltNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));
                        mtQty = Convert.ToInt32(gC.GetMTQty(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]));
                        grdQty = Convert.ToInt32(gC.GetTrnsfrdQty(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]));
                        string itmCodePrev = gC.GetItemUzngLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        string itmCode = gC.GetControlItemCd(itmCodePrev, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        
                        //bool trnsfrStatus = gC.CheckIfAlreadyTrnsfrd(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (grdQty < mtQty)
                        {
                            if (Convert.ToInt32( gC.GetLotQty(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])) > 0)
                            {
                                ddlProduct.SelectedValue = itmCode;
                                txtMTQty.Text = mtQty.ToString();
                                txtGraded.Text = grdQty.ToString();
                                txtRemQty.Text = (mtQty - grdQty).ToString();
                                txtQty.Text = gC.GetLotQty(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

                                BindTable();
                                if (itmCode == "0")
                                {
                                    txtQty.Text = "";
                                    txtRemQty.Text = "";
                                    ddlProduct.SelectedValue = "0";
                                    ucMsgPnl.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Error);
                                }
                            }
                            else
                            {
                                txtQty.Text = "";
                                txtRemQty.Text = "";
                                ddlProduct.SelectedValue = "0";
                                ucMsgPnl.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Error);
                            }
                        }
                        else
                        {
                            txtQty.Text = "";
                            txtRemQty.Text = "";
                            ddlProduct.SelectedValue = "0";
                            ucMsgPnl.ShowMessage("Card already exists or lot not yet created.", RMS.BL.Enums.MessageType.Error);
                        }
                    }
                    catch
                    {
                        txtQty.Text = "";
                        txtRemQty.Text = "";
                        ddlProduct.SelectedValue = "0";
                        ucMsgPnl.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    txtQty.Text = "";
                    txtRemQty.Text = "";
                    ddlProduct.SelectedValue = "0";
                    ucMsgPnl.ShowMessage("Please enter lot no.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull(); throw ex;
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DropDownList ddOpp = (DropDownList)e.Row.Cells[1].FindControl("ddlProduct");
                //BindOpponentDD(ddOpp);
                //DropDownList ddSG = (DropDownList)e.Row.Cells[1].FindControl("ddlSizGrd");
                //BindOpponentDDSG(ddSG);
                
            }
        }
        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
            //SetDropDownList();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/gradingcardmgt.aspx?PID=431");
        }
        protected void btnList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/gradingcardhome.aspx?PID=424");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool flag = false;

            //=======================
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (((TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text != "")
                {
                    if (((TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text == "")
                    {
                        ((TextBox)GridView1.Rows[i].FindControl("txtWeight")).Focus();
                        flag = true;
                    }
                }
            }
            //=====================
            itmDetEnt.Clear();
           
            if (Convert.ToBoolean(Session["IfEdit"]) == false)
            {

                if (flag == false)
                {
                    Save();
                }
                else
                {
                    ucMessage.ShowMessage("Enter area against quantity.", RMS.BL.Enums.MessageType.Error);
                }

            }
            else
            {
                if (flag == false)
                {
                    Edit();
                }
                else
                {
                    ucMessage.ShowMessage("Enter area against quantity.", RMS.BL.Enums.MessageType.Error);
                }
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["PrePgAddress"].ToString());
        }

        #endregion
        
        #region Helping Method

        public void Edit()
        {
            try
            {
                int countDetRec = 0;

                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtGpDate.Text);

                string no = txtGPNo.Text.Substring(5);
                string yrno = txtGPNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                vrNo = Convert.ToInt32(yrno);
                itmData.vt_cd = 15;
                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);

                itmData.vr_nrtn = txtRemarks.Text;
                itmData.gl_cd = "";
                itmData.post_2gl = true;
                itmData.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                if (txtLotNo.Text != "")
                    itmData.LotNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));
                itmData.ToLocId = Convert.ToInt16(ddlLoc.SelectedValue);
                itmData.Freight = 0;
                itmData.Tax = 0;

                itmData.vr_apr = Convert.ToString(ddlStatus.SelectedValue);
                itmData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (ddlStatus.SelectedValue == "A" || ddlStatus.SelectedValue == "C")
                {
                    if (Session["UserName"] == null)
                        itmData.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itmData.updateby = Session["UserName"].ToString();
                }
                itmData.Status = "Gr";

                int srCount = 1;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text;
                    string ftg = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtWeight")).Text;
                    if (qty != "" && ftg != "")
                    {
                        countDetRec++;
                        tblItemDataDet itmDet = new tblItemDataDet();
                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = 15;
                        string nos = txtGPNo.Text.Substring(5);
                        string yrnos = txtGPNo.Text.Substring(0, 4) + no;
                        itmDet.vr_no = Convert.ToInt32(yrnos);
                        itmDet.vr_seq = Convert.ToByte(srCount++);
                        itmDet.itm_cd = ddlProduct.SelectedValue;
                        itmDet.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text != "")
                            itmDet.vr_qty = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text);
                        else
                            itmDet.vr_qty = 0;
                        //Area
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text != "")
                            itmDet.Feetage = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text);
                        else
                            itmDet.Feetage = 0;
                        itmDet.vr_val = 0;
                        itmDet.vr_rate = 0;
                        itmDet.KGSwt = 0;
                        itmDet.vr_rmk = "";


                        string gradIdSelctionId = ((System.Web.UI.WebControls.HiddenField)GridView1.Rows[i].Cells[1].FindControl("hidSize")).Value;


                        string gradeId = "";
                        string selectionId = "";
                        int count = 0;
                        string[] parts = Regex.Split(gradIdSelctionId, ":");
                        foreach (string part in parts)
                        {
                            if (count == 0)
                            {
                                gradeId = part;
                            }
                            else
                            {
                                selectionId = part;
                            }
                            count++;
                        }

                        itmDet.GradeId = gradeId;
                        itmDet.SelectionId = selectionId;


                        itmDet.LotRef = 0;
                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        itmDet.DrumNo = 0;
                        itmDetEnt.Add(itmDet);
                    }
                }

                int sumQty = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text;
                    if (qty != "")
                    {
                        sumQty = sumQty + Convert.ToInt32(qty);
                    }
                }

                if (sumQty + grdQty <= mtQty)
                {


                    int sgNo = Convert.ToInt32(Session["SGno"]);
                    int yr = Convert.ToInt32(Session["DateYear"]);
                    string vr_no = Session["vrNO"].ToString();
                    bool res = gC.EditRec(sgNo, Convert.ToInt32(vr_no.Substring(0, 4)), 15, itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == true)
                    {
                        if (ddlStatus.SelectedValue == "A")
                        {
                            //here-> uncomment(3 locations) for wet blue transfer
                            string glYear = gC.GetGlYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            List<tblItemDataDet> lstDet = gC.GetItemDetUsingVrNo(vrNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (lstDet.Count > 0)
                            {
                                foreach (tblItemDataDet tbl in lstDet)
                                {
                                    tblItem Item = gC.GetItemUsingSelection(tbl.itm_cd, tbl.SelectionId, tbl.LocId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                                    if (Item != null)
                                    {
                                        gC.GetUpdTblItemGrading(tbl.LocId, tbl.itm_cd, tbl.SelectionId, Convert.ToInt32(tbl.vr_qty), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    }
                                    else
                                    {
                                        gC.GetInsrtTblitem(tbl.br_id, tbl.LocId, tbl.itm_cd, tbl.SelectionId, Convert.ToInt32(tbl.vr_qty), "00000", Convert.ToDecimal(glYear), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    }
                                }
                            }
                        }
                        ucMessage.ShowMessage("Updated successfully", RMS.BL.Enums.MessageType.Info);
                        itmDetEnt.Clear();
                        Response.Redirect("~/invsetup/gradingcardhome.aspx?PID=424");
                    }
                    else
                    {
                        ucMessage.ShowMessage("Update was  unsuccessful", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Total quantity should match the quantity in parts.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void Save()
        {
            try
            {
                int countDetRec = 0;

                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtGpDate.Text);

                string no = txtGPNo.Text.Substring(5);
                string yrno = txtGPNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                vrNo = Convert.ToInt32(yrno);
                itmData.vt_cd = 15;
                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);

                itmData.vr_nrtn = txtRemarks.Text;
                itmData.gl_cd = "";
                itmData.post_2gl = true;
                itmData.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                if (txtLotNo.Text != "")
                    itmData.LotNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));
                itmData.ToLocId = Convert.ToInt16(ddlLoc.SelectedValue);
                itmData.Freight = 0;
                itmData.Tax = 0;

                itmData.vr_apr = Convert.ToString(ddlStatus.SelectedValue);
                itmData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (ddlStatus.SelectedValue == "A" || ddlStatus.SelectedValue == "C")
                {
                    if (Session["UserName"] == null)
                        itmData.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itmData.updateby = Session["UserName"].ToString();
                }
                itmData.Status = "Gr";

                int srCount = 1;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text;
                    string ftg = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtWeight")).Text;
                    if (qty != "" && ftg != "")
                    {
                        countDetRec++;
                        tblItemDataDet itmDet = new tblItemDataDet();
                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = 15;
                        string nos = txtGPNo.Text.Substring(5);
                        string yrnos = txtGPNo.Text.Substring(0, 4) + no;
                        itmDet.vr_no = Convert.ToInt32(yrnos);
                        itmDet.vr_seq = Convert.ToByte(srCount++);
                        itmDet.itm_cd = ddlProduct.SelectedValue;
                        itmDet.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text != "")
                            itmDet.vr_qty = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text);
                        else
                            itmDet.vr_qty = 0;
                        //area
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text != "")
                            itmDet.Feetage = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text);
                        else
                            itmDet.Feetage = 0;
                        itmDet.vr_val = 0;
                        itmDet.vr_rate = 0;
                        itmDet.KGSwt = 0;
                        itmDet.vr_rmk = "";

                        string gradIdSelctionId = ((System.Web.UI.WebControls.HiddenField)GridView1.Rows[i].Cells[1].FindControl("hidSize")).Value;


                        string gradeId = "";
                        string selectionId = "";
                        int count = 0;
                        string[] parts = Regex.Split(gradIdSelctionId, ":");
                        foreach (string part in parts)
                        {
                            if (count == 0)
                            {
                                gradeId = part;
                            }
                            else
                            {
                                selectionId = part;
                            }
                            count++;
                        }

                        itmDet.GradeId = gradeId;
                        itmDet.SelectionId = selectionId;


                        itmDet.LotRef = 0;
                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        itmDet.DrumNo = 0;
                        itmDetEnt.Add(itmDet);
                    }
                }
                bool res = false;
                int sumQty = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text;
                    if (qty != "")
                    {
                        sumQty = sumQty + Convert.ToInt32(qty);
                    }
                }

                if (sumQty + grdQty <= mtQty)
                {
                    if (countDetRec > 0)
                    {
                        res = gC.SaveItemData(itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                    else
                    {
                        res = false;
                    }


                    if (res == true)
                    {
                        if (ddlStatus.SelectedValue == "A")
                        {
                            //here-> uncomment(3 locations) for wet blue transfer
                            string glYear = gC.GetGlYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            List<tblItemDataDet> lstDet = gC.GetItemDetUsingVrNo(vrNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (lstDet.Count > 0)
                            {
                                foreach (tblItemDataDet tbl in lstDet)
                                {
                                    tblItem Item = gC.GetItemUsingSelection(tbl.itm_cd, tbl.SelectionId, tbl.LocId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                                    if (Item != null)
                                    {
                                        gC.GetUpdTblItemGrading(tbl.LocId, tbl.itm_cd, tbl.SelectionId, Convert.ToInt32(tbl.vr_qty), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    }
                                    else
                                    {
                                        gC.GetInsrtTblitem(tbl.br_id, tbl.LocId, tbl.itm_cd, tbl.SelectionId, Convert.ToInt32(tbl.vr_qty), "00000", Convert.ToDecimal(glYear), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    }
                                }
                            }
                        }
                        ucMessage.ShowMessage("Saved successfully", RMS.BL.Enums.MessageType.Info);
                        itmDetEnt.Clear();
                        Response.Redirect("~/invsetup/gradingcardhome.aspx?PID=424");
                    }
                    else
                    {
                        ucMessage.ShowMessage("Save was unsuccessful", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Total quantity should match the quantity in parts.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindProduct()
        {
            ddlProduct.DataSource = sG.GetProduct((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlProduct.DataTextField = "itm_dsc";
            ddlProduct.DataValueField = "itm_cd";
            ddlProduct.DataBind();

        }

        public void BindOpponentDDSG(DropDownList ddlSG)
        {

            string itmCode = "0";
            if (Convert.ToBoolean(Session["IfEdit"]) == false)
            {
                itmCode = ddlProduct.SelectedValue;
            }
            else
            {
                int sgNo = Convert.ToInt32(Session["SGno"]);
                int yr = Convert.ToInt32(Session["DateYear"]);
                string vrNo = Session["vrNO"].ToString();
                itmCode = sG.GetItemCode(sgNo, Convert.ToInt32(vrNo.Substring(0, 4)), 15, 2, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }

            
            ddlSG.DataSource = gC.GetSizeGradeCode(itmCode, 2,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSG.DataTextField = "SizeGrade_Desc";
            ddlSG.DataValueField = "GradeId";
            ddlSG.DataBind();

        }

        public void BindFields()
        {
            int sgNo = Convert.ToInt32(Session["SGno"]);
            int yr = Convert.ToInt32(Session["DateYear"]);
            string vrNo = Session["vrNO"].ToString();
            tblItemData rec = gC.GetRec(sgNo, Convert.ToInt32(vrNo.Substring(0, 4)), 15, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlLoc.SelectedValue = rec.LocId.ToString();
            txtGPNo.Text = rec.vr_no.ToString().Substring(0, 4) + "/" + rec.vr_no.ToString().Substring(4);

            CalendarExtender1.SelectedDate = rec.vr_dt;


            if (rec.LotNo > 0)
                txtLotNo.Text = rec.LotNo.ToString().Substring(0, 4) + "-" + rec.LotNo.ToString().Substring(4);
                        ddlStatus.SelectedValue = rec.vr_apr.ToString();
                        txtRemarks.Text = rec.vr_nrtn;
                        txtQty.Text = gC.GetLotQty(Convert.ToInt32(rec.LotNo), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
                        mtQty = Convert.ToInt32(gC.GetMTQty(Convert.ToInt32(rec.LotNo), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]));
                        grdQty = Convert.ToInt32(gC.GetTrnsfrdQty(Convert.ToInt32(rec.LotNo), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]));
                        
                        vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);
                        vrQty = gC.GetVrQty(Convert.ToInt32(vrNo),(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        txtMTQty.Text = mtQty.ToString();
                        txtGraded.Text = (grdQty -vrQty).ToString();
                        grdQty = Convert.ToInt32(txtGraded.Text);
                        txtRemQty.Text = (mtQty - grdQty).ToString();
        }

        public void GetGradingNo()
        {
            txtGPNo.Text = gC.GetSGNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 15, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        
        public void BindDDLLoc()
        {
            ddlLoc.DataSource = gC.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
            ddlLoc.SelectedValue = "2";

        }

        public void BindDDLVendor()
        {
            //ddlVendor.DataSource = gP.GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlVendor.DataTextField = "gl_dsc";
            //ddlVendor.DataValueField = "gl_cd";
            //ddlVendor.DataBind();
        }

        public void BindDDLCity()
        {
            //ddlCity.DataSource = gP.GetCity((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlCity.DataTextField = "CityName";
            //ddlCity.DataValueField = "CityID";
            //ddlCity.DataBind();
        }

        public void AddRow()
        {
            if (CurrentTable != null)
            {
                d_table = CurrentTable;
                d_Row = d_table.NewRow();
                d_Row["Sr"] = d_table.Rows.Count + 1;
                d_table.Rows.Add(d_Row);
                CurrentTable = d_table;
                BindGrid();
            }
        }

        public void UpdateTable()
        {
            if (CurrentTable != null)
            {
                GetColumns();
                rowsCount = CurrentTable.Rows.Count;
                for (int i = 0; i < rowsCount; i++)
                {
                    d_Row = d_table.NewRow();
                    string srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].Cells[0].FindControl("lblSr")).Text;
                   // string Prdct = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlProduct")).SelectedValue;
                    string szCode = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("SizeCode")).Text;
                    string Code = ((System.Web.UI.WebControls.HiddenField)GridView1.Rows[i].Cells[1].FindControl("hidSize")).Value;
                   
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text;
                    string vat = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtWeight")).Text;
                    
                    d_Row["Sr"] = srNo;
                    d_Row["CodeDesc"] = szCode;
                    d_Row["Code"] = Code;
                    

                    if (qty != "")
                        d_Row["Quantity"] = Convert.ToInt32(qty);
                    if (vat != "")
                        d_Row["Weight"] = Convert.ToDecimal(vat);
                    
                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;//assigning to viewstate datatable
                BindGrid();
                //SetDropDownList();

            }
        }

        //public void SetDropDownList()
        //{
        //    rowsCount = CurrentTable.Rows.Count;
        //    for (int i = 0; i < rowsCount; i++)
        //    {
        //        //DropDownList ddl1 = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlProduct"));
        //        DropDownList ddl2 = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlSizGrd"));
        //        if (i < CurrentTable.Rows.Count)
        //        {

        //            //if (CurrentTable.Rows[i]["Product"] != DBNull.Value )
        //            //{
        //            //    ddl1.ClearSelection();
                        
        //            //    ddl1.Items.FindByValue(CurrentTable.Rows[i]["Product"].ToString()).Selected = true;
        //            //}
        //            if (CurrentTable.Rows[i]["SizingGrading"] != DBNull.Value)
        //            {
        //                ddl2.ClearSelection();
        //                ddl2.Items.FindByValue(CurrentTable.Rows[i]["SizingGrading"].ToString()).Selected = true;

        //            }
        //        }
        //    }
        //}

        public void GetColumns()
        {
            d_table.Columns.Clear();
            d_table.Rows.Clear();

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Sr";
            d_table.Columns.Add(d_Col);
            //d_Col = new DataColumn();
            //d_Col.DataType = System.Type.GetType("System.String");
            //d_Col.ColumnName = "Product";
            //d_table.Columns.Add(d_Col);
            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "CodeDesc";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Code";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Int32");
            d_Col.ColumnName = "Quantity";
            d_table.Columns.Add(d_Col);
            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Decimal");
            d_Col.ColumnName = "Weight";
            d_table.Columns.Add(d_Col);
       

        }

        public void BindTable()
        {
            GetColumns();
            Item_Code = ddlProduct.SelectedValue;
            List<string> lst = gC.GetSizeGradeCode1(Item_Code, 2, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            string cdDesc = "";
            string code = "";
            int count = 0;
            for (int i = 0; i < lst.Count; i++)
            {
                string[] parts = Regex.Split(lst[i], ",");
                foreach (string part in parts)
                {
                    if (count == 0)
                    {
                        cdDesc = part;
                    }
                    else
                    {
                        code = part;
                    }
                    count++;
                }
                count = 0;


                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;

                d_Row["CodeDesc"] = cdDesc;
                d_Row["Code"] = code;


                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }

        public void BindTableEdit()
        {
            GetColumns();
            int count = 0;
            string selection = "";
            string gradeSelection = "";
            int count1 = 0;
            int count2 = 0;

            int sgNo = Convert.ToInt32(Session["SGno"]);
            int yr =Convert.ToInt32(Session["DateYear"]);
            string vrNo = Session["vrNO"].ToString();
            Item_Code = sG.GetItemCode(sgNo, Convert.ToInt32(vrNo.Substring(0, 4)), 15, 2, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlProduct.SelectedValue = Item_Code;
            List<string> lst = gC.GetSizeGradeCode1(Item_Code, 2, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblItemDataDet> list = gC.GetRecDet(sgNo, Convert.ToInt32(vrNo.Substring(0,4)), 15, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            for (int i = 0; i < lst.Count; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;

                string[] parts = Regex.Split(lst[i], ",");
                foreach (string part in parts)
                {
                    if (count1 == 0)
                    {
                        selection = part;
                    }
                    else
                    {
                        gradeSelection = part;
                    }
                    count1++;
                }
                count1 = 0;

                d_Row["CodeDesc"] = selection;
                d_Row["Code"] = gradeSelection;



                string gradeId = "";
                string selectionId = "";
                string[] partss = Regex.Split(gradeSelection, ":");
                foreach (string part in partss)
                {
                    if (count2 == 0)
                    {
                        gradeId = part;
                    }
                    else
                    {
                        selectionId = part;
                    }
                    count2++;
                }

                count2 = 0;
                for (int j = 0; j < list.Count(); j++)
                {
                    if (list[j].GradeId == gradeId && list[j].SelectionId == selectionId)
                    {
                        d_Row["Quantity"] = list[j].vr_qty;
                        d_Row["Weight"] = list[j].Feetage;

                    }
                }
                count++;
                d_table.Rows.Add(d_Row);
            }

            CurrentTable = d_table;
            BindGrid();
            //SetDropDownList();
        }
        
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }

        #endregion
    }
}
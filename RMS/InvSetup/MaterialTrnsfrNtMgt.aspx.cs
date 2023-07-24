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

namespace RMS
{
    public partial class MaterialTrnsfrNtMgt : BasePage
    {
        #region DataMembers

        //DashBoardBL dashBoardBL = new DashBoardBL();
        InvGP_BL gP = new InvGP_BL();
        InvSG_BL sG = new InvSG_BL();
        InvMN_BL mN = new InvMN_BL();
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

        public string docRef
        {
            set { ViewState["docRef"] = value; }
            get { return Convert.ToString(ViewState["docRef"] ?? ""); }
        }

        public string previousQty
        {
            set { ViewState["previousQty"] = value; }
            get { return Convert.ToString(ViewState["previousQty"] ?? ""); }
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
                lblFromStore.Text = "From Loc:";
                lblRemarks.Text = "Remarks:";
                lblGpDate.Text = "Date:";
                lblStatus.Text = "Status:";
                lblProduct.Text = "Product:";
                lblToStore.Text = "To Loc:";

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
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "MatTrnsfrNote").ToString();
                    BindTable();
                    BindDDLLoc();
                    BindDDLToLoc();
                    BindProduct();
                    //BindDDLVendor();
                    //BindDDLCity();
                    GetMatNoteNo();
                    ddlStatus.SelectedValue = "P";
                    btnBack.Visible = false;
                    btnClear.Visible = true;
                    btnList.Visible = true;
                }
                else
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", Session["PgTile"].ToString()).ToString();

                   
                    BindDDLLoc();
                    BindDDLToLoc();
                    BindProduct();
                    //BindDDLVendor();
                    //BindDDLCity();
                    BindFields();
                    BindTableEdit();
                    txtLotNo.ReadOnly = true;
                    btnClear.Visible = false;
                    btnBack.Visible = true;
                    btnList.Visible = false;
                    imgFind.Visible = false;
                   
                }

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DropDownList ddOpp = (DropDownList)e.Row.Cells[1].FindControl("ddlProducts");
                //BindOpponentDD(ddOpp);
               // DropDownList ddSG = (DropDownList)e.Row.Cells[1].FindControl("ddlSizGrd");
               // BindOpponentDDSG(ddSG);
                
            }
        }

        protected void imgFind_Click(object sender, EventArgs e)
        {
            grdSGC.Visible = false;
            string lotQty = "";
                if (txtLotNo.Text != "")
                {
                    try
                    {
                        string status = "";
                        ltNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));
                        string itmCode = mN.GetItemUzngLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        status = itmCode.Substring(0, 1);
                        itmCode = itmCode.Substring(2);

                        previousQty = mN.GetPrevQty(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //Change2
                        lotQty =Convert.ToInt32(mN.GetLotQty(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])).ToString();
                        bool trnsfrStatus = mN.CheckIfAlreadyTrnsfrd(ltNo, lotQty,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (trnsfrStatus == false)
                        {
                            if (status == "A")
                            {
                                //-----------------
                                //Change1
                                if (mN.CheckIfPendingOrNotExist(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) == false)
                                {
                                    ddlProduct.SelectedValue = "0";
                                    txtQty.Text = "";
                                    txtPrevQty.Text = "";
                                    ucMsgPnl.ShowMessage("Lot is either pending or not sized yet.", RMS.BL.Enums.MessageType.Error);

                                }
                                //-----------------
                                else{

                                if (itmCode != "0")
                                {
                                    ddlProduct.SelectedValue = itmCode;
                                    txtPrevQty.Text = previousQty;
                                    txtQty.Text = Convert.ToInt32(mN.GetLotQty(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])).ToString();
                                    txtRemQty.Text = (Convert.ToInt32(txtQty.Text) - Convert.ToInt32(previousQty)).ToString();
                                    docRef = mN.GetDocRefByLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    //grdSGC.Visible = true;
                                    //BindSGcGrid();

                                }
                                else
                                {
                                    ddlProduct.SelectedValue = "0";
                                    txtQty.Text = "";
                                    txtPrevQty.Text = "";
                                    ucMsgPnl.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Error);
                                }

                            }//end of else
                            }
                            else
                            {
                                if (status != "0")
                                {
                                    ddlProduct.SelectedValue = "0";
                                    txtQty.Text = "";
                                    txtPrevQty.Text = "";
                                    ucMsgPnl.ShowMessage("Cannot transfer material with lot status 'Panding/Cancelled'.", RMS.BL.Enums.MessageType.Error);
                                }
                                else
                                {
                                    ddlProduct.SelectedValue = "0";
                                    txtQty.Text = "";
                                    txtPrevQty.Text = "";
                                    ucMsgPnl.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Error);
                                }
                            }
                        }
                        else
                        {
                            ucMsgPnl.ShowMessage("Already transferred.", RMS.BL.Enums.MessageType.Error);
                        }
                    }
                    catch
                    {
                        ucMsgPnl.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMsgPnl.ShowMessage("Please enter lot no.", RMS.BL.Enums.MessageType.Error);
                }

        }

        protected void grdSGC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSGC.PageIndex = e.NewPageIndex;
            BindSGcGrid();
        }

        protected void grdSGC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
            //SetDropDownList();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/materialtrnsfrntmgt.aspx?PID=429");
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/materialtrnsfrnthome.aspx?PID=423");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            itmDetEnt.Clear();
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

        public void Edit()
        {
            try
            {
                int countDetRec = 0;
                ltNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));

                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtGpDate.Text);

                string no = txtGPNo.Text.Substring(5);
                string yrno = txtGPNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                itmData.vt_cd = 23;
                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);

                itmData.vr_nrtn = txtRemarks.Text;
                itmData.gl_cd = "";
                itmData.post_2gl = true;
                itmData.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                itmData.LotNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));
                itmData.ToLocId = Convert.ToInt16(ddlToLoc.SelectedValue);
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
                itmData.Status = "Tr";
                docRef = mN.GetDocRefByLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                itmData.DocRef = docRef;

                int srCount = 1;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text;

                    if (qty != "")
                    {
                        countDetRec++;
                        tblItemDataDet itmDet = new tblItemDataDet();
                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = 23;
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
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text != "")
                            itmDet.KGSwt = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text);
                        else
                            itmDet.KGSwt = 0;
                        itmDet.vr_val = 0;
                        itmDet.vr_rate = 0;
                        itmDet.Feetage = 0;
                        itmDet.vr_rmk = "";
                        itmDet.GradeId = "";
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtLotRef")).Text != "")
                            itmDet.LotRef = Convert.ToInt32(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtLotRef")).Text);
                        else
                            itmDet.LotRef = 0;

                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDrumNo")).Text != "")
                            itmDet.DrumNo = Convert.ToByte(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDrumNo")).Text);
                        else
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
                if (sumQty + Convert.ToInt32(txtPrevQty.Text) <= Convert.ToInt32(txtQty.Text))
                {

                    int sgNo = Convert.ToInt32(Session["SGno"]);
                    int yr = Convert.ToInt32(Session["DateYear"]);
                    string vrNo = Session["vrNo"].ToString();
                    bool res = mN.EditRec(sgNo, Convert.ToInt32(vrNo.Substring(0, 4)), 23, itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == true)
                    {
                        ucMessage.ShowMessage("Updated successfully", RMS.BL.Enums.MessageType.Info);
                        itmDetEnt.Clear();
                        Response.Redirect("~/invsetup/materialtrnsfrnthome.aspx?PID=423");
                    }
                    else
                    {
                        ucMessage.ShowMessage("Update was  unsuccessful", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Cannot edit, total quantity should exactly match the quantity in parts.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
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
                itmData.vt_cd = 23;
                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);

                itmData.vr_nrtn = txtRemarks.Text;
                itmData.gl_cd = "";
                itmData.post_2gl = true;
                itmData.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                itmData.LotNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));
                itmData.ToLocId = Convert.ToInt16(ddlToLoc.SelectedValue);
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
                itmData.Status = "Tr";
                itmData.DocRef = docRef;


                int srCount = 1;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text;

                    if (qty != "")
                    {
                        countDetRec++;
                        tblItemDataDet itmDet = new tblItemDataDet();
                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = 23;
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
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text != "")
                            itmDet.KGSwt = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text);
                        else
                            itmDet.KGSwt = 0;
                        itmDet.vr_val = 0;
                        itmDet.vr_rate = 0;
                        itmDet.Feetage = 0;
                        itmDet.vr_rmk = "";
                        itmDet.GradeId = "";
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtLotRef")).Text != "")
                            itmDet.LotRef = Convert.ToInt32(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtLotRef")).Text);
                        else
                            itmDet.LotRef = 0;

                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDrumNo")).Text != "")
                            itmDet.DrumNo = Convert.ToByte(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDrumNo")).Text);
                        else
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

                sumQty = sumQty + Convert.ToInt32(previousQty);

                if (sumQty <= Convert.ToInt32(txtQty.Text))
                {
                    if (countDetRec > 0)
                    {
                        res = mN.SaveItemData(itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                    else
                    {
                        res = false;
                    }


                    if (res == true)
                    {
                        Response.Redirect("~/invsetup/materialtrnsfrnthome.aspx?PID=423");
                        itmDetEnt.Clear();
                        ucMessage.ShowMessage("Saved successfully", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        ucMessage.ShowMessage("Save was unsuccessful", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Cannot save, total quantity should exactly match the quantity in parts.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindSGcGrid()
        {
            object lst = mN.GetLotRecByLotNo(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdSGC.DataSource = lst;
            grdSGC.DataBind();
                grdSGC.Visible = true;
            
        }

        public void BindOpponentDD(DropDownList ddlPrdct)
        {
            ddlPrdct.DataSource = mN.GetProduct((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlPrdct.DataTextField = "itm_dsc";
            ddlPrdct.DataValueField = "itm_cd";
            ddlPrdct.DataBind();
           
        }

        public void BindOpponentDDSG(DropDownList ddlSG)
        {
            ddlSG.DataSource = mN.GetSizeGradeCode((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSG.DataTextField = "SizeGrade_Desc";
            ddlSG.DataValueField = "GradeId";
            ddlSG.DataBind();

        }

        public void BindProduct()
        {
            ddlProduct.DataSource = mN.GetProduct((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlProduct.DataTextField = "itm_dsc";
            ddlProduct.DataValueField = "itm_cd";
            ddlProduct.DataBind();

        }

        public void BindFields()
        {
            int sgNo = Convert.ToInt32(Session["SGno"]);
            int yr = Convert.ToInt32(Session["DateYear"]);
            string vrNo = Session["vrNo"].ToString();



            tblItemData rec = mN.GetRec(sgNo, Convert.ToInt32(vrNo.Substring(0,4)), 23, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlLoc.SelectedValue = rec.LocId.ToString();
            ddlToLoc.SelectedValue = rec.ToLocId.ToString();
            txtGPNo.Text = rec.vr_no.ToString().Substring(0, 4) + "/" + rec.vr_no.ToString().Substring(4);
            txtLotNo.Text = rec.LotNo.ToString().Substring(0, 4) + "-"+rec.LotNo.ToString().Substring(4);
            CalendarExtender1.SelectedDate = rec.vr_dt;

                        ddlStatus.SelectedValue = rec.vr_apr.ToString();
                        txtRemarks.Text = rec.vr_nrtn;
                        txtQty.Text =Convert.ToInt32( mN.GetLotQty(Convert.ToInt32(rec.LotNo), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])).ToString();
                        previousQty = mN.GetPrevQty(Convert.ToInt32(rec.LotNo), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);
                        string vrQty = mN.GetVrQty(Convert.ToInt32( vrNo), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        txtPrevQty.Text = (Convert.ToInt32(previousQty) - Convert.ToInt32(vrQty)).ToString();
                        txtRemQty.Text = (Convert.ToInt32(txtQty.Text) - Convert.ToInt32(txtPrevQty.Text)).ToString();

        }

        public void GetMatNoteNo()
        {
            txtGPNo.Text = mN.GetSGNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 23, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        
        public void BindDDLLoc()
        {
            ddlLoc.DataSource = mN.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
            ddlLoc.SelectedValue = "1";

        }

        public void BindDDLToLoc()
        {
            ddlToLoc.DataSource = mN.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlToLoc.DataValueField = "LocId";
            ddlToLoc.DataTextField = "LocName";
            ddlToLoc.DataBind();
            ddlToLoc.SelectedValue = "2";

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
                   string LtRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtLotRef")).Text;
                   // string prdct = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[2].FindControl("ddlProducts")).SelectedValue;
                    
                   // string LtRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtLotRef")).Text;
                    string drmNo = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtDrumNo")).Text;
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtQuantity")).Text;
                    string vat = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[4].FindControl("txtWeight")).Text;
                   
                    d_Row["Sr"] = srNo;

                 //   if (LtRef != "")
                   //     d_Row["LotNo"] = Convert.ToInt32(LtRef);
                    

                    d_Row["DrumNo"]=drmNo;
                    d_Row["LotRef"] = LtRef;
                    //d_Row["Product"] = prdct;
                           
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
        //        DropDownList ddl1 = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlProducts"));
        //        //DropDownList ddl2 = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlSizGrd"));
        //        if (i < CurrentTable.Rows.Count)
        //        {

        //            if (CurrentTable.Rows[i]["Product"] != DBNull.Value )
        //            {
        //                ddl1.ClearSelection();
                        
        //                ddl1.Items.FindByValue(CurrentTable.Rows[i]["Product"].ToString()).Selected = true;
        //            }
        //            //if (CurrentTable.Rows[i]["SizingGrading"] != DBNull.Value)
        //            //{
        //            //    ddl2.ClearSelection();
        //            //    ddl2.Items.FindByValue(CurrentTable.Rows[i]["SizingGrading"].ToString()).Selected = true;

        //            //}
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
            //d_Col.DataType = System.Type.GetType("System.Int32");
            //d_Col.ColumnName = "LotNo";
            //d_table.Columns.Add(d_Col);

            //d_Col = new DataColumn();
            //d_Col.DataType = System.Type.GetType("System.String");
            //d_Col.ColumnName = "Product";
            //d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "LotRef";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "DrumNo";
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
            for (int i = 0; i < 20; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;

                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }

        public void BindTableEdit()
        {
            GetColumns();
            int count = 0;
            int sgNo = Convert.ToInt32(Session["SGno"]);
            int yr =Convert.ToInt32(Session["DateYear"]);
            string vrnNo = Session["vrNo"].ToString();
            List<tblItemDataDet> list = mN.GetRecDet(sgNo, Convert.ToInt32(vrnNo.Substring(0,4)), 23, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            foreach (var l in list)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                //d_Row["LotNo"] = 0;
                //d_Row["Product"] = l.itm_cd;
                d_Row["LotRef"] = l.LotRef;
                d_Row["DrumNo"] = l.DrumNo;
                d_Row["Quantity"] = l.vr_qty;
                d_Row["Weight"] = l.KGSwt;
                
                count++;
                d_table.Rows.Add(d_Row);
                ddlProduct.SelectedValue = l.itm_cd;
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
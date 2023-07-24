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
    public partial class SizingGradingCardMgt1 : BasePage
    {
        #region DataMembers

        //DashBoardBL dashBoardBL = new DashBoardBL();
        InvGP_BL gP = new InvGP_BL();
        InvSG_BL sG = new InvSG_BL();
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
        public string Item_Code
        {
            set { ViewState["Item_Code"] = value; }
            get { return Convert.ToString(ViewState["Item_Code"]); }
        }

        public string srchVal
        {
            set { ViewState["srchVal"] = value; }
            get { return Convert.ToString(ViewState["srchVal"]); }
        }

        public int igpNo
        {
            set { ViewState["igpNo"] = value; }
            get { return Convert.ToInt32(ViewState["igpNo"] ?? 0); }
        }
#pragma warning disable CS0414 // The field 'SizingGradingCardMgt1.flagClear' is assigned but its value is never used
        bool flagClear = false;
#pragma warning restore CS0414 // The field 'SizingGradingCardMgt1.flagClear' is assigned but its value is never used
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
                lblToStore.Text = "Store:";

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
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SizingGradingCard").ToString();
                    //BindTable();
                    BindDDLLoc();
                    //BindDDLVendor();
                    //BindDDLCity();
                    BindProduct();
                    ddlStatus.SelectedValue = "P";
                    GetSizeGradingNo();
                    btnBack.Visible = false;
                    btnClear.Visible = true;
                    btnList.Visible = true;
                    txtSrcIGP.Attributes.Add("onclick", "javascript:this.select();");
                }
                else
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", Session["PgTile"].ToString()).ToString();
                    
                    BindDDLLoc();
                    //BindDDLVendor();
                    //BindDDLCity();
                    BindProduct();
                    BindFields();
                    BindTableEdit();
                    ddlLoc.Enabled = false;
                    //ddlProduct.SelectedValue = Item_Code;
                    txtLotNo.ReadOnly = true;
                    btnClear.Visible = false;
                    btnBack.Visible = true;
                    btnList.Visible = false;
                    imgIGP.Visible = false;
                    srchPanel.Enabled = false;
                    txtSrcIGP.Enabled = false;
                }

            }
        }

        protected void srchIGP_Click(object sender, EventArgs e)
        {
            srchVal = aceValue.Value;
            
            if (txtSrcIGP.Text != "")
            {

                
                bool exist = false;
                try
                {
                    igpNo = Convert.ToInt32(srchVal.Substring(0, 4)+srchVal.Substring(5));
                    exist = sG.getIfCardAlreadyExist(igpNo, 14, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (exist == false)
                    {
                        BindGridIGP();
                        
                    }
                    else
                    {
                        BindGridIGP();
                        ClearFields();
                        ucMessage.ShowMessage("Feetage card already exists against this IGP.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                catch
                {
                    ucMessage.ShowMessage("No IGP found.", RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                ucMessage.ShowMessage("Please enter IGP No.", RMS.BL.Enums.MessageType.Error);
            }
          
        }

        protected void grdIGP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[3].Text)).ToString();

                string ig = e.Row.Cells[0].Text;

                igpNo = Convert.ToInt32(ig.Substring(0,4)+ig.Substring(5));
                bool exist = sG.getIfCardAlreadyExist(igpNo, 14, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (exist == true)
                {
                    e.Row.Cells[5].Text = "Yes";
                }
                else
                {
                    e.Row.Cells[5].Text = "No";
                }
            }
        }

        protected void grdIGP_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            GridViewRow clickedRow = grdIGP.Rows[e.NewSelectedIndex];
            string ig = clickedRow.Cells[0].Text;
            txtSrcIGP.Text = ig;
        }

        protected void grdIGP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdIGP.PageIndex = e.NewPageIndex;
            BindGridIGP();
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
        

        protected void lnkFind_Click(object sender, EventArgs e)
        {
        //    try
        //    {
        //        if (txtLotNo.Text != "")
        //        {
        //            int ltNo = Convert.ToInt32(txtLotNo.Text);
        //            string itmCode = sG.GetItemUzngLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            ddlProduct.SelectedValue = itmCode;
                   // BindTable();
        //            if (itmCode == "0")
        //            {
        //                ucMsgPnl.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Error);
        //            }
        //        }
        //        else
        //        {
        //            ucMsgPnl.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull(); throw ex;
        //    }
        }
        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
           
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/sizinggradingcardmgt.aspx?PID=464");
        }
        protected void btnList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/sizinggradinghome.aspx?PID=422");
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

        public void BindGridIGP()
        {
            if (txtSrcIGP.Text != "")
            {
                try
                {
                    
                    grdIGP.DataSource = sG.getGridList(igpNo, 11, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    grdIGP.DataBind();

                    if (grdIGP.Rows.Count > 0)
                    {
                        srchPanel.Visible = true;
                        string status = grdIGP.Rows[0].Cells[4].Text;
                        if (status == "Approved")
                        {
                            GridView1.Visible = true;
                            txtIgpDisp.Text = txtSrcIGP.Text;
                            string itmCode = sG.GetProducByIGP(igpNo, 11, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            ddlProduct.SelectedValue = itmCode;
                            
                            Item_Code = itmCode;
                            //string lotNo = sG.GetLotByIGP(igpNo, 11, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                            //txtLotNo.Text = lotNo;
                            BindTable();
                        }
                        else
                        {
                            ClearFields();
                            ucMessage.ShowMessage("Cannot create feetage card with status 'Pending/Cancelled'.", RMS.BL.Enums.MessageType.Error);
                        }
                    }
                    else
                    {
                        ClearFields();
                        srchPanel.Visible = false;
                        ucMessage.ShowMessage("No IGP found.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                catch
                {
                    ClearFields();
                    srchPanel.Visible = false;
                    ucMessage.ShowMessage("No IGP found.", RMS.BL.Enums.MessageType.Error);
                }
                
            }
        }

        public void ClearFields()
        {
            txtLotNo.Text = "";
            txtIgpDisp.Text = "";
            GridView1.Visible = false;
            ddlProduct.SelectedValue = "0";
            flagClear = true;
            BindTable();
           
        }
        public void BindProduct()
        {
            ddlProduct.DataTextField = "itm_dsc";
            ddlProduct.DataValueField = "itm_cd"; 
            ddlProduct.DataSource = sG.GetProduct((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlProduct.DataBind();
          
        }
        //public void BindOpponentDDSG(DropDownList ddlSG)
        //{
        //    string itmCode = "0";
        //    if (flagClear == false)
        //    {
        //        if (Convert.ToBoolean(Session["IfEdit"]) == false)
        //        {
        //            itmCode = ddlProduct.SelectedValue;
        //        }
        //        else
        //        {
        //            int sgNo = Convert.ToInt32(Session["SGno"]);
        //            int yr = Convert.ToInt32(Session["DateYear"]);
        //            string vrNo = Session["vrNo"].ToString();
        //            itmCode = sG.GetItemCode(sgNo,Convert.ToInt32(vrNo.Substring(0,4)), 14, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        }

        //        ddlSG.DataSource = sG.GetSizeGradeCode(itmCode, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        ddlSG.DataTextField = "SizeGrade_Desc";
        //        ddlSG.DataValueField = "GradeId";
        //        ddlSG.DataBind();
        //    }
        //    else
        //    {
        //        flagClear = false;
        //        ddlSG.DataSource = null;
        //        ddlSG.DataBind();
        //    }

        //}


        public void Edit()
        {
            try
            {
                int countDetRec = 0;
                igpNo = Convert.ToInt32(txtSrcIGP.Text.Substring(0, 4) + txtSrcIGP.Text.Substring(5));
                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtGpDate.Text);

                string no = txtGPNo.Text.Substring(5);
                string yrno = txtGPNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);



                itmData.vt_cd = 14;
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
                tblItemData rec = sG.GetRec(Convert.ToInt32(no), Convert.ToInt32(txtSrcIGP.Text.Substring(0, 4)), 14, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                itmData.DocRef = rec.DocRef;
                itmData.IGPNo = rec.IGPNo;


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
                        itmDet.vt_cd = 14;
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



                        itmDet.LotRef = 0;//
                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        itmDet.DrumNo = 0;
                        itmDetEnt.Add(itmDet);
                    }
                }
                /**********************/
                int sum = 0;
                string quantity = sG.GetQtyByIGP(igpNo, 11, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                int qtySum = Convert.ToInt32(Convert.ToDecimal(quantity));
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string val = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtQuantity")).Text;
                    if (val != "")
                    {
                        sum = sum + Convert.ToInt32(val);
                    }
                }


                if (sum == qtySum)
                {

                    int sgNo = Convert.ToInt32(Session["SGno"]);
                    int yr = Convert.ToInt32(Session["DateYear"]);
                    string vrNo = Session["vrNo"].ToString();
                    bool res = sG.EditRec(sgNo, Convert.ToInt32(vrNo.Substring(0, 4)), 14, itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (res == true)
                    {
                        ucMessage.ShowMessage("Updated successfully", RMS.BL.Enums.MessageType.Info);
                        Response.Redirect("~/invsetup/sizinggradinghome.aspx?PID=422");
                    }
                    else
                    {
                        ucMessage.ShowMessage("Update was  unsuccessful", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Quantity in parts should match IGP quantity.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Cannot edit. Message: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        
        
        
        //[WebMethod]
        //public static List<string> GetControlProduct(string sname)
        //{
        //    InvGP_BL gP1 = new InvGP_BL();
        //    return gP1.GetControlProduct(sname);
        //}



        public void Save()
        {
            try
            {
                int countDetRec = 0;
                igpNo = Convert.ToInt32(srchVal.Substring(0, 4) + srchVal.Substring(5));

                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtGpDate.Text);

                string no = txtGPNo.Text.Substring(5);
                string yrno = txtGPNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                itmData.vt_cd = 14;
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
                itmData.DocRef = grdIGP.DataKeys[0].Value.ToString();
                itmData.IGPNo = igpNo;

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
                        itmDet.vt_cd = 14;
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

                /***********************/
                bool res = false;
                int sum = 0;
                string quantity = sG.GetQtyByIGP(igpNo, 11, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                int qtySum = Convert.ToInt32(Convert.ToDecimal(quantity));
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string val = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtQuantity")).Text;
                    if (val != "")
                    {
                        sum = sum + Convert.ToInt32(val);
                    }
                }


                if (sum == qtySum)
                {


                    if (countDetRec > 0)
                        res = sG.SaveItemData(itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    else
                        res = false;


                    if (res == true)
                    {
                        ucMessage.ShowMessage("Saved successfully", RMS.BL.Enums.MessageType.Info);
                        Response.Redirect("~/invsetup/sizinggradinghome.aspx?PID=422");
                    }
                    else
                    {
                        ucMessage.ShowMessage("Save was unsuccessful", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Quantity in parts should match IGP quantity.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Cannot save. Message: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindFields()
        {
            int sgNo = Convert.ToInt32(Session["SGno"]);
            int yr = Convert.ToInt32(Session["DateYear"]);
            string vrNO = Session["vrNo"].ToString();
            tblItemData rec = sG.GetRec(sgNo, Convert.ToInt32(vrNO.Substring(0,4)), 14, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlLoc.SelectedValue = rec.LocId.ToString();
            txtGPNo.Text = rec.vr_no.ToString().Substring(0, 4) + "/" + rec.vr_no.ToString().Substring(4);

            CalendarExtender1.SelectedDate = rec.vr_dt;


            if (rec.LotNo > 0)
                txtLotNo.Text = rec.LotNo.ToString().Substring(0, 4) + "-" + rec.LotNo.ToString().Substring(4);
                        ddlStatus.SelectedValue = rec.vr_apr.ToString();
                        txtRemarks.Text = rec.vr_nrtn;
                        ddlProduct.SelectedValue = sG.GetItemCode1(rec.vr_no,Convert.ToInt32( rec.vr_no.ToString().Substring(0, 4)), 14, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        igpNo = Convert.ToInt32(rec.IGPNo);
                        txtSrcIGP.Text = rec.IGPNo.ToString().Substring(0, 4) + "/" + rec.IGPNo.ToString().Substring(4);
                        BindGridIGP();

        }

        public void GetSizeGradingNo()
        {
            txtGPNo.Text = sG.GetSGNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 14, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        
        public void BindDDLLoc()
        {
            ddlLoc.DataSource = sG.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
            ddlLoc.SelectedValue = "1";

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
                    //string igpNo = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtIGpGrid")).Text;
                    //string Prdct = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlProduct")).SelectedValue;
                    string sizGrad = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[2].FindControl("ddlSizGrd")).SelectedValue;
                   
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtQuantity")).Text;
                    string vat = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[4].FindControl("txtWeight")).Text;
                    d_Row["Sr"] = srNo;
                    //if(igpNo != "")
                    //d_Row["IGPNo"] = igpNo;
                    //d_Row["Product"] = Prdct;
                    d_Row["SizingGrading"] = sizGrad;
           
                    if (qty != "")
                        d_Row["Quantity"] = Convert.ToInt32(qty);
                    if (vat != "")
                        d_Row["Weight"] = Convert.ToDecimal(vat);
                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;//assigning to viewstate datatable
                BindGrid();
              

            }
        }
        
        public void GetColumns()
        {
            d_table.Columns.Clear();
            d_table.Rows.Clear();

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Sr";
            d_table.Columns.Add(d_Col);



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
            List<string> lst = sG.GetSizeGradeCode1(Item_Code, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            string cdDesc = "";
            string code = "";
            int count = 0;
            GetColumns();
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
            string vrNo = Session["vrNo"].ToString();

            List<string> lst = sG.GetSizeGradeCode1(Item_Code, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblItemDataDet> list = sG.GetRecDet(sgNo,Convert.ToInt32(vrNo.Substring(0,4)), 14, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            
            for(int i=0; i< lst.Count; i++)
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
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }

        #endregion
    }
}
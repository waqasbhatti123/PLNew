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
    public partial class MaterialPurchaseNoteMgt : BasePage
    {
        #region DataMembers

        //DashBoardBL dashBoardBL = new DashBoardBL();
        EntitySet<tblItemDataDet> itmDetEnt = new EntitySet<tblItemDataDet>();
        InvGP_BL gP = new InvGP_BL();
        InvSG_BL sG = new InvSG_BL();
        InvMN_BL mN = new InvMN_BL();
#pragma warning disable CS0414 // The field 'MaterialPurchaseNoteMgt.count' is assigned but its value is never used
        int count;
#pragma warning restore CS0414 // The field 'MaterialPurchaseNoteMgt.count' is assigned but its value is never used
      
        #endregion

        #region Properties
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;

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
        public string grdIGPCheck
        {
            set { ViewState["grdIGPCheck"] = value; }
            get { return Convert.ToString(ViewState["grdIGPCheck"] ?? ""); }
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
                
          

                //lblToStore.Text = "Store Location:";
           
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                }

                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender2.Format = Session["DateFormat"].ToString();
                }

                //CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender3.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender3.Format = Session["DateFormat"].ToString();
                }
                if (Convert.ToBoolean(Session["IfEdit"]) == false)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "MatPurchNt").ToString();
                    
                    BindDDLLoc();
                    BindDDLVendor();
                    BindDDLCity();
                    GetMatPurchaseNo();
                    btnBack.Visible = false;
                    btnClear.Visible = true;
                    btnList.Visible = true;
                }
                else
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", Session["PgTile"].ToString()).ToString();
                    
                    BindDDLLoc();
                    BindDDLVendor();
                    BindDDLCity();
                    
                    BindFields();
                    BindIGPFlds();
                    imgFind_Click(null, null);
                    BindRateFields();
                    BindEditTable();
                    txtLotNo.ReadOnly = true;
                    btnClear.Visible = false;
                    btnBack.Visible = true;
                    btnList.Visible = false;
                    imgFind.Visible = false;
                }

            }
             count = 0;
        }
        protected void imgFind_Click(object sender, EventArgs e)
        {
            if (txtLotNo.Text != "")
            {
                try
                {
                    bool exist = false;
                    ltNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));
                    exist = mN.GetIfMPExists(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    bool matched = mN.GetIfQtyMathced(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    if (matched == true)
                    {
                        if (exist == false)
                        {

                            Item_Code = mN.GetItemUzngLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            Item_Code = Item_Code.Substring(2);
                            docRef = mN.GetDocRefByLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            
                            BindTable();
                            BindIGPFlds();
                            BindGrids();
                            DateTime dtTime = CalendarExtender2.SelectedDate.Value;
                            dtTime = dtTime.AddDays(90);
                            CalendarExtender3.SelectedDate = dtTime.Date;
                            
                        }
                        else
                        {
                            if (Convert.ToBoolean(Session["IfEdit"]) == true)
                            {
                                Item_Code = mN.GetItemUzngLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                Item_Code = Item_Code.Substring(2);
                                docRef = mN.GetDocRefByLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                BindIGPFlds();
                                BindGrids();
                            }
                            else
                            {
                                
                                ucMsgPnl.ShowMessage("Card already exists against this lot.", RMS.BL.Enums.MessageType.Error);
                            }
                        }
                    }
                    else
                    {
                        ucMsgPnl.ShowMessage("Grading or feetage card is not created yet.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                catch(Exception ex)
                {
                    ucMsgPnl.ShowMessage("Exception: "+ ex.Message, RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                ucMsgPnl.ShowMessage("Please enter lot no.", RMS.BL.Enums.MessageType.Error);
            }
            
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/MaterialPurchaseNoteMgt.aspx?PID=434");
        }
        protected void btnList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/MaterialPurchaseNoteHome.aspx?PID=433");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool flagAmount = false;
            decimal amnt = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                amnt = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text);
                if (amnt < 1)
                {
                    flagAmount = true;
                    break;
                }
            }

            if (Convert.ToBoolean(Session["IfEdit"]) == false)
            {
                string ratePerSqft = txtFinalRate.Text;
                string ratePerPc = txtPerPieceCost.Text;
                if (ratePerPc == "NaN" || ratePerSqft == "NaN")
                {
                    ucMessage.ShowMessage("Cannot save, Please enter valid amount", RMS.BL.Enums.MessageType.Error);
                }
                else
                {

                    if (flagAmount == false)
                    {
                        Save();
                    }
                    else
                    {
                        ucMessage.ShowMessage("Cannot save, Please enter valid amount", RMS.BL.Enums.MessageType.Error);
                    }
                }


            }
            else
            {
                string ratePerSqft = txtFinalRate.Text;
                string ratePerPc = txtPerPieceCost.Text;
                if (ratePerPc == "NaN" || ratePerSqft == "NaN")
                {
                    ucMessage.ShowMessage("Cannot edit, Please enter valid amount", RMS.BL.Enums.MessageType.Error);
                }
                else
                {
                    if (flagAmount == false)
                    {
                        Edit();
                    }
                    else
                    {
                        ucMessage.ShowMessage("Cannot edit, Please enter valid amount", RMS.BL.Enums.MessageType.Error);
                    }
                }

            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["PrePgAddress"].ToString());
        }
        //=======================================================
        protected void grdFeetage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
             
                    if (e.Row.Cells[0].Text == grdIGPCheck)
                    {
                      
                            e.Row.Cells[0].Text = "";
                       
                    }
                    else
                    {
                        grdIGPCheck = e.Row.Cells[0].Text.ToString();
                        e.Row.Cells[0].Text = e.Row.Cells[0].Text.ToString().Substring(0, 4) + "/" + e.Row.Cells[0].Text.ToString().Substring(4);
                    }
                    
              
                   
                
                e.Row.Cells[3].Text = (Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[3].Text))).ToString();
                decimal d = Convert.ToDecimal(e.Row.Cells[4].Text);
                e.Row.Cells[4].Text = decimal.Round(d, 2).ToString();


            }

        }

        protected void grdGrading_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[2].Text)).ToString();
                

                decimal d2 = Convert.ToDecimal(e.Row.Cells[3].Text);
                e.Row.Cells[3].Text = decimal.Round(d2, 2).ToString();

                decimal d3 = Convert.ToDecimal(e.Row.Cells[4].Text);
                e.Row.Cells[4].Text = decimal.Round(d3, 2).ToString();

                decimal d4 = Convert.ToDecimal(e.Row.Cells[5].Text);
                e.Row.Cells[5].Text = decimal.Round(d4, 2).ToString();
            }
        }
        //===============================================
        #endregion
        
        #region Helping Method

        public void BindIGPFlds()
        {
            string igpsNo="";
            decimal sumAmount = 0;
            string igp = "";
            int m = 0;
            List<tblStkGP> rec = mN.GetIGPRecByLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (rec.Count > 0)
            {
                foreach (var r in rec)
                {
                    igp = r.vr_no.ToString().Substring(0, 4) + "/" +r.vr_no.ToString().Substring(4);
                    if (m == 0)
                    {
                        igpsNo = igp;
                        ddlVendor.SelectedValue = r.VendorId;
                        ddlCity.SelectedValue = r.VendorCity;
                        txtIGPDate.Text = r.vr_dt.ToString();
                        CalendarExtender2.SelectedDate = r.vr_dt;
                        txtFrt.Text = r.Freight.Value.ToString();
                    }
                    else
                    {
                        igpsNo = igpsNo + "," + igp;
                    }
                    m++;
                }
               // txtNewIGPNo.Text = igpsNo;

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    sumAmount = sumAmount + Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text); ;
                }
                txtFinalAmount.Text = sumAmount.ToString();
            }
        }

        public void BindRateFields()
        {
            try
            {
                decimal rate = 0;
                //int sgNo = Convert.ToInt32(Session["SGno"]);
                //int yr = Convert.ToInt32(Session["DateYear"]);
                //int ltNo = Convert.ToInt32(txtLotNo.Text);
                int count=0;
                string  obj = mN.GetItemUzngLot4mItemData(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                string[] spltStr = Regex.Split(obj, ":");
                foreach (string part in spltStr)
                {
                    count++;
                    if (count == 1)
                    {
                        //txtFinalRate.Text =part;
                        rate =Convert.ToDecimal( part);
                    }
                    if (count == 2)
                    {
                        decimal round = decimal.Round(Convert.ToDecimal(lblFTtlArea.Text) * rate, 0);
                        txtFinalAmount.Text = Convert.ToString(round);
                    }
                    if (count == 3)
                    {
                        txtCommission.Text = part;
                    }
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull(); throw ex;
            }
        }

        public void BindFields()
        {
            int yr = Convert.ToInt32(Session["DateYear"]);
            ltNo = Convert.ToInt32(Session["LotNo"]);
            string vrNo = Session["vrNo"].ToString();

            txtLotNo.Text = ltNo.ToString().Substring(0, 4) + "-" + ltNo.ToString().Substring(4);
            int sgNo = Convert.ToInt32(Session["SGno"]);
            
            tblItemData rec = mN.GetRec(sgNo, Convert.ToInt32(vrNo.Substring(0, 4)), 21, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            txtGPNo.Text = rec.vr_no.ToString().Substring(0, 4) + "/" + rec.vr_no.ToString().Substring(4);
            CalendarExtender1.SelectedDate = rec.vr_dt;


            ddlStatus.SelectedValue = rec.vr_apr.ToString();
            txtRemarks.Text = rec.vr_nrtn;
            CalendarExtender3.SelectedDate = rec.Due_Date;

        }

        public void Edit()
        {
            try
            {
                decimal ftg = Convert.ToDecimal(lblFTtlArea.Text);
                decimal qtySum = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    qtySum = qtySum + Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text); ;
                }

                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtGpDate.Text);

                string no = txtGPNo.Text.Substring(5);
                string yrno = txtGPNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                itmData.vt_cd = 21;
                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);

                itmData.vr_nrtn = txtRemarks.Text;
                itmData.gl_cd = "";
                itmData.post_2gl = true;
                itmData.LocId = 1;

                itmData.LotNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));


                itmData.ToLocId = 0;
                itmData.Freight = Convert.ToDecimal(txtFrt.Text);
                itmData.Tax = 0;

                if (txtCommission.Text != "")
                    itmData.Commission = Convert.ToDecimal(txtCommission.Text);
                else
                    itmData.Commission = 0;

                itmData.vr_apr = Convert.ToString(ddlStatus.SelectedValue);
                itmData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (ddlStatus.SelectedValue == "A" || ddlStatus.SelectedValue == "C")
                {
                    if (Session["UserName"] == null)
                        itmData.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itmData.updateby = Session["UserName"].ToString();
                }
                itmData.Status = "Mp";
                docRef = mN.GetDocRefByLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                itmData.DocRef = docRef;
                itmData.Due_Date = Convert.ToDateTime(txtDueDate.Text);
                itmData.Pay_Status = "OP";


                int srCount = 1;
                string igpNo = "";
                string LtNo = txtLotNo.Text;

                if (LtNo != "")
                {
                   // List<spGetFeetageParamByLotNoResult> lst = mN.GetFeetCardPara(14, ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    for (int j = 0; j < GridView1.Rows.Count; j++)
                    {


                        tblItemDataDet itmDet = new tblItemDataDet();
                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = 21;
                        string nos = txtGPNo.Text.Substring(5);
                        string yrnos = txtGPNo.Text.Substring(0, 4) + no;
                        itmDet.vr_no = Convert.ToInt32(yrnos);

                        itmDet.vr_seq = Convert.ToByte(srCount++);
                        itmDet.itm_cd = Item_Code;

                        itmDet.LocId = 1;



                        //if (lblFTtlQty.Text != "")
                        // itmDet.vr_qty = Convert.ToDecimal(lblFTtlQty.Text);
                        //itmDet.vr_qty = Convert.ToDecimal(a.quantity);
                        itmDet.vr_qty = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text);


                        itmDet.KGSwt = 0;

                        //itmDet.vr_val = Convert.ToDecimal(txtFinalAmount.Text);
                        itmDet.vr_val = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtAmount")).Text);


                        //itmDet.vr_rate = Convert.ToDecimal(txtFinalRate.Text);
                        itmDet.vr_rate = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtAmount")).Text) /
                            Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text); ;

                        //if (lblFTtlArea.Text != "")
                        //itmDet.Feetage = Convert.ToDecimal(lblFTtlArea.Text);

                        itmDet.Feetage = ftg * Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text) / qtySum;




                        igpNo = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtIGP")).Text;
                        igpNo = igpNo.Substring(0, 4) + igpNo.Substring(5);
                        itmDet.IGP_Ref = Convert.ToInt32(igpNo);


                        itmDet.vr_rmk = "";
                        itmDet.GradeId = "";

                        itmDet.LotRef = 0;

                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        itmDet.DrumNo = 0;
                        itmDetEnt.Add(itmDet);
                    }

                }
                int sgNo = Convert.ToInt32(Session["SGno"]);
                int yr = Convert.ToInt32(Session["DateYear"]);
                string vrNo = Session["vrNo"].ToString();

                bool res = false;
                if (LtNo != "")
                    res = mN.EditRec(sgNo, Convert.ToInt32(vrNo.Substring(0, 4)), 21, itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                else
                    res = false;


                if (res == true)
                {
                    ucMessage.ShowMessage("Updated successfully", RMS.BL.Enums.MessageType.Info);
                    Response.Redirect("~/invsetup/MaterialPurchaseNoteHome.aspx?PID=433");
                }
                else
                {
                    ucMessage.ShowMessage("Update was unsuccessful", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage("Exception: "+ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void Save()
        {
            try
            {

                decimal ftg = Convert.ToDecimal(lblFTtlArea.Text);
                decimal qtySum = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    qtySum = qtySum + Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text); ;
                }
                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtGpDate.Text);

                string no = txtGPNo.Text.Substring(5);
                string yrno = txtGPNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                itmData.vt_cd = 21;
                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);

                itmData.vr_nrtn = txtRemarks.Text;
                itmData.gl_cd = "";
                itmData.post_2gl = true;
                itmData.LocId = 1;

                itmData.LotNo = Convert.ToInt32(txtLotNo.Text.Substring(0, 4) + txtLotNo.Text.Substring(5));


                itmData.ToLocId = 0;

                if (txtFrt.Text != "")
                    itmData.Freight = Convert.ToDecimal(txtFrt.Text);
                else
                    itmData.Freight = 0;

                itmData.Tax = 0;

                if (txtCommission.Text != "")
                    itmData.Commission = Convert.ToDecimal(txtCommission.Text);
                else
                    itmData.Commission = 0;

                itmData.vr_apr = Convert.ToString(ddlStatus.SelectedValue);
                itmData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (ddlStatus.SelectedValue == "A" || ddlStatus.SelectedValue == "C")
                {
                    if (Session["UserName"] == null)
                        itmData.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itmData.updateby = Session["UserName"].ToString();
                }
                itmData.Status = "Mp";
                itmData.DocRef = docRef;
                itmData.Due_Date = Convert.ToDateTime(txtDueDate.Text);
                itmData.Pay_Status = "OP";


                int srCount = 1;
                string igpNo = "";
                string LtNo = txtLotNo.Text;

                if (LtNo != "")
                {

                    //List<spGetFeetageParamByLotNoResult> lst = mN.GetFeetCardPara(14, ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    for (int j = 0; j < GridView1.Rows.Count;j++ )
                    {

                        tblItemDataDet itmDet = new tblItemDataDet();
                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = 21;
                        string nos = txtGPNo.Text.Substring(5);
                        string yrnos = txtGPNo.Text.Substring(0, 4) + no;
                        itmDet.vr_no = Convert.ToInt32(yrnos);

                        itmDet.vr_seq = Convert.ToByte(srCount++);
                        itmDet.itm_cd = Item_Code;
                        itmDet.LocId = 1;



                        //if (lblFTtlQty.Text != "")
                        // itmDet.vr_qty = Convert.ToDecimal(lblFTtlQty.Text);
                        itmDet.vr_qty = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text);



                        itmDet.KGSwt = 0;

                        //itmDet.vr_val = Convert.ToDecimal(txtFinalAmount.Text);
                        itmDet.vr_val = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtAmount")).Text);


                        //itmDet.vr_rate = Convert.ToDecimal(txtFinalRate.Text);
                        itmDet.vr_rate = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtAmount")).Text)/
                            Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text); ;

                        //if (lblFTtlArea.Text != "")
                        //itmDet.Feetage = Convert.ToDecimal(lblFTtlArea.Text);
                        
                        //itmDet.Feetage = Convert.ToDecimal(a.area);


                        itmDet.Feetage = ftg * Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text) / qtySum;

                        igpNo = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtIGP")).Text;
                        igpNo = igpNo.Substring(0, 4) + igpNo.Substring(5);
                        itmDet.IGP_Ref = Convert.ToInt32(igpNo);

                        itmDet.vr_rmk = "";
                        itmDet.GradeId = "";

                        itmDet.LotRef = 0;

                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        itmDet.DrumNo = 0;
                        itmDetEnt.Add(itmDet);
                    }
                }


                bool res = false;
                if (LtNo != "")
                    res = mN.SaveItemData(itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                else
                    res = false;


                if (res == true)
                {
                    ucMessage.ShowMessage("Saved successfully", RMS.BL.Enums.MessageType.Info);
                    Response.Redirect("~/invsetup/MaterialPurchaseNoteHome.aspx?PID=433");
                }
                else
                {
                    ucMessage.ShowMessage("Save was unsuccessful", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage("Exception: " +ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindOtherFields()
        {
            decimal areaFeetage = 0;
            decimal qtyFeetage = 0;
            decimal areaGrading = 0;
            decimal qtyGrading = 0;

            List<tblItemDataDet> obj1 = mN.GetObjByLot(ltNo, 14, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblItemDataDet> obj2 = mN.GetObjByLot(ltNo, 15, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            foreach (var a in obj1)
            {
                areaFeetage = areaFeetage + Convert.ToDecimal(a.Feetage);
                qtyFeetage = qtyFeetage + Convert.ToDecimal(a.vr_qty);
            }

            foreach (var b in obj2)
            {
                areaGrading = areaGrading + Convert.ToDecimal(b.Feetage);
                qtyGrading = qtyGrading + Convert.ToDecimal(b.vr_qty);
            }

            

            lblFTtlQty.Text = Convert.ToInt32(qtyFeetage).ToString();
            lblFTtlArea.Text = Decimal.Round(areaFeetage,2).ToString();


            lblGTtlArea.Text = Decimal.Round(areaGrading,2).ToString();
            lblGTtlQty.Text = Convert.ToInt32(qtyGrading).ToString();
        }

        public void BindGrids()
        {

            List<spGetFeetageGradingRecResult> resFeetage = mN.GetFeetageGradingRec(14, ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<spGetSelectionRecResult> resGrding = mN.GetSelectionRec(15, ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (resFeetage.Count > 0 || resGrding.Count > 0)
            {
                if (resFeetage.Count > 0 && resGrding.Count > 0)
                {
                    lblGradingCard.Visible = true;
                    lblFeetageCard.Visible = true;
                    lblFeetTtlQty.Visible = true;
                    lblFeetTtlArea.Visible = true;
                    lblGradTtlQty.Visible = true;
                    lblGradTtlArea.Visible = true;
                    lblGTtlArea.Visible = true;
                    lblFTtlArea.Visible = true;
                    lblFTtlQty.Visible = true;
                    lblGTtlQty.Visible = true;


                    string itmCode = mN.GetItemUzngLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    itmCode = itmCode.Substring(2);
                    string product = mN.GetProductByItmCode(itmCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    lblFeetageCard.Text = "Feetage Card (" + product + ")";


                    grdFeetage.DataSource = resFeetage;
                    grdFeetage.DataBind();
                    grdGrading.DataSource = resGrding;
                    grdGrading.DataBind();
                    BindOtherFields();

                }
                else
                {
                    if (resFeetage.Count > 0)
                    {
                        ucMsgPnl.ShowMessage("Grading card not created yet.", RMS.BL.Enums.MessageType.Error);
                    }
                    else
                    {
                        ucMsgPnl.ShowMessage("Feetage card not created yet.", RMS.BL.Enums.MessageType.Error);
                    }
                }
            }
            else
            {
                ucMsgPnl.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Error);
            }

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

        public void GetMatPurchaseNo()
        {
            txtGPNo.Text = mN.GetSGNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 21, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        
        public void BindDDLLoc()
        {
            //ddlLoc.DataSource = sG.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlLoc.DataValueField = "LocId";
            //ddlLoc.DataTextField = "LocName";
            //ddlLoc.DataBind();
            //ddlLoc.SelectedValue = "1";

        }

        public void BindDDLVendor()
        {
            ddlVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.DataTextField = "gl_dsc";
            ddlVendor.DataValueField = "gl_cd";
            ddlVendor.DataBind();
        }

        public void BindDDLCity()
        {
            ddlCity.DataSource = gP.GetCity((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityID";
            ddlCity.DataBind();
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
            d_Col.ColumnName = "vr_no";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "GPRef";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Party";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Int32");
            d_Col.ColumnName = "vr_qty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Decimal");
            d_Col.ColumnName = "Price";
            d_table.Columns.Add(d_Col);


            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Decimal");
            d_Col.ColumnName = "Amount";
            d_table.Columns.Add(d_Col);

        }

        public void BindTable()
        {
            GetColumns();

            List<spGetIGPByLotResult> lst = mN.GetIGPByLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            
            foreach(var l in lst)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = l.Sr;
                d_Row["vr_no"] = l.vr_no;
                d_Row["GPRef"] = l.GPRef;
                d_Row["Party"] = l.Party;
                d_Row["vr_qty"] = l.vr_qty;
                d_Row["Price"] = l.Price;
                d_Row["Amount"] = l.Price;
                

                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }

        public void BindEditTable()
        {
            decimal sumAmount = 0;
            GetColumns();
            List<spGetIGPByLotResult> lst1 = mN.GetIGPByLot(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblItemDataDet> lst = mN.GetIGPByLotEdit(ltNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            for (int i = 0; i < lst.Count;i++ )
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = lst[i].vr_seq;
                d_Row["vr_no"] = lst[i].IGP_Ref.ToString().Substring(0, 4) + "/" + lst[i].IGP_Ref.ToString().Substring(4);
                d_Row["GPRef"] = mN.GetGPRefByIGP(Convert.ToInt32(lst[i].IGP_Ref), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]); ;
                d_Row["Party"] = mN.GetPartyByIGP(Convert.ToInt32(lst[i].IGP_Ref), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                d_Row["vr_qty"] = lst[i].vr_qty;
                d_Row["Price"] = lst1[i].Price;
                d_Row["Amount"] = lst[i].vr_val;

                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                sumAmount = sumAmount + Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text); ;
            }
            txtFinalAmount.Text = sumAmount.ToString();
        }

        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }

        #endregion
    }
}
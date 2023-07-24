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
using AjaxControlToolkit; 

namespace RMS
{
    public partial class MaterialPurchaseNoteMgtNew : BasePage
    {

        #region DataMembers

        InvMN_BL mN = new InvMN_BL();
        EntitySet<tblItemDataDet> itmDetEnt = new EntitySet<tblItemDataDet>();
        InvGP_BL gP = new InvGP_BL();

        DataTable d_table = new DataTable();
#pragma warning disable CS0169 // The field 'MaterialPurchaseNoteMgtNew.d_Row' is never used
        DataRow d_Row;
#pragma warning restore CS0169 // The field 'MaterialPurchaseNoteMgtNew.d_Row' is never used
#pragma warning disable CS0169 // The field 'MaterialPurchaseNoteMgtNew.d_Col' is never used
        DataColumn d_Col;
#pragma warning restore CS0169 // The field 'MaterialPurchaseNoteMgtNew.d_Col' is never used

        DataTable d_tableIGP = new DataTable();
        DataRow d_RowIGP;
        DataColumn d_ColIGP;
        decimal sT = 0;
        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        public DataTable CurrentTable
        {
            set { ViewState["CurrentTable"] = value; }
            get { return (DataTable)(ViewState["CurrentTable"] ?? 0); }
        }

        public int lotNo
        {
            get { return Convert.ToInt32(ViewState["lotNo"]); }
            set { ViewState["lotNo"] = value; }
        }

        public int igpNo
        {
            get { return Convert.ToInt32(ViewState["igpNo"]); }
            set { ViewState["igpNo"] = value; }
        }

        public int AreaFtg
        {
            get { return Convert.ToInt32(ViewState["AreaFtg"]); }
            set { ViewState["AreaFtg"] = value; }
        }

        public int QuantityFtg
        {
            get { return Convert.ToInt32(ViewState["QuantityFtg"]); }
            set { ViewState["QuantityFtg"] = value; }
        }

        public int AreaGrd
        {
            get { return Convert.ToInt32(ViewState["AreaGrd"]); }
            set { ViewState["AreaGrd"] = value; }
        }

        public int QuantityGrd
        {
            get { return Convert.ToInt32(ViewState["QuantityGrd"]); }
            set { ViewState["QuantityGrd"] = value; }
        }

        public double Amount
        {
            get { return Convert.ToDouble(ViewState["Amount"]); }
            set { ViewState["Amount"] = value; }
        }

        public int Frieght
        {
            get { return Convert.ToInt32(ViewState["Frieght"]); }
            set { ViewState["Frieght"] = value; }
        }

        public string lotNoformtated
        {
            get { return Convert.ToString(ViewState["lotNoformtated"]); }
            set { ViewState["lotNoformtated"] = value; }
        }

        public DataTable LotTable
        {
            get { return (DataTable)(ViewState["LotTable"]); }
            set { ViewState["LotTable"] = value; }
        }

        public string Item_Code
        {
            set { ViewState["Item_Code"] = value; }
            get { return Convert.ToString(ViewState["Item_Code"]); }
        }

        public string grdIGPCheck
        {
            set { ViewState["grdIGPCheck"] = value; }
            get { return Convert.ToString(ViewState["grdIGPCheck"] ?? ""); }
        }

        public string Doc_Ref
        {
            set { ViewState["Doc_Ref"] = value; }
            get { return Convert.ToString(ViewState["Doc_Ref"] ?? ""); }
        }

        public string Doc_RefFirst
        {
            set { ViewState["Doc_Ref"] = value; }
            get { return Convert.ToString(ViewState["Doc_Ref"] ?? ""); }
        }

        public int igpNoFirst
        {
            get { return Convert.ToInt32(ViewState["igpNoFirst"]); }
            set { ViewState["igpNoFirst"] = value; }
        }

        public int mpnNo
        {
            get { return Convert.ToInt32(ViewState["mpnNo"]); }
            set { ViewState["mpnNo"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                }

                if (Session["DateFormat"] == null)
                {
                    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender2.Format = Session["DateFormat"].ToString();
                }

                if (Session["DateFormat"] == null)
                {
                    CalendarExtender3.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender3.Format = Session["DateFormat"].ToString();
                }

                PID = Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 434)
                {
                    DiscardFalseEntities();
                    
                    if (Convert.ToBoolean(Session["IfEdit"]) == false)
                    {
                        Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "MatPurchNtMgtNew").ToString();

                        GetMatPurchaseNo();

                        BindDdlSelectVendor();
                        BindDdlSelectItem();
                        BindDDLVendor();
                        BindDDLCity();

                        pnlFields.Visible = false;
                        btnBack.Visible = false;
                        btnClear.Visible = false;
                        btnSave.Visible = false;
                        btnList.Visible = true;
                    }
                    else
                    {
                        Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", Session["PgTile"].ToString()).ToString();

                        pnlSrchIGPs.Visible = false;
                        pnlGetIGPs.Visible = false;
                        pnlVendor.Visible = true;
                        tblLine.Visible = true;
                        lblGradingCard.Visible = true;
                        lblFeetageCard.Visible = true;
                        lblIGPsDet.Visible = true;
                        lblVvDet.Visible = true;

                        igpNoFirst = Convert.ToInt32(Session["igpNum"]);
                        Doc_RefFirst =  mN.GetDocRefByIGP(igpNoFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        mpnNo = Convert.ToInt32(Session["vrNum"]);
                        lblVvDet.Attributes.Add("onclick", "window.open('../InvSetupSupport/ViewIgp.aspx?ID=" + Doc_RefFirst + "','IGPDetail','height=400,width=800,toolbar=no, menubar=no, scrollbars=yes, resizable=yes,location=no, directories=no, status=no');return false");

                        BindDDLVendor();
                        BindDDLCity();

                        BindIGPFldsEdit();
                        BindCardFieldsEdit();
                        GetDataFtgGrdEdit();
                        BindGridsFtgGrdCards();
                        lblFTtlQty.Text = QuantityFtg.ToString();
                        lblFTtlArea.Text = AreaFtg.ToString();
                        BindTableEdit();
          
                        btnBack.Visible = false;
                        btnClear.Visible = false;
                        btnSave.Visible = true;
                        btnList.Visible = true;
                        
                    }
                    
                }
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            itmDetEnt.Clear();
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

        protected void LnkSelectVendor_Click(object sender, EventArgs e)
        {
            try
            {
                List<spGetIgpsDetailByVendor4MPNResult> lst = mN.GetIGPsDetailByVendor4MPN(Convert.ToInt32(ddlSelectVendor.SelectedValue), ddlSelectItem.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (lst.Count > 0)
                {
                    BindGridIGPs(lst);
                }
                else
                {
                    BindGridIGPs(null);
                    uMsg.ShowMessage("No record found against this vendor.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                uMsg.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void LnkMergIgps_Click(object sender, EventArgs e)
        {
            try
            {
                bool Checked = false;
                for (int i = 0; i < grdIGPs.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)grdIGPs.Rows[i].FindControl("cbxSelectIGP");
                    if (cbx.Checked == true)
                    {
                        Checked = true;                     
                    }
                }
                if (Checked == true)
                {
                    igpNo = 0;
                    int m = 0;
                    Item_Code = ddlSelectItem.SelectedValue;
                    mN.DeleteFtgTempTable((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    mN.DeleteGrdTempTable((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    pnlSrchIGPs.Visible = false;
                    pnlGetIGPs.Visible = false;

                    for (int i = 0; i < grdIGPs.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)grdIGPs.Rows[i].FindControl("cbxSelectIGP");
                        if (cbx.Checked == true)
                        {
                            igpNo = Convert.ToInt32(grdIGPs.Rows[i].Cells[1].Text.Substring(0, 4) + grdIGPs.Rows[i].Cells[1].Text.Substring(5));
                            if (m == 0)
                            {
                                igpNoFirst = igpNo;
                                Doc_RefFirst = mN.GetDocRefByIGP(igpNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                m++;
                            }
                            GetIGPsVal();

                            //inserting PMN_Ref----------------

                            List<tblStkGP> recs = mN.GetRecsStkGPByStrtIGP1(igpNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            foreach (tblStkGP rec in recs)
                            {
                                rec.PMN_Ref = Doc_RefFirst;
                            }
                            //update tblStkGP
                            mN.UpdateTblStkGP((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                            List<tblItemData> records = mN.GetRecordsTblItemData(14, igpNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            foreach (tblItemData record in records)
                            {
                                record.PMN_Ref = Doc_RefFirst;
                            }
                            //update tblItemData
                            mN.UpdateTblItemData(records, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                           
                        }
                    }
                    //Insert Into FtgTempTable-------------
                    mN.InsertingIntoFtgTempTable(Doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    //Insert Into GrdTempTable-------------
                    mN.InsertingIntoGrdTempTable(Doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    BindGridsFtgGrdCards();
                    GetPurchseNoteInfo();
                    lblGradingCard.Visible = true;
                    lblFeetageCard.Visible = true;
                    pnlVendor.Visible = true;
                    pnlFields.Visible = true;
                    pnlIGPs.Visible = true;
                    btnClear.Visible = true;
                    btnSave.Visible = true;
                    lblIGPsDet.Visible = true;
                    lblVvDet.Visible = true;
                    tblLine.Visible = true;

                    txtGpDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date.ToString();
                    CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    txtFrt.Text = Frieght.ToString();
                    txtFinalAmount.Text = Amount.ToString();

                    lblFTtlQty.Text = QuantityFtg.ToString();
                    lblFTtlArea.Text = AreaFtg.ToString();

                    BindTable();
                    lblVvDet.Attributes.Add("onclick", "window.open('../InvSetupSupport/ViewIgp.aspx?ID=" + Doc_RefFirst + "','IGPDetail','height=400,width=800,toolbar=no, menubar=no, scrollbars=yes, resizable=yes,location=no, directories=no, status=no');return false");
                }
                else
                {
                    uMsg.ShowMessage("Please, select IGP to merge.", RMS.BL.Enums.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                uMsg.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void GrdIGPs_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0,4)+"/"+e.Row.Cells[1].Text.Substring(4);
            }
        }

        protected void grdFeetage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total ";
                e.Row.Cells[2].Text = QuantityFtg.ToString();
                e.Row.Cells[3].Text = AreaFtg.ToString();
            }

        }

        protected void grdGrading_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[2].Text)).ToString();
                e.Row.Cells[4].Text = Math.Round(Convert.ToDecimal(e.Row.Cells[4].Text), 3).ToString();
                e.Row.Cells[5].Text = Math.Round(Convert.ToDecimal(e.Row.Cells[5].Text), 2).ToString();
                sT = sT + Convert.ToDecimal(e.Row.Cells[5].Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total ";
                e.Row.Cells[2].Text = QuantityGrd.ToString();
                e.Row.Cells[3].Text = AreaGrd.ToString();
                e.Row.Cells[5].Text = Convert.ToInt32(sT).ToString();
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DiscardFalseEntities();
            Response.Redirect("~/invsetup/MaterialPurchaseNoteHomeNew.aspx?PID=433");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            DiscardFalseEntities();
            pnlIGPs.Visible = true;
            Response.Redirect("~/invsetup/materialpurchasenotemgtnew.aspx?PID=434");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/MaterialPurchaseNoteHomeNew.aspx?PID=433");
        }

        #endregion

        #region Helping Methods

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
                itmData.LocId = 2;

                itmData.LotNo = lotNo;


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
                itmData.DocRef = Doc_Ref;
                itmData.PMN_Ref = Doc_RefFirst;
                itmData.PMN_RefTemp = Doc_RefFirst;
                itmData.Due_Date = Convert.ToDateTime(txtDueDate.Text);
                itmData.Pay_Status = "OP";


                int srCount = 1;
                string igpNum = "";

                if (Doc_RefFirst != "")
                {
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
                        itmDet.LocId = 2;

                        itmDet.vr_qty = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text);
                        itmDet.KGSwt = 0;
                        itmDet.vr_val = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtAmount")).Text);
                        itmDet.vr_rate = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtAmount")).Text) /
                            Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text); ;

                        igpNum = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtIGP")).Text;
                        igpNum = igpNum.Substring(0, 4) + igpNum.Substring(5);
                        itmDet.IGP_Ref = Convert.ToInt32(igpNum);

                        //itmDet.Feetage = ftg * Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text) / qtySum;
                        
                        //Lot Wise Feetage============================
                        
                        string currentDocRef = "";
                        currentDocRef = mN.GetDocRefByIGP(Convert.ToInt32(igpNum), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        decimal avgFtg = mN.GetAvgFeetage(Doc_RefFirst, currentDocRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        itmDet.Feetage = avgFtg * Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text);

                        //============================================
                        

                        

                        itmDet.vr_rmk = "";
                        itmDet.GradeId = "";
                        itmDet.LotRef = 0;
                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        itmDet.DrumNo = 0;
                        itmDetEnt.Add(itmDet);
                    }
                }


                bool res = false;
                if (Doc_RefFirst != "")
                {
                    res = mN.SaveItemData(itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                else
                {
                    res = false;
                }

                if (res == true)
                {
                    if (ddlStatus.SelectedValue != "C")
                    {

                            List<tblStkGP> recs = mN.GetRecsStkGPByStrtIGP(igpNoFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            foreach (tblStkGP rec in recs)
                            {
                                rec.PMN_RefTemp = Doc_RefFirst;
                            }
                            //update reocords    
                            mN.UpdateTblStkGP((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            

                            List<tblItemData> records = mN.GetRecsTblItemDataByStrtIGP(Doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            foreach (tblItemData rec in records)
                            {
                                rec.PMN_RefTemp = Doc_RefFirst;
                            }
                            //update reocord    
                            mN.UpdateTblItemData(records, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            
                    }
                    else
                    {
                        DiscardFalseEntities();
                    }
                    ucMessage.ShowMessage("Saved successfully", RMS.BL.Enums.MessageType.Info);
                    itmDetEnt.Clear();
                    Response.Redirect("~/invsetup/materialpurchasenotehomenew.aspx?PID=433");
                }
                else
                {
                    ucMessage.ShowMessage("Save was unsuccessful", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void Edit()
        {
            try
            {
                decimal ftg = Convert.ToDecimal(lblFTtlArea.Text);
                decimal qtySum = 0;
                string igpNum = "";

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
                itmData.LocId = 2;

                itmData.LotNo = lotNo;


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

                itmData.DocRef = Doc_RefFirst;
                itmData.PMN_Ref = Doc_RefFirst;
                itmData.PMN_RefTemp = Doc_RefFirst;
                itmData.Due_Date = Convert.ToDateTime(txtDueDate.Text);
                itmData.Pay_Status = "OP";


                int srCount = 1;
 
                if (Doc_RefFirst != "")
                {

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

                        itmDet.LocId = 2;
                        itmDet.vr_qty = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text);
                        itmDet.KGSwt = 0;
                        itmDet.vr_val = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtAmount")).Text);
                        itmDet.vr_rate = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtAmount")).Text) /
                            Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text); ;

                        igpNum = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtIGP")).Text;
                        igpNum = igpNum.Substring(0, 4) + igpNum.Substring(5);
                        itmDet.IGP_Ref = Convert.ToInt32(igpNum);
                        
                        
                        //itmDet.Feetage = ftg * Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text) / qtySum;

                        //Lot Wise Feetage============================


                        string currentDocRef = "";
                        currentDocRef = mN.GetDocRefByIGP(Convert.ToInt32(igpNum), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        decimal avgFtg = mN.GetAvgFeetage(Doc_RefFirst, currentDocRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        itmDet.Feetage = avgFtg * Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text);

                        //============================================


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
                if (Doc_RefFirst != "")
                {
                    res = mN.EditRec(sgNo, Convert.ToInt32(vrNo.Substring(0, 4)), 21, itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                else
                {
                    res = false;
                }

                if (res == true)
                {
                    if (ddlStatus.SelectedValue == "C")
                    {
                        List<tblStkGP> recs = mN.GetRecsStkGPByStrtIGP(igpNoFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        foreach (tblStkGP rec in recs)
                        {
                            rec.PMN_Ref = "";
                            rec.PMN_RefTemp = "";
                        }
                        //update reocords    
                        mN.UpdateTblStkGP((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                        List<tblItemData> records = mN.GetRecsTblItemDataByStrtIGP(Doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        foreach (tblItemData rec in records)
                        {
                            rec.PMN_Ref = "";
                            rec.PMN_RefTemp = "";
                        }
                        //update reocord    
                        mN.UpdateTblItemData(records, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        DiscardFalseEntities();
                    }

                    ucMessage.ShowMessage("Updated successfully", RMS.BL.Enums.MessageType.Info);
                    itmDetEnt.Clear();
                    Response.Redirect("~/invsetup/materialpurchasenotehomenew.aspx?PID=433");
                }
                else
                {
                    ucMessage.ShowMessage("Update was unsuccessful", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void GetMatPurchaseNo()
        {
            txtGPNo.Text = mN.GetSGNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 21, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        public void GetPurchseNoteInfo()
        {
            tblStkGP vendorInfo= mN.GetVendorInfo(ddlSelectVendor.SelectedValue, igpNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            string product = mN.GetProductByItmCode(Item_Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            txtProduct.Text = product;
            ddlVendor.SelectedValue = vendorInfo.VendorId;
            ddlCity.SelectedValue = vendorInfo.VendorCity;
        }

        public void BindGridIGPs(List<spGetIgpsDetailByVendor4MPNResult> lst)
        {
            grdIGPs.DataSource = lst;
            grdIGPs.DataBind();
        }

        public void BindGridsFtgGrdCards()
        {
            List<spGetFtgTempTableRecsResult> lstFtg = mN.GetFeetageRecords((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<spGetGrdTempTableRecsResult> lstGrd = mN.GetGradingRecords((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            foreach (var l in lstFtg)
            {
                AreaFtg = AreaFtg + Convert.ToInt32(l.area);
                QuantityFtg = QuantityFtg + Convert.ToInt32(l.quantity);
            }
            foreach (var l in lstGrd)
            {
                AreaGrd = AreaGrd + Convert.ToInt32(l.area);
                QuantityGrd = QuantityGrd + Convert.ToInt32(l.quantity);
            }
            grdFeetage.DataSource = lstFtg;
            grdFeetage.DataBind();
            grdGrading.DataSource = lstGrd;
            grdGrading.DataBind();
        }

        public void DiscardFalseEntities()
        {
            mN.DiscardFalseRecords((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        public void GetIGPsVal()
        {
             List<tblStkGP> rec = mN.GetIGPRecsByStrtIGP(igpNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
             List<tblStkGPDet> recsDet = mN.GetIGPRecsDetailByStrtIGP(igpNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]); 
            
            if (rec.Count > 0)
             {
                DateTime dtTime;
                 string docRef = "";
                 string docRefPrev = "";
                 int m = 0;
                 foreach (var r in rec)
                 {
                     docRef = r.DocRef;
                     if (docRef != docRefPrev)
                     {
                         Frieght = Frieght +Convert.ToInt32(r.Freight.Value);
                     }
                     docRefPrev = docRef;
                     if (m == 0)
                     {
                         txtIGPDate.Text = r.vr_dt.ToString();
                         CalendarExtender2.SelectedDate = r.vr_dt;
                         dtTime = r.vr_dt.AddDays(90);
                         
                         txtDueDate.Text = dtTime.Date.ToString();
                         CalendarExtender3.SelectedDate = dtTime.Date;
                     }
                     m++;
                 }
             }
            
            if (recsDet.Count > 0)
            {
                foreach (var rd in recsDet)
                {
                    Amount = Amount + Convert.ToDouble(rd.Price);
                }
            }
        }

        public void BindIGPFlds()
        {
            string igpsNo = "";
            decimal sumAmount = 0;
            decimal frt = 0;
            string igp = "";
            string docRef = "";
            string docRefPrev = "";
            int m = 0;
            List<tblStkGP> rec = mN.GetIGPRecsByStrtIGP(igpNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (rec.Count > 0)
            {
                foreach (var r in rec)
                {
                    igp = r.vr_no.ToString().Substring(0, 4) + "/" + r.vr_no.ToString().Substring(4);
                    docRef = r.DocRef;
                    if (docRef != docRefPrev)
                    {
                        frt = frt + r.Freight.Value;
                    }
                    docRefPrev = docRef;
                    if (m == 0)
                    {
                        igpsNo = igp;
                        //-------------
                        ddlVendor.SelectedValue = r.VendorId;
                        ddlCity.SelectedValue = r.VendorCity;
                        //-------------
                        txtIGPDate.Text = r.vr_dt.ToString();
                        CalendarExtender2.SelectedDate = r.vr_dt;
                    }
                    else
                    {
                        igpsNo = igpsNo + "," + igp;
                    }
                    m++;
                }
                txtFrt.Text = frt.ToString();

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    sumAmount = sumAmount + Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text); ;
                }
                txtFinalAmount.Text = sumAmount.ToString();


                DateTime dtTime = CalendarExtender2.SelectedDate.Value;
                dtTime = dtTime.AddDays(90);
                txtDueDate.Text = dtTime.Date.ToString();
                CalendarExtender3.SelectedDate = dtTime.Date;
            }
        }

        public void GetColumnsIGP()
        {
            d_tableIGP.Columns.Clear();
            d_tableIGP.Rows.Clear();

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "Sr";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "vr_no";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "GPRef";
            d_tableIGP.Columns.Add(d_ColIGP);

            //d_ColIGP = new DataColumn();
            //d_ColIGP.DataType = System.Type.GetType("System.String");
            //d_ColIGP.ColumnName = "LotNo";
            //d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "Party";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.Int32");
            d_ColIGP.ColumnName = "vr_qty";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.Decimal");
            d_ColIGP.ColumnName = "Price";
            d_tableIGP.Columns.Add(d_ColIGP);


            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.Decimal");
            d_ColIGP.ColumnName = "Amount";
            d_tableIGP.Columns.Add(d_ColIGP);

        }

        public void BindTable()
        {
            GetColumnsIGP();

            List<spGetIGPs4MPNResult> lst = mN.GetIGPByDocRef(Doc_RefFirst,"PartyS", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            foreach (var l in lst)
            {
                d_RowIGP = d_tableIGP.NewRow();
                d_RowIGP["Sr"] = l.Sr;
                d_RowIGP["vr_no"] = l.vr_no;
                d_RowIGP["GPRef"] = l.GPRef;
                d_RowIGP["Party"] = l.Party;
                d_RowIGP["vr_qty"] = l.vr_qty;
                d_RowIGP["Price"] = l.Price;
                d_RowIGP["Amount"] = l.Price;


                d_tableIGP.Rows.Add(d_RowIGP);
            }
            CurrentTable = d_tableIGP;
            BindGrid();
            //-----
            Session["getFirstDocRef"] = Doc_RefFirst;
        }

        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }

        public void BindDdlSelectItem()
        {
            ddlSelectItem.DataSource = gP.GetProduct((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSelectItem.DataTextField = "itm_dsc";
            ddlSelectItem.DataValueField = "itm_cd";
            ddlSelectItem.DataBind();
        }

        public void BindDdlSelectVendor()
        {
            ddlSelectVendor.DataSource = gP.GetSelectedVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSelectVendor.DataTextField = "gl_dsc";
            ddlSelectVendor.DataValueField = "vendorId";
            ddlSelectVendor.DataBind();
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

        public void BindIGPFldsEdit()
        {
            try
            {
                tblStkGP rec = mN.GetRecStkByIGP(igpNoFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                tblStkGPDet recDet = mN.GetRecStkDetByIGP(igpNoFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                Item_Code = recDet.Itm_cd;

                string product = mN.GetProductByItmCode(Item_Code, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                txtProduct.Text = product;
                ddlVendor.SelectedValue = rec.VendorId;
                ddlCity.SelectedValue = rec.VendorCity;
            }
            catch (Exception ex)
            {
                uMsg.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindCardFieldsEdit()
        {
            try
            {
                tblStkGP recStk = mN.GetRecStkByIGP(igpNoFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                tblItemData rec = mN.GetCardRec(mpnNo, 21, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                List<tblItemDataDet> recs = mN.GetCardRecDet(mpnNo, 21, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                
                foreach (var r in recs)
                {
                    Amount = Amount + Convert.ToDouble(r.vr_val);
                }

                txtGPNo.Text = rec.vr_no.ToString().Substring(0, 4) + "/" + rec.vr_no.ToString().Substring(4);
                txtGpDate.Text = rec.vr_dt.Date.ToString();
                CalendarExtender1.SelectedDate = rec.vr_dt.Date;
                ddlStatus.SelectedValue = rec.vr_apr.ToString();
                txtIGPDate.Text = recStk.vr_dt.Date.ToString();
                CalendarExtender2.SelectedDate = recStk.vr_dt.Date;
                txtDueDate.Text = rec.Due_Date.Value.Date.ToString();
                CalendarExtender3.SelectedDate = rec.Due_Date.Value.Date;
                txtFrt.Text = rec.Freight.Value.ToString();
                txtFinalAmount.Text = Amount.ToString();
                txtCommission.Text = rec.Commission.Value.ToString();
                txtRemarks.Text = rec.vr_nrtn;
            }
            catch (Exception ex)
            {
                uMsg.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void GetDataFtgGrdEdit()
        {
            try
            {
                mN.DeleteFtgTempTable((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                mN.DeleteGrdTempTable((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                //Insert Into FtgTempTable-------------
                mN.InsertingIntoFtgTempTable(Doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                //Insert Into GrdTempTable-------------
                mN.InsertingIntoGrdTempTable(Doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            catch (Exception ex)
            {
                uMsg.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindTableEdit()
        {
            try
            {
                 GetColumnsIGP();

                List<spGetIGPs4MPNResult> lst = mN.GetIGPByDocRef(Doc_RefFirst,"PartyS", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                List<tblItemDataDet> recs = mN.GetCardRecDet(mpnNo, 21, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);  
                
                foreach (var l in lst)
                {
                    d_RowIGP = d_tableIGP.NewRow();
                    d_RowIGP["Sr"] = l.Sr;
                    d_RowIGP["vr_no"] = l.vr_no;
                    d_RowIGP["GPRef"] = l.GPRef;
                    d_RowIGP["Party"] = l.Party;
                    d_RowIGP["vr_qty"] = l.vr_qty;
                    d_RowIGP["Price"] = l.Price;
                    foreach (tblItemDataDet det in recs)
                    {
                        if (det.IGP_Ref == Convert.ToInt32(l.vr_no.Substring(0,4)+l.vr_no.Substring(5)))
                        {
                            d_RowIGP["Amount"] = Math.Round(det.vr_val, 2);
                        }
                    }
                    
                    d_tableIGP.Rows.Add(d_RowIGP);
                }
                CurrentTable = d_tableIGP;
                BindGrid();
                //------------
                Session["getFirstDocRef"] = Doc_RefFirst;
            }
            catch (Exception ex)
            {
                uMsg.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion


    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using RMS.BL; 

namespace RMS.Inv
{
    public partial class InvCancelMPN : BasePage
    {

        #region DataMembers

        MPNBL mpnBl = new MPNBL();
        tblStkGP stkGp = new tblStkGP();
        EntitySet<tblStkDataDet> entStkDet = new EntitySet<tblStkDataDet>();

        DataTable d_tableIGP = new DataTable();
        DataRow d_RowIGP;
        DataColumn d_ColIGP;

        DataTable d_table = new DataTable();
#pragma warning disable CS0169 // The field 'InvCancelMPN.d_Row' is never used
        DataRow d_Row;
#pragma warning restore CS0169 // The field 'InvCancelMPN.d_Row' is never used
        DataColumn d_Col;
        EntitySet<tblStkCostDet> entCostDet = new EntitySet<tblStkCostDet>();

#pragma warning disable CS0169 // The field 'InvCancelMPN.glmf' is never used
        Glmf_Data glmf;
#pragma warning restore CS0169 // The field 'InvCancelMPN.glmf' is never used

        int IgpVrCode = 0;
        int MpnVrCode = 0;
  
        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        public int BrID
        {
            get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
        }

        public int locId
        {
            get { return Convert.ToInt32(ViewState["locId"]); }
            set { ViewState["locId"] = value; }
        }

        public DataTable CurrentTable
        {
            set { ViewState["CurrentTable"] = value; }
            get { return (DataTable)(ViewState["CurrentTable"] ?? null); }
        }

        public DataTable CurrentTablePurchCostSheet
        {
            set { ViewState["CurrentTablePurchCostSheet"] = value; }
            get { return (DataTable)(ViewState["CurrentTablePurchCostSheet"] ?? null); }
        }

        public string doc_RefFirst
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

        public string mpnNoFormated
        {
            get { return Convert.ToString(ViewState["mpnNoFormated"]); }
            set { ViewState["mpnNoFormated"] = value; }
        }

        public bool IsEdit
        {
            get { return Convert.ToBoolean(ViewState["IsEdit"]); }
            set { ViewState["IsEdit"] = value; }
        }

        public decimal frt
        {
            get { return Convert.ToDecimal(ViewState["frt"]); }
            set { ViewState["frt"] = value; }
        }

        public decimal WHT
        {
            get { return Convert.ToDecimal(ViewState["WHT"]); }
            set { ViewState["WHT"] = value; }
        }

        public bool saveData
        {
            get { return Convert.ToBoolean(ViewState["saveData"]); }
            set { ViewState["saveData"] = value; }
        }

        public bool editData
        {
            get { return Convert.ToBoolean(ViewState["editData"]); }
            set { ViewState["editData"] = value; }
        }

        public string POType
        {
            set { ViewState["POType"] = value; }
            get { return Convert.ToString(ViewState["POType"] ?? ""); }
        }

        public string FltParty
        {
            set { ViewState["FltParty"] = value; }
            get { return Convert.ToString(ViewState["FltParty"] ?? ""); }
        }

        public DateTime FltFrmDt
        {
            set { ViewState["FltFrmDt"] = value; }
            get { return Convert.ToDateTime(ViewState["FltFrmDt"]); }
        }

        public DateTime FltToDt
        {
            set { ViewState["FltToDt"] = value; }
            get { return Convert.ToDateTime(ViewState["FltToDt"]); }
        }

        public char FltStts
        {
            set { ViewState["FltStts"] = value; }
            get { return Convert.ToChar(ViewState["FltStts"]); }
        }

        //public decimal ChargedCost
        //{
        //    get { return Convert.ToDecimal(ViewState["ChargedCost"] ?? 0); }
        //    set { ViewState["ChargedCost"] = value; }
        //}
        
        //public decimal NotCharged
        //{
        //    get { return Convert.ToDecimal(ViewState["NotCharged"] ?? 0); }
        //    set { ViewState["NotCharged"] = value; }
        //}

        public decimal TotalAmnt
        {
            get { return Convert.ToDecimal(ViewState["TotalAmnt"] ?? 0); }
            set { ViewState["TotalAmnt"] = value; }
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


            IgpVrCode = 12;
            MpnVrCode = 21;
            //locId = 5;
            mpnBl.DiscardFalseRecords(IgpVrCode, locId, 'C', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (Session["BranchID"] == null)
            {
                BrID = Convert.ToByte(Request.Cookies["uzr"]["BranchID"]);
            }
            else
            {
                BrID = Convert.ToByte(Session["BranchID"].ToString());
            }

            if (!IsPostBack)
            {
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender3.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender4.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender5.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender7.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                    CalendarExtender2.Format = Session["DateFormat"].ToString();
                    CalendarExtender3.Format = Session["DateFormat"].ToString();
                    CalendarExtender4.Format = Session["DateFormat"].ToString();
                    CalendarExtender5.Format = Session["DateFormat"].ToString();
                    CalendarExtender7.Format = Session["DateFormat"].ToString();
                }

               
                PID = Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 562)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "CancelMPN").ToString();
                    ClearSession();
                    string dt = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month.ToString() + "-01-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString();
                    CalendarExtender4.SelectedDate = Convert.ToDateTime(dt);
                    txtFltFrmDt.Text = Convert.ToDateTime(dt).
                        ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                    CalendarExtender5.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    txtFltToDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date.
                        ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                    FltParty = txtFltParty.Text;
                    FltFrmDt = Convert.ToDateTime(txtFltFrmDt.Text);
                    FltToDt = Convert.ToDateTime(txtFltToDt.Text);
                    FltStts = Convert.ToChar( ddlFltType.SelectedValue);

                    //lnkCostSht.Attributes.Add("onclick", "window.open('../Inv/PurchCostMgt.aspx?PID=514','Purchase_Cost_Sheet','height=400,width=710,toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=center, directories=no, status=no');return false");
                    BindMPNGrid();
                    FillShowVendor();
                    FillDropDownLoc();
                    FillDropDownCity();
                    FillDdlForwarder();

                    saveData = false;
                    editData = false;
                    
                    pnlFields.Style.Add("Display", "none");
                    btnClear.Visible = false;
                    btnCancel.Visible = false;
                    btnClear.Visible = false;

                    txtMPNDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date.ToString();
                    CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;

                    igpNoFirst = 0;
                    doc_RefFirst = "";
                    POType = "F";
                   
                }
                
            }
            BindMPNGrid();
            
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlUOM");
                BindDdlUOM(ddl);
                DropDownList ddlGST = (DropDownList)e.Row.FindControl("ddlGST");
                FillGridDropDownGST(ddlGST);

                if (((TextBox)e.Row.FindControl("txtPoRef")).Text.Trim() != "9999/9")
                {
                    string potyp = new POrderBL().GetPOType(BrID, 
                                       Convert.ToInt32(((TextBox)e.Row.FindControl("txtPoRef")).Text.Trim().Replace("/","")),
                                       (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (potyp == "L")
                    {
                        //((TextBox)e.Row.FindControl("txtAmount")).ReadOnly = true;
                        //((TextBox)e.Row.FindControl("txtAmount")).TabIndex = -1;

                        ((TextBox)e.Row.FindControl("txtCustomDuty")).Enabled = false;
                        ((TextBox)e.Row.FindControl("txtCustomDuty")).TabIndex = -1;
                    }
                    
                    ddlGST.Enabled = false;
                }

                if (Convert.ToDecimal(((TextBox)e.Row.FindControl("txtRecQty")).Text.Trim()) < 1)
                {
                    ((TextBox)e.Row.FindControl("txtAmount")).ReadOnly = true;
                    ((TextBox)e.Row.FindControl("txtAmount")).TabIndex = -1;
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Save();            
        }
        protected void GrdIGPs_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex+1).ToString();
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, 4) + "/" + e.Row.Cells[1].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString(Session["DateFormat"].ToString());
                }
            }
        }
        protected void grdMPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[0].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text).ToString(Session["DateFormat"].ToString());
                }
                e.Row.Cells[2].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[2].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString(Session["DateFormat"].ToString());
                }
                //if (e.Row.Cells[6].Text == "Approved")
                //{
                //}
                if (e.Row.Cells[6].Text == "Cancelled")
                {
                     e.Row.Cells[7].Text = "";
                }
             }
        }
        protected void grdMPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearSession();
            ClearAllPurchCostSheet();

            if (grdMPN.Rows[grdMPN.SelectedIndex].Cells[6].Text == "Approved")
            {
                pnlFields.Enabled = false;
                pnlIGPs.Enabled = false;
            }
            else
            {
                pnlFields.Enabled = true;
                pnlIGPs.Enabled = true;
            }
            btnCancel.Visible = true;
            btnClear.Visible = true;
            editData = true;
            pnlFields.Style.Add("Display", "true");
            pnlIGPs.Visible = true;
            lblIGPsDet.Visible = true;
            divCostSht.Style.Add("Display", "false");
            div1.Style.Add("Display", "false");
            editData = true;

            locId = Convert.ToInt32(grdMPN.SelectedDataKey.Value);
            mpnNo = Convert.ToInt32(grdMPN.Rows[grdMPN.SelectedIndex].Cells[0].Text.Substring(0, 4) + grdMPN.Rows[grdMPN.SelectedIndex].Cells[0].Text.Substring(5));
            mpnNoFormated = grdMPN.Rows[grdMPN.SelectedIndex].Cells[0].Text.Trim();
            IsEdit = true;

            BindFieldsEdit();
            BindTableEdit();
            //BindTablePurchCostSheet();

            /*GET PO TYPE************************************/
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim() != "9999/9")
                {
                    POType = new POrderBL().GetPOType(BrID,
                          Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim().Replace("/", "")),
                          (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (POType == "L" || POType == "F")
                    {
                        break;
                    }
                }
                else
                {
                    POType = "L";
                    break;
                }
            }
            txtPOType.Text = POType == "L" ? "Local" : "Foreign";
            if (POType == "L")
            {
                txtImpFrt.Enabled = false;
                ddlForwarder.Enabled = false;
            }
            else
            {
                txtImpFrt.Enabled = true;
                ddlForwarder.Enabled = true;
            }
            /*END GET PO TYPE********************************/
        }
        protected void grdMPN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMPN.PageIndex = e.NewPageIndex;
            BindMPNGrid();
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            ClearSession();
            FltParty = txtFltParty.Text;
            FltFrmDt = Convert.ToDateTime(txtFltFrmDt.Text); ;
            FltToDt = Convert.ToDateTime(txtFltToDt.Text) ;
            FltStts = Convert.ToChar(ddlFltType.SelectedValue);
            BindMPNGrid();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            mpnBl.DiscardFalseRecords(IgpVrCode, locId, 'C', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            pnlFields.Style.Add("Display", "none");
            pnlIGPs.Visible = false;
            saveData = false;
            editData = false;
            ClearSession();

            Response.Redirect("~/Inv/InvCancelMPN.aspx?PID=562");
        }
      
        #endregion

        #region PurchaseCostSheet

        protected void lnkPurchCostSheet_Click(object sender, EventArgs e)
        {
            MPE.Show();
        }
        protected void btnSavePurchCostSheet_Click(object sender, EventArgs e)
        {
            //SaveCost();
        }
        protected void popupGrd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CalendarExtender Cal = (CalendarExtender)e.Row.FindControl("C2");
                if (Session["DateFormat"] == null)
                {
                    Cal.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    Cal.Format = Session["DateFormat"].ToString();
                }
            }
        }
        public void GetColumnsPurchCostSheet()
        {
            d_table.Columns.Clear();
            d_table.Rows.Clear();

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "CostId";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "TempId";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Desc";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Date";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "DocRef";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Amount";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Remarks";
            d_table.Columns.Add(d_Col);


        }
        public bool CheckDataPurchCostSheet()
        {
            bool res = false;
            for (int i = 0; i < popupGrd.Rows.Count; i++)
            {
                TextBox txtAmnt = (TextBox)(popupGrd.Rows[i].FindControl("txtAmount"));
                if (txtAmnt.Text != "")
                {
                    if (Convert.ToDecimal(txtAmnt.Text) > 0)
                    {
                        res = true;
                        break;
                    }
                }
            }
            return res;
        }
        //public void BindTablePurchCostSheet()
        //{
        //    GetColumnsPurchCostSheet();
        //    int brid = 0;
        //    if (Session["BranchID"] == null)
        //    {
        //        brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
        //    }
        //    else
        //    {
        //        brid = Convert.ToInt32(Session["BranchID"].ToString());
        //    }
        //    string potyp = mpnBl.GetPOType(brid, Convert.ToInt32(Session["POREF"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    /****************************************************************/
        //    List<tblStkCostDet> lstStk = mpnBl.GetPreviousRec(mpnNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    List<tblCost_TempDet> lst = mpnBl.GetCostIdDet(potyp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


        //    if (lstStk != null && lstStk.Count > 0)
        //    {
        //        foreach (var l in lst)
        //        {
        //            d_Row = d_table.NewRow();
        //            bool found = false;
        //            for (int j = 0; j < lstStk.Count; j++)
        //            {
        //                if (l.CostId == lstStk[j].CostId)
        //                {
        //                    d_Row["CostId"] = l.CostId;
        //                    d_Row["TempId"] = l.TempId;
        //                    d_Row["Desc"] = l.Cost_Desc;
        //                    if (lstStk[j].DocRef_Date != null)
        //                    {
        //                        if (Session["DateFormat"] == null)
        //                        {
        //                            d_Row["Date"] = lstStk[j].DocRef_Date.Value.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
        //                        }
        //                        else
        //                        {
        //                            d_Row["Date"] = lstStk[j].DocRef_Date.Value.ToString(Session["DateFormat"].ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        d_Row["Date"] = "";
        //                    }
        //                    d_Row["DocRef"] = lstStk[j].DocRef;
        //                    d_Row["Amount"] = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(lstStk[j].Paid_Amt.Value)));
        //                    d_Row["Remarks"] = lstStk[j].vr_rmk;
        //                    found = true;
        //                }
        //                else if (found == false)
        //                {
        //                    d_Row["CostId"] = l.CostId;
        //                    d_Row["TempId"] = l.TempId;
        //                    d_Row["Desc"] = l.Cost_Desc;
        //                    d_Row["Date"] = "";
        //                    d_Row["DocRef"] = "";
        //                    d_Row["Amount"] = "";
        //                    d_Row["Remarks"] = "";
        //                }
        //                else
        //                {
        //                }
        //            }
        //            d_table.Rows.Add(d_Row);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var l in lst)
        //        {
        //            d_Row = d_table.NewRow();

        //            d_Row["CostId"] = l.CostId;
        //            d_Row["TempId"] = l.TempId;
        //            d_Row["Desc"] = l.Cost_Desc;
        //            d_Row["Date"] = "";
        //            d_Row["DocRef"] = "";
        //            d_Row["Amount"] = "";
        //            d_Row["Remarks"] = "";

        //            d_table.Rows.Add(d_Row);
        //        }
        //    }
        //    CurrentTablePurchCostSheet = d_table;
        //    BindGridPurchCostSheet();
        //}
        public void BindGridPurchCostSheet()
        {
            popupGrd.DataSource = CurrentTablePurchCostSheet;
            popupGrd.DataBind();
        }
        //public void SaveCost()
        //{
        //    try
        //    {
        //        ChargedCost = 0;
        //        string stts = "";
        //        int count = 1;
        //        //tblStkCost=====================================
        //        tblStkCost cost = new tblStkCost();
        //        cost.vr_id = 0;
        //        cost.vr_dt = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
        //        cost.DocRef = doc_RefFirst;
        //        cost.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
        //        if (Session["LoginID"] == null)
        //        {
        //            if (Request.Cookies["uzr"] != null)
        //            {
        //                cost.updateby = Request.Cookies["uzr"]["LoginID"];
        //            }
        //        }
        //        else
        //        {
        //            cost.updateby = Session["LoginID"].ToString();
        //        }
        //        cost.post_2gl = true;

        //        //tblStkCost_Det===================================

        //        for (int i = 0; i < popupGrd.Rows.Count; i++)
        //        {
        //            TextBox txtAmnt = (TextBox)(popupGrd.Rows[i].FindControl("txtAmount"));
        //            if (txtAmnt.Text != "")
        //            {
        //                if (Convert.ToDecimal(txtAmnt.Text) > 0)
        //                {
        //                    tblStkCostDet costDet = new tblStkCostDet();
        //                    costDet.vr_id = 0;
        //                    costDet.vr_seq = Convert.ToByte(count);

        //                    costDet.CostId = Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtCostId"))).Text);

        //                    costDet.DocRef = ((TextBox)(popupGrd.Rows[i].FindControl("txtDocRef"))).Text;
        //                    try
        //                    {
        //                        if (((TextBox)(popupGrd.Rows[i].FindControl("txtDate"))).Text != "")
        //                        {
        //                            costDet.DocRef_Date = Convert.ToDateTime(((TextBox)(popupGrd.Rows[i].FindControl("txtDate"))).Text);
        //                            costDet.Pay_Date = Convert.ToDateTime(((TextBox)(popupGrd.Rows[i].FindControl("txtDate"))).Text);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        ucMsgCS.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
        //                    }
        //                    costDet.Paid_Amt = Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtAmount"))).Text);
        //                    costDet.Claim_Amt = Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtAmount"))).Text);
        //                    costDet.vr_rmk = ((TextBox)(popupGrd.Rows[i].FindControl("txtRem"))).Text;


        //                    //Getting cost when column 'Charg_Cost' in tblCost_TempDet is 'Y'
        //                    stts = mpnBl.GetChargeCostStts(Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtCostId"))).Text), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //                    if (stts != "-")
        //                    {
        //                        if (stts == "Y")
        //                        {
        //                            ChargedCost = ChargedCost + Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtAmount"))).Text);
        //                        }
        //                        else
        //                        {
        //                            NotCharged = NotCharged + Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtAmount"))).Text);
        //                        }
        //                    }

        //                    entCostDet.Add(costDet);
        //                    count++;
        //                }
        //            }
        //        }
        //        //To Save or Put In Session=================================
        //        if (entCostDet.Count > 0)
        //        {
        //            Session["StkCost"] = cost;
        //            Session["StkCostDet"] = entCostDet;
        //            Session["ChargedCost"] = ChargedCost;
        //            Session["NotCharged"] = NotCharged;

        //            //ClearAllPurchCostSheet();

        //            //=================================================

        //            //Result = mpnBl.SaveCostSheet(cost, entCostDet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            //if (Result == true)
        //            //{
        //            //    Result = false;
        //            //    ClearAll();
        //            //    ucMsgCS.ShowMessage("Saved successfully.", RMS.BL.Enums.MessageType.Info);
        //            //}
        //            //else
        //            //{
        //            //    ucMsgCS.ShowMessage("Canot save at this time.", RMS.BL.Enums.MessageType.Error);
        //            //}
        //        }
        //        else
        //        {
        //            ucMsgCS.ShowMessage("Cannot add, internal error occured.", RMS.BL.Enums.MessageType.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ucMsgCS.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
        //    }
        //}
        public void ClearAllPurchCostSheet()
        {
            //ChargedCost = 0;
            //NotCharged = 0;
            
   
            MPE.Dispose();
            CurrentTablePurchCostSheet = null;
            BindGridPurchCostSheet();

        }

        #endregion

        #region Helping Methods

        public void ClearSession()
        {
            Session["VrNo"] = null;
            Session["POREF"] = null;
            Session["VendorId"] = null;
            Session["DocRef"] = null;
            Session["StkCost"] = null;
            Session["StkCostDet"] = null;
            Session["MPNStatus"] = null;
        }
        public void ClearFeilds()
        {
            pnlFields.Style.Add("Display", "none");
            pnlIGPs.Visible = false;
            lblIGPsDet.Visible = false;
            divCostSht.Style.Add("Display", "none");
            div1.Style.Add("Display", "none");
            btnCancel.Visible = false;
            btnClear.Visible = false;
            pnlFieldsTotal.Visible = false;

            CurrentTable.Columns.Clear();
            CurrentTable.Rows.Clear();
            BindGrid();
            BindMPNGrid();
            saveData = false;
            editData = false;
            txtCommission.Text = "";
            txtRemarks.Text = "";
            ddlStatus.SelectedValue = "P";
            txtMPNNo.Text = "";

            txtGrAmount.Text = "";
            txtDisc.Text = "";
            txtWHT.Text = "";
            txtFrt.Text = "";
            txtImpFrt.Text = "";
            ddlForwarder.SelectedValue = "0";
            txtImpFrt.Enabled = false;
            ddlForwarder.Enabled = false;
            txtAdv.Text = "";
            txtDue.Text = "";
            txtInvNo.Text = "";
            txtInvDate.Text = "";

            TotalAmnt = 0;
            igpNoFirst = 0;
            mpnNo = 0;

            ClearSession();
            ClearAllPurchCostSheet();
            txtOtrCost.Text = "";
        }
        public void ChangePMNStatus(string docRef)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                int igp = Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtIGP")).Text.Substring(0, 4) + ((TextBox)GridView1.Rows[i].FindControl("txtIGP")).Text.Substring(5));
                tblStkGP record = mpnBl.GetRecord(BrID, igp, IgpVrCode, locId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (record != null)
                {
                    record.PMN_Ref = docRef;
                    record.PMN_RefTemp = docRef;
                }
                mpnBl.UpdateRecord((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
        }
        public void GetMatPurchaseNo()
        {
            mpnNoFormated = mpnBl.GetMPNNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            mpnNo = Convert.ToInt32(mpnNoFormated.Substring(0, 4) + mpnNoFormated.Substring(5));
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
            d_ColIGP.ColumnName = "PO_Ref";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "ItmCd";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "ItmDsc";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "UOM";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.Decimal");
            d_ColIGP.ColumnName = "IGPQty";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.Decimal");
            d_ColIGP.ColumnName = "RecQty";
            d_tableIGP.Columns.Add(d_ColIGP);

           
            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "Date";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "City";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.Decimal");
            d_ColIGP.ColumnName = "Amount";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "GST";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "GSTAmount";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "CustomDuty";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "matrec_id";
            d_tableIGP.Columns.Add(d_ColIGP);

        }
        public void BindTable()
        {
            GetColumnsIGP();
            int counter=1;
            decimal discount = 0;
            List<spChemIgps4MPNResult> lst = mpnBl.GetIGPs(locId, IgpVrCode, doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            foreach (var l in lst)
            {
                d_RowIGP = d_tableIGP.NewRow();
                d_RowIGP["Sr"] = counter++;
                d_RowIGP["vr_no"] = l.igpno;
                d_RowIGP["PO_Ref"] = l.po.Value.ToString().Substring(0,4) + "/" + l.po.Value.ToString().Substring(4);
                d_RowIGP["ItmCd"] = l.itm_cd;
                d_RowIGP["ItmDsc"] = l.itm_dsc;
                d_RowIGP["UOM"] = l.vr_uom;
                d_RowIGP["IGPQty"] = l.igpqty;
                d_RowIGP["RecQty"] = l.recqty;
                d_RowIGP["Date"] = l.vrdate;

                string potyp = "F";
                if (l.po.Value.ToString() != "99999")
                {
                    potyp = new POrderBL().GetPOType(BrID,
                                       Convert.ToInt32(l.po.Value),
                                       (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    discount = discount + new POrderBL().GetPODiscountOnItem(BrID,
                                       Convert.ToInt32(l.po.Value), l.itm_cd,
                                       (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                  
                }

                if (potyp == "L")
                {
                    d_RowIGP["Amount"] = Math.Round(Convert.ToDecimal(l.recqty) * l.vr_rate, 2);
                }
                else
                {
                    d_RowIGP["Amount"] = 0;
                }
                if (l.po.Value != 99999)
                {
                    string gstId = new TaxBL().GetGSTIdByPORefItemCode(BrID, l.po.Value, l.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    decimal gstPercent = new TaxBL().GetGSTPercentByPORefItemCode(BrID, l.po.Value, l.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    d_RowIGP["GST"] = gstId;
                    if (potyp == "L")
                    {
                        d_RowIGP["GSTAmount"] = Math.Round(Convert.ToDecimal(l.recqty) * l.vr_rate * gstPercent / 100, 0);
                    }
                    else
                    {
                        d_RowIGP["GSTAmount"] = 0;
                    }
                }
                else
                {
                    d_RowIGP["GST"] = "0";
                    d_RowIGP["GSTAmount"] = "0";
                }
                d_RowIGP["matrec_id"] = l.matrec_id;
                if (l.cust_duty != null)
                d_RowIGP["CustomDuty"] = l.cust_duty;
                else
                    d_RowIGP["CustomDuty"] = "0";

                d_tableIGP.Rows.Add(d_RowIGP);
            }
            txtDisc.Text = Math.Round(discount,0).ToString();
            CurrentTable = d_tableIGP;
            BindGrid();
            SetDropDownList();
        }
        public decimal GetWHT()
        {
            int brid = 0, poref = 0;
            decimal[,] arr = new decimal[GridView1.Rows.Count, 2];
            decimal amount =0, wht=0, discount =0;
            bool isInserted = false;
            string PO_Type = "F";
            if (Session["BranchID"] == null)
            {
                brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
            }
            else
            {
                brid = Convert.ToInt32(Session["BranchID"].ToString());
            }
            //Initialize
            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    arr[j, k] = 0;
                }
            }

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                isInserted = false;
                poref = Convert.ToInt32(((TextBox)(GridView1.Rows[i].FindControl("txtPoRef"))).Text.Trim().Replace("/",""));
                amount = Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtAmount"))).Text.Trim());
                if (poref != 99999)
                {
                    PO_Type = new POrderBL().GetPOType(brid, poref, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    discount = new POrderBL().GetPODiscountOnItem(brid,
                                       poref, ((TextBox)(GridView1.Rows[i].FindControl("txtItemCode"))).Text.Trim(),
                                       (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    amount = amount - discount;
                    if (PO_Type == "L")
                    {
                        for (int j = 0; j < GridView1.Rows.Count; j++)
                        {
                            if (arr[j, 0] == poref)
                            {
                                isInserted = true;
                                arr[j, 1] = arr[j, 1] + amount ;
                            }
                        }
                        if (!isInserted)
                        {
                            for (int l = 0; l < GridView1.Rows.Count; l++)
                            {
                                if (arr[l, 0] == 0)
                                {
                                    arr[l, 0] = poref;
                                    arr[l, 1] = amount;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            decimal rt = 0;
            for (int m = 0; m < GridView1.Rows.Count; m++)
            {
                if (arr[m, 0] != 0)
                {
                    rt = new TaxBL().GetWHTByPoRef(brid, Convert.ToInt32(arr[m, 0]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //wht = wht + arr[m, 1]/100 * rt;
                    wht = wht + (arr[m, 1] / (1 - rt / 100)) - arr[m, 1];
                }
            }
            return wht;
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }
        public void BindMPNGrid()
        {
            grdMPN.DataSource = mpnBl.GetMPNGrid(FltFrmDt, FltToDt, FltParty, FltStts, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdMPN.DataBind();
        }
        public void BindFieldsEdit()
        {
            pnlFieldsTotal.Visible = true;

            List<tblStkData> lst = mpnBl.GetRecordList(mpnNo, MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            igpNoFirst = Convert.ToInt32(lst[0].IGPNo);
            doc_RefFirst = lst[0].DocRef;


            txtMPNNo.Text = mpnNoFormated;
            txtMPNDate.Text = lst[0].vr_dt.Date.ToString();
            CalendarExtender1.SelectedDate = lst[0].vr_dt.Date;
            ddlStatus.SelectedValue = lst[0].vr_apr.ToString();
            txtIGPDate.Text = mpnBl.GetIGPById(Convert.ToInt32(lst[0].IGPNo), IgpVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).vr_dt.Date.ToString();
            CalendarExtender2.SelectedDate = mpnBl.GetIGPById(Convert.ToInt32(lst[0].IGPNo), IgpVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).vr_dt.Date;
            txtDueDate.Text = lst[0].Due_Date.ToString();
            txtWHT.Text = Math.Round( lst[0].Tax.Value,0).ToString();
            CalendarExtender3.SelectedDate = lst[0].Due_Date;
            txtFrt.Text = lst[0].Freight.ToString();
            txtImpFrt.Text = lst[0].ImpFreight.ToString();
            if (!string.IsNullOrEmpty(lst[0].Forwarder))
                ddlForwarder.SelectedValue = lst[0].Forwarder;
            else
                ddlForwarder.SelectedValue = "0";
            txtCommission.Text = lst[0].Commission.ToString();
            txtRemarks.Text = lst[0].vr_nrtn;
            txtInvNo.Text = lst[0].InvNo;
            if (lst[0].InvDate != null)
            {
                if (Session["DateFormat"] == null)
                {
                    txtInvDate.Text = lst[0].InvDate.Value.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    txtInvDate.Text = lst[0].InvDate.Value.ToString(Session["DateFormat"].ToString());
                }
            }
            //For Purchase cost sheet

            Session["DocRef"] = doc_RefFirst;
            Session["VrNo"] = mpnNoFormated;
            //if (Session["DateFormat"] == null)
            //{
            //    Session["VrDate"] = Convert.ToDateTime(mpnNoFormated).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
            //}
            //else
            //{
            //    Session["VrDate"] = Convert.ToDateTime(txtMPNDate.Text).ToString(Session["DateFormat"].ToString());
            //}
            //Session["VrDate"] = txtMPNDate.Text.Trim();
            tblStkGP gpRec =  mpnBl.GetIGPById(Convert.ToInt32(lst[0].IGPNo), 12, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            Session["VendorId"] = gpRec.VendorId;
            Session["MPNStatus"] = lst[0].vr_apr.ToString();
            
            ddlShowVendor.SelectedValue = gpRec.VendorId;
            ddlShowCity.SelectedValue = gpRec.VendorCity;
            ddlShowLoc.SelectedValue = gpRec.LocId.ToString();
        }
        public void BindTableEdit()
        {
            int count=0;
            int gpcount=0;
            decimal discount = 0;
            GetColumnsIGP();
            List<spGetGpByPmnRefResult> gpLst = mpnBl.GetGPRecords(doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblStkData> lst = mpnBl.GetRecordList(mpnNo, MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblStkDataDet> lstDet = mpnBl.GetEditTableList(mpnNo, MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            Session["POREF"] = lstDet.First().PO_Ref;
            /*GET PO TYPE************************************/

            if (lstDet.First().PO_Ref != 99999)
            {
                POType = new POrderBL().GetPOType(BrID, Convert.ToInt32(lstDet.First().PO_Ref), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            else
            {
                POType = "L";
            }
            txtPOType.Text = POType == "L" ? "Local" : "Foreign";
            /*END GET PO TYPE********************************/
            foreach (var l in lstDet)
            {
                d_RowIGP = d_tableIGP.NewRow();
                d_RowIGP["Sr"] = l.vr_seq;
                d_RowIGP["vr_no"] = gpLst[gpcount].vr_no.ToString().Substring(0, 4) + "/" + gpLst[gpcount].vr_no.ToString().Substring(4);
                d_RowIGP["PO_Ref"] = l.PO_Ref.Value.ToString().Substring(0, 4) + "/" + l.PO_Ref.Value.ToString().Substring(4);
                d_RowIGP["ItmCd"] = l.itm_cd;
                d_RowIGP["UOM"] = l.vr_uom;
                d_RowIGP["ItmDsc"] = mpnBl.GetItemDesc(l.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                d_RowIGP["IGPQty"] = (l.vr_qty);
                d_RowIGP["RecQty"] = (l.vr_qty- l.vr_qty_Rej);
                if (Session["DateFormat"] == null)
                {
                    d_RowIGP["Date"]= lst[count].vr_dt.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    d_RowIGP["Date"] = lst[count].vr_dt.ToString(Session["DateFormat"].ToString());
                }
                d_RowIGP["Amount"] = Math.Round(l.vr_val + Convert.ToDecimal(l.overall_disc), 2);
                string gstId = "0";
                if (l.TaxID == null)
                    gstId = "0";
                else
                    gstId = l.TaxID;
                if (l.PO_Ref.Value != 99999)
                {
                    decimal gstPercent = new TaxBL().GetGSTPercentByPORefItemCode(BrID, l.PO_Ref.Value, l.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    d_RowIGP["GST"] = gstId;
                    //d_RowIGP["GSTAmount"] = Math.Round(l.GSTamt.Value, 0);
                }
                else
                {
                    d_RowIGP["GST"] = gstId;
                    //d_RowIGP["GSTAmount"] = Math.Round(l.GSTamt.Value, 0);
                }
                if (l.cust_duty != null)
                {
                    d_RowIGP["CustomDuty"] = l.cust_duty;
                }
                else
                {
                    d_RowIGP["CustomDuty"] = "0";
                }
                if (gstId == "0")
                {
                    d_RowIGP["GSTAmount"] = "0";
                }
                else
                {
                    if (gstId != "VAR")
                        d_RowIGP["GSTAmount"] = Math.Round((l.vr_val + Convert.ToDecimal(l.overall_disc)) / 100 * new TaxBL().GetPercentValByTaxID(gstId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]), 0);
                    else
                        d_RowIGP["GSTAmount"] = l.GSTamt;
                }
                d_RowIGP["matrec_id"] = l.matrec_id;
                discount = discount + Convert.ToDecimal(l.overall_disc);


                d_tableIGP.Rows.Add(d_RowIGP);
                gpcount++;
            }
            txtDisc.Text = Math.Round(discount,0).ToString();
            CurrentTable = d_tableIGP;
            BindGrid();
            SetDropDownList();
        }
        public void SetDropDownList()
        {
            int rowsCount = CurrentTable.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                DropDownList dduom = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlUOM"));
                DropDownList ddgst = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlGST"));
                if (i < CurrentTable.Rows.Count)
                {
                    if (CurrentTable.Rows[i]["UOM"] != DBNull.Value)
                    {
                        dduom.ClearSelection();
                        dduom.Items.FindByValue(CurrentTable.Rows[i]["UOM"].ToString()).Selected = true;
                    }
                    if (CurrentTable.Rows[i]["GST"] != DBNull.Value)
                    {
                        ddgst.ClearSelection();
                        ddgst.Items.FindByValue(CurrentTable.Rows[i]["GST"].ToString()).Selected = true;
                    }
                }
            }
        }
        public void BindDdlUOM(DropDownList ddlUOM)
        {
            ddlUOM.DataSource = mpnBl.GetUOM((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlUOM.DataTextField = "uom_dsc";
            ddlUOM.DataValueField = "uom_cd";
            ddlUOM.DataBind();
        }
        public void FillShowVendor()
        {
            ddlShowVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlShowVendor.DataTextField = "gl_dsc";
            ddlShowVendor.DataValueField = "gl_cd";
            ddlShowVendor.DataBind();
        }
        public void FillDdlForwarder()
        {
            ddlForwarder.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlForwarder.DataTextField = "gl_dsc";
            ddlForwarder.DataValueField = "gl_cd";
            ddlForwarder.DataBind();
        }
        public void FillDropDownCity()
        {
            ddlShowCity.DataSource = mpnBl.GetCity((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlShowCity.DataTextField = "CityName";
            ddlShowCity.DataValueField = "CityID";
            ddlShowCity.DataBind();
        }
        public void FillDropDownLoc()
        {
            ddlShowLoc.DataSource = mpnBl.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlShowLoc.DataValueField = "LocId";
            ddlShowLoc.DataTextField = "LocName";
            ddlShowLoc.DataBind();
        }
        public void FillGridDropDownGST(DropDownList ddlGST)
        {
            ddlGST.DataSource = new TaxBL().GetGSTTaxes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlGST.DataTextField = "TaxDesc";
            ddlGST.DataValueField = "TaxID";
            ddlGST.DataBind();
        }
        public void Save()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            try
            {
                string result = "";
                if ( GridView1.Rows.Count > 0)
                {
                    result = mpnBl.CancelMPN(BrID, MpnVrCode, mpnNo, POType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (result == "Done")
                    {
                        ClearFeilds();
                        uMsg.ShowMessage("MPN No " + mpnNoFormated + " cancelled successfully.", RMS.BL.Enums.MessageType.Info);
                        mpnNoFormated = "";
                    }
                    else
                    {
                        uMsg.ShowMessage("Exception:" + result, RMS.BL.Enums.MessageType.Error);
                    }

                }
            }
            catch (Exception ex)
            {
                uMsg.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion

        #region WebMethods

        [WebMethod]
        public static object GetGSTPercent(string gstId)
        {
            return new TaxBL().GetPercentByTaxID(gstId, null);
        }

        #endregion
    }
}
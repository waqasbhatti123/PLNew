using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data;
using System.Data.Linq;
using System.Collections.Generic;

namespace RMS.Inv
{
    public partial class StoreRetMgt : BasePage
    {

        #region DataMembers
        //InvGP_BL gP = new InvGP_BL();
        StoreRetBL stRetBL = new StoreRetBL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        short VoucherTypeCode = 18; // voucher type code 18 for Sotre Return Note in Vr_Type table

#pragma warning disable CS0169 // The field 'StoreRetMgt.cc' is never used
#pragma warning disable CS0169 // The field 'StoreRetMgt.QtyIss' is never used
#pragma warning disable CS0169 // The field 'StoreRetMgt.QtyRet' is never used
#pragma warning disable CS0169 // The field 'StoreRetMgt.uomitem' is never used
#pragma warning disable CS0169 // The field 'StoreRetMgt.rem' is never used
#pragma warning disable CS0169 // The field 'StoreRetMgt.itemCode' is never used
#pragma warning disable CS0169 // The field 'StoreRetMgt.srNo' is never used
        string srNo, itemCode, uomitem, QtyRet, QtyIss, cc, rem;
#pragma warning restore CS0169 // The field 'StoreRetMgt.srNo' is never used
#pragma warning restore CS0169 // The field 'StoreRetMgt.itemCode' is never used
#pragma warning restore CS0169 // The field 'StoreRetMgt.rem' is never used
#pragma warning restore CS0169 // The field 'StoreRetMgt.uomitem' is never used
#pragma warning restore CS0169 // The field 'StoreRetMgt.QtyRet' is never used
#pragma warning restore CS0169 // The field 'StoreRetMgt.QtyIss' is never used
#pragma warning restore CS0169 // The field 'StoreRetMgt.cc' is never used
        bool validatorFlag = false;
        #endregion

        #region Properties

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
        public int VrIDIss
        {
            get { return (ViewState["VrIDIss"] == null) ? 0 : Convert.ToInt32(ViewState["VrIDIss"]); }
            set { ViewState["VrIDIss"] = value; }
        }
        

        public int VrID
        {
            get { return (ViewState["VrID"] == null) ? 0 : Convert.ToInt32(ViewState["VrID"]); }
            set { ViewState["VrID"] = value; }
        }

        public int DocNo
        {
            get { return (ViewState["DocNo"] == null) ? 0 : Convert.ToInt32(ViewState["DocNo"]); }
            set { ViewState["DocNo"] = value; }
        }

        public string DocNoFormated
        {
            get { return Convert.ToString(ViewState["DocNoFormated"]); }
            set { ViewState["DocNoFormated"] = value; }
        }

        public bool IsEdit
        {
            get { return Convert.ToBoolean(ViewState["IsEdit"]); }
            set { ViewState["IsEdit"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "StoreRetMgt").ToString();
                if (Session["DateFormat"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                }
                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;

                
                FillDropDownLoc();
                FillDropDownDepartment();

                IsEdit = false;
                //GetGatePassNo();
                //GetDocRef();

                BindGridMain("","","0");

                txtDocNoIss.Focus();

            }
        }
        protected void grdStRet_SelectedIndexChanged(object sender, EventArgs e)
        {
            VrID = Convert.ToInt32(grdStRet.SelectedDataKey.Values["vr_id"]);
            validatorFlag = true;


            IsEdit = true;


            GetByID();

            btnSrchIGP.Visible = false;
            txtDocNoIss.Enabled = false;

            if (ddlStatus.SelectedValue != "P")
            {
                pnlMain.Enabled = false;
                GridView1.Enabled = false;
            }

        }
        protected void grdIssDocNos_SelectedIndexChanged(object sender, EventArgs e)
        {
            VrIDIss = Convert.ToInt32(grdIssDocNos.SelectedDataKey.Values["vr_id"]);

            GetByIDSrch();

            divGrid.Visible = true;
            grdIssDocNos.Visible = false;

            validatorFlag = false;
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
            d_Col.ColumnName = "Item";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "ItemCode";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UOMItem";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UOMItemCode";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "QtyRet";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "QtyIss";
            d_table.Columns.Add(d_Col);

            //d_Col = new DataColumn();
            //d_Col.DataType = System.Type.GetType("System.String");
            //d_Col.ColumnName = "QtyIssBalance";
            //d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "CostCenter";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "CostCenterCode";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Rem";
            d_table.Columns.Add(d_Col);

        }
        //public void BindTable()
        //{
        //    GetColumns();
        //    for (int i = 0; i < 3; i++)
        //    {
        //        d_Row = d_table.NewRow();
        //        d_Row["Sr"] = i + 1;

        //        d_table.Rows.Add(d_Row);
        //    }
        //    CurrentTable = d_table;
        //    BindGrid();
        //}
        public void BindTableEdit(EntitySet<tblStkDataDet> dets)
        {
            GetColumns();
            int count = 0;
            decimal tot = 0;
            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Item"] = GetItemNameFromCode(dt.itm_cd);
                d_Row["ItemCode"] = dt.itm_cd;
                d_Row["QtyRet"] = "";
                d_Row["QtyIss"] = dt.vr_qty.ToString("0.000");
                //QtyIssBal = Qty issued - Qty returned
                //d_Row["QtyIssBalance"] = Convert.ToString(25);
                d_Row["UomItem"] = GetItemUOMLabelFromUOMId(dt.vr_uom);
                d_Row["UomItemCode"] = dt.vr_uom.ToString();
                d_Row["CostCenter"] = GetCostCenterFromCode(dt.CC_cd);
                d_Row["CostCenterCode"] = dt.CC_cd;
                d_Row["Rem"] = "";
                count++;
                d_table.Rows.Add(d_Row);

                tot = tot + dt.vr_qty;
            }

            CurrentTable = d_table;
            BindGrid();

            //SetDropDownList();
        }
        private string GetItemUOMLabelFromUOMId(byte uomid)
        {
            string uomDesc = "";
            uomDesc = stRetBL.GetUOMDescFromID(uomid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            return uomDesc;
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {
                string itemCode, uomitem, qtyiss, qtyret, cc, rem;
                bool proceedSaving = false;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    itemCode = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("ddlItemCode")).Text;
                    uomitem = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("txtUomItemCode")).Text.Trim();
                    qtyiss = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyIss")).Text.Trim();
                    qtyret = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyRet")).Text.Trim();
                    cc = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("ddlCostCenterCode")).Text;
                    rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text.Trim();


                    if (!qtyiss.Equals("") && !itemCode.Equals("0")
                        && !uomitem.Equals("") && !qtyret.Equals(""))
                    {

                        proceedSaving = true;
                        break;

                    }
                }
                if (proceedSaving && SaveStRet())
                {
                    ClearFieldsOnly();
                    BindGridMain(txtFltDocNo.Text.Trim(), txtFltIssDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
                }
                else
                {
                    ucMessage.ShowMessage("Please enter item details completely in the grid", RMS.BL.Enums.MessageType.Error);
                }

            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }
        private bool checkAnyEmptyQtyAccepted()
        {
            bool forFlag = false;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //////////////////////////////////////////////////////////////////////////////////
                // qty
               string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyAccepted")).Text.Trim();
                if (qty.Equals(""))
                {
                    forFlag = false;
                    break;
                }
                else
                {
                    forFlag = true;
                }
            }
            return forFlag;

        }
        protected void btnSearchIGP_Click(object sender, EventArgs e)
        {
            string igpno = txtDocNoIss.Text.Trim();
            //if (!igpno.Equals(""))
            //{
                if (igpno.Contains("/") && igpno.Length > 5)
                {
                    try
                    {
                        char[] st = igpno.ToCharArray();
                        if (st[4].ToString().Equals("/"))
                        {
                            igpno = igpno.Substring(0, 4) + igpno.Substring(5);
                        }
                        else
                        {
                            igpno = "";
                            for (int i = 0; i < st.Length; i++)
                            {
                                if (!st[i].ToString().Equals("/"))
                                {
                                    igpno = igpno + st[i];
                                }
                            }
                        }
                    }
                    catch { }

                }
                else if (igpno.Contains("/"))
                {
                    char[] st = igpno.ToCharArray();
                    igpno = "";
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (!st[i].ToString().Equals("/"))
                        {
                            igpno = igpno + st[i];
                        }
                    }
                }
                else
                {
                    //nothin
                    igpno = txtDocNoIss.Text.Trim();
                }


                BindGridSrchMatIssueNotes(igpno);
                if (grdIssDocNos.Rows.Count > 0)
                {
                    grdIssDocNos.Visible = true;
                    divGrid.Visible = false;
                    divSelectedIGPInfo.Visible = false;
                }
                else
                {
                    grdIssDocNos.Visible = false;
                    divGrid.Visible = false;
                    divSelectedIGPInfo.Visible = false;
                    ucMessage.ShowMessage("No issue note found", RMS.BL.Enums.MessageType.Error);
                }
            //}
            //else
            //{
            //    ucMessage.ShowMessage("Please give some value to search", RMS.BL.Enums.MessageType.Error);
            //    txtDocNoIss.Focus();
            //}
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string noIgp = txtDocNoIss.Text.Substring(5);
                string yrnoIgp = txtDocNoIss.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(noIgp));

                string returnNo = Convert.ToString( DocNo);//txtDocNo.Text.Substring(0, 4) + txtDocNo.Text.Substring(5);
                string itmCode = ((Label)e.Row.FindControl("ddlItemCode")).Text;


                TextBox qtyIssBalance = (TextBox)e.Row.FindControl("txtQtyIssBalance");
                if(GetIssueBalance(Convert.ToInt32(yrnoIgp), Convert.ToInt32(returnNo), itmCode) != "-1")
                {
                    qtyIssBalance.Text = GetIssueBalance(Convert.ToInt32(yrnoIgp), Convert.ToInt32(returnNo), itmCode);
                }
                else
                {
                    qtyIssBalance.Text = ((TextBox)e.Row.FindControl("txtQtyIss")).Text;
                }

                if (validatorFlag)
                {
                    CompareValidator cmpValidId = (CompareValidator)e.Row.FindControl("CompValidatorID");
                    cmpValidId.Enabled = false;
                }
            }

        }
        private string GetIssueBalance(int igpno, int returnNo, string itmCode)
        {
            return stRetBL.GetIssueBalance(igpno, returnNo, itmCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltDocNo.Text.Trim(), txtFltIssDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void grdStRet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[0].Text.Substring(4);
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, 4) + "/" + e.Row.Cells[1].Text.Substring(4);

                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Session["DateFormat"].ToString());
                }

                if (e.Row.Cells[4].Text.Equals("A"))
                {
                    e.Row.Cells[4].Text = "Approved";
                }
                else if (e.Row.Cells[4].Text.Equals("P"))
                {
                    e.Row.Cells[4].Text = "Pending";
                }
                else if (e.Row.Cells[4].Text.Equals("C"))
                {
                    e.Row.Cells[4].Text = "Cancelled";
                }
            }
        }
        protected void grdStRet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdStRet.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltDocNo.Text.Trim(), txtFltIssDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void grdIssDocNos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[0].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Session["DateFormat"].ToString());
                }

            }
        }

        #endregion

        #region Helping Method

        private void GetByIDSrch()
        {

            tblStkData stkGp = stRetBL.GetByID4MatIssueStkData(VrIDIss, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            txtDocNoIss.Text = stkGp.vr_no.ToString().Substring(0, 4) + "/" + stkGp.vr_no.ToString().Substring(4);

            try
            {
                ddlLoc.SelectedValue = stkGp.LocId.ToString();

                ddlLoc.Enabled = false;
            }
            catch
            {
                ucMessage.ShowMessage("Selected Material issued note's Location is not found here", RMS.BL.Enums.MessageType.Error);
                return;
            }
            try
            {
                ddlDept.SelectedValue = stkGp.DeptId.ToString();

                ddlDept.Enabled = false;
            }
            catch
            {
                ucMessage.ShowMessage("Selected Material issued note's Department is not found here", RMS.BL.Enums.MessageType.Error);
                return;
            }

            BindTableEdit(stkGp.tblStkDataDets);

            lblSelectedParty.Text = "";

        }
        private string GetVendorNmeFromId(string vId)
        {
            return stRetBL.GetVendorNmeFromId(vId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private string GetVendorNmeFromIGP(int igpno, int locid, int vt_cd, int brId)
        {
            return stRetBL.GetVendorNmeFromIGP(igpno, locid, vt_cd, brId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private void GetByID()
        {

            tblStkData stkData = stRetBL.GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlLoc.SelectedValue = stkData.LocId.ToString();
            ddlDept.SelectedValue = stkData.DeptId.ToString();

            ddlStatus.SelectedValue = stkData.vr_apr.ToString();
            
            if (stkData.vr_apr.ToString().Equals("P"))
            {
                ucButtons.EnableSave();
            }
            else
            {
                ucButtons.DisableSave();
            }

            if (IsEdit)
            {
                txtDocNo.Text = stkData.vr_no.ToString().Substring(0, 4) + "/" + stkData.vr_no.ToString().Substring(4);
            }
            DocNoFormated = stkData.vr_no.ToString().Substring(0, 4) + "/" + stkData.vr_no.ToString().Substring(4);
            DocNo = stkData.vr_no;
            
            
            //txtDocDate.Text = stkData.vr_dt.ToString("dd-MMM-yy");
            CalendarExtender1.SelectedDate = stkData.vr_dt;
            //------
            txtRemarks.Text = stkData.vr_nrtn;

            txtDocNoIss.Text = stkData.IGPNo.Value.ToString().Substring(0, 4) + "/" + stkData.IGPNo.Value.ToString().Substring(4);
            //lblSelectedParty.Text = "Party: " + GetVendorNmeFromIGP(stkData.IGPNo.Value, stkData.LocId, 12, stkData.br_id);

            BindTableEditMain(stkData.tblStkDataDets);

            divGrid.Visible = true;
            grdIssDocNos.Visible = false;
        }
        public void BindTableEditMain(EntitySet<tblStkDataDet> dets)
        {
            GetColumns();
            int count = 0;
            decimal tot = 0;
            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Item"] = GetItemNameFromCode(dt.itm_cd);
                d_Row["ItemCode"] = dt.itm_cd;
                d_Row["QtyRet"] = dt.vr_qty.ToString("0.000");
                d_Row["QtyIss"] = dt.vr_qty_Rej.ToString("0.000");
                //QtyIssBal = Qty issued - Qty returned
                //d_Row["QtyIssBal"] = Convert.ToString(25);
                d_Row["UomItem"] = GetItemUOMLabelFromUOMId(dt.vr_uom);
                d_Row["UomItemCode"] = dt.vr_uom.ToString(); 
                d_Row["CostCenter"] = GetCostCenterFromCode(dt.CC_cd);
                d_Row["CostCenterCode"] = dt.CC_cd;
                d_Row["Rem"] = dt.vr_rmk;
                count++;
                d_table.Rows.Add(d_Row);

                tot = tot + dt.vr_qty;
            }
            //txtTotalQty.Text = tot.ToString("0.000");
            CurrentTable = d_table;
            BindGrid();

            //SetDropDownList();
        }
        private string GetItemNameFromCode(string itemcode)
        {
            return stRetBL.GetItemNameFromCode(itemcode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private string GetCostCenterFromCode(string cccode)
        {
            if (cccode != null && !cccode.Equals(""))
            {
                return stRetBL.GetCostCenterFromCode(cccode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            else
            {
                return "";
            }
        }
        private void ClearFieldsOnly()
        {
            VrIDIss = 0;
            VrID = 0;

            ddlLoc.SelectedValue = "0";
            CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlStatus.SelectedValue = "P";
            txtRemarks.Text = "";
            txtDocNoIss.Text = "";

            txtDocNo.Text = "";
            DocNo = 0;
            DocNoFormated = "";
            IsEdit = false;
            //GetGatePassNo();
            //GetDocRef();
            //BindTable();

            divSelectedIGPInfo.Visible = false;
            divGrid.Visible = false;
            grdIssDocNos.Visible = false;

            btnSrchIGP.Visible = true;
            txtDocNoIss.Enabled = true;

            validatorFlag = false;
        }
        private void ClearFields()
        {
            Response.Redirect("~/inv/storeretmgt.aspx?PID=517");
        }
        private bool SaveStRet()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            tblStkData stkData;
            if (VrID == 0)
            {
                stkData = new tblStkData();

                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        stkData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    stkData.br_id = Convert.ToInt32(Session["BranchID"]);
                }
                stkData.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                stkData.vt_cd = VoucherTypeCode; // voucher type code 18 for Store Return

                stkData.DeptId = Convert.ToInt16(ddlDept.SelectedValue);

                if (!IsEdit)
                {
                    GetGatePassNo();
                }
                //string no = txtDocNo.Text.Substring(5);
                //string yrno = txtDocNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(no));
                stkData.vr_no = DocNo;//Convert.ToInt32(yrno);

                //stkData.DocRef = txtDocRef.Text.Trim();

                string noIgp = txtDocNoIss.Text.Substring(5);
                string yrnoIgp = txtDocNoIss.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(noIgp));
                stkData.IGPNo = Convert.ToInt32(yrnoIgp);
            }
            else
            {
                stkData = stRetBL.GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }

            try
            {
                stkData.vr_dt = Convert.ToDateTime(txtDocDate.Text.Trim());
            }
            catch
            {
                ucMessage.ShowMessage("Invalid Doc Date", RMS.BL.Enums.MessageType.Error);
                txtDocDate.Focus();
                return false;
            }

            stkData.vr_nrtn = txtRemarks.Text.Trim(); // remarks
            if (Session["LoginID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    stkData.updateby = Request.Cookies["uzr"]["LoginID"];
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                stkData.updateby = Session["LoginID"].ToString();
            }

            stkData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            stkData.vr_apr = Convert.ToString(ddlStatus.SelectedValue);



            EntitySet<tblStkDataDet> stkDataDets = new EntitySet<tblStkDataDet>();
            tblStkDataDet stkDataDet;
            string itemCode, uomitem, qtyiss, qtyret, cc, rem, qtyissBal;

            HashSet<string[]> hashset = new HashSet<string[]>();
            string[] list = null;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                list = new string[3];

                //poref = ""; itemCode = ""; packs = ""; uom = ""; unitsize = ""; qty = ""; uomqty = ""; gross = ""; rem = ""; 
                itemCode = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("ddlItemCode")).Text;
                uomitem = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("txtUomItemCode")).Text.Trim();
                qtyiss = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyIss")).Text.Trim();
                qtyret = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyRet")).Text.Trim();
                cc = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("ddlCostCenterCode")).Text;
                rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text.Trim();

                qtyissBal = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyIssBalance")).Text.Trim();

                if (!itemCode.Equals("0")
                        && !qtyiss.Equals("") && !uomitem.Equals(""))
                {
                    if (string.IsNullOrEmpty(qtyret))
                        qtyret = "0";

                    stkDataDet = new tblStkDataDet();

                    stkDataDet.vr_seq = Convert.ToByte(i + 1);
                    stkDataDet.itm_cd = itemCode;

                    /////////////////
                    list[0] = stkData.vr_id.ToString();
                    list[1] = stkDataDet.vr_seq.ToString();
                    list[2] = qtyissBal;

                    hashset.Add(list);
                    ////////////////

                    try
                    {
                        stkDataDet.vr_qty = Convert.ToDecimal(qtyret);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Quantity is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    stkDataDet.vr_qty_Rej = Convert.ToDecimal(qtyiss); // saving for just info that what was Qty Iss for this return.....
                    stkDataDet.vr_uom = Convert.ToByte(uomitem);
                    stkDataDet.vr_val = 0;

                    if (cc != null && (cc.Equals("0") || cc.Equals("")))
                    {
                        stkDataDet.CC_cd = null;
                    }
                    else
                    {
                        stkDataDet.CC_cd = cc;
                    }

                    stkDataDet.vr_rmk = rem;

                    stkDataDets.Add(stkDataDet);
                }
            }
            if (stkDataDets == null || stkDataDets.Count < 1)
            {
                Response.Redirect("~/login.aspx");
            }

            if (VrID == 0)
            {
                stkData.tblStkDataDets = stkDataDets;
                string msg = stRetBL.SaveStRetFull(stkData, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("Doc No "+DocNoFormated+" "+GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
                    return false;
                }
            }
            else
            {
                string msg = stRetBL.UpdStRetFull(stkData, stkDataDets, hashset, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("Doc No " + DocNoFormated + " " + GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                }
                else if (msg.Equals("QTY_EXCEEDED"))
                {
                    ucMessage.ShowMessage("Qty Returned value is exceeding total of issued quatity value", RMS.BL.Enums.MessageType.Error);
                    return false;
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
                    return false;
                }
            }
            IsEdit = false;
            return true;
        }
        private byte GetUOMIdFromLabel(string uomitem)
        {
            return stRetBL.GetUOMIdFromLabel(uomitem, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = stRetBL.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
            ddlLoc.SelectedValue = "0";

        }
        //public void GetDocRef()
        //{
        //    txtDocRef.Text = stRetBL.GetDocReference(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 18, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //}
        public void GetGatePassNo()
        {
            //txtDocNo.Text = stRetBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 18, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //DocNoFormated = stRetBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 18, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNoFormated = stRetBL.GetDocNo(Convert.ToDateTime(txtDocDate.Text.Trim()), 18, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
        }
        private void BindGridMain(string retDocNo, string issDocNo, string status)
        {
            if (!retDocNo.Equals(""))
            {
                if (retDocNo.Contains("/") && retDocNo.Length > 5)
                {
                    try
                    {
                        char[] st = retDocNo.ToCharArray();
                        if (st[4].ToString().Equals("/"))
                        {
                            retDocNo = retDocNo.Substring(0, 4) + retDocNo.Substring(5);
                        }
                        else
                        {
                            retDocNo = "";
                            for (int i = 0; i < st.Length; i++)
                            {
                                if (!st[i].ToString().Equals("/"))
                                {
                                    retDocNo = retDocNo + st[i];
                                }
                            }
                        }
                    }
                    catch { }

                }
                else if (retDocNo.Contains("/"))
                {
                    char[] st = retDocNo.ToCharArray();
                    retDocNo = "";
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (!st[i].ToString().Equals("/"))
                        {
                            retDocNo = retDocNo + st[i];
                        }
                    }
                }
                else
                {
                    //nothin
                    //docNo = txtDocNo.Text.Trim();
                }
            }

            if (!issDocNo.Equals(""))
            {
                if (issDocNo.Contains("/") && issDocNo.Length > 5)
                {
                    try
                    {
                        char[] st = issDocNo.ToCharArray();
                        if (st[4].ToString().Equals("/"))
                        {
                            issDocNo = issDocNo.Substring(0, 4) + issDocNo.Substring(5);
                        }
                        else
                        {
                            issDocNo = "";
                            for (int i = 0; i < st.Length; i++)
                            {
                                if (!st[i].ToString().Equals("/"))
                                {
                                    issDocNo = issDocNo + st[i];
                                }
                            }
                        }
                    }
                    catch { }

                }
                else if (issDocNo.Contains("/"))
                {
                    char[] st = issDocNo.ToCharArray();
                    issDocNo = "";
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (!st[i].ToString().Equals("/"))
                        {
                            issDocNo = issDocNo + st[i];
                        }
                    }
                }
                else
                {
                    //nothin
                    //docNo = txtDocNo.Text.Trim();
                }
            }

            grdStRet.DataSource = stRetBL.GetAllStoreRets(retDocNo, issDocNo, status, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdStRet.DataBind();
        }
        private void BindGridSrchMatIssueNotes(string igpno)
        {
            int brId = 0;
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    brId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                brId = Convert.ToInt32(Session["BranchID"].ToString());
            }
            grdIssDocNos.DataSource = stRetBL.GetAllMatIssuedDocNos(igpno, brId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdIssDocNos.DataBind();
        }
        private void FillDropDownDepartment()
        {
            ddlDept.DataTextField = "CodeDesc";
            ddlDept.DataValueField = "CodeID";
            ddlDept.DataSource = new PlCodeBL().GetAll4Grid(3, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDept.DataBind();

            //ddlDept.DataTextField = "DeptNme";
            //ddlDept.DataValueField = "DeptId";
            //ddlDept.DataSource = stRetBL.GetAllDepartment((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlDept.DataBind();
        }
       
        #endregion


    }
}
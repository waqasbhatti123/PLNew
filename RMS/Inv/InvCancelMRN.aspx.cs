using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data;
using System.Data.Linq;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace RMS.Inv
{
    public partial class InvCancelMRN : BasePage
    {

        #region DataMembers
        //InvGP_BL gP = new InvGP_BL();
        MatRecBL matBL = new MatRecBL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        short VoucherTypeCode = 16; // voucher type code 16 for MATERIAL RECEIVING in Vr_Type table

#pragma warning disable CS0169 // The field 'InvCancelMRN.item' is never used
#pragma warning disable CS0169 // The field 'InvCancelMRN.uomqtycode' is never used
        string srNo, grdPoRef, item, itemCd, packs, unitsize, qty, uomqty, uomqtycode, qtyShort, qtyRej, rem;//uom, uomcode,
#pragma warning restore CS0169 // The field 'InvCancelMRN.uomqtycode' is never used
#pragma warning restore CS0169 // The field 'InvCancelMRN.item' is never used

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
        public int LocID
        {
            get { return (ViewState["LocID"] == null) ? 0 : Convert.ToInt32(ViewState["LocID"]); }
            set { ViewState["LocID"] = value; }
        }
        public int BrID
        {
            get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
        }
        public int VtCD
        {
            get { return (ViewState["VtCD"] == null) ? 0 : Convert.ToInt32(ViewState["VtCD"]); }
            set { ViewState["VtCD"] = value; }
        }
        public int VrNO
        {
            get { return (ViewState["VrNO"] == null) ? 0 : Convert.ToInt32(ViewState["VrNO"]); }
            set { ViewState["VrNO"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "MatRecMgt").ToString();
                if (Session["DateFormat"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        BrID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                    BrID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                BindTable();
                FillDropDownLoc();
                FillDropDownVendor();
                //GetMatRecDocNo();
                //GetDocRef();
                IsEdit = false;
                BindGridMain("","","","0");

                txtIGP.Focus();

            }
        }
        protected void grdMatRec_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsEdit = true;

            VrID = Convert.ToInt32(grdMatRec.SelectedDataKey.Values["vr_id"]);

            DocNoFormated = grdMatRec.SelectedRow.Cells[0].Text.Trim();
            DocNo = Convert.ToInt32(DocNoFormated.ToString().Substring(0, 4) +DocNoFormated.ToString().Substring(5));

            GetByID();
        }
        protected void grdSrchIgp_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocID = Convert.ToInt32(grdSrchIgp.SelectedDataKey.Values["locid"]);
            BrID = Convert.ToInt32(grdSrchIgp.SelectedDataKey.Values["br_id"]);
            VtCD = Convert.ToInt32(grdSrchIgp.SelectedDataKey.Values["vt_cd"]);
            VrNO = Convert.ToInt32(grdSrchIgp.SelectedDataKey.Values["vr_no"]);

            try
            {
                ddlLoc.SelectedValue = LocID.ToString();
            }
            catch
            {
                ucMessage.ShowMessage("Selected IGP's To Location is not found here", RMS.BL.Enums.MessageType.Error);
                return;
            }

            GetByIDSrch();

            divGrid.Visible = true;
            divSelectedIGPInfo.Visible = true;
            grdSrchIgp.Visible = false;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //DropDownList ddVen = (DropDownList)e.Row.Cells[0].FindControl("ddlItem");
                //DropDownList ddUom = (DropDownList)e.Row.Cells[1].FindControl("ddlUom");
                //DropDownList ddUomQty = (DropDownList)e.Row.Cells[1].FindControl("ddlUomQty");
                
                //BindDdlUOM(ddUom);
                //BindDdlUOM(ddUomQty);
                //BindddlItemDD(ddVen);

            }

        }
        public void BindDdlUOM(DropDownList ddlUom)
        {
            ddlUom.DataSource = matBL.GetAllUOM((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlUom.DataTextField = "uom_dsc";
            ddlUom.DataValueField = "uom_cd";
            ddlUom.DataBind();
        }
        public void BindddlItemDD(DropDownList ddlItem)
        {
            //ddlItem.DataSource = matBL.GetAllItem((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataSource = new ItemCodeBL().GetAllGeneralItems((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataTextField = "itm_dsc";
            ddlItem.DataValueField = "itm_cd";
            ddlItem.DataBind();
        }
        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
            //SetDropDownList();
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
                    srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].Cells[0].FindControl("lblSr")).Text;
                    grdPoRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtPoref")).Text;
                    itemCd = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].Cells[1].FindControl("ddlItem")).Text;
                    packs = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtPacks")).Text;
                    //uom = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].Cells[2].FindControl("ddlUom")).Text;
                    unitsize = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtUnitSize")).Text;
                    qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtQuantity")).Text;
                    uomqty = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].Cells[2].FindControl("ddlUomQty")).Text;
                    qtyShort = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtQtyShort")).Text;
                    qtyRej = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtQtyRej")).Text;
                    rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtRemarks")).Text;
                    
                    d_Row["Sr"] = srNo;
                    d_Row["PoRef"] = grdPoRef;
                    d_Row["Item"] = itemCd;
                    d_Row["ItemCode"] = itemCd;
                    d_Row["Packs"] = packs;
                    //d_Row["UOM"] = uom;
                    //d_Row["UOMCode"] = uom;
                    d_Row["UnitSize"] = unitsize;
                    d_Row["Quantity"] = qty;
                    d_Row["UomQty"] = uomqty;
                    d_Row["UomQtyCode"] = uomqty;
                    d_Row["QtyShort"] = qtyShort;
                    d_Row["QtyRej"] = qtyRej;
                    d_Row["Rem"] = rem;
                    
                    
                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;//assigning to viewstate datatable
                BindGrid();
                //SetDropDownList();

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
            d_Col.ColumnName = "PoRef";
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
            d_Col.ColumnName = "Packs";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UOM";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UOMCode";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UnitSize";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Quantity";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UomQty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UomQtyCode";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "QtyShort";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "QtyRej";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Rem";
            d_table.Columns.Add(d_Col);
            
        }
        public void BindTable()
        {
            GetColumns();
            for (int i = 0; i < 3; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;
                
                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }
        public void BindTableEdit(EntitySet<tblStkGPDet> dets)
        {
            GetColumns();
            int count = 0;
            decimal tot = 0;
            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Item"] = GetItemNameFromCode(dt.Itm_cd);
                d_Row["ItemCode"] = dt.Itm_cd;
                d_Row["PoRef"] = dt.PO_Ref;
                d_Row["Packs"] = dt.Pkg.Value.ToString("F");
                //d_Row["UOM"] = GetUomCodeFromUomId(dt.Pkg_UOM.ToString());
                //d_Row["UOMCode"] = dt.Pkg_UOM;
                d_Row["UnitSize"] = dt.Pkg_Size.Value.ToString("0.000");
                d_Row["Quantity"] = (dt.vr_qty).ToString("0.000");
                d_Row["UomQty"] = GetUomCodeFromUomId(dt.Itm_UOM.ToString()); 
                d_Row["UomQtyCode"] = dt.Itm_UOM;
                d_Row["QtyShort"] = "";// dt.KgsWt.Value.ToString("0.000");
                d_Row["QtyRej"] = "";
                d_Row["Rem"] = "";//dt.Remarks;
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
            return matBL.GetItemNameFromCode(itemcode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private string GetUomCodeFromUomId(string uomid)
        {
            return matBL.GetUomCodeFromUomId(uomid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                string poref, itemCode, packs, unitsize, qty, uomqty, qtyShort, qtyRej, rem;//uom, 
                poref = ((System.Web.UI.WebControls.Label)GridView1.Rows[0].FindControl("txtPoref")).Text;
                itemCode = ((System.Web.UI.WebControls.Label)GridView1.Rows[0].FindControl("ddlItem")).Text;
                packs = ((System.Web.UI.WebControls.Label)GridView1.Rows[0].FindControl("txtPacks")).Text;
                //uom = ((System.Web.UI.WebControls.Label)GridView1.Rows[0].FindControl("ddlUom")).Text;
                unitsize = ((System.Web.UI.WebControls.Label)GridView1.Rows[0].FindControl("txtUnitSize")).Text;
                qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[0].FindControl("txtQuantity")).Text;
                uomqty = ((System.Web.UI.WebControls.Label)GridView1.Rows[0].FindControl("ddlUomQty")).Text;
                qtyShort = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[0].FindControl("txtQtyShort")).Text;
                qtyRej = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[0].FindControl("txtQtyRej")).Text;
                rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[0].FindControl("txtRemarks")).Text;

                if (!qty.Trim().Equals("") && !itemCode.Trim().Equals("0") && !packs.Trim().Equals("")
                     && !unitsize.Trim().Equals("") && !qty.Trim().Equals("")
                    && !uomqty.Trim().Equals("0") )//&& !uom.Trim().Equals("0")
                //&& (!qtyShort.Trim().Equals("") || !qtyRej.Trim().Equals(""))
                {
                    //if (checkAnyEmptyQtyAccepted())
                    //{
                        for (int i = 0; i < GridView1.Rows.Count; i++)
                        {
                            qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                            qtyShort = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyShort")).Text.Trim();
                            qtyRej = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyRej")).Text.Trim();
                            if (qtyShort == "")
                                qtyShort = "0";
                            if (qtyRej == "")
                                qtyRej = "0";
                            try
                            {
                                if (Convert.ToDecimal(qty) - Convert.ToDecimal(qtyShort) - Convert.ToDecimal(qtyRej) >= 0)
                                {
                                    //Do Nothing
                                }
                                else
                                {
                                    ucMessage.ShowMessage(" Accepted quantity must be greater than equal to 0", RMS.BL.Enums.MessageType.Error);
                                    return;
                                }
                                    
                            }
                            catch
                            {
                                ucMessage.ShowMessage("Please enter valid quantities(Short, Rejected) in grid", RMS.BL.Enums.MessageType.Error);
                                return;
                            }

                        }
                        if (SaveMatRec())
                        {
                            ClearFieldsOnly();
                            BindGridMain(txtFltDocNo.Text.Trim(), txtFltIgpNo.Text.Trim(), ddlFltVendor.SelectedValue, ddlFltStatus.SelectedValue);
                        }
                       
                    //}
                    //else
                    //{
                    //    ucMessage.ShowMessage("Please enter all Accept Qty values", RMS.BL.Enums.MessageType.Error);
                    //    //txtTotalQty.Focus();
                    //}

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
                qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyAccepted")).Text.Trim();
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
            string igpno = txtIGP.Text.Trim();
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
                    igpno = txtIGP.Text.Trim();
                }

                  
                BindGridSrchChemIgps(igpno);
                if (grdSrchIgp.Rows.Count > 0)
                {
                    grdSrchIgp.Visible = true;
                    divGrid.Visible = false;
                    divSelectedIGPInfo.Visible = false;
                }
                else
                {
                    grdSrchIgp.Visible = false;
                    divGrid.Visible = false;
                    divSelectedIGPInfo.Visible = false;
                    ucMessage.ShowMessage("No IGP found", RMS.BL.Enums.MessageType.Error);
                }
            //}
            //else
            //{
            //    ucMessage.ShowMessage("Please give some value to search", RMS.BL.Enums.MessageType.Error);
            //    txtIGP.Focus();
            //}
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltDocNo.Text.Trim(), txtFltIgpNo.Text.Trim(), ddlFltVendor.SelectedValue, ddlFltStatus.SelectedValue);
        }
        protected void grdMatRec_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[0].Text.Substring(4);
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, 4) + "/" + e.Row.Cells[1].Text.Substring(4);

                e.Row.Cells[3].Text = GetPartyName(e.Row.Cells[3].Text);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[4].Text = DateTime.Parse(e.Row.Cells[4].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[4].Text = DateTime.Parse(e.Row.Cells[4].Text).ToString(Session["DateFormat"].ToString());
                }

                if (e.Row.Cells[5].Text.Equals("A"))
                {
                    e.Row.Cells[5].Text = "Approved";
                }
                else if (e.Row.Cells[5].Text.Equals("P"))
                {
                    e.Row.Cells[5].Text = "Pending";
                }
                else if (e.Row.Cells[5].Text.Equals("C"))
                {
                    e.Row.Cells[5].Text = "Cancelled";
                }
            }
        }
        protected void grdMatRec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMatRec.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltDocNo.Text.Trim(), txtFltIgpNo.Text.Trim(), ddlFltVendor.SelectedValue, ddlFltStatus.SelectedValue);
        }
        protected void grdSrchIgp_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void lnkPrint_Click(object sender, EventArgs e)
        {

            int index = 0;
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            index = clickedRow.RowIndex;
            GetMatRec(
                Convert.ToInt32(grdMatRec.DataKeys[index].Values["vr_id"])
            );
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int brid = 0;
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                brid = Convert.ToInt32(Session["BranchID"].ToString());
            }

            int vrno = matBL.checkMRN(VrID, brid, VtCD, LocID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if(vrno > 0)    
            {
                ucMessage.ShowMessage("MPN :" + vrno +" exists. Please delete MPN first to proceed." , RMS.BL.Enums.MessageType.Error);
                //ClearFieldsOnly();
                //BindGridMain("", "", "", "0");
            }
            else
            {
                string obj = matBL.cancelMRN(VrID,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (obj == "OK")
                {
                    ucMessage.ShowMessage("MRN Cancelled Successfully.", RMS.BL.Enums.MessageType.Info);

                    ClearFieldsOnly();
                    BindGridMain("", "", "", "0");
                }
                else
                {
                    ucMessage.ShowMessage("Exception: " + obj, RMS.BL.Enums.MessageType.Error);
                }
            }
            
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        #endregion

        #region Helping Method

        public void GetMatRec(int vrid)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/MatRec.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();

            List<spMatRecResult> recs = matBL.GetMaterialRecieveRecs(vrid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ReportDataSource dataSource = new ReportDataSource("spMatRecResult", recs);

            string rptLogoPath = "";
            string company = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            //company = System.Configuration.ConfigurationManager.AppSettings["CompanyName"].ToString().Trim();
            if (Session["CompName"] == null)
            {
                if (Request.Cookies["uzr"]["CompName"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    company = Request.Cookies["uzr"]["CompName"].ToString().ToString();
                }
            }
            else
            {
                company = Session["CompName"].ToString();
            }
            string brName = "", brAddress = "", brTel = "", brFax = "", brNTN = "";
            try
            {
                int branchid = 0;
                if (Session["BranchID"] == null)
                {
                    branchid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
                }
                else
                {
                    branchid = Convert.ToInt32(Session["BranchID"].ToString());
                }

                Branch branch = new voucherHomeBL().GetBranch(branchid, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                if (branch != null)
                {
                    brName = branch.br_nme;
                    brAddress = branch.br_address;
                    brTel = branch.br_tel;
                    brFax = branch.br_fax;
                    brNTN = branch.br_ntn;
                }
            }
            catch { }

            ReportParameter[] rpt = new ReportParameter[8];
            rpt[0] = new ReportParameter("ReportName", "Material Receipt Report");
            rpt[1] = new ReportParameter("LogoPath", rptLogoPath);
            rpt[2] = new ReportParameter("CompanyName", company);

            rpt[3] = new ReportParameter("brName", brName, false);
            rpt[4] = new ReportParameter("brAddress", brAddress, false);
            rpt[5] = new ReportParameter("brTel", brTel, false);
            rpt[6] = new ReportParameter("brFax", brFax, false);
            rpt[7] = new ReportParameter("brNTN", brNTN, false);


            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(rpt);

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(dataSource);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            filename = string.Format("{0}.{1}", "MatRec", "pdf");
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }
        private void GetByIDSrch()
        {

            tblStkGP stkGp = matBL.GetByID4StkGP(LocID, BrID, VrNO, VtCD, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            txtIGP.Text = stkGp.vr_no.ToString().Substring(0, 4) + "/" + stkGp.vr_no.ToString().Substring(4);

            BindTableEdit(stkGp.tblStkGPDets);

            lblSelectedParty.Text = "Party: " + GetVendorNmeFromId(stkGp.VendorId);

        }
        private string GetVendorNmeFromId(string vId)
        {
            return matBL.GetVendorNmeFromId(vId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private string GetVendorNmeFromIGP(int igpno, int locid, int vt_cd, int brId)
        {
            return matBL.GetVendorNmeFromIGP(igpno, locid, vt_cd, brId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private void GetByID()
        {

            tblStkData stkData = matBL.GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlLoc.SelectedValue = stkData.LocId.ToString();
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


            CalendarExtender1.SelectedDate = stkData.vr_dt;
            
            txtRemarks.Text = stkData.vr_nrtn;

            txtIGP.Text = stkData.IGPNo.Value.ToString().Substring(0, 4) + "/" + stkData.IGPNo.Value.ToString().Substring(4);
            lblSelectedParty.Text = "Party: " + GetVendorNmeFromIGP(stkData.IGPNo.Value, stkData.LocId, 12,stkData.br_id);

            BindTableEditMain(stkData.tblStkDataDets);

            divGrid.Visible = true;
            divSelectedIGPInfo.Visible = true;

            grdSrchIgp.Visible = false;

            btnSrchIGP.Visible = false;
            txtIGP.Enabled = false;

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
                d_Row["PoRef"] = dt.PO_Ref;
                d_Row["Packs"] = dt.vr_pkg.ToString("F");
                //d_Row["UOM"] = GetUomCodeFromUomId(dt.vr_pkg_uom.ToString());
                //d_Row["UOMCode"] = dt.vr_pkg_uom;
                d_Row["UnitSize"] = dt.vr_pkg_Size.ToString("0.000");
                d_Row["Quantity"] = (dt.vr_qty).ToString("0.000");
                d_Row["UomQty"] = GetUomCodeFromUomId(dt.vr_uom.ToString());
                d_Row["UomQtyCode"] = dt.vr_uom;
                d_Row["QtyShort"] = dt.vr_qty_shrt.ToString("0.000");
                d_Row["QtyRej"] = dt.vr_qty_Rej.ToString("0.000");
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
        private void ClearFieldsOnly()
        {
            LocID = 0;
            VtCD = 0;
            VrNO = 0;
            VrID = 0;
            DocNo = 0;
            DocNoFormated = "";
            IsEdit = false;
            txtDocNo.Text = "";

            ddlLoc.SelectedValue = "0";
            CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlStatus.SelectedValue = "P";
            txtRemarks.Text = "";
            txtIGP.Text = "";

            //GetMatRecDocNo();
            //GetDocRef();
            BindTable();

            divSelectedIGPInfo.Visible = false;
            divGrid.Visible = false;
            grdSrchIgp.Visible = false;

            btnSrchIGP.Visible = true;
            txtIGP.Enabled = true;
        }
        private void ClearFields()
        {
            Response.Redirect("~/inv/InvCancelMRN.aspx?PID=563");
        }
        private bool SaveMatRec()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }

            tblStkData stkData;
            tblStkDataDet stkDataDet = null;
            string poref, itemCode, packs, unitsize, uomqty, qtyAcc ="0", qtyShort="0", qtyRej="0", rem;//uom,
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
                stkData.vt_cd = VoucherTypeCode; // voucher type code 16 for Material Receiving


                if (!IsEdit)
                {
                    GetMatRecDocNo();
                }

                //string no = txtDocNo.Text.Substring(5);
                //string yrno = txtDocNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(no));
                stkData.vr_no = DocNo;//Convert.ToInt32(yrno);

                //stkData.DocRef = txtDocRef.Text.Trim();

                string noIgp = txtIGP.Text.Substring(5);
                string yrnoIgp = txtIGP.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(noIgp));
                stkData.IGPNo = Convert.ToInt32(yrnoIgp);
            }
            else
            {
                stkData = matBL.GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
            
            

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //poref = ""; itemCode = ""; packs = ""; uom = ""; unitsize = ""; qty = ""; uomqty = ""; gross = ""; rem = ""; 
                poref = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("txtPoref")).Text.Trim();
                itemCode = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("ddlItemCode")).Text;
                packs = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("txtPacks")).Text.Trim();
                //uom = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("ddlUomCode")).Text;
                unitsize = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("txtUnitSize")).Text.Trim();
                //qty = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                uomqty = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("ddlUomQtyCode")).Text;
                qtyAcc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                qtyShort = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyShort")).Text.Trim();
                qtyRej = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyRej")).Text.Trim();
                rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text.Trim();
                if (qtyShort == "")
                    qtyShort = "0";
                if (qtyRej == "")
                    qtyRej = "0";


                if (!itemCode.Equals("") && !packs.Equals("")
                        && !unitsize.Equals("") //&& !qty.Equals("")
                       && !uomqty.Equals("") && !qtyAcc.Equals(""))//&& !uom.Equals("")
                {
                    stkDataDet = new tblStkDataDet();

                    stkDataDet.vr_seq = Convert.ToByte(i + 1);
                    stkDataDet.itm_cd = itemCode;
                    stkDataDet.vr_pkg = Convert.ToDecimal(packs);
                    //stkDataDet.vr_pkg_uom = Convert.ToByte(uom);
                    stkDataDet.vr_pkg_Size = Convert.ToDecimal(unitsize);
                    try
                    {
                        stkDataDet.vr_qty = Convert.ToDecimal(qtyAcc);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Quantity is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }
                    stkDataDet.vr_qty_shrt = Convert.ToDecimal(qtyShort);
                    stkDataDet.vr_qty_Rej = Convert.ToDecimal(qtyRej);
                    stkDataDet.vr_uom = Convert.ToByte(uomqty);
                    stkDataDet.vr_rmk = rem;
                    stkDataDet.PO_Ref = Convert.ToInt32(poref);
                    
                    if (Convert.ToInt32(poref) != 99999 &&
                        new POrderBL().GetPOType(BrID, Convert.ToInt32(poref), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) == "L")
                    {
                        decimal discount = new POrderBL().GetPOItemDiscount(BrID, Convert.ToInt32(poref), itemCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        stkDataDet.vr_val = new POrderBL().GetPOItemRate(BrID, Convert.ToInt32(poref), itemCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) * (Convert.ToDecimal(qtyAcc) - Convert.ToDecimal(qtyShort) - Convert.ToDecimal(qtyRej))
                                                - discount;
                        stkDataDet.overall_disc = discount;
                    }
                    else
                    {
                        stkDataDet.vr_val = 0;
                        stkDataDet.overall_disc = 0;
                    }
                    ////////////////////////////////////////////////////////////
                    decimal whtRate = 0;
                    try
                    {
                        if (stkDataDet.PO_Ref != 99999)
                        {
                            tblPOrder po = new POrderBL().GetPORec(stkData.br_id,
                                  Convert.ToInt32(stkDataDet.PO_Ref),
                                  (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            whtRate = new TaxBL().GetWHTByPoRef(stkData.br_id, po.vr_no, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        }
                    }
                    catch { }
                    //stkDataDet.WHT_Amnt = stkDataDet.vr_val * whtRate / 100;
                    stkDataDet.WHT_Amnt = Math.Round((stkDataDet.vr_val /( 1 - whtRate / 100)) - stkDataDet.vr_val,2);
                    ////////////////////////////////////////////////////////////
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
                string msg = matBL.SaveMatRecFull(stkData, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("Doc No "+DocNoFormated+" "+ GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
                    return false;
                }
            }
            else
            {
                string msg = matBL.UpdMatRecFull(stkData, stkDataDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("Doc No " + DocNoFormated + " " + GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
                    return false;
                }
            }
            ////&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
            //Posting to tblStk
            string poTyp = "F";
            if (stkDataDet.PO_Ref != 99999)
                poTyp = new MPNBL().GetPOType(BrID, Convert.ToInt32(stkDataDet.PO_Ref), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (ddlStatus.SelectedValue == "A" && stkDataDet.PO_Ref != 99999 && poTyp == "L")
            {
                string glYear = new MPNBL().GetGlYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                string itmCd = "";
                decimal purchQty = 0, purchVal = 0, qty = 0, qtyshort = 0, qtyrej = 0, value=0;
                
                 
                int locId = Convert.ToInt16(ddlLoc.SelectedValue);
                for (int k = 0; k < GridView1.Rows.Count; k++)
                {
                    if (!string.IsNullOrEmpty(((TextBox)GridView1.Rows[k].FindControl("txtQuantity")).Text))
                    {
                        qty = Convert.ToDecimal(((TextBox)GridView1.Rows[k].FindControl("txtQuantity")).Text);
                    }
                    else
                        qty = 0;
                    if (!string.IsNullOrEmpty(((TextBox)GridView1.Rows[k].FindControl("txtQtyShort")).Text))
                    {
                        qtyshort = Convert.ToDecimal(((TextBox)GridView1.Rows[k].FindControl("txtQtyShort")).Text);
                    }
                    else
                        qtyshort = 0;
                    if (!string.IsNullOrEmpty(((TextBox)GridView1.Rows[k].FindControl("txtQtyRej")).Text))
                    {
                        qtyrej = Convert.ToDecimal(((TextBox)GridView1.Rows[k].FindControl("txtQtyRej")).Text);
                    }
                    else
                        qtyrej = 0;

                    itmCd = ((Label)GridView1.Rows[k].FindControl("ddlItemCode")).Text;
                    purchQty = qty - qtyshort - qtyrej;
                    /*********************************************************/
                    decimal whtRate = 0, discount = 0;
                    try
                    {
                        if (((Label)GridView1.Rows[k].FindControl("txtPoref")).Text.Trim().Replace("/", "") != "99999")
                        {
                            tblPOrder po = new POrderBL().GetPORec(BrID,
                                  Convert.ToInt32(((Label)GridView1.Rows[k].FindControl("txtPoref")).Text.Trim().Replace("/", "")),
                                  (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            whtRate = new TaxBL().GetWHTByPoRef(BrID, po.vr_no, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            discount = new POrderBL().GetPOItemDiscount(BrID, Convert.ToInt32(po.vr_no), itmCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        }
                    }
                    catch { }
                    /*********************************************************/
                    
                    purchVal = new MPNBL().GetPOItemDetail(BrID, Convert.ToInt32(stkDataDet.PO_Ref), itmCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    tblStk record = new MPNBL().GetRecByItemCode(BrID, itmCd, Convert.ToDecimal(glYear), locId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    value = purchVal * purchQty;
                    if (record == null)//Inserting
                    {
                        tblStk stk = new tblStk();
                        if (Session["BranchID"] == null)
                            stk.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            stk.br_id = Convert.ToInt32(Session["BranchID"]);
                        stk.LocId = Convert.ToInt16(locId);
                        stk.itm_cd = itmCd;
                        stk.itm_pur_qty = stk.itm_pur_qty + purchQty;
                        stk.itm_pur_val = stk.itm_pur_val + Math.Round(value , 2)
                                        + Math.Round((((value-discount) / (1 - whtRate/100)) - (value-discount)),2)
                                        - discount;

                        stk.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                        if (Session["LoginID"] == null)
                        {
                            if (Request.Cookies["uzr"] != null)
                            {
                                stk.updateby = Request.Cookies["uzr"]["LoginID"];
                            }
                        }
                        else
                        {
                            stk.updateby = Session["LoginID"].ToString();
                        }
                        new MPNBL().PostToTblStkInsert(stk, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                    else//Updating
                    {
                        record.itm_pur_qty = record.itm_pur_qty + purchQty;
                        record.itm_pur_val = record.itm_pur_val + Math.Round(value, 2)
                                           + Math.Round((((value - discount) / (1 - whtRate / 100)) - (value - discount)), 2)
                                           - discount;
                            

                        record.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                        if (Session["LoginID"] == null)
                        {
                            if (Request.Cookies["uzr"] != null)
                            {
                                record.updateby = Request.Cookies["uzr"]["LoginID"];
                            }
                        }
                        else
                        {
                            record.updateby = Session["LoginID"].ToString();
                        }
                        new MPNBL().PostToTblStkUpdate(record, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                }
            }
            ////&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&


            IsEdit = false;
            return true;
        }
        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = matBL.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
            ddlLoc.SelectedValue = "0";

        }
        public void FillDropDownVendor()
        {
            ddlFltVendor.DataSource = matBL.GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltVendor.DataTextField = "gl_dsc";
            ddlFltVendor.DataValueField = "gl_cd";
            ddlFltVendor.DataBind();
        }
        public void GetMatRecDocNo()
        {
            //txtDocNo.Text = matBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 16, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNoFormated = matBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 16, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
        }
        private void BindGridMain(string docNo, string igpNo, string vendorId, string status)
        {
            if (!docNo.Equals(""))
            {
                if (docNo.Contains("/") && docNo.Length > 5)
                {
                    try
                    {
                        char[] st = docNo.ToCharArray();
                        if (st[4].ToString().Equals("/"))
                        {
                            docNo = docNo.Substring(0, 4) + docNo.Substring(5);
                        }
                        else
                        {
                            docNo = "";
                            for (int i = 0; i < st.Length; i++)
                            {
                                if (!st[i].ToString().Equals("/"))
                                {
                                    docNo = docNo + st[i];
                                }
                            }
                        }
                    }
                    catch { }

                }
                else if (docNo.Contains("/"))
                {
                    char[] st = docNo.ToCharArray();
                    docNo = "";
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (!st[i].ToString().Equals("/"))
                        {
                            docNo = docNo + st[i];
                        }
                    }
                }
                else
                {
                    //nothin
                    //docNo = txtDocNo.Text.Trim();
                }
            }
            if (!igpNo.Equals(""))
            {
                if (igpNo.Contains("/") && igpNo.Length > 5)
                {
                    try
                    {
                        char[] st = igpNo.ToCharArray();
                        if (st[4].ToString().Equals("/"))
                        {
                            igpNo = igpNo.Substring(0, 4) + igpNo.Substring(5);
                        }
                        else
                        {
                            igpNo = "";
                            for (int i = 0; i < st.Length; i++)
                            {
                                if (!st[i].ToString().Equals("/"))
                                {
                                    igpNo = igpNo + st[i];
                                }
                            }
                        }
                    }
                    catch { }

                }
                else if (igpNo.Contains("/"))
                {
                    char[] st = igpNo.ToCharArray();
                    igpNo = "";
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (!st[i].ToString().Equals("/"))
                        {
                            igpNo = igpNo + st[i];
                        }
                    }
                }
                else
                {
                    //nothin
                    //docNo = txtDocNo.Text.Trim();
                }
            }
            grdMatRec.DataSource = matBL.GetAllMatRecs(docNo, igpNo, vendorId, "A", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdMatRec.DataBind();
        }
        private void BindGridSrchChemIgps(string igpno)
        {
            grdSrchIgp.DataSource = matBL.GetAllMatRecsSrch4IGP(igpno, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdSrchIgp.DataBind();
        }
        private string GetPartyName(string vendId)
        {
            return matBL.GetPartyName(vendId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        #endregion

       
    }
}

using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data;
using System.Data.Linq;
using System.Web.Services;

namespace RMS.sales
{
    public partial class SRNMgt : BasePage
    {

        #region DataMembers

        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        short VoucherTypeCode = 32; // voucher type code 32 for Store Receipt Note in Vr_Type table
        string srNo, itemCode, uomitem,  batch, trnsfrqty, rcvdqty, rem;

        #endregion

        #region Properties

        public int BrId
        {
            get { return (ViewState["BrId"] == null) ? 0 : Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }

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
        
        public int VrID
        {
            get { return (ViewState["VrID"] == null) ? 0 : Convert.ToInt32(ViewState["VrID"]); }
            set { ViewState["VrID"] = value; }
        }

        public int PTNVrID
        {
            get { return (ViewState["PTNVrID"] == null) ? 0 : Convert.ToInt32(ViewState["PTNVrID"]); }
            set { ViewState["PTNVrID"] = value; }
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

            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"]);
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SRNMgt").ToString();
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
                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                BindTable();
                FillDropDownLoc();
                BindGridMain("", "0");
                IsEdit = false;
                txtSrchDoc.Focus();

            }
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"]);
            }

            if (e.CommandName == "Save")
            {
                Save();
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }
        protected void grd_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsEdit = true;
            VrID = Convert.ToInt32(grd.SelectedDataKey.Values["vr_id"]);
            PTNVrID = Convert.ToInt32(grd.SelectedDataKey.Values["ptnvr_id"]);
            GetByID();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Session["DateFormat"].ToString());
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
        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void btnSrchDoc_Click(object sender, EventArgs e)
        {
            ClearFieldsOnly();
            BindGridSrchDoc();
        }
        protected void grdSrchDoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }
        protected void grdSrchDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            PTNVrID = Convert.ToInt32(grdSrchDoc.SelectedDataKey.Values["vr_id"]);
            GetSrchDocByID();
        }

        #endregion

        #region Templates GridView

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddItem = (DropDownList)e.Row.FindControl("ddlItem");
                FillDropDownItem(ddItem);
            }
        }
        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
            SetDropDownList();
        }
        public void AddRow()
        {
            if (CurrentTable != null)
            {
                d_table = CurrentTable;
                for (int i = 0; i < 5; i++)
                {
                    d_Row = d_table.NewRow();
                    d_Row["Sr"] = d_table.Rows.Count + 1;
                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;
                BindGrid();
            }
        }
        public void UpdateTable()//keeps track of previously entered data
        {
            if (CurrentTable != null)
            {

                GetColumns();
                rowsCount = CurrentTable.Rows.Count;
                for (int i = 0; i < rowsCount; i++)
                {
                    d_Row = d_table.NewRow();
                    srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lblSr")).Text;
                    itemCode = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                    batch = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtBatch")).Text;
                    uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text;
                    trnsfrqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtTrnsfrQty")).Text;
                    rcvdqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRcvdQty")).Text;
                    rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text;

                    d_Row["Sr"] = srNo;
                    d_Row["Item"] = itemCode;
                    d_Row["Batch"] = batch;
                    d_Row["UOMItem"] = uomitem;
                    d_Row["TrnsfrQty"] = trnsfrqty;
                    d_Row["RcvdQty"] = rcvdqty;
                    d_Row["Rem"] = rem;


                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;//assigning to viewstate datatable
            }
        }
        public void SetDropDownList()
        {
            rowsCount = CurrentTable.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                DropDownList dditem = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem"));
                if (i < CurrentTable.Rows.Count)
                {
                    if (CurrentTable.Rows[i]["Item"] != DBNull.Value)
                    {
                        dditem.ClearSelection();
                        dditem.Items.FindByValue(CurrentTable.Rows[i]["Item"].ToString()).Selected = true;
                    }
                }
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
            d_Col.ColumnName = "Item";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Batch";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UOMItem";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "TrnsfrQty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "RcvdQty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Rem";
            d_table.Columns.Add(d_Col);

        }
        public void BindTable()//calls on page load
        {
            GetColumns();
            for (int i = 0; i < 5; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;

                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }
        public void BindTableEdit(EntitySet<tblStkDataDet> dets)
        {
            GetColumns();
            int count = 0;
 
            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Item"] = dt.itm_cd;
                d_Row["TrnsfrQty"] = dt.vr_qty.ToString("F0");
                d_Row["RcvdQty"] = (dt.vr_qty - dt.vr_qty_Rej).ToString("F0");
                d_Row["UomItem"] = GetItemUOMLabelFromUOMId(dt.vr_uom);
                d_Row["Batch"] = dt.batch_id;
                d_Row["Rem"] = dt.vr_rmk;
                count++;
                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
            SetDropDownList();
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }

        #endregion

        #region Helping Method

        private void GetByID()
        {

            tblStkData stkD = new SRNBL().GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //ddlLoc.SelectedValue = stkD.LocId.ToString();
            ddlStatus.SelectedValue = stkD.vr_apr.ToString();
            if (stkD.vr_apr.ToString().Equals("P"))
            {
                ucButtons.EnableSave();

                //addRow.Visible = true;
                //GridView1.Enabled = true;
                //pnlMain.Enabled = true;
            }
            else
            {
                ucButtons.DisableSave();

                //addRow.Visible = false;
                //GridView1.Enabled = false;
                //pnlMain.Enabled = false;
            }

            if (IsEdit)
            {
                txtDocNo.Text = stkD.vr_no.ToString().Substring(0, 4) + "/" + stkD.vr_no.ToString().Substring(4);
            }
            DocNoFormated = stkD.vr_no.ToString().Substring(0, 4) + "/" + stkD.vr_no.ToString().Substring(4);
            DocNo = stkD.vr_no;

            if (Session["DateFormat"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    //txtDocDate.Text = stkD.vr_dt.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                    CalendarExtender1.SelectedDate = stkD.vr_dt;
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                //txtDocDate.Text = stkD.vr_dt.ToString(Session["DateFormat"].ToString());
                CalendarExtender1.SelectedDate = stkD.vr_dt;
            }

            
            
           
            txtRemarks.Text = stkD.vr_nrtn;

            //BindTableEdit(stkD.tblStkDataDets);
            GetSrchDocByID();

            foreach(tblStkDataDet stkdet in stkD.tblStkDataDets)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    if (((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue == stkdet.itm_cd)
                    {
                        ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRcvdQty")).Text = stkdet.vr_qty.ToString("F0");
                    }
                }
            }

        }
        private void GetSrchDocByID()
        {
            tblStkData stkD = new PTNBL().GetByID(PTNVrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlLoc.SelectedValue = stkD.LocId.ToString();
            txtPTNNo.Text = stkD.vr_no.ToString().Substring(0, 4) + "/" + stkD.vr_no.ToString().Substring(4);
            BindTableEdit(stkD.tblStkDataDets);

            grdSrchDoc.DataSource = null;
            grdSrchDoc.DataBind();

            ddlStatus.Focus();
        }
        private void ClearFieldsOnly()
        {
            VrID = 0;
            PTNVrID = 0;

            ddlStatus.SelectedValue = "P";
            ddlLoc.SelectedValue = "0";
            CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            
            txtDocNo.Text = "";
            txtPTNNo.Text = "";
            DocNo = 0;
            DocNoFormated = "";
           
            txtRemarks.Text = "";

            IsEdit = false;
            //GetDocNo();
            //GetDocRef();
            BindTable();
        }
        private void ClearFields()
        {
            Response.Redirect("~/sales/srnmgt.aspx?PID=924");
        }
        private bool Save()
        {
            //Validate
            int valCount = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue != "0"
                    && ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtTrnsfrQty")).Text.Trim() != ""
                    && ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRcvdQty")).Text.Trim() !=""
                    )
                {
                    valCount++;
                }
                //string batchid = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtBatch")).Text.Trim();
                //if (batchid != "")
                //{
                //    if (new PTNBL().IsBatchAlreadyExists(batchid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                //    {
                //        ucMessage.ShowMessage("Batch: " + batchid + " already exists", RMS.BL.Enums.MessageType.Error);
                //        return false;
                //    }
                //}
            }
            if (valCount == 0)
            {
                ucMessage.ShowMessage("Please select atleast one item to continue", RMS.BL.Enums.MessageType.Error);
                return false;
            }
            try
            {
                Convert.ToDateTime(txtDocDate.Text.Trim());
            }
            catch
            {
                ucMessage.ShowMessage("Invalid document date", RMS.BL.Enums.MessageType.Error);
                return false;
            }
            
            
            
            
            /*****************************************************************************************/



            tblStkData stkData;
            if (VrID == 0)
            {
                stkData = new tblStkData();

                stkData.br_id = BrId;
                stkData.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                stkData.vt_cd = VoucherTypeCode; // voucher type code 32 for Stock Receipt Note

                if (!IsEdit)
                {
                    GetDocNo();
                }
                stkData.vr_no = DocNo;

            }
            else
            {
                stkData = new SRNBL().GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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

           // stkData.DeptId = Convert.ToInt16(ddlDept.SelectedValue);
            stkData.vr_nrtn = txtRemarks.Text.Trim();
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

            if (ddlStatus.SelectedValue != "C")
                stkData.Status = "OP";
            else
                stkData.Status = "CL";



            EntitySet<tblStkDataDet> stkDataDets = new EntitySet<tblStkDataDet>();
            tblStkDataDet stkDataDet;
            string itemCode, uomitem, rcdqty, batch, rem;//trqty

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                itemCode = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text.Trim();
                //trqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtTrnsfrQty")).Text.Trim();
                rcdqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRcvdQty")).Text.Trim();
                batch = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtBatch")).Text.Trim();
                rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text.Trim();

                if (!rcdqty.Equals("") && !itemCode.Equals("0"))
                {
                    stkDataDet = new tblStkDataDet();
                    try
                    {
                        stkDataDet.batch_id =batch;
                    }
                    catch
                    {
                        stkDataDet.batch_id = null;
                    }
                    
                    stkDataDet.vr_seq = Convert.ToByte(i + 1);
                    stkDataDet.itm_cd = itemCode;
                    //stkDataDet.vr_pkg = Convert.ToDecimal(packs);
                    //stkDataDet.vr_pkg_uom = Convert.ToByte(uom);
                    //stkDataDet.vr_pkg_Size = Convert.ToDecimal(unitsize);
                    try
                    {
                        stkDataDet.vr_qty = Convert.ToDecimal(rcdqty);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Quantity is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    try
                    {
                        if (stkDataDet.vr_qty < Convert.ToDecimal(rcdqty))
                        {
                            ucMessage.ShowMessage("Received quantity should be less than equal to transfer quantity", RMS.BL.Enums.MessageType.Error);
                            return false;
                        }
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid received quantity ", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    stkDataDet.vr_qty_Rej = 0;
                    //stkDataDet.vr_uom = GetUOMIdFromLabel(uomitem);
                    stkDataDet.vr_uom = GetUOMIdFromItmCode(itemCode);
                    stkDataDet.vr_val = 0;

                    stkDataDet.CC_cd = null;
                    stkDataDet.vr_rmk = rem;
                    
                    stkDataDets.Add(stkDataDet);
                }
            }

            if (VrID == 0)
            {
                stkData.tblStkDataDets = stkDataDets;
                string msg = new SRNBL().Save(PTNVrID, stkData, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                string msg = new SRNBL().Update(PTNVrID, stkData, stkDataDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("Doc No "+DocNoFormated+" "+GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
                    return false;
                }
            }
            ClearFieldsOnly();
            BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
            return true;
        }
        private void BindGridMain(string docNo, string status)
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
            grd.DataSource = new SRNBL().GetProductionNote(docNo, Convert.ToChar(status), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grd.DataBind();
        }
        private string GetItemUOMLabelFromUOMId(byte uomid)
        {
            string uomDesc = "";
            uomDesc = new MatIssBL().GetUOMDescFromID(uomid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            return uomDesc;
        }
        private byte GetUOMIdFromLabel(string uomitem)
        {
            return new MatIssBL().GetUOMIdFromLabel(uomitem, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private byte GetUOMIdFromItmCode(string itmcode)
        {
            return new MatIssBL().GetUOMIdFromItmCode(itmcode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = new MatIssBL().GetFinishedGoodsStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
        }
        public void FillDropDownItem(DropDownList ddlItem)
        {
            //ddlItem.DataSource = new MatIssBL().GetAllItem((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataSource = new ItemCodeBL().GetAllFinishedItems((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataTextField = "itm_dsc";
            ddlItem.DataValueField = "itm_cd";
            ddlItem.DataBind();
        }
        public void GetDocNo()
        {
            DocNoFormated = new MatIssBL().GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, VoucherTypeCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0,4)+DocNoFormated.Substring(5));
        }
        public void BindGridSrchDoc()
        {
            grdSrchDoc.DataSource = new SRNBL().SrchDocByNo(txtSrchDoc.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdSrchDoc.DataBind();
        }

        #endregion

        #region WebMethods

        [WebMethod]
        public static object GetItemDetail(string itemid)
        {
            return new STNBL().wmGetItemDetail(itemid, null);
        }

        #endregion
    }
}

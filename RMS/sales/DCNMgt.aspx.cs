using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data;
using System.Data.Linq;
using System.Web.Services;
using Microsoft.Reporting.WebForms;
using System.Linq;
using System.Collections.Generic;

namespace RMS.sales
{
    public partial class DCNMgt : BasePage
    {

        #region DataMembers
        
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        short VoucherTypeCode = 34; // voucher type code 34 for Delivery Challan Note in Vr_Type table
        decimal totalQty = 0;

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
        
        public int VrID
        {
            get { return (ViewState["VrID"] == null) ? 0 : Convert.ToInt32(ViewState["VrID"]); }
            set { ViewState["VrID"] = value; }
        }

        public int BrId
        {
            get { return (ViewState["BrId"] == null) ? 0 : Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
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

        public string DCNType
        {
            get { return Convert.ToString(ViewState["DCNType"]); }
            set { ViewState["DCNType"] = value; }
        }

        public bool IsEdit
        {
            get { return Convert.ToBoolean(ViewState["IsEdit"]); }
            set { ViewState["IsEdit"] = value; }
        }

        public int SaleOrderID
        {
            get { return (ViewState["SaleOrderID"] == null) ? 0 : Convert.ToInt32(ViewState["SaleOrderID"]); }
            set { ViewState["SaleOrderID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "DCNMgt").ToString();
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
                FillDropDownParty();
                BindGridMain("","0");
                IsEdit = false;
                ddlLoc.Focus();

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
            DCNType = ddlDCNType.SelectedValue;
            if (DCNType != "SaleOrder")
            {
                SaleOrderID = 0;
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
            GetByID();
        }
        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString(Session["DateFormat"].ToString());
                }

                if (e.Row.Cells[4].Text.Equals("A"))
                {
                    e.Row.Cells[4].Text = "Approved";
                }
                else if (e.Row.Cells[4].Text.Equals("P"))
                {
                    e.Row.Cells[4].Text = "Pending";
                    e.Row.Cells[6].Text = "";
                }
                else if (e.Row.Cells[4].Text.Equals("C"))
                {
                    e.Row.Cells[4].Text = "Cancelled";
                    e.Row.Cells[6].Text = "";
                }
            }
        }
        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            int vrId = Convert.ToInt32(grd.DataKeys[grdrow.RowIndex].Values[0]);

            IQueryable<spPrintNoteResult> objDCN = new DCNBL().PrintNote(vrId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            // Variables
            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string FileName="DCN", extension = "PDF";
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + extension + "</OutputFormat>" +
            "  <PageWidth>8.27in</PageWidth>" +
            "  <PageHeight>11.69in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.2in</MarginBottom>" +
            "</DeviceInfo>";

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();

            viewer.LocalReport.ReportPath = "sales/rdlc/PrintNote.rdlc";
            ReportDataSource datasource = new ReportDataSource("spPrintNoteResult", objDCN);

            ReportParameter prm = new ReportParameter("CompanyName", System.Configuration.ConfigurationManager.AppSettings["CompanyName"]);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
            viewer.LocalReport.SetParameters(new ReportParameter[] { prm });

            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
            Response.BinaryWrite(bytes);
            //// create the file
            //// send it to the client to download
            Response.Flush();
        }
        protected void lnkPrintInvoice_Click(object sender, EventArgs e)
        {
            GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            int vrId = Convert.ToInt32(grd.DataKeys[grdrow.RowIndex].Values[0]);

            IQueryable<spPrintInvoiceResult> objDCN = new DCNBL().PrintInvoice(vrId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            // Variables
            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string FileName = "DCN", extension = "PDF";
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + extension + "</OutputFormat>" +
            "  <PageWidth>8.27in</PageWidth>" +
            "  <PageHeight>11.69in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.2in</MarginLeft>" +
            "  <MarginRight>0.2in</MarginRight>" +
            "  <MarginBottom>0.2in</MarginBottom>" +
            "</DeviceInfo>";

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();

            viewer.LocalReport.ReportPath = "sales/rdlc/PrintInvoice.rdlc";
            ReportDataSource datasource = new ReportDataSource("spPrintInvoiceResult", objDCN);

            ReportParameter prm = new ReportParameter("CompanyName", System.Configuration.ConfigurationManager.AppSettings["CompanyName"]);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
            viewer.LocalReport.SetParameters(new ReportParameter[] { prm });

            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
            Response.BinaryWrite(bytes);
            //// create the file
            //// send it to the client to download
            Response.Flush();
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
            SaleOrderID = Convert.ToInt32(grdSrchDoc.SelectedDataKey.Values["OrderID"]);
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
        public void UpdateTable()
        {
            if (CurrentTable != null)
            {
                GetColumns();
                rowsCount = CurrentTable.Rows.Count;
                string pc, us, qty, btc, btcqty, sr, itm, uom, rem; 
                for (int i = 0; i < rowsCount; i++)
                {
                    d_Row = d_table.NewRow();
                    sr = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lblSr")).Text;
                    itm = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                    uom = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text;
                    btc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtBatch")).Text;
                    btcqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txt1QtyBatch")).Text;
                    pc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPacks")).Text;
                    us = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUnitSize")).Text;
                    qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQty")).Text;
                    rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text;

                    d_Row["Sr"] = sr;
                    d_Row["Item"] = itm;
                    d_Row["UOMItem"] = uom;
                    d_Row["Batch"] = btc;
                    d_Row["BatchQty"] = btcqty;
                    d_Row["Packs"] = pc;
                    d_Row["UnitSize"] = us;
                    d_Row["Qty"] = qty;
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
            d_Col.ColumnName = "UOMItem";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Batch";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "BatchQty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Packs";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UnitSize";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Qty";
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
                d_Row["Batch"] = dt.batch_id;
                if (!string.IsNullOrEmpty(dt.batch_id) && dt.stk_id != null)
                {
                    if (dt.tblStkData.vr_apr == "P")
                        d_Row["BatchQty"] = (GetBatchQty(dt.batch_id, dt.itm_cd) + dt.vr_qty).ToString("F0");
                    else
                        d_Row["BatchQty"] = GetBatchQty(dt.batch_id, dt.itm_cd).ToString("F0");
                }
                else
                {
                    if (dt.tblStkData.vr_apr == "P")
                        d_Row["BatchQty"] = (GetStkQty(BrId, dt.itm_cd) + dt.vr_qty).ToString("F0");
                    else
                        d_Row["BatchQty"] = GetStkQty(BrId, dt.itm_cd).ToString("F0");
                }
                
                d_Row["Item"] = dt.itm_cd;
                d_Row["UomItem"] = GetItemUOMLabelFromUOMId(dt.vr_uom);
                d_Row["Packs"] = dt.vr_pkg.ToString("F0");
                d_Row["UnitSize"] = dt.vr_pkg_Size.ToString("F0");
                d_Row["Qty"] = dt.vr_qty.ToString("F0");
                d_Row["Rem"] = dt.vr_rmk;
                count++;
                d_table.Rows.Add(d_Row);

                totalQty = totalQty + dt.vr_qty;
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

        private void GetSrchDocByID()
        {
            tblSaleOrder slOrd = new DCNBL().GetSaleOrderByID(SaleOrderID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlParty.SelectedValue = slOrd.Party;
            txtGpRef.Text = slOrd.OrderNo.ToString().Substring(0, 4) + "/" + slOrd.OrderNo.ToString().Substring(4);

            ddlParty.Enabled = false;
            txtGpRef.Enabled = false;

            BindGridSaleOrderItems(SaleOrderID);

            ddlParty.Focus();
        }
        public void BindGridSaleOrderItems(int orderId)
        {
            //grdSaleOrderItems.DataSource = new DCNBL().GetSaleOrderDetailByID(orderId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //grdSaleOrderItems.DataBind();
        }
        public void BindGridSrchDoc()
        {
            grdSrchDoc.DataSource = new DCNBL().GetSaleOrders(ddlFltParty.SelectedValue, txtSrchDoc.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdSrchDoc.DataBind();
        }
        public void GetDocNo()
        {
            DocNoFormated = new MatIssBL().GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, VoucherTypeCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
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
            grd.DataSource = new DCNBL().GetDCNote(docNo, Convert.ToChar(status), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grd.DataBind();
        }
        private void GetByID()
        {
            tblStkData stkD = new DCNBL().GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (stkD.OrderID != 0)
                ddlDCNType.SelectedValue = "SaleOrder";
            else
                ddlDCNType.SelectedValue = "Direct";
            ddlDCNType.Enabled = false;


            ddlLoc.SelectedValue = stkD.LocId.ToString();
            ddlStatus.SelectedValue = stkD.vr_apr.ToString();
            txtGpRef.Text = stkD.DocRef;
            ddlParty.SelectedValue = stkD.gl_cd;

            if (stkD.vr_apr.ToString().Equals("P"))
            {
                ucButtons.EnableSave();

                addRow.Visible = true;
                GridView1.Enabled = true;
                pnlMain.Enabled = true;
            }
            else
            {
                ucButtons.DisableSave();

                addRow.Visible = false;
                GridView1.Enabled = false;
                pnlMain.Enabled = false;
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
            if (stkD.OrderID != null)
            {
                SaleOrderID = stkD.OrderID.Value;
                if (SaleOrderID > 0)
                {
                    grdSrchDoc.DataSource = new DCNBL().GetSaleOrders(stkD.gl_cd, stkD.DocRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    grdSrchDoc.DataBind();
                }
                else
                {
                    grdSrchDoc.DataSource = null;
                    grdSrchDoc.DataBind();
                }
                BindGridSaleOrderItems(SaleOrderID);
            }
            BindTableEdit(stkD.tblStkDataDets);
            txtTotalQty.Text = totalQty.ToString("F0");
        }
        private void ClearFieldsOnly()
        {
            VrID = 0;
            SaleOrderID = 0;

            ddlLoc.SelectedValue = "0";
            ddlParty.SelectedValue = "0";
            ddlStatus.SelectedValue = "P";
            CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
     
            txtDocNo.Text = "";
            DocNo = 0;
            DocNoFormated = "";
            txtTotalQty.Text = "";
            txtRemarks.Text = "";
            txtGpRef.Text = "";

            ddlParty.Enabled = true;
            txtGpRef.Enabled = true;
            IsEdit = false;
            BindTable();

            grdSrchDoc.DataSource = null;
            grdSrchDoc.DataBind();
            grdSaleOrderItems.DataSource = null;
            grdSaleOrderItems.DataBind();
        }
        private void ClearFields()
        {
            Response.Redirect("~/sales/dcnmgt.aspx?PID=925");
        }
        private int GetStkIdUsingBatchId(string batchid)
        {
            return new DCNBL().GetStkIdByBatch(batchid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private int GetStkId(int brid, int locid, string itmcode)
        {
            return new DCNBL().GetStkId(brid, locid, itmcode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private decimal GetStkQty(int brid, string itmcd)
        {
            return new DCNBL().GetStk(brid, itmcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private decimal GetBatchQty(string batchid, string itmcode)
        {
            return new DCNBL().GetBatchQty(BrId, batchid, itmcode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
        public void FillDropDownItem(DropDownList ddlItem)
        {
            //ddlItem.DataSource = new MatIssBL().GetAllItem((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataSource = new ItemCodeBL().GetAllFinishedItems((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataTextField = "itm_dsc";
            ddlItem.DataValueField = "itm_cd";
            ddlItem.DataBind();
        }
        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = new MatIssBL().GetFinishedGoodsStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
            ddlLoc.SelectedValue = "0";

        }
        public void FillDropDownParty()
        {
            ddlParty.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlParty.DataTextField = "gl_dsc";
            ddlParty.DataValueField = "gl_cd";
            ddlParty.DataBind();

            ddlFltParty.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltParty.DataTextField = "gl_dsc";
            ddlFltParty.DataValueField = "gl_cd";
            ddlFltParty.DataBind();
        }
        private bool Save()
        {
            //Validate
            int valCount = 0, repeatCount = 0, repeatCount1 = 0, grdRowsCount;
            string itmcd, batch;
            grdRowsCount = GridView1.Rows.Count;
            for (int i = 0; i < grdRowsCount; i++)
            {
                repeatCount = 0;
                repeatCount1 = 0;
                itmcd = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                batch = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtBatch")).Text.Trim();
                if (itmcd != "0"
                    && ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPacks")).Text.Trim() != ""
                    && ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUnitSize")).Text.Trim() != ""
                    )
                {
                    valCount++;

                    for (int j = 0; j < grdRowsCount; j++)
                    {
                        if (itmcd != "0" && itmcd == ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[j].FindControl("ddlItem")).SelectedValue &&
                            batch == ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtBatch")).Text.Trim()
                            )
                        {
                            repeatCount++;
                        }
                        if (itmcd != "0" && itmcd == ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[j].FindControl("ddlItem")).SelectedValue &&
                            batch == ""
                            )
                        {
                            repeatCount1++;
                        }
                    }
                    if (repeatCount > 1 || repeatCount1 > 1)
                    {
                        ucMessage.ShowMessage("Item cannot be selected more than once", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }


                }
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




            if (SaleOrderID > 0)
            {
                List<tblSaleOrderDet> lst = new DCNBL().GetSaleOrderItemList(SaleOrderID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                bool exist;
                for (int i = 0; i < grdRowsCount; i++)
                {
                    exist = true;
                    itmcd = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                    if (itmcd != "0")
                    {
                        foreach (var d in lst)
                        {
                            if (d.ItemID == itmcd)
                            {
                                exist = true;
                                break;
                            }
                            else
                            {
                                exist = false;
                            }
                        }

                        if (!exist)
                        {
                            ucMessage.ShowMessage("Item (" + new DCNBL().GetItemByCode(itmcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).itm_dsc + ") does not present in sale order", RMS.BL.Enums.MessageType.Error);
                            return false;
                        }
                    }
                }

                //******************************************************************//
                string msg, cd, q;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    cd = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                    q = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQty")).Text.Trim();

                    foreach (var slDet in lst)
                    {
                        if (cd == slDet.ItemID)
                        {
                            msg = new DCNBL().IsItemQtyGreaterThanSaleOrderQty(slDet, Convert.ToDecimal(q), IsEdit, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (msg != "ok")
                            {
                                ucMessage.ShowMessage("Item: " + new DCNBL().GetItemByCode(slDet.ItemID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).itm_dsc + " cannot have quantity greater then sale order quantity", RMS.BL.Enums.MessageType.Error);
                                return false;
                            }
                        }
                    }
                }
                //******************************************************************//
            }




            /*****************************************************************************************/



            tblStkData stkData;
            if (VrID == 0)
            {
                stkData = new tblStkData();

                stkData.br_id = BrId;
                stkData.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                stkData.vt_cd = VoucherTypeCode; // voucher type code 34 for Delivery Challan Note


                if (!IsEdit)
                {
                    GetDocNo();
                }
                stkData.vr_no = DocNo;

            }
            else
            {
                stkData = new DCNBL().GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
            if (SaleOrderID > 0)
            {
                stkData.OrderID = SaleOrderID;
                stkData.DocRef = txtGpRef.Text.Replace("/", "");
            }
            else
            {
                stkData.OrderID = 0;
                stkData.DocRef = txtGpRef.Text;
            }
            stkData.gl_cd = ddlParty.SelectedValue;
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



            EntitySet<tblStkDataDet> stkDataDets = new EntitySet<tblStkDataDet>();
            tblStkDataDet stkDataDet;
            string itemCode, uomitem, qty, pc, us, btc, rem;

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                itemCode = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text.Trim();
                btc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtBatch")).Text.Trim();
                qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQty")).Text.Trim();
                pc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPacks")).Text.Trim();
                us = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUnitSize")).Text.Trim();
                rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text.Trim();

                if (!qty.Equals("") && !itemCode.Equals("0") &&
                     !pc.Equals("") && !us.Equals("") && !uomitem.Equals(""))
                {
                    stkDataDet = new tblStkDataDet();
                    try
                    {
                        stkDataDet.batch_id = btc;
                        try
                        {
                            if (!string.IsNullOrEmpty(btc))
                                stkDataDet.stk_id = GetStkIdUsingBatchId(btc);
                            else
                            {
                                stkDataDet.stk_id = GetStkId(stkData.br_id, stkData.LocId, itemCode);
                            }
                        }
                        catch
                        {
                            ucMessage.ShowMessage("Exception: Could not find stkid", RMS.BL.Enums.MessageType.Error);
                            return false;
                        }
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
                        stkDataDet.vr_qty = Convert.ToDecimal(qty);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Quantity is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }


                    stkDataDet.vr_qty_Rej = 0;
                    stkDataDet.vr_uom = GetUOMIdFromLabel(uomitem);
                    stkDataDet.vr_val = 0;
                    stkDataDet.vr_pkg_Size = Convert.ToDecimal(us);
                    stkDataDet.vr_pkg = Convert.ToDecimal(pc);


                    //if (cc.Equals("0"))
                    //{
                    stkDataDet.CC_cd = null;
                    //}
                    //else
                    //{
                    //    stkDataDet.CC_cd = cc;
                    //}

                    stkDataDet.vr_rmk = rem;

                    stkDataDets.Add(stkDataDet);
                }
            }

            if (VrID == 0)
            {
                stkData.tblStkDataDets = stkDataDets;
                //string msg = new DCNBL().Save(stkData, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                string msg = new DCNBL().SaveDCN(stkData, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("Doc No " + DocNoFormated + " " + GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
                    return false;
                }
            }
            else
            {
                //string msg = new DCNBL().Update(stkData, stkDataDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                string msg = new DCNBL().UpdateDCN(stkData, stkDataDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
            ClearFieldsOnly();
            BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
            return true;
        }

        #endregion

        #region WebMethods

        [WebMethod]
        public static object GetItemDetail(string itemid)
        {
            return new STNBL().wmGetItemDetail(itemid, null);
        }

        [WebMethod]
        public static object GetBatchDetail(string batchid)
        {
            return new DCNBL().wmGetBatchDetail(batchid, null);
        }

        #endregion
    }
}

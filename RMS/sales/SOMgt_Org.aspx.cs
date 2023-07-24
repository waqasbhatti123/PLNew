using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data;
using System.Data.Linq;
using System.Web.Services;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

namespace RMS.sales
{
    public partial class SOMgt_Org : BasePage
    {

        #region DataMembers

        SalesOrderBL slBL = new SalesOrderBL();

        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;

        string srNo, itemCode, uomitem, orderqty, rate, schdate, rem;

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

        public int SRVrID
        {
            get { return (ViewState["SRVrID"] == null) ? 0 : Convert.ToInt32(ViewState["SRVrID"]); }
            set { ViewState["SRVrID"] = value; }
        }

        public int OrderID
        {
            get { return (ViewState["OrderID"] == null) ? 0 : Convert.ToInt32(ViewState["OrderID"]); }
            set { ViewState["OrderID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SOMgt").ToString();
                if (Session["DateFormat"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        CalendarExtender3.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                    CalendarExtender3.Format = Session["DateFormat"].ToString();
                }
                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                BindTable();
                FillDropDownParty();
                BindGridMain("");
                IsEdit = false;
                ddlStatus.Focus();
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltDocNo.Text.Trim());
        }
        protected void grd_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsEdit = true;
            OrderID = Convert.ToInt32(grd.SelectedDataKey.Values["OrderID"]);
            GetByID();
        }
        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text).ToString(Session["DateFormat"].ToString());
                }
            }
        }
        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltDocNo.Text.Trim());
        }
        protected void lnkSalesOrder_Click(object sender, EventArgs e)
        {
            GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            int ordId = Convert.ToInt32(grd.DataKeys[grdrow.RowIndex].Values[0]);

            List<spSalesOrderResult> obj = slBL.GetSalesOrder(ordId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

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

            viewer.LocalReport.ReportPath = "sales/rdlc/PrintSalesOrder.rdlc";
            ReportDataSource datasource = new ReportDataSource("spSalesOrderResult", obj);

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
            SRVrID = Convert.ToInt32(grdSrchDoc.SelectedDataKey.Values["vr_id"]);
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
                    uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text;
                    orderqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtOrderQty")).Text;
                    rate = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRate")).Text;
                    schdate = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSchDate")).Text;
                    rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text;

                    d_Row["Sr"] = srNo;
                    d_Row["Item"] = itemCode;
                    d_Row["UOMItem"] = uomitem;
                    d_Row["OrderQty"] = orderqty;
                    d_Row["Rate"] = rate;
                    d_Row["SchDate"] = schdate;
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
            d_Col.ColumnName = "OrderQty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Rate";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "SchDate";
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
        public void BindTableEdit(List<tblSaleOrderDet> dets)
        {
            GetColumns();
            int count = 0;
 
            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Item"] = dt.ItemID;
                d_Row["OrderQty"] = dt.OrderQty.ToString("F0");
                d_Row["Rate"] = dt.ItemRate.ToString("F0");
                d_Row["UomItem"] = GetItemUOMLabelFromUOMId(Convert.ToByte(dt.UnitID));
                if (dt.SchDate != null)
                {
                    d_Row["SchDate"] = dt.SchDate.Value.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                }
                d_Row["Rem"] = dt.OrderRemarks;
                count++;
                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
            SetDropDownList();
        }
        public void BindTableEditWithSaleReq(EntitySet<tblSdSaleReqDet> dets)
        {
            GetColumns();
            int count = 0;

            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Item"] = dt.itm_cd;
                d_Row["OrderQty"] = dt.vr_qty_req.ToString("F0");
                d_Row["Rate"] = "0";
                d_Row["UomItem"] = GetItemUOMLabelFromUOMId(Convert.ToByte(dt.vr_uom));
                if (dt.vr_exp_date != null)
                {
                    d_Row["SchDate"] = dt.vr_exp_date.Value.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                }
                d_Row["Rem"] = "";
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

        private void GetSrchDocByID()
        {
            tblSdSaleReq stkD = slBL.GetByIDReq(SRVrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //BindTableEditWithSaleReq(stkD.tblSdSaleReqDets);

            grdSrchDoc.DataSource = null;
            grdSrchDoc.DataBind();

            ddlStatus.Focus();
        }
        public void BindGridSrchDoc()
        {
            grdSrchDoc.DataSource = slBL.SrchDocByNo(txtSrchDoc.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdSrchDoc.DataBind();
        }
        private void GetByID()
        {

            tblSaleOrder slDet = slBL.GetByID(OrderID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (slDet.Ord_Apr.ToString().Equals("P"))
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
                txtOrderNo.Text = slDet.OrderNo.ToString().Substring(0, 4) + "/" + slDet.OrderNo.ToString().Substring(4);
            }
            DocNoFormated = slDet.OrderNo.ToString().Substring(0, 4) + "/" + slDet.OrderNo.ToString().Substring(4);
            DocNo = slDet.OrderNo;

            if (Session["DateFormat"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    CalendarExtender1.SelectedDate = slDet.OrderDate;
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                CalendarExtender1.SelectedDate = slDet.OrderDate;
            }
            if (slDet.SalesPerson != null)
            {
                txtSalesPerson.Text = GetSalesPersonById(slDet.SalesPerson.Value);
                hdnSalesPerson.Value = slDet.SalesPerson.Value.ToString();
            }
            ddlParty.SelectedValue = slDet.Party;
            txtShipTo.Text = slDet.ShipTo;
            if (slDet.DelPeriod != null)
                txtDeliveryPeriod.Text = slDet.DelPeriod.Value.ToString("F0");
            txtBuyerRef.Text = slDet.BuyerRef;
            if (slDet.BuyerRefDate != null)
                CalendarExtender3.SelectedDate = slDet.BuyerRefDate.Value;
            if (slDet.DiscountPC != null)
                txtDiscountPercent.Text = slDet.DiscountPC.Value.ToString();
            if (slDet.PaidAmount != null)
                txtPaidAmount.Text = slDet.PaidAmount.Value.ToString("F0");
            txtBillTerms.Text = slDet.BillTerms;
            txtPaymentTerms.Text = slDet.PaymentTerms;
            txtDeliveryTerms.Text = slDet.DeliveryTerms;
            txtRemarks.Text = slDet.Remarks;
            if (slDet.QTY_Var_Pc != null)
                txtQtyVariance.Text = slDet.QTY_Var_Pc.Value.ToString();
            ddlStatus.SelectedValue = slDet.Ord_Apr.Value.ToString();

            BindTableEdit(slBL.GetDetailByID(OrderID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]));

        }
        private void BindGridMain(string docNo)
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
            grd.DataSource = slBL.GetSaleOrder(docNo, "OP", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
        private void ClearFieldsOnly()
        {
            OrderID = 0;
            txtOrderNo.Text = "";
            txtSalesPerson.Text = "";
            ddlParty.SelectedValue = "0";
            txtShipTo.Text = "";
            txtDeliveryPeriod.Text = "";
            txtBuyerRef.Text = "";
            txtBuyerRefDate.Text = "";
            txtDiscountPercent.Text = "";
            txtPaidAmount.Text = "";
            txtBillTerms.Text = "";
            txtPaymentTerms.Text = "";
            txtDeliveryTerms.Text = "";
            txtRemarks.Text = "";
            txtQtyVariance.Text = "";
            ddlStatus.SelectedValue = "P";

            CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            CalendarExtender3.SelectedDate = null;

            DocNo = 0;
            DocNoFormated = "";

            hdnSalesPerson.Value = null;

            IsEdit = false;
            BindTable();
        }
        private void ClearFields()
        {
            Response.Redirect("~/sales/somgt.aspx?PID=921");
        }
        private bool Save()
        {
            //Validate
            int valCount = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue != "0"
                    && ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtOrderQty")).Text.Trim() != ""
                    )
                {
                    valCount++;
                }
                string schdate = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSchDate")).Text.Trim();
                if (schdate != "")
                {
                    try
                    {
                        Convert.ToDateTime(schdate.Trim());
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid schedule date", RMS.BL.Enums.MessageType.Error);
                        ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSchDate")).Focus();
                        return false;
                    }
                }
            }
            if (valCount == 0)
            {
                ucMessage.ShowMessage("Please select atleast one item to continue", RMS.BL.Enums.MessageType.Error);
                return false;
            }
            string txt = "orderdate";
            try
            {
                Convert.ToDateTime(txtOrderDate.Text.Trim());
                txt = "buyerrefdate";
                if (!string.IsNullOrEmpty(txtBuyerRefDate.Text))
                    Convert.ToDateTime(txtBuyerRefDate.Text.Trim());
            }
            catch
            {
                if (txt == "orderdate")
                    ucMessage.ShowMessage("Invalid order date", RMS.BL.Enums.MessageType.Error);
                else
                    ucMessage.ShowMessage("Invalid buyer reference date", RMS.BL.Enums.MessageType.Error);
                return false;
            }




            /*****************************************************************************************/



            tblSaleOrder OrdData;
            if (!IsEdit)
            {
                OrdData = new tblSaleOrder();

                OrdData.br_id = BrId;
                OrderID = GetOrderID(BrId);
                OrdData.OrderID = OrderID;
                GetDocNo();
                OrdData.OrderNo = DocNo;
              
            }
            else
            {
                OrdData = slBL.GetByID(OrderID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            OrdData.OrderDate = Convert.ToDateTime(txtOrderDate.Text.Trim());
            OrdData.TransTypeID = 1;
            if (!string.IsNullOrEmpty(hdnSalesPerson.Value))
                OrdData.SalesPerson = Convert.ToInt32(hdnSalesPerson.Value);
            else
                OrdData.SalesPerson = null;
            OrdData.Party = ddlParty.SelectedValue;
            OrdData.ShipTo = txtShipTo.Text;
            OrdData.BillTerms = txtBillTerms.Text;
            if (txtDiscountPercent.Text != "")
                OrdData.DiscountPC = Convert.ToDecimal(txtDiscountPercent.Text.Trim());
            else
                OrdData.DiscountPC = 0;
            if (txtPaidAmount.Text != "")
                OrdData.PaidAmount = Convert.ToDecimal(txtPaidAmount.Text.Trim());
            else
                OrdData.PaidAmount = 0;
            OrdData.BuyerRef = txtBuyerRef.Text;
            if (txtBuyerRef.Text.Trim() != "")
                OrdData.BuyerRefDate = Convert.ToDateTime(txtBuyerRefDate.Text.Trim());
            OrdData.PaymentTerms = txtPaymentTerms.Text;
            OrdData.DeliveryTerms = txtDeliveryTerms.Text;
            if (!IsEdit)
            {
                if (Session["UserID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        OrdData.CreatedBy = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    OrdData.CreatedBy = Convert.ToInt32(Session["UserID"].ToString());
                }
                OrdData.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            else
            {
                //if (Session["UserID"] == null)
                //{
                //    if (Request.Cookies["uzr"] != null)
                //    {
                //        OrdData.UpdateBy = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                //    }
                //    else
                //    {
                //        Response.Redirect("~/login.aspx");
                //    }
                //}
                //else
                //{
                //    OrdData.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                //}
                //OrdData.UpdateDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            OrdData.Remarks = txtRemarks.Text;
            OrdData.Status = "OP";
            if (txtDeliveryPeriod.Text != "")
                OrdData.DelPeriod = Convert.ToInt32(txtDeliveryPeriod.Text);
            if (!string.IsNullOrEmpty(txtQtyVariance.Text))
                OrdData.QTY_Var_Pc = Convert.ToDecimal(txtQtyVariance.Text);
            OrdData.Ord_Apr = Convert.ToChar(ddlStatus.SelectedValue);


            EntitySet<tblSaleOrderDet> enttyOrdDet = new EntitySet<tblSaleOrderDet>();
            tblSaleOrderDet OrderDataDet;
            string itemCode, uomitem, ordqty, rate, schdate1, rem;

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                itemCode = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text.Trim();
                ordqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtOrderQty")).Text.Trim();
                rate = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRate")).Text.Trim();
                schdate1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSchDate")).Text.Trim();
                rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text.Trim();

                if (!ordqty.Equals("") && !itemCode.Equals("0"))
                {
                    OrderDataDet = new tblSaleOrderDet();

                    OrderDataDet.OrderID = OrdData.OrderID;
                    OrderDataDet.Oseq = i + 1;
                    OrderDataDet.ItemID = itemCode;
                    OrderDataDet.UnitID = GetUOMIdFromItmCode(itemCode);
                    try
                    {
                        if (schdate != "")
                            OrderDataDet.SchDate = Convert.ToDateTime(schdate1);
                    }
                    catch
                    {
                        OrderDataDet.SchDate = null;
                    }

                    OrderDataDet.OrderQty = Convert.ToDecimal(ordqty);
                    OrderDataDet.ShipQty = 0;//Fill when DCN is created
                    OrderDataDet.RecdQty = 0;
                    OrderDataDet.ItemRate = Convert.ToDecimal(rate);
                    OrderDataDet.OrderRemarks = rem;
                    OrderDataDet.Status = "OP";

                    enttyOrdDet.Add(OrderDataDet);
                }
            }

            if (!IsEdit)
            {
                string msg = slBL.Save(OrdData, enttyOrdDet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                string msg = slBL.Update(OrdData, enttyOrdDet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
            BindGridMain(txtFltDocNo.Text.Trim());
            return true;
        }
        private byte GetUOMIdFromItmCode(string itmcode)
        {
            return new MatIssBL().GetUOMIdFromItmCode(itmcode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public void FillDropDownItem(DropDownList ddlItem)
        {
            //ddlItem.DataSource = new MatIssBL().GetAllItem((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataSource = new ItemCodeBL().GetAllFinishedItems((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataTextField = "itm_dsc";
            ddlItem.DataValueField = "itm_cd";
            ddlItem.DataBind();
        }
        public void FillDropDownParty()
        {
            ddlParty.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlParty.DataTextField = "gl_dsc";
            ddlParty.DataValueField = "gl_cd";
            ddlParty.DataBind();
        }
        public void GetDocNo()
        {
            DocNoFormated = slBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0,4)+DocNoFormated.Substring(5));
        }
        public int GetOrderID(int brid)
        {
            return slBL.GetOrderIDByBranch(brid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public string GetSalesPersonById(int id)
        {
            return slBL.GetSalesPersonInfo(id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        #endregion

        #region WebMethods

        [WebMethod]
        public static object GetItemDetail(string itemid)
        {
            return new SalesOrderBL().wmGetItemDetail(itemid, null);
        }

        [WebMethod]
        public static object GetSalesPersonDetail(string salespersoninfo)
        {
            return new SalesOrderBL().wmGetSalesPerson(salespersoninfo, null);
        }

        #endregion
    }
}

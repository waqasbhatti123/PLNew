using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Data.Linq;
using Microsoft.Reporting.WebForms;
using System.Web.Services;

namespace RMS.sales
{
    public partial class SaleReq : BasePage
    {

        #region DataMembers

        SaleReqBL saleReqBL = new SaleReqBL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
#pragma warning disable CS0414 // The field 'SaleReq.DistrGroupId' is assigned but its value is never used
        int DistrGroupId = 5;
#pragma warning restore CS0414 // The field 'SaleReq.DistrGroupId' is assigned but its value is never used
        

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

#pragma warning disable CS0114 // 'SaleReq.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'SaleReq.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public int PID
        {
            get { return (ViewState["PID"] == null) ? 0 : Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        public int GroupID
        {
            get { return (ViewState["GroupID"] == null) ? 0 : Convert.ToInt32(ViewState["GroupID"]); }
            set { ViewState["GroupID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SaleReq").ToString();
                if (Session["DateFormat"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        calFltFromDt.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        calFltToDt.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    calFltFromDt.Format = Session["DateFormat"].ToString();
                    calFltToDt.Format = Session["DateFormat"].ToString();
                }
                txtFltFromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month.ToString() + "-01").ToString("dd-MMM-yy");
                txtFltToDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yy");

                PID = Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 922)
                {
                    pnlTop.Visible = false;
                    grdPurchReq.Columns[6].Visible = true;
                    grdPurchReq.Columns[5].Visible = false;
                }
                else if (PID == 927)
                {
                    pnlTop.Visible = true;
                    grdPurchReq.Columns[6].Visible = false;
                    grdPurchReq.Columns[5].Visible = true;

                    if (Session["DateFormat"] == null)
                    {
                        if (Request.Cookies["uzr"] != null)
                        {
                            txtDteCal.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        }
                        else
                        {
                            Response.Redirect("~/login.aspx");
                        }
                    }
                    else
                    {
                        txtDteCal.Format = Session["DateFormat"].ToString();
                    }
                    txtDte.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yy");
                    txtUser.Text = Session["LoginID"].ToString();
                    BindTable();
                    IsEdit = false;
                }
                else { }
                BindGridMain(txtFltDocNo.Text.Trim(), txtFltDocRef.Text.Trim(), ddlFltStatus.SelectedValue);
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DropDownList dditem = (DropDownList)e.Row.FindControl("ddlItem");
                
                TextBox txtduedte = (TextBox)e.Row.FindControl("txtDueDate");
                if (!txtduedte.Text.Trim().Equals(""))
                {
                    txtduedte.Text = Convert.ToDateTime(txtduedte.Text).ToString("dd-MMM-yy");
                }
                
                FillDropDownItem(dditem);
            }

        }
        public void FillDropDownItem(DropDownList ddlItem)
        {
            ddlItem.DataSource = new ItemCodeBL().GetAllFinishedItems((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataTextField = "itm_dsc";
            ddlItem.DataValueField = "itm_cd";
            ddlItem.DataBind();
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
                for (int i = 0; i < rowsCount; i++)
                {
                    d_Row = d_table.NewRow();
                    string srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].Cells[0].FindControl("lblSr")).Text.Trim();
                    string itemCode = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlItem")).SelectedValue;
                    string uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtUomItem")).Text.Trim();
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text.Trim();
                    string dte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtDueDate")).Text.Trim();

                    d_Row["Sr"] = srNo;
                    d_Row["Item"] = itemCode;
                    d_Row["Quantity"] = qty;
                    d_Row["UOM"] = uomitem;
                    if (dte != "")
                    {
                        d_Row["DueDate"] = Convert.ToDateTime(dte).ToString("dd-MMM-yy");
                    }
                    
                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;//assigning to viewstate datatable
                //BindGrid();
                //SetDropDownList();

            }
        }
        public void SetDropDownList()
        {
            rowsCount = CurrentTable.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                DropDownList dditem = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlItem"));
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
            d_Col.ColumnName = "UOM";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Quantity";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.DateTime");
            d_Col.ColumnName = "DueDate";
            d_table.Columns.Add(d_Col);
            
        }
        public void BindTable()
        {
            GetColumns();
            for (int i = 0; i < 8; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;
                
                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }
        public void BindTableEdit(EntitySet<tblSdSaleReqDet> dets)
        {
            GetColumns();
            int count = 0;

            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
               
                d_Row["Item"] = dt.itm_cd;
                d_Row["Quantity"] = dt.vr_qty_req.ToString("F");
                d_Row["UOM"] = GetUOMLabelFromUOMId(dt.vr_uom);
                if (dt.vr_exp_date != null)
                {
                    d_Row["DueDate"] = dt.vr_exp_date.Value.ToString("dd-MMM-yy");
                }
                
                count++;
                d_table.Rows.Add(d_Row);
               
            }
            CurrentTable = d_table;
            BindGrid();

            SetDropDownList();
        }
        private string GetUOMLabelFromUOMId(byte uomid)
        {
            return saleReqBL.GetUOMDescFromID(uomid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);   
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }
        //protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList itemdd = (DropDownList)sender;
        //    string itcode = itemdd.SelectedValue;

        //    GridViewRow grdrDropDownRow = ((GridViewRow)itemdd.Parent.Parent);
        //    TextBox lbluom = (TextBox)grdrDropDownRow.FindControl("txtUomItem");
        //    if (lbluom != null)
        //    {
        //        if (itemdd.SelectedIndex > 0)
        //        {
        //            lbluom.Text = saleReqBL.GetItemUOMLabel(itcode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
        //        }
        //        else
        //        {
        //            lbluom.Text = "";
        //        }
        //    }
        //    itemdd.Focus();
        //}
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {

                //Validate
                int valCount = 0, repeatCount = 0, repeatCount1 = 0, grdRowsCount;
                string itmcd, qty, dueDte;
                grdRowsCount = GridView1.Rows.Count;
                for (int i = 0; i < grdRowsCount; i++)
                {
                    repeatCount = 0;
                    repeatCount1 = 0;
                    itmcd = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                    qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                    dueDte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDueDate")).Text.Trim();

                    if (itmcd != "0" && qty != "" && dueDte != "")
                    {
                        try
                        {
                            Convert.ToDateTime(dueDte);
                        }
                        catch
                        {
                            ucMessage.ShowMessage("Invalid due date provided", RMS.BL.Enums.MessageType.Error);
                            return;
                        }

                        valCount++;

                        for (int j = 0; j < grdRowsCount; j++)
                        {
                            if (itmcd != "0" && itmcd == ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[j].FindControl("ddlItem")).SelectedValue)
                            {
                                repeatCount1++;
                            }
                        }
                        if (repeatCount > 1 || repeatCount1 > 1)
                        {
                            ucMessage.ShowMessage("Item cannot be selected more than once", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                }
                if (valCount == 0)
                {
                    ucMessage.ShowMessage("Please select atleast one item to continue", RMS.BL.Enums.MessageType.Error);
                    return;
                }

                if (SaveSaleRequest())
                {
                    ClearFieldsOnly();
                    BindGridMain(txtFltDocNo.Text.Trim(), txtFltDocRef.Text.Trim(), ddlFltStatus.SelectedValue);

                }

                
                    //string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[0].FindControl("txtQuantity")).Text.Trim();
                    //string dueDte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[0].FindControl("txtDueDate")).Text.Trim();
                    //string item = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[0].FindControl("ddlItem")).SelectedValue;
                    ////string uom = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[0].FindControl("txtUomItem")).Text;
                    //if (!qty.Trim().Equals("") && !dueDte.Trim().Equals("") && !item.Trim().Equals("0"))
                    //{
                    //    if (SavePurchRequest())
                    //    {
                    //        ClearFieldsOnly();
                    //        BindGridMain(txtFltDocNo.Text.Trim(), txtFltDocRef.Text.Trim(), Convert.ToInt32(ddlFltDept.SelectedValue), ddlFltStatus.SelectedValue);
                            
                    //    }
                    //}
                    //else
                    //{
                    //    ucMessage.ShowMessage("Please enter item details completely in the grid", RMS.BL.Enums.MessageType.Error);
                    //}
               
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltDocNo.Text.Trim(), txtFltDocRef.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void grdPurchReq_SelectedIndexChanged(object sender, EventArgs e)
        {

            ID = Convert.ToInt32(grdPurchReq.SelectedDataKey.Values["vr_id"]);
            IsEdit = true;
            GetByID();
           BindGridMain(txtFltDocNo.Text, txtFltDocRef.Text, ddlFltStatus.SelectedValue);
            
        }
        protected void grdPurchReq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
                    if (PID == 927)
                        e.Row.Cells[4].Text = "Confirmed";
                    else if (PID == 922)
                        e.Row.Cells[4].Text = "Approved";
                    else
                        e.Row.Cells[4].Text = "";
                    e.Row.Cells[6].Text = "";
                }
                else if (e.Row.Cells[4].Text.Equals("P"))
                {
                    e.Row.Cells[4].Text = "Pending";
                }
                else if (e.Row.Cells[4].Text.Equals("C"))
                {
                    e.Row.Cells[5].Text = "Cancelled";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

            }
        }
        protected void grdPurchReq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPurchReq.PageIndex = e.NewPageIndex;
           BindGridMain(txtFltDocNo.Text.Trim(), txtFltDocRef.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            try
            {
                int vrId = 0;
                char status = 'C';
                LinkButton lnkPrint = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)lnkPrint.NamingContainer;
                int rowIndex = gvRow.RowIndex;

                if (grdPurchReq.Rows[rowIndex].Cells[4].Text.Equals("Approved") || grdPurchReq.Rows[rowIndex].Cells[4].Text.Equals("Confirmed"))
                {
                    status = 'A';
                }
                else if (grdPurchReq.Rows[rowIndex].Cells[4].Text.Equals("Pending"))
                {
                    status = 'P';
                }
                else
                {
                    status = 'C';
                }


                vrId = Convert.ToInt32(grdPurchReq.DataKeys[rowIndex].Value);
                if (vrId > 0)
                {
                    PrintSaleRequest(vrId, status);   
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        protected void lnkApprove_Click(object sender, EventArgs e)
        {
            try
            {
                int vrId = 0;
                LinkButton lnkPrint = (LinkButton)sender;
                GridViewRow gvRow = (GridViewRow)lnkPrint.NamingContainer;
                int rowIndex = gvRow.RowIndex;

                vrId = Convert.ToInt32(grdPurchReq.DataKeys[rowIndex].Value);
                if (vrId > 0)
                {
                   // PrintSaleRequest(vrId, status);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion

        #region Helping Method

        private void GetByID()
        {

            tblSdSaleReq stkD = saleReqBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            txtDocRef.Text = stkD.DocRef;
            if (Session["DateFormat"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    txtDte.Text = stkD.vr_dt.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                txtDte.Text = stkD.vr_dt.ToString(Session["DateFormat"].ToString());
            }
            hdnCustomer.Value = stkD.Cust_Cd;
            txtCustomer.Text = saleReqBL.GetCustomerByID(stkD.Cust_Cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            txtUser.Text = stkD.CreatedBy;
            ddlStatus.SelectedValue = stkD.vr_apr.ToString();
            
            if (stkD.vr_apr.ToString().Equals("P"))
            {
                ucButtons.EnableSave();
            }
            else
            {
                ucButtons.DisableSave();
            }
            if (IsEdit)
            {
                txtDocNo.Text = stkD.vr_no.ToString().Substring(0, 4) + "/" + stkD.vr_no.ToString().Substring(4);
            }
            
            DocNoFormated = stkD.vr_no.ToString().Substring(0, 4) + "/" + stkD.vr_no.ToString().Substring(4);
            DocNo = stkD.vr_no;

            //BindTableEdit(stkD.tblSdSaleReqDets);

        }
        private void ClearFields()
        {
            Response.Redirect("~/sales/salereq.aspx?PID=922");

        }
        private void ClearFieldsOnly()
        {
            ID = 0;
            txtDocNo.Text = "";
            txtDocRef.Text = "";
            txtCustomer.Text = "";
            hdnCustomer.Value = "";
            txtUser.Text = Session["LoginID"].ToString();
            txtDte.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yy");
            ddlStatus.SelectedValue = "P";
            DocNoFormated = "";
            DocNo = 0;
            IsEdit = false;
            //GetDocNo();
            BindTable();

        }
        private bool SaveSaleRequest()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }

            tblSdSaleReq saleReq = null;

            if (ID == 0)
            {
                saleReq = new tblSdSaleReq();
            }
            else
            {
                saleReq = saleReqBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }

            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    saleReq.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                saleReq.br_id = Convert.ToInt32(Session["BranchID"]);
            }
            saleReq.Cust_Cd = hdnCustomer.Value;
            saleReq.Status = "OP";
            if (!IsEdit)
            {
                GetDocNo();
            }
            
            //string no = txtDocNo.Text.Substring(5);
            //string yrno = txtDocNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(no));
            saleReq.vr_no = DocNo;//Convert.ToInt32(yrno); // Doc No

            // = Convert.ToInt32(txtDocNo.Text.Trim());

            try
            {
                saleReq.vr_dt = Convert.ToDateTime(txtDte.Text.Trim());
            }
            catch 
            {
                ucMessage.ShowMessage("Invalid Date", RMS.BL.Enums.MessageType.Error);
                txtDte.Focus();
                return false;
            }
            
            saleReq.DocRef = txtDocRef.Text.Trim();
            saleReq.vr_nrtn = "";
            if (ddlStatus.SelectedValue != "A")
            {
                saleReq.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                saleReq.CreatedBy = Session["LoginID"].ToString();
            }
            else
            {
                saleReq.ApprovedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                saleReq.ApprovedBy = Session["LoginID"].ToString();
            }
            saleReq.vr_apr = Convert.ToString(ddlStatus.SelectedValue);

            EntitySet<tblSdSaleReqDet> reqDets = new EntitySet<tblSdSaleReqDet>();
            string item, qty, dueDte;
            tblSdSaleReqDet rdet;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text;
                dueDte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDueDate")).Text;
                item = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                //uom = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlUom")).SelectedValue;

                if (!qty.Equals("") && !dueDte.Equals("") && !item.Equals("0"))
                {
                    rdet = new tblSdSaleReqDet();
                    rdet.vr_seq = Convert.ToByte(i + 1);
                    rdet.itm_cd = item;
                    rdet.vr_id = saleReq.vr_id;
                    rdet.vr_qty_aprvd = 0;
                    rdet.vr_uom = Convert.ToByte(GetItemUOM(item));
                    try
                    {
                        rdet.vr_qty_req = Convert.ToDecimal(qty);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid Qty Reqd ", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    try
                    {
                        rdet.vr_exp_date = Convert.ToDateTime(dueDte);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid due date", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }
                    
                    reqDets.Add(rdet);
                }
            }

            if (reqDets == null || reqDets.Count < 1)
            {
                Response.Redirect("~/login.aspx");
            }

            if (ID == 0)
            {
                //saleReq.tblSdSaleReqDets = reqDets;
                string msg = saleReqBL.SaveSaleReqFull(saleReq, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("Doc No "+DocNoFormated+" "+GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                    
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
           
                }
            }
            else
            {
                string msg = saleReqBL.UpdSaleReqFull(saleReq, reqDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
            IsEdit = false;
            return true;
        }
        private byte GetItemUOM(string item)
        {
            byte uom = 0;
            uom = saleReqBL.GetItemUOM(item, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            return uom;
        }
        private void BindGridMain(string docNo, string docRefNo, string status)
        {
            string txt = "fromdt";
            try
            {
                Convert.ToDateTime(txtFltFromDt.Text.Trim());
                txt = "todt";
                Convert.ToDateTime(txtFltToDt.Text.Trim());
            }
            catch
            {
                if (txt == "fromdt")
                {
                    ucMessage.ShowMessage("Invalid from date", RMS.BL.Enums.MessageType.Error);
                }
                else
                {
                    ucMessage.ShowMessage("Invalid to date", RMS.BL.Enums.MessageType.Error);
                }
                grdPurchReq.DataSource = null;
                grdPurchReq.DataBind();
                return;
            }

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

            grdPurchReq.DataSource = saleReqBL.GetSaleReqs(docNo, docRefNo, status, txtFltCust.Text, Convert.ToDateTime(txtFltFromDt.Text.Trim()), Convert.ToDateTime(txtFltToDt.Text.Trim()), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdPurchReq.DataBind();
        }
        public void GetDocNo()
        {
            //txtDocNo.Text = saleReqBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNoFormated = saleReqBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
        }
        private void PrintSaleRequest(int vrId, char status)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "sales/rdlc/SaleReq.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();

            List<spSaleRegRptResult> purchreq = saleReqBL.GetSaleReqDetail(vrId, status, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ReportDataSource dataSource1 = new ReportDataSource("spSaleRegRptResult", purchreq);

           string rptLogoPath = "";

            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            string company = "";
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

            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Sales Request");
            rpt[1] = new ReportParameter("LogoPath", rptLogoPath);
            rpt[2] = new ReportParameter("CompanyName", company);

            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(rpt);

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(dataSource1);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            filename = string.Format("{0}.{1}", "SaleRequest", "pdf");
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        #endregion

        #region WebMethods

        [WebMethod]
        public static object GetItemDetail(string itemid)
        {
            return new SaleReqBL().wmGetItemDetail(itemid, null);
        }
        [WebMethod]
        public static object GetCustomerDetail(string custinfo)
        {
            return new SaleReqBL().wmGetCustomer(custinfo, null);
        }

        #endregion
    }
}

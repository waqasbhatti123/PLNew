using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data;
using System.Data.Linq;
using System.Web.Services;

namespace RMS.Inv
{
    public partial class STNMgt : BasePage
    {

        #region DataMembers

        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        short VoucherTypeCode = 19; // voucher type code 19 for Store Transfer Note in Vr_Type table

        string srNo, itemCode, uomitem,  qtyiss, qtyhand, cc, rem;

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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "STNMgt").ToString();
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

                BindTable();
                FillDropDownLoc();
                BindGridMain("","0");
                IsEdit = false;
                ddlLoc.Focus();

            }
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
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
            ddlLoc.Enabled = false;
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
                }
                else if (e.Row.Cells[4].Text.Equals("C"))
                {
                    e.Row.Cells[4].Text = "Cancelled";
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
                    srNo = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("lblSr")).Text;
                    itemCode = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                    uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text;
                    qtyiss = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyIss")).Text;
                    qtyhand = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtIssueBal")).Text;
                    cc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyAck")).Text;
                    rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text;

                    d_Row["Sr"] = srNo;
                    d_Row["Item"] = itemCode;
                    d_Row["QtyIssued"] = qtyiss;
                    d_Row["QtyIssueBal"] = qtyhand;
                    d_Row["UOMItem"] = uomitem;
                    d_Row["QtyACK"] = cc;
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
            d_Col.ColumnName = "QtyIssued";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "QtyIssueBal";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "QtyACK";
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
            decimal qtyOnHand = 0;
            
            foreach (var dt in dets)
            {
                if (ddlStatus.SelectedValue == "A")
                {
                    qtyOnHand = Convert.ToDecimal(GetQtyOnHandofItem(dt.itm_cd));
                }
                else
                {
                    qtyOnHand = Convert.ToDecimal(GetQtyOnHandofItem(dt.itm_cd) + Convert.ToDecimal(dt.vr_qty));
                }

                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Item"] = dt.itm_cd;
                d_Row["UOMItem"] = GetItemUOMLabelFromUOMId(dt.vr_uom);
                d_Row["QtyIssued"] = (dt.vr_qty).ToString("0.000");
                d_Row["QtyIssueBal"] = (qtyOnHand).ToString("0.000");
                d_Row["QtyACK"] = "0";
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

        #region WebMethods

        [WebMethod]
        public static object GetItemDetail(string itemid, string  locid)
        {
            return new STNBL().wmGetSTNItemDetail(itemid, Convert.ToInt32(locid), null);
        }

        #endregion

        #region Helping Method

        private void GetByID()
        {

            tblStkData stkD = new STNBL().GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlLoc.SelectedValue = stkD.LocId.ToString();
            ddlToLoc.SelectedValue = stkD.ToLocId.Value.ToString();
            ddlStatus.SelectedValue = stkD.vr_apr.ToString();
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

            BindTableEdit(stkD.tblStkDataDets);

        }
        private bool Save()
        {
            //Validate
            int valCount = 0, grdRowsCount = GridView1.Rows.Count, repeatCount = 0, repeatCount1 = 0;
            string itmcd = "";
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                repeatCount = 0;
                repeatCount1 = 0;
                itmcd = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                if (itmcd != "0"
                    && ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyIss")).Text.Trim() != ""
                    //&& ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyAck")).Text.Trim() != ""
                    )
                {
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

            if (ddlLoc.SelectedValue == ddlToLoc.SelectedValue)
            {
                ucMessage.ShowMessage("From location and to location cannot be same", RMS.BL.Enums.MessageType.Error);
                return false;
            }


            /*****************************************************************************************/



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
                stkData.ToLocId = Convert.ToInt16(ddlToLoc.SelectedValue);
                stkData.vt_cd = VoucherTypeCode; // voucher type code 19 for Store Transfer Note

                if (!IsEdit)
                {
                    GetDocNo();
                }
                stkData.vr_no = DocNo;

            }
            else
            {
                stkData = new STNBL().GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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

            //stkData.DeptId = Convert.ToInt16(ddlDept.SelectedValue);
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
            stkData.Status = "OP";


            EntitySet<tblStkDataDet> stkDataDets = new EntitySet<tblStkDataDet>();
            tblStkDataDet stkDataDet;
            string itemCode, uomitem, qtyiss, rem;//qtyack

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                itemCode = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text.Trim();
                qtyiss = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyIss")).Text.Trim();
                //qtyack = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyAck")).Text.Trim();
                rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text.Trim();

                if (!itemCode.Equals("0") && !qtyiss.Equals("") && !uomitem.Equals(""))//!qtyack.Equals("") && 
                {
                    stkDataDet = new tblStkDataDet();

                    stkDataDet.vr_seq = Convert.ToByte(i + 1);
                    stkDataDet.itm_cd = itemCode;
                    //stkDataDet.vr_pkg = Convert.ToDecimal(packs);
                    //stkDataDet.vr_pkg_uom = Convert.ToByte(uom);
                    //stkDataDet.vr_pkg_Size = Convert.ToDecimal(unitsize);
                    try
                    {
                        stkDataDet.vr_qty = Convert.ToDecimal(qtyiss);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Quantity issued is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    //try
                    //{
                    //    if (stkDataDet.vr_qty < Convert.ToDecimal(qtyack))
                    //    {
                    //        ucMessage.ShowMessage("Qty Issued should be less than or equal to Qty Ack", RMS.BL.Enums.MessageType.Error);
                    //        return false;
                    //    }
                    //}
                    //catch
                    //{
                    //    ucMessage.ShowMessage("Invalid Qty Ack ", RMS.BL.Enums.MessageType.Error);
                    //    return false;
                    //}

                    stkDataDet.vr_qty_Rej = 0;//Convert.ToDecimal(qtyiss) -Convert.ToDecimal(qtyack); ;
                    stkDataDet.vr_uom = GetUOMIdFromLabel(uomitem);
                    stkDataDet.vr_val = 0;

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
                string msg = new STNBL().Save(stkData, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                string msg = new STNBL().Update(stkData, stkDataDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
            grd.DataSource = new STNBL().GetStoreTransferNote(docNo, Convert.ToChar(status), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grd.DataBind();
        }     
        public void GetDocNo()
        {
            //DocNoFormated = new MatIssBL().GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, VoucherTypeCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNoFormated = new MatIssBL().GetDocNo(Convert.ToDateTime(txtDocDate.Text.Trim()), VoucherTypeCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0,4)+DocNoFormated.Substring(5));
        }       
        private byte GetUOMIdFromLabel(string uomitem)
        {
            return new MatIssBL().GetUOMIdFromLabel(uomitem, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private decimal GetQtyOnHandofItem(string itemCode)
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
            decimal qtyOnHand = new STNBL().GetLocBasedQtyOnHandofItem(itemCode, brId, Convert.ToInt32(ddlLoc.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            return qtyOnHand;
        }
        private string GetItemUOMLabelFromUOMId(byte uomid)
        {
            string uomDesc = "";
            uomDesc = new MatIssBL().GetUOMDescFromID(uomid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            return uomDesc;
        }
        public void FillDropDownItem(DropDownList ddlItem)
        {
            //ddlItem.DataSource = new MatIssBL().GetAllItem((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataSource = new ItemCodeBL().GetAllGeneralItems((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataTextField = "itm_dsc";
            ddlItem.DataValueField = "itm_cd";
            ddlItem.DataBind();
        }
        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = new MatIssBL().GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();

            ddlToLoc.DataSource = new MatIssBL().GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlToLoc.DataValueField = "LocId";
            ddlToLoc.DataTextField = "LocName";
            ddlToLoc.DataBind();

        }
        private void ClearFieldsOnly()
        {
            VrID = 0;

            ddlStatus.SelectedValue = "P";
            CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            txtDocNo.Text = "";
            DocNo = 0;
            DocNoFormated = "";

            txtRemarks.Text = "";
            ddlLoc.Enabled = true;
            ddlLoc.SelectedIndex = 0;
            ddlToLoc.SelectedIndex = 0;


            IsEdit = false;
            //GetMatIssDocNo();
            //GetDocRef();
            BindTable();
        }
        private void ClearFields()
        {
            Response.Redirect("~/inv/stnmgt.aspx?PID=551");
        }

        #endregion

       
    }
}

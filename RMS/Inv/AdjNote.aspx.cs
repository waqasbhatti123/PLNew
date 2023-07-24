using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data;
using System.Data.Linq;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace RMS.Inv
{
    public partial class AdjNote : BasePage
    {

        #region DataMembers
        
        AdjBL adjBL = new AdjBL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        short VoucherTypeCode = 27; // voucher type code 17 for Adjustment Note in Vr_Type table
        string srNo, itemCode, uomitem, qtyhand, valhand, qtyadj, valadj, rem;

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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "AdjNote").ToString();
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
        protected void grdAdj_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsEdit = true; 
            VrID = Convert.ToInt32(grdAdj.SelectedDataKey.Values["vr_id"]);
            GetByID();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddItem = (DropDownList)e.Row.FindControl("ddlItem");
                FillDropDownItem(ddItem);
            }
        }
        protected void ddlLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTable();
        }
        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList itemdd = (DropDownList)sender;
            string itcode = itemdd.SelectedValue;

            GridViewRow grdrDropDownRow = ((GridViewRow)itemdd.Parent.Parent);
            TextBox lbluom = (TextBox)grdrDropDownRow.FindControl("txtUomItem");
            TextBox lblQtyHand = (TextBox)grdrDropDownRow.FindControl("txtQtyHand");
            TextBox lblValHand = (TextBox)grdrDropDownRow.FindControl("txtValHand");
            TextBox lblAdjQty = (TextBox)grdrDropDownRow.FindControl("txtAdjQty");
            TextBox lblAdjVal = (TextBox)grdrDropDownRow.FindControl("txtAdjVal");
            if (itcode != "0")
            {
                if (lbluom != null)
                {
                    if (itemdd.SelectedIndex > 0)
                    {
                        lbluom.Text = adjBL.GetItemUOMLabel(itcode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
                    }
                    else
                    {
                        lbluom.Text = "";
                    }
                }
                lblQtyHand.Text = GetQtyOnHandofItem(itcode).ToString("0.000");
                lblValHand.Text = GetValueOnHandofItem(itcode).ToString("0.000");
            }
            else
            {
                lbluom.Text = "";
                lblQtyHand.Text = "";
                lblValHand.Text = "";
                lblAdjQty.Text = "";
                lblAdjVal.Text = "";
            }
            itemdd.Focus();
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
            decimal qtyOnHand = adjBL.GetQtyOnHandofItem(itemCode, brId, Convert.ToInt32(ddlLoc.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            return qtyOnHand;
        }
        private decimal GetValueOnHandofItem(string itemCode)
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
            decimal valOnHand = adjBL.GetValueOnHandofItem(itemCode, brId, Convert.ToInt32(ddlLoc.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            return valOnHand;
        }
        public void FillDropDownItem(DropDownList ddlItem)
        {
            //ddlItem.DataSource = adjBL.GetAllItem((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataSource = new ItemCodeBL().GetAllGeneralItems((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                    srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lblSr")).Text;
                    itemCode = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                    uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text;
                    qtyadj = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtAdjQty")).Text;
                    valadj = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtAdjVal")).Text;
                    qtyhand = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyHand")).Text;
                    valhand = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtValHand")).Text;
                    rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text;
                    
                    d_Row["Sr"] = srNo;
                    d_Row["Item"] = itemCode;
                    d_Row["AdjQty"] = qtyadj;
                    d_Row["AdjVal"] = valadj;
                    d_Row["QtyHand"] = qtyhand;
                    d_Row["ValHand"] = valhand;
                    d_Row["UomItem"] = uomitem;
                    d_Row["Rem"] = rem;
                    
                    
                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;//assigning to viewstate datatable
                BindGrid();
                SetDropDownList();

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
            d_Col.ColumnName = "AdjQty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "AdjVal";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "QtyHand";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "ValHand";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Rem";
            d_table.Columns.Add(d_Col);
            
        }
        public void BindTable()
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
            decimal qtyOnHand = 0, valOnHand = 0;

            foreach (var dt in dets)
            {
                if (ddlStatus.SelectedValue != "A")
                {
                    qtyOnHand = Convert.ToDecimal(GetQtyOnHandofItem(dt.itm_cd) - Convert.ToDecimal(dt.vr_qty));
                }
                else
                {
                    qtyOnHand = Convert.ToDecimal(GetQtyOnHandofItem(dt.itm_cd));
                }
                if (ddlStatus.SelectedValue != "A")
                {
                    valOnHand = Convert.ToDecimal(GetValueOnHandofItem(dt.itm_cd) - Convert.ToDecimal(dt.vr_val));
                }
                else
                {
                    valOnHand = Convert.ToDecimal(GetValueOnHandofItem(dt.itm_cd));
                }
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Item"] = dt.itm_cd;
                d_Row["AdjQty"] = dt.vr_qty.ToString("0.000");
                d_Row["AdjVal"] = dt.vr_val.ToString("0.000");
                d_Row["QtyHand"] = qtyOnHand.ToString("0.000");
                d_Row["ValHand"] = valOnHand.ToString("0.000");
                d_Row["UomItem"] = GetItemUOMLabelFromUOMId(dt.vr_uom);
                d_Row["Rem"] = dt.vr_rmk;
                count++;
                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();

            SetDropDownList();
        }
        private string GetItemUOMLabelFromUOMId(byte uomid)
        {
            string uomDesc = "";
            uomDesc = adjBL.GetUOMDescFromID(uomid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                if (SaveAdjNote())
                {
                    ClearFieldsOnly();
                    BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
                }
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void grdAdj_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[0].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString(Session["DateFormat"].ToString());
                }

                if (e.Row.Cells[3].Text.Equals("A"))
                {
                    e.Row.Cells[3].Text = "Approved";
                }
                else if (e.Row.Cells[3].Text.Equals("P"))
                {
                    e.Row.Cells[3].Text = "Pending";
                }
                else if (e.Row.Cells[3].Text.Equals("C"))
                {
                    e.Row.Cells[3].Text = "Cancelled";
                }
            }
        }
        protected void grdAdj_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAdj.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {

            int index = 0;
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            index = clickedRow.RowIndex;
            GetAdjNote(
                Convert.ToInt32(grdAdj.DataKeys[index].Values["vr_id"])
            );
        }

        #endregion

        #region Helping Method

        public void GetAdjNote(int vrid)
        {
            //ReportViewer reportViewer = new ReportViewer();
            //reportViewer.Visible = false;
            //reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/MatIss.rdlc";
            //reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            //reportViewer.LocalReport.Refresh();

            //List<spMatIssResult> recs = adjBL.GetMatIssRec(vrid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ReportDataSource dataSource = new ReportDataSource("spMatIssResult", recs);

            //string rptLogoPath = "";
            //string company = "";
            //rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            ////company = System.Configuration.ConfigurationManager.AppSettings["CompanyName"].ToString().Trim();
            //if (Session["CompName"] == null)
            //{
            //    if (Request.Cookies["uzr"]["CompName"] == null)
            //    {
            //        Response.Redirect("~/login.aspx");
            //    }
            //    else
            //    {
            //        company = Request.Cookies["uzr"]["CompName"].ToString().ToString();
            //    }
            //}
            //else
            //{
            //    company = Session["CompName"].ToString();
            //}
            //string brName = "", brAddress = "", brTel = "", brFax = "", brNTN = "";
            //try
            //{
            //    int branchid = 0;
            //    if (Session["BranchID"] == null)
            //    {
            //        branchid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
            //    }
            //    else
            //    {
            //        branchid = Convert.ToInt32(Session["BranchID"].ToString());
            //    }

            //    Branch branch = new voucherHomeBL().GetBranch(branchid, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            //    if (branch != null)
            //    {
            //        brName = branch.br_nme;
            //        brAddress = branch.br_address;
            //        brTel = branch.br_tel;
            //        brFax = branch.br_fax;
            //        brNTN = branch.br_ntn;
            //    }
            //}
            //catch { }

            //ReportParameter[] rpt = new ReportParameter[8];
            //rpt[0] = new ReportParameter("ReportName", "Material Issue Note");
            //rpt[1] = new ReportParameter("LogoPath", rptLogoPath);
            //rpt[2] = new ReportParameter("CompanyName", company);

            //rpt[3] = new ReportParameter("brName", brName, false);
            //rpt[4] = new ReportParameter("brAddress", brAddress, false);
            //rpt[5] = new ReportParameter("brTel", brTel, false);
            //rpt[6] = new ReportParameter("brFax", brFax, false);
            //rpt[7] = new ReportParameter("brNTN", brNTN, false);


            //reportViewer.LocalReport.EnableExternalImages = true;
            //reportViewer.LocalReport.Refresh();
            //reportViewer.LocalReport.SetParameters(rpt);

            //reportViewer.LocalReport.DataSources.Clear();
            //reportViewer.LocalReport.DataSources.Add(dataSource);

            //Warning[] warnings;
            //string[] streamids;
            //string mimeType;
            //string encoding;
            //string extension;
            //string filename;
            //byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            //filename = string.Format("{0}.{1}", "MatIss", "pdf");
            //Response.ClearHeaders();
            //Response.Clear();
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            //Response.ContentType = mimeType;
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();

        }
        private void GetByID()
        {

            tblStkData stkD = adjBL.GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlLoc.SelectedValue = stkD.LocId.ToString();
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
        private void ClearFieldsOnly()
        {
            VrID = 0;

            ddlLoc.SelectedValue = "0";
            ddlStatus.SelectedValue = "P";
            CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            txtDocNo.Text = "";
            
            DocNo = 0;
            DocNoFormated = "";
           
            txtRemarks.Text = "";

            IsEdit = false;
            BindTable();
        }
        private void ClearFields()
        {
            Response.Redirect("~/inv/adjnote.aspx?PID=555");
        }
        private bool SaveAdjNote()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }

            //Validate

            if (ddlLoc.SelectedValue == "0")
            {
                ucMessage.ShowMessage("Please, select location", RMS.BL.Enums.MessageType.Error);
                return false;
            }

            int valCount = 0, repeatCount = 0, repeatCount1 = 0, grdRowsCount;
            string itmcd;
            grdRowsCount = GridView1.Rows.Count;
            for (int i = 0; i < grdRowsCount; i++)
            {
                repeatCount = 0;
                repeatCount1 = 0;
                itmcd = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                
                if (itmcd != "0")
                {
                    valCount++;

                    for (int j = 0; j < grdRowsCount; j++)
                    {
                        if (itmcd != "0" && itmcd == ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[j].FindControl("ddlItem")).SelectedValue)
                        {
                            repeatCount++;
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

            /**************************************************************************/


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
                stkData.vt_cd = VoucherTypeCode; // voucher type code 27 for Adjustment Note

                if (!IsEdit)
                {
                    GetAdjNoteNo();
                }
                //string no = txtDocNo.Text.Substring(5);
                //string yrno = txtDocNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(no));
                stkData.vr_no = DocNo;//Convert.ToInt32(yrno); // Doc No

            }
            else
            {
                stkData = adjBL.GetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }

            try
            {
                stkData.vr_dt = Convert.ToDateTime(txtDocDate.Text.Trim()); // date
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
            string itemCode, uomitem, qtyhand, valhand, qtyadj, valadj, rem;

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                itemCode = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlItem")).SelectedValue;
                uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text.Trim();
                qtyadj = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtAdjQty")).Text.Trim();
                valadj = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtAdjVal")).Text.Trim();
                qtyhand = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQtyHand")).Text.Trim();
                valhand = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtValHand")).Text.Trim();
                rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text.Trim();

                if (!valhand.Equals("") && !itemCode.Equals("0")
                        && !qtyadj.Equals("") && !uomitem.Equals(""))
                {
                    stkDataDet = new tblStkDataDet();

                    stkDataDet.vr_seq = Convert.ToByte(i + 1);
                    stkDataDet.itm_cd = itemCode;
                    //stkDataDet.vr_pkg = Convert.ToDecimal(packs);
                    //stkDataDet.vr_pkg_uom = Convert.ToByte(uom);
                    //stkDataDet.vr_pkg_Size = Convert.ToDecimal(unitsize);
                    try
                    {
                        stkDataDet.vr_qty = Convert.ToDecimal(qtyadj);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Quantity is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    try
                    {
                        if (stkDataDet.vr_qty > Convert.ToDecimal(qtyhand))
                        {
                            ucMessage.ShowMessage("Adjustment Qty should be less than or equal to Qty on Hand", RMS.BL.Enums.MessageType.Error);
                            return false;
                        }
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid Adjustment Qty ", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    try
                    {
                        stkDataDet.vr_val = Convert.ToDecimal(valadj);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Value is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    try
                    {
                        if (stkDataDet.vr_val > Convert.ToDecimal(valadj))
                        {
                            ucMessage.ShowMessage("Adjustment Value should be less than or equal to Value on Hand", RMS.BL.Enums.MessageType.Error);
                            return false;
                        }
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid Adjustment Value", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    stkDataDet.vr_qty_Rej = 0;
                    stkDataDet.vr_uom = GetUOMIdFromLabel(uomitem);
                    stkDataDet.CC_cd = null;
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
                string msg = adjBL.SaveAdjNoteFull(stkData, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                string msg = adjBL.UpdateAdjNoteFull(stkData, stkDataDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
        private byte GetUOMIdFromLabel(string uomitem)
        {
           return adjBL.GetUOMIdFromLabel(uomitem, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = adjBL.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
            ddlLoc.SelectedValue = "0";

        }
        public void GetAdjNoteNo()
        {
            //txtDocNo.Text = adjBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 17, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //DocNoFormated = adjBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, VoucherTypeCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNoFormated = adjBL.GetDocNo(Convert.ToDateTime(txtDocDate.Text.Trim()), VoucherTypeCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0,4)+DocNoFormated.Substring(5));
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
            grdAdj.DataSource = adjBL.GetAllAdjNote(docNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdAdj.DataBind();
        }

        #endregion

       
    }
}

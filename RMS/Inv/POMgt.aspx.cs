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
using System.Web;

namespace RMS.Inv
{
    public partial class POMgt : BasePage
    {
        #region DataMembers

        POrderBL poBL = new POrderBL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;

        ReportViewer reportViewer = new ReportViewer();
        private static List<tblPOReqDet> ReqDet;

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
        
#pragma warning disable CS0114 // 'POMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'POMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int PID
        {
            get { return (ViewState["PID"] == null) ? 0 : Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
        public int BrID
        {
            get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
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

        public int ReqID
        {
            get { return (ViewState["ReqID"] == null) ? 0 : Convert.ToInt32(ViewState["ReqID"]); }
            set { ViewState["ReqID"] = value; }
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
            PID = Convert.ToInt32(Request.QueryString["PID"]);
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "POMgt").ToString();
                if (Session["BranchID"] == null)
                {
                    BrID = Convert.ToByte(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BrID = Convert.ToByte(Session["BranchID"].ToString());
                }
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                }
                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender2.Format = Session["DateFormat"].ToString();
                }

                BindTable();
                //GetDocNo();
                FillDropDownVendor();
                FillDropDownWHT();
                FiilDdlCurrency();
                BindGridMain("","","0");
                ddlCurrency.SelectedValue = "PKR";
                IsEdit = false;
                txtReqNoSrch.Attributes.Add("onclick", "javascript:this.select();");
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //DropDownList dditem = (DropDownList)e.Row.FindControl("ddlItem");
                //TextBox req = (TextBox)e.Row.FindControl("txtReqNo");
                //if (req.Text != "99999")
                //{
                //    ((TextBox)e.Row.FindControl("txtItem")).Enabled = false;
                //    ((TextBox)e.Row.FindControl("txtItemDesc")).Enabled = false;
                //}

                DropDownList ddgst = (DropDownList)e.Row.FindControl("ddlGST");
                
                TextBox txtduedte = (TextBox)e.Row.FindControl("txtDueDate");
                if (!txtduedte.Text.Trim().Equals(""))
                {
                    txtduedte.Text = Convert.ToDateTime(txtduedte.Text).ToString("dd-MMM-yy");
                }
                
                //FillDropDownItem(dditem);
                FillGridDropDownGST(ddgst);
            }

        }
        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
            SetDropDownList();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //------------------------
            BindGridMain(txtFltPoNo.Text.Trim(), txtFltParty.Text.Trim(),ddlFltStatus.SelectedValue);
        }
        protected void grdPo_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsEdit = true;

            ID = Convert.ToInt32(grdPo.SelectedDataKey.Values["vr_id"]);
            GetByID();
            BindGridMain(txtFltPoNo.Text.Trim(), txtFltParty.Text.Trim(), ddlFltStatus.SelectedValue);
            //if (ddlStatus.SelectedValue == "A" && ddlStatus.SelectedValue == "C")
            //{
            //    addRow.Visible = false;
            //}
            //else
            //{
            //    addRow.Visible = true;
            //}
            
        }
        protected void grdPo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPo.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltPoNo.Text.Trim(), txtFltParty.Text.Trim(), ddlFltStatus.SelectedValue);
        }
        protected void btnReqNoSrch_Click(object sender, EventArgs e)
        {
            if (aceValue.Value != "")
            {
                ReqID = Convert.ToInt32(aceValue.Value);
            }

            if (ReqID > 0)
            {
                ReqDet = GetRecByID();
                if (ReqDet.Count() > 0)
                {
                    AddReqOrder();
                }
            }
        }
        protected void grdPo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int vrid = Convert.ToInt32(grdPo.DataKeys[e.Row.RowIndex].Value);
                tblPOrder po = poBL.GetByID(vrid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                string status = po.Status == "Op" ? "Open" : "Closed";
                e.Row.Cells[0].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[0].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Session["DateFormat"].ToString());
                }

                if (e.Row.Cells[4].Text.Equals("L"))
                {
                    e.Row.Cells[4].Text = "Local";
                }
                else 
                {
                    e.Row.Cells[4].Text = "Foreign";
                }

                if (e.Row.Cells[5].Text.Equals("A"))
                {
                    e.Row.Cells[5].Text = "Approved/" + status;

                }
                else if (e.Row.Cells[5].Text.Equals("P"))
                {
                    e.Row.Cells[5].Text = "Pending/" + status;
                    e.Row.Cells[7].Text = "";
                }
                else if (e.Row.Cells[4].Text.Equals("C"))
                {
                    e.Row.Cells[5].Text = "Cancelled/" + status;
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }
               
            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            

            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;

            tblPOrder po = poBL.GetByVrID(Convert.ToInt32(grdPo.DataKeys[clickedRow.RowIndex].Value), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (po == null)
            {
                return;
            }
            DocNoFormated = po.vr_no.ToString().Substring(0, 4) + "/" + po.vr_no.ToString().Substring(4);
            DocNo = po.vr_no;
            ID = po.vr_id;
            
            

            string status = clickedRow.Cells[4].Text;

            if (ID > 0)
            {
                reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(setSubDataSource);
                reportViewer.LocalReport.Refresh();

                GetPurchaseOrder();
            }
            else
            {
                ucMessage.ShowMessage("Cannot print with status '"+status+"'.", RMS.BL.Enums.MessageType.Error);
            }

            DocNoFormated = "";
            DocNo = 0;
            ID = 0;
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {
                string itmCd = "", itmNme = "";
                int BrID = 0;
                if (Session["BranchID"] == null)
                {
                    BrID = Convert.ToByte(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BrID = Convert.ToByte(Session["BranchID"].ToString());
                }
                //Validate
                int valCount = 0, repeatCount = 0, repeatCount1 = 0, grdRowsCount;
                string itmcd, qty, rate, dueDte;
                grdRowsCount = GridView1.Rows.Count;
                for (int i = 0; i < grdRowsCount; i++)
                {
                    repeatCount = 0;
                    repeatCount1 = 0;
                    itmcd = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                    qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                    rate = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRate")).Text.Trim();
                    dueDte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDueDate")).Text.Trim();
                    if (itmcd != "" && qty != "" && rate != "" && dueDte != "")
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
                            if (itmcd != "" && itmcd == ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtItem")).Text)
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
                /***********************************************************************/
                if (ddlStatus.SelectedValue == "C")
                {
                    bool canCencel = true;
                    if (BrID > 0)
                    {
                        for (int i = 0; i < grdRowsCount; i++)
                        {
                            itmCd = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                            itmNme = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItemDesc")).Text;

                            if (!string.IsNullOrEmpty(itmCd) && !string.IsNullOrEmpty(txtPoNo.Text.Trim()) && !poBL.CanPOBeCancelled(BrID, Convert.ToInt32(txtPoNo.Text.Trim().Replace("/", "")), itmCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                            {
                                canCencel = false;
                                break;
                            }
                        }
                        if (!canCencel)
                        {
                            ucMessage.ShowMessage("PO Cannot be cancelled as " + itmNme + " has been received", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                /***********************************************************************/
                if (!string.IsNullOrEmpty(txtPoNo.Text.Trim()) && !string.IsNullOrEmpty(txtPoRev.Text.Trim()))
                {
                    bool canRevised = true;
                    
                    if (BrID > 0)
                    {
                        for (int i = 0; i < grdRowsCount; i++)
                        {
                            itmCd = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                            itmNme = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItemDesc")).Text;
                            qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();

                            if (!string.IsNullOrEmpty(itmCd) && !poBL.CanRevisionBeDone(BrID, Convert.ToInt32(txtPoNo.Text.Trim().Replace("/", "")), itmCd, Convert.ToDecimal(qty), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                            {
                                canRevised = false;
                                break;
                            }
                        }
                        if (!canRevised)
                        {
                            ucMessage.ShowMessage("PO Cannot be revised as " + itmNme + " quantity is less than received quantity", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                /***********************************************************************/
                for (int i = 0; i < grdRowsCount; i++)
                {
                    itmcd = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text.Trim();

                    if (itmcd != "" && !new ItemCodeBL().IsItemExists(itmcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage(itmcd + " does not exist in database", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                }
                /***********************************************************************/
                if (SavePO())
                {
                    ClearFieldsOnly();
                    BindGridMain(txtFltPoNo.Text.Trim(), txtFltParty.Text.Trim(), ddlFltStatus.SelectedValue);

                }

            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }

        #endregion

        #region Helping Method

        public void setSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            int vrId = Convert.ToInt32(e.Parameters[0].Values[0]);
            string currency = e.Parameters[1].Values[0];

            List<spPORptResult> res = poBL.GetPORptRes(vrId, 'A',(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            e.DataSources.Add(new ReportDataSource("spPORptResult", res));
        }
        public void GetPurchaseOrder()
        {
            string showGST="No", vendor = "", brName = "", brAddress = "", brTel = "", brFax = "", brNTN = "", brSTN = "", updateby, approvedby;
            decimal gst = 0;
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/PO.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
 
            List<Anonymous4PO> po = poBL.GetPORec(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<spPORptResult> res = poBL.GetPORptRes(Convert.ToInt32(po.First().vr_id), 'A', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            foreach (var r in res)
            {
                gst = gst + r.GST;
                if (r.GSTid == "VAR")
                {
                    showGST = "Yes";
                    break;
                }
            }
            if (gst > 0)
                showGST = "Yes";
            ReportDataSource dataSource = new ReportDataSource("Anonymous4PO", po);
            vendor = po.First().vendor;
            string rptLogoPath = "";
            string company="";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            
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
           
            updateby = po.First().CreatedBy;
            approvedby = po.First().ApprovedBy;
            try
            {
                int brid = 0;
                if (Session["BranchID"] == null)
                {
                    brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
                }
                else
                {
                    brid = Convert.ToInt32(Session["BranchID"].ToString());
                }
                Branch branch = new voucherHomeBL().GetBranch(brid, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                if (branch != null)
                {
                    brName = branch.br_nme;
                    brAddress = branch.br_address;
                    brTel = branch.br_tel;
                    brFax = branch.br_fax;
                    brNTN = branch.br_ntn;
                    brSTN = branch.br_stx_no;
                }
            }
            catch { }

            ReportParameter[] rpt = new ReportParameter[14];
            rpt[0] = new ReportParameter("ReportName", "Purchase Order");
            rpt[1] = new ReportParameter("LogoPath", rptLogoPath);
            rpt[2] = new ReportParameter("CompanyName", company);

            rpt[3] = new ReportParameter("brName", brName, false);
            rpt[4] = new ReportParameter("brAddress", brAddress, false);
            rpt[5] = new ReportParameter("brTel", brTel, false);
            rpt[6] = new ReportParameter("brFax", brFax, false);
            rpt[7] = new ReportParameter("brNTN", brNTN, false);
            rpt[8] = new ReportParameter("brSTN", brSTN, false);

            rpt[9] = new ReportParameter("AprBy", approvedby, false);
            rpt[10] = new ReportParameter("UpdBy", updateby, false);
            rpt[11] = new ReportParameter("vendor", vendor, false);
            rpt[12] = new ReportParameter("showGST", showGST, false);
            rpt[13] = new ReportParameter("currency", po.First().currency, false);

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
            filename = string.Format("{0}.{1}", "PurchaseOrder", "pdf");
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }
        public void AddReqOrder()
        {
            int count = 1;
            int tblcount = 0;

            if (CurrentTable != null)
            {
                UpdateTable();
                d_table = CurrentTable;
                tblcount = d_table.Rows.Count;

                    foreach (tblPOReqDet rcDt in ReqDet)
                    {
                        tblPOReq rc = GetPOReqById(rcDt.vr_id);
                        if(rc != null)
                        {
                            d_Row = d_table.NewRow();

                            d_Row["Sr"] = tblcount + count;
                            d_Row["ReqNo"] = rc.vr_no;
                            d_Row["Item"] = rcDt.itm_cd;
                            d_Row["ItemDesc"] = new ItemCodeBL().GetItemDescByItemCode(rcDt.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            d_Row["PoQuantity"] = Convert.ToInt32(rcDt.vr_qty_req);
                            d_Row["Spec"] = "";
                            d_Row["UOM"] = GetItemUomDesc(rcDt.vr_uom);
                            d_Row["Amnt"] = "";
                            d_Row["GST"] = "0";
                            d_Row["Quantity"] = "";
                            d_Row["Rate"] = "";
                            d_Row["DueDate"] = rcDt.vr_exp_date;

                            d_table.Rows.Add(d_Row);
                            count++;
                        }
                    }

                CurrentTable = d_table;
                BindGrid();
                SetDropDownList();
                ReqID = 0;
            }
        }
        private void GetByID()
        {

            tblPOrder po = poBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlPoType.SelectedValue = po.PO_Type;
            ddlStatus.SelectedValue = po.vr_apr.ToString();
            txtPoRev.Text = po.RevSeq.Value == 0 ? "" : po.RevSeq.Value.ToString();
            
            if (IsEdit)
            {
                txtPoNo.Text = po.vr_no.ToString().Substring(0, 4) + "/" + po.vr_no.ToString().Substring(4);
            }
            DocNoFormated = po.vr_no.ToString().Substring(0, 4) + "/" + po.vr_no.ToString().Substring(4);
            DocNo = po.vr_no;

            ddlCurrency.SelectedValue = po.CRUNCY;

            ddlVendor.SelectedValue = po.VendorId;
            ddlShipment.SelectedValue = po.Ship_Mode;

            txtDelLoc.Text = po.Del_Loc;
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
            if (po.Del_Days != null && po.Del_Days != 0)
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                txtDelPeriod.Text = po.Del_Days.ToString();
            else
                txtDelPeriod.Text = "";
            txtPayDays.Text = (po.Pay_Days != null && po.Pay_Days.Value > 0) ? po.Pay_Days.Value.ToString() : "";
            txtPayTerms.Text = po.Pay_Terms;
            txtTerms.Text = po.vr_nrtn;
            txtInst.Text = po.PO_Instructions;
            txtQtyVar.Text = (po.QTY_Var_Pc != null && po.QTY_Var_Pc.Value > 0) ? po.QTY_Var_Pc.Value.ToString() :"";

            txtVendorDocRef.Text = string.IsNullOrEmpty(po.DocRef) ? "" : po.DocRef;
            
            if (po.WHTid != null)
            {
                ddlWHT.SelectedValue = po.WHTid;
            }
            else
            {
                ddlWHT.SelectedValue = "0";
            }
            
            if (po.DocRefDate != null)
            {
                txtRefDate.Text = po.DocRefDate.Value.ToString("dd-MMM-yy");
            }
            else
            {
                txtRefDate.Text = "";
            }

            if (Session["DateFormat"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    txtPoDate.Text = po.vr_dt.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                txtPoDate.Text = po.vr_dt.ToString(Session["DateFormat"].ToString());
            }

            if (po.vr_apr.ToString().Equals("P"))
            {
                ucButtons.EnableSave();
            }
            else
            {
                ucButtons.DisableSave();
            }


            BindTableEdit(po.tblPOrderDets);

        }
        private void ClearFields()
        {
            Response.Redirect("~/inv/pomgt.aspx?PID="+PID);

        }
        private void ClearFieldsOnly()
        {
            ID = 0;
            ReqID = 0;

            ddlPoType.SelectedIndex = 1;
            ddlSupType.SelectedIndex = 1;
            ddlCurrency.SelectedValue = "PKR";
            ddlWHT.SelectedValue = "0";
            txtPoDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yy");
            ddlStatus.SelectedValue = "P";
            ddlVendor.SelectedIndex = 0;
            ddlShipment.SelectedIndex = 1;
            txtDelLoc.Text = "";
            txtDelPeriod.Text = "";
            txtPayDays.Text = "";
            txtQtyVar.Text = "";
            txtPayTerms.Text = "";
            txtTerms.Text = "";
            txtInst.Text = "";
            txtPoNo.Text = "";
            DocNo = 0;
            DocNoFormated = "";
            txtReqNoSrch.Text = "";
            txtVendorDocRef.Text = "";
            txtRefDate.Text = "";
            txtPoRev.Text = "";
            txtOverAllDisc.Text = "";

            Session["POREF"] = null;

            IsEdit = false;
            BindTable();
        }
        private bool SavePO()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            
            /////////////////////////////////////////////////////////////
            //GET TOTAL AMOUNT OF PO
            /////////////////////////////////////////////////////////////
            decimal totalAmount = 0;
            string reqNo1, item1, gst1, spec1, qty1, rate1, dueDte1;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                reqNo1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtReqNo")).Text.Trim();
                item1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                gst1 = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlGST")).SelectedValue;
                spec1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSpec")).Text.Trim();
                qty1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                rate1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRate")).Text.Trim();
                dueDte1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDueDate")).Text.Trim();

                if (!reqNo1.Trim().Equals("") && !dueDte1.Trim().Equals("") && !item1.Trim().Equals("")
                      && !qty1.Trim().Equals("") && !rate1.Trim().Equals(""))//&& !spec.Trim().Equals("")
                {
                    totalAmount = totalAmount + Convert.ToDecimal(qty1) * Convert.ToDecimal(rate1);
                }
            }
            /////////////////////////////////////////////////////////////

            tblPOrder porder = null;

            if (ID == 0)
            {
                porder = new tblPOrder();
            }
            else
            {
                porder = poBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }

            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    porder.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                porder.br_id = Convert.ToInt32(Session["BranchID"]);
            }

            if (!IsEdit)
            {
                GetDocNo();
            }
            porder.vr_no = DocNo; // PO No
            //porder.Org_vr_no = DocNo;
            /***************************************/
            if (porder.RevOf == null)
            {
                porder.RevSeq = 0;
            }
            if (ddlStatus.SelectedValue.Equals("C"))
            {
                porder.Status = "Cl";
            }
            else
            {
                porder.Status = "Op";
            }
            /***************************************/
            try
            {
                if (txtQtyVar.Text.Equals(""))
                {
                    porder.QTY_Var_Pc = 0;
                }
                else
                {
                    porder.QTY_Var_Pc = Convert.ToDecimal(txtQtyVar.Text.Trim());
                }
            }
            catch
            {
                ucMessage.ShowMessage("Invalid Quantity Variance %", RMS.BL.Enums.MessageType.Error);
                txtQtyVar.Focus();
                return false;
            }

            porder.PO_Nature = ddlSupType.SelectedValue;

            try
            {
                porder.vr_dt = Convert.ToDateTime(txtPoDate.Text.Trim());
            }
            catch
            {
                ucMessage.ShowMessage("Invalid Date", RMS.BL.Enums.MessageType.Error);
                txtPoDate.Focus();
                return false;
            }

            //poReq.DocRef = txtDocRef.Text.Trim();
            //poReq.DeptId = Convert.ToInt16(ddlDept.SelectedValue);
            //poReq.CC_cd = ddlCostCenter.SelectedValue;

            porder.VendorId = ddlVendor.SelectedValue;

            porder.PO_Type = ddlPoType.SelectedValue;
            if (ddlCurrency.SelectedValue != "0")
            {
                porder.CRUNCY = ddlCurrency.SelectedValue;
            }
            try
            {
                if (txtDelPeriod.Text.Trim() != "")
                    porder.Del_Days = Convert.ToByte(txtDelPeriod.Text.Trim());
                else
                    porder.Del_Days = 0;
            }
            catch
            {
                ucMessage.ShowMessage("Invalid Delivery period", RMS.BL.Enums.MessageType.Error);
                txtDelPeriod.Focus();
                return false;
            }

            try
            {
                if (txtPayDays.Text.Trim().Equals(""))
                {
                    porder.Pay_Days = 0;
                }
                else
                {
                    porder.Pay_Days = Convert.ToByte(txtPayDays.Text.Trim());
                }
            }
            catch
            {
                ucMessage.ShowMessage("Invalid payment days", RMS.BL.Enums.MessageType.Error);
                txtPayDays.Focus();
                return false;
            }

            porder.Pay_Terms = txtPayTerms.Text.Trim();
            porder.Del_Loc = txtDelLoc.Text.Trim();
            if (ddlShipment.SelectedIndex > 0)
            {
                porder.Ship_Mode = ddlShipment.SelectedValue;
            }
            else
            {
                porder.Ship_Mode = null;
            }

            porder.DocRef = txtVendorDocRef.Text;
            if (txtRefDate.Text != "")
            {
                try
                {
                    porder.DocRefDate = Convert.ToDateTime(txtRefDate.Text);
                }
                catch
                {
                    ucMessage.ShowMessage("Invalid ref date", RMS.BL.Enums.MessageType.Error);
                    txtRefDate.Focus();
                    return false;
                }
            }
            else
            {
                porder.DocRefDate = null;
            }

            porder.PO_Instructions = txtInst.Text.Trim();
            porder.vr_nrtn = txtTerms.Text.Trim();

            
            int uzrid = 0;
            if (Session["LoginID"] == null)
            {
                uzrid = Convert.ToInt32( Request.Cookies["uzr"]["UserID"].ToString());
            }
            else
            {
                uzrid = Convert.ToInt32(Session["UserID"].ToString());
            }
            string username = poBL.GetUserNameByID(uzrid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (ddlStatus.SelectedValue.Equals("A"))
            {
                if (porder.CreatedBy == null)
                {
                    porder.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    porder.CreatedBy = username;
                }
                porder.ApprovedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                porder.ApprovedBy = username;
            }
            else
            {
                porder.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                porder.CreatedBy = username;
            }
            
            if (ddlWHT.SelectedValue != "0")
            {
                porder.WHTid = ddlWHT.SelectedValue;
            }
            else
            {
                porder.WHTid = null;
            }
            porder.vr_apr = ddlStatus.SelectedValue;

            EntitySet<tblPOrderDet> reqDets = new EntitySet<tblPOrderDet>();
            string item, qty, dueDte, reqNo, spec, rate, gst;
            tblPOrderDet rdet;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                reqNo = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtReqNo")).Text.Trim();
                item = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                gst = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlGST")).SelectedValue;
                spec = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSpec")).Text.Trim();
                qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                rate = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRate")).Text.Trim();
                dueDte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDueDate")).Text.Trim();

                if (!reqNo.Trim().Equals("") && !dueDte.Trim().Equals("") && !item.Trim().Equals("")
                      && !qty.Trim().Equals("") && !rate.Trim().Equals(""))//&& !spec.Trim().Equals("")
                {
                    rdet = new tblPOrderDet();
                    rdet.vr_seq = Convert.ToByte(i + 1);
                    rdet.vr_Req_Ref = reqNo;
                    rdet.itm_cd = item;
                    rdet.vr_uom = GetItemUOM(item);
                    rdet.vr_id = porder.vr_id;
                    rdet.vr_specs = spec;
                    rdet.GSTid = gst;
                    try
                    {
                        rdet.vr_qty = Convert.ToDecimal(qty);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid Qty Reqd ", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    try
                    {
                        rdet.vr_Rate = Convert.ToDecimal(rate);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid rate ", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }
                    try
                    {
                        rdet.vr_sch_date = Convert.ToDateTime(dueDte);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid due date", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }
                    if (!string.IsNullOrEmpty(txtOverAllDisc.Text.Trim()))
                        rdet.overall_disc = Math.Round(Convert.ToDecimal(qty) * Convert.ToDecimal(rate) * Convert.ToDecimal(txtOverAllDisc.Text.Trim()) / totalAmount, 2);
                    else
                        rdet.overall_disc = 0;


                    reqDets.Add(rdet);
                }
            }
            if (reqDets == null || reqDets.Count < 1)
            {
                Response.Redirect("~/login.aspx");
            }
            if (ID == 0)
            {
                porder.tblPOrderDets = reqDets;
                string msg = poBL.SavePurchReqFull(porder, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("PO No " + DocNoFormated + " " + GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
                    return false;
                }
            }
            else
            {
                string msg = poBL.UpdPurchReqFull(porder, reqDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("PO No " + DocNoFormated + " " + GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
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
        private byte GetItemUOM(string itemcd)
        {
            return poBL.GetItemUOM(itemcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private string GetItemUomDesc(int uomcd)
        {
            return new UomBL().GetUomDescByID(uomcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public void FillDropDownVendor()
        {
            ddlVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.DataTextField = "gl_dsc";
            ddlVendor.DataValueField = "gl_cd";
            ddlVendor.DataBind();
        }
        public void FillGridDropDownGST(DropDownList ddlGST)
        {
            ddlGST.DataSource = new TaxBL().GetGSTTaxes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlGST.DataTextField = "TaxDesc";
            ddlGST.DataValueField = "TaxID";
            ddlGST.DataBind();
        }
        public void FillDropDownWHT()
        {
            ddlWHT.DataSource = new TaxBL().GetWHTTaxes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlWHT.DataTextField = "TaxDesc";
            ddlWHT.DataValueField = "TaxID";
            ddlWHT.DataBind();
        }
        private void BindGridMain(string poNo, string partyNme, string status)
        {
            if (!poNo.Equals(""))
            {
                if (poNo.Contains("/") && poNo.Length > 5)
                {
                    try
                    {
                        char[] st = poNo.ToCharArray();
                        if (st[4].ToString().Equals("/"))
                        {
                            poNo = poNo.Substring(0, 4) + poNo.Substring(5);
                        }
                        else
                        {
                            poNo = "";
                            for (int i = 0; i < st.Length; i++)
                            {
                                if (!st[i].ToString().Equals("/"))
                                {
                                    poNo = poNo + st[i];
                                }
                            }
                        }
                    }
                    catch { }

                }
                else if (poNo.Contains("/"))
                {
                    char[] st = poNo.ToCharArray();
                    poNo = "";
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (!st[i].ToString().Equals("/"))
                        {
                            poNo = poNo + st[i];
                        }
                    }
                }
                else
                {
                    //nothin
                }
            }

            grdPo.DataSource = poBL.GetPOrders(BrID, poNo, partyNme, status, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdPo.DataBind();
        }
        public void GetDocNo()
        {
            //DocNoFormated = poBL.GetPoNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNoFormated = poBL.GetPoNo(Convert.ToDateTime(txtPoDate.Text.Trim()).Date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
        }
        public List<tblPOReqDet> GetRecByID()
        {
            return poBL.GetRecDetById( ReqID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public tblPOReq GetPOReqById(int id)
        {
            return poBL.GetPOReq(id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public void FiilDdlCurrency()
        {
            ddlCurrency.DataSource = new CurrencyBL().GetAllCurrencies((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCurrency.DataTextField = "CRUNCY";
            ddlCurrency.DataValueField = "CRUNCY";
            ddlCurrency.DataBind();
        }
        
        #endregion

        #region GridView

        public void AddRow()
        {
            if (CurrentTable != null)
            {
                d_table = CurrentTable;
                for (int i = 0; i < 5; i++)
                {
                    d_Row = d_table.NewRow();
                    d_Row["Sr"] = d_table.Rows.Count + 1;
                    d_Row["ReqNo"] = "99999";
                    d_Row["PoQuantity"] = "0";
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
                string srNo, reqno, itemCode, itemDesc, spec, amnt, qty, rate, duedte, uom, poqty, gst;

                GetColumns();
                rowsCount = CurrentTable.Rows.Count;
                for (int i = 0; i < rowsCount; i++)
                {
                    d_Row = d_table.NewRow();
                    srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lblSr")).Text;
                    reqno = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtReqNo")).Text;
                    itemCode = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                    itemDesc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItemDesc")).Text;
                    gst = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlGST")).SelectedValue;
                    spec = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSpec")).Text;
                    uom = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomItem")).Text;
                    amnt = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtAmnt")).Text;
                    qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text;
                    poqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPoQuantity")).Text;
                    rate = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRate")).Text;
                    duedte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDueDate")).Text;

                    if (!reqno.Trim().Equals(""))// && !itemCode.Trim().Equals("0")&& !duedte.Trim().Equals("")  && !spec.Trim().Equals("")&& !qty.Trim().Equals("") && !rate.Trim().Equals("")
                    {

                        d_Row["Sr"] = srNo;
                        d_Row["ReqNo"] = reqno;
                        d_Row["Item"] = itemCode;
                        d_Row["ItemDesc"] = itemDesc;
                        d_Row["GST"] = gst;
                        d_Row["Spec"] = spec;
                        d_Row["UOM"] = uom;
                        d_Row["Amnt"] = amnt;
                        if (qty != "")
                            d_Row["Quantity"] = qty;
                        if (poqty != "")
                            d_Row["PoQuantity"] = poqty;


                        d_Row["Rate"] = rate;
                        if (duedte != "")
                            d_Row["DueDate"] = duedte;

                        d_table.Rows.Add(d_Row);
                    }
                }
                CurrentTable = d_table;//assigning to viewstate datatable
                //BindGrid();
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
            d_Col.ColumnName = "ReqNo";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Item";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "ItemDesc";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "GST";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UOM";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Spec";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Amnt";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Quantity";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "PoQuantity";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Rate";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.DateTime");
            d_Col.ColumnName = "DueDate";
            d_table.Columns.Add(d_Col);

        }
        public void BindTable()
        {
            GetColumns();
            for (int i = 0; i < 10; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;
                d_Row["ReqNo"] = "99999";
                d_Row["PoQuantity"] = "0";

                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }
        public void BindTableEdit(EntitySet<tblPOrderDet> dets)
        {
            GetColumns();
            int count = 0;
            decimal overallDiscount = 0;
            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["ReqNo"] = dt.vr_Req_Ref;
                d_Row["Item"] = dt.itm_cd;
                d_Row["ItemDesc"] = new ItemCodeBL().GetItemDescByItemCode(dt.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                d_Row["UOM"] = GetItemUomDesc(dt.vr_uom);
                d_Row["Spec"] = dt.vr_specs;
                d_Row["Quantity"] = dt.vr_qty.ToString();
                if (dt.vr_Req_Ref == "99999")
                    d_Row["PoQuantity"] = "0";
                else
                    d_Row["PoQuantity"] = poBL.GetPOQty(dt.itm_cd, Convert.ToInt32(dt.vr_Req_Ref), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
                d_Row["Rate"] = dt.vr_Rate.ToString();
                d_Row["Amnt"] = (dt.vr_qty * dt.vr_Rate).ToString("F");
                if (dt.vr_sch_date != null)
                {
                    d_Row["DueDate"] = dt.vr_sch_date.Value.ToString("dd-MMM-yy");
                }
                if (dt.GSTid != null)
                {
                    d_Row["GST"] = dt.GSTid;
                }
                else
                {
                    d_Row["GST"] = "0";
                }
                overallDiscount = overallDiscount + Convert.ToDecimal(dt.overall_disc);

                count++;
                d_table.Rows.Add(d_Row);

            }
            CurrentTable = d_table;

            txtOverAllDisc.Text = overallDiscount.ToString();

            BindGrid();

            SetDropDownList();
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }
        public void SetDropDownList()
        {
            rowsCount = CurrentTable.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                //DropDownList dditem = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlItem"));
                DropDownList ddgst = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[9].FindControl("ddlGST"));
                if (i < CurrentTable.Rows.Count)
                {
                    //if (CurrentTable.Rows[i]["Item"] != DBNull.Value)
                    //{
                    //    dditem.ClearSelection();
                    //    dditem.Items.FindByValue(CurrentTable.Rows[i]["Item"].ToString()).Selected = true;
                    //}
                    if (CurrentTable.Rows[i]["GST"] != DBNull.Value)
                    {
                        ddgst.ClearSelection();
                        ddgst.Items.FindByValue(CurrentTable.Rows[i]["GST"].ToString()).Selected = true;
                    }
                }
            }
        }
        //public void FillDropDownItem(DropDownList ddlItem)
        //{
        //    //ddlItem.DataSource = poBL.GetAllItem((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlItem.DataSource = new ItemCodeBL().GetAllGeneralItems((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlItem.DataTextField = "itm_dsc";
        //    ddlItem.DataValueField = "itm_cd";
        //    ddlItem.DataBind();
        //}

        #endregion

        #region WebMethods

        [WebMethod]
        public static object GetItemDetail(string itemid)
        {
            //return new PurchReqBL().wmGetItemDetail(itemid, null);
            int brid = 0;
            if (HttpContext.Current.Session["BranchID"] == null)
            {
                brid = Convert.ToInt32(HttpContext.Current.Request.Cookies["uzr"]["BranchID"]);
            }
            else
            {
                brid = Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            }

            return new ItemCodeBL().GetSearchItems(brid, itemid, "UG", 0, null);
        }

        #endregion
    }
}

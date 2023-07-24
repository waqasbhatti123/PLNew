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
    public partial class PurchReqMgt : BasePage
    {

        #region DataMembers

        PurchReqBL purchReqBL = new PurchReqBL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;

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

#pragma warning disable CS0114 // 'PurchReqMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'PurchReqMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public int PID
        {
            get { return (ViewState["PID"] == null) ? 0 : Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
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
            PID = Convert.ToInt32(Request.QueryString["PID"]);
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "PurchRequest").ToString();
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

                BindTable();
                FillDropDownCostCenter();
                FillDropDownDepartment();

                //GetDocNo();
                //GetDocRef();
                
                BindGridMain("","",0,"0");
                IsEdit = false;
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DropDownList dditem = (DropDownList)e.Row.FindControl("txtItem");
                //FillDropDownItem(dditem);

                TextBox txtduedte = (TextBox)e.Row.FindControl("txtDueDate");
                if (!txtduedte.Text.Trim().Equals(""))
                {
                    txtduedte.Text = Convert.ToDateTime(txtduedte.Text).ToString("dd-MMM-yy");
                }
            }
        }
        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
            //SetDropDownList();
        }
        private string GetUOMLabelFromUOMId(byte uomid)
        {
            return purchReqBL.GetUOMDescFromID(uomid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);   
        }
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
                    itmcd = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text.Trim();
                    qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                    dueDte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDueDate")).Text.Trim();

                    if (itmcd != "" && qty != "" && dueDte != "")
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
                            if (itmcd != "" && itmcd == ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtItem")).Text.Trim())
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
                for (int i = 0; i < grdRowsCount; i++)
                {
                    itmcd = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text.Trim();
                    
                    if (itmcd != "" && !new ItemCodeBL().IsItemExists(itmcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage(itmcd + "does not exist in database", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                }
                if (SavePurchRequest())
                {
                    ClearFieldsOnly();
                    BindGridMain(txtFltDocNo.Text.Trim(), txtFltDocRef.Text.Trim(), Convert.ToInt32(ddlFltDept.SelectedValue), ddlFltStatus.SelectedValue);

                }
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltDocNo.Text.Trim(), txtFltDocRef.Text.Trim(), Convert.ToInt32(ddlFltDept.SelectedValue), ddlFltStatus.SelectedValue);
        }
        protected void grdPurchReq_SelectedIndexChanged(object sender, EventArgs e)
        {

            ID = Convert.ToInt32(grdPurchReq.SelectedDataKey.Values["vr_id"]);
            IsEdit = true;
            GetByID();
            BindGridMain(txtFltDocNo.Text, txtFltDocRef.Text, Convert.ToInt32(ddlFltDept.SelectedValue), ddlFltStatus.SelectedValue);
            
        }
        protected void grdPurchReq_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    e.Row.Cells[6].Text = "";
                }

            }
        }
        protected void grdPurchReq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPurchReq.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltDocNo.Text.Trim(), txtFltDocRef.Text.Trim(), Convert.ToInt32(ddlFltDept.SelectedValue), ddlFltStatus.SelectedValue);
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

                if (grdPurchReq.Rows[rowIndex].Cells[4].Text.Equals("Approved"))
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
                    PrintPurchaseRequest(vrId, status);   
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropDownEmp();
        }

        #endregion

        #region Helping Method

        private void GetByID()
        {

            tblPOReq stkD = purchReqBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //txtDocNo.Text = stkD.vr_no.ToString().Substring(0, 4) + "/" + stkD.vr_no.ToString().Substring(4);
            

            txtDocRef.Text = stkD.DocRef;

            //txtDte.Text = stkD.vr_dt.ToString("dd-MMM-yy");
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

            ddlDept.SelectedValue = stkD.DeptId.ToString();
            if (stkD.IssTo != null)
            {
                FillDropDownEmp();
                ddlIssTo.SelectedValue = stkD.IssTo;
            }
            ddlCostCenter.SelectedValue = stkD.CC_cd;
            

            BindTableEdit(stkD.tblPOReqDets);

        }
        private void ClearFields()
        {
            Response.Redirect("~/inv/purchreqmgt.aspx?PID="+PID);

        }
        private void ClearFieldsOnly()
        {
            ID = 0;
            txtDocNo.Text = "";
            txtDocRef.Text = "";
            ddlCostCenter.SelectedIndex = 0;
            ddlDept.SelectedIndex = 0;
            txtDte.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yy");
            ddlStatus.SelectedValue = "P";
            DocNoFormated = "";
            DocNo = 0;
            IsEdit = false;
            //GetDocNo();
            BindTable();

        }
        private bool SavePurchRequest()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }

            tblPOReq poReq = null;

            if (ID == 0)
            {
               poReq = new tblPOReq();
            }
            else
            {
                poReq = purchReqBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }

            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    poReq.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                poReq.br_id = Convert.ToInt32(Session["BranchID"]);
            }
            if (!IsEdit)
            {
                GetDocNo();
            }
            
            //string no = txtDocNo.Text.Substring(5);
            //string yrno = txtDocNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(no));
            poReq.vr_no = DocNo;//Convert.ToInt32(yrno); // Doc No

            // = Convert.ToInt32(txtDocNo.Text.Trim());

            try
            {
                poReq.vr_dt = Convert.ToDateTime(txtDte.Text.Trim());
            }
            catch 
            {
                ucMessage.ShowMessage("Invalid Date", RMS.BL.Enums.MessageType.Error);
                txtDte.Focus();
                return false;
            }
            
            poReq.DocRef = txtDocRef.Text.Trim();
            poReq.DeptId = Convert.ToInt16(ddlDept.SelectedValue);
            poReq.CC_cd = ddlCostCenter.SelectedValue;
            poReq.vr_nrtn = "";
            poReq.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
            poReq.CreatedBy = Session["LoginID"].ToString();
            poReq.vr_apr = Convert.ToString(ddlStatus.SelectedValue);
            poReq.IssTo = ddlIssTo.SelectedValue;

            EntitySet<tblPOReqDet> reqDets = new EntitySet<tblPOReqDet>();
            string item, qty, dueDte;
            tblPOReqDet rdet;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text;
                dueDte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtDueDate")).Text;
                item = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text.Trim();
                //uom = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlUom")).SelectedValue;

                if (!qty.Equals("") && !dueDte.Equals("") && !item.Equals(""))
                {
                    rdet = new tblPOReqDet();
                    rdet.vr_seq = Convert.ToByte(i + 1);
                    rdet.itm_cd = item;
                    rdet.vr_id = poReq.vr_id;
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
                poReq.tblPOReqDets = reqDets;
                string msg = purchReqBL.SavePurchReqFull(poReq, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                string msg = purchReqBL.UpdPurchReqFull(poReq, reqDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
            uom = purchReqBL.GetItemUOM(item, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            return uom;
        }
        private void FillDropDownCostCenter()
        {
            ddlCostCenter.DataTextField = "cc_nme";
            ddlCostCenter.DataValueField = "cc_cd";
            ddlCostCenter.DataSource = new CCBL().GetInventoryCC((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCostCenter.DataBind();
        }
        private void FillDropDownDepartment()
        {
            ddlDept.DataTextField = "CodeDesc";
            ddlDept.DataValueField = "CodeID";
            ddlDept.DataSource = new PlCodeBL().GetAll4Grid(3, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDept.DataBind();

            ddlFltDept.DataTextField = "CodeDesc";
            ddlFltDept.DataValueField = "CodeID";
            ddlFltDept.DataSource = new PlCodeBL().GetAll4Grid(3, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltDept.DataBind();




            //ddlDept.DataTextField = "DeptNme";
            //ddlDept.DataValueField = "DeptId";
            //ddlDept.DataSource = purchReqBL.GetAllDepartment((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlDept.DataBind();

            //ddlFltDept.DataTextField = "DeptNme";
            //ddlFltDept.DataValueField = "DeptId";
            //ddlFltDept.DataSource = purchReqBL.GetAllDepartment((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlFltDept.DataBind();
        }
        private void BindGridMain(string docNo, string docRefNo, int deptId, string status)
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

            grdPurchReq.DataSource = purchReqBL.GetPurchReqs(docNo, docRefNo, deptId, status, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdPurchReq.DataBind();
        }
        public void GetDocNo()
        {
            //txtDocNo.Text = purchReqBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNoFormated = purchReqBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
        }
        private void PrintPurchaseRequest(int vrId, char status)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/PurchReq.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();

            List<spPurchRegRptResult> purchreq = purchReqBL.GetPurchReqMaster(vrId, status, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ReportDataSource dataSource1 = new ReportDataSource("spPurchRegRptResult", purchreq);

            List<spPurchRegDetRptResult> purchreqdet = purchReqBL.GetPurchReqDetail(vrId, status, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ReportDataSource dataSource2 = new ReportDataSource("spPurchRegDetRptResult", purchreqdet);

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
            rpt[0] = new ReportParameter("ReportName", "Demand Note");
            rpt[1] = new ReportParameter("LogoPath", rptLogoPath);
            rpt[2] = new ReportParameter("CompanyName", company);

            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(rpt);

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(dataSource1);
            reportViewer.LocalReport.DataSources.Add(dataSource2);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            filename = string.Format("{0}.{1}", "PurchaseRequest", "pdf");
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
        private void FillDropDownEmp()
        {
            ddlIssTo.Items.Clear();

            ddlIssTo.DataSource = new MatIssBL().GetEmpByDept(Convert.ToInt32(ddlDept.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlIssTo.DataValueField = "EmpID";
            ddlIssTo.DataTextField = "Name";
            ddlIssTo.DataBind();
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
                    string itemCode = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtItem")).Text.Trim();
                    string itemDesc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtItemDesc")).Text.Trim();
                    string uomitem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtUomItem")).Text.Trim();
                    string qoh = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtQOH")).Text.Trim();
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text.Trim();
                    string dte = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtDueDate")).Text.Trim();

                    d_Row["Sr"] = srNo;
                    d_Row["Item"] = itemCode;
                    d_Row["ItemDesc"] = itemDesc;
                    d_Row["Quantity"] = qty;
                    d_Row["UOM"] = uomitem;
                    d_Row["QOH"] = qoh;
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
            d_Col.ColumnName = "ItemDesc";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UOM";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "QOH";
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
            for (int i = 0; i < 20; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;

                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }
        public void BindTableEdit(EntitySet<tblPOReqDet> dets)
        {
            GetColumns();
            int count = 0;

            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;

                d_Row["Item"] = dt.itm_cd;
                d_Row["ItemDesc"] = new ItemCodeBL().GetItemDescByItemCode(dt.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                d_Row["Quantity"] = dt.vr_qty_req.ToString("F");
                d_Row["UOM"] = GetUOMLabelFromUOMId(dt.vr_uom);
                d_Row["QOH"] = purchReqBL.GetQOH(dt.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (dt.vr_exp_date != null)
                {
                    d_Row["DueDate"] = dt.vr_exp_date.Value.ToString("dd-MMM-yy");
                }

                count++;
                d_table.Rows.Add(d_Row);

            }
            CurrentTable = d_table;
            BindGrid();

            //SetDropDownList();
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }
        //public void SetDropDownList()
        //{
        //    rowsCount = CurrentTable.Rows.Count;
        //    for (int i = 0; i < rowsCount; i++)
        //    {
        //        DropDownList dditem = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("txtItem"));
        //        if (i < CurrentTable.Rows.Count)
        //        {
        //            if (CurrentTable.Rows[i]["Item"] != DBNull.Value)
        //            {
        //                dditem.ClearSelection();
        //                dditem.Items.FindByValue(CurrentTable.Rows[i]["Item"].ToString()).Selected = true;
        //            }
        //        }
        //    }
        //}
        //public void FillDropDownItem(DropDownList ddlItem)
        //{
        //    //ddlItem.DataSource = purchReqBL.GetAllItem((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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

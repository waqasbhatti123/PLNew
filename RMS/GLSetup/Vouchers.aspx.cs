using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RMS.BL;
using System.Data.Linq;
using System.Web.Services;
using AjaxControlToolkit;
using Microsoft.Reporting.WebForms;
using System.Text;

namespace RMS.GLSetup
{
    public partial class Vouchers : BasePage
    {
        #region Properties

        voucherDetailBL vdBl = new voucherDetailBL();
        voucherHomeBL voucObj = new voucherHomeBL();
        EntitySet<Glmf_Data_Det> gldatadet = new EntitySet<Glmf_Data_Det>();
        GroupBL grpBl = new GroupBL();

        voucherDetailBL objVoucher = new voucherDetailBL();
        DataRow d_Row;
#pragma warning disable CS0169 // The field 'Vouchers.d_Col' is never used
        DataColumn d_Col;
#pragma warning restore CS0169 // The field 'Vouchers.d_Col' is never used
        ListItem selitem = new ListItem();

        #endregion

        # region DataMembers

        public bool flagEdit
        {
            get { return Convert.ToBoolean(ViewState["flagEdit"]); }
            set { ViewState["flagEdit"] = value; }
        }
        public int voucherNo
        {
            get { return Convert.ToInt32(ViewState["voucherNo"]); }
            set { ViewState["voucherNo"] = value; }
        }
        public string pgTitle
        {
            get { return Convert.ToString(ViewState["pgTitle"]); }
            set { ViewState["pgTitle"] = value; }
        }
        public int pgID
        {
            get { return Convert.ToInt32(ViewState["pgID"]); }
            set { ViewState["pgID"] = value; }
        }
        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
        public int VoucherTypeID
        {
            get { return Convert.ToInt32(ViewState["VoucherTypeID"]); }
            set { ViewState["VoucherTypeID"] = value; }
        }
        public int VoucherID
        {
            get { return Convert.ToInt32(ViewState["VoucherID"]); }
            set { ViewState["VoucherID"] = value; }
        }
        public int ddt
        {
            get { return Convert.ToInt32(ViewState["ddt"]); }
            set { ViewState["ddt"] = value; }
        }
        public decimal Financialyear
        {
            get { return Convert.ToDecimal(ViewState["Financialyear"]); }
            set { ViewState["Financialyear"] = value; }
        }
        public string VoucherStatus
        {
            get { return ViewState["VoucherStatus"].ToString(); }
            set { ViewState["VoucherStatus"] = value; }
        }
        public DataTable currenttable
        {
            get { return (DataTable)ViewState["currenttable"]; }
            set { ViewState["currenttable"] = value; }
        
        }
        public int GroupID
        {
            get { return Convert.ToInt32(ViewState["GroupID"]); }
            set { ViewState["GroupID"] = value; }
        }

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

        public bool IsSearch
        {
            get { return (ViewState["IsSearch"] == null) ? false : Convert.ToBoolean(ViewState["IsSearch"]); }
            set { ViewState["IsSearch"] = value; }
        }

        #endregion

        #region events

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
                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                if (Session["Divi"] == null)
                {
                    ddt = Convert.ToInt32(Request.Cookies["uzr"]["Division"]);
                }
                else
                {
                    ddt = Convert.ToInt32(Session["Divi"].ToString());
                }

                txtFrom.Text = new DateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year, Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month, 1).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                txtTo.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                if (Session["DateFullYearFormat"] == null)
                {
                    txtChqDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    this.txtFromDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    this.txtToDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    txtChqDateCal.Format = Session["DateFullYearFormat"].ToString();
                    this.txtFromDate.Format = Session["DateFullYearFormat"].ToString();
                    this.txtToDate.Format = Session["DateFullYearFormat"].ToString();

                }
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                PID = Convert.ToInt32(Request.QueryString["PID"]);

                //==================== Start Edit record from GL Voucher Screen =============================//

                if (Session["VoucherId"] != null)
                {
                    VoucherID =Convert.ToInt32( Session["VoucherId"]);
                    VoucherTypeID = Convert.ToInt32(Session["VoucherTypeId"]);
                    FillDdlSource();
                    this.BindDataTable();
                    Session["VoucherId"] = null;
                    Session["PrevPgID"] = null;
                    Session["VoucherTypeId"] = null;
                    Session["FromDate"] = null;
                    Session["ToDate"] = null;
                    Session["Status"] = null;
 
                }

                //==================== End Edit record from GL Voucher Screen ===============================//

                // Now assigning value to voucherTypeID 

                switch (PID)
                {
                    case 321:
                        {
                            VoucherTypeID = 1;
                            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "JV").ToString();
                        }
                        break;
                    case 322:
                        {
                            VoucherTypeID = 2;
                            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "CP").ToString();
                            break;
                        }
                    case 323:
                        {
                            VoucherTypeID = 4;
                            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "CR").ToString();
                            break;
                        }
                    case 324:
                        {
                            VoucherTypeID = 3;
                            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "BP").ToString();
                            break;
                        }
                    case 325:
                        {
                            VoucherTypeID = 5;
                            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "BR").ToString();
                            break;
                        }
                    case 328:
                        {
                            VoucherTypeID = 55;
                            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "OB").ToString();
                            break;
                        }
                    case 399:
                        {
                            VoucherTypeID = 6;
                            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "IPV").ToString();
                            break;
                        }
 
                }

                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    divBankData.Visible = true;
                    if (Session["DateFullYearFormat"] == null)
                    {
                        txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    }
                    else
                    {
                        txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Session["DateFullYearFormat"].ToString());
                    }
                }
                else
                {
                    divBankData.Visible = false;
                }
                

                
                //SOURCE & REF NO MANAGEMENT
                if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
                {
                    lblRefSource.Text = "Voucher No";
                }
                else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
                {
                    lblRefSource.Text = "Voucher No.";
                }
                else//JV, OP & IP
                {
                    lblRefSource.Visible = false;
                    txtRefSource.Visible = false;
                }
                //END SOURCE & REF NO MANAGEMENT
                           
                //VoucherID = Convert.ToInt32(Session["VoucherID"]);
                //VoucherID = Convert.ToInt32(Session["VoucherId"]);
                //if (Session["VouchNo"] != null)
                //{
                //    string vr_No = Convert.ToString(Session["VouchNo"]).Substring(3);
                //    voucherNo = Convert.ToInt32(vr_No);
                //}
                Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //PID = Convert.ToInt32(Session["PrevPgID"]);
                txtdt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    this.txtDate.Format = Session["DateFullYearFormat"].ToString();
                }

              BindDataTable();

              //===================== Maintaning Privilage Status==========================
              if (Session["GroupID"] == null)
              {
                  GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
              }
              else
              {
                  GroupID = Convert.ToInt32(Session["GroupID"].ToString());
              }
              //int pid = 0;
              //pid = Convert.ToInt32(Session["PrevPgID"]);

              tblAppPrivilage appPrivilage = grpBl.GetPrivilageStatus(GroupID, PID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

              if (appPrivilage != null)
              {
                  //if (appPrivilage.CanEdit.Equals(true))//approval status given
                  //{
                  //    ddlstatus.Items.RemoveAt(0);
                  //}
                  BindDropDownStatus(appPrivilage.CanEdit);
              }
              else
              {
                  ucMessage.ShowMessage("Enter privilages for the logged in group", RMS.BL.Enums.MessageType.Error);
              }
              //===================== End Maintaning Privilage Status======================

              if (Session["dtFrmGLVoucher"] != null)
              {
                  this.txtFromDate.SelectedDate = Convert.ToDateTime(Session["dtFrmGLVoucher"]);
                  this.txtFrom.Text = Convert.ToDateTime(Session["dtFrmGLVoucher"]).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                  Session["dtFrmGLVoucher"] = null;
              }

              FillDdlSource();
              this.BindGrid(VoucherTypeID);

                
            }
        }

        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            this.searchBranchDropDown.DataTextField = "br_nme";
            searchBranchDropDown.DataValueField = "br_id";
            if (BranchObj.IsHead == true)
            {
                searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                List<Branch> BranchList = new List<Branch>();

                if (BranchObj != null)
                {
                    if (BranchObj.IsDisplay == true)
                    {
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BranchID).ToList();
                        BranchList.Insert(0, BranchObj);
                    }
                    else
                    {
                        BranchList.Add(BranchObj);
                    }
                }
                searchBranchDropDown.DataSource = BranchList.ToList();
            }
            searchBranchDropDown.DataBind();

        }



        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                    IsSearch = true;
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    this.BindGrid(VoucherTypeID);
                    // BindSalaryPackage(BranchID, IsSearch);
                }
            }
            catch
            { }
        }





        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IsSearch = true;
            this.BindGrid(VoucherTypeID);
        }
        protected void grdRemarks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text).ToString(System.Configuration.ConfigurationManager.AppSettings["DateTimeWOYearFormat"]);

                Glmf_Data_Rmk rem = voucObj.GetRemarksByID((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], VoucherID, Convert.ToInt32(e.Row.Cells[0].Text));
                if (rem != null)
                {
                    if (rem.Enabled)
                    {
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
        protected void linkBtn_Click(object sender, EventArgs e)
        {
            updateBtn_Click(null, null);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GLSetup/Vouchers.aspx?PID=" + PID);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (!IsAnyDataEnteredInGrid())
            {
                ucMessage.ShowMessage("Please enter data in grid to continue", RMS.BL.Enums.MessageType.Error);
                return;
            }
            if (!ValidateGrid())
            {
                ucMessage.ShowMessage("Voucher debit amount should be equal to credit amount", RMS.BL.Enums.MessageType.Error);
                return;
            }
            DateTime currdate = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
            DateTime Previous;
            try
            {
               Previous = Convert.ToDateTime(txtdt.Text.Trim());
            }
            catch 
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                     return;
            }
            if (VoucherTypeID != 6 && Previous > currdate)
            {
                ucMessage.ShowMessage("Voucher date can't be greater than current date", RMS.BL.Enums.MessageType.Error);
                return;
            }
            if (flagEdit == false)
            {
                if (VoucherTypeID != 6)
                {
                    SaveNew();
                }
                else
                {
                    ucMessage.ShowMessage("Cannot insert interface payment voucher.<br/> It can only be generated through payroll system", RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                EditPrevious();
            }
            BindGrid(VoucherTypeID);
        }
        protected void lnkInfo_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                int rowIndex = clickedRow.RowIndex;
                if (rowIndex > -1)
                {
                    string glCode = ((TextBox)grdView.Rows[rowIndex].FindControl("txtcode")).Text;
                    if (!string.IsNullOrEmpty(glCode))
                    {
                        RMSDataContext mSDataContext = new RMSDataContext();
                        var codeHead = mSDataContext.Glmf_Codes.Where(x => x.code.ToLower() == glCode.ToLower() || x.gl_cd == glCode).FirstOrDefault();

                        String script = "window.open('../GLSetup/ViewLedgerCard.aspx?Code=" + glCode + "', 'myPopup','directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,addressbar=0')";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), codeHead.gl_cd, script, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        protected void updateBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("seq", typeof(string)));
            dt.Columns.Add(new DataColumn("code", typeof(string)));
            dt.Columns.Add(new DataColumn("desc", typeof(string)));
            dt.Columns.Add(new DataColumn("debit", typeof(string)));
            dt.Columns.Add(new DataColumn("credit", typeof(string)));
            dt.Columns.Add(new DataColumn("remark", typeof(string)));
            dt.Columns.Add(new DataColumn("costcenter", typeof(string)));
            dt.Columns.Add(new DataColumn("balance", typeof(string)));

            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                string vr_seq = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtseq")).Text;
                string code = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcode")).Text;
                string debit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdebit")).Text;
                string desc = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdescription")).Text;
                string credit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcredit")).Text;
                string remarks = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtremarks")).Text;
                string costcenter = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlcostcenter")).SelectedItem.Text;
                 d_Row = dt.NewRow();
                d_Row["seq"] = vr_seq;
                RMSDataContext mSDataContext = new RMSDataContext();
                var codeHead = mSDataContext.Glmf_Codes.Where(x => x.code.ToLower() == code.ToLower() || x.gl_cd == code).FirstOrDefault();
                if(codeHead != null)
                {
                    d_Row["code"] = codeHead.code != null ? codeHead.code.ToString() : codeHead.gl_cd.ToString();
                }
                else
                {
                    d_Row["code"] = "";
                }
                d_Row["desc"] = desc;
                d_Row["debit"] = debit;
                d_Row["credit"] = credit;
                d_Row["remark"] = remarks;
                d_Row["costcenter"] = costcenter;
                d_Row["balance"] = 3000;//pick here





                  dt.Rows.Add(d_Row);




                dt.AcceptChanges();
            }
            for (int k = 1; k < 6; k++)
            {
                d_Row = dt.NewRow();
                d_Row["seq"] = grdView.Rows.Count + k;
                dt.Rows.Add(d_Row);
            }
            dt.AcceptChanges();
            currenttable = dt;
            BindGrid();
            SetDropDown();
            btnSave.Focus();
        }
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((DropDownList)e.Row.FindControl("ddlcostcenter")).DataTextField = "cc_nme";
                ((DropDownList)e.Row.FindControl("ddlcostcenter")).DataValueField = "cc_cd";
                ((DropDownList)e.Row.FindControl("ddlcostcenter")).DataSource = new CCBL().GetGLCC((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ((DropDownList)e.Row.FindControl("ddlcostcenter")).DataBind();


            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {

            }

        }
        protected void grdVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdVoucher.PageIndex = e.NewPageIndex;
            BindGrid(VoucherTypeID);

        }
        protected void grdVoucher_SelectedIndexChanging(object sender, EventArgs e)
        {
            VoucherID = Convert.ToInt32(grdVoucher.SelectedDataKey.Values["vrid"]);
            this.BindDataTable();
            //this.ValidateBtnSave();
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            
            VoucherID = Convert.ToInt32(grdVoucher.DataKeys[clickedRow.RowIndex].Values["vrid"]);
            Label lblstatus = (Label)clickedRow.FindControl("lblstatus");
            VoucherStatus = lblstatus.Text;
            CreatePDF(Session["PageTitle"].ToString(), "pdf", VoucherID);
        }
        protected void grdVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (!voucObj.GetRemarksStatus((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], Convert.ToInt32(e.Row.Cells[0].Text)))
                {
                    Image img = e.Row.FindControl("imgRemarks") as Image;
                    img.Visible = false;
                }

            }
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            string username = "";
            if (Session["LoginID"] == null)
            {
                username = Request.Cookies["uzr"]["LoginID"];
            }
            else
            {
                username = Session["LoginID"].ToString();
            }
            if (username.Length > 15)
            {
                username.Substring(0, 14);
            }
            foreach (GridViewRow row in grdVoucher.Rows)
            {
                char flag;
                Label lblvrid = (Label)row.FindControl("lbl");
                int vrid = Convert.ToInt32(lblvrid.Text);
                CheckBox chkstatus = (CheckBox)row.FindControl("chkstatus");
                Label lblStat = (Label)row.FindControl("lblstatus");
                if (chkstatus.Checked == true)
                {
                    flag = 'A';
                    if (!lblStat.Text.Equals("A") && !lblStat.Text.Equals("Approved"))
                    {
                        voucObj.Save((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid, flag, username);
                        vdBl.PostVoucher4mHome((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid, username);
                    }
                }
            }
            BindGrid(VoucherTypeID);
        }

        #endregion
        
        #region helping Methods

        protected void CreatePDF(String FileName, String extension, int vrid)
        {
            // Variables

            

            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            ReportDataSource datasource = null;
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + extension + "</OutputFormat>" +
            "  <PageWidth>8.27in</PageWidth>" +
            "  <PageHeight>11.69in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.15in</MarginLeft>" +
            "  <MarginRight>0.15in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            string brName = "";
            string brAddress = "";
            string brTel = "";
            string brFax = "";
            string rptLogoPath = "";
            string updateby = "";
            string approvedby = "";
            string USerName = "";
            if (PID == 324 || PID == 325)  // || PID == 328
            {
                //if (ddt == 0)
                //{
                //    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                //    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1.rdlc";
                //    datasource = new ReportDataSource("spBPnBRResult", sal);
                //    updateby = sal.First().updateby;
                //    approvedby = sal.First().approvedby;
                //}
                //if (ddt == 1)
                //{
                //    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                //    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1Division.rdlc";
                //    datasource = new ReportDataSource("spBPnBRResult", sal);
                //    updateby = sal.First().updateby;
                //    approvedby = sal.First().approvedby;
                //}
                //if (ddt == 2)
                //{
                //    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                //    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1District.rdlc";
                //    datasource = new ReportDataSource("spBPnBRResult", sal);
                //    updateby = sal.First().updateby;
                //    approvedby = sal.First().approvedby;
                //}
                //if (ddt == 3)
                //{
                //    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                //    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1District.rdlc";
                //    datasource = new ReportDataSource("spBPnBRResult", sal);
                //    updateby = sal.First().updateby;
                //    approvedby = sal.First().approvedby;
                //}

                if (BranchID == 1)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 3)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1Division.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 4)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1Division.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 5)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1Division.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 6)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1Division.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 7)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1Division.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 8)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1Division.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 9)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1Division.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 10)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1Division.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 11)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1District.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 12)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1District.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 13)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1District.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                if (BranchID == 15)
                {
                    IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                    viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR1District.rdlc";
                    datasource = new ReportDataSource("spBPnBRResult", sal);
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
            }

            else
            {
                IList<spTestGLResult> sal = voucObj.GetReport((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptJV1.rdlc";
                datasource = new ReportDataSource("spTestGLResult", sal);
                if(sal.Count > 0)
                {
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                else
                {
                    updateby = "";
                    approvedby = ""; 
                }
                
            }


            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            try
            {
                Branch branch = voucObj.GetBranch(BranchID, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                if (branch != null)
                {
                    brName = Convert.ToString(branch.br_nme);
                    brAddress = Convert.ToString(branch.br_address);
                    brTel = Convert.ToString(branch.br_tel);
                    brFax = Convert.ToString(branch.br_fax);
                }
            }
            catch { }

            int UseID = Convert.ToInt32(Session["UserID"]);
            

            tblAppUser User = voucObj.GetUsersss(UseID, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);

            if (User != null)
            {
                USerName = User.UserName;
            }
            

            ReportParameter[] paramz = new ReportParameter[10];

            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }

            paramz[1] = new ReportParameter("ReportName", FileName, false);
            paramz[2] = new ReportParameter("voucherstatus", VoucherStatus , false);
            paramz[3] = new ReportParameter("brName", brName, false);
            paramz[4] = new ReportParameter("brAddress", brAddress, false);
            paramz[5] = new ReportParameter("brTel", brTel, false);
            paramz[6] = new ReportParameter("brFax", brFax, false);
            paramz[7] = new ReportParameter("LogoPath", rptLogoPath, false);
            paramz[8] = new ReportParameter("AprBy", approvedby, false);
            paramz[9] = new ReportParameter("UpdBy", updateby, false);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + "_" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) + ".") + extension);
            Response.BinaryWrite(bytes);
            //// create the file
            //// send it to the client to download
            Response.Flush();
        }
        //public void ValidateBtnSave()
        //{
        //    btnSave.Enabled = false;
        //    foreach (ListItem itm in ddlstatus.Items)
        //    {
        //        if (itm.Value.ToString().ToLower() == "p")
        //        {
        //            btnSave.Enabled = true;
        //        }
        //    }
        //}
        public void BindDropDownStatus(bool canApprove)
        {
            ddlstatus.Items.Clear();
            ddlFltrStatus.Items.Clear();

            ListItem list;
            list = new ListItem();
            list.Value = "0";
            list.Text = "All";
            ddlFltrStatus.Items.Add(list);

            if (canApprove)
            {
                list = new ListItem();
                list.Value = "A";
                list.Text = "Approved";
                ddlstatus.Items.Add(list);
                ddlFltrStatus.Items.Add(list);

                list = new ListItem();
                list.Value = "D";
                list.Text = "Cancelled";
                ddlstatus.Items.Add(list);
                ddlFltrStatus.Items.Add(list);
            }
            list = new ListItem();
            list.Value = "P";
            list.Text = "Pending";
            ddlstatus.Items.Add(list);
            ddlFltrStatus.Items.Add(list);
            ddlstatus.SelectedValue= "A";
         }
        public void BindGrid()
        {
            grdView.DataSource = currenttable;
            grdView.DataBind();
        }
        protected void BindGrid(int vt_cd)
        {
            string txt = "";
            try
            {
                txt = "fromDate";
                Convert.ToDateTime(txtFrom.Text);
                txt = "toDate";
                Convert.ToDateTime(txtTo.Text);
            }
            catch
            {
                if (txt.Equals("fromDate"))
                {
                    ucMessage.ShowMessage("Invalid from date", RMS.BL.Enums.MessageType.Error);
                    txtFrom.Focus();
                }
                else
                {
                    ucMessage.ShowMessage("Invalid to date", RMS.BL.Enums.MessageType.Error);
                    txtTo.Focus();
                }
                return;
            }


            DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(txtFrom.Text, out dtFrm);
            DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(txtTo.Text, out dt2);

            grdVoucher.DataSource = voucObj.GetDataForGrid(vt_cd, Convert.ToDateTime(txtFrom.Text), Convert.ToDateTime(txtTo.Text), Convert.ToChar(ddlFltrStatus.SelectedItem.Value),BranchID, IsSearch, Financialyear, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdVoucher.DataBind();

        }
        public bool ValidateGrid()
        {
            decimal debt = 0;
            decimal crdt = 0;
            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                string debit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdebit")).Text.Trim();
                string credit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcredit")).Text.Trim();
                if (!string.IsNullOrEmpty(debit))
                    debt = debt + Convert.ToDecimal(debit);
                if (!string.IsNullOrEmpty(credit))
                    crdt = crdt + Convert.ToDecimal(credit);
            }
            if (debt != crdt)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool IsAnyDataEnteredInGrid()
        {
            decimal debt = 0;
            decimal crdt = 0;
            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                string debit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdebit")).Text.Trim();
                string credit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcredit")).Text.Trim();
                if (!string.IsNullOrEmpty(debit))
                    debt = debt + Convert.ToDecimal(debit);
                if (!string.IsNullOrEmpty(credit))
                    crdt = crdt + Convert.ToDecimal(credit);
            }
            if (debt < 1 || crdt < 1)
            {
                return false;
            }
            else
                return true;
        }
        public void BindDataTable()
        {
            List<spVouchersFill2GridResult> dtl = vdBl.GetVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], VoucherID, BranchID);

            DataTable d_table = new DataTable();
            d_table.Columns.Clear();
            d_table.Rows.Clear();
            d_table.Columns.Add(new DataColumn("seq", typeof(string)));
            d_table.Columns.Add(new DataColumn("code", typeof(string)));
            d_table.Columns.Add(new DataColumn("desc", typeof(string)));
            d_table.Columns.Add(new DataColumn("debit", typeof(string)));
            d_table.Columns.Add(new DataColumn("credit", typeof(string)));
            d_table.Columns.Add(new DataColumn("remark", typeof(string)));
            d_table.Columns.Add(new DataColumn("costcenter", typeof(string)));
            d_table.Columns.Add(new DataColumn("balance", typeof(string)));
            if (VoucherID != 0 && dtl.Count > 0)
            {
                foreach (spVouchersFill2GridResult l in dtl)
                {
                    flagEdit = true;
                    txtdt.Text = Convert.ToDateTime(l.vr_dt).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    ddlstatus.SelectedValue = l.vr_apr.ToString();
                    if (l.vr_apr.ToString().ToLower() == "a" || l.vr_apr.ToString().ToLower() == "d")
                    {
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                    }
                    txtnarration.Text = l.narration.ToString();
                    if (string.IsNullOrEmpty(l.NTN))
                    {
                        txtNtnNumbere.Text = "";
                    }
                    else
                    {
                        txtNtnNumbere.Text = l.NTN.ToString();
                    }
                    

                    //SOURCE & REF NO MANAGEMENT
                    if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
                    {
                        txtRefSource.Text = l.Ref_no;
                    }
                    else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
                    {
                        txtRefSource.Text = l.Ref_no;
                    }
                    else//JV, OP & IP
                    {
                    }
                    if (!string.IsNullOrEmpty(l.source))
                    {
                        ddlSource.SelectedItem.Text = l.source;
                        ddlSource.Enabled = false;
                    }
                    else
                    {
                        ddlSource.Enabled = true;
                    }
                    //END SOURCE & REF NO MANAGEMENT

                    d_Row = d_table.NewRow();
                    d_Row["seq"] = l.vr_seq.ToString();
                    d_Row["code"] = l.code!= null? l.code.ToString() : l.gl_cd.ToString();
                    d_Row["desc"] = l.gl_dsc.ToString();
                    d_Row["debit"] = l.vrd_debit.ToString();
                    d_Row["credit"] = l.vrd_credit.ToString();
                    d_Row["remark"] = l.remarks.ToString();
                    if (l.cc_nme != null)
                        d_Row["costcenter"] = l.cc_nme.ToString();
                    else
                        d_Row["costcenter"] = "";
                    d_Row["balance"] = "3000";//pick here;
                    d_table.Rows.Add(d_Row);
                }
                currenttable = d_table;
                BindGrid();
                SetInitialDropDown(dtl);
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    SetVoucherCheqDet();
                }

            }
            else
            {
                flagEdit = false;
                for (int i = 1; i < 6; i++)
                {
                    d_Row = d_table.NewRow();
                    d_Row["seq"] = i.ToString();
                    d_table.Rows.Add(d_Row);

                }
                currenttable = d_table;
                BindGrid();

            }



        }
        private void SetVoucherCheqDet()
        {
            Glmf_Data_chq glmfChq = objVoucher.GetCheqDetByID(VoucherID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (glmfChq != null)
            {
                txtChqBranch.Text = glmfChq.vr_chq_branch;
                txtChqNo.Text = glmfChq.vr_chq;
                if (Session["DateFullYearFormat"] == null)
                {
                    txtChqDate.Text = glmfChq.vr_chq_dt.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    txtChqDate.Text = glmfChq.vr_chq_dt.ToString(Session["DateFullYearFormat"].ToString());
                }
                txtChqAcctNo.Text = glmfChq.vr_chq_ac;
            }
        }
        public void SetInitialDropDown(List<spVouchersFill2GridResult> lst)
        {
            int i = 0;
            foreach (spVouchersFill2GridResult l in lst)
            {
                DropDownList ddl = (DropDownList)grdView.Rows[i].Cells[6].FindControl("ddlcostcenter");
                ddl.ClearSelection();
                if (l.cc_nme != null)
                    ddl.Items.FindByText(l.cc_nme.ToString()).Selected = true;

                i++;
            }

        }
        public void EditPrevious()
        {
            Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            
            //*************************************************************
            try
            {
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    Glmf_Data_chq chq = new Glmf_Data_chq();
                    chq.vrid = VoucherID;
                    chq.vr_chq_branch = txtChqBranch.Text.Trim();
                    chq.vr_chq = txtChqNo.Text;
                    if (RMS.BL.BranchBL.IsChequeNoExists1(chq, Convert.ToInt32(Session["BranchID"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage("Cheque no already exists", RMS.BL.Enums.MessageType.Error);
                        txtChqNo.Focus();
                        chq = null;
                        return;
                    }
                    chq = null;
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
                return;
            }
            //*************************************************************

            gldatadet.Clear() ;


            string username = "";
            if (Session["LoginID"] == null)
            {
                username = Request.Cookies["uzr"]["LoginID"];
            }
            else
            {
                username = Session["LoginID"].ToString();
            }

            if (username.Length > 15)
            {
                username = username.Substring(0, 14);
            }

            int vrID = VoucherID;
            Glmf_Data glmfdata = objVoucher.GetGlmf_Data(VoucherID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (glmfdata != null)
            {
                try
                {
                    try
                    {
                        glmfdata.vr_dt = Convert.ToDateTime(txtdt.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                        {
                            ucMessage.ShowMessage("Date is invalid", RMS.BL.Enums.MessageType.Error);
                            txtdt.Focus();
                        }
                        return;
                    }
                    //glmfdata.vr_dt = Convert.ToDateTime(txtdt.Text.Trim());
                    //if (!txtdt.Text.Trim().Equals(""))
                    //{
                    //    try
                    //    {
                    //        glmfdata.vr_dt = Convert.ToDateTime(txtdt.Text.Trim());
                    //    }
                    //    catch
                    //    {
                    //        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    //        return;
                    //    }
                    //}
                    DateTime dtFrm = DateTime.Parse("01-Jul-" + Financialyear);
                    dtFrm = dtFrm.AddYears(-1);
                    DateTime dtTo = DateTime.Parse("30-Jun-" + Financialyear);
                    if (glmfdata.vr_dt >= dtFrm && glmfdata.vr_dt <= dtTo)
                    {
                        //ok
                    }
                    //else if (glmfdata.vr_dt >= DateTime.Parse("01-Jul-" + Financialyear)
                    //&& glmfdata.vr_dt <= DateTime.Parse("30-Jun-" + (Financialyear + 1))
                    //&& glmfdata.vr_dt <= Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date
                    //)
                    //{
                    //    Financialyear++;
                    //}
                    else
                    {
                        ucMessage.ShowMessage("Voucher Date should be within these financial years i.e. " + Financialyear + " and " + (Financialyear + 1), RMS.BL.Enums.MessageType.Error);
                        txtDate.Focus();
                        return;
                    }

                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                    {
                        ucMessage.ShowMessage("Voucher Date is invalid", RMS.BL.Enums.MessageType.Error);
                        txtDate.Focus();
                    }
                    return;
                }

                glmfdata.Gl_Year = Financialyear;

                
                glmfdata.vr_nrtn = txtnarration.Text.Trim();
                glmfdata.NTN = txtNtnNumbere.Text.Trim();
                glmfdata.vr_apr = Convert.ToString(ddlstatus.SelectedItem.Value);

                if (glmfdata.vr_apr == "A")
                {
                    glmfdata.approvedby = username;
                    glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                else
                {
                    glmfdata.updateby = username;
                    glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                //SOURCE & REF NO MANAGEMENT
                if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
                {
                    glmfdata.Ref_no = txtRefSource.Text;
                }
                else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
                {
                    glmfdata.Ref_no = txtRefSource.Text;
                }
                else//JV, OP & IP
                {
                }
                if (ddlSource.Items.Count > 0)
                    glmfdata.source = ddlSource.SelectedItem.Text;
                else
                    glmfdata.source = null;
                //END SOURCE & REF NO MANAGEMENT

                int count = 0;
                for (int i = 0; i < grdView.Rows.Count; i++)
                {
                    //Glmf_Data_Det gldet = new Glmf_Data_Det();//UPDATE HERE CHILD TABLE
                    string vr_seq = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtseq")).Text.Trim();
                    string code = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcode")).Text.Trim();
                    string debit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdebit")).Text.Trim();
                    string credit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcredit")).Text.Trim();
                    string remarks = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtremarks")).Text.Trim();
                    //string costcenter = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcostcenter")).Text;
                    string costcenter = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlcostcenter")).SelectedItem.Value;

                    if (String.IsNullOrEmpty(code) == false)
                    {
                        count = count+1;
                        //Glmf_Data_Det gldet = objVoucher.GetGlmf_Data_Det(VoucherID, Convert.ToInt32(vr_seq), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        //if (gldet != null)
                        //{
                        //    gldet.gl_cd = code;
                        //    try
                        //    {
                        //        if (debit != null && debit.Trim().Equals(""))
                        //        {
                        //            gldet.vrd_debit = 0;
                        //        }
                        //        else
                        //        {
                        //            gldet.vrd_debit = Convert.ToDecimal(debit);
                        //        }
                        //    }
                        //    catch
                        //    {
                        //        gldet.vrd_debit = 0;
                        //    }

                        //    try
                        //    {
                        //        if (credit != null && credit.Trim().Equals(""))
                        //        {
                        //            gldet.vrd_credit = 0;
                        //        }
                        //        else
                        //        {
                        //            gldet.vrd_credit = Convert.ToDecimal(credit);
                        //        }
                        //    }
                        //    catch
                        //    {
                        //        gldet.vrd_credit = 0;
                        //    }
                        //    try
                        //    {
                        //        if (remarks.Length > 500)
                        //            remarks = remarks.Substring(0, 499);
                        //    }
                        //    catch { }

                        //    gldet.vrd_nrtn = remarks;

                        //    if (costcenter == "")
                        //    {
                        //        gldet.cc_cd = null;
                        //    }
                        //    else
                        //    {
                        //        gldet.cc_cd = costcenter;
                        //    }
                        //}
                        //else
                        //{
                        Glmf_Data_Det gldet = new Glmf_Data_Det();
                            
                            gldet.vrid = VoucherID;
                            gldet.vr_seq = count;
                        RMSDataContext mSDataContext = new RMSDataContext();
                        var codeHead = mSDataContext.Glmf_Codes.Where(x => x.code.ToLower() == code.ToLower() || x.gl_cd == code).FirstOrDefault();

                        gldet.gl_cd = codeHead.gl_cd;
                            try
                            {
                                if (debit != null && debit.Trim().Equals(""))
                                {
                                    gldet.vrd_debit = 0;
                                }
                                else
                                {
                                    gldet.vrd_debit = Convert.ToDecimal(debit);
                                }
                            }
                            catch
                            {
                                gldet.vrd_debit = 0;
                            }

                            try
                            {
                                if (credit != null && credit.Trim().Equals(""))
                                {
                                    gldet.vrd_credit = 0;
                                }
                                else
                                {
                                    gldet.vrd_credit = Convert.ToDecimal(credit);
                                }
                            }
                            catch
                            {
                                gldet.vrd_credit = 0;
                            }
                            try
                            {
                                if (remarks.Length > 500)
                                    remarks = remarks.Substring(0, 499);
                            }
                            catch { }

                            gldet.vrd_nrtn = remarks;

                            if (costcenter == "")
                            {
                                gldet.cc_cd = null;
                            }
                            else

                                gldet.cc_cd = costcenter;
                        //}
                        gldatadet.Add(gldet);
                    }
                }
            }
   
            Glmf_Data_chq glmfChq = null;
            if (VoucherTypeID == 3 || VoucherTypeID == 5)
            {
                glmfChq = objVoucher.GetGlmf_Data_Chq(VoucherID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                if (glmfChq != null)
                {
                    glmfChq.vr_chq_branch = txtChqBranch.Text.Trim();
                    glmfChq.vr_chq = txtChqNo.Text.Trim();
                    try
                    {
                        glmfChq.vr_chq_dt = Convert.ToDateTime(txtChqDate.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                        {
                            ucMessage.ShowMessage("Cheque Date is invalid", RMS.BL.Enums.MessageType.Error);
                            txtChqDate.Focus();
                        }
                        return;
                    }

                    glmfChq.vr_chq_ac = txtChqAcctNo.Text.Trim();
                }
                
            }
            string vrDescptn = objVoucher.VoucherDesc(VoucherTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                //glmfdata.Glmf_Data_Dets = gldatadet;
                objVoucher.UpdateVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);

                voucherNo = glmfdata.vr_no;

                voucObj.UpdateRemarksStatus((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], vrID);
                
                
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    objVoucher.UdateChq((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfChq);
                }
                if (glmfdata.vr_apr.ToString().Equals("A"))
                {
                    if (VoucherTypeID == 55)
                    {
                        objVoucher.PostVoucherOpeningBalDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                    }
                    else
                    {
                        objVoucher.PostVoucherDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                    }
                }
                ucMessage.ShowMessage("voucher " + vrDescptn + '-' + voucherNo + " has been updated successfully", RMS.BL.Enums.MessageType.Info);
                ClearFields();
                //Session["DisplayMsg"] = true;
                //Session["MsgType"] = "Info";
                //Session["Msg"] = "voucher " + vrDescptn + '-' + voucherNo + " has been updated successfully";
            }
            catch (Exception e)
            {
                if (e.Message.Contains("An attempt was made to remove a relationship between a Glmf_Data and a Glmf_Data_Det. However, one of the relationship's foreign keys (Glmf_Data_Det.vrid) cannot be set to null."))
                {
                    ucMessage.ShowMessage("Please enter GL Code", RMS.BL.Enums.MessageType.Error);
                    //Session["DisplayMsg"] = true;
                    //Session["MsgType"] = "Error";
                    //Session["Msg"] = "Please enter GL Code";
                }
                else
                {
                    ucMessage.ShowMessage("Exception occured in updating data.<br/>Exception is: " + e.Message, RMS.BL.Enums.MessageType.Error);
                    //Session["DisplayMsg"] = true;
                    //Session["MsgType"] = "Error";
                    //Session["Msg"] = "Exception occured in updating data.<br/>Exception is: " + e.Message;
                }
                return;
            }
            
            //System.Threading.Thread.Sleep(5000);
            //Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + pgID);
            //Response.Redirect("~/GLSetup/frmVoucherDetail.aspx?PID=327");
            

        }
        public void SaveNew()
        {
            Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //*************************************************************
            try
            {
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    Glmf_Data_chq chq = new Glmf_Data_chq();
                    chq.vr_chq_branch = txtChqBranch.Text.Trim();
                    chq.vr_chq = txtChqNo.Text;
                    if (RMS.BL.BranchBL.IsChequeNoExists1(chq, Convert.ToInt32(Session["BranchID"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage("Cheque no already exists", RMS.BL.Enums.MessageType.Error);
                        txtChqNo.Focus();
                        chq = null;
                        return;
                    }
                    chq = null;
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
                return;
            }
            //*************************************************************
            
            string username = "";
            if(Session["LoginID"] == null)
            {
                username = Request.Cookies["uzr"]["LoginID"];
            }
            else
            {
                username = Session["LoginID"].ToString();
            }

            if (username.Length > 15)
            {
                username = username.Substring(0, 14);
            }
            
            Glmf_Data glmfdata = new Glmf_Data();
            try
            {
                try
                {
                    glmfdata.vr_dt = Convert.ToDateTime(txtdt.Text.Trim());
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                    {
                        ucMessage.ShowMessage("Date is invalid", RMS.BL.Enums.MessageType.Error);
                        txtdt.Focus();
                    }
                    return;
                }
                //if (!txtdt.Text.Trim().Equals(""))
                //{
                //    try
                //    {
                //        glmfdata.vr_dt = Convert.ToDateTime(txtdt.Text.Trim());
                //    }
                //    catch 
                //    {
                //        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                //        return;
                //    }
                //}

                DateTime dtFrm = DateTime.Parse("01-Jul-" + Financialyear);
                dtFrm = dtFrm.AddYears(-1);
                DateTime dtTo = DateTime.Parse("30-Jun-" + Financialyear);
                if (glmfdata.vr_dt >= dtFrm && glmfdata.vr_dt <= dtTo)
                {
                    //ok
                }
                //else if (    glmfdata.vr_dt >= DateTime.Parse("01-Jul-" + Financialyear) 
                //    &&  glmfdata.vr_dt <= DateTime.Parse("30-Jun-" + (Financialyear+1))
                //    &&  glmfdata.vr_dt <= Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date
                //    )
                //{
                //    Financialyear++;
                //}
                else
                {
                    ucMessage.ShowMessage("Voucher Date should be within these financial years i.e. " + (Financialyear - 1) + " and " + Financialyear, RMS.BL.Enums.MessageType.Error);
                    txtDate.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                {
                    ucMessage.ShowMessage("Voucher Date is invalid", RMS.BL.Enums.MessageType.Error);
                    txtDate.Focus();
                }
                return;
            }
            int voucherno=0;
            if (ddlSource.Items.Count > 0)
                voucherno = objVoucher.GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], BranchID, VoucherTypeID, Financialyear, ddlSource.SelectedItem.Text);
            else
                voucherno = objVoucher.GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], BranchID, VoucherTypeID, Financialyear, null);
            
            glmfdata.br_id = BranchID;
            glmfdata.Gl_Year = Financialyear;
            glmfdata.vt_cd = Convert.ToInt16(VoucherTypeID);
            glmfdata.vr_no = voucherno;
            

            glmfdata.vr_nrtn = txtnarration.Text.Trim();
            glmfdata.NTN = txtNtnNumbere.Text.Trim();
            glmfdata.vr_apr = Convert.ToString(ddlstatus.SelectedItem.Value);
           

            //SOURCE & REF NO MANAGEMENT
            if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
            {
                glmfdata.Ref_no = txtRefSource.Text;
            }
            else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
            {
                glmfdata.Ref_no = txtRefSource.Text;
            }
            else//JV, OP & IP
            {
            }
            if (ddlSource.Items.Count > 0)
                glmfdata.source = ddlSource.SelectedItem.Text;
            else
                glmfdata.source = null;
            //END SOURCE & REF NO MANAGEMENT


            glmfdata.updateby = username;
            glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (glmfdata.vr_apr == "A")
            {
                glmfdata.approvedby = username;
                glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            int count = 1;
            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                Glmf_Data_Det gldet = new Glmf_Data_Det();
                string vr_seq = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtseq")).Text.Trim();
                string code = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcode")).Text.Trim();
                string debit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdebit")).Text.Trim();
                string credit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcredit")).Text.Trim();
                string remarks = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtremarks")).Text.Trim();
                //string costcenter = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcostcenter")).Text;
                string costcenter = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlcostcenter")).SelectedItem.Value;

                if (String.IsNullOrEmpty(code) == false)
                {
                    gldet.vr_seq = count++;
                    RMSDataContext mSDataContext = new RMSDataContext();
                    var codeHead = mSDataContext.Glmf_Codes.Where(x => x.code.ToLower() == code.ToLower() || x.gl_cd == code).FirstOrDefault();

                    gldet.gl_cd = codeHead.gl_cd;
                    try
                    {
                        if (debit != null && debit.Trim().Equals(""))
                        {
                            gldet.vrd_debit = 0;
                        }
                        else
                        {
                            gldet.vrd_debit = Convert.ToDecimal(debit);
                        }
                    }
                    catch
                    {
                        gldet.vrd_debit = 0;
                    }

                    try
                    {
                        if (credit != null && credit.Trim().Equals(""))
                        {
                            gldet.vrd_credit = 0;
                        }
                        else
                        {
                            gldet.vrd_credit = Convert.ToDecimal(credit);
                        }
                    }
                    catch 
                    {
                        gldet.vrd_credit = 0;
                    }
                    try
                    {
                        if (remarks.Length > 500)
                            remarks = remarks.Substring(0, 499);
                    }
                    catch { }
                    gldet.vrd_nrtn = remarks;
                    if (costcenter == "")
                    {
                        gldet.cc_cd = null;
                    }
                    else

                        gldet.cc_cd = costcenter;
                    gldatadet.Add(gldet);
                }


            }

            Glmf_Data_chq glmfChq = null;
            if (VoucherTypeID == 3 || VoucherTypeID == 5)
            {
                glmfChq = new Glmf_Data_chq();
                glmfChq.vr_chq_branch = txtChqBranch.Text.Trim();
                glmfChq.vr_chq = txtChqNo.Text.Trim();
                try
                {
                    glmfChq.vr_chq_dt = Convert.ToDateTime(txtChqDate.Text.Trim());
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                    {
                        ucMessage.ShowMessage("Cheque Date is invalid", RMS.BL.Enums.MessageType.Error);
                        txtChqDate.Focus();
                    }
                    return;
                }
                
                glmfChq.vr_chq_ac = txtChqAcctNo.Text.Trim();
            }

            string vrDescptn = objVoucher.VoucherDesc(VoucherTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                objVoucher.SaveVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    if (glmfdata.vrid > 0)
                    {
                        objVoucher.SaveVoucherCheqDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata.vrid, glmfChq);
                    }
                }
                if (glmfdata.vr_apr.ToString().Equals("A"))
                {
                    

                    if (VoucherTypeID == 55)
                    {
                        objVoucher.PostVoucherOpeningBalDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                    }
                    else
                    {
                        objVoucher.PostVoucherDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                    }
                }
                ucMessage.ShowMessage("voucher " + vrDescptn + '-' + voucherno + " has been posted successfully", RMS.BL.Enums.MessageType.Info);
                ClearFields();
                //Session["DisplayMsg"] = true;
                //Session["MsgType"] = "Info";
                //Session["Msg"] = "voucher " + vrDescptn + '-' + voucherno + " has been posted successfully";
            }
            catch (Exception e)
            {
                ucMessage.ShowMessage("Exception occured in saving data.<br/>Exception is: " + e.Message, RMS.BL.Enums.MessageType.Error);
                //Session["DisplayMsg"] = true;
                //Session["MsgType"] = "Error";
                //Session["Msg"] = "Exception occured in saving data.<br/>Exception is: " + e.Message;
                return;
            }

            //System.Threading.Thread.Sleep(5000);
            //Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + pgID);
            //Response.Redirect("~/GLSetup/frmVoucherDetail.aspx?PID=327");
        }
        protected void SetDropDown()
        {
            
            if (currenttable != null)
            { 
                for(int i=0;i < grdView.Rows.Count; i++)
                {
                    DropDownList ddl = (DropDownList)grdView.Rows[i].Cells[6].FindControl("ddlcostcenter");
                    ddl.ClearSelection();
                    ddl.Items.FindByText(currenttable.Rows[i]["costcenter"].ToString()).Selected = true;


                }
            
            }
                
        }
        protected void FillDdlSource()
        {
            ddlSource.DataSource = vdBl.GetSource(VoucherTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSource.DataTextField = "desc";
            ddlSource.DataValueField = "vtcd";
            ddlSource.DataBind();
        }
        private void ClearFields()
        {
            //VoucherTypeID = Convert.ToInt32(Session["VoucherTypeId"].ToString());
            VoucherID = 0;
            //Session["aFlag"] = true;
            txtnarration.Text = "";
            //txtDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
            txtRefSource.Text = "";
            ddlSource.Enabled = true;
            txtNtnNumbere.Text = "";
            if (VoucherTypeID == 3 || VoucherTypeID == 5)
            {
                divBankData.Visible = true;
                if (Session["DateFullYearFormat"] == null)
                {
                    txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Session["DateFullYearFormat"].ToString());
                }
                txtChqBranch.Text = "";
                txtChqAcctNo.Text = "";
                txtChqNo.Text = "";
            }
            else
            {
                divBankData.Visible = false;
            }

            //SOURCE & REF NO MANAGEMENT
            if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
            {
                lblRefSource.Text = "Voucher No";
            }
            else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
            {
                lblRefSource.Text = "Voucher No";
            }
            else//JV, OP & IP
            {
                lblRefSource.Visible = false;
                txtRefSource.Visible = false;
            }
            //END SOURCE & REF NO MANAGEMENT

            DataTable d_table = new DataTable();
            d_table.Columns.Clear();
            d_table.Rows.Clear();
            d_table.Columns.Add(new DataColumn("seq", typeof(string)));
            d_table.Columns.Add(new DataColumn("code", typeof(string)));
            d_table.Columns.Add(new DataColumn("desc", typeof(string)));
            d_table.Columns.Add(new DataColumn("debit", typeof(string)));
            d_table.Columns.Add(new DataColumn("credit", typeof(string)));
            d_table.Columns.Add(new DataColumn("remark", typeof(string)));
            d_table.Columns.Add(new DataColumn("costcenter", typeof(string)));
            d_table.Columns.Add(new DataColumn("balance", typeof(string)));
            flagEdit = false;
            for (int i = 1; i < 6; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["seq"] = i.ToString();
                d_table.Rows.Add(d_Row);

            }
            currenttable = d_table;
            BindGrid();
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string GetCodeDesc(string glCd)
        {
            voucherDetailBL vrBl = new voucherDetailBL();
            return vrBl.GetCodeDesc(glCd, Convert.ToInt32(HttpContext.Current.Session["BranchID"]), (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<spGetBankA_CResult> GetBranch(string bank)
        {
            voucherDetailBL vrBl = new voucherDetailBL();
            List<spGetBankA_CResult> acc = vrBl.GetBranch(Convert.ToInt32(HttpContext.Current.Session["BranchID"]), bank, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return acc;
        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static object GetGlCodeRec(string glcd)
        {
            voucherDetailBL vrBl = new voucherDetailBL();
            object data = vrBl.GetGLTypeByGlCd(glcd, Convert.ToInt32(HttpContext.Current.Session["BranchID"]), (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return data;
        }

        #endregion 

    }
}

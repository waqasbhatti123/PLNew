using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using Microsoft.Reporting.WebForms;

namespace RMS.GLSetup
{
    public partial class frmViewVoucher : System.Web.UI.Page
    {
        voucherHomeBL voucObj = new voucherHomeBL();
        voucherDetailBL vouchDetBL = new voucherDetailBL();
        GroupBL grpBl = new GroupBL();
        decimal debit = 0;
        decimal totaldebit = 0;
        decimal credit = 0;
        decimal totalcredit = 0;
        public int VRID
        {

            get { return Convert.ToInt32(Session["VRID"]); }
            set { Session["VRID"] = value; }
        }
        public int VoucherTypeID
        {
            get { return Convert.ToInt32(ViewState["VoucherTypeID"]); }
            set { ViewState["VoucherTypeID"] = value; }
        }

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        public int PrevPID
        {
            get { return Convert.ToInt32(ViewState["PrevPID"]); }
            set { ViewState["PrevPID"] = value; }
        }

        public int GroupID
        {
            get { return Convert.ToInt32(ViewState["GroupID"]); }
            set { ViewState["GroupID"] = value; }
        }

        public string VoucherStatus
        {
            get { return ViewState["VoucherStatus"].ToString(); }
            set { ViewState["VoucherStatus"] = value; }
        }


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
                //UrlReferrer = Request.UrlReferrer.ToString();

                VoucherTypeID = Convert.ToInt32(Session["VoucherTypeId"]);
                if (VoucherTypeID == 3 || VoucherTypeID == 5 || VoucherTypeID == 6)
                {
                    divBankData.Visible = true;
                }
                else
                {
                    divBankData.Visible = false;
                }
                VRID = Convert.ToInt32(Session["VoucherId"]);
                GetVouchData(VRID);
                BindGrid(VRID);


                PID = Convert.ToInt32(Request.QueryString["PID"]);

                //Maintaning Privilage Status==========================
                if (Session["GroupID"] == null)
                {
                    GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                }
                else
                {
                    GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                }
                PrevPID = 0;
                PrevPID = Convert.ToInt32(Session["PrevPgID"]);

                if (PrevPID == 365)
                    btnUnpost.Visible = true;
                else
                    btnUnpost.Visible = false;
                
                tblAppPrivilage appPrivilage = grpBl.GetPrivilageStatus(GroupID, PrevPID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (appPrivilage != null)
                {
                    //CanAdd = appPrivilage.CanAdd;
                    //CanApprove = appPrivilage.CanEdit;
                    if (appPrivilage.CanAdd.Equals(true))
                    {
                      //  divRemEntry.Visible = false;
                    }
                    if (appPrivilage.CanEdit.Equals(true))//Can Approve Status
                    {
                        if (Convert.ToBoolean(Session["Flag"]) == false)
                        {
                            btnApprove.Visible = true;
                        }
                    }
                }
                else
                {
                    //ucMessage.ShowMessage("Enter privilages for the logged in group", RMS.BL.Enums.MessageType.Error);
                }
                //=================================================

                if (Convert.ToBoolean(Session["Flag"]) == true)
                {
                    divRem.Visible = true;
                    divRemEntry.Visible = false;
                }
                else
                {
                    divRem.Visible = true;
                    divRemEntry.Visible = true;

                    btnCancel.Visible = true;
                }

                BindGridRems(VRID);
            }
        }

        protected void btnUnpost_Click(object sender, EventArgs e)
        {
            try
            {
                string username = ""; int userid = 0;
                if (Session["UserID"] == null)
                {
                    userid = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                }
                else
                {
                    userid = Convert.ToInt32(Session["UserID"].ToString());
                }
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
                VoucherStatus = lblstatus.Text;
                Byte[] bytes = CreatePdfByteArray(Session["PageTitle"].ToString(), "pdf", VRID);



                string msg =  voucObj.Unpost((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], VRID, username, userid, bytes);
               

                if (PrevPID.Equals(361))
                {
                    Response.Redirect("~/GLSetup/frmGLVoucher.aspx?PID=" + PrevPID);
                }
                else if (PrevPID.Equals(365))
                {
                    Session["UnpostMsg"] = msg;
                    Response.Redirect("~/GLSetup/frmGLVoucherUnpost.aspx?PID=" + PrevPID);
                }
                else
                {
                    Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + PrevPID);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

//        public static int databaseFilePut(byte[] bytes)
//        {
//            int varID = 0;
//            byte[] file = bytes;
//            const string preparedCommand = @"
//                    INSERT INTO [dbo].[Raporty]
//                               ([RaportPlik])
//                         VALUES
//                               (@File)
//                        SELECT [RaportID] FROM [dbo].[Raporty]
//            WHERE [RaportID] = SCOPE_IDENTITY()
//                    ";
//            using (var varConnection = Locale.sqlConnectOneTime(Locale.sqlDataConnectionDetails))
//            using (var sqlWrite = new SqlCommand(preparedCommand, varConnection))
//            {
//                sqlWrite.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;

//                using (var sqlWriteQuery = sqlWrite.ExecuteReader())
//                    while (sqlWriteQuery != null && sqlWriteQuery.Read())
//                    {
//                        varID = sqlWriteQuery["RaportID"] is int ? (int)sqlWriteQuery["RaportID"] : 0;
//                    }
//            }
//            return varID;
//        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                string username = "";
                char flag = 'A';
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

                voucObj.Save((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], VRID, flag, username);
                vouchDetBL.PostVoucher4mHome((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], VRID, username);

                if (PrevPID.Equals(361))
                {
                    Response.Redirect("~/GLSetup/frmGLVoucher.aspx?PID=" + PrevPID);
                }
                else if (PrevPID.Equals(365))
                {
                    Response.Redirect("~/GLSetup/frmGLVoucherUnpost.aspx?PID=" + PrevPID);
                }
                else
                {
                    Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + PrevPID);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                voucObj.CancelVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], VRID);

                if (PrevPID.Equals(361))
                {
                    Response.Redirect("~/GLSetup/frmGLVoucher.aspx?PID=" + PrevPID);
                }
                else if (PrevPID.Equals(365))
                {
                    Response.Redirect("~/GLSetup/frmGLVoucherUnpost.aspx?PID=" + PrevPID);
                }
                else
                {
                    Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + PrevPID);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                VoucherStatus = lblstatus.Text;

                CreatePDF(Session["PageTitle"].ToString(), "pdf", VRID);
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void grdRemarks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdRemarks.PageIndex = e.NewPageIndex;
            BindGridRems(VRID);
        }

        protected void grdRemarks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text).ToString(System.Configuration.ConfigurationManager.AppSettings["DateTimeWOYearFormat"]);
                
                Glmf_Data_Rmk rem = voucObj.GetRemarksByID((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], VRID, Convert.ToInt32(e.Row.Cells[0].Text));
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!txtRemarks.Text.Equals(""))
            {
                Glmf_Data_Rmk glmfRems = new Glmf_Data_Rmk();
                glmfRems.vrid = VRID;
                glmfRems.Rmk_seq = voucObj.RemarksSeq((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], VRID);
                glmfRems.Remark = txtRemarks.Text;
                glmfRems.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (Session["UserID"] == null)
                {
                    glmfRems.updateby = Request.Cookies["uzr"]["UserID"];
                }
                else
                {
                    glmfRems.updateby = Session["UserID"].ToString();
                } 
                glmfRems.Enabled = false;

                if (voucObj.SaveRemarks((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], glmfRems))
                {
                    txtRemarks.Text = "";
                    BindGridRems(VRID);
                    ucMessage.ShowMessage("Remarks saved successfully", RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage("Unable to save remarks at this time please try later", RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                ucMessage.ShowMessage("Insert remarks first", RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void grdVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
             
                Label lbldebit= e.Row.FindControl("lbldebit") as Label;
                debit = Convert.ToDecimal(lbldebit.Text);
                Label lblcredit = e.Row.FindControl("lblcredit") as Label;
                credit = Convert.ToDecimal(lbldebit.Text);
                totaldebit = totaldebit + debit;
                totalcredit = totalcredit + credit;
            //    e.Row.Cells[4].Text =  _totalUnitsInStock.ToString();
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "TOTAL";
                e.Row.Cells[3].Text = totaldebit.ToString();
                e.Row.Cells[4].Text = totalcredit.ToString();
            
            
            }

        
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            if (PrevPID.Equals(361))
            {
                Response.Redirect("~/GLSetup/frmGLVoucher.aspx?PID=" + PrevPID);
            }
            else if (PrevPID.Equals(365))
            {
                Response.Redirect("~/GLSetup/frmGLVoucherUnpost.aspx?PID=" + PrevPID);
            }
            else
            {
                Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + PrevPID);
            }
        }

        protected void grdVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdVoucher.PageIndex = e.NewPageIndex;
            BindGrid(VRID);
        }

        protected void BindGridRems(int id)
        {
            grdRemarks.DataSource = voucObj.GetRemarks((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], id);
            grdRemarks.DataBind();
        }

        protected void BindGrid(int id)
        {
            grdVoucher.DataSource = voucObj.GetReport((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], id);
            grdVoucher.DataBind();
        }

        protected void GetVouchData(int id)
        {
            string vrNarration = voucObj.GetNarration((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], VRID);
            if (Session["VouchNo"] != null)
                lblvoucher.Text = Session["VouchNo"].ToString();
            //DateTime dt = Convert.ToDateTime(Session["DateVal"]);
            lbldate.Text = Convert.ToDateTime(Session["DateVal"]).ToString(System.Configuration.ConfigurationManager.AppSettings["DateTimeFormat"]);//dt.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            if (Session["Status"] != null)
                lblstatus.Text = Session["Status"].ToString();
            lblnarration.Text = vrNarration;

            if (VoucherTypeID == 3 || VoucherTypeID == 5 || VoucherTypeID == 6)
            {
                Glmf_Data_chq glmfChq = voucObj.GetCheqDetByID(VRID, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                string bank = "";
                try
                {
                    bank = voucObj.GetBankByCode(glmfChq.vr_chq_branch, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                    if(!string.IsNullOrEmpty(bank))
                    {
                        bank = " - "+ bank;
                    }
                }
                catch { }
                try
                {
                    lblChqAcctNo.Text = glmfChq.vr_chq_ac;
                    lblChqBranch.Text = glmfChq.vr_chq_branch + bank;
                    if (Session["DateFullYearFormat"] == null)
                    {
                        lblChqDate.Text = glmfChq.vr_chq_dt.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    }
                    else
                    {
                        lblChqDate.Text = glmfChq.vr_chq_dt.ToString(Session["DateFullYearFormat"].ToString());
                    }

                    lblChqNo.Text = glmfChq.vr_chq;
                }
                catch { }
            }
            else
            {

            }

            Glmf_Data glmfData = voucObj.GetGlmf_DataByID(VRID, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                //SOURCE & REF NO MANAGEMENT
                if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
                {
                    lblRefSourceHdr.Visible = true;
                    lblRefSource.Visible = true;
                    lblRefSourceHdr.Text = "Source: ";
                    if (glmfData != null)
                    {
                        lblRefSource.Text = glmfData.source;
                    }
                }
                else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
                {
                    lblRefSourceHdr.Visible = true;
                    lblRefSource.Visible = true;
                    lblRefSourceHdr.Text = "Ref. No.: ";
                    if (glmfData != null)
                    {
                        lblRefSource.Text = glmfData.Ref_no;
                    }
                }
                else//JV, OP & IP
                {
                    lblRefSourceHdr.Visible = false;
                    lblRefSource.Visible = false;
                }

            //END SOURCE & REF NO MANAGEMENT
        }

        protected Byte[] CreatePdfByteArray(String FileName, String extension, int vrid)
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
            "  <MarginLeft>0.1in</MarginLeft>" +
            "  <MarginRight>0.1in</MarginRight>" +
            "  <MarginBottom>0.1in</MarginBottom>" +
            "</DeviceInfo>";

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            //IQueryable<spRptEmployeeListResult> sal;
            //sal = SalBl.RptEmployeeRec(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //
            if (PID == 324 || PID == 325 || VoucherTypeID == 3 || VoucherTypeID == 5 || VoucherTypeID == 6)
            {
                IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR.rdlc";
                datasource = new ReportDataSource("spBPnBRResult", sal);
            }

            else
            {
                IList<spTestGLResult> sal = voucObj.GetReport((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptJV.rdlc";
                datasource = new ReportDataSource("spTestGLResult", sal);


            }

            string brName = "";
            string brAddress = "";
            string brTel = "";
            string brFax = "";
            try
            {
                Branch branch = voucObj.GetBranch(1, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                if (branch != null)
                {
                    brName = branch.br_nme;
                    brAddress = branch.br_address;
                    brTel = branch.br_tel;
                    brFax = branch.br_fax;
                }
            }
            catch { }
            //ReportParameter prm = new ReportParameter("Loan_ReportResult");
            ReportParameter[] paramz = new ReportParameter[7];

            //paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }

            paramz[1] = new ReportParameter("ReportName", FileName, false);
            paramz[2] = new ReportParameter("voucherstatus", VoucherStatus, false);
            paramz[3] = new ReportParameter("brName", brName, false);
            paramz[4] = new ReportParameter("brAddress", brAddress, false);
            paramz[5] = new ReportParameter("brTel", brTel, false);
            paramz[6] = new ReportParameter("brFax", brFax, false);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.SetParameters(paramz);
            ReportViewer1 = viewer;

            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            return bytes;
        }

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
            //IQueryable<spRptEmployeeListResult> sal;
            //sal = SalBl.RptEmployeeRec(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //
            string brName = "";
            string brAddress = "";
            string brTel = "";
            string brFax = "";
            string rptLogoPath = "";
            string updateby = "";
            string approvedby = "";

            if (PID == 324 || PID == 325 || VoucherTypeID == 3 || VoucherTypeID == 5 || VoucherTypeID == 6)
            {
                IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR.rdlc";
                datasource = new ReportDataSource("spBPnBRResult", sal);

                updateby = sal.First().updateby;
                approvedby = sal.First().approvedby;
            }

            else
            {
                IList<spTestGLResult> sal = voucObj.GetReport((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptJV.rdlc";
                datasource = new ReportDataSource("spTestGLResult", sal);

                updateby = sal.First().updateby;
                approvedby = sal.First().approvedby;
            }
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            try
            {
                Branch branch = voucObj.GetBranch(1, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                if (branch != null)
                {
                    brName = branch.br_nme;
                    brAddress = branch.br_address;
                    brTel = branch.br_tel;
                    brFax = branch.br_fax;
                }
            }
            catch { }
            viewer.LocalReport.EnableExternalImages = true;
            //ReportParameter prm = new ReportParameter("Loan_ReportResult");
            ReportParameter[] paramz = new ReportParameter[10];

            //paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }

            paramz[1] = new ReportParameter("ReportName", FileName, false);
            paramz[2] = new ReportParameter("voucherstatus", VoucherStatus == "Pending" ? "Posted" : VoucherStatus, false);
            paramz[3] = new ReportParameter("brName", brName, false);
            paramz[4] = new ReportParameter("brAddress", brAddress, false);
            paramz[5] = new ReportParameter("brTel", brTel, false);
            paramz[6] = new ReportParameter("brFax", brFax, false);
            paramz[7] = new ReportParameter("LogoPath", rptLogoPath, false);
            paramz[8] = new ReportParameter("AprBy", approvedby, false);
            paramz[9] = new ReportParameter("UpdBy", updateby, false);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.SetParameters(paramz);
            ReportViewer1 = viewer;

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
    }
}

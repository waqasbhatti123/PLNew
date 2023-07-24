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
    public partial class frmBankPaymentAdvice : BasePage
    {
        #region Properties

        voucherDetailBL vdBl = new voucherDetailBL();
        voucherHomeBL voucObj = new voucherHomeBL();
        ChequePrintBL cP = new ChequePrintBL();
        EntitySet<Glmf_Data_Det> gldatadet = new EntitySet<Glmf_Data_Det>();
        GroupBL grpBl = new GroupBL();

        voucherDetailBL objVoucher = new voucherDetailBL();
#pragma warning disable CS0169 // The field 'frmBankPaymentAdvice.d_Row' is never used
        DataRow d_Row;
#pragma warning restore CS0169 // The field 'frmBankPaymentAdvice.d_Row' is never used
#pragma warning disable CS0169 // The field 'frmBankPaymentAdvice.d_Col' is never used
        DataColumn d_Col;
#pragma warning restore CS0169 // The field 'frmBankPaymentAdvice.d_Col' is never used
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


        #endregion

        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            try {
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
                        BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        BranchID = Convert.ToInt32(Session["BranchID"].ToString());
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
                    PID = Convert.ToInt32(Request.QueryString["PID"]);

                    // Now assigning value to voucherTypeID 
                    switch (PID)
                    {
                        case 324:
                            {
                                VoucherTypeID = 3;
                                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "BP").ToString();
                                break;
                            }
                        case 962:
                            {
                                VoucherTypeID = 3;
                                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "BA").ToString();
                                if (Session["DateFullYearFormat"] == null)
                                {
                                    txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                                }
                                else
                                {
                                    txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Session["DateFullYearFormat"].ToString());
                                }
                                break;
                            }
                    }
                    Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    txtdt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    if (Session["DateFullYearFormat"] == null)
                    {
                        this.txtDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    }
                    else
                    {
                        this.txtDate.Format = Session["DateFullYearFormat"].ToString();
                    }

                    //===================== Maintaning Privilage Status==========================
                    if (Session["GroupID"] == null)
                    {
                        GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                    }
                    else
                    {
                        GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                    }
                    tblAppPrivilage appPrivilage = grpBl.GetPrivilageStatus(GroupID, PID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    if (appPrivilage != null)
                    {
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
                    this.BindGrid(VoucherTypeID);
                    FillSearchBranchDropDown();
                    searchBranchDropDown.SelectedValue = BranchID.ToString();

                }
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, BL.Enums.MessageType.Error);
            }
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            this.BindGrid(VoucherTypeID);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GLSetup/frmBankPaymentAdvice.aspx?PID=" + PID);
        }
        protected void grdVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdVoucher.PageIndex = e.NewPageIndex;
            BindGrid(VoucherTypeID);
        }
        protected void grdVoucher_SelectedIndexChanging(object sender, EventArgs e)
        {
            VoucherID = Convert.ToInt32(grdVoucher.SelectedDataKey.Values["vrid"]);
            this.GetData();
        }     
        protected void grdVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime currdate = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
            if (VoucherTypeID != 6 && Convert.ToDateTime(txtdt.Text.Trim()) > currdate)
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
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;

            VoucherID = Convert.ToInt32(grdVoucher.DataKeys[clickedRow.RowIndex].Values["vrid"]);
            Label lblstatus = (Label)clickedRow.FindControl("lblstatus");
            VoucherStatus = lblstatus.Text;
            CreatePDF(Session["PageTitle"].ToString(), "pdf", VoucherID);
        }
        protected void btnPrintCheque_Click(object sender, EventArgs e)
        {
            PrintCheque(VoucherID);
        }

        #endregion

        #region helping Methods

        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            this.searchBranchDropDown.DataTextField = "br_nme";
            searchBranchDropDown.DataValueField = "br_id";
            if (BranchID == (int)BranchEnum.HeadQuerterBranchID)
            {
                searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else if (BranchID > (int)BranchEnum.HeadQuerterBranchID)
            {
                List<Branch> BranchList = new List<Branch>();
                Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
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
                    
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    this.BindGrid(VoucherTypeID);
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
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
            string brName = "";
            string brAddress = "";
            string brTel = "";
            string brFax = "";
            string rptLogoPath = "";
            string updateby = "";
            string approvedby = "";
            if (PID == 962)
            {
                IList<spBPnBRResult> sal = voucObj.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR.rdlc";
                datasource = new ReportDataSource("spBPnBRResult", sal);
                updateby = sal.First().updateby;
                approvedby = sal.First().approvedby;
            }

            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            try
            {
                Branch branch = voucObj.GetBranch(BranchID, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                if (branch != null)
                {
                    brName = branch.br_nme;
                    brAddress = branch.br_address;
                    brTel = branch.br_tel;
                    brFax = branch.br_fax;
                }
            }
            catch { }
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
            paramz[2] = new ReportParameter("voucherstatus", VoucherStatus, false);
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
            ddlstatus.SelectedValue = "P";
            ddlFltrStatus.Items.Add(list);
            
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

            grdVoucher.DataSource = voucObj.GetDataForBPAGrid(vt_cd, Convert.ToDateTime(txtFrom.Text), Convert.ToDateTime(txtTo.Text), Convert.ToChar(ddlFltrStatus.SelectedItem.Value), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdVoucher.DataBind();

        }
        public void GetData()
        {
            List<spVouchersFill2GridResult> dtl = vdBl.GetVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], VoucherID, BranchID);

            if (VoucherID != 0 && dtl.Count > 0)
            {
                foreach (spVouchersFill2GridResult l in dtl)
                {
                    if(l.vr_seq == 1)
                    {
                        flagEdit = true;
                        txtdt.Text = Convert.ToDateTime(l.vr_dt).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                        ddlstatus.SelectedValue = l.vr_apr.ToString();
                        if (l.vr_apr.ToString().ToLower() == "a" || l.vr_apr.ToString().ToLower() == "d")
                        {
                            btnSave.Enabled = false;
                        }
                        else
                        {
                            btnSave.Enabled = true;
                        }
                        txtnarration.Text = l.narration.ToString();
                        txtRefSource.Text = l.Ref_no;
                        txtAmountPayable.Text = l.vrd_debit.ToString();
                        txtPayeeAc.Text = l.gl_cd + " ~ " + l.gl_dsc;
                        hdnAccPayee.Value = l.gl_cd;
                    }
                    else if (l.vr_seq == 2)
                    {
                        txtChqBranch.Text = l.gl_cd + " ~ " + l.gl_dsc;
                        hdnAccNo.Value = l.gl_cd;
                    }
                    else if (l.vr_seq == 3)
                    {
                        txtAcWHT.Text = l.gl_cd + " ~ " + l.gl_dsc;
                        txtWHT.Text = l.vrd_credit.ToString();
                        hdnAcWHT.Value = l.gl_cd;
                    }
                }
                SetVoucherCheqDet();
            }
            else
            {
                flagEdit = false;
            }
        }
        private void SetVoucherCheqDet()
        {
            Glmf_Data_chq glmfChq = objVoucher.GetCheqDetByID(VoucherID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (glmfChq != null)
            {
                ////txtChqBranch.Text = glmfChq.vr_chq_branch;
                txtChqNo.Text = glmfChq.vr_chq;
                //txtPayee.Text = glmfChq.payee;
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
        public void EditPrevious()
        {
            Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            gldatadet.Clear();


            if (txtWHT.Text.Trim() != "" && txtAcWHT.Text.Trim() == "")
            {
                ucMessage.ShowMessage("Please enter WHT account.", BL.Enums.MessageType.Error);
                return;
            }

            ////CHEQUE DETAILS VALIDATIONS------------------------------------------------

            Glmf_Data_chq glmfChq = null;
            if (VoucherTypeID == 3)
            {
                glmfChq = objVoucher.GetGlmf_Data_Chq(VoucherID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                if (glmfChq != null)
                {
                    glmfChq.vr_chq_branch = hdnAccNo.Value;
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
                    //glmfChq.payee = txtPayee.Text.Trim();
                }

            }
            try
            {
                if (VoucherTypeID == 3)
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
            


            ////USER NAME VALIDATIONS

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


            ////UPDATE Glmf_Data

            int vrID = VoucherID;
            Glmf_Data glmfdata = objVoucher.GetGlmf_Data(VoucherID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (glmfdata != null)
            {
                ////FINANCIAL YEAR VALIDATIONS
                try
                {
                    glmfdata.vr_dt = Convert.ToDateTime(txtdt.Text.Trim());
                    DateTime dtFrm = DateTime.Parse("01-Jul-" + Financialyear);
                    dtFrm = dtFrm.AddYears(-1);
                    DateTime dtTo = DateTime.Parse("30-Jun-" + Financialyear);
                    if (glmfdata.vr_dt >= dtFrm && glmfdata.vr_dt <= dtTo)
                    {
                        //ok
                    }
                    else if (glmfdata.vr_dt >= DateTime.Parse("01-Jul-" + Financialyear)
                    && glmfdata.vr_dt <= DateTime.Parse("30-Jun-" + (Financialyear + 1))
                    && glmfdata.vr_dt <= Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date
                    )
                    {
                        Financialyear++;
                    }
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
                glmfdata.Ref_no = txtRefSource.Text;



                //GET VOUCHER DETAIL------------------------------------------------------------------
                int count = 1;


                ////DEBIT
                Glmf_Data_Det gldet = new Glmf_Data_Det();
                gldet.vrid = glmfdata.vrid;
                gldet.vr_seq = count++;
                if (!string.IsNullOrEmpty(hdnAccPayee.Value))
                    gldet.gl_cd = hdnAccPayee.Value;
                else
                {
                    ucMessage.ShowMessage("Invalid payee account", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                gldet.vrd_debit = Convert.ToDecimal(txtAmountPayable.Text.Trim());
                gldet.vrd_credit = 0;
                gldet.vrd_nrtn = "";
                gldet.cc_cd = null;
                gldatadet.Add(gldet);


                ////CREDIT
                gldet = new Glmf_Data_Det();
                gldet.vrid = glmfdata.vrid;
                gldet.vr_seq = count++;
                if (!string.IsNullOrEmpty(hdnAccNo.Value))
                    gldet.gl_cd = hdnAccNo.Value;
                else
                {
                    ucMessage.ShowMessage("Invalid bank/branch account", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                gldet.vrd_debit = 0;
                gldet.vrd_credit = Convert.ToDecimal(txtAmountPayable.Text.Trim()) - Convert.ToDecimal(txtWHT.Text.Trim() != "" ? txtWHT.Text.Trim() : "0");
                gldet.vrd_nrtn = "";
                gldet.cc_cd = null;
                gldatadet.Add(gldet);


                ////CREDIT
                if (!string.IsNullOrEmpty(txtWHT.Text.Trim()) && !string.IsNullOrEmpty(hdnAcWHT.Value))
                {
                    gldet = new Glmf_Data_Det();
                    gldet.vrid = glmfdata.vrid;
                    gldet.vr_seq = count++;
                    gldet.gl_cd = hdnAcWHT.Value;
                    gldet.vrd_debit = 0;
                    gldet.vrd_credit = Convert.ToDecimal(txtWHT.Text.Trim());
                    gldet.vrd_nrtn = "";
                    gldet.cc_cd = null;
                    gldatadet.Add(gldet);
                }
                else
                {
                    //do nothing
                }
               
            }

            
            ////SAVE DATA
            string vrDescptn = objVoucher.VoucherDesc(VoucherTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                objVoucher.UpdateVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                voucherNo = glmfdata.vr_no;
                voucObj.UpdateRemarksStatus((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], vrID);


                if (VoucherTypeID == 3)
                {
                    objVoucher.UdateChq((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfChq);
                }
                if (glmfdata.vr_apr.ToString().Equals("A"))
                {
                    objVoucher.PostVoucherDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                }
                ucMessage.ShowMessage("voucher " + vrDescptn + '-' + voucherNo + " has been updated successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid(VoucherTypeID);
                ClearFields();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("An attempt was made to remove a relationship between a Glmf_Data and a Glmf_Data_Det. However, one of the relationship's foreign keys (Glmf_Data_Det.vrid) cannot be set to null."))
                {
                    ucMessage.ShowMessage("Please enter GL Code", RMS.BL.Enums.MessageType.Error);
                }
                else
                {
                    ucMessage.ShowMessage("Exception occured in updating data.<br/>Exception is: " + e.Message, RMS.BL.Enums.MessageType.Error);
                }
                return;
            }
        }
        public void SaveNew()
        {
            Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            if(txtWHT.Text.Trim() != "" && txtAcWHT.Text.Trim() == "")
            {
                ucMessage.ShowMessage("Please enter WHT account.", BL.Enums.MessageType.Error);
                return;
            }

            ////CHEQUE DETAILS VALIDATION------------------------------------------
            Glmf_Data_chq glmfChq = null;
            if (VoucherTypeID == 3)
            {
                glmfChq = new Glmf_Data_chq();
                if (!string.IsNullOrEmpty(hdnAccNo.Value))
                    glmfChq.vr_chq_branch = hdnAccNo.Value;
                else
                {
                    ucMessage.ShowMessage("Invlid bank/branch account", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                glmfChq.vr_chq = txtChqNo.Text.Trim();
                //glmfChq.payee = txtPayee.Text.Trim();
                try
                {
                    glmfChq.vr_chq_dt = Convert.ToDateTime(txtChqDate.Text.Trim());
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                    {
                        ucMessage.ShowMessage("Cheque date is invalid", RMS.BL.Enums.MessageType.Error);
                        txtChqDate.Focus();
                    }
                    return;
                }

                glmfChq.vr_chq_ac = txtChqAcctNo.Text.Trim();
            }
            try
            {
                if (VoucherTypeID == 3)
                {
                    Glmf_Data_chq chq = new Glmf_Data_chq();
                    chq.vr_chq_branch = hdnAccNo.Value;
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



            ////USER NAME VALIDATION-----------------------------------------------------------------
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


            Glmf_Data glmfdata = new Glmf_Data();

            ////FINANCIAL YEAR VALIDATION-----------------------------------------------------------------
            try
            {
                glmfdata.vr_dt = Convert.ToDateTime(txtdt.Text.Trim());
                DateTime dtFrm = DateTime.Parse("01-Jul-" + Financialyear);
                dtFrm = dtFrm.AddYears(-1);
                DateTime dtTo = DateTime.Parse("30-Jun-" + Financialyear);
                if (glmfdata.vr_dt >= dtFrm && glmfdata.vr_dt <= dtTo)
                {
                    //ok
                }
                else if (glmfdata.vr_dt >= DateTime.Parse("01-Jul-" + Financialyear)
                    && glmfdata.vr_dt <= DateTime.Parse("30-Jun-" + (Financialyear + 1))
                    && glmfdata.vr_dt <= Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date
                    )
                {
                    Financialyear++;
                }
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


            ////GET VOUCHER MASTER-----------------------------------------------------------------
            glmfdata.source = "BPA";

            int voucherno = 0;
            voucherno = objVoucher.GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], BranchID, VoucherTypeID, Financialyear, glmfdata.source);
            glmfdata.br_id = BranchID;
            glmfdata.Gl_Year = Financialyear;
            glmfdata.vt_cd = Convert.ToInt16(VoucherTypeID);
            glmfdata.vr_no = voucherno;
            glmfdata.vr_nrtn = txtnarration.Text.Trim();
            glmfdata.vr_apr = Convert.ToString(ddlstatus.SelectedItem.Value);


            //SOURCE & REF NO MANAGEMENT---------------------------------------------------------------
            if (VoucherTypeID.Equals(3))
            {
                glmfdata.Ref_no = txtRefSource.Text;
            }
            else//JV, OP & IP
            {
            }


            //VOUCHER STATUS----------------------------------------------------------------------
            glmfdata.updateby = username;
            glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (glmfdata.vr_apr == "A")
            {
                glmfdata.approvedby = username;
                glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }



            //GET VOUCHER DETAIL------------------------------------------------------------------
            int count = 1;


            ////DEBIT
            Glmf_Data_Det gldet = new Glmf_Data_Det();
            gldet.vr_seq = count++;
            if (!string.IsNullOrEmpty(hdnAccPayee.Value))
                gldet.gl_cd = hdnAccPayee.Value;
            else
            {
                ucMessage.ShowMessage("Invalid payee account", RMS.BL.Enums.MessageType.Error);
                return;
            }
            gldet.vrd_debit = Convert.ToDecimal(txtAmountPayable.Text.Trim());
            gldet.vrd_credit = 0;
            gldet.vrd_nrtn = "";
            gldet.cc_cd = null;
            gldatadet.Add(gldet);


            ////CREDIT
            gldet = new Glmf_Data_Det();
            gldet.vr_seq = count++;
            if (!string.IsNullOrEmpty(hdnAccNo.Value))
                gldet.gl_cd = hdnAccNo.Value;
            else
            {
                ucMessage.ShowMessage("Invalid bank/branch account", RMS.BL.Enums.MessageType.Error);
                return;
            }
            gldet.vrd_debit = 0;
            gldet.vrd_credit = Convert.ToDecimal(txtAmountPayable.Text.Trim()) - Convert.ToDecimal(txtWHT.Text.Trim() != "" ? txtWHT.Text.Trim() : "0");
            gldet.vrd_nrtn = "";
            gldet.cc_cd = null;
            gldatadet.Add(gldet);


            ////CREDIT
            if (!string.IsNullOrEmpty(txtWHT.Text.Trim()) && !string.IsNullOrEmpty(hdnAcWHT.Value))
            {
                gldet = new Glmf_Data_Det();
                gldet.vr_seq = count++;
                gldet.gl_cd = hdnAcWHT.Value;
                gldet.vrd_debit = 0;
                gldet.vrd_credit = Convert.ToDecimal(txtWHT.Text.Trim());
                gldet.vrd_nrtn = "";
                gldet.cc_cd = null;
                gldatadet.Add(gldet);
            }
            else
            {
                //do nothing
            }
            

    
            ////SAVE DATA
            string vrDescptn = objVoucher.VoucherDesc(VoucherTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                objVoucher.SaveVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                if (VoucherTypeID == 3)
                {
                    objVoucher.SaveVoucherCheqDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata.vrid, glmfChq);
                }
                if (glmfdata.vr_apr.ToString().Equals("A"))
                {
                    objVoucher.PostVoucherDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                }
                ucMessage.ShowMessage("Voucher " + vrDescptn + '-' + voucherno + " has been posted successfully", RMS.BL.Enums.MessageType.Info);
                BindGrid(VoucherTypeID);
                ClearFields();
            }
            catch (Exception e)
            {
                ucMessage.ShowMessage("Exception occured in saving data.<br/>Exception is: " + e.Message, RMS.BL.Enums.MessageType.Error);
                return;
            }
        }
        private void ClearFields()
        {
            VoucherID = 0;
            txtnarration.Text = "";
            //txtDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
            txtPayee.Text = "";
            txtPayeeAc.Text = "";
            txtAmountPayable.Text = "";
            txtWHT.Text = "";
            txtAcWHT.Text = "";
            txtChequeAmount.Text = "";
            txtRefSource.Text = "";

            hdnAccNo.Value = "";
            hdnAccPayee.Value = "";
            hdnAcWHT.Value = "";

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
            flagEdit = false;
        }
        public void CreateLog()
        {
            ChequeParameters paramz = new ChequeParameters();
            paramz.PayAc = hdnAccNo.Value;
            paramz.PayeeAc = hdnAccPayee.Value;
            paramz.Payee = txtPayee.Text;
            paramz.AcPayeeOnly = chkAcPayeeOnly.Checked;
            paramz.Amount = Convert.ToDecimal(txtAmountPayable.Text.Trim()) - Convert.ToDecimal(txtWHT.Text.Trim() != "" ? txtWHT.Text.Trim() : "0");
            paramz.ChequeDate = Convert.ToDateTime(txtChqDate.Text);
            paramz.CreateBy = Convert.ToInt32(Session["UserID"]);
            paramz.CreateDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            string result = new ChequeOperations().CreateLog(paramz);
        }
        public void PrintCheque(int vrId)
        {
            try
            {
                if(vrId == 0)
                {
                    ucMessage.ShowMessage("Please select a record to print cheque.", BL.Enums.MessageType.Info);
                    return;
                }
                string amountWords = "", amountWords1 = "", amountWords2 = "";
                char[] amountWordsArray;
                int whiteSpaceIndex = 0, arrayLength = 0, i = 0;
                int lineBreakCharactersCount = 0;
                ChequePrintBL cpBL = new ChequePrintBL();
               // tblChequePrint cp = cpBL.GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //lineBreakCharactersCount =Convert.ToInt32( cp.LineBreak);

                amountWords = Number.ConvertNumberToWord(Convert.ToDouble(txtAmountPayable.Text.Trim()) - Convert.ToDouble(txtWHT.Text.Trim() == "" ? "0" : txtWHT.Text.Trim()));
                amountWordsArray = amountWords.ToCharArray();
                arrayLength = amountWordsArray.Length;
                if(arrayLength >= lineBreakCharactersCount)
                {
                    for (i = 0; i < arrayLength; i++)
                    {
                        if (i <= lineBreakCharactersCount)
                        {
                            if(amountWordsArray[i] == ' ')//white space
                            {
                                whiteSpaceIndex = i;
                            }
                        }
                        else
                            break;
                    }
                    amountWords1 = amountWords.Substring(0, whiteSpaceIndex);
                    amountWords2 = amountWords.Substring(whiteSpaceIndex);
                }
                else
                {
                    amountWords1 = amountWords;
                    amountWords2 = "";
                }

               
                ChequeParameters paramz = new ChequeParameters();

                paramz.PayAc = hdnAccNo.Value;
                paramz.AmountWords1 = amountWords1;
                paramz.AmountWords2 = amountWords2;
                paramz.Payee = txtPayee.Text;
                paramz.Amount = Convert.ToDecimal(txtAmountPayable.Text.Trim()) - Convert.ToDecimal(txtWHT.Text.Trim() == "" ? "0" : txtWHT.Text.Trim());
                paramz.ChequeNo = txtChqNo.Text;

                string chequeDate = Convert.ToDateTime(txtChqDate.Text.Trim()).ToString("dd MM yyyy");
                char[] dateTimeArray = chequeDate.ToCharArray();

                for (int j = 0; j < dateTimeArray.Length; j++)
                {
                    if (j == 0)
                    {
                        paramz.d1 = Convert.ToString(dateTimeArray[j]);
                    }
                   else if (j == 1)
                    {
                        paramz.d2 = Convert.ToString(dateTimeArray[j]);
                    }
                   else if (j == 3)
                    {
                        paramz.m1 = Convert.ToString(dateTimeArray[j]);
                    }
                    else if (j == 4)
                    {
                        paramz.m2 = Convert.ToString(dateTimeArray[j]);
                    }
                    else if (j == 6)
                    {
                        paramz.y1 = Convert.ToString(dateTimeArray[j]);
                    }
                    else if (j == 7)
                    {
                        paramz.y2 = Convert.ToString(dateTimeArray[j]);
                    }
                    else if (j == 8)
                    {
                        paramz.y3 = Convert.ToString(dateTimeArray[j]);
                    }
                    else if (j == 9)
                    {
                        paramz.y4 = Convert.ToString(dateTimeArray[j]);
                    }
                }
                paramz.AcPayeeOnly = chkAcPayeeOnly.Checked;
                string result = new ChequeOperations().Print(paramz);
                if(result == "ok")
                {
                    ucMessage.ShowMessage("Please get cheque file.", BL.Enums.MessageType.Info);
                    CreateLog();
                }
                else
                {
                    ucMessage.ShowMessage(result, BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, BL.Enums.MessageType.Error);
            }
        }

        #endregion

        #region WebMethods


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<spGetBankA_CResult> GetBranch(string bank)
        {
            voucherDetailBL vrBl = new voucherDetailBL();
            List<spGetBankA_CResult> acc = vrBl.GetBranch(Convert.ToInt32(HttpContext.Current.Session["BranchID"]),bank, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
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


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string GetCodeDesc(string glCd)
        {
            voucherDetailBL vrBl = new voucherDetailBL();
            return vrBl.GetCodeDesc(glCd, Convert.ToInt32(HttpContext.Current.Session["BranchID"]), (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);

        }


        #endregion

    }
}

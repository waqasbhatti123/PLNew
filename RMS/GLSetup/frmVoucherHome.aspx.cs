using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using Microsoft.Reporting.WebForms;
using System.Web.Services;
using System.IO;
namespace RMS.GLSetup
{
    public partial class frmVoucherHome : BasePage
    {
        voucherHomeBL vouchHomeBL = new voucherHomeBL();
        voucherDetailBL vouchDetBL = new voucherDetailBL();
        GroupBL grpBl = new GroupBL();

        #region DataMembers

        public int GroupID
        {
            get { return Convert.ToInt32(ViewState["GroupID"]); }
            set { ViewState["GroupID"] = value; }
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

        public string VoucherStatus
        {
            get { return ViewState["VoucherStatus"].ToString(); }
            set { ViewState["VoucherStatus"] = value; }
        }

        public int VoucherId
        {
            get { return Convert.ToInt32(ViewState["VoucherId"]); }
            set { ViewState["VoucherId"] = value; }
        }

        public bool CanAdd
        {
            get { return Convert.ToBoolean(ViewState["CanAdd"]); }
            set { ViewState["CanAdd"] = value; }
        }
        
        public bool CanApprove
        {
            get { return Convert.ToBoolean(ViewState["CanApprove"]); }
            set { ViewState["CanApprove"] = value; }
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

        #region event

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
                    BranchID = Convert.ToInt32(Session["BranchID"]);
                }
                //DateTime dt =  Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                txtFrom.Text = new DateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year, Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month, 1).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                txtTo.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtFromDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    this.txtToDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    this.txtFromDate.Format = Session["DateFullYearFormat"].ToString();
                    this.txtToDate.Format = Session["DateFullYearFormat"].ToString();

                }

                //if(Request.QueryString[""]
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

                tblAppPrivilage appPrivilage = grpBl.GetPrivilageStatus(GroupID, PID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (appPrivilage != null)
                {
                    CanAdd = appPrivilage.CanAdd;
                    CanApprove = appPrivilage.CanEdit;
                    if (appPrivilage.CanAdd.Equals(false))
                    {
                        btnNew.Visible = false;
                    }
                    if (appPrivilage.CanEdit.Equals(false))//Can Approve Status
                    {
                        grdVoucher.Columns[5].Visible = false;
                    }
                }
                else
                {
                    grdVoucher.Enabled = false;
                    btnNew.Enabled = false;
                    btnSearch.Enabled = false;
                    ucMessage.ShowMessage("Enter privilages for the logged in group", RMS.BL.Enums.MessageType.Error);
                }



                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                //=================================================




                if (PID == 321)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "JV").ToString();
                    VoucherTypeID = 1;
                    BindGrid(VoucherTypeID);
                }
                else if (PID == 322)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "CP").ToString();

                    VoucherTypeID = 2;
                    BindGrid(VoucherTypeID);
                }
                else if (PID == 323)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "CR").ToString();
                    VoucherTypeID = 4;
                    BindGrid(VoucherTypeID);
                }
                else if (PID == 324)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "BP").ToString();
                    VoucherTypeID = 3;
                    BindGrid(VoucherTypeID);
                }
                else if (PID == 325)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "BR").ToString();
                    VoucherTypeID = 5;
                    BindGrid(VoucherTypeID);
                }
                else if (PID == 328)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "OB").ToString();
                    VoucherTypeID = 55;
                    BindGrid(VoucherTypeID);
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["FromDate"] != null && Session["ToDate"] != null && Session["Status"] != null)
            {
                txtFrom.Text = Session["FromDate"].ToString();
                txtFromDate.SelectedDate = Convert.ToDateTime(Session["FromDate"]);
                txtToDate.SelectedDate = Convert.ToDateTime(Session["ToDate"]);
                txtTo.Text = Session["ToDate"].ToString();
                ddlstatus.SelectedValue = Session["Status"].ToString();

                btnSearch_Click(null, null);

                Session["FromDate"] = null;
                Session["ToDate"] = null;
                Session["Status"] = null;
            }

            //if (Session["DisplayMsg"] != null)
            //{
            //    if (Convert.ToBoolean(Session["DisplayMsg"]).Equals(true))
            //    {
            //        string msg = Convert.ToString(Session["Msg"]);

            //        if (Convert.ToString(Session["MsgType"]).Equals("Info"))
            //        {
            //            ucMessage.ShowMessage(msg, RMS.BL.Enums.MessageType.Info);
            //        }
            //        else
            //        {
            //            ucMessage.ShowMessage(msg, RMS.BL.Enums.MessageType.Error);
            //        }
            //    }
            //}
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
                    BindGrid(VoucherTypeID);
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }
        protected void grdVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdVoucher.PageIndex = e.NewPageIndex;
            BindGrid(VoucherTypeID);
        
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IsSearch = true;
            BindGrid(VoucherTypeID);
        }

        protected void grdVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string Status = null;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbostatus = e.Row.FindControl("chkstatus") as CheckBox;
                //HyperLink hypEdit = e.Row.FindControl("lnkEdit") as HyperLink;
                //HyperLink hypDelete = e.Row.FindControl("lnkDelete") as HyperLink;
                //HyperLink hypPrint = e.Row.FindControl("lnkPrint") as HyperLink;

                LinkButton hypEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                LinkButton hypDelete = e.Row.FindControl("lnkDelete") as LinkButton;
                LinkButton hypPrint = e.Row.FindControl("lnkPrint") as LinkButton;
                LinkButton hypView = e.Row.FindControl("lnkView") as LinkButton;

                Status = (string)DataBinder.Eval(e.Row.DataItem, "status");

                if (Status == "Pending")
                {
                    cbostatus.Enabled = true;
                    hypDelete.Text = "Cancel";

                    if(CanAdd.Equals(true))
                    {
                        hypEdit.Visible = true;
                    }
                    else
                    {
                        hypEdit.Visible = false;
                    }
                    if (CanApprove.Equals(true))
                    {
                        hypView.Visible = true;
                    }
                    else
                    {
                        hypView.Visible = false;
                    }
                    
                    
                }
                else if (Status == "Approved")
                {
                    hypView.Visible = true;
                    hypEdit.Visible = false;
                    cbostatus.Checked = true;
                    cbostatus.Enabled = false;
                    hypDelete.Visible = false;
                }
                else
                {
                    cbostatus.Checked = false;
                    cbostatus.Enabled = false;
                    hypView.Visible = true;
                    hypEdit.Visible = false;
                    hypDelete.Visible = false;
                }


                if(!vouchHomeBL.GetRemarksStatus((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], Convert.ToInt32(e.Row.Cells[0].Text)))
                {
                    Image img = e.Row.FindControl("imgRemarks") as Image;
                    img.Visible = false;
                }
                
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblID = (Label)clickedRow.FindControl("lbl");
            VoucherId = Convert.ToInt32(lblID.Text);

            Label lblstatus = (Label)clickedRow.FindControl("lblstatus");
            VoucherStatus = lblstatus.Text;
            CreatePDF(Session["PageTitle"].ToString(), "pdf", VoucherId);
            

        }

        protected void lnkView_Click(object sender, EventArgs e)
        {
            
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblID = (Label)clickedRow.FindControl("lbl");
            Label srno = (Label)clickedRow.FindControl("lblsr_no");
            Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            VoucherId = Convert.ToInt32(lblID.Text);
            Session["VoucherId"] = VoucherId;
            //Session["PageID"] = Convert.ToInt32(Request.QueryString["PID"]);
            Session["PrevPgID"] = Convert.ToInt32(Request.QueryString["PID"]);
            if (status.Text == "Pending")
                Session["Flag"] = false;
            else
                Session["Flag"] = true;
            Session["VouchNo"] = srno.Text;
            Session["DateVal"] = vr_dt.Text;
            Session["Status"] = status.Text;
            Session["VoucherTypeId"] = VoucherTypeID.ToString();


            Session["FromDate"] = txtFrom.Text;
            Session["ToDate"] = txtTo.Text;
            //Session["Status"] = ddlstatus.SelectedItem.Text;

            Response.Redirect("~/GLSetup/frmViewVoucher.aspx?PID=326");
            
        
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {

            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblID = (Label)clickedRow.FindControl("lbl");
            Label srno = (Label)clickedRow.FindControl("lblsr_no");
            Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            VoucherId = Convert.ToInt32(lblID.Text);
            Session["VoucherId"] = VoucherId;
            Session["PageID"] = Convert.ToInt32(Request.QueryString["PID"]);
            Session["VouchNo"] = srno.Text;
            Session["DateVal"] = vr_dt.Text;
            Session["Status"] = status.Text;
            bool res = vouchHomeBL.CancelVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], VoucherId);
            if (res == true)
            {
                string vrDescptn = vouchDetBL.VoucherDesc(VoucherTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("voucher " + vrDescptn + '-' + vrno.Text + " has been cancelled successfully", RMS.BL.Enums.MessageType.Info);
                System.Threading.Thread.Sleep(5000);
               // Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + PID);
                BindGrid(VoucherTypeID);
            }
            else
            {
                string vrDescptn = vouchDetBL.VoucherDesc(VoucherTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("voucher " + vrDescptn + '-' + vrno.Text + " has not been cancelled", RMS.BL.Enums.MessageType.Error);
                System.Threading.Thread.Sleep(5000);
               // Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + PID);
            }
  
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblID = (Label)clickedRow.FindControl("lbl");
            Label srno = (Label)clickedRow.FindControl("lblsr_no");
            Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            VoucherId = Convert.ToInt32(lblID.Text);
            Session["VoucherId"] = VoucherId;
            Session["VoucherTypeId"] = VoucherTypeID.ToString();
            Session["PageID"] = Convert.ToInt32(Request.QueryString["PID"]);
            Session["PrevPgID"] = Convert.ToInt32(Request.QueryString["PID"]);
            Session["VouchNo"] = srno.Text;
            Session["DateVal"] = vr_dt.Text;
            Session["Status"] = status.Text;
            Session["aFlag"] = false;
            Response.Redirect("~/GLSetup/frmVoucherDetail.aspx?PID=327");
         
        
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Session["VoucherTypeId"] = VoucherTypeID.ToString();
            //Session["PID"] = Convert.ToInt32(Request.QueryString["PID"]);
            Session["PrevPgID"] = Convert.ToInt32(Request.QueryString["PID"]);
            Session["aFlag"] = true;

            Session["FromDate"] = txtFrom.Text;
            Session["ToDate"] = txtTo.Text;
            Session["Status"] = ddlstatus.SelectedValue;

            Response.Redirect("~/GLSetup/frmVoucherDetail.aspx?PID=327");
            
            //int pageid = Convert.ToInt32(Session["PageID"]);
            //Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + pageid);
            


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
                        vouchHomeBL.Save((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid, flag, username);
                        vouchDetBL.PostVoucher4mHome((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid, username);
                    }
                }
            }
            BindGrid(VoucherTypeID);
        }

        #endregion

        #region helping events

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
            grdVoucher.DataSource = vouchHomeBL.GetDataForGrid(vt_cd,dtFrm,dt2,Convert.ToChar(ddlstatus.SelectedItem.Value),BranchID,IsSearch,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdVoucher.DataBind();
            
        }

        protected void CreatePDF(String FileName, String extension,int vrid)
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
            if (PID == 324 || PID == 325)
            {
                IList<spBPnBRResult> sal = vouchHomeBL.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR.rdlc";
                datasource = new ReportDataSource("spBPnBRResult", sal);
                updateby = sal.First().updateby;
                approvedby = sal.First().approvedby;
            }

            else
            {
                IList<spTestGLResult> sal = vouchHomeBL.GetReport((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptJV.rdlc";
                datasource = new ReportDataSource("spTestGLResult", sal);
                updateby = sal.First().updateby;
                approvedby = sal.First().approvedby;
            }
            
            
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            try
            {
                Branch branch = vouchHomeBL.GetBranch(1, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
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

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            
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

        #endregion

    }
}

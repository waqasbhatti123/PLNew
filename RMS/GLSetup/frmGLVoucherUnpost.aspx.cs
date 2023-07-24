using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Script.Services;
using System.Web.Services;
using Microsoft.Reporting.WebForms;

namespace RMS.GLSetup
{
    public partial class frmGLVoucherUnpost : BasePage
    {
        #region DataMembers

        voucherHomeBL vouchHomeBL = new voucherHomeBL();
        voucherDetailBL vouchDetBL = new voucherDetailBL();
        GroupBL grpBl = new GroupBL();

        #endregion

        #region Properties

        public string VoucherStatus
        {
            get { return ViewState["VoucherStatus"].ToString(); }
            set { ViewState["VoucherStatus"] = value; }
        }
 
#pragma warning disable CS0114 // 'frmGLVoucherUnpost.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'frmGLVoucherUnpost.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
 
        public int GroupID
        {
            get { return (ViewState["GroupID"] == null) ? 0 : Convert.ToInt32(ViewState["GroupID"]); }
            set { ViewState["GroupID"] = value; }
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

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            FillVoucherTypeDropDown();
            
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

            if (Session["DateFormat"] == null)
            {
                CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
            }
            else
            {
                CalendarExtender1.Format = Session["DateFormat"].ToString();
            }
            if (Session["DateFormat"] == null)
            {
                CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
            }
            else
            {
                CalendarExtender2.Format = Session["DateFormat"].ToString();
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GLVoucherUnpost").ToString();

                if (Session["UserID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    else
                    {
                        ID = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                    }
                }
                else
                {
                    ID = Convert.ToInt32(Session["UserID"].ToString());
                }
                int PID = Convert.ToInt32(Request.QueryString["PID"]);

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
                        //Do some action
                    }
                    if (appPrivilage.CanEdit.Equals(false))//Can Approve Status
                    {
                        //Do some action
                    }
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }

                //=================================================

                
                CalendarExtender1.SelectedDate = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01");
                CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                txtFromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01").ToString();
                txtToDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();

                BindGrid(Convert.ToInt16(ddlVoucherType.SelectedValue));
            }
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session["FromDate"] != null && Session["ToDate"] != null && Session["Status"] != null)
            {
                txtFromDt.Text = Session["FromDate"].ToString();
                CalendarExtender1.SelectedDate = Convert.ToDateTime(Session["FromDate"]);
                CalendarExtender2.SelectedDate = Convert.ToDateTime(Session["ToDate"]);
                txtToDt.Text = Session["ToDate"].ToString();
                ddlStatus.SelectedValue = Session["Status"].ToString();
                string v = Session["VoucherTypeId"].ToString();
                ddlVoucherType.SelectedValue = Session["VoucherTypeId"].ToString();

                btnShow_Click(null, null);

                if (Session["UnpostMsg"] != null)
                {
                    if (Session["UnpostMsg"].ToString() == "ok")
                    {
                        ucMessage.ShowMessage("Voucher unpost is successful", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        ucMessage.ShowMessage(Session["UnpostMsg"].ToString(), RMS.BL.Enums.MessageType.Error);
                    }
                }

                Session["FromDate"] = null;
                Session["ToDate"] = null;
                Session["Status"] = null;
                Session["UnpostMsg"] = null;
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
                    BindGrid(Convert.ToInt16(ddlVoucherType.SelectedValue));
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }
        protected void grdVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GetColumns(sender, e);
        }
        protected void grdVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdVoucher.PageIndex = e.NewPageIndex;
            BindGrid(Convert.ToInt16(ddlVoucherType.SelectedValue));
        }
        protected void lnkUnpost_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblVoucherTypeID = (Label)clickedRow.FindControl("lblVoucherTypeID");
            Label lblID = (Label)clickedRow.FindControl("lbl");
            int VoucherId = Convert.ToInt32(lblID.Text);

             Glmf_Data glData = vouchHomeBL.GetByID(VoucherId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
             VoucherStatus = glData.vr_apr.ToString();

             string vrDescptn = vouchDetBL.VoucherDesc(glData.vt_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (VoucherStatus == "P")
            {
                VoucherStatus = "Pending";
            }
            else if (VoucherStatus == "D")
            {
                VoucherStatus = "Canceled";
            }
            else if (VoucherStatus == "A")
            {
                VoucherStatus = "Approved";
            }

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
                Byte[] bytes = CreatePdfByteArray(Session["PageTitle"].ToString(), "pdf", VoucherId, glData.vt_cd);



                string msg = vouchHomeBL.Unpost((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], VoucherId, username, userid, bytes);

                if (msg == "ok")
                {
                    BindGrid(Convert.ToInt16(ddlVoucherType.SelectedValue));
                    ucMessage.ShowMessage("voucher " + vrDescptn + "- Sr #: " + glData.vr_no + "- Voucher #: " + glData.Ref_no + " has been unposted successfully", RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage("Exception: " + msg, RMS.BL.Enums.MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }

        
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblVoucherTypeID = (Label)clickedRow.FindControl("lblVoucherTypeID");
            Label lblID = (Label)clickedRow.FindControl("lbl");
            int VoucherId = Convert.ToInt32(lblID.Text);

            Label lblstatus = (Label)clickedRow.FindControl("lblstatus");
            string VoucherStatus = lblstatus.Text;
            string filename = Session["PageTitle"].ToString();
            if (Convert.ToInt32(lblVoucherTypeID.Text).Equals(1))
            {
                filename = "Journal Voucher";
            }
            else if (Convert.ToInt32(lblVoucherTypeID.Text).Equals(2))
            {
                filename = "Cash Payment Voucher";
            }
            else if (Convert.ToInt32(lblVoucherTypeID.Text).Equals(3))
            {
                filename = "Bank Payment Voucher";
            }
            else if (Convert.ToInt32(lblVoucherTypeID.Text).Equals(4))
            {
                filename = "Cash Receipt Voucher";
            }
            else if (Convert.ToInt32(lblVoucherTypeID.Text).Equals(5))
            {
                filename = "Bank Receipt Voucher";
            }
            else if (Convert.ToInt32(lblVoucherTypeID.Text).Equals(55))
            {
                filename = "Opening Balance";
            }
            else if (Convert.ToInt32(lblVoucherTypeID.Text).Equals(6))
            {
                filename = "Interface Payment Voucher";
            }
            
            CreatePDF(filename, "pdf", VoucherId, Convert.ToInt32(lblVoucherTypeID.Text), VoucherStatus);


        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            IsSearch = true;
            BindGrid(Convert.ToInt16(ddlVoucherType.SelectedValue));
        }

        #endregion

        #region Helping Method

        protected Byte[] CreatePdfByteArray(String FileName, String extension, int vrid, int VoucherTypeID)
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


            string rptLogoPath = "";
            string updateby = "";
            string approvedby = "";

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            if (VoucherTypeID == 3 || VoucherTypeID == 5 || VoucherTypeID == 6)
            {
                IList<spBPnBRResult> sal = vouchHomeBL.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR.rdlc";
                datasource = new ReportDataSource("spBPnBRResult", sal);

                try
                {
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                catch
                {
                    updateby = "";
                    approvedby = "";
                }
            }

            else
            {
                IList<spTestGLResult> sal = vouchHomeBL.GetReport((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptJV.rdlc";
                datasource = new ReportDataSource("spTestGLResult", sal);


            }
           

            string brName = "";
            string brAddress = "";
            string brTel = "";
            string brFax = "";
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
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
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
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.SetParameters(paramz);
            ReportViewer1 = viewer;

            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            return bytes;
        }
        protected void GetColumns(object sender, GridViewRowEventArgs e)
        {
            string Status = null;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton hypPrint = e.Row.FindControl("lnkPrint") as LinkButton;
                LinkButton hypView = e.Row.FindControl("lnkView") as LinkButton;

                Status = (string)DataBinder.Eval(e.Row.DataItem, "status");

                if (Status == "Pending")
                {
                    hypView.Visible = false;
                }
                else if (Status == "Approved")
                {
                    hypView.Visible = true;
                }
                else
                {
                    hypView.Visible = true;
                }

                if (!vouchHomeBL.GetRemarksStatus((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], Convert.ToInt32(e.Row.Cells[0].Text)))
                {
                    Image img = e.Row.FindControl("imgRemarks") as Image;
                    img.Visible = false;
                }
                
            }
        }

        protected void FillVoucherTypeDropDown()
        {
            ddlVoucherType.Controls.Clear();
            RMSDataContext Data = new RMSDataContext();
            ddlVoucherType.DataValueField = "TemplateID";
            ddlVoucherType.DataTextField = "Name";
            ddlVoucherType.DataSource = Data.Templates.Where(x => x.IsActive == true).ToList();
            ddlVoucherType.DataBind();
        }


        protected void BindGrid(int vt_cd)
        {
            DateTime dtFrm, dt2;
            string txt = "";
            try
            {
                txt = "fromdate";
                dtFrm = Convert.ToDateTime(txtFromDt.Text);
                txt = "todate";
                dt2= Convert.ToDateTime(txtToDt.Text);
            }
            catch
            {
                if (txt.Equals("fromdate"))
                {
                    ucMessage.ShowMessage("Invalid from date", RMS.BL.Enums.MessageType.Error);
                    txtFromDt.Focus();
                }
                else
                {
                    ucMessage.ShowMessage("Invalid to date", RMS.BL.Enums.MessageType.Error);
                    txtToDt.Focus();
                }
                return;
            }

            grdVoucher.DataSource = vouchHomeBL.GetDataForGrid(vt_cd, dtFrm, dt2, Convert.ToChar(ddlStatus.SelectedValue),BranchID, IsSearch, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdVoucher.DataBind();
        }
        protected void CreatePDF(String FileName, String extension, int vrid, int vrTypID, string VoucherStatus)
        {
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();
            ReportDataSource datasource = null;

            string brName = "";
            string brAddress = "";
            string brTel = "";
            string brFax = "";
            string rptLogoPath = "";
            string updateby = "";
            string approvedby = "";

            if (vrTypID == 3 || vrTypID == 5 || vrTypID == 6)
            {
                IList<spBPnBRResult> sal = vouchHomeBL.GetReport2((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                ReportViewer1.LocalReport.ReportPath = "GLSetup/rdlc/rptBPnBR.rdlc";
                datasource = new ReportDataSource("spBPnBRResult", sal);
                try
                {
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                catch
                {
                    updateby = "";
                    approvedby = "";
                }
            }

            else
            {
                IList<spTestGLResult> sal = vouchHomeBL.GetReport((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], vrid);
                ReportViewer1.LocalReport.ReportPath = "GLSetup/rdlc/rptJV.rdlc";
                datasource = new ReportDataSource("spTestGLResult", sal);

                try
                {
                    updateby = sal.First().updateby;
                    approvedby = sal.First().approvedby;
                }
                catch
                {
                    updateby = "";
                    approvedby = "";
                }
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

            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);

            ReportViewer1.LocalReport.SetParameters(paramz);
         }

        #endregion
    }
}

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
    public partial class frmGLVoucher : BasePage
    {
        #region DataMembers

        voucherHomeBL vouchHomeBL = new voucherHomeBL();
        voucherDetailBL vouchDetBL = new voucherDetailBL();
        GroupBL grpBl = new GroupBL();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'frmGLVoucher.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'frmGLVoucher.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GLVoucher").ToString();

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
                BindVoucherDropDown();
                BindGridJV(Convert.ToInt32(ddlVoucher.SelectedValue));
                //BindGridCPV(2);
                //BindGridBPV(3);
                //BindGridCR(4);
                //BindGridBR(5);
                //BindGridOB(55);
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

                btnShow_Click(null, null);

                Session["FromDate"] = null;
                Session["ToDate"] = null;
                Session["Status"] = null;
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
                    BindGridJV(Convert.ToInt32(ddlVoucher.SelectedValue));
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
            BindGridJV(Convert.ToInt32(ddlVoucher.SelectedValue));
        }

        //protected void grdVoucherCPV_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    GetColumns(sender, e);
        //}

        //protected void grdVoucherCPV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdVoucherCPV.PageIndex = e.NewPageIndex;
        //    BindGridCPV(2);
        //}

        //protected void grdVoucherBPV_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    GetColumns(sender, e);
        //}

        //protected void grdVoucherBPV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdVoucherBPV.PageIndex = e.NewPageIndex;
        //    BindGridBPV(3);
        //}

        //protected void grdVoucherCR_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    GetColumns(sender, e);
        //}

        //protected void grdVoucherCR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdVoucherCR.PageIndex = e.NewPageIndex;
        //    BindGridCR(4);
        //}

        //protected void grdVoucherBR_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    GetColumns(sender, e);
        //}

        //protected void grdVoucherBR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdVoucherBR.PageIndex = e.NewPageIndex;
        //    BindGridBR(5);
        //}

        //protected void grdOB_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    GetColumns(sender, e);
        //}

        //protected void grdOB_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdOB.PageIndex = e.NewPageIndex;
        //    BindGridOB(55);
        //}

        //protected void grdIV_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    GetColumns(sender, e);
        //}

        //protected void grdIV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdIV.PageIndex = e.NewPageIndex;
        //    BindGridIV(6);
        //}

        
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            //Label lblVoucherTypeID = (Label)clickedRow.FindControl("lblVoucherTypeID");
            Label lblID = (Label)clickedRow.FindControl("lbl");
            //Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            //Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            //Label status = (Label)clickedRow.FindControl("lblstatus");
            int VoucherId = Convert.ToInt32(lblID.Text);
            Glmf_Data obj = vouchHomeBL.GetVoucherMasterDetail(VoucherId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            int voucherTypeId = obj.vt_cd;
            Session["VoucherId"] = VoucherId;

            DateTime dtFrmGLVoucher = vouchHomeBL.GetByID(VoucherId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).vr_dt;
            Session["dtFrmGLVoucher"] = dtFrmGLVoucher;
            //Session["PageID"] = Convert.ToInt32(Request.QueryString["PID"]);
            Session["PrevPgID"] = Convert.ToInt32(Request.QueryString["PID"]);
            //if (status.Text == "Pending")
            //    Session["Flag"] = false;
            //else
            //    Session["Flag"] = true;
            //Session["VouchNo"] = vrno.Text;
            //Session["DateVal"] = vr_dt.Text;
            //Session["Status"] = status.Text;
            Session["VoucherTypeId"] = obj.vt_cd;

            Session["FromDate"] = txtFromDt.Text;
            Session["ToDate"] = txtToDt.Text;
            Session["Status"] = ddlStatus.SelectedValue;


            string title = "";
            if (voucherTypeId.Equals(1))
            {
                title = "Journal Voucher";
            }
            else if (voucherTypeId.Equals(2))
            {
                title = "Cash Payment Voucher";
            }
            else if (voucherTypeId.Equals(3))
            {
                title = "Bank Payment Voucher";
            }
            else if (voucherTypeId.Equals(4))
            {
                title = "Cash Receipt Voucher";
            }
            else if (voucherTypeId.Equals(5))
            {
                title = "Bank Receipt Voucher";
            }
            else if (voucherTypeId.Equals(55))
            {
                title = "Opening Balance";
            }
            else if (voucherTypeId.Equals(6))
            {
                title = "Interface Payment Voucher";
            }

            Session["PageTitle"] = title;
            /////////////////////////////////////////////////////////////


            //GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            //Label lblID = (Label)clickedRow.FindControl("lbl");
            //Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            //Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            //Label status = (Label)clickedRow.FindControl("lblstatus");
            //VoucherId = Convert.ToInt32(lblID.Text);
            //Session["VoucherId"] = VoucherId;
            //Session["VoucherTypeId"] = VoucherTypeID.ToString();
            //Session["PageID"] = Convert.ToInt32(Request.QueryString["PID"]);
            //Session["PrevPgID"] = Convert.ToInt32(Request.QueryString["PID"]);
            //Session["VouchNo"] = vrno.Text;
            //Session["DateVal"] = vr_dt.Text;
            //Session["Status"] = status.Text;
            Session["aFlag"] = false;
            if (voucherTypeId == 1)
                Response.Redirect("~/GLSetup/Vouchers.aspx?PID=321");
            else if
                (voucherTypeId == 2)
                Response.Redirect("~/GLSetup/Vouchers.aspx?PID=322");
            else if
                (voucherTypeId == 3)
                Response.Redirect("~/GLSetup/Vouchers.aspx?PID=324");
            else if
                (voucherTypeId == 4)
                Response.Redirect("~/GLSetup/Vouchers.aspx?PID=323");
            else if
                (voucherTypeId == 5)
                Response.Redirect("~/GLSetup/Vouchers.aspx?PID=325");
            else if
                (voucherTypeId == 55)
                Response.Redirect("~/GLSetup/Vouchers.aspx?PID=328");
            else if
                (voucherTypeId == 6)
                Response.Redirect("~/GLSetup/Vouchers.aspx?PID=399");
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            //Label lblVoucherTypeID = (Label)clickedRow.FindControl("lblVoucherTypeID");
            Label lblID = (Label)clickedRow.FindControl("lbl");
            Label srno = (Label)clickedRow.FindControl("lblsr_no");
            Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            int VoucherId = Convert.ToInt32(lblID.Text);
            int voucherTypeId=GetVoucherTypeID(VoucherId);
            Session["VoucherId"] = VoucherId;
            Session["PageID"] = Convert.ToInt32(Request.QueryString["PID"]);
            Session["VouchNo"] = srno.Text;
            Session["DateVal"] = vr_dt.Text;
            Session["Status"] = status.Text;
            bool res = vouchHomeBL.CancelVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], VoucherId);
            if (res == true)
            {
                string vrDescptn = vouchDetBL.VoucherDesc(voucherTypeId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("voucher " + vrDescptn + "- Sr# :" + srno.Text + "- Voucher# : " + vrno.Text + " has been cancelled successfully", RMS.BL.Enums.MessageType.Info);
                //System.Threading.Thread.Sleep(5000);
                // Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + PID);
            }
            else
            {
                string vrDescptn = vouchDetBL.VoucherDesc(voucherTypeId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("voucher " + vrDescptn + "- Sr# :" + srno.Text + "- Voucher# : "+ vrno.Text + "has not been cancelled", RMS.BL.Enums.MessageType.Error);
                //System.Threading.Thread.Sleep(5000);
                // Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + PID);
            }

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            //Label lblVoucherTypeID = (Label)clickedRow.FindControl("lblVoucherTypeID");
            Label lblID = (Label)clickedRow.FindControl("lbl");
            int VoucherId = Convert.ToInt32(lblID.Text);
            int voucherTypeId = GetVoucherTypeID(VoucherId);
            Label lblstatus = (Label)clickedRow.FindControl("lblstatus");
            string VoucherStatus = lblstatus.Text;
            string filename = Session["PageTitle"].ToString();
            if (voucherTypeId.Equals(1))
            {
                filename = "Journal Voucher";
            }
            else if (voucherTypeId.Equals(2))
            {
                filename = "Cash Payment Voucher";
            }
            else if (voucherTypeId.Equals(3))
            {
                filename = "Bank Payment Voucher";
            }
            else if (voucherTypeId.Equals(4))
            {
                filename = "Cash Receipt Voucher";
            }
            else if (voucherTypeId.Equals(5))
            {
                filename = "Bank Receipt Voucher";
            }
            else if (voucherTypeId.Equals(55))
            {
                filename = "Opening Balance";
            }
            else if (voucherTypeId.Equals(6))
            {
                filename = "Interface Payment Voucher";
            }
            
            CreatePDF(filename, "pdf", VoucherId, voucherTypeId, VoucherStatus);


        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            IsSearch = true;
            BindGridJV(Convert.ToInt32(ddlVoucher.SelectedValue));
            //BindGridCPV(2);
            //BindGridBPV(3);
            //BindGridCR(4);
            //BindGridBR(5);
            //BindGridOB(55);
            //BindGridIV(6);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            CalendarExtender1.SelectedDate = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01");
            CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            txtFromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01").ToString();
            txtToDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

            ddlStatus.SelectedValue = "P";
            viewer.Reset();

            BindGridJV(Convert.ToInt32(ddlVoucher.SelectedValue));
            //BindGridCPV(2);
            //BindGridBPV(3);
            //BindGridCR(4);
            //BindGridBR(5);
            //BindGridOB(55);
            //BindGridIV(6);
        }

        #endregion

        #region Helping Method

        protected void GetColumns(object sender, GridViewRowEventArgs e)
        {
            string Status = null;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //CheckBox cbostatus = e.Row.FindControl("chkstatus") as CheckBox;
                //HyperLink hypEdit = e.Row.FindControl("lnkEdit") as HyperLink;
                //HyperLink hypDelete = e.Row.FindControl("lnkDelete") as HyperLink;
                //HyperLink hypPrint = e.Row.FindControl("lnkPrint") as HyperLink;

                LinkButton hypEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                LinkButton hypDelete = e.Row.FindControl("lnkDelete") as LinkButton;
                LinkButton hypPrint = e.Row.FindControl("lnkPrint") as LinkButton;
                //LinkButton hypView = e.Row.FindControl("lnkView") as LinkButton;

                Status = (string)DataBinder.Eval(e.Row.DataItem, "status");

                if (Status == "Pending")
                {
                    //cbostatus.Enabled = true;
                    hypDelete.Text = "Cancel";

                    if(CanAdd.Equals(true))
                    {
                        hypEdit.Visible = true;
                    }
                    else
                    {
                        hypEdit.Visible = false;
                    }
                }
                else if (Status == "Approved")
                {
                    //hypView.Visible = true;
                    hypEdit.Visible = false;
                    //cbostatus.Checked = true;
                    //cbostatus.Enabled = false;
                    hypDelete.Visible = false;
                }
                else
                {
                    //cbostatus.Checked = false;
                    //cbostatus.Enabled = false;
                    //hypView.Visible = true;
                    hypEdit.Visible = false;
                    hypDelete.Visible = false;
                }


                if (!vouchHomeBL.GetRemarksStatus((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], Convert.ToInt32(e.Row.Cells[0].Text)))
                {
                    Image img = e.Row.FindControl("imgRemarks") as Image;
                    img.Visible = false;
                }
                
            }
        }

        protected void BindVoucherDropDown()
        {
            ListItem list = null;

            list = new ListItem();
            list.Value = "1";
            list.Text = "Journal Voucher";
            ddlVoucher.Items.Add(list);
            list = new ListItem();
            list.Value = "2";
            list.Text = "Cash Payment Voucher";
            ddlVoucher.Items.Add(list);
            list = new ListItem();
            list.Value = "3";
            list.Text = "Bank Payment Voucher";
            ddlVoucher.Items.Add(list);
            list = new ListItem();
            list.Value = "4";
            list.Text = "Cash Receipt Voucher";
            ddlVoucher.Items.Add(list);
            list = new ListItem();
            list.Value = "5";
            list.Text = "Bank Receipt Voucher";
            ddlVoucher.Items.Add(list);
            list = new ListItem();
            list.Value = "55";
            list.Text = "Opening Balance";
            ddlVoucher.Items.Add(list);
            list = new ListItem();
            list.Value = "6";
            list.Text = "Interface Payment";
            ddlVoucher.Items.Add(list);
        }
        protected int GetVoucherTypeID(int vrid)
        {
            return vouchHomeBL.GetVoucherTypeID(vrid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
           
        }
        //protected Glmf_Data GetVoucherMasterDetail(int vrid)
        //{
        //    return vouchHomeBL.GetVoucherMasterDetail(vrid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //}
        protected void BindGridJV(int vt_cd)
        {
            DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(txtFromDt.Text, out dtFrm);
            DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DateTime.TryParse(txtToDt.Text, out dt2);
            dt2 = dt2.AddDays(1);
            grdVoucher.DataSource = vouchHomeBL.GetDataForGrid(vt_cd, dtFrm, dt2, Convert.ToChar(ddlStatus.SelectedValue),BranchID, IsSearch,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdVoucher.DataBind();
        }

        //protected void BindGridCPV(int vt_cd)
        //{
        //    DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtFromDt.Text, out dtFrm);
        //    DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtToDt.Text, out dt2);
        //    grdVoucherCPV.DataSource = vouchHomeBL.GetDataForGrid(vt_cd, dtFrm, dt2, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    grdVoucherCPV.DataBind();
        //}

        //protected void BindGridBPV(int vt_cd)
        //{
        //    DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtFromDt.Text, out dtFrm);
        //    DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtToDt.Text, out dt2);
        //    grdVoucherBPV.DataSource = vouchHomeBL.GetDataForGrid(vt_cd, dtFrm, dt2, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    grdVoucherBPV.DataBind();
        //}

        //protected void BindGridCR(int vt_cd)
        //{
        //    DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtFromDt.Text, out dtFrm);
        //    DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtToDt.Text, out dt2);
        //    grdVoucherCR.DataSource = vouchHomeBL.GetDataForGrid(vt_cd, dtFrm, dt2, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    grdVoucherCR.DataBind();
        //}

        //protected void BindGridBR(int vt_cd)
        //{
        //    DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtFromDt.Text, out dtFrm);
        //    DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtToDt.Text, out dt2);
        //    grdVoucherBR.DataSource = vouchHomeBL.GetDataForGrid(vt_cd, dtFrm, dt2, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    grdVoucherBR.DataBind();
        //}

        //protected void BindGridOB(int vt_cd)
        //{
        //    DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtFromDt.Text, out dtFrm);
        //    DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtToDt.Text, out dt2);
        //    grdOB.DataSource = vouchHomeBL.GetDataForGrid(vt_cd, dtFrm, dt2, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    grdOB.DataBind();
        //}

        //protected void BindGridIV(int vt_cd)
        //{
        //    DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtFromDt.Text, out dtFrm);
        //    DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    DateTime.TryParse(txtToDt.Text, out dt2);
        //    grdIV.DataSource = vouchHomeBL.GetDataForGrid(vt_cd, dtFrm, dt2, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    grdIV.DataBind();
        //}

        protected void CreatePDF(String FileName, String extension, int vrid, int vrTypID, string VoucherStatus)
        {
            viewer.Reset();
            viewer.LocalReport.Refresh();
            ReportDataSource datasource = null;
            string rptLogoPath = "";
            string updateby = "";
            string approvedby = "";
            if (vrTypID == 3 || vrTypID == 5 || vrTypID == 6)
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

            string brName = "";
            string brAddress = "";
            string brTel = "";
            string brFax = "";
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
            
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
            //viewer.LocalReport.EnableExternalImages = true;
            //viewer.LocalReport.Refresh();
            //viewer.LocalReport.SetParameters(paramz);
         }

        #endregion
    }
}

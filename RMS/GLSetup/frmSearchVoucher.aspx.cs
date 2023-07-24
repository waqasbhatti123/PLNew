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
    public partial class frmSearchVoucher : System.Web.UI.Page
    {
        voucherHomeBL hmBl = new voucherHomeBL();

        public int BrID
        {
            get { return Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SearchVoucher").ToString();

                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        BrID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    BrID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BrID.ToString();
                BindVoucherDropDown();
                BindGrid();
            }
        }


        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BrID).FirstOrDefault();

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
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BrID).ToList();
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
                    BrID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    BindGrid();
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }


        protected void grdVoucher_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = Convert.ToDateTime(e.Row.Cells[0].Text).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void grdVoucher_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdVoucher.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void BindGrid()
        {
            decimal debit = 0, credit = 0;
            try
            {
                if (!string.IsNullOrEmpty(txtDebit.Text))
                {
                    debit = Convert.ToDecimal(txtDebit.Text);
                }
            }
            catch
            {
                txtDebit.Focus();
                ucMessage.ShowMessage("Please enter valid debit amount.", RMS.BL.Enums.MessageType.Error);
                return; 
            }
            try
            {
                if (!string.IsNullOrEmpty(txtCredit.Text))
                {
                    credit = Convert.ToDecimal(txtCredit.Text);
                }
            }
            catch
            {
                txtCredit.Focus();
                ucMessage.ShowMessage("Please enter valid credit amount.", RMS.BL.Enums.MessageType.Error);
                return;
            }

            if(string.IsNullOrEmpty(txtCCntr.Text))
            {
                Ac.Value = "";
            }

            try
            {
                grdVoucher.DataSource = hmBl.GetSearchedVouchers(BrID, Convert.ToChar(ddlStatus.SelectedValue),
                    txtNarr.Text, txtTitle.Text, txtChqNo.Text, debit, credit, Ac.Value, txtVoucherNo.Text, Convert.ToInt32(ddlVoucher.SelectedValue),
                    (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                grdVoucher.DataBind();
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }

            ddlVoucher.Focus();
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

        private void ClearAll()
        {
            ddlVoucher.SelectedValue = "0";
            txtVoucherNo.Text = "";
            txtNarr.Text = "";
            txtTitle.Text = "";
            txtChqNo.Text = "";
            txtCCntr.Text = "";
            txtDebit.Text = "";
            txtCredit.Text = "";
            ddlStatus.SelectedValue = "-";
            Ac.Value = "";
            
            BindGrid();
        }
    }
}

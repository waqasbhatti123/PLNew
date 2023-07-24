using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.Linq;

namespace RMS.Inv
{
    public partial class InvSettlement : BasePage
    {
        #region DataMembers

        InvSettlementBL invBL = new InvSettlementBL();

        #endregion

        #region Properties

        public int BrID
        {
            get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "InvSettlement").ToString();
                FillDdlVendor();
            }
        }
        protected void grdInvoices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text.StartsWith("JV"))
                    e.Row.Cells[0].Text = e.Row.Cells[0].Text;
                else
                    e.Row.Cells[0].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[0].Text.Substring(4);
                e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[3].Style.Add("text-align", "right");
                e.Row.Cells[3].Text = Math.Round(Convert.ToDecimal(e.Row.Cells[3].Text.Trim()), 2).ToString("N");
                e.Row.Cells[4].Style.Add("text-align", "right");
                e.Row.Cells[4].Text = Convert.ToDecimal(e.Row.Cells[4].Text.Trim()).ToString("N");
                e.Row.Cells[5].Style.Add("text-align", "right");
                e.Row.Cells[5].Text = Convert.ToDecimal(e.Row.Cells[5].Text.Trim()).ToString("N");
                e.Row.Cells[6].Style.Add("text-align", "right");
            }
        }
        protected void grdInvoices_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void grdInvoices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdInvoices.PageIndex = e.NewPageIndex;
            BindGridInvoices();
        }
        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridInvoices();
            BindGridPayments();
        }
        protected void grdPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[2].Style.Add("text-align", "right");
                e.Row.Cells[2].Text = Convert.ToDecimal(e.Row.Cells[2].Text.Trim()).ToString("N");
                e.Row.Cells[3].Style.Add("text-align", "right");
                e.Row.Cells[3].Text = Convert.ToDecimal(e.Row.Cells[3].Text.Trim()).ToString("N");
                e.Row.Cells[4].Style.Add("text-align", "right");
                e.Row.Cells[4].Text = Convert.ToDecimal(e.Row.Cells[4].Text.Trim()).ToString("N");
                e.Row.Cells[5].Style.Add("text-align", "right");
            }
        }
        protected void grdPayments_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void grdPayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPayments.PageIndex = e.NewPageIndex;
            BindGridPayments();
        }
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            bool isInvChecked = false, isPayChecked = false;
            for (int i = 0; i < grdInvoices.Rows.Count; i++)
            {
                if (((CheckBox)grdInvoices.Rows[i].FindControl("chkInvoice")).Checked)
                {
                    isInvChecked = true;
                    break;
                }
            }
            for (int i = 0; i < grdPayments.Rows.Count; i++)
            {
                if (((CheckBox)grdPayments.Rows[i].FindControl("chkPayment")).Checked)
                {
                    isPayChecked = true;
                    break;
                }
            }
            if (!isInvChecked)
            {
                ucMessage.ShowMessage("Please check atleast one invoice to continue", BL.Enums.MessageType.Error);
                return;
            }
            else if (!isPayChecked)
            {
                ucMessage.ShowMessage("Please check atleast one payment to continue", BL.Enums.MessageType.Error);
                return;
            }
            if (grdInvoices.Rows.Count > 0 && isInvChecked && grdPayments.Rows.Count > 0 && isPayChecked)
            {
                tblBill bill = null;
                EntitySet<tblBill> entBill = new EntitySet<tblBill>();
                tblSettlement set = null;
                EntitySet<tblSettlement> entSet = new EntitySet<tblSettlement>();
                for (int i = 0; i < grdInvoices.Rows.Count; i++)
                {
                    if (((CheckBox)grdInvoices.Rows[i].FindControl("chkInvoice")).Checked)
                    {
                        bill = new tblBill();
                        bill.vrid = Convert.ToInt32(grdInvoices.DataKeys[i].Value);
                        bill.brid = 1;
                        entBill.Add(bill);
                    }
                }
                for (int i = 0; i < grdPayments.Rows.Count; i++)
                {
                    if (((CheckBox)grdPayments.Rows[i].FindControl("chkPayment")).Checked)
                    {
                        set = new tblSettlement();
                        set.TransNo = Convert.ToInt32(grdPayments.DataKeys[i].Value);
                        entSet.Add(set);
                    }
                }

                string msg = invBL.SaveSettlements(entBill, entSet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg == "Done")
                {
                    BindGridInvoices();
                    BindGridPayments();
                    ucMessage.ShowMessage("Settlements updated successfully", BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage("Exception: " + msg, BL.Enums.MessageType.Error);
                }
            }
        }

        #endregion

        #region Helping Method

        private void FillDdlVendor()
        {
            ddlVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.DataTextField = "gl_dsc";
            ddlVendor.DataValueField = "gl_cd";
            ddlVendor.DataBind();
        }
        private void BindGridInvoices()
        {
            grdInvoices.DataSource = invBL.GetAllBillByVendorID(ddlVendor.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdInvoices.DataBind();
        }
        private void BindGridPayments()
        {
            grdPayments.DataSource = invBL.GetAllPaymentsByVendorID(ddlVendor.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdPayments.DataBind();
        }

        #endregion
    }
}

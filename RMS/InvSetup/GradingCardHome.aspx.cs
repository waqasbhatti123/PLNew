using System;
using RMS.BL;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Web.UI;
// 
namespace RMS
{
    public partial class GradingCardHome : BasePage
    {
        #region DataMembers

        InvGP_BL gP = new InvGP_BL();
        InvSG_BL sG = new InvSG_BL();
        InvGC_BL gC = new InvGC_BL();

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 424)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GradingCard").ToString();
                    btnCreateSGC.Visible = true;

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
                    CalendarExtender1.SelectedDate = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01");
                    CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    txtfromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01").ToString();
                    txttoDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
                    BindGridSG();
                }
            }
        }

        protected void btnCreateSGC_Click(object sender, EventArgs e)
        {
            Session["IfEdit"] = false;
            Response.Redirect("~/invsetup/GradingCardMgt.aspx?PID=431");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridSG();
        }

        protected void grdIGP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string Status = null;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton hypEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                LinkButton hypApprove = e.Row.FindControl("LinkApprove") as LinkButton;
                LinkButton hypDelete = e.Row.FindControl("lnkCancel") as LinkButton;
                LinkButton hypPrint = e.Row.FindControl("lnkPrint") as LinkButton;
                LinkButton hypView = e.Row.FindControl("lnkView") as LinkButton;

                Status = (string)DataBinder.Eval(e.Row.DataItem, "status");
                if (Status == "Pending")
                {
                    hypEdit.Visible = true;
                    hypView.Visible = false;
                    hypApprove.Visible = true;
                    hypDelete.Text = "Cancel";

                }
                else if (Status == "Approved")
                {
                    hypView.Visible = true;
                    hypApprove.Visible = false;
                    hypEdit.Visible = false;
                    hypDelete.Visible = false;
                    hypApprove.Visible = false;
                }
                else
                {
                    hypView.Visible = true;
                    hypApprove.Visible = false;
                    hypEdit.Visible = false;
                    hypDelete.Visible = false;
                }
            }
        }

        protected void grdIGP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdIGP.PageIndex = e.NewPageIndex;
            BindGridSG();
        }

        protected void lnkApprove_Click(object sender, EventArgs e)
        {
            string uzrName="";
            string voucherNo = "";
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            
            int sgNo = Convert.ToInt32(vrno.Text.Substring(5).ToString());
            int yr = Convert.ToDateTime(vr_dt.Text).Year;
            if (Session["UserName"] == null)
                uzrName = Request.Cookies["uzr"]["UserName"].ToString();
            else
                uzrName = Session["UserName"].ToString();

            bool res = gC.ChngedSG_Status(sgNo, Convert.ToInt32(vrno.Text.Substring(0,4)), 15, 'A', uzrName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            BindGridSG();
            if (res == true)
            {
                //here-> uncomment for wet blue transfer
                voucherNo = vrno.Text.Substring(0, 4) + vrno.Text.Substring(5);
                string glYear = gC.GetGlYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                List<tblItemDataDet> lstDet = gC.GetItemDetUsingVrNo(Convert.ToInt32(voucherNo), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (lstDet.Count > 0)
                {
                    foreach (tblItemDataDet tbl in lstDet)
                    {
                        tblItem Item = gC.GetItemUsingSelection(tbl.itm_cd, tbl.SelectionId, tbl.LocId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        if (Item != null)
                        {
                            gC.GetUpdTblItemGrading(tbl.LocId, tbl.itm_cd, tbl.SelectionId, Convert.ToInt32(tbl.vr_qty), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        }
                        else
                        {
                            gC.GetInsrtTblitem(tbl.br_id, tbl.LocId, tbl.itm_cd, tbl.SelectionId, Convert.ToInt32(tbl.vr_qty), "00000", Convert.ToDecimal(glYear), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        }
                    }
                }

                ucMessage.ShowMessage("Approved successfully.", RMS.BL.Enums.MessageType.Info);
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            int sgNo = Convert.ToInt32(vrno.Text.Substring(5).ToString());
            int DateYr =Convert.ToDateTime(vr_dt.Text).Year;
            Session["SGno"] = sgNo;
            Session["vrNO"] = vrno.Text;
            Session["IfEdit"] = true;
            Session["DateYear"] = DateYr;
            Session["PrePgAddress"] = "~/invsetup/gradingcardhome.aspx?PID=424";
            Session["PgTile"] = "GradingCardEdit";
            Response.Redirect("~/invsetup/gradingcardmgt.aspx?PID=431");
        }

        protected void lnkView_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            int sgNo = Convert.ToInt32(vrno.Text.Substring(5).ToString());
            int DateYr = Convert.ToDateTime(vr_dt.Text).Year;
            Session["vrNO"] = vrno.Text;
            Session["SGNo"] = sgNo;
            Session["IfEdit"] = true;
            Session["DateYear"] = DateYr;
            Session["PrePgAddress"] = "~/invsetup/gradingcardhome.aspx?PID=424";
            Session["PgTile"] = "GradingCardView";
            Response.Redirect("~/invsetup/gradingcardview.aspx?PID=432");
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            string uzrName = "";
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            int sgNo = Convert.ToInt32(vrno.Text.Substring(5).ToString());
            string date = vr_dt.Text;
            if (Session["UserName"] == null)
                uzrName = Request.Cookies["uzr"]["UserName"].ToString();
            else
                uzrName = Session["UserName"].ToString();
            int yr =  Convert.ToDateTime(date).Year;
            bool res = gC.ChngedSG_Status(sgNo, Convert.ToInt32(vrno.Text.Substring(0, 4)), 15, 'C', uzrName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            BindGridSG();
            if (res == true)
            {
                ucMessage.ShowMessage("Cancelled successfully.", RMS.BL.Enums.MessageType.Info);
            }
            
        }

        #endregion

        #region Helping Method

        public void BindGridSG()
        {
            try
            {
                grdIGP.DataSource = gC.GetGrid_SG(Convert.ToDateTime(txtfromDt.Text), Convert.ToDateTime(txttoDt.Text), Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                grdIGP.DataBind();
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion
    }
}
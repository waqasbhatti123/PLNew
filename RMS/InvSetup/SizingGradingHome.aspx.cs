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
    public partial class sizinggradinghome : BasePage
    {
        #region DataMembers

        InvGP_BL gP = new InvGP_BL();
        InvSG_BL sG = new InvSG_BL();

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
                if (PID == 422)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SizingGradingCard").ToString();
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
            Response.Redirect("~/invsetup/sizinggradingcardmgt.aspx?PID=464");
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

            bool res = sG.ChngedSG_Status(sgNo, Convert.ToInt32(vrno.Text.Substring(0,4)), 14, 'A', uzrName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            BindGridSG();
            if (res == true)
            {
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
            Session["vrNo"] = vrno.Text;
            Session["SGno"] = sgNo;
            Session["IfEdit"] = true;
            Session["DateYear"] = DateYr;
            Session["PrePgAddress"] = "~/invsetup/sizinggradinghome.aspx?PID=422";
            Session["PgTile"] = "SizeGradeEdit";
            Response.Redirect("~/invsetup/sizinggradingcardmgt.aspx?PID=464");
        }

        protected void lnkView_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            int sgNo = Convert.ToInt32(vrno.Text.Substring(5).ToString());
            int DateYr = Convert.ToDateTime(vr_dt.Text).Year;
            Session["IfEdit"] = true;
            Session["SGNo"] = sgNo;
            Session["vrNo"] = vrno.Text;
            Session["DateYear"] = DateYr;
            Session["PrePgAddress"] = "~/invsetup/sizinggradinghome.aspx?PID=422";
            Session["PgTile"] = "SizeGradeView";
            Response.Redirect("~/invsetup/SizingGradingCardView.aspx?PID=427");
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
            bool res = sG.ChngedSG_Status(sgNo, Convert.ToInt32(vrno.Text.Substring(0, 4)), 14, 'C', uzrName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
                grdIGP.DataSource = sG.GetGrid_SG(Convert.ToDateTime(txtfromDt.Text), Convert.ToDateTime(txttoDt.Text), Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
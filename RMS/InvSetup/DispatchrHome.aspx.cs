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
    public partial class DispatchHome : BasePage
    {
        #region DataMembers

        DispatchMgt mgt = new DispatchMgt();
        InvMN_BL mN = new InvMN_BL();

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        public int vrCode
        {
            set { ViewState["vrCode"] = value; }
            get { return Convert.ToInt32(ViewState["vrCode"] ?? 0); }
        }

        public int vrNo
        {
            set { ViewState["vrNo"] = value; }
            get { return Convert.ToInt32(ViewState["vrNo"] ?? 0); }
        }

        public int locID
        {
            set { ViewState["locID"] = value; }
            get { return Convert.ToInt32(ViewState["locID"] ?? 0); }
        }

        #endregion

        #region Event

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
                
                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 448)
                {
                    vrCode = 28;
                    locID = 4;

                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "DispatchHome").ToString();
                    btnCreate.Visible = true;

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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Session["IfEdit"] = false;
            Response.Redirect("~/invsetup/dispatchmgt.aspx?PID=466");
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
                //LinkButton hypCancel = e.Row.FindControl("lnkCancelAfterApprove") as LinkButton;
                LinkButton hypView = e.Row.FindControl("lnkView") as LinkButton;

                Status = (string)DataBinder.Eval(e.Row.DataItem, "status");
                if (Status == "Pending")
                {
                    //hypCancel.Visible = false;
                    hypEdit.Visible = true;
                    hypView.Visible = false;
                    hypApprove.Visible = true;
                    hypDelete.Text = "Cancel";

                }
                else if (Status == "Approved")
                {
                    hypView.Visible = true;
                    //hypCancel.Visible = true;
                    hypApprove.Visible = false;
                    hypEdit.Visible = false;
                    hypDelete.Visible = false;
                    hypApprove.Visible = false;
                }
                else
                {
                    //hypCancel.Visible = false;
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

            string vrNo = vrno.Text;
            vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);

            bool res = mN.ChngedWetBlueTrnsfr_Status(Convert.ToInt32( vrNo), Convert.ToInt32(vrno.Text.Substring(0, 4)), vrCode, 'A', uzrName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            BindGridSG();
            if (res == true)
            {
                mgt.Post2TblItem(Convert.ToInt32(vrNo));
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
            Session["PrePgAddress"] = "~/invSetup/dispatchrhome.aspx?PID=448";
            Session["PgTile"] = "dispatchedit";
            Response.Redirect("~/invsetup/dispatchmgt.aspx?PID=466");
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
            Session["vrNo"] = vrno.Text;
            Session["SGNo"] = sgNo;
            Session["DateYear"] = DateYr;
            Session["PrePgAddress"] = "~/invSetup/dispatchrhome.aspx?PID=448";
            Session["PgTile"] = "dispatchview";
            Response.Redirect("~/invsetup/dispatchview.aspx?PID=467");
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

            string vrNo = vrno.Text;
            vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);

            bool res = mN.ChngedWetBlueTrnsfr_Status(Convert.ToInt32(vrNo), Convert.ToInt32(vrno.Text.Substring(0, 4)), vrCode, 'C', uzrName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            BindGridSG();
            if (res == true)
            {
                ucMessage.ShowMessage("Cancelled successfully.", RMS.BL.Enums.MessageType.Info);
            }
            
        }

        protected void lnkCancelAfterApprove_Click(object sender, EventArgs e)
        {
            try
            {
                //GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                //string vrNum = ((Label)clickedRow.FindControl("lblvr_no")).Text;
                //vrNo = Convert.ToInt32(vrNum.Substring(0, 4) + vrNum.Substring(5));
                //vrCode = 25;

                //tblItemData itm =  GetByID();
                //if (itm != null)
                //{

                //}
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage("Exception: "+ ex.Message, RMS.BL.Enums.MessageType.Error);   
            }
        }

        #endregion

        #region Helping Method
        
        public void BindGridSG()
        {
            try
            {
                grdIGP.DataSource = mN.GetGrid_WetBlueTrnsfr(Convert.ToDateTime(txtfromDt.Text), Convert.ToDateTime(txttoDt.Text), vrCode, locID, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                grdIGP.DataBind();
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public tblItemData GetByID()
        {
            return mN.GetCardRec(vrNo, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        #endregion
    }
}
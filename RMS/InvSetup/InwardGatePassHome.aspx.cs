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
    public partial class InwardGatePassHome : BasePage
    {
        #region DataMembers

        InvGP_BL gP = new InvGP_BL();

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }


        public bool IsInPage
        {
            get { return Convert.ToBoolean(ViewState["IsInPage"]); }
            set { ViewState["IsInPage"] = value; }

        }
    
        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {   
                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 421)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "IGP").ToString();
                    btnCreateGp.Visible = true;
                    
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

                    // txtfromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01").ToString();
                    //txttoDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
                   
                    DateTime dt = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01");

                    if (string.IsNullOrEmpty(Convert.ToString(Session["Mgt_View"])))
                    {
                        Session.Remove("strIGPNo");
                        Session.Remove("dtIGP");
                        
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(Session["dtIGP"])))
                    {
                        dt = Convert.ToDateTime(Session["dtIGP"]);
                        CalendarExtender1.SelectedDate = dt;
                    }
                    txtfromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-01" + "-01").ToString();

                    if(dt<Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString()+"-01"+"-01") )
                    {
                        txtfromDt.Text = dt.ToString();
                    }
                    
                    txttoDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
                    
                    BindGridIGP();
                    Session["Mgt_View"] = null;
                }

                
            }
           
        }
        
        protected void btnCreateGp_Click(object sender, EventArgs e)
        {
            Session["IfEdit"] = false;
            Response.Redirect("~/invsetup/inwardgatepassmgt.aspx?PID=463");
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridIGP();
        }
        
        protected void grdIGP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string Status = null;
            string dRef = "";
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
                    //hypDelete.Visible = false;
                    hypApprove.Visible = false;
                }
                else
                {
                    hypView.Visible = true;
                    hypApprove.Visible = false;
                    hypEdit.Visible = false;
                    // hypDelete.Visible = false;
                }

                //Cancel button visibilty status
                //---------------------------

                dRef = e.Row.Cells[2].Text;
                bool exists = gP.GetIfFeetageCardExists(dRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                if (exists == true || Status == "Cancelled")
                {
                    hypDelete.Visible = false;
                }
                else
                {
                    hypDelete.Visible = true;
                }

                //---------------------------

                //-------For Highlighting gridView Newly saved Row----
                if (!string.IsNullOrEmpty(Convert.ToString(Session["strIGPNo"])))
                {
                    if (e.Row.Cells[0].Text == Session["strIGPNo"].ToString())
                    {
                        IsInPage = true;
                        e.Row.Attributes.Add("style", "background-color:#ffffcc");
                    }
                }
                //-----------
            }
        }
        
        protected void grdIGP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdIGP.PageIndex = e.NewPageIndex;
            BindGridIGP();
        }
        //---------

        protected void grdIGP_DataBound(object sender, EventArgs e)
        {
            if (IsInPage == false && !string.IsNullOrEmpty(Convert.ToString(Session["strIGPNo"])))
            {
                grdIGP.PageIndex = grdIGP.PageIndex + 1;
                BindGridIGP();
            }

        }
        
        //---------
        protected void lnkApprove_Click(object sender, EventArgs e)
        {
            string uzrName = "";
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            //Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label docRef = (Label)clickedRow.FindControl("lblDocRef");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            //int igpNo = Convert.ToInt32(vrno.Text.Substring(5).ToString());
            int yr = Convert.ToDateTime(vr_dt.Text).Year;
            if (Session["UserName"] == null)
                uzrName = Request.Cookies["uzr"]["UserName"].ToString();
            else
                uzrName = Session["UserName"].ToString();

            bool res = gP.ChngedIGPStatus(docRef.Text, yr, 11, 'A', uzrName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            BindGridIGP();
            if (res == true)
            {
                ucMessage.ShowMessage("Approved successfully.", RMS.BL.Enums.MessageType.Info);
            }
        }
        
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
           // Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label dcRf = (Label)clickedRow.FindControl("lblDocRef");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            //int igpNo = Convert.ToInt32(vrno.Text.Substring(5).ToString());
            int DateYr =Convert.ToDateTime(vr_dt.Text).Year;
            //Session["IGPno"] = igpNo;
            Session["DocRef"] = dcRf.Text;
            Session["IfEdit"] = true;
            Session["DateYear"] = DateYr;
            Session["PrePgAddress"] = "~/invsetup/inwardgatepasshome.aspx?PID=421";
            Session["PgTile"] = "InwardGateEdit";
            Response.Redirect("~/invsetup/inwardgatepassmgt.aspx?PID=463");
        }
        
        protected void lnkView_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            //Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label docRef = (Label)clickedRow.FindControl("lblDocRef");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            //int igpNo = Convert.ToInt32(vrno.Text.Substring(5).ToString());
            int DateYr = Convert.ToDateTime(vr_dt.Text).Year;
            //Session["IGPno"] = igpNo;
            Session["IfEdit"] = true;
            Session["DocRef"] = docRef.Text;
            Session["DateYear"] = DateYr;
            Session["PrePgAddress"] = "~/invsetup/inwardgatepasshome.aspx?PID=421";
            Session["PgTile"] = "InwardGateView";
            Response.Redirect("~/invsetup/inwardgatepassview.aspx?PID=428");
        }
        
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            string uzrName = "";
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            //Label vrno = (Label)clickedRow.FindControl("lblvr_no");
            Label docRef = (Label)clickedRow.FindControl("lblDocRef");
            Label vr_dt = (Label)clickedRow.FindControl("lblvrdt");
            Label status = (Label)clickedRow.FindControl("lblstatus");
            //int igpNo = Convert.ToInt32(vrno.Text.Substring(5).ToString());
            int yr = Convert.ToDateTime(vr_dt.Text).Year;
            if (Session["UserName"] == null)
                uzrName = Request.Cookies["uzr"]["UserName"].ToString();
            else
                uzrName = Session["UserName"].ToString();

            bool res = gP.ChngedIGPStatus(docRef.Text, yr, 11, 'C', uzrName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            BindGridIGP();
            if (res == true)
            {
                ucMessage.ShowMessage("Cancelled successfully.", RMS.BL.Enums.MessageType.Info);
            }
            
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {


        }

        #endregion

        #region Helping Method

        public void BindGridIGP()
        {
            try
            {
               // DateTime frmDate = CalendarExtender1.SelectedDate.Value;
               // DateTime toDate = CalendarExtender2.SelectedDate.Value;
                grdIGP.DataSource = gP.GetGrid_IGP(Convert.ToDateTime(txtfromDt.Text), Convert.ToDateTime(txttoDt.Text), Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //grdIGP.DataSource = gP.GetGrid_IGP(frmDate,toDate, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                grdIGP.DataBind();
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception:" + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion
    }
}
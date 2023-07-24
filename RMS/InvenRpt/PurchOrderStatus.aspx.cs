using System;
using RMS.BL;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

namespace RMS.InvenRpt
{
    public partial class PurchOrderStatus : BasePage
    {
        #region DataMembers

        InvReports_BL rptBl = new InvReports_BL();

        //public DateTime fromDate
        //{
        //    get { return Convert.ToDateTime(ViewState["fromDate"]); }
        //    set { ViewState["fromDate"] = value; }
        //}
        //public DateTime toDate
        //{
        //    get { return Convert.ToDateTime(ViewState["toDate"]); }
        //    set { ViewState["toDate"] = value; }
        //}

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
        //public int GroupID
        //{
        //    get { return (ViewState["GroupID"] == null) ? 0 : Convert.ToInt32(ViewState["GroupID"]); }
        //    set { ViewState["GroupID"] = value; }
        //}


        #endregion

        #region Event
    
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                //if (Session["DateFormat"] == null)
                //{
                //    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                //}
                //else
                //{
                //    CalendarExtender1.Format = Session["DateFormat"].ToString();
                //}
                //if (Session["DateFormat"] == null)
                //{
                //    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                //}
                //else
                //{
                //    CalendarExtender2.Format = Session["DateFormat"].ToString();
                //}
                //CalendarExtender1.SelectedDate = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-07" + "-01");
                //CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                

                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 533 || PID == 883)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "PurchOrderStatus").ToString();
                    
                    
                    //txtfromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-07" + "-01").ToString();
                    //txttoDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

                    //FillDropDownLoc();
                }

            }
        }
    
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //DateTime fromDt = Convert.ToDateTime(txtfromDt.Text);
                //DateTime toDt = Convert.ToDateTime(txttoDt.Text);
                int brid = 0;
                string filter = "";
                try
                {
                    //filter = "Location: " + ddlLoc.SelectedItem.Text + "    " +
                    //         "Stock: " + ddlType.SelectedItem.Text + "    " +
                    //         "From Date: " + fromDt.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]) +"    "+
                    //         "To Date: " + toDt.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"])
                    //         ;
                    filter = "Status: " + ddlStatus.SelectedItem.Text ;
                }
                catch
                {
                    //ucMessage.ShowMessage("Invalid date", RMS.BL.Enums.MessageType.Error);
                    //txttoDt.Focus();
                    return;
                }


                reportViewer.Visible = false;
                reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/PurchOrderStatus.rdlc";
                reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.Refresh();

                if (Session["BranchID"] == null)
                {
                    brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
                }
                else
                {
                    brid = Convert.ToInt32(Session["BranchID"].ToString());
                }
                List<spPurchOrderSatusResult> Stock = rptBl.GetPurchOrderStatus(brid, ddlStatus.SelectedValue, ddlOrderBy.SelectedValue , (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                ReportDataSource dataSource = new ReportDataSource("spPurchOrderSatusResult", Stock);

                ReportParameter[] rpt = new ReportParameter[3];
                rpt[0] = new ReportParameter("ReportName", "Purchase Order Status");
                rpt[1] = new ReportParameter("Filter", filter);
                if (Session["CompName"] == null)
                {
                    rpt[2] = new ReportParameter("CompanyName", Request.Cookies["uzr"]["CompName"].ToString(), false);
                }
                else
                {
                    rpt[2] = new ReportParameter("CompanyName", Session["CompName"].ToString(), false);
                }

                reportViewer.LocalReport.SetParameters(rpt);
                reportViewer.LocalReport.DataSources.Clear();

                reportViewer.LocalReport.DataSources.Add(dataSource);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension, filename;

                string ext = "pdf", type = "PDF";
                if (ddlExtension.SelectedValue == "Excel")
                {
                    ext = "xls";
                    type = "Excel";
                }

                byte[] bytes = reportViewer.LocalReport.Render(
                   type, null, out mimeType, out encoding, out extension,
                   out streamids, out warnings);


                filename = string.Format("{0}.{1}", "PurchOrderStatus", ext);
                Response.ClearHeaders();
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                Response.ContentType = mimeType;
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: "+ ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        //public void FillDropDownLoc()
        //{
        //    ddlLoc.DataSource = rptBl.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlLoc.DataValueField = "LocId";
        //    ddlLoc.DataTextField = "LocName";
        //    ddlLoc.DataBind();
        //}
       
        #endregion

        #region Helping Method

        #endregion
    }
}
using System;
#pragma warning disable CS0105 // The using directive for 'System' appeared previously in this namespace
using System;
#pragma warning restore CS0105 // The using directive for 'System' appeared previously in this namespace
#pragma warning disable CS0105 // The using directive for 'System' appeared previously in this namespace
using System;
#pragma warning restore CS0105 // The using directive for 'System' appeared previously in this namespace
#pragma warning disable CS0105 // The using directive for 'System' appeared previously in this namespace
using System;
#pragma warning restore CS0105 // The using directive for 'System' appeared previously in this namespace
using RMS.BL;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace RMS.InvenRpt
{
    public partial class CriticaltemsList : BasePage
    {
        #region DataMembers

        InvReports_BL rptBl = new InvReports_BL();

        public DateTime fromDate
        {
            get { return Convert.ToDateTime(ViewState["fromDate"]); }
            set { ViewState["fromDate"] = value; }
        }
        public DateTime toDate
        {
            get { return Convert.ToDateTime(ViewState["toDate"]); }
            set { ViewState["toDate"] = value; }
        }

        #endregion

        #region Properties
        public int BrId
        {
            get { return Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }
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
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"]);
            }

            if (!IsPostBack)
            {

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
                CalendarExtender1.SelectedDate = Convert.ToDateTime((Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year -1).ToString() + "-07" + "-01").Date;
                CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                

                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 532)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "CriticalItemsListReport").ToString();
                    
                    
                    txtfromDt.Text = Convert.ToDateTime((Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year -1).ToString() + "-07" + "-01").ToString();
                    txttoDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();

                    FillDropDownLoc();
                }
                FillDdlItemGroup();
            }
        }
    
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fromDt = Convert.ToDateTime(txtfromDt.Text);
                DateTime toDt = Convert.ToDateTime(txttoDt.Text);

                string filter = "";
                try
                {
                    filter = "Location: " + ddlLoc.SelectedItem.Text + "    " +
                             "Stock: " + ddlType.SelectedItem.Text + "    " +
                             "From Date: " + fromDt.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]) +"    "+
                             "To Date: " + toDt.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"])
                             ;
                }
                catch
                {
                    ucMessage.ShowMessage("Invalid date", RMS.BL.Enums.MessageType.Error);
                    txttoDt.Focus();
                    return;
                }


                reportViewer.Visible = false;
                reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/CriticalItemsList.rdlc";
                reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.Refresh();

                List<spCriticalItemsResult> Stock = rptBl.GetCriticalItems(Convert.ToInt32(ddlType.SelectedValue), BrId,ddlItemGroup.SelectedValue, Convert.ToInt32(ddlLoc.SelectedValue), fromDt, toDt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                ReportDataSource dataSource = new ReportDataSource("spCriticalItemsResult", Stock);

                ReportParameter[] rpt = new ReportParameter[3];
                rpt[0] = new ReportParameter("ReportName", "Citical Items List");
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
                string extension;
                string filename;


                byte[] bytes = reportViewer.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                filename = string.Format("{0}.{1}", "CriticalItemList", "pdf");
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

        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = rptBl.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
        }
       
        #endregion

        #region Helping Method
        public void FillDdlItemGroup()
        {
            ddlItemGroup.DataValueField = "itm_cd";
            ddlItemGroup.DataTextField = "itm_dsc";
            ddlItemGroup.DataSource = rptBl.GetItemGroup(BrId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItemGroup.DataBind();
        }
        #endregion
    }
}
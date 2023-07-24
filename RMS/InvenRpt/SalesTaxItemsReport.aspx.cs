using System;
using RMS.BL;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

namespace RMS.InvenRpt
{
    public partial class SalesTaxItemsReport : BasePage
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
        public int BrId
        {
            get { return Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }

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

                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 537 || PID == 886)
                {
                        Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "STRpt").ToString();


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
                        CalendarExtender1.SelectedDate = Convert.ToDateTime((Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year - 1).ToString() + "-07" + "-01").Date;
                        CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;


                        //PID = Convert.ToInt32(Request.QueryString["PID"]);
                   
                        //Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "InvenStockStatusReport").ToString();


                        txtfromDt.Text = Convert.ToDateTime((Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year - 1).ToString() + "-07" + "-01").ToString();
                        txttoDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
                        FillDdlItemGroup();
                        FillDropDownLoc();
                        FillDropDownParty();
                }

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
                             "From Date: " + fromDt.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]) +"    "+
                             "To Date: " + toDt.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"])
                             ;
                   
                }
                catch
                {
                    //ucMessage.ShowMessage("Invalid date", RMS.BL.Enums.MessageType.Error);
                    //txttoDt.Focus();
                    return;
                }


                reportViewer.Visible = false;
                reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/SalesTaxItem.rdlc";
                reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.Refresh();

                List<spSalesTaxItemsResult> sTax = rptBl.GetSalesTaxItems(BrId, Convert.ToInt32(ddlLoc.SelectedValue),ddlItemGroup.SelectedValue, ddlParty.SelectedValue, fromDt, toDt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                ReportDataSource dataSource = new ReportDataSource("spSalesTaxItemsResult", sTax);

                ReportParameter[] rpt = new ReportParameter[3];
                rpt[0] = new ReportParameter("ReportName", "Sales Tax Items");
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


                filename = string.Format("{0}.{1}", "Sales Tax Item Report", ext);
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
        public void FillDropDownParty()
        {
            ddlParty.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlParty.DataTextField = "gl_dsc";
            ddlParty.DataValueField = "gl_cd";
            ddlParty.DataBind();
        }
        #endregion

        #region Helping Method
        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = rptBl.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
        }
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
using System;
using RMS.BL;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

namespace RMS.InvenRpt
{
    public partial class LastPriceReport : BasePage
    {
        #region DataMembers

        InvReports_BL rptBl = new InvReports_BL();

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
        public int BrId
        {
            get { return Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }

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

                PID = Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 534)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "LastPriceReport").ToString();
                    FillDdlItemGroup();
                }

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string filter = string.Empty;

                reportViewer.Visible = false;
                reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/LastPriceReport.rdlc";
                reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.Refresh();

                // Status hard coded as Closed to get all records with Closed status
                List<spItemLastPurchaseResult> LastPriceRptData = rptBl.GetItemLastPurchase(ddlItemGroup.SelectedValue, ddlOrderBy.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                ReportDataSource dataSource = new ReportDataSource("spItemLastPurchaseResult", LastPriceRptData);

                ReportParameter[] rpt = new ReportParameter[3];
                rpt[0] = new ReportParameter("ReportName", "Last Price Report");
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


                filename = string.Format("{0}.{1}", "Last Price Report", ext);
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
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
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
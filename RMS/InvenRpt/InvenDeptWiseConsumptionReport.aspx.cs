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
using Microsoft.Reporting.WebForms;
// 
namespace RMS.InvenRpt
{
    public partial class InvenDeptWiseConsumptionReport : BasePage
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
                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 527)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "DeptWiseConsumption").ToString();
                    BindDdlDepartment();
                    //rdolistRptBy.SelectedValue = "Summary";
                    //rdolistRptType.SelectedValue = "Monthly";
                    FillDropDownLoc();
                    FillDdlItemGroup();
                }
            }
        }
    
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdolistRptBy.SelectedValue.Equals("Summary"))
                {
                    getDeptWiseConsumptionSummary();
                }
                else if (rdolistRptBy.SelectedValue.Equals("Detail"))
                {
                    getDeptWiseConsumptionDetail();
                }
                else if (rdolistRptBy.SelectedValue.Equals("CostCenter"))
                {
                    getDeptWiseCCConsumption();
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
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

        public void getDeptWiseCCConsumption()
        {
            int month = 0;
            if (ddlMonth.Enabled)
                month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            int deptId = Convert.ToInt32(ddlDept.SelectedValue);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/DeptWiseCCConsumptiont.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();


            List<spDeptWiseConsumptionDetResult> ConsumptionDet = rptBl.getDeptConsumptionDetailOfCC(BrId,ddlItemGroup.SelectedValue, Convert.ToInt32(ddlLoc.SelectedValue), deptId, month, year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            ReportDataSource dataSource = new ReportDataSource("spDeptWiseConsumptionDetResult", ConsumptionDet);

            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Department Wise (Cost Center) Consumption");
            rpt[1] = new ReportParameter("Year", Convert.ToString(year));
            rpt[2] = new ReportParameter("Month", month == 0 ? "" : Convert.ToString(ddlMonth.SelectedItem.Text));

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);




            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;

            string ext = "pdf", type = "PDF";
            if (ddlExtension.SelectedValue == "Excel")
            {
                ext = "xls";
                type = "Excel";
            }
            byte[] bytes = reportViewer.LocalReport.Render(
               type, null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            filename = string.Format("{0}.{1}", "DeptWiseConsumption", ext);
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        public void getDeptWiseConsumptionSummary()
        {
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            int deptId = Convert.ToInt32(ddlDept.SelectedValue);
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/DeptWiseConsumption.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();


            List<spDeptWiseConsumptionResult> Consumption = rptBl.GetDeptConsumptionRecs(BrId, ddlItemGroup.SelectedValue, Convert.ToInt32(ddlLoc.SelectedValue) ,deptId, month, year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            ReportDataSource dataSource = new ReportDataSource("spDeptWiseConsumptionResult", Consumption);

            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Department Wise Consumption");
            rpt[1] = new ReportParameter("Year", Convert.ToString(year));
            rpt[2] = new ReportParameter("Month",  month == 0 ? "" : Convert.ToString(ddlMonth.SelectedItem.Text));

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);




            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;

            string ext = "pdf", type = "PDF";
            if (ddlExtension.SelectedValue == "Excel")
            {
                ext = "xls";
                type = "Excel";
            }
            byte[] bytes = reportViewer.LocalReport.Render(
               type, null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            filename = string.Format("{0}.{1}", "DeptWiseConsumption", ext);
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        public void getDeptWiseConsumptionDetail()
        {
            int month = 0;
            if(ddlMonth.Enabled)
                month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            int deptId = Convert.ToInt32(ddlDept.SelectedValue);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/DeptWiseConsumptionDet.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();


            List<spDeptWiseConsumptionDetResult> ConsumptionDet = rptBl.getDeptConsumptionDetail(BrId,ddlItemGroup.SelectedValue, Convert.ToInt32(ddlLoc.SelectedValue), deptId, month, year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            ReportDataSource dataSource = new ReportDataSource("spDeptWiseConsumptionDetResult", ConsumptionDet);

            ReportParameter[] rpt = new ReportParameter[3];
            rpt[0] = new ReportParameter("ReportName", "Department Wise Consumption Detail");
            rpt[1] = new ReportParameter("Year", Convert.ToString(year));
            rpt[2] = new ReportParameter("Month", month == 0 ? "" : Convert.ToString(ddlMonth.SelectedItem.Text));

            reportViewer.LocalReport.SetParameters(rpt);
            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.DataSources.Add(dataSource);




            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;

            string ext = "pdf", type = "PDF";
            if (ddlExtension.SelectedValue == "Excel")
            {
                ext = "xls";
                type = "Excel";
            }
            byte[] bytes = reportViewer.LocalReport.Render(
               type, null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            filename = string.Format("{0}.{1}", "DeptWiseConsumption", ext);
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        public void BindDdlDepartment()
        {
            ddlDept.DataTextField = "CodeDesc";
            ddlDept.DataValueField = "CodeID";
            ddlDept.DataSource = new PlCodeBL().GetAll4Grid(3, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDept.DataBind();

            //ddlDept.DataSource = rptBl.GetDeptarments((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlDept.DataValueField = "DeptId";
            //ddlDept.DataTextField = "DeptNme";
            //ddlDept.DataBind();
        }
            
        #endregion
    }
}
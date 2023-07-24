using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using Microsoft.Reporting.WebForms;

namespace RMS.GLSetup
{
    public partial class frmGLCashBook : BasePage
    {
        GlCashBookBL cashBL = new GlCashBookBL();
        COA_BL cty = new COA_BL();
        ListItem selitm = new ListItem();

        #region event

        protected void Page_Load(object sender, EventArgs e)
        {
            int BrId = 0;
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GLCashBook").ToString();

                txtFrom.Text = new DateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year, Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month, 1).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                txtTo.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtFromDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    this.txtToDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    this.txtFromDate.Format = Session["DateFullYearFormat"].ToString();
                    this.txtToDate.Format = Session["DateFullYearFormat"].ToString();
                }
                BindDropDown();
            }
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            string txt = "";
            try
            {
                txt = "fromdate";
                Convert.ToDateTime(txtFrom.Text);
                txt = "todate";
                Convert.ToDateTime(txtTo.Text);
            }
            catch
            {
                if (txt.Equals("fromdate"))
                {
                    ucMessage.ShowMessage("Invalid date from", RMS.BL.Enums.MessageType.Error);
                }
                else
                {
                    ucMessage.ShowMessage("Invalid date to", RMS.BL.Enums.MessageType.Error);
                }
                return;
            }
            CreatePDF("Chart_OF_Account", "pdf"); 
            
        }

        #endregion

        #region helpingmethods

        protected void BindDropDown()
        {
            //ddlgltype.Items.Clear();
            //ddlgltype.Dispose();
            //selitm.Text = "All";
            //selitm.Value = " ";
            //ddlgltype.Items.Insert(0, selitm);
            //ddlgltype.DataTextField = "gt_dsc";
            //ddlgltype.DataValueField = "gt_cd";
            //ddlgltype.DataSource = cty.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            //ddlgltype.DataBind();
        }

        protected void CreatePDF(String FileName, String extension)
        {
            int brid = 0;
            if (Session["BranchID"] == null)
            {
                brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
            }
            else
            {
                brid = Convert.ToInt32(Session["BranchID"].ToString());
            }

            //// Variables
            //Warning[] warnings = null;
            //String[] streamids = null;
            //string mimeType = null;
            //string encoding = null;
            ////The DeviceInfo settings should be changed based on the reportType
            ////http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            //string deviceInfo =
            //"<DeviceInfo>" +
            //"  <OutputFormat>" + extension + "</OutputFormat>" +
            //"  <PageWidth>8.27in</PageWidth>" +
            //"  <PageHeight>11.69in</PageHeight>" +
            //"  <MarginTop>0.5in</MarginTop>" +
            //"  <MarginLeft>0.5in</MarginLeft>" +
            //"  <MarginRight>0.5in</MarginRight>" +
            //"  <MarginBottom>0.5in</MarginBottom>" +
            //"</DeviceInfo>";

            //// Setup the report viewer object and get the array of bytes
            //ReportViewer viewer = new ReportViewer();
            //IQueryable<spRptEmployeeListResult> sal;
            //sal = SalBl.RptEmployeeRec(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            //List<spGL_Cash_BookResult> cash = cashBL.GetGLCashBookResults(brid, Convert.ToDateTime(txtFrom.Text), Convert.ToDateTime(txtTo.Text), txtcodefrom.Text, txtcodeto.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptGlCashBook.rdlc";
            ReportDataSource datasource = new ReportDataSource("spGL_Cash_BookResult", ""); //cash);
            //ReportParameter prm = new ReportParameter("Loan_ReportResult");
            ReportParameter[] paramz = new ReportParameter[1];

            //paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }


            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.SetParameters(paramz);
            //ReportViewer1 = viewer;

            //Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            ////Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + "_" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) + ".") + extension);
            //Response.BinaryWrite(bytes);
            ////// create the file
            ////// send it to the client to download
            //Response.Flush();
        }
        #endregion

        
    }
}

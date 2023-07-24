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
    public partial class frmBgtVarianceRpt : BasePage
    {
        GlBudgetSetupBL bg = new GlBudgetSetupBL();

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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "BgtVarianceRpt").ToString();
                BindDropDown();
            
            }
        }


        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            string y = ddlFinYear.SelectedItem.Text;
            decimal yr = Convert.ToDecimal(y);
            //viewer.Visible = false;
            viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBudgetVariance.rdlc";
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();



            List<spBgtVarianceRptResult> BgtV = bg.BudgetVarianceRpt(yr, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            ReportDataSource dataSource = new ReportDataSource("spBgtVarianceRptResult", BgtV);

            ReportParameter[] rpt = new ReportParameter[3];
            if (Session["CompName"] == null)
            {
                rpt[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                rpt[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            rpt[1] = new ReportParameter("ReportName", "Budget Variance Report");
            rpt[2] = new ReportParameter("rptFrom", " For the Year Jul "+(yr-1).ToString()+" - Jun "+yr);
            //rpt[2] = new ReportParameter("ToDate", toDt.ToString());

            viewer.LocalReport.SetParameters(rpt);
            viewer.LocalReport.DataSources.Clear();

            viewer.LocalReport.DataSources.Add(dataSource);




            //Warning[] warnings;
            //string[] streamids;
            //string mimeType;
            //string encoding;
            //string extension;
            //string filename;


            //byte[] bytes = viewer.LocalReport.Render(
            //   "PDF", null, out mimeType, out encoding,
            //    out extension,
            //   out streamids, out warnings);

            //filename = string.Format("{0}.{1}", "BudgetVarianceRpt", "pdf");
            //Response.ClearHeaders();
            //Response.Clear();
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            //Response.ContentType = mimeType;
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();
 
            
        }
        #endregion

        #region helpingmethods

        public void BindDropDown()
        {

            ddlFinYear.DataSource = bg.getBudgetYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            ddlFinYear.DataBind();
        }

        protected void CreatePDF( )
        {

           
            
            
            
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
            //"  <PageWidth>11.69in</PageWidth>" +
            //"  <PageHeight>8.27in</PageHeight>" +
            //"  <MarginTop>0.3in</MarginTop>" +
            //"  <MarginLeft>0.3in</MarginLeft>" +
            //"  <MarginRight>0.3in</MarginRight>" +
            //"  <MarginBottom>0.3in</MarginBottom>" +
            //"</DeviceInfo>";

            //// Setup the report viewer object and get the array of bytes
            //viewer viewer = new viewer();
            ////IQueryable<spRptEmployeeListResult> sal;
            ////sal = SalBl.RptEmployeeRec(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //IList<spCOAResult> sal = cty.GetReport((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], Convert.ToChar(ddlFinYear.SelectedItem.Value));
            //viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptCOA.rdlc";
            //ReportDataSource datasource = new ReportDataSource("spCOAResult", sal);
            ////ReportParameter prm = new ReportParameter("Loan_ReportResult");
            //ReportParameter[] paramz = new ReportParameter[1];

            ////paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            //if (Session["CompName"] == null)
            //{
            //    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            //}
            //else
            //{
            //    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            //}


            //viewer.LocalReport.DataSources.Clear();
            //viewer.LocalReport.DataSources.Add(datasource);

            //viewer.LocalReport.SetParameters(paramz);
            //viewer1 = viewer;

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

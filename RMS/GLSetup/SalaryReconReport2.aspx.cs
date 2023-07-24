using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using RMS.BL;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

namespace RMS.report.rdlc
{
    public partial class SalaryReconReport2 : System.Web.UI.Page
    {
        //BL.PlReportBL rptBL = new RMS.BL.PlReportBL();

        SlalaryPacakageBL rptBL = new SlalaryPacakageBL();

        #region Properties
        public int CompId
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

        public int PayPerd
        {
            get { return (ViewState["PayPerd"] == null) ? 0 : Convert.ToInt32(ViewState["PayPerd"]); }
            set { ViewState["PayPerd"] = value; }
        }

        #endregion

        //private void FillDropDownPayPeriod()
        //{
        //    BL.SalaryBL SalBl = new RMS.BL.SalaryBL();

        //    this.ddlPayPerd.DataTextField = "PayPerd";
        //    ddlPayPerd.DataValueField = "PayPerd";
        //    ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlPayPerd.DataBind();
        //}

        //protected void Load_report()
        //{
        //    object sal;
        //    sal = rptBL.rptSalaryRecon(CompId, PayPerd, 0,"", "", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ReportViewer viewer = new ReportViewer();
            
        //    viewer.LocalReport.ReportPath = "report/rdlc/rptSalaryRecon2.rdlc";
        //    ReportDataSource datasource = new ReportDataSource("spSalaryReconTORResult", sal);
        //    viewer.LocalReport.DataSources.Clear();
        //    viewer.LocalReport.DataSources.Add(datasource);

        //    viewer.LocalReport.Refresh();
        //    //ReportViewer1 = viewer;
            
        //}


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalReconRpt2").ToString();

                int iCompid;
                if (Session["CompID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    int.TryParse(Request.Cookies["uzr"]["CompID"], out iCompid);
                }
                else
                {
                    int.TryParse(Session["CompID"].ToString(), out iCompid);
                }
                CompId = iCompid;

               // FillDropDownPayPeriod();
            }
        }


        protected void CreatePDF(String FileName, DateTime slectedmonthyear)
        {
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            // ReportViewer viewer =new ReportViewer();
            object sal;

            int month = slectedmonthyear.Month;
            int year = slectedmonthyear.Year;
            //string paypd = ddlPayPerd.SelectedItem.Text;
            //string paypd = "";
            //string yr = paypd.Substring(0, 4);
            //string mn = paypd.Substring(4, 2);


            //DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);

            sal = rptBL.SalaryPackageReport(year, month, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            viewer.LocalReport.ReportPath = "report/rdlc/rptEmpSalSheet.rdlc";
            ReportDataSource datasource = new ReportDataSource("DataSet1", sal);

            ReportParameter[] paramz = new ReportParameter[3];
            paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
            if (Session["CompName"] == null)
            {
                paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
            }
            else
            {
                paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString());
            }

            paramz[2] = new ReportParameter("SalaryMonth", slectedmonthyear.ToString("MMM-yyyy"));

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);


            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);


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
            //"  <PageHeight>11in</PageHeight>" +
            //"  <MarginTop>0.3in</MarginTop>" +
            //"  <MarginLeft>0.3in</MarginLeft>" +
            //"  <MarginRight>0.3in</MarginRight>" +
            //"  <MarginBottom>0.3in</MarginBottom>" +
            //"</DeviceInfo>";

            //Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            ////Response.Buffer = true;
            //Response.ClearHeaders();
            //Response.Clear();
            //Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
            //Response.ContentType = mimeType;
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {

            //int iPeriod;
            //int empID = Convert.ToInt32(ddlEmployee.SelectedValue);
            //PayPerd = iPeriod;

            DateTime slectedMonthYear = Convert.ToDateTime(MonthSelected.Text);



            CreatePDF("SalarySheet", slectedMonthYear);

        }

    }
}

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
using Microsoft.Reporting.WebForms;

namespace RMS.report.rdlc
{
    public partial class SalaryTransferBankReportNew : System.Web.UI.Page
    {
        BL.SalaryBL SalBl = new RMS.BL.SalaryBL();

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

        private void FillDropDownPayPeriod()
        {
            this.ddlPayPerd.DataTextField = "PayPerd";
            ddlPayPerd.DataValueField = "PayPerd";
            ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlPayPerd.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SelectedSalRpt").ToString();
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

                FillDropDownPayPeriod();
                FillDropDownCodeDept();

                lblMsg.Visible = false;
            }
        }

        protected void CreatePDF(String FileName)
        {

            int minSal = 0;
            if(!txtMinSal.Text.Equals(""))
            {
               minSal = Convert.ToInt32(txtMinSal.Text);
            }
            int maxsal = -1;
            if(!txtMaxSal.Text.Equals(""))
            {
               maxsal = Convert.ToInt32(txtMaxSal.Text);
            }

            if (!minSal.Equals(0) && maxsal.Equals(-1))
            {
                if (maxsal < minSal)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Enter maximum salary";
                    return;
                }
            }

            if (!minSal.Equals(0) && !maxsal.Equals(-1))
            {
                if (maxsal < minSal)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Maximum salary cannot be less than minimum salary";
                    return;
                }
            }

            string minparam = txtMinSal.Text;
            if(minparam.Equals(""))
            {
                minparam = "-";
            }
            string maxparam = txtMaxSal.Text;
            if(maxparam.Equals(""))
            {
                maxparam = "-";
            }

            string FilterParam = " | Department: "+ ddlDept.SelectedItem
                               + " | Min. Salary: " + minparam + " | Max. Salary: " + maxparam
                               + " | Job Type: "+ ddlJobType.SelectedItem +" |";

        // Variables
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

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
            //"  <MarginLeft>0.2in</MarginLeft>" +
            //"  <MarginRight>0.2in</MarginRight>" +
            //"  <MarginBottom>0.2in</MarginBottom>" +
            //"</DeviceInfo>";

        // Setup the report viewer object and get the array of bytes
            //ReportViewer viewer =new ReportViewer();
            string paypd = ddlPayPerd.SelectedItem.Text;
            string yr = paypd.Substring(0, 4);
            string mn = paypd.Substring(4, 2);
            DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);

            IQueryable<spSalaryTransferBankNewResult> sal;
            sal = SalBl.RptSalTransferBankNew(CompId, PayPerd, Convert.ToInt32(ddlDept.SelectedValue), minSal, maxsal, ddlJobType.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            viewer.LocalReport.ReportPath = "report/rdlc/rptSalaryTransferBankNew.rdlc";
            ReportDataSource datasource = new ReportDataSource("spSalaryTransferBankNewResult", sal);
            
            ReportParameter[] paramz = new ReportParameter[4];
            paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            if (Session["CompName"] == null)
            {
                paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[2] = new ReportParameter("LogoPath", rptLogoPath);
            paramz[3] = new ReportParameter("FilterParam", FilterParam);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);

            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            

            //Byte[] bytes = viewer.LocalReport.Render(extension=="XLS"?"EXCEL":extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            ////Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
            //Response.BinaryWrite(bytes);
            ////// create the file
            ////// send it to the client to download
            //Response.Flush();
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {


            lblMsg.Visible = false;

            int iPeriod;
            int.TryParse(ddlPayPerd.SelectedValue, out iPeriod);
            PayPerd = iPeriod;

            CreatePDF("BankTransfer");
        }


        private void FillDropDownCodeDept()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 3;

            this.ddlDept.DataTextField = "CodeDesc";
            ddlDept.DataValueField = "CodeID";
            ddlDept.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDept.DataBind();

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Collections;
using Microsoft.Reporting.WebForms;

namespace RMS.GLSetup
{
    public partial class frmVoucherSummary : System.Web.UI.Page
    {

        public frmVoucherSummary()
        { }

        #region datamembers
        VoucherSummaryBL cty = new VoucherSummaryBL();
        PucarAccountReportsBL pucarBl = new PucarAccountReportsBL();
        string[] arrStr = { "All", "Approved", "Pending", "Cancelled" };
        ListItem[] itemArr = new ListItem[4];
        ListItem seitem = new ListItem();
#pragma warning disable CS0414 // The field 'frmVoucherSummary.vouh_Status' is assigned but its value is never used
        ListItemCollection vouh_Status = null;
#pragma warning restore CS0414 // The field 'frmVoucherSummary.vouh_Status' is assigned but its value is never used
        List<ListItem> stat = new List<ListItem>();

        CuurentYearBL cuurentYear = new CuurentYearBL();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

        #endregion
        #region event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            if (!IsPostBack)
            {
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }

                txtFrom.Text = cuurentYear.fromFinalYearDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]); 
                txtTo.Text = cuurentYear.toFinalYearDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "vouchsummary").ToString();
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();

                BindTypeDropDown();
                
                

            }

        }


        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            this.searchBranchDropDown.DataTextField = "br_nme";
            searchBranchDropDown.DataValueField = "br_id";
            if (BranchObj.IsHead == true)
            {
                searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                List<Branch> BranchList = new List<Branch>();

                if (BranchObj != null)
                {
                    if (BranchObj.IsDisplay == true)
                    {
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BranchID).ToList();
                        BranchList.Insert(0, BranchObj);
                    }
                    else
                    {
                        BranchList.Add(BranchObj);
                    }
                }
                searchBranchDropDown.DataSource = BranchList.ToList();
            }
            searchBranchDropDown.DataBind();

        }



        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                   // IsSearch = true;
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                   // BindGrid(Convert.ToInt16(ddlVoucherType.SelectedValue));
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }


        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            CreatePDF("VoucherSummary", "pdf", txtFrom.Text, txtTo.Text, ddltype.SelectedItem.Value, ddlstatus.SelectedItem.Text);

        }
        #endregion
        #region helpingmethod
        private void BindTypeDropDown()
        {
            seitem.Text = "All";
            seitem.Value="0";
            this.ddltype.Items.Clear();
            this.ddltype.Dispose();
            this.ddltype.Items.Insert(0, seitem);
            this.ddltype.DataValueField = "vt_cd";
            this.ddltype.DataTextField = "vt_dsc";
            this.ddltype.DataSource = cty.GetVoucherType((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            this.ddltype.DataBind();
        
        }
        

        protected void CreatePDF(String FileName, String extension,string dtStart,string dtTo,string searchType,string searchStatus)
        {
            // Variables
          
            
            //Warning[] warnings = null;
            //String[] streamids = null;
            //string mimeType = null;
            //string encoding = null;
            DateTime dtFrm = DateTime.Today;
            DateTime.TryParse(dtStart, out dtFrm);
            DateTime dt2 = DateTime.Today;
            DateTime.TryParse(dtTo, out dt2);
            FIN_PERD fnObject = cty.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            if (dtFrm >= fnObject.Start_Date && dt2 <= fnObject.End_Date)
            { }
            else
            {
                dtFrm = Convert.ToDateTime(fnObject.Start_Date);
                dt2 = Convert.ToDateTime(fnObject.End_Date);
            }


            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
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

            // Setup the report viewer object and get the array of bytes
            //ReportViewer viewer = new ReportViewer();
            //IQueryable<spRptEmployeeListResult> sal;
            //sal = SalBl.RptEmployeeRec(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
           
            // List<vwLedger> sal = cty.GetVoucherSummary(BranchID, dtFrm, dt2, searchStatus, searchType, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);

            List<vwLedger> sal = pucarBl.VoucherSummaryReport(BranchID, dtFrm, dt2, searchStatus, searchType, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);

            viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptVoucherSummary.rdlc";
            ReportDataSource datasource = new ReportDataSource("vwLedger", sal);
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

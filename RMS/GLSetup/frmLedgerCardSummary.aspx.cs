using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using RMS.BL;

namespace RMS.GLSetup
{

    public partial class frmLedgerCardSummary : BasePage
    {

        #region DataMembers
        LedgerCardBL cty = new LedgerCardBL();
        CuurentYearBL cuurentYear = new CuurentYearBL();

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"]= value;}
        }

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


                //if(Request.QueryString[""]
                PID = Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 364)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "TBSummary").ToString();
                }
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();

                FillHeadsDetail();

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
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }

        private void FillHeadsDetail()
        {
            RMSDataContext db = new RMSDataContext();

            this.codeDropDown.DataTextField = "gl_dsc";
            codeDropDown.DataValueField = "gl_cd";

            codeDropDown.DataSource = db.Glmf_Codes.Where(x => x.ct_id == "D").ToList();
            codeDropDown.DataBind();


        }
        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            if (PID == 364)
            {
                CreatePDF("TrialBalanceSummary", "pdf", txtFrom.Text, txtTo.Text);
            
            }

        }
        #endregion
        #region helpingmethod
        protected void CreatePDF(String FileName, String extension, string dtStart, string dtTo)
        {
            //// Variables
            //Warning[] warnings = null;
            //String[] streamids = null;
            //string mimeType = null;
            //string encoding = null;

            ////DateTime dtFrm = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ////DateTime.TryParse(dtStart, out dtFrm);
            ////DateTime dt2 = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ////DateTime.TryParse(dtTo + " 23:59:59", out dt2);
            ////FIN_PERD fnObject = cty.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            ////if (dtFrm >= fnObject.Start_Date && dt2 <= fnObject.End_Date)
            ////{ }
            ////else
            ////{
            ////    dtFrm = Convert.ToDateTime(fnObject.Start_Date);
            ////    dt2 = Convert.ToDateTime(fnObject.End_Date);
            ////}

            string txt = "";
            try
            {
                txt = "fdate";
                Convert.ToDateTime(dtStart);
                txt = "tdate";
                Convert.ToDateTime(dtTo);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                if (txt == "fdate")
                {
                    ucMessage.ShowMessage("Invalid from date", RMS.BL.Enums.MessageType.Error);
                    txtFrom.Focus();
                }
                else
                {
                    ucMessage.ShowMessage("Invalid to date", RMS.BL.Enums.MessageType.Error);
                    txtTo.Focus();
                }
                return;
            }
            FIN_PERD fnObject = cty.GetFinancialYearByDate(Convert.ToDateTime(dtStart), (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            DateTime dtFrm = Convert.ToDateTime(dtStart);
            DateTime dt2 = Convert.ToDateTime(dtTo);


            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            //string deviceInfo =null;
            //if (FileName.Equals("LedgerCardBal"))
            //{
            //string deviceInfo =
            // "<DeviceInfo>" +
            // "  <OutputFormat>" + extension + "</OutputFormat>" +
            // "  <PageWidth>8.27in</PageWidth>" +
            // "  <PageHeight>11.69in</PageHeight>" +
            // "  <MarginTop>0.2in</MarginTop>" +
            // "  <MarginLeft>0.2in</MarginLeft>" +
            // "  <MarginRight>0.2in</MarginRight>" +
            // "  <MarginBottom>0.2in</MarginBottom>" +
            // "</DeviceInfo>";
            //}
            //else
            //{
            //    deviceInfo =
            //    "<DeviceInfo>" +
            //    "  <OutputFormat>" + extension + "</OutputFormat>" +
            //    "  <PageWidth>11.69in</PageWidth>" +
            //    "  <PageHeight>8.27in</PageHeight>" +
            //    "  <MarginTop>0.3in</MarginTop>" +
            //    "  <MarginLeft>0.3in</MarginLeft>" +
            //    "  <MarginRight>0.3in</MarginRight>" +
            //    "  <MarginBottom>0.3in</MarginBottom>" +
            //    "</DeviceInfo>";
            //}
            // Setup the report viewer object and get the array of bytes
            //ReportViewer viewer = new ReportViewer();
            //IQueryable<spRptEmployeeListResult> sal;
            //sal = SalBl.RptEmployeeRec(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<sp_LedgerSummaryResult> sal = cty.GetLedgerBalSummary(BranchID, dtFrm, dt2, codeDropDown.SelectedValue.Trim(), fnObject.Gl_Year, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], 'A', ddlType.SelectedValue.Trim(), Convert.ToChar(ddlStatus.SelectedValue));

                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptTBSummary.rdlc";


                ReportDataSource datasource = new ReportDataSource("sp_LedgerSummaryResult", sal);
                ReportParameter[] paramz = new ReportParameter[6];
                if (Session["CompName"] == null)
                {
                    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
                }
                else
                {
                    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
                }
                paramz[1] = new ReportParameter("dateRange", "Period From:  " + dtFrm.ToString("dd-MMM-yyyy") + "  To  " + dt2.ToString("dd-MMM-yyyy"));
                paramz[2] = new ReportParameter("PageTitle", "Trial Balance");
                paramz[3] = new ReportParameter("ClBalDbtTotal", "0");
                paramz[4] = new ReportParameter("ClBalCredTotal", "0");
                paramz[5] = new ReportParameter("RecType", ddlStatus.SelectedValue == "A" ? "" : "Provisional");

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

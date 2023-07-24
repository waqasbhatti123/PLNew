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
using System.Collections.Generic;

namespace RMS.report.rdlc
{
    public partial class frmBankSalaryTransfer : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
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
        //    this.ddlPayPerd.DataTextField = "PayPerd";
        //    ddlPayPerd.DataValueField = "PayPerd";
        //    ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlPayPerd.DataBind();
        //}
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }


        public int JobTypeID
        {
            get { return (ViewState["JobTypeID"] == null) ? 0 : Convert.ToInt32(ViewState["JobTypeID"]); }
            set { ViewState["JobTypeID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalTransRpt").ToString();
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

                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                FillddlJobType();
                GetMonth();
                //FillDropDownPayPeriod();
                //FillDropDownCodeDept();

                //lblMsg.Visible = false;
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


                }

            }
            catch
            { }
        }



        protected void FillddlJobType()
        {
            try
            {
                ddlJobType.DataTextField = "JobTypeName";
                ddlJobType.DataValueField = "JobTypeID";
                using (RMSDataContext dataContext = new RMSDataContext())
                {

                    ddlJobType.DataSource = dataContext.JobTypes.Where(x => x.IsActive == true).ToList();
                    ddlJobType.DataBind();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }


        protected void ddlJobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlJobType.SelectedValue.Equals("0"))
                {
                    JobTypeID = Convert.ToInt32(ddlJobType.SelectedValue);

                }

            }
            catch
            { }
        }
        //protected void CreatePDF(String FileName)
        //{

        //    int minSal = 0;
        //    //if(!txtMinSal.Text.Equals(""))
        //    //{
        //    //   minSal = Convert.ToInt32(txtMinSal.Text);
        //    //}
        //    int maxsal = -1;
        //    //if(!txtMaxSal.Text.Equals(""))
        //    //{
        //    //   maxsal = Convert.ToInt32(txtMaxSal.Text);
        //    //}

        //    if (!minSal.Equals(0) && maxsal.Equals(-1))
        //    {
        //        if (maxsal < minSal)
        //        {
        //            //lblMsg.Visible = true;
        //            //lblMsg.ForeColor = System.Drawing.Color.Red;
        //            //lblMsg.Text = "Enter maximum salary";
        //            //return;
        //        }
        //    }

        //    if (!minSal.Equals(0) && !maxsal.Equals(-1))
        //    {
        //        if (maxsal < minSal)
        //        {
        //            //lblMsg.Visible = true;
        //            //lblMsg.ForeColor = System.Drawing.Color.Red;
        //            //lblMsg.Text = "Maximum salary cannot be less than minimum salary";
        //            //return;
        //        }
        //    }

        //    //string minparam = txtMinSal.Text;
        //    //if(minparam.Equals(""))
        //    //{
        //    //    minparam = "-";
        //    //}
        //    //string maxparam = txtMaxSal.Text;
        //    //if(maxparam.Equals(""))
        //    //{
        //    //    maxparam = "-";
        //    //}

        //    //string FilterParam = " | Department: "+ ddlDept.SelectedItem
        //    //                   + " | Min. Salary: " + minparam + " | Max. Salary: " + maxparam
        //    //                   + " | Job Type: "+ ddlJobType.SelectedItem +" |";

        //// Variables
        //    string rptLogoPath = "";
        //    rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

        ////    Warning[] warnings = null;
        ////    String[] streamids = null;
        ////    string mimeType = null;
        ////    string encoding = null;
        ////    //The DeviceInfo settings should be changed based on the reportType
        ////    //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
        ////    string deviceInfo =
        ////    "<DeviceInfo>" +
        ////    "  <OutputFormat>" + extension + "</OutputFormat>" +
        ////    "  <PageWidth>8.27in</PageWidth>" +
        ////    "  <PageHeight>11.69in</PageHeight>" +
        ////    "  <MarginTop>0.5in</MarginTop>" +
        ////    "  <MarginLeft>0.2in</MarginLeft>" +
        ////    "  <MarginRight>0.2in</MarginRight>" +
        ////    "  <MarginBottom>0.2in</MarginBottom>" +
        ////    "</DeviceInfo>";

        ////// Setup the report viewer object and get the array of bytes
        ////    ReportViewer viewer =new ReportViewer();



        //    //string paypd = ddlPayPerd.SelectedItem.Text;
        //    //string yr = paypd.Substring(0, 4);
        //    //string mn = paypd.Substring(4, 2);
        //   // DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);

        //    IQueryable<spSalaryTransferBankNewResult> sal;
        //    //sal = SalBl.RptSalTransferBankNew(CompId, PayPerd, Convert.ToInt32(ddlDept.SelectedValue), minSal, maxsal, ddlJobType.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    viewer.LocalReport.ReportPath = "report/rdlc/rptSalaryTransferBank.rdlc";
        //    //ReportDataSource datasource = new ReportDataSource("spSalaryTransferBankNewResult", sal);

        //    ReportParameter[] paramz = new ReportParameter[4];
        //    paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", DateTime.Now.ToString("MMM-yyyy"), false);

        //    if (Session["CompName"] == null)
        //    {
        //        paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
        //    }
        //    else
        //    {
        //        paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
        //    }
        //    paramz[2] = new ReportParameter("LogoPath", rptLogoPath);



        //    paramz[3] = new ReportParameter("CompAddress", new CompanyBL().GetByID(CompId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).CompAdd1);

        //    viewer.LocalReport.EnableExternalImages = true;
        //    viewer.LocalReport.Refresh();
        //    viewer.LocalReport.SetParameters(paramz);

        //    viewer.LocalReport.DataSources.Clear();
        //    //viewer.LocalReport.DataSources.Add(datasource);



        //    //Byte[] bytes = viewer.LocalReport.Render(extension=="XLS"?"EXCEL":extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

        //    ////Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
        //    //Response.Buffer = true;
        //    //Response.Clear();
        //    //Response.ContentType = mimeType;
        //    //Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
        //    //Response.BinaryWrite(bytes);
        //    ////// create the file
        //    ////// send it to the client to download
        //    //Response.Flush();
        //}

        //protected void btnGenerat_Click(object sender, EventArgs e)
        //{


        //    //lblMsg.Visible = false;

        //    int iPeriod;
        //    //int.TryParse(ddlPayPerd.SelectedValue, out iPeriod);
        //    //PayPerd = iPeriod;

        //    CreatePDF("BankTransfer");
        //}


        //private void FillDropDownCodeDept()
        //{
        //    tblPlCode pl = new tblPlCode();
        //    byte _cmp = 1;
        //    if (Session["CompID"] == null)
        //    {
        //        _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
        //    }
        //    else
        //    {
        //        _cmp = Convert.ToByte(Session["CompID"].ToString());
        //    }
        //    pl.CompID = _cmp;
        //    pl.CodeTypeID = 3;

        //    //this.ddlDept.DataTextField = "CodeDesc";
        //    //ddlDept.DataValueField = "CodeID";
        //    //ddlDept.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    //ddlDept.DataBind();

        //}


        protected void CreatePDF(String FileName, string MonthVal, int monthID)
        {
            string JobName;
            //int JobID = Convert.ToInt32(ddlJobType.SelectedValue);
            if (JobTypeID != 0)
            {
                JobName = db.JobTypes.Where(x => x.JobTypeID == JobTypeID).FirstOrDefault().JobTypeName;
            }
            else
            {
                JobName = "Officers / Officials";
            }
            
            int Br_ID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            Branch brr = db.Branches.Where(x => x.br_id == Br_ID).FirstOrDefault();
            string br_nme = brr.br_nme;
            int brIId = brr.br_id;
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            
            if (brIId == 1)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransfer.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
                
            }
            else if (brIId == 3)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransferRawal.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 5)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransferFaisal.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 6)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransferSargodh.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 7)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransferMultan.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 8)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransferBahawal.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 10)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransferSahiwal.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 11)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransferskp.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 13)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransferMurac.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 14)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransfer.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 15)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransferPACC.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 16)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/rptEmpBankTransfer.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            ReportParameter[] paramz = new ReportParameter[10];
            
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString());
            }


            if (Session["Br_Address"] == null)
            {
                paramz[1] = new ReportParameter("BranchAddress", Request.Cookies["uzr"]["Br_Address"].ToString());
            }
            else
            {
                paramz[1] = new ReportParameter("BranchAddress", Session["Br_Address"].ToString());
            }


            if (Session["Br_PrimaryContact"] == null)
            {
                paramz[2] = new ReportParameter("PrimaryContact", Request.Cookies["uzr"]["Br_PrimaryContact"].ToString());
            }
            else
            {
                paramz[2] = new ReportParameter("PrimaryContact", Session["Br_PrimaryContact"].ToString());
            }


            if (Session["Br_SecondaryContact"] == null)
            {
                paramz[3] = new ReportParameter("SecondaryContact", Convert.ToString(Request.Cookies["uzr"]["Br_SecondaryContact"]));
            }
            else
            {
                paramz[3] = new ReportParameter("SecondaryContact", Session["Br_SecondaryContact"].ToString());
            }


            paramz[4] = new ReportParameter("SalaryMonth", Convert.ToDateTime(MonthVal).ToString("MMM-yyyy"));
            paramz[5] = new ReportParameter("LogoPath", rptLogoPath);

            if (Session["BankName"] == null)
            {
                paramz[6] = new ReportParameter("BankName", Request.Cookies["uzr"]["BankName"].ToString());
            }
            else
            {
                paramz[6] = new ReportParameter("BankName", Session["BankName"].ToString());
            }
            paramz[7] = new ReportParameter("jobID", JobName);
            paramz[8] = new ReportParameter("br_nme", br_nme);
            paramz[9] = new ReportParameter("br_id", Br_ID.ToString());

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);


            


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

        protected void CreateExcel(String FileName, string MonthVal, int monthID)
        {
            string JobName;
            //int JobID = Convert.ToInt32(ddlJobType.SelectedValue);
            if (JobTypeID != 0)
            {
                JobName = db.JobTypes.Where(x => x.JobTypeID == JobTypeID).FirstOrDefault().JobTypeName;
            }
            else
            {
                JobName = "Officers / Officials";
            }

            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = "Excel";
            //ReportDataSource datasource = null;
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + extension + "</OutputFormat>" +
            "  <PageWidth>8.27in</PageWidth>" +
            "  <PageHeight>11.69in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.15in</MarginLeft>" +
            "  <MarginRight>0.15in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            int Br_ID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            Branch brr = db.Branches.Where(x => x.br_id == Br_ID).FirstOrDefault();
            string br_nme = brr.br_nme;
            int brIId = brr.br_id;
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            if (brIId == 1)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);

            }
            else if (brIId == 3)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 5)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 6)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 7)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 8)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 10)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 11)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 13)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 14)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 15)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            else if (brIId == 16)
            {
                IList<SP_MonthlySalarySummaryEmpReportResult> sal;
                sal = rptBL.SalarySummaryEmpReport(monthID, brIId, JobTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                viewer.LocalReport.ReportPath = "report/rdlc/SalaryTransferExcel.rdlc";
                ReportDataSource datasource = new ReportDataSource("DataSet1", sal);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource);
            }
            //ReportParameter[] paramz = new ReportParameter[10];

            //if (Session["CompName"] == null)
            //{
            //    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
            //}
            //else
            //{
            //    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString());
            //}


            //if (Session["Br_Address"] == null)
            //{
            //    paramz[1] = new ReportParameter("BranchAddress", Request.Cookies["uzr"]["Br_Address"].ToString());
            //}
            //else
            //{
            //    paramz[1] = new ReportParameter("BranchAddress", Session["Br_Address"].ToString());
            //}


            //if (Session["Br_PrimaryContact"] == null)
            //{
            //    paramz[2] = new ReportParameter("PrimaryContact", Request.Cookies["uzr"]["Br_PrimaryContact"].ToString());
            //}
            //else
            //{
            //    paramz[2] = new ReportParameter("PrimaryContact", Session["Br_PrimaryContact"].ToString());
            //}


            //if (Session["Br_SecondaryContact"] == null)
            //{
            //    paramz[3] = new ReportParameter("SecondaryContact", Convert.ToString(Request.Cookies["uzr"]["Br_SecondaryContact"]));
            //}
            //else
            //{
            //    paramz[3] = new ReportParameter("SecondaryContact", Session["Br_SecondaryContact"].ToString());
            //}


            //paramz[4] = new ReportParameter("SalaryMonth", Convert.ToDateTime(MonthVal).ToString("MMM-yyyy"));
            //paramz[5] = new ReportParameter("LogoPath", rptLogoPath);

            //if (Session["BankName"] == null)
            //{
            //    paramz[6] = new ReportParameter("BankName", Request.Cookies["uzr"]["BankName"].ToString());
            //}
            //else
            //{
            //    paramz[6] = new ReportParameter("BankName", Session["BankName"].ToString());
            //}
            //paramz[7] = new ReportParameter("jobID", JobName);
            //paramz[8] = new ReportParameter("br_nme", br_nme);
            //paramz[9] = new ReportParameter("br_id", Br_ID.ToString());

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            //viewer.LocalReport.SetParameters(paramz);



            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + "_" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) + ".") + extension);
            Response.BinaryWrite(bytes);
            //// create the file
            //// send it to the client to download
            Response.Flush();
        }

        protected void btnGenerate_Excel(object sender, EventArgs e)
        {
            if (MonthSelected.Text.Trim() != null && MonthSelected.Text.Trim() != "")
            {
                try
                {
                    if (BranchID == 14 || BranchID == 16)
                    {
                        string slectedMonth = MonthSelected.Text.Trim().ToLower().ToString();

                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonth) && x.BranchID == 1).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreateExcel("SalarySymmaryEmpReport", selectedMonth, selectedMonthID);

                        }
                        else
                        {
                            ucMessage.ShowMessage("Salary transfer Month is not exist", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                    else
                    {
                        string slectedMonth = MonthSelected.Text.Trim().ToLower().ToString();

                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonth) && x.BranchID == BranchID).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreateExcel("SalarySymmaryEmpReport", selectedMonth, selectedMonthID);

                        }
                        else
                        {
                            ucMessage.ShowMessage("Salary transfer Month is not exist", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    ucMessage.ShowMessage(ex.Message.ToString(), RMS.BL.Enums.MessageType.Error);
                }
            }
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {

            if (MonthSelected.Text.Trim() != null && MonthSelected.Text.Trim() != "")
            {
                try
                {
                    if (BranchID == 14 || BranchID == 16)
                    {
                        string slectedMonth = MonthSelected.Text.Trim().ToLower().ToString();

                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonth) && x.BranchID == 1).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreatePDF("SalarySymmaryEmpReport", selectedMonth, selectedMonthID);

                        }
                        else
                        {
                            ucMessage.ShowMessage("Salary transfer Month is not exist", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                    else
                    {
                        string slectedMonth = MonthSelected.Text.Trim().ToLower().ToString();

                        TblSalaryMonth tblSalaryMonth = db.TblSalaryMonths.Where(x => x.MonthVal.ToLower().Equals(slectedMonth) && x.BranchID == BranchID).FirstOrDefault();
                        if (tblSalaryMonth != null)
                        {
                            int selectedMonthID = tblSalaryMonth.MonthID;
                            string selectedMonth = tblSalaryMonth.MonthVal;
                            CreatePDF("SalarySymmaryEmpReport", selectedMonth, selectedMonthID);

                        }
                        else
                        {
                            ucMessage.ShowMessage("Salary transfer Month is not exist", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    ucMessage.ShowMessage(ex.Message.ToString(), RMS.BL.Enums.MessageType.Error);
                }
            }
        }

        protected void GetMonth()
        {
            var month = db.TblSalaryMonths.Where(x => x.MonthIsActive == true && x.BranchID == BranchID).FirstOrDefault();
            if (month.MonthVal != null || month.MonthVal == "")
            {
                this.MonthSelected.Text = month.MonthVal;
            }
            else
            {
                this.MonthSelected.Text = "";
            }
            
        }


    }
}

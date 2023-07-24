using Microsoft.Reporting.WebForms;
using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
// 
namespace RMS.home
{
    public partial class Home : BasePage
    {
        #region DataMembers
        RMSDataContext db = new RMSDataContext();
        EmpBL empBL = new EmpBL();
        

        EmpProfRptBL empProfRptBL = new EmpProfRptBL();
        //DashBoardBL dashBoardBL = new DashBoardBL();



        #endregion

        #region Properties
        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        public int EmpID
        {
            get { return (ViewState["EmpID"] == null) ? 0 : Convert.ToInt32(ViewState["EmpID"]); }
            set { ViewState["EmpID"] = value; }
        }
        //public int GroupID
        //{
        //    get { return (ViewState["GroupID"] == null) ? 0 : Convert.ToInt32(ViewState["GroupID"]); }
        //    set { ViewState["GroupID"] = value; }
        //}


        public static int BranchID = 0;
        public static bool IsSearch = true;
        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
               
                

                //if (Session["DateTimeFormat"] == null)
                //{
                //    Response.Redirect("~/login.aspx");
                //} 

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Home").ToString();

                //Response.Cookies["uzr"].Values["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Home").ToString();
                int GroupID = 0;
                if (Session["GroupID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                }
                else
                {
                    GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                }

                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                FillSearchBranchDropDown();
               // BindGridEmployee(BranchID, IsSearch);
                //if (GroupID == 3)
                //{
                //    Response.Redirect("~/profile/empmgtview.aspx?PID=1");
                //}
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
        //protected void grdEmps_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        //protected void grdEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdEmps.PageIndex = e.NewPageIndex;
        //    BindGridEmployee(BranchID, IsSearch);
        //}

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrint = (LinkButton)sender;
            GridViewRow lnkPrintRow = (GridViewRow)lnkPrint.NamingContainer;
            int rowIndex = lnkPrintRow.RowIndex;

            //EmpID = Convert.ToInt32(grdEmps.DataKeys[rowIndex].Values[0]);

            PrintEmpProfile(EmpID);
        }

        #endregion

        #region Helping Method

        //protected  void BindGridEmployee(int brId, bool isSearch)
        //{
            
        //        var EmpList = empBL.GetAll("","","true",brId,isSearch,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    grdEmps.DataSource = EmpList;
        //    this.grdEmps.DataBind();
        //}

        #endregion


        private void PrintEmpProfile(int empid)
        {
            try
            {
                if (empid > 0)
                {


                    List<spEmpBasicInfoResult> result1 = empProfRptBL.GetEmpBasicInfo(EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    List<spCurrentSalaryPackageResult> result2 = empProfRptBL.GetCurrentSalaryPackage(CompID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.Visible = false;
                    reportViewer.LocalReport.ReportPath = "report/rdlc/rptEmpProfile.rdlc";
                    reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                    reportViewer.LocalReport.Refresh();
                    reportViewer.LocalReport.EnableExternalImages = true;
                    reportViewer.LocalReport.Refresh();

                    string passcoLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
                    string empImagePath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["EmpImage"].ToString().Trim());

                    ReportDataSource dataSource1 = new ReportDataSource("spEmpBasicInfoResult", result1);
                    ReportDataSource dataSource2 = new ReportDataSource("spCurrentSalaryPackageResult", result2);

                    ReportParameter[] rpt = new ReportParameter[4];
                    rpt[0] = new ReportParameter("LogoPath", passcoLogoPath);
                    rpt[1] = new ReportParameter("EmpImagePath", empImagePath + result1.Single().EmpPic);
                    rpt[2] = new ReportParameter("ReportName", "EMPLOYEE REPORT");
                    if (Session["CompName"] == null)
                    {
                        rpt[3] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
                    }
                    else
                    {
                        rpt[3] = new ReportParameter("CompName", Session["CompName"].ToString());
                    }

                    reportViewer.LocalReport.SetParameters(rpt);

                    reportViewer.LocalReport.DataSources.Clear();
                    reportViewer.LocalReport.DataSources.Add(dataSource1);
                    reportViewer.LocalReport.DataSources.Add(dataSource2);

                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string extension;
                    string filename;
                    byte[] bytes = reportViewer.LocalReport.Render(
                       "PDF", null, out mimeType, out encoding,
                        out extension,
                       out streamids, out warnings);
                    filename = string.Format("{0}.{1}", "Employee_Profile_Rpt_EmpID_" + result1.Single().EmpCode, "pdf");
                    Response.ClearHeaders();
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    Response.ContentType = mimeType;
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    ucMessage.ShowMessage("Select an employee to print", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static  void BranchSelectList(int BrId)
        {

            BranchID = BrId;
            //GetChartEmpData();
            //GetChartEmpSalariesData();
            //Home homePg = new Home();
            //IsSearch = true;
            //homePg.BindGridEmployee(BranchID, IsSearch);




        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetChartEmpData()
        {
           
            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                var data = dc.SP_BarChartEmp(BranchID).ToList();
                
                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Deparment",
                "No. of Employee"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.CodeDesc.ToString(), i.count, };
                }


                return chartData;
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetChartEmpDataScaleWise()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                var data = dc.sp_EmpSclaeWise(BranchID).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Scale",
                "No. of Employee"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.ScaleName.ToString(), i.counts, };
                }


                return chartData;
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetChartEmpDataQualiWise()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                var data = dc.EmpQualificationWise(BranchID).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Scale",
                "No. of Employee"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.Name.ToString(), i.Counts, };
                }
                
                return chartData;
            }

        }





        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static object[] GetChartEmpData1()
        //{



        //    //Here MyDatabaseEntities  is our dbContext
        //    using (RMSDataContext dc = new RMSDataContext())
        //    {

        //        var data = dc.SP_BarChartEmp1().ToList();

        //        var chartData = new object[data.Count + 1];
        //        chartData[0] = new object[]{
        //            "Deparment",
        //        "No. of Employee"
        //    };

        //        int j = 0;
        //        foreach (var i in data)
        //        {
        //            j++;

        //            chartData[j] = new object[] { i.CodeDesc.ToString(), i.count };
        //        }


        //        return chartData;
        //    }
        //}


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetChartEmpSalariesData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                var data1 = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault();
                if (data1 != null)
                {
                    int actMonthObj = data1.MonthID;

                    var data = dc.Sp_DptSalaries(actMonthObj, BranchID).ToList();

                    var chartData = new object[data.Count + 1];
                    chartData[0] = new object[]{
                    "Deparment",
                "Employee Salaries"
                    };

                    int j = 0;
                    foreach (var i in data)
                    {
                        j++;
                        chartData[j] = new object[] { i.Dpt.ToString(), i.Salary };
                    }


                    return chartData;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
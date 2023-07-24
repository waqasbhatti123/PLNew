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
    public partial class GLHomecopy : BasePage
    {
        #region DataMembers

        RMSDataContext db = new RMSDataContext();

        voucherDetailBL vBL = new voucherDetailBL();
        //DashBoardBL dashBoardBL = new DashBoardBL();

        public static int BranchID = 0;
        public static bool IsSearch = true;

        #endregion

        #region Properties


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
                
                //if (Session["DateTimeFormat"] == null)
                //{
                //    Response.Redirect("~/login.aspx");
                //} 

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GLHome").ToString();

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
                
                //FillSearchBranchDropDown();
                //searchBranchDropDown.SelectedValue = BranchID.ToString();
                //BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                //BindVouchers();
                BindGridForHead();
                BindGridForRWP();
                BindGridForGuj();
                BindGridForFai();
                BindGridForSar();
                BindGridForMul();
                BindGridForBaw();
                BindGridForDGK();
                BindGridForSah();
                BindGridForMur();
                //if (GroupID == 3)
                //{
                //    Response.Redirect("~/profile/empmgtview.aspx?PID=1");
                //}
            }
        }




        #endregion

        #region Helping Method


        //private void FillSearchBranchDropDown()
        //{
        //    RMSDataContext db = new RMSDataContext();
        //    Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

        //    this.searchBranchDropDown.DataTextField = "br_nme";
        //    searchBranchDropDown.DataValueField = "br_id";
        //    if (BranchObj.IsHead == true)
        //    {
        //        searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
        //    }
        //    else 
        //    {
        //        List<Branch> BranchList = new List<Branch>();
        //        if (BranchObj != null)
        //        {
        //            if (BranchObj.IsDisplay == true)
        //            {
        //                BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BranchID).ToList();
        //                BranchList.Insert(0, BranchObj);
        //            }
        //            else
        //            {
        //                BranchList.Add(BranchObj);
        //            }
        //        }
        //        searchBranchDropDown.DataSource = BranchList.ToList();
        //    }
        //    searchBranchDropDown.DataBind();
        //}




        protected void grdVch_PageIndexChangin(object sender, GridViewPageEventArgs e)
        {

        }

        protected void grdVch_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void grdVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (!searchBranchDropDown.SelectedValue.Equals("0"))
                //{
                //    IsSearch = true;
                //    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                //    this.BindVouchersBranch(BranchID);
                //    // BindSalaryPackage(BranchID, IsSearch);
                //}
                //else
                //{
                //    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                //    this.BindVouchersBranch(BranchID);
                //}
            }
            catch
            { }
        }



        //protected void BindVouchers()
        //{
        //    RMSDataContext Data = new RMSDataContext();
        //    DateTime today = DateTime.Now;
        //    //this.grdVouchers.DataSource = vBL.GetAllVouchers(BranchID,dt2,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    this.grdVouchers.DataSource =  (from a in Data.Glmf_Datas
        //                                    where a.br_id == BranchID
        //                                    && a.vr_dt == DateTime.Now.Date
        //                                    orderby a.vr_dt descending, a.vr_no descending
        //                                    select new
        //                                    {
        //                                        vrid = a.vrid,
        //                                        vt_cd = a.vt_cd,
        //                                        Gl_Year = a.Gl_Year,
        //                                        vr_no = a.Vr_Type.vt_use + "-" + a.vr_no,
        //                                       // headsInvolved = GetGLMFCode(a.vrid, Data),
        //                                        ref_no = a.Ref_no,
        //                                        vr_dt = a.vr_dt,
        //                                        status = a.vr_apr == "P" ? "Pending" :
        //                                                       a.vr_apr == "A" ? "Approved" :
        //                                                       a.vr_apr == "D" ? "Cancelled" : "NULL",
        //                                        vr_nrtn = a.vr_nrtn,
        //                                        a.source
        //                                    }).ToList();
        //    this.grdVouchers.DataBind();
        //}

        //protected void BindVouchersBranch(int Branch)
        //{
        //    RMSDataContext Data = new RMSDataContext();
        //    DateTime today = DateTime.Now;
        //    //this.grdVouchers.DataSource = vBL.GetAllVouchers(BranchID,dt2,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    if (Branch == 0)
        //    {
        //        this.grdVouchers.DataSource = "";
        //    }
        //    else
        //    {
        //        this.grdVouchers.DataSource = (from a in Data.Glmf_Datas
        //                                       where a.br_id == Branch
        //                                       && a.vr_dt == DateTime.Now.Date
        //                                       orderby a.vr_dt descending, a.vr_no descending
        //                                       select new
        //                                       {
        //                                           vrid = a.vrid,
        //                                           vt_cd = a.vt_cd,
        //                                           Gl_Year = a.Gl_Year,
        //                                           vr_no = a.Vr_Type.vt_use + "-" + a.vr_no,
        //                                           // headsInvolved = GetGLMFCode(a.vrid, Data),
        //                                           ref_no = a.Ref_no,
        //                                           vr_dt = a.vr_dt,
        //                                           status = a.vr_apr == "P" ? "Pending" :
        //                                                          a.vr_apr == "A" ? "Approved" :
        //                                                          a.vr_apr == "D" ? "Cancelled" : "NULL",
        //                                           vr_nrtn = a.vr_nrtn,
        //                                           a.source
        //                                       }).ToList();
        //    }
        //    this.grdVouchers.DataBind();
        //}

        protected void BindGridForHead()
        {
            var grdlhr = (from vr in db.Glmf_Datas
                          where vr.br_id == 1 && vr.vr_apr == "A"
                          orderby vr.vr_dt ascending
                          select new
                         {
                             vr.vrid,
                             vr.approvedby,
                             vr.approvedon,
                             vr.updateon,
                             vr.Ref_no,
                             vr.vr_nrtn,
                             vr.vr_dt
                         }).ToList();
            grdVoucherLHR.DataSource = grdlhr.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherLHR.DataBind();
        }
        protected void BindGridForRWP()
        {
            var grdRWP = (from vr in db.Glmf_Datas
                          where vr.br_id == 3 && vr.vr_apr == "A"
                          orderby vr.vr_dt ascending
                          select new
                          {
                              vr.vrid,
                              vr.approvedby,
                              vr.approvedon,
                              vr.updateon,
                              vr.Ref_no,
                              vr.vr_nrtn,
                              vr.vr_dt
                          }).ToList();
            grdVoucherRWP.DataSource = grdRWP.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherRWP.DataBind();
        }
        protected void BindGridForGuj()
        {
            var grdGuj = (from vr in db.Glmf_Datas
                          where vr.br_id == 4 && vr.vr_apr == "A"
                          orderby vr.vr_dt ascending
                          select new
                          {
                              vr.vrid,
                              vr.approvedby,
                              vr.approvedon,
                              vr.updateon,
                              vr.Ref_no,
                              vr.vr_nrtn,
                              vr.vr_dt
                          }).ToList();
            grdVoucherGuj.DataSource = grdGuj.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherGuj.DataBind();
        }
        protected void BindGridForFai()
        {
            var grdFai = (from vr in db.Glmf_Datas
                          where vr.br_id == 5 && vr.vr_apr == "A"
                          orderby vr.vr_dt ascending
                          select new
                          {
                              vr.vrid,
                              vr.approvedby,
                              vr.approvedon,
                              vr.updateon,
                              vr.Ref_no,
                              vr.vr_nrtn,
                              vr.vr_dt
                          }).ToList();
            grdVoucherFai.DataSource = grdFai.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherFai.DataBind();
        }
        protected void BindGridForSar()
        {
            var grdSar = (from vr in db.Glmf_Datas
                          where vr.br_id == 6 && vr.vr_apr == "A"
                          orderby vr.vr_dt ascending
                          select new
                          {
                              vr.vrid,
                              vr.approvedby,
                              vr.approvedon,
                              vr.updateon,
                              vr.Ref_no,
                              vr.vr_nrtn,
                              vr.vr_dt
                          }).ToList();
            grdVoucherSar.DataSource = grdSar.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherSar.DataBind();
        }
        protected void BindGridForMul()
        {
            var grdMul = (from vr in db.Glmf_Datas
                          where vr.br_id == 7 && vr.vr_apr == "A"
                          orderby vr.vr_dt ascending
                          select new
                          {
                              vr.vrid,
                              vr.approvedby,
                              vr.approvedon,
                              vr.updateon,
                              vr.Ref_no,
                              vr.vr_nrtn,
                              vr.vr_dt
                          }).ToList();
            grdVoucherMul.DataSource = grdMul.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherMul.DataBind();
        }
        protected void BindGridForBaw()
        {
            var grdBaw= (from vr in db.Glmf_Datas
                          where vr.br_id == 8 && vr.vr_apr == "A"
                         orderby vr.vr_dt ascending
                          select new
                          {
                              vr.vrid,
                              vr.approvedby,
                              vr.approvedon,
                              vr.updateon,
                              vr.Ref_no,
                              vr.vr_nrtn,
                              vr.vr_dt
                          }).ToList();
            grdVoucherBaw.DataSource = grdBaw.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherBaw.DataBind();
        }
        protected void BindGridForDGK()
        {
            var grdDGK = (from vr in db.Glmf_Datas
                          where vr.br_id == 9 && vr.vr_apr == "A"
                          orderby vr.vr_dt ascending
                          select new
                          {
                              vr.vrid,
                              vr.approvedby,
                              vr.approvedon,
                              vr.updateon,
                              vr.Ref_no,
                              vr.vr_nrtn,
                              vr.vr_dt
                          }).ToList();
            grdVoucherDGK.DataSource = grdDGK.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherDGK.DataBind();
        }
        protected void BindGridForSah()
        {
            var grdDGK = (from vr in db.Glmf_Datas
                          where vr.br_id == 10 && vr.vr_apr == "A"
                          orderby vr.vr_dt ascending
                          select new
                          {
                              vr.vrid,
                              vr.approvedby,
                              vr.approvedon,
                              vr.updateon,
                              vr.Ref_no,
                              vr.vr_nrtn,
                              vr.vr_dt
                          }).ToList();
            grdVoucherSah.DataSource = grdDGK.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherSah.DataBind();
        }
        protected void BindGridForMur()
        {
            var grdMur = (from vr in db.Glmf_Datas
                          where vr.br_id == 13 && vr.vr_apr == "A"
                          orderby vr.vr_dt ascending
                          select new
                          {
                              vr.vrid,
                              vr.approvedby,
                              vr.approvedon,
                              vr.updateon,
                              vr.Ref_no,
                              vr.vr_nrtn,
                              vr.vr_dt
                          }).ToList();
            grdVoucherMur.DataSource = grdMur.OrderByDescending(x => x.vr_dt).Take(5).ToList();
            grdVoucherMur.DataBind();
        }


        #endregion






        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static void BranchSelectList(int BrId)
        {

            BranchID = BrId;
            

        }







        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static object[] GetChartEmpSalariesinPrevMonthData()
        //{

        //    //Here MyDatabaseEntities  is our dbContext
        //    using (RMSDataContext dc = new RMSDataContext())
        //    {
        //        int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

        //        var data = dc.Sp_SalariesVSExpenses(actMonthObj, BranchID).ToList();

        //        var chartData = new object[data.Count + 1];
        //        chartData[0] = new object[]{
        //            "Months",
        //        "Salaries"
        //    };

        //        int j = 0;
        //        foreach (var i in data)
        //        {
        //            j++;
        //            chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
        //        }
        //        return chartData;
        //    }
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetChartEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 1).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] RawalEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 3).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GujranEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 4).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] FaisEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 5).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] SarEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 6).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] MulEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 7).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] BahEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 8).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] DgEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 9).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] SaEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 10).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] MurEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, 13).ToList();

                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Months",
                "Salaries"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.MonthVal.ToString(), i.Salary };
                }
                return chartData;
            }
        }

    }
}
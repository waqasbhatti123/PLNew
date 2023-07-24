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
    public partial class GLHomeBudCharts : BasePage
    {
        #region DataMembers


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
                
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                BindVouchers();
                //if (GroupID == 3)
                //{
                //    Response.Redirect("~/profile/empmgtview.aspx?PID=1");
                //}
            }
        }




        #endregion

        #region Helping Method


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
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                    IsSearch = true;
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    this.BindVouchersBranch(BranchID);
                    // BindSalaryPackage(BranchID, IsSearch);
                }
                else
                {
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    this.BindVouchersBranch(BranchID);
                }
            }
            catch
            { }
        }



        protected void BindVouchers()
        {
            RMSDataContext Data = new RMSDataContext();
            DateTime today = DateTime.Now;
            //this.grdVouchers.DataSource = vBL.GetAllVouchers(BranchID,dt2,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdVouchers.DataSource =  (from a in Data.Glmf_Datas
                                            where a.br_id == BranchID
                                            && a.vr_dt == DateTime.Now.Date
                                            orderby a.vr_dt descending, a.vr_no descending
                                            select new
                                            {
                                                vrid = a.vrid,
                                                vt_cd = a.vt_cd,
                                                Gl_Year = a.Gl_Year,
                                                vr_no = a.Vr_Type.vt_use + "-" + a.vr_no,
                                               // headsInvolved = GetGLMFCode(a.vrid, Data),
                                                ref_no = a.Ref_no,
                                                vr_dt = a.vr_dt,
                                                status = a.vr_apr == "P" ? "Pending" :
                                                               a.vr_apr == "A" ? "Approved" :
                                                               a.vr_apr == "D" ? "Cancelled" : "NULL",
                                                vr_nrtn = a.vr_nrtn,
                                                a.source
                                            }).ToList();
            this.grdVouchers.DataBind();
        }

        protected void BindVouchersBranch(int Branch)
        {
            RMSDataContext Data = new RMSDataContext();
            DateTime today = DateTime.Now;
            //this.grdVouchers.DataSource = vBL.GetAllVouchers(BranchID,dt2,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (Branch == 0)
            {
                this.grdVouchers.DataSource = "";
            }
            else
            {
                this.grdVouchers.DataSource = (from a in Data.Glmf_Datas
                                               where a.br_id == Branch
                                               && a.vr_dt == DateTime.Now.Date
                                               orderby a.vr_dt descending, a.vr_no descending
                                               select new
                                               {
                                                   vrid = a.vrid,
                                                   vt_cd = a.vt_cd,
                                                   Gl_Year = a.Gl_Year,
                                                   vr_no = a.Vr_Type.vt_use + "-" + a.vr_no,
                                                   // headsInvolved = GetGLMFCode(a.vrid, Data),
                                                   ref_no = a.Ref_no,
                                                   vr_dt = a.vr_dt,
                                                   status = a.vr_apr == "P" ? "Pending" :
                                                                  a.vr_apr == "A" ? "Approved" :
                                                                  a.vr_apr == "D" ? "Cancelled" : "NULL",
                                                   vr_nrtn = a.vr_nrtn,
                                                   a.source
                                               }).ToList();
            }
            this.grdVouchers.DataBind();
        }



        #endregion






        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static void BranchSelectList(int BrId)
        {

            BranchID = BrId;
            

        }




     


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetChartEmpSalariesinPrevMonthData()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                int actMonthObj = dc.TblSalaryMonths.Where(x => x.MonthIsActive == true).FirstOrDefault().MonthID;

                var data = dc.Sp_SalariesVSExpenses(actMonthObj, BranchID).ToList();

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
        public static object[] GetAppBudPieChart()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                decimal gl = dc.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().Gl_Year;

                var data = dc.sp_ApprovedBudgetPieChart(Convert.ToInt32(gl)).ToList();
                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                    "Division",
                "Budget"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.br_nme.ToString(), i.bud };
                }


                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetPerAppBudBarChart()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                decimal gl = dc.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().Gl_Year;

                var data = dc.sp_PerAppBarchart(Convert.ToInt32(gl)).ToList();
                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                "Division",
                "Demand",
                "SNE Allocation"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.br_nme.ToString(), i.Proposed, i.Approved };
                }


                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetAppEstConStackChart()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                decimal gl = dc.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().Gl_Year;

                var data = dc.sp_ApprovedBudEstConti(Convert.ToInt32(gl), "0301020017", "030102", "030101").ToList();
                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                "Division",
                "Establisment",
                "Contigent",
                "Cultural"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.br_nme.ToString(), i.EstablishmentExpense, i.ContigentExpense, i.CulturalExpense };
                }


                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetAppIncoGranttackChart()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                decimal gl = dc.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().Gl_Year;

                var data = dc.sp_ApprovedIncomGrantSum(Convert.ToInt32(gl)).ToList();
                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                "Division",
                "Grant",
                "Income",
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.br_nme.ToString(), i.grantt, i.income };
                }


                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetAppbudgetCouncilsStackChart()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                decimal gl = dc.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().Gl_Year;

                var data = dc.sp_ApprovedBudgetBifurcation(Convert.ToInt32(gl), "0301020017", "030102", "030101").ToList();
                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                "Division",
                "Establisment",
                "Contigent",
                "Cultural"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] { i.br_nme.ToString(), i.EstablishmentExpense, i.ContigentExpense, i.CulturalExpense };
                }


                return chartData;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetAppbudgetbifructionStackChart()
        {

            //Here MyDatabaseEntities  is our dbContext
            using (RMSDataContext dc = new RMSDataContext())
            {
                decimal gl = dc.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().Gl_Year;

                // var data = dc.sp_BudgetvsUtilization(Convert.ToInt32(gl)).ToList();
                //var data = (from re in dc.ReviseBudgetExpenditures
                //           join bra in dc.Branches on re.branch equals bra.br_id
                //           select new
                //           {
                //               re.RiveiseBudget,
                //               re.TotalExpenditure,
                //               bra.br_nme
                //           }).ToList();

                var data = dc.sp_BudgetvsUtilization(Convert.ToInt32(gl)).ToList();
                var chartData = new object[data.Count + 1];
                chartData[0] = new object[]{
                "Division",
                "Budget",
                "Utilization"
            };

                int j = 0;
                foreach (var i in data)
                {
                    j++;
                    chartData[j] = new object[] {i.Branch,i.ApprovedBudget,i.ConsumedBudget };
                }
                return chartData;
            }
        }

    }
}
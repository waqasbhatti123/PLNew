using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.GLSetup
{
    public partial class SNEApproved : System.Web.UI.Page
    {
        voucherDetailBL objVoucher = new voucherDetailBL();

        public int BrId
        {
            get { return (ViewState["BrId"] == null) ? 0 : Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }

        public static decimal Financialyear
        {
            get; set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SNE").ToString();
               
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
                Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                FillSearchBranchDropDown();
            }
        }


        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            //this.searchBranchDropDown.DataTextField = "br_nme";
            //this.searchBranchDropDown.DataValueField = "br_id";
            //this.searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_id == 15).ToList();
            //this.searchBranchDropDown.DataBind();


            Branch BranchObj = db.Branches.Where(x => x.br_id == BrId).FirstOrDefault();

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
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BrId).ToList();
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

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static object BindGridJquery(int brID)
        {
            return new BudgetBL().GetSNERelease((RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"], Financialyear, brID);
        }
    }
}
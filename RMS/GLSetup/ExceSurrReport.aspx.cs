using Microsoft.Reporting.WebForms;
using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.GLSetup
{
    public partial class ExceSurrReport : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        ExcessSurrenderBL exc = new ExcessSurrenderBL();
        
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
                    Session["PageTitle"] = "Excess & Surrenders Reports";
                
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BrId.ToString();
            }
        }



        private void FillSearchBranchDropDown()
        {
            

            Branch BranchObj = db.Branches.Where(x => x.br_id == BrId).FirstOrDefault();

            this.searchBranchDropDown.DataTextField = "br_nme";
            searchBranchDropDown.DataValueField = "br_id";
            //if (BranchObj.IsHead == true)
            //{
                searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true && x.br_id == BrId).ToList();
            //}
            //else
            //{
            //    List<Branch> BranchList = new List<Branch>();

            //    if (BranchObj != null)
            //    {
            //        if (BranchObj.IsDisplay == true)
            //        {
            //            BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BrId).ToList();
            //            BranchList.Insert(0, BranchObj);
            //        }
            //        else
            //        {
            //            BranchList.Add(BranchObj);
            //        }
            //    }
            //    searchBranchDropDown.DataSource = BranchList.ToList();
            //}
            searchBranchDropDown.DataBind();

        }

        protected void Save_click(object sender, EventArgs e)
        {
            string gl = SelectedYear.SelectedValue;
            string[] yr = gl.Split('-');
            int glyear = Convert.ToInt32(yr[1]);
            Branch brName = db.Branches.Where(x => x.br_id == BrId).FirstOrDefault();
            decimal fin = db.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().Gl_Year;
            int EsID = Convert.ToInt32(ddlExcess.SelectedValue);
            if (EsID == 1)
            {
                IList<sp_ExcessSurrenderResult> ex = exc.GetExcessReport(BrId, glyear, EsID);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptExcess.rdlc";
                ReportDataSource reportData = new ReportDataSource("DataSet1", ex);
                ReportParameter[] param = new ReportParameter[5];
                param[0] = new ReportParameter("fin", fin.ToString());
                param[1] = new ReportParameter("DivName", brName.br_nme);
                param[2] = new ReportParameter("DivCode", brName.LoCode);
                param[3] = new ReportParameter("EsID", EsID.ToString());
                param[4] = new ReportParameter("SelectYr", gl.ToString());

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(param);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(reportData);
            }
            else
            {
                IList<sp_ExcessSurrenderResult> ex = exc.GetExcessReport(BrId, glyear, EsID);
                viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptExcess2.rdlc";
                ReportDataSource reportData = new ReportDataSource("DataSet1", ex);
                ReportParameter[] param = new ReportParameter[5];
                param[0] = new ReportParameter("fin", fin.ToString());
                param[1] = new ReportParameter("DivName", brName.br_nme);
                param[2] = new ReportParameter("DivCode", brName.LoCode);
                param[3] = new ReportParameter("EsID", EsID.ToString());
                param[4] = new ReportParameter("SelectYr", gl.ToString());

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(param);
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(reportData);
            }
            
        }
        protected void Clear_Click(object sender, EventArgs e)
        {

        }
    }
}
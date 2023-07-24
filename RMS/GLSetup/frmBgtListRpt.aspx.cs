using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using Microsoft.Reporting.WebForms;

namespace RMS.GLSetup
{
    public partial class frmBgtListRpt : BasePage
    {
        GlBudgetSetupBL bg = new GlBudgetSetupBL();

        ListItem selitm = new ListItem();

        #region event
        protected void Page_Load(object sender, EventArgs e)
        {
            int BrId = 0;
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

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "BgtListRpt").ToString();
           
           }
        }


        protected void btnGenerat_Click(object sender, EventArgs e)
        {


            generateReport();
        }
        #endregion

        #region helpingmethods



        public void generateReport()
        {
        //    //string y = ddlFinYear.SelectedItem.Text;
        //    // decimal yr = Convert.ToDecimal(y);
            //viewer.Visible = false;
            viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptBudgetList.rdlc";
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();



            List<tblBgtHead> Bgt = bg.rptBudgetList((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            ReportDataSource dataSource = new ReportDataSource("tblBgtHead", Bgt);

            ReportParameter[] rpt = new ReportParameter[2];
            if (Session["CompName"] == null)
            {
                rpt[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                rpt[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            rpt[1] = new ReportParameter("ReportName", "Budget List Report");
           // rpt[1] = new ReportParameter("rptFrom", " For the Year Jul " + (yr - 1).ToString() + " - Jun " + yr);
            //rpt[2] = new ReportParameter("ToDate", toDt.ToString());

            viewer.LocalReport.SetParameters(rpt);
            viewer.LocalReport.DataSources.Clear();

            viewer.LocalReport.DataSources.Add(dataSource);




            //Warning[] warnings;
            //string[] streamids;
            //string mimeType;
            //string encoding;
            //string extension;
            //string filename;


            //byte[] bytes = viewer.LocalReport.Render(
            //   "PDF", null, out mimeType, out encoding,
            //    out extension,
            //   out streamids, out warnings);

            //filename = string.Format("{0}.{1}", "BudgetListRpt", "pdf");
            //Response.ClearHeaders();
            //Response.Clear();
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            //Response.ContentType = mimeType;
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();
        }
        #endregion

        
    }
}

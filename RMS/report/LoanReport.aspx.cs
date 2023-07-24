using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

namespace RMS.Setup
{
    public partial class LoanReport : BasePage
    {

        #region DataMembers

       //RMS.BL.tblPlEmpData empDate = new tblPlEmpData();
      // RMS.BL.tblPlCode plcode = new tblPlCode();
       RMS.BL.tblCompany company = new tblCompany();
      // RMS.BL.tblPlCode code = new tblPlCode();

        PlReportBL report = new PlReportBL();

        string frst = null;
        string scnd = null;     

        #endregion

        #region Properties
        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }

        }
    
        #endregion

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "LoanReport").ToString();

                //if (Session["DateFullYearFormat"] == null)
                //{
                //    txtReportDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                 
                //}
                //else
                //{
                //    txtReportDateCal.Format = Session["DateFullYearFormat"].ToString();
                BindGrid();
                GetByID();
           //}


            }

           
        
        }

        protected void grdEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           grdlev.PageIndex = e.NewPageIndex;
            BindGrid();

           // BindGrid(txtFltEmp.Text.Trim(), Convert.ToInt32(ddlFltRegion.SelectedValue), Convert.ToInt32(ddlFltSegment.SelectedValue));
        }
       
        protected void grdEmps_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text.Length > 20)
                {
                    frst = e.Row.Cells[2].Text.Substring(0, 15);
                    scnd = e.Row.Cells[2].Text.Substring(15, e.Row.Cells[2].Text.Length - 15);

                    e.Row.Cells[2].Text = frst + "<br/>" + scnd;
                }

                //if (e.Row.Cells[3].Text.Length > 43)
                //{
                //    frst = e.Row.Cells[3].Text.Substring(0, 29);
                //    scnd = e.Row.Cells[3].Text.Substring(29, e.Row.Cells[3].Text.Length - 29);

                //    e.Row.Cells[3].Text = frst + "<br/>" + scnd;
                //}

                if (Session["DateFormat"] == null)
                {

                    e.Row.Cells[6].Text = DateTime.Parse(e.Row.Cells[6].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());

                }
                else
                {
                    e.Row.Cells[6].Text = DateTime.Parse(e.Row.Cells[6].Text).ToString(Session["DateFormat"].ToString());
                }


                //if (!e.Row.Cells[7].Text.Equals("&nbsp;"))
                //{
                //    e.Row.Cells[7].Text = DateTime.Parse(e.Row.Cells[7].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                //}
                ////else 
                //{
                //   // e.Row.Cells[7].Text = DateTime.Parse(e.Row.Cells[7].Text).ToString(Session["DateFormat"].ToString());
                    
                //}
            }
        }

        #endregion

        #region Helping Method
        protected void BindGrid()
        {
            grdlev.DataSource = report.GetLoanReport((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdlev.DataBind();

        }
        protected void GetByID()
        {
            if (Session["CompID"] == null)
            {
                CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                CompID = Convert.ToByte(Session["CompID"].ToString());
            }
            company = report.GetByID(CompID,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            tblPlCode code = report.GetByPlCode(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            tblPlCode div = report.GetByPlDiv(2, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            
            //this.rblStatus.SelectedValue = cty.Enabled == true ? "1" : "0";
            //ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
            this.cpnyidv.Text = company.CompName;
            this.regidv.Text = code.CodeDesc;
            this.dividv.Text = div.CodeDesc;
        
        }
        #endregion

       
     
    }
}
 
using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;

namespace RMS.Setup
{
    public partial class EmpMgtReport : BasePage
    {

        #region DataMembers

        RMS.BL.tblPlEmpData empDate = new tblPlEmpData();

        PlReportBL report = new PlReportBL();

        string frst = null;
        string scnd = null;     

        #endregion

        #region Properties
       

        #endregion

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                txtReportDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yyyy");
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpReport").ToString();

                if (Session["DateFullYearFormat"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    txtReportDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                 
                }
                else
                {
                    txtReportDateCal.Format = Session["DateFullYearFormat"].ToString();
                   
                }

            
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
                if (e.Row.Cells[3].Text.Length > 20)
                {
                    frst = e.Row.Cells[3].Text.Substring(0, 15);
                    scnd = e.Row.Cells[3].Text.Substring(15, e.Row.Cells[3].Text.Length - 15);

                    e.Row.Cells[3].Text = frst + "<br/>" + scnd;
                }

                if (e.Row.Cells[4].Text.Length > 43)
                {
                    frst = e.Row.Cells[4].Text.Substring(0, 29);
                    scnd = e.Row.Cells[4].Text.Substring(29, e.Row.Cells[4].Text.Length - 29);

                    e.Row.Cells[4].Text = frst + "<br/>" + scnd;
                }

                if (Session["DateFormat"] == null)
                {

                    e.Row.Cells[8].Text = DateTime.Parse(e.Row.Cells[8].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                    
                }
                else
                {
                    e.Row.Cells[8].Text = DateTime.Parse(e.Row.Cells[8].Text).ToString(Session["DateFormat"].ToString()); 
                    
                }


                if (!e.Row.Cells[9].Text.Equals("&nbsp;"))
                {
                    e.Row.Cells[9].Text = DateTime.Parse(e.Row.Cells[9].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                //else 
                //{
                //   // e.Row.Cells[9].Text = DateTime.Parse(e.Row.Cells[9].Text).ToString(Session["DateFormat"].ToString());
                    
                //}
            }
        }

        #endregion

        #region Helping Method
        protected void BindGrid()
        {
            grdlev.DataSource = report.GetAllDate(Convert.ToDateTime(txtReportDate.Text), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdlev.DataBind();

        }
       
        #endregion

        protected void btnReport_Click(object sender, EventArgs e)
        {
            if (!txtReportDate.Text.Equals(""))
            {
                this.BindGrid();
            }
           
        }
    }
}
 
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
using System.Windows.Forms;
using System.IO;

namespace RMS.report
{
    public partial class AttendanceReport : System.Web.UI.Page
    {

        #region Data Members

        AttendanceBL attendBl = new AttendanceBL();
        DataTable dTable = new DataTable();
  
        #endregion

        #region Properties

        public int CompId
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

        public int DaysInMonth
        {
            get { return (ViewState["DaysInMonth"] == null) ? 0 : Convert.ToInt32(ViewState["DaysInMonth"]); }
            set { ViewState["DaysInMonth"] = value; }
        }

        public int NoOfWorkDays
        {
            get { return (ViewState["NoOfWorkDays"] == null) ? 0 : Convert.ToInt32(ViewState["NoOfWorkDays"]); }
            set { ViewState["NoOfWorkDays"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "AttenReport").ToString();
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

                try
                {
                    ddlYear.SelectedValue = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString(); ;
                    ddlMonth.SelectedValue = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month.ToString();
                }
                catch { }
            }
        }

        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateReport();

                string rptLogoPath = "";
                rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

                viewer.LocalReport.ReportPath = "report/rdlc/rptAttendance.rdlc";
                
                ReportDataSource datasource1 = new ReportDataSource("dsAttendance_Attendance_28_Days", dTable);
                ReportDataSource datasource2 = new ReportDataSource("dsAttendance_Attendance_29_Days", dTable);
                ReportDataSource datasource3 = new ReportDataSource("dsAttendance_Attendance_30_Days", dTable);
                ReportDataSource datasource4 = new ReportDataSource("dsAttendance_Attendance_31_Days", dTable);

                ReportParameter[] paramz = new ReportParameter[4];

                if (Session["CompName"] == null)
                {
                    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
                }
                else
                {
                    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
                }
                paramz[1] = new ReportParameter("LogoPath", rptLogoPath);
                paramz[2] = new ReportParameter("NoOfWorkDays", DaysInMonth.ToString());
                paramz[3] = new ReportParameter("ForTheMonth", ddlMonth.SelectedItem.Text +" "+ ddlYear.SelectedItem.Text);

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();
                viewer.LocalReport.SetParameters(paramz);

                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(datasource1);
                viewer.LocalReport.DataSources.Add(datasource2);
                viewer.LocalReport.DataSources.Add(datasource3);
                viewer.LocalReport.DataSources.Add(datasource4);
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            try
            {
                //Exporting Grid to Excel
                //GridViewExportUtil.Export("AttendanceReport.xls", this.grdAttendance, GridViewExportUtil.FileType.Excel);
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion

        #region Helping Method

        public void GenerateReport()
        {
            DaysInMonth = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue));
            //convert.todatetime() format (mm-dd-yyyy)
            DateTime rptDate = Convert.ToDateTime(ddlMonth.SelectedValue + "-" + "1" + "-" + ddlYear.SelectedValue);
            DateTime curDate = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month.ToString() + "-" + "1" + "-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString());

            if (rptDate < curDate)
            {
                NoOfWorkDays = DaysInMonth;
            }
            else if (rptDate == curDate)
            {
                NoOfWorkDays = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Day;
            }
            else
            {
                NoOfWorkDays = 0;
            }

            GetInitials();
            GetRecords(rptDate);
            BindGrid();
     
        }

        public void BindGrid()
        {
            //grdAttendance.DataSource = dTable;
            //grdAttendance.DataBind();
        }

        public void GetRecords(DateTime rptDate)
        {
            List<Anonymous4Attendance> emps = attendBl.GetEmployees(CompId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            foreach (Anonymous4Attendance emp in emps)
            {
                DataRow dRow = dTable.NewRow();

                dRow["Region"] = emp.Region;
                dRow["Department"] = emp.Department;
                dRow["Division"] = emp.Division;
                dRow["Designation"] = emp.Designation;
                dRow["EmpID"] = emp.EmpID;
                dRow["EmpCode"] = emp.EmpCode;
                dRow["Name"] = emp.Name;

                List<int> listLeaveDays = attendBl.GetEmpLeaves(CompId, Convert.ToInt32(emp.EmpID), rptDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (listLeaveDays.Count == 0)
                {
                    for (int i = 1; i <= NoOfWorkDays; i++)
                    {
                        if (emp.DOJ.Year == rptDate.Year && emp.DOJ.Month == rptDate.Month)
                        {
                            if (i < emp.DOJ.Day)
                            {
                                dRow[Convert.ToString(i)] = "-";
                            }
                            else
                            {
                                if (dTable.Rows[0][Convert.ToString(i)].Equals("Sun"))
                                    dRow[Convert.ToString(i)] = "";
                                else
                                    dRow[Convert.ToString(i)] = "P";
                            }
                        }
                        else if (emp.DOJ > rptDate)
                        {
                            dRow[Convert.ToString(i)] = "-";
                        }
                        else if (emp.DOJ < rptDate)
                        {
                            if (dTable.Rows[0][Convert.ToString(i)].Equals("Sun"))
                                dRow[Convert.ToString(i)] = "";
                            else
                                dRow[Convert.ToString(i)] = "P";
                        }
                    }
                }
                else if (listLeaveDays.Count > 0)
                {
                    for (int i = 1; i <= NoOfWorkDays; i++)
                    {
                        if (emp.DOJ.Year == rptDate.Year && emp.DOJ.Month == rptDate.Month)
                        {
                            if ( i < emp.DOJ.Day)
                            {
                                dRow[Convert.ToString(i)] = "-";
                            }
                            else
                            {
                                if (listLeaveDays.Contains(i))
                                {
                                    string leavedate = ddlMonth.SelectedValue + "-" + i.ToString() + "-" + ddlYear.SelectedValue;
                                    dRow[Convert.ToString(i)] = attendBl.GetLeaveAbbr( Convert.ToInt32(emp.EmpID), Convert.ToDateTime(leavedate), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                }
                                else
                                {
                                    if (dTable.Rows[0][Convert.ToString(i)].Equals("Sun"))
                                        dRow[Convert.ToString(i)] = "";
                                    else
                                        dRow[Convert.ToString(i)] = "P";
                                }
                            }
                        }
                        else if (emp.DOJ.Year <= rptDate.Year && rptDate.Month < Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month)
                        {
                            if (listLeaveDays.Contains(i))
                            {
                                string leavedate = ddlMonth.SelectedValue + "-" + i.ToString() + "-" + ddlYear.SelectedValue;
                                dRow[Convert.ToString(i)] = attendBl.GetLeaveAbbr( Convert.ToInt32(emp.EmpID), Convert.ToDateTime(leavedate), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            else
                            {
                                if (dTable.Rows[0][Convert.ToString(i)].Equals("Sun"))
                                    dRow[Convert.ToString(i)] = "";
                                else
                                    dRow[Convert.ToString(i)] = "P";
                            }
                        }
                        else
                        {
                            dRow[Convert.ToString(i)] = "-";
                        }
                    }
                }

                dTable.Rows.Add(dRow);
            }
        }

        public void GetInitials()
        {
            //Add Columns
            dTable.Columns.Add("Region", typeof(string));
            dTable.Columns.Add("Department", typeof(string));
            dTable.Columns.Add("Division", typeof(string));
            dTable.Columns.Add("Designation", typeof(string));
            dTable.Columns.Add("EmpID", typeof(string));
            dTable.Columns.Add("EmpCode", typeof(string));
            dTable.Columns.Add("Name", typeof(string));
            
            for (int i = 1; i <= DaysInMonth; i++)
            {
                dTable.Columns.Add(Convert.ToString(i), typeof(string));
            }
            
            //Add First Row
            DataRow dRow = dTable.NewRow();
            dRow["Region"] = "";
            dRow["Department"] = "";
            dRow["Division"] = "";
            dRow["Designation"] = "";
            dRow["EmpID"] = "";
            dRow["EmpCode"] = "";
            dRow["Name"] = "";

            for (int i = 1; i <= DaysInMonth; i++)
            {
                dRow[Convert.ToString(i)] = new DateTime(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), i).ToString("ddd");
            }
            dTable.Rows.Add(dRow);
        }

        #endregion
    }
}

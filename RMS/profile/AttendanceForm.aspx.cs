using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.profile
{
    public partial class AttendanceForm : System.Web.UI.Page
    {

        RMSDataContext db = new RMSDataContext();
        tblPlEmpAttance att = new tblPlEmpAttance();
        AttendanceBL attBL = new AttendanceBL();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
#pragma warning disable CS0114 // 'AttendanceForm.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'AttendanceForm.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "attendancescreen").ToString();
                //FillSearchDropDownEmployee();
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
                FillJobeTypeDrpdown();
                FillSearchDropDownEmployee();
                BindGridAttendance(BranchID);
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID =Convert.ToInt32(searchBranchDropDown.SelectedValue);
                
                

            }
        }


        protected void Onclick_Save(object sender, EventArgs e)
        {
            try
            {
                if (ID == 0)
                {
                    if (searchBranchDropDown.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Branch", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        att.BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    }
                    if (ddlJobType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Job Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        att.JobNameID = Convert.ToInt32(ddlJobType.SelectedValue.Trim());
                    }
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        att.EmpID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue.Trim());
                    }
                    if (txtMonth.Text == "")
                    {
                        ucMessage.ShowMessage("Please Select Month", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        att.month = txtMonth.Text.Trim();
                    }
                    if (txtPrestDays.Text == "")
                    {
                        ucMessage.ShowMessage("Please Insert Present Days", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        att.PresentDays = Convert.ToInt32(txtPrestDays.Text.Trim());
                    }
                    if (txtLeaveDays.Text == "")
                    {
                        ucMessage.ShowMessage("Please Insert Leave Days", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        att.LeaveDays = Convert.ToInt32(txtLeaveDays.Text.Trim());
                    }

                    db.tblPlEmpAttances.InsertOnSubmit(att);
                    db.SubmitChanges();

                    ucMessage.ShowMessage("Save Successfully", RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    tblPlEmpAttance cont = db.tblPlEmpAttances.Where(x => x.AttID == ID).FirstOrDefault();
                    if (searchBranchDropDown.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Branch", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cont.BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    }
                    if (ddlJobType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Job Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cont.JobNameID = Convert.ToInt32(ddlJobType.SelectedValue.Trim());
                    }
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cont.EmpID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue.Trim());
                    }
                    if (txtMonth.Text == "")
                    {
                        ucMessage.ShowMessage("Please Select Month", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cont.month = txtMonth.Text.Trim();
                    }
                    if (txtPrestDays.Text == "")
                    {
                        ucMessage.ShowMessage("Please Insert Present Days", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cont.PresentDays = Convert.ToInt32(txtPrestDays.Text.Trim());
                    }
                    if (txtLeaveDays.Text == "")
                    {
                        ucMessage.ShowMessage("Please Insert Leave Days", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cont.LeaveDays = Convert.ToInt32(txtLeaveDays.Text.Trim());
                    }

                    db.SubmitChanges();
                    //attBL.Update(att, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucMessage.ShowMessage("Updateded Successfully", RMS.BL.Enums.MessageType.Info);
                }
                BindGridAttendance(BranchID);
                ClearFields();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void Onclick_Clear(object sender, EventArgs e)
        {

        }
        



        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                     BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    RMSDataContext db = new RMSDataContext();
                    ddlEmployeeSearch.Controls.Clear();
                    ddlEmployeeSearch.DataTextField = "FullName";
                    ddlEmployeeSearch.DataValueField = "EmpID";
                    ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
                    ddlEmployeeSearch.DataBind();
                    ddlEmployeeSearch.Items.Insert(0, new ListItem("Select Employee", "0"));
                    BindGridAttendance(BranchID);
                    //FillSearchDropDownEmployee();
                }
            }
            catch
            { }
        }
        protected void ddlJobtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlJobType.SelectedValue.Equals("0"))
                {
                    int jobtype = Convert.ToInt32(ddlJobType.SelectedValue.Trim());
                    int brr = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                    //RMSDataContext db = new RMSDataContext();
                    ddlEmployeeSearch.Controls.Clear();
                    ddlEmployeeSearch.DataTextField = "FullName";
                    ddlEmployeeSearch.DataValueField = "EmpID";
                    ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.JobNameID == jobtype && x.BranchID == brr).ToList();
                    ddlEmployeeSearch.DataBind();
                    ddlEmployeeSearch.Items.Insert(0, new ListItem("Select Employee", "0"));
                    //FillSearchDropDownEmployee();

                    //ddlJobType.SelectedValue = "0";
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }


        protected void grdEduEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdAttendance.SelectedValue);
            tblPlEmpAttance atten = new tblPlEmpAttance();
            atten = db.tblPlEmpAttances.Where(x => x.AttID == ID).FirstOrDefault();
            searchBranchDropDown.SelectedValue = atten.BranchID.ToString();
            ddlJobType.SelectedValue = atten.JobNameID.ToString();
            ddlEmployeeSearch.SelectedValue = atten.EmpID.ToString();
            txtMonth.Text = atten.month;
            txtPrestDays.Text = atten.PresentDays.ToString();
            txtLeaveDays.Text = atten.LeaveDays.ToString();
        }

        protected void grdEduEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAttendance.PageIndex = e.NewPageIndex;
            BindGridAttendance(BranchID);
        }


        protected void BindGridAttendance(int br)
        {

            this.grdAttendance.DataSource = from att in db.tblPlEmpAttances
                                            where att.BranchID == br
                                            select new
                                            {
                                                att.AttID,
                                                att.EmpID,
                                                att.Branch.br_nme,
                                                att.tblPlEmpData.FullName,
                                                att.month,
                                                att.PresentDays,
                                                att.LeaveDays
                                            };
            this.grdAttendance.DataBind();
        }


        protected void FillSearchDropDownEmployee()
        {

            ddlEmployeeSearch.DataTextField = "FullName";
            ddlEmployeeSearch.DataValueField = "EmpID";
            ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1  && x.BranchID == BranchID && x.JobNameID == 6).ToList();
            ddlEmployeeSearch.DataBind();
            ddlEmployeeSearch.Items.Insert(0, new ListItem("Select Employee", "0"));
            //RMSDataContext db = new RMSDataContext();

            //this.ddlEmployeeSearch.DataTextField = "FullName";
            //ddlEmployeeSearch.DataValueField = "EmpID";

            //ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.BranchID == BranchID).ToList();
            //ddlEmployeeSearch.DataBind();
            //ddlEmployeeSearch.Items.Insert(0, new ListItem("Select Employee", "0"));
            //ddlEmployeeSearch.Controls.Clear();
            //ddlEmployeeSearch.Dispose();


        }

        private void FillJobeTypeDrpdown()
        {
            ddlJobType.DataTextField = "JobTypeName1";
            ddlJobType.DataValueField = "JobNameID";
            ddlJobType.DataSource = db.JobTypeNames.Where(x => x.IsActive == true && x.JobNameID == 6).ToList();
            ddlJobType.DataBind();
            //ddlJobType.Items.Insert(0, new ListItem("Select Job Type", "0"));
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


        protected void ClearFields()
        {
            ID = 0;
            txtLeaveDays.Text = "";
            txtMonth.Text = "";
            txtPrestDays.Text = "";
            ddlEmployeeSearch.SelectedValue = "0";
            searchBranchDropDown.SelectedValue = "0";
            ddlJobType.SelectedValue = "0";
        }
    }
}
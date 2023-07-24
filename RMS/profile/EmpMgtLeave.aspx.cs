using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;

namespace RMS.Setup
{
    public partial class EmpMgtLeave : BasePage
    {

        #region DataMembers
        //RMS.BL.tblAppEmp usr;
        RMSDataContext db = new RMSDataContext();
        GroupBL groupManager = new GroupBL();
        PlLeaveBL mgtleave = new PlLeaveBL();
        //PlAllowBL allowBL = new PlAllowBL();
        EmpBL empBL = new EmpBL();
        GroupBL grpBl = new GroupBL();
        CompanyBL compBl = new CompanyBL();

        //ListItem selList = new ListItem();
        //ListItem selListSub = new ListItem();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'EmpMgtLeave.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpMgtLeave.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public int BlncID
        {
            get { return (ViewState["BlncID"] == null) ? 0 : Convert.ToInt32(ViewState["BlncID"]); }
            set { ViewState["BlncID"] = value; }
        }

        public string ActionStr
        {
            get { return (ViewState["ActionStr"] == null) ? "Insert" : Convert.ToString(ViewState["ActionStr"]); }
            set { ViewState["ActionStr"] = value; }
        }
        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

        public int GroupID
        {
            get { return (ViewState["GroupID"] == null) ? 0 : Convert.ToInt32(ViewState["GroupID"]); }
            set { ViewState["GroupID"] = value; }
        }
        public int PID
        {
            get { return (ViewState["PID"] == null) ? 0 : Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

        public DateTime LeaveDate
        {
            get { return (ViewState["LeaveDate"] == null) ? new DateTime() : Convert.ToDateTime(ViewState["LeaveDate"]); }
            set { ViewState["LeaveDate"] = value; }
        }
        public string CurPayPeriod
        {
            get { return (ViewState["CurPayPeriod"] == null) ? "" : Convert.ToString(ViewState["CurPayPeriod"]); }
            set { ViewState["CurPayPeriod"] = value; }

        }

        public bool CanApprove
        {
            get { return Convert.ToBoolean(ViewState["CanApprove"]); }
            set { ViewState["CanApprove"] = value; }

        }
        public bool CanEnter
        {
            get { return Convert.ToBoolean(ViewState["CanEnter"]); }
            set { ViewState["CanEnter"] = value; }

        }
        public bool AppCycle
        {
            get { return Convert.ToBoolean(ViewState["AppCycle"]); }
            set { ViewState["AppCycle"] = value; }

        }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpLeaveSetup").ToString();
               // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                if (Session["DateFormat"] == null)
                {
                    txtStartDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtEndDateCal.Format = Request.Cookies["uzr"]["DateFormat"];

                }
                else
                {
                    txtStartDateCal.Format = Session["DateFormat"].ToString();
                    txtEndDateCal.Format = Session["DateFormat"].ToString();
                }

                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }

                if (Session["CompID"] == null)
                {
                    CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                }
                else
                {
                    CompID = Convert.ToByte(Session["CompID"].ToString());
                }
                if (Session["CurPayPeriod"] == null)
                {
                    CurPayPeriod = Request.Cookies["uzr"]["CurPayPeriod"];
                }
                else
                {
                    CurPayPeriod = Session["CurPayPeriod"].ToString();
                }
                btnDelete.Visible = false;
                FillDropDownLeaveType();
                FillDropDownEmployee();
                FillDropDownLeaveTypeSelect();
                FillDropdownPersonal();
                //BindGrid();
                //BindGridLeaveBlnc();

                try
                {
                    AppCycle = Convert.ToBoolean(compBl.GetByID(CompID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).AppCycle);
                }
                catch 
                {
                    AppCycle = false;
                }

                if (AppCycle)
                {
                    //Maintaning Privilage Status==========================
                    if (Session["GroupID"] == null)
                    {
                        GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                    }
                    else
                    {
                        GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                    }

                    PID = Convert.ToInt32(Request.QueryString["PID"]);

                    tblAppPrivilage appPrivilage = grpBl.GetPrivilageStatus(GroupID, PID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    if (appPrivilage != null)
                    {
                        if (appPrivilage.CanEdit.Equals(true))//approval status given
                        {
                            CanApprove = true;
                        }
                        else
                        {
                            CanApprove = true;
                        }
                        if (appPrivilage.CanAdd.Equals(true))
                        {
                            CanEnter = true;
                        }
                        else
                        {
                            CanEnter = false;
                        }

                        if (CanApprove && !CanEnter)
                        {
                            //ListItem itemPending = ddlStatus.Items.FindByText("Pending");
                            //ddlStatus.Items.Remove(itemPending);
                        }

                        if (CanApprove && CanEnter)
                        {
                            //ListItem itemApproved = ddlStatus.Items.FindByText("Approved");
                            //ddlStatus.Items.Remove(itemApproved);
                        }

                    }
                    else
                    {
                        ucMessage.ShowMessage("Enter privilages for this page for the logged in group", RMS.BL.Enums.MessageType.Error);
                    }
                    //====================================================================
                }
                else
                {
                   // grdLeaveStatus.Visible = false;
                    grdLeave.Columns[4].Visible = false;
                    //lblStatus.Visible = false;
                    //ddlStatus.Visible = false;
                    //reqVal_ddlStatus.Enabled = false;
                }

                //ucButtons.ValidationGroupName = "main";
               // EmpSrchUC.Focus();
            }
            //BindGrid();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            //if (EmpSrchUC.EmpBindGrid.Equals("Yes"))
            //{
            //    ClearFields();
            //    EmpSrchUC.EmpBindGrid = "No";
            //    BindGrid();
            //    ID = EmpSrchUC.EmpIDUC;
            //    BindGridLeaveStatus(ID);
            //}
            base.OnLoadComplete(e);
        }

        protected void grdLeave_SelectedIndexChanged(object sender, EventArgs e)
        {
            RMSDataContext db = new RMSDataContext();
            ID = Convert.ToInt32(grdLeave.SelectedDataKey.Values["LeaveID"].ToString());
            //CompID = Convert.ToInt32(grdLeave.SelectedDataKey.Values["CompID"].ToString());
            //LeaveDate = Convert.ToDateTime(grdLeave.SelectedDataKey.Values["LeaveDate"].ToString());
            // this.GetByID();
            tblPlLeave pl = new tblPlLeave();
            pl = db.tblPlLeaves.Where(x => x.LeaveID == ID).FirstOrDefault();
            this.ddlEmployee.SelectedValue = pl.EmpID.ToString();
            if (Session["DateFullYearFormat"] == null)
            {
                this.txtStartDate.Text = pl.StartDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            else
            {
                this.txtStartDate.Text = pl.StartDate.Value.ToString(Session["DateFullYearFormat"].ToString());
            }
            if (Session["DateFullYearFormat"] == null)
            {
                this.txtEndDate.Text = pl.EndDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            else
            {
                this.txtEndDate.Text = pl.EndDate.Value.ToString(Session["DateFullYearFormat"].ToString());
            }
            //this.txtStartDate.Text = pl.StartDate.ToString();
            //this.txtEndDate.Text = pl.EndDate.ToString();
            if (pl.Remarks== null || pl.Remarks == "")
            {
                txtRemarks.Text = "";
            }
            else
            {
                this.txtRemarks.Text = pl.Remarks.ToString();
            }
            if (pl.LeaveTypeID == null )
            {
                ddlleaveType.SelectedValue = "0";
            }
            else
            {
                this.ddlleaveType.SelectedValue = pl.LeaveTypeID.ToString();
            }
            if (pl.Status == null || pl.Status == "")
            {
                ddlStatus.SelectedValue = "0";
            }
            else
            {
                this.ddlStatus.SelectedValue = pl.Status.ToString();
            }
            

        }

        protected void grdLeave_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLeave.PageIndex = e.NewPageIndex;
            BindGrid();
            
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                
            }
            else if (e.CommandName == "Save")
            {
                if (Convert.ToInt32(ddlEmployee.SelectedValue) < 1)
                {
                    ucMessage.ShowMessage("Please select Employee", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                try
                {
                    Convert.ToDateTime(txtStartDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage("Invalid start date", RMS.BL.Enums.MessageType.Error);
                    return;
                }

                try
                {
                    Convert.ToDateTime(txtEndDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage("Invalid end date", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                
                if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
                {
                    if (Convert.ToDateTime(txtStartDate.Text.Trim()).Date != Convert.ToDateTime(txtEndDate.Text.Trim()).Date)
                    {
                        ucMessage.ShowMessage("Start and end date should be same for half leave", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                }



                string mn = "";
                string yr = "";

                if (Session["CurPayPeriod"] == null)
                {
                    string dt = Request.Cookies["uzr"]["CurPayPeriod"].ToString();
                    yr = dt.Substring(0, 4);
                    mn = dt.Substring(4, 2);
                }
                else
                {
                    string dt = Session["CurPayPeriod"].ToString();
                    yr = dt.Substring(0, 4);
                    mn = dt.Substring(4, 2);
                }

                DateTime PayrollProcessDate = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 1);

                if (Convert.ToDateTime(txtStartDate.Text.Trim()).Date >= PayrollProcessDate)
                {

                    if (ActionStr.Equals("Insert"))
                    {
                        this.Insert();
                        BindGrid();
                       // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    }
                    else
                    {
                        this.Update();
                       // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    }
                    BindGridLeaveStatus(ID);
                }
                else
                {
                    ucMessage.ShowMessage("Start date should be greater than payroll process date i.e. "+PayrollProcessDate.ToString("dd-MMM-yyyy")+".", RMS.BL.Enums.MessageType.Error);
                    return;
                }
            }
            else if (e.CommandName == "Delete")
            {

                try
                {
                    this.Delete();
                 
                   // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletionDependency").ToString(), RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        Session["errors"] = ex.Message;
                        Response.Redirect("~/home/Error.aspx");
                    }
                }

                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                //pnlMain.Enabled = true;
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }

        protected void grdLeave_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["DateFormat"] == null)
                {
                    
                    if (!e.Row.Cells[2].Text.Equals("&nbsp;"))
                    {
                        e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
                    }
                    if (!e.Row.Cells[3].Text.Equals("&nbsp;"))
                    {
                        e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
                    }
                }
                else
                {
                    
                    if (!e.Row.Cells[2].Text.Equals("&nbsp;"))
                    {
                        e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text.ToString()).ToString(Session["DateFormat"].ToString());
                    }
                    if (!e.Row.Cells[3].Text.Equals("&nbsp;"))
                    {
                        e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
                    }
                }
                
                    if (e.Row.Cells[5].Text == "A")
                    {
                        e.Row.Cells[5].Text = "Approved";
                    }
                    else
                    {
                        e.Row.Cells[5].Text = "Pending";
                    }
                    
               
            }
        }

        protected void grdLeaveStatus_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    if (Convert.ToDecimal(e.Row.Cells[6].Text) < 0)
                    {
                        e.Row.Cells[6].BackColor = System.Drawing.Color.Red;
                    }
                    if (Convert.ToDecimal(e.Row.Cells[7].Text) < 0)
                    {
                        e.Row.Cells[7].BackColor = System.Drawing.Color.Red;
                    }
                    if (Convert.ToDecimal(e.Row.Cells[8].Text) < 0)
                    {
                        e.Row.Cells[8].BackColor = System.Drawing.Color.Red;
                    }
                }
                catch { }
            }
        }

        protected void Search_Grid(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue == "0")
            {
                ucMessage.ShowMessage("Select Employee", BL.Enums.MessageType.Error);
                return;
            }
            else
            {
                BindGrid();
                BindGridLeaveBlnc();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Delete())
                {
                   // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
                    BindGrid();
                    BindGridLeaveStatus(ID);
                    ClearFields();
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeleteException").ToString(), RMS.BL.Enums.MessageType.Error);
                }
            }
            catch { }
 
        }

        protected void btnBalanceLeave_Save(object sender, EventArgs e)
        {
            using(RMSDataContext db = new RMSDataContext())
            {
                //BlncID = Convert.ToInt32(ddlEmployee.SelectedValue);
                
                if (ID == 0)
                {
                    int empIDD = Convert.ToInt32(ddlEmployee.SelectedValue);
                    int LeaID = Convert.ToInt32(ddlLeaeveTYpe.SelectedValue);
                    if (txtavailBlnc.Text == "" || txtavailBlnc.Text == null || txtavailBlnc.Text == "0")
                    {
                        tblPlLeaveBlnc blnc = new tblPlLeaveBlnc();
                        blnc.empID = empIDD;
                        blnc.LeaveTypeID = LeaID;
                        blnc.LeaveBlnc = Convert.ToInt32(txtentirler.Text);
                        db.tblPlLeaveBlncs.InsertOnSubmit(blnc);
                        db.SubmitChanges();
                    }
                    else
                    {
                        //int avail = Convert.ToInt32(txtavailBlnc.Text);
                        //tblPlLeaveBlnc bl = db.tblPlLeaveBlncs.Where(x => x.empID == empIDD && x.LeaveTypeID == LeaID).FirstOrDefault();
                        //int bla = Convert.ToInt32(bl.LeaveBlnc) - avail;
                        //bl.LeaveBlnc = bla;
                        //db.SubmitChanges();
                    }
                }
                else
                {
                    tblPlLeaveBlnc lebl = db.tblPlLeaveBlncs.Where(x => x.LeaID == ID).FirstOrDefault();
                    lebl.empID = Convert.ToInt32(ddlEmployee.SelectedValue);
                    lebl.LeaveTypeID = Convert.ToInt32(ddlLeaeveTYpe.SelectedValue);
                    lebl.LeaveBlnc = Convert.ToInt32(txtentirler.Text);
                    db.SubmitChanges();
                }
                

                

                
                BindGridLeaveBlnc();
                ClearFields();
            }
            
        }

        protected void grdBlnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            using(RMSDataContext db = new RMSDataContext())
            {
                ID = Convert.ToInt32(grdBlnc.SelectedValue);
                tblPlLeaveBlnc leablnc = db.tblPlLeaveBlncs.Where(x => x.LeaID == ID).FirstOrDefault();
                if (leablnc.LeaveTypeID == null)
                {
                    ddlLeaeveTYpe.SelectedValue = "0";
                }
                else
                {
                    ddlLeaeveTYpe.SelectedValue = leablnc.LeaveTypeID.ToString();
                }
                if (leablnc.LeaveBlnc == null)
                {
                    txtentirler.Text = "0";
                }
                else
                {
                    txtentirler.Text = leablnc.LeaveBlnc.ToString();
                }

            }
        }

        protected void searchemps_changeIndex(object sender, EventArgs e)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
                int emppp = Convert.ToInt32(ddlEmployee.SelectedValue);
                string empref = db.tblPlEmpDatas.Where(x => x.EmpID == emppp).FirstOrDefault().EmpCode;
                ddlperson.SelectedValue = empref;
               // FillDropdowntitleLiti(emppp);
               // FillDropdowntitleEnq(emppp);
            }
        }


        protected void ddlPersonal_change(object sender, EventArgs e)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
                string emppp = ddlperson.SelectedValue;
                int empref = db.tblPlEmpDatas.Where(x => x.EmpCode == emppp).FirstOrDefault().EmpID;
                ddlEmployee.SelectedValue = empref.ToString();
            }
        }


        protected void searchemp_changeIndex(object sender, EventArgs e)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
                int empID;
                int leaty;
                if (ddlEmployee.SelectedValue == null || ddlEmployee.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                     empID = Convert.ToInt32(ddlEmployee.SelectedValue);
                }
                if (ddlLeaeveTYpe.SelectedValue == null || ddlLeaeveTYpe.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please Select Leave Type", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                     leaty = Convert.ToInt32(ddlLeaeveTYpe.SelectedValue);
                }
                
                tblPlLeaveBlnc entir = db.tblPlLeaveBlncs.Where(x => x.LeaveTypeID == leaty && x.empID == empID).FirstOrDefault();
                if (entir == null)
                {
                    txtentirler.Text = "";
                }
                else
                {
                    txtentirler.Text = entir.LeaveBlnc.ToString();
                }
                
            }
        }

        protected void grdBlnc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBlnc.PageIndex = e.NewPageIndex;
            BindGridLeaveBlnc();
        }

        protected void grdBlnc_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        #endregion

        #region Helping Method

        protected void BindGrid()
        {
            int emppp = Convert.ToInt32(ddlEmployee.SelectedValue);
            RMSDataContext db = new RMSDataContext();
            //this.grdLeave.DataSource = mgtleave.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //this.grdLeave.DataBind();
            grdLeave.DataSource = from emp in db.tblPlLeaves
                                  join ty in db.tblPlLeaveTypes on emp.LeaveTypeID equals ty.LeaveTypeID
                                  where emp.EmpID == emppp
                                  select new
                                  {
                                      emp.LeaveID,
                                      endDate = emp.EndDate,
                                      EmpID = emp.EmpID,
                                     // CompID = emp.CompID,
                                      fName = emp.tblPlEmpData.FullName,
                                      //LeaveDate = emp.LeaveDate,
                                      start = emp.StartDate,
                                      end = emp.EndDate,
                                      
                                      LeaveTypeID = ty.LeaveTypeDesc,
                                      LeaveDays = emp.LeaveDays,
                                      Remarks = emp.Remarks,
                                      Status = emp.Status
                                      
                                  };
            grdLeave.DataBind();


        }
        
        protected void BindGridLeaveStatus(int Cid)
        {
            if (AppCycle)
            {
                DateTime date = Convert.ToDateTime("12-31-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date.Year);

                //this.grdLeaveStatus.DataSource = mgtleave.GetEmpLeaveStatus(CompID, Cid, date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //this.grdLeaveStatus.DataBind();
            }
        }

        protected void BindGridLeaveBlnc()
        {
            int empp = Convert.ToInt32(ddlEmployee.SelectedValue);
                grdBlnc.DataSource = from blc in db.tblPlLeaveBlncs
                                     join emp in db.tblPlEmpDatas on blc.empID equals emp.EmpID
                                     join leav in db.tblPlLeaveTypes on blc.LeaveTypeID equals leav.LeaveTypeID
                                     where blc.empID == empp
                                     select new
                                     {
                                         blc.empID,
                                         blc.LeaID,
                                         blc.LeaveBlnc,
                                         emp.FullName,
                                         leav.LeaveTypeDesc
                                     };
                grdBlnc.DataBind();
        }
       
        private void FillDropDownLeaveType()
        {
            ddlleaveType.DataTextField = "LeaveTypeDesc";
            ddlleaveType.DataValueField = "leaveTypeID";
            ddlleaveType.DataSource = mgtleave.GetAllLeaveTypeCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlleaveType.DataBind();
        }

        private void FillDropDownLeaveTypeSelect()
        {
            ddlLeaeveTYpe.DataTextField = "LeaveTypeDesc";
            ddlLeaeveTYpe.DataValueField = "leaveTypeID";
            ddlLeaeveTYpe.DataSource = mgtleave.GetAllLeaveTypeCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLeaeveTYpe.DataBind();
        }

        protected void GetByID()
        {
            
            tblPlLeave leav = mgtleave.GetByID(ID, LeaveDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlleaveType.SelectedValue = leav.LeaveTypeID.ToString();
            //if
            //{
            //    this.txtStartDate.Text = leav.StartDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            //}
            //else
            //{
            //    this.txtStartDate.Text = leav.StartDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            //}
            //txtStartDate.Text = leav.StartDate.ToString();
            if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
            {

                txtEndDate.Text = leav.EndDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            else
            {
                txtEndDate.Text = leav.EndDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                // DateTime EndDate = Convert.ToDateTime(leav.EndDate);
                //// EndDate = EndDate.AddDays(Convert.ToDouble(leav.LeaveDays) - 1);
                // txtEndDate.Text = EndDate.ToString("dd-MMM-yy");
            }
            //txtStartDate.Enabled = true;
            //txtStartDate.ReadOnly = true;
            if (AppCycle)
            {
                if (leav.Status != null)
                {
                    ddlStatus.SelectedValue = leav.Status.ToString();
                }
            }

            txtRemarks.Text = leav.Remarks;

            int payPeriod = 0;
            int.TryParse(CurPayPeriod, out payPeriod);
            int prd = 0;
            int.TryParse(Convert.ToDateTime(txtStartDate.Text).ToString("yyyyMM"), out prd);

            if (prd >= payPeriod)
            {
                btnDelete.Visible = true;
            }

            //try
            //{
            //    EmpSrchUC.EditModeDataShow(leav.tblPlEmpData.FullName, "EN-" + leav.EmpID, leav.tblPlEmpData.EmpCode, leav.tblPlEmpData.tblPlCode1.CodeDesc, leav.tblPlEmpData.tblPlCode.CodeDesc);
            //}
            //catch { }
            //EmpSrchUC.EmpIDUC = ID;
           // ActionStr = "Update";
            
          // ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }
        protected void FillDropdownPersonal()
        {
            RMSDataContext db = new RMSDataContext();
            ddlperson.DataTextField = "EmpCode";
            ddlperson.DataValueField = "EmpCode";
            if (BranchID == 1)
            {
                ddlperson.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID != 14 && x.BranchID != null).ToList();
            }
            else
            {
                ddlperson.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
            }

            ddlperson.DataBind();
            ddlperson.Items.Insert(0, new ListItem("Select", "0"));
        }

        private void FillDropDownEmployee()
        {
            RMSDataContext db = new RMSDataContext();
            //int id = Convert.ToInt32(ddlEmployee.SelectedValue);

            this.ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataValueField = "EmpID";
            if (BranchID == 1)
            {
            ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID != 14 && x.BranchID != null).ToList().OrderBy(x => x.FullName);
            }
            else
            {
                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList().OrderBy(x => x.FullName);
            }
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("---Select Employee---", "0"));
        }

            protected void Update()
        {
            if (ddlleaveType.SelectedValue != "0")
            {
                RMSDataContext db = new RMSDataContext();
                tblPlLeave leav;
                DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                DateTime tempStart = StartDate;
                DateTime tempEnd = EndDate;
                if (Convert.ToDateTime(txtEndDate.Text.Trim()) >= Convert.ToDateTime(txtStartDate.Text.Trim()))
                {
                    if (Convert.ToDateTime(txtEndDate.Text.Trim()).Month == Convert.ToDateTime(txtStartDate.Text.Trim()).Month)
                    {
                         leav = mgtleave.GetByID(ID, LeaveDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        // leav = db.tblPlLeaves.Where(x => x.EmpID == ID).FirstOrDefault();
                        if (leav != null)
                        {
                           // leav.CompID = leav.CompID;
                            leav.EmpID = ID;
                            leav.StartDate = StartDate;
                            int day = ((EndDate - StartDate).Days + 1);
                            leav.LeaveDays = Convert.ToDecimal(day);
                            leav.Remarks = txtRemarks.Text;
                            leav.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                            leav.Status = ddlStatus.SelectedValue;
                            // temp.LeaveTypeID = leav.LeaveTypeID;



                            if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
                            {
                                leav.LeaveDays = Convert.ToDecimal(0.5);
                            }
                            else
                            {
                                int dtDiff = (Convert.ToDateTime(txtEndDate.Text.Trim()) - Convert.ToDateTime(txtStartDate.Text.Trim())).Days + 1;
                                leav.LeaveDays = Convert.ToDecimal(dtDiff);
                            }
                            
                            leav.UpdateBy = Session["UserID"].ToString();
                            leav.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            leav.Remarks = txtRemarks.Text;

                            //  db.SubmitChanges();

                            mgtleave.Update(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                            BindGrid();
                        }
                        else
                        {
                           // leav.CompID = leav.CompID;
                            leav.EmpID = ID;
                            leav.StartDate = StartDate;
                            int day = ((EndDate - StartDate).Days + 1);
                            leav.LeaveDays = Convert.ToDecimal(day);
                            leav.Remarks = txtRemarks.Text;
                            leav.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                            leav.Status = ddlStatus.SelectedValue;
                            // temp.LeaveTypeID = leav.LeaveTypeID;



                            if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
                            {
                                leav.LeaveDays = Convert.ToDecimal(0.5);
                            }
                            else
                            {
                                int dtDiff = (Convert.ToDateTime(txtEndDate.Text.Trim()) - Convert.ToDateTime(txtStartDate.Text.Trim())).Days + 1;
                                leav.LeaveDays = Convert.ToDecimal(dtDiff);
                            }

                            leav.UpdateBy = Session["UserID"].ToString();
                            leav.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            leav.Remarks = txtRemarks.Text;

                            //  db.SubmitChanges();

                            mgtleave.Insert(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                            BindGrid();
                        }
                        

                        //if (!mgtleave.ISAlreadyExist_ForUpdate(temp, leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                        //{
                        //    leav.LeaveTypeID = temp.LeaveTypeID;
                        //    leav.LeaveDays = temp.LeaveDays;

                        //    if (AppCycle)
                        //    {
                        //        //if (leav.LeaveTypeID.Value.Equals(1))
                        //        //{
                        //        //    //if (leav.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[6].Text))
                        //        //    //{
                        //        //    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining casual leaves", RMS.BL.Enums.MessageType.Error);
                        //        //    //    //leav = null;
                        //        //    //    return;
                        //        //    //}
                        //        //}
                        //        //if (leav.LeaveTypeID.Value.Equals(2))
                        //        //{
                        //        //    //if (leav.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[7].Text))
                        //        //    //{
                        //        //    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining medical leaves", RMS.BL.Enums.MessageType.Error);
                        //        //    //    //leav = null;
                        //        //    //    return;
                        //        //    //}
                        //        //}
                        //        //if (leav.LeaveTypeID.Value.Equals(3))
                        //        //{
                        //        //    //if (leav.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[8].Text))
                        //        //    //{
                        //        //    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining annual leaves", RMS.BL.Enums.MessageType.Error);
                        //        //    //    //leav = null;
                        //        //    //    return;
                        //        //    //}
                        //        //}
                        //        if (CanApprove && CanEnter)
                        //        {
                        //            if (Session["UserID"] == null)
                        //                temp.ReqBy = Request.Cookies["uzr"]["UserID"].ToString();
                        //            else
                        //                temp.ReqBy = Session["UserID"].ToString();
                        //            temp.ReqDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //        }

                        //        if (ddlStatus.SelectedValue.Equals("A"))
                        //        {
                        //            if (CanApprove && !CanEnter)
                        //            {
                        //                if (Session["UserID"] == null)
                        //                    temp.AppBy = Request.Cookies["uzr"]["UserID"].ToString();
                        //                else
                        //                    temp.AppBy = Session["UserID"].ToString();
                        //                temp.AppDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //            }
                        //        }
                        //        temp.Status = Convert.ToString(ddlStatus.SelectedValue);
                        //    }
                        //    if (Session["UserID"] == null)
                        //        temp.UpdateBy = Request.Cookies["uzr"]["UserID"].ToString();
                        //    else
                        //        temp.UpdateBy = Session["UserID"].ToString();
                        //    temp.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //    temp.Remarks = txtRemarks.Text;
                           
                        //    mgtleave.Update(temp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                        //    BindGrid();

                        //    ClearFields();
                        //}
                        //else
                        //{
                        //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EmpAlreadyTakenLeave").ToString(), RMS.BL.Enums.MessageType.Error);

                        //}
                    }
                    else
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updatedError").ToString(), RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EnterValidDates").ToString(), RMS.BL.Enums.MessageType.Error);
                }

            }
            txtStartDate.Enabled = true;
        }

        protected bool Delete()
        {
            tblPlLeave tblLeave = new tblPlLeave();
            //tblLeave.CompID = Convert.ToByte(CompID);
            tblLeave.EmpID = ID;
            tblLeave.StartDate = Convert.ToDateTime(txtStartDate.Text);
            return mgtleave.DeleteLeave(tblLeave, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void btnSave_Save(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                if (ddlleaveType.SelectedValue != "0")
                {
                    DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                    DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                    DateTime tempStart = StartDate;
                    DateTime tempEnd = EndDate;
                    int empcheck = Convert.ToInt32(ddlEmployee.SelectedValue);
                   

                    if (Convert.ToDateTime(txtEndDate.Text.Trim()) >= Convert.ToDateTime(txtStartDate.Text.Trim()))
                    {
                        if (Convert.ToDateTime(txtEndDate.Text.Trim()).Month == Convert.ToDateTime(txtStartDate.Text.Trim()).Month)
                        {

                            tblPlLeave plLeave = new tblPlLeave();
                           
                            //plLeave.CompID = Convert.ToByte(CompID);

                            if (ddlEmployee.SelectedValue == "0")
                            {
                                ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                plLeave.EmpID = Convert.ToInt32(ddlEmployee.SelectedValue);
                            }
                            if (ddlleaveType.SelectedValue == "0")
                            {
                                ucMessage.ShowMessage("Please Select Leave Type", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                plLeave.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                            }
                            if (txtStartDate.Text == "")
                            {
                                ucMessage.ShowMessage("Start Date Is Required", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                plLeave.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                            }
                            if (txtEndDate.Text == "")
                            {
                                ucMessage.ShowMessage("End Date Is Required", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                plLeave.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                            }
                            if (txtRemarks.Text == "")
                            {
                                plLeave.Remarks = null;
                            }
                            else
                            {
                                plLeave.Remarks = txtRemarks.Text;
                            }
                            if (ddlStatus.SelectedValue == "0")
                            {
                                ucMessage.ShowMessage("Please Select Status", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                plLeave.Status = Convert.ToString(ddlStatus.SelectedValue);
                            }

                            TimeSpan tss1 = EndDate.Subtract(StartDate);
                            int Diff = tss1.Days + 1;
                            plLeave.LeaveDays = Diff;
                            
                            //plLeave.IsActive = Convert.ToBoolean(checkIsactive.Checked);

                            //if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
                            //{
                            //    if (Convert.ToDateTime(txtStartDate.Text.Trim()).Date != Convert.ToDateTime(txtEndDate.Text.Trim()).Date)
                            //    {
                            //        ucMessage.ShowMessage("Start and end date should be same for half leave", RMS.BL.Enums.MessageType.Error);
                            //        return;
                            //    }
                            //    else
                            //    {
                            //        plLeave.LeaveDays = Convert.ToDecimal(0.5);
                            //    }
                               
                            //}
                            //else
                            //{
                            //    int dtDiff = (Convert.ToDateTime(txtEndDate.Text.Trim()) - Convert.ToDateTime(txtStartDate.Text.Trim())).Days;
                            //    plLeave.LeaveDays = Convert.ToDecimal(dtDiff);
                            //}

                            if (AppCycle)
                            {
                                if (plLeave.LeaveTypeID.Value.Equals(1))
                                {
                                    //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[6].Text))
                                    //{
                                    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining casual leaves", RMS.BL.Enums.MessageType.Error);
                                    //    plLeave = null;
                                    //    return;
                                    //}
                                }
                                if (plLeave.LeaveTypeID.Value.Equals(2))
                                {
                                    //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[7].Text))
                                    //{
                                    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining medical leaves", RMS.BL.Enums.MessageType.Error);
                                    //    plLeave = null;
                                    //    return;
                                    //}
                                }
                                if (plLeave.LeaveTypeID.Value.Equals(3))
                                {
                                    //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[8].Text))
                                    //{
                                    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining annual leaves", RMS.BL.Enums.MessageType.Error);
                                    //    plLeave = null;
                                    //    return;
                                    //}
                                }
                                if (CanApprove && CanEnter)
                                {
                                    if (Session["UserID"] == null)
                                        plLeave.ReqBy = Request.Cookies["uzr"]["UserID"].ToString();
                                    else
                                        plLeave.ReqBy = Session["UserID"].ToString();
                                    plLeave.ReqDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                }
                                if (ddlStatus.SelectedValue.Equals("A"))
                                {
                                    if (CanApprove && !CanEnter)
                                    {
                                        if (Session["UserID"] == null)
                                            plLeave.AppBy = Request.Cookies["uzr"]["UserID"].ToString();
                                        else
                                            plLeave.AppBy = Session["UserID"].ToString();
                                        plLeave.AppDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    }
                                }
                               
                            }
                            if (Session["UserID"] == null)
                                plLeave.UpdateBy = Request.Cookies["uzr"]["UserID"].ToString();
                            else
                                plLeave.UpdateBy = Session["UserID"].ToString();
                            plLeave.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                            if (ddlStatus.SelectedValue == "A")
                            {
                                int em = Convert.ToInt32(ddlEmployee.SelectedValue);
                                int leadtt = Convert.ToInt32(ddlleaveType.SelectedValue);
                                DateTime startdat = Convert.ToDateTime(txtStartDate.Text);
                                DateTime endDa = Convert.ToDateTime(txtEndDate.Text);
                                TimeSpan tss = endDa.Subtract(startdat);
                                int dayy = tss.Days;
                                tblPlLeaveBlnc leavn = db.tblPlLeaveBlncs.Where(x => x.LeaveTypeID == leadtt && x.empID == em).FirstOrDefault();
                                if (leavn != null)
                                
                                {
                                    if (leavn.LeaveBlnc < 0)
                                    {
                                        int? ddiflea = leavn.LeaveBlnc + dayy;
                                        leavn.LeaveBlnc = ddiflea;
                                        db.SubmitChanges();
                                        BindGridLeaveBlnc();
                                    }
                                    else
                                    {
                                        int? ddiflea = leavn.LeaveBlnc - dayy;
                                        leavn.LeaveBlnc = ddiflea;
                                        db.SubmitChanges();
                                        BindGridLeaveBlnc();
                                    }
                                    
                                }
                                else
                                {
                                    ucMessage.ShowMessage("Leave Balance Not Found", BL.Enums.MessageType.Error);
                                    return;
                                }
                                
                            }
                            else
                            {
                                int em = Convert.ToInt32(ddlEmployee.SelectedValue);
                                int leadtt = Convert.ToInt32(ddlleaveType.SelectedValue);
                                tblPlLeaveBlnc leavn = db.tblPlLeaveBlncs.Where(x => x.LeaveTypeID == leadtt && x.empID == em).FirstOrDefault();
                                if (leavn == null)
                                {
                                    ucMessage.ShowMessage("Leave Balance Not Found", BL.Enums.MessageType.Error);
                                    return;
                                }
                            }

                            if (!mgtleave.ISAlreadyExist(plLeave, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                            {

                                mgtleave.Insert(plLeave, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                                BindGrid();
                                ClearFields();
                            }
                            else
                            {
                                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EmpAlreadyTakenLeave").ToString(), RMS.BL.Enums.MessageType.Error);

                            }
                        }
                        else
                        {
                            TimeSpan ts;
                            tblPlLeave leav;
                            int dtDiff;
                            while (tempStart < tempEnd)
                            {
                                //if (tempStart.Month + 1 >= 13)
                                //{
                                //    tempEnd = new DateTime(tempStart.Year + 1, 1, 1);
                                //}
                                //else
                                //{
                                //    tempEnd = new DateTime(tempStart.Year, tempStart.Month + 1, 1);
                                //}

                                //if (tempEnd > EndDate)
                                //{
                                //    tempEnd = EndDate;
                                //    tempEnd = tempEnd.AddDays(1.0);
                                //}
                                ts = tempEnd.Subtract(tempStart);

                                leav = new tblPlLeave();
                                //if (Session["CompID"] == null)
                                //{
                                //    leav = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                                //}
                                //else
                                //{
                                //    leav.CompID = Convert.ToByte(Session["CompID"].ToString());
                                //}

                                leav.EmpID = Convert.ToInt32(ddlEmployee.SelectedValue);

                                leav.StartDate = tempStart;
                                leav.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                                leav.EndDate = EndDate;
                                dtDiff = ts.Days + 1;
                                leav.Remarks = txtRemarks.Text;

                                leav.LeaveDays = Convert.ToDecimal(dtDiff);
                                leav.Status = ddlStatus.SelectedValue;

                                if (ddlStatus.SelectedValue == "A")
                                {
                                    int em = Convert.ToInt32(ddlEmployee.SelectedValue);
                                    int leadtt = Convert.ToInt32(ddlleaveType.SelectedValue);
                                    DateTime startdat = Convert.ToDateTime(txtStartDate.Text);
                                    DateTime endDa = Convert.ToDateTime(txtEndDate.Text);
                                    TimeSpan tss = endDa.Subtract(startdat);
                                    int dayy = tss.Days + 1;
                                    tblPlLeaveBlnc leavn = db.tblPlLeaveBlncs.Where(x => x.LeaveTypeID == leadtt && x.empID == em).FirstOrDefault();
                                    if (leavn != null)
                                    {
                                        int? ddiflea = leavn.LeaveBlnc - dayy;
                                        leavn.LeaveBlnc = ddiflea;
                                        db.SubmitChanges();
                                        BindGridLeaveBlnc();
                                    }
                                    else
                                    {
                                        ucMessage.ShowMessage("Leave Balance Not Found", BL.Enums.MessageType.Error);
                                        return;
                                    }

                                }


                                if (!mgtleave.ISAlreadyExist(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                                {

                                    mgtleave.Insert(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                                    BindGrid();

                                    ClearFields();
                                }
                                else
                                {
                                    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EmpAlreadyTakenLeave").ToString(), RMS.BL.Enums.MessageType.Error);
                                }
                                tempStart = tempEnd;
                                if (tempStart.Month + 1 >= 13)
                                {
                                    tempEnd = new DateTime(tempStart.Year + 1, 1, 1);
                                }
                                else
                                {
                                    tempEnd = new DateTime(tempStart.Year, tempStart.Month + 1, 1);
                                }
                                if (tempEnd > EndDate)
                                {
                                    tempEnd = EndDate;
                                    tempEnd = tempEnd.AddDays(1.0);
                                }
                            }
                        }
                    }
                    else
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EnterValidDates").ToString(), RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EnterLeaveType").ToString(), RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                
                    RMSDataContext db = new RMSDataContext();
                    tblPlLeave leav;
                    DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                    DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                    DateTime tempStart = StartDate;
                    DateTime tempEnd = EndDate;
                    if (Convert.ToDateTime(txtEndDate.Text.Trim()) >= Convert.ToDateTime(txtStartDate.Text.Trim()))
                    {
                    if (ddlStatus.SelectedValue == "A")
                    {
                        int em = Convert.ToInt32(ddlEmployee.SelectedValue);
                        int leadtt = Convert.ToInt32(ddlleaveType.SelectedValue);
                        DateTime startdat = Convert.ToDateTime(txtStartDate.Text);
                        DateTime endDa = Convert.ToDateTime(txtEndDate.Text);
                        TimeSpan tss = endDa.Subtract(startdat);
                        int dayy = tss.Days + 1;
                        tblPlLeaveBlnc leavn = db.tblPlLeaveBlncs.Where(x => x.LeaveTypeID == leadtt && x.empID == em).FirstOrDefault();
                        if (leavn != null)
                        {
                            tblPlLeave tbLeave = db.tblPlLeaves.Where(x => x.LeaveID == ID).FirstOrDefault();
                            if (tbLeave != null)
                            {
                                if (tbLeave.StartDate == startdat && tbLeave.EndDate == endDa)
                                {

                                    if (tbLeave.Status == "A")
                                    {
                                        if (leavn.LeaveBlnc < 0)
                                        {
                                            int? ddufk = leavn.LeaveBlnc - Convert.ToInt32(tbLeave.LeaveDays);
                                            int? ddiflea = ddufk + dayy;
                                            leavn.LeaveBlnc = ddiflea;
                                            db.SubmitChanges();
                                            BindGridLeaveBlnc();
                                        }
                                        else
                                        {
                                            int? ddufk = leavn.LeaveBlnc + Convert.ToInt32(tbLeave.LeaveDays);
                                            int? ddiflea = ddufk - dayy;
                                            leavn.LeaveBlnc = ddiflea;
                                            db.SubmitChanges();
                                            BindGridLeaveBlnc();
                                        }
                                    }
                                    else
                                    {
                                        if (leavn.LeaveBlnc < 0)
                                        {
                                            //int? ddufk = leavn.LeaveBlnc - Convert.ToInt32(tbLeave.LeaveDays);
                                            int? ddiflea = leavn.LeaveBlnc + dayy;
                                            leavn.LeaveBlnc = ddiflea;
                                            db.SubmitChanges();
                                            BindGridLeaveBlnc();
                                        }
                                        else
                                        {
                                            //int? ddufk = leavn.LeaveBlnc + Convert.ToInt32(tbLeave.LeaveDays);
                                            int? ddiflea = leavn.LeaveBlnc - dayy;
                                            leavn.LeaveBlnc = ddiflea;
                                            db.SubmitChanges();
                                            BindGridLeaveBlnc();
                                        }
                                    }
                                    
                                        
                                    
                                }
                                else
                                {
                                    if (leavn.LeaveBlnc<0)
                                    {
                                        int? ddiflea = leavn.LeaveBlnc - Convert.ToInt32(tbLeave.LeaveDays);
                                        int? ddufk = ddiflea + dayy;
                                        leavn.LeaveBlnc = ddufk;
                                        db.SubmitChanges();
                                        BindGridLeaveBlnc();
                                    }
                                    else
                                    {
                                        int? ddiflea = leavn.LeaveBlnc + Convert.ToInt32(tbLeave.LeaveDays);
                                        int? ddufk = ddiflea - dayy;
                                        leavn.LeaveBlnc = ddufk;
                                        db.SubmitChanges();
                                        BindGridLeaveBlnc();
                                    }
                                }
                            }

                        }
                        else
                        {
                            ucMessage.ShowMessage("Leave Balance Not Found", BL.Enums.MessageType.Error);
                            return;
                        }

                    }
                    else
                    {
                        if (ddlStatus.SelectedValue == "P")
                        {
                            
                            DateTime startdat = Convert.ToDateTime(txtStartDate.Text);
                            DateTime endDa = Convert.ToDateTime(txtEndDate.Text);
                            TimeSpan tss = endDa.Subtract(startdat);
                            int dayy = tss.Days + 1;
                            tblPlLeave UpLea = db.tblPlLeaves.Where(x => x.LeaveID == ID).FirstOrDefault();
                            int empII = Convert.ToInt32(UpLea.EmpID);
                            int leaI = Convert.ToInt32(UpLea.LeaveTypeID);
                            tblPlLeaveBlnc leab = db.tblPlLeaveBlncs.Where(x => x.LeaveTypeID == leaI && x.empID == empII).FirstOrDefault();
                            if (UpLea != null)
                            {
                                //tblPlLeave tbLeave = db.tblPlLeaves.Where(x => x.LeaveID == ID).FirstOrDefault();

                                if (UpLea.Status == "A")
                                {
                                    if (leab.LeaveBlnc < 0)
                                    {
                                        decimal? ddiflea = UpLea.LeaveDays;
                                        decimal? decci = leab.LeaveBlnc - ddiflea;
                                        leab.LeaveBlnc = Convert.ToInt32(decci);
                                        db.SubmitChanges();
                                        BindGridLeaveBlnc();
                                    }
                                    else
                                    {
                                        decimal? ddiflea = UpLea.LeaveDays;
                                        decimal? decci = leab.LeaveBlnc + ddiflea;
                                        leab.LeaveBlnc = Convert.ToInt32(decci);
                                        db.SubmitChanges();
                                        BindGridLeaveBlnc();
                                    }

                                    
                                }
                                else
                                {
                                    if (leab.LeaveBlnc < 0)
                                    {
                                        int? ddiflea = leab.LeaveBlnc - Convert.ToInt32(UpLea.LeaveDays);
                                        int? ddufk = ddiflea + dayy;
                                        leab.LeaveBlnc = ddufk;
                                        db.SubmitChanges();
                                        BindGridLeaveBlnc();
                                    }
                                    else
                                    {
                                        int? ddiflea = leab.LeaveBlnc + Convert.ToInt32(UpLea.LeaveDays);
                                        int? ddufk = ddiflea - dayy;
                                        leab.LeaveBlnc = ddufk;
                                        db.SubmitChanges();
                                        BindGridLeaveBlnc();
                                    }
                                    
                                }

                            }
                        }
                    }

                    // leav = mgtleave.GetByID(ID, LeaveDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    // leav = db.tblPlLeaves.Where(x => x.EmpID == ID).FirstOrDefault();
                    if (Convert.ToDateTime(txtEndDate.Text.Trim()).Month == Convert.ToDateTime(txtStartDate.Text.Trim()).Month)
                    {

                        tblPlLeave plLeave = new tblPlLeave();
                        plLeave = db.tblPlLeaves.Where(x => x.LeaveID == ID).FirstOrDefault();

                        //plLeave.CompID = Convert.ToByte(CompID);

                        if (ddlEmployee.SelectedValue == "0")
                        {
                            ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            plLeave.EmpID = Convert.ToInt32(ddlEmployee.SelectedValue);
                        }
                        if (ddlleaveType.SelectedValue == "0")
                        {
                            ucMessage.ShowMessage("Please Select Leave Type", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            plLeave.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                        }
                        if (txtStartDate.Text == "")
                        {
                            ucMessage.ShowMessage("Start Date Is Required", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            plLeave.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                        }
                        if (txtEndDate.Text == "")
                        {
                            ucMessage.ShowMessage("End Date Is Required", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            plLeave.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                        }
                        if (txtRemarks.Text == "")
                        {
                            plLeave.Remarks = null;
                        }
                        else
                        {
                            plLeave.Remarks = txtRemarks.Text;
                        }
                        if (ddlStatus.SelectedValue == "0")
                        {
                            ucMessage.ShowMessage("Please Select Status", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            plLeave.Status = Convert.ToString(ddlStatus.SelectedValue);
                        }

                        TimeSpan tss1 = EndDate.Subtract(StartDate);
                        int Diff = tss1.Days + 1;
                        plLeave.LeaveDays = Diff;

                        //plLeave.IsActive = Convert.ToBoolean(checkIsactive.Checked);

                        //if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
                        //{
                        //    if (Convert.ToDateTime(txtStartDate.Text.Trim()).Date != Convert.ToDateTime(txtEndDate.Text.Trim()).Date)
                        //    {
                        //        ucMessage.ShowMessage("Start and end date should be same for half leave", RMS.BL.Enums.MessageType.Error);
                        //        return;
                        //    }
                        //    else
                        //    {
                        //        plLeave.LeaveDays = Convert.ToDecimal(0.5);
                        //    }

                        //}
                        //else
                        //{
                        //    int dtDiff = (Convert.ToDateTime(txtEndDate.Text.Trim()) - Convert.ToDateTime(txtStartDate.Text.Trim())).Days;
                        //    plLeave.LeaveDays = Convert.ToDecimal(dtDiff);
                        //}

                        if (AppCycle)
                        {
                            if (plLeave.LeaveTypeID.Value.Equals(1))
                            {
                                //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[6].Text))
                                //{
                                //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining casual leaves", RMS.BL.Enums.MessageType.Error);
                                //    plLeave = null;
                                //    return;
                                //}
                            }
                            if (plLeave.LeaveTypeID.Value.Equals(2))
                            {
                                //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[7].Text))
                                //{
                                //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining medical leaves", RMS.BL.Enums.MessageType.Error);
                                //    plLeave = null;
                                //    return;
                                //}
                            }
                            if (plLeave.LeaveTypeID.Value.Equals(3))
                            {
                                //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[8].Text))
                                //{
                                //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining annual leaves", RMS.BL.Enums.MessageType.Error);
                                //    plLeave = null;
                                //    return;
                                //}
                            }
                            if (CanApprove && CanEnter)
                            {
                                if (Session["UserID"] == null)
                                    plLeave.ReqBy = Request.Cookies["uzr"]["UserID"].ToString();
                                else
                                    plLeave.ReqBy = Session["UserID"].ToString();
                                plLeave.ReqDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            if (ddlStatus.SelectedValue.Equals("A"))
                            {
                                if (CanApprove && !CanEnter)
                                {
                                    if (Session["UserID"] == null)
                                        plLeave.AppBy = Request.Cookies["uzr"]["UserID"].ToString();
                                    else
                                        plLeave.AppBy = Session["UserID"].ToString();
                                    plLeave.AppDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                }
                            }

                        }
                        if (Session["UserID"] == null)
                            plLeave.UpdateBy = Request.Cookies["uzr"]["UserID"].ToString();
                        else
                            plLeave.UpdateBy = Session["UserID"].ToString();
                        plLeave.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        

                        db.SubmitChanges();
                        BindGrid();
                        ClearFields();
                        //if (!mgtleave.ISAlreadyExist(plLeave, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                        //{

                        //    mgtleave.Insert(plLeave, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                        //    BindGrid();
                        //    ClearFields();
                        //}
                        //else
                        //{
                        //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EmpAlreadyTakenLeave").ToString(), RMS.BL.Enums.MessageType.Error);

                        //}
                    }
                    else
                    {
                        leav = db.tblPlLeaves.Where(x => x.LeaveID == ID).FirstOrDefault();
                        if (leav != null)
                        {
                            // leav.CompID = leav.CompID;
                            if (ddlEmployee.SelectedValue == "0")
                            {
                                ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                leav.EmpID = Convert.ToInt32(ddlEmployee.SelectedValue);
                            }
                            if (ddlleaveType.SelectedValue == "")
                            {
                                ucMessage.ShowMessage("Please Select Leave Type", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                leav.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                            }
                            if (txtStartDate.Text == "")
                            {
                                ucMessage.ShowMessage("Start Date is Required", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                leav.StartDate = StartDate;
                            }
                            if (txtEndDate.Text == "")
                            {
                                ucMessage.ShowMessage("End Date is Required", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                leav.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                            }
                            if (txtRemarks.Text == "")
                            {
                                leav.Remarks = null;
                            }
                            else
                            {
                                leav.Remarks = txtRemarks.Text;
                            }

                            if (ddlStatus.SelectedValue == "")
                            {
                                ucMessage.ShowMessage("Please Select Status", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                leav.Status = ddlStatus.SelectedValue;
                            }
                            TimeSpan day = EndDate.Subtract(StartDate);
                            //int day = ((EndDate - StartDate).Days + 1);
                            int ddifdy = day.Days + 1;
                            leav.LeaveDays = Convert.ToDecimal(ddifdy);
                            //if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
                            //{
                            //    if (Convert.ToDateTime(txtStartDate.Text.Trim()).Date != Convert.ToDateTime(txtEndDate.Text.Trim()).Date)
                            //    {
                            //        ucMessage.ShowMessage("Start and end date should be same for half leave", RMS.BL.Enums.MessageType.Error);
                            //        return;
                            //    }
                            //    else
                            //    {
                            //        leav.LeaveDays = Convert.ToDecimal(0.5);
                            //    }
                            //}
                            //else
                            //{
                            //    int dtDiff = (Convert.ToDateTime(txtEndDate.Text.Trim()) - Convert.ToDateTime(txtStartDate.Text.Trim())).Days + 1;
                            //    //leav.LeaveDays = Convert.ToDecimal(dtDiff);
                            //}



                            leav.UpdateBy = Session["UserID"].ToString();
                            leav.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                            db.SubmitChanges();
                            ClearFields();
                            //mgtleave.Update(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                            BindGrid();
                        }
                        else
                        {
                            // leav.CompID = leav.CompID;
                            leav.EmpID = ID;
                            leav.StartDate = StartDate;
                            int day = ((EndDate - StartDate).Days + 1);
                            leav.LeaveDays = Convert.ToDecimal(day);
                            leav.Remarks = txtRemarks.Text;
                            leav.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                            leav.Status = ddlStatus.SelectedValue;
                            // temp.LeaveTypeID = leav.LeaveTypeID;



                            if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
                            {
                                leav.LeaveDays = Convert.ToDecimal(0.5);
                            }
                            else
                            {
                                int dtDiff = (Convert.ToDateTime(txtEndDate.Text.Trim()) - Convert.ToDateTime(txtStartDate.Text.Trim())).Days + 1;
                                leav.LeaveDays = Convert.ToDecimal(dtDiff);
                            }

                            leav.UpdateBy = Session["UserID"].ToString();
                            leav.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            leav.Remarks = txtRemarks.Text;

                            //  db.SubmitChanges();

                            mgtleave.Insert(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                            ClearFields();
                            BindGrid();
                        }
                    }


                           
                        }
                    else
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EnterValidDates").ToString(), RMS.BL.Enums.MessageType.Error);
                    }
                    
                txtStartDate.Enabled = true;
            }
        }

        protected void Insert()
        {
            if (ID == 0)
            {
                if (ddlleaveType.SelectedValue != "0")
                {
                    DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                    DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                    DateTime tempStart = StartDate;
                    DateTime tempEnd = EndDate;

                    if (Convert.ToDateTime(txtEndDate.Text.Trim()) >= Convert.ToDateTime(txtStartDate.Text.Trim()))
                    {
                        if (Convert.ToDateTime(txtEndDate.Text.Trim()).Month == Convert.ToDateTime(txtStartDate.Text.Trim()).Month)
                        {

                            tblPlLeave plLeave = new tblPlLeave();

                            //plLeave.CompID = Convert.ToByte(CompID);

                            if (ddlEmployee.SelectedValue == "0")
                            {
                                ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                            }
                            else
                            {
                                plLeave.EmpID = Convert.ToInt32(ddlEmployee.SelectedValue);
                            }
                            if (txtStartDate.Text == "")
                            {
                                ucMessage.ShowMessage("Start Date Is Required", BL.Enums.MessageType.Error);
                            }
                            else
                            {
                                plLeave.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                            }
                            if (txtEndDate.Text == "")
                            {
                                ucMessage.ShowMessage("End Date Is Required", BL.Enums.MessageType.Error);
                            }
                            else
                            {
                                plLeave.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                            }
                            if (ddlleaveType.SelectedValue == "0")
                            {
                                ucMessage.ShowMessage("Please Select Leave Type", BL.Enums.MessageType.Error);
                            }
                            else
                            {
                                plLeave.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                            }
                            
                            //plLeave.IsActive = Convert.ToBoolean(checkIsactive.Checked);

                            if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
                            {
                                plLeave.LeaveDays = Convert.ToDecimal(0.5);
                            }
                            else
                            {
                                int dtDiff = (Convert.ToDateTime(txtEndDate.Text.Trim()) - Convert.ToDateTime(txtStartDate.Text.Trim())).Days + 1;
                                plLeave.LeaveDays = Convert.ToDecimal(dtDiff);
                            }

                            if (AppCycle)
                            {
                                if (plLeave.LeaveTypeID.Value.Equals(1))
                                {
                                    //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[6].Text))
                                    //{
                                    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining casual leaves", RMS.BL.Enums.MessageType.Error);
                                    //    plLeave = null;
                                    //    return;
                                    //}
                                }
                                if (plLeave.LeaveTypeID.Value.Equals(2))
                                {
                                    //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[7].Text))
                                    //{
                                    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining medical leaves", RMS.BL.Enums.MessageType.Error);
                                    //    plLeave = null;
                                    //    return;
                                    //}
                                }
                                if (plLeave.LeaveTypeID.Value.Equals(3))
                                {
                                    //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[8].Text))
                                    //{
                                    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining annual leaves", RMS.BL.Enums.MessageType.Error);
                                    //    plLeave = null;
                                    //    return;
                                    //}
                                }
                                if (CanApprove && CanEnter)
                                {
                                    if (Session["UserID"] == null)
                                        plLeave.ReqBy = Request.Cookies["uzr"]["UserID"].ToString();
                                    else
                                        plLeave.ReqBy = Session["UserID"].ToString();
                                    plLeave.ReqDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                }
                                if (ddlStatus.SelectedValue.Equals("A"))
                                {
                                    if (CanApprove && !CanEnter)
                                    {
                                        if (Session["UserID"] == null)
                                            plLeave.AppBy = Request.Cookies["uzr"]["UserID"].ToString();
                                        else
                                            plLeave.AppBy = Session["UserID"].ToString();
                                        plLeave.AppDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    }
                                }
                                plLeave.Status = Convert.ToString(ddlStatus.SelectedValue);
                            }
                            if (Session["UserID"] == null)
                                plLeave.UpdateBy = Request.Cookies["uzr"]["UserID"].ToString();
                            else
                                plLeave.UpdateBy = Session["UserID"].ToString();
                            plLeave.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            plLeave.Remarks = txtRemarks.Text;

                            if (!mgtleave.ISAlreadyExist(plLeave, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                            {

                                mgtleave.Insert(plLeave, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                                BindGrid();
                                ClearFields();
                            }
                            else
                            {
                                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EmpAlreadyTakenLeave").ToString(), RMS.BL.Enums.MessageType.Error);

                            }
                        }
                        else
                        {
                            TimeSpan ts;
                            tblPlLeave leav;
                            int dtDiff;
                            while (tempStart < tempEnd)
                            {
                                if (tempStart.Month + 1 >= 13)
                                {
                                    tempEnd = new DateTime(tempStart.Year + 1, 1, 1);
                                }
                                else
                                {
                                    tempEnd = new DateTime(tempStart.Year, tempStart.Month + 1, 1);
                                }

                                if (tempEnd > EndDate)
                                {
                                    tempEnd = EndDate;
                                    tempEnd = tempEnd.AddDays(1.0);
                                }
                                ts = tempEnd.Subtract(tempStart);

                                leav = new tblPlLeave();
                                if (Session["CompID"] == null)
                                {
                                    //leav.CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                                }
                                else
                                {
                                    // leav.CompID = Convert.ToByte(Session["CompID"].ToString());
                                }

                                leav.EmpID = ID;

                                leav.StartDate = tempStart;
                                leav.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                                leav.EndDate = tempEnd;
                                dtDiff = ts.Days;

                                leav.LeaveDays = Convert.ToDecimal(dtDiff);
                                

                                if (!mgtleave.ISAlreadyExist(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                                {

                                    mgtleave.Insert(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                                    BindGrid();

                                    ClearFields();
                                }
                                else
                                {
                                    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EmpAlreadyTakenLeave").ToString(), RMS.BL.Enums.MessageType.Error);
                                }
                                tempStart = tempEnd;
                                if (tempStart.Month + 1 >= 13)
                                {
                                    tempEnd = new DateTime(tempStart.Year + 1, 1, 1);
                                }
                                else
                                {
                                    tempEnd = new DateTime(tempStart.Year, tempStart.Month + 1, 1);
                                }
                                if (tempEnd > EndDate)
                                {
                                    tempEnd = EndDate;
                                    tempEnd = tempEnd.AddDays(1.0);
                                }
                            }
                        }
                    }
                    else
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EnterValidDates").ToString(), RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EnterLeaveType").ToString(), RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                if (ddlleaveType.SelectedValue != "0")
                {
                    DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                    DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                    DateTime tempStart = StartDate;
                    DateTime tempEnd = EndDate;

                    if (Convert.ToDateTime(txtEndDate.Text.Trim()) >= Convert.ToDateTime(txtStartDate.Text.Trim()))
                    {
                        if (Convert.ToDateTime(txtEndDate.Text.Trim()).Month == Convert.ToDateTime(txtStartDate.Text.Trim()).Month)
                        {

                            tblPlLeave plLeave = new tblPlLeave();

                            //plLeave.CompID = Convert.ToByte(CompID);

                            plLeave.EmpID = Convert.ToInt32(ddlEmployee.SelectedValue);

                            plLeave.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                            plLeave.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());

                            plLeave.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
                            //plLeave.IsActive = Convert.ToBoolean(checkIsactive.Checked);

                            if (Convert.ToInt32(ddlleaveType.SelectedValue).Equals(5) || Convert.ToInt32(ddlleaveType.SelectedValue).Equals(6))
                            {
                                plLeave.LeaveDays = Convert.ToDecimal(0.5);
                            }
                            else
                            {
                                int dtDiff = (Convert.ToDateTime(txtEndDate.Text.Trim()) - Convert.ToDateTime(txtStartDate.Text.Trim())).Days + 1;
                                plLeave.LeaveDays = Convert.ToDecimal(dtDiff);
                            }

                            if (AppCycle)
                            {
                                if (plLeave.LeaveTypeID.Value.Equals(1))
                                {
                                    //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[6].Text))
                                    //{
                                    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining casual leaves", RMS.BL.Enums.MessageType.Error);
                                    //    plLeave = null;
                                    //    return;
                                    //}
                                }
                                if (plLeave.LeaveTypeID.Value.Equals(2))
                                {
                                    //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[7].Text))
                                    //{
                                    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining medical leaves", RMS.BL.Enums.MessageType.Error);
                                    //    plLeave = null;
                                    //    return;
                                    //}
                                }
                                if (plLeave.LeaveTypeID.Value.Equals(3))
                                {
                                    //if (plLeave.LeaveDays > Convert.ToDecimal(grdLeaveStatus.Rows[0].Cells[8].Text))
                                    //{
                                    //    ucMessage.ShowMessage("Requested leaves should be less than equal to remaining annual leaves", RMS.BL.Enums.MessageType.Error);
                                    //    plLeave = null;
                                    //    return;
                                    //}
                                }
                                if (CanApprove && CanEnter)
                                {
                                    if (Session["UserID"] == null)
                                        plLeave.ReqBy = Request.Cookies["uzr"]["UserID"].ToString();
                                    else
                                        plLeave.ReqBy = Session["UserID"].ToString();
                                    plLeave.ReqDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                }
                                if (ddlStatus.SelectedValue.Equals("A"))
                                {
                                    if (CanApprove && !CanEnter)
                                    {
                                        if (Session["UserID"] == null)
                                            plLeave.AppBy = Request.Cookies["uzr"]["UserID"].ToString();
                                        else
                                            plLeave.AppBy = Session["UserID"].ToString();
                                        plLeave.AppDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    }
                                }
                                plLeave.Status = Convert.ToString(ddlStatus.SelectedValue);
                            }
                            if (Session["UserID"] == null)
                                plLeave.UpdateBy = Request.Cookies["uzr"]["UserID"].ToString();
                            else
                                plLeave.UpdateBy = Session["UserID"].ToString();
                            plLeave.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            plLeave.Remarks = txtRemarks.Text;

                            if (!mgtleave.ISAlreadyExist(plLeave, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                            {

                                mgtleave.Update(plLeave, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "Updated Successfully").ToString(), RMS.BL.Enums.MessageType.Info);
                                BindGrid();
                                ClearFields();
                            }
                            else
                            {
                                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EmpAlreadyTakenLeave").ToString(), RMS.BL.Enums.MessageType.Error);

                            }
                        }
                        else
                        {
                            TimeSpan ts;
                            tblPlLeave leav;
                            int dtDiff;
                            while (tempStart < tempEnd)
                            {
                                if (tempStart.Month + 1 >= 13)
                                {
                                    tempEnd = new DateTime(tempStart.Year + 1, 1, 1);
                                }
                                else
                                {
                                    tempEnd = new DateTime(tempStart.Year, tempStart.Month + 1, 1);
                                }

                                if (tempEnd > EndDate)
                                {
                                    tempEnd = EndDate;
                                    tempEnd = tempEnd.AddDays(1.0);
                                }
                                ts = tempEnd.Subtract(tempStart);

                                leav = new tblPlLeave();
                                if (Session["CompID"] == null)
                                {
                                    //leav.CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                                }
                                else
                                {
                                    // leav.CompID = Convert.ToByte(Session["CompID"].ToString());
                                }

                                leav.EmpID = ID;

                                leav.StartDate = tempStart;
                                leav.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);

                                dtDiff = ts.Days;

                                leav.LeaveDays = Convert.ToDecimal(dtDiff);

                                if (!mgtleave.ISAlreadyExist(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                                {

                                    mgtleave.Update(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "Updated Successfully").ToString(), RMS.BL.Enums.MessageType.Info);
                                    BindGrid();

                                    ClearFields();
                                }
                                else
                                {
                                    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EmpAlreadyTakenLeave").ToString(), RMS.BL.Enums.MessageType.Error);
                                }
                                tempStart = tempEnd;
                                if (tempStart.Month + 1 >= 13)
                                {
                                    tempEnd = new DateTime(tempStart.Year + 1, 1, 1);
                                }
                                else
                                {
                                    tempEnd = new DateTime(tempStart.Year, tempStart.Month + 1, 1);
                                }
                                if (tempEnd > EndDate)
                                {
                                    tempEnd = EndDate;
                                    tempEnd = tempEnd.AddDays(1.0);
                                }
                            }
                        }
                    }
                    else
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EnterValidDates").ToString(), RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "EnterLeaveType").ToString(), RMS.BL.Enums.MessageType.Error);
                }
            }
        }

        private void ClearFields()
        {
            ID = 0;
            //CompID = 0;
            ActionStr = "Insert";
           // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            ddlLeaeveTYpe.SelectedValue = "0";
            //txtavailBlnc.Text = "";
            txtentirler.Text = "";
            //ddlEmployee.SelectedValue = "0";
            ddlStatus.SelectedValue = "0";
            ddlleaveType.SelectedIndex = 0;
            if (AppCycle)
            {
              //  ddlStatus.SelectedValue = "0";
            }
            grdLeave.SelectedIndex = -1;
            txtStartDate.Enabled = true;
            txtStartDate.ReadOnly = false;
            btnDelete.Visible = false;
            //txtcasualLeave.Text = "";
            //txtCasualBlnc.Text = "";
            //txtEarnedLeave.Text = "";
            //txtEarnedBlnc.Text = "";
            //txtMeteLeave.Text = "";
            //txtMeterBlnc.Text = "";
            //txtPeterLeave.Text = "";
            //txtPeterBlnc.Text = "";




            txtRemarks.Text = "";
           // EmpSrchUC.ClearFields();
           // EmpSrchUC.EditModeDataHide();
          //  EmpSrchUC.Focus();
            //EmpSrchUC.EditModeDataHide();
            //EmpSrchUC.Focus();
        }

        #endregion

    }
}

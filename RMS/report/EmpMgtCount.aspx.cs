using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;

namespace RMS.Setup
{
    public partial class EmpMgtCount : BasePage
    {

        #region DataMembers
        //RMS.BL.tblAppEmp usr;

        PlCountBL countmgr=new PlCountBL();
        GroupBL groupManager = new GroupBL();
        PlLeaveBL mgtleave=new PlLeaveBL();
        //PlAllowBL allowBL = new PlAllowBL();
        EmpBL empBL = new EmpBL();

        ListItem selList = new ListItem();
        ListItem selListSub = new ListItem();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'EmpMgtCount.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpMgtCount.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        public DateTime LeaveDate
        {
            get { return (ViewState["LeaveDate"] == null) ? new DateTime() : Convert.ToDateTime(ViewState["LeaveDate"]); }
            set { ViewState["LeaveDate"] = value; }
        }

        #endregion

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "RptHeadcountSumm").ToString();
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                //txtStartDateCal.Format = Session["DateFormat"].ToString();
                //txtEndDateCal.Format = Session["DateFormat"].ToString();
                FillDropDownLeaveType();
                //FillDropDownPaymentType();
                BindGrid("", 0, 0);
                //ucButtons.ValidationGroupName = "main";
                //EmpSrchUC.Focus();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //BindGrid(txtFltEmp.Text.Trim(), Convert.ToInt32(ddlFltRegion.SelectedValue), Convert.ToInt32(ddlFltSegment.SelectedValue));
        }

        protected void grdEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ClearFields();
            //ID = Convert.ToInt32(grdlev.SelectedDataKey.Values["EmpID"].ToString());
            //CompID = Convert.ToInt32(grdlev.SelectedDataKey.Values["CompID"].ToString());
            //LeaveDate = Convert.ToDateTime(grdlev.SelectedDataKey.Values["LeaveDate"].ToString());
            this.GetByID();

        }

        protected void grdRegion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdRegion.PageIndex = e.NewPageIndex;
            grdRegion.DataSource = countmgr.getRegionalData(txtmonth.Text, txtYear.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdRegion.DataBind();
            //BindGrid(txtFltEmp.Text.Trim(), Convert.ToInt32(ddlFltRegion.SelectedValue), Convert.ToInt32(ddlFltSegment.SelectedValue));
        }

        protected void grdDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDivision.PageIndex = e.NewPageIndex;
            grdDivision.DataSource = countmgr.getDivisionalData(txtmonth.Text, txtYear.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdDivision.DataBind();
            //BindGrid(txtFltEmp.Text.Trim(), Convert.ToInt32(ddlFltRegion.SelectedValue), Convert.ToInt32(ddlFltSegment.SelectedValue));
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                //pnlMain.Enabled = true;
            }
            else if (e.CommandName == "Save")
            {
                if (ID == 0)
                {
                    this.Insert();
                    //pnlMain.Enabled = false;
                    //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                    //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);

                }
                else
                {
                    this.Update();
                    //pnlMain.Enabled = false;
                    //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);

                }
            }
            else if (e.CommandName == "Delete")
            {
                // TRANSACTION WALA KAAM KARNA HAI......

                try
                {
                    this.Delete(ID);
                    //pnlMain.Enabled = false;
                 //   ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
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
                BindGrid("", 0, 0);
                ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                //pnlMain.Enabled = true;
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
            }
            else if (e.CommandName == "Cancel")
            {
                //pnlMain.Enabled = false;
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                ClearFields();

            }
        }
        protected void grdEmps_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[2].Text = DateTime.Parse(Convert.ToDateTime(e.Row.Cells[2].Text).ToString()).ToString(Session["DateFormat"].ToString());
                //e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString(Session["DateFormat"].ToString());
            }
        }

        #endregion

        #region Helping Method
        protected void BindGrid(string empName, int RegId, int segId)
        {
            //this.grdlev.DataSource = mgtleave.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //this.grdlev.DataBind();
        }
        private void FillDropDownLeaveType()
        {
            //ddlleaveType.DataTextField = "LeaveTypeDesc";
            //ddlleaveType.DataValueField = "leaveTypeID";
            //ddlleaveType.DataSource = mgtleave.GetAllLeaveTypeCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlleaveType.DataBind();
        }

protected void DisplayData()
{


    if(RadioButtonList1.SelectedValue.Equals("Division"))
    {
        grdDivision.DataSource = countmgr.getDivisionalData(txtmonth.Text, txtYear.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
           grdDivision.DataBind();
            Division.Visible=true;
    }
    else if (RadioButtonList1.SelectedValue.Equals("Region"))
    {
        grdRegion.DataSource = countmgr.getRegionalData(txtmonth.Text, txtYear.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        grdRegion.DataBind();
        Region.Visible = true;
    }

}

        protected void GetByID()
        {
            //tblPlAlow empPojo = allowBL.GetByID(CompID, ID, DateTime.Parse(EffDateStr), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            tblPlLeave leav=mgtleave.GetByID(ID,LeaveDate,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            

            ////this.txtEmpCode.Text = empPojo.EmpID.ToString();
            ////this.txtFullName.Text = empPojo.FirstName + ' ' + empPojo.MidName + ' ' + empPojo.SirName;

            //EmpSrchUC.EditModeDataShow(leav.tblPlEmpData.FullName,
            //    leav.tblPlEmpData.EmpCode,
            //    leav.tblPlEmpData.tblPlCode1.CodeDesc,
            //    leav.tblPlEmpData.tblPlCode.CodeDesc);
            
            //ddlleaveType.SelectedValue=leav.LeaveTypeID.ToString();
            //txtStartDate.Text=leav.LeaveDate.ToString("dd-MMM-yy");
            //DateTime EndDate=Convert.ToDateTime(leav.LeaveDate);
            //EndDate=EndDate.AddDays(Convert.ToDouble(leav.LeaveDays));
            //txtEndDate.Text = EndDate.ToString("dd-MMM-yy");
            ////txtBasicPay.Text = empPojo.Basic.ToString();
            ////txtEffDate.Text = empPojo.EffDate.ToString(Session["DateFormat"].ToString());
            ////txtHouseRent.Text = empPojo.HR.ToString();
            ////txtSplAll.Text = empPojo.SplAlow.ToString();
            ////txtUtilities.Text = empPojo.Utilities.ToString();
            ////txtFuelAll.Text = empPojo.FuelLimit.ToString();
            
            //pnlMain.Enabled = true;
            //ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update()
        {
            ////tblPlAlow allow = allowBL.GetByID(CompID, ID, DateTime.Parse(EffDateStr), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //tblPlLeave leav = mgtleave.GetByID(CompID, ID, LeaveDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //leav.LeaveDate = Convert.ToDateTime(txtStartDate.Text.Trim());
            //leav.LeaveTypeID = Convert.ToByte(ddlleaveType.SelectedValue);
            //leav.LeaveDays = Convert.ToDecimal((Convert.ToDateTime(txtEndDate.Text.Trim()).Day) - (Convert.ToDateTime(txtStartDate.Text.Trim()).Day));

            ////allow.CompID = Convert.ToByte(Session["CompID"].ToString());
            ////allow.EmpID = EmpSrchUC.EmpIDUC;// Convert.ToInt32(txtEmpCode.Text.Trim());
            ////allow.EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());
            //if (txtBasicPay.Text.Trim().Equals(""))
            //{
            //    allow.Basic = 0;
            //}
            //else
            //{
            //    allow.Basic = Convert.ToDecimal(txtBasicPay.Text.Trim());
            //}

            //if (txtHouseRent.Text.Trim().Equals(""))
            //{
            //    allow.HR = 0;
            //}
            //else
            //{
            //    allow.HR = Convert.ToDecimal(txtHouseRent.Text.Trim()); 
            //}

            //if (txtUtilities.Text.Trim().Equals(""))
            //{
            //    allow.Utilities = 0;
            //}
            //else
            //{
            //    allow.Utilities = Convert.ToDecimal(txtUtilities.Text.Trim());
            //}

            //if (txtSplAll.Text.Trim().Equals(""))
            //{
            //    allow.SplAlow = 0;
            //}
            //else
            //{
            //    allow.SplAlow = Convert.ToDecimal(txtSplAll.Text.Trim());
            //}

            //if (txtFuelAll.Text.Trim().Equals(""))
            //{
            //    allow.FuelLimit = 0;
            //}
            //else
            //{
            //    allow.FuelLimit = Convert.ToInt16(txtFuelAll.Text.Trim());
            //}

            //if (!allowBL.ISAlreadyExist(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //{
            //mgtleave.Update(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
            //BindGrid("", 0, 0);
            //ClearFields();
            //}
            //else
            //{
            //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
            //    pnlMain.Enabled = true;
            //}
        }

        protected void Delete(int Id)
        {
            //allowBL.DeleteByID(
            //              Convert.ToInt32(Session["CompID"].ToString()),
            //              Convert.ToInt32(Id), Convert.ToDateTime(txtEffDate.Text),
            //              (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        }

        protected void Insert()
        {
            //RMS.BL.Employee emp = new RMS.BL.Employee();
            //tblPlAlow allow = new tblPlAlow();
//tblPlLeave leav=new tblPlLeave();
//leav.CompID = Convert.ToByte(Session["CompID"].ToString());
//leav.EmpID = EmpSrchUC.EmpIDUC;// Convert.ToInt32(txtEmpCode.Text.Trim());
//leav.LeaveDate = Convert.ToDateTime(txtStartDate.Text.Trim());
//leav.LeaveTypeID=Convert.ToByte(ddlleaveType.SelectedValue);
//leav.LeaveDays = Convert.ToDecimal((Convert.ToDateTime(txtEndDate.Text.Trim()).Day) - (Convert.ToDateTime(txtStartDate.Text.Trim()).Day));

            //allow.EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());

            //if (txtBasicPay.Text.Trim().Equals(""))
            //{
            //    allow.Basic = 0;
            //}
            //else
            //{
            //    allow.Basic = Convert.ToDecimal(txtBasicPay.Text.Trim());
            //}

            //if (txtHouseRent.Text.Trim().Equals(""))
            //{
            //    allow.HR = 0;
            //}
            //else
            //{
            //    allow.HR = Convert.ToDecimal(txtHouseRent.Text.Trim());
            //}

            //if (txtUtilities.Text.Trim().Equals(""))
            //{
            //    allow.Utilities = 0;
            //}
            //else
            //{
            //    allow.Utilities = Convert.ToDecimal(txtUtilities.Text.Trim());
            //}

            //if (txtSplAll.Text.Trim().Equals(""))
            //{
            //    allow.SplAlow = 0;
            //}
            //else
            //{
            //    allow.SplAlow = Convert.ToDecimal(txtSplAll.Text.Trim());
            //}

            //if (txtFuelAll.Text.Trim().Equals(""))
            //{
            //    allow.FuelLimit = 0;
            //}
            //else
            //{
            //    allow.FuelLimit = Convert.ToInt16(txtFuelAll.Text.Trim());
            //}

            //if (!mgtleave.ISAlreadyExist(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //{  
            //    mgtleave.Insert(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
            //    BindGrid("", 0, 0);
            //    ClearFields();
            //}
            //else
            //{
            //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
            //    pnlMain.Enabled = true;
            //}
        }

        private void ClearFields()
        {
            ID = 0;
            CompID = 0;
            //EffDateStr = "";
            //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            //txtStartDate.Text = "";
            //txtEndDate.Text = "";
            //ddlleaveType.SelectedIndex=0;
            //txtBasicPay.Text = "";
            //txtHouseRent.Text = "";
            //txtFuelAll.Text = "";
            //txtSplAll.Text = "";
            //txtUtilities.Text = "";
            //grdlev.SelectedIndex = -1;
            //EmpSrchUC.ClearFields();
            //EmpSrchUC.EditModeDataHide();
            //EmpSrchUC.Focus();
        }

 
        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            Region.Visible=false;
            Division.Visible=false;
            DisplayData();
        }
    }
}
 
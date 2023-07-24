using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

namespace RMS.Setup
{
    public partial class MonthSal : BasePage
    {

        #region DataMembers
        SalaryBL salarymgr = new SalaryBL();
        //RMS.BL.tblAppEmp usr;

        //GroupBL groupManager = new GroupBL();
        PlLeaveBL mgtleave = new PlLeaveBL();
        ////PlAllowBL allowBL = new PlAllowBL();
        //EmpBL empBL = new EmpBL();

        //ListItem selList = new ListItem();
        //ListItem selListSub = new ListItem();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'MonthSal.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'MonthSal.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpMonthSal").ToString();
                
                FillStartDropDownPaymentPred(1);
                ddlPayPred.Visible=false;

                int GroupID = 0;
                if (Session["GroupID"] == null)
                {
                    GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                }
                else
                {
                    GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                }
                //if (GroupID == 3)
                //{
                //    if (Session["EmpCode"] == null)
                //    {
                //        txtempcode.Text = Request.Cookies["uzr"]["EmpCode"];
                //    }
                //    else
                //    {
                //        txtempcode.Text = Session["EmpCode"].ToString();
                //    }
                //    txtempcode.Enabled = false;
                //}
            }
            
            //if(ID!=0)
            //{
            //    FillDropDownPaymentPred(ID,CompID);
            //}
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //BindGrid(txtFltEmp.Text.Trim(), Convert.ToInt32(ddlFltRegion.SelectedValue), Convert.ToInt32(ddlFltSegment.SelectedValue));
        }

        protected void grdEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////ClearFields();
            //ID = Convert.ToInt32(grdlev.SelectedDataKey.Values["EmpID"].ToString());
            //CompID = Convert.ToInt32(grdlev.SelectedDataKey.Values["CompID"].ToString());
            //LeaveDate = Convert.ToDateTime(grdlev.SelectedDataKey.Values["LeaveDate"].ToString());
            //this.GetByID();

        }

        protected void grdEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //grdlev.PageIndex = e.NewPageIndex;

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
                    //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
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

        protected void GetByID()
        {
            //tblPlAlow empPojo = allowBL.GetByID(CompID, ID, DateTime.Parse(EffDateStr), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //tblPlLeave leav=mgtleave.GetByID(CompID,ID,LeaveDate,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


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
            ////if (txtBasicPay.Text.Trim().Equals(""))
            ////{
            ////    allow.Basic = 0;
            ////}
            ////else
            ////{
            ////    allow.Basic = Convert.ToDecimal(txtBasicPay.Text.Trim());
            ////}

            ////if (txtHouseRent.Text.Trim().Equals(""))
            ////{
            ////    allow.HR = 0;
            ////}
            ////else
            ////{
            ////    allow.HR = Convert.ToDecimal(txtHouseRent.Text.Trim()); 
            ////}

            ////if (txtUtilities.Text.Trim().Equals(""))
            ////{
            ////    allow.Utilities = 0;
            ////}
            ////else
            ////{
            ////    allow.Utilities = Convert.ToDecimal(txtUtilities.Text.Trim());
            ////}

            ////if (txtSplAll.Text.Trim().Equals(""))
            ////{
            ////    allow.SplAlow = 0;
            ////}
            ////else
            ////{
            ////    allow.SplAlow = Convert.ToDecimal(txtSplAll.Text.Trim());
            ////}

            ////if (txtFuelAll.Text.Trim().Equals(""))
            ////{
            ////    allow.FuelLimit = 0;
            ////}
            ////else
            ////{
            ////    allow.FuelLimit = Convert.ToInt16(txtFuelAll.Text.Trim());
            ////}

            ////if (!allowBL.ISAlreadyExist(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            ////{
            //mgtleave.Update(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
            //BindGrid("", 0, 0);
            //ClearFields();
            ////}
            ////else
            ////{
            ////    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
            ////    pnlMain.Enabled = true;
            ////}
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
            //            //RMS.BL.Employee emp = new RMS.BL.Employee();
            //            //tblPlAlow allow = new tblPlAlow();
            //tblPlLeave leav=new tblPlLeave();
            //leav.CompID = Convert.ToByte(Session["CompID"].ToString());
            //leav.EmpID = EmpSrchUC.EmpIDUC;// Convert.ToInt32(txtEmpCode.Text.Trim());
            //leav.LeaveDate = Convert.ToDateTime(txtStartDate.Text.Trim());
            //leav.LeaveTypeID=Convert.ToByte(ddlleaveType.SelectedValue);
            //leav.LeaveDays = Convert.ToDecimal((Convert.ToDateTime(txtEndDate.Text.Trim()).Day) - (Convert.ToDateTime(txtStartDate.Text.Trim()).Day));

            //            //allow.EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());

            //            //if (txtBasicPay.Text.Trim().Equals(""))
            //            //{
            //            //    allow.Basic = 0;
            //            //}
            //            //else
            //            //{
            //            //    allow.Basic = Convert.ToDecimal(txtBasicPay.Text.Trim());
            //            //}

            //            //if (txtHouseRent.Text.Trim().Equals(""))
            //            //{
            //            //    allow.HR = 0;
            //            //}
            //            //else
            //            //{
            //            //    allow.HR = Convert.ToDecimal(txtHouseRent.Text.Trim());
            //            //}

            //            //if (txtUtilities.Text.Trim().Equals(""))
            //            //{
            //            //    allow.Utilities = 0;
            //            //}
            //            //else
            //            //{
            //            //    allow.Utilities = Convert.ToDecimal(txtUtilities.Text.Trim());
            //            //}

            //            //if (txtSplAll.Text.Trim().Equals(""))
            //            //{
            //            //    allow.SplAlow = 0;
            //            //}
            //            //else
            //            //{
            //            //    allow.SplAlow = Convert.ToDecimal(txtSplAll.Text.Trim());
            //            //}

            //            //if (txtFuelAll.Text.Trim().Equals(""))
            //            //{
            //            //    allow.FuelLimit = 0;
            //            //}
            //            //else
            //            //{
            //            //    allow.FuelLimit = Convert.ToInt16(txtFuelAll.Text.Trim());
            //            //}

            //            if (!mgtleave.ISAlreadyExist(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //            {  
            //                mgtleave.Insert(leav, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
            //                BindGrid("", 0, 0);
            //                ClearFields();
            //            }
            //            else
            //            {
            //                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
            //                pnlMain.Enabled = true;
            //            }
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

        protected void CreatePDF(String FileName, String extension)
        {
            // Variables
            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + extension + "</OutputFormat>" +
            "  <PageWidth>14in</PageWidth>" +
            "  <PageHeight>8.27in</PageHeight>" +
            "  <MarginTop>0.3in</MarginTop>" +
            "  <MarginLeft>0.3in</MarginLeft>" +
            "  <MarginRight>0.3in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();

            viewer.LocalReport.ReportPath = "report/rdlc/rptSalarySlip.rdlc";
            ReportDataSource datasource = new ReportDataSource("spRptEmployeeListResult", emp);

            //ReportParameter prm = new ReportParameter("rpt_Prm_PayPeriod", ddlPayPerd.SelectedItem.Text);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
            //viewer.LocalReport.SetParameters(new ReportParameter[] { prm });

            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + ".") + extension);
            Response.BinaryWrite(bytes);
            //// create the file
            //// send it to the client to download
            Response.Flush();
        }

        private void FillStartDropDownPaymentPred(int compid)
        {
            ddlPayPred.Items.Clear();
            ddlPayPred.DataSource=null;
            ddlPayPred.DataBind();
            ddlPayPred.DataTextField = "CurPayPeriod";
            ddlPayPred.DataValueField = "CurPayPeriod";
            ddlPayPred.DataSource = salarymgr.GetStartPaymentTypeCombo(compid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlPayPred.DataBind();
        }
        
        private void FillDropDownPaymentPred(int empid,int compoid)
        {
            ddlPayPred.Items.Clear();
            ddlPayPred.DataSource = null;
            ddlPayPred.DataBind();
            ddlPayPred.DataTextField = "PayPerd";
            ddlPayPred.DataValueField = "PayPerd";
            ddlPayPred.DataSource = salarymgr.GetAllPaymentTypeCombo(empid,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlPayPred.DataBind();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            CreatePDF("SalarySlip", "pdf");
        }

        tblPlEmpData emp = new tblPlEmpData();
        protected void Button1_Click(object sender, EventArgs e)
        {

            emp = salarymgr.GetEmpByCode(txtempcode.Text, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (emp.EmpID != 0)
            {
                int paypred = Convert.ToInt32(ddlPayPred.SelectedValue);
                ID = emp.EmpID;
                CompID = emp.CompID;
                FillDropDownPaymentPred(ID, CompID);
                ddlPayPred.Visible = true;
                
                
                tblPlSalary sal = salarymgr.GetSalByid(emp.EmpID, emp.CompID,paypred, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                if (sal.EmpID != 0)
                {
                    LblDispName.Text = sal.tblPlEmpData.FullName.ToString();
                    LblDispCode.Text = sal.tblPlEmpData.EmpCode.ToString();

                    LblDispDesignation.Text = sal.tblPlEmpData.tblPlCode1 == null ? " " : sal.tblPlEmpData.tblPlCode1.CodeDesc.ToString();
                    LblDispDepartment.Text = sal.tblPlEmpData.tblPlCode == null ? " " : sal.tblPlEmpData.tblPlCode.CodeDesc.ToString();


                    LbldispbasicPay.Text = Convert.ToDouble((sal.Basic == null ? 0 : sal.Basic).ToString()).ToString("N");
                    LbldispHR.Text = Convert.ToDouble((sal.HR == null ? 0 : sal.HR).ToString()).ToString("N");
                    LbldispUtilities.Text = Convert.ToDouble((sal.Utilities == null ? 0 : sal.Utilities).ToString()).ToString("N");
                    lbldispHardShipAll.Text = Convert.ToDouble((sal.NSHA == null ? 0 : sal.NSHA).ToString()).ToString("N");
                    lbldispSpecialAll.Text = Convert.ToDouble((sal.SplAlow == null ? 0 : sal.SplAlow).ToString()).ToString("N");
                    Lbldispmed_bill.Text = Convert.ToDouble((sal.MedBil == null ? 0 : sal.MedBil).ToString()).ToString("N");
                    Lbldispsal_Arr.Text = Convert.ToDouble((salarymgr.CAlculateArrears(sal.tblPlEmpData.EmpID,paypred, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])).ToString()).ToString("N");
                    LbldispEDA.Text = Convert.ToDouble((sal.EDA == null ? 0 : sal.EDA).ToString()).ToString("N");
                    LbldispPSM.Text = Convert.ToDouble((sal.PSMInc == null ? 0 : sal.PSMInc).ToString()).ToString("N");
                    LbldispMOB_Alownce.Text = Convert.ToDouble((sal.MobAlow == null ? 0 : sal.MobAlow).ToString()).ToString("N");
                    LbldispFeul_Alownce.Text = Convert.ToDouble((sal.CA == null ? 0 : sal.CA).ToString()).ToString("N");
                    Lbldispexp_claim.Text = Convert.ToDouble((sal.ExpClaim == null ? 0 : sal.ExpClaim).ToString()).ToString("N");
                    Lbldispsales_Incentives.Text = Convert.ToDouble((sal.SIMInc == null ? 0 : sal.SIMInc).ToString()).ToString("N");
                    LbldispOthers.Text = Convert.ToDouble("0").ToString("N");
                    LbldispsimpTotal.Text = Convert.ToDouble((Convert.ToInt32(sal.Basic) + Convert.ToInt32(sal.HR) + Convert.ToInt32(sal.Utilities) + Convert.ToInt32(sal.NSHA) + Convert.ToInt32(sal.SplAlow) + Convert.ToInt32(sal.MedBil) + Convert.ToDouble(Lbldispsal_Arr.Text) + Convert.ToInt32(sal.EDA) + Convert.ToInt32(sal.PSMInc) + Convert.ToInt32(sal.MobAlow) + Convert.ToInt32(sal.ExpClaim) + Convert.ToInt32(sal.SIMInc) + Convert.ToDouble(LbldispFeul_Alownce.Text) + Convert.ToDouble(LbldispOthers.Text)).ToString()).ToString("N");

                    Lbldisplwop.Text = Convert.ToDouble((sal.LWOP == null ? 0 : sal.LWOP).ToString()).ToString("N");
                    LbldispMob_exces_limit.Text = Convert.ToDouble((sal.MobDed == null ? 0 : sal.MobDed).ToString()).ToString("N");
                    LbldispMOb_Device.Text = Convert.ToDouble((sal.ShopingDed == null ? 0 : sal.ShopingDed).ToString()).ToString("N");
                    LbldispEOBI.Text = Convert.ToDouble((sal.EOBIDed == null ? 0 : sal.EOBIDed).ToString()).ToString("N");
                    LbldispOther.Text = Convert.ToDouble((sal.OtrDed == null ? 0 : sal.OtrDed).ToString()).ToString("N");
                    LbldispIncome_Tax.Text = Convert.ToDouble((sal.TaxDed == null ? 0 : sal.TaxDed).ToString()).ToString("N");
                    LbldispdedTotal.Text = Convert.ToDouble((Convert.ToInt32(sal.LWOP) + Convert.ToInt32(sal.MobDed) + Convert.ToInt32(sal.ShopingDed) + Convert.ToInt32(sal.EOBIDed) + Convert.ToInt32(sal.OtrDed) + Convert.ToInt32(sal.TaxDed)).ToString()).ToString("N");

                    LblDispMonthDays.Text = Convert.ToDouble((Convert.ToInt32(sal.PDays) + Convert.ToInt32(sal.LWOP)).ToString()).ToString("N");
                    LblDispWorkedDays.Text = Convert.ToDouble((sal.PDays == null ? 0 : sal.PDays).ToString()).ToString("N");
                    LbldispNetPay.Text = Convert.ToDouble((Convert.ToDouble(LbldispsimpTotal.Text) - Convert.ToDouble(LbldispdedTotal.Text)).ToString()).ToString("N");

                    main_area.Visible = true;

                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ErrorinSalary").ToString(), RMS.BL.Enums.MessageType.Info);
                }
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "WrongEmpCode").ToString(), RMS.BL.Enums.MessageType.Info);
            }

        }

    }
}

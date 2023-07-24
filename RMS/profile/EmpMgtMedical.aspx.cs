using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;

namespace RMS.Setup
{
    public partial class EmpMgtMedical : BasePage
    {

        #region DataMembers
        //RMS.BL.tblAppEmp usr;
        PlExpenseBL expmgr = new PlExpenseBL();
        GroupBL groupManager = new GroupBL();
        PlAllowBL allowBL = new PlAllowBL();
        //EmpBL empBL = new EmpBL();

        ListItem selList = new ListItem();
        ListItem selListSub = new ListItem();

        #endregion

        #region Properties
        public int EmpIDUC
        {
            get { return (ViewState["EmpIDUC"] == null) ? 0 : Convert.ToInt32(ViewState["EmpIDUC"]); }
            set { ViewState["EmpIDUC"] = value; }
        }
#pragma warning disable CS0114 // 'EmpMgtMedical.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpMgtMedical.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        public string EffDateStr
        {
            get { return (ViewState["EffDateStr"] == null) ? "" : Convert.ToString(ViewState["EffDateStr"]); }
            set { ViewState["EffDateStr"] = value; }
        }

        public string TypeID
        {
            get { return (ViewState["TypeID"] == null) ? "" : Convert.ToString(ViewState["TypeID"]); }
            set { ViewState["TypeID"] = value; }
        }

        public string Year
        {
            get { return (ViewState["Year"] == null) ? "" : Convert.ToString(ViewState["Year"]); }
            set { ViewState["Year"] = value; }
        }
        public string ExpRef
        {
            get { return (ViewState["ExpRef"] == null) ? "" : Convert.ToString(ViewState["ExpRef"]); }
            set { ViewState["ExpRef"] = value; }
        }
        public string ExpAprovby
        {
            get { return (ViewState["ExpAprovby"] == null) ? "" : Convert.ToString(ViewState["ExpAprovby"]); }
            set { ViewState["ExpAprovby"] = value; }
        }

        #endregion

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpExpenseSetup").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                string dateformat = "dd-MMM-yy";
                if (Session["CompID"] == null)
                {
                    CompID = int.Parse(Request.Cookies["uzr"]["CompID"].ToString());
                }
                else
                {
                    CompID = int.Parse(Session["CompID"].ToString());
                }
                if (Session["DateFormat"] == null)
                {
                    txtEffDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                }
                else
                {
                    txtEffDateCal.Format = Session["DateFormat"].ToString();
                }
                txtEffDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(dateformat); 
                FillDropDownPaymentType();
                //BindGrid("", 0, 0);
                divgrdEmps.Visible = false;
                ucButtons.ValidationGroupName = "main";
                //EmpSrchUC.
                this.Focus();
            }
        }

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    //BindGrid(txtFltEmp.Text.Trim(), Convert.ToInt32(ddlFltRegion.SelectedValue), Convert.ToInt32(ddlFltSegment.SelectedValue));
        //}

        protected void grdEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ClearFields();
            ID = Convert.ToInt32(grdEmps.SelectedDataKey.Values["EmpID"].ToString());
            CompID = Convert.ToInt32(grdEmps.SelectedDataKey.Values["CompID"].ToString());
            //EffDateStr = grdEmps.SelectedDataKey.Values["EffDate"].ToString();
            TypeID = grdEmps.SelectedDataKey.Values["ExpTypeID"].ToString();
            Year = grdEmps.SelectedDataKey.Values["ExpYear"].ToString();
            ExpRef = grdEmps.SelectedDataKey.Values["ExpRef"].ToString();
            ExpAprovby = grdEmps.SelectedDataKey.Values["ExpAprovby"].ToString();
            this.GetByID();

        }

        protected void grdEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEmps.PageIndex = e.NewPageIndex;

            //BindGrid(txtFltEmp.Text.Trim(), Convert.ToInt32(ddlFltRegion.SelectedValue), Convert.ToInt32(ddlFltSegment.SelectedValue));
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                //pnlMain.Enabled = true;
            }
            else if (e.CommandName == "Save")
            {
                if (ddlPayType.SelectedIndex != 0)
                {
                    if (ID == 0)
                    {
                        this.Insert();
                        //pnlMain.Enabled = false;
                        ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                        divgrdEmps.Visible = false;
                        //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);

                    }
                    else
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "expAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);

                        //this.Update();
                        ////pnlMain.Enabled = false;
                        //divgrdEmps.Visible = false;
                        //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                        //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);

                    }
                }
                else
                {
                    ucMessage.ShowMessage("Payment Type Required", RMS.BL.Enums.MessageType.Info);
                }
            }
            else if (e.CommandName == "Delete")
            {
                // TRANSACTION WALA KAAM KARNA HAI......

                try
                {
                    this.Delete(ID);
                    //pnlMain.Enabled = false;
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
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
                BindGrid(EmpIDUC);
                ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                //pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
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
        protected void BindGrid(int empID)
        {
            this.grdEmps.DataSource = expmgr.GetAll(empID,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdEmps.DataBind();
        }

        private void FillDropDownPaymentType()
        {
            ddlPayType.DataTextField = "ExpTypeDesc";
            ddlPayType.DataValueField = "ExpTypeID";
            ddlPayType.DataSource = allowBL.GetAllPaymentTypeCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlPayType.DataBind();
        }

        protected void GetYTD(int empID)
        {
            try
            { 
                if (ddlPayType.SelectedValue != "MED")
                    return;

                tblCompany cmp = new CompanyBL().GetByID(CompID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                tblPlAlow alw = new PlAllowBL().GetByID(CompID, empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                DateTime dtMed;
                DateTime dtYr = new DateTime(int.Parse(cmp.FinYearStart.Value.ToString().Substring(0, 4)),
                    int.Parse(cmp.FinYearStart.Value.ToString().Substring(4, 2)),
                    int.Parse(cmp.FinYearStart.Value.ToString().Substring(6, 2)));
                DateTime.TryParse(txtEffDate.Text, out dtMed);
                Decimal ytdamt = 0;
                tblPlExpClaim clm = new PlExpenseBL().GetclaimByID(CompID, empID, ddlPayType.SelectedValue, dtMed.Year.ToString(), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                //check if date is in current financial year
                int monthsApart = 12 * (dtMed.Year - dtYr.Year) + dtMed.Month - dtYr.Month;
                ytdamt = (alw.Basic / 12) * Math.Abs(monthsApart);
                txtLimYtd.Text = ytdamt.ToString("#,#0");
                if (clm == null)
                {
                    txtClaimYtd.Text = "0";
                    this.txtClaimYtdOrg.Text = "0";
                    txtOv.Text = "0";
                }
                else
                {
                    txtClaimYtd.Text = clm.ExpRcvdYTD.Value.ToString("#,#0");
                    this.txtClaimYtdOrg.Text = clm.ExpRcvdYTD.Value.ToString("#,#0");
                    txtOv.Text = (clm.ExpRcvdYTD.Value > ytdamt ? clm.ExpRcvdYTD.Value - ytdamt : 0).ToString("#,#0");
                }
            }
            catch { }
        }

        protected void GetByID()
        {
            //tblPlAlow empPojo = expmgr.GetEmpByID(CompID, ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            tblPlExpClaim exp = expmgr.GetclaimByID(CompID, ID, TypeID, Year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            tblPlExpClaimDet detail = expmgr.GetClaimDetailByID(CompID, ID, TypeID, Year, ExpRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //this.txtEmpCode.Text = empPojo.EmpID.ToString();
            //this.txtFullName.Text = empPojo.FirstName + ' ' + empPojo.MidName + ' ' + empPojo.SirName;
            if (exp.tblPlEmpData.tblPlCode1 != null)
                this.EditModeDataShow(exp.tblPlEmpData.FullName,
                    exp.tblPlEmpData.EmpCode,
                    exp.tblPlEmpData.tblPlCode1.CodeDesc.ToString(),
                    exp.tblPlEmpData.tblPlCode.CodeDesc.ToString());
            else
                this.EditModeDataShow(exp.tblPlEmpData.FullName,
                    exp.tblPlEmpData.EmpCode,
                    "",
                    "");

            ddlPayType.SelectedValue = exp.ExpTypeID.ToString();
            if (Session["DateFormat"] == null)
            {
                txtEffDate.Text = Convert.ToDateTime(detail.ExpDate.ToString()).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
            }
            else
            {
                txtEffDate.Text = Convert.ToDateTime(detail.ExpDate.ToString()).ToString(Session["DateFormat"].ToString());
            }


            txtClaimRefNo.Text = detail.ExpRef;
            txtClaimAmt.Text = detail.ExpClaim.ToString();
            txtOvLimApp.Text = detail.ExpAproved.ToString();
            txtAppBy.Text = detail.ExpAprovedBy.ToString();
 
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void txtEffDate_TextChanged(object sender, EventArgs e)
        {
            GetYTD(EmpIDUC);
        }

        protected void Update()
        {
            tblPlExpClaimDet detail = expmgr.GetClaimDetailByID(CompID, ID, TypeID, Year, ExpRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            detail.ExpClaim = Convert.ToDecimal(txtClaimAmt.Text);
            detail.ExpAproved = Convert.ToDecimal(txtOvLimApp.Text);
            detail.ExpDate = Convert.ToDateTime(txtEffDate.Text);
            detail.ExpAprovedBy = txtAppBy.Text;
            detail.ExpPayDate = txtEffDateCal.SelectedDate;
            detail.ExpStatus = "1";
            detail.ExpPaidAmt = Convert.ToDecimal(txtOv.Text == "" ? "0" : txtOv.Text);

            //expmgr.Update(detail, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
            //BindGrid(detail.EmpID);
            //ClearFields();
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
            if (Convert.ToInt32(txtClaimAmt.Text) >= Convert.ToInt32(txtOvLimApp.Text == "" ? "0" : txtOvLimApp.Text))
            {
                //tblPlExpClaim exp = new tblPlExpClaim();
                tblPlExpClaimDet detail = new tblPlExpClaimDet();

                if (Session["CompID"] == null)
                {
                    detail.CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                }
                else
                {
                    detail.CompID = Convert.ToByte(Session["CompID"].ToString());
                }

                detail.EmpID = EmpIDUC;// Convert.ToInt32(txtEmpCode.Text.Trim());
                detail.ExpTypeID = ddlPayType.SelectedValue.ToString();
                detail.ExpYear = Convert.ToDecimal("2011");
                detail.ExpRef = txtClaimRefNo.Text;
                
                detail.ExpClaim = Convert.ToDecimal(txtClaimAmt.Text);
                detail.ExpAproved = Decimal.Parse(txtPaidAmt.Text);
                detail.ExpPaidAmt = Convert.ToDecimal(txtPaidAmt.Text);

                //if (txtOvLimApp.Text != "")
                //    detail.ExpAproved = Convert.ToDecimal(txtOvLimApp.Text);
                //else
                //    detail.ExpAproved = Convert.ToDecimal(txtClaimAmt.Text);

                detail.ExpDate = Convert.ToDateTime(txtEffDate.Text);
                detail.ExpAprovedBy = txtAppBy.Text;
                detail.ExpPayDate = Convert.ToDateTime(txtEffDate.Text);
                detail.ExpStatus = "1";

                //tblPlExpClaimDet ext = new tblPlExpClaimDet();
                //if (!expmgr.ISAlreadyExist(detail.CompID, detail.EmpID, detail.ExpTypeID, detail.ExpYear, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                //{
                expmgr.Insert(detail, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid(detail.EmpID);
                ClearFields();
                //}
                //else
                //{
                //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                    //pnlMain.Enabled = true;
                //}
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "AprovedGraterThenClaim").ToString(), RMS.BL.Enums.MessageType.Error);
            }
        }

        private void ClearFields()
        {
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            //txtEffDate.Text = "";
            txtClaimRefNo.Text = "";
            txtClaimAmt.Text = "";
            txtAppBy.Text = "";
            txtOvLimApp.Text = "";
            txtLimYtd.Text = "";
            txtClaimYtd.Text = "";
            this.txtClaimYtdOrg.Text = "0";
            txtOv.Text = "";
            ddlPayType.SelectedIndex=0;
            grdEmps.SelectedIndex = -1;
            this.ClearFieldsUC();
            this.EditModeDataHide();
            this.Focus();
            divgrdEmps.Visible=false;
        }


        #endregion
        
        #region search

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridUC();
        }

        private void BindGridUC()
        {
            if (!txtEmpSrch.Text.Trim().Equals(""))
            {
                grdEmpSrchUC.Visible = true;
                divEmpInfo.Visible = false;
                //lblCUstomerName.Visible = false;
                //lblCustomer.Visible = false;

                //int orderID, stateID;
                //string plateCode, plateNo;
                //int.TryParse(txtOrder.Text, out orderID);
                //int.TryParse(ddlState.SelectedValue, out stateID);

                //plateCode = ddlPlateCode.SelectedValue.Equals("Qala") ? "0" : ddlPlateCode.SelectedValue;

                //plateNo = txtPlateNum.Text.Trim() == "" ? "0" : txtPlateNum.Text;

                this.grdEmpSrchUC.DataSource = new EmpBL().GetAllSearch(txtEmpSrch.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                this.grdEmpSrchUC.DataBind();
            }
            else
            {
                this.grdEmpSrchUC.DataSource = null;
                this.grdEmpSrchUC.DataBind();
            }
            //TextBox txt = (TextBox)this.Page.Page.FindControl("txtTest");

            ////this.Page.Page.FindControl("txtTest");
            //Response.Write(txt.Text);

        }
        protected void grdEmpSrchUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                EmpIDUC = Convert.ToInt32(grdEmpSrchUC.SelectedDataKey.Value);
                txtEmpSrch.Text = grdEmpSrchUC.SelectedRow.Cells[1].Text;
                //UCvehicleID = Convert.ToInt32(grdSearch.SelectedRow.Cells[0].Text);
                lblEmpName.Text = "Name: " + grdEmpSrchUC.SelectedRow.Cells[1].Text; //grdEmpSrchUC.SelectedRow.Cells[0].Text;
                lblEmpCode.Text = "Emp No: " + grdEmpSrchUC.SelectedRow.Cells[0].Text;
                lblEmpDesig.Text = "Designation: " + grdEmpSrchUC.SelectedRow.Cells[2].Text;
                lblEmpDept.Text = "Deptartment: " + grdEmpSrchUC.SelectedRow.Cells[3].Text;
                divEmpInfo.Visible = true;
                //lblCustomer.Visible = true;
                //txtVehIDAcci.Value = UCvehicleID.ToString();
                grdEmpSrchUC.Visible = false;
                BindGrid(EmpIDUC);
                GetYTD(EmpIDUC);
                
                //txtStartDate.Enabled = true;
                //leavegrd.Visible = true;
                divgrdEmps.Visible=true;
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }
        public void ClearFieldsUC()
        {
            txtEmpSrch.Text = "";
            EmpIDUC = 0;
            divEmpInfo.Visible = false;
        }

        public void EditModeDataShow(string EmpName, string EmpCode, string EmpDesig, string EmpDept)
        {
            lblEmpName.Text = "Name: " + EmpName;
            lblEmpCode.Text = "Emp No: " + EmpCode;
            lblEmpDesig.Text = "Designation: " + EmpDesig;
            lblEmpDept.Text = "Department: " + EmpDept;

            divEmpInfo.Visible = true;
            lblEmpSrch.Visible = false;
            txtEmpSrch.Visible = false;
            btnSearch.Visible = false;
            grdEmpSrchUC.Visible = false;
        }
        public void EditModeDataHide()
        {
            divEmpInfo.Visible = false;
            lblEmpSrch.Visible = true;
            txtEmpSrch.Visible = true;
            btnSearch.Visible = true;
        }
        public void SetTitle(string title)
        {
            txtEmpSrch.Text = title;
        }
        public string GetTitle()
        {
            return txtEmpSrch.Text;
        }

        public void HideEmpInfo()
        {
            divEmpInfo.Visible = false;
        }
        public void HideAll()
        {
            divEmpInfo.Visible = false;
            lblEmpSrch.Visible = false;
            txtEmpSrch.Visible = false;
            btnSearch.Visible = false;
        }
        public void ShowAll()
        {
            divEmpInfo.Visible = true;
            lblEmpSrch.Visible = true;
            txtEmpSrch.Visible = true;
            btnSearch.Visible = true;
        }

        #endregion

        protected void ddlPayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetYTD(EmpIDUC);
        }
    }
}
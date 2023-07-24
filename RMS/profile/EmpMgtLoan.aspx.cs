using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Data.Linq;

namespace RMS.Setup
{
    public partial class EmpMgtLoan : BasePage
    {

        #region DataMembers
        //RMS.BL.tblAppEmp usr; 
        RMSDataContext db = new RMSDataContext();
        GroupBL groupManager = new GroupBL();
        PlLoanBL allowBL = new PlLoanBL();
        EmpProfRptBL empProfBl = new EmpProfRptBL();
        voucherDetailBL objVoucher = new voucherDetailBL();
        PreferenceBL prefBl = new PreferenceBL();
        EmpBL empBL = new EmpBL();
        PlCodeBL codeBl = new PlCodeBL();
        ListItem selList = new ListItem();
        ListItem selListSub = new ListItem();
        tblPlCPFAdvance adv = new tblPlCPFAdvance();
        tblPlProvidentFund pf = new tblPlProvidentFund();

        //public int LoanTypeID;
        //public string PaymentRef;

        EntitySet<Glmf_Data_Det> enttyGlDet = new EntitySet<Glmf_Data_Det>();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'EmpMgtLoan.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpMgtLoan.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int UserID
        {
            get { return (ViewState["UserID"] == null) ? 0 : Convert.ToInt32(ViewState["UserID"]); }
            set { ViewState["UserID"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        public int EmpIDUC
        {
            get { return (ViewState["EmpIDUC"] == null) ? 0 : Convert.ToInt32(ViewState["EmpIDUC"]); }
            set { ViewState["EmpIDUC"] = value; }
        }
        public int BrID
        {
            get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
        }
        public int LoanID
        {
            get { return (ViewState["LoanID"] == null) ? 0 : Convert.ToInt32(ViewState["LoanID"]); }
            set { ViewState["LoanID"] = value; }
        }
        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        public string LoanTypeID
        {
            get { return (ViewState["LoanTypeID"] == null) ? "" : ViewState["LoanTypeID"].ToString(); }
            set { ViewState["LoanTypeID"] = value; }
        }

        public string PaymentRef
        {
            get { return (ViewState["PaymentRef"] == null) ? "" : Convert.ToString(ViewState["PaymentRef"]); }
            set { ViewState["PaymentRef"] = value; }
        }
        public string CurPayPeriod
        {
            get { return (ViewState["CurPayPeriod"] == null) ? "" : Convert.ToString(ViewState["CurPayPeriod"]); }
            set { ViewState["CurPayPeriod"] = value; }

        }
        public int VrID
        {
            get { return (ViewState["VrID"] == null) ? 0 : Convert.ToInt32(ViewState["VrID"]); }
            set { ViewState["VrID"] = value; }
        }

        private string vrRef;
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                BindGrid();
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpLoanSetup").ToString();
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                if (Session["DateFormat"] == null)
                {
                    txtEffDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtChqDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtOvLimAppCal.Format = Request.Cookies["uzr"]["DateFormat"];

                }
                else
                {
                    txtEffDateCal.Format = Session["DateFormat"].ToString();
                    txtChqDateCal.Format = Session["DateFormat"].ToString();
                    txtOvLimAppCal.Format = Session["DateFormat"].ToString();
                }
                if (Session["UserID"] == null)
                {
                    UserID = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                }
                else
                {
                    UserID = Convert.ToInt32(Session["UserID"].ToString());
                }
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
                    BranchID = Convert.ToByte(Session["BranchID"].ToString());
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
               // btnDelete.Visible = false;
                FillDropDownLoanType();
               // ucButtons.ValidationGroupName = "main";
                
                txtClaimAmt.Style["text-align"] = "right";
                txtOv.Style["text-align"] = "right";

                if (ddlAppStatus.SelectedValue == "P")
                {
                    RequiredFieldValidator8.Enabled = false;
                    RequiredFieldValidator9.Enabled = false;
                    RequiredFieldValidator10.Enabled = false;
                }
                FillSearchBranchDropDown();
                FillJobeTypeDrpdown();
                FillSearchDropDownEmployee();
                //BranchID =Convert.ToInt32(searchBranchDropDown.SelectedValue);
                searchBranchDropDown.SelectedValue = BranchID.ToString();
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
                    ClearFields();
                }
                else
                {
                    ucMessage.ShowMessage("Could not delete loan as exception occurred", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch
            { }

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            //if (EmpSrchUC.EmpBindGrid.Equals("Yes"))
            //{
            //    EmpSrchUC.EmpBindGrid = "No";
            //    ID = EmpSrchUC.EmpIDUC;
            //    BindGrid();
                
            //}
            base.OnLoadComplete(e);
        }

        protected void grdLoans_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdLoans.SelectedValue);
            tblPlCPFAdvance CPF = new tblPlCPFAdvance();
            CPF = db.tblPlCPFAdvances.Where(x => x.CPFID == ID).FirstOrDefault();
            searchBranchDropDown.SelectedValue = CPF.BranchID.ToString();
            ddlJobType.SelectedValue = CPF.JobTypeID.ToString();
            ddlEmployeeSearch.SelectedValue = CPF.EmpID.ToString();
            ddlPayType.SelectedValue = CPF.Paytype.ToString();
            ddlLoanType.SelectedValue = CPF.LoanTypeID.ToString();
            txtClaimAmt.Text = CPF.AdvanceAmount.ToString();
            txtEffDate.Text = CPF.AdvanceDate.ToString();
            txtLimYtd.Text = CPF.NoOfInst.ToString();
            txtOv.Text = CPF.DedAmount.ToString();
            txtOvLimApp.Text = CPF.DedDate.ToString();
            ddlAppStatus.SelectedValue = CPF.Advstatus.ToString();
            
            if (CPF.Advstatus.ToString() == "A")
            {
                lblBranch.Visible = true;
                lblAccount.Visible = true;
                lblChqNo.Visible = true;
                lblChqDate.Visible = true;
                
                txtChqBranch.Text = CPF.BankBranch.ToString();
                txtChqAcctNo.Text = CPF.AccountNo.ToString();
                txtChqNo.Text = CPF.ChqNo.ToString();
                txtChqDate.Text = CPF.ChqDate.ToString();
            }
            else
            {
                lblBranch.Visible = false;
                lblAccount.Visible = false;
                lblChqNo.Visible = false;
                lblChqDate.Visible = false;

            }

            if (Session["DateFullYearFormat"] == null)
            {
                txtEffDate.Text = CPF.AdvanceDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                txtChqDate.Text = CPF.ChqDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                var ded = Convert.ToDateTime(CPF.DedDate);
                txtOvLimApp.Text = ded.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            else
            {
                this.txtEffDate.Text = CPF.AdvanceDate.Value.ToString(Session["DateFullYearFormat"].ToString());
                this.txtChqDate.Text = CPF.ChqDate.Value.ToString(Session["DateFullYearFormat"].ToString());
                var dded = Convert.ToDateTime(CPF.DedDate);
                this.txtOvLimApp.Text = dded.ToString(Session["DateFullYearFormat"].ToString());
            }


        }

        protected void grdLoans_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLoans.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
               // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                // pnlMain.Enabled = true;
            }
            else if (e.CommandName == "Save")
            {
                if (ID < 1)
                {
                    ucMessage.ShowMessage("Please select Employee", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                if (ddlLoanType.SelectedIndex != 0)
                {
                    if (PaymentRef.Equals(""))
                    {
                        //this.Insert();
                       // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    }
                    else
                    {
                        this.Update();
                       // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Loan Type Required", RMS.BL.Enums.MessageType.Info);
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
                //BindGrid();
                ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
               // ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }
        
        protected void grdLoans_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //string strBPNo = e.Row.Cells[0].Text.Substring(3);
                //try
                //{
                //    if (Convert.ToInt32(strBPNo) > 0)
                //    {
                //        Glmf_Data glmfdata = objVoucher.GetGlmf_Data(Convert.ToInt32(strBPNo), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                //        e.Row.Cells[0].Text = "IP-" + glmfdata.vr_no;
                //    }
                //}
                //catch { }

                if (Session["DateFormat"] == null)
                {
                    
                        e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
                    
                }
                else
                {
                        e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text.ToString()).ToString(Session["DateFormat"].ToString());
                  
                }

                //if (e.Row.Cells[6].Text.Equals("A"))
                //{
                //    e.Row.Cells[7].Text = "------";
                //}

                if (e.Row.Cells[7].Text.Equals("A"))
                {
                    e.Row.Cells[7].Text = "Approved";
                }
                else
                {
                    e.Row.Cells[7].Text = "Pending";
                }

                
            }
        }

        protected void ddlJobtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlJobType.SelectedValue.Equals("0"))
                {
                    int jobtype = Convert.ToInt32(ddlJobType.SelectedValue.Trim());
                    //RMSDataContext db = new RMSDataContext();
                    ddlEmployeeSearch.Controls.Clear();
                    ddlEmployeeSearch.Dispose();
                    ddlEmployeeSearch.DataTextField = "FullName";
                    ddlEmployeeSearch.DataValueField = "EmpID";
                    ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.JobNameID == jobtype && x.BranchID == BranchID).ToList();
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



        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    RMSDataContext db = new RMSDataContext();
                    ddlEmployeeSearch.Controls.Clear();
                    ddlEmployeeSearch.Dispose();
                    ddlEmployeeSearch.DataTextField = "FullName";
                    ddlEmployeeSearch.DataValueField = "EmpID";
                    ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
                    ddlEmployeeSearch.DataBind();
                    ddlEmployeeSearch.Items.Insert(0, new ListItem("Select Employee", "0"));

                    //FillSearchDropDownEmployee();


                }

            }
            catch
            { }
        }
        #endregion

        #region Helping Method

        protected void FillSearchDropDownEmployee()
        {

            ddlEmployeeSearch.DataTextField = "FullName";
            ddlEmployeeSearch.DataValueField = "EmpID";
            ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1).ToList();
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
            ddlJobType.DataSource = db.JobTypeNames.Where(x => x.IsActive == true).ToList();
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
        protected void BindGrid()
        {
            this.grdLoans.DataSource = from loan in db.tblPlCPFAdvances
                                       join type in db.tblPlLoanTypes on loan.LoanTypeID equals type.LoanTypeID
                                       where loan.IsActive == true
                                       orderby loan.CPFID descending
                                       select new
                                       {
                                           loan.tblPlEmpData.EmpID,
                                           type.LoanTypeID,
                                           loan.CPFID,
                                           loan.tblPlEmpData.FullName,
                                           loan.JobTypeName,
                                           loan.Branch.br_nme,
                                           loan.Paytype,
                                           loan.NoOfInst,
                                           loan.AdvanceAmount,
                                           loan.AdvanceDate,
                                           loan.DedAmount,
                                           loan.Advstatus,
                                           loan.BankBranch,
                                           loan.ChqNo,
                                           loan.IsActive
                                       };
            this.grdLoans.DataBind();
        }
        
        private void FillDropDownLoanType()
        {
            ddlLoanType.DataTextField = "LoanTypeDesc";
            ddlLoanType.DataValueField = "LoanTypeID";
            ddlLoanType.DataSource = allowBL.GetAllLoanTypeCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoanType.DataBind();
        }

        protected void GetByID()
        {
            tblPlLoan lon = allowBL.GetByID(CompID, ID, LoanID, LoanTypeID, PaymentRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlPayType.SelectedValue = lon.PaymentType;
            ddlAppStatus.SelectedValue = lon.LoanAppStatus;
            ddlLoanType.SelectedValue = lon.LoanTypeID.ToString();
            //txtClaimRefNo.Text = lon.PaymentRef.ToString();
            txtClaimAmt.Text = lon.LoanAmt.ToString();
            txtLimYtd.Text = lon.NoOfInst.ToString();
            //txtRefDate.Text = lon.PaymentDate.Value.ToString("dd-MMM-yyyy");
            txtEffDate.Text = lon.InstStartDate.Value.ToString("dd-MMM-yyyy");
            txtOv.Text = lon.InstAmt.ToString();
            if (lon.LoanPaidBack > 0)
                txtOvLimApp.Text = lon.LoanPaidBack.ToString();
            else
                txtOvLimApp.Text = "";
            ddlPayType.SelectedValue = lon.PaymentType;
            int curPayPeriod = 0;
            int.TryParse(CurPayPeriod, out curPayPeriod);
            int effDatePeriod = 0;
            int.TryParse(Convert.ToDateTime(txtEffDate.Text).ToString("yyyyMM"), out effDatePeriod);

            if (effDatePeriod >= curPayPeriod)
            {
               // btnDelete.Visible = true;
            }
            if (lon.LoanAppStatus == "A")
            {
                ddlPayType.Enabled = false;
                ddlLoanType.Enabled = false;
                ddlAppStatus.Enabled = false;
                txtClaimAmt.Enabled = false;
                txtEffDate.Enabled = false;
            }
            //ddlAppStatus.SelectedValue = lon.LoanAppStatus.ToString();

            ////Cheque Info
            //try
            //{
            //    string pamentRef = lon.PaymentRef;
            //    pamentRef = pamentRef.Substring(3, 6);

            //    VrID = Convert.ToInt32(pamentRef);

            //    Glmf_Data_chq glChq = objVoucher.GetCheqDetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    if (glChq != null)
            //    {
            //        txtChqBranch.Text = glChq.vr_chq_branch + " - " + objVoucher.GetGlmfCodeByID(glChq.vr_chq_branch.Trim(), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
            //        hdnGlCode.Value = glChq.vr_chq_branch;
            //        txtChqAcctNo.Text = glChq.vr_chq_ac;
            //        txtChqNo.Text = glChq.vr_chq;
            //        txtChqDateCal.SelectedDate = glChq.vr_chq_dt;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            //    return;
            //}
            //ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update()
        {
            tblPlLoan lon = allowBL.GetByID(CompID, ID, LoanID, LoanTypeID, PaymentRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //*************************************************************
            if (lon.LoanAppStatus != "A")
            {

                try
                {
                    if (ddlAppStatus.SelectedValue.Equals("A"))
                    {
                        Glmf_Data_chq chq = new Glmf_Data_chq();
                        chq.vr_chq_branch = hdnGlCode.Value;
                        chq.vr_chq = txtChqNo.Text;
                        if (RMS.BL.BranchBL.IsChequeNoExists(chq, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                        {
                            ucMessage.ShowMessage("Cheque no already exists", RMS.BL.Enums.MessageType.Error);
                            txtChqNo.Focus();
                            chq = null;
                            return;
                        }
                        chq = null;
                    }
                }
                catch (Exception ex)
                {
                    ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
                    return;
                }
                
                decimal Financialyear = 0;
                if (ddlAppStatus.SelectedValue == "A" && ddlPayType.SelectedValue == "Cheque")
                {
                    try
                    {
                        Convert.ToDateTime(txtChqDate.Text.Trim());
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Invalid cheque date", RMS.BL.Enums.MessageType.Error);
                        txtChqDate.Focus();
                        return;
                    }


                    if (Convert.ToDateTime(txtChqDate.Text.Trim()) >= Convert.ToDateTime(txtEffDate.Text.Trim()))
                    {
                        ucMessage.ShowMessage("Cheque date should be less than effective date", RMS.BL.Enums.MessageType.Error);
                        return;
                    }

                    Financialyear = objVoucher.GetFinancialYearByDate(Convert.ToDateTime(txtEffDate.Text.Trim()), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    DateTime vrDate = Convert.ToDateTime(txtChqDate.Text.Trim());
                    DateTime dtFrm = DateTime.Parse("01-Jul-" + Financialyear);
                    dtFrm = dtFrm.AddYears(-1);
                    DateTime dtTo = DateTime.Parse("30-Jun-" + Financialyear);
                    if (vrDate >= dtFrm && vrDate <= dtTo)
                    {
                        // ok
                    }
                    else
                    {
                        ucMessage.ShowMessage("Cheque Date should be within this financial year i.e. " + Financialyear, RMS.BL.Enums.MessageType.Error);
                        txtChqDate.Focus();
                        return;
                    }
                }


                try
                {
                    Convert.ToDateTime(txtEffDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage("Invalid effective date", RMS.BL.Enums.MessageType.Error);
                    txtEffDate.Focus();
                    return;
                }

                try
                {
                    Convert.ToByte(txtLimYtd.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage("Invalid installment size", RMS.BL.Enums.MessageType.Error);
                    txtLimYtd.Focus();
                    return;
                }


                decimal paidBackAmount = 0;
                if (!string.IsNullOrEmpty(txtOvLimApp.Text))
                {
                    paidBackAmount = Convert.ToDecimal(txtOvLimApp.Text);
                }
                decimal claimAmount = Convert.ToDecimal(txtClaimAmt.Text) - paidBackAmount;
                decimal instAmount = Convert.ToDecimal(Convert.ToByte(txtLimYtd.Text.Trim())) * Convert.ToDecimal(txtOv.Text);

                if (claimAmount != instAmount)
                {
                    ucMessage.ShowMessage("Invalid installment size or amount", RMS.BL.Enums.MessageType.Error);
                    txtOv.Focus();
                    return;
                }

                bool IsSaved = false;
                if (ddlAppStatus.SelectedValue.Equals("A"))
                {
                    IsSaved = SaveIPV();

                    if (!IsSaved)
                    {
                        ucMessage.ShowMessage("Unable to save IPV at this time", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                }


                if (IsSaved)
                {
                    lon.PaymentRef = "PL-" + vrRef;
                    if (ddlAppStatus.SelectedValue == "A" && ddlPayType.SelectedValue == "Cheque")
                    {
                        lon.PaymentDate = Convert.ToDateTime(txtChqDate.Text.Trim());
                    }
                    else
                    {
                        lon.PaymentDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                }
                else
                {
                    lon.PaymentRef = "00-000000";
                    lon.PaymentDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                lon.LoanTypeID = ddlLoanType.SelectedValue;
                lon.LoanAmt = Convert.ToDecimal(txtClaimAmt.Text.Trim());
                lon.NoOfInst = Convert.ToByte(txtLimYtd.Text.Trim());
                lon.InstStartDate = Convert.ToDateTime(txtEffDate.Text.Trim());
                lon.InstAmt = Convert.ToDecimal(txtOv.Text);
                lon.LoanStatus = true;
                lon.LoanAppStatus = ddlAppStatus.SelectedValue;
                lon.PaymentType = ddlPayType.SelectedValue;

                if (Session["UserID"] == null)
                    lon.UpdateBy = Request.Cookies["uzr"]["UserID"].ToString();
                else
                    lon.UpdateBy = Session["UserID"].ToString();
                lon.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            else
            {
                decimal paidBackAmount = 0;
                if (!string.IsNullOrEmpty(txtOvLimApp.Text))
                {
                    paidBackAmount = Convert.ToDecimal(txtOvLimApp.Text);
                }
                decimal claimAmount = Convert.ToDecimal(txtClaimAmt.Text) - paidBackAmount;
                decimal instAmount = Convert.ToDecimal(Convert.ToByte(txtLimYtd.Text.Trim())) * Convert.ToDecimal(txtOv.Text);

                if (instAmount != 0 && claimAmount != instAmount)
                {
                    ucMessage.ShowMessage("Invalid installment size or amount", RMS.BL.Enums.MessageType.Error);
                    txtOv.Focus();
                    return;
                }
                lon.NoOfInst = Convert.ToByte(txtLimYtd.Text);
                lon.InstAmt = Convert.ToDecimal(txtOv.Text);

            }
            //*************************************************************
            

            allowBL.Update(lon, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
            BindGrid();
            ClearFields();

        }

        protected bool Delete()
        {
            tblPlLoan tblLoan = new tblPlLoan();
            tblLoan.CompID = Convert.ToByte(CompID);
            tblLoan.EmpID = ID;
            tblLoan.PaymentRef = PaymentRef;
            tblLoan.LoanTypeID = ddlLoanType.SelectedValue;

            return allowBL.DeleteLoan(tblLoan, VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        private string GetNarration()
        {
            List<spEmpBasicInfoResult> empInfo = empProfBl.GetEmpBasicInfo(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            string vrNarr = "";
            StringBuilder builder = new StringBuilder();
            foreach (var info in empInfo)
            {
                builder.Append("EN-" + info.EmpID.ToString().PadLeft(5, '0'));
                builder.Append(", ");
                builder.Append(info.FullName).Append(", ").AppendLine();
                //builder.Append(info.region + " (Region)");
                builder.Append(", ");
                builder.Append(info.department + " (Dept)");
                builder.Append(", ");
                builder.Append(info.Curretdesignation).Append(", ").AppendLine();
                builder.Append(ddlLoanType.SelectedItem.Text);
                builder.Append(", ");
                builder.Append("Rs. " + txtClaimAmt.Text);
            }
            vrNarr = builder.ToString();
            if (vrNarr.Length > 500)
            {
                vrNarr = vrNarr.Substring(0, 500);
            }
            return vrNarr;
        }

        private bool SaveIPV()
        {
            string vrNarr = GetNarration();

            string username = "";
            if (Session["LoginID"] == null)
            {
                username = Request.Cookies["uzr"]["LoginID"];
            }
            else
            {
                username = Session["LoginID"].ToString();
            }

            if (username.Length > 15)
            {
                username = username.Substring(0, 14);
            }

            //int voucherTypeId = 3;
            int voucherTypeId = 6;
            decimal Financialyear = objVoucher.GetFinancialYearByDate(Convert.ToDateTime(txtEffDate.Text.Trim()), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            string source = "PAY";
            int voucherno = objVoucher.GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], BranchID, voucherTypeId, Financialyear,source);

            
            Glmf_Data glmfdata = new Glmf_Data();
            glmfdata.br_id = BrID;
            glmfdata.Gl_Year = Financialyear;
            glmfdata.vt_cd = Convert.ToInt16(voucherTypeId);
            glmfdata.vr_no = voucherno;
            if (ddlAppStatus.SelectedValue == "A" && ddlPayType.SelectedValue == "Cheque")
            {
                glmfdata.vr_dt = Convert.ToDateTime(txtChqDate.Text.Trim()).Date;
            }
            else
            {
                //glmfdata.vr_dt = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                glmfdata.vr_dt = Convert.ToDateTime(txtEffDate.Text.Trim()).Date;
            }
            glmfdata.vr_nrtn = vrNarr;
            glmfdata.vr_apr = "A";
            glmfdata.updateby = username;
            glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            glmfdata.approvedby = username;
            glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            glmfdata.source = source;

            Glmf_Data_Det glDet1 = new Glmf_Data_Det();
            glDet1.vr_seq = 1;
            glDet1.gl_cd = hdnGlCode.Value;
            glDet1.vrd_debit = 0;
            glDet1.vrd_credit = Convert.ToDecimal(txtClaimAmt.Text);
            glDet1.vrd_nrtn = ddlLoanType.SelectedItem.Text + " Paid.";
            glDet1.cc_cd = null;

            enttyGlDet.Add(glDet1);

            Glmf_Data_Det glDet2 = new Glmf_Data_Det();
            glDet2.vr_seq = 2;
            //glDet2.gl_cd = prefBl.GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ctrl_Dept;
            tblPlEmpData employee = empBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            string glcd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscPayable + employee.EmpID.ToString().PadLeft(4,'0');
            //if (string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).LoansPayable))
            //{
            //    return false;
            //}
            //glDet2.gl_cd = codeBl.GetByID(Convert.ToInt32(empBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).LoansPayable;
            if (string.IsNullOrEmpty(glcd))
            {
                return false;
            }
            glDet2.gl_cd = glcd;
            glDet2.vrd_debit = Convert.ToDecimal(txtClaimAmt.Text);
            glDet2.vrd_credit = 0;
            glDet2.vrd_nrtn = ddlLoanType.SelectedItem.Text + " Paid.";
            glDet2.cc_cd = null;

            enttyGlDet.Add(glDet2);
            Glmf_Data_chq glmfChq = null ;
            bool saveChq = false;
            if (ddlAppStatus.SelectedValue == "A" && ddlPayType.SelectedValue == "Cheque")
            {
                glmfChq = new Glmf_Data_chq();
                glmfChq.vr_chq_branch = hdnGlCode.Value;
                glmfChq.vr_chq = txtChqNo.Text.Trim();
                glmfChq.vr_chq_dt = Convert.ToDateTime(txtChqDate.Text.Trim());
                glmfChq.vr_chq_ac = txtChqAcctNo.Text.Trim();
                saveChq = true;
            }
            string msg = objVoucher.SaveLoanAdvPaymentVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, glmfChq, saveChq, enttyGlDet);
            if (msg == "ok")
            {
                vrRef = glmfdata.vrid.ToString().PadLeft(6, '0');
                return true;
            }
            else
            {
                ucMessage.ShowMessage("Exception: " + msg, RMS.BL.Enums.MessageType.Error);
                return false;
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
                        adv.BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    }
                    if (ddlJobType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Job type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        adv.JobTypeID = Convert.ToInt32(ddlJobType.SelectedValue.Trim());
                    }
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        adv.EmpID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue.Trim());
                    }
                    if (ddlPayType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Pay type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        adv.Paytype = ddlPayType.SelectedValue.Trim();
                    }
                    if (ddlLoanType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select LoanType", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        adv.LoanTypeID = ddlLoanType.SelectedValue;
                    }
                    if (txtClaimAmt.Text == "")
                    {
                        ucMessage.ShowMessage("Advance amount is Required", BL.Enums.MessageType.Error);

                    }
                    else
                    {
                        adv.AdvanceAmount = Convert.ToInt32(txtClaimAmt.Text.Trim());
                    }
                    if (txtEffDate.Text == "")
                    {
                        ucMessage.ShowMessage("Effective Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        adv.AdvanceDate = Convert.ToDateTime(txtEffDate.Text.Trim());
                    }
                    if (txtLimYtd.Text == "")
                    {
                        ucMessage.ShowMessage("Installment Size is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        adv.NoOfInst = Convert.ToInt32(txtLimYtd.Text.Trim());
                    }
                    if (txtOv.Text == "")
                    {
                        ucMessage.ShowMessage("Deduction Amount is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        adv.DedAmount = Convert.ToInt32(txtOv.Text.Trim());
                    }
                    if (txtOvLimApp.Text == "")
                    {
                        ucMessage.ShowMessage("Deducted Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        adv.DedDate = txtOvLimApp.Text.Trim();
                    }
                    if (ddlAppStatus.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Status", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        adv.Advstatus = ddlAppStatus.SelectedValue.Trim();
                    }
                    if (checkIsActive.Checked == true)
                    {
                        adv.IsActive = true;
                    }
                    else
                    {
                        adv.IsActive = false;
                    }
                    adv.createdOn = DateTime.Now;
                    adv.createdby = UserID;

                    if (ddlAppStatus.SelectedValue == "A")
                    {
                      
                            var exit = db.tblPlCPFAdvances.Where(x => x.ChqNo == txtChqNo.Text.Trim()).FirstOrDefault();
                            if (exit != null)
                            {
                                ucMessage.ShowMessage("Cheque Number already Exit", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                if (txtChqNo.Text == "")
                                {
                                    ucMessage.ShowMessage("Cheque No is Required", BL.Enums.MessageType.Error);
                                    return;
                                }
                                else
                                {

                                    adv.ChqNo = txtChqNo.Text.Trim();

                                }
                                if (txtChqBranch.Text == "")
                                {
                                    ucMessage.ShowMessage("Branch is Required", BL.Enums.MessageType.Error);
                                    return;
                                }
                                else
                                {
                                    adv.BankBranch = txtChqBranch.Text.Trim();
                                }
                                if (txtChqAcctNo.Text == "")
                                {
                                    ucMessage.ShowMessage("Account No is Required", BL.Enums.MessageType.Error);
                                    return;
                                }
                                else
                                {
                                    adv.AccountNo = txtChqAcctNo.Text.Trim();
                                }
                                if (txtChqDate.Text == "")
                                {
                                    ucMessage.ShowMessage("Cheque Date Is required", BL.Enums.MessageType.Error);
                                    return;
                                }
                                else
                                {
                                    adv.ChqDate = Convert.ToDateTime(txtChqDate.Text.Trim());
                                }
                            db.tblPlCPFAdvances.InsertOnSubmit(adv);
                            db.SubmitChanges();
                            pf = db.tblPlProvidentFunds.Where(x => x.EmpID == Convert.ToInt32(ddlEmployeeSearch.SelectedValue)).OrderByDescending(x => x.PfID).Take(1).Single();
                            if (pf != null)
                            {
                                tblPlProvidentFund pfPro = new tblPlProvidentFund();
                                pfPro.CPFID = adv.CPFID;
                                var close = pf.closeBlnc;
                                pfPro.OpenBlnc = Convert.ToDecimal(close);
                                pfPro.type = 2;
                                pfPro.createdon = DateTime.Now;
                                pfPro.createdBy = Session["UserID"].ToString();
                                pfPro.closeBlnc = close - Convert.ToDecimal(txtClaimAmt.Text);
                                pfPro.EmpID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                                db.tblPlProvidentFunds.InsertOnSubmit(pfPro);
                                
                                db.SubmitChanges();
                            }

                        }
                    }
                    else
                    {

                    }

                   
                    ucMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);

                }
                else
                {
                    tblPlCPFAdvance cpfAd = db.tblPlCPFAdvances.Where(x => x.CPFID == ID).FirstOrDefault();
                    if (searchBranchDropDown.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Branch", BL.Enums.MessageType.Error);
                        return;

                    }
                    else
                    {
                        cpfAd.BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    }
                    if (ddlJobType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Job type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cpfAd.JobTypeID = Convert.ToInt32(ddlJobType.SelectedValue.Trim());
                    }
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cpfAd.EmpID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue.Trim());
                    }
                    if (ddlPayType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Pay type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cpfAd.Paytype = ddlPayType.SelectedValue.Trim();
                    }
                    if (ddlLoanType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select LoanType", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cpfAd.LoanTypeID = ddlLoanType.SelectedValue;
                    }
                    if (txtClaimAmt.Text == "")
                    {
                        ucMessage.ShowMessage("Advance amount is Required", BL.Enums.MessageType.Error);

                    }
                    else
                    {
                        cpfAd.AdvanceAmount = Convert.ToInt32(txtClaimAmt.Text.Trim());
                    }
                    if (txtEffDate.Text == "")
                    {
                        ucMessage.ShowMessage("Effective Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cpfAd.AdvanceDate = Convert.ToDateTime(txtEffDate.Text.Trim());
                    }
                    if (txtLimYtd.Text == "")
                    {
                        ucMessage.ShowMessage("Installment Size is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cpfAd.NoOfInst = Convert.ToInt32(txtLimYtd.Text.Trim());
                    }
                    if (txtOv.Text == "")
                    {
                        ucMessage.ShowMessage("Deduction Amount is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cpfAd.DedAmount = Convert.ToInt32(txtOv.Text.Trim());
                    }
                    if (txtOvLimApp.Text == "")
                    {
                        ucMessage.ShowMessage("Deducted Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cpfAd.DedDate = txtOvLimApp.Text.Trim();
                    }
                    if (ddlAppStatus.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Status", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        cpfAd.Advstatus = ddlAppStatus.SelectedValue.Trim();
                    }
                    if (checkIsActive.Checked == true)
                    {
                        cpfAd.IsActive = true;
                    }
                    else
                    {
                        cpfAd.IsActive = false;
                    }
                    cpfAd.updatedOn = DateTime.Now;
                    cpfAd.UpdatedBy = UserID;
                    if (ddlAppStatus.SelectedValue == "A")
                    {
                       
                            //var exit = db.tblPlCPFAdvances.Where(x => x.ChqNo == txtChqNo.Text.Trim()  && x.CPFID != ID).FirstOrDefault();
                            //if (exit != null)
                            //{
                            //    ucMessage.ShowMessage("Cheque Number already Exit", BL.Enums.MessageType.Error);
                            //    return;
                            //}
                            //else
                            //{
                                if (txtChqNo.Text == "")
                                {
                                    ucMessage.ShowMessage("Cheque No is Required", BL.Enums.MessageType.Error);
                                    return;
                                }
                                else
                                {

                                    cpfAd.ChqNo = txtChqNo.Text.Trim();

                                }
                                if (txtChqBranch.Text == "")
                                {
                                    ucMessage.ShowMessage("Branch is Required", BL.Enums.MessageType.Error);
                                    return;
                                }
                                else
                                {
                                    cpfAd.BankBranch = txtChqBranch.Text.Trim();
                                }
                                if (txtChqAcctNo.Text == "")
                                {
                                    ucMessage.ShowMessage("Account No is Required", BL.Enums.MessageType.Error);
                                    return;
                                }
                                else
                                {
                                    cpfAd.AccountNo = txtChqAcctNo.Text.Trim();
                                }
                                if (txtChqDate.Text == "")
                                {
                                    ucMessage.ShowMessage("Cheque Date Is required", BL.Enums.MessageType.Error);
                                    return;
                                }
                                else
                                {
                                    cpfAd.ChqDate = Convert.ToDateTime(txtChqDate.Text.Trim());
                                }

                            pf = db.tblPlProvidentFunds.Where(x => x.EmpID == Convert.ToInt32(ddlEmployeeSearch.SelectedValue) && x.CPFID == cpfAd.CPFID).FirstOrDefault();
                            if (pf != null)
                            {

                                //var close = pf.closeBlnc;
                                // pf.closeBlnc = pf.OpenBlnc;
                                pf.closeBlnc = pf.OpenBlnc - Convert.ToDecimal(txtClaimAmt.Text);
                                db.SubmitChanges();
                            }
                            List<tblPlProvidentFund> pfList = db.tblPlProvidentFunds.Where(x => x.EmpID == Convert.ToInt32(ddlEmployeeSearch.SelectedValue) && x.CPFID >= cpfAd.CPFID).ToList();
                            for (int i = 0; i < pfList.Count; i++)
                            {
                                int pfIDforanEmp = Convert.ToInt32(pfList[i].PfID);
                                tblPlProvidentFund pfForanEmp = db.tblPlProvidentFunds.Where(x => x.PfID == pfIDforanEmp).FirstOrDefault();


                                
                                if (i > 0)
                                {
                                    int PreviouspFIDforanEmp = Convert.ToInt32(pfList[i - 1].PfID);
                                    if (pfList[i].type == 1 || pfList[i].type == 3)
                                    {
                                        int pfID = Convert.ToInt32(pfList[i].PfID);

                                        tblPlProvidentFundDetail pfDetail = db.tblPlProvidentFundDetails.Where(x => x.EmpID == Convert.ToInt32(ddlEmployeeSearch.SelectedValue) && x.PfID == pfID).FirstOrDefault();

                                        decimal currentPFvalue = Convert.ToDecimal(pfDetail.value);
                                        pfList[i].OpenBlnc = pfList[i - 1].closeBlnc;
                                        pfList[i].closeBlnc = pfList[i].OpenBlnc + currentPFvalue;
                                    }
                                    else if (pfList[i].type == 2)
                                    {
                                        int pfID = Convert.ToInt32(pfList[i].CPFID);

                                        tblPlCPFAdvance CpfDetail = db.tblPlCPFAdvances.Where(x => x.EmpID == Convert.ToInt32(ddlEmployeeSearch.SelectedValue) && x.CPFID == pfID).FirstOrDefault();

                                        decimal currentPFvalue = Convert.ToDecimal(CpfDetail.AdvanceAmount);
                                        pfList[i].OpenBlnc = pfList[i - 1].closeBlnc;
                                        pfList[i].closeBlnc = pfList[i].OpenBlnc - currentPFvalue;
                                    }
                                    else if (pfList[i].type == 4)
                                    {
                                        int pfID = Convert.ToInt32(pfList[i].PfID);

                                        tblPlCPFAdvanceDetail CpfAdvanceDetail= db.tblPlCPFAdvanceDetails.Where(x => x.EmpID == Convert.ToInt32(ddlEmployeeSearch.SelectedValue) && x.pfID == pfID).FirstOrDefault();

                                        decimal currentPFvalue = Convert.ToDecimal(CpfAdvanceDetail.InstalValue);
                                        pfList[i].OpenBlnc = pfList[i - 1].closeBlnc;
                                        pfList[i].closeBlnc = pfList[i].OpenBlnc + currentPFvalue;
                                    }
                                    

                                }

                                db.SubmitChanges();
                            }
                        }
                    else
                    {

                    }
                    db.SubmitChanges();
                    ucMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
                    
                }
                BindGrid();
                ClearFields();

            }
            catch (Exception)
            {

                throw;
            }


            //*************************************************************
            //try
            //{
            //    if (ddlAppStatus.SelectedValue.Equals("A"))
            //    {
            //        Glmf_Data_chq chq = new Glmf_Data_chq();
            //        chq.vr_chq_branch = hdnGlCode.Value;
            //        chq.vr_chq = txtChqNo.Text;
            //        if (RMS.BL.BranchBL.IsChequeNoExists(chq, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //        {
            //            ucMessage.ShowMessage("Cheque no already exists", RMS.BL.Enums.MessageType.Error);
            //            txtChqNo.Focus();
            //            chq = null;
            //            return;
            //        }
            //        chq = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            //    return;
            //}
            ////*************************************************************
            //try
            //{
            //    Convert.ToDateTime(txtEffDate.Text.Trim());
            //}
            //catch
            //{
            //    ucMessage.ShowMessage("Invalid effective date", RMS.BL.Enums.MessageType.Error);
            //    txtEffDate.Focus();
            //    return;
            //}

            //try
            //{
            //    Convert.ToByte(txtLimYtd.Text.Trim());
            //}
            //catch
            //{
            //    ucMessage.ShowMessage("Invalid installment size", RMS.BL.Enums.MessageType.Error);
            //    txtLimYtd.Focus();
            //    return;
            //}

            //if (ddlAppStatus.SelectedValue == "A" && ddlPayType.SelectedValue == "Cheque")
            //{
            //    try
            //    {
            //        Convert.ToDateTime(txtChqDate.Text.Trim());
            //    }
            //    catch
            //    {
            //        ucMessage.ShowMessage("Invalid cheque date", RMS.BL.Enums.MessageType.Error);
            //        txtChqDate.Focus();
            //        return;
            //    }


            //    if (Convert.ToDateTime(txtChqDate.Text.Trim()) >= Convert.ToDateTime(txtEffDate.Text.Trim()))
            //    {
            //        ucMessage.ShowMessage("Cheque date should be less than effective date", RMS.BL.Enums.MessageType.Error);
            //        return;
            //    }

            //    decimal Financialyear = objVoucher.GetFinancialYearByDate(Convert.ToDateTime(txtEffDate.Text.Trim()), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //    DateTime vrDate = Convert.ToDateTime(txtChqDate.Text.Trim());
            //    DateTime dtFrm = DateTime.Parse("01-Jul-" + Financialyear);
            //    dtFrm = dtFrm.AddYears(-1);
            //    DateTime dtTo = DateTime.Parse("30-Jun-" + Financialyear);
            //    if (vrDate >= dtFrm && vrDate <= dtTo)
            //    {
            //        // ok
            //    }
            //    else
            //    {
            //        ucMessage.ShowMessage("Cheque Date should be within this financial year i.e. " + Financialyear, RMS.BL.Enums.MessageType.Error);
            //        txtChqDate.Focus();
            //        return;
            //    }
            //}
            //decimal paidBackAmount = 0;
            //if (!string.IsNullOrEmpty(txtOvLimApp.Text))
            //{
            //    paidBackAmount = Convert.ToDecimal(txtOvLimApp.Text);
            //}
            //decimal claimAmount = Convert.ToDecimal(txtClaimAmt.Text) - paidBackAmount;
            //decimal instAmount = Convert.ToDecimal(Convert.ToByte(txtLimYtd.Text.Trim())) * Convert.ToDecimal(txtOv.Text);

            //if (claimAmount != instAmount)
            //{
            //    ucMessage.ShowMessage("Invalid installment size or amount", RMS.BL.Enums.MessageType.Error);
            //    txtOv.Focus();
            //    return;
            //}

            ////********************************************************************************

            //bool IsSaved = false;

            //if (ddlAppStatus.SelectedValue.Equals("A"))
            //{
            //    IsSaved = SaveIPV();

            //    if (!IsSaved)
            //    {
            //        ucMessage.ShowMessage("Unable to save IPV at this time", RMS.BL.Enums.MessageType.Error);
            //        return;
            //    }
            //}
           
            //tblPlLoan lon = new tblPlLoan();
            //lon.CompID = Convert.ToByte(CompID);
            //lon.EmpID = ID;
            //lon.LoanTypeID = ddlLoanType.SelectedValue.ToString();
            //if (IsSaved)
            //{
            //    lon.PaymentRef = "PL-" + vrRef;
            //    if (ddlAppStatus.SelectedValue == "A" && ddlPayType.SelectedValue == "Cheque")
            //    {
            //        lon.PaymentDate = Convert.ToDateTime(txtChqDate.Text.Trim());
            //    }
            //    else
            //    {
            //        lon.PaymentDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //    }
            //}
            //else
            //{
            //    lon.PaymentRef = "00-000000";
            //    lon.PaymentDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //}
            //lon.LoanAmt = Convert.ToDecimal(txtClaimAmt.Text.Trim());
            //lon.NoOfInst = Convert.ToByte(txtLimYtd.Text.Trim());
            //lon.InstAmt = Convert.ToDecimal(txtOv.Text);
            //lon.InstStartDate = Convert.ToDateTime(txtEffDate.Text.Trim());
            //lon.LoanStatus = true;
            //lon.LoanAppStatus = ddlAppStatus.SelectedValue;
            //lon.PaymentType = ddlPayType.SelectedValue;

            //if (Session["UserID"] == null)
            //    lon.UpdateBy = Request.Cookies["uzr"]["UserID"].ToString();
            //else
            //    lon.UpdateBy = Session["UserID"].ToString();
            //lon.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ////if (!allowBL.ISAlreadyExist(lon, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            ////{
            //    allowBL.Insert(lon, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
            //    BindGrid();
            //    ClearFields();
            //}
            //else
            //{
            //    ucMessage.ShowMessage("Loan already exist with same reference or dates", RMS.BL.Enums.MessageType.Error);
            //}

        }

        protected void Onclick_Clear(object sender, EventArgs e)
        {
            searchBranchDropDown.SelectedValue = "0";
            ddlEmployeeSearch.SelectedValue = "0";
            ddlJobType.SelectedValue = "0";
            ddlLoanType.SelectedValue = "0";
            ddlPayType.SelectedValue = "0";
            txtChqAcctNo.Text = "";
            txtChqBranch.Text = "";
            txtChqDate.Text = "";
            txtClaimAmt.Text = "";
            txtLimYtd.Text = "";
            txtEffDate.Text = "";
            txtOv.Text = "";
            txtChqDate.Text = "";
            txtOvLimApp.Text = "";
            checkIsActive.Checked = true;
            ID = 0;
        }

        private void ClearFields()
        {
            ID = 0;
            //CompID = 0;
            searchBranchDropDown.SelectedValue = "0";
            ddlJobType.SelectedValue = "0";
            ddlEmployeeSearch.SelectedValue = "0";
            PaymentRef = "";
            //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtEffDate.Text = "";
           // btnDelete.Visible = false;

            ddlLoanType.SelectedIndex = 0;
            //txtClaimRefNo.Text = "";
            txtClaimAmt.Text = "";
            txtLimYtd.Text = "";
            //txtRefDate.Text = "";
            txtEffDate.Text = "";
            txtOv.Text = "";

            //txtBasicPay.Text = "";
            //txtHouseRent.Text = "";
            //txtFuelAll.Text = "";
            //txtSplAll.Text = "";
            //txtUtilities.Text = "";
            //divGrdLoan.Visible = false;
            //grdLoans.SelectedIndex = -1;
            
            //this.txtOv.Enabled=true;
            txtChqDateCal.SelectedDate = null;
            txtChqBranch.Text = "";
            txtChqAcctNo.Text = "";
            txtChqNo.Text = "";
            txtChqDate.Text = "";
            hdnGlCode.Value = "";

            ddlPayType.Enabled = true;
            ddlLoanType.Enabled = true;
            ddlAppStatus.Enabled = true;
            txtClaimAmt.Enabled = true;
            txtEffDate.Enabled = true;
            txtOvLimApp.Text = "";
            checkIsActive.Checked = true;
        }


        [WebMethod]
        public static List<spGetBankA_CResult> GetBranch(string bank)
        {
            voucherDetailBL vrBl = new voucherDetailBL();
            List<spGetBankA_CResult> acc = vrBl.GetBranch(Convert.ToInt32(HttpContext.Current.Session["BranchID"]), bank, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return acc;
        }

        #endregion

    }
}

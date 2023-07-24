using System;
using RMS.BL.Model;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Data.Linq;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using System.Web.Script.Services;

namespace RMS.Profile
{
    public partial class EmpMgtSalPayment : BasePage
    {

        #region DataMembers

        RMSDataContext db = new RMSDataContext();

        PlAllowBL allowBL = new PlAllowBL();


        //BL.SalaryBL SalBl = new RMS.BL.SalaryBL();
        //BL.PlSalPayBL SalPayBl = new RMS.BL.PlSalPayBL();
        //voucherDetailBL objVoucher = new voucherDetailBL();
        //PreferenceBL prefBl = new PreferenceBL();
        //PlCodeBL codeBl = new PlCodeBL();

        //EntitySet<Glmf_Data_Det> enttyGlDet = new EntitySet<Glmf_Data_Det>();
        //EntitySet<tblPlSalVouDet> enttyVouDet = new EntitySet<tblPlSalVouDet>();

        #endregion

        #region Properties

#pragma warning disable CS0108 // 'EmpMgtSalPayment.ID' hides inherited member 'Page.ID'. Use the new keyword if hiding was intended.
        public static int ID;
#pragma warning restore CS0108 // 'EmpMgtSalPayment.ID' hides inherited member 'Page.ID'. Use the new keyword if hiding was intended.
        //{
        //    get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
        //    set { ViewState["ID"] = value; }
        //}

        public static int EmpID;
        //{
        //    get { return (ViewState["EmpID"] == null) ? 0 : Convert.ToInt32(ViewState["EmpID"]); }
        //    set { ViewState["EmpID"] = value; }
        //}

        public decimal PayAmount
        {
            get { return (ViewState["PayAmount"] == null) ? 0 : Convert.ToDecimal(ViewState["PayAmount"]); }
            set { ViewState["PayAmount"] = value; }
        }

        public DateTime VouDate
        {
            get { return Convert.ToDateTime(ViewState["VouDate"]); }
            set { ViewState["VouDate"] = value; }
        }

        //public decimal TaxAmount
        //{
        //    get { return (ViewState["TaxAmount"] == null) ? 0 : Convert.ToDecimal(ViewState["TaxAmount"]); }
        //    set { ViewState["TaxAmount"] = value; }
        //}

        public decimal LoanAmount
        {
            get { return (ViewState["LoanAmount"] == null) ? 0 : Convert.ToDecimal(ViewState["LoanAmount"]); }
            set { ViewState["LoanAmount"] = value; }
        }

        public decimal OtrDed
        {
            get { return (ViewState["OtrDed"] == null) ? 0 : Convert.ToDecimal(ViewState["OtrDed"]); }
            set { ViewState["OtrDed"] = value; }
        }
        public decimal MiscDed
        {
            get { return (ViewState["MiscDed"] == null) ? 0 : Convert.ToDecimal(ViewState["MiscDed"]); }
            set { ViewState["MiscDed"] = value; }
        }

        //public decimal EobiAmount
        //{
        //    get { return (ViewState["EobiAmount"] == null) ? 0 : Convert.ToDecimal(ViewState["EobiAmount"]); }
        //    set { ViewState["EobiAmount"] = value; }
        //}


        public int Seq
        {
            get { return (ViewState["Seq"] == null) ? 0 : Convert.ToInt32(ViewState["Seq"]); }
            set { ViewState["Seq"] = value; }
        }

        public int MinSal
        {
            get { return (ViewState["MinSal"] == null) ? 0 : Convert.ToInt32(ViewState["MinSal"]); }
            set { ViewState["MinSal"] = value; }
        }
        
        public int MaxSal
        {
            get { return (ViewState["MaxSal"] == null) ? 0 : Convert.ToInt32(ViewState["MaxSal"]); }
            set { ViewState["MaxSal"] = value; }
        }

        public static int BrID;
        //{
        //    get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
        //    set { ViewState["BrID"] = value; }
        //}

        public int UserID
        {
            get { return (ViewState["UserID"] == null) ? 0 : Convert.ToInt32(ViewState["UserID"]); }
            set { ViewState["UserID"] = value; }
        }

        public int DeptID
        {
            get { return (ViewState["DeptID"] == null) ? 0 : Convert.ToInt32(ViewState["DeptID"]); }
            set { ViewState["DeptID"] = value; }
        }

        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

        public int BranIDD
        {
            get { return (ViewState["BranIDD"] == null) ? 0 : Convert.ToInt32(ViewState["BranIDD"]); }
            set { ViewState["BranIDD"] = value; }
        }

        public string PaymentRef
        {
            get { return (ViewState["PaymentRef"] == null) ? "" : Convert.ToString(ViewState["PaymentRef"]); }
            set { ViewState["PaymentRef"] = value; }
        }
        
        public string PayPeriod
        {
            get { return (ViewState["PayPeriod"] == null) ? "" : Convert.ToString(ViewState["PayPeriod"]); }
            set { ViewState["PayPeriod"] = value; }

        }
        public string vrRef
        {
            get { return (ViewState["vrRef"] == null) ? "" : Convert.ToString(ViewState["vrRef"]); }
            set { ViewState["vrRef"] = value; }

        }

        public int VrID
        {
            get { return (ViewState["VrID"] == null) ? 0 : Convert.ToInt32(ViewState["VrID"]); }
            set { ViewState["VrID"] = value; }
        }
        
        public int PayVrID
        {
            get { return (ViewState["PayVrID"] == null) ? 0 : Convert.ToInt32(ViewState["PayVrID"]); }
            set { ViewState["PayVrID"] = value; }
        }

        public bool IsEdit
        {
            get { return Convert.ToBoolean(ViewState["IsEdit"]); }
            set { ViewState["IsEdit"] = value; }
        }


        public string BankDesc
        {
            get { return (ViewState["BankDesc"] == null) ? "" : Convert.ToString(ViewState["BankDesc"]); }
            set { ViewState["BankDesc"] = value; }

        }

        public string BankCode
        {
            get { return (ViewState["BankCode"] == null) ? "" : Convert.ToString(ViewState["BankCode"]); }
            set { ViewState["BankCode"] = value; }

        }

        public string Department
        {
            get { return (ViewState["Department"] == null) ? "" : Convert.ToString(ViewState["Department"]); }
            set { ViewState["Department"] = value; }

        }

        public static int IsBranch
        {
            get; set;
        }


        public int ActiveMonhtID
        {
            get { return (ViewState["ActiveMonhtID"] == null) ? 0 : Convert.ToInt32(ViewState["ActiveMonhtID"]); }
            set { ViewState["ActiveMonhtID"] = value; }
        }

        public static int DivisionID;
        //{
        //    get { return (ViewState["DivisionID"] == null) ? 0 : Convert.ToInt32(ViewState["DivisionID"]); }
        //    set { ViewState["DivisionID"] = value; }
        //}


        protected class GetCPFDetailVM
        {
            public int cpfID { get; set; }
            public decimal Installment { get; set; }

        }

        #endregion

        #region Events


        protected void Page_Init(object sender, EventArgs e)
        {
            if (ID > 0 || EmpID > 0)
            {

                OnupdateEvent();

            }

            //DivisionID = Convert.ToInt32(string.IsNullOrEmpty(ddlBranchDropdown.SelectedValue) ? "0" : ddlBranchDropdown.SelectedValue);


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //tblPlProvidentFundDetail isProdiventDetailExist1 = db.tblPlProvidentFundDetails.Where(x => x.EmpID == 1096).ToList().ElementAtOrDefault(3);


                //if (ID > 0)
                //{
                //    OnupdateEvent();
                //}
                //else if (EmpID > 0)
                //{
                //    DynamicGridforCreate(EmpID);
                //}

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpSalPayment").ToString();
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);


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
                CompID = iCompid;

               

                //if (Session["DateFormat"] == null)
                //{
                //    txtChqDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                //}
                //else
                //{
                //    txtChqDateCal.Format = Session["DateFormat"].ToString();
                //}

                if (Session["BranchID"] == null)
                {
                    BrID = Convert.ToByte(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BrID = Convert.ToByte(Session["BranchID"].ToString());
                }



                if (Session["UserID"] == null)
                {
                    UserID = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                }
                else
                {
                    UserID = Convert.ToInt32(Session["UserID"].ToString());
                }

              

                string salp = GetSalPeriod(BrID);

                if (salp.Equals(""))
                {
                    lblMonthSal.Visible = true;
                    lblMonthSal.Text = "There is not any active month yet ";

                }
                else
                {
                    lblMonthSal.Visible = true;

                    lblMonthSal.Text = "Currently Active Month is " + salp + ". Do you want to Tranfer Salaries for this Month?";
                }

                FillddlJobType();
                FillDropDownEmployee();
                FillSearchBranchDropDown();
               searchBranchDropDown.SelectedValue = BrID.ToString();
                FillDropDownMonth();
                FillDropdownCascadeDropdown();
                searchMonthDropDown.SelectedValue = ActiveMonhtID.ToString();
                
                BindSalaryTransfer(BrID, ActiveMonhtID, "");

                //divSearch.Visible = true;
                //divView.Visible = false ;
                //btnDelete.Visible = false;
                //IsEdit = false;

                //FillDropDownPayPeriod();
                //FillDropDownCodeDept();
                //FillDDlBankBranch();

                //if (ddlAppStatus.SelectedValue == "P")
                //{
                //    RequiredFieldValidator8.Enabled = false;
                //    RequiredFieldValidator9.Enabled = false;
                //    RequiredFieldValidator10.Enabled = false;
                //}

                //BindGridPayments();

                //ucButtons.ValidationGroupName = "main";


                
                    // DivisionID = Convert.ToInt32(ddlBranchDropdown.SelectedValue);

                   // OnupdateEvent();
                


            }

            
        }

        protected void FillddlJobType()
        {
            try
            {
                ddlJobType.DataTextField = "JobTypeName1";
                ddlJobType.DataValueField = "JobNameID";
                using (RMSDataContext dataContext = new RMSDataContext())
                {

                    ddlJobType.DataSource = dataContext.JobTypeNames.Where(x => x.IsActive == true).ToList();
                    ddlJobType.DataBind();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void FillDropDownEmployee()
        {
            int bb = DivisionID != 0 ? DivisionID : BrID;
            RMSDataContext db = new RMSDataContext();
            
            this.ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataValueField = "EmpID";

            ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == bb).ToList();
            ddlEmployee.DataBind(); 
            ddlEmployee.Items.Insert(0, new ListItem("All", "0"));


        }

        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BrID).FirstOrDefault();

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
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BrID).ToList();
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

        private void FillDropDownMonth()
        {
            RMSDataContext db = new RMSDataContext();

            this.searchMonthDropDown.DataTextField = "MonthVal";
            searchMonthDropDown.DataValueField = "MonthID";

            searchMonthDropDown.DataSource = db.TblSalaryMonths.OrderByDescending(x => x.MonthID).ToList();
            searchMonthDropDown.DataBind();
            


        }
        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
   
                    BrID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    FillDropDownEmployee();
                    BindSalaryTransfer(BrID, ActiveMonhtID, "");


                }

            }
            catch
            { }
        }

        protected void Button_Command(object sender, EventArgs e)
        {

            try
            {


                int empID = Convert.ToInt32(ddlEmployee.SelectedValue.Trim());
                string chkInsert = InsertOrUpdate(empID);
                BindSalaryTransfer(BrID, ActiveMonhtID, "");
                if (chkInsert == "OK")
                {
                    ucMessage.ShowMessage("Salary transfered Successfully", RMS.BL.Enums.MessageType.Info);
                    ClearFields();
                   
                }
                else
                {
                    ucMessage.ShowMessage(chkInsert, RMS.BL.Enums.MessageType.Info);
                }

            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage(ex.Message.ToString(), RMS.BL.Enums.MessageType.Error);
            }

           // InsertOrUpdate(EmpID, activeMonthID,);
        }


        protected  string InsertOrUpdate(int EmpID)
        {
            try{
                int selectedMonth = 0;
                int selectedYear = 0;
                int activeMonthID = 0;
                TblSalaryMonth salaryMonthActive = db.TblSalaryMonths.Where(x => x.MonthIsActive == true && x.BranchID == BrID).FirstOrDefault();
                if (salaryMonthActive != null)
                {
                    DateTime MonthActive = Convert.ToDateTime(salaryMonthActive.MonthVal);
                    selectedMonth = MonthActive.Month;
                    selectedYear = MonthActive.Year;
                    activeMonthID = salaryMonthActive.MonthID;
                }
                else if (salaryMonthActive == null)
                {
                    return "Required active month first";
                }
                int brr = Convert.ToInt32(ddlBranchDropdown.SelectedValue);
                List<SpSalaryPackageResult>spSalPackgList = db.SpSalaryPackage(selectedYear, selectedMonth, EmpID, brr != 0?brr:BrID).ToList();
                var SpSalPackageResults = spSalPackgList.GroupBy(x => x.EmpID).ToList();
                if(SpSalPackageResults.Count() <= 0)
                {
                    return "Salary Package for employee(s) is not exist";
                }



                //Inertion Or Updations

                if(EmpID == 0)
                {
               
                foreach (var itemsList in SpSalPackageResults)
                {
                    int i = 0;
                    int salarytrfID = 0;
                        int wholecurrEmp = 0;
                    foreach (SpSalaryPackageResult spSalaryitem in itemsList)
                    {
                        int CurrentEmpID = Convert.ToInt32(spSalaryitem.EmpID);
                            wholecurrEmp = CurrentEmpID;
                        if (i == 0)
                        {
                            //int CurrentEmpID = Convert.ToInt32(spSalaryitem.EmpID);

                            SalaryTranferTbl checkIsExist = db.SalaryTranferTbls.Where(x => x.SalaryMonth == activeMonthID && x.EmpID == CurrentEmpID).FirstOrDefault();
                            if (checkIsExist != null)
                            {
                                    salarytrfID = checkIsExist.SalTrfID;
                                    //checkIsExist.Basic = Convert.ToDecimal(spSalaryitem.Basic);
                                    checkIsExist.IsActive = true;

                                    checkIsExist.updatedBy = UserID;
                                    checkIsExist.UpdatedOn = System.DateTime.Now;

                                    //checkIsExist.Basic = Convert.ToDecimal(spSalaryitem.Basic);
                                    //checkIsExist.IsActive = true;

                                    db.SubmitChanges();

                                }
                            else
                            {
                                int salaryTrfCount = db.SalaryTranferTbls.Count();
                                if(salaryTrfCount == 0)
                                    {
                                        salarytrfID = db.SalaryTranferTbls.Count() + 1;
                                    }
                                else if (salaryTrfCount > 0)
                                    {
                                        salarytrfID = db.SalaryTranferTbls.Max(x => x.SalTrfID) + 1;
                                    }
                                
                                SalaryTranferTbl salaryTranferTbl = new SalaryTranferTbl();
                                salaryTranferTbl.SalTrfID = salarytrfID;
                                salaryTranferTbl.SalaryMonth = activeMonthID;
                                salaryTranferTbl.EmpID = CurrentEmpID;

                                salaryTranferTbl.Basic = Convert.ToDecimal(spSalaryitem.Basic);
                                salaryTranferTbl.IsActive = true;

                                    salaryTranferTbl.CreatedId = UserID;
                                    salaryTranferTbl.CreatedOn = System.DateTime.Now;
                                    salaryTranferTbl.updatedBy = UserID;
                                    salaryTranferTbl.UpdatedOn = System.DateTime.Now;


                                    db.SalaryTranferTbls.InsertOnSubmit(salaryTranferTbl);
                                    db.SubmitChanges();
                                    if (salaryTranferTbl.SalTrfID > 0)
                                    {
                                        if (salaryTranferTbl.SalTrfID != salarytrfID)
                                        {
                                            salarytrfID = salaryTranferTbl.SalTrfID;
                                        }
                                    }

                                }
                            
                           
                            if(spSalaryitem.Name != null && spSalaryitem.Name != "")
                            {
                                string allowanceName = spSalaryitem.Name.ToLower();
                                SalaryTranferDetailTbl salaryTranferDetailIsExist = db.SalaryTranferDetailTbls.Where(x => x.SalTrfID == salarytrfID && x.Name.ToLower() == allowanceName).FirstOrDefault();
                                if (salaryTranferDetailIsExist != null)
                                {

                                        //salaryTranferDetailIsExist.SizeValue = spSalaryitem.SizeValue;

                                        //if (spSalaryitem.AllownanceType == "Allownance")
                                        //{
                                        //    salaryTranferDetailIsExist.SalaryContentTypeID = 1;
                                        //}
                                        //else if (spSalaryitem.AllownanceType == "Deduction")
                                        //{
                                        //    salaryTranferDetailIsExist.SalaryContentTypeID = 2;
                                        //}

                                        salaryTranferDetailIsExist.IsActive = true;

                                        db.SubmitChanges();

                                        //if (spSalaryitem.Name.ToLower().Equals("cpf"))
                                        //{
                                        //    decimal cpValue = Convert.ToInt32(spSalaryitem.SizeValue);
                                           
                                        //        insertOrUpdateCPF(CurrentEmpID, selectedMonth, selectedYear, cpValue);


                                        //}
                                    }
                                else
                                {
                                    SalaryTranferDetailTbl salaryTranferDetailTbl = new SalaryTranferDetailTbl();
                                    salaryTranferDetailTbl.SalTrfID = salarytrfID;
                                    salaryTranferDetailTbl.Name = spSalaryitem.Name;
                                    salaryTranferDetailTbl.SizeValue = spSalaryitem.SizeValue;

                                    if (spSalaryitem.AllownanceType == "Allownance")
                                    {
                                        salaryTranferDetailTbl.SalaryContentTypeID = 1;
                                    }
                                    else if (spSalaryitem.AllownanceType == "Deduction")
                                    {
                                        salaryTranferDetailTbl.SalaryContentTypeID = 2;
                                    }
                                    
                                       
                                    salaryTranferDetailTbl.IsActive = true;
                                    db.SalaryTranferDetailTbls.InsertOnSubmit(salaryTranferDetailTbl);
                                    db.SubmitChanges();
                                        if (spSalaryitem.Name.ToLower().Equals("cpf"))
                                        {
                                            decimal cpValue = Convert.ToInt32(spSalaryitem.SizeValue);

                                            insertOrUpdateCPF(CurrentEmpID, selectedMonth, selectedYear, cpValue);

                                        }

                                    }
                                
                            }
                           


                        }
                        else if (i > 0)
                        {
                            if (spSalaryitem.Name != null && spSalaryitem.Name != "")
                            {
                                string allowanceName = spSalaryitem.Name.ToLower();
                                SalaryTranferDetailTbl salaryTranferDetailIsExist = db.SalaryTranferDetailTbls.Where(x => x.SalTrfID == salarytrfID && x.Name.ToLower() == allowanceName).FirstOrDefault();
                                if (salaryTranferDetailIsExist != null)
                                {

                                        //salaryTranferDetailIsExist.SizeValue = spSalaryitem.SizeValue;

                                        //if (spSalaryitem.AllownanceType == "Allownance")
                                        //{
                                        //    salaryTranferDetailIsExist.SalaryContentTypeID = 1;
                                        //}
                                        //else if (spSalaryitem.AllownanceType == "Deduction")
                                        //{
                                        //    salaryTranferDetailIsExist.SalaryContentTypeID = 2;
                                        //}

                                        salaryTranferDetailIsExist.IsActive = true;
                                        db.SubmitChanges();

                                        //if (spSalaryitem.Name.ToLower().Equals("cpf"))
                                        //{
                                        //    decimal cpValue = Convert.ToInt32(spSalaryitem.SizeValue);
                                           
                                        //    insertOrUpdateCPF(CurrentEmpID, selectedMonth, selectedYear, cpValue);

                                        //}
                                    }
                                else
                                {
                                    SalaryTranferDetailTbl salaryTranferDetailTbl = new SalaryTranferDetailTbl();
                                    salaryTranferDetailTbl.SalTrfID = salarytrfID;
                                    salaryTranferDetailTbl.Name = spSalaryitem.Name;
                                    salaryTranferDetailTbl.SizeValue = spSalaryitem.SizeValue;

                                    if (spSalaryitem.AllownanceType == "Allownance")
                                    {
                                        salaryTranferDetailTbl.SalaryContentTypeID = 1;
                                    }
                                    else if (spSalaryitem.AllownanceType == "Deduction")
                                    {
                                        salaryTranferDetailTbl.SalaryContentTypeID = 2;
                                    }
                                        salaryTranferDetailTbl.IsActive = true;
                                        db.SalaryTranferDetailTbls.InsertOnSubmit(salaryTranferDetailTbl);
                                        db.SubmitChanges();
                                        if (spSalaryitem.Name.ToLower().Equals("cpf"))
                                        {
                                            decimal cpValue = Convert.ToInt32(spSalaryitem.SizeValue);

                                            insertOrUpdateCPF(CurrentEmpID, selectedMonth, selectedYear, cpValue);

                                        }
                                       

                                    }
                            }
                        }
                        i++;
                    }
                    if(db.tblPlCPFAdvances.Where(x => x.IsActive == true && x.Advstatus == "A" && x.EmpID == wholecurrEmp).Count() > 0)
                        {
                            decimal TotalAdvEmp = GetCurrentTotalCPFAdv(wholecurrEmp);
                            if (TotalAdvEmp > 0)
                            {
                                string cpfADVchk = insertorUpdateCPFAdvinPFtbl(wholecurrEmp, activeMonthID, selectedMonth, selectedYear);
                                if(cpfADVchk == "OK")
                                {
                                    string ddName = "CPF (Adv)";
                                    SalaryTranferDetailTbl salaryTranferDetailIsAdvExist = db.SalaryTranferDetailTbls.Where(x => x.SalTrfID == salarytrfID && x.Name.ToLower() == ddName).FirstOrDefault();
                                    if (salaryTranferDetailIsAdvExist != null)
                                    {
                                        salaryTranferDetailIsAdvExist.IsActive = true;

                                        db.SubmitChanges();

                                        
                                    }
                                    else
                                    {
                                        SalaryTranferDetailTbl salaryTranferDetailTbl = new SalaryTranferDetailTbl();
                                        salaryTranferDetailTbl.SalTrfID = salarytrfID;
                                        salaryTranferDetailTbl.Name = ddName;
                                        salaryTranferDetailTbl.SizeValue = TotalAdvEmp;
                                        salaryTranferDetailTbl.SalaryContentTypeID = 2;
                                        salaryTranferDetailTbl.IsActive = true;
                                        db.SalaryTranferDetailTbls.InsertOnSubmit(salaryTranferDetailTbl);
                                        db.SubmitChanges();

                                    }
                                }
                            }
                        }
                       
                }



                }
                
                else if (EmpID > 0)
                {

                   // OnupdateEvent();


                    int salarytrfSingleID = 0;
                    SalaryTranferTbl checkIsSalaryTrfExist = db.SalaryTranferTbls.Where(x => x.SalaryMonth == activeMonthID && x.EmpID == EmpID).FirstOrDefault();
                    if (checkIsSalaryTrfExist != null)
                    {
                        checkIsSalaryTrfExist.Basic = Convert.ToDecimal(txtBasicPay.Text.Trim());
                        checkIsSalaryTrfExist.Remarks = txtRemaks.Text.Trim();
                        if (txtfromPerid.Text == "")
                        {
                            checkIsSalaryTrfExist.fromPeriod = null;
                        }
                        else
                        {
                            checkIsSalaryTrfExist.fromPeriod = Convert.ToDateTime(txtfromPerid.Text);
                        }
                        if (txtToPeriod.Text == "")
                        {
                            checkIsSalaryTrfExist.toPeriod = null;
                        }
                        else
                        {
                            checkIsSalaryTrfExist.toPeriod = Convert.ToDateTime(txtToPeriod.Text);
                        }
                        if (txtPerDay.Text == "")
                        {
                            checkIsSalaryTrfExist.perdayrate = null;
                        }
                        else
                        {
                            checkIsSalaryTrfExist.perdayrate = Convert.ToInt32(txtPerDay.Text);
                        }
                        if (CheckIsActive.Checked == true)
                        {
                            checkIsSalaryTrfExist.IsActive = true;
                        }
                        else
                        {
                            checkIsSalaryTrfExist.IsActive = false;
                        }
                        salarytrfSingleID = checkIsSalaryTrfExist.SalTrfID;
                        checkIsSalaryTrfExist.updatedBy = UserID;
                        checkIsSalaryTrfExist.UpdatedOn = System.DateTime.Now;
                        db.SubmitChanges();

                    }
                    else
                    {
                        int salaryTrfCount = db.SalaryTranferTbls.Count();
                        if (salaryTrfCount == 0)
                        {
                            salarytrfSingleID = db.SalaryTranferTbls.Count() + 1;
                        }
                        else if (salaryTrfCount > 0)
                        {
                            salarytrfSingleID = db.SalaryTranferTbls.Max(x => x.SalTrfID) + 1;
                        }
                        SalaryTranferTbl salaryTranferTbl = new SalaryTranferTbl();
                        salaryTranferTbl.SalTrfID = salarytrfSingleID;
                        salaryTranferTbl.SalaryMonth = activeMonthID;
                        salaryTranferTbl.EmpID = EmpID;

                        salaryTranferTbl.Basic = Convert.ToDecimal(txtBasicPay.Text.Trim());
                        salaryTranferTbl.Remarks = txtRemaks.Text.Trim();
                        if (txtfromPerid.Text == "")
                        {
                            salaryTranferTbl.fromPeriod = null;
                        }
                        else
                        {
                            salaryTranferTbl.fromPeriod = Convert.ToDateTime(txtfromPerid.Text);
                        }
                        if (txtToPeriod.Text == "")
                        {
                            salaryTranferTbl.toPeriod = null;
                        }
                        else
                        {
                            salaryTranferTbl.toPeriod = Convert.ToDateTime(txtToPeriod.Text);
                        }
                        if (txtPerDay.Text == "")
                        {
                            salaryTranferTbl.perdayrate = null;
                        }
                        else
                        {
                            salaryTranferTbl.perdayrate = Convert.ToInt32(txtPerDay.Text);
                        }
                        if (CheckIsActive.Checked == true)
                        {
                            salaryTranferTbl.IsActive = true;
                        }
                        else
                        {
                            salaryTranferTbl.IsActive = false;
                        }
                       

                        salaryTranferTbl.CreatedId = UserID;
                        salaryTranferTbl.CreatedOn = System.DateTime.Now;
                        salaryTranferTbl.updatedBy = UserID;
                        salaryTranferTbl.UpdatedOn = System.DateTime.Now;


                        db.SalaryTranferTbls.InsertOnSubmit(salaryTranferTbl);
                        db.SubmitChanges();

                        if (salaryTranferTbl.SalTrfID > 0)
                        {
                            if (salaryTranferTbl.SalTrfID != salarytrfSingleID)
                            {
                                salarytrfSingleID = salaryTranferTbl.SalTrfID;
                            }
                        }

                    }
                  

                    int gridAllCount = EmployAllowanceLableGridDetail.Controls.OfType<TextBox>().Count();
                    var gridDdCount = EmployDeductionLableGridDetail.Controls.OfType<TextBox>().Count();

                    for (int x = 0; x < gridAllCount; x++)
                    {
                        TextBox txtAllName = EmployAllowanceLableGridDetail.Controls.OfType<TextBox>().ElementAtOrDefault(x);
                        TextBox txtAllVal = EmployAllowanceGridDetail.Controls.OfType<TextBox>().ElementAtOrDefault(x);
                        string txtAllValValue = txtAllVal.Text.Trim();
                        string txtAllNameValue = txtAllName.Text.Trim();

                        if (txtAllNameValue != null && txtAllNameValue != "")
                        {
                            string allowanceName = txtAllNameValue.ToLower();
                            SalaryTranferDetailTbl salaryTranferDetailIsExistSingle = db.SalaryTranferDetailTbls.Where(z => z.SalTrfID == salarytrfSingleID && z.Name.ToLower() == allowanceName).FirstOrDefault();
                            if (salaryTranferDetailIsExistSingle != null)
                            {
                                salaryTranferDetailIsExistSingle.SalTrfID = salarytrfSingleID;
                                salaryTranferDetailIsExistSingle.Name = txtAllNameValue;
                                if(txtAllValValue != null && txtAllValValue != "")
                                {
                                    salaryTranferDetailIsExistSingle.SizeValue = Convert.ToDecimal(txtAllValValue);
                                }
                                else
                                {
                                    salaryTranferDetailIsExistSingle.SizeValue = 0;
                                }
                               


                                salaryTranferDetailIsExistSingle.SalaryContentTypeID = 1;

                                if (CheckIsActive.Checked == true)
                                {
                                    salaryTranferDetailIsExistSingle.IsActive = true;
                                }
                                else
                                {
                                    salaryTranferDetailIsExistSingle.IsActive = false;
                                }
                               
                            }
                            else
                            {
                                SalaryTranferDetailTbl salaryTranferSingleDetailTbl = new SalaryTranferDetailTbl();
                                salaryTranferSingleDetailTbl.SalTrfID = salarytrfSingleID;
                                salaryTranferSingleDetailTbl.Name = txtAllNameValue;
                                if (txtAllValValue != null && txtAllValValue != "")
                                {
                                    salaryTranferSingleDetailTbl.SizeValue = Convert.ToDecimal(txtAllValValue);
                                }
                                else
                                {
                                    salaryTranferSingleDetailTbl.SizeValue = 0;
                                }
                                salaryTranferSingleDetailTbl.SalaryContentTypeID = 1;

                                if (CheckIsActive.Checked == true)
                                {
                                    salaryTranferSingleDetailTbl.IsActive = true;
                                }
                                else
                                {
                                    salaryTranferSingleDetailTbl.IsActive = false;
                                }

                                db.SalaryTranferDetailTbls.InsertOnSubmit(salaryTranferSingleDetailTbl);

                            }
                            db.SubmitChanges();
                        }


                    }
                    for (int y = 0; y < gridDdCount; y++)
                    {
                        TextBox txtDdName = EmployDeductionLableGridDetail.Controls.OfType<TextBox>().ElementAtOrDefault(y);
                        TextBox txtDdVal = EmployDeductionGridDetail.Controls.OfType<TextBox>().ElementAtOrDefault(y);
                        string txtDdNameValue = txtDdName.Text.Trim();
                        string txtDdValValue = txtDdVal.Text.Trim();
                        

                        if (txtDdNameValue != null && txtDdNameValue != "")
                        {
                            string deductionName = txtDdNameValue.ToLower();
                            SalaryTranferDetailTbl salaryTranferDetailIsExistSingleDd = db.SalaryTranferDetailTbls.Where(z => z.SalTrfID == salarytrfSingleID && z.Name.ToLower() == deductionName).FirstOrDefault();
                            if (salaryTranferDetailIsExistSingleDd != null)
                            {
                                salaryTranferDetailIsExistSingleDd.SalTrfID = salarytrfSingleID;
                                salaryTranferDetailIsExistSingleDd.Name = txtDdNameValue;
                                if (txtDdValValue != null && txtDdValValue != "")
                                {
                                    salaryTranferDetailIsExistSingleDd.SizeValue = Convert.ToDecimal(txtDdValValue);
                                }
                                else
                                {
                                    salaryTranferDetailIsExistSingleDd.SizeValue = 0;
                                }

                                
                                salaryTranferDetailIsExistSingleDd.SalaryContentTypeID = 2;

                                if (CheckIsActive.Checked == true)
                                {
                                    salaryTranferDetailIsExistSingleDd.IsActive = true;
                                }
                                else
                                {
                                    salaryTranferDetailIsExistSingleDd.IsActive = false;
                                }
                                
                                db.SubmitChanges();
                                if (txtDdNameValue.ToLower().Equals("cpf"))
                                {
                                    decimal cpValue = 0;
                                    if (CheckIsActive.Checked == true)
                                    {
                                         cpValue = Convert.ToDecimal(txtDdValValue);
                                    }
                                   
                                    

                                    insertOrUpdateCPF(EmpID, selectedMonth, selectedYear, cpValue);

                                }

                                if (txtDdNameValue.ToLower().Equals("cpf (adv)"))
                                {

                                    insertorUpdateCPFAdvinPFtbl(EmpID, activeMonthID, selectedMonth, selectedYear);
                                }
                               
                            }
                            else
                            {
                                SalaryTranferDetailTbl salaryTranferSingleDetailTblDd = new SalaryTranferDetailTbl();
                                salaryTranferSingleDetailTblDd.SalTrfID = salarytrfSingleID;
                                salaryTranferSingleDetailTblDd.Name = txtDdNameValue;
                                if (txtDdValValue != null && txtDdValValue != "")
                                {
                                    salaryTranferSingleDetailTblDd.SizeValue = Convert.ToDecimal(txtDdValValue);
                                }
                                else
                                {
                                    salaryTranferSingleDetailTblDd.SizeValue = 0;
                                }
                                salaryTranferSingleDetailTblDd.SalaryContentTypeID = 2;
                                if (CheckIsActive.Checked == true)
                                {
                                    salaryTranferSingleDetailTblDd.IsActive = true;
                                }
                                else
                                {
                                    salaryTranferSingleDetailTblDd.IsActive = false;
                                }

                                db.SalaryTranferDetailTbls.InsertOnSubmit(salaryTranferSingleDetailTblDd);
                                db.SubmitChanges();

                                if (txtDdNameValue.ToLower().Equals("cpf"))
                                {
                                    decimal cpValue = 0;
                                    if (CheckIsActive.Checked == true)
                                    {
                                        cpValue = Convert.ToDecimal(txtDdValValue);
                                    }
                                    insertOrUpdateCPF(EmpID, selectedMonth, selectedYear, cpValue);

                                }
                                if (txtDdNameValue.ToLower().Equals("cpf (adv)"))
                                {

                                    insertorUpdateCPFAdvinPFtbl(EmpID, activeMonthID, selectedMonth, selectedYear);
                                }


                            }
                           
                        }


                    }

                    //foreach (TextBox txtDd in EmployDeductionGridDetail.Controls.OfType<TextBox>())
                    //{
                    //    int deductionID = Convert.ToInt32(txtDd.ID.Trim());
                    //    tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                    //    if (txtDd.Text.Trim() == "" || txtDd.Text.Trim() == null)
                    //    {
                    //        txtDd.Text = "0";
                    //    }

                    //    tblPlSalaryDetailObj.SalaryContentID = deductionID;
                    //    decimal ddVal = Convert.ToDecimal(txtDd.Text.Trim());
                    //    tblPlSalaryDetailObj.SizeValue = ddVal;

                    //    tblPlSalaryDetailObj.IsActive = true;
                    //    //tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                       

                    //}

                }



                //Check Salary package Exist otherWise False
                foreach (var itemschkList in SpSalPackageResults)
                {
                    int i = 0;
                    int salarytrfexitID = 0;
                    foreach (SpSalaryPackageResult spSalaryitemChk in itemschkList)
                    {

                        if (i == 0)
                        {
                            int CurrentEmpID = Convert.ToInt32(spSalaryitemChk.EmpID);

                            SalaryTranferTbl salaryTranferTblCHKObj = db.SalaryTranferTbls.Where(x => x.SalaryMonth == activeMonthID && x.EmpID == CurrentEmpID && x.IsActive == true).FirstOrDefault();
                            salarytrfexitID = salaryTranferTblCHKObj.SalTrfID;
                        }


                        i++;
                    }

                    List<SalaryPackacgeVM> spSalaryVMList = new List<SalaryPackacgeVM>();
                    List<SalaryPackacgeVM> dbSalaryVMList = new List<SalaryPackacgeVM>();
                    List<SalaryPackacgeVM> combineVMList = new List<SalaryPackacgeVM>();
                    List<SpSalaryPackageResult> spSalaryPackageResultsList = itemschkList.ToList();
                    foreach (SpSalaryPackageResult spItem in spSalaryPackageResultsList)
                    {
                        if(spItem.Name != null && spItem.Name != "")
                        {
                            SalaryPackacgeVM spVM = new SalaryPackacgeVM();
                            spVM.Name = spItem.Name;
                            spVM.EmpID = spItem.EmpID;
                            spSalaryVMList.Add(spVM);
                        }

                    }
                    List<SalaryTranferDetailTbl> salaryTranferDetailTblsList = db.SalaryTranferDetailTbls.Where(x => x.IsActive == true && x.SalTrfID == salarytrfexitID).ToList();
                    foreach (SalaryTranferDetailTbl dbItem in salaryTranferDetailTblsList)
                    {
                        SalaryPackacgeVM dbVM = new SalaryPackacgeVM();
                        dbVM.Name = dbItem.Name;
                        dbVM.trfID = Convert.ToInt32(dbItem.SalTrfID);
                        dbVM.TrfDetailID = Convert.ToInt32(dbItem.SalTrfDetatailID);
                        dbSalaryVMList.Add(dbVM);
                    }


                    combineVMList = dbSalaryVMList.Where(x => spSalaryVMList.All(y => y.Name.ToLower() != x.Name.ToLower())).ToList();
                    foreach (var itemincombList in combineVMList)
                    {
                        int SalMonthDetID = itemincombList.TrfDetailID;
                        SalaryTranferDetailTbl salaryTranferDetailTblnotUse = db.SalaryTranferDetailTbls.Where(x => x.SalTrfDetatailID == SalMonthDetID).FirstOrDefault();
                        if (salaryTranferDetailTblnotUse != null)
                        {
                            if(salaryTranferDetailTblnotUse.Name == "CPF (Adv)")
                            {
                                if(CheckIsActive.Checked == true)
                                {
                                    salaryTranferDetailTblnotUse.IsActive = true;
                                }
                                else if (CheckIsActive.Checked == false)
                                {
                                    salaryTranferDetailTblnotUse.IsActive = false;
                                }
                            }
                            else
                            {
                                salaryTranferDetailTblnotUse.IsActive = false;
                            }
                            
                            db.SubmitChanges();
                        }

                    }

                }



                return "OK";
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }

        }

       

        public string insertOrUpdateCPF(int empPFID, int selectedPFMonth,int selectedPFYear, decimal pfValue)
        {
            try
            {
                int CPFType = 0;


                for (int l = 0; l < 2; l++)
                {
                    if (l == 0)
                    {
                        CPFType = 1;//For CPF Deduct
                    }
                    else if (l == 1)
                    {
                        CPFType = 3; //For CPF Comp Deduct
                    }
                    int pfID = 0;

                    tblPlProvidentFundDetail isProdiventDetailExist = db.tblPlProvidentFundDetails.Where(x => x.EmpID == empPFID && x.Month == selectedPFMonth && x.Year == selectedPFYear && x.Type == CPFType).FirstOrDefault();


                    if (isProdiventDetailExist != null)
                    {
                        isProdiventDetailExist.value = pfValue;
                        pfID = Convert.ToInt32(isProdiventDetailExist.PfID);
                        db.SubmitChanges();

                        // tblPlProvidentFund tblPlProvident = db.tblPlProvidentFunds.Where(x => x.PfID == pfID && x.EmpID == empPFID && x.type == CPFType).FirstOrDefault();

                        List<tblPlProvidentFund> maintainCPFListofaSinglePerson = db.tblPlProvidentFunds.Where(x => x.EmpID == empPFID && x.PfID >= pfID).ToList();

                        for (int j = 0; j < maintainCPFListofaSinglePerson.Count; j++)
                        {

                            //int pfIDforanEmp = Convert.ToInt32(maintainCPFListofaSinglePerson[j].PfID);
                            //tblPlProvidentFund pfForanEmp = db.tblPlProvidentFunds.Where(x => x.PfID == pfIDforanEmp).FirstOrDefault();


                            if (j == 0)
                            {
                                maintainCPFListofaSinglePerson[j].closeBlnc = Convert.ToDecimal(maintainCPFListofaSinglePerson[j].OpenBlnc) + pfValue;

                            }
                            else if (j > 0)
                            {

                                if (maintainCPFListofaSinglePerson[j].type == 1 || maintainCPFListofaSinglePerson[j].type == 3)
                                {
                                    int currFIDforanEmp = Convert.ToInt32(maintainCPFListofaSinglePerson[j].PfID);

                                    tblPlProvidentFundDetail currentPFDetail = db.tblPlProvidentFundDetails.Where(x => x.EmpID == empPFID && x.PfID == currFIDforanEmp).FirstOrDefault();

                                    decimal currentPFvalue = Convert.ToDecimal(currentPFDetail.value);
                                    maintainCPFListofaSinglePerson[j].OpenBlnc = maintainCPFListofaSinglePerson[j - 1].closeBlnc;
                                    maintainCPFListofaSinglePerson[j].closeBlnc = maintainCPFListofaSinglePerson[j].OpenBlnc + currentPFvalue;

                                }
                                else if (maintainCPFListofaSinglePerson[j].type == 2)
                                {
                                    int currCPFIDforanEmp = Convert.ToInt32(maintainCPFListofaSinglePerson[j].CPFID);

                                    tblPlCPFAdvance CpfDetail = db.tblPlCPFAdvances.Where(x => x.EmpID == empPFID && x.CPFID == currCPFIDforanEmp).FirstOrDefault();

                                    decimal currentPFvalue = Convert.ToDecimal(CpfDetail.AdvanceAmount);
                                    maintainCPFListofaSinglePerson[j].OpenBlnc = maintainCPFListofaSinglePerson[j - 1].closeBlnc;
                                    maintainCPFListofaSinglePerson[j].closeBlnc = maintainCPFListofaSinglePerson[j].OpenBlnc + currentPFvalue;
                                }
                                else if (maintainCPFListofaSinglePerson[j].type == 4)
                                {
                                    int currCPFIDforanEmp = Convert.ToInt32(maintainCPFListofaSinglePerson[j].PfID);

                                    tblPlCPFAdvanceDetail CpfAdvanceDetail = db.tblPlCPFAdvanceDetails.Where(x => x.EmpID == empPFID && x.pfID == pfID).FirstOrDefault();

                                    decimal currentPFvalue = Convert.ToDecimal(CpfAdvanceDetail.InstalValue);
                                    maintainCPFListofaSinglePerson[j].OpenBlnc = maintainCPFListofaSinglePerson[j - 1].closeBlnc;
                                    maintainCPFListofaSinglePerson[j].closeBlnc = maintainCPFListofaSinglePerson[j].OpenBlnc + currentPFvalue;

                                }

                            }

                            db.SubmitChanges();
                        }

                    }


                    else
                    {
                        insertonlyCPF(empPFID, selectedPFMonth, selectedPFYear, pfValue, CPFType);
                    }


                }




                return "OK";

            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }


       
        protected string insertonlyCPF(int empPFID, int selectedPFMonth, int selectedPFYear, decimal pfValue, int cpfType)
        {
            try
            {
                int pfID = 0;
                int pfCount = db.tblPlProvidentFunds.Count();
                if (pfCount == 0)
                {
                    pfID = db.tblPlProvidentFunds.Count() + 1;
                }
                else if (pfCount > 0)
                {
                    pfID = db.tblPlProvidentFunds.Max(x => x.PfID) + 1;
                }


                tblPlProvidentFund tblPlProvidentFund = new tblPlProvidentFund();
                tblPlProvidentFund.PfID = pfID;
                tblPlProvidentFund.EmpID = empPFID;
                int empPfCount = db.tblPlProvidentFunds.Where(x => x.EmpID == empPFID).Count();
                if (empPfCount == 0)
                {
                    tblPlProvidentFund.OpenBlnc = 0;
                }
                else if (empPfCount > 0)
                {
                    tblPlProvidentFund previousCloseBalanceforEmp = db.tblPlProvidentFunds.Where(x => x.EmpID == empPFID).OrderByDescending(x => x.PfID).Take(1).Single();
                    decimal previousCloseBalance = Convert.ToDecimal(previousCloseBalanceforEmp.closeBlnc);
                    tblPlProvidentFund.OpenBlnc = previousCloseBalance;
                }

                tblPlProvidentFund.closeBlnc = tblPlProvidentFund.OpenBlnc + pfValue;
                tblPlProvidentFund.type = cpfType;
                tblPlProvidentFund.createdBy = UserID.ToString();
                tblPlProvidentFund.createdon = System.DateTime.Now;
                db.tblPlProvidentFunds.InsertOnSubmit(tblPlProvidentFund);
                db.SubmitChanges();
                if(tblPlProvidentFund.PfID > 0)
                {
                    if (tblPlProvidentFund.PfID != pfID)
                    {
                        pfID = tblPlProvidentFund.PfID;
                    }
                }
                   

                tblPlProvidentFundDetail tblPlProvidentFundDetail = new tblPlProvidentFundDetail();
                tblPlProvidentFundDetail.EmpID = empPFID;
                tblPlProvidentFundDetail.PfID = pfID;
                tblPlProvidentFundDetail.Year = selectedPFYear;
                tblPlProvidentFundDetail.Month = selectedPFMonth;
                tblPlProvidentFundDetail.value = pfValue;
                tblPlProvidentFundDetail.Type = cpfType;
                db.tblPlProvidentFundDetails.InsertOnSubmit(tblPlProvidentFundDetail);
                db.SubmitChanges();





                return "OK";
            }
            catch(Exception ex){
                return ex.Message.ToString();
            }
        }



        //Adv Recovery
        protected string insertorUpdateCPFAdvinPFtbl(int empId, int activeMonthId, int monthVal, int yearVal)
        {
            try
            {

                List<tblPlProvidentFundDetail> isProdiventDetailExistList = db.tblPlProvidentFundDetails.Where(x => x.EmpID == empId && x.Month == monthVal && x.Year == yearVal && x.Type == 4).ToList();
                List<GetCPFDetailVM> getCurrAdvList = GetCurrentCPFAdvList(empId).ToList();
                if (isProdiventDetailExistList.Count == 0)
                {
                    foreach (GetCPFDetailVM advObj in getCurrAdvList)
                    {
                        insertonlyCPFAdv(empId, monthVal, yearVal, advObj.Installment, 4, activeMonthId, advObj.cpfID);
                    }

                }
                else if (isProdiventDetailExistList.Count > getCurrAdvList.Count)
                {

                }
                int pfID = 0;

                for (int i = 0; i < getCurrAdvList.Count; i++)
                {
                    tblPlProvidentFundDetail isProdiventDetailExist = db.tblPlProvidentFundDetails.Where(x => x.EmpID == empId).ToList().ElementAtOrDefault(i);


                    if (isProdiventDetailExist != null)
                    {
                        isProdiventDetailExist.value = getCurrAdvList[i].Installment;
                        pfID = Convert.ToInt32(isProdiventDetailExist.PfID);
                        db.SubmitChanges();

                        // tblPlProvidentFund tblPlProvident = db.tblPlProvidentFunds.Where(x => x.PfID == pfID && x.EmpID == empPFID && x.type == CPFType).FirstOrDefault();

                        List<tblPlProvidentFund> maintainCPFListofaSinglePerson = db.tblPlProvidentFunds.Where(x => x.EmpID == empId && x.PfID >= pfID).ToList();

                        for (int j = 0; j < maintainCPFListofaSinglePerson.Count; j++)
                        {

                            //int pfIDforanEmp = Convert.ToInt32(maintainCPFListofaSinglePerson[j].PfID);
                            //tblPlProvidentFund pfForanEmp = db.tblPlProvidentFunds.Where(x => x.PfID == pfIDforanEmp).FirstOrDefault();


                            if (j == 0)
                            {
                                maintainCPFListofaSinglePerson[j].closeBlnc = Convert.ToDecimal(maintainCPFListofaSinglePerson[j].OpenBlnc) + getCurrAdvList[i].Installment;

                            }
                            else if (j > 0)
                            {

                                if (maintainCPFListofaSinglePerson[j].type == 1 || maintainCPFListofaSinglePerson[j].type == 3)
                                {
                                    int currFIDforanEmp = Convert.ToInt32(maintainCPFListofaSinglePerson[j].PfID);

                                    tblPlProvidentFundDetail currentPFDetail = db.tblPlProvidentFundDetails.Where(x => x.EmpID == empId && x.PfID == currFIDforanEmp).FirstOrDefault();

                                    decimal currentPFvalue = Convert.ToDecimal(currentPFDetail.value);
                                    maintainCPFListofaSinglePerson[j].OpenBlnc = maintainCPFListofaSinglePerson[j - 1].closeBlnc;
                                    maintainCPFListofaSinglePerson[j].closeBlnc = maintainCPFListofaSinglePerson[j].OpenBlnc + currentPFvalue;

                                }
                                else if (maintainCPFListofaSinglePerson[j].type == 2)
                                {
                                    int currCPFIDforanEmp = Convert.ToInt32(maintainCPFListofaSinglePerson[j].CPFID);

                                    tblPlCPFAdvance CpfDetail = db.tblPlCPFAdvances.Where(x => x.EmpID == empId && x.CPFID == currCPFIDforanEmp).FirstOrDefault();

                                    decimal currentPFvalue = Convert.ToDecimal(CpfDetail.AdvanceAmount);
                                    maintainCPFListofaSinglePerson[j].OpenBlnc = maintainCPFListofaSinglePerson[j - 1].closeBlnc;
                                    maintainCPFListofaSinglePerson[j].closeBlnc = maintainCPFListofaSinglePerson[j].OpenBlnc + currentPFvalue;
                                }
                                else if (maintainCPFListofaSinglePerson[j].type == 4)
                                {
                                    int currCPFIDforanEmp = Convert.ToInt32(maintainCPFListofaSinglePerson[j].PfID);

                                    tblPlCPFAdvanceDetail CpfAdvanceDetail = db.tblPlCPFAdvanceDetails.Where(x => x.EmpID == empId && x.pfID == pfID).FirstOrDefault();

                                    decimal currentPFvalue = Convert.ToDecimal(CpfAdvanceDetail.InstalValue);
                                    maintainCPFListofaSinglePerson[j].OpenBlnc = maintainCPFListofaSinglePerson[j - 1].closeBlnc;
                                    maintainCPFListofaSinglePerson[j].closeBlnc = maintainCPFListofaSinglePerson[j].OpenBlnc + currentPFvalue;

                                }

                            }

                            db.SubmitChanges();


                            insertintblPlCPFAdvanceDetail(getCurrAdvList[i].Installment, empId, activeMonthId, getCurrAdvList[i].cpfID, pfID);
                        }

                    }


                    else
                    {
                        insertonlyCPFAdv(empId, monthVal, yearVal, getCurrAdvList[i].Installment, 4, activeMonthId, getCurrAdvList[i].cpfID);
                        //insertonlyCPF(empId, monthVal, yearVal, getCurrAdvList[i].Installment, 3);
                    }
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        protected string insertonlyCPFAdv(int empPFID, int selectedPFMonth, int selectedPFYear, decimal pfValue, int cpfType, int monthId, int cpfID)
        {
            try
            {
                int pfID = 0;
                int pfCount = db.tblPlProvidentFunds.Count();
                if (pfCount == 0)
                {
                    pfID = db.tblPlProvidentFunds.Count() + 1;
                }
                else if (pfCount > 0)
                {
                    pfID = db.tblPlProvidentFunds.Max(x => x.PfID) + 1;
                }


                tblPlProvidentFund tblPlProvidentFund = new tblPlProvidentFund();
                tblPlProvidentFund.PfID = pfID;
                tblPlProvidentFund.EmpID = empPFID;
                int empPfCount = db.tblPlProvidentFunds.Where(x => x.EmpID == empPFID).Count();
                if (empPfCount == 0)
                {
                    tblPlProvidentFund.OpenBlnc = 0;
                }
                else if (empPfCount > 0)
                {
                    tblPlProvidentFund previousCloseBalanceforEmp = db.tblPlProvidentFunds.Where(x => x.EmpID == empPFID).OrderByDescending(x => x.PfID).Take(1).Single();
                    decimal previousCloseBalance = Convert.ToDecimal(previousCloseBalanceforEmp.closeBlnc);
                    tblPlProvidentFund.OpenBlnc = previousCloseBalance;
                }

                tblPlProvidentFund.closeBlnc = tblPlProvidentFund.OpenBlnc + pfValue;
                tblPlProvidentFund.type = cpfType;
                tblPlProvidentFund.createdBy = UserID.ToString();
                tblPlProvidentFund.createdon = System.DateTime.Now;
                db.tblPlProvidentFunds.InsertOnSubmit(tblPlProvidentFund);
                db.SubmitChanges();
                if (tblPlProvidentFund.PfID > 0)
                {
                    if (tblPlProvidentFund.PfID != pfID)
                    {
                        pfID = tblPlProvidentFund.PfID;
                    }
                }


                tblPlProvidentFundDetail tblPlProvidentFundDetail = new tblPlProvidentFundDetail();
                tblPlProvidentFundDetail.EmpID = empPFID;
                tblPlProvidentFundDetail.PfID = pfID;
                tblPlProvidentFundDetail.Year = selectedPFYear;
                tblPlProvidentFundDetail.Month = selectedPFMonth;
                tblPlProvidentFundDetail.value = pfValue;
                tblPlProvidentFundDetail.Type = cpfType;
                db.tblPlProvidentFundDetails.InsertOnSubmit(tblPlProvidentFundDetail);
                db.SubmitChanges();


                insertintblPlCPFAdvanceDetail(pfValue, empPFID, monthId, cpfID, pfID);


                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        protected string insertintblPlCPFAdvanceDetail(decimal installment,int empId,  int monthID, int CPFID, int pfID)
        {
            try
            {
                tblPlCPFAdvanceDetail tblPlCPFAdvance = db.tblPlCPFAdvanceDetails.Where(x => x.EmpID == empId && x.pfID == pfID && x.MonthID == monthID && x.CPFID == CPFID).FirstOrDefault();
                if(tblPlCPFAdvance != null)
                {
                    tblPlCPFAdvance.InstalValue = installment;
                    tblPlCPFAdvance.UpdatedOn = System.DateTime.Now;
                }
                else
                {
                    tblPlCPFAdvanceDetail tblPlCPFAdvanceObj = new tblPlCPFAdvanceDetail();
                    tblPlCPFAdvanceObj.InstalValue = installment;
                    tblPlCPFAdvanceObj.EmpID = empId;
                    tblPlCPFAdvanceObj.MonthID = monthID;
                    tblPlCPFAdvanceObj.CPFID = CPFID;
                    tblPlCPFAdvanceObj.pfID = pfID;
                    tblPlCPFAdvanceObj.UpdatedOn = System.DateTime.Now;
                    tblPlCPFAdvanceObj.CreatedOn = System.DateTime.Now;
                    db.tblPlCPFAdvanceDetails.InsertOnSubmit(tblPlCPFAdvanceObj);          
                }
                db.SubmitChanges();
                return "OK";
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }
           

        //--------------

        protected void Clear_All(object sender, EventArgs e)
        {
            ClearFields();
        }
        protected void ClearFields()
        {
            ID = 0;
            EmpID = 0;
            ddlEmployee.SelectedValue = "0";
            ddlJobType.SelectedValue = "0";
            txtBasicPay.Text = "";
            txtfromPerid.Text = "";
            txtRemaks.Text = "";
            txtToPeriod.Text = "";
            txtPerDay.Text = "";
            //DivisionID = 0;



            EmployAllowanceLableGridDetail.Controls.Clear();
            EmployAllowanceGridDetail.Controls.Clear();
            EmployDeductionLableGridDetail.Controls.Clear();
            EmployDeductionGridDetail.Controls.Clear();
            BindSalaryTransfer(BrID, ActiveMonhtID, "");


        }

        
       protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlEmployee.SelectedValue.Equals("0"))
                {
                    
                    int empID = Convert.ToInt32(ddlEmployee.SelectedValue.Trim());
                    EmpID = empID;
                    ID = 0;
                    EmployAllowanceLableGridDetail.Controls.Clear();
                    EmployAllowanceGridDetail.Controls.Clear();
                    EmployDeductionLableGridDetail.Controls.Clear();
                    EmployDeductionGridDetail.Controls.Clear();
                    OnupdateEvent();
                    //DynamicGridforCreate(empID);
                    if (BrID == 1)
                    {
                        int bran = Convert.ToInt32(ddlBranchDropdown.SelectedValue);
                        BindSalaryTransfer(bran, ActiveMonhtID, "");
                    }
                    else
                    {
                        BindSalaryTransfer(BrID, ActiveMonhtID, "");
                    }
                    
                }
                else
                {
                    int empID = Convert.ToInt32(ddlEmployee.SelectedValue.Trim());
                    EmpID = empID;
                    ID = 0;
                    txtBasicPay.Text = "";
                    EmployAllowanceLableGridDetail.Controls.Clear();
                    EmployAllowanceGridDetail.Controls.Clear();
                    EmployDeductionLableGridDetail.Controls.Clear();
                    EmployDeductionGridDetail.Controls.Clear();
                    if (BrID == 1)
                    {
                        int bran = Convert.ToInt32(ddlBranchDropdown.SelectedValue);
                        BindSalaryTransfer(bran, ActiveMonhtID, "");

                    }
                    else
                    {
                        BindSalaryTransfer(BrID, ActiveMonhtID, "");
                    }
                    
                }
                
            }
            catch
            { }
        }
        //public string DynamicGridforCreate(int employeeID)
        //{

        //    return Hahah
            
          
        //}

        protected string GetSalPeriod(int brId)
        {
            try
            {

                TblSalaryMonth salaryMonth = db.TblSalaryMonths.Where(x => x.BranchID == brId && x.MonthIsActive == true).FirstOrDefault();
                if (salaryMonth != null)
                {
                    ActiveMonhtID = salaryMonth.MonthID;
                    string currMonth = salaryMonth.MonthVal.ToString();
                    
                    return currMonth;
                }
                else
                {
                    return "";
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }
        }


        protected void grdSalaryTranfer_PageIndexChanged(object sender, EventArgs e)
        {

            ID = Convert.ToInt32(grdSalaryTranfer.SelectedDataKey.Values["SalTrfID"].ToString());
            EmpID = 0;
            EmployAllowanceLableGridDetail.Controls.Clear();
            EmployAllowanceGridDetail.Controls.Clear();
            EmployDeductionLableGridDetail.Controls.Clear();
            EmployDeductionGridDetail.Controls.Clear();
            OnupdateEvent();
            BindSalaryTransfer(BrID, ActiveMonhtID, "");
            //ClearFields();

        }


        protected void grdSalaryTranfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSalaryTranfer.PageIndex = e.NewPageIndex;
            BindSalaryTransfer(BrID, ActiveMonhtID, "");
        }

        protected void grdSalaryTranfer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ID = Convert.ToInt32(grdSalaryTranfer.DataKeys[e.RowIndex].Values[0]);

            using(RMSDataContext db = new RMSDataContext())
            {
                var de = db.SalaryTranferDetailTbls.Where(x => x.SalTrfID == ID).ToList();
                var sal = db.SalaryTranferTbls.Where(x => x.SalTrfID == ID).FirstOrDefault();
                foreach (var item in de)
                {
                    db.SalaryTranferDetailTbls.DeleteOnSubmit(item);
                }
                db.SalaryTranferTbls.DeleteOnSubmit(sal);
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
                
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
                ucMessage.ShowMessage("Deleted Successfully", BL.Enums.MessageType.Info);
            }
        }

        //protected void grdSalaryTranfer_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{
        //    GridViewRow gvr = grdSalaryTranfer.Rows[e.NewSelectedIndex];

        //}
        //int Counter = 1;
        protected void grdSalaryTranfer_RowBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string lsDataKeyActiveValue = grdSalaryTranfer.DataKeys[e.Row.RowIndex].Values[1].ToString();
                TblSalaryMonth tblSalaryMonthActive = db.TblSalaryMonths.Where(x => x.MonthIsActive == true && x.BranchID == BrID).FirstOrDefault();
                string MonthActiveID = tblSalaryMonthActive.MonthID.ToString();
                //if (lsDataKeyActiveValue.Equals(MonthActiveID))
                //{
                //    //e.Row.Cells[3].Text = "Yes";
                //    e.Row.Cells[3].Style["visibility"] = "show";
                //}
                //else
                //{
                //    e.Row.Cells[3].Text= "";
                //    //e.Row.Cells[3].Text = "No";
                //}

            }


        }


        protected decimal GetCurrentTotalCPFAdv(int empId)
        {
            
            try
            {
                decimal totalreturnCPF = 0;
                List<tblPlCPFAdvance> empCPFList = db.tblPlCPFAdvances.Where(x => x.IsActive == true && x.Advstatus == "A" && x.EmpID == empId).ToList();
                foreach(tblPlCPFAdvance  cpf in empCPFList)
                {
                    int cpfID = cpf.CPFID;
                    decimal currentCPFTotal = Convert.ToDecimal(cpf.AdvanceAmount);
                    decimal currentCPFRuning = 0;
                    List<tblPlCPFAdvanceDetail> empCPFDetailList = db.tblPlCPFAdvanceDetails.Where(y => y.CPFID == cpfID && y.EmpID == empId).ToList();
                    foreach(tblPlCPFAdvanceDetail cpDet in empCPFDetailList)
                    {
                        currentCPFRuning += Convert.ToDecimal(cpDet.InstalValue);
                    }

                    if(currentCPFRuning < currentCPFTotal)
                    {
                        totalreturnCPF += Convert.ToDecimal(cpf.DedAmount);
                    }
                }
                return totalreturnCPF;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return 0;
            }
        }

       
        protected List<GetCPFDetailVM> GetCurrentCPFAdvList(int empId)
        {
            List<GetCPFDetailVM> getCPFadvList = new List<GetCPFDetailVM>();

            try
            {
                //decimal totalreturnCPF = 0;
                List<tblPlCPFAdvance> empCPFList = db.tblPlCPFAdvances.Where(x => x.IsActive == true && x.Advstatus == "A" && x.EmpID == empId).ToList();
                foreach (tblPlCPFAdvance cpf in empCPFList)
                {
                    int cpfID = cpf.CPFID;
                    decimal currentCPFTotal = Convert.ToDecimal(cpf.AdvanceAmount);
                    decimal currentCPFRuning = 0;
                    List<tblPlCPFAdvanceDetail> empCPFDetailList = db.tblPlCPFAdvanceDetails.Where(y => y.CPFID == cpfID && y.EmpID == empId).ToList();
                    foreach (tblPlCPFAdvanceDetail cpDet in empCPFDetailList)
                    {
                        currentCPFRuning += Convert.ToDecimal(cpDet.InstalValue);
                    }

                    if (currentCPFRuning < currentCPFTotal)
                    {
                        GetCPFDetailVM advObj = new GetCPFDetailVM();
                        advObj.cpfID = cpfID;
                        advObj.Installment = Convert.ToDecimal(cpf.DedAmount);
                        getCPFadvList.Add(advObj);
                        //return Convert.ToDecimal(cpf.DedAmount);
                    }
                }
                return getCPFadvList;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return getCPFadvList;
            }
        }

        string OnupdateEvent()
        {
            
            CheckIsActive.Checked = true;

            if (ID > 0)
            {
                try
                { 
                    //txtBasicPay.Text = "";
                    //EmployAllowanceLableGridDetail.Controls.Clear();
                    //EmployAllowanceGridDetail.Controls.Clear();
                    //EmployDeductionLableGridDetail.Controls.Clear();
                    //EmployDeductionGridDetail.Controls.Clear();

                    SalaryTranferTbl SalTrf = db.SalaryTranferTbls.Where(x => x.SalTrfID == ID).FirstOrDefault();
                    if (SalTrf != null)
                    {
                        EmpID = Convert.ToInt32(SalTrf.EmpID);
                        //EmpID = empsID;
                        ddlEmployee.SelectedValue = SalTrf.EmpID.ToString();
                        txtBasicPay.Text = SalTrf.Basic.ToString();
                        tblPlEmpData empData = db.tblPlEmpDatas.Where(x => x.EmpID == EmpID).FirstOrDefault();
                        ddlJobType.SelectedValue = empData.JobNameID.ToString();
                        List<SalaryTranferDetailTbl> saltrfDetailList = db.SalaryTranferDetailTbls.Where(x => x.SalTrfID == ID && x.IsActive == true).ToList();
                        foreach (SalaryTranferDetailTbl items in saltrfDetailList)
                        {





                            if (items.SalaryContentTypeID == 1)
                            {
                                TextBox LabelAllowanceTextBox = new TextBox();
                                LabelAllowanceTextBox.Text = items.Name;
                                LabelAllowanceTextBox.CssClass = "form-control";
                                EmployAllowanceLableGridDetail.Controls.Add(LabelAllowanceTextBox);


                                TextBox ValueAllownceTextBox = new TextBox();
                                //Assigning the textbox ID name 
                                // ValueAllownceTextBox.ID = allowitem.SalaryContentID.ToString();
                                ValueAllownceTextBox.CssClass = "form-control";
                                ValueAllownceTextBox.Text = Convert.ToInt32(items.SizeValue).ToString();
                                ValueAllownceTextBox.TextMode = TextBoxMode.Number;
                                EmployAllowanceGridDetail.Controls.Add(ValueAllownceTextBox);
                            }
                            else if (items.SalaryContentTypeID == 2)
                            {
                                TextBox LabelDeductionTextBox = new TextBox();
                                LabelDeductionTextBox.Text = items.Name;
                                LabelDeductionTextBox.CssClass = "form-control";
                                EmployDeductionLableGridDetail.Controls.Add(LabelDeductionTextBox);


                                TextBox ValueDEductionTextBox = new TextBox();
                                //Assigning the textbox ID name 
                                // ValueDEductionTextBox.ID = deductionitem.SalaryContentID.ToString();
                                ValueDEductionTextBox.CssClass = "form-control";
                                ValueDEductionTextBox.Text = Convert.ToInt32(items.SizeValue).ToString();
                                ValueDEductionTextBox.TextMode = TextBoxMode.Number;
                                EmployDeductionGridDetail.Controls.Add(ValueDEductionTextBox);
                            }

                        }

                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
            else if(EmpID > 0)
            {
                try
                {
                    //txtBasicPay.Text = "";
                    //EmployAllowanceLableGridDetail.Controls.Clear();
                    //EmployAllowanceGridDetail.Controls.Clear();
                    //EmployDeductionLableGridDetail.Controls.Clear();
                    //EmployDeductionGridDetail.Controls.Clear();


                    
                    int selectedMonth = 0;
                    int selectedYear = 0;
                    int activeMonthID = 0;
                    TblSalaryMonth salaryMonthActive = db.TblSalaryMonths.Where(x => x.MonthIsActive == true && x.BranchID == BrID).FirstOrDefault();
                    if (salaryMonthActive != null)
                    {
                        DateTime MonthActive = Convert.ToDateTime(salaryMonthActive.MonthVal);
                        selectedMonth = MonthActive.Month;
                        selectedYear = MonthActive.Year;
                        activeMonthID = salaryMonthActive.MonthID;
                    }
                    else if (salaryMonthActive == null)
                    {
                        return "Required active month first";
                    }
                    //int brachh = 0;

                     //DivisionID = Convert.ToInt32(string.IsNullOrEmpty(ddlBranchDropdown.SelectedValue) ? "0" : ddlBranchDropdown.SelectedValue);


                    List<SpSalaryPackageResult> spSalPackgList = db.SpSalaryPackage(selectedYear, selectedMonth, EmpID, DivisionID != 0? DivisionID : BrID).ToList();
                    var SpSalPackageResults = spSalPackgList.GroupBy(x => x.EmpID).ToList();
                    if (SpSalPackageResults.Count() <= 0)
                    {
                        return "Salary Package for employee(s) is not exist";
                    }



                    //Inertion Or Updations
                    foreach (var itemsList in SpSalPackageResults)
                    {
                        int i = 0;
                        foreach (SpSalaryPackageResult spSalaryitem in itemsList)
                        {

                            if (i == 0)
                            {
                                int CurrentEmpID = Convert.ToInt32(spSalaryitem.EmpID);

                                txtBasicPay.Text = spSalaryitem.Basic.ToString();
                                if (spSalaryitem.fromper == null)
                                {
                                    txtfromPerid.Text = "";
                                }
                                else
                                {
                                    txtfromPerid.Text = spSalaryitem.fromper.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                                }
                                if (spSalaryitem.perto == null)
                                {
                                    txtToPeriod.Text = "";
                                }
                                else
                                {
                                    txtToPeriod.Text = spSalaryitem.perto.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                                }
                                if (spSalaryitem.perday == 0)
                                {
                                    txtPerDay.Text = "";
                                }
                                else
                                {
                                    txtPerDay.Text = spSalaryitem.perday.ToString();

                                }

                                if (spSalaryitem.Name != null && spSalaryitem.Name != "")
                                {
                                    if (spSalaryitem.AllownanceType == "Allownance")
                                    {
                                        TextBox LabelAllowanceTextBox = new TextBox();
                                        LabelAllowanceTextBox.Text = spSalaryitem.Name;
                                        LabelAllowanceTextBox.CssClass = "form-control";
                                        EmployAllowanceLableGridDetail.Controls.Add(LabelAllowanceTextBox);


                                        TextBox ValueAllownceTextBox = new TextBox();
                                        //Assigning the textbox ID name 
                                        // ValueAllownceTextBox.ID = allowitem.SalaryContentID.ToString();
                                        ValueAllownceTextBox.CssClass = "form-control";
                                        ValueAllownceTextBox.Text = Convert.ToInt32(spSalaryitem.SizeValue).ToString();
                                        ValueAllownceTextBox.TextMode = TextBoxMode.Number;
                                        EmployAllowanceGridDetail.Controls.Add(ValueAllownceTextBox);
                                    }
                                    else if (spSalaryitem.AllownanceType == "Deduction")
                                    {
                                        TextBox LabelDeductionTextBox = new TextBox();
                                        LabelDeductionTextBox.Text = spSalaryitem.Name;
                                        LabelDeductionTextBox.CssClass = "form-control";
                                        EmployDeductionLableGridDetail.Controls.Add(LabelDeductionTextBox);


                                        TextBox ValueDEductionTextBox = new TextBox();
                                        //Assigning the textbox ID name 
                                        // ValueDEductionTextBox.ID = deductionitem.SalaryContentID.ToString();
                                        ValueDEductionTextBox.CssClass = "form-control";
                                        ValueDEductionTextBox.Text = Convert.ToInt32(spSalaryitem.SizeValue).ToString();
                                        ValueDEductionTextBox.TextMode = TextBoxMode.Number;
                                        EmployDeductionGridDetail.Controls.Add(ValueDEductionTextBox);
                                    }
                                }

                            }
                            else if (i > 0)
                            {
                                if (spSalaryitem.Name != null && spSalaryitem.Name != "")
                                {
                                    if (spSalaryitem.AllownanceType == "Allownance")
                                    {
                                        TextBox LabelAllowanceTextBox = new TextBox();
                                        LabelAllowanceTextBox.Text = spSalaryitem.Name;
                                        LabelAllowanceTextBox.CssClass = "form-control";
                                        EmployAllowanceLableGridDetail.Controls.Add(LabelAllowanceTextBox);


                                        TextBox ValueAllownceTextBox = new TextBox();
                                        //Assigning the textbox ID name 
                                        // ValueAllownceTextBox.ID = allowitem.SalaryContentID.ToString();
                                        ValueAllownceTextBox.CssClass = "form-control";
                                        ValueAllownceTextBox.Text = Convert.ToInt32(spSalaryitem.SizeValue).ToString();
                                        ValueAllownceTextBox.TextMode = TextBoxMode.Number;
                                        EmployAllowanceGridDetail.Controls.Add(ValueAllownceTextBox);
                                    }
                                    else if (spSalaryitem.AllownanceType == "Deduction")
                                    {
                                        TextBox LabelDeductionTextBox = new TextBox();
                                        LabelDeductionTextBox.Text = spSalaryitem.Name;
                                        LabelDeductionTextBox.CssClass = "form-control";
                                        EmployDeductionLableGridDetail.Controls.Add(LabelDeductionTextBox);


                                        TextBox ValueDEductionTextBox = new TextBox();
                                        //Assigning the textbox ID name 
                                        // ValueDEductionTextBox.ID = deductionitem.SalaryContentID.ToString();
                                        ValueDEductionTextBox.CssClass = "form-control";
                                        ValueDEductionTextBox.Text = Convert.ToInt32(spSalaryitem.SizeValue).ToString();
                                        ValueDEductionTextBox.TextMode = TextBoxMode.Number;
                                        EmployDeductionGridDetail.Controls.Add(ValueDEductionTextBox);
                                    }
                                }
                            }
                            i++;
                        }



                    }


                    //CPF (Adv)
                    int chKADVCountIsExist = db.tblPlCPFAdvances.Where(x => x.IsActive == true && x.Advstatus == "A" && x.EmpID == EmpID).Count();
                    if (chKADVCountIsExist > 0)
                    {
                        if(GetCurrentTotalCPFAdv(EmpID) > 0)
                        {
                            TextBox LabelCPFTextBox = new TextBox();
                            LabelCPFTextBox.Text = "CPF(Adv)";
                            LabelCPFTextBox.CssClass = "form-control";
                            EmployDeductionLableGridDetail.Controls.Add(LabelCPFTextBox);


                            TextBox ValueCPFTextBox = new TextBox();
                            //Assigning the textbox ID name 
                            // ValueDEductionTextBox.ID = deductionitem.SalaryContentID.ToString();
                            ValueCPFTextBox.CssClass = "form-control";
                            ValueCPFTextBox.Text = GetCurrentTotalCPFAdv(EmpID).ToString();
                            ValueCPFTextBox.TextMode = TextBoxMode.Number;
                            ValueCPFTextBox.ReadOnly = true;
                            EmployDeductionGridDetail.Controls.Add(ValueCPFTextBox);
                        }
                        
                    }


                    return "OK";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }

            return "Error Unhandeled";
        }


        protected void Button_CommandSearch(object sender, EventArgs e)
        {
            ActiveMonhtID = Convert.ToInt32(searchMonthDropDown.SelectedValue.Trim());
            
            BindSalaryTransfer(BrID, ActiveMonhtID, searchEmployeeTxt.Text.Trim());
        }
        protected void BindSalaryTransfer(int brID, int monthId, string empname)
        {
            this.grdSalaryTranfer.DataSource = allowBL.GetAllSalaryTranfers(brID, monthId, empname, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdSalaryTranfer.DataBind();
        }





        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    PayPeriod = ddlPayPerd.SelectedValue;
        //    VouDate = Convert.ToDateTime(Convert.ToInt32(PayPeriod.Substring(4, 2)) + "-" +
        //                   DateTime.DaysInMonth(Convert.ToInt32(PayPeriod.Substring(0, 4)), Convert.ToInt32(PayPeriod.Substring(4, 2))) + "-" +
        //                   Convert.ToInt32(Convert.ToInt32(PayPeriod.Substring(0, 4))));

        //    MinSal = 0;
        //    MaxSal = -1;
        //    try
        //    {

        //        if (!txtMinSal.Text.Equals(""))
        //        {
        //            MinSal = Convert.ToInt32(txtMinSal.Text);
        //        }

        //        if (!txtMaxSal.Text.Equals(""))
        //        {
        //            MaxSal = Convert.ToInt32(txtMaxSal.Text);
        //        }

        //        if (!MinSal.Equals(0) && MaxSal.Equals(-1))
        //        {
        //            if (MaxSal < MinSal)
        //            {
        //                ucMessage.ShowMessage("Enter maximum salary", RMS.BL.Enums.MessageType.Error);
        //                return;
        //            }
        //        }

        //        if (!MinSal.Equals(0) && !MaxSal.Equals(-1))
        //        {
        //            if (MaxSal < MinSal)
        //            {
        //                ucMessage.ShowMessage("Maximum salary cannot be less than minimum salary", RMS.BL.Enums.MessageType.Error);
        //                return;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        ucMessage.ShowMessage("Enter valid salaries", RMS.BL.Enums.MessageType.Error);
        //        return;
        //    }

        //    try
        //    {
        //        Department = ddlDept.SelectedItem.Text;
        //        RMS.BL.BankBL bnk = new RMS.BL.BankBL();
        //        tblBank bank = bnk.GetByID(ddlBank.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        if (bank != null)
        //        {
        //            BankDesc = bank.BankABv + " (" + bank.BankName + ")";

        //            if (!string.IsNullOrEmpty(bank.GlAccCd))
        //            {
        //                Session["GlAccCode"] = bank.GlAccCd;
        //            }
        //            else
        //            {
        //                Session["GlAccCode"] = "-";
        //            }
        //        }
        //        else
        //        {
        //            Session["GlAccCode"] = "-";
        //        }
        //    }
        //    catch 
        //    {
        //        Session["GlAccCode"] = "-";
        //    }
        //    DeptID = Convert.ToInt32(ddlDept.SelectedValue);
        //    BindGridSalPay();
        //}
        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (this.Delete())
        //        {
        //            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
        //            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
        //            BindGridPayments();
        //            ClearFields();
        //        }
        //        else
        //        {
        //            ucMessage.ShowMessage("Could not delete as exception occurred", RMS.BL.Enums.MessageType.Error);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
        //    }

        //}
        //protected void grdSalPay_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdSalPay.PageIndex = e.NewPageIndex;
        //    BindGridSalPay();
        //}
        //protected void grdSalPay_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        //if (Session["DateFormat"] == null)
        //        //{
        //        //    if (!e.Row.Cells[1].Text.Equals("&nbsp;"))
        //        //    {
        //        //        e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
        //        //    }
        //        //    if (!e.Row.Cells[3].Text.Equals("&nbsp;"))
        //        //    {
        //        //        e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    if (!e.Row.Cells[1].Text.Equals("&nbsp;"))
        //        //    {
        //        //        e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text.ToString()).ToString(Session["DateFormat"].ToString());
        //        //    }
        //        //    if (!e.Row.Cells[3].Text.Equals("&nbsp;"))
        //        //    {
        //        //        e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text.ToString()).ToString(Session["DateFormat"].ToString());
        //        //    }
        //        //}
        //    }
        //}
        //protected void ButtonCommand(object sender, CommandEventArgs e)
        //{
        //    if (e.CommandName == "New")
        //    {
        //        ClearFields();
        //        ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
        //        // pnlMain.Enabled = true;
        //    }
        //    else if (e.CommandName == "Save")
        //    {
        //        if (!IsEdit)
        //        {
        //            this.Insert();
        //            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
        //        }
        //        else
        //        {
        //            this.Update();
        //            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
        //        }
        //    }
        //    else if (e.CommandName == "Delete")
        //    {

        //        try
        //        {
        //            this.Delete();
        //            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
        //        }
        //        catch (SqlException ex)
        //        {
        //            if (ex.Number == 547)
        //            {
        //                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletionDependency").ToString(), RMS.BL.Enums.MessageType.Error);
        //                return;
        //            }
        //            else
        //            {
        //                Session["errors"] = ex.Message;
        //                Response.Redirect("~/home/Error.aspx");
        //            }
        //        }

        //        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
        //        //BindGrid();
        //        ClearFields();

        //    }
        //    else if (e.CommandName == "Edit")
        //    {
        //        ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        //    }
        //    else if (e.CommandName == "Cancel")
        //    {
        //        ClearFields();
        //    }
        //}
        //protected void grdPayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdPayments.PageIndex = e.NewPageIndex;
        //    BindGridPayments();
        //}
        //protected void grdPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        string strBPNo = e.Row.Cells[0].Text.Substring(3);
        //        try
        //        {
        //            if (Convert.ToInt32(strBPNo) > 0)
        //            {
        //                Glmf_Data glmfdata = objVoucher.GetGlmf_Data(Convert.ToInt32(strBPNo), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //                e.Row.Cells[0].Text = "IP-" + glmfdata.vr_no;
        //            }
        //        }
        //        catch { }

        //        if (Session["DateFormat"] == null)
        //        {
        //            if (!e.Row.Cells[1].Text.Equals("&nbsp;"))
        //            {
        //                e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
        //            }
        //        }
        //        else
        //        {
        //            if (!e.Row.Cells[1].Text.Equals("&nbsp;"))
        //            {
        //                e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text.ToString()).ToString(Session["DateFormat"].ToString());
        //            }
        //        }

        //    }
        //}
        //protected void grdPayments_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ClearFields();

        //        PayVrID = Convert.ToInt32(grdPayments.SelectedDataKey.Value);

        //        tblPlSalVou voucher = SalPayBl.GetVoucher(PayVrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        if (voucher != null)
        //        {
        //            txtChqAmnt.Text = SalPayBl.GetVoucherAmount(PayVrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
        //            ddlAppStatus.SelectedValue = voucher.VouStatus.Trim();
        //            PayPeriod = voucher.PayPerd.ToString();

        //            VouDate = Convert.ToDateTime(Convert.ToInt32(PayPeriod.Substring(4, 2)) + "-" +
        //                    DateTime.DaysInMonth(Convert.ToInt32(PayPeriod.Substring(0, 4)), Convert.ToInt32(PayPeriod.Substring(4, 2))) + "-" +
        //                    Convert.ToInt32(Convert.ToInt32(PayPeriod.Substring(0, 4))));

        //            IsEdit = true;
        //            divSearch.Visible = false;
        //            divView.Visible = true;
        //            btnDelete.Visible = true;
        //            BindGridSalPayView();

        //            //if (voucher.VouStatus.Equals("A"))
        //            //{
        //            //    divChq.Visible = true;
        //            //    ucButtons.DisableSave();
        //            //    btnDelete.Visible = false;
        //            //    //Cheque Info

        //            //    string pamentRef = voucher.VouRef;
        //            //    pamentRef = pamentRef.Substring(3, 6);

        //            //    VrID = Convert.ToInt32(pamentRef);

        //            //    Glmf_Data_chq glChq = objVoucher.GetCheqDetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //            //    if (glChq != null)
        //            //    {
        //            //        txtChqBranch.Text = glChq.vr_chq_branch + " - " + objVoucher.GetGlmfCodeByID(glChq.vr_chq_branch.Trim(), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
        //            //        hdnGlCode.Value = glChq.vr_chq_branch;
        //            //        txtChqAcctNo.Text = glChq.vr_chq_ac;
        //            //        txtChqNo.Text = glChq.vr_chq;
        //            //        txtChqDateCal.SelectedDate = glChq.vr_chq_dt;
        //            //    }
        //            //}

        //            if (voucher.VouStatus.Equals("A"))
        //            {
        //                //divChq.Visible = true;
        //                ucButtons.DisableSave();
        //                btnDelete.Visible = false;
        //                //Cheque Info

        //                string pamentRef = voucher.VouRef;
        //                pamentRef = pamentRef.Substring(3, 6);

        //                VrID = Convert.ToInt32(pamentRef);

        //                //Glmf_Data_chq glChq = objVoucher.GetCheqDetByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //                //if (glChq != null)
        //                //{
        //                //    txtChqBranch.Text = glChq.vr_chq_branch + " - " + objVoucher.GetGlmfCodeByID(glChq.vr_chq_branch.Trim(), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc;
        //                //    hdnGlCode.Value = glChq.vr_chq_branch;
        //                //    txtChqAcctNo.Text = glChq.vr_chq_ac;
        //                //    txtChqNo.Text = glChq.vr_chq;
        //                //    txtChqDateCal.SelectedDate = glChq.vr_chq_dt;
        //                //}
        //            }

        //            try
        //            {
        //                RMS.BL.BankBL bnk = new RMS.BL.BankBL();
        //                tblBank bank = bnk.GetByID(BankCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //                if (bank != null)
        //                {
        //                    if (!string.IsNullOrEmpty(bank.GlAccCd))
        //                    {
        //                        Session["GlAccCode"] = bank.GlAccCd;
        //                    }
        //                    else
        //                    {
        //                        Session["GlAccCode"] = "-";
        //                    }
        //                }
        //            }
        //            catch { }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
        //    }
        //}
        //protected void grdSalPayView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdSalPayView.PageIndex = e.NewPageIndex;
        //    BindGridSalPayView();
        //}
        //protected void grdSalPayView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {

        //    }
        //}
        //protected void lnkPrint_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        //        int rowIndex = clickedRow.RowIndex;
        //        int vrid = Convert.ToInt32(grdPayments.DataKeys[rowIndex].Value);
        //        int payperd = Convert.ToInt32(grdPayments.Rows[rowIndex].Cells[2].Text);
        //        string status = grdPayments.Rows[rowIndex].Cells[4].Text;
        //        string paymentref = grdPayments.Rows[rowIndex].Cells[0].Text;
        //        string amount = grdPayments.Rows[rowIndex].Cells[3].Text;
        //        string paymentrefdate = grdPayments.Rows[rowIndex].Cells[1].Text;


        //        string rptLogoPath = "";
        //        rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

        //        string paypd = payperd.ToString();
        //        string yr = paypd.Substring(0, 4);
        //        string mn = paypd.Substring(4, 2);
        //        DateTime ddfrom = new DateTime(Convert.ToInt32(yr), Convert.ToInt32(mn), 13);

        //        List<spSalaryPaymentGridResult> empsSalPay = SalBl.GetSalPaymentGrid(CompID, payperd, vrid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //        //ReportViewer viewer = new ReportViewer();

        //        viewer.LocalReport.EnableExternalImages = true;

        //        viewer.LocalReport.ReportPath = "report/rdlc/rptSalaryPayment.rdlc";
        //        ReportDataSource datasource = new ReportDataSource("spSalaryPaymentGridResult", empsSalPay);

        //        ReportParameter[] paramz = new ReportParameter[11];
        //        paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

        //        if (Session["CompName"] == null)
        //        {
        //            paramz[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
        //        }
        //        else
        //        {
        //            paramz[1] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
        //        }
        //        paramz[2] = new ReportParameter("LogoPath", rptLogoPath);
        //        paramz[3] = new ReportParameter("Status", status);
        //        paramz[4] = new ReportParameter("Amount", amount);



        //        if (status.Equals("Approved"))
        //        {
        //            tblPlSalVou voucher = SalPayBl.GetVoucher(vrid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //            string pamentRef = voucher.VouRef;
        //            pamentRef = pamentRef.Substring(3);

        //             Glmf_Data_chq glChq = objVoucher.GetCheqDetByID(Convert.ToInt32(pamentRef), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //            if (glChq != null)
        //            {
        //                //paramz[5] = new ReportParameter("Bank", glChq.vr_chq_branch + " - " + objVoucher.GetGlmfCodeByID(glChq.vr_chq_branch.Trim(), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).gl_dsc);
        //                //paramz[6] = new ReportParameter("Account", glChq.vr_chq_ac);
        //                //paramz[7] = new ReportParameter("ChqNo", glChq.vr_chq);
        //                //paramz[8] = new ReportParameter("ChqDate", glChq.vr_chq_dt.Date.ToString("dd-MMM-yyyy"));
        //                //paramz[9] = new ReportParameter("RefNo", paymentref);
        //                //paramz[10] = new ReportParameter("RefDate", paymentrefdate);


        //            }
        //        }
        //        else
        //        {
        //            paramz[5] = new ReportParameter("Bank", "-");
        //            paramz[6] = new ReportParameter("Account", "");
        //            paramz[7] = new ReportParameter("ChqNo", "");
        //            paramz[8] = new ReportParameter("ChqDate", "");
        //            paramz[9] = new ReportParameter("RefNo", paymentref);
        //            paramz[10] = new ReportParameter("RefDate", paymentrefdate);
        //        }

        //        paramz[5] = new ReportParameter("Bank", "-");
        //        paramz[6] = new ReportParameter("Account", "");
        //        paramz[7] = new ReportParameter("ChqNo", "");
        //        paramz[8] = new ReportParameter("ChqDate", "");
        //        paramz[9] = new ReportParameter("RefNo", paymentref);
        //        paramz[10] = new ReportParameter("RefDate", paymentrefdate);

        //        viewer.LocalReport.Refresh();
        //        viewer.LocalReport.SetParameters(paramz);

        //        viewer.LocalReport.DataSources.Clear();
        //        viewer.LocalReport.DataSources.Add(datasource);

        //        viewer.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
        //    }
        //}

        #endregion

        #region Helping Method

        //private void Insert()
        //{
        //    enttyGlDet.Clear();
        //    enttyVouDet.Clear();
        //    try
        //    {
        //         string username = "";
        //        if (Session["LoginID"] == null)
        //        {
        //            username = Request.Cookies["uzr"]["LoginID"];
        //        }
        //        else
        //        {
        //            username = Session["LoginID"].ToString();
        //        }

        //        if (username.Length > 20)
        //        {
        //            username = username.Substring(0, 19);
        //        }

        //        //*********************************************************

        //        bool isCheckedAny = false;
        //        for (int i = 0; i < grdSalPay.Rows.Count; i++)
        //        {
        //            CheckBox chkBox = (CheckBox)grdSalPay.Rows[i].FindControl("chkIncludeInVoucher");
        //            if (chkBox.Checked)
        //            {
        //                isCheckedAny = true;
        //                break;
        //            }
        //        }
        //        if (!isCheckedAny)
        //        {
        //            ucMessage.ShowMessage("Please select atleast one employee to proceed", RMS.BL.Enums.MessageType.Error);
        //            return;
        //        }

        //        //*********************************************************

        //        bool IsSaved = false;

        //        if (ddlAppStatus.SelectedValue.Equals("A"))
        //        {
        //            Seq = 0;
        //            string vrNarr = GetNarration();

        //            if (Session["LoginID"] == null)
        //            {
        //                username = Request.Cookies["uzr"]["LoginID"];
        //            }
        //            else
        //            {
        //                username = Session["LoginID"].ToString();
        //            }

        //            if (username.Length > 15)
        //            {
        //                username = username.Substring(0, 14);
        //            }

        //            //int voucherTypeId = 3;
        //            int voucherTypeId = 6;
        //            decimal Financialyear = objVoucher.GetFinancialYearByDate(VouDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            string source = "PAY";
        //            int voucherno = objVoucher.GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], voucherTypeId, Financialyear, source);
        //            /*  ONE */
        //            Glmf_Data glmfdata = new Glmf_Data();
        //            glmfdata.br_id = BrID;
        //            glmfdata.Gl_Year = Financialyear;
        //            glmfdata.vt_cd = Convert.ToInt16(voucherTypeId);
        //            glmfdata.vr_no = voucherno;
        //            glmfdata.vr_dt = VouDate;
        //            glmfdata.vr_nrtn = vrNarr;
        //            glmfdata.vr_apr = 'P';
        //            glmfdata.updateby = username;
        //            glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            glmfdata.approvedby = username;
        //            glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            glmfdata.source = source;

        //            bool firstentry = true;
        //            for (int i = 0; i < grdSalPay.Rows.Count; i++)
        //            {
        //                CheckBox chkBox = (CheckBox)grdSalPay.Rows[i].FindControl("chkIncludeInVoucher");
        //                if (chkBox.Checked)
        //                {
        //                    EmpID = Convert.ToInt32(grdSalPay.DataKeys[i].Values[1]);
        //                    PayAmount = Convert.ToDecimal(grdSalPay.Rows[i].Cells[12].Text.Trim());
        //                    //TaxAmount = Convert.ToDecimal(grdSalPay.Rows[i].Cells[7].Text.Trim());
        //                    LoanAmount = Convert.ToDecimal(grdSalPay.Rows[i].Cells[8].Text.Trim());
        //                    OtrDed = Convert.ToDecimal(grdSalPay.Rows[i].Cells[10].Text.Trim());
        //                    MiscDed = Convert.ToDecimal(grdSalPay.Rows[i].Cells[11].Text.Trim());
        //                    //EobiAmount = Convert.ToDecimal(grdSalPay.Rows[i].Cells[9].Text.Trim());
        //                    ////IsSaved = SaveIPV();
        //                    tblPlEmpData employee = new EmpBL().GetByID(EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //                    if (firstentry)
        //                    {
        //                        firstentry = false;
        //                        /*  TWO */
        //                        if (!string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).SalaryExpAct))
        //                        {
        //                            Glmf_Data_Det glDet1 = new Glmf_Data_Det();
        //                            Seq = Seq + 1;
        //                            glDet1.vr_seq = Seq;
        //                            glDet1.gl_cd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).SalaryExpAct;
        //                            glDet1.vrd_debit = Convert.ToDecimal(txtChqAmnt.Text)
        //                                            + Convert.ToDecimal(txtTaxDed.Text)
        //                                            + Convert.ToDecimal(txtLoanAdvDed.Text)
        //                                            + Convert.ToDecimal(txtEobiDed.Text)
        //                                            + Convert.ToDecimal(txtOtherDed.Text)
        //                                            + Convert.ToDecimal(txtMiscDed.Text);
        //                            glDet1.vrd_credit = 0;
        //                            glDet1.vrd_nrtn = "";
        //                            glDet1.cc_cd = null;
        //                            enttyGlDet.Add(glDet1);
        //                        }
        //                        else
        //                        {
        //                            ucMessage.ShowMessage("Staff salary A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                            return;
        //                        }
        //                        /*  THREE */
        //                        if (Convert.ToDecimal(txtTaxDed.Text) > 0)
        //                        {
        //                            if (!string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).TaxPayable))
        //                            {
        //                                Glmf_Data_Det glDet2 = new Glmf_Data_Det();
        //                                Seq = Seq + 1;
        //                                glDet2.vr_seq = Seq;
        //                                glDet2.gl_cd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).TaxPayable;
        //                                glDet2.vrd_debit = 0;
        //                                glDet2.vrd_credit = Convert.ToDecimal(txtTaxDed.Text);
        //                                glDet2.vrd_nrtn = "";
        //                                glDet2.cc_cd = null;
        //                                enttyGlDet.Add(glDet2);
        //                            }
        //                            else
        //                            {
        //                                ucMessage.ShowMessage("Tax A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                                return;
        //                            }
        //                        }
        //                        /*  FOUR */
        //                        //if (Convert.ToDecimal(txtLoanAdvDed.Text) > 0)
        //                        //{
        //                        //    if (!string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).LoansPayable))
        //                        //    {
        //                        //        Glmf_Data_Det glDet3 = new Glmf_Data_Det();
        //                        //        Seq = Seq + 1;
        //                        //        glDet3.vr_seq = Seq;
        //                        //        glDet3.gl_cd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).LoansPayable;
        //                        //        glDet3.vrd_debit = 0;
        //                        //        glDet3.vrd_credit = Convert.ToDecimal(txtLoanAdvDed.Text); ;
        //                        //        glDet3.vrd_nrtn = "";
        //                        //        glDet3.cc_cd = null;
        //                        //        enttyGlDet.Add(glDet3);
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        ucMessage.ShowMessage("Loan advance A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                        //        return;
        //                        //    }
        //                        //}
        //                        /*  FIVE */
        //                        if (Convert.ToDecimal(txtEobiDed.Text) > 0)
        //                        {
        //                            if (!string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).EOBIPayable))
        //                            {
        //                                Glmf_Data_Det glDet4 = new Glmf_Data_Det();
        //                                Seq = Seq + 1;
        //                                glDet4.vr_seq = Seq;
        //                                glDet4.gl_cd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).EOBIPayable; ;
        //                                glDet4.vrd_debit = 0;
        //                                glDet4.vrd_credit = Convert.ToDecimal(txtEobiDed.Text); ;
        //                                glDet4.vrd_nrtn = "";
        //                                glDet4.cc_cd = null;
        //                                enttyGlDet.Add(glDet4);
        //                            }
        //                            else
        //                            {
        //                                ucMessage.ShowMessage("EOBI payable A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                                return;
        //                            }
        //                        }
        //                    }
        //                    /*  FOUR */
        //                    if (LoanAmount > 0)
        //                    {
        //                        string glcd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscPayable + employee.EmpID.ToString().PadLeft(4, '0');
        //                        if (string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscPayable))
        //                        {
        //                            ucMessage.ShowMessage("Ctrl Employee A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                            return;
        //                        }
        //                        Glmf_Data_Det glDet3 = new Glmf_Data_Det();
        //                        Seq = Seq + 1;
        //                        glDet3.vr_seq = Seq;
        //                        glDet3.gl_cd = glcd;
        //                        glDet3.vrd_debit = 0;
        //                        glDet3.vrd_credit = LoanAmount;
        //                        glDet3.vrd_nrtn = "";
        //                        glDet3.cc_cd = null;
        //                        enttyGlDet.Add(glDet3);
        //                    }
        //                    /*  SIX */
        //                    if (PayAmount > 0)
        //                    {
        //                        Glmf_Data_Det glDet5 = new Glmf_Data_Det();
        //                        Seq = Seq + 1;
        //                        glDet5.vr_seq = Seq;

        //                        string glcd1 = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscPayable + employee.EmpID.ToString().PadLeft(4, '0');
        //                        if (string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscPayable))
        //                        {
        //                            ucMessage.ShowMessage("Ctrl Employee A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                            return;
        //                        }
        //                        glDet5.gl_cd = glcd1;
        //                        glDet5.vrd_debit = 0;
        //                        glDet5.vrd_credit = PayAmount;
        //                        glDet5.vrd_nrtn = "";
        //                        glDet5.cc_cd = null;
        //                        enttyGlDet.Add(glDet5);
        //                    }
        //                    /*  SEVEN */
        //                    if (OtrDed > 0)
        //                    {
        //                        string glcd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).OtrDed + employee.EmpID.ToString().PadLeft(4, '0');
        //                        if (string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).OtrDed))
        //                        {
        //                            ucMessage.ShowMessage("Ctrl other deduction A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                            return;
        //                        }
        //                        Glmf_Data_Det glDet7 = new Glmf_Data_Det();
        //                        Seq = Seq + 1;
        //                        glDet7.vr_seq = Seq;
        //                        glDet7.gl_cd = glcd;
        //                        glDet7.vrd_debit = 0;
        //                        glDet7.vrd_credit = OtrDed;
        //                        glDet7.vrd_nrtn = "";
        //                        glDet7.cc_cd = null;
        //                        enttyGlDet.Add(glDet7);
        //                    }
        //                    /*  EIGHT */
        //                    if (MiscDed > 0)
        //                    {
        //                        string glcd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscDed + employee.EmpID.ToString().PadLeft(4, '0');
        //                        if (string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscDed))
        //                        {
        //                            ucMessage.ShowMessage("Ctrl misc. deduction A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                            return;
        //                        }
        //                        Glmf_Data_Det glDet8 = new Glmf_Data_Det();
        //                        Seq = Seq + 1;
        //                        glDet8.vr_seq = Seq;
        //                        glDet8.gl_cd = glcd;
        //                        glDet8.vrd_debit = 0;
        //                        glDet8.vrd_credit = MiscDed;
        //                        glDet8.vrd_nrtn = "";
        //                        glDet8.cc_cd = null;
        //                        enttyGlDet.Add(glDet8);
        //                    }
        //                }
        //            }
        //            ///*  SEVEN */
        //            //Glmf_Data_chq glmfChq = new Glmf_Data_chq();
        //            //glmfChq.vr_chq_branch = hdnGlCode.Value;
        //            //glmfChq.vr_chq = txtChqNo.Text.Trim();
        //            //if (!string.IsNullOrEmpty(txtChqDate.Text))
        //            //    glmfChq.vr_chq_dt = Convert.ToDateTime(txtChqDate.Text.Trim());
        //            //glmfChq.vr_chq_ac = txtChqAcctNo.Text.Trim();
        //            string msg = objVoucher.SaveLoanAdvPaymentVoucherWithoutChq((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, true, enttyGlDet);
        //            if (msg == "ok")
        //            {
        //                vrRef = glmfdata.vrid.ToString().PadLeft(6, '0');
        //                IsSaved = true;
        //            }
        //            else
        //            {
        //                ucMessage.ShowMessage("Exception: " + msg, RMS.BL.Enums.MessageType.Error);
        //                IsSaved = false;
        //                return;
        //            }
        //        }
        //        /************************************************************/



        //        tblPlSalVou voucher = new tblPlSalVou();

        //        voucher.CompID =  Convert.ToByte(CompID);
        //        voucher.PayPerd = Convert.ToInt32(PayPeriod);
        //        if (IsSaved)
        //        {
        //            voucher.VouRef = "PL-" + vrRef;
        //            voucher.VouDat = VouDate;
        //        }
        //        else
        //        {
        //            voucher.VouRef = "00-000000";
        //            voucher.VouDat = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
        //        }
        //        voucher.CreatedBy = username;
        //        voucher.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        voucher.VouStatus = ddlAppStatus.SelectedValue;


        //        for (int i = 0; i < grdSalPay.Rows.Count; i++)
        //        {
        //            CheckBox chkBox = (CheckBox)grdSalPay.Rows[i].FindControl("chkIncludeInVoucher");
        //            if (chkBox.Checked)
        //            {
        //                tblPlSalVouDet voucherDet = new tblPlSalVouDet();

        //                voucherDet.EmpId = Convert.ToInt32(grdSalPay.DataKeys[i].Values[1]);
        //                voucherDet.PayAmt = Convert.ToInt32(grdSalPay.Rows[i].Cells[12].Text.Trim());

        //                enttyVouDet.Add(voucherDet);
        //            }
        //        }

        //        voucher.tblPlSalVouDets = enttyVouDet;

        //        if (SalPayBl.SaveSalaryPayment(voucher, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
        //        {
        //            ClearFields();
        //            BindGridPayments();
        //            ucMessage.ShowMessage("Salary payment saved successfully", RMS.BL.Enums.MessageType.Info);
        //        }
        //        //*********************************************************
        //    }
        //    catch (Exception ex)
        //    {
        //        ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
        //    }
        //}
        //private void Update()
        //{
        //    enttyGlDet.Clear();
        //    enttyVouDet.Clear();
        //    try
        //    {
        //      string username = "";
        //        if (Session["LoginID"] == null)
        //        {
        //            username = Request.Cookies["uzr"]["LoginID"];
        //        }
        //        else
        //        {
        //            username = Session["LoginID"].ToString();
        //        }

        //        if (username.Length > 20)
        //        {
        //            username = username.Substring(0, 19);
        //        }

        //        //*********************************************************

        //        bool IsSaved = false;

        //        if (ddlAppStatus.SelectedValue.Equals("A"))
        //        {
        //            Seq = 0;
        //            string vrNarr = GetNarration();

        //            if (Session["LoginID"] == null)
        //            {
        //                username = Request.Cookies["uzr"]["LoginID"];
        //            }
        //            else
        //            {
        //                username = Session["LoginID"].ToString();
        //            }

        //            if (username.Length > 15)
        //            {
        //                username = username.Substring(0, 14);
        //            }

        //            //int voucherTypeId = 3;
        //            int voucherTypeId = 6;
        //            decimal Financialyear = objVoucher.GetFinancialYearByDate(VouDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            string source = "PAY";
        //            int voucherno = objVoucher.GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], voucherTypeId, Financialyear,source);
        //            /*  ONE */
        //            Glmf_Data glmfdata = new Glmf_Data();
        //            glmfdata.br_id = BrID;
        //            glmfdata.Gl_Year = Financialyear;
        //            glmfdata.vt_cd = Convert.ToInt16(voucherTypeId);
        //            glmfdata.vr_no = voucherno;
        //            glmfdata.vr_dt = VouDate;
        //            glmfdata.vr_nrtn = vrNarr;
        //            glmfdata.vr_apr = 'P';
        //            glmfdata.updateby = username;
        //            glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            glmfdata.approvedby = username;
        //            glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            glmfdata.source = source;

        //            bool firstentry = true;
        //            for (int i = 0; i < grdSalPayView.Rows.Count; i++)
        //            {
        //                //CheckBox chkBox = (CheckBox)grdSalPayView.Rows[i].FindControl("chkIncludeInVoucher");
        //                //if (chkBox.Checked)
        //                //{
        //                EmpID = Convert.ToInt32(grdSalPayView.DataKeys[i].Values[1]);
        //                PayAmount = Convert.ToDecimal(grdSalPayView.Rows[i].Cells[12].Text.Trim());
        //                //TaxAmount = Convert.ToDecimal(grdSalPayView.Rows[i].Cells[7].Text.Trim());
        //                LoanAmount = Convert.ToDecimal(grdSalPayView.Rows[i].Cells[8].Text.Trim());
        //                OtrDed = Convert.ToDecimal(grdSalPayView.Rows[i].Cells[10].Text.Trim());
        //                MiscDed = Convert.ToDecimal(grdSalPayView.Rows[i].Cells[11].Text.Trim());
        //                //EobiAmount = Convert.ToDecimal(grdSalPayView.Rows[i].Cells[9].Text.Trim());
        //                ////IsSaved = SaveIPV();
        //                tblPlEmpData employee = new EmpBL().GetByID(EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //                if (firstentry)
        //                {
        //                    firstentry = false;
        //                    /*  TWO */
        //                    if (!string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).SalaryExpAct))
        //                    {
        //                        Glmf_Data_Det glDet1 = new Glmf_Data_Det();
        //                        Seq = Seq + 1;
        //                        glDet1.vr_seq = Seq;
        //                        glDet1.gl_cd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).SalaryExpAct;
        //                        glDet1.vrd_debit = Convert.ToDecimal(txtChqAmnt.Text)
        //                                            + Convert.ToDecimal(txtTaxDed.Text)
        //                                            + Convert.ToDecimal(txtLoanAdvDed.Text)
        //                                            + Convert.ToDecimal(txtEobiDed.Text)
        //                                            + Convert.ToDecimal(txtOtherDed.Text)
        //                                            + Convert.ToDecimal(txtMiscDed.Text);
        //                        glDet1.vrd_credit = 0;
        //                        glDet1.vrd_nrtn = "";
        //                        glDet1.cc_cd = null;
        //                        enttyGlDet.Add(glDet1);
        //                    }
        //                    else
        //                    {
        //                        ucMessage.ShowMessage("Staff salary A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                        return;
        //                    }
        //                    /*  THREE */
        //                    if (Convert.ToDecimal(txtTaxDed.Text) > 0)
        //                    {
        //                        if (!string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).TaxPayable))
        //                        {
        //                            Glmf_Data_Det glDet2 = new Glmf_Data_Det();
        //                            Seq = Seq + 1;
        //                            glDet2.vr_seq = Seq;
        //                            glDet2.gl_cd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).TaxPayable;
        //                            glDet2.vrd_debit = 0;
        //                            glDet2.vrd_credit = Convert.ToDecimal(txtTaxDed.Text);
        //                            glDet2.vrd_nrtn = "";
        //                            glDet2.cc_cd = null;
        //                            enttyGlDet.Add(glDet2);
        //                        }
        //                        else
        //                        {
        //                            ucMessage.ShowMessage("Tax A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                            return;
        //                        }
        //                    }
        //                    /*  FOUR */
        //                    //if (Convert.ToDecimal(txtLoanAdvDed.Text) > 0)
        //                    //{
        //                    //    if (!string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).LoansPayable))
        //                    //    {
        //                    //        Glmf_Data_Det glDet3 = new Glmf_Data_Det();
        //                    //        Seq = Seq + 1;
        //                    //        glDet3.vr_seq = Seq;
        //                    //        glDet3.gl_cd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).LoansPayable;
        //                    //        glDet3.vrd_debit = 0;
        //                    //        glDet3.vrd_credit = Convert.ToDecimal(txtLoanAdvDed.Text); ;
        //                    //        glDet3.vrd_nrtn = "";
        //                    //        glDet3.cc_cd = null;
        //                    //        enttyGlDet.Add(glDet3);
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        ucMessage.ShowMessage("Loan advance A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                    //        return;
        //                    //    }
        //                    //}
        //                    /*  FIVE */
        //                    if (Convert.ToDecimal(txtEobiDed.Text) > 0)
        //                    {
        //                        if (!string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).EOBIPayable))
        //                        {
        //                            Glmf_Data_Det glDet4 = new Glmf_Data_Det();
        //                            Seq = Seq + 1;
        //                            glDet4.vr_seq = Seq;
        //                            glDet4.gl_cd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).EOBIPayable; ;
        //                            glDet4.vrd_debit = 0;
        //                            glDet4.vrd_credit = Convert.ToDecimal(txtEobiDed.Text); ;
        //                            glDet4.vrd_nrtn = "";
        //                            glDet4.cc_cd = null;
        //                            enttyGlDet.Add(glDet4);
        //                        }
        //                        else
        //                        {
        //                            ucMessage.ShowMessage("EOBI payable A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                            return;
        //                        }
        //                    }
        //                }
        //                /*  FOUR */
        //                if (LoanAmount > 0)
        //                {
        //                    string glcd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscPayable + employee.EmpID.ToString().PadLeft(4, '0');
        //                    if (string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscPayable))
        //                    {
        //                        ucMessage.ShowMessage("Ctrl Employee A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                        return;
        //                    }
        //                    Glmf_Data_Det glDet3 = new Glmf_Data_Det();
        //                    Seq = Seq + 1;
        //                    glDet3.vr_seq = Seq;
        //                    glDet3.gl_cd = glcd;
        //                    glDet3.vrd_debit = 0;
        //                    glDet3.vrd_credit = LoanAmount;
        //                    glDet3.vrd_nrtn = "";
        //                    glDet3.cc_cd = null;
        //                    enttyGlDet.Add(glDet3);
        //                }
        //                /*  SIX */
        //                if (PayAmount > 0)
        //                {
        //                    Glmf_Data_Det glDet5 = new Glmf_Data_Det();
        //                    Seq = Seq + 1;
        //                    glDet5.vr_seq = Seq;
        //                    ////glDet2.gl_cd = prefBl.GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ctrl_Dept;
        //                    //if (string.IsNullOrEmpty(codeBl.GetByID(DeptID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).SalaryPayable))
        //                    //{
        //                    //    return false;
        //                    //}
        //                    //glDet2.gl_cd = codeBl.GetByID(DeptID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).SalaryPayable;

        //                    string glcd1 = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscPayable + employee.EmpID.ToString().PadLeft(4, '0');
        //                    if (string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscPayable))
        //                    {
        //                        ucMessage.ShowMessage("Ctrl Employee A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                        return;
        //                    }
        //                    glDet5.gl_cd = glcd1;
        //                    glDet5.vrd_debit = 0;
        //                    glDet5.vrd_credit = PayAmount;
        //                    glDet5.vrd_nrtn = "";
        //                    glDet5.cc_cd = null;
        //                    enttyGlDet.Add(glDet5);
        //                }
        //                /*  SEVEN */
        //                if (OtrDed > 0)
        //                {
        //                    string glcd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).OtrDed + employee.EmpID.ToString().PadLeft(4, '0');
        //                    if (string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).OtrDed))
        //                    {
        //                        ucMessage.ShowMessage("Ctrl other deduction A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                        return;
        //                    }
        //                    Glmf_Data_Det glDet7 = new Glmf_Data_Det();
        //                    Seq = Seq + 1;
        //                    glDet7.vr_seq = Seq;
        //                    glDet7.gl_cd = glcd;
        //                    glDet7.vrd_debit = 0;
        //                    glDet7.vrd_credit = OtrDed;
        //                    glDet7.vrd_nrtn = "";
        //                    glDet7.cc_cd = null;
        //                    enttyGlDet.Add(glDet7);
        //                }
        //                /*  EIGHT */
        //                if (MiscDed > 0)
        //                {
        //                    string glcd = codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscDed + employee.EmpID.ToString().PadLeft(4, '0');
        //                    if (string.IsNullOrEmpty(codeBl.GetByID(Convert.ToInt32(employee.DeptID), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).MiscDed))
        //                    {
        //                        ucMessage.ShowMessage("Ctrl misc. deduction A/C is missing, Plz update department", RMS.BL.Enums.MessageType.Error);
        //                        return;
        //                    }
        //                    Glmf_Data_Det glDet8 = new Glmf_Data_Det();
        //                    Seq = Seq + 1;
        //                    glDet8.vr_seq = Seq;
        //                    glDet8.gl_cd = glcd;
        //                    glDet8.vrd_debit = 0;
        //                    glDet8.vrd_credit = MiscDed;
        //                    glDet8.vrd_nrtn = "";
        //                    glDet8.cc_cd = null;
        //                    enttyGlDet.Add(glDet8);
        //                }
        //            }
        //            ///*  SEVEN */
        //            //Glmf_Data_chq glmfChq = new Glmf_Data_chq();
        //            //glmfChq.vr_chq_branch = hdnGlCode.Value;
        //            //glmfChq.vr_chq = txtChqNo.Text.Trim();
        //            //if (!string.IsNullOrEmpty(txtChqDate.Text))
        //            //    glmfChq.vr_chq_dt = Convert.ToDateTime(txtChqDate.Text.Trim());
        //            //glmfChq.vr_chq_ac = txtChqAcctNo.Text.Trim();
        //            string msg = objVoucher.SaveLoanAdvPaymentVoucherWithoutChq((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, true, enttyGlDet);
        //            if (msg == "ok")
        //            {
        //                vrRef = glmfdata.vrid.ToString().PadLeft(6, '0');
        //                IsSaved = true;
        //            }
        //            else
        //            {
        //                ucMessage.ShowMessage("Exception: " + msg, RMS.BL.Enums.MessageType.Error);
        //                IsSaved = false;
        //                return;
        //            }

        //        }
        //        //*********************************************************



        //        tblPlSalVou voucher = SalPayBl.GetVoucher(PayVrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //        if (IsSaved)
        //        {
        //            voucher.VouRef = "PL-" + vrRef;
        //            voucher.VouDat = VouDate;
        //        }
        //        else
        //        {
        //            voucher.VouRef = "00-000000";
        //            voucher.VouDat = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
        //        }
        //        voucher.VouStatus = ddlAppStatus.SelectedValue;

        //        if (SalPayBl.UpdateSalaryPayment(voucher, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
        //        {
        //            ClearFields();
        //            BindGridPayments();
        //            ucMessage.ShowMessage("Salary payment updated successfully", RMS.BL.Enums.MessageType.Info);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
        //    }
        //}
        //private bool Delete()
        //{
        //    enttyVouDet.Clear();

        //    tblPlSalVou voucher = SalPayBl.GetVoucher(PayVrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    voucher.VouStatus = "C";

        //    return SalPayBl.UpdateSalaryPayment(voucher, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        // }
        //private void ClearFields()
        //{
        //    PayPeriod = "";
        //    PayVrID = 0;

        //    txtChqAmnt.Text = "";
        //    //txtChqBranch.Text = "";
        //    //txtChqAcctNo.Text = "";
        //    //txtChqNo.Text = "";
        //    //txtChqDate.Text = "";
        //    txtTaxDed.Text = "";
        //    txtLoanAdvDed.Text = "";
        //    txtEobiDed.Text = "";
        //    txtOtherDed.Text = "";
        //    txtMiscDed.Text = "";
        //    ddlAppStatus.SelectedValue = "P";

        //    divSearch.Visible = true;
        //    divView.Visible = false;
        //    btnDelete.Visible = false;
        //    ucButtons.EnableSave();
        //    IsEdit = false;

        //    viewer.Reset();
        //    grdSalPay.DataSource = string.Empty;
        //    grdSalPay.DataBind();

        //    grdSalPay.SelectedIndex = -1;

        //    Session["GlAccCode"] = null;
        //}
        //private string GetNarration()
        //{
        //    string vrNarr = "";
        //    StringBuilder builder = new StringBuilder();

        //    builder.Append("Salary transfer for the pay period ");
        //    builder.Append(PayPeriod+", ").AppendLine();
        //    builder.Append(Department + " (Dept.)").AppendLine();
        //    builder.Append(!string.IsNullOrEmpty(BankDesc) ? ", " + BankDesc : "");


        //    vrNarr = builder.ToString();
        //    if (vrNarr.Length > 500)
        //    {
        //        vrNarr = vrNarr.Substring(0, 500);
        //    }
        //    return vrNarr;
        //}
        //private bool SaveIPV()
        //{



        //}

        //private bool SaveIPV1()
        //{
        //    string vrNarr = GetNarration();

        //    string username = "";
        //    if (Session["LoginID"] == null)
        //    {
        //        username = Request.Cookies["uzr"]["LoginID"];
        //    }
        //    else
        //    {
        //        username = Session["LoginID"].ToString();
        //    }

        //    if (username.Length > 15)
        //    {
        //        username = username.Substring(0, 14);
        //    }

        //    //int voucherTypeId = 3;
        //    int voucherTypeId = 6;
        //    decimal Financialyear = objVoucher.GetFinancialYearByDate(Convert.ToDateTime(txtChqDate.Text.Trim()), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    int voucherno = objVoucher.GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], voucherTypeId, Financialyear);

        //    Glmf_Data glmfdata = new Glmf_Data();
        //    glmfdata.br_id = BrID;
        //    glmfdata.Gl_Year = Financialyear;
        //    glmfdata.vt_cd = Convert.ToInt16(voucherTypeId);
        //    glmfdata.vr_no = voucherno;
        //    glmfdata.vr_dt = VouDate;

        //    glmfdata.vr_nrtn = vrNarr;
        //    glmfdata.vr_apr = 'A';
        //    glmfdata.updateby = username;
        //    glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    glmfdata.approvedby = username;
        //    glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    glmfdata.source = "PAY";

        //    Glmf_Data_Det glDet1 = new Glmf_Data_Det();
        //    glDet1.vr_seq = 1;
        //    glDet1.gl_cd = hdnGlCode.Value;
        //    glDet1.vrd_debit = 0;
        //    glDet1.vrd_credit = Convert.ToDecimal(txtChqAmnt.Text);
        //    glDet1.vrd_nrtn = "";
        //    glDet1.cc_cd = null;

        //    enttyGlDet.Add(glDet1);

        //    Glmf_Data_Det glDet2 = new Glmf_Data_Det();
        //    glDet2.vr_seq = 2;
        //    //glDet2.gl_cd = prefBl.GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ctrl_Dept;
        //    if (string.IsNullOrEmpty(codeBl.GetByID(DeptID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).SalaryPayable))
        //    {
        //        return false;
        //    }
        //    glDet2.gl_cd = codeBl.GetByID(DeptID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).SalaryPayable;
        //    glDet2.vrd_debit = Convert.ToDecimal(txtChqAmnt.Text);
        //    glDet2.vrd_credit = 0;
        //    glDet2.vrd_nrtn = "";
        //    glDet2.cc_cd = null;

        //    enttyGlDet.Add(glDet2);

        //    Glmf_Data_chq glmfChq = new Glmf_Data_chq();
        //    glmfChq.vr_chq_branch = hdnGlCode.Value;
        //    glmfChq.vr_chq = txtChqNo.Text.Trim();
        //    glmfChq.vr_chq_dt = Convert.ToDateTime(txtChqDate.Text.Trim());
        //    glmfChq.vr_chq_ac = txtChqAcctNo.Text.Trim();
        //    string msg = objVoucher.SaveLoanAdvPaymentVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, glmfChq, true, enttyGlDet);
        //    if (msg == "ok")
        //    {
        //        vrRef = glmfdata.vrid.ToString().PadLeft(6, '0');
        //        return true;
        //    }
        //    else
        //    {
        //        ucMessage.ShowMessage("Exception: " + msg, RMS.BL.Enums.MessageType.Error);
        //        return false;
        //    }
        //}
        //protected void BindGridSalPay()
        //{
        //    if (ddlPayType.SelectedValue.Equals("All") || ddlPayType.SelectedValue.Equals("Cash"))
        //    {
        //        this.grdSalPay.DataSource = SalBl.GetSalPaymentSearch(CompID, Convert.ToInt32(ddlPayPerd.SelectedValue), Convert.ToInt32(ddlDept.SelectedValue), MinSal, MaxSal, ddlPayType.SelectedValue, ddlJobType.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    }
        //    else
        //    {
        //        this.grdSalPay.DataSource = SalBl.GetSalPaymentSearch(CompID, Convert.ToInt32(ddlPayPerd.SelectedValue), Convert.ToInt32(ddlDept.SelectedValue), MinSal, MaxSal, ddlBank.SelectedValue, ddlJobType.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    }
        //    this.grdSalPay.DataBind();
        //}
        //protected void BindGridSalPayView()
        //{
        //    List<spSalaryPaymentGridResult> sal = SalBl.GetSalPaymentGrid(CompID, Convert.ToInt32(PayPeriod), PayVrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    this.grdSalPayView.DataSource = sal;
        //    this.grdSalPayView.DataBind();
        //    try
        //    {
        //        decimal tax = 0, loanAdv = 0, eobided = 0, otrded = 0, miscded = 0;
        //        string bnkName = sal.First().bankname;
        //        bool flag = false;
        //        foreach (var s in sal)
        //        {
        //            tax = tax + Convert.ToDecimal(s.taxded);
        //            loanAdv = loanAdv + Convert.ToDecimal(s.loanadvded);
        //            eobided = eobided + Convert.ToDecimal(s.eobided);
        //            otrded = otrded + Convert.ToDecimal(s.otrded);
        //            miscded = miscded + Convert.ToDecimal(s.messded);
        //            if (s.bankname != bnkName)
        //            {
        //                bnkName = s.bankname;
        //                flag = true;
        //            }
        //        }
        //        txtTaxDed.Text = tax.ToString();
        //        txtLoanAdvDed.Text = loanAdv.ToString();
        //        txtEobiDed.Text = eobided.ToString();
        //        txtOtherDed.Text = otrded.ToString();
        //        txtMiscDed.Text = miscded.ToString();
        //        if (!flag)
        //        {
        //            BankCode = sal.First().bank;
        //        }
        //        else
        //        {
        //            BankCode = "-";
        //        }
        //    }
        //    catch { }
        //}
        //protected void BindGridPayments()
        //{
        //    grdPayments.DataSource = SalPayBl.GetSalPayment(CompID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    grdPayments.DataBind();
        //}
        //protected void GetByID()
        //{
        //    //ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        //}
        //private void FillDropDownCodeDept()
        //{
        //    tblPlCode pl = new tblPlCode();
        //    byte _cmp = 1;
        //    if (Session["CompID"] == null)
        //    {
        //        _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
        //    }
        //    else
        //    {
        //        _cmp = Convert.ToByte(Session["CompID"].ToString());
        //    }
        //    pl.CompID = _cmp;
        //    pl.CodeTypeID = 3;

        //    this.ddlDept.DataTextField = "CodeDesc";
        //    ddlDept.DataValueField = "CodeID";
        //    ddlDept.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlDept.DataBind();

        //}
        //private void FillDropDownPayPeriod()
        //{
        //    this.ddlPayPerd.DataTextField = "PayPerd";
        //    ddlPayPerd.DataValueField = "PayPerd";
        //    ddlPayPerd.DataSource = SalBl.GetPayPeriods((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlPayPerd.DataBind();
        //}
        //private void FillDDlBankBranch()
        //{
        //    ddlBank.DataValueField = "BankCode";
        //    ddlBank.DataTextField = "BankBranchName";
        //    ddlBank.DataSource = SalPayBl.GetBankBranch((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlBank.DataBind();
        //}
        //[WebMethod]
        //public static List<spGetBankA_CResult> GetBranch(string bank)
        //{
        //    voucherDetailBL vrBl = new voucherDetailBL();
        //    List<spGetBankA_CResult> acc;
        //    string glcd = "-"; ;

        //    if(HttpContext.Current.Session["GlAccCode"] != null)
        //        glcd = HttpContext.Current.Session["GlAccCode"].ToString();

        //    if (!glcd.Equals("-"))
        //    {
        //        acc = vrBl.GetBranch(bank, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]).Where(ac => ac.gl_cd.Equals(glcd)).ToList();
        //    }
        //    else
        //    {
        //        acc = vrBl.GetBranch(bank, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
        //    }
        //    return acc;
        //}

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BrID == 1)
            {
                DivisionID = Convert.ToInt32(ddlBranchDropdown.SelectedValue);
                RMSDataContext db = new RMSDataContext();

                this.ddlEmployee.DataTextField = "FullName";
                ddlEmployee.DataValueField = "EmpID";

                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == DivisionID).ToList();
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));

            }
            else
            {
                RMSDataContext db = new RMSDataContext();

                this.ddlEmployee.DataTextField = "FullName";
                ddlEmployee.DataValueField = "EmpID";

                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BrID).ToList();
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));
            }
        }

        private void FillDropdownCascadeDropdown()
        {
            RMSDataContext db = new RMSDataContext();
            ddlBranchDropdown.DataTextField = "br_nme";
            ddlBranchDropdown.DataValueField = "br_id";
            ddlBranchDropdown.DataSource = db.Branches.Where(x => x.cascad == 1).ToList();
            ddlBranchDropdown.DataBind();
            ddlBranchDropdown.Items.Insert(0, new ListItem("Select Division", "0"));
        }

        protected void ddlJobtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlJobType.SelectedValue.Equals("0"))
            {
                int jobtype = Convert.ToInt32(ddlJobType.SelectedValue.Trim());
                int brr = Convert.ToInt32(ddlBranchDropdown.SelectedValue);
                int v = brr != 0 ? brr : BrID;
                RMSDataContext db = new RMSDataContext();
                ddlEmployee.Controls.Clear();
                ddlEmployee.DataTextField = "FullName";
                ddlEmployee.DataValueField = "EmpID";

                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.JobNameID == jobtype && x.BranchID ==v) .ToList();
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));
                //FillSearchDropDownEmployee();

                //ddlJobType.SelectedValue = "0";
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetCascadingEmployeeList(int empType)
        {

            //Here MyDatabaseEntities  is our dbContext
            int brID = 0;


            brID = Convert.ToInt32(HttpContext.Current.Session["BranchID"].ToString());
            // bool isDisplay = false;

           
                using (RMSDataContext db = new RMSDataContext())
                {
                    var data = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.JobNameID == empType && x.BranchID == BrID).OrderBy(x => x.FullName).ToList();


                    //if (brID > 1)
                    //{
                    //    Branch branchObj = db.Branches.Where(x => x.br_id == brID).FirstOrDefault();
                    //    List<tblPlEmpData> empBranchList = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.BranchID == brID && x.jobtype == empType).ToList();
                    //    data = empBranchList.OrderBy(x => x.FullName).ToList();
                    //    if (branchObj.IsDisplay == true)
                    //    {
                    //        List<tblPlEmpData> empSubBranchList = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.Branch1.br_idd == brID && x.jobtype == empType).ToList();
                    //        data = empBranchList.Concat(empSubBranchList).OrderBy(x => x.FullName).ToList();

                    //    }


                    //}


                    var EmpData = new object[data.Count + 1];
                    EmpData[0] = new object[]{
                    "0",
                "All"
            };

                    int j = 0;
                    foreach (var i in data)
                    {
                        j++;
                        EmpData[j] = new object[] { i.EmpID.ToString(), i.FullName };
                    }


                    return EmpData;
                }
            

        }

        #endregion

    }
}

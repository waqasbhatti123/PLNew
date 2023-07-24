using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.profile
{
    public partial class EmpSalaryPackage : BasePage
    {
#pragma warning disable CS0114 // 'EmpSalaryPackage.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int? ID
#pragma warning restore CS0114 // 'EmpSalaryPackage.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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

        public static int BranchID;
        

        public bool IsSearch
        {
            get { return (ViewState["IsSearch"] == null) ? false : Convert.ToBoolean(ViewState["IsSearch"]); }
            set { ViewState["IsSearch"] = value; }
        }

        public static int IsBranch
        {
            get; set;
        }

        public static int IsEmp
        {
            get; set;
        }
        //{
        //    get { return (ViewState["IsBranch"] == null) ? 0 : Convert.ToInt32(ViewState["IsBranch"]); }
        //    set { ViewState["IsBranch"] = value; }
        //}

        PlAllowBL allowBL = new PlAllowBL();

        protected void Page_Init(object sender, EventArgs e)
        {

            DynamicGrid();
            //select_changed(null, new EventArgs());
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                
                //if (Session["DateTimeFormat"] == null)
                //{
                //    Response.Redirect("~/login.aspx");
                //} 

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpSalaryPkg").ToString();

                //Response.Cookies["uzr"].Values["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Home").ToString();
                int GroupID = 0;
                if (Session["GroupID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                }
                else
                {
                    GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                }

                if (Session["DateFormat"] == null)
                {
                    txtEffDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtfromPeridCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtToPeriodCal.Format = Request.Cookies["uzr"]["DateFormat"];
                }
                else
                {
                    txtEffDateCal.Format = Session["DateFormat"].ToString();
                    txtfromPeridCal.Format = Session["DateFormat"].ToString();
                    txtToPeriodCal.Format = Session["DateFormat"].ToString();
                }


                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }

                FillddlJobType();
                FillDropDownEmployee();
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BindSalaryPackage(BranchID, "");
                FillDropdownCascadeDropdown();

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

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BranchID == 1)
            {
                IsBranch = Convert.ToInt32(ddlBranchDropdown.SelectedValue);
                RMSDataContext db = new RMSDataContext();

                this.ddlEmployee.DataTextField = "FullName";
                ddlEmployee.DataValueField = "EmpID";

                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == IsBranch).ToList();
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));

            }
            else
            {
                RMSDataContext db = new RMSDataContext();

                this.ddlEmployee.DataTextField = "FullName";
                ddlEmployee.DataValueField = "EmpID";

                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));
            }
        }

        protected void ddlJobtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlJobType.SelectedValue.Equals("0"))
            {
                int jobtype = Convert.ToInt32(ddlJobType.SelectedValue.Trim());
                int brr = Convert.ToInt32(ddlBranchDropdown.SelectedValue);
                int v = brr != 0 ? brr : BranchID;
                RMSDataContext db = new RMSDataContext();
                ddlEmployee.Controls.Clear();
                ddlEmployee.DataTextField = "FullName";
                ddlEmployee.DataValueField = "EmpID";

                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.JobNameID == jobtype && x.BranchID == v).ToList();
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));
                //FillSearchDropDownEmployee();

                //ddlJobType.SelectedValue = "0";
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

        private void FillDropDownEmployee()
        {
            RMSDataContext db = new RMSDataContext();
            
            this.ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataValueField = "EmpID";
            if (BranchID == 1)
            {
               // int br = Convert.ToInt32(ddlBranchDropdown.SelectedValue);
                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
            }
            else
            {
                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
            }
            
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));
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


        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                    IsSearch = true;
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    FillDropDownEmployee();
                    BindSalaryPackage(BranchID, "");


                }

            }
            catch
            { }
        }

        protected void select_change(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedValue != "0" || ddlEmployee.SelectedValue != "")
            {
                IsEmp = Convert.ToInt32(ddlEmployee.SelectedValue);
            }
        }

        //protected void select_changed(object sender, EventArgs e)
        //{
        //    int bas = 0;
        //    if (txtBasicPay.Text != "")
        //    {
        //         bas = Convert.ToInt32(txtBasicPay.Text.Trim());
        //    }
        //    else
        //    {
        //        bas = 0;
        //    }
        //    using (RMSDataContext db = new RMSDataContext())
        //    {
        //        //int empdd = Convert.ToInt32(ddlEmployee.SelectedValue);
        //        //int? scal = db.tblPlEmpDatas.Where(x => x.EmpID == empdd).FirstOrDefault().ScaleID;
        //        //string scaleI = db.TblEmpScales.Where(x => x.ScaleID == scal).FirstOrDefault().ScaleName;
        //        var dataAllowance = db.SalaryContents.Where(x => x.IsActive == true && x.SalaryContentTypeID == 1).OrderBy(x => x.Sort).ToList();
        //        foreach (var allowitem in dataAllowance)
        //        {

        //            if (bas != 0)
        //            {
        //                if (allowitem.Name == "Adhoc Relief Allowance 2016 @ 10%" || allowitem.Name == "Adhoc Relief Allowance 2017 @ 10%")
        //                {
        //                    TextBox LabelAllowanceTextBox = new TextBox();
        //                    LabelAllowanceTextBox.Text = allowitem.Name;
        //                    LabelAllowanceTextBox.CssClass = "form-control";
        //                    EmployAllowanceLableGridDetail.Controls.Add(LabelAllowanceTextBox);

        //                    TextBox ValueAllownceTextBox = new TextBox();
        //                    //Assigning the textbox ID name 
        //                    ValueAllownceTextBox.ID = allowitem.SalaryContentID.ToString();
        //                    ValueAllownceTextBox.CssClass = "form-control";
        //                    ValueAllownceTextBox.Text = ((bas * 10) / 100).ToString();
        //                    EmployAllowanceGridDetail.Controls.Add(ValueAllownceTextBox);
        //                }
        //                if (allowitem.Name == "Adhoc Relief Allowance 2018 @ 10%")
        //                {

        //                    TextBox LabelAllowanceTextBox = new TextBox();
        //                    LabelAllowanceTextBox.Text = allowitem.Name;
        //                    LabelAllowanceTextBox.CssClass = "form-control";
        //                    EmployAllowanceLableGridDetail.Controls.Add(LabelAllowanceTextBox);

        //                    TextBox ValueAllownceTextBox = new TextBox();
        //                    //Assigning the textbox ID name 
        //                    ValueAllownceTextBox.ID = allowitem.SalaryContentID.ToString();
        //                    ValueAllownceTextBox.CssClass = "form-control";
        //                    ValueAllownceTextBox.Text = ((bas * 10) / 100).ToString();
        //                    EmployAllowanceGridDetail.Controls.Add(ValueAllownceTextBox);
        //                }
        //                if (allowitem.Name == "Adhoc Relief Allowance 2019 @ 10%")
        //                {
        //                    TextBox LabelAllowanceTextBox = new TextBox();
        //                    LabelAllowanceTextBox.Text = allowitem.Name;
        //                    LabelAllowanceTextBox.CssClass = "form-control";
        //                    EmployAllowanceLableGridDetail.Controls.Add(LabelAllowanceTextBox);

        //                    TextBox ValueAllownceTextBox = new TextBox();
        //                    //Assigning the textbox ID name 
        //                    ValueAllownceTextBox.ID = allowitem.SalaryContentID.ToString();
        //                    ValueAllownceTextBox.CssClass = "form-control";
        //                    ValueAllownceTextBox.Text = ((bas * 10) / 100).ToString();
        //                    EmployAllowanceGridDetail.Controls.Add(ValueAllownceTextBox);

        //                }
        //                if (allowitem.Name == "Cultural Allowance 30% Running B/P")
        //                {
        //                    TextBox LabelAllowanceTextBox = new TextBox();
        //                    LabelAllowanceTextBox.Text = allowitem.Name;
        //                    LabelAllowanceTextBox.CssClass = "form-control";
        //                    EmployAllowanceLableGridDetail.Controls.Add(LabelAllowanceTextBox);

        //                    TextBox ValueAllownceTextBox = new TextBox();
        //                    //Assigning the textbox ID name 
        //                    ValueAllownceTextBox.ID = allowitem.SalaryContentID.ToString();
        //                    ValueAllownceTextBox.CssClass = "form-control";
        //                    ValueAllownceTextBox.Text = ((bas * 30) / 100).ToString();
        //                    EmployAllowanceGridDetail.Controls.Add(ValueAllownceTextBox);
        //                }

        //            }
        //        }

        //        var dataDeduction = db.SalaryContents.Where(x => x.IsActive == true && x.SalaryContentTypeID == 2).OrderBy(x => x.Sort).ToList();
        //        foreach (var deductionitem in dataDeduction)
        //        {
        //            if (bas != 0)
        //            {
        //                if (deductionitem.Name == "CPF")
        //                {
        //                    TextBox LabelDeductionTextBox = new TextBox();
        //                    LabelDeductionTextBox.Text = deductionitem.Name;
        //                    LabelDeductionTextBox.CssClass = "form-control";
        //                    EmployDeductionLableGridDetail.Controls.Add(LabelDeductionTextBox);


        //                    TextBox ValueDEductionTextBox = new TextBox();
        //                    //Assigning the textbox ID name 
        //                    ValueDEductionTextBox.ID = deductionitem.SalaryContentID.ToString();
        //                    ValueDEductionTextBox.CssClass = "form-control";
        //                    ValueDEductionTextBox.Text = ((bas * 10) / 100).ToString();
        //                    EmployDeductionGridDetail.Controls.Add(ValueDEductionTextBox);
        //                }
        //            }

        //        }

        //       // DynamicGrid();

        //    }
        //}

        public void DynamicGrid()
        {

            using (RMSDataContext db = new RMSDataContext())
            {
                int empp;
                int? empi;
                int? SclName;
                if ((IsEmp == 0))
                {
                    empp = 0;
                }
                else
                {
                    empp = IsEmp;
                }
                if (empp == 0)
                {
                    empi = 0;
                }
                else
                {
                    empi = db.tblPlEmpDatas.Where(x => x.EmpID == empp).FirstOrDefault().ScaleID;
                }
                if (empi == 0)
                {
                    SclName = 0;
                }
                else
                {
                    SclName = db.TblEmpScales.Where(x => x.ScaleID == empi).FirstOrDefault().Orderby;
                }
                 
                var dataAllowance = db.SalaryContents.Where(x => x.IsActive == true && x.SalaryContentTypeID == 1).OrderBy(x => x.Sort).ToList();
                foreach (var allowitem in dataAllowance)
                {
                    //if ((allowitem.Name != "Adhoc Relief Allowance 2016 @ 10%" && allowitem.Name != "Adhoc Relief Allowance 2017 @ 10%" && 
                    //    allowitem.Name != "Adhoc Relief Allowance 2018 @ 10%" && allowitem.Name != "Adhoc Relief Allowance 2019 @ 10%" &&
                    //    allowitem.Name != "Adhoc Relief Allowance 2021 @ 10%" && allowitem.Name != "Cultural Allowance 30% Running B/P"))
                    //{
                        TextBox LabelAllowanceTextBox = new TextBox();
                        LabelAllowanceTextBox.Text = allowitem.Name;
                    
                        LabelAllowanceTextBox.CssClass = "form-control";
                        
                        EmployAllowanceLableGridDetail.Controls.Add(LabelAllowanceTextBox);


                        TextBox ValueAllownceTextBox = new TextBox();
                        //Assigning the textbox ID name 
                        ValueAllownceTextBox.ID = allowitem.SalaryContentID.ToString();
                    if (allowitem.SalaryContentID >= 40 && allowitem.SalaryContentID <= 41)
                    {
                        ValueAllownceTextBox.CssClass = "form-control cal10";
                    }
                    else if (allowitem.SalaryContentID == 42)
                    {
                        if ( SclName >= 3 && SclName <= 18)
                        {
                            ValueAllownceTextBox.CssClass = "form-control cal19";
                        }
                        else
                        {
                            ValueAllownceTextBox.CssClass = "form-control cal5";
                        }
                        
                    }
                    else if (allowitem.SalaryContentID == 96)
                    {
                        ValueAllownceTextBox.CssClass = "form-control cal21";
                    }
                    else if (allowitem.SalaryContentID == 43) {
                        ValueAllownceTextBox.CssClass = "form-control cal30";
                    }
                    else
                    {
                        ValueAllownceTextBox.CssClass = "form-control";
                    }
                        ValueAllownceTextBox.TextMode = TextBoxMode.Number;
                        EmployAllowanceGridDetail.Controls.Add(ValueAllownceTextBox);
                    }

                    
                //}

                var dataDeduction = db.SalaryContents.Where(x => x.IsActive == true && x.SalaryContentTypeID == 2).OrderBy(x => x.Sort).ToList();
                foreach (var deductionitem in dataDeduction)
                {
                    //if (deductionitem.Name != "CPF")
                    //{
                        TextBox LabelDeductionTextBox = new TextBox();
                        LabelDeductionTextBox.Text = deductionitem.Name;
                        LabelDeductionTextBox.CssClass = "form-control";
                        EmployDeductionLableGridDetail.Controls.Add(LabelDeductionTextBox);


                        TextBox ValueDEductionTextBox = new TextBox();
                        //Assigning the textbox ID name 
                        ValueDEductionTextBox.ID = deductionitem.SalaryContentID.ToString();
                    if (deductionitem.SalaryContentID == 47)
                    {
                        ValueDEductionTextBox.CssClass = "form-control cal10";
                    }
                    else
                    {
                        ValueDEductionTextBox.CssClass = "form-control";
                    }

                     
                        ValueDEductionTextBox.TextMode = TextBoxMode.Number;
                        EmployDeductionGridDetail.Controls.Add(ValueDEductionTextBox);
                    //}
                }

            }


        }


        protected void Button_Command(object sender, EventArgs e)
        {

           
            int selectedEmp = Convert.ToInt32(ddlEmployee.SelectedValue);
            if (selectedEmp < 1)
            {
                ucMessage.ShowMessage("Please Select Employee", RMS.BL.Enums.MessageType.Error);
                return;
            }
            if (txtEffDate.Text.Trim() ==  "" || txtEffDate.Text.Trim() == null)
            {
                ucMessage.ShowMessage("Please Enter Effective Date", RMS.BL.Enums.MessageType.Error);
                return;
            }
            if (txtBasicPay.Text.Trim() == "" || txtBasicPay.Text.Trim() == null)
            {
                ucMessage.ShowMessage("Please Enter Basic Pay", RMS.BL.Enums.MessageType.Error);
                return;
            }

            if(ID == 0)
            {
                try
                {
                    DateTime EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());
                    if (EffDate.Date > DateTime.Now.Date)
                    {
                        ucMessage.ShowMessage("Effective Date shuould not be less than today date", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                }
                catch
                {
                    ucMessage.ShowMessage("Please enter correct effective date", RMS.BL.Enums.MessageType.Error);
                    return;
                }
            }
          




            InsertOrUpdate();
        }

        protected void Clear_All(object sender, EventArgs e)
        {
            ClearFields();
        }




        protected void InsertOrUpdate()
        {
            try
            {
                byte _cmp = 1;
                if (Session["CompID"] == null)
                {
                    _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                }
                else
                {
                    _cmp = Convert.ToByte(Session["CompID"].ToString());
                }


                RMSDataContext db = new RMSDataContext();

                int empID = Convert.ToInt32(ddlEmployee.SelectedValue.Trim());
                DateTime effDate = Convert.ToDateTime(txtEffDate.Text.Trim());

                decimal basicPay = 0;
                if (!txtBasicPay.Text.Trim().Equals(""))
                {
                    basicPay = Convert.ToDecimal(txtBasicPay.Text.Trim());
                }


                List<tblPlSalaryDetail> tblPlSalaryDetailList = new List<tblPlSalaryDetail>();

                decimal totalAllowances = 0;
                decimal totaldeduction = 0;

                foreach (TextBox txtAll in EmployAllowanceGridDetail.Controls.OfType<TextBox>())
                {
                    int allowanceID = Convert.ToInt32(txtAll.ID.Trim());
                    tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                    if (txtAll.Text.Trim() == "" || txtAll.Text.Trim() == null)
                    {
                        txtAll.Text = "0";
                    }
                     decimal allowanceVal = Convert.ToDecimal(txtAll.Text.Trim());
                    tblPlSalaryDetailObj.SizeValue = allowanceVal;
                    tblPlSalaryDetailObj.SalaryContentID = allowanceID;
                    
                    
                    if (checkIsActive.Checked == true)
                    {
                        tblPlSalaryDetailObj.IsActive = true;
                    }
                    else
                    {
                        tblPlSalaryDetailObj.IsActive = false;
                    }

                    tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);


                    //if (allowanceVal != null)
                    //{

                    //    SalaryContent salaryContent = db.SalaryContents.Where(x => x.SalaryContentID == allowanceID).FirstOrDefault();
                    //    if (salaryContent != null)
                    //    {
                    //        tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                    //        bool isValue = salaryContent.IsValue;

                    //        if (isValue == true)
                    //        {
                    //            totalAllowances += salaryContent.Size;
                    //        }
                    //        else
                    //        {
                    //            decimal actutalAllowance = (salaryContent.Size / 100) * basicPay;
                    //            totalAllowances += actutalAllowance;
                    //        }
                    //        tblPlSalaryDetailObj.SalaryContentID = allowanceID;
                    //        tblPlSalaryDetailObj.IsActive = true;
                    //        tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                    //    }
                    //}

                    //else
                    //{
                    //    var chkingExist = db.tblPlSalaryDetails.Where(x => x.CompID == _cmp && x.EmpID == empID && x.EffDate == effDate && x.SalaryContentID == allowanceID && x.IsActive == true).FirstOrDefault();
                    //    SalaryContent salaryContent = db.SalaryContents.Where(x => x.SalaryContentID == allowanceID).FirstOrDefault();

                    //    if (chkingExist != null)
                    //    {
                    //        tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                    //        bool isValue = salaryContent.IsValue;

                    //        if (isValue == true)
                    //        {
                    //            totalAllowances -= salaryContent.Size;
                    //        }
                    //        else
                    //        {
                    //            decimal actutalAllowance = (salaryContent.Size / 100) * basicPay;
                    //            totalAllowances -= actutalAllowance;
                    //        }
                    //        tblPlSalaryDetailObj.SalaryContentID = allowanceID;
                    //        tblPlSalaryDetailObj.IsActive = false;
                    //        tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                    //    }
                    //}

                }
                foreach (TextBox txtDd in EmployDeductionGridDetail.Controls.OfType<TextBox>())
                {
                    int deductionID = Convert.ToInt32(txtDd.ID.Trim());
                    tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                    if (txtDd.Text.Trim() == "" || txtDd.Text.Trim() == null)
                    {
                        txtDd.Text = "0";
                    }
                  
                    tblPlSalaryDetailObj.SalaryContentID = deductionID;
                    decimal ddVal = Convert.ToDecimal(txtDd.Text.Trim());
                    tblPlSalaryDetailObj.SizeValue = ddVal;

                    tblPlSalaryDetailObj.IsActive = checkIsActive.Checked;
                    tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                    //if (chkDd.Checked == true)
                    //{

                    //    SalaryContent salaryContent = db.SalaryContents.Where(x => x.SalaryContentID == deductionID).FirstOrDefault();
                    //    if (salaryContent != null)
                    //    {
                    //        tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                    //        bool isValue = salaryContent.IsValue;

                    //        if (isValue == true)
                    //        {
                    //            totaldeduction += salaryContent.Size;
                    //            tblPlSalaryDetailObj.SalaryContentID = deductionID;

                    //        }
                    //        else
                    //        {
                    //            decimal actutalDeduction = (salaryContent.Size / 100) * basicPay;
                    //            totaldeduction += actutalDeduction;
                    //            tblPlSalaryDetailObj.SalaryContentID = deductionID;
                    //        }
                    //        tblPlSalaryDetailObj.IsActive = true;
                    //        tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                    //    }
                    //}

                    //else
                    //{
                    //    var chkingExist = db.tblPlSalaryDetails.Where(x => x.CompID == _cmp && x.EmpID == empID && x.EffDate == effDate && x.SalaryContentID == deductionID && x.IsActive == true).FirstOrDefault();
                    //    SalaryContent salaryContent = db.SalaryContents.Where(x => x.SalaryContentID == deductionID).FirstOrDefault();

                    //    if (chkingExist != null)
                    //    {
                    //        tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                    //        bool isValue = salaryContent.IsValue;

                    //        if (isValue == true)
                    //        {
                    //            totaldeduction -= salaryContent.Size;
                    //        }
                    //        else
                    //        {
                    //            decimal actutaldeduction = (salaryContent.Size / 100) * basicPay;
                    //            totaldeduction -= actutaldeduction;
                    //        }
                    //        tblPlSalaryDetailObj.SalaryContentID = deductionID;
                    //        tblPlSalaryDetailObj.IsActive = false;
                    //        tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                    //    }

                    //}

                }


                // Insertions or Updations 

                tblPlAlow checkIsExist = db.tblPlAlows.Where(x => x.CompID == _cmp && x.EmpID == empID && x.EffDate == effDate).FirstOrDefault();

                if (checkIsExist != null)
                {
                    decimal editTotalAllowance = 0;
                    decimal editTotalDeduction = 0;
                    if (checkIsExist.OtrAlow > totalAllowances)
                    {
                        editTotalAllowance = Math.Abs(checkIsExist.OtrAlow) - Math.Abs(totalAllowances);
                    }
                    else if (checkIsExist.OtrAlow < totalAllowances)
                    {
                        editTotalAllowance = Math.Abs(totalAllowances) - Math.Abs(checkIsExist.OtrAlow);
                    }
                    else
                    {
                        editTotalAllowance = Math.Abs(checkIsExist.OtrAlow);
                    }

                    //For Deduction
                    if (checkIsExist.OtherDed > totaldeduction)
                    {
                        editTotalDeduction = Math.Abs(checkIsExist.OtherDed) - Math.Abs(totaldeduction);
                    }
                    else if (checkIsExist.OtherDed < totaldeduction)
                    {
                        editTotalDeduction = Math.Abs(totaldeduction) - Math.Abs(checkIsExist.OtherDed);
                    }
                    else
                    {
                        editTotalDeduction = Math.Abs(checkIsExist.OtherDed);
                    }


                    //checkIsExist.OtrAlow = Math.Abs(editTotalAllowance);
                    //checkIsExist.OtherDed = Math.Abs(editTotalDeduction);
                    //decimal netpay = Convert.ToDecimal((checkIsExist.Basic + Math.Abs(editTotalAllowance)) - Math.Abs(editTotalDeduction));
                    //checkIsExist.NSHA = netpay;

                    checkIsExist.Basic = basicPay;
                    if (txtfromPerid.Text == "" || txtfromPerid.Text == null)
                    {
                        checkIsExist.fromPeriod = null;
                    }
                    else
                    {
                        checkIsExist.fromPeriod = Convert.ToDateTime(txtfromPerid.Text);
                    }
                    if (txtToPeriod.Text == "" || txtToPeriod.Text == null)
                    {
                        checkIsExist.toPeriod = null;
                    }
                    else
                    {
                        checkIsExist.toPeriod = Convert.ToDateTime(txtToPeriod.Text);
                    }
                    if (txtPerDay.Text == "" || txtPerDay.Text == null)
                    {
                        checkIsExist.perdayrate = null;
                    }
                    else
                    {
                        checkIsExist.perdayrate = Convert.ToInt32(txtPerDay.Text);
                    }
                    
                    if (checkIsActive.Checked == true)
                    {
                        checkIsExist.IsActive = true;
                    }
                    else
                    {
                        checkIsExist.IsActive = false;
                    }
                    
                    db.SubmitChanges();
                    string chkIsInsert = InsertSalaryPackageinDetails(checkIsExist.CompID, empID, effDate, tblPlSalaryDetailList);

                    if (chkIsInsert == "OK")
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                        ClearFields();
                        BindSalaryPackage(BranchID, "");
                    }
                    else
                    {
                        ucMessage.ShowMessage(chkIsInsert, RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {

                    List<tblPlAlow> EmpPreviousData = db.tblPlAlows.Where(x => x.EmpID == empID && x.IsActive == true).ToList();
                    foreach (tblPlAlow plAlowObj in EmpPreviousData)
                    {
                        plAlowObj.IsActive = false;
                        db.SubmitChanges();
                    }

                    string insertChk = insertinTblAllow(_cmp, empID, effDate, basicPay, tblPlSalaryDetailList);
                    if (insertChk == "OK")
                    {

                        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                        ClearFields();
                        BindSalaryPackage(BranchID, "");
                    }
                    else
                    {
                        ucMessage.ShowMessage(insertChk, RMS.BL.Enums.MessageType.Error);
                    }

                    //int checkIsExistMonthCount = db.tblPlAlows.Where(x => x.CompID == _cmp && x.EmpID == empID ).OrderByDescending(x => x.EffDate).Take(1).Count();
                    //if(checkIsExistMonthCount > 0)
                    //{
                    //    int checkIsExistMonthCouts = db.tblPlAlows.Where(x => x.CompID == _cmp && x.EmpID == empID && x.EffDate.Month == effDate.Month).OrderByDescending(x => x.EffDate).Take(1).Count();

                    //    if(checkIsExistMonthCouts > 0)
                    //    {
                    //        tblPlAlow checkIsExistMonth = db.tblPlAlows.Where(x => x.CompID == _cmp && x.EmpID == empID && x.EffDate.Month == effDate.Month).OrderByDescending(x => x.EffDate).Take(1).Single();

                    //        if (checkIsExistMonth != null)
                    //        {
                    //            //checkIsExistMonthCounts.EffDate = effDate;
                    //            checkIsExistMonth.Basic = basicPay;
                    //            db.SubmitChanges();
                    //            string chkInsert = InsertSalaryPackageinDetails(_cmp, empID, effDate, tblPlSalaryDetailList);

                    //            if (chkInsert == "OK")
                    //            {
                    //                ucMessage.ShowMessage("Month is Already Exist So, Effective Date is not Changed. Other Changes are updated", RMS.BL.Enums.MessageType.Error);
                    //                ClearFields();
                    //                BindSalaryPackage();
                    //            }
                    //            else
                    //            {
                    //                ucMessage.ShowMessage(chkInsert, RMS.BL.Enums.MessageType.Error);
                    //            }
                    //        }
                    //    }

                    //    else
                    //    {
                    //        string insertChk = insertinTblAllow(_cmp, empID, effDate, basicPay, tblPlSalaryDetailList);
                    //        if (insertChk == "OK")
                    //        {
                    //            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                    //            ClearFields();
                    //            BindSalaryPackage();
                    //        }
                    //        else
                    //        {
                    //            ucMessage.ShowMessage(insertChk, RMS.BL.Enums.MessageType.Error);
                    //        }
                    //    }


                    //}
                    //else
                    //{

                    //}

                }

            }

            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message.ToString(), RMS.BL.Enums.MessageType.Error);

            }



        }


        protected string insertinTblAllow(byte _cmp, int empID, DateTime effDate, decimal basicPay, List<tblPlSalaryDetail> tblPlSalaryDetailList)
        {
            try
            {
                RMSDataContext db = new RMSDataContext();

                tblPlAlow allow = new tblPlAlow();

                allow.CompID = _cmp;
                allow.EmpID = empID;
                allow.EffDate = effDate;
                allow.Basic = basicPay;
                if (txtfromPerid.Text == "" || txtfromPerid.Text == null)
                {
                    allow.fromPeriod = null;
                }
                else
                {
                    allow.fromPeriod = Convert.ToDateTime(txtfromPerid.Text);
                }
                if (txtToPeriod.Text == "" || txtToPeriod.Text == null)
                {
                    allow.toPeriod = null;
                }
                else
                {
                    allow.toPeriod = Convert.ToDateTime(txtToPeriod.Text);
                }
                if (txtPerDay.Text == "" || txtPerDay.Text == null)
                {
                    allow.perdayrate = null;
                }
                else
                {
                    allow.perdayrate = Convert.ToInt32(txtPerDay.Text);
                }
                //For Net Pay                
                allow.OtrAlow = 0;
                allow.OtherDed = 0;
                allow.NSHA = 0;

                allow.DecidePeriod = 0;
                allow.EDA = 0;
                allow.FuelLimit = 0;
                allow.HR = 0;
                allow.MessDed = 0;
                allow.MobLmt = 0;
                allow.CA = 0;
                allow.SplAlow = 0;
                allow.TaxDed = 0;
                allow.UniformAlow = 0;
                allow.Utilities = 0;
                if(checkIsActive.Checked == true)
                {
                    allow.IsActive = true;
                }
                else
                {
                    allow.IsActive = false;
                }
                db.tblPlAlows.InsertOnSubmit(allow);
                db.SubmitChanges();
                //int noOfRow = db.tblPlAlows.Count();
                string chkInsert = InsertSalaryPackageinDetails(allow.CompID, empID, effDate, tblPlSalaryDetailList);

                if (chkInsert == "OK")
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                    ClearFields();
                    BindSalaryPackage(BranchID, "");
                }
                else
                {
                    ucMessage.ShowMessage(chkInsert, RMS.BL.Enums.MessageType.Error);
                }


                return "OK";



            }

            
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
            
            }

        string InsertSalaryPackageinDetails(int compID, int empID,DateTime effDate, List<tblPlSalaryDetail> CheckedContentSalaryList)
        {
            try
            {
                using (RMSDataContext db = new RMSDataContext())
                {
                    for (int i = 0; i < CheckedContentSalaryList.Count; i++)
                    {
                        var scd = db.tblPlSalaryDetails.Where(x => x.CompID == compID && x.EffDate == effDate && x.EmpID == empID && x.SalaryContentID == CheckedContentSalaryList[i].SalaryContentID).SingleOrDefault();
                        if(scd != null)
                        {
                            scd.SizeValue = CheckedContentSalaryList[i].SizeValue;
                            if (checkIsActive.Checked == true)
                            {
                                scd.IsActive = true;
                            }
                            else
                            {
                                scd.IsActive = false;
                            }
                            
                            db.SubmitChanges();
                        }
                        else
                        {
                            List<tblPlSalaryDetail> chksdList = db.tblPlSalaryDetails.Where(x => x.CompID == compID && x.EmpID == empID && x.SalaryContentID == CheckedContentSalaryList[i].SalaryContentID && x.IsActive == true).ToList();
                            foreach (var item in chksdList)
                            {
                                item.IsActive = false;

                            }

                            tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                            tblPlSalaryDetailObj.CompID = compID;
                            tblPlSalaryDetailObj.EmpID = empID;
                            tblPlSalaryDetailObj.EffDate = effDate;
                            tblPlSalaryDetailObj.SalaryContentID = CheckedContentSalaryList[i].SalaryContentID;
                           // tblPlSalaryDetailObj.IsActive = CheckedContentSalaryList[i].IsActive;
                            tblPlSalaryDetailObj.SizeValue = CheckedContentSalaryList[i].SizeValue;
                            if (checkIsActive.Checked == true)
                            {
                                tblPlSalaryDetailObj.IsActive = true;
                            }
                            else
                            {
                                tblPlSalaryDetailObj.IsActive = false;
                            }

                            db.tblPlSalaryDetails.InsertOnSubmit(tblPlSalaryDetailObj);
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
            //return "OK";
        }


        private void ClearFields()
        {
            ID = 0;
           
            txtEffDate.Text = "";
            txtBasicPay.Text = "";
            txtfromPerid.Text = "";
            txtToPeriod.Text = "";
            txtPerDay.Text = "";
            checkIsActive.Checked = true;

            foreach (TextBox txtAll in EmployAllowanceGridDetail.Controls.OfType<TextBox>())
            {

                txtAll.Text= "" ;

            }
            foreach (TextBox chkDd in EmployDeductionGridDetail.Controls.OfType<TextBox>())
            {
                chkDd.Text = "";

            }
           
            ddlJobType.SelectedValue = "0";
            ddlEmployee.SelectedValue = "0";
            BindSalaryPackage(BranchID, "");

        }



        protected void grdSalaryPackage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSalaryPackage.PageIndex = e.NewPageIndex;
            BindSalaryPackage(BranchID, searchEmployeeTxt.Text.Trim());
        }

        protected void grdSalaryPackage_PageIndexChanged(object sender, EventArgs e)
        {

            ID = Convert.ToInt32(grdSalaryPackage.SelectedDataKey.Values["EmpID"].ToString());
            CompID = Convert.ToInt32(grdSalaryPackage.SelectedDataKey.Values["CompID"].ToString());
            EffDateStr = grdSalaryPackage.SelectedDataKey.Values["EffDate"].ToString();

            this.OnupdateEvent();
            BindSalaryPackage(BranchID, searchEmployeeTxt.Text.Trim());

            //ClearFields();

        }
        //int Counter = 1;
        protected void grdSalaryPackage_RowBound(object sender, GridViewRowEventArgs e)
        {
            grdRowBound(sender, e);

        }


        protected void grdRowBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[0].Text = Counter.ToString();

                int brID =Convert.ToInt32(grdSalaryPackage.DataKeys[e.Row.RowIndex].Values[3].ToString());

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                }

                if (e.Row.Cells[3].Text.Equals("True"))
                {
                    e.Row.Cells[3].Text = "Yes";
                    //e.Row.Cells[4].Style["visibility"] = "show";

                    //if (brID == BranchID)
                    //{
                    //    e.Row.Cells[3].Text = "Yes";
                    //    e.Row.Cells[4].Style["visibility"] = "show";
                    //}
                    //else
                    //{
                    //    e.Row.Cells[3].Text = "Yes";
                    //    e.Row.Cells[4].Text = "";
                    //}
                }
                else
                {
                    
                    e.Row.Cells[3].Text = "No";
                   // e.Row.Cells[4].Text = "";
                }

            }


        }

        //protected void grdSalaryPackage_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{
        //    GridViewRow gvr = grdSalaryPackage.Rows[e.NewSelectedIndex];

        //}


        //public void ToggleButtons(int selectedRowIndex)
        //{
        //    foreach (GridViewRow gvr in grdSalaryPackage.Rows)
        //    {
        //        bool show = (gvr.RowIndex == selectedRowIndex);
        //        LinkButton selectButton = (LinkButton)gvr.FindControl("LinkButtonSelect");
        //        if (selectButton != null)
        //            selectButton.Visible = !show;

        //        Panel p = (Panel)gvr.FindControl("Panel1");
        //        if (p != null)
        //        {
        //            p.Visible = show;
        //        }

        //        LinkButton linkButtonUpdate = (LinkButton)gvr.FindControl("LinkButtonUpdate");
        //        if (linkButtonUpdate != null)
        //            linkButtonUpdate.Visible = false;//reset
        //    }
        //}

        //protected void grdSalaryPackage_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if ((e.Row.RowType == DataControlRowType.DataRow &
        //        ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)))
        //    {
        //        LinkButton selectButton = (LinkButton)e.Row.FindControl("LinkButtonSelect");
        //        if (selectButton != null)
        //            selectButton.Visible = false;
        //        Panel p = (Panel)e.Row.FindControl("Panel1");
        //        if (p != null)
        //            p.Visible = false;
        //        LinkButton linkButtonUpdate = (LinkButton)e.Row.FindControl("LinkButtonUpdate");
        //        if (linkButtonUpdate != null)
        //            linkButtonUpdate.Visible = true;
        //    }
        //}





        string OnupdateEvent()
        {
            DateTime dateTimeeff = Convert.ToDateTime(EffDateStr);
            RMSDataContext db = new RMSDataContext();
            try
            {
                tblPlAlow empPojo = allowBL.GetByID(CompID, ID.Value, DateTime.Parse(EffDateStr), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if(empPojo != null)
                {
                    if (empPojo.IsActive == false)
                    {
                        checkIsActive.Visible = false;
                        lblActive.Visible = false;
                        Save.Visible = false;
                        Clear.Visible = false;
                    }
                    ddlEmployee.SelectedValue = empPojo.EmpID.ToString();
                    tblPlEmpData empData = db.tblPlEmpDatas.Where(x => x.EmpID == ID.Value).FirstOrDefault();
                    ddlJobType.SelectedValue = empData.JobNameID.ToString();
                    txtEffDate.Text = empPojo.EffDate.ToString();
                    if (Session["DateFullYearFormat"] == null)
                    {
                        this.txtEffDate.Text = empPojo.EffDate.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    }
                    else
                    {
                        this.txtEffDate.Text = empPojo.EffDate.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    }
                    txtBasicPay.Text = empPojo.Basic.ToString();
                    if (empPojo.fromPeriod == null)
                    {
                        txtfromPerid.Text = null;
                    }
                    else
                    {
                        txtfromPerid.Text = empPojo.fromPeriod.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    }
                    if (empPojo.toPeriod == null)
                    {
                        txtToPeriod.Text = null;
                    }
                    else
                    {
                        txtToPeriod.Text = empPojo.toPeriod.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    }
                    if (empPojo.perdayrate == null)
                    {
                        txtPerDay.Text = null;
                    }
                    else
                    {
                        txtPerDay.Text = empPojo.perdayrate.ToString();
                    }
                    checkIsActive.Checked =Convert.ToBoolean(empPojo.IsActive);
                    List<tblPlSalaryDetail> salaryDetailsList = db.tblPlSalaryDetails.Where(x => x.CompID == CompID && x.EmpID == ID && x.EffDate == dateTimeeff).ToList();
                    for(int i= 0; i< salaryDetailsList.Count; i++)
                    {
                        int typeContentID = salaryDetailsList[i].SalaryContentID;
                        foreach (TextBox txtAll in EmployAllowanceGridDetail.Controls.OfType<TextBox>())
                        {
                            int chkAllID = Convert.ToInt32(txtAll.ID);
                            if(chkAllID == typeContentID)
                            {
                                txtAll.Text = salaryDetailsList[i].SizeValue.ToString();
                            }
                            

                        }
                        foreach (TextBox txtDd in EmployDeductionGridDetail.Controls.OfType<TextBox>())
                        {
                            int chkDdID = Convert.ToInt32(txtDd.ID);
                            if (chkDdID == typeContentID)
                            {
                                txtDd.Text = salaryDetailsList[i].SizeValue.ToString();
                            }                     

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

        protected void Button_CommandSearch(object sender, EventArgs e)
        {
            BindSalaryPackage(BranchID, searchEmployeeTxt.Text.Trim());
        }
            protected void BindSalaryPackage(int brID, string empNme)
        {
            this.grdSalaryPackage.DataSource = allowBL.GetAllSalaryPackage(BranchID, empNme, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdSalaryPackage.DataBind();
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

                var data = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.JobNameID == empType && x.BranchID == BranchID).OrderBy(x => x.FullName).ToList();


                //if (brID > 1)
                //{
                //    Branch branchObj = db.Branches.Where(x => x.br_id == brID).FirstOrDefault();
                //    List<tblPlEmpData> empBranchList = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.BranchID == brID && x.jobtype == empType).ToList();
                //    data = empBranchList.OrderBy(x => x.FullName).ToList();
                //    if (branchObj.IsDisplay == true)
                //    {
                //        List<tblPlEmpData> empSubBranchList = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.Branch1.br_idd == brID && x.jobtype == empType && x.Branch1.br_status == true).ToList();
                //        data = empBranchList.Concat(empSubBranchList).OrderBy(x => x.FullName).ToList();

                //    }


                //}


                var EmpData = new object[data.Count + 1];
                EmpData[0] = new object[]{
                    "0",
                "Select Employee"
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

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<sp_GetEmployeeSearchResult> GetEmployee(string employee)
        {
            EmpProfileBL pro = new EmpProfileBL();
            List<sp_GetEmployeeSearchResult> emp = pro.GetEmployeeSearch(IsBranch, employee, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return emp;
        }
    }
}
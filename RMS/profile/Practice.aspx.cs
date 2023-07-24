using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.profile
{
    public partial class Practice : System.Web.UI.Page
    {


#pragma warning disable CS0114 // 'Practice.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int? ID
#pragma warning restore CS0114 // 'Practice.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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

        PlAllowBL allowBL = new PlAllowBL();

        protected void Page_Init(object sender, EventArgs e)
        {

            DynamicAllDdForm();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDownEmployee();
                BindSalaryPackage();
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
                }
                else
                {
                    txtEffDateCal.Format = Session["DateFormat"].ToString();
                }


            }
        }

        private void FillDropDownEmployee()
        {
            RMSDataContext db = new RMSDataContext();

            this.ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataValueField = "EmpID";
            ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1).OrderBy(x => x.FullName).ToList();
            ddlEmployee.DataBind();

        }



        public void DynamicAllDdForm()
        {

            using (RMSDataContext db = new RMSDataContext())
            {

                var dataAllowance = db.SalaryContents.Where(x => x.SalaryContentTypeID == 1 && x.IsActive == true).ToList();
                foreach (var allowitem in dataAllowance)
                {

                    TextBox LabelAllowanceTextBox = new TextBox();
                    LabelAllowanceTextBox.Text = allowitem.Name;
                    LabelAllowanceTextBox.CssClass = "form-control";
                    EmployAllowanceLableGridDetail.Controls.Add(LabelAllowanceTextBox);


                    CheckBox allowanceCheckBox = new CheckBox();
                    //Assigning the textbox ID name 
                    allowanceCheckBox.ID = allowitem.SalaryContentID.ToString();
                    allowanceCheckBox.CssClass = "form-control";
                    EmployAllowanceGridDetail.Controls.Add(allowanceCheckBox);
                }

                var dataDeduction = db.SalaryContents.Where(x => x.SalaryContentTypeID == 2 && x.IsActive == true).ToList();
                foreach (var deductionitem in dataDeduction)
                {
                    TextBox LabelDeductionTextBox = new TextBox();
                    LabelDeductionTextBox.Text = deductionitem.Name;
                    LabelDeductionTextBox.CssClass = "form-control";
                    EmployDeductionLableGridDetail.Controls.Add(LabelDeductionTextBox);


                    CheckBox deductionCheckBox = new CheckBox();
                    //Assigning the textbox ID name 
                    deductionCheckBox.ID = deductionitem.SalaryContentID.ToString();
                    deductionCheckBox.CssClass = "form-control";
                    EmployDeductionGridDetail.Controls.Add(deductionCheckBox);
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
            if (txtEffDate.Text.Trim() == "" || txtEffDate.Text.Trim() == null)
            {
                ucMessage.ShowMessage("Please Enter Effective Date", RMS.BL.Enums.MessageType.Error);
                return;
            }
            if (txtBasicPay.Text.Trim() == "" || txtBasicPay.Text.Trim() == null)
            {
                ucMessage.ShowMessage("Please Enter Basic Pay", RMS.BL.Enums.MessageType.Error);
                return;
            }

            try
            {
                DateTime EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());
                if (EffDate.Date < DateTime.Now.Date)
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

                foreach (CheckBox chkAll in EmployAllowanceGridDetail.Controls.OfType<CheckBox>())
                {
                    int allowanceID = Convert.ToInt32(chkAll.ID.Trim());
                    if (chkAll.Checked == true)
                    {

                        SalaryContent salaryContent = db.SalaryContents.Where(x => x.SalaryContentID == allowanceID).FirstOrDefault();
                        if (salaryContent != null)
                        {
                            tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                            bool isValue = Convert.ToBoolean(salaryContent.IsValue);

                            if (isValue == true)
                            {
                                totalAllowances += Convert.ToDecimal(salaryContent.Size);
                            }
                            else
                            {
                                decimal actutalAllowance = (Convert.ToDecimal(salaryContent.Size) / 100) * basicPay;
                                totalAllowances += actutalAllowance;
                            }
                            tblPlSalaryDetailObj.SalaryContentID = allowanceID;
                            tblPlSalaryDetailObj.IsActive = true;
                            tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                        }
                    }

                    else
                    {
                        var chkingExist = db.tblPlSalaryDetails.Where(x => x.CompID == _cmp && x.EmpID == empID && x.EffDate == effDate && x.SalaryContentID == allowanceID && x.IsActive == true).FirstOrDefault();
                        SalaryContent salaryContent = db.SalaryContents.Where(x => x.SalaryContentID == allowanceID).FirstOrDefault();

                        if (chkingExist != null)
                        {
                            tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                            bool isValue = Convert.ToBoolean(salaryContent.IsValue);

                            if (isValue == true)
                            {
                                totalAllowances -= Convert.ToDecimal(salaryContent.Size);
                            }
                            else
                            {
                                decimal actutalAllowance = (Convert.ToDecimal(salaryContent.Size) / 100) * basicPay;
                                totalAllowances -= actutalAllowance;
                            }
                            tblPlSalaryDetailObj.SalaryContentID = allowanceID;
                            tblPlSalaryDetailObj.IsActive = false;
                            tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                        }
                    }

                }
                foreach (CheckBox chkDd in EmployDeductionGridDetail.Controls.OfType<CheckBox>())
                {
                    int deductionID = Convert.ToInt32(chkDd.ID.Trim());
                    if (chkDd.Checked == true)
                    {

                        SalaryContent salaryContent = db.SalaryContents.Where(x => x.SalaryContentID == deductionID).FirstOrDefault();
                        if (salaryContent != null)
                        {
                            tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                            bool isValue = Convert.ToBoolean(salaryContent.IsValue);

                            if (isValue == true)
                            {
                                totaldeduction += Convert.ToDecimal(salaryContent.Size);
                                tblPlSalaryDetailObj.SalaryContentID = deductionID;

                            }
                            else
                            {
                                decimal actutalDeduction = (Convert.ToDecimal(salaryContent.Size) / 100) * basicPay;
                                totaldeduction += actutalDeduction;
                                tblPlSalaryDetailObj.SalaryContentID = deductionID;
                            }
                            tblPlSalaryDetailObj.IsActive = true;
                            tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                        }
                    }

                    else
                    {
                        var chkingExist = db.tblPlSalaryDetails.Where(x => x.CompID == _cmp && x.EmpID == empID && x.EffDate == effDate && x.SalaryContentID == deductionID && x.IsActive == true).FirstOrDefault();
                        SalaryContent salaryContent = db.SalaryContents.Where(x => x.SalaryContentID == deductionID).FirstOrDefault();

                        if (chkingExist != null)
                        {
                            tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                            bool isValue = Convert.ToBoolean(salaryContent.IsValue);

                            if (isValue == true)
                            {
                                totaldeduction -= Convert.ToDecimal(salaryContent.Size);
                            }
                            else
                            {
                                decimal actutaldeduction = (Convert.ToDecimal(salaryContent.Size) / 100) * basicPay;
                                totaldeduction -= actutaldeduction;
                            }
                            tblPlSalaryDetailObj.SalaryContentID = deductionID;
                            tblPlSalaryDetailObj.IsActive = false;
                            tblPlSalaryDetailList.Add(tblPlSalaryDetailObj);
                        }

                    }

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
                    checkIsExist.IsActive = true;
                    db.SubmitChanges();
                    string chkIsInsert = InsertSalaryPackageinDetails(checkIsExist.CompID, empID, effDate, tblPlSalaryDetailList);

                    if (chkIsInsert == "OK")
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                        ClearFields();
                        BindSalaryPackage();
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
                        BindSalaryPackage();
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
                allow.IsActive = true;
                db.tblPlAlows.InsertOnSubmit(allow);
                db.SubmitChanges();
                //int noOfRow = db.tblPlAlows.Count();
                string chkInsert = InsertSalaryPackageinDetails(allow.CompID, empID, effDate, tblPlSalaryDetailList);

                if (chkInsert == "OK")
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                    ClearFields();
                    BindSalaryPackage();
                }
                else
                {
                    ucMessage.ShowMessage(chkInsert, RMS.BL.Enums.MessageType.Error);
                }


                return "OK";



            }


            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        string InsertSalaryPackageinDetails(int compID, int empID, DateTime effDate, List<tblPlSalaryDetail> CheckedContentSalaryList)
        {
            try
            {
                using (RMSDataContext db = new RMSDataContext())
                {
                    for (int i = 0; i < CheckedContentSalaryList.Count; i++)
                    {
                        var scd = db.tblPlSalaryDetails.Where(x => x.CompID == compID && x.EmpID == empID && x.EffDate == effDate && x.SalaryContentID == CheckedContentSalaryList[i].SalaryContentID).SingleOrDefault();
                        if (scd != null)
                        {
                            scd.IsActive = CheckedContentSalaryList[i].IsActive;
                            db.SubmitChanges();
                        }
                        else
                        {
                            tblPlSalaryDetail tblPlSalaryDetailObj = new tblPlSalaryDetail();
                            tblPlSalaryDetailObj.CompID = compID;
                            tblPlSalaryDetailObj.EmpID = empID;
                            tblPlSalaryDetailObj.EffDate = effDate;
                            tblPlSalaryDetailObj.SalaryContentID = CheckedContentSalaryList[i].SalaryContentID;
                            tblPlSalaryDetailObj.IsActive = CheckedContentSalaryList[i].IsActive;

                            db.tblPlSalaryDetails.InsertOnSubmit(tblPlSalaryDetailObj);
                            db.SubmitChanges();
                        }




                    }
                }


                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            //return "OK";
        }


        private void ClearFields()
        {
            //ID = 0;

            txtEffDate.Text = "";
            txtBasicPay.Text = "";

            foreach (CheckBox chkAll in EmployAllowanceGridDetail.Controls.OfType<CheckBox>())
            {

                chkAll.Checked = false;

            }
            foreach (CheckBox chkDd in EmployDeductionGridDetail.Controls.OfType<CheckBox>())
            {
                chkDd.Checked = false;

            }

            ddlEmployee.SelectedValue = "0";
            ddlEmpType.SelectedValue = "0";

        }



        protected void grdSalaryPackage_PageIndexChanging(object sender, EventArgs e)
        {

        }

        protected void grdSalaryPackage_PageIndexChanged(object sender, EventArgs e)
        {

            ID = Convert.ToInt32(grdSalaryPackage.SelectedDataKey.Values["EmpID"].ToString());
            CompID = Convert.ToInt32(grdSalaryPackage.SelectedDataKey.Values["CompID"].ToString());
            EffDateStr = grdSalaryPackage.SelectedDataKey.Values["EffDate"].ToString();
            this.OnupdateEvent();
            //ClearFields();

        }
        //int Counter = 1;
        protected void grdSalaryPackage_RowBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // e.Row.Cells[0].Text = Counter.ToString();

                if (e.Row.Cells[3].Text.Equals("True"))
                {
                    e.Row.Cells[3].Text = "Yes";
                    e.Row.Cells[4].Style["visibility"] = "show";
                }
                else
                {
                    e.Row.Cells[4].Style["visibility"] = "hidden";
                    e.Row.Cells[3].Text = "No";
                }

            }


        }

        string OnupdateEvent()
        {
            DateTime dateTimeeff = Convert.ToDateTime(EffDateStr);
            RMSDataContext db = new RMSDataContext();
            try
            {
                tblPlAlow empPojo = allowBL.GetByID(CompID, ID.Value, DateTime.Parse(EffDateStr), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (empPojo != null)
                {
                    ddlEmployee.SelectedValue = empPojo.EmpID.ToString();
                    txtEffDate.Text = empPojo.EffDate.ToString();
                    txtBasicPay.Text = empPojo.Basic.ToString();
                    List<tblPlSalaryDetail> salaryDetailsList = db.tblPlSalaryDetails.Where(x => x.CompID == CompID && x.EmpID == ID && x.EffDate == dateTimeeff && x.IsActive == true).ToList();
                    for (int i = 0; i < salaryDetailsList.Count; i++)
                    {
                        int typeContentID = salaryDetailsList[i].SalaryContentID;
                        foreach (CheckBox chkAll in EmployAllowanceGridDetail.Controls.OfType<CheckBox>())
                        {
                            int chkAllID = Convert.ToInt32(chkAll.ID);
                            if (chkAllID == typeContentID)
                            {
                                chkAll.Checked = true;
                            }


                        }
                        foreach (CheckBox chkDd in EmployDeductionGridDetail.Controls.OfType<CheckBox>())
                        {
                            int chkDdID = Convert.ToInt32(chkDd.ID);
                            if (chkDdID == typeContentID)
                            {
                                chkDd.Checked = true;
                            }

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

        protected void BindSalaryPackage()
        {
            this.grdSalaryPackage.DataSource = allowBL.GetAllSalaryPackage(-1,"", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdSalaryPackage.DataBind();
        }
    }
}
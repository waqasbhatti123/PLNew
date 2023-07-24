using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.Data.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web;

namespace RMS.Profile
{
    public partial class EmpMgt : BasePage
    {

        #region DataMembers


        //RMS.BL.tblAppEmp usr;
        //GroupBL groupManager = new GroupBL();
        EmpBL empManager = new EmpBL();
        EmpTransferBL empTBL = new EmpTransferBL();
        EmpProfRptBL empProfRptBL = new EmpProfRptBL();
        ListItem selList = new ListItem();
        ListItem selListSub = new ListItem();

        RMSDataContext data = new RMSDataContext();


        #endregion

        #region Properties


#pragma warning disable CS0114 // 'EmpMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

        public int EmpID
        {
            get { return (ViewState["EmpID"] == null) ? 0 : Convert.ToInt32(ViewState["EmpID"]); }
            set { ViewState["EmpID"] = value; }
        }

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }


        public bool IsSearch
        {
            get { return (ViewState["IsSearch"] == null) ? false : Convert.ToBoolean(ViewState["IsSearch"]); }
            set { ViewState["IsSearch"] = value; }
        }


        #endregion

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {

                //FillDropdownDivision();
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Emp").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                if (Session["CompID"] == null)
                {
                    CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                }
                else
                {
                    CompID = Convert.ToByte(Session["CompID"].ToString());
                }

                if (Session["DateFullYearFormat"] == null)
                {
                    txtDOBCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    txtIssueDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    txtConfDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    txtExpDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    txtJoinDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    txtDOECal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    txtRelievingCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];


                }
                else
                {
                    txtDOBCal.Format = Session["DateFullYearFormat"].ToString();
                    txtIssueDateCal.Format = Session["DateFullYearFormat"].ToString();
                    txtConfDateCal.Format = Session["DateFullYearFormat"].ToString();
                    txtExpDateCal.Format = Session["DateFullYearFormat"].ToString();
                    txtJoinDateCal.Format = Session["DateFullYearFormat"].ToString();
                    txtDOECal.Format = Session["DateFullYearFormat"].ToString();
                    txtRelievingCal.Format = Session["DateFullYearFormat"].ToString();
                }


                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }



                BindGrid("", "",BranchID,IsSearch);
                
                FillDropDownCodeDept();
                FillDropDownScale();
                FillBranchDropDown();
                FillDllJobType();
                FillDllAppointedJobType();
                FillDropDownCodeDsgnApp();
                //FillddlJobType();
                //FillDistricDropDown(idddd);
                //FillDropDownCodeDivision();
                FillDropDownCodeDsgn();
                //FillDropDownCodeRegion();
                FillDropDownCodeSect();
                //FillDropDownCities();
                FillDropDownBanks();
                BindGridGrid();
                FillDropDownCitiesTransfer();
                FillDropDownCodeDeptTransfer();
                FillDropDownCodeDivisionTransfer();
                FillDropDownCodeDsgnTransfer();
                FillDropDownCodeRegionTransfer();
                FillDropDownAddtionalDes();
               // FillSearchBranchDropDown();
                FillDropdownBranchCasCade();
                AddtionalChargePlace();
                FillappoiDropDownScale();
                FillDropDownDCodeLastsgnApp();
                FillDropdownEmployeee();
                FillDropdownPersonal();
                //searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchDropDown.SelectedValue = BranchID.ToString();

                ucButtons.ValidationGroupName = "main";
                ucButtonEmpTransfers.ValidationGroupName = "main2";
                txtEmpCode.Focus();
            }
            //-------------------------------------
            if (rblMarStatus.SelectedValue == "S")
            {
                ddlDaughterCount.Enabled = false;
                ddlSonCount.Enabled = false;
            }
            else
            {
                ddlDaughterCount.Enabled = true;
                ddlSonCount.Enabled = true;
            }
            //-----------------------------------
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RMSDataContext db = new RMSDataContext();
            IsSearch = true;
            string empName = ddlEmpDrpdwn.SelectedValue;
            string Personal = ddlperson.SelectedValue;
            if (BranchID == 1)
            {
                if (empName != "0" && Personal != "0")
                {
                    grdEmps.DataSource = from emp in db.tblPlEmpDatas
                                         where emp.FullName.Contains(empName)
                    && emp.EmpCode.Contains(Personal)
                                         select new
                                         {
                                             emp.EmpID,
                                             emp_Id = "EN-" + emp.EmpID,
                                             emp.EmpCode,
                                             emp.FullName,
                                             Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                             emp.tblCity.CityName,
                                             Dept = emp.tblPlCode.CodeDesc,
                                             Desig = emp.tblPlCode1.CodeDesc,
                                             branch = emp.Branch1.br_nme,
                                             emp.tblPlLocation.LocName

                                         };
                }
                if (empName != "0" && Personal == "0")
                {
                    grdEmps.DataSource = from emp in db.tblPlEmpDatas
                                         where emp.FullName.Contains(empName)
                                         select new
                                         {
                                             emp.EmpID,
                                             emp_Id = "EN-" + emp.EmpID,
                                             emp.EmpCode,
                                             emp.FullName,
                                             Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                             emp.tblCity.CityName,
                                             Dept = emp.tblPlCode.CodeDesc,
                                             Desig = emp.tblPlCode1.CodeDesc,
                                             branch = emp.Branch1.br_nme,
                                             emp.tblPlLocation.LocName

                                         };
                }
                if (empName == "0" && Personal != "0")
                {
                    grdEmps.DataSource = from emp in db.tblPlEmpDatas
                                         where emp.EmpCode.Contains(Personal)
                                         select new
                                         {
                                             emp.EmpID,
                                             emp_Id = "EN-" + emp.EmpID,
                                             emp.EmpCode,
                                             emp.FullName,
                                             Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                             emp.tblCity.CityName,
                                             Dept = emp.tblPlCode.CodeDesc,
                                             Desig = emp.tblPlCode1.CodeDesc,
                                             branch = emp.Branch1.br_nme,
                                             emp.tblPlLocation.LocName

                                         };
                }

            }
            else
            {
                if (empName != "0" && Personal != "0")
                {
                    grdEmps.DataSource = from emp in db.tblPlEmpDatas
                                         where emp.FullName.Contains(empName)
                    && emp.EmpCode.Contains(Personal)
                                         select new
                                         {
                                             emp.EmpID,
                                             emp_Id = "EN-" + emp.EmpID,
                                             emp.EmpCode,
                                             emp.FullName,
                                             Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                             emp.tblCity.CityName,
                                             Dept = emp.tblPlCode.CodeDesc,
                                             Desig = emp.tblPlCode1.CodeDesc,
                                             branch = emp.Branch1.br_nme,
                                             emp.tblPlLocation.LocName

                                         };
                }
                if (empName != "0" && Personal == "0")
                {
                    grdEmps.DataSource = from emp in db.tblPlEmpDatas
                                         where emp.FullName.Contains(empName)
                                         select new
                                         {
                                             emp.EmpID,
                                             emp_Id = "EN-" + emp.EmpID,
                                             emp.EmpCode,
                                             emp.FullName,
                                             Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                             emp.tblCity.CityName,
                                             Dept = emp.tblPlCode.CodeDesc,
                                             Desig = emp.tblPlCode1.CodeDesc,
                                             branch = emp.Branch1.br_nme,
                                             emp.tblPlLocation.LocName

                                         };
                }
                if (empName == "0" && Personal != "0")
                {
                    grdEmps.DataSource = from emp in db.tblPlEmpDatas
                                         where emp.EmpCode.Contains(Personal)
                                         select new
                                         {
                                             emp.EmpID,
                                             emp_Id = "EN-" + emp.EmpID,
                                             emp.EmpCode,
                                             emp.FullName,
                                             Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                             emp.tblCity.CityName,
                                             Dept = emp.tblPlCode.CodeDesc,
                                             Desig = emp.tblPlCode1.CodeDesc,
                                             branch = emp.Branch1.br_nme,
                                             emp.tblPlLocation.LocName

                                         };
                }
            }

            grdEmps.DataBind();

        }


        protected void FillDllJobType()
        {
            RMSDataContext db = new RMSDataContext();
            ddlJobType.DataTextField = "JobTypeName1";
            ddlJobType.DataValueField = "JobNameID";
            ddlJobType.DataSource = db.JobTypeNames.ToList();
            ddlJobType.DataBind();
        }

        protected void FillDllAppointedJobType()
        {
            RMSDataContext db = new RMSDataContext();
            appJobType.DataTextField = "JobTypeName1";
            appJobType.DataValueField = "JobNameID";
            appJobType.DataSource = db.JobTypeNames.ToList();
            appJobType.DataBind();
        }


        //protected void FillddlJobType()
        //{
        //    RMSDataContext dataContext = new RMSDataContext();
        //    try
        //    {
        //        ddlJobType.DataTextField = "JobTypeName1";
        //        ddlJobType.DataValueField = "JobNameID";

        //            ddlJobType.DataSource = dataContext.JobTypeNames.Where(x => x.IsActive == true).ToList();
        //            ddlJobType.DataBind();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (ddlCity.SelectedIndex != 0)
            //{
            //    FillDropDownLocations();
            //}
            //else
            //{
            //    ddlLoc.Items.Clear();
            //    ddlLoc.Dispose();
            //    selList.Text = "Select Location";
            //    selList.Value = "0";
            //    ddlLoc.Items.Insert(0, selList);
            //}
        }

        //protected void BranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    RMSDataContext Data = new RMSDataContext();
        //    BranchBL branchBL = new BranchBL();
        //    int br = Convert.ToInt32(BranchDropDown.SelectedValue);

        //    try
        //    {
               
        //            ddlDictric.Items.Clear();
        //            ddlDictric.Dispose();
        //            selList.Text = "Select District";
        //            selList.Value = "0";
        //            ddlDictric.Items.Insert(0, selList);
        //            ddlDictric.DataValueField = "br_id";
        //            ddlDictric.DataTextField = "br_nme";
        //            ddlDictric.DataSource = branchBL.GetDistric(br, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            ddlDictric.DataBind();
               
                
        //    }
        //    catch (Exception)
        //    {
                
        //    }
        //}

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlBank.SelectedValue.Equals("0"))
                {
                    txtBankBranch.Text = (new BankBL().GetByID(ddlBank.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])).BankName;
                }
                else
                {
                    txtBankBranch.Text = "";
                }
            }
            catch
            { }
        }

        protected void ddlCityTransfer_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlCityTransfer.SelectedIndex != 0)
            {
                FillDropDownLocationsTransfer();
            }
            else
            {
                ddlLocTransfer.Items.Clear();
                ddlLocTransfer.Dispose();
                selList.Text = "Select Location";
                selList.Value = "0";
                ddlLocTransfer.Items.Insert(0, selList);
            }

        }

        protected void grdEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ClearFields();
            ID = Convert.ToInt32(grdEmps.SelectedDataKey.Value);
            GetByID();

        }

        protected void grdEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEmps.PageIndex = e.NewPageIndex;
            BindGridGrid();
            
            //BindGrid(ddlEmpDrpdwn.SelectedValue, ddlperson.SelectedValue , BranchID, IsSearch);
        }

        protected void grdEmpTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    e.Row.Cells[0].Text = DateTime.Parse(e.Row.Cells[0].Text).ToString(Request.Cookies["uzr"]["DateFullYearFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[0].Text = DateTime.Parse(e.Row.Cells[0].Text).ToString(Session["DateFullYearFormat"].ToString());
                }
            }
        }

        protected void grdEmpTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEmpTransfer.PageIndex = e.NewPageIndex;
            BindGridTransfers();
        }

        protected void btnUploadStart_Click(object sender, EventArgs e)
        {
            try
            {
                string fileNme = GetUploadDocName();
                if (!fileNme.Equals(""))
                {
                    if (fileNme.Equals("ImgSizeExceeded"))
                    {
                        return;
                    }
                    string ext = fileNme.Substring(fileNme.LastIndexOf("."), fileNme.Length - fileNme.LastIndexOf("."));
                    if (ext.ToLower().Equals(".jpeg") || ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".gif"))
                    {

                        tblPlEmpData emp = empManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        fileNme = emp.EmpCode + ".jpg";

                        if (!emp.EmpPic.Equals("noimage.jpg"))
                        {
                            DeleteOldPic(emp.EmpPic);
                        }
                        emp.EmpPic = fileNme;
                        empManager.Update(emp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        UploadImg(fileNme);

                        //imgEmp.ImageUrl = "~/empix/" + fileNme;

                        //btnUploadStart.Visible = false;
                        //fileUploadImg.Visible = false;
                    }
                    else
                    {
                        ucMessage.ShowMessage("Image file with .jpg/.jpeg or .gif extension required", RMS.BL.Enums.MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetUploadDocName()
        {
            string file = "";

            if (empImageFileUploader.HasFile)
            {

                string filepath = empImageFileUploader.PostedFile.FileName;

                int fileSize = empImageFileUploader.PostedFile.ContentLength;

                if (fileSize > 5 * 1024 * 1024)
                {
                    ucMessage.ShowMessage("Image size should be less than 5MB", RMS.BL.Enums.MessageType.Error);
                    return "ImgSizeExceeded";
                }
                try
                {
                    string pat = @"\\(?:.+)\\(.+)\.(.+)";
                    Regex r = new Regex(pat);
                    //run
                    Match m = r.Match(filepath);
                    string file_ext = m.Groups[2].Captures[0].ToString();
                    //string filename = m.Groups[1].Captures[0].ToString();
                    file = "." + file_ext;

                }
                catch
                {
                    file = filepath;
                }
            }

            return file;
        }

        private void UploadImg(string file)
        {
            try
            {
                empImageFileUploader.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeleteOldPic(string fileName)
        {

            try
            {
                //string fileName = docBL.DeleteByID(ID, (KSBSalesDataContext)Session[Session["UserID"] + "ksbDBObj"]);
                System.IO.File.Delete(Server.MapPath("~/empix/" + fileName));

            }
            catch
            {

            }
        }

        private void ClearFieldsTransfer()
        {
            this.ddlDesigTransfer.SelectedValue = "0";
            this.ddlDeptTransfer.SelectedValue = "0";
            ddlLocTransfer.Items.Clear();
            ddlLocTransfer.Dispose();
            selList.Text = "Select Location";
            selList.Value = "0";
            ddlLocTransfer.Items.Insert(0, selList);

            ddlDivTransfer.SelectedIndex = 0;
            ddlRegionTransfer.SelectedIndex = 0;
            ddlCityTransfer.SelectedIndex = 0;
            ddlEmpDrpdwn.SelectedValue = "0";
            ddlperson.SelectedValue = "0";

            txtDOE.Text = "";


            txtGradeTransfer.Text = "";
            //divEmpTransfer.Visible = false;
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {

                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                //ddlRegion.Focus();
            }
            else if (e.CommandName == "Save")
            {

                DateTime dob = Convert.ToDateTime("01-01-1900");
                DateTime issDate = Convert.ToDateTime("01-01-1900");
                DateTime expDate = Convert.ToDateTime("01-01-1900");
                DateTime joinDate = Convert.ToDateTime("01-01-1900");
                DateTime cnfrmDate = Convert.ToDateTime("01-01-1900");

                int counter = 0;
                try
                {
                    if (!txtDOB.Text.Trim().Equals(""))
                    {
                        dob = Convert.ToDateTime(txtDOB.Text.Trim());
                    }
                    ++counter;
                    if (!txtIssueDate.Text.Trim().Equals(""))
                    {
                        issDate = Convert.ToDateTime(txtIssueDate.Text.Trim());
                    }
                    ++counter;
                    if (!txtExpDate.Text.Trim().Equals(""))
                    {
                        expDate = Convert.ToDateTime(txtExpDate.Text.Trim());
                    }
                    ++counter;
                    if (!txtJoinDate.Text.Trim().Equals(""))
                    {
                        joinDate = Convert.ToDateTime(txtJoinDate.Text.Trim());
                    }
                    ++counter;
                    if (!txtConfDate.Text.Trim().Equals(""))
                    {
                        cnfrmDate = Convert.ToDateTime(txtConfDate.Text.Trim());
                    }

                    if (!issDate.Equals(Convert.ToDateTime("01-01-1900")))
                    {
                        if (issDate.Date > Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date)
                        {
                            ucMessage.ShowMessage("CNIC Issue date cannot be greater than the current date.", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }


                    if (!joinDate.Equals(Convert.ToDateTime("01-01-1900")))
                    {
                        if (joinDate.Date > Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date)
                        {
                            ucMessage.ShowMessage("Joining date cannot be greater than the current date.", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }


                    if (!dob.Equals(Convert.ToDateTime("01-01-1900")) && !issDate.Equals(Convert.ToDateTime("01-01-1900")))
                    {
                        if (dob.AddYears(18).Date >= issDate.Date)
                        {
                            ucMessage.ShowMessage("Date of birth should be less than " + issDate.Date.ToString("dd-MMM-yyyy") + ".", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }


                    if (!expDate.Equals(Convert.ToDateTime("01-01-1900")) && !issDate.Equals(Convert.ToDateTime("01-01-1900")))
                    {
                        if (issDate.Date >= expDate.Date)
                        {
                            ucMessage.ShowMessage("CNIC expiry date should be greater than " + issDate.Date.ToString("dd-MMM-yyyy") + ".", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }

                    if (!joinDate.Equals(Convert.ToDateTime("01-01-1900")) && !dob.Equals(Convert.ToDateTime("01-01-1900")))
                    {
                        if (joinDate < dob.AddYears(18))
                        {
                            ucMessage.ShowMessage("Joining date should be greater than " + dob.AddYears(18).Date.ToString("dd-MMM-yyyy") + ".", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }

                    if (!joinDate.Equals(Convert.ToDateTime("01-01-1900")) && !cnfrmDate.Equals(Convert.ToDateTime("01-01-1900")))
                    {
                        if (cnfrmDate.Date < joinDate.Date)
                        {
                            ucMessage.ShowMessage("Confirm Date should be greter than " + joinDate.Date.ToString("dd-MMM-yyyy") + ".", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }




                }
                catch
                {
                    if (counter == 0)
                    {
                        ucMessage.ShowMessage("Invalid date of birth", RMS.BL.Enums.MessageType.Error);
                    }
                    else if (counter == 1)
                    {
                        ucMessage.ShowMessage("Invalid NIC issue date", RMS.BL.Enums.MessageType.Error);
                    }
                    else if (counter == 2)
                    {
                        ucMessage.ShowMessage("Invalid NIC expiry date", RMS.BL.Enums.MessageType.Error);
                    }
                    else if (counter == 3)
                    {
                        ucMessage.ShowMessage("Invalid joining date", RMS.BL.Enums.MessageType.Error);
                    }
                    else if (counter == 4)
                    {
                        ucMessage.ShowMessage("Invalid confirmation date", RMS.BL.Enums.MessageType.Error);
                    }
                    return;
                }

                if (ID == 0)
                {
                    bool flag = empManager.FindDuplicate(txtEmpCode.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (flag == false)
                    {
                        this.Insert();
                        //pnlMain.Enabled = false;
                        ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    }
                    else
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "CantDuplicateEmpCode").ToString(), RMS.BL.Enums.MessageType.Error);
                        return;
                    }

                }
                else
                {
                    this.Update();
                    //pnlMain.Enabled = false;
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
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
               BindGrid("", "",BranchID,IsSearch);
                ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                //ddlRegion.Focus();
            }
            else if (e.CommandName == "Cancel")
            {
                //pnlMain.Enabled = false;
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                ClearFields();

            }
            else if (e.CommandName == "ShowUploadImg")
            {
                if (ID == 0)
                {
                    //fileUploadImg.Visible = true;
                    //btnUploadStart.Visible = false;
                }
                else
                {
                    //fileUploadImg.Visible = true;
                    //btnUploadStart.Visible = true;
                }
            }
        }

        protected void ButtonCommandTransfer(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {

                if (ID > 0)
                {
                    InsertTransfer();
                    ClearFieldsTransfer();
                }
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFieldsTransfer();
            }
        }

        protected void btnViewTransfer_Click(object sender, EventArgs e)
        {
            divEmpTransfer.Visible = true;
            BindGridTransfers();
            ClearFieldsTransfer();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrint = (LinkButton)sender;
            GridViewRow lnkPrintRow = (GridViewRow)lnkPrint.NamingContainer;
            int rowIndex = lnkPrintRow.RowIndex;

            EmpID = Convert.ToInt32(grdEmps.DataKeys[rowIndex].Values[0]);

            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
           string.Format("window.open('EmpMgtReport.aspx?ID={0}');", EmpID), true);

            //Response.Redirect("EmpMgtReport.aspx?ID=" + EmpID);


           // PrintEmpProfile(EmpID);
        }


        #endregion

        #region Helping Method


        protected void BindGrid(string empName, string empNo, int branchID, bool isSearch)
        {
            this.grdEmps.DataSource = empManager.GetAll(empName, empNo, branchID, isSearch, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            this.grdEmps.DataBind();
        }
        protected void BindGridGrid()
        {
            RMSDataContext Data = new RMSDataContext();
            
            this.grdEmps.DataSource = from emp in Data.tblPlEmpDatas
                                      where emp.BranchID == BranchID
                                      orderby emp.ScaleID descending
                                      select new
                                      {
                                          emp.EmpID,
                                          emp_Id = "EN-" + emp.EmpID,
                                          emp.EmpCode,
                                          emp.FullName,
                                          Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                          emp.tblCity.CityName,
                                          Dept = emp.tblPlCode.CodeDesc,
                                          Desig = emp.tblPlCode1.CodeDesc,
                                          branch = emp.Branch1.br_nme,
                                          emp.tblPlLocation.LocName

                                      };
            this.grdEmps.DataBind();
        }

        protected void BindGridTransfers()
        {
            this.grdEmpTransfer.DataSource = empTBL.GetAll(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdEmpTransfer.DataBind();
        }

        protected void InsertTransfer()
        {
            //RMS.BL.Employee emp = new RMS.BL.Employee();
            tblPlEmpData emp = new tblPlEmpData();
            emp = empManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (!ddlDesigTransfer.SelectedValue.Equals("0"))
            {
                emp.tblPlCode1 = new PlCodeBL().GetByID(Convert.ToInt16(ddlDesigTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                emp.DesigID = Convert.ToInt16(ddlDesigTransfer.SelectedValue);
            }
            else
            {
                emp.tblPlCode1 = null;
            }

            if (!ddlRegionTransfer.SelectedValue.Equals("0"))
            {
                emp.tblPlCode3 = new PlCodeBL().GetByID(Convert.ToInt16(ddlRegionTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                emp.RegID = Convert.ToInt16(ddlRegionTransfer.SelectedValue);
            }
            else
            {
                emp.tblPlCode3 = null;
            }

            if (!ddlDivTransfer.SelectedValue.Equals("0"))
            {
                emp.tblPlCode2 = new PlCodeBL().GetByID(Convert.ToInt16(ddlDivTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                emp.DivID = Convert.ToInt16(ddlDivTransfer.SelectedValue);
            }
            else
            {
                emp.tblPlCode2 = null;
            }

            if (!ddlDeptTransfer.SelectedValue.Equals("0"))
            {
                emp.tblPlCode = new PlCodeBL().GetByID(Convert.ToInt16(ddlDeptTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                emp.DeptID = Convert.ToInt16(ddlDeptTransfer.SelectedValue);
            }
            else
            {
                emp.tblPlCode = null;
            }

            if (!ddlCityTransfer.SelectedValue.Equals("0"))
            {
                emp.tblCity = new CityBL().GetByID(Convert.ToInt32(ddlCityTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                emp.CityID = Convert.ToInt32(ddlCityTransfer.SelectedValue);

                if (!ddlLocTransfer.SelectedValue.Equals("0"))
                {
                    emp.tblPlLocation = new LocationBL().GetByID(Convert.ToByte(ddlLocTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    emp.LocID = Convert.ToByte(ddlLocTransfer.SelectedValue);
                }
                else
                {
                    emp.tblPlLocation = null;
                }
            }
            else
            {
                emp.tblCity = null;
                emp.tblPlLocation = null;
            }

            emp.Grade = txtGradeTransfer.Text.Trim();

            tblPlEmpTransfer empT = new tblPlEmpTransfer();
            empT.EmpID = emp.EmpID;



            empT.EfDate = Convert.ToDateTime(txtDOE.Text.Trim());

            if (!ddlDesigTransfer.SelectedValue.Equals("0"))
            {
                empT.tblPlCode1 = new PlCodeBL().GetByID(Convert.ToInt16(ddlDesigTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                empT.DesigID = Convert.ToInt16(ddlDesigTransfer.SelectedValue);
            }

            if (!ddlRegionTransfer.SelectedValue.Equals("0"))
            {
                empT.tblPlCode3 = new PlCodeBL().GetByID(Convert.ToInt16(ddlRegionTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                empT.RegID = Convert.ToInt16(ddlRegionTransfer.SelectedValue);
            }


            if (!ddlDivTransfer.SelectedValue.Equals("0"))
            {
                empT.tblPlCode2 = new PlCodeBL().GetByID(Convert.ToInt16(ddlDivTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                empT.DivID = Convert.ToInt16(ddlDivTransfer.SelectedValue);
            }

            if (!ddlDeptTransfer.SelectedValue.Equals("0"))
            {
                empT.tblPlCode = new PlCodeBL().GetByID(Convert.ToInt16(ddlDeptTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                empT.DeptID = Convert.ToInt16(ddlDeptTransfer.SelectedValue);
            }

            if (!ddlCityTransfer.SelectedValue.Equals("0"))
            {
                empT.tblCity = new CityBL().GetByID(Convert.ToInt32(ddlCityTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                empT.CityID = Convert.ToInt32(ddlCityTransfer.SelectedValue);

                if (!ddlLocTransfer.SelectedValue.Equals("0"))
                {
                    empT.tblPlLocation = new LocationBL().GetByID(Convert.ToByte(ddlLocTransfer.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    empT.LocID = Convert.ToByte(ddlLocTransfer.SelectedValue);
                }
            }

            empT.Grade = txtGradeTransfer.Text.Trim();

            empT.tblPlEmpData = emp;
            empT.tblCompany = emp.tblCompany;

            empTBL.Insert(empT, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
            BindGridTransfers();
            //ClearFields();

        }

        protected void GetByID()
        {
            tblPlEmpData empPojo = empManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //btnView_Transfer.Visible = true;
            this.txtEmpId.Text = "EN-" + empPojo.EmpID;
            this.txtEmpCode.Text = empPojo.EmpCode.ToString();
            this.txtFullName.Text = empPojo.FullName;
            ddlReligion.SelectedValue = empPojo.Religion.ToString();
            // this.txtMidName.Text = empPojo.MidName;
            // this.txtSirName.Text = empPojo.SirName;

            //if (empPojo.sortRef != null || empPojo.sortRef != 0)
            //{
            //    txtSortReference.Text = empPojo.sortRef.ToString();
            //}
            //else
            //{
            //    txtSortReference.Text = "";
            //}

            if (empPojo.AddtionalPost == null || empPojo.AddtionalPost == "" || empPojo.AddtionalPost == "0" || empPojo.AddtionalPost == "N/A")
            {
                txtadditionalPlace.Text = "";
            }
            else
            {
                txtadditionalPlace.Text = empPojo.AddtionalPlace.ToString();
            }
            if (empPojo.AddtionalPost == null || empPojo.AddtionalPost == "" || empPojo.AddtionalPost == "0")
            {
                txtaddtionalChargePost.Text = "";
            }
            else
            {
                txtaddtionalChargePost.Text = empPojo.AddtionalPost.ToString();
            }
            

            if (empPojo.JobNameID != null && empPojo.JobNameID != 0)
            {
                this.ddlJobType.SelectedValue = empPojo.JobNameID.ToString();
            }
            if (empPojo.AppointJobType != null && empPojo.AppointJobType != 0)
            {
                this.appJobType.SelectedValue = empPojo.AppointJobType.ToString();
            }
            this.ddlDomicile.SelectedValue = empPojo.Domicil;
            this.rblGender.SelectedValue = empPojo.Sex.ToString();
            this.rblMarStatus.SelectedValue = empPojo.MarStatus.ToString();

            if (empPojo.addchargePalce == null)
            {
                ddlAddPlace.SelectedValue = "0";
            }
            else
            {
                ddlAddPlace.SelectedValue = empPojo.addchargePalce.ToString();
            }

            if (empPojo.apposcal == null)
            {
                ddlappointscale.SelectedValue = "0";
            }
            else
            {
                ddlappointscale.SelectedValue = empPojo.apposcal.ToString();
            }
            if (empPojo.LastperDes == null)
            {
                ddlLastPero.SelectedValue = "0";
            }
            else
            {
                ddlLastPero.SelectedValue = empPojo.LastperDes.ToString();
            }
            //if (string.IsNullOrEmpty(empPojo.DepNo))
            //{
            //    txtDepu.Enabled = false;
            //    txtDepu.Text = "";
            //}
            //else
            //{
            //    txtDepu.Text = empPojo.DepNo.ToString();
            //}

            if (empPojo.Appointed == null)
            {
                ddlappointed.SelectedValue = "0";
            }
            else
            {
                ddlappointed.SelectedValue = empPojo.Appointed.ToString();
            }

            ddlRelieving.SelectedValue = empPojo.EmpStatus.ToString();
            if (empPojo.RelievingDate == null)
            {
                txtRelieving.Text = "";
            }
            else
            {
                txtRelieving.Text = empPojo.RelievingDate.Value.ToString(Session["DateFullYearFormat"].ToString());
            }
            
            ddlEmpAddtional.SelectedValue = empPojo.AddtionalCharg;
            ddlDisablity.SelectedValue = empPojo.Disbality;

            rdDeput.SelectedValue = empPojo.DepEnb.ToString();
            rdpoliceveri.SelectedValue = empPojo.polveri.ToString();
            rdmediver.SelectedValue = empPojo.Mediveri.ToString();
            
            rddegveri.SelectedValue = empPojo.Degveri.ToString();

            if (empPojo.apposcal == null)
            {
                ddlappointscale.SelectedValue = "0";
            }
            else
            {
                ddlappointscale.SelectedValue = empPojo.apposcal.ToString();
            }
            if (string.IsNullOrEmpty(empPojo.HelInNo))
            {
                txtHealthInsurance.Enabled = false;
                txtHealthInsurance.Text = "";
            }
            else
            {
                txtHealthInsurance.Text = empPojo.HelInNo.ToString();
            }
            
            //if (empPojo.tblPlCode3 != null)
            //{
            //    this.ddlRegion.SelectedValue = empPojo.RegID.ToString();
            //}
            //else
            //{
            //    ddlRegion.SelectedIndex = 0;
            //}
            //if (empPojo.tblPlCode2 != null)
            //{
            //    this.ddlDivision.SelectedValue = empPojo.DivID.ToString();
            //}
            //else
            //{
            //    ddlDivision.SelectedIndex = 0;
            //}

            if (empPojo.tblPlCode != null)
            {
                this.ddlDept.SelectedValue = empPojo.DeptID.ToString();
            }
            else
            {
                ddlDept.SelectedIndex = 0;
            }

            if (empPojo.ScaleID != null)
            {
                this.ScaleDropDown.SelectedValue = empPojo.ScaleID.ToString();
            }
            else
            {
                ScaleDropDown.SelectedIndex = 0;
            }

            ddlQuota.SelectedValue = empPojo.Quota;

            if (empPojo.BranchID != null)
            {
                this.BranchDropDown.SelectedValue = empPojo.BranchID.ToString();
            }
            else
            {
                BranchDropDown.SelectedIndex = 0;
            }

            if (empPojo.tblPlCode1 != null)
            {
                this.ddlDesignation.SelectedValue = empPojo.DesigID.ToString();
            }
            else
            {
                ddlDesignation.SelectedIndex = 0;
            }

            if (empPojo.tblPlCode4 != null)
            {
                this.ddlSection.SelectedValue = empPojo.SectID.ToString();
            }
            else
            {
                ddlSection.SelectedIndex = 0;
            }
            if (empPojo.tblCity != null)
            {
               // this.ddlCity.SelectedValue = empPojo.CityID.ToString();
                FillDropDownLocations();
                if (empPojo.tblPlLocation != null)
                {
                    this.ddlLoc.SelectedValue = empPojo.LocID.ToString();
                }
            }
            else
            {
               // ddlCity.SelectedIndex = 0;
                FillDropDownLocations();
            }

            this.txtFatherName.Text = empPojo.FatherName;
            this.txtNic.Text = empPojo.NIC;
            this.txtNtn.Text = empPojo.NTN;
            this.txtPhoneNo.Text = empPojo.TelNo;
            this.txtMobNo.Text = empPojo.MobNo;
            this.txtAdd.Text = empPojo.EmpAdd1;
            this.txtAdd2Perm.Text = empPojo.EmpAdd2;


            if (empPojo.Bank != null)
            {
                try {
                    this.ddlBank.SelectedValue = empPojo.Bank;
                    txtBankBranch.Text = (new BankBL().GetByID(empPojo.Bank, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])).BankName;

                }
                catch { }
            }
            this.txtBankAcct.Text = empPojo.AccountNo;


            this.txtEobi.Text = empPojo.EobiNo;
            this.rdDeput.SelectedValue = empPojo.DepEnb.ToString().ToLower();
            this.rblEobiEnbl.SelectedValue = empPojo.EobiEnb.ToString().ToLower();
            this.rblScsiEnbl.SelectedValue = empPojo.ScsiEnb.ToString().ToLower();

            if (empPojo.EobiEnb.ToString().ToLower().Equals("false"))
            {
                this.txtEobi.Enabled = false;
            }
            this.txtScsi.Text = empPojo.ScsiNo;
            if (empPojo.ScsiEnb.ToString().ToLower().Equals("false"))
            {
                this.txtScsi.Enabled = false;
            }

            if (!string.IsNullOrEmpty(empPojo.CNICAttach))
            {
                imageID.ImageUrl = "~/empix/" + empPojo.CNICAttach;
            }
            else
            {
                imageID.ImageUrl = "";
            }
            if (!string.IsNullOrEmpty(empPojo.AppAttach))
            {
                appoImage.ImageUrl = "~/empix/" + empPojo.AppAttach;
            }
            else
            {
                appoImage.ImageUrl = "";
            }
            if (!string.IsNullOrEmpty(empPojo.OrderAttach))
            {
                orderImage.ImageUrl = "~/empix/" + empPojo.OrderAttach;
            }
            else
            {
                orderImage.ImageUrl = "";
            }

            if (!string.IsNullOrEmpty(empPojo.ReguAttachement))
            {
                regID.ImageUrl = "~/empix/" + empPojo.ReguAttachement;
            }
            else
            {
                regID.ImageUrl = "";
            }




            this.rdoHealthIns.SelectedValue = empPojo.HlthInsEnb.ToString().ToLower();

            if (empPojo.DOB != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtDOB.Text = empPojo.DOB.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.txtDOB.Text = empPojo.DOB.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                txtDOB.Text = "";
            }
            if (empPojo.DOJ != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtJoinDate.Text = empPojo.DOJ.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.txtJoinDate.Text = empPojo.DOJ.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                txtJoinDate.Text = "";
            }

            if (empPojo.DOC != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtConfDate.Text = empPojo.DOC.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.txtConfDate.Text = empPojo.DOC.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                txtConfDate.Text = "";
            }
            if (empPojo.NICIssueDate != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtIssueDate.Text = empPojo.NICIssueDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.txtIssueDate.Text = empPojo.NICIssueDate.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                txtIssueDate.Text = "";
            }
            if (empPojo.NICExpiryDate != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtExpDate.Text = empPojo.NICExpiryDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.txtExpDate.Text = empPojo.NICExpiryDate.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                txtExpDate.Text = "";
            }

            this.txtEmail.Text = empPojo.Email;
            this.txtEdu.Text = empPojo.Education;
            //this.txtGrade.Text = empPojo.Grade;
            this.ddlSonCount.SelectedValue = empPojo.SonCount;
            this.ddlDaughterCount.SelectedValue = empPojo.DauughterCount;

            if(empPojo.EmpPic != null && empPojo.EmpPic != "")
            {
               empImage.ImageUrl = "~/empix/" + empPojo.EmpPic;
            }
            else
            {
                empImage.ImageUrl = "";
            }


            //ClearFieldsTransfer();
            divEmpTransfer.Visible = false;
            
            //imgEmp.ImageUrl = "~/empix/" + empPojo.EmpPic;
            //btnAddImg.Visible = false;
            //btnUpdImg.Visible = true;
            //            btnViewTransfer.Visible = true;

            pnlMain.Enabled = true;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Insert()
        {
            //RMS.BL.Employee emp = new RMS.BL.Employee();
            tblPlEmpData emp = new tblPlEmpData();

            if (Session["CompID"] == null)
            {
                emp.CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                emp.CompID = Convert.ToByte(Session["CompID"].ToString());
            }
            
            emp.EmpCode = txtEmpCode.Text.Trim();
            emp.FullName = txtFullName.Text.Trim();
            //emp.sortRef = Convert.ToInt32(txtSortReference.Text);
            emp.Sex = Convert.ToChar(rblGender.SelectedValue);
            emp.MarStatus = Convert.ToChar(rblMarStatus.SelectedValue);
            emp.CreatedBy = Session["LoginID"].ToString();
            emp.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (!ddlDesignation.SelectedValue.Equals("0"))
            {
                emp.tblPlCode1 = new PlCodeBL().GetByID(Convert.ToInt16(ddlDesignation.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                emp.DesigID = Convert.ToInt16(ddlDesignation.SelectedValue);
            }
            if (!ddlSection.SelectedValue.Equals("0"))
            {
                emp.tblPlCode4 = new PlCodeBL().GetByID(Convert.ToInt16(ddlSection.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                emp.SectID = Convert.ToInt16(ddlSection.SelectedValue);
            }

            //if (!ddlRegion.SelectedValue.Equals("0"))
            //{
            //    emp.tblPlCode3 = new PlCodeBL().GetByID(Convert.ToInt16(ddlRegion.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //    emp.RegID = Convert.ToInt16(ddlRegion.SelectedValue);
            //}


            //if (!ddlDivision.SelectedValue.Equals("0"))
            //{
            //    emp.tblPlCode2 = new PlCodeBL().GetByID(Convert.ToInt16(ddlDivision.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //    emp.DivID = Convert.ToInt16(ddlDivision.SelectedValue);
            //}

            if (!ddlDept.SelectedValue.Equals("0"))
            {
                emp.tblPlCode = new PlCodeBL().GetByID(Convert.ToInt16(ddlDept.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                emp.DeptID = Convert.ToInt16(ddlDept.SelectedValue);
            }
            emp.Religion = Convert.ToChar(ddlReligion.SelectedValue);
            if (!ScaleDropDown.SelectedValue.Equals("0"))
            {
                emp.ScaleID = Convert.ToInt16(ScaleDropDown.SelectedValue);
            }

            if (!BranchDropDown.SelectedValue.Equals("0"))
            {
                emp.BranchID = Convert.ToInt16(BranchDropDown.SelectedValue);
            }
            if (txtadditionalPlace.Text == "" || txtadditionalPlace.Text == null)
            {
                emp.AddtionalPlace = null;
            }
            else
            {
                emp.AddtionalPlace = txtadditionalPlace.Text;
            }
            if (txtaddtionalChargePost.Text == "" || txtaddtionalChargePost.Text == null)
            {
                emp.AddtionalPost = null;
            }
            else
            {
                emp.AddtionalPost = txtaddtionalChargePost.Text;
            }
            //if (ddlDictric.SelectedValue == "0" && ddlDictric.SelectedValue == "")
            //{
            //    emp.DistricID = null;
            //}
            //else
            //{
            //    emp.DistricID = Convert.ToInt32(ddlDictric.SelectedValue.Trim());
            //}



            //if (!ddlCity.SelectedValue.Equals("0"))
            //{
            //    emp.tblCity = new CityBL().GetByID(Convert.ToInt32(ddlCity.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //    emp.CityID = Convert.ToInt32(ddlCity.SelectedValue);

            //    if (!ddlLoc.SelectedValue.Equals("0"))
            //    {
            //        emp.tblPlLocation = new LocationBL().GetByID(Convert.ToByte(ddlLoc.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //        emp.LocID = Convert.ToByte(ddlLoc.SelectedValue);
            //    }
            //}
            emp.Domicil = ddlDomicile.SelectedValue;
            emp.FatherName = txtFatherName.Text.Trim();
            emp.MotherName = txtMotherName.Text.Trim();

            emp.NIC = txtNic.Text.Trim();
            emp.NTN = txtNtn.Text.Trim();
            emp.TelNo = txtPhoneNo.Text.Trim();
            emp.MobNo = txtMobNo.Text.Trim();
            emp.EmpAdd1 = txtAdd.Text.Trim();
            emp.Quota = ddlQuota.SelectedValue;
            emp.Disbality = ddlDisablity.SelectedValue;
            emp.AddtionalCharg = ddlEmpAddtional.SelectedValue;
            emp.addchargePalce = Convert.ToInt32(ddlAddPlace.SelectedValue);
            emp.apposcal = Convert.ToInt32(ddlappointscale.SelectedValue);
            emp.LastperDes = Convert.ToInt32(ddlLastPero.SelectedValue);

            emp.EmpAdd2 = txtAdd2Perm.Text.Trim();

            if (ddlBank.SelectedIndex > 0)
            {
                emp.Bank = ddlBank.SelectedValue;
            }
            emp.Branch = txtBankBranch.Text.Trim();
            emp.AccountNo = txtBankAcct.Text.Trim();


            emp.EobiNo = txtEobi.Text.Trim();
            emp.ScsiNo = txtScsi.Text.Trim();

            Boolean eobi = false;
            Boolean.TryParse(rblEobiEnbl.SelectedValue, out eobi);
            emp.EobiEnb = eobi;

            Boolean scsienable = false;
            Boolean.TryParse(rblScsiEnbl.SelectedValue, out scsienable);
            emp.ScsiEnb = scsienable;

            Boolean hltenable = false;
            Boolean.TryParse(this.rdoHealthIns.SelectedValue, out hltenable);
            emp.HlthInsEnb = hltenable;

            if (rdoHealthIns.SelectedValue.Equals("true"))
            {
                emp.HelInNo = txtHealthInsurance.Text.Trim();
            }
            else
            {
                emp.HelInNo = null;
            }

            Boolean police = false;
            Boolean.TryParse(rdpoliceveri.SelectedValue, out police);
            emp.polveri = police;
            Boolean Medi = false;
            Boolean.TryParse(rdmediver.SelectedValue, out Medi);
            emp.Mediveri = Medi;
            Boolean redd = false;
            Boolean.TryParse(rddegveri.SelectedValue, out redd);
            emp.Degveri = redd;
            Boolean Dep = false;
            Boolean.TryParse(rdDeput.SelectedValue, out Dep);
            emp.DepEnb = Dep;
            //if (rdDeput.SelectedValue.Equals("true"))
            //{
            //    emp.DepNo = txtDepu.Text.Trim();
            //}
            //else
            //{
            //    emp.DepNo = null;
            //}

            if (!txtDOB.Text.Trim().Equals(""))
            {
                try
                {
                    emp.DOB = Convert.ToDateTime(txtDOB.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            
            else
            {
                emp.DOB = null;
            }
            if (ddlappointed.SelectedValue == "0")
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "Select Designation").ToString(), RMS.BL.Enums.MessageType.Info);
                return;
            }
            else
            {
                emp.Appointed = Convert.ToInt32(ddlappointed.SelectedValue);
            }
            if (!txtJoinDate.Text.Trim().Equals(""))
            {
                try
                {
                    emp.DOJ = Convert.ToDateTime(txtJoinDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            else
            {
                emp.DOJ = null;
            }

            if (!txtConfDate.Text.Trim().Equals(""))
            {
                try
                {
                    emp.DOC = Convert.ToDateTime(txtConfDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            else
            {
                emp.DOC = null;
            }

            if (!txtIssueDate.Text.Trim().Equals(""))
            {
                try
                {
                    emp.NICIssueDate = Convert.ToDateTime(txtIssueDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            else
            {
                emp.NICIssueDate = null;
            }

            if (!txtExpDate.Text.Trim().Equals(""))
            {
                try
                {
                    emp.NICExpiryDate = Convert.ToDateTime(txtExpDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            else
            {
                emp.NICExpiryDate = null;
            }

            emp.Email = txtEmail.Text.Trim();
            emp.SonCount = ddlSonCount.SelectedValue;
            emp.DauughterCount = ddlDaughterCount.SelectedValue;
            emp.Education = txtEdu.Text.Trim();
            //emp.Grade = txtGrade.Text.Trim();
            emp.EmpStatus = Convert.ToInt32(ddlRelieving.SelectedValue);

            if (txtRelieving.Text == "" || txtRelieving.Text == null)
            {
                emp.RelievingDate = null;
            }
            else
            {
                emp.RelievingDate = Convert.ToDateTime(txtRelieving.Text);
            }

            string cnicfile = null;

            if (fuCNIC.HasFile)
            {
                cnicfile = fuCNIC.PostedFile.FileName;
                string extt = System.IO.Path.GetExtension(fuCNIC.FileName);
                int filesiz = fuCNIC.PostedFile.ContentLength;

                    if (filesiz > 5 * 1024 * 1024)
                    {
                        ucMessage.ShowMessage("Attachment Exceeded 5MB", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        if (!fuCNIC.Equals(""))
                        {
                            emp.CNICAttach = cnicfile;
                        }
                    }
                
                fuCNIC.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + cnicfile);
            }
            else
            {
                emp.CNICAttach = null;
            }


            string fileNme = "";

            if (empImageFileUploader.HasFile)
            {
                try
                {
                    fileNme = GetUploadDocName();
                    if (!fileNme.Equals(""))
                    {
                        if (fileNme.Equals("ImgSizeExceeded"))
                        {
                            return;
                        }
                        fileNme = emp.EmpCode + ".jpg";
                        emp.EmpPic = fileNme;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                emp.EmpPic = "noimage.jpg";
            }

            string appoifile = null;
            if (fuAppointment.HasFile)
            {
                appoifile = fuAppointment.PostedFile.FileName;
                string extt = System.IO.Path.GetExtension(fuAppointment.FileName);
                int filesiz = fuAppointment.PostedFile.ContentLength;

                    if (filesiz > 5 * 1024 * 1024)
                    {
                        ucMessage.ShowMessage("Attachment Exceeded 5MB", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        if (!fuAppointment.Equals(""))
                        {
                            emp.AppAttach = appoifile;
                        }
                    }
                
                fuAppointment.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + appoifile);
            }
            else
            {
                emp.AppAttach = null;
            }

            string Orderfile = null;
            if (fuOrder.HasFile)
            {
                Orderfile = fuOrder.PostedFile.FileName;
                string extt = System.IO.Path.GetExtension(fuOrder.FileName);
                int filesiz = fuOrder.PostedFile.ContentLength;

                    if (filesiz > 5 * 1024 * 1024)
                    {
                        ucMessage.ShowMessage("Attachment Exceeded 5MB", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        if (!fuOrder.Equals(""))
                        {
                            emp.OrderAttach = Orderfile;
                        }
                    }
                

                fuOrder.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + Orderfile);
            }
            else
            {
                emp.OrderAttach = null;
            }

            string Regulfile = null;
            if (RegularID.HasFile)
            {
                Regulfile = RegularID.PostedFile.FileName;
                string extt = System.IO.Path.GetExtension(RegularID.FileName);
                int filesiz = RegularID.PostedFile.ContentLength;

                    if (filesiz > 5 * 1024 * 1024)
                    {
                        ucMessage.ShowMessage("Attachment Exceeded 5MB", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        if (!RegularID.Equals(""))
                        {
                            emp.ReguAttachement = Regulfile;
                        }
                    }
                
                RegularID.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + Regulfile);
            }
            else
            {
                emp.ReguAttachement = null;
            }
            emp.jobtype = "Permanent";
            //////////////// JOB TYPE
            if (Convert.ToInt32(ddlJobType.SelectedValue) != 0)
            {
                emp.JobNameID = Convert.ToInt32(ddlJobType.SelectedValue);
            }

            if (Convert.ToInt32(appJobType.SelectedValue) != 0)
            {
                emp.AppointJobType = Convert.ToInt32(ddlJobType.SelectedValue);
            }


            /////////////////////////////////////////////////////////

            //if (!empManager.ISAlreadyExist(emp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //{
            empManager.Insert(emp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            tblPlCode pref = empManager.GetPlCodeByID(emp.DeptID.Value, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            /*
            if (pref != null && !string.IsNullOrEmpty(pref.MiscPayable))
            {
                if (!empManager.IsEmpAcCodeExists(pref.MiscPayable, emp.EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    Glmf_Code gl = new Glmf_Code();
                    gl.gl_cd = pref.MiscPayable + emp.EmpID.ToString().PadLeft(4, '0');
                    gl.gl_dsc = emp.FullName;
                    gl.ct_id = "D";
                    gl.cnt_gl_cd = pref.MiscPayable;
                    gl.updateon = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    gl.gt_cd = empManager.GetGt_CtByCtrl(pref.MiscPayable, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (Session["UserName"] == null)
                    {
                        gl.updateby = Request.Cookies["uzr"]["UserName"];
                    }
                    else
                    {
                        gl.updateby = Session["UserName"].ToString();
                    }
                    //*****************************************************************************
                    EntitySet<Glmf> ettyglmf = new EntitySet<Glmf>();
                    List<Branch> branches = new GlCodeBL1().GetBranches((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    foreach (var b in branches)
                    {
                        Glmf obj = new Glmf();
                        obj.gl_cd = gl.gl_cd;
                        obj.br_id = b.br_id;
                        obj.gl_op = 0;
                        obj.gl_db = 0;
                        obj.gl_cr = 0;
                        obj.gl_obc = 0;
                        obj.gl_not = 0;
                        obj.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                        obj.updateby = gl.updateby;
                        obj.gl_cl = 0;
                        obj.gl_year = new voucherDetailBL().GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (!new GlCodeBL1().IsGlmfCodeExist(obj, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                            ettyglmf.Add(obj);
                    }
                    //*****************************************************************************
                    empManager.SaveGlmf_Code(gl, ettyglmf, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
            }
            */

            //INSERT USER LOGIN
            //UserBL usrBL = new UserBL();
            //if (!usrBL.ISAlreadyExist(emp.EmpCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //{
            //    tblAppUser usr = new tblAppUser();
            //    usr.CompID = emp.CompID;
            //    usr.LoginID = emp.EmpCode;
            //    usr.Password = emp.NIC.Replace("-", "");
            //    usr.GroupID = 3;
            //    usr.UserName = emp.FullName;
            //    usr.Gender = emp.Sex.ToString();
            //    usr.CityID = emp.CityID.Value;
            //    usr.Enabled = true;
            //    usr.UpdatedBy = int.Parse(Session["UserID"].ToString());
            //    usr.UpdatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    usrBL.Insert(usr, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //}

            try
            {
                if (!fileNme.Equals(""))
                {
                    UploadImg(fileNme);
                }
            }
            catch { }

            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), BL.Enums.MessageType.Info);
            BindGrid("", "", BranchID,IsSearch);
            ClearFields();
            //}
            //else
            //{
            //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
            //    pnlMain.Enabled = true;
            //}
        }
        //public EntitySet<Glmf> Get_Glmf_Data(string glCode)
        //{
        //    try
        //    {
        //        string username = "";
        //        if (Session["LoginID"] == null)
        //        {
        //            username = Request.Cookies["uzr"]["LoginID"];
        //        }
        //        else
        //        {
        //            username = Session["LoginID"].ToString();
        //        }

        //        if (username.Length > 15)
        //        {
        //            username = username.Substring(0, 14);
        //        }

        //        List<Branch> branches = new GlCodeBL1().GetBranches((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        foreach (var b in branches)
        //        {
        //            Glmf obj = new Glmf();
        //            obj.gl_cd = glCode;
        //            obj.br_id = b.br_id;
        //            obj.gl_op = 0;
        //            obj.gl_db = 0;
        //            obj.gl_cr = 0;
        //            obj.gl_obc = 0;
        //            obj.gl_not = 0;
        //            obj.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            obj.updateby = username;
        //            obj.gl_cl = 0;
        //            obj.gl_year = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            if (!new GlCodeBL1().IsGlmfCodeExist(obj, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
        //                enttySetGlmf.Add(obj);
        //        }
        //        return enttySetGlmf;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        protected void Update()
        {
            tblPlEmpData updEmp = empManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //updEmp.CompID = Convert.ToByte(Session["CompID"].ToString());
            updEmp.EmpCode = txtEmpCode.Text.Trim();
            updEmp.FullName = txtFullName.Text.Trim();
            //updEmp.sortRef = Convert.ToInt32(txtSortReference.Text);
            // emp.MidName = txtMidName.Text.Trim();
            //  emp.SirName = txtSirName.Text.Trim();

            updEmp.Sex = Convert.ToChar(rblGender.SelectedValue);
            updEmp.MarStatus = Convert.ToChar(rblMarStatus.SelectedValue);
            updEmp.Religion = Convert.ToChar(ddlReligion.SelectedValue);

            updEmp.LastperDes = Convert.ToInt32(ddlLastPero.SelectedValue);
            updEmp.apposcal = Convert.ToInt32(ddlappointscale.SelectedValue);
            updEmp.addchargePalce = Convert.ToInt32(ddlAddPlace.SelectedValue);

            updEmp.UpdateBy = Session["LoginID"].ToString();
            updEmp.UpdateOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            updEmp.Domicil = ddlDomicile.SelectedValue;
            try
            {
                if (!ddlDesignation.SelectedValue.Equals("0"))
                {
                    updEmp.tblPlCode1 = new PlCodeBL().GetByID(Convert.ToInt16(ddlDesignation.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    updEmp.DesigID = Convert.ToInt16(ddlDesignation.SelectedValue);
                }
                else
                {
                    updEmp.tblPlCode1 = null;
                }
            }
            catch { updEmp.tblPlCode1 = null; }


            if (txtadditionalPlace.Text == "")
            {
                updEmp.AddtionalPlace = null;
            }
            else
            {
                updEmp.AddtionalPlace = txtadditionalPlace.Text;
            }
            if (txtaddtionalChargePost.Text == "")
            {
                updEmp.AddtionalPost = null;
            }
            else
            {
                updEmp.AddtionalPost = txtaddtionalChargePost.Text;
            }

            try
            {
                if (!ddlSection.SelectedValue.Equals("0"))
                {
                    updEmp.tblPlCode4 = new PlCodeBL().GetByID(Convert.ToInt16(ddlSection.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    updEmp.SectID = Convert.ToInt16(ddlSection.SelectedValue);
                }
                else
                {
                    updEmp.tblPlCode4 = null;
                }
            }
            catch { updEmp.tblPlCode4 = null; }

            updEmp.EmpStatus = Convert.ToInt32(ddlRelieving.SelectedValue);

            if (txtRelieving.Text == "" || txtRelieving.Text == null)
            {
                updEmp.RelievingDate = null;
            }
            else
            {
                updEmp.RelievingDate = Convert.ToDateTime(txtRelieving.Text);
            }

            //try
            //{
            //    if (!ddlRegion.SelectedValue.Equals("0"))
            //    {
            //        updEmp.tblPlCode3 = new PlCodeBL().GetByID(Convert.ToInt16(ddlRegion.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //        updEmp.RegID = Convert.ToInt16(ddlRegion.SelectedValue);
            //    }
            //    else
            //    {
            //        updEmp.tblPlCode3 = null;
            //    }
            //}
            //catch { updEmp.tblPlCode3 = null; }

            //try
            //{
            //    if (!ddlDivision.SelectedValue.Equals("0"))
            //    {
            //        updEmp.tblPlCode2 = new PlCodeBL().GetByID(Convert.ToInt16(ddlDivision.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //        updEmp.DivID = Convert.ToInt16(ddlDivision.SelectedValue);
            //    }
            //    else
            //    {
            //        updEmp.tblPlCode2 = null;
            //    }
            //}
            //catch { updEmp.tblPlCode2 = null; }

            try
            {
                if (!ddlDept.SelectedValue.Equals("0"))
                {
                    updEmp.tblPlCode = new PlCodeBL().GetByID(Convert.ToInt16(ddlDept.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    updEmp.DeptID = Convert.ToInt16(ddlDept.SelectedValue);
                }
                else
                {
                    updEmp.tblPlCode = null;
                }
            }
            catch { updEmp.tblPlCode = null; }


            try
            {
                if (!ScaleDropDown.SelectedValue.Equals("0"))
                {
                    updEmp.ScaleID = Convert.ToInt16(ScaleDropDown.SelectedValue);
                }

            }
#pragma warning disable CS0219 // The variable 'ex' is assigned but its value is never used
            catch { string ex = null; }
#pragma warning restore CS0219 // The variable 'ex' is assigned but its value is never used


            try
            {
                if (!BranchDropDown.SelectedValue.Equals("0"))
                {
                    updEmp.BranchID = Convert.ToInt16(BranchDropDown.SelectedValue);
                }

            }
#pragma warning disable CS0219 // The variable 'ex' is assigned but its value is never used
            catch { string ex = null; }
#pragma warning restore CS0219 // The variable 'ex' is assigned but its value is never used
            try
            {
                //if (!ddlCity.SelectedValue.Equals("0"))
                //{
                //    updEmp.tblCity = new CityBL().GetByID(Convert.ToInt32(ddlCity.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //    updEmp.CityID = Convert.ToInt32(ddlCity.SelectedValue);

                //    if (!ddlLoc.SelectedValue.Equals("0"))
                //    {
                //        updEmp.tblPlLocation = new LocationBL().GetByID(Convert.ToByte(ddlLoc.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //        updEmp.LocID = Convert.ToByte(ddlLoc.SelectedValue);
                //    }
                //    else
                //    {
                //        updEmp.tblPlLocation = null;
                //    }
                //}
                //else
                //{
                //    updEmp.tblCity = null;
                //    updEmp.tblPlLocation = null;
                //}
            }
            catch { updEmp.tblCity = null; }


            updEmp.FatherName = txtFatherName.Text.Trim();
            updEmp.MotherName = txtMotherName.Text.Trim();

            updEmp.NIC = txtNic.Text.Trim();
            updEmp.NTN = txtNtn.Text.Trim();
            updEmp.TelNo = txtPhoneNo.Text.Trim();
            updEmp.MobNo = txtMobNo.Text.Trim();
            updEmp.EmpAdd1 = txtAdd.Text.Trim();
            updEmp.Quota = ddlQuota.SelectedValue;
            updEmp.Disbality = ddlDisablity.SelectedValue;
            updEmp.EmpAdd2 = txtAdd2Perm.Text.Trim();

            try
            {
                if (ddlBank.SelectedIndex > 0)
                {
                    updEmp.Bank = ddlBank.SelectedValue;
                }
                else
                {
                    updEmp.Bank = null;
                }
            }
            catch { updEmp.Bank = null; }

            updEmp.Branch = txtBankBranch.Text.Trim();
            updEmp.AccountNo = txtBankAcct.Text.Trim();

            updEmp.EobiNo = txtEobi.Text.Trim();
            updEmp.ScsiNo = txtScsi.Text.Trim();

            Boolean eobi = false;
            Boolean.TryParse(rblEobiEnbl.SelectedValue, out eobi);
            updEmp.EobiEnb = eobi;

            Boolean scsienable = false;
            Boolean.TryParse(rblScsiEnbl.SelectedValue, out scsienable);
            updEmp.ScsiEnb = scsienable;

            Boolean hltenable = false;
            Boolean.TryParse(this.rdoHealthIns.SelectedValue, out hltenable);
            updEmp.HlthInsEnb = hltenable;
            if (rdoHealthIns.SelectedValue.Equals("true"))
            {
                updEmp.HelInNo = txtHealthInsurance.Text.Trim();
            }
            else
            {
                updEmp.HelInNo = null;
            }
            if (ddlappointed.SelectedValue == "0")
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "Select Designation").ToString(), RMS.BL.Enums.MessageType.Info);
                return;
            }
            else
            {
                updEmp.Appointed = Convert.ToInt32(ddlappointed.SelectedValue);
            }
            updEmp.AddtionalCharg = ddlEmpAddtional.SelectedValue;
            Boolean Dep = false;
            Boolean.TryParse(rdDeput.SelectedValue, out Dep);
            updEmp.DepEnb = Dep;
            Boolean pol = false;
            Boolean.TryParse(rdpoliceveri.SelectedValue, out pol);
            updEmp.polveri = pol;
            Boolean med = false;
            Boolean.TryParse(rdmediver.SelectedValue, out med);
            updEmp.Mediveri = med;
            Boolean red = false;
            Boolean.TryParse(rddegveri.SelectedValue, out red);
            updEmp.Degveri = red;

            //if (rdDeput.SelectedValue.Equals("true"))
            //{
            //    updEmp.DepNo = txtDepu.Text.Trim();
            //}
            //else
            //{
            //    updEmp.DepNo = null;
            //}

            if (!txtDOB.Text.Trim().Equals(""))
            {
                try
                {
                    updEmp.DOB = Convert.ToDateTime(txtDOB.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            else
            {
                updEmp.DOB = null;
            }

            if (!txtJoinDate.Text.Trim().Equals(""))
            {
                try
                {
                    updEmp.DOJ = Convert.ToDateTime(txtJoinDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            else
            {
                updEmp.DOJ = null;
            }

            if (!txtConfDate.Text.Trim().Equals(""))
            {
                try
                {
                    updEmp.DOC = Convert.ToDateTime(txtConfDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            else
            {
                updEmp.DOC = null;
            }

            if (!txtIssueDate.Text.Trim().Equals(""))
            {
                try
                {
                    updEmp.NICIssueDate = Convert.ToDateTime(txtIssueDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            else
            {
                updEmp.NICIssueDate = null;
            }

            if (!txtExpDate.Text.Trim().Equals(""))
            {
                try
                {
                    updEmp.NICExpiryDate = Convert.ToDateTime(txtExpDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DateFormat").ToString(), RMS.BL.Enums.MessageType.Info);
                    return;
                }
            }
            else
            {
                updEmp.NICExpiryDate = null;
            }
            if(Convert.ToInt32(ddlJobType.SelectedValue) != 0)
            {
                updEmp.JobNameID = Convert.ToInt32(ddlJobType.SelectedValue);
            }

            if (Convert.ToInt32(appJobType.SelectedValue) != 0)
            {
                updEmp.AppointJobType = Convert.ToInt32(appJobType.SelectedValue);
            }


            updEmp.Email = txtEmail.Text.Trim();
            updEmp.SonCount = ddlSonCount.SelectedValue;
            updEmp.DauughterCount = ddlDaughterCount.SelectedValue;
            updEmp.Education = txtEdu.Text.Trim();
            //updEmp.Grade = txtGrade.Text.Trim();

            string fileNme = "";
            
            string Upcnicfile = null;

            if (fuCNIC.HasFile)
            {
                Upcnicfile = fuCNIC.PostedFile.FileName;
                string extt = System.IO.Path.GetExtension(fuCNIC.FileName);
                int filesiz = fuCNIC.PostedFile.ContentLength;
                if (extt == ".jpg" || extt == ".png" || extt == ".gif")
                {
                    if (filesiz > 5 * 1024 * 1024)
                    {
                        ucMessage.ShowMessage("Attachment Exceeded 5MB", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        if (!fuCNIC.Equals(""))
                        {
                            updEmp.CNICAttach = Upcnicfile;
                        }
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Attachment Extension Must be jpg,png and gif", BL.Enums.MessageType.Error);
                    return;
                }
                fuCNIC.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + Upcnicfile);
            }

            string Upappoifile = null;
            if (fuAppointment.HasFile)
            {
                Upappoifile = fuAppointment.PostedFile.FileName;
                string extt = System.IO.Path.GetExtension(fuAppointment.FileName);
                int filesiz = fuAppointment.PostedFile.ContentLength;
                if (extt == ".jpg" || extt == ".png" || extt == ".gif")
                {
                    if (filesiz > 5 * 1024 * 1024)
                    {
                        ucMessage.ShowMessage("Attachment Exceeded 5MB", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        if (!fuAppointment.Equals(""))
                        {
                            updEmp.AppAttach = Upappoifile;
                        }
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Attachment Extension Must be jpg,png and gif", BL.Enums.MessageType.Error);
                    return;
                }
                fuAppointment.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + Upappoifile);
            }
            else
            {
            }

            string upOrderfile = null;
            if (fuOrder.HasFile)
            {
                upOrderfile = fuOrder.PostedFile.FileName;
                string extt = System.IO.Path.GetExtension(fuOrder.FileName);
                int filesiz = fuOrder.PostedFile.ContentLength;
                if (extt == ".jpg" || extt == ".png" || extt == ".gif")
                {
                    if (filesiz > 5 * 1024 * 1024)
                    {
                        ucMessage.ShowMessage("Attachment Exceeded 5MB", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        if (!fuOrder.Equals(""))
                        {
                            updEmp.OrderAttach = upOrderfile;
                        }
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Attachment Extension Must be jpg,png and gif", BL.Enums.MessageType.Error);
                    return;
                }
                fuOrder.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + upOrderfile);
            }
            else
            {
            }

            string RegulFile = null;
            if (RegularID.HasFile)
            {
                RegulFile = RegularID.PostedFile.FileName;
                string extt = System.IO.Path.GetExtension(RegularID.FileName);
                int filesiz = RegularID.PostedFile.ContentLength;
                if (extt == ".jpg" || extt == ".png" || extt == ".gif")
                {
                    if (filesiz > 5 * 1024 * 1024)
                    {
                        ucMessage.ShowMessage("Attachment Exceeded 5MB", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        if (!RegularID.Equals(""))
                        {
                            updEmp.ReguAttachement = RegulFile;
                        }
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Attachment Extension Must be jpg,png and gif", BL.Enums.MessageType.Error);
                    return;
                }
                RegularID.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + RegulFile);
            }
            else
            {
            }






            if (empImageFileUploader.HasFile)
            {
                try
                {
                    fileNme = GetUploadDocName();
                    if (!fileNme.Equals(""))
                    {
                        if (fileNme.Equals("ImgSizeExceeded"))
                        {
                            return;
                        }
                        fileNme = updEmp.EmpCode + ".jpg";
                        UploadImg(fileNme);
                        updEmp.EmpPic = fileNme;
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    //throw ex;
                }
            }
            else
            {
            }


           

            //if (!empManager.ISAlreadyExist(updEmp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //{
            empManager.Update(updEmp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            tblPlCode pref = empManager.GetPlCodeByID(updEmp.DeptID.Value, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (pref != null && !string.IsNullOrEmpty(pref.MiscPayable))
            {
                if (!empManager.IsEmpAcCodeExists(pref.MiscPayable, updEmp.EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    Glmf_Code gl = new Glmf_Code();
                    gl.gl_cd = pref.MiscPayable + updEmp.EmpID.ToString().PadLeft(4, '0');
                    gl.gl_dsc = updEmp.FullName;
                    gl.ct_id = "D";
                    gl.cnt_gl_cd = pref.MiscPayable;
                    gl.updateon = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    gl.gt_cd = empManager.GetGt_CtByCtrl(pref.MiscPayable, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (Session["UserName"] == null)
                    {
                        gl.updateby = Request.Cookies["uzr"]["UserName"];
                    }
                    else
                    {
                        gl.updateby = Session["UserName"].ToString();
                    }
                    //*****************************************************************************
                    EntitySet<Glmf> ettyglmf = new EntitySet<Glmf>();
                    List<Branch> branches = new GlCodeBL1().GetBranches((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    foreach (var b in branches)
                    {
                        Glmf obj = new Glmf();
                        obj.gl_cd = gl.gl_cd;
                        obj.br_id = b.br_id;
                        obj.gl_op = 0;
                        obj.gl_db = 0;
                        obj.gl_cr = 0;
                        obj.gl_obc = 0;
                        obj.gl_not = 0;
                        obj.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                        obj.updateby = gl.updateby;
                        obj.gl_cl = 0;
                        obj.gl_year = new voucherDetailBL().GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        if (!new GlCodeBL1().IsGlmfCodeExist(obj, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                            ettyglmf.Add(obj);
                    }
                    //*****************************************************************************
                    empManager.SaveGlmf_Code(gl, ettyglmf, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                else
                {
                    Glmf_Code gl = empManager.GetGlmf_CodeBYGl_Cd(pref.MiscPayable, updEmp.EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    gl.gl_dsc = updEmp.FullName;
                    gl.updateon = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (Session["UserName"] == null)
                    {
                        gl.updateby = Request.Cookies["uzr"]["UserName"];
                    }
                    else
                    {
                        gl.updateby = Session["UserName"].ToString();
                    }
                    empManager.UpdateGlmf_Code(gl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
            }





            //INSERT USER LOGIN
            //UserBL usrBL = new UserBL();
            //if (!usrBL.ISAlreadyExist(updEmp.EmpCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //{
            //    tblAppUser usr = new tblAppUser();
            //    usr.CompID = updEmp.CompID;
            //    usr.LoginID = updEmp.EmpCode;
            //    usr.Password = updEmp.NIC.Replace("-", "");
            //    usr.GroupID = 3;
            //    usr.UserName = updEmp.FullName;
            //    usr.Gender = updEmp.Sex.ToString();
            //    usr.CityID = updEmp.CityID.Value;
            //    usr.Enabled = true;
            //    usr.UpdatedBy = int.Parse(Session["UserID"].ToString());
            //    usr.UpdatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    usrBL.Insert(usr, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //}


            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
            BindGrid("", "", BranchID,IsSearch);
            ClearFields();
            //}
            //else
            //{
            //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
            //    pnlMain.Enabled = true;
            //}
        }

        protected void Delete(int Id)
        {
            // TRANSACTION WALA KAAM KARNA HAI......
            empManager.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        }

        private void ClearFields()
        {
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

            //fileUploadImg.Visible = false;
            //btnUploadStart.Visible = false;

            empImage.ImageUrl = "~/empix/noimage.jpg";
            imageID.ImageUrl = "";
            appoImage.ImageUrl = "";
            orderImage.ImageUrl = "";
            regID.ImageUrl = "";
            txtaddtionalChargePost.Text = "";
            txtadditionalPlace.Text = "";
            //btnAddImg.Visible = true;
            //btnUpdImg.Visible = false;

            divEmpTransfer.Visible = false;
            ddlDomicile.SelectedValue = "0";
            ddlEmpAddtional.SelectedValue = "0";
            txtIssueDate.Text = "";
            txtExpDate.Text = "";
            txtMotherName.Text = "";
            this.txtEmpId.Text = "";
            this.txtEmpCode.Text = "";
            this.txtFullName.Text = "";
            //this.txtSortReference.Text = "";
            ddlQuota.SelectedValue = "0";
            this.ddlJobType.SelectedValue = "0";
            ddlDisablity.SelectedValue = "0";
            this.rblGender.SelectedValue = "M";
            this.rblMarStatus.SelectedValue = "M";
            //this.ddlRegion.SelectedValue = "0";
            //this.ddlDivision.SelectedValue = "0";
            this.ddlDept.SelectedValue = "0";
            this.ScaleDropDown.SelectedValue = "0";
            ddlappointed.SelectedValue = "0";
            ddlReligion.SelectedValue = "0";
            this.BranchDropDown.SelectedValue = "0";
            this.ddlDesignation.SelectedValue = "0";
            this.ddlSection.SelectedValue = "0";

            ddlLoc.Items.Clear();
            ddlLoc.Dispose();
            selListSub.Text = "Select Location";
            selListSub.Value = "0";
            ddlLoc.Items.Insert(0, selListSub);

            this.txtFatherName.Text = "";
            this.txtNic.Text = "";
            this.txtNtn.Text = "";
            this.txtPhoneNo.Text = "";
            this.txtMobNo.Text = "";
            this.txtAdd.Text = "";

            this.txtDOB.Text = "";
            this.txtJoinDate.Text = "";
            this.txtConfDate.Text = "";
            this.txtEmail.Text = "";
            this.txtEdu.Text = "";
            //this.txtGrade.Text = "";
            this.ddlSonCount.SelectedValue = "0";
            this.ddlDaughterCount.SelectedValue = "0";
            grdEmps.SelectedIndex = -1;
           // this.ddlCity.SelectedIndex = -1;

            ClearFieldsTransfer();
            btnViewTransfer.Visible = false;

            this.ddlBank.SelectedValue = "0";
            this.txtBankBranch.Text = "";
            this.txtBankAcct.Text = "";
            this.txtEobi.Text = "";
            this.txtScsi.Text = "";
            ddlappointscale.SelectedValue = "0";
            ddlLastPero.SelectedValue = "0";
            ddlAddPlace.SelectedValue = "0";
            this.txtAdd2Perm.Text = "";
            this.rblEobiEnbl.SelectedValue = "true";
            this.rblScsiEnbl.SelectedValue = "true";
            this.rdoHealthIns.SelectedValue = "True";
            rdDeput.SelectedValue = "True";
            rdpoliceveri.SelectedValue = "True";
            rdmediver.SelectedValue = "True";
            rddegveri.SelectedValue = "True";
            //txtDepu.Text = "";
            txtHealthInsurance.Text = "";
            txtEobi.Enabled = true;
            txtScsi.Enabled = true;
            txtEmpCode.Focus();
        }

        protected void ddlEmpDrpdown_change(object sender, EventArgs e)
        {
            using(RMSDataContext db = new RMSDataContext())
            {
                string emppp = ddlEmpDrpdwn.SelectedValue;
                string empref = db.tblPlEmpDatas.Where(x => x.FullName == emppp).FirstOrDefault().EmpCode;
                ddlperson.SelectedValue = empref;
            }
        }

        protected void ddlPersonal_change(object sender, EventArgs e)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
                string emppp = ddlperson.SelectedValue;
                string empref = db.tblPlEmpDatas.Where(x => x.EmpCode == emppp).FirstOrDefault().FullName;
                ddlEmpDrpdwn.SelectedValue = empref;
            }
        }

        private void FillDropDownCodeDept()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 3;

            this.ddlDept.DataTextField = "CodeDesc";
            ddlDept.DataValueField = "CodeID";
            ddlDept.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDept.DataBind();

        }
        //protected void FillDropdownDivision()
        //{
        //    BranchBL br = new BranchBL();
        //    ddlDivision.DataTextField = "br_nme";
        //    ddlDivision.DataValueField = "br_id";
        //    ddlDivision.DataSource = br.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlDivision.DataBind();
        //}

        protected void FillDropdownEmployeee()
        {
            RMSDataContext db = new RMSDataContext();
            ddlEmpDrpdwn.DataTextField = "FullName";
            ddlEmpDrpdwn.DataValueField = "FullName";
            if (BranchID == 1)
            {
                ddlEmpDrpdwn.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID != 14 && x.BranchID != null).OrderBy(x => x.FullName).ToList();
            }
            else
            {
                ddlEmpDrpdwn.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
            }
            
            ddlEmpDrpdwn.DataBind();
            ddlEmpDrpdwn.Items.Insert(0, new ListItem("Select", "0"));
        }

        protected void FillDropdownPersonal()
        {
            RMSDataContext db = new RMSDataContext();
            ddlperson.DataTextField = "EmpCode";
            ddlperson.DataValueField = "EmpCode";
            if (BranchID == 1)
            {
                ddlperson.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID != null && x.BranchID != 14).ToList();
            }
            else
            {
                ddlperson.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
            }
            
            ddlperson.DataBind();
            ddlperson.Items.Insert(0, new ListItem("Select", "0"));
        }



        private void FillDropDownScale()
        {
            RMSDataContext db = new RMSDataContext();

            this.ScaleDropDown.DataTextField = "ScaleName";
            ScaleDropDown.DataValueField = "ScaleID";
            ScaleDropDown.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.Orderby);
            ScaleDropDown.DataBind();

        }

        private void FillappoiDropDownScale()
        {
            RMSDataContext db = new RMSDataContext();

            this.ddlappointscale.DataTextField = "ScaleName";
            ddlappointscale.DataValueField = "ScaleID";
            ddlappointscale.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.Orderby);
            ddlappointscale.DataBind();

        }


        private void FillDropdownBranchCasCade()
        {
            //int id = Convert.ToInt32(BranchDropDown.SelectedValue);
            //RMSDataContext db = new RMSDataContext();
            //dropdownbranch.DataTextField = "br_nme";
            //dropdownbranch.DataValueField = "br_id";
            //dropdownbranch.DataSource = db.Branches.Where(x => x.br_idd == BranchID && x.br_status == true).ToList();
            //dropdownbranch.DataBind();
        }

        private void FillBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();
            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            this.BranchDropDown.DataTextField = "br_nme";
            BranchDropDown.DataValueField = "br_id";
            if (BranchObj.IsHead == true)
            {
                BranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
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
                BranchDropDown.DataSource = BranchList.ToList();
            }
            BranchDropDown.DataBind();

        }
        //private void FillDistricDropDown(object idddd)
        //{
        //    RMSDataContext db = new RMSDataContext();

        //    this.BranchDropDown.DataTextField = "br_nme";
        //    BranchDropDown.DataValueField = "br_id";
        //    BranchDropDown.DataSource = db.Branches.Where(x => x.br_idd.Equals(idddd)).ToList();
        //    BranchDropDown.DataBind();

        //}
        //private void FillDropDownCodeRegion()
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
        //    pl.CodeTypeID = 1;
        //    this.ddlRegion.DataTextField = "CodeDesc";
        //    ddlRegion.DataValueField = "CodeID";
        //    ddlRegion.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlRegion.DataBind();

        //}

        //private void FillDropDownCodeDivision()
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
        //    pl.CodeTypeID = 2;
        //    this.ddlDivision.DataTextField = "CodeDesc";
        //    ddlDivision.DataValueField = "CodeID";
        //    ddlDivision.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlDivision.DataBind();
        //}

        private void FillDropDownCodeDsgn()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 4;
            this.ddlDesignation.DataTextField = "CodeDesc";
            ddlDesignation.DataValueField = "CodeID";
            ddlDesignation.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDesignation.DataBind();
        }

        private void FillDropDownCodeDsgnApp()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 4;
            this.ddlappointed.DataTextField = "CodeDesc";
            ddlappointed.DataValueField = "CodeID";
            ddlappointed.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlappointed.DataBind();
        }


        private void FillDropDownDCodeLastsgnApp()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 4;
            this.ddlLastPero.DataTextField = "CodeDesc";
            ddlLastPero.DataValueField = "CodeID";
            ddlLastPero.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLastPero.DataBind();
        }

        private void FillDropDownAddtionalDes()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 4;
            this.ddlEmpAddtional.DataTextField = "CodeDesc";
            ddlEmpAddtional.DataValueField = "CodeDesc";
            ddlEmpAddtional.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlEmpAddtional.DataBind();
        }
        private void FillDropDownCodeSect()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 5;
            this.ddlSection.DataTextField = "CodeDesc";
            ddlSection.DataValueField = "CodeID";
            ddlSection.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSection.DataBind();
        }

        //private void FillDropDownCities()
        //{
        //    ddlCity.DataTextField = "CityName";
        //    ddlCity.DataValueField = "CityID";
        //    ddlCity.DataSource = new CityBL().GetAllCityCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlCity.DataBind();
        //}

        private void FillDropDownBanks()
        {
            ddlBank.DataTextField = "BankABv";
            ddlBank.DataValueField = "BankCode";
            ddlBank.DataSource = new BankBL().GetAll(BranchID,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlBank.DataBind();
        }

        private void FillDropDownLocations()
        {
           // ddlLoc.Items.Clear();
           // ddlLoc.Dispose();
           // selList.Text = "Select Location";
           // selList.Value = "0";
           // ddlLoc.Items.Insert(0, selList);

           // ddlLoc.DataTextField = "LocName";
           // ddlLoc.DataValueField = "LocID";
           //// ddlLoc.DataSource = new CityBL().GetAllCityLocsCombo(Convert.ToInt32(ddlCity.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
           // ddlLoc.DataBind();

        }

        private void FillDropDownCodeDeptTransfer()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 3;

            this.ddlDeptTransfer.DataTextField = "CodeDesc";
            ddlDeptTransfer.DataValueField = "CodeID";
            ddlDeptTransfer.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDeptTransfer.DataBind();

        }




        private void FillDropDownCodeRegionTransfer()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 1;
            this.ddlRegionTransfer.DataTextField = "CodeDesc";
            ddlRegionTransfer.DataValueField = "CodeID";
            ddlRegionTransfer.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlRegionTransfer.DataBind();

        }

        private void FillDropDownCodeDivisionTransfer()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 2;
            this.ddlDivTransfer.DataTextField = "CodeDesc";
            ddlDivTransfer.DataValueField = "CodeID";
            ddlDivTransfer.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDivTransfer.DataBind();
        }

        private void FillDropDownCodeDsgnTransfer()
        {
            tblPlCode pl = new tblPlCode();
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            }
            pl.CompID = _cmp;
            pl.CodeTypeID = 4;
            this.ddlDesigTransfer.DataTextField = "CodeDesc";
            ddlDesigTransfer.DataValueField = "CodeID";
            ddlDesigTransfer.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDesigTransfer.DataBind();
        }

        private void FillDropDownCitiesTransfer()
        {
            ddlCityTransfer.DataTextField = "CityName";
            ddlCityTransfer.DataValueField = "CityID";
            ddlCityTransfer.DataSource = new CityBL().GetAllCityCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCityTransfer.DataBind();
        }

        private void FillDropDownLocationsTransfer()
        {
            //ddlLocTransfer.Items.Clear();
            //ddlLocTransfer.Dispose();
            //selList.Text = "Select Location";
            //selList.Value = "0";
            //ddlLocTransfer.Items.Insert(0, selList);

            //ddlLocTransfer.DataTextField = "LocName";
            //ddlLocTransfer.DataValueField = "LocID";
            //ddlLocTransfer.DataSource = new CityBL().GetAllCityLocsCombo(Convert.ToInt32(ddlCity.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //ddlLocTransfer.DataBind();

        }

        //private void FillSearchBranchDropDown()
        //{
        //    RMSDataContext db = new RMSDataContext();

        //    Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

        //    this.searchBranchDropDown.DataTextField = "br_nme";
        //    searchBranchDropDown.DataValueField = "br_id";
        //    if(BranchObj.IsHead == true)
        //    {
        //        searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
        //    }
        //    else 
        //    {
        //        List<Branch> BranchList = new List<Branch>();
                
        //        if(BranchObj != null)
        //        {
        //            if (BranchObj.IsDisplay == true)
        //            {
        //                BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BranchID).ToList();
        //                BranchList.Insert(0,BranchObj);
        //            }
        //            else
        //            {
        //                BranchList.Add(BranchObj);
        //            }
        //        }
        //        searchBranchDropDown.DataSource = BranchList.ToList();
        //    }
        //    searchBranchDropDown.DataBind();

        //}


        private void AddtionalChargePlace()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            this.ddlAddPlace.DataTextField = "br_nme";
            ddlAddPlace.DataValueField = "br_id";
            ddlAddPlace.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            ddlAddPlace.DataBind();

        }

        //protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!searchBranchDropDown.SelectedValue.Equals("0"))
        //        {
        //            IsSearch = true;
        //            BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
        //            BindGrid("","","True",BranchID, IsSearch);
        //        }

        //    }
        //    catch
        //    { }
        //}

        private void PrintEmpProfile(int empid)
        {
            //try
            //{
            //    if (empid > 0)
            //    {


            //        List<spEmpBasicInfoResult> result1 = empProfRptBL.GetEmpBasicInfo(EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //        List<spCurrentSalaryPackageResult> result2 = empProfRptBL.GetCurrentSalaryPackage(CompID, EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //        //ReportViewer reportViewer = new ReportViewer();
            //        //reportViewer.Visible = false;
            //        reportViewer.LocalReport.ReportPath = "report/rdlc/rptEmpProfile.rdlc";
            //       // reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            //        reportViewer.LocalReport.Refresh();
            //        reportViewer.LocalReport.EnableExternalImages = true;
            //        reportViewer.LocalReport.Refresh();

            //        string passcoLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            //        string empImagePath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["EmpImage"].ToString().Trim());

            //        ReportDataSource dataSource1 = new ReportDataSource("spEmpBasicInfoResult", result1);
            //        ReportDataSource dataSource2 = new ReportDataSource("spCurrentSalaryPackageResult", result2);

            //        ReportParameter[] rpt = new ReportParameter[4];
            //        rpt[0] = new ReportParameter("LogoPath", passcoLogoPath);
            //        rpt[1] = new ReportParameter("EmpImagePath", empImagePath + result1.Single().EmpPic);
            //        rpt[2] = new ReportParameter("ReportName", "EMPLOYEE REPORT");
            //        if (Session["CompName"] == null)
            //        {
            //            rpt[3] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
            //        }
            //        else
            //        {
            //            rpt[3] = new ReportParameter("CompName", Session["CompName"].ToString());
            //        }


            //        reportViewer.LocalReport.SetParameters(rpt);

            //        reportViewer.LocalReport.DataSources.Clear();
            //        reportViewer.LocalReport.DataSources.Add(dataSource1);
            //        reportViewer.LocalReport.DataSources.Add(dataSource2);

            //        //Warning[] warnings;
            //        //string[] streamids;
            //        //string mimeType;
            //        //string encoding;
            //        //string extension;
            //        //string filename;
            //        //byte[] bytes = reportViewer.LocalReport.Render(
            //        //   "PDF", null, out mimeType, out encoding,
            //        //    out extension,
            //        //   out streamids, out warnings);
            //        //filename = string.Format("{0}.{1}", "Employee_Profile_Rpt_EmpID_" + result1.Single().EmpCode, "pdf");
            //        //Response.ClearHeaders();
            //        //Response.Clear();
            //        //Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            //        //Response.ContentType = mimeType;
            //        //Response.BinaryWrite(bytes);
            //        //Response.Flush();
            //        //Response.End();
            //    }
            //    else
            //    {
            //        ucMessage.ShowMessage("Select an employee to print", RMS.BL.Enums.MessageType.Error);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            //}
        }
        //[WebMethod]
        //[System.Web.Script.Services.ScriptMethod]
        //public static object GetCascadingBranchChange(int Branches)
        //{

        //    BranchBL branchBL = new BranchBL();

        //    //Here MyDatabaseEntities  is our dbContext

        //    var data = branchBL.GetDistric(Branches, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
        //    return data;

        //    //using (RMSDataContext db = new RMSDataContext())
        //    //{

        //    //    var data = db.Branches.Where(x => x.br_idd == Branches).ToList();


        //    //    //var EmpData = new object[data.Count + 1];
        //    //    //EmpData[0] = new object[]{
        //    //    //    "0",
        //    //    //"Select District"
        //    //    return data;
        //    //};

        //    //int j = 0;
        //    //foreach (var i in data)
        //    //{
        //    //    j++;
        //    //    EmpData[j] = new object[] { i.br_id.ToString(), i.br_nme };
        //    //}



        //}
    #endregion

    }
}

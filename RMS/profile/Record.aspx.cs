using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Web.UI;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace RMS.Profile
{
    public partial class Record : Page
    {

        #region DataMembers
        RMSDataContext db = new RMSDataContext();
        EmpBL empBL = new EmpBL();
        EmpEduBL empedu = new EmpEduBL();
        EmpExpBL empexp = new EmpExpBL();
        EmpAcrBL empacr = new EmpAcrBL();
        EmpEnqBL empenq = new EmpEnqBL();
        EmpLitiBL emplit = new EmpLitiBL();
        EmpCpfBL empcpf = new EmpCpfBL();
        CityBL city = new CityBL();
        #endregion

        #region Properties
#pragma warning disable CS0114 // 'Record.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'Record.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public int LitiID
        {
            get { return (ViewState["LitiID"] == null) ? 0 : Convert.ToInt32(ViewState["LitiID"]); }
            set { ViewState["LitiID"] = value; }
        }

        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

        public static int IsBranch
        {
            get; set;
        }
        #endregion

        #region Events
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                    BranchID = Convert.ToInt32(Session["BranchID"]);
                }

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "profileRecord").ToString();

                //FillSearchBranchDropDown();
                FillSearchDropDownEmployee();
                //FillDropDownCodeDsgn();
                FillDropDownpostingDsgn();
                FillDropDownCity();
                FillDivisionDropDownEmployee();
                FillDropDownScaleTenure();
                FillDropDownYearPanel();
                FillDropDownLitiPanel();
                FillDropDownLiti2Panel();
                FillDropDownScale();
                FillDropDownReportingDsgn();
                FillDropDownofficerDsgn();
                FillDropDownPermotionDsgn();
                FillDropDownPermotionScal();
                FillDropDownDegreetype();
                FillDropdownScaleTenureExp();
                FillEnquiryTypes();
                //FillDropDownPriorPostDsgn();
                //FillDropDownPriorAddtinalDsgn();
                FillDropDownTenureAddtinalDsgn();
                FillDropDownTenurePostDsgn();
                FillDropDownPlaceTenure();
                FillDropdownPersonal();
                //searchBranchDropDown.SelectedValue = BranchID.ToString();
                //BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                if (Session["DateFormat"] == null)
                {
                    txtExpDOJCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    //txtEndDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    EnqDateEnq.Format = Request.Cookies["uzr"]["DateFormat"];
                    litiDateliti.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtperfromCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtPerToCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtExpLeaving.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtleaCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtjoinCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    //txtofficerDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtrepDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtDateFromCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtDateToCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtupdatedateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtFinalDateCal.Format = Request.Cookies["uzr"]["DateFormat"];

                }
                else
                {
                    txtExpDOJCal.Format = Session["DateFormat"].ToString();
                    // txtEndDateCal.Format = Session["DateFormat"].ToString();
                    EnqDateEnq.Format = Session["DateFormat"].ToString();
                    litiDateliti.Format = Session["DateFormat"].ToString();
                    txtperfromCal.Format = Session["DateFormat"].ToString();
                    txtPerToCal.Format = Session["DateFormat"].ToString();
                    txtExpLeaving.Format = Session["DateFormat"].ToString();
                    txtleaCal.Format = Session["DateFormat"].ToString();
                    txtjoinCal.Format = Session["DateFormat"].ToString();
                    //txtofficerDateCal.Format = Session["DateFormat"].ToString();
                    txtrepDateCal.Format = Session["DateFormat"].ToString();
                    txtDateFromCal.Format = Session["DateFormat"].ToString();
                    txtDateToCal.Format = Session["DateFormat"].ToString();
                    txtupdatedateCal.Format = Session["DateFormat"].ToString();
                    txtFinalDateCal.Format = Session["DateFormat"].ToString();
                }
            }
        }

        protected void BtnEmployeeEducation_Click(object sender, EventArgs e)
        {
            if (ddlEmployeeSearch.SelectedValue == "0")
            {
                txterror.InnerText = "Please Select Employee";
            }
            else
            {
                BindGridEduPanel();
                BindGridExpPanel();
                BindGridAcrPanel();
                BindGridEnqPanel();
                BindGridLitiPanel();
                BindGridPerPanel();
                BindGridtenure();
                BindGridUpdateEnq();
                BindGridLitiUpdate();
            }
        }
        protected void btnEmpEdu_Click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                try
                {
                    int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    int city = Convert.ToInt32(ddlCity.SelectedValue);
                    int year = Convert.ToInt32(ddlYear.SelectedValue);
                    tblPlEmpEdu empEdu = new tblPlEmpEdu();

                    
                        string fileNme = "";

                        if (FileUploader.HasFile)
                        {
                            try
                            {
                            string extension = System.IO.Path.GetExtension(FileUploader.FileName);
                            fileNme = FileUploader.PostedFile.FileName;
                            int filesize = FileUploader.PostedFile.ContentLength;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucMessage.ShowMessage("File Size Should be less than 5MB ", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileNme.Equals(""))
                                {
                                    empEdu.filePath = fileNme;
                                }
                                
                            }
                            else
                            {
                               
                                ucMessage.ShowMessage("file extension must be jpg, png, gif, doc or docx", BL.Enums.MessageType.Error);
                                return;
                            }
                            
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        FileUploader.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + fileNme);
                        }
                    else
                    {
                        
                    }
                    

                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.EmpID = empsearch;
                    }
                    if (ddlDegreeType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Degree Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.Degreetype = ddlDegreeType.SelectedValue;
                    }
                    if (ddlCity.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select City", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.CityID = city;
                    }
                    if (txtEduDegTtl.Text == "")
                    {
                        ucMessage.ShowMessage("Degree Title is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.DegreeTitle = txtEduDegTtl.Text.Trim();
                    }
                    if (ddluniversity.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please select University / Board", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.UniversityBoard = ddluniversity.SelectedValue;
                    }
                    if (txtPercentage.Text == "")
                    {
                        ucMessage.ShowMessage("Percentage Field is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.Percente = txtPercentage.Text.Trim();
                    }
                    if (year == 0)
                    {
                        ucMessage.ShowMessage("Year is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.Year = year;
                    }
                        empEdu.Verified = Convert.ToBoolean(ddlEduVerified.SelectedValue);
                    empedu.Insert(empEdu, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucMessage.ShowMessage("Sucessfully Added", RMS.BL.Enums.MessageType.Info);
                    

                }
                catch (Exception ex)
                {
                    ucMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);

                }
            }
            else
            {
                try
                {
                    int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    int city = Convert.ToInt32(ddlCity.SelectedValue);
                    int year = Convert.ToInt32(ddlYear.SelectedValue);
                    tblPlEmpEdu empEdu = new tblPlEmpEdu();
                    empEdu = empedu.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    string fileNme = "";

                    if (FileUploader.HasFile)
                    {
                        try
                        {
                            string extension = System.IO.Path.GetExtension(FileUploader.FileName);
                            fileNme = FileUploader.PostedFile.FileName;
                            int filesize = FileUploader.PostedFile.ContentLength;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucMessage.ShowMessage("File Size Should be less than 5MB ", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileNme.Equals(""))
                                {
                                        empEdu.filePath = fileNme;
                                }

                            }
                            else
                            {
                                ucMessage.ShowMessage("File Extension Must be jpg, png, gif, doc or docx", BL.Enums.MessageType.Error);
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        FileUploader.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + fileNme);
                    }
                    else
                    {
                    }

                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.EmpID = empsearch;
                    }
                    if (ddlDegreeType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Degree Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.Degreetype = ddlDegreeType.SelectedValue;
                    }
                    if (ddlCity.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select City", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.CityID = city;
                    }
                    if (txtEduDegTtl.Text == "")
                    {
                        ucMessage.ShowMessage("Degree Title is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.DegreeTitle = txtEduDegTtl.Text.Trim();
                    }
                    if (ddluniversity.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please select University / Board", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.UniversityBoard = ddluniversity.SelectedValue;
                    }
                    if (txtPercentage.Text == "")
                    {
                        ucMessage.ShowMessage("Percentage Field is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.Percente = txtPercentage.Text.Trim();
                    }
                    if (year == 0)
                    {
                        ucMessage.ShowMessage("Year is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        empEdu.Year = year;
                    }
                    empEdu.Verified = Convert.ToBoolean(ddlEduVerified.Text.Trim());
                    
                    empedu.Update(empEdu, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucMessage.ShowMessage("Updated Succesfully", RMS.BL.Enums.MessageType.Info);
                    

                }
                catch (Exception ex)
                {
                    ucMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);

                }
            }
            
              BindGridEduPanel();
              clear_Text();
        }


        protected void btnEmpEduClear_Clear(object sender, EventArgs e)
        {
            //this.ddlEmployeeSearch.SelectedValue = "0";
            this.ddlYear.SelectedValue = "0";
            txtEduDegTtl.Text = "";
            ddluniversity.SelectedValue = "0";
            this.ddlCity.SelectedValue = "0";
            this.ddlEduVerified.SelectedValue = "True";
            this.ImageEdu.ImageUrl = "";
            txtPercentage.Text = "";
            ddlDegreeType.SelectedValue = "0";
        }

        protected void btnEmpExp_Save(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                try
                {

                    int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    //short designation = Convert.ToInt16(ddlDesignation.Text.Trim());
                    tblPlEmpExp emp = new tblPlEmpExp();
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucExpMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.EmpID = empsearch;
                    }
                    //if (ddlDesignation.SelectedValue == "0")
                    //{
                    //    ucExpMessage.ShowMessage("Please Select Designation", BL.Enums.MessageType.Error);
                    //    return;
                    //}
                    //else
                    //{
                    //    emp.CodeID = designation;
                    //}
                    //if (txtExpAppAs.Text == "")
                    //{
                    //    ucExpMessage.ShowMessage("Applied As is Required", BL.Enums.MessageType.Error);
                    //    return;
                    //}
                    //else
                    //{
                    //    emp.Appointedas = txtExpAppAs.Text.Trim();
                    //}
                    if (txtpriorExp.Text == "")
                    {
                        ucExpMessage.ShowMessage("Post Held is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.Postedas = txtpriorExp.Text.Trim();
                    }
                    if (txtaddtionalChar.Text == "")
                    {
                    }
                    else
                    {
                        emp.Department = txtaddtionalChar.Text.Trim();
                    }
                    if (ddlExpSector.SelectedValue == "0")
                    {
                        ucExpMessage.ShowMessage("Please Select Sector", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.Sector = ddlExpSector.SelectedValue.Trim();
                    }

                    if (txtOrganization.Text == "")
                    {
                        ucExpMessage.ShowMessage("Organization Name is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.OrgName = txtOrganization.Text.Trim();
                    }


                    if (ddlScale.SelectedValue == "")
                    {
                        ucExpMessage.ShowMessage("Please Select Scale", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.Scale = Convert.ToInt32(ddlScale.SelectedValue);
                    }
                    //emp.Designation = txtdes.Text.Trim();
                    if (txtExpDOJ.Text == "")
                    {
                        ucExpMessage.ShowMessage("From Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.joinDate = Convert.ToDateTime(txtExpDOJ.Text);
                    }

                    if (txtLeaving.Text == "")
                    {
                        ucExpMessage.ShowMessage("To Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.leavDate = Convert.ToDateTime(txtLeaving.Text.Trim());
                    }
                    emp.monthExp = Convert.ToInt32(ddlMonthExp.SelectedValue);
                    if (ddlDomicile.SelectedValue == "0")
                    {
                        
                    }
                    else
                    {
                        emp.YOE = Convert.ToInt32(ddlDomicile.SelectedValue);
                    }

                    string fileExp = "";

                    if (ExpFileUpload.HasFile)
                    {
                        try
                        {
                            string extension = System.IO.Path.GetExtension(ExpFileUpload.FileName);
                            fileExp = ExpFileUpload.PostedFile.FileName;
                            int filesize = ExpFileUpload.PostedFile.ContentLength;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucExpMessage.ShowMessage("File Size Should be less than 5MB ", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileExp.Equals(""))
                                {
                                    
                                    emp.Attachment = fileExp;
                                }

                            }
                            else
                            {

                                ucExpMessage.ShowMessage("file extension must be jpg, png, gif, doc or docx", BL.Enums.MessageType.Error);
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        ExpFileUpload.PostedFile.SaveAs(Server.MapPath("..\\ExpAttachments\\") + fileExp);
                    }
                    else
                    {
                    }

                    empexp.Insert(emp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucExpMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
                    BindGridExpPanel();
                    clear_Text();
                }
                catch (Exception ex)
                {
                    ucMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                    throw;
                }
            }
            else
            {
                try
                {

                    int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                   // short designation = Convert.ToInt16(ddlDesignation.Text.Trim());
                    tblPlEmpExp emp = new tblPlEmpExp();
                    emp = empexp.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucExpMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.EmpID = empsearch;
                    }
                    //if (ddlDesignation.SelectedValue == "0")
                    //{
                    //    ucExpMessage.ShowMessage("Please Select Designation", BL.Enums.MessageType.Error);
                    //    return;
                    //}
                    //else
                    //{
                    //    emp.CodeID = designation;
                    //}
                    //if (txtExpAppAs.Text == "")
                    //{
                    //    ucExpMessage.ShowMessage("Applied As is Required", BL.Enums.MessageType.Error);
                    //    return;
                    //}
                    //else
                    //{
                    //    emp.Appointedas = txtExpAppAs.Text.Trim();
                    //}
                    if (txtpriorExp.Text == "")
                    {
                        ucExpMessage.ShowMessage("Post Held is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.Postedas = txtpriorExp.Text.Trim();
                    }
                    if (txtaddtionalChar.Text == "")
                    {
                    }
                    else
                    {
                        emp.Department = txtaddtionalChar.Text.Trim();
                    }
                    if (ddlExpSector.SelectedValue == "0")
                    {
                        ucExpMessage.ShowMessage("Please Select Sector", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.Sector = ddlExpSector.SelectedValue.Trim();
                    }
                    if(txtOrganization.Text == "")
                    {
                        ucExpMessage.ShowMessage("Organization Name is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.OrgName = txtOrganization.Text.Trim();
                    }
                    if (ddlScale.SelectedValue == "")
                    {
                    }
                    else
                    {
                        emp.Scale = Convert.ToInt32(ddlScale.SelectedValue);
                    }
                    //emp.Designation = txtdes.Text.Trim();
                    if (txtExpDOJ.Text == "")
                    {
                        ucExpMessage.ShowMessage("Date of joining is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.joinDate = Convert.ToDateTime(txtExpDOJ.Text);
                    }
                    if (txtLeaving.Text == "")
                    {
                    }
                    else
                    {
                        emp.leavDate = Convert.ToDateTime(txtLeaving.Text);
                    }
                    emp.monthExp = Convert.ToInt32(ddlMonthExp.SelectedValue);
                    if (ddlDomicile.SelectedValue == "0")
                    {
                        
                    }
                    else
                    {
                        emp.YOE = Convert.ToInt32(ddlDomicile.SelectedValue);
                    }

                    string fileExp = "";

                    if (ExpFileUpload.HasFile)
                    {
                        try
                        {
                            string extension = System.IO.Path.GetExtension(ExpFileUpload.FileName);
                            fileExp = ExpFileUpload.PostedFile.FileName;
                            int filesize = ExpFileUpload.PostedFile.ContentLength;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucMessage.ShowMessage("File Size Should be less than 5MB ", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileExp.Equals(""))
                                {
                                    //if(string.IsNullOrEmpty(emp.Attachment))
                                    //{
                                    //}
                                    //else
                                    //{
                                    //    System.IO.File.Delete(Server.MapPath("~/ExpAttachments/" + emp.Attachment));
                                    //}
                                    emp.Attachment = fileExp;
                                }

                            }
                            else
                            {

                                ucMessage.ShowMessage("file extension must be jpg, png, gif, doc or docx", BL.Enums.MessageType.Error);
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        ExpFileUpload.PostedFile.SaveAs(Server.MapPath("..\\ExpAttachments\\") + fileExp);
                    }
                    else
                    {
                    }


                    empexp.Update(emp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucExpMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
                    
                }
                catch (Exception ex)
                {
                    ucExpMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                    throw;
                }
            }

            BindGridExpPanel();
            clear_Text();
        }

        protected void EmpExpClear_Clear(object sender, EventArgs e)
        {
            //this.ddlEmployeeSearch.SelectedValue = "0";
           // this.ddlDesignation.SelectedValue = "0";
            txtpriorExp.Text = "";
           // txtExpAppAs.Text = "";
            txtLeaving.Text = "";
            txtLeaving.Text = "";
            ImageExp.ImageUrl = "";
            ddlExpSector.SelectedValue = "0";
            txtOrganization.Text = "";
           // txtdep.Text = "";
            ddlScale.SelectedValue = "";
            this.ddlDomicile.SelectedValue = "0";
            txtExpDOJ.Text = "";
            ddlMonthExp.SelectedValue = "0";
            txtaddtionalChar.Text = "";
        }
        protected void btnEmpTenureExp_Save(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                try
                {

                    int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    //short designation = Convert.ToInt16(ddlDesignation.Text.Trim());
                    TenureExperience emp = new TenureExperience();
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucTMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.EmpID = empsearch;
                    }
                    //if (ddlDesignation.SelectedValue == "0")
                    //{
                    //    ucExpMessage.ShowMessage("Please Select Designation", BL.Enums.MessageType.Error);
                    //    return;
                    //}
                    //else
                    //{
                    //    emp.CodeID = designation;
                    //}
                    if (ddlDivsion.SelectedValue == "0")
                    {
                        emp.BranchID = null;
                    }
                    else
                    {
                        emp.BranchID = Convert.ToInt32(ddlDivsion.SelectedValue);
                    }
                    
                    if (ddlTenurePost.SelectedValue == "0")
                    {
                        ucTMessage.ShowMessage("Post Held is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.Postedas = ddlTenurePost.SelectedValue;
                    }
                    if (ddlTenureAddtional.SelectedValue == "0")
                    {
                        emp.AddtionalCharge = null;
                    }
                    else
                    {
                        emp.AddtionalCharge = ddlTenureAddtional.SelectedValue;
                    }
                    if (txtaddtionalChargePost.Text == "")
                    {
                        emp.addpost = "";
                    }
                    else
                    {
                        emp.addpost = txtaddtionalChargePost.Text.Trim();
                    }
                    if (ddlAdditionPlace.SelectedValue == "0")
                    {
                        emp.addtionalChargePlace = null;
                    }
                    else
                    {
                        emp.addtionalChargePlace = Convert.ToInt32(ddlAdditionPlace.SelectedValue);
                    }

                    if (txtadditionalPlace.Text == "")
                    {
                        emp.addPlace = "";
                    }
                    else
                    {
                        emp.addPlace = txtadditionalPlace.Text.Trim();
                    }



                    if (ddlTenureScale.SelectedValue == "")
                    {
                        ucTMessage.ShowMessage("Scale is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.Scale = Convert.ToInt32(ddlTenureScale.SelectedValue);
                    }
                    if (ddljobtype.SelectedValue == "")
                    {
                        ucTMessage.ShowMessage("job Type is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.jobtype = Convert.ToInt32(ddljobtype.SelectedValue);
                    }
                    //emp.Designation = txtdes.Text.Trim();
                    if (txtjoi.Text == "")
                    {
                        ucTMessage.ShowMessage("Date of joining is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.joinDate = Convert.ToDateTime(txtjoi.Text);
                    }

                    if (txtlea.Text == "")
                    {
                        emp.LeavDate = null;
                    }
                    else
                    {
                        emp.LeavDate = Convert.ToDateTime(txtlea.Text.Trim());
                    }
                    if (dllexp.SelectedValue == "0")
                    {
                       
                    }
                    else
                    {
                        emp.YOE = Convert.ToInt32(dllexp.SelectedValue);
                    }

                    emp.MonthExp = Convert.ToInt32(ddlTenureMonth.SelectedValue);

                    string fileTen = "";

                    if (TenureFileUpload.HasFile)
                    {
                        try
                        {
                            string extension = System.IO.Path.GetExtension(TenureFileUpload.FileName);
                            fileTen = TenureFileUpload.PostedFile.FileName;
                            int filesize = TenureFileUpload.PostedFile.ContentLength;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucTMessage.ShowMessage("File Size Should be less than 5MB ", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileTen.Equals(""))
                                {
                                    emp.Attachment = fileTen;
                                }

                            }
                            else
                            {

                                ucTMessage.ShowMessage("file extension must be jpg, png, gif, doc or docx", BL.Enums.MessageType.Error);
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        TenureFileUpload.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + fileTen);
                    }
                    else
                    {
                    }


                    db.TenureExperiences.InsertOnSubmit(emp);
                    db.SubmitChanges();
                    ucTMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
                }
                catch (Exception ex)
                {
                    ucTMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                    throw;
                }
            }
            else
            {
                try
                {

                    int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    // short designation = Convert.ToInt16(ddlDesignation.Text.Trim());
                    TenureExperience emp = new TenureExperience();
                    //emp = empexp.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    emp = db.TenureExperiences.Where(x => x.TenID == ID).FirstOrDefault();
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucTMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.EmpID = empsearch;
                    }
                    //if (ddlDesignation.SelectedValue == "0")
                    //{
                    //    ucExpMessage.ShowMessage("Please Select Designation", BL.Enums.MessageType.Error);
                    //    return;
                    //}
                    //else
                    //{
                    //    emp.CodeID = designation;
                    //}

                    if (ddlDivsion.SelectedValue == "0")
                    {
                        emp.BranchID = null;
                    }
                    else
                    {
                        emp.BranchID = Convert.ToInt32(ddlDivsion.SelectedValue);
                    }
                    if (ddlTenurePost.SelectedValue == "0")
                    {
                        ucTMessage.ShowMessage("Posted Held is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.Postedas = ddlTenurePost.SelectedValue;
                    }
                    if (ddlTenureAddtional.SelectedValue == "0")
                    {
                        emp.AddtionalCharge = "";
                    }
                    else
                    {
                        emp.AddtionalCharge = ddlTenureAddtional.SelectedValue;
                    }
                    if (txtaddtionalChargePost.Text == "")
                    {
                        emp.addpost = "";
                    }
                    else
                    {
                        emp.addpost = txtaddtionalChargePost.Text.Trim();
                    }
                    if (ddlAdditionPlace.SelectedValue == "0")
                    {
                        emp.addtionalChargePlace = null;
                    }
                    else
                    {
                        emp.addtionalChargePlace = Convert.ToInt32(ddlAdditionPlace.SelectedValue);
                    }

                    if (txtadditionalPlace.Text == "")
                    {
                        emp.addPlace = "";
                    }
                    else
                    {
                        emp.addPlace = txtadditionalPlace.Text.Trim();
                    }

                    if (ddlTenureScale.SelectedValue == "")
                    {
                        ucTMessage.ShowMessage("select Scale", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.Scale = Convert.ToInt32(ddlTenureScale.SelectedValue);
                    }
                    if (ddljobtype.SelectedValue == "")
                    {
                        ucTMessage.ShowMessage("select Job Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.jobtype = Convert.ToInt32(ddljobtype.SelectedValue);
                    }
                    //emp.Designation = txtdes.Text.Trim();
                    if (txtjoi.Text == "")
                    {
                        ucTMessage.ShowMessage("Date of joining is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        emp.joinDate = Convert.ToDateTime(txtjoi.Text);
                    }
                    if (txtlea.Text == "")
                    {
                        emp.LeavDate = null;
                    }
                    else
                    {
                        emp.LeavDate = Convert.ToDateTime(txtlea.Text);
                    }
                    if (dllexp.SelectedValue == "0")
                    {
                       
                    }
                    else
                    {
                        emp.YOE = Convert.ToInt32(dllexp.SelectedValue);
                    }
                    emp.MonthExp = Convert.ToInt32(ddlTenureMonth.SelectedValue);

                    string fileTe = "";

                    if (TenureFileUpload.HasFile)
                    {
                        try
                        {
                            string extension = System.IO.Path.GetExtension(TenureFileUpload.FileName);
                            fileTe = TenureFileUpload.PostedFile.FileName;
                            int filesize = TenureFileUpload.PostedFile.ContentLength;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucMessage.ShowMessage("File Size Should be less than 5MB ", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileTe.Equals(""))
                                {
                                    emp.Attachment = fileTe;
                                }

                            }
                            else
                            {

                                ucMessage.ShowMessage("file extension must be jpg, png, gif, doc or docx", BL.Enums.MessageType.Error);
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        TenureFileUpload.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + fileTe);
                    }
                    else
                    {
                    }


                    db.SubmitChanges();

                   // empexp.Update(emp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucTMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);

                }
                catch (Exception ex)
                {
                    ucTMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                    throw;
                }
            }

            BindGridtenure();
            clear_Text();
        }

        protected void EmpExpTenureClear_Clear(object sender, EventArgs e)
        {
            //this.ddlEmployeeSearch.SelectedValue = "0";
            // this.ddlDesignation.SelectedValue = "0";
            ddlTenurePost.SelectedValue = "0";
            ddlTenureAddtional.SelectedValue = "0";
            txtlea.Text = "";
            // txtdep.Text = "";
            ddljobtype.SelectedValue = "0";
            this.dllexp.SelectedValue = "0";
            txtjoi.Text = "";
            TenureExpImage.ImageUrl = "";
        }

        protected void EmpAcr_Save(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                try
                {
                    int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    tblPlEmpAcr acr = new tblPlEmpAcr();
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucAcrMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        acr.EmpID = empsearch;
                    }
                    if (dllpostingdes.SelectedValue == "0")
                    {
                        ucAcrMessage.ShowMessage("Please Select Designation", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        acr.CodeID = Convert.ToInt16(dllpostingdes.Text.Trim());
                    }
                    if (txtDateFrom.Text == "")
                    {
                        ucAcrMessage.ShowMessage("From Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        acr.DateFrom = Convert.ToDateTime(txtDateFrom.Text.Trim());
                    }
                    if (txtDateTo.Text == "")
                    {
                        acr.DateTo = null;  
                    }
                    else
                    {
                        acr.DateTo = Convert.ToDateTime(txtDateTo.Text.Trim());
                    }
                    if (ReporOff.Text == "")
                    {
                        ucAcrMessage.ShowMessage("Reporting Officer Field is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        acr.ReportingOfficer = ReporOff.Text.Trim();
                    }
                    if (ddlropdes.SelectedValue == "0")
                    {
                        ucAcrMessage.ShowMessage("Please Select report Officer Designation", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        acr.RepOffDes = ddlropdes.SelectedValue;
                    }
                    if (txtrepDate.Text == "")
                    {
                        ucAcrMessage.ShowMessage("Report Officer Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        acr.RepOffDate = Convert.ToDateTime(txtrepDate.Text.Trim());
                    }

                    if (txtcountroff.Text == "")
                    {
                        acr.CounterSignOff = null;
                    }
                    else
                    {
                        acr.CounterSignOff = txtcountroff.Text.Trim();
                    }
                    if (ddloffDes.SelectedValue == "0")
                    {
                        acr.CouOffDes = null;
                    }
                    else
                    {
                        acr.CouOffDes = ddloffDes.SelectedValue;
                    }
                    //if (txtofficerDate.Text == "")
                    //{
                    //    acr.CouOffDate = null;
                    //}
                    //else
                    //{
                    //    acr.CouOffDate = Convert.ToDateTime(txtofficerDate.Text.Trim());
                    //}

                    acr.adverrem = Convert.ToBoolean(ddlAdverse.SelectedValue);
                    if (txtacrRemaks.Text == "")
                    {
                        acr.Remarks = null;
                    }
                    else
                    {
                        acr.Remarks = txtacrRemaks.Text.Trim();
                    }

                    string fileacr = "";

                    if (AcrfileUpload.HasFile)
                    {
                        try
                        {
                            string extension = System.IO.Path.GetExtension(AcrfileUpload.FileName);
                            fileacr = AcrfileUpload.PostedFile.FileName;
                            int filesize = AcrfileUpload.PostedFile.ContentLength;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucMessage.ShowMessage("File Size Should be less than 5MB ", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileacr.Equals(""))
                                {
                                    acr.Attachment = fileacr;
                                }

                            }
                            else
                            {

                                ucMessage.ShowMessage("file extension must be jpg, png, gif, doc or docx", BL.Enums.MessageType.Error);
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        AcrfileUpload.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + fileacr);
                    }
                    else
                    {
                    }

                    empacr.Insert(acr, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucAcrMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
                    //BindGridAcrPanel();
                    clear_Text();
                }
                catch (Exception ex)
                {

                    ucAcrMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                }
            }
            else
            {
                try
                {
                    int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    tblPlEmpAcr acr = new tblPlEmpAcr();
                    acr = empacr.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucAcrMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        acr.EmpID = empsearch;
                    }
                    if (dllpostingdes.SelectedValue == "0")
                    {
                        ucAcrMessage.ShowMessage("Please Select Designation", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        acr.CodeID = Convert.ToInt16(dllpostingdes.Text.Trim());
                    }
                    if (txtDateFrom.Text == "")
                    {
                        ucAcrMessage.ShowMessage("Date From is Required", BL.Enums.MessageType.Error);
                    }
                    else
                    {
                        acr.DateFrom = Convert.ToDateTime(txtDateFrom.Text.Trim());
                    }
                    if (txtDateTo.Text == "")
                    {
                        ucAcrMessage.ShowMessage("Date To is Required", BL.Enums.MessageType.Error);
                    }
                    else
                    {
                        acr.DateTo = Convert.ToDateTime(txtDateTo.Text.Trim());
                    }
                    if (ReporOff.Text == "")
                    {
                       
                    }
                    else
                    {
                        acr.ReportingOfficer = ReporOff.Text.Trim();
                    }
                    if (ddlropdes.SelectedValue == "0")
                    {
                       
                    }
                    else
                    {
                        acr.RepOffDes = ddlropdes.SelectedValue;
                    }
                    if (txtrepDate.Text == "")
                    {
                        
                    }
                    else
                    {
                        acr.RepOffDate = Convert.ToDateTime(txtrepDate.Text.Trim());
                    }
                    if (txtcountroff.Text == "")
                    {
                    }
                    else
                    {
                        acr.CounterSignOff = txtcountroff.Text.Trim();
                    }
                    if (ddloffDes.SelectedValue == "0")
                    {
                       
                    }
                    else
                    {
                        acr.CouOffDes = ddloffDes.SelectedValue;
                    }
                    //if (txtofficerDate.Text == "")
                    //{
                        
                    //}
                    //else
                    //{
                    //    acr.CouOffDate = Convert.ToDateTime(txtofficerDate.Text.Trim());
                    //}
                    acr.adverrem = Convert.ToBoolean(ddlAdverse.SelectedValue);
                    if (txtacrRemaks.Text == "")
                    {
                        acr.Remarks = null;
                    }
                    else
                    {
                        acr.Remarks = txtacrRemaks.Text.Trim();
                    }

                    string fileAc = "";

                    if (AcrfileUpload.HasFile)
                    {
                        try
                        {
                            string extension = System.IO.Path.GetExtension(AcrfileUpload.FileName);
                            fileAc = AcrfileUpload.PostedFile.FileName;
                            int filesize = AcrfileUpload.PostedFile.ContentLength;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucMessage.ShowMessage("File Size Should be less than 5MB ", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileAc.Equals(""))
                                {
                                    acr.Attachment = fileAc;
                                }

                            }
                            else
                            {

                                ucMessage.ShowMessage("file extension must be jpg, png, gif, doc or docx", BL.Enums.MessageType.Error);
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        AcrfileUpload.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + fileAc);
                    }
                    else
                    {
                    }

                    empacr.Update(acr, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucAcrMessage.ShowMessage("Updated  Successfully", BL.Enums.MessageType.Info);
                    
                }
                catch (Exception ex)
                {

                    ucAcrMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                }
            }
            BindGridAcrPanel();
            clear_Text();
        }

        protected void EmpAcrClear_Clear(object sender, EventArgs e)
        {
            //this.ddlEmployeeAcrRepPanel.SelectedValue = "0";
            this.dllpostingdes.SelectedValue = "0";
            //duration.Text = "";
            ddloffDes.SelectedValue = "0";
            ddlropdes.SelectedValue = "0";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            //txtofficerDate.Text = "";
            txtrepDate.Text = "";
            txtcountroff.Text = "";
            ReporOff.Text = "";
            txtacrRemaks.Text = "";
            AcrImage.ImageUrl = "";
        }

        protected void btnEnq_Save(object sender, EventArgs e)
        {
            int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            if (ID == 0)
            {
                try
                {
                    
                    tblPlEmpEnq enq = new tblPlEmpEnq();
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucEnqMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enq.EmpID = empsearch;
                    }

                    if (txtEnqtitle.Text == "")
                    {
                        ucEnqMessage.ShowMessage("Please Enquiry Title is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enq.EnqTitle = txtEnqtitle.Text.Trim();
                    }

                    if (ddlEnqtypes.SelectedValue == "0")
                    {
                        ucEnqMessage.ShowMessage("Please Select Enquiry/Audit", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enq.EnquiryAud = ddlEnqtypes.SelectedValue;
                    }


                    if (EnqDate.Text == "")
                    {
                        ucEnqMessage.ShowMessage("Date Field is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enq.EnquiryDate = Convert.ToDateTime(EnqDate.Text);
                    }
                    if (IssuAuthori.Text == "")
                    {
                        ucEnqMessage.ShowMessage("Issuing Authority Field is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enq.IssuAut = IssuAuthori.Text.Trim();
                    }
                    if (ddlStatus.SelectedValue == "")
                    {
                        ucEnqMessage.ShowMessage("Status Field is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enq.Statuss = ddlStatus.Text.Trim();
                    }
                    if (txtarearemaks.Text == "")
                    {
                        enq.Remarks = null;
                    }
                    else
                    {
                        enq.Remarks = txtarearemaks.Text;
                    }

                    string enqfile = null;

                    if (enqfileupload.HasFile)
                    {

                        enqfile = enqfileupload.PostedFile.FileName;
                        int filesize = enqfileupload.PostedFile.ContentLength;
                        string ext = System.IO.Path.GetExtension(enqfile);

                        if (ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".png") || ext.ToLower().Equals(".gif"))
                        {
                            if (filesize > 5 * 1024 * 1024)
                            {
                                ucEnqMessage.ShowMessage("File Size Must be Less Than 5MB", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                enq.Attachment = enqfile;
                            }
                        }
                        else
                        {
                            ucEnqMessage.ShowMessage("File Extension Should be jpg,png or gif", BL.Enums.MessageType.Error);
                            return;
                        }
                        enqfileupload.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + enqfile);
                    }
                    else
                    {
                    }


                    empenq.Insert(enq, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucEnqMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
                }
                catch (Exception ex)
                {
                    ucEnqMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                }
            }
            else
            {
                try
                {
                    //int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    tblPlEmpEnq enqupd = new tblPlEmpEnq();
                    enqupd = db.tblPlEmpEnqs.Where(x => x.EmpAcrID == ID).FirstOrDefault();
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucEnqMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enqupd.EmpID = empsearch;
                    }

                    if (txtEnqtitle.Text == "")
                    {
                        ucEnqMessage.ShowMessage("Please Enquiry Title is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enqupd.EnqTitle = txtEnqtitle.Text.Trim();
                    }

                    if (ddlEnqtypes.SelectedValue == "0")
                    {
                        ucEnqMessage.ShowMessage("Please Select Enquiry/Audit", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enqupd.EnquiryAud = ddlEnqtypes.SelectedValue;
                    }


                    if (EnqDate.Text == "")
                    {
                        ucEnqMessage.ShowMessage("Date Field is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enqupd.EnquiryDate = Convert.ToDateTime(EnqDate.Text);
                    }
                    if (IssuAuthori.Text == "")
                    {
                        ucEnqMessage.ShowMessage("Issuing Authority Field is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enqupd.IssuAut = IssuAuthori.Text.Trim();
                    }
                    if (ddlStatus.SelectedValue == "")
                    {
                        ucEnqMessage.ShowMessage("Status Field is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        enqupd.Statuss = ddlStatus.Text.Trim();
                    }
                    if (txtarearemaks.Text == "")
                    {
                        enqupd.Remarks = null;
                    }
                    else
                    {
                        enqupd.Remarks = txtarearemaks.Text;
                    }

                    string enqfile = null;

                    if (enqfileupload.HasFile)
                    {

                        enqfile = enqfileupload.PostedFile.FileName;
                        int filesize = enqfileupload.PostedFile.ContentLength;
                        string ext = System.IO.Path.GetExtension(enqfile);

                        if (ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".png") || ext.ToLower().Equals(".gif"))
                        {
                            if (filesize > 5 * 1024 * 1024)
                            {
                                ucEnqMessage.ShowMessage("File Size Must be Less Than 5MB", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                enqupd.Attachment = enqfile;
                            }
                        }
                        else
                        {
                            ucEnqMessage.ShowMessage("File Extension Should be jpg,png or gif", BL.Enums.MessageType.Error);
                            return;
                        }

                        enqfileupload.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + enqfile);
                    }
                    else
                    {
                    }
                    db.SubmitChanges();
                    ucEnqMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
                    
                }
                catch (Exception ex)
                {
                    ucEnqMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                }
            }
            BindGridEnqPanel();
            FillDropdowntitleEnq(empsearch);
            clear_Text();
        }
        protected void UdapteEnq_Click(object sender, EventArgs e)
        {

            if (ID == 0)
            {
                int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                tblPlEmEnqDetail enq = new tblPlEmEnqDetail();
                if (ddlEmployeeSearch.SelectedValue == "0")
                {
                    ucEnqMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    enq.EmpId = empID;
                }
                if (ddlenquirytitle.SelectedValue == "0")
                {
                    ucEnqMessage.ShowMessage("Please Select Enquiry Title", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    enq.OngionEnq = ddlenquirytitle.SelectedValue;
                }
                if (txtupdatedate.Text == "")
                {
                    ucEnqMessage.ShowMessage("Update Date is Required", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    enq.UpdateDate = Convert.ToDateTime(txtupdatedate.Text.Trim());
                }

                enq.updateStatus = ddlUpdaStatus.SelectedValue;
                if (txtUpdateRemarks.Text == "")
                {
                    enq.updateremarks = null;
                }
                else
                {
                    enq.updateremarks = txtUpdateRemarks.Text.Trim();
                }

                db.tblPlEmEnqDetails.InsertOnSubmit(enq);
                db.SubmitChanges();
                ucEnqMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                tblPlEmEnqDetail enqupdate = new tblPlEmEnqDetail();
                enqupdate = db.tblPlEmEnqDetails.Where(x => x.EnqDelID == ID).FirstOrDefault();
                if (ddlEmployeeSearch.SelectedValue == "0")
                {
                    ucEnqMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    enqupdate.EmpId = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                }
                if (ddlenquirytitle.SelectedValue == "0")
                {
                    ucEnqMessage.ShowMessage("Please Select Enquiry Title", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    enqupdate.OngionEnq = ddlenquirytitle.SelectedValue;
                }
                if (txtupdatedate.Text == "")
                {
                    ucEnqMessage.ShowMessage("Update Date is Required", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    enqupdate.UpdateDate = Convert.ToDateTime(txtupdatedate.Text.Trim());
                }

                enqupdate.updateStatus = ddlUpdaStatus.SelectedValue;
                if (txtUpdateRemarks.Text == "")
                {
                    enqupdate.updateremarks = null;
                }
                else
                {
                    enqupdate.updateremarks = txtUpdateRemarks.Text.Trim();
                }
                
                db.SubmitChanges();
                ucEnqMessage.ShowMessage("Update Successfully", BL.Enums.MessageType.Info);
            }
            BindGridUpdateEnq();
            clear_Text();
            Clear_UpdateText();
        }
        protected void EmpEnqClear_Clear(object sender, EventArgs e)
        {
            //this.ddlEmployeeSearch.SelectedValue = "0";
            this.ddlEnqtypes.SelectedValue = "0";
            EnqDate.Text = "";
            IssuAuthori.Text = "";
            this.ddlStatus.SelectedValue = "0";
            txtarearemaks.Text = "";
        }
        protected void btnLiti_Save(object sender, EventArgs e)
        {
            int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            if (LitiID == 0)
            {
                try
                {
                    
                    tblPlEmpLitigation liti = new tblPlEmpLitigation();

                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucLitiMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liti.EmpID = empsearch;
                    }
                    if (txtLitiTitle.Text == "")
                    {
                        ucLitiMessage.ShowMessage("Litigation Title is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liti.LitiTitle = txtLitiTitle.Text.Trim();
                    }
                    if (ddlLitigation.SelectedValue == "0")
                    {
                        ucLitiMessage.ShowMessage("Please Select Litigation Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liti.LitiID = Convert.ToInt32(ddlLitigation.SelectedValue);
                    }
                    if (litiDate.Text == "")
                    {
                        ucLitiMessage.ShowMessage("Litigation Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liti.LitiDate = Convert.ToDateTime(litiDate.Text);
                    }
                    if (authorityforum.Text == "")
                    {
                        ucLitiMessage.ShowMessage("Authority / forum is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liti.Authority = authorityforum.Text.Trim();
                    }
                    if (txtAuthorityTitle.Text == "")
                    {
                        ucLitiMessage.ShowMessage("Authority Title is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liti.AuthorityTitle = txtAuthorityTitle.Text.Trim();
                    }
                    if (txtareaEnqremarks.Text == "")
                    {
                        liti.Remarks = null;
                    }
                    else
                    {
                        liti.Remarks = txtareaEnqremarks.Text;
                    }
                    if (status.SelectedValue == "")
                    {
                        ucLitiMessage.ShowMessage("Please Select Status", BL.Enums.MessageType.Error);
                    }
                    else
                    {
                        liti.Status = status.SelectedValue;
                    }
                    string litiFile = null;
                    if (litifileUpload.HasFile)
                    {
                        litiFile = litifileUpload.PostedFile.FileName;
                        int size = litifileUpload.PostedFile.ContentLength;
                        string ext = System.IO.Path.GetExtension(litiFile);

                        if (ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".png") || ext.ToLower().Equals(".gif"))
                        {
                            if (size > 5 * 1024 * 1024)
                            {
                                ucLitiMessage.ShowMessage("File Size Should be Less than 5MB", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                liti.Attachment = litiFile;
                            }
                        }
                        else
                        {
                            ucLitiMessage.ShowMessage("File Extension Must be jpg, png or gif", BL.Enums.MessageType.Error);
                            return;
                        }
                        litifileUpload.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + litiFile);
                    }
                    else
                    {
                    }


                    emplit.Insert(liti, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    ucLitiMessage.ShowMessage("Save Succesfully", BL.Enums.MessageType.Info);
                    BindGridLitiPanel();
                    clear_Text();

                }
                catch (Exception ex)
                {
                    ucLitiMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                }
            }
            else
            {
                try
                {
                    tblPlEmpLitigation liga = db.tblPlEmpLitigations.Where(x => x.EmpAcrID == LitiID).FirstOrDefault();
                    //int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    tblPlEmpLitigation liti = new tblPlEmpLitigation();

                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucLitiMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liga.EmpID = empsearch;
                    }
                    if (txtLitiTitle.Text == "")
                    {
                        ucLitiMessage.ShowMessage("Litigation Title is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liga.LitiTitle = txtLitiTitle.Text.Trim();
                    }
                    if (ddlLitigation.SelectedValue == "0")
                    {
                        ucLitiMessage.ShowMessage("Please Select Litigation Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liga.LitiID = Convert.ToInt32(ddlLitigation.SelectedValue);
                    }
                    if (litiDate.Text == "")
                    {
                        ucLitiMessage.ShowMessage("Litigation Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liga.LitiDate = Convert.ToDateTime(litiDate.Text);
                    }
                    if (authorityforum.Text == "")
                    {
                        ucLitiMessage.ShowMessage("Authority / forum is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liga.Authority = authorityforum.Text.Trim();
                    }
                    if (txtAuthorityTitle.Text == "")
                    {
                        ucLitiMessage.ShowMessage("Authority Title is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        liga.AuthorityTitle = txtAuthorityTitle.Text.Trim();
                    }
                    if (txtareaEnqremarks.Text == "")
                    {
                        liga.Remarks = null;
                    }
                    else
                    {
                        liga.Remarks = txtareaEnqremarks.Text;
                    }
                    if (status.SelectedValue == "")
                    {
                        ucLitiMessage.ShowMessage("Please Select Status", BL.Enums.MessageType.Error);
                    }
                    else
                    {
                        liga.Status = status.SelectedValue;
                    }
                    string litiFile = null;
                    if (litifileUpload.HasFile)
                    {
                        litiFile = litifileUpload.PostedFile.FileName;
                        int size = litifileUpload.PostedFile.ContentLength;
                        string ext = System.IO.Path.GetExtension(litiFile);

                        if (ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".png") || ext.ToLower().Equals(".gif"))
                        {
                            if (size > 5 * 1024 * 1024)
                            {
                                ucLitiMessage.ShowMessage("File Size Should be Less than 5MB", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                liga.Attachment = litiFile;
                            }
                        }
                        else
                        {
                            ucLitiMessage.ShowMessage("File Extension Must be jpg, png or gif", BL.Enums.MessageType.Error);
                            return;
                        }
                        litifileUpload.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + litiFile);
                    }
                    else
                    {
                    }

                    db.SubmitChanges();
                    ucLitiMessage.ShowMessage("Updated Succesfully", BL.Enums.MessageType.Info);
                    BindGridLitiPanel();
                    clear_Text();

                }
                catch (Exception ex)
                {
                    ucLitiMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                }
            }

            FillDropdowntitleLiti(empsearch);
            
        }

        protected void updateliti_click(object sender, EventArgs e)
        {
            if (LitiID == 0)
            {
                int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                tblPlEmpLitigationDetail litidetail = new tblPlEmpLitigationDetail();
                if (ddlEmployeeSearch.SelectedValue == "0")
                {
                    ucLitiMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    litidetail.EmpID = empsearch;
                }
                if (ddlLitigationUpd.SelectedValue == "0")
                {
                    ucLitiMessage.ShowMessage("Please Select Title", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    litidetail.LitiTitle = ddlLitigationUpd.SelectedValue;
                }
                if (txtFinalDate.Text == "")
                {
                    ucLitiMessage.ShowMessage("Proceeding Select Date", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    litidetail.FinalDate = Convert.ToDateTime(txtFinalDate.Text);
                }
                if (ddlStatLitiup.SelectedValue == "0")
                {
                    ucLitiMessage.ShowMessage("Please Update Status", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    litidetail.statuss = ddlStatLitiup.SelectedValue;
                }
                if (txtFinaljud.Text == "")
                {
                    litidetail.FinalJud = null;
                }
                else
                {
                    litidetail.FinalJud = txtFinaljud.Text.Trim();
                }

                db.tblPlEmpLitigationDetails.InsertOnSubmit(litidetail);
                db.SubmitChanges();
                ucLitiMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                int empsearch = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                tblPlEmpLitigationDetail litidet = db.tblPlEmpLitigationDetails.Where(x => x.litiDeID == LitiID).FirstOrDefault();
                if (ddlEmployeeSearch.SelectedValue == "0")
                {
                    ucLitiMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    litidet.EmpID = empsearch;
                }
                if (ddlLitigationUpd.SelectedValue == "0")
                {
                    ucLitiMessage.ShowMessage("Please Select Title", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    litidet.LitiTitle = ddlLitigationUpd.SelectedValue;
                }
                if (txtFinalDate.Text == "")
                {
                    ucLitiMessage.ShowMessage("Proceeding Date is Required", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    litidet.FinalDate = Convert.ToDateTime(txtFinalDate.Text);
                }
                if (ddlStatLitiup.SelectedValue == "0")
                {
                    ucLitiMessage.ShowMessage("Please Update Status", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    litidet.statuss = ddlStatLitiup.SelectedValue;
                }
                if (txtFinaljud.Text == "")
                {
                    litidet.FinalJud = null;
                }
                else
                {
                    litidet.FinalJud = txtFinaljud.Text.Trim();
                }
                db.SubmitChanges();
                ucLitiMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
            }
            BindGridLitiUpdate();
            clear_Text();
            Clear_UpdateText();
        }
        protected void EmpLitiClear_Clear(object sender, EventArgs e)
        {
            //this.ddlEmployeeLitiPanel.SelectedValue = "0";
            //lititype.Text = "";
            ddlLitigation.SelectedValue = "0";
            litiDate.Text = "";
            authorityforum.Text = "";
            txtareaEnqremarks.Text = "";
        }
        protected void btnper_save(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                try
                {
                    tblPlEmpPermotion per = new tblPlEmpPermotion();
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucCpfMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        per.empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    }
                    if (ddlPerDes.SelectedValue == "0")
                    {
                        ucCpfMessage.ShowMessage("Select Designation", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        per.DesID = Convert.ToInt32(ddlPerDes.SelectedValue);
                    }
                    if (ddlperscal.SelectedValue == "0")
                    {
                        ucCpfMessage.ShowMessage("Select Scale", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        per.scale = Convert.ToInt32(ddlperscal.SelectedValue);
                    }
                    if (txtperfrom.Text == "")
                    {
                        ucCpfMessage.ShowMessage("From Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        per.FromDate = Convert.ToDateTime(txtperfrom.Text);
                    }
                    if (txtPerTo.Text == "")
                    {
                        per.todate = null;
                    }
                    else
                    {
                        per.todate = Convert.ToDateTime(txtPerTo.Text);
                    }

                    if (ddlpertypes.SelectedValue == "0")
                    {
                        ucCpfMessage.ShowMessage("Please Select Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        per.pertype = ddlpertypes.SelectedValue;
                    }
                    string perFile = null;
                    if (promotionAttach.HasFile)
                    {
                        perFile = promotionAttach.PostedFile.FileName;
                        int size = promotionAttach.PostedFile.ContentLength;
                        string ext = System.IO.Path.GetExtension(perFile);

                        if (ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".png") || ext.ToLower().Equals(".gif"))
                        {
                            if (size > 5 * 1024 * 1024)
                            {
                                ucLitiMessage.ShowMessage("File Size Should be Less than 5MB", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                per.Attachement = perFile;
                            }
                        }
                        else
                        {
                            ucCpfMessage.ShowMessage("File Extension Must be jpg, png or gif", BL.Enums.MessageType.Error);
                            return;
                        }
                        promotionAttach.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + perFile);
                    }
                    else
                    {
                    }
                    db.tblPlEmpPermotions.InsertOnSubmit(per);
                    db.SubmitChanges();
                    ucCpfMessage.ShowMessage("Save Succesfully", BL.Enums.MessageType.Info);
                    BindGridPerPanel();
                    clear_Text();
                    
                }
                catch (Exception ex)
                {
                    ucCpfMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                }
            }
            else
            {
                try
                {
                    tblPlEmpPermotion permotion = new tblPlEmpPermotion();
                    permotion = db.tblPlEmpPermotions.Where(x => x.PerID == ID).FirstOrDefault();
                    if (ddlEmployeeSearch.SelectedValue == "0")
                    {
                        ucCpfMessage.ShowMessage("Please Select Designation", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        permotion.empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                    }
                    if (ddlPerDes.SelectedValue == "0")
                    {
                        ucCpfMessage.ShowMessage("Select Designation", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        permotion.DesID = Convert.ToInt32(ddlPerDes.SelectedValue);
                    }
                    if (ddlperscal.SelectedValue == "0")
                    {
                        ucCpfMessage.ShowMessage("Select Scale", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        permotion.scale = Convert.ToInt32(ddlperscal.SelectedValue);
                    }
                    if (txtperfrom.Text == "")
                    {
                        ucCpfMessage.ShowMessage("From Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        permotion.FromDate = Convert.ToDateTime(txtperfrom.Text);
                    }
                    if (txtPerTo.Text == "")
                    {
                        permotion.todate = null;
                    }
                    else
                    {
                        permotion.todate = Convert.ToDateTime(txtPerTo.Text);
                    }
                    if (ddlpertypes.SelectedValue == "0")
                    {
                        ucCpfMessage.ShowMessage("Please Select Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        permotion.pertype = ddlpertypes.SelectedValue;
                    }
                    string perFile = null;
                    if (promotionAttach.HasFile)
                    {
                        perFile = promotionAttach.PostedFile.FileName;
                        int size = promotionAttach.PostedFile.ContentLength;
                        string ext = System.IO.Path.GetExtension(perFile);

                        if (ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".png") || ext.ToLower().Equals(".gif"))
                        {
                            if (size > 5 * 1024 * 1024)
                            {
                                ucLitiMessage.ShowMessage("File Size Should be Less than 5MB", BL.Enums.MessageType.Error);
                                return;
                            }
                            else
                            {
                                permotion.Attachement = perFile;
                            }
                        }
                        else
                        {
                            ucLitiMessage.ShowMessage("File Extension Must be jpg, png or gif", BL.Enums.MessageType.Error);
                            return;
                        }
                        promotionAttach.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + perFile);
                    }
                    else
                    {
                    }

                    db.SubmitChanges();
                    ucCpfMessage.ShowMessage("Updated Succesfully", BL.Enums.MessageType.Info);
                    
                }
                catch (Exception ex)
                {
                    ucCpfMessage.ShowMessage(ex.Message.ToString(), BL.Enums.MessageType.Error);
                }
                BindGridPerPanel();
                clear_Text();
            }
        }

        protected void btnPer_Clear(object sender, EventArgs e)
        {
            this.ddlEmployeeSearch.SelectedValue = "0";
            this.ddlPerDes.SelectedValue = "0";
            this.ddlperscal.SelectedValue = "0";
            this.txtperfrom.Text = "";
            this.txtPerTo.Text = "";
            this.ddlpertypes.SelectedValue = "0";
            this.tblPRo.ImageUrl = "";
        }

        protected void grdAcrEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdAcr.SelectedValue);

            tblPlEmpAcr tblempacr = new tblPlEmpAcr();
            tblempacr = empacr.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

           
            if (tblempacr.CodeID == null)
            {
                this.dllpostingdes.SelectedValue = "";
            }
            else
            {
                this.dllpostingdes.SelectedValue = tblempacr.CodeID.ToString();
            }
            this.txtDateFrom.Text = tblempacr.DateFrom.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            this.txtDateTo.Text = tblempacr.DateTo.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            this.ReporOff.Text = tblempacr.ReportingOfficer.ToString();
            this.ddlropdes.SelectedValue = tblempacr.RepOffDes;
            this.txtrepDate.Text = tblempacr.RepOffDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            this.txtcountroff.Text = tblempacr.CounterSignOff;
            //this.ddloffDes.SelectedValue = tblempacr.CouOffDes.ToString();
           
            ddlAdverse.SelectedValue = tblempacr.adverrem.ToString();
            //if (tblempacr.CouOffDate == null)
            //{
            //    this.txtofficerDate.Text = "";
            //}
            //else
            //{
            //    this.txtofficerDate.Text = tblempacr.CouOffDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            //}
            if (string.IsNullOrEmpty(tblempacr.CouOffDes))
            {
                ddloffDes.SelectedValue = "0";
            }
            else
            {
                ddloffDes.SelectedValue = tblempacr.CouOffDes.ToString();
            }
            if (tblempacr.Attachment == "" || tblempacr.Attachment == null)
            {
                AcrImage.ImageUrl = "";
            }
            else
            {
                this.AcrImage.ImageUrl = "~/Attachments/" + tblempacr.Attachment.ToString();
            }
            
            this.txtacrRemaks.Text = tblempacr.Remarks.ToString();
        }
        protected void grdAcrEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAcr.PageIndex = e.NewPageIndex;
            BindGridAcrPanel();
        }
        protected void grdAcr_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType ==  DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[5].Text = Convert.ToDateTime(e.Row.Cells[5].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                //e.Row.Cells[6].Text = Convert.ToDateTime(e.Row.Cells[6].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }
        protected void grdEnqEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdEnq.SelectedValue);
            tblPlEmpEnq enq = db.tblPlEmpEnqs.Where(x => x.EmpAcrID == ID).FirstOrDefault();
            txtEnqtitle.Text = enq.EnqTitle;
            ddlEnqtypes.SelectedValue = enq.EnquiryAud;
            EnqDate.Text = enq.EnquiryDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            IssuAuthori.Text = enq.IssuAut;
            ddlStatus.SelectedValue = enq.Statuss;
            txtarearemaks.Text = enq.Remarks;
            if (enq.Attachment == "" || enq.Attachment == null)
            {
                EnqImage.ImageUrl = "";
            }
            else
            {
                EnqImage.ImageUrl = "~/Attachments/" + enq.Attachment;
            }
            
        }
        protected void grdEnqEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEnq.PageIndex = e.NewPageIndex;
            BindGridEnqPanel();
        }
        protected void grdenq_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                
            }
        }
        protected void grdEnqUp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdEnqUpdate.SelectedValue);
            tblPlEmEnqDetail enqup = db.tblPlEmEnqDetails.Where(x => x.EnqDelID == ID).FirstOrDefault();
            ddlenquirytitle.SelectedValue = enqup.OngionEnq;
            ddlUpdaStatus.SelectedValue = enqup.updateStatus;
            txtupdatedate.Text = enqup.UpdateDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            txtUpdateRemarks.Text = enqup.updateremarks;
        }
        protected void grdEnqup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEnqUpdate.PageIndex = e.NewPageIndex;
            BindGridUpdateEnq();
        }

        protected void grdenqup_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }
        protected void grdLitiEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            LitiID = Convert.ToInt32(grdLiti.SelectedValue);

            tblPlEmpLitigation tblempliti = new tblPlEmpLitigation();
            tblempliti = emplit.GetByID(LitiID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            txtLitiTitle.Text = tblempliti.LitiTitle;
            ddlLitigation.SelectedValue = tblempliti.LitiID.ToString();
            litiDate.Text = tblempliti.LitiDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            authorityforum.Text = tblempliti.Authority;
            txtAuthorityTitle.Text = tblempliti.AuthorityTitle;
            status.SelectedValue = tblempliti.Status.ToString();
            txtareaEnqremarks.Text = tblempliti.Remarks;
            if (tblempliti.Attachment == "" || tblempliti.Attachment == null)
            {
                litiImage.ImageUrl = "";
            }
            else
            {
                litiImage.ImageUrl = "~/Attachments/" + tblempliti.Attachment;
            }
            

            //this.ddlLitigation.SelectedValue = tblempliti.LitiID.ToString();
            //this.litiDate.Text = tblempliti.LitiDate.ToString();
            //this.authorityforum.Text = tblempliti.Authority.ToString();
            //this.txtareaEnqremarks.Text = tblempliti.Remarks.ToString();
        }
        protected void grdLitiEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLiti.PageIndex = e.NewPageIndex;
            BindGridLitiPanel();
        }
        protected void grdLiti_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void grdPerEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdPermotion.SelectedValue);
            tblPlEmpPermotion permo = db.tblPlEmpPermotions.Where(x => x.PerID == ID).FirstOrDefault();
            ddlPerDes.SelectedValue = permo.DesID.ToString();
            ddlperscal.SelectedValue = permo.scale.ToString();
            txtperfrom.Text = permo.FromDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            
            if (permo.todate == null)
            {
                txtPerTo.Text = "";
            }
            else
            {
                txtPerTo.Text = permo.todate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            ddlpertypes.SelectedValue = permo.pertype;
            if (permo.Attachement == "" || permo.Attachement == null)
            {
                tblPRo.ImageUrl = "";
            }
            else
            {
                tblPRo.ImageUrl = "~/Attachments/" + permo.Attachement;
            }
        }

        protected void grdPerEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPermotion.PageIndex = e.NewPageIndex;
            BindGridPerPanel();
        }
        protected void grdPer_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int brID = Convert.ToInt32(grdPermotion.DataKeys[e.Row.RowIndex].Values[0].ToString());
                DateTime? Ord = db.tblPlEmpPermotions.Where(x => x.PerID == brID).FirstOrDefault().todate;

                if (Ord == null)
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                    e.Row.Cells[4].Text = "";
                }
                else
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                    e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                }


                
                
                
               


            }
        }
        protected void grdEduEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
             ID = Convert.ToInt32(grdEducation.SelectedValue);

            tblPlEmpEdu tblempedu = new tblPlEmpEdu();
            tblempedu = empedu.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            this.txtEduDegTtl.Text = tblempedu.DegreeTitle.ToString();
            this.ddluniversity.SelectedValue = tblempedu.UniversityBoard.ToString();
            this.ddlDegreeType.SelectedValue = tblempedu.Degreetype;
            this.txtPercentage.Text = tblempedu.Percente;
            this.ddlCity.SelectedValue = tblempedu.CityID.ToString();
            this.ddlYear.SelectedValue = tblempedu.Year.ToString();
            //this.txtEduCity.Text = tblempedu.City.ToString();
            //this.txtEduYear.Text = tblempedu.Year.ToString();
            ddlEduVerified.SelectedValue = tblempedu.Verified.ToString();
            if (tblempedu.filePath == "" || tblempedu.filePath == null)
            {
                ImageEdu.ImageUrl = "";
            }
            else
            {
                this.ImageEdu.ImageUrl = "~/Attachments/" + tblempedu.filePath.ToString();
            }
            
        }

        protected void grdEduEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEducation.PageIndex = e.NewPageIndex;
            BindGridEduPanel();
        }
        protected void grdEdu_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[6].Text.Equals("True"))
                {
                    e.Row.Cells[6].Text = "Yes";
                }
                else
                {
                    e.Row.Cells[6].Text = "No";
                }
            }
        }

        protected void grdExpEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdExperience.SelectedValue);

            tblPlEmpExp tblempexp = new tblPlEmpExp();
            tblempexp = empexp.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (string.IsNullOrEmpty(tblempexp.Postedas))
            {
                txtpriorExp.Text = "";
            }
            else
            {
                this.txtpriorExp.Text = tblempexp.Postedas.ToString();
            }

            // this.txtExpAppAs.Text = tblempexp.Appointedas.ToString();
            // this.txtaddtional.Text = tblempexp.Department.ToString();
            if (string.IsNullOrEmpty(tblempexp.Department))
            {
                txtaddtionalChar.Text = "";
            }
            else
            {
                txtaddtionalChar.Text = tblempexp.Department.ToString();
            }
            txtOrganization.Text = tblempexp.OrgName.ToString();
            
            // this.ddlDesignation.SelectedValue = tblempexp.CodeID.ToString();
            this.ddlScale.SelectedValue = tblempexp.Scale.ToString();
            this.txtExpDOJ.Text = tblempexp.joinDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            ddlMonthExp.SelectedValue = tblempexp.monthExp.ToString();
            if (tblempexp.leavDate == null)
            {
                txtLeaving.Text = "";
            }
            else
            {
                txtLeaving.Text = tblempexp.leavDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            if (tblempexp.Attachment == "" || tblempexp.Attachment == null)
            {
                ImageExp.ImageUrl = "";
            }
            else
            {
                this.ImageExp.ImageUrl = "~/ExpAttachments/" + tblempexp.Attachment.ToString();
            }
            
            this.ddlDomicile.SelectedValue = tblempexp.YOE.ToString();
            
            ddlExpSector.SelectedValue = tblempexp.Sector.ToString();
            
        }

        protected void grdExpEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdExperience.PageIndex = e.NewPageIndex;
            BindGridExpPanel();
        }
        protected void grdexp_rowbound(object sender,GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void grdTenEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(gedTenure.SelectedValue);

            TenureExperience tblempexp = new TenureExperience();
            tblempexp = db.TenureExperiences.Where(x => x.TenID == ID).FirstOrDefault();

            this.ddlTenurePost.SelectedValue = tblempexp.Postedas.ToString();
            //this.txtAddio.Text = tblempexp.AddtionalCharge.ToString();
            if (string.IsNullOrEmpty(tblempexp.AddtionalCharge))
            {
                ddlTenureAddtional.SelectedValue = "0";
            }
            else
            {
                ddlTenureAddtional.SelectedValue = tblempexp.AddtionalCharge.ToString();
            }
            if (tblempexp.LeavDate == null)
            {
                txtlea.Text = "";
            }
            else
            {
                txtlea.Text = tblempexp.LeavDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
           
            // this.ddlDesignation.SelectedValue = tblempexp.CodeID.ToString();
            this.ddljobtype.SelectedValue = tblempexp.jobtype.ToString();
            this.ddlTenureScale.SelectedValue = tblempexp.Scale.ToString();
            this.txtjoi.Text = tblempexp.joinDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            this.dllexp.SelectedValue = tblempexp.YOE.ToString();
            this.ddlTenureMonth.SelectedValue = tblempexp.MonthExp.ToString();
            if (tblempexp.Attachment == "" || tblempexp.Attachment == null)
            {
                TenureExpImage.ImageUrl = "";
            }
            else
            {
                this.TenureExpImage.ImageUrl = "~/Attachments/" + tblempexp.Attachment.ToString();
            }

            if (tblempexp.BranchID == null)
            {
                ddlDivsion.SelectedValue = "0";
            }
            else
            {
                ddlDivsion.SelectedValue = tblempexp.BranchID.ToString();
            }

            if (tblempexp.addtionalChargePlace == null)
            {
                ddlAdditionPlace.SelectedValue = "0";
            }
            else
            {
                ddlAdditionPlace.SelectedValue = tblempexp.addtionalChargePlace.ToString();
            }
            if (tblempexp.addpost == null || tblempexp.addpost == "" || tblempexp.addpost == "0")
            {
                txtaddtionalChargePost.Text = "";
            }
            else
            {
                txtaddtionalChargePost.Text = tblempexp.addpost.ToString();
            }
            if (tblempexp.addPlace == null || tblempexp.addPlace == "" || tblempexp.addPlace == "0")
            {
                txtadditionalPlace.Text = "";
            }
            else
            {
                txtadditionalPlace.Text = tblempexp.addPlace.ToString();
            }

        }

        protected void grdTenEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gedTenure.PageIndex = e.NewPageIndex;
            BindGridtenure();

        }
        protected void grdten_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int brID = Convert.ToInt32(gedTenure.DataKeys[e.Row.RowIndex].Values[0].ToString());
                DateTime? dat = db.TenureExperiences.Where(x => x.TenID == brID).FirstOrDefault().LeavDate;
                if (dat == null)
                {
                    e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                    e.Row.Cells[3].Text = "Till Date";
                }
                else
                {
                    e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                }
                
            }
        }
        protected void Titles_SelectedIndexChanged(object sender, EventArgs e)
        {
            int empId = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            ddlenquirytitle.Items.Clear();
            ddlenquirytitle.Items.Insert(0, new ListItem("Please Select Title", "0"));

            this.ddlenquirytitle.DataTextField = "EnqTitle";
            this.ddlenquirytitle.DataValueField = "EnqTitle";
            this.ddlenquirytitle.DataSource = db.tblPlEmpEnqs.Where(x => x.EmpID == empId).ToList();
            this.ddlenquirytitle.DataBind();


            ddlLitigationUpd.Items.Clear();
            ddlLitigationUpd.Items.Insert(0, new ListItem("Select Title", "0"));
            ddlLitigationUpd.DataTextField = "LitiTitle";
            ddlLitigationUpd.DataValueField = "LitiTitle";
            ddlLitigationUpd.DataSource = db.tblPlEmpLitigations.Where(x => x.EmpID == empId).ToList();
            ddlLitigationUpd.DataBind();

        }
        protected void grdlitiup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LitiID = Convert.ToInt32(grdlitiup.SelectedValue);
            tblPlEmpLitigationDetail LitiDetail = db.tblPlEmpLitigationDetails.Where(x => x.litiDeID == LitiID).FirstOrDefault();
            ddlLitigationUpd.SelectedValue = LitiDetail.LitiTitle;
            txtFinalDate.Text = LitiDetail.FinalDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            ddlStatLitiup.SelectedValue = LitiDetail.statuss;
            txtFinaljud.Text = LitiDetail.FinalJud;
        }
        protected void grdlitiup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdlitiup.PageIndex = e.NewPageIndex;
            BindGridLitiUpdate();
        }
        protected void grdLitiup_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

            }
        }
        protected void lnkEduPrint_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrint = (LinkButton)sender;
            string ImgID = lnkPrint.CommandArgument;
            string filename;
            var img = db.tblPlEmpEdus.Where(x => x.filePath == ImgID).FirstOrDefault();
            if (img == null)
            {
                filename =  "../empix/noimage.jpg";
            }
            else
            {
                filename = img.filePath;
            }
            
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
           string.Format("window.open('Seepicture.aspx?ID={0}');", filename), true);
            //contenttype = img.EmpEduID.ToString();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.TransmitFile(Server.MapPath("~/Attachments/" + filename));
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            //Response.Flush();
            //Response.End();
        }

        protected void lnkPriorPrint_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrintExp = (LinkButton)sender;
            string ImgID = lnkPrintExp.CommandArgument;
            string Expfilename;
            var img = db.tblPlEmpExps.Where(x => x.Attachment == ImgID).FirstOrDefault();
            if (img == null)
            {
                Expfilename = "../empix/noimage.jpg";
            }
            else
            {
                Expfilename = img.Attachment;
            }
            
                ClientScript.RegisterStartupScript(this.GetType(), "Popup",
           string.Format("window.open('SeePictureExpe.aspx?ID={0}');", Expfilename), true);
            
            
        }

        protected void lnkTenPrint_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrintTen = (LinkButton)sender;
            string ImgID = lnkPrintTen.CommandArgument;
            string filename;
            var img = db.TenureExperiences.Where(x => x.Attachment == ImgID).FirstOrDefault();
            //filename = img.Attachment;
            if (img == null)
            {
                filename = "../empix/noimage.jpg";
            }
            else
            {
                filename = img.Attachment;
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
           string.Format("window.open('Seepicture.aspx?ID={0}');", filename), true);
        }

        protected void lnkEnqPrint_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrintTen = (LinkButton)sender;
            string ImgID = lnkPrintTen.CommandArgument;
            string filename;
            var img = db.tblPlEmpEnqs.Where(x => x.Attachment == ImgID).FirstOrDefault();
            if (img == null)
            {
                filename = "../empix/noimage.jpg";
            }
            else
            {
                filename = img.Attachment;
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
           string.Format("window.open('Seepicture.aspx?ID={0}');", filename), true);
        }
        protected void lnkLitiPrint_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrintTen = (LinkButton)sender;
            string ImgID = lnkPrintTen.CommandArgument;
            string filename;
            var img = db.tblPlEmpLitigations.Where(x => x.Attachment == ImgID).FirstOrDefault();
            //filename = img.Attachment;
            if (img == null)
            {
                filename = "../empix/noimage.jpg";
            }
            else
            {
                filename = img.Attachment;
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
           string.Format("window.open('Seepicture.aspx?ID={0}');", filename), true);
        }

        protected void lnkProPrint_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrintPro = (LinkButton)sender;
            string ImgID = lnkPrintPro.CommandArgument;
            string filename;
            var img = db.tblPlEmpPermotions.Where(x => x.Attachement == ImgID).FirstOrDefault();
            if (img == null)
            {
                filename = "../empix/noimage.jpg";
            }
            else
            {
                filename = img.Attachement;
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
           string.Format("window.open('Seepicture.aspx?ID={0}');", filename), true);
        }

        protected void searchemp_changeIndex(object sender, EventArgs e)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
                int emppp = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
                string empref = db.tblPlEmpDatas.Where(x => x.EmpID == emppp).FirstOrDefault().EmpCode;
                ddlperson.SelectedValue = empref;
                FillDropdowntitleLiti(emppp);
                FillDropdowntitleEnq(emppp);
            }
            //int empIDw = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            
        }
        

        protected void ddlPersonal_change(object sender, EventArgs e)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
                string emppp = ddlperson.SelectedValue;
                int empref = db.tblPlEmpDatas.Where(x => x.EmpCode == emppp).FirstOrDefault().EmpID;
                ddlEmployeeSearch.SelectedValue = empref.ToString();
            }
        }

        #endregion

        #region Helping Method



        private void FillDivisionDropDownEmployee()
        {
            RMSDataContext db = new RMSDataContext();

            this.ddlDivsion.DataTextField = "br_nme";
            ddlDivsion.DataValueField = "br_id";
            ddlDivsion.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            ddlDivsion.DataBind();
        }

        private void FillDropdowntitleEnq(int empIDd)
        {
            ddlenquirytitle.Items.Clear();
            this.ddlenquirytitle.DataTextField = "EnqTitle";
            this.ddlenquirytitle.DataValueField = "EnqTitle";
            this.ddlenquirytitle.DataSource = db.tblPlEmpEnqs.Where(x => x.EmpID == empIDd).ToList();
            this.ddlenquirytitle.DataBind();
            ddlenquirytitle.Items.Insert(0, new ListItem("Select title", "0"));
        }

        private void FillDropdowntitleLiti(int empIDd)
        {
            ddlLitigationUpd.Items.Clear();
            ddlLitigationUpd.Items.Insert(0, new ListItem("Select Title", "0"));
            ddlLitigationUpd.DataTextField = "LitiTitle";
            ddlLitigationUpd.DataValueField = "LitiTitle";
            ddlLitigationUpd.DataSource = db.tblPlEmpLitigations.Where(x => x.EmpID == empIDd).ToList();
            ddlLitigationUpd.DataBind();
        }


        private void FillSearchDropDownEmployee()
        {
            RMSDataContext db = new RMSDataContext();

            this.ddlEmployeeSearch.DataTextField = "FullName";
            ddlEmployeeSearch.DataValueField = "EmpID";

            ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID != 14 && x.BranchID != null).ToList().OrderBy(x => x.FullName);
            ddlEmployeeSearch.DataBind();
            ddlEmployeeSearch.Items.Insert(0, new ListItem("Select Employee", "0"));


        }

        //private void FillSearchBranchDropDown()
        //{
        //    RMSDataContext db = new RMSDataContext();

        //    Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

        //    this.searchBranchDropDown.DataTextField = "br_nme";
        //    searchBranchDropDown.DataValueField = "br_id";
        //    if (BranchObj.IsHead == true)
        //    {
        //        searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
        //    }
        //    else
        //    {
        //        List<Branch> BranchList = new List<Branch>();

        //        if (BranchObj != null)
        //        {
        //            if (BranchObj.IsDisplay == true)
        //            {
        //                BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BranchID).ToList();
        //                BranchList.Insert(0, BranchObj);
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


        //protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!searchBranchDropDown.SelectedValue.Equals("0"))
        //        {
        //            IsBranch = Convert.ToInt32(searchBranchDropDown.SelectedValue);
        //            BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
        //            FillSearchDropDownEmployee();


        //        }

        //    }
        //    catch
        //    { }
        //}

        protected void FillEnquiryTypes()
        {
            ddlEnqtypes.Items.Insert(0, new ListItem("Select Type", "0"));
            ddlEnqtypes.DataTextField = "EnquiryName";
            ddlEnqtypes.DataValueField = "EnquiryName";
            ddlEnqtypes.DataSource = db.tblPlEmpEnquiryTypes.ToList();
            ddlEnqtypes.DataBind();
            
        }

        private void FillDropDownCity()
        {
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityID";
            ddlCity.DataSource = city.GetAllCities((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCity.DataBind();
        }

        private void FillDropDownScaleTenure()
        {
            RMSDataContext db = new RMSDataContext();
            ddljobtype.DataTextField = "JobTypeName1";
            ddljobtype.DataValueField = "JobNameID";
            ddljobtype.DataSource = db.JobTypeNames.Where(x => x.IsActive == true).ToList();
            ddljobtype.DataBind();
        }
        private void FillDropdownScaleTenureExp()
        {
            RMSDataContext db = new RMSDataContext();
            ddlTenureScale.DataTextField = "ScaleName";
            ddlTenureScale.DataValueField = "ScaleID";
            ddlTenureScale.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.Orderby);
            ddlTenureScale.DataBind();
        }
        

        private void FillDropDownScale()
        {
            RMSDataContext db = new RMSDataContext();
            ddlScale.DataTextField = "ScaleName";
            ddlScale.DataValueField = "ScaleID";
            ddlScale.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.Orderby);
            ddlScale.DataBind();
        }

        //private void FillDropDownEmployeeSearch()
        //{
        //    ddlEmployeeSearch.DataTextField = "FullName";
        //    ddlEmployeeSearch.DataValueField = "EmpID";
        //    ddlEmployeeSearch.DataSource = empBL.GetAllEmployees((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlEmployeeSearch.DataBind();
        //}

        private void FillDropDownYearPanel()
        {
            ddlYear.DataTextField = "YearName";
            ddlYear.DataValueField = "YearID";
            ddlYear.DataSource = empBL.GetYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlYear.DataBind();
        }
        private void FillDropDownLitiPanel()
        {
            ddlLitigation.DataTextField = "LitiName";
            ddlLitigation.DataValueField = "LitiID";
            ddlLitigation.DataSource = empBL.GeLitiTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLitigation.DataBind();
        }
        private void FillDropDownLiti2Panel()
        {
            //int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
           
        }
        //private void FillDropDownCodeDsgn()
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
        //    pl.CodeTypeID = 4;
        //    this.ddlDesignation.DataTextField = "CodeDesc";
        //    ddlDesignation.DataValueField = "CodeID";
        //    ddlDesignation.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlDesignation.DataBind();
        //}

        private void FillDropDownPlaceTenure()
        {
            ddlAdditionPlace.DataTextField = "br_nme";
            ddlAdditionPlace.DataValueField = "br_id";
            ddlAdditionPlace.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            ddlAdditionPlace.DataBind();
        }

        private void FillDropDownpostingDsgn()
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
            this.dllpostingdes.DataTextField = "CodeDesc";
            dllpostingdes.DataValueField = "CodeID";
            dllpostingdes.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            dllpostingdes.DataBind();
        }
        private void FillDropDownofficerDsgn()
        {
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            };
            this.ddlropdes.DataValueField = "CodeDesc";
            this.ddlropdes.DataValueField = "CodeDesc";
            this.ddlropdes.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 4 && x.CompID == _cmp && x.Enabled == true).ToList().OrderBy(x => x.sort);
            this.ddlropdes.DataBind();
            //this.dllpostingdes.DataTextField = "CodeDesc";
            //dllpostingdes.DataValueField = "CodeID";
            //dllpostingdes.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //dllpostingdes.DataBind();
        }
        //private void FillDropDownPriorPostDsgn()
        //{
        //    byte _cmp = 1;
        //    if (Session["CompID"] == null)
        //    {
        //        _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
        //    }
        //    else
        //    {
        //        _cmp = Convert.ToByte(Session["CompID"].ToString());
        //    };
        //    this.ddlPriorPost.DataValueField = "CodeDesc";
        //    this.ddlPriorPost.DataValueField = "CodeDesc";
        //    this.ddlPriorPost.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 4 && x.CompID == _cmp && x.Enabled == true).ToList().OrderBy(x => x.CodeID);
        //    this.ddlPriorPost.DataBind();
        //    //this.dllpostingdes.DataTextField = "CodeDesc";
        //    //dllpostingdes.DataValueField = "CodeID";
        //    //dllpostingdes.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    //dllpostingdes.DataBind();
        //}
        //private void FillDropDownPriorAddtinalDsgn()
        //{
        //    byte _cmp = 1;
        //    if (Session["CompID"] == null)
        //    {
        //        _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
        //    }
        //    else
        //    {
        //        _cmp = Convert.ToByte(Session["CompID"].ToString());
        //    };
        //    this.ddlPrioraddtional.DataValueField = "CodeDesc";
        //    this.ddlPrioraddtional.DataValueField = "CodeDesc";
        //    this.ddlPrioraddtional.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 4 && x.CompID == _cmp && x.Enabled == true).ToList().OrderBy(x => x.CodeID);
        //    this.ddlPrioraddtional.DataBind();
        //    //this.dllpostingdes.DataTextField = "CodeDesc";
        //    //dllpostingdes.DataValueField = "CodeID";
        //    //dllpostingdes.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    //dllpostingdes.DataBind();
        //}
        private void FillDropDownTenurePostDsgn()
        {
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            };
            this.ddlTenurePost.DataValueField = "CodeDesc";
            this.ddlTenurePost.DataValueField = "CodeDesc";
            this.ddlTenurePost.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 4 && x.CompID == _cmp && x.Enabled == true).ToList().OrderBy(x => x.sort);
            this.ddlTenurePost.DataBind();
            //this.dllpostingdes.DataTextField = "CodeDesc";
            //dllpostingdes.DataValueField = "CodeID";
            //dllpostingdes.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //dllpostingdes.DataBind();
        }
        private void FillDropDownTenureAddtinalDsgn()
        {
            byte _cmp = 1;
            if (Session["CompID"] == null)
            {
                _cmp = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                _cmp = Convert.ToByte(Session["CompID"].ToString());
            };
            this.ddlTenureAddtional.DataValueField = "CodeDesc";
            this.ddlTenureAddtional.DataValueField = "CodeDesc";
            this.ddlTenureAddtional.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 4 && x.CompID == _cmp && x.Enabled == true).ToList().OrderBy(x => x.sort);
            this.ddlTenureAddtional.DataBind();
            //this.dllpostingdes.DataTextField = "CodeDesc";
            //dllpostingdes.DataValueField = "CodeID";
            //dllpostingdes.DataSource = new PlCodeBL().GetAll(pl, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //dllpostingdes.DataBind();
        }
        private void FillDropDownReportingDsgn()
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
            this.ddloffDes.DataValueField = "CodeDesc";
            this.ddloffDes.DataValueField = "CodeDesc";
            this.ddloffDes.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 4 && x.CompID == _cmp && x.Enabled == true).ToList().OrderBy(x => x.sort);
            this.ddloffDes.DataBind();
        }
        protected void FillDropDownPermotionScal()
        {
            ddlperscal.DataTextField = "ScaleName";
            ddlperscal.DataValueField = "ScaleID";
            ddlperscal.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.Orderby);
            ddlperscal.DataBind();
        }

        protected void FillDropDownDegreetype()
        {
            ddlDegreeType.DataTextField = "Name";
            ddlDegreeType.DataValueField = "Name";
            ddlDegreeType.DataSource = db.tblPlEmpDegreeTypes.ToList().OrderBy(x => x.sort);
            ddlDegreeType.DataBind();
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

        private void FillDropDownPermotionDsgn()
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
            this.ddlPerDes.DataValueField = "CodeID";
            this.ddlPerDes.DataTextField = "CodeDesc";
            this.ddlPerDes.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 4 && x.CompID == _cmp && x.Enabled == true).ToList().OrderBy(x => x.sort);
            this.ddlPerDes.DataBind();
        }
        protected void BindGridEduPanel()
        {
            int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            this.grdEducation.DataSource = empBL.GetEmployeeEducation(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdEducation.DataBind();
        }
        protected void BindGridExpPanel()
        {
            int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            this.grdExperience.DataSource = empBL.GetEmployeeExperience(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdExperience.DataBind();
        }
        protected void BindGridAcrPanel()
        {
            int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            this.grdAcr.DataSource = empBL.GetEmployeeAcr(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdAcr.DataBind();
        }

        protected void BindGridEnqPanel()
        {
            int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            this.grdEnq.DataSource = empBL.GetEmployeeEnq(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdEnq.DataBind();
        }
        protected void BindGridLitiPanel()
        {
            int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            this.grdLiti.DataSource = empBL.GetEmployeeLiti(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdLiti.DataBind();
        }
        protected void BindGridPerPanel()
        {
            int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            this.grdPermotion.DataSource = from per in db.tblPlEmpPermotions
                                           join des in db.tblPlCodes on per.DesID equals des.CodeID
                                           join scal in db.TblEmpScales on per.scale equals scal.ScaleID
                                           join emp in db.tblPlEmpDatas on per.empID equals emp.EmpID
                                           where per.empID == empID
                                           select new
                                           {
                                               per.PerID,
                                               per.FromDate,
                                               per.todate,
                                               per.pertype,
                                               des.CodeDesc,
                                               scal.ScaleName,
                                               per.Attachement

                                           };
            this.grdPermotion.DataBind();
        }


        protected void BindGridtenure()
        {
            int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            this.gedTenure.DataSource = from ten in db.TenureExperiences
                                        join sca in db.JobTypeNames on ten.jobtype equals sca.JobNameID
                                        join sc in db.TblEmpScales on ten.Scale equals sc.ScaleID
                                        where ten.EmpID == empID
                                        select new
                                        {
                                            ten.TenID,
                                            sca.JobTypeName1,
                                            ten.EmpID,
                                            ten.joinDate,
                                            ten.LeavDate,
                                            ten.Postedas,
                                            ten.Appointedas,
                                            ten.YOE,
                                            ten.AddtionalCharge,
                                            ten.Branch.br_nme,
                                            ten.Attachment,
                                            ten.MonthExp,
                                            sc.ScaleName
                                        };
            this.gedTenure.DataBind();
        }

        protected void BindGridUpdateEnq()
        {
            int empId = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            this.grdEnqUpdate.DataSource = from enq in db.tblPlEmEnqDetails
                                           where enq.EmpId == empId
                                           select new
                                           {
                                               enq.EnqDelID,
                                               enq.OngionEnq,
                                               enq.UpdateDate,
                                               enq.updateremarks,
                                               enq.updateStatus
                                           };
            grdEnqUpdate.DataBind();
        }

        protected void BindGridLitiUpdate()
        {
            int empID = Convert.ToInt32(ddlEmployeeSearch.SelectedValue);
            this.grdlitiup.DataSource = from liti in db.tblPlEmpLitigationDetails
                                        where liti.EmpID == empID
                                        select new
                                        {
                                            liti.litiDeID,
                                            liti.FinalDate,
                                            liti.statuss,
                                            liti.EmpID,
                                            liti.FinalJud,
                                            liti.LitiTitle
                                        };
            this.grdlitiup.DataBind();
        }

       

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<sp_GetEmployeeSearchResult> GetEmployee(string employee)
        {
            EmpProfileBL pro = new EmpProfileBL();
            List<sp_GetEmployeeSearchResult> emp = pro.GetEmployeeSearch(IsBranch, employee, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return emp;
        }

        private void ClearFields()
        {
            ID = 0;
            CompID = 0;
        }
        private void clear_Text()
        {
            LitiID = 0;
            ID = 0;
           ddlYear.SelectedValue = "0";
            txtEduDegTtl.Text = "";
            ddluniversity.SelectedValue = "0";
            ddlDegreeType.SelectedValue = "0";
            txtPercentage.Text = "";
            txtEnqtitle.Text = "";
            ImageEdu.ImageUrl = "";
            ImageExp.ImageUrl = "";
            TenureExpImage.ImageUrl = "";
            tblPRo.ImageUrl = "";
            ddlStatus.SelectedValue = "0";
            ddlUpdaStatus.SelectedValue = "0";
            AcrImage.ImageUrl = "";
            ddlCity.SelectedValue = "0";
            txtEduDegTtl.Focus();
            txtpriorExp.Text = "";
            ddlTenurePost.SelectedValue = "0";
            ddlTenureAddtional.SelectedValue = "0";
            txtlea.Text = "";
            txtaddtionalChargePost.Text = "";
            ddlAdditionPlace.SelectedValue = "0";
            txtadditionalPlace.Text = "";
            txtjoi.Text = "";
            dllexp.SelectedValue = "0";
            ddljobtype.SelectedValue = "0";
            ddlDivsion.SelectedValue = "0";
            //txtExpAppAs.Text = "";
            ddlYear.SelectedValue = "0";
            ddlExpSector.SelectedValue = "0";
            txtLeaving.Text = "";
            txtOrganization.Text = "";
            txtaddtionalChar.Text = "0";
            //ddlCity.Text = "";
            //txtdep.Text = "";
            ddlStatLitiup.SelectedValue = "";
            txtFinaljud.Text = "";
            //ddlLitigationUpd.SelectedValue = null;
            //txtdes.Text = "";
            ddlTenureScale.SelectedValue = "0";
            ddlTenureMonth.SelectedValue = "0";
            ddlScale.SelectedValue = null;
            txtExpDOJ.Text = "";
            ddlDomicile.SelectedValue = "0";
            // ddlDesignation.SelectedValue = "0";
            // duration.Text = "";
            dllpostingdes.SelectedValue = "0";
            ReporOff.Text = "";
            ddloffDes.SelectedValue = "0";
            ddlropdes.SelectedValue = "0";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            //txtofficerDate.Text = "";
            txtrepDate.Text = "";
            txtcountroff.Text = "";
            txtacrRemaks.Text = "";
            ddlEnqtypes.SelectedValue = "0";
            EnqDate.Text = "";
            IssuAuthori.Text = "";
            ddlStatus.Text = "";
            txtarearemaks.Text = "";
            ddlLitigation.SelectedValue = "0";
            litiDate.Text = "";
            authorityforum.Text = "";
            txtareaEnqremarks.Text = "";
            txtperfrom.Text = "";
            txtPerTo.Text = "";
            txtPercentage.Text = "";
            ddlPerDes.SelectedValue = "0";
            ddlpertypes.SelectedValue = "0";
            ddlperscal.SelectedValue = "0";
            //ddlenquirytitle.SelectedValue = "0";
            ddlStatus.SelectedValue = "Pending";
            ddlUpdaStatus.SelectedValue = "Pending";
            txtupdatedate.Text = "";
            txtUpdateRemarks.Text = "";
            txtLitiTitle.Text = "";
            txtFinalDate.Text = "";
            ddlStatLitiup.SelectedValue = "0";
            txtFinaljud.Text = "";
            litiImage.ImageUrl = "";
           // ddlLitigationUpd.SelectedValue = "0";
            txtAuthorityTitle.Text = "";
            ddlEduVerified.SelectedValue = "True";
            status.SelectedValue = "0";
            ddlStatLitiup.SelectedValue = "0";
        }

        private void Clear_UpdateText()
        {
            ddlLitigationUpd.SelectedValue = "0";
            ddlenquirytitle.SelectedValue = "0";
        }

        #endregion

    }
}

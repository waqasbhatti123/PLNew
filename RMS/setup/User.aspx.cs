using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace RMS.Setup
{
    public partial class User : BasePage
    {

        #region DataMembers

        tblAppUser userPojo;
        UserBL userManager = new UserBL();
        ListItem selList = new ListItem();
        RMSDataContext dataContext = new RMSDataContext();


        #endregion

        #region Properties

#pragma warning disable CS0114 // 'User.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'User.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        #endregion

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


        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "User").ToString();


                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }

                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
               
                FillDropDownGroups();
                FillDropDownCities();
                FillDropDownCompanies();
                FillDropDownParty();
                ddlComp.SelectedIndex = 1;
                FillSBranchDropDown();
                FillDropDownEmployee();
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BindGrid("", 0, 1, 0, BranchID, IsSearch);
                //FillDropDownBranch(ddlComp.SelectedValue);
                //ddlBranch.SelectedIndex = 1;
                ddlFltGroup.SelectedValue = "1";
                ucButtons.ValidationGroupName = "main";
                //txtName.Focus();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IsSearch = true;
            BindGrid(txtFltUser.Text.Trim(), Convert.ToInt32(ddlFltGroup.SelectedValue), Convert.ToInt32(ddlFltComp.SelectedValue), Convert.ToInt32(ddlFltCity.SelectedValue), BranchID, IsSearch);
        }


       

        private void FillSBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();
            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            this.ddlBranch.DataTextField = "br_nme";
            ddlBranch.DataValueField = "br_id";
            if (BranchObj.IsHead == true)
            {
                ddlBranch.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
                
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
                ddlBranch.DataSource = BranchList.ToList();
            }
            ddlBranch.DataBind();

        }

        protected void BranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlBranch.SelectedValue.Equals("0"))
                {
                    BranchID = Convert.ToInt32(ddlBranch.SelectedValue.Trim());
                    ddlEmployee.Items.Clear();

                    FillDropDownEmployee();

                    //ddlEmployee.Items.Insert(0, new tblPlEmpData
                    //{
                    //    EmpID = 0,
                    //    FullName = "Select Emplyee"


                    //});
                }

            }
            catch
            { }
        }



        private void FillDropDownEmployee()
        {
            RMSDataContext db = new RMSDataContext();

            this.ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataValueField = "EmpID";
            if (BranchID == 1)
            {
                ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1).OrderBy(x => x.FullName).ToList();
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));
            }

            else if (BranchID > 1)
            {
                Branch branchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
                List<tblPlEmpData> empBranchList = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
                if (branchObj.IsDisplay == true)
                {
                    List<tblPlEmpData> empSubBranchList = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.Branch1.br_idd == BranchID && x.Branch1.br_status == true).ToList();
                    ddlEmployee.DataSource = empBranchList.Concat(empSubBranchList).OrderBy(x => x.FullName).ToList();

                }
                else
                {
                    ddlEmployee.DataSource = empBranchList.OrderBy(x => x.FullName).ToList();
                }
                ddlEmployee.DataBind();
                ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));
            }

            


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
                    BindGrid("", 0, 1, 0, BranchID, IsSearch);
                }

            }
            catch
            { }
        }





        protected void grdUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdUsers.SelectedDataKey.Value);
            this.GetByID();
            ddlGroup.Focus();
        }

        protected void grdUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUsers.PageIndex = e.NewPageIndex;
            BindGrid(txtFltUser.Text.Trim(), Convert.ToInt32(ddlFltGroup.SelectedValue), Convert.ToInt32(ddlFltComp.SelectedValue), Convert.ToInt32(ddlFltCity.SelectedValue), BranchID, IsSearch);
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                //pnlMain.Enabled = true;
                ddlGroup.Focus();
            }
            else if (e.CommandName == "Save")
            {
                int empID = Convert.ToInt32(ddlEmployee.SelectedValue);
                if (ID == 0)
                {

                    int empCount = dataContext.tblAppUsers.Where(x => x.EmpID == empID).Count();
                    if(empCount > 0)
                    {
                        ucMessage.ShowMessage("Employee is Already Exist", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        this.Insert();
                        ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                       
                    }

                }
                else
                {
                    int empCount = dataContext.tblAppUsers.Where(x => x.EmpID == empID && x.UserID != ID).Count();
                    if (empCount > 0)
                    {
                        ucMessage.ShowMessage( "Employee is Already Exist", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        this.Insert();
                        ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                    }
                    this.Update();
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                }
            }
            else if (e.CommandName == "Delete")
            {
                //// TRANSACTION WALA KAAM KARNA HAI......

                //try
                //{
                //    this.Delete(ID);
                //    //pnlMain.Enabled = false;
                //    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                //}
                //catch (SqlException ex)
                //{
                //    if (ex.Number == 547)
                //    {
                //        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletionDependency").ToString(), RMS.BL.Enums.MessageType.Error);
                //        return;
                //    }
                //    else
                //    {
                //        Session["errors"] = ex.Message;
                //        Response.Redirect("~/home/Error.aspx");
                //    }
                //}

                //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
                //BindGrid("", 1, 0, 0);
                //ddlFltGroup.SelectedValue = "1";
                //ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                //pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                ddlGroup.Focus();
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }

        //protected void ddlComp_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FillDropDownBranch(ddlComp.SelectedValue);
        //}
        
        #endregion

        #region Helping Method

        protected void BindGrid(string userName, int groupId, int compId, int cityId, int brId, bool isSearch)
        {
            this.grdUsers.DataSource = userManager.GetAll(userName, groupId, compId, cityId,BranchID,  IsSearch, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdUsers.DataBind();
        }

        protected void GetByID()
        {
            userPojo = userManager.GetByIDDetailed(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (userPojo.vendor != null)
                ddlParty.SelectedValue = userPojo.vendor;
            else
                ddlParty.SelectedValue = "0";
            this.ddlEmployee.SelectedValue = userPojo.EmpID.ToString();
            //this.txtName.Text = userPojo.UserName;
            this.txtEmail.Text = userPojo.LoginID;

            this.ddlGroup.SelectedValue = userPojo.GroupID.ToString();
            this.ddlCity.SelectedValue = userPojo.CityID.ToString();
            if (userPojo.tblCompany != null)
            {
                this.ddlComp.SelectedValue = userPojo.CompID.ToString();
                
               
                
                if (userPojo.Branch != null)
                {
                    this.ddlBranch.SelectedValue = userPojo.br_id.ToString();
                }
            }

            this.txtPassword.Text = userPojo.Password;
            this.txtConfPwd.Text = userPojo.Password;
            

            this.rblStatus.SelectedValue = userPojo.Enabled == true ? "1" : "0";
            this.rblGender.SelectedValue = userPojo.Gender;


            //pnlMain.Enabled = true;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);

        }

        protected void Update()
        {
            tblAppUser updUser = userManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            
            if (ddlParty.SelectedValue != "0")
                updUser.vendor = ddlParty.SelectedValue;
            else
                updUser.vendor = null;
            int emID = Convert.ToInt32(ddlEmployee.SelectedValue);
            if(emID > 0)
            {
                updUser.EmpID = Convert.ToInt32(ddlEmployee.SelectedValue);
                tblPlEmpData empObj = dataContext.tblPlEmpDatas.Where(x => x.EmpID == emID).FirstOrDefault();
                updUser.UserName = empObj.FullName;
            }
            
            
            updUser.Gender = rblGender.SelectedValue;
            updUser.LoginID = txtEmail.Text.Trim();
            updUser.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;

            updUser.tblAppGroup = new GroupBL().GetByID(Convert.ToInt32(ddlGroup.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            updUser.GroupID = Convert.ToInt32(ddlGroup.SelectedValue);

            updUser.Password = txtPassword.Text.Trim();

            if (Session["UserID"] == null)
            {
                updUser.UpdatedBy = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
            }
            else
            {
                updUser.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
            }

            updUser.UpdatedOn = BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            if (ddlComp.SelectedValue.Equals("0"))
            {
                ucMessage.ShowMessage("Company is required", RMS.BL.Enums.MessageType.Error);
                return;
            }
            else
            {
                updUser.tblCompany = userManager.GetByIDComp(ddlComp.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }

            if (ddlBranch.SelectedValue.Equals("0"))
            {
                updUser.Branch = null;
            }
            else
            {
                updUser.Branch = userManager.GetByIDBranch(ddlBranch.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }

            if (!userManager.ISAlreadyExist(updUser, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                userManager.Update(updUser, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid("", Convert.ToInt32(ddlGroup.SelectedValue), 0, 0, BranchID, IsSearch);
                ddlFltGroup.SelectedValue = ddlGroup.SelectedValue;
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "userAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                //pnlMain.Enabled = true;
            }
        }

        protected void Delete()
        {
            //userManager.DeleteByID(Id, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            userPojo = new tblAppUser();

            if (ddlComp.SelectedValue.Equals("0"))
            {
                ucMessage.ShowMessage("Company is required", RMS.BL.Enums.MessageType.Error);
                return;
            }
            else
            {
                userPojo.CompID = Convert.ToByte(ddlComp.SelectedValue);
            }

            if (!ddlBranch.SelectedValue.Equals("0"))
            {
                userPojo.br_id = BranchID;
            }
            if (ddlParty.SelectedValue != "0")
                userPojo.vendor = ddlParty.SelectedValue;
            else
                userPojo.vendor = null;

            int emID = Convert.ToInt32(ddlEmployee.SelectedValue);
            if (emID > 0)
            {
                userPojo.EmpID = Convert.ToInt32(ddlEmployee.SelectedValue);
                tblPlEmpData empObj = dataContext.tblPlEmpDatas.Where(x => x.EmpID == emID).FirstOrDefault();
                userPojo.UserName = empObj.FullName;
            }
           
            userPojo.Gender = rblGender.SelectedValue;

            userPojo.CityID = Convert.ToInt32(ddlCity.SelectedValue);

            userPojo.LoginID = txtEmail.Text.Trim();

            userPojo.Enabled = rblStatus.SelectedValue.Equals("1") ? true : false;
            userPojo.GroupID = Convert.ToInt32(ddlGroup.SelectedValue);
            
            userPojo.Password = txtPassword.Text.Trim();
            if (Session["UserID"] == null)
            {
                userPojo.UpdatedBy = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
            }
            else
            {
                userPojo.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
            }

            userPojo.UpdatedOn = BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (!userManager.ISAlreadyExist(txtEmail.Text.Trim(), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                userManager.Insert(userPojo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid("", Convert.ToInt32(ddlGroup.SelectedValue), 0, 0,BranchID, IsSearch);
                ddlFltGroup.SelectedValue = ddlGroup.SelectedValue;
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "userAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                //pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            //this.txtName.Text = "";
            this.txtEmail.Text = "";
            this.txtPassword.Text = "";
            this.txtConfPwd.Text = "";
            this.rblStatus.SelectedValue = "1";
            this.rblGender.SelectedValue = "M";
            this.ddlGroup.SelectedValue = "0";
            this.ddlCity.SelectedValue = "0";
            this.ddlEmployee.SelectedIndex = 0;
            ddlParty.SelectedValue = "0";
            //this.ddlComp.SelectedValue = "0";
            //FillDropDownBranch("0");
            grdUsers.SelectedIndex = -1;
            //txtName.Focus();
        }

        private void FillDropDownGroups()
        {
            ddlGroup.DataTextField = "GroupName";
            ddlGroup.DataValueField = "GroupID";
            ddlGroup.DataSource = userManager.GetAllGroupsCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlGroup.DataBind();

            ddlFltGroup.DataTextField = "GroupName";
            ddlFltGroup.DataValueField = "GroupID";
            ddlFltGroup.DataSource = userManager.GetAllGroupsCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltGroup.DataBind();
        }

        private void FillDropDownCities()
        {
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityID";
            ddlCity.DataSource = userManager.GetAllCityCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCity.DataBind();

            ddlFltCity.DataTextField = "CityName";
            ddlFltCity.DataValueField = "CityID";
            ddlFltCity.DataSource = userManager.GetAllCityCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltCity.DataBind();
        }

        private void FillDropDownCompanies()
        {
            ddlComp.DataTextField = "CompName";
            ddlComp.DataValueField = "CompID";
            ddlComp.DataSource = userManager.GetAllCompCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlComp.DataBind();

            ddlFltComp.DataTextField = "CompName";
            ddlFltComp.DataValueField = "CompID";
            ddlFltComp.DataSource = userManager.GetAllCompCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltComp.DataBind();
        }

        //private void FillDropDownBranch(string compid)
        //{
        //    if (Convert.ToInt32(ddlComp.SelectedValue) > 0)
        //    {
        //        ddlBranch.Items.Clear();
        //        ddlBranch.Dispose();
        //        selList.Text = "Select Branch";
        //        selList.Value = "0";
        //        ddlBranch.Items.Insert(0, selList);

        //        ddlBranch.DataTextField = "br_nme";
        //        ddlBranch.DataValueField = "br_id";
        //        ddlBranch.DataSource = userManager.GetAllBranchCombo(Convert.ToInt32(compid), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        ddlBranch.DataBind();
        //    }
        //    else
        //    {
        //        ddlBranch.Items.Clear();
        //        ddlBranch.Dispose();
        //        selList.Text = "Select Branch";
        //        selList.Value = "0";
        //        ddlBranch.Items.Insert(0, selList);
        //    }
        //}

        public void FillDropDownParty()
        {
            ddlParty.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlParty.DataTextField = "gl_dsc";
            ddlParty.DataValueField = "gl_cd";
            ddlParty.DataBind();
        }
        
        #endregion
    }
}
 
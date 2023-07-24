using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.profile
{
    public partial class AddtionalAllDdd : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        SalContentBL salContent = new SalContentBL();
        EmpBL empBL = new EmpBL();
        AddtionalAllDdBL add = new AddtionalAllDdBL();


#pragma warning disable CS0114 // 'AddtionalAllDdd.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'AddtionalAllDdd.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public int Counter
        {
            get { return (ViewState["Counter"] == null) ? 0 : Convert.ToInt32(ViewState["Counter"]); }
            set { ViewState["Counter"] = value; }
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


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "AllDudction").ToString();

                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtToCal.Format = Request.Cookies["uzr"]["DateFormat"];

                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                    txtToCal.Format = Session["DateFormat"].ToString();
                }

                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                FillDropDownAllDdd();
                FillDropDownEmployee();
                FillSearchBranchDropDown();
                AllDedDropdown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();

                BindGrid(BranchID, IsSearch);
            }
        }

        private void FillDropDownAllDdd()
        {
            ddlAllDd.DataTextField = "Name";
            ddlAllDd.DataValueField = "SalaryContentTypeID";
            ddlAllDd.DataSource = salContent.GetAllDed((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlAllDd.DataBind();
        }
        private void FillDropDownEmployee()
        {
            RMSDataContext db = new RMSDataContext();

            this.ddlEmployee.DataTextField = "FullName";
            ddlEmployee.DataValueField = "EmpID";

            ddlEmployee.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == 1 && x.BranchID == BranchID).ToList();
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
                    BindGrid(BranchID, IsSearch);
                }

            }
            catch
            { }
        }


        protected void AllDedDropdown()
        {
            ddlallDed.DataTextField = "Name";
            ddlallDed.DataValueField = "Name";
            ddlallDed.DataSource = db.SalaryContents.Where(x => x.IsActive == true).OrderBy(x => x.Sort).ToList();
            ddlallDed.DataBind();
            ddlallDed.Items.Insert(0, new ListItem("Select", "0"));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                int empid = Convert.ToInt32(ddlEmployee.SelectedValue);
                int alli = Convert.ToInt32(ddlAllDd.SelectedValue);
                tblPlAddtionalAllDd all = new tblPlAddtionalAllDd();
                if (ddlEmployee.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    all.EmpID = empid;
                }
                if (ddlAllDd.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please Select Allowance / Deduction Type", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    all.SalaryContentTypeID = alli;
                }
                if (ddlallDed.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please Select Allowance / Dedcution Name", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    all.Name = ddlallDed.SelectedValue;
                }
                if (txtSize.Text == "")
                {
                    ucMessage.ShowMessage("Please Insert Size", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    all.size = Convert.ToInt32(txtSize.Text);
                }
                if (txtfrom.Text == "")
                {
                    ucMessage.ShowMessage("Please Insert From Date", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    all.fromd = Convert.ToDateTime(txtfrom.Text);
                }
                if (txtTo.Text == "")
                {
                    ucMessage.ShowMessage("Please Insert To Date", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    all.tod = Convert.ToDateTime(txtTo.Text.Trim());
                }
                if (CheckIsActive.Checked == true)
                {
                    all.isActive = true;
                }
                else
                {
                    all.isActive = false;
                }
                
                if (Convert.ToBoolean(checkIsPercen.Checked))
                {
                    all.isValue = Convert.ToBoolean(0);
                }
                else
                {
                    all.isValue = Convert.ToBoolean(1);
                }
                add.Insert(all, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Save Succesfully", BL.Enums.MessageType.Info);
            }
            else
            {
                int empid = Convert.ToInt32(ddlEmployee.SelectedValue);
                int alldd = Convert.ToInt32(ddlAllDd.SelectedValue);
                RMS.BL.tblPlAddtionalAllDd cont = add.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (ddlEmployee.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please Select Employee", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    cont.EmpID = empid;
                }
                if (ddlAllDd.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please Select Allowance / Deduction Type", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    cont.SalaryContentTypeID = alldd;
                }
                if (ddlallDed.SelectedValue == "0")
                {
                    ucMessage.ShowMessage("Please Select Allowance / Dedcution Name", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    cont.Name = ddlallDed.SelectedValue;
                }
                if (txtSize.Text == "")
                {
                    ucMessage.ShowMessage("Please Insert Size", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    cont.size = Convert.ToInt32(txtSize.Text);
                }
                if (txtfrom.Text == "")
                {
                    ucMessage.ShowMessage("Please Insert From Date", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    cont.fromd = Convert.ToDateTime(txtfrom.Text);
                }
                if (txtTo.Text == "")
                {
                    ucMessage.ShowMessage("Please Insert To Date", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    cont.tod = Convert.ToDateTime(txtTo.Text.Trim());
                }
                if (CheckIsActive.Checked == true)
                {
                    cont.isActive = true;
                }
                else
                {
                    cont.isActive = false;
                }
                if (Convert.ToBoolean(checkIsPercen.Checked))
                {
                    cont.isValue = Convert.ToBoolean(0);
                }
                else
                {
                    cont.isValue = Convert.ToBoolean(1);
                }
                add.Update(cont, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Updated Succesfully", BL.Enums.MessageType.Info);
            }
           
            BindGrid(BranchID, IsSearch);
            clearfield();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ID = 0;
            this.ddlEmployee.SelectedValue = "0";
            this.ddlAllDd.SelectedValue = "0";
            //txtadd.Text = "";
            txtfrom.Text = "";
            txtTo.Text = "";
            txtSize.Text = "";
            ddlallDed.SelectedValue = "0";
        }

        protected void grdAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdAddtional.SelectedValue);

            tblPlAddtionalAllDd addTab = new tblPlAddtionalAllDd();
            addTab = add.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlEmployee.SelectedValue = addTab.EmpID.ToString();
            ddlAllDd.SelectedValue = addTab.SalaryContentTypeID.ToString();
            ddlallDed.SelectedValue = addTab.Name.ToString();
            txtSize.Text = addTab.size.ToString();
            if (Session["DateFullYearFormat"] == null)
            {
                this.txtfrom.Text = addTab.fromd.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            else
            {
                this.txtfrom.Text = addTab.fromd.Value.ToString(Session["DateFullYearFormat"].ToString());
            }
            if (Session["DateFullYearFormat"] == null)
            {
                this.txtTo.Text = addTab.tod.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            else
            {
                this.txtTo.Text = addTab.tod.Value.ToString(Session["DateFullYearFormat"].ToString());
            }
            //txtTo.Text = addTab.tod.ToString();
            this.CheckIsActive.Checked =Convert.ToBoolean(addTab.isActive);
            if (addTab.isValue == false)
            {
                this.checkIsPercen.Checked = true;
            }
            else
            {
                this.checkIsPercen.Checked = false;
            }
        }

        protected void grdAdd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAddtional.PageIndex = e.NewPageIndex;
            BindGrid(BranchID, IsSearch);
        }
        protected void grdAddtional_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[5].Text = Convert.ToDateTime(e.Row.Cells[5].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                //e.Row.Cells[0].Text = Counter.ToString();
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


        public void BindGrid(int brID, bool isSearch)
        {
            
            this.grdAddtional.DataSource = add.GetAll(brID, isSearch,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdAddtional.DataBind();
        }

        public void clearfield()
        {
            ID = 0;
            
            ddlAllDd.SelectedValue = "0";
            ddlEmployee.SelectedValue = "0";
           // txtadd.Text = "";
            txtSize.Text = "";
            txtfrom.Text = "";
            txtTo.Text = "";
            checkIsPercen.Checked = false;
            CheckIsActive.Checked = true;
            ddlallDed.SelectedValue = "0";
        }
    }
}
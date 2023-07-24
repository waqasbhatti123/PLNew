using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.profile
{
    public partial class Employee : System.Web.UI.Page
    {
        EmpBL empManager = new EmpBL();
        EmpTransferBL empTBL = new EmpTransferBL();
        EmpProfRptBL empProfRptBL = new EmpProfRptBL();
        ListItem selList = new ListItem();
        ListItem selListSub = new ListItem();

        RMSDataContext data = new RMSDataContext();
        public int ID
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
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

        public static int IsBranch
        {
            get; set;
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Employee").ToString();
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }

                if (Session["BranchID"] == null)
                {
                    
                    IsBranch = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    IsBranch = Convert.ToInt32(Session["BranchID"].ToString());
                }

                FillDropDownBanks();
                FillDropdownPersonal();
                FillDropdownEmployeee();
                BindGrid("", "", BranchID, IsSearch);
            }
        }



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

        protected void grdEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdEmps.SelectedValue);
            tblPlEmpData emp = data.tblPlEmpDatas.Where(x => x.EmpID == ID).FirstOrDefault();
            txtEmpCode.Text = emp.EmpCode.ToString();
            txtFullName.Text = emp.FullName;
            txtFatherName.Text = emp.FatherName;
            txtBankAcct.Text = emp.AccountNo;
            txtBankBranch.Text = emp.Branch;
            ddlBank.SelectedValue = emp.Bank;
            txtsortRefe.Text = emp.sortRef.ToString();
        }




        protected void grdEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEmps.PageIndex = e.NewPageIndex;
           // BindGrid();

            BindGrid("", "" , BranchID, IsSearch);
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            tblPlEmpData emp = data.tblPlEmpDatas.Where(x => x.EmpID == ID).FirstOrDefault();
            emp.CompID = 1;
            emp.DeptID = emp.DeptID;
            emp.DesigID = emp.DesigID;
            emp.RegID = emp.RegID;
            emp.DivID = emp.DivID;
            emp.SectID = emp.SectID;
            emp.CityID = emp.CityID;
            emp.LocID = emp.LocID;
            emp.EmpCode = txtEmpCode.Text;
            emp.ContractNo = emp.ContractNo;
            emp.FullName = txtFullName.Text;
            emp.EmpAdd1 = emp.EmpAdd1;
            emp.EmpAdd2 = emp.EmpAdd2;
            emp.NIC = emp.NIC;
            emp.DOB = emp.DOB;
            emp.DOJ = emp.DOJ;
            emp.DOL = emp.DOL;
            emp.TelNo = emp.TelNo;
            emp.MobNo = emp.MobNo;
            emp.DOC = emp.DOC;
            emp.CreatedOn = emp.CreatedOn;
            emp.EmpStatus = emp.EmpStatus;
            emp.NTN = emp.NTN;
            emp.CreatedBy = emp.CreatedBy;
            emp.Bank = ddlBank.SelectedValue;
            emp.Branch = txtBankBranch.Text;
            if (txtBankAcct.Text == "")
            {
                emp.AccountNo = "";
            }
            else
            {
                emp.AccountNo = txtBankAcct.Text;
            }
            emp.UpdateOn = emp.UpdateOn;
            emp.FatherName = txtFatherName.Text;
            emp.UpdateBy = emp.UpdateBy;
            emp.MotherName = emp.MotherName;
            emp.Grade = emp.Grade;
            emp.Sex = emp.Sex;
            emp.MarStatus = emp.MarStatus;
            emp.EmpPic = emp.EmpPic;
            emp.Email = emp.Email;
            emp.SonCount = emp.SonCount;
            emp.DauughterCount = emp.DauughterCount;
            emp.Education = emp.Education;
            emp.NICIssueDate = emp.NICIssueDate;
            emp.NICExpiryDate = emp.NICExpiryDate;
            emp.EobiNo = emp.EobiNo;
            emp.ScsiNo = emp.ScsiNo;
            emp.ScsiEnb = emp.ScsiEnb;
            emp.HlthInsEnb = emp.HlthInsEnb;
            emp.Religion = emp.Religion;
            emp.jobtype = emp.jobtype;
            emp.EobiEnb = emp.EobiEnb;
            emp.ScaleID = emp.ScaleID;
            emp.BranchID = emp.BranchID;
            emp.DistricID = emp.DistricID;
            emp.JobNameID = emp.JobNameID;
            emp.Domicil = emp.Domicil;
            emp.HelInNo = emp.HelInNo;
            emp.DepEnb = emp.DepEnb;
            emp.DepNo = emp.DepNo;
            emp.CNICAttach = emp.CNICAttach;
            emp.AppAttach = emp.AppAttach;
            emp.OrderAttach = emp.OrderAttach;
            emp.polveri = emp.polveri;
            emp.Mediveri = emp.Mediveri;
            emp.Appointed = emp.Appointed;
            emp.AddtionalCharg = emp.AddtionalCharg;
            emp.Quota = emp.Quota;
            emp.Disbality = emp.Disbality;
            if (txtsortRefe.Text != null || txtsortRefe.Text != "")
            {
                emp.sortRef = Convert.ToInt32(txtsortRefe.Text);
            }
            else
            {
                emp.sortRef = emp.sortRef;
            }
            emp.Degveri = emp.Degveri;
            emp.ReguAttachement = emp.ReguAttachement;
            emp.apposcal = emp.apposcal;
            emp.LastperDes = emp.LastperDes;
            emp.addchargePalce = emp.addchargePalce;
            emp.AddtionalPost = emp.AddtionalPost;
            emp.AddtionalPlace = emp.AddtionalPlace;
            emp.AppointJobType = emp.AppointJobType;
            data.SubmitChanges();
            ClearForm();
            BindGrid("", "", BranchID, IsSearch);
        }


        private void FillDropDownBanks()
        {
            ddlBank.DataTextField = "BankABv";
            ddlBank.DataValueField = "BankCode";
            ddlBank.DataSource = new BankBL().GetAll(BranchID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlBank.DataBind();
        }

        protected void BindGrid(string empName, string empNo, int branchID, bool isSearch)
        {
            this.grdEmps.DataSource = empManager.GetAll(empName, empNo, branchID, isSearch, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            this.grdEmps.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RMSDataContext db = new RMSDataContext();
            IsSearch = true;
            string empName = txtEmpSearch.Text; //Convert.ToInt32(ddlEmpDrpdwn.SelectedValue);
            string Personal = "";//ddlperson.SelectedValue;
            
                if (empName != "" || empName != null)
                {
                    grdEmps.DataSource = from emp in db.tblPlEmpDatas
                                         where emp.FullName == empName
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

            grdEmps.DataBind();

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<sp_GetEmployeeSearchResult> GetEmployee(string employee)
        {
            EmpProfileBL pro = new EmpProfileBL();
            RMSDataContext db = new RMSDataContext();
            
            List<sp_GetEmployeeSearchResult> emp = pro.GetEmployeeSearch(IsBranch, employee, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
            return emp;
        }


        //protected void ddlEmpDrpdown_change(object sender, EventArgs e)
        //{
        //    using (RMSDataContext db = new RMSDataContext())
        //    {
        //        int emppp = Convert.ToInt32(ddlEmpDrpdwn.SelectedValue);
        //        string empref = db.tblPlEmpDatas.Where(x => x.EmpID == emppp).FirstOrDefault().EmpCode;
        //        ddlperson.SelectedValue = empref;
        //    }
        //}

        //protected void ddlPersonal_change(object sender, EventArgs e)
        //{
        //    using (RMSDataContext db = new RMSDataContext())
        //    {
        //        string emppp = ddlperson.SelectedValue;
        //        int empref = db.tblPlEmpDatas.Where(x => x.EmpCode == emppp).FirstOrDefault().EmpID;
        //        ddlEmpDrpdwn.SelectedValue = empref.ToString();
        //    }
        //}

        protected void FillDropdownEmployeee()
        {
            //RMSDataContext db = new RMSDataContext();
            //ddlEmpDrpdwn.DataTextField = "FullName";
            //ddlEmpDrpdwn.DataValueField = "EmpID";
            //ddlEmpDrpdwn.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.BranchID == BranchID && x.BranchID != null && x.BranchID != 14).ToList();
            //ddlEmpDrpdwn.DataBind();
            //ddlEmpDrpdwn.Items.Insert(0, new ListItem("Select", "0"));
        }

        protected void FillDropdownPersonal()
        {
            //RMSDataContext db = new RMSDataContext();
            //ddlperson.DataTextField = "EmpCode";
            //ddlperson.DataValueField = "EmpCode";
            //ddlperson.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.BranchID == BranchID && x.BranchID != null && x.BranchID != 14).ToList();
            //ddlperson.DataBind();
            //ddlperson.Items.Insert(0, new ListItem("Select", "0"));
        }

        public void ClearForm()
        {
            txtBankAcct.Text = "";
            txtBankBranch.Text = "";
            txtEmpCode.Text = "";
            txtFatherName.Text = "";
            ddlBank.SelectedValue = "0";
            txtFullName.Text = "";
            txtsortRefe.Text = "";
            //ddlEmpDrpdwn.SelectedValue = "0";
            //ddlperson.SelectedValue = "0";
        }

    }
}
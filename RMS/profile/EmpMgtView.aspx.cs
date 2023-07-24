using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace RMS.Profile
{
    public partial class EmpMgtView : BasePage
    {

        #region DataMembers
        //RMS.BL.tblAppEmp usr;

        //GroupBL groupManager = new GroupBL();
        
        
        EmpBL empManager = new EmpBL();
        

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'EmpMgtView.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpMgtView.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        #endregion

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Emp").ToString();
                if (Session["EmpID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    ID = Convert.ToInt32(Request.Cookies["uzr"]["EmpID"]);
                }
                else
                {
                    ID = Convert.ToInt32(Session["EmpID"].ToString());
                }
                GetByID();
            }
        }

       
        protected void GetByID()
        {
            tblPlEmpData empPojo = empManager.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //btnView_Transfer.Visible = true;
            this.lblEmpNoView.Text = empPojo.EmpCode.ToString();
            this.lblFullNameView.Text = empPojo.FullName;
            // this.txtMidName.Text = empPojo.MidName;
            // this.txtSirName.Text = empPojo.SirName;

            this.lblGenderView.Text = empPojo.Sex.ToString();
            this.lblMarView.Text = empPojo.MarStatus.ToString();
            if (empPojo.tblPlCode3 != null)
            {
                this.lblRegionView.Text = empPojo.tblPlCode3.CodeDesc;
            }
            else
            {
                lblRegionView.Text = "";
            }
            if (empPojo.tblPlCode2 != null)
            {
                this.lblDivisionView.Text = empPojo.tblPlCode2.CodeDesc;
            }
            else
            {
                lblDivisionView.Text = "";
            }

            if (empPojo.tblPlCode != null)
            {
                this.lblDepartmentView.Text = empPojo.tblPlCode.CodeDesc;
            }
            else
            {
                lblDepartmentView.Text = "";
            }

            if (empPojo.tblPlCode1 != null)
            {
                this.lblDesignationView.Text = empPojo.tblPlCode1.CodeDesc;
            }
            else
            {
                lblDesignationView.Text = "";
            }

            if (empPojo.tblPlCode4 != null)
            {
                this.lblSection.Text = empPojo.tblPlCode4.CodeDesc;
            }
            else
            {
                lblSection.Text = "";
            }
            if (empPojo.tblCity != null)
            {
                this.lblCityView.Text = empPojo.tblCity.CityName;
                //FillDropDownLocations();
                if (empPojo.tblPlLocation != null)
                {
                    this.lblLocationView.Text = empPojo.tblPlLocation.LocName;
                }
            }
            else
            {
                lblCityView.Text = "";
                //FillDropDownLocations();
            }

            this.lblFatherNameView.Text = empPojo.FatherName;
            this.lblCnicView.Text = empPojo.NIC;
            this.lblNtnView.Text = empPojo.NTN;
            this.lblPhNoView.Text = empPojo.TelNo;
            this.lblMobileNoView.Text = empPojo.MobNo;
            this.lblCurrentAddressView.Text = empPojo.EmpAdd1;
            
            this.lblPermanentAddressView.Text = empPojo.EmpAdd2;

            this.lblBankNameView.Text = empPojo.Bank;
            this.lblBranchNameView.Text = empPojo.Branch;
            this.lblAccountNoView.Text = empPojo.AccountNo;


            this.lblEobiNoView.Text = empPojo.EobiNo;
            this.lblScsiNoView.Text = empPojo.ScsiNo;
            

            if (empPojo.DOB != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.lblBirthDateView.Text = empPojo.DOB.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.lblBirthDateView.Text = empPojo.DOB.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                lblBirthDateView.Text = "";
            }
            if (empPojo.DOJ != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.lblJoiningDateView.Text = empPojo.DOJ.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.lblJoiningDateView.Text = empPojo.DOJ.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                lblJoiningDateView.Text = "";
            }

            if (empPojo.DOC != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.lblCinfirmDateView.Text = empPojo.DOC.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.lblCinfirmDateView.Text = empPojo.DOC.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                lblCinfirmDateView.Text = "";
            }
            if (empPojo.NICIssueDate != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.lblIssueDateView.Text = empPojo.NICIssueDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.lblIssueDateView.Text = empPojo.NICIssueDate.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                lblIssueDateView.Text = "";
            }
            if (empPojo.NICExpiryDate != null)
            {
                if (Session["DateFullYearFormat"] == null)
                {
                    this.lblExpiryDateView.Text = empPojo.NICExpiryDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    this.lblExpiryDateView.Text = empPojo.NICExpiryDate.Value.ToString(Session["DateFullYearFormat"].ToString());
                }
            }
            else
            {
                lblExpiryDateView.Text = "";
            }

            this.lblEmail.Text = empPojo.Email;
            this.lblEdu.Text = empPojo.Education;
            this.lblScaleView.Text = empPojo.Grade;
            this.lblSonView.Text = empPojo.SonCount;
            this.lblDaughterView.Text = empPojo.DauughterCount;

            imgEmp.ImageUrl = "~/empix/" + empPojo.EmpPic;

        }
        #endregion
    }
}
 
using Microsoft.Reporting.WebForms;
using RMS.BL;
using RMS.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.report
{
    public partial class EmployeeProfileReport : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        EmpProfileBL pro = new EmpProfileBL();


        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

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
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
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
                    txtJoinDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    txtjointoCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    txtJoinDateCal.Format = Session["DateFullYearFormat"].ToString();
                    txtjointoCal.Format = Session["DateFullYearFormat"].ToString();
                }
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "empprofile").ToString();
                FillEmpDropDown();
                FillSearchBranchDropDown();
                FillDropdownDesignation();
                FillToScaleDropDown();
                FillDropdownSections();
                FilljobDownDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
            }
        }

        protected void grdEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        protected void grdEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEmps.PageIndex = e.NewPageIndex;
            if (searchBranchDropDown.SelectedValue != "0")
            {
                int brrr = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                BindGrid(brrr);
            }
            
        }

        protected void EmpRow_Click(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].Text = Convert.ToDateTime(e.Row.Cells[6].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                if (e.Row.Cells[7].Text == "M")
                {
                    e.Row.Cells[7].Text = "Muslim";
                }
                if (e.Row.Cells[7].Text == "C")
                {
                    e.Row.Cells[7].Text = "Christian";
                }
                if (e.Row.Cells[7].Text == "H")
                {
                    e.Row.Cells[7].Text = "Hindu";
                }
                if (e.Row.Cells[7].Text == "N")
                {
                    e.Row.Cells[7].Text = "Non-Muslim";
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            int cmd = Convert.ToInt32(link.CommandArgument);
            int _newtab = cmd;
            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
   string.Format("window.open('EmployeeProfileDetailReport.aspx?ID={0}');", _newtab), true);
            //Response.Redirect("EmployeeProfileDetailReport.aspx?ID=" + Convert.ToInt32(link.CommandArgument));
            //GridViewRow grd = (GridViewRow)(((Control)sender).NamingContainer);
            //DropDownList list = (DropDownList)grd.FindControl("ddlProfile");
            //string profile = list.SelectedItem.Value;
            //LinkButton link = (LinkButton)sender;
            //int empID = Convert.ToInt32(link.CommandArgument);
            //Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            //string rptLogoPath = "";
            //rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            //if (profile == "1")
            //{
            //    IList<ap_EmployeeBasicInfoResult> emp;
            //    emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    IList<sp_EmpEducationResult> edu;
            //    edu = pro.getEmpEdu(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpEducation.rdlc";
            //    ReportDataSource source = new ReportDataSource("rptEmpEducation", edu);
            //    ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            //    viewer.LocalReport.DataSources.Clear();
            //    viewer.LocalReport.DataSources.Add(source);
            //    viewer.LocalReport.DataSources.Add(empsource);
            //}
            //if (profile == "2")
            //{
            //    IList<ap_EmployeeBasicInfoResult> emp;
            //    emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    IList<sp_EmpPriorExperienceResult> exp;
            //    exp = pro.GetEmpPriorExp(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpPriorExp.rdlc";
            //    ReportDataSource source = new ReportDataSource("rptEmpPrior", exp);
            //    ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            //    viewer.LocalReport.DataSources.Clear();
            //    viewer.LocalReport.DataSources.Add(source);
            //    viewer.LocalReport.DataSources.Add(empsource);
            //}
            //if (profile == "3")
            //{
            //    IList<ap_EmployeeBasicInfoResult> emp;
            //    emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    IList<sp_EmpTenureResult> ten;
            //    ten = pro.GetEmpTenureExp(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpTenureExpe.rdlc";
            //    ReportDataSource source = new ReportDataSource("rptEmpTenure", ten);
            //    ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            //    viewer.LocalReport.DataSources.Clear();
            //    viewer.LocalReport.DataSources.Add(source);
            //    viewer.LocalReport.DataSources.Add(empsource);
            //}
            //if (profile == "5")
            //{
            //    IList<ap_EmployeeBasicInfoResult> emp;
            //    emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    IList<sp_EmployeeEnquiryResult> enq;
            //    enq = pro.GetEmpEnq(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpEnquiryReport.rdlc";
            //    ReportDataSource source = new ReportDataSource("rptEmpEnquiry", enq);
            //    ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            //    viewer.LocalReport.DataSources.Clear();
            //    viewer.LocalReport.DataSources.Add(source);
            //    viewer.LocalReport.DataSources.Add(empsource);
            //}
            //if (profile == "6")
            //{
            //    IList<ap_EmployeeBasicInfoResult> emp;
            //    emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            //    IList<sp_EmployeeLitigationResult> lit;
            //    lit = pro.GetEmpLitigation(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpLitigationReport.rdlc";
            //    ReportDataSource source = new ReportDataSource("rptEmpLitigation", lit);
            //    ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            //    viewer.LocalReport.DataSources.Clear();
            //    viewer.LocalReport.DataSources.Add(source);
            //    viewer.LocalReport.DataSources.Add(empsource);
            //}
            //if (profile == "7")
            //{
            //    IList<ap_EmployeeBasicInfoResult> emp;
            //    emp = pro.getEmployeeBasicInfo(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            //    IList<sp_EmpPermotionResult> per;
            //    per = pro.GetEmpPermotion(empID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //    viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpPermotion.rdlc";
            //    ReportDataSource source = new ReportDataSource("rptEmpPer", per);
            //    ReportDataSource empsource = new ReportDataSource("rptEmpBasic", emp);
            //    viewer.LocalReport.DataSources.Clear();
            //    viewer.LocalReport.DataSources.Add(source);
            //    viewer.LocalReport.DataSources.Add(empsource);
            //}
            //ReportParameter[] paramz = new ReportParameter[3];
            //if (Session["CompName"] == null)
            //{
            //    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            //}
            //else
            //{
            //    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            //}
            //paramz[1] = new ReportParameter("LogoPath", rptLogoPath);
            //paramz[2] = new ReportParameter("Div", br.br_nme);
            //viewer.LocalReport.EnableExternalImages = true;
            //viewer.LocalReport.Refresh();
            //viewer.LocalReport.SetParameters(paramz);



        }

        private void FillSearchBranchDropDown()
        {


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

        protected void FillEmpDropDown()
        {
            ddlScale.DataTextField = "ScaleName";
            ddlScale.DataValueField = "ScaleID";
            ddlScale.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.ScaleID);
            ddlScale.DataBind();
        }
        protected void FillToScaleDropDown()
        {
            ddlTOScale.DataTextField = "ScaleName";
            ddlTOScale.DataValueField = "ScaleID";
            ddlTOScale.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.ScaleID);
            ddlTOScale.DataBind();
        }
        protected void FilljobDownDropDown()
        {
            ddlJobType.DataTextField = "JobTypeName1";
            ddlJobType.DataValueField = "JobNameID";
            ddlJobType.DataSource = db.JobTypeNames.Where(x => x.IsActive == true).ToList();
            ddlJobType.DataBind();
        }
        protected void FillDropdownDesignation()
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
            ddlDesg.DataTextField = "CodeDesc";
            ddlDesg.DataValueField = "CodeID";
            ddlDesg.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 4 && x.Enabled == true && x.CompID == _cmp).ToList();
            ddlDesg.DataBind();
        }

        protected void FillDropdownSections()
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
            ddlsection.DataTextField = "CodeDesc";
            ddlsection.DataValueField = "CodeID";
            ddlsection.DataSource = db.tblPlCodes.Where(x => x.CodeTypeID == 3 && x.Enabled == true && x.CompID == _cmp).ToList();
            ddlsection.DataBind();
        }

        protected void BindGrid(int brr)
        {
            var emp = (from em in db.tblPlEmpDatas
                       join c in db.tblPlCodes on em.DesigID equals c.CodeID
                       join cs in db.tblPlCodes on em.DeptID equals cs.CodeID
                       join sca in db.TblEmpScales on em.ScaleID equals sca.ScaleID
                       join job in db.JobTypeNames on em.JobNameID equals job.JobNameID
                       join br in db.Branches on em.BranchID equals br.br_id
                       where em.BranchID == brr && em.EmpStatus == 1
                       select new {

                           em.EmpID,
                           em.DeptID,
                           em.DesigID,
                           em.DOB,
                           em.BranchID,
                           em.JobNameID,
                           em.Religion,
                           em.ScaleID,
                           em.Domicil,
                           em.Sex,
                           em.polveri,
                           em.DOJ,
                           em.AddtionalCharg,
                           c.CodeDesc,
                           em.FullName,
                           sca.ScaleName,
                           job.JobTypeName1,
                           br.br_nme,
                           em.Disbality,
                           em.Quota
                       }).ToList();
            var list = new List<EmployeeProfileSummary>();

            foreach (var item in emp)
            {
                var e = new EmployeeProfileSummary();
                e.ID = item.EmpID;
                e.FullName = item.FullName;
                e.designation = item.DesigID;
                e.Des = item.CodeDesc;
                e.Section = item.DeptID;
                e.Age = (DateTime.Now.Year - Convert.ToDateTime(item.DOB).Year) - 1;
                e.br = item.BranchID;
                e.brName = item.br_nme;
                e.JobType = item.JobNameID;
                e.Religion = item.Religion;
                e.scale = item.ScaleID;
                e.Domicil = item.Domicil;
                e.Gender = item.Sex;
                e.police = item.polveri;
                e.JoinDate = item.DOJ;
                e.AddtionCharg = item.AddtionalCharg;
                e.ScaleName = item.ScaleName;
                e.JobeTypeName = item.JobTypeName1;
                e.Disablity = item.Disbality;
                e.Quota = item.Quota;
                list.Add(e);
            }

            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue !="0" && ddlScale.SelectedValue != "0" && ddlTOScale.SelectedValue != "0" &&
                ddlJobType.SelectedValue != "0" && ddlDomicile.SelectedValue != "0" && searchBranchDropDown.SelectedValue != "0" && ddlGender.SelectedValue != "0" &&
                ddlReligion.SelectedValue != "0" && ddlAddionReport.SelectedValue != "0" && ddlFromAge.SelectedValue != "0" && ddlToAge.SelectedValue != "0" &&
                txtJoinDate.Text != "" && txtjointo.Text != "" && ddlPoliceVerifi.SelectedValue != "0" && ddlDisablity.SelectedValue != "0" && ddlQuota.SelectedValue != "0")
            {
                grdEmps.DataSource = list.Where(x => x.designation == Convert.ToInt32(ddlDesg.SelectedValue) && x.Section == Convert.ToInt32(ddlsection.SelectedValue)
                                    && x.scale >= Convert.ToInt32(ddlScale) && x.scale <= Convert.ToInt32(ddlTOScale.SelectedValue) && x.JobType == Convert.ToInt32(ddlJobType.SelectedValue) &&
                                    x.Domicil == ddlDomicile.SelectedValue && x.br == Convert.ToInt32(searchBranchDropDown.SelectedValue) && x.Gender == Convert.ToChar(ddlGender.SelectedValue) &&
                                    x.Religion == Convert.ToChar(ddlReligion.SelectedValue) && x.AddtionCharg == ddlAddionReport.SelectedValue && x.Age >= Convert.ToInt32(ddlFromAge.SelectedValue) &&
                                    x.Age <= Convert.ToInt32(ddlToAge.SelectedValue) && x.JoinDate >= Convert.ToDateTime(txtJoinDate.Text) && x.JoinDate <= Convert.ToDateTime(txtjointo.Text) && x.police == Convert.ToBoolean(ddlPoliceVerifi.SelectedValue)).ToList();

            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0"  && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
            }
            if (ddlsection.SelectedValue != "0" &&ddlDesg.SelectedValue == "0"   && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue != "0" && ddlTOScale.SelectedValue != "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.scale >= Convert.ToInt32(ddlScale.SelectedValue) && x.scale <= Convert.ToInt32(ddlTOScale.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue != "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.JobType == Convert.ToInt32(ddlJobType.SelectedValue) ).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue != "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Domicil == ddlDomicile.SelectedValue).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue != "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Gender == Convert.ToChar(ddlGender.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue != "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlReligion.SelectedValue == "M")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion == 'M').ToList();
                }
                else
                {
                    grdEmps.DataSource = list.Where(x => x.Religion != 'M').ToList();
                }
                
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue != "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlAddionReport.SelectedValue == "Yes")
                {
                    grdEmps.DataSource = list.Where(x => x.AddtionCharg != null).ToList();
                }
                else
                {
                    grdEmps.DataSource = list.Where(x => x.AddtionCharg == null).ToList();
                }
                
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue != "0" && ddlToAge.SelectedValue != "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Age >= Convert.ToInt32(ddlFromAge.SelectedValue) && x.Age <= Convert.ToInt32(ddlToAge.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text != "" && txtjointo.Text != "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.JoinDate >= Convert.ToDateTime(txtJoinDate.Text) && x.JoinDate <= Convert.ToDateTime(txtjointo.Text)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue != "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.police == Convert.ToBoolean(ddlPoliceVerifi.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue != "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlDisablity.SelectedValue == "Yes")
                {
                    grdEmps.DataSource = list.Where(x => x.Disablity != null).ToList();
                }
                else
                {
                    grdEmps.DataSource = list.Where(x => x.Disablity == null).ToList();
                }
                
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue != "0")
            {
                grdEmps.DataSource = list.Where(x => x.Quota == ddlQuota.SelectedValue).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.designation == Convert.ToInt32(ddlDesg.SelectedValue) && 
                                                x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue != "0" && ddlDomicile.SelectedValue != "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.JobType == Convert.ToInt32(ddlJobType.SelectedValue)
                                                && x.Domicil == ddlDomicile.SelectedValue).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue != "0" && ddlAddionReport.SelectedValue != "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlReligion.SelectedValue == "M" && ddlAddionReport.SelectedValue == "Yes")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion == 'M' && x.AddtionCharg != null).ToList();
                }
                else if (ddlReligion.SelectedValue != "M" && ddlAddionReport.SelectedValue == "Yes")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion != 'M' && x.AddtionCharg != null).ToList();
                }
                else if (ddlReligion.SelectedValue == "M" && ddlAddionReport.SelectedValue != "Yes")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion == 'M' && x.AddtionCharg == null).ToList();
                }
                else if(ddlReligion.SelectedValue != "M" && ddlAddionReport.SelectedValue != "Yes")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion != 'M' && x.AddtionCharg == null).ToList();
                }
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue != "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue != "0")
            {
                grdEmps.DataSource = list.Where(x => x.police == Convert.ToBoolean(ddlPoliceVerifi.SelectedValue) && x.Quota == ddlQuota.SelectedValue).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue != "0" && ddlTOScale.SelectedValue != "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.designation == Convert.ToInt32(ddlDesg.SelectedValue) && x.scale >= Convert.ToInt32(ddlScale.SelectedValue) 
                                                && x.scale <= Convert.ToInt32(ddlTOScale.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue != "0" && ddlDomicile.SelectedValue != "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.JobType == Convert.ToInt32(ddlJobType.SelectedValue)
                                                && x.Domicil == ddlDomicile.SelectedValue && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue != "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x =>  x.Gender == Convert.ToChar(ddlGender.SelectedValue) && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue != "0" && ddlAddionReport.SelectedValue != "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlReligion.SelectedValue == "M" && ddlAddionReport.SelectedValue == "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion == 'M' && x.AddtionCharg != null && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList(); 
                }
                else if (ddlReligion.SelectedValue != "M" && ddlAddionReport.SelectedValue == "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion != 'M' && x.AddtionCharg != null && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
                }
                else if (ddlReligion.SelectedValue == "M" && ddlAddionReport.SelectedValue != "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion == 'M' && x.AddtionCharg == null && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
                }
                else if (ddlReligion.SelectedValue != "M" && ddlAddionReport.SelectedValue != "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion != 'M' && x.AddtionCharg == null && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
                }
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue != "0" && ddlToAge.SelectedValue != "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Age >= Convert.ToInt32(ddlFromAge.SelectedValue) && x.Age <= Convert.ToInt32(ddlToAge.SelectedValue)
                                                && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text != "" && txtjointo.Text != "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.JoinDate >= Convert.ToDateTime(txtJoinDate.Text) && x.JoinDate <= Convert.ToDateTime(txtjointo.Text)
                                                && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue != "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue != "0")
            {
                grdEmps.DataSource = list.Where(x => x.police == Convert.ToBoolean(ddlPoliceVerifi.SelectedValue) && x.Quota == ddlQuota.SelectedValue
                                                && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue != "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlDisablity.SelectedValue == "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Disablity != null && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
                }
                else if(ddlDisablity.SelectedValue != "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Disablity == null && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
                }

            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue != "0" && ddlTOScale.SelectedValue != "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Section == Convert.ToInt32(ddlsection.SelectedValue) && x.scale >= Convert.ToInt32(ddlScale.SelectedValue)
                                                && x.scale <= Convert.ToInt32(ddlTOScale.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
              ddlJobType.SelectedValue != "0" && ddlDomicile.SelectedValue != "0" && ddlGender.SelectedValue == "0" &&
              ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
              txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.JobType == Convert.ToInt32(ddlJobType.SelectedValue)
                                                && x.Domicil == ddlDomicile.SelectedValue && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue != "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Gender == Convert.ToChar(ddlGender.SelectedValue) && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue != "0" && ddlAddionReport.SelectedValue != "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlReligion.SelectedValue == "M" && ddlAddionReport.SelectedValue == "Yes" && ddlsection.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion == 'M' && x.AddtionCharg != null && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }
                else if (ddlReligion.SelectedValue != "M" && ddlAddionReport.SelectedValue == "Yes" && ddlsection.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion != 'M' && x.AddtionCharg != null && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }
                else if (ddlReligion.SelectedValue == "M" && ddlAddionReport.SelectedValue != "Yes" && ddlsection.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion == 'M' && x.AddtionCharg == null && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }
                else if (ddlReligion.SelectedValue != "M" && ddlAddionReport.SelectedValue != "Yes" && ddlsection.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion != 'M' && x.AddtionCharg == null && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue != "0" && ddlToAge.SelectedValue != "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Age >= Convert.ToInt32(ddlFromAge.SelectedValue) && x.Age <= Convert.ToInt32(ddlToAge.SelectedValue)
                                                && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text != "" && txtjointo.Text != "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.JoinDate >= Convert.ToDateTime(txtJoinDate.Text) && x.JoinDate <= Convert.ToDateTime(txtjointo.Text)
                                                && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue != "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue != "0")
            {
                grdEmps.DataSource = list.Where(x => x.police == Convert.ToBoolean(ddlPoliceVerifi.SelectedValue) && x.Quota == ddlQuota.SelectedValue
                                                && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue != "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.designation == Convert.ToInt32(ddlDesg.SelectedValue) && x.JobType == Convert.ToInt32(ddlJobType.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue != "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.designation == Convert.ToInt32(ddlDesg.SelectedValue) && x.Domicil == ddlDomicile.SelectedValue).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue != "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlReligion.SelectedValue == "M" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion == 'M' && x.designation ==Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
                }
                else if(ddlReligion.SelectedValue != "M" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion != 'M' && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
                }
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue != "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlAddionReport.SelectedValue == "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.AddtionCharg != null && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
                }
                else if (ddlAddionReport.SelectedValue != "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.AddtionCharg == null  && x.designation == Convert.ToInt32(ddlDesg.SelectedValue)).ToList();
                }
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue != "0")
            {
                grdEmps.DataSource = list.Where(x => x.designation == Convert.ToInt32(ddlDesg.SelectedValue) && x.Quota == ddlQuota.SelectedValue).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue != "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.designation == Convert.ToInt32(ddlDesg.SelectedValue) && x.police == Convert.ToBoolean(ddlPoliceVerifi.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue != "0" && ddlsection.SelectedValue == "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue != "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.designation == Convert.ToInt32(ddlDesg.SelectedValue) && x.police == Convert.ToBoolean(ddlPoliceVerifi.SelectedValue)).ToList();
            }

            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue != "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Section == Convert.ToInt32(ddlsection.SelectedValue) && x.JobType == Convert.ToInt32(ddlJobType.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue != "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Section == Convert.ToInt32(ddlsection.SelectedValue) && x.Domicil == ddlDomicile.SelectedValue).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue != "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Section == Convert.ToInt32(ddlsection.SelectedValue) && x.Gender == Convert.ToChar(ddlGender.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
              ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
              ddlReligion.SelectedValue != "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
              txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlReligion.SelectedValue == "M" && ddlsection.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion == 'M' && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }
                else if (ddlReligion.SelectedValue != "M" && ddlsection.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Religion != 'M' && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
              ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
              ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue != "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
              txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlAddionReport.SelectedValue == "Yes" && ddlsection.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.AddtionCharg != null && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }
                else if (ddlAddionReport.SelectedValue != "Yes" && ddlsection.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.AddtionCharg == null && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue != "0" && ddlToAge.SelectedValue != "0" &&
                txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Age >= Convert.ToInt32(ddlFromAge.SelectedValue) && x.Age <= Convert.ToInt32(ddlToAge.SelectedValue)
                                                && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
                ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
                ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
                txtJoinDate.Text != "" && txtjointo.Text != "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.JoinDate >= Convert.ToDateTime(txtJoinDate.Text) && x.JoinDate <= Convert.ToDateTime(txtjointo.Text)
                                                && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue != "0")
            {
                grdEmps.DataSource = list.Where(x => x.Section == Convert.ToInt32(ddlsection.SelectedValue) && x.Quota == ddlQuota.SelectedValue).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue != "0" && ddlDisablity.SelectedValue == "0" && ddlQuota.SelectedValue == "0")
            {
                grdEmps.DataSource = list.Where(x => x.Section == Convert.ToInt32(ddlsection.SelectedValue) && x.police == Convert.ToBoolean(ddlPoliceVerifi.SelectedValue)).ToList();
            }
            if (ddlDesg.SelectedValue == "0" && ddlsection.SelectedValue != "0" && ddlScale.SelectedValue == "0" && ddlTOScale.SelectedValue == "0" &&
               ddlJobType.SelectedValue == "0" && ddlDomicile.SelectedValue == "0" && ddlGender.SelectedValue == "0" &&
               ddlReligion.SelectedValue == "0" && ddlAddionReport.SelectedValue == "0" && ddlFromAge.SelectedValue == "0" && ddlToAge.SelectedValue == "0" &&
               txtJoinDate.Text == "" && txtjointo.Text == "" && ddlPoliceVerifi.SelectedValue == "0" && ddlDisablity.SelectedValue != "0" && ddlQuota.SelectedValue == "0")
            {
                if (ddlDisablity.SelectedValue == "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Disablity != null && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }
                else if (ddlDisablity.SelectedValue != "Yes" && ddlDesg.SelectedValue != "0")
                {
                    grdEmps.DataSource = list.Where(x => x.Disablity == null && x.Section == Convert.ToInt32(ddlsection.SelectedValue)).ToList();
                }

            }
            grdEmps.DataBind();


        }

        

        protected void genrate_Click(object sender, EventArgs e)
        {
            if (searchBranchDropDown.SelectedValue != "0")
            {
                int brr = Convert.ToInt32(searchBranchDropDown.SelectedValue);
                BindGrid(brr);
            }
            

        }

        //public void CreatePdf()
        //{
        //    Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
        //    int Scale = Convert.ToInt32(ddlScale.SelectedValue);
        //    int scaleto = Convert.ToInt32(ddlTOScale.SelectedValue);


        //    string logoFile = "";
        //    logoFile = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
        //    IList<SpProfileEducationResult> edu = pro.getEdu(1, BranchID, 0, Scale, scaleto, "", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    IList<SpProfileExperienceResult> exp = pro.getPriorExpe(1, BranchID, 0, Scale, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    IList<SpProfileGetAllEmployeesResult> emp = pro.getEmployees((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    IList<SpProfileTenureExperienceResult> TExp = pro.getTenureExpe(1, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    IList<SpProfileAcrResult> Acr = pro.getAcrRecord(1, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    IList<SpProfileEnquiryResult> enq = pro.getEnquiryRecord(1, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    IList<SpProfileLitigationResult> lit = pro.getLitigationRecord(1, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    IList<SpProfilePromotionResult> per = pro.getPermotion(1, BranchID, 0, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    viewer.LocalReport.ReportPath = "report/rdlc/rdlcEmpProfilecomplete.rdlc";
        //    viewer.LocalReport.Refresh();
        //    viewer.LocalReport.EnableExternalImages = true;

        //    // ReportDataSource empData = new ReportDataSource("spEmployees", emp);

        //    ReportDataSource empSource = new ReportDataSource("spEmployees", emp);
        //    ReportDataSource EduSource = new ReportDataSource("spEmpEducation", edu);
        //    ReportDataSource ExpSource = new ReportDataSource("spEmpPriorExp", exp);
        //    ReportDataSource texpSource = new ReportDataSource("spEmpTenureExp", TExp);
        //    ReportDataSource acrSource = new ReportDataSource("spEmpAcr", Acr);
        //    ReportDataSource enqSource = new ReportDataSource("spEmpEnquiry", enq);
        //    ReportDataSource litSource = new ReportDataSource("spEmpLitigation", lit);
        //    ReportDataSource perSource = new ReportDataSource("spEmpPermotion", per);

        //    ReportParameter[] rpt = new ReportParameter[3];
        //    rpt[0] = new ReportParameter("LogoFile", logoFile);
        //    if (Session["CompName"] == null)
        //    {
        //        rpt[1] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString());
        //    }
        //    else
        //    {
        //        rpt[1] = new ReportParameter("CompName", Session["CompName"].ToString());
        //    }
        //    rpt[2] = new ReportParameter("Div", br.br_nme);

        //    viewer.LocalReport.SetParameters(rpt);


        //    viewer.LocalReport.DataSources.Clear();
        //    viewer.LocalReport.DataSources.Add(empSource);
        //    viewer.LocalReport.DataSources.Add(EduSource);

        //    viewer.LocalReport.DataSources.Add(ExpSource);
        //    viewer.LocalReport.DataSources.Add(texpSource);
        //    viewer.LocalReport.DataSources.Add(acrSource);
        //    viewer.LocalReport.DataSources.Add(enqSource);
        //    viewer.LocalReport.DataSources.Add(litSource);
        //    viewer.LocalReport.DataSources.Add(perSource);


        //    //viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(EduSubReportProcessing);
        //    //viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ExperienceSubReportProcessing);
        //}
        //private void EduSubReportProcessing(object sender, SubreportProcessingEventArgs e)
        //{
        //    int empID = Convert.ToInt32(e.Parameters["EmpID"].Values[0].ToString());
        //    IList<SpProfileEducationResult> edu = pro.getEdu(1, 1, empID, "", "", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ReportDataSource ds = new ReportDataSource("DataSet1", edu);
        //    e.DataSources.Add(ds);
        //}

        //private void ExperienceSubReportProcessing(object sender, SubreportProcessingEventArgs e)
        //{
        //    int empID = Convert.ToInt32(e.Parameters["EmpID"].Values[0].ToString());
        //    IList<SpProfileExperienceResult> exp = pro.getPriorExpe(1, 1, empID, "", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ReportDataSource ds = new ReportDataSource("DataSet1", exp);
        //    e.DataSources.Add(ds);
        //}
    }
}
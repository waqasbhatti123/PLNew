using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.GLSetup
{
    public partial class CouncilCourtCases : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        tblCourtCase court = new tblCourtCase();
        tblCourtCasesDetail courtDel = new tblCourtCasesDetail();
#pragma warning disable CS0114 // 'CouncilCourtCases.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'CouncilCourtCases.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int UpID
        {
            get { return (ViewState["UpID"] == null) ? 0 : Convert.ToInt32(ViewState["UpID"]); }
            set { ViewState["UpID"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = "Arts Council Court Cases";
                if (Session["DateFormat"] == null)
                {
                    txtCaseDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtupdateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtNextHearingCal.Format = Request.Cookies["uzr"]["DateFormat"];

                }
                else
                {
                    txtCaseDateCal.Format = Session["DateFormat"].ToString();
                    txtupdateCal.Format = Session["DateFormat"].ToString();
                    txtNextHearingCal.Format = Session["DateFormat"].ToString();
                }
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                FillDivisionDropDown();
                FillCityDropDown();
                FillCaseDropDown();
                BindGrid();
                updateBindGrid();
                ddldivision.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(ddldivision.SelectedValue);
            }
        }


        protected void Savebtn_Click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                court.BranchId = Convert.ToInt32(ddldivision.SelectedValue);
                court.CaseTitle = txtcasetitle.Text.Trim();
                court.caseDate = Convert.ToDateTime(txtCaseDate.Text.Trim());
                court.CourtDetai = ddlCourtName.SelectedValue;
                court.JudgeName = txtJudgeName.Text.Trim();
                court.partyOne = txtpartyone.Text.Trim();
                court.partyTwo = txtpartytwo.Text.Trim();
                court.CityID = Convert.ToInt32(ddlCity.SelectedValue);
                court.status = ddlStatus.SelectedValue;
                court.Remarks = txtarearemarks.Text.Trim();
                court.LawyerName = txtLawyerName.Text.Trim();
                court.LawyerContact = txtLawyerContact.Text.Trim();
                db.tblCourtCases.InsertOnSubmit(court);
                db.SubmitChanges();
                ucMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                tblCourtCase caseee = db.tblCourtCases.Where(x => x.CourtID == ID).FirstOrDefault();
                caseee.BranchId = Convert.ToInt32(ddldivision.SelectedValue);
                caseee.CaseTitle = txtcasetitle.Text.Trim();
                caseee.caseDate = Convert.ToDateTime(txtCaseDate.Text.Trim());
                caseee.CourtDetai = ddlCourtName.SelectedValue;
                caseee.JudgeName = txtJudgeName.Text.Trim();
                caseee.partyOne = txtpartyone.Text.Trim();
                caseee.partyTwo = txtpartytwo.Text.Trim();
                caseee.CityID = Convert.ToInt32(ddlCity.SelectedValue);
                caseee.status = ddlStatus.SelectedValue;
                caseee.Remarks = txtarearemarks.Text.Trim();
                caseee.LawyerName = txtLawyerName.Text.Trim();
                caseee.LawyerContact = txtLawyerContact.Text.Trim();
                db.SubmitChanges();
                ucMessage.ShowMessage("Update Successfully", BL.Enums.MessageType.Info);
            }
            BindGrid();
            ClearFields();
        }

        protected void Clearbtn_Click(object sender, EventArgs e)
        {
            ID = 0;
            ddldivision.SelectedValue = "0";
            ddlCity.SelectedValue = "0";
            txtcasetitle.Text = "";
            txtCaseDate.Text = "";
            ddlCourtName.SelectedValue = "";
            txtJudgeName.Text = "";
            txtpartyone.Text = "";
            txtpartytwo.Text = "";
            ddlStatus.SelectedValue = "0";
            txtarearemarks.Text = "";
            txtLawyerContact.Text = "";
            txtLawyerName.Text = "";
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            if (UpID == 0)
            {
                courtDel.casetitle = ddlcasetitle.SelectedValue;
                courtDel.DateUpdate = Convert.ToDateTime(txtupdate.Text.Trim());
                courtDel.status = ddlupdStatus.SelectedValue;
                courtDel.Remarks = txtRemarksUpd.Text.Trim();
                courtDel.NextHearing = Convert.ToDateTime(txtNextHearing.Text.Trim());
                db.tblCourtCasesDetails.InsertOnSubmit(courtDel);
                db.SubmitChanges();
                ucMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                tblCourtCasesDetail ccd = db.tblCourtCasesDetails.Where(x => x.CourtDetaiID == UpID).FirstOrDefault();
                ccd.casetitle = ddlcasetitle.SelectedValue;
                ccd.DateUpdate = Convert.ToDateTime(txtupdate.Text.Trim());
                ccd.status = ddlupdStatus.SelectedValue;
                ccd.Remarks = txtRemarksUpd.Text.Trim();
                ccd.NextHearing = Convert.ToDateTime(txtNextHearing.Text.Trim());
                db.SubmitChanges();
                ucMessage.ShowMessage("Update Successfully", BL.Enums.MessageType.Info);
            }
            updateBindGrid();
            ClearFields();
        }

        protected void grdCourt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdCourt.SelectedValue);
            tblCourtCase casee = db.tblCourtCases.Where(x => x.CourtID == ID).FirstOrDefault();
            ddldivision.SelectedValue = casee.BranchId.ToString();
            txtcasetitle.Text = casee.CaseTitle;
            txtCaseDate.Text = casee.caseDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            ddlCourtName.SelectedValue = casee.CourtDetai;
            txtJudgeName.Text = casee.JudgeName;
            txtpartyone.Text = casee.partyOne;
            txtpartytwo.Text = casee.partyTwo;
            ddlCity.SelectedValue = casee.CityID.ToString();
            ddlStatus.SelectedValue = casee.status;
            txtarearemarks.Text = casee.Remarks;
            txtLawyerName.Text = casee.LawyerName;
            txtLawyerContact.Text = casee.LawyerContact;
        }
        protected void grdCourt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCourt.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdCourt_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void grdUpd_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpID = Convert.ToInt32(grdUpd.SelectedValue);
            tblCourtCasesDetail cd = db.tblCourtCasesDetails.Where(x => x.CourtDetaiID == UpID).FirstOrDefault();
            ddlcasetitle.SelectedValue = cd.casetitle;
            txtupdate.Text = cd.DateUpdate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            txtNextHearing.Text = cd.NextHearing.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            ddlupdStatus.SelectedValue = cd.status;
            txtRemarksUpd.Text = cd.Remarks;
        }

        protected void grdUpd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUpd.PageIndex = e.NewPageIndex;
            updateBindGrid();
        }

        protected void grdUpd_rowbound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void FillDivisionDropDown()
        {
            ddldivision.DataTextField = "br_nme";
            ddldivision.DataValueField = "br_id";
            Branch br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            List<Branch> list = new List<Branch>();
            if (br.IsHead == true)
            {
                list = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                list = db.Branches.Where(x => x.br_status == true && x.br_id == BranchID).ToList();
            }

            ddldivision.DataSource = list.ToList();
            ddldivision.DataBind();
            ddldivision.Items.Insert(0, new ListItem("Select Division", "0"));
        }

        protected void FillCityDropDown()
        {
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityID";
            ddlCity.DataSource = db.tblCities.Where(x => x.Enabled == true).ToList();
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("Select City", "0"));
        }

        protected void FillCaseDropDown()
        {
            ddlcasetitle.DataTextField = "CaseTitle";
            ddlcasetitle.DataValueField = "CaseTitle";
            ddlcasetitle.DataSource = db.tblCourtCases.Where(x => x.BranchId == BranchID).ToList();
            ddlcasetitle.DataBind();
            ddlcasetitle.Items.Insert(0, new ListItem("Select Case", "0"));
        }

        protected void BindGrid()
        {
            grdCourt.DataSource = from cou in db.tblCourtCases
                                  join br in db.Branches on cou.BranchId equals br.br_id
                                  join ci in db.tblCities on cou.CityID equals ci.CityID
                                  select new
                                  {
                                      cou.CourtID,
                                      cou.CaseTitle,
                                      cou.caseDate,
                                      cou.CourtDetai,
                                      cou.JudgeName,
                                      cou.partyOne,
                                      cou.partyTwo,
                                      cou.status,
                                      cou.Remarks,
                                      br.br_nme,
                                      ci.CityName
                                  };
            grdCourt.DataBind();
        }

        protected void updateBindGrid()
        {
            grdUpd.DataSource = from up in db.tblCourtCasesDetails
                                select new
                                {
                                    up.casetitle,
                                    up.DateUpdate,
                                    up.CourtDetaiID,
                                    up.status,
                                    up.Remarks
                                };
            grdUpd.DataBind();
        }

        public void ClearFields()
        {
            ID = 0;
            ddldivision.SelectedValue = "0";
            ddlCity.SelectedValue = "0";
            txtcasetitle.Text = "";
            txtCaseDate.Text = "";
            ddlCourtName.SelectedValue = "";
            txtJudgeName.Text = "";
            txtpartyone.Text = "";
            txtpartytwo.Text = "";
            ddlStatus.SelectedValue = "0";
            txtarearemarks.Text = "";
            ddlcasetitle.SelectedValue = "0";
            txtupdate.Text = "";
            ddlupdStatus.SelectedValue = "0";
            txtRemarksUpd.Text = "";
            txtLawyerName.Text = "";
            txtLawyerContact.Text = "";
            txtNextHearing.Text = "";
        }
    }
}
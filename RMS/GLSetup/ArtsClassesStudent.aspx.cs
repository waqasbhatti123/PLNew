using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.GLSetup
{
    public partial class ArtsClassesStudent : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        ArtsClassesStudentCount art = new ArtsClassesStudentCount();

#pragma warning disable CS0114 // 'ArtsClassesStudent.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'ArtsClassesStudent.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Art").ToString();
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                if (Session["DateFullYearFormat"] == null)
                {
                    txtCourceEndCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                    txtcourceStartCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    txtCourceEndCal.Format = Session["DateFullYearFormat"].ToString();
                    txtcourceStartCal.Format = Session["DateFullYearFormat"].ToString();
                }
                FilldivDropDown();
                BindGrid();
            }
        }

        protected void StudentSave_Click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                art.Branch = Convert.ToInt32(ddlDivisional.SelectedValue);
                art.CourseName = txtCourceName.Text.Trim();
                art.TeacherName = txtTeacherName.Text.Trim();
                art.NumofStu = Convert.ToDecimal(txtStudentNum.Text.Trim());
                art.CourseFee = Convert.ToDecimal(txtCourceFee.Text.Trim());
                art.CourseStart = Convert.ToDateTime(txtcourceStart.Text.Trim());
                art.CourseEnd = Convert.ToDateTime(txtCourceEnd.Text.Trim());
                db.ArtsClassesStudentCounts.InsertOnSubmit(art);
                db.SubmitChanges();
                ucMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                ArtsClassesStudentCount clas = db.ArtsClassesStudentCounts.Where(x => x.ArtID == ID).FirstOrDefault();
                clas.Branch = Convert.ToInt32(ddlDivisional.SelectedValue);
                clas.CourseName = txtCourceName.Text.Trim();
                clas.TeacherName = txtTeacherName.Text.Trim();
                clas.NumofStu = Convert.ToDecimal(txtStudentNum.Text.Trim());
                clas.CourseFee = Convert.ToDecimal(txtCourceFee.Text.Trim());
                clas.CourseStart = Convert.ToDateTime(txtcourceStart.Text.Trim());
                clas.CourseEnd = Convert.ToDateTime(txtCourceEnd.Text.Trim());
                db.SubmitChanges();
                ucMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
            }
            BindGrid();
            ClearFields();
            
        }

        protected void grdClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdClasses.SelectedValue);
            ArtsClassesStudentCount artcls = db.ArtsClassesStudentCounts.Where(x => x.ArtID == ID).FirstOrDefault();
            ddlDivisional.SelectedValue = artcls.Branch.ToString();
            txtCourceName.Text = artcls.CourseName.ToString();
            txtTeacherName.Text = artcls.TeacherName.ToString();
            txtStudentNum.Text = artcls.NumofStu.ToString();
            txtCourceFee.Text = artcls.CourseFee.ToString();
            txtcourceStart.Text = artcls.CourseStart.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            txtCourceEnd.Text = artcls.CourseEnd.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
        }

        protected void grdClasses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdClasses.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdClasses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[5].Text = Convert.ToDateTime(e.Row.Cells[5].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void StudentClear_Click(object sender, EventArgs e)
        {
            txtCourceName.Text = "";
            txtTeacherName.Text = "";
            txtStudentNum.Text = "";
            txtCourceFee.Text = "";
            txtcourceStart.Text = "";
            txtCourceEnd.Text = "";
        }


        protected void FilldivDropDown()
        {
            ddlDivisional.DataValueField = "br_id";
            ddlDivisional.DataTextField = "br_nme";
            ddlDivisional.DataSource = db.Branches.Where(x => x.br_id == BranchID).ToList();
            ddlDivisional.DataBind();
        }

        protected void BindGrid()
        {
            grdClasses.DataSource = from ar in db.ArtsClassesStudentCounts
                                    where ar.Branch == BranchID
                                    select new
                                    {
                                        ar.CourseName,
                                        ar.TeacherName,
                                        ar.NumofStu,
                                        ar.CourseFee,
                                        ar.CourseStart,
                                        ar.CourseEnd,
                                        ar.ArtID
                                    };
            grdClasses.DataBind();
        }

        public void ClearFields()
        {
            txtCourceName.Text = "";
            txtTeacherName.Text = "";
            txtStudentNum.Text = "";
            txtCourceFee.Text = "";
            txtcourceStart.Text = "";
            txtCourceEnd.Text = "";
        }
    }
}
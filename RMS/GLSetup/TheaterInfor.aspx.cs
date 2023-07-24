using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.GLSetup
{
    public partial class TheaterInfor : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();

#pragma warning disable CS0114 // 'TheaterInfor.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'TheaterInfor.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        public static decimal Financialyear
        {
            get; set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "theater").ToString();
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
                    txtvalidCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    txtvalidCal.Format = Session["DateFullYearFormat"].ToString();
                }
                FillDivisionDropdown();
                BindGrid();
            }
        }

        protected void Save_click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                tblTheaterInfo th = new tblTheaterInfo();
                th.Branch = Convert.ToInt32(ddlDivisional.SelectedValue);
                th.title = txtTheaterTitle.Text.Trim();
                th.Capacity = Convert.ToInt32(txtCapacity.Text.Trim());
                th.ConPerson = txtconPer.Text.Trim();
                th.Contactno = txtconNo.Text.Trim();
                th.Addresss = txtarearemaks.Text.Trim();
                th.license = txtLicense.Text.Trim();
                th.validDate = Convert.ToDateTime(txtvalid.Text.Trim());
                th.ScuritySystem = Convert.ToBoolean(ddlScurity.SelectedValue);
                db.tblTheaterInfos.InsertOnSubmit(th);
                db.SubmitChanges();
                ucMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                tblTheaterInfo thinfor = db.tblTheaterInfos.Where(x => x.ThID == ID).FirstOrDefault();
                thinfor.Branch = Convert.ToInt32(ddlDivisional.SelectedValue);
                thinfor.title = txtTheaterTitle.Text.Trim();
                thinfor.Capacity = Convert.ToInt32(txtCapacity.Text.Trim());
                thinfor.ConPerson = txtconPer.Text.Trim();
                thinfor.Contactno = txtconNo.Text.Trim();
                thinfor.Addresss = txtarearemaks.Text.Trim();
                thinfor.license = txtLicense.Text.Trim();
                thinfor.validDate = Convert.ToDateTime(txtvalid.Text.Trim());
                thinfor.ScuritySystem = Convert.ToBoolean(ddlScurity.SelectedValue);
                db.SubmitChanges();
                ucMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
            }
            ClearFields();
            BindGrid();
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            ID = 0;
            txtTheaterTitle.Text = "";
            txtCapacity.Text = "";
            txtconPer.Text = "";
            txtconNo.Text = "";
            ddlDivisional.SelectedValue = "0";
            txtarearemaks.Text = "";
            txtvalid.Text = "";
            txtLicense.Text = "";
            ddlScurity.SelectedValue = "true";
        }

        protected void grdtheater_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdTheater.SelectedValue);
            tblTheaterInfo info = db.tblTheaterInfos.Where(x => x.ThID == ID).FirstOrDefault();
            ddlDivisional.SelectedValue = info.Branch.ToString();
            txtTheaterTitle.Text = info.title.ToString();
            txtCapacity.Text = info.Capacity.ToString();
            txtconPer.Text = info.ConPerson.ToString();
            txtconNo.Text = info.Contactno.ToString();
            txtarearemaks.Text = info.Addresss.ToString();
            txtLicense.Text = info.license.ToString();
            txtvalid.Text = info.validDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            ddlScurity.SelectedValue = info.ScuritySystem.ToString();
        }

        protected void grdtheater_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTheater.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdtheater_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void FillDivisionDropdown()
        {
            Branch Br = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
            ddlDivisional.DataTextField = "br_nme";
            ddlDivisional.DataValueField = "br_id";
            if (Br.IsHead == true)
            {
                ddlDivisional.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                ddlDivisional.DataSource = db.Branches.Where(x => x.br_id == BranchID).ToList();
            }
            ddlDivisional.DataBind();
            ddlDivisional.Items.Insert(0, new ListItem("Select Division", "0"));
        }

        protected void BindGrid()
        {
            grdTheater.DataSource = from th in db.tblTheaterInfos
                                    join br in db.Branches on th.Branch equals br.br_id
                                    select new
                                    {
                                        br.br_nme,
                                        th.ThID,
                                        th.title,
                                        th.Capacity,
                                        th.ConPerson,
                                        th.Contactno,
                                        th.Addresss
                                    };
            grdTheater.DataBind();
        }

        public void ClearFields()
        {
            ID = 0;
            txtTheaterTitle.Text = "";
            txtCapacity.Text = "";
            txtconPer.Text = "";
            txtconNo.Text = "";
            ddlDivisional.SelectedValue = "0";
            txtarearemaks.Text = "";
            txtvalid.Text = "";
            txtLicense.Text = "";
            ddlScurity.SelectedValue = "true";
        }
    }
}
using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.GLSetup
{
    public partial class DramaInfo : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();

#pragma warning disable CS0114 // 'DramaInfo.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'DramaInfo.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Drama").ToString();
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                if (Session["DateFormat"] == null)
                {
                    txtDateFromCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtDatetoCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    txtSurintDateCal.Format = Request.Cookies["uzr"]["DateFormat"];

                }
                else
                {
                    txtDateFromCal.Format = Session["DateFormat"].ToString();
                    txtDatetoCal.Format = Session["DateFormat"].ToString();
                    txtSurintDateCal.Format = Session["DateFormat"].ToString();
                }
                FillTheatreDropdown();
                BindGrid();
            }
        }


        protected void Save_click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                tblDramaInfo dr = new tblDramaInfo();
                dr.title = txtDramatitle.Text.Trim();
                dr.DateFrom = Convert.ToDateTime(txtDateFrom.Text.Trim());
                dr.DateTo = Convert.ToDateTime(txtDateto.Text.Trim());
                //dr.Lisence = txtlicense.Text.Trim();
                dr.ScScruName = txtpersonName.Text.Trim();
                dr.ScScruDate = Convert.ToDateTime(txtSurintDate.Text.Trim());
                dr.ScScruFee = Convert.ToInt32(txtFeePaid.Text.Trim());
                dr.branch = BranchID;
                dr.moniName = txtMoniName.Text.Trim();
                dr.moniContact = txtMoniContact.Text.Trim();
                dr.Theaetretile = Convert.ToInt32(ddltheatreName.SelectedValue);
                db.tblDramaInfos.InsertOnSubmit(dr);
                db.SubmitChanges();
                ucMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                tblDramaInfo dram = db.tblDramaInfos.Where(x => x.DrID == ID).FirstOrDefault();
                dram.title = txtDramatitle.Text.Trim();
                dram.DateFrom = Convert.ToDateTime(txtDateFrom.Text.Trim());
                dram.DateTo = Convert.ToDateTime(txtDateto.Text.Trim());
                //dram.Lisence = txtlicense.Text.Trim();
                dram.ScScruName = txtpersonName.Text.Trim();
                dram.ScScruDate = Convert.ToDateTime(txtSurintDate.Text.Trim());
                dram.ScScruFee = Convert.ToInt32(txtFeePaid.Text.Trim());
                dram.branch = BranchID;
                dram.moniName = txtMoniName.Text.Trim();
                dram.moniContact = txtMoniContact.Text.Trim();
                dram.Theaetretile = Convert.ToInt32(ddltheatreName.SelectedValue);
                db.SubmitChanges();
                ucMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
            }
            clearFields();
            BindGrid();
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            ID = 0;
            txtDramatitle.Text = "";
            txtDateFrom.Text = "";
            txtDateto.Text = "";
            //txtlicense.Text = "";
            txtpersonName.Text = "";
            txtSurintDate.Text = "";
            txtFeePaid.Text = "";
            txtMoniName.Text = "";
            txtMoniContact.Text =  "";
            ddltheatreName.SelectedValue = "0";
        }

        protected void grddrama_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdDrama.SelectedValue);
            tblDramaInfo drama = db.tblDramaInfos.Where(x => x.DrID == ID).FirstOrDefault();
            txtDramatitle.Text = drama.title.ToString();
            txtDateFrom.Text = drama.DateFrom.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            txtDateto.Text = drama.DateTo.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            //txtlicense.Text = drama.Lisence.ToString();
            txtpersonName.Text = drama.ScScruName.ToString();
            txtSurintDate.Text = drama.ScScruDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            txtFeePaid.Text = drama.ScScruFee.ToString();
            txtMoniName.Text = drama.moniName.ToString();
            txtMoniContact.Text = drama.moniContact.ToString();
            ddltheatreName.SelectedValue = drama.Theaetretile.ToString();
        }

        protected void grddrama_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDrama.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grddrama_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                e.Row.Cells[5].Text = Convert.ToDateTime(e.Row.Cells[5].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void BindGrid()
        {
            grdDrama.DataSource = from dra in db.tblDramaInfos
                                  join th in db.tblTheaterInfos on dra.Theaetretile equals th.ThID
                                  select new
                                  {
                                      dra.DrID,
                                      dra.title,
                                      dra.DateFrom,
                                      dra.DateTo,
                                      dra.Lisence,
                                      dra.ScScruName,
                                      dra.ScScruDate,
                                      dra.ScScruFee,
                                      thName = th.title
                                  };
            grdDrama.DataBind();
        }

        protected void FillTheatreDropdown()
        {
            ddltheatreName.DataTextField = "title";
            ddltheatreName.DataValueField = "thID";
            ddltheatreName.DataSource = db.tblTheaterInfos.Where(x => x.Branch == BranchID).ToList();
            ddltheatreName.DataBind();
            ddltheatreName.Items.Insert(0, new ListItem("Select Theatre", "0"));
        }

        public void clearFields()
        {
            ID = 0;
            txtDramatitle.Text = "";
            txtDateFrom.Text = "";
            txtDateto.Text = "";
            //txtlicense.Text = "";
            txtpersonName.Text = "";
            txtSurintDate.Text = "";
            txtFeePaid.Text = "";
            txtMoniName.Text = "";
            txtMoniContact.Text = "";
            ddltheatreName.SelectedValue = "0";
        }
    }
}
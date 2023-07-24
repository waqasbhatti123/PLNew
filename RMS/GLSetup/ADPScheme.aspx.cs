using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.GLSetup
{
    public partial class ADPScheme : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        tblADPScheme adp = new tblADPScheme();
        tblADPSchemeDetail sd = new tblADPSchemeDetail();
        voucherDetailBL objVoucher = new voucherDetailBL();
#pragma warning disable CS0114 // 'ADPScheme.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'ADPScheme.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int ProID
        {
            get { return (ViewState["ProID"] == null) ? 0 : Convert.ToInt32(ViewState["ProID"]); }
            set { ViewState["ProID"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "adpscheme").ToString();
                if (Session["DateFormat"] == null)
                {
                    txtApprovalDateCal.Format = Request.Cookies["uzr"]["DateFormat"];

                }
                else
                {
                    txtApprovalDateCal.Format = Session["DateFormat"].ToString();
                }

                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }
                Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                FillDivisionDropdown();
                FillDropDownADPscheme();
                BindGrid();
                BindGridPro();
            }
        }

        protected void Save_click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                var fin = (from y in db.FIN_PERDs
                           where y.Cur_Year.Equals("CUR")
                           select y.Gl_Year).Single();

                adp.fin = Convert.ToInt32(fin);
                if (txtSchemeID.Text == "")
                {
                    adp.SchemeID = null;
                }
                else
                {
                    adp.SchemeID = txtSchemeID.Text.Trim();
                }
                if (txtSchemeTitle.Text == "")
                {
                    adp.title = null;
                }
                else
                {
                    adp.title = txtSchemeTitle.Text.Trim();
                }
                if (txtApprovalDate.Text == "")
                {
                    adp.AppDate = null;
                }
                else
                {
                    adp.AppDate = Convert.ToDateTime(txtApprovalDate.Text.Trim());
                }
                if (ddlDivisional.SelectedValue == "0")
                {
                    adp.Locations = null;
                }
                else
                {
                    adp.Locations = Convert.ToInt32(ddlDivisional.SelectedValue);
                }
                if (txtSchemeCat.Text == "")
                {
                    adp.SchCate = null;
                }
                else
                {
                    adp.SchCate = txtSchemeCat.Text.Trim();
                }
                if (txtEstiCost.Text == "")
                {
                    adp.EstimatedCost = null;
                }
                else
                {
                    adp.EstimatedCost = Convert.ToDecimal(txtEstiCost.Text.Trim());
                }
                if (ddlStatus.SelectedValue == "0")
                {
                    adp.status = null;
                }
                else
                {
                    adp.status = ddlStatus.SelectedValue;
                }
                if (checkIsActive.Checked == true)
                {
                    adp.isActive = true;
                }
                else
                {
                    adp.isActive = false;
                }
                db.tblADPSchemes.InsertOnSubmit(adp);
                db.SubmitChanges();
                ucMessage.ShowMessage("Save successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                tblADPScheme adsc = db.tblADPSchemes.Where(x => x.ADID == ID).FirstOrDefault();
                if (txtSchemeID.Text == "")
                {
                    adsc.SchemeID = null;
                }
                else
                {
                    adsc.SchemeID = txtSchemeID.Text.Trim();
                }
                if (txtSchemeTitle.Text == "")
                {
                    adsc.title = null;
                }
                else
                {
                    adsc.title = txtSchemeTitle.Text.Trim();
                }
                if (txtApprovalDate.Text == "")
                {
                    adsc.AppDate = null;
                }
                else
                {
                    adsc.AppDate = Convert.ToDateTime(txtApprovalDate.Text.Trim());
                }
                if (ddlDivisional.SelectedValue == "0")
                {
                    adsc.Locations = null;
                }
                else
                {
                    adsc.Locations = Convert.ToInt32(ddlDivisional.SelectedValue);
                }
                if (txtSchemeCat.Text == "")
                {
                    adsc.SchCate = null;
                }
                else
                {
                    adsc.SchCate = txtSchemeCat.Text.Trim();
                }
                if (txtEstiCost.Text == "")
                {
                    adsc.EstimatedCost = null;
                }
                else
                {
                    adsc.EstimatedCost = Convert.ToDecimal(txtEstiCost.Text.Trim());
                }
                if (ddlStatus.SelectedValue == "0")
                {
                    adsc.status = null;
                }
                else
                {
                    adsc.status = ddlStatus.SelectedValue;
                }
                if (checkIsActive.Checked == true)
                {
                    adsc.isActive = true;
                }
                else
                {
                    adsc.isActive = false;
                }
                db.SubmitChanges();
                ucMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
            }
            BindGrid();
            ClearFields();
        }
        protected void Clear_Click(object sender, EventArgs e)
        {
            ID = 0;
            txtSchemeID.Text = "";
            txtSchemeTitle.Text = "";
            txtApprovalDate.Text = "";
            txtEstiCost.Text = "";
            txtSchemeCat.Text = "";
            ddlDivisional.SelectedValue = "0";
            ddlStatus.SelectedValue = "0";
        }

        protected void btnProgress_Save(object sender, EventArgs e)
        {
            if (ProID == 0)
            {

                if (ddlSchemeTitle.SelectedValue == "0")
                {
                    sd.titleID = null;
                }
                else
                {
                    sd.titleID = Convert.ToInt32(ddlSchemeTitle.SelectedValue);
                }
                if (SelectedYear.SelectedValue == "0")
                {
                    sd.CurYear = null;
                }
                else
                {
                    sd.CurYear = SelectedYear.SelectedValue;
                }
                if (txtAllocationPrice.Text == "")
                {
                    sd.Allocation = null;
                }
                else
                {
                    sd.Allocation = Convert.ToDecimal(txtAllocationPrice.Text.Trim());
                }
                if (txtrealseRrice.Text == "")
                {
                    sd.Release = null;
                }
                else
                {
                    sd.Release = Convert.ToDecimal(txtrealseRrice.Text.Trim());
                }
                if (txtTotalExp.Text == "")
                {
                    sd.TotalExp = null;
                }
                else
                {
                    sd.TotalExp = Convert.ToDecimal(txtTotalExp.Text.Trim());
                }
                if (txtarearemaks.Text == "")
                {
                    sd.progress = null;
                }
                else
                {
                    sd.progress = txtarearemaks.Text.Trim();
                }
                
                db.tblADPSchemeDetails.InsertOnSubmit(sd);
                db.SubmitChanges();
                ucMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                tblADPSchemeDetail td = db.tblADPSchemeDetails.Where(x => x.ADPID == ProID).FirstOrDefault();
                
                if (ddlSchemeTitle.SelectedValue == "0")
                {
                    td.titleID = null;
                }
                else
                {
                    td.titleID = Convert.ToInt32(ddlSchemeTitle.SelectedValue);
                }
                if (SelectedYear.SelectedValue == "0")
                {
                    td.CurYear = null;
                }
                else
                {
                    td.CurYear = SelectedYear.SelectedValue;
                }
                if (txtAllocationPrice.Text == "")
                {
                    td.Allocation = null;
                }
                else
                {
                    td.Allocation = Convert.ToDecimal(txtAllocationPrice.Text.Trim());
                }
                if (txtrealseRrice.Text == "")
                {
                    td.Release = null;
                }
                else
                {
                    td.Release = Convert.ToDecimal(txtrealseRrice.Text.Trim());
                }
                if (txtTotalExp.Text == "")
                {
                    td.TotalExp = null;
                }
                else
                {
                    td.TotalExp = Convert.ToDecimal(txtTotalExp.Text.Trim());
                }
                if (txtarearemaks.Text == "")
                {
                    td.progress = null;
                }
                else
                {
                    td.progress = txtarearemaks.Text.Trim();
                }
                db.SubmitChanges();
                ucMessage.ShowMessage("Updated Successfully ", BL.Enums.MessageType.Info);
            }
            BindGridPro();
            ClearFields();
        }
        protected void btnProgress_Clear(object sender, EventArgs e)
        {
            txtAllocationPrice.Text = "";
            txtarearemaks.Text = "";
            txtrealseRrice.Text = "";
            ddlSchemeTitle.SelectedValue = "0";
            SelectedYear.SelectedValue = "0";
            txtTotalExp.Text = "";

        }
        protected void grdadp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdAdpScheme.SelectedValue);
            tblADPScheme adpsc = db.tblADPSchemes.Where(x => x.ADID == ID).FirstOrDefault();
            txtSchemeTitle.Text = adpsc.title.ToString();
            txtSchemeID.Text = adpsc.SchemeID.ToString();
            if (adpsc.AppDate == null)
            {
                txtApprovalDate.Text = "";
            }
            else{
                txtApprovalDate.Text = adpsc.AppDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            if (string.IsNullOrEmpty(adpsc.SchCate))
            {
                txtSchemeCat.Text = "";
            }
            else
            {
                txtSchemeCat.Text = adpsc.SchCate.ToString();
            }
            if (adpsc.EstimatedCost == null)
            {
                txtEstiCost.Text = "";
            }
            else
            {
                txtEstiCost.Text = adpsc.EstimatedCost.ToString();
            }
            if (adpsc.Locations == null)
            {
                ddlDivisional.SelectedValue = "0";
            }
            else
            {
                ddlDivisional.SelectedValue = adpsc.Locations.ToString();
            }
            if (adpsc.status == null)
            {
                ddlStatus.SelectedValue = "0";
            }
            else
            {
                ddlStatus.SelectedValue = adpsc.status.ToString();
            }

            if (adpsc.isActive == false)
            {
               checkIsActive.Checked = false;
            }
            else
            {
                checkIsActive.Checked = true;
            }


        }

        protected void grdadp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAdpScheme.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void grdadp_RowDataBound(object sender, GridViewRowEventArgs e)
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
                //e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void grdpro_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProID = Convert.ToInt32(grdPro.SelectedValue);
            tblADPSchemeDetail tb = db.tblADPSchemeDetails.Where(x => x.ADPID == ProID).FirstOrDefault();
            if (tb.titleID == null)
            {
                ddlSchemeTitle.SelectedValue = "0";
            }
            else
            {
                ddlSchemeTitle.SelectedValue = tb.titleID.ToString();
            }
            if (tb.CurYear == null)
            {
                SelectedYear.SelectedValue = "0";
            }
            else
            {
                SelectedYear.SelectedValue = tb.CurYear.ToString();
            }
            if (tb.Allocation == null)
            {
                txtAllocationPrice.Text = "";
            }
            else
            {
                txtAllocationPrice.Text = tb.Allocation.ToString();
            }
            if (tb.Release == null)
            {
                txtrealseRrice.Text = "";
            }
            else
            {
                txtrealseRrice.Text = tb.Release.ToString();
            }
            if (tb.TotalExp == null)
            {
                txtTotalExp.Text = "";
            }
            else
            {
                txtTotalExp.Text = tb.TotalExp.ToString();
            }
            if (txtarearemaks.Text == null)
            {
                txtarearemaks.Text = "";
            }
            else
            {
                txtarearemaks.Text = tb.progress.ToString();
            }
            
        }

        protected void grdpro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void grdpro_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void FillDropDownADPscheme()
        {
            ddlSchemeTitle.DataTextField = "title";
            ddlSchemeTitle.DataValueField = "ADID";
            ddlSchemeTitle.DataSource = db.tblADPSchemes.ToList();
            ddlSchemeTitle.DataBind();
            ddlSchemeTitle.Items.Insert(0, new ListItem("Select Scheme", "0"));
        }

        protected void BindGrid()
        {
            grdAdpScheme.DataSource = from ad in db.tblADPSchemes
                                      join bra in db.Branches on ad.Locations equals bra.br_id
                                      where ad.fin == Financialyear
                                      select new
                                      {
                                          ad.ADID,
                                          ad.title,
                                          ad.SchemeID,
                                          ad.AppDate,
                                          ad.Locations,
                                          ad.status,
                                          ad.EstimatedCost,
                                          bra.br_nme,
                                          ad.isActive,
                                      };
            grdAdpScheme.DataBind();
        }

        protected void BindGridPro()
        {
            grdPro.DataSource = from pro in db.tblADPSchemeDetails
                                join sc in db.tblADPSchemes on pro.titleID equals sc.ADID
                                select new
                                {
                                    pro.ADPID,
                                    sc.title,
                                   year =  pro.CurYear,
                                    pro.Allocation,
                                    pro.Release,
                                    pro.TotalExp
                                };
            grdPro.DataBind();
        }

        public void ClearFields()
        {
            ID = 0;
            txtSchemeID.Text = "";
            txtSchemeTitle.Text = "";
            txtApprovalDate.Text = "";
            txtEstiCost.Text = "";
            txtSchemeCat.Text = "";
            ddlDivisional.SelectedValue = "0";
            ddlStatus.SelectedValue = "0";
            txtAllocationPrice.Text = "";
            txtarearemaks.Text = "";
            txtrealseRrice.Text = "";
            ddlSchemeTitle.SelectedValue = "0";
            SelectedYear.SelectedValue = "0";
            txtTotalExp.Text = "";
        }
    }
}
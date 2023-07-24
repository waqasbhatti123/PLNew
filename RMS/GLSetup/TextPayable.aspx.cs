using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.GLSetup
{
    public partial class TextPayable : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();

#pragma warning disable CS0114 // 'TextPayable.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'TextPayable.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public int TEXTID
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "textPayable").ToString();
               
                

                if (Session["DateFullYearFormat"] == null)
                {
                    ChqDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    ChqDateCal.Format = Session["DateFullYearFormat"].ToString();
                }

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

                FillSearchBranchDropDown();
                BindGrid(BranchID);
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
               
            }
        }

        protected void Onclick_Search(object sender, EventArgs e)
        {
            string acc = txtaccount.Text.Trim();
            BindGridSearch(acc);
        }
        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                   
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    FillSearchBranchDropDown();
                    BindGrid(BranchID);
                    ClearFields();
                }

            }
            catch
            { }
        }
        protected void BindGridSearch(string account)
        {
            this.grdTextPayable.DataSource = (from a in db.Glmf_Datas
                                              join type in db.Vr_Types on a.vt_cd equals type.vt_cd
                                              join chq in db.Glmf_Data_chqs on a.vrid equals chq.vrid
                                              join text in db.tblTextPayables on a.vrid equals text.VrID
                                              into textpay
                                              from textpayable in textpay.DefaultIfEmpty()
                                              where a.vt_cd == 64 && a.vr_apr == "A"
                                              && chq.vr_chq == account && a.br_id == BranchID
                                              orderby a.vr_dt ascending, a.vr_no descending
                                              select new
                                              {
                                                  textpayable.ITAmount,
                                                  textpayable.GSTAmount,
                                                  textpayable.PRA,
                                                  a.vrid,
                                                  vr_nrtn = a.vr_nrtn,
                                                  ref_no = a.Ref_no,
                                                  type = type.vt_use + (a.source != null && type.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
                                                  cheNo = chq.vr_chq,
                                                  cheDat = chq.vr_chq_dt,
                                                  a.source,

                                                  headsInvolved = GetGLMFCode(a.vrid, db),
                                              }).ToList();
            this.grdTextPayable.DataBind();
        }
        protected void BindGrid(int br)
        {
            this.grdTextPayable.DataSource = (from a in db.Glmf_Datas
                                              join type in db.Vr_Types on a.vt_cd equals type.vt_cd
                                              join chq in db.Glmf_Data_chqs on a.vrid equals chq.vrid
                                              join text in db.tblTextPayables on a.vrid equals text.VrID
                                              into textpay from textpayable in textpay.DefaultIfEmpty()
                                              where a.vt_cd == 64 && a.vr_apr == "A" && a.br_id == br
                                              orderby a.vr_dt ascending, a.vr_no descending
                                              select new
                                              {
                                                  textpayable.ITAmount,
                                                  textpayable.GSTAmount,
                                                  textpayable.PRA,
                                                  a.vrid,
                                                  vr_nrtn = a.vr_nrtn,
                                                  ref_no = a.Ref_no,
                                                  type = type.vt_use + (a.source != null && type.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
                                                  cheNo = chq.vr_chq,
                                                  cheDat = chq.vr_chq_dt,
                                                  a.source,

                                                  headsInvolved = GetGLMFCode(a.vrid,db),
                                              }).ToList();
            this.grdTextPayable.DataBind();
        }

        public string GetGLMFCode(int vrId, RMSDataContext Data)
        {
            try
            {
                var codeDesc =
                               from c in Data.Glmf_Data_Dets
                               join d in Data.Glmf_Codes on c.gl_cd equals d.gl_cd
                               where c.vrid == vrId
                               select d.gl_dsc;

                string code = "";
                foreach (var item in codeDesc)
                {
                    code += item + "\r\n";
                }
                return code;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }
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

        protected void grdText_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdTextPayable.SelectedValue);
            var vr = (from a in db.Glmf_Datas
                      join type in db.Vr_Types on a.vt_cd equals type.vt_cd
                      join chq in db.Glmf_Data_chqs on a.vrid equals chq.vrid
                      join text in db.tblTextPayables on a.vrid equals text.VrID
                      into textpay from textpayable in textpay.DefaultIfEmpty()
                      where a.vrid == ID && a.br_id == BranchID
                      select new
                      {
                          ID,
                          vr_nrtn = a.vr_nrtn,
                          Ref_no = a.Ref_no,
                          chqNo = chq.vr_chq,
                          chqDat = chq.vr_chq_dt,
                          IT = textpayable.ITAmount,
                          GST = textpayable.GSTAmount,
                          PRA = textpayable.PRA
                      }).FirstOrDefault();
            vrid.Text = ID.ToString();
            txtnarration.Text = vr.vr_nrtn;
            VoucherNO.Text = vr.Ref_no.ToString();
            ChqNo.Text = vr.chqNo.ToString();
            //ChqDate.Text = vr.chqDat.ToString();
            txtIncome.Text = vr.IT.ToString();
            txtGST.Text = vr.GST.ToString();
            txtPRA.Text = vr.PRA.ToString();
            if (Session["DateFullYearFormat"] == null)
            {
                this.ChqDate.Text = vr.chqDat.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
            }
            else
            {
                this.ChqDate.Text = vr.chqDat.ToString(Session["DateFullYearFormat"].ToString());
            }
        }

        protected void grdText_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTextPayable.PageIndex = e.NewPageIndex;
            BindGrid(BranchID);
        }

        protected void grdText_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow )
            {
                e.Row.Cells[5].Text = Convert.ToDateTime(e.Row.Cells[5].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
            }
        }

        protected void Onclick_Save(object sender, EventArgs e)
        {
            if (ID > 0)
            {
                var text = db.tblTextPayables.Where(x => x.VrID == ID).FirstOrDefault();
                if (text == null)
                {
                    tblTextPayable txt = new tblTextPayable();
                    txt.VrID = ID;
                    txt.ITAmount = Convert.ToInt32(txtIncome.Text);
                    txt.GSTAmount = Convert.ToInt32(txtGST.Text);
                    txt.PRA = Convert.ToInt32(txtPRA.Text);
                    db.tblTextPayables.InsertOnSubmit(txt);
                    db.SubmitChanges();
                }
                else
                {
                    var tp = db.tblTextPayables.Where(x => x.VrID == ID).FirstOrDefault();
                    tp.ITAmount = Convert.ToInt32(txtIncome.Text);
                    tp.GSTAmount = Convert.ToInt32(txtGST.Text);
                    tp.PRA = Convert.ToInt32(txtPRA.Text);
                    db.SubmitChanges();
                }
            }
            ClearFields();
            BindGrid(BranchID);
        }
        protected void Onclick_Clear(object sender, EventArgs e)
        {
            txtnarration.Text = "";
            VoucherNO.Text = "";
            ChqNo.Text = "";
            ChqDate.Text = "";
            txtIncome.Text = "";
            txtGST.Text = "";
            txtPRA.Text = "";
        }

        public void ClearFields()
        {
            txtnarration.Text = "";
            VoucherNO.Text = "";
            ChqNo.Text = "";
            ChqDate.Text = "";
            txtIncome.Text = "";
            txtGST.Text = "";
            txtPRA.Text = "";
            txtaccount.Text = "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using RMS.BL;

namespace RMS.Inv
{
    public partial class PostConsumption : BasePage
    {

        #region DataMembers
        PostConsumptionBL post = new PostConsumptionBL();

        List<List<string>> upperList;
        static List<int> vridList = new List<int>();
        List<int> vridTempList = new List<int>();

        string msg = "", cc;
        #endregion

        #region Properties

        public int brId
        {
            set { ViewState["brId"] = value; }
            get { return Convert.ToInt32(ViewState["brId"] ?? 0); }
        }
        public DateTime TillDate
        {
            set { ViewState["TillDate"] = value; }
            get { return Convert.ToDateTime(ViewState["TillDate"]); }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "PC").ToString();

                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        brId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    brId = Convert.ToInt32(Session["BranchID"].ToString());
                }
                GetMonth();
                this.BindGridParent();
            }
        }
        protected void gvConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                
                string cc_cd = gvConsumption.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string prnt_cd = gvConsumption.DataKeys[e.Row.RowIndex].Values[1].ToString();
                GridView gvConsumptnDet = e.Row.FindControl("gvConsumptnDet") as GridView;

                gvConsumptnDet.DataSource = post.GetCosumptionGrdDataDetail(brId, cc_cd,prnt_cd, TillDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                gvConsumptnDet.DataBind();
                
            }
        }
        protected void gvConsumptnDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text).ToString(Session["DateFormat"].ToString());
                }
            }

            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
        }
        protected void Post_Click(object sender, EventArgs e)
        {
            //string month = post.GetConsumptionMonth((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            string month = TillDate.Month.ToString();
            string currmonth = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month.ToString();
            if (month == currmonth)
            {
                ucMessage.ShowMessage("Consumption can't be posted twice in the same month", BL.Enums.MessageType.Error);
                return;
            }
            if (vridList.Count == 0)
            {
                ucMessage.ShowMessage("Please select atleast one consumption to continue", BL.Enums.MessageType.Error);
                return;
            }
            bool valid = this.ValidateData();
            if (valid)
            {
                this.CreateIPV();
            }
        }
        protected void chkSelect_Change(object sender, EventArgs e)
        {
            try
            {
                int vrid = 0;
                bool assignval = false;
                vridList.Clear();
                GridViewRow clickedRow = ((CheckBox)sender).NamingContainer as GridViewRow;
                GridView gvdet = (GridView)clickedRow.FindControl("gvConsumptnDet");
                assignval = ((CheckBox)clickedRow.FindControl("chkSelect")).Checked;
                
                //getting clicked row vrids
                foreach (GridViewRow gvdetTempRow in gvdet.Rows)
                {
                    vrid = Convert.ToInt32(gvdet.DataKeys[gvdetTempRow.RowIndex].Value);
                    if (!vridTempList.Exists(x => x == vrid))
                    {
                        vridTempList.Add(vrid);
                    }   
                }
                //changing checkboxes of other rows according to clicked row
                GridView innerGrid;
                foreach (GridViewRow gvRow in gvConsumption.Rows)
                {
                    innerGrid = ((GridView)gvConsumption.Rows[gvRow.RowIndex].FindControl("gvConsumptnDet"));
                    foreach (GridViewRow gvdetRow in innerGrid.Rows)
                    {
                        if (clickedRow.RowIndex != gvdetRow.RowIndex)
                        {
                            if (vridTempList.Exists(y => y == Convert.ToInt32(innerGrid.DataKeys[gvdetRow.RowIndex].Value)))
                            {
                                ((CheckBox)gvRow.FindControl("chkSelect")).Checked = assignval;
                                break;
                            }
                        }
                    }
                }
                //filling vrids to change post2gl column
                innerGrid = null;
                vrid = 0;
                foreach (GridViewRow row in gvConsumption.Rows)
                {
                    if (((CheckBox)row.FindControl("chkSelect")).Checked)
                    {
                        innerGrid = ((GridView)gvConsumption.Rows[row.RowIndex].FindControl("gvConsumptnDet"));
                        foreach (GridViewRow detrow in innerGrid.Rows)
                        {
                            vrid = Convert.ToInt32(innerGrid.DataKeys[detrow.RowIndex].Value);
                            if (!vridList.Exists(x => x == vrid))
                            {
                                vridList.Add(vrid);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, BL.Enums.MessageType.Error);
            }
        }
        protected void chkSelectAll_Change(object sender, EventArgs e)
        {
            try
            {
                int vrid = 0;
                bool assignval = false;
                vridList.Clear();
                GridViewRow clickedRow = ((CheckBox)sender).NamingContainer as GridViewRow;
                GridView gvdet = (GridView)clickedRow.FindControl("gvConsumptnDet");
                assignval = ((CheckBox)clickedRow.FindControl("chkselectAll")).Checked;

                //filling vrids to change post2gl column
                GridView innerGrid = null;
                vrid = 0;
                foreach (GridViewRow row in gvConsumption.Rows)
                {
                    ((CheckBox)(row.FindControl("chkSelect"))).Checked = assignval;
                    if (assignval)
                    {
                        innerGrid = ((GridView)gvConsumption.Rows[row.RowIndex].FindControl("gvConsumptnDet"));
                        foreach (GridViewRow detrow in innerGrid.Rows)
                        {
                            vrid = Convert.ToInt32(innerGrid.DataKeys[detrow.RowIndex].Value);
                            if (!vridList.Exists(x => x == vrid))
                            {
                                vridList.Add(vrid);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, BL.Enums.MessageType.Error);
            }
        }

        #endregion

        #region Methods

        public void GetMonth()
        {
            //int month = Convert.ToInt32(post.GetConsumptionMonth((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])) + 2;
            //TillDate = Convert.ToDateTime(
            //    month.ToString()
            //    + "-01-" +
            //    RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString()
            //    ).AddDays(-1);
            DateTime date = post.GetConsumptionDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (date.Month <= 10)
            {
                TillDate = Convert.ToDateTime((date.Month + 1).ToString()
                    + "-01-" + date.Year.ToString()
                    ).AddDays(-1);

            }
            else if (date.Month == 10)
            {
                TillDate = Convert.ToDateTime("11-30-" + date.Year.ToString());

            }
            else if (date.Month == 11)
            {
                TillDate = Convert.ToDateTime("12-31-" + date.Year.ToString());
            }
            else if (date.Month == 12)
            {
                TillDate = Convert.ToDateTime("01-31-" + (date.Year + 1).ToString());
            }

            lblTitle.Text = "Post consumption till " + TillDate.ToString("dd-MMM-yyyy");
        }
        public void BindGridParent()
        {
            gvConsumption.DataSource = post.GetCosumptionGrdData(brId, TillDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            gvConsumption.DataBind();
        }
        private string GetNarration()
        {
            string vrNarr = "";
            StringBuilder builder = new StringBuilder();

            builder.Append("CONSUMPTION").AppendLine();
            builder.Append("Till Date: " + TillDate.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]));


            vrNarr = builder.ToString();
            if (vrNarr.Length > 500)
            {
                vrNarr = vrNarr.Substring(0, 500);
            }
            return vrNarr;
        }
        public void CreateIPV()
        {
            EntitySet<Glmf_Data_Det> enttyGlDet = new EntitySet<Glmf_Data_Det>();
            string username = "", vrNarr = GetNarration();
            if (Session["LoginID"] == null)
            {
                username = Request.Cookies["uzr"]["LoginID"];
            }
            else
            {
                username = Session["LoginID"].ToString();
            }

            if (username.Length > 15)
            {
                username = username.Substring(0, 14);
            }
            
            
            
            int voucherTypeId = 6;
            //decimal Financialyear = new voucherDetailBL().GetFinancialYearByDate(RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            decimal Financialyear = new voucherDetailBL().GetFinancialYearByDate(TillDate.Date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            string source = "INV";
            int voucherno = new voucherDetailBL().GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], brId, voucherTypeId, Financialyear, source);
            Preference pref = new PreferenceBL().GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            /***************************************************************************/
            /*  ONE */
            /***************************************************************************/
            Glmf_Data glmfdata = new Glmf_Data();//MASTER ROW
            glmfdata.br_id = brId;
            glmfdata.Gl_Year = Financialyear;
            glmfdata.vt_cd = Convert.ToInt16(voucherTypeId);
            glmfdata.vr_no = voucherno;
            glmfdata.vr_dt = TillDate;//RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
            glmfdata.vr_nrtn = vrNarr;
            glmfdata.vr_apr = "P";
            glmfdata.updateby = username;
            glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            glmfdata.approvedby = username;
            glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            glmfdata.source = source;

            /***************************************************************************/
            /*  Two */
            /***************************************************************************/

            this.FillIPVList();
            if (msg == "")
            {
                int Seq = 0;
                foreach (List<string> lstDet in upperList)
                {

                    Glmf_Data_Det glDet1 = new Glmf_Data_Det();
                    Seq = Seq + 1;
                    glDet1.vr_seq = Seq;
                    glDet1.gl_cd = Convert.ToDecimal(lstDet[5]) > 0 ? lstDet[2] : lstDet[3];
                    glDet1.vrd_debit = Convert.ToDecimal(lstDet[4]);
                    glDet1.vrd_credit = Convert.ToDecimal(lstDet[5]);
                    glDet1.vrd_nrtn = "";
                    glDet1.cc_cd = Convert.ToDecimal(lstDet[5]) > 0 ? null : lstDet[0];
                    enttyGlDet.Add(glDet1);
                }

                glmfdata.Glmf_Data_Dets = enttyGlDet;
                string pstConsmptn = post.PostConsumption(glmfdata, vridList, TillDate, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (pstConsmptn == "ok")
                {
                    GetMonth();
                    this.BindGridParent();
                    ucMessage.ShowMessage("Consumption Posted SuccessFully", BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage(pstConsmptn, BL.Enums.MessageType.Error);
                }

            }
            else
            {
                ucMessage.ShowMessage(msg, BL.Enums.MessageType.Error);
            }
        }
        public void FillIPVList()
        {
            upperList = new List<List<string>>();
            List<string> innerList;
            int  grpCodeLen = 0;
            grpCodeLen = 5;//new InvCode().GetGroupCodeLength((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            string itmcd = "", purchaseCd = "", issueCd="";
            bool allClear;
            for (int i = 0; i < gvConsumption.Rows.Count; i++)   // ****** DEBIT AMOUNTS ******* 
            {

                GridView gv = (GridView)gvConsumption.Rows[i].FindControl("gvConsumptnDet");
             
                CheckBox chk = (CheckBox)gvConsumption.Rows[i].FindControl("chkSelect");
                //CheckBox innerChk;
                if (chk.Checked)
                {
                    
                    cc = gvConsumption.DataKeys[i].Value.ToString();
                    for (int j = 0; j < gv.Rows.Count; j++)
                    {
                        //innerChk = (CheckBox)gv.Rows[j].FindControl("chkChildSelect");
                        //if (innerChk.Checked)
                        //{
                            allClear = true;
                            itmcd = gv.Rows[j].Cells[4].Text.Trim();
                            purchaseCd = new InvCode().GetGroupPurchaseAccount(brId, itmcd.Substring(0, grpCodeLen), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            issueCd = new InvCode().GetGroupIssueAccount(brId, itmcd.Substring(0, grpCodeLen), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (string.IsNullOrEmpty(purchaseCd))
                            {
                                allClear = false;
                                msg = msg + "Ctrl purchase a/c is missing, Plz update item code master" + "</br>";
                            }
                            if (string.IsNullOrEmpty(issueCd))
                            {
                                allClear = false;
                                msg = msg + "Ctrl issue a/c is missing, Plz update item code master" + "</br>";
                            }

                            foreach (List<string> lst in upperList)
                            {
                                if (lst[0] == cc && lst[2] == purchaseCd && lst[3] == issueCd)
                                {
                                    lst[4] = (Convert.ToDecimal(lst[4]) + Convert.ToDecimal(gv.Rows[j].Cells[8].Text)).ToString();
                                    allClear = false;
                                    break;

                                }
                            }
                            if (allClear)
                            {
                                innerList = new List<string>();
                                innerList.Add(cc);
                                innerList.Add(itmcd);
                                innerList.Add(purchaseCd);
                                innerList.Add(issueCd);
                                innerList.Add(gv.Rows[j].Cells[8].Text);
                                innerList.Add("0");
                                upperList.Add(innerList);
                            }
                        //}
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Check Altleast One Row To Continue", BL.Enums.MessageType.Info);
                }
            }

            // ****** CREDIT AMOUNTS *******            
            bool clear;
            List<List<string>> tempList = new List<List<string>>();
            foreach (List<string> uplst in upperList)
            {
                clear = true;
                foreach (List<string> inlst in tempList)
                {
                    if (uplst[2] == inlst[2])
                    {
                        clear = false;
                        inlst[5] = (Convert.ToDecimal(inlst[5]) + Convert.ToDecimal(uplst[4])).ToString();
                    }
                }
                if (clear)
                {
                    innerList = new List<string>();
                    innerList.Add(uplst[0]);
                    innerList.Add(null);
                    innerList.Add(uplst[2]);
                    innerList.Add(uplst[3]);
                    innerList.Add("0");
                    innerList.Add(uplst[4]);
                    tempList.Add(innerList);
                }
            }

            foreach (List<string> lststr in tempList)
            {
                upperList.Add(lststr);
            }
        }
        public bool ValidateData()
        {
            bool inChk = false;
            //CheckBox innerChk;

            for (int i = 0; i < gvConsumption.Rows.Count; i++)
            {
                GridView gv = (GridView)gvConsumption.Rows[i].FindControl("gvConsumptnDet");
                for (int j = 0; j < gv.Rows.Count; j++)
                {
                    //innerChk = (CheckBox)gv.Rows[j].FindControl("chkChildSelect");
                    //if (innerChk.Checked)
                    //{
                        inChk = true;
                    //}
                }
            }
            return inChk;
        }

        #endregion
    }
}

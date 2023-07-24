using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.GLSetup
{
    public partial class frmChqClearance : BasePage
    {


        #region DataMembers

        ChqBL chqBL = new ChqBL();

        #endregion

        #region Properties

#pragma warning disable CS0114 // 'frmChqClearance.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'frmChqClearance.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public string UserName
        {
            get { return Convert.ToString(ViewState["ID"]); }
            set { ViewState["UserName"] = value; }
        }
        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        public bool IsSearch
        {
            get { return (ViewState["IsSearch"] == null) ? false : Convert.ToBoolean(ViewState["IsSearch"]); }
            set { ViewState["IsSearch"] = value; }
        }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserName"] == null)
                UserName = Request.Cookies["uzr"]["UserName"].ToString();
            else
                UserName = Session["UserName"].ToString();


            //if (Session["UserName"] == null)
            //{
            //    if (Request.Cookies["uzr"] == null)
            //        Response.Redirect("~/login.aspx");
            //    UserName = Request.Cookies["uzr"]["UserName"];
            //}
            //else
            //{
            //    UserName = Session["UserName"].ToString();
            //}

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ChqClearance").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

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
                    calClrDate.Format = Request.Cookies["uzr"]["DateFormat"];
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"];
                    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"];
                }
                else
                {
                    calClrDate.Format = Session["DateFormat"].ToString();
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                    CalendarExtender2.Format = Session["DateFormat"].ToString();
                }
                txtFltFromDt.Text = Convert.ToDateTime(RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month +"-01-" 
                                    + RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year).
                                    ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString());
                txtFltToDt.Text = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).
                                    ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString());

                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BindGrid();
                txtClrDate.Focus();
            }
        }



        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            this.searchBranchDropDown.DataTextField = "br_nme";
            searchBranchDropDown.DataValueField = "br_id";
            if (BranchID == 1)
            {
                searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else if (BranchID > 1)
            {
                List<Branch> BranchList = new List<Branch>();
                Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();
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



        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                    IsSearch = true;
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    this.BindGrid();
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {

            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
            }
            else if (e.CommandName == "Save")
            {
                string txt = "ClearanceDate";
                try
                {
                    Convert.ToDateTime(txtClrDate.Text);
                }
                catch
                {
                    if (txt.Equals("ClearanceDate"))
                    {
                        ucMessage.ShowMessage("Invalid from date", RMS.BL.Enums.MessageType.Error);
                        txtFltFromDt.Focus();
                    }
                    return;
                }

                if (ID == 0)
                {
                    this.Insert();
                    BindGrid();
                    ClearFields();
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                }

                else
                {
                    this.Update();

                }
            }

            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
            }

            else if (e.CommandName == "Cancel")
            {
                //pnlMain.Enabled = false;
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                ClearFields();

            }
        }
        protected void grdChq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    if (Session["DateFormat"] == null)
                    {
                        e.Row.Cells[5].Text = DateTime.Parse(e.Row.Cells[5].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
                    }
                    else
                    {
                        e.Row.Cells[5].Text = DateTime.Parse(e.Row.Cells[5].Text.ToString()).ToString(Session["DateFormat"].ToString());
                    }
                    if (Session["DateFormat"] == null)
                    {
                        e.Row.Cells[7].Text = DateTime.Parse(e.Row.Cells[7].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
                    }
                    else
                    {
                        e.Row.Cells[7].Text = DateTime.Parse(e.Row.Cells[7].Text.ToString()).ToString(Session["DateFormat"].ToString());
                    }
                }
                catch { }
            }
        }
        protected void grdChq_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdChq.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void grdChq_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdChq.SelectedDataKey.Value);
            GetByID();

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

        #region Helping Method

        protected void BindGrid()
        {
            string txt = "FromDate";
            try
            {
                Convert.ToDateTime(txtFltFromDt.Text);
                txt = "ToDate";
                Convert.ToDateTime(txtFltToDt.Text);
            }
            catch
            {
                if (txt.Equals("FromDate"))
                {
                    ucMessage.ShowMessage("Invalid from date", RMS.BL.Enums.MessageType.Error);
                    txtFltFromDt.Focus();
                }
                else
                {
                    ucMessage.ShowMessage("Invalid to date", RMS.BL.Enums.MessageType.Error);
                    txtFltToDt.Focus();
                }
                return;
            }

            this.grdChq.DataSource = chqBL.GetChqDetails(Convert.ToInt32(ddlFltType.SelectedValue),
                                                    txtFltBank.Text, txtFltVch.Text, txtFltChq.Text, 
                                                    Convert.ToDateTime(txtFltFromDt.Text),
                                                    Convert.ToDateTime(txtFltToDt.Text), BranchID,
                                                    (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdChq.DataBind();
        }
        protected void GetByID()
        {
            spCheqDetailsResult chq = chqBL.GetChqDetailById(ID, BranchID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (chq.Chq_clr_dt != null)
            {
                if (Session["DateFormat"] == null)
                {
                    this.txtClrDate.Text = chq.Chq_clr_dt.Value.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {

                    this.txtClrDate.Text = chq.Chq_clr_dt.Value.ToString(Session["DateFormat"].ToString());
                }

                pnlMain.Enabled = false;
            }
            else
            {
                //txtClrDate.Text = "";
                pnlMain.Enabled = true;
            }
            chkClear.Checked = true;
            pnlInfo.Visible = true;
            lblBank.Text = chq.bnkcd + "  -  " + chq.bnk;
            lblAC.Text = chq.Ref_no.ToString();
            lblChq.Text = chq.chq;
            lblAmount.Text = chq.amount.Value.ToString();
            if (Session["DateFormat"] == null)
            {
                this.lblChqDt.Text = chq.chqdt.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
            }
            else
            {

                this.lblChqDt.Text = chq.chqdt.ToString(Session["DateFormat"].ToString());
            }


            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }
        protected void Insert()
        {
            ucMessage.ShowMessage("Please select a cheque from below", RMS.BL.Enums.MessageType.Error);
        }
        protected void Update()
        {
            if (chkClear.Checked)
            {
                Glmf_Data_chq chq = chqBL.GetChkDataById(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                if (chq.Chq_clr_dt != null)
                {
                    return;
                }

                if (Convert.ToDateTime(txtClrDate.Text) < chq.vr_chq_dt)
                {
                    ucMessage.ShowMessage("Clearance date cannot be less than cheque date", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                //try
                //{
                //    UserName = chqBL.GetUserByLoginId(UserName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).UserName;
                //}
                //catch
                //{
                //    ucMessage.ShowMessage("Invalid user", RMS.BL.Enums.MessageType.Error);
                //    return;
                //}

                chq.Chq_clr_dt = Convert.ToDateTime(txtClrDate.Text);
                chq.Clr_entryBy = UserName.Length > 20 ? UserName.Substring(0, 20) : UserName;
                chq.Clr_entryOn = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                string msg = chqBL.Update(chq, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg == "ok")
                {
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    BindGrid();
                    ClearFields();
                    ucMessage.ShowMessage("Updated successfully", RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage("Exception: " + msg, RMS.BL.Enums.MessageType.Info);
                }
            }
            else
            {
                ucMessage.ShowMessage("Please check the checkbox to clear the cheque", RMS.BL.Enums.MessageType.Error);
                return;
            }

        }
        private void ClearFields()
        {
            ID = 0;
            //txtClrDate.Text = "";
            txtClrDate.Focus();

            pnlInfo.Visible = false;
            lblBank.Text = "";
            lblAC.Text = "";
            lblChq.Text = "";
            lblAmount.Text = "";
            lblChqDt.Text = "";
        }
        
        #endregion
        
        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class EmpMgtResignation : BasePage
    {


        #region DataMembers
        GroupBL groupManager = new GroupBL();
        PlResionationBL mgtRes = new PlResionationBL();

        EmpBL empBL = new EmpBL();
        GroupBL grpBl = new GroupBL();
        CompanyBL compBl = new CompanyBL();
        ListItem selList = new ListItem();
        ListItem selListSub = new ListItem();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'EmpMgtResignation.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpMgtResignation.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        public int GroupID
        {
            get { return (ViewState["GroupID"] == null) ? 0 : Convert.ToInt32(ViewState["GroupID"]); }
            set { ViewState["GroupID"] = value; }
        }
        public int PID
        {
            get { return (ViewState["PID"] == null) ? 0 : Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
        public bool CanApprove
        {
            get { return Convert.ToBoolean(ViewState["CanApprove"]); }
            set { ViewState["CanApprove"] = value; }

        }
        public bool CanEnter
        {
            get { return Convert.ToBoolean(ViewState["CanEnter"]); }
            set { ViewState["CanEnter"] = value; }

        }
        public bool AppCycle
        {
            get { return Convert.ToBoolean(ViewState["AppCycle"]); }
            set { ViewState["AppCycle"] = value; }

        }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpResigSetup").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                EmpSrchUC.ResgId = 1;
                if (Session["DateFormat"] == null)
                {
                    txtLeavingdatecal.Format = Request.Cookies["uzr"]["DateFormat"];
                }
                else
                {
                    txtLeavingdatecal.Format = Session["DateFormat"].ToString();
                }
                 if (Session["CompID"] == null)
                {
                    CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                }
                else
                {
                    CompID = Convert.ToByte(Session["CompID"].ToString());
                }

                try
                {
                    AppCycle = Convert.ToBoolean(compBl.GetByID(CompID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).AppCycle);
                }
                catch 
                {
                    AppCycle = false;
                }

                if (AppCycle)
                {
                    //Maintaning Privilage Status==========================
                    if (Session["GroupID"] == null)
                    {
                        GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                    }
                    else
                    {
                        GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                    }

                    PID = Convert.ToInt32(Request.QueryString["PID"]);

                    tblAppPrivilage appPrivilage = grpBl.GetPrivilageStatus(GroupID, PID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    if (appPrivilage != null)
                    {
                        if (appPrivilage.CanEdit.Equals(true))//approval status given
                        {
                            CanApprove = true;
                        }
                        else
                        {
                            CanApprove = false ;
                        }
                        if (appPrivilage.CanAdd.Equals(true))
                        {
                            CanEnter = true;
                        }
                        else
                        {
                            CanEnter = false;
                        }

                        if (CanApprove && !CanEnter)
                        {
                            //ListItem itemPending = ddlStatus.Items.FindByText("Pending");
                            //ddlStatus.Items.Remove(itemPending);
                        }

                        if (CanApprove && CanEnter)
                        {
                            //ListItem itemApproved = ddlStatus.Items.FindByText("Approved");
                            //ddlStatus.Items.Remove(itemApproved);
                        }

                    }
                    else
                    {
                        ucMessage.ShowMessage("Enter privilages for this page for the logged in group", RMS.BL.Enums.MessageType.Error);
                    }
                    //====================================================================
                }
                else
                {
                    grdResign.Columns[6].Visible = false;
                    //lblStatus.Visible = false;
                    //ddlStatus.Visible = false;
                    //reqVal_ddlStatus.Enabled = false;
                }

                //BindGrid("", "");
                FillDropDownReason();
                ucButtons.ValidationGroupName = "main";
                EmpSrchUC.Focus();
            }
            BindGrid("", "");
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
                if (EmpSrchUC.EmpIDUC == 0)
                {
                    ucMessage.ShowMessage("Please select Employee", RMS.BL.Enums.MessageType.Error);
                    return;
                }

                try
                {
                    Convert.ToDateTime(txtLeavingdate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage("Invalid leaving date", RMS.BL.Enums.MessageType.Error);
                    return;
                }

                if (ID == 0)
                {
                    this.Insert();
                    BindGrid("", "");
                    ClearFields();
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                }

                else
                {
                    this.Update();
                    //pnlMain.Enabled = false;
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    BindGrid("", "");
                    ClearFields();
                    //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
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

        protected void grdEmps_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"]).ToString();
                }
                else
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text.ToString()).ToString(Session["DateFormat"].ToString());
                }

                if (AppCycle)
                {
                    if (e.Row.Cells[6].Text.Equals("True"))
                    {
                        e.Row.Cells[6].Text = "Yes";
                    }
                    else
                    {
                        e.Row.Cells[6].Text = "No";
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(txtFltEmp.Text.Trim(), txtFltEmpCode.Text.Trim());
        }

        #endregion

        #region Helping Method

        protected void BindGrid(string empname, string empcode)
        {
            this.grdResign.DataSource = mgtRes.GetAll(empname, empcode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdResign.DataBind();
        }

        private void FillDropDownReason()
        {
            ddlLeaveReason.DataTextField = "Rsg_Desc";
            ddlLeaveReason.DataValueField = "Rsg_Code";
            ddlLeaveReason.DataSource = mgtRes.GetAllReasonCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLeaveReason.DataBind();
        }

        protected void Insert()
        {
            RMSDataContext db = new RMSDataContext();
            BL.EmpBL empBL = new EmpBL();
            UserBL user = new UserBL();
            ID = EmpSrchUC.EmpIDUC;
            tblPlEmpData emp = empBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            // tblAppUser appuser = user.GetByIDUser(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            tblAppUser appuser = db.tblAppUsers.Where(x => x.EmpID == ID).FirstOrDefault();
            DateTime effective = Convert.ToDateTime(txtLeavingdate.Text.Trim());
            tblPlRsgTran Gettrans = db.tblPlRsgTrans.Where(x => x.EmpId == ID && x.TransDate == effective).FirstOrDefault();
            tblPlAlow alemp = db.tblPlAlows.Where(x => x.EmpID == ID && x.IsActive == true).FirstOrDefault();
            List<tblPlSalaryDetail> saldet = db.tblPlSalaryDetails.Where(x => x.EmpID == ID).ToList();

            if (Gettrans != null)
            {
                emp.DOL = Convert.ToDateTime(txtLeavingdate.Text.Trim());
                emp.LvgReason = txtReason.Text.Trim();
                //tblPlRsgTran trans = new tblPlRsgTran();
                Gettrans.EmpId = ID;
                Gettrans.Rsg_Code = int.Parse(ddlLeaveReason.SelectedValue);
                Gettrans.TransDate = DateTime.Parse(txtLeavingdate.Text.Trim());
                //trans.IsResigned = Convert.ToBoolean(CheckIsRes.Checked);
                if (CheckIsRes.Checked == true)
                {
                    Gettrans.IsResigned = true;
                    emp.EmpStatus = 0;
                    if (appuser != null)
                    {
                        appuser.Enabled = false;
                    }
                    if (alemp != null)
                    {
                        alemp.IsActive = false;
                    }

                    foreach (var item in saldet)
                    {
                        item.IsActive = false;
                    }
                }
                else
                {
                    Gettrans.IsResigned = false;
                    emp.EmpStatus = 1;
                    if (appuser != null)
                    {
                        appuser.Enabled = true;
                    }
                    if (alemp != null)
                    {
                        alemp.IsActive = true;
                    }
                    foreach (var item in saldet)
                    {
                        item.IsActive = true;
                    }
                }
                //trans.CreatedBy = "Admin";
                if (Session["UserID"] == null)
                    Gettrans.CreatedBy = Request.Cookies["uzr"]["UserID"].ToString();
                else
                    Gettrans.CreatedBy = Session["UserID"].ToString();
                Gettrans.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                Gettrans.Reason = txtReason.Text.Trim();


                if (AppCycle)
                {
                    if (CanApprove && CanEnter)
                    {
                        if (Session["UserID"] == null)
                            Gettrans.ReqBy = Request.Cookies["uzr"]["UserID"].ToString();
                        else
                            Gettrans.ReqBy = Session["UserID"].ToString();
                        Gettrans.ReqDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                    //if (ddlStatus.SelectedValue.Equals("A"))
                    //{
                    //    if (CanApprove && !CanEnter)
                    //    {
                    //        if (Session["UserID"] == null)
                    //            trans.AppBy = Request.Cookies["uzr"]["UserID"].ToString();
                    //        else
                    //            trans.AppBy = Session["UserID"].ToString();
                    //        trans.AppDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //    }
                    //}
                    //trans.Status = Convert.ToString(ddlStatus.SelectedValue);
                }


                // mgtRes.InsertUpdate(trans, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                db.SubmitChanges();
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
            }
            else
            {
                emp.DOL = Convert.ToDateTime(txtLeavingdate.Text.Trim());
                emp.LvgReason = txtReason.Text.Trim();
                tblPlRsgTran trans = new tblPlRsgTran();
                trans.EmpId = ID;
                trans.Rsg_Code = int.Parse(ddlLeaveReason.SelectedValue);
                trans.TransDate = DateTime.Parse(txtLeavingdate.Text.Trim());
               // trans.IsResigned = Convert.ToBoolean(CheckIsRes.Checked);
                if (CheckIsRes.Checked == true)
                {
                    trans.IsResigned = true;
                    emp.EmpStatus = 2;
                    if (appuser != null)
                    {
                        appuser.Enabled = false;
                    }
                    if (alemp != null)
                    {
                        alemp.IsActive = false;
                    }

                    foreach (var item in saldet)
                    {
                        item.IsActive = false;
                    }
                }
                else
                {
                    trans.IsResigned = false;
                    emp.EmpStatus = 1;
                    if (appuser != null)
                    {
                        appuser.Enabled = true;
                    }
                    if (alemp != null)
                    {
                        alemp.IsActive = true;
                    }
                    foreach (var item in saldet)
                    {
                        item.IsActive = true;
                    }
                }
                //trans.CreatedBy = "Admin";
                if (Session["UserID"] == null)
                    trans.CreatedBy = Request.Cookies["uzr"]["UserID"].ToString();
                else
                    trans.CreatedBy = Session["UserID"].ToString();
                trans.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                trans.Reason = txtReason.Text.Trim();


                if (AppCycle)
                {
                    if (CanApprove && CanEnter)
                    {
                        if (Session["UserID"] == null)
                            trans.ReqBy = Request.Cookies["uzr"]["UserID"].ToString();
                        else
                            trans.ReqBy = Session["UserID"].ToString();
                        trans.ReqDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                    //if (ddlStatus.SelectedValue.Equals("A"))
                    //{
                    //    if (CanApprove && !CanEnter)
                    //    {
                    //        if (Session["UserID"] == null)
                    //            trans.AppBy = Request.Cookies["uzr"]["UserID"].ToString();
                    //        else
                    //            trans.AppBy = Session["UserID"].ToString();
                    //        trans.AppDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //    }
                    //}
                    //trans.Status = Convert.ToString(ddlStatus.SelectedValue);
                }


                mgtRes.Insert(trans, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
            }

            List<tblPlRsgTran> Resignation = db.tblPlRsgTrans.Where(x => x.EmpId == ID).ToList();
            for (int i = 0; i < Resignation.Count-1; i++)
            {
                Resignation[i].IsResigned = false;
            }
            if (CheckIsRes.Checked == true)
            {
                Resignation[Resignation.Count - 1].IsResigned = true;
            }
            else
            {
                Resignation[Resignation.Count - 1].IsResigned = false;
            }
            db.SubmitChanges();
            
        }

        protected tblPlRsgTran InsertUpdate()
        {
            BL.EmpBL empBL = new EmpBL();
            tblPlEmpData emp = empBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            emp.EmpStatus = 2;
            emp.DOL = Convert.ToDateTime(txtLeavingdate.Text.Trim());
            emp.LvgReason = txtReason.Text.Trim();
            tblPlRsgTran trans = new tblPlRsgTran();
            trans.EmpId = ID;
            trans.Rsg_Code = int.Parse(ddlLeaveReason.SelectedValue);
            trans.TransDate = DateTime.Parse(txtLeavingdate.Text.Trim());
            //trans.CreatedBy = "Admin";
            if (Session["UserID"] == null)
                trans.CreatedBy = Request.Cookies["uzr"]["UserID"].ToString();
            else
                trans.CreatedBy = Session["UserID"].ToString();
            trans.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            trans.Reason = txtReason.Text.Trim();

            
            
            return trans;
        }

        protected void Update()
        {
            RMSDataContext db = new RMSDataContext();
            BL.EmpBL empBL = new EmpBL();
            
            tblAppUser appuser = db.tblAppUsers.Where(x => x.EmpID == ID).FirstOrDefault();
            tblPlRsgTran trans = empBL.GetResgByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            tblPlEmpData emp = empBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            tblPlAlow alemp = db.tblPlAlows.Where(x => x.EmpID == ID && x.IsActive == true).FirstOrDefault();
            List<tblPlSalaryDetail> saldet = db.tblPlSalaryDetails.Where(x => x.EmpID == ID).ToList();
            
            tblPlRsgTran transcopy = InsertUpdate();
            if (trans != null)
            {

            }
            DateTime effective = transcopy.TransDate;
            if (AppCycle)
            {
                if (CanApprove && CanEnter)
                {
                    if (Session["UserID"] == null)
                        transcopy.ReqBy = Request.Cookies["uzr"]["UserID"].ToString();
                    else
                        transcopy.ReqBy = Session["UserID"].ToString();
                    transcopy.ReqDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                //if (ddlStatus.SelectedValue.Equals("A"))
                //{
                //    if (CanApprove && !CanEnter)
                //    {
                //        if (Session["UserID"] == null)
                //            transcopy.AppBy = Request.Cookies["uzr"]["UserID"].ToString();
                //        else
                //            transcopy.AppBy = Session["UserID"].ToString();
                //        transcopy.AppDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //    }
                //}

                transcopy.ReqBy = trans.ReqBy;
                transcopy.ReqDate = trans.ReqDate;
                // transcopy.Status = Convert.ToString(ddlStatus.SelectedValue);
                transcopy.IsResigned = Convert.ToBoolean(CheckIsRes.Checked);
                if (CheckIsRes.Checked == true)
                {
                    emp.EmpStatus = 2;
                    if (appuser != null)
                    {
                        appuser.Enabled = false;
                    }
                    if (alemp != null)
                    {
                        alemp.IsActive = false;
                    }
                    
                    foreach (var item in saldet)
                    {
                        item.IsActive = false;
                    }
                    
                }
                else
                {
                    emp.EmpStatus = 1;
                    if (appuser != null)
                    {
                        appuser.Enabled = true;
                    }
                    if (alemp != null)
                    {
                        alemp.IsActive = true;
                    }
                    foreach (var item in saldet)
                    {
                        item.IsActive = true;
                    }
                }
                
            }
            
            mgtRes.Update((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            mgtRes.Delete(trans, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            mgtRes.Insert(transcopy, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
        }
        private void ClearFields()
        {
            ID = 0;
            CompID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtLeavingdate.Text = "";
            txtReason.Text = "";
            ddlLeaveReason.SelectedIndex = 0;
            if (AppCycle)
            {
               // ddlStatus.SelectedValue = "0";
            }
            EmpSrchUC.ClearFields();
            EmpSrchUC.EditModeDataHide();
            EmpSrchUC.Focus();
        }

        protected void grdResign_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResign.PageIndex = e.NewPageIndex;
            BindGrid(txtFltEmp.Text.Trim(), txtFltEmpCode.Text.Trim());
        }

        #endregion
        protected void grdResign_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdResign.SelectedDataKey.Value);
            GetByID();

        }
        protected void GetByID()
        {
            tblPlRsgTran empRsg = mgtRes.GetResgByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (Session["DateFormat"] == null)
            {
                this.txtLeavingdate.Text = empRsg.TransDate.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
            }
            else
            {

                this.txtLeavingdate.Text = empRsg.TransDate.ToString(Session["DateFormat"].ToString());
            }

            this.ddlLeaveReason.SelectedValue = empRsg.Rsg_Code.ToString();
            this.txtReason.Text = empRsg.Reason;
            this.CheckIsRes.Checked = Convert.ToBoolean(empRsg.IsResigned);
            if (AppCycle)
            {
                if (empRsg.Status != null)
                {
                   // ddlStatus.SelectedValue = empRsg.Status.ToString();
                }
            }
            
            try
            {
                EmpSrchUC.EditModeDataShow(empRsg.tblPlEmpData.FullName, "EN-"+empRsg.EmpId,empRsg.tblPlEmpData.EmpCode, empRsg.tblPlEmpData.tblPlCode1.CodeDesc, empRsg.tblPlEmpData.tblPlCode.CodeDesc);
            }
            catch { }
            EmpSrchUC.EmpIDUC = ID;
           
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

    }
}

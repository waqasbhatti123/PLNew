using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RMS.BL;
using System.Data.Linq;
using System.Web.Services;
using AjaxControlToolkit;
using Microsoft.Reporting.WebForms;
using System.Text;

namespace RMS.GLSetup
{
    public partial class frmVoucherDetail : BasePage
    {
        voucherDetailBL vdBl = new voucherDetailBL();
        voucherHomeBL voucObj = new voucherHomeBL();
        EntitySet<Glmf_Data_Det> gldatadet = new EntitySet<Glmf_Data_Det>();
        GroupBL grpBl = new GroupBL();

        voucherDetailBL objVoucher = new voucherDetailBL();
        DataRow d_Row;
#pragma warning disable CS0169 // The field 'frmVoucherDetail.d_Col' is never used
        DataColumn d_Col;
#pragma warning restore CS0169 // The field 'frmVoucherDetail.d_Col' is never used
        ListItem selitem = new ListItem();
        
        public bool flagEdit
        {
            get { return Convert.ToBoolean(ViewState["flagEdit"]); }
            set { ViewState["flagEdit"] = value; }
        }
        public int voucherNo
        {
            get { return Convert.ToInt32(ViewState["voucherNo"]); }
            set { ViewState["voucherNo"] = value; }
        }
        public string pgTitle
        {
            get { return Convert.ToString(ViewState["pgTitle"]); }
            set { ViewState["pgTitle"] = value; }
        }
        public int pgID
        {
            get { return Convert.ToInt32(ViewState["pgID"]); }
            set { ViewState["pgID"] = value; }
        }
        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
        public int VoucherTypeID
        {
            get { return Convert.ToInt32(ViewState["VoucherTypeID"]); }
            set { ViewState["VoucherTypeID"] = value; }
        }
        public int VoucherID
        {
            get { return Convert.ToInt32(ViewState["VoucherID"]); }
            set { ViewState["VoucherID"] = value; }
        }
        public decimal Financialyear
        {
            get { return Convert.ToDecimal(ViewState["Financialyear"]); }
            set { ViewState["Financialyear"] = value; }
        }
        public DataTable currenttable
        {
            get { return (DataTable)ViewState["currenttable"]; }
            set { ViewState["currenttable"] = value; }
        
        }
        public int GroupID
        {
            get { return Convert.ToInt32(ViewState["GroupID"]); }
            set { ViewState["GroupID"] = value; }
        }

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

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
                    txtChqDateCal.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    txtChqDateCal.Format = Session["DateFullYearFormat"].ToString();
                }
                //if(Session["pgTitle"] != null)
                pgTitle = Session["PageTitle"].ToString();
               // if(Session["PrevPgID"] != null)
                pgID = Convert.ToInt32(Session["PrevPgID"]);
                //if (Session["PageID"] != null)
                //pgID =Convert.ToInt32(Session["PageID"]);

                lblPgTilte.Text = pgTitle;
                VoucherTypeID = Convert.ToInt32(Session["VoucherTypeId"]);
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    divBankData.Visible = true;
                    if (Session["DateFullYearFormat"] == null)
                    {
                        txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    }
                    else
                    {
                        txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Session["DateFullYearFormat"].ToString());
                    }
                }
                else
                {
                    divBankData.Visible = false;
                }
                

                
                //SOURCE & REF NO MANAGEMENT
                if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
                {
                    lblRefSource.Text = "Source";
                }
                else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
                {
                    lblRefSource.Text = "Ref. No.";
                }
                else//JV, OP & IP
                {
                    lblRefSource.Visible = false;
                    txtRefSource.Visible = false;
                }
                //END SOURCE & REF NO MANAGEMENT
                           



                //VoucherID = Convert.ToInt32(Session["VoucherID"]);
                VoucherID = Convert.ToInt32(Session["VoucherId"]);
                if (Session["VouchNo"] != null)
                {
                    string vr_No = Convert.ToString(Session["VouchNo"]).Substring(3);
                    voucherNo = Convert.ToInt32(vr_No);
                }
                Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                PID = Convert.ToInt32(Session["PrevPgID"]);
                txtdt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                if (Session["DateFullYearFormat"] == null)
                {
                    this.txtDate.Format = Request.Cookies["uzr"]["DateFullYearFormat"];
                }
                else
                {
                    this.txtDate.Format = Session["DateFullYearFormat"].ToString();
                }
    
              Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "voucherentry").ToString();

                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();

                BindDataTable();

             
                
                
                //Maintaning Privilage Status==========================
              if (Session["GroupID"] == null)
              {
                  GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
              }
              else
              {
                  GroupID = Convert.ToInt32(Session["GroupID"].ToString());
              }
              int pid = 0;
              pid = Convert.ToInt32(Session["PrevPgID"]);

              tblAppPrivilage appPrivilage = grpBl.GetPrivilageStatus(GroupID, pid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

              if (appPrivilage != null)
              {
                  if (appPrivilage.CanEdit.Equals(false))//approval status given
                  {
                      ddlstatus.Items.RemoveAt(0);
                  }
              }
              else
              {
                  ucMessage.ShowMessage("Enter privilages for the logged in group", RMS.BL.Enums.MessageType.Error);
              }
                //====================================================================

              if (Convert.ToBoolean(Session["aFlag"]) == true)//is update?
              {
                  divRem.Visible = false;
                  ddlstatus.Items.RemoveAt(1);
              }
              else
              {
                  divRem.Visible = true;
                  divRemEntry.Visible = false;
              }
              BindGridRems(VoucherID);

              //Session["DisplayMsg"] = false;
              //Session["MsgType"] = "Info";
              //Session["Msg"] = null;
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

                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                    //this.BindGrid();
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }


        protected void grdRemarks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdRemarks.PageIndex = e.NewPageIndex;
            BindGridRems(VoucherID);
        }

        protected void grdRemarks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text).ToString(System.Configuration.ConfigurationManager.AppSettings["DateTimeWOYearFormat"]);

                Glmf_Data_Rmk rem = voucObj.GetRemarksByID((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], VoucherID, Convert.ToInt32(e.Row.Cells[0].Text));
                if (rem != null)
                {
                    if (rem.Enabled)
                    {
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void BindGridRems(int id)
        {
            grdRemarks.DataSource = voucObj.GetRemarks((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], id);
            grdRemarks.DataBind();
        }

        public void BindDataTable()
        {
            List<spVouchersFill2GridResult> dtl = vdBl.GetVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], VoucherID, BranchID);
            
            DataTable d_table = new DataTable();    
            d_table.Columns.Clear();
            d_table.Rows.Clear();
            d_table.Columns.Add(new DataColumn("seq", typeof(string)));
            d_table.Columns.Add(new DataColumn("code", typeof(string)));
            d_table.Columns.Add(new DataColumn("desc", typeof(string)));
            d_table.Columns.Add(new DataColumn("debit", typeof(string)));
            d_table.Columns.Add(new DataColumn("credit", typeof(string)));
            d_table.Columns.Add(new DataColumn("remark", typeof(string)));
            d_table.Columns.Add(new DataColumn("costcenter", typeof(string)));
            if (VoucherID != 0 && dtl.Count > 0 && Convert.ToBoolean(Session["aFlag"]) == false)
            {
                foreach (spVouchersFill2GridResult l in dtl)
                {
                    flagEdit = true;
                    txtdt.Text = Convert.ToDateTime(l.vr_dt).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                    ddlstatus.SelectedValue = l.vr_apr.ToString();
                    txtnarration.Text = l.narration.ToString();
                    
                    //SOURCE & REF NO MANAGEMENT
                    if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
                    {
                        txtRefSource.Text = l.Ref_no;
                    }
                    else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
                    {
                        txtRefSource.Text = l.Ref_no;
                    }
                    else//JV, OP & IP
                    {
                    }
                    //END SOURCE & REF NO MANAGEMENT
                    
                    d_Row = d_table.NewRow();
                    d_Row["seq"] = l.vr_seq.ToString();
                    d_Row["code"] = l.gl_cd.ToString();
                    d_Row["desc"] = l.gl_dsc.ToString();
                    d_Row["debit"] = l.vrd_debit.ToString();
                    d_Row["credit"] = l.vrd_credit.ToString();
                    d_Row["remark"] = l.remarks.ToString();
                    if (l.cc_nme != null)
                        d_Row["costcenter"] = l.cc_nme.ToString();
                    else
                        d_Row["costcenter"] = "";
                    d_table.Rows.Add(d_Row);
                }
                currenttable = d_table;
                BindGrid();
                SetInitialDropDown(dtl);
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    SetVoucherCheqDet();
                }
                                
            }
            else
            {
                flagEdit = false;
                for (int i = 1; i < 9; i++)
                {
                    d_Row = d_table.NewRow();
                    d_Row["seq"] = i.ToString();
                    d_table.Rows.Add(d_Row);

                }
                currenttable = d_table;
                BindGrid();
                
            }
           
            
            
        }

        private void SetVoucherCheqDet()
        {
            Glmf_Data_chq glmfChq = objVoucher.GetCheqDetByID(VoucherID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (glmfChq != null)
            {
                txtChqBranch.Text = glmfChq.vr_chq_branch;
                txtChqNo.Text = glmfChq.vr_chq;
                if (Session["DateFullYearFormat"] == null)
                {
                    txtChqDate.Text = glmfChq.vr_chq_dt.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    txtChqDate.Text = glmfChq.vr_chq_dt.ToString(Session["DateFullYearFormat"].ToString());
                }
                txtChqAcctNo.Text = glmfChq.vr_chq_ac;
            }
        }

        public void SetInitialDropDown(List<spVouchersFill2GridResult> lst)
        {
            int i = 0;
                foreach(spVouchersFill2GridResult l in lst)
                {
                    DropDownList ddl = (DropDownList)grdView.Rows[i].Cells[6].FindControl("ddlcostcenter");
                    ddl.ClearSelection();
                    if(l.cc_nme != null)
                    ddl.Items.FindByText(l.cc_nme.ToString()).Selected = true;

                    i++;
                }

        }

        protected void linkBtn_Click(object sender, EventArgs e)
        {
            updateBtn_Click(null, null);
        }

        public void BindGrid()
        {
            grdView.DataSource = currenttable;
            grdView.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + pgID);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateGrid())
            {
                ucMessage.ShowMessage("Voucher debit amount should be equal to credit amount", RMS.BL.Enums.MessageType.Error);
                return;
            }

            if (flagEdit == false)
            {
                SaveNew();
            }
            else
            {
                EditPrevious();
            }
        }

        public bool ValidateGrid()
        {
            decimal debt = 0;
            decimal crdt = 0;
            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                string debit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdebit")).Text.Trim();
                string credit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcredit")).Text.Trim();
                if (!string.IsNullOrEmpty(debit))
                    debt = debt + Convert.ToDecimal(debit);
                if (!string.IsNullOrEmpty(credit))
                    crdt = crdt + Convert.ToDecimal(credit);
            }
            if (debt != crdt)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void EditPrevious()
        {
            Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            
            //*************************************************************
            try
            {
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    Glmf_Data_chq chq = new Glmf_Data_chq();
                    chq.vrid = VoucherID;
                    chq.vr_chq_branch = txtChqBranch.Text.Trim();
                    chq.vr_chq = txtChqNo.Text;
                    if (RMS.BL.BranchBL.IsChequeNoExists1(chq, Convert.ToInt32(Session["BranchID"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage("Cheque no already exists", RMS.BL.Enums.MessageType.Error);
                        txtChqNo.Focus();
                        chq = null;
                        return;
                    }
                    chq = null;
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
                return;
            }
            //*************************************************************

            gldatadet.Clear() ;


            string username = "";
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

            int vrID = VoucherID;
            Glmf_Data glmfdata = objVoucher.GetGlmf_Data(VoucherID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (glmfdata != null)
            {
                try
                {
                    glmfdata.vr_dt = Convert.ToDateTime(txtdt.Text.Trim());
                    DateTime dtFrm = DateTime.Parse("01-Jul-" + Financialyear);
                    dtFrm = dtFrm.AddYears(-1);
                    DateTime dtTo = DateTime.Parse("30-Jun-" + Financialyear);
                    if (glmfdata.vr_dt >= dtFrm && glmfdata.vr_dt <= dtTo)
                    {
                        //ok
                    }
                    else if (glmfdata.vr_dt >= DateTime.Parse("01-Jul-" + Financialyear)
                    && glmfdata.vr_dt <= DateTime.Parse("30-Jun-" + (Financialyear + 1))
                    && glmfdata.vr_dt <= Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date
                    )
                    {
                        Financialyear++;
                    }
                    else
                    {
                        ucMessage.ShowMessage("Voucher Date should be within these financial years i.e. " + Financialyear + " and " + (Financialyear + 1), RMS.BL.Enums.MessageType.Error);
                        txtDate.Focus();
                        return;
                    }

                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                    {
                        ucMessage.ShowMessage("Voucher Date is invalid", RMS.BL.Enums.MessageType.Error);
                        txtDate.Focus();
                    }
                    return;
                }

                glmfdata.Gl_Year = Financialyear;

                
                glmfdata.vr_nrtn = txtnarration.Text.Trim();
                glmfdata.vr_apr = Convert.ToString(ddlstatus.SelectedItem.Value);

                if (glmfdata.vr_apr == "A")
                {
                    glmfdata.approvedby = username;
                    glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                else
                {
                    glmfdata.updateby = username;
                    glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                //SOURCE & REF NO MANAGEMENT
                if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
                {
                    glmfdata.Ref_no = txtRefSource.Text;
                }
                else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
                {
                    glmfdata.Ref_no = txtRefSource.Text;
                }
                else//JV, OP & IP
                {
                }
                //END SOURCE & REF NO MANAGEMENT

                int count = 0;
                for (int i = 0; i < grdView.Rows.Count; i++)
                {
                    //Glmf_Data_Det gldet = new Glmf_Data_Det();//UPDATE HERE CHILD TABLE
                    string vr_seq = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtseq")).Text.Trim();
                    string code = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcode")).Text.Trim();
                    string debit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdebit")).Text.Trim();
                    string credit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcredit")).Text.Trim();
                    string remarks = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtremarks")).Text.Trim();
                    //string costcenter = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcostcenter")).Text;
                    string costcenter = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlcostcenter")).SelectedItem.Value;

                    if (String.IsNullOrEmpty(code) == false)
                    {
                        count = count+1;
                        //Glmf_Data_Det gldet = objVoucher.GetGlmf_Data_Det(VoucherID, Convert.ToInt32(vr_seq), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        //if (gldet != null)
                        //{
                        //    gldet.gl_cd = code;
                        //    try
                        //    {
                        //        if (debit != null && debit.Trim().Equals(""))
                        //        {
                        //            gldet.vrd_debit = 0;
                        //        }
                        //        else
                        //        {
                        //            gldet.vrd_debit = Convert.ToDecimal(debit);
                        //        }
                        //    }
                        //    catch
                        //    {
                        //        gldet.vrd_debit = 0;
                        //    }

                        //    try
                        //    {
                        //        if (credit != null && credit.Trim().Equals(""))
                        //        {
                        //            gldet.vrd_credit = 0;
                        //        }
                        //        else
                        //        {
                        //            gldet.vrd_credit = Convert.ToDecimal(credit);
                        //        }
                        //    }
                        //    catch
                        //    {
                        //        gldet.vrd_credit = 0;
                        //    }
                        //    try
                        //    {
                        //        if (remarks.Length > 500)
                        //            remarks = remarks.Substring(0, 499);
                        //    }
                        //    catch { }

                        //    gldet.vrd_nrtn = remarks;

                        //    if (costcenter == "")
                        //    {
                        //        gldet.cc_cd = null;
                        //    }
                        //    else
                        //    {
                        //        gldet.cc_cd = costcenter;
                        //    }
                        //}
                        //else
                        //{
                        Glmf_Data_Det gldet = new Glmf_Data_Det();
                            
                            gldet.vrid = VoucherID;
                            gldet.vr_seq = count;
                            gldet.gl_cd = code;
                            try
                            {
                                if (debit != null && debit.Trim().Equals(""))
                                {
                                    gldet.vrd_debit = 0;
                                }
                                else
                                {
                                    gldet.vrd_debit = Convert.ToDecimal(debit);
                                }
                            }
                            catch
                            {
                                gldet.vrd_debit = 0;
                            }

                            try
                            {
                                if (credit != null && credit.Trim().Equals(""))
                                {
                                    gldet.vrd_credit = 0;
                                }
                                else
                                {
                                    gldet.vrd_credit = Convert.ToDecimal(credit);
                                }
                            }
                            catch
                            {
                                gldet.vrd_credit = 0;
                            }
                            try
                            {
                                if (remarks.Length > 500)
                                    remarks = remarks.Substring(0, 499);
                            }
                            catch { }

                            gldet.vrd_nrtn = remarks;

                            if (costcenter == "")
                            {
                                gldet.cc_cd = null;
                            }
                            else

                                gldet.cc_cd = costcenter;
                        //}
                        gldatadet.Add(gldet);
                    }
                }
            }
   
            Glmf_Data_chq glmfChq = null;
            if (VoucherTypeID == 3 || VoucherTypeID == 5)
            {
                glmfChq = objVoucher.GetGlmf_Data_Chq(VoucherID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                if (glmfChq != null)
                {
                    glmfChq.vr_chq_branch = txtChqBranch.Text.Trim();
                    glmfChq.vr_chq = txtChqNo.Text.Trim();
                    try
                    {
                        glmfChq.vr_chq_dt = Convert.ToDateTime(txtChqDate.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                        {
                            ucMessage.ShowMessage("Cheque Date is invalid", RMS.BL.Enums.MessageType.Error);
                            txtChqDate.Focus();
                        }
                        return;
                    }

                    glmfChq.vr_chq_ac = txtChqAcctNo.Text.Trim();
                }
                
            }
            string vrDescptn = objVoucher.VoucherDesc(VoucherTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                //glmfdata.Glmf_Data_Dets = gldatadet;
                objVoucher.UpdateVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);

                voucObj.UpdateRemarksStatus((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], vrID);
                
                
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    objVoucher.UdateChq((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfChq);
                }
                if (glmfdata.vr_apr.ToString().Equals("A"))
                {
                    if (VoucherTypeID == 55)
                    {
                        objVoucher.PostVoucherOpeningBalDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                    }
                    else
                    {
                        objVoucher.PostVoucherDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                    }
                }
                ucMessage.ShowMessage("voucher " + vrDescptn + '-' + voucherNo + " has been updated successfully", RMS.BL.Enums.MessageType.Info);
                ClearFields();
                //Session["DisplayMsg"] = true;
                //Session["MsgType"] = "Info";
                //Session["Msg"] = "voucher " + vrDescptn + '-' + voucherNo + " has been updated successfully";
            }
            catch (Exception e)
            {
                if (e.Message.Contains("An attempt was made to remove a relationship between a Glmf_Data and a Glmf_Data_Det. However, one of the relationship's foreign keys (Glmf_Data_Det.vrid) cannot be set to null."))
                {
                    ucMessage.ShowMessage("Please enter GL Code", RMS.BL.Enums.MessageType.Error);
                    //Session["DisplayMsg"] = true;
                    //Session["MsgType"] = "Error";
                    //Session["Msg"] = "Please enter GL Code";
                }
                else
                {
                    ucMessage.ShowMessage("Exception occured in updating data.<br/>Exception is: " + e.Message, RMS.BL.Enums.MessageType.Error);
                    //Session["DisplayMsg"] = true;
                    //Session["MsgType"] = "Error";
                    //Session["Msg"] = "Exception occured in updating data.<br/>Exception is: " + e.Message;
                }
                return;
            }
            
            //System.Threading.Thread.Sleep(5000);
            //Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + pgID);
            //Response.Redirect("~/GLSetup/frmVoucherDetail.aspx?PID=327");
            

        }

        public void SaveNew()
        {
            Financialyear = objVoucher.GetFinancialYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //*************************************************************
            try
            {
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    Glmf_Data_chq chq = new Glmf_Data_chq();
                    chq.vr_chq_branch = txtChqBranch.Text.Trim();
                    chq.vr_chq = txtChqNo.Text;
                    if (RMS.BL.BranchBL.IsChequeNoExists1(chq, Convert.ToInt32(Session["BranchID"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage("Cheque no already exists", RMS.BL.Enums.MessageType.Error);
                        txtChqNo.Focus();
                        chq = null;
                        return;
                    }
                    chq = null;
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
                return;
            }
            //*************************************************************
            
            string username = "";
            if(Session["LoginID"] == null)
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
            
            Glmf_Data glmfdata = new Glmf_Data();
            try
            {
                glmfdata.vr_dt = Convert.ToDateTime(txtdt.Text.Trim());
                DateTime dtFrm = DateTime.Parse("01-Jul-" + Financialyear);
                dtFrm = dtFrm.AddYears(-1);
                DateTime dtTo = DateTime.Parse("30-Jun-" + Financialyear);
                if (glmfdata.vr_dt >= dtFrm && glmfdata.vr_dt <= dtTo)
                {
                    //ok
                }
                else if (    glmfdata.vr_dt >= DateTime.Parse("01-Jul-" + Financialyear) 
                    &&  glmfdata.vr_dt <= DateTime.Parse("30-Jun-" + (Financialyear+1))
                    &&  glmfdata.vr_dt <= Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date
                    )
                {
                    Financialyear++;
                }
                else
                {
                    ucMessage.ShowMessage("Voucher Date should be within these financial years i.e. " + Financialyear + " and " + (Financialyear + 1), RMS.BL.Enums.MessageType.Error);
                    txtDate.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                {
                    ucMessage.ShowMessage("Voucher Date is invalid", RMS.BL.Enums.MessageType.Error);
                    txtDate.Focus();
                }
                return;
            }


            int voucherno = objVoucher.GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], BranchID, VoucherTypeID, Financialyear, null);
            
            glmfdata.br_id = 1;
            glmfdata.Gl_Year = Financialyear;
            glmfdata.vt_cd = Convert.ToInt16(VoucherTypeID);
            glmfdata.vr_no = voucherno;
           

            glmfdata.vr_nrtn = txtnarration.Text.Trim();
            glmfdata.vr_apr = Convert.ToString(ddlstatus.SelectedItem.Value);
           

            //SOURCE & REF NO MANAGEMENT
            if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
            {
                glmfdata.Ref_no = txtRefSource.Text;
            }
            else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
            {
                glmfdata.Ref_no = txtRefSource.Text;
            }
            else//JV, OP & IP
            {
            }
            //END SOURCE & REF NO MANAGEMENT


            glmfdata.updateby = username;
            glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (glmfdata.vr_apr == "A")
            {
                glmfdata.approvedby = username;
                glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            int count = 1;
            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                Glmf_Data_Det gldet = new Glmf_Data_Det();
                string vr_seq = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtseq")).Text.Trim();
                string code = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcode")).Text.Trim();
                string debit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdebit")).Text.Trim();
                string credit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcredit")).Text.Trim();
                string remarks = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtremarks")).Text.Trim();
                //string costcenter = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcostcenter")).Text;
                string costcenter = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlcostcenter")).SelectedItem.Value;

                if (String.IsNullOrEmpty(code) == false)
                {
                    gldet.vr_seq = count++;
                    gldet.gl_cd = code;
                    try
                    {
                        if (debit != null && debit.Trim().Equals(""))
                        {
                            gldet.vrd_debit = 0;
                        }
                        else
                        {
                            gldet.vrd_debit = Convert.ToDecimal(debit);
                        }
                    }
                    catch
                    {
                        gldet.vrd_debit = 0;
                    }

                    try
                    {
                        if (credit != null && credit.Trim().Equals(""))
                        {
                            gldet.vrd_credit = 0;
                        }
                        else
                        {
                            gldet.vrd_credit = Convert.ToDecimal(credit);
                        }
                    }
                    catch 
                    {
                        gldet.vrd_credit = 0;
                    }
                    try
                    {
                        if (remarks.Length > 500)
                            remarks = remarks.Substring(0, 499);
                    }
                    catch { }
                    gldet.vrd_nrtn = remarks;
                    if (costcenter == "")
                    {
                        gldet.cc_cd = null;
                    }
                    else

                        gldet.cc_cd = costcenter;
                    gldatadet.Add(gldet);
                }


            }

            Glmf_Data_chq glmfChq = null;
            if (VoucherTypeID == 3 || VoucherTypeID == 5)
            {
                glmfChq = new Glmf_Data_chq();
                glmfChq.vr_chq_branch = txtChqBranch.Text.Trim();
                glmfChq.vr_chq = txtChqNo.Text.Trim();
                try
                {
                    glmfChq.vr_chq_dt = Convert.ToDateTime(txtChqDate.Text.Trim());
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("The string was not recognized as a valid DateTime"))
                    {
                        ucMessage.ShowMessage("Cheque Date is invalid", RMS.BL.Enums.MessageType.Error);
                        txtChqDate.Focus();
                    }
                    return;
                }
                
                glmfChq.vr_chq_ac = txtChqAcctNo.Text.Trim();
            }

            string vrDescptn = objVoucher.VoucherDesc(VoucherTypeID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                objVoucher.SaveVoucher((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                if (VoucherTypeID == 3 || VoucherTypeID == 5)
                {
                    objVoucher.SaveVoucherCheqDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata.vrid, glmfChq);
                }
                if (glmfdata.vr_apr.ToString().Equals("A"))
                {
                    if (VoucherTypeID == 55)
                    {
                        objVoucher.PostVoucherOpeningBalDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                    }
                    else
                    {
                        objVoucher.PostVoucherDet((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], glmfdata, gldatadet);
                    }
                }
                ucMessage.ShowMessage("voucher " + vrDescptn + '-' + voucherno + " has been posted successfully", RMS.BL.Enums.MessageType.Info);
                ClearFields();
                //Session["DisplayMsg"] = true;
                //Session["MsgType"] = "Info";
                //Session["Msg"] = "voucher " + vrDescptn + '-' + voucherno + " has been posted successfully";
            }
            catch (Exception e)
            {
                ucMessage.ShowMessage("Exception occured in saving data.<br/>Exception is: " + e.Message, RMS.BL.Enums.MessageType.Error);
                //Session["DisplayMsg"] = true;
                //Session["MsgType"] = "Error";
                //Session["Msg"] = "Exception occured in saving data.<br/>Exception is: " + e.Message;
                return;
            }

            //System.Threading.Thread.Sleep(5000);
            //Response.Redirect("~/GLSetup/frmVoucherHome.aspx?PID=" + pgID);
            //Response.Redirect("~/GLSetup/frmVoucherDetail.aspx?PID=327");
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
          
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DropDownList ddl = e.Row.FindControl("ddlcostcenter") as DropDownList;
                ((DropDownList)e.Row.FindControl("ddlcostcenter")).DataTextField = "cc_nme";
                ((DropDownList)e.Row.FindControl("ddlcostcenter")).DataValueField= "cc_cd";
                //((DropDownList)e.Row.FindControl("ddlcostcenter")).DataSource = objVoucher.GetAllCostCenter((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ((DropDownList)e.Row.FindControl("ddlcostcenter")).DataSource = new CCBL().GetGLCC((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ((DropDownList)e.Row.FindControl("ddlcostcenter")).DataBind();
                
               
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                
            }
           
        }

        protected void SetDropDown()
        {
            
            if (currenttable != null)
            { 
                for(int i=0;i < grdView.Rows.Count; i++)
                {
                    DropDownList ddl = (DropDownList)grdView.Rows[i].Cells[6].FindControl("ddlcostcenter");
                    ddl.ClearSelection();
                    ddl.Items.FindByText(currenttable.Rows[i]["costcenter"].ToString()).Selected = true;


                }
            
            }
                
        }

        protected void updateBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("seq", typeof(string)));
                dt.Columns.Add(new DataColumn("code", typeof(string)));
                dt.Columns.Add(new DataColumn("desc", typeof(string)));
                dt.Columns.Add(new DataColumn("debit", typeof(string)));
                dt.Columns.Add(new DataColumn("credit", typeof(string)));
                dt.Columns.Add(new DataColumn("remark", typeof(string)));
                dt.Columns.Add(new DataColumn("costcenter", typeof(string)));
    //            dt.Columns.Add(new DataColumn("cost", typeof(string)));
                
            for (int i = 0; i < grdView.Rows.Count; i++)
                {
                    string vr_seq = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtseq")).Text;
                    string code = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcode")).Text;
                    string debit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdebit")).Text;
                    string desc = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtdescription")).Text;
                    string credit = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcredit")).Text;
                    string remarks = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtremarks")).Text;
                    string costcenter = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlcostcenter")).SelectedItem.Text;
  //                  string hidden = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlcostcenter")).SelectedItem.Value;
             //       string costcenter = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtcostcenter")).Text;
                    d_Row = dt.NewRow();
                    d_Row["seq"] = vr_seq;
                    d_Row["code"] = code;
                    d_Row["desc"] = desc;
                    d_Row["debit"] = debit;
                    d_Row["credit"] = credit;
                    d_Row["remark"] = remarks;
                    d_Row["costcenter"] = costcenter;
      //              d_Row["cost"] = hidden;
      
                    dt.Rows.Add(d_Row);
                    dt.AcceptChanges();
                }
            for(int k=1; k<6;k++)
            {
                d_Row = dt.NewRow();
                d_Row["seq"] = grdView.Rows.Count + k;
                dt.Rows.Add(d_Row);
            }
                dt.AcceptChanges();
                currenttable = dt;
                BindGrid();
                SetDropDown();
                btnSave.Focus();
            }

        private void ClearFields()
        {
            //VoucherTypeID = Convert.ToInt32(Session["VoucherTypeId"].ToString());
            VoucherID = 0;
            Session["aFlag"] = true;
            txtnarration.Text = "";
            txtDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
            txtRefSource.Text = "";


            if (VoucherTypeID == 3 || VoucherTypeID == 5)
            {
                divBankData.Visible = true;
                if (Session["DateFullYearFormat"] == null)
                {
                    txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                else
                {
                    txtChqDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString(Session["DateFullYearFormat"].ToString());
                }
                txtChqBranch.Text = "";
                txtChqAcctNo.Text = "";
                txtChqNo.Text = "";
            }
            else
            {
                divBankData.Visible = false;
            }

            //SOURCE & REF NO MANAGEMENT
            if (VoucherTypeID.Equals(4) || VoucherTypeID.Equals(5))//CRV & BRV
            {
                lblRefSource.Text = "Source";
            }
            else if (VoucherTypeID.Equals(2) || VoucherTypeID.Equals(3))//CPV & BPV
            {
                lblRefSource.Text = "Ref. No.";
            }
            else//JV, OP & IP
            {
                lblRefSource.Visible = false;
                txtRefSource.Visible = false;
            }
            //END SOURCE & REF NO MANAGEMENT

            DataTable d_table = new DataTable();
            d_table.Columns.Clear();
            d_table.Rows.Clear();
            d_table.Columns.Add(new DataColumn("seq", typeof(string)));
            d_table.Columns.Add(new DataColumn("code", typeof(string)));
            d_table.Columns.Add(new DataColumn("desc", typeof(string)));
            d_table.Columns.Add(new DataColumn("debit", typeof(string)));
            d_table.Columns.Add(new DataColumn("credit", typeof(string)));
            d_table.Columns.Add(new DataColumn("remark", typeof(string)));
            d_table.Columns.Add(new DataColumn("costcenter", typeof(string)));
            flagEdit = false;
            for (int i = 1; i < 9; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["seq"] = i.ToString();
                d_table.Rows.Add(d_Row);

            }
            currenttable = d_table;
            BindGrid();
        }

        protected void lnkInfo_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                int rowIndex = clickedRow.RowIndex;
                if (rowIndex > -1)
                {
                    string glCode = ((TextBox)grdView.Rows[rowIndex].FindControl("txtcode")).Text;
                    if (!string.IsNullOrEmpty(glCode))
                    {
                        String script = "window.open('../GLSetup/ViewLedgerCard.aspx?Code=" + glCode + "', 'myPopup','directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,addressbar=0')";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), glCode, script, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }


        

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
            public static string GetCodeDesc(string glCd)
            {
                voucherDetailBL vrBl = new voucherDetailBL();
                return vrBl.GetCodeDesc(glCd, Convert.ToInt32(HttpContext.Current.Session["BranchID"]), (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);

            }
        
           [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
           public static List<spGetBankA_CResult> GetBranch(string bank)
           {
               voucherDetailBL vrBl = new voucherDetailBL();
               List<spGetBankA_CResult> acc =  vrBl.GetBranch(Convert.ToInt32(HttpContext.Current.Session["BranchID"]), bank, (RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
               return acc;
           }
        
    }
}

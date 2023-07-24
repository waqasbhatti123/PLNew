using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;

namespace RMS.profile
{
    public partial class EmpMgtSalary : BasePage
    {

        #region DataMembers
        //RMS.BL.tblAppEmp usr;

        GroupBL groupManager = new GroupBL();
        PlAllowBL allowBL = new PlAllowBL();
        //EmpBL empBL = new EmpBL();

        ListItem selList = new ListItem();
        ListItem selListSub = new ListItem();

        #endregion

        #region Properties
#pragma warning disable CS0114 // 'EmpMgtSalary.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int? ID
#pragma warning restore CS0114 // 'EmpMgtSalary.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int CompID
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        public string EffDateStr
        {
            get { return (ViewState["EffDateStr"] == null) ? "" : Convert.ToString(ViewState["EffDateStr"]); }
            set { ViewState["EffDateStr"] = value; }
        }
        public string CurPayPeriod
        {
            get { return (ViewState["CurPayPeriod"] == null) ? "" : Convert.ToString(ViewState["CurPayPeriod"]); }
            set { ViewState["CurPayPeriod"] = value; }
            
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpSalSetup").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                if (Session["CompID"] == null)
                {
                    CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
                }
                else
                {
                    CompID = Convert.ToByte(Session["CompID"].ToString());
                }


                if (Session["DateFormat"] == null)
                {
                    txtEffDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                }
                else
                {
                    txtEffDateCal.Format = Session["DateFormat"].ToString();
                }
                if (Session["CurPayPeriod"] == null)
                {
                    CurPayPeriod = Request.Cookies["uzr"]["CurPayPeriod"];
                }
                else
                {
                    CurPayPeriod = Session["CurPayPeriod"].ToString();
                }
                //BindGrid("", 0, 0);
                ucButtons.ValidationGroupName = "main";
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            if (EmpSrchUC.EmpBindGrid.Equals("Yes"))
            {
                EmpSrchUC.EmpBindGrid = "No";
                BindGrid(EmpSrchUC.EmpIDUC);
                ID = EmpSrchUC.EmpIDUC;
            }
            base.OnLoadComplete(e);
        }
        protected void grdEmps_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ClearFields();
            ID = Convert.ToInt32(grdEmps.SelectedDataKey.Values["EmpID"].ToString());
            CompID = Convert.ToInt32(grdEmps.SelectedDataKey.Values["CompID"].ToString());
            EffDateStr = grdEmps.SelectedDataKey.Values["EffDate"].ToString();
            this.GetByID();

        }
        protected void grdEmps_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEmps.PageIndex = e.NewPageIndex;
            BindGrid(ID);
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                btnDelete.Visible = false;
            }
            else if (e.CommandName == "Save")
            {
                tblPlAlow allow = new tblPlAlow();
                allow.CompID = (byte)CompID;
                allow.EmpID = ID.Value;// Convert.ToInt32(txtEmpCode.Text.Trim());
                if (allow.EmpID < 1)
                {
                    ucMessage.ShowMessage("Please select Employee", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                try
                {
                    allow.EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());
                }
                catch
                {
                    ucMessage.ShowMessage("Please enter correct effective date", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                
                string validMsg = ValidateAllAmounts();
                if (!validMsg.Equals(""))
                {
                    ucMessage.ShowMessage(validMsg, RMS.BL.Enums.MessageType.Error);
                    return;
                }

                int curPayPeriod = 0;
                int.TryParse(CurPayPeriod, out curPayPeriod);
                int effDatePeriod = 0;
                int.TryParse(allow.EffDate.ToString("yyyyMM"), out effDatePeriod);

                tblPlEmpData emp = new EmpBL().GetByID(allow.EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                
                //if (effDatePeriod > curPayPeriod)
                //{
                    if (allow.EffDate >= emp.DOJ)
                    {
                        if (!allowBL.ISAlreadyExist4UpdateSal(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                        {
                            this.Insert();
                            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                        }
                        else
                        {
                            if (effDatePeriod >= curPayPeriod)
                            {
                                this.Update();
                                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                            }
                            else
                            {
                                ucMessage.ShowMessage("Cannot update salary package when Effective Date is less than Process Month.", RMS.BL.Enums.MessageType.Error);
                            }
                        }
                    //}
                    //else
                    //{
                    //    ucMessage.ShowMessage("Effective date can't be less than joining date.", RMS.BL.Enums.MessageType.Error);
                    //}
                }
                else
                {
                    ucMessage.ShowMessage("Effective date can't be less than current period.", RMS.BL.Enums.MessageType.Error);
                }

            }
            else if (e.CommandName == "Delete")
            {
                // TRANSACTION WALA KAAM KARNA HAI......
                try
                {
                    this.Delete();
                    //pnlMain.Enabled = false;
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletionDependency").ToString(), RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        Session["errors"] = ex.Message;
                        Response.Redirect("~/home/Error.aspx");
                    }
                }

                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid(ID);
                ClearFields();

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
                    e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[1].Text = DateTime.Parse(e.Row.Cells[1].Text).ToString(Session["DateFormat"].ToString());
                }

            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            tblPlAlow allow = new tblPlAlow();
            allow.CompID = (byte)CompID;
            allow.EmpID = ID.Value;// Convert.ToInt32(txtEmpCode.Text.Trim());
            allow.EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());

            int payPeriod = 0;
            int.TryParse(CurPayPeriod, out payPeriod);
            int prd = 0;
            int.TryParse(allow.EffDate.ToString("yyyyMM"), out prd);

            if (prd >= payPeriod)
            {
                //if (!allowBL.ISAlreadyExist(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                //{
                    this.Delete();
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
                    BindGrid(ID);
                    ClearFields();
                //}
            }
            else
            {
                ucMessage.ShowMessage("Effective date can't be less than current period.", RMS.BL.Enums.MessageType.Info);
            }


        }

        #endregion

        #region Helping Method

        protected void BindGrid(int? EmpID)
        {
            this.grdEmps.DataSource = allowBL.GetAll(EmpID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdEmps.DataBind();
        }
        protected void GetByID()
        {
            tblPlAlow empPojo = allowBL.GetByID(CompID, ID.Value, DateTime.Parse(EffDateStr), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //this.txtEmpCode.Text = empPojo.EmpID.ToString();
            //this.txtFullName.Text = empPojo.FirstName + ' ' + empPojo.MidName + ' ' + empPojo.SirName;

            //EmpSrchUC.EditModeDataShow(empPojo.tblPlEmpData.FullName,
            //    empPojo.tblPlEmpData.EmpCode,
            //    empPojo.tblPlEmpData.tblPlCode1 != null ? empPojo.tblPlEmpData.tblPlCode1.CodeDesc : "",
            //    empPojo.tblPlEmpData.tblPlCode != null ? empPojo.tblPlEmpData.tblPlCode.CodeDesc : "");


            if (Session["DateFormat"] == null)
            {
                txtEffDate.Text = empPojo.EffDate.ToString(Request.Cookies["uzr"]["DateFormat"]);

            }
            else
            {
                txtEffDate.Text = empPojo.EffDate.ToString(Session["DateFormat"].ToString());
            }
            
            txtBasicPay.Text = empPojo.Basic.ToString();
            txtHouseRent.Text = empPojo.HR == 0?"":empPojo.HR.ToString();
            txtSplAll.Text = empPojo.SplAlow == 0 ? "" : empPojo.SplAlow.ToString();
            txtUtilities.Text = empPojo.Utilities == 0 ? "" : empPojo.Utilities.ToString();
            txtFuelAll.Text = empPojo.CA == 0 ? "" : empPojo.CA.ToString();
            txtAllounce.Text = empPojo.NSHA == 0 ? "" : empPojo.NSHA.ToString();
            txtTaxDed.Text = empPojo.TaxDed == 0 ? "" : empPojo.TaxDed.ToString();
            txtMessDed.Text = empPojo.MessDed == 0 ? "" : empPojo.MessDed.ToString();
            txtOtherDed.Text = empPojo.OtherDed == 0 ? "" : empPojo.OtherDed.ToString();

            txtTotalPay.Text = ((empPojo.Basic + empPojo.HR + empPojo.Utilities + empPojo.CA + empPojo.NSHA + empPojo.SplAlow) 
                                 - (empPojo.MessDed + empPojo.TaxDed + empPojo.OtherDed)).ToString();
            //txtTotalPay.Text = (Convert.ToInt32(txtBasicPay.Text) + Convert.ToInt32(txtHouseRent.Text) 
            //+ Convert.ToInt32(txtUtilities.Text) + Convert.ToInt32(txtFuelAll.Text) 
            //+ Convert.ToInt32(txtAllounce.Text) + Convert.ToInt32(txtSplAll.Text)).ToString();

            pnlMain.Enabled = true;

            int curPayPeriod = 0;
            int.TryParse(CurPayPeriod, out curPayPeriod);
            int effDatePeriod = 0;
            int.TryParse(empPojo.EffDate.ToString("yyyyMM"), out effDatePeriod);

            if (effDatePeriod >= curPayPeriod)
            {
                btnDelete.Visible = true;
            }

            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }
        protected void Update()
        {
            tblPlAlow allow = allowBL.GetByID(CompID, ID.Value, Convert.ToDateTime(txtEffDate.Text.Trim()), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //allow.CompID = Convert.ToByte(Session["CompID"].ToString());
            //allow.EmpID = EmpSrchUC.EmpIDUC;// Convert.ToInt32(txtEmpCode.Text.Trim());
            //allow.EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());
            if (txtBasicPay.Text.Trim().Equals(""))
            {
                allow.Basic = 0;
            }
            else
            {
                allow.Basic = Convert.ToDecimal(txtBasicPay.Text.Trim());
            }

            if (txtHouseRent.Text.Trim().Equals(""))
            {
                allow.HR = 0;
            }
            else
            {
                allow.HR = Convert.ToDecimal(txtHouseRent.Text.Trim());
            }

            if (txtUtilities.Text.Trim().Equals(""))
            {
                allow.Utilities = 0;
            }
            else
            {
                allow.Utilities = Convert.ToDecimal(txtUtilities.Text.Trim());
            }

            if (txtAllounce.Text.Trim().Equals(""))
            {
                allow.NSHA = 0;
            }
            else
            {
                allow.NSHA = Convert.ToInt16(txtAllounce.Text.Trim());
            }

            if (txtSplAll.Text.Trim().Equals(""))
            {
                allow.SplAlow = 0;
            }
            else
            {
                allow.SplAlow = Convert.ToDecimal(txtSplAll.Text.Trim());
            }

            if (txtFuelAll.Text.Trim().Equals(""))
            {
                allow.CA = 0;
            }
            else
            {
                allow.CA = Convert.ToDecimal(txtFuelAll.Text.Trim());
            }
            if (txtTaxDed.Text.Trim().Equals(""))
            {
                allow.TaxDed = 0;
            }
            else
            {
                allow.TaxDed = Convert.ToDecimal(txtTaxDed.Text.Trim());
            }

            if (txtMessDed.Text.Trim().Equals(""))
            {
                allow.MessDed = 0;
            }
            else
            {
                allow.MessDed = Convert.ToDecimal(txtMessDed.Text.Trim());
            }


            if (txtOtherDed.Text.Trim().Equals(""))
            {
                allow.OtherDed = 0;
            }
            else
            {
                allow.OtherDed = Convert.ToDecimal(txtOtherDed.Text.Trim());
            }

            //if (!allowBL.ISAlreadyExist(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //{
            string msg = allowBL.Update(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (msg == "ok")
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid(ID);
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(msg, RMS.BL.Enums.MessageType.Error);
            }
            //}
            //else
            //{
            //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
            //    pnlMain.Enabled = true;
            //}
        }
        protected void Delete()
        {
            allowBL.DeleteByID(CompID, ID.Value, DateTime.Parse(EffDateStr), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        }
        protected string ValidateAllAmounts()
        {
            try
            {
                if (Convert.ToDecimal(txtBasicPay.Text.Trim()) < 1)
                {
                    return "Please enter some basic pay";
                }
            }
            catch
            {
                return "Please enter correct amount in basic pay";
            }
            if (!txtHouseRent.Text.Trim().Equals(""))
            {
                try
                {
                    Convert.ToDecimal(txtHouseRent.Text.Trim());
                }
                catch
                {
                    return "Please enter correct amount in house rent";
                }
            }
            if (!txtUtilities.Text.Trim().Equals(""))
            {
                try
                {
                    Convert.ToDecimal(txtUtilities.Text.Trim());
                }
                catch
                {
                    return "Please enter correct amount in utilities";
                }
            }
            if (!txtFuelAll.Text.Trim().Equals(""))
            {
                try
                {
                    Convert.ToDecimal(txtFuelAll.Text.Trim());
                }
                catch
                {
                    return "Please enter correct amount in conveyance allowance";
                }
            }

            if (!txtAllounce.Text.Trim().Equals(""))
            {
                try
                {
                    Convert.ToDecimal(txtAllounce.Text.Trim());
                }
                catch
                {
                    return "Please enter correct amount in H/Ship allowance";
                }
            } 
            if (!txtSplAll.Text.Trim().Equals(""))
            {
                try
                {
                    Convert.ToDecimal(txtSplAll.Text.Trim());
                }
                catch
                {
                    return "Please enter correct amount in special allowance";
                }
            }
            if (!txtMessDed.Text.Trim().Equals(""))
            {
                try
                {
                    Convert.ToDecimal(txtMessDed.Text.Trim());
                }
                catch
                {
                    return "Please enter correct amount in mess deduction";
                }
            } 
            if (!txtTaxDed.Text.Trim().Equals(""))
            {
                try
                {
                    Convert.ToDecimal(txtTaxDed.Text.Trim());
                }
                catch
                {
                    return "Please enter correct amount in tax deduction";
                }
            } if (!txtOtherDed.Text.Trim().Equals(""))
            {
                try
                {
                    Convert.ToDecimal(txtOtherDed.Text.Trim());
                }
                catch
                {
                    return "Please enter correct amount in other deduction";
                }
            }

            return "";
        }
        protected void Insert()
        {
               
            tblPlAlow allow = new tblPlAlow();
          
            allow.CompID = (byte)CompID;
            allow.EmpID = ID.Value;// Convert.ToInt32(txtEmpCode.Text.Trim());
            allow.EffDate = Convert.ToDateTime(txtEffDate.Text.Trim());

            if (txtBasicPay.Text.Trim().Equals(""))
            {
                allow.Basic = 0;
            }
            else
            {
                allow.Basic = Convert.ToDecimal(txtBasicPay.Text.Trim());
            }

            if (txtHouseRent.Text.Trim().Equals(""))
            {
                allow.HR = 0;
            }
            else
            {
                allow.HR = Convert.ToDecimal(txtHouseRent.Text.Trim());
            }

            if (txtUtilities.Text.Trim().Equals(""))
            {
                allow.Utilities = 0;
            }
            else
            {
                allow.Utilities = Convert.ToDecimal(txtUtilities.Text.Trim());
            }

            if (txtSplAll.Text.Trim().Equals(""))
            {
                allow.SplAlow = 0;
            }
            else
            {
                allow.SplAlow = Convert.ToDecimal(txtSplAll.Text.Trim());
            }

            if (txtFuelAll.Text.Trim().Equals(""))
            {
                allow.CA = 0;
            }
            else
            {
                allow.CA = Convert.ToDecimal(txtFuelAll.Text.Trim());
            }

            if (txtAllounce.Text.Trim().Equals(""))
            {
                allow.NSHA = 0;
            }
            else
            {
                allow.NSHA = Convert.ToDecimal(txtAllounce.Text.Trim());
            }

            if (txtTaxDed.Text.Trim().Equals(""))
            {
                allow.TaxDed = 0;
            }
            else
            {
                allow.TaxDed = Convert.ToDecimal(txtTaxDed.Text.Trim());
            }

            if (txtMessDed.Text.Trim().Equals(""))
            {
                allow.MessDed = 0;
            }
            else
            {
                allow.MessDed = Convert.ToDecimal(txtMessDed.Text.Trim());
            }

            if (txtOtherDed.Text.Trim().Equals(""))
            {
                allow.OtherDed = 0;
            }
            else
            {
                allow.OtherDed = Convert.ToDecimal(txtOtherDed.Text.Trim());
            }


            string msg = allowBL.Insert(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (msg == "ok")
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid(ID);
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(msg, RMS.BL.Enums.MessageType.Error);
            }

        }
        private void ClearFields()
        {
            //ID = 0;
            //CompID = 0;
            EffDateStr = "";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtEffDate.Text = "";
            txtBasicPay.Text = "";
            txtHouseRent.Text = "";
            txtFuelAll.Text = "";
            txtSplAll.Text = "";
            txtUtilities.Text = "";
            txtTaxDed.Text = "";
            txtMessDed.Text = "";
            txtOtherDed.Text = "";
            txtAllounce.Text = "";
            txtTotalPay.Text = "";
            grdEmps.SelectedIndex = -1;
            btnDelete.Visible = false;
        }

        #endregion
  

    }
}

using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;

namespace RMS.Setup
{
    public partial class EmpMgtSalary_x : BasePage
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
#pragma warning disable CS0114 // 'EmpMgtSalary_x.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'EmpMgtSalary_x.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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

        #endregion

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "EmpSalSetup").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                if (Session["DateFormat"] == null)
                {
                    txtEffDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                }
                else
                {
                    txtEffDateCal.Format = Session["DateFormat"].ToString();
                }

                BindGrid("", 0, 0);
                ucButtons.ValidationGroupName = "main";
                EmpSrchUC.Focus();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(txtFltEmp.Text.Trim(), Convert.ToInt32(ddlFltRegion.SelectedValue), Convert.ToInt32(ddlFltSegment.SelectedValue));
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

            BindGrid(txtFltEmp.Text.Trim(), Convert.ToInt32(ddlFltRegion.SelectedValue), Convert.ToInt32(ddlFltSegment.SelectedValue));
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
                if (ID == 0)
                {
                    this.Insert();
                    //pnlMain.Enabled = false;
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                    //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);

                }
                else
                {
                    this.Update();
                    //pnlMain.Enabled = false;
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                    //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);

                }
            }
            else if (e.CommandName == "Delete")
            {
                // TRANSACTION WALA KAAM KARNA HAI......

                try
                {
                    this.Delete(ID);
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
                BindGrid("", 0, 0);
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
                    e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString(Session["DateFormat"].ToString());
                }

            }
        }

        #endregion

        #region Helping Method
        protected void BindGrid(string empName, int RegId, int segId)
        {
            this.grdEmps.DataSource = allowBL.GetAll(null,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdEmps.DataBind();
        }

        protected void GetByID()
        {
            tblPlAlow empPojo = allowBL.GetByID(CompID, ID, DateTime.Parse(EffDateStr), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            //this.txtEmpCode.Text = empPojo.EmpID.ToString();
            //this.txtFullName.Text = empPojo.FirstName + ' ' + empPojo.MidName + ' ' + empPojo.SirName;

            EmpSrchUC.EditModeDataShow(empPojo.tblPlEmpData.FullName,"EN-" +empPojo.tblPlEmpData.EmpID,
                empPojo.tblPlEmpData.EmpCode,
                empPojo.tblPlEmpData.tblPlCode1 != null ? empPojo.tblPlEmpData.tblPlCode1.CodeDesc : "",
                empPojo.tblPlEmpData.tblPlCode != null ? empPojo.tblPlEmpData.tblPlCode.CodeDesc : "");

            txtBasicPay.Text = empPojo.Basic.ToString();
            if (Session["DateFormat"] == null)
            {
                txtEffDate.Text = empPojo.EffDate.ToString(Request.Cookies["uzr"]["DateFormat"]);

            }
            else
            {
                txtEffDate.Text = empPojo.EffDate.ToString(Session["DateFormat"].ToString());
            }
            
            txtHouseRent.Text = empPojo.HR.ToString();
            txtSplAll.Text = empPojo.SplAlow.ToString();
            txtUtilities.Text = empPojo.Utilities.ToString();
            txtFuelAll.Text = empPojo.CA.ToString();
            txtAllounce.Text = empPojo.NSHA.ToString();
            txtTaxDed.Text = empPojo.TaxDed.ToString();
            txtOtherDed.Text = empPojo.OtherDed.ToString();

            pnlMain.Enabled = true;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
        }

        protected void Update()
        {
            tblPlAlow allow = allowBL.GetByID(CompID, ID, DateTime.Parse(EffDateStr), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

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
                allow.FuelLimit = 0;
            }
            else
            {
                allow.FuelLimit = Convert.ToInt16(txtFuelAll.Text.Trim());
            }
            if (txtTaxDed.Text.Trim().Equals(""))
            {
                allow.TaxDed = 0;
            }
            else
            {
                allow.TaxDed = Convert.ToDecimal(txtTaxDed.Text.Trim());
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
            allowBL.Update(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
            BindGrid("", 0, 0);
            ClearFields();
            //}
            //else
            //{
            //    ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
            //    pnlMain.Enabled = true;
            //}
        }

        protected void Delete(int Id)
        {
            //allowBL.DeleteByID(
            //              Convert.ToInt32(Session["CompID"].ToString()),
            //              Convert.ToInt32(Id), Convert.ToDateTime(txtEffDate.Text),
            //              (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        }

        protected void Insert()
        {
            //RMS.BL.Employee emp = new RMS.BL.Employee();
            tblPlAlow allow = new tblPlAlow();

            if (Session["CompID"] == null)
            {
                allow.CompID = Convert.ToByte(Request.Cookies["uzr"]["CompID"]);
            }
            else
            {
                allow.CompID = Convert.ToByte(Session["CompID"].ToString());
            }
            allow.EmpID = EmpSrchUC.EmpIDUC;// Convert.ToInt32(txtEmpCode.Text.Trim());
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
                allow.CA = Convert.ToInt16(txtFuelAll.Text.Trim());
            }

            if (txtAllounce.Text.Trim().Equals(""))
            {
                allow.NSHA = 0;
            }
            else
            {
                allow.NSHA = Convert.ToInt16(txtAllounce.Text.Trim());
            }

            if (txtTaxDed.Text.Trim().Equals(""))
            {
                allow.TaxDed = 0;
            }
            else
            {
                allow.TaxDed = Convert.ToDecimal(txtTaxDed.Text.Trim());
            }

            if (txtOtherDed.Text.Trim().Equals(""))
            {
                allow.OtherDed = 0;
            }
            else
            {
                allow.OtherDed = Convert.ToDecimal(txtOtherDed.Text.Trim());
            }

            if (!allowBL.ISAlreadyExist(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                allowBL.Insert(allow, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid("", 0, 0);
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "empAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            ID = 0;
            CompID = 0;
            EffDateStr = "";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtEffDate.Text = "";
            txtBasicPay.Text = "";
            txtHouseRent.Text = "";
            txtFuelAll.Text = "";
            txtSplAll.Text = "";
            txtUtilities.Text = "";
            txtTaxDed.Text = "";
            txtOtherDed.Text = "";
            txtAllounce.Text = "";
            grdEmps.SelectedIndex = -1;
            EmpSrchUC.ClearFields();
            EmpSrchUC.EditModeDataHide();
            EmpSrchUC.Focus();
        }


        #endregion
    }
}

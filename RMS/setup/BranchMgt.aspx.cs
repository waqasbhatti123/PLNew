using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;

namespace RMS.Setup
{
    public partial class BranchMgt : BasePage
    {

        #region DataMembers
        //RMS.BL.tblAppBranch usr;

        BranchBL branchMgr = new BranchBL();

        ListItem selList = new ListItem();


        #endregion

        #region Properties
#pragma warning disable CS0114 // 'BranchMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'BranchMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        #endregion

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {

            //pnlMain.Enabled = false;
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Branch").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
                FillDropDownCompany();
                //FillDropDownBranch();
                ucButtons.ValidationGroupName = "main";
                txtBrName.Focus();
            }
        }

        
        protected void grdBranchs_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ClearFields();
            ID = Convert.ToInt32(grdBranchs.SelectedDataKey.Value);
            GetByID();

            txtBrName.Focus();
        }

        protected void grdBranchs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBranchs.PageIndex = e.NewPageIndex;
            BindGrid();
        }





        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {

                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                pnlMain.Enabled = true;
                txtBrName.Focus();
            }
            else if (e.CommandName == "Save")
            {
                if (ID == 0)
                {
                    Insert();
                    //pnlMain.Enabled = false;
                    ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                    //ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);

                }
                else
                {
                    Update();
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
                    Delete();
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
                BindGrid();
                ClearFields();

            }
            else if (e.CommandName == "Edit")
            {
                pnlMain.Enabled = true;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                txtBrName.Focus();
            }
            else if (e.CommandName == "Cancel")
            {
                //pnlMain.Enabled = false;
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                ClearFields();

            }
        }

        #endregion

        #region Helping Method
        protected void BindGrid()
        {
            this.grdBranchs.DataSource = branchMgr.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdBranchs.DataBind();
        }


        protected void GetByID()
        {
            Branch br = branchMgr.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            txtBrName.Text = br.br_nme;
            ddlCompany.SelectedValue = br.CompID.ToString();
            //if(br.br_idd != null && br.br_idd  > 0)
            //{
            //    ddlBranch.SelectedValue = br.br_idd.ToString();
            //}
            
            txtNtn.Text = br.br_ntn;
            txtSTx.Text = br.br_stx_no;
            txtTel.Text = br.br_tel;
            txtFaxNo.Text = br.br_fax;
            txtAdd.Text = br.br_address;
            txtSecTel.Text = br.br_contact;
            txtLoCode.Text = br.LoCode;
            if (br.DDt.ToString() == "")
            {
                DdlTehsil.SelectedValue = "";
            }
            else
            {
                DdlTehsil.Text = br.DDt.ToString();
            }
            

            rblStatus.SelectedValue = br.br_status == true ? "1" : "0";

            pnlMain.Enabled = true;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);

        }

        protected void Update()
        {
            Branch br = branchMgr.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            br.br_nme = txtBrName.Text.Trim();
            br.CompID = Convert.ToByte(ddlCompany.SelectedValue);
            br.br_ntn = txtNtn.Text.Trim();
            br.LoCode = txtLoCode.Text.Trim();
            br.br_stx_no = txtSTx.Text.Trim();
            br.br_tel = txtTel.Text.Trim();
            br.br_fax = txtFaxNo.Text.Trim();
            if (DdlTehsil.SelectedValue == "")
            {
                br.DDt = null;
            }
            else
            {
                br.DDt = Convert.ToInt32(DdlTehsil.SelectedValue);
            }
            br.br_address = txtAdd.Text.Trim();
            br.br_contact = txtSecTel.Text.Trim();
            br.br_status = rblStatus.SelectedValue.Equals("1") ? true : false;
            br.updateby =  Session["LoginID"].ToString();
            br.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (!branchMgr.ISAlreadyExist(br, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                branchMgr.Update(br, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                //FillDropDownBranch();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "branchAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        //protected void FillDropDownBranch()
        //{
        //    RMSDataContext Data = new RMSDataContext();

        //    ddlBranch.Items.Clear();

        //    ddlBranch.DataValueField = "br_id";
        //    ddlBranch.DataTextField = "br_nme";
        //    ddlBranch.DataSource = Data.Branches.Where(x => x.br_idd == null && x.br_status == true).ToList();
        //    ddlBranch.DataBind();
        //}

        protected void Delete()
        {
            branchMgr.DeleteByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        protected void Insert()
        {
            Branch br = new Branch();

            br.br_nme = txtBrName.Text.Trim();
            br.CompID = Convert.ToByte(ddlCompany.SelectedValue);
            //if (ddlBranch.SelectedValue != null && ddlBranch.SelectedValue != "")
            //{
            //    br.br_idd = Convert.ToInt32(ddlBranch.SelectedValue);
            //}
            
            br.br_ntn = txtNtn.Text.Trim();
            br.LoCode = txtLoCode.Text.Trim();
            br.br_stx_no = txtSTx.Text.Trim();
            br.br_tel = txtTel.Text.Trim();
            br.br_fax = txtFaxNo.Text.Trim();
            br.br_contact = txtSecTel.Text.Trim();
            br.br_address = txtAdd.Text.Trim();
            if (DdlTehsil.SelectedValue == "")
            {
                br.DDt = null;
            }
            else
            {
                br.DDt = Convert.ToInt32(DdlTehsil.SelectedValue);
            }
            br.br_status = rblStatus.SelectedValue.Equals("1") ? true : false;
            br.updateby = Session["LoginID"].ToString();
            br.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (!branchMgr.ISAlreadyExist(br, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                branchMgr.Insert(br, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                //FillDropDownBranch();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "branchAlreadyExist").ToString(), RMS.BL.Enums.MessageType.Error);
                pnlMain.Enabled = true;
            }
        }

        private void ClearFields()
        {
            ID = 0;
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            txtBrName.Text = "";
            //ddlCompany.SelectedValue = "0";
            txtNtn.Text = "";
            txtSTx.Text = "";
            txtTel.Text = "";
            txtLoCode.Text = "";
            txtFaxNo.Text = "";
            txtAdd.Text = "";
            txtSecTel.Text = "";
            rblStatus.SelectedValue = "1";
            grdBranchs.SelectedIndex = -1;
            txtBrName.Focus();
            DdlTehsil.SelectedValue = "";
        }

        private void FillDropDownCompany()
        {
            ddlCompany.DataTextField = "CompName";
            ddlCompany.DataValueField = "CompID";
            ddlCompany.DataSource = branchMgr.GetAllCompaniesCombo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCompany.DataBind();
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RMS.BL;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.Inv
{
    public partial class Departments : BasePage
    {
        #region DataMembers
        Depart_ment dept;
        DepartmentBL deptBL = new DepartmentBL();
        #endregion

        #region Properties
        public string DeptName
        {
            get { return ViewState["DeptName"].ToString(); }
            set { ViewState["DeptName"] = value; }

        }
        public bool isEdit
        {
            get { return (bool)ViewState["isEdit"]; }
            set { ViewState["isEdit"] = value; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Departments").ToString();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                this.BindGrid();
               // this.txtDeptID.Focus();
                isEdit = false;
                txtName.Focus();
            }

        }

        protected void grdcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DeptName = grdcc.SelectedDataKey.Value.ToString().Trim();
                GridViewRow grdRow = grdcc.SelectedRow;
                txtName.Text = grdRow.Cells[1].Text;
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                this.txtName.Focus();
                isEdit = true;
                
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }

        
        protected void grdcc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdcc.PageIndex = e.NewPageIndex;
            BindGrid();
        }


        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
               
                //this.txtDeptID.Focus();
                txtName.Focus();
            }
            else if (e.CommandName == "Save")
            {
                string DptName = txtName.Text;
                if (isEdit != true)
                {
                    if (!DptName.Equals(""))
                    {
                        this.Insert();
                    }
                }
                else if (isEdit == true) 
                {
                    if (!DptName.Equals(""))
                    {
                        Update(DptName);
                    }
                }
            }
            
            //else if (e.CommandName == "Delete")
            //{
            //    try
            //    {
            //        this.Delete(ID);
            //        pnlMain.Enabled = false;
            //        ucButtons.SetMode(RMS.BL.Enums.PageMode.None);
            //    }
            //    catch (SqlException ex)
            //    {
            //        if (ex.Number == 547)
            //        {
            //            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletionDependency").ToString(), RMS.BL.Enums.MessageType.Error);
            //            return;
            //        }
            //        else

            //            Session["errors"] = ex.Message;
            //        Response.Redirect("~/home/Error.aspx");

            //    }

            //    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "DeletedSuccessfully").ToString(), RMS.BL.Enums.MessageType.Info);
            //    BindGrid();
            //    ClearFields();

            //}
            else if (e.CommandName == "Edit")
            {
                ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                //this.txtDeptID.Focus();
                txtName.Focus();
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();

            }
        }




        #endregion

        #region Helping Method
        protected void BindGrid()
        {
            this.grdcc.DataSource = deptBL.getAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdcc.DataBind();
        }

        protected void GetByID()
        {
            dept = deptBL.getByName(DeptName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
           this.txtName.Text = dept.DeptNme.ToString();
           
        }


        protected void Insert()
        {
            RMS.BL.Depart_ment deptR = new RMS.BL.Depart_ment();
            string deptName= this.txtName.Text.Trim();
            deptR.DeptNme = deptName;
            if (!deptBL.IsAlreadyExist(deptName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                deptBL.insertRecord(deptR, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
            }
            else
            {
                ucMessage.ShowMessage("Record already exist", RMS.BL.Enums.MessageType.Error);
               
            }
        }

        protected void Update(string DptName)
        {
           
                isEdit = false;
                deptBL.update(DptName,DeptName, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                BindGrid();
                ClearFields();
                

        }


        private void ClearFields()
        {
            //this.txtDeptID.Text = "";
            this.txtName.Text = "";
            ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            grdcc.SelectedIndex = -1;
            isEdit = false;
            txtName.Focus();
            //this.txtDeptID.Enabled = true;
            //this.txtDeptID.Focus();

        }

        #endregion

    }
}

using System;
using RMS.BL;

namespace RMS.UserControl
{
    public partial class EmpSearchUC : System.Web.UI.UserControl
    {
        
        public int EmpIDUC
        {
            get { return (ViewState["EmpIDUC"] == null) ? 0 : Convert.ToInt32(ViewState["EmpIDUC"]); }
            set { ViewState["EmpIDUC"] = value; }
        }
        public string EmpBindGrid
        {
            get { return (ViewState["EmpBindGrid"] == null) ? "No" : Convert.ToString(ViewState["EmpBindGrid"]); }
            set { ViewState["EmpBindGrid"] = value; }
        }
        public int ResgId
        {
            get { return (ViewState["ResgId"] == null) ? 0 : Convert.ToInt32(ViewState["ResgId"]); }
            set { ViewState["ResgId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
           BindGrid();
        }

        private void BindGrid()
        {
            //if (!txtEmpSrch.Text.Trim().Equals(""))
            //{
                grdEmpSrchUC.Visible = true;
                divEmpInfo.Visible = false;
                //lblCUstomerName.Visible = false;
                //lblCustomer.Visible = false;

                //int orderID, stateID;
                //string plateCode, plateNo;
                //int.TryParse(txtOrder.Text, out orderID);
                //int.TryParse(ddlState.SelectedValue, out stateID);

                //plateCode = ddlPlateCode.SelectedValue.Equals("Qala") ? "0" : ddlPlateCode.SelectedValue;

                //plateNo = txtPlateNum.Text.Trim() == "" ? "0" : txtPlateNum.Text;

                if (ResgId == 0)
                {
                    this.grdEmpSrchUC.DataSource = new EmpBL().GetAllSearch(txtEmpSrch.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                else
                {
                    this.grdEmpSrchUC.DataSource = new EmpBL().GetAllSearchForResg(txtEmpSrch.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                //this.grdEmpSrchUC.DataSource = new EmpBL().GetAllSearch(txtEmpSrch.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                this.grdEmpSrchUC.DataBind();

                grdEmpSrchUC.SelectedIndex = -1;
            //}
            //else
            //{
            //    this.grdEmpSrchUC.DataSource = null;
            //    this.grdEmpSrchUC.DataBind();
            //}
            //TextBox txt = (TextBox)this.Page.Page.FindControl("txtTest");

            ////this.Page.Page.FindControl("txtTest");
            //Response.Write(txt.Text);

        }
        protected void grdEmpSrchUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                EmpIDUC = Convert.ToInt32(grdEmpSrchUC.SelectedDataKey.Value);
                txtEmpSrch.Text = grdEmpSrchUC.SelectedRow.Cells[1].Text;
                //UCvehicleID = Convert.ToInt32(grdSearch.SelectedRow.Cells[0].Text);
                lblEmpName.Text = "<b>Name:</b> " + grdEmpSrchUC.SelectedRow.Cells[2].Text; //grdEmpSrchUC.SelectedRow.Cells[0].Text;
                lblEmpId.Text = "<b>Emp ID: </b>" + grdEmpSrchUC.SelectedRow.Cells[0].Text;
                lblEmpCode.Text = "<b>Emp Ref: </b>" + grdEmpSrchUC.SelectedRow.Cells[1].Text;
                lblEmpDesig.Text = "<b>Designation: </b>" + grdEmpSrchUC.SelectedRow.Cells[3].Text;
                lblEmpDept.Text = "<b>Deptartment: </b>" + grdEmpSrchUC.SelectedRow.Cells[4].Text;
                EmpBindGrid = "Yes";
                divEmpInfo.Visible = true;
                //lblCustomer.Visible = true;
                //txtVehIDAcci.Value = UCvehicleID.ToString();
                grdEmpSrchUC.Visible = false;
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }
        public void ClearFields()
        {
            txtEmpSrch.Text = "";
            EmpIDUC = 0;
            EmpBindGrid = "No";
            divEmpInfo.Visible = false;
        }

        public void EditModeDataShow(string EmpName, string emp_Id, string EmpCode, string EmpDesig, string EmpDept)
        {
            lblEmpName.Text = "<b>Name: </b>" + EmpName;
            lblEmpId.Text = "<b>Emp ID: </b>" + emp_Id;
            lblEmpCode.Text = "<b>Emp Ref: </b>" + EmpCode;
            lblEmpDesig.Text = "<b>Designation: </b>" + EmpDesig;
            lblEmpDept.Text = "<b>Department: </b>" + EmpDept;

            divEmpInfo.Visible = true;
            lblEmpSrch.Visible = false;
            txtEmpSrch.Visible = false;
            btnSearch.Visible = false;
            grdEmpSrchUC.Visible = false;
        }
        public void EditModeDataHide()
        {
            divEmpInfo.Visible = false;
            lblEmpSrch.Visible = true;
            txtEmpSrch.Visible = true;
            btnSearch.Visible = true;
        }
        public void SetTitle(string title)
        {
            txtEmpSrch.Text = title;
        }
        public string GetTitle()
        {
            return txtEmpSrch.Text;
        }

        public void SetBindGrid(string str)
        {
            EmpBindGrid = str;
        }

        public void HideEmpInfo()
        {
            divEmpInfo.Visible = false;
        }
        public void HideAll()
        {
            divEmpInfo.Visible = false;
            lblEmpSrch.Visible = false;
            txtEmpSrch.Visible = false;
            btnSearch.Visible = false;
        }
        public void ShowAll()
        {
            divEmpInfo.Visible = true;
            lblEmpSrch.Visible = true;
            txtEmpSrch.Visible = true;
            btnSearch.Visible = true;
        }
        //protected void grdEmpSrchUC_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Cells[5].Text = DateTime.Parse(e.Row.Cells[5].Text).ToString(Session["DateTimeFormat"].ToString());
        //    }
        //}

    }
}
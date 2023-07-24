using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.Linq;
namespace RMS
{
    public partial class PrivilageMgt : BasePage
    {
        #region DataMembers
       
        //tblAppPrivilage privlage;
        PrivilageBL privilageManager = new PrivilageBL();
       
        #endregion

        #region Properties
       
#pragma warning disable CS0114 // 'PrivilageMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'PrivilageMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }

        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GroupPer").ToString();
                BindGroups();
                this.BindGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            tblAppPrivilage privilage;
            GroupBL groupbl = new GroupBL();
            RMS.BL.tblAppGroup group = groupbl.GetByID(Convert.ToInt32(ddlGroup.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            EntitySet<tblAppPrivilage> privilages = new EntitySet<tblAppPrivilage>();


            for (int rowcount = 0; rowcount < grdGroups.Rows.Count; rowcount++)
            {
                string modid = this.grdGroups.DataKeys[rowcount].Values[2].ToString();

                string amparent = null;
                try
                {
                    amparent = this.grdGroups.DataKeys[rowcount].Values[1].ToString();
                }
                catch { }
                
                
               
                
                privilage = new tblAppPrivilage();
                privilage.AmID = Convert.ToInt32(grdGroups.DataKeys[rowcount].Value);
                privilage.Enabled = ((CheckBox)grdGroups.Rows[rowcount].FindControl("chkEnabled")).Checked;



                if (modid == "3" && amparent == "320")
                {
                    privilage.CanAdd = ((CheckBox)grdGroups.Rows[rowcount].FindControl("chkAdd")).Checked;
                    privilage.CanEdit = ((CheckBox)grdGroups.Rows[rowcount].FindControl("chkEdit")).Checked;
                }
                else if (modid == "2" && grdGroups.DataKeys[rowcount].Value.ToString() == "34")
                {
                    privilage.CanAdd = ((CheckBox)grdGroups.Rows[rowcount].FindControl("chkAdd")).Checked;
                    privilage.CanEdit = ((CheckBox)grdGroups.Rows[rowcount].FindControl("chkEdit")).Checked;
                }
                else if (modid == "2" && grdGroups.DataKeys[rowcount].Value.ToString() == "36")
                {
                    privilage.CanAdd = ((CheckBox)grdGroups.Rows[rowcount].FindControl("chkAdd")).Checked;
                    privilage.CanEdit = ((CheckBox)grdGroups.Rows[rowcount].FindControl("chkEdit")).Checked;
                }
                else
                {
                    privilage.CanAdd = true;
                    privilage.CanEdit = true;
                }

                
                privilage.CanDel = true; //((CheckBox)grdGroups.Rows[rowcount].FindControl("chkDelete")).Checked;
                privilage.CanPrint = true; //((CheckBox)grdGroups.Rows[rowcount].FindControl("chkPrint")).Checked;

                privilages.Add(privilage);
            }
            groupbl.DeleteAllPrivilages(group.GroupID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            group.tblAppPrivilages = privilages;
            groupbl.Update((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
        }
        
        #endregion

        #region Helping Method

        protected void BindGrid()
        {

            this.grdGroups.DataSource = privilageManager.GetPrivilagesByGroup(Convert.ToInt32(ddlGroup.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdGroups.DataBind();
            try
            {
                for (int i = 0; i < grdGroups.Rows.Count; i++)
                {
                    try
                    {
                        Convert.ToInt32(grdGroups.DataKeys[i].Values[1].ToString());
                        grdGroups.Rows[i].Cells[0].Text = "&nbsp;&nbsp;&nbsp;" + grdGroups.Rows[i].Cells[0].Text;
                    }
                    catch { }
                }
            }
            catch { }
        }

        private void BindGroups()
        {
            RMS.BL.GroupBL group = new GroupBL();
            ddlGroup.DataTextField = "GroupName";
            ddlGroup.DataValueField = "GroupID";
            ddlGroup.DataSource = group.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlGroup.DataBind();

        }

        #endregion

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void grdGroups_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = grdGroups.HeaderRow;
            // Access the CheckBox
            CheckBox cb = (CheckBox)row.FindControl("chkEnabledAll");
            if (cb != null)
                cb.Checked = chkSelectAll.Checked;

            //CheckBox cbAdd = (CheckBox)row.FindControl("chkAdd");
            //if (cbAdd != null)
            //  cbAdd.Checked = chkSelectAll.Checked;

            //CheckBox cbEd = (CheckBox)row.FindControl("chkEdit");
            //if (cbEd != null)
            //  cbEd.Checked = chkSelectAll.Checked;

            //CheckBox cbDel = (CheckBox)row.FindControl("chkDelete");
            //if (cbDel != null)
            //  cbDel.Checked = chkSelectAll.Checked;


            //CheckBox cbPt = (CheckBox)row.FindControl("chkPrint");
            //if (cbPt != null)
            //  cbPt.Checked = chkSelectAll.Checked;


        }

        protected void grdGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

                //Find the checkbox control in header and add an attribute
                ((CheckBox)e.Row.FindControl("chkEnabledAll")).Attributes.Add("onclick", "javascript:SelectCol('" +
                        ((CheckBox)e.Row.FindControl("chkEnabledAll")).ClientID + "',1)");
                ((CheckBox)e.Row.FindControl("chkAddAll")).Attributes.Add("onclick", "javascript:SelectCol('" +
                        ((CheckBox)e.Row.FindControl("chkAddAll")).ClientID + "',2)");
                ((CheckBox)e.Row.FindControl("chkEditAll")).Attributes.Add("onclick", "javascript:SelectCol('" +
                        ((CheckBox)e.Row.FindControl("chkEditAll")).ClientID + "',3)");

            }
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string amid = this.grdGroups.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string modid = this.grdGroups.DataKeys[e.Row.RowIndex].Values[2].ToString();

                string amparent = null;
                try 
                { 
                    amparent = this.grdGroups.DataKeys[e.Row.RowIndex].Values[1].ToString();
                }    
                catch { }


                //Give option for 'Edit/Approve, Add' GL Vouchers
                if (modid == "3" && amparent == "320")
                {
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else //Edit/Approve option for leavemgt
                if (modid == "2" && amparent == "3" && amid == "34")
                {
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (modid == "2" && amparent == "3" && amid == "36")
                {
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }

                

                if (amid.Equals("1"))
                {
                    e.Row.Cells[0].Text = "<table width='100%'><tr class='text-info'><td>Admin Module<td></tr></table><b>Admin</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("2"))
                {
                    e.Row.Cells[0].Text = "<table width='100%'><tr class='text-info'><td>Payroll Module<td></tr></table><b>Setup</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;

                    //CheckBox cb = ((CheckBox)e.Row.FindControl("chkEdit"));
                    //cb.Checked = false;
                }
                else if (amid.Equals("3"))
                {
                    e.Row.Cells[0].Text = "<b>Employee Data</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("4"))
                {
                    e.Row.Cells[0].Text = "<b>Processing</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("5"))
                {
                    e.Row.Cells[0].Text = "<b>Reports</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("6"))
                {
                    e.Row.Cells[0].Text = "<b>Reports Attendance</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("7"))
                {
                    e.Row.Cells[0].Text = "<b>Reports Salary</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("8"))
                {
                    e.Row.Cells[0].Text = "<b>Reports Misc</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("310"))
                {
                    e.Row.Cells[0].Text = "<table width='100%'><tr class='text-info'><td>GL Module<td></tr></table><b>Setup</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("320"))
                {
                    e.Row.Cells[0].Text = "<b>Vouchers</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("330"))
                {
                    e.Row.Cells[0].Text = "<b>Reports</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("410"))
                {
                    e.Row.Cells[0].Text = "<table width='100%'><tr class='text-info'><td>Stock Module<td></tr></table><b>Setup</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("420"))
                {
                    e.Row.Cells[0].Text = "<b>Transactions</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("450"))
                {
                    e.Row.Cells[0].Text = "<b>Reports</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("501"))
                {
                    e.Row.Cells[0].Text = "<table width='100%'><tr class='text-info'><td>Inventory Module<td></tr></table><b>Setup</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("510"))
                {
                    e.Row.Cells[0].Text = "<b>Transactions</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
                else if (amid.Equals("520"))
                {
                    e.Row.Cells[0].Text = "<b>Reports</b>";
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEnabled")).Checked = true;
                    ((CheckBox)e.Row.FindControl("chkAdd")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("chkPrint")).Visible = false;
                }
            }
        }

        /*
         protected void chkEnabledHD_CheckedChanged(object sender, EventArgs e)
         {
           foreach (GridViewRow row in grdGroups.Rows)
           {
             CheckBox cb = (CheckBox)row.FindControl("chkEnabled");
             if (cb != null)
               cb.Checked = ((CheckBox)sender).Checked;
           }
         }

         protected void chkAddHD_CheckedChanged(object sender, EventArgs e)
         {
           foreach (GridViewRow row in grdGroups.Rows)
           {
             CheckBox cb = (CheckBox)row.FindControl("chkAdd");
             if (cb != null)
               cb.Checked = ((CheckBox)sender).Checked;
           }

         }

         protected void chkEditHD_CheckedChanged(object sender, EventArgs e)
         {
           foreach (GridViewRow row in grdGroups.Rows)
           {
             CheckBox cb = (CheckBox)row.FindControl("chkEdit");
             if (cb != null)
               cb.Checked = ((CheckBox)sender).Checked;
           }

         }

         protected void chkDeleteHD_CheckedChanged(object sender, EventArgs e)
         {
           foreach (GridViewRow row in grdGroups.Rows)
           {
             CheckBox cb = (CheckBox)row.FindControl("chkDelete");
             if (cb != null)
               cb.Checked = ((CheckBox)sender).Checked;
           }

         }

         protected void chkPrintHD_CheckedChanged(object sender, EventArgs e)
         {
           foreach (GridViewRow row in grdGroups.Rows)
           {
             CheckBox cb = (CheckBox)row.FindControl("chkPrint");
             if (cb != null)
               cb.Checked = ((CheckBox)sender).Checked;
           }

         }
       */
    }
}

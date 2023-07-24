using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.Linq;
using System.Data;

namespace RMS.GL.Setup
{
    public partial class frmThicknessSetup : BasePage
    {
        #region DataMembers
        ThicknessSetupBL ds = new ThicknessSetupBL();
     
        #endregion

        #region Properties

       public DataTable tblAll
       {
           set { ViewState["tblAll"] = value; }
           get { return (DataTable)ViewState["tblAll"]; }
       }

       public bool flag
       {
           set { ViewState["flag"] = value; }
           get { return (bool)ViewState["flag"]; }
       }

       public int thickCode
       {
           set { ViewState["thickCode"] = value; }

           get { return Convert.ToInt32(ViewState["thickCode"]); }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ThicknessSetup").ToString();
                //ucButtons.SetMode(RMS.BL.Enums.PageMode.New);

                flag = true;
                getAll();
                BindGrid();
                

            }

        }


        protected void grdViewItemD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[3].Text == "A")
                {
                    e.Row.Cells[3].Text = "Enable";

                }
                else
                {
                    e.Row.Cells[3].Text = "Disable";
                }

            }
        
        
        }

        protected void grdViewItemD_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow grow = grdViewItemD.SelectedRow;
            txtThicknessCode.Text = grow.Cells[0].Text;
            txtThickness.Text = grow.Cells[1].Text;
            txtDescription.Text = grow.Cells[2].Text;
           
            if (grow.Cells[3].Text == "Disable")
            {
                ddlStatus.SelectedValue = "C";
            }
            else ddlStatus.SelectedValue = "A";

            thickCode = Convert.ToInt32(grow.Cells[0].Text);
            flag = false;
        }



        //protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
        //{





        //}






        protected void btnSave_click(object sender, EventArgs e)
        {
            if (flag == true)
            {
                save();
            }
            else if (flag == false)
            {
                edit();
            }
        }

        protected void btnClear_click(object sender, EventArgs e)
        {
            clearFields();
            flag = true;
        }

        //protected void ButtonCommand(object sender, CommandEventArgs e)
        //{
        //    if (e.CommandName == "New")
        //    {
        //       // ClearFields();
        //        ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
               
               
        //    }
        //    else if (e.CommandName == "Save")
        //    {
               
        //    }
        //    else if (e.CommandName == "Edit")
        //    {
               
        //        ucButtons.SetMode(RMS.BL.Enums.PageMode.Edit);
                

        //    }
        //    else if (e.CommandName == "Cancel")
        //    {
               
        //    }
        //}


        #endregion

        #region Helping Method

        public void getAll()
        {
            List<tblItemThick> rcd= ds.getAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DataTable dtbl = new DataTable();

            dtbl.Columns.Add("ThicknessCode", typeof(int));
            dtbl.Columns.Add("Thickness", typeof(decimal));
            dtbl.Columns.Add("Description", typeof(string));
            dtbl.Columns.Add("Status", typeof(char));

            DataRow dr;

            foreach (var record in rcd)
            {
                dr = dtbl.NewRow();
                dr["ThicknessCode"] = record.ThickId;
                dr["Thickness"] = record.Thickness;
                dr["Description"] = record.Thick_Desc;
                dr["Status"] = record.Status;
                dtbl.Rows.Add(dr);
            }

            tblAll = dtbl;
        }



        public void BindGrid()
        {
            grdViewItemD.DataSource = tblAll;
            grdViewItemD.DataBind();

        }

        public void edit()
        {
           
            if(ds.delete( thickCode,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            {
                flag = true;
                save();
               
            }

        }

        public void save()
        {
            int br;
            short cd = Convert.ToInt16(txtThicknessCode.Text);
            string desc = txtDescription.Text;
            decimal thick = decimal.Parse(txtThickness.Text);
            char st = char.Parse(ddlStatus.SelectedValue);

            if (txtThicknessCode.Text != "" && desc != "")
            {

                    if (ds.exist(cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) != true)
                    {
                        if (Session["BranchID"] == null)
                            br = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            br = Convert.ToInt32(Session["BranchID"]);
                        if (ds.save(br,cd,thick, desc, st, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                        {
                            getAll();
                            BindGrid();
                            clearFields();
                        }
                    }
                    else
                    {
                        ucMessage.ShowMessage("Design Code already exist...", RMS.BL.Enums.MessageType.Error);
                        clearFields();
                    }
                
            }
        }


        public void BindDdlUOM()
        {
           // ds.GetUOM((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }


        public void clearFields()
        {
            txtThicknessCode.Text = "";
            txtDescription.Text = "";
            txtThickness.Text = "";

        }

       
        
        #endregion
    }
}

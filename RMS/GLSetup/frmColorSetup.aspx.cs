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
    public partial class frmColorSetup : BasePage
    {
        #region DataMembers
        DesignColorBL dc = new DesignColorBL();
        

        public string colorid
        {
            set { ViewState["color"] = value; }
            get { return Convert.ToString(ViewState["color"]); }
        }

        public string Descr
        {
            set { ViewState["Descr"]=value;}
            get{ return Convert.ToString(ViewState["Descr"]);}
        }

        public bool flag
        {
            set { ViewState["flag"] = value; }
            get{ return Convert.ToBoolean(ViewState["flag"]);}
        }


        #endregion

        #region Properties

       public DataTable tblAll
       {
           get{ return (DataTable)ViewState["tblAll"];}
           set{ViewState["tblAll"]=value;}
       }


        #endregion


        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            int BrId = 0;
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"]);
            }
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ColorSetup").ToString();
               // ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
                BindGrid();
                flag = true;
              
            }

        }



        protected void grdViewColor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) 
            {
                if (e.Row.Cells[2].Text == "A")
                {
                    e.Row.Cells[2].Text = "Enable";
                }
                else
                    e.Row.Cells[2].Text = "Disable";
            }
        }

        protected void grdViewColor_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdViewColor.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdViewColor_SelectedIndexChanged(object sender, EventArgs e )
        {
            GridViewRow grRow= grdViewColor.SelectedRow;
            txtColorCode.Text = grRow.Cells[0].Text;
            txtDescription.Text = grRow.Cells[1].Text;
            if (grRow.Cells[2].Text == "Disable")
            {
                ddlStatus.SelectedValue = "C";
            }
            else ddlStatus.SelectedValue = "A";

            colorid = grRow.Cells[0].Text;
            Descr = grRow.Cells[1].Text;
            flag = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (flag == false)
            {

                edit();

            }
            else if (flag == true)
            {
                save();

            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearFields();
            flag = true;


        }

        //protected void ButtonCommand(object sender, CommandEventArgs e)
        //{
        //    if (e.CommandName == "New")
        //    {
        //        //clearFields();
        //        ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
               
               
        //    }
        //    else if (e.CommandName == "Save")
        //    {
        //        //--------------------

               
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

        #region Helping Method:

        public void getData()
        {

            List<tblItemColor> rcd = dc.getAll((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            DataTable dtable = new DataTable();
            dtable.Columns.Add("ColorCode", typeof(String));
            dtable.Columns.Add("Description", typeof(String));
            dtable.Columns.Add("Status", typeof(Char));

            DataRow dr;

            foreach (var record in rcd)
            {
                dr = dtable.NewRow();
                dr["ColorCode"] = record.ColorId;
                dr["Description"] = record.Color;
                dr["Status"] = record.Status;
                dtable.Rows.Add(dr);

            }
            tblAll = dtable;

        }

        public void BindGrid()
        {
            getData();
            grdViewColor.DataSource = tblAll;
            grdViewColor.DataBind();
        }


        public void clearFields()
        {
            txtColorCode.Text = "";
            txtDescription.Text = "";
            
        }

        public void save()
        {
            int br;
            string cid = txtColorCode.Text;
            string desc = txtDescription.Text;
            char st = Convert.ToChar(ddlStatus.SelectedValue);
           // tblItemColor tic = new tblItemColor();
            if (cid != "" && desc != "")
            {
                //tic.br_id = 1;
                //tic.ColorId = cid;
                //tic.Color = desc;
                //tic.Status = st;
                if (flag == true)
                {
                    if (dc.exist(cid,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])==false)
                    {
                        if (Session["BranchID"] == null)
                            br = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            br = Convert.ToInt32(Session["BranchID"]);
                        if (dc.save(br,cid, desc, st, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                        {
                            //Response.Write("Record has been saved");
                            getData();
                            BindGrid();
                            clearFields();
                        }
                    }
                    else
                    {
                        ucMessage.ShowMessage("Color Code is already exist.", RMS.BL.Enums.MessageType.Error);
                        clearFields();
                    } 

                }

            }

        }
                    public void edit()
                    {

                    if (dc.delete(colorid, Descr, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        flag = true;
                        save();
                    }

                    }
        #endregion
    }
}

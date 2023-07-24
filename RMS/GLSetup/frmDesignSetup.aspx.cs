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
    public partial class frmDesignSetup : BasePage
    {
       
        #region DataMembers

        DesignSetupBL desBl = new DesignSetupBL();
        
        #endregion

        
        #region Properties

        public string designCode
        {
            set { ViewState["designCode"] = value; }
            get { return Convert.ToString(ViewState["designCode"]); }
        }   
        
        public bool IsEdit
        {
            set { ViewState["IsEdit"] = value; }
            get { return (bool)ViewState["IsEdit"]; }
        }

        public bool saved
        {
            set { ViewState["saved"] = value; }
            get { return (bool)ViewState["saved"]; }
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
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "DesignSetup").ToString();
                FillDdlColor();
                FillDdlThick();
                BindGridDesign();
                IsEdit = false;
            }
        }


        protected void grdViewItemD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[7].Text == "A")
                {
                    e.Row.Cells[7].Text = "Enable";
                }
                else
                {
                    e.Row.Cells[7].Text = "Disable";
                }
            }
        }


        protected void grdViewItemD_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDesignById(grdViewItemD.SelectedRow.Cells[0].Text, Convert.ToInt32(grdViewItemD.SelectedRow.Cells[2].Text), grdViewItemD.SelectedRow.Cells[5].Text);
            IsEdit = true;
            txtDesignCode.Enabled = false;
            txtDescription.Enabled = false;
        }


        protected void btnSave_click(object sender, EventArgs e)
        {
            if (IsEdit == false)
            {
                if (!CheckIfDesignCodeExists() && !CheckIfDesignDescExists())
                {
                    saved = SaveDesign();
                    if (saved == true)
                    {
                        ClearFields();
                        BindGridDesign();
                        ucMessage.ShowMessage("Saved successfully.", RMS.BL.Enums.MessageType.Info);
                    }
                    else
                    {
                        ucMessage.ShowMessage("Cannot save at this time, internal error occured.", RMS.BL.Enums.MessageType.Error);
                    }
                    saved = false;
                }
            }
            else
            {
                IsEdit = false;
                saved = EditDesign();
                if (saved == true)
                {
                    ClearFields();
                    BindGridDesign();
                    ucMessage.ShowMessage("Edited successfully.", RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage("Cannot edit at this time, internal error occured.", RMS.BL.Enums.MessageType.Error);
                }
                saved = false;
            }
        }


        protected void grdViewItemD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdViewItemD.PageIndex = e.NewPageIndex;
            BindGridDesign();
        }


        protected void btnClear_click(object sender, EventArgs e)
        {
            Response.Redirect("~/GLSetup/frmDesignSetup.aspx?PID=413");
        }


        #endregion


        #region Helping Method


        public bool SaveDesign()
        {
            try
            {
                tblItemDesign desgin = new tblItemDesign();

                //Fill Design---------------------------------

                if (Session["BranchID"] == null)
                {
                    desgin.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    desgin.br_id = Convert.ToInt32(Session["BranchID"]);
                }
                desgin.DesignId = txtDesignCode.Text;
                desgin.ThickId = Convert.ToInt16(ddlThick.SelectedValue);
                desgin.ColorId = ddlColor.SelectedValue;
                desgin.Design_Desc = txtDescription.Text;
                desgin.Status = Convert.ToString(ddlStatus.SelectedValue);

                //Save Design---------------------------------

                return desBl.SaveDesign(desgin, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
                return false;
            }
        }


        public bool EditDesign()
        {
            try
            {
                tblItemDesign des = desBl.GetDesByDesc(txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                //Fill Design---------------------------------

                des.ThickId = Convert.ToInt16(ddlThick.SelectedValue);
                des.ColorId = ddlColor.SelectedValue;
                des.Status = Convert.ToString(ddlStatus.SelectedValue);


                //Edit Design---------------------------------

                return desBl.EditDesign(des, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
                return false;
            }
        }


        public bool CheckIfDesignDescExists()
        {
            tblItemDesign des = desBl.GetDesByDesc(txtDescription.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (des != null)
            {
                ucMessage.ShowMessage("Design description already exists.", RMS.BL.Enums.MessageType.Error);
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool CheckIfDesignCodeExists()
        {
            tblItemDesign des = desBl.GetDesById(txtDesignCode.Text, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (des != null)
            {
                ucMessage.ShowMessage("Design code already exists.", RMS.BL.Enums.MessageType.Error);
                return true;
            }
            else
            {
                return false;
            }
        }


        public void BindGridDesign()
        {
            grdViewItemD.DataSource = desBl.GetGridObject((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdViewItemD.DataBind();
        }


        public void GetDesignById(string desId, int thkId, string colId)
        {
            try
            {
                tblItemDesign des = desBl.GetDesById(desId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                if (des != null)
                {

                    txtDesignCode.Text = des.DesignId;
                    txtDescription.Text = des.Design_Desc;
                    ddlStatus.SelectedValue = Convert.ToString(des.Status);
                    ddlThick.SelectedValue = Convert.ToString(des.ThickId);
                    ddlColor.SelectedValue = des.ColorId;

                }
                else
                {
                    ucMessage.ShowMessage("Cannot select at this time, internal error occured.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exctption: "+ ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }


        public void FillDdlColor()
        {
            ddlColor.DataSource = desBl.GetColor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlColor.DataTextField = "Color";
            ddlColor.DataValueField = "ColorId";
            ddlColor.DataBind();
        }


        public void FillDdlThick()
        {
            ddlThick.DataSource = desBl.GetThickness((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlThick.DataTextField = "Thick_Desc";
            ddlThick.DataValueField = "ThickId";
            ddlThick.DataBind();
        }


        public void ClearFields()
        {
            txtDesignCode.Text = "";
            txtDescription.Text = "";
            ddlStatus.SelectedValue = "A";
            ddlThick.SelectedValue = "0";
            ddlColor.SelectedValue = "0";
        }


        #endregion

    }
}

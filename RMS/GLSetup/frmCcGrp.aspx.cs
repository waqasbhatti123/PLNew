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

namespace RMS.GLSetup
{
    public partial class frmCcGrp : BasePage
    {
        #region DataMembers

        GLReportParameterBL glRptParam = new GLReportParameterBL();

        #endregion

        #region Properties

        public int CompID
        {
            get { return Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }
        public int CCGrp
        {
            get { return Convert.ToInt32(ViewState["CCGrp"]); }
            set { ViewState["CCGrp"] = value; }
        }
        public int CCSeq
        {
            get { return Convert.ToInt32(ViewState["CCSeq"]); }
            set { ViewState["CCSeq"] = value; }
        }
        public bool IsEdit
        {
            get { return Convert.ToBoolean(ViewState["IsEdit"]); }
            set { ViewState["IsEdit"] = value; }
        }
        public string Description
        {
            get{return ViewState["Description"].ToString();}
            set{ViewState["Description"]=value;}
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["CompID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    CompID = Convert.ToInt32(Request.Cookies["uzr"]["CompID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                CompID = Convert.ToInt32(Session["CompID"]);
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "CostCenterGroup").ToString();
                BindGrid();
            }
        }
        protected void grdcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearFields();

                CompID = Convert.ToInt32(grdcode.SelectedDataKey["CompId"]);
                CCGrp = Convert.ToInt32(grdcode.SelectedDataKey["CCGrp"]);
                CCSeq = Convert.ToInt32(grdcode.SelectedDataKey["CCSeq"]);

                tblglCCGrp ccGrp = glRptParam.GetCCGrpById(CompID, CCGrp, CCSeq, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (ccGrp != null)
                {
                    IsEdit = true;
                    txtGrp.Text = ccGrp.CCGrp.ToString();
                    txtGrpDesc.Text = ccGrp.GrpDesc;
                    txtCCFrom.Text = string.IsNullOrEmpty(ccGrp.CC_From) ? "" : ccGrp.CC_From;
                    txtCCTo.Text = string.IsNullOrEmpty(ccGrp.CC_To) ? "" : ccGrp.CC_To;

                    txtGrp.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        protected void grdcode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdcode.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void grdcode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsEdit)
            {
                Save();
            }
            else
            {
                Edit();
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GLSetup/frmCcGrp.aspx?PID=612");
        }
 
        #endregion

        #region Method

        public void BindGrid()
        {
            this.grdcode.DataSource = glRptParam.GetAllCCGrps((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdcode.DataBind();
        }
        private void ClearFields()
        {
            CCGrp = 0;
            CCSeq = 0;
            IsEdit = false;
            txtGrp.Enabled = true;
            txtGrp.Text = "";
            txtGrpDesc.Text = "";
            txtCCFrom.Text = "";
            txtCCTo.Text = "";
        }
        private Int16 GetMaxSeq(int compId, int ccgrp)
        {
            return glRptParam.GetMaxSeq(compId, ccgrp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        private void Save()
        {
            try
            {
                tblglCCGrp ccGrp = new tblglCCGrp();
                ccGrp.CompId = Convert.ToByte(CompID);
                ccGrp.CCGrp = Convert.ToInt16(txtGrp.Text.Trim());
                ccGrp.CCSeq = GetMaxSeq(CompID, Convert.ToInt32(txtGrp.Text.Trim()));
                ccGrp.GrpDesc = txtGrpDesc.Text;
                ccGrp.CC_From = txtCCFrom.Text;
                ccGrp.CC_To = txtCCTo.Text;
                if (Session["UserID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        ccGrp.Enteredby = Convert.ToString(Request.Cookies["uzr"]["UserID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    ccGrp.Enteredby = Convert.ToString(Session["UserID"]);
                }
                ccGrp.Enteredon = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                string msg = glRptParam.SaveCCGrp(ccGrp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg == "ok")
                {
                    ClearFields();
                    BindGrid();
                    ucMessage.ShowMessage("Saved successfully", BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage("Exception: " + msg, BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        private void Edit()
        {
            try
            {
                tblglCCGrp ccGrp = glRptParam.GetCCGrpById(CompID, CCGrp, CCSeq, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                
                ccGrp.GrpDesc = txtGrpDesc.Text;
                ccGrp.CC_From = txtCCFrom.Text;
                ccGrp.CC_To = txtCCTo.Text;
                if (Session["UserID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        ccGrp.updateby = Convert.ToString(Request.Cookies["uzr"]["UserID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    ccGrp.updateby = Convert.ToString(Session["UserID"]);
                }
                ccGrp.updateon = RMS.BL.Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                string msg = glRptParam.UpdateCCGrp(ccGrp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg == "ok")
                {
                    ClearFields();
                    BindGrid();
                    ucMessage.ShowMessage("Updated successfully", BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage("Exception: " + msg, BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion
    }
}

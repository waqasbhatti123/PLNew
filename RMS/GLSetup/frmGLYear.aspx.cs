using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Services;
using System.Collections.Generic;
using System.Web;

namespace RMS.GLSetup
{
    public partial class frmGLYear : BasePage
    {
        #region DataMembers

        GLYearBL glYrBL = new GLYearBL();

        #endregion

        #region Properties
       
        public int UserID
        {
            get { return  Convert.ToInt32(ViewState["UserID"]); }
            set { ViewState["UserID"] = value; }
        }
        public int BrId
        {
            get { return Convert.ToInt32(ViewState["BrId"]); }
            set { ViewState["BrId"] = value; }
        }
        public decimal GLYear
        {
            get { return Convert.ToDecimal(ViewState["GLYear"]); }
            set { ViewState["GLYear"] = value; }
        }

        #endregion

        #region Events
        
        protected void Page_Load(object sender, EventArgs e)
        {
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
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    UserID = Convert.ToInt32(Request.Cookies["uzr"]["UserID"]);
                }
            }
            else
            {
                UserID = Convert.ToInt32(Session["UserID"]);
            }

            GLYear = glYrBL.GetCurrentGLYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            lblGLYear.Text = "(Current GL Year: " + GLYear.ToString() + ")";

            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GLYear").ToString();
            if (!IsPostBack)
            {
                
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            string msg = glYrBL.UpdateGLYear(BrId, ddlGLYrProcess.SelectedValue, GLYear, UserID.ToString(), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (msg.Equals("ok"))
            {
                if (ddlGLYrProcess.SelectedValue == "0")
                {
                    GLYear++;
                }
                else if (ddlGLYrProcess.SelectedValue == "1")
                {
                    GLYear--;
                }
                else { }
                lblGLYear.Text = "(Current GL Year: " + GLYear.ToString() + ")";
                ucMessage.ShowMessage("GL Year updated successfully", RMS.BL.Enums.MessageType.Info);
            }
            else
            {
                ucMessage.ShowMessage(msg, RMS.BL.Enums.MessageType.Error);
            }

        }

        #endregion

        #region Helping Method
       

        #endregion
    }
}

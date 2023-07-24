using System;
using System.Web.UI.WebControls;
using RMS.BL.Enums;
using RMS.BL;
namespace RMS.UserControl
{
    public partial class Buttons : System.Web.UI.UserControl
    {
        public event EventHandler<CommandEventArgs> ButtonClick;

        private int PID
        {
            get
            {
                if (Request.QueryString["PID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(Request.QueryString["PID"]);
            }

        }
        public string ValidationGroupName
        {
            get { return (ViewState["ValidationGroupName"] == null) ? "main" : Convert.ToString(ViewState["ValidationGroupName"]); }
            set { ViewState["ValidationGroupName"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSave.ValidationGroup = ValidationGroupName;
                btnEdit.ValidationGroup = ValidationGroupName;
            }
        }

        /// <summary>
        /// set enable disable button on user privileges
        /// </summary>
        private void CheckSecurity()
        {
            if (PID == 0)
                return;
            PrivilageBL privilagebl = new PrivilageBL();

            try
            {
                tblAppPrivilage privilage = null;//
                if (Session["GroupID"] == null)
                {
                    privilage = privilagebl.GetByPageID(PID, Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }
                else
                {
                    privilage = privilagebl.GetByPageID(PID, Convert.ToInt32(Session["GroupID"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                }

                //if( btnDelete.Enabled)
                //  btnDelete.Enabled = privilage.Delete;
                //if(btnEdit.Enabled)
                btnEdit.Enabled = privilage.CanEdit;

                //if( btnNew.Enabled)
                btnSave.Enabled = privilage.CanAdd;
                btnCancel.Enabled = privilage.CanAdd;
            }
            catch (Exception es)
            {
                if (es.Message.Contains("Object reference set"))
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
        }



        public void SetMode(PageMode mode)
        {
            CheckSecurity();
            //btnEdit.Enabled = btnSave.Enabled = btnDelete.Enabled = btnNew.Enabled = btnCancel.Enabled = btnPrint.Visible= btnPrint.Enabled= false;
            if (mode.Equals(PageMode.None))
            {
                btnNew.Enabled = true;
                btnSave.Visible = false;
            }
            if (mode.Equals(PageMode.New))
            {


                btnCancel.Visible = btnSave.Visible = true;

                btnEdit.Visible = false;
            }
            //else if (mode.Equals(PageMode.Edit))
            //{
            //    btnSave.Enabled = btnCancel.Enabled = true;

            //}
            else if (mode.Equals(PageMode.Edit))
            {

                btnCancel.Visible = btnEdit.Visible = true;

                btnSave.Visible = false;
            }
            //else if (mode.Equals(PageMode.Print ))
            //{
            //  btnDelete.Enabled = btnNew.Enabled = btnEdit.Enabled = btnPrint.Visible =btnPrint.Enabled = true;
            //}



        }

        public void DisableSave()
        {
            btnSave.Enabled = false;
        }
        public void EnableSave()
        {
            btnSave.Enabled = true;
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (ButtonClick != null)
                ButtonClick(this, new CommandEventArgs(e.CommandName, e.CommandArgument));
        }
    }
}
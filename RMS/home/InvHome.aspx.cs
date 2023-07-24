using System;
// 
namespace RMS.home
{
    public partial class InvHome : BasePage
    {
        #region DataMembers



        //DashBoardBL dashBoardBL = new DashBoardBL();


        #endregion

        #region Properties

        //public int GroupID
        //{
        //    get { return (ViewState["GroupID"] == null) ? 0 : Convert.ToInt32(ViewState["GroupID"]); }
        //    set { ViewState["GroupID"] = value; }
        //}


        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Stock").ToString();
                //if (Session["DateTimeFormat"] == null)
                //{
                //    Response.Redirect("~/login.aspx");
                //} 
             

            }
        }
        
        #endregion
    }
}
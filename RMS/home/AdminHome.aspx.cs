using System;
// 
namespace RMS.home
{
    public partial class AdminHome : BasePage
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
                //if (Session["DateTimeFormat"] == null)
                //{
                //    Response.Redirect("~/login.aspx");
                //} 

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "AdminHome").ToString();

                //Response.Cookies["uzr"].Values["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Home").ToString();
                int GroupID = 0;
                if (Session["GroupID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                }
                else
                {
                    GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                }
                //if (GroupID == 3)
                //{
                //    Response.Redirect("~/profile/empmgtview.aspx?PID=1");
                //}
            }
        }


        #endregion

        #region Helping Method



        #endregion
    }
}
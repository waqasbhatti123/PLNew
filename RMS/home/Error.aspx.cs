using System;

namespace RMS.home
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = Convert.ToString(Session["errors"]);
        }
    }
}
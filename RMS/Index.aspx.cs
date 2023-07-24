
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.Mail;
//using System.Net;
//using System.Net.Mail; 

using RMS.BL;
using System.Web;
using System.Configuration;
using System.Text;

namespace RMS
{
    public partial class Index : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url.ToString().ToLower().Contains("http://3wpvt.com/index.aspx") ||
                Request.Url.ToString().Contains("http://www.3wpvt.com/index.aspx"))
            {
                Response.Redirect("/3wpvt/login.aspx");
            }
            else if (Request.Url.ToString().ToLower().Contains("http://3weec.com/index.aspx") ||
                Request.Url.ToString().Contains("http://www.3weec.com/index.aspx"))
            {
                Response.Redirect("/3weec/login.aspx");
            }
            else if (Request.Url.ToString().ToLower().Contains("http://ksb.3werp.com") ||
                Request.Url.ToString().StartsWith("http://ksb"))
            {
                Response.Redirect("/ksb");
            }
            else if (Request.Url.ToString().ToLower().Contains("http://3werp.com/index.aspx") ||
                Request.Url.ToString().Contains("http://www.3werp.com/index.aspx"))
            {
                Response.Redirect("/login.aspx");
            }
            else
            {
                Response.Redirect("/login.aspx");
                divSelectURL.Visible = true;
            }

        }

     

    }
}
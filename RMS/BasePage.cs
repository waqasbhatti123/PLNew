using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;


/// <summary>
    /// Custom base page used for all web forms.
    /// </summary>
public class BasePage : Page
{
    private const string m_DefaultCulture = "en-us"; //"ar-ae"; //"en-us";

    protected override void InitializeCulture()
    {
        //retrieve culture information from session
        string culture = Convert.ToString(Session["MyCulture"]);

        //check whether a culture is stored in the session
        if (!string.IsNullOrEmpty(culture))
        { }
        else
        {
            culture = m_DefaultCulture;
            Session["MyCulture"] = culture;
        }


        //set culture to current thread
        CultureInfo cultureinfo = new CultureInfo(culture);
        Thread.CurrentThread.CurrentCulture = cultureinfo;
        Thread.CurrentThread.CurrentUICulture = cultureinfo;

        //call base class
        base.InitializeCulture();
    }
}
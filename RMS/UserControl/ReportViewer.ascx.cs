using System;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
namespace RMS.UserControl
{
  public partial class ReportViewer : System.Web.UI.UserControl
  {
    public void Show(string reportName, string methodName, string typeName, string entityName, string parameters,string reportParameters,Boolean isReport)
    {

      Microsoft.Reporting.WebForms.ReportDataSource data = new Microsoft.Reporting.WebForms.ReportDataSource();
      ObjectDataSource datasource = new ObjectDataSource();
      datasource.ID = "dt";
      datasource.SelectMethod = methodName;
      datasource.TypeName = typeName;
      //string branchID = "1";
      if (parameters.Length != 0)
      {
         string[] para   = parameters.Split(',');
         for (int i = 0; i < para.Length; i++)
         {
             string[] name = para[i].Split('=');

             datasource.SelectParameters.Add(name[0], name[1]);
             //if (name[0].Equals("branchId"))
             //{
             //    if (!name[1].Equals("0"))
             //    {
             //        branchID = name[1];
             //    }
             //}
         }
      }
      
      ReportParameter rpara;
      List<ReportParameter> paras = new List<ReportParameter>();
     if(reportParameters.Length !=0)
     {
       string[] rparas= reportParameters.Split(',');
       for (int i = 0; i < rparas.Length; i++)
       {
         string[] p=rparas[i].Split('=');
         rpara = new ReportParameter(p[0], p[1]);
         paras.Add(rpara);
       }
     }

     
       
     

//adding company logo param to report
        string rptLogoPath;
        //rptLogoPath = Convert.ToString(Request.Url);
        //rptLogoPath = rptLogoPath.Substring(0, rptLogoPath.IndexOf(Request.RawUrl, 0));

        //rptLogoPath = System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString();

        //rptLogoPath = "http://" + Request.Url.Host.ToString().Trim() + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim();
        
        rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

        rpara = new ReportParameter("rptPath", rptLogoPath);
        paras.Add(rpara);

        //paras.Add(new ReportParameter("rptPath3", Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim())));
        //paras.Add(new ReportParameter("rptUriPartial", Request.Url.GetLeftPart(UriPartial.Authority)));
        //paras.Add(new ReportParameter("rptPageResolveUrl", Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim())));
      data.DataSourceId = datasource.ID;
      data.Name = entityName;
      this.Controls.Add(datasource);
      //if (Session["MyCulture"].ToString().Equals("ar-ae"))
      //  rptViewer.LocalReport.ReportPath = Server.MapPath("~/Report/ar/" + reportName);
      //else
      if (isReport)
      {
          rptViewer.LocalReport.ReportPath = Server.MapPath("~/Report/rdlc/" + reportName);
      }
      else
      {
          rptViewer.LocalReport.ReportPath = Server.MapPath("~/chart/rdlcchart/" + reportName);
      }
        //Code for showing Logo

      rptViewer.LocalReport.EnableExternalImages = true;
      rptViewer.LocalReport.Refresh();
      
      rptViewer.LocalReport.SetParameters(paras);
      rptViewer.LocalReport.DataSources.Add(data);


    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
  }
}
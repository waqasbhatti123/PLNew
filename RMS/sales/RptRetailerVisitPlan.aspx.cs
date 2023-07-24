using System;
using System.Web.UI;
using RMS.BL;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace RMS.sales
{
    public partial class RptRetailerVisitPlan : System.Web.UI.Page
    {
        #region DataMember
        
        SalesRptBL slBL = new SalesRptBL();
        SalesPersonBL salesPersonBL = new SalesPersonBL();
        RetailerVisitsBL retailerVisitBL = new RetailerVisitsBL();
        AreaCodeBL areaCodeBL = new AreaCodeBL();
        #endregion

        #region Properties

        public int CompId
        {
            get { return (ViewState["CompID"] == null) ? 0 : Convert.ToInt32(ViewState["CompID"]); }
            set { ViewState["CompID"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            int iCompid;
            if (Session["CompID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                int.TryParse(Request.Cookies["uzr"]["CompID"], out iCompid);
            }
            else
            {
                int.TryParse(Session["CompID"].ToString(), out iCompid);
            }

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "VisitPlanReport").ToString();
               
                CompId = iCompid;

                calFromDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                txtFromDate.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month + "-01-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year).
                                ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                calToDate.Format = System.Configuration.ConfigurationManager.AppSettings["DateFormat"];
                txtToDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).
                                ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                btnGenerat.Focus();

                BindAreaDropDown();
                BindSalesPersonDropDown();
                BindSubAreaDropDown("0");
            }
        }
        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateReport("Retailer Visit Plan");
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubAreaDropDown(ddlArea.SelectedValue);
        }

        //protected void ddlSubArea_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //}
        //protected void ddlSalesman_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //}

        #endregion

        #region HelpingMethods

        private void BindSalesPersonDropDown()
        {
            ddlSalesman.DataValueField = "ID";
            ddlSalesman.DataTextField = "SalesPerson";
            ddlSalesman.DataSource = salesPersonBL.GetAllSalesPerson((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSalesman.DataBind();

        }

        private void BindAreaDropDown()
        {
            ddlArea.DataValueField = "ar_cd";
            ddlArea.DataTextField = "ar_dsc";
            ddlArea.DataSource = areaCodeBL.GetAllAreas((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlArea.DataBind();
        }

        private void BindSubAreaDropDown(string areaCd)
        {
            ddlSubArea.DataValueField = "ar_cd";
            ddlSubArea.DataTextField = "ar_dsc";
            ddlSubArea.DataSource = areaCodeBL.GetAllSubAreasByAreaCode(areaCd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSubArea.DataBind();
        }

        protected void GenerateReport(string reportName)
        {
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());

            viewer.Visible = true;
            DateTime fromDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                fromDate = Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                ucMessage.ShowMessage("Please, enter valid from date", RMS.BL.Enums.MessageType.Error);
                return;
            }
            DateTime toDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            try
            {
                toDate = Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                ucMessage.ShowMessage("Please, enter valid to date", RMS.BL.Enums.MessageType.Error);
                return;
            }

            //List<spVisitPlanRptResult> visitPlan;
            //visitPlan = retailerVisitBL.GetVisitPlanReport(ddlArea.SelectedValue,ddlSubArea.SelectedValue,Convert.ToInt32(ddlSalesman.SelectedValue),
                                        //fromDate, toDate,ddlSort.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            viewer.LocalReport.ReportPath = "sales/rdlc/VisitPlan.rdlc";
            ReportDataSource datasource = new ReportDataSource("spVisitPlanRptResult", " ");//visitPlan);

            ReportParameter[] paramz = new ReportParameter[5];
            
            paramz[0] = new ReportParameter("ReportName", reportName);
            paramz[1] = new ReportParameter("LogoPath", rptLogoPath);
            if (Session["CompName"] == null)
            {
                paramz[2] = new ReportParameter("CompanyName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[2] = new ReportParameter("CompanyName", Session["CompName"].ToString(), false);
            }
            

            paramz[3] = new ReportParameter("FromDate", fromDate.ToString(), false);
            paramz[4] = new ReportParameter("ToDate", toDate.ToString(), false);

            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
        }

        #endregion
    }
}

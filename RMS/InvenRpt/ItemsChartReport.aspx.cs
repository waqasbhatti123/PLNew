using System;
#pragma warning disable CS0105 // The using directive for 'System' appeared previously in this namespace
using System;
#pragma warning restore CS0105 // The using directive for 'System' appeared previously in this namespace
#pragma warning disable CS0105 // The using directive for 'System' appeared previously in this namespace
using System;
#pragma warning restore CS0105 // The using directive for 'System' appeared previously in this namespace
#pragma warning disable CS0105 // The using directive for 'System' appeared previously in this namespace
using System;
#pragma warning restore CS0105 // The using directive for 'System' appeared previously in this namespace
using RMS.BL;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace RMS.InvenRpt
{
    public partial class ItemsChartReport : BasePage
    {
        #region DataMembers

        InvReports_BL rptBl = new InvReports_BL();

        InvCode InvCode = new InvCode();

     

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
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

                if (Session["DateFormat"] == null)
                {
                    //CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    //CalendarExtender1.Format = Session["DateFormat"].ToString();
                }
                if (Session["DateFormat"] == null)
                {
                    //CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    //CalendarExtender2.Format = Session["DateFormat"].ToString();
                }
                //CalendarExtender1.SelectedDate = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-07" + "-01");
                //CalendarExtender2.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                

                PID =Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 530)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ItemsMasterList").ToString();
                  
                }

                BindDDLGroup();
                FillDdlItemGroup();
               // pnlControl.Visible = false;
                lblControlName.Visible = false;
                ddlControlName.Visible = false;
                reportViewer.Visible = false;
            }
        }
    
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string grp=ddlGroupName.SelectedValue.ToString();
            string cntrl = ddlControlName.SelectedValue.ToString();
            if (ddlGroupName.SelectedIndex == 0)
            {
                grp = "";
            }
            if (ddlControlName.SelectedIndex == 0)
            {
                cntrl = "";
            }

            string filter;
            if (ddlControlName.Visible == true)
            {
                filter = "| Item Group : " + ddlItemGroup.SelectedItem.Text +" | " + 
                    " |Group : " + ddlGroupName.SelectedItem.Text + " | Control : " + ddlControlName.SelectedItem.Text + " |";
            }
            else
            {
                filter = "| Item Group : " + ddlItemGroup.SelectedItem.Text + " | " + 
                    " |Group : " + ddlGroupName.SelectedItem.Text + " | Control : All |";
            }
                try
            {


                reportViewer.Visible = false;
                reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/ItemMasterListRpt.rdlc";
                reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.Refresh();

                List<spItemMasterListReportResult> ItemMaster = rptBl.getItemMasterListReportData(grp, cntrl, Convert.ToInt32(ddlItemGroup.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                ReportDataSource dataSource = new ReportDataSource("spItemMasterListReportResult", ItemMaster);

                ReportParameter[] rpt = new ReportParameter[2];
                rpt[0] = new ReportParameter("ReportName", "Item Master List");
                rpt[1] = new ReportParameter("filter", filter);
                

                reportViewer.LocalReport.SetParameters(rpt);
                reportViewer.LocalReport.DataSources.Clear();

                reportViewer.LocalReport.DataSources.Add(dataSource);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                string filename;

                string ext = "pdf", type = "PDF";
                if (ddlExtension.SelectedValue == "Excel")
                {
                    ext = "xls";
                    type = "Excel";
                }
                byte[] bytes = reportViewer.LocalReport.Render(
                   type, null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                filename = string.Format("{0}.{1}", "ItemMasterList", ext);
                Response.ClearHeaders();
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                Response.ContentType = mimeType;
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();

            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }


        //protected void ddlItemType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string index = ddlItemType.SelectedIndex.ToString();

        //    if (index == "1")
        //    {
              
        //        ForGroup.Visible = true;
        //        ForControls.Visible = false;
        //        BindDDLGroup();
              
        //    }
        //    else if (index == "2")
        //    {
        //        ForGroup.Visible = true;
        //        ForControls.Visible = true;
        //        BindDDLGroup();
        //        BindDDLControl();
        //    }
        //    else if (index == "3")
        //    {
        //        ForGroup.Visible = true;
        //        ForControls.Visible = true;
                
        //        //ForDetail.Visible = true;
        //    }
        //    else
        //    {
        //        ForGroup.Visible = false;
        //        ForControls.Visible = false;
        //       // ForDetail.Visible = false;
        //    }
        //}

        protected void ddlGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGroupName.SelectedIndex != 0)
            {
                lblControlName.Visible = true;
                ddlControlName.Visible = true;
                //pnlControl.Visible = true;
                BindDDLControlForGroup(ddlGroupName.SelectedValue);
            }
            else
            {
                //pnlControl.Visible = false;
                lblControlName.Visible = false;
                ddlControlName.Visible = false;
            }
        }
        protected void ddlControlName_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void ddlDetailName_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        #endregion

        #region Helping Method

        private void FillDdlItemGroup()
        {
            ddlItemGroup.DataSource = new ItemCodeBL().GetItemGroup((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItemGroup.DataTextField = "itm_grp_desc";
            ddlItemGroup.DataValueField = "itm_grp_id";
            ddlItemGroup.DataBind();
        }

        public void BindDDLGroup()
        {
            ddlGroupName.Items.Clear();
            ddlGroupName.Items.Insert(0, "All");
            ddlGroupName.DataSource = InvCode.GetGroups('A', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlGroupName.DataTextField = "itm_dsc";
            ddlGroupName.DataValueField = "itm_cd";
            ddlGroupName.DataBind();
            
        }

        public void BindDDLControlForGroup(string controlId)
        {
            ddlControlName.Items.Clear();
            ddlControlName.Items.Insert(0, "All");
            ddlControlName.DataSource = InvCode.GetControlsForItemReport('C', controlId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlControlName.DataTextField = "itm_dsc";
            ddlControlName.DataValueField = "itm_cd";
            ddlControlName.DataBind();
        }

    
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using Microsoft.Reporting.WebForms;

namespace RMS.GLSetup
{
    public partial class frmGLAccInfo : BasePage
    {
        COA_BL cty = new COA_BL();
        ListItem selitm = new ListItem();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

        #region event
        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack)
            {
                if (Session["BranchID"] == null)
                {
                    BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GlAccInfo").ToString();
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();

                BindDropDown();
            
            }
        }


        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            this.searchBranchDropDown.DataTextField = "br_nme";
            searchBranchDropDown.DataValueField = "br_id";
            if (BranchObj.IsHead == true)
            {
                searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                List<Branch> BranchList = new List<Branch>();

                if (BranchObj != null)
                {
                    if (BranchObj.IsDisplay == true)
                    {
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BranchID).ToList();
                        BranchList.Insert(0, BranchObj);
                    }
                    else
                    {
                        BranchList.Add(BranchObj);
                    }
                }
                searchBranchDropDown.DataSource = BranchList.ToList();
            }
            searchBranchDropDown.DataBind();

        }



        protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!searchBranchDropDown.SelectedValue.Equals("0"))
                {
                    
                    BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue.Trim());
                   
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }


        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            CreatePDF("Chart_OF_Account", "pdf"); 
            
        }
        #endregion

        #region helpingmethods
        protected void BindDropDown()
        {
            ddlgltype.Items.Clear();
            ddlgltype.Dispose();
            selitm.Text = "All";
            selitm.Value = "X";
            ddlgltype.Items.Insert(0, selitm);
            ddlgltype.DataTextField = "gt_dsc";
            ddlgltype.DataValueField = "gt_cd";
            ddlgltype.DataSource = cty.GetAll((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            ddlgltype.DataBind();
            

        
        }
        protected void CreatePDF(String FileName, String extension)
        {
            //// Variables
            //Warning[] warnings = null;
            //String[] streamids = null;
            //string mimeType = null;
            //string encoding = null;
            ////The DeviceInfo settings should be changed based on the reportType
            ////http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            //string deviceInfo =
            //"<DeviceInfo>" +
            //"  <OutputFormat>" + extension + "</OutputFormat>" +
            //"  <PageWidth>8.27in</PageWidth>" +
            //"  <PageHeight>11.69in</PageHeight>" +
            //"  <MarginTop>0.5in</MarginTop>" +
            //"  <MarginLeft>0.5in</MarginLeft>" +
            //"  <MarginRight>0.5in</MarginRight>" +
            //"  <MarginBottom>0.5in</MarginBottom>" +
            //"</DeviceInfo>";

            //// Setup the report viewer object and get the array of bytes
            //ReportViewer viewer = new ReportViewer();
            //IQueryable<spRptEmployeeListResult> sal;
            //sal = SalBl.RptEmployeeRec(CompId, PayPerd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            IList<spGLAccountInfoResult> sal = cty.GetGLAccountInfoReport((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], Convert.ToChar(ddlgltype.SelectedItem.Value));
            viewer.LocalReport.ReportPath = "GLSetup/rdlc/rptGLAccInfo.rdlc";
            ReportDataSource datasource = new ReportDataSource("spGLAccountInfoResult", sal);
            //ReportParameter prm = new ReportParameter("Loan_ReportResult");
            ReportParameter[] paramz = new ReportParameter[1];

            //paramz[0] = new ReportParameter("rpt_Prm_PayPeriod", ddfrom.ToString("MMM-yyyy"), false);

            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }


            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.SetParameters(paramz);
            //ReportViewer1 = viewer;

            //Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            ////Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", ("attachment; filename=" + FileName + "_" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) + ".") + extension);
            //Response.BinaryWrite(bytes);
            ////// create the file
            ////// send it to the client to download
            //Response.Flush();
        }
        #endregion

        
    }
}

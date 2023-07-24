using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using RMS.BL;

namespace RMS.GLSetup
{

    public partial class frmChqClearanceRpt : BasePage
    {

        #region DataMembers
        LedgerCardBL cty = new LedgerCardBL();
        PucarAccountReportsBL pucarBl = new PucarAccountReportsBL();
        CuurentYearBL cuurentYear = new CuurentYearBL();

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"]= value;}
        }

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }


        #endregion
        #region event
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "frmChqClearanceRpt").ToString();

            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
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

                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"];
                    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"];
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                    CalendarExtender2.Format = Session["DateFormat"].ToString();
                }
                                 
                txtFltFromDt.Text = cuurentYear.fromFinalYearDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                txtFltToDt.Text = cuurentYear.toFinalYearDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();

                FillHeadsDetail();
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
                    //this.BindGrid(VoucherTypeID);
                    // BindSalaryPackage(BranchID, IsSearch);
                }

            }
            catch
            { }
        }

        private void FillHeadsDetail()
        {
            RMSDataContext db = new RMSDataContext();

            this.codeDropDown.DataTextField = "gl_dsc";
            codeDropDown.DataValueField = "gl_cd";

            codeDropDown.DataSource = db.Glmf_Codes.Where(x => x.ct_id == "D").ToList();
            codeDropDown.DataBind();


        }
        protected void btnGenerat_Click(object sender, EventArgs e)
        {
            try
            {
                string txt = "FromDate";
                try
                {
                    Convert.ToDateTime(txtFltFromDt.Text);
                    txt = "ToDate";
                    Convert.ToDateTime(txtFltToDt.Text);
                }
                catch
                {
                    if (txt.Equals("FromDate"))
                    {
                        ucMessage.ShowMessage("Invalid from date", RMS.BL.Enums.MessageType.Error);
                        txtFltFromDt.Focus();
                    }
                    else
                    {
                        ucMessage.ShowMessage("Invalid to date", RMS.BL.Enums.MessageType.Error);
                        txtFltToDt.Focus();
                    }
                    return;
                }
                CreatePDF("ChqClrearance", "pdf");
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        #endregion
        #region helpingmethod
        protected void CreatePDF(String FileName, String extension)
        {
            DateTime dtFrm = Convert.ToDateTime(txtFltFromDt.Text.Trim());
            DateTime dt2 = Convert.ToDateTime(txtFltToDt.Text.Trim());

            //List<spCheqDetailsResult> sal = new ChqBL().GetChqDetails(Convert.ToInt32(ddlFltType.SelectedValue),
            //                                txtFltBank.Text, txtFltAC.Text, txtFltChq.Text, 
            //                                Convert.ToDateTime(txtFltFromDt.Text.Trim()),
            //                                Convert.ToDateTime(txtFltToDt.Text.Trim()), BranchID,                          
            //                                (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);

            List<spCheqDetailsResult> sal1 = pucarBl.ChequeReport(Convert.ToInt32(ddlFltType.SelectedValue),
                                            txtFltBank.Text, txtFltChq.Text, txtFltVoucher.Text, 
                                            codeDropDown.SelectedValue.Trim(),
                                            Convert.ToDateTime(txtFltFromDt.Text.Trim()),
                                            Convert.ToDateTime(txtFltToDt.Text.Trim()), BranchID,
                                            (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]).ToList();

            viewer.LocalReport.ReportPath = "GLSetup/rdlc/ChqClearance.rdlc";



            ReportDataSource datasource = new ReportDataSource("spCheqDetailsResult", sal1);
            ReportParameter[] paramz = new ReportParameter[3];
            if (Session["CompName"] == null)
            {
                paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            }
            else
            {
                paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            }
            paramz[1] = new ReportParameter("dateRange","Period From:  " + dtFrm.ToString("dd-MMM-yyyy") + "  To  " + dt2.ToString("dd-MMM-yyyy"));
            paramz[2] = new ReportParameter("PageTitle", Session["PageTitle"].ToString());

            
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

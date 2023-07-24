using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Data.Linq;
using Microsoft.Reporting.WebForms;
using System.Web.Services;
using System.Web;

namespace RMS.Inv
{
    public partial class POClosureMgt : BasePage
    {
        #region DataMembers

        POrderBL poBL = new POrderBL();

        #endregion

        #region Properties

        public int rowsCount
        {
            set { ViewState["rowsCount"] = value; }
            get { return Convert.ToInt32(ViewState["rowsCount"] ?? 0); }
        }
        
#pragma warning disable CS0114 // 'POClosureMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'POClosureMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
        public int PID
        {
            get { return (ViewState["PID"] == null) ? 0 : Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
        public int BrID
        {
            get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
        }

        public int DocNo
        {
            get { return (ViewState["DocNo"] == null) ? 0 : Convert.ToInt32(ViewState["DocNo"]); }
            set { ViewState["DocNo"] = value; }
        }

        public decimal OverAllDisc
        {
            get { return (ViewState["OverAllDisc"] == null) ? 0 : Convert.ToDecimal(ViewState["OverAllDisc"]); }
            set { ViewState["OverAllDisc"] = value; }
        }

        public string DocNoFormated
        {
            get { return Convert.ToString(ViewState["DocNoFormated"]); }
            set { ViewState["DocNoFormated"] = value; }
        }

        public bool IsEdit
        {
            get { return Convert.ToBoolean(ViewState["IsEdit"]); }
            set { ViewState["IsEdit"] = value; }
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
            PID = Convert.ToInt32(Request.QueryString["PID"]);
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "POClosureMgt").ToString();
                if (Session["BranchID"] == null)
                {
                    BrID = Convert.ToByte(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    BrID = Convert.ToByte(Session["BranchID"].ToString());
                }
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                }
                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender2.Format = Session["DateFormat"].ToString();
                }

                //GetDocNo();
                FillDropDownVendor();
                FillDropDownWHT();
                FiilDdlCurrency();
                BindGridMain("","");
                ucButtons.DisableSave();
              
                IsEdit = false;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltPoNo.Text.Trim(), txtFltParty.Text.Trim());
        }
        protected void grdPo_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsEdit = true;

            ID = Convert.ToInt32(grdPo.SelectedDataKey.Values["vr_id"]);
            GetByID();
            BindGridPODet(ID);
            BindGridMain(txtFltPoNo.Text.Trim(), txtFltParty.Text.Trim());
            txtOverAllDisc.Text = poBL.GetOverAllDiscount(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString();
            ucButtons.EnableSave();
        }
        protected void grdPo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPo.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltPoNo.Text.Trim(), txtFltParty.Text.Trim());
        }
        protected void grdPo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[0].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToString(Session["DateFormat"].ToString());
                }

                if (e.Row.Cells[4].Text.Equals("A"))
                {
                    e.Row.Cells[4].Text = "Approved";

                }
                else if (e.Row.Cells[4].Text.Equals("P"))
                {
                    e.Row.Cells[4].Text = "Pending";
                    e.Row.Cells[6].Text = "";
                }
                else if (e.Row.Cells[4].Text.Equals("C"))
                {
                    e.Row.Cells[4].Text = "Cancelled";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                }
            }
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {
                if (SavePO())
                {
                    ClearFieldsOnly();
                    BindGridPODet(ID);
                    BindGridMain(txtFltPoNo.Text.Trim(), txtFltParty.Text.Trim());
                    ucButtons.DisableSave();
                }
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }

        #endregion

        #region Helping Method

        public void BindGridPODet(int vrid)
        {
            grdPODet.DataSource = poBL.GetPoDetailsByID(vrid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdPODet.DataBind();
        }
        public void GetPurchaseOrder()
        {
            //string showGST="No", vendor = "", brName = "", brAddress = "", brTel = "", brFax = "", brNTN = "", brSTN = "", updateby, approvedby;
            //decimal gst = 0;
            //reportViewer.Visible = false;
            //reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/PO.rdlc";
            //reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            //reportViewer.LocalReport.Refresh();
 
            //List<Anonymous4PO> po = poBL.GetPORec(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //List<spPORptResult> res = poBL.GetPORptRes(Convert.ToInt32(po.First().vr_id), 'A', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //foreach (var r in res)
            //{
            //    gst = gst + r.GST;
            //}
            //if (gst > 0)
            //    showGST = "Yes";
            //ReportDataSource dataSource = new ReportDataSource("Anonymous4PO", po);
            //vendor = po.First().vendor;
            //string rptLogoPath = "";
            //string company="";
            //rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            
            //if (Session["CompName"] == null)
            //{
            //    if (Request.Cookies["uzr"]["CompName"] == null)
            //    {
            //        Response.Redirect("~/login.aspx");
            //    }
            //    else
            //    {
            //        company = Request.Cookies["uzr"]["CompName"].ToString().ToString();
            //    }
            //}
            //else
            //{
            //    company = Session["CompName"].ToString();
            //}
           
            //updateby = po.First().CreatedBy;
            //approvedby = po.First().ApprovedBy;
            //try
            //{
            //    int brid = 0;
            //    if (Session["BranchID"] == null)
            //    {
            //        brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
            //    }
            //    else
            //    {
            //        brid = Convert.ToInt32(Session["BranchID"].ToString());
            //    }
            //    Branch branch = new voucherHomeBL().GetBranch(brid, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
            //    if (branch != null)
            //    {
            //        brName = branch.br_nme;
            //        brAddress = branch.br_address;
            //        brTel = branch.br_tel;
            //        brFax = branch.br_fax;
            //        brNTN = branch.br_ntn;
            //        brSTN = branch.br_stx_no;
            //    }
            //}
            //catch { }

            //ReportParameter[] rpt = new ReportParameter[13];
            //rpt[0] = new ReportParameter("ReportName", "Purchase Order");
            //rpt[1] = new ReportParameter("LogoPath", rptLogoPath);
            //rpt[2] = new ReportParameter("CompanyName", company);

            //rpt[3] = new ReportParameter("brName", brName, false);
            //rpt[4] = new ReportParameter("brAddress", brAddress, false);
            //rpt[5] = new ReportParameter("brTel", brTel, false);
            //rpt[6] = new ReportParameter("brFax", brFax, false);
            //rpt[7] = new ReportParameter("brNTN", brNTN, false);
            //rpt[8] = new ReportParameter("brSTN", brSTN, false);

            //rpt[9] = new ReportParameter("AprBy", approvedby, false);
            //rpt[10] = new ReportParameter("UpdBy", updateby, false);
            //rpt[11] = new ReportParameter("vendor", vendor, false);
            //rpt[12] = new ReportParameter("showGST", showGST, false);

            //reportViewer.LocalReport.EnableExternalImages = true;
            //reportViewer.LocalReport.Refresh();
            //reportViewer.LocalReport.SetParameters(rpt);

            //reportViewer.LocalReport.DataSources.Clear();
            //reportViewer.LocalReport.DataSources.Add(dataSource);

            //Warning[] warnings;
            //string[] streamids;
            //string mimeType;
            //string encoding;
            //string extension;
            //string filename;
            //byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            //filename = string.Format("{0}.{1}", "PurchaseOrder", "pdf");
            //Response.ClearHeaders();
            //Response.Clear();
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            //Response.ContentType = mimeType;
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();

        }
        private void GetByID()
        {

            tblPOrder po = poBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlPoType.SelectedValue = po.PO_Type;
            ddlStatus.SelectedValue = po.vr_apr.ToString();
            txtPoRev.Text = po.RevSeq.Value == 0 ? "" : po.RevSeq.Value.ToString();
            txtPoNo.Text = po.vr_no.ToString().Substring(0, 4) + "/" + po.vr_no.ToString().Substring(4);
            DocNoFormated = po.vr_no.ToString().Substring(0, 4) + "/" + po.vr_no.ToString().Substring(4);
            DocNo = po.vr_no;

            ddlCurrency.SelectedValue = po.CRUNCY;

            ddlVendor.SelectedValue = po.VendorId;
            ddlShipment.SelectedValue = po.Ship_Mode;

            txtDelLoc.Text = po.Del_Loc;
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
            if (po.Del_Days != null && po.Del_Days != 0)
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                txtDelPeriod.Text = po.Del_Days.ToString();
            else
                txtDelPeriod.Text = "";
            txtPayDays.Text = (po.Pay_Days != null && po.Pay_Days.Value > 0) ? po.Pay_Days.Value.ToString() : "";
            txtPayTerms.Text = po.Pay_Terms;
            txtTerms.Text = po.vr_nrtn;
            txtInst.Text = po.PO_Instructions;
            txtQtyVar.Text = (po.QTY_Var_Pc != null && po.QTY_Var_Pc.Value > 0) ? po.QTY_Var_Pc.Value.ToString() :"";

            txtVendorDocRef.Text = string.IsNullOrEmpty(po.DocRef) ? "" : po.DocRef;
            
            if (po.WHTid != null)
            {
                ddlWHT.SelectedValue = po.WHTid;
            }
            else
            {
                ddlWHT.SelectedValue = "0";
            }
            
            if (po.DocRefDate != null)
            {
                txtRefDate.Text = po.DocRefDate.Value.ToString("dd-MMM-yy");
            }
            else
            {
                txtRefDate.Text = "";
            }

            if (Session["DateFormat"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    txtPoDate.Text = po.vr_dt.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                txtPoDate.Text = po.vr_dt.ToString(Session["DateFormat"].ToString());
            }

        }
        private void ClearFields()
        {
            Response.Redirect("~/inv/poclosuremgt.aspx?PID="+PID);
        }
        private void ClearFieldsOnly()
        {
            ID = 0;
  
            ddlPoType.SelectedIndex = 1;
            ddlSupType.SelectedIndex = 1;
            ddlCurrency.SelectedValue = "0";
            ddlWHT.SelectedValue = "0";
            txtPoDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yy");
            ddlStatus.SelectedValue = "P";
            ddlVendor.SelectedIndex = 0;
            ddlShipment.SelectedIndex = 1;
            txtDelLoc.Text = "";
            txtDelPeriod.Text = "";
            txtPayDays.Text = "";
            txtQtyVar.Text = "";
            txtPayTerms.Text = "";
            txtTerms.Text = "";
            txtInst.Text = "";
            txtPoNo.Text = "";
            DocNo = 0;
            DocNoFormated = "";
            txtVendorDocRef.Text = "";
            txtRefDate.Text = "";
            txtPoRev.Text = "";
            txtOverAllDisc.Text = "";
            Session["POREF"] = null;
            ucButtons.DisableSave();
            IsEdit = false;
        }
        private bool SavePO()
        {
            tblPOrder po = poBL.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            po.Status = "Cl";
            string msg = poBL.Update(po, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (msg == "Done")
            {
                ucMessage.ShowMessage("PO " + po.vr_no.ToString().Substring(0, 4) + "/" + po.vr_no.ToString().Substring(4) + " closed successfully", RMS.BL.Enums.MessageType.Info);
                return true;
            }
            else
            {
                ucMessage.ShowMessage("Exception: " + msg, RMS.BL.Enums.MessageType.Error);
                return false;
            }
        }
        public void FillDropDownVendor()
        {
            ddlVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.DataTextField = "gl_dsc";
            ddlVendor.DataValueField = "gl_cd";
            ddlVendor.DataBind();
        }
        public void FillDropDownWHT()
        {
            ddlWHT.DataSource = new TaxBL().GetWHTTaxes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlWHT.DataTextField = "TaxDesc";
            ddlWHT.DataValueField = "TaxID";
            ddlWHT.DataBind();
        }
        private void BindGridMain(string poNo, string partyNme)
        {
            if (!poNo.Equals(""))
            {
                if (poNo.Contains("/") && poNo.Length > 5)
                {
                    try
                    {
                        char[] st = poNo.ToCharArray();
                        if (st[4].ToString().Equals("/"))
                        {
                            poNo = poNo.Substring(0, 4) + poNo.Substring(5);
                        }
                        else
                        {
                            poNo = "";
                            for (int i = 0; i < st.Length; i++)
                            {
                                if (!st[i].ToString().Equals("/"))
                                {
                                    poNo = poNo + st[i];
                                }
                            }
                        }
                    }
                    catch { }

                }
                else if (poNo.Contains("/"))
                {
                    char[] st = poNo.ToCharArray();
                    poNo = "";
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (!st[i].ToString().Equals("/"))
                        {
                            poNo = poNo + st[i];
                        }
                    }
                }
                else
                {
                    //nothin
                }
            }

            grdPo.DataSource = poBL.GetPOsToClose(BrID, poNo, partyNme, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdPo.DataBind();
        }
        public void GetDocNo()
        {
            DocNoFormated = poBL.GetPoNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
        }
        public void FiilDdlCurrency()
        {
            ddlCurrency.DataSource = new CurrencyBL().GetAllCurrencies((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCurrency.DataTextField = "CRUNCY";
            ddlCurrency.DataValueField = "CRUNCY";
            ddlCurrency.DataBind();
        }

        #endregion

    }
}

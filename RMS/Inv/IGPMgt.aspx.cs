using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Data.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace RMS.Inv
{
    public partial class IGPMgt : BasePage
    {

        #region DataMembers
        InvGP_BL gP = new InvGP_BL();
        IGPBL igpBL = new IGPBL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        short VoucherTypeCode = 12; // voucher type code 12 for GATE PASS in Vr_Type table

        string srNo, grdPoRef, itemCode, itemDesc, packs, unitsize, qty, uomqty, gross, rem, seqno;//uom,

        #endregion

        #region Properties
        
        public DataTable CurrentTable
        {
            set { ViewState["CurrentTable"] = value; }
            get { return (DataTable)(ViewState["CurrentTable"] ?? 0); }
        }
        
        public int rowsCount
        {
            set { ViewState["rowsCount"] = value; }
            get { return Convert.ToInt32(ViewState["rowsCount"] ?? 0); }
        }
        public int LocID
        {
            get { return (ViewState["LocID"] == null) ? 0 : Convert.ToInt32(ViewState["LocID"]); }
            set { ViewState["LocID"] = value; }
        }
        public int BrID
        {
            get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
        }
        public int VtCD
        {
            get { return (ViewState["VtCD"] == null) ? 0 : Convert.ToInt32(ViewState["VtCD"]); }
            set { ViewState["VtCD"] = value; }
        }
        public int VrId
        {
            get { return (ViewState["VrId"] == null) ? 0 : Convert.ToInt32(ViewState["VrId"]); }
            set { ViewState["VrId"] = value; }
        }
        public int VrNO
        {
            get { return (ViewState["VrNO"] == null) ? 0 : Convert.ToInt32(ViewState["VrNO"]); }
            set { ViewState["VrNO"] = value; }
        }

        public int DocNo
        {
            get { return (ViewState["DocNo"] == null) ? 0 : Convert.ToInt32(ViewState["DocNo"]); }
            set { ViewState["DocNo"] = value; }
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

            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "IGPMgt").ToString();
                if (Session["DateFormat"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                }
                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;

                BindTable();

                FillDropDownLoc();
                FillDropDownVendor();
                FillDropDownCity();
                
                //GetGatePassNo();
                GetDocRef();

                BindGridMain("","",0,"0");
                IsEdit = false;
                IGPBL.IsEditable = false;
                ddlVendor.Enabled = true;
                ddlLoc.Focus();
               
                txtPoSrch.Attributes.Add("onclick", "javascript:this.select();");
            }
        }
        protected void ddlLoc_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string party = ddlVendor.SelectedValue == "0" ? "" : ddlVendor.SelectedValue;
            string status = ddlStatus.SelectedValue == "0" ? "" : ddlStatus.SelectedValue;
            //BindGridMain("", party, Convert.ToInt32(ddlCity.SelectedValue), status);
            BindGridMain(txtFltIgpNo.Text.Trim(), txtFltParty.Text.Trim(), Convert.ToInt32(ddlFltCity.SelectedValue), ddlFltStatus.SelectedValue);
        }
        protected void grdIgp_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsEdit = true;
            IGPBL.IsEditable = true;
            ddlVendor.Enabled = false;

            LocID = Convert.ToInt32(grdIgp.SelectedDataKey.Values["locid"]);
            BrID = Convert.ToInt32(grdIgp.SelectedDataKey.Values["br_id"]);
            VtCD = Convert.ToInt32(grdIgp.SelectedDataKey.Values["vt_cd"]);
            VrNO = Convert.ToInt32(grdIgp.SelectedDataKey.Values["vr_no"]);

            DocNo = Convert.ToInt32(grdIgp.SelectedDataKey.Values["vr_no"]);
            DocNoFormated = grdIgp.SelectedDataKey.Values["vr_no"].ToString().Substring(0, 4) + "/" + grdIgp.SelectedDataKey.Values["vr_no"].ToString().Substring(4);

            
            GetByID();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //DropDownList ddVen = (DropDownList)e.Row.Cells[0].FindControl("ddlItem");
                //DropDownList ddUom = (DropDownList)e.Row.Cells[1].FindControl("ddlUom");
                //Label ddUomQty = (Label)e.Row.Cells[1].FindControl("txtUomQty");
                
                //BindDdlUOM(ddUom);
                //BindDdlUOM(ddUomQty);
                //BindddlItemDD(ddVen);

            }

        }
        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
            SetDropDownList();
        }
        private string GetItemUOMLabelFromUOMId(byte uomid)
        {
            string uomDesc = "";
            uomDesc = igpBL.GetUOMDescFromID(uomid, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            return uomDesc;
        }
        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            int brid = 0;
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                brid = Convert.ToInt32(Session["BranchID"].ToString());
            }

            if (e.CommandName == "Save")
            {
                //Validate
                int valCount = 0, repeatCount = 0, repeatCount1 = 0, grdRowsCount;
                decimal totalqty=0;
                string itmcd, poref, qty, pack, unitsize;
                grdRowsCount = GridView1.Rows.Count;
                for (int i = 0; i < grdRowsCount; i++)
                {
                    repeatCount = 0; 
                    repeatCount1 = 0;
                    poref = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPoref")).Text;
                    itmcd = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                    qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                    pack = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPacks")).Text.Trim();
                    unitsize = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUnitSize")).Text.Trim();


                    if (itmcd != "" && qty != "" && pack != "" && unitsize != "")
                    {
                        if (poref != "99999" && !new POrderBL().IsItemExistsInPo(brid, Convert.ToInt32(poref), itmcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                        {
                            ucMessage.ShowMessage("Item " + itmcd + " does not exist in po", RMS.BL.Enums.MessageType.Error);
                            return;
                        }

                        totalqty = totalqty + Convert.ToDecimal(qty);
                        valCount++;

                        for (int j = 0; j < grdRowsCount; j++)
                        {
                            if (itmcd != "" && itmcd == ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtItem")).Text)
                            {
                                repeatCount1++;
                            }
                        }
                        if (repeatCount > 1 || repeatCount1 > 1)
                        {
                            ucMessage.ShowMessage("Item cannot be selected more than once", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                }
                if (valCount == 0)
                {
                    ucMessage.ShowMessage("Please select atleast one item to continue", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                if (totalqty != Convert.ToDecimal(txtTotalQty.Text.Trim()))
                {
                    ucMessage.ShowMessage("Total quantity is not equal to items total qty", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                /***********************************************************************/
                for (int i = 0; i < grdRowsCount; i++)
                {
                    itmcd = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text.Trim();

                    if (itmcd != "" && !new ItemCodeBL().IsItemExists(itmcd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage(itmcd + " does not exist in database", RMS.BL.Enums.MessageType.Error);
                        return;
                    }
                }
                /***********************************************************************/
                if (SaveIGP())
                {
                    ClearFieldsOnly();
                    BindGridMain(txtFltIgpNo.Text.Trim(), txtFltParty.Text.Trim(), Convert.ToInt32(ddlFltCity.SelectedValue), ddlFltStatus.SelectedValue);
                }
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridMain(txtFltIgpNo.Text.Trim(), txtFltParty.Text.Trim(), Convert.ToInt32(ddlFltCity.SelectedValue), ddlFltStatus.SelectedValue);
        }
        protected void grdIgp_RowDataBound(object sender, GridViewRowEventArgs e)
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
                }
                else if (e.Row.Cells[4].Text.Equals("C"))
                {
                    e.Row.Cells[4].Text = "Cancelled";
                }
            }
        }
        protected void grdIgp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdIgp.PageIndex = e.NewPageIndex;
            BindGridMain(txtFltIgpNo.Text.Trim(), txtFltParty.Text.Trim(), Convert.ToInt32(ddlFltCity.SelectedValue), ddlFltStatus.SelectedValue);
        }
        protected void btnPoSrch_Click(object sender, EventArgs e)
        {

            int grdRowsCount = GridView1.Rows.Count;
            string vrno = "";
           


            if (!string.IsNullOrEmpty(aceValue.Value))
            {
                if (ddlVendor.SelectedValue != "0")
                {
                    VrId = Convert.ToInt32(aceValue.Value);
                    List<spSrchPOResult> PoDet = GetTblPOrderDet(VrId);
                    int poref = PoDet.First().vr_no;
                    for (int i = 0; i < grdRowsCount; i++)
                    {
                        vrno = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPoref")).Text.Trim();
                        if (vrno != "99999" && vrno != poref.ToString())
                        {
                            ucMessage.ShowMessage("Multiple purchase orders cannot be added in an IGP", RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }



                    if (PoDet.Count() > 0)
                    {
                        UpdateTable1();
                        AddRow1(PoDet);
                        SetDropDownList();
                        ddlVendor.Enabled = false;
                    }
                }
            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {

            int index = 0;
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            index = clickedRow.RowIndex;
            GetGP(
                Convert.ToInt32(grdIgp.DataKeys[index].Values["br_id"]),
                Convert.ToInt32(grdIgp.DataKeys[index].Values["locid"]),
                Convert.ToInt32(grdIgp.DataKeys[index].Values["vt_cd"]),
                Convert.ToInt32(grdIgp.DataKeys[index].Values["vr_no"])
            );
        }

        #endregion

        #region Helping Method

        public void GetGP(int brid, int locid, int vtcd, int vrno)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Visible = false;
            reportViewer.LocalReport.ReportPath = "InvenRpt/rdlc/GP.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.Refresh();

            List<spGPResult> gp = igpBL.GetGPRecs(brid, locid, vtcd, vrno, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ReportDataSource dataSource = new ReportDataSource("spGPResult", gp);

            string rptLogoPath = "";
            string company = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());
            //company = System.Configuration.ConfigurationManager.AppSettings["CompanyName"].ToString().Trim();
            if (Session["CompName"] == null)
            {
                if (Request.Cookies["uzr"]["CompName"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    company = Request.Cookies["uzr"]["CompName"].ToString().ToString();
                }
            }
            else
            {
                company = Session["CompName"].ToString();
            }
            string brName = "", brAddress = "", brTel = "", brFax = "", brNTN = "";
            try
            {
                int branchid = 0;
                if (Session["BranchID"] == null)
                {
                    branchid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
                }
                else
                {
                    branchid = Convert.ToInt32(Session["BranchID"].ToString());
                }

                Branch branch = new voucherHomeBL().GetBranch(branchid, 1, (RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                if (branch != null)
                {
                    brName = branch.br_nme;
                    brAddress = branch.br_address;
                    brTel = branch.br_tel;
                    brFax = branch.br_fax;
                    brNTN = branch.br_ntn;
                }
            }
            catch { }

            ReportParameter[] rpt = new ReportParameter[8];
            rpt[0] = new ReportParameter("ReportName", "Stock IGP");
            rpt[1] = new ReportParameter("LogoPath", rptLogoPath);
            rpt[2] = new ReportParameter("CompanyName", company);

            rpt[3] = new ReportParameter("brName", brName, false);
            rpt[4] = new ReportParameter("brAddress", brAddress, false);
            rpt[5] = new ReportParameter("brTel", brTel, false);
            rpt[6] = new ReportParameter("brFax", brFax, false);
            rpt[7] = new ReportParameter("brNTN", brNTN, false);


            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.SetParameters(rpt);

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(dataSource);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            filename = string.Format("{0}.{1}", "GP", "pdf");
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }
        private void GetByID()
        {

            tblStkGP stkGp = igpBL.GetByID(LocID, BrID, VrNO, VtCD, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            IGPBL.stkGatePass = stkGp;

            ddlLoc.SelectedValue = stkGp.LocId.ToString();
            ddlStatus.SelectedValue = stkGp.vr_apr.ToString();
            if (stkGp.vr_apr.ToString().Equals("P"))
            {
                ucButtons.EnableSave();
            }
            else
            {
                ucButtons.DisableSave();
            }

            if (IsEdit)
            {
                txtGPNo.Text = stkGp.vr_no.ToString().Substring(0, 4) + "/" + stkGp.vr_no.ToString().Substring(4);
            }
            
            
            txtGpDate.Text = stkGp.vr_dt.ToString("dd-MMM-yy");
            //--------
            CalendarExtender1.SelectedDate = stkGp.vr_dt;
            
            ddlVendor.SelectedValue = stkGp.VendorId;
            ddlCity.SelectedValue = stkGp.VendorCity;

            txtVehicleNo.Text = stkGp.VehicleNo;
            txtFrieght.Text = stkGp.Freight.Value.ToString();
            txtBiltyNo.Text = stkGp.BiltyNo;
            txtRemarks.Text = stkGp.vr_nrtn;

            //EntitySet<tblStkGPDet> dets = stk.tblStkGPDets;

            BindTableEdit(stkGp.tblStkGPDets);

        }
        private void ClearFieldsOnly()
        {
            IsEdit = false;
            IGPBL.IsEditable = false;
            IGPBL.stkGatePass = null;

            ddlVendor.Enabled = true;

            LocID = 0;
            BrID = 0;
            VtCD = 0;
            VrNO = 0;
            DocNo = 0;
            VrId = 0;

            DocNoFormated = "";
            txtPoSrch.Text = "";

            txtGPNo.Text = "";
            //ddlLoc.SelectedValue = "5";
            ddlStatus.SelectedValue = "P";
            CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.SelectedIndex = 0;
            ddlCity .SelectedIndex = 0;
            txtVehicleNo.Text = "";
            txtFrieght.Text = "";
            txtBiltyNo.Text = "";
            txtTotalQty.Text = "";
            txtRemarks.Text = "";

            txtIgpRef.Text = "";
            GetGatePassNo();
            GetDocRef();
            BindTable();
        }
        private void ClearFields()
        {
            Response.Redirect("~/inv/igpmgt.aspx?PID=513");
        }
        private bool SaveIGP()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }

            tblStkGP stkGp;
            if (!IsEdit)
            {
                stkGp = new tblStkGP();

                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        stkGp.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    stkGp.br_id = Convert.ToInt32(Session["BranchID"]);
                }
                
                stkGp.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                stkGp.vt_cd = VoucherTypeCode; // voucher type code 12 for Gate Pass

                if (!IsEdit)
                {
                    GetGatePassNo();
                }
                //string no = txtGPNo.Text.Substring(5);
                //string yrno = txtGPNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(no));
                stkGp.vr_no = DocNo;//Convert.ToInt32(yrno);

                stkGp.DocRef = txtDocRef.Text.Trim();
            }
            else
            {
                stkGp = igpBL.GetByID(LocID, BrID, VrNO, VtCD, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }

            try
            {
                stkGp.vr_dt = Convert.ToDateTime(txtGpDate.Text.Trim());
            }
            catch
            {
                ucMessage.ShowMessage("Invalid IGP Date", RMS.BL.Enums.MessageType.Error);
                txtGpDate.Focus();
                return false;
            }
            stkGp.VendorId = ddlVendor.SelectedValue;
            stkGp.VendorCity = ddlCity.SelectedValue;
            stkGp.VehicleNo = txtVehicleNo.Text.Trim();
            stkGp.Freight = txtFrieght.Text.Trim().Equals("") ? 0 : Convert.ToDecimal(txtFrieght.Text.Trim());
            stkGp.BiltyNo = txtBiltyNo.Text.Trim();
            stkGp.vr_nrtn = txtRemarks.Text.Trim(); // remarks
            if (Session["LoginID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    stkGp.updateby = Request.Cookies["uzr"]["LoginID"];
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                stkGp.updateby = Session["LoginID"].ToString();
            }
            
            stkGp.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            stkGp.vr_apr = Convert.ToString(ddlStatus.SelectedValue);
            stkGp.PMN_Ref = "";
            stkGp.PMN_RefTemp = "";


            
            EntitySet<tblStkGPDet> stkGPDets = new EntitySet<tblStkGPDet>();
            tblStkGPDet stkGpdet;
            string poref, itemCode, packs, unitsize, qty, uomqty, gross, rem;//uom,
            bool chkIncluded;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //poref = ""; itemCode = ""; packs = ""; uom = ""; unitsize = ""; qty = ""; uomqty = ""; gross = ""; rem = ""; 
                poref = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPoref")).Text.Trim();
                itemCode = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                packs = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPacks")).Text.Trim();
                //uom = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlUom")).SelectedValue;
                unitsize = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUnitSize")).Text.Trim();
                qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text.Trim();
                uomqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomQty")).Text.Trim();
                gross = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtGrossWt")).Text.Trim();
                rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text.Trim();
                seqno = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSeqNo")).Text.Trim();
                chkIncluded = ((System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("chkIncluded")).Checked;

                if (chkIncluded && !qty.Equals("") && !itemCode.Equals("") && !packs.Equals("")
                        && !unitsize.Equals("") 
                       && !uomqty.Equals(""))//&& !uom.Equals("")
                {
                    stkGpdet = new tblStkGPDet();
                    stkGpdet.br_id = stkGp.br_id;
                    stkGpdet.LocId = stkGp.LocId;
                    stkGpdet.vt_cd = stkGp.vt_cd;
                    stkGpdet.vr_no = stkGp.vr_no;

                    stkGpdet.vr_seq = Convert.ToByte(i + 1);
                    stkGpdet.Itm_cd = itemCode;

                    try
                    {
                        stkGpdet.PO_Ref = Convert.ToInt32(poref);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("PO Ref# is invalid, it should be integer", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }
                    try
                    {
                        stkGpdet.Pkg = Convert.ToDecimal(packs);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Packs is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    //stkGpdet.Pkg_UOM = Convert.ToByte(uom);
                    try
                    {
                        stkGpdet.Pkg_Size = Convert.ToDecimal(unitsize);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Unit Size is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }
                    try
                    {
                        stkGpdet.vr_qty = Convert.ToDecimal(qty);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Quantity is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    stkGpdet.Itm_UOM = igpBL.GetItemUOMFromLabel(uomqty, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    try
                    {
                        if (gross.Equals(""))
                        {
                            stkGpdet.KgsWt = 0;
                        }
                        else
                        {
                            stkGpdet.KgsWt = Convert.ToDecimal(gross);
                        }
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Gross weight is invalid, it should be numeric", RMS.BL.Enums.MessageType.Error);
                        return false;
                    }

                    stkGpdet.Remarks = rem;

                    stkGpdet.GPRef = txtIgpRef.Text.Trim();
                    if (!string.IsNullOrEmpty(seqno))
                    {
                        stkGpdet.PO_Ref_Seq = Convert.ToInt16(seqno);
                    }

                    stkGPDets.Add(stkGpdet);
                }
            }

            if (stkGPDets == null || stkGPDets.Count < 1)
            {
                Response.Redirect("~/login.aspx");
            }

            if (!IsEdit)
            {
                stkGp.tblStkGPDets = stkGPDets;
                string msg = igpBL.SaveIGPFull(stkGp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("IGP No "+DocNoFormated+" "+GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
                    return false;
                }

            }
            else
            {
                string msg = igpBL.UpdIGPFull(stkGp, stkGPDets, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (msg.Equals("Done"))
                {
                    ucMessage.ShowMessage("IGP No " + DocNoFormated + " " + GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
                }
                else
                {
                    ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
                    return false;
                }
            }
            IsEdit = false;
            return true;
        }
        private void UpdateIGP()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
        }
        public void FillDropDownLoc()
        {
            ddlLoc.DataSource = igpBL.GetStockLoc1((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
        }
        public void FillDropDownVendor()
        {
            ddlVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.DataTextField = "gl_dsc";
            ddlVendor.DataValueField = "gl_cd";
            ddlVendor.DataBind();
        }
        public void FillDropDownCity()
        {
            ddlCity.DataSource = gP.GetCity((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityID";
            ddlCity.DataBind();

            ddlFltCity.DataSource = gP.GetCity((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltCity.DataTextField = "CityName";
            ddlFltCity.DataValueField = "CityID";
            ddlFltCity.DataBind();
        }      
        public void GetDocRef()
        {
            txtDocRef.Text = igpBL.GetDocReference(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 12, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public void GetGatePassNo()
        {
            //txtGPNo.Text = igpBL.GetGpNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 12, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //DocNoFormated = igpBL.GetGpNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 12, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNoFormated = igpBL.GetGpNo(Convert.ToDateTime(txtGpDate.Text.Trim()), 12, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
        }
        private void BindGridMain(string igpNo, string party, int cityId, string status)
        {
            if (!igpNo.Equals(""))
            {
                if (igpNo.Contains("/") && igpNo.Length > 5)
                {
                    try
                    {
                        char[] st = igpNo.ToCharArray();
                        if (st[4].ToString().Equals("/"))
                        {
                            igpNo = igpNo.Substring(0, 4) + igpNo.Substring(5);
                        }
                        else
                        {
                            igpNo = "";
                            for (int i = 0; i < st.Length; i++)
                            {
                                if (!st[i].ToString().Equals("/"))
                                {
                                    igpNo = igpNo + st[i];
                                }
                            }
                        }
                    }
                    catch { }

                }
                else if (igpNo.Contains("/"))
                {
                    char[] st = igpNo.ToCharArray();
                    igpNo = "";
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (!st[i].ToString().Equals("/"))
                        {
                            igpNo = igpNo + st[i];
                        }
                    }
                }
                else
                {
                    //nothin
                    //docNo = txtDocNo.Text.Trim();
                }
            }
            grdIgp.DataSource = igpBL.GetAllChemIGPs(igpNo, party, cityId, status, Convert.ToInt32(ddlLoc.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdIgp.DataBind();
        }
        public List<spSrchPOResult> GetTblPOrderDet(int vrid)
        {
            return igpBL.GetPODet1(vrid, ddlVendor.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
     
        #endregion

        #region GridView

        public void AddRow()
        {
            if (CurrentTable != null)
            {
                d_table = CurrentTable;
                for (int i = 0; i < 5; i++)
                {
                    d_Row = d_table.NewRow();
                    d_Row["Sr"] = d_table.Rows.Count + 1;
                    d_Row["PoRef"] = "99999";
                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;
                BindGrid();
            }
        }
        public void AddRow1(List<spSrchPOResult> PoDet)
        {
            if (CurrentTable != null)
            {
                bool found = false;
                d_table = CurrentTable;
                for (int i = 0; i < PoDet.Count(); i++)
                {
                    for (int k = 0; k < GridView1.Rows.Count; k++)
                    {
                        if (PoDet[i].vr_no.ToString() == ((TextBox)GridView1.Rows[k].FindControl("txtPoref")).Text &&
                            PoDet[i].itm_cd.ToString() == ((TextBox)GridView1.Rows[k].FindControl("txtItem")).Text)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        d_Row = d_table.NewRow();
                        d_Row["Sr"] = d_table.Rows.Count + 1;
                        d_Row["PoRef"] = PoDet[i].vr_no;
                        d_Row["Item"] = PoDet[i].itm_cd;
                        d_Row["ItemDesc"] = new ItemCodeBL().GetItemDescByItemCode(PoDet[i].itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        d_Row["UomQty"] = igpBL.GetItemUOMDesc(PoDet[i].vr_uom, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        d_Row["SeqNo"] = PoDet[i].vr_seq;
                        d_Row["Included"] = true;

                        d_table.Rows.Add(d_Row);
                    }
                    found = false;
                }
                CurrentTable = d_table;
                BindGrid();
            }
        }
        public void UpdateTable()
        {
            if (CurrentTable != null)
            {

                GetColumns();
                rowsCount = CurrentTable.Rows.Count;
                for (int i = 0; i < rowsCount; i++)
                {
                    d_Row = d_table.NewRow();
                    srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lblSr")).Text;
                    grdPoRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPoref")).Text;
                    itemCode = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                    itemDesc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItemDesc")).Text;
                    packs = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPacks")).Text;
                    //uom = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlUom")).SelectedValue;
                    unitsize = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUnitSize")).Text;
                    qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text;
                    uomqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomQty")).Text;
                    gross = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtGrossWt")).Text;
                    rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text;
                    seqno = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSeqNo")).Text;

                    d_Row["Sr"] = srNo;
                    d_Row["PoRef"] = grdPoRef;
                    d_Row["Item"] = itemCode;
                    d_Row["ItemDesc"] = itemDesc;
                    d_Row["Packs"] = packs;
                    //d_Row["UOM"] = uom;
                    d_Row["UnitSize"] = unitsize;
                    d_Row["Quantity"] = qty;
                    d_Row["UomQty"] = uomqty;
                    d_Row["GrossWt"] = gross;
                    d_Row["Rem"] = rem;
                    d_Row["SeqNo"] = seqno;
                    d_Row["Included"] = true;//((System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("chkIncluded")).Checked;


                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;//assigning to viewstate datatable
                //BindGrid();
                //SetDropDownList();

            }
        }
        public void UpdateTable1()
        {
            if (CurrentTable != null)
            {

                GetColumns();
                rowsCount = CurrentTable.Rows.Count;
                for (int i = 0; i < rowsCount; i++)
                {
                    d_Row = d_table.NewRow();
                    srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lblSr")).Text;
                    grdPoRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPoref")).Text;
                    itemCode = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItem")).Text;
                    itemDesc = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtItemDesc")).Text;
                    packs = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtPacks")).Text;
                    //uom = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlUom")).SelectedValue;
                    unitsize = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUnitSize")).Text;
                    qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text;
                    uomqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtUomQty")).Text;
                    gross = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtGrossWt")).Text;
                    rem = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text;
                    seqno = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtSeqNo")).Text;

                    if (itemCode != "")//(itemCode != "0" && uom != "0" && packs != "" && unitsize != "" && qty != "")
                    {
                        d_Row["Sr"] = srNo;
                        d_Row["PoRef"] = grdPoRef;
                        d_Row["Item"] = itemCode;
                        d_Row["ItemDesc"] = itemDesc;
                        d_Row["Packs"] = packs;
                        //d_Row["UOM"] = uom;
                        d_Row["UnitSize"] = unitsize;
                        d_Row["Quantity"] = qty;
                        d_Row["UomQty"] = uomqty;
                        d_Row["GrossWt"] = gross;
                        d_Row["Rem"] = rem;
                        d_Row["SeqNo"] = seqno;
                        d_Row["Included"] = true;//((System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("chkIncluded")).Checked;

                        d_table.Rows.Add(d_Row);
                    }
                }
                CurrentTable = d_table;//assigning to viewstate datatable
                BindGrid();
                SetDropDownList();
            }
        }
        public void GetColumns()
        {
            d_table.Columns.Clear();
            d_table.Rows.Clear();

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Sr";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "PoRef";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Item";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "ItemDesc";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Packs";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UOM";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UnitSize";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Quantity";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "UomQty";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "GrossWt";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Rem";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "SeqNo";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Included";
            d_table.Columns.Add(d_Col);

        }
        public void BindTable()
        {
            GetColumns();
            for (int i = 0; i < 5; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;
                d_Row["PoRef"] = "99999";

                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }
        public void BindTableEdit(EntitySet<tblStkGPDet> dets)
        {
            GetColumns();
            int count = 0;
            decimal tot = 0;
            foreach (var dt in dets)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Item"] = dt.Itm_cd;
                d_Row["ItemDesc"] = new ItemCodeBL().GetItemDescByItemCode(dt.Itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                d_Row["PoRef"] = dt.PO_Ref;
                d_Row["Packs"] = dt.Pkg.Value.ToString("F");
                //d_Row["UOM"] = dt.Pkg_UOM;
                d_Row["UnitSize"] = dt.Pkg_Size.Value.ToString("0.000");
                d_Row["Quantity"] = dt.vr_qty.ToString("0.000");
                d_Row["UomQty"] = GetItemUOMLabelFromUOMId(dt.Itm_UOM.Value);
                d_Row["GrossWt"] = dt.KgsWt.Value.ToString("0.000");
                d_Row["Rem"] = dt.Remarks;
                d_Row["SeqNo"] = dt.PO_Ref_Seq;
                d_Row["Included"] = true;

                count++;
                d_table.Rows.Add(d_Row);

                tot = tot + dt.vr_qty;

                txtIgpRef.Text = dt.GPRef;
            }
            txtTotalQty.Text = tot.ToString("0.000");
            CurrentTable = d_table;
            BindGrid();

            SetDropDownList();
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }
        public void SetDropDownList()
        {
            rowsCount = CurrentTable.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                CheckBox chkIncluded = ((System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("chkIncluded"));
                if (i < CurrentTable.Rows.Count)
                {
                    if (CurrentTable.Rows[i]["Included"] != DBNull.Value)
                    {
                        chkIncluded.Checked = false;
                        chkIncluded.Checked = Convert.ToBoolean(CurrentTable.Rows[i]["Included"]);
                    }
                }
            }
        }
        //public void BindddlItemDD(DropDownList ddlItem)
        //{
        //    //ddlItem.DataSource = igpBL.GetAllItem((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlItem.DataSource = new ItemCodeBL().GetAllGeneralItems((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    ddlItem.DataTextField = "itm_dsc";
        //    ddlItem.DataValueField = "itm_cd";
        //    ddlItem.DataBind();
        //}

        #endregion

        #region Web Method

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static object VariantData(string poref, string seqno, string qty)
        {
            IGPBL igpBL = new IGPBL();
            return igpBL.VariantData(poref, seqno, qty,(RMSDataContext)HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]);
        }

        [WebMethod]
        public static object GetItemDetail(string itemid)
        {
            //return new PurchReqBL().wmGetItemDetail(itemid, null);
            int brid = 0;
            if (HttpContext.Current.Session["BranchID"] == null)
            {
                brid = Convert.ToInt32(HttpContext.Current.Request.Cookies["uzr"]["BranchID"]);
            }
            else
            {
                brid = Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            }

            return new ItemCodeBL().GetSearchItems(brid, itemid, "UG", 0, null);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using RMS.BL; 

namespace RMS.Inv
{
    public partial class InvMPNMgt :   BasePage
    {

        #region DataMembers

        MPNBL mpnBl = new MPNBL();
        tblStkGP stkGp = new tblStkGP();
        EntitySet<tblStkDataDet> entStkDet = new EntitySet<tblStkDataDet>();

        DataTable d_tableIGP = new DataTable();
        DataRow d_RowIGP;
        DataColumn d_ColIGP;

        DataTable d_table = new DataTable();
#pragma warning disable CS0169 // The field 'InvMPNMgt.d_Row' is never used
        DataRow d_Row;
#pragma warning restore CS0169 // The field 'InvMPNMgt.d_Row' is never used
        DataColumn d_Col;
        EntitySet<tblStkCostDet> entCostDet = new EntitySet<tblStkCostDet>();

        Glmf_Data glmf;

        int IgpVrCode = 0;
        int MpnVrCode = 0;

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        public int BrID
        {
            get { return (ViewState["BrID"] == null) ? 0 : Convert.ToInt32(ViewState["BrID"]); }
            set { ViewState["BrID"] = value; }
        }

        public int locId
        {
            get { return Convert.ToInt32(ViewState["locId"]); }
            set { ViewState["locId"] = value; }
        }

        public DataTable CurrentTable
        {
            set { ViewState["CurrentTable"] = value; }
            get { return (DataTable)(ViewState["CurrentTable"] ?? null); }
        }

        public DataTable CurrentTablePurchCostSheet
        {
            set { ViewState["CurrentTablePurchCostSheet"] = value; }
            get { return (DataTable)(ViewState["CurrentTablePurchCostSheet"] ?? null); }
        }

        public string doc_RefFirst
        {
            set { ViewState["Doc_Ref"] = value; }
            get { return Convert.ToString(ViewState["Doc_Ref"] ?? ""); }
        }

        public int igpNoFirst
        {
            get { return Convert.ToInt32(ViewState["igpNoFirst"]); }
            set { ViewState["igpNoFirst"] = value; }
        }

        public int mpnNo
        {
            get { return Convert.ToInt32(ViewState["mpnNo"]); }
            set { ViewState["mpnNo"] = value; }
        }

        public string mpnNoFormated
        {
            get { return Convert.ToString(ViewState["mpnNoFormated"]); }
            set { ViewState["mpnNoFormated"] = value; }
        }

        public bool IsEdit
        {
            get { return Convert.ToBoolean(ViewState["IsEdit"]); }
            set { ViewState["IsEdit"] = value; }
        }

        public decimal frt
        {
            get { return Convert.ToDecimal(ViewState["frt"]); }
            set { ViewState["frt"] = value; }
        }

        public decimal WHT
        {
            get { return Convert.ToDecimal(ViewState["WHT"]); }
            set { ViewState["WHT"] = value; }
        }

        public bool saveData
        {
            get { return Convert.ToBoolean(ViewState["saveData"]); }
            set { ViewState["saveData"] = value; }
        }

        public bool editData
        {
            get { return Convert.ToBoolean(ViewState["editData"]); }
            set { ViewState["editData"] = value; }
        }

        public string POType
        {
            set { ViewState["POType"] = value; }
            get { return Convert.ToString(ViewState["POType"] ?? ""); }
        }

        public string FltParty
        {
            set { ViewState["FltParty"] = value; }
            get { return Convert.ToString(ViewState["FltParty"] ?? ""); }
        }

        public DateTime FltFrmDt
        {
            set { ViewState["FltFrmDt"] = value; }
            get { return Convert.ToDateTime(ViewState["FltFrmDt"]); }
        }

        public DateTime FltToDt
        {
            set { ViewState["FltToDt"] = value; }
            get { return Convert.ToDateTime(ViewState["FltToDt"]); }
        }

        public char FltStts
        {
            set { ViewState["FltStts"] = value; }
            get { return Convert.ToChar(ViewState["FltStts"]); }
        }

        //public decimal ChargedCost
        //{
        //    get { return Convert.ToDecimal(ViewState["ChargedCost"] ?? 0); }
        //    set { ViewState["ChargedCost"] = value; }
        //}
        
        //public decimal NotCharged
        //{
        //    get { return Convert.ToDecimal(ViewState["NotCharged"] ?? 0); }
        //    set { ViewState["NotCharged"] = value; }
        //}

        public decimal TotalAmnt
        {
            get { return Convert.ToDecimal(ViewState["TotalAmnt"] ?? 0); }
            set { ViewState["TotalAmnt"] = value; }
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


            IgpVrCode = 12;
            MpnVrCode = 21;
            //locId = 5;
            mpnBl.DiscardFalseRecords(IgpVrCode, locId, 'C', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (Session["BranchID"] == null)
            {
                BrID = Convert.ToByte(Request.Cookies["uzr"]["BranchID"]);
            }
            else
            {
                BrID = Convert.ToByte(Session["BranchID"].ToString());
            }

            if (!IsPostBack)
            {
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender2.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender3.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender4.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender5.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    CalendarExtender7.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                    CalendarExtender2.Format = Session["DateFormat"].ToString();
                    CalendarExtender3.Format = Session["DateFormat"].ToString();
                    CalendarExtender4.Format = Session["DateFormat"].ToString();
                    CalendarExtender5.Format = Session["DateFormat"].ToString();
                    CalendarExtender7.Format = Session["DateFormat"].ToString();
                }

               
                PID = Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 515 || PID == 876)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "MatPurchNtMgtNew").ToString();
                    ClearSession();
                    string dt = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month.ToString() + "-01-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString();
                    CalendarExtender4.SelectedDate = Convert.ToDateTime(dt);
                    txtFltFrmDt.Text = Convert.ToDateTime(dt).
                        ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                    CalendarExtender5.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                    txtFltToDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date.
                        ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                    FltParty = txtFltParty.Text;
                    FltFrmDt = Convert.ToDateTime(txtFltFrmDt.Text);
                    FltToDt = Convert.ToDateTime(txtFltToDt.Text);
                    FltStts = 'M';

                    //lnkCostSht.Attributes.Add("onclick", "window.open('../Inv/PurchCostMgt.aspx?PID=514','Purchase_Cost_Sheet','height=400,width=710,toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=center, directories=no, status=no');return false");
                    BindMPNGrid();
                    FillShowVendor();
                    FillDropDownLoc();
                    FillDropDownCity();
                    FillDdlForwarder();

                    saveData = false;
                    editData = false;
                    
                    //GetMatPurchaseNo();

                    ddlSelectVendor.Items.Clear();
                    BindDdlSelectVendor();

                    pnlFields.Style.Add("Display", "none");
                    btnClear.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = true;

                    txtMPNDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date.ToString();
                    CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;

                    igpNoFirst = 0;
                    doc_RefFirst = "";
                    POType = "F";
                   
                }
                
            }
            BindMPNGrid();
            
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlUOM");
                BindDdlUOM(ddl);
                DropDownList ddlGST = (DropDownList)e.Row.FindControl("ddlGST");
                FillGridDropDownGST(ddlGST);

                if (((TextBox)e.Row.FindControl("txtPoRef")).Text.Trim() != "9999/9")
                {
                    string potyp = new POrderBL().GetPOType(BrID, 
                                       Convert.ToInt32(((TextBox)e.Row.FindControl("txtPoRef")).Text.Trim().Replace("/","")),
                                       (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (potyp == "L")
                    {
                        ((TextBox)e.Row.FindControl("txtAmount")).ReadOnly = true;
                        ((TextBox)e.Row.FindControl("txtAmount")).TabIndex = -1;

                        ((TextBox)e.Row.FindControl("txtCustomDuty")).Enabled = false;
                        ((TextBox)e.Row.FindControl("txtCustomDuty")).TabIndex = -1;
                    }
                    
                    ddlGST.Enabled = false;
                }

                if (Convert.ToDecimal(((TextBox)e.Row.FindControl("txtRecQty")).Text.Trim()) <= 0)
                {
                    ((TextBox)e.Row.FindControl("txtAmount")).ReadOnly = true;
                    ((TextBox)e.Row.FindControl("txtAmount")).TabIndex = -1;
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string txt = "mpnDate";
            try
            {
                Convert.ToDateTime(txtMPNDate.Text.Trim());
                txt = "dueDate";
                Convert.ToDateTime(txtDueDate.Text.Trim());
                txt = "invDate";
                if (txtInvDate.Text.Trim() != "")
                    Convert.ToDateTime(txtInvDate.Text.Trim());
            }
            catch
            {
                if(txt == "mpnDate")
                    uMsg.ShowMessage("Invalid MPN date", RMS.BL.Enums.MessageType.Error);
                else if(txt == "dueDate")
                    uMsg.ShowMessage("Invalid due date", RMS.BL.Enums.MessageType.Error);
                else if(txt == "invDate")
                    uMsg.ShowMessage("Invalid invoice date", RMS.BL.Enums.MessageType.Error);
                else{}
                return;
            }
            saveData = CheckData();
            if (saveData == false)
            {
                uMsg.ShowMessage("Please enter valid amount.", RMS.BL.Enums.MessageType.Error);
            }
            else
            {
                    Save();
                    //ClearSession();
            }
            ddlSelectVendor.Items.Clear();
            BindDdlSelectVendor();

        }
        protected void LnkSelectVendor_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlSelectVendor.SelectedValue))
                {
                    mpnBl.DiscardFalseRecords(IgpVrCode, locId, 'C', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //pnlSrchIGPs.Visible = false;
                    pnlIGPs.Visible = true;
                    string[] vals = ddlSelectVendor.SelectedValue.Split('-');
                    locId = Convert.ToInt32(vals[1].ToString());
                    object obj = mpnBl.GetIGPsOfVendor(vals[0].ToString(), locId, IgpVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (obj != null)
                    {
                        pnlGetIGPs.Visible = true;
                        //locId = Convert.ToInt32(grdIGPs.DataKeys[0].Value);
                        BindGridIGPs(obj);


                        //string igp_Num = grdIGPs.Rows[0].Cells[1].Text.Substring(0, 4) + grdIGPs.Rows[0].Cells[1].Text.Substring(5);
                        //igpNoFirst = Convert.ToInt32(igp_Num);
                        //doc_RefFirst = mpnBl.GetDocRefByIGP(BrID, igpNoFirst, IgpVrCode, locId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        Session["VendorId"] = ddlSelectVendor.SelectedValue;
                    }
                    else
                    {
                        BindGridIGPs(null);
                        uMsg.ShowMessage("No record found.", RMS.BL.Enums.MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                uMsg.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        protected void LnkMergIgps_Click(object sender, EventArgs e)
        {
            try
            {
                bool Checked = false;
                frt =0;
                WHT = 0;
                for (int i = 0; i < grdIGPs.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)grdIGPs.Rows[i].FindControl("cbxSelectIGP");
                    if (cbx.Checked == true)
                    {
                        Checked = true;                     
                    }
                }
                if (Checked == true)
                {
                    lblIGPsDet.Visible = true;
                    divCostSht.Style.Add("Display", "false");
                    div1.Style.Add("Display", "false");
                    pnlGetIGPs.Visible = false;
                    pnlSrchIGPs.Visible = false;
                    pnlFields.Style.Add("Display", "true");
                    pnlFieldsTotal.Visible = true;
                    btnSave.Visible = true;
                    btnClear.Visible = true;
                    string vendorId ="";
                    GetMatPurchaseNo();

                 
                    for (int j = 0; j < grdIGPs.Rows.Count; j++)
                    {
                        CheckBox cbx = (CheckBox)grdIGPs.Rows[j].FindControl("cbxSelectIGP");
                        if (cbx.Checked == true)
                        {
                            if (igpNoFirst == 0)
                            {
                                igpNoFirst = Convert.ToInt32(grdIGPs.Rows[j].Cells[1].Text.Substring(0, 4) + grdIGPs.Rows[j].Cells[1].Text.Substring(5));
                                doc_RefFirst = mpnBl.GetDocRefByIGP(BrID, igpNoFirst, IgpVrCode, locId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            int igp = Convert.ToInt32(grdIGPs.Rows[j].Cells[1].Text.Substring(0, 4) + grdIGPs.Rows[j].Cells[1].Text.Substring(5));
                            tblStkGP record = mpnBl.GetRecord(BrID, igp, IgpVrCode, locId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (record.tblStkGPDets.First().PO_Ref != 99999)
                            {
                                Session["POREF"] = record.tblStkGPDets.First().PO_Ref;

                            }
                            if (record != null)
                            {
                                record.PMN_Ref = doc_RefFirst;
                            }
                            mpnBl.UpdateRecord((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            BindTable();
                            
                            
                            stkGp = mpnBl.GetRecord(BrID, igp, IgpVrCode, locId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            ddlShowVendor.SelectedValue = stkGp.VendorId;
                            vendorId = stkGp.VendorId;
                            ddlShowCity.SelectedValue = stkGp.VendorCity;
                            ddlShowLoc.SelectedValue = stkGp.LocId.ToString();
                            txtIGPDate.Text = stkGp.vr_dt.Date.ToString();
                            CalendarExtender2.SelectedDate = stkGp.vr_dt.Date;
                            txtDueDate.Text = stkGp.vr_dt.AddDays(new CompanyBL().GetInvDueDays((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])).Date.ToString();
                            CalendarExtender3.SelectedDate = stkGp.vr_dt.AddDays(new CompanyBL().GetInvDueDays((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])).Date;
                            frt = frt + Convert.ToDecimal(stkGp.Freight);
                            
                        }
                    }
                    
                    Session["DocRef"] = doc_RefFirst;
                    //Session["VrNo"] = mpnNoFormated;
                    //Session["VrDate"] = txtMPNDate.Text.Trim();
                    Session["VendorId"] = vendorId;
                    Session["MPNStatus"] = "P";

                    /*GET PO TYPE************************************/
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        if (((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim() != "9999/9")
                        {
                            POType = new POrderBL().GetPOType(BrID,
                                  Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim().Replace("/", "")),
                                  (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (POType == "L" || POType == "F")
                            {
                                break;
                            }
                        }
                        else
                        {
                            POType = "L";
                            break;
                        }
                    }
                    txtPOType.Text = POType == "L" ? "Local" : "Foreign";
                    if (POType == "L")
                    {
                        txtFrt.Text = frt.ToString();
                        txtImpFrt.Text = "0";
                        txtImpFrt.Enabled = false;
                        ddlForwarder.Enabled = false;
                        txtFrt.Enabled = true;
                    }
                    else
                    {
                        txtFrt.Text = "0";
                        txtImpFrt.Text = frt.ToString();
                        txtImpFrt.Enabled = true;
                        ddlForwarder.Enabled = true;
                        txtFrt.Enabled = false;
                    }
                    /*END GET PO TYPE********************************/
                }
                else
                {
                    uMsg.ShowMessage("Please, select IGP to prepare MPN.", RMS.BL.Enums.MessageType.Error);
                }
                ddlSelectVendor.Items.Clear();
                BindDdlSelectVendor();
                txtWHT.Text = Math.Round(GetWHT(),0).ToString();
                //BindTablePurchCostSheet();
            }
            catch (Exception ex)
            {
                uMsg.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }
        protected void GrdIGPs_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex+1).ToString();
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, 4) + "/" + e.Row.Cells[1].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString(Session["DateFormat"].ToString());
                }
            }
        }
        protected void grdMPN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = e.Row.Cells[0].Text.Substring(0, 4) + "/" + e.Row.Cells[0].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text).ToString(Session["DateFormat"].ToString());
                }
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Substring(0, 4) + "/" + e.Row.Cells[2].Text.Substring(4);
                if (Session["DateFormat"] == null)
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString(Session["DateFormat"].ToString());
                }
                //if (e.Row.Cells[6].Text == "Approved")
                //{
                //}
                if (e.Row.Cells[6].Text == "Cancelled")
                {
                     e.Row.Cells[7].Text = "";
                }
             }
        }
        protected void grdMPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearSession();
            ClearAllPurchCostSheet();

            if (grdMPN.Rows[grdMPN.SelectedIndex].Cells[6].Text == "Approved")
            {
                pnlFields.Enabled = false;
                pnlIGPs.Enabled = false;
                btnSave.Visible = false;
            }
            else
            {
                pnlFields.Enabled = true;
                pnlIGPs.Enabled = true;
                btnSave.Visible = true;
            }
            editData = true;
            pnlSrchIGPs.Visible = false;
            pnlGetIGPs.Visible = false;
            pnlFields.Style.Add("Display", "true");
            pnlIGPs.Visible = true;
            lblIGPsDet.Visible = true;
            divCostSht.Style.Add("Display", "false");
            div1.Style.Add("Display", "false");
            editData = true;

            locId = Convert.ToInt32(grdMPN.SelectedDataKey.Value);
            mpnNo = Convert.ToInt32(grdMPN.Rows[grdMPN.SelectedIndex].Cells[0].Text.Substring(0, 4) + grdMPN.Rows[grdMPN.SelectedIndex].Cells[0].Text.Substring(5));
            mpnNoFormated = grdMPN.Rows[grdMPN.SelectedIndex].Cells[0].Text.Trim();
            IsEdit = true;

            BindFieldsEdit();
            BindTableEdit();
            //BindTablePurchCostSheet();

            /*GET PO TYPE************************************/
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim() != "9999/9")
                {
                    POType = new POrderBL().GetPOType(BrID,
                          Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim().Replace("/", "")),
                          (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (POType == "L" || POType == "F")
                    {
                        break;
                    }
                }
                else
                {
                    POType = "L";
                    break;
                }
            }
            txtPOType.Text = POType == "L" ? "Local" : "Foreign";
            if (POType == "L")
            {
                txtImpFrt.Enabled = false;
                ddlForwarder.Enabled = false;
            }
            else
            {
                txtImpFrt.Enabled = true;
                ddlForwarder.Enabled = true;
            }
            /*END GET PO TYPE********************************/
        }
        protected void grdMPN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMPN.PageIndex = e.NewPageIndex;
            BindMPNGrid();
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            ClearSession();
            FltParty = txtFltParty.Text;
            FltFrmDt = Convert.ToDateTime(txtFltFrmDt.Text); ;
            FltToDt = Convert.ToDateTime(txtFltToDt.Text) ;
            FltStts = Convert.ToChar(ddlFltType.SelectedValue);
            BindMPNGrid();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            mpnBl.DiscardFalseRecords(IgpVrCode, locId, 'C', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            pnlFields.Style.Add("Display", "none");
            pnlIGPs.Visible = false;
            saveData = false;
            editData = false;
            pnlSrchIGPs.Visible = true;
            ddlSelectVendor.Items.Clear();
            BindDdlSelectVendor();
            ClearSession();
   
            Response.Redirect("~/Inv/InvMPNMgt.aspx?PID=515");
        }
      
        #endregion

        #region PurchaseCostSheet

        protected void lnkPurchCostSheet_Click(object sender, EventArgs e)
        {
            MPE.Show();
        }
        protected void btnSavePurchCostSheet_Click(object sender, EventArgs e)
        {
            //SaveCost();
        }
        protected void popupGrd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CalendarExtender Cal = (CalendarExtender)e.Row.FindControl("C2");
                if (Session["DateFormat"] == null)
                {
                    Cal.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    Cal.Format = Session["DateFormat"].ToString();
                }
            }
        }
        public void GetColumnsPurchCostSheet()
        {
            d_table.Columns.Clear();
            d_table.Rows.Clear();

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "CostId";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "TempId";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Desc";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Date";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "DocRef";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Amount";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Remarks";
            d_table.Columns.Add(d_Col);


        }
        public bool CheckDataPurchCostSheet()
        {
            bool res = false;
            for (int i = 0; i < popupGrd.Rows.Count; i++)
            {
                TextBox txtAmnt = (TextBox)(popupGrd.Rows[i].FindControl("txtAmount"));
                if (txtAmnt.Text != "")
                {
                    if (Convert.ToDecimal(txtAmnt.Text) > 0)
                    {
                        res = true;
                        break;
                    }
                }
            }
            return res;
        }
        //public void BindTablePurchCostSheet()
        //{
        //    GetColumnsPurchCostSheet();
        //    int brid = 0;
        //    if (Session["BranchID"] == null)
        //    {
        //        brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
        //    }
        //    else
        //    {
        //        brid = Convert.ToInt32(Session["BranchID"].ToString());
        //    }
        //    string potyp = mpnBl.GetPOType(brid, Convert.ToInt32(Session["POREF"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    /****************************************************************/
        //    List<tblStkCostDet> lstStk = mpnBl.GetPreviousRec(mpnNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    List<tblCost_TempDet> lst = mpnBl.GetCostIdDet(potyp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


        //    if (lstStk != null && lstStk.Count > 0)
        //    {
        //        foreach (var l in lst)
        //        {
        //            d_Row = d_table.NewRow();
        //            bool found = false;
        //            for (int j = 0; j < lstStk.Count; j++)
        //            {
        //                if (l.CostId == lstStk[j].CostId)
        //                {
        //                    d_Row["CostId"] = l.CostId;
        //                    d_Row["TempId"] = l.TempId;
        //                    d_Row["Desc"] = l.Cost_Desc;
        //                    if (lstStk[j].DocRef_Date != null)
        //                    {
        //                        if (Session["DateFormat"] == null)
        //                        {
        //                            d_Row["Date"] = lstStk[j].DocRef_Date.Value.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
        //                        }
        //                        else
        //                        {
        //                            d_Row["Date"] = lstStk[j].DocRef_Date.Value.ToString(Session["DateFormat"].ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        d_Row["Date"] = "";
        //                    }
        //                    d_Row["DocRef"] = lstStk[j].DocRef;
        //                    d_Row["Amount"] = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(lstStk[j].Paid_Amt.Value)));
        //                    d_Row["Remarks"] = lstStk[j].vr_rmk;
        //                    found = true;
        //                }
        //                else if (found == false)
        //                {
        //                    d_Row["CostId"] = l.CostId;
        //                    d_Row["TempId"] = l.TempId;
        //                    d_Row["Desc"] = l.Cost_Desc;
        //                    d_Row["Date"] = "";
        //                    d_Row["DocRef"] = "";
        //                    d_Row["Amount"] = "";
        //                    d_Row["Remarks"] = "";
        //                }
        //                else
        //                {
        //                }
        //            }
        //            d_table.Rows.Add(d_Row);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var l in lst)
        //        {
        //            d_Row = d_table.NewRow();

        //            d_Row["CostId"] = l.CostId;
        //            d_Row["TempId"] = l.TempId;
        //            d_Row["Desc"] = l.Cost_Desc;
        //            d_Row["Date"] = "";
        //            d_Row["DocRef"] = "";
        //            d_Row["Amount"] = "";
        //            d_Row["Remarks"] = "";

        //            d_table.Rows.Add(d_Row);
        //        }
        //    }
        //    CurrentTablePurchCostSheet = d_table;
        //    BindGridPurchCostSheet();
        //}
        public void BindGridPurchCostSheet()
        {
            popupGrd.DataSource = CurrentTablePurchCostSheet;
            popupGrd.DataBind();
        }
        //public void SaveCost()
        //{
        //    try
        //    {
        //        ChargedCost = 0;
        //        string stts = "";
        //        int count = 1;
        //        //tblStkCost=====================================
        //        tblStkCost cost = new tblStkCost();
        //        cost.vr_id = 0;
        //        cost.vr_dt = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
        //        cost.DocRef = doc_RefFirst;
        //        cost.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
        //        if (Session["LoginID"] == null)
        //        {
        //            if (Request.Cookies["uzr"] != null)
        //            {
        //                cost.updateby = Request.Cookies["uzr"]["LoginID"];
        //            }
        //        }
        //        else
        //        {
        //            cost.updateby = Session["LoginID"].ToString();
        //        }
        //        cost.post_2gl = true;

        //        //tblStkCost_Det===================================

        //        for (int i = 0; i < popupGrd.Rows.Count; i++)
        //        {
        //            TextBox txtAmnt = (TextBox)(popupGrd.Rows[i].FindControl("txtAmount"));
        //            if (txtAmnt.Text != "")
        //            {
        //                if (Convert.ToDecimal(txtAmnt.Text) > 0)
        //                {
        //                    tblStkCostDet costDet = new tblStkCostDet();
        //                    costDet.vr_id = 0;
        //                    costDet.vr_seq = Convert.ToByte(count);

        //                    costDet.CostId = Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtCostId"))).Text);

        //                    costDet.DocRef = ((TextBox)(popupGrd.Rows[i].FindControl("txtDocRef"))).Text;
        //                    try
        //                    {
        //                        if (((TextBox)(popupGrd.Rows[i].FindControl("txtDate"))).Text != "")
        //                        {
        //                            costDet.DocRef_Date = Convert.ToDateTime(((TextBox)(popupGrd.Rows[i].FindControl("txtDate"))).Text);
        //                            costDet.Pay_Date = Convert.ToDateTime(((TextBox)(popupGrd.Rows[i].FindControl("txtDate"))).Text);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        ucMsgCS.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
        //                    }
        //                    costDet.Paid_Amt = Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtAmount"))).Text);
        //                    costDet.Claim_Amt = Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtAmount"))).Text);
        //                    costDet.vr_rmk = ((TextBox)(popupGrd.Rows[i].FindControl("txtRem"))).Text;


        //                    //Getting cost when column 'Charg_Cost' in tblCost_TempDet is 'Y'
        //                    stts = mpnBl.GetChargeCostStts(Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtCostId"))).Text), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //                    if (stts != "-")
        //                    {
        //                        if (stts == "Y")
        //                        {
        //                            ChargedCost = ChargedCost + Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtAmount"))).Text);
        //                        }
        //                        else
        //                        {
        //                            NotCharged = NotCharged + Convert.ToDecimal(((TextBox)(popupGrd.Rows[i].FindControl("txtAmount"))).Text);
        //                        }
        //                    }

        //                    entCostDet.Add(costDet);
        //                    count++;
        //                }
        //            }
        //        }
        //        //To Save or Put In Session=================================
        //        if (entCostDet.Count > 0)
        //        {
        //            Session["StkCost"] = cost;
        //            Session["StkCostDet"] = entCostDet;
        //            Session["ChargedCost"] = ChargedCost;
        //            Session["NotCharged"] = NotCharged;

        //            //ClearAllPurchCostSheet();

        //            //=================================================

        //            //Result = mpnBl.SaveCostSheet(cost, entCostDet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //            //if (Result == true)
        //            //{
        //            //    Result = false;
        //            //    ClearAll();
        //            //    ucMsgCS.ShowMessage("Saved successfully.", RMS.BL.Enums.MessageType.Info);
        //            //}
        //            //else
        //            //{
        //            //    ucMsgCS.ShowMessage("Canot save at this time.", RMS.BL.Enums.MessageType.Error);
        //            //}
        //        }
        //        else
        //        {
        //            ucMsgCS.ShowMessage("Cannot add, internal error occured.", RMS.BL.Enums.MessageType.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ucMsgCS.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
        //    }
        //}
        public void ClearAllPurchCostSheet()
        {
            //ChargedCost = 0;
            //NotCharged = 0;
            
   
            MPE.Dispose();
            CurrentTablePurchCostSheet = null;
            BindGridPurchCostSheet();

        }

        #endregion

        #region Helping Methods

        public void ClearSession()
        {
            Session["VrNo"] = null;
            Session["POREF"] = null;
            Session["VendorId"] = null;
            Session["DocRef"] = null;
            Session["StkCost"] = null;
            Session["StkCostDet"] = null;
            Session["MPNStatus"] = null;
        }
        public void ClearFeilds()
        {
            pnlFields.Style.Add("Display", "none");
            pnlIGPs.Visible = false;
            pnlSrchIGPs.Visible = true;
            lblIGPsDet.Visible = false;
            divCostSht.Style.Add("Display", "none");
            div1.Style.Add("Display", "none");
            btnSave.Visible = false;
            pnlFieldsTotal.Visible = false;

            CurrentTable.Columns.Clear();
            CurrentTable.Rows.Clear();
            BindGrid();
            BindMPNGrid();
            saveData = false;
            editData = false;
            txtCommission.Text = "";
            txtRemarks.Text = "";
            ddlStatus.SelectedValue = "P";
            txtMPNNo.Text = "";

            txtGrAmount.Text = "";
            txtDisc.Text = "";
            txtWHT.Text = "";
            txtFrt.Text = "";
            txtImpFrt.Text = "";
            ddlForwarder.SelectedValue = "0";
            txtImpFrt.Enabled = false;
            ddlForwarder.Enabled = false;
            txtAdv.Text = "";
            txtDue.Text = "";
            txtInvNo.Text = "";
            txtInvDate.Text = "";

            TotalAmnt = 0;
            igpNoFirst = 0;
            mpnNo = 0;

            ClearSession();
            ClearAllPurchCostSheet();
            txtOtrCost.Text = "";
        }
        public void ChangePMNStatus(string docRef)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                int igp = Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtIGP")).Text.Substring(0, 4) + ((TextBox)GridView1.Rows[i].FindControl("txtIGP")).Text.Substring(5));
                tblStkGP record = mpnBl.GetRecord(BrID, igp, IgpVrCode, locId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (record != null)
                {
                    record.PMN_Ref = docRef;
                    record.PMN_RefTemp = docRef;
                }
                mpnBl.UpdateRecord((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
        }
        public void GetMatPurchaseNo()
        {
            //txtMPNNo.Text = mpnBl.GetMPNNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //mpnNoFormated = mpnBl.GetMPNNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            mpnNoFormated = mpnBl.GetMPNNo(Convert.ToDateTime(txtMPNDate.Text.Trim()), MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            mpnNo = Convert.ToInt32(mpnNoFormated.Substring(0, 4) + mpnNoFormated.Substring(5));
        }
        public void BindGridIGPs(object obj)
        {
            grdIGPs.DataSource = obj;
            grdIGPs.DataBind();
        }
        public void GetColumnsIGP()
        {
            d_tableIGP.Columns.Clear();
            d_tableIGP.Rows.Clear();

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "Sr";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "vr_no";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "PO_Ref";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "ItmCd";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "ItmDsc";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "UOM";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.Decimal");
            d_ColIGP.ColumnName = "IGPQty";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.Decimal");
            d_ColIGP.ColumnName = "RecQty";
            d_tableIGP.Columns.Add(d_ColIGP);

           
            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "Date";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "City";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.Decimal");
            d_ColIGP.ColumnName = "Amount";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "GST";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "GSTAmount";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "CustomDuty";
            d_tableIGP.Columns.Add(d_ColIGP);

            d_ColIGP = new DataColumn();
            d_ColIGP.DataType = System.Type.GetType("System.String");
            d_ColIGP.ColumnName = "matrec_id";
            d_tableIGP.Columns.Add(d_ColIGP);

        }
        public void BindTable()
        {
            GetColumnsIGP();
            int counter=1;
            decimal discount = 0;
            List<spChemIgps4MPNResult> lst = mpnBl.GetIGPs(locId, IgpVrCode, doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            foreach (var l in lst)
            {
                d_RowIGP = d_tableIGP.NewRow();
                d_RowIGP["Sr"] = counter++;
                d_RowIGP["vr_no"] = l.igpno;
                d_RowIGP["PO_Ref"] = l.po.Value.ToString().Substring(0,4) + "/" + l.po.Value.ToString().Substring(4);
                d_RowIGP["ItmCd"] = l.itm_cd;
                d_RowIGP["ItmDsc"] = l.itm_dsc;
                d_RowIGP["UOM"] = l.vr_uom;
                d_RowIGP["IGPQty"] = l.igpqty;
                d_RowIGP["RecQty"] = l.recqty;
                d_RowIGP["Date"] = l.vrdate;

                string potyp = "F";
                if (l.po.Value.ToString() != "99999")
                {
                    potyp = new POrderBL().GetPOType(BrID,
                                       Convert.ToInt32(l.po.Value),
                                       (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    discount = discount + new POrderBL().GetPODiscountOnItem(BrID,
                                       Convert.ToInt32(l.po.Value), l.itm_cd,
                                       (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                  
                }

                if (potyp == "L")
                {
                    d_RowIGP["Amount"] = Math.Round(Convert.ToDecimal(l.recqty) * l.vr_rate, 2);
                }
                else
                {
                    d_RowIGP["Amount"] = 0;
                }
                if (l.po.Value != 99999)
                {
                    string gstId = new TaxBL().GetGSTIdByPORefItemCode(BrID, l.po.Value, l.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    decimal gstPercent = new TaxBL().GetGSTPercentByPORefItemCode(BrID, l.po.Value, l.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    d_RowIGP["GST"] = gstId;
                    if (potyp == "L")
                    {
                        d_RowIGP["GSTAmount"] = Math.Round(Convert.ToDecimal(l.recqty) * l.vr_rate * gstPercent / 100, 0);
                    }
                    else
                    {
                        d_RowIGP["GSTAmount"] = 0;
                    }
                }
                else
                {
                    d_RowIGP["GST"] = "0";
                    d_RowIGP["GSTAmount"] = "0";
                }
                d_RowIGP["matrec_id"] = l.matrec_id;
                if (l.cust_duty != null)
                d_RowIGP["CustomDuty"] = l.cust_duty;
                else
                    d_RowIGP["CustomDuty"] = "0";

                d_tableIGP.Rows.Add(d_RowIGP);
            }
            txtDisc.Text = Math.Round(discount,0).ToString();
            CurrentTable = d_tableIGP;
            BindGrid();
            SetDropDownList();
        }
        public decimal GetWHT()
        {
            int brid = 0, poref = 0;
            decimal[,] arr = new decimal[GridView1.Rows.Count, 2];
            decimal amount =0, wht=0, discount =0;
            bool isInserted = false;
            string PO_Type = "F";
            if (Session["BranchID"] == null)
            {
                brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
            }
            else
            {
                brid = Convert.ToInt32(Session["BranchID"].ToString());
            }
            //Initialize
            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    arr[j, k] = 0;
                }
            }

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                isInserted = false;
                poref = Convert.ToInt32(((TextBox)(GridView1.Rows[i].FindControl("txtPoRef"))).Text.Trim().Replace("/",""));
                amount = Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtAmount"))).Text.Trim());
                if (poref != 99999)
                {
                    PO_Type = new POrderBL().GetPOType(brid, poref, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    discount = new POrderBL().GetPODiscountOnItem(brid,
                                       poref, ((TextBox)(GridView1.Rows[i].FindControl("txtItemCode"))).Text.Trim(),
                                       (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    amount = amount - discount;
                    if (PO_Type == "L")
                    {
                        for (int j = 0; j < GridView1.Rows.Count; j++)
                        {
                            if (arr[j, 0] == poref)
                            {
                                isInserted = true;
                                arr[j, 1] = arr[j, 1] + amount ;
                            }
                        }
                        if (!isInserted)
                        {
                            for (int l = 0; l < GridView1.Rows.Count; l++)
                            {
                                if (arr[l, 0] == 0)
                                {
                                    arr[l, 0] = poref;
                                    arr[l, 1] = amount;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            decimal rt = 0;
            for (int m = 0; m < GridView1.Rows.Count; m++)
            {
                if (arr[m, 0] != 0)
                {
                    rt = new TaxBL().GetWHTByPoRef(brid, Convert.ToInt32(arr[m, 0]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //wht = wht + arr[m, 1]/100 * rt;
                    wht = wht + (arr[m, 1] / (1 - rt / 100)) - arr[m, 1];
                }
            }
            return wht;
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }
        public void BindMPNGrid()
        {
            grdMPN.DataSource = mpnBl.GetMPNGrid(FltFrmDt, FltToDt, FltParty, FltStts, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdMPN.DataBind();
        }
        public void BindDdlSelectVendor()
        {
            ddlSelectVendor.DataSource = mpnBl.GetSelectedPurchaseVendor(IgpVrCode, locId,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSelectVendor.DataTextField = "gl_dsc";
            ddlSelectVendor.DataValueField = "vendorId";
            ddlSelectVendor.DataBind();
        }
        public void BindFieldsEdit()
        {
            pnlFieldsTotal.Visible = true;

            List<tblStkData> lst = mpnBl.GetRecordList(mpnNo, MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            igpNoFirst = Convert.ToInt32(lst[0].IGPNo);
            doc_RefFirst = lst[0].DocRef;


            txtMPNNo.Text = mpnNoFormated;
            txtMPNDate.Text = lst[0].vr_dt.Date.ToString();
            CalendarExtender1.SelectedDate = lst[0].vr_dt.Date;
            ddlStatus.SelectedValue = lst[0].vr_apr.ToString();
            txtIGPDate.Text = mpnBl.GetIGPById(Convert.ToInt32(lst[0].IGPNo), IgpVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).vr_dt.Date.ToString();
            CalendarExtender2.SelectedDate = mpnBl.GetIGPById(Convert.ToInt32(lst[0].IGPNo), IgpVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).vr_dt.Date;
            txtDueDate.Text = lst[0].Due_Date.ToString();
            txtWHT.Text = Math.Round( lst[0].Tax.Value,0).ToString();
            CalendarExtender3.SelectedDate = lst[0].Due_Date;
            txtFrt.Text = lst[0].Freight.ToString();
            txtImpFrt.Text = lst[0].ImpFreight.ToString();
            if (!string.IsNullOrEmpty(lst[0].Forwarder))
                ddlForwarder.SelectedValue = lst[0].Forwarder;
            else
                ddlForwarder.SelectedValue = "0";
            txtCommission.Text = lst[0].Commission.ToString();
            txtRemarks.Text = lst[0].vr_nrtn;
            txtInvNo.Text = lst[0].InvNo;
            if (lst[0].InvDate != null)
            {
                if (Session["DateFormat"] == null)
                {
                    txtInvDate.Text = lst[0].InvDate.Value.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    txtInvDate.Text = lst[0].InvDate.Value.ToString(Session["DateFormat"].ToString());
                }
            }
            //For Purchase cost sheet

            Session["DocRef"] = doc_RefFirst;
            Session["VrNo"] = mpnNoFormated;
            //if (Session["DateFormat"] == null)
            //{
            //    Session["VrDate"] = Convert.ToDateTime(mpnNoFormated).ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
            //}
            //else
            //{
            //    Session["VrDate"] = Convert.ToDateTime(txtMPNDate.Text).ToString(Session["DateFormat"].ToString());
            //}
            //Session["VrDate"] = txtMPNDate.Text.Trim();
            tblStkGP gpRec =  mpnBl.GetIGPById(Convert.ToInt32(lst[0].IGPNo), 12, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            Session["VendorId"] = gpRec.VendorId;
            Session["MPNStatus"] = lst[0].vr_apr.ToString();
            
            ddlShowVendor.SelectedValue = gpRec.VendorId;
            ddlShowCity.SelectedValue = gpRec.VendorCity;
            ddlShowLoc.SelectedValue = gpRec.LocId.ToString();
        }
        public void BindTableEdit()
        {
            int count=0;
            int gpcount=0;
            decimal discount = 0;
            GetColumnsIGP();
            List<spGetGpByPmnRefResult> gpLst = mpnBl.GetGPRecords(doc_RefFirst, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblStkData> lst = mpnBl.GetRecordList(mpnNo, MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblStkDataDet> lstDet = mpnBl.GetEditTableList(mpnNo, MpnVrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            Session["POREF"] = lstDet.First().PO_Ref;
            /*GET PO TYPE************************************/

            if (lstDet.First().PO_Ref != 99999)
            {
                POType = new POrderBL().GetPOType(BrID, Convert.ToInt32(lstDet.First().PO_Ref), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            }
            else
            {
                POType = "L";
            }
            txtPOType.Text = POType == "L" ? "Local" : "Foreign";
            /*END GET PO TYPE********************************/
            foreach (var l in lstDet)
            {
                d_RowIGP = d_tableIGP.NewRow();
                d_RowIGP["Sr"] = l.vr_seq;
                d_RowIGP["vr_no"] = gpLst[gpcount].vr_no.ToString().Substring(0, 4) + "/" + gpLst[gpcount].vr_no.ToString().Substring(4);
                d_RowIGP["PO_Ref"] = l.PO_Ref.Value.ToString().Substring(0, 4) + "/" + l.PO_Ref.Value.ToString().Substring(4);
                d_RowIGP["ItmCd"] = l.itm_cd;
                d_RowIGP["UOM"] = l.vr_uom;
                d_RowIGP["ItmDsc"] = mpnBl.GetItemDesc(l.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                d_RowIGP["IGPQty"] = (l.vr_qty);
                d_RowIGP["RecQty"] = (l.vr_qty- l.vr_qty_Rej);
                if (Session["DateFormat"] == null)
                {
                    d_RowIGP["Date"]= lst[count].vr_dt.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                }
                else
                {
                    d_RowIGP["Date"] = lst[count].vr_dt.ToString(Session["DateFormat"].ToString());
                }
                d_RowIGP["Amount"] = Math.Round(l.vr_val + Convert.ToDecimal(l.overall_disc), 2);
                string gstId = "0";
                if (l.TaxID == null)
                    gstId = "0";
                else
                    gstId = l.TaxID;
                if (l.PO_Ref.Value != 99999)
                {
                    decimal gstPercent = new TaxBL().GetGSTPercentByPORefItemCode(BrID, l.PO_Ref.Value, l.itm_cd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    d_RowIGP["GST"] = gstId;
                    //d_RowIGP["GSTAmount"] = Math.Round(l.GSTamt.Value, 0);
                }
                else
                {
                    d_RowIGP["GST"] = gstId;
                    //d_RowIGP["GSTAmount"] = Math.Round(l.GSTamt.Value, 0);
                }
                if (l.cust_duty != null)
                {
                    d_RowIGP["CustomDuty"] = l.cust_duty;
                }
                else
                {
                    d_RowIGP["CustomDuty"] = "0";
                }
                if (gstId == "0")
                {
                    d_RowIGP["GSTAmount"] = "0";
                }
                else
                {
                    if (gstId != "VAR")
                        d_RowIGP["GSTAmount"] = Math.Round((l.vr_val + Convert.ToDecimal(l.overall_disc)) / 100 * new TaxBL().GetPercentValByTaxID(gstId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]), 0);
                    else
                        d_RowIGP["GSTAmount"] = l.GSTamt;
                }
                d_RowIGP["matrec_id"] = l.matrec_id;
                discount = discount + Convert.ToDecimal(l.overall_disc);


                d_tableIGP.Rows.Add(d_RowIGP);
                gpcount++;
            }
            txtDisc.Text = Math.Round(discount,0).ToString();
            CurrentTable = d_tableIGP;
            BindGrid();
            SetDropDownList();
        }
        public void SetDropDownList()
        {
            int rowsCount = CurrentTable.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                DropDownList dduom = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlUOM"));
                DropDownList ddgst = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlGST"));
                if (i < CurrentTable.Rows.Count)
                {
                    if (CurrentTable.Rows[i]["UOM"] != DBNull.Value)
                    {
                        dduom.ClearSelection();
                        dduom.Items.FindByValue(CurrentTable.Rows[i]["UOM"].ToString()).Selected = true;
                    }
                    if (CurrentTable.Rows[i]["GST"] != DBNull.Value)
                    {
                        ddgst.ClearSelection();
                        ddgst.Items.FindByValue(CurrentTable.Rows[i]["GST"].ToString()).Selected = true;
                    }
                }
            }
        }
        public void BindDdlUOM(DropDownList ddlUOM)
        {
            ddlUOM.DataSource = mpnBl.GetUOM((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlUOM.DataTextField = "uom_dsc";
            ddlUOM.DataValueField = "uom_cd";
            ddlUOM.DataBind();
        }
        public bool CheckData()
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                TextBox txtAmt = ((TextBox)GridView1.Rows[i].FindControl("txtAmount"));
                if (((TextBox)GridView1.Rows[i].FindControl("txtAmount")).Enabled && txtAmt.Text != "")
                {
                    if (!((TextBox)GridView1.Rows[i].FindControl("txtAmount")).ReadOnly && Convert.ToInt32(Convert.ToDecimal(txtAmt.Text)) < 1)
                    {
                        saveData = false;
                        break;
                    }
                    else
                    {
                        saveData = true;
                    }
                }
                else
                {
                    if (txtAmt.Text != "")
                    {
                        saveData = false;
                        break;
                    }
                    else
                    {
                        saveData = true;
                     //   break;
                    }
                }
            }
            if (saveData == false)
                return false;
            else
                return true;
        }
        public void FillShowVendor()
        {
            ddlShowVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlShowVendor.DataTextField = "gl_dsc";
            ddlShowVendor.DataValueField = "gl_cd";
            ddlShowVendor.DataBind();
        }
        public void FillDdlForwarder()
        {
            ddlForwarder.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlForwarder.DataTextField = "gl_dsc";
            ddlForwarder.DataValueField = "gl_cd";
            ddlForwarder.DataBind();
        }
        public void FillDropDownCity()
        {
            ddlShowCity.DataSource = mpnBl.GetCity((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlShowCity.DataTextField = "CityName";
            ddlShowCity.DataValueField = "CityID";
            ddlShowCity.DataBind();
        }
        public void FillDropDownLoc()
        {
            ddlShowLoc.DataSource = mpnBl.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlShowLoc.DataValueField = "LocId";
            ddlShowLoc.DataTextField = "LocName";
            ddlShowLoc.DataBind();
        }
        public void FillGridDropDownGST(DropDownList ddlGST)
        {
            ddlGST.DataSource = new TaxBL().GetGSTTaxes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlGST.DataTextField = "TaxDesc";
            ddlGST.DataValueField = "TaxID";
            ddlGST.DataBind();
        }
        private string GetNarration()
        {
            string vrNarr = "";
            StringBuilder builder = new StringBuilder();

            builder.Append("").AppendLine();
            builder.Append("Location: " + ddlShowLoc.SelectedItem.Text).AppendLine();
            builder.Append("Vendor: " + ddlShowVendor.SelectedItem.Text).AppendLine();
            //builder.Append("City: " + ddlShowCity.SelectedItem.Text);


            vrNarr = builder.ToString();
            if (vrNarr.Length > 500)
            {
                vrNarr = vrNarr.Substring(0, 500);
            }
            return vrNarr;
        }
        public string CreateIPV()
        {
            string msg = "";
            if (ddlStatus.SelectedValue.Equals("A"))
            {
                glmf = null;
                int Seq = 0, grpCodeLen = 0;
                grpCodeLen = new InvCode().GetGroupCodeLength((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                string username = "", vrNarr = GetNarration();

                if (Session["LoginID"] == null)
                {
                    username = Request.Cookies["uzr"]["LoginID"];
                }
                else
                {
                    username = Session["LoginID"].ToString();
                }

                if (username.Length > 15)
                {
                    username = username.Substring(0, 14);
                }
                EntitySet<Glmf_Data_Det> enttyGlDet = new EntitySet<Glmf_Data_Det>();
                int voucherTypeId = 6;
                decimal Financialyear = new voucherDetailBL().GetFinancialYearByDate(Convert.ToDateTime(txtMPNDate.Text), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                string source = "INV";
                int voucherno = new voucherDetailBL().GetVoucherNo((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], BrID, voucherTypeId, Financialyear, source);
                Preference pref = new PreferenceBL().GetByID(1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                decimal custduty = 0, localfrt = 0, impfrt = 0;
                try
                {
                    localfrt = Convert.ToDecimal(txtFrt.Text.Trim());
                }
                catch { }
                try
                {
                    impfrt = Convert.ToDecimal(txtImpFrt.Text.Trim());
                }
                catch { }
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    try
                    {
                        string amount = ((TextBox)GridView1.Rows[i].FindControl("txtCustomDuty")).Text.Trim();
                        if (!string.IsNullOrEmpty(amount))
                            custduty = custduty + Convert.ToDecimal(amount);
                    }
                    catch { }
                }
                decimal whtRate = 0, whtamnt = 0, amt = 0, discount = 0, totalDiscount = 0;//CREDIT WHT 
                if (!string.IsNullOrEmpty(txtDisc.Text.Trim()))
                    totalDiscount = Convert.ToDecimal(txtDisc.Text.Trim());
                else
                    totalDiscount = 0;

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    try
                    {
                        amt = 0;
                        amt = Convert.ToDecimal(((TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text.Trim());

                    }
                    catch { }
                    try
                    {
                        whtRate = 0;
                        if (((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim() != "9999/9")
                        {
                            tblPOrder po = new POrderBL().GetPORec(BrID,
                                  Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim().Replace("/", "")),
                                  (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            whtRate = new TaxBL().GetWHTByPoRef(BrID, po.vr_no, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                            if (POType == "L")
                            {
                                discount = new POrderBL().GetPOItemDiscount(BrID,
                                              Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim().Replace("/", "")), ((TextBox)(GridView1.Rows[i].FindControl("txtItemCode"))).Text,
                                              (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                        }
                    }
                    catch { }
                    amt = amt - discount;
                    whtamnt = whtamnt + ((amt / (1 - whtRate / 100)) - amt);
                }
                /***************************************************************************/
                /*  ONE */
                /***************************************************************************/
                Glmf_Data glmfdata = new Glmf_Data();//MASTER ROW
                glmfdata.br_id = BrID;
                glmfdata.Gl_Year = Financialyear;
                glmfdata.vt_cd = Convert.ToInt16(voucherTypeId);
                glmfdata.vr_no = voucherno;
                glmfdata.vr_dt = Convert.ToDateTime(txtMPNDate.Text);
                glmfdata.vr_nrtn = vrNarr;
                glmfdata.vr_apr = "P";
                glmfdata.updateby = username;
                glmfdata.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                glmfdata.approvedby = username;
                glmfdata.approvedon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                glmfdata.source = source;
                glmfdata.Ref_no = txtMPNNo.Text.Replace("/","").Trim();
                /***************************************************************************/
                /*  TWO */
                /***************************************************************************/
                decimal tamount = 0, tdisc = 0, gstamnt = 0, totalamount = 0, tgst = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    try
                    {
                        string gst = ((TextBox)GridView1.Rows[i].FindControl("txtGSTAmount")).Text.Trim();
                        if (!string.IsNullOrEmpty(gst))
                            tgst = tgst + Convert.ToDecimal(gst);
                    }
                    catch { }
                    try
                    {
                        string amount = ((TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text.Trim();
                        if (!string.IsNullOrEmpty(amount))
                            tamount = tamount + Convert.ToDecimal(amount);
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(txtDisc.Text))
                    tdisc = Convert.ToDecimal(txtDisc.Text.Trim());
                gstamnt = tgst - (tdisc * tgst / tamount);
               discount = totalDiscount;
                
           
                totalamount = tamount;
                tamount = tamount + gstamnt - discount;

                if (!string.IsNullOrEmpty(ddlShowVendor.SelectedValue))
                {
                    Glmf_Data_Det glDet1 = new Glmf_Data_Det();
                    Seq = Seq + 1;
                    glDet1.vr_seq = Seq;
                    glDet1.gl_cd = ddlShowVendor.SelectedValue;
                    glDet1.vrd_debit = 0;
                    if (POType == "F")
                        glDet1.vrd_credit = tamount;
                    else if (POType == "L")
                        glDet1.vrd_credit = tamount + localfrt;
                    else
                        glDet1.vrd_credit = 0;
                    glDet1.vrd_nrtn = "";
                    glDet1.cc_cd = null;
                    enttyGlDet.Add(glDet1);
                }
                else
                {
                    msg = msg + "Vendor a/c is missing, Plz select vendor" + "</br>";
                }
                /***************************************************************************/
                /*  THREE */
                /***************************************************************************/
                List<ListItem> lst = new List<ListItem>();//DEBIT AMOUNT
                string itmcd = "", purchaseCd = "";
                decimal tempWHTAmount = 0, customduty = 0;
                bool found;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    discount = 0;
                    itmcd = ((TextBox)GridView1.Rows[i].FindControl("txtItemCode")).Text.Trim();
                    if (((TextBox)GridView1.Rows[i].FindControl("txtCustomDuty")).Text.Trim() != "")
                        customduty = Convert.ToDecimal(((TextBox)GridView1.Rows[i].FindControl("txtCustomDuty")).Text.Trim());
                    else
                        customduty = 0;
                    purchaseCd = new InvCode().GetGroupPurchaseAccount(BrID, itmcd.Substring(0, grpCodeLen), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    if (string.IsNullOrEmpty(purchaseCd))
                    {
                        msg = msg + "Ctrl purchase a/c is missing, Plz update item code master" + "</br>";
                    }
                    try
                    {
                        whtRate = 0;
                        if (((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim() != "9999/9")
                        {
                            tblPOrder po = new POrderBL().GetPORec(BrID,
                                  Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim().Replace("/", "")),
                                  (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            whtRate = new TaxBL().GetWHTByPoRef(BrID, po.vr_no, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (POType == "L")
                            {
                                discount = new POrderBL().GetPOItemDiscount(BrID,
                                          Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim().Replace("/", "")), ((TextBox)(GridView1.Rows[i].FindControl("txtItemCode"))).Text,
                                          (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            else
                            {
                                discount = Math.Round(Convert.ToDecimal(((TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text.Trim()) * totalDiscount / totalamount,2);
                            }
                        }
                    }
                    catch { }
                    if (!string.IsNullOrEmpty(purchaseCd))
                    {
                        decimal tempamount = 0, tempfrt = 0;
                        if (lst.Count > 0)
                        {
                            found = false;
                            foreach (ListItem l in lst)
                            {
                                if (l.Text == purchaseCd)
                                {
                                    tempamount = Convert.ToDecimal(((TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text.Trim());
                                    tempamount = tempamount - discount;
                                    tempWHTAmount = Math.Round(Convert.ToDecimal(POType == "L" ?
                                                       (tempamount / (1 - whtRate / 100)) - tempamount
                                                    : 0),0);
                                    tempfrt = Math.Round((Convert.ToDecimal(txtImpFrt.Text.Trim()) * tempamount / (totalamount-totalDiscount)), 2);
                                    tempfrt = POType == "L" ? 0 : tempfrt;
                                    l.Value = Convert.ToString(Convert.ToDecimal(l.Value) + tempamount + tempWHTAmount + customduty + tempfrt);
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                tempamount = Convert.ToDecimal(((TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text.Trim());
                                tempamount = tempamount - discount;

                                ListItem itm = new ListItem();
                                itm.Text = purchaseCd;
                                tempWHTAmount = Math.Round(Convert.ToDecimal(POType == "L" ?
                                                       (tempamount / (1 - whtRate / 100)) - tempamount
                                                    : 0),0);
                                tempfrt = Math.Round((Convert.ToDecimal(txtImpFrt.Text.Trim()) * tempamount / (totalamount - totalDiscount)), 2);
                                tempfrt = POType == "L" ? 0 : tempfrt;
                                itm.Value = Convert.ToString(tempamount + tempWHTAmount + customduty + tempfrt);
                                lst.Add(itm);
                            }
                        }
                        else
                        {
                            tempamount = Convert.ToDecimal(((TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text.Trim());
                            tempamount = tempamount - discount;

                            ListItem itm = new ListItem();
                            itm.Text = purchaseCd;
                            tempWHTAmount = Math.Round(Convert.ToDecimal(POType == "L" ?
                                                       (tempamount / (1 - whtRate / 100)) - tempamount
                                                    : 0),0);
                            tempfrt = Math.Round((Convert.ToDecimal(txtImpFrt.Text.Trim()) * tempamount / (totalamount - totalDiscount)), 2);
                            tempfrt = POType == "L" ? 0 : tempfrt;
                            itm.Value = Convert.ToString(tempamount + tempWHTAmount + customduty + tempfrt);   
                                   
                            lst.Add(itm);
                        }
                    }
                }
                
                foreach (ListItem l in lst)
                {
                    Glmf_Data_Det glDet1 = new Glmf_Data_Det();
                    Seq = Seq + 1;
                    glDet1.vr_seq = Seq;
                    glDet1.gl_cd = l.Text;
                    glDet1.vrd_debit = Convert.ToDecimal(l.Value) ;
                    glDet1.vrd_credit = 0;
                    glDet1.vrd_nrtn = "";
                    glDet1.cc_cd = null;
                    enttyGlDet.Add(glDet1);
                }
                /***************************************************************************/
                /*  FOUR */
                /***************************************************************************/
                gstamnt = 0;
                tamount = 0; tdisc = 0; tgst = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    try
                    {
                      
                        string gst = ((TextBox)GridView1.Rows[i].FindControl("txtGSTAmount")).Text.Trim();
                        if (!string.IsNullOrEmpty(gst))
                            tgst = tgst + Convert.ToDecimal(gst);
                        }
                    catch { }
                    try
                    {
                        string amount = ((TextBox)GridView1.Rows[i].FindControl("txtAmount")).Text.Trim();
                        if (!string.IsNullOrEmpty(amount))
                            tamount = tamount + Convert.ToDecimal(amount);
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(txtDisc.Text))
                    tdisc = Convert.ToDecimal(txtDisc.Text.Trim());
                gstamnt = tgst - (tdisc * tgst / tamount);
                if (gstamnt > 0)
                {
                    if (!string.IsNullOrEmpty(pref.InvCtrl_GST))//DEBIT GST
                    {
                        Glmf_Data_Det glDet1 = new Glmf_Data_Det();
                        Seq = Seq + 1;
                        glDet1.vr_seq = Seq;
                        glDet1.gl_cd = pref.InvCtrl_GST;
                        glDet1.vrd_debit = gstamnt;
                        glDet1.vrd_credit = 0;
                        glDet1.vrd_nrtn = "";
                        glDet1.cc_cd = null;
                        enttyGlDet.Add(glDet1);
                    }
                    else
                    {
                        msg = msg + "Ctrl GST a/c is missing, Plz update preferences" + "</br>";
                    }
                }
                /***************************************************************************/
                /*  FIVE */
                /***************************************************************************/
                if (POType == "L" && whtamnt > 0)
                {
                    if (!string.IsNullOrEmpty(pref.InvCtrl_WHT))
                    {
                        Glmf_Data_Det glDet1 = new Glmf_Data_Det();
                        Seq = Seq + 1;
                        glDet1.vr_seq = Seq;
                        glDet1.gl_cd = pref.InvCtrl_WHT;
                        glDet1.vrd_debit = 0;
                        glDet1.vrd_credit = Math.Round(whtamnt,0);
                        glDet1.vrd_nrtn = "";
                        glDet1.cc_cd = null;
                        enttyGlDet.Add(glDet1);
                    }
                    else
                    {
                        msg = msg + "Ctrl WHT a/c is missing, Plz update preferences" + "</br>";
                    }
                }
                /***************************************************************************/
                /*  SIX */
                /***************************************************************************/
                custduty = 0;//CREDIT CUSTOM DUTY
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    try
                    {
                        string amount = ((TextBox)GridView1.Rows[i].FindControl("txtCustomDuty")).Text.Trim();
                        if (!string.IsNullOrEmpty(amount))
                            custduty = custduty + Convert.ToDecimal(amount);
                    }
                    catch { }
                }
                if (custduty > 0)
                {
                    if (!string.IsNullOrEmpty(pref.InvCtrl_CustDuty))
                    {
                        Glmf_Data_Det glDet1 = new Glmf_Data_Det();
                        Seq = Seq + 1;
                        glDet1.vr_seq = Seq;
                        glDet1.gl_cd = pref.InvCtrl_CustDuty;
                        glDet1.vrd_debit = 0;
                        glDet1.vrd_credit = custduty;
                        glDet1.vrd_nrtn = "";
                        glDet1.cc_cd = null;
                        enttyGlDet.Add(glDet1);
                    }
                    else
                    {
                        msg = msg + "Ctrl custom duty a/c is missing, Plz update preferences" + "</br>";
                    }
                }
                /***************************************************************************/
                /*  SEVEN */
                /***************************************************************************/
                if (POType == "F" && !string.IsNullOrEmpty(txtImpFrt.Text.Trim()) && Convert.ToDecimal(txtImpFrt.Text.Trim()) > 0)
                {
                    if (!string.IsNullOrEmpty(pref.InvCtrl_ImpFreight))//CREDIT IMPORTED FREIGHT
                    {
                        Glmf_Data_Det glDet1 = new Glmf_Data_Det();
                        Seq = Seq + 1;
                        glDet1.vr_seq = Seq;
                        if (ddlForwarder.SelectedValue != "0")
                            glDet1.gl_cd = ddlForwarder.SelectedValue;
                        else
                            glDet1.gl_cd = pref.InvCtrl_ImpFreight;
                        glDet1.vrd_debit = 0;
                        glDet1.vrd_credit = Convert.ToDecimal(txtImpFrt.Text.Trim());
                        glDet1.vrd_nrtn = "";
                        glDet1.cc_cd = null;
                        enttyGlDet.Add(glDet1);
                    }
                    else
                    {
                        msg = msg + "Ctrl imported freight a/c is missing, Plz update preferences" + "</br>";
                    }
                }
                /***************************************************************************/
                /*  EIGHT */
                /***************************************************************************/
                if (POType =="L" && !string.IsNullOrEmpty(txtFrt.Text.Trim()) && Convert.ToDecimal(txtFrt.Text.Trim()) > 0)
                {
                    if (!string.IsNullOrEmpty(pref.InvCtrl_Freight))//DEBIT FREIGHT
                    {
                        Glmf_Data_Det glDet1 = new Glmf_Data_Det();
                        Seq = Seq + 1;
                        glDet1.vr_seq = Seq;
                        glDet1.gl_cd = pref.InvCtrl_Freight;
                        glDet1.vrd_debit = Convert.ToDecimal(txtFrt.Text.Trim());
                        glDet1.vrd_credit = 0;
                        glDet1.vrd_nrtn = "";
                        glDet1.cc_cd = null;
                        enttyGlDet.Add(glDet1);
                    }
                    else
                    {
                        msg = msg + "Ctrl freight a/c is missing, Plz update preferences" + "</br>";
                    }
                }
                /***************************************************************************/
                /*  NINE */
                /***************************************************************************/
                //if (!string.IsNullOrEmpty(txtOtrCost.Text.Trim()) && Convert.ToDecimal(txtOtrCost.Text.Trim()) > 0)
                //{
                //    if (!string.IsNullOrEmpty(pref.InvCtrl_OtrCost))//DEBIT OTHER COST
                //    {
                //        Glmf_Data_Det glDet1 = new Glmf_Data_Det();
                //        Seq = Seq + 1;
                //        glDet1.vr_seq = Seq;
                //        glDet1.gl_cd = pref.InvCtrl_OtrCost;
                //        glDet1.vrd_debit = Convert.ToDecimal(txtOtrCost.Text.Trim());
                //        glDet1.vrd_credit = 0;
                //        glDet1.vrd_nrtn = "";
                //        glDet1.cc_cd = null;
                //        enttyGlDet.Add(glDet1);
                //    }
                //    else
                //    {
                //        msg = msg + "Ctrl other cost a/c is missing, Plz update preferences";
                //    }
                //}
                /***************************************************************************/
                /* END */
                /***************************************************************************/
                if (msg == "")
                {
                    decimal debit = 0, credit = 0;
                    foreach (var obj in enttyGlDet)
                    {
                        debit = debit + obj.vrd_debit;
                        credit = credit + obj.vrd_credit;
                    }
                    if (debit != credit)
                    {
                        if (debit > credit && ((debit - credit) <= 10))
                        {
                            enttyGlDet[1].vrd_debit = enttyGlDet[1].vrd_debit - (debit - credit);
                        }
                        else if (credit > debit && ((credit - debit) <= 10))
                        {
                            enttyGlDet[1].vrd_debit = enttyGlDet[1].vrd_debit + (credit - debit);
                        }
                    }
                    glmf = new Glmf_Data();
                    glmf = glmfdata;
                    glmf.Glmf_Data_Dets = enttyGlDet;
                }
            }
            return msg;
        }
        public void Save()
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            try
            {
                if (!IsEdit)
                {
                    GetMatPurchaseNo();
                }
                ////=======================================================
                //ChargedCost = 0;
                //if (Session["ChargedCost"] != null)
                //{
                //    ChargedCost = Convert.ToDecimal(Session["ChargedCost"]);
                //    NotCharged = Convert.ToDecimal(Session["NotCharged"]);
                //}
                //else
                //{
                //    int vr_Id = mpnBl.GetVrIDByIGP(igpNoFirst, mpnNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //    if (editData == true)
                //    {
                //        ChargedCost = mpnBl.GetChargedCost(vr_Id, "Y", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //        NotCharged = mpnBl.GetChargedCost(vr_Id, "N", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //    }
                //}
                ////=======================================================
                decimal customDutyAmnt = 0, freightAmnt = 0, totalVal = 0, discount = 0, totalDiscount = 0;// totalQty = 0;
                for (int j = 0; j < GridView1.Rows.Count; j++)
                {
                    TotalAmnt = TotalAmnt + Convert.ToDecimal(((TextBox)(GridView1.Rows[j].FindControl("txtAmount"))).Text);
                    try
                    {
                        string amount = ((TextBox)GridView1.Rows[j].FindControl("txtCustomDuty")).Text.Trim();
                        if (!string.IsNullOrEmpty(amount))
                            customDutyAmnt = customDutyAmnt + Convert.ToDecimal(amount);
                    }
                    catch { }
                    try
                    {
                        string value = ((TextBox)GridView1.Rows[j].FindControl("txtAmount")).Text.Trim();
                        if (!string.IsNullOrEmpty(value))
                            totalVal = totalVal + Convert.ToDecimal(value);
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(txtDisc.Text.Trim()))
                    totalDiscount = Convert.ToDecimal(txtDisc.Text.Trim());
                else
                    totalDiscount = 0;
                TotalAmnt = TotalAmnt - discount;

                if (txtImpFrt.Text.Trim() != "")
                    freightAmnt = Convert.ToDecimal(txtImpFrt.Text);
                else
                    freightAmnt = 0;
                //tblStkData==========================================

                tblStkData stkData = new tblStkData();
                tblStkDataDet stkDet = null;

                if (Session["BranchID"] == null)
                    stkData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    stkData.br_id = Convert.ToInt32(Session["BranchID"]);

                stkData.LocId = Convert.ToInt16(locId);

                stkData.vr_no = mpnNo;
                //stkData.vr_no = Convert.ToInt32(txtMPNNo.Text.Substring(0, 4) + txtMPNNo.Text.Substring(5));
                //mpnNo = Convert.ToInt32(txtMPNNo.Text.Substring(0, 4) + txtMPNNo.Text.Substring(5));

                stkData.vt_cd = Convert.ToInt16(MpnVrCode);
                stkData.vr_dt = Convert.ToDateTime(txtMPNDate.Text);
                stkData.vr_nrtn = txtRemarks.Text;
                stkData.gl_cd = "0";
                stkData.post_2gl = true;
                stkData.ToLocId = 0;
                if (txtFrt.Text.Trim() != "")
                    stkData.Freight = Convert.ToDecimal(txtFrt.Text);
                else
                    stkData.Freight = 0;
                if (txtWHT.Text.Trim() != "")
                    stkData.Tax = Convert.ToDecimal(txtWHT.Text.Trim());
                else
                    stkData.Tax = 0;
                if (txtCommission.Text != "")
                    stkData.Commission = Convert.ToDecimal(txtCommission.Text);
                else
                    stkData.Commission = 0;
                stkData.vr_apr = Convert.ToString(ddlStatus.SelectedValue);
                stkData.DocRef = doc_RefFirst;
                stkData.IGPNo = igpNoFirst;
                stkData.Due_Date = Convert.ToDateTime(txtDueDate.Text);
                stkData.DeptId = 0;
                if (Session["LoginID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        stkData.updateby = Request.Cookies["uzr"]["LoginID"];
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    stkData.updateby = Session["LoginID"].ToString();
                }
                stkData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                stkData.Pay_Status = "";
                stkData.Status = "";
                stkData.InvNo = txtInvNo.Text;
                if (txtInvDate.Text.Trim() != "")
                    stkData.InvDate = Convert.ToDateTime(txtInvDate.Text.Trim());
                else
                    stkData.InvDate = null;
                if (!string.IsNullOrEmpty(txtImpFrt.Text))
                    stkData.ImpFreight = Convert.ToDecimal(txtImpFrt.Text.Trim());
                else
                    stkData.ImpFreight = 0;
                if (ddlForwarder.SelectedValue != "0")
                    stkData.Forwarder = ddlForwarder.SelectedValue;
                else
                    stkData.Forwarder = null;
                //tblStkDataDet==========================================

                if (igpNoFirst != 0)
                {

                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        stkDet = new tblStkDataDet();

                        stkDet.vr_seq = Convert.ToByte(i + 1);
                        stkDet.itm_cd = ((TextBox)(GridView1.Rows[i].FindControl("txtItemCode"))).Text;
                        stkDet.vr_qty = Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtIgpQty"))).Text);
                        stkDet.vr_qty_Rej = Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtIgpQty"))).Text) -
                                            Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtRecQty"))).Text);
                        stkDet.vr_uom = Convert.ToByte(((DropDownList)GridView1.Rows[i].FindControl("ddlUOM")).SelectedValue);
                        stkDet.PO_Ref = Convert.ToInt32(((TextBox)(GridView1.Rows[i].FindControl("txtPoRef"))).Text.Replace("/", ""));
                         decimal val = Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtAmount"))).Text);
                        decimal whtRate = 0;
                        try
                        {
                            if (((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim() != "9999/9")
                            {
                                tblPOrder po = new POrderBL().GetPORec(BrID,
                                      Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim().Replace("/", "")),
                                      (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                whtRate = new TaxBL().GetWHTByPoRef(BrID, po.vr_no, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                if (POType == "L")
                                {
                                    discount = new POrderBL().GetPOItemDiscount(BrID,
                                          Convert.ToInt32(((TextBox)GridView1.Rows[i].FindControl("txtPoRef")).Text.Trim().Replace("/", "")), stkDet.itm_cd,
                                          (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                }
                                else
                                {
                                    discount = totalDiscount * val / totalVal;
                                }
                            }
                        }
                        catch { }
                        /*VALUE CALCULATIONS##########################################*/
                       
                        val = val - discount;//subtracting discount from amount
                        decimal qty = stkDet.vr_qty - stkDet.vr_qty_Rej;

                        stkDet.vr_val = Math.Round(Convert.ToDecimal(val),2);
                        stkDet.Mat_val = 0;// TotalAmnt != 0 ? Math.Round(Convert.ToDecimal(val) * ChargedCost / TotalAmnt, 2) : 0;
                        stkDet.Otr_val = 0;//TotalAmnt != 0 ? Math.Round(Convert.ToDecimal(val) * NotCharged / TotalAmnt, 2) : 0;
                        //stkDet.WHT_Amnt = (POType == "L" ? Convert.ToDecimal(val * whtRate / 100) : 0);
                        stkDet.WHT_Amnt = (POType == "L" ? Math.Round(Convert.ToDecimal(((val /(1 - whtRate / 100)) - val)),2) : 0);
                        stkDet.overall_disc = Math.Round(discount,2);
                        if (((DropDownList)(GridView1.Rows[i].FindControl("ddlGST"))).SelectedValue == "0")
                            stkDet.TaxID = null;
                        else
                            stkDet.TaxID = ((DropDownList)(GridView1.Rows[i].FindControl("ddlGST"))).SelectedValue;
                        stkDet.GSTamt = Math.Round(Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtGSTAmount"))).Text.Trim()),2);
                        if (stkDet.TaxID != null)
                            stkDet.NetGST = Math.Round(Convert.ToDecimal(val) *
                                new TaxBL().GetPercentValByTaxID(stkDet.TaxID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) / 100, 2);
                        else
                            stkDet.NetGST = 0;
                        /*END OF VALUE CALCULATIONS###################################*/
                        if (POType == "F")
                        {
                            stkDet.freight = Math.Round((freightAmnt * val / (totalVal-totalDiscount)),2);
                        }
                        else
                            stkDet.freight = 0;

                        stkDet.vr_pkg = 0;
                        stkDet.vr_pkg_uom = Convert.ToByte(0);
                        stkDet.vr_pkg_Size = 0;
                        stkDet.CC_cd = "";
                        stkDet.vr_rmk = "";
                        
                        stkDet.cust_duty = Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtCustomDuty"))).Text.Trim());
                        stkDet.matrec_id = Convert.ToInt32(((TextBox)(GridView1.Rows[i].FindControl("txtMatRec_Id"))).Text.Trim());
                        entStkDet.Add(stkDet);

                    }
                }

                if (entStkDet == null || entStkDet.Count < 1)
                {
                    Response.Redirect("~/login.aspx");
                }

                //int prvVrId = 0;
                bool res = false;
                if (entStkDet.Count > 0)
                {
                    if (ddlStatus.SelectedValue == "A")
                    {
                        string msg = CreateIPV();
                        if (msg != "")
                        {
                            uMsg.ShowMessage("Exception: " + msg, RMS.BL.Enums.MessageType.Error);
                            return;
                        }
                    }
                    if (editData == false)
                    {
                        res = mpnBl.SaveMPN(stkData, entStkDet, glmf, ddlShowVendor.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                    else
                    {
                        //prvVrId = mpnBl.GetVrIDByIGP(igpNoFirst, mpnNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        res = mpnBl.EditMPN(BrID, mpnNo, MpnVrCode, stkData, entStkDet, glmf, ddlShowVendor.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                    if (res == true)
                    {
                        glmf = null;//Clearing Glmf records
                       
                        ////&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                        //Changing tblStkGp(PMN_Ref, PMN_RefTemp)
                        if (ddlStatus.SelectedValue != "C")
                        {
                            ChangePMNStatus(doc_RefFirst);
                        }
                        else
                        {
                            ChangePMNStatus("");
                        }
                        ////&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                        //Posting to tblStk

                        if (ddlStatus.SelectedValue == "A")
                        {
                            string glYear = mpnBl.GetGlYear((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            string itmCd = "";
                            
                            for (int k = 0; k < GridView1.Rows.Count; k++)
                            {
                                if (((TextBox)GridView1.Rows[k].FindControl("txtPoRef")).Text.Trim() == "9999/9" || POType == "F")
                                {
                                    itmCd = ((TextBox)GridView1.Rows[k].FindControl("txtItemCode")).Text;

                                    try
                                    {
                                        if (POType == "L")
                                        {
                                            discount = new POrderBL().GetPOItemDiscount(BrID,
                                             Convert.ToInt32(((TextBox)GridView1.Rows[k].FindControl("txtPoRef")).Text.Trim().Replace("/", "")), itmCd,
                                             (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                        }
                                        else
                                        {
                                           discount =  Convert.ToDecimal(((TextBox)(GridView1.Rows[k].FindControl("txtAmount"))).Text) * totalDiscount / totalVal;
                                        }
                                    }
                                    catch 
                                    {
                                        discount = 0;
                                    }


                                    
                                    tblStk record = mpnBl.GetRecByItemCode(BrID, itmCd, Convert.ToDecimal(glYear), locId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                                    if (record == null)//Inserting
                                    {
                                        tblStk stk = new tblStk();
                                        if (Session["BranchID"] == null)
                                            stk.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                                        else
                                            stk.br_id = Convert.ToInt32(Session["BranchID"]);
                                        stk.LocId = Convert.ToInt16(locId);
                                        //stk.Gl_Year = Convert.ToDecimal(glYear);
                                        stk.itm_cd = itmCd;
                                        //stk.uom_cd = Convert.ToByte(((DropDownList)GridView1.Rows[k].FindControl("ddlUOM")).SelectedValue);

                                        stk.itm_pur_qty = stk.itm_pur_qty + Convert.ToDecimal(((TextBox)GridView1.Rows[k].FindControl("txtRecQty")).Text);
                                        //stk.itm_pur_val = stk.itm_pur_val + Convert.ToDecimal(((TextBox)GridView1.Rows[k].FindControl("txtAmount")).Text);
                                        decimal whtRate = 0;
                                        try
                                        {
                                            if (((TextBox)GridView1.Rows[k].FindControl("txtPoRef")).Text.Trim() != "9999/9")
                                            {
                                                tblPOrder po = new POrderBL().GetPORec(BrID,
                                                      Convert.ToInt32(((TextBox)GridView1.Rows[k].FindControl("txtPoRef")).Text.Trim().Replace("/", "")),
                                                      (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                                whtRate = new TaxBL().GetWHTByPoRef(BrID, po.vr_no, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                            }
                                        }
                                        catch { }
                                        /*VALUE CLACULATIONS##########################################*/
                                        decimal cduty = ((TextBox)(GridView1.Rows[k].FindControl("txtCustomDuty"))).Text == "" ? 0 : Convert.ToDecimal(((TextBox)(GridView1.Rows[k].FindControl("txtCustomDuty"))).Text);
                                        decimal qty = Convert.ToDecimal(((TextBox)GridView1.Rows[k].FindControl("txtRecQty")).Text);
                                        decimal tempamount = Convert.ToDecimal(((TextBox)(GridView1.Rows[k].FindControl("txtAmount"))).Text);
                                        tempamount = tempamount - discount;
                                        stk.itm_pur_val = TotalAmnt != 0 ?
                                                                Math.Round(stk.itm_pur_val
                                                                    + tempamount
                                                                    //+ tempamount * ChargedCost / TotalAmnt
                                                                    //+ tempamount * NotCharged / TotalAmnt
                                                                    + cduty
                                                                    + (POType == "F" ? tempamount * freightAmnt / (totalVal-totalDiscount) : 0)
                                                                    + (POType == "L" ? Math.Round((tempamount / (1 - whtRate / 100)) - tempamount, 2) : 0)
                                                                  
                                                                , 2)
                                                            : 0;
                                        /*END OF VALUE CALCULATIONS###################################*/

                                        stk.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                                        if (Session["LoginID"] == null)
                                        {
                                            if (Request.Cookies["uzr"] != null)
                                            {
                                                stk.updateby = Request.Cookies["uzr"]["LoginID"];
                                            }
                                        }
                                        else
                                        {
                                            stk.updateby = Session["LoginID"].ToString();
                                        }
                                        mpnBl.PostToTblStkInsert(stk, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    }
                                    else//Updating
                                    {
                                        decimal whtRate = 0;
                                        try
                                        {
                                            if (((TextBox)GridView1.Rows[k].FindControl("txtPoRef")).Text.Trim() != "9999/9")
                                            {
                                                tblPOrder po = new POrderBL().GetPORec(BrID,
                                                      Convert.ToInt32(((TextBox)GridView1.Rows[k].FindControl("txtPoRef")).Text.Trim().Replace("/", "")),
                                                      (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                                whtRate = new TaxBL().GetWHTByPoRef(BrID, po.vr_no, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                            }
                                        }
                                        catch { }
                                        record.itm_pur_qty = record.itm_pur_qty + Convert.ToDecimal(((TextBox)GridView1.Rows[k].FindControl("txtRecQty")).Text);
                                       
                                        /*VALUE CLACULATIONS##########################################*/
                                        decimal cduty = ((TextBox)(GridView1.Rows[k].FindControl("txtCustomDuty"))).Text == "" ? 0 : Convert.ToDecimal(((TextBox)(GridView1.Rows[k].FindControl("txtCustomDuty"))).Text);
                                        decimal qty = Convert.ToDecimal(((TextBox)GridView1.Rows[k].FindControl("txtRecQty")).Text);
                                        decimal tempamount = Convert.ToDecimal(((TextBox)(GridView1.Rows[k].FindControl("txtAmount"))).Text);
                                        tempamount = tempamount - discount;
                                        record.itm_pur_val = TotalAmnt != 0 ?
                                                                Math.Round(record.itm_pur_val
                                                                    + tempamount
                                                                    //+ tempamount * ChargedCost / TotalAmnt
                                                                    //+ tempamount * NotCharged / TotalAmnt
                                                                    + cduty
                                                                    + (POType == "F" ? tempamount * freightAmnt / (totalVal - totalDiscount) : 0)
                                                                    + (POType == "L" ? Math.Round((tempamount /(1- whtRate / 100))- tempamount,2) : 0)
                                                                  
                                                                , 2)
                                                            : 0;
                                        /*END OF VALUE CALCULATIONS###################################*/

                                        record.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                                        if (Session["LoginID"] == null)
                                        {
                                            if (Request.Cookies["uzr"] != null)
                                            {
                                                record.updateby = Request.Cookies["uzr"]["LoginID"];
                                            }
                                        }
                                        else
                                        {
                                            record.updateby = Session["LoginID"].ToString();
                                        }
                                        mpnBl.PostToTblStkUpdate(record, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                    }
                                }
                            }
                        }


                        //////&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                        ////Saving Purchase Cost Sheet

                        //int vrId = mpnBl.GetVrIDByIGP(igpNoFirst, mpnNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //if (ddlStatus.SelectedValue != "C")
                        //{

                        //    if (Session["StkCost"] != null && vrId > 0)
                        //    {
                        //        tblStkCost cost = (tblStkCost)Session["StkCost"];
                        //        EntitySet<tblStkCostDet> costDetUpd = new EntitySet<tblStkCostDet>();
                        //        EntitySet<tblStkCostDet> costDet = (EntitySet<tblStkCostDet>)Session["StkCostDet"];

                        //        //tblStkCost===================================
                        //        cost.vr_id = vrId;
                        //        cost.vr_dt = Convert.ToDateTime(txtMPNDate.Text);
                        //        if (ddlStatus.SelectedValue != "P")
                        //        {
                        //            if (Session["LoginID"] == null)
                        //            {
                        //                if (Request.Cookies["uzr"] != null)
                        //                {
                        //                    cost.updateby = Request.Cookies["uzr"]["LoginID"];
                        //                }
                        //            }
                        //            else
                        //            {
                        //                cost.updateby = Session["LoginID"].ToString();
                        //            }
                        //        }
                        //        cost.vr_apr = Convert.ToChar(ddlStatus.SelectedValue);
                        //        //tblStkCostDet====================================

                        //        foreach (tblStkCostDet rec in costDet)
                        //        {
                        //            tblStkCostDet recUpd = new tblStkCostDet();
                        //            recUpd.vr_id = vrId;
                        //            recUpd.vr_seq = rec.vr_seq;
                        //            recUpd.CostId = rec.CostId;
                        //            recUpd.DocRef = rec.DocRef;
                        //            recUpd.DocRef_Date = rec.DocRef_Date;
                        //            recUpd.Claim_Amt = rec.Claim_Amt;
                        //            recUpd.Paid_Amt = rec.Paid_Amt;
                        //            recUpd.Pay_Date = rec.Pay_Date;
                        //            recUpd.vr_rmk = rec.vr_rmk;

                        //            costDetUpd.Add(recUpd);
                        //        }

                        //        mpnBl.SaveCostSheet(prvVrId, cost, costDetUpd, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //    }
                        //    else
                        //    {
                        //        mpnBl.UpdateCostSheet(vrId, prvVrId, Convert.ToChar(ddlStatus.SelectedValue), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //    }
                        //}
                        //else
                        //{
                        //    mpnBl.UpdateStatus(prvVrId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //}
                        //////&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

                        ClearFeilds();
                        uMsg.ShowMessage("MPN No " + mpnNoFormated + " Saved successfully.", RMS.BL.Enums.MessageType.Info);

                        mpnNoFormated = "";
                        IsEdit = false;

                    }
                    else
                    {
                        uMsg.ShowMessage("save was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ClearFeilds();
                uMsg.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        #endregion

        #region WebMethods

        [WebMethod]
        public static object GetGSTPercent(string gstId)
        {
            return new TaxBL().GetPercentByTaxID(gstId, null);
        }

        #endregion
    }
}
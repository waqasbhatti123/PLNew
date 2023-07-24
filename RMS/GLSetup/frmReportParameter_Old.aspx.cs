using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using RMS.BL;
using System.Data.Linq;
using Microsoft.Reporting.WebForms;

namespace RMS.GL.Setup
{

    public partial class frmReportParameter_Old : System.Web.UI.Page
    {

        #region DataMember
        
        EntitySet<tblglrptdet> glrptDetEntSet = new EntitySet<tblglrptdet>();
        DataTable d_table = new DataTable();
        DataRow drow;
        GLReportParameterBL glParameter = new GLReportParameterBL();
        
        #endregion

        #region Properties

        public DataTable CurrentTable
        {
            set { ViewState["CurrentTable"] = value; }
            get { return (DataTable)ViewState["CurrentTable"]; }
        }

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }
        public Int16 rptNo
        {
            get { return Convert.ToInt16(ViewState["rptNo"]); }
            set { ViewState["rptNo"] = value; }
        }
        public byte compId
        {
            get { return Convert.ToByte(ViewState["compId"]); }
            set { ViewState["compId"] = value; }
        }
        public int brId
        {
            get { return Convert.ToInt32(ViewState["brId"]); }
            set { ViewState["brId"] = value; }
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
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    brId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                brId = Convert.ToInt32(Session["BranchID"]);
            }
            if (!IsPostBack)
            {
                PID = Convert.ToInt32(Request.QueryString["PID"]);
                //if (PID == 318)
                //{
                    

                        Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ReportParameter").ToString();

                        if (Session["DateFormat"] == null)
                        {
                           // ClExt1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        }
                        else
                        {
                            //ClExt1.Format = Session["DateFormat"].ToString();
                        }

                        compId = Convert.ToByte(Session["CompID"]);

                   if (Convert.ToBoolean(Session["IfEdit"]) != true)
                    {

                        bindCurrentTable();
                        bindGrid();


                        //ClExt1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        //txtDate.Text = ClExt1.SelectedDate.Value.ToString();
                    }

                    else
                    {
                        Session["PageTitle"] = "Report Parameter (Edit)";
                        txtReportNo.Enabled = false;
                        rptNo = Convert.ToInt16(Session["rptNo"]);
                        getByRptNumber();
                    }
                //}
            }
        }
        protected void txtAcFrom_TextChanged(object sender, EventArgs e)
        {
            GridViewRow currRow = (GridViewRow)((Control)sender).NamingContainer as GridViewRow;
            TextBox txtAcFrom=(TextBox)currRow.FindControl("txtAcFrom");
            //HiddenField AcFr= (HiddenField)currRow.FindControl("AcFr");
            //txtAcFrom.Text = AcFr.Value;
            
            string[] txtFr = txtAcFrom.Text.Split('-');
            txtAcFrom.Text = txtFr[0];
        }
        protected void txtFromCC_TextChanged(object sender, EventArgs e)
        {
            GridViewRow currRow = (GridViewRow)((Control)sender).NamingContainer as GridViewRow;
            TextBox txtFromCC = (TextBox)currRow.FindControl("txtFromCC");

            string[] txtFr = txtFromCC.Text.Split('-');
            txtFromCC.Text = txtFr[0];
        }
        protected void txtToCC_TextChanged(object sender, EventArgs e)
        {
            GridViewRow currRow = (GridViewRow)((Control)sender).NamingContainer as GridViewRow;
            TextBox txtToCC = (TextBox)currRow.FindControl("txtToCC");

            string[] txtTo = txtToCC.Text.Split('-');
            txtToCC.Text = txtTo[0];
        }
        protected void txtAcTo_TextChanged(object sender, EventArgs e)
        {
            GridViewRow currRow = (GridViewRow)((Control)sender).NamingContainer as GridViewRow;
            TextBox txtAcTo = (TextBox)currRow.FindControl("txtAcTo");
            //HiddenField AcTo = (HiddenField)currRow.FindControl("AcTo");
            //txtAcTo.Text = AcTo.Value;
            string[] txtAcT = txtAcTo.Text.Split('-');
            txtAcTo.Text = txtAcT[0];
        }
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
            }
        }
        protected void lnkAddNewRow_Click(object sender, EventArgs e)
        {
            string recNo;
            string totalLevel;
            string printL;
            string initialization;
            //----for decurating
            string skipB;
            string skipA;
            string fsize;
            string bold;
            string ul;
            string ol;
            //-----
            getColumn();

            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                drow = d_table.NewRow();

                recNo = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtRecNo")).Text;
                if (!string.IsNullOrEmpty(recNo))
                {
                    drow["RecNo"] = Convert.ToInt32(recNo);
                }
                drow["Type"] = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlType")).SelectedValue;
                drow["Narration"] = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[2].FindControl("txtNarration")).Text;
                drow["AcFrom"] = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[3].FindControl("txtAcFrom")).Text;
                drow["AcTo"] = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[4].FindControl("txtAcTo")).Text;

                drow["FromCC"] = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[3].FindControl("txtFromCC")).Text;
                drow["ToCC"] = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[4].FindControl("txtToCC")).Text;

                drow["ArthOp"] = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlArithOp")).SelectedValue;

                totalLevel = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[5].FindControl("txtTotalLevel")).Text;
                if (!string.IsNullOrEmpty(totalLevel))
                {
                    drow["TotalLevel"] = Convert.ToInt32(totalLevel);
                }
                printL = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[7].FindControl("txtPrintLevel")).Text;
                if (!string.IsNullOrEmpty(printL))
                {
                    drow["PrintLevel"] = Convert.ToInt32(printL);
                }

                initialization = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[8].FindControl("txtInit")).Text;
                if (!string.IsNullOrEmpty(initialization))
                {
                    drow["Init"] = Convert.ToInt32(initialization);
                }

                skipB = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[9].FindControl("txtSkipB")).Text;
                if (!string.IsNullOrEmpty(skipB))
                {
                    drow["SkipB4"] = Convert.ToInt32(skipB);
                }
                skipA = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[10].FindControl("txtSkipA")).Text;
                if (!string.IsNullOrEmpty(skipA))
                {
                    drow["SkipA4"] = Convert.ToInt32(skipA);
                }
                fsize = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[11].FindControl("txtFontSize")).Text;
                if (!string.IsNullOrEmpty(fsize))
                {
                    drow["FSize"] = Convert.ToInt32(fsize);
                }
                bold = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[12].FindControl("txtBold")).Text;
                if (!string.IsNullOrEmpty(bold))
                {
                    drow["Bold"] = bold;
                }
                ul = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[13].FindControl("txtUnderLine")).Text;
                if (!string.IsNullOrEmpty(ul))
                {
                    drow["UL"] = ul;
                }
                ol = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[14].FindControl("txtOverLine")).Text;
                if (!string.IsNullOrEmpty(ol))
                {

                    drow["OL"] = ol;
                }


                d_table.Rows.Add(drow);
            }
            drow = d_table.NewRow();
            d_table.Rows.Add(drow);

            CurrentTable = d_table;
            grdView.DataSource = CurrentTable;
            grdView.DataBind();
            setDropDownTypeandArthOp();
            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["IfEdit"]) != true)
            {

                save();
            }
            else
            {
                edit();
            }
        }
        protected void btnList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GLSetup/frmReportParameterHome.aspx?PID=315");
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GLSetup/frmReportParameter.aspx?PID=611");
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gRow = (GridViewRow)((Control)sender).NamingContainer as GridViewRow;
            DropDownList ddlType = (DropDownList)gRow.FindControl("ddlType");
            manageGridCells(gRow, ddlType.SelectedValue);
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            
            //======================Get Data For Report============================
            printRpt(Convert.ToInt32(txtReportNo.Text));
        }

        #endregion

        #region Helping Methods

        private void manageGridCells(GridViewRow gRow, string type)
        {

            if (type == "H")
            {
                gRow.Cells[3].Enabled = false;
                gRow.Cells[4].Enabled = false;

                gRow.Cells[15].Enabled = false;
                gRow.Cells[16].Enabled = false;
                gRow.Cells[15].Text = "";
                gRow.Cells[16].Text = "";

                gRow.Cells[5].Enabled = false;
                gRow.Cells[6].Enabled = false;
                gRow.Cells[7].Enabled = false;
                gRow.Cells[8].Enabled = false;

                gRow.Cells[13].Enabled = false;
                gRow.Cells[14].Enabled = false;
                

                gRow.Cells[3].Text = "";
                gRow.Cells[4].Text = "";
                gRow.Cells[5].Text = "0";
                ((DropDownList)gRow.FindControl("ddlArithOp")).SelectedValue = "0";
                gRow.Cells[7].Text = "0";
                gRow.Cells[8].Text = "0";

            }
            else if (type == "D")
            {
                gRow.Cells[3].Enabled = false;
                gRow.Cells[4].Enabled = false;

                

                gRow.Cells[5].Enabled = false;
                gRow.Cells[6].Enabled = false;

                gRow.Cells[3].Text = "";
                gRow.Cells[4].Text = "";

                gRow.Cells[15].Enabled = false;
                gRow.Cells[16].Enabled = false;
                gRow.Cells[15].Text = "";
                gRow.Cells[16].Text = "";

                gRow.Cells[5].Text = "0";
                ((DropDownList)gRow.FindControl("ddlArithOp")).SelectedValue = "0";

                gRow.Cells[7].Enabled = true;
                gRow.Cells[8].Enabled = true;
                gRow.Cells[9].Enabled = true;
                gRow.Cells[10].Enabled = true;
                gRow.Cells[11].Enabled = true;
                gRow.Cells[12].Enabled = true;
                gRow.Cells[13].Enabled = true;
                gRow.Cells[14].Enabled = true;
                gRow.Cells[15].Enabled = false;
                gRow.Cells[16].Enabled = false;

            }
            else if (type == "C" || type == "E")
            {
                gRow.Cells[3].Enabled = true;
                gRow.Cells[4].Enabled = true;
                gRow.Cells[5].Enabled = false;
                gRow.Cells[6].Enabled = false;
                ((DropDownList)gRow.FindControl("ddlArithOp")).SelectedValue = "0";
                
                gRow.Cells[7].Enabled = false;
                gRow.Cells[7].Text = "0";
                gRow.Cells[8].Enabled = false;
                gRow.Cells[9].Enabled = true;
                gRow.Cells[10].Enabled = true;
                gRow.Cells[11].Enabled = true;
                gRow.Cells[12].Enabled = true;
                gRow.Cells[13].Enabled = true;
                gRow.Cells[14].Enabled = true;

                gRow.Cells[15].Enabled = true;
                gRow.Cells[16].Enabled = true;
            }
            else
            {
                gRow.Cells[3].Enabled = true;
                gRow.Cells[4].Enabled = true;
                gRow.Cells[5].Enabled = true;
                gRow.Cells[6].Enabled = true;

                gRow.Cells[7].Enabled = false;
                gRow.Cells[7].Text = "0";

                gRow.Cells[8].Enabled = true;

                gRow.Cells[9].Enabled = false;
                gRow.Cells[10].Enabled = false;
                gRow.Cells[11].Enabled = false;
                gRow.Cells[12].Enabled = false;
                gRow.Cells[13].Enabled = false;
                gRow.Cells[14].Enabled = false;

                gRow.Cells[15].Enabled = true;
                gRow.Cells[16].Enabled = true;
                
            }
        }
        private void bindGrid()
        {
            grdView.DataSource = CurrentTable;
            grdView.DataBind();
        }
        private void getColumn()
        {
            d_table.Columns.Clear();
            d_table.Rows.Clear();

            d_table.Columns.Add("RecNo", typeof(System.Int32));
            d_table.Columns.Add("Type", typeof(System.String));
            d_table.Columns.Add("Narration", typeof(System.String));
            d_table.Columns.Add("AcFrom", typeof(System.String));
            d_table.Columns.Add("AcTo", typeof(System.String));
            d_table.Columns.Add("TotalLevel", typeof(System.Int32));
            d_table.Columns.Add("ArthOp", typeof(System.String));
            d_table.Columns.Add("PrintLevel", typeof(System.Int32));
            d_table.Columns.Add("Init", typeof(System.Int32));
            d_table.Columns.Add("SkipB4", typeof(System.Int32));
            d_table.Columns.Add("SkipA4", typeof(System.Int32));
            d_table.Columns.Add("FSize", typeof(System.Int32));
            d_table.Columns.Add("Bold", typeof(System.String));
            d_table.Columns.Add("UL", typeof(System.String));
            d_table.Columns.Add("OL", typeof(System.String));

            d_table.Columns.Add("FromCC", typeof(System.String));
            d_table.Columns.Add("ToCC", typeof(System.String));

        }
        private void bindCurrentTable()
        {
            getColumn();
            for (int i = 10; i < 15; i++)
            {
                drow = d_table.NewRow();
                drow["RecNo"] = i;
                d_table.Rows.Add(drow);
            }
            CurrentTable = d_table;
        }
        private void save()
        {
            try
            {
                if (glParameter.isExist(compId, Convert.ToInt16(txtReportNo.Text), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) == false)
                {
                    tblglrpt glrpt = new tblglrpt();

                    glrpt.CompId = compId;
                    glrpt.RptNo = Convert.ToInt16(txtReportNo.Text);
                    glrpt.Rptdesc = txtName.Text;
                    glrpt.NoteNo = txtNoteNo.Text;

                    glrpt.CompLevelRpt = Convert.ToChar(ddlCompLevel.SelectedValue);

                    glrpt.PrintDrCr = Convert.ToChar(ddlPrintDrCr.SelectedValue); ;
                    glrpt.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    Int16 RecNo = 0;
                    glrpt.PrintPrvYr = Convert.ToChar(ddlPrintPreYr.SelectedValue);
                    glrpt.updateby = Session["UserID"].ToString();

                    for (int i = 0; i < grdView.Rows.Count; i++)
                    {

                        string recNo = Convert.ToString(((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[0].FindControl("txtRecNo")).Text);
                        if (!string.IsNullOrEmpty(recNo))
                        {
                            RecNo = Convert.ToInt16(recNo);
                        }
                        string Type = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlType")).SelectedValue;
                        string Narration = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[2].FindControl("txtNarration")).Text;
                        string AcFr = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[3].FindControl("txtAcFrom")).Text;
                        string AcTo = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[4].FindControl("txtAcTo")).Text;

                        string CcFr = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtFromCC")).Text;
                        string CcTo = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtToCC")).Text;

                        string ArthOp = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlArithOp")).SelectedValue;

                        string totalLevel = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[5].FindControl("txtTotalLevel")).Text;
                        string printL = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[7].FindControl("txtPrintLevel")).Text;
                        string initialization = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[8].FindControl("txtInit")).Text;
                        //-----------For Decurating---------
                        string skipB = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[9].FindControl("txtSkipB")).Text;
                        string skipA = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[10].FindControl("txtSkipA")).Text;
                        string fsize = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[11].FindControl("txtFontSize")).Text;
                        string bold = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[12].FindControl("txtBold")).Text;
                        string uline = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[13].FindControl("txtUnderLine")).Text;
                        string oline = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[14].FindControl("txtOverLine")).Text;
                        //----------------------------------

                        if (recNo != "" && Type != "0" && Narration != "")
                        {
                            tblglrptdet rptDet = new tblglrptdet();
                            rptDet.CompId = compId;
                            rptDet.RptNo = Convert.ToInt16(txtReportNo.Text);
                            rptDet.RecNo = RecNo;
                            rptDet.RecType = Convert.ToChar(Type);
                            rptDet.Recdesc = Narration;
                            rptDet.ActFrom = AcFr;
                            rptDet.ActTo = AcTo;

                            rptDet.CC_From = CcFr == "" ? null : CcFr; ;
                            rptDet.CC_To = CcTo == "" ? null : CcTo; ;

                            if (totalLevel != "")
                            {
                                rptDet.TotalLevel = Convert.ToDecimal(totalLevel);
                            }
                            else
                            {
                                rptDet.TotalLevel = 0;
                            }
                            rptDet.ArithOp = ArthOp;
                            if (printL != "")
                            {
                                rptDet.PrintLevel = Convert.ToDecimal(printL);
                            }
                            else
                            {
                                rptDet.PrintLevel = 0;
                            }
                            if (initialization != "")
                            {
                                rptDet.InitLevel = Convert.ToDecimal(initialization);
                            }
                            else
                            {
                                rptDet.InitLevel = 0;
                            }

                            //-------For Decurating----
                            if (skipB != "")
                            {
                                rptDet.SkipB4 = Convert.ToDecimal(skipB);
                            }
                            else
                            {
                                rptDet.SkipB4 = 0;
                            }

                            if (skipA != "")
                            {
                                rptDet.SkipA4 = Convert.ToDecimal(skipA);
                            }
                            else
                            {
                                rptDet.SkipA4 = 0;
                            }
                            if (fsize != "")
                            {
                                rptDet.FontSize = Convert.ToDecimal(fsize);
                            }
                            else
                            {
                                rptDet.FontSize = 10;
                            }
                            if (bold != "")
                            {
                                rptDet.Bold = Convert.ToChar(bold.ToUpper());
                            }
                            else
                            {
                                rptDet.Bold = 'N';
                            }
                            if (uline != "")
                            {
                                rptDet.ULDesc = Convert.ToChar(uline.ToUpper());
                            }
                            else
                            {
                                rptDet.ULDesc = 'N';
                            }
                            if (oline != "")
                            {
                                rptDet.OLTotal = Convert.ToChar(oline.ToUpper());
                            }
                            else
                            {
                                rptDet.OLTotal = 'N';
                            }

                            //-------------------------

                            glrptDetEntSet.Add(rptDet);

                            // RecNo++;

                        }


                    }
                    //if ()
                    //{
                    if (glParameter.save(glrpt, glrptDetEntSet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage("Saved Successfully...", RMS.BL.Enums.MessageType.Info);
                        glrptDetEntSet.Clear();


                    }
                    //}
                    //else
                    //{
                    //    ucMessage.ShowMessage("kindly insert adleast one record", RMS.BL.Enums.MessageType.Info);
                    //}
                }
                else
                {
                    ucMessage.ShowMessage("Record Already Exist", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
            }


        }
        private void edit()
        {
            try
            {

                tblglrpt glrpt = new tblglrpt();

                glrpt.CompId = compId;
                glrpt.RptNo = Convert.ToInt16(txtReportNo.Text);
                glrpt.Rptdesc = txtName.Text;
                glrpt.NoteNo = txtNoteNo.Text;

                glrpt.CompLevelRpt = Convert.ToChar(ddlCompLevel.SelectedValue);

                glrpt.PrintDrCr = Convert.ToChar(ddlPrintDrCr.SelectedValue);
                glrpt.PrintPrvYr = Convert.ToChar(ddlPrintPreYr.SelectedValue);
                glrpt.updateby = Session["UserID"].ToString();
                glrpt.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                Int16 RecNo = 0;

                for (int i = 0; i < grdView.Rows.Count; i++)
                {
                    string recNo = Convert.ToString(((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[0].FindControl("txtRecNo")).Text);
                    if (!string.IsNullOrEmpty(recNo))
                    {
                        RecNo = Convert.ToInt16(recNo);
                    }
                    string Type = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlType")).SelectedValue;
                    string Narration = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[2].FindControl("txtNarration")).Text;
                    string AcFr = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[3].FindControl("txtAcFrom")).Text;
                    string AcTo = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[4].FindControl("txtAcTo")).Text;

                    string CcFr = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtFromCC")).Text;
                    string CcTo = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].FindControl("txtToCC")).Text;

                    string ArthOp = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlArithOp")).SelectedValue;

                    string totalLevel = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[5].FindControl("txtTotalLevel")).Text;

                    string printL = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[7].FindControl("txtPrintLevel")).Text;
                    string initialization = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[8].FindControl("txtInit")).Text;

                    //-----------For Decurating---------
                    string skipB = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[9].FindControl("txtSkipB")).Text;
                    string skipA = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[10].FindControl("txtSkipA")).Text;
                    string fsize = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[11].FindControl("txtFontSize")).Text;
                    string bold = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[12].FindControl("txtBold")).Text;
                    string uline = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[13].FindControl("txtUnderLine")).Text;
                    string oline = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[14].FindControl("txtOverLine")).Text;
                    //----------------------------------

                    if (recNo != "" && Type != "0" && Narration != "")
                    {
                        tblglrptdet rptDet = new tblglrptdet();
                        rptDet.CompId = compId;
                        rptDet.RptNo = Convert.ToInt16(txtReportNo.Text);
                        rptDet.RecNo = RecNo;
                        rptDet.RecType = Convert.ToChar(Type);
                        rptDet.Recdesc = Narration;
                        rptDet.ActFrom = AcFr;
                        rptDet.ActTo = AcTo;

                        rptDet.CC_From = CcFr == "" ? null : CcFr; ;
                        rptDet.CC_To = CcTo == "" ? null : CcTo; ;

                        if (totalLevel != "")
                        {
                            rptDet.TotalLevel = Convert.ToDecimal(totalLevel);
                        }
                        else
                        {
                            rptDet.TotalLevel = 0;
                        }
                        rptDet.ArithOp = ArthOp;
                        if (printL != "")
                        {
                            rptDet.PrintLevel = Convert.ToDecimal(printL);
                        }
                        else
                        {
                            rptDet.PrintLevel = 0;
                        }
                        if (initialization != "")
                        {
                            rptDet.InitLevel = Convert.ToDecimal(initialization);
                        }
                        else
                        {
                            rptDet.InitLevel = 0;
                        }

                        //-------For Decurating----
                        if (skipB != "")
                        {
                            rptDet.SkipB4 = Convert.ToDecimal(skipB);
                        }
                        else
                        {
                            rptDet.SkipB4 = 0;
                        }

                        if (skipA != "")
                        {
                            rptDet.SkipA4 = Convert.ToDecimal(skipA);
                        }
                        else
                        {
                            rptDet.SkipA4 = 0;
                        }
                        if (fsize != "")
                        {
                            rptDet.FontSize = Convert.ToDecimal(fsize);
                        }
                        else
                        {
                            rptDet.FontSize = 10;
                        }
                        if (bold != "")
                        {
                            rptDet.Bold = Convert.ToChar(bold.ToUpper());
                        }
                        else
                        {
                            rptDet.Bold = 'N';
                        }
                        if (uline != "")
                        {
                            rptDet.ULDesc = Convert.ToChar(uline.ToUpper());
                        }
                        else
                        {
                            rptDet.ULDesc = 'N';
                        }
                        if (oline != "")
                        {
                            rptDet.OLTotal = Convert.ToChar(oline.ToUpper());
                        }
                        else
                        {
                            rptDet.OLTotal = 'N';
                        }

                        //-------------------------

                        glrptDetEntSet.Add(rptDet);

                        //RecNo++;

                    }


                }
                //---------Deleting records
                glParameter.deleteRecord(compId, rptNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //----------
                if (glParameter.save(glrpt, glrptDetEntSet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                {
                    ucMessage.ShowMessage("Saved Successfully...", RMS.BL.Enums.MessageType.Info);
                    glrptDetEntSet.Clear();

                }


            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
            }
            //try
            //{
            //    if (Convert.ToInt16(txtReportNo.Text)!=rptNo)
            //    {

            //    }

            //    txtReportNo.Enabled = false;

            //    if (glParameter.deleteRecord(compId,rptNo,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
            //    {
            //        save();

            //    }

            //}
            //catch (Exception ex)
            //{
            //    ucMessage.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
            //}
        }
        private void getByRptNumber()
        {
            tblglrpt tbl = glParameter.getByRptNo(rptNo, compId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            txtReportNo.Text = tbl.RptNo.ToString();
            if (tbl.NoteNo != null)
                txtNoteNo.Text = tbl.NoteNo;
            else
                txtNoteNo.Text = "";

            txtName.Text = tbl.Rptdesc;
            ddlCompLevel.SelectedValue = tbl.CompLevelRpt.ToString();
            ddlPrintDrCr.SelectedValue = tbl.PrintDrCr.ToString();
            ddlPrintPreYr.SelectedValue = tbl.PrintPrvYr.ToString();
            getColumn();
            List<tblglrptdet> ls = glParameter.getDetailByRptNo(rptNo, compId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            foreach (var a in ls)
            {
                drow = d_table.NewRow();
                drow["RecNo"] = a.RecNo;
                drow["Type"] = a.RecType;
                drow["Narration"] = a.Recdesc;
                drow["AcFrom"] = a.ActFrom;
                drow["AcTo"] = a.ActTo;

                drow["FromCC"] = a.CC_From;
                drow["ToCC"] = a.CC_To;

                drow["TotalLevel"] = a.TotalLevel;
                drow["ArthOp"] = a.ArithOp;
                drow["PrintLevel"] = a.PrintLevel;
                drow["Init"] = a.InitLevel;

                //--------for decurating
                drow["SkipB4"] = a.SkipB4;
                drow["SkipA4"] = a.SkipA4;
                drow["FSize"] = a.FontSize;
                drow["Bold"] = a.Bold;
                drow["UL"] = a.ULDesc;
                drow["OL"] = a.OLTotal;
                //-----------------

                d_table.Rows.Add(drow);
            }
            d_table.DefaultView.Sort = "RecNo ASC";

            CurrentTable = d_table.DefaultView.Table;

            grdView.DataSource = CurrentTable;
            grdView.DataBind();
            setDropDownTypeandArthOp();
        }
        private void setDropDownTypeandArthOp()
        {
            int rows = CurrentTable.Rows.Count;
            try
            {
                for (int i = 0; i < rows; i++)
                {
                    DropDownList ddlType = (DropDownList)grdView.Rows[i].FindControl("ddlType");
                    DropDownList ddlArOp = (DropDownList)grdView.Rows[i].FindControl("ddlArithOp");

                    if (CurrentTable.Rows[i]["Type"] != DBNull.Value)
                    {
                        ddlType.SelectedValue = CurrentTable.Rows[i]["Type"].ToString();
                        //if (CurrentTable.Rows[i]["Type"].ToString() == "H")
                        //{
                        //    grdView.Rows[i].Cells[3].Enabled = false;
                        //    grdView.Rows[i].Cells[4].Enabled = false;
                        //    grdView.Rows[i].Cells[5].Enabled = false;
                        //    grdView.Rows[i].Cells[6].Enabled = false;
                        //    grdView.Rows[i].Cells[7].Enabled = false;
                        //    grdView.Rows[i].Cells[8].Enabled = false;


                        //}

                        //------Replaced the above code with this 26 March---------
                        manageGridCells(grdView.Rows[i], CurrentTable.Rows[i]["Type"].ToString());
                        //------------

                    }
                    if (CurrentTable.Rows[i]["ArthOp"] != DBNull.Value)
                    {
                        string arth = CurrentTable.Rows[i]["ArthOp"].ToString();
                        // ddlArOp.ClearSelection();
                        ddlArOp.SelectedValue = arth;
                        //ddlArOp.Items.FindByValue(CurrentTable.Rows[i]["ArthOp"].ToString()).Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
            }

        }
        protected void printRpt(int reportN)
        {
            string ReportName = "";
            string NoteNumber = "";


            decimal[,] Level = new decimal[2, 10];
            Level[0, 0] = 0;
            Level[0, 1] = 0;
            Level[0, 2] = 0;
            Level[0, 3] = 0;
            Level[0, 4] = 0;
            Level[0, 5] = 0;
            Level[0, 6] = 0;
            Level[0, 7] = 0;
            Level[0, 8] = 0;
            Level[0, 9] = 0;

            DataTable dtable = new DataTable();
            DataRow dataRow;
            dtable.Columns.Add("RptNo", typeof(System.Int32));
            dtable.Columns.Add("RptDesc", typeof(System.String));
            dtable.Columns.Add("PrintDrCr", typeof(System.String));
            dtable.Columns.Add("PrintPrvYr", typeof(System.String));
            dtable.Columns.Add("CompLevelRpt", typeof(System.String));
            dtable.Columns.Add("RecNo", typeof(System.String));
            dtable.Columns.Add("RecType", typeof(System.String));
            dtable.Columns.Add("Recdesc", typeof(System.String));
            dtable.Columns.Add("ActFrom", typeof(System.String));
            dtable.Columns.Add("ActTo", typeof(System.String));
            dtable.Columns.Add("TotalLevel", typeof(System.Int32));
            dtable.Columns.Add("PrintLevel", typeof(System.Int32));
            dtable.Columns.Add("ArithOp", typeof(System.String));
            dtable.Columns.Add("InitLevel", typeof(System.Int32));
            dtable.Columns.Add("debit", typeof(System.Decimal));
            dtable.Columns.Add("credit", typeof(System.Decimal));
            dtable.Columns.Add("printDebit", typeof(System.Decimal));
            dtable.Columns.Add("printCredit", typeof(System.Decimal));
            string arith = "";
            int tLevel;
            decimal debt = 0;
            decimal credit = 0;
            try
            {

                List<spGlReportResult> ls = glParameter.getGlReportData(compId, brId, reportN, Convert.ToDateTime("01-07-2012"), 'A', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                foreach (var a in ls)
                {
                    dataRow = dtable.NewRow();

                    dataRow["RptNo"] = a.RptNo;
                    dataRow["RptDesc"] = a.Rptdesc;
                    dataRow["PrintDrCr"] = a.PrintDrCr;
                    dataRow["PrintPrvYr"] = a.PrintPrvYr;
                    dataRow["CompLevelRpt"] = a.CompLevelRpt;
                    dataRow["RecNo"] = a.RecNo;
                    dataRow["RecType"] = a.RecType;
                    dataRow["Recdesc"] = a.Recdesc;
                    dataRow["ActFrom"] = a.ActFrom;
                    dataRow["ActTo"] = a.ActTo;
                    dataRow["TotalLevel"] = a.TotalLevel;
                    dataRow["PrintLevel"] = a.PrintLevel;
                    dataRow["ArithOp"] = a.ArithOp;
                    dataRow["InitLevel"] = a.InitLevel;
                    if (a.debit != null)
                    {
                        dataRow["debit"] = a.debit;
                        debt = Convert.ToDecimal(a.debit);
                    }
                    if (a.credit != null)
                    {
                        dataRow["credit"] = a.credit;
                        credit = Convert.ToDecimal(a.credit);
                    }


                    arith = a.ArithOp;
                    tLevel = Convert.ToInt32(a.TotalLevel);
                    //--------------------------
                    ReportName = a.Rptdesc;
                    NoteNumber = a.NoteNo;
                    //----------------------



                    //------------for Debit
                    switch (tLevel)
                    {
                        case 1:
                            Level[0, 1] = calculate(Level[0, 1], debt, arith);
                            break;
                        case 2:
                            Level[0, 1] = calculate(Level[0, 1], debt, arith);
                            Level[0, 2] = calculate(Level[0, 2], debt, arith);
                            break;
                        case 3:
                            Level[0, 1] = calculate(Level[0, 1], debt, arith);
                            Level[0, 2] = calculate(Level[0, 2], debt, arith);
                            Level[0, 3] = calculate(Level[0, 3], debt, arith);
                            break;
                        case 4:
                            Level[0, 1] = calculate(Level[0, 1], debt, arith);
                            Level[0, 2] = calculate(Level[0, 2], debt, arith);
                            Level[0, 3] = calculate(Level[0, 3], debt, arith);
                            Level[0, 4] = calculate(Level[0, 4], debt, arith);
                            break;
                        case 5:
                            Level[0, 1] = calculate(Level[0, 1], debt, arith);
                            Level[0, 2] = calculate(Level[0, 2], debt, arith);
                            Level[0, 3] = calculate(Level[0, 3], debt, arith);
                            Level[0, 4] = calculate(Level[0, 4], debt, arith);
                            Level[0, 5] = calculate(Level[0, 5], debt, arith);
                            break;
                        case 6:
                            Level[0, 1] = calculate(Level[0, 1], debt, arith);
                            Level[0, 2] = calculate(Level[0, 2], debt, arith);
                            Level[0, 3] = calculate(Level[0, 3], debt, arith);
                            Level[0, 4] = calculate(Level[0, 4], debt, arith);
                            Level[0, 5] = calculate(Level[0, 5], debt, arith);
                            Level[0, 6] = calculate(Level[0, 6], debt, arith);
                            break;
                        case 7:
                            Level[0, 1] = calculate(Level[0, 1], debt, arith);
                            Level[0, 2] = calculate(Level[0, 2], debt, arith);
                            Level[0, 3] = calculate(Level[0, 3], debt, arith);
                            Level[0, 4] = calculate(Level[0, 4], debt, arith);
                            Level[0, 5] = calculate(Level[0, 5], debt, arith);
                            Level[0, 6] = calculate(Level[0, 6], debt, arith);
                            Level[0, 7] = calculate(Level[0, 7], debt, arith);
                            break;
                        case 8:
                            Level[0, 1] = calculate(Level[0, 1], debt, arith);
                            Level[0, 2] = calculate(Level[0, 2], debt, arith);
                            Level[0, 3] = calculate(Level[0, 3], debt, arith);
                            Level[0, 4] = calculate(Level[0, 4], debt, arith);
                            Level[0, 5] = calculate(Level[0, 5], debt, arith);
                            Level[0, 6] = calculate(Level[0, 6], debt, arith);
                            Level[0, 7] = calculate(Level[0, 7], debt, arith);
                            Level[0, 8] = calculate(Level[0, 8], debt, arith);
                            break;
                        case 9:
                            Level[0, 1] = calculate(Level[0, 1], debt, arith);
                            Level[0, 2] = calculate(Level[0, 2], debt, arith);
                            Level[0, 3] = calculate(Level[0, 3], debt, arith);
                            Level[0, 4] = calculate(Level[0, 4], debt, arith);
                            Level[0, 5] = calculate(Level[0, 5], debt, arith);
                            Level[0, 6] = calculate(Level[0, 6], debt, arith);
                            Level[0, 7] = calculate(Level[0, 7], debt, arith);
                            Level[0, 8] = calculate(Level[0, 8], debt, arith);
                            Level[0, 9] = calculate(Level[0, 9], debt, arith);
                            break;
                    }

                    //----------for Credit
                    switch (tLevel)
                    {
                        case 1:
                            Level[1, 1] = calculate(Level[1, 1], credit, arith);
                            break;
                        case 2:
                            Level[1, 1] = calculate(Level[1, 1], credit, arith);
                            Level[1, 2] = calculate(Level[1, 2], credit, arith);
                            break;
                        case 3:
                            Level[1, 1] = calculate(Level[1, 1], credit, arith);
                            Level[1, 2] = calculate(Level[1, 2], credit, arith);
                            Level[1, 3] = calculate(Level[1, 3], credit, arith);
                            break;
                        case 4:
                            Level[1, 1] = calculate(Level[1, 1], credit, arith);
                            Level[1, 2] = calculate(Level[1, 2], credit, arith);
                            Level[1, 3] = calculate(Level[1, 3], credit, arith);
                            Level[1, 4] = calculate(Level[1, 4], credit, arith);
                            break;
                        case 5:
                            Level[1, 1] = calculate(Level[1, 1], credit, arith);
                            Level[1, 2] = calculate(Level[1, 2], credit, arith);
                            Level[1, 3] = calculate(Level[1, 3], credit, arith);
                            Level[1, 4] = calculate(Level[1, 4], credit, arith);
                            Level[1, 5] = calculate(Level[1, 5], credit, arith);
                            break;
                        case 6:
                            Level[1, 1] = calculate(Level[1, 1], credit, arith);
                            Level[1, 2] = calculate(Level[1, 2], credit, arith);
                            Level[1, 3] = calculate(Level[1, 3], credit, arith);
                            Level[1, 4] = calculate(Level[1, 4], credit, arith);
                            Level[1, 5] = calculate(Level[1, 5], credit, arith);
                            Level[1, 6] = calculate(Level[1, 6], credit, arith);
                            break;
                        case 7:
                            Level[1, 1] = calculate(Level[1, 1], credit, arith);
                            Level[1, 2] = calculate(Level[1, 2], credit, arith);
                            Level[1, 3] = calculate(Level[1, 3], credit, arith);
                            Level[1, 4] = calculate(Level[1, 4], credit, arith);
                            Level[1, 5] = calculate(Level[1, 5], credit, arith);
                            Level[1, 6] = calculate(Level[1, 6], credit, arith);
                            Level[1, 7] = calculate(Level[1, 7], credit, arith);
                            break;
                        case 8:
                            Level[1, 1] = calculate(Level[1, 1], credit, arith);
                            Level[1, 2] = calculate(Level[1, 2], credit, arith);
                            Level[1, 3] = calculate(Level[1, 3], credit, arith);
                            Level[1, 4] = calculate(Level[1, 4], credit, arith);
                            Level[1, 5] = calculate(Level[1, 5], credit, arith);
                            Level[1, 6] = calculate(Level[1, 6], credit, arith);
                            Level[1, 7] = calculate(Level[1, 7], credit, arith);
                            Level[1, 8] = calculate(Level[1, 8], credit, arith);
                            break;
                        case 9:
                            Level[1, 1] = calculate(Level[1, 1], credit, arith);
                            Level[1, 2] = calculate(Level[1, 2], credit, arith);
                            Level[1, 3] = calculate(Level[1, 3], credit, arith);
                            Level[1, 4] = calculate(Level[1, 4], credit, arith);
                            Level[1, 5] = calculate(Level[1, 5], credit, arith);
                            Level[1, 6] = calculate(Level[1, 6], credit, arith);
                            Level[1, 7] = calculate(Level[1, 7], credit, arith);
                            Level[1, 8] = calculate(Level[1, 8], credit, arith);
                            Level[1, 9] = calculate(Level[1, 9], credit, arith);
                            break;
                    }


                    if (a.PrintLevel != 0)
                    {
                        dataRow["printDebit"] = Level[0, Convert.ToInt32(a.PrintLevel)];
                        dataRow["printCredit"] = Level[1, Convert.ToInt32(a.PrintLevel)];

                    }
                    else
                    {
                        //dataRow["print"] = 0;
                    }
                    dtable.Rows.Add(dataRow);
                    Level[0, Convert.ToInt32(a.InitLevel)] = 0;
                    Level[1, Convert.ToInt32(a.InitLevel)] = 0;

                }



                //==================================================





                ReportViewer reportViewer = new ReportViewer();
                reportViewer.Visible = false;
                reportViewer.LocalReport.ReportPath = "GLSetup/rdlc/rptGlReport.rdlc";
                reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                reportViewer.LocalReport.Refresh();
                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.Refresh();


                ReportDataSource dataSource = new ReportDataSource("GlRptDataSet_GlRptDataTable", dtable);

                ReportParameter[] rpt = new ReportParameter[2];
                rpt[0] = new ReportParameter("ReportName", ReportName);
                rpt[1] = new ReportParameter("NoteNo", NoteNumber);


                reportViewer.LocalReport.SetParameters(rpt);
                reportViewer.LocalReport.DataSources.Clear();

                reportViewer.LocalReport.DataSources.Add(dataSource);




                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                string filename;


                byte[] bytes = reportViewer.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

                filename = string.Format("{0}.{1}", "GLReport", "pdf");
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
                ucMessages1.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
                //throw ex;
            }
        }
        private decimal calculate(decimal x, decimal y, string op)
        {
            if (op == "Add")
            {
                return x + y;
            }
            else if (op == "Sub")
            {
                return x - y;
            }
            else if (op == "Mul")
            {
                return x * y;
            }
            else if (op == "Div" && y != 0)
            {
                return x / y;
            }
            else
            {
                return 0;
            }
        }

        #endregion



    }
}

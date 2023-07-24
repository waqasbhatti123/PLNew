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

    public partial class frmReportParameterHome : System.Web.UI.Page
    {

        #region DataMember
        EntitySet<tblglrptdet> glrptDetEntSet = new EntitySet<tblglrptdet>();
        DataTable d_table = new DataTable();
        
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
                //if (PID == 315)
                //{
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ReportParameter").ToString();

                    if (Session["DateFormat"] == null)
                    {
                        ClExt1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    }
                    else
                    {
                        ClExt1.Format = Session["DateFormat"].ToString();
                    }
                    compId = Convert.ToByte(Session["CompID"]);

                    ClExt1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    txtDate.Text = ClExt1.SelectedDate.Value.ToString();

                    bindGrid("", "", "", Convert.ToByte(Session["CompID"]));
              //  }
            }
            
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            Session["IfEdit"] = false;
            Response.Redirect("~/GLSetup/frmReportParameter.aspx?PID=611");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindGrid(txtReportNo.Text,txtNoteNo.Text,txtName.Text, compId);
        }


        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            string rptNo = clickedRow.Cells[0].Text;
            Session["rptNo"] = rptNo;
            Session["IfEdit"] = true;
           
           // Session["PgTile"] = "InwardGateEdit";
            Response.Redirect("~/GLSetup/frmReportParameter.aspx?PID=611");

        }
        protected void lnkView_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            string rptNo = clickedRow.Cells[0].Text;
            Session["rptNo"] = rptNo;
            Session["IfEdit"] = true;

            // Session["PgTile"] = "InwardGateEdit";
            Response.Redirect("~/GLSetup/frmReportParameterView.aspx?PID=319");
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            string rptNo = clickedRow.Cells[0].Text;
            printRpt(Convert.ToInt32(rptNo),Convert.ToDateTime(txtDate.Text));
        }

        
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        
        private void bindGrid(string rptNo,string noteNo,string rptName,byte compId)
        {
            Int16 rpNo=0;
            Int16 ntNo=0;

            if (rptNo!="")
            {
                rpNo = Convert.ToInt16(rptNo);
            }
            else
            {
                rpNo = 0;
            }

            if (noteNo!="")
            {
                ntNo = Convert.ToInt16(noteNo);
            }
            else
            {
                ntNo = 0;
            }
            grdView.DataSource = glParameter.getHomeGridData(rpNo, ntNo, rptName.Trim(), compId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdView.DataBind();
        }
    





        #region helping method

        protected void printRpt(int reportN, DateTime toDate)
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

            // For Previous Year
            decimal[,] LevelPrv = new decimal[2, 10];
            LevelPrv[0, 0] = 0;
            LevelPrv[0, 1] = 0;
            LevelPrv[0, 2] = 0;
            LevelPrv[0, 3] = 0;
            LevelPrv[0, 4] = 0;
            LevelPrv[0, 5] = 0;
            LevelPrv[0, 6] = 0;
            LevelPrv[0, 7] = 0;
            LevelPrv[0, 8] = 0;
            LevelPrv[0, 9] = 0;
            //--------------------------

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
            dtable.Columns.Add("OpBlnce", typeof(System.Decimal));
            dtable.Columns.Add("debitPrvYear", typeof(System.Decimal));
            dtable.Columns.Add("creditPrvYear", typeof(System.Decimal));
            dtable.Columns.Add("printDebitPrvYear", typeof(System.Decimal));
            dtable.Columns.Add("printCreditPrvYear", typeof(System.Decimal));
            dtable.Columns.Add("OpBlncePrvYear", typeof(System.Decimal));

            //------------for Decurating---
            dtable.Columns.Add("SkipB4", typeof(System.Decimal));
            dtable.Columns.Add("SkipA4", typeof(System.Decimal));
            dtable.Columns.Add("FontSize", typeof(System.Decimal));
            dtable.Columns.Add("Bold", typeof(System.String));
            dtable.Columns.Add("ULDesc", typeof(System.String));
            dtable.Columns.Add("OLTotal", typeof(System.String));
            dtable.Columns.Add("ULTotal", typeof(System.String));
            //-----------------------------
            string arith = "";
            int tLevel;
            decimal debt = 0;
            decimal credit = 0;
            decimal opblnc = 0;

            //--------For Prv Year

            decimal debtPrvYr = 0;
            decimal creditPrvYr = 0;
            decimal opblncPrvYr = 0;
            //---------
            try
            {

                List<spGlReportResult> ls = glParameter.getGlReportData(compId, brId, reportN,toDate, 'A', (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                foreach (var a in ls)
                {
                    dataRow = dtable.NewRow();

                    //-----------for skip--
                    if (a.SkipB4 != 0)
                    {
                        int i=0;
                        while (i < a.SkipB4)
                        {
                            DataRow extraRow = dtable.NewRow();
                            dtable.Rows.Add(extraRow);
                            i++;
                        }
                       
                    }
                    //---------------------

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
                    //------For decurating
                    dataRow["SkipB4"]   = a.SkipB4;
                    dataRow["SkipA4"]   = a.SkipA4;
                    dataRow["FontSize"] = a.FontSize;
                    dataRow["Bold"]     = a.Bold;
                    dataRow["ULDesc"]   = a.ULDesc;
                    dataRow["OLTotal"]  = a.OLTotal;
                    dataRow["ULTotal"]  = a.ULTotal;
                    //-------------------

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
                    if (a.OpBlnc != null)
                    {
                        dataRow["OpBlnce"] = a.OpBlnc;
                        opblnc = Convert.ToDecimal(a.OpBlnc);
                    }

                    //-------------For Prv Year
                    if (a.debitPrvYear != null)
                    {
                        dataRow["debitPrvYear"] = a.debitPrvYear;
                        debtPrvYr = Convert.ToDecimal(a.debitPrvYear);
                    }
                    if (a.creditPrvYear != null)
                    {
                        dataRow["creditPrvYear"] = a.creditPrvYear;
                        creditPrvYr = Convert.ToDecimal(a.creditPrvYear);
                    }
                    if (a.OpBlncPrvYear != null)
                    {
                        dataRow["OpBlncePrvYear"] = a.OpBlncPrvYear;
                        opblncPrvYr = Convert.ToDecimal(a.OpBlncPrvYear);
                    }

                    //----------------


                    //----------Adding Op Blnc in debit/credit-----------

                    if (opblnc > 0)
                    {
                        debt = debt + opblnc;
                    }
                    else if (opblnc < 0)
                    {
                        opblnc = opblnc * -1;
                        credit = credit + opblnc;
                    }
                    //---------------------


                    //----------Adding Op Blnc in debit/credit  For Prv Year-----------

                    if (opblncPrvYr > 0)
                    {
                        debtPrvYr = debtPrvYr + opblncPrvYr;
                    }
                    else if (opblncPrvYr < 0)
                    {
                        opblncPrvYr = opblncPrvYr * -1;
                        creditPrvYr = creditPrvYr + opblncPrvYr;
                    }
                    //---------------------

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
                    //-------------------For Previous Year
                    //------------for Debit  Previous Year
                    switch (tLevel)
                    {
                        case 1:
                            LevelPrv[0, 1] = calculate(LevelPrv[0, 1], debtPrvYr, arith);
                            break;
                        case 2:
                            LevelPrv[0, 1] = calculate(LevelPrv[0, 1], debtPrvYr, arith);
                            LevelPrv[0, 2] = calculate(LevelPrv[0, 2], debtPrvYr, arith);
                            break;
                        case 3:
                            LevelPrv[0, 1] = calculate(LevelPrv[0, 1], debtPrvYr, arith);
                            LevelPrv[0, 2] = calculate(LevelPrv[0, 2], debtPrvYr, arith);
                            LevelPrv[0, 3] = calculate(LevelPrv[0, 3], debtPrvYr, arith);
                            break;
                        case 4:
                            LevelPrv[0, 1] = calculate(LevelPrv[0, 1], debtPrvYr, arith);
                            LevelPrv[0, 2] = calculate(LevelPrv[0, 2], debtPrvYr, arith);
                            LevelPrv[0, 3] = calculate(LevelPrv[0, 3], debtPrvYr, arith);
                            LevelPrv[0, 4] = calculate(LevelPrv[0, 4], debtPrvYr, arith);
                            break;
                        case 5:
                            LevelPrv[0, 1] = calculate(LevelPrv[0, 1], debtPrvYr, arith);
                            LevelPrv[0, 2] = calculate(LevelPrv[0, 2], debtPrvYr, arith);
                            LevelPrv[0, 3] = calculate(LevelPrv[0, 3], debtPrvYr, arith);
                            LevelPrv[0, 4] = calculate(LevelPrv[0, 4], debtPrvYr, arith);
                            LevelPrv[0, 5] = calculate(LevelPrv[0, 5], debtPrvYr, arith);
                            break;
                        case 6:
                            LevelPrv[0, 1] = calculate(LevelPrv[0, 1], debtPrvYr, arith);
                            LevelPrv[0, 2] = calculate(LevelPrv[0, 2], debtPrvYr, arith);
                            LevelPrv[0, 3] = calculate(LevelPrv[0, 3], debtPrvYr, arith);
                            LevelPrv[0, 4] = calculate(LevelPrv[0, 4], debtPrvYr, arith);
                            LevelPrv[0, 5] = calculate(LevelPrv[0, 5], debtPrvYr, arith);
                            LevelPrv[0, 6] = calculate(LevelPrv[0, 6], debtPrvYr, arith);
                            break;
                        case 7:
                            LevelPrv[0, 1] = calculate(LevelPrv[0, 1], debtPrvYr, arith);
                            LevelPrv[0, 2] = calculate(LevelPrv[0, 2], debtPrvYr, arith);
                            LevelPrv[0, 3] = calculate(LevelPrv[0, 3], debtPrvYr, arith);
                            LevelPrv[0, 4] = calculate(LevelPrv[0, 4], debtPrvYr, arith);
                            LevelPrv[0, 5] = calculate(LevelPrv[0, 5], debtPrvYr, arith);
                            LevelPrv[0, 6] = calculate(LevelPrv[0, 6], debtPrvYr, arith);
                            LevelPrv[0, 7] = calculate(LevelPrv[0, 7], debtPrvYr, arith);
                            break;
                        case 8:
                            LevelPrv[0, 1] = calculate(LevelPrv[0, 1], debtPrvYr, arith);
                            LevelPrv[0, 2] = calculate(LevelPrv[0, 2], debtPrvYr, arith);
                            LevelPrv[0, 3] = calculate(LevelPrv[0, 3], debtPrvYr, arith);
                            LevelPrv[0, 4] = calculate(LevelPrv[0, 4], debtPrvYr, arith);
                            LevelPrv[0, 5] = calculate(LevelPrv[0, 5], debtPrvYr, arith);
                            LevelPrv[0, 6] = calculate(LevelPrv[0, 6], debtPrvYr, arith);
                            LevelPrv[0, 7] = calculate(LevelPrv[0, 7], debtPrvYr, arith);
                            LevelPrv[0, 8] = calculate(LevelPrv[0, 8], debtPrvYr, arith);
                            break;
                        case 9:
                            LevelPrv[0, 1] = calculate(LevelPrv[0, 1], debtPrvYr, arith);
                            LevelPrv[0, 2] = calculate(LevelPrv[0, 2], debtPrvYr, arith);
                            LevelPrv[0, 3] = calculate(LevelPrv[0, 3], debtPrvYr, arith);
                            LevelPrv[0, 4] = calculate(LevelPrv[0, 4], debtPrvYr, arith);
                            LevelPrv[0, 5] = calculate(LevelPrv[0, 5], debtPrvYr, arith);
                            LevelPrv[0, 6] = calculate(LevelPrv[0, 6], debtPrvYr, arith);
                            LevelPrv[0, 7] = calculate(LevelPrv[0, 7], debtPrvYr, arith);
                            LevelPrv[0, 8] = calculate(LevelPrv[0, 8], debtPrvYr, arith);
                            LevelPrv[0, 9] = calculate(LevelPrv[0, 9], debtPrvYr, arith);
                            break;
                    }

                    //----------for Credit Previous Year
                    switch (tLevel)
                    {
                        case 1:
                            LevelPrv[1, 1] = calculate(LevelPrv[1, 1], creditPrvYr, arith);
                            break;
                        case 2:
                            LevelPrv[1, 1] = calculate(LevelPrv[1, 1], creditPrvYr, arith);
                            LevelPrv[1, 2] = calculate(LevelPrv[1, 2], creditPrvYr, arith);
                            break;
                        case 3:
                            LevelPrv[1, 1] = calculate(LevelPrv[1, 1], creditPrvYr, arith);
                            LevelPrv[1, 2] = calculate(LevelPrv[1, 2], creditPrvYr, arith);
                            LevelPrv[1, 3] = calculate(LevelPrv[1, 3], creditPrvYr, arith);
                            break;
                        case 4:
                            LevelPrv[1, 1] = calculate(LevelPrv[1, 1], creditPrvYr, arith);
                            LevelPrv[1, 2] = calculate(LevelPrv[1, 2], creditPrvYr, arith);
                            LevelPrv[1, 3] = calculate(LevelPrv[1, 3], creditPrvYr, arith);
                            LevelPrv[1, 4] = calculate(LevelPrv[1, 4], creditPrvYr, arith);
                            break;
                        case 5:
                            LevelPrv[1, 1] = calculate(LevelPrv[1, 1], creditPrvYr, arith);
                            LevelPrv[1, 2] = calculate(LevelPrv[1, 2], creditPrvYr, arith);
                            LevelPrv[1, 3] = calculate(LevelPrv[1, 3], creditPrvYr, arith);
                            LevelPrv[1, 4] = calculate(LevelPrv[1, 4], creditPrvYr, arith);
                            LevelPrv[1, 5] = calculate(LevelPrv[1, 5], creditPrvYr, arith);
                            break;
                        case 6:
                            LevelPrv[1, 1] = calculate(LevelPrv[1, 1], creditPrvYr, arith);
                            LevelPrv[1, 2] = calculate(LevelPrv[1, 2], creditPrvYr, arith);
                            LevelPrv[1, 3] = calculate(LevelPrv[1, 3], creditPrvYr, arith);
                            LevelPrv[1, 4] = calculate(LevelPrv[1, 4], creditPrvYr, arith);
                            LevelPrv[1, 5] = calculate(LevelPrv[1, 5], creditPrvYr, arith);
                            LevelPrv[1, 6] = calculate(LevelPrv[1, 6], creditPrvYr, arith);
                            break;
                        case 7:
                            LevelPrv[1, 1] = calculate(LevelPrv[1, 1], creditPrvYr, arith);
                            LevelPrv[1, 2] = calculate(LevelPrv[1, 2], creditPrvYr, arith);
                            LevelPrv[1, 3] = calculate(LevelPrv[1, 3], creditPrvYr, arith);
                            LevelPrv[1, 4] = calculate(LevelPrv[1, 4], creditPrvYr, arith);
                            LevelPrv[1, 5] = calculate(LevelPrv[1, 5], creditPrvYr, arith);
                            LevelPrv[1, 6] = calculate(LevelPrv[1, 6], creditPrvYr, arith);
                            LevelPrv[1, 7] = calculate(LevelPrv[1, 7], creditPrvYr, arith);
                            break;
                        case 8:
                            LevelPrv[1, 1] = calculate(LevelPrv[1, 1], creditPrvYr, arith);
                            LevelPrv[1, 2] = calculate(LevelPrv[1, 2], creditPrvYr, arith);
                            LevelPrv[1, 3] = calculate(LevelPrv[1, 3], creditPrvYr, arith);
                            LevelPrv[1, 4] = calculate(LevelPrv[1, 4], creditPrvYr, arith);
                            LevelPrv[1, 5] = calculate(LevelPrv[1, 5], creditPrvYr, arith);
                            LevelPrv[1, 6] = calculate(LevelPrv[1, 6], creditPrvYr, arith);
                            LevelPrv[1, 7] = calculate(LevelPrv[1, 7], creditPrvYr, arith);
                            LevelPrv[1, 8] = calculate(LevelPrv[1, 8], creditPrvYr, arith);
                            break;
                        case 9:
                            LevelPrv[1, 1] = calculate(LevelPrv[1, 1], creditPrvYr, arith);
                            LevelPrv[1, 2] = calculate(LevelPrv[1, 2], creditPrvYr, arith);
                            LevelPrv[1, 3] = calculate(LevelPrv[1, 3], creditPrvYr, arith);
                            LevelPrv[1, 4] = calculate(LevelPrv[1, 4], creditPrvYr, arith);
                            LevelPrv[1, 5] = calculate(LevelPrv[1, 5], creditPrvYr, arith);
                            LevelPrv[1, 6] = calculate(LevelPrv[1, 6], creditPrvYr, arith);
                            LevelPrv[1, 7] = calculate(LevelPrv[1, 7], creditPrvYr, arith);
                            LevelPrv[1, 8] = calculate(LevelPrv[1, 8], creditPrvYr, arith);
                            LevelPrv[1, 9] = calculate(LevelPrv[1, 9], creditPrvYr, arith);
                            break;
                    }
                    //-------------------------------------

                    if (a.PrintLevel != 0)
                    {
                        dataRow["printDebit"] = Level[0, Convert.ToInt32(a.PrintLevel)];
                        dataRow["printCredit"] = Level[1, Convert.ToInt32(a.PrintLevel)];

                    }
                    else
                    {
                        //dataRow["print"] = 0;
                    }


                    //------------------------For Previous Year---------

                    if (a.PrintLevel != 0)
                    {
                        dataRow["printDebitPrvYear"] = LevelPrv[0, Convert.ToInt32(a.PrintLevel)];
                        dataRow["printCreditPrvYear"] = LevelPrv[1, Convert.ToInt32(a.PrintLevel)];

                    }
                    else
                    {
                        //dataRow["print"] = 0;
                    }

                    //------------------------

                    dtable.Rows.Add(dataRow);

                    //-----------for skip--
                    if (a.SkipA4 != 0)
                    {
                        int i = 0;
                        while (i < a.SkipA4)
                        {
                            DataRow extraRow = dtable.NewRow();
                            dtable.Rows.Add(extraRow);
                            i++;
                        }
                        
                    }
                    //---------------------
                    
                    Level[0, Convert.ToInt32(a.InitLevel)] = 0;
                    Level[1, Convert.ToInt32(a.InitLevel)] = 0;




                    //----For Previous Year
                    LevelPrv[0, Convert.ToInt32(a.InitLevel)] = 0;
                    LevelPrv[1, Convert.ToInt32(a.InitLevel)] = 0;

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
                ucMessage.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
                //throw ex;
            }
        }

        //--------------------------------
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
        //----------------------------------------

        #endregion


        [WebMethod]
        public static List<string> GetNames(string sname)
        {
            List<string> strNamesData = new List<string>();
            strNamesData.Add("Jitendra");
            strNamesData.Add("Manoranjan");
            strNamesData.Add("Santosh");
            strNamesData.Add("Minal");
            strNamesData.Add("Lovely");
            strNamesData.Add("Vinit");
            strNamesData.Add("Nimesh");
            strNamesData.Add("Sonal");
            strNamesData.Add("Sachin");
            strNamesData.Add("Manish");
            var namelist = strNamesData.Where(n => n.ToLower().StartsWith(sname.ToLower()));
            return namelist.ToList();
        }
    }
}

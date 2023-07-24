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

namespace RMS.GL.Setup
{

    public partial class frmReportParameterView : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                PID = Convert.ToInt32(Request.QueryString["PID"]);
                //if (PID == 318)
                //{
                btnClear.Visible = false;
                btnSave.Visible = false;

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
                        Session["PageTitle"] = "Report Parameter (View)";
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
            Response.Redirect("~/GLSetup/frmReportParameter.aspx?PID=318");
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gRow = (GridViewRow)((Control)sender).NamingContainer as GridViewRow;
            DropDownList ddlType = (DropDownList)gRow.FindControl("ddlType");
            if (ddlType.SelectedValue == "H")
            {
                gRow.Cells[3].Enabled = false;
                gRow.Cells[4].Enabled = false;
                gRow.Cells[5].Enabled = false;
                gRow.Cells[6].Enabled = false;
                gRow.Cells[7].Enabled = false;
                gRow.Cells[8].Enabled = false;
            }
            else
            {
                gRow.Cells[3].Enabled = true;
                gRow.Cells[4].Enabled = true;
                gRow.Cells[5].Enabled = true;
                gRow.Cells[6].Enabled = true;
                gRow.Cells[7].Enabled = true;
                gRow.Cells[8].Enabled = true;
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
           
        }

        private void bindCurrentTable()
        {
            getColumn();
            for (int i = 1; i < 4; i++)
            {
                drow = d_table.NewRow();
                drow["RecNo"] = i;
                d_table.Rows.Add(drow);
            }
            CurrentTable = d_table;
        }

        #region helping method

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

                    glrpt.CompLevelRpt =Convert.ToChar( ddlCompLevel.SelectedValue);

                    glrpt.PrintDrCr = Convert.ToChar(ddlPrintDrCr.SelectedValue);
                    glrpt.PrintPrvYr = Convert.ToChar(ddlPrintPreYr.SelectedValue);
                    glrpt.updateby = Session["UserID"].ToString();
                    glrpt.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    Int16 RecNo = 1;

                    for (int i = 0; i < grdView.Rows.Count; i++)
                    {
                        // Int16 RecNo = Convert.ToInt16(((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[0].FindControl("txtRecNo")).Text);
                        string Type = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlType")).SelectedValue;
                        string Narration = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[2].FindControl("txtNarration")).Text;
                        string AcFr = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[3].FindControl("txtAcFrom")).Text;
                        string AcTo = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[4].FindControl("txtAcTo")).Text;

                        string ArthOp = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlArithOp")).SelectedValue;

                        string totalLevel = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[5].FindControl("txtTotalLevel")).Text;
                        string printL = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[7].FindControl("txtTotalLevel")).Text;
                        string initialization = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[8].FindControl("txtInit")).Text;
                        
                        if (Type != "0" && Narration != "")
                        {
                            tblglrptdet rptDet = new tblglrptdet();
                            rptDet.CompId =compId;
                            rptDet.RptNo = Convert.ToInt16(txtReportNo.Text);
                            rptDet.RecNo = RecNo;
                            rptDet.RecType = Convert.ToChar(Type);
                            rptDet.Recdesc = Narration;
                            rptDet.ActFrom = AcFr;
                            rptDet.ActTo = AcTo;
                            if (totalLevel != "")
                            {
                                rptDet.TotalLevel = Convert.ToDecimal(totalLevel);
                            }
                            rptDet.ArithOp = ArthOp;
                            if (printL != "")
                            {
                                rptDet.PrintLevel = Convert.ToDecimal(printL);
                            }
                            if (initialization != "")
                            {
                                rptDet.InitLevel = Convert.ToDecimal(initialization);
                            }

                            glrptDetEntSet.Add(rptDet);

                            RecNo++;

                        }


                    }
                    if (glParameter.save(glrpt, glrptDetEntSet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]))
                    {
                        ucMessage.ShowMessage("Saved Successfully...", RMS.BL.Enums.MessageType.Info);
                        glrptDetEntSet.Clear();
                    }

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
                    Int16 RecNo = 1;

                    for (int i = 0; i < grdView.Rows.Count; i++)
                    {
                        // Int16 RecNo = Convert.ToInt16(((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[0].FindControl("txtRecNo")).Text);
                        string Type = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlType")).SelectedValue;
                        string Narration = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[2].FindControl("txtNarration")).Text;
                        string AcFr = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[3].FindControl("txtAcFrom")).Text;
                        string AcTo = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[4].FindControl("txtAcTo")).Text;

                        string ArthOp = ((System.Web.UI.WebControls.DropDownList)grdView.Rows[i].FindControl("ddlArithOp")).SelectedValue;

                        string totalLevel = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[5].FindControl("txtTotalLevel")).Text;
                        
                        string printL = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[7].FindControl("txtTotalLevel")).Text;
                        string initialization = ((System.Web.UI.WebControls.TextBox)grdView.Rows[i].Cells[8].FindControl("txtInit")).Text;
                        
                        if (Type != "0" && Narration != "")
                        {
                            tblglrptdet rptDet = new tblglrptdet();
                            rptDet.CompId = compId;
                            rptDet.RptNo = Convert.ToInt16(txtReportNo.Text);
                            rptDet.RecNo = RecNo;
                            rptDet.RecType = Convert.ToChar(Type);
                            rptDet.Recdesc = Narration;
                            rptDet.ActFrom = AcFr;
                            rptDet.ActTo = AcTo;
                            if (totalLevel != "")
                            {
                               rptDet.TotalLevel = Convert.ToDecimal(totalLevel);
                            }
                            rptDet.ArithOp = ArthOp;
                            if (printL != "")
                            {
                                rptDet.PrintLevel =Convert.ToDecimal(printL);
                            }
                            if (initialization != "")
                            {
                                rptDet.InitLevel = Convert.ToDecimal(initialization);
                            }

                            glrptDetEntSet.Add(rptDet);

                            RecNo++;

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
            List<tblglrptdet> ls=glParameter.getDetailByRptNo(rptNo,compId,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            foreach(var a in ls )
            {
                drow = d_table.NewRow();
                drow["RecNo"] = a.RecNo;
                drow["Type"] = a.RecType;
                drow["Narration"] = a.Recdesc;
                drow["AcFrom"] = a.ActFrom;
                drow["AcTo"] = a.ActTo;
                drow["TotalLevel"] = a.TotalLevel;
                drow["ArthOp"] = a.ArithOp;
                drow["PrintLevel"] = a.PrintLevel;
                drow["Init"] = a.InitLevel;
                
                
                d_table.Rows.Add(drow);
            }
            CurrentTable = d_table;

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
                        if (CurrentTable.Rows[i]["Type"].ToString() == "H")
                        {
                            grdView.Rows[i].Cells[3].Enabled = false;
                            grdView.Rows[i].Cells[4].Enabled = false;
                            grdView.Rows[i].Cells[5].Enabled = false;
                            grdView.Rows[i].Cells[6].Enabled = false;
                            grdView.Rows[i].Cells[7].Enabled = false;
                            grdView.Rows[i].Cells[8].Enabled = false;
                        }
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

        #endregion



    }
}

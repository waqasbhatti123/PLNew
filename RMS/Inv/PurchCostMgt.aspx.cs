using System;
using RMS.BL;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Text.RegularExpressions;
using AjaxControlToolkit;
using System.Web.UI; 

namespace RMS.Inv
{
    public partial class PurchCostMgt : System.Web.UI.Page
    {
        #region DataMembers

        MPNBL mpnBl = new MPNBL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        EntitySet<tblStkCostDet> entCostDet = new EntitySet<tblStkCostDet>();

        #endregion

        #region Properties

        public int PID
        {
            get { return Convert.ToInt32(ViewState["PID"]); }
            set { ViewState["PID"] = value; }
        }

        public DataTable CurrentTable
        {
            set { ViewState["CurrentTable"] = value; }
            get { return (DataTable)(ViewState["CurrentTable"] ?? 0); }
        }

        public int MPN_No
        {
            set { ViewState["MPN_No"] = value; }
            get { return Convert.ToInt32(ViewState["MPN_No"]); }
        }

        public string DocRefer
        {
            set { ViewState["DocRefer"] = value; }
            get { return Convert.ToString(ViewState["DocRefer"]); }
        }

        public bool Result
        {
            set { ViewState["Results"] = value; }
            get { return Convert.ToBoolean(ViewState["Results"]); }
        }

        public decimal ChargedCost
        {
            get { return Convert.ToDecimal(ViewState["ChargedCost"] ?? 0); }
            set { ViewState["ChargedCost"] = value; }
        }

        public decimal NotCharged
        {
            get { return Convert.ToDecimal(ViewState["NotCharged"] ?? 0); }
            set { ViewState["NotCharged"] = value; }
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
                //if (Session["DateFormat"] == null)
                //{
                //    C1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                //}
                //else
                //{
                //    C1.Format = Session["DateFormat"].ToString();
                //}

                PID = Convert.ToInt32(Request.QueryString["PID"]);
                if (PID == 514)
                {
                    lblTitle.Text =  GetGlobalResourceObject("PageTitlesResource", "PurchaseCostSheet").ToString();

                    if (Session["VrNo"] != null)
                    {
                        string mpn = Convert.ToString(Session["VrNo"]);
                        MPN_No = Convert.ToInt32(mpn.Substring(0, 4) + mpn.Substring(5));
                    }
                        //txtDocNo.Text = Convert.ToString(Session["VrNo"]);
                        //txtDocDate.Text = Convert.ToString(Session["VrDate"]);
                        ddlVendor.SelectedValue = Convert.ToString(Session["VendorId"]);
                        DocRefer = Convert.ToString(Session["DocRef"]);
                        //MPN_No = Convert.ToInt32(txtDocNo.Text.Substring(0, 4) + txtDocNo.Text.Substring(5));

                        if (Session["MPNStatus"].ToString() == "A")
                        {
                            btnAdd.Visible = false;
                            pnlCost.Enabled = false;
                        }
                    //}
                    //else
                    //{See here
                        //btnAdd.Visible = false;
                    //}
                    BindDDLVendor();
                    BindTable();
                    Result = false;
                    
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    ucMessage.ShowMessage("Connection disconnected.", RMS.BL.Enums.MessageType.Error);
                }
            }

           Result = CheckData();
           if (Result == true)
           {
               Result = false;
               SaveCost();
           }
           else
           {
               ucMessage.ShowMessage("Please enter atleast one entry.", RMS.BL.Enums.MessageType.Error);
           }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            ClearAll();
            Session["VrNo"] = null;
            //Session["VrDate"] = null;
            Session["VendorId"] = null;
            Session["DocRef"] = null;
            Session["StkCost"] = null;
            Session["StkCostDet"] = null;
            Session["MPNStatus"] = null;
            Session["ChargedCost"] = null;
            Session["NotCharged"] = null;
            Response.Write("<script language= 'javascript'> {self.close()}</script>");
        }

        #endregion

        #region Helping Methods

        public void SaveCost()
        {
            try
            {
                ChargedCost = 0;
                string stts ="";
                int count = 1;
                //tblStkCost=====================================
                tblStkCost cost = new tblStkCost();
                cost.vr_id = 0;
                cost.vr_dt = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                cost.DocRef = DocRefer;
                cost.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date;
                if (Session["LoginID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        cost.updateby = Request.Cookies["uzr"]["LoginID"];
                    }
                }
                else
                {
                    cost.updateby = Session["LoginID"].ToString();
                }
                cost.post_2gl = true;

                //tblStkCost_Det===================================

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    TextBox txtAmnt = (TextBox)(GridView1.Rows[i].FindControl("txtAmount"));
                    if (txtAmnt.Text != "")
                    {
                        if (Convert.ToDecimal(txtAmnt.Text) > 0)
                        {
                            tblStkCostDet costDet = new tblStkCostDet();
                            costDet.vr_id = 0;
                            costDet.vr_seq = Convert.ToByte(count);
                            
                            costDet.CostId = Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtCostId"))).Text);
                            
                            costDet.DocRef = ((TextBox)(GridView1.Rows[i].FindControl("txtDocRef"))).Text;
                            try
                            {
                                if (((TextBox)(GridView1.Rows[i].FindControl("txtDate"))).Text != "")
                                {
                                    costDet.DocRef_Date = Convert.ToDateTime(((TextBox)(GridView1.Rows[i].FindControl("txtDate"))).Text);
                                    costDet.Pay_Date = Convert.ToDateTime(((TextBox)(GridView1.Rows[i].FindControl("txtDate"))).Text);
                                }
                            }
                            catch (Exception ex)
                            {
                                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
                            }
                            costDet.Paid_Amt = Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtAmount"))).Text);
                            costDet.Claim_Amt = Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtAmount"))).Text);
                            costDet.vr_rmk = ((TextBox)(GridView1.Rows[i].FindControl("txtRem"))).Text;


                            //Getting cost when column 'Charg_Cost' in tblCost_TempDet is 'Y'
                            stts = mpnBl.GetChargeCostStts(Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtCostId"))).Text), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (stts != "-")
                            {
                                if (stts == "Y")
                                {
                                    ChargedCost = ChargedCost + Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtAmount"))).Text);
                                }
                                else
                                {
                                    NotCharged = NotCharged + Convert.ToDecimal(((TextBox)(GridView1.Rows[i].FindControl("txtAmount"))).Text);
                                }
                            }

                            entCostDet.Add(costDet);
                            count++;
                        }
                    }
                }
                //To Save or Put In Session=================================
                if (entCostDet.Count > 0 )
                {
                    Session["StkCost"] = cost;
                    Session["StkCostDet"] = entCostDet;
                    Session["ChargedCost"] = ChargedCost;
                    Session["NotCharged"] = NotCharged;

                    ClearAll();
                    Response.Write("<script language= 'javascript'> {self.close()}</script>");

                    //=================================================

                    //Result = mpnBl.SaveCostSheet(cost, entCostDet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    //if (Result == true)
                    //{
                    //    Result = false;
                    //    ClearAll();
                    //    ucMessage.ShowMessage("Saved successfully.", RMS.BL.Enums.MessageType.Info);
                    //}
                    //else
                    //{
                    //    ucMessage.ShowMessage("Canot save at this time.", RMS.BL.Enums.MessageType.Error);
                    //}
                }
                else
                {
                    ucMessage.ShowMessage("Cannot add, internal error occured.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage("Exception: "+ ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public bool CheckData()
        {
            bool res= false;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
               TextBox txtAmnt = (TextBox)(GridView1.Rows[i].FindControl("txtAmount"));
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

        public void BindTable()
        {
            GetColumnsIGP();
            int brid=0;
            if (Session["BranchID"] == null)
                {
                    brid = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"].ToString());
                }
                else
                {
                    brid = Convert.ToInt32(Session["BranchID"].ToString());
            }
            string potyp = mpnBl.GetPOType(brid, Convert.ToInt32(Session["POREF"]), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            /****************************************************************/
            List<tblStkCostDet> lstStk = mpnBl.GetPreviousRec(MPN_No, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblCost_TempDet> lst = mpnBl.GetCostIdDet(potyp, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            

            if (lstStk != null && lstStk.Count > 0)
            {
                foreach (var l in lst)
                {
                    d_Row = d_table.NewRow();
                    bool found = false;
                    for (int j = 0; j < lstStk.Count; j++)
                    {
                        if (l.CostId == lstStk[j].CostId)
                        {
                            d_Row["CostId"] = l.CostId;
                            d_Row["TempId"] = l.TempId;
                            d_Row["Desc"] = l.Cost_Desc;
                            if (lstStk[j].DocRef_Date != null)
                            {
                                if (Session["DateFormat"] == null)
                                {
                                    d_Row["Date"] = lstStk[j].DocRef_Date.Value.ToString(Request.Cookies["uzr"]["DateFormat"].ToString());
                                }
                                else
                                {
                                    d_Row["Date"] = lstStk[j].DocRef_Date.Value.ToString(Session["DateFormat"].ToString());
                                }
                            }
                            else
                            {
                                d_Row["Date"] = "";
                            }
                            d_Row["DocRef"] = lstStk[j].DocRef;
                            d_Row["Amount"] = Convert.ToString(Convert.ToInt32(Convert.ToDecimal(lstStk[j].Paid_Amt.Value)));
                            d_Row["Remarks"] = lstStk[j].vr_rmk;
                            found = true;
                        }
                        else if (found == false)
                        {
                            d_Row["CostId"] = l.CostId;
                            d_Row["TempId"] = l.TempId;
                            d_Row["Desc"] = l.Cost_Desc;
                            d_Row["Date"] = "";
                            d_Row["DocRef"] = "";
                            d_Row["Amount"] = "";
                            d_Row["Remarks"] = "";
                        }
                        else
                        {
                        }
                    }
                    d_table.Rows.Add(d_Row);
                }
            }
            else
            {
                foreach (var l in lst)
                {
                    d_Row = d_table.NewRow();

                    d_Row["CostId"] = l.CostId;
                    d_Row["TempId"] = l.TempId;
                    d_Row["Desc"] = l.Cost_Desc;
                    d_Row["Date"] = "";
                    d_Row["DocRef"] = "";
                    d_Row["Amount"] = "";
                    d_Row["Remarks"] = "";

                    d_table.Rows.Add(d_Row);
                }
            }
            CurrentTable = d_table;
            BindGrid();
        }

        public void GetColumnsIGP()
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

        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }

        public void BindDDLVendor()
        {
            ddlVendor.DataSource = mpnBl.GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.DataTextField = "gl_dsc";
            ddlVendor.DataValueField = "gl_cd";
            ddlVendor.DataBind();
        }

        public void ClearAll()
        {
            btnAdd.Visible = false;
            //txtDocNo.Text = "";
            //txtDocDate.Text = "";
            ChargedCost = 0;
            NotCharged = 0;
            ddlVendor.SelectedIndex = 0;
            BindTable();
        }

        #endregion
    }
}

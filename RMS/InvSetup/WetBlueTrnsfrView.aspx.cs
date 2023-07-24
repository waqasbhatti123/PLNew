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

namespace RMS
{
    public partial class WetBlueTrnsfrView : BasePage
    {
        #region DataMembers
        EntitySet<tblItemDataDet> itmDetEnt = new EntitySet<tblItemDataDet>();
        InvMN_BL mN = new InvMN_BL();

        #endregion

        #region Properties

        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
        
        public DataTable CurrentTable
        {
            set { ViewState["CurrentTable"] = value; }
            get { return (DataTable)(ViewState["CurrentTable"] ?? 0); }
        }

        public string Item_Code
        {
            set { ViewState["Item_Code"] = value; }
            get { return Convert.ToString(ViewState["Item_Code"]); }
        }

        public int vrCode
        {
            set { ViewState["vrCode"] = value; }
            get { return Convert.ToInt32(ViewState["vrCode"] ?? 0); }
        }

        public int locID
        {
            set { ViewState["locID"] = value; }
            get { return Convert.ToInt32(ViewState["locID"] ?? 0); }
        }

        public int ToLocID
        {
            set { ViewState["ToLocID"] = value; }
            get { return Convert.ToInt32(ViewState["ToLocID"] ?? 0); }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                if (Request.Cookies["uzr"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            vrCode = 24;
            locID = 2;
            ToLocID = 3;
            if (!IsPostBack)
            {
                int GroupID = 0;

                if (Session["GroupID"] == null)
                {
                    if (Request.Cookies["uzr"] == null)
                    {
                        Response.Redirect("~/login.aspx");
                    }
                    GroupID = Convert.ToInt32(Request.Cookies["uzr"]["GroupID"]);
                }
                else
                {
                    GroupID = Convert.ToInt32(Session["GroupID"].ToString());
                }

                if (Session["DateFormat"] == null)
                {
                    dateExtender.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    dateExtender.Format = Session["DateFormat"].ToString();
                }
                dateExtender.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                
                BindDDLItem();
                BindDDLDesign();
                BindDDLColor();
                BindDDLThickness();
                

                if (Convert.ToBoolean(Session["IfEdit"]) == false)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "WetBlueTrnsfrNt").ToString();

                    GetWetBlueTrnsfrNo();
                    txtQty.Text = "0";
                    txtHalfQty.Text = "0";
                    btnBack.Visible = false;
                    btnClear.Visible = true;
                    btnSave.Visible = false;
                    btnList.Visible = true;
                    rbdPending.Checked = true;
                }
                else
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", Session["PgTile"].ToString()).ToString();
                    BindEditFields();
                    BindEditTable();
                    ddlItem.Enabled = false;
                    btnClear.Visible = false;
                    btnBack.Visible = false;
                    btnList.Visible = true;
                    btnSave.Visible = false;
                }

            }
      
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue != "0")
            {
                Item_Code = ddlItem.SelectedValue;
                BindTable();
                btnSave.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/WetBlueTrnsfrMgt.aspx?PID=440");
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/wetbluetrnsfrhome.aspx?PID=425");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["IfEdit"]) == false)
            {
                Save();
            }
            else
            {
                Edit();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["PrePgAddress"].ToString());
        }

        #endregion
        
        #region Helping Method

        public void BindEditFields()
        {
            int count=0;
            int qty = 0;
            int halfqty = 0;
            string vrNo = Session["vrNo"].ToString();
            vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);
            int yr = Convert.ToInt32(vrNo.Substring(0, 4));
            tblItemData record = mN.GetRecWetBlueTrnsfr(Convert.ToInt32(vrNo), yr, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblItemDataDet> recordsDet = mN.GetRecDetWetBlueTrnsfr(Convert.ToInt32(vrNo), yr, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);           

            txtTrnsfrNo.Text = record.vr_no.ToString().Substring(0,4)+"/" +record.vr_no.ToString().Substring(4);
            dateExtender.SelectedDate = record.vr_dt;
            
            if (record.vr_apr == "A")
            {
                rbdApprove.Checked = true;
            }
            else if (record.vr_apr == "P")
            {
                rbdPending.Checked = true;
            }
            else
            {
                rbdCancelled.Checked = true;
            }
            foreach(var rec in recordsDet)
            {
                if(count == 0)
                {
                    ddlItem.SelectedValue = rec.itm_cd;
                    Item_Code = rec.itm_cd;
                    ddlDesign.SelectedValue = rec.DesignId;
                    ddlColor.SelectedValue = rec.ColorId;
                    ddlThickness.SelectedValue = rec.ThickId.ToString();
                    txtLotRef.Text = rec.LotRef.Value.ToString();
                    count++;
                }
                qty = qty + Convert.ToInt32(rec.vr_qty);
                halfqty = halfqty + Convert.ToInt32(rec.vr_half);
            }
            txtQty.Text = qty.ToString();
            txtHalfQty.Text = halfqty.ToString();
        }

        public void BindEditTable()
        {
            GetColumns();
            string selection = "";
            string gradeSelection = "";
            int count = 0;
            
            string vrNo = Session["vrNo"].ToString();
            vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);
            int yr = Convert.ToInt32(vrNo.Substring(0, 4));

            List<spGetAvgSqftResult> avgList = mN.GetAvgSqft(Item_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<string> lst = mN.GetSelectionGrade(Item_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblItemDataDet> recordsDet = mN.GetRecDetWetBlueTrnsfr(Convert.ToInt32(vrNo), yr, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            for (int i = 0; i < lst.Count(); i++)
            {

                string[] parts = Regex.Split(lst[i], ",");
                foreach (string part in parts)
                {
                    if (count == 0)
                    {
                        selection = part;
                    }
                    else
                    {
                        gradeSelection = part;
                    }
                    count++;
                }
                count = 0;
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;
                d_Row["Selection"] = selection;
                d_Row["SelectionVal"] = gradeSelection;


                string gradeId = "";
                string selectionId = "";
                int count1 = 0;
                string[] partss = Regex.Split(gradeSelection, ":");
                foreach (string part in partss)
                {
                    if (count1 == 0)
                    {
                        gradeId = part;
                    }
                    else
                    {
                        selectionId = part;
                    }
                    count1++;
                }

                //foreach (var l in avgList)
                //{
                //    if (gradeSelection == l.GradeSelection)
                //    {
                //        d_Row["Area"] = Math.Round(Convert.ToDecimal(l.Sqft) / Convert.ToDecimal(l.Qty), 2).ToString();
                //    }
                //}

                for (int j = 0; j < recordsDet.Count(); j++)
                {
                    if (recordsDet[j].GradeId == gradeId && recordsDet[j].SelectionId == selectionId)
                    {
                        d_Row["Quantity"] = recordsDet[j].vr_qty;
                        d_Row["HalfQuantity"] = recordsDet[j].vr_half;
                       if(recordsDet[j].vr_qty > 0)
                        d_Row["Area"] =Math.Round( Convert.ToDecimal( recordsDet[j].Feetage / recordsDet[j].vr_qty), 2);
                        else
                           d_Row["Area"]= Math.Round( Convert.ToDecimal( recordsDet[j].Feetage),2);
                    }
                }


                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }

        public void Edit()
        {
            try
            {

                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtDate.Text);

                string no = txtTrnsfrNo.Text.Substring(5);
                string yrno = txtTrnsfrNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                itmData.vt_cd = Convert.ToInt16(vrCode);
                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);

                itmData.vr_nrtn = "";
                itmData.gl_cd = "";
                itmData.post_2gl = true;
                itmData.LocId = Convert.ToInt16(locID);

                itmData.LotNo = 0;


                itmData.ToLocId = Convert.ToInt16(ToLocID);
                itmData.Freight = 0;
                itmData.Tax = 0;
                itmData.Commission = 0;

                if (rbdApprove.Checked == true)
                {
                    itmData.vr_apr = "A";
                }
                else if (rbdPending.Checked == true)
                {
                    itmData.vr_apr = "P";
                }
                else
                {
                    itmData.vr_apr = "C";
                }
                itmData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (rbdApprove.Checked == true || rbdCancelled.Checked == true)
                {
                    if (Session["UserName"] == null)
                        itmData.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itmData.updateby = Session["UserName"].ToString();
                }
                itmData.Status = "";
                itmData.DocRef = "";


                int srCount = 1;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {

                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text;
                    string halfqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtHalfQuantity")).Text;
                    string ftg = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[4].FindControl("txtArea")).Text;
                    if (qty == "")
                        qty = "0";
                    if (halfqty == "")
                        halfqty = "0";
                    //if ((rbdPending.Checked == true) ? (qty != "" || ftg != "") : (Convert.ToInt32(qty) > 0))
                    if ((rbdPending.Checked == true) ? (qty != "" || halfqty != "" || ftg != "0") : (Convert.ToInt32(qty) > 0 || Convert.ToInt32(halfqty) > 0))
                    {

                        tblItemDataDet itmDet = new tblItemDataDet();
                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = Convert.ToInt16(vrCode);
                        string nos = txtTrnsfrNo.Text.Substring(5);
                        string yrnos = txtTrnsfrNo.Text.Substring(0, 4) + no;
                        itmDet.vr_no = Convert.ToInt32(yrnos);

                        itmDet.vr_seq = Convert.ToByte(srCount++);
                        itmDet.itm_cd = Item_Code;
                        itmDet.LocId = Convert.ToInt16(locID);
                        string txt1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text;
                        if (txt1 != "")
                        {
                            itmDet.vr_qty = Convert.ToInt32(txt1);
                        }
                        else
                        {
                            itmDet.vr_qty = 0;
                        }

                        string txt3 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtHalfQuantity")).Text;
                        if (txt3 != "")
                        {
                            itmDet.vr_half = Convert.ToInt32(txt3);
                        }
                        else
                        {
                            itmDet.vr_half = 0;
                        }

                        itmDet.KGSwt = 0;
                        itmDet.vr_val = 0;
                        itmDet.vr_rate = 0;
                        string txt2 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[4].FindControl("txtArea")).Text;
                        if (txt2 != "" && Convert.ToInt32(txt1) > 0)
                        {
                            itmDet.Feetage =    Math.Round(Convert.ToDecimal(txt2) * Convert.ToInt32(txt1), 5);
                        }
                        else
                        {
                            if (txt2 != "" && Convert.ToInt32(txt1) == 0)
                            {
                                itmDet.Feetage = Math.Round(Convert.ToDecimal(txt2), 2);
                            }
                            else
                            {
                                itmDet.Feetage = 0;
                            }
                        }

                        itmDet.IGP_Ref = 0;

                        itmDet.vr_rmk = "";
                        itmDet.GradeId = "";

                        if (txtLotRef.Text != "")
                        {
                            itmDet.LotRef = Convert.ToInt32(txtLotRef.Text);
                        }
                        else
                        {
                            itmDet.LotRef = 0;
                        }

                        string gradIdSelctionId = ((System.Web.UI.WebControls.HiddenField)GridView1.Rows[i].Cells[1].FindControl("hidnSelection")).Value;


                        string gradeId = "";
                        string selectionId = "";
                        int count = 0;
                        string[] parts = Regex.Split(gradIdSelctionId, ":");
                        foreach (string part in parts)
                        {
                            if (count == 0)
                            {
                                gradeId = part;
                            }
                            else
                            {
                                selectionId = part;
                            }
                            count++;
                        }

                        itmDet.DesignId = ddlDesign.SelectedValue;
                        itmDet.ColorId = ddlColor.SelectedValue;
                        itmDet.ThickId = Convert.ToInt16(ddlThickness.SelectedValue);

                        itmDet.GradeId = gradeId;
                        itmDet.SelectionId = selectionId;

                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        itmDet.DrumNo = 0;
                        itmDetEnt.Add(itmDet);
                    }
                }

                bool res = false;
                int sum = 0;
                int sumhalf = 0;
                for (int j = 0; j < GridView1.Rows.Count; j++)
                {
                    string txt = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].Cells[2].FindControl("txtQuantity")).Text;
                    if (txt != "")
                    {

                        sum = sum + Convert.ToInt32(txt);
                    }
                }
                for (int j = 0; j < GridView1.Rows.Count; j++)
                {
                    string txt = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].Cells[3].FindControl("txtHalfQuantity")).Text;
                    if (txt != "")
                    {

                        sumhalf = sumhalf + Convert.ToInt32(txt);
                    }
                }
                if (sum == Convert.ToInt32(txtQty.Text) && sumhalf == Convert.ToInt32(txtHalfQty.Text))
                {
                    string vrNo = Session["vrNo"].ToString();
                    vrNo = vrNo.Substring(0, 4) + vrNo.Substring(5);
                    int yr = Convert.ToInt32(vrNo.Substring(0, 4));

                    if (itmDetEnt.Count() > 0)
                        res = mN.EditRecWetBlu(Convert.ToInt32(vrNo), yr, vrCode, itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    else
                        res = false;


                    if (res == true)
                    {
                        ucMessage.ShowMessage("Updated successfully.", RMS.BL.Enums.MessageType.Info);
                        Response.Redirect("~/invsetup/wetbluetrnsfrhome.aspx?PID=425");
                    }
                    else
                    {
                        ucMessage.ShowMessage("Update was unsuccessful.", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Full Piece/Half Piece quantity should exactly be same to the quantity in parts.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage("Exception:"+ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void Save()
        {
            try
            {

                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtDate.Text);

                string no = txtTrnsfrNo.Text.Substring(5);
                string yrno = txtTrnsfrNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                itmData.vt_cd = Convert.ToInt16(vrCode);
                if (Session["BranchID"] == null)
                    itmData.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                else
                    itmData.br_id = Convert.ToInt32(Session["BranchID"]);

                itmData.vr_nrtn = "";
                itmData.gl_cd = "";
                itmData.post_2gl = true;
                itmData.LocId = Convert.ToInt16(locID);

                itmData.LotNo = 0;


                itmData.ToLocId = Convert.ToInt16(ToLocID);
                itmData.Freight = 0;
                itmData.Tax = 0;
                itmData.Commission = 0;

                if (rbdApprove.Checked == true)
                {
                    itmData.vr_apr = "A";
                }
                else if (rbdPending.Checked == true)
                {
                    itmData.vr_apr = "P";
                }
                else
                {
                    itmData.vr_apr = "C";
                }
                itmData.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (rbdApprove.Checked == true || rbdCancelled.Checked == true)
                {
                    if (Session["UserName"] == null)
                        itmData.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                    else
                        itmData.updateby = Session["UserName"].ToString();
                }
                itmData.Status = "";
                itmData.DocRef = "";


                int srCount = 1;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {

                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text;
                    string halfqty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtHalfQuantity")).Text;
                    string ftg = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[4].FindControl("txtArea")).Text;
                    if (qty == "")
                        qty = "0";
                    if (halfqty == "")
                        halfqty = "0";
                    //if (Convert.ToInt32(qty) > 0 || Convert.ToInt32(halfqty) >0 && ftg !="")
                    if((rbdPending.Checked == true)? (qty !="" || halfqty!= "" || ftg != "0") : (Convert.ToInt32(qty)>0 || Convert.ToInt32(halfqty)>0))
                    {

                        tblItemDataDet itmDet = new tblItemDataDet();
                        if (Session["BranchID"] == null)
                            itmDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        else
                            itmDet.br_id = Convert.ToInt32(Session["BranchID"]);
                        itmDet.vt_cd = Convert.ToInt16(vrCode);
                        string nos = txtTrnsfrNo.Text.Substring(5);
                        string yrnos = txtTrnsfrNo.Text.Substring(0, 4) + no;
                        itmDet.vr_no = Convert.ToInt32(yrnos);

                        itmDet.vr_seq = Convert.ToByte(srCount++);
                        itmDet.itm_cd = Item_Code;
                        itmDet.LocId = Convert.ToInt16(locID);
                        string txt1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text;
                        if (txt1 != "")
                        {
                            itmDet.vr_qty = Convert.ToInt32(txt1);
                        }
                        else
                        {
                            itmDet.vr_qty = 0;
                        }

                        string txt3 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtHalfQuantity")).Text;
                        if (txt3 != "")
                        {
                            itmDet.vr_half = Convert.ToInt32(txt3);
                        }
                        else
                        {
                            itmDet.vr_half = 0;
                        }

                        itmDet.KGSwt = 0;
                        itmDet.vr_val = 0;
                        itmDet.vr_rate = 0;
                        string txt2 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[4].FindControl("txtArea")).Text;
                        if (txt2 != "" && txt1 != "")
                        {
                            itmDet.Feetage = Math.Round(Convert.ToDecimal(txt2) * Convert.ToInt32(txt1), 5);
                        }
                        else
                        {
                            if (txt2 != "" && txt1 == "")
                            {
                                itmDet.Feetage = Math.Round(Convert.ToDecimal(txt2),2);
                            }
                            else
                            {
                                itmDet.Feetage = 0;
                            }
                        }

                        itmDet.IGP_Ref = 0;

                        itmDet.vr_rmk = "";
                        itmDet.GradeId = "";

                        if (txtLotRef.Text != "")
                        {
                            itmDet.LotRef = Convert.ToInt32(txtLotRef.Text);
                        }
                        else
                        {
                            itmDet.LotRef = 0;
                        }

                        string gradIdSelctionId = ((System.Web.UI.WebControls.HiddenField)GridView1.Rows[i].Cells[1].FindControl("hidnSelection")).Value;


                        string gradeId = "";
                        string selectionId = "";
                        int count = 0;
                        string[] parts = Regex.Split(gradIdSelctionId, ":");
                        foreach (string part in parts)
                        {
                            if (count == 0)
                            {
                                gradeId = part;
                            }
                            else
                            {
                                selectionId = part;
                            }
                            count++;
                        }

                        itmDet.DesignId = ddlDesign.SelectedValue;
                        itmDet.ColorId = ddlColor.SelectedValue;
                        itmDet.ThickId =Convert.ToInt16( ddlThickness.SelectedValue);
                        itmDet.GradeId = gradeId;
                        itmDet.SelectionId = selectionId;

                        itmDet.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        itmDet.DrumNo = 0;
                        itmDetEnt.Add(itmDet);
                    }
                }

                bool res = false;
                int sum = 0;
                int sumhalf = 0;
                for (int j = 0; j < GridView1.Rows.Count; j++)
                {
                    string txt = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].Cells[2].FindControl("txtQuantity")).Text;
                    if (txt != "")
                    {

                        sum = sum + Convert.ToInt32(txt);
                    }
                }
                for (int j = 0; j < GridView1.Rows.Count; j++)
                {
                    string txt = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].Cells[3].FindControl("txtHalfQuantity")).Text;
                    if (txt != "")
                    {

                        sumhalf = sumhalf + Convert.ToInt32(txt);
                    }
                }
                if (sum == Convert.ToInt32(txtQty.Text) && sumhalf == Convert.ToInt32(txtHalfQty.Text))
                {

                    if (itmDetEnt.Count() > 0)
                        res = mN.SaveItemData(itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    else
                        res = false;


                    if (res == true)
                    {
                        //if (rbdApprove.Checked == true)
                        //{
                        //    tblItem itm = new tblItem();

                        //    if (Session["BranchID"] == null)
                        //        itm.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                        //    else
                        //        itm.br_id = Convert.ToInt32(Session["BranchID"]);
                        //    itm.LocId =Convert.ToInt16( locID);
                        //    itm.itm_cd = ddlItem.SelectedValue;
                        //    itm.DesignId = ddlDesign.SelectedValue;
                        //    itm.ColorId = ddlColor.SelectedValue;
                        //}
                        
                        ucMessage.ShowMessage("Saved successfully", RMS.BL.Enums.MessageType.Info);
                        Response.Redirect("~/invsetup/wetbluetrnsfrhome.aspx?PID=425");
                    }
                    else
                    {
                        ucMessage.ShowMessage("Save was unsuccessful", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Full Piece/Half Piece quantity should exactly be same to the quantity in parts.", RMS.BL.Enums.MessageType.Error);
                }
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage("Exception:"+ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void GetWetBlueTrnsfrNo()
        {
            txtTrnsfrNo.Text = mN.GetSGNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        public void BindDDLItem()
        {
            ddlItem.DataSource = mN.GetProduct((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlItem.DataTextField = "itm_dsc";
            ddlItem.DataValueField = "itm_cd";
            ddlItem.DataBind();
        }

        public void BindDDLColor()
        {
            ddlColor.DataSource = mN.GetColor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlColor.DataTextField = "Color";
            ddlColor.DataValueField = "ColorId";
            ddlColor.DataBind();
        }

        public void BindDDLDesign()
        {
            ddlDesign.DataSource = mN.GetDesign((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlDesign.DataTextField = "Design_Desc";
            ddlDesign.DataValueField = "DesignId";
            ddlDesign.DataBind();
        }

        public void BindDDLThickness()
        {
            ddlThickness.DataSource = mN.GetThickness((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlThickness.DataTextField = "Thick_Desc";
            ddlThickness.DataValueField = "ThickId";
            ddlThickness.DataBind();
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
            d_Col.ColumnName = "Selection";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "SelectionVal";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Int32");
            d_Col.ColumnName = "Quantity";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Int32");
            d_Col.ColumnName = "HalfQuantity";
            d_table.Columns.Add(d_Col);


            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Area";
            d_table.Columns.Add(d_Col);
 
        }

        public void BindTable()
        {
            GetColumns();
            string selection = "";
            string gradeSelection = "";
            int count = 0;
            
            List<string> lst = mN.GetSelectionGrade(Item_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<spGetAvgSqftResult> avgList = mN.GetAvgSqft(Item_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            for (int i = 0; i < lst.Count(); i++)
            {

                string[] parts = Regex.Split(lst[i], ",");
                foreach (string part in parts)
                {
                    if (count == 0)
                    {
                        selection = part;
                    }
                    else
                    {
                        gradeSelection = part;
                    }
                    count++;
                }
                count = 0;
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;
                d_Row["Selection"] = selection;
                d_Row["SelectionVal"] = gradeSelection;

                foreach (var l in avgList)
                {
                    if (gradeSelection == l.GradeSelection)
                    {
                        d_Row["Area"] = Math.Round(Convert.ToDecimal(l.Sqft) / Convert.ToDecimal(l.Qty),2).ToString();
                    }
                }


                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }

        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }


        #endregion
    }
}
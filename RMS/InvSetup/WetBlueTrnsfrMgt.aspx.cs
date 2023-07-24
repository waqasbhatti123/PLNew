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
    public partial class WetBlueTrnsfrMgt : BasePage
    {
        #region DataMembers
        EntitySet<tblItemDataDet> itmDetEnt = new EntitySet<tblItemDataDet>();
        InvMN_BL mN = new InvMN_BL();
        InvGC_BL gC = new InvGC_BL();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;

        #endregion

        #region Properties
        
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

        public int vrNo
        {
            set { ViewState["vrNo"] = value; }
            get { return Convert.ToInt32(ViewState["vrNo"] ?? 0); }
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

        public List<string> listGrades
        {
            set { ViewState["listGrades"] = value; }
            get { return (List<string>)ViewState["listGrades"]; }
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
                    ddlDesign.Enabled = false;
                    btnClear.Visible = false;
                    btnBack.Visible = true;
                    btnList.Visible = false;
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
            itmDetEnt.Clear();
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

        protected void txtQuantity_textChanged(object sender, EventArgs e) 
        {
            //GridViewRow clickedRow = ((TextBox)sender).NamingContainer as GridViewRow;
            //TextBox txtQty = (TextBox)clickedRow.FindControl("txtQuantity");
            //TextBox txtHalfQty = (TextBox)clickedRow.FindControl("txtHalfQuantity");
            //decimal Fqty=0;
            //decimal Hqty=0;
            //if(txtQty.Text != "")
            //{
            //    Fqty=Convert.ToDecimal(txtQty.Text);
            //}
            //if (txtHalfQty.Text != "")
            //{
            //    Hqty = Convert.ToDecimal(txtHalfQty.Text);
            //}
            
            //TextBox txtSe = (TextBox)clickedRow.FindControl("txtSelection");
            //string Selc = txtSe.Text;
            //bool flag=false;
            //bool flag1 = false;
           
            
            //decimal Qty = Fqty + Hqty/2;

            //string selection ="";

            //int outerCount = 0;
            //int count=0;
            //int count1=0;
            //for (int i = 0; i < listGrades.Count(); i++)
            //{

            //    string[] parts = Regex.Split(listGrades[i], ",");
            //    count = 0;
            //    foreach (string part in parts)
            //    {
            //        if (count == 0)
            //        {
            //            selection = part;
            //            if (selection == Selc)
            //            {
            //                flag = true;
            //            }
            //        }
            //        else
            //        {
            //            if(flag==true)
            //            {

            //                    string[] grdSal = Regex.Split(part, ":");
            //                    count1 = 0;    
            //                    foreach (string Sl in grdSal)
            //                    {
            //                            if (count1 == 0)
            //                            {
            //                            }
            //                            else
            //                            {
            //                                string sID = Sl;
            //                                if (mN.QtyCheck(Qty, Item_Code, locID, sID, Convert.ToBoolean(Session["IfEdit"]), vrNo,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"])==false)
            //                                    {
            //                                        QtyMessage.ShowMessage("Quantity should be less than or equal to purchase quantity.", RMS.BL.Enums.MessageType.Error);
            //                                        ((TextBox)clickedRow.FindControl("txtQuantity")).Text = "";
            //                                        ((TextBox)clickedRow.FindControl("txtHalfQuantity")).Text="";
                                                    
            //                                        flag1 = true;
            //                                        break;
            //                                    }
            //                                else if (mN.QtyCheck(Qty, Item_Code, locID, sID, Convert.ToBoolean(Session["IfEdit"]), vrNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) == true)
            //                                {
            //                                    ((TextBox)clickedRow.FindControl("txtHalfQuantity")).Focus();
            //                                    flag1 = true;
            //                                    break;
            //                                }
            //                            }
            //                            count1++;
            //                    }
            //            }
            //        }
            //        if (flag1 == true)
            //            break;
            //        count++;
            //        }

            //    if (flag1 == true)
            //        break;
            //    outerCount++;
            //    }
            //if (outerCount >= listGrades.Count())
            //{
            //    QtyMessage.ShowMessage("Quantity should be less than or equal to purchase quantity.", RMS.BL.Enums.MessageType.Error);
            //    ((TextBox)clickedRow.FindControl("txtQuantity")).Text = "";
            //    ((TextBox)clickedRow.FindControl("txtHalfQuantity")).Text = "";
            //    ((TextBox)clickedRow.FindControl("txtQuantity")).Focus();
            //}
        }

        protected void txtHalfQuantity_textChanged(object sender, EventArgs e)
        {
            //GridViewRow clickedRow = ((TextBox)sender).NamingContainer as GridViewRow;
            //TextBox txtQty = (TextBox)clickedRow.FindControl("txtQuantity");
            //TextBox txtHalfQty = (TextBox)clickedRow.FindControl("txtHalfQuantity");
            //decimal Fqty = 0;
            //decimal Hqty = 0;
            //if (txtQty.Text != "")
            //{
            //    Fqty = Convert.ToDecimal(txtQty.Text);
            //}
            //if (txtHalfQty.Text != "")
            //{
            //    Hqty = Convert.ToDecimal(txtHalfQty.Text);
            //}

            //TextBox txtSe = (TextBox)clickedRow.FindControl("txtSelection");
            //string Selc = txtSe.Text;
            //bool flag = false;
            //bool flag1 = false;


            //decimal Qty = Fqty + Hqty / 2;

            //string selection = "";

            //int outerCount = 0;
            //int count = 0;
            //int count1 = 0;
            //for (int i = 0; i < listGrades.Count(); i++)
            //{

            //    string[] parts = Regex.Split(listGrades[i], ",");
            //    count = 0;
            //    foreach (string part in parts)
            //    {
            //        if (count == 0)
            //        {
            //            selection = part;
            //            if (selection == Selc)
            //            {
            //                flag = true;

            //            }
            //        }
            //        else
            //        {
            //            if (flag == true)
            //            {

            //                string[] grdSal = Regex.Split(part, ":");
            //                count1 = 0;
            //                foreach (string Sl in grdSal)
            //                {
            //                    if (count1 == 0)
            //                    {

            //                    }
            //                    else
            //                    {
            //                        string sID = Sl;
            //                        if (mN.QtyCheck(Qty, Item_Code, locID, sID, Convert.ToBoolean(Session["IfEdit"]), vrNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) == false)
            //                        {
            //                            QtyMessage.ShowMessage("Quantity should be less than or equal to Purchase Qty", RMS.BL.Enums.MessageType.Error);
            //                            ((TextBox)clickedRow.FindControl("txtQuantity")).Text = "";
            //                            ((TextBox)clickedRow.FindControl("txtHalfQuantity")).Text = "";
            //                            flag1 = true;
            //                            break;
            //                        }
            //                        else if (mN.QtyCheck(Qty, Item_Code, locID, sID, Convert.ToBoolean(Session["IfEdit"]), vrNo, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) == true)
            //                        {
            //                            ((TextBox)clickedRow.FindControl("txtHalfQuantity")).Focus();
            //                            flag1 = true;
            //                            break;
            //                        }
            //                    }
            //                    count1++;
            //                }
            //            }
            //        }
            //        if (flag1 == true)
            //            break;
            //        count++;
            //    }

            //    if (flag1 == true)
            //        break;
            //    outerCount++;
            //}
            //if (outerCount >= listGrades.Count())
            //{
            //    QtyMessage.ShowMessage("Quantity should be less than or equal to Purchase Qty", RMS.BL.Enums.MessageType.Error);
            //    ((TextBox)clickedRow.FindControl("txtQuantity")).Text = "";
            //    ((TextBox)clickedRow.FindControl("txtHalfQuantity")).Text = "";
            //    ((TextBox)clickedRow.FindControl("txtHalfQuantity")).Focus();
            //}
        }

        protected void ddlDesign_SelectedIndexChanged(object sednder, EventArgs e)
        {
            GetDesignById();
        }

        #endregion
        
        #region Helping Method

        public void Edit()
        {
            try
            {
                itmDetEnt = new EntitySet<tblItemDataDet>();

                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtDate.Text);

                string no = txtTrnsfrNo.Text.Substring(5);
                string yrno = txtTrnsfrNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                vrNo = Convert.ToInt32(yrno);
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
                    if (string.IsNullOrEmpty(qty))
                        qty = "0";
                    if (string.IsNullOrEmpty(halfqty))
                        halfqty = "0";
                    //if ((rbdPending.Checked == true) ? (qty != "" || ftg != "") : (Convert.ToInt32(qty) > 0))
                    //if ((rbdPending.Checked == true) ? (Convert.ToInt32(qty) > 0 || Convert.ToInt32(halfqty) > 0 || ftg != "0") : false)
                    if ((rbdPending.Checked == true) ? (Convert.ToInt32(qty) > 0 || Convert.ToInt32(halfqty) > 0) || ftg != "0" : (Convert.ToInt32(qty) > 0 || Convert.ToInt32(halfqty) > 0) && ftg != "0")
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
                            txt1 = "0";
                            itmDet.vr_qty = 0;
                        }

                        string txt3 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtHalfQuantity")).Text;
                        if (txt3 != "")
                        {
                            itmDet.vr_half = Convert.ToInt32(txt3);
                        }
                        else
                        {
                            txt3 = "0";
                            itmDet.vr_half = 0;
                        }

                        itmDet.KGSwt = 0;
                        itmDet.vr_val = 0;
                        itmDet.vr_rate = 0;
                        string txt2 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[4].FindControl("txtArea")).Text;
                        if (txt2 != "" && Convert.ToInt32(txt1) > 0)
                        {
                            itmDet.Feetage = Math.Round(Convert.ToDecimal(txt2) * Convert.ToInt32(txt1), 5);
                        }
                        else
                        {
                            if (txt2 != "" && Convert.ToInt32(txt1) == 0)
                            {
                                itmDet.Feetage = Math.Round(Convert.ToDecimal(txt2), 2);
                            }
                            else
                            {
                                txt2 = "0";
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
                    string vr_no = Session["vrNo"].ToString();
                    vr_no = vr_no.Substring(0, 4) + vr_no.Substring(5);
                    int yr = Convert.ToInt32(vr_no.Substring(0, 4));

                    if (itmDetEnt.Count() > 0)
                        res = mN.EditRecWetBlu(Convert.ToInt32(vr_no), yr, vrCode, itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    else
                        res = false;


                    if (res == true)
                    {

                        if (rbdApprove.Checked == true)
                        {
                            Post2TblItem(vrNo);
                        }

                        ucMessage.ShowMessage("Updated successfully.", RMS.BL.Enums.MessageType.Info);
                        itmDetEnt.Clear();
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
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void Save()
        {
            try
            {
                itmDetEnt = new EntitySet<tblItemDataDet>();

                tblItemData itmData = new tblItemData();

                itmData.vr_dt = Convert.ToDateTime(txtDate.Text);

                string no = txtTrnsfrNo.Text.Substring(5);
                string yrno = txtTrnsfrNo.Text.Substring(0, 4) + no;
                itmData.vr_no = Convert.ToInt32(yrno);
                vrNo = Convert.ToInt32(yrno);
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
                    //if((rbdPending.Checked == true)? (qty !="" || halfqty!= "" || ftg != "0") : (Convert.ToInt32(qty)>0 || Convert.ToInt32(halfqty)>0))
                    if ((rbdPending.Checked == true) ? (Convert.ToInt32(qty) > 0 || Convert.ToInt32(halfqty) > 0) || ftg != "0" : (Convert.ToInt32(qty) > 0 || Convert.ToInt32(halfqty) > 0) && ftg != "0")
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

                    if (itmDetEnt.Count() > 0)
                    {
                        res = mN.SaveItemData(itmData, itmDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                    }
                    else
                    {
                        res = false;
                    }

                    if (res == true)
                    {
                        if (rbdApprove.Checked == true)
                        {
                            Post2TblItem(vrNo);
                        }
                        ucMessage.ShowMessage("Saved successfully", RMS.BL.Enums.MessageType.Info);
                        itmDetEnt.Clear();
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
            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void GetDesignById()
        {
            try
            {
                tblItemDesign design = mN.GetDesignByID(ddlDesign.SelectedValue, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                ddlThickness.SelectedValue = Convert.ToString(design.ThickId);
                ddlColor.SelectedValue = design.ColorId;
            }
            catch (Exception ex)
            {
                ucMessage.ShowMessage("Exception: " + ex.Message, RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindEditFields()
        {
            int count=0;
            int qty = 0;
            int halfqty = 0;
            string vrNum = Session["vrNo"].ToString();
            vrNum = vrNum.Substring(0, 4) + vrNum.Substring(5);
            vrNo = Convert.ToInt32(vrNum);
            int yr = Convert.ToInt32(vrNum.Substring(0, 4));
            tblItemData record = mN.GetRecWetBlueTrnsfr(vrNo, yr, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
            //List<string> lst = mN.GetSelectionGrade(Item_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            listGrades = mN.GetSelectionGrade(Item_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblItemDataDet> recordsDet = mN.GetRecDetWetBlueTrnsfr(Convert.ToInt32(vrNo), yr, vrCode, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            for (int i = 0; i < listGrades.Count(); i++)
            {

                string[] parts = Regex.Split(listGrades[i], ",");
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

        public void Post2TblItem(int vchrNum)
        {
            if (vrNo <= 0)
            {
                vrNo = vchrNum;
                ToLocID = 3;
                locID = 2;
            }

            List<tblItemDataDet> lstDet = mN.GetItemDetUsingVrNo(vrNo, 24, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (lstDet.Count > 0)
            {
                foreach (tblItemDataDet tbl in lstDet)
                {
                    tblItem Item = mN.GetItemUsingSelection(tbl.itm_cd, tbl.SelectionId, tbl.LocId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                    if (Item != null)
                    {
                        mN.GetUpdTblItemWetBlue(tbl.LocId, tbl.itm_cd, tbl.SelectionId, tbl.DesignId,Convert.ToInt32(tbl.vr_qty), Convert.ToInt32(tbl.vr_half), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        //posting to crust dept.
                        tblItem ItemCrust = mN.GetItemUsingAttributes(tbl.itm_cd, "00000", ToLocID, tbl.DesignId, Item.Gl_Year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                        if (ItemCrust != null)
                        {
                            //update
                            mN.GetUpdTblItemCrustTrnsfr(ToLocID, Item.itm_cd, "00000", Convert.ToInt32(tbl.vr_qty), Convert.ToInt32(tbl.vr_half), 0, tbl.DesignId, Item.Gl_Year, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        }
                        else
                        {
                            //insert
                            mN.GetInsrtTblItemCrustTrnsfr(1, ToLocID, Item.itm_cd, "00000", Convert.ToInt32(tbl.vr_qty), Convert.ToInt32(tbl.vr_half), 0, tbl.DesignId, Convert.ToInt32(Item.Gl_Year), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                        }
                    }
                }
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
            //-------------------
            //List<string> lst = mN.GetSelectionGrade(Item_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
             listGrades = mN.GetSelectionGrade(Item_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<spGetAvgSqftResult> avgList = mN.GetAvgSqft(Item_Code, locID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            for (int i = 0; i < listGrades.Count(); i++)
            {

                string[] parts = Regex.Split(listGrades[i], ",");
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
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

namespace RMS.InvSetup
{
    public partial class InwardGatePassView : BasePage
    {
        #region DataMembers

        //DashBoardBL dashBoardBL = new DashBoardBL();
        InvGP_BL gP = new InvGP_BL();
       
        #endregion

        #region Properties

        EntitySet<tblStkGPDet> stkGpDetEnt = new EntitySet<tblStkGPDet>();
        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;
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

        public List<String> lstIgpRef
        {
            set { ViewState["lstIgpRef"] = value; }
            get { return (List<string>)ViewState["lstIgpRef"]; }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
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
                GridView1.Columns[2].Visible = false;

               
                
                
                //lblProduct.Text = "Product:";
                if (Session["DateFormat"] == null)
                {
                    CalendarExtender1.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                }
                else
                {
                    CalendarExtender1.Format = Session["DateFormat"].ToString();
                }
                CalendarExtender1.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                if (Convert.ToBoolean(Session["IfEdit"]) == false)
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "IGP").ToString();
                    BindTable();
                    BindDDLLoc();
                    BindProduct();
                    BindDDLVendor();
                    BindDDLCity();
                    GetGatePassNo();
                    GetDocRef();
                    btnBack.Visible = false;
                    btnClear.Visible = true;
                    btnList.Visible = true;
                }
                else
                {
                    Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", Session["PgTile"].ToString()).ToString();
                    
                    BindDDLLoc();
                    BindProduct();
                    BindDDLVendor();
                    BindDDLCity();
                    BindFields();
                    BindTableEdit();
                    btnClear.Visible = false;
                    btnBack.Visible = false;
                    btnList.Visible = true;
                    addRow.Visible = false;
                    btnSave.Visible = false;
                    List<string> lst=new List<string>();
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        lst.Add(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtIGPRef")).Text);
                    }
                    lstIgpRef = lst;
                }

            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                DropDownList ddVen = (DropDownList)e.Row.Cells[0].FindControl("ddlVendor");
                DropDownList ddOpp = (DropDownList)e.Row.Cells[1].FindControl("ddlProduct");
                //ddVen.Attributes["style"] = "color:red";
                //if (ddVen.SelectedItem.Text == "Select Party")
                //{
                //    ddVen.SelectedItem.Attributes["style"] = "color:red";
                //}

                //BindOpponentDD(ddOpp);
                BindDDLVendorDD(ddVen);
            }
            
        }
        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
            SetDropDownList();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/invsetup/inwardgatepassmgt.aspx?PID=463");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(Session["IfEdit"]) == false)
            {
                Save();

            }
            else
            {
                bool result = GetRowStatus(GridView1);
                if (result == true)
                {
                    Edit();
                }
                else
                {
                    ucMessage.ShowMessage("Cannot save...", RMS.BL.Enums.MessageType.Error);
                }
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
          
            Response.Redirect(Session["PrePgAddress"].ToString());
        }
        protected void btnList_Click(object sender, EventArgs e)
        {
            //-----------
            Session["strIGPNo"] = txtGPNo.Text;
            Session["dtIGP"] = CalendarExtender1.SelectedDate.Value.ToString();//txtGpDate.Text;
            Session["Mgt_View"] = "1";
            //-----------

            Response.Redirect("~/invsetup/inwardgatepasshome.aspx?PID=421");
        }
        #endregion
        
        #region Helping Method

        //=======================
        public bool GetRowStatus(GridView gd)
        {
            bool chkEmpt=true;


            for(int i=0; i<gd.Rows.Count;i++){

                if (((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlVendor")).SelectedValue == "0")
                {
                    chkEmpt = false;
                    break;
                }


                else if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text == "")
                {
                    chkEmpt = false;
                    break;
                }
                else
                    chkEmpt = true;
           
              
            }
            return chkEmpt;

        }

        //=================
        public void BindProduct()
        {
            ddlProduct.DataSource = gP.GetProduct((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlProduct.DataTextField = "itm_dsc";
            ddlProduct.DataValueField = "itm_cd";
            ddlProduct.DataBind();
           
        }
        public void BindDDLVendorDD(DropDownList ddlVendor)
        {
            ddlVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.DataTextField = "gl_dsc";
            ddlVendor.DataValueField = "gl_cd";
            ddlVendor.DataBind();
        }

        public void  Edit()
        {
            int rowCount = 0;
            int srCount = 0;
            bool res = false;
            int sumQty = 0;

             bool gpRefExist=false;

        
            //=================================
             for (int i = 0; i < GridView1.Rows.Count; i++)
             {
                 string gpRef1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtIGPRef")).Text;

              
                     if (gpRef1 != lstIgpRef[i])
                     {

                         gpRefExist = gP.checkIfGpExist(gpRef1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                         if (gpRefExist == true)
                         {
                             break;
                         }

                     }
               
             }

             //---------------------------
             if (gpRefExist == false)
             {
                 int outerLoop = 0;
                 for (int k = 0; k < GridView1.Rows.Count; k++)
                 {
                     string gpRef1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[k].Cells[2].FindControl("txtIGPRef")).Text;
                     if (gpRef1 != "")
                     {
                         for (int m = 0; m < k; m++)
                         {
                             string gpRef2 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[m].Cells[2].FindControl("txtIGPRef")).Text;

                             if (gpRef1 == gpRef2)
                             {
                                 gpRefExist = true;
                                 outerLoop = 1;
                                 break;
                             }

                         }
                     }
                     if (outerLoop == 1)
                     {
                         break;
                     }

                 }

             }

             //================================
            if (gpRefExist == false)
            {


                for (int j = 0; j < GridView1.Rows.Count; j++)
                {
                    string qtySub = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text;
                    if (qtySub != "")
                    {
                        sumQty = sumQty + Convert.ToInt32(qtySub);
                    }
                }


                if (sumQty == Convert.ToInt32(txtTotalQty.Text))
                {


                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        string gpRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtIGPRef")).Text;
                        string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtQuantity")).Text;
                        string prty = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlVendor")).SelectedValue;

                        if (gpRef != "" && qty != "" && prty != "0")
                        {
                            /*=tblStkGP================================================*/

                            tblStkGP stkGp = new tblStkGP();
                            if (Session["BranchID"] == null)
                                stkGp.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                            else
                                stkGp.br_id = Convert.ToInt32(Session["BranchID"]);
                            stkGp.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                            stkGp.vt_cd = 11;


                            string no = txtGPNo.Text.Substring(5);
                            string yrno = txtGPNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(no) + i);
                            stkGp.vr_no = Convert.ToInt32(yrno);

                            try
                            {
                                stkGp.vr_dt = Convert.ToDateTime(txtGpDate.Text);
                            }
                            catch
                            {
                                ucMessage.ShowMessage("Invalid IGP Date", RMS.BL.Enums.MessageType.Error);
                                return;
                            }

                            stkGp.DocRef = txtDocRef.Text;
                            stkGp.DocRefDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            stkGp.LotRef = 0;
                            stkGp.VendorId = ddlVendor.SelectedValue;
                            stkGp.VendorCity = Convert.ToString(ddlCity.SelectedValue);
                            stkGp.BiltyNo = txtBiltyNo.Text;
                            stkGp.VehicleNo = txtVehicleNo.Text;
                            stkGp.Driver = txtDriver.Text;
                            stkGp.vr_nrtn = txtRemarks.Text;
                            stkGp.vr_apr = Convert.ToString(ddlStatus.SelectedValue);
                            stkGp.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (ddlStatus.SelectedValue == "A" || ddlStatus.SelectedValue == "C")
                            {
                                if (Session["UserName"] == null)
                                    stkGp.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                                else
                                    stkGp.updateby = Session["UserName"].ToString();
                            }
                            if (txtFrieght.Text != "")
                                stkGp.Freight = Convert.ToDecimal(txtFrieght.Text);
                            else
                                stkGp.Freight = 0;


                            /*=tblStkGPDet=============================================*/

                            rowCount++;
                            tblStkGPDet stkGpDet = new tblStkGPDet();
                            if (Session["BranchID"] == null)
                                stkGpDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                            else
                                stkGpDet.br_id = Convert.ToInt32(Session["BranchID"]);
                            stkGpDet.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                            stkGpDet.vt_cd = 11;


                            string nos = txtGPNo.Text.Substring(5);
                            string yrnos = txtGPNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(nos) + i);
                            stkGpDet.vr_no = Convert.ToInt32(yrnos);

                            stkGpDet.vr_seq = Convert.ToInt16(srCount + 1);
                            stkGpDet.LotNo = Convert.ToInt32(yrno);
                            stkGpDet.PartyId = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlVendor")).SelectedValue;
                            stkGpDet.Itm_cd = ddlProduct.SelectedValue;

                            if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text != "")
                                stkGpDet.vr_qty = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text);
                            else
                                stkGpDet.vr_qty = 0;
                            stkGpDet.Feetage = 0;
                            stkGpDet.Remarks = "";
                            if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text != "")
                                stkGpDet.KgsWt = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text);
                            else
                                stkGpDet.KgsWt = 0;

                            stkGpDet.GPRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtIGPRef")).Text;

                            srCount++;
                            stkGpDetEnt.Add(stkGpDet);

                            /*=Edit GP==============================================*/


                            if (rowCount > 0)
                            {
                                if (i == 0)
                                {
                                    gP.EditRec(stkGp.DocRef, Convert.ToInt32(stkGp.vr_no.ToString().Substring(0, 4)), 11, stkGp, stkGpDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                }
                                res = gP.SaveStkIGP(stkGp, stkGpDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                stkGpDetEnt.Clear();
                            }
                            else
                            {
                                res = false;
                                break;
                            }

                        }
                    }
                    if (res == true)
                    {
                        ucMessage.ShowMessage("Edited successfully", RMS.BL.Enums.MessageType.Info);
                        Response.Redirect("~/invsetup/inwardgatepasshome.aspx?PID=421");
                    }
                    else
                    {
                        ucMessage.ShowMessage("Edit was unsuccessful", RMS.BL.Enums.MessageType.Error);
                    }
                }
                else
                {
                    ucMessage.ShowMessage("Total quantity should match the quantity in parts.", RMS.BL.Enums.MessageType.Error);
                }

            }
            else
            {
                ucMessage.ShowMessage("GP Ref already exists.", RMS.BL.Enums.MessageType.Error);
            }
        }       
        public void Save()
        {
            bool gpRefExist=false;
            for(int k =0; k< GridView1.Rows.Count;k++)
            {
            string gpRef1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[k].Cells[2].FindControl("txtIGPRef")).Text;
               gpRefExist = gP.checkIfGpExist(gpRef1, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
               if(gpRefExist == true)
                break;
            }
            //---------------------------
            if (gpRefExist == false)
            {
                int outerLoop = 0;
                for (int k = 0; k < GridView1.Rows.Count; k++)
                {
                    string gpRef1 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[k].Cells[2].FindControl("txtIGPRef")).Text;
                    if (gpRef1 != "")
                    {
                        for (int m = 0; m < k; m++)
                        {
                            string gpRef2 = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[m].Cells[2].FindControl("txtIGPRef")).Text;

                            if (gpRef1 == gpRef2)
                            {
                                gpRefExist = true;
                                outerLoop = 1;
                                break;
                            }

                        }
                    }
                    if (outerLoop == 1)
                    {
                        break;
                    }

                }

            }

            //-----------------------------------------
            if (gpRefExist == false)
            {

                int rowCount = 0;
                int srCount = 0;
                bool res = false;
                int sumQty = 0;
                for (int j = 0; j < GridView1.Rows.Count; j++)
                {
                    string qtySub = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[j].FindControl("txtQuantity")).Text;
                    if (qtySub != "")
                    {
                        sumQty = sumQty + Convert.ToInt32(qtySub);
                    }
                }
                if (sumQty == 0)
                {

                    ucMessage.ShowMessage("Please add at least one party from the grid", RMS.BL.Enums.MessageType.Error);
                    return;

                }
                try
                {
                    Convert.ToInt32(txtTotalQty.Text);
                }
                catch
                {
                    ucMessage.ShowMessage("Invalid/Out of range total quantity", RMS.BL.Enums.MessageType.Error);
                    return;
                }
                if (sumQty == Convert.ToInt32(txtTotalQty.Text))
                {

                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                       string gpRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtIGPRef")).Text;



                        string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtQuantity")).Text;
                        string prty = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlVendor")).SelectedValue;

                        if (gpRef != "" && qty != "" && prty != "0")
                        {
                            /*=tblStkGP================================================*/

                            tblStkGP stkGp = new tblStkGP();
                            if (Session["BranchID"] == null)
                                stkGp.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                            else
                                stkGp.br_id = Convert.ToInt32(Session["BranchID"]);
                            stkGp.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                            stkGp.vt_cd = 11;


                            string no = txtGPNo.Text.Substring(5);
                            string yrno = txtGPNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(no) + i);
                            stkGp.vr_no = Convert.ToInt32(yrno);

                            try
                            {
                                stkGp.vr_dt = Convert.ToDateTime(txtGpDate.Text);

                            }
                            catch
                            {
                                ucMessage.ShowMessage("Invalid IGP Date", RMS.BL.Enums.MessageType.Error);
                                return;
                            }
                            stkGp.DocRef = txtDocRef.Text;
                            stkGp.DocRefDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            stkGp.LotRef = 0;
                            stkGp.VendorId = ddlVendor.SelectedValue;
                            stkGp.VendorCity = Convert.ToString(ddlCity.SelectedValue);
                            stkGp.BiltyNo = txtBiltyNo.Text;
                            stkGp.VehicleNo = txtVehicleNo.Text;
                            stkGp.Driver = txtDriver.Text;
                            stkGp.vr_nrtn = txtRemarks.Text;
                            stkGp.vr_apr = Convert.ToString(ddlStatus.SelectedValue);
                            stkGp.updateon = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (ddlStatus.SelectedValue == "A" || ddlStatus.SelectedValue == "C")
                            {
                                if (Session["UserName"] == null)
                                    stkGp.updateby = Request.Cookies["uzr"]["UserName"].ToString();
                                else
                                    stkGp.updateby = Session["UserName"].ToString();
                            }
                            if (txtFrieght.Text != "")
                                stkGp.Freight = Convert.ToDecimal(txtFrieght.Text);
                            else
                                stkGp.Freight = 0;



                            /*=tblStkGPDet=============================================*/

                            rowCount++;
                            tblStkGPDet stkGpDet = new tblStkGPDet();
                            if (Session["BranchID"] == null)
                                stkGpDet.br_id = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                            else
                                stkGpDet.br_id = Convert.ToInt32(Session["BranchID"]);
                            stkGpDet.LocId = Convert.ToInt16(ddlLoc.SelectedValue);
                            stkGpDet.vt_cd = 11;


                            string nos = txtGPNo.Text.Substring(5);
                            string yrnos = txtGPNo.Text.Substring(0, 4).ToString() + Convert.ToString(Convert.ToInt32(nos) + i);
                            stkGpDet.vr_no = Convert.ToInt32(yrnos);

                            stkGpDet.vr_seq = Convert.ToInt16(srCount + 1);
                            stkGpDet.LotNo = Convert.ToInt32(yrno);
                            stkGpDet.PartyId = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].FindControl("ddlVendor")).SelectedValue;
                            stkGpDet.Itm_cd = ddlProduct.SelectedValue;

                            if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text != "")
                                stkGpDet.vr_qty = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtQuantity")).Text);
                            else
                                stkGpDet.vr_qty = 0;
                            stkGpDet.Feetage = 0;
                            stkGpDet.Remarks = "";
                            if (((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text != "")
                                stkGpDet.KgsWt = Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtWeight")).Text);
                            else
                                stkGpDet.KgsWt = 0;

                            stkGpDet.GPRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtIGPRef")).Text;

                            srCount++;
                            stkGpDetEnt.Add(stkGpDet);

                            /*=Save GP==============================================*/

                            if (rowCount > 0)
                            {
                                try
                                {
                                    res = gP.SaveStkIGP(stkGp, stkGpDetEnt, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                                }
                                catch
                                {
                                    ucMessage.ShowMessage("Exception occurred in saving data", RMS.BL.Enums.MessageType.Error);
                                    return;
                                }
                                stkGpDetEnt.Clear();
                            }
                            else
                            {
                                res = false;
                                break;
                            }
                        }

                    }


                    if (res == true)
                    {
                        ucMessage.ShowMessage("Saved successfully", RMS.BL.Enums.MessageType.Info);
                        Response.Redirect("~/invsetup/inwardgatepasshome.aspx?PID=421");
                    }
                    else
                    {
                        ucMessage.ShowMessage("Save was unsuccessful", RMS.BL.Enums.MessageType.Error);
                    }


                }
                else
                {
                    ucMessage.ShowMessage("Total quantity should match the quantity in parts", RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                ucMessage.ShowMessage("GP Ref already exists.", RMS.BL.Enums.MessageType.Error);
            }
                
        }
        public void BindFields()
        {
            int IGPNo = Convert.ToInt32(Session["IGPno"]);
            int yr = Convert.ToInt32(Session["DateYear"]);
            string docRef = Convert.ToString(Session["DocRef"]);
            tblStkGP rec = gP.GetRec(docRef, yr, 11, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            ddlLoc.SelectedValue = rec.LocId.ToString();
            txtGPNo.Text = rec.vr_no.ToString().Substring(0, 4) + "/" + rec.vr_no.ToString().Substring(4);

            CalendarExtender1.SelectedDate = rec.vr_dt;


            if (rec.DocRef != null)
                txtDocRef.Text = rec.DocRef;
            if (rec.VendorId != null)
                ddlVendor.SelectedValue = rec.VendorId;
            ddlCity.SelectedValue = rec.VendorCity;
            if (rec.BiltyNo != null)
                txtBiltyNo.Text = rec.BiltyNo.ToString();
            else
                txtBiltyNo.Text = "";
            if (rec.VehicleNo != null)
                txtVehicleNo.Text = rec.VehicleNo.ToString();
            else
                txtVehicleNo.Text = "";
            if (rec.Driver != null)
                txtDriver.Text = rec.Driver.ToString();
            else
                txtDriver.Text = "";
            if (rec.vr_nrtn != null)
                txtRemarks.Text = rec.vr_nrtn.ToString();
            else
                txtRemarks.Text = "";
            if (rec.Freight != null)
                txtFrieght.Text = Convert.ToString(rec.Freight);
            else
                txtFrieght.Text = "";
            ddlStatus.SelectedValue = rec.vr_apr.ToString();

            txtTotalQty.Text = gP.GetGPQty(docRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        }
        public void GetDocRef()
        {
            txtDocRef.Text = gP.GetDocReference(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 11, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }
        public void GetGatePassNo()
        {
            txtGPNo.Text = gP.GetGpNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, 11, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        } 
        public void BindDDLLoc()
        {
            ddlLoc.DataSource = gP.GetStockLoc((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlLoc.DataValueField = "LocId";
            ddlLoc.DataTextField = "LocName";
            ddlLoc.DataBind();
            ddlLoc.SelectedValue = "1";

        }
        public void BindDDLVendor()
        {
            ddlVendor.DataSource = new VendorBL().GetVendor((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlVendor.DataTextField = "gl_dsc";
            ddlVendor.DataValueField = "gl_cd";
            ddlVendor.DataBind();
        }
        public void BindDDLCity()
        {
            ddlCity.DataSource = gP.GetCity((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityID";
            ddlCity.DataBind();
        }
        public void AddRow()
        {
            if (CurrentTable != null)
            {
                d_table = CurrentTable;
                d_Row = d_table.NewRow();
                d_Row["Sr"] = d_table.Rows.Count + 1;
                d_table.Rows.Add(d_Row);
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
                    string srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].Cells[0].FindControl("lblSr")).Text;
                    string PartyID = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlVendor")).SelectedValue;
                    //string Prdct = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[2].FindControl("ddlProduct")).SelectedValue;
                    
                    // string Prdct = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtProduct")).Text;
                    //string PrdctCode = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[1].FindControl("txtProductCode")).Text;
                    string igpRef = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[2].FindControl("txtIGPRef")).Text;
                    
                    string qty = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[3].FindControl("txtQuantity")).Text;
                    string vat = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].Cells[4].FindControl("txtWeight")).Text;
                    
                    d_Row["Sr"] = srNo;
                    d_Row["Party"] = PartyID;
                    //d_Row["Product"] = Prdct;
                    //d_Row["ProductCode"] = PrdctCode;
                    //d_Row["Particulars"] = partclrs;
                    if (qty != "")
                        d_Row["Quantity"] = Convert.ToInt32(qty);
                    if (vat != "")
                        d_Row["Weight"] = Convert.ToDecimal(vat);
                    d_Row["IGPRef"] = igpRef;
                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;//assigning to viewstate datatable
                BindGrid();
                SetDropDownList();

            }
        }
        public void SetDropDownList()
        {
            rowsCount = CurrentTable.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                //DropDownList ddl1 = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[2].FindControl("ddlProduct"));
                DropDownList ddl2 = ((System.Web.UI.WebControls.DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlVendor"));
                if (i < CurrentTable.Rows.Count)
                {

                    //if (CurrentTable.Rows[i]["Product"] != DBNull.Value)
                    //{
                    //    ddl1.ClearSelection();
                    //    ddl1.Items.FindByValue(CurrentTable.Rows[i]["Product"].ToString()).Selected = true;
                    //}
                    if (CurrentTable.Rows[i]["Party"] != DBNull.Value)
                    {
                        ddl2.ClearSelection();
                        ddl2.Items.FindByValue(CurrentTable.Rows[i]["Party"].ToString()).Selected = true;
                    }
                }
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
            d_Col.ColumnName = "Party";
            d_table.Columns.Add(d_Col);
            //d_Col = new DataColumn();
            //d_Col.DataType = System.Type.GetType("System.String");
            //d_Col.ColumnName = "Product";
            //d_table.Columns.Add(d_Col);
            //d_Col = new DataColumn();
            //d_Col.DataType = System.Type.GetType("System.String");
            //d_Col.ColumnName = "ProductCode";
            //d_table.Columns.Add(d_Col);
            //d_Col = new DataColumn();
            //d_Col.DataType = System.Type.GetType("System.String");
            //d_Col.ColumnName = "Particulars";
            //d_table.Columns.Add(d_Col);
            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Int32");
            d_Col.ColumnName = "Quantity";
            d_table.Columns.Add(d_Col);
            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Decimal");
            d_Col.ColumnName = "Weight";
            d_table.Columns.Add(d_Col);
            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "IGPRef";
            d_table.Columns.Add(d_Col);

        }
        public void BindTable()
        {
            GetColumns();
            for (int i = 0; i < 3; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;
                //d_Row["ItemCode"] = "";
                //d_Row["ItemName"] = "";
                //d_Row["Price"] = 0;
                //d_Row["Quantity"] = 0;
                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }
        public void BindTableEdit()
        {
            GetColumns();
            int count = 0;
            int IGPNo = Convert.ToInt32(Session["IGPno"]);
            int yr =Convert.ToInt32(Session["DateYear"]);
            string docRef = Convert.ToString(Session["DocRef"]);
            List<tblStkGPDet> list = gP.GetRecDet(docRef, yr, 11, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            foreach (var l in list)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = count + 1;
                d_Row["Party"] = l.PartyId;
                //d_Row["Product"] = l.Itm_cd;
                //d_Row["ProductCode"] = "";
                //d_Row["Particulars"] = l
                d_Row["Quantity"] = l.vr_qty;
                d_Row["Weight"] = l.Price;
                d_Row["IGPRef"] = l.GPRef;
                count++;
                d_table.Rows.Add(d_Row);
                ddlProduct.SelectedValue = l.Itm_cd;
            }
            CurrentTable = d_table;
            BindGrid();
            
            SetDropDownList();
        }
        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }

        #endregion
    }
}
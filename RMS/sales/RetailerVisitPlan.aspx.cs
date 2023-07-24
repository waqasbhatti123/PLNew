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
using System.Web.UI;
using System.Web;
using System.Web.Script.Serialization;

namespace RMS.sales
{
    public partial class RetailerVisitPlan : BasePage
    {

        #region DataMembers

        RetailerVisitsBL retailerVisitBL = new RetailerVisitsBL();
        SalesPersonBL salesPersonBL = new SalesPersonBL();
        AreaCodeBL areaCodeBL = new AreaCodeBL();

        DataTable d_table = new DataTable();
        DataRow d_Row;
        DataColumn d_Col;

        //EntitySet<tblSdVisitDet> entitySetRetailerVisitsDet = new EntitySet<tblSdVisitDet>();

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

#pragma warning disable CS0114 // 'RetailerVisitPlan.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'RetailerVisitPlan.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
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

        public int VrID
        {
            get { return Convert.ToInt32(ViewState["VrID"]); }
            set { ViewState["VrID"] = value; }
        }

        public int SalesPersonID
        {
            get { return Convert.ToInt32(ViewState["SalesPersonID"]); }
            set { ViewState["SalesPersonID"] = value; }
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
                ClearFields();

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "RetailerVisitPlan").ToString();
                if (Session["DateFormat"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        //calFltFromDt.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        //calFltToDt.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        calDocDate.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        calSchDate.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    //calFltFromDt.Format = Session["DateFormat"].ToString();
                    //calFltToDt.Format = Session["DateFormat"].ToString();
                    calDocDate.Format = Session["DateFormat"].ToString();
                    calSchDate.Format = Session["DateFormat"].ToString();
                }

                calDocDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                calSchDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

                //txtFltFromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month.ToString() + "-01").ToString("dd-MMM-yy");
                //txtFltToDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yy");
                ShowDistributorName();
                BindSalesPersonDropDown();
                BindTable();
                //BindArtifactTypesDropDown();
                BindFilterSalesPersonDropDown();
                //BindFilterArtifactTypeDropDown();
                //BindFilterResponseTypeDropDown();

                BindFooterDataGrid();

            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindFooterDataGrid();
        }

        protected void addRow_Click(object sender, EventArgs e)
        {
            UpdateTable();
            AddRow();
        }

        #region data grid

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }

        protected void grd_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime schDate = Convert.ToDateTime(grd.SelectedRow.Cells[3].Text).Date;
            if (schDate.Date < Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date)
            {
                ucMessage.ShowMessage("Sorry,you can not edit previous schedules.", RMS.BL.Enums.MessageType.Error);
            }
            else
            {
                IsEdit = true;
                VrID = Convert.ToInt32(grd.SelectedDataKey.Values["vr_id"]);
                SalesPersonID = Convert.ToInt32(grd.SelectedDataKey.Values["sale_person_id"]);
                //GetByID();
            }
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            BindFooterDataGrid();
        }

        #endregion

        //private void GetByID()
        //{
        //    tblSdVisit tblVisitData = retailerVisitBL.GetVisitDataByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    DocNo = tblVisitData.doc_no;
        //    DocNoFormated = DocNo.ToString().Substring(0, 4) + "/" + DocNo.ToString().Substring(4);
        //    txtDocNo.Text = DocNoFormated;
        //    ddlSalesPerson.SelectedValue = tblVisitData.sale_person_id.ToString();
        //    if (Session["DateFormat"] == null)
        //    {
        //        if (Request.Cookies["uzr"] != null)
        //        {
        //            calDocDate.SelectedDate = tblVisitData.doc_dt;
        //            calSchDate.SelectedDate = tblVisitData.sch_dt;
        //        }
        //        else
        //        {
        //            Response.Redirect("~/login.aspx");
        //        }
        //    }
        //    else
        //    {
        //        calDocDate.SelectedDate = tblVisitData.doc_dt;
        //        calSchDate.SelectedDate = tblVisitData.sch_dt;
        //    }

        //    List<tblSdVisitDet> lstPromDetail = retailerVisitBL.GetAllVisitDetailByVisitDataID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    BindTableEdit(lstPromDetail);

        //}

        private void BindFooterDataGrid()
        {
            int salesPersonId = Convert.ToInt32(ddlFltSalesPerson.SelectedValue);
            string documentNo = txtFltDocNo.Text.Trim();
            int docNumber = 0;

            if (!string.IsNullOrEmpty(documentNo))
            {
                if (documentNo.Contains('/'))
                {
                    documentNo = documentNo.Replace("/", string.Empty);
                }
                try
                {
                    docNumber = Convert.ToInt32(documentNo);
                }
                catch
                {
                    docNumber = 0;
                }
            }

            grd.DataSource = retailerVisitBL.GetAllVisitData(docNumber, salesPersonId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grd.DataBind();
        }

        public void BindSalesPersonDropDown()
        {
            ddlSalesPerson.DataSource = salesPersonBL.GetAllSalesPerson((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlSalesPerson.DataValueField = "ID";
            ddlSalesPerson.DataTextField = "SalesPerson";
            ddlSalesPerson.DataBind();
        }

        public void BindFilterSalesPersonDropDown()
        {
            ddlFltSalesPerson.DataSource = salesPersonBL.GetAllSalesPerson((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltSalesPerson.DataValueField = "ID";
            ddlFltSalesPerson.DataTextField = "SalesPerson";
            ddlFltSalesPerson.DataBind();
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                ClearFields();
                ucButtons.SetMode(RMS.BL.Enums.PageMode.New);
            }
            else if (e.CommandName == "Save")
            {
                if (Convert.ToDateTime(txtScheduleDate.Text.Trim()).Date < Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date)
                {
                    ucMessage.ShowMessage("Schedule date should be greater or equal to current date.", RMS.BL.Enums.MessageType.Error);
                    txtScheduleDate.Focus();
                }
                else
                {
                    bool isEmpty = true;
                    foreach (GridViewRow grow in GridView1.Rows)
                    {
                        if (grow.RowType == DataControlRowType.DataRow)
                        {
                            if (!string.IsNullOrEmpty(((HiddenField)grow.FindControl("hdnRetailerId")).Value))
                            {
                                isEmpty = false;
                            }
                        }
                    }
                    if (isEmpty)
                    {
                        ucMessage.ShowMessage("Enter atleast one record.", RMS.BL.Enums.MessageType.Error);
                    }
                    else
                    {
                        //Save();
                        BindFooterDataGrid();
                    }
                }
            }

            else if (e.CommandName == "Edit")
            {
            }
            else if (e.CommandName == "Cancel")
            {
                ClearFields();
            }
        }

        #endregion

        #region Helping Method

        //private void Save()
        //{
        //    tblSdVisit tblVisitData = new tblSdVisit();
        //    if (IsEdit)
        //    {
        //        tblVisitData = retailerVisitBL.GetVisitDataByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    }
        //    else
        //    {
        //        GetDocNo();
        //        tblVisitData.doc_no = DocNo;
        //    }

        //    try
        //    {
        //        tblVisitData.doc_dt = Convert.ToDateTime(txtDocDate.Text.Trim());
        //    }
        //    catch
        //    {
        //        tblVisitData.doc_dt = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    }
        //    try
        //    {
        //        tblVisitData.sch_dt = Convert.ToDateTime(txtScheduleDate.Text.Trim());
        //    }
        //    catch
        //    {
        //        tblVisitData.sch_dt = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    }
        //    tblVisitData.sale_person_id = Convert.ToInt32(ddlSalesPerson.SelectedValue);
        //    tblVisitData.created_on = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    if (!string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
        //        tblVisitData.created_by = Convert.ToInt32(Session["UserID"]);
        //    tblVisitData.status = true;

        //    entitySetRetailerVisitsDet = GetVisitDetailForSave(tblVisitData);
        //    //-------------------
        //    if (IsEdit)
        //    {
        //        tblVisitData.updated_on = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        if (!string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
        //            tblVisitData.updated_by = Convert.ToInt32(Session["UserID"]);

        //        string msg = retailerVisitBL.UpdateVisitData(tblVisitData, entitySetRetailerVisitsDet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        if (msg.Equals("Done"))
        //        {
        //            ucMessage.ShowMessage("Doc No " + DocNoFormated + " " + GetGlobalResourceObject("MainResource", "updated").ToString(), RMS.BL.Enums.MessageType.Info);
        //            ClearFields();
        //        }
        //        else
        //        {
        //            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
        //        }
        //    }
        //    else
        //    {
        //        tblVisitData.tblSdVisitDets = entitySetRetailerVisitsDet;
        //        string msg = retailerVisitBL.InsertVisitData(tblVisitData, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        if (msg.Equals("Done"))
        //        {
        //            ucMessage.ShowMessage("Doc No " + DocNoFormated + " " + GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
        //            ClearFields();
        //        }
        //        else
        //        {
        //            ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "ExceptionMsg").ToString() + "<br/>" + msg, RMS.BL.Enums.MessageType.Error);
        //        }
        //    }

        //}

        private void ClearFields()
        {
            IsEdit = false;
            calDocDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            calSchDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = 0;
            DocNoFormated = string.Empty;
            SalesPersonID = 0;
            ddlSalesPerson.SelectedIndex = 0;
            txtDocNo.Text = string.Empty;
            BindTable();
        }

        public void GetDocNo()
        {
            DocNoFormated = retailerVisitBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
        }

        private void ShowDistributorName()
        {
            if (Session["UserName"] == null)
            {
                lblDistributorName.Text = Request.Cookies["uzr"]["UserName"];
            }
            else
            {
                lblDistributorName.Text = Session["UserName"].ToString();
            }
        }

        #endregion

        public void AddRow()
        {
            if (CurrentTable != null)
            {
                d_table = CurrentTable;
                for (int i = 0; i < 5; i++)
                {
                    d_Row = d_table.NewRow();
                    d_Row["Sr"] = d_table.Rows.Count + 1;
                    d_Row["Selected"] = true;
                    d_table.Rows.Add(d_Row);
                }
                CurrentTable = d_table;
                BindGrid();
            }
        }

        public void UpdateTable()
        {
            if (CurrentTable != null)
            {
                string srNo, retailerCode, retailerId, lastVisit, remarks;
                bool isSelected;

                GetColumns();
                rowsCount = CurrentTable.Rows.Count;
                for (int i = 0; i < rowsCount; i++)
                {
                    d_Row = d_table.NewRow();
                    srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lblSr")).Text;
                    retailerCode = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRetailer")).Text;
                    retailerId = ((System.Web.UI.WebControls.HiddenField)GridView1.Rows[i].FindControl("hdnRetailerId")).Value;
                    lastVisit = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtLastVisit")).Text;

                    remarks = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text;
                    isSelected = ((System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("chkSelect")).Checked;
                    if (! string.IsNullOrEmpty( retailerCode.Trim()))
                    {
                        d_Row["Sr"] = srNo;
                        d_Row["Retailer"] = retailerCode;
                        d_Row["RetailerId"] = retailerId;
                        d_Row["LastVisit"] = lastVisit;
                        d_Row["Remarks"] = remarks;
                        d_Row["Selected"] = isSelected;
                        d_table.Rows.Add(d_Row);
                    }
                }
                CurrentTable = d_table;//assigning to viewstate datatable
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
            d_Col.ColumnName = "Retailer";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "RetailerId";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "LastVisit";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.String");
            d_Col.ColumnName = "Remarks";
            d_table.Columns.Add(d_Col);

            d_Col = new DataColumn();
            d_Col.DataType = System.Type.GetType("System.Boolean");
            d_Col.ColumnName = "Selected";
            d_table.Columns.Add(d_Col);
        }
        public void BindTable()
        {
            GetColumns();
            for (int i = 0; i < 5; i++)
            {
                d_Row = d_table.NewRow();
                d_Row["Sr"] = i + 1;
                d_Row["Selected"] = true;
                d_table.Rows.Add(d_Row);
            }
            CurrentTable = d_table;
            BindGrid();
        }
        //private EntitySet<tblSdVisitDet> GetVisitDetailForSave(tblSdVisit tblVisitData)
        //{
        //    string srNo, retailerCode, retailerId, remarks;
        //    bool isSelected;

        //    entitySetRetailerVisitsDet = new EntitySet<tblSdVisitDet>();

        //    rowsCount = GridView1.Rows.Count;
        //    for (int i = 0; i < rowsCount; i++)
        //    {
        //        srNo = ((System.Web.UI.WebControls.Label)GridView1.Rows[i].FindControl("lblSr")).Text;
        //        retailerCode = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRetailer")).Text;
        //        retailerId = ((System.Web.UI.WebControls.HiddenField)GridView1.Rows[i].FindControl("hdnRetailerId")).Value;
        //        //lastVisit = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtLastVisit")).Text;
        //        remarks = ((System.Web.UI.WebControls.TextBox)GridView1.Rows[i].FindControl("txtRemarks")).Text;
        //        isSelected = ((System.Web.UI.WebControls.CheckBox)GridView1.Rows[i].FindControl("chkSelect")).Checked;

        //        if (!string.IsNullOrEmpty(retailerCode.Trim()) && Convert.ToBoolean(isSelected))// && !itemCode.Trim().Equals("0")&& !duedte.Trim().Equals("")  && !spec.Trim().Equals("")&& !qty.Trim().Equals("") && !rate.Trim().Equals("")
        //        {
        //            tblSdVisitDet visitDet = new tblSdVisitDet();
        //            visitDet.vr_id = tblVisitData.vr_id;
        //            visitDet.vr_seq = Convert.ToInt32(srNo);
        //            visitDet.shop_id = Convert.ToInt32(retailerId);
        //            visitDet.remarks = remarks;
        //            entitySetRetailerVisitsDet.Add(visitDet);
        //        }
        //    }

        //    return entitySetRetailerVisitsDet;

        //}
        //public void BindTableEdit(List<tblSdVisitDet> dets)
        //{
        //    GetColumns();
        //    foreach (var dt in dets)
        //    {
        //        d_Row = d_table.NewRow();
        //        d_Row["Sr"] = dt.vr_seq;
        //        d_Row["RetailerId"] = dt.shop_id;
        //        d_Row["Retailer"] = new AreaCodeBL().GetDetailByID(Convert.ToInt32(dt.shop_id), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ar_dsc;
        //        d_Row["LastVisit"] = new RetailerVisitsBL().GetLastVisitDateByShopID(Convert.ToInt32(dt.shop_id), (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //        d_Row["Remarks"] = dt.remarks;
        //        d_Row["Selected"] = true;
        //        d_table.Rows.Add(d_Row);

        //    }
        //    CurrentTable = d_table;
        //    BindGrid();
        //}

        public void BindGrid()
        {
            GridView1.DataSource = CurrentTable;
            GridView1.DataBind();
        }

        #region WebMethods

        //[WebMethod]
        //public static object GetRetailer(string searchText)
        //{
        //    return new AreaCodeBL().GetDetaildRecordsForRetailer(searchText, (RMSDataContext)(HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]));
        //}

        //[WebMethod]
        //public static string GetLastVisitDate(string shopId)
        //{
        //    return new RetailerVisitsBL().GetLastVisitDateByShopID(Convert.ToInt32(shopId), (RMSDataContext)(HttpContext.Current.Session[HttpContext.Current.Session["UserID"] + "rmsDBObj"]));
        //}

        #endregion
    }
}





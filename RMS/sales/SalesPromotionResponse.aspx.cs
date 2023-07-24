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

namespace RMS.sales
{
    public partial class SalesPromotionResponse : BasePage
    {

        #region DataMembers

        ResponseBL responseBL = new ResponseBL();
        AreaCodeBL areaCodeBL = new AreaCodeBL();
        ArtifactsBL artifactsBL = new ArtifactsBL();
        DataTable d_table = new DataTable();

        //EntitySet<tblSDPromDataDet> entitySetPromDataDet = new EntitySet<tblSDPromDataDet>();

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

#pragma warning disable CS0114 // 'SalesPromotionResponse.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'SalesPromotionResponse.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
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

        public int ResponseTypeID
        {
            get { return Convert.ToInt32(ViewState["ResponseTypeID"]); }
            set { ViewState["ResponseTypeID"] = value; }
        }

        public int ArtifactTypeID
        {
            get { return Convert.ToInt32(ViewState["ArtifactTypeID"]); }
            set { ViewState["ArtifactTypeID"] = value; }
        }

        public int VrID
        {
            get { return Convert.ToInt32(ViewState["VrID"]); }
            set { ViewState["VrID"] = value; }
        }

        public int OutletID
        {
            get { return Convert.ToInt32(ViewState["OutletID"]); }
            set { ViewState["OutletID"] = value; }
        }

        public Dictionary<int, int> ArtResponsePair
        {
            get { return (Dictionary<int, int>)(ViewState["ArtResponsePair"]); }
            set { ViewState["ArtResponsePair"] = value; }
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

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "SalesPromotionResponse").ToString();
                if (Session["DateFormat"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        //calFltFromDt.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        //calFltToDt.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
                        calDocDate.Format = Request.Cookies["uzr"]["DateFormat"].ToString();
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
                }

                calDocDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                //txtFltFromDt.Text = Convert.ToDateTime(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Year.ToString() + "-" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month.ToString() + "-01").ToString("dd-MMM-yy");
                //txtFltToDt.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yy");

                BindOutletDropDown();
                BindArtifactTypesDropDown();
                BindResponseTypeDropDown();
                BindFilterOutletDropDown();
                BindFilterArtifactTypeDropDown();
                BindFilterResponseTypeDropDown();
                BindFooterDataGrid();

            }

            if (ArtifactTypeID != 0 && ResponseTypeID != 0)
            {
                BindResponseGrid(ArtifactTypeID, ResponseTypeID);
            }
            if (IsEdit)
            {
                //GetByID();
                SelectResponseRadios();
            }
        }

        protected void ddlArtifactType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlResponseType.SelectedIndex > 0)
            {
                ResponseTypeID = Convert.ToInt32(ddlResponseType.SelectedValue);
                ArtifactTypeID = Convert.ToInt32(ddlArtifactType.SelectedValue);
                BindResponseGrid(ArtifactTypeID, ResponseTypeID);
            }

        }

        protected void ddlResponseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlArtifactType.SelectedIndex > 0)
            {
                ResponseTypeID = Convert.ToInt32(ddlResponseType.SelectedValue);
                ArtifactTypeID = Convert.ToInt32(ddlArtifactType.SelectedValue);
                BindResponseGrid(ArtifactTypeID, ResponseTypeID);
            }
            else
            {
                ucMessage.ShowMessage("Please select artifact type.", RMS.BL.Enums.MessageType.Error);
                ddlResponseType.SelectedIndex = 0;
                ddlArtifactType.Focus();
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindFooterDataGrid();
        }

        #region data grid

        protected void grd_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime schDate = Convert.ToDateTime(grd.SelectedRow.Cells[1].Text).Date;
            if (schDate.Date < Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date)
            {
                ucMessage.ShowMessage("Sorry,you can not edit previous sales promotion responses.", RMS.BL.Enums.MessageType.Error);
            }
            else
            {
                IsEdit = true;
                VrID = Convert.ToInt32(grd.SelectedDataKey.Values["vr_id"]);
                OutletID = Convert.ToInt32(grd.SelectedDataKey.Values["DlrId"]);
                ArtifactTypeID = Convert.ToInt32(grd.SelectedDataKey.Values["ArtTypeId"]);
                ResponseTypeID = Convert.ToInt32(grd.SelectedDataKey.Values["RespTypeId"]);
                //GetByID();
                BindResponseGrid(ArtifactTypeID, ResponseTypeID);
                SelectResponseRadios();
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
            //BindGridMain(txtFltDocNo.Text.Trim(), ddlFltStatus.SelectedValue);
        }

        #endregion

        //private void GetByID()
        //{
        //    tblSDPromData tblPromD = responseBL.GetPromDataByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    DocNo = tblPromD.doc_no;
        //    DocNoFormated = DocNo.ToString().Substring(0, 4) + "/" + DocNo.ToString().Substring(4);
        //    txtDocNo.Text = DocNoFormated;

        //    if (Session["DateFormat"] == null)
        //    {
        //        if (Request.Cookies["uzr"] != null)
        //        {
        //            calDocDate.SelectedDate = tblPromD.doc_dt;
        //        }
        //        else
        //        {
        //            Response.Redirect("~/login.aspx");
        //        }
        //    }
        //    else
        //    {
        //        calDocDate.SelectedDate = tblPromD.doc_dt;
        //    }

        //    //txtDocDate.Text = tblPromD.doc_dt.ToString("dd-MMM-yy");

        //    ddlOutlet.SelectedValue = OutletID.ToString();
        //    ddlArtifactType.SelectedValue = ArtifactTypeID.ToString();
        //    ddlResponseType.SelectedValue = ResponseTypeID.ToString();

        //    List<tblSDPromDataDet> lstPromDetail = responseBL.GetAllPromDataDetByPromDataID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

        //    Dictionary<int, int> dicArtRespons = new Dictionary<int, int>();

        //    foreach (tblSDPromDataDet tbl in lstPromDetail)
        //    {
        //        dicArtRespons.Add(Convert.ToInt32(tbl.ArtId), Convert.ToInt32(tbl.RespId));
        //    }

        //    ArtResponsePair = dicArtRespons;

        //}

        private void SelectResponseRadios()
        {
            foreach (GridViewRow row in gvResp.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField hdnArtId = (HiddenField)row.FindControl("hdnArtifacts");

                    if (ArtResponsePair.ContainsKey(Convert.ToInt32(hdnArtId.Value)))
                    {
                        RadioButton rdb = (RadioButton)row.FindControl("rdo" + ArtResponsePair[Convert.ToInt32(hdnArtId.Value)]);
                        if (rdb != null)
                        {
                            rdb.Checked = true;
                        }
                    }
                }
            }
        }

        private void BindFooterDataGrid()
        {
            int outletId = Convert.ToInt32(ddlFltOutlet.SelectedValue);
            int artTypeId = Convert.ToInt32(ddlFltArtType.SelectedValue);
            int respTypeId = Convert.ToInt32(ddlFltRespType.SelectedValue);

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

           // grd.DataSource = responseBL.GetAllPromotionData(docNumber, outletId, artTypeId, respTypeId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grd.DataBind();
        }

        private void BindResponseTypeDropDown()
        {
            ddlResponseType.DataSource = responseBL.GetAllResponseTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlResponseType.DataValueField = "RespTypeId";
            ddlResponseType.DataTextField = "Desc";
            ddlResponseType.DataBind();
        }

        protected void BindArtifactTypesDropDown()
        {
            ddlArtifactType.DataValueField = "ArtTypeId";
            ddlArtifactType.DataTextField = "Desc";
           // ddlArtifactType.DataSource = artifactsBL.GetAllArtifactTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlArtifactType.DataBind();
        }

        public void BindOutletDropDown()
        {
            ddlOutlet.DataSource = areaCodeBL.GetAllDetaildRecords((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlOutlet.DataValueField = "id";
            ddlOutlet.DataTextField = "ar_dsc";
            ddlOutlet.DataBind();
        }

        protected void BindFilterArtifactTypeDropDown()
        {
            ddlFltArtType.DataValueField = "ArtTypeId";
            ddlFltArtType.DataTextField = "Desc";
            //ddlFltArtType.DataSource = artifactsBL.GetAllArtifactTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltArtType.DataBind();
        }

        public void BindFilterResponseTypeDropDown()
        {
            ddlFltRespType.DataSource = responseBL.GetAllResponseTypes((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltRespType.DataValueField = "RespTypeId";
            ddlFltRespType.DataTextField = "Desc";
            ddlFltRespType.DataBind();
        }

        public void BindFilterOutletDropDown()
        {
            ddlFltOutlet.DataSource = areaCodeBL.GetAllDetaildRecords((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlFltOutlet.DataValueField = "id";
            ddlFltOutlet.DataTextField = "ar_dsc";
            ddlFltOutlet.DataBind();
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

                //Save();
                BindFooterDataGrid();
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
        //    tblSDPromData tblPromData = new tblSDPromData();
        //    if (IsEdit)
        //    {
        //        tblPromData = responseBL.GetPromDataByID(VrID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    }
        //    else
        //    {
        //        GetDocNo();
        //        tblPromData.doc_no = DocNo;
        //    }
        //    try
        //    {
        //        tblPromData.doc_dt = Convert.ToDateTime(txtDocDate.Text.Trim());
        //    }
        //    catch
        //    {
        //        tblPromData.doc_dt = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    }
        //    tblPromData.DlrId = Convert.ToInt32(ddlOutlet.SelectedValue);
        //    tblPromData.ArtTypeId = Convert.ToInt32(ddlArtifactType.SelectedValue);
        //    tblPromData.RespTypeId = Convert.ToInt32(ddlResponseType.SelectedValue);
        //    tblPromData.CreatedOn = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        //    tblPromData.vr_apr = 'A';

        //    entitySetPromDataDet = new EntitySet<tblSDPromDataDet>();

        //    foreach (GridViewRow row in gvResp.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            bool isAll = true;
        //            tblSDPromDataDet tblPromDataDet = new tblSDPromDataDet();

        //            HiddenField hdnArtId = (HiddenField)row.FindControl("hdnArtifacts");
        //            tblPromDataDet.ArtId = Convert.ToInt32(hdnArtId.Value);
        //            for (int i = 1; i < gvResp.Columns.Count; i++)
        //            {
        //                RadioButton rdb = (RadioButton)row.Cells[i].Controls[0];
        //                if (rdb.Checked)
        //                {
        //                    tblPromDataDet.RespId = Convert.ToInt32(rdb.ID.Replace("rdo", string.Empty));
        //                    isAll = false;
        //                }
        //            }
        //            if (isAll)
        //            {
        //                //NA
        //                tblPromDataDet.RespId = 0;
        //            }

        //            tblPromDataDet.vr_id = tblPromData.vr_id;
        //            tblPromDataDet.vr_seq = Convert.ToByte(row.RowIndex);
        //            entitySetPromDataDet.Add(tblPromDataDet);
        //        }
        //    }



        //    if (IsEdit)
        //    {
        //        string msg = responseBL.UpdatePromotionData(tblPromData, entitySetPromDataDet, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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
        //        tblPromData.tblSDPromDataDets = entitySetPromDataDet;
        //        string msg = responseBL.InsertPromotionData(tblPromData, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
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

        private void BindResponseGrid(int artTypeId, int respTypId)
        {
            //------------
            gvResp.Columns.Clear();
            //------------
            DataTable dt = new DataTable();

            //List<tblSdPromArt> lstArt = artifactsBL.GetAllArtifacts((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            List<tblSdPromArt> lstArt = artifactsBL.GetAllArtifactsByArtTypeId(artTypeId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            dt.Columns.Add("ArtId", typeof(System.String));
            dt.Columns.Add("Artifacts", typeof(System.String));

            List<tblSdPromResp> lstResp = responseBL.GetAllResponsesByRespTypeID(respTypId, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            TemplateField artifactsTemplateField = new TemplateField();
            artifactsTemplateField.HeaderTemplate = new GridViewTemplate(ListItemType.Header, "Artifacts", "");
            artifactsTemplateField.ItemTemplate = new GridViewTemplate(ListItemType.Item, "Artifacts", "ArtId");
            //artifactsTemplateField.ItemTemplate = new GridViewTemplate(ListItemType.Item, "ArtId", "hidden");
            gvResp.Columns.Add(artifactsTemplateField);


            foreach (var resp in lstResp)
            {
                dt.Columns.Add(resp.RespId.ToString(), typeof(System.String));

                TemplateField tf = new TemplateField();

                tf.HeaderTemplate = new GridViewTemplate(ListItemType.Header, resp.Desc, "");
                tf.ItemTemplate = new GridViewTemplate(ListItemType.Item, resp.RespId.ToString(), "");
                tf.ItemStyle.Width = 50;
                tf.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvResp.Columns.Add(tf);

            }

            foreach (var art in lstArt)
            {
                DataRow dr = dt.NewRow();
                dr["ArtId"] = art.ArtId;
                dr["Artifacts"] = art.Desc;
                dt.Rows.Add(dr);
            }

            gvResp.DataSource = dt;
            gvResp.DataBind();
            
            dt.Dispose();
        }

        private void ClearFields()
        {
            IsEdit = false;
            calDocDate.SelectedDate = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            //txtDocDate.Text = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("dd-MMM-yy");
            DocNo = 0;
            DocNoFormated = string.Empty;
            ResponseTypeID = 0;
            ArtifactTypeID = 0;
            OutletID = 0;

            ddlOutlet.SelectedIndex = 0;
            ddlArtifactType.SelectedIndex = 0;
            ddlResponseType.SelectedIndex = 0;
            txtDocNo.Text = string.Empty;

            gvResp.DataSource = null;
            gvResp.DataBind();
        }

        public void GetDocNo()
        {
            //txtDocNo.Text = saleReqBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date,(RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNoFormated = responseBL.GetDocNo(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Date, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            DocNo = Convert.ToInt32(DocNoFormated.Substring(0, 4) + DocNoFormated.Substring(5));
        }

        #endregion

    }
}





using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.setup
{
    public partial class EmpScaleMgt : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
#pragma warning disable CS0114 // 'EmpScaleMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int? ID
#pragma warning restore CS0114 // 'EmpScaleMgt.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }
       


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindScaleGrid();
                //if (Session["DateTimeFormat"] == null)
                //{
                //    Response.Redirect("~/login.aspx");
                //} 

                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ScaleSet").ToString();

                //Response.Cookies["uzr"].Values["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "Home").ToString();
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

            }
        }




        protected void Button_Command(object sender, EventArgs e)
        {


           
            if (txtScaleName.Text.Trim() == "" || txtScaleName.Text.Trim() == null)
            {
                ucMessage.ShowMessage("Please Enter Scale Name", RMS.BL.Enums.MessageType.Error);
                return;
            }
            if (txtScaleDescription.Text.Trim() == "" || txtScaleDescription.Text.Trim() == null)
            {
                ucMessage.ShowMessage("Please Enter Scale Description", RMS.BL.Enums.MessageType.Error);
                return;
            }
            if (txtBasicMax.Text.Trim() == "" || txtBasicMax.Text.Trim() == null)
            {
                ucMessage.ShowMessage("Please Enter Basic Maximum", RMS.BL.Enums.MessageType.Error);
                return;
            }
            if (txtBasicMin.Text.Trim() == "" || txtBasicMin.Text.Trim() == null)
            {
                ucMessage.ShowMessage("Please Enter Basic Minimum", RMS.BL.Enums.MessageType.Error);
                return;
            }
            if (txtIncre.Text.Trim() == "" || txtIncre.Text.Trim() == null)
            {
                ucMessage.ShowMessage("Please Enter Increment Rate", RMS.BL.Enums.MessageType.Error);
                return;
            }


            InsertOrUpdate();
        }

        protected void InsertOrUpdate()
        {
            try
            {
                if(ID > 0)
                {
                    TblEmpScale empScaleVal = db.TblEmpScales.FirstOrDefault(x => x.ScaleID == ID);
                    empScaleVal.ScaleName = txtScaleName.Text.Trim();
                    empScaleVal.ScaleDescription = txtScaleDescription.Text.Trim();
                    empScaleVal.Maxminmum = Convert.ToInt32(txtBasicMax.Text.Trim());
                    empScaleVal.Minimum = Convert.ToInt32(txtBasicMin.Text.Trim());
                    empScaleVal.IncRate = Convert.ToInt32(txtIncre.Text.Trim());
                    db.SubmitChanges();
                }
                else
                {
                    TblEmpScale empScale = new TblEmpScale();
                    empScale.ScaleName = txtScaleName.Text.Trim();
                    empScale.ScaleDescription = txtScaleDescription.Text.Trim();
                    empScale.Maxminmum = Convert.ToInt32(txtBasicMax.Text.Trim());
                    empScale.Minimum = Convert.ToInt32(txtBasicMin.Text.Trim());
                    empScale.IncRate = Convert.ToInt32(txtIncre.Text.Trim());
                    db.TblEmpScales.InsertOnSubmit(empScale);
                    db.SubmitChanges();                    
                }
                ucMessage.ShowMessage(GetGlobalResourceObject("MainResource", "SavedSuccesfully").ToString(), RMS.BL.Enums.MessageType.Info);
                ClearFields();
                BindScaleGrid();
            }

            catch (Exception ex)
            {
                ucMessage.ShowMessage(ex.Message.ToString(), RMS.BL.Enums.MessageType.Error);

            }

        }

        string OnupdateEvent()
        {
            
            try
            {
                TblEmpScale empScaleVal = db.TblEmpScales.FirstOrDefault(x => x.ScaleID == ID);
               if (empScaleVal != null)
                {
                    txtScaleName.Text = empScaleVal.ScaleName.ToString();
                    txtScaleDescription.Text = empScaleVal.ScaleDescription.ToString();
                    txtBasicMax.Text = empScaleVal.Maxminmum.ToString();
                    txtBasicMin.Text = empScaleVal.Minimum.ToString();
                    txtIncre.Text = empScaleVal.IncRate.ToString();
                   
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
        protected void Clear_All(object sender, EventArgs e)
        {
            ClearFields();
        }
        protected void ClearFields()
        {
            ID = 0;
            txtScaleName.Text = "";
            txtScaleDescription.Text = "";
            txtBasicMax.Text = "";
            txtBasicMin.Text = "";
            txtIncre.Text = "";
        }


        protected void BindScaleGrid()
        {
            this.grdEmpScale.DataSource = (from sc in db.TblEmpScales
                                          orderby sc.Orderby
                                          select sc).ToList();
            this.grdEmpScale.DataBind();
        }


        protected void grdEmpScale_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEmpScale.PageIndex = e.NewPageIndex;
            BindScaleGrid();
        }

        protected void grdEmpScale_PageIndexChanged(object sender, EventArgs e)
        {

            ID = Convert.ToInt32(grdEmpScale.SelectedDataKey.Values["ScaleID"].ToString());
            this.OnupdateEvent();
            //ClearFields();

        }
        //int Counter = 1;
    }
}
using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;

namespace RMS.Setup
{
    public partial class SalaryContent : BasePage
    {
        SalContentBL salContent = new SalContentBL();
        RMSDataContext db = new RMSDataContext();



#pragma warning disable CS0114 // 'SalaryContent.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'SalaryContent.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        public int Counter
        {
            get { return (ViewState["Counter"] == null) ? 0 : Convert.ToInt32(ViewState["Counter"]); }
            set { ViewState["Counter"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "AllDed").ToString();

                FillDropDownAllDdd();
                BindGridAllDd();
            }

        }


        private void FillDropDownAllDdd()
        {
            ddlAllDd.DataTextField = "Name";
            ddlAllDd.DataValueField = "SalaryContentTypeID";
            ddlAllDd.DataSource = salContent.GetAllDed((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlAllDd.DataBind();
        }

        protected void btnAll_Ddu(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                int allId = Convert.ToInt32(ddlAllDd.SelectedValue);
                RMS.BL.SalaryContent cont = new RMS.BL.SalaryContent();

                if (allId == 0)
                {
                    ucMessage.ShowMessage("Please Select Allowance / Deduction", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    cont.SalaryContentTypeID = allId;
                }
                if (txtName.Text == "")
                {
                    // txtNameError.Text = "Please Insert Name";
                    ucMessage.ShowMessage("Please Insert Name", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    cont.Name = txtName.Text.Trim();
                }
                if (txtSortReg.Text == "")
                {
                    // txtNameError.Text = "Please Insert Name";
                    ucMessage.ShowMessage("Please Set Sort Reference", BL.Enums.MessageType.Error);
                    return;
                }
                else
                {
                    cont.Sort = Convert.ToInt32(txtSortReg.Text.Trim());
                }
                //if (txtSize.Text == "")
                //{
                //    ucMessage.ShowMessage("Please Insert Size", BL.Enums.MessageType.Error);
                //    return;
                //}
                //else
                //{
                //    cont.Size = Convert.ToDecimal(txtSize.Text);
                //}
                //// cont.Size = Convert.ToDecimal(txtSize.Text.Trim());
                //bool chk = Convert.ToBoolean(checkIsPercen.Checked);
                //if (Convert.ToBoolean(checkIsPercen.Checked))
                //{
                //    cont.IsValue = Convert.ToBoolean(0);
                //}
                //else
                //{
                //    cont.IsValue = Convert.ToBoolean(1);
                //}

                cont.IsActive = Convert.ToBoolean(CheckIsActive.Checked);
                salContent.Insert(cont, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
            }
            else
            {
                int alldd = Convert.ToInt32(ddlAllDd.SelectedValue);
                RMS.BL.SalaryContent cont = salContent.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                cont.SalaryContentTypeID = alldd;
                cont.Name = txtName.Text.Trim();
                cont.Sort = Convert.ToInt32(txtSortReg.Text.Trim());
                
                //cont.Size = Convert.ToDecimal(txtSize.Text.Trim());
                ////cont.IsValue = Convert.ToBoolean(checkIsPercen.Checked);
                //if (Convert.ToBoolean(checkIsPercen.Checked))
                //{
                //    cont.IsValue = Convert.ToBoolean(0);
                //}
                //else
                //{
                //    cont.IsValue = Convert.ToBoolean(1);
                //}
                cont.IsActive = Convert.ToBoolean(CheckIsActive.Checked);
                salContent.update(cont, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                ucMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
               
            }
            BindGridAllDd();
            ClearField();
        }

        protected void grdAll_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = Counter++.ToString();


                //if (e.Row.Cells[4].Text.Equals("True"))
                //{
                //    e.Row.Cells[4].Text = "Yes";
                //}
                //else
                //{
                //    e.Row.Cells[4].Text = "No";
                //}


            }


        }

        protected void grdAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdAll.SelectedValue);

            RMS.BL.SalaryContent salaryContent = new RMS.BL.SalaryContent();
            salaryContent = salContent.GetByID(ID, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            ddlAllDd.SelectedValue = salaryContent.SalaryContentTypeID.ToString();
            this.txtName.Text = salaryContent.Name;
            txtSortReg.Text = salaryContent.Sort.ToString();
            //this.txtSize.Text = salaryContent.Size.ToString();
            this.CheckIsActive.Checked = Convert.ToBoolean(salaryContent.IsActive);
            //this.checkIsPercen.Checked = salaryContent.IsValue;

            //if (salaryContent.IsValue == false)
            //{
            //    this.checkIsPercen.Checked = true;
            //}
            //else
            //{
            //    this.checkIsPercen.Checked = false;
            //}
        }
        protected void grdAll_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAll.PageIndex = e.NewPageIndex;
            BindGridAllDd();

        }


        protected void BindGridAllDd()
        {
            Counter = 1;

            this.grdAll.DataSource = salContent.GetAllDedRep((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            this.grdAll.DataBind();
        }
        public void ClearField()
        {
            ID = 0;
            ddlAllDd.SelectedValue = "0";
            txtName.Text = "";
            txtSortReg.Text = "";
            //txtSize.Text = "";
            CheckIsActive.Checked = true;
            //checkIsPercen.Checked = false;
        }
    }
}

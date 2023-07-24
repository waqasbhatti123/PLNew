using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.Linq;

namespace RMS.GL.Setup
{
    public partial class frmBudgetEntry : BasePage
    {
        #region DataMembers
#pragma warning disable CS0169 // The field 'frmBudgetEntry.tbl' is never used
        tblBgtHeadDet tbl;
#pragma warning restore CS0169 // The field 'frmBudgetEntry.tbl' is never used
        GlBudgetSetupBL glB = new GlBudgetSetupBL();      
        #endregion

        #region Properties

        public string Code
        {
            get { return ViewState["Code"].ToString(); }
            set { ViewState["Code"] = value; }

        }
        public string bgtYear
        {
            get { return ViewState["bgtYear"].ToString(); }
            set { ViewState["bgtYear"] = value; }
        }

        public bool Edit
        {
            get { return Convert.ToBoolean(ViewState["Edit"]); }
            set { ViewState["Edit"] = value; }
        }

        #endregion


        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            int BrId = 0;
            if (Session["BranchID"] == null)
            {
                if (Request.Cookies["uzr"] != null)
                {
                    BrId = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }
            else
            {
                BrId = Convert.ToInt32(Session["BranchID"]);
            }
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "GlBudgetEntry").ToString();
               
                this.BindGrid();
                this.txtBudgetCode.Focus();

                txtBudgetYear.Text = (glB.getCurrnetFinYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"])).ToString();
               
                Edit = false;

                ddlFltBgtYear.DataSource = glB.getBudgetYear((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"]);
                ddlFltBgtYear.DataBind();
            }
  
        }

        protected void grdcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Code = grdcode.SelectedDataKey.Value.ToString().Trim();
               Code= grdcode.DataKeys[grdcode.SelectedIndex].Values["Bgt_Code"].ToString().Trim();
               bgtYear = grdcode.DataKeys[grdcode.SelectedIndex].Values["Bgt_Year"].ToString().Trim();
               
                Edit = true;

                txtBudgetCode.ReadOnly = true;
                lblQ1.Visible = true;
                lblQ2.Visible = true;
                lblQ3.Visible = true;
                lblQ4.Visible = true;

                txtQ1.Visible = true;
                txtQ2.Visible = true;
                txtQ3.Visible = true;
                txtQ4.Visible = true;
     
                getByDataKeys(Code, bgtYear);
            }
            catch (Exception ex)
            {
                //Session["errors"] = ex.Message;
                //Response.Redirect("~/home/Error.aspx");
                ucMessage.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
            }
        }

        
       
        protected void grdcode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdcode.PageIndex = e.NewPageIndex;
            this.BindGrid();
        }

        protected void grdcode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                if (e.Row.Cells[3].Text == "Y")
                {
                    e.Row.Cells[3].Text = "Yearly";
                }
                else if (e.Row.Cells[3].Text == "Q")
                {
                    e.Row.Cells[3].Text = "Quaterly";
                }
             
            }
        }


        protected void btnsearch_Click(object sender, EventArgs e)
        {
            string bgCd=txtFltBudgetCode.Text;
            if (bgCd == "")
            {
                bgCd = "";
            }
            string desc= txtFltDesc.Text;
            if (desc == "") 
            {
                desc = "";
            }
            char bgType;
            Decimal bgYr=Convert.ToDecimal(ddlFltBgtYear.SelectedValue);
            if (ddlFltBgtYear.SelectedValue == "0")
            {
                bgYr = 0000;
            }
          
             bgType = Convert.ToChar(ddlFltBudgetType.SelectedValue);
            if (ddlFltBudgetType.SelectedValue == "0")
            {
                bgType = ' ';
             
            }


            grdcode.DataSource = glB.getAllBgtEntryGrid((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"],bgYr,bgCd,desc,bgType);
            grdcode.DataBind();
            if (grdcode.Rows.Count == 0)
            {
                ucMessage.ShowMessage("No record found", RMS.BL.Enums.MessageType.Error);
            }
      
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (Edit == true)
            {
                this.Update(Code,bgtYear);
                ClearFields();
            }
            else
            {
                this.Insert();
                ClearFields();
            }
          
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();

            Edit = false;
        }

        protected void txtBudgetCode_TextChanged(object sender, EventArgs e)
        {
            //txtBudgetCode.Text = hdnBudgetCode.Value;
            
            int yr=Convert.ToInt32(txtBudgetYear.Text)-1;

            tblBgtHead tbl = glB.getBgtHeadData((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"],hdnBudgetCode.Value);
           // txtdescription.Text = tbl.Headg_Desc;
            txtAcFrom.Text = tbl.GL_AC_Fr;
            txtAcTo.Text = tbl.GL_AC_To;

            tblBgtHeadDet tblDet = glB.getLastYearBudget((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], yr.ToString());
            if (tblDet != null)
            {
                int q1 = Convert.ToInt32(tblDet.Q1_Amt);
                int q2 = Convert.ToInt32(tblDet.Q2_Amt);
                int q3 = Convert.ToInt32(tblDet.Q3_Amt);
                int q4 = Convert.ToInt32(tblDet.Q4_Amt);

                int sum = q1 + q2 + q3 + q4;

                txtLastYearBudget.Text = sum.ToString();
            }
            else
            {
                txtLastYearBudget.Text = "0";
            }

            string bgType= tbl.Bgt_Type.ToString();

            if(bgType.Equals("Y"))
            {
                txtBudgetType.Text = "Yearly";
                lblYearl.Visible = true;
                txtYearlyAmount.Visible =true;
                txtYearlyAmount.Focus();

                lblQ1.Visible = false;
                lblQ2.Visible = false;
                lblQ3.Visible = false;
                lblQ4.Visible = false;

                txtQ1.Visible = false;
                txtQ2.Visible = false;
                txtQ3.Visible = false;
                txtQ4.Visible = false;

                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = false;
                RequiredFieldValidator3.Enabled = false;
                RequiredFieldValidator4.Enabled = false;
                RequiredFieldValidator5.Enabled = false;


            }
            else if (bgType.Equals("Q"))
            {
                txtBudgetType.Text = "Quaterly";
                lblYearl.Visible = false;
                txtYearlyAmount.Visible = false;

                lblQ1.Visible = true;
                lblQ2.Visible = true;
                lblQ3.Visible = true;
                lblQ4.Visible = true;

                txtQ1.Visible = true;
                txtQ2.Visible = true;
                txtQ3.Visible = true;
                txtQ4.Visible = true;

                RequiredFieldValidator1.Enabled = false;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;

                txtQ1.Focus();
            }

            txtLastYearUtilized.Text = "0";
        }

        protected void txtYearlyAmount_textChanged(object sender, EventArgs e)
        {
            
            try
            {
                int amnt = Convert.ToInt32(txtYearlyAmount.Text) / 4;

                lblQ1.Visible = true;
                lblQ2.Visible = true;
                lblQ3.Visible = true;
                lblQ4.Visible = true;

                txtQ1.Visible = true;
                txtQ2.Visible = true;
                txtQ3.Visible = true;
                txtQ4.Visible = true;

                txtQ1.Text = amnt.ToString();
                txtQ2.Text = amnt.ToString();
                txtQ3.Text = amnt.ToString();
                txtQ4.Text = amnt.ToString();

                txtQ1.Focus();
        
            }
            catch(Exception ex)
            {
                ucMessage.ShowMessage(ex.ToString(), RMS.BL.Enums.MessageType.Error);
                txtYearlyAmount.Text = "";
                txtYearlyAmount.Focus();
            }
            
        }

       
        #endregion


        #region Helping Method
        protected void BindGrid()
        {
            grdcode.DataSource = glB.getAllBgtEntryGrid((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"],0000,"","",' ');
            grdcode.DataBind();
        }

       

        protected char GetGLTypeCode(string gl_cd)
        {
            return glB.GetGLTypeCode((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], gl_cd);

        }
        

        protected int GetCodeTypeLength(char gt_cd)
        {
           
            return glB.GetCodeTypeLength((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], gt_cd);
        }

        private void ClearFields()
        {
           
            Code = null;
            this.txtBudgetCode.ReadOnly = false;
            this.txtBudgetCode.Text = "";
           // this.txtdescription.Text = "";
            this.txtBudgetCode.Focus();
            this.txtBudgetType.Text = "";
            this.txtYearlyAmount.Text = "";
            this.txtLastYearBudget.Text = "";
            this.txtLastYearUtilized.Text = "";
            this.txtQ1.Text = "";
            this.txtQ2.Text = "";
            this.txtQ3.Text = "";
            this.txtQ4.Text = "";
            this.txtAcFrom.Text = "";
            this.txtAcTo.Text = "";
            this.BindGrid();
            
        }

        protected void DisableControl()
        {
            this.txtBudgetCode.Enabled = false;
        
            
        }
       

    
        protected void Insert()
        {
           // RMS.BL.tblBgtHead ctyR = new RMS.BL.tblBgtHead();
            RMS.BL.tblBgtHeadDet bgtHeadDet = new tblBgtHeadDet();
            bgtHeadDet.br_id = 1;

            decimal bgYr = Convert.ToDecimal(txtBudgetYear.Text);
            //string bgCd = txtBudgetCode.Text;
            string bgCd = hdnBudgetCode.Value;

            int q1 = Convert.ToInt32(txtQ1.Text);
            int q2 = Convert.ToInt32(txtQ2.Text);
            int q3 = Convert.ToInt32(txtQ3.Text);
            int q4 = Convert.ToInt32(txtQ4.Text);

            bgtHeadDet.Bgt_Code = bgCd;
            bgtHeadDet.Bgt_Year = bgYr;
            bgtHeadDet.Q1_Amt = q1;
            bgtHeadDet.Q2_Amt = q2;
            bgtHeadDet.Q3_Amt = q3;
            bgtHeadDet.Q4_Amt = q4;

            bgtHeadDet.Created_By = "admin";
            bgtHeadDet.Created_On = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

            if (glB.isExisting((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], bgCd, bgYr.ToString()) == false)
            {
                if (glB.Save((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"],bgtHeadDet) == true)
                {
                    ucMessage.ShowMessage("Saved Successfully", RMS.BL.Enums.MessageType.Info); 
                }
                else
                {
                    ucMessage.ShowMessage("Can not save", RMS.BL.Enums.MessageType.Error);
                }
            }
            else
            {
                ucMessage.ShowMessage("Record already exists", RMS.BL.Enums.MessageType.Error);
            }
 
            
        }
        
        protected void Update(string code,string bgyear)
        {
            Edit = false;
            txtBudgetCode.ReadOnly = false;
           // tbl = glB.getRecordForUpdate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"],code,Convert.ToDecimal(bgyear));
            //glB.RefreshBgtEntry((RMSDataContext)Session[Session["UserID"] + "rmsDBobj"], tbl);
                
            
            int q1 = Convert.ToInt32(txtQ1.Text);
            int q2 = Convert.ToInt32(txtQ2.Text);
            int q3 = Convert.ToInt32(txtQ3.Text);
            int q4 = Convert.ToInt32(txtQ4.Text);

           
            //tbl.Q1_Amt = q1;
            //tbl.Q2_Amt = q2;
            //tbl.Q3_Amt = q3;
            //tbl.Q4_Amt = q4;

            //tbl.Updated_By = "admin";
            //tbl.Updated_On = Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);

             glB.getRecordForUpdate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], code, Convert.ToDecimal(bgyear),q1,q2,q3,q4,"admin", Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]));

            ////if (glB.isExisting((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], code, bgyear.ToString()) == false)
            ////{
            //    glB.updateBgEntry((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], tbl);

            //    if (glB.updateBgEntry((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], tbl) == true)
            //    {
                  ucMessage.ShowMessage("Update Successfully", RMS.BL.Enums.MessageType.Info);
            //    }
            //    else
            //    {
            //        ucMessage.ShowMessage("Can not update", RMS.BL.Enums.MessageType.Error);
            //    }
            
            ////else
            ////{
            ////    ucMessage.ShowMessage("Record already exists", RMS.BL.Enums.MessageType.Error);
            ////}
 


        }

        public void getByDataKeys(string cd, string yr)
        {

            decimal year= Convert.ToDecimal(yr);
            List<string> ob=glB.getByDataKeys((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], cd, year);

            txtBudgetCode.Text = ob[0]+"-"+ob[2];
            
                string bgType= ob[1];
            if(bgType.Equals("Y"))
            {
                txtBudgetType.Text = "Yearly";
            }
            else if(bgType.Equals("Q"))
            {
                txtBudgetType.Text = "Quarterly";
            }

            //txtdescription.Text=ob[2];

            txtAcFrom.Text=ob[3];
            txtAcTo.Text=ob[4];
            txtBudgetYear.Text=ob[5];

            txtQ1.Text=ob[6];
            txtQ2.Text=ob[7];
            txtQ3.Text=ob[8];
            txtQ4.Text=ob[9];

            txtYearlyAmount.Text = (Convert.ToInt32(txtQ1.Text) + Convert.ToInt32(txtQ2.Text) + Convert.ToInt32(txtQ3.Text) + Convert.ToInt32(txtQ4.Text)).ToString() ;
            //-------------------------------------
            tblBgtHeadDet tblDet = glB.getLastYearBudget((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"], (year-1).ToString());
            if (tblDet != null)
            {
                int q1 = Convert.ToInt32(tblDet.Q1_Amt);
                int q2 = Convert.ToInt32(tblDet.Q2_Amt);
                int q3 = Convert.ToInt32(tblDet.Q3_Amt);
                int q4 = Convert.ToInt32(tblDet.Q4_Amt);

                int sum = q1 + q2 + q3 + q4;

                txtLastYearBudget.Text = sum.ToString();
            }
            else
            {
                txtLastYearBudget.Text = "0";
            }
            //-------------------------------------
        
        }

     
        #endregion
    }
}

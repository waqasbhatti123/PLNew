using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RMS.BL;
using RMS.UserControl;

namespace RMS.InvSetupSupport
{
    public partial class ViewIgp : System.Web.UI.Page
    {
        #region DataMembers

        InvMN_BL mpnBl = new InvMN_BL();

        #endregion

        #region Properties

        public string PmnRef
        {
            get { return Convert.ToString(ViewState["PmnRef"]); }
            set { ViewState["PmnRef"] = value; }
        }

        public int GrdQty
        {
            get { return Convert.ToInt32(ViewState["GrdQty"]); }
            set { ViewState["GrdQty"] = value; }
        }

        public int GrdArea
        {
            get { return Convert.ToInt32(ViewState["GrdArea"]); }
            set { ViewState["GrdArea"] = value; }
        }

        public int FtgQty
        {
            get { return Convert.ToInt32(ViewState["FtgQty"]); }
            set { ViewState["FtgQty"] = value; }
        }

        public int FtgArea
        {
            get { return Convert.ToInt32(ViewState["FtgArea"]); }
            set { ViewState["FtgArea"] = value; }
        }

        public string PrevLotGrd
        {
            get { return Convert.ToString(ViewState["PrevLotGrd"]); }
            set { ViewState["PrevLotGrd"] = value; }
        }

        public string PrevLotFtg
        {
            get { return Convert.ToString(ViewState["PrevLotFtg"]); }
            set { ViewState["PrevLotFtg"] = value; }
        }

        public decimal IgpQty
        {
            get { return Convert.ToDecimal(ViewState["IgpQty"]); }
            set { ViewState["IgpQty"] = value; }
        }

        public double IgpPrice
        {
            get { return Convert.ToDouble(ViewState["IgpPrice"]); }
            set { ViewState["IgpPrice"] = value; }
        }
        #endregion
        
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                PmnRef = Request.QueryString["ID"];
                if (PmnRef != "")
                {
                    try
                    {
                        if (Session["UserID"] == null)
                        {
                            if (Request.Cookies["uzr"] == null)
                            {
                                lblIGP.Visible = false;
                                
                                lblMsg.Visible = true;
                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                lblMsg.Text = "Please close the window and login again.";
                            }
                        }
                        if (Session["UserID"] != null || Request.Cookies["uzr"] != null)
                        {
                            BindDDLGrading();
                            BindDDLFeetage();
                            BindIGPgrid();
                            IgpQty = 0;
                            IgpPrice = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        protected void grdGrading_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text != "Sub Total")
                {
                    GrdArea = GrdArea + Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[3].Text));
                    e.Row.Cells[3].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[3].Text)).ToString();
                    GrdQty = GrdQty + Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[2].Text));
                    e.Row.Cells[2].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[2].Text)).ToString();
                    if (Convert.ToDecimal(e.Row.Cells[2].Text) > 0)
                    {
                        e.Row.Cells[4].Text = Math.Round((Convert.ToDecimal(e.Row.Cells[3].Text) / Convert.ToDecimal(e.Row.Cells[2].Text)), 2).ToString();
                    }
                    else
                    {
                        e.Row.Cells[4].Text = "0";
                    }
                }
                else
                {
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[1].Text = "Sub Total";
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[2].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[2].Text)).ToString();
                    e.Row.Cells[3].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[3].Text)).ToString();
                    if (Convert.ToDecimal(e.Row.Cells[2].Text) > 0)
                    {
                        e.Row.Cells[4].Text = Math.Round((Convert.ToDecimal(e.Row.Cells[3].Text) / Convert.ToDecimal(e.Row.Cells[2].Text)), 2).ToString();
                    }
                    else
                    {
                        e.Row.Cells[4].Text = "0";
                    }
                    e.Row.BackColor = System.Drawing.Color.Ivory;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total";
                e.Row.Cells[2].Text = GrdQty.ToString();
                e.Row.Cells[3].Text = GrdArea.ToString();
                e.Row.Cells[4].Text = (Convert.ToDecimal(GrdArea) / Convert.ToDecimal(GrdQty)).ToString("F");
                e.Row.Font.Size = 8;
                e.Row.Cells[1].Font.Bold = true;
                e.Row.Cells[2].Font.Bold = true;
                e.Row.Cells[3].Font.Bold = true;
                e.Row.Cells[4].Font.Bold = true;

            }
            
        }

        protected void grdFeetage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text != "Sub Total")
                {
                    FtgArea = FtgArea + Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[3].Text));
                    e.Row.Cells[3].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[3].Text)).ToString();
                    FtgQty = FtgQty + Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[2].Text));
                    e.Row.Cells[2].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[2].Text)).ToString();
                }
                else
                {
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[1].Text = "Sub Total";
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[2].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[2].Text)).ToString();
                    e.Row.Cells[3].Text = Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[3].Text)).ToString();
                    if (Convert.ToDecimal(e.Row.Cells[2].Text) > 0)
                    {
                        e.Row.Cells[4].Text = Math.Round((Convert.ToDecimal(e.Row.Cells[3].Text) / Convert.ToDecimal(e.Row.Cells[2].Text)), 2).ToString();
                    }
                    else
                    {
                        e.Row.Cells[4].Text = "0";
                    }
                    e.Row.BackColor = System.Drawing.Color.Ivory;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total";
                e.Row.Cells[2].Text = FtgQty.ToString();
                e.Row.Cells[3].Text = FtgArea.ToString();
                e.Row.Cells[4].Text =(Convert.ToDecimal(FtgArea) / Convert.ToDecimal(FtgQty)).ToString("F");
                e.Row.Font.Size = 8;
                e.Row.Cells[1].Font.Bold = true;
                e.Row.Cells[2].Font.Bold = true;
                e.Row.Cells[3].Font.Bold = true;
                e.Row.Cells[4].Font.Bold = true;

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IgpQty+=Convert.ToDecimal(e.Row.Cells[5].Text);
                IgpPrice+=Convert.ToDouble(e.Row.Cells[6].Text);
                e.Row.Cells[5].Text =Convert.ToInt32(Convert.ToDecimal(e.Row.Cells[5].Text)).ToString() ;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Total:";
                e.Row.Cells[5].Text = Convert.ToInt32(IgpQty).ToString();
                e.Row.Cells[6].Text = IgpPrice.ToString();

                e.Row.Cells[4].Font.Bold = true;
                e.Row.Cells[5].Font.Bold = true;
                e.Row.Cells[6].Font.Bold = true;
                e.Row.Font.Size = 8;
            }
        }

        #endregion

        #region HelpingMethods

        public void BindDDLGrading()
        {
            grdGrading.DataSource = mpnBl.GetGradingRecByIGP(PmnRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdGrading.DataBind();
        }

        public void BindDDLFeetage()
        {
            grdFeetage.DataSource = mpnBl.GetFeetageRecByIGP(PmnRef, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            grdFeetage.DataBind();
        }

        public void BindIGPgrid()
        {
            string Doc_RefFirst = Session["getFirstDocRef"].ToString();
            GridView1.DataSource = mpnBl.GetIGPByDocRef(Doc_RefFirst, "Srigp", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            GridView1.DataBind();
        }

        #endregion
    }
}

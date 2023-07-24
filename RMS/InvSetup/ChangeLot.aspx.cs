using System;
using System.Linq;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Data.Linq;

namespace RMS.InvSetup
{
    public partial class ChangeLot : BasePage
    {

        #region DataMembers

        AlterLot alter = new AlterLot();
        List<Anonymous4Grid> itmList1 = new List<Anonymous4Grid>();
        List<Anonymous4Grid> itmList2 = new List<Anonymous4Grid>();

        #endregion

        #region Properties

        public int existingLot
        {
            get { return (ViewState["existingLot"] == null) ? 0 : Convert.ToInt32(ViewState["existingLot"]); }
            set { ViewState["existingLot"] = value; }
        }

        public int newLot
        {
            get { return (ViewState["newLot"] == null) ? 0 : Convert.ToInt32(ViewState["newLot"]); }
            set { ViewState["newLot"] = value; }
        }

        public int confrmLot
        {
            get { return (ViewState["confrmLot"] == null) ? 0 : Convert.ToInt32(ViewState["confrmLot"]); }
            set { ViewState["confrmLot"] = value; }
        }

        public bool SaveClick
        {
            get { return Convert.ToBoolean(ViewState["SaveClick"]); }
            set { ViewState["SaveClick"] = value; }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "ChangeLotNo").ToString();
                SaveClick = false;
            }
        }

        protected void lnkExstLot_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtExistingLot.Text.Contains('-'))
                {
                    existingLot = Convert.ToInt32(txtExistingLot.Text.Substring(0, 4) + txtExistingLot.Text.Substring(5));
                }
                else
                {
                    ucMessage.ShowMessage("Existing lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                    goto Out;
                }
            }
            catch
            {
                ucMessage.ShowMessage("Existing lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                goto Out;
            }

            CheckExistingLotStatus();

        Out:
            DoNothing();
        }

        protected void lnkChkStts_Click(object sender, EventArgs e)
        {
            int existlot = 0;
            string eixstinglotyr = "";
            string existinglotmnth = "";
            try
            {
                if (txtNewLot.Text.Contains('-'))
                {
                    newLot = Convert.ToInt32(txtNewLot.Text.Substring(0, 4) + txtNewLot.Text.Substring(5));

                    try
                    {
                        existlot = Convert.ToInt32(txtExistingLot.Text.Substring(0, 4) + txtExistingLot.Text.Substring(5));
                        eixstinglotyr = existlot.ToString().Substring(0, 2);
                        existinglotmnth = existlot.ToString().Substring(2, 2);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Existing lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                        goto Out;
                    }

                    string lotyr = txtNewLot.Text.Substring(0, 2);
                    string lotmnth = txtNewLot.Text.Substring(2, 2);
                    
                    //if (Convert.ToInt32(lotyr) > Convert.ToInt32(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("yy")))
                    if (Convert.ToInt32(lotyr) > Convert.ToInt32(eixstinglotyr))
                    {
                        ucMessage.ShowMessage("Lot year must be less than or equal to existing lot year.", RMS.BL.Enums.MessageType.Error);
                        goto Out;
                    }
                    //else if (Convert.ToInt32(lotmnth) > Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month)
                    else if (Convert.ToInt32(lotmnth) > Convert.ToInt32(existinglotmnth))
                    {
                        ucMessage.ShowMessage("Lot month must be less than or equal to existing lot month.", RMS.BL.Enums.MessageType.Error);
                        goto Out;
                    }
                    else
                    {
                    }
                }
                else
                {
                    ucMessage.ShowMessage("New lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                    goto Out;
                }
            }
            catch
            {
                ucMessage.ShowMessage("New lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                goto Out;
            }
           
            CheckNewLotStatus();
                
        Out:
            DoNothing();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveClick = true;
            int existlot = 0;
            string eixstinglotyr = "";
            string existinglotmnth = "";
            
            try
            {
                if (txtExistingLot.Text.Contains('-'))
                {
                    existingLot = Convert.ToInt32(txtExistingLot.Text.Substring(0, 4) + txtExistingLot.Text.Substring(5));
                }
                else
                {
                    ucMessage.ShowMessage("Existing lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                    goto Out;
                }

            }
            catch
            {
                ucMessage.ShowMessage("Existing lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                goto Out;
            }
            
            try
            {
                //if (txtNewLot.Text.Contains('-'))
                //{
                //    newLot = Convert.ToInt32(txtNewLot.Text.Substring(0, 4) + txtNewLot.Text.Substring(5));

                //    string lotyr = txtNewLot.Text.Substring(0, 2);
                //    string lotmnth = txtNewLot.Text.Substring(2, 2);

                //    if (Convert.ToInt32(lotyr) > Convert.ToInt32(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("yy")))
                //    {
                //        ucMessage.ShowMessage("Lot year must be less than or equal to current year.", RMS.BL.Enums.MessageType.Error);
                //        goto Out;
                //    }
                //    else if (Convert.ToInt32(lotmnth) > Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month)
                //    {
                //        ucMessage.ShowMessage("Lot month must be less than or equal to current month.", RMS.BL.Enums.MessageType.Error);
                //        goto Out;
                //    }
                //    else
                //    {
                //    }

                //}
                //else
                //{
                //    ucMessage.ShowMessage("New lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                //    goto Out;
                //}

                if (txtNewLot.Text.Contains('-'))
                {
                    newLot = Convert.ToInt32(txtNewLot.Text.Substring(0, 4) + txtNewLot.Text.Substring(5));

                    try
                    {
                        existlot = Convert.ToInt32(txtExistingLot.Text.Substring(0, 4) + txtExistingLot.Text.Substring(5));
                        eixstinglotyr = existlot.ToString().Substring(0, 2);
                        existinglotmnth = existlot.ToString().Substring(2, 2);
                    }
                    catch
                    {
                        ucMessage.ShowMessage("Existing lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                        goto Out;
                    }

                    string lotyr = txtNewLot.Text.Substring(0, 2);
                    string lotmnth = txtNewLot.Text.Substring(2, 2);

                    //if (Convert.ToInt32(lotyr) > Convert.ToInt32(Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).ToString("yy")))
                    if (Convert.ToInt32(lotyr) > Convert.ToInt32(eixstinglotyr))
                    {
                        ucMessage.ShowMessage("Lot year must be less than or equal to existing lot year.", RMS.BL.Enums.MessageType.Error);
                        goto Out;
                    }
                    //else if (Convert.ToInt32(lotmnth) > Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]).Month)
                    else if (Convert.ToInt32(lotmnth) > Convert.ToInt32(existinglotmnth))
                    {
                        ucMessage.ShowMessage("Lot month must be less than or equal to existing lot month.", RMS.BL.Enums.MessageType.Error);
                        goto Out;
                    }
                    else
                    {
                    }
                }
                else
                {
                    ucMessage.ShowMessage("New lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                    goto Out;
                }
            }
            catch
            {
                ucMessage.ShowMessage("New lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                goto Out;
            }
            
            try
            {
                if (txtConfirmLot.Text.Contains('-'))
                {
                    confrmLot = Convert.ToInt32(txtConfirmLot.Text.Substring(0, 4) + txtConfirmLot.Text.Substring(5));
                }
                else
                {
                    ucMessage.ShowMessage("Re-Type lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                    goto Out;
                }

            }
            catch
            {
                ucMessage.ShowMessage("Re-Type lot no format is incorrect.", RMS.BL.Enums.MessageType.Error);
                goto Out;
            }

            
            bool exstlotstts = CheckExistingLotStatus();
            bool newlotstts  = CheckNewLotStatus();

            GridView1.Visible = false;
            GridView2.Visible = false;
            grd1Hd.Visible = false;
            grd2Hd.Visible = false;
            SaveClick = false;

            if (exstlotstts == true && newlotstts == true)
            {
                if (newLot == confrmLot)
                {
                    SaveNewLotNo();
                }
                else
                {
                    ucMessage.ShowMessage("New lot no must match to re-type lot no.", RMS.BL.Enums.MessageType.Error);
                }
            }

        Out:
            DoNothing();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll(); 
        }

        #endregion

        #region Helping Method

        public bool CheckExistingLotStatus()
        {
            bool res = false;
            itmList2 = alter.GetLotStatus(existingLot, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (itmList2.Count() < 1)
            {
                GridView2.Visible = false;
                grd2Hd.Visible = false;
                res = false;
                ucMessage1.ShowMessage("No lot found.", RMS.BL.Enums.MessageType.Info);
            }
            else
            {
                GridView2.Visible = true;
                grd2Hd.Visible = true;
                BindGridExistingLot();
                if (CheckMPNExist())
                {
                    res = false;
                    ucMessage1.ShowMessage("Cannot change lot no as MPN exists.", RMS.BL.Enums.MessageType.Error);
                }
                else
                {
                    res = true;
                }
            }
            return res;
        }

        public bool CheckMPNExist()
        {
            return alter.CheckMpnExist(existingLot, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
        }

        public bool CheckNewLotStatus()
        {
            bool res = false;

            itmList1 = alter.GetLotStatus(newLot, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (itmList1.Count() < 1)
            {
                GridView1.Visible = false;
                grd1Hd.Visible = false;
                res = true;
                if (SaveClick == false)
                {
                    ucMessage.ShowMessage("New lot no is available.", RMS.BL.Enums.MessageType.Info);
                }
            }
            else
            {
                GridView1.Visible = true;
                grd1Hd.Visible = true;
                res = false;
                BindGridNewLot();
                ucMessage.ShowMessage("New lot no already exists.", RMS.BL.Enums.MessageType.Error);
            }
            return res;
        }

        public void SaveNewLotNo()
        {
            bool res = false;

            res = alter.SaveLot(existingLot, newLot, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
            if (res == true)
            {
                txtExistingLot.Text = "";
                txtNewLot.Text = "";
                txtConfirmLot.Text = "";
                ucMessage.ShowMessage("Lot no. "+ newLot.ToString().Substring(0,4)+"-"+ newLot.ToString().Substring(4)+" saved successfully.", RMS.BL.Enums.MessageType.Info);
            }
            else
            {
                ucMessage.ShowMessage("Save was unsuccessful", RMS.BL.Enums.MessageType.Error);
            }
        }

        public void BindGridExistingLot()
        {
            GridView2.DataSource = itmList2;
            GridView2.DataBind();
        }

        public void BindGridNewLot()
        {
            GridView1.DataSource = itmList1;
            GridView1.DataBind();
        }

        public void ClearAll()
        {
           Response.Redirect("~/invSetup/ChangeLot.aspx?PID=449");
        }

        public void DoNothing()
        {
        }
      
        #endregion

    }
}

using Microsoft.Reporting.WebForms;
using RMS.BL;
using RMS.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.report
{
    public partial class EmpRelievingReport : System.Web.UI.Page
    {
        RMSDataContext db = new RMSDataContext();
        EmpBL empBL = new EmpBL();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "empreliv").ToString();
                if (Session["BranchID"] == null)
                {
                    if (Request.Cookies["uzr"] != null)
                    {
                        BranchID = Convert.ToInt32(Request.Cookies["uzr"]["BranchID"]);
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }
                }
                else
                {
                    BranchID = Convert.ToInt32(Session["BranchID"].ToString());
                }

                

                FillEmployeeDropDown();
                FillSearchBranchDropDown();
                searchBranchDropDown.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            }

            
        }

        protected void Report_Click(object sender, EventArgs e)
        {
            Warning[] warnings = null;
            String[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = "EXCEL";
            //ReportDataSource datasource = null;
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + extension + "</OutputFormat>" +
            "  <PageWidth>8.27in</PageWidth>" +
            "  <PageHeight>11.69in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.15in</MarginLeft>" +
            "  <MarginRight>0.15in</MarginRight>" +
            "  <MarginBottom>0.3in</MarginBottom>" +
            "</DeviceInfo>";

            string ddlrop = ddldropdown.SelectedValue;
            string ddldrop = "";
            if (ddlrop == "1" || ddlrop == "2" || ddlrop == "3" || ddlrop == "4" || ddlrop == "13")
            {
                ddldrop = "";
            }
            else
            {
                ddldrop = ddldropdown.SelectedValue;
            }

            int brI = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            int SortOrder = Convert.ToInt32(ddlSortOrder.SelectedValue);
            int desgID = Convert.ToInt32(ddlselectoption.SelectedValue);
            int appointed = Convert.ToInt32(ddlappoited.SelectedValue);
            int secID = Convert.ToInt32(ddlsection.SelectedValue);
            int fromscale = Convert.ToInt32(ddlfromScale.SelectedValue);
            int toscale = Convert.ToInt32(ddlToScale.SelectedValue);
            int jobType = Convert.ToInt32(ddlJobtype.SelectedValue);
            int fromage = Convert.ToInt32(ddlFromAge.SelectedValue);
            int toage = Convert.ToInt32(ddlToAge.SelectedValue);
            string domicile = ddlDomicile.SelectedValue;
            string Gender = ddlGender.SelectedValue;
            string disability = ddlDisablity.SelectedValue;
            string Religion = ddlReligion.SelectedValue;
            string addtional = ddlAddionReport.SelectedValue;
            string quota = ddlQuota.SelectedValue;
            string poli = ddlPoliceVerifi.SelectedValue;
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());


            List<sp_Profiledbo> con;
            con = empBL.getRelieving(brI, desgID, secID, fromscale, toscale, jobType, fromage, toage, domicile,
                Gender, disability, Religion, addtional, quota, poli, SortOrder, appointed, ddldrop, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            //viewer.LocalReport.ReportPath = "report/rdlc/AttenReport.rdlc";
            viewer.LocalReport.ReportPath = "report/rdlc/EmpRelievingExcel.rdlc";
            ReportDataSource datasource = new ReportDataSource("DataSet1", con);
            ReportParameter[] paramz = new ReportParameter[2];
            paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
            paramz[1] = new ReportParameter("colum", ddldrop);
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);

            Byte[] bytes = viewer.LocalReport.Render(extension == "XLS" ? "EXCEL" : extension, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", ("attachment; filename= Contact Report" + "_" + Common.MyDate((RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]) + ".") + extension);
            Response.BinaryWrite(bytes);
            //// create the file
            //// send it to the client to download
            Response.Flush();
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            string ddlrop = ddldropdown.SelectedValue;
            string ddldrop = "";
            if (ddlrop == "1" || ddlrop == "2" || ddlrop == "3" || ddlrop == "4" || ddlrop == "13")
            {
                ddldrop = "";
            }
            else
            {
                ddldrop = ddldropdown.SelectedValue;
            }
            //var Name = ddlEmployeeSearch.SelectedValue;
            //object Idss =  (object)searchBranchDropDown.SelectedValue;
            //object brID = (object)searchBranchDropDown.SelectedValue;
            int brI = Convert.ToInt32(searchBranchDropDown.SelectedValue);
            int SortOrder = Convert.ToInt32(ddlSortOrder.SelectedValue);
            int desgID = Convert.ToInt32(ddlselectoption.SelectedValue);
            int appointed = Convert.ToInt32(ddlappoited.SelectedValue);
            int secID = Convert.ToInt32(ddlsection.SelectedValue);
            int fromscale = Convert.ToInt32(ddlfromScale.SelectedValue);
            int toscale = Convert.ToInt32(ddlToScale.SelectedValue);
            int jobType = Convert.ToInt32(ddlJobtype.SelectedValue);
            int fromage = Convert.ToInt32(ddlFromAge.SelectedValue);
            int toage = Convert.ToInt32(ddlToAge.SelectedValue);
            string domicile = ddlDomicile.SelectedValue;
            string Gender = ddlGender.SelectedValue;
            string disability = ddlDisablity.SelectedValue;
            string Religion = ddlReligion.SelectedValue;
            string addtional = ddlAddionReport.SelectedValue;
            string quota = ddlQuota.SelectedValue;
             string poli =  ddlPoliceVerifi.SelectedValue;
            string rptLogoPath = "";
            rptLogoPath = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl(System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim());


            List<sp_Profiledbo> con;
            con = empBL.getRelieving(brI, desgID, secID, fromscale, toscale, jobType, fromage, toage, domicile,
                Gender, disability, Religion, addtional, quota, poli, SortOrder, appointed, ddldrop, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);


            //viewer.LocalReport.ReportPath = "report/rdlc/AttenReport.rdlc";
            viewer.LocalReport.ReportPath = "report/rdlc/rpttEmpRelieving.rdlc";
            ReportDataSource datasource = new ReportDataSource("DataSet1", con);
            
            ReportParameter[] paramz = new ReportParameter[2];
            //if (Session["CompName"] == null)
            //{
            //    paramz[0] = new ReportParameter("CompName", Request.Cookies["uzr"]["CompName"].ToString(), false);
            //}
            //else
            //{
            //    paramz[0] = new ReportParameter("CompName", Session["CompName"].ToString(), false);
            //}
            paramz[0] = new ReportParameter("LogoPath", rptLogoPath);
            paramz[1] = new ReportParameter("colum", ddldrop);
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.Refresh();
            viewer.LocalReport.SetParameters(paramz);
            viewer.LocalReport.DataSources.Clear();
            viewer.LocalReport.DataSources.Add(datasource);
            //ClearForm();
            
        }

        private void FillSearchBranchDropDown()
        {
            RMSDataContext db = new RMSDataContext();

            //this.searchBranchDropDown.DataTextField = "br_nme";
            //this.searchBranchDropDown.DataValueField = "br_id";
            //this.searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_id == 15).FirstOrDefault();
            //this.searchBranchDropDown.DataBind();

            Branch BranchObj = db.Branches.Where(x => x.br_id == BranchID).FirstOrDefault();

            this.searchBranchDropDown.DataTextField = "br_nme";
            searchBranchDropDown.DataValueField = "br_id";
            if (BranchObj.IsHead == true)
            {
                searchBranchDropDown.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            }
            else
            {
                List<Branch> BranchList = new List<Branch>();

                if (BranchObj != null)
                {
                    if (BranchObj.IsDisplay == true)
                    {
                        BranchList = db.Branches.Where(x => x.br_status == true && x.br_idd == BranchID).ToList();
                        BranchList.Insert(0, BranchObj);
                    }
                    else
                    {
                        BranchList.Add(BranchObj);
                    }
                }
                searchBranchDropDown.DataSource = BranchList.ToList();
            }
            searchBranchDropDown.DataBind();
            searchBranchDropDown.Items.Insert(0, new ListItem("All", "0"));

        }
        protected void FillEmployeeDropDown()
        {
           // int brID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
           
            //ddlEmployeeSearch.DataTextField = "FullName";
            //ddlEmployeeSearch.DataValueField = "FullName";
            //ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.BranchID == BranchID).ToList().OrderByDescending(x => x.ScaleID);
            
            //ddlEmployeeSearch.DataBind();
            //ddlEmployeeSearch.Items.Insert(0, new ListItem("ALL", "0"));
        }

        //protected void searchBranchDropDown_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int brID = Convert.ToInt32(searchBranchDropDown.SelectedValue);
        //        ddlEmployeeSearch.DataTextField = "FullName";
        //        ddlEmployeeSearch.DataValueField = "FullName";
        //        if (BranchID == 1)
        //        {
        //            if (brID == 0)
        //            {
        //                ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == true).ToList();
        //            }
        //            else
        //            {
        //                ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.BranchID == brID).ToList();
        //            }
        //        }
        //        else
        //        {
        //            ddlEmployeeSearch.DataSource = db.tblPlEmpDatas.Where(x => x.EmpStatus == true && x.BranchID == brID).ToList();
        //        }
                
        //        ddlEmployeeSearch.DataBind();
        //        ddlEmployeeSearch.Items.Insert(0, new ListItem("ALL", "0"));
        //}

        protected void option_SelectedIndexChanged(object sender, EventArgs e)
        {
            int option = Convert.ToInt32(ddldropdown.SelectedValue);
            if (option == 0)
            {
                ddlselectoption.Controls.Clear();
                ddlselectoption.Dispose();
                ddlselectoption.DataTextField = "";
                ddlselectoption.DataValueField = "";
                ddlselectoption.DataSource = "";
                ddlselectoption.DataBind();

                ddlsection.DataTextField = "";
                ddlsection.DataValueField = "";
                ddlsection.DataSource = null;
                ddlsection.DataBind();

                ddlappoited.Controls.Clear();
                ddlappoited.Dispose();
                ddlappoited.DataTextField = "";
                ddlappoited.DataValueField = "";
                ddlappoited.DataSource = "";
                ddlappoited.DataBind();
                ddlappoited.Items.Insert(0, new ListItem("Select", "0"));

                ddlfromScale.DataTextField = "";
                ddlfromScale.DataValueField = "";
                ddlfromScale.DataSource = "";
                ddlfromScale.DataBind();

                ddlToScale.DataTextField = "";
                ddlToScale.DataValueField = "";
                ddlToScale.DataSource = "";
                ddlToScale.DataBind();

                ddlJobtype.DataTextField = "";
                ddlJobtype.DataValueField = "";
                ddlJobtype.DataSource = "";
                ddlJobtype.DataBind();
            }
            else if (option == 1)
            {
                ddlselectoption.Controls.Clear();
                ddlselectoption.Dispose();
                ddlselectoption.DataTextField = "CodeDesc";
                ddlselectoption.DataValueField = "CodeID";
                ddlselectoption.DataSource = db.tblPlCodes.Where(x => x.Enabled == true && x.CodeTypeID == 4).OrderBy(x => x.sort).ToList();
                ddlselectoption.DataBind();
                ddlselectoption.Items.Insert(0, new ListItem("Select", "0"));
                ddlselectoption.Items.Insert(1, new ListItem("All", "1"));


                ddlappoited.Controls.Clear();
                ddlappoited.Dispose();
                ddlappoited.DataTextField = "";
                ddlappoited.DataValueField = "";
                ddlappoited.DataSource = "";
                ddlappoited.DataBind();
                ddlappoited.Items.Insert(0, new ListItem("Select", "0"));

                ddlsection.DataTextField = "";
                ddlsection.DataValueField = "";
                ddlsection.DataSource = "";
                ddlsection.DataBind();
                ddlsection.Items.Insert(0, new ListItem("Select", "0"));

                ddlfromScale.DataTextField = "";
                ddlfromScale.DataValueField = "";
                ddlfromScale.DataSource = "";
                ddlfromScale.DataBind();
                ddlfromScale.Items.Insert(0, new ListItem("Select", "0"));

                ddlToScale.DataTextField = "";
                ddlToScale.DataValueField = "";
                ddlToScale.DataSource = "";
                ddlToScale.DataBind();
                ddlToScale.Items.Insert(0, new ListItem("Select", "0"));

                ddlJobtype.DataTextField = "";
                ddlJobtype.DataValueField = "";
                ddlJobtype.DataSource = "";
                ddlJobtype.DataBind();
                ddlJobtype.Items.Insert(0, new ListItem("Select", "0"));
            }
            else if( option == 2)
            {
                ddlsection.DataTextField = "CodeDesc";
                ddlsection.DataValueField = "CodeID";
                ddlsection.DataSource = db.tblPlCodes.Where(x => x.Enabled == true && x.CodeTypeID == 3).ToList();
                ddlsection.DataBind();
                ddlsection.Items.Insert(0, new ListItem("Select", "0"));
                ddlsection.Items.Insert(1, new ListItem("All", "1"));

                ddlappoited.Controls.Clear();
                ddlappoited.Dispose();
                ddlappoited.DataTextField = "";
                ddlappoited.DataValueField = "";
                ddlappoited.DataSource = "";
                ddlappoited.DataBind();
                ddlappoited.Items.Insert(0, new ListItem("Select", "0"));

                ddlselectoption.DataTextField = "";
                ddlselectoption.DataValueField = "";
                ddlselectoption.DataSource = "";
                ddlselectoption.DataBind();
                ddlselectoption.Items.Insert(0, new ListItem("Select", "0"));

                ddlfromScale.DataTextField = "";
                ddlfromScale.DataValueField = "";
                ddlfromScale.DataSource = "";
                ddlfromScale.DataBind();
                ddlfromScale.Items.Insert(0, new ListItem("Select", "0"));

                ddlToScale.DataTextField = "";
                ddlToScale.DataValueField = "";
                ddlToScale.DataSource = "";
                ddlToScale.DataBind();
                ddlToScale.Items.Insert(0, new ListItem("Select", "0"));

                ddlJobtype.DataTextField = "";
                ddlJobtype.DataValueField = "";
                ddlJobtype.DataSource = "";
                ddlJobtype.DataBind();
                ddlJobtype.Items.Insert(0, new ListItem("Select", "0"));
            }
            else if (option == 3)
            {
                ddlfromScale.DataTextField = "ScaleName";
                ddlfromScale.DataValueField = "ScaleID";
                ddlfromScale.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.Orderby);
                ddlfromScale.DataBind();
                ddlfromScale.Items.Insert(0, new ListItem("Select", "0"));
                ddlfromScale.Items.Insert(1, new ListItem("All", "1"));

                ddlToScale.DataTextField = "ScaleName";
                ddlToScale.DataValueField = "ScaleID";
                ddlToScale.DataSource = db.TblEmpScales.ToList().OrderBy(x => x.Orderby);
                ddlToScale.DataBind();
                ddlToScale.Items.Insert(0, new ListItem("Select", "0"));
                ddlToScale.Items.Insert(1, new ListItem("All", "1"));

                ddlappoited.Controls.Clear();
                ddlappoited.Dispose();
                ddlappoited.DataTextField = "";
                ddlappoited.DataValueField = "";
                ddlappoited.DataSource = "";
                ddlappoited.DataBind();
                ddlappoited.Items.Insert(0, new ListItem("Select", "0"));

                ddlselectoption.DataTextField = "";
                ddlselectoption.DataValueField = "";
                ddlselectoption.DataSource = "";
                ddlselectoption.DataBind();
                ddlselectoption.Items.Insert(0, new ListItem("Select", "0"));

                ddlsection.DataTextField = "";
                ddlsection.DataValueField = "";
                ddlsection.DataSource = "";
                ddlsection.DataBind();
                ddlsection.Items.Insert(0, new ListItem("Select", "0"));

                ddlJobtype.DataTextField = "";
                ddlJobtype.DataValueField = "";
                ddlJobtype.DataSource = "";
                ddlJobtype.DataBind();
                ddlJobtype.Items.Insert(0, new ListItem("Select", "0"));

            }
            else if (option == 4)
            {
                ddlJobtype.DataTextField = "JobTypeName1";
                ddlJobtype.DataValueField = "JobNameID";
                ddlJobtype.DataSource = db.JobTypeNames.Where(x => x.IsActive == true).ToList();
                ddlJobtype.DataBind();
                ddlJobtype.Items.Insert(0, new ListItem("Select", "0"));
                ddlJobtype.Items.Insert(1, new ListItem("All", "9"));

                ddlappoited.Controls.Clear();
                ddlappoited.Dispose();
                ddlappoited.DataTextField = "";
                ddlappoited.DataValueField = "";
                ddlappoited.DataSource = "";
                ddlappoited.DataBind();
                ddlappoited.Items.Insert(0, new ListItem("Select", "0"));

                ddlselectoption.DataTextField = "";
                ddlselectoption.DataValueField = "";
                ddlselectoption.DataSource = "";
                ddlselectoption.DataBind();
                ddlselectoption.Items.Insert(0, new ListItem("Select", "0"));

                ddlsection.DataTextField = "";
                ddlsection.DataValueField = "";
                ddlsection.DataSource = "";
                ddlsection.DataBind();
                ddlsection.Items.Insert(0, new ListItem("Select", "0"));

                ddlfromScale.DataTextField = "";
                ddlfromScale.DataValueField = "";
                ddlfromScale.DataSource = "";
                ddlfromScale.DataBind();
                ddlfromScale.Items.Insert(0, new ListItem("Select", "0"));

                ddlToScale.DataTextField = "";
                ddlToScale.DataValueField = "";
                ddlToScale.DataSource = "";
                ddlToScale.DataBind();
                ddlToScale.Items.Insert(0, new ListItem("Select", "0"));

               

            }
            else if (option == 13)
            {
                ddlappoited.Controls.Clear();
                ddlappoited.Dispose();
                ddlappoited.DataTextField = "CodeDesc";
                ddlappoited.DataValueField = "CodeID";
                ddlappoited.DataSource = db.tblPlCodes.Where(x => x.Enabled == true && x.CodeTypeID == 4).OrderBy(x => x.sort).ToList().OrderBy(x => x.sort);
                ddlappoited.DataBind();
                ddlappoited.Items.Insert(0, new ListItem("Select", "0"));
                ddlappoited.Items.Insert(1, new ListItem("All", "1"));

                ddlselectoption.Controls.Clear();
                ddlselectoption.Dispose();
                ddlselectoption.DataTextField = "";
                ddlselectoption.DataValueField = "";
                ddlselectoption.DataSource = "";
                ddlselectoption.DataBind();
                ddlselectoption.Items.Insert(0, new ListItem("Select", "0"));


                ddlsection.DataTextField = "";
                ddlsection.DataValueField = "";
                ddlsection.DataSource = "";
                ddlsection.DataBind();
                ddlsection.Items.Insert(0, new ListItem("Select", "0"));

                ddlfromScale.DataTextField = "";
                ddlfromScale.DataValueField = "";
                ddlfromScale.DataSource = "";
                ddlfromScale.DataBind();
                ddlfromScale.Items.Insert(0, new ListItem("Select", "0"));

                ddlToScale.DataTextField = "";
                ddlToScale.DataValueField = "";
                ddlToScale.DataSource = "";
                ddlToScale.DataBind();
                ddlToScale.Items.Insert(0, new ListItem("Select", "0"));

                ddlJobtype.DataTextField = "";
                ddlJobtype.DataValueField = "";
                ddlJobtype.DataSource = "";
                ddlJobtype.DataBind();
                ddlJobtype.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        //public void ClearForm()
        //{
        //    ddlselectoption.SelectedValue = "0";
        //    ddlsection.SelectedValue = "0";
        //    ddlfromScale.SelectedValue = "0";
        //    ddlToScale.SelectedValue = "0";
        //    ddlJobtype.SelectedValue = "0";
        //    ddlDomicile.SelectedValue = "0";
        //    ddlFromAge.SelectedValue = "0";
        //    ddlToScale.SelectedValue = "0";
        //    ddlGender.SelectedValue = "0";
        //    ddlDisablity.SelectedValue = "0";
        //    ddlReligion.SelectedValue = "0";
        //    ddlAddionReport.SelectedValue = "0";
        //    ddlQuota.SelectedValue = "0";
        //    ddlPoliceVerifi.SelectedValue = "0";
        //    ddlappoited.SelectedValue = "0";
        //}
    }
}
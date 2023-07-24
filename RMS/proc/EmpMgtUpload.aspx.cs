using System;
using System.Web.UI.WebControls;
using RMS.BL;
using System.Text.RegularExpressions;
namespace RMS.Setup
{
    public partial class EmpMgtUpload: BasePage
    {

        #region DataMembers

        PlUploadBL uploadmgr = new PlUploadBL();
       
        #endregion

        #region Properties
        
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "DataUpload").ToString();
            }
        }

        protected void ButtonCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                Response.Redirect("~/proc/empmgtupload.aspx?PID=40");
            }
            else 
            {
                try
                {
                    string fileNme = GetFileAndUpload();

                    if (!fileNme.Equals(""))
                    {
                        fileNme = Server.MapPath("~/empix/" + fileNme);

                        if (rblSheetNames.SelectedValue.Equals("6"))
                        {
                            if (Session["CompID"] == null)
                            {
                                new EmpSplitterWithInsertion().FileSplitter(Request.Cookies["uzr"]["CompID"].ToString(), fileNme, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            else
                            {
                                new EmpSplitterWithInsertion().FileSplitter(Session["CompID"].ToString(), fileNme, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            ucMessage.ShowMessage("Employees inserted successfully", RMS.BL.Enums.MessageType.Info);
                        }
                        else if (rblSheetNames.SelectedValue.Equals("2"))
                        {//ATTENDANCE SHEET
                            string filename = "";
                            if (Session["CompID"] == null)
                            {
                                filename = uploadmgr.LeaveUpload(Request.Cookies["uzr"]["UserName"].ToString(), Convert.ToByte(Request.Cookies["uzr"]["CompID"]), fileNme, "ATT", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            else
                            {
                                filename = uploadmgr.LeaveUpload(Session["UserName"].ToString(), Convert.ToByte(Session["CompID"].ToString()), fileNme, "ATT", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }

                            
                            
                            if (filename.Equals("0"))
                            {
                                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "UploadSuccess").ToString(), RMS.BL.Enums.MessageType.Info);
                            }
                            else if (filename.Equals("1"))
                            {
                                ucMessage.ShowMessage("Errors in uploading data", RMS.BL.Enums.MessageType.Error);

                            }
                            else
                            {
                                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "FileAlreadyExixt").ToString() + "\t, File Name: \" " + filename + " \" ", RMS.BL.Enums.MessageType.Error);
                            }
                        }
                        else if (rblSheetNames.SelectedValue.Equals("7"))
                        {//COMPANY PARAMETERS
                            string filename = "";
                            if (Session["CompID"] == null)
                            {
                                filename = uploadmgr.CompanyUpload(Request.Cookies["uzr"]["UserName"].ToString(), Convert.ToByte(Request.Cookies["uzr"]["CompID"]), fileNme, "ATT", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            else
                            {
                                filename = uploadmgr.CompanyUpload(Session["UserName"].ToString(), Convert.ToByte(Session["CompID"].ToString()), fileNme, "ATT", (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }



                            if (filename.Equals("0"))
                            {
                                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "UploadSuccess").ToString(), RMS.BL.Enums.MessageType.Info);
                            }
                            else if (filename.Equals("1"))
                            {
                                ucMessage.ShowMessage("Errors in uploading data", RMS.BL.Enums.MessageType.Error);

                            }
                            else
                            {
                                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "FileAlreadyExixt").ToString() + "\t, File Name: \" " + filename + " \" ", RMS.BL.Enums.MessageType.Error);
                            }
                        }
                        else
                        {
                            string transType = "";
                            if (rblSheetNames.SelectedValue.Equals("1"))
                                transType = "EDA";
                            else if (rblSheetNames.SelectedValue.Equals("3")) //INCENTIVE
                                transType = "INC";
                            else if (rblSheetNames.SelectedValue.Equals("4")) //MOBILE BILL DEDUCTION
                                transType = "MBD";
                            else if (rblSheetNames.SelectedValue.Equals("5")) //EXPENSE
                                transType = "EXP";
                            else if (rblSheetNames.SelectedValue.Equals("7")) //MEDICAL
                                transType = "MED";
                            else if (rblSheetNames.SelectedValue.Equals("8")) //ADVANCE SHEET
                                transType = "ADV";
                            else if (rblSheetNames.SelectedValue.Equals("10")) //ARREAR SHEET
                                transType = "ARR";

                            string filename = "";
                            if (Session["CompID"] == null)
                            {
                                filename = uploadmgr.EDAUpload(Request.Cookies["uzr"]["UserName"].ToString(), Convert.ToByte(Request.Cookies["uzr"]["CompID"]), fileNme, transType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }
                            else
                            {
                                filename = uploadmgr.EDAUpload(Session["UserName"].ToString(), Convert.ToByte(Session["CompID"].ToString()), fileNme, transType, (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            }

                            //string filename = uploadmgr.EDAUpload(Session["UserName"].ToString(), Convert.ToByte(Session["CompID"].ToString()), fileNme,transType , (RMSDataContext)Session[Session["UserID"] + "rmsDBObj"]);
                            if (filename.Equals("0"))
                            {
                                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "UploadSuccess").ToString(), RMS.BL.Enums.MessageType.Info);
                            }
                            else if (filename.Equals("1"))
                            {
                                ucMessage.ShowMessage("Errors in uploading data", RMS.BL.Enums.MessageType.Error);

                            }
                            else
                            {
                                ucMessage.ShowMessage(GetGlobalResourceObject("MiscMsgs", "FileAlreadyExixt").ToString() + "\t, File Name: \" " + filename + " \" ", RMS.BL.Enums.MessageType.Error);
                            }

                        }

                        //Delete Uploaded File now
                        try
                        {
                            DeleteFile(fileNme);
                        }
                        catch { }
                    }
                }
                catch
                {
                    ucMessage.ShowMessage("Errors in uploading data", RMS.BL.Enums.MessageType.Error);
                }
            }
            
        }
        
        

        #endregion

        #region Helping Method
 
        private string GetFileAndUpload()
        {
            string file = "";

            if (fileUploadImg.HasFile)
            {

                string filepath = fileUploadImg.PostedFile.FileName;
                try
                {
                    string pat = @"\\(?:.+)\\(.+)\.(.+)";
                    Regex r = new Regex(pat);
                    //run
                    Match m = r.Match(filepath);
                    string file_ext = m.Groups[2].Captures[0].ToString();
                    string filename = m.Groups[1].Captures[0].ToString();
                    file = filename + "." + file_ext;

                }
                catch
                {
                    file = filepath;
                }
            }

            if (!file.Equals(""))
            {
                string txt = file.Substring(file.LastIndexOf("."), 4);
                if (txt.ToLower().Equals(".txt"))
                {
                    UploadFile(file);
                }
                else
                {
                    ucMessage.ShowMessage("File should be in Text (Tab delimited) (*.txt) format", RMS.BL.Enums.MessageType.Error);
                    return "";
                }
            }
            else
            {
                ucMessage.ShowMessage("File not found", RMS.BL.Enums.MessageType.Error);
                return "";
            }

            return file;
        }
        private void UploadFile(string file)
        {
            try
            {
                fileUploadImg.PostedFile.SaveAs(Server.MapPath("..\\empix\\") + file);
            }
            catch (Exception ex)
            {
                Session["errors"] = ex.Message;
                Response.Redirect("~/home/Error.aspx");
            }
        }
        private void DeleteFile(string fileName)
        {
            try
            {
                System.IO.File.Delete(fileName);
            }
            catch { }
        }
        #endregion

    }
}
 
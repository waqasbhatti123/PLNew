using RMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RMS.profile
{
    public partial class InterComminication : System.Web.UI.Page
    {

        RMSDataContext db = new RMSDataContext();
        tblIntercommu inter = new tblIntercommu();

        public int BranchID
        {
            get { return (ViewState["BranchID"] == null) ? 0 : Convert.ToInt32(ViewState["BranchID"]); }
            set { ViewState["BranchID"] = value; }
        }

#pragma warning disable CS0114 // 'InterComminication.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public int ID
#pragma warning restore CS0114 // 'InterComminication.ID' hides inherited member 'Page.ID'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            get { return (ViewState["ID"] == null) ? 0 : Convert.ToInt32(ViewState["ID"]); }
            set { ViewState["ID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageTitle"] = GetGlobalResourceObject("PageTitlesResource", "inter").ToString();
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
                    BranchID = Convert.ToInt32(Session["BranchID"]);
                }

                if (Session["DateFormat"] == null)
                {
                    txtDateCal.Format = Request.Cookies["uzr"]["DateFormat"];
                    ReplyDateCal.Format = Request.Cookies["uzr"]["DateFormat"];

                }
                else
                {
                    txtDateCal.Format = Session["DateFormat"].ToString();
                    ReplyDateCal.Format = Session["DateFormat"].ToString();
                }
                //Clear_fields();
                //ddlFromdivision.SelectedValue = "0";
                FillDivisionDropdown();
                //FillAllDivisionDropdown();
                FillFromDivisionDropdown();

                ddlFromdivision.SelectedValue = BranchID.ToString();
                BranchID = Convert.ToInt32(ddlFromdivision.SelectedValue);
                BindGrid();

                ReplyLable.Visible = false;
                ReplyDatee.Visible = false;
                ReplyBied.Visible = false;
                ReplyStatus.Visible = false;
                ReplyRemarks.Visible = false;
                ReplyAttach.Visible = false;
                ReplyImage.Visible = false;

            }
        }


        protected void btn_Save(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                try
                {
                    if (ddlFromdivision.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Division", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        inter.FromBranch = Convert.ToInt32(ddlFromdivision.SelectedValue);
                    }
                    if (ddlFromdivision.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Division", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        inter.BranchID = Convert.ToInt32(ddldivision.SelectedValue);
                    }
                    if (txtDate.Text == "")
                    {
                        ucMessage.ShowMessage("Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        inter.ComDate = Convert.ToDateTime(txtDate.Text.Trim());
                    }
                    if (ddlDocType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Doc. Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        inter.Doctype = ddlDocType.SelectedValue;
                    }
                    if (txtsubmitted.Text == "")
                    {
                        ucMessage.ShowMessage("Submitted by is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        inter.SubBy = txtsubmitted.Text.Trim();
                    }
                    if (txtRemarks.Text == "")
                    {
                        inter.Remakrs = null;
                    }
                    else
                    {
                        inter.Remakrs = txtRemarks.Text.Trim();
                    }
                    //if (ddlStatus.SelectedValue == "0")
                    //{
                    //    ucMessage.ShowMessage("Please Select Status", BL.Enums.MessageType.Error);
                    //    return;
                    //}
                    //else
                    //{
                    //    inter.InStatus = ddlStatus.SelectedValue;
                    //}
                    string fileName = "";
                    if (fileUploader.HasFile)
                    {
                        try
                        {
                            fileName = fileUploader.PostedFile.FileName;
                            string ext = System.IO.Path.GetExtension(fileName);
                            int filesize = fileUploader.PostedFile.ContentLength;

                            if (ext == ".jpg" || ext == ".png" || ext == ".gif")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucMessage.ShowMessage("File Size Should be less than 5MB", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileName.Equals(""))
                                {
                                    inter.Attachment = fileName;
                                }
                            }
                            else
                            {
                                ucMessage.ShowMessage("File extension Should be jpg,png and gif", BL.Enums.MessageType.Error);
                                return;
                            }
                            fileUploader.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\" + fileName));
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                    }

                    db.tblIntercommus.InsertOnSubmit(inter);
                    db.SubmitChanges();
                    ucMessage.ShowMessage("Save Successfully", BL.Enums.MessageType.Info);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                tblIntercommu com = db.tblIntercommus.Where(x => x.InterID == ID).FirstOrDefault();

                if (com.FromBranch == BranchID)
                {
                    if (ddlFromdivision.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Division", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        com.FromBranch = Convert.ToInt32(ddlFromdivision.SelectedValue);
                    }
                    if (ddlFromdivision.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Division", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        com.BranchID = Convert.ToInt32(ddldivision.SelectedValue);
                    }
                    if (txtDate.Text == "")
                    {
                        ucMessage.ShowMessage("Date is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        com.ComDate = Convert.ToDateTime(txtDate.Text.Trim());
                    }
                    if (ddlDocType.SelectedValue == "0")
                    {
                        ucMessage.ShowMessage("Please Select Doc. Type", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        com.Doctype = ddlDocType.SelectedValue;
                    }
                    if (txtsubmitted.Text == "")
                    {
                        ucMessage.ShowMessage("Submitted by is Required", BL.Enums.MessageType.Error);
                        return;
                    }
                    else
                    {
                        com.SubBy = txtsubmitted.Text.Trim();
                    }
                    if (txtRemarks.Text == "")
                    {
                        com.Remakrs = null;
                    }
                    else
                    {
                        com.Remakrs = txtRemarks.Text.Trim();
                    }

                    string fileName = "";
                    if (fileUploader.HasFile)
                    {
                        try
                        {
                            fileName = fileUploader.PostedFile.FileName;
                            string ext = System.IO.Path.GetExtension(fileName);
                            int filesize = fileUploader.PostedFile.ContentLength;

                            if (ext == ".jpg" || ext == ".png" || ext == ".gif")
                            {
                                if (filesize > 5 * 1024 * 1024)
                                {
                                    ucMessage.ShowMessage("File Size Should be less than 5MB", BL.Enums.MessageType.Error);
                                    return;
                                }
                                if (!fileName.Equals(""))
                                {
                                    com.Attachment = fileName;
                                }
                            }
                            else
                            {
                                ucMessage.ShowMessage("File extension Should be jpg,png and gif", BL.Enums.MessageType.Error);
                                return;
                            }
                            fileUploader.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\" + fileName));
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                    }


                    db.SubmitChanges();
                    ucMessage.ShowMessage("Updated Successfully", BL.Enums.MessageType.Info);
                }
                else
                {
                    try
                    {
                        if (ddlFromdivision.SelectedValue == "0")
                        {
                            ucMessage.ShowMessage("Please Select Division", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            com.FromBranch = Convert.ToInt32(ddlFromdivision.SelectedValue);
                        }
                        if (ddlFromdivision.SelectedValue == "0")
                        {
                            ucMessage.ShowMessage("Please Select Division", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            com.BranchID = Convert.ToInt32(ddldivision.SelectedValue);
                        }
                        if (txtDate.Text == "")
                        {
                            ucMessage.ShowMessage("Date is Required", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            com.ComDate = Convert.ToDateTime(txtDate.Text.Trim());
                        }
                        if (ddlDocType.SelectedValue == "0")
                        {
                            ucMessage.ShowMessage("Please Select Doc. Type", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            com.Doctype = ddlDocType.SelectedValue;
                        }
                        if (txtsubmitted.Text == "")
                        {
                            ucMessage.ShowMessage("Submitted by is Required", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            com.SubBy = txtsubmitted.Text.Trim();
                        }
                        if (txtRemarks.Text == "")
                        {
                            com.Remakrs = null;
                        }
                        else
                        {
                            com.Remakrs = txtRemarks.Text.Trim();
                        }
                        string fileName = "";
                        if (fileUploader.HasFile)
                        {
                            try
                            {
                                fileName = fileUploader.PostedFile.FileName;
                                string ext = System.IO.Path.GetExtension(fileName);
                                int filesize = fileUploader.PostedFile.ContentLength;

                                if (ext == ".jpg" || ext == ".png" || ext == ".gif")
                                {
                                    if (filesize > 5 * 1024 * 1024)
                                    {
                                        ucMessage.ShowMessage("File Size Should be less than 5MB", BL.Enums.MessageType.Error);
                                        return;
                                    }
                                    if (!fileName.Equals(""))
                                    {
                                        com.Attachment = fileName;
                                    }
                                }
                                else
                                {
                                    ucMessage.ShowMessage("File extension Should be jpg,png and gif", BL.Enums.MessageType.Error);
                                    return;
                                }
                                fileUploader.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\" + fileName));
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        else
                        {
                        }
                        //if (ddlStatus.SelectedValue == "0")
                        //{
                        //    ucMessage.ShowMessage("Please Select Status", BL.Enums.MessageType.Error);
                        //    return;
                        //}
                        //else
                        //{
                        //    com.InStatus = ddlStatus.SelectedValue;
                        //}
                        //string fileName = "";
                        //if (fileUploader.HasFile)
                        //{
                        //    try
                        //    {
                        //        fileName = fileUploader.PostedFile.FileName;
                        //        string ext = System.IO.Path.GetExtension(fileName);
                        //        int filesize = fileUploader.PostedFile.ContentLength;

                        //        if (ext == ".jpg" || ext == ".png" || ext == ".gif")
                        //        {
                        //            if (filesize > 5 * 1024 * 1024)
                        //            {
                        //                ucMessage.ShowMessage("File Size Should be less than 5MB", BL.Enums.MessageType.Error);
                        //                return;
                        //            }
                        //            if (!fileName.Equals(""))
                        //            {
                        //                com.Attachment = fileName;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            ucMessage.ShowMessage("File extension Must be jpg,png and gif", BL.Enums.MessageType.Error);
                        //            return;
                        //        }
                        //        fileUploader.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\" + fileName));
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        throw ex;
                        //    }
                        //}
                        //else
                        //{
                        //}


                        if (ReplyDate.Text == "")
                        {
                            ucMessage.ShowMessage("Reply Date is Required", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            com.RepDate = Convert.ToDateTime(ReplyDate.Text);
                        }
                        if (txtReplBy.Text == "")
                        {
                            ucMessage.ShowMessage("Reply By is Required", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            com.RepBy = txtReplBy.Text.Trim();
                        }
                        if (ddlRepStatus.SelectedValue == "0")
                        {
                            ucMessage.ShowMessage("Please Select Reply Status", BL.Enums.MessageType.Error);
                            return;
                        }
                        else
                        {
                            com.RepStatus = ddlRepStatus.SelectedValue;
                        }
                        if (txtReplRemarks.Text == "")
                        {
                            com.RepRemarks = null;
                        }
                        else
                        {
                            com.RepRemarks = txtReplRemarks.Text.Trim();
                        }

                        string replyFile = null;
                        if (RepliFile.HasFile)
                        {
                            replyFile = RepliFile.PostedFile.FileName;
                            int size = RepliFile.PostedFile.ContentLength;
                            string ex = System.IO.Path.GetExtension(replyFile);

                            if (ex.ToLower().Equals(".jpg") || ex.ToLower().Equals(".png") || ex.ToLower().Equals(".gif"))
                            {
                                if (size > 5 * 1024 * 1024)
                                {
                                    ucMessage.ShowMessage("File Size Should be less than 5MB", BL.Enums.MessageType.Error);
                                    return;
                                }
                                else
                                {
                                    if (!replyFile.Equals(""))
                                    {
                                        com.RepAttach = replyFile;
                                    }

                                }
                            }
                            else
                            {
                                ucMessage.ShowMessage("File Extension Must be jpg, png or gif", BL.Enums.MessageType.Error);
                                return;
                            }
                            RepliFile.PostedFile.SaveAs(Server.MapPath("..\\Attachments\\") + replyFile);
                        }

                        db.SubmitChanges();
                        ucMessage.ShowMessage("Update Successfully", BL.Enums.MessageType.Info);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                
            }
            BindGrid();
            Clear_fields();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
            // FillAlDivisionDropdown();
        }
        

        protected void btn_Clear(object sender, EventArgs e)
        {

        }

        protected void lnkEduPrint_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrint = (LinkButton)sender;
            string ImgID = lnkPrint.CommandArgument;
            string filename;
            var img = db.tblIntercommus.Where(x => x.Attachment == ImgID).FirstOrDefault();
            if (img == null)
            {
                filename = "../empix/noimage.jpg";
            }
            else
            {
                filename = img.Attachment;
            }

            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
           string.Format("window.open('Seepicture.aspx?ID={0}');", filename), true);
            //contenttype = img.EmpEduID.ToString();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.TransmitFile(Server.MapPath("~/Attachments/" + filename));
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            //Response.Flush();
            //Response.End();
        }

        protected void lnkEduApp_Click(object sender, EventArgs e)
        {
            LinkButton lnkPrintRep = (LinkButton)sender;
            string ImgID = lnkPrintRep.CommandArgument;
            string filename;
            var img = db.tblIntercommus.Where(x => x.RepAttach == ImgID).FirstOrDefault();
            if (img == null)
            {
                filename = "../empix/noimage.jpg";
            }
            else
            {
                filename = img.RepAttach;
            }

            ClientScript.RegisterStartupScript(this.GetType(), "Popup",
           string.Format("window.open('Seepicture.aspx?ID={0}');", filename), true);
            //contenttype = img.EmpEduID.ToString();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.TransmitFile(Server.MapPath("~/Attachments/" + filename));
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            //Response.Flush();
            //Response.End();
        }


        protected void grdInter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(grdIntercom.SelectedValue);
            tblIntercommu intercom = new tblIntercommu();
            intercom = db.tblIntercommus.Where(x => x.InterID == ID).FirstOrDefault();
            if (intercom.FromBranch == BranchID)
            {
                ddlFromdivision.SelectedValue = intercom.FromBranch.ToString();
                ddldivision.SelectedValue = intercom.BranchID.ToString();
                txtDate.Text = intercom.ComDate.ToString();
                ddlDocType.SelectedValue = intercom.Doctype.ToString();
                txtDate.Text = intercom.ComDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                txtsubmitted.Text = intercom.SubBy;
                if (intercom.Remakrs == null || intercom.Remakrs == "")
                {
                    txtRemarks.Text = "";
                }
                else
                {
                    txtRemarks.Text = intercom.Remakrs;
                }

                //ddlStatus.SelectedValue = intercom.InStatus;
                if (intercom.Attachment == "" || intercom.Attachment == null)
                {
                    imageurl.ImageUrl = "";
                }
                else
                {
                    imageurl.ImageUrl = "~/Attachments/" + intercom.Attachment.ToString();
                }

                if (string.IsNullOrEmpty(intercom.RepBy))
                {
                    txtReplBy.Text = "";
                }
                else
                {
                    txtReplBy.Text = intercom.RepBy.ToString();
                }
                if (intercom.RepDate == null)
                {
                    ReplyDate.Text = "";
                }
                else
                {
                    ReplyDate.Text = intercom.RepDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                if (string.IsNullOrEmpty(intercom.RepRemarks))
                {
                    txtReplRemarks.Text = "";
                }
                else
                {
                    txtReplRemarks.Text = intercom.RepRemarks.ToString();
                }
                if (string.IsNullOrEmpty(intercom.RepStatus))
                {
                    ddlRepStatus.Text = "Pending";
                }
                else
                {
                    ddlRepStatus.SelectedValue = intercom.RepStatus;
                }
                if (intercom.RepAttach == "" || intercom.RepAttach == null)
                {
                    repimageFile.ImageUrl = "";
                }
                else
                {
                    repimageFile.ImageUrl = "~/Attachments/" + intercom.RepAttach;
                }

                ddlFromdivision.Enabled = true;
                ddldivision.Enabled = true;
                txtDate.Enabled = true;
                ddlDocType.Enabled = true;
                txtDate.Enabled = true;
                txtsubmitted.Enabled = true;
                txtRemarks.Enabled = true;
                fileUploader.Enabled = true;
                ReplyLable.Visible = false;
                ReplyDatee.Visible = false;
                ReplyBied.Visible = false;
                ReplyStatus.Visible = false;
                ReplyRemarks.Visible = false;
                ReplyAttach.Visible = false;
                ReplyImage.Visible = false;
            }
            else
            {
                FillAllDivisionDropdown();
                ddlFromdivision.SelectedValue = intercom.FromBranch.ToString();
                ddldivision.SelectedValue = intercom.BranchID.ToString();
                txtDate.Text = intercom.ComDate.ToString();
                ddlDocType.SelectedValue = intercom.Doctype.ToString();
                txtDate.Text = intercom.ComDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                txtsubmitted.Text = intercom.SubBy;
                if (intercom.Remakrs == null || intercom.Remakrs == "")
                {
                    txtRemarks.Text = "";
                }
                else
                {
                    txtRemarks.Text = intercom.Remakrs;
                }

                //ddlStatus.SelectedValue = intercom.InStatus;
                if (intercom.Attachment == "" || intercom.Attachment == null)
                {
                    imageurl.ImageUrl = "";
                }
                else
                {
                    imageurl.ImageUrl = "~/Attachments/" + intercom.Attachment.ToString();
                }

                if (string.IsNullOrEmpty(intercom.RepBy))
                {
                    txtReplBy.Text = "";
                }
                else
                {
                    txtReplBy.Text = intercom.RepBy.ToString();
                }
                if (intercom.RepDate == null)
                {
                    ReplyDate.Text = "";
                }
                else
                {
                    ReplyDate.Text = intercom.RepDate.Value.ToString(Request.Cookies["uzr"]["DateFullYearFormat"]);
                }
                if (string.IsNullOrEmpty(intercom.RepRemarks))
                {
                    txtReplRemarks.Text = "";
                }
                else
                {
                    txtReplRemarks.Text = intercom.RepRemarks.ToString();
                }
                if (string.IsNullOrEmpty(intercom.RepStatus))
                {
                    ddlRepStatus.Text = "Pending";
                }
                else
                {
                    ddlRepStatus.SelectedValue = intercom.RepStatus;
                }
                if (intercom.RepAttach == "" || intercom.RepAttach == null)
                {
                    repimageFile.ImageUrl = "";
                }
                else
                {
                    repimageFile.ImageUrl = "~/Attachments/" + intercom.RepAttach;
                }

                ddlFromdivision.Enabled = false;
                ddldivision.Enabled = false;
                txtDate.Enabled = false;
                ddlDocType.Enabled = false;
                txtDate.Enabled = false;
                txtsubmitted.Enabled = false;
                txtRemarks.Enabled = false;
                fileUploader.Enabled = false;
                ReplyLable.Visible = true;
                ReplyDatee.Visible = true;
                ReplyBied.Visible = true;
                ReplyStatus.Visible = true;
                ReplyRemarks.Visible = true;
                ReplyAttach.Visible = true;
                ReplyImage.Visible = true;
            }
            
            
        }

        protected void grdInter_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdIntercom.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void grdInter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);

                //if (e.Row.Cells[6].Equals(""))
                //{
                //    e.Row.Cells[6].Text = "";
                //}
                //else
                //{
                //    e.Row.Cells[6].Text = Convert.ToDateTime(e.Row.Cells[6].Text.Trim()).ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]);
                //}

            }
        }

        private void FillDivisionDropdown()
        {
            this.ddldivision.DataTextField = "br_nme";
            this.ddldivision.DataValueField = "br_id";
            this.ddldivision.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            this.ddldivision.DataBind();
        }

        private void FillAllDivisionDropdown()
        {
            this.ddlFromdivision.DataTextField = "br_nme";
            this.ddlFromdivision.DataValueField = "br_id";
            this.ddlFromdivision.DataSource = db.Branches.Where(x => x.br_status == true).ToList();
            this.ddlFromdivision.DataBind();
        }


        private void FillFromDivisionDropdown()
        {
            ddlFromdivision.Items.Insert(0, new ListItem("Select Division", "0"));
            this.ddlFromdivision.DataTextField = "br_nme";
            this.ddlFromdivision.DataValueField = "br_id";
            this.ddlFromdivision.DataSource = db.Branches.Where(x => x.br_status == true && x.br_id == BranchID).ToList();
            this.ddlFromdivision.DataBind();

            //Branch br = db.Branches.Where(x => x.br_status == true && x.br_id == BranchID).FirstOrDefault();
            //this.ddlFromdivision.DataTextField = "br_nme";
            //this.ddlFromdivision.DataValueField = "br_id";
            
            //    List<Branch> BranchList = new List<Branch>();
            //    if (br != null)
            //    {
            //        BranchList = db.Branches.Where(x => x.br_status == true && x.br_id == BranchID).ToList();
            //        BranchList.Insert(0, br);
            //    }

            //    this.ddlFromdivision.DataSource = BranchList.ToList();
            //ddlFromdivision.Items.Clear();
            //     this.ddlFromdivision.DataBind();
        }

        private void BindGrid()
        {
            this.grdIntercom.DataSource = from inter in db.tblIntercommus
                                          join br in db.Branches 
                                          on inter.BranchID equals br.br_id
                                          join from_br in db.Branches
                                          on inter.FromBranch equals from_br.br_id
                                          where inter.FromBranch == BranchID || inter.BranchID == BranchID
                                          select new 
                                          {
                                              inter.InterID,
                                              br.br_nme,
                                              inter.InStatus,
                                              inter.Attachment,
                                              inter.Doctype,
                                              from_br_nme = from_br.br_nme ,
                                              inter.SubBy,
                                              inter.BranchID,
                                              inter.ComDate,
                                              inter.RepBy,
                                              inter.RepDate,
                                              inter.RepRemarks,
                                              inter.RepStatus,
                                              inter.RepAttach
                                          };
            this.grdIntercom.DataBind();
        }

        protected void Clear_fields()
        {
            //FillFromDivisionDropdown();
            ID = 0;
            ddlFromdivision.SelectedValue = "0";
            ddldivision.SelectedValue = "0";
            txtsubmitted.Text = "";
            txtDate.Text = "";
            ddlDocType.SelectedValue = "0";
            //ddlStatus.SelectedValue = "0";
            txtRemarks.Text = "";
            repimageFile.ImageUrl = "";
            imageurl.ImageUrl = "";
            ReplyDate.Text = "";
            txtReplBy.Text = "";
            ddlRepStatus.SelectedValue = "0";
            txtReplRemarks.Text = "";
            ddlFromdivision.Enabled = true;
            ddldivision.Enabled = true;
            txtDate.Enabled = true;
            ddlDocType.Enabled = true;
            txtDate.Enabled = true;
            txtsubmitted.Enabled = true;
            txtRemarks.Enabled = true;
            ReplyLable.Visible = false;
            ReplyDatee.Visible = false;
            ReplyBied.Visible = false;
            ReplyStatus.Visible = false;
            ReplyRemarks.Visible = false;
            ReplyAttach.Visible = false;
            ReplyImage.Visible = false;
        }

    }
}
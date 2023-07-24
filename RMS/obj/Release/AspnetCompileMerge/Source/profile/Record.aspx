<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="Record.aspx.cs" Inherits="RMS.Profile.Record" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script src="../Scripts/jquery-ui-1.8.16.js"></script>
    <link href="../Scripts/jquery-ui.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" rel="stylesheet" />
    <script>

        $(function () {

            $(".fileupload").change(function () {
                if (this.files && this.files[0]) {
                    var filereader = new FileReader();
                    filereader.readAsDataURL(this.files[0])
                    filereader.onload = function (a) {
                        $(".imageshow").attr('src', a.target.result);
                    }
                }
            });
            $(".Tenfileupload").change(function () {
                if (this.files && this.files[0]) {
                    var filereader2 = new FileReader();
                    filereader2.readAsDataURL(this.files[0])
                    filereader2.onload = function (c) {
                        $(".imageTenureshow").attr('src', c.target.result);
                    }
                }
            });
            $(".acrfileupload").change(function () {
                if (this.files && this.files[0]) {
                    var filereader3 = new FileReader();
                    filereader3.readAsDataURL(this.files[0])
                    filereader3.onload = function (d) {
                        $(".imageAcrshow").attr('src', d.target.result);
                    }
                }
            });
            $(".Expfileupload").change(function () {
                if (this.files && this.files[0]) {
                    var reader = new FileReader();
                    reader.readAsDataURL(this.files[0]);
                    reader.onload = function (e) {
                        $('.imagExpeshow').attr('src', e.target.result);
                    };
                }
            });
            $(".enqfileupload").change(function () {
                if (this.files && this.files[0]) {
                    var reader = new FileReader();
                    reader.readAsDataURL(this.files[0]);
                    reader.onload = function (e) {
                        $('.imageenqshow').attr('src', e.target.result);
                    };
                }
            });

            $(".litifile").change(function () {
                if (this.files && this.files[0]) {
                    var reader = new FileReader();
                    reader.readAsDataURL(this.files[0]);
                    reader.onload = function (e) {
                        $('.litiImage').attr('src', e.target.result);
                    };
                }

            });

            $(".Profileupload").change(function () {
                if (this.files && this.files[0]) {
                    var reader = new FileReader();
                    reader.readAsDataURL(this.files[0]);
                    reader.onload = function (e) {
                        $('.Proshow').attr('src', e.target.result);
                    };
                }

            });

            $(".additi").change(function () {
                var vall = $(this).val();
                if (vall == "Other") {
                    $(".addti").show();
                }
                else {
                    $(".addti").hide();
                }
            });

            $(".place").change(function () {
                var valll = $(this).val();
                if (valll == 17) {
                    $(".addplacee").show();
                }
                else {
                    $(".addplacee").hide();
                }
            });

            var addpost = $(".additi").val();
            if (addpost == "Other") {
                $(".addti").show();
            }
            else {
                $(".addti").hide();
            }

            var addplace = $(".place").val();
            if (addplace == 17) {
                $(".addplacee").show();
            }
            else {
                $(".addplacee").hide();
            }

            <%--$('#<%= txtEmpSearch.ClientID %>').autocomplete({
                source: function (request, response) {
                    debugger
                    var param = { employee: $(".txtsearch").val() };
                    $.ajax({
                        url: "Record.aspx/GetEmployee",
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            debugger
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.FullName,
                                    result: item.EmpID,
                                    id: item.EmpID
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                select: function (e, ui) {
                    //glCd = ui.item.id;
                    // getFirstRowCode();
                    if (ui.item.result != '') {
                        $('#<%= ddlEmployeeSearch.ClientID %>').val(ui.item.result);
                }
                else {
                    $('#<%= ddlEmployeeSearch.ClientID %>').val('0');
                    }

                },
                minLength: 1
            });--%>
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">
        <div class="col-lg-4 col-md-4 col-sm-4">
            <label runat="server" id="txterror" style="font-size: 20px;"></label>
        </div>
    </div>


    <div class="row">
        <%--<div class="col-lg-4 col-md-4 col-sm-4">
            <label>Divisions </label>
            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                <asp:ListItem Value="0">Select Division</asp:ListItem>
            </asp:DropDownList>
        </div>--%>
        <%--<div class="col-lg-4 col-md-4 col-sm-4">
            <label>Employee Search</label>
            <asp:TextBox runat="server" ID="txtEmpSearch" CssClass="form-control txtsearch"></asp:TextBox>
            
            <div id="EmployeeList">
            </div>
        </div>--%>
        <div class="col-lg-4 col-md-4 col-sm-4">
            <label>Employees*</label>
            <asp:DropDownList ID="ddlEmployeeSearch" CssClass="form-control" runat="server" AppendDataBoundItems="False" OnSelectedIndexChanged="searchemp_changeIndex" AutoPostBack="true">
                <asp:ListItem Value="0">Select Employee</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-lg-4 col-md-4 col-sm-4">
            <label>Personal File No*</label>
            <asp:DropDownList ID="ddlperson" runat="server" CssClass="form-control ddlEmpDrpdwnrt" OnSelectedIndexChanged="ddlPersonal_change" AutoPostBack="true">
                <asp:ListItem Value="0">Select Personal File Number</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top: 30px;">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator50" runat="server" ErrorMessage="Employee is required"
                ControlToValidate="ddlEmployeeSearch" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
            <asp:Button ID="ddlEmployeeSear" CssClass="btn btn-primary" runat="server" OnClick="BtnEmployeeEducation_Click" Text="Search" />

        </div>
    </div>



    <ajaxToolkit:Accordion ID="a1" runat="server" SelectedIndex="0" FadeTransitions="true" TransitionDuration="1000" FramesPerSecond="200" Width="100%">
        <Panes>

            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                </Header>
                <Header>
                    <br />
                    <div class="p-2 bg-info font-weight-bold">
                        <h2 style="color: white !important;">Qualification </h2>
                    </div>
                </Header>
                <Content>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="card card-shadow">
                                <div class="form-group" style="padding-left: 10px; padding-right: 10px;">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                                        ValidationGroup="main" />
                                                    <uc1:Messages ID="ucMessage" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Degree Type*</label>
                                                    <asp:DropDownList ID="ddlDegreeType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0" Selected="True">Select Degree</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:DropDownList ID="ddlDegreeType" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0">Select Degree</asp:ListItem>
                                                        <asp:ListItem Value="Middle">Middle</asp:ListItem>
                                                        <asp:ListItem Value="Matric">Matric</asp:ListItem>
                                                        <asp:ListItem Value="Intermediate">Intermediate</asp:ListItem>
                                                        <asp:ListItem Value="Becholar(14 Years)">Becholar(14 Years)</asp:ListItem>
                                                        <asp:ListItem Value="Becholar(16 Years)">Becholar(16 Years)</asp:ListItem>
                                                        <asp:ListItem Value="Master(16 Years)">Master(16 Years)</asp:ListItem>
                                                        <asp:ListItem Value="MS/M.Phil">MS/M.Phil</asp:ListItem>
                                                        <asp:ListItem Value="PHD">PHD</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="City is required"
                                                        ControlToValidate="ddlCity" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Degree Title*</label>
                                                    <asp:TextBox ID="txtEduDegTtl" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Select*</label>
                                                    <asp:DropDownList ID="ddluniversity" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0">Board / University</asp:ListItem>
                                                        <asp:ListItem Value="Lahore Board">Lahore Board</asp:ListItem>
                                                        <asp:ListItem Value="Gujranwala Board">Gujranwala Board</asp:ListItem>
                                                        <asp:ListItem Value="Bahawalpur Board">Bahawalpur Board</asp:ListItem>
                                                        <asp:ListItem Value="D.G Khan Board">D.G Khan Board</asp:ListItem>
                                                        <asp:ListItem Value="Faisalabad Board">Faisalabad Board</asp:ListItem>
                                                        <asp:ListItem Value="Multan Board">Multan Board</asp:ListItem>
                                                        <asp:ListItem Value="Rawalpindi Board<">Rawalpindi Board</asp:ListItem>
                                                        <asp:ListItem Value="Sargodha Board">Sargodha Board</asp:ListItem>
                                                        <asp:ListItem Value="Mirpur Baord">Mirpur Baord</asp:ListItem>
                                                        <asp:ListItem Value="Federal Baord">Federal Baord</asp:ListItem>
                                                        <asp:ListItem Value="Haily Collage">Haily Collage</asp:ListItem>
                                                        <asp:ListItem Value="Punjab Board of Technical Education">Punjab Board of Technical Education</asp:ListItem>
                                                        <asp:ListItem Value="University of the Punjab">University of the Punjab</asp:ListItem>
                                                        <asp:ListItem Value="University of Central Punjab">University of Central Punjab</asp:ListItem>
                                                        <asp:ListItem Value="University of Engineering and Technology">University of Engineering and Technology</asp:ListItem>
                                                        <asp:ListItem Value="Govt Collage University">Govt Collage University</asp:ListItem>
                                                        <asp:ListItem Value="Lahore University of Management Science">Lahore University of Management Science</asp:ListItem>
                                                        <asp:ListItem Value="University of Management & Technology">University of Management & Technology</asp:ListItem>
                                                        <asp:ListItem Value="University of Education">University of Education</asp:ListItem>
                                                        <asp:ListItem Value="Virtual University">Virtual University</asp:ListItem>
                                                        <asp:ListItem Value="Allama Iqbal Open University">Allama Iqbal Open University</asp:ListItem>
                                                        <asp:ListItem Value="Lahore Garrison University">Lahore Garrison University</asp:ListItem>
                                                        <asp:ListItem Value="Bahauddin Zakariya University">Bahauddin Zakariya University</asp:ListItem>
                                                        <asp:ListItem Value="University of Sargodha">University of Sargodha</asp:ListItem>
                                                        <asp:ListItem Value="Ali Institute of Education">Ali Institute of Education</asp:ListItem>
                                                        <asp:ListItem Value="Beaconhouse National University">Beaconhouse National University</asp:ListItem>
                                                        <asp:ListItem Value="Cholistan University of Veterinary and Animal Sciences, Bahawalpur">Cholistan University of Veterinary and Animal Sciences, Bahawalpur</asp:ListItem>
                                                        <asp:ListItem Value="Faisalabad Medical University, Faisalabad">Faisalabad Medical University, Faisalabad</asp:ListItem>
                                                        <asp:ListItem Value="Fatima Jinnah Medical University, Lahore">Fatima Jinnah Medical University, Lahore</asp:ListItem>
                                                        <asp:ListItem Value="Fatima Jinnah Women University">Fatima Jinnah Women University</asp:ListItem>
                                                        <asp:ListItem Value="Forman Christian College">Forman Christian College</asp:ListItem>
                                                        <asp:ListItem Value="GIFT University">GIFT University</asp:ListItem>
                                                        <asp:ListItem Value="Ghazi University">Ghazi University</asp:ListItem>
                                                        <asp:ListItem Value="Global Institute, Lahore">Global Institute, Lahore</asp:ListItem>
                                                        <asp:ListItem Value="Government College University">Government College University</asp:ListItem>
                                                        <asp:ListItem Value="Government College University, Lahore">Government College University, Lahore</asp:ListItem>
                                                        <asp:ListItem Value="Government College for Women University">Government College for Women University</asp:ListItem>
                                                        <asp:ListItem Value="Government College for Women University, Sialkot">Government College for Women University, Sialkot</asp:ListItem>
                                                        <asp:ListItem Value="Government Sadiq College Women University">Government Sadiq College Women University</asp:ListItem>
                                                        <asp:ListItem Value="HITEC University">HITEC University</asp:ListItem>
                                                        <asp:ListItem Value="Hajvery University, Lahore">Hajvery University, Lahore</asp:ListItem>
                                                        <asp:ListItem Value="Imperial College of Business Studies">Imperial College of Business Studies</asp:ListItem>
                                                        <asp:ListItem Value="Information Technology University of the Punjab">Information Technology University of the Punjab</asp:ListItem>
                                                        <asp:ListItem Value="Institute for Art and Culture">Institute for Art and Culture</asp:ListItem>
                                                        <asp:ListItem Value="Institute of Management Sciences">Institute of Management Sciences</asp:ListItem>
                                                        <asp:ListItem Value="Institute of Southern Punjab">Institute of Southern Punjab</asp:ListItem>
                                                        <asp:ListItem Value="Islamia University">Islamia University</asp:ListItem>
                                                        <asp:ListItem Value="Khawaja Freed University of Engineering & Information Technology">Khawaja Freed University of Engineering & Information Technology</asp:ListItem>
                                                        <asp:ListItem Value="King Edward Medical University">King Edward Medical University</asp:ListItem>
                                                        <asp:ListItem Value="Kinnaird College for Women">Kinnaird College for Women</asp:ListItem>
                                                        <asp:ListItem Value="Kohsar University, Murree">Kohsar University, Murree</asp:ListItem>
                                                        <asp:ListItem Value="Lahore College for Women University">Lahore College for Women University</asp:ListItem>
                                                        <asp:ListItem Value="University of Gujrat">University of Gujrat</asp:ListItem>
                                                        <asp:ListItem Value="Lahore Garrison University">Lahore Garrison University</asp:ListItem>
                                                        <asp:ListItem Value="Lahore Leads University">Lahore Leads University</asp:ListItem>
                                                        <asp:ListItem Value="Lahore School of Economics">Lahore School of Economics</asp:ListItem>
                                                        <asp:ListItem Value="Lahore University of Management Sciences">Lahore University of Management Sciences</asp:ListItem>
                                                        <asp:ListItem Value="Minhaj University">Minhaj University</asp:ListItem>
                                                        <asp:ListItem Value="Muhammad Nawaz Shareef University of Agriculture">Muhammad Nawaz Shareef University of Agriculture</asp:ListItem>
                                                        <asp:ListItem Value="Muhammad Nawaz Sharif University of  Engineering & Technology">Muhammad Nawaz Sharif University of  Engineering & Technology</asp:ListItem>
                                                        <asp:ListItem Value="NFC Institute of Engineering & Technology">NFC Institute of Engineering & Technology</asp:ListItem>
                                                        <asp:ListItem Value="Namal Institute, Mainwali">Namal Institute, Mainwali</asp:ListItem>
                                                        <asp:ListItem Value="National College of Arts">National College of Arts</asp:ListItem>
                                                        <asp:ListItem Value="National College of Business Administration & Economics">National College of Business Administration & Economics</asp:ListItem>
                                                        <asp:ListItem Value="National Textile University">National Textile University</asp:ListItem>
                                                        <asp:ListItem Value="National University of Medical Sciences">National University of Medical Sciences</asp:ListItem>
                                                        <asp:ListItem Value="Nishtar Medical University Multan">Nishtar Medical University Multan</asp:ListItem>
                                                        <asp:ListItem Value="Nur International University">Nur International University</asp:ListItem>
                                                        <asp:ListItem Value="Pakistan Institute of Fashion & Design">Pakistan Institute of Fashion & Design</asp:ListItem>
                                                        <asp:ListItem Value="Pir Mehr Ali Shah Arid Agriculture University">Pir Mehr Ali Shah Arid Agriculture University</asp:ListItem>
                                                        <asp:ListItem Value="Punjab Tianjin University of Technology, Lahore">Punjab Tianjin University of Technology, Lahore</asp:ListItem>
                                                        <asp:ListItem Value="Qarshi University">Qarshi University</asp:ListItem>
                                                        <asp:ListItem Value="Rawalpindi Medical University">Rawalpindi Medical University</asp:ListItem>
                                                        <asp:ListItem Value="Rawalpindi Women University, Rawalpindi">Rawalpindi Women University, Rawalpindi</asp:ListItem>
                                                        <asp:ListItem Value="The Green International University, Lahore">The Green International University, Lahore</asp:ListItem>
                                                        <asp:ListItem Value="The Superior College">The Superior College</asp:ListItem>
                                                        <asp:ListItem Value="The University of Faisalabad">The University of Faisalabad</asp:ListItem>
                                                        <asp:ListItem Value="The Women University">The Women University</asp:ListItem>
                                                        <asp:ListItem Value="Times Institute, Multan">Times Institute, Multan</asp:ListItem>
                                                        <asp:ListItem Value="University of Agriculture">University of Agriculture</asp:ListItem>
                                                        <asp:ListItem Value="University of Chakwal, Chakwal">University of Chakwal, Chakwal</asp:ListItem>
                                                        <asp:ListItem Value="University of Engineering & Technology, Taxila">University of Engineering & Technology, Taxila</asp:ListItem>
                                                        <asp:ListItem Value="University of Health Sciences">University of Health Sciences</asp:ListItem>
                                                        <asp:ListItem Value="University of Home Economics, Lahore">University of Home Economics, Lahore</asp:ListItem>
                                                        <asp:ListItem Value="University of Jhang">University of Jhang</asp:ListItem>
                                                        <asp:ListItem Value="University of Lahore">University of Lahore</asp:ListItem>
                                                        <asp:ListItem Value="University of Mianwali">University of Mianwali</asp:ListItem>
                                                        <asp:ListItem Value="University of Narowal">University of Narowal</asp:ListItem>
                                                        <asp:ListItem Value="University of Okara">University of Okara</asp:ListItem>
                                                        <asp:ListItem Value="University of Sahiwal">University of Sahiwal</asp:ListItem>
                                                        <asp:ListItem Value="University of Sargodha">University of Sargodha</asp:ListItem>
                                                        <asp:ListItem Value="University of Sialkot, Sialkot">University of Sialkot, Sialkot</asp:ListItem>
                                                        <asp:ListItem Value="University of South Asia">University of South Asia</asp:ListItem>
                                                        <asp:ListItem Value="University of Veterinary & Animal Sciences">University of Veterinary & Animal Sciences</asp:ListItem>
                                                        <asp:ListItem Value="University of Wah">University of Wah</asp:ListItem>
                                                        <asp:ListItem Value="Pakistan Institute of Developement and Economics">Pakistan Institute of Developement and Economics</asp:ListItem>
                                                        <asp:ListItem Value="International Islamic University">International Islamic University</asp:ListItem>
                                                        <asp:ListItem Value="FAST Nuces">FAST Nuces</asp:ListItem>
                                                        <asp:ListItem Value="University of Cambridge">University of Cambridge</asp:ListItem>
                                                        <asp:ListItem Value="Quaid e Azam University">Quaid e Azam University</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="City is required"
                                                        ControlToValidate="ddlCity" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Select City*</label>

                                                    <asp:DropDownList ID="ddlCity" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0">Select City</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator51" runat="server" ErrorMessage="City is required"
                                                        ControlToValidate="ddlCity" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Marks Percentage(%)*</label>
                                                    <asp:TextBox runat="server" ID="txtPercentage" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Passing Year*</label>
                                                    <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0">Select Year</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator52" runat="server" ErrorMessage="Year is required"
                                                        ControlToValidate="ddlYear" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Verified</label>
                                                    <asp:DropDownList ID="ddlEduVerified" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="True">Yes</asp:ListItem>
                                                        <asp:ListItem Value="False">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>

                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                            </div>
                                            <div class="row">

                                                <div class="col-lg-3 col-md-3  col-sm-3" style="margin-top: 35px;">
                                                    <asp:FileUpload ID="FileUploader" CssClass="fileupload" runat="server" />
                                                </div>
                                                <div class="col-lg-3 col-md-3  col-sm-3" style="margin-top: 20px;">
                                                    <asp:Image ID="ImageEdu" ImageUrl="" Width="70" Height="70" CssClass="imageshow" runat="server" />
                                                </div>
                                            </div>
                                            &nbsp;
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <br />
                                                    <%--<%--<asp:Button runat="server" ID="btnSaveEdu" OnCommand="ButtonCommand" class="btn btn-primary"  Text="Save" ValidationGroup=""></asp:Button>
                                                    <asp:Button runat="server" ID="btnCancel" class="btn btn-danger" Text="Clear"></asp:Button>
                                                    <uc2:Buttons ID="EduButton" OnButtonClick="ButtonCommand" runat="server" />--%>
                                                    <asp:Button runat="server" ID="Button3" class="btn btn-primary" Text="Save"
                                                        OnClick="btnEmpEdu_Click"
                                                        ValidationGroup="one"></asp:Button>
                                                    <asp:Button ID="Button1" OnClick="btnEmpEduClear_Clear" runat="server" CssClass="btn btn-danger" ValidationGroup="one" Text="Clear" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <asp:GridView ID="grdEducation" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpEduID,EmpID" OnSelectedIndexChanged="grdEduEmps_SelectedIndexChanged"
                                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEduEmps_PageIndexChanging" OnRowDataBound="grdEdu_rowbound"
                                                EmptyDataText="There is no employee defined" Width="100%">
                                                <Columns>
                                                    <asp:BoundField DataField="Degreetype" HeaderText="Degree Type" />
                                                    <asp:BoundField DataField="DegreeTitle" HeaderText="Degree Title" />
                                                    <asp:BoundField DataField="Percente" HeaderText="Percentage" />
                                                    <asp:BoundField DataField="YearName" HeaderText="Passing Year" />
                                                    <asp:BoundField DataField="UniversityBoard" HeaderText="University/Board" />
                                                    <asp:BoundField DataField="CityName" HeaderText="City Name" />
                                                    <asp:BoundField DataField="Verified" HeaderText="Verified" />
                                                    <asp:TemplateField HeaderText="Image">
                                                        <ItemTemplate>
                                                            <asp:Image ID="img" Width="40px" Height="40px" runat="server" ImageUrl='<%#Eval("filepath", "~/Attachments/{0}") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkPrint" runat="server" Text="See Picture" ToolTip="Print Employee Education Record" CommandArgument='<%#Eval("filepath")%>' OnClick="lnkEduPrint_Click" CssClass="lnk">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                        <ControlStyle CssClass="lnk"></ControlStyle>
                                                    </asp:CommandField>
                                                </Columns>
                                                <HeaderStyle CssClass="grid_hdr" />
                                                <RowStyle CssClass="grid_row" />
                                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                <SelectedRowStyle CssClass="gridSelectedRow" />
                                            </asp:GridView>
                                        </div>
                                        <div class="row">
                                            <div class="modal" id="MyModal">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <p>This is Modal Pop Up</p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Content>
            </ajaxToolkit:AccordionPane>

            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                    <br />
                    <div class="p-2 bg-info font-weight-bold">
                        <h2 style="color: white !important;">Prior Experience </h2>
                    </div>
                </Header>
                <Content>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="card card-shadow">
                                <div class="form-group" style="padding-left: 10px; padding-right: 10px;">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                                        ValidationGroup="main" />
                                                    <uc1:Messages ID="ucExpMessage" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">

                                                <%--<div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Appointed As*</label>
                                                    <asp:TextBox ID="txtExpAppAs" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>--%>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Post Held*</label>
                                                    <asp:TextBox ID="txtpriorExp" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <%--<asp:DropDownList ID="ddlPriorPost" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Post</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Scale*</label>
                                                    <asp:DropDownList ID="ddlScale" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:DropDownList ID="ddlExpScale" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="true">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Additional Charge</label>
                                                    <asp:TextBox ID="txtaddtionalChar" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <%--<asp:DropDownList ID="ddlPrioraddtional" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Organization Name:*</label>
                                                    <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <%--<div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Designation*</label>
                                            <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Select Designation</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator54" runat="server" ControlToValidate="ddlDesignation"
                                                ErrorMessage="Please select designation" SetFocusOnError="true" ValidationGroup="main"
                                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </div>--%>
                                                <%--<div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Department</label>
                                                    <asp:DropDownList ID="ddlExpDept" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Department</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlExpDept"
                                                        ErrorMessage="Please select department" SetFocusOnError="true" ValidationGroup="main"
                                                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Designation</label>
                                                    <asp:DropDownList ID="ddlExpDesig" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Designation</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlExpDesig"
                                                        ErrorMessage="Please select designation" SetFocusOnError="true" ValidationGroup="main"
                                                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </div>--%>
                                            </div>

                                            <div class="row">

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Sector*</label>
                                                    <asp:DropDownList ID="ddlExpSector" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Sector</asp:ListItem>
                                                        <asp:ListItem Value="Government">Government Sector</asp:ListItem>
                                                        <asp:ListItem Value="Government">Semi Government Sector</asp:ListItem>
                                                        <asp:ListItem Value="Private">Private Sector</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>From Date*</label>
                                                    <asp:TextBox ID="txtExpDOJ" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtExpDOJCal" runat="server" TargetControlID="txtExpDOJ" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>To Date*</label>
                                                    <asp:TextBox ID="txtLeaving" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtExpLeaving" runat="server" TargetControlID="txtLeaving" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Experience(Year)*</label>
                                                    <asp:DropDownList ID="ddlDomicile" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Value="0">Select Year</asp:ListItem>
                                                        <asp:ListItem Value="1">1</asp:ListItem>
                                                        <asp:ListItem Value="2">2</asp:ListItem>
                                                        <asp:ListItem Value="3">3</asp:ListItem>
                                                        <asp:ListItem Value="4">4</asp:ListItem>
                                                        <asp:ListItem Value="5">5</asp:ListItem>
                                                        <asp:ListItem Value="6">6</asp:ListItem>
                                                        <asp:ListItem Value="7">7</asp:ListItem>
                                                        <asp:ListItem Value="8">8</asp:ListItem>
                                                        <asp:ListItem Value="9">9</asp:ListItem>
                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtyearExp" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                                    <%--<asp:DropDownList ID="ddlExpYear" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="true">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>
                                            </div>
                                            <div class="row">

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Exprerience(Month)</label>
                                                    <asp:DropDownList ID="ddlMonthExp" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Value="0">Select Month</asp:ListItem>
                                                        <asp:ListItem Value="1">1</asp:ListItem>
                                                        <asp:ListItem Value="2">2</asp:ListItem>
                                                        <asp:ListItem Value="3">3</asp:ListItem>
                                                        <asp:ListItem Value="4">4</asp:ListItem>
                                                        <asp:ListItem Value="5">5</asp:ListItem>
                                                        <asp:ListItem Value="6">6</asp:ListItem>
                                                        <asp:ListItem Value="7">7</asp:ListItem>
                                                        <asp:ListItem Value="8">8</asp:ListItem>
                                                        <asp:ListItem Value="9">9</asp:ListItem>
                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtyearExp" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                                    <%--<asp:DropDownList ID="ddlExpYear" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="true">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>
                                                <div class="col-lg-3 col-md-3  col-sm-3" style="margin-top: 35px;">
                                                    <asp:FileUpload ID="ExpFileUpload" CssClass="Expfileupload" runat="server" />
                                                </div>
                                                <div class="col-lg-3 col-md-3  col-sm-3" style="margin-top: 20px; margin-left: 45px;">
                                                    <asp:Image ID="ImageExp" ImageUrl="" Width="70" Height="70" CssClass="imagExpeshow" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <br />
                                                    <asp:Button runat="server" ID="Button5" OnClick="btnEmpExp_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                                                    <asp:Button runat="server" ID="Button8" OnClick="EmpExpClear_Clear" class="btn btn-danger" Text="Clear"></asp:Button>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>

                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:GridView ID="grdExperience" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpExpID,EmpID" OnSelectedIndexChanged="grdExpEmps_SelectedIndexChanged"
                                                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdExpEmps_PageIndexChanging" OnRowDataBound="grdexp_rowbound"
                                                        EmptyDataText="No Experience Record" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="Postedas" HeaderText="Post Held" />
                                                            <asp:BoundField DataField="Sector" HeaderText="Sector" />
                                                            <asp:BoundField DataField="OrgName" HeaderText="Organization Name" />
                                                            <asp:BoundField DataField="joinDate" HeaderText="From Date" />
                                                            <asp:BoundField DataField="leavDate" HeaderText="To Date" />
                                                            <asp:BoundField DataField="YOE" HeaderText="Experience(Year)" />
                                                            <asp:TemplateField HeaderText="Image">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="img" Width="40px" Height="40px" runat="server" ImageUrl='<%#Eval("Attachment", "~/ExpAttachments/{0}") %>' OnClick="lnkEduPrint_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPrintExp" runat="server" Text="See Picture" ToolTip="Print Employee Education Record" CommandArgument='<%#Eval("Attachment")%>' OnClick="lnkPriorPrint_Click" CssClass="lnk">
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                                <ControlStyle CssClass="lnk"></ControlStyle>
                                                            </asp:CommandField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grid_hdr" />
                                                        <RowStyle CssClass="grid_row" />
                                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane7" runat="server">
                <Header>
                    <br />
                    <div class="p-2 bg-info font-weight-bold">
                        <h2 style="color: white !important;">Tenure Experience </h2>
                    </div>
                </Header>
                <Content>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="card card-shadow">
                                <div class="form-group" style="padding-left: 10px; padding-right: 10px;">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:ValidationSummary ID="ValidationSummary6" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                                        ValidationGroup="main" />
                                                    <uc1:Messages ID="ucTMessage" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Post Held*</label>
                                                    <asp:DropDownList ID="ddlTenurePost" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0">Select Post</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Place of Posting</label>
                                                    <asp:DropDownList ID="ddlDivsion" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                        <asp:ListItem Selected="True" Value="0">Select Division</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtyearExp" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                                    <%--<asp:DropDownList ID="ddlExpYear" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="true">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Additional Charge(Post)</label>
                                                    <asp:DropDownList ID="ddlTenureAddtional" runat="server" CssClass="form-control additi" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 addti">
                                                    <label>Additional Charge(Post)</label>
                                                    <asp:TextBox ID="txtaddtionalChargePost" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Additional Charge(Place)</label>
                                                    <asp:DropDownList ID="ddlAdditionPlace" runat="server" CssClass="form-control place" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3 addplacee">
                                                    <label>Additional Charge(Place)</label>
                                                    <asp:TextBox ID="txtadditionalPlace" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Job Type*</label>
                                                    <asp:DropDownList ID="ddljobtype" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Job Type*</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:DropDownList ID="ddlExpScale" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="true">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>
                                                <%--<div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Designation*</label>
                                            <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Select Designation</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator54" runat="server" ControlToValidate="ddlDesignation"
                                                ErrorMessage="Please select designation" SetFocusOnError="true" ValidationGroup="main"
                                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </div>--%>
                                                <%--<div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Department</label>
                                                    <asp:DropDownList ID="ddlExpDept" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Department</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlExpDept"
                                                        ErrorMessage="Please select department" SetFocusOnError="true" ValidationGroup="main"
                                                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Designation</label>
                                                    <asp:DropDownList ID="ddlExpDesig" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Designation</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlExpDesig"
                                                        ErrorMessage="Please select designation" SetFocusOnError="true" ValidationGroup="main"
                                                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </div>--%>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Scale</label>
                                                    <asp:DropDownList ID="ddlTenureScale" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>From Date*</label>
                                                    <asp:TextBox ID="txtjoi" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtjoinCal" runat="server" TargetControlID="txtjoi" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>To Date</label>
                                                    <asp:TextBox ID="txtlea" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtleaCal" runat="server" TargetControlID="txtlea" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Experience(Year)*</label>
                                                    <asp:DropDownList ID="dllexp" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Value="0">Select Year</asp:ListItem>
                                                        <asp:ListItem Value="1">1</asp:ListItem>
                                                        <asp:ListItem Value="2">2</asp:ListItem>
                                                        <asp:ListItem Value="3">3</asp:ListItem>
                                                        <asp:ListItem Value="4">4</asp:ListItem>
                                                        <asp:ListItem Value="5">5</asp:ListItem>
                                                        <asp:ListItem Value="6">6</asp:ListItem>
                                                        <asp:ListItem Value="7">7</asp:ListItem>
                                                        <asp:ListItem Value="8">8</asp:ListItem>
                                                        <asp:ListItem Value="9">9</asp:ListItem>
                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtyearExp" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                                    <%--<asp:DropDownList ID="ddlExpYear" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="true">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Exprerience(Month)</label>
                                                    <asp:DropDownList ID="ddlTenureMonth" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="True" Value="0">Select Month</asp:ListItem>
                                                        <asp:ListItem Value="1">1</asp:ListItem>
                                                        <asp:ListItem Value="2">2</asp:ListItem>
                                                        <asp:ListItem Value="3">3</asp:ListItem>
                                                        <asp:ListItem Value="4">4</asp:ListItem>
                                                        <asp:ListItem Value="5">5</asp:ListItem>
                                                        <asp:ListItem Value="6">6</asp:ListItem>
                                                        <asp:ListItem Value="7">7</asp:ListItem>
                                                        <asp:ListItem Value="8">8</asp:ListItem>
                                                        <asp:ListItem Value="9">9</asp:ListItem>
                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtyearExp" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                                    <%--<asp:DropDownList ID="ddlExpYear" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="true">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>
                                                <div class="col-lg-3 col-md-3  col-sm-3" style="margin-top: 35px;">
                                                    <asp:FileUpload ID="TenureFileUpload" CssClass="Tenfileupload" runat="server" />
                                                </div>
                                                <div class="col-lg-3 col-md-3  col-sm-3" style="margin-top: 20px; margin-left: 45px;">
                                                    <asp:Image ID="TenureExpImage" ImageUrl="" Width="70" Height="70" CssClass="imageTenureshow" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <br />
                                                    <asp:Button runat="server" ID="Button2" OnClick="btnEmpTenureExp_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                                                    <asp:Button runat="server" ID="Button4" OnClick="EmpExpTenureClear_Clear" class="btn btn-danger" Text="Clear"></asp:Button>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>

                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:GridView ID="gedTenure" runat="server" CssClass="table table-responsive-sm" DataKeyNames="TenID,EmpID" OnSelectedIndexChanged="grdTenEmps_SelectedIndexChanged"
                                                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdTenEmps_PageIndexChanging" OnRowDataBound="grdten_rowbound"
                                                        EmptyDataText="No Experience Record" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="Postedas" HeaderText="Post Held" />
                                                            <asp:BoundField DataField="br_nme" HeaderText="Place of Posting" />
                                                            <asp:BoundField DataField="joinDate" HeaderText="From Date" />
                                                            <asp:BoundField DataField="LeavDate" HeaderText="To Date" />
                                                            <asp:BoundField DataField="YOE" HeaderText="Experience (Year)" />
                                                            <asp:BoundField DataField="JobTypeName1" HeaderText="Job Type" />
                                                            <asp:TemplateField HeaderText="Image">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="img" Width="40px" Height="40px" runat="server" ImageUrl='<%#Eval("Attachment", "~/Attachments/{0}") %>' OnClick="lnkEduPrint_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPrintTenExp" runat="server" Text="See Picture" ToolTip="Print Employee Tenore Exprience Record" CommandArgument='<%#Eval("Attachment")%>' OnClick="lnkTenPrint_Click" CssClass="lnk">
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                                <ControlStyle CssClass="lnk"></ControlStyle>
                                                            </asp:CommandField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grid_hdr" />
                                                        <RowStyle CssClass="grid_row" />
                                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                <Header>
                    <br />
                    <div class="p-2 bg-info font-weight-bold">
                        <h2 style="color: white !important;">ACR Record </h2>
                    </div>
                </Header>
                <Content>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="card card-shadow">
                                <div class="form-group" style="padding-left: 10px; padding-right: 10px;">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:ValidationSummary ID="ValidationSummary2" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                                        ValidationGroup="main" />
                                                    <uc1:Messages ID="ucAcrMessage" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>Post Held*</label>
                                                    <asp:DropDownList ID="dllpostingdes" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Designation</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator53" runat="server" ControlToValidate="dllpostingdes"
                                                        ErrorMessage="Please select designation" SetFocusOnError="true" ValidationGroup="main"
                                                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>From Date*</label>
                                                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDateFromCal" runat="server" TargetControlID="txtDateFrom" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>To Date</label>
                                                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDateToCal" runat="server" TargetControlID="txtDateTo" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>Reporting Officer(Name)*</label>
                                                    <asp:TextBox ID="ReporOff" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>Reporting Officer(Designation)*</label>
                                                    <asp:DropDownList ID="ddlropdes" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Designation</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlropdes"
                                                        ErrorMessage="Please Reporting Officer select designation" SetFocusOnError="true" ValidationGroup="main"
                                                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>Signing Date*</label>
                                                    <asp:TextBox ID="txtrepDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtrepDateCal" runat="server" TargetControlID="txtrepDate" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>Counter Signing Officer(Name)</label>
                                                    <asp:TextBox ID="txtcountroff" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>Counter Signing Officer(Designation)</label>
                                                    <asp:DropDownList ID="ddloffDes" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Designation</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddloffDes"
                                                        ErrorMessage="Please select Counter Officer designation" SetFocusOnError="true" ValidationGroup="main"
                                                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </div>
                                                <%--<div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>counter Signing Date</label>
                                                    <asp:TextBox ID="txtofficerDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtofficerDateCal" runat="server" TargetControlID="txtofficerDate" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>--%>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-4 col-md-4 col-sm-4">
                                                    <label>Adverse Remarks</label>
                                                    <asp:DropDownList ID="ddlAdverse" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="True">Yes</asp:ListItem>
                                                        <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">

                                                <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top: 35px;">
                                                    <asp:FileUpload ID="AcrfileUpload" CssClass="acrfileupload" runat="server" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top: 20px; margin-left: 45px;">
                                                    <asp:Image ID="AcrImage" ImageUrl="" Width="70" Height="70" CssClass="imageAcrshow" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">

                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <label>Remarks</label>
                                                    <asp:TextBox ID="txtacrRemaks" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,1000);" onblur="LimitText(this,1000);" Height="80px"> </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <br />
                                                    <asp:Button runat="server" ID="Button11" OnClick="EmpAcr_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                                                    <asp:Button runat="server" ID="Button13" OnClick="EmpAcrClear_Clear" class="btn btn-danger" Text="Clear"></asp:Button>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:GridView ID="grdAcr" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpAcrID,EmpID" OnSelectedIndexChanged="grdAcrEmps_SelectedIndexChanged"
                                                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdAcrEmps_PageIndexChanging" OnRowDataBound="grdAcr_rowbound"
                                                        EmptyDataText="There is no employee Experience" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="CodeDesc" HeaderText="Post Held" />
                                                            <asp:BoundField DataField="DateFrom" HeaderText="From Date" />
                                                            <asp:BoundField DataField="DateTo" HeaderText="To Date" />
                                                            <asp:BoundField DataField="ReportingOfficer" HeaderText="Reporting Officer(Name)" />
                                                            <asp:BoundField DataField="RepOffDes" HeaderText="Reporting Officer(Designation)" />
                                                            <asp:BoundField DataField="RepOffDate" HeaderText="Signing Date" />
                                                            <%--<asp:TemplateField>
                                                            <ItemTemplate>
                                                            <asp:Image ID="img" Width="40px" Height="40px" runat="server" ImageUrl='<%#Eval("Attachment", "~/Attachments/{0}") %>'  OnClick="lnkEduPrint_Click"/>
                                                        </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkPrintAcrExp" runat="server" Text="Download" ToolTip="Print ACR  Record" CommandArgument='<%#Eval("Attachment")%>' OnClick="lnkAcrPrint_Click" CssClass="lnk">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                                <ControlStyle CssClass="lnk"></ControlStyle>
                                                            </asp:CommandField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grid_hdr" />
                                                        <RowStyle CssClass="grid_row" />
                                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Content>
            </ajaxToolkit:AccordionPane>

            <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                <Header>
                    <br />
                    <div class="p-2 bg-info font-weight-bold">
                        <h2 style="color: white !important;">Enquiries/Disciplinary Proceedings</h2>
                    </div>
                </Header>
                <Content>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="card card-shadow">
                                <div class="form-group" style="padding-left: 10px; padding-right: 10px;">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:ValidationSummary ID="ValidationSummary3" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                                        ValidationGroup="main" />
                                                    <uc1:Messages ID="ucEnqMessage" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Type*</label>
                                                    <asp:DropDownList ID="ddlEnqtypes" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Title*</label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtEnqtitle"></asp:TextBox>
                                                </div>

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Authority/Forum*</label>
                                                    <asp:TextBox ID="IssuAuthori" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Status*</label>
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="Initiated">Initiated</asp:ListItem>
                                                        <asp:ListItem Value="In Progress">In Progress</asp:ListItem>
                                                        <asp:ListItem Value="Disposed Off">Disposed Off</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                            </div>

                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Date*</label>
                                                    <asp:TextBox ID="EnqDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="EnqDateEnq" runat="server" TargetControlID="EnqDate" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top: 35px;">
                                                    <asp:FileUpload ID="enqfileupload" CssClass="enqfileupload" runat="server" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top: 20px;">
                                                    <asp:Image ID="EnqImage" ImageUrl="" Width="70" Height="70" CssClass="imageenqshow" runat="server" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <label>Remarks</label>
                                                    <%--<asp:TextBox ID="EnqRemarks" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                                    <asp:TextBox ID="txtarearemaks" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,5000);" onblur="LimitText(this,1000);" Height="80px"> </asp:TextBox>
                                                </div>
                                            </div>

                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:Button runat="server" ID="Button14" OnClick="btnEnq_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                                                    <asp:Button runat="server" ID="Button15" OnClick="EmpEnqClear_Clear" class="btn btn-danger" Text="Clear"></asp:Button>
                                                </div>
                                            </div>
                                            &nbsp;
                                             <div class="row">
                                                 <div class="col-lg-12 col-md-12 col-sm-12">
                                                     <asp:GridView ID="grdEnq" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpAcrID,EmpID" OnSelectedIndexChanged="grdEnqEmps_SelectedIndexChanged"
                                                         AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEnqEmps_PageIndexChanging" OnRowDataBound="grdenq_rowbound"
                                                         EmptyDataText="There is no employee Enquiry" Width="100%">
                                                         <Columns>
                                                             <asp:BoundField DataField="EnquiryAud" HeaderText="Type" />
                                                             <asp:BoundField DataField="EnqTitle" HeaderText="Title" />
                                                             <asp:BoundField DataField="IssuAut" HeaderText="Authority/Forum" />
                                                             <asp:BoundField DataField="Statuss" HeaderText="Status" />
                                                             <asp:BoundField DataField="EnquiryDate" HeaderText="Date" />

                                                             <asp:TemplateField HeaderText="Image">
                                                                 <ItemTemplate>
                                                                     <asp:Image ID="img" Width="40px" Height="40px" runat="server" ImageUrl='<%#Eval("Attachment", "~/Attachments/{0}") %>' OnClick="lnkEduPrint_Click" />
                                                                 </ItemTemplate>
                                                             </asp:TemplateField>
                                                             <asp:TemplateField>
                                                                 <ItemTemplate>
                                                                     <asp:LinkButton ID="lnkPrintAcrExp" runat="server" Text="See Picture" ToolTip="Print ACR  Record" CommandArgument='<%#Eval("Attachment")%>' OnClick="lnkEnqPrint_Click" CssClass="lnk">
                                                                     </asp:LinkButton>
                                                                 </ItemTemplate>
                                                             </asp:TemplateField>
                                                             <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                                 <ControlStyle CssClass="lnk"></ControlStyle>
                                                             </asp:CommandField>
                                                         </Columns>
                                                         <HeaderStyle CssClass="grid_hdr" />
                                                         <RowStyle CssClass="grid_row" />
                                                         <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                         <SelectedRowStyle CssClass="gridSelectedRow" />
                                                     </asp:GridView>

                                                 </div>
                                             </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <hr title="Update Enquiry Status" />

                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <p><u style="color: white; font-size: 20px; background-color: #17a2b8; padding: 10px">Proceedings</u></p>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Title*</label>
                                                    <asp:DropDownList ID="ddlenquirytitle" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Status*</label>
                                                    <asp:DropDownList ID="ddlUpdaStatus" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                                        <asp:ListItem Value="In Progress">In Progress</asp:ListItem>
                                                        <asp:ListItem Value="Disposed Off">Disposed  Off</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Date*</label>
                                                    <asp:TextBox ID="txtupdatedate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtupdatedateCal" runat="server" TargetControlID="txtupdatedate" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                            </div>
                                            &nbsp;
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <label>Remarks</label>
                                                    <%--<asp:TextBox ID="EnqRemarks" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                                    <asp:TextBox ID="txtUpdateRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,5000);" Height="80px"> </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <br />
                                                    <asp:Button runat="server" ID="Button16" class="btn btn-primary" Text="Update" OnClick="UdapteEnq_Click" ValidationGroup=""></asp:Button>

                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                            </div>
                                            &nbsp;
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:GridView ID="grdEnqUpdate" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EnqDelID" OnSelectedIndexChanged="grdEnqUp_SelectedIndexChanged"
                                                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEnqup_PageIndexChanging" OnRowDataBound="grdenqup_rowbound"
                                                        EmptyDataText="There is no employee Enquiry/Audit" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="OngionEnq" HeaderText="Title" />
                                                            <asp:BoundField DataField="updateStatus" HeaderText="Status" />
                                                            <asp:BoundField DataField="UpdateDate" HeaderText="Date" />
                                                            <asp:BoundField DataField="updateremarks" HeaderText="Remarks" />
                                                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                                <ControlStyle CssClass="lnk"></ControlStyle>
                                                            </asp:CommandField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grid_hdr" />
                                                        <RowStyle CssClass="grid_row" />
                                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Content>
            </ajaxToolkit:AccordionPane>

            <ajaxToolkit:AccordionPane ID="AccordionPane5" runat="server">
                <Header>
                    <br />
                    <div class="p-2 bg-info font-weight-bold">
                        <h2 style="color: white !important;">Litigations / Court Cases </h2>
                    </div>
                </Header>
                <Content>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="card card-shadow">
                                <div class="form-group" style="padding-left: 10px; padding-right: 10px;">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:ValidationSummary ID="ValidationSummary4" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                                        ValidationGroup="main" />
                                                    <uc1:Messages ID="ucLitiMessage" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Type*</label>

                                                    <asp:DropDownList ID="ddlLitigation" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0">Litigation Type</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator55" runat="server" ErrorMessage="Litigation Type is required"
                                                        ControlToValidate="ddlLitigation" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Title*</label>
                                                    <%--<asp:TextBox ID="litiRemarks" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                                    <%--<textarea id="txtAreaLitiRemarks" runat="server" class="form-control" cols="20" rows="2"></textarea>--%>
                                                    <asp:TextBox ID="txtLitiTitle" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>



                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Authority/Forum*</label>
                                                    <asp:TextBox ID="authorityforum" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <%--<asp:DropDownList ID="DropDownList9" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Anticorruption</asp:ListItem>
                                                        <asp:ListItem Value="1">Court</asp:ListItem>
                                                        <asp:ListItem Value="1">Mohtasib</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Authority Title*</label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAuthorityTitle"></asp:TextBox>
                                                </div>
                                            </div>
                                            &nbsp;
                                            <div class="row">

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Status*</label>
                                                    <asp:DropDownList ID="status" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                                        <asp:ListItem Value="Initiated">Initiated</asp:ListItem>
                                                        <asp:ListItem Value="In Progress">In Progress</asp:ListItem>
                                                        <asp:ListItem Value="Dispossed Off">Disposed  Off</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Date*</label>
                                                    <asp:TextBox ID="litiDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="litiDateliti" runat="server" TargetControlID="litiDate" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <asp:FileUpload runat="server" CssClass="litifile" ID="litifileUpload" Style="margin-top: 30px;" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <asp:Image ImageUrl="" ID="litiImage" Height="70" Width="70" CssClass="litiImage" runat="server" />
                                                </div>
                                            </div>
                                            &nbsp;
                                            <div class="row">

                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <label>Remarks</label>
                                                    <%--<asp:TextBox ID="litiRemarks" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                                    <%--<textarea id="txtAreaLitiRemarks" runat="server" class="form-control" cols="20" rows="2"></textarea>--%>
                                                    <asp:TextBox ID="txtareaEnqremarks" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,5000);" onblur="LimitText(this,5000);" Height="80px"> </asp:TextBox>
                                                </div>
                                            </div>
                                            &nbsp;
                                            
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:Button runat="server" ID="Button17" OnClick="btnLiti_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                                                    <asp:Button runat="server" ID="Button18" class="btn btn-danger" Text="Clear"></asp:Button>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:GridView ID="grdLiti" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpAcrID,EmpID" OnSelectedIndexChanged="grdLitiEmps_SelectedIndexChanged"
                                                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdLitiEmps_PageIndexChanging" OnRowDataBound="grdLiti_rowbound"
                                                        EmptyDataText="There is no employee Litigation Record" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="LitiName" HeaderText="Type" />
                                                            <asp:BoundField DataField="LitiTitle" HeaderText="Title" />
                                                            <asp:BoundField DataField="Authority" HeaderText="Authority/Forum" />
                                                            <asp:BoundField DataField="status" HeaderText="Status" />
                                                            <asp:BoundField DataField="LitiDate" HeaderText="Date" />

                                                            <asp:TemplateField HeaderText="Image">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="img" Width="40px" Height="40px" runat="server" ImageUrl='<%#Eval("Attachment", "~/Attachments/{0}") %>' OnClick="lnkEduPrint_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPrintAcrExp" runat="server" Text="See Picture" ToolTip="Print ACR  Record" CommandArgument='<%#Eval("Attachment")%>' OnClick="lnkLitiPrint_Click" CssClass="lnk">
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                                <ControlStyle CssClass="lnk"></ControlStyle>
                                                            </asp:CommandField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grid_hdr" />
                                                        <RowStyle CssClass="grid_row" />
                                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <hr title="Update litigation/legal section Status" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <p><u style="color: white; font-size: 20px; background-color: #17a2b8; padding: 10px">Proceedings</u></p>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Title*</label>
                                                    <asp:DropDownList ID="ddlLitigationUpd" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Litigation Type is required"
                                                        ControlToValidate="ddlLitigationUpd" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>


                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Status*</label>
                                                    <asp:DropDownList ID="ddlStatLitiup" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                                        <asp:ListItem Value="In Progress">In Progress</asp:ListItem>
                                                        <asp:ListItem Value="Dispossed Off">Disposed  Off</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Date*</label>
                                                    <asp:TextBox ID="txtFinalDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtFinalDateCal" runat="server" TargetControlID="txtFinalDate" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>


                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <label>Remarks</label>
                                                    <asp:TextBox ID="txtFinaljud" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,5000);" onblur="LimitText(this,5000);" Height="80px"> </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <br />
                                                    <asp:Button runat="server" ID="Button19" OnClick="updateliti_click" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                                                    <asp:Button runat="server" ID="Button244" OnClick="EmpLitiClear_Clear" CssClass="btn btn-danger" Text="Clear" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label></label>
                                                </div>
                                            </div>
                                            &nbsp;
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:GridView ID="grdlitiup" runat="server" CssClass="table table-responsive-sm" DataKeyNames="litiDeID" OnSelectedIndexChanged="grdlitiup_SelectedIndexChanged"
                                                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdlitiup_PageIndexChanging" OnRowDataBound="grdLitiup_rowbound"
                                                        EmptyDataText="There is no employee Litigation Record" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="LitiTitle" HeaderText="Title" />
                                                            <asp:BoundField DataField="statuss" HeaderText="Status" />
                                                            <asp:BoundField DataField="FinalDate" HeaderText="Date" />
                                                            <asp:BoundField DataField="FinalJud" HeaderText="Remarks" />
                                                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                                <ControlStyle CssClass="lnk"></ControlStyle>
                                                            </asp:CommandField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grid_hdr" />
                                                        <RowStyle CssClass="grid_row" />
                                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                            <br />


                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Content>
            </ajaxToolkit:AccordionPane>

            <ajaxToolkit:AccordionPane ID="AccordionPane6" runat="server">
                <Header>
                    <br />
                    <div class="p-2 bg-info font-weight-bold">
                        <h2 style="color: white !important;">Promotion/Time Scale/Upgradation </h2>
                    </div>
                </Header>
                <Content>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="card card-shadow mb-4">
                                <div class="form-group" style="padding-left: 10px; padding-right: 10px;">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:ValidationSummary ID="ValidationSummary5" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                                        ValidationGroup="main" />
                                                    <uc1:Messages ID="ucCpfMessage" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Designation*</label>
                                                    <asp:DropDownList ID="ddlPerDes" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Designation</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Scale*</label>
                                                    <asp:DropDownList ID="ddlperscal" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Scale</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>w.e.f Date*</label>
                                                    <asp:TextBox ID="txtperfrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtperfromCal" runat="server" TargetControlID="txtperfrom" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Order Date</label>
                                                    <asp:TextBox ID="txtPerTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtPerToCal" runat="server" TargetControlID="txtPerTo" Enabled="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <label>Type*</label>
                                                    <asp:DropDownList ID="ddlpertypes" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">Select Type</asp:ListItem>
                                                        <asp:ListItem Value="Appointment">Appointment</asp:ListItem>
                                                        <asp:ListItem Value="Promotion">Promotion</asp:ListItem>
                                                        <asp:ListItem Value="Time Scale">Time Scale</asp:ListItem>
                                                        <asp:ListItem Value="Upgradation">Upgradation</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top: 15px;">
                                                    <label>Order</label>
                                                    <asp:FileUpload ID="promotionAttach" CssClass="Profileupload" runat="server" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top: 20px; margin-left: 45px;">
                                                    <asp:Image ID="tblPRo" ImageUrl="" Width="70" Height="70" CssClass="Proshow" runat="server" />
                                                </div>

                                            </div>


                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <br />
                                                    <asp:Button runat="server" ID="Button20" OnClick="btnper_save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                                                    <asp:Button runat="server" ID="Button21" OnClick="btnPer_Clear" class="btn btn-danger" Text="Clear"></asp:Button>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:GridView ID="grdPermotion" runat="server" CssClass="table table-responsive-sm" DataKeyNames="PerID" OnSelectedIndexChanged="grdPerEmps_SelectedIndexChanged"
                                                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdPerEmps_PageIndexChanging" OnRowDataBound="grdPer_rowbound"
                                                        EmptyDataText="No Record" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="pertype" HeaderText="Type" />
                                                            <asp:BoundField DataField="CodeDesc" HeaderText="Designation" />
                                                            <asp:BoundField DataField="ScaleName" HeaderText="Scale" />
                                                            <asp:BoundField DataField="FromDate" HeaderText="w.e.f Date" />
                                                            <asp:BoundField DataField="todate" HeaderText="Order Date" />
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="img" Width="40px" Height="40px" runat="server" ImageUrl='<%#Eval("Attachement", "~/Attachments/{0}") %>' OnClick="lnkEduPrint_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPrintPro" runat="server" Text="See Picture" ToolTip="Print ACR  Record" CommandArgument='<%#Eval("Attachement")%>' OnClick="lnkProPrint_Click" CssClass="lnk">
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                                <ControlStyle CssClass="lnk"></ControlStyle>
                                                            </asp:CommandField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grid_hdr" />
                                                        <RowStyle CssClass="grid_row" />
                                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Content>
            </ajaxToolkit:AccordionPane>
        </Panes>
    </ajaxToolkit:Accordion>

    <script>
        $('#<%=ddlEmployeeSearch.ClientID%>').chosen();
        $('#<%=ddlperson.ClientID%>').chosen();
    </script>

</asp:Content>







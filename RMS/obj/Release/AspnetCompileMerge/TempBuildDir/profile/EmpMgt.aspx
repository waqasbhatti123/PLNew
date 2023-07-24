<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgt.aspx.cs" Inherits="RMS.Profile.EmpMgt" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>

    <script src="http://code.jquery.com/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.16.js"></script>
    <link href="../Scripts/jquery-ui.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            debugger
            var brID = '<%=Session["BranchID"].ToString() %>';
            if (brID == "1") {
                $(".divi").show();
            }
            else {
                $(".divi").hide();
            }





            //var vaa = $(".addtional").val();
            //if (vaa == "Other") {
            //    $(".addti").show();
            //}








            $(".addtional").change(function () {
                var v = $(this).val();
                if (v == "Other") {
                    $(".addti").show();
                }
                else {
                    $(".addti").hide();
                }
            });

            $(".addplace").change(function () {
                var va = $(this).val();
                if (va == 17) {
                    $(".addplacee").show();
                }
                else {
                    $(".addplacee").hide();
                }
            });

            if ($(".addtional").val() == "Other") {
                $(".addti").show();
            }
            else {
                $(".addti").hide();
                $(".addplacee").hide();
            }

            if ($(".addplace").val() == 17) {
                $(".addplacee").show();
            }
            else {
                $(".addti").hide();
                $(".addplacee").hide();
            }

            $(".ddlEmpDrpdwnty").change(function () {
                var valll = $(this).val();
                alert(valll);
            });


        });
        function ImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=empImage.ClientID%>').prop('src', e.target.result)
                        .width(110)
                        .height(110);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        function ImageCNIC(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imageID.ClientID%>').prop('src', e.target.result)
                        .width(70)
                        .height(70);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        function ImageApp(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=appoImage.ClientID%>').prop('src', e.target.result)
                        .width(70)
                        .height(70);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        function ImageOrder(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=orderImage.ClientID%>').prop('src', e.target.result)
                        .width(70)
                        .height(70);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        function ImageRegul(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=regID.ClientID%>').prop('src', e.target.result)
                        .width(70)
                        .height(70);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }



    </script>

    <script type="text/javascript">





<%--        $(function () {
            $('#<%=BranchDropDown.ClientID %>').change(function () {
                debugger
                var branch = $('#<%=BranchDropDown.ClientID %>').val();
                CascadingBranch(branch);
            });
        });

        function CascadingBranch(branchchange) {
               

                $.ajax({
                    url: "EmpMgt.aspx/GetCascadingBranchChange",
                    data: JSON.stringify({ Branches: branchchange }),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; chartset=utf-8",
                    success: function (data) {
                         debugger;
                        var chk = data.d;

                        $('#<%=ddlDictric.ClientID %>').empty();
                        $('#<%=ddlDictric.ClientID %>').append("<option value='" + "0" + "'>" + "Select District" + "</option>")
                        $.each(data.d, function (index, row) {

                                $('#<%=ddlDictric.ClientID %>').append("<option value='" + row.br_id + "'>" + row.br_nme + "</option>")
                            });


                    },
                    error: function () {
                        alert("Error loading data! Please try again.");
                    }
                })

            }--%>

        function pageLoad() {

            $('#<%=rblScsiEnbl.ClientID %>').change(function (event) {

                if ($('[id*=rblScsiEnbl]').find('input:checked').val() == "true") {

                    $('[id*=txtScsi]').attr("disabled", false);
                }
                else {
                    $('[id*=txtScsi]').val("");
                    $('[id*=txtScsi]').attr("disabled", true);
                }

            });

            $('#<%=rdoHealthIns.ClientID%>').change(function (event) {
                if ($('[id*=rdoHealthIns]').find('input:checked').val() == 'true') {
                    $('[id*=txtHealthInsurance]').attr("disabled", false);
                }
                else {
                    $('[id*=txtHealthInsurance]').val("");
                    $('[id*=txtHealthInsurance]').attr("disabled", true);
                }
            });

            $('#<%=rdDeput.ClientID%>').change(function (event) {
                debugger
                if ($('[id*=rdDeput]').find('input:checked').val() == "True") {
                    debugger
                    $('[id*=txtDepu]').attr("disabled", false);
                }
                else {
                    $('[id*=txtDepu]').val("");
                    $('[id*=txtDepu]').attr("disabled", true);
                }
            });

            $('#<%=rblEobiEnbl.ClientID %>').change(function (event) {

                if ($('[id*=rblEobiEnbl]').find('input:checked').val() == "true") {

                    $('[id*=txtEobi]').attr("disabled", false);

                }
                else {
                    $('[id*=txtEobi]').val("");
                    $('[id*=txtEobi]').attr("disabled", true);
                }

            });



        }



        function clickSameAsAbove() {
            //alert(document.getElementById("chkSameAsAboveId"));
            if (document.getElementById('chkSameAsAboveId').checked) {
                var txtAreaz = document.getElementsByTagName("TEXTAREA");
                txtAreaz[0].value = txtAreaz[0].value.trim();
                txtAreaz[1].value = txtAreaz[0].value.trim();

                //       alert("Length: "
                //      + txtAreaz.length);
                //      alert("First address: "
                //      + txtAreaz[0].value);
                //      alert("second addresss: "
                //      + txtAreaz[1].value);       
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <%-- <ajaxToolkit:Accordion ID="a1" runat="server" SelectedIndex="0" FadeTransitions="true" TransitionDuration="1000" FramesPerSecond="200" Width="100%">
        <Panes>
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    <br />
                    <div class="p-2 bg-info font-weight-bold">
                        <h2 style="color:white !important;">Employee Profile </h2>
                    </div>
                </Header>
                <Content>--%>
    <div class="row" id="hiddenclick">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow">
                <asp:Panel runat="server" ID="pnlMain">
                </asp:Panel>


                <div class="form-group" style="padding-left: 10px; padding-right: 10px;">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main" />
                            <asp:ValidationSummary ID="ValidationSummary1" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main2" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3"></div>
                        <div class="col-md-3"></div>
                        <div class="col-md-3"></div>
                        <div class="col-md-3">
                            <asp:Image ID="empImage" runat="server" ImageUrl="../empix/noimage.jpg" Width="110" Height="110" />
                            <asp:FileUpload ID="empImageFileUploader" runat="server" onchange="ImagePreview(this);" />


                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Employee Id</label>
                            <asp:TextBox ID="txtEmpId" runat="server" MaxLength="50" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Personal File No*</label>
                            <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmpCode"
                                ErrorMessage="Please enter employee code." SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Name*</label>
                            <asp:TextBox ID="txtFullName" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtFullName"
                                ErrorMessage="Please enter name" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <%--<div class="col-lg-3 col-md-3 col-sm-3">
                                            <label>Sort Reference</label>
                                            <asp:TextBox ID="txtSortReference" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSortReference"
                                                ErrorMessage="Please enter Sort Reference" SetFocusOnError="true" ValidationGroup="main"
                                                Display="None"></asp:RequiredFieldValidator>
                                        </div>--%>
                    </div>

                    <div class="row">

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Father Name*</label>
                            <asp:TextBox ID="txtFatherName" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Mother Name</label>
                            <asp:TextBox ID="txtMotherName" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Gender</label>
                            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="M" Selected="True">Male</asp:ListItem>
                                <asp:ListItem Value="F">Female</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="reqGender" runat="server" ControlToValidate="rblGender"
                                ErrorMessage="Please select gender" SetFocusOnError="true" ValidationGroup="main"
                                Display="None">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Domicile</label>
                            <asp:DropDownList ID="ddlDomicile" runat="server" CssClass="form-control">
                                <asp:ListItem Selected="True" Value="0">Select City</asp:ListItem>
                                <asp:ListItem Value="Bahawalnagar">Bahawalnagar</asp:ListItem>
                                <asp:ListItem Value="Bahawalpur">Bahawalpur</asp:ListItem>
                                <asp:ListItem Value="Bhakkar">Bhakkar</asp:ListItem>
                                <asp:ListItem Value="Chakwal">Chakwal</asp:ListItem>
                                <asp:ListItem Value="Chiniot">Chiniot</asp:ListItem>
                                <asp:ListItem Value="D.G.Khan">D.G.Khan</asp:ListItem>
                                <asp:ListItem Value="Faisalabad">Faisalabad</asp:ListItem>
                                <asp:ListItem Value="Gujranwala">Gujranwala</asp:ListItem>
                                <asp:ListItem Value="Hafizabad">Hafizabad</asp:ListItem>
                                <asp:ListItem Value="Jhang">Jhang</asp:ListItem>
                                <asp:ListItem Value="Jhelum">Jhelum</asp:ListItem>
                                <asp:ListItem Value="Kasur">Kasur</asp:ListItem>
                                <asp:ListItem Value="Khanewal">Khanewal</asp:ListItem>
                                <asp:ListItem Value="Khushab">Khushab</asp:ListItem>
                                <asp:ListItem Value="Lahore">Lahore</asp:ListItem>
                                <asp:ListItem Value="Layyah">Layyah</asp:ListItem>
                                <asp:ListItem Value="Mandi Baha ud din">Mandi Baha ud din</asp:ListItem>
                                <asp:ListItem Value="Mianwali">Mianwali</asp:ListItem>
                                <asp:ListItem Value="Multan">Multan</asp:ListItem>
                                <asp:ListItem Value="Muzaffargarh">Muzaffargarh</asp:ListItem>
                                <asp:ListItem Value="Nankana Sahib">Nankana Sahib</asp:ListItem>
                                <asp:ListItem Value="Narowal">Narowal</asp:ListItem>
                                <asp:ListItem Value="Okara">Okara</asp:ListItem>
                                <asp:ListItem Value="Pakpattan">Pakpattan</asp:ListItem>
                                <asp:ListItem Value="Rajanpur">Rajanpur</asp:ListItem>
                                <asp:ListItem Value="Rawalpindi">Rawalpindi</asp:ListItem>
                                <asp:ListItem Value="Sahiwal">Sahiwal</asp:ListItem>
                                <asp:ListItem Value="Sargodha">Sargodha</asp:ListItem>
                                <asp:ListItem Value="Sheikhupura">Sheikhupura</asp:ListItem>
                                <asp:ListItem Value="Sialkot">Sialkot</asp:ListItem>
                                <asp:ListItem Value="Toba Tek Singh">Toba Tek Singh</asp:ListItem>
                                <asp:ListItem Value="Vehari">Vehari</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Quota</label>
                            <asp:DropDownList ID="ddlQuota" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Selected="True" Value="0">Select Quota</asp:ListItem>
                                <asp:ListItem Value="Open Merit">Open Merit</asp:ListItem>
                                <asp:ListItem Value="Minorty">Minorty</asp:ListItem>
                                <asp:ListItem Value="Woman">Woman</asp:ListItem>
                                <asp:ListItem Value="Govt. Employee">Govt. Employee</asp:ListItem>
                                <asp:ListItem Value="Disable">Disable</asp:ListItem>
                                <asp:ListItem Value="Rule-17A">Rule-17A</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Appointment (Designation)*</label>
                            <asp:DropDownList ID="ddlappointed" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Designation</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlDesignation"
                                ErrorMessage="Please select designation" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Appointment (Scale)*</label>
                            <asp:DropDownList ID="ddlappointscale" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Scale</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ScaleDropDown"
                                ErrorMessage="Please Select Scale" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Appointment(Job Type)*</label>
                            <asp:DropDownList ID="appJobType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Job Type</asp:ListItem>
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddlJobType"
                                ErrorMessage="Please select job type" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Joining Date*</label>
                            <ajaxToolkit:CalendarExtender ID="txtJoinDateCal" runat="server" TargetControlID="txtJoinDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtJoinDate" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                            <%-- <span class="DteLtrl">
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFullYearFormatPageText %>" />
                                </span>--%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtJoinDate"
                                ErrorMessage="Please select joining date" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Regularization Date</label>
                            <ajaxToolkit:CalendarExtender ID="txtConfDateCal" runat="server" TargetControlID="txtConfDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtConfDate" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                            <%-- <span class="DteLtrl">
                                    <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFullYearFormatPageText %>" />
                                </span>--%>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Current Job Type*</label>
                            <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Job Type</asp:ListItem>
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlJobType"
                                ErrorMessage="Please select job type" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Last Promotion (Designation)*</label>
                            <asp:DropDownList ID="ddlLastPero" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Designation</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlDesignation"
                                ErrorMessage="Please select designation" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Last Promotion (Scale)*</label>
                            <asp:DropDownList ID="ScaleDropDown" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Scale</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ScaleDropDown"
                                ErrorMessage="Please Select Scale" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Current Posting*</label>
                            <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Designation</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlDesignation"
                                ErrorMessage="Please select Current Posting" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Section*</label>
                            <asp:DropDownList ID="ddlDept" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Section</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlDept"
                                ErrorMessage="Please select department" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Place of Posting*</label>
                            <asp:DropDownList ID="BranchDropDown" runat="server" CssClass="form-control branchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="BranchDropDown"
                                ErrorMessage="Please Select Branch" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <%--<div class="col-lg-3 col-md-3 col-sm-3 divi">
                                            <label>Place of Posting*</label>
                                            <asp:DropDownList ID="dropdownbranch"  runat="server" CssClass="form-control branchchange" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="BranchDropDown"
                                                ErrorMessage="Please Select Branch" SetFocusOnError="true" ValidationGroup="main"
                                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                        </div>--%>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Addition Charge (Post)</label>
                            <asp:DropDownList ID="ddlEmpAddtional" runat="server" CssClass="form-control addtional" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 addti">
                            <label>Additional Charge(Post)</label>
                            <asp:TextBox ID="txtaddtionalChargePost" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Addition Charge (Place)</label>
                            <asp:DropDownList ID="ddlAddPlace" runat="server" CssClass="form-control addplace" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 addplacee">
                            <label>Additional Charge(Place)</label>
                            <asp:TextBox ID="txtadditionalPlace" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Relieving (Reason)</label>
                            <asp:DropDownList ID="ddlRelieving" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Selected="True" Value="1">N/A</asp:ListItem>
                                <asp:ListItem Value="2">Resignation</asp:ListItem>
                                <asp:ListItem Value="3">Termination</asp:ListItem>
                                <asp:ListItem Value="4">Retirement</asp:ListItem>
                                <asp:ListItem Value="5">New Job (NOC)</asp:ListItem>
                                <asp:ListItem Value="6">Contract Expire</asp:ListItem>
                                <asp:ListItem Value="7">Other</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Relieving Date</label>
                            <asp:TextBox ID="txtRelieving" runat="server" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtRelievingCal" runat="server" TargetControlID="txtRelieving" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Deputation</label>
                            <asp:RadioButtonList ID="rdDeput" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                    </div>

                    <div class="row">
                    </div>

                    <div class="row">

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label></label>
                        </div>
                    </div>

                    <div class="row">
                    </div>

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <br />
                            <hr title="" style="line-break: normal" />
                            <br />
                        </div>
                    </div>



                    <div class="row">

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Marital Status</label>
                            <asp:RadioButtonList ID="rblMarStatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                                <asp:ListItem Value="S">Single</asp:ListItem>
                                <asp:ListItem Value="M" Selected="True">Married</asp:ListItem>
                                <asp:ListItem Value="P">Separated</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>CNIC</label>
                            <asp:TextBox ID="txtNic" runat="server" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtNic" runat="server" MaskType="Number" Mask="99999-9999999-9" InputDirection="LeftToRight">
                            </ajaxToolkit:MaskedEditExtender>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Issue Date</label>
                            <ajaxToolkit:CalendarExtender ID="txtIssueDateCal" runat="server" TargetControlID="txtIssueDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtIssueDate" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                            <%--<span class="DteLtrl">
                                    <asp:Literal ID="Literal3" runat="server" Text="<%$ AppSettings: DateFullYearFormatPageText %>" />
                                </span>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Expiry Date</label>
                            <ajaxToolkit:CalendarExtender ID="txtExpDateCal" runat="server" TargetControlID="txtExpDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtExpDate" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                            <%--<span class="DteLtrl">
                                    <asp:Literal ID="Literal4" runat="server" Text="<%$ AppSettings: DateFullYearFormatPageText %>" />
                                </span>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Date of Birth</label>
                            <ajaxToolkit:CalendarExtender ID="txtDOBCal" runat="server" TargetControlID="txtDOB" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtDOB" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                            <%--<span class="DteLtrl">
                                    <asp:Literal ID="Literal6" runat="server" Text="<%$ AppSettings: DateFullYearFormatPageText %>" />
                                </span>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Phone</label>
                            <asp:TextBox ID="txtPhoneNo" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Mobile</label>
                            <asp:TextBox ID="txtMobNo" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="validEmail" runat="server" ErrorMessage="Invalid email"
                                ControlToValidate="txtEmail" SetFocusOnError="true" ValidationGroup="main" ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)(;)*)*"
                                Display="None"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Highest Qualification:</label>
                            <asp:TextBox ID="txtEdu" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>NTN</label>
                            <asp:TextBox ID="txtNtn" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Relgion</label>
                            <asp:DropDownList ID="ddlReligion" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select Religion</asp:ListItem>
                                <asp:ListItem Value="M">Muslim</asp:ListItem>
                                <asp:ListItem Value="C">Christian</asp:ListItem>
                                <asp:ListItem Value="H">Hindu</asp:ListItem>
                                <asp:ListItem Value="S">Sikh</asp:ListItem>
                                <asp:ListItem Value="O">Other</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Disablity</label>
                            <asp:DropDownList ID="ddlDisablity" runat="server" CssClass="form-control">
                                <asp:ListItem Selected="True" Value="None">None</asp:ListItem>
                                <asp:ListItem Value="Hand Disablity">Hand Disablity</asp:ListItem>
                                <asp:ListItem Value="Leg Disablity">Leg Disablity</asp:ListItem>
                                <asp:ListItem Value="Eye Disablity">Eye Disablity</asp:ListItem>
                                <asp:ListItem Value="Deaf">Deaf</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <label>Current Address*</label>
                            <asp:TextBox ID="txtAdd" runat="server" TextMode="MultiLine" Height="67"
                                onkeyup="LimitText(this,500);" onblur="LimitText(this,500);" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAdd"
                                ErrorMessage="Please enter current address" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <label>Permanent Address*</label>
                            <asp:TextBox ID="txtAdd2Perm" runat="server" TextMode="MultiLine" Height="67" CssClass="form-control"
                                onkeyup="LimitText(this,500);" onblur="LimitText(this,500);"></asp:TextBox>
                            <input id="chkSameAsAboveId" name="chkSameAsAbove" type="checkbox" onchange="clickSameAsAbove()" /><asp:Label runat="server" Text=" Same as above"></asp:Label>
                        </div>

                    </div>

                    <div class="row">

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Bank</label>
                            <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Bank</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Bank Branch</label>
                            <asp:UpdatePanel ID="upnl" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBankBranch" runat="server" MaxLength="50" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlBank" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Account#</label>
                            <asp:TextBox ID="txtBankAcct" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>EOBI</label>
                            <asp:RadioButtonList ID="rblEobiEnbl" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="true" Selected="True">Yes</asp:ListItem>
                                <asp:ListItem Value="false">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>EOBI No</label>
                            <asp:TextBox ID="txtEobi" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>SCSI</label>
                            <asp:RadioButtonList ID="rblScsiEnbl" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="true" Selected="True">Yes</asp:ListItem>
                                <asp:ListItem Value="false">No</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="rblScsiEnbl"
                                ErrorMessage="Please select SCSI Enabled" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>SCSI No*</label>
                            <asp:TextBox ID="txtScsi" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Health Insurance</label>
                            <asp:RadioButtonList ID="rdoHealthIns" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="true" Selected="True">Yes</asp:ListItem>
                                <asp:ListItem Value="false">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Health Insurance No</label>
                            <asp:TextBox ID="txtHealthInsurance" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Police Verification</label>
                            <asp:RadioButtonList ID="rdpoliceveri" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="True" Selected="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Medical</label>
                            <asp:RadioButtonList ID="rdmediver" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="True" Selected="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Degree Verification</label>
                            <asp:RadioButtonList ID="rddegveri" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="True" Selected="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    &nbsp;
                                    <div class="row">
                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                            <label>CNIC</label>
                                            <asp:FileUpload runat="server" onchange="ImageCNIC(this)" ID="fuCNIC" />
                                        </div>
                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                            <asp:Image ImageUrl="Cnic" ID="imageID" Width="60px" Height="60px" runat="server" />
                                        </div>
                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                            <label>Appointment</label>
                                            <asp:FileUpload runat="server" onchange="ImageApp(this)" ID="fuAppointment" />
                                        </div>
                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                            <asp:Image ImageUrl="app" ID="appoImage" Width="60px" Height="60px" runat="server" />
                                        </div>
                                    </div>
                    &nbsp;
                                    <div class="row">
                                    </div>
                    &nbsp;
                                    <div class="row">
                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                            <label>Joioning:</label>
                                            <asp:FileUpload runat="server" onchange="ImageOrder(this)" ID="fuOrder" />
                                        </div>
                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                            <asp:Image ImageUrl="order" ID="orderImage" Width="60px" Height="60px" runat="server" />
                                        </div>
                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                            <label>Regularization:</label>
                                            <asp:FileUpload runat="server" onchange="ImageRegul(this)" ID="RegularID" />
                                        </div>
                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                            <asp:Image ImageUrl="regul" ID="regID" Width="60px" Height="60px" runat="server" />
                                        </div>
                                    </div>
                    <div class="row">


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
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label></label>
                            <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
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
                        <div class="col-md-12">


                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Label ID="lblFltName" runat="server" Text="Emp Name:"></asp:Label><br />
                                    <asp:DropDownList ID="ddlEmpDrpdwn" runat="server" CssClass="form-control ddlEmpDrpdwnty" OnSelectedIndexChanged="ddlEmpDrpdown_change" AutoPostBack="true">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Label ID="Label1" runat="server" Text="Personal File No:"></asp:Label><br />

                                    <asp:DropDownList ID="ddlperson" runat="server" CssClass="form-control ddlEmpDrpdwnrt" OnSelectedIndexChanged="ddlPersonal_change" AutoPostBack="true">
                                        <asp:ListItem Value="0">Select Personal File Number</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top: 30px">
                                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                        OnClick="btnSearch_Click" ToolTip="Search Emps" />
                                </div>
                            </div>

                            <br />
                            <asp:GridView ID="grdEmps" runat="server" DataKeyNames="EmpId" CssClass="table table-responsive-sm" OnSelectedIndexChanged="grdEmps_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEmps_PageIndexChanging"
                                EmptyDataText="There is no employee defined" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="EmpCode" HeaderText="Personal File No" />
                                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                                    <asp:BoundField DataField="Desig" HeaderText="Current Posting" />
                                    <asp:BoundField DataField="Dept" HeaderText="Section" />
                                    <asp:BoundField DataField="branch" HeaderText="Place of Posting" />
                                    <%-- <asp:BoundField DataField="Reg" HeaderText="Region" />--%>
                                    <asp:BoundField DataField="CityName" HeaderText="City" />
                                    <%--<asp:BoundField DataField="LocName" HeaderText="Location" />--%>
                                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                                    <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                        <ControlStyle CssClass="lnk lkk"></ControlStyle>
                                    </asp:CommandField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" ToolTip="Print Employee Profile" OnClick="lnkPrint_Click" CssClass="lnk">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>

                &nbsp;
                               


                                <%--Hidden Fields--%>

                <asp:LinkButton ID="btnViewTransfer" runat="server" Text="Transfers" OnClick="btnViewTransfer_Click"
                    Visible="False"></asp:LinkButton>

                <asp:DropDownList ID="ddlSection" runat="server" CssClass="form-control" AppendDataBoundItems="True" Visible="false">
                    <asp:ListItem Value="0">Select Section</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="True" Visible="false">
                    <asp:ListItem Value="0">Select Location</asp:ListItem>
                </asp:DropDownList>

                <asp:DropDownList ID="ddlDaughterCount" runat="server" AppendDataBoundItems="True" Visible="false">
                    <asp:ListItem Value="0">None</asp:ListItem>
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
                <asp:DropDownList ID="ddlSonCount" runat="server" AppendDataBoundItems="True" Visible="false">
                    <asp:ListItem Value="0">None</asp:ListItem>
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

                <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                            <tr>
                                                <td width="110" align="right">
                                                    <asp:Image runat="server" ID="imgEmp" ImageUrl="../empix/noimage.jpg" Width="110"
                                                        Height="110" />
                                                </td>
                                                <td width="70" align="left">
                                                    <asp:ImageButton ID="btnAddImg" runat="server" ImageUrl="~/images/rightArrow.jpg"
                                                        onMouseOver="this.src='../images/rightArrowAdd.jpg'" onMouseOut="this.src='../images/rightArrow.jpg'"
                                                        CommandName="ShowUploadImg" OnCommand="ButtonCommand" />
                                                    <asp:ImageButton ID="btnUpdImg" runat="server" ImageUrl="~/images/rightArrow.jpg"
                                                        Visible="false" onMouseOver="this.src='../images/rightArrowChange.jpg'" onMouseOut="this.src='../images/rightArrow.jpg'"
                                                        CommandName="ShowUploadImg" OnCommand="ButtonCommand" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:FileUpload runat="server" ID="fileUploadImg" Visible="false" CssClass="form-control" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Button runat="server" ID="btnUploadStart" Text="  Upload  " OnClick="btnUploadStart_Click"
                                                                    Visible="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUploadStart" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAddImg" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>

                <%--End of Hidden Fields--%>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <asp:Panel ID="Panel2" runat="server" Width="100%" Height="600">
                <rsweb:ReportViewer ID="reportViewer" runat="server" Width="100%" Height="580px">
                </rsweb:ReportViewer>
            </asp:Panel>
        </div>
    </div>




    <%--</Content>
            </ajaxToolkit:AccordionPane>
          
        </Panes>
    </ajaxToolkit:Accordion>--%>



    <br />




    <div id="divEmpTransfer" runat="server" visible="false">


        <asp:TextBox ID="txtGradeTransfer" runat="server"></asp:TextBox>
        <asp:GridView ID="grdEmpTransfer" runat="server" DataKeyNames="EmpId" OnRowDataBound="grdEmpTransfer_RowDataBound"
            AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEmpTransfer_PageIndexChanging"
            EmptyDataText="" Width="763px">
            <Columns>
                <asp:BoundField DataField="EfDate" HeaderText="Effective From" />
                <asp:BoundField DataField="Desig" HeaderText="Designation" />
                <asp:BoundField DataField="Dept" HeaderText="Department" />
                <asp:BoundField DataField="Div" HeaderText="Division" />
                <asp:BoundField DataField="Reg" HeaderText="Region" />
                <asp:BoundField DataField="CityName" HeaderText="City" />
                <asp:BoundField DataField="LocName" HeaderText="Location" />
                <asp:BoundField DataField="Grade" HeaderText="Scale" />
            </Columns>
            <HeaderStyle CssClass="grid_hdr" />
            <RowStyle CssClass="grid_row" />
            <AlternatingRowStyle CssClass="gridAlternateRow" />
            <SelectedRowStyle CssClass="gridSelectedRow" />
        </asp:GridView>

        <asp:Panel runat="server" ID="Panel1">

            <ajaxToolkit:CalendarExtender ID="txtDOECal" runat="server" TargetControlID="txtDOE"
                Enabled="True">
            </ajaxToolkit:CalendarExtender>
            <asp:TextBox ID="txtDOE" runat="server" MaxLength="11" Width="80px"></asp:TextBox>
            <br />
            <%-- <asp:Literal ID="Literal5" runat="server" Text="<%$ AppSettings: DateFullYearFormatPageText %>" />--%>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDOE"
                ErrorMessage="Please enter effective from date" SetFocusOnError="true" ValidationGroup="main2"
                Display="None"></asp:RequiredFieldValidator>

            <asp:DropDownList ID="ddlDesigTransfer" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                <asp:ListItem Value="0">Select Designation</asp:ListItem>
            </asp:DropDownList>


            <asp:DropDownList ID="ddlDeptTransfer" runat="server" AppendDataBoundItems="True"
                CssClass="form-control">
                <asp:ListItem Value="0">Select Department</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="ddlCityTransfer" runat="server" AutoPostBack="true" AppendDataBoundItems="True"
                CssClass="form-control" OnSelectedIndexChanged="ddlCityTransfer_SelectedIndexChanged">
                <asp:ListItem Value="0">Select City</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="ddlLocTransfer" runat="server" AppendDataBoundItems="True"
                CssClass="form-control">
                <asp:ListItem Value="0">Select Location</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="ddlRegionTransfer" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                <asp:ListItem Value="0">Select Region</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="ddlDivTransfer" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                <asp:ListItem Value="0">Select Division</asp:ListItem>
            </asp:DropDownList>

        </asp:Panel>

        <uc2:Buttons ID="ucButtonEmpTransfers" OnButtonClick="ButtonCommandTransfer" runat="server" />
        <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->

    </div>

    <script>
        $('#<%=ddlEmpDrpdwn.ClientID%>').chosen();
        $('#<%=ddlperson.ClientID%>').chosen();

                    <%--$('#<%=ddlEmpDrpdwn.ClientID%>').change(function () {
                        var vaal = $(this).val();
                        alert(vaal);
                        $('#<%=ddlperson.ClientID%>').val("0");
                    });


                    $('#<%=ddlperson.ClientID%>').change(function () {
                        //debugger
                        //alert("This is del");
                        $(".ddlEmpDrpdwnty").val("");
                    });--%>

</script>

</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpContactReport.aspx.cs" Inherits="RMS.report.EmpContactReport" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script src="../Scripts/jquery-ui-1.8.16.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" rel="stylesheet" />

    <script>
        $(document).ready(function () {
            var val1 = $(".opt").val();
            //$(".opt").val("0");
            $(".designation").hide();
            $(".section").hide();
            $(".scale").hide();
            $(".jobtype").hide();
            $(".domicile").hide();
            $(".age").hide();
            $(".Gender").hide();
            $(".Disability").hide();
            $(".Religion").hide();
            $(".Charge").hide();
            $(".Quota").hide();
            $(".police").hide();
            $(".appoited").hide();
            if (val1 == 0) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 1) {
                $(".designation").show();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 2) {
                $(".designation").hide();
                $(".section").show();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 3) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").show();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 4) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").show();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 5) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").show();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 6) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").show();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 7) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").show();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 8) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").show();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 9) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").show();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 10) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").show();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 11) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").show();
                $(".police").hide();
                $(".appoited").hide();
            }
            else if (val1 == 12) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").show();
                $(".appoited").hide();
            }
            else if (val1 == 13) {
                $(".designation").hide();
                $(".section").hide();
                $(".scale").hide();
                $(".jobtype").hide();
                $(".domicile").hide();
                $(".age").hide();
                $(".Gender").hide();
                $(".Disability").hide();
                $(".Religion").hide();
                $(".Charge").hide();
                $(".Quota").hide();
                $(".police").hide();
                $(".appoited").show();
            }
            $(".opt").change(function () {
                debugger
                var vall = $(this).val();
                //alert(vall);
                if (vall == 1) {
                    $(".designation").show();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 2) {
                    $(".designation").hide();
                    $(".section").show();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 3) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").show();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 4) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").show();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 5) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").show();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 6) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").show();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 7) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").show();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 8) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").show();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 9) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").show();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 10) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").show();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 11) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").show();
                    $(".police").hide();
                    $(".appoited").hide();
                }
                else if (vall == 12) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").show();
                    $(".appoited").hide();
                }
                else if (vall == 13) {
                    $(".designation").hide();
                    $(".section").hide();
                    $(".scale").hide();
                    $(".jobtype").hide();
                    $(".domicile").hide();
                    $(".age").hide();
                    $(".Gender").hide();
                    $(".Disability").hide();
                    $(".Religion").hide();
                    $(".Charge").hide();
                    $(".Quota").hide();
                    $(".police").hide();
                    $(".appoited").show();
                }
            });

        });

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <label>Divisions:</label>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange"
                                AppendDataBoundItems="True">
                            </asp:DropDownList>

                        </div>
                        <%--<div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Employees*</label>
                            <%--<asp:UpdatePanel ID="upnl" runat="server">
                                <ContentTemplate>
                            <asp:DropDownList ID="ddlEmployeeSearch" CssClass="form-control ddl" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                            </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="searchBranchDropDown" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>--%>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Select Option</label>
                            <asp:DropDownList ID="ddldropdown" CssClass="form-control opt" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="option_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="13">Designation</asp:ListItem>
                                <asp:ListItem Value="1">Post Held</asp:ListItem>
                                <asp:ListItem Value="2">Section</asp:ListItem>
                                <asp:ListItem Value="3">Scale</asp:ListItem>
                                <asp:ListItem Value="4">Job Type</asp:ListItem>
                                <asp:ListItem Value="5">Domicile</asp:ListItem>
                                <asp:ListItem Value="6">Age</asp:ListItem>
                                <asp:ListItem Value="7">Gender</asp:ListItem>
                                <asp:ListItem Value="8">Disability</asp:ListItem>
                                <asp:ListItem Value="9">Religion</asp:ListItem>
                                <asp:ListItem Value="10">Additional Charge</asp:ListItem>
                                <asp:ListItem Value="11">Quota</asp:ListItem>
                                <asp:ListItem Value="12">Police Verification</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 appoited">
                            <label>Designation:</label>
                            <asp:DropDownList ID="ddlappoited" runat="server" CssClass="form-control searchbranchchange"
                                AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 designation">
                            <label>Post Held:</label>
                            <asp:DropDownList ID="ddlselectoption" runat="server" CssClass="form-control searchbranchchange"
                                AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 section">
                            <label>Section:</label>
                            <asp:DropDownList ID="ddlsection" runat="server" CssClass="form-control searchbranchchange"
                                AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 scale">
                            <label>From Scale:</label>
                            <asp:DropDownList ID="ddlfromScale" runat="server" CssClass="form-control searchbranchchange"
                                AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 scale">
                            <label>To Scale:</label>
                            <asp:DropDownList ID="ddlToScale" runat="server" CssClass="form-control searchbranchchange"
                                AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 jobtype">
                            <label>Job Type:</label>
                            <asp:DropDownList ID="ddlJobtype" runat="server" CssClass="form-control searchbranchchange"
                                AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 domicile">
                            <label>Domicile</label>
                            <asp:DropDownList ID="ddlDomicile" runat="server" CssClass="form-control">
                                <asp:ListItem Selected="True" Value="0">Select City</asp:ListItem>
                                <asp:ListItem Value="1">All</asp:ListItem>
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
                        <div class="col-lg-3 col-md-3 col-sm-3 age">
                            <label>From Age</label>
                            <asp:DropDownList ID="ddlFromAge" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select Age</asp:ListItem>
                                <asp:ListItem Value="1">All</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                                <asp:ListItem Value="24">24</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="26">26</asp:ListItem>
                                <asp:ListItem Value="27">27</asp:ListItem>
                                <asp:ListItem Value="28">28</asp:ListItem>
                                <asp:ListItem Value="29">29</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                                <asp:ListItem Value="31">31</asp:ListItem>
                                <asp:ListItem Value="32">32</asp:ListItem>
                                <asp:ListItem Value="33">33</asp:ListItem>
                                <asp:ListItem Value="34">34</asp:ListItem>
                                <asp:ListItem Value="35">35</asp:ListItem>
                                <asp:ListItem Value="36">36</asp:ListItem>
                                <asp:ListItem Value="37">37</asp:ListItem>
                                <asp:ListItem Value="38">38</asp:ListItem>
                                <asp:ListItem Value="39">39</asp:ListItem>
                                <asp:ListItem Value="40">40</asp:ListItem>
                                <asp:ListItem Value="41">41</asp:ListItem>
                                <asp:ListItem Value="42">42</asp:ListItem>
                                <asp:ListItem Value="43">43</asp:ListItem>
                                <asp:ListItem Value="44">44</asp:ListItem>
                                <asp:ListItem Value="45">45</asp:ListItem>
                                <asp:ListItem Value="46">46</asp:ListItem>
                                <asp:ListItem Value="47">47</asp:ListItem>
                                <asp:ListItem Value="48">48</asp:ListItem>
                                <asp:ListItem Value="49">49</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="51">51</asp:ListItem>
                                <asp:ListItem Value="52">52</asp:ListItem>
                                <asp:ListItem Value="53">53</asp:ListItem>
                                <asp:ListItem Value="54">54</asp:ListItem>
                                <asp:ListItem Value="55">55</asp:ListItem>
                                <asp:ListItem Value="56">56</asp:ListItem>
                                <asp:ListItem Value="57">57</asp:ListItem>
                                <asp:ListItem Value="58">58</asp:ListItem>
                                <asp:ListItem Value="59">59</asp:ListItem>
                                <asp:ListItem Value="60">60</asp:ListItem>
                                <asp:ListItem Value="61">61</asp:ListItem>
                                <asp:ListItem Value="62">62</asp:ListItem>
                                <asp:ListItem Value="63">63</asp:ListItem>
                                <asp:ListItem Value="64">64</asp:ListItem>
                                <asp:ListItem Value="65">65</asp:ListItem>
                                <asp:ListItem Value="66">66</asp:ListItem>
                                <asp:ListItem Value="67">67</asp:ListItem>
                                <asp:ListItem Value="68">68</asp:ListItem>
                                <asp:ListItem Value="69">69</asp:ListItem>
                                <asp:ListItem Value="70">70</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 age">
                            <label>To Age</label>
                            <asp:DropDownList ID="ddlToAge" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select Age</asp:ListItem>
                                <asp:ListItem Value="1">All</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                                <asp:ListItem Value="24">24</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="26">26</asp:ListItem>
                                <asp:ListItem Value="27">27</asp:ListItem>
                                <asp:ListItem Value="28">28</asp:ListItem>
                                <asp:ListItem Value="29">29</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                                <asp:ListItem Value="31">31</asp:ListItem>
                                <asp:ListItem Value="32">32</asp:ListItem>
                                <asp:ListItem Value="33">33</asp:ListItem>
                                <asp:ListItem Value="34">34</asp:ListItem>
                                <asp:ListItem Value="35">35</asp:ListItem>
                                <asp:ListItem Value="36">36</asp:ListItem>
                                <asp:ListItem Value="37">37</asp:ListItem>
                                <asp:ListItem Value="38">38</asp:ListItem>
                                <asp:ListItem Value="39">39</asp:ListItem>
                                <asp:ListItem Value="40">40</asp:ListItem>
                                <asp:ListItem Value="41">41</asp:ListItem>
                                <asp:ListItem Value="42">42</asp:ListItem>
                                <asp:ListItem Value="43">43</asp:ListItem>
                                <asp:ListItem Value="44">44</asp:ListItem>
                                <asp:ListItem Value="45">45</asp:ListItem>
                                <asp:ListItem Value="46">46</asp:ListItem>
                                <asp:ListItem Value="47">47</asp:ListItem>
                                <asp:ListItem Value="48">48</asp:ListItem>
                                <asp:ListItem Value="49">49</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="51">51</asp:ListItem>
                                <asp:ListItem Value="52">52</asp:ListItem>
                                <asp:ListItem Value="53">53</asp:ListItem>
                                <asp:ListItem Value="54">54</asp:ListItem>
                                <asp:ListItem Value="55">55</asp:ListItem>
                                <asp:ListItem Value="56">56</asp:ListItem>
                                <asp:ListItem Value="57">57</asp:ListItem>
                                <asp:ListItem Value="58">58</asp:ListItem>
                                <asp:ListItem Value="59">59</asp:ListItem>
                                <asp:ListItem Value="60">60</asp:ListItem>
                                <asp:ListItem Value="61">61</asp:ListItem>
                                <asp:ListItem Value="62">62</asp:ListItem>
                                <asp:ListItem Value="63">63</asp:ListItem>
                                <asp:ListItem Value="64">64</asp:ListItem>
                                <asp:ListItem Value="65">65</asp:ListItem>
                                <asp:ListItem Value="66">66</asp:ListItem>
                                <asp:ListItem Value="67">67</asp:ListItem>
                                <asp:ListItem Value="68">68</asp:ListItem>
                                <asp:ListItem Value="69">69</asp:ListItem>
                                <asp:ListItem Value="70">70</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 Gender">
                            <label>Gender</label>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">All</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                                <asp:ListItem Value="F">Female</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 Disability">
                            <label>Disablity</label>
                            <asp:DropDownList ID="ddlDisablity" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">All</asp:ListItem>
                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                <asp:ListItem Value="No">No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 Religion">
                            <label>Relgion</label>
                            <asp:DropDownList ID="ddlReligion" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select Religion</asp:ListItem>
                                <asp:ListItem Value="1">All</asp:ListItem>
                                <asp:ListItem Value="M">Muslim</asp:ListItem>
                                <asp:ListItem Value="N">Non-Muslim</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 Charge">
                            <label>Addtion Charge</label>
                            <asp:DropDownList ID="ddlAddionReport" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">All</asp:ListItem>
                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                <asp:ListItem Value="No">No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 Quota">
                            <label>Quota</label>
                            <asp:DropDownList ID="ddlQuota" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Selected="True" Value="0">Select Quota</asp:ListItem>
                                <asp:ListItem Value="1">All</asp:ListItem>
                                <asp:ListItem Value="Open Merit">Open Merit</asp:ListItem>
                                <asp:ListItem Value="Minorties">Minorties</asp:ListItem>
                                <asp:ListItem Value="Woman">Woman</asp:ListItem>
                                <asp:ListItem Value="Govt. Employee">Govt. Employee</asp:ListItem>
                                <asp:ListItem Value="Disabled">Disabled</asp:ListItem>
                                <asp:ListItem Value="Rule-17A">Rule-17A</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 police">
                            <label>Police Verification</label>
                            <asp:DropDownList ID="ddlPoliceVerifi" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">All</asp:ListItem>
                                <asp:ListItem Value="true">Yes</asp:ListItem>
                                <asp:ListItem Value="false">No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Sort Order</label>
                            <asp:DropDownList ID="ddlSortOrder" CssClass="form-control opt" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">Designation</asp:ListItem>
                                <asp:ListItem Value="13">Post Held</asp:ListItem>
                                <asp:ListItem Value="2">Section</asp:ListItem>
                                <asp:ListItem Value="3">Scale</asp:ListItem>
                                <asp:ListItem Value="4">Job Type</asp:ListItem>
                                <asp:ListItem Value="5">Domicile</asp:ListItem>
                                <asp:ListItem Value="6">Age</asp:ListItem>
                                <asp:ListItem Value="7">Gender</asp:ListItem>
                                <asp:ListItem Value="8">Disability</asp:ListItem>
                                <asp:ListItem Value="9">Religion</asp:ListItem>
                                <asp:ListItem Value="10">Additional Charge</asp:ListItem>
                                <asp:ListItem Value="11">Quota</asp:ListItem>
                                <asp:ListItem Value="12">Police Verification</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Report Type</label>
                            <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">All</asp:ListItem>
                                <asp:ListItem Value="1">Serving Employee</asp:ListItem>
                                <asp:ListItem Value="2">Relieved Employee</asp:ListItem>

                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top: 30px">
                            <asp:Button CssClass="btn btn-primary" OnClick="Search_Click" Text="Report" runat="server" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top: 30px">
                            <asp:Button CssClass="btn btn-success" OnClick="Report_Click" Text="Save Excel File" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:Panel ID="Panel1" runat="server" Width="1000px" Height="600">
                                <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
                                </rsweb:ReportViewer>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        $('#<%=ddlselectoption.ClientID%>').chosen();
        $('#<%=ddlSortOrder.ClientID%>').chosen();
        $('#<%=ddlappoited.ClientID%>').chosen();
        $('#<%=searchBranchDropDown.ClientID%>').chosen();
        $('#<%=ddldropdown.ClientID%>').chosen();
        $('#<%=ddlselectoption.ClientID%>').chosen();
        $('#<%=ddlsection.ClientID%>').chosen();
        $('#<%=ddlfromScale.ClientID%>').chosen();
        $('#<%=ddlToScale.ClientID%>').chosen();
        $('#<%=ddlJobtype.ClientID%>').chosen();
        $('#<%=ddlDomicile.ClientID%>').chosen();
        $('#<%=ddlFromAge.ClientID%>').chosen();
        $('#<%=ddlToAge.ClientID%>').chosen();
    </script>

</asp:Content>

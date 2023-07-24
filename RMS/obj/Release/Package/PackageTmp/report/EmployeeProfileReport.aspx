<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmployeeProfileReport.aspx.cs" Inherits="RMS.report.EmployeeProfileReport" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript">
       
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            
                            <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label Text="Division" ID="Label2" runat="server" />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Designations</label>
                            <asp:DropDownList ID="ddlDesg" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Designation</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Sections</label>
                            <asp:DropDownList ID="ddlsection" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Section</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>From Scale</label>
                            <asp:DropDownList ID="ddlScale" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Scale</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>To Scale</label>
                            <asp:DropDownList ID="ddlTOScale" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Scale</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Job Type</label>
                            <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Job Type</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
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
                    </div>
                    <div class="row">
                        
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label Text="Gender" ID="Label1" runat="server" />
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                                <asp:ListItem Value="F">Female</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label Text="Disablity" ID="Label3" runat="server" />
                            <asp:DropDownList ID="ddlDisablity" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem  Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                <asp:ListItem Value="No">No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Relgion</label>
                            <asp:DropDownList ID="ddlReligion" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select Religion</asp:ListItem>
                                <asp:ListItem Value="M">Muslim</asp:ListItem>
                                <asp:ListItem Value="N">Non-Muslim</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Addtion Charge</label>
                            <asp:DropDownList ID="ddlAddionReport" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                <asp:ListItem Value="No">No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>From Age</label>
                            <asp:DropDownList ID="ddlFromAge" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select Age</asp:ListItem>
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
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>To Age</label>
                            <asp:DropDownList ID="ddlToAge" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select Age</asp:ListItem>
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
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Joining From Date</label>
                            <ajaxToolkit:CalendarExtender ID="txtJoinDateCal" runat="server" TargetControlID="txtJoinDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtJoinDate" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Joining To Date</label>
                            <ajaxToolkit:CalendarExtender ID="txtjointoCal" runat="server" TargetControlID="txtjointo" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtjointo" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Quota</label>
                            <asp:DropDownList ID="ddlQuota" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Selected="True" Value="0">Select Quota</asp:ListItem>
                                <asp:ListItem Value="Open Merit">Open Merit</asp:ListItem>
                                <asp:ListItem Value="Minorties">Minorties</asp:ListItem>
                                <asp:ListItem Value="Woman">Woman</asp:ListItem>
                                <asp:ListItem Value="Govt. Employee">Govt. Employee</asp:ListItem>
                                <asp:ListItem Value="Disabled">Disabled</asp:ListItem>
                                <asp:ListItem Value="Rule-17A">Rule-17A</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Police Verification</label>
                            <asp:DropDownList ID="ddlPoliceVerifi" runat="server" AppendDataBoundItems="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button Text="Reprot" runat="server" CssClass="btn btn-primary" OnClick="genrate_Click" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdEmps" runat="server" DataKeyNames="ID" CssClass="table table-responsive-sm" OnSelectedIndexChanged="grdEmps_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEmps_PageIndexChanging" OnRowDataBound="EmpRow_Click"
                                EmptyDataText="There is no employee defined" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                                    <asp:BoundField DataField="Des" HeaderText="Designation Code" />
                                    <asp:BoundField DataField="ScaleName" HeaderText="Scale" />
                                    <asp:BoundField DataField="JobeTypeName" HeaderText="Job Type" />
                                    <asp:BoundField DataField="Age" HeaderText="Age" />
                                    <asp:BoundField DataField="brName" HeaderText="Palce of Posting" />
                                    <asp:BoundField DataField="JoinDate" HeaderText="Joining Date" />
                                    <%--<asp:TemplateField HeaderText="Cost Center">
                                        <ItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddlProfile" AppendDataBoundItems="true" CssClass="form-control form-control-sm">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Qualificaton" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Prior Experience"  Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Tenure Experience"  Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Acr Record"  Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Enquiry/Disciplinary Proceeding"  Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Litigations/Court Cases"  Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Permotion/Time Scale/Upgration"  Value="7"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ControlStyle />
                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="220px" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrint" runat="server"  Text="Print" ToolTip="Print Employee Profile" CommandArgument='<%#Eval("ID")%>' OnClick="lnkPrint_Click" CssClass="lnk">
                                            </asp:LinkButton>
                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <%--<div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
                                <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
                                </rsweb:ReportViewer>
                            </asp:Panel>
                        </div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

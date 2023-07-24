<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtView.aspx.cs" Inherits="RMS.Profile.EmpMgtView" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="3%"></td>
            <td>
                 
                <table cellspacing="2" cellpadding="2" align="center" border="0" width="98%">
                  <tr>
                    <td colspan="4" valign="top"></td>
                  </tr>
                  <tr>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label12" runat="server" Text="Emp No:" ></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblEmpNoView" runat="server" Text="Emp No:" ></asp:Label>
                    </td>
                    <td rowspan="4"></td>
                    
                    <td rowspan="4" align="right">
                   
                    <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                    <td width="110" align="right">
                        <asp:Image runat="server" ID="imgEmp" ImageUrl="../empix/noimage.jpg" Width="110" Height="110" />
                    </td>
                    <td width="70" align="left">
                      </td>
                     </tr>
                     <tr>
                        <td colspan="2">
                            <table><tr><td>
                            
                            </td></tr><tr><td>
                            </td></tr></table>
                        </td>
                     </tr>
                     </table>
                    
                    </td>   
                  </tr>
                  <tr>
                    <td class="LblBgSetup">
                        <asp:Label ID="lblFullName" runat="server" Text="Name:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblFullNameView" runat="server" Text="Name:" ></asp:Label>
                        
                        </td>
                 </tr>
                 <tr valign="top">
                    <td class="LblBgSetup">
                        <asp:Label ID="Label29" runat="server" Text="Current Address:"></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblCurrentAddressView" runat="server" Text="Current Address:"></asp:Label>
                        
                        </td>
                 </tr>
                 <tr valign="top">
                    <td class="LblBgSetup" rowspan="3">
                        <asp:Label ID="Label5" runat="server" Text="Permanent Address:"></asp:Label>
                    </td>
                    <td rowspan="3">
                    <asp:Label ID="lblPermanentAddressView" runat="server" Text="Permanent Address:"></asp:Label>
                        
                        </td>
                 </tr>
                 <tr>
                     <td class="LblBgSetup">
                        <asp:Label ID="lblGender" runat="server" Text="Gender:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblGenderView" runat="server" Text="Gender:" ></asp:Label>
                        
                        </td>
                  </tr>
                  <tr valign="top">
                    <td class="LblBgSetup">
                        <asp:Label ID="lblMar" runat="server" Text="Marital Status:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblMarView" runat="server" Text="Marital Status:" ></asp:Label>
                        
                        </td>
                    
                </tr>
                 <tr>
                   <td class="LblBgSetup">
                        <asp:Label ID="Label6" runat="server" Text="Father Name:" ></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFatherNameView" runat="server" Text="Father Name:" ></asp:Label></td>
                   
                    <td class="LblBgSetup">
                        <asp:Label ID="Lable15" runat="server" Text="Mother Name:"></asp:Label>
                    </td>
                   <td>
                        <asp:Label ID="lblMotherNameView" runat="server" Text="Mother Name:"></asp:Label>
                    
                    </td> 
                </tr>
                <tr>
                   <td class="LblBgSetup">
                        <asp:Label ID="lblEmail" runat="server" Text="Email:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblEmailView" runat="server" Text="Email:" ></asp:Label>
                        
                        </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="lblEdu" runat="server" Text="Education:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblEduView" runat="server" Text="Education:" ></asp:Label>
                        
                        </td>
                </tr>   
                <tr>
                 
                     <td class="LblBgSetup">
                        <asp:Label ID="Label3" runat="server" Text="Phone No:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblPhNoView" runat="server" Text="Phone No:" ></asp:Label>
                        
                        </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label2" runat="server" Text="Mobile No:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblMobileNoView" runat="server" Text="Mobile No:" ></asp:Label>
                        
                        </td>
                </tr>
                 <tr>
                     <td class="LblBgSetup">
                        <asp:Label ID="Label7" runat="server" Text="CNIC:" ></asp:Label>
                    </td>
                    <td>
                     <asp:Label ID="lblCnicView" runat="server" Text="CNIC:" ></asp:Label>
                       
                       </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label16" runat="server" Text="Issue Date:"></asp:Label>
                    </td>
                    <td>
                      <asp:Label ID="lblIssueDateView" runat="server" Text="Issue Date:"></asp:Label>
                      
                      </td>
                </tr>
                <tr>
                   <td class="LblBgSetup">
                       <asp:Label ID="Label17" runat="server" Text="Expiry Date:"></asp:Label>
                   </td>
                      <td>
                    <asp:Label ID="lblExpiryDateView" runat="server" Text="Expiry Date:"></asp:Label>
                      
                      </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="lblOpenDte" runat="server" Text="Birth Date:"></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblBirthDateView" runat="server" Text="Birth Date:"></asp:Label>
                        
                        </td>
               </tr>
               <tr>
                   <td class="LblBgSetup">
                        <asp:Label ID="Label13" runat="server" Text="Joining Date:"></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblJoiningDateView" runat="server" Text="Joining Date:"></asp:Label>
                        
                        </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label14" runat="server" Text="Confirm Date:"></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblCinfirmDateView" runat="server" Text="Confirm Date:"></asp:Label>
                        
                        </td>
                </tr>
                <tr>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label8" runat="server" Text="NTN:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblNtnView" runat="server" Text="NTN:" ></asp:Label>
                    
                        </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label28" runat="server" Text="SCSI No:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblScsiNoView" runat="server" Text="SCSI No:" ></asp:Label>
                    
                        </td>
                    
                </tr>
                <tr>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label21" runat="server" Text="Scale:"></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblScaleView" runat="server" Text="Scale:"></asp:Label>
                        
                        
                    </td>
                
                    <td class="LblBgSetup">
                        <asp:Label ID="lblDept" runat="server" Text="Department:"></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblDepartmentView" runat="server" Text="Department:"></asp:Label>
                        
                    </td>
                </tr>
                <tr>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label11" runat="server" Text="Designation:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblDesignationView" runat="server" Text="Designation:" ></asp:Label>
                        
                    </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label9" runat="server" Text="Division:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblDivisionView" runat="server" Text="Division:" ></asp:Label>
                        
                        </td>
              </tr>
              <tr>
                    <td class="LblBgSetup">
                        <asp:Label ID="lblGroupName" runat="server" Text="Region:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblRegionView" runat="server" Text="Region:" ></asp:Label>
                        
                        </td>
                    <td class="LblBgSetup">
                    <asp:Label ID="Label23" runat="server" Text="Section:" ></asp:Label>
                  </td>
                  <td>
                  <asp:Label ID="lblSection" runat="server" Text="Section:" ></asp:Label>
                    
                    
                    </td>
                    
                </tr>
                <tr>
                
                    <td class="LblBgSetup">
                        <asp:Label ID="lblCity" runat="server" Text="City:"></asp:Label>
                    </td>
                    <td>
                        
                    <asp:Label ID="lblCityView" runat="server" Text="City:"></asp:Label>
                        
                        </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label10" runat="server" Text="Location:" ></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblLocationView" runat="server" Text="Location:" ></asp:Label>
                                           
                    </td>
             </tr>
             <tr>
                <td class="LblBgSetup">
                       <asp:Label ID="lblChildren" runat="server" Text="Son:" ></asp:Label>
                        
                </td>
                <td>
                    <asp:Label ID="lblSonView" runat="server" Text="Son:" ></asp:Label>
                            
                 </td>
                 <td class="LblBgSetup">
                       <asp:Label ID="Label1" runat="server" Text="Daughter:" ></asp:Label>
                        
                </td>
                <td>
                    <asp:Label ID="lblDaughterView" runat="server" Text="Daughter:" ></asp:Label>
                           
                 </td>
               </tr>
               <tr>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label24" runat="server" Text="Bank Name:" ></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblBankNameView" runat="server" Text="Bank Name:" ></asp:Label>
                        
                        </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label25" runat="server" Text="Branch Name:" ></asp:Label>
                    </td>
                    <td>
                        
                    <asp:Label ID="lblBranchNameView" runat="server" Text="Branch Name:" ></asp:Label>
                        
                        </td>
               </tr>
               <tr>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label26" runat="server" Text="Account No:"></asp:Label>
                    </td>
                    <td>
                    <asp:Label ID="lblAccountNoView" runat="server" Text="Account No:"></asp:Label>
                        
                        
                    </td>
                    <td class="LblBgSetup">
                        <asp:Label ID="Label27" runat="server" Text="EOBI No:" ></asp:Label>
                    </td>
                    <td>
                        
                    <asp:Label ID="lblEobiNoView" runat="server" Text="EOBI No:" ></asp:Label>
                        
                        </td>
                </tr>
                
                <tr>
                <td colspan="4">
                   
                </tr>
                </table>
                 
             </td>
             <td width="3%"></td>
            </tr>
        </table>
        
</asp:Content>

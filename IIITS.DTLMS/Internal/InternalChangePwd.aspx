<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="InternalChangePwd.aspx.cs" Inherits="IIITS.DTLMS.Internal.InternalChangePwd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script  type="text/javascript">
     
     function ValidateMyForm() {
         if (document.getElementById('<%= txtOldPwd.ClientID %>').value.trim() == "") {
             alert('Enter  Old Password')
             document.getElementById('<%= txtOldPwd.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtNewPwd.ClientID %>').value.trim() == "") {
             alert('Enter New Password')
             document.getElementById('<%= txtNewPwd.ClientID %>').focus()
             return false
         }


         if (document.getElementById('<%= txtConfirmPwd.ClientID %>').value.trim() == "") {
             alert('Enter Confirm password')
             document.getElementById('<%= txtConfirmPwd.ClientID %>').focus()
             return false
         }

     }
     function ResetForm() {

         document.getElementById('<%= txtOldPwd.ClientID %>').value = "";
         document.getElementById('<%= txtNewPwd.ClientID %>').value = "";
         document.getElementById('<%= txtConfirmPwd.ClientID %>').value = "";
         
         return false
     }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Change Password
                   </h3>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>
                               </div>
                           </form>
                       </li>
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->
               </div>


            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" > 
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Change Password</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                        <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                    <div class="span1"></div>
                                        <div class="span5">
                                                        <div class="control-group">
                        
                           <label class="control-label">Old Password <span class="Mandotary"> *</span></label>
                    
                        <div class="controls">
                            <div class="input-append">
                                                       
                               <asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password" CssClass="input-text"
        MaxLength="30" TabIndex="1"></asp:TextBox>

    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ControlToValidate="txtOldPwd" ErrorMessage="Enter old password" 
        ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>
                                
                                               

                            </div>
                        </div>
                    </div>

              

                                                        <div class="control-group">
                        <label class="control-label">New Password <span class="Mandotary"> *</span></label>
                  
                        <div class="controls">
                            <div class="input-append">
                                                       
                             <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password" 
        CssClass="input-text" MaxLength="30" TabIndex="2"></asp:TextBox>

    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ControlToValidate="txtNewPwd" ErrorMessage="Enter new password" 
        ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>

                            </div>
                        </div>
                    </div>



                                                        <div class="control-group">
                        <label class="control-label">Confirm Password <span class="Mandotary"> *</span></label>
                       
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password" 
       CssClass="input-text" MaxLength="30" TabIndex="3" ></asp:TextBox>
      
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
        ControlToValidate="txtConfirmPwd" ErrorMessage="Enter confirm password" 
        ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>
                                                       
                            </div>
                        </div>
                    </div>
                                   </div>
                          
                                            </div>
                                  
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span1">
                           <asp:Button ID="btnsubmit" runat="server" Text="Submit"  CssClass="btn btn-primary"
                          ValidationGroup="reg"   Height="30px" Width="80px"   OnClientClick="javascript:return ValidateMyForm();"
                          onclick="btnsubmit_Click" TabIndex="4"/>

                                         </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span1">  
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset"   OnClientClick="javascript:return ResetForm();"
                                             CssClass="btn btn-primary"  /><br />
                                    </div>
                                                <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                                    </div>
                                    </div>
                                </div>
                                
                                <div class="space20"></div>
                               <!-- END FORM-->                           
                                              

    </div>
    </div>
    </div>
    </div>
    </div>

</asp:Content>

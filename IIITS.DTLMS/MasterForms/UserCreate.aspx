<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="UserCreate.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.UserCreate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="../Scripts/functions.js" type="text/javascript"></script>
 <script type="text/javascript">
             function ValidateMyForm() {
                
                 if (document.getElementById('<%= txtFullName.ClientID %>').value.trim() == "") {
                     alert('Enter Full Name')
                     document.getElementById('<%= txtFullName.ClientID %>').focus()
                     return false
                 }


                 if (document.getElementById('<%= txtLoginName.ClientID %>').value.trim() == "") {
                     alert('Enter Login Name')
                     document.getElementById('<%= txtLoginName.ClientID %>').focus()
                     return false
                 }
                 if (document.getElementById('<%= txtOfficeName.ClientID %>').value.trim() == "") {
                     alert('Select Valid Office Code')
                     document.getElementById('<%= txtOffCode.ClientID %>').focus()
                     return false
                 }
                 if (document.getElementById('<%= txtEmailId.ClientID %>').value.trim() == "") {
                     alert('Enter EmailId')
                     document.getElementById('<%= txtEmailId.ClientID %>').focus()
                     return false
                 }
                 if (document.getElementById('<%= txtMobile.ClientID %>').value.trim() == "") {
                     alert('Enter MobileNo.')
                     document.getElementById('<%= txtMobile.ClientID %>').focus()
                     return false
                 }
//                 if (document.getElementById('<%= txtPhone.ClientID %>').value == "") {
//                     alert('Enter PhoneNo.')
//                     document.getElementById('<%= txtPhone.ClientID %>').focus()
//                     return false
//                 }

                 if (document.getElementById('<%= cmbRole.ClientID %>').value == "-Select-") {
                     alert('Select Role.')
                     document.getElementById('<%= cmbRole.ClientID %>').focus()
                     return false
                 }
                 if (document.getElementById('<%= cmbDesignation.ClientID %>').value == "-Select-") {
                     alert('Select Designation')
                     document.getElementById('<%= cmbDesignation.ClientID %>').focus()
                     return false
                 }

                 if (document.getElementById('<%= txtPassword.ClientID %>').value.trim() == "") {
                     alert('Enter Password.')
                     document.getElementById('<%=txtPassword.ClientID %>').focus()
                     return false
                 }

                 if (document.getElementById('<%= txtPassword.ClientID %>').value.trim() != "") {
                     var pass = document.getElementById('<%= txtPassword.ClientID %>').value
                     
                     // /^(?=.{8,})(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[#!*()$%^&+-={}@@]).*$/
                     if (!pass.match( /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,12}$/)) {
                         alert("Password Length Should 8 Character and  contains at least 1 Capital Letter or 1 Small Letter,1 Digit, 1 Special Character")
                         document.getElementById('<%=txtPassword.ClientID %>').focus()
                 return false;
             }

         }

                 if (document.getElementById('<%= txtAddress.ClientID %>').value.trim() == "") {
                     alert('Enter Address.')
                     document.getElementById('<%=txtAddress.ClientID %>').focus()
                     return false
                 }
             }
     // Only NUmbers to enter
     function onlyNumbers(event) {
         var charCode = (event.which) ? event.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57))
             return false;

         return true;
     }

     // Only Characters to enter
     function character(evt) {
         evt = (evt) ? evt : event;
         var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
         ((evt.which) ? evt.which : 0));
         if (charCode > 31 && (charCode < 65 || charCode > 90) &&
         (charCode < 97 || charCode > 122)) {

             return false;
         }
         return true;
     }

     //Charactes with Numbers and ._/ and space
     function characterAndspecialAndNum(event) {
        var evt = (evt) ? evt : event;
         var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
         ((evt.which) ? evt.which : 0));
         if ((charCode < 65 || charCode > 90) &&
          (charCode < 97 || charCode > 122) && (charCode < 48 || charCode > 57) && charCode != 32 && charCode != 46 && charCode != 47 && charCode != 95)
          {

             return false;
         }
         return true;
     }

       // validation for address field which desnt allow some selected fields   .
     function DontAllowPlusAndSingleQuotes(event) {
         debugger;
         var charCode = (event.which) ? event.which : event.keyCode
         if ((charCode >= 91 && charCode <= 94) || (charCode >= 60 && charCode <= 64) || (charCode >= 36 && charCode <= 37) || (charCode >= 123 && charCode <= 126) || ( charCode == 33 || charCode == 47 || charCode == 92 || charCode == 96 || charCode == 43 ||  charCode== 39))
             return false;
         else
             return true;
     }

     //Remove Special charactes except ._/ and space
     function cleanSpecial(t) {
       
         t.value = t.value.toString().replace(/[^a-zA-Z 0-9 _ ./\t\n\r]+/g, '');
         
        
     }

     //Remove Special charactes and Charactes
     function cleanCharAndSpecial(t)
     {
         t.value = t.value.toString().replace(/[^0-9]+/g, '');
     }
     // Not allow to paste
     //function nopaste() {
     //    //alert("pasting is not allowed!");
     //    return false;
     //}
    
   

     </script>
    <%--<script type="text/javascript">

        $(document).ready(function () {


            $("#ContentPlaceHolder1_txtLoginName").bind('paste', function () {
                setTimeout(function () {
                   
                    var data = $('#ContentPlaceHolder1_txtLoginName').val();
                    
                    var dataFull = data.replace(/[^a-zA-Z\n\r]+/g, '');
                 
                    $('#ContentPlaceHolder1_txtLoginName').val(dataFull);
                });

            });
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Create User                                    
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
               <div style="float:right;margin-top:20px;margin-right:12px" >
                      <asp:Button ID="Button1" runat="server" Text="User View" 
                                      OnClientClick="javascript:window.location.href='UserView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>
                             
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Create User</h4>
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
                        <label class="control-label">Full Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                                                      
                                <asp:TextBox ID="txtFullName" runat="server" MaxLength="20" onkeypress="return DontAllowPlusAndSingleQuotes(event)"
                                   onpaste="return false" oncopy="return false" oncut="return false"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Login Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtLoginName" runat="server" onkeypress="return characterAndspecialAndNum(event)"  
                                        MaxLength="100" onchange = " return cleanSpecial(this)"  oncopy="return false" ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Office Code<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtOffCode"  runat="server" onkeypress="return onlyNumbers(event)" MaxLength="4"  onchange = "cleanCharAndSpecial(this)" oncopy="return false"></asp:TextBox>
                                  <asp:Button ID="btnSearch" runat="server" Text="S"  
                                       CssClass="btn btn-primary" onclick="btnSearch_Click" />
                               
                                 <asp:TextBox ID="txtuserID"  runat="server" Width="20px" Visible="false" ></asp:TextBox>
                                  <asp:TextBox ID="txtSignImagePath"  runat="server" Width="20px" Visible="false" ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Office Name</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtOfficeName" runat="server" MaxLength="100" ReadOnly="true"  onpaste="return false" oncopy="return false" ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                      <div class="control-group">
                        <label class="control-label">Email Id<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtEmailId" runat="server" MaxLength="50" onkeypress="javascript:return validateEmail(txtEmailId);" onpaste="return false" oncopy="return false" ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                     <div class="control-group">
                        <label class="control-label">Mobile<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtMobile" runat="server"  onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" onpaste="return false" oncopy="return false" ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                     <div class="control-group">
                        <label class="control-label">Phone</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="15" onkeypress="javascript:return OnlyNumberHyphen(this,event);" onpaste="return false" oncopy="return false" ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                                                       
            </div>
            <div class="span5">
                
  
                   <div class="control-group">
                        <label class="control-label">Role<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:DropDownList ID="cmbRole" runat="server" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>                            

                       <div class="control-group">
                        <label class="control-label">Designation<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:DropDownList ID="cmbDesignation" runat="server" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>                   

                <div  id ="divPassword" runat ="server">

                   <div class="control-group">
                        <label class="control-label">Password<span class="Mandotary"> *</span></label>
                          
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" TextMode="Password" onpaste="return false" oncopy="return false"  ></asp:TextBox>
                               
                               
                             
                                      

                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ControlToValidate="txtPassword" ErrorMessage="Enter new password" 
        ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>
                                                       
                            </div>
                        </div>
                    </div> 
                <div  style="padding-left:180px" class="space1">
                
                                <label style="font-size:small" >
            <span class="Mandotary">*Password should be greater than or equal to 8 digit (It should Contain at least 1 Capital Letter or 1 Small Letter,1 Digit, 1 Special Character)</span></label>   </div> 
                
                </div>
                                                 
                    <div class="control-group">
                        <label class="control-label">Address<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtAddress" runat="server" MaxLength="200" onkeyup="return ValidateTextlimit(this,200);"  onkeypress="return DontAllowPlusAndSingleQuotes(event)"  style = "resize:none" TextMode="MultiLine"  onpaste="return false"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>       

                     <div class="control-group" style="display:none">
                        <label class="control-label">Sign Copy</label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:FileUpload ID="fupSign" runat="server" AllowMultiple="False" />   
                            </div>
                        </div>
                    </div>

                     </div>
                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="text-center" align="center">

                                      
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" 
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-success" />

                                       <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                            onclick="cmdReset_Click"  CssClass="btn btn-danger" /><br/>
                                   
                                                <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                                    </div>
                                    </div>
                                </div>
                                
                                <div class="space20"></div>
                                <!-- END FORM-->

                           
                        
                            
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
         </div>
         
      </div>
</asp:Content>

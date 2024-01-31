<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="InternalUserCreate.aspx.cs" Inherits="IIITS.DTLMS.Internal.InternalUserCreate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script src="../Scripts/functions.js" type="text/javascript"></script>
 <script type="text/javascript">
     function ValidateMyForm() {
         if (document.getElementById('<%= txtFullName.ClientID %>').value.trim() == "") 
         {
             alert('Enter Full Name')
             document.getElementById('<%= txtFullName.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtDob.ClientID %>').value == "") {
             alert('Enter Date of birth')
             document.getElementById('<%= txtDob.ClientID %>').focus()
             return false
         }
        
         if (document.getElementById('<%= txtMobile.ClientID %>').value.trim() == "") {
             alert('Enter MobileNo.')
             document.getElementById('<%= txtMobile.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtDoj.ClientID %>').value == "") {
             alert('Enter Date of joining')
             document.getElementById('<%= txtDoj.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= cmbUserType.ClientID %>').value == "-Select-") {
             alert('Select User type.')
             document.getElementById('<%= cmbUserType.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtPassword.ClientID %>').value.trim() == "") {
             alert('Enter Password.')
             document.getElementById('<%=txtPassword.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtConPwd.ClientID %>').value.trim() == "") {
             alert('Enter confirm Password.')
             document.getElementById('<%=txtConPwd.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtAddress.ClientID %>').value.trim() == "") {
             alert('Enter Address.')
             document.getElementById('<%=txtAddress.ClientID %>').focus()
             return false
         }
     }

     </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                      User Create                                   
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
                      <asp:Button ID="btnClose" runat="server" Text="User View" 
                                      OnClientClick="javascript:window.location.href='InternalUserView.aspx'; return false;"
                            CssClass="btn btn-primary" onclick="btnClose_Click"   /></div>
                             
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" > 
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> User Create </h4>
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
                                <asp:TextBox ID="txtFullName" runat="server" MaxLength="50" 
                                    onkeypress="return AllowOnlysCharNotAllowSpecial(event);" TabIndex="1" ></asp:TextBox>
                            
                            </div>
                        </div>
                    </div>
                                                   <%--     <div class="control-group">
                        <label class="control-label">Login Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtLoginName" runat="server" onkeypress="return AllowOnlysCharNotAllowSpecial(event);"  
                                        MaxLength="100" ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>--%>
                    <div class="control-group">
                        <label class="control-label">Mobile<span class="Mandotary">*</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtMobile" runat="server"  
                                        onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" TabIndex="2" ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                     <div class="control-group">
                        <label class="control-label">Date Of Birth<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtDob" runat="server" MaxLength="10" TabIndex="3" ></asp:TextBox>
                                     <ajax:calendarextender ID="Calendarextender1" runat="server" 
                                        CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                  TargetControlID="txtDob"></ajax:calendarextender>                      
                            </div>
                        </div>
                    </div>


                    
                   <div class="control-group">
                        <label class="control-label">Create Login</label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:CheckBox ID="chkCreateLogin" runat="server" 
                                    oncheckedchanged="chkCreateLogin_CheckedChanged"  AutoPostBack="True" 
                                    TabIndex="7" />
                                                       
                            </div>
                        </div>
                    </div>   

                   <div class="control-group" >
                      <asp:Label  class="control-label" ID="lblPwd" runat="server" Visible="false" 
                            Text="Password"></asp:Label>
                        <div class="controls">
                            <div class="input-append" >
                                <asp:HiddenField ID="hdfSupervisor" runat="server" />
                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" TextMode="Password" visible="false"  ></asp:TextBox>
                                              
                            </div>
                        </div>
                                 
                    </div>


                     <div class="control-group" >
                      
                         <asp:Label  class="control-label" ID="lblConPwd" runat="server" Visible="false"  
                            Text="Confirm Password"></asp:Label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConPwd" runat="server" MaxLength="50" TextMode="Password" visible="false"   ></asp:TextBox>
                                     
                                    <asp:HiddenField ID="hdfPwd" runat="server" />                                    
                                     <asp:TextBox ID="txtuserID"  runat="server" Width="20px" visible="false" ></asp:TextBox>                  
                            </div>
                        </div>
                    </div>
                     
                                                       
            </div>
            <div class="span5">
 
                   <div class="control-group">
                        <label class="control-label">User Type<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:DropDownList ID="cmbUserType" runat="server"  AutoPostBack="true"
                                    onselectedindexchanged="cmbUserType_SelectedIndexChanged" TabIndex="4" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>     
                    <div class="control-group" runat="server" style="display:none" id="dvSupervisor">
                        <label class="control-label">Supervisor<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:DropDownList ID="cmbSupervisor" runat="server" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>                                                             
                   <div class="control-group">
                        <label class="control-label">Date of joining<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtDoj" runat="server"  MaxLength="10" TabIndex="5" ></asp:TextBox>
                                    <ajax:calendarextender ID="PurchaseCalender" runat="server" 
                                        CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                  TargetControlID="txtDoj"></ajax:calendarextender>                   
                            </div>
                        </div>
                    </div>

                                             
                    <div class="control-group">
                        <label class="control-label">Address<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtAddress" runat="server" MaxLength="250" 
                                        onkeyup="return ValidateTextlimit(this,250);" style = "resize:none" 
                                        TextMode="MultiLine" TabIndex="6"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>   
                    
                    </div>
               <div class="span5" >

                    </div>
                     
                         <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span1">
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" 
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" />
                                         </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span1">  <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                            onclick="cmdReset_Click"  CssClass="btn btn-primary" /><br/>
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
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
         </div>
         
      </div>

</asp:Content>

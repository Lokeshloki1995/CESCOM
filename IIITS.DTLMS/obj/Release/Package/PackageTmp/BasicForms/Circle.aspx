<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Circle.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.Circle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/functions.js" type="text/javascript"></script>
   
     <script  type="text/javascript">
         function ValidateMyForm() {
             if (document.getElementById('<%= txtCircleCode.ClientID %>').value.trim() == "0") {
                 alert('Circle Code Cannot be 0')
                 document.getElementById('<%= txtCircleCode.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtCircleCode.ClientID %>').value.trim() == "") {
                 alert('Please Enter Circle Code')
                 document.getElementById('<%= txtCircleCode.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtCircleName.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Circle Name')
                 document.getElementById('<%= txtCircleName.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtFullName.ClientID %>').value.trim() == "") {
                 alert('Please Enter Name Of Head')
                 document.getElementById('<%= txtFullName.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtMobile.ClientID %>').value.trim() == "") {
                 alert('Enter Mobile No')
                 document.getElementById('<%= txtMobile.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtEmailId.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Mail Id')
                 document.getElementById('<%= txtEmailId.ClientID %>').focus()
                 return false
             }
         }
         //&& charCode != 38-->&
         //Charactes and space 
         function characterAndSpace(event) {
             var evt = (evt) ? evt : event;
             var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
             ((evt.which) ? evt.which : 0));
             if ((charCode < 65 || charCode > 90) &&
              (charCode < 97 || charCode > 122) && charCode != 32 ) {

                 return false;
             }
             return true;
         }
         // Only Characters to enter
         //function character(evt) {
         //    evt = (evt) ? evt : event;
         //    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
         //    ((evt.which) ? evt.which : 0));
         //    if (charCode > 31 && (charCode < 65 || charCode > 90) &&
         //    (charCode < 97 || charCode > 122)) {

         //        return false;
         //    }
         //    return true;
         //}
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
            Create Circle
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
                   <%--   <asp:Button ID="cmdClose" runat="server" Text="Close" 
                                    CssClass="btn btn-primary" />--%></div>
                    <div style="float:right;margin-top:20px;margin-right:12px" >
                      <asp:Button ID="cmdClose" runat="server" Text="Circle View" 
                                      OnClientClick="javascript:window.location.href='CircleView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>          
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Create Circle</h4>
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
                        <label class="control-label">Circle Code <span class="Mandotary"> *</span></label>
                 
                        <div class="controls">
                            <div class="input-append">
                                <asp:TextBox ID="txtCircleCode" runat="server" onkeypress="javascript:return OnlyNumber(event);" MaxLength="1"></asp:TextBox>
                                <asp:TextBox ID="txtCrID" runat="server" Visible="false"  ></asp:TextBox>
                            </div>
                        </div>
                    </div>
     
                    <div class="control-group">
                        <label class="control-label">Circle Name <span class="Mandotary"> *</span></label>
                        
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtCircleName" runat="server"  MaxLength="30" onkeypress="return characterAndSpace(event)"
                                     onpaste="return false;"></asp:TextBox>
                            </div>
                        </div>
                    </div>


                        <div class="control-group">
                        <label class="control-label"> Name Of Head<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtFullName" runat="server" MaxLength="40" onkeypress="return characterAndSpace(event)" onpaste="return false;"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                        </div>


               <div class="span5">
   
                 

               <div class="control-group">
                        <label class="control-label">Mobile<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtMobile" runat="server"  onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" onpaste="return false;" ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>


                      <div class="control-group">
                        <label class="control-label">Phone</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="13" onkeypress="javascript:return OnlyNumberHyphen(this,event);" onpaste="return false;"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>


               <div class="control-group">
                        <label class="control-label">Email Id<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtEmailId" runat="server" MaxLength="50" onkeypress="javascript:return validateEmail(txtEmailId);" onpaste="return false;"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                
                                                       
            </div>



                                <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="text-center" align="center">
                                      
                                        
                                     
                                    <asp:Button ID="cmdSave" runat="server" Text="Save"  
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" onclick="cmdSave_Click" 
                                                />
                                   
                                   
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-danger" onclick="cmdReset_Click" /><br />
                                   
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



</asp:Content>

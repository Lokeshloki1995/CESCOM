<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StoreCreate.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.StoreCreate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/functions.js" type="text/javascript"></script>
   
     <script  type="text/javascript">
         function ValidateMyForm() {
             if (document.getElementById('<%= txtStoreCode.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Store Code')
                 document.getElementById('<%= txtStoreCode.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtStoreName.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Store Name')
                 document.getElementById('<%= txtStoreName.ClientID %>').focus()
                 return false
             }


             if (document.getElementById('<%= cmbDivision.ClientID %>').value == "-Select-") {
                 alert('Select Division Name')
                 document.getElementById('<%= cmbDivision.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtStoreDescription.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Description')
                 document.getElementById('<%= txtStoreDescription.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtInchargeName.ClientID %>').value.trim() == "") {
                 alert('Enter Store incharge Name')
                 document.getElementById('<%= txtInchargeName.ClientID %>').focus()
                 return false
             }

             if (document.getElementById('<%= txtEmailId.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Email Id')
                 document.getElementById('<%= txtEmailId.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtMobile.ClientID %>').value.trim() == "") {
                 alert('Enter Mobile No')
                 document.getElementById('<%= txtMobile.ClientID %>').focus()
                 return false
             }
             if (document.getElementById('<%= txtAddress.ClientID %>').value.trim() == "") {
                 alert('Enter Valid Address')
                 document.getElementById('<%= txtAddress.ClientID %>').focus()
                 return false
             }

         }

         //Charactes and space to create Store Name
         function characterAndspecialStore(event) {
             var evt = (evt) ? evt : event;
             var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
             ((evt.which) ? evt.which : 0));
             if ((charCode < 65 || charCode > 90) &&
              (charCode < 97 || charCode > 122) && charCode != 32) {

                 return false;
             }
             return true;
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
                  Create Store
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
                      <asp:Button ID="Button1" runat="server" Text="Store View" 
                                      OnClientClick="javascript:window.location.href='StoreView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>
                            
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Create Store</h4>
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
                        <label class="control-label">Store Code<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                             
                                <asp:TextBox ID="txtStoreId"  runat="server" MaxLength="10" Visible="false"></asp:TextBox>
                                 
                                 <asp:TextBox ID="txtStoreCode"  runat="server" MaxLength="2" 
                                 onkeypress="javascript:return OnlyNumber(event)" onpaste="return false;" ></asp:TextBox>
                            </div>
                        </div>
                    </div>
   
               

                    <div class="control-group">
                        <label class="control-label">Store Name<span class="Mandotary"> *</span></label>
                    
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtStoreName" runat="server" MaxLength="25" onkeypress="return characterAndspecialStore(event)"  onpaste="return false;"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                          <div class="control-group">
                        <label class="control-label">Division<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                  <asp:DropDownList ID="cmbDivision" runat="server" >
                                </asp:DropDownList>                      
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Store Description<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtStoreDescription" runat="server" onkeyup="return ValidateTextlimit(this,250);" style = "resize:none" TextMode="MultiLine" onpaste="return false;"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                                                        
                </div>
                                                            
                                                           <%-- another span--%>
                    <div class="span5">
                                 
                             <div class="control-group">
                        <label class="control-label">Incharge Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtInchargeName" runat="server" onkeypress="return characterAndspecialStore(event)" 
                                        MaxLength="100" onpaste="return false;"></asp:TextBox>
                                                       
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
                        <div class="control-group">
                        <label class="control-label">Mobile<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtMobile" runat="server"  onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" onpaste="return false;"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>
                          <div class="control-group">
                        <label class="control-label">Phone No</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="15" onkeypress="javascript:return OnlyNumberHyphen(this,event);" onpaste="return false;"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                     <div class="control-group">
                        <label class="control-label">Address<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtAddress" runat="server" onkeyup="return ValidateTextlimit(this,250);" style = "resize:none" TextMode="MultiLine" onpaste="return false;"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

             </div>
                <div class="span1"></div>
                    </div>
                    <div class="space20"></div>
                                        
                <div  class="text-center" align="center">

                  
                   
                    <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" 
                    OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" />
               
                  
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
         
      </div>

</asp:Content>

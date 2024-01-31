<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="VendorMaster.aspx.cs" Inherits="IIITS.DTLMS.Internal.VendorMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/functions.js" type="text/javascript"></script>
  <script  type="text/javascript">

      function ValidateMyForm() {
          if (document.getElementById('<%= txtFullName.ClientID %>').value.trim() == "") {
              alert('Enter Vendor Name')
              document.getElementById('<%= txtFullName.ClientID %>').focus()
              return false
          }

          if (document.getElementById('<%= txtMobile.ClientID %>').value.trim() == "") {
              alert('Enter Mobile Number')
              document.getElementById('<%= txtMobile.ClientID %>').focus()
              return false
          }
      }
      // Only Characters to create
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
      //Charactes and space ./ to search Vender Name
      function characterAndspecialVen(event) {
          var evt = (evt) ? evt : event;
          var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
          ((evt.which) ? evt.which : 0));
          if ((charCode < 65 || charCode > 90) &&
           (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 46 && charCode != 47) {

              return false;
          }
          return true;
      }
      //Remove Numbers, Special characters except space to search Vender Name
      function cleanSpecialAndNumVen(t) {

          t.value = t.value.toString().replace(/[^./a-zA-Z \t\n\r]+/g, '');


      }
       
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                      Vendor Create                                   
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
                      <%--<asp:Button ID="btnClose" runat="server" Text="User View" 
                                      OnClientClick="javascript:window.location.href='VendorMasterView.aspx'; return false;"
                            CssClass="btn btn-primary"  /></div>--%>
                             
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i> Vendor Create </h4>
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
                        <label class="control-label">Vendor Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                                                      
                                <asp:TextBox ID="txtFullName" runat="server" MaxLength="50" 
                                   TabIndex="1"
                                     onpaste="return false;"></asp:TextBox>
                                 <%--onkeypress="return AllowOnlysCharNotAllowSpecial(event);" 
                            --%>
                            </div>
                        </div>
                    </div>                                                  
                                                        
            </div>
            <div class="span5">
                <div class="control-group">
                        <label class="control-label">Mobile<span class="Mandotary">*</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtMobile" runat="server" onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" TabIndex="2" MinLength="10"  onpaste="return false;"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>     
                
                 <asp:Label ID="tblVmId" runat="server" Visible="false"></asp:Label>                
                    <%--<div class="control-group">
                        <label class="control-label">Remarks</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="250" 
                                        onkeyup="return ValidateTextlimit(this,250);" style = "resize:none" 
                                        TextMode="MultiLine" TabIndex="6"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>--%>   
                    
                    </div>
               <div class="span5" >

                    </div>
                     
                         <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="text-center" align="center">

                                    
                                    
                                        <asp:Button ID="cmdSave" runat="server" Text="Save"
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                                      
                                     <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br/>
                               
                                                <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                                    </div>
                                    </div>
                                </div>
                                
                                <div class="space20"></div>
                                <!-- END FORM-->

                           
                            <asp:GridView ID="grdVendorDetails" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnPageIndexChanging="grdVendorDetails_PageIndexChanging" OnRowCommand="grdVendorDetails_RowCommand" OnRowEditing="grdVendorDetails_RowEditing">
                                <Columns>
                                  
                                    <asp:TemplateField AccessibleHeaderText="ID" HeaderText="ID"  Visible="false">                                
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblVendorId" runat="server" Text='<%# Bind("VM_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="VENDOR NAME" HeaderText="VENDOR NAME" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblVmName" runat="server" Text='<%# Bind("VM_NAME") %>' style="word-break:break-all" ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                          <asp:TextBox ID="txtVmName" runat="server" placeholder="Enter VENDOR NAME " onkeypress="return characterAndspecialVen(event)"  
                                        onchange="return cleanSpecialAndNumVen(this)"></asp:TextBox>
                                        </FooterTemplate>
                                  </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="MOBNUM" HeaderText="MOBILE NUMBER" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblVmNumber" runat="server" Text='<%# Bind("VM_MOBILE_NUM") %>' style="word-break:break-all"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                       </FooterTemplate>
                                    </asp:TemplateField>                               
                                   
                                   <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  CommandName="edit" ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" 
                                                Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  CommandName="View" ID="imgView" runat="server" Height="12px" ImageUrl="~/img/Manual/view.png" 
                                                Width="12px" ToolTip="View Comments" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                
                                </Columns>

                            </asp:GridView>
                        
                            
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
         </div>
         
      </div>

</asp:Content>

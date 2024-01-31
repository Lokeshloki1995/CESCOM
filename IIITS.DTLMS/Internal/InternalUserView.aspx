<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="InternalUserView.aspx.cs" Inherits="IIITS.DTLMS.Internal.InternalUserView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">
      function ConfirmStatus(status) {

          var result = confirm('Are you sure,Do you want to ' + status + ' User?');
          if (result) {
              return true;
          }
          else {
              return false;
          }
      }

    
    </script>
     <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ajax:toolkitscriptmanager ID="ScriptManager1" runat="server">
    </ajax:toolkitscriptmanager>
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                      User View
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
                            <h4><i class="icon-reorder"></i> User View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                        
                                <div style="float:right" >
                                <div class="span2">
                                   <asp:Button ID="cmdNew" runat="server" Text="New User" 
                                              CssClass="btn btn-primary" onclick="cmdNew_Click" /><br /></div>

                                            </div>
                                  
                                  
                      

                                <div class="space20"></div>
                                <!-- END FORM-->
                           
                        
                            <asp:GridView ID="grdInternalUser" 
                                AutoGenerateColumns="false"  PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdInternalUser_PageIndexChanging" 
                                    onrowcommand="grdInternalUser_RowCommand" 
                                    ShowFooter="True">
                                <Columns>
                                         <asp:TemplateField AccessibleHeaderText="IU_ID" HeaderText="ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("IU_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="IU_FULLNAME" HeaderText="Full Name">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblFullName" runat="server" Text='<%# Bind("IU_FULLNAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                             <asp:TextBox ID="txtsFullName" runat="server"  Width="150px"  placeholder="Enter Name" ToolTip="Enter Name to Search"></asp:TextBox>
                                         </FooterTemplate>
                                    </asp:TemplateField>

       
                                    
                                    <asp:TemplateField AccessibleHeaderText="IU_MOBILENO" HeaderText="Mobile Number">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("IU_MOBILENO") %>' ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                             <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9"/>
                                         </FooterTemplate>   
                                    </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="IU_DOJ" HeaderText="Date Of joining" >
                                      <ItemTemplate>
                                            <asp:Label ID="lblDoj" runat="server" Text='<%# Bind("IU_DOJ") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                        
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="IU_USERTYPE" HeaderText="User Type">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserType" runat="server" Text='<%# Bind("IU_USERTYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create" 
                                                Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                CausesValidation="false" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </div>

                                </div>
                            

        <div>
           

                </div>


            
               
           
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                   <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
      </div>
 


    </div>
</asp:Content>

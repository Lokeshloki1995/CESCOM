<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StoreIndentView.aspx.cs" Inherits="IIITS.DTLMS.StoreTransfer.StoreIndentView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       

         .ascending th a {
        background:url(/img/sort_asc.png) no-repeat;
        display: block;
        padding: 0px 4px 0 20px;
    }

    .descending th a {
        background:url(/img/sort_desc.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
     .both th a {
         background: url(/img/sort_both.png) no-repeat;
         display: block;
          padding: 0 4px 0 20px;
     }
    </style>
      <script type="text/javascript">
     //Indent allow to enter nums 
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

// Indent allow only Numbers to paste
        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                  Indent View
                  
                   </h3>
                       <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i> Store Indent View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                                </span>
                        </div>
                        <div class="widget-body">
                       
                        <div class="row-fluid">
                             <div class="span2">
                              <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>
                              </div>
                          <div class="span3">
                            <asp:RadioButton ID="rdbPendingIndent" runat="server" Text="Pending for Invoice" CssClass="radio" 
                                  GroupName="a" Checked="true" AutoPostBack="true" oncheckedchanged="rdbPendingIndent_CheckedChanged"
                                   />
                          </div>
                           <div class="span4">
                              <asp:RadioButton ID="rdbAlreadyCompleted" runat="server"  Text="Completed Invoice for Indent" 
                                   CssClass="radio" GroupName="a"  AutoPostBack="true" oncheckedchanged="rdbAlreadyCompleted_CheckedChanged"
                                   />
                            </div>

                             <div style="float:right;">
                               <div style="float:right" >
                                   <%--<div class="span5">
                                   <asp:Button ID="cmdNew" runat="server" Text="New" 
                                              CssClass="btn btn-primary" onclick="cmdNew_Click" /><br /></div>--%>
                                   <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickStoreindent" /><br />
                                          </div>
                                 </div> 
                             </div>
                        </div>
                      
                                <div class="space20"></div>
                                <!-- END FORM-->
                                                  
                            <asp:GridView ID="grdIndentView" 
                                AutoGenerateColumns="false"  PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdIndentView_PageIndexChanging" 
                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" 
                                onrowcommand="grdIndentView_RowCommand" OnSorting="grdIndentView_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                          <asp:TemplateField AccessibleHeaderText="SI_ID" HeaderText="Indent Id" Visible=false>                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblIndentId" runat="server" Text='<%# Bind("SI_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
  
                                    <asp:TemplateField AccessibleHeaderText="SI_NO" HeaderText="Indent Number" SortExpression="SI_NO">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblSiNo" runat="server" Text='<%# Bind("SI_NO") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                         <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                            <asp:TextBox ID="txtIndentNo" runat="server" placeholder="Enter Indent No " Width="150px" MaxLength="50" onkeypress="return onlyNumbers(event)" onchange = "return cleanSpecialAndChar(this)" ></asp:TextBox>
                                       </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="SI_DATE" HeaderText="Indent Date">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblIndentDate" runat="server" Text='<%# Bind("SI_DATE") %>'></asp:Label>                                  
                                        </ItemTemplate>
                                        <FooterTemplate>
                                          <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="SO_QNTY" HeaderText="Requested No. of Transformers">                                    
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("SO_QNTY") %>'></asp:Label>
                                        </ItemTemplate>          
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="SI_TO_STORE" HeaderText="To Store" SortExpression="SI_TO_STORE">
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblToStore" runat="server" Text='<%# Bind("SI_TO_STORE") %>' style="word-break:break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" runat="server"  ImageUrl="../img/Manual/view.png"  
                                              CommandName="Submit"   />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                   <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
      </div>
      </div>

    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">
                        Help</h4>
                </div>
                <div class="modal-body">
                    <ul><li>
                   This Web Page Can Be Used To View Store Indent Details and To Create New Store Indent.</li>
                   <li>   Store Indent Can Be Filtered By Selecting <u>Pending for Invoice</u> and <u>Completed Invoice for Indent</u> Radio Button
                 </li><li>     New Store Indent Can Be Created By Clicking <u>New</u> Button
                      </li></ul>  

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

</asp:Content>

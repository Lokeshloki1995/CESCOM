<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StockStatus.aspx.cs" Inherits="IIITS.DTLMS.Transaction.StockStatus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .table td
    {
       text-align:center;
    }

    .table th
    {
       text-align:center;
    }

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
                     Stock Status
                   </h3>
                       <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="Text1" type="text">
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
                            <h4><i class="icon-reorder"></i>Stock Status</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                              
                      
                             
                                <div style="float:right" >
                                    <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickStockStatus" /><br />
                                          </div>
                              

                                            </div>
                                  
                                  <%--  <div class="space20"> </div>--%>
                                 
                                <!-- END FORM-->
                           
                        <div class="form-horizontal">
                                <div class="row-fluid">
                               <%-- <div class="span1"></div>--%>
                               <div class="span5">
                                   <div class="control-group">
                                <label class="control-label">Strore Name</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmdStore" runat="server"  TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="cmdStore_SelectedIndexChanged">
          
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>
                                   
                             </div>
                              <div class="span5">
                          <div class="control-group">
                                <label class="control-label">Capacity(in KVA)</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbCapacity" runat="server"  TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="cmbCapacity_SelectedIndexChanged">
                                                   
          
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>

                              </div>

                               </div>
                            </div>
                            <asp:GridView ID="grdStockStatus" 
                                AutoGenerateColumns="false"  
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="false" EmptyDataText="No Records Found"
                                runat="server" onpageindexchanging="grdStockStatus_PageIndexChanging" OnDataBound="grdStockStatus_DataBound1"
                               OnSorting="grdStockStatus_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>   
                                    <asp:BoundField DataField="SM_NAME" HeaderText="Store Name"  ItemStyle-BorderStyle="Solid" SortExpression="SM_NAME"/>
<%--                                    <asp:BoundField DataField="SM_OFF_CODE" HeaderText="Location" ItemStyle-BorderStyle="Solid" SortExpression="SM_OFF_CODE"/>--%>
                                    <asp:BoundField DataField="TC_CAPACITY" HeaderText="Capacity(in KVA)" ItemStyle-BorderStyle="Solid"/>
                                    <asp:BoundField DataField="TC_CODE" HeaderText="StockCount"  ItemStyle-BorderStyle="Solid"/>
                                  <%--<asp:TemplateField AccessibleHeaderText="SM_ID" HeaderText="ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblStoreId" runat="server" Text='<%# Bind("SM_ID") %>'></asp:Label>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                                    

                                    <asp:TemplateField AccessibleHeaderText="Store Name" HeaderText="Store Name">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblStoreName" runat="server" Text='<%# Bind("SM_NAME") %>'></asp:Label>
                                                
                                        </ItemTemplate>
                                          <FooterTemplate>
                                           <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch" >
                                             <asp:TextBox ID="txtstoreName" runat="server"  Width="150px"  placeholder="Enter store Name" ToolTip="Enter Name to Search"></asp:TextBox>
                                        </asp:Panel>
                                         </FooterTemplate>
                                    </asp:TemplateField>
                                   
                                  <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location">
                                       
                                        <ItemTemplate>

                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("SM_OFF_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                           <asp:Panel ID="panel2" runat="server" DefaultButton="btnSearch" >
                                           <asp:TextBox ID="txtLocation" runat="server"  Width="150px"  placeholder="Enter Location  Name" ToolTip="Enter Location Name to Search"></asp:TextBox>
                                           </asp:Panel>
                                           
                                         </FooterTemplate>   
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="Capacity" HeaderText="Capacity(in KVA)">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9"/>
                                    </FooterTemplate>  
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="StockCount" HeaderText="Stock Count">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblStockCount" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
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
                                </asp:TemplateField>--%>



                                     <%-- <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="CAPACITY">
                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblcapacity" runat="server" Style="text-align:center" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                                
                                        </ItemTemplate>
                                         
                                    </asp:TemplateField>
                                   
                                  <asp:TemplateField AccessibleHeaderText="STORENAME" HeaderText="STORENAME">
                                       
                                        <ItemTemplate>

                                            <asp:Label ID="lblStoreName" runat="server" Style="width:20px" Text='<%# Bind("STORENAME") %>'></asp:Label>
                                        </ItemTemplate>
                                           
                                    </asp:TemplateField>--%>

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
                   This Web Page Can Be Used To View Stock Status
                       
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

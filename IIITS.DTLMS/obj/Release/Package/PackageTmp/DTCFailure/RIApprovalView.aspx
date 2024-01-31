<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="RIApprovalView.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.RIApprovalView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    function ConfirmDecomm() {
        var sTypeValue = document.getElementById('<%= cmbType.ClientID %>');
        var selectedText = sTypeValue.options[sTypeValue.selectedIndex].innerHTML;
        var result = confirm('Are you sure,Do you want to Decommision for ' + selectedText + ' ?');
        if (result) {
            return true;
        }
        else {
            return false;
        }
    }

        //Charactes and space() - . / 0 1 2 3 4 7  to search DTC Name
        function characterAndspecialDtc(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 40 && charCode != 41 && charCode != 45 && charCode != 46 && charCode != 47 && (charCode < 48 || charCode > 55)) {

                return false;
            }
            return true;
        }
        //Remove Numbers, Special characters except space to search DTC Name
        function cleanSpecialAndNumDtc(t) {

            t.value = t.value.toString().replace(/[^-./()a-zA-Z0-7\t\n\r]+/g, '');


        }
</script>
     <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
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
        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                    RI Acknowledgement View
                   </h3>
                       <a style="float:right!important;margin-left:-372px!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> RI Acknowledgement</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>

                         <div class="widget-body">
                         <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div style="float:right" >
                            <div class="span1">
                         <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                        OnClick="Export_ClickRI" /><br />
                                       </div>
                                            </div>
                       <%-- <div style="float:left" >--%>
                              <%--  <div class="span8">--%>

                               <div class="span4">
                                <asp:Label ID="lblType" runat="server" Text="Type" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>

                                    &nbsp;&nbsp;&nbsp;&nbsp;

                                    <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="true" 
                                       onselectedindexchanged="cmbType_SelectedIndexChanged" >  
                                               
                                         <asp:ListItem Text="Failure" Value="1" Selected="True"></asp:ListItem>
                                         <asp:ListItem Text="Failure With Enhancement" Value="4"></asp:ListItem>
                                          <asp:ListItem Text="Enhancement" Value="2"></asp:ListItem>
                                    </asp:DropDownList>   
                               </div>
                                
                      </div>
                        </div>
                        </div>
                          
                        <div class="widget-body">
                          <div class="form-horizontal">
                              <div class="row-fluid">
                                     <asp:Label ID="lblGridType" runat="server"  Font-Bold="true" ForeColor="#4A8BC2"
                                        Font-Size="Medium"></asp:Label>
                               </div>
                           </div>
                       </div>


                      

                    <asp:GridView ID="grdReplacementDetails" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"   
                      CssClass="table table-striped table-bordered table-advance table-hover"  
                      runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"  ShowFooter="true"
                            onrowcommand="grdReplacementDetails_RowCommand" onpageindexchanging="grdReplacementDetails_PageIndexChanging" 
                        OnSorting="grdReplacementDetails_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" /> 
                    <Columns>
                      <asp:TemplateField AccessibleHeaderText="TR_ID" HeaderText="Id" Visible="false">                            
                            <ItemTemplate>                                                
                                <asp:Label ID="lblReplaceId" runat="server" Text='<%# Bind("TR_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
               

                        <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="DTC Name" SortExpression="DT_NAME">                          
                            <ItemTemplate>
                                <asp:Label ID="lblDtname" runat="server" Text='<%# Bind("DT_NAME") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                 <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtDtcName" runat="server" placeholder="Enter DTC Name" CssClass="span12" onkeypress="return characterAndspecialDtc(event)"  
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                 </asp:Panel>
                                        </FooterTemplate>
                        </asp:TemplateField>
      
                        <asp:TemplateField AccessibleHeaderText="DT_TC_ID" HeaderText="DTR Code">                           
                            <ItemTemplate>
                                <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("DT_TC_ID") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                           <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure Id" Visible="false" SortExpression="DF_ID">                          
                            <ItemTemplate>
                                <asp:Label ID="lblFailureId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                          <%-- Both Columns are same but adding for User Interface Purpose--%>
                        <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Enhancement ID" Visible="false" SortExpression="DF_ID">                           
                            <ItemTemplate> 
                                <asp:Label ID="lblEnhanceId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="TI_INDENT_NO" HeaderText="Indent No" SortExpression="TI_INDENT_NO">                           
                            <ItemTemplate>
                                <asp:Label ID="lblIndentNo" runat="server" Text='<%# Bind("TI_INDENT_NO") %>'></asp:Label>
                            </ItemTemplate>
                            
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="TI_INDENT_NO" HeaderText="Invoice No" SortExpression="IN_INV_NO">                         
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("IN_INV_NO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                       

                         <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <center>
                             <%--   <asp:LinkButton runat="server"  CommandName="Create" ID="lnkCreate" >
                                        <img src="../Styles/images/Create.png" style="width:20px" />Approve</asp:LinkButton>--%>
                                   <asp:LinkButton runat="server"  CommandName="Create" ID="lnkCreate"   >
                                        <img src="../img/Manual/view.png" style="width:20px" />View</asp:LinkButton>
                                            
                                </center>
                            </ItemTemplate>
                             <HeaderTemplate>
                                <center>
                                    <asp:Label ID="lblHeader" runat="server" Text="Action" ></asp:Label>
                                </center>
                             </HeaderTemplate>
                       </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>

        
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>




         </div>
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
                   This Web Page Can Be Used To View RI Acknowledgement Details of Failure, Failure with Enhancement and Enhancement.</li>
                    <li>   To View RI Acknowledgement Details for Particular Type Select From Type DropDownList</li>
                      <li> To Get More Details About RI Acknowledgement click on View LinkButton 
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

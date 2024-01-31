<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="WorkOrderView.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.WorkOrderView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
    function ConfirmWO() {
        var sTypeValue = document.getElementById('<%= cmbType.ClientID %>');
        var selectedText = sTypeValue.options[sTypeValue.selectedIndex].innerHTML;
        var result = confirm('Are you sure,Do you want to declare Work Order for ' + selectedText + ' ?');
        if (result) {
            return true;
        }
        else {
            return false;
        }
    }
        //DTC allow to search chars and nums for 6
        function characterAndnumbers(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode > 31 && (charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }

        // Dtc allow Chatractes and Numbers to paste
        function cleanSpecialChars(t) {
            debugger;
            t.value = t.value.toString().replace(/[^a-zA-Z 0-9\n\r]+/g, '');
            //alert(" Special charactes are not allowed!");
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
        //DTR allow to enter nums for 6 digits
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        // DTR allow only Numbers to paste
        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
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
                       Work Order View
                   </h3>
                       <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i>ddd </button>
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
                            <h4><i class="icon-reorder"></i>Work Order View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                         <div class="widget-body">
                         <div class="form-horizontal">
                                    <div class="row-fluid">
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
                                          <asp:ListItem Text="New DTC" Value="3"></asp:ListItem>
                                    </asp:DropDownList>   
                               </div>

                                        <div style="float:right;">
                              
                                   <div class="span1">
                         <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                        OnClick="Export_ClickWorkorder" /><br />
                                       </div>
                         </div>

                           <div class="span2">
                              <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" Visible="false"
                                        Font-Size="Medium"></asp:Label>
                           </div>

                          <div class="span1">
                                <asp:RadioButton ID="rdbViewAll" runat="server" Text="View All" CssClass="radio" 
                                  GroupName="a"   AutoPostBack="true" style="display:none"
                                  oncheckedchanged="rdbViewAll_CheckedChanged" />
                          </div>

                           <div class="span4">
                                <asp:RadioButton ID="rdbAlready" runat="server"  Text="Already Created" 
                                   CssClass="radio" GroupName="a"  AutoPostBack="true" Checked="true" style="display:none"
                                   oncheckedchanged="rdbAlready_CheckedChanged" />
                            </div>

                             <div style="float:right;">
                                 <asp:Button ID="cmdNew" runat="server" Text="New" OnClientClick="return ConfirmWO();"
                                       CssClass="btn btn-primary" onclick="cmdNew_Click" Visible="false" />
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
                        
                            <asp:GridView ID="grdWorkOrder" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                                  CssClass="table table-striped table-bordered table-advance table-hover" ShowFooter="true"
                                runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                       onrowcommand="grdWorkOrder_RowCommand" 
                            onpageindexchanging="grdWorkOrder_PageIndexChanging" 
                            onrowdatabound="grdWorkOrder_RowDataBound" OnSorting="grdWorkOrder_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" /> 
    
                                <Columns>


                                 <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure ID" Visible="false">                           
                                        <ItemTemplate> 
                                           <asp:Label ID="lblFailureId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                       <FooterTemplate>
                                       <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                          <asp:TextBox ID="txtFailureId" runat="server" placeholder="Enter Failure Id " Width="150px" MaxLength="10" ></asp:TextBox>
                                       </asp:Panel>
                                       </FooterTemplate>
                                  </asp:TemplateField>

                                 <%-- Both Columns are same but adding for User Interface Purpose--%>
                                  <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Enhancement ID" Visible="false">                           
                                        <ItemTemplate> 
                                           <asp:Label ID="lblEnhanceId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                          <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                          <asp:TextBox ID="txtEnhanceId" runat="server" placeholder="Enter Enhance Id " Width="150px" MaxLength="10"></asp:TextBox>
                                      </asp:Panel>
                                       </FooterTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" SortExpression="DT_CODE">                              
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblDtCode" runat="server" Text='<%# Bind("DT_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                                          <asp:TextBox ID="txtDtcCode" runat="server" placeholder="Enter DTC Code " Width="150px" MaxLength="6" onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)"  ></asp:TextBox>
                                       </asp:Panel>
                                        </FooterTemplate>
                                     
                                   </asp:TemplateField>
                                  
                                  <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="DTC Name"  SortExpression="DT_NAME">                             
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDtName" runat="server" Text='<%# Bind("DT_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch" >
                                          <asp:TextBox ID="txtdtcName" runat="server" placeholder="Enter DTC Name " Width="150px" MaxLength="50" onkeypress="return characterAndspecialDtc(event)"  
                                        onchange = " return cleanSpecialAndNumDtc(this)" ></asp:TextBox>
                                       </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code">                            
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        <asp:Panel ID="panel5" runat="server" DefaultButton="imgBtnSearch" >
                                          <asp:TextBox ID="txtDtrCode" runat="server" placeholder="Enter DTr Code " Width="150px" MaxLength="6" onkeypress="return onlyNumbers(event)" onchange = "return cleanSpecialAndChar(this)"></asp:TextBox>
                                         </asp:Panel>
                                         </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="" HeaderText="WO Issued" Visible="false">           
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="WO No" SortExpression="WO_NO">          
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblWoNo" runat="server" Text='<%# Bind("WO_NO") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                           <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <center>
                                        <asp:LinkButton runat="server"  CommandName="CreateNew" ID="lnkCreate" >
                                             <img src="../Styles/images/Create.png" style="width:20px" />Issue WO</asp:LinkButton>
                                         <asp:LinkButton runat="server"  CommandName="Create" ID="lnkUpdate"  visible="false" >
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

                             <asp:GridView ID="grdNewDTC" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"                                 
                            CssClass="table table-striped table-bordered table-advance table-hover" Visible="false"
                                runat="server" ShowHeaderWhenEmpty="True" ShowFooter="true"
                            EmptyDataText="No Records Found" 
                            onpageindexchanging="grdNewDTC_PageIndexChanging" 
                            onrowcommand="grdNewDTC_RowCommand" OnSorting="grdNewDTC_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" /> 
    
                                <Columns>
                                 <asp:TemplateField AccessibleHeaderText="WO_SLNO" HeaderText="WO Slno" Visible="false">                           
                                        <ItemTemplate> 
                                           <asp:Label ID="lblWOSlno" runat="server" Text='<%# Bind("WO_SLNO") %>'></asp:Label>
                                        </ItemTemplate>
                                      
                                  </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="WO No" SortExpression="WO_NO">          
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblWoNo" runat="server" Text='<%# Bind("WO_NO") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                          <asp:TextBox ID="txtWoNo" runat="server" placeholder="Enter WO No" Width="150px"  MaxLength="17" ></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date" >          
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblWoDate" runat="server" Text='<%# Bind("WO_DATE") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                             <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="WO_ACC_CODE" HeaderText="WO Account Code">          
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblWoAccountCode" runat="server" Text='<%# Bind("WO_ACC_CODE") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <center>
                                        
                                         <asp:LinkButton runat="server"  CommandName="Create" ID="lnkUpdate" >
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
                    <!-- END SAMPLE FORM PORTLET-->
                 
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->

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
                   This Web Page Can Be Used To View All Type Of WorkOrder and For New DTC Work Order Can be Created.</li>
                     <li>  To Create Work Order For New DTC Click on Type DropDown and Select New DTC, Right Corner New Button Will Appear Click on The Button
                       
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

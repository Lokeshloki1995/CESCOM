<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcMaintainanceView.aspx.cs" Inherits="IIITS.DTLMS.Maintainance.TcMaintainanceView" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>

    <div >
      
         <div class="container-fluid">
            <%-- BEGIN PAGE HEADER--%>
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                       Maintenance View
                   </h3>
                       <a style="float:right!important;margin-left:-372px!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> ddd </button>
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
                            <h4><i class="icon-reorder"></i> Maintenance View </h4>
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
                              <div class="span2">
                              <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>
                              </div>
                          <div class="span3">
                            <asp:RadioButton ID="rdbQuarterly" runat="server" Text="Quarterly" CssClass="radio" 
                                  GroupName="a" Checked="true" AutoPostBack="true" oncheckedchanged="rdbQuarterly_CheckedChanged"
                                   />
                          </div>
                           <div class="span4">
                              <asp:RadioButton ID="rdbHalfYearly" runat="server"  Text="Half Yearly" 
                                   CssClass="radio" GroupName="a"  AutoPostBack="true" oncheckedchanged="rdbHalfYearly_CheckedChanged"
                                    />
                            </div>

                             <div style="float:right;">
                                      <div class="span5">
                                   <asp:Button ID="cmdNew" runat="server" Text="New" 
                                              CssClass="btn btn-primary" onclick="cmdNew_Click" /><br /></div>
                                         <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickTCMaintainance" /><br />
                                          </div>
                                        </div>
                             </div>

                                  
                      </div>
                        </div>


                           
                                  
                                    <div class="space20"> </div>
                                 
                                <!-- END FORM-->
                           
                        
                            <asp:GridView ID="grdTcMaintainance"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10"  ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdTcMaintainance_PageIndexChanging" 
                                    onrowcommand="grdTcMaintainance_RowCommand" OnSorting="grdTcMaintainance_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="TM_ID" HeaderText=" Maintance ID"  Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMaintainId" runat="server" Text='<%# Bind("TM_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="DTCNAME" HeaderText="DTC Name" SortExpression="DTCNAME">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblrepairtype" runat="server" Text='<%# Bind("DTCNAME") %>' style="word-break:break-all" Width="200px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                          <asp:Panel ID="panel1" runat="server" DefaultButton="cmdSearch" >
                                             <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name" Width="200px" onkeypress="return characterAndspecialDtc(event)"  
                                        onchange ="return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                       </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                   
                                   <asp:TemplateField AccessibleHeaderText="TM_DT_CODE" HeaderText="DTC code" SortExpression="TM_DT_CODE">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTmDtCode" runat="server" Text='<%# Bind("TM_DT_CODE") %>' style="word-break:break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                         <asp:Panel ID="panel2" runat="server" DefaultButton="cmdSearch" >
                                              <asp:TextBox ID="txtDTCCode" runat="server" placeholder="Enter DTC Code" Width="100px"  MaxLength="6" onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)" ></asp:TextBox>
                                   </asp:Panel>
                                   </FooterTemplate>
                                    </asp:TemplateField>
                                  
                                  <asp:TemplateField AccessibleHeaderText="TM_TC_CODE" HeaderText="DTR Code" SortExpression="TM_TC_CODE">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTmTcCode" runat="server" Text='<%# Bind("TM_TC_CODE") %>' style="word-break:break-all" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                          <asp:Panel ID="panel3" runat="server" DefaultButton="cmdSearch" >
                                              <asp:TextBox ID="txtTCCode" runat="server" placeholder="Enter DTr Code" Width="100px"  MaxLength="6" onkeypress="return onlyNumbers(event)" onchange = "return cleanSpecialAndChar(this)" ></asp:TextBox>
                                  </asp:Panel>
                                   </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TM_MAINTAIN_TYPE" HeaderText="Maintenance Type" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMaintainType" runat="server" Text='<%# Bind("TM_MAINTAIN_TYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                               <asp:ImageButton ID="cmdSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="30px"
                                                ToolTip="Search" CommandName="search" />
                                   </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TM_DATE" HeaderText="Last Service Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lbldate" runat="server" Text='<%# Bind("TM_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                    <asp:TemplateField AccessibleHeaderText="TM_MAINTAIN_BY" HeaderText="Maintenance By" SortExpression="TM_MAINTAIN_BY">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMaintananceBy" runat="server" Text='<%# Bind("TM_MAINTAIN_BY") %>' style="word-break:break-all" Width="200px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" OnClick="imgBtnEdit_Click"
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
                    <!-- END SAMPLE FORM PORTLET-->
                  <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
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
                   This Web Page Can Be Used To View  Maintenance Details and New DTC Maintenance Plan Can Be Created.</li>
                     <li>  Maintenance View Can Be Filtered by Selecting <u>Quarterly</u> and <u>Half Yearly Radio</u> Button</li>
                  <li> New Maintenance Plan Can Be Created By Clicking <u>New</u> Button
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

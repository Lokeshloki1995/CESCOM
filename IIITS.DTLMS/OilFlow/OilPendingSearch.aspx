<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="OilPendingSearch.aspx.cs" Inherits="IIITS.DTLMS.OilFlow.OilPendingSearch" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
   <script type="text/javascript" >
//       function ValidateMyForm() {

//           if (document.getElementById('<%= txtPONo.ClientID %>').value.trim() == "") {
//               alert('Enter Valid Purchase Order No')
//               document.getElementById('<%= txtPONo.ClientID %>').focus()
//               return false
//           }
//       }
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
     <script type="text/javascript">
     //Purchase Order allow to search chars, nums and -/
        function characterAndnumbersPo(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 47) {

                return false;
            }
            return true;
        }

        // Purchase Order allow Chatractes and Numbers to paste
        function cleanSpecialCharsPo(t) {
           
            //t.value = t.value.toString().replace(/[^-/a-zA-Z 0-9\n\r]+/g, '');
            //alert(" Special charactes are not allowed!");
        }
         </script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
 <div>
      <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Oil Pending to Recieve                   
                                      
                   </h3>
                        <a style="margin-right:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
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
                            <h4><i class="icon-reorder"></i> Oil Pending to Receive   </h4>
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
                                            <label class="control-label">Purchase Order No</label>
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                    <asp:TextBox ID="txtPONo" runat="server" MaxLength="50" onkeypress="return characterAndnumbersPo(event)" onchange ="return cleanSpecialCharsPo(this)" ></asp:TextBox>
                                                     <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary" 
                                                      TabIndex="2"  />      
                                                </div>
                                            </div>
                                        </div>                             

                                     

                                    </div>
                                                                       
                </div>
                <div class="space20"></div>
                <div  class="text-center" align="center">
                  <div class="span4"></div>
                    <div class="span3">
                        <asp:Button ID="cmdLoad" runat="server" Text="Load"  OnClientClick="javascript:return ValidateMyForm()"
                            CssClass="btn btn-primary" onclick="cmdLoad_Click" />
                    </div>
                    <div style="margin-left:-12px!important"class="span1">
                        <asp:Button ID="cmdReset" runat="server" Text="Reset"  CssClass="btn btn-danger"  onclick="cmdReset_Click" />
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

              <div class="row-fluid" runat="server"  >
                    <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Testing Pass Details</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
<%--                            <div class="">
                         <asp:Button ID="cmbExport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                        OnClick="Export_ClickDeliverPendingSearch" /><br />
                         </div>--%>
                            <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                           <asp:GridView ID="grdTestingPass" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server"  
                                TabIndex="5" onrowcommand="grdTestingPass_RowCommand" 
                                        onpageindexchanging="grdTestingPass_PageIndexChanging" OnSorting="grdTestingPass_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                    <asp:TemplateField AccessibleHeaderText="OSD_ID" HeaderText="Oil_PO_ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblOSDID" runat="server" Text='<%# Bind("OSD_ID") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   

                                    <asp:TemplateField AccessibleHeaderText="OSD_PO_NO" HeaderText="PO No">                                
                                         <ItemTemplate>                                       
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("OSD_PO_NO") %>' style="word-break: break-all;" width="80px"></asp:Label>                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("OSD_PO_DATE") %>' style="word-break: break-all;" width="130px"></asp:Label>                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText="Oil Quantity(in Kltr)">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTotalQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="OSD_OFFICE_CODE" HeaderText="Location">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblOfficecode" runat="server" Text='<%# Bind("OSD_OFFICE_CODE") %>' style="word-break: break-all;" width="90px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText="Received Quantity" Visible="false">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecievedQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   <asp:TemplateField HeaderText="Receive">
                                    <ItemTemplate>
                                        <center>
                                           <asp:ImageButton  ID="imgbtnRecieve" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"  
                                              CommandName="Recieve"   Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
 
                                </Columns>

                            </asp:GridView>



                               <asp:GridView ID="GridView1" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server"  
                                TabIndex="5" onpageindexchanging="grdTestPending_PageIndexChanging" OnSorting="grdTestPending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                
                                    <asp:TemplateField AccessibleHeaderText="OSD_PO_NO" HeaderText="PO No" SortExpression="OSD_PO_NO">                                
                                         <ItemTemplate>                                       
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("OSD_PO_NO") %>' style="word-break: break-all;" width="120px"></asp:Label>                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("OSD_PO_DATE") %>' style="word-break: break-all;" width="80px"></asp:Label>                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText="Oil Quantity(in Kltr)">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Receive">
                                    <ItemTemplate>
                                        <center>
                                           <asp:ImageButton  ID="imgbtnRecieve" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"  
                                              CommandName="Recieve"   Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
 
                                </Columns>

                            </asp:GridView>
                                     </div>
                                    </div>

                    </div>
                </div>
             </div>


               <div  class="row-fluid" >
                    <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Pending To Receive</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

<%--                            <div class="">
                         <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info"
                        OnClick="Export_ClickPendingTesting" /><br />
                         </div>--%>
                            <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                           <asp:GridView ID="grdTestPending" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server"  
                                TabIndex="5" onpageindexchanging="grdTestPending_PageIndexChanging" OnSorting="grdTestPending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                
                                    <asp:TemplateField AccessibleHeaderText="OSD_PO_NO" HeaderText="PO No" SortExpression="RSM_PO_NO">                                
                                         <ItemTemplate>                                       
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("OSD_PO_NO") %>' style="word-break: break-all;" width="70px"></asp:Label>                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("OSD_PO_DATE") %>' style="word-break: break-all;" width="130px"></asp:Label>                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="OSD_INVOICE_NO" HeaderText="Invoice No" visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("OSD_INVOICE_NO") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText="Oil Quantity(in Kltr)" >                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' style="word-break: break-all;" width="70px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OSD_OFFICE_CODE" HeaderText="Location">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblOfficeCode" runat="server" Text='<%# Bind("OSD_OFFICE_CODE") %>' style="word-break: break-all;" width="90px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
 
                                </Columns>

                            </asp:GridView>
                                     </div>
                                    </div>

          

                    </div>
                </div>
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
This Web Page Can Be Used To Receive The Transformer for Which Testing is Completed it will also Display the Testing Pass Details and Pending for Testing Details</li>
                      <li></li> To Receive The Transformer User Need To Click On <u>Receive</u> LinkButton</li>
                      <li> Once Receive Button is Clicked User Need To Enter Receive Details and  after that <u>Receive</u> Button To Receive The TC in Store</li>
                     </ul>

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

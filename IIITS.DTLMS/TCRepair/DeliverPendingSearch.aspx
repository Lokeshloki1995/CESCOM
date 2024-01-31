<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DeliverPendingSearch.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.DeliverPendingSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        //       function ValidateMyForm() {

        //           if (document.getElementById('<%= txtWoNo.ClientID %>').value.trim() == "") {
        //               alert('Enter Valid Purchase Order No')
        //               document.getElementById('<%= txtWoNo.ClientID %>').focus()
        //               return false
        //           }
        //       }
    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ascending th a {
            background: url(/img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(/img/sort_desc.png) no-repeat;
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Transformer Pending to Recieve                   
                                      
                    </h3>
                    <a style="margin-right: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
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
                            <h4><i class="icon-reorder"></i>Transformer Pending to Recieve   </h4>
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
                                                        <asp:TextBox ID="txtWoNo" runat="server" MaxLength="50" onkeypress="return characterAndnumbersPo(event)" onchange="return cleanSpecialCharsPo(this)"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary"
                                                            TabIndex="2" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Repairer</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRepairer" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>



                                        </div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Supplier</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSupplier" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="text-center" align="center">
                                        <div class="span4"></div>
                                        <div class="span3">
                                            <asp:Button ID="cmdLoad" runat="server" Text="Load Pending Transformer" OnClientClick="javascript:return ValidateMyForm()"
                                                CssClass="btn btn-primary" OnClick="cmdLoad_Click" />
                                        </div>
                                        <div style="margin-left: -12px!important" class="span1">
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="cmdReset_Click" />
                                        </div>
                                        <%--<div class="span1">
                         <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                        OnClick="Export_ClickDeliverPendingSearch" OnClientClick="javascript:return ValidateMyForm()"/><br />
                         </div>--%>
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

                <div class="row-fluid" runat="server">
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
                                <div class="">
                                    <asp:Button ID="cmbExport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        OnClick="Export_ClickDeliverPendingSearch" /><br />
                                </div>
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <asp:GridView ID="grdTestingPass" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                        AutoGenerateColumns="false" PageSize="10"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                        runat="server"
                                        TabIndex="5" OnRowCommand="grdTestingPass_RowCommand"
                                        OnPageIndexChanging="grdTestingPass_PageIndexChanging" OnSorting="grdTestingPass_Sorting" AllowSorting="true">
                                        <HeaderStyle CssClass="both" />
                                        <Columns>

                                            <asp:TemplateField AccessibleHeaderText="RSM_ID" HeaderText="Master Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepairMasterId" runat="server" Text='<%# Bind("RSM_ID") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO No" SortExpression="RSM_PO_NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("PODATE") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="ISSUEDATE" HeaderText="Issue Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssueDate" runat="server" Text='<%# Bind("ISSUEDATE") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier/Repairer" SortExpression="SUP_REPNAME">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupRepName" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PO_QUANTITY" HeaderText="Total Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalQty" runat="server" Text='<%# Bind("PO_QUANTITY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PENDING_QNTY" HeaderText="Pending Qty for Recieve">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingQty" runat="server" Text='<%# Bind("PENDING_QNTY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="DELIVERED_QNTY" HeaderText="Recieved Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeliveredQty" runat="server" Text='<%# Bind("DELIVERED_QNTY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="REPAIR_SENT_OIL" HeaderText="Repair Sent Oil">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepairsentoil" runat="server" Text='<%# Bind("REPAIR_SENT_OIL") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Recieve">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="imgbtnRecieve" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                            CommandName="Recieve" Width="12px" />
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


                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4><i class="icon-reorder"></i>Pending for Testing</h4>
                                <span class="tools">
                                    <a href="javascript:;" class="icon-chevron-down"></a>
                                    <a href="javascript:;" class="icon-remove"></a>
                                </span>
                            </div>
                            <div class="widget-body">

                                <div class="">
                                    <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        OnClick="Export_ClickPendingTesting" /><br />
                                </div>
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <asp:GridView ID="grdTestPending" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                        AutoGenerateColumns="false" PageSize="10"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                        runat="server"
                                        TabIndex="5" OnPageIndexChanging="grdTestPending_PageIndexChanging" OnSorting="grdTestPending_Sorting" AllowSorting="true">
                                        <HeaderStyle CssClass="both" />
                                        <Columns>


                                            <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO No" SortExpression="RSM_PO_NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("PODATE") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="ISSUEDATE" HeaderText="Issue Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssueDate" runat="server" Text='<%# Bind("ISSUEDATE") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier/Repairer" SortExpression="SUP_REPNAME">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupRepName" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PO_QUANTITY" HeaderText="Total Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalQty" runat="server" Text='<%# Bind("PO_QUANTITY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PENDING_QNTY" HeaderText="Pending Qty With HT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingQty" runat="server" Text='<%# Bind("PENDING_QNTY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="DELIVERED_QNTY" HeaderText="Delivered Quantity" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeliveredQty" runat="server" Text='<%# Bind("DELIVERED_QNTY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PENDING_QTY_EE" HeaderText="Pending With EE" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeliveredQty" runat="server" Text='<%# Bind("PENDING_QTY_EE") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
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
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul>
                        <li>This Web Page Can Be Used To Recieve The Transformer for Which Testing is Completed it will also Display the Testing Pass Details and Pending for Testing Details</li>
                        <li>To Receive The Transformer User Need To Click On <u>Receive</u> LinkButton</li>
                        <li>Once Receive Button is Clicked User Need To Enter Receive Details and  after that <u>Recieve</u> Button To Receive The TC in Store</li>
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

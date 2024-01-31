<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ReclassificationView.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.ReclassificationView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Pending for Item Type Reclassification                   
                                      
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

                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4><i class="icon-reorder"></i>Pending for Item Type Reclassification</h4>
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
                                        runat="server" OnRowCommand="grdTestingPass_RowCommand"
                                        TabIndex="5" OnPageIndexChanging="grdTestPending_PageIndexChanging" OnSorting="grdTestPending_Sorting" AllowSorting="true">
                                        <HeaderStyle CssClass="both" />
                                        <Columns>

                                            <asp:TemplateField AccessibleHeaderText="RSM_ID" HeaderText="Master Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepairMasterId" runat="server" Text='<%# Bind("RSM_ID") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO No" SortExpression="RSM_PO_NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpono" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpodate" runat="server" Text='<%# Bind("PODATE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="ISSUEDATE" HeaderText="Issue Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblissuedate" runat="server" Text='<%# Bind("ISSUEDATE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier/Repairer" SortExpression="SUP_REPNAME">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepname" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PO_QUANTITY" HeaderText="Total Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpoquantity" runat="server" Text='<%# Bind("PO_QUANTITY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="EE_INS_QTY" HeaderText="Inspected Qty By HT">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEEinsqty" runat="server" Text='<%# Bind("INSPECTED_QNTY_HT") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PENDING_QNTY" HeaderText="Qty Pending with EE for Type Reclassification">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingqty" runat="server" Text='<%# Bind("PENDING_QNTY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Action">
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
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>


                        </div>
                    </div>
                </div>

            </div>
            <!-- END PAGE CONTENT-->

                            <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4><i class="icon-reorder"></i>Completed Item Type Reclassification</h4>
                                <span class="tools">
                                    <a href="javascript:;" class="icon-chevron-down"></a>
                                    <a href="javascript:;" class="icon-remove"></a>
                                </span>
                            </div>
                            <div class="widget-body">

                                <div class="">
                                    <asp:Button ID="Button2" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                        OnClick="Export_ClickPassedTesting" /><br />
                                </div>
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <asp:GridView ID="grdTestingPassEE" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                        AutoGenerateColumns="false" PageSize="10"
                                        CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                        runat="server" OnRowCommand="grdTestingPass_RowCommand"
                                        TabIndex="5" OnPageIndexChanging="grdTestingPassEE_PageIndexChanging" OnSorting="grdTestingPassEE_Sorting" AllowSorting="true">
                                        <HeaderStyle CssClass="both" />
                                        <Columns>

                                            <asp:TemplateField AccessibleHeaderText="RSM_ID" HeaderText="Master Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepairMasterId" runat="server" Text='<%# Bind("RSM_ID") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO No" SortExpression="RSM_PO_NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpono" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpodate" runat="server" Text='<%# Bind("PODATE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="ISSUEDATE" HeaderText="Issue Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblissuedate" runat="server" Text='<%# Bind("ISSUEDATE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier/Repairer" SortExpression="SUP_REPNAME">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepname" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="PO_QUANTITY" HeaderText="Total Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpoquantity" runat="server" Text='<%# Bind("PO_QUANTITY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="EE_INS_QTY" HeaderText="Inspected By EE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEEinsqty" runat="server" Text='<%# Bind("EE_INS_QTY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>

                                    </asp:GridView>
                                </div>
                            </div>
                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>


                        </div>
                    </div>
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
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul>
                        <li>This Web page will direct you to reclassify the transformer condition with respective to the PO.</li>
                        <li>To reclassify the transformer, you should click on approve button.</li>
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

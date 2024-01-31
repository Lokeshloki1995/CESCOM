<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DesignationView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DesignationView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css" />
    <style type="text/css">
        .ascending th a {
            background: url(img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }



        .descending th a {
            background: url(img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }
    </style>

    <script type="text/javascript">
        
        jQuery(function ($) {
            $(document).ready(function () {
                $('#grdDesignationDetails').dataTable({
                    "aaSorting": [],
                    "jQueryUI": true
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Designation View
                </h3>
                <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
                        <h4><i class="icon-reorder"></i>Designation View</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">




                        <div style="float: right">
                            <div class="span7">
                                <asp:Button ID="cmdNew" runat="server" Text="New Designation"
                                    CssClass="btn btn-primary" OnClick="cmdNew_Click" /><br />
                            </div>



                            <div class="span1">
                                <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-primary"
                                    OnClick="Export_clickDest" /><br />
                            </div>
                        </div>

                        <div class="space20"></div>
                        <!-- END FORM-->




                        <asp:GridView ID="grdDesignationDetails" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                            ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                            CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" OnPageIndexChanging="grdDesignationDetails_PageIndexChanging" OnSorting="grdDesignationDetails_Sorting" AllowSorting="true">
                            <HeaderStyle CssClass="both" />

                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="DM_DESGN_ID" HeaderText="Id" Visible="false">

                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("DM_DESGN_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="DM_NAME" HeaderText="Designation" Visible="true" SortExpression="DM_NAME">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("DM_NAME") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("DM_NAME") %>' Style="word-break: break-all;" Width="200px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="DM_DESC" HeaderText="Description"
                                    Visible="true" SortExpression="DM_DESC">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAddress" runat="server" Text='<%# Bind("DM_DESC") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("DM_DESC") %>' Style="word-break: break-all;" Width="200px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>




                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                OnClick="imgBtnEdit_Click" Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                CausesValidation="false" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="span7"></div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

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
                 <li>This Web Page Can Be Used To View All The Designation and To Add New Designation.</li>
                <li>  Existing Designation Details Can Be Edited By Clicking Edit Button</li>
                 <li>New Designation  Can Be Added By Clicking New Designation Button
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

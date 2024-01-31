<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="AgencyMasterView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.AgencyMasterView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../assets/jquery.dataTables.css" rel="stylesheet" />
    <script type="text/javascript" src="../assets/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.13/js/jquery.dataTables.min.js"></script>
    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.13/css/jquery.dataTables.min.css" />
    <script type="text/javascript" src="/lib/jquery.min.js"></script>
    <script type="text/javascript" src="/lib/jquery.plugin.js"></script>
    <script type="text/javascript" src="assets/jquery-1.11.1.min.js"></script>

    <script type="text/javascript" src="assets/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" href="assets/jquery.dataTables.min.css" />
    <script type="text/javascript" src="~/Scripts/jquery.js"></script>
    <script type="text/javascript" src="~/Scripts/data-table/jquery.dataTables.js"></script>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.16/js/jquery.dataTables.min.js"></script>

    <script type="text/javascript">$(document).ready(function () {
    $.noConflict();
    var table = $('grdRepairer').DataTable();
});
    </script>

    <style type="text/css">
        .dataTables_wrapper .dataTables_length, .dataTables_wrapper .dataTables_filter, .dataTables_wrapper .dataTables_info, .dataTables_wrapper .dataTables_processing, .dataTables_wrapper .dataTables_paginate {
            color: #333;
        }

        .dataTables_wrapper .dataTables_paginate {
            float: right;
            text-align: right;
            padding-top: 0.25em;
        }

            .dataTables_wrapper .dataTables_paginate .paginate_button.current, .dataTables_wrapper .dataTables_paginate .paginate_button.current:hover {
                color: #333 !important;
                border: 1px solid #979797;
                background-color: #fff;
                background: -webkit-gradient(linear, left top, left bottom, color-stop(0%, white), color-stop(100%, #dcdcdc));
                background: -webkit-linear-gradient(top, white 0%, #dcdcdc 100%);
                background: -moz-linear-gradient(top, white 0%, #dcdcdc 100%);
                background: -ms-linear-gradient(top, white 0%, #dcdcdc 100%);
                background: -o-linear-gradient(top, white 0%, #dcdcdc 100%);
                background: linear-gradient(to bottom, white 0%, #dcdcdc 100%);
            }

            .dataTables_wrapper .dataTables_paginate .paginate_button {
                box-sizing: border-box;
                display: inline-block;
                min-width: 1.5em;
                padding: 0.5em 1em;
                margin-left: 2px;
                text-align: center;
                text-decoration: none !important;
                cursor: pointer;
                *cursor: hand;
                color: #333 !important;
                border: 1px solid transparent;
                border-radius: 2px;
            }

        .dataTables_wrapper .dataTables_info {
            clear: both;
            float: left;
            padding-top: 0.755em;
        }

        table {
            overflow: scroll;
        }

        td {
            border: 1px solid #ccc;
            text-align: center;
        }

        .table-advance thead tr th {
            background-color: #438eb9 !important;
            color: #fff;
        }

        table.dataTable thead th {
            border-bottom: 1px solid #111;
            font-size: 12px !important;
        }

        table.dataTable tbody th, table.dataTable tbody td {
            padding: 10px 0px !important;
            text-align: center !important;
        }

        .table-advance tr td {
            border-left-width: 1px !important;
            border: 1px solid #d4d4d4;
            font-size: 12px !important;
        }

        th {
            white-space: nowrap;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $('#ContentPlaceHolder1_grdRepairer').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({

                //"sPaginationType": "full_numbers"
            });
        });
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip(); // added css in font-awesome.min.css line 43 and 405
        });

        function ConfirmStatus(status) {

            var result = confirm('Are you sure,Do you want to ' + status + ' Agency?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <style type="text/css">
        table#ContentPlaceHolder1_grdRepairer {
            overflow: auto;
        }

        td {
            border: none;
            text-align: center;
        }

        .table-advance thead tr th {
            background-color: #438eb9 !important;
            color: #fff;
        }

        th {
            white-space: nowrap;
            text-align: center !important;
        }

        thead {
            text-align: center !important;
        }
        /*span {
    text-align: center;
}*/
        select#ContentPlaceHolder1_cmbZone, select#ContentPlaceHolder1_cmbsubdivision, select#ContentPlaceHolder1_cmbCircle, select#ContentPlaceHolder1_cmbSection, select#ContentPlaceHolder1_cmbDivision {
            width: 216px !important;
        }

        div#ContentPlaceHolder1_grdRepairer_length {
            margin-bottom: -43px !important;
        }

        select {
            width: 70px;
        }

        .gvPagerCss span {
            background-color: #f9f9f9;
            font-size: 18px;
        }

        .gvPagerCss td {
            padding-left: 5px;
            padding-right: 5px;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

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
        /*div.pager {
    text-align: center;
    margin: 1em 0;
}

div.pager {
    text-align: center;
    margin: 1em 0;
}

div.pg-goto {
color: #000000;
font-size: 15px;
cursor: pointer;
background: #D0B389;
padding: 2px 4px 2px 4px;
}

div.pg-selected {
color: #fff;
font-size: 15px;
background: #000000;
padding: 2px 4px 2px 4px;
}

div.pg-normal {
color: #000000;
font-size: 15px;
cursor: pointer;
background: #D0B389;
padding: 2px 4px 2px 4px;
}*/
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Agency Master Details
                </h3>
                <ul class="breadcrumb" style="display: none">

                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="Text1" type="text">
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
        <!-- END PAGE CONTENT-->
        <div class="widget-body">


            <div style="float: right">
                <div class="span7">
                    <asp:Button ID="cmdNew" runat="server" Text="New Agency"
                        CssClass="btn btn-primary" OnClick="cmdNew_Click" />
                </div>

            </div>
            <div class="space10"></div>

        </div>
        <div class="row-fluid">
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Existing Agency</h4>
                    <span class="tools">
                        <a href="javascript:;" class="icon-chevron-down"></a>
                        <a href="javascript:;" class="icon-remove"></a>
                    </span>
                </div>
                <div class="widget-body">

                    <div class="widget-body form">
                        <!-- begin form-->
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="">
                                    <asp:GridView ID="grdRepairer"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" OnRowDataBound="grdRepairer_RowDataBound" OnRowCommand="grdRepairer_RowCommand"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        runat="server"
                                        ShowFooter="false">
                                        <PagerStyle CssClass="gvPagerCss " />
                                        <HeaderStyle CssClass="both" />


                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="RA_ID" HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepairId" runat="server" Text='<%# Bind("RA_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="AM_ID" HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMappingid" runat="server" Text='<%# Bind("AM_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="RA_NAME" HeaderText="Agency Name" Visible="true" SortExpression="TR_NAME" ItemStyle-Width="200">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("RA_NAME") %>' Style="word-break: break-all;" Width="250"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                        <asp:TextBox ID="txtAgencyName" runat="server" placeholder="Enter Agency Name" Width="100px"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="RA_ADDRESS" HeaderText="Addresss" Visible="true" ItemStyle-Width="300">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("RA_ADDRESS") %>' Style="word-break: break-all;" Width="300"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="RA_PHNO" HeaderText="Mobile Number" Visible="true" ItemStyle-Width="130">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblphone" runat="server" Text='<%# Bind("RA_PHNO") %>' Style="word-break: break-all;" Width="130"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="TS_EMAIL" HeaderText="EmailId" Visible="true" ItemStyle-Width="300">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("RA_MAIL") %>' Style="word-break: break-all;" Width="300"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="OFFICE_NAME" HeaderText="Location" Visible="true" ItemStyle-Width="100">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiv" runat="server" Text='<%# Bind("OFFICE_NAME") %>' Style="word-break: break-all;" Width="100"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Click to Edit" ItemStyle-Width="100">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" Height="25px" ImageUrl="~/Styles/images/edit64x64.png"
                                                            Width="25px" CommandName="upload" />
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("AM_STATUS") %>'></asp:Label>
                                                    <centre>
                       <asp:ImageButton Visible="false"  ID="imgDeactive"  runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status" 
                       OnClientClick="return ConfirmStatus('Enable');" tooltip="Click to Activate Agency"  width="18px" />     
                     <!-- OnClientClick="return ConfirmStatus('Activate');" -->   
                        <asp:ImageButton Visible="false"  ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif"  CommandName="status" 
                        OnClientClick="return ConfirmStatus('Disable');" tooltip="Click to DeActivate Agency" width="25px"  />        
                 </centre>
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                        </Columns>
                                        <PagerSettings FirstPageText="first" LastPageText="last" Mode="NumericFirstLast" />
                                        <%--      <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Font-Size="Medium" Width="15px"/>
      <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" Font-Bold="true" Font-Size="Medium" Width="15px" HorizontalAlign="Center" VerticalAlign="Middle"/>--%>
                                    </asp:GridView>
                                </div>



                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </div>

    </div>


</asp:Content>

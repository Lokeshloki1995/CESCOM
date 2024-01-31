<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="InchargeView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.InchargeView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">

        <link type="text/css" href="../assets/jquery.dataTables.css" rel="stylesheet" />
    <script type="text/javascript" src="../assets/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.13/js/jquery.dataTables.min.js"></script>
    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.13/css/jquery.dataTables.min.css" />
        <script type="text/javascript" src="/lib/jquery.min.js"></script>
    <script type="text/javascript" src="/lib/jquery.plugin.js"></script>
        <script  type="text/javascript" src="assets/jquery-1.11.1.min.js"></script>

         <script type="text/javascript" src="assets/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" href="assets/jquery.dataTables.min.css" />
    <script type="text/javascript" src="~/Scripts/jquery.js"></script>
    <script type="text/javascript" src="~/Scripts/data-table/jquery.dataTables.js"></script>

            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.16/js/jquery.dataTables.min.js"></script>
 

    
    <script type="text/javascript">$(document).ready(function () {
    $.noConflict();
    var table = $('grdUser').DataTable();
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
            $('#ContentPlaceHolder1_grdUser').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({

                //"sPaginationType": "full_numbers"
            });
        });
    </script>

    <script type="text/javascript">


        //allow only characters to enter
        function character(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode > 31 && (charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && charCode != 46 && charCode != 47) {
               
                return false;
            }
            return true;
        }

        //allow only characters to paste
        function cleanSpecialAndNum(t) {
            t.value = t.value.toString().replace(/[^a-zA-Z0-9\t\n\r]+/g, '');
            //alert(" Special charactes and Numbers are not allowed!");
        }

        function enterSearchBox(evt) {
            debugger;
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if(charCode == 13)
            {
                return false;
               // document.getElementById("btnSearch").click();
            }
        }

        function cmbOfficselection(control) {
            debugger;
            var selectedValue = control.value;

            //   0 : corporate office 
            if (selectedValue == 0) {
              
                debugger;
                document.getElementById('officeCode').style.display = "none";
                document.getElementById('btnCorporateOffice').click();
            }
            else {
                document.getElementById('officeCode').style.display = "block";
              //  document.getElementById('ContentPlaceHolder1_btnCorporateOffice').style.display = "none";
            }
        }
       
       


    </script>
    <style type="text/css">
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

        #DropDownList1 {
    position: absolute;
    left: 252px;
}
        .span4 {
    margin-left: 0px!important;
}
       
    </style>

     <style type="text/css">
        table#ContentPlaceHolder1_grdUser {
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

        div#ContentPlaceHolder1_grdUser_length {
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
       
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Incharge View
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

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Incharge View</h4>
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
                                        <div class="container-fluid">
                                            <div style="float: right">
                                                <div class="span2">
                                                   <!-- --> 
                                                    <asp:Button ID="cmdNew" runat="server" Text="Create"
                                                        CssClass="btn btn-success" OnClick="cmdNew_Click" /><br />
                                                </div>

                                            </div>
                                            <div class="space20">
                                            </div>

                                        <%--    <div class="span5" style="border-left:-200px!important" >--%>
<%--                                                 <div class="control-group">
                                                    <label  class="control-label">Select Office</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbOfficeType" runat="server" 
                                                                TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="cmb_OfficeIndexChanged">
                                                                 <asp:ListItem Text="--Select" Value="-1"> --Select-- </asp:ListItem>
                                                                <asp:ListItem Text="Corporate Office" Value="0"> Corporate Office </asp:ListItem>
                                                                <asp:ListItem Text="Others" Value="1"> Others </asp:ListItem>


                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>--%>

                                              <%--  </div>--%>

                                        <%--     <div class="space20">
                                            </div>--%>

                                      <!--      <div id="officeCode"> -->

                                            <div class="span5">


                                                <div class="control-group">
                                                    <label class="control-label">Circle Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:DropDownList ID="cmbCircle" runat="server">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Division Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbDivision" runat="server">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>



                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">Sub Division Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbsubdivision" runat="server" >
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Section Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbSection" runat="server">
                                                            </asp:DropDownList>
                                              
                                                        </div>
                                                    </div>
                                                </div>




                                            </div>

                                              <!--  </div>-->

                                            <div class="span20"></div>

                                            <div class="text-center">

                                                <div class="span5"></div>

                                                <div class="span1">
                                                    <asp:Button ID="cmdload" runat="server" Text="Load" TabIndex="11"
                                                        CssClass="btn btn-success" OnClick="cmdLoad_click" /><br />
                                                </div>


                                                 <div class="span1">
                                                </div>


                                                <div class="space20"></div>
                                            </div>
                                            <div class="span20"></div>
                                            <div>

             <%--                                   <asp:GridView ID="grdUser"
                                                    AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                    runat="server" OnPageIndexChanging="grdUser_PageIndexChanging"
                                                    OnRowCommand="grdUser_RowCommand"
                                                    ShowFooter="True" OnSorting="grdUser_Sorting" AllowSorting="true">
                                                    <HeaderStyle CssClass="both" />
                                                    <Columns>--%>

                                        <asp:GridView ID="grdUser"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" OnRowCommand="grdUser_RowCommand" OnRowDataBound="grdUser_RowDataBound"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        runat="server"
                                        ShowFooter="false">
                                        <PagerStyle CssClass="gvPagerCss " />
                                        <HeaderStyle CssClass="both" />
                                             <Columns>

                                                        <asp:TemplateField AccessibleHeaderText="IOMD_AUTOGEN_OM_NUMBER" HeaderText="Auto Generate Om No" SortExpression="IOMD_AUTOGEN_OM_NUMBER">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblautoomnum" runat="server" Text='<%# Bind("IOMD_AUTOGEN_OM_NUMBER") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                         <asp:TemplateField AccessibleHeaderText="IOMD_MAN_OM_NUMBER" HeaderText="OM Number">

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblomnumber" runat="server" Text='<%# Bind("IOMD_MAN_OM_NUMBER") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField AccessibleHeaderText="IOMD_OM_DATE" HeaderText="OM Date">

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblomdate" runat="server" Text='<%# Bind("IOMD_OM_DATE") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                            </ItemTemplate>
<%--                                                              <FooterTemplate>
                                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9" />
                                                            </FooterTemplate>--%>
                                                        </asp:TemplateField>

                                                       
                                                        <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Office Name" SortExpression="OFF_NAME">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbloffname" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                                            </ItemTemplate>
                                                          
                                                        </asp:TemplateField>

                                                        <asp:TemplateField AccessibleHeaderText="IOML_CHARGE_FROM_DATE" HeaderText="From Date" SortExpression="IOML_CHARGE_FROM_DATE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfromdate" runat="server" Text='<%# Bind("IOML_CHARGE_FROM_DATE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtsDesignation" runat="server" Width="100px" placeholder="Enter Designation" ToolTip="Enter Designation to Search" Visible="false"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField AccessibleHeaderText="IOML_CHARGE_TO_DATE" HeaderText="To Date" SortExpression="IOML_CHARGE_TO_DATE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbltodate" runat="server" Text='<%# Bind("IOML_CHARGE_TO_DATE") %>' Style="word-break: break-all;" Width="200px"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtOfficeName" runat="server" Width="150px" placeholder="Enter Designation" ToolTip="Enter Designation to Search" Visible="false"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Edit" Visible="false">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create"
                                                                        Width="12px" />
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                             <asp:TemplateField AccessibleHeaderText="ACTUAL_USER" HeaderText="Actual User" SortExpression="ACTUAL_USER">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActualuser" runat="server" Text='<%# Bind("ACTUAL_USER") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                                            </ItemTemplate>
                                                          
                                                        </asp:TemplateField>

                                                     <asp:TemplateField AccessibleHeaderText="HANDOVER_USER" HeaderText="Handover User">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHandover" runat="server" Text='<%# Bind("HANDOVER_USER") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                              <PagerSettings FirstPageText="first" LastPageText="last" Mode="NumericFirstLast" />
                                                </asp:GridView>


                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>


                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>



                    <!-- END PAGE HEADER-->
                    <!-- BEGIN PAGE CONTENT-->



                    <!-- END PAGE CONTENT-->
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
                    <ul><li>
                   This Web Page Can be used To View Incharge User Details and Create New Incharge</li>
                 <li> Create Incharge Can Be Added By Clicking Create Button</li>
                 <li> To View The Incharge Details Click On Load Button
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
    <script type="text/javascript">
        
</script>
   

 

</asp:Content>

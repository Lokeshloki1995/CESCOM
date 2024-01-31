<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="MisDTCCountFeeder.aspx.cs" Inherits="IIITS.DTLMS.Reports.MisDTCCountFeeder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtFromDate.ClientID%>").datepicker(
        {
           
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true ,
            changeDate: false,
            maxDate: 0,
             onSelect: function (selected) {
                $("#<%=txtToDate.ClientID%>").datepicker("option", "minDate", selected)
            } ,
           
        })
        })

        $(document).ready(function () {
            $("#<%=txtToDate.ClientID%>").datepicker(
        {
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            changeDate: false,
            maxDate: 0,
            onSelect: function (selected) {
                $("#<%=txtFromDate.ClientID%>").datepicker("option", "maxDate", selected)
            } ,
        })
        })
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">DTC COUNT
                </h3>

                <ul class="breadcrumb" style="display: none">
                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text" />
                                <button class="btn" type="button">
                                    <i class="icon-search"></i>
                                </button>
                            </div>
                        </form>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
            <div style="float: right; margin-top: 20px; margin-right: 12px">
                <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='DTCAddDetails.aspx'; return false;"
                    CssClass="btn btn-primary" />
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DTC COUNT</h4>
                        <a style="float: right!important; padding-top: 6px; color: #fff; margin-right: 1%!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>

                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span5">

                                        <div class="control-group">
                                            <label class="control-label">
                                                Circle <span class="Mandotary">*</span>
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Sub Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSubDivision" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbSubDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                From Date <span class="Mandotary">*</span>
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="5" AutoComplete="off"></asp:TextBox>
                                                   <%-- <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Feeder Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbFeederName" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <%-- another span--%>
                                    <div class="span5">

                                        <div class="control-group">
                                            <label class="control-label">
                                                Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                O & M Section</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbOMSection" runat="server" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="control-group">
                                            <label class="control-label">
                                                To Date<span class="Mandotary">*</span>
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="5" AutoComplete="off"></asp:TextBox>
                                                   <%-- <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="span5">
                                    </div>
                                    <%-- <div class="space20">
                                        </div>
                                       <div style="padding-left: 108px;">
                                            <asp:RadioButtonList ID="rdbReportType" runat="server" CssClass="radio" RepeatDirection="Horizontal"
                                                AutoPostBack="true" Width="679px">
                                                <asp:ListItem Value="1" Selected="True">Division Wise</asp:ListItem>
                                                <asp:ListItem Value="2">SUB Division Wise</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>--%>
                                </div>
                            </div>
                        </div>
                        <!-- END FORM-->
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->
        <!-- BEGIN PAGE CONTENT-->
        <!-- END PAGE CONTENT-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <div class="text-center" align="center">
                    <asp:Button ID="cmdGenerate" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyForm()"
                        CssClass="btn btn-primary" TabIndex="10" OnClick="cmdGenerate_Click" />

                    <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                        TabIndex="11" OnClick="cmdReset_Click" /><br />

                    <br />
                    <%-- <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info"
                        TabIndex="12" OnClick="Export_click" /><br />--%>

                    <div class="span7">
                    </div>
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
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul>
                        <li>This Report will display List of DTC's Added during the Time period.</li>
                        <li>Report can be filtered based on Feeder , Location and From date and To date  .</li>
                        <li>We are considering Created Date of DTC in applicaiton</li>
                    </ul>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
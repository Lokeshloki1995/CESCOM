<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="DTCFailureReport.aspx.cs" Inherits="IIITS.DTLMS.Reports.DTCFailureReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<script type="text/javascript">
        $(function () {
            $('[id*=failureTypeBox]').multiselect({
                includeSelectAllOption: true
            });
        });

    </script>--%>

<%--        <script src="../Scripts/functions.js" type="text/javascript"></script>--%>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtFromDate.ClientID%>").datepicker(
           {
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               maxDate: 0,
           })


            $("#<%=txtToDate.ClientID%>").datepicker(
           {
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               maxDate: 0,
           })
            $("#<%=txtFromDate2.ClientID%>").datepicker(
            {
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: 0,
            })


            $("#<%=txtToDate2.ClientID%>").datepicker(
           {
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               maxDate: 0,
           })

            $("#<%=txtFromDate3.ClientID%>").datepicker(
            {
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: 0,
            })


            $("#<%=txtToDate3.ClientID%>").datepicker(
           {
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               maxDate: 0,
           })


            $("#<%=txtFreqDTRFromDate.ClientID%>").datepicker(
            {
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: 0,
            })


            $("#<%=txtFreqDTRToDate.ClientID%>").datepicker(
           {
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               maxDate: 0,
           })

        });
        </script>

    <style type="text/css">
        .radio {
        }

        /*input#ContentPlaceHolder1_txtFromDate {
            z-index: 1051!important;
        }*/

        .modal.fade.in {
            top: 10%;
            z-index: 9999;
        }

        label {
            display: contents;
            margin-bottom: 5px;
        }
      
        textarea, select, input[type="text"], input[type="password"], input[type="datetime"], input[type="datetime-local"], input[type="date"], input[type="month"], input[type="time"], input[type="week"], input[type="number"], input[type="email"], input[type="url"], input[type="search"], input[type="tel"], input[type="color"], .uneditable-input, .fileupload-new .input-append .btn-file {
   
    margin-top: -15px!important;
}
    </style>
    <script type="text/javascript">
        function ValidateMyForm(x) {
            debugger;
            
            var reportType = x;
            
            if (document.getElementById('<%= cmbReportType.ClientID %>').value.trim() == "1") {
                alert('Please Select Report Type')
                document.getElementById('<%= cmbReportType.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbStage.ClientID %>').value.trim() == "0") {
                alert('Please Select Stage')
                document.getElementById('<%= cmbStage.ClientID %>').focus()
                return false
            }
            if( (document.getElementById('<%=cmbStage.ClientID%>').value.trim() == "10" ) && (reportType =="PDF") )
            {
                alert('OOPS this feature is available in Excel only')
                 document.getElementById('<%= cmbStage.ClientID %>').focus()
                return false    
            }
            if( (document.getElementById('<%=cmbStage.ClientID%>').value.trim() == "11" ) && (reportType =="PDF") )
            {
                alert('OOPS this feature is available in Excel only')
                 document.getElementById('<%= cmbStage.ClientID %>').focus()
                return false    
            }

        }

        function ValidateMyForm1() {
            if (document.getElementById('<%= txtFailMonth.ClientID %>').value.trim() == "0") {
                alert('Please Select Month')
                document.getElementById('<%= txtFailMonth.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbAbstract_ReportType.ClientID %>').value.trim() == "0") {
                alert('Please Select Failure Abstract Report Type')
                document.getElementById('<%= cmbAbstract_ReportType.ClientID %>').focus()
                return false
            }
        }


        function AllowalphabetsNumber(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if ( (charCode >= 65 && charCode <= 90) || (charCode >= 48 && charCode <= 57))
                return true;
            else
                return false;
        }

        function AllowNumbersOnly(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if ((charCode >= 48 && charCode <= 57))
                return true;
            else
                return false;
        }

    </script>

    <script type="text/javascript" language="javascript">
        function onCalendarShown(sender, args) {
            sender._switchMode("years", true);
            sender._switchMode("months", true);

            //sender._switchMode("day", false);            
        }

        $(document).ready(function()
        {
          $("#<%=txtFreqDTRFromDate.ClientID%>").datepicker(
        {
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            maxDate: 0,
        })

            $('#<%=txtToDate%>')
        }
            )
    </script>


    


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <h3 class="page-title">DTC Failure Report
                    </h3>
                    <a style="float: right!important; margin-right: -372px!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>

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
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='DTCFailureReport.aspx'; return false;"
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
                                <i class="icon-reorder"></i>Pending / Completed Failure Details </h4>
                            <a href="#" data-toggle="modal" data-target="#myModal1" title="Click For Help"></a>
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
                                                    From Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <%--<ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    To Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <%--<ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTC Make</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Failure Reason</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFailureType" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Gurantee Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbGrntyType" runat="server" TabIndex="1">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                            <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                            <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label id="Label3" class="control-label" runat="server">Report Type <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbReportType" runat="server" TabIndex="1">
                                                            <asp:ListItem Value="1">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="2">Failure DateWise</asp:ListItem>
                                                            <asp:ListItem Value="3">DateWise at each Stage </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label id="Label5" class="control-label" runat="server">Failure Type <span class="Mandotary">*</span></label>
                                                <div style="margin-top: -25px;width: 36%; margin-left: 35%;"
                                                    class="controls">
                                                    <div class="input-append">

                                                        <asp:CheckBoxList ID="checkboxlist1" runat="server">
                                                            <asp:ListItem Value="1">Failure</asp:ListItem>
                                                            <asp:ListItem Value="4">Failure Enhancement</asp:ListItem>
                                                            <asp:ListItem Value="2">Good Enhancement</asp:ListItem>
                                                              <asp:ListItem Value="5">Good Reduction</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle</label>
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
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Sub Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbSubDiv_SelectedIndexChanged">
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
                                                    Seclect Capacity</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity1" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Select Stage <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStage" runat="server" TabIndex="1">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="1">TC Failed</asp:ListItem>
                                                            <asp:ListItem Value="2">WorkOrder Pending</asp:ListItem>
                                                            <asp:ListItem Value="3">Indent Pending</asp:ListItem>
                                                            <asp:ListItem Value="4">Invoice Pending</asp:ListItem>
                                                            <asp:ListItem Value="5">Return Invoice Pending</asp:ListItem>
                                                            <asp:ListItem Value="6">RV  Pending</asp:ListItem>
                                                            <asp:ListItem Value="7">CR Pending</asp:ListItem>
                                                            <asp:ListItem Value="8">CR Completed</asp:ListItem>
                                                            <asp:ListItem Value="9">All</asp:ListItem>
                                                            <asp:ListItem Value="10">Detailed Failure Report</asp:ListItem>
                                                           <asp:ListItem Value="11">Deleted Failure Report</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="space20">
                                        </div>
                                        <div style="padding-left: 160px;">
                                            <asp:RadioButtonList ID="rdbReportType" runat="server" CssClass="radio" RepeatDirection="Horizontal" Width="900px">
                                                <asp:ListItem Value="1" Selected="True">TC Failed</asp:ListItem>
                                                <asp:ListItem Value="2">WorkOrdered</asp:ListItem>
                                                <asp:ListItem Value="3">Indent</asp:ListItem>
                                                <asp:ListItem Value="4">Invoice</asp:ListItem>
                                                <asp:ListItem Value="5">Decommission</asp:ListItem>
                                                <asp:ListItem Value="6">Return Invoice</asp:ListItem>
                                                <asp:ListItem Value="7">CR Pending</asp:ListItem>
                                                <asp:ListItem Value="8">CR Completed</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="span5">--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span12">
                                <div class="text-center" align="center">


                                    <asp:Button ID="cmdGenerate" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyForm('PDF')"
                                        CssClass="btn btn-primary" TabIndex="10" OnClick="cmdGenerate_Click" />

                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                        TabIndex="11" OnClick="cmdReset_Click" /><br />
                                    <br />
                                    <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info" OnClientClick="javascript:return ValidateMyForm('EXCEL')"
                                        TabIndex="12" OnClick="Export_click" /><br />

                                    <div class="span7">
                                    </div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
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


        <!-- need to uncomment tomo and add this  -->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DTR WORK ORDER REG DETAILS</h4>
                        <a href="#" style="float: right!important; color: #fff!important; padding: 8px 7px 0 0" data-toggle="modal" data-target="#myModal2" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;"></a></span>
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
                                                From Date <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFromDate2" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                   <%-- <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFromDate2" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>--%>
                                                </div>
                                            </div>



                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Circle</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCircle2" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbCircle1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDiv2" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbDiv1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                To Date <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtToDate2" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                    <%--<ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate2" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Sub Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSubDiv2" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbSubDiv1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                O & M Section</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSection" runat="server" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span5">
                            </div>
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="text-center" align="center">


                                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyForm2()"
                                            CssClass="btn btn-primary" TabIndex="10" OnClick="btnGenerate_Click" />

                                        <asp:Button ID="BtnWOReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" TabIndex="11" OnClick="BtnWOReset_Click" /><br />
                                        <br />
                                        <asp:Button ID="Button2" runat="server" Text="Export Excel" CssClass="btn btn-info" OnClientClick="javascript:return ValidateMyForm2()"
                                            TabIndex="12" OnClick="Export_clickworkorder" /><br />

                                        <div class="span7">
                                        </div>
                                        <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>

        <div id="invisible" runat="server" visible="false">
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Failure Abstract</h4>
                        <a href="#" style="float: right!important; padding: 8px 7px 0 0px !important; color: #fff" data-toggle="modal" data-target="#myModal3" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;"></a></span>
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
                                                Select Month <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">



                                                    <asp:TextBox ID="txtFailMonth" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="dtPickerFromDate" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFailMonth" DefaultView="Months" Format="yyyy-MM" OnClientShown="onCalendarShown">
                                                    </ajax:CalendarExtender>


                                                    <%--<ajax:CalendarExtender  runat="server" CssClass="calendarClass" 
                                                                Enabled="true" Format="MMM-yy" PopupButtonID="imgcalendarFileDate" 
                                                                TargetControlID="txtFailMonth" DefaultView="Months">
                                                            </asp:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">

                                        <div class="control-group">
                                            <asp:Label ID="lblReportType" class="control-label" runat="server">Select Report Type<span class="Mandotary">*</span></asp:Label>

                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbAbstract_ReportType" runat="server" TabIndex="1">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">Till Month Transaction</asp:ListItem>
                                                        <%--ALL--%>
                                                        <asp:ListItem Value="2"> On Month Transaction</asp:ListItem>
                                                        <%--Selected Month--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="span5">
                            </div>
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="text-center" align="center">


                                        <asp:Button ID="btnFailGenerate" runat="server" Text="Generate"
                                            CssClass="btn btn-primary" TabIndex="10" OnClick="btnFailGenerate_Click" OnClientClick="javascript:return ValidateMyForm1()" />

                                        <asp:Button ID="btnFailreset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" TabIndex="11" OnClick="btnFailreset_Click" /><br />
                                        <br />
                                        <asp:Button ID="Button5" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                            TabIndex="12" OnClick="Export_clickFailure" OnClientClick="javascript:return ValidateMyForm1()" /><br />

                                        <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
            </div>

        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>FREQUENTLY FAILING DTC'S</h4>
                        <a href="#" style="float: right!important; color: #fff; padding: 8px 7px 0 0px !important" data-toggle="modal" data-target="#myModal4" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;"></a></span>
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
                                                From Date <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFromDate3" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                    <%--<ajax:CalendarExtender ID="CalendarExtender5" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFromDate3" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>--%>
                                                </div>
                                            </div>



                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTCCode" maxlength="6"  onkeypress="return AllowalphabetsNumber(event)" runat="server"  TabIndex="5"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Failure Type</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbFailureType1" runat="server" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Circle</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCircle3" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbCircle3_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDivision3" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbDivision3_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                To Date <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtToDate3" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                   <%-- <ajax:CalendarExtender ID="CalendarExtender6" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate3" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                DTR Code <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTRCode" maxlength="6"  onkeypress="return AllowNumbersOnly(event)"  runat="server"  TabIndex="5"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Gurantee Type</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbguranteetype1" runat="server" TabIndex="1">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                        <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Sub Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSubDivision3" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbSubDivision3_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                O & M Section</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbOMSection3" runat="server" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span5">
                            </div>
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="form-horizontal" align="center">


                                        <asp:Button ID="Button3" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyForm2()"
                                            CssClass="btn btn-primary" TabIndex="10" OnClick="btnGenerate3_Click" />

                                        <asp:Button ID="Button4" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" TabIndex="11" OnClick="BtnReset3_Click" /><br />
                                        <br />
                                        <asp:Button ID="Button6" runat="server" Text="Export Excel" CssClass="btn btn-info" OnClientClick="javascript:return ValidateMyForm2()"
                                            TabIndex="12" OnClick="Export_click3" /><br />

                                        <asp:Label ID="Label6" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->

        <!--Start of Frequent Failing DTR's -->

        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>FREQUENTLY FAILING DTR'S</h4>
                        <a href="#" style="float: right!important; color: #fff; padding: 8px 7px 0 0px !important" data-toggle="modal" data-target="#myModal4" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;"></a></span>
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
                                                From Date <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFreqDTRFromDate" runat="server" MaxLength="10" TabIndex="5" autocomplete="off"></asp:TextBox>
                                                    <%--<ajax:CalendarExtender ID="CalendarExtender7" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFreqDTRFromDate" Format="dd/MM/yyyy">--%>
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFreqDTRDTCCode"    onkeypress="return AllowalphabetsNumber(event)" runat="server" maxlength="6"  TabIndex="5"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Failure Type</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdFreqDTRFailType" runat="server" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Circle</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdFreqDTRCircle" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmdFreqDTRCircle_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdFreqDTRDivision" runat="server" AutoPostBack="true" TabIndex="1" 
                                                        OnSelectedIndexChanged="cmdFreqDTRDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                To Date <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFreqDTRToDate" runat="server" MaxLength="10" TabIndex="5" autocomplete="off"></asp:TextBox>
                                                  <%--  <ajax:CalendarExtender ID="CalendarExtender8" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtFreqDTRToDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                DTR Code <%--<span class="Mandotary">*</span>--%></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFreqDTRDTRCode" maxlength="6"  onkeypress="return AllowNumbersOnly(event)" runat="server"  TabIndex="5"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Gurantee Type</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdFreqDTRGuaranteeType" runat="server" TabIndex="1">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                        <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Sub Division</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdFreqDTRSubDivision" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmdFreqDTRSubDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                O & M Section</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdFreqDTROMSection" runat="server" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span5">
                            </div>
                            <div class="row-fluid">
                                <div class="span12">
                                    <div class="form-horizontal" align="center">


                                        <asp:Button ID="Button7" runat="server" Text="Generate" OnClientClick="javascript:return ValidateMyForm2()"
                                            CssClass="btn btn-primary" TabIndex="10" OnClick="btnFreqDTRGenerate_Click" />

                                        <asp:Button ID="Button8" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" TabIndex="11" OnClick="BtnFreqDTRReset_Click" /><br />
                                        <br />
                                        <asp:Button ID="Button9" runat="server"  Text="Export Excel" CssClass="btn btn-info" OnClientClick="javascript:return ValidateMyForm2()"
                                            TabIndex="12" OnClick="BtnFreqDTRExcel_click3" /><br />

                                        <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>

        <!-- End Of Frequent Failing DTR's -->





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
                        <li>This Form include 4 Reports</li>
                        <li>Click <span style="font-size: 20px; font-weight: bold">!</span> in each Section to know more
                        </li>
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

    <!-- MODAL-->
    <div class="modal fade" id="myModal1" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul>
                        <li>This Report Will Display Pending / Completed Failure Details</li>
                        <li>You Can Take Report by selecting FromDate or ToDate or Make or FailureType 
                        or Guarentee Type or Capacity or  Circle or Division or Subdivision or Section
                 But Report Type And Select Stage is Mandatory</li>
                        <li>In Select Stage you will find Different Stages , you can select any stage and can view the report.</li>
                        <li>If you Select FailureDateWise in ReportType then It will Display Records that 
                        had got Failed between provided from date and to date and also completed the different stage in that date as well 
                        </li>
                        <li>If you Select DateWiseAt Eact Stage in ReportType then It will Display Records
                         between provided from date and to date in the different stage , it may be Failure in any date 
                        </li>
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

    <!-- MODAL-->
    <div class="modal fade" id="myModal2" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">

                    <ul>
                        <li>This Report Will Display Pending Failure Details</li>
                        <li>It may be in Any stage but Work order has to be complete</li>
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

    <!-- MODAL-->
    <div class="modal fade" id="myModal3" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul>
                        <li>This Report Will Display DTC Failure Abstract for Selected Month</li>
                        <li>You must Select any Month</li>
                        <li>You must Select Report Type As well, there is 2 types in it</li>
                        <li>1st Till Month Transaction will display the failure records till Selected month</li>
                        <li>2nd On Month Transaction will display the failure records for only Selected month
                        </li>
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

    <!-- MODAL-->
    <div class="modal fade" id="myModal4" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul>
                        <li>This Report Will Display DTC's that has failed >= 2 times</li>
                        <li>In this you can get report by selecting FromDate or ToDate or dtc code, tc code
                        </li>
                    </ul>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="myModal5" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul>
                        <li>This Report Will Display DTR's that has failed >= 2 times</li>
                        <li>In this you can get report by selecting FromDate or ToDate or dtc code, tc code
                        </li>
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

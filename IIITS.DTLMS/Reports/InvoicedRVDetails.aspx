<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/DTLMS.Master" CodeBehind="InvoicedRVDetails.aspx.cs" Inherits="IIITS.DTLMS.Reports.DTRFailureReport" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<script type="text/javascript">
        $(function () {
            $('[id*=failureTypeBox]').multiselect({
                includeSelectAllOption: true
            });
        });

    </script>--%>


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
       
       $(document).ready(function () {
   
   

    $("#<%= txtFromDate.ClientID%>").datepicker(
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


});
        function AllowalphabetsNumber(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if ((charCode >= 65 && charCode <= 90) || (charCode >= 48 && charCode <= 57))
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
                    <h3 class="page-title"> Invoiced and RV Details 
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
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='DTRFailureReport.aspx'; return false;"
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
                                <i class="icon-reorder"></i>Invoice and RV Details  </h4>
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
                                                  Invoiced/RV From Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" ></asp:TextBox>
                                                        <%--<ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="control-group">
                                                <label class="control-label">
                                                    Circle</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" 
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
                                                        <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true" 
                                                            OnSelectedIndexChanged="cmbSubDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">
                                                    Report Type  <span class="Mandotary">*</span> </label>
                                                 
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmdReportType" runat="server" AutoPostBack="true" >
                                                              <asp:ListItem Text ="--SELECT--" Value ="0"> </asp:ListItem>
                                                            <asp:ListItem Text ="Invoiced" Value ="1"> </asp:ListItem>
                                                            <asp:ListItem Text ="RV" Value ="2"  > </asp:ListItem>

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                           
                                         </div>
                                        <div class="span5">
                                           <div class="control-group">
                                                <label class="control-label">
                                                    Invoiced/RV To Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" ></asp:TextBox>
                                                       <%-- <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="cmbDiv_SelectedIndexChanged" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                           
                                            <div class="control-group">
                                                <label class="control-label">
                                                    O & M Section</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbOMSection" runat="server" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--<div class="control-group">
                                                <label class="control-label">
                                                    Seclect Capacity</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity1" runat="server" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>--%>

                                        </div>
                                       
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row-fluid">
                            <div class="span12">
                                <div class="text-center" align="center">


                                    <asp:Button ID="cmdGenerate" runat="server" Text="Generate" 
                                        CssClass="btn btn-primary" OnClick="cmdGenerate_Click" />

                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                        TabIndex="11" OnClick="cmdReset_Click" /><br />
                                    <br />
                                    <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info" 
                                        TabIndex="12" OnClick="Export_click"  /><br />

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
                        <li>This Report Will Display Invoiced and RV Details of the Transformers issued</li>
                        <li>You Can Take Report by selecting FromDate or ToDate and Circle till Section  </li>
                         <li> Only Work orders that have been invoiced and decommission approved by SDO will be shown  </li>
                           <li>  Work orders that have been pending for RV and CR will be shown  </li>
                        <li> From Date and To Date condition is based on Failure Date   </li>
                         <li> Cannot compare the count with Dashboard </li>
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




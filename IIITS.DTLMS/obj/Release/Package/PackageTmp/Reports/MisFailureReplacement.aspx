<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="MisFailureReplacement.aspx.cs" Inherits="IIITS.DTLMS.Reports.MisFailureReplacement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        label {
            display: -webkit-inline-box !important;
        }

        .ui-datepicker-calendar {
    display: none;
    }
    </style>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

    <script type="text/javascript">
        <%--$(document).ready(function () {
            $("#<%=selectedMonth.ClientID%>").datepicker(
        {
            dateFormat: 'M-yy',
            changeMonth: true,
            changeYear: true,
            changeDate: false,
            maxDate: 0,
        })
        })--%>

        $(document).ready(function () {
            $("#<%=txtFromDate.ClientID%>").datepicker(
        {
           
            changeMonth: true,
            changeYear: true ,
            changeDate: false,
            maxDate: 0,
            showButtonPanel: true,
            dateFormat: 'M-yy',
            onClose: function(dateText, inst) { 
                $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            },

            onSelect: function (selected) {
                $("#<%=txtFromDate.ClientID%>").datepicker("option", "minDate", selected);
            } ,
        })
        })

        $(document).ready(function () {
            $("#<%=txtToDate.ClientID%>").datepicker(
        {
            dateFormat: 'M-yy',
            changeMonth: true,
            changeYear: true,
            changeDate: false,
            showButtonPanel: true,
            maxDate: 0,
            onClose: function (dateText, inst) {
                $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            onSelect

             }, function (selected) {
                 $("#<%=txtToDate.ClientID%>").datepicker("option", "maxDate", selected);
            } ,
            
        })
        })


     function   ValidateForm()
     {
         debugger;
         var startDate = new Date($("#<%=txtFromDate.ClientID%>").val());
         var endDate = new Date($("#<%=txtToDate.ClientID%>").val());

         if (endDate < startDate) {
             alert("FromDate cannot be greater than ToDate.");
             return false;
         }
         else {
             return true;
         }

        }
    </script>

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">TRANSFORMER FAILURE / REPLACEMENT (AGP/WGP)
                </h3>
                <ul class="breadcrumb" style="display: none">
                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text">
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
                <%-- <asp:Button ID="Button1" runat="server" Text="Store View" 
                                      OnClientClick="javascript:window.location.href='StoreView.aspx'; return false;"
                            CssClass="btn btn-primary" />--%>
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
                            <i class="icon-reorder"></i>TRANSFORMER FAILURE / REPLACEMENT (AGP/WGP)</h4>
                        <a href="#" style="float: right!important; padding: 8px 7px 0 0px !important; color: #fff!important" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;"></a><a href="javascript:;"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>

                                    <%-- another span--%>

                                    <div class="span1">
                                    </div>
                                    <div class="span5">
                                        <%-- <div class="control-group">
                                                <label class="control-label">
                                                    From Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate1" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Group By  <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="groupBy" runat="server" AutoPostBack="true" TabIndex="1">
                                                        <asp:ListItem Text="--Select--" Value="0"> </asp:ListItem>
                                                        <asp:ListItem Text="Division" Value="2"> </asp:ListItem>
                                                        <asp:ListItem Text="Subdivision" Value="3"> </asp:ListItem>
                                                        <asp:ListItem Text="Section" Value="4"> </asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- <div class="control-group" >
                                                <label class="control-label"> Guaranty Type <span class="Mandotary">*</span></label>
                                                <div class="input-append" >
                                                        <asp:CheckBoxList ID="guarantyType" runat="server" >
                                                         <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                          <asp:ListItem Value="WGP">WGP</asp:ListItem> 
                                                         <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                       </asp:CheckBoxList>
                                                </div>
                                            </div>--%>
                                    </div>
                                    <div class="span5">
                                        <%-- <div class="control-group">
                                                <label class="control-label">
                                                    To Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate1" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate1" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>--%>

                                       <%-- <div class="control-group"  >
                                            <label class="control-label">
                                                Month <span class="Mandotary">*</span>
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="selectedMonth" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                     <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1" 
                                                            TargetControlID="selectedMonth" Format="MMM-yyyy">
                                                        </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>--%>


                                        <div class="control-group">
                                            <label class="control-label">
                                                From Date 
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFromDate" class="date-picker"  runat="server" MaxLength="10" TabIndex="5"  AutoComplete="off"></asp:TextBox>
                                                   <%-- <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                To Date 
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="5" AutoComplete="off"></asp:TextBox>
                                                   <%-- <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </div>




                                        <div id="Div1" class="control-group" runat="server" visible="false">
                                            <label class="control-label">
                                                To Date 
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtToDate1" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                        TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                    </ajax:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="space20">
                                </div>
                                <div class="space20">
                                </div>

                                <div class="text-center" align="center">


                                    <asp:Button ID="cmdReport" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                     OnClientClick="javascript:return ValidateForm()"  OnClick="cmdReport_Click" />

                                    <asp:Button ID="Button3" runat="server" Text="Reset" CssClass="btn btn-danger"
                                        OnClick="BtnReset1_Click" /><br />
                                    <div class="span13"></div>

                                    <%-- <asp:Button ID="cmdExcel" runat="server" Text="Export To Excel" CssClass="btn btn-primary" OnClick="cmdReport_Click"/>
                                          <br />--%>


                                    <%--  <asp:Button ID="Button2" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                            onclick="Export_clickFailureAbstract" /><br />--%>


                                    <div class="span7">
                                    </div>
                                    <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="space20">
                        </div>
                        <!-- END FORM-->
                    </div>
                </div>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->

    </div>
    </div>

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
                        <li>This Report is used to View Abstract AGP / WGP / WRGP count of DTR that has been failed/invoiced</li>
                        <li>Report can be generated by Selecting  all the three fields that is  Month , Guaranty Type and Group By compulsory  .  </li>


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

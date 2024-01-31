<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="MisRepairCentre.aspx.cs" Inherits="IIITS.DTLMS.Reports.MisRepairCentre" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtMonth.ClientID%>").datepicker(
          {
              dateFormat: 'mmyy',
              changeMonth: true,
              changeYear: true,
              changeDate: false,
              maxDate: 0,
          })
            $("#<%=txtMonth1.ClientID%>").datepicker(
         {
             dateFormat: 'mmyy',
             changeMonth: true,
             changeYear: true,
             changeDate: false,
             maxDate: 0,
         })
            $("#<%=txtMonth3.ClientID%>").datepicker(
         {
             dateFormat: 'mmyy',
             changeMonth: true,
             changeYear: true,
             changeDate: false,
             maxDate: 0,
         })


        })
    </script>

    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">MIS Repairer Report
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
                            <i class="icon-reorder"></i>MIS REPAIR CENTRE </h4>
                        <a href="#" style="float: right!important; padding: 8px 7px 0 0px !important; color: #fff!important" data-toggle="modal" data-target="#myModal1" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px; color: white"></i></a>
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
                                        <div class="control-group">
                                            <label class="control-label">
                                                Circle
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCircle1" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbCircle1_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Division
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDiv1" runat="server" AutoPostBack="true" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Month 
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtMonth" runat="server" MaxLength="10" TabIndex="5" AutoComplete="Off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Financial Year 
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdFinancialYear" runat="server" AutoPostBack="true" TabIndex="1">
                                                    </asp:DropDownList>
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
                                        OnClick="cmdReport_Click" />

                                    <asp:Button ID="Button3" runat="server" Text="Reset" CssClass="btn btn-danger"
                                        OnClick="BtnReset1_Click" /><br />
                                    <br />
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


                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->

        </div>

        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>MIS REPAIR PERFORMANCE </h4>
                        <a href="#" style="float: right!important; padding: 8px 7px 0 0px !important; color: #fff!important" data-toggle="modal" data-target="#myModal2" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px; color: white"></i></a>
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
                                        <div class="control-group">
                                            <label class="control-label">
                                                Circle
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdPerformanceCircle" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmdPerformanceCircle_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Division
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdPerformanceDiv" runat="server" AutoPostBack="true" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Month 
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtMonth1" runat="server" MaxLength="10" TabIndex="5" AutoComplete="Off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Financial Year  
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdFinancialYear1" runat="server" AutoPostBack="true" TabIndex="1">
                                                    </asp:DropDownList>

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
                                    <asp:Button ID="btnGeneratePerformance" runat="server" Text="Generate Performance Report" CssClass="btn btn-primary"
                                        OnClick="cmdPerformanceReport_Click" />

                                    <asp:Button ID="btnResetPerformance" runat="server" Text="Reset" CssClass="btn btn-danger"
                                        OnClick="BtnPerformanceReset1_Click" /><br />
                                    <br />
                                    <div class="span7">
                                    </div>

                                </div>
                            </div>
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
                        <h4>
                            <i class="icon-reorder"></i>AGE-WISE PENDENCY DTR DETAILS WITH THE REPAIR</h4>
                        <a href="#" style="float: right!important; padding: 8px 7px 0 0px !important; color: #fff!important" data-toggle="modal" data-target="#myModal2" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px; color: white"></i></a>
                        <span class="tools"><a href="javascript:;"></a><a href="javascript:;"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span1">
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Circle
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbCircle3" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmdCircle3_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Division
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDivision3" runat="server" AutoPostBack="true" TabIndex="1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Month 
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtMonth3" runat="server" MaxLength="10" TabIndex="5" AutoComplete="Off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Financial Year 
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmdFinancialYear3" runat="server" AutoPostBack="true" TabIndex="1">
                                                    </asp:DropDownList>
                                                    <div class="span7">
                                                    </div>
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
                                    <asp:Button ID="BtnRepPending" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                        OnClick="cmdBtnRepPending_Click" />

                                    <asp:Button ID="BtnReset3" runat="server" Text="Reset" CssClass="btn btn-danger"
                                        OnClick="BtnReset3_Click" /><br />
                                    <br />
                                    <div class="span7">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

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
                                            <li>This Report is used to View Abstract OB and CB Count of DTR at Repair Centre  </li>
                                            <li>Report can be generated by Selecting Either Month or Financial Year </li>
                                            <li>Report can be generated for Month Wise as well as  Financial Year Wise  </li>
                                            <li>Report will be generated Starting from  2019 Financial Year  </li>
                                            <li>This Report cannot  be cross checked with other reports or in the Dashboard </li>
                                        </ul>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-danger" data-dismiss="modal">
                                            Close</button>
                                    </div>
                                </div>
                            </div>
                        </div>

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
                                            <li>This Report is used to View Abstract Repairer Performance Report  </li>
                                            <li>Report can be generated by Selecting  Financial Report  </li>
                                            <li>Report will be generated Starting from  2019 Financial Year  </li>
                                            <li>This Report cannot  be cross checked with other reports or in the Dashboard </li>
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

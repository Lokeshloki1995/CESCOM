<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="MisFaultyDTRs.aspx.cs" Inherits="IIITS.DTLMS.Reports.MisFaultyDTRs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                      MIS Faulty DTR At RepairCentre / Store / Field
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
                            CssClass="btn btn-primary" />--%></div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>MIS Faulty DTR At RepairCentre / Store /Field Report</h4> 
              <a href="#" style="float:right!important;padding:8px 7px 0 0px !important;color:#fff!important"data-toggle="modal" data-target="#myModal2" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px;color:white"></i></a> 
                            <span class="tools"><a href="javascript:;" ></a><a href="javascript:;"
                                ></a></span>
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
                                            <%--<div class="control-group">
                                                <label class="control-label">
                                                    Circle</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle1" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbCircle1_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>--%>
                                           <%-- <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv1" runat="server" AutoPostBack="true" TabIndex="1">
                                                            
                                                        </asp:DropDownList>
                                                    </div>
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
                                             <div class="control-group" visible="false" runat ="server">
                                                <label class="control-label">
                                                    From Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" visible="false" runat="server">
                                                <label class="control-label">
                                                    To Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
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
                                                OnClick="cmdReport_Click" />
                                      
                                                <asp:Button ID="Button3" runat="server" Text="Reset" Visible="false" CssClass="btn btn-danger" 
                                                     onclick="BtnReset1_Click" /><br />
                                          <br />

                                     
                                        <asp:Button ID="Button2" Visible="false" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                            onclick="Export_click" /><br />
                                         
                                        
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

     <div class="modal fade" id="myModal2" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">
                        Help</h4>
                </div>
                <div class="modal-body">
                   <ul><li>
                  This Report is user to View the Count of DTR's that are present in the  Repair Centre / Store / Field    </li>
                 
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

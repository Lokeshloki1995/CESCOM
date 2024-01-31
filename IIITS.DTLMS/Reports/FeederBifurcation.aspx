<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FeederBifurcation.aspx.cs" Inherits="IIITS.DTLMS.Reports.FeederBifurcation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

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
                    <h3 class="page-title">
                       Feeder Bifurcation
                    </h3>                    
                     
                     <a style="float:right!important;margin-right:-372px!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>     
                    
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
                                <i class="icon-reorder"></i>Feeder Bifurcation</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Old Feeder Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbOldFeederCode" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Circle </label>
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
                                                    Enumeration Status<span class="Mandotary">*</span> </label> 
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbEnumerationStatus" runat="server" TabIndex="1">
                                                            <asp:ListItem Text="--SELECT--"> </asp:ListItem>  
                                                            <asp:ListItem Text ="Enumeration Pending" Value="0"> </asp:ListItem>
                                                            <asp:ListItem Text= "Enumeration Completed" Value="1"> </asp:ListItem>
                                                            <asp:ListItem Text= "Enumeration Pending and Completed" Value="0,1"></asp:ListItem> 

                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <%-- another span--%>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    New Feeder Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%--<asp:TextBox ID="txtTcCapacity" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                        MaxLength="10" ></asp:TextBox>--%>
                                                        <asp:DropDownList ID="cmbNewFeederCode" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
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
                                                  OM From Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                   OM To Date <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span1">
                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="text-center" align="center">
                                      
                                      
                                            <asp:Button ID="cmdReport" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                                OnClick="cmdReport_Click" />
                                       
                                               <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                            CssClass="btn btn-danger" onclick="cmdReset_Click" /><br />
                                       <br />
                                <asp:Button ID="Button1" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                     TabIndex="12" OnClick="Export_click" /><br />
                                              
                                        <%-- <div class="span1"></div>--%>
                                        
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


    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">
                        Help</h4>
                </div>
                <div class="modal-body"><ul><li>
                  This Report is used to View Feeder that has been Bifurcated </li>
                  <li>You Can Take Report by selecting Old  Feeder or New Feeder Code or Circle or Division or Subdivision or Section </li>
                 <li> you can Take Full Report by seclecting nothing in the form </li>
                 <li> By Clicking Generate Report Button crystal Report will be Generate After that you can download it in to PDF file</li>
                  <li> By Clicking ExportExcel Button Excel will be downloaded

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

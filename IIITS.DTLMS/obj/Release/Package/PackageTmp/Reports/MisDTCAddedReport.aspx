<%@ Page Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="MisDTCAddedReport.aspx.cs" Inherits="IIITS.DTLMS.Reports.MisDTCAddedReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script  type="text/javascript">
        function ValidateMyForm() {
            if (document.getElementById('<%= cmbReportType.ClientID %>').value.trim() == "--Select--") {
                alert('Please Select ReportType')
                document.getElementById('<%= cmbReportType.ClientID %>').focus()
                return false
            }
        }


         
               
        $(document).ready(function ()
        {
            $("#<%= txtFromMonth.ClientID%>").datepicker(
                {
                    dateFormat: 'dd/mm/yy',
                    maxDate: 0,
                    changeYear: true,
                    changeMonth :true ,

                })

            $("#<%= txtToMonth.ClientID%>").datepicker(
                {
                    dateFormat: 'dd/mm/yy',
                    maxDate: 0,
                    changeYear: true,
                    changeMonth :true ,

                })
        })  
            


            </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid" >
            
            
            

            <div class="row-fluid">
                <div class="span12">
                     <h3 class="page-title">DTC Added Abstract Report
                    </h3>
<%--                      <a href="#" style="float:right!important;padding:8px 7px 0 0px !important;color:#fff!important" data-toggle="modal" data-target="#myModal1" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px;color:white"></i></a>--%>
                            <span class="tools"><a href="javascript:;" ></a><a href="javascript:;"
                                ></a></span>
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">

                            <h4>
                                <i class="icon-reorder"></i>Abstract DTC Added Report</h4>
                            <a href="#" style="float:right!important;padding:8px 7px 0 0px !important;color:#fff!important"data-toggle="modal" data-target="#myModal1" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px;color:white"></i></a>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <%--<div class="form-horizontal">
                                    <div class="row-fluid">--%>
                                       <div class="container-fluid">
                                        <div class="span4">
                                            <div class="control-group">
                                                <label class="control-label"> ReportType <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbReportType" runat="server">
                                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>

                                                            <asp:ListItem Value="1" Text="Capacity">    </asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Workwise">     </asp:ListItem>
                                                            <asp:ListItem Value="3" Text="FeederType/Category">   </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span4">
                                            <div class="control-group">
                                                <label class="control-label">From</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromMonth" runat="server" MaxLength="10" AutoComplete="Off" ></asp:TextBox>
                                                       <%-- <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromMonth" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span4">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    To</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToMonth" runat="server" MaxLength="10" TabIndex="5" AutoComplete="Off"></asp:TextBox>
                                                      <%--  <ajax:CalendarExtender ID="CalendarExtender5" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToMonth" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
</div>
                                    </div>
                                </div>
                         <%--   </div>
                            <!-- END FORM-->
                        </div>--%>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="text-center" align="center">


                        <asp:Button ID="cmdReport" runat="server" Text="Report" 
                            CssClass="btn btn-primary" TabIndex="10" OnClick="cmdReport_Click" OnClientClick="javascript:return ValidateMyForm()" />

                        <asp:Button ID="cmbReset1" runat="server" Text="Reset" CssClass="btn btn-danger"
                            TabIndex="11"   OnClick="cmdReset_Click1"   /><br />
                      
                        <br />
                        <asp:Button ID="cmbExport" Visible="false" runat="server" Text="Export" CssClass="btn btn-info"
                            TabIndex="12"  /><br /><%--OnClick="cmbExport_click"--%>

                        <div class="span7">
                        </div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>

    <!-- MODAL-->
   <%-- <div class="modal fade" id="myModal1" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul>
                        <li>This Report Will Display DTC Records Added based on FromDate, ToDate, Feeder Type, Capacity</li>
                        <li>Select Radio button Division Wise or Sub Division Wise</p>
                        </li>
                    </ul>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>--%>

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
                        <li>This Report Will Display Abstract Count of DTC Added based on FromDate, ToDate, Feeder Type, Capacity and WorkWise </li>
                        <li> Please Select Report Type as its Mandatory</p>
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
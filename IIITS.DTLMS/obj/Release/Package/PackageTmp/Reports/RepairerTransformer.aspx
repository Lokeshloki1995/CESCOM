<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="RepairerTransformer.aspx.cs" Inherits="IIITS.DTLMS.Reports.RepairerTransformer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../Scripts/functions.js" type="text/javascript"></script>
    <script  type="text/javascript">
        $(document).ready(function ()
        {
            $("#<%=txtFromDate.ClientID  %>").datepicker(
                {
                    dateFormat: 'dd/mm/yy',
                    maxDate: 0,
                    changeYear : true,
                    changeMonth: true, 
                })

             $("#<%=txtToDate.ClientID  %>").datepicker(
                {
                    dateFormat: 'dd/mm/yy',
                    maxDate: 0,
                    changeYear : true,
                    changeMonth: true, 
                })

        })


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
   Repairer Performance Report
                   </h3>
 <a href="#" style="float:right!important;margin-right:-372px!important"data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>
                               </div>
                           </form>
                       </li>
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->
               </div>
                <div style="float:right;margin-top:20px;margin-right:12px" >           
              </div>
                </div>
                 <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Select Location</h4>
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
                                                    From Date </label>
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
                                                    Capacity </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                          
                                        </div>
                                        <div class="span5">
                                        
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
                                                    To Date </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                        <%--<ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>

                  
                                        </div>
                                       
                                    </div>
                                </div>
                            </div>
                         
                            <!-- END FORM-->
                        </div>
                    </div>
                    </div>
                    </div>



                        <div class="row-fluid">
                        <div class="span12">

                        <div class="widget blue">
                        <div class="widget-title">
                         <h4> <i class="icon-reorder"></i>Report Type </h4>
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
                        <label class="control-label">Report Type<span class="Mandotary"> *</span></label>
                        
                        <div class="controls">
                        <asp:DropDownList ID="cmbReportType" runat="server" CssClass="input_select" >
                        <asp:ListItem Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Pending Analysis Report" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Delivered Analysis Report" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                        </div>
                        </div>

                          <div class="span5"> 
                         <div class="control-group">
                        <label class="control-label">Reprier Name</label>

                        <div class="controls">
                        <asp:DropDownList ID="cmbRepairerName" runat="server" CssClass="input_select" 
                        AutoPostBack ="true">
                        </asp:DropDownList>
                        </div>
                        </div>
                        </div>


                        </div>
                        </div>
                        <!-- END SAMPLE FORM PORTLET-->
                        </div>
                        </div>
          
                        <!-- END PAGE CONTENT-->
                        </div>

                       

                        </div>
                        </div>

                           <div class="row-fluid">
                                <div class="span12">
                                    <div class="text-center" align="center">
                                       
                                     
                                            <asp:Button ID="cmdGenerate" runat="server" Text="Generate" 
                                                CssClass="btn btn-primary" TabIndex="10" onclick="cmdGenerate_Click" />
                                    
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                                TabIndex="11" onclick="cmdReset_Click"/><br />
                                     <br />
                                                <asp:Button ID="Button1" runat="server" Text="Export to Excel"  CssClass="btn btn-info" 
                                                    TabIndex="12" onclick="Export_click" /><br />
                                          
                                        <div class="span7">
                                      
                                </div>
                            </div>
                              </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>




                    <!-- END SAMPLE FORM PORTLET-->
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
                <div class="modal-body">
                    <ul><li>
                     This Report Will Display Repairer Performance</li>
                  <li> Select Circle, division, FromDate, ToDate,capacity </li>
                  <li>  Select Report Type , there is 2 options in it </li>
                 <li>   If you select Pending Analysis report. Pending TC's at Repairer records will display </li>
                  <li>  If you select Delivered Analysis report. Delevered TC's Repairer records will display 
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


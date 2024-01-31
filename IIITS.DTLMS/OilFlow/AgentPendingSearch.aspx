<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="AgentPendingSearch.aspx.cs" Inherits="IIITS.DTLMS.OilFlow.AgentPendingSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

    </script>

    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ascending th a {
            background: url(/img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(/img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(/img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }
    </style>
    <script type="text/javascript">
        //Purchase Order allow to search chars, nums and -/
        function characterAndnumbersPo(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 47) {

                return false;
            }
            return true;
        }
        function onlynumbers(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode < 45 || charCode > 57) {

                return false;
            }
            return true;
        }
        // Purchase Order allow Chatractes and Numbers to paste
        function cleanSpecialCharsPo(t) {

            //t.value = t.value.toString().replace(/[^-/a-zA-Z 0-9\n\r]+/g, '');
            //alert(" Special charactes are not allowed!");
        }

    </script>
    <%--        <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=txtPodate.ClientID%>').datepicker(
                {
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    maxDate: 0
                }
                )

            $('#<%=txtinvoicedate.ClientID%>').datepicker(
                {
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    maxDate: 0
                })


        }

        )



    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </ajax:ToolkitScriptManager>--%>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>


    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Oil Purchase Order        
                    <div style="float: right; margin-top: 10px; margin-right: -380px">
                        <asp:Button ID="cmdclose" runat="server" Text="Close" OnClientClick="javascript:return ValidateMyForm()"
                            CssClass="btn btn-primary" OnClick="cmdClose_Click" />
                    </div>


                    </h3>

                    <a style="margin-right: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Oil Purchase Order   </h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <asp:Panel ID="pnlApproval" runat="server">
                                    </asp:Panel>
                                    <div class="row-fluid">
                                        <div class="span1"></div>
                                        <div class="control-group">

                                            <div class="span5">
                                                <asp:RadioButton ID="rbdNewPo" runat="server" Text="New Purchase Order" CssClass="radio"
                                                    GroupName="a" AutoPostBack="true" Checked="true" OnCheckedChanged="rbdNewPo_CheckedChanged" />
                                            </div>
                                            <%--                                            <div class="span5">
                                               <asp:RadioButton ID ="rbdOldPo" runat ="server" Text="Already Sent Purchase Order" CssClass="radio"
                                                   GroupName="a" AutoPostBack="true" OnCheckedChanged="rbdOldPo_CheckedChanged"/>
                                           </div>--%>
                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Purchase Order No <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPoNo" runat="server" MaxLength="30" onkeypress="return characterAndnumbersPo(event)" onchange="return cleanSpecialCharsPo(this)"></asp:TextBox>
                                                        <%-- <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary" 
                                                      TabIndex="2"  /> --%>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">PO Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPodate" runat="server" MaxLength="10" Text='<%# System.DateTime.Now%>'></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtPodate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                        <%--                                                         <asp:CalendarExtender ID="Podate_CalendarExtender1" runat="server" CssClass="cal_Theme1" 
                                                           TargetControlID="txtPodate"  Format="dd/MM/yyyy"></asp:CalendarExtender>--%>
                                                        <%--  <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtPodate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                        <%-- <asp:Calendar ID="txtPodate_CalendarExtender1" runat="server"
            onselectionchanged="Calendar1_SelectionChanged" Visible="true"> </asp:Calendar>--%>

                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdftotalqty" runat="server" />
                                                        <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />

                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Visible="false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Oil Quantity(in Kltr)<span class="Mandotary"> *</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtquantity" runat="server" onkeypress="return onlynumbers(event)" MaxLength="10" AutoPostBack="true"
                                                            OnTextChanged="txtqty_SelectedIndexChanged"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Oil Quantity(in Ltr) </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtoilquantityltr" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Amount<span class="Mandotary"> *</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtamt" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--                                <asp:Label ID="Label1" runat="server"  Font-Bold="true"  
                                        Font-Size="Medium">Total Quantity Available :  </asp:Label>
                                           <u>   <asp:HyperLink Target="_blank" runat="server" ID="totalquantity" CssClass="btn-link" Font-Size="Large"></asp:HyperLink> </u>--%>

                                            <asp:Label ID="lbltotalquantity" runat="server" ForeColor="Green" Font-Size="Medium"></asp:Label>
                                            <asp:Label ID="lblTotalBudget" runat="server" ForeColor="Green" Font-Size="Medium" Visible="false"></asp:Label>
                                            <br />
                                            <asp:Label ID="lblTotalAmt" runat="server" ForeColor="Green" Font-Size="Medium" Visible="false"></asp:Label><br />
                                            <asp:Label ID="lblAvailablebudget" runat="server" ForeColor="Green" Font-Size="Medium" Visible="false"></asp:Label>
                                            <br />

                                        </div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Item Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbOilType" runat="server" AutoPostBack="true" TabIndex="2">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" id="dvinvoice" runat="server" style="display: none">
                                                <label class="control-label">Invoice No </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtinvoiceno" runat="server" MaxLength="20"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--                                         <div class="control-group">
                                            <label class="control-label">Select Old PO NO</label>
                                            
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                         <asp:TextBox ID="txtPonum" runat="server" MaxLength="10" TabIndex="12" ></asp:TextBox>
                                                          <asp:Button ID="cmdSearchPO" Text="S" class="btn btn-primary" runat="server" 
                                                           CausesValidation="false" TabIndex="13" onclick="cmdSearchPO_Click"  />
                                                    </div>
                                                </div>
                                        </div>--%>

                                            <div class="control-group">
                                                <label class="control-label">Invoice Date <span class="Mandotary">*</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtinvoicedate" runat="server" MaxLength="10" Text='<%# System.DateTime.Now%>'></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtinvoicedate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                        <%--                                                        <asp:CalendarExtender ID="txtinvoicedate_CalendarExtender2" runat="server" CssClass="cal_Theme1" 
                                                           TargetControlID="txtinvoicedate"  Format="dd/MM/yyyy"></asp:CalendarExtender>--%>
                                                        <%--<ajax:CalendarExtender ID="txtinvoicedate_CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtinvoicedate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" TabIndex="2">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Agency Name <span class="Mandotary">*</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbAgency" runat="server" AutoPostBack="true" TabIndex="2">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row-fluid" runat="server" id="dvhead" style="display: none">
                                                <div class="control-group">
                                                    <label class="control-label">Account Head <span class="Mandotary">*</span> </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbBudgethead" runat="server" AutoPostBack="true" TabIndex="2">
                                                            </asp:DropDownList>
                                                            <br />
                                                            <asp:LinkButton ID="lnkBudgetstat" runat="server" Visible="false"
                                                                Style="font-size: 12px; color: Blue" OnClick="lnkBudgetstat_Click">View Budget Status</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span1">
                                            </div>

                                            <%--                        OnSelectedIndexChanged="cmbdiv_SelectedIndexChanged"--%>
                                            <%--CssClass="btn btn-primary" --%>
                                        </div>

                                    </div>
                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="text-center" align="center">

                                        <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return ValidateMyForm()"
                                            CssClass="btn btn-success" OnClick="cmdSave_Click" />

                                        <%--                    <div style="margin-left:-12px!important"class="span1">--%>
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="cmdReset_Click" />
                                        <%--</div>--%>
                                        <%--             <asp:Button ID="ViewReport" runat="server" Text="View Purchase Order"  OnClientClick="javascript:return ValidateMyForm()"
                            CssClass="btn btn-success" onclick="cmdReport_click" />--%>

                                        <%--       <div class="span7"></div>--%>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->

                        </div>
                    </div>

                    <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                        <div class="span12">
                            <!-- BEGIN SAMPLE FORMPORTLET-->
                            <div class="widget blue">
                                <div class="widget-title">
                                    <h4><i class="icon-reorder"></i>Comments for Approve</h4>
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
                                                <div class="span1"></div>
                                                <div class="span5">

                                                    <div class="control-group">
                                                        <label class="control-label">Comments<span class="Mandotary"> *</span></label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="4" TextMode="MultiLine" onchange="return CleanSpecialSymbols(this)"
                                                                    Width="550px" Height="125px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- END SAMPLE FORM PORTLET-->
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
                                <p style="color: Black">
                                    <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Save The Oil Purchase Order Details.
                                </p>
                                <p style="color: Black">
                                    <i class="fa fa-info-circle"></i>User Need To Enter  Purchase Order No in Purchase Order No Textbox
                                </p>
                                <p style="color: Black">
                                    <i class="fa fa-info-circle"></i>Once The Details Entered Please Click On <u>Save Button</u>
                                </p>

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

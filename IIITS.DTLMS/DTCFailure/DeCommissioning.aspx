<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="DeCommissioning.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.DeCommissioning" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">


        function ValidateMyForm() {

            if (document.getElementById('<%= txtFailureId.ClientID %>').value.trim() == "") {
                alert('Enter Valid Failure ID')
                document.getElementById('<%= txtFailureId.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDecommDate.ClientID %>').value.trim() == "") {
                alert('Enter Valid Decommission Date')
                document.getElementById('<%= txtDecommDate.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtRINo.ClientID %>').value.trim() == "") {
                alert('Enter Valid RI Number')
                document.getElementById('<%= txtRINo.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtRIDate.ClientID %>').value.trim() == "") {
                alert('Enter Valid RI Date')
                document.getElementById('<%= txtRIDate.ClientID %>').focus()
                return false

            }
            if (document.getElementById('<%= cmbStore.ClientID %>').value == "-Select-") {
                alert('Select Store')
                document.getElementById('<%= cmbStore.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtRemarks.ClientID %>').value.trim() == "") {
                alert('Enter Valid Remarks')
                document.getElementById('<%= txtRemarks.ClientID %>').focus()
                return false
            }
        }

        //From/Todate allow to enter nums /
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 47)
                return false;

            return true;
        }

        //From/Todate  allow only Numbers / to paste 
        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^/0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }


        function CleanSpecialSymbols(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9a-zA-Z\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Decommissioning
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
                <asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="btn btn-primary"
                    OnClick="cmdClose_Click" />
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
                            <i class="icon-reorder"></i>Basic Details</h4>
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
                                                <asp:Label ID="lblIDText" runat="server" Text="Failure Id"></asp:Label>
                                                <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfFailureId" runat="server" />
                                                    <asp:TextBox ID="txtFailureId" runat="server" onkeypress="return onlyNumbers(event)" onchange="return cleanSpecialAndChar(this)"
                                                        MaxLength="10" TabIndex="1"></asp:TextBox>
                                                    <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click"
                                                        TabIndex="2" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                    <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                    <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                    <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox><br />
                                                    <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkDTCDetails_Click">View DTC Details</asp:LinkButton>
                                                    <asp:TextBox ID="txtType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTCName" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtInvoiceId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                <asp:Label ID="lblDateText" runat="server" Text="Failure Date"></asp:Label>
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfOfficeCode" runat="server" />
                                                    <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                    <asp:TextBox ID="txtFailureDate" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtTCCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Make Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfDTCId" runat="server" />
                                                    <asp:HiddenField ID="hdfTCId" runat="server" />
                                                    <asp:TextBox ID="txtMake" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtReplaceId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Capacity(in KVA)</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtCapcity" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space20">
                        </div>
                        <!-- END FORM-->
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Decommissioning</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <asp:Panel ID="pnlApproval" runat="server">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Decommissioning Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDecommDate" runat="server" MaxLength="10" TabIndex="3" onkeypress="return onlyNumbers(event)" onchange="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDecommDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Manual RI Number<span class="Mandotary"> </span>
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtManualRINo" runat="server" MaxLength="25" TabIndex="4" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RI Number<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRINo" runat="server" MaxLength="10" TabIndex="4" onkeypress="javascript:return OnlyNumber(event);" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RI Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRIDate" runat="server" MaxLength="10" TabIndex="5" onkeypress="return onlyNumbers(event)" onchange="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtRIDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTR Reading
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTrReading" runat="server" MaxLength="10" TabIndex="6" onkeypress="javascript:return AllowNumber(this,event);" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTR Commission Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="TxtCommDate" runat="server" MaxLength="10" TabIndex="3" onkeypress="return onlyNumbers(event)" onchange="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="TxtCommDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Store Name <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" Enabled="false" TabIndex="7">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Tank Capacity(in Liter)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTankcapacity" runat="server" MaxLength="7" autocomplete="off"
                                                            onkeypress="javascript:return onlyNumberswithdot(event);" onpaste="return false" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label" style="width:180px!important">Total Oil Quantity(in Litre)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtQuantityOfOil" runat="server" MaxLength="7" autocomplete="off"
                                                            onkeypress="javascript:return onlyNumberswithdot(event);" onpaste="return false" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Oil Returned To The Store(in Liter)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOilQuantity" runat="server" TabIndex="8" MaxLength="7" onkeypress="javascript:return AllowNumber(this,event);" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Remarks<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="500" onkeyup="return ValidateTextlimit(this,500);"
                                                            Style="resize: none" TextMode="MultiLine" TabIndex="9" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span1">
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="space20">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-reorder"></i>Comments for Approve/Reject</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
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
                                                        Comments<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                            <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="12" TextMode="MultiLine" onchange="return CleanSpecialSymbols(this)"
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
            </div>
            <div class="text-center" align="center">


                <asp:Button ID="cmdSave" runat="server" Text="Submit" TabIndex="10" OnClientClick="javascript:return ValidateMyForm()"
                    CssClass="btn btn-success" OnClick="cmdSave_Click" />


                <asp:Button ID="cmdReset" runat="server" Text="Reset" TabIndex="11" CssClass="btn btn-danger"
                    OnClick="cmdReset_Click" /><br />
                <br />
                <asp:Button ID="cmdViewInvoice" runat="server" Text="View Invoice" CssClass="btn btn-primary"
                    OnClick="cmdViewInvoice_Click" TabIndex="13" />


                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                <div class="space20">
                </div>
            </div>
            <!-- END SAMPLE FORM PORTLET-->
        </div>
    </div>
    <!-- END PAGE CONTENT-->
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="RepairerInvoiceCreate.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.RepairerInvoiceCreate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=cmdSave.ClientID %>').prop('disabled', true);
        }
        window.onbeforeunload = preventMultipleSubmissions;

    </script>
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <style type="text/css">
        .form-horizontal .control-label {
            text-align: right;
        }

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
            if (charCode < 46 || charCode > 57) {

                return false;
            }
            return true;
        }


        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 47)
                return false;

            return true;
        }


        function isNumberKey(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8))
                return false;
            else {
                var len = $(element).val().length;
                var index = $(element).val().indexOf('.');
                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    var CharAfterdot = (len + 1) - index;
                    if (CharAfterdot > 2) {
                        return false;
                    }
                }

            }
            return true;
        }


               

        // Dtc allow Chatractes and Numbers to paste
        function cleanSpecialChars(t) {
            debugger;
            t.value = t.value.toString().replace(/[^a-zA-Z 0-9\t\n\r]+/g, '');
            //alert(" Special charactes are not allowed!");
        }

       // DTR allow to enter nums for 6 digits
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function onlynumbers(event) {
            var charcode = (event.which) ? event.which : event.keycode
            if (charcode > 31 && (charcode < 48 || charcode > 57))
                return false;

            return true;
        }

        //function onlyNumberswithdot(event) {
        //    var charCode = (event.which) ? event.which : event.keyCode
        //    if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46))
        //        return false;

        //    return true;
        //}


        //function onlyNumberswithdot(event, t) {
        //    var charCode = (event.which) ? event.which : event.keyCode
        //    if (charCode == 46 && t.value.split('.').length == 2)
        //        return false;
        //    if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46))
        //        return false;
        //    return true;
        //}
        function onlyNumberswithdot(event, t) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode == 46 && (t.value == "" || t.value.indexOf('.') == 0))
                return false;
            if (charCode == 46 && t.value.split('.').length == 2)
                return false;
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46))
                return false;
            return true;
        }
        // DTR allow only Numbers to paste
        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }

        function CleanSpecialSymbols(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9a-zA-Z\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        //allow only characters,space . and , to Reason
        function characterAndSpace(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
                ((evt.which) ? evt.which : 0));
            if (charCode > 31 && (charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 46 && charCode != 44) {

                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Oil Invoice Search                   
                                      
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
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                  <asp:Button ID="cmdclose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="cmdClose_click" />
                </div>
            </div>
            <!-- END PAGE HEADER-->

            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Oil Invoice Search   </h4>
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

                                                <label class="control-label">Purchase Order No</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPONo" runat="server" MaxLength="50" onkeypress="return characterAndnumbersPo(event)" onchange="return cleanSpecialCharsPo(this)"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary" TabIndex="2" />
                                                    </div>
                                                </div>

                                            </div>

                                        </div>

                                        <div class="span5">
                                            <div class="control-group">

                                                <label class="control-label">Repairer</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRepairer" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                    </div>

                                    <div class="space20"></div>

                                    <div class="space20"></div>

                                    <div class="space20"></div>

                                    <div class="text-center" align="center">

                                        <div class="span4"></div>

                                        <div class="span2">
                                            <asp:Button ID="cmdLoad" runat="server" Text="Load" OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" OnClick="cmdLoad_Click" />
                                        </div>

                                        <div style="margin-left: -12px!important" class="span1">
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="cmdReset_Click" />
                                        </div>

                                        <div class="span7"></div>

                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                    </div>
                                    <div id="divgrd" runat="server" visible="false">
                                        <asp:GridView ID="grdOilInvioce" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                            AutoGenerateColumns="false" PageSize="10" DataKeyNames="RSD_ID"
                                            CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                            runat="server" OnPageIndexChanging="grdOilInvioce_PageIndexChanging"
                                            TabIndex="5" OnSorting="grdOilInvioce_Sorting" AllowSorting="true">
                                            <HeaderStyle CssClass="both" />
                                            <Columns>



                                                <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="TC ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRepairDetailsId" runat="server" Text='<%# Bind("RSD_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="Po No" SortExpression="RSM_PO_NO">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPONo" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TC_QUANTITY" HeaderText="Tc Quantity" SortExpression="TC_QUANTITY">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltcquantity" runat="server" Text='<%# Bind("TC_QUANTITY") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="RSM_INV_NO" HeaderText="Invoice No" SortExpression="RSM_INV_NO" Visible="false">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblinvoiceno" runat="server" Text='<%# Bind("RSM_INV_NO") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TOTAL_OIL_AMOUNT" HeaderText="Total Oil(Ltrs)" SortExpression="TOTAL_OIL_AMOUNT">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lbloiltotalqty" runat="server" Text='<%# Bind("TOTAL_OIL_AMOUNT") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="SENT_OIL" HeaderText="Oil Sent By Store Keeper(Ltrs)" SortExpression="SENT_OIL">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsentoil" runat="server" Text='<%# Bind("SENT_OIL") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="ROI_PENDINGQTY" HeaderText="Oil Supplied By Repairer(Ltrs)" SortExpression="ROI_PENDINGQTY">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrepairsentoil" runat="server" Text='<%# Bind("REPAIRER_SENT_OIL") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="ROI_PENDINGQTY" HeaderText="Pending Oil To Send(Ltrs)" SortExpression="ROI_PENDINGQTY">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpendingoil" runat="server" Text='<%# Bind("ROI_PENDINGQTY") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;" Width="150px"></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="RSM_ISSUE_DATE" HeaderText="Issue Date" Visible="false">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssuedate" runat="server" Text='<%# Bind("RSM_ISSUE_DATE") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="RSM_Oil_Amount" HeaderText="Oil Qty" Visible="false">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOilAmount" runat="server" Text='<%# Bind("RSD_OIL_QUANTITY") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier/Repairer" Visible="false">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Edit" Visible="false">
                                                    <ItemTemplate>
                                                        <center>

                                                            <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                                CommandName="Submit" Width="12px" />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>

                                        </asp:GridView>
                                    </div>


                                    <div class="space20"></div>
                                    <div class="text-center" align="center">

                                        <div class="span4"></div>
                                        <div class="span3">
                                            <asp:Button ID="btnloadfilldetails" runat="server" Text="Click To Oil Invoice" CssClass="btn btn-primary" OnClick="cmd_LoadTextFields" Visible="false" />
                                        </div>
                                        <div class="space20"></div>
                                    </div>
                                    <div class="space20"></div>
                                    <div id="divFormDetails" runat="server" visible="false">
                                        <div class="form-horizontal">
                                            <div class="row-fluid">
                                                <div class="span1"></div>
                                                <div class="span5">

                                                    <div class="control-group">

                                                        <label class="control-label">Invoice No</label>

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50"></asp:TextBox>

                                                            </div>
                                                        </div>

                                                    </div>

                                                    <div class="control-group">
                                                        <label class="control-label">Item Code</label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:DropDownList ID="cmbItemCode" runat="server"
                                                                    OnSelectedIndexChanged="cmbItemCode_SelectedIndexChanged" 
                                                                    AutoPostBack="true" TabIndex="2">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <asp:Label ID="lblAvailableQty" runat="server" ForeColor="Green" Font-Size="Medium"></asp:Label>
                                                </div>


                                                <div class="span5">
                                                    <div class="control-group">

                                                        <label class="control-label">Invoice Date <span class="Mandotary">*</span></label>

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtInvoiceDate" runat="server" autocomplete="off" MaxLength="10"></asp:TextBox>
                                                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                    TargetControlID="txtInvoiceDate" Format="dd/MM/yyyy">
                                                                </ajax:CalendarExtender>
                                                            </div>
                                                        </div>

                                                    </div>

                                                    <div class="control-group">

                                                        <label class="control-label">Invoice Oil Qty(in Ltr)<span class="Mandotary"> *</span></label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtEnterInvoiceOilQty" runat="server" MaxLength="10"  autocomplete="off"  onkeypress="javascript:return onlyNumberswithdot(event,this);"
                                                                    AutoPostBack="true" OnTextChanged="txtqty_SelectedIndexChanged"></asp:TextBox>
                                                           
                                                                <asp:HiddenField ID="hdftotalqty" runat="server" />
                                                                <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
                                                                <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                                <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                                <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                                <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                                <asp:HiddenField ID="hdfPono" runat="server" />
                                                                <asp:HiddenField ID="hdfPodate" runat="server" />
                                                            </div>
                                                        </div>

                                                    </div>

                                                    <div class="control-group">
                                                        <label class="control-label">Oil Quantity(in Kltr)<span class="Mandotary"> *</span> </label>
                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <asp:TextBox ID="txtquantity" ReadOnly="true" runat="server" onkeypress="javascript:return onlyNumbers(event)" MaxLength="10" AutoPostBack="true"></asp:TextBox>
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

                                            <div class="space20"></div>

                                            <div class="space20"></div>

                                            <div class="space20"></div>

                                            <div class="text-center" align="center">

                                                <div class="span4"></div>

                                                <div class="span3">
                                                    <%--OnClientClick="javascript:return ValidateMyForm()"--%>
                                                    <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                                                </div>

                                                <div style="margin-left: -12px!important" class="span1">
                                                    <asp:Button ID="cmdresetdetails" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="cmdResetDetails_Click" />
                                                </div>

                                                <div class="space20"></div>

                                                <%--<asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>        --%>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row-fluid">
                                        <div class="span12">
                                            <!-- BEGIN SAMPLE FORMPORTLET-->
                                            <div class="widget blue">
                                                <div class="widget-title">
                                                    <h4><i class="icon-reorder"></i>Invoiced Oil Po Details</h4>
                                                    <span class="tools">
                                                        <a href="javascript:;" class="icon-chevron-down"></a>
                                                    </span>
                                                </div>
                                                <div class="widget-body">

                                                    <div class="widget-body form">
                                                        <!-- BEGIN FORM-->
                                                        <asp:GridView ID="grdInvoicedPo" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                                            AutoGenerateColumns="false" PageSize="10"
                                                            CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                            runat="server"
                                                            TabIndex="5" OnPageIndexChanging="grdInvoicedPo_PageIndexChanging" OnSorting="grdInvoicedPo_Sorting" AllowSorting="true">
                                                            <HeaderStyle CssClass="both" />
                                                            <Columns>


                                                                <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="Po No" SortExpression="RSM_PO_NO">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInvoicedNO" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;" Width="130px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField AccessibleHeaderText="ROI_INVOICE_NO" HeaderText="Invoice No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInvoicedInvoiceNo" runat="server" Text='<%# Bind("ROI_INVOICE_NO") %>' Style="word-break: break-all;" Width="130px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField AccessibleHeaderText="ROI_INVOICE_DATE" HeaderText="Invoice Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInvoicedDate" runat="server" Text='<%# Bind("ROI_INVOICE_DATE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField AccessibleHeaderText="ROI_MMS_INVOICE_NO" HeaderText="MMS Invoice No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblmmsinvoiceno" runat="server" Text='<%# Bind("ROI_MMS_INVOICE_NO") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField AccessibleHeaderText="ROI_TOTAL_OILQTY" HeaderText="Total Oil Qty(Ltrs)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbltotaloilqty" runat="server" Text='<%# Bind("ROI_TOTAL_OILQTY") %>' Style="word-break: break-all;" Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="ROI_INVOICED_OILQTY" HeaderText="Invoiced Qty(Ltrs)">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblinvoicedoilqty" runat="server" Text='<%# Bind("ROI_INVOICED_OILQTY") %>' Style="word-break: break-all;" Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>

                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                            </div>
                            <div class="space20"></div>
                            <!-- END FORM-->
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
                                    <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Save The Repairer Oil Invoice Details.
                                </p>
                                <p style="color: Black">
                                    <i class="fa fa-info-circle"></i>User Need To Select PO No Then Click On Load Button
                                </p>
                                <p style="color: Black">
                                    <i class="fa fa-info-circle"></i>User Need To select the PO check box And Click On Load The Details.
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

            </div>


        </div>
    </div>
</asp:Content>

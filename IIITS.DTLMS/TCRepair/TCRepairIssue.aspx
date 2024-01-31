<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TCRepairIssue.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.TCRepairIssue" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=cmdSave.ClientID %>').prop('disabled', true);
        }

        window.onbeforeunload = preventMultipleSubmissions;

    </script>


    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <%--done by roshan--%>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#<%= txtIssueDate.ClientID %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: 0,
                minDate: -3,

            })

            $("#<%=txtInvoiceDate.ClientID%>").datepicker(
                {
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    maxDate: 0,
                    minDate: -3,

                })
            $("#<%=txtPODate.ClientID%>").datepicker(
                {
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    maxDate: 0,

                })


        });

    </script>
    <%--end--%>

    <script type="text/javascript">

        function ValidateMyForm() {

            if (document.getElementById('<%= cmbGuarantyType.ClientID %>').value == "-Select-") {
                alert('Select Guarantee Type')
                document.getElementById('<%= cmbGuarantyType.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= ddlType.ClientID %>').value == "-Select-") {
                alert('Select Type (Supplier/Repairer)')
                document.getElementById('<%= ddlType.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbRepairer.ClientID %>').value == "--Select--") {
                alert('Select Repairer / Supplier')
                document.getElementById('<%= cmbRepairer.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtIssueDate.ClientID %>').value.trim() == "") {
                alert('Enter Valid Issue Date')
                document.getElementById('<%= txtIssueDate.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtPONo.ClientID %>').value.trim() == "") {
                alert('Enter Valid Purchase Order No.')
                document.getElementById('<%= txtPONo.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtPODate.ClientID %>').value.trim() == "") {
                alert('Enter Valid Purchase Order Date')
                document.getElementById('<%= txtPODate.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtInvoiceNo.ClientID %>').value.trim() == "") {
                alert('Enter Valid Invoice No.')
                document.getElementById('<%= txtInvoiceNo.ClientID %>').focus()
                return false
            }
            debugger;
            if (document.getElementById('<%= txtInvoiceDate.ClientID %>').value.trim() == "") {
                alert('Enter Valid Invoice Date')
                document.getElementById('<%= txtInvoiceDate.ClientID %>').focus()
                return false
            }

            <%--if (document.getElementById('<%= txtManualInvoiceNo.ClientID %>').value.trim() == "") {
                alert('Enter Manual Invoice Number')
                document.getElementById('<%= txtManualInvoiceNo.ClientID %>').focus()
                return false
            }--%>
            <%--if (document.getElementById('<%= txtManualInvoiceNo.ClientID %>').value.length != "11") {
                alert('Manual Invoice number should be equal to 11 digits')
                return false
            }--%>
            if (documen.getElementById('<%= rbdNewPo.ClientID%>').value.length == 0 && documen.getElementById('<%= rbdOldPo.ClientID%>').value.length == 0) {
                alert("Please Select either NewPO or OldPO");
                return false;
            }

        }

        function ValidateManualInvNumber() {
            return true;
                <%--if (document.getElementById('<%= txtManualInvoiceNo.ClientID %>').value.length != "11") {
                 alert('Manual Invoice number should be equal to 11 digits')
                 return false--%>
        }


        //function ConfirmDelete(){

        //}

        //function ValidateManualInvNumber()
        //{

        //}
        //function ValidateMyForm()
        //{

        //}

        function ConfirmDelete() {
            var result = confirm('Are you sure you want to Remove?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }

        function CleanSpecialSymbols(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9a-zA-Z\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }

    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

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
                    <h3 class="page-title">Faulty Transformer Issue                    
                                      
                    </h3>

                    <div class="span10"></div>
                    <img src="../img/animated-hand-image-0010.gif" />
                    <a style="font-size: 26px; margin-left: -472px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 26px"></i></a>

                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close"
                        CssClass="btn btn-primary" OnClick="cmdClose_Click" />
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Supplier / Repairer Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>

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

                                                <label class="control-label">Guarantee Type<span class="Mandotary"> *</span></label>
                                                <div class="span1">
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbGuarantyType" runat="server" OnSelectedIndexChanged="cmbGuaranty_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem>-Select-</asp:ListItem>
                                                            <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                            <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                            <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>

                                            </div>
                                            <div class="control-group">

                                                <label class="control-label">Type<span class="Mandotary"> *</span></label>
                                                <div class="span1">
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" enabled="false">
                                                            <asp:ListItem>-Select-</asp:ListItem>
                                                            <asp:ListItem Value="2">Repairer</asp:ListItem>
                                                            <asp:ListItem Value="1">Supplier</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>

                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">
                                                    <asp:Label ID="lblSuppRep" runat="server" Text="Supplier/Repairer"></asp:Label>
                                                    <span class="Mandotary">*</span>
                                                </label>
                                                <div class="span1">
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtStoreId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfTccode" runat="server" />
                                                        <asp:HiddenField ID="hdfNthTime" runat="server" />
                                                        <asp:DropDownList ID="cmbRepairer" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbRepairer_SelectedIndexChanged" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="space5"></div>
                                                <div class="space5"></div>
                                                <div class="space5"></div>


                                                <div class="control-group">
                                                    <%--<label class="control-label">   --%>                 <%--Inspection Done By<span class="Mandotary"> *</span></label><div class="span1"></div>--%>
                                                    <label class="control-label">
                                                        <asp:Label ID="lblinspect" runat="server" Text="Inspection Done By"> </asp:Label>
                                                        <span id="spStar" runat="server" class="Mandotary">*</span>
                                                    </label>
                                                    <div class="span1">
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmblochtlt" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>



                                                <%--<label class="control-label">Name</label>
                                             <div class="span1"> 
                                            </div>
                                            <div class="controls">
                                                <div class="input-append">                                                       
                                                    <asp:TextBox ID="txtName" runat="server" MaxLength="50" TabIndex="2" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>--%>
                                            </div>
                                        </div>

                                        <div class="span5">
                                            <div class="control-group">

                                                <label class="control-label">Name</label>
                                                <div class="span1">
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtName" runat="server" MaxLength="50" TabIndex="2" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="space5"></div>
                                                <div class="space5"></div>
                                                <div class="space5"></div>

                                                <label class="control-label">Phone</label>
                                                <div class="span1">
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" TabIndex="3" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Address</label>
                                                <div class="span1">
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="50" TabIndex="4" TextMode="MultiLine" Style="resize: none"
                                                            onkeyup="javascript:ValidateTextlimit(this,100)" ReadOnly="true"></asp:TextBox>

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
            </div>
            <!-- END PAGE CONTENT-->

            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Issue Details</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>

                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <asp:Panel ID="pnlApproval" runat="server">
                                        <div class="row-fluid">
                                            <div class="span1"></div>
                                            <div class="span5">

                                                <div class="control-group">

                                                    <div class="span5">
                                                        <asp:RadioButton ID="rbdNewPo" runat="server" Text="New Purchase Order" CssClass="radio"
                                                            GroupName="a" AutoPostBack="true" Checked="true" OnCheckedChanged="rbdNewPo_CheckedChanged" />
                                                    </div>
                                                    <div class="span5">
                                                        <asp:RadioButton ID="rbdOldPo" runat="server" Text="Already Sent Purchase Order" CssClass="radio"
                                                            GroupName="a" AutoPostBack="true" OnCheckedChanged="rbdOldPo_CheckedChanged" />
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Issue Date<span class="Mandotary"> *</span></label>
                                                    <div class="span1">
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtIssueDate" runat="server" MaxLength="10" TabIndex="5"></asp:TextBox>
                                                            <%--    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" TargetControlID="txtIssueDate" Format="dd/MM/yyyy"></ajax:CalendarExtender> --%>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">PO No<span class="Mandotary"> *</span></label>
                                                    <div class="span1">
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtPONo" runat="server" MaxLength="25" autocomplete="off" TabIndex="6" onkeypress="javascript:return RestrictSpace(event)" onpaste="return false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">PO Date<span class="Mandotary"> *</span></label>
                                                    <div class="span1">
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtPODate" runat="server" MaxLength="10" TabIndex="7"></asp:TextBox>
                                                            <%--                                                           <ajax:CalendarExtender ID="CalendarExtender_txtPODate" runat="server" CssClass="cal_Theme1" TargetControlID="txtPODate" Format="dd/MM/yyyy" ></ajax:CalendarExtender>                                             --%>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Invoice Date<span class="Mandotary"> *</span></label>
                                                    <div class="span1">
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtInvoiceDate" runat="server" MaxLength="10" TabIndex="9"></asp:TextBox>
                                                            <%--                                                           <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1" TargetControlID="txtInvoiceDate" Format="dd/MM/yyyy" ></ajax:CalendarExtender>                                             --%>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="span5">

                                                <div class="control-group">

                                                    <div class="span5">
                                                        <asp:RadioButton ID="rbdWithOil" runat="server" Text="With Oil" CssClass="radio"
                                                            GroupName="b" AutoPostBack="true" OnCheckedChanged="rbdWithOil_CheckedChanged" />
                                                    </div>
                                                    <div class="span5">
                                                        <asp:RadioButton ID="rbdWithoutOil" runat="server" Text="WithOut Oil" CssClass="radio"
                                                            GroupName="b" AutoPostBack="true" Checked="true" OnCheckedChanged="rbdWithoutOil_CheckedChanged" />
                                                    </div>
                                                </div>

                                                <div class="control-group" id="divOil" runat="server" visible="false">
                                                    <label class="control-label">Enter Oil Quantity in Liters (Klts)</label>
                                                    <div class="span1"><span class="Mandotary">*</span></div>
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOilQnty" runat="server" MaxLength="8" TabIndex="8"></asp:TextBox>

                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Auto DTLMS Invoice No<span class="Mandotary"> *</span></label>
                                                    <div class="span1">
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="20" TabIndex="8"></asp:TextBox>
                                                            <asp:TextBox ID="txtRepairMasterId" runat="server" MaxLength="20" TabIndex="8" Visible="false"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Select Old PO NO</label>
                                                    <div class="span1"></div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtPonum" runat="server" MaxLength="10" TabIndex="12"></asp:TextBox>
                                                            <asp:Button ID="cmdSearchPO" Text="S" class="btn btn-primary" runat="server"
                                                                CausesValidation="false" TabIndex="13" OnClick="cmdSearchPO_Click" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Remarks</label>
                                                    <div class="span1">
                                                        <span class="Mandotary"></span>
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtRemarks" AutoPostBack="false" runat="server" Style="resize: none"  
                                                                onkeyup="javascript:ValidateTextlimit(this,500)" onclick="cmdSearchPO_Click" 
                                                                TextMode="MultiLine" TabIndex="9"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </asp:Panel>
                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span1">
                                            <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return ValidateMyForm()"
                                                CssClass="btn btn-primary" OnClick="cmdSave_Click" TabIndex="10" CausesValidation="false" />
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                                                CssClass="btn btn-primary" OnClick="cmdReset_Click" TabIndex="11" /><br />
                                        </div>
                                        <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>

                            </div>

                            <div class="space20"></div>
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
                            <h4><i class="icon-reorder"></i>Selected Transformer</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>

                            </span>
                        </div>
                        <div class="widget-body">
                            <div id="divDTrDetails" runat="server">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <div class="form-horizontal">
                                        <div class="row-fluid">
                                            <div class="span1"></div>
                                            <div class="span5">
                                                <div id="DTrCODE" runat="server" class="control-group" visible="false">
                                                    <%-- <label class="control-label">DTr Code</label>--%>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTcCode" runat="server" MaxLength="10" TabIndex="12" Visible="false"></asp:TextBox>
                                                            <asp:Button ID="cmdSearchTC" Text="S" class="btn btn-primary" runat="server"
                                                                CausesValidation="false" OnClick="cmdSearchTC_Click" TabIndex="13" Visible="false" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="span4">
                                                <div id="MAKE" runat="server" class="control-group" visible="false">
                                                    <%--<label class="control-label">Make</label>           --%>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtMake" runat="server" MaxLength="50" TabIndex="14" Visible="false"></asp:TextBox>
                                                            <asp:TextBox ID="txtSelectedTcId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                            <asp:Button ID="cmdLoad" Text="Load" class="btn btn-primary" runat="server"
                                                                OnClick="cmdLoad_Click" TabIndex="15" CausesValidation="false" Visible="false" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->

                            <asp:GridView ID="grdFaultTC" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false" PageSize="10" DataKeyNames="TC_ID"
                                CssClass="table table-striped table-bordered table-advance table-hover"
                                runat="server" TabIndex="16" OnRowCommand="grdFaultTC_RowCommand">
                                <Columns>

                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" SortExpression="TC_SLNO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="IT_ID" HeaderText="ItemID" SortExpression="IT_ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("IT_ID") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="IT_CODE" HeaderText="Item Code" SortExpression="code">

                                        <ItemTemplate>
                                            <asp:Label ID="lblItemcode" runat="server" Text='<%# Bind("IT_CODE") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="IT_NAME" HeaderText="Item Name" SortExpression="IT_NAME">

                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("IT_NAME") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all" Width="150px"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">

                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TC_STAR_RATE" HeaderText="Star Rating">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstarrating" runat="server" Text='<%# Bind("TC_STAR_RATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Amount" HeaderText="Oil Qty" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TC_GUARANTY_TYPE" HeaderText="Guarantee Type" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("TC_GUARANTY_TYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remove" Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="imgBtnDelete" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();"></asp:ImageButton>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>


                        </div>
                    </div>
                </div>

                <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4><i class="icon-reorder"></i>Comments for Approve/Reject</h4>
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

                                                            <asp:HiddenField ID="hdfRepairId" runat="server" />
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

                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4><i class="icon-reorder"></i>GatePass</h4>
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
                                                    <label class="control-label">Vehicle No<span class="Mandotary"> *</span></label>
                                                    <div class="span1">
                                                    </div>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtGpId" runat="server" Visible="false"></asp:TextBox>
                                                            <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfInvoiceNo" runat="server" />
                                                            <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <label class="control-label">Receipient Name<span class="Mandotary"> *</span></label>
                                                <div class="span1">
                                                </div>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:TextBox ID="txtReciepient" runat="server" MaxLength="50"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <label class="control-label">Challen Number<span class="Mandotary"> *</span></label>
                                            <div class="span1">
                                            </div>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:TextBox ID="txtChallen" runat="server" MaxLength="50"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="space20"></div>

                                        <div class="form-horizontal" align="center">

                                            <div class="span3"></div>
                                            <div class="span1">
                                            </div>
                                            <div class="span1">
                                                <asp:Button ID="cmdGatePass" runat="server" Text="Print GatePass"
                                                    CssClass="btn btn-primary" OnClick="cmdGatePass_Click" Enabled="false" />
                                            </div>

                                            <div class="space20"></div>

                                        </div>





                                    </div>



                                </div>


                            </div>
                        </div>
                    </div>



                </div>
                <!-- END SAMPLE FORM PORTLET-->
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
                        <li style="font-size: 18px">ಹೊಸ PO : New Purchase Order  ಕ್ಲಿಕ್ ಮಾಡಿ ನಂತರ ಹಿಂದಿನ ವಿಧಾನವನ್ನು ಅನುಸರಿಸಿ</li>
                        <li style="font-size: 18px">ಹಳೆಯ PO : ಹುಡುಕಾಟ ಬಟನ್ನಿಂದ (S) ಹಳೆಯ ಖರೀದಿ ಆರ್ಡರ್ ಸಂಖ್ಯೆ ಆಯ್ಕೆಮಾಡಿ ನಂತರ ಮುಂದುವರೆಯಿರಿ</li>
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

<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TcMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateMyForm() {
            debugger;
            if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
                alert('Enter DTr Code')
                document.getElementById('<%= txtTcCode.ClientID %>').focus()
                return false
            }


            if (document.getElementById('<%= cmbMake.ClientID %>').value.trim() == "-Select-") {
                alert('Select DTr Make Name')
                document.getElementById('<%= cmbMake.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbCapacity.ClientID %>').value == "-Select-") {
                alert('Select valid Capacity')
                document.getElementById('<%= cmbCapacity.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbStarRated.ClientID %>').value == "--Select--") {
                alert(' Please Select Star Rating ')
                document.getElementById('<%= cmbStarRated.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbTcLocation.ClientID %>').value == "-Select-") {
                alert('Select DTr Current Location')
                document.getElementById('<%= cmbTcLocation.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtOilCapacity.ClientID %>').value.trim() == "") {
                alert('Enter Valid Total Oil Quantity(in Liter)')
                document.getElementById('<%= txtOilCapacity.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtWeight.ClientID %>').value.trim() == "") {
                alert('Enter Valid Weight of DTr')
                document.getElementById('<%= txtWeight.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtTankcapacity.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtTankcapacity.ClientID %>').value

                if (pass.match(/^(?=.*?[A-Z])/)) {
                    alert("Please enter valid Tank Capacity(in Liter)")
                    document.getElementById('<%=txtTankcapacity.ClientID %>').focus()
                    document.getElementById('<%= txtTankcapacity.ClientID %>').value = "";
                    return false;
                }
                if (pass.match(/^(?=.*?[a-z])/)) {
                    alert("Please enter valid Tank Capacity(in Liter)")
                    document.getElementById('<%=txtTankcapacity.ClientID %>').focus()
                    document.getElementById('<%= txtTankcapacity.ClientID %>').value = "";
                    return false;
                }

            }

            if (document.getElementById('<%= txtOilCapacity.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtOilCapacity.ClientID %>').value

                if (pass.match(/^(?=.*?[A-Z])/)) {
                    alert("Please enter valid Total Oil Quantity(in Liter)")
                    document.getElementById('<%=txtOilCapacity.ClientID %>').focus()
                    document.getElementById('<%= txtOilCapacity.ClientID %>').value = "";
                    return false;
                }
                if (pass.match(/^(?=.*?[a-z])/)) {
                    alert("Please enter valid Total Oil Quantity(in Liter)")
                    document.getElementById('<%=txtOilCapacity.ClientID %>').focus()
                    document.getElementById('<%= txtOilCapacity.ClientID %>').value = "";
                    return false;
                }

            }

        }
        // function to block  some of the special characters . 
        function DontAllowPlusAndSingleQuotes(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode >= 33 && charCode <= 44 || (charCode >= 58 && charCode <= 64) || (charCode >= 91 && charCode <= 94) || (charCode == 96))
                return false;
            else
                return true;
        }

        function onlyNumbers(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if ((charCode >= 48 && charCode <= 57) || (charCode == 46))
                return true;
            else
                return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div style="margin-top:20px !important;padding:0px;z-index:-9999;">
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <h3 class="page-title">DTR Master
                    </h3>
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
                <div style="float: right;  margin-right: 12px">
                    <asp:Button ID="btnDtrView" runat="server" Text="DTR View"
                        OnClientClick="javascript:window.location.href='TCMasterView.aspx'; return false;"
                        CssClass="btn btn-primary" />
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>DTR Master</h4>
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
                                                <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtTcCode" runat="server" AutoPostBack="true"
                                                            onkeypress="javascript:return OnlyNumber(event);" MaxLength="10"
                                                            OnTextChanged="txtTcCode_TextChanged"></asp:TextBox>
                                                        <asp:TextBox ID="txtTcID" runat="server" Visible="false" Width="20px"></asp:TextBox><br />
                                                        <asp:LinkButton ID="lnkDTRHistory" runat="server"
                                                            Style="font-size: 12px; color: Blue" OnClick="lnkDTRHistory_Click">View DTR History</asp:LinkButton>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">DTr Serial No </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%-- onkeypress="javascript:return OnlyNumber(event);"  --%>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" onkeypress="return DontAllowPlusAndSingleQuotes(event)" onpaste="return false" MaxLength="20"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Make<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake" runat="server">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Capacity(in KVA)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Manufacturing Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtManufactureDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="ManufactureCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtManufactureDate">
                                                        </ajax:CalendarExtender>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Purchasing Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPurchaseDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="PurchaseCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtPurchaseDate">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Supplier Name</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSupplier" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Purchase Order No</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPoNo" onkeypress="return DontAllowPlusAndSingleQuotes(event)" onpaste="return false" runat="server" MaxLength="50"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Price</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPrice" runat="server" MaxLength="9" autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfStarRate" runat="server" />
                                                        <asp:HiddenField ID="hdfTcCode" runat="server" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Life Span</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcLifeSpan" runat="server" autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"
                                                            MaxLength="5"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Tank Capacity(in Liter)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTankcapacity" runat="server" autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"
                                                            MaxLength="7"></asp:TextBox>

                                                        <%--<asp:RegularExpressionValidator ID="regexValidator" runat="server"
                                                            ControlToValidate="txtTankcapacity"
                                                            ValidationExpression="\.\d+$"
                                                            ErrorMessage="Please enter a valid decimal number with mandatory digits after the decimal point.">
                                                        </asp:RegularExpressionValidator>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" style="width: 180px!important">Total Oil Quantity(in Liter)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOilCapacity" runat="server" autocomplete="off" onkeypress="javascript:return AllowNumber(this,event);"
                                                            MaxLength="7"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Weight of DTr(in KG)<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWeight" runat="server" autocomplete="off" onkeypress="javascript:return onlyNumbers(event)"
                                                            MaxLength="5"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Warrenty Period(in year)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWarrentyPeriod" runat="server" MaxLength="2" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Last Service Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLastServiceDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                            TargetControlID="txtLastServiceDate">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" id="dvStar">
                                                <label class="control-label">Star Rated <span class="Mandotary">* </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStarRated" runat="server" TabIndex="16">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Current Location</label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbTcLocation" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbTcLocation_SelectedIndexChanged" Enabled="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">TC Condition</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTCstatus" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>



                                        </div>
                                    </div>
                                    <div class="span1"></div>
                                </div>
                                <div class="space20"></div>

                                <div class="text-center" align="center">



                                    <asp:Button ID="cmdSave" runat="server" Text="Save"
                                        OnClientClick="javascript:return ValidateMyForm();" CssClass="btn btn-success"
                                        OnClick="cmdSave_Click" />

                                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                        CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />


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
        <div class="row-fluid" runat="server" id="divField" style="display: none">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Basic Details of DTC</h4>
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
                                            <label class="control-label">DTC Code<span class="Mandotary"> *</span> </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtcCode" runat="server"></asp:TextBox>
                                                    <asp:Button ID="btnDtcSearch" runat="server" Text="S" Visible="false"
                                                        CssClass="btn btn-primary" OnClick="btnDtcSearch_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">DTC Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtcName" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span1"></div>
                            </div>
                            <div class="space20"></div>

                            <div class="form-horizontal" align="center">

                                <div class="span3"></div>
                                <div class="span1">
                                    <asp:Button ID="btnReset" runat="server" Text="Reset" Visible="false"
                                        CssClass="btn btn-primary" OnClick="btnReset_Click" /><br />
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


        <div class="row-fluid" runat="server" id="divRepairer" style="display: none">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Basic Details of Repairer</h4>
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
                                            <label class="control-label">Repairer Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtRepairerName" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Repairer Address </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtReAddress" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">Reapirer Mobile No.</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtReMobileNo" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Repairer EmailId</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtReEmailId" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <div class="space20"></div>


                        </div>
                    </div>

                    <div class="space20"></div>
                    <!-- END FORM-->

                </div>
            </div>
            <!-- END SAMPLE FORM PORTLET-->
        </div>



        <div class="row-fluid" runat="server" id="divStore" style="display: none">

            <div class="span12">

                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Basic Details of Store/Transformer Bank</h4>
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
                                            <label class="control-label">Store Name</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtStoreName" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">Store Address</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtStoreAddress" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">

                                        <div class="control-group">
                                            <label class="control-label">Store Incharge</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtStoreincharge" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Store Incharge MobileNo.</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtStoreMobile" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="space20"></div>


                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->

                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>

        </div>
    </div>
</asp:Content>

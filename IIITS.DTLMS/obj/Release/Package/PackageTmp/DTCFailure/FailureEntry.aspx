<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FailureEntry.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.FailureEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script>
        $(function () {
            $("#txtFailedDate").datepicker();
        });
    </script>
    <script type="text/javascript">

        function ValidateMyForm() {

            if (document.getElementById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
                alert('Select Valid DTC Code')
                document.getElementById('<%= txtDTCCode.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbFailureType.ClientID %>').value.trim() == "--Select--") {
                alert('Select Failure Type')
                document.getElementById('<%= cmbFailureType.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbStarRated.ClientID %>').value == "--Select--") {
                alert(' Please Click on  View DTr Details and Update Star Rate.')
                document.getElementById('<%= cmbStarRated.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtReason.ClientID %>').value.trim() == "") {
                alert('Enter The Failure Reason')
                document.getElementById('<%= txtReason.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtFailedDate.ClientID %>').value.trim() == "") {
                alert('Select The Failed Date')
                document.getElementById('<%= txtFailedDate.ClientID %>').focus()
                return false
            }


            if (document.getElementById('<%= cmbHtBusing.ClientID %>').value.trim() == "--Select--") {
                alert('Select H.T.Bushing')
                document.getElementById('<%= cmbHtBusing.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbLtBusing.ClientID %>').value.trim() == "--Select--") {
                alert('Select L.T.Bushing')
                document.getElementById('<%= cmbLtBusing.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbHtBusingRod.ClientID %>').value.trim() == "--Select--") {
                alert('Select H.T.Bushing Rod & Nut')
                document.getElementById('<%= cmbHtBusingRod.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbLtBusingRod.ClientID %>').value.trim() == "--Select--") {
                alert('Select L.T.Bushing Rod & Nut')
                document.getElementById('<%= cmbLtBusingRod.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbDrainValve.ClientID %>').value.trim() == "--Select--") {
                alert('Select Drain Valve')
                document.getElementById('<%= cmbDrainValve.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbOilLevel.ClientID %>').value.trim() == "--Select--") {
                alert('Select Oil Level Gauge')
                document.getElementById('<%= cmbOilLevel.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbTankCondition.ClientID %>').value.trim() == "--Select--") {
                alert('Select Condition of Tank')
                document.getElementById('<%= cmbTankCondition.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbExplosion.ClientID %>').value.trim() == "--Select--") {
                alert('Select Explosion Vent Valve')
                document.getElementById('<%= cmbExplosion.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbBreather.ClientID %>').value.trim() == "--Select--") {
                alert('Select Breather')
                document.getElementById('<%= cmbBreather.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtDTCCommissionDate.ClientID %>').value.trim() == "") {
                alert('Enter Commission Date .NOTE : 1) Goto Masters -> DTC Master 2) Search DTC Code 3) Click on EDIT button 4) Enter DTC Commission Date')
                document.getElementById('<%= txtDTCCommissionDate.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "" || document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "0") {
                alert('This DTC is Currently having No TC, please contact the DTLMS Team')
                document.getElementById('<%= txtTcCode.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtLatitude.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtLatitude.ClientID %>').value
                if (pass.match(/^(?=.*?[a-z])/)) {
                    alert("Please enter valid Latitude")
                    document.getElementById('<%=txtLatitude.ClientID %>').focus()
                    return false;
                }
                if (pass.match(/^(?=.*?[A-Z])/)) {
                    alert("Please enter valid Latitude")
                    document.getElementById('<%=txtLatitude.ClientID %>').focus()
                    return false;
                }
            }

            if (document.getElementById('<%= txtLongitude.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtLongitude.ClientID %>').value

                if (pass.match(/^(?=.*?[A-Z])/)) {
                    alert("Please enter valid Longitude")
                    document.getElementById('<%=txtLongitude.ClientID %>').focus()
                    return false;
                }
                if (pass.match(/^(?=.*?[a-z])/)) {
                    alert("Please enter valid Longitude")
                    document.getElementById('<%=txtLongitude.ClientID %>').focus()
                    return false;
                }

            }

            if (document.getElementById('<%= txtTankcapacity.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtTankcapacity.ClientID %>').value
                if (pass.match(/^(?=.*?[a-z])/)) {
                    alert("Enter Valid Tank Capacity(in Liter)")
                    document.getElementById('<%=txtTankcapacity.ClientID %>').focus()
                    return false;
                }
                if (pass.match(/^(?=.*?[A-Z])/)) {
                    alert("Enter Valid Tank Capacity(in Liter)")
                    document.getElementById('<%=txtTankcapacity.ClientID %>').focus()
                    return false;
                }
            }

            if (document.getElementById('<%= txtQuantityOfOil.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtQuantityOfOil.ClientID %>').value
                if (pass.match(/^(?=.*?[a-z])/)) {
                    alert("Enter Valid Total Oil Quantity(in Liter)")
                    document.getElementById('<%=txtQuantityOfOil.ClientID %>').focus()
                    return false;
                }
                if (pass.match(/^(?=.*?[A-Z])/)) {
                    alert("Enter Valid Total Oil Quantity(in Liter)")
                    document.getElementById('<%=txtQuantityOfOil.ClientID %>').focus()
                    return false;
                }
            }

        }

        //DTC allow to search chars and nums for 6
        function characterAndnumbers(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode > 31 && (charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }

        // Dtc allow Chatractes and Numbers to paste
        function cleanSpecialChars(t) {
            debugger;
            t.value = t.value.toString().replace(/[^a-zA-Z 0-9\t\n\r]+/g, '');
            //alert(" Special charactes are not allowed!");
        }

        //DTR allow to enter nums for 6 digits
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function onlyNumberswithdot(event, t) {
            var charCode = (event.which) ? event.which : event.keyCode
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
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>


    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Declare Failure
                    </h3>
                    <div class="span1">
                        <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
                    </div>
                    <div class="span2">
                        <asp:RadioButton ID="rdbFail" runat="server" Text="Failure"
                            CssClass="radio" GroupName="a" AutoPostBack="true" Checked="true"
                            OnCheckedChanged="rdbFailEnhance_CheckedChanged" />
                    </div>
                    <div>
                        <asp:RadioButton ID="rdbFailEnhance" runat="server" Text="Failure And Enhancement"
                            CssClass="radio" GroupName="a" AutoPostBack="true"
                            OnCheckedChanged="rdbFailEnhance_CheckedChanged" Width="200px" />
                    </div>
                    <div class="span7">
                    </div>


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
                            <h4><i class="icon-reorder"></i>Declare Failure</h4>
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
                                                <label class="control-label">DTC Code <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfDTCcode" runat="server" />

                                                        <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="6" onkeypress="return characterAndnumbers(event)" onchange="return cleanSpecialChars(this)"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server"
                                                            OnClick="cmdSearch_Click" /><br />
                                                        <asp:LinkButton ID="lnkDTCDetails" runat="server"
                                                            Style="font-size: 12px; color: Blue" OnClick="lnkDTCDetails_Click">View DTC Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">DTC Name </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                        <asp:TextBox ID="txtFailureOfficCode" runat="server" MaxLength="100" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtTCId" runat="server" MaxLength="100" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="txtDTCName" runat="server" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Load KW  </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLoadKW" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Width="20px" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Load Hp </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                        <asp:TextBox ID="txtLoadHP" runat="server" ReadOnly="true" onkeypress="return OnlyNumber(event)" MaxLength="10"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTC Commission Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTCCommissionDate" runat="server" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">DTr Commission Date <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTrCommDate" runat="server" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDTrCommDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Capacity(in KVA) </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCapacity" runat="server" MaxLength="15" ReadOnly="true"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Section</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLocation" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Latitude<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLatitude" runat="server" TabIndex="16" MaxLength="20" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">DTr Code </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTcCode" runat="server" ReadOnly="true" MaxLength="6" onkeypress="return onlyNumbers(event)" onchange="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkDTrDetails" runat="server"
                                                            Style="font-size: 12px; color: Blue" OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">DTr Make </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDtcId" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="txtFailurId" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="txtTCMake" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Serial Number </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTCSlno" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server">
                                                <label class="control-label">Star Rated <span class="Mandotary">* </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStarRated" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Manf. Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtManfDate" runat="server" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Guarantee Type <span class="Mandotary">*</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbGuarenteeType" runat="server" ReadOnly="true" Enabled="false">
                                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Value="AGP" Text="After Guaranty Period"></asp:ListItem>
                                                            <asp:ListItem Value="WRGP" Text="Within Repair Guaranty Period -- Repairer"></asp:ListItem>
                                                            <asp:ListItem Value="WGP" Text="Within Guaranty Period -- Supplier"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <asp:HiddenField ID="hdfGuarenteeSource" runat="server" />
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Last Repaired By </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLastRepairer" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Last Repaired Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLastRepairDate" runat="server" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Longitude<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtLongitude" runat="server" TabIndex="16" MaxLength="20" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <asp:Label ID="lblEnCap" CssClass="control-label" runat="server" Text="Enhancment Capacity <span class='Mandotary'> *</span>" Visible="false"></asp:Label>
                                                <%--<label class="control-label" style="visibility:hidden">Enhancment Capacity</label>--%>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbEnhanceCapacity" runat="server" Enabled="false" Visible="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="span1"></div>
                                </div>

                            </div>
                        </div>

                        <!-- END FORM-->

                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>Failure Entry Details</h4>
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

                                        <div class="row-fluid">
                                            <div class="span1"></div>

                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">Failure Type<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbFailureType" runat="server">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Failure Reason<span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtReason" runat="server" MaxLength="10" TextMode="MultiLine"
                                                                Style="resize: none" onkeyup="return ValidateTextlimit(this,100);" onkeypress="return characterAndSpace(event);" onpaste="return false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Failed Date <span class="Mandotary">*</span> </label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtFailedDate" runat="server" MaxLength="10" Text='<%# System.DateTime.Now%>'></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                                TargetControlID="txtFailedDate" Format="dd/MM/yyyy">
                                                            </ajax:CalendarExtender>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">DTC Reading </label>

                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:TextBox ID="txtDTCRead" runat="server" MaxLength="10" autocomplete="off"
                                                                onkeypress="javascript:return AllowNumber(this,event);" onpaste="return false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">H.T.Bushing<span class="Mandotary"> *</span> </label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbHtBusing" runat="server">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Good"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Bad"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">L.T.Bushing <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbLtBusing" runat="server">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Good"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Bad"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Drain Valve<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbDrainValve" runat="server">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">H.T.Bushing Rod & Nut <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbHtBusingRod" runat="server">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Good"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Bad"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">L.T.Bushing Rod & Nut <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbLtBusingRod" runat="server">

                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Good"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Bad"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="span5">


                                                <div class="control-group">
                                                    <label class="control-label">Oil Level Gauge<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbOilLevel" runat="server">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Tank Capacity(in Liter)<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtTankcapacity" runat="server" MaxLength="7" autocomplete="off"
                                                                onkeypress="javascript:return onlyNumberswithdot(event,this);" onpaste="return false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label" style="width: 180px!important">Total Oil Quantity(in Liter)<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtQuantityOfOil" runat="server" MaxLength="7" autocomplete="off"
                                                                onkeypress="javascript:return onlyNumberswithdot(event,this);" onpaste="return false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Condition Of Tank<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbTankCondition" runat="server">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Good"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Bad"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Explosion Vent Valve<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbExplosion" runat="server">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Breather<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbBreather" runat="server">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Customer Name<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtCustomerName" runat="server" MaxLength="35" onpaste="return false"
                                                                autocomplete="off" onkeypress="return characterAndSpace(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Customer Mobile No<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtCustMobileNo" runat="server" MaxLength="10" onpaste="return false"
                                                                autocomplete="off" onkeypress="javascript:return onlyNumbers(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Number Of Installation<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtNumberofInstalmets" runat="server" MaxLength="4" onpaste="return false"
                                                                autocomplete="off" onkeypress="javascript:return onlyNumbers(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>



                                        </div>

                                    </asp:Panel>
                                    <div class="space20"></div>


                                </div>

                            </div>

                            <!-- END FORM-->

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
                    <div class="form-horizontal" align="center">

                        <div class="span3"></div>
                        <div class="span2">
                            <asp:Button ID="cmdSave" runat="server" Text="Save"
                                OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary"
                                OnClick="cmdSave_Click" />
                        </div>
                        <div class="span1">
                            <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                CssClass="btn btn-primary" OnClick="cmdReset_Click" />

                        </div>
                        <div class="span7"></div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                    </div>

                </div>
            </div>

        </div>

    </div>

    <!-- MODAL-->
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
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Declare TC as Failure and Failure with Enhancement
                        .
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>There Are Two Radio Button 1.Failure 2.Failure
                        And Enhancement Available
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Radio Button 1.Failure (Which will be selected
                        By Default) is to Declare TC as Failure
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Radio Button 2.Failure And Enhancement is to Declare
                        TC as Failure With Enhancement
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>There are Two Links Available <u>View DTC Details</u> & <u>View DTr Details To Get More Details about DTC & DTR</u>
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->
    <style>
        select#ContentPlaceHolder1_cmbGuarenteeType {
            font-size: 11px;
            font-weight: bold;
            /*width:auto!important*/
        }
    </style>
</asp:Content>

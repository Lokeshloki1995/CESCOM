<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="WorkOrder.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.WorkOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">

        function ValidateMyForm() {

            //         if (document.getElementById('<%= txtType.ClientID %>').value.trim() != "3") {
            //             if (document.getElementById('<%= txtFailureId.ClientID %>').value.trim() == "") {
            //                 alert('Enter  Failure Id')
            //                 document.getElementById('<%= txtFailureId.ClientID %>').focus()
            //                 return false
            //             }
            //         }
            //         if (document.getElementById('<%= txtFailureDate.ClientID %>').value == "") {
            //             alert('Enter Failure Date')
            //             document.getElementById('<%= txtFailureDate.ClientID %>').focus()
            //             return false
            //         }
            if (document.getElementById('<%= cmbIssuedBy.ClientID %>').value == "--Select--") {
                alert('Select Issued By')
                document.getElementById('<%= cmbIssuedBy.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= cmbCapacity.ClientID %>').value == "--Select--") {
                alert('Select Capacity')
                document.getElementById('<%= cmbCapacity.ClientID %>').focus()
             return false
         }

            //         if (document.getElementById('<%= txtComWoNo1.ClientID %>').value.trim() == "") {
            //             alert('Enter Commissioning WO Number')
            //             document.getElementById('<%= txtComWoNo1.ClientID %>').focus()
            //             return false
            //         }


            if (document.getElementById('<%= txtCommdate.ClientID %>').value.trim() == "") {
                alert('Enter Commissioning Date')
                document.getElementById('<%= txtCommdate.ClientID %>').focus()
             return false
         }


         if (document.getElementById('<%= txtCommAmount.ClientID %>').value.trim() == "") {
                alert('Enter Commissioning Amount')
                document.getElementById('<%= txtCommAmount.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtAcCode.ClientID %>').value.trim() == "") {
                alert('Select Commissioning Account Code')
                document.getElementById('<%= txtAcCode.ClientID %>').focus()
             return false
         }

            //         if (document.getElementById('<%= txtDeWoNo1.ClientID %>').value.trim() == "") {
            //             alert('Enter DeCommissioning Wo No')
            //             document.getElementById('<%= txtDeWoNo1.ClientID %>').focus()
            //             return false
            //         }

            if (document.getElementById('<%= txtDeDate.ClientID %>').value.trim() == "") {
                alert('Enter DeCommissioning Date')
                document.getElementById('<%= txtDeDate.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtDeAmount.ClientID %>').value.trim() == "") {
                alert('Enter DeCommissioning amount')
                document.getElementById('<%= txtDeAmount.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtDecAccCode.ClientID %>').value.trim() == "") {
                alert('Select DeCommissioning Account Code')
                document.getElementById('<%= txtDecAccCode.ClientID %>').focus()
             return false
         }


     }

     function isHypen(evt) {

         var charCode = (evt.which) ? evt.which : event.keyCode;
         if (charCode != 45) {
             return false;
         }


     }

     function allLetter(evt) {
         debugger;
         var charCode = (evt.which) ? evt.which : event.keyCode;
         if (charCode >= 65 && charCode <= 90) {
             return true;
         }
         else {
             return false;
         }
     }

     function nopaste() {

         return false;
     }

     function CleanSpecialSymbols(t) {
         debugger;
         t.value = t.value.toString().replace(/[^0-9a-zA-Z\n\r]+/g, '');
         //alert(" Special charactes and characters are not allowed!");
     }



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
                    <h3 class="page-title">Work Order</h3>
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
            <div class="row-fluid" runat="server" id="dvBasic">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Basic Details</h4>
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
                                                <label class="control-label">
                                                    <asp:Label ID="lblIDText" runat="server" Text="Failure Id"></asp:Label>
                                                    <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        sWFOAutId
                                                        <asp:HiddenField ID="hdfFailureId" runat="server" />
                                                        <asp:TextBox ID="txtFailureId" runat="server"
                                                            onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" TabIndex="1"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server"
                                                            OnClick="cmdSearch_Click" TabIndex="2" />

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTC Code</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfAppDesc" runat="server" />
                                                        <asp:HiddenField ID="hdfBudget" runat="server" />
                                                        <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox></br/>
                                                       <asp:TextBox ID="txtType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:LinkButton ID="lnkDTCDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkDTCDetails_Click">View DTC Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTC Name</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                        <asp:TextBox ID="txtDTCName" runat="server" ReadOnly="true"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Failure Type </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFailureType" runat="server" ReadOnly="true"> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">SubDivision </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtSubdivisionName" runat="server" ReadOnly="true"> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Customer Name</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCustomerName" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Customer Number</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCustomerNumber" runat="server" ReadOnly="true"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtFailureDate" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">DTr Code</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTCCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue" OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTR Capacity</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDtrCapacity" runat="server" ReadOnly="true"> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Declared By</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDeclaredBy" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Section </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtSectionName" runat="server" ReadOnly="true"> </asp:TextBox>
                                                        <br />
                                                        <asp:LinkButton ID="lnkBudgetstat" runat="server" Visible="false"
                                                            Style="font-size: 12px; color: Blue" OnClick="lnkBudgetstat_Click">View Budget Status</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Number Of Installation</label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtNumberofInstalmets" runat="server" ReadOnly="true"></asp:TextBox>
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

                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->




            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid" runat="server" id="dvWorkOrder">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Work Order</h4>
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
                                                    <label class="control-label">Issued By<span class="Mandotary"> *</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbIssuedBy" runat="server" TabIndex="3" Enabled="false">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtWOId" runat="server" Visible="false" Width="20px"
                                                                MaxLength="100"></asp:TextBox>
                                                            <asp:TextBox ID="txtDTCId" runat="server" Visible="false" Width="20px"
                                                                MaxLength="100"></asp:TextBox>
                                                            <asp:TextBox ID="txtTCId" runat="server" Visible="false" Width="20px"
                                                                MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group" runat="server" id="dvSection" style="display: none">
                                                    <label class="control-label">Section<span class="Mandotary"> *</span></label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbSection" runat="server" TabIndex="3">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">Capacity(in KVA) <span class="Mandotary">*</span> </label>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtCapacity" runat="server" Visible="false" Width="20px"
                                                                onkeypress="javascript:return OnlyNumber(event);" MaxLength="10" TabIndex="4"></asp:TextBox>
                                                            <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="4">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <%--<label class="control-label" id ="lblDtcScheme" title="DTC SCHEME"> <span class="Mandotary"> *</span> </label>--%>
                                                    <asp:Label ID="lblDtcScheme" CssClass="control-label" runat="server"> DTC Scheme <span class="Mandotary"> *</span></asp:Label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbDtc_Scheme_Type" runat="server" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="cmbDtc_Scheme_Type_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group" style="display: none">
                                                    <label class="control-label">Upload Document</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:FileUpload ID="fupWODocument" runat="server" AllowMultiple="False" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="space20"></div>
                                            <div class="space20"></div>
                                            <div class="row-fluid">

                                                <div class="span6" runat="server" id="dvComm">
                                                    <!-- BEGIN BASIC PORTLET-->
                                                    <div class="widget blue">
                                                        <div class="widget-title">
                                                            <h4><i class="icon-reorder"></i>Commissioning</h4>
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
                                                                        <div class="span5">
                                                                            <div class="control-group">
                                                                                <label class="control-label">Work Order No  <span class="Mandotary">*</span> <span class="Mandotary">Upper Case Only</span> </label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtComWoNo1" runat="server" MaxLength="5" TabIndex="5" Width="45px" autocomplete="off" onkeypress="javascript:return allLetter(event)" onpaste="return false"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtComWoNo2" runat="server" MaxLength="1" TabIndex="5" Width="45px" autocomplete="off" onkeypress="javascript:return isHypen(event)" onpaste="return false"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtComWoNo3" runat="server" MaxLength="6" TabIndex="5" Width="45px" autocomplete="off" onkeypress="javascript:return OnlyNumber(event)" onpaste="return false"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Date <span class="Mandotary">*</span> </label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtCommdate" runat="server" MaxLength="10" TabIndex="6"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender_txtCommdate" runat="server" CssClass="cal_Theme1"
                                                                                            Format="dd/MM/yyyy" TargetControlID="txtCommdate">
                                                                                        </ajax:CalendarExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Amount  <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtCommAmount" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                                                                            MaxLength="10" TabIndex="7"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">A/C Code <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:DropDownList ID="txtAcCode" runat="server">
                                                                                        </asp:DropDownList>

                                                                                        <%--<asp:TextBox ID="txtAcCode" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                                MaxLength="15" TabIndex="8" ReadOnly="true" ></asp:TextBox>--%>
                                                                                    </div>
                                                                                </div>
                                                                            </div>



                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- END BASIC PORTLET-->
                                                </div>

                                                <div class="span6" id="dvDecomm" runat="server">
                                                    <!-- BEGIN BASIC PORTLET-->
                                                    <div class="widget blue">
                                                        <div class="widget-title">
                                                            <h4><i class="icon-reorder"></i>Decommissioning</h4>
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
                                                                        <div class="span5">
                                                                            <div class="control-group">
                                                                                <label class="control-label">Work Order No <span class="Mandotary">* </span><span class="Mandotary">Upper Case Only</span> </label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDeWoNo1" runat="server" MaxLength="5" TabIndex="5" Width="45px" onkeypress="javascript:return allLetter(event)" onpaste="return false"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtDeWoNo2" runat="server" MaxLength="1" TabIndex="5" Width="45px" onkeypress="javascript:return isHypen(event)" onpaste="return false"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtDeWoNo3" runat="server" MaxLength="6" TabIndex="5" Width="45px" onkeypress="javascript:return OnlyNumber(event)" onpaste="return false"></asp:TextBox>
                                                                                        <%-- <asp:TextBox ID="txtDeWoNo" runat="server" MaxLength="17" TabIndex="9"   ></asp:TextBox>    --%>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Date  <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDeDate" runat="server" MaxLength="10" TabIndex="10"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender_txtDeDate" runat="server" CssClass="cal_Theme1"
                                                                                            Format="dd/MM/yyyy" TargetControlID="txtDeDate">
                                                                                        </ajax:CalendarExtender>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">Amount <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDeAmount" runat="server" MaxLength="10"
                                                                                            onkeypress="javascript:return AllowNumber(this,event);" TabIndex="11"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="control-group">
                                                                                <label class="control-label">A/C Code <span class="Mandotary">*</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append">
                                                                                        <asp:TextBox ID="txtDecAccCode" runat="server" onkeypress="javascript:return OnlyNumber(event);"
                                                                                            MaxLength="15" TabIndex="12" ReadOnly="true"></asp:TextBox>

                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- END BASIC PORTLET-->
                                                </div>


                                            </div>

                                            <div class="space20"></div>

                                            <%--  </div>--%>
                                        </div>
                                    </asp:Panel>
                                    <!-- END SAMPLE FORM PORTLET-->

                                </div>


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

                    <div class="text-center">

                        <div class="span5"></div>
                        <div class="span1">
                            <asp:Button ID="cmdSave" runat="server" Text="Save"
                                CssClass="btn btn-success"
                                OnClick="cmdSave_Click" TabIndex="13" />
                        </div>
                        <div style="margin-left: -17px;" class="span2">
                            <asp:Button ID="cmdViewEstimate" runat="server" Text="View Estimate"
                                CssClass="btn btn-primary"
                                OnClick="cmdViewEstimate_Click" TabIndex="13" />
                        </div>
                        <%-- <div class="span1"></div>--%>
                        <div class="span5">
                            <asp:Button ID="cmdReset" runat="server" Text="Reset" Visible="false"
                                CssClass="btn btn-primary" OnClick="cmdReset_Click" TabIndex="14" /><br />
                            <br />
                        </div>
                        <div class="span7"></div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>


                </div>
            </div>
        </div>

        <!-- END PAGE CONTENT-->
    </div>


</asp:Content>

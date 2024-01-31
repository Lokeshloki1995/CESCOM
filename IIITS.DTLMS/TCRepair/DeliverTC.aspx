<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DeliverTC.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.DeliverTC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateForm() {
            if (document.getElementById('<%= txtRVNo.ClientID %>').value.length != "11") {
                alert('RV number should be equal to 11 digits')
                document.getElementById('<%= txtRVNo.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtChallenNo.ClientID %>').value == "") {
                alert('Please enter Deliver challan No. ')
                document.getElementById('<%= txtChallenNo.ClientID %>').focus()
                return false
            }
            debugger;
            if (document.getElementById('<%= cmbVerifiedBy.ClientID %>').value == "--Select--") {
                alert('Please select Verified by ')
                document.getElementById('<%= cmbVerifiedBy.ClientID %>').focus()
                return false
            }
        }

        function ValidateRVNumber() {
            if (document.getElementById('<%= txtRVNo.ClientID %>').value.length != "11") {
                alert('RV number should be equal to 11 digits')
                return false
            }
        }
        //OMNUM allow to search chars, nums and -/
        function characterAndnumbersOm(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }

    </script>

    <style type="text/css">
        .handPointer {
            cursor: pointer;
        }

        .blockpointer {
            cursor: not-allowed;
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
                    <h3 class="page-title">Receive Transformers
                    </h3>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
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
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='DeliverPendingSearch.aspx'; return false;"
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
                            <h4>
                                <i class="icon-reorder"></i>Receive Transformers</h4>
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
                                                    Store<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" Enabled="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Verified By<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbVerifiedBy" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Deliver Challen No.<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtChallenNo" runat="server" MaxLength="30"></asp:TextBox>
                                                        <asp:TextBox ID="txtRepairMasterId" runat="server" MaxLength="10" Visible="false"
                                                            Width="20px"> </asp:TextBox>
                                                        <asp:TextBox ID="txtInsResultId" runat="server" Visible="false" Width="20px"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Item Code.<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <asp:DropDownList runat="server" ID="cmbItemCode"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">



                                            <div class="control-group">
                                                <label class="control-label">
                                                    Deliver Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDeliverdate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="DeliverCalender" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDeliverdate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    DTLMS Auto RV No<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRVNo" ToolTip="This RV number wont be shown in MMS Report" runat="server" MaxLength="11" onblur="javascript: return ValidateRVNumber()"></asp:TextBox>
                                                    </div>
                                                    <div class="input-append">
                                                        <label style="color: red">Since its DTLMS Auto RV Number it won't reflect in MMS Report  </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    RV Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRVDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="RVDateCalender" runat="server" CssClass="cal_Theme1" TargetControlID="txtRVDate"
                                                            Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" runat="server" id="divOmNo" visible="false">
                                                <label class="control-label">
                                                    OM No<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOMNo" runat="server" MaxLength="11" onkeypress="return characterAndnumbersOm(event)"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" runat="server" id="divOmDate" visible="false">
                                                <label class="control-label">
                                                    OM Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOMDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="OMDateCalender" runat="server" CssClass="cal_Theme1" TargetControlID="txtOMDate"
                                                            Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
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
                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-reorder"></i>Receive Pending Transformers</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <div id="div2" style="overflow: scroll; height: 450px; width: 100%!important;" runat="server">
                                        <asp:GridView ID="grdReceivePending" AutoGenerateColumns="false" PageSize="10" CssClass="table table-striped table-bordered table-advance table-hover"
                                            AllowPaging="false" runat="server" ShowHeaderWhenEmpty="True"
                                            EmptyDataText="No Records Found"
                                            OnRowDataBound="grdReceivePending_RowDataBound"
                                            OnRowCommand="grdReceivePending_RowCommand">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="Repair Details Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRepairDetailsId" runat="server" Text='<%# Bind("RSD_ID") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="STATUS" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInsResult" runat="server" Text='<%# Bind("STATE") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField AccessibleHeaderText="ITEMTYPE" HeaderText="Item Type" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemType" runat="server" Text='<%# Bind("ITEM_TYPE") %>' Style="word-break: break-all;"
                                                            Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>



                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;"
                                                            Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                            Width="120px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmake" runat="server" Text='<%# Bind("MAKE") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("CAPACITY") %>' Style="word-break: break-all;"
                                                            Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                            Width="90px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier / Repairer"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblname" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="warranty Period">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWarentyPeriod" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>' Style="word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="GUARENTEE_TYPE" HeaderText="GUARENTEE TYPE" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="cmbGuarantyType" runat="server" Style="width: 100px">
                                                            <asp:ListItem>--Select--</asp:ListItem>
                                                            <%--<asp:ListItem Value="AGP">AGP</asp:ListItem>--%>
                                                            <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                            <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:Label ID="lblWarrentyType" runat="server" Text='<%# Bind("WARRENTY_TYPE") %>' Style="word-break: break-all;"
                                                            ></asp:Label>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="STATUS" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtWarrenty" runat="server" Text='<%# Bind("STATUS") %>' Style="width: 100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="WARRANTY_TYPE" HeaderText="warranty Period (In Year)" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTCWarrenty" runat="server" Text='<%# Bind("TC_WARRENTY") %>' Style="width: 100px" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="REMARKS" HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("REMARKS") %>' style="width:100px"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("REMARKS") %>' Height="40px" TextMode="MultiLine" Style="resize: none" MaxLength="200" ReadOnly="true"></asp:TextBox>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="REMARKSEE" HeaderText="Remarks by EE">
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("REMARKS") %>' style="width:100px"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtRemarksEE" runat="server" Text='<%# Bind("REMARKS_EE") %>' Height="40px" TextMode="MultiLine" Style="resize: none" MaxLength="200" ReadOnly="true"></asp:TextBox>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Test Document" Visible="true">
                                                    <%--<ItemTemplate>
                                                        <asp:LinkButton ID="lnkDwnld" runat="server" CommandName="Download">
                                                        <img src="../img/Manual/Pdficon.png" style="width:20px" />Download Test Report</asp:LinkButton>
                                                        <asp:LinkButton ID="lnkNodownload" runat="server" Enabled="false">
                                                        <img src="../img/Manual/nodoc.png" style="width:20px" />Report Not Available</asp:LinkButton>
                                                            
                                                    </ItemTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDwnld" runat="server" CommandArgument='<%# Eval("IND_DOC") %>' OnClick="DownloadFile">
                                                        <i class="fa fa-eye" style="color:black!important;margin-left:15px;"></i></asp:LinkButton>

                                                        <asp:LinkButton ID="lnkNodownload" runat="server" Enabled="false">
                                                        <i class="fa fa-eye-slash" style="color:black!important;margin-left:15px;"></i></asp:LinkButton>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="OMNO" HeaderText="OM No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtOMNo" runat="server" Text='<%# Bind("OM_No") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="OMDATE" HeaderText="OM date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtOMdate" runat="server" Text='<%# Bind("OM_DATE") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="OM Doc">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownloadOM_Doc" runat="server"
                                                            Text="VIEW" OnClick="DownloadFile" Style="word-break: break-all;" Width="50px"
                                                            CommandArgument='<%# Eval("OM_Doc") %>'><i class="fa fa-eye" style="margin-left:15px;"></i></asp:LinkButton>

                                                        <asp:LinkButton ID="lnkNoDnlOM_Doc" runat="server" Enabled="false"
                                                            Style="word-break: break-all;" Width="50px">
                                                            <i class="fa fa-eye-slash" style="margin-left:15px;"></i></asp:LinkButton>

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

                <div class="form-horizontal" align="center">
                    <div class="span3"></div>
                    <div class="span3">
                        <asp:Button ID="cmdLoad" runat="server" Text="Load Updated DTR" CssClass="btn btn-success" OnClick="cmdLoad_Click" />
                    </div>
                    <div class="span7"></div>
                </div>

                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-reorder"></i>Receive Updated Item Code of Transformers</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <div id="div1" style="overflow: scroll; height: 450px;" runat="server">
                                        <asp:GridView ID="grdUpdatedTcItemCode" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                                            AllowPaging="false" runat="server" ShowHeaderWhenEmpty="True"
                                            EmptyDataText="No Records Found"
                                            OnRowDataBound="grdReceivePending_RowDataBound"
                                            OnRowCommand="grdReceivePending_RowCommand">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="Repair Details Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRepairDetailsId" runat="server" Text='<%# Bind("RSD_ID") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="STATE" HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInsResult" runat="server" Text='<%# Bind("STATE") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;"
                                                            Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                            Width="120px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmake" runat="server" Text='<%# Bind("MAKE") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("CAPACITY") %>' Style="word-break: break-all;"
                                                            Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                            Width="90px"></asp:Label>
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

                                                <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier / Repairer"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblname" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="warranty Period">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWarentyPeriod" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>' Style="word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="GUARENTEE_TYPE" HeaderText="GUARENTEE TYPE">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="cmbGuarantyType" runat="server" Style="width: 100px">
                                                            <asp:ListItem>--Select--</asp:ListItem>
                                                            <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                            <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                            <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:Label ID="lblWarrentyType" runat="server" Text='<%# Bind("WARRENTY_TYPE") %>' Style="word-break: break-all;"
                                                            ></asp:Label>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="WARRANTY_TYPE" HeaderText="warranty Period (In Year)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTCWarrenty" runat="server" Text='<%# Bind("TC_WARRENTY") %>' Style="width: 100px" MaxLength="1" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="STATUS" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtWarrenty" runat="server" Text='<%# Bind("STATUS") %>' Style="width: 100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>




                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>


                        </div>

                        <div class="space20">
                        </div>
                        <div class="form-horizontal" align="right">
                            <div>
                            </div>
                            <div class="span6">
                                <asp:Button ID="cmdSave" runat="server" Text="Recieve" Visible="true" CssClass="btn btn-success"
                                    OnClick="cmdSave_Click" OnClientClick="Javascript:return ValidateForm()" />

                                <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                    OnClick="cmdReset_Click" />
                            </div>
                            <div class="span7">
                            </div>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </div>

                    </div>
                </div>

                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-reorder"></i>Recieved Transformers</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <asp:GridView ID="grdRecievedDTr" AutoGenerateColumns="false" PageSize="10" CssClass="table table-striped table-bordered table-advance table-hover"
                                        AllowPaging="true" runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="Repair Details Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepairDetailsId" runat="server" Text='<%# Bind("RSD_ID") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmake" runat="server" Text='<%# Bind("MAKE") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("CAPACITY") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier / Repairer">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblname" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
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
            <!-- END PAGE CONTENT-->
        </div>
    </div>



</asp:Content>

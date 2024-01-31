<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FaultTCSearch.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.FaultTCSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ValidateMyForm() {

            if (document.getElementById('<%= cmbStore.ClientID %>').value == "--Select--") {
                alert('Select Store to Search')
                document.getElementById('<%= cmbStore.ClientID %>').focus()
                return false
            }
        }

        function ConfirmDelete() {
            var result = confirm('Are you sure you want to Remove?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }

        function onlyNumbers(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 47)
                return false;
            else
                return true;
        }

        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^/0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }

        function DontAllowPlusAndSingleQuotes(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode >= 33 && charCode <= 44 || (charCode >= 58 && charCode <= 64) || (charCode >= 91 && charCode <= 94) || (charCode == 96))
                return false;
            else
                return true;
        }
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Faulty Transformer Search
                    </h3>
                    <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
                            <h4><i class="icon-reorder"></i>Faulty Transformer Search</h4>
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
                                                <label class="control-label">Make</label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbMake" runat="server">
                                                        </asp:DropDownList>


                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Store<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" TabIndex="3" Enabled="false">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Star Rating </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbstarrating" runat="server" TabIndex="4">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>


                                            <%-- 
                                                <asp:DropDownList ID="cmbstarrating" AutoPostBack="true" runat="server" TabIndex="4" OnSelectedIndexChanged="cmbstarrating_Click" >

                                               <div class="control-group">
                                                <label class="control-label">Oil quantity <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtOilQuantity" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Capacity(in KVA)<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbCapacity" AutoPostBack="true" runat="server" TabIndex="1" OnSelectedIndexChanged="cmdCapacity_Click">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <%-- <div class="control-group">
                        <label class="control-label">Suplier/Repairer</label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:DropDownList ID="cmbSupplier" runat="server" TabIndex="2">
                                </asp:DropDownList>  
                                                      
                            </div>
                        </div>
                      </div>--%>
                                            <div class="control-group">
                                                <label class="control-label">Guarantee Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbGuarantyType" runat="server">
                                                            <asp:ListItem>Select</asp:ListItem>
                                                            <asp:ListItem Value="AGP">AGP</asp:ListItem>
                                                            <asp:ListItem Value="WGP">WGP</asp:ListItem>
                                                            <asp:ListItem Value="WRGP">WRGP</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <div id="divItemCode" class="control-group" runat="server" visible="false">
                                                    <label class="control-label">Item Code <span class="Mandotary">*</span> </label>
                                                    <%--<span class="Mandotary"> *</span>--%>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList runat="server" ID="cmbItemCode"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="text-center" align="center">



                                        <asp:Button ID="cmdLoad" runat="server" Text="Load Fault Transformer" OnClientClick="javascript:return ValidateMyForm()"
                                            CssClass="btn btn-primary" OnClick="cmdLoad_Click" TabIndex="4" />


                                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" TabIndex="5" OnClick="cmdReset_Click" /><br />

                                        <br />
                                        <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-info"
                                            OnClick="Export_ClickTcsearch" /><br />

                                        <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                    </div>
                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->

                            <asp:GridView ID="grdFaultTC" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false" PageSize="10" DataKeyNames="TC_ID" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnRowCommand="grdFaultTC_RowCommand"
                                OnPageIndexChanging="grdFaultTC_PageIndexChanging" TabIndex="6" OnSorting="grdFaultTC_Sorting">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtDTRCode" onkeypress="return onlyNumbers(event)" onchange="return cleanSpecialAndChar(this)" runat="server" placeholder="Enter DTR CODE" Width="80px"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" SortExpression="TC_SLNO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTCSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all" Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtSlNo" onpaste="return false" onkeypress="return DontAllowPlusAndSingleQuotes(event)" runat="server" placeholder="Enter DTR SLNO" Width="120px"></asp:TextBox>
                                        </FooterTemplate>
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
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">

                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">

                                        <ItemTemplate>

                                            <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="TC_PURCHASE_DATE" HeaderText="Purchase Date" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("TC_PURCHASE_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="Guarantee Period" Visible="false">


                                        <ItemTemplate>
                                            <asp:Label ID="lblWarrenty" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" Visible="false">

                                        <ItemTemplate>
                                            <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="RepSentcount" HeaderText="Sent To Repairer" Visible="true">

                                        <ItemTemplate>
                                            <asp:Label ID="lblRepSentcount" runat="server" Text='<%# Bind("RCOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_GUARANTY_TYPE" HeaderText="Guarantee Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblguarentee" runat="server" Text='<%# Bind("TC_GUARANTY_TYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="TC_STAR_RATE" HeaderText="Star Rating">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstarrating" runat="server" Text='<%# Bind("TC_STAR_RATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  

                                    <asp:TemplateField AccessibleHeaderText="Status" HeaderText="STATUS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all" Width="80px"></asp:Label>
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

                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">

                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span3">
                                            <asp:Button ID="cmdLoadItemCode" runat="server" Text="Click To Add TC with Item Code"
                                                CssClass="btn btn-primary" OnClick="cmdLoadItemCode_Click" Visible="false" TabIndex="7" />
                                        </div>
                                        <%-- <div class="span1"></div>--%>
                                        <div class="space20"></div>
                                    </div>

                                </div>
                            </div>

                            <%-- <div class="widget-body">
                                <div class ="space5"> </div>
                                <div class="space10"> 
                                    <asp:Button Id="" Visible="false" Text="Click To Add TC with Item Code" CssClass="btn btn-primary" runat="server" OnClick="cmdLoadItemCode_Click" />
                                </div>
                             </div>--%>


                            <!--   BEGIN MUTLI GRID -->
                            <div id="divgrdItem" style="overflow: scroll" runat="server" visible="false">

                                <asp:GridView ID="grdItemCode" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                    AutoGenerateColumns="false" OnRowCommand="grdItemCode_RowCommand" DataKeyNames="TC_ID"
                                    CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server"
                                    TabIndex="6">
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

                                        <asp:TemplateField AccessibleHeaderText="IT_ID" HeaderText="Item ID" SortExpression="IT_ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemId" runat="server" Text='<%# Bind("IT_ID") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="IT_CODE" HeaderText="Item Code" SortExpression="code">

                                            <ItemTemplate>
                                                <asp:Label ID="lblItemcode" runat="server" Text='<%# Bind("IT_CODE") %>' Style="word-break: break-all" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="IT_NAME" HeaderText="Item Name" SortExpression="IT_NAME" Visible="false">

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

                                        <asp:TemplateField AccessibleHeaderText="TC_GUARANTY_TYPE" HeaderText="Guarantee Type" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblguarentee" runat="server" Text='<%# Bind("TC_GUARANTY_TYPE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_STAR_RATE" HeaderText="Star Rating">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstarrating" runat="server" Text='<%# Bind("TC_STAR_RATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imgBtnDelete" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                    CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();"></asp:ImageButton>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                            </div>
                            <!-- END MULTI GRID -->



                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">

                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span3">
                                            <asp:Button ID="cmdSend" runat="server" Text="Click to send for Supplier/Repairer"
                                                CssClass="btn btn-primary" OnClick="cmdSend_Click" Visible="false" TabIndex="7" />
                                        </div>
                                        <%-- <div class="span1"></div>--%>
                                        <div class="space20"></div>
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

        <!-- MODAL-->
        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                            &times;odal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                </div>
            </div>
                            <button type="button" class="close" data-dismiss="modal">
                                &times;</button>
                            <h4 class="modal-title">Help</h4>
                    </div>
                    <div class="modal-body">
                        <ul>
                            <li>This Web Page Can Be Used To Search Faulty Transformer By Clicking <u>Load Fault Transformer</u> Button</li>
                            <li>Transformer Can Be Searched By Using Make, Capacity and Guarantee Type Filter Option</li>
                            <li>Once The Faulty Transformer List Is Loaded User Can Select The Transformer By Clicking CheckBoxes</li>
                            <li>After Selection Process User Can Click On <u>Click to send Supplier/Repairer</u> Button
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

    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PendingAgencyTest.aspx.cs" Inherits="IIITS.DTLMS.OilFlow.PendingAgencyTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateMyForm() {

            if (document.getElementById('<%= txtPONo.ClientID %>').value.trim() == "") {
                alert('Enter Valid Purchase Order No')
                document.getElementById('<%= txtPONo.ClientID %>').focus()
                return false
            }
        }

        function ValidateMyFormnew() {

            if (document.getElementById('<%=cmbCircle.ClientID %>').value.trim() == "--Select--") {
                alert('Please Select Circle')
                document.getElementById('<%= cmbCircle.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%=cmbDivision.ClientID %>').value.trim() == "--Select--") {
                alert('Please Select Division')
                document.getElementById('<%= cmbDivision.ClientID %>').focus()
                return false
            }
        }


        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^/0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
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
<%--    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>--%>
       <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Oil Inspection at Repair Center
                        <%--                           <div style="float:right;margin-top:20px;margin-right:12px" >
                      <asp:Button ID="cmdClose" runat="server" Text="Close" 
                       OnClientClick="javascript:window.location.href='Dashboard.aspx'; return false;"
                       CssClass="btn btn-danger" /></div>--%>
                        <div style="float: right; margin-top: 20px; margin-right: 12px">
                            <asp:Button ID="cmdclose" runat="server" Text="Close"
                                CssClass="btn btn-primary" OnClick="cmdClose_Click" />
                        </div>
                    </h3>

                    <%--<a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                    <a style="margin-right: -2px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
                            <h4><i class="icon-reorder"></i>Pending For Agency Test </h4>
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
                                                <label class="control-label">Circle<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server"
                                                            AutoPostBack="true" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="span3"></div>
                                            <div class="span3"></div>
                                            <div class="span1"></div>
                                             <div class="span1"></div>
                                             <div class="span1"></div> 
                                         <div class="span1"></div>
                                            <div class="span1">
                                                <asp:Button ID="btnProceed" runat="server" Text="Proceed" CssClass="btn btn-primary" OnClick="btnProceed_Click" />
                                            </div>
                                            <div class="space20"></div>

                                            <div class="control-group">
                                                <label class="control-label">Purchase Order No <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtPONo" runat="server" MaxLength="50"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" runat="server" Text="S" CssClass="btn btn-primary"
                                                            TabIndex="2" />
                                                    </div>
                                                </div>
                                            </div>




                                        </div>
                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>


                                    </div>


                                    <div class="space20"></div>
                                    <div class="space20"></div>
                                    <div class="form-horizontal" align="center">
                                        <div class="span3"></div>
                                        <div class="span3">
                                            <asp:Button ID="cmdLoad" runat="server" Text="Load" OnClientClick="javascript:return ValidateMyForm()"
                                                CssClass="btn btn-primary" OnClick="cmdLoad_Click" />
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="cmdReset_Click" />
                                        </div>
                                        <%--                     <div class="span1">
                         <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                        OnClick="Export_ClickTestPendingSearch" OnClientClick="javascript:return ValidateMyForm()"/><br />
                         </div>--%>
                                        <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <%-- <div class="space20"></div>--%>
                            <!-- END FORM-->

                            <asp:GridView ID="grdPendingTc" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false" PageSize="10" DataKeyNames="OSD_ID"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnPageIndexChanging="grdPendingTc_PageIndexChanging"
                                TabIndex="5" OnSorting="grdPendingTc_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>



                                    <asp:TemplateField AccessibleHeaderText="OSD_ID" HeaderText="Po ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoId" runat="server" Text='<%# Bind("OSD_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="PO NO">
                                        <ItemTemplate>
                                            <%--  <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("OSD_PO_NO") %>' style="word-break: break-all;" width="150px"></asp:Label>--%>
                                            <asp:Label ID="lblPONo" runat="server" Text='<%# Bind("OSD_PO_NO") %>' Style="word-break: break-all;" Width="100px"></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OSD_PO_DATE" HeaderText="PO Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("OSD_PO_DATE") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OSD_INVOICE_NO" HeaderText="Invoice No" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoice" runat="server" Text='<%# Bind("OSD_INVOICE_NO") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText="Oil Quantity(in Kltr)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OSD_OFFICE_CODE" HeaderText="Division Code" Visible="true">

                                        <ItemTemplate>
                                            <asp:Label ID="lbldivNo" runat="server" Text='<%# Bind("OSD_OFFICE_CODE") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
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
                                            <asp:Button ID="cmdDeliver" runat="server" Text="Click to Inspect"
                                                CssClass="btn btn-primary" Visible="false" OnClick="cmdDeliver_Click"
                                                TabIndex="6" />
                                        </div>
                                        <%-- <div class="span1"></div>--%>
                                        <div class="space20"></div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>

        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Pending for Testing</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">

                        <%--                            <div class="">
                         <asp:Button ID="Button1" runat="server" visible="false" Text="Export Excel" CssClass="btn btn-primary"
                        OnClick="Export_ClickPendingTesting" /><br />
                         </div>--%>
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <asp:GridView ID="grdTestPending" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false" PageSize="10"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server"
                                TabIndex="5" OnPageIndexChanging="grdTestPending_PageIndexChanging" OnSorting="grdTestPending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>


                                    <asp:TemplateField AccessibleHeaderText="OSD_PO_NO" HeaderText="PO No" SortExpression="RSM_PO_NO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("OSD_PO_NO") %>' Style="word-break: break-all;" Width="70px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="PODATE" HeaderText="PO Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("OSD_PO_DATE") %>' Style="word-break: break-all;" Width="130px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText="Oil Quantity(in Kltr)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="OSD_OFFICE_CODE" HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOfficecode" runat="server" Text='<%# Bind("OSD_OFFICE_CODE") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>
                        </div>
                    </div>



                </div>
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
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To Inspect The Oil at Repair Center
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>User Need To Enter Or Search PO No in Purchase Order No Textbox
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Once Purchase Order No is Entered Click On <u>Load</u> Button,It will List Oil Which Are Pending For Inspection for That Particular PO No
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>User Can Select Checkboxes For The Oil which They Want To Inspect after That They Need To Click <u>Click To Inspect</u> Button To Inspect The Oil
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


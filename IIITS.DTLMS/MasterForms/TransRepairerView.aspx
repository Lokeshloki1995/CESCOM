<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TransRepairerView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TransRepairerView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip(); // added css in font-awesome.min.css line 43 and 405
        });
        function ConfirmStatus(status) {

            var result = confirm('Are you sure,Do you want to ' + status + ' Repairer?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }

        //Charactes and space . / to search Tc Name
        function characterAndspecialTcRe(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 46 && charCode != 47) {

                return false;
            }
            return true;
        }
        //Remove Numbers, Special characters except space to search Tc Name
        function cleanSpecialAndNumTcRe(t) {

            t.value = t.value.toString().replace(/[^./a-zA-Z \t\n\r]+/g, '');


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN THEME CUSTOMIZER-->

                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">Repairer View At Taluk Level 
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
                        <h4><i class="icon-reorder"></i>Repairer Details View </h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">


                        <div style="float: left">
                            <div class="span1">
                                <asp:Button ID="cmdNew" runat="server" Text="Create New Repairer/Map Existing Repairer to Taluk"
                                    CssClass="btn btn-primary" OnClick="cmdNew_Click" BackColor="Orange" />
                            </div>
                        </div>
                        <div style="float: none">
                        </div>
                        <div style="float: right">
                            <div class="span1">
                                <div class="span1">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-primary"
                                        OnClick="Export_clickRepairer" BackColor="Green" /><br />
                                </div>

                            </div>
                        </div>



                        <div class="space20"></div>

                        <div class="span1">
                            <div class="control-group">
                                <label class="control-label">Division</label>
                                <div class="controls">
                                    <div class="input-append">
                                        <asp:DropDownList ID="cmbdiv" Width="300px" runat="server" AutoPostBack="true" TabIndex="2" OnSelectedIndexChanged="cmbdiv_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- END FORM-->




                        <asp:GridView ID="grdRepairer" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                            ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true"
                            CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" OnPageIndexChanging="grdRepairer_PageIndexChanging" OnRowCommand="grdRepairer_RowCommand"
                            OnRowDataBound="grdRepairer_RowDataBound" OnSorting="grdRepairer_Sorting" AllowSorting="true">
                            <HeaderStyle CssClass="both" />
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="TR_ID" HeaderText="Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRepairId" runat="server" Text='<%# Bind("TR_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Repairer Name" Visible="true" SortExpression="TR_NAME" ItemStyle-Width="500px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("TR_NAME") %>' Style="word-break: break-all;" Width="400px"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                            <asp:TextBox ID="txtRepairerName" runat="server" placeholder="Enter Repairer Name" Width="100px" onkeypress="return characterAndspecialTcRe(event)"
                                                onchange="return cleanSpecialAndNumTcRe(this)"></asp:TextBox>
                                        </asp:Panel>
                                    </FooterTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="TS_ADDRESS" HeaderText="Taluk Addresss" Visible="true" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("RA_TALQ_ADDR") %>' Style="word-break: break-all;" Width="300px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="TS_PHONE" HeaderText="Contact no" Visible="true" ItemStyle-Width="130px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblphone" runat="server" Text='<%# Bind("RA_CONTACT_NO") %>' Style="word-break: break-all;" Width="130px"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION"
                                    Visible="true" SortExpression="DIVISION">

                                    <ItemTemplate>
                                        <asp:Label ID="lblOfficeCode" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="TALUK"
                                    Visible="true" SortExpression="DIVISION">

                                    <ItemTemplate>
                                        <asp:Label ID="lbltalukCode" runat="server" Text='<%# Bind("TALUK") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField AccessibleHeaderText="TS_BLACK_LISTED" HeaderText="Block List" Visible="true">

                                    <ItemTemplate>
                                        <asp:Label ID="lblblacklist" runat="server" Text='<%# Bind("RA_BLACK_LISTED") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField AccessibleHeaderText="TS_BLACKED_UPTO" HeaderText="Black Listed upto"
                                    Visible="false">

                                    <ItemTemplate>
                                        <asp:Label ID="lbldate" runat="server" Text='<%# Bind("RA_BLACK_LISTED_UPTO") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                Width="12px" CommandName="create" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Status" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("RA_STATUS") %>'></asp:Label>
                                        <center>
                                            <asp:ImageButton Visible="false" ID="imgDeactive" runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status"
                                                ToolTip="Click to Activate Repairer" OnClientClick="return ConfirmStatus('Activate');" Width="10px" />
                                            <asp:ImageButton Visible="false" ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif" CommandName="status"
                                                ToolTip="Click to DeActivate Repairer" OnClientClick="return ConfirmStatus('Deactivate');" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Font-Size="Medium" Width="15px" />
                            <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" Font-Bold="true" Font-Size="Medium" Width="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:GridView>

                        <ajax:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="cmdClose"
                            PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
                        <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
                            <div style="display: none">
                                <asp:Button ID="btnshow" runat="server" Text="Button" />
                            </div>
                            <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="200px" Width="550px">
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4>Reason for Deactivation </h4>
                                        <div class="space20"></div>
                                        <%--<div class="row-fluid">--%>
                                        <div class="span1"></div>
                                        <div class="space20">
                                            <div class="span1"></div>

                                            <div class="span5">

                                                <div class="control-group" style="font-weight: bold">
                                                    <label class="control-label">Reason<span class="Mandotary"> *</span></label>

                                                    <div class="controls">
                                                        <div class="input-append" align="center">

                                                            <asp:TextBox ID="txtReason" runat="server" MaxLength="500" TabIndex="4" TextMode="MultiLine" Style="resize: none"
                                                                onkeyup="javascript:ValidateTextlimit(this,100)"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div align="center">
                                                    <div class="control-group" style="font-weight: bold">
                                                        <label class="control-label">Effect From<span class="Mandotary"> *</span></label>
                                                        <div class="controls">
                                                            <div class="input-append" align="center">

                                                                <asp:TextBox ID="txtEffectFrom" runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server"
                                                                    CssClass="cal_Theme1" TargetControlID="txtEffectFrom" Format="dd/MM/yyyy">
                                                                </ajax:CalendarExtender>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                                <%--<div align="center">
                                                                            <div class="control-group" style="font-weight: bold">
                                                                                <label class="control-label"> Effect Till <span class="Mandotary"> * </span> </label>
                                                                                 <div class="controls">
                                                                                     <div class="input-append" align="center">
                                                                                         <asp:TextBox ID ="txtEffectTill" runat="server" MaxLength ="10" ></asp:TextBox>
                                                                                         <ajax:CalendarExtender ID="effectTillCalendar" runat="server" CssClass="cal_Theme1" TargetControlID ="txtEffectTill" Format="dd/MM/yyyy">
                                                                                             </ajax:CalendarExtender>
                                                                                     </div>
                                                                                     </div>
                                                                                </div>
                                                                            </div>--%>

                                                <div class="span5">
                                                    <div class="control-group" style="font-weight: bold">

                                                        <div class="controls">
                                                            <div class="input-append">

                                                                <div class="span10">
                                                                    <asp:Button ID="cmdSubmit" runat="server" CssClass="btn btn-primary"
                                                                        OnClick="cmdSubmit_Click" TabIndex="10" Text="Submit" />
                                                                </div>
                                                                <div class="span2">
                                                                    <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-primary"
                                                                        TabIndex="10" Text="Close" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="space20" align="center">

                                                    <div class="form-horizontal" align="center">
                                                        <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="space20"></div>
                                    <div class="space20"></div>

                                </div>
                            </asp:Panel>
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
                    <ul>
                        <li>This Web Page Can Be Used To View All Existing Repairer and New Repairer Can Be Added</li>
                        <li>Existing Repairer Details Can Be Edited By Clicking Edit Button</li>
                        <li>New Repairer Can Be Added By Clicking New Repairer Button
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
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="ScrapTest.aspx.cs" Inherits="IIITS.DTLMS.ScrapEntry.ScrapTest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ConfirmDelete() {
            var result = confirm('Are you sure you want to Remove?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
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
     <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid" style="width: 1100px;">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Faulty Transformer Search
                    </h3>
                    <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Faulty Transformer Search</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div style="width: 300px; margin-left: 45px; padding">
                                        <div style="float: left">
                                            <asp:RadioButton ID="rdbPendingForTest" GroupName="a" CssClass="radio" Checked="true"
                                                runat="server" Text="Pending For Test" Font-Size="Medium" />
                                        </div>
                                        <div style="float: right">
                                            <asp:RadioButton ID="rdbAlreadyTested" GroupName="a" CssClass="radio" runat="server"
                                                Text="Already Tested" Font-Size="Medium" />
                                        </div>
                                    </div>
                                    <div class="row-fluid">
                                        <div style="height: 50px;">
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Store</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" TabIndex="3" Enabled="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Make</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMake" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Capacity(in KVA)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCapacity" runat="server" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="display: none">
                                                <label class="control-label">
                                                    Repairer</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbRepairer" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group" style="display: none">
                                                <label class="control-label">
                                                    Supplier</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSupplier" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span1">
                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="text-center" align="center">
                                       
                                       
                                            <asp:Button ID="cmdLoad" runat="server" Text="Load Fault Transformer" CssClass="btn btn-primary"
                                                OnClick="cmdLoad_Click" TabIndex="4" />
                                    
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                                TabIndex="5" OnClick="cmdReset_Click" /><br />
                                       <br />
                                            <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info"
                                                OnClick="Export_clickscraptest" /><br />
                                     
                                        <div class="span7">
                                         </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="space20">
                            </div>
                            <!-- END FORM-->
                            <div style="width: 1080px; overflow: auto;">
                                <asp:GridView ID="grdFaultTC" AutoGenerateColumns="False" DataKeyNames="TC_ID" CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" OnRowCommand="grdFaultTC_RowCommand" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" TabIndex="9" OnRowDataBound="grdFaultTC_RowDataBound"
                                    HorizontalAlign="Center" OnSorting="grdFaultTC_Sorting" AllowSorting="true">
                                    <HeaderStyle CssClass="both" />
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all"
                                                    Width="90px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCSlno" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all"
                                                    Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all"
                                                    Width="80px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_PURCHASE_DATE" HeaderText="Purchase Date"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("TC_PURCHASE_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Supplier" SortExpression="TS_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("TS_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Testing Result">
                                            <ItemTemplate>
                                                <div style="float: right; width: 120px; margin-left: 15px;">
                                                    <asp:RadioButton ID="rdbScrap" runat="server" Text="Scarp" GroupName="a" CssClass="radio" />
                                                    <asp:RadioButton ID="rdbTest" runat="server" Text="Send For Test" GroupName="a" CssClass="radio" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_STATUS" HeaderText="TC STATUS" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCStatus" runat="server" Text='<%# Bind("TC_STATUS") %>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="Test Document" Visible="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDwnld" runat="server" CommandName="Download">
                                                 <img src="../img/Manual/Pdficon.png" style="width:20px" />Download Test Report</asp:LinkButton>
                                                <asp:LinkButton ID="lnkNodownload" runat="server" Enabled="false">
                                                 <img src="../img/Manual/nodoc.png" style="width:20px" />Report Not Available</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                        CommandName="Remove" Width="12px" OnClientClick="return ConfirmDelete();" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <div class="form-horizontal" align="center">
                                        <div class="span3">
                                        </div>
                                        <div class="span3">
                                            <asp:Button ID="cmdSend" runat="server" Text="Declare Scrap" CssClass="btn btn-primary"
                                                OnClick="cmdSend_Click" Visible="false" TabIndex="7" />
                                        </div>
                                        <%-- <div class="span1"></div>--%>
                                        <div class="space20">
                                        </div>
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
                    <ul><li>
                    This Web Page Can Be Used To Declare Tc as Scrap ( Once TC is Marked as Scrap in Repairer Center ) .</li>
                  <li> Test Result Can Be Assigned By Selecting Radio Button .</li>
               <li>   Test Report Can Be Downloaded By Clicking <u>Download Report</u> Link
                        </li></ul>
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

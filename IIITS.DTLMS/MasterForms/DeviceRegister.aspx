<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DeviceRegister.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DeviceRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script type="text/javascript">
        function ConfirmStatus(status) {

            var result = confirm('Are you sure,Do you want to ' + status + ' Device?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }

        // Only NUmbers to search
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        //allow only Numbers to paste
        function cleanSpecialAndChar(t) {
            t.value = t.value.toString().replace(/[^0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }

        //allow only characters to enter
        function character(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode > 31 && (charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122)) {

                return false;
            }
            return true;
        }

        //allow only characters to paste
        function cleanSpecialAndNum(t) {
            t.value = t.value.toString().replace(/[^a-zA-Z\t\n\r]+/g, '');
           // alert(" Special charactes and Numbers are not allowed!");
        }


    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ascending th a {
            background: url(img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }
        .btn{
            background:transparent!important
        }
        i.fa.fa-trash{
            color:red!important;
            font-size:14px!important;
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
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Mobile Register View
                    </h3>
                    <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
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
                            <h4><i class="icon-reorder"></i>Mobile Register View</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div style="float: right">
                                <div class="span2">
                                </div>
                                <div class="">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-primary"
                                        OnClick="Export_clickDeviceRegister" /><br />
                                </div>

                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->

                            <asp:GridView ID="grdDeviceRegister"
                                AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" OnPageIndexChanging="grdDeviceRegister_PageIndexChanging"
                                OnRowCommand="grdDeviceRegister_RowCommand" OnRowDataBound="grdDeviceRegister_RowDataBound"
                                ShowFooter="True" OnSorting="grdDeviceRegister_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="MR_REQUEST_BY" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("MR_REQUEST_BY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="MR_ID" HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMrId" runat="server" Text='<%# Bind("MR_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>




                                    <asp:TemplateField AccessibleHeaderText="MU_DEVICE_ID" HeaderText="Device Id" SortExpression="MR_DEVICE_ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIvDeviceId" runat="server" Text='<%# Bind("MR_DEVICE_ID") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtIvDeviceId" runat="server" Width="150px" placeholder="Enter Device Id" ToolTip="Enter Device Id to Search"  ></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField AccessibleHeaderText="US_FULL_NAME" HeaderText="Name" SortExpression="US_FULL_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFullName" runat="server" Text='<%# Bind("US_FULL_NAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtsFullName" runat="server" Width="150px" placeholder="Enter Name" ToolTip="Enter Name to Search" onkeypress="return character(event)" onchange = "cleanSpecialAndNum(this)" ></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="US_MOBILE_NO" HeaderText="MobileNumber" SortExpression="US_MOBILE_NO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMobileNumber" runat="server" Text='<%# Bind("US_MOBILE_NO") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtsPhoneNumber" runat="server" Width="150px" placeholder="Enter Phone Number" ToolTip="Enter Phone Number to Search"   ></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="US_OFFICE_CODE"  HeaderText="Office Code">
                                         <ItemTemplate>
                                            <asp:Label ID="lblOfficeCode" runat="server" Text='<%# Bind("US_OFFICE_CODE") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                             <FooterTemplate>
                                            <asp:TextBox ID="txtsOfficeCode" runat="server" Width="150px" placeholder="Enter Office Code" MaxLength="4" ToolTip="Enter Office code to Search"   onkeypress="return onlyNumbers(event)"  ></asp:TextBox>
                                        </FooterTemplate>
                                         </asp:TemplateField>


                                   

                                    <asp:TemplateField AccessibleHeaderText="RO_NAME"  HeaderText="Role Name">
                                         <ItemTemplate>
                                            <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RO_NAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9" />
                                        </FooterTemplate>
                                         </asp:TemplateField>

                                      

                                    <asp:TemplateField AccessibleHeaderText="MU_CRON" HeaderText="Created On">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Bind("MR_CRON") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                     <asp:TemplateField AccessibleHeaderText="MU_APPROVE_STATUS" HeaderText="Approval Status">

                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Bind("MR_APPROVE_STATUS") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approval">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("MR_APPROVE_STATUS") %>'></asp:Label>
                                            <center>
                                                <asp:ImageButton Visible="false" ID="imgBtnApproval" runat="server" ImageUrl="../img/Manual/Approve.png" Width="15px" Height="15px" CommandName="status"
                                                    ToolTip="Click to Approve Device" OnClientClick="return ConfirmStatus('Enable');" />
                                                <asp:ImageButton Visible="false" ID="imgBtnReject" runat="server" ImageUrl="../img/Manual/Reject.png" Width="15px" Height="15px" CommandName="status"
                                                    ToolTip="Click to Approve Device" OnClientClick="return ConfirmStatus('Disable');" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Delete" Visible="true">
                                        <ItemTemplate>
                                            <center>
                                               <asp:ImageButton Visible="true" ID="imgBtnDelete" runat="server" ImageUrl="../img/Manual/trash.png" Width="15px" Height="15px" CommandName="deleteDeviceId"
                                                    ToolTip="Click to Delete Device" OnClientClick="return ConfirmStatus('Delete');" />
                                              <%-- <button runat="server" id="imbBtnDelete" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                    CausesValidation="false"  CommandName="delete" class="btn btn-mini" title="Search">
    <i class="fa fa-trash"></i> 
</button>--%>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </div>



                        <ajax:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnshow"
                            PopupControlID="pnlControls"
                            BackgroundCssClass="modalBackground" />
                        <div style="width: 100%; vertical-align: middle; height: 369px;"
                            align="center">
                            <div style="display: none">
                                <asp:Button ID="btnshow" runat="server" Text="Button" />
                            </div>
                            <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="362px"
                                Width="434px">
                                <div class="widget blue">
                                    <div class="widget-title">
                                        <h4>Give Reason </h4>
                                        <div class="space20"></div>
                                        <%--<div class="row-fluid">--%>
                                        <div class="span1"></div>


                                        <div class="space20">

                                            <div class="span1"></div>

                                            <div class="span5">

                                                <div class="control-group" style="font-weight: bold">
                                                    <label class="control-label">Reason</label>

                                                    <div class="controls">
                                                        <div class="input-append" align="center">
                                                            <div class="span3"></div>
                                                            <asp:TextBox ID="txtReason" runat="server" MaxLength="500" TabIndex="4" TextMode="MultiLine" Style="resize: none"
                                                                onkeyup="javascript:ValidateTextlimit(this,100)"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>




                                                <div align="center">
                                                    <div class="control-group" style="font-weight: bold">
                                                        <label class="control-label">Effect From</label>
                                                        <div class="controls">
                                                            <div class="input-append" align="center">
                                                                <div class="span3"></div>
                                                                <asp:TextBox ID="txtEffectFrom" runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server"
                                                                    CssClass="cal_Theme1" TargetControlID="txtEffectFrom" Format="dd/MM/yyyy">
                                                                </ajax:CalendarExtender>



                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div>

                                                    <div class="control-group" style="font-weight: bold">

                                                        <div class="controls">
                                                            <div class="input-append">
                                                                <div class="span3"></div>
                                                                <asp:Button ID="cmdSubmit" runat="server" CssClass="btn btn-primary"
                                                                    OnClientClick="javascript:return ValidateMyForm()"
                                                                    TabIndex="10" Text="SUBMIT" />


                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="space20" align="center">

                                                    <div class="form-horizontal" align="center">

                                                        <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>



                                                    </div>

                                                    <div class="form-horizontal" align="center">
                                                        <asp:Table ID="Table1" runat="server" CssClass="table table-striped table-bordered table-advance table-hover">
                                                        </asp:Table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="space20"></div>
                                        <div class="space20"></div>




                                    </div>


                                </div>
                        </div>
                    </div>


                    <div>
                    </div>


                    <div class="space20"></div>

                    <div class="row-fluid">
                        <div class="span1"></div>






                        </asp:Panel>                         
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
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
                    <ul><li>
                   This Web Page Can Be Used To View All Mobile Register Details and To Approve Mobile Users</li>
                  <li>  To Approve Mobile User Click On Approval Button
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

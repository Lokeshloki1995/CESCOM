<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="RIApprove.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.RIApprove" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateMyForm() {
            if (document.getElementById('<%= txtCommentFromStoreKeeper.ClientID %>').value.trim() == "") {
                alert('Enter Remarks for RI Approval')
                document.getElementById('<%= txtCommentFromStoreKeeper.ClientID %>').focus()
                return false
            }

            //            if (document.getElementById('<%= txtAckNo.ClientID %>').value.trim() == "") {
            //                alert('Enter Valid RV Number')
            //                document.getElementById('<%= txtAckNo.ClientID %>').focus()
            //                return false
            //            }

            //            if (document.getElementById('<%= txtAckDate.ClientID %>').value.trim() == "") {
            //                alert('Enter Valid RV Date')
            //                document.getElementById('<%= txtAckDate.ClientID %>').focus()
            //                return false
            //            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                        RI Acknowledgement
                    </h3>
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
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="btn btn-primary"
                        OnClick="cmdClose_Click" /></div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4>
                                <i class="icon-reorder"></i>RI Acknowledgement</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtrCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <asp:LinkButton ID="lnkDTrDetails" runat="server" Style="font-size: 12px; color: Blue"
                                                        OnClick="lnkDTrDetails_Click">View DTr Details</asp:LinkButton>
                                                    <asp:HiddenField ID="hdfDecommDate" runat="server" />
                                                   
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Slno</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                    <asp:TextBox ID="txtDTrSlno" runat="server" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtDTrId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfDTCCode" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Make</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtMake" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="control-group" style="display:none">
                                            <label class="control-label">
                                                Failure Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtFailureDate" runat="server" ReadOnly="true" Visible="false" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Oil Quantity(In Liter) Entered By SO</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtOilQuantity" runat="server" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);"
                                                        ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="txtActiontype" runat="server" MaxLength="10" Visible="false" Width="20px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Ack Number
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfCrBy" runat="server" />
                                                        <asp:TextBox ID="txtAckNo" runat="server" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Work Order Number
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDeCommWO" runat="server" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                    </div>
                                    <div class="span5">
                                        <asp:Panel ID="pnlApproval" runat="server">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Oil Quantity(In Liter) <span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOilQtySK" runat="server" MaxLength="10" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">
                                                  Manual Ack Number
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtManualAckNo" runat="server" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Ack Date
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAckDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtAckDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Remarks<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                        <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:TextBox ID="txtDecommId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtFailureId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtType" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtCommentFromStoreKeeper" runat="server" TextMode="MultiLine" MaxLength="500"
                                                            Height="80px" Style="resize: none" onkeyup="javascript:ValidateTextlimit(this,250)"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="space20">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue" >
                            <div class="widget-title" >
                                <h4>
                                    <i class="icon-reorder"></i>Comments for Approve/Reject</h4>
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
                                                        Comments<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="4" TextMode="MultiLine"
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
                <div class="text-center" align="center">
                   
                        <asp:Button ID="cmdApprove" runat="server" Text="Approve" CssClass="btn btn-primary"
                            OnClick="cmdApprove_Click" OnClientClick="javascript:return ValidateMyForm()" />
                 
                        <asp:Button ID="cmdViewDecomm" runat="server" Text="View Decommission" CssClass="btn btn-success"
                            OnClick="cmdViewDecomm_Click" TabIndex="13" />
                   
                    <div class="space20">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="span7">
    </div>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>

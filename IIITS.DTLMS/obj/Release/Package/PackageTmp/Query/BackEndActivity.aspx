<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="BackEndActivity.aspx.cs" Inherits="IIITS.DTLMS.Query.BackEndActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidateMyForm() {

            if (document.getElementById('<%= cmbCircle.ClientID %>').value == "--Select--") {
                alert('Select Circle')
                document.getElementById('<%= cmbCircle.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbDivision.ClientID %>').value.trim() == "--Select--") {
                alert('Select Division')
                document.getElementById('<%= cmbDivision.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbsubdivision.ClientID %>').value.trim() == "--Select--") {
                alert('Select Sub Division')
                document.getElementById('<%= cmbsubdivision.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbSection.ClientID %>').value.trim() == "--Select--") {
                alert('Select Section')
                document.getElementById('<%= cmbSection.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbFeeder.ClientID %>').value.trim() == "--Select--") {
                alert('Select Feeder')
                document.getElementById('<%= cmbFeeder.ClientID %>').focus()
                return false
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div id ="temp" runat ="server" visible ="true">
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                    Back End Activity
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
                <%--   <asp:Button ID="cmdClose" runat="server" Text="Close" 
                                    CssClass="btn btn-primary" />--%></div>
            <div style="float: right; margin-top: 20px; margin-right: 12px">
            </div>
        </div>
            </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div id ="Div4" runat ="server" visible ="false">
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Back End Activity</h4>
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
                                                Circle Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:HiddenField ID="hdfStarRate" runat="server" />
                                                    <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true" TabIndex="1"
                                                        OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtStatus" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Division Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" TabIndex="2"
                                                        OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Sub Division Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbsubdivision" runat="server" AutoPostBack="true" TabIndex="3"
                                                        OnSelectedIndexChanged="cmbsubdivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtEnumDetailsId" runat="server" MaxLength="50" Visible="false"
                                                        Width="50px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Section Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSection" runat="server" TabIndex="4">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                Feeder Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbFeeder" runat="server" TabIndex="5" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbFeeder_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="6"></asp:TextBox>
                                                    <asp:Button ID="cmdSearch" Text="S" class="btn btn-primary" runat="server" OnClick="cmdSearch_Click" />
                                                    <asp:Button ID="btnAdd" Text="add" class="btn btn-primary" runat="server" OnClick="btnAdd_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:ListBox ID="ListDtcCode" runat="server" Visible="false"></asp:ListBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="space20">
                                    </div>
                                    <asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                                <div class="space20">
                                </div>
                                <div class="form-horizontal" align="center">
                                    <div class="span3">
                                    </div>
                                    <div class="span1">
                                        <asp:Button ID="cmdDelete" runat="server" Text="DELETE" OnClientClick="javascript:return ValidateMyForm()"
                                            CssClass="btn btn-primary" OnClick="cmdDelete_Click" />
                                    </div>
                                    <%-- <div class="span1"></div>--%>
                                    <div class="span1">
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-primary"
                                            OnClick="cmdReset_Click" /><br />
                                    </div>
                                    <div class="span7">
                                    </div>
                                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
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
        </div>
        </div>

        <!-- END PAGE CONTENT-->
        <!-- BEGIN PAGE CONTENT-->
        
        <div class="row-fluid">
            <div id ="Div5" runat ="server" visible ="false">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Image Deletion</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="span1">
                                </div>
                                <div class="row-fluid">
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtImageDtcCode" runat="server" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Image Type<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbDeletePhotos" runat="server" TabIndex="5" AutoPostBack="true">
                                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="SS Plate Photo" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Name Plate Photo" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="DTC Code(DTLMS) Photo" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="DTC Code(Ip Enum) Photo" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="Old DTC Code(CESC) Photo" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="Infosys Asset Id Photo" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="DTC Photo" Value="7"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtColumnName" runat="server" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space20">
                        </div>
                        <div class="form-horizontal" align="center">
                            <div class="span3">
                            </div>
                            <div class="span1">
                                <asp:Button ID="cmdDeleteImage" runat="server" Text="Image Delete" TabIndex="7" OnClientClick="javascript:return AlertTCCount()"
                                    CssClass="btn btn-primary" OnClick="cmdDeleteImage_Click" />
                            </div>
                            <div class="space20">
                            </div>
                        </div>
                        <!-- END FORM-->
                    </div>
                
         </div>
      </div>
                </div>
         <div id ="Div8" runat ="server" visible ="false">
                <!-- END SAMPLE FORM PORTLET-->
                <div class="row-fluid" runat="server" id="dvComments" style="display: none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
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
                                                            <asp:TextBox ID="txtComment" runat="server" MaxLength="200" TabIndex="8" TextMode="MultiLine"
                                                                Width="550px" Height="125px" style="resize:none"  onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
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
            </div>
        
    </div>
        <!-- END PAGE CONTENT-->
        
        
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DELETE FAILURE/ENHANCEMENT RECORD</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="span1">
                                </div>
                                <div class="row-fluid">
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtCode" runat="server" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space20">
                        </div>
                        <div class="form-horizontal" align="center">
                            <div class="span3">
                            </div>
                            <div class="span1">
                                <asp:Button ID="btnLoad" runat="server" Text="Load" TabIndex="7" 
                                    CssClass="btn btn-primary" onclick="btnLoad_Click"/>
                            </div>
                            <div class="space20">
                            </div>
                        </div>
                        <!-- END FORM-->
                        <asp:HiddenField ID="FailureID" runat="server" />
                        <asp:GridView ID="GrdFailDetail" runat="server" AutoGenerateColumns="False" 
                                            CssClass="table table-striped table-bordered table-advance table-hover" 
                                           PageSize="10" AllowPaging="true" 
                            onpageindexchanging="GrdFailDetail_PageIndexChanging" 
                            onrowcommand="GrdFailDetail_RowCommand" 
                            onrowdeleting="GrdFailDetail_RowDeleting" 
                            onrowdatabound="GrdFailDetail_RowDataBound" >
                                            <Columns>
                                            <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="FAIL ID" Visible="false"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDfId" runat="server" Text='<%# Bind("DF_ID") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DF_DTC_CODE" HeaderText="DTC CODE" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DF_DTC_CODE") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="DF_REPLACE_FLAG" HeaderText="REPLACE FLAG" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReplaceFlag" runat="server" Text='<%# Bind("DF_REPLACE_FLAG") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="DF_EQUIPMENT_ID" HeaderText="TC CODE" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTccode" runat="server" Text='<%# Bind("DF_EQUIPMENT_ID") %>' Style="word-break: break-all;"
                                                            Width="60px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="FAILURE_TYPE" HeaderText="FAILURE TYPE"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFailureType" runat="server" Text='<%# Bind("FAILURE_TYPE") %>'
                                                             Width="120px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="ACTION" Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <center>
                                                         <asp:LinkButton runat="server"  CommandName="Delete" ID="lnkView" ToolTip="View">
                                                         <img src="../img/Manual/view.png" style="width:20px" /></asp:LinkButton> 
                                                        </center>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
                <div class="row-fluid" runat="server" id="Div3" style="display: none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
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
                                                            <asp:TextBox ID="TextBox5" runat="server" MaxLength="200" TabIndex="8" TextMode="MultiLine"
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
            </div>
        </div>
        <!-- END PAGE CONTENT-->

        <!-- BEGIN PAGE CONTENT-->
    <div id ="Div6" runat ="server" visible ="false">
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Duplicate DTR and DTC</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="span1">
                                </div>
                                <div class="row-fluid">
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code
                                            </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDuplicateDtcCode" runat="server" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTr Code</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDTRCode" runat="server" MaxLength="10" TabIndex="12"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space20">
                        </div>
                        <div class="space20">
                        </div>
                        <div class="form-horizontal" align="center">
                            <div class="span3">
                            </div>
                            <div class="span1">
                                <asp:Button ID="cmdLoad" runat="server" Text="Load" TabIndex="7" CssClass="btn btn-primary"
                                    OnClick="cmdLoad_Click" />
                            </div>
                            <div class="space20">
                            </div>
                        </div>
                        <!-- END FORM-->
                        <asp:GridView ID="grdTcDetails" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="true"
                            EmptyDataText="No records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                            AllowPaging="true" runat="server" TabIndex="16" OnRowCommand="grdTcDetails_RowCommand">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="DTE_ED_ID" HeaderText="DTE_ED_ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEdId" runat="server" Text='<%# Bind("DTE_ED_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="DTE_DTCCODE" HeaderText="DTC CODE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDTEDTCCODE" runat="server" Text='<%# Bind("DTE_DTCCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="DTE_TC_CODE" HeaderText="DTr CODE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDTETCCODE" runat="server" Text='<%# Bind("DTE_TC_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="DTE_TC_SLNO" HeaderText="DTr SL NO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDTETCSLNO" runat="server" Text='<%# Bind("DTE_TC_SLNO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="ED_OFFICECODE" HeaderText="OFFICE CODE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEDOFFICECODE" runat="server" Text='<%# Bind("ED_OFFICECODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="OFFICEnAME" HeaderText="OFFICE NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOFFICENAME" runat="server" Text='<%# Bind("OFFICEnAME") %>'></asp:Label>
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
                </div>
                <!-- END SAMPLE FORM PORTLET-->
                <div class="row-fluid" runat="server" id="Div1" style="display: none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
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
                                                            <asp:TextBox ID="TextBox3" runat="server" MaxLength="200" TabIndex="8" TextMode="MultiLine"
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
            </div>
        </div>
        </div>
        <!-- END PAGE CONTENT-->
        <div id ="Div7" runat ="server" visible ="false">
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Section Swapping</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="span1">
                                </div>
                                <div class="row-fluid">
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">

                                                    <asp:DropDownList ID="cmbSecondDevision" runat="server" TabIndex="1"
                                                    AutoPostBack="true" onselectedindexchanged="cmbSecondDevision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">SubDivision<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSecondSubDevision" runat="server" TabIndex="1"
                                                    AutoPostBack="true" onselectedindexchanged="cmbSecondSubDevision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                     </div>

                                     <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">Section Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbSecondSection" runat="server" TabIndex="1"
                                                    AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Feeder Name<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbsecondFeeder" runat="server" TabIndex="1"
                                                    AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                     </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space20">
                        </div>
                        <div class="form-horizontal" align="center">
                            <div class="span3">
                            </div>
                            <div class="span1">
                                <asp:Button ID="btnMove" runat="server" Text="Move" TabIndex="7" CssClass="btn btn-primary"
                                    OnClick="btnMove_Click" />
                            </div>
                            <div class="space20">
                            </div>
                        </div>
                        <!-- END FORM-->
                    </div>skype
                </div>
                <!-- END SAMPLE FORM PORTLET-->
                <div class="row-fluid" runat="server" id="Div2" style="display: none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
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
                                                            <asp:TextBox ID="TextBox4" runat="server" MaxLength="200" TabIndex="8" TextMode="MultiLine"
                                                                Width="550px" Height="125px" style="resize:none"  onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>
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
            </div>
        </div>
        </div>
    
    
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="ApprovalPending.aspx.cs" Inherits="IIITS.DTLMS.Approval.ApprovalPending" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <script type="text/javascript">

    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip(); 
    });

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
    //Charactes and space() - . / 0 1 2 3 4 7  to search DTC Name
    function characterAndspecialDtc(event) {
        var evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        if ((charCode < 65 || charCode > 90) &&
         (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 40 && charCode != 41 && charCode != 45 && charCode != 46 && charCode != 47 && (charCode < 48 || charCode > 55)) {

            return false;
        }
        return true;
    }
    //Remove Numbers, Special characters except space to search DTC Name
    function cleanSpecialAndNumDtc(t) {

        t.value = t.value.toString().replace(/[^-./()a-zA-Z0-7\t\n\r]+/g, '');


    }
        </script>

    <style type="text/css">
        .Normal
        {
            background-color: Lime;
            vertical-align: middle;
            text-align: center;
            position: absolute;
        }
        .Warning
        {
            font-weight: bold;
            text-align: center;
        }
    </style>
     <style type="text/css">
         .ascending th a {
        background:url(/img/sort_asc.png) no-repeat;
        display: block;
        padding: 0px 4px 0 20px;
    }

    .descending th a {
        background:url(/img/sort_desc.png) no-repeat;
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
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                        Failure Pending Details
                    </h3>
                    <ul class="breadcrumb" style="display: none;">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="Text1" type="text">
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
            <div class="row-fluid" >
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4>
                                <i class="icon-reorder"></i>Approval Pending Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <%-- <div class="span1"></div>--%>
                                        <div class="span5">
                                          <div class="control-group">
                                            <label class="control-label"> Division</label>                                            
                                           <div class="controls">
                                                <div class="input-append">                                                       
                                                     <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true"
                                                             TabIndex="1" onselectedindexchanged="cmbDiv_SelectedIndexChanged"> </asp:DropDownList>   
                                                </div>
                                              
                                            </div>
                                            </div>

                                             <div class="control-group">
                                            <label class="control-label">Sub Division</label>
                                             
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                          <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true"
                                                             TabIndex="1" onselectedindexchanged="cmbSubDiv_SelectedIndexChanged" > </asp:DropDownList>   
                                                    </div>
                                                   
                                                </div>
                                        </div>  
                                        </div>
                                        <div class="span5">
                                          <div class="control-group">
                                            <label class="control-label">Section </label>
                                             
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                          <asp:DropDownList ID="cmbSection" runat="server" AutoPostBack="true"
                                                             TabIndex="1" > </asp:DropDownList>   
                                                    </div>
                                                </div>
                                         </div>
                                          <div class="control-group">
                                            <label class="control-label">Type</label>                                            
                                           <div class="controls">
                                                <div class="input-append">                                                       
                                                     <asp:DropDownList ID="cmbType" runat="server"  TabIndex="1" > 
                                                     </asp:DropDownList>   
                                                </div>
                                            </div>
                                         </div>   

                                            <div class="span5">
                                                <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary" Width="116px"
                                                    OnClick="cmdLoad_Click" />
                                            </div>
                                            <%--  <div class="span5">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickApprovalPending" /><br />
                                          </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="widget-body">
                           <div style="float: right">
                            </div>
                            <!-- END FORM-->
                            <div style="float: right">
                                <%--<div class="span2">
                                    <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                                        OnClick="cmbExport_Click" Visible="false" /> <br />
                                </div>--%>
                                <asp:HiddenField ID="hdfOffCode" runat="server" />
                            </div>
                            <div class="space20">
                            </div>
                            <div style=" overflow: auto;">
                                <asp:GridView ID="grdApprovePending" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" OnPageIndexChanging="grdApprovePending_PageIndexChanging"
                                    OnRowCreated="grdApprovePending_RowCreated" 
                                    OnRowCommand="grdApprovePending_RowCommand" 
                                    onrowdatabound="grdApprovePending_RowDataBound"  OnSorting="grdApprovePending_Sorting" AllowSorting="true">
                               <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' Width="80px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="80px"
                                                        MaxLength="6" onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="DTC Name" Visible="true" SortExpression="DT_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcName" runat="server" Text='<%# Bind("DT_NAME") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name " Width="150px"
                                                  onkeypress="return characterAndspecialDtc(event)"  
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="OMSECTION" HeaderText="Section Name" Visible="true" SortExpression="OMSECTION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOmSectionName" runat="server" Text='<%# Bind("OMSECTION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                                 
                                        <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Status" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;"
                                                    Width="210px" ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                  
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                   </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

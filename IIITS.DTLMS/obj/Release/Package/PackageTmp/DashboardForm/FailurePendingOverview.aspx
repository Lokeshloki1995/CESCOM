<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FailurePendingOverview.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.FailurePendingOverview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
   <meta content="width=device-width, initial-scale=1.0" name="viewport" />
   <meta content="" name="description" />
   <meta content="Mosaddek" name="author" />
   <link href="/assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
   <link href="/assets/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" />
   <link href="/assets/bootstrap/css/bootstrap-fileupload.css" rel="stylesheet" />
   <link href="/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
   <link href="/css/style.css" rel="stylesheet" />
   <link href="/css/style-responsive.css" rel="stylesheet" />
   <link href="/css/style-default.css" rel="stylesheet" id="style_color" />
   <link href="/assets/fullcalendar/fullcalendar/bootstrap-fullcalendar.css" rel="stylesheet" />
   <link href="/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="/Styles/calendar.css" rel="stylesheet" type="text/css" />
   <script type="text/javascript" src="Scripts/functions.js"></script>
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
    <script type="text/javascript">
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
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                        <asp:Label ID="failure" runat="server" Text="Failure Pending Details" ForeColor="White"></asp:Label>
                    </h3>
                     <div style="float:right" >
                                
                             <div class="span1">
                                        
                                          </div>

                                            </div>
                    <%--<ul class="breadcrumb" style="display: none;">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <button class="btn" type="button">
                                    <i class="icon-search"></i>
                                </button>
                            </div>
                            </form>
                        </li>
                    </ul>--%>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
            </div>
            <div class="row-fluid" 
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i><asp:Label ID="failureText" runat="server" Text="Failure Pending Details"></asp:Label></h4>
<%--                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>--%>
                        </div>
                        <div class="widget-body">
                            <div style="float: right">
                            </div>
                            <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickFailurePendingOverview" /><br />

                            <!-- END FORM-->
                            <div style="float: right">
                                <asp:HiddenField ID="hdfOffCode" runat="server" />
                            </div>
                            <div class="space20">
                            </div>
                            <div style=" overflow:auto;">
                                <asp:GridView ID="grdFailurePending" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" OnPageIndexChanging="grdFailurePending_PageIndexChanging"
                                     OnRowCommand="grdFailurePending_RowCommand" Visible="false" OnSorting="grdFailurePending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCogrdEstimationPendingde" runat="server" Text='<%# Bind("DT_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="150px"
                                                       MaxLength="6" onkeypress="return characterAndnumbers(event)" onpaste="return false" onchange = "return cleanSpecialChars(this)"></asp:TextBox>
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
                                                        onkeypress="return characterAndspecialDtc(event)"  onpaste="return false" 
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION Name" SortExpression="SUBDIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDIVISIONName" runat="server" Text='<%# Bind("DIV") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                                <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField AccessibleHeaderText="SUBDIVSION" HeaderText="Sub-Division Name" SortExpression="SUBDIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVSION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField AccessibleHeaderText="OM_CODE" HeaderText="Section Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOmCode" runat="server" Text='<%# Bind("OMSECTION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure No" SortExpression="DF_ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailureNo" runat="server" Text='<%# Bind("DF_ID") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="DF_DATE" HeaderText="Failure Date" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailureDate" runat="server" Text='<%# Bind("DF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="WorkOrder" Visible="true" SortExpression="WO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWoNo" runat="server" Text='<%# Bind("WO_NO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                          
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WorkOrder Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWoDate" runat="server" Text='<%# Bind("WO_DATE") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      
                                       
                                         <asp:TemplateField AccessibleHeaderText="GUARANTY_TYPE" HeaderText="GUARANTY_TYPE" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("GUARANTY_TYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                         </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="FAIL_CAPACITY" HeaderText="Failure Capacity" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailCapacity" runat="server" Text='<%# Bind("FAIL_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                         </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="INV_CAPACITY" HeaderText="Invoice Capacity" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblnvoiceCapacity" runat="server" Text='<%# Bind("INV_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                         </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="grdEstimationPending" AutoGenerateColumns="false" PageSize="10"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" 
                                    ShowFooter="true" Visible="false"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server" OnRowCommand="grdEstimationPending_RowCommand" 
                                    onpageindexchanging="grdEstimationPending_PageIndexChanging" OnSorting="grdEstimationPending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                    <%--<HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                    <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="150px"
                                                     MaxLength="6" onpaste="return false" onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)"></asp:TextBox>
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
                                                         onkeypress="return characterAndspecialDtc(event)"  onpaste="return false"
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="Division Name" SortExpression="DIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="SUBDIVSION" HeaderText="Sub-Division Name" SortExpression="SUBDIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVSION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
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
<%--                                        <asp:TemplateField AccessibleHeaderText="EST_NO" HeaderText="Estimation No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimationNo" runat="server" Text='<%# Bind("EST_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField AccessibleHeaderText="DTRCODE" HeaderText="DTR CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDTRCode" runat="server" Text='<%# Bind("DTRCODE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="EST_CRON" HeaderText="Estimation Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimationDate" runat="server" Text='<%# Bind("EST_CRON") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

										
										<asp:TemplateField AccessibleHeaderText="FAILURECAPACITY" HeaderText="FAILURE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAILURECAPACITY" runat="server" Text='<%# Bind("FAILURECAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="INVOICECAPACITY" HeaderText="INVOICE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblINVOICECAPACITY" runat="server" Text='<%# Bind("INVOICECAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										
										<asp:TemplateField AccessibleHeaderText="FAILURETYPE" HeaderText="FAILURE TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAILURETYPE" runat="server" Text='<%# Bind("FAILURETYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="GUARANTY_TYPE" HeaderText="GUARANTY_TYPE" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("GUARANTY_TYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                         </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="FL_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailStatus" runat="server" Text='<%# Bind("FL_STATUS") %>' Style="word-break: break-all;"
                                                    Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="grdWorkorderPending" AutoGenerateColumns="false" PageSize="10"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" 
                                    ShowFooter="true" Visible="false"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server" OnRowCommand="grdWorkorderPending_RowCommand" 
                                    onpageindexchanging="grdWorkorderPending_PageIndexChanging" OnSorting="grdWorkorderPending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                    <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="150px" onpaste="return false"
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
                                                    <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name " Width="150px" onpaste="return false"
                                                        onkeypress="return characterAndspecialDtc(event)"  
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="Division Name" SortExpression="DIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="SUBDIVSION" HeaderText="Sub-Division Name" SortExpression="SUBDIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVSION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
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
                                        <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="Work Order NO" SortExpression="WO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONO" runat="server" Text='<%# Bind("WO_NO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODATE" runat="server" Text='<%# Bind("WO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <%--<asp:TemplateField AccessibleHeaderText="WO_NEW_CAP" HeaderText="Commissioning Capacity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWoCapacity" runat="server" Text='<%# Bind("WO_NEW_CAP") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>


                                        <asp:TemplateField AccessibleHeaderText="DECOMCAPACITY" HeaderText="FAILURE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDECOMCAPACITY" runat="server" Text='<%# Bind("DECOMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="COMMCAPACITY" HeaderText="INVOICE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCOMMCAPACITY" runat="server" Text='<%# Bind("COMMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										
										<asp:TemplateField AccessibleHeaderText="FAILURETYPE" HeaderText="FAILURE TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAILURETYPE" runat="server" Text='<%# Bind("FAILURETYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="GUARANTY_TYPE" HeaderText="GUARANTY_TYPE" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("GUARANTY_TYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                         </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WO_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOStatus" runat="server" Text='<%# Bind("WO_STATUS") %>' Style="word-break: break-all;"
                                                    Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>


                                <asp:GridView ID="grdIndentPending" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" Visible="false"
                                    OnRowCommand="grdIndentPending_RowCommand" 
                                    onpageindexchanging="grdIndentPending_PageIndexChanging" OnSorting="grdIndentPending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                    <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="150px" onpaste="return false"
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
                                                    <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name " Width="150px" onpaste="return false"
                                                         onkeypress="return characterAndspecialDtc(event)"  
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="Division Name" SortExpression="DIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="SUBDIVSION" HeaderText="Sub-Division Name" SortExpression="SUBDIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVSION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
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

                                        <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="Work Order NO" SortExpression="WO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONO" runat="server" Text='<%# Bind("WO_NO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_NO_DECOM" HeaderText="DECOMMISSIONING WORK ORDER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWO_NO_DECOMY" runat="server" Text='<%# Bind("WO_NO_DECOM") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODATE" runat="server" Text='<%# Bind("WO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField AccessibleHeaderText="WO_NEW_CAP" HeaderText="Commissioning Capacity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWoCapacity" runat="server" Text='<%# Bind("WO_NEW_CAP") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       
<%--                                        <asp:TemplateField AccessibleHeaderText="TI_INDENT_NO" HeaderText="Indent No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentNo" runat="server" Text='<%# Bind("TI_INDENT_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField AccessibleHeaderText="TI_INDENT_DATE" HeaderText="Indent Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentDATE" runat="server" Text='<%# Bind("TI_INDENT_DATE") %>'
                                                    Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        
										
										<asp:TemplateField AccessibleHeaderText="DECOMCAPACITY" HeaderText="FAILURE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDECOMCAPACITY" runat="server" Text='<%# Bind("DECOMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="COMMCAPACITY" HeaderText="INVOICE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCOMMCAPACITY" runat="server" Text='<%# Bind("COMMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										
										<asp:TemplateField AccessibleHeaderText="FAILURETYPE" HeaderText="FAILURE TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAILURETYPE" runat="server" Text='<%# Bind("FAILURETYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="GUARANTY_TYPE" HeaderText="GUARANTY_TYPE" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("GUARANTY_TYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                         </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="INDT_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentStatus" runat="server" Text='<%# Bind("INDT_STATUS") %>'
                                                    Style="word-break: break-all;" Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="grdinvoicePending" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" Visible="false"
                                    OnRowCommand="grdinvoicePending_RowCommand" 
                                    onpageindexchanging="grdinvoicePending_PageIndexChanging" OnSorting="grdinvoicePending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                  <%--  <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                    <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="150px"
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
                                                         onkeypress="return characterAndspecialDtc(event)"  onpaste="return false" 
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="Division Name" SortExpression="DIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="SUBDIVSION" HeaderText="Sub-Division Name" SortExpression="SUBDIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVSION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
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
                                        
                                         <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="Work Order NO" SortExpression="WO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONO" runat="server" Text='<%# Bind("WO_NO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="WO_NO_DECOM" HeaderText="DECOMMISSIONING WORK ORDER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWO_NO_DECOMY" runat="server" Text='<%# Bind("WO_NO_DECOM") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODATE" runat="server" Text='<%# Bind("WO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField AccessibleHeaderText="WO_NEW_CAP" HeaderText="Commissioning Capacity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWoCapacity" runat="server" Text='<%# Bind("WO_NEW_CAP") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        
<%--                                        <asp:TemplateField AccessibleHeaderText="IN_INV_NO" HeaderText="Invoice No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("IN_INV_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        

                                       
										
										<asp:TemplateField AccessibleHeaderText="DECOMCAPACITY" HeaderText="FAILURE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDECOMCAPACITY" runat="server" Text='<%# Bind("DECOMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="COMMCAPACITY" HeaderText="INVOICE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCOMMCAPACITY" runat="server" Text='<%# Bind("COMMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
                                        <asp:TemplateField AccessibleHeaderText="IN_DATE" HeaderText="Commission Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDATE" runat="server" Text='<%# Bind("IN_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="FAILURETYPE" HeaderText="FAILURE TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAILURETYPE" runat="server" Text='<%# Bind("FAILURETYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="GUARANTY_TYPE" HeaderText="GUARANTY_TYPE" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("GUARANTY_TYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                         </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="INV_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvStatus" runat="server" Text='<%# Bind("INV_STATUS") %>' Style="word-break: break-all;"
                                                    Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="grdDecommissionPending" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" Visible="false"
                                    OnRowCommand="grdDecommissionPending_RowCommand" 
                                    onpageindexchanging="grdDecommissionPending_PageIndexChanging" OnSorting="grdDecommissionPending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                    <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="150px" onpaste="return false"
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
                                                    <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name " Width="150px" onpaste="return false"
                                                         onkeypress="return characterAndspecialDtc(event)"  
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DTR_CODE" HeaderText="DTR Code" Visible="true" SortExpression="DTR_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldtrCode" runat="server" Text='<%# Bind("DTR_CODE") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                          <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="Division Name" SortExpression="DIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="SUBDIVSION" HeaderText="Sub-Division Name" SortExpression="SUBDIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVSION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
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


                                          <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="Work Order NO" SortExpression="WO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONO" runat="server" Text='<%# Bind("WO_NO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="WO_NO_DECOM" HeaderText="DECOMMISSIONING WORK ORDER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWO_NO_DECOMY" runat="server" Text='<%# Bind("WO_NO_DECOM") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODATE" runat="server" Text='<%# Bind("WO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="DECOMCAPACITY" HeaderText="FAILURE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDECOMCAPACITY" runat="server" Text='<%# Bind("DECOMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="COMMCAPACITY" HeaderText="INVOICE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCOMMCAPACITY" runat="server" Text='<%# Bind("COMMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										
										<asp:TemplateField AccessibleHeaderText="FAILURETYPE" HeaderText="FAILURE TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAILURETYPE" runat="server" Text='<%# Bind("FAILURETYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="GUARANTY_TYPE" HeaderText="GUARANTY_TYPE" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("GUARANTY_TYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                         </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="DECOMM_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDecomStatus" runat="server" Text='<%# Bind("DECOMM_STATUS") %>' Style="word-break: break-all;"
                                                    Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="grdRIPending" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" Visible="false"
                                    OnRowCommand="grdRIPending_RowCommand" 
                                    onpageindexchanging="grdRIPending_PageIndexChanging" OnSorting="grdRIPending_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                    <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="150px" onpaste="return false"
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
                                                         onkeypress="return characterAndspecialDtc(event)"  onpaste="return false"
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="DTR_CODE" HeaderText="DTR Code" Visible="true" SortExpression="DTR_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldtrCode" runat="server" Text='<%# Bind("DTR_CODE") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="Division Name" SortExpression="DIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                         <asp:TemplateField AccessibleHeaderText="SUBDIVSION" HeaderText="Sub-Division Name" SortExpression="SUBDIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVSION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
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
<%--                                        <asp:TemplateField AccessibleHeaderText="TR_RI_NO" HeaderText="RI No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblriNo" runat="server" Text='<%# Bind("TR_RI_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="Work Order NO" SortExpression="WO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONO" runat="server" Text='<%# Bind("WO_NO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="WO_NO_DECOM" HeaderText="DECOMMISSIONING WORK ORDER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWO_NO_DECOMY" runat="server" Text='<%# Bind("WO_NO_DECOM") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODATE" runat="server" Text='<%# Bind("WO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="DECOMCAPACITY" HeaderText="FAILURE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDECOMCAPACITY" runat="server" Text='<%# Bind("DECOMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="COMMCAPACITY" HeaderText="INVOICE CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCOMMCAPACITY" runat="server" Text='<%# Bind("COMMCAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
                                         <asp:TemplateField AccessibleHeaderText="TR_RI_DATE" HeaderText="RI Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRiDATE" runat="server" Text='<%# Bind("TR_RI_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
										<asp:TemplateField AccessibleHeaderText="FAILURETYPE" HeaderText="FAILURE TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAILURETYPE" runat="server" Text='<%# Bind("FAILURETYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="GUARANTY_TYPE" HeaderText="GUARANTY_TYPE" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("GUARANTY_TYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>                                            
                                         </asp:TemplateField>

                                       
                                        <asp:TemplateField AccessibleHeaderText="RI_STATUS" HeaderText="RI STATUS">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRIStatus" runat="server" Text='<%# Bind("RI_STATUS") %>' Style="word-break: break-all;"
                                                    Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="CR_STATUS" HeaderText="CR STATUS">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRIStatus" runat="server" Text='<%# Bind("CR_STATUS") %>' Style="word-break: break-all;"
                                                    Width="200px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    
                                
                                        
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="grdInvoiceTCDetails" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" Visible="false" AllowSorting="true" OnPageIndexChanging="grdInvoiceTCDetails_PageIndexChanging" OnRowCommand="grdInvoiceTCDetails_RowCommand">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                    <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltccode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="TxtTcCode" runat="server" placeholder="Enter DTR Code " Width="150px" onpaste="return false"
                                                        MaxLength="6"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SERIAL NO" Visible="true" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblSLNO" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtTCSLNO" runat="server" placeholder="Enter DTR SLNO " Width="150px" onpaste="return false"
                                                        MaxLength="6"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="CAPACITY" Visible="true" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="STATUS" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
        </Columns>
        </asp:GridView>

                            </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

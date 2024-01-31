<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="TcFailed.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.TcFailed" %>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FailurePendingOverview.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.FailurePendingOverview" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
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
    <title></title>
     <script type="text/javascript">
     //DTR allow to enter nums for 6 digits
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

// DTR allow only Numbers to paste
        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }
           </script>
    
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
                        <asp:Label ID="failure" runat="server" Text="Tc Failure Details" ForeColor="White"></asp:Label>
                    </h3>
                      <div style="float:right" >
                                
                             <div class="span1">
                                       
                                          </div>

                                            </div>
                   
                </div>
            </div>
             
            <div class="row-fluid" >
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i><asp:Label ID="failureText" runat="server" Text="Tc Failure Details"></asp:Label></h4>
                            <%--<span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>--%>
                        </div>
                        <div class="widget-body">
                            <div style="float: right">
                            </div>

                             <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickTcFailure" /><br />
                            <!-- END FORM-->
                            <div style="float: right">
                                <asp:HiddenField ID="hdfOffCode" runat="server" />
                            </div>
                            <div class="space20">
                            </div>
                           
                            <div style=" overflow:auto;">
                                
                                <%--//total tc failed--%>
                                <asp:GridView ID="grdFailuretc" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" 
                                      OnRowCommand="grdFailuretc_RowCommand" 
                                    onpageindexchanging="grdFailuretc_PageIndexChanging"
                                     Visible="false" OnSorting="grdFailuretc_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>

                                         <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR CODE" Visible="true" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttCode" runat="server" placeholder="Enter TC Code " Width="150px" onpaste = "return false"
                                                         MaxLength="6" onkeypress="return onlyNumbers(event)" onchange = "return cleanSpecialAndChar(this)" ></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTR Capacity" Visible="true" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch"> 
                                                    <asp:TextBox ID="txttcCapacity" runat="server" placeholder="Enter TC CAPACITY " Width="150px" onpaste = "return false"
                                                       MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)" ></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SLNO" Visible="true" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>

                                       

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="DTR MANUFACTURE_DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCMANFDATE" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                    
                                    </Columns>
                                </asp:GridView>



                                  <asp:GridView ID="grdtotaldtr" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" 
                                      OnRowCommand="grdtotaldtr_RowCommand" 
                                    onpageindexchanging="grdtotaldtr_PageIndexChanging"
                                     Visible="false" OnSorting="grdtotaldtr_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>

                                         <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR CODE" Visible="true" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttCode" runat="server" placeholder="Enter TC Code " Width="150px" onpaste = "return false"
                                                        MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTR Capacity" Visible="true" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttcCapacity" runat="server" placeholder="Enter TC CAPACITY " Width="150px" onpaste = "return false"
                                                         MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SLNO" Visible="true" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>

                                        

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="DTR MANUFACTURE_DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCMANFDATE" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                    
                                    </Columns>
                                </asp:GridView>


                                   <asp:GridView ID="grdfaultyfield" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" 
                                      OnRowCommand="grdfaultyfield_RowCommand" 
                                    onpageindexchanging="grdfaultyfield_PageIndexChanging"
                                     Visible="false" OnSorting="grdfaultyfield_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>

                                        <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="Division" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="SUBDIVISION" HeaderText="Subdivision" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubdivision" runat="server" Text='<%# Bind("SUBDIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="SECTION" HeaderText="Section" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSection" runat="server" Text='<%# Bind("SECTION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code" Visible="true" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttCode" runat="server" placeholder="Enter TC Code " Width="150px" onpaste = "return false"
                                                         MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTr Capacity" Visible="true" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttcCapacity" runat="server" placeholder="Enter TC CAPACITY " Width="150px" onpaste = "return false"
                                                          MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr Slno" Visible="true" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>


                                         <asp:TemplateField AccessibleHeaderText="DF_GUARANTY_TYPE" HeaderText="Guaranty Type" Visible="true" SortExpression="DF_GUARANTY_TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("DF_GUARANTY_TYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>

                                         

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="DTr Manufacture Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCMANFDATE" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>





                                        
                                      <asp:TemplateField AccessibleHeaderText="IN_INV_NO" HeaderText="Invoice No" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbInvoiceNo" runat="server" Text='<%# Bind("IN_MANUAL_INVNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                      <asp:TemplateField AccessibleHeaderText="IN_DATE" HeaderText="Invoice Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Bind("IN_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>



                                      <asp:TemplateField AccessibleHeaderText="TR_RI_DATE" HeaderText="Decommissioning Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRIDate" runat="server" Text='<%# Bind("TR_RI_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                    
                                    </Columns>
                                </asp:GridView>


                                   <asp:GridView ID="grdfaultystore" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" 
                                      OnRowCommand="grdfaultystore_RowCommand" 
                                    onpageindexchanging="grdfaultystore_PageIndexChanging"
                                     Visible="false" OnSorting="grdfaultystore_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>

                                        <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR CODE" Visible="true" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttCode" runat="server" placeholder="Enter TC Code " Width="150px" onpaste = "return false"
                                                         MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTR CAPACITY" Visible="true" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttcCapacity" runat="server" placeholder="Enter TC CAPACITY " Width="150px" onpaste = "return false"
                                                        MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SLNO" Visible="true" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>

                                          <asp:TemplateField AccessibleHeaderText="DF_GUARANTY_TYPE" HeaderText="Guaranty Type" Visible="true" SortExpression="DF_GUARANTY_TYPE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuarantyType" runat="server" Text='<%# Bind("DF_GUARANTY_TYPE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>

                                     

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="DTR MANUFACTURE_DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCMANFDATE" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        
                                    
                                    </Columns>
                                </asp:GridView>


                                 <asp:GridView ID="grdBrandNewDtr" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" 
                                      OnRowCommand="grdBrandNewDtr_RowCommand" 
                                    onpageindexchanging="grdBrandNewDtr_PageIndexChanging"
                                     Visible="false" OnSorting="grdBrandNewDtr_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                         <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR CODE" Visible="true" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttCode" runat="server" placeholder="Enter TC Code " Width="150px" onpaste = "return false"
                                                        MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTR CAPACITY" Visible="true" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttcCapacity" runat="server" placeholder="Enter TC CAPACITY " Width="150px" onpaste = "return false"
                                                        MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SLNO" Visible="true" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>

                                         

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="DTR MANUFACTURE_DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCMANFDATE" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="PO_NO" HeaderText="PO Number" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoNumber" runat="server" Text='<%# Bind("PO_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="PO_DATE" HeaderText="PO Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("PO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                         <asp:TemplateField AccessibleHeaderText="VM_NAME" HeaderText="Vendor Name" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorNAme" runat="server" Text='<%# Bind("VM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       
                                    
                                    </Columns>
                                </asp:GridView>



                                 <asp:GridView ID="grdScrapdtr" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" 
                                      OnRowCommand="grdScrapdtr_RowCommand" 
                                    onpageindexchanging="grdScrapdtr_PageIndexChanging"
                                     Visible="false" OnSorting="grdScrapdtr_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>

                                        <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR CODE" Visible="true" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttCode" runat="server" placeholder="Enter TC Code " Width="150px" onpaste = "return false"
                                                          MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTR CAPACITY" Visible="true" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttcCapacity" runat="server" placeholder="Enter TC CAPACITY " Width="150px" onpaste = "return false"
                                                          MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SLNO" Visible="true" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>

                                        


                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="DTR MANUFACTURE_DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCMANFDATE" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        
                                    
                                    </Columns>
                                </asp:GridView>



                                 <asp:GridView ID="grdNonRepairable" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" 
                                      OnRowCommand="grdNonRepairabler_RowCommand" 
                                    onpageindexchanging="grdNonRepairable_PageIndexChanging"
                                     Visible="false" OnSorting="grdNonRepairable_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                   <%-- <HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>

                                         <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR CODE" Visible="true" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttCode" runat="server" placeholder="Enter TC Code " Width="150px" onpaste = "return false"
                                                         MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTR CAPACITY" Visible="true" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttcCapacity" runat="server" placeholder="Enter TC CAPACITY " Width="150px" onpaste = "return false"
                                                        MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SLNO" Visible="true" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="DTR MANUFACTURE_DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCMANFDATE" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       
                                    
                                    </Columns>
                                </asp:GridView>

                                

                                   <asp:GridView ID="grdfaultyrepairer" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" 
                                      OnRowCommand="grdfaultyrepairer_RowCommand" 
                                    onpageindexchanging="grdfaultyrepairer_PageIndexChanging"
                                     Visible="false" OnSorting="grdfaultyrepairer_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                    <%--<HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR CODE" Visible="true" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttCode" runat="server" placeholder="Enter TC Code " Width="150px" onpaste = "return false"
                                                         MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                          
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTR CAPACITY" Visible="true" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttcCapacity" runat="server" placeholder="Enter TC CAPACITY " Width="150px" onpaste = "return false"
                                                         MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                             
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SLNO" Visible="true" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>

                                       

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="DTR MANUFACTURE_DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCMANFDATE" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO Number" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoNumber" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="RSM_PO_DATE" HeaderText="PO Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("RSM_PO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="RSM_PO_DATE" HeaderText="Invoice Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server" Text='<%# Bind("RSD_INV_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                         <asp:TemplateField AccessibleHeaderText="SUPPLIER" HeaderText="Repairer Name" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRepairer" runat="server" Text='<%# Bind("SUPPLIER") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       

                                        <%--<asp:TemplateField AccessibleHeaderText="SUPPLIER" HeaderText="Repairer/Supplier Name" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRepairer" runat="server" Text='<%# Bind("SUPPLIER") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="grdSupplier" AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found" ShowFooter="true" CssClass="table table-striped table-bordered table-advance table-hover"
                                    AllowPaging="true" runat="server" 
                                      OnRowCommand="grdfaultyrepairer_RowCommand" 
                                    onpageindexchanging="grdfaultyrepairer_PageIndexChanging"
                                     Visible="false" OnSorting="grdfaultyrepairer_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                    <%--<HeaderStyle HorizontalAlign="center" CssClass="Warning" />--%>
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DIVISION" HeaderText="DIVISION" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivision" runat="server" Text='<%# Bind("DIVISION") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR CODE" Visible="true" SortExpression="TC_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttCode" runat="server" placeholder="Enter TC Code " Width="150px" onpaste = "return false"
                                                         MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                          
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="DTR CAPACITY" Visible="true" SortExpression="TC_CAPACITY">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txttcCapacity" runat="server" placeholder="Enter TC CAPACITY " Width="150px" onpaste = "return false"
                                                         MaxLength="6" onkeypress="return onlyNumbers(event)" onchange ="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                             
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SLNO" Visible="true" SortExpression="TC_SLNO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>

                                       

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="DTR MANUFACTURE_DATE" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTCMANFDATE" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO Number" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoNumber" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="RSM_PO_DATE" HeaderText="PO Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("RSM_PO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField AccessibleHeaderText="RSM_PO_DATE" HeaderText="Invoice Date" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server" Text='<%# Bind("RSD_INV_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                         <asp:TemplateField AccessibleHeaderText="SUPPLIER" HeaderText="Supplier Name" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRepairer" runat="server" Text='<%# Bind("SUPPLIER") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       

                                        <%--<asp:TemplateField AccessibleHeaderText="SUPPLIER" HeaderText="Repairer/Supplier Name" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRepairer" runat="server" Text='<%# Bind("SUPPLIER") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    
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

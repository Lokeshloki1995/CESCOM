<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LatestUpdate.aspx.cs" Inherits="IIITS.DTLMS.LatestUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title>DTC Life Cycle Management System</title>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta content="" name="description" />
    <meta content="Mosaddek" name="author" />
    <link href="assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="assets/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="assets/bootstrap/css/bootstrap-fileupload.css" rel="stylesheet" />
    <link href="assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style-responsive.css" rel="stylesheet" />
    <link href="css/style-default.css" rel="stylesheet" id="style_color" />
    <link href="assets/fullcalendar/fullcalendar/bootstrap-fullcalendar.css" rel="stylesheet" />
    <link href="assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet"
        type="text/css" media="screen" />
    <link href="Styles/calendar.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.12/css/all.css" integrity="sha384-G0fIWCsCzJIMAVNQPfjH08cyYaUtMwjJwqiRKxxE/rx96Uroj1BtIQ6MLJuheaO9" crossorigin="anonymous">
    <script type="text/javascript" src="Scripts/functions.js"></script>
    <%--<script src='<%= ResolveUrl("~/Scripts/functions.js") %>' type="text/javascript"></script>--%>
</head>

<body>
    <form id="form1" runat="server">
    <div>
             <div id="header" class="navbar navbar-inverse navbar-fixed-top">
                <!-- BEGIN TOP NAVIGATION BAR -->
                <div class="navbar-inner">
                    <div class="container-fluid">
                        <!--BEGIN SIDEBAR TOGGLE-->
                        <div class="sidebar-toggle-box hidden-phone">
                            <div class="icon-reorder tooltips" data-placement="right" data-original-title="Toggle Navigation">
                            </div>
                        </div>
                        <!--END SIDEBAR TOGGLE-->
                        <!-- BEGIN LOGO -->
                        <a class="brand" style="width: 500px; color: White">DTC Life Cycle Management System
                        </a>
                        <!-- END LOGO -->
                        <!-- BEGIN RESPONSIVE MENU TOGGLER -->
                        <a class="btn btn-navbar collapsed"
                            id="main_menu_trigger" data-toggle="collapse" data-target=".nav-collapse"><span class="icon-bar"></span><span
                                class="icon-bar"></span><span class="icon-bar"></span><span class="arrow"></span></a>
                        <!-- END RESPONSIVE MENU TOGGLER -->
                        <!-- END  NOTIFICATION -->
                        <div
                            class="top-nav ">
                            <ul class="nav pull-right top-menu">
                                <!-- BEGIN SUPPORT -->
                                <!-- END SUPPORT -->
                                <!-- BEGIN USER LOGIN DROPDOWN -->
                                <li style="margin-right: 10px !important; margin-top: -7px;" runat="server" id="liOffDesg">
                                    <%-- <span style="font-weight:bold"> Login Page </span>
                                               <span> <asp:Label ID="lblOfficeName" style="font-size:12px;color:White"  runat="server"></asp:Label></span>
                                    --%>
                                    <asp:LinkButton ID="lknLoginPage" runat="server" Style="font-size: 16px; color: White; font-weight: bold"
                                        OnClick="lknLoginPage_Click"><i class="fa fa-home" aria-hidden="true"></i> Home </asp:LinkButton>
                                    <br />
                                </li>
                            </ul>
                            <!-- END TOP NAVIGATION MENU -->
                        </div>
                    </div>
                </div>
                <!-- END TOP NAVIGATION BAR -->
            </div>
    </div>
        
        <div class="container-fluid">
            <!-- Begin page header --> 
            <div class="row-fluid">
                <div class="widget blue">
                            <div class="widget-body">
                                <div class="widget-body form">
                                      <!-- BEGIN FORM-->      
                                    <div class="form-horizontal">
                                        <div class="space10"></div>
                                        <div class="space10"></div>
                                        <div class="space10"></div
                                        <div class="space10"></div>
                                        <div class="space20"></div>
                                            <div align="center">
                                                <asp:GridView ID="grdLatestUpdates" AutoGenerateColumns="false" PageSize="15"  ShowHeaderWhenEmpty="true" EmptyDataText="No records Found" 
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true" runat="server" TabIndex="16" Width="800px" >
                                                    <Columns>
                                                        <asp:TemplateField AccessibleHeaderText="UPDATEDESCRIPTION" HeaderText="What's New">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRecentUpdates" runat="server" Text='<%# Bind("UPDATEDESCRIPTION") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField AccessibleHeaderText="EFFECTFROM" HeaderText="Last Updated Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLastUpdateDate" runat="server" Text='<%# Bind("EFFECTFROM") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                               </div>
                                            </div>
                                        </div>
                                 </div>
                                             <div class="span7">
                                            </div>
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                    </div>              
                  </div>
            

    </form>
</body>
</html>

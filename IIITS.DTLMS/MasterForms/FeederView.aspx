<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="FeederView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.FeederView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="/ReportFilterControl.ascx" TagName="ReportFilterControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
      <script type="text/javascript">
          //Charactes and space _ to search Station Name
          function characterAndspecialSt(event) {
              var evt = (evt) ? evt : event;
              var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
              ((evt.which) ? evt.which : 0));
              if ((charCode < 65 || charCode > 90) &&
               (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 95) {

                  return false;
              }
              return true;
          }
          //Remove Numbers, Special characters except space to search Station Name
          function cleanSpecialAndNumSt(t) {

              t.value = t.value.toString().replace(/[^_a-zA-Z t\n\r]+/g, '');


          }
          //Charactes and space - . 1 2 to search Feeder Name
          function characterAndspecialFd(event) {
              var evt = (evt) ? evt : event;
              var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
              ((evt.which) ? evt.which : 0));
              if ((charCode < 65 || charCode > 90) &&
               (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 45 && charCode != 46 && charCode != 49 && charCode != 50) {

                  return false;
              }
              return true;
          }
          //Remove Numbers, Special characters except space to search Feeder Name
          function cleanSpecialAndNumFd(t) {

              t.value = t.value.toString().replace(/[^-.a-zA-Z1-2\t\n\r]+/g, '');


          }
          //Remove Special charactes  to search FeederCode
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
          //Remove Special charactes to paste FeederCode
          function cleanSpecialChars(t) {
              debugger;
              t.value = t.value.toString().replace(/[^a-zA-Z 0-9\t\n\r]+/g, '');
              //alert(" Special charactes are not allowed!");
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
                        Feeder View
                    </h3>
                        <a style="margin-right:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4>
                                <i class="icon-reorder"></i>Feeder View</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div style="float: right">
                                <div class="span2">
                                    <asp:Button ID="cmdNew" runat="server" Text="New Feeder" CssClass="btn btn-primary"
                                        OnClick="cmdNew_Click" /></div>
                                 
                            </div>


                            <div class="widget-body form">
                            
                                <uc1:ReportFilterControl ID="ReportFilterControl1" runat="server" />


                                 
                                

                                <div class="widget-body form">
                                    <div class="text-center">
                                        <div class="row-fluid">
                                         
                                          
                                                <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-success" OnClick="cmdLoad_Click" />
                                           
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickFeeder" /><br />
                                        
                                        </div>
                                    </div>
                                </div>
                                <asp:GridView ID="grdFeeder" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowFooter="True" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found"
                                    AllowPaging="true" OnPageIndexChanging="grdFeeder_PageIndexChanging" OnRowCommand="grdFeeder_RowCommand" 
                                    OnSorting="grdFeeder_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" /> 
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="SD_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFeederId" runat="server" Text='<%# Bind("FD_FEEDER_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ST_STATION_CODE" HeaderText="Station Code" SortExpression="ST_STATION_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStationCode" runat="server" Text='<%# Bind("ST_STATION_CODE") %>' Style="word-break: break-all"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panelStationCode" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtStationCode" runat="server" CssClass="input_textSearch" Width="150px"
                                                        placeholder="Enter Station Code" ToolTip="Enter Station Code to Search"   
                                        ></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="ST_NAME" HeaderText="Station Name" SortExpression="ST_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStation" runat="server" Text='<%# Bind("ST_NAME") %>' Style="word-break: break-all"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtStation" runat="server" CssClass="input_textSearch" Width="150px"
                                                        placeholder="Enter Station Name" ToolTip="Enter Station Name to Search" onkeypress="return characterAndspecialSt(event)"  
                                        onchange="return cleanSpecialAndNumSt(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="FD_FEEDER_NAME" HeaderText="Feeder Name" SortExpression="FD_FEEDER_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCorpPhone" runat="server" Text='<%# Bind("FD_FEEDER_NAME") %>'
                                                    Width="150px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtFeederName" runat="server" CssClass="input_textSearch" Width="150px"
                                                        placeholder="Enter Feeder Name" ToolTip="Enter Feeder Name to Search" onkeypress="return characterAndspecialFd(event)"  
                                        onchange = " return cleanSpecialAndNumFd(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="FD_FEEDER_CODE" HeaderText="Feeder Code" SortExpression="FD_FEEDER_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCorpName2" runat="server" Text='<%# Bind("FD_FEEDER_CODE") %>'
                                                    Width="80px" Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel3" runat="server" DefaultButton="btnSearch">
                                                    <asp:TextBox ID="txtFeederCode" runat="server" CssClass="input_textSearch" Width="150px"
                                                        placeholder="Enter Feeder Code" ToolTip="Enter Feeder Code to Search" MaxLength="6" onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)" ></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField AccessibleHeaderText="div_name" HeaderText="Division Name" SortExpression="DIV_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubdivision" runat="server" Text='<%# Bind("DIV_NAME") %>' Width="200px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    Height="25px" ToolTip="Search" TabIndex="9" CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Sub Division Name" SortExpression="OFF_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("OFF_NAME") %>' Width="200px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="FD_TYPE" HeaderText="Feeder Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("FD_TYPE") %>' Width="150px"
                                                    Style="word-break: break-all"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                        CommandName="create" Width="12px" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- END FORM-->
                <!-- END PAGE CONTENT-->
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
                    <h4 class="modal-title">
                        Help</h4>
                </div>
                <div class="modal-body">
                    <ul><li>
                  This Web Page Can Be Used To View All Feeder Details, Edit Existing Feeder Details and Add New Feeder</li>
                      <li> Existing Feeder Details Can Be Edited By Clicking Edit Button</li>
                    <li>  New Feeder Can Be Added By Clicking New Feeder Button
                    
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

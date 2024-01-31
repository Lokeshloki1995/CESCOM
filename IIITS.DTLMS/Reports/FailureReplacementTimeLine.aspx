<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FailureReplacementTimeLine.aspx.cs" Inherits="IIITS.DTLMS.DtcMissMatch.FailureReplacementTimeLine" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);
            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "img/Manual/Expand.png";
            } else {
                div.style.display = "none";
                img.src = "img/Manual/Expand.png";
            }
        }
        function divexpandcollapseChild(divname) {
            var div1 = document.getElementById(divname);
            var img = document.getElementById('img' + divname);
            if (div1.style.display == "none") {
                div1.style.display = "inline";
                img.src = "img/Manual/collapse.png";
            } else {
                div1.style.display = "none";
                img.src = "img/Manual/Expand.png";
            }
        }
    </script>
    <style type="text/css">
        .hidden-column {
            display: none;
        }

        .table tr th {
            background-color: white;
            color: darkblue;
            text-align: center;
        }

        .table tr td {
            word-break: keep-all;
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
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Failure Replacement TimeLine
                    </h3>
                    <a style="float: right!important; margin-right: -372px!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Location</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    From Date
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" TabIndex="1"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">
                                                    To Date
                                                </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" TabIndex="2"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary"
                                                OnClick="cmdLoad_Click" />
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="Button1" runat="server" Text="Export" CssClass="btn btn-primary"
                                                OnClick="cmdExport_Click" />
                                        </div>
                                    </div>
                                </div>
                                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp                           
                                 <div style="overflow-x: auto; width: 1060px">
                                     <asp:GridView ID="GrdCircle" runat="server" AutoGenerateColumns="False" Width="1000px"
                                         ShowFooter="True" CssClass="table table-striped table-bordered table-advance table-hover"
                                         OnRowDataBound="GrdCircle_OnRowDataBound" OnDataBound="OnDataBound">
                                         <Columns>
                                             <asp:TemplateField ItemStyle-Width="60px" HeaderText="">
                                                 <ItemTemplate>
                                                     <a href="JavaScript:divexpandcollapseChild('div<%# Eval("CircleCode") %>');">
                                                         <img id="imgdiv2<%# Eval("CircleCode") %>" width="15px" border="0"
                                                             src="../img/Manual/collapse.png" alt="" /></a>
                                                 </ItemTemplate>
                                                 <ItemStyle Width="40px" VerticalAlign="Middle"></ItemStyle>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="CircleCode" Visible="false">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblCircle" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,
                                                                    "CircleCode") %>'></asp:Label>
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:BoundField ItemStyle-Width="50" DataField="CIRCLECODE" HeaderText="CircleCode" Visible="false" />
                                             <asp:BoundField ItemStyle-Width="170px" ItemStyle-ForeColor="DarkBlue" DataField="CIRCLE" HeaderText="" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAY" HeaderText="Completed" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAYNEW" HeaderText="Pending" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7" HeaderText="Completed" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7NEW" HeaderText="Pending" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15" HeaderText="Completed" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15NEW" HeaderText="Pending" />
                                             <asp:BoundField ItemStyle-Width="100px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30" HeaderText="Completed" />
                                             <asp:BoundField ItemStyle-Width="100px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30NEW" HeaderText="Pending" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30" HeaderText="Completed" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30NEW" HeaderText="Pending" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="TOTAL" HeaderText="Completed" />
                                             <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="TOTALNEW" HeaderText="Pending" />
                                             <asp:TemplateField HeaderStyle-CssClass="hidden-column" ItemStyle-CssClass="hidden-column" FooterStyle-CssClass="hidden-column">
                                                 <ItemTemplate>
                                                     <tr>
                                                         <td colspan="100%">
                                                             <div id="div<%# Eval("CircleCode") %>" style="overflow: auto; display: none; position: relative; left: 15px; overflow: auto">
                                                                 <asp:GridView ID="grdDivision" runat="server" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                     AutoGenerateColumns="false" Width="1000px" DataKeyNames="CircleCode"
                                                                     OnRowDataBound="grdDivision_OnRowDataBound">
                                                                     <Columns>
                                                                         <asp:TemplateField ItemStyle-Width="60px" HeaderText="">
                                                                             <ItemTemplate>
                                                                                 <a href="JavaScript:divexpandcollapseChild('div1<%# Eval("Divisioncode") %>');">
                                                                                     <img id="imgdiv3<%# Eval("Divisioncode") %>" width="15px" border="0" src="../img/Manual/collapse.png"
                                                                                         alt="" /></a>
                                                                             </ItemTemplate>
                                                                             <ItemStyle Width="10px" VerticalAlign="Middle"></ItemStyle>
                                                                         </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="Division" Visible="false">
                                                                             <ItemTemplate>
                                                                                 <asp:Label ID="lblDivision" runat="server" Text='<%#DataBinder.Eval
                                                                               (Container.DataItem, "Divisioncode") %>'></asp:Label>
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>
                                                                         <asp:BoundField ItemStyle-Width="80px" DataField="Divisioncode" HeaderText="Divisioncode" Visible="false" />
                                                                         <asp:BoundField ItemStyle-Width="170px" ItemStyle-ForeColor="Black" DataField="DIVISION" HeaderText="" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAY" HeaderText="Completed" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAYNEW" HeaderText="Pending" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7" HeaderText="Completed" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7NEW" HeaderText="Pending" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15" HeaderText="Completed" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15NEW" HeaderText="Pending" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30" HeaderText="Completed" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30NEW" HeaderText="Pending" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30" HeaderText="Completed" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30NEW" HeaderText="Pending" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="TOTAL" HeaderText="Completed" />
                                                                         <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="TOTALNEW" HeaderText="Pending" />
                                                                         <asp:TemplateField HeaderStyle-CssClass="hidden-column" ItemStyle-CssClass="hidden-column" FooterStyle-CssClass="hidden-column">
                                                                             <ItemTemplate>
                                                                                 <tr>
                                                                                     <td colspan="100%">
                                                                                         <div id="div1<%# Eval("Divisioncode") %>" style="overflow: auto; display: none; position: relative; left: 15px; overflow: auto">
                                                                                             <asp:GridView ID="grdSubdivision" runat="server" Width="110%" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                                                 AutoGenerateColumns="false" OnRowDataBound="grdSubdivision_OnRowDataBound">
                                                                                                 <Columns>
                                                                                                     <asp:TemplateField ItemStyle-Width="70px" HeaderText="">
                                                                                                         <ItemTemplate>
                                                                                                             <a href="JavaScript:divexpandcollapseChild('div1<%# Eval("Subdivisioncode") %>');">
                                                                                                                 <img id="imgdiv4<%# Eval("Subdivisioncode") %>" width="15px" border="0" src="../img/Manual/collapse.png"
                                                                                                                     alt="" /></a>
                                                                                                         </ItemTemplate>
                                                                                                         <ItemStyle Width="40px" VerticalAlign="Middle"></ItemStyle>
                                                                                                     </asp:TemplateField>
                                                                                                     <asp:TemplateField HeaderText="SubDivision" Visible="false" HeaderStyle-CssClass="hidden-column" ItemStyle-CssClass="hidden-column" FooterStyle-CssClass="hidden-column">
                                                                                                         <ItemTemplate>
                                                                                                             <asp:Label ID="lblSubDivision" runat="server" Text='<%#DataBinder.Eval
                                                                                                          (Container.DataItem, "Subdivisioncode") %>'></asp:Label>
                                                                                                         </ItemTemplate>
                                                                                                     </asp:TemplateField>
                                                                                                     <asp:BoundField ItemStyle-Width="80px" DataField="Subdivisioncode" HeaderText="Subdivisioncode" Visible="false" />
                                                                                                     <asp:BoundField ItemStyle-Width="170px" ItemStyle-ForeColor="Black" DataField="SUBDIVISION" HeaderText="" ControlStyle-ForeColor="Black" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAY" HeaderText="Completed" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAYNEW" HeaderText="Pending" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7" HeaderText="Completed" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7NEW" HeaderText="Pending" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15" HeaderText="Completed" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15NEW" HeaderText="Pending" />
                                                                                                     <asp:BoundField ItemStyle-Width="100px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30" HeaderText="Completed" />
                                                                                                     <asp:BoundField ItemStyle-Width="100px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30NEW" HeaderText="Pending" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30" HeaderText="Completed" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30NEW" HeaderText="Pending" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="TOTAL" HeaderText="Completed" />
                                                                                                     <asp:BoundField ItemStyle-Width="80px" ItemStyle-ForeColor="DarkBlue" DataField="TOTALNEW" HeaderText="Pending" />
                                                                                                     <asp:TemplateField HeaderStyle-CssClass="hidden-column" ItemStyle-CssClass="hidden-column" FooterStyle-CssClass="hidden-column">
                                                                                                         <ItemTemplate>
                                                                                                             <tr>
                                                                                                                 <td colspan="200%">
                                                                                                                     <div id="div1<%# Eval("Subdivisioncode") %>" style="overflow: auto; display: none; position: relative; left: 15px; overflow: auto">
                                                                                                                         <asp:GridView ID="grdSection" runat="server" Width="200%" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                                                                             OnRowDataBound="grdSection_OnRowDataBound" AutoGenerateColumns="false">
                                                                                                                             <Columns>
                                                                                                                                 <asp:TemplateField ItemStyle-Width="300px" HeaderText="">
                                                                                                                                     <ItemTemplate>
                                                                                                                                         <a href="JavaScript:divexpandcollapseChild('div1<%# Eval("Sectioncode") %>');">
                                                                                                                                             <img id="imgdiv5<%# Eval("Sectioncode") %>" width="15px" border="0" src="../img/Manual/collapse.png"
                                                                                                                                                 alt="" /></a>
                                                                                                                                     </ItemTemplate>
                                                                                                                                     <ItemStyle Width="40px" VerticalAlign="Middle"></ItemStyle>
                                                                                                                                 </asp:TemplateField>
                                                                                                                                 <asp:TemplateField HeaderText="Section" Visible="false">
                                                                                                                                     <ItemTemplate>
                                                                                                                                         <asp:Label ID="lblSection" runat="server" Text='<%#DataBinder.Eval (Container.DataItem, "Sectioncode") %>'></asp:Label>
                                                                                                                                     </ItemTemplate>
                                                                                                                                 </asp:TemplateField>
                                                                                                                                 <asp:BoundField ItemStyle-Width="10px" DataField="Sectioncode" HeaderText="Sectioncode" Visible="false" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="200px" ItemStyle-ForeColor="Black" DataField="SECTION" HeaderText="" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAY" HeaderText="Completed" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAYNEW" HeaderText="Pending" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7" HeaderText="Completed" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7NEW" HeaderText="Pending" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15" HeaderText="Completed" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15NEW" HeaderText="Pending" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30" HeaderText="Completed" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30NEW" HeaderText="Pending" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30" HeaderText="Completed" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30NEW" HeaderText="Pending" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="TOTAL" HeaderText="Completed" />
                                                                                                                                 <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="TOTALNEW" HeaderText="Pending" />
                                                                                                                                 <asp:TemplateField HeaderStyle-CssClass="hidden-column" ItemStyle-CssClass="hidden-column" FooterStyle-CssClass="hidden-column">
                                                                                                                                     <ItemTemplate>
                                                                                                                                         <tr>
                                                                                                                                             <td colspan="100%">
                                                                                                                                                 <div id="div1<%# Eval("Sectioncode") %>" style="overflow: auto; display: none; position: relative; left: 15px; overflow: auto">
                                                                                                                                                     <asp:GridView ID="grdCategory" runat="server" Width="110%" CssClass="table table-striped table-bordered table-advance table-hover"
                                                                                                                                                         AutoGenerateColumns="false" OnRowDataBound="grdCategory_OnRowDataBound" OnRowCommand="grdCategory_RowCommand" EnableViewState="false">
                                                                                                                                                         <Columns>
                                                                                                                                                             <asp:TemplateField AccessibleHeaderText="FC_ID" HeaderText="ID" Visible="false">
                                                                                                                                                                 <ItemTemplate>
                                                                                                                                                                     <asp:Label ID="lblCategroyid" runat="server" Text='<%# Bind("FC_ID") %>'></asp:Label>
                                                                                                                                                                 </ItemTemplate>
                                                                                                                                                             </asp:TemplateField>
                                                                                                                                                             <asp:BoundField ItemStyle-Width="80px" DataField="Sectioncode" HeaderText="Sectioncode" Visible="false" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="80px" DataField="FC_ID" HeaderText="FC_ID" Visible="false" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="170px" ItemStyle-ForeColor="Black" DataField="FC_NAME" HeaderText="" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAY" HeaderText="Completed" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="LESSTHAN1DAYNEW" HeaderText="Pending" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7" HeaderText="Completed" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="BW1TO7NEW" HeaderText="Pending" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15" HeaderText="Completed" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="BW7TO15NEW" HeaderText="Pending" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30" HeaderText="Completed" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="BW15TO30NEW" HeaderText="Pending" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30" HeaderText="Completed" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="250px" ItemStyle-ForeColor="DarkBlue" DataField="ABOVE30NEW" HeaderText="Pending" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="TOTAL" HeaderText="Completed" />
                                                                                                                                                             <asp:BoundField ItemStyle-Width="350px" ItemStyle-ForeColor="DarkBlue" DataField="TOTALNEW" HeaderText="Pending" />
                                                                                                                                                             <asp:TemplateField ItemStyle-Width="60px" HeaderText="Action">
                                                                                                                                                                 <ItemTemplate>
                                                                                                                                                                     <center>
                                                                                                                                                                         <a href='<%#"/Reports/FailureTimeLineDetails.aspx?FeederCatId="+ DataBinder.Eval(Container.DataItem, "FC_ID")%>&SectionCode=<%# DataBinder.Eval(Container.DataItem, "Sectioncode")%>&fromDate=<%#txtFromDate.Text.Trim()%>&ToDate=<%#txtToDate.Text.Trim()%>' target="_blank" style="font-size: medium">
                                                                                                                                                                             <img alt="" src="../img/Manual/View1.jpg" style="width: 30px" /></a>
                                                                                                                                                                     </center>
                                                                                                                                                                 </ItemTemplate>
                                                                                                                                                             </asp:TemplateField>
                                                                                                                                                         </Columns>
                                                                                                                                                         <HeaderStyle />
                                                                                                                                                     </asp:GridView>
                                                                                                                                                 </div>
                                                                                                                                             </td>
                                                                                                                                         </tr>
                                                                                                                                     </ItemTemplate>
                                                                                                                                 </asp:TemplateField>
                                                                                                                             </Columns>
                                                                                                                             <%-- BackColor="#0063A6" ForeColor="Blue" --%>
                                                                                                                             <HeaderStyle />
                                                                                                                         </asp:GridView>
                                                                                                                     </div>
                                                                                                                 </td>
                                                                                                             </tr>
                                                                                                         </ItemTemplate>
                                                                                                     </asp:TemplateField>
                                                                                                 </Columns>
                                                                                                 <HeaderStyle BackColor="#95B4CA" ForeColor="Blue" />
                                                                                             </asp:GridView>
                                                                                         </div>
                                                                                     </td>
                                                                                 </tr>
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>
                                                                     </Columns>
                                                                     <HeaderStyle BackColor="#4D92C1" ForeColor="Blue" />
                                                                 </asp:GridView>
                                                             </div>
                                                         </td>
                                                     </tr>
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                         </Columns>
                                         <HeaderStyle BackColor="#0063A6" ForeColor="Blue" />
                                     </asp:GridView>
                                 </div>
                            </div>
                        </div>
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
                        <li>This Report Will Display Pending And Completed invoice for DTC Failure</li>
                        <li>It will show Pending And Completed counts within 1 day, between 1 to 7 days, 
                        between 7 to 15 days, between 15 to 30 days, above 30 days  </li>
                        <li>If you select from date and to date you will get only records between from date and to date </li>
                        <li>Completed Indicate Invoice done and pending with RI or CR </li>
                        <li>Pending Indicate Pending For invoice </li>
                        <li>If you click on circle view button you will get expand and will display Division Views and so on
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

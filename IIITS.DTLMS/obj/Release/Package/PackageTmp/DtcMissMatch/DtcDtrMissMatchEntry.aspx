<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="DtcDtrMissMatchEntry.aspx.cs" Inherits="IIITS.DTLMS.DtcMissMatch.DtcDtrMissMatchEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateMyForm() {

            if (document.getElementById('<%= txtDtcCode.ClientID %>').value.trim() == "") {
                alert('Select Valid DTC Code')
                document.getElementById('<%= txtDtcCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDtrCode.ClientID %>').value.trim() == "") {
                alert('Select Valid DTR Code')
                document.getElementById('<%= txtDtrCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtremarks.ClientID %>').value.trim() == "") {
                alert('Select Valid Remarks')
                document.getElementById('<%= txtremarks.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%=cmdAllocate.ClientID %>').value.trim() == "Allocate") {
                return confirm("Are You Sure Want To Allocate DTC");
            }
        }

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

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

        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        //allow only Numbers to paste
        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }

        //allow Chatractes and Numbers to paste
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
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                  MisMatch  DTC DTR Allocation
                </h3>
               <%-- <h5><span class="label label label-info">Description: This Web Page Can be Used For DTC/DTR Mapping If There is Any Mis-Match </span></h5>--%>

                 <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
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
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To DTC DTR Allocation
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->
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
            </div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i> MisMatch DTC DTR Allocation</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                        <asp:HiddenField ID="hdfDtrCode" runat="server" />
                                        <asp:HiddenField ID="hdfLocType" runat="server" />
                                        <asp:HiddenField ID="hdfOffCode" runat="server" />
                                        <asp:HiddenField ID="hdfNewLocType" runat="server" />
                                        <asp:HiddenField ID="hdfNewOffCode" runat="server" />
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtcCode" runat="server" MaxLength="6" onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)" ></asp:TextBox>
                                                    <asp:Button ID="btnDtcSearch" runat="server" Text="S" TabIndex="2" CssClass="btn btn-primary"
                                                        OnClick="btnDtcSearch_Click" />
                                                </div>
                                            </div>
                                            
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Remarks<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="span4"></div>
                                        <div class="span4" id="checkboxdiv" runat="server">
                                        <div class="checkbox">
                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="" AutoPostBack="True" 
                                                oncheckedchanged="CheckBox1_CheckedChanged" />
                                            <span class="label label-important">Select If DTC is Available For Above
                                            <asp:Label ID="lblDtrCode" runat="server" Text=""></asp:Label> </span>
                                        </div>
                                    </div>
                                    <div style="margin-left:35px;" id="divOldTc" runat="server">
                                    
                                    <div class="span5">
                                    <div id="dtcold" class="span3" runat="server">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTC Code </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtOldDtc" runat="server"></asp:TextBox>
                                                    <asp:Button ID="btnSearchdtc2" runat="server" Text="S" TabIndex="2" 
                                                        CssClass="btn btn-primary" onclick="btnSearchdtc2_Click"/>
                                                </div>
                                            </div>
                                        </div>




                                    </div></div></div>

                                    </div>
                                    <div class="span5" runat="server">
                                        <div class="control-group">
                                            <label class="control-label">
                                                DTR Code<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDtrCode" runat="server" MaxLength="6" onkeypress="return onlyNumbers(event)" onchange = "return cleanSpecialAndChar(this)"></asp:TextBox>
                                                    <asp:Button ID="btnDtrSearch" runat="server" Text="S" TabIndex="2" CssClass="btn btn-primary"
                                                        OnClick="btnDtrSearch_Click" />
                                                </div>
                                            </div>
                                        </div>

                                           <div class="control-group">
                                                <label class="control-label">
                                                    DTR Commission Date <span class="Mandotary"> *</span> </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTRCommDate" runat="server" MaxLength="10" TabIndex="5" AutoComplete ="Off"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtDTRCommDate"  Format="dd/MM/yyyy" >
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                    </div>
                                    </div>
                                    
                                </div>
                                <div class="space20">
                                </div>
                               <%-- <div class="space5">
                                 <div class="control-group">
                                            <label class="control-label">
                                                Remarks </label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:Label ID="lblRemarks" runat="server" Text=""></asp:Label> <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                </div>--%>
                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span12">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>Mapping Details</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body" >
                        <div class="widget-body form">
                            <div class="form-horizontal">
                                <div class="row-fluid" style="width:50%">
                                    <asp:GridView ID="grdDtcDetails" runat="server" AutoGenerateColumns="False" 
                                        CssClass="table table-striped table-bordered table-advance table-hover" 
                                        Width="200px" onrowcreated="grdDtcDetails_RowCreated">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="DTC Code" Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("dt_code") %>' Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_Name" HeaderText="DTC Name" Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcName" runat="server" Text='<%# Bind("DT_NAME") %>' Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_Capacity" HeaderText="DTC Capacity"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_tc_id" HeaderText="Dtr Code"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtrCode" runat="server" Text='<%# Bind("DT_TC_ID") %>' Style="word-break: break-all;"
                                                        Width="100px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                   <%-- <div style="position:absolute; top: 432px; left: 740px;">--%>
                                   <div style="margin-left:500px;margin-top:-135px;">

                                      <asp:GridView ID="grdDtrDetails" runat="server" AutoGenerateColumns="False" 
                                            CssClass="table table-striped table-bordered table-advance table-hover" 
                                            onrowcreated="grdDtrDetails_RowCreated">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="TC Code" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="TC Capacity" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CURRENT_LOCATION" HeaderText="TC Current Location"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCurrentLocation" runat="server" Text='<%# Bind("CURRENTLOCATION") %>'
                                                            Style="word-break: break-all;" Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_STATUS" HeaderText="TC Status" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_Location" HeaderText="TC Location" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOffNmae" runat="server" Text='<%# Bind("OFFNAME") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        </div>
                                        <div style="margin-top:140px;">
                                    <asp:GridView ID="grdSecondDtcDetails" runat="server" AutoGenerateColumns="False" 
                                        CssClass="table table-striped table-bordered table-advance table-hover" 
                                                Width="200px" onrowcreated="grdSecondDtcDetails_RowCreated"  >
                                      <Columns>
                                            <asp:TemplateField AccessibleHeaderText="DTC_CODE" HeaderText="DTC Code" Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcCode2" runat="server" Text='<%# Bind("dt_code") %>' Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_Name" HeaderText="DTC Name" Visible="true"
                                                HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtcName2" runat="server" Text='<%# Bind("DT_NAME") %>' Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_Capacity" HeaderText="DTC Capacity"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTcCapacity2" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="DTC_tc_id" HeaderText="Dtr Code"
                                                Visible="true" HeaderStyle-ForeColor="Black">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDtrCode2" runat="server" Text='<%# Bind("DT_TC_ID") %>' Style="word-break: break-all;"
                                                        Width="90px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>


                                    <div style="margin-left:500px;margin-top:-135px;">
                                    <asp:GridView ID="grdDtrDetails2" runat="server" AutoGenerateColumns="False" 
                                            CssClass="table table-striped table-bordered table-advance table-hover" 
                                            onrowcreated="grdDtrDetails2_RowCreated" >
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="TC Code" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCode2" runat="server" Text='<%# Bind("TC_Code") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="TC Capacity" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCapacity2" runat="server" Text='<%# Bind("TC_CAPACITY") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_CURRENT_LOCATION" HeaderText="TC Current Location"
                                                    Visible="true" HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTcCurrentLocation2" runat="server" Text='<%# Bind("CURRENTLOCATION") %>'
                                                            Style="word-break: break-all;" Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_STATUS" HeaderText="TC Status" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus2" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="TC_Location" HeaderText="TC Location" Visible="true"
                                                    HeaderStyle-ForeColor="Black">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOffNmae2" runat="server" Text='<%# Bind("OFFNAME") %>' Style="word-break: break-all;"
                                                            Width="90px" ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>


                                </div>
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--<div class="row-fluid">
                <div class="span12">
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>DTR Details</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form">
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <asp:GridView ID="grdNewMappingDetails" runat="server" 
                                            onrowcreated="grdNewMappingDetails_RowCreated">
                                        </asp:GridView>
                                    </div>
                                    <div class="space20">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
        </div>
    </div>
    </div>
     <div class="form-horizontal" align="center">
                                    <div class="span5">
                                    </div>
                                    <div class="span1">
                                        <asp:Button ID="cmdAllocate" runat="server" Text="Allocate" CssClass="btn btn-primary"
                                            OnClientClick="javascript:return ValidateMyForm()" 
                                            onclick="cmdAllocate_Click" />
                                    </div>
                                    <%-- <div class="span1"></div>--%>
                                    <div class="span1">
                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                            CssClass="btn btn-primary" onclick="cmdReset_Click" /><br />
                                    </div>
                                    <div class="span7">
                                    </div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
</asp:Content>

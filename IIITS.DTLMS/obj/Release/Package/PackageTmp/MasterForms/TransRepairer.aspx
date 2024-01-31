<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TransRepairer.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TransRepairer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateMyForm() {
            if (Page_ClientValidate()) {
                if (document.getElementById('<%= txtRepairName.ClientID %>').value.trim() == "") {
                    alert('Enter Valid Repairer name')
                    document.getElementById('<%= txtRepairName.ClientID %>').focus()
                    return false
                }

                if (document.getElementById('<%= cmbDivision.ClientID %>').value == "-Select-") {
                    alert('Select Division Name')
                    document.getElementById('<%= cmbDivision.ClientID %>').focus()
                    return false
                }

                if (document.getElementById('<%= txtRepairPhnNo.ClientID %>').value.trim() == "") {
                    alert('Enter Valid Repairer Phone No')
                    document.getElementById('<%= txtRepairPhnNo.ClientID %>').focus()
                    return false

                }

                if (document.getElementById('<%= txtRepairEmailId.ClientID %>').value.trim() == "") {
                    alert('Enter Valid Repairer EmailId')
                    document.getElementById('<%= txtRepairEmailId.ClientID %>').focus()
                    return false
                }


                if (document.getElementById('<%= txtRepairAddress.ClientID %>').value.trim() == "") {
                    alert('Enter Valid Register Address')
                    document.getElementById('<%= txtRepairAddress.ClientID %>').focus()
                    return false
                }
                if (document.getElementById('<%= txtContactPerson.ClientID %>').value.trim() == "") {
                    alert('Enter contact  Person')
                    document.getElementById('<%= txtContactPerson.ClientID %>').focus()
                    return false
                }
            }
        }

        //Charactes and space to create Tc Name
        function characterAndspecialTcRe(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && charCode != 32) {

                return false;
            }
            return true;
        }

        // Only Characters to enter
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
                    <h3 class="page-title">Repairer Details at Taluk
                    </h3>
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="Text1" type="text">
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Repairer View"
                        OnClientClick="javascript:window.location.href='TransRepairerView.aspx'; return false;"
                        CssClass="btn btn-primary" />
                </div>

            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Repairer Details at Taluk</h4>
                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1"></div>


                                        <div class="span5">



                                            <div class="control-group">
                                                <label class="control-label">Repairer Name <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtRepairName" runat="server" MaxLength="100" Width="300px"  TabIndex="1" onkeypress="return characterAndspecialTcRe(event)" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    Phone Number
                                                    <br />
                                                    <span>(main office with STD Code)</span><span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRepairPhnNo" runat="server"
                                                            MaxLength="15" Width="300px"
                                                            onkeypress="javascript:return OnlyNumberHyphen(this,event);" TabIndex="3" onpaste="return false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Email Id <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRepairEmailId" runat="server" MaxLength="50" Width="300px"
                                                            CausesValidation="True" TabIndex="4" onpaste="return false"></asp:TextBox>

                                                        <asp:RegularExpressionValidator runat="server" ID="regular" ControlToValidate="txtRepairEmailId"
                                                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
                                                            ErrorMessage="Please enter a valid email id!!!!" ForeColor="Red"
                                                            Display="Dynamic" Font-Size="Small" />


                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" Width="300px" AutoPostBack="true" TabIndex="2" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Taluk<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTalq" Width="300px" runat="server" TabIndex="2" onpaste="return false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>







                                            <div class="control-group">
                                                <label class="control-label">Contact Person Name<span class="Mandotary"> *</span> </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtContactPerson" runat="server" MaxLength="50" TabIndex="5" onpaste="return false" Width="300px"  onkeypress="return character(event)"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Mobile Number </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10" Width="300px"
                                                            onkeypress="javascript:return OnlyNumber(event);" TabIndex="8" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                           

                                        </div>
                                        <div class="span5" rowspan="2">

                                            <div class="control-group">
                                                <label class="control-label">Register Address<span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRepairerId" runat="server" MaxLength="50" Visible="false" Width="20px"></asp:TextBox>
                                                          <asp:TextBox ID="txtRepairerTalukID" runat="server" MaxLength="50" Visible="false" Width="20px"></asp:TextBox>
                                                        <asp:TextBox ID="txtRepairAddress" runat="server" MaxLength="500" Height="60px" Width="300px"
                                                            TextMode="MultiLine" 
                                                            onkeyup="return ValidateTextlimit(this,250);" TabIndex="12"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="control-group">
                                                <label class="control-label">Fax No </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="20" TabIndex="9" Width="300px" onpaste="return false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Contract Period(in Years) </label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtContractPeriod" runat="server" MaxLength="4" TabIndex="6" Width="300px"
                                                            onkeypress="javascript:return AllowNumber(this,event);" onpaste="return false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                           

                                            <div class="control-group">
                                                <label class="control-label">Block Listed</label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbIsBlack" runat="server" Width="300px"
                                                            OnSelectedIndexChanged="cmbIsBlack_SelectedIndexChanged"
                                                            AutoPostBack="true" TabIndex="10">
                                                            <asp:ListItem>-Select-</asp:ListItem>
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">BlockListed Upto<span class="Mandotary" runat="server" id="blocklist" > *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
<%--                                                        <asp:TextBox ID="txtStatus" runat="server" MaxLength="10" TabIndex="11"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtBlackUpto" runat="server" MaxLength="10" Width="300px" TabIndex="11"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtdateExtender" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtBlackUpto" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>





                                            <div class="control-group">
                                                <label class="control-label">Communication  Address </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                       <!-- Style="resize: none"-->
                                                        <asp:TextBox ID="txtCommAddress" runat="server" MaxLength="200"
                                                             Height="60px" TextMode="MultiLine" Width="300px"
                                                            onkeyup="return ValidateTextlimit(this,200);" TabIndex="13" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>



                                        </div>
                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="text-center" align="center">


                                        <asp:Button ID="cmdSave" runat="server" Text="Save" OnClick="cmdSave_Click" CausesValidation="false"
                                            OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-success" />


                                        <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                                            OnClick="cmdReset_Click" CssClass="btn btn-danger" /><br />

                                        <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                                    </div>
                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->



                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->
            <!--Addition of Table -->
            <div id ="talqReapirerTable" runat="server">
            <div class="row-fluid">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>Existing Repairer</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">

                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">

                                    <asp:GridView ID="grdRepairer" AutoGenerateColumns="false" PageSize="30" AllowPaging="true"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        runat="server" OnPageIndexChanging="grdRepairer_PageIndexChanging" OnRowCommand="grdRepairer_RowCommand"
                                        OnSorting="grdRepairer_Sorting" AllowSorting="true">
                                        <HeaderStyle CssClass="both" />
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="TR_ID" HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRepairId" runat="server" Text='<%# Bind("TR_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            

                                            <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Repairer Name" Visible="true" SortExpression="TR_NAME" ItemStyle-Width="500">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("TR_NAME") %>' Style="word-break: break-all;" Width="500"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                        <asp:TextBox ID="txtRepairerName" runat="server" placeholder="Enter Repairer Name" Width="100px"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="TS_ADDRESS" HeaderText="Addresss" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("TR_ADDRESS") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="TS_PHONE" HeaderText="Phone no" Visible="true" ItemStyle-Width="130">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblphone" runat="server" Text='<%# Bind("TR_PHONE") %>' Style="word-break: break-all;" Width="130"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="TS_EMAIL" HeaderText="EmailId" Visible="true" ItemStyle-Width="300">

                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("TR_EMAIL") %>' Style="word-break: break-all;" Width="300"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Click to Create">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                                            Width="12px" CommandName="upload" />
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Font-Size="Medium" Width="15px" />
                                        <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" Font-Bold="true" Font-Size="Medium" Width="15px" HorizontalAlign="Center" VerticalAlign="Middle" />

                                    </asp:GridView>

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

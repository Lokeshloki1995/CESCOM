<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TCTesting.aspx.cs" Inherits="IIITS.DTLMS.TCRepair.TCTesting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=cmdSave.ClientID %>').prop('disabled', true);
        }

        window.onbeforeunload = preventMultipleSubmissions;

    </script>

    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">



        function ValidateSave() {


            if (document.getElementById('<%= cmbTestedBy.ClientID %>').value == "--Select--") {
                alert('Select Tested By')
                document.getElementById('<%= cmbTestedBy.ClientID %>').focus()
                return false;
            }
            if (document.getElementById('<%= txtTestedOn.ClientID %>').value.trim() == "") {
                alert('Enter valid Tested On')
                document.getElementById('<%= txtTestedOn.ClientID %>').focus()
                return false;
            }
            if (document.getElementById('<%= cmbTestLocation.ClientID %>').value == "--Select--") {
                alert('Select Testing Location')
                document.getElementById('<%= cmbTestLocation.ClientID %>').focus()
                return false;
            }
            if (document.getElementById('<%= hdfrsmoiltype.ClientID %>').value == "0") {
                if (document.getElementById('<%= cmdOilSupplyingBy.ClientID %>').value == "--Select--") {
                    alert('Please Select Oil Supplying By.')
                    document.getElementById('<%= cmdOilSupplyingBy.ClientID %>').focus()
                    return false;
                }
            }
            if (document.getElementById('<%= cmdOilSupplyingBy.ClientID %>').value == "2") {
                if (document.getElementById('<%= txtRepairerSupplyingOilQTY.ClientID %>').value == "") {
                    alert('Please Enter Repairer Supplying Oil Quantity.(Ltrs).');
                    document.getElementById('<%= cmdOilSupplyingBy.ClientID %>').focus()
                  return false;
              }
          }

          var Totalquantity = document.getElementById('<%=  txtPOTotalOilQTY.ClientID %>').value;
            var AvailQuantity = document.getElementById('<%=  txtPOPendingOilQTY.ClientID %>').value;
            var Enteredoilqty = document.getElementById('<%= txtRepairerSupplyingOilQTY.ClientID %>').value;
            var Pono = document.getElementById('<%= txtPONo.ClientID %>').value;

            if (AvailQuantity != "" && document.getElementById('<%= hdfrsmoiltype.ClientID %>').value == "0" && (document.getElementById('<%= cmdOilSupplyingBy.ClientID %>').value == "2")) {
                debugger;
                var Remainingqty = AvailQuantity - Enteredoilqty;
                var result = confirm('In PO No ' + Pono + '' + Remainingqty + ' Ltr Oil is Still Due to Issue out of Total Oil ' + Totalquantity + '  Ltr Quantity Do you want to Continue?');
                if (result) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function ConfirmDelete() {

            var result = confirm('Are you sure you want to Remove?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }

        function onlyNumberswithdot(event, t) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode == 46 && (t.value == "" || t.value.indexOf('.') == 0))
                return false;
            if (charCode == 46 && t.value.split('.').length == 2)
                return false;
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46))
                return false;
            return true;
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            var temp = "<%= hiddenTestedOn.Text %>";


            $("#<%= txtTestedOn.ClientID %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: 0,
                minDate: new Date(temp),


            })
            //  $("#<%= txtTestedOn.ClientID %>").datepicker("setDate" ,"24/08/2020" )
        }


        );
        //  alert("<%= hiddenTestedOn.Text %>");


        function isNumberKey(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8))
                return false;
            else {
                var len = $(element).val().length;
                var index = $(element).val().indexOf('.');
                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    var CharAfterdot = (len + 1) - index;
                    if (CharAfterdot > 2) {
                        return false;
                    }
                }

            }
            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Transformer Inspection at Repair Center          
                                      
                    </h3>

                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='TestPendingSearch.aspx'; return false;"
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
                            <h4><i class="icon-reorder"></i>Deliver DTR</h4>
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
                                                <label class="control-label">Store</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStore" runat="server" Enabled="false">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Purchase Order No.</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPONo" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Issue Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtIssueDate" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" TargetControlID="txtIssueDate" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Old Purchase Order No.</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOldPONo" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="row-fluid" runat="server" id="divwithoutoil" style="display: none">
                                                <div class="control-group">
                                                    <label class="control-label">Total Oil Qty(Ltrs)</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtPOTotalOilQTY" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Pending Oil Qty(Ltrs)</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtPOPendingOilQTY" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Already Issued by Store Keeper(Ltrs)</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtStoreAlradyIssued" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Already Issued by Repairer(Ltrs)</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtRepairerAlreadyIssued" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span5">

                                            <div class="control-group">
                                                <label class="control-label">Tested By<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTestedBy" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Tested On<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTestedOn" runat="server" MaxLength="25"></asp:TextBox>
                                                        <asp:TextBox ID="txtSelectedDetailsId" runat="server" MaxLength="10" Visible="false" Width="20px"> </asp:TextBox>
                                                        <asp:TextBox ID="hiddenTestedOn" runat="server" MaxLength="10" Visible="false" Width="20px"> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Testing Location<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbTestLocation" runat="server">
                                                            <asp:ListItem Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Vendor Premises"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Department"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="row-fluid" runat="server" id="divwithoutoilright" style="display: none">
                                                <div class="control-group">
                                                    <label class="control-label">Oil Supplying By<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtrsmoiltype" runat="server" MaxLength="10" Visible="false" Width="20px"> </asp:TextBox>

                                                            <asp:DropDownList ID="cmdOilSupplyingBy" Enabled="false" AutoPostBack="true" runat="server" TabIndex="1" OnSelectedIndexChanged="cmdOilSupplyingby_Click">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Store"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Repairer"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Oil Supplying By Repairer(Ltrs)</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtRepairerSupplyingOilQTY" runat="server" oncopy="return false" onpaste="return false"  autocomplete="off" Enabled="false" MaxLength="10" onkeypress="javascript:return onlyNumberswithdot(event,this);"
                                                                AutoPostBack="true" ></asp:TextBox>
                                                           <%-- onkeypress="return isNumberKey(event,this)"--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Remarks</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtPO_Remarks" runat="server" MaxLength="25" TextMode="MultiLine" Style="resize: none" Enabled="false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" style="display: none" runat="server">
                                                <label class="control-label">Upload<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupTestDocument" runat="server" AllowMultiple="False" />
                                                    </div>
                                                </div>
                                            </div>
                                            
                                        </div>

                                    </div>
                                    <div class="space20"></div>
                                    <div class="space20"></div>

                                    <div class="form-horizontal" align="center">
                                        <div class="span3"></div>
                                        <div class="span1">
                                            <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return ValidateSave()"
                                                CssClass="btn btn-primary" OnClick="cmdSave_Click" />
                                        </div>
                                        <div class="span1">
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                                CssClass="btn btn-primary" OnClick="cmdReset_Click" />
                                        </div>
                                        <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:HiddenField ID="hdfResult" runat="server" />
                                        <asp:HiddenField ID="hdfRemarks" runat="server" />
                                        <asp:HiddenField ID="hdfrsmoiltype" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="space20"></div>
                            <!-- END FORM-->
                            <%--<div id = "div1" style= "height:600px; width:1050px;" runat="server">--%>
                            <div id="divResult" style="overflow: scroll; height: 600px;" runat="server">
                                <asp:GridView ID="grdDeliverDetails" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowHeaderWhenEmpty="True"
                                    EmptyDataText="No Records Found"
                                    OnRowCommand="grdDeliverDetails_RowCommand"
                                    OnRowDataBound="grdDeliverDetails_RowDataBound">
                                    <Columns>



                                        <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmake" runat="server" Text='<%# Bind("TM_NAME") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("CAPACITY") %>' Style="word-break: break-all;" Width="40px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>' Style="word-break: break-all;" Width="80px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="SUP_REPNAME" HeaderText="Supplier / Repairer" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" runat="server" Text='<%# Bind("SUP_REPNAME") %>' Style="word-break: break-all;" Width="40px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="RSM_PO_NO" HeaderText="PO Number" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPONo" runat="server" Text='<%# Bind("RSM_PO_NO") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Testing Result">
                                            <ItemTemplate>
                                                <div style="float: right; width: 441px; margin-left: 30px;">
                                                    <span class="span6">
                                                        <asp:RadioButton ID="rdbPass" runat="server" Text="Pass" GroupName="a" CssClass="radio" />
                                                        <asp:RadioButton ID="rdbScrap" runat="server" Text="Not Repairable" GroupName="a" CssClass="radio" />
                                                        <asp:RadioButton ID="rdbPasswithCopper" runat="server" Text="All test Ok expt Cu loss" GroupName="a" CssClass="radio" />
                                                        <asp:RadioButton ID="rdbPassBoth" runat="server" Text="All test Ok expt Cu & Iron loss" GroupName="a" CssClass="radio" />

                                                    </span>
                                                    <span class="span6">
                                                        <asp:RadioButton ID="rdbFail" runat="server" Text="Fail" GroupName="a" CssClass="radio" />
                                                        <asp:RadioButton ID="rdbSendToStore" runat="server" Text="Faulty For Reclassification" GroupName="a" CssClass="radio" />
                                                        <asp:RadioButton ID="rdbPassCore" runat="server" Text="All test OK expt Iron loss" GroupName="a" CssClass="radio" />
                                                    </span>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Height="40px" TextMode="MultiLine" Style="resize: none" MaxLength="250" onkeyup="return ValidateTextlimit(this,250);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Upload Documetnt">
                                            <ItemTemplate>
                                                <asp:FileUpload ID="fupdDoc" runat="server" AllowMultiple="False" Width="180px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="Repair Details Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltransid" runat="server" Text='<%# Bind("RSD_ID") %>'></asp:Label>
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
                            <!-- END SAMPLE FORM PORTLET-->
                        </div>
                    </div>
                    <!-- END PAGE CONTENT-->
                </div>
            </div>

        </div>
    </div>
</asp:Content>

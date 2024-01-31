<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="OilTesting.aspx.cs" Inherits="IIITS.DTLMS.OilFlow.OilTesting" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript">
 
function preventMultipleSubmissions() {
  $('#<%=cmdSave.ClientID %>').prop('disabled', true);
}
 
window.onbeforeunload = preventMultipleSubmissions;
 
</script>
       <script type="text/javascript">
        $(function () {
            $('input').keypress(function (event) {
                var $this = $(this);
                if ((event.which != 46 || $this.val().indexOf('.') != -1) && ((event.which <48 || event.which > 57) && (event.which != 0 && event.which != 8))) {
                    event.preventDefault();
                }
                var text = $(this).val();
                if (text.length === 18) {
                    $(this).val(text + ".")
                }
                if ((event.which == 46) && (text.indexOf('.') == -1)) {
                    setTimeout(function () {
                        if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                            $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                        }
                    }, 1);
                }
                if ((text.indexOf('.') == 18 && text.substring(text.indexOf('.')).length > 4)) {
                    event.preventDefault();
                }
                if (((text.indexOf('.') != -1) && (text.substring(text.indexOf('.')).length > 4) && (event.which != 0 && event.which != 8) && ($(this)[0].selectionStart >= text.length - 2))) {
                    event.preventDefault();
                }
            });
        });
    </script>
    <script src="../Scripts/functions.js" type="text/javascript"></script>
     
        function ValidateSave() {

<%--            if (document.getElementById('<%= cmbTestedBy.ClientID %>').value == "--Select--") {
                alert('Select Tested By')
                document.getElementById('<%= cmbTestedBy.ClientID %>').focus()
                return false
            }--%>
            if (document.getElementById('<%= txtTestedOn.ClientID %>').value.trim() == "") {
                alert('Enter valid Tested On')
                document.getElementById('<%= txtTestedOn.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= cmbTestLocation.ClientID %>').value == "--Select--") {
                alert('Select Testing Location')
                document.getElementById('<%= cmbTestLocation.ClientID %>').focus()
                return false
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



    <script type="text/javascript">
<%--        $(document).ready(function () {
            debugger;
            var  temp  =  "<%= hiddenTestedOn.Text %>" ;
         

            $("#<%= txtTestedOn.ClientID %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: 0,
                minDate: new Date(temp),
               

            })
        } 
        
       
        );--%>

        function onlynumbers(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode < 46 || charCode > 57) {

                return false;
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
                    <h3 class="page-title">Oil Inspection at Repair Center          
                                      
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
<%--                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='TestPendingSearch.aspx'; return false;"
                        CssClass="btn btn-primary" />
                </div>--%>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Oil Inspection</h4>
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
                                                <label class="control-label">Division</label>
                                                <div class="controls">
                                                    <div class="input-append">                          
                                                        <asp:TextBox ID="txtDivision" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Width="20px" Visible="false" ></asp:TextBox>     
                                                       <asp:TextBox ID="txtApproveId" runat="server" MaxLength="10" Visible="false" Width="20px" ></asp:TextBox>
                                                          <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Purchase Order No.</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <%--<asp:TextBox ID="txtWoNo" runat="server" MaxLength="25" Visible="false"></asp:TextBox> --%>
                                                             <asp:HiddenField ID="hdfRejectApproveRef" runat="server" />
                                                               <asp:HiddenField ID="hdfWFOId" runat="server" />
                                                               <asp:HiddenField ID="hdfWFDataId" runat="server" />
                                                               <asp:HiddenField ID="hdfApproveStatus" runat="server" />
                                                           <asp:TextBox ID="txtSelectedDetailsId" runat="server" MaxLength="10" Visible="false" Width="20px"> </asp:TextBox>
                                                         <asp:TextBox ID="txtPO_Remarks" runat="server" MaxLength="25" TextMode="MultiLine" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtPONo" runat="server" MaxLength="25" Enabled="false" ></asp:TextBox>
                                                         <asp:TextBox ID="txtWOno" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                         <asp:TextBox ID="txtComment" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                        

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Purchase Order Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtpodate" runat="server" MaxLength="25"></asp:TextBox>  
                                                    </div>
                                                </div>
                                            </div>
                                                <div class="control-group"  runat="server">
                                                <label class="control-label">Upload Inspection Report<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupTestDocument" runat="server" AllowMultiple="False" />
                                                    </div>
                                                </div>
                                            </div>
                                               <asp:Label ID="lblInspqty" runat="server" ForeColor="Green" Font-Size="Medium"></asp:Label> 

                                        </div>

                                        <div class="span5">

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
                                            
                                         <div class="control-group">
                                                <label class="control-label">
                                                    Tested By<span class="Mandotary"> *</span></label>
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
                                                        <asp:TextBox ID="txtTestedOn" runat="server" autocomplete="off" MaxLength="25"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtTestedOn" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                        <asp:TextBox ID="TextBox1" runat="server" MaxLength="10" Visible="false" Width="20px"> </asp:TextBox>
                                                        <asp:TextBox ID="hiddenTestedOn" runat="server" MaxLength="10" Visible="false" Width="20px"> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" style="display: none" runat="server">
                                                <label class="control-label">Upload<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="False" />
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
                                                CssClass="btn btn-danger" OnClick="cmdReset_Click" />
                                        </div>
                                        <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:HiddenField ID="hdfResult" runat="server" />
                                        <asp:HiddenField ID="hdfRemarks" runat="server" />
                                        <asp:HiddenField ID="hdfpendingqty" runat="server" />
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



                                        <asp:TemplateField AccessibleHeaderText="OSD_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPONO" runat="server" Text='<%# Bind("OSD_ID") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="OSD_PO_NO" HeaderText="PO No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPo" runat="server" Text='<%# Bind("OSD_PO_NO") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="OSD_PO_DATE" HeaderText="PO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("OSD_PO_DATE") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText="Oil Quantity(in Kltr)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' Style="word-break: break-all;" Width="40px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

<%--                                        <asp:TemplateField HeaderText="Testing Result">
                                            <ItemTemplate>
                                                <div style="float: right; width: 300px; margin-left: 35px;">
                                                    <span class="span5">
                                                        <asp:RadioButton ID="rdbPass" runat="server" Text="Reclaimed Oil" GroupName="a" CssClass="radio" />
                                                        <asp:RadioButton ID="rdbFail" runat="server" Text="Bad Oil" GroupName="a" CssClass="radio" />
                                                    </span>

                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Inspection Quantity(in Kltr)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInspection" runat="server" Height="20px" Width="150"  Style="resize: none" MaxLength="10" onkeypress="return onlynumbers(event)"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Height="20px" Width="150" TextMode="MultiLine" Style="resize: none" MaxLength="200" onkeyup="return ValidateTextlimit(this,200);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remove" Visible="false">
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
                            <%--                 </div>--%>
                            <!-- END SAMPLE FORM PORTLET-->
                        </div>
                    </div>
                    <!-- END PAGE CONTENT-->
                </div>
            </div>

        </div>
    </div>
</asp:Content>

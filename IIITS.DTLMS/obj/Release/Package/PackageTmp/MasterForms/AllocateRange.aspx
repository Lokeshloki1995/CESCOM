<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="AllocateRange.aspx.cs" Inherits="IIITS.DTLMS.Internal.AllocateRange" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="../Scripts/functions.js" type="text/javascript"></script>
      
 <script type="text/javascript">
     function ValidateMyForm() {
         if (document.getElementById('<%= cmbMake.ClientID %>').value == "-Select-") 
         {
             alert('Please select Vendor')
             document.getElementById('<%= cmbMake.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtStartRange.ClientID %>').value.trim() == "") {
             alert('Please select Start Range')
             document.getElementById('<%= txtStartRange.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtEndRange.ClientID %>').value.trim() == "") {
             alert('Please select End Range')
             document.getElementById('<%= txtEndRange.ClientID %>').focus()
             return false
         }
     }
     // date part  
     $(document).ready(function () {

          $("#<%=txtDWADate.ClientID%>").datepicker(
           {
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               maxDate: 0,
           })

          $("#<%=txtPODate.ClientID%>").datepicker(
           {
               dateFormat: 'dd/mm/yy',
               changeMonth: true,
               changeYear: true,
               maxDate: 0,
           })

     })


     //Remove Special charactes and Charactes to search DistrictCode
     function onlyNumbers(event) {
         var charCode = (event.which) ? event.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57))
             return false;

         return true;
     }
     //Remove Special charactes and Charactes to paste DistrictCode
     function cleanCharAndSpecial(t) {
         t.value = t.value.toString().replace(/[^0-9]+/g, '');
     }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span8">
                <h3 class="page-title">
                    SS PLATE RANGE ALLOCATION
                </h3>
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
          <%--  <div style="float: right; margin-top: 20px; margin-right: 12px">
                <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href=..Dashboard.aspx'; return false;"
                    CssClass="btn btn-primary" />
            </div>--%>
        </div>
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i> SS PLATE RANGE ALLOCATION</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">
                                                Select Vendor<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="cmbMake" runat="server" TabIndex="5">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                DWA Number</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDwaNumber"  runat="server" MaxLength="50"
                                                        TabIndex="12"></asp:TextBox><br />
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label">
                                                DWA Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                             <asp:TextBox ID="txtDWADate" runat="server" MaxLength="10" TabIndex="5" autocomplete="off"></asp:TextBox>
<br />
                                                </div>
                                            </div>
                                        </div>

                                         <div class="control-group">
                                            <label class="control-label">
                                                PO Number</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtPONumber" runat="server" MaxLength="50"
                                                        TabIndex="13"></asp:TextBox><br />
                                                </div>
                                            </div>
                                        </div>


                                         <div class="control-group">
                                            <label class="control-label">
                                                PO Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtPODate" runat="server" MaxLength="10" TabIndex="5" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                         
                                        <div class="control-group">
                                            <asp:Label ID="lblRange_count" runat="server" Text=""></asp:Label>
                                           
                                        </div>
                                    </div>
                                    <div class="span5">
                                       <%-- <div class="control-group">
                                            <label class="control-label">
                                                Enter Quantity</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtQty" runat="server" MaxLength="15" TabIndex="12"></asp:TextBox><br />
                                                </div>
                                            </div>
                                        </div>--%>

                                        <div class="control-group">
                                            <label class="control-label">
                                                Quantity</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtcapacity" runat="server" MaxLength="10"  TabIndex="5"  AutoPostBack="True" OnTextChanged="txtcapacity_SelectedIndexChanged" onkeypress="javascript:return AllowNumber(this,event);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        

                                       <div class="control-group">
                                            <label class="control-label">
                                               Start Range<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtStartRange" runat="server" MaxLength="06"
                                                        TabIndex="12" onkeypress="return onlyNumbers(event)" onchange = "cleanCharAndSpecial(this)"></asp:TextBox><br />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">
                                                End Range <span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtEndRange" runat="server" MaxLength="06" TabIndex="12" onkeypress="return onlyNumbers(event)" onchange = "cleanCharAndSpecial(this)"></asp:TextBox><br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                   
                                     
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="widget-body">
            <div class="text-center" align="center">
               
            
                    <asp:Button ID="cmdSubmit" runat="server" Text="Allocate Range" CssClass="btn btn-primary"
                        TabIndex="53" OnClientClick="javascript:return ValidateMyForm()" OnClick="cmdSubmit_Click" />
             
               
                    <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                        TabIndex="54" Width="105px" OnClick="cmdReset_Click" /><br />
              
               
             <br />
                    <asp:Button ID="cmdLoad" runat="server" Text="Load Plate Allocation" CssClass="btn btn-success"
                        TabIndex="54" Width="203px" OnClick="cmdLoad_Click" /><br />
               
                <div class="span7">
                </div>
                <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <asp:GridView ID="GridAllocateRange" runat="server" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
            AllowPaging="true" PageSize="10" ShowFooter="true" AutoGenerateColumns="false" onpageindexchanging="grdAllocateRange_PageIndexChanging" OnRowCommand ="grdAllocateRange_RowCommand">
            <Columns>
                <asp:TemplateField AccessibleHeaderText="TCPM_ID" HeaderText="Id" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRangeId" runat="server" Text='<%# Bind("TCPM_ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField AccessibleHeaderText="make name" HeaderText="VENDOR NAME">
                    <ItemTemplate>
                        <asp:Label ID="lblVendorName" runat="server" Text='<%# Bind("VM_NAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
              
                <asp:TemplateField AccessibleHeaderText="PO Number" HeaderText="PO NUMBER">
                    <ItemTemplate>
                        <asp:Label ID="lblPONUmber" runat="server" Text='<%# Bind("TCPM_PO_NO") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="PO Date" HeaderText="PO DATE">
                    <ItemTemplate>
                        <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("TCPM_PO_DATE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="DWA Number" HeaderText="DWA NUMBER">
                    <ItemTemplate>
                        <asp:Label ID="lblDWAnumber" runat="server" Text='<%# Bind("TCPM_DWA_NO") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="DWA Date" HeaderText="DWA DATE">
                    <ItemTemplate>
                        <asp:Label ID="lblDWAdate" runat="server" Text='<%# Bind("TCPM_DWA_DATE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>




              
                <asp:TemplateField AccessibleHeaderText="Start Range" HeaderText="START RANGE">
                    <ItemTemplate>
                        <asp:Label ID="lblStartRange" runat="server" Text='<%# Bind("TCPM_START_RANGE") %>'></asp:Label>
                    </ItemTemplate>
                     <FooterTemplate>
                        <asp:TextBox ID="txtPlateNumber" runat="server" placeholder="Enter Plate Number" onkeypress="return onlyNumbers(event)" onchange = "cleanCharAndSpecial(this)"></asp:TextBox>
                     </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField AccessibleHeaderText="End Range" HeaderText="END RANGE">
                    <ItemTemplate>
                        <asp:Label ID="lblEndRange" runat="server" Text='<%# Bind("TCPM_END_RANGE") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                         <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                    </FooterTemplate>
                </asp:TemplateField>
                 <asp:TemplateField AccessibleHeaderText="QUANTITY" HeaderText="QUANTITY">
                    <ItemTemplate>
                        <asp:Label ID="lblquaty" runat="server" Text='<%# Bind("TCPM_QUANTITY") %>'></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>
                <asp:TemplateField AccessibleHeaderText="Created on" HeaderText="CREATED ON">
                    <ItemTemplate>
                        <asp:Label ID="lblCron" runat="server" Text='<%# Bind("TCPM_CRON") %>'></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

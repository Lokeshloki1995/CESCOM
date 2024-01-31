<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DeliverOil.aspx.cs" Inherits="IIITS.DTLMS.OilFlow.DeliverOil" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type ="text/javascript">


        function ValidateForm() {
            if (document.getElementById('<%= txtRVNo.ClientID %>').value.length != "11") {
                alert('RV number should be equal to 11 digits')
                 document.getElementById('<%= txtRVNo.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtChallenNo.ClientID %>').value == "")
            {
                alert('Please enter Challen Number ')
                  document.getElementById('<%= txtChallenNo.ClientID %>').focus()
                return false
            }
            debugger;
            if (document.getElementById('<%= cmbVerifiedBy.ClientID %>').value == "--Select--")
            {
                alert('Please enter Deliver Tested By ')
                 document.getElementById('<%= cmbVerifiedBy.ClientID %>').focus()
                return false
            }
        }

        function ValidateRVNumber() {
            if (document.getElementById('<%= txtRVNo.ClientID %>').value.length != "11") {
                alert('RV number should be equal to 11 digits')
                return false
            }
        }
        //OMNUM allow to search chars, nums and -/
        function characterAndnumbersOm(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }

        </script>
<%--        <script type="text/javascript">
        $(document).ready(function () {
            debugger;
            var  temp  =  "<%= txtRVDate.Text %>" ;
         

            $("#<%= txtRVDate.ClientID %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                maxDate: 0,
                minDate: new Date(temp),
               

            })
        } 
        
       
        );
    </script>--%>
        <script language="Javascript" type="text/javascript">
        function OnlyNumberHyphen(e, t) {
            var text = document.getElementById("txtChallenNo").value;
            var regx = /^[0-9]\d{9}$/;
            if (regx.test(text))
                alert("valid");
            else
                alert("invalid");
        }
    </script>


    <style type="text/css">
        .handPointer {
            cursor: pointer;
        }

        .blockpointer {
            cursor: not-allowed;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <h3 class="page-title">Receive Oil
                    </h3>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text" />
                                    <button class="btn" type="button">
                                        <i class="icon-search"></i>
                                    </button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
<%--                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClientClick="javascript:window.location.href='DeliverPendingSearch.aspx'; return false;"
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
                            <h4>
                                <i class="icon-reorder"></i>Receive Oil</h4>
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
                                                <label class="control-label">Division</label>
                                                <div class="controls">
                                                    <div class="input-append">                          
                                                        <asp:TextBox ID="txtDivision" runat="server" MaxLength="25" Enabled="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" MaxLength="50" Width="20px" Visible="false" ></asp:TextBox>     
                                                       <asp:TextBox ID="txtApproveId" runat="server" MaxLength="10" Visible="false" Width="20px" ></asp:TextBox>
                                                       <asp:TextBox ID="txtSelectedDetailsId" runat="server" MaxLength="10" Visible="false" Width="20px"> </asp:TextBox>
                                                          <asp:HiddenField ID="hdfWFOAutoId" runat="server" />
                                                        <asp:HiddenField ID="hdfagency" runat="server" />
                                                           <asp:HiddenField ID="hdfosdid" runat="server" />
                                                         <asp:HiddenField ID="hdfpercentagevalue" runat="server" />
                                                    </div>
                                                </div>
                                            </div>


                                         <div class="control-group">
                                                <label class="control-label">
                                                    Tested By<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbVerifiedBy" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                           <div class="control-group">
                                                <label class="control-label">Item Code.<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                 <asp:DropDownList runat="server" ID="cmbItemCode"></asp:DropDownList>
                                            </div>
                                          </div>

                                             <div class="control-group" >
                                                <label class="control-label">
                                                   Inspected On<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtinspectedon"  runat="server"></asp:TextBox>
                                                    </div>

                                                </div>
                                            </div>


                                        </div>
                                        <div class="span5">
  
                                            <div class="control-group">
                                                <label class="control-label">
                                                    Deliver Challen No.<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtChallenNo" runat="server" MaxLength="30"  onkeypress="javascript:return OnlyNumberHyphen(this,event);"></asp:TextBox>                                       
                                                         <asp:TextBox ID="txtPONo" runat="server" MaxLength="30" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtRepairMasterId" runat="server" MaxLength="10" Visible="false"
                                                            Width="20px"> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            
                                            <div class="control-group" runat="server" ID="DivDeliverDate">
                                                <label class="control-label">
                                                    Deliver Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="DropDownList2" runat="server" Enabled="false" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtDeliverDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1" TargetControlID="txtDeliverDate"
                                                            Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" ID="divOmDate" visible="false">
                                                <label class="control-label">
                                                    OM Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="DropDownList1" runat="server" Enabled="false" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtOMDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="OMDateCalender" runat="server" CssClass="cal_Theme1" TargetControlID="txtOMDate"
                                                            Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                                <div class="control-group" id="dvinvoice" runat="server" style="display:none">
                                  
                                                <label class="control-label">
                                                   DTLMS Auto RV No<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRVNo" ToolTip="This RV number wont be shown in MMS Report" runat="server" MaxLength="11" onblur ="javascript: return ValidateRVNumber()"></asp:TextBox>
                                                    </div>
                                                    <div class="input-append">
                                                        <label style="color:red"> Since its DTLMS Auto RV Number it won't reflect in MMS Report  </label>
                                                        </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">
                                                    RV Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRVDate" runat="server" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtRVDate" Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
<%--                                                        <ajax:CalendarExtender ID="RVDateCalender" runat="server" CssClass="cal_Theme1" TargetControlID="txtRVDate"
                                                            Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="space20">
                            </div>
                            <!-- END FORM-->
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-reorder"></i>Receive Pending Oil</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
<%--                                     style="overflow: scroll; height: 450px;" --%>
                                    <div id="divResult" runat="server">
                                        <asp:GridView ID="grdReceivePending" AutoGenerateColumns="false" PageSize="10" CssClass="table table-striped table-bordered table-advance table-hover"
                                            AllowPaging="false" runat="server" ShowHeaderWhenEmpty="True"
                                            EmptyDataText="No Records Found"
                                            OnRowDataBound="grdReceivePending_RowDataBound"
                                            OnRowCommand="grdReceivePending_RowCommand">


                                   <Columns>

                                    <asp:TemplateField AccessibleHeaderText="OSD_ID" HeaderText="Master Id" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblOsdId" runat="server" Text='<%# Bind("OSD_ID") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="OSD_PO_NO" HeaderText="Select">
                                       <ItemTemplate>
                                         <asp:CheckBox ID="chkSelect" runat="server" />
                                          </ItemTemplate>
                                    </asp:TemplateField>
                                   

                                    <asp:TemplateField AccessibleHeaderText="OSD_PO_NO" HeaderText="PO No" SortExpression="OSD_PO_NO">                                
                                         <ItemTemplate>                                       
                                            <asp:Label ID="lblPoNo" runat="server" Text='<%# Bind("OSD_PO_NO") %>' style="word-break: break-all;" width="80px"></asp:Label>                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="OSD_PO_DATE" HeaderText="PO Date" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPODate" runat="server" Text='<%# Bind("OSD_PO_DATE") %>' style="word-break: break-all;" width="125px"></asp:Label>                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="Invoice" HeaderText="Invoice" Visible="false">
                                        <ItemTemplate>
                                           <asp:Label ID="lblinvoice" runat="server" Text='<%# Bind("OSD_INVOICE_NO") %>' Style="word-break: break-all;"
                                               Width="110px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText="Total Invoice Quantity(in Kltr)">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblInvoiceQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' style="word-break: break-all;" width="60px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField AccessibleHeaderText="OSD_OFFICE_CODE" HeaderText="Location">                                       
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("OSD_OFFICE_CODE") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="STATUS" Visible="false">
                                        <ItemTemplate>
                                           <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;"
                                               Width="110px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="AIN_INSP_QTY" HeaderText="Inspection Quantity(in Kltr)">
                                        <ItemTemplate>
                                           <asp:Label ID="lblinspqty" runat="server" Text='<%# Bind("AIN_INSP_QTY") %>' Style="word-break: break-all;"
                                               Width="110px"></asp:Label>
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

<%--                <div class="form-horizontal" align="center">
                    <div class="span3"></div>
                     <div class="span3">
                     <asp:Button ID="cmdLoad" runat="server" Text="Load To Update Oil" CssClass="btn btn-success" OnClick="cmdLoad_Click"/>
                         </div>
                    <div class="span7"></div>
                </div>--%>
                                            <div  class="form-horizontal" align="right">
                                        <div>
                                        </div>
                                        <div class="span6">
                                            <asp:Button ID="cmdSave" runat="server" Text="Recieve" Visible ="true" CssClass="btn btn-success"
                                                OnClick="cmdSave_Click" OnClientClick ="Javascript:return ValidateForm()"/>
                                      
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CssClass="btn btn-danger"
                                                OnClick="cmdReset_Click"/>
                                       </div>
                                        <div class="span7">
                                        </div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible ="false"></asp:Label>
                           </div>

          <%--      <div class="row-fluid">--%>
                           <div class="row-fluid" runat="server" id="dvReceive" style="display:none">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-reorder"></i>Receive Updated Item Code of Oil</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <div id="div1" style="overflow: scroll; height: 450px;" runat="server">
                                        <asp:GridView ID="grdUpdatedTcItemCode" AutoGenerateColumns="false"  CssClass="table table-striped table-bordered table-advance table-hover"
                                            AllowPaging="false" runat="server" ShowHeaderWhenEmpty="True"
                                            EmptyDataText="No Records Found"
                                            OnRowDataBound="grdReceivePending_RowDataBound"
                                            OnRowCommand="grdReceivePending_RowCommand">
                                            <Columns>
                                                <asp:TemplateField AccessibleHeaderText="OSD_ID" HeaderText="Repair Details Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOsdId" runat="server" Text='<%# Bind("OSD_ID") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                             

                                                <asp:TemplateField AccessibleHeaderText="OSD_PO_NO" HeaderText="PO NO">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPONO" runat="server" Text='<%# Bind("OSD_PO_NO") %>' Style="word-break: break-all;"
                                                            Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="OSD_PO_DATE" HeaderText="PO Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPoDate" runat="server" Text='<%# Bind("OSD_PO_DATE") %>' Style="word-break: break-all;"
                                                            Width="130px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText=" Invoice Quantity(in Kltr)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' Style="word-break: break-all;"
                                                            Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                
                                                <asp:TemplateField AccessibleHeaderText="OSD_INVOICE_NO" HeaderText="Invoice No" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoice" runat="server" Text='<%# Bind("OSD_INVOICE_NO") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="AIN_INSP_QTY" HeaderText="Insp Qty">
                                        <ItemTemplate>
                                           <asp:Label ID="lblinspqty" runat="server" Text='<%# Bind("AIN_INSP_QTY") %>' Style="word-break: break-all;"
                                               Width="110px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Received Quantity" Visible="false">
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="txtQuantity" runat="server" Height="40px" Width="250"  TextMode="MultiLine" Style="resize: none" MaxLength="3" onkeyup="return ValidateTextlimit(this,10,2);"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtQuantity" runat="server" Height="40px" Width="150"  Style="resize: none" MaxLength="10"  onkeypress="javascript:return OnlyNumberHyphen(this,event);"></asp:TextBox>
                                                        <br />
         <%--                                              <div class="input-append">
                                                        <label style="color:red"> Received Quantity Should Be Less Than Or Equals To Invoice Quantity </label>
                                                       </div>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="OSD_OFFICE_CODE" HeaderText="Location">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOfficeCode" runat="server" Text='<%# Bind("OSD_OFFICE_CODE") %>' Style="word-break: break-all;"
                                                            Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                               <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Status" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break: break-all;"
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                               </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="OSD_STATUS" HeaderText="STATUS" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblOsdStatus" runat="server" Text='<%# Bind("OSD_STATUS") %>' style="width:100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                               

                                               
                                            </Columns>
                                        </asp:GridView>
                                    </div>  
                                </div>
                            </div>
                            

                        </div>

                        <div class="space20">
                                </div>


                    </div>
                </div>

                <div class="row-fluid">
                    <div class="span12">
                        <!-- BEGIN SAMPLE FORMPORTLET-->
                        <div class="widget blue">
                            <div class="widget-title">
                                <h4>
                                    <i class="icon-reorder"></i>Received Oil</h4>
                                <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                    class="icon-remove"></a></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-body form">
                                    <!-- BEGIN FORM-->
                                    <asp:GridView ID="grdRecievedDTr" AutoGenerateColumns="false" PageSize="10" CssClass="table table-striped table-bordered table-advance table-hover"
                                        AllowPaging="true" runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" onpageindexchanging="grdTestPending_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField AccessibleHeaderText="RSD_ID" HeaderText="Repair Details Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOSDId" runat="server" Text='<%# Bind("OSD_ID") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="OSD_PO_NO" HeaderText="PO No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPONO" runat="server" Text='<%# Bind("OSD_PO_NO") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="OSD_PO_DATE" HeaderText="PO Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPodate" runat="server" Text='<%# Bind("OSD_PO_DATE") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="OSD_QUANTITY" HeaderText="Oil Quantity(in Kltr)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("OSD_QUANTITY") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="OSD_OFFICE_CODE" HeaderText="Location">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOffieCode" runat="server" Text='<%# Bind("OSD_OFFICE_CODE") %>' Style="word-break: break-all;"
                                                        Width="150px"></asp:Label>
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
            <!-- END PAGE CONTENT-->
        </div>
    </div>
    
    
   
</asp:Content>

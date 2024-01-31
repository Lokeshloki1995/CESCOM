<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcTracker.aspx.cs" Inherits="IIITS.DTLMS.Transaction.TcTracker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript" ></script>
     <script  type="text/javascript">

         function ValidateMyForm() {
             if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
                 alert('Select Valid DTr Code')
                 document.getElementById('<%= txtTcCode.ClientID %>').focus()
                 return false
             }
         }
         //Dtr allow to enter nums 
         function onlyNumbers(event) {
             var charCode = (event.which) ? event.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                 return false;

             return true;
         }

         // Dtr allow only Numbers to paste
         function cleanSpecialAndChar(t) {
             debugger;
             t.value = t.value.toString().replace(/[^0-9\n\r]+/g, '');
             //alert(" Special charactes and characters are not allowed!");
         }
         //From/Todate allow to enter nums 
         function onlyNumbersAndSlash(event) {
             var charCode = (event.which) ? event.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 47)
                 return false;

             return true;
         }

         //From/Todate  allow only Numbers to paste
         function cleanSpecialAndChars(t) {
             debugger;
             t.value = t.value.toString().replace(/[^/0-9\n\r]+/g, '');
             //alert(" Special charactes and characters are not allowed!");
         }
    </script>
     <style type="text/css">
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
                   DTR Tracker
                </h3>
                    <a style="margin-right:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
                <ul class="breadcrumb" style="display:none">
                       
                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text"/>
                                <button class="btn" type="button"><i class="icon-search"></i> </button>
                            </div>
                        </form>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
            <%-- <div style="float:right;margin-top:20px;margin-right:12px" >
                    <asp:Button ID="cmdClose" runat="server" Text="Close"  OnClientClick="javascript:window.location.href='FaultTCSearch.aspx'; return false;"
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
                        <h4><i class="icon-reorder"></i>DTR Tracker</h4>
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
                               <%-- <div class="span1"></div>--%>
                               <div class="span5">
                                  <div class="control-group">

                                    <label class="control-label" >DTr Code<span class="Mandotary"> *</span></label>
                                        <div class="controls">
                                            <div class="input-append">
                                            <asp:TextBox ID="txtTcCode" runat="server" MaxLength="6" onkeypress="return onlyNumbers(event)" onchange = "return cleanSpecialAndChar(this)"></asp:TextBox>
                                             <asp:Button ID="btnSearchId" Text="S" class="btn btn-primary" runat="server" 
                                                   /><br />
                                          <asp:LinkButton ID="lnkDTrDetails" runat="server"  
                                                style="font-size:12px;color:Blue" onclick="lnkDTrDetails_Click" OnClientClick="javascript:return ValidateMyForm()"
                                                        >View DTr Details</asp:LinkButton>
                                            </div>
                                       </div>
                                     </div>
                                   <div class="control-group">
                                         <label class="control-label"  >From Date</label>
                                    <div class="controls">
                                        <div class="input-append">
                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" onkeypress="return onlyNumbersAndSlash(event)" onchange = "return cleanSpecialAndChars(this)"></asp:TextBox>
                                         <asp:CalendarExtender ID="txtFromDate_CalendarExtender1" runat="server" CssClass="cal_Theme1" 
                                                           TargetControlID="txtFromDate"  Format="dd/MM/yyyy"></asp:CalendarExtender>    
                                        </div>
                                   </div>
                               </div>
                               </div>  
                               <div class="span5">
                                    <div class="control-group">

                                <label class="control-label">Type</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmbType" runat="server" >
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Commissioning"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Failure"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Dispatch"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Receive"></asp:ListItem>
                                              <asp:ListItem Value="2" Text="RI"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                   </div>
                                   </div>
                                   <div class="control-group">
                                <label class="control-label" >To Date</label>
                                    <div class="controls">
                                        <div class="input-append">
                                        <asp:TextBox ID="txtToDate" runat="server"  MaxLength="10" onkeypress="return onlyNumbersAndSlash(event)" onchange = "return cleanSpecialAndChars(this)"></asp:TextBox>
                                         <asp:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1" 
                                                           TargetControlID="txtToDate"  Format="dd/MM/yyyy"></asp:CalendarExtender>  
                                        </div>
                                   </div>
                                   </div>
               
                               <div class="span4">
                                    <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary" 
                                        onclick="cmdLoad_Click" Width="116px" />
                                </div> 
                                   <div class="span5">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                          OnClick="Export_clickTCtracker"  OnClientClick="javascript:return ValidateMyForm()" /><br />
                                          </div>
                               </div>            
                                    </div>
                               </div>
                               </div>
                              </div>
                            </div>
              </div>                                   
            </div>
               <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>DTR Basic Details</h4>
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
                               <%-- <div class="span1"></div>--%>
                               <div class="span12">

                                   <asp:Label ID="lbl1"  runat="server" Text="DTr Sl No"></asp:Label> &nbsp&nbsp&nbsp&nbsp
                                   <asp:TextBox ID="txtTcSlNo" runat="server" ReadOnly="true"></asp:TextBox> &nbsp&nbsp&nbsp&nbsp 
                                   <asp:Label ID="lbl2" runat="server" Text="DTr Make"></asp:Label> &nbsp&nbsp&nbsp&nbsp 
                                   <asp:TextBox ID="txtTcMake" runat="server" ReadOnly="true"></asp:TextBox> &nbsp&nbsp&nbsp&nbsp 
                                   <asp:Label ID="lbl3" runat="server" Text="Capacity(in KVA)"></asp:Label> &nbsp&nbsp&nbsp&nbsp 
                                   <asp:TextBox ID="txtCapacity" runat="server" ReadOnly="true"></asp:TextBox><br />
                                   <div class="space20"></div>
                                    <asp:Label ID="lblRepCount" runat="server" Text="Repair Count"></asp:Label> &nbsp&nbsp&nbsp&nbsp 
                                   <asp:TextBox ID="txtRepairCount" runat="server" ReadOnly="true" Width="188px"></asp:TextBox>

                                 <%--   <div class="control-group">

            <label class="control-label" >Tc Sl No:</label>
                <div class="controls">
                    <div class="input-append">
                        <asp:TextBox ID="txtTcSlNo" runat="server" Enabled="false"></asp:TextBox>
                    </div>
               </div>
               </div>--%>
                </div>
            <%--    <div class="span4">
               <div class="control-group">

            <label class="control-label" >Tc Make:</label>
                <div class="controls">
                    <div class="input-append">
                       <asp:TextBox ID="txtTcMake" runat="server" Enabled="false"></asp:TextBox>
                          <asp:TextBox ID="txtDtcId" runat="server" Visible="false"></asp:TextBox>
                    </div>
               </div>
               </div>
               <div class="control-group">

            <label class="control-label">Capacity</label>
                <div class="controls">
                    <div class="input-append">
                       <asp:TextBox ID="txtCapacity" runat="server" Enabled="false"></asp:TextBox>
                    </div>
               </div>
               </div>

                </div> --%>
                <div class="space20"></div>
                <asp:GridView ID="grdTcDetails" 
                                AutoGenerateColumns="false" 
                                ShowHeaderWhenEmpty="true"  EmptyDataText="No records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"  
                                runat="server" onpageindexchanging="grdTcDetails_PageIndexChanging" 
                                        onrowcommand="grdTcDetails_RowCommand"  OnSorting="grdTcDetails_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" />
                                <Columns>

                                <asp:TemplateField AccessibleHeaderText="DRT_ACT_REFTYPE" HeaderText="Ref Type" Visible="false">                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblRefType" runat="server" Text='<%# Bind("DRT_ACT_REFTYPE") %>' ></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="DRT_DTR_STATUS" HeaderText="DTR Status" Visible="false">                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblDTrStatus" runat="server" Text='<%# Bind("DRT_DTR_STATUS") %>' ></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>


                                   <asp:TemplateField AccessibleHeaderText="DRT_ACT_REFNO" HeaderText="Ref No" Visible="false">                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblRefNo" runat="server" Text='<%# Bind("DRT_ACT_REFNO") %>' ></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>


                                    <asp:TemplateField AccessibleHeaderText="DRT_DTR_CODE" HeaderText="DTR CODE" ItemStyle-Width="100px" >                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblDtrCode" runat="server" Text='<%# Bind("DRT_DTR_CODE") %>' ></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="DATE" HeaderText="Transaction Date"  ItemStyle-Width="200px" >                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblTransDate" runat="server" Text='<%# Bind("TRANSDATE") %>' Style="word-break:break-all" width="200px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Entry DATE" HeaderText="Entry Date"  ItemStyle-Width="200px" >                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblEntryDate" runat="server" Text='<%# Bind("DRT_ENTRYDATE") %>' Style="word-break:break-all" width="200px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location" SortExpression="LOCATION"  ItemStyle-Width="250px">                                
                                        <ItemTemplate> 
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("LOCATION") %>' Style="word-break:break-all" Width="250px"></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>
                                     <asp:TemplateField AccessibleHeaderText="STATUS" HeaderText="Status" SortExpression="STATUS" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Style="word-break:break-all"  width="500px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <center>
                                         <asp:ImageButton  ID="imgView" runat="server" Height="15px" ImageUrl="~/img/Manual/view.png" 
                                                Width="15px" CommandName="View"  />
                                        </center>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <center>
                                             <asp:Label ID="lblHead" runat="server" Text="View Details" ></asp:Label>
                                        </center>
                                    </HeaderTemplate>
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
            </div>
           
             <asp:Label ID="lblMessage" runat="server" ForeColor="Red" ></asp:Label>
           <div class="span3"></div>
          <asp:Label ID="lblTcDetails" runat="server" ForeColor="Blue"></asp:Label> 

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
                   This Web Page Can Be Used To Track DTR </li>
                      <li> User Can Enter Or Search Dtr Code in Dtr Code TextBox</li>
                  <li>    After Entering Dtr Code Click On Load Button To Get The Details
                       
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

     </div>
</asp:Content>

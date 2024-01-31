<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="NewTcMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.NewTcMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
  <script  type="text/javascript">
      function ValidateSave() {
          if (document.getElementById('<%= txtPONo.ClientID %>').value.trim() == "") {
              alert('Enter valid PO Number')
              document.getElementById('<%= txtPONo.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtPurchaseDate.ClientID %>').value.trim() == "") {
              alert('Select valid Purchasing Date')
              document.getElementById('<%= txtPurchaseDate.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtQuantity.ClientID %>').value.trim() == "") {
              alert('Enter valid Quantity')
              document.getElementById('<%= txtQuantity.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbSupplier.ClientID %>').value.trim() == "") {
              alert('Select Supplier Name')
              document.getElementById('<%= cmbSupplier.ClientID %>').focus()
              return false
          }

      }
      function ValidateMyForm() {


          if (document.getElementById('<%= txtTcCode.ClientID %>').value.trim() == "") {
              alert('Enter DTr Code')
              document.getElementById('<%= txtTcCode.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtSerialNo.ClientID %>').value.trim() == "") {
              alert('Enter valid DTr serial No')
              document.getElementById('<%= txtSerialNo.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbMake.ClientID %>').value == "--Select--") {
              alert('Select DTr Make Name')
              document.getElementById('<%= cmbMake.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbCapacity.ClientID %>').value == "-Select-") {
                 alert('Select Capacity')
                 document.getElementById('<%= cmbCapacity.ClientID %>').focus()
                  return false
           }
              if (document.getElementById('<%= txtManufactureDate.ClientID %>').value.trim() == "") {
                   alert('Enter valid Manufacturing Date')
                  document.getElementById('<%= txtManufactureDate.ClientID %>').focus()
                  return false
              }

              if (document.getElementById('<%= txtTcLifeSpan.ClientID %>').value.trim() == "") {
                alert('Enter Life Span')
                document.getElementById('<%= txtTcLifeSpan.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtWarrentyPeriod.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Warrenty Period')
                  document.getElementById('<%= txtWarrentyPeriod.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= txtOilCapacity.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Oil Capacity')
                  document.getElementById('<%= txtOilCapacity.ClientID %>').focus()
                  return false
              }
              if (document.getElementById('<%= txtWeight.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Weight of DTr')
                  document.getElementById('<%= txtWeight.ClientID %>').focus()
                  return false
              }            
              

      }
    
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>

   <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <h3 class="page-title">
                    New DTR Inward
                   </h3>
                       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>
                               </div>
                           </form>
                       </li>
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->
               </div>

               <div style="float:right;margin-to/*p:2*/0px;margin-right:12px" >
                      <asp:Button ID="Button1" runat="server" Text="DTR View" 
                                      OnClientClick="javascript:window.location.href='TCMasterView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>

            </div>
            <!-- END PAGE HEADER-->
              
              <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Purchase Details</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                           
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
                                                <label class="control-label">Purchase Order No<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfPOId" runat="server" />
                                                         <asp:TextBox ID="txtPOId" runat="server" MaxLength="50" Width="20px" Visible="false"></asp:TextBox>
                                                            <asp:TextBox ID="txtPONo" runat="server" MaxLength="50" ></asp:TextBox>
                                                             <asp:Button ID="btnPoSearch" runat="server" Text="S" 
                                                                  CssClass="btn btn-primary" onclick="btnPoSearch_Click" /> 
                                                       
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Purchase Order Date</label>
                       
                                                <div class="controls">
                                                    <div class="input-append">                                                      
                                                         <asp:TextBox ID="txtPurchaseDate" runat="server"   MaxLength="10" 
                                                             Enabled="False" ></asp:TextBox>
                                                       <ajax:CalendarExtender ID="PurchaseCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                     TargetControlID="txtPurchaseDate"></ajax:CalendarExtender>          
                                                      
                                                    </div>
                                                </div>
                                            </div>


                               
                                    </div>
                                       
                                    <div class="span5">
                                               <div class="control-group">
                                                    <label class="control-label">Total Quantity Ordered</label>
                        
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtQuantity" runat="server"  MaxLength="8" 
                                                                onkeypress="javascript:return OnlyNumber(event);" Enabled="False" ></asp:TextBox>
                                                       
                                                        </div>
                                                    </div>
                                                </div>
                                   

                                               <div class="control-group">
                                                    <label class="control-label">Supplier Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                                    <asp:DropDownList ID="cmbSupplier" runat="server" Enabled="False"> </asp:DropDownList>       
                                               
                                                        </div>
                                                    </div>
                              
                                             </div>

                                        
                                    </div>  
                                    <div class="span1"></div>
                                                           
                                </div> 
                                
                                
                                  
                                 <div class="space20"></div>
                                 
                                 <asp:GridView ID="grdPOQuantity" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" 
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdPOQuantity_PageIndexChanging">
                                <Columns>

                                
                                   <asp:TemplateField AccessibleHeaderText="PO_ID" HeaderText="PurchaseOrderId" Visible="false" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPoId" runat="server" Text='<%# Bind("PO_ID") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="CAPACITY" HeaderText="Capacity(in KVA)">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("CAPACITY") %>'></asp:Label>                                
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="MAKE" HeaderText="Make Name">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMake" runat="server" Text='<%# Bind("MAKE") %>'></asp:Label>                                
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField AccessibleHeaderText="REQ_QNTY" HeaderText="Requested Quantity">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblReqCapacity" runat="server" Text='<%# Bind("REQ_QNTY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="PENDINGCOUNT " HeaderText="Pending Quantity">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblPendingCapacity" runat="server" Text='<%# Bind("PENDINGCOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                </Columns>

                            </asp:GridView>                
                                      <div class="space20"></div>
                                    
                                                                                 
                            </div>
                                <div class="widget-body form">
                            <div  class="form-horizontal" align="center">
                             
                                     <div class="span3"></div>
                                        <div class="span1">
                                             <asp:Button ID="cmdResetPO" runat="server" Text="Reset" 
                                             CssClass="btn btn-primary" onclick="cmdResetPO_Click"  />
                                        </div>
                                     </div>    
                           </div>         
                                                               
                        </div>
                                
                         
                            
                        <!-- END FORM-->                            
                    </div>
                 </div>
    <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
            <!-- END PAGE CONTENT-->

            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>DTr Details</h4>
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
                        <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtTcCode"  runat="server" onkeypress="javascript:return OnlyNumber(event);"  MaxLength="10"></asp:TextBox>
            
                               
                            </div>
                        </div>
                    </div>
  
                   <div class="control-group">
                        <label class="control-label">DTr Serial No <span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtSerialNo" runat="server"   MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">DTr Make<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                        <asp:DropDownList ID="cmbMake" runat="server">                             
                                             </asp:DropDownList>                     
                             
                            </div>
                        </div>
                    </div>

                     <div class="control-group">
                        <label class="control-label">Capacity(in KVA)<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                                   
                             <asp:DropDownList ID="cmbCapacity" runat="server" >
                             </asp:DropDownList>   
                                                       
                            </div>
                        </div>
                    </div>

                    
                 
           </div>
          <div class="span5">

                  <div class="control-group">
                        <label class="control-label">Manufacturing Date<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                            
                                    <asp:TextBox ID="txtManufactureDate" runat="server" MaxLength="10"></asp:TextBox>
                                      <ajax:CalendarExtender ID="ManufactureCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                     TargetControlID="txtManufactureDate"></ajax:CalendarExtender>
                                                       
                            </div>
                        </div>
                  </div>

                   <div class="control-group">
                        <label class="control-label">DTr Life Span<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtTcLifeSpan" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                        MaxLength="5" ></asp:TextBox>
                                                       
                            </div>
                        </div>

                    </div>


                   <div class="control-group">
                        <label class="control-label">Warrenty Period(in Year)<span class="Mandotary">*</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtWarrentyPeriod" runat="server" MaxLength="2" 
                                        onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                            </div>
                        </div>
                    </div>


                     <div class="control-group">
                        <label class="control-label">Oil Capacity(in Liter)<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtOilCapacity" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                        MaxLength="5" ></asp:TextBox>
                                                       
                            </div>
                        </div>

                    </div>

                    <div class="control-group">
                        <label class="control-label">Weight of DTr(in KG)<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtWeight" runat="server" onkeypress="javascript:return AllowNumber(this,event);"
                                        MaxLength="5" ></asp:TextBox>
                                                       
                            </div>
                        </div>

                    </div>
               </div>

              

               

                       <div class="span1"></div>
                           </div>
                           <div class="space20"></div>
                                        
                            <div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span1">
                                        <asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="btn btn-primary" onclick="cmdAdd_Click" 
                                              OnClientClick="javascript:return ValidateMyForm();"   />
                                         </div>
                                      <%-- <div class="span1"></div>  OnClientClick="javascript:return ResetForm();"--%>
                                     <div class="span1">  
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-primary" onclick="cmdReset_Click"  /><br />
                                    </div>
                                                <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                                    </div>
                        </div>
                    </div>
                                
                                <div class="space20"></div>
                                <!-- END FORM-->
                                   
                                   <asp:GridView ID="grdTCDetails" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" 
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" TabIndex="16" onrowcommand="grdTCDetails_RowCommand" 
                                onpageindexchanging="grdTCDetails_PageIndexChanging" >
                                <Columns>

                                
                                   <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTr Code" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCCode" runat="server" Text='<%# Bind("TC_CODE") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr SlNo" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>'></asp:Label>                                
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMakeName" runat="server" Text='<%# Bind("TM_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="MAKE_ID" HeaderText="Make ID" Visible="false" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMakeID" runat="server" Text='<%# Bind("MAKE_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField AccessibleHeaderText="TC_MANF_DATE" HeaderText="Manf. Date">                                    
                                        <ItemTemplate>                                          
                                            <asp:Label ID="lblManfDate" runat="server" Text='<%# Bind("TC_MANF_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    
                                    <asp:TemplateField AccessibleHeaderText="LIFE_SPAN" HeaderText="Life Span">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblLifeSpan" runat="server" Text='<%# Bind("LIFE_SPAN") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     
                                    <asp:TemplateField AccessibleHeaderText="TC_WARANTY_PERIOD" HeaderText="Guarantee" Visible="false">                                     
                                        <ItemTemplate>
                                            <asp:Label ID="lblWarrenty" runat="server" Text='<%# Bind("TC_WARANTY_PERIOD") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_OIL_CAPACITY" HeaderText="Oil Capacity" Visible="false">                                     
                                        <ItemTemplate>
                                            <asp:Label ID="lblOilCapacity" runat="server" Text='<%# Bind("TC_OIL_CAPACITY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_WEIGHT" HeaderText="Weight" Visible="false">                                     
                                        <ItemTemplate>
                                            <asp:Label ID="lblWeight" runat="server" Text='<%# Bind("TC_WEIGHT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                    <%--<asp:TemplateField AccessibleHeaderText="SERVICE_DATE" HeaderText="Supplier" Visible="false">                                     
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceDate" runat="server" Text='<%# Bind("SERVICE_DATE") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                  
                                   <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <center>
                                         <asp:ImageButton  ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" 
                                                Width="12px" CommandName="editT" OnClientClick="return confirm ('Are you sure, you want to Edit the Details');" />
                                            <asp:ImageButton  ID="img" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png" CommandName="remove"
                                                Width="12px" OnClientClick="return confirm ('Are you sure, you want to Remove');"/>
                                        </center>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <center>
                                             <asp:Label ID="lblHead" runat="server" Text="Action" ></asp:Label>
                                        </center>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                   
                                </Columns>

                            </asp:GridView>
                            <div class="widget-body form">
                              <div  class="form-horizontal" align="center">
                               <div class="space20"></div>
                                     <div class="span3"></div>
                                        <div class="span1">
                                             <asp:Button ID="cmdSave" runat="server" Text="Save" Visible="false"
                                              OnClientClick="javascript:return ValidateSave();" CssClass="btn btn-primary" 
                                                onclick="cmdSave_Click" />
                                        </div>
                                     </div>        
                            </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->

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
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used For New DTR Inward.
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Purchase Order No can Be Searched by Clicking On Search Button Or Purchase Order No Can Be Entered Directly to the TextBox.
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>In Dtr Details Section All The Details Regarding The DTR Can Be Entered.
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>After Filling all The Required Filed,Click Add Button To Save The DTR Details.
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

</asp:Content>

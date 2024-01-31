<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="PoMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.PoMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ConfirmDelete() {

            var result = confirm('Are you sure you want to Delete?');
            if (result)
             {
                return true;
            }
            else {

                return false;
            }
        }


        function ValidateForm() 
        {

            if (document.getElementById('<%= txtPoNumber.ClientID %>').value.trim() == "") {
                alert('Enter Po Number')
                document.getElementById('<%= txtPoNumber.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtDate.ClientID %>').value.trim() == "") {
                alert('Enter Date')
                document.getElementById('<%= txtDate.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbSupplier.ClientID %>').value.trim() == "--Select--") {
                alert('Select Supplier')
                document.getElementById('<%= cmbSupplier.ClientID %>').focus()
                return false
            }

        }

        function ValidateForm1() 
        {
            if (document.getElementById('<%= ddlMake.ClientID %>').value.trim() == "-Select-") {
                alert('Select Make')
                document.getElementById('<%= ddlMake.ClientID %>').focus()
                return false
            }

             if (document.getElementById('<%= ddlCapacity.ClientID %>').value.trim() == "--Select--") 
            {
                alert('Select Capacity')
                document.getElementById('<%= ddlCapacity.ClientID %>').focus()
                return false
            }
           
            if (document.getElementById('<%= txtQuantity.ClientID %>').value.trim() == "") {
                alert('Enter Quantity')
                document.getElementById('<%= txtQuantity.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtQuantity.ClientID %>').value.trim() == "0") {
                alert('Quantity should be Greater than 0')
                document.getElementById('<%= txtQuantity.ClientID %>').focus()
                return false
            }



        }










        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                Create Purchase Order
                <div style="float:right" >
                             
                         <asp:Button ID="cmdClose" runat="server" Text="PO View" 
                                              CssClass="btn btn-primary" onclick="cmdClose_Click"  /><br />
                 </div>
           
                   </h3>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i>
                                   
                                  
                                    </button>
                               </div>
                                
                           </form>
                          
                       </li>
                       
                                    
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->
               <%-- <div style="float:right" >
                             
                                   <asp:Button ID="cmdClose" runat="server" Text="Indent View" 
                                              CssClass="btn btn-primary" onclick="cmdClose_Click" /><br /></div>
--%>
                                      
               </div>
          <div class="row-fluid" >
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" > 
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Create Purchase Order</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            
                            </span>
                        </div>
                        <div class="widget-body">
                            <div class="widget-body form" >
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                    <div class="span1"></div>
                  <div class="span5">
                    <div class="control-group">
                        <label class="control-label">PO Number<span class="Mandotary">*</span></label>                   
                        <div class="controls">
                            <div class="input-append">
                             <asp:TextBox ID="txtPoId" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtPoNumber" runat="server" MaxLength="20"   ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                   <div class="control-group">
                        <label class="control-label">Supplier Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">                                     
                                 <asp:DropDownList ID="cmbSupplier" runat="server"> </asp:DropDownList>                                                      
                            </div>
                        </div>
                              
                  </div>
 
                  </div>           
                                 
                    <div class="span5">
                        <div class="control-group">
                            <label class="control-label">Date<span class="Mandotary">*</span></label>
                            <div class="controls">
                                <div class="input-append">                                                       
                                    <asp:TextBox ID="txtDate" runat="server"   MaxLength="10"></asp:TextBox>
                               <ajax:CalendarExtender ID="ManufactureCalender" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                    TargetControlID="txtDate"></ajax:CalendarExtender>
                                                       
                                </div>
                            </div>
                        </div>

                          <div class="control-group">
                            <label class="control-label">PO Amount</label>                   
                            <div class="controls">
                                <div class="input-append">                                
                                    <asp:TextBox ID="txtRate" runat="server" MaxLength="15"  onkeypress="javascript:return AllowNumber(this,event);" ></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>

             </div>
                                             
           </div>                  
                 <div class="space20"></div>

                              <div  class="form-horizontal" align="center">

                                   <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                             </div>
                
            </div>
                 </div>
    <!-- END SAMPLE FORM PORTLET-->
            </div>
            </div>
              
           
        
    </div>

        <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Capacity Details</h4>
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
                                            <label class="control-label">Make <span class="Mandotary">*</span></label>                   
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:DropDownList ID="ddlMake" runat="server"></asp:DropDownList>  
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">Quantity <span class="Mandotary">*</span></label>                   
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8"   onkeypress="javascript:return OnlyNumber(event);" ></asp:TextBox>
                                                </div>
                                            </div>
                                      </div>
                                    </div>
                                       
                                  <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">Capacity(in KVA) <span class="Mandotary">*</span></label>                   
                                            <div class="controls">
                                                <div class="input-append">                            
                                                    <asp:DropDownList ID="ddlCapacity" runat="server"></asp:DropDownList>                                  
                                                </div>
                                            </div>
                                        </div>    
                                        
                                          <div class="control-group">                                       
                                            <div class="controls">
                                                <div class="input-append">
                                                    <div class="span1">
                                                         <asp:Button ID="cmdAdd" runat="server" Text="Add" onclick="cmdAdd_Click"  OnClientClick="return ValidateForm1();" CssClass="btn btn-primary" />
                                                    </div>                 
                                                </div>
                                            </div>
                                          </div>                                      
                                        
                                  </div>  
                                    <div class="span1"></div>                                                           
                                </div> 
  
                                 <div class="space20"></div>
                                 
                           <asp:GridView ID="grdPoMaster" AutoGenerateColumns="false"  PageSize="5"  
                                ShowHeaderWhenEmpty="true"  EmptyDataText="No records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true" 
                                runat="server" TabIndex="16"   onrowcommand="grdPoMaster_RowCommand" onpageindexchanging="grdPoMaster_PageIndexChanging" 
                                >
                                <Columns>
                                 <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText="TC ID" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcId" runat="server" Text='<%# Bind("PO_NO") %>'></asp:Label>
                                               <asp:TextBox ID="txtTcId" runat="server" ></asp:TextBox>
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="Make" HeaderText="Make" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMakeId" runat="server" Text='<%# Bind("MAKE_ID") %>'></asp:Label>                       
                                        </ItemTemplate>
                                   </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Make" HeaderText="Make">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblMake" runat="server" Text='<%# Bind("PB_MAKE") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="SO_CAPACITY" HeaderText="Capacity">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%# Bind("PB_CAPACITY") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                   </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="SO_QNTY" HeaderText="Quantity" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("PB_QUANTITY") %>'></asp:Label>
                                  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png" 
                                              CommandName="Remove"  Width="12px" OnClientClick="return ConfirmDelete();" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                            </asp:GridView>        
                                      <div class="space20"></div>
                                    <div  class="text-center" align="center">
                                    
                                     
                                              <asp:Button ID="btnSave" runat="server" Text="Save" 
                                                     OnClientClick="return ValidateForm();" CssClass="btn btn-success" onclick="btnSave_Click1" 
                                                  />
                                       
                                             <asp:Button ID="btnReset" runat="server" Text="Reset" 
                                                 CssClass="btn btn-danger" onclick="btnReset_Click" />
                                       
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
            
    </div>
    
    </div>
   


</asp:Content>

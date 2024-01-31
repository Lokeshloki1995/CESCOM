<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TransRepairerMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.WebForm1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script src="../Scripts/functions.js" type="text/javascript"></script>
      <script  type="text/javascript">
          function ValidateMyForm() {
              if (Page_ClientValidate()) {
                  if (document.getElementById('<%= txtRepairName.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Repairer name')
                  document.getElementById('<%= txtRepairName.ClientID %>').focus()
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
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                  Master  Repairer Details
                   </h3>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="Text1" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>
                               </div>
                           </form>
                       </li>
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->
               </div>
               <%-- <div style="float:right;margin-top:20px;margin-right:12px" >
                      <asp:Button ID="cmdClose" runat="server" Text="Repairer View" 
                                      OnClientClick="javascript:window.location.href='TransRepairerMasterView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>--%>
                         
            </div>
            <!-- END PAGE HEADER-->
             <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> Master Repairer Details</h4>
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
                        <label class="control-label" >Repairer Name <span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <asp:TextBox ID="txtRepairName" runat="server" MaxLength="50" TabIndex="1" style="width:300px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                      <div class="control-group">
                        <label class="control-label">Phone Number <br /><span>(main office with STD Code)</span><span class="Mandotary"> *</span></label>
                       
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtRepairPhnNo" runat="server" 
                                        MaxLength="15"  style="width:300px"
                                        onkeypress="javascript:return OnlyNumberHyphen(this,event);" TabIndex="3"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                      <div class="control-group">
                        <label class="control-label">Email Id <span class="Mandotary"> *</span></label>
                      
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtRepairEmailId" runat="server" MaxLength="50" 
                                         CausesValidation="True" TabIndex="4" style="width:300px"></asp:TextBox>
                                      </br>
                                      <asp:RegularExpressionValidator runat="server" id="regular" controltovalidate="txtRepairEmailId" 
                             Validationexpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" 
                            errormessage="Please enter a valid email id!!!!" ForeColor="Red" 
                             Display="Dynamic" Font-Size="Small"/>   
                             
                
                            </div>
                        </div>
                    </div>
                         
                     <%-- <div class="control-group" >
                        <label class="control-label">Contact Person Name </label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtContactPerson" runat="server" MaxLength="50" TabIndex="5" 
                                        ></asp:TextBox> 
                        </div>
                        </div>
                    </div>--%>

                      <div class="control-group">
                        <label class="control-label">Fax No </label>
                     
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtFaxNo" style="width:300px" runat="server" MaxLength="20" TabIndex="9"  ></asp:TextBox>          

                        </div>
                       </div>
                    </div>
                                         
                        </div>
                        <div class="span5" >
                                
                     <div class="control-group">
                        <label class="control-label">Register Address<span class="Mandotary"> *</span></label>
                       
                        <div class="controls">
                            <div class="input-append">
                                         <asp:TextBox ID="txtRepairerId" runat="server" MaxLength="50" Visible="false" Width="20px"></asp:TextBox>              
                                <asp:TextBox ID="txtRepairAddress" runat="server" MaxLength="500" Height="60px" style="width:300px" 
                                    TextMode="MultiLine"  
                                             onkeyup="return ValidateTextlimit(this,250);" TabIndex="12" ></asp:TextBox>
                            </div>
                        </div>
                    </div>                                                

                  </div>
                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="text-center" align="center">
                                    
                                        
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" CausesValidation="false"
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-success" />
                                      
                              
                                      <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                                            onclick="cmdReset_Click"  CssClass="btn btn-danger" /><br />
                                 
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
            
             <div class="row-fluid" >
                 <div class="widget blue" >
                        <div class="widget-title" >
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
  CssClass="table table-advance table-striped table-bordered"
     

  runat="server" onpageindexchanging="grdRepairer_PageIndexChanging" onrowcommand="grdRepairer_RowCommand" 
                                    OnSorting="grdRepairer_Sorting" AllowSorting="true"  onrowdatabound="grdRepairer_RowDataBound" >
                                <HeaderStyle CssClass="both" />
     <Columns>
      <asp:TemplateField AccessibleHeaderText="TR_ID" HeaderText="Id" Visible="false">        
            <ItemTemplate>                                                 
                <asp:Label ID="lblRepairId" runat="server" Text='<%# Bind("TR_ID") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="TS_NAME" HeaderText="Repairer Name" Visible="true" SortExpression="TR_NAME" ItemStyle-Width="200">           
            <ItemTemplate>
                <asp:Label ID="lblName" runat="server" Text='<%# Bind("TR_NAME") %>' style="word-break: break-all;" width="250"></asp:Label>
            </ItemTemplate>
             <FooterTemplate>
             <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                    <asp:TextBox ID="txtRepairerName" runat="server" placeholder="Enter Repairer Name" Width="100px" ></asp:TextBox>
             </asp:Panel>
             </FooterTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="TS_ADDRESS" HeaderText="Addresss" Visible="true" ItemStyle-Width="300">          
            <ItemTemplate>
                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("TR_ADDRESS") %>' style="word-break: break-all;" width="300"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="TS_PHONE" HeaderText="Phone no" Visible="true" ItemStyle-Width="130">          
            <ItemTemplate>
                <asp:Label ID="lblphone" runat="server" Text='<%# Bind("TR_PHONE") %>' style="word-break: break-all;" width="130"></asp:Label>
            </ItemTemplate>
              <FooterTemplate>
                 <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
             </FooterTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="TS_EMAIL" HeaderText="EmailId"  Visible="true" ItemStyle-Width="300">
            
            <ItemTemplate>
                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("TR_EMAIL") %>' style="word-break: break-all;" width="300"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField AccessibleHeaderText="TS_FAX" HeaderText="Fax No"  Visible="true" ItemStyle-Width="100">
            
            <ItemTemplate>
                <asp:Label ID="lblFax" runat="server" Text='<%# Bind("TR_FAX") %>' style="word-break: break-all;" width="100"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField HeaderText="Click to Edit" ItemStyle-Width="100">
            <ItemTemplate>
                <center>
                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="25px" ImageUrl="~/Styles/images/edit64x64.png"
                        Width="25px"   CommandName="upload" />
                </center>
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText ="Status" ItemStyle-Width="100">
             <ItemTemplate>
                 <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("TR_MASTER_STATUS") %>' ></asp:Label>
                 <centre>
                       <asp:ImageButton Visible="false"  ID="imgDeactive"  runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status" 
                        tooltip="Click to Activate Master Repairer"  width="25px" />     
                     <!-- OnClientClick="return ConfirmStatus('Activate');" -->   
                        <asp:ImageButton Visible="false"  ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif"  CommandName="status" 
                        tooltip="Click to DeActivate MaaSter Repairer" width="25px"  />        
                 </centre>
             </ItemTemplate>

             </asp:TemplateField>

         </Columns>
      <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Font-Size="Medium" Width="15px"/>
      <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" Font-Bold="true" Font-Size="Medium" Width="15px" HorizontalAlign="Center" VerticalAlign="Middle"/>
     </asp:GridView>

                                     </div>
                            </div>

                            </div>
                          </div>
                </div>

             </div>

        </div>


</asp:Content>

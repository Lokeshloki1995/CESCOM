<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FormRoleMapping.aspx.cs" Inherits="IIITS.DTLMS.Approval.FormRoleMapping" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
<script  type="text/javascript">
    function ValidateMyForm() {
        

        if (document.getElementById('<%= cmbModules.ClientID %>').value == "--Select--") {
                alert('Select the Module')
                document.getElementById('<%= cmbModules.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= cmbRoles.ClientID %>').value == "--Select--") {
                alert('Select the Role Required')
                document.getElementById('<%= cmbRoles.ClientID %>').focus()
                return false
              
        }
    }
      
      
</script>

            <script type="text/javascript">

    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip(); 
    });
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
                <h3 class="page-title">
                Access Rights View                 
                                      
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
                <div class="widget blue" >
                    <div class="widget-title" >
                        <h4><i class="icon-reorder"></i>Access Rights</h4>
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
                                <div class="span4">
                                    <div class="control-group">

            <label class="control-label" style="font-size: medium; font-weight: bold;" >Module Name<span class="Mandotary"> *</span></label>
                <div class="controls">
                    <div class="input-append">
                          <div class="space0"></div>   
                                <asp:DropDownList ID="cmbModules" runat="server" AutoPostBack="true" 
                                    onselectedindexchanged="cmbModules_SelectedIndexChanged">
                                </asp:DropDownList>
                                                      
                    </div>
               </div>
              </div>
            </div>
           
            <div class="span2"></div>                     
            <div class="span4">
            <div class="control-group">
                <label class="control-label" style="font-size: medium; font-weight: bold;">Roles<span class="Mandotary"> *</span></label>
               
                    <div class="controls">
                        <div class="input-append">   
                             <div class="space0"></div>                                                         
                                <asp:DropDownList ID="cmbRoles" runat="server" AutoPostBack="true" onselectedindexchanged="cmbRoles_SelectedIndexChanged" 
                                > </asp:DropDownList>    
                        </div>
                    </div>
                </div>
                </div>
                    <div class="space5"></div>
                <div class="space5"></div>
                    <div  class="form-horizontal" align="center">
                <div class="span7"></div>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>  
                                        
        </div>                                   
    </div>                                                    
         </div>
                                                
      
        
                    <div align="center">
            <asp:GridView ID="grdAccessRights" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                runat="server" ShowHeaderWhenEmpty="True" DataKeyNames="BO_ID" EmptyDataText="No Records Found"
                PageSize="20"   AllowPaging="True"
               width="70%" onrowdatabound="grdAccessRights_RowDataBound"   >
                <Columns>
                       
                    <asp:TemplateField AccessibleHeaderText="UR_ID" HeaderText="UR_ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUrId" runat="server" Text='<%# Bind("UR_ID") %>'></asp:Label>
                        </ItemTemplate>
                            <ItemStyle  Width="10%" Height="2%"/>
                    </asp:TemplateField>
                    <asp:TemplateField AccessibleHeaderText="BO_FORMNAME" HeaderText="Form Name" Visible="true">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFormName"   runat="server"  Text='<%# Bind("BO_NAME") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            
                            <asp:Label ID="lblFormName" runat="server" Text='<%# Bind("BO_NAME") %>'></asp:Label>
                        </ItemTemplate>
                                <ItemStyle Width="10%" Height="2%"/>
                    </asp:TemplateField>

                   <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>       
                       <asp:CheckBoxList ID="chklstAccess" runat="server" RepeatDirection="Horizontal" CssClass="checkbox"  AutoPostBack="true"   
                              Width="90%" align="center" OnSelectedIndexChanged="chklstAccess_OnSelectedIndexChanged">
                            </asp:CheckBoxList>
                    </ItemTemplate>
                        <ItemStyle  Width="35%"/>
                </asp:TemplateField>

        
                    <asp:TemplateField AccessibleHeaderText="BO_ID" HeaderText="BO_ID" Visible="false">
                       
                        <ItemTemplate>
                            <asp:Label ID="lblBoId" runat="server" Text='<%# Bind("BO_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            



<div class="space20"></div>

<div  class="form-horizontal" align="center">

                                        <div class="span3"></div>
                                        <div class="span1">                   
    
                                <div class="control-group">
   
<div class="controls">
    <div class="input-append">
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" 
                Text="Save"  OnClientClick="return ValidateMyForm()" 
                onclick="btnSave_Click" CausesValidation="false"/>                                       
        </div>
      
                </div>    
            </div>
            </div>

                   <div class="span1">  
            <div class="control-group">
            
   
<div class="controls">
    <div class="input-append">
            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" 
                Text="Reset"   CausesValidation="false" onclick="btnReset_Click"/>                                       
        </div>
                      </div>    
            </div>
            </div>
            <div class="space5"></div>


                                                   <div class="span7"></div>
                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                        </div>  
                                        
                                            <div class="span7"></div>
                <asp:TextBox ID="txtMappingId" runat="server" Visible="false"></asp:TextBox>
                                        </div>       




</div>
</div>
</div>
</div>
    </div>
    </div>
</asp:Content>

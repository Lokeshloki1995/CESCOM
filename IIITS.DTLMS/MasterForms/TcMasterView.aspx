<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcMasterView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TcMasterView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         .ascending th a {
        background:url(img/sort_asc.png) no-repeat;
        display: block;
        padding: 0px 4px 0 20px;
    }

    .descending th a {

        background:url(img/sort_desc.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
     .both th a {
        background:url(img/sort_both.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }

    </style>
    <script type="text/javascript">
     function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        //allow only Numbers to paste
        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }

        //Charactes and space - _/to search Dtr Slno
        function characterAndspecialAndNum(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && (charCode < 48 || charCode > 57)
                && charCode != 32 && charCode != 45 && charCode != 47 && charCode != 95) {

                return false;
            }
            return true;
        }
        function cleanSpecialAndNum(t) {

            t.value = t.value.toString().replace(/[^-_/a-zA-Z0-9 \t\n\r]+/g, '');


        }

        //Charactes and space - . & () to search Tc Name
        function characterAndspecialTc(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 40 && charCode != 41 && charCode != 45 && charCode != 46 && charCode != 38) {

                return false;
            }
            return true;
        }
        //Remove Numbers, Special characters except space - . & () to search Tc Name
        function cleanSpecialAndNumTc(t) {

            t.value = t.value.toString().replace(/[^-.&()a-zA-Z \t\n\r]+/g, '');


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
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Existing DTR View
                   </h3>
                       <a style="margin-left:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
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
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Existing DTR View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                                <div style="float:right" >
                                <%--<div class="span6">
                                   <asp:Button ID="cmdNew" runat="server" Text="Create DTR" 
                                              CssClass="btn btn-primary" onclick="cmdNew_Click" /><br /></div>--%>
                                    <div class="span1">
                                        <asp:Button ID="cmdexport" runat="server" Text="Export Excel"  CssClass="btn btn-primary" 
                                          OnClick="Export_clickTCMaster" /><br />
                                          </div>

                                            </div>
                                  
                                
                                <!-- END FORM-->
                          <div class="widget-body form">
                            <!-- BEGIN FORM-->
                              <div class="form-horizontal">
                                <div class="row-fluid">
                               <%-- <div class="span1"></div>--%>
                               <div class="span5">
                                  <div class="control-group">
                                <label class="control-label">Make Name</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbMake" runat="server"  TabIndex="9" 
                                                   AutoPostBack="true" onselectedindexchanged="cmbMake_SelectedIndexChanged">                                   
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>
                             </div>

                             <div class="span5">
  
                             <div class="control-group">
                                <label class="control-label">Capacity(in KVA)</label>
                                    <div class="controls">
                                        <div class="input-append">
                                               <asp:DropDownList ID="cmbCapacity" runat="server"  TabIndex="9" AutoPostBack="true"
                                               onselectedindexchanged="cmbCapacity_SelectedIndexChanged">                                   
                                                </asp:DropDownList>       
                                        </div>
                                   </div>
                              </div>
               
                               <div class="span5">
                                  
                                    <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary" Visible="false"
                                         Width="116px" />
                                </div> 
                               </div>   
                                                              
                                    </div>
                                   <asp:Label ID="lblTotalDTr" runat="server" ForeColor="Green" Font-Size="Medium"></asp:Label>         
                               </div>
                            </div>
                        
                            <asp:GridView ID="grdTcMaster" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="10" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onpageindexchanging="grdTcMaster_PageIndexChanging" 
                                    onrowcommand="grdTcMaster_RowCommand"    OnSorting="grdTcMaster_Sorting" AllowSorting="true" >
                             <HeaderStyle CssClass="both"  />
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="TC_ID" HeaderText=" Transformer ID"  Visible="false">                                
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblTcId" runat="server" Text='<%# Bind("TC_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                  <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code" SortExpression="TC_CODE">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' style="word-break: break-all;" width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        <asp:Panel ID="panel4" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtTCCode" runat="server" placeholder="Enter DTr Code" onkeypress="return onlyNumbers(event)" onchange = "return cleanSpecialAndChar(this)" MaxLength="6"></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTR SlNo" SortExpression="TC_SLNO">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcSlNo" runat="server" Text='<%# Bind("TC_SLNO") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                           <FooterTemplate>
                                           <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtTCSlno" runat="server" placeholder="Enter DTr Slno" onkeypress="return characterAndspecialAndNum(event)"  
                                        onchange ="return cleanSpecialAndNum(this)" ></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <%--<asp:TemplateField AccessibleHeaderText="TC_MAKE_ID" HeaderText="Make" SortExpression="TC_MAKE_ID">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcMake" runat="server" Text='<%# Bind("TC_MAKE_ID") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                           <FooterTemplate>
                                           <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtMake" runat="server" placeholder="Enter Make" onkeypress="return characterAndspecialTc(event)"  
                                        onchange ="return cleanSpecialAndNumTc(this)" ></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make" SortExpression="TM_NAME">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcMake" runat="server" Text='<%# Bind("TM_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                           <FooterTemplate>
                                           <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                                           <asp:TextBox ID="txtMake" runat="server" placeholder="Enter Make" onkeypress="return characterAndspecialTc(event)"  
                                        onchange ="return cleanSpecialAndNumTc(this)" ></asp:TextBox>
                                           </asp:Panel>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" SortExpression="TC_CAPACITY">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcCapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                           <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="TC_LIFE_SPAN" HeaderText="Life Span" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTcLifeSpan" runat="server" Text='<%# Bind("TC_LIFE_SPAN") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 
                                   
                                    <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create" 
                                                Width="12px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                CausesValidation="false" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                  <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
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
                    <ul><li>
                   This Web Page Can Be Used To View All DTR Details and To Add New DTR.</li>
                    <li>  Existing DTR Details Can Be Edited By Clicking Edit Button</li>
                     <li>  New DTR  Can Be Added By Clicking New DTR Button
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

</asp:Content>

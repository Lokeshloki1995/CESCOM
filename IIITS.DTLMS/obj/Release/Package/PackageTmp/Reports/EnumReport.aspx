<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="EnumReport.aspx.cs" Inherits="IIITS.DTLMS.Reports.EnumReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script src="../Scripts/functions.js" type="text/javascript"></script>
  <script  type="text/javascript">

      function ValidateMyForm() {
          if (document.getElementById('<%= cmbType.ClientID %>').value == "--Select--") {
              alert('Select Type')
              document.getElementById('<%= cmbType.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbDiv.ClientID %>').value == "--All--") {
              alert('Select Division')
              document.getElementById('<%= cmbDiv.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= cmbSection.ClientID %>').value == "--All--") {
              alert('Select Section')
              document.getElementById('<%= cmbSection.ClientID %>').focus()
              return false
          }
//          if (document.getElementById('<%= cmbFeeder.ClientID %>').value == "--Select--") {
//              alert('Select Feeder')
//              document.getElementById('<%= cmbFeeder.ClientID %>').focus()
//              return false
//          }

      }
       
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
 <div >
     
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                   Enumeration Report
                   </h3>
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
                <div style="float:right;margin-top:20px;margin-right:12px" >
                     <%-- <asp:Button ID="Button1" runat="server" Text="Store View" 
                                      OnClientClick="javascript:window.location.href='StoreView.aspx'; return false;"
                            CssClass="btn btn-primary" />--%></div>
                            
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>  Enum Report</h4>
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
                                            <label class="control-label">Report Type<span class="Mandotary"> *</span></label>                                            
                                           <div class="controls">
                                                <div class="input-append">                                                       
                                                     <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="true"
                                                             TabIndex="1" > 
                                                           <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>  
                                                              <asp:ListItem Value="1" Text="Store Report"></asp:ListItem> 
                                                               <asp:ListItem Value="2" Text="Field Report"></asp:ListItem> 
                                                     </asp:DropDownList>   
                                                </div>
                                            </div>
                                        </div>   

                                        <div class="control-group">
                                            <label class="control-label"> Division</label>                                            
                                           <div class="controls">
                                                <div class="input-append">                                                       
                                                     <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true"
                                                             TabIndex="1" onselectedindexchanged="cmbDiv_SelectedIndexChanged"> </asp:DropDownList>   
                                                </div>
                                              
                                            </div>
                                        </div>
                                        
                                           <div class="control-group">
                                            <label class="control-label">Sub Division</label>
                                             
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                          <asp:DropDownList ID="cmbSubDiv" runat="server" AutoPostBack="true"
                                                             TabIndex="1" onselectedindexchanged="cmbSubDiv_SelectedIndexChanged" > </asp:DropDownList>   
                                                    </div>
                                                   
                                                </div>
                                        </div>  
                                        
                                         <div class="control-group">
                                            <label class="control-label">Section </label>
                                             
                                                <div class="controls">
                                                    <div class="input-append">                                                       
                                                          <asp:DropDownList ID="cmbSection" runat="server" AutoPostBack="true"
                                                             TabIndex="1" > </asp:DropDownList>   
                                                    </div>
                                                </div>
                                        </div>
 
                                    </div>
                                       
                                    <div class="span5">

                                      <div class="control-group">
                                      </div>
                                    
                                      

                                       <div class="control-group">
                                            <label class="control-label">Feeder </label>                                            
                                           <div class="controls">
                                                <div class="input-append">                                                       
                                                     <asp:DropDownList ID="cmbFeeder" runat="server" 
                                                             TabIndex="1"> </asp:DropDownList>   
                                                             
                                                </div>
                                                
                                            </div>
                                        </div>    

                                         <div class="control-group">
                                            <label class="control-label">From Date</label>
                                            <div class="controls">
                                                <div class="input-append">
                                                      <asp:TextBox ID="txtFromDate"  runat="server" MaxLength="15"></asp:TextBox>
                                                     <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                                      TargetControlID="txtFromDate"></ajax:CalendarExtender>
                                                </div>
                                            </div>
                                      </div>

                                     <div class="control-group">
                                        <label class="control-label">To Date</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                  <asp:TextBox ID="txtToDate"  runat="server" MaxLength="15"></asp:TextBox> 
                                                  
                                                 <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy" 
                                                  TargetControlID="txtToDate"></ajax:CalendarExtender>
                                            </div>
                                        </div>
                                 </div>    
                                              
                                        
                                    </div>      
                <div class="span1"></div>
                    </div>
                    <div class="space20"></div>
                                        
                <div  class="form-horizontal" align="center">

                    <div class="span3"></div>
                    <div class="span2">
                

                        <asp:Button ID="cmpReport" runat="server" Text="Generate Report" 
                            CssClass="btn btn-primary" onclick="cmpReport_Click" />

                        </div>
                    <%-- <div class="span1"></div>--%>
                    <div class="span1">  
                     <%--   <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                            CssClass="btn btn-primary" onclick="cmdReset_Click" /><br />--%>
                </div>
                            <div class="span7"></div>
                    <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                            
                </div>
                </div>
            </div>
                                
             <div class="space20"></div>
                                <!-- END FORM-->

               
                        </div>
                    </div>
                      <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
         </div>
         
      </div>
</asp:Content>

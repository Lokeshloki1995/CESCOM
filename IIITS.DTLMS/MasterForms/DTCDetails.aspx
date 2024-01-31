<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTCDetails.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DTCDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        function onlyNumbers(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if ((charCode == 46) || (charCode >= 48 && charCode <= 57 ))
                return true;
            else
                return false;
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
               Commisioning of DTC
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
                      <asp:Button ID="cmdClose" runat="server" Text="DTC View"                                     
                            CssClass="btn btn-primary" OnClientClick="javascript:window.location.href='DTCView.aspx'; return false;" /></div>
                                      
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Commissioning of DTC</h4>
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
                        <label class="control-label">DTC Code</label>
                        <div class="controls">
                            <div class="input-append">                                     
                                <asp:TextBox ID="txtDTCCode"  runat="server" 
                                       MaxLength="6" TabIndex="1" ReadOnly ="true"></asp:TextBox>            
                            </div>
                        </div>
                    </div>
   
                     <div class="control-group">
                        <label class="control-label">Platform Type</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="cmbPlatformType" runat="server" 
                                     TabIndex="8">
                                    <asp:ListItem Text="--select--" Selected="True"></asp:ListItem>
                                     <asp:ListItem Text="Single Pole" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="Double Pole" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtDTCId"  runat="server" TabIndex="15" MaxLength="10" Visible="false"  ></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Breaker Type</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlBreakertype"   runat="server"
                                     TabIndex="9">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="GOS" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="DOLO" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="NONE" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>

                    

                    <div class="control-group">
                        <label class="control-label">DTC Meters Available</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddldtcmeters"  runat="server"  Visible="false"
                                     TabIndex="10">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">HT Protection</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlhtprotection"   runat="server"
                                     TabIndex="11">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                      <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">LT Protection</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlLTProtection"  runat="server"
                                     TabIndex="12">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Grounding</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlgrounding"   runat="server"
                                     TabIndex="13">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                   <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                                       
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Lightning Arresters</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="ddlArresters"    runat="server"
                                     TabIndex="14">
                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:DropDownList>                                                       
                            </div>
                        </div>
                    </div>

                    
                                  
   
                </div>
                <div class="span5"> 
                    
                     <div class="control-group">
                        <label class="control-label">DTC Name</label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtDTCName" runat="server" MaxLength="50" 
                                    onkeypress="return AllowOnlysCharNotAllowSpecial(event);" TabIndex="2" ReadOnly="true" ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Load Type</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="cmbLoadtype" runat="server"  TabIndex="14">
                                </asp:DropDownList>                                                       
                            </div>
                        </div>
                    </div>
                    
                    <div class="control-group" style="display:none">
                        <label class="control-label">Project/Scheme Type</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:DropDownList ID="cmbprojecttype" runat="server"  TabIndex="14">
                                </asp:DropDownList>                                                       
                            </div>
                        </div>
                    </div>
                    
                    <div class="control-group" style="display:none">
                        <label class="control-label">HT Line Length(in KM)</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:TextBox ID="txthtLine"  runat="server" TabIndex="15" MaxLength="10" onkeypress="return onlyNumbers(event)" ></asp:TextBox>               
                            </div>                            
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">LT Line Length(in KM)</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:TextBox ID="txtltLine" runat="server" TabIndex="16" MaxLength="10" onkeypress="return onlyNumbers(event)" onpaste="return false" ></asp:TextBox>               
                            </div>                            
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label">Depreciation</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:TextBox ID="txtDepreciation" runat="server" TabIndex="16" MaxLength="10" onkeypress="return onlyNumbers(event)" onpaste="return false" ></asp:TextBox>               
                            </div>                            
                        </div>
                    </div>
                    
                    <div class="control-group">
                        <label class="control-label">Latitude</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:TextBox ID="txtLatitude" onpaste="return false"  runat="server" TabIndex="16" MaxLength="10" onkeypress="return onlyNumbers(event)"  ></asp:TextBox>               
                            </div>                            
                        </div>
                    </div>            
                    
                     <div class="control-group">
                        <label class="control-label">Longitude</label>
                        <div class="controls">
                            <div class="input-append">
                                 <asp:TextBox ID="txtlongitude" runat="server" onpaste="return false" TabIndex="16" MaxLength="10" onkeypress="return onlyNumbers(event)" ></asp:TextBox>               
                            </div>                            
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
                                        <asp:Button ID="cmbBack" runat="server" Text="Back" 
                                        CssClass="btn btn-primary" 
                                                TabIndex="26" onclick="cmbBack_Click" />
                                         </div>
                                        <div class="span1">
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" 
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary" 
                                                TabIndex="26" />
                                         </div>
                                      <%-- <div class="span1"></div>--%>
                                     <div class="span1">  
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-primary" onclick="cmdReset_Click" TabIndex="27" /><br />
                                    </div>
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
         </div>

</asp:Content>

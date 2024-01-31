<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="EnumLevel.aspx.cs" Inherits="IIITS.DTLMS.Internal.EnumLevel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
<script  type="text/javascript">

    function ValidateMyForm() {

       
        if (document.getElementById('<%= cmbDivision.ClientID %>').value.trim() == "--Select--") {
            alert('Select Division')
            document.getElementById('<%= cmbDivision.ClientID %>').focus()
            return false
        }

        if (document.getElementById('<%= cmbsubdivision.ClientID %>').value.trim() == "--Select--") {
            alert('Select Sub Division')
            document.getElementById('<%= cmbsubdivision.ClientID %>').focus()
            return false
        }
        if (document.getElementById('<%= cmbSection.ClientID %>').value.trim() == "--Select--") {
            alert('Select Section')
            document.getElementById('<%= cmbSection.ClientID %>').focus()
            return false
        }
        if (document.getElementById('<%= cmbFeeder.ClientID %>').value.trim() == "--Select--") {
            alert('Select Feeder')
            document.getElementById('<%= cmbFeeder.ClientID %>').focus()
            return false
        }
        if (document.getElementById('<%= cmbLevels.ClientID %>').value.trim() == "--Select--") {
            alert('Select Level')
            document.getElementById('<%= cmbLevels.ClientID %>').focus()
            return false
        }

        if (document.getElementById('<%= txtRemarks.ClientID %>').value.trim() == "") {
            alert('Enter Remarks')
            document.getElementById('<%= txtRemarks.ClientID %>').focus()
            return false
        }
       

    }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"> 
 </asp:ScriptManager>
<div >     
  <div class="container-fluid">
        <div class="row-fluid">
            <div class="span8">          
                <h3 class="page-title">
                Enumeration Level
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
          
        </div>            
        
        <div class="row-fluid">
            <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Enumeration Level</h4>
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
                                                   <label class="control-label">Division Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbDivision" runat="server"  AutoPostBack="true" 
                                                                TabIndex="2" onselectedindexchanged="cmbDivision_SelectedIndexChanged" >                                   
                                                            </asp:DropDownList>
                                                            
                                                       </div>
                                                    </div>
                                               </div>

                                                <div class="control-group">
                                                   <label class="control-label">Sub Division Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbsubdivision" runat="server"  AutoPostBack="true" 
                                                                TabIndex="3" onselectedindexchanged="cmbsubdivision_SelectedIndexChanged">                                   
                                                            </asp:DropDownList>
                                                            
                                                       </div>
                                                    </div>
                                               </div>


                                                  <div class="control-group">
                                                   <label class="control-label">Section Name<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbSection" runat="server"  TabIndex="4">                                   
                                                            </asp:DropDownList>
                                                       </div>
                                                    </div>
                                               </div>
                                              

                                        
                                            
                                           </div>   
                                           <div class="span5">   




                                              <div class="control-group">
                                                   <label class="control-label">Feeder Code<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                            <asp:DropDownList ID="cmbFeeder" runat="server"  TabIndex="5" >                                   
                                                            </asp:DropDownList>
                                                       </div>
                                                    </div>
                                               </div> 


                                               <div class="control-group">
                                                   <label class="control-label">Levels<span class="Mandotary"> *</span></label>
                                                   <div class="controls">   
                                                       <div class="input-append">
                                                             <asp:DropDownList ID="cmbLevels" runat="server"  AutoPostBack="true" 
                                                                TabIndex="1"  >                                   
                                                            </asp:DropDownList>
                                                           
                                                       </div>
                                                    </div>                                            
                                             </div>    
                                            
                                                        <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                                                        <asp:HiddenField ID="hdfDivision" runat="server" />
                                                            <asp:HiddenField ID="hdfSubdivision" runat="server" />
                                                            <asp:HiddenField ID="hdfSection" runat="server" />
                                                            <asp:HiddenField ID="hdfFeeder" runat="server" />
                                           </div>  

                                           <div class="span20"></div>
                                           <div class="span12">   

                                           <div class="span1"></div>
                                            
                                            <div class="control-group">
                                                <label class="control-label">Remarks <span class="Mandotary"> *</span></label>
                    
                                                <div class="controls">
                                                    <div class="input-append">
          
                                                         <asp:TextBox ID="txtRemarks" runat="server"  TextMode="MultiLine" 
                                                         style="resize:none" onkeyup="return ValidateTextlimit(this,500);" Width="676px" 
                                                              ></asp:TextBox>
                                                    </div>
                       
                                             </div>
                   
               
                                           </div>  
                                             

  
                <div class="span3"></div>
                <div >
                    <asp:Button ID="cmdSave" runat="server" Text="Save"  CssClass="btn btn-primary" 
                        TabIndex="53" Width="105px" onclick="cmdSave_Click" OnClientClick="javascript:return ValidateMyForm()"   />
                </div>    
                                         
                                           </div>       
                                      



                                     
                                    </div>
                            </div>
                        </div>
                            
                               </div>
                  
                                 </div>


                              <div class="widget-body">
                                                                                           
                               
               
                             </div>
            </div>         
        </div>

     

       

     
      
    
 </div>
</div>

    </div>
</asp:Content>

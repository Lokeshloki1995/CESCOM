<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="SubDivision.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.SubDivision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
    window.onload = function () {
        var seconds = 5;
        setTimeout(function () {
            document.getElementById("<%=lblErrormsg.ClientID %>").style.display = "none";
        }, seconds * 1000);
    };

       
</script>

 <script type="text/javascript">

     function Validate() {

         if (document.getElementById('<%=cmbCircle.ClientID %>').value.trim() == "--Select--") {
             alert('Select  Circle')
             document.getElementById('<%= cmbCircle.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%=cmbDivision.ClientID %>').value.trim() == "--Select--") {
             alert('Select  Division')
             document.getElementById('<%= cmbDivision.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtSubDivCode.ClientID %>').value.trim() == "") {
             alert('Enter Sub-Division Code')
             document.getElementById('<%= txtSubDivCode.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtName.ClientID %>').value.trim() == "") {
             alert('Enter valid Sub-Division Name ')
             document.getElementById('<%= txtName.ClientID %>').focus()
             return false
         }

       
         if (document.getElementById('<%= txtOfficeHead.ClientID %>').value.trim() == "") {
             alert('Enter office head')
             document.getElementById('<%= txtOfficeHead.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtOfficeHead.ClientID %>').value.trim() == "") {
             alert('Enter OfficeHead')
             document.getElementById('<%= txtOfficeHead.ClientID %>').focus()
             return false
         }

         if (document.getElementById('<%= txtMobileNo.ClientID %>').value.trim() == "") {
             alert('Enter valid Mobile Number')
             document.getElementById('<%= txtMobileNo.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtPhoneNo.ClientID %>').value.trim() == "") {
             alert('Enter valid Phone No')
             document.getElementById('<%= txtPhoneNo.ClientID %>').focus()
             return false
         }
         if (document.getElementById('<%= txtEmail.ClientID %>').value.trim() == "") {
             alert('Enter Email Id')
             document.getElementById('<%= txtEmail.ClientID %>').focus()
             return false
         }

     }
     // Only Characters to enter
     //function character(evt) {
     //    evt = (evt) ? evt : event;
     //    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
     //    ((evt.which) ? evt.which : 0));
     //    if (charCode > 31 && (charCode < 65 || charCode > 90) &&
     //    (charCode < 97 || charCode > 122)) {

     //        return false;
     //    }
     //    return true;
     //}
     //Charactes and space to create Subdiv Name
     //&& charCode != 45 && charCode != 46 && charCode != 49 -> - . 1 to create Subdiv Name
     function characterAndspecialSub(event) {
         var evt = (evt) ? evt : event;
         var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
         ((evt.which) ? evt.which : 0));
         if ((charCode < 65 || charCode > 90) &&
          (charCode < 97 || charCode > 122) && charCode != 32 ) {

             return false;
         }
         return true;
     }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
<div class="span8">
<!-- BEGIN THEME CUSTOMIZER-->
                 
<!-- END THEME CUSTOMIZER-->
<!-- BEGIN PAGE TITLE & BREADCRUMB-->
<h3 class="page-title">
Create SubDivision
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
<div style="float:right;margin-right:10px;margin-top:20px" class="span2"><asp:Button ID="cmdSubDivisionView" class="btn btn-primary" Text="SubDivision View" OnClientClick="javascript:window.location.href='SubDivisionView.aspx'; return false;" runat="server" />  </div>
</div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>SubDivision Office Entry</h4>
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
                                                <label class="control-label">Circle<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" runat="server" 
                                                          AutoPostBack="true" onselectedindexchanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server"  AutoPostBack="true"
                                                            onselectedindexchanged="cmbDivision_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                                     <div class="control-group">
                                                <label class="control-label">Sub-Division Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                       
                                                        <asp:TextBox ID="txtSubDivCode" runat="server" onkeypress="return OnlyNumber(event)"  MaxLength="3" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="control-group">
                                                <label class="control-label">Sub Division Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                       
                                                        <asp:TextBox ID="txtName" runat="server" MaxLength="50"  onkeypress="return characterAndspecialSub(event)" onpaste="return false;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                             
                                         
                                           
                                           
                                        </div>
                                        <div class="span5">
                                         

                                            
                                           <div class="control-group">
                                                <label class="control-label">Office Head<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                       
                                                        <asp:TextBox ID="txtOfficeHead" runat="server" MaxLength="20" onkeypress="return characterAndspecialSub(event)" onpaste="return false;"></asp:TextBox>
                                                        <asp:TextBox ID="txtSubDivId" runat="server" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">MobileNo<span class="Mandotary"> *</span></label>


                                                <div class="controls">
                                                    <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>  

                                            <div class="control-group">
                                                <label class="control-label">PhoneNo<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                         <asp:TextBox ID="txtPhoneNo" runat="server" MaxLength="15" onkeypress="javascript:return OnlyNumberHyphen(this,event);" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>  

                                            <div class="control-group">
                                                <label class="control-label">Email<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-prepend">
                                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" onkeypress="javascript:return validateEmail(txtEmailId);" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>                                           

                                            </div>
    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div class="text-center" align="center">

                                        
                                      
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" 
                                       OnClientClick="javascript:return Validate()" CssClass="btn btn-success" />
                                       
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                            onclick="cmdReset_Click"  CssClass="btn btn-danger" /><br />
                                   
                                                <div class="span7"></div>
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                            
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

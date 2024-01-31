<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TcMakeMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TcMakeMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
  <script  type="text/javascript">

      function ValidateMyForm() {
          if (document.getElementById('<%= txtMakeName.ClientID %>').value.trim() == "") {
              alert('Enter Valid Make Name')
              document.getElementById('<%= txtMakeName.ClientID %>').focus()
              return false
          }
          if (document.getElementById('<%= txtDescription.ClientID %>').value.trim() == "") {
              alert('Enter Valid Description')
              document.getElementById('<%= txtDescription.ClientID %>').focus()
              return false
          }

      }

      function ResetForm() {
          document.getElementById('<%= txtMakeName.ClientID %>').value = "";
          document.getElementById('<%= txtDescription.ClientID %>').value = "";
          document.getElementById('<%= txtMakeId.ClientID %>').value = "";

          return false;
      }

      //Charactes and space - . to search Tc Name
      function characterAndspecialTc(event) {
          var evt = (evt) ? evt : event;
          var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
          ((evt.which) ? evt.which : 0));
          if ((charCode < 65 || charCode > 90) &&
           (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 45 && charCode != 46 ) {

              return false;
          }
          return true;
      }

      function cleanSpecialAndChar(t) {
          debugger;
          t.value = t.value.toString().replace(/[^/0-9\n\r]+/g, '');
          //alert(" Special charactes and characters are not allowed!");
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
                     Create Make
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
                      <asp:Button ID="Button1" runat="server" Text="Make View" 
                                      OnClientClick="javascript:window.location.href='TcMakeMasterView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>
                         
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Create Make</h4>
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
                       
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtMakeId"  runat="server" onkeypress="return OnlyNumber(event)" MaxLength="50" Visible ="false"></asp:TextBox>            
                                   
                            </div>
                        </div>
                    </div>

                                                        <div class="control-group">
                        <label class="control-label">Make Name<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                                       
                                <asp:TextBox ID="txtMakeName" runat="server" MaxLength="50"  onkeypress="return characterAndspecialTc(event)" 
                                     onpaste="return false;"></asp:TextBox>
                            &nbsp;
                            </div>
                        </div>
                    </div>
                                                        <div class="control-group">
                        <label class="control-label">Description<span class="Mandotary"> *</span></label><div class="controls">
                                                                <div class="input-append">
                                    <asp:TextBox ID="txtDescription" runat="server" onkeyup="return ValidateTextlimit(this,100);"  style = "resize:none" TextMode="MultiLine"
                                         onpaste="return false;"></asp:TextBox>
                                                       
                            </div>
                        </div>
                    </div>

                     <div class="control-group">
                        <label class="control-label" ></label>
                         <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <div class="controls">
                            
                        </div>
                    </div>



                                  </div>
                                                         
                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="text-center" align="center">

                                       
                                        <asp:Button ID="cmdSave" runat="server" Text="Save"
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-success" 
                                                onclick="cmdSave_Click" />
                                        
                                         <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                             CssClass="btn btn-danger" onclick="cmdReset_Click" /><br />
                               
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

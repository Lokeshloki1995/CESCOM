<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTCSectionSwapping.aspx.cs" Inherits="IIITS.DTLMS.Query.DTCSectionSwapping" %>
<%@ Register Src="/ReportFilterControl.ascx" TagName="ReportFilterControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type ="text/javascript">
    function ValidateMyForm() {

        if ((document.getElementById('<%= txtDTCCode.ClientID %>').value == "") || (document.getElementById('<%= txtDTCCode.ClientID %>').value == null)) {
             alert('Please select DTC Code')
             document.getElementById('<%= txtDTCCode.ClientID %>').focus()
             return false
        }

        if ((document.getElementById('<%= txtDTRCode.ClientID %>').value == "") || (document.getElementById('<%= txtDTRCode.ClientID %>').value == null)) {
            alert('Please select DTR Code')
            document.getElementById('<%= txtDTRCode.ClientID %>').focus()
             return false
         }
    }
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class ="container-fluid">
           <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                   DTC Section Swapping
                </h3>
                <ul class="breadcrumb" style="display: none">
                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                        <div class="input-append search-input-area">
                            <input class="" id="appendedInputButton" type="text">
                            <button class="btn" type="button">
                                <i class="icon-search"></i>
                            </button>
                        </div>
                        </form>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
            <div style="float: right; margin-top: 20px; margin-right: 12px">
            </div>
        </div>
        <!-- END PAGE HEADER-->
         <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
           <div class ="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>
                            <i class="icon-reorder"></i>DTC SECTION SWAPPPING</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
               
                <div class="widget-body">
                        <div class="widget-body form">
                            <!--BEGIN FORM -->
                            <div class ="form-horizontal">
                                 <uc1:ReportFilterControl ID="ReportFilterControl1" runat="server" />
                                <div class ="row-fluid">
                                    <div class ="span1"></div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <label class ="control-label" >
                                                DTC CODE <span class ="Mandotary" >*</span>
                                            </label>
                                            <asp:TextBox id="txtDTCCode"  runat ="server" MaxLength ="6" style ="margin-left:30px" ToolTip="Enter DTC Code"></asp:TextBox>
                                        </div>
                                    </div>
                                  <div class ="span5">
                                        <div class="control-group">
                                            <label class ="control-label">
                                                DTR CODE <span class ="Mandotary" >*</span>
                                            </label>
                                            <asp:TextBox id="txtDTRCode"  runat ="server" MaxLength ="6" style ="margin-left:18px" ToolTip="Enter DTR Code" ></asp:TextBox>
                                        </div>
                                  </div>
                                </div>    
                          </div>
                            <div class="space20"></div>
                            <div class="span4"></div>
                            <div class="span1">
                                <asp:Button ID="cmdSave" runat="server" Text="Update Section" CssClass="btn btn-primary" OnClientClick="javascript:return ValidateMyForm()" OnClick="cmdSave_Click" />
                            </div>
                            <asp:Label id="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                             <div class="space5"></div>

                       </div>
                   </div><!--END OF WIDGET FORM  --->
                        
                </div>
               </div>
           </div>
        </div>
</div>



   
</asp:Content>

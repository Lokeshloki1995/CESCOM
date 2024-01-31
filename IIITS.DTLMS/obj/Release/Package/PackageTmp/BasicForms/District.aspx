<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="District.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.District" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateMyForm() {

            if (document.getElementById('<%= txtDistrictCode.ClientID %>').value == "0") {
                alert('District Code an not be 0')
                document.getElementById('<%= txtDistrictCode.ClientID %>').focus()
                return false
            }
            
            if (document.getElementById('<%= txtDistrictCode.ClientID %>').value == "") {
                alert('Enter the District Code')
                document.getElementById('<%= txtDistrictCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDistrictName.ClientID %>').value == "") {
                alert('Enter the District Name')
                document.getElementById('<%= txtDistrictName.ClientID %>').focus()
                return false
            }

        }

        // Only Characters and Space to enter
        function characterAndSpace(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode > 31 && (charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && charCode != 32) {

                return false;
            }
            return true;
        }
     
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span8">
                <!-- BEGIN THEME CUSTOMIZER-->
                <!-- END THEME CUSTOMIZER-->
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">
                    Create District 
                </h3>
                <ul class="breadcrumb" style="display: none">
                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                        <div class="input-append search-input-area">
                            <input class="" id="appendedInputButton" type="text" />
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
                  <asp:Button ID="cmdClose" runat="server" Text="District View" 
                                      OnClientClick="javascript:window.location.href='DistrictView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>
        </div>
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue" >
                    <div class="widget-title" >
                        <h4>
                            <i class="icon-reorder"></i>Create District</h4>
                        <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                            class="icon-remove"></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <!-- BEGIN FORM-->
                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1">
                                    </div>
                                    <div class="span5">
                                        <div class="control-group">
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDistId" runat="server" MaxLength="4" Visible="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                District Code <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDistrictCode" onkeypress="javascript:return OnlyNumber(event);"
                                                        runat="server" MaxLength="1"></asp:TextBox>
                                                </div>
                                              
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label">
                                                District Name <span class="Mandotary">*</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:TextBox ID="txtDistrictName" runat="server" MaxLength="30" onkeypress="return characterAndSpace(event)"
                                     onpaste="return false;"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span1">
                                    </div>
                                </div>
                                <div class="space20">
                                </div>
                                <div class="text-center" align="center">
                                   
                                   
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return ValidateMyForm()"
                                            CssClass="btn btn-success" OnClick="cmdSave_Click" />
                                
                                  
                                        <asp:Button ID="cmdClear" runat="server" Text="Reset" CssClass="btn btn-danger"
                                            OnClick="cmdClear_Click" /><br />
                                  
                                    <div class="span7">
                                    </div>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="space20">
                        </div>
            
                        <!-- END FORM-->
                    </div>
                </div>
                <!-- END SAMPLE FORM PORTLET-->
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
</asp:Content>

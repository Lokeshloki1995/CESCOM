<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="TransSupplier.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.TransSupplier" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValidateMyForm() {

            if (Page_ClientValidate()) {

                if (document.getElementById('<%= txtSupplierName.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Supplier name')
                  document.getElementById('<%= txtSupplierName.ClientID %>').focus()
                  return false
              }


              if (document.getElementById('<%= txtSupplierPhnNo.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Supplier Phone No')
                  document.getElementById('<%= txtSupplierPhnNo.ClientID %>').focus()
                  return false
              }



              if (document.getElementById('<%= txtSupplierEmailId.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Supplier EmailId')
                  document.getElementById('<%= txtSupplierEmailId.ClientID %>').focus()
                  return false
              }



              //              if (document.getElementById('<%= cmbIsBlack.ClientID %>').value == "-Select-") {
              //                  alert('Select the Black condition')
              //                  document.getElementById('<%= cmbIsBlack.ClientID %>').focus()
              //                  return false
              //              }


              if (document.getElementById('<%= txtSupplierAddress.ClientID %>').value.trim() == "") {
                  alert('Please Enter Valid Register Address')
                  document.getElementById('<%= txtSupplierAddress.ClientID %>').focus()
                  return false
              }
          }

      }
        //Charactes and space to create Tc Name
        function characterAndspecialTcSup(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && charCode != 32) {

                return false;
            }
            return true;
        }

        // Only Characters to enter
        function character(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode > 31 && (charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122)) {

                return false;
            }
            return true;
        }

    </script>
    <style type="text/css">
        .ascending th a {
            background: url(img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }
    </style>
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
                    <h3 class="page-title">Supplier Details
                    </h3>
                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
                <div style="float: right; margin-top: 20px; margin-right: 12px">
                    <asp:Button ID="cmdClose" runat="server" Text="Supplier View"
                        OnClientClick="javascript:window.location.href='TransSupplierView.aspx'; return false;"
                        CssClass="btn btn-primary" />
                </div>


            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Supplier Details</h4>
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
                                                <label class="control-label">Supplier Name <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtSupplierName" runat="server" MaxLength="50" onkeypress="return characterAndspecialTcSup(event)" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Phone Number
                                                    <br />
                                                    <span>(with STD Code)</span>  <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtSupplierPhnNo" runat="server"
                                                            MaxLength="15" onkeypress="javascript:return OnlyNumberHyphen(this,event);" onpaste="return false"></asp:TextBox>
                                                        <asp:TextBox ID="txtSuppId" runat="server" MaxLength="50" Visible="false" Width="20px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Email Id <span class="Mandotary">*</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtSupplierEmailId" runat="server" MaxLength="50"
                                                            CausesValidation="True" onpaste="return false"></asp:TextBox>
                                                        </br>
                                      <asp:RegularExpressionValidator runat="server" ID="regular" ControlToValidate="txtSupplierEmailId"
                                          ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
                                          ErrorMessage="Please enter a valid email Id" ForeColor="Red"
                                          Display="Dynamic" Font-Size="Small" />


                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Contact Person Name </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtContactPerson" runat="server" MaxLength="50" onpaste="return false"  onkeypress="return character(event)"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Mobile Number </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10" onkeypress="javascript:return OnlyNumber(event);" onpaste="return false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5" rowspan="2">

                                            <div class="control-group">
                                                <label class="control-label">Fax No </label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="20" onpaste="return false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Block Listed</label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbIsBlack" runat="server"
                                                            OnSelectedIndexChanged="cmbIsBlack_SelectedIndexChanged" AutoPostBack="true" onpaste="return false">
                                                            <asp:ListItem>-Select-</asp:ListItem>
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">
                                                    <label class="control-label">Block Listed Upto<span class="Mandotary" visible="false" id="blocklist" runat="server"> *</span></label>
                                                    <%-- <asp:Label ID="lblBlack" runat="server" Text="BlackUpto"></asp:Label><span class="Mandotary"> *</span>
                                                    --%>

                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtDateUpto" runat="server" MaxLength="10"></asp:TextBox>
                                                            <asp:CalendarExtender ID="txtfromdateExtender" runat="server" CssClass="cal_Theme1"
                                                                TargetControlID="txtDateUpto" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">Register Address<span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtSupplierAddress" runat="server" MaxLength="250"
                                                            Style="resize: none" Height="60px" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,250);" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Communication  Address </label>

                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:TextBox ID="txtCommAddress" runat="server" MaxLength="200"
                                                            Style="resize: none" Height="60px" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,200);" onpaste="return false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="text-center" align="center">

                                    
                                      
                                            <asp:Button ID="cmdSave" runat="server" Text="Save"
                                                OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-success"
                                                OnClick="cmdSave_Click" />
                                
                                      
                                            <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                                                CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
                                      
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

    </div>





</asp:Content>

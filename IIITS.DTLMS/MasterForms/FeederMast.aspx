<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="FeederMast.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.FeederMast" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'
          src='http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js'>

    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblErrormsg.ClientID %>").style.display = "none";
}, seconds * 1000);
};

function cleanSpecialAndChar(t) {
    debugger;
    t.value = t.value.toString().replace(/[^/0-9\n\r]+/g, '');
    //alert(" Special charactes and characters are not allowed!");
}

function IsAlphaNumeric(e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || keyCode == 32 || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
    document.getElementById("error").style.display = ret ? "none" : "inline";
    return ret;
}
    </script>

    <script type="text/javascript">
    function Validate() {
        var isValid = false;
        var regex = /^[a-zA-Z0-9]*$/;
        isValid = regex.test(document.getElementById("<%=txtlstfedercode.ClientID %>").value);
        document.getElementById("spnError").style.display = !isValid ? "block" : "none";
        return isValid;
    }
</script>

    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
    <script type="text/javascript">
     var specialKeys = new Array();
     specialKeys.push(8);  //Backspace
     specialKeys.push(9);  //Tab
     specialKeys.push(46); //Delete
     specialKeys.push(36); //Home
     specialKeys.push(35); //End
     specialKeys.push(37); //Left
     specialKeys.push(39); //Right
 
     function IsAlphaNumeric(e) {
         var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
         var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || keyCode == 32 || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
         document.getElementById("error").style.display = ret ? "none" : "inline";
         return ret;
     }
</script>


      <script language="Javascript" type="text/javascript">


         function onlyAlphabets(e, t) {
             var code = ('charCode' in e) ? e.charCode : e.keyCode;
             if ( // space
               !(code == 32 || code == 46) &&
                //  !(code == 40 || code == 41) && // Special characters
                //       !(code >43 && code<= 47) &&
               !(code > 64 && code < 91) && // upper alpha (A-Z)
               !(code > 96 && code < 123) && !(code >= 48 && code <= 57)) { // lower alpha (a-z)
                 e.preventDefault();
             }
         }
            </script>

    <%--
         !(keycode == 8 || keycode == 46) &&  (keycode < 48 || keycode > 57)
        <script>
function Validate() {



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

if (document.getElementById('<%= cmbDistrict.ClientID %>').value.trim() == "--Select--") {
alert('Select  District')
document.getElementById('<%= cmbDistrict.ClientID %>').focus()
return false
}


if (document.getElementById('<%=cmbTaluk.ClientID %>').value.trim() == "--Select--") {
alert('Select  Taluk')
document.getElementById('<%= cmbTaluk.ClientID %>').focus()
return false
}

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

}
</script>--%>
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
                    <h3 class="page-title">Create Feeder
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
                    <asp:Button ID="cmdFeederView" class="btn btn-primary" Text="Feeder View"
                        OnClientClick="javascript:window.location.href='FeederView.aspx'; return false;" runat="server" />
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Create Feeder</h4>
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
                                                <label class="control-label">Station Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbStation" runat="server"   AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbStation_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtFeederId" runat="server" MaxLength="25" Visible="false" onchange="return cleanSpecialAndChar(this)">0</asp:TextBox>
                                                        <asp:HiddenField ID="hdfBank" runat="server" />
                                                        <asp:HiddenField ID="hdfBus" runat="server" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Bank Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbBank" runat="server" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbBank_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Bus Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbbus" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label"> Sub Division  Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOfficeCode" runat="server" onchange ="return cleanSpecialAndChar(this)"  MaxLength="25"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearch_Click" />

                                                    </div>
                                                </div>
                                            </div>

                                 <%--           <div class="control-group">
                                                <label class="control-label"></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:Label ID="lblSlno" runat="server"></asp:Label>

                                                    </div>
                                                </div>
                                            </div>--%>

                                            <%--onkeypress="return IsAlphaNumeric(event);"                 ondrop="return false;" onpaste="return false;"--%>


                                              <div class="control-group">
                                                <label class="control-label">Last Digit of Feeder Code<span class="Mandotary">*</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtlstfedercode"   runat="server" MaxLength="1"  AutoPostBack="true" 
                                                            OnTextChanged="txtlstfedercode_SelectedIndexChanged" onkeypress="return onlyAlphabets(event,this);" ></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Feeder Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFeederCode"   runat="server" MaxLength="4"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">Feeder Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFeederName"   runat="server" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Feeder Type<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbType" runat="server">
                                                            <%--   <asp:ListItem Text="---SELECT---" Value="0" />
                                    <asp:ListItem  Value="Rural mixed" />
                                    <asp:ListItem  Value="IPP" />
                                    <asp:ListItem  Value="Urban" />--%>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Feeder Category<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">

                                                        <asp:DropDownList ID="cmbCat" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Interflow<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbInt" runat="server">
                                                            <asp:ListItem Text="---SELECT---" Value="0" />
                                                            <asp:ListItem Text="Yes" Value="1" />
                                                            <asp:ListItem Text="No" Value="2" />

                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Connected dtc capacity (KVA)</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTC" runat="server" MaxLength="25"  ></asp:TextBox>
                                                        <%--<asp:DropDownList ID="cmbCapacity" runat="server" >                                      
                                  </asp:DropDownList>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="span1"></div>
                                    </div>
                                    <div class="space20"></div>

                                    <div class="text-center" align="center">



                                        <asp:Button ID="cmdSave" runat="server" Text="Save"
                                            OnClientClick="javascript:return Validate()" CssClass="btn btn-success"
                                            OnClick="cmdSave_Click" />



                                        <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                            CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />

                                        <div class="span7"></div>
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>

                                    </div>

                                    <div class="space20"></div>
                                    <div class="form-horizontal">
                                        <div class="span8">
                                            <asp:Label ID="lblTxt" runat="server" ForeColor="Green" Text="Note : Add or Replace Office Code by Selecting/DeSelecting from checkbox"></asp:Label>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                            <%-- <asp:Panel ID="pnlControls" runat="server" Height="500px"  BackColor="White">
                            --%>
                        </div>
                        <div class="space20"></div>

                    </div>
                </div>
            </div>

            <!-- END PAGE CONTENT-->
        </div>

        <div class="space20"></div>
        <div class="space20"></div>
        <div class="space20"></div>
        <div class="space20"></div>

        <asp:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnShowPopup" CancelControlID="cmdClose"
            PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
        <div style="width: 100%; vertical-align: middle" align="center">



            <asp:Panel ID="pnlControls" runat="server"  CssClass="modalPopup" align="center" style = "display:none"  Height="550px" Width="500px">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>Select Office Codes And Click On Proceed</h4>
                        <div class="space20"></div>


                        <asp:GridView ID="GrdOffices" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" OnPageIndexChanging="GrdOffices_PageIndexChanging" ShowHeaderWhenEmpty="True"
                            EmptyDataText="No Records Found" ShowFooter="true"
                            PageSize="6" Width="90%" OnRowDataBound="GrdOffices_RowDataBound"
                            AllowPaging="True" DataKeyNames="OFF_CODE" OnRowCommand="GrdOffices_RowCommand">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="OFF_CODE" HeaderText="Office Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFF_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffCode" runat="server" placeholder="Enter Office Code" onpaste="return false" onchange ="return cleanSpecialAndChar(this)" Width="100px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Office Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStaDesc" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all" Width="150px"> </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtOffName" runat="server" placeholder="Enter Office Name" onpaste="return false" onchange ="return cleanSpecialAndChar(this)" Width="200px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" Visible="true">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="space20"></div>

                        <div class="row-fluid">
                            <div class="span1"></div>
                            <div class="span2">

                                <div class="control-group">
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:Button ID="btnOK" runat="server" CssClass="btn btn-primary" Text="Proceed" OnClick="btnOK_Click1" />

                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="span2">

                                <div class="control-group">

                                    <div class="controls">
                                        <div class="input-append">
                                            <%--onclick="btnClose_Click"--%>
                                            <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-primary" Text="Cancel" />

                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>

        </div>


    </div>

    </div>


</asp:Content>

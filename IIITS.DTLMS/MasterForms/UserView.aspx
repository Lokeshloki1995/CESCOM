<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="UserView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.UserView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script  src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>

    <script type="text/javascript">

        //$(document).ready(function () {
        //    debugger;
        //    var isPostBackObject = document.getElementById('isPostBack');
        //    if (isPostBackObject != null )
        //    {
        //        //alert(document.getElementById('officeCode').style.display);
        //        document.getElementById('officeCode').style.display = "none";

        //    }


        //    $('#btnCorporateOffice').click(function () {
        //        debugger;
        //        $.ajax({
        //            url: 'UserView.aspx/btnCorporateOffice_click',
        //            contentType: 'application/json; charset=utf-8',
        //            dataType: 'json',
        //            type : 'POST',
        //            success: function (response) {
        //                alert(response.d);
        //            }
        //        })

        //    })
      

        //}



        //);



        function ConfirmStatus(status) {

            var result = confirm('Are you sure,Do you want to ' + status + ' User?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }

        //allow only characters to enter
        function character(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode > 31 && (charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && charCode != 46 && charCode != 47) {
               
                return false;
            }
            return true;
        }
        // allow only Numbers to enter
        function onlyNumbers(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 47)
                return false;
            else
                return true;
        }
        //allow only characters to paste
        function cleanSpecialAndNum(t) {
            debugger;
            t.value = t.value.toString().replace(/[^a-zA-Z\t\n\r]+/g, '');
            //alert(" Special charactes and Numbers are not allowed!");
        }

        function cleanSpecialAndalfabets(t) {
            debugger;
            t.value = t.value.toString().replace(/[^0-9\t\n\r]+/g, '');
            //alert(" Special charactes and Numbers are not allowed!");
        }


        function enterSearchBox(evt) {
            debugger;
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if(charCode == 13)
            {
                return false;
               // document.getElementById("btnSearch").click();
            }
        }

        function cmbOfficselection(control) {
            debugger;
            var selectedValue = control.value;

            //   0 : corporate office 
            if (selectedValue == 0) {
              
                debugger;
                document.getElementById('officeCode').style.display = "none";
                document.getElementById('btnCorporateOffice').click();
            }
            else {
                document.getElementById('officeCode').style.display = "block";
              //  document.getElementById('ContentPlaceHolder1_btnCorporateOffice').style.display = "none";
            }
        }
       
        
       
        

    //       document.getElementById("txtsFullName")
    //.addEventListener("keyup", function (event) {
    //    event.preventDefault();
    //    if (event.keyCode === 13) {
    //        document.getElementById("btnSearch").click();
    //    }
    //});

        //$("#txtsFullName").keydown(function (event) {
        //    debugger;
        //    if (event.keyCode === 13) {
        //        $("#btnSearch").click();
        //    }
        //});

       


    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        

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

        .both th a {
            background: url(img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        #DropDownList1 {
    position: absolute;
    left: 252px;
}
        .span4 {
    margin-left: 0px!important;
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
                <div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">User View
                    </h3>
                    <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
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
            </div>

            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>User View</h4>
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
                                        <div class="container-fluid">
                                            <div style="float: right">
                                                <div class="span2">
                                                   <!-- --> 
                                                    <asp:Button ID="cmdNew" runat="server" Text="New User"
                                                        CssClass="btn btn-success" OnClick="cmdNew_Click" /><br />
                                                </div>

                                            </div>
                                            <div class="space20">
                                            </div>

                                            <div class="span5" style="border-left:-200px!important" >
                                                 <div class="control-group">
                                                    <label  class="control-label">Select Office</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbOfficeType" runat="server" 
                                                                TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="cmb_OfficeIndexChanged">
                                                                 <%--<asp:ListItem Text="--Select" Value="-1"> --Select-- </asp:ListItem>
                                                                <asp:ListItem Text="Corporate Office" Value="0"> Corporate Office </asp:ListItem>
                                                                <asp:ListItem Text="Others" Value="1"> Others </asp:ListItem>--%>
                                                                <asp:ListItem Text="--Select" Value="0"> --Select-- </asp:ListItem>
                                                                <asp:ListItem Text="Corporate Office" Value="1"> Corporate Office </asp:ListItem>
                                                                <asp:ListItem Text="Others" Value="2"> Others </asp:ListItem>


                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                </div>

                                             <div class="space20">
                                            </div>

                                      <!--      <div id="officeCode"> -->

                                            <div class="span5">


                                                <div class="control-group">
                                                    <label class="control-label">Circle Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:DropDownList ID="cmbCircle" runat="server" AutoPostBack="true"
                                                                TabIndex="1" OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Division Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true"
                                                                TabIndex="2" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>



                                            <div class="span5">

                                                <div class="control-group">
                                                    <label class="control-label">Sub Division Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbsubdivision" runat="server" AutoPostBack="true"
                                                                TabIndex="3" OnSelectedIndexChanged="cmbsubdivision_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Section Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbSection" runat="server" AutoPostBack="true" TabIndex="4" OnSelectedIndexChanged="cmbSection_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hdfDivision" runat="server" />
                                                            <asp:HiddenField ID="hdfSubdivision" runat="server" />
                                                            <asp:HiddenField ID="hdfSection" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>




                                            </div>

                                              <!--  </div>-->

                                            <div class="span20"></div>

                                            <div class="text-center">

                                                <div class="span5"></div>

                                                <div class="span1">
                                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" TabIndex="11"
                                                        CssClass="btn btn-danger" OnClick="cmdReset_Click" /><br />
                                                </div>
                                                <div class="span1">
                                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-success"
                                                        OnClick="Export_click" /><br />
                                                </div>

                                                 <div class="span1">

                                                    <%-- <button id="btnCorporateOffice" > </button>--%>
                                                  <%--  <asp:Button ID="btnCorporateOffice" runat="" Text="Load Corporate Office" CssClass="btn btn-success"
                                                        style="display:none"/><br />--%>
                                                </div>


                                                <div class="space20"></div>
                                            </div>
                                            <div class="span20"></div>
                                            <div>

                                                <asp:GridView ID="grdUser"
                                                    AutoGenerateColumns="false" PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                                    runat="server" OnPageIndexChanging="grdUser_PageIndexChanging"
                                                    OnRowCommand="grdUser_RowCommand" OnRowDataBound="grdUser_RowDataBound"
                                                    ShowFooter="True" OnSorting="grdUser_Sorting" AllowSorting="true">
                                                    <HeaderStyle CssClass="both" />
                                                    <Columns>
                                                        <asp:TemplateField AccessibleHeaderText="US_ID" HeaderText="ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("US_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField AccessibleHeaderText="US_FULL_NAME" HeaderText="Name" SortExpression="US_FULL_NAME">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFullName" runat="server" Text='<%# Bind("US_FULL_NAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <%--onkeypress="return enterSearchBox(event)"--%>
                                                                <asp:TextBox ID="txtsFullName" runat="server" Width="120px" placeholder="Enter Name" ToolTip="Enter Name to Search" onkeypress="return character(event)"  onchange = "cleanSpecialAndNum(this)" ></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>

                                                         <asp:TemplateField AccessibleHeaderText="US_MOBILE_NO" HeaderText="Mobile No">

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("US_MOBILE_NO") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                            </ItemTemplate>
                                                             <%--onkeypress="return enterSearchBox(event)"--%>
                                                              <FooterTemplate>
                                                                <asp:TextBox ID="txtMobileNumber" runat="server" Width="120px" placeholder="Enter Mobile Number" ToolTip="Enter Mobile Number to Search" onkeypress="return onlyNumbers(event)"  onchange = "cleanSpecialAndalfabets(this)"  ></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField AccessibleHeaderText="US_EMAIL" HeaderText="Email Id">

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("US_EMAIL") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                            </ItemTemplate>
                                                              <FooterTemplate>
                                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>

                                                       
                                                        <asp:TemplateField AccessibleHeaderText="RO_NAME" HeaderText="Role Name" SortExpression="RO_NAME">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRole" runat="server" Text='<%# Bind("RO_NAME") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                                            </ItemTemplate>
                                                          
                                                        </asp:TemplateField>

                                                        <asp:TemplateField AccessibleHeaderText="US_DESG_ID" HeaderText="Designation" SortExpression="US_DESG_ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Bind("US_DESG_ID") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtsDesignation" runat="server" Width="100px" placeholder="Enter Designation" ToolTip="Enter Designation to Search" Visible="false"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Location" SortExpression="OFF_NAME">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOfficeName" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;" Width="200px"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtOfficeName" runat="server" Width="150px" placeholder="Enter Designation" ToolTip="Enter Designation to Search" Visible="false"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create"
                                                                        Width="12px" />
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("US_STATUS1") %>'></asp:Label>
                                                                <center>
                                                                    <asp:ImageButton Visible="false" ID="imgDeactive" runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status"
                                                                        ToolTip="Click to Enable User" OnClientClick="return ConfirmStatus('Enable');" Width="10px" />
                                                                    <asp:ImageButton Visible="false" ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif" CommandName="status"
                                                                        ToolTip="Click to Disable User" OnClientClick="return ConfirmStatus('Disable');" />
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Delete" Visible="false">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:ImageButton ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                                        Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                                        CausesValidation="false" />
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>

                                                </asp:GridView>

                                                <ajax:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="cmdClose"
                                                    PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
                                                <div style="width: 100%; vertical-align: middle; height: 369px;" align="center">
                                                    <div style="display: none">
                                                        <asp:Button ID="btnshow" runat="server" Text="Button" />
                                                    </div>
                                                    <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="200px" Width="550px">
                                                        <div class="widget blue">
                                                            <div class="widget-title">
                                                                <h4>Give Reason </h4>
                                                                <div class="space20"></div>
                                                                <%--<div class="row-fluid">--%>
                                                                <div class="span1"></div>
                                                                <div class="space20">
                                                                    <div class="span1"></div>

                                                                    <div class="span5">

                                                                        <div class="control-group" style="font-weight: bold">
                                                                            <label class="control-label">Reason<span class="Mandotary"> *</span></label>

                                                                            <div class="controls">
                                                                                <div class="input-append" align="center">

                                                                                    <asp:TextBox ID="txtReason" runat="server" MaxLength="500" TabIndex="4" TextMode="MultiLine" Style="resize: none"
                                                                                        onkeyup="javascript:ValidateTextlimit(this,100)"></asp:TextBox>

                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div align="center">
                                                                            <div class="control-group" style="font-weight: bold">
                                                                                <label class="control-label">Effect From<span class="Mandotary"> *</span></label>
                                                                                <div class="controls">
                                                                                    <div class="input-append" align="center">

                                                                                        <asp:TextBox ID="txtEffectFrom" runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server"
                                                                                            CssClass="cal_Theme1" TargetControlID="txtEffectFrom" Format="dd/MM/yyyy">
                                                                                        </ajax:CalendarExtender>

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="span5">
                                                                            <div class="control-group" style="font-weight: bold">

                                                                                <div class="controls">
                                                                                    <div class="input-append">

                                                                                        <div class="span10">
                                                                                            <asp:Button ID="cmdSubmit" runat="server" CssClass="btn btn-primary"
                                                                                                OnClick="cmdSubmit_Click" TabIndex="10" Text="Submit" />
                                                                                        </div>
                                                                                        <div class="span1">
                                                                                            <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-primary"
                                                                                                TabIndex="10" Text="Close" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                        <div class="space20" align="center">

                                                                            <div class="form-horizontal" align="center">
                                                                                <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                                                                            </div>


                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="space20"></div>
                                                            <div class="space20"></div>

                                                        </div>
                                                    </asp:Panel>
                                                </div>


                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>


                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>



                    <!-- END PAGE HEADER-->
                    <!-- BEGIN PAGE CONTENT-->



                    <!-- END PAGE CONTENT-->
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul><li>
                   This Web Page Can be used To View Existing User Details and To Add New User</li>
                 <li> New User Can Be Added By Clicking New User Button</li>
                 <li> User Can Be Enabled/Disabled By clicking Status Radio Button
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
    <script type="text/javascript">
        
</script>
   

 

</asp:Content>

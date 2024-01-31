<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Incharge.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.Incharge" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <script  src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>

    <script type="text/javascript">
        function onlynumbers(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode < 48 || charCode > 57) {

                return false;
            }
            return true;
        }
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
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <h3 class="page-title">Incharge Create
                                        <div style="float:right;margin-top:20px;margin-right:12px" >
                    <asp:Button ID="cmdclose" runat="server" Text="Close" 
                            CssClass="btn btn-primary" onclick="cmdClose_Click" />
                  </div>  
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
                            <h4><i class="icon-reorder"></i>Incharge Create</h4>
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
                                        <%--<div class="container-fluid">--%>
                                        <div class="span1"></div>
                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">Circle Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:DropDownList ID="cmbCircle" runat="server">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Division Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbDivision" runat="server">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Sub Division Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbsubdivision" runat="server" >
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Section Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbSection" runat="server">
                                                            </asp:DropDownList>
                                              
                                                        </div>
                                                    </div>
                                                </div>
                                          

                                              <!--  </div>-->

                                         
                                                <div class="control-group">
                                                    <label class="control-label">Employee Login Name</label>
                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:DropDownList ID="cmbempname" runat="server" AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbempname_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">
                                                   Employee Name</label>
                                                    <div class="controls">
                                                   <div class="input-append">

                                                            <asp:DropDownList ID="cmbname" runat="server">
                                                            </asp:DropDownList>

                                                        </div>
                                                        </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Actual Role</label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbActualRole" runat="server" >
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Incharge Role<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbInchargeRole" runat="server"  AutoPostBack="true" TabIndex="1"
                                                            OnSelectedIndexChanged="cmbInchargeRole_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                              
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="span5">
                                                <div class="control-group">
                                                    <label class="control-label">Charge Handover Emplyee<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">

                                                            <asp:DropDownList ID="cmbHandoverEmp" runat="server">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">
                                                   Charge From Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtfromdate"  runat="server"  ></asp:TextBox>
                                                         <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" TargetControlID="txtfromdate"
                                                            Format="dd/MM/yyyy" >
                                                        </ajax:CalendarExtender>
                                                    </div>

                                                </div>
                                                </div>

                                                      <div class="control-group">
                                                    <label class="control-label">
                                                   Charge To Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txttodate"  runat="server"></asp:TextBox>
                                                         <ajax:CalendarExtender ID="OMDateCalender" runat="server" CssClass="cal_Theme1" TargetControlID="txttodate"
                                                            Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>

                                                </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Remarks</label>
                                                    <div class="controls">
                                                     <div class="input-append">
                                                        <asp:TextBox ID="txtRemakrs" Maxlength="100"  runat="server"></asp:TextBox>
                                                    </div>
                                                    </div>
                                                </div>
                                         

                                                <div class="control-group">
                                                    <label class="control-label">
                                                   Auto Generated OM Number</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtAutoOmnumber"  runat="server"></asp:TextBox>                                                      
                                                    </div>

                                                </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">
                                                   OM Number<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOmnumber" Maxlength="10"  runat="server" onkeypress="return onlynumbers(event)"></asp:TextBox>                                                      
                                                    </div>

                                                </div>
                                                </div>

                                                      <div class="control-group">
                                                    <label class="control-label">
                                                   OM Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtomdate"  runat="server"></asp:TextBox>
                                                         <ajax:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1" TargetControlID="txtomdate"
                                                            Format="dd/MM/yyyy">
                                                        </ajax:CalendarExtender>
                                                    </div>

                                                </div>
                                                </div>

                                                
                                            </div>

                                            <div class="span20"></div>

                                            <div class="text-center">

                                                <div class="span5"></div>

<%--                                                <div class="span1">
                                                    <asp:Button ID="cmdload" runat="server" Text="Load" TabIndex="11"
                                                        CssClass="btn btn-success" OnClick="cmdLoad_click" /><br />
                                                </div>--%>


                                                 <div class="span1">
                                                </div>


                                                <div class="space20"></div>
                                            </div>
                                            <div class="span20"></div>
                                        
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>


                                           <%-- </div>--%>

                                        </div>

                                      <div  class="text-center" align="center">

                                      
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click"  CssClass="btn btn-success" />

                                       <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                                            onclick="cmdReset_Click"  CssClass="btn btn-danger" /><br/>
                                   
                                                <div class="span7"></div>
                                 
                                            
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
                   This Web Page Can be used To Create Incharge Details</li>
                 <li>Once Fill All The Mandatory Fields Please Click On Save Button To Save The Details</li>
                </ul>
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

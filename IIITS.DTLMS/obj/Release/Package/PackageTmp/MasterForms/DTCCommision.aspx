<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DTCCommision.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.DTCCommision" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">

        function preventMultipleSubmissions() {
            $('#<%=cmdSave.ClientID %>').prop('disabled', true);
        }
        window.onbeforeunload = preventMultipleSubmissions;

    </script>

    <script src="../Scripts/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ValidateMyForm() {
            if (document.getEleme.ntById('<%= txtDTCCode.ClientID %>').value.trim() == "") {
                alert('Enter Valid  DTC Code')
                document.getElementById('<%= txtDTCCode.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtDTCName.ClientID %>').value.trim() == "") {
                alert('Enter Valid DTC Name')
                document.getElementById('<%= txtDTCName.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtOMSection.ClientID %>').value.trim() == "") {
                alert('Enter OM Section')
                document.getElementById('<%= txtOMSection.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtTCCode.ClientID %>').value.trim() == "") {
                alert('Enter valid DTr Code')
                document.getElementById('<%= txtTCCode.ClientID %>').focus()
                return false
            }
        }

        function DisplayFullImage(ctrlimg) {
            txtCode = "<HTML><HEAD>"
      + "</HEAD><BODY TOPMARGIN=0 LEFTMARGIN=0 MARGINHEIGHT=0 MARGINWIDTH=0><CENTER>"
      + "<IMG src='" + ctrlimg.src + "' BORDER=0 NAME=FullImage "
      + "onload='window.resizeTo(document.FullImage.width,document.FullImage.height)'>"
      + "</CENTER>"
      + "</BODY></HTML>";
            mywindow = window.open('', 'image', '');
            mywindow.document.open();
            mywindow.document.write(txtCode);
            mywindow.document.close();

        }
        //From/Todate allow to enter nums /
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 47)
                return false;

            return true;
        }

        function onlyAlphabets(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if ((charCode >= 97 && charCode <= 122) || (charCode >= 65 && charCode <= 90) ||
                (charCode >= 48 && charCode <= 57) || (charCode == 45) || (charCode == 32))
                return true;
            else
                return false;

        }
        function onlyAlphabetsformeter(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if ((charCode >= 97 && charCode <= 122) || (charCode >= 65 && charCode <= 90) ||
                (charCode >= 48 && charCode <= 57))
                return true;
            else
                return false;

        }

        //From/Todate  allow only Numbers / to paste 
        function cleanSpecialAndChar(t) {
            debugger;
            t.value = t.value.toString().replace(/[^/0-9\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }

        function onlyDotNumbers(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if ((charCode == 46) || (charCode >= 48 && charCode <= 57))
                return true;
            else
                return false;
        }

        function CleanSpecialSymbols(t) {
            debugger;
            t.value = t.value.toString().replace(/[^.0-9a-zA-Z\n\r]+/g, '');
            //alert(" Special charactes and characters are not allowed!");
        }

        function AvoidComma(event) {
            var k = event ? event.which : window.event.keyCode;
            if (k == 44) return false;
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"> 
 </asp:ScriptManager>--%>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->

                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Commissioning of DTC
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
                    <asp:Button ID="cmdClose" runat="server" Text="DTC View"
                        OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary"
                        TabIndex="26" OnClick="cmdClose_Click" />
                </div>

            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
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
                                                <label class="control-label">Select Feeder</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbFeeder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbFeeder_SelectedIndexChanged">
                                                            <%-- AutoPostBack="True" onselectedindexchanged="cmbFeeder_SelectedIndexChanged"--%>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="control-group">
                                                <label class="control-label">DTC Code<span class="Mandotary"> *</span></label>
                                                <asp:Label ID="lblAutoDTCCode" runat="server" ForeColor="Red"></asp:Label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:HiddenField ID="hdfTcCode" runat="server" />
                                                        <asp:HiddenField ID="hdfDTRImagePath" runat="server" />
                                                        <asp:HiddenField ID="hdfDTCImagePath" runat="server" />
                                                        <asp:HiddenField ID="hdfDTCPath" runat="server" />
                                                        <asp:HiddenField ID="hdfDTRNamePlatePath" runat="server" />
                                                        <asp:TextBox ID="txtDTCId" runat="server" onkeypress="return OnlyNumber(event)" MaxLength="4" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtDTCCode" runat="server" MaxLength="6" TabIndex="1"></asp:TextBox>


                                                        <asp:TextBox ID="txtNamePlatePhotoPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <!--name plate   -->
                                                        <asp:TextBox ID="txtSSPlatePath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <!--ss  code (6gdigit) -->
                                                        <asp:TextBox ID="txtDTLMSDTCPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <!--dtc code  -->
                                                        <asp:TextBox ID="txtOLDDTCPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtIPDTCPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtInfosysPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtDTCPath" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <!--dtc  structure -->
                                                        <asp:TextBox ID="txtTempDTCCode1Path" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtTempDTCCode2Path" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="DtcCodeApprove" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="Dtid" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="Ltstatus" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="Status" runat="server" Width="20px" Visible="false"></asp:TextBox>


                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">DTC Name<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtDTCName" runat="server" MaxLength="50" onpaste="return false;" onkeypress="return onlyAlphabets(event);"
                                                            TabIndex="2"></asp:TextBox><br />
                                                        <asp:LinkButton ID="lnkDTCHistory" runat="server"
                                                            Style="font-size: 12px; color: Blue" OnClick="lnkDTCHistory_Click">View DTC History</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">O & M Section<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtOMSection" runat="server" MaxLength="10" TabIndex="3" onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>
                                                        <asp:Button ID="btnOmSearch" runat="server" Text="S"
                                                            CssClass="btn btn-primary" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Internal Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtWOslno" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtWFOId" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtActiontype" runat="server" Width="20px" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtInternalCode" runat="server" MaxLength="5" TabIndex="4" onpaste="return false;" onchange="return cleanSpecialAndChar(this)"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Connected KW</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtConnectedKW" runat="server" MaxLength="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);" TabIndex="5"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Connected HP</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtConnectedHP" runat="server" MaxLength="6"
                                                            onkeypress="javascript:return AllowNumber(this,event);" TabIndex="6"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">KWH Reading</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtKWHReading" runat="server" MaxLength="10" autocomplete="off"
                                                            onkeypress="javascript:return AllowNumber(this,event);" TabIndex="7" onpaste="return false;"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>



                                            <div id="latlong" runat="server">

                                                <div class="control-group">
                                                    <label class="control-label">Latitude <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtLatitude" onkeypress="return onlyDotNumbers(event)" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Longitude <span class="Mandotary">*</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="txtlongitude" onkeypress="return onlyDotNumbers(event)" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">DTC Metering<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDTCMetered" runat="server" AutoPostBack="true"
                                                            TabIndex="42" OnSelectedIndexChanged="cmbDTCMetered_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Meter Status<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbMeterstatus" runat="server"
                                                            TabIndex="41" AutoPostBack="true"
                                                            OnSelectedIndexChanged="Meterstatus_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Modem<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbModem" runat="server"
                                                            TabIndex="41">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group" runat="server" id="Ltdiv" style="display: none">
                                                <div class="control-group">
                                                    <label class="control-label">Meter Recording<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:DropDownList ID="cmbMeterRecording" runat="server"
                                                                TabIndex="41">
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Remarks<span class="Mandotary"> *</span></label>
                                                    <div class="controls">
                                                        <div class="input-append">
                                                            <asp:TextBox ID="Remarks" runat="server" Style="resize: none" TextMode="MultiLine" TabIndex="20" onkeyup="javascript:ValidateTextlimit(this,200)"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="span5">
                                            <div class="control-group">
                                                <label class="control-label">DTr Code<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTCCode" runat="server" MaxLength="10" Enabled="false"
                                                            onkeypress="javascript:return OnlyNumber(event);" TabIndex="17"></asp:TextBox>
                                                        <asp:Button ID="cmdSearch" runat="server" Text="S" Visible="false"
                                                            CssClass="btn btn-primary" OnClick="cmdSearch_Click" TabIndex="18" />
                                                        <asp:TextBox ID="txtOldTCCode" runat="server" Visible="false" Width="20px"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Make<span class="Mandotary"> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtTCMake" runat="server" Enabled="false" TabIndex="19"></asp:TextBox>
                                                        <asp:TextBox ID="txtMakeId" runat="server" Visible="false"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTr Capacity(in KVA)<span class="Mandotary"> </span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCapacity" runat="server" Enabled="false" TabIndex="20"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>


                                            <%-- <div class="control-group">
                        <label class="control-label">Connection Date<span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtConnectionDate" runat="server" TabIndex="21" MaxLength="10" ></asp:TextBox>
                                     <asp:CalendarExtender ID="txtConnectionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                       TargetControlID="txtConnectionDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                   
                            </div>
                        </div>
                    </div>--%>

                                            <div class="control-group">
                                                <label class="control-label">DTC Commission Date<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtCommisionDate" runat="server" TabIndex="24" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtCommisionDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtCommisionDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Last Service Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtServiceDate" runat="server" TabIndex="23" MaxLength="10" onkeypress="return onlyNumbers(event)" onchange="return cleanSpecialAndChar(this)"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txtServiceDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtServiceDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Project/Scheme Type</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbprojecttype" runat="server" TabIndex="14">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Feeder Change Date</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtFeederChngDate" runat="server" TabIndex="25" autocomplete="off" MaxLength="10"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            TargetControlID="txtFeederChngDate" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Meter Make<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmdmake" runat="server"
                                                            TabIndex="41" AutoPostBack="true" OnSelectedIndexChanged="Metermake_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">CT Ratio<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCtratio" runat="server"
                                                            TabIndex="41">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Wiring<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbWiring" runat="server"
                                                            TabIndex="41">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Meter SL No<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtslno" runat="server"
                                                            onkeyup="this.value=this.value.toUpperCase()" TabIndex="20"
                                                            MaxLength="10" onkeypress="return onlyAlphabetsformeter(event);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <label class="control-label">Year Of Manufacture<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="Manufactureyear" runat="server"
                                                            TabIndex="41">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>


                                            <div class="control-group">
                                                <%--<label id="Label5" class="control-label" runat="server">Requires Painting  </label>--%>
                                                <div class="control control-label">
                                                    <asp:CheckBox ID="chkRequiresPainting" runat="server" Text="Requires Painting of DTC as per the Specifications" OnCheckedChanged="chkRequiresPainting_CheckedChanged" AutoPostBack="true" />
                                                </div>
                                            </div>


                                        </div>





                                    </div>

                                </div>
                                <div class="span1"></div>
                            </div>
                            <div class="space20"></div>

                            <div class="form-horizontal" align="center">

                                <div class="span3"></div>
                                <div class="span2">
                                    <asp:Button ID="cmdSave" runat="server" Text="Save and Continue"
                                        OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-primary"
                                        TabIndex="26" OnClick="cmdSave_Click" />
                                </div>
                                <%-- <div class="span1"></div>--%>
                                <div class="span1">
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset"
                                        CssClass="btn btn-primary" TabIndex="27" OnClick="cmdReset_Click" /><br />
                                </div>
                                <div class="span6" align="right">
                                    <asp:Button ID="cmdNext" runat="server" Text="Next>>"
                                        CssClass="btn btn-large" TabIndex="28" Visible="false"
                                        OnClick="cmdNext_Click" /><br />
                                </div>
                                <div class="span7"></div>
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                            </div>

                            <div class="form-horizontal" runat="server" id="photoUpload">

                                <div class="row-fluid">
                                    <div class="space5"></div>
                                    <div class="span5">

                                        <div id="finalDTCCode" runat="server">

                                            <div class="control-group">
                                                <label class="control-label">DTC Code (DTLMS) Photo<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupDTCStructure" runat="server" AllowMultiple="False"
                                                            TabIndex="19" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">DTC  Photo<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupDTCCode" runat="server" AllowMultiple="False"
                                                            TabIndex="19" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="tempDTCCode" runat="server" visible="false">
                                            <div class="control-group">
                                                <label class="control-label">Temp DTC Photo1<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupTempDTCCode1" runat="server" AllowMultiple="false" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Temp DTC Photo2<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:FileUpload ID="fupTempDTCCode2" runat="server" AllowMultiple="false" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="span5">
                                        <div class="control-group">
                                            <label class="control-label">SS Plate Photo<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:FileUpload ID="fupSSplatePhoto" runat="server" AllowMultiple="False"
                                                        TabIndex="19" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label">Name Plate Photo<span class="Mandotary"> *</span></label>
                                            <div class="controls">
                                                <div class="input-append">
                                                    <asp:FileUpload ID="fupNamePlatePhoto" runat="server" AllowMultiple="False"
                                                        TabIndex="19" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>

                            </div>


                            <div class="form-horizontal">
                                <div class="row-fluid">
                                    <div class="span1"></div>
                                    <%-- entire structure photo --%>
                                    <div class="span5">
                                        <div class="control-group" runat="server" id="dvDTCCode" style="display: none">
                                            <div align="center">
                                                <label>DTC Code (DTLMS) Photo </label>
                                                <div align="center">
                                                    <asp:Image ID="imgDTCCode" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                        runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%-- painted DTC code photo --%>
                                    <div class="span5">
                                        <div class="control-group" runat="server" id="dvDTCPhoto" style="display: none">
                                            <div align="center">
                                                <label>DTC Photo </label>
                                                <div align="center">
                                                    <asp:Image ID="imgDTCPhoto" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                        runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="span5">
                                        <div class="control-group" runat="server" id="dvDTrCode" style="display: none">
                                            <div align="center">
                                                <label>DTr Code Photo </label>
                                                <div align="center">
                                                    <asp:Image ID="imgDTrCode" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                        runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="span5">
                                        <div class="control-group" runat="server" id="divDTRNamePlate" style="display: none">
                                            <div align="center">
                                                <label>DTr Name Plate  Photo </label>
                                                <div align="center">
                                                    <asp:Image ID="imgDTRNamePlate" BorderColor="lightgray" BorderWidth="3px" Height="58px" Width="400px"
                                                        runat="server" ImageUrl="../img/Manual/NoImage.png" class="fancybox" onmousedown="DisplayFullImage(this);" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- <div class="span5">
                                            <div class="control-group" runat="server" id="divDTRNamePlate" style="display:none">
                                                <div align="center">
                                                     <label >DTR Plate Photo </label>
                                                     <div align="center"> 
                                                         <asp:Image ID="imgDTRNamePlate"  BorderColor="lightgray" BorderWidth="3px"  Height="300px" Width="400px" 
                                                          runat="server" ImageUrl = "../img/Manual/NoImage.png" class="fancybox"    />
                                                        </div> 
                                                </div>
                                            </div>
                                               </div>--%>
                                </div>
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

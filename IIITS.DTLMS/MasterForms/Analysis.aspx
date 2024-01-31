<%@ Page Title="" Language="C#"  MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Analysis.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.Analysis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script   type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
     <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            debugger;
            //alert(ContentPlaceHolder1_currTab.val);
            retainCurrenTab();
            <%--if ($("#<%= currTab.ClientID %>").val() != "") {
                $(currTab).addClass('current');
                $("#" + currTab).addClass('current');
            }--%>
            $('ul.tabs li').click(function () {

                debugger;
                var tab_id = $(this).attr('data-tab');
                $('#<%=currTab.ClientID %>').val(tab_id);
                //currTab = tab_id
<%--                alert(document.getElementById('<%=currTab.ClientID%>').value);--%>

                $('ul.tabs li').removeClass('current');
                $('.tab-content').removeClass('current');

              //  alert(this);
                $(this).addClass('current');
                $("#" + tab_id).addClass('current');
            })

        })

        function retainCurrenTab() {
            debugger;
            // var tabid = $("<%= currTab.ClientID %>").val();
            var tabid = document.getElementById('<%=currTab.ClientID%>').value;
            if (tabid != '')
            {
                // remove 
                $('ul.tabs li').removeClass('current');
                $('.tab-content').removeClass('current');

                // add
                $("#l" + tabid).addClass('current');
                $("#" + tabid).addClass('current');

            }
             
        }
    </script>
    <style type="text/css">
        .container {
            width: 800px;
            margin: 0 auto;
        }



        ul.tabs {
            margin: 0px;
            padding: 0px;
            list-style: none;
        }

            ul.tabs li {
                background: none;
                color: #222;
                display: inline-block;
                padding: 10px 15px;
                cursor: pointer;
            }

                ul.tabs li.current {
                    background: #ededed;
                    color: #222;
                }

        .tab-content {
            display: none;
            background: #ededed;
            padding: 15px;
        }

            .tab-content.current {
                display: inherit;
            }
    </style>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <br />

    <div class="container"  style="width:auto">
       <asp:HiddenField ID="currTab" runat="server" />
       
        <ul class="tabs">
            <li class="tab-link current" id="ltab-1" data-tab="tab-1">Failure Analysis</li>
            <li class="tab-link "  id="ltab-2"  data-tab="tab-2">Replacement Timeline</li>
            <li class="tab-link"  id="ltab-3"  data-tab="tab-3">WGP , AGP and WRGP</li>
            <li class="tab-link"  id="ltab-4"   data-tab="tab-4">Repairer Cost </li>
            <li class="tab-link"  id="ltab-5"   data-tab="tab-5"> New DTC Commission</li>
        </ul>
        
        <!-- Failure Analysis   --> 
        <div id="tab-1" class="tab-content current">
            
            <div class="widget blue">

                <div class="span5">
                    <div class="control-group">
                                                <label class="control-label">
                                                  Circle <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID ="cmbCircle" runat="server"  AutoPostBack="true"  OnSelectedIndexChanged="cmbCircle_SelectedIndexChanged" > 
                                                            </asp:DropDownList>
                                                    </div>
                                                </div>
                  </div>
                     <div class="control-group">
                                                <label class="control-label">
                                                  Division <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID ="cmbDivision" runat="server"  AutoPostBack="true" > 
                                                            </asp:DropDownList>
                                                    </div>
                                                </div>
                  </div>

                     <div class="control-group">
                                                <label class="control-label">
                                                  Capacity <%--<span class="Mandotary">*</span>--%></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID ="cmbCapacity" runat="server"  AutoPostBack="true" > 
                                                            </asp:DropDownList>
                                                    </div>
                                                </div>
                  </div>


                    </div>
               
                <div class ="span4">
                  <asp:GridView ID="grdFeeder" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowFooter="True" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found"
                                    AllowPaging="true" AllowSorting="true">
                                <HeaderStyle CssClass="both" /> 
                                    <Columns>
                                         <asp:TemplateField AccessibleHeaderText="SD_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFeederId" runat="server" Text='<%# Bind("FD_FEEDER_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="SD_ID" HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFeederId" runat="server" Text='<%# Bind("FD_FEEDER_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        </Columns>

                      </asp:GridView>


                </div>
                
                </div>

        </div>

        <!--Replacement Timeline -->
        <div id="tab-2" class="tab-content">
            <div class="control-group">
                <label class="control-label">
                    Station Name<span class="Mandotary"> *</span></label>
                <div class="controls">
                    <div class="input-append">
                        <asp:TextBox ID="txtStatName" runat="server" MaxLength="100"></asp:TextBox>

                    </div>
                </div>
            </div>
           
            <div class="text-center" align="center">

                <asp:Button ID="cmdSave" runat="server" Text="Save" OnClientClick="javascript:return Validate()"
                    CssClass="btn btn-success" OnClick="cmdSave_Click" />
            </div>
             </div>
        <!-- WGP , AGP and WRGP -->
        <div id="tab-3" class="tab-content">
            Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
        </div>
          <!-- Repairer Cost-->
        <div id="tab-4" class="tab-content">
            Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
        </div>
         <!-- New DTC Commission-->
          <div id="tab-5" class="tab-content">
            Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
        </div>

    </div>
    <!-- container -->

</asp:Content>



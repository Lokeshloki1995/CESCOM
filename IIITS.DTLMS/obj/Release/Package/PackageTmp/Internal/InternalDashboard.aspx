<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="InternalDashboard.aspx.cs" Inherits="IIITS.DTLMS.InternalDashboard"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
      <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
               
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Dashboard
                   </h3>
                
                   <!-- END PAGE TITLE & BREADCRUMB-->
               </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">

            <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Dashboard</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">

                      <ul class="breadcrumb" style="font-weight:bold">
                       <li>

                           <a href="StatusReport.aspx?RefId=true" target="_blank">Current Status as On 
                               <asp:Label ID="lblDate" runat="server" ></asp:Label> <span style="color:Blue">  (View Details..)</span></a>
                           
                       </li>                      
                       <li class="pull-right search-wrap">
                           <form action="search_result.html" class="hidden-phone">
                               <div class="input-append search-input-area">
                                  <%-- <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>--%>
                                  
                               </div>
                               <a target="_blank" href="http://cescdtlms.com"> <span style="font-weight:bold">Main Application</span></a> 
                           </form>
                       </li>
                   </ul>

                <!--BEGIN METRO STATES-->
                <div class="metro-nav">

                  <div class="metro-nav-block nav-block-orange" runat="server" id="dvOperator" style="display:none">
                        <a  href="#">
                            <i class="icon-user"></i>
                            <div class="info"> <asp:Label ID="lblOperatorCount" runat="server" ></asp:Label></div>
                            <div class="status" >No. of Operator</div>
                        </a>
                    </div>

                     <div class="metro-nav-block nav-block-orange">
                        <a  href="#">
                            <i class="icon-bar-chart"></i>
                            <div class="info"> <asp:Label ID="lblCurrentTotalEnum" runat="server" ></asp:Label></div>
                            <div class="status" >Total Acitivity</div>
                        </a>
                    </div>
                    <div class="metro-nav-block nav-olive">
                        <a  href="#">
                            <i class="icon-shield"></i>
                            <div class="info"><asp:Label ID="lblCurrentQCDone" runat="server" ></asp:Label></div>
                            <div class="status">QC Done</div>
                        </a>
                    </div>
                    <div class="metro-nav-block nav-block-yellow">
                        <a  href="#">
                            <i class="icon-check-minus"></i>
                            <div class="info"><asp:Label ID="lblCurrentPending" runat="server" ></asp:Label></div>
                            <div class="status">Pending for QC</div>
                        </a>
                    </div>

                     <div class="metro-nav-block nav-light-brown" runat="server" id="Div1" >
                        <a  href="#">
                            <i class="icon-remove-sign"></i>
                            <div class="info"><asp:Label ID="lblCurrentReject" runat="server" ></asp:Label></div>
                            <div class="status">Reject</div>
                        </a>
                    </div>

                    <div class="metro-nav-block nav-block-red" runat="server" id="Div2" >
                        <a  href="#">
                            <i class="icon-pause"></i>
                            <div class="info"><asp:Label ID="lblCurrentPendingClarif" runat="server" ></asp:Label></div>
                            <div class="status">Pending for Clarification</div>
                        </a>
                    </div>

                </div>
                  <div class="space10"></div>
                   <div class="space10"></div>
                 <ul class="breadcrumb" style="font-weight:bold">
                       <li>
                           <a href="StatusReport.aspx?RefId=false" target="_blank">Overall Status  <span style="color:Blue">  (View Details..)</span></a>
                           
                       </li>                      
                       <li class="pull-right search-wrap">
                           <form action="search_result.html" class="hidden-phone">
                               <div class="input-append search-input-area">
                                  <%-- <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>--%>
                                  
                               </div>
                               <%--<a target="_blank" href="http://192.168.4.22:72"> <span style="font-weight:bold">Main Application</span></a> --%>
                           </form>
                       </li>
                   </ul>

                  <div class="metro-nav">                  
                    <div class="metro-nav-block nav-block-orange">
                        <a  href="#">
                            <i class="icon-bar-chart"></i>
                            <div class="info"> <asp:Label ID="lblTotalEnum" runat="server" ></asp:Label></div>
                            <div class="status" >Total Activity</div>
                        </a>
                    </div>
                    <div class="metro-nav-block nav-olive">
                        <a  href="#">
                            <i class="icon-shield"></i>
                            <div class="info"><asp:Label ID="lblQCDone" runat="server" ></asp:Label></div>
                            <div class="status">QC Done</div>
                        </a>
                    </div>
                    <div class="metro-nav-block nav-block-yellow">
                        <a  href="#">
                            <i class="icon-check-minus"></i>
                            <div class="info"><asp:Label ID="lblQCPending" runat="server" ></asp:Label></div>
                            <div class="status">Pending for QC</div>
                        </a>
                    </div>

                     <div class="metro-nav-block nav-light-brown" runat="server" id="dvReject" >
                        <a  href="#">
                            <i class="icon-remove-sign"></i>
                            <div class="info"><asp:Label ID="lblReject" runat="server" ></asp:Label></div>
                            <div class="status">Reject</div>
                        </a>
                    </div>

                     <div class="metro-nav-block nav-block-red" runat="server" id="Div3" >
                        <a  href="#">
                            <i class="icon-pause"></i>
                            <div class="info"><asp:Label ID="lblPendingForClarif" runat="server" ></asp:Label></div>
                            <div class="status">Pending for Clarification</div>
                        </a>
                    </div>

                 </div>
               

                 <div class="space10"></div>
                   <div class="space10"></div>
                    <ul class="breadcrumb" style="font-weight:bold">
                       <li>
                            <%--<asp:Label ID="lblMonthwiseTitle" runat="server" ></asp:Label>--%>
                           <asp:LinkButton ID="lnkMonthWiseTitle" runat="server" 
                               onclick="lnkMonthWiseTitle_Click" >Custom  <span style="color:Blue" >  (View Details..)</span></asp:LinkButton>
                       </li>                      
                       <li class="pull-right search-wrap">
                           <form action="search_result.html" class="hidden-phone">
                               <div class="input-append search-input-area">
                                  <%-- <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>--%>
                                  
                               </div>
                               <%--<a target="_blank" href="http://192.168.4.22:72"> <span style="font-weight:bold">Main Application</span></a> --%>
                           </form>
                       </li>
                   </ul>

                    <div class="form-horizontal">
                        <div class="row-fluid">           
                            <div class="span3">
                             </div>
                        <div class="span3">                       
                             <div class="control-group">
                            <label class="control-label">From Date</label>
                            <div class="input-small">
                                <div class="input-append">
                                      <asp:TextBox ID="txtFromDate"  runat="server" MaxLength="15"></asp:TextBox>
                                     <ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy"
                                      TargetControlID="txtFromDate"></ajax:CalendarExtender>
                                </div>
                            </div>
                        </div>
                                                            
                     </div>
                        <div class="span3">                      
                       <div class="control-group">
                        <label class="control-label">To Date</label>
                        <div class="input-small">
                            <div class="input-append">
                                  <asp:TextBox ID="txtToDate"  runat="server" MaxLength="15"></asp:TextBox> 
                                  <asp:Button ID="cmdSearch" Text="Search" class="btn btn-primary" runat="server" onclick="cmdSearch_Click"  
                                       />
                                 <ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1" Format="dd/MM/yyyy" 
                                  TargetControlID="txtToDate"></ajax:CalendarExtender>
                            </div>
                        </div>
                    </div>                                          
                 </div>

                 
                         <div class="span3">                      
                         <div class="control-group">
                      
                        <div class="input-small">
                            <div class="input-append">
                                
                            </div>
                        </div>
                    </div>
                  
                         
                                           
                </div>
                 </div>


                 <div  class="row-fluid">
                    <div class="span3">
                             </div>
                     <div class="span3">                       
                             <div class="control-group">
                            <label class="control-label">Division</label>
                            <div class="input-small">
                                <div class="input-append">
                                  <asp:DropDownList ID="cmbDiv" runat="server" AutoPostBack="true"
                                   TabIndex="1" onselectedindexchanged="cmbDiv_SelectedIndexChanged" > </asp:DropDownList>   
                                </div>
                            </div>
                        </div>
                                                            
                     </div>
                    <div class="span3">                      
                       <div class="control-group">
                        <label class="control-label">Feeder</label>
                        <div class="input-small">
                            <div class="input-append">
                                   <asp:DropDownList ID="cmbFeeder" runat="server" AutoPostBack="true"
                                 TabIndex="1" onselectedindexchanged="cmbFeeder_SelectedIndexChanged"> </asp:DropDownList>   
                            </div>
                        </div>
                    </div>                                          
                 </div>

                 </div>

              <div class="space20"></div>
             </div>

                  <div class="metro-nav">                  
                    <div class="metro-nav-block nav-block-orange">
                        <a  href="#">
                            <i class="icon-bar-chart"></i>
                            <div class="info"> <asp:Label ID="lblTotalEnumDateWise" runat="server" ></asp:Label></div>
                            <div class="status" >Total Enumeration</div>
                        </a>
                    </div>
                    <div class="metro-nav-block nav-olive">
                        <a  href="#">
                            <i class="icon-shield"></i>
                            <div class="info"><asp:Label ID="lblQCdoneDateWise" runat="server" ></asp:Label></div>
                            <div class="status">QC Done</div>
                        </a>
                    </div>
                    <div class="metro-nav-block nav-block-yellow">
                        <a  href="#">
                            <i class="icon-check-minus"></i>
                            <div class="info"><asp:Label ID="lblPendingQCDateWise" runat="server" ></asp:Label></div>
                            <div class="status">Pending for QC</div>
                        </a>
                    </div>

                     <div class="metro-nav-block nav-light-brown" runat="server" id="Div4" >
                        <a  href="#">
                            <i class="icon-remove-sign"></i>
                            <div class="info"><asp:Label ID="lblRejectDateWise" runat="server" ></asp:Label></div>
                            <div class="status">Reject</div>
                        </a>
                    </div>

                     <div class="metro-nav-block nav-block-red" runat="server" id="Div5" >
                        <a  href="#">
                            <i class="icon-pause"></i>
                            <div class="info"><asp:Label ID="lblPendingClarifDateWise" runat="server" ></asp:Label></div>
                            <div class="status">Pending for Clarification</div>
                        </a>
                    </div>

                 </div>

                  <div class="space10"></div>
                   <div class="space10"></div>




                </div>
              </div>
          </div>
                
                <!--END METRO STATES-->
            </div>
              <div class="row-fluid">
                <!--BEGIN METRO STATES-->
           
             </div>
           
</asp:Content>

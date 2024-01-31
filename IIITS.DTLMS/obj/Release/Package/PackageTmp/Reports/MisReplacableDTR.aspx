<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="MisReplacableDTR.aspx.cs" Inherits="IIITS.DTLMS.Reports.MisReplacableDTR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
     <div class="container-fluid">
           <!-- BEGIN PAGE HEADER-->
           <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                      MIS Brand New / Released Good / Repaired Good DTR
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
</div>

         <div class="row-fluid">
             <div class="span12">
                 <div class="widget blue">
                     <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i> MIS Brand New / Released Good / Repaired Good DTR</h4> 
              <a href="#" style="float:right!important;padding:8px 7px 0 0px !important;color:#fff!important"data-toggle="modal" data-target="#myModal2" title="Click For Help" >Help <i class="fa fa-exclamation-circle" style="font-size:16px;color:white"></i></a> 
                            <span class="tools"><a href="javascript:;" ></a><a href="javascript:;"
                                ></a></span>
                        </div>
                      <div class="widget-body">
                            <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                        <div class="span1">
                                        </div>
                                         <div class="space20">
                                    </div>
                                    <div class="space20">
                                    </div>
                                         <div class="text-center" align="center">
                                         
                                  
                                            <asp:Button ID="cmdReport" runat="server" Text="Generate Report" CssClass="btn btn-primary"
                                                OnClick="cmdReport_Click" />
                                      
                                               <%-- <asp:Button ID="Button3" runat="server" Text="Reset" Visible="false" CssClass="btn btn-danger" 
                                                     onclick="BtnReset1_Click" /><br />--%>
                                          <br />

                                     
                                        <%--<asp:Button ID="Button2" Visible="false" runat="server" Text="Export Excel"  CssClass="btn btn-info" 
                                            onclick="Export_click" /><br />--%>
                                         
                                        
                                        <div class="span7">
                                        </div>
                                        <asp:Label ID="lblErrormsg" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                        </div>
                                    </div>
                                </div>
                          </div>


                 </div>



             </div>

           
         </div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="Qrywizard.aspx.cs" Inherits="IIITS.DTLMS.Query.Qrywizard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            <h3 class="page-title">
                Query Wizard
            </h3>
            <ul class="breadcrumb" style="display:none">
                       
                <li class="pull-right search-wrap">
                    <form action="" class="hidden-phone">
                        <div class="input-append search-input-area">
                            <input class="" id="appendedInputButton" type="text"/>
                            <button class="btn" type="button"><i class="icon-search"></i> </button>
                        </div>
                    </form>
                </li>
            </ul>
            <!-- END PAGE TITLE & BREADCRUMB-->
        </div>           
    </div>
    <!-- END PAGE HEADER-->
    <!-- BEGIN PAGE CONTENT-->
        
    <div class="row-fluid">
        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Query Wizard</h4>
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
                            <%-- <div class="span1"></div>--%>
                            <div class="span12">
                                <asp:TextBox ID="txtQuery" CssClass="input-xxlarge" style="width:1000px;height:250px;resize:none"  runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="space20"></div>
                            <div  class="form-horizontal" align="center">
                                <div class="span2"></div>
                                <div class="span2">
                                <asp:Button ID="cmdLoad" runat="server" Text="Load" CssClass="btn btn-primary" 
                                    Width="116px" onclick="cmdLoad_Click" />
                                    </div>

                                    <div class="span2">
                                    <asp:Button ID="cmdExport" runat="server" Text="Export" CssClass="btn btn-primary" 
                                    Width="116px" onclick="cmdExport_Click"  />
                                        </div>
                                    <div class="span2">
                                    <asp:Button ID="cmdClear" runat="server" Text="Clear" CssClass="btn btn-primary" 
                                    Width="116px" onclick="cmdClear_Click"  />
                                        </div>


                            </div>

                                </div>
                            </div>
                            </div>
                            </div>
                        </div>
            </div>                                   
    </div>
    <div class="row-fluid"  ID="divResult" runat="server" style="display:none">
        <div class="span12">
            <!-- BEGIN SAMPLE FORMPORTLET-->
            <div class="widget blue">
                <div class="widget-title">
                    <h4><i class="icon-reorder"></i>Result</h4>
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
                            <%-- <div class="span1"></div>--%>
                   
        
            <div class="space20"></div>
            <div id="Div1" style="overflow:scroll" runat="server">             
                        <asp:GridView ID="grdResult" 
                            ShowHeaderWhenEmpty="true"  EmptyDataText="No records Found"
                            CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="false"  
                            runat="server" >
                                
                        </asp:GridView>
            </div>
               
                            </div>
                           
                </div>





            </div>
            </div>
            </div>

        </div>
            </div>                                   
        </div>
    </div>          
           
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" ></asp:Label>
    
     
</asp:Content>

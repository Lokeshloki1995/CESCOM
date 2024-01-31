<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="StatusReport.aspx.cs" Inherits="IIITS.DTLMS.Internal.StatusReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div >
      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                    Enumeration Status Report
                   </h3>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
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
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> 
                                <asp:Label ID="lblEnumStatus" runat="server" Text="Enumeration Status Report"></asp:Label></h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                              
                                 <div style="float:right" >
                                <div class="span2">
                                   <asp:Button ID="cmdBack" runat="server" Text="Back" 
                                              CssClass="btn btn-primary" onclick="cmdBack_Click" /><br /></div>
                                     <asp:HiddenField ID="hdfFeederCode" runat="server" />
                                      <asp:HiddenField ID="hdfFromDate" runat="server" />
                                       <asp:HiddenField ID="hdfToDate" runat="server" />
                                     <asp:HiddenField ID="hdfRefId" runat="server" />
                                     <asp:HiddenField ID="hdfOfficeCode" runat="server" />
                                            </div>
                                  
                                    <div class="space20"> </div>
                                 
                                <!-- END FORM-->
                           
                        
                            <asp:GridView ID="grdStatusReport" 
                                AutoGenerateColumns="false"  PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                CssClass="table table-striped table-bordered table-advance table-hover" 
                                     AllowPaging="true" ShowFooter="true"
                                runat="server" onrowcommand="grdStatusReport_RowCommand" 
                                     onpageindexchanging="grdStatusReport_PageIndexChanging"  >
                                <Columns>
                                        <asp:TemplateField AccessibleHeaderText="OFFICECODE" HeaderText="OFFICECODE" Visible="false">                                
                                            <ItemTemplate>                                       
                                                <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("OFFICECODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="LOCATION" HeaderText="Location Name" >                                
                                        <ItemTemplate> 
                                            <asp:LinkButton ID="lnkLocation" runat="server" Text='<%# Bind("LOCATION") %>' style="word-break: break-all;" 
                                            width="200px" CommandName="view"></asp:LinkButton>                                
                                            <%--<asp:Label ID="lblLocation" runat="server" Text='<%# Bind("LOCATION") %>' style="word-break: break-all;" width="150px"></asp:Label>--%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                           <asp:TextBox ID="txtLocName" runat="server" placeholder="Enter Location Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>   
                                      
                                    <asp:TemplateField AccessibleHeaderText="FD_FEEDER_NAME" HeaderText="Feeder Name" Visible="false">                                
                                        <ItemTemplate>                    
                                            <asp:Label ID="lblFeeder" runat="server" Text='<%# Bind("FD_FEEDER_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                           <asp:TextBox ID="txtFeederName" runat="server" placeholder="Enter Feeder Name" CssClass="span12"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>                              
                                  
                                    <asp:TemplateField AccessibleHeaderText="TOTAL" HeaderText="Total Activity">                                   
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("TOTAL") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>  
                                         <FooterTemplate>
                                           <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>                                    
                                    </asp:TemplateField>

                                    <asp:TemplateField AccessibleHeaderText="QCPENDING" HeaderText="QC Pending">                          
                                        <ItemTemplate>
                                            <asp:Label ID="lblQCPending" runat="server" Text='<%# Bind("QCPENDING") %>' style="word-break:break-all" Width="180px"></asp:Label>
                                        </ItemTemplate>                                       
                                    </asp:TemplateField>

                                      <asp:TemplateField AccessibleHeaderText="QCDONE" HeaderText="QC Done">                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblQCDone" runat="server" Text='<%# Bind("QCDONE") %>' style="word-break: break-all;" width="100px"></asp:Label>
                                        </ItemTemplate>                                       
                                    </asp:TemplateField>

                                   <asp:TemplateField AccessibleHeaderText="QCREJECT" HeaderText="QC Reject" >                                    
                                        <ItemTemplate>
                                            <asp:Label ID="lblQCReject" runat="server" Text='<%# Bind("QCREJECT") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>                                          
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField AccessibleHeaderText="PENDING_CLAR" HeaderText="Pending for Clarification" >                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblPendingClar" runat="server" Text='<%# Bind("PENDING_CLAR") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField AccessibleHeaderText="ENUMTYPE" HeaderText="Activity Type" >                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblActivity" runat="server" Text='<%# Bind("ENUMTYPE") %>' style="word-break: break-all;" width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                
                                </Columns>

                            </asp:GridView>
                        
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                  <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
      </div>
      </div>



</asp:Content>

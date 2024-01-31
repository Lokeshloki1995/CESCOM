<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DownLoad.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.DownLoad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     function openTab(th) {
         window.open(th.name, '_blank');
     }

     function onlyNumbers(event) {
         var charCode = (event.which) ? event.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57))
             return false;

         return true;
     }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                  Downloads
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
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>Downloads</h4>
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
      
 <div class="span20">

                 <div class="control-group">
                       <asp:ImageButton ID="imgBtnEdit" runat="server" Height="60px" ImageUrl="~/img/Manual/pdfimage.jpg"
                         Width="60px" />
                 
                      
                            <div class="input-append"  color: #0099FF">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <asp:LinkButton ID="lnkDownload" runat="server" 
                               style="font-size:12px;color:Blue;font-weight:bold" 
                             onclick="lnkDownload_Click" ForeColor="#3399FF">DTLMS Web Application User Manual</asp:LinkButton>                     
                               
                           
                        </div>
                    </div>
     
                 <div class="control-group">
                       <asp:ImageButton ID="ImageButton1" runat="server" Height="60px" ImageUrl="~/img/Manual/pdfimage.jpg"
                         Width="60px" />
                 
                       
                            <div class="input-append">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <asp:LinkButton ID="lnkAndroidManual" runat="server" 
                                   style="font-size:12px;color:Blue;font-weight:bold" onclick="lnkAndroidManual_Click" 
                                      >DTLMS Android Application User Manual</asp:LinkButton>                     
                            </div>
                        
                    </div>

<%--                  <div class="control-group">
                       <asp:imagebutton id="imagebutton2" runat="server" height="60px" imageurl="~/img/manual/folderimg.png"
                         width="60px" />
                 
                     
                            <div class="input-append">
                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <asp:linkbutton id="lnkandroidapk" runat="server"
                                    style="font-size:12px;color:blue;font-weight:bold" 
                                       onclick="lnkAndroidApk_Click" >android apk file</asp:linkbutton>                     
                               
                           
                        </div>
                    </div> --%>



                        </div>













                                      


                                        
                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                         
                                    </div>
                                </div>
                                
                                <div class="space20"></div>
                                <!-- END FORM-->
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                           
                        
                            
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
          

            <!-- END PAGE CONTENT-->
         </div>
</asp:Content>

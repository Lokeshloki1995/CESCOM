<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TutorialVideos.aspx.cs"  MasterPageFile="~/DTLMS.Master" Inherits="IIITS.DTLMS.DashboardForm.TutorialVideos" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function openTab(th) {
            window.open(th.name, '_blank');
        }
        </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdfvideopath" runat="server" />
    <style>
fieldset.scheduler-border {
    border: 1px groove #ddd !important;
    padding: 0 1.4em 1.4em 1.4em !important;
    margin: 0 0 1.5em 0 !important;
    -webkit-box-shadow:  0px 0px 0px 0px #000;
            box-shadow:  0px 0px 0px 0px #000;
}

    legend.scheduler-border {
        font-size: 1.2em !important;
        font-weight: bold !important;
        text-align: left !important;
        width:auto;
        padding:0 10px;
        border-bottom:none;
    }
</style>
    <br /><br />
         <div class="container-fluid">
 <fieldset class="scheduler-border">
    <legend class="scheduler-border">Video Tutorials: Android</legend>
  <!-- Trigger the modal with a button -->
  <a href="#"  data-toggle="modal" data-target="#myModal">001-New DTC Commissioning (Offline)</a> 
     

 
 <%-- <asp:LinkButton runat="server" ID="lnkbtnVDO1" text="video_1" OnClick="vdo_click1"></asp:LinkButton>--%>
  <!-- Modal -->
  <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
       
          <h4 class="modal-title"style="color:#fff;font-weight:bold"><center>001-New DTC Commissioning (Offline)</center></h4>
             
        </div>
        <div style="padding: 33px!important;overflow-x: hidden;"class="modal-body">
       
<video width="507" height="281" id="Videoass" controls  src= "https://cescdtlms.com/VideoTutorial/NewDTCComm.mp4">
  
</video>




   
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  </div>


   

 
    
 

       <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
 
</asp:Content>

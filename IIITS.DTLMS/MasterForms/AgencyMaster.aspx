<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="AgencyMaster.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.AgencyMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script src="../Scripts/functions.js" type="text/javascript"></script>
      <script  type="text/javascript">
          function preventMultipleSubmissions() {
  $('#<%=cmdSave.ClientID %>').prop('disabled', true);
}
window.onbeforeunload = preventMultipleSubmissions;
          function ValidateMyForm() {
              if (Page_ClientValidate()) {
                  if (document.getElementById('<%= txtRepairName.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Agency name')
                  document.getElementById('<%= txtRepairName.ClientID %>').focussearch

                  return false
                  }
               if (document.getElementById('<%= cmbDivision.ClientID %>').value   == "-Select-") {
                  alert('Select Division Name')
                  document.getElementById('<%= cmbDivision.ClientID %>').focus()
                  return false
              }

              

              if (document.getElementById('<%= txtRepairPhnNo.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Repairer Phone No')
                  document.getElementById('<%= txtRepairPhnNo.ClientID %>').focus()
                  return false

              }

              if (document.getElementById('<%= txtRepairEmailId.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Repairer EmailId')
                  document.getElementById('<%= txtRepairEmailId.ClientID %>').focus()
                  return false
              }



<%--              if (document.getElementById('<%= txtRepairAddress.ClientID %>').value.trim() == "") {
                  alert('Enter Valid Register Address')
                  document.getElementById('<%= txtRepairAddress.ClientID %>').focus()
                  return false
              }--%>
                  if (document.getElementById('<%= txtRepairAddress.ClientID %>').value.trim() == "") {
                  alert('Please Enter Valid Register Address')
                  document.getElementById('<%= txtRepairAddress.ClientID %>').focus()
                  return false
              }
            
          }
      }

     </script>
<%--    <script language="Javascript" type="text/javascript">    
    
function allowOnlyLetters(e, t)   
{    
   if (window.event)    
   {    
      var charCode = window.event.keyCode;    
   }    
   else if (e)   
   {    
      var charCode = e.which;    
   }    
   else { return true; }    
   if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 160	))    
       return true;    
   else  
   {    
      alert("Please enter only alphabets");    
      return false;    
   }           
}      
</script> --%>
    <script language="javascript" type="text/javascript">
        function allowOnlyLetters(e, t) {
             try {
                 if (window.event) {
                     var charCode = window.event.keyCode;
                 }
                 else if (e) {
                     var charCode = e.which;
                 }
                 else { return true; }
                 if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 32)
                  
                     return true;
                 else
                     //alert("Please enter only alphabets");
                     return false;
             }
             catch (err) {
                 alert(err.Description);
             }
             }
        </script>


    <script language="Javascript" type="text/javascript">


         function onlyAlphabets(e, t) {
             var code = ('charCode' in e) ? e.charCode : e.keyCode;
             if ( // space
               !(code > 31 && code < 47) && // Special characters
               !(code > 64 && code < 91) && // upper alpha (A-Z)
               !(code > 96 && code < 123)) { // lower alpha (a-z)
                 e.preventDefault();
             }
         }
         function onlyAlphabets(e, t) {
             var code = ('charCode' in e) ? e.charCode : e.keyCode;
             if ( // space
               !(code > 31 && code < 47) && // Special characters
               !(code > 64 && code < 91) && // upper alpha (A-Z)
               !(code > 96 && code < 123)) { // lower alpha (a-z)
                 e.preventDefault();
             }
         }


         

    </script> 

    <script language="Javascript" type="text/javascript">
        function OnlyNumberHyphen(e, t) {
            var text = document.getElementById("txtRepairPhnNo").value;
            var regx = /^[6-9]\d{9}$/;
            if (regx.test(text))
                alert("valid");
            else
                alert("invalid");
        }
    </script>
    <script language="Javascript" type="text/javascript">


        function AddressAlphabets(e, t) {
            var text = document.getElementById("txtRepairPhnNo").value;
            var regx = /^[0-9a-zA-Z]+$/;
            if (regx.test(text))
                alert("valid");
            else
                alert("invalid");
        }
            </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span8">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                  Agency Master Details
                   </h3>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="Text1" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>
                               </div>
                           </form>
                       </li>
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->

               </div>
                <div style="float:right;margin-top:20px;margin-right:12px" >
                      <asp:Button ID="cmdClose" runat="server" Text="Agency View" 
                                      OnClientClick="javascript:window.location.href='AgencyMasterView.aspx'; return false;"
                            CssClass="btn btn-primary" /></div>
                         
            </div>
            <!-- END PAGE HEADER-->
             <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i> Agency Master Details</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                   <div class="widget-body">                     

                  
                            <div class="space20"></div>
                  </div>
                                    
                        <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                    <div class="span1"></div>
                                        <div class="span5">
                              

                      
                      <div class="control-group">
                        <label class="control-label" >Agency Name <span class="Mandotary"> *</span></label>
                        <div class="controls">
                            <div class="input-append">
                                <%--<asp:TextBox ID="txtRepairName" runat="server" MaxLength="30" TabIndex="1" style="width:300px" onkeypress="return allowOnlyLetters(event,this);"></asp:TextBox>--%>
                                <asp:TextBox ID="txtRepairName" runat="server"  MaxLength="50"  TabIndex="1"  style="width:250px" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                      <div class="control-group">
                        <label class="control-label">Mobile Number <span class="Mandotary"> *</span></label>
                       
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtRepairPhnNo" runat="server" 
                                        MaxLength="10"  style="width:250px"
                                         onkeypress="javascript:return OnlyNumberHyphen(this,event);" TabIndex="3"></asp:TextBox>
                             <%--                  onkeypress="javascript:return Phonevalidate(this,event);" TabIndex="3"></asp:TextBox>--%>
                            
           
                                                       
                            </div>
                        </div>
                    </div>

<%--                      <div class="control-group">
                        <label class="control-label">Email Id <span class="Mandotary"> *</span></label>
                      
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtRepairEmailId" runat="server" MaxLength="30" 
                                         CausesValidation="True" TabIndex="4" style="width:250px"></asp:TextBox>
                                      </br>
                                      <asp:RegularExpressionValidator runat="server" id="regular" controltovalidate="txtRepairEmailId" 
                             Validationexpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" 
                           errormessage="Please enter a valid email id!!!!" ForeColor="Red" 
                             Display="Dynamic" Font-Size="Small"/>   
                             
                
                            </div>
                        </div>
                    </div>--%>

                                            <div class="control-group">
                                                <label class="control-label">Email Id<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:TextBox ID="txtRepairEmailId"  style="width:250px" runat="server" MaxLength="50" onkeypress="javascript:return validateEmail(txtRepairEmailId);"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>


        </div>
                         
                     <%-- <div class="control-group" >
                        <label class="control-label">Contact Person Name </label>
                        <div class="controls">
                            <div class="input-append">
                                    <asp:TextBox ID="txtContactPerson" runat="server" MaxLength="50" TabIndex="5" 
                                        ></asp:TextBox> 
                        </div>
                        </div>
                    </div>--%>
                    <div class="span5" rowspan="2">
<%--                     <div class="control-group">
                        <label class="control-label">Register Address<span class="Mandotary"> *</span></label>
                       
                        <div class="controls">
                            <div class="input-append">
                               <asp:TextBox ID="txtRepairerId" runat="server" MaxLength="50" Visible="false" Width="20px"></asp:TextBox>    
                                <asp:TextBox ID="txtRepairAddress" runat="server" MaxLength="100" Height="60px" style="width:275px ; resize: none" 
                                    TextMode="MultiLine"  
                                             onkeyup="return ValidateTextlimit(this,100);" TabIndex="12" ></asp:TextBox>
                            </div>
                        </div>
                    </div>  --%>  

                                           <div class="control-group">
                                                <label class="control-label">Register Address<span class="Mandotary"> *</span></label>

                                                <div class="controls">
                                                    <div class="input-append">
                                                         <asp:TextBox ID="txtRepairerId" runat="server" MaxLength="50" Visible="false" Width="20px"></asp:TextBox>  
                                                        <asp:TextBox ID="txtRepairAddress" runat="server" MaxLength="250" 
                                                            Style="width:275px ; resize: none" onkeypress="return AddressAlphabets(event,this);" Height="60px" TextMode="MultiLine" onkeyup="return ValidateTextlimit(this,250);" onpaste="return false"></asp:TextBox>
                                                        
                                                    </div>
                                                </div>
                                            </div>

                                           <div class="control-group">
                                                <label class="control-label">Division<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" runat="server" TabIndex="2" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtOfficeCode" runat="server" MaxLength="50" style="width:250px"></asp:TextBox>
                                                        <asp:Button ID="btnSearch" Text="S" class="btn btn-primary"
                                                            runat="server" OnClick="btnSearch_Click" />
                                                    </div>
                                                </div>
                                            </div>

                        </div>
                                         
                        <%--</div>--%>
                                    <div class="span1"></div>
                                        </div>
                                        <div class="space20"></div>
                                        
                                    <div  class="text-center" align="center">
                                    
                                        
                                        <asp:Button ID="cmdSave" runat="server" Text="Save" onclick="cmdSave_Click" CausesValidation="false"
                                       OnClientClick="javascript:return ValidateMyForm()" CssClass="btn btn-success" />
                                      
                              
                                      <asp:Button ID="cmdReset" runat="server" Text="Reset" CausesValidation="false"
                                            onclick="cmdReset_Click"  CssClass="btn btn-danger" /><br />
                                 
                                                <div class="span7"></div>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            
                                    </div>
                                    </div>
                             <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                </div>
                                
                                <div class="space20"></div>

                                <asp:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnShowPopup" CancelControlID="cmdClose"
            PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />
        <div style="width: 100%; vertical-align: middle" align="center">
                          <div style="width: 100%; vertical-align: middle" align="center">
            <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="550px" Width="500px" visible="false">
                <div class="widget blue">
                    <div class="widget-title">
                        <h4>Select Division Codes And Click On Proceed</h4>
                        <div class="space20"></div>


                        <asp:GridView ID="GrdOffices" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-advance table-hover"
                            runat="server" ShowHeaderWhenEmpty="True"
                            EmptyDataText="No Records Found" ShowFooter="true"
                            PageSize="6" Width="90%"
                            AllowPaging="True" DataKeyNames="DIV_CODE" OnPageIndexChanging="GrdOffices_PageIndexChanging" OnRowCommand="GrdOffices_RowCommand" OnRowDataBound="GrdOffices_RowDataBound">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="DIV_CODE" HeaderText="Division Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffCode" runat="server" Text='<%# Bind("DIV_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtdivCode" runat="server" maxlength="2" placeholder="Enter Division Code" Width="100px"  onkeypress="javascript:return OnlyNumberHyphen(this,event);" ></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="DIV_NAME" HeaderText="Division Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStaDesc" runat="server" Text='<%# Bind("DIV_NAME") %>' Style="word-break: break-all" Width="150px"> </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtdivName" runat="server" maxlength="15" placeholder="Enter Division Name" onkeypress="javascript:return allowOnlyLetters(event,this);" Width="200px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" Visible="true">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="space20"></div>

                        <div class="row-fluid">
                            <div class="span1"></div>
                            <div class="span2">

                                <div class="control-group">
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:Button ID="btnOK" runat="server" CssClass="btn btn-primary" Text="Proceed" OnClick="btnOK_Click1" />

                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="span2">

                                <div class="control-group">

                                    <div class="controls">
                                        <div class="input-append">
                                            <%--<onclick="btnClose_Click"></onclick>--%>
                                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary"  Text="Cancel" />

                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>
          </div>


                                <!-- END FORM-->

                           
                            
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                </div>
            </div>
            <!-- END PAGE CONTENT-->
            
<%--             <div class="row-fluid" >
                 <div class="widget blue" >
                        <div class="widget-title" >
                            <h4><i class="icon-reorder"></i>Existing Agency</h4>
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

 <asp:GridView ID="grdRepairer" AutoGenerateColumns="false" PageSize="30" AllowPaging="true"  
 ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true"
  CssClass="table table-advance table-striped table-bordered"
     

  runat="server" onpageindexchanging="grdAgency_PageIndexChanging" onrowcommand="grdRepairer_RowCommand" 
                                    OnSorting="grdRepairer_Sorting" AllowSorting="true"  onrowdatabound="grdRepairer_RowDataBound" >
                                <HeaderStyle CssClass="both" />
     <Columns>
      <asp:TemplateField AccessibleHeaderText="RA_ID" HeaderText="Id" Visible="false">        
            <ItemTemplate>                                                 
                <asp:Label ID="lblRepairId" runat="server" Text='<%# Bind("RA_ID") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="RA_NAME" HeaderText="Agency Name" Visible="true" SortExpression="TR_NAME" ItemStyle-Width="200">           
            <ItemTemplate>
                <asp:Label ID="lblName" runat="server" Text='<%# Bind("RA_NAME") %>' style="word-break: break-all;" width="250"></asp:Label>
            </ItemTemplate>
             <FooterTemplate>
             <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                    <asp:TextBox ID="txtRepairerName" runat="server" placeholder="Enter Agency Name" Width="100px" ></asp:TextBox>
             </asp:Panel>
             </FooterTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="RA_ADDRESS" HeaderText="Addresss" Visible="true" ItemStyle-Width="300">          
            <ItemTemplate>
                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("RA_ADDRESS") %>' style="word-break: break-all;" width="300"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="RA_PHNO" HeaderText="Phone no" Visible="true" ItemStyle-Width="130">          
            <ItemTemplate>
                <asp:Label ID="lblphone" runat="server" Text='<%# Bind("RA_PHNO") %>' style="word-break: break-all;" width="130"></asp:Label>
            </ItemTemplate>
              <FooterTemplate>
                 <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
             </FooterTemplate>
        </asp:TemplateField>

          <asp:TemplateField AccessibleHeaderText="TS_EMAIL" HeaderText="EmailId"  Visible="true" ItemStyle-Width="300">
            
            <ItemTemplate>
                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("RA_MAIL") %>' style="word-break: break-all;" width="300"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    <%--      <asp:TemplateField AccessibleHeaderText="TS_FAX" HeaderText="Fax No"  Visible="true" ItemStyle-Width="100">
            
            <ItemTemplate>
                <asp:Label ID="lblFax" runat="server" Text='<%# Bind("TR_FAX") %>' style="word-break: break-all;" width="100"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

         <%-- <asp:TemplateField HeaderText="Click to Edit" ItemStyle-Width="100">
            <ItemTemplate>
                <center>
                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="25px" ImageUrl="~/Styles/images/edit64x64.png"
                        Width="25px"   CommandName="upload" />
                </center>
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText ="Status" ItemStyle-Width="100">
             <ItemTemplate>
                 <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("RA_STATUS") %>' ></asp:Label>
                 <centre>
                       <asp:ImageButton Visible="false"  ID="imgDeactive"  runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status" 
                        tooltip="Click to Activate Master Repairer"  width="25px" />     
                     <!-- OnClientClick="return ConfirmStatus('Activate');" -->   
                        <asp:ImageButton Visible="false"  ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif"  CommandName="status" 
                        tooltip="Click to DeActivate MaaSter Repairer" width="25px"  />        
                 </centre>
             </ItemTemplate>

             </asp:TemplateField>

         </Columns>
      <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Font-Size="Medium" Width="15px"/>
      <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" Font-Bold="true" Font-Size="Medium" Width="15px" HorizontalAlign="Center" VerticalAlign="Middle"/>
     </asp:GridView>

                                     </div>
                            </div>

                            </div>
                          </div>
                </div>

             </div>--%>

<%--        </div>--%>
   

</asp:Content>

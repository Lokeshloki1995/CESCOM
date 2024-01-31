<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="TestEx.aspx.cs" Inherits="IIITS.DTLMS.Others.TestEx" %>
<%@ Register Src="~/ReportFilterControl.ascx" TagName="ReportFilterControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <div class="widget-body">
                    <div class="widget-body form">
                    <uc1:ReportFilterControl ID="ucFilterControl" runat="server" />
                    </div>
                    <asp:TextBox ID="txtValue" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

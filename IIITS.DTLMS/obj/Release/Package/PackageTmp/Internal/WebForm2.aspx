<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="IIITS.DTLMS.Internal.WebForm2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="txtFailedDate" runat="server" MaxLength="10" ></asp:TextBox>
                                 <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1" 
                                    TargetControlID="txtFailedDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
    </div>
    </form>
</body>
</html>

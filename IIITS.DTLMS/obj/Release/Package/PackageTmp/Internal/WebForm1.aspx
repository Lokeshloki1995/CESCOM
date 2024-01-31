<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="IIITS.DTLMS.Internal.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        li{
          border:1px solid black;
          padding:20px 20px 20px 20px;
          width:110px;
          background-color:Gray;
          color:White;
          cursor:pointer;
          }
        a{ 
          color:White; font-family:Tahoma; 
        }
    </style>

    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.4.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $("ul.level1 li").hover(
            function () {
                $(this).stop().animate({ opacity: 0.7, width: "170px" }, "slow");
            }, function () {
                $(this).stop().animate({ opacity: 1, width: "110px" }, "slow");
            });
        });
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<form id="form1" runat="server">--%>
        <div id="menu">    
            <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" RenderingMode="List">
                <Items>
                    <asp:MenuItem NavigateUrl="~/Internal/WebForm1.aspx" ImageUrl="~/Styles/LoginPage/1.jpg" Text="Home" Value="Home"  />
                    <asp:MenuItem NavigateUrl="~/Internal/WebForm1.aspx" ImageUrl="~/Styles/LoginPage/2.jpg" Text="About Us" Value="AboutUs" />
                    <asp:MenuItem NavigateUrl="~/Internal/WebForm1.aspx" ImageUrl="~/Styles/LoginPage/3.jpg" Text="Products" Value="Products" />
                    <asp:MenuItem NavigateUrl="~/Internal/WebForm1.aspx" ImageUrl="~/Styles/LoginPage/4.jpg" Text="Contact Us" Value="ContactUs" />
                </Items>
            </asp:Menu>
        </div>

        <ul class="level1">
            <li><a class="level1" href="Default.aspx"><img src="Images/Home.png" alt="" title="" class="icon" />Home</a></li>
            <li><a class="level1" href="About.aspx"><img src="Images/Friends.png" alt="" title="" class="icon" />About Us</a></li>
            <li><a class="level1" href="Products.aspx"><img src="Images/Box.png" alt="" title="" class="icon" />Products</a></li>
            <li><a class="level1" href="Contact.aspx"><img src="Images/Chat.png" alt="" title="" class="icon" />Contact Us</a></li>
        </ul>


    <%--</form>--%>

</asp:Content>



















<%--<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>ASP.NET Menu + jQuery</title>
        <style type="text/css">
            li {
                border:1px solid black;
                padding:20px 20px 20px 20px;
                width:110px;
                background-color:Gray;
                color:White;
                cursor:pointer;
                }
            a { color:White; font-family:Tahoma; 
            }    

        </style>
        <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.4.min.js"></script>
        <script type="text/javascript">
            $(function () {
                $("ul.level1 li").hover(function () { $(this).stop().animate({ opacity: 0.7, width: "170px" }, "slow"); }, function () { $(this).stop().animate({ opacity: 1, width: "110px" }, "slow"); });
            });    </script>

    </head>
    <body>    
        <form id="form2" runat="server">    
            <div id="menu">        
                <asp:Menu ID="Menu2" runat="server" Orientation="Horizontal" RenderingMode="List">                        
                    <Items>                
                        <asp:MenuItem NavigateUrl="~/Default.aspx" ImageUrl="~/Images/Home.png" Text="Home" Value="Home"  />                
                        <asp:MenuItem NavigateUrl="~/About.aspx" ImageUrl="~/Images/Friends.png" Text="About Us" Value="AboutUs" />                
                        <asp:MenuItem NavigateUrl="~/Products.aspx" ImageUrl="~/Images/Box.png" Text="Products" Value="Products" />                
                        <asp:MenuItem NavigateUrl="~/Contact.aspx" ImageUrl="~/Images/Chat.png" Text="Contact Us" Value="ContactUs" />            

                    </Items>        

                </asp:Menu>    

            </div>    

        </form>

    </body>

</html>--%>






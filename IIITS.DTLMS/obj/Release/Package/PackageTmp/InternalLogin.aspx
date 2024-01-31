<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternalLogin.aspx.cs" Inherits="IIITS.DTLMS.InternalLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta content="width=device-width, initial-scale=1.0" name="viewport"/>
<meta http-equiv="Content-type" content="text/html; charset=utf-8">
<meta content="" name="description"/>
<meta content="" name="author"/>
<!-- BEGIN GLOBAL MANDATORY STYLES -->

    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all" rel="stylesheet" type="text/css"/>
<link href="../Styles/LoginPage/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>

<link href="Styles/LoginPage/bootstrap.min.css" rel="stylesheet" type="text/css"/>
<link href="Styles/LoginPage/uniform.default.css" rel="stylesheet" type="text/css"/>
<!-- END GLOBAL MANDATORY STYLES -->
<!-- BEGIN PAGE LEVEL STYLES -->

<link href="Styles/LoginPage/select2.css" rel="stylesheet" type="text/css"/>
<link href="Styles/LoginPage/login-soft.css" rel="stylesheet" type="text/css"/>

<!-- END PAGE LEVEL SCRIPTS -->
<!-- BEGIN THEME STYLES -->
<link href="Styles/LoginPage/components-rounded.css" id="style_components" rel="stylesheet" type="text/css"/>
<link href="Styles/LoginPage/plugins.css" rel="stylesheet" type="text/css"/>
<link href="Styles/LoginPage/layout.css" rel="stylesheet" type="text/css"/>
<link href="Styles/LoginPage/default.css" rel="stylesheet" type="text/css" id="style_color"/>
<%--<link href="Styles/LoginPage/custom.css" rel="stylesheet" type="text/css"/>--%>
<!-- END THEME STYLES -->
<%--<link rel="shortcut icon" href="favicon.ico"/>--%>

<script src="Styles/LoginPage/jquery.min.js" type="text/javascript"></script>
<script src="Styles/LoginPage/jquery-migrate.min.js" type="text/javascript"></script>
<script src="Styles/LoginPage/bootstrap.min.js" type="text/javascript"></script>
<script src="Styles/LoginPage/jquery.blockui.min.js" type="text/javascript"></script>
<script src="Styles/LoginPage/jquery.uniform.min.js" type="text/javascript"></script>
<script src="Styles/LoginPage/jquery.cokie.min.js" type="text/javascript"></script>
<!-- END CORE PLUGINS -->
<!-- BEGIN PAGE LEVEL PLUGINS -->
<script src="Styles/LoginPage/jquery.validate.min.js" type="text/javascript"></script>

<script src="Styles/LoginPage/jquery.backstretch.min.js" type="text/javascript"></script>
<script type="text/javascript" src="Styles/LoginPage/select2.min.js"></script>


<!-- END PAGE LEVEL PLUGINS -->
<!-- BEGIN PAGE LEVEL SCRIPTS -->
<script src="Styles/LoginPage/metronic.js" type="text/javascript"></script>
<script src="Styles/LoginPage/layout.js" type="text/javascript"></script>
<script src="Styles/LoginPage/demo.js" type="text/javascript"></script>
<script src="Styles/LoginPage/login-soft.js" type="text/javascript"></script>


<!-- END PAGE LEVEL SCRIPTS -->
<script>
    jQuery(document).ready(function () {
        Metronic.init(); // init metronic core components
        Layout.init(); // init current layout
        Login.init();
        Demo.init();
        // init background slide images
        $.backstretch([
        "Styles/LoginPage/Int1.jpg",
        "Styles/LoginPage/Int2.jpg",
        "Styles/LoginPage/Int3.jpg",
        "Styles/LoginPage/4.jpg"
        ], {
            fade: 1000,
            duration: 8000
        }
    );
    });
</script>

<script type="text/javascript">

    function ValidateMyForm() {
        if (document.getElementById('<%= txtUsername.ClientID %>').value.trim() == "") {
            alert('Enter Valid User Name')
            document.getElementById('<%= txtUsername.ClientID %>').focus()
            return false
        }
        if (document.getElementById('<%= txtPassword.ClientID %>').value.trim() == "") {
            alert('Enter Valid Password')
            document.getElementById('<%= txtPassword.ClientID %>').focus()
            return false
        }
    }

</script>
</head>
<body class="login">
    <form id="form1" runat="server">
  <div class="logo" style="color:White;font-weight:bold">
  <h1>DTC Life Cycle Management System</h1>
	<%--<a href="index.html">
	<img src="../../assets/admin/layout4/img/logo-big.png" alt=""/>
	</a>--%>
</div>
  <div class="menu-toggler sidebar-toggler">
</div>
<!-- END SIDEBAR TOGGLER BUTTON -->
<!-- BEGIN LOGIN -->
<div class="content">
	<!-- BEGIN LOGIN FORM -->
	<div id="Form2" class="login-form" runat="server">
		<h3 class="form-title" style="font-weight:bold" >Login to your account</h3>
		<div class="alert alert-danger display-hide">
			<button class="close" data-close="alert"></button>
			<span>
			Enter any username and password. </span>
		</div>
		<div class="form-group">
			<!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
			<label class="control-label visible-ie8 visible-ie9">Login Type</label>
			<div class="input-icon">
				<i class="fa fa-user"></i>
		
			      <asp:TextBox ID="txtUsername" class="form-control placeholder-no-fix" placeholder="User Name"  runat="server"></asp:TextBox>
            </div>
		</div>
		<div class="form-group">
			<label class="control-label visible-ie8 visible-ie9">Section Office</label>
			<div class="input-icon">
              
				<i class="fa fa-lock"></i>
				 <asp:TextBox ID="txtPassword" class="form-control placeholder-no-fix" placeholder="Password"  runat="server" TextMode="Password" ></asp:TextBox>
			</div>
		</div>
        
		<div class="form-actions">

		  <asp:Button ID="cmdLogin" runat="server" Text="Login" OnClientClick="javascript:return ValidateMyForm()"
                class="btn blue pull-right " onclick="cmdLogin_Click" />

			

           <div class="forget-password" runat="server" id="dvForgtPwd">		
			<p>
				 <a href="javascript:;" id="forget-password" style="color:White" >
				Forgot your password ? </a>
				
			</p>
		  </div>
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red" ></asp:Label>
		</div>
	</div>
	<!-- END LOGIN FORM -->

    <!-- BEGIN FORGOT PASSWORD FORM -->
	<div class="forget-form" action="index.html" method="post">
		<h3>Forget Password ?</h3>
		<p>
			 Enter your Mobile No below to reset your password.
		</p>
		<div class="form-group">
			<div class="input-icon">
				<i class="fa fa-envelope"></i>
                 <asp:TextBox ID="txtEmail" class="form-control placeholder-no-fix" placeholder="Mobile No"  runat="server"></asp:TextBox>
				
			</div>
             <asp:Label ID="lblFMsg" runat="server" ForeColor="Red" ></asp:Label>
		</div>
		<div class="form-actions">
			<button type="button" id="back-btn" class="btn">
			<i class="m-icon-swapleft"></i> Back </button>
            <asp:Button ID="cmdFSave" runat="server" Text="Submit" 
                class="btn blue pull-right " onclick="cmdFSave_Click" />
          <%--  <i class="m-icon-swapright m-icon-white"></i>--%>
			
		</div>
	</div>
	<!-- END FORGOT PASSWORD FORM -->

	
</div>
<!-- END LOGIN -->
<!-- BEGIN COPYRIGHT -->
<div class="copyright">
	 2016 &copy; Idea Infinity IT Solutions (P) Ltd.
</div>
    </form>
</body>
</html>

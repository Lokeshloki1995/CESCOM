<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IIITS.DTLMS.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <title>CESCOM::Distribution Transformer LifeCycle Management Software</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="Content-type" content="text/html; charset=utf-8" />
    <meta content="" name="description" />
    <meta content="" name="author" />
    <link rel="icon" href="img/CescLogo%20(2).png" />

    <!-- BEGIN GLOBAL MANDATORY STYLES -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" rel="stylesheet" />
    <%--<link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" />--%>
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css" rel="stylesheet" />
    <link href="js/bootsrap1.css" rel="stylesheet" />
    <link href="assets/uniform/css/uniform.default.css" rel="stylesheet" />
    <%--<link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all" rel="stylesheet" type="text/css" />--%>
    <%--<link href="Styles/LoginPage/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />--%>
    <link href="Styles/LoginPage/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="Styles/LoginPage/uniform.default.css" rel="stylesheet" type="text/css" />
    <!-- END GLOBAL MANDATORY STYLES -->

    <!-- BEGIN PAGE LEVEL STYLES -->
    <link href="Styles/LoginPage/select2.css" rel="stylesheet" type="text/css" />
    <link href="Styles/LoginPage/login-soft.css" rel="stylesheet" type="text/css" />
    <!-- END PAGE LEVEL SCRIPTS -->

    <!-- BEGIN THEME STYLES -->
    <link href="Styles/LoginPage/components-rounded.css" id="style_components" rel="stylesheet" type="text/css" />
    <link href="Styles/LoginPage/plugins.css" rel="stylesheet" type="text/css" />
    <link href="Styles/LoginPage/layout.css" rel="stylesheet" type="text/css" />
    <script src="Styles/LoginPage/jquery.min.js" type="text/javascript"></script>
    <script src="Styles/LoginPage/jquery-migrate.min.js" type="text/javascript"></script>
    <script src="Styles/LoginPage/bootstrap.min.js" type="text/javascript"></script>
    <script src="Styles/LoginPage/jquery.blockui.min.js" type="text/javascript"></script>
    <script src="Styles/LoginPage/jquery.uniform.min.js" type="text/javascript"></script>
    <script src="Styles/LoginPage/jquery.cokie.min.js" type="text/javascript"></script>
    <!-- END CORE PLUGINS -->

    <!-- BEGIN PAGE LEVEL PLUGINS -->
    <script type="text/javascript" src="Styles/LoginPage/jquery.validate1.min.js"></script>
    <script src="Styles/LoginPage/jquery.backstretch.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Styles/LoginPage/select2.min.js"></script>
    <!-- END PAGE LEVEL PLUGINS -->

    <!-- BEGIN PAGE LEVEL SCRIPTS -->
    <script src="Styles/LoginPage/demo.js" type="text/javascript"></script>
    <script src="Styles/LoginPage/login-soft.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL SCRIPTS -->
    <style type="text/css">
        i.fa.fa-eye, .fa-eye-slash:before {
            font-size: 16px;
        }

        a.icon-view {
            position: absolute;
            margin-left: -26px;
            z-index: 999 !important;
        }

        .progress-bar {
            float: left;
            height: 100%;
            font-size: 12px;
            line-height: 20px;
            color: #fff;
            text-align: center;
            background-color: #5cb85c;
        }

        .progress {
            height: 7px;
            width: 220px;
            margin-left: -4px !important;
        }

        .form-horizontal .control-group {
            margin-bottom: 0px !important;
        }

        .btn-success {
            background: #449d44;
        }

        .btn-danger {
            background: #d9534f !important;
        }

        .form-horizontal .control-label {
            margin-left: -4px !important;
        }

        ul.list-unstyled {
            list-style: none;
        }

        ul, ol {
            padding: 0;
            margin: 0 0 10px 0px;
        }

        .glyphicon {
            position: absolute;
            top: 8px;
            left: 278px;
            display: inline-block;
            font-family: 'Glyphicons Halflings';
            font-style: normal;
            font-weight: 400;
            line-height: 1;
            -webkit-font-smoothing: antialiased;
        }

        #UserNamePswPag {
            margin-top: 144px !important;
        }

        #PasswordStrength {
            margin-top: 45PX !important;
        }

        .input-icon > .form-control {
            padding-left: 33px;
        }


        .backstretch {
            opacity: .6;
        }

            .backstretch img {
                margin-top: 180.85px !important;
            }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            // On Click SignIn Button Checks For Valid E-mail And All Field Should Be Filled
            $("#login").click(function () {
                var email = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,4}$/i);
                if ($("#loginemail").val() == '' || $("#loginpassword").val() == '') {
                    alert("Please fill all fields!!!!!!");
                } else if (!($("#loginemail").val()).match(email)) {
                    alert("Please enter valid Email!!!!!!");
                } else {
                    alert("You have successfully Logged in!!!!!!");
                    $("form")[0].reset();
                }
            });
            $("#register").click(function () {
                var email = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,4}$/i);
                if ($("#name").val() == '' || $("#registeremail").val() == '' || $("#registerpassword").val() == '' || $("#contact").val() == '') {
                    alert("Please fill all fields!!!!!!");
                } else if (!($("#registeremail").val()).match(email)) {
                    alert("Please enter valid Email!!!!!!");
                } else {
                    alert("You have successfully Sign Up, Now you can login!!!!!!");
                    $("#form")[0].reset();
                    $("#second").slideUp("slow", function () {
                        $("#first").slideDown("slow");
                    });
                }
            });
            // On Click SignUp It Will Hide Login Form and Display Registration Form
            $("#signup").click(function () {
                $("#first").slideUp("slow", function () {
                    $("#second").slideDown("slow");
                });
            });
            // On Click SignIn It Will Hide Registration Form and Display Login Form
            $("#signin").click(function () {
                $("#second").slideUp("slow", function () {
                    $("#first").slideDown("slow");
                });
            });
        });


    </script>
    <script type="text/javascript">
        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };
    </script>

    <!-- END PAGE LEVEL SCRIPTS -->
    <script type="text/javascript">
        jQuery(document).ready(function () {

            const d = new Date();
            let year = d.getFullYear();
            document.getElementById("demo").innerHTML = year;

            Login.init();
            Demo.init();

            $.backstretch([

            "Styles/LoginPage/2.jpg"

            ], {
                fade: 1000,
                duration: 8000
            }
        );
        });

    </script>


    <script type="text/javascript">
        document.onkeydown = function (e) {

            if (event.keyCode == 123) {
                return true;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
                return true;
            }
            if (e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
                return true;
            }
            if (e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
                return true;
            }
        }

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

        function characterAndspecialAndNum(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && (charCode < 48 || charCode > 57) && charCode != 32 && charCode != 46 && charCode != 47 && charCode != 95) {

                return false;
            }
            return true;
        }

        function nopaste() {

            return false;
        }

        function AvoidSpace(event) {
            var k = event ? event.which : window.event.keyCode;
            if (k == 32) return false;
        }

        $(document).ready(function () {
            debugger;
            $('#txtNewpwd').keyup(function () {
                debugger;
                var password = $('#txtNewpwd').val();

                if (checkStrength(password) == false) {
                    $(".progress-bar.progress-bar-success.progress-bar-danger").css("background-color", "transparent")
                    $("#result").hide();
                }
            });

            function checkStrength(password) {
                debugger;
                var strength = 0;
                if (password.match(/([a-zA-Z])/)) {
                    strength += 1;
                }
                //If password contains both lower and uppercase characters, increase strength value.
                if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) {

                    $('.low-upper-case').addClass('text-success');
                    $('.low-upper-case i').removeClass('fa-file-text').addClass('fa-check');
                    $('#popover-password-top').addClass('hide');

                } else {
                    $('.low-upper-case').removeClass('text-success');
                    $('.low-upper-case i').addClass('fa-file-text').removeClass('fa-check');
                    $('#popover-password-top').removeClass('hide');
                }

                //If it has numbers and characters, increase strength value.
                if (password.match(/([0-9])/)) {
                    strength += 1;
                    $('.one-number').addClass('text-success');
                    $('.one-number i').removeClass('fa-file-text').addClass('fa-check');
                    $('#popover-password-top').addClass('hide');

                } else {
                    $('.one-number').removeClass('text-success');
                    $('.one-number i').addClass('fa-file-text').removeClass('fa-check');
                    $('#popover-password-top').removeClass('hide');
                }

                //If it has one special character, increase strength value.
                if (password.match(/([\[,!,%,&,@@,#,-,$,^,*,?,_,~,+,(,),`,{,},\-,',.,",<,>,/,=,:,\;,\,, ,|,\]])/)) {
                    strength += 1;
                    $('.one-special-char').addClass('text-success');
                    $('.one-special-char i').removeClass('fa-file-text').addClass('fa-check');
                    $('#popover-password-top').addClass('hide');
                }
                else {
                    $('.one-special-char').removeClass('text-success');
                    $('.one-special-char i').addClass('fa-file-text').removeClass('fa-check');
                    $('#popover-password-top').removeClass('hide');
                }

                if (password.length > 7) {
                    strength += 1;
                    $('.eight-character').addClass('text-success');
                    $('.eight-character i').removeClass('fa-file-text').addClass('fa-check');
                    $('#popover-password-top').addClass('hide');

                } else {
                    $('.eight-character').removeClass('text-success');
                    $('.eight-character i').addClass('fa-file-text').removeClass('fa-check');
                    $('#popover-password-top').removeClass('hide');
                }
                // If value is less than 2
                if (strength < 1) {
                    $('#result').removeClass()
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-danger').text('');
                    $('#password-strength').css('width', '0%');
                }
                else if (strength < 2) {
                    $('#result').addClass('good');
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-danger').text('Very Weak');
                    $('#password-strength').css('width', '10%');
                }
                else if (strength < 4) {
                    $('#result').addClass('good');
                    $('#password-strength').removeClass('progress-bar-success');
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-warning').text('Weak')
                    $('#password-strength').css('width', '60%');
                    return 'Weak'
                } else if (strength >= 4 && password.match(/([a-z])/) && password.match(/([A-Z])/)) {
                    $('#result').removeClass()
                    $('#result').addClass('strong');
                    $('#password-strength').removeClass('progress-bar-warning');
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-success').text('Strong');
                    $('#password-strength').css('width', '100%');
                    return 'Strong'
                }
                else if (strength >= 4) {
                    $('#result').addClass('good');
                    $('#password-strength').removeClass('progress-bar-danger');
                    $('#password-strength').addClass('progress-bar-success');
                    $('#result').addClass('text-warning').text('Weak')
                    $('#password-strength').css('width', '60%');
                    return 'Weak'
                }
            }
        });

    </script>

</head>
<body class="login" oncontextmenu="return false;">
    <form id="form1" runat="server">
        <div class="logo" style="color: White; font-weight: bold">
        </div>

        <div class="menu-toggler sidebar-toggler">
        </div>

        <!-- END SIDEBAR TOGGLER BUTTON -->
        <!-- BEGIN LOGIN -->
        <%--class="content refre"--%>
        <div class="content refre" id="UserNamePswPag" runat="server">

            <!-- BEGIN LOGIN FORM -->
            <div id="Form2" class="login-form" runat="server" autocomplete="off">
                <h3 class="form-title" style="font-weight: bold; color: #000">
                    <center style="color: #fff!important; font-size: 16px; margin-bottom: 20px;">Distribution Transformer LifeCycle Management Software</center>
                </h3>
                <div class="alert alert-danger display-hide">
                    <button class="close" data-close="alert"></button>
                    <span>Enter any username and password. </span>
                </div>


                <div class="form-group">
                    <!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
                    <label class="control-label visible-ie8 visible-ie9">Login Type</label>
                    <div class="input-icon">
                        <i style="color: #000" class="fa fa-user"></i>

                        <asp:TextBox ID="txtUsername" autocomplete="off" class="form-control placeholder-no-fix" placeholder="User Name" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label visible-ie8 visible-ie9">Section Office</label>
                    <div class="input-icon">

                        <i style="color: #000" class="fa fa-lock"></i>
                        <asp:TextBox ID="txtPassword" class="form-control placeholder-no-fix" placeholder="Password" MaxLength="15" runat="server" TextMode="Password"></asp:TextBox><span class="glyphicon glyphicon-eye-open"></span>

                    </div>
                </div>


                <div class="form-actions">

                    <asp:Button ID="cmdLogin" runat="server" Text="Login" OnClientClick="javascript:return ValidateMyForm()"
                        class="btn blue pull-right " OnClick="cmdLogin_Click" />

                    <div class="Reset-password" runat="server" id="dvResetPwd" style="width: 137px;">
                        <p>
                            <a href="javascript:;"
                                id="Reset-password"
                                style="color: #fff; font-size: 12.5px; z-index: 9999999!important;">Forgot your password ?  
                            </a>
                        </p>
                    </div>
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                    <br />

                    <div style="color: #fff; font-size: 14px" class="copyright">
                        <label id="demo"></label>
                        &copy; Idea Infinity IT Solutions (P) Ltd.
                    </div>
                </div>
            </div>
            <!-- END LOGIN FORM -->
        </div>

        <div class="content refre1" id="ResetPswPag" style="display: none" runat="server">
            <!-- BEGIN FORGOT PASSWORD BY OTP FORM -->
            <div id="ResetPwd" runat="server" class="ResetPwd-form" action="index.html" method="post">

                <h3>Forget Password ?</h3>
                <p>
                    Enter your e-mail ID / Mobile No. below to reset your password.
                </p>
                <div class="form-group">
                    <div class="input-icon">
                        <i class="fa fa-phone"></i>
                        <asp:TextBox ID="txtEmail" class="form-control placeholder-no-fix" MaxLength="30" placeholder="Email/PhoneNumber" onpaste="return nopaste();" autocomplete="off" onkeypress="return AvoidSpace(event);" runat="server"></asp:TextBox>

                    </div>
                    <asp:Label ID="lblFMsg" runat="server" ForeColor="Red"></asp:Label>
                </div>

                <div class="form-actions">
                    <div class="Reset-password" runat="server" id="Div2">
                        <asp:Button ID="cmdFSave" runat="server" Text="Get OTP"
                            class="btn blue pull-right " OnClick="cmdFSave_Click" Style="margin-top: -11px;" OnClientClick="javascript:return ValidateNumber()" />
                    </div>
                </div>

                <p>
                    Enter OTP
                </p>
                <div class="form-group">
                    <div class="inpt-icon">
                        <i class="fa ufa-user"></i>
                        <asp:TextBox ID="txtOTP" class="form-control placeholder-no-fix" onkeypress="return characterAndspecialAndNum(event);" onpaste="return nopaste();" autocomplete="off" placeholder="Enter OTP" MaxLength="9" runat="server" ></asp:TextBox>

                    </div>
                </div>

                <p>
                    Enter New Password
                </p>
                <div class="form-group">
                    <div class="input-icon">
                        <i style="color: #000" class="fa fa-lock"></i>
                        <asp:TextBox ID="txtNewpwd"
                            class="form-control placeholder-no-fix input-text"
                            autocomplete="off"
                            TextMode="Password"
                            placeholder="New Password"
                            onpaste="return nopaste();"
                            MaxLength="12" runat="server"
                            onkeypress="return AvoidSpace(event);">
                        </asp:TextBox>

                        <div class="input-group-append">
                            <span style="float: right; margin-top: -24px; margin-right: 13px;">
                                <a href="#" class="toggle_hide_password">
                                    <i class="fas fa-eye-slash" style="color: #000"></i></a>
                            </span>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="txtNewpwd" ErrorMessage="Enter new password"
                            ValidationGroup="reg" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <p>
                    Enter Confirm New Password
                </p>
                <div class="form-group">
                    <div class="input-icon">
                        <i style="color: #000" class="fa fa-lock"></i>
                        <asp:TextBox ID="txtCnfrmPwd"
                            class="form-control placeholder-no-fix input-text"
                            autocomplete="off"
                            TextMode="Password"
                            MaxLength="15"
                            placeholder="Confirm New Password"
                            onpaste="return nopaste();"
                            runat="server"
                            onkeypress="return AvoidSpace(event);">
                        </asp:TextBox>
                        <div class="input-group-append">
                            <span style="float: right; margin-top: -24px; margin-right: 13px;">
                                <a href="#" class="toggle_hide_password">
                                    <i class="fas fa-eye-slash" style="color: #000"></i></a>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-actions">
                    <button type="button" id="back-btn" class="btn" style="background-color: #fffafa">
                        <i class="m-icon-swapleft"></i>Back
                    </button>
                    <div class="Reset-password" runat="server" id="Div1">
                        <asp:Button ID="btnResetPwd" runat="server" Text="Reset Password" OnClientClick="javascript:return ValidatePassword()"
                            class="btn blue pull-right " OnClick="btnResetPwd_Click" />
                    </div>
                    <%--  <i class="m-icon-swapright m-icon-white"></i>--%>
                </div>

                <%--start password strength--%>
                <div class="control-group" id="PasswordStrength">

                    <div class="controls">
                        <p>New Password Strength: <span id="result"></span></p>
                        <div class="progress">
                            <div id="password-strength" class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                            </div>
                        </div>
                        <ul class="list-unstyled" style="color: white">
                            <li class=""><span class="low-upper-case"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp;1 Lower Case &amp; 1 Upper Case</li>
                            <li class=""><span class="one-number"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp;1 Number (0-9)</li>
                            <li class=""><span class="one-special-char"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp;1 Special Character</li>
                            <li class=""><span class="eight-character"><i class="fa fa-file-text" aria-hidden="true"></i></span>&nbsp; Atleast 8 Character</li>
                        </ul>
                    </div>
                </div>
                <%--end--%>
            </div>
            <!-- END FORGOT PASSWORD BY OTP FORM -->
        </div>

        <!-- END LOGIN -->
        <!-- BEGIN COPYRIGHT -->
    </form>

    <script type="text/javascript">
        //Created by santhosh on 13-06-2023
        function ValidateForgotYourPassword() {
            debugger;
            var ValidateResult = true;
            if (document.getElementById('<%= txtUsername.ClientID %>').value.trim() == "") {
                debugger;
                alert('Please Enter Valid User Name');
                document.getElementById('<%= txtUsername.ClientID %>').focus();
                ValidateResult = false;
            }
            return ValidateResult;
        }


        $("#dvResetPwd").click(function (e) {
            debugger;
            var Result = false;
            Result = ValidateForgotYourPassword(); // Calls for the ValidateForgotYourPassword() funtion.
            if (Result != true) {
                $("#UserNamePswPag").show();
                $('.login-form').show();
                $("#ResetPswPag").hide();
            } else {
                $("#UserNamePswPag").hide();
                $("#ResetPswPag").show();
            }
        });
        //$("#dvResetPwd").click(function (e) {
        //    debugger
        //    $(".refre").hide();
        //    $(".content.refre1").show();
        //});
        $("#back-btn").click(function (e) {
            debugger
            //$(".content.refre1").hide();
            //$(".content.refre").show();

            $("#UserNamePswPag").show();
            $("#ResetPswPag").hide();
        });

        $(document).ready(function (e) {
            // target the link
            $(".toggle_hide_password").on('click', function (e) {
                debugger

                // get input group of clicked link
                var input_group = $(this).closest('.input-icon')

                // find the input, within the input group
                var input = input_group.find('.input-text')

                // find the icon, within the input group
                var icon = input_group.find('i')

                // toggle field type
                input.attr('type', input.attr("type") === "text" ? 'password' : 'text')

                // toggle icon class
                icon.toggleClass('fa-eye-slash fa-eye')
            });
        });

        $(".glyphicon-eye-open").on("click", function () {
            $(this).toggleClass("glyphicon-eye-close");
            var type = $("#txtPassword").attr("type");
            if (type == "text")
            { $("#txtPassword").prop('type', 'password'); }
            else
            { $("#txtPassword").prop('type', 'text'); }
        });

        $("#toggleNewPassword").on("click", function () {
            debugger;
            $(this).toggleClass("#toggleNewPassword");
            var type = $("#txtNewpwd").attr("type");
            if (type == "text")
            { $("#txtNewpwd").prop('type', 'password'); }
            else
            { $("#txtNewpwd").prop('type', 'text'); }
        });

        $("#togglePassword").on("click", function () {
            debugger;
            $(this).toggleClass("#togglePassword");
            var type = $("#txtCnfrmPwd").attr("type");
            if (type == "text")
            { $("#txtCnfrmPwd").prop('type', 'password'); }
            else
            { $("#txtCnfrmPwd").prop('type', 'text'); }
        });

        function ValidatePassword() {
            debugger;
             if (document.getElementById('<%= txtOTP.ClientID %>').value == "") {
                 alert('Please Enter OTP')
                document.getElementById('<%= txtNewpwd.ClientID %>').focus()
                document.getElementById('<%= txtCnfrmPwd.ClientID %>').value = "";
                return false
            }
            if (document.getElementById('<%= txtNewpwd.ClientID %>').value == "") {
                alert('Please Enter New Password')
                document.getElementById('<%= txtNewpwd.ClientID %>').focus()
                document.getElementById('<%= txtCnfrmPwd.ClientID %>').value = "";
                return false
            }
            if (document.getElementById('<%= txtNewpwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewpwd.ClientID %>').value

                if (!pass.match(/^(?=.*?[a-z])/)) {
                    alert("Password should contain At least one lower case letter")
                    document.getElementById('<%=txtNewpwd.ClientID %>').focus()
                    document.getElementById('<%= txtCnfrmPwd.ClientID %>').value = "";
                    return false;
                }
            }
            if (document.getElementById('<%= txtNewpwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewpwd.ClientID %>').value

                   if (!pass.match(/^(?=.*?[A-Z])/)) {
                       alert("Password should contain At least one Upper case letter")
                       document.getElementById('<%=txtNewpwd.ClientID %>').focus()
                    document.getElementById('<%= txtCnfrmPwd.ClientID %>').value = "";
                    return false;
                }
            }
            if (document.getElementById('<%= txtNewpwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewpwd.ClientID %>').value
                if (!pass.match(/^(?=.*?[0-9])/)) {
                    alert("Password should contain At least one number")
                    document.getElementById('<%=txtNewpwd.ClientID %>').focus()
                    document.getElementById('<%= txtCnfrmPwd.ClientID %>').value = "";
                    return false;
                }
            }
            if (document.getElementById('<%= txtNewpwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewpwd.ClientID %>').value
                if (!pass.match(/^(?=.*?[!@#~`><;:'",. {}$%^?/=+&*()_-])/)) {
                    alert("Password should contain At least one Special Character")
                    document.getElementById('<%=txtNewpwd.ClientID %>').focus()
                       document.getElementById('<%= txtCnfrmPwd.ClientID %>').value = "";
                       return false;
                   }
               }
               if (document.getElementById('<%= txtNewpwd.ClientID %>').value != "") {
                var pass = document.getElementById('<%= txtNewpwd.ClientID %>').value
                if (!pass.match(/^(?=.*?).{8,12}$/)) {
                    alert("Passwords Length Should be greater than 7 characters")
                    document.getElementById('<%=txtNewpwd.ClientID %>').focus()
                                   document.getElementById('<%= txtCnfrmPwd.ClientID %>').value = "";
                                   return false;
                               }
                           }
                           if (document.getElementById('<%= txtCnfrmPwd.ClientID %>').value == "") {
                alert('Please Enter Confirm Password')
                document.getElementById('<%= txtCnfrmPwd.ClientID %>').focus()
                return false
            }
            if (document.getElementById('<%= txtCnfrmPwd.ClientID %>').value != document.getElementById('<%= txtNewpwd.ClientID %>').value) {
                alert('New Password & Confirm Password do not match')
                document.getElementById('<%= txtCnfrmPwd.ClientID %>').focus()
                document.getElementById('<%= txtCnfrmPwd.ClientID %>').value = "";
                return false
            }
        }
    </script>



</body>
</html>

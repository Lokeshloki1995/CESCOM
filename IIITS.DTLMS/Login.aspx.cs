using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;
using System.IO;

namespace IIITS.DTLMS
{
    public partial class Login : System.Web.UI.Page
    {
        string url = ConfigurationManager.AppSettings["CommonLoginPath"].ToString();
        string ErrorURL = ConfigurationManager.AppSettings["CommonErrorPath"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
            string sPath = sFolderPath + "//" + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            try
            {
                lblMsg.Text = string.Empty;
                Form.DefaultButton = cmdLogin.UniqueID;
                if (Session["ProjectType"] != null)
                {
                    if (Session["ProjectType"].ToString() == "MMS")
                    {
                        Session.Abandon();
                        Session.Clear();
                        Response.Redirect("~/Login.aspx", false);
                    }
                }
                if ((Session["clsSession"] != null))
                {
                    Response.Redirect("Dashboard.aspx", false);
                }
                if (Convert.ToString(ConfigurationManager.AppSettings["CommonLogin"]).ToUpper().Equals("ON"))
                {
                    if (Request.QueryString["Userid"] != null && Request.QueryString["LocationCode"].ToString() != "" && Request.QueryString["RoleId"].ToString() != "")
                    {
                        string UserId = Genaral.ERPUrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Userid"]));
                        string Officecode = Genaral.ERPUrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["LocationCode"]));
                        string Roleid = Genaral.ERPUrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RoleId"]));
                        string ProjectList = Genaral.ERPUrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ModuleName"]));
                        string ChangePasswordFlag = Genaral.ERPUrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ChangePasswordFlag"]));
                        File.AppendAllText(sPath, " DTLMS " + Environment.NewLine + "  DateTime    : " + System.DateTime.Now + Environment.NewLine + " Inputs : " + Environment.NewLine + " UserID = " + UserId + Environment.NewLine + " Officecode = " + Officecode + Environment.NewLine + "Roleid = " + Roleid + Environment.NewLine + " ProjectList = " + ProjectList + Environment.NewLine + " ChangePasswordFlag = " + ChangePasswordFlag + Environment.NewLine + " ##############################################################" + Environment.NewLine);
                        Session["ChangePasswordFlag"] = ChangePasswordFlag;
                        if (Officecode == "999")
                        {
                            Officecode = "";
                        }
                        CommonLogin(UserId, Roleid, Officecode, ProjectList, ChangePasswordFlag);
                    }
                    else
                    {
                        Response.Redirect(url, false);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                if (Convert.ToString(ConfigurationManager.AppSettings["CommonLogin"]).ToUpper().Equals("ON"))
                {
                    Response.Redirect(ErrorURL, false);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string clientIp = string.Empty;
                clsLogin objLogin = new clsLogin();
                clsSession objSession = new clsSession();
                objLogin.sLoginName = txtUsername.Text.Trim().ToUpper();
                objLogin.sPassword = txtPassword.Text.Trim();
                objLogin.sUnencryptedPwd = txtPassword.Text.Trim();
                objLogin.UserLogin(objLogin);
                objLogin.Updatelastlogin(objLogin);
                if (objLogin.sMessage == null && (objLogin.sUserId ?? "").Length > 0)
                {
                    Session["FullName"] = objLogin.sFullName;
                    Session["ChangPwd"] = objLogin.sChangePwd;

                    if (objLogin.sOfficeCode == "0")
                    {
                        objLogin.sOfficeCode = "";
                    }
                    objSession.UserId = objLogin.sUserId;
                    objSession.FullName = objLogin.sFullName;
                    objSession.RoleId = objLogin.sRoleId;
                    objSession.OfficeCode = objLogin.sOfficeCode;
                    objSession.OfficeName = objLogin.sOfficeName;
                    objSession.Designation = objLogin.sDesignation;
                    objSession.OfficeNameWithType = objLogin.sOfficeNamewithType;
                    //get the configuration details 
                    DataTable configurationDetails = Genaral.GetConfiguration();
                    if (configurationDetails.Rows.Count > 0)
                    {
                        objSession.sGeneralLog = Convert.ToString(configurationDetails.Rows[0]["CG_GEN_LOG"]);
                        objSession.sTransactionLog = Convert.ToString(configurationDetails.Rows[0]["CG_TRANS_LOG"]);
                        objSession.sPasswordChangeRequest = Convert.ToString(configurationDetails.Rows[0]["CG_PASS_CHANGE_REQ"]);
                        objSession.sPasswordChangeInDays = Convert.ToString(configurationDetails.Rows[0]["CG_PASS_CHANGE_DAYS"]);
                        objSession.sPassordAcceptance = Convert.ToString(configurationDetails.Rows[0]["CG_PRE_PASS_ACCEPTANCE"]);
                        if (objSession.sPasswordChangeRequest == "1") // check password by days
                        {
                            string numberOfDays = objLogin.GetPasswordDetails(objSession.UserId);
                            if (numberOfDays != null && numberOfDays != "")
                            {
                                if (Convert.ToInt32(numberOfDays) > Convert.ToInt32(objSession.sPasswordChangeInDays))
                                {
                                    Session["ChangPwd"] = "";
                                    Response.Redirect("~/MasterForms/ChangePassword.aspx", false);
                                }
                            }
                        }
                        if (objSession.sGeneralLog == "1")
                        {
                            clientIp = Genaral.GetClientIp();
                            Genaral.GeneralLog(clientIp, objSession.UserId, "LOGIN");
                        }
                        string guid = Guid.NewGuid().ToString();
                        Session["AuthToken"] = guid;
                        // now create a new cookie with this guid value
                        var cookie = new HttpCookie("AuthToken");
                        Response.Cookies["AuthToken"].Value = guid;
                        Session["clsSession"] = objSession;
                        if (Session["ChangPwd"] == null || Session["ChangPwd"].ToString() == "")
                        {
                            Response.Redirect("~/MasterForms/ChangePassword.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("Dashboard.aspx", false);
                        }
                    }
                }
                else
                {
                    lblMsg.Text = objLogin.sMessage;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                clsException.LogError(
                  ex.ToString(),
                  ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name
                  );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sMsg"></param>
        private void ShowMsgBox(string sMsg)
        {
            string sShowMsg = string.Empty;
            try
            {
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(
                  ex.ToString(),
                  ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name
                  );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ValidateForm()
        {
            bool bValidate = true;
            try
            {
                if (txtUsername.Text == "" || txtUsername.Text == null)
                {
                    if (!txtEmail.Text.Contains("@"))
                    {
                        if (txtEmail.Text.Length == 10)
                        {
                            int Mob_First_Digit = Convert.ToInt32(txtEmail.Text.Substring(0, 1));
                            if (Mob_First_Digit <= 6)
                            {
                                txtEmail.Focus();
                                ShowMsgBox("Please Enter Valid Mobile Number");
                                return bValidate;
                            }
                        }
                        else
                        {
                            txtEmail.Focus();
                            ShowMsgBox("Please Enter Valid 10 Digit Mobile Number");
                            return bValidate;
                        }
                    }
                    else
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                        {
                            txtEmail.Focus();
                            ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                            return bValidate;
                        }
                    }
                    if (txtEmail.Text == "")
                    {
                        ShowMsgBox("Please Enter Register Mail Id / Mobile number to get OTP");
                        txtEmail.Focus();
                    }
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(
                  ex.ToString(),
                  ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name
                  );
                return bValidate;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdFSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsLogin objLogin = new clsLogin();
                objLogin.sEmail = txtEmail.Text;

                if (ValidateForm() == true)
                {
                    objLogin.sLoginName = txtUsername.Text;
                    objLogin = objLogin.CheckEmailOrMobialNoDetails(objLogin); //new implementation
                    if (objLogin.VLDEmail_Or_Mob == false)
                    {
                        if (objLogin.sMessage == "Please enter valid user name.")
                        {
                            ResetPwd.Style.Add("display", "none");
                            Form2.Style.Add("display", "block");
                        }
                        else
                        {
                            UserNamePswPag.Style.Add("display", "none");
                            Form2.Style.Add("display", "none");
                            ResetPswPag.Style.Add("display", "block");
                            ResetPwd.Style.Add("display", "block");
                            Form2.Visible = true;
                            ResetPwd.Visible = true;
                        }
                        ShowMsgBox(objLogin.sMessage);
                        return;
                    }
                    objLogin.ForgtPassword(objLogin);

                    if (objLogin.sResult == "-1")
                    {
                        ShowMsgBox(objLogin.sMessage);
                    }
                    else
                    {
                        if (objLogin.sMessage != null)
                        {
                            string sPattern = @"^\d{10}$";
                            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(sPattern);
                            if (r.IsMatch(objLogin.sEmail))
                            {
                                ShowMsgBox("OTP has been sent to your Mobile Number");
                            }
                            else
                            {
                                ShowMsgBox("OTP has been sent to your Registered Email ID");
                            }
                            cmdFSave.Enabled = true;
                        }
                        else
                        {
                            //ShowMsgBox(objLogin.sMessage);
                            //Form2.Visible = true;
                            //ResetPwd.Visible = true;
                            ShowMsgBox("Please get OTP through Registered Mobile No.");
                        }
                        UserNamePswPag.Style.Add("display", "none");
                        Form2.Style.Add("display", "none");
                        ResetPswPag.Style.Add("display", "block");
                        ResetPwd.Style.Add("display", "block");
                        Form2.Visible = true;
                        ResetPwd.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(
                  ex.ToString(),
                  ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name
                  );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResetPwd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtOtpDetails = new DataTable();
                DataTable dtConfiguration = new DataTable();
                string[] Arr = new string[2];
                if (ValidateForgotPassword())
                {
                    if (txtNewpwd.Text == txtCnfrmPwd.Text)
                    {
                        clsLogin objOTPDetails = new clsLogin();
                        dtOtpDetails = objOTPDetails.GetOTPDetails(txtOTP.Text, txtUsername.Text);
                        dtConfiguration = objOTPDetails.GetConfiguration();
                        if (dtOtpDetails.Rows.Count > 0)
                        {
                            if (Convert.ToString(dtConfiguration.Rows[0]["CG_PRE_PASS_ACCEPTANCE"]) == "1")
                            {
                                bool res = objOTPDetails.GetStatus(Genaral.EncryptPassword(txtCnfrmPwd.Text), Convert.ToString(dtOtpDetails.Rows[0]["otp_us_id"]));
                                if (res == false)
                                {
                                    ShowMsgBox("Password Should not be same as old Password");
                                    UserNamePswPag.Style.Add("display", "none");
                                    Form2.Style.Add("display", "none");
                                    ResetPswPag.Style.Add("display", "block");
                                    ResetPwd.Style.Add("display", "block");
                                    Form2.Visible = true;
                                    ResetPwd.Visible = true;
                                    return;
                                }

                            }
                            if (dtOtpDetails.Rows[0]["otp_no"].ToString() == txtOTP.Text)
                            {
                                clsUser objUser = new clsUser();
                                objUser.sPassword = Genaral.EncryptPassword(txtCnfrmPwd.Text);
                                objUser.lSlNo = Convert.ToString(dtOtpDetails.Rows[0]["otp_us_id"]);
                                objUser.sOTP = Convert.ToString(dtOtpDetails.Rows[0]["otp_no"]);
                                string sClientIP = Genaral.GetClientIp();
                                Genaral.PasswordChangeLog(sClientIP, Convert.ToString(dtOtpDetails.Rows[0]["otp_us_id"]));
                                Arr = objUser.UpdatePwd(objUser);
                                
                                if (Arr[0] == "1") //modified on 14-06-2023. // positive Case for Success
                                {
                                    ShowMsgBox(Arr[1]);

                                    txtOTP.Text = string.Empty;
                                    txtNewpwd.Text = string.Empty;
                                    txtCnfrmPwd.Text = string.Empty;

                                    UserNamePswPag.Style.Add("display", "block");
                                    Form2.Style.Add("display", "block");
                                    ResetPswPag.Style.Add("display", "none");
                                    ResetPwd.Style.Add("display", "none");
                                    Form2.Visible = true;
                                    ResetPwd.Visible = true;
                                }else
                                {
                                    ShowMsgBox(Arr[1]);

                                    UserNamePswPag.Style.Add("display", "none");
                                    Form2.Style.Add("display", "none");
                                    ResetPswPag.Style.Add("display", "block");
                                    ResetPwd.Style.Add("display", "block");
                                    Form2.Visible = true;
                                    ResetPwd.Visible = true;
                                }

                            }
                            else
                            {
                                ShowMsgBox("Your OTP Expired Please Generate New OTP");
                                txtOTP.Text = string.Empty;
                                txtNewpwd.Text = string.Empty;
                                txtCnfrmPwd.Text = string.Empty;

                                UserNamePswPag.Style.Add("display", "none");
                                Form2.Style.Add("display", "none");
                                ResetPswPag.Style.Add("display", "block");
                                ResetPwd.Style.Add("display", "block");
                                Form2.Visible = true;
                                ResetPwd.Visible = true;
                            }
                        }
                        else
                        {
                            ShowMsgBox("Please enter correct OTP");
                            txtOTP.Text = string.Empty;
                            txtNewpwd.Text = string.Empty;
                            txtCnfrmPwd.Text = string.Empty;

                            UserNamePswPag.Style.Add("display", "none");
                            Form2.Style.Add("display", "none");
                            ResetPswPag.Style.Add("display", "block");
                            ResetPwd.Style.Add("display", "block");
                            Form2.Visible = true;
                            ResetPwd.Visible = true;
                        }
                    }
                    else
                    {
                        ShowMsgBox("New Password and confirm New Password has to be Same ");

                        UserNamePswPag.Style.Add("display", "none");
                        Form2.Style.Add("display", "none");
                        ResetPswPag.Style.Add("display", "block");
                        ResetPwd.Style.Add("display", "block");
                        Form2.Visible = true;
                        ResetPwd.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = clsException.ErrorMsg();
                clsException.LogError(
                  ex.ToString(),
                  ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name
                  );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateForgotPassword()
        {
            bool bValidate = true;
            try
            {
                if ((txtOTP.Text ?? "").Length == 0)
                {
                    txtOTP.Focus();
                    ShowMsgBox("Please Enter OTP");
                    bValidate = false;
                }
                else if ((txtNewpwd.Text ?? "").Length == 0)
                {
                    txtNewpwd.Focus();
                    ShowMsgBox("Please Enter New Password");
                    bValidate = false;
                }
                else if (txtCnfrmPwd.Text.Length == 0 && txtNewpwd.Text.Length > 0)
                {
                    txtCnfrmPwd.Focus();
                    ShowMsgBox("Please Confirm Password");
                    bValidate = false;
                }
                else if (txtCnfrmPwd.Text.Length == 0 && txtNewpwd.Text.Length == 0)
                {
                    txtCnfrmPwd.Focus();
                    ShowMsgBox("Please Enter New Password and Confirm Password");
                    bValidate = false;
                }
                return bValidate;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                  ex.ToString(),
                  ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name
                  );
                return bValidate;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Roleid"></param>
        /// <param name="Officecode"></param>
        /// <param name="ProjectList"></param>
        /// <param name="ChangePasswordFlag"></param>
        public void CommonLogin(string UserId, string Roleid, string Officecode, string ProjectList, string ChangePasswordFlag)
        {
            try
            {
                string clientIp = string.Empty;
                clsLogin objLogin = new clsLogin();
                clsSession objSession = new clsSession();
                objLogin.sUserId = UserId;
                objLogin.sRoleId = Roleid;
                objLogin.sOfficeCode = Officecode;
                objLogin.sUnencryptedPwd = txtPassword.Text.Trim();
                objLogin.CommonUserLogin(objLogin);
                objLogin.Updatelastlogin(objLogin);
                if (objLogin.sMessage == null)
                {
                    Session["FullName"] = objLogin.sFullName;
                    Session["ChangPwd"] = objLogin.sChangePwd;
                    Session["ChangePasswordFlag"] = ChangePasswordFlag;
                    if (objLogin.sOfficeCode == "0")
                    {
                        objLogin.sOfficeCode = "";
                    }
                    objSession.UserId = objLogin.sUserId;
                    objSession.FullName = objLogin.sFullName;
                    objSession.RoleId = objLogin.sRoleId;
                    objSession.OfficeCode = objLogin.sOfficeCode;
                    objSession.OfficeName = objLogin.sOfficeName;
                    objSession.Designation = objLogin.sDesignation;
                    objSession.OfficeNameWithType = objLogin.sOfficeNamewithType;
                    //get the configuration details 
                    DataTable configurationDetails = Genaral.GetConfiguration();
                    if (configurationDetails.Rows.Count > 0)
                    {
                        objSession.sGeneralLog = Convert.ToString(configurationDetails.Rows[0]["CG_GEN_LOG"]);
                        objSession.sTransactionLog = Convert.ToString(configurationDetails.Rows[0]["CG_TRANS_LOG"]);
                        objSession.sPasswordChangeRequest = Convert.ToString(configurationDetails.Rows[0]["CG_PASS_CHANGE_REQ"]);
                        objSession.sPasswordChangeInDays = Convert.ToString(configurationDetails.Rows[0]["CG_PASS_CHANGE_DAYS"]);
                        objSession.sPassordAcceptance = Convert.ToString(configurationDetails.Rows[0]["CG_PRE_PASS_ACCEPTANCE"]);
                    }
                    objSession.sProjectList = ProjectList;
                    if (objSession.sPasswordChangeRequest == "1") // check password by days
                    {
                        string numberOfDays = objLogin.GetPasswordDetails(objSession.UserId);
                        if (numberOfDays != null && numberOfDays != "")
                        {
                            if (Convert.ToInt32(numberOfDays) > Convert.ToInt32(objSession.sPasswordChangeInDays))
                            {
                                Session["ChangPwd"] = "";
                                Response.Redirect("~/MasterForms/ChangePassword.aspx", false);
                            }
                        }
                    }
                    if (objSession.sGeneralLog == "1")
                    {
                        clientIp = Genaral.GetClientIp();
                        Genaral.GeneralLog(clientIp, objSession.UserId, "LOGIN");
                    }
                    string guid = Guid.NewGuid().ToString();
                    Session["AuthToken"] = guid;
                    // now create a new cookie with this guid value
                    var cookie = new HttpCookie("AuthToken");
                    Response.Cookies["AuthToken"].Value = guid;
                    Session["clsSession"] = objSession;

                    if (ChangePasswordFlag == "0" && Session["ChangPwd"].ToString() == "")
                    {
                        Response.Redirect("~/MasterForms/ChangePassword.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("Dashboard.aspx", false);
                    }
                }
                else
                {
                    lblMsg.Text = objLogin.sMessage;
                    Response.Redirect(ErrorURL, false);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                clsException.LogError(
                  ex.ToString(),
                  ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                  System.Reflection.MethodBase.GetCurrentMethod().Name
                  );
                Response.Redirect(url, false);
            }
        }
    }
}
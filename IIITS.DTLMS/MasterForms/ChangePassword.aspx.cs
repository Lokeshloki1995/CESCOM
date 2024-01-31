using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Globalization;
using System.Data.OleDb;
using System.Configuration;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

namespace IIITS.DTLMS
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string strFormCode = "ChangePassword";
        clsSession objSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = btnsubmit.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }

        }

        public static class ValidatePwd
        {
            public static bool IsValidPwd(string pwd)
            {
                var r = new System.Text.RegularExpressions.Regex(@"^(?=.{8,})(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[#!*()><;:',/.~`,$%^&+-={}@@]).*$"); 

                return !string.IsNullOrEmpty(pwd) && r.IsMatch(pwd);
            }
        }
        //Change password function start
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            //string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LogServiceRole"]) + DateTime.Now.ToString("yyyyMM");
            //if (!Directory.Exists(sFolderPath))
            //{
            //    Directory.CreateDirectory(sFolderPath);

            //}
            //string sPath = sFolderPath + "//" + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            string Logouturl = ConfigurationSettings.AppSettings["CommonLogoutPath"].ToString();
            try
            {
                string[] Arr = new string[3];
                string clientIp = string.Empty;
                clsChangePwd objChangepwd = new clsChangePwd();

                objChangepwd.strOldPwd = txtOldPwd.Text.Replace("'", "");
                objChangepwd.strNewPwd = txtNewPwd.Text.Replace("'", "").Trim();
                objChangepwd.strConfirmPwd = txtConfirmPwd.Text.Replace("'", "").Trim();
                objChangepwd.struserId = objSession.UserId;
                string NewPwd = objChangepwd.strNewPwd;
                //bool PwdValidate = ChangePassword.ValidatePwd.IsValidPwd(NewPwd);
                //if (PwdValidate == true)
                //{
                if ((Session["clsSession"] != null))
                {
                    if (ValidateForm() == true)
                    {
                        if (txtNewPwd.Text == txtConfirmPwd.Text)
                        {

                            if (Convert.ToString(ConfigurationSettings.AppSettings["CommonLogin"]).ToUpper().Equals("ON"))
                            {
                                objChangepwd.usId = Session["HRMSUsID"].ToString();
                                objChangepwd.OldPassword = txtOldPwd.Text.Replace("'", "");
                                objChangepwd.NewPassword = txtNewPwd.Text.Replace("'", "").Trim();
                                WebClient objclien = new WebClient();
                                objclien.Headers.Add("Authorization", "HRMS14062019");
                                DataContractJsonSerializer objS = new DataContractJsonSerializer(typeof(string));
                                objclien.Headers["Content-type"] = "application/json";
                                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                DataContractJsonSerializer serializerToUplaod = new DataContractJsonSerializer(typeof(clsChangePwd));
                                serializerToUplaod.WriteObject(ms, objChangepwd);
                                string hrmsip = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SingleSignInchangepassword"]);
                                byte[] data = objclien.UploadData(hrmsip, "POST", ms.ToArray());
                                Stream stream = new System.IO.MemoryStream(data);
                                objS = new DataContractJsonSerializer(typeof(clsChangePwd));
                                var Result = objS.ReadObject(stream) as clsChangePwd;
                                var Status = Result.iStatus;

                               // File.AppendAllText(sPath, " DTLMS Change Password " + Environment.NewLine + "  DateTime    : " + System.DateTime.Now + Environment.NewLine + " HRMS USER ID : " + objChangepwd.usId + "Old Password=" + objChangepwd.OldPassword + "&New Password=" + objChangepwd.NewPassword + Environment.NewLine + "Result:" + Environment.NewLine + "Status= " + Status + Environment.NewLine + "Message= " + Result.iMessage + Environment.NewLine + " ##############################################################" + Environment.NewLine);

                                //HRMS Method return 1 in case of success 
                                if (Status == "1")
                                {
                                    //ShowMsgBox(Convert.ToString(Result.iMessage));
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Convert.ToString(Result.iMessage) + "');window.location ='" + Logouturl + "';", true);
                                    return;
                                }
                                else
                                {
                                    ShowMsgBox(Convert.ToString(Result.iMessage));
                                    return;
                                }
                            }
                            else
                            {
                                if (objSession.sPassordAcceptance == "1")
                                {
                                    bool res = objChangepwd.GetStatus(Genaral.Encrypt(txtConfirmPwd.Text), objSession.UserId);
                                    if (res == false)
                                    {
                                        ShowMsgBox("you had Already used This Password, Please Use Another Password");
                                        return;
                                    }
                                }
                                Arr = objChangepwd.ChangePwd(objChangepwd);
                                if (Arr[1].ToString() == "0")
                                {
                                    if (objSession.sPasswordChangeRequest == "1")
                                    {
                                        clientIp = Genaral.GetClientIp();
                                        Genaral.PasswordChangeLog(clientIp, objSession.UserId);
                                    }
                                   // ShowMsgBox("Password Changed Successfully");
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password changed sucessfully');window.location ='..Login.aspx';", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Password changed sucessfully'); window.location='" + Request.ApplicationPath + "Login.aspx';", true);
                                    Session.Abandon();
                                    Session.Clear();
                                    Session.RemoveAll();
                                    //string Logouturl = ConfigurationSettings.AppSettings["CommonLogoutPath"].ToString();
                                    //Response.Redirect(Logouturl, false);
                                    return;
                                }
                                else
                                {
                                    ShowMsgBox(Convert.ToString(Arr[0]));
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        ShowMsgBox("Password Length Should 8 Character and  contains at least 1 Capital Letter or 1 Small Letter,1 Digit, 1 Special Character");
                    }
                    // }
                }
                else
                {
                    Response.Redirect("~/Login.aspx", false);
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnsubmit_Click");
            }
        }

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;

            if (txtOldPwd.Text.Length == 0)
            {
                txtOldPwd.Focus();
                ShowMsgBox("Please Enter Current Password");
                return false;
            }


            if (txtNewPwd.Text.Length == 0)
            {
                txtNewPwd.Focus();
                ShowMsgBox("Please Enter New Password");
                return false;
            }
            if (txtConfirmPwd.Text.Trim().Length == 0)
            {
                txtConfirmPwd.Focus();
                ShowMsgBox("Please Enter Confirm Password");
                return false;
            }
            if (txtOldPwd.Text == txtNewPwd.Text)
            {
                ShowMsgBox("New Password & Current password should not be same");
                txtConfirmPwd.Focus();
                return false;
            }
            if (txtNewPwd.Text != txtConfirmPwd.Text)
            {
                ShowMsgBox("New Password & Confirm Password do not match");
                txtConfirmPwd.Focus();
                return false;
            }
       
            if (txtNewPwd.Text.Length < 8)
            {
                ShowMsgBox("Passwords Length Should be greater than 7 characters.");
                txtNewPwd.Focus();
                return false;
            }

            bValidate = true;
            return bValidate;
        }

    }
}
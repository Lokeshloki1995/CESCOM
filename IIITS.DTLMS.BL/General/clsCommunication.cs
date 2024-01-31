using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Configuration;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Mail;
using System.Data;

namespace IIITS.DTLMS.BL
{
    public class clsCommunication
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clscommunication";
        OleDbCommand oledbCommand;
        public string sSMSTemplate { get; set; }
        public string sSMSTemplateID { get; set; }
        public string sSMSkey { get; set; }
        public string sendSMS(string SMS, string Mobileno, string strUserName)
        {

            try
            {
                string strMobile1 = "91" + Mobileno;
                string sResult = SMSSendFuction(strMobile1, SMS);
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                string sMaxNo = Convert.ToString(ObjCon.Get_max_no("SMS_ID", "TBLSMSLOG"));
                strQry = "INSERT INTO TBLSMSLOG (SMS_ID,SMS_PHONE_NO,SMS_MESSAGE,SMS_STATUS) VALUES (";
                strQry += " :sMaxNo,:Mobileno,:SMS,:sResult)";
                oledbCommand.Parameters.AddWithValue("sMaxNo", sMaxNo);
                oledbCommand.Parameters.AddWithValue("Mobileno", Mobileno);
                oledbCommand.Parameters.AddWithValue("SMS", SMS);
                oledbCommand.Parameters.AddWithValue("sResult", sResult);
                ObjCon.Execute(strQry, oledbCommand);
                return "Sent Successfully";
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "sendSMS");
            }
            return "";
        }
        public clsCommunication GetsmsTempalte(clsCommunication objcom)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    oledbCommand = new OleDbCommand();
                    strQry = "SELECT SMT_TRANSACTION_ID,SMT_TEMPLATE FROM TBLSMSTEMPLATE WHERE SMT_STATUS=1 AND SMT_KEY=:smskey";
                    oledbCommand.Parameters.AddWithValue("smskey", objcom.sSMSkey);
                    dt = ObjCon.getDataTable(strQry, oledbCommand);

                    if (dt.Rows.Count > 0)
                    {
                        objcom.sSMSTemplate = Convert.ToString(dt.Rows[0]["SMT_TEMPLATE"]);
                        objcom.sSMSTemplateID = Convert.ToString(dt.Rows[0]["SMT_TRANSACTION_ID"]);
                    }
                }
                return objcom;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objcom;
            }

        }

        public clsCommunication GetsmsTempalte1(clsCommunication objcom, CustOledbConnection objconn)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    oledbCommand = new OleDbCommand();
                    strQry = "SELECT SMT_TRANSACTION_ID,SMT_TEMPLATE FROM TBLSMSTEMPLATE WHERE SMT_STATUS=1 AND SMT_KEY=:smskey";
                    oledbCommand.Parameters.AddWithValue("smskey", objcom.sSMSkey);
                    dt = objconn.getDataTable(strQry, oledbCommand);

                    if (dt.Rows.Count > 0)
                    {
                        objcom.sSMSTemplate = Convert.ToString(dt.Rows[0]["SMT_TEMPLATE"]);
                        objcom.sSMSTemplateID = Convert.ToString(dt.Rows[0]["SMT_TRANSACTION_ID"]);
                    }
                }
                return objcom;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objcom;
            }

        }
        public void DumpSms(string MobileNo, string SMSTEXT, string TemplateId)
        {
            try
            {
                if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    // IDADTC new old DTCLMS
                    oledbCommand = new OleDbCommand();
                    if ((SMSTEXT??"").Length>0)
                    {
                        long strID = ObjCon.Get_max_no("TS_ID", "TBLSMSDUMP");
                        string strQry = string.Empty;
                        strQry = "INSERT INTO TBLSMSDUMP(TS_ID,TS_MOBILE_NUMBER,TS_CONTENT,TS_SENDER_ID,TS_SENDER_TYPE,TS_TEMPLATEID) VALUES(";
                        strQry += " :strID,:MobileNo, :SMSTEXT,'IDADTC','WEB',:TemplateId)";
                        oledbCommand.Parameters.AddWithValue("strID", strID);
                        oledbCommand.Parameters.AddWithValue("MobileNo", MobileNo);
                        oledbCommand.Parameters.AddWithValue("SMSTEXT", SMSTEXT);
                        oledbCommand.Parameters.AddWithValue("TemplateId", TemplateId);
                        ObjCon.Execute(strQry, oledbCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DumpSms");
            }

        }

        public void DumpSms1(string MobileNo, string SMSTEXT, string TemplateId, CustOledbConnection objconn)
        {
            try
            {
                if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    // IDADTC new old DTCLMS
                    oledbCommand = new OleDbCommand();
                    if ((SMSTEXT ?? "").Length > 0)
                    {
                        long strID = objconn.Get_max_no("TS_ID", "TBLSMSDUMP");
                        string strQry = string.Empty;
                        strQry = "INSERT INTO TBLSMSDUMP(TS_ID,TS_MOBILE_NUMBER,TS_CONTENT,TS_SENDER_ID,TS_SENDER_TYPE,TS_TEMPLATEID) VALUES(";
                        strQry += " :strID,:MobileNo, :SMSTEXT,'IDADTC','WEB',:TemplateId)";
                        oledbCommand.Parameters.AddWithValue("strID", strID);
                        oledbCommand.Parameters.AddWithValue("MobileNo", MobileNo);
                        oledbCommand.Parameters.AddWithValue("SMSTEXT", SMSTEXT);
                        oledbCommand.Parameters.AddWithValue("TemplateId", TemplateId);
                        objconn.Execute(strQry, oledbCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DumpSms1");
            }

        }
        public void DumpSms(string MobileNo, string SMSTEXT)
        {
            try
            {
                // IDADTC new  old  DTCLMS 
                oledbCommand = new OleDbCommand();
                if ((SMSTEXT ?? "").Length > 0)
                {
                    long strID = ObjCon.Get_max_no("TS_ID", "TBLSMSDUMP");
                    string strQry = string.Empty;
                    strQry = "INSERT INTO TBLSMSDUMP(TS_ID,TS_MOBILE_NUMBER,TS_CONTENT,TS_SENDER_ID,TS_SENDER_TYPE) VALUES(";
                    strQry += " :strID,:MobileNo, :SMSTEXT,'IDADTC','WEB')";
                    oledbCommand.Parameters.AddWithValue("strID", strID);
                    oledbCommand.Parameters.AddWithValue("MobileNo", MobileNo);
                    oledbCommand.Parameters.AddWithValue("SMSTEXT", SMSTEXT);
                    ObjCon.Execute(strQry, oledbCommand);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DumpSms");
            }

        }


        private string SMSSendFuction(string MobileNo, string SMSTEXT)
        {
            string strResult = string.Empty;
            try
            {
                if (SMSTEXT.Contains("#"))
                {
                    SMSTEXT = SMSTEXT.Replace("#", "%23");
                }
                if (SMSTEXT.Contains("&"))
                {
                    SMSTEXT = SMSTEXT.Replace("&", "%26");
                }
                string strSenderId = Convert.ToString(ConfigurationSettings.AppSettings["VENDOR_SENDERID"]);

                if (Convert.ToString(ConfigurationSettings.AppSettings["SendSMS"]).ToUpper().Equals("ON"))
                {
                    string strUsername = Convert.ToString(ConfigurationSettings.AppSettings["VENDOR_USERNAME"]);
                    string strPassword = Convert.ToString(ConfigurationSettings.AppSettings["VENDOR_PASS"]);

                    string strLog = "sendSMS : '~' Mobile No : " + MobileNo + ", Subject : " + SMSTEXT + "";
                    string baseurl = "http://smslogin.mobi/spanelv2/api.php?username=" + strUsername + "&password=" + strPassword;
                    baseurl += "&to=" + System.Uri.EscapeUriString(MobileNo) + "&from=" + strSenderId + "&message=";
                    baseurl += "" + System.Uri.EscapeUriString(SMSTEXT) + "";
                    String result = GetPageContent(baseurl);
                    if (result == "Invalid Parameters")
                    {
                        strResult = "Error - Invalid MobileNumber";
                    }
                    else
                    {
                        strResult = GetPageContent("http://smslogin.mobi/spanelv2/api.php?username=" + strUsername + "&password=" + strPassword + "&msgid=" + result);
                    }
                }
                return strResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SMSSendFuction");
                return "";
            }
        }

        private static string GetPageContent(string FullUri)
        {
            HttpWebRequest Request;
            StreamReader ResponseReader;
            Request = ((HttpWebRequest)(WebRequest.Create(FullUri)));
            ResponseReader = new StreamReader(Request.GetResponse().GetResponseStream());
            return ResponseReader.ReadToEnd();
        }
        /// <summary>
        /// Code for Sending Email
        /// </summary>
        /// <param name="strSubject"></param>
        /// <param name="strMailid"></param>
        /// <param name="strMailMsg"></param>
        /// <returns></returns>
        public string[] SendMail(string strSubject, string strMailid, string strMailMsg, string sCrby)
        {
            string[] MasilStatusResult = new string[2];
            try
            {
                if (Convert.ToString(ConfigurationManager.AppSettings["SendEmail"]).ToUpper().Equals("ON"))
                {
                    #region Fearther purpose to trigger a mail
                    //MailMessage mail = new MailMessage();
                    //mail.To.Add(strMailid);
                    //mail.From = new MailAddress(
                    //    Convert.ToString(ConfigurationManager.AppSettings["SENDMAILID"]),
                    //    "CESCOM~DTLMS"
                    //    );
                    //mail.Subject = strSubject;
                    //mail.IsBodyHtml = true;
                    //string Body = strMailMsg;
                    //mail.Body = Body;
                    //mail.IsBodyHtml = true;
                    //SmtpClient smtp = new SmtpClient(
                    //    Convert.ToString(ConfigurationManager.AppSettings["SENDSMTP"]),
                    //    Convert.ToInt32(ConfigurationManager.AppSettings["SENDSMTPPORT"])
                    //    );
                    ////Password need to be entered
                    //smtp.Credentials = new System.Net.NetworkCredential(
                    //    Convert.ToString(ConfigurationManager.AppSettings["SENDMAILID"]),
                    //    Convert.ToString(ConfigurationManager.AppSettings["SENDPWD"])
                    //    );
                    //smtp.EnableSsl = true;
                    //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    //smtp.Send(mail);
                    #endregion

                    #region This Code is not working as Expeted
                    MailMessage mail = new MailMessage();
                    mail.To.Add(strMailid);
                    mail.From = new MailAddress("dtlms.cesc@gmail.com", "DTLMS");
                    mail.Subject = strSubject;
                    mail.IsBodyHtml = true;
                    string Body = strMailMsg;
                    mail.Body = Body;
                    //mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential("dtlms.cesc@gmail.com", "idea@123");
                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    #endregion

                    MasilStatusResult[0] = "0";
                    MasilStatusResult[1] = "Mail Sent SuccessFully";

                    //inserting Maile sent records to DataBase.
                    oledbCommand = new OleDbCommand();
                    long sMaxNo = ObjCon.Get_max_no("EM_ID", "TBLEMAILLOG");
                    string strQry = string.Empty;
                    strQry = "INSERT INTO TBLEMAILLOG(EM_ID,EM_EMAIL_ID,EM_SUBJECT,EM_CRBY) VALUES (";
                    strQry += " :sMaxNo,:strMailid,:strSubject,:sCrby)";
                    oledbCommand.Parameters.AddWithValue("sMaxNo", sMaxNo);
                    oledbCommand.Parameters.AddWithValue("strMailid", strMailid);
                    oledbCommand.Parameters.AddWithValue("strSubject", strSubject);
                    oledbCommand.Parameters.AddWithValue("sCrby", sCrby);
                    ObjCon.Execute(strQry, oledbCommand);

                }else
                {
                    MasilStatusResult[0] = "1";
                    MasilStatusResult[1] = "Send Email Feacher is OFF";
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "clsCommunication", "SendMail");
                MasilStatusResult[0] = "-1";
                MasilStatusResult[1] = "Failed to send Mail";

            }
            return MasilStatusResult;

        }

        public void SaveSMSDetails(string sMobileNo, string sMessage, string sRoleId)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                long sMaxNo = ObjCon.Get_max_no("SMD_ID", "TBLSMSDUMP");
                string strQry = string.Empty;

                // For Reference Saving RoleId also. It will be usefull in future
                strQry = "INSERT INTO TBLSMSDUMP(SMD_ID,SMD_PHONE_NO,SMD_MESSAGE,SMD_ENTRY_DATE,SMD_ROLE_ID) VALUES (";
                strQry += " :sMaxNo,:sMobileNo,:sMessage,SYSDATE,:sRoleId)";
                oledbCommand.Parameters.AddWithValue("sMaxNo", sMaxNo);
                oledbCommand.Parameters.AddWithValue("sMobileNo", sMobileNo);
                oledbCommand.Parameters.AddWithValue("sMessage", sMessage);

                oledbCommand.Parameters.AddWithValue("sRoleId", sRoleId);

                ObjCon.Execute(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveSMSDetails");

            }
        }

    }
}

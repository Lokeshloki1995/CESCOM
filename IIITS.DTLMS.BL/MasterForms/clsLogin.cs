using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.BL
{
    public class clsLogin
    {
        public string sLoginName { get; set; }
        public string sFullName { get; set; }
        public string sOfficeCode { get; set; }
        public string sUserType { get; set; }
        public string sUserId { get; set; }
        public string sPassword { get; set; }
        public string sMessage { get; set; }
        public string sEmail { get; set; }
        public string sRoleId { get; set; }
        public string sOfficeName { get; set; }
        public string sDesignation { get; set; }
        public string sMobileNo { get; set; }
        public string sOfficeNamewithType { get; set; }
        public string sChangePwd { get; set; }
        public string sOTP { get; set; }
        public string sUnencryptedPwd { set; get; }
        public string sResult { get; set; }
        public bool VLDEmail_Or_Mob { set; get; }
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objLogin"></param>
        public void SavePassword(clsLogin objLogin)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "UPDATE TBLUSER set US_PASSWORD_REF = '" + objLogin.sUnencryptedPwd + "' WHERE US_ID =  '" + objLogin.sUserId + "'";
                ObjCon.Execute(strQry);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objLogin"></param>
        public void Updatelastlogin(clsLogin objLogin)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "UPDATE TBLUSER set US_LAST_LOGIN = SYSDATE WHERE US_ID =  '" + objLogin.sUserId + "'";
                ObjCon.Execute(strQry);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        #region newPwdEncryption
        public clsLogin UserLogin(clsLogin objLogin)
        {
            try
            {
                string sQry = string.Empty;
                string sEffectFrom = string.Empty;
                string sStatus = string.Empty;
                DataTable dt = new DataTable();
                bool Passwordstatus = false;
                bool wrongPwd = false;
                oledbCommand = new OleDbCommand();
                sQry = "SELECT US_ID,US_FULL_NAME,US_OFFICE_CODE,US_ROLE_ID,TO_CHAR(US_EFFECT_FROM,'DD/MM/YYYY') US_EFFECT_FROM,US_STATUS, US_PWD ,  ";
                sQry += " US_CHPWD_ON,(SELECT DM_NAME FROM TBLDESIGNMAST WHERE DM_DESGN_ID=US_DESG_ID) DM_NAME";
                sQry += " FROM TBLUSER ";
                sQry += " WHERE UPPER(US_LG_NAME)=:LoginName ";
                oledbCommand.Parameters.AddWithValue("LoginName", objLogin.sLoginName.ToUpper());
                dt = ObjCon.getDataTable(sQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    Passwordstatus = Genaral.CompareLogin(Convert.ToString(dt.Rows[0]["US_PWD"]), objLogin.sPassword);
                    sEffectFrom = Convert.ToString(dt.Rows[0]["US_EFFECT_FROM"]);
                    sStatus = Convert.ToString(dt.Rows[0]["US_STATUS"]);
                }
                else
                {
                    objLogin.sMessage = "Please enter Valid User Name or Password ";
                    return objLogin;
                }
                if (Passwordstatus == false)
                {
                    wrongPwd = true;
                }
                if ((sStatus == "A") && (!wrongPwd))
                {
                    objLogin.sUserId = dt.Rows[0]["US_ID"].ToString();
                    objLogin.sFullName = dt.Rows[0]["US_FULL_NAME"].ToString();
                    objLogin.sOfficeCode = dt.Rows[0]["US_OFFICE_CODE"].ToString();
                    objLogin.sRoleId = dt.Rows[0]["US_ROLE_ID"].ToString();
                    objLogin.sOfficeName = Getofficename(dt.Rows[0]["US_OFFICE_CODE"].ToString());
                    objLogin.sDesignation = dt.Rows[0]["DM_NAME"].ToString();
                    objLogin.sChangePwd = dt.Rows[0]["US_CHPWD_ON"].ToString();
                    objLogin.sOfficeNamewithType = GetofficeNameWithType(objLogin.sOfficeCode);
                    oledbCommand = new OleDbCommand();
                    sQry = "UPDATE TBLUSERLOGINATTEMPT SET ULA_STATUS = 1  WHERE ULA_USER_ID = :UserId ";
                    oledbCommand.Parameters.AddWithValue("UserId", objLogin.sUserId);
                    ObjCon.Execute(sQry, oledbCommand);
                    oledbCommand = new OleDbCommand();
                    sQry = "";
                }
                else if (wrongPwd)
                {
                    objLogin.sMessage = "Please enter Valid User Name or Password ";
                    UpdateUserLoginAttempts(objLogin);
                    return objLogin;
                }
                else
                {
                    objLogin.sMessage = "User is Disabled,Please contact Administrator";
                }
                return objLogin;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                objLogin.sMessage = "An exception occurred while processing your request.";
                return objLogin;
            }
        }

        //Check if the user has entered the wrong password with in 5 min then disable the user .
        private void UpdateUserLoginAttempts(clsLogin objLogin)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("LoginName1", objLogin.sLoginName.ToUpper());
                string strQry = "SELECT US_ID FROM TBLUSER WHERE UPPER(US_LG_NAME)=:LoginName1 AND US_STATUS='A'";
                string userID = ObjCon.get_value(strQry, oledbCommand);
                int loginattempts = 0;
                int TotalAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttempts"]);
                int TotalSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttemptsTimeRange"]);
                int LoginAttemptsApply = Convert.ToInt32(ConfigurationManager.AppSettings["LoginAttemptsApply"]);
                int pendingAttempts = 0;
                string res = string.Empty;
                int dateDifference = 0;

                if (userID != "" && userID != null)
                {
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("UserID1", userID);
                    strQry = "SELECT  round((SYSDATE - ula_cron) * 24 * 60 * 60)    FROM (SELECT ula_id,ula_cron,row_number() over (PARTITION by ula_user_id ORDER BY ula_attempts desc) row_number FROM tbluserloginattempt WHERE ula_user_id=:UserID1 and ula_status = 0)A WHERE row_number=1";
                    res = ObjCon.get_value(strQry, oledbCommand);
                    if (res != null && res != "")
                    {
                        dateDifference = Convert.ToInt32(res);
                    }
                    if (dateDifference < TotalSeconds)
                    {
                        oledbCommand = new OleDbCommand();
                        oledbCommand.Parameters.AddWithValue("UserID2", userID);
                        strQry = "SELECT COALESCE(max(ula_attempts),0)+1 FROM tbluserloginattempt WHERE ula_user_id=:UserID2 AND ula_status =0";
                        loginattempts = Convert.ToInt32(ObjCon.get_value(strQry, oledbCommand));
                        oledbCommand = new OleDbCommand();
                        oledbCommand.Parameters.AddWithValue("Userid", userID);
                        oledbCommand.Parameters.AddWithValue("loginattempt", loginattempts);
                        strQry = "INSERT into tbluserloginattempt (ula_id,ula_user_id,ula_attempts,ula_cron) VALUES((SELECT COALESCE(max(ula_id),0)+1 FROM tbluserloginattempt),";
                        strQry += " :Userid,:loginattempt,SYSDATE)";
                        ObjCon.Execute(strQry, oledbCommand);
                    }
                    else
                    {
                        oledbCommand = new OleDbCommand();
                        oledbCommand.Parameters.AddWithValue("Userid1", userID);
                        strQry = "UPDATE tbluserloginattempt set ula_status=1 WHERE ula_user_id=:Userid1 and ula_status =0";
                        ObjCon.Execute(strQry, oledbCommand);
                        oledbCommand = new OleDbCommand();
                        oledbCommand.Parameters.AddWithValue("UserID12", userID);
                        strQry = "SELECT COALESCE(max(ula_attempts),0)+1 FROM tbluserloginattempt WHERE ula_user_id=:UserID12 AND ula_status =0";
                        loginattempts = Convert.ToInt32(ObjCon.get_value(strQry, oledbCommand));
                        oledbCommand = new OleDbCommand();
                        oledbCommand.Parameters.AddWithValue("Userid2", userID);
                        oledbCommand.Parameters.AddWithValue("loginattempt1", loginattempts);
                        strQry = "INSERT into tbluserloginattempt (ula_id,ula_user_id,ula_attempts,ula_cron) VALUES((SELECT COALESCE(max(ula_id),0)+1 FROM tbluserloginattempt),";
                        strQry += " :Userid2,:loginattempt1,SYSDATE)";
                        ObjCon.Execute(strQry, oledbCommand);
                    }
                }
                if (loginattempts != 0 && (loginattempts > LoginAttemptsApply && loginattempts <= (TotalAttempts - 1)))
                {
                    pendingAttempts = TotalAttempts - loginattempts;
                    objLogin.sMessage = "You have " + pendingAttempts + " more Attempts left ,Enter Valid User Name and Password";
                }
                else if (loginattempts == TotalAttempts)
                {
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("UserID3", userID);
                    strQry = "UPDATE TBLUSER set US_REASON = '5 Time Wrong Password' , US_DEACTIVATED_DATE = SYSDATE,  US_STATUS='D' where US_ID= " + userID + " AND US_STATUS='A' ";
                    ObjCon.Execute(strQry);
                    oledbCommand = new OleDbCommand();
                    oledbCommand.Parameters.AddWithValue("UserID4", userID);
                    strQry = "UPDATE tbluserloginattempt set ula_status=1 WHERE ula_user_id=:UserID4 and ula_status =0";
                    ObjCon.Execute(strQry, oledbCommand);
                    objLogin.sMessage = "Your Accout has been Locked, kindly contact DTLMS Support";
                }
                else
                {
                    objLogin.sMessage = "Enter Valid User Name or Password";
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
            }
        }
        #endregion


        //// Compare the Password which is given by User and saved Password in DataBase
        public bool CompareLogin(string sActualPassword, string sGivenPassword)
        {
            try
            {
                string sEncGivenPassword = string.Empty;
                byte[] hashbbytes = Convert.FromBase64String(sActualPassword);
                // Take the salt out of string
                byte[] salt = new byte[16];
                Array.Copy(hashbbytes, 0, salt, 0, 16);
                // Hash the user input pw with salt
                var pwdwithsalt = new Rfc2898DeriveBytes(sGivenPassword, salt, 10000);
                //put the hashed input in a byte array to compare with byte-byte
                byte[] hash = pwdwithsalt.GetBytes(20);
                sEncGivenPassword = Convert.ToBase64String(hash);
                byte[] PwdByte = new byte[36];
                Array.Copy(salt, 0, PwdByte, 0, 16);
                Array.Copy(hash, 0, PwdByte, 16, 20);
                string finalsavepwd = Convert.ToBase64String(PwdByte);
                sEncGivenPassword = finalsavepwd;
                int ok = 1;
                for (int i = 0; i < 20; i++)

                    if (hashbbytes[i + 16] != hash[i])
                        ok = 0;
                if (ok == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return false;
            }
        }
        // Logic for Password Exception
        public static string EncryptPassword(string sPassword)
        {
            // hash is 20 bytes, and the salt 16.
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            //HASH AND SALT IT USING PBKDF2
            var pwwithsalt = new Rfc2898DeriveBytes(sPassword, salt, 10000);
            //place the string in the byte array 
            byte[] hash = pwwithsalt.GetBytes(20);
            // make new byte array where to store the hashed password + salt 
            // why 36 cos hash is 20 bytes, and the salt 16.
            byte[] hashbytes = new byte[36];
            // place the hash and password at respective places 
            Array.Copy(salt, 0, hashbytes, 0, 16);
            Array.Copy(hash, 0, hashbytes, 16, 20);
            string finalsavepwd = Convert.ToBase64String(hashbytes);
            return finalsavepwd;
        }

        public void UpdateOldPwd()
        {
            string sQry = string.Empty;
            DataTable dtUsers = new DataTable();
            sQry = "SELECT US_ID, US_OLD_PWD,US_PWD FROM TBLUSER  ";
            dtUsers = ObjCon.getDataTable(sQry);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string sUserid = Convert.ToString(dtUsers.Rows[i]["US_ID"]);
                string sPassword = Convert.ToString(dtUsers.Rows[i]["US_OLD_PWD"]);
                string sEncPass = string.Empty;
                //this will return the encrypted string
                int n, j;
                string temp;
                temp = "";
                n = sPassword.Length;
                for (j = 0; j < n; j++)
                {
                    temp = temp + (char)((int)sPassword[j] - 123);
                }
                sPassword = temp;
                // hash is 20 bytes, and the salt 16.
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                //HASH AND SALT IT USING PBKDF2
                var pwwithsalt = new Rfc2898DeriveBytes(sPassword, salt, 10000);
                //place the string in the byte array 
                byte[] hash = pwwithsalt.GetBytes(20);
                // make new byte array where to store the hashed password + salt 
                // why 36 cos hash is 20 bytes, and the salt 16.
                byte[] hashbytes = new byte[36];
                // place the hash and password at respective places 
                Array.Copy(salt, 0, hashbytes, 0, 16);
                Array.Copy(hash, 0, hashbytes, 16, 20);
                string finalsavepwd = Convert.ToBase64String(hashbytes);
                sEncPass = finalsavepwd;
                sQry = "UPDATE TBLUSER SET US_PWD = '" + sEncPass + "' WHERE US_ID = '" + sUserid + "'";
                ObjCon.Execute(sQry);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sOldPassword"></param>
        /// <param name="sUserId"></param>
        /// <returns></returns>
        public bool GetStatus(string sOldPassword, string sUserId)
        {
            oledbCommand = new OleDbCommand();
            string strQry = string.Empty;
            try
            {
                oledbCommand.Parameters.AddWithValue("psw", sOldPassword);
                oledbCommand.Parameters.AddWithValue("usid", sUserId);
                strQry = "SELECT \"UOP_ID\" FROM \"TBLUSER_OLD_PASSWORD\" WHERE \"UOP_PWD\"=:psw AND \"UOP_US_ID\"=:usid";
                string sRes = ObjCon.get_value(strQry, oledbCommand);
                if (sRes != null && sRes != "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sOtp"></param>
        /// <returns></returns>
        public DataTable GetOTPDetails(string sOtp, string UserName)
        {
            oledbCommand = new OleDbCommand();
            OleDbCommand cmd = new OleDbCommand();
            DataTable dtOTPDetails = new DataTable();
            string sQry = string.Empty;
            try
            {
                string QryUser = " SELECT US_ID  FROM TBLUSER WHERE US_LG_NAME =:UserName ||'' ";
                oledbCommand.Parameters.AddWithValue("UserName", UserName.ToUpper());
                int userId = Convert.ToInt32(ObjCon.get_value(QryUser, oledbCommand));

               
                sQry = "SELECT * FROM tblotp WHERE otp_no=:Otp and otp_sent_flag='0' and otp_us_id=:P_UserId";
                cmd.Parameters.AddWithValue("Otp", sOtp);
                cmd.Parameters.AddWithValue("P_UserId", userId);
                dtOTPDetails = ObjCon.getDataTable(sQry, cmd);
                return dtOTPDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtOTPDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetConfiguration()
        {
            DataTable dtConfiguration = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT * FROM \"TBLCONFIGURATION\"";
                return ObjCon.getDataTable(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtConfiguration;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public clsLogin MMSUserLogin(clsLogin objLogin)
        {
            try
            {

                oledbCommand = new OleDbCommand();
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                bool bActiveResult = false;
                sQry = "SELECT US_ID,US_FULL_NAME,US_OFFICE_CODE,US_ROLE_ID,TO_CHAR(US_EFFECT_FROM,'DD/MM/YYYY') US_EFFECT_FROM,US_STATUS, ";
                sQry += " US_CHPWD_ON,(SELECT DM_NAME FROM TBLDESIGNMAST WHERE DM_DESGN_ID=US_DESG_ID) DM_NAME";
                sQry += " FROM TBLUSER WHERE US_ID=:UserID";
                oledbCommand.Parameters.AddWithValue("UserID", objLogin.sUserId);
                dt = ObjCon.getDataTable(sQry);
                if (dt.Rows.Count > 0)
                {
                    //Check for EffectFrom Condition
                    string sEffectFrom = Convert.ToString(dt.Rows[0]["US_EFFECT_FROM"]);
                    string sStatus = Convert.ToString(dt.Rows[0]["US_STATUS"]);
                    if (sEffectFrom != "" && sStatus == "D")
                    {
                        string sResult = Genaral.DateComparision(sEffectFrom, "", true, false);
                        if (sResult == "1")
                        {
                            bActiveResult = true;
                            sStatus = "A";
                        }
                    }
                    if (sStatus == "A" || bActiveResult == true)
                    {
                        objLogin.sUserId = dt.Rows[0]["US_ID"].ToString();
                        objLogin.sFullName = dt.Rows[0]["US_FULL_NAME"].ToString();
                        objLogin.sOfficeCode = dt.Rows[0]["US_OFFICE_CODE"].ToString();
                        objLogin.sRoleId = dt.Rows[0]["US_ROLE_ID"].ToString();
                        objLogin.sOfficeName = Getofficename(dt.Rows[0]["US_OFFICE_CODE"].ToString());
                        objLogin.sDesignation = dt.Rows[0]["DM_NAME"].ToString();
                        objLogin.sChangePwd = dt.Rows[0]["US_CHPWD_ON"].ToString();
                        objLogin.sOfficeNamewithType = GetofficeNameWithType(objLogin.sOfficeCode);
                    }
                    else
                    {
                        objLogin.sMessage = "User is Disabled,Please contact Administrator";
                    }
                }
                else
                {
                    objLogin.sMessage = "Enter Valid User Name and Password";
                }
                return objLogin;
            }
            catch (Exception ex)
            {
                objLogin.sMessage = ex.Message;
                return objLogin;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sOfficecode"></param>
        /// <returns></returns>
        public string Getofficename(string sOfficecode)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+1) AS OFFICENAME  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=:OfficeCode";
                oledbCommand.Parameters.AddWithValue("OfficeCode", sOfficecode);
                string Offname = ObjCon.get_value(strQry, oledbCommand);

                if (Offname == null || Offname == "")
                {
                    Offname = "CORP OFFICE";
                }
                return Offname;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return ex.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sOfficecode"></param>
        /// <returns></returns>
        public string GetofficeNameWithType(string sOfficecode)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT OFF_NAME AS OFFICENAME  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=:OfficeCode";
                oledbCommand.Parameters.AddWithValue("OfficeCode", sOfficecode);
                string Offname = ObjCon.get_value(strQry, oledbCommand);
                if (Offname == null || Offname == "")
                {
                    Offname = "CORP OFFICE";
                }
                return Offname;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return ex.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public clsLogin ForgtPassword(clsLogin objLogin)
        {
            try
            {
                string[] Arr = new string[2];
                oledbCommand = new OleDbCommand();
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                Regex r1 = new Regex(@"^\d+$");
                string sPattern = @"^\d{10}$";
                Regex r = new Regex(sPattern);
                if (r.IsMatch(objLogin.sEmail))
                {
                    sQry = "SELECT US_FULL_NAME,US_LG_NAME,US_PWD,US_ID,US_MOBILE_NO FROM TBLUSER where US_STATUS='A' AND US_MOBILE_NO=:MobileNO";
                    oledbCommand.Parameters.AddWithValue("MobileNO", objLogin.sEmail);
                    dt = ObjCon.getDataTable(sQry, oledbCommand);
                    if (dt.Rows.Count > 0)
                    {
                        objLogin.sPassword = dt.Rows[0]["US_PWD"].ToString();
                        objLogin.sFullName = dt.Rows[0]["US_FULL_NAME"].ToString();
                        objLogin.sLoginName = dt.Rows[0]["US_LG_NAME"].ToString();
                        objLogin.sUserId = dt.Rows[0]["US_ID"].ToString();
                        objLogin.sMobileNo = dt.Rows[0]["US_MOBILE_NO"].ToString();
                        //SendMailForgotPwd(objLogin);
                        clsCommunication objComm = new clsCommunication();
                        Random generator = new Random();
                        int OTP = generator.Next(100000, 1234567);
                        Random random = new Random();
                        int num = random.Next(0, 26);
                        int num1 = random.Next(0, 26);
                        char let = (char)('A' + num);
                        char let1 = (char)('A' + num1);
                        objLogin.sOTP = Convert.ToString(OTP) + let + let1;
                        objLogin.sOTP = Shuffle(objLogin.sOTP);
                        //string sSMSText = String.Format(Convert.ToString(ConfigurationSettings.AppSettings["SMStoOTP"]), objLogin.sFullName, objLogin.sOTP);
                        string sSMSText = string.Empty;
                        objComm.sSMSkey = "SMStoOTP";
                        objComm = objComm.GetsmsTempalte(objComm);
                        sSMSText = String.Format(objComm.sSMSTemplate, objLogin.sFullName, objLogin.sOTP);
                        oledbCommand = new OleDbCommand();
                        //sQry = "SELECT otp_sent_flag FROM tblotp WHERE otp_us_id='"+ objLogin.sUserId + "'";
                        sQry = "SELECT to_char(otp_cron,'yyyy-MM-dd HH24:mi') || '~' || otp_sent_flag  FROM (SELECT otp_cron,otp_sent_flag, otp_id,row_number() over(partition by otp_us_id ORDER BY otp_id desc) as rownum555 FROM tblotp WHERE otp_us_id= :userId )A WHERE rownum555=1 ";
                        oledbCommand.Parameters.AddWithValue("userId", objLogin.sUserId);
                        string sSentFlag = ObjCon.get_value(sQry, oledbCommand);
                        if (sSentFlag == "")
                        {
                            oledbCommand = new OleDbCommand();
                            oledbCommand.Parameters.AddWithValue("userId1", objLogin.sUserId);
                            oledbCommand.Parameters.AddWithValue("OTP", objLogin.sOTP);
                            sQry = "INSERT INTO tblotp (OTP_ID,otp_us_id,otp_no,otp_cron,otp_sent_flag,otp_change_pwd_on) VALUES((SELECT nvl(max(otp_id),0)+1 FROM tblotp),:userId1,:OTP,SYSDATE,'0',SYSDATE)";
                            ObjCon.Execute(sQry, oledbCommand);
                            objComm.DumpSms(objLogin.sMobileNo, sSMSText, objComm.sSMSTemplateID);
                            objLogin.sMessage = "OTP has been Sent to your mobile number";
                        }
                        else
                        {
                            if (Convert.ToString(sSentFlag.Split('~').GetValue(1)) == "1" || Convert.ToString(sSentFlag.Split('~').GetValue(1)) == "")
                            {
                                oledbCommand = new OleDbCommand();
                                oledbCommand.Parameters.AddWithValue("userId2", Convert.ToInt16(objLogin.sUserId));
                                oledbCommand.Parameters.AddWithValue("OTP1", objLogin.sOTP);
                                sQry = "INSERT INTO tblotp (OTP_ID , otp_us_id,otp_no,otp_cron,otp_sent_flag,otp_change_pwd_on) VALUES((SELECT nvl(max(otp_id),0)+1 FROM tblotp),:userId2,:OTP1,SYSDATE,'0',SYSDATE)";
                                ObjCon.Execute(sQry, oledbCommand);
                                objComm.DumpSms(objLogin.sMobileNo, sSMSText, objComm.sSMSTemplateID);
                                objLogin.sMessage = "OTP has been Sent to your mobile number";
                            }
                            else
                            {
                                DateTime PrevOTP_DATE = DateTime.ParseExact(Convert.ToString(sSentFlag.Split('~').GetValue(0)), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                                DateTime Now_DATE = DateTime.Now;
                                TimeSpan finalres = Now_DATE - PrevOTP_DATE;
                                int iTotalSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["TotalSeconds"]);
                                if (finalres.TotalSeconds < iTotalSeconds)
                                {
                                    objLogin.sMessage = "OTP Already Sent to your mobile number please try later";
                                    objLogin.sResult = "-1";
                                }
                                else
                                {
                                    oledbCommand = new OleDbCommand();
                                    oledbCommand.Parameters.AddWithValue("mobileNo", objLogin.sMobileNo);
                                    sQry = "SELECT TS_ID FROM TBLSMSDUMP WHERE TS_MOBILE_NUMBER=:mobileNo AND TS_CONTENT LIKE '%OTP%' AND TS_SENT_FLAG='0'";
                                    string sDump_id = ObjCon.get_value(sQry, oledbCommand);
                                    if (sDump_id != "" && sDump_id != null)
                                    {
                                        oledbCommand = new OleDbCommand();
                                        oledbCommand.Parameters.AddWithValue("Dump_id", Convert.ToInt32(sDump_id));
                                        sQry = "UPDATE TBLSMSDUMP  SET TS_OTP_FLAG='1',TS_SENT_FLAG='1' WHERE TS_ID=:Dump_id";
                                        ObjCon.Execute(sQry, oledbCommand);
                                    }
                                    oledbCommand = new OleDbCommand();
                                    oledbCommand.Parameters.AddWithValue("UserId", Convert.ToInt16(objLogin.sUserId));
                                    sQry = "UPDATE tblotp set otp_cancel_flag='1',otp_sent_flag='1' WHERE otp_us_id=:UserId";
                                    ObjCon.Execute(sQry, oledbCommand);
                                    oledbCommand = new OleDbCommand();
                                    oledbCommand.Parameters.AddWithValue("UserId1", Convert.ToInt16(objLogin.sUserId));
                                    oledbCommand.Parameters.AddWithValue("OTP2", objLogin.sOTP);
                                    sQry = "INSERT INTO tblotp (otp_id,otp_us_id,otp_no,otp_cron,otp_sent_flag,otp_change_pwd_on) VALUES((SELECT nvl(max(otp_id),0)+1 FROM tblotp),:UserId1,:OTP2,SYSDATE,'0',SYSDATE)";
                                    ObjCon.Execute(sQry, oledbCommand);
                                    objComm.DumpSms(objLogin.sMobileNo, sSMSText, objComm.sSMSTemplateID);
                                    objLogin.sMessage = "OTP has been sent to your registered Mobile Number";
                                }
                            }
                        }
                        //  string sSMSText = String.Format(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoForgotPwd"]), objLogin.sFullName, Genaral.Decrypt(objLogin.sPassword));
                        //objComm.sendSMS(sSMSText, objLogin.sMobileNo, objLogin.sFullName);
                        // objComm.DumpSms(objLogin.sMobileNo, sSMSText);
                        //objLogin.sMessage = "OTP has been sent to your number";
                    }
                    else
                    {
                        objLogin.sMessage = "Enter Valid MobileNo";
                        objLogin.sResult = "-1";
                    }
                }
                else
                {
                    if (r1.IsMatch(objLogin.sEmail))
                    {
                        objLogin.sMessage = "Enter Valid MobileNo";
                        objLogin.sResult = "-1";
                    }
                    else
                    {
                        oledbCommand = new OleDbCommand();
                        sQry = "SELECT US_FULL_NAME,US_LG_NAME,US_PWD,US_ID,US_MOBILE_NO FROM TBLUSER where US_EMAIL=:MobileNO";
                        oledbCommand.Parameters.AddWithValue("MobileNO", objLogin.sEmail);
                        dt = ObjCon.getDataTable(sQry, oledbCommand);
                        if (dt.Rows.Count > 0)
                        {
                            objLogin.sPassword = dt.Rows[0]["US_PWD"].ToString();
                            objLogin.sFullName = dt.Rows[0]["US_FULL_NAME"].ToString();
                            objLogin.sLoginName = dt.Rows[0]["US_LG_NAME"].ToString();
                            objLogin.sUserId = dt.Rows[0]["US_ID"].ToString();
                            objLogin.sMobileNo = dt.Rows[0]["US_MOBILE_NO"].ToString();

                            SendMailForgotPwd(objLogin);

                            //Arr = SendMailForgotPwd(objLogin);
                            //objLogin.sResult = Arr[0];
                            //objLogin.sMessage = Arr[1];
                        }
                        else
                        {
                            objLogin.sMessage = "Enter Valid Email Id";
                            objLogin.sResult = "-1";
                        }
                    }
                }
                return objLogin;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return objLogin;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objLogin"></param>
        public string[] SendMailForgotPwd(clsLogin objLogin)
        {
            string[] SentResult = new string[2];
            try
            {
                string strMailMsg = string.Empty;
                string strmailFormat = string.Empty;
                clsCommunication objComm = new clsCommunication();
                using (StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailFormats/CreateUser.txt")))
                {
                    String line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        strMailMsg += line;
                    }
                }
                strmailFormat = String.Format(strMailMsg, objLogin.sFullName, objLogin.sLoginName, Genaral.Decrypt(objLogin.sPassword));
                SentResult = objComm.SendMail("DTLMS – Forgot Password", objLogin.sEmail, strmailFormat, objLogin.sUserId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return SentResult;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetPasswordDetails(string userId)
        {
            string query = string.Empty;
            oledbCommand = new OleDbCommand();
            try
            {
                query = "SELECT TRUNC(SYSDATE - US_CHPWD_ON) DAYS FROM TBLUSER WHERE US_ID = :UserId";
                oledbCommand.Parameters.AddWithValue("UserId", userId);
                return ObjCon.get_value(query, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public clsLogin CommonUserLogin(clsLogin objLogin)
        {
            try
            {
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                bool wrongPwd = false;
                oledbCommand = new OleDbCommand();
                sQry = "SELECT US_ID,US_FULL_NAME,US_OFFICE_CODE,US_ROLE_ID,TO_CHAR(US_EFFECT_FROM,'DD/MM/YYYY') US_EFFECT_FROM,US_STATUS, US_PWD ,  ";
                sQry += " US_CHPWD_ON,US_DESIGNATION_NAME";
                sQry += " FROM TBLUSER ";
                sQry += " WHERE US_ID=:UserId";
                oledbCommand.Parameters.AddWithValue("UserId", objLogin.sUserId);
                dt = ObjCon.getDataTable(sQry, oledbCommand);
                //  Check for EffectFrom Condition
                string sEffectFrom = Convert.ToString(dt.Rows[0]["US_EFFECT_FROM"]);
                string sStatus = Convert.ToString(dt.Rows[0]["US_STATUS"]);
                if ((sStatus == "A") && (!wrongPwd))
                {
                    objLogin.sUserId = dt.Rows[0]["US_ID"].ToString();
                    objLogin.sFullName = dt.Rows[0]["US_FULL_NAME"].ToString();
                    objLogin.sOfficeCode = dt.Rows[0]["US_OFFICE_CODE"].ToString();
                    objLogin.sRoleId = dt.Rows[0]["US_ROLE_ID"].ToString();
                    objLogin.sOfficeName = Getofficename(dt.Rows[0]["US_OFFICE_CODE"].ToString());
                    objLogin.sDesignation = dt.Rows[0]["US_DESIGNATION_NAME"].ToString();
                    objLogin.sChangePwd = dt.Rows[0]["US_CHPWD_ON"].ToString();
                    objLogin.sOfficeNamewithType = GetofficeNameWithType(objLogin.sOfficeCode);
                    oledbCommand = new OleDbCommand();
                    sQry = "UPDATE TBLUSERLOGINATTEMPT SET ULA_STATUS = 1  WHERE ULA_USER_ID = :UserId ";
                    oledbCommand.Parameters.AddWithValue("UserId", objLogin.sUserId);
                    ObjCon.Execute(sQry, oledbCommand);
                    oledbCommand = new OleDbCommand();
                    sQry = "";
                }
                else if (wrongPwd)
                {
                    objLogin.sMessage = "Please enter Valid User Name or Password ";
                    UpdateUserLoginAttempts(objLogin);
                    return objLogin;
                }
                else
                {
                    objLogin.sMessage = "User is Disabled,Please contact Administrator";
                }
                return objLogin;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                objLogin.sMessage = "An exception occurred while processing your request.";
                return objLogin;
            }
        }
        /// <summary>
        /// this method will check the db with the user name and get Email and mobial no.
        /// and compares the provided email/mobial no with existing email or mobial no.
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public clsLogin CheckEmailOrMobialNoDetails(clsLogin objLogin)
        {
            oledbCommand = new OleDbCommand();
            string Email = string.Empty;
            string Mobile = string.Empty;
            string QryChck = string.Empty;
            int ComparedValu;
            DataTable DtCheck = new DataTable();
            try
            {
                QryChck = " SELECT US_EMAIL,US_MOBILE_NO,US_LG_NAME from TBLUSER ";
                QryChck += " WHERE US_LG_NAME =:UserName ||'' ";
                oledbCommand.Parameters.AddWithValue("UserName", objLogin.sLoginName.ToUpper());
                DtCheck = ObjCon.getDataTable(QryChck, oledbCommand);

                if (DtCheck.Rows.Count > 0)
                {
                    Email = Convert.ToString(DtCheck.Rows[0]["US_EMAIL"]);
                    Mobile = Convert.ToString(DtCheck.Rows[0]["US_MOBILE_NO"]);
                }
                else
                {
                    objLogin.VLDEmail_Or_Mob = false;
                    objLogin.sMessage = "Please enter valid user name.";
                    return objLogin;
                }

                if (objLogin.sEmail.Contains('@')) // compares email
                {
                    ComparedValu = string.Compare(Email, objLogin.sEmail);
                    // 0 means the compared string are same.
                    if (ComparedValu == 0)
                    {
                        objLogin.VLDEmail_Or_Mob = true;
                    }
                    else
                    {
                        objLogin.VLDEmail_Or_Mob = false;
                        objLogin.sMessage = "Entered Email ID is not registered.";
                    }
                }
                else if (Regex.IsMatch(objLogin.sEmail, @"^\d+$"))
                {
                    if (objLogin.sEmail.Length == 10)
                    {
                        ComparedValu = string.Compare(Mobile, objLogin.sEmail);
                        // 0 means the compared string are same.
                        if (ComparedValu == 0)
                        {
                            objLogin.VLDEmail_Or_Mob = true;

                        }
                        else
                        {
                            objLogin.VLDEmail_Or_Mob = false;
                            objLogin.sMessage = "Entered Mobile No is not registered.";
                        }
                    }
                    else
                    {
                        objLogin.VLDEmail_Or_Mob = false;
                        objLogin.sMessage = "Enter vaild Mobile No.";
                    }
                }
                else
                {
                    objLogin.VLDEmail_Or_Mob = false;
                    objLogin.sMessage = "Enter vaild registered Email ID / Mobile No.";
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw ex;
            }
            return objLogin;
        }
    }
}

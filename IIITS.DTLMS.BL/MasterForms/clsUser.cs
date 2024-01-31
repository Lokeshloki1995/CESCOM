using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace IIITS.DTLMS.BL
{
    public class clsUser
    {
        string strFormCode = "clsUser";
        public string lSlNo { get; set; }
        public string sOfficeCode { get; set; }
        public string sFullName { get; set; }
        public string sLoginName { get; set; }
        public string sPassword { get; set; }
        public string sMobileNo { get; set; }
        public string sDesignation { get; set; }
        public string sRole { get; set; }
        public int dsgnt { get; set; }
        public string sEmail { get; set; }
        public string sPhoneNo { get; set; }
        public string sAddress { get; set; }
        public string sUserType { get; set; }
        public string sCrby { get; set; }
        public string sStatus { get; set; }

        public Byte[] sSignImage { get; set; }

        public string sEffectFrom { get; set; }
        public string sReason { get; set; }
        public string sOffCode { get; set; }
        public string sDivision { get; set; }
        public string ssubDivision { get; set; }
        public string sSection { get; set; }
        public string sCircle { get; set; }
        public string sZone { get; set; }
        public string sOTP { get; set; }

        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        OleDbCommand oledbcommand;
        OleDbCommand comd;


        public string[] SaveUpdateUserDetails(clsUser objUser)
        {
            
            string[] Arr = new string[2];
            OleDbDataReader drUser;
            string strQry = string.Empty;
            try
            {

              
                if (objUser.sOfficeCode != "")
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("OffCode", objUser.sOfficeCode);
                    drUser = ObjCon.Fetch("select OFF_CODE from VIEW_ALL_OFFICES where OFF_CODE=:OffCode", oledbcommand);
                    if (!drUser.Read())
                    {
                        drUser.Close();
                        Arr[0] = "Enter Valid Office Code";
                        Arr[1] = "4";
                        return Arr;
                    }
                    drUser.Close();
                }

                if (objUser.lSlNo == "")
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("UsLgName",  objUser.sLoginName.ToUpper() );
                    drUser = ObjCon.Fetch("select * from TBLUSER where  UPPER(US_LG_NAME)=:UsLgName", oledbcommand);
                    if (drUser.Read())
                    {
                        drUser.Close();
                        Arr[0] = "Login Name Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    drUser.Close();

                    string sMaxNo = Convert.ToString(ObjCon.Get_max_no("US_ID", "TBLUSER"));

                    strQry = "INSERT INTO TBLUSER (US_ID,US_FULL_NAME,US_LG_NAME,US_OFFICE_CODE,US_PWD,US_ROLE_ID,US_EMAIL,US_MOBILE_NO,";
                    strQry += " US_PHONE_NO,US_ADDRESS,US_CRON,US_CRBY,US_DESG_ID,US_SIGN_IMAGE) ";
                    strQry += "values (:sMaxNo,:sFullName,:sLoginName,";
                    strQry += ":sOfficeCode,:sPassword,";
                    strQry += ":sRole,:sEmail,";
                    strQry += ":sMobileNo ,:sPhoneNo,:sAddress,SYSDATE,";
                    strQry += ":sCrby ,"+ objUser.sDesignation+",:Photo)";

                    OleDbParameter docPhoto = new OleDbParameter();
                    
                    docPhoto.DbType = DbType.Binary;

                    if (objUser.sSignImage != null)
                    {
                        docPhoto.ParameterName = "Photo";
                        docPhoto.Value = objUser.sSignImage;
                    }
                    if (ObjCon.Con.State == ConnectionState.Closed) {
                        ObjCon.Con.Open();
                    }
                      comd = new OleDbCommand(strQry, ObjCon.Con);
                   // comd = new OleDbCommand();
                    comd.Parameters.AddWithValue("sMaxNo", sMaxNo);
                    comd.Parameters.AddWithValue("sFullName", objUser.sFullName.ToUpper());
                    comd.Parameters.AddWithValue("sLoginName", objUser.sLoginName.ToUpper());
                    comd.Parameters.AddWithValue("sOfficeCode", objUser.sOfficeCode);
                    comd.Parameters.AddWithValue("sPassword", Genaral.EncryptPassword(objUser.sPassword));
                    comd.Parameters.AddWithValue("sRole", objUser.sRole);
                    comd.Parameters.AddWithValue("sEmail", objUser.sEmail);
                    comd.Parameters.AddWithValue("sMobileNo", objUser.sMobileNo);
                    comd.Parameters.AddWithValue("sPhoneNo", objUser.sPhoneNo);
                    comd.Parameters.AddWithValue("sAddress", objUser.sAddress);
                  //  comd.Parameters.AddWithValue("sUserDesignation", objUser.sDesignation);
                    comd.Parameters.AddWithValue("lSlNo", objUser.lSlNo);
                    comd.Parameters.AddWithValue("sCrby", objUser.sCrby);
                      comd.ExecuteNonQuery();
                    //ObjCon.Execute(strQry, comd);
                   ObjCon.Con.Close();
                     
                    oledbcommand = new OleDbCommand();
                    DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();
                    oledbcommand.Parameters.AddWithValue("UsId", sMaxNo);
                    strQry = "SELECT * FROM TBLUSER WHERE US_ID=:UsId";
                    DataTable dt = new DataTable("UserDetails");
                    dt = ObjCon.getDataTable(strQry, oledbcommand);
                    bool IsSuccess = objWcf.SaveUserDetails(dt);

                    // To send the Mail for successfull creation
                    SendMailUserSuccCreate(objUser);
                    SendSMSUserSuccCreate(objUser);

                    Arr[0] = sMaxNo;
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("uslgname1", objUser.sLoginName.ToUpper());
                    oledbcommand.Parameters.AddWithValue("usid1", objUser.lSlNo);
                    drUser = ObjCon.Fetch("select * from TBLUSER where  UPPER(US_LG_NAME)= :uslgname1 and US_ID<> :usid1", oledbcommand);
                    if (drUser.Read())
                    {
                        drUser.Close();
                        Arr[0] = "Login Name Already Exists";
                        Arr[1] = "4";
                        return Arr;
                    }
                    drUser.Close();

                    #region updating user by modifying  oledbcommand.
                     oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("sFullName", objUser.sFullName.ToUpper());
                    oledbcommand.Parameters.AddWithValue("sLoginName", objUser.sLoginName.ToUpper());
                    oledbcommand.Parameters.AddWithValue("sOfficeCode", objUser.sOfficeCode);
                    if ((objUser.sPassword != null) && (objUser.sPassword != ""))
                    {
                          oledbcommand.Parameters.AddWithValue("sPassword", Genaral.EncryptPassword(objUser.sPassword));
                    }
                    oledbcommand.Parameters.AddWithValue("sRole", objUser.sRole);
                    oledbcommand.Parameters.AddWithValue("sEmail", objUser.sEmail);
                    oledbcommand.Parameters.AddWithValue("sMobileNo", objUser.sMobileNo);
                    oledbcommand.Parameters.AddWithValue("sPhoneNo", objUser.sPhoneNo);
                    oledbcommand.Parameters.AddWithValue("sAddress", objUser.sAddress);
                     oledbcommand.Parameters.AddWithValue("sDesignation", objUser.sDesignation);
                    oledbcommand.Parameters.AddWithValue("lSlNo", objUser.lSlNo);

                    strQry = "UPDATE TBLUSER SET US_FULL_NAME=:sFullName,US_LG_NAME=:sLoginName,US_OFFICE_CODE=:sOfficeCode,";
                    if ((objUser.sPassword != null) && (objUser.sPassword != ""))
                    {
                        strQry += " US_PWD =:sPassword,";
                    }

                    strQry += "US_ROLE_ID=:sRole,US_EMAIL=:sEmail,US_MOBILE_NO=:sMobileNo ,US_PHONE_NO=:sPhoneNo ,";
                    strQry += "US_ADDRESS=:sAddress,US_DESG_ID=:sDesignation";
                    strQry += " , US_UPDATED_ON = SYSDATE , US_UPDATED_BY =  '"+objUser.sCrby+"' WHERE US_ID =:lSlNo";
                    ObjCon.Execute(strQry, oledbcommand);
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;

                    #endregion

                    // commented as the update was not working  . 
                  //  strQry = "UPDATE TBLUSER SET US_FULL_NAME=:sFullName,US_LG_NAME=:sLoginName,US_OFFICE_CODE=:sOfficeCode,US_PWD=:sPassword,";
                  //  strQry += "US_ROLE_ID=:sRole,US_EMAIL=:sEmail,US_MOBILE_NO=:sMobileNo ,US_PHONE_NO=:sPhoneNo ,";
                  //  strQry += "US_ADDRESS=:sAddress,US_DESG_ID="+objUser.sDesignation+"";

                  //  if (objUser.sSignImage != null)
                  //  {
                  //      strQry += " ,US_SIGN_IMAGE=:Photo ";
                  //  }
                  //  strQry += " WHERE US_ID =:lSlNo";
                
                   
                  


                  //  OleDbParameter docPhoto = new OleDbParameter();
                  //  OleDbCommand comd = new OleDbCommand();

                  //  if (objUser.sSignImage != null)
                  //  {
                  //      docPhoto.DbType = DbType.Binary;
                  //      docPhoto.ParameterName = "Photo";
                  //      docPhoto.Value = objUser.sSignImage;
                  //  }

                   
                  //  if (objUser.sSignImage != null)
                  //  {
                  //      comd.Parameters.Add(docPhoto);
                  //  }
                  //  comd.Parameters.AddWithValue("sFullName", objUser.sFullName.ToUpper());
                  //  comd.Parameters.AddWithValue("sLoginName", objUser.sLoginName.ToUpper());
                  //  comd.Parameters.AddWithValue("sOfficeCode", objUser.sOfficeCode);
                  //  comd.Parameters.AddWithValue("sPassword", Genaral.Encrypt(objUser.sPassword));
                  //  comd.Parameters.AddWithValue("sRole", objUser.sRole);
                  //  comd.Parameters.AddWithValue("sEmail", objUser.sEmail);
                  //  comd.Parameters.AddWithValue("sMobileNo", objUser.sMobileNo);
                  //  comd.Parameters.AddWithValue("sPhoneNo", objUser.sPhoneNo);
                  //  comd.Parameters.AddWithValue("sAddress", objUser.sAddress);
                  // // comd.Parameters.AddWithValue("sDesignation", objUser.sDesignation);
                  //  comd.Parameters.AddWithValue("lSlNo", objUser.lSlNo);
                  //   comd = new OleDbCommand(strQry, ObjCon.Con);
                  //  comd.ExecuteNonQuery();
                  ////  ObjCon.Execute(strQry,comd);
                  //  Arr[0] = "Updated Successfully";
                  //  Arr[1] = "1";
                  //  return Arr;
                }
            }
            catch (Exception ex)
            {

                ObjCon.Con.Close();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveFunction");
                Arr[0] = "Oops Something Went Wrong ....Please Contact our Support Team";
                Arr[1] = "4";
                return Arr;
            }
            finally
            {
                
            }

        }

        public DataTable LoadUserGrid(clsUser objuser)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtUserDetails = new DataTable();
            
            string strQry = string.Empty;
            try
            {
                
                //strQry = " SELECT * FROM (SELECT US_ID,US_FULL_NAME,(SELECT UPPER(DM_NAME) FROM TBLDESIGNMAST";
                //strQry += " WHERE DM_DESGN_ID= US_DESG_ID) AS US_DESG_ID, US_EMAIL,US_MOBILE_NO,(SELECT RO_NAME FROM TBLROLES WHERE RO_ID=US_ROLE_ID) RO_NAME, ";
                //strQry += "(SELECT OFF_NAME  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=US_OFFICE_CODE) OFF_NAME,  US_STATUS,CASE  WHEN TO_CHAR(US_EFFECT_FROM,'YYYYMMDD') > TO_CHAR(SYSDATE,'YYYYMMDD') ";
                //strQry += " AND US_STATUS='D' THEN 'A'  ELSE US_STATUS END  US_STATUS1 FROM TBLUSER ORDER BY US_ID DESC)";

                strQry = " SELECT * FROM (SELECT US_ID,US_FULL_NAME,(SELECT UPPER(DM_NAME) FROM TBLDESIGNMAST";
                strQry += " WHERE DM_DESGN_ID= US_DESG_ID) AS US_DESG_ID, US_EMAIL,US_MOBILE_NO,(SELECT RO_NAME FROM TBLROLES WHERE RO_ID=US_ROLE_ID) RO_NAME, ";
                strQry += "(SELECT OFF_NAME  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=US_OFFICE_CODE) OFF_NAME,  US_STATUS,CASE  WHEN TO_CHAR(US_EFFECT_FROM,'YYYYMMDD') > TO_CHAR(SYSDATE,'YYYYMMDD') ";
                strQry += " AND US_STATUS='D' THEN 'A'  ELSE US_STATUS END  US_STATUS1 FROM TBLUSER";

                if (objuser.sOffCode == null || objuser.sOffCode == "--Select--" )
                {
                    strQry += "  ORDER BY US_ID DESC)";
                }
                else if(objuser.sOffCode.Length == 0)
                {
                    if (objuser.dsgnt == 1)
                    {
                        strQry += " where US_OFFICE_CODE IS NULL ORDER BY US_ID DESC) ";
                    }
                    else if (objuser.dsgnt == 2)
                    {
                        strQry += "  where US_OFFICE_CODE IS NOT NULL ORDER BY US_ID DESC) ";
                    }
                }
                else
                {
                    oledbcommand.Parameters.AddWithValue("offcode", objuser.sOffCode);
                    strQry += " where US_OFFICE_CODE like :offcode||'%' ORDER BY US_ID DESC)";
                }



                dtUserDetails = ObjCon.getDataTable(strQry, oledbcommand);
                
                return dtUserDetails;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadUserGrid");
                return dtUserDetails;

            }
            finally
            {
                
            }
        }


        public object GetUserDetails(clsUser objuser)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtUserDetails = new DataTable();
            try
            {
                
                
                string strQry = string.Empty;
                //strQry = "SELECT US_ID,US_FULL_NAME,US_LG_NAME,US_OFFICE_CODE,US_PWD,US_EMAIL,US_MOBILE_NO,US_PHONE_NO,US_ROLE_ID,";
                //strQry += " US_ADDRESS,US_USER_TYPE,US_DESG_ID FROM TBLUSER WHERE US_ID='" + objuser.lSlNo + "' ";

              
                strQry = "SELECT US_ID,US_FULL_NAME,US_LG_NAME,US_OFFICE_CODE, ";
                strQry += "(SELECT  ZO_NAME FROM TBLZONE)ZONE,";
                strQry += "(SELECT CM_CIRCLE_NAME  FROM TBLCIRCLE WHERE CM_CIRCLE_CODE = SUBSTR(US_OFFICE_CODE, 0, 1))CIRCLE,";
                strQry += "(SELECT DIV_NAME  FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(US_OFFICE_CODE, 0, 2)) DIVISION ,";
                strQry += "(SELECT SD_SUBDIV_NAME  FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE = SUBSTR(US_OFFICE_CODE, 0, 3)) SUBDIVISION,";
                strQry += "(SELECT OM_NAME   FROM TBLOMSECMAST WHERE OM_CODE  = SUBSTR(US_OFFICE_CODE, 0, 4)) SECTION ,";
                strQry += "(SELECT RO_ID  FROM TBLROLES WHERE RO_ID = US_ROLE_ID ) RoleName,";
                strQry += "US_PWD,US_EMAIL,US_MOBILE_NO,US_PHONE_NO, US_ADDRESS,US_USER_TYPE,US_DESG_ID FROM TBLUSER WHERE US_ID= " + objuser.lSlNo + "";

               // oledbcommand.Parameters.AddWithValue("uid", objuser.lSlNo);
                dtUserDetails = ObjCon.getDataTable(strQry);

                
                if (dtUserDetails.Rows.Count > 0)
                {
                    objuser.lSlNo = Convert.ToString(dtUserDetails.Rows[0]["US_ID"]);
                    objuser.sOfficeCode = Convert.ToString(dtUserDetails.Rows[0]["US_OFFICE_CODE"]);
                    objuser.sFullName = Convert.ToString(dtUserDetails.Rows[0]["US_FULL_NAME"]);
                    objuser.sLoginName = Convert.ToString(dtUserDetails.Rows[0]["US_LG_NAME"]);
                    objuser.sPassword = Convert.ToString(dtUserDetails.Rows[0]["US_PWD"]);
                    objuser.sRole = Convert.ToString(dtUserDetails.Rows[0]["RoleName"]);
                    objuser.sEmail = Convert.ToString(dtUserDetails.Rows[0]["US_EMAIL"]);
                    objuser.sMobileNo = Convert.ToString(dtUserDetails.Rows[0]["US_MOBILE_NO"]);
                    objuser.sPhoneNo = Convert.ToString(dtUserDetails.Rows[0]["US_PHONE_NO"]);
                    objuser.sAddress = Convert.ToString(dtUserDetails.Rows[0]["US_ADDRESS"]);
                    objuser.sUserType = Convert.ToString(dtUserDetails.Rows[0]["US_USER_TYPE"]);
                    objuser.sDesignation = Convert.ToString(dtUserDetails.Rows[0]["US_DESG_ID"]);
                    objuser.sZone = Convert.ToString(dtUserDetails.Rows[0]["ZONE"]);
                    objuser.sCircle = Convert.ToString(dtUserDetails.Rows[0]["CIRCLE"]);
                    objuser.sDivision = Convert.ToString(dtUserDetails.Rows[0]["DIVISION"]);
                    objuser.ssubDivision = Convert.ToString(dtUserDetails.Rows[0]["SUBDIVISION"]);
                    objuser.sSection = Convert.ToString(dtUserDetails.Rows[0]["SECTION"]);

                }
                return objuser;
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetUserDetails");
                return objuser;
            }
            finally
            {
                
            }
        }

        public void SendMailUserSuccCreate(clsUser objUser)
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
            strmailFormat = String.Format(strMailMsg, objUser.sFullName, objUser.sLoginName, objUser.sPassword);
            objComm.SendMail("DTLMS – User Created Successfully", objUser.sEmail, strmailFormat, objUser.sCrby);
        }

        public void SendSMSUserSuccCreate(clsUser objUser)
        {
            string strSms = string.Empty;
            clsCommunication objComm = new clsCommunication();

            objComm.sSMSkey = "SMStoUserSuccCreat";
            objComm = objComm.GetsmsTempalte(objComm);
            // string strSms = String.Format(objComm.sSMSTemplate,
            // strSms = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SMStoUserSuccCreat"]);
            strSms = String.Format(objComm.sSMSTemplate, objUser.sFullName, objUser.sLoginName, objUser.sPassword);
            //objComm.sendSMS(strSms, objUser.sMobileNo, objUser.sFullName);
            objComm.DumpSms(sMobileNo, strSms, objComm.sSMSTemplateID);
        }



        public bool ActiveDeactiveUser(clsUser objUser)
        {
            try
            {

                string strQry = string.Empty;

                strQry = "UPDATE TBLUSER SET US_STATUS= :sStatus,";  //'" + objUser.sStatus + "'
                strQry += " US_EFFECT_FROM = TO_DATE(:sEffectFrom,'dd/MM/yyyy'),US_REASON= :sReason"; //'" + objUser.sEffectFrom + "'  //'" + objUser.sReason + "'
                strQry += " WHERE US_ID= :lSlNo"; //'" + objUser.lSlNo + "'
                OleDbCommand cmd = new OleDbCommand();
                cmd.Parameters.AddWithValue("sStatus", objUser.sStatus);
                cmd.Parameters.AddWithValue("sEffectFrom", objUser.sEffectFrom);
                cmd.Parameters.AddWithValue("sReason", objUser.sReason);
                cmd.Parameters.AddWithValue("lSlNo", objUser.lSlNo);
                ObjCon.Execute(strQry, cmd);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ActiveDeactiveUser");
                return false;

            }
            finally
            {

            }
        }

        public string[] UpdatePwd(clsUser objUser)
        {
            oledbcommand = new OleDbCommand();
            string[] Arr = new string[2];
            try
            {
                string strQry = string.Empty;
                oledbcommand.Parameters.AddWithValue("Password", objUser.sPassword);
                oledbcommand.Parameters.AddWithValue("lSlNo", objUser.lSlNo);

                strQry = "UPDATE \"TBLUSER\" set \"US_PWD\"=:Password , US_CHPWD_ON = SYSDATE WHERE \"US_ID\"=:lSlNo";
                ObjCon.Execute(strQry, oledbcommand);

                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("OTP", objUser.sOTP);
                oledbcommand.Parameters.AddWithValue("lSlNo1", objUser.lSlNo);

                strQry = "UPDATE tblotp set otp_no=:OTP,otp_change_pwd_on=SYSDATE,otp_sent_flag='1' WHERE otp_us_id=:lSlNo1 and otp_sent_flag='0'";
                //strQry = "INSERT into tblotp (otp_us_id,otp_no,otp_cron,otp_sent_flag) values ('"+ objUser.lSlNo + "','"+ objUser.sOTP + "',now(),'1')";
                ObjCon.Execute(strQry, oledbcommand);

                Arr[0] = "1";
                Arr[1] = "Password Changed Succesfully";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                Arr[0] = "0";
                Arr[1] = "Something went wrong";
                return Arr;
            }
        }

        public string getMMSUserId(string Uid)
        {
            oledbcommand = new OleDbCommand();
            string MMSUid = string.Empty;
            try
            {
                
                string strQry = string.Empty;

               // oledbcommand.Parameters.AddWithValue("uid", Uid);
                strQry = "SELECT US_MMS_ID FROM TBLUSER WHERE US_ID = " + Uid + "";
                MMSUid = Genaral.EncryptMMS(ObjCon.get_value(strQry));
                return MMSUid;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getMMSUserId");
                return MMSUid;
            }
            finally
            {
                
            }

        }
        public string getVersion()
        {
            string strQry = string.Empty;
            string Latestversion = string.Empty;

            oledbcommand = new OleDbCommand();
            try
            {
                strQry = "select V_VERSION from TBLVERSIONS where V_STATUS='1'";
                return Latestversion = ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Latestversion;
            }
        }

    }
}



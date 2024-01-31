using System;
using System.Collections.Generic;
using System.Text;
using IIITS.DAL;
using System.Configuration;
using System.Data;
using System.IO;

namespace IIITS.DTLMS.BL
{
    public static class clsException
    {
        /// <summary>
        /// Log Error 
        /// </summary>
        /// <param name="sStackTrace"></param>
        /// <param name="sErrorMsg"></param>
        /// <param name="sFormName"></param>
        /// <param name="sFunctionName"></param>
        /// <param name="exceptionQuery"></param>
        public static void LogError(string sStackTrace, string sErrorMsg, string sFormName, string sFunctionName, string exceptionQuery = "")
        {
            try
            {
                string strQry = string.Empty;
                if (sErrorMsg.StartsWith("Object"))
                {
                    return;
                }
                if (Convert.ToString(ConfigurationManager.AppSettings["LOGTOFILE"]) == "ON")
                {
                    if (!sErrorMsg.Contains("Thread was being aborted.") && !sErrorMsg.Contains("TNS:Connect timeout occurred"))
                    {
                        WriteLogFile(sFormName + "$" + sErrorMsg, sFunctionName, sStackTrace);
                    }
                }
                if (sErrorMsg.Trim().Length > 4000)
                {
                    sErrorMsg = sErrorMsg.Remove(4000);
                }
                if (sStackTrace.Trim().Length > 4000)
                {
                    sStackTrace = sStackTrace.Remove(4000);
                }
                //long iMaxId = ObjCon.Get_max_no("ERR_ID", "TBLERRORLOG");
                if (Convert.ToString(ConfigurationManager.AppSettings["ErrorMsg"]) == "ON")
                {
                    CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
                    if (!sErrorMsg.Contains("Thread was being aborted.") && !sErrorMsg.Contains("TNS:Connect timeout occurred") && !sErrorMsg.Contains("unique"))
                    {
                        strQry = "Insert into TBLERRORLOG (ERR_ID,ERR_PAGE_NAME,ERR_FUNCTION,ERR_ERROR,ERR_STACK_TRACE,ERR_QUERY_LOG) values (";
                        strQry += " '" + ObjCon.Get_max_no("ERR_ID", "TBLERRORLOG") + "','" + sFormName + "','" + sFunctionName + "','" + sErrorMsg.Replace("'", "''") + "','" + sStackTrace.Replace("'", "''") + "', '" + exceptionQuery.Replace("'", "''") + "')";
                        ObjCon.Execute(strQry);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogFile(ex.Message, sFunctionName, "", "DB");
            }
        }
        /// <summary>
        /// Write Log File
        /// </summary>
        /// <param name="sClassName"></param>
        /// <param name="sMethodName"></param>
        /// <param name="sValue"></param>
        /// <param name="sFileName"></param>
        public static void WriteLogFile(string sClassName, string sMethodName, string sValue, string sFileName = "")
        {
            try
            {
                sValue = sValue.Replace(',', '-');
                string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
                if (!Directory.Exists(sFolderPath))
                {
                    Directory.CreateDirectory(sFolderPath);
                }
                int NoofTimes = 0;
                LOOP:
                try
                {
                    string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
                    File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , Method Name --" + sMethodName + " , ClassName --" + sClassName + " , ErrorMessage--" + sValue + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(250);
                    NoofTimes++;
                    if (NoofTimes <= 3)
                    {
                        goto LOOP;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        /// <summary>
        /// Application Error Log
        /// </summary>
        /// <param name="locationCode"></param>
        /// <param name="MyClassName"></param>
        /// <param name="MyFunctionName"></param>
        /// <param name="Message"></param>
        /// <param name="sQuery"></param>
        /// <param name="FileName"></param>
        public static void ApplicationErrorLog(string locationCode, string MyClassName, string MyFunctionName, string Message, string sQuery = "", string FileName = "")
        {
            try
            {
                string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
                if (!Directory.Exists(sFolderPath))
                {
                    Directory.CreateDirectory(sFolderPath);
                }
                string sPath = sFolderPath + "//" + FileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
                // Calculate GMT offset
                int GmtOffset = DateTime.Compare(DateTime.Now, DateTime.UtcNow);
                string GmtPrefix = null;
                if (GmtOffset > 0)
                {
                    GmtPrefix = "+";
                }
                else
                {
                    GmtPrefix = "";
                }
                // Create DateTime string
                string ErrorDateTime = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + " @ " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " (GMT " + GmtPrefix + GmtOffset.ToString() + ")";
                StreamWriter MsStreamWriter = new StreamWriter(sPath, true);
                MsStreamWriter.WriteLine(Environment.NewLine);
                MsStreamWriter.WriteLine("Date And Time - " + ErrorDateTime);
                MsStreamWriter.WriteLine("Location Code - " + locationCode);
                MsStreamWriter.WriteLine("Class Name - " + MyClassName);
                MsStreamWriter.WriteLine("Function Name - " + MyFunctionName);
                MsStreamWriter.WriteLine("SQL Query - " + sQuery);
                MsStreamWriter.WriteLine("Error Message - " + Message);
                MsStreamWriter.WriteLine("##################################################################");
                MsStreamWriter.Close();
            }
            catch (Exception ex)
            {
                return;
            }
        }
        /// <summary>
        /// Intigration Log Error
        /// </summary>
        /// <param name="sStackTrace"></param>
        /// <param name="sErrorMsg"></param>
        /// <param name="sFormName"></param>
        /// <param name="sFunctionName"></param>
        /// <param name="suserid"></param>
        public static void Intigration_LogError(string sStackTrace, string sErrorMsg, string sFormName, string sFunctionName, string suserid)
        {
            try
            {
                string strQry = string.Empty;
                if (sErrorMsg.StartsWith("Object"))
                {
                    return;
                }
                if (sErrorMsg.Trim().Length > 1500)
                {
                    sErrorMsg = sErrorMsg.Remove(1500);
                }
                if (sStackTrace.Trim().Length > 1500)
                {
                    sStackTrace = sStackTrace.Remove(1450);
                }
                if (Convert.ToString(ConfigurationManager.AppSettings["LOGTOFILE"]) == "ON")
                {
                    WriteLogFile(sFormName, sFunctionName, sStackTrace);
                }
                //long iMaxId = ObjCon.Get_max_no("ERR_ID", "TBLERRORLOG");
                if (Convert.ToString(ConfigurationManager.AppSettings["ErrorMsg"]) == "ON")
                {
                    CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
                    strQry = "Insert into TBLERRORLOG (ERR_ID,ERR_PAGE_NAME,ERR_FUNCTION,ERR_ERROR,ERR_STACK_TRACE) values (";
                    strQry += "'" + ObjCon.Get_max_no("ERR_ID", "TBLERRORLOG") + "','" + sFormName + "','" + sFunctionName + "','" + sErrorMsg.Replace("'", "''") + "','" + sStackTrace.Replace("'", "''") + "')";
                    ObjCon.Execute(strQry);
                }
            }
            catch (Exception ex)
            {
                WriteLogFile(ex.Message, sFunctionName, "", "DB");
            }
        }
        /// <summary>
        /// Error Msg
        /// </summary>
        /// <returns></returns>
        public static string ErrorMsg()
        {
            string strError = string.Empty;
            if (Convert.ToString(ConfigurationManager.AppSettings["ErrorMsg"]) == "ON")
            {
                strError = "An exception occurred while processing your request.";
            }
            return strError;
        }

        public static void SaveFunctionExecLog(string sFunctionName)
        {
            //CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

            //string strQry = string.Empty;
            //strQry = "INSERT INTO TBLPERFORMANCETRACK (PT_ID,PT_FUNCTION,PT_LOG_DATE) VALUES (";
            //strQry += " '" + ObjCon.Get_max_no("PT_ID", "TBLPERFORMANCETRACK") + "','"+ sFunctionName +"',SYSDATE)";
            //ObjCon.Execute(strQry);
        }
    }
}

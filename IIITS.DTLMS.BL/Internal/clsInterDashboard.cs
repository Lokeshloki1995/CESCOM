using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
    public class clsInterDashboard
    {
        string strFormCode = "clsInterDashboard";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);

        //public string sUserId { get; set; }
        public string sFromDate { get; set; }
        public string sToDate { get; set; }
        public bool bCurrentDate { get; set; }
        public string sOfficeCode { get; set; }
        public string sFeederCode { get; set; }

        public string GetToatlEnumerationCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_ID IS NOT NULL AND ED_STATUS_FLAG<>'5'";
                if (sUserId != "")
                {
                    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "')";
                }

                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate!="")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '"+ sOfficeCode +"%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
               
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetToatlEnumerationCount");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetQCPendingEnumCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG='0' ";
                if (sUserId != "")
                {
                    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "') ";

                }
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetToatlEnumerationCount");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetQCDoneEnumCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
             string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG='1' ";
                if (sUserId != "")
                {
                    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "') ";

                }
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetToatlEnumerationCount");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetRejectEnumCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG='3' ";
                if (sUserId != "")
                {
                    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "') ";

                }
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetRejectEnumCount");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetPendingForClarificationEnumCount(string sUserId = "", bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT  NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG='2' ";
                if (sUserId != "")
                {
                    strQry += " AND (ED_OPERATOR1='" + sUserId + "' OR ED_OPERATOR2='" + sUserId + "') ";

                }
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetRejectEnumCount");
                return ex.Message;
            }
        }

        public string GetOperatorCountForSuperVisor(string sUserId)
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT NVL(COUNT(IU_ID),0) FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "' ";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetOperatorCountForSuperVisor");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetTotalEnumCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
             string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='"+ sUserId +"'))";
                strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetOperatorCountForSuperVisor");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetQCDoneCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))";
                strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                strQry += " AND  ED_STATUS_FLAG='1'";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetOperatorCountForSuperVisor");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetQCPendingCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))";
                strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                strQry += " AND  ED_STATUS_FLAG='0'";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetQCPendingCountForSuperVisor");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetQCRejectCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
            string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))";
                strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                strQry += " AND  ED_STATUS_FLAG='3'";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetQCRejectCountForSuperVisor");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetPendingForClarificationCountForSuperVisor(string sUserId, bool bDate = false, string sFromDate = "", string sTodate = "",
             string sOfficeCode = "", string sFeederCode = "")
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT NVL(COUNT(ED_ENUM_TYPE),0) FROM TBLENUMERATIONDETAILS WHERE  ((ED_OPERATOR2 IN ";
                strQry += " (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))";
                strQry += " OR (ED_OPERATOR1 IN (SELECT IU_ID FROM TBLINTERNALUSERS WHERE IU_SUPERVISORID='" + sUserId + "'))) ";
                strQry += " AND  ED_STATUS_FLAG='2'";
                if (bDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')=TO_CHAR(SYSDATE,'DD/MM/YYYY')";
                }
                if (sFromDate != "" && sTodate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(sTodate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                }
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE LIKE '" + sFeederCode + "%'";
                }
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetPendingForClarificationCountForSuperVisor");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public string GetOperatorCount()
        {
            try
            {
                
                string strQry = string.Empty;
                strQry = "SELECT NVL(COUNT(IU_ID),0) FROM TBLINTERNALUSERS ";
                return ObjCon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetOperatorCount");
                return ex.Message;
            }
            finally
            {
                
            }
        }

        public DataTable LoadStatusReportLocationWise(clsInterDashboard objDashBoard)
        {
            DataTable dt = new DataTable();
            try
            {
                
                string strQry = string.Empty;
                string sOffcodeLength = string.Empty;

                if (objDashBoard.sOfficeCode.Length == 0)
                {
                    sOffcodeLength = "2";
                }
                if (objDashBoard.sOfficeCode.Length == 2)
                {
                    sOffcodeLength = "3";
                }
                strQry = "  SELECT (SELECT OFF_NAME FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(ED_OFFICECODE,1," + sOffcodeLength + ")) as LOCATION,";
                strQry += " SUBSTR(ED_OFFICECODE,1," + sOffcodeLength + ") OFFICECODE,'' AS FD_FEEDER_NAME,";
                strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG<>'5' THEN 1 ELSE 0 END ),0) TOTAL,";
                strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG='0' THEN 1 ELSE 0 END ),0) QCPENDING,";
                strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG='1' THEN 1 ELSE 0 END ),0) QCDONE, ";
                strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG='3' THEN 1 ELSE 0 END ),0) QCREJECT,";
                strQry += " NVL(SUM(CASE WHEN ED_STATUS_FLAG='2' THEN 1 ELSE 0 END ),0) PENDING_CLAR,";
                if (sOffcodeLength == "2")
                {
                    strQry += " '' as ENUMTYPE ";
                }
                else
                {
                    strQry += " DECODE(ED_LOCTYPE,'1','STORE','2','FIELD','3','REPAIRER','5','TRANSFORMER BANK') as ENUMTYPE";
                }
                strQry += " FROM TBLENUMERATIONDETAILS WHERE ED_STATUS_FLAG<>'5' AND ED_OFFICECODE<>'8888'  ";
                //AND LENGTH(SUBSTR(ED_OFFICECODE,1," + sOffcodeLength + ")) = " + sOffcodeLength + "
                if (bCurrentDate == true)
                {
                    strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')= TO_CHAR(SYSDATE,'DD/MM/YYYY') ";
                }
                if (sOfficeCode != "")
                {
                    strQry += " AND ED_OFFICECODE LIKE '" + objDashBoard.sOfficeCode + "%'";
                }
                if (objDashBoard.sFromDate != "" && objDashBoard.sToDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(objDashBoard.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    DateTime dToDate = DateTime.ParseExact(objDashBoard.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                strQry += " GROUP BY  SUBSTR(ED_OFFICECODE,1," + sOffcodeLength + ")";

                if (sOffcodeLength != "2")
                {
                    strQry += ",ED_LOCTYPE";
                }
                return ObjCon.getDataTable(strQry);

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStatusReportLocationWise");
                return dt;
            }
            finally
            {
                
            }
        }

        public DataTable LoadStatusReportFeederWise(clsInterDashboard objDashBoard)
        {
            DataTable dt = new DataTable();
            try
            {
                
                string strQry = string.Empty;

                strQry = "SELECT ED_FEEDERCODE || ' - ' || FD_FEEDER_NAME AS FD_FEEDER_NAME,ED_FEEDERCODE,'' AS LOCATION,'' AS OFFICECODE,";
               strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG<>'5' THEN 1 ELSE 0 END ),0) TOTAL,";
               strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG='0' THEN 1 ELSE 0 END ),0) QCPENDING,";
               strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG='1' THEN 1 ELSE 0 END ),0) QCDONE,";
               strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG='3' THEN 1 ELSE 0 END ),0) QCREJECT,";
               strQry+= " NVL(SUM(CASE WHEN ED_STATUS_FLAG='2' THEN 1 ELSE 0 END ),0) PENDING_CLAR, ";
               strQry += " '' AS ENUMTYPE";
               strQry+= " FROM TBLENUMERATIONDETAILS,TBLFEEDERMAST,TBLFEEDEROFFCODE WHERE  ED_STATUS_FLAG<>'5' AND FD_FEEDER_ID=FDO_FEEDER_ID ";
               strQry += " AND ED_FEEDERCODE=FD_FEEDER_CODE AND FDO_OFFICE_CODE LIKE '" + objDashBoard.sOfficeCode + "%' ";
               if (objDashBoard.bCurrentDate == true)
               {
                   strQry += " AND TO_CHAR(ED_CRON,'DD/MM/YYYY')= TO_CHAR(SYSDATE,'DD/MM/YYYY') ";
               }
               if (objDashBoard.sFeederCode != "")
               {
                   strQry += " AND ED_FEEDERCODE LIKE '" + objDashBoard.sFeederCode + "%' ";
               }
               if (objDashBoard.sFromDate != "" && objDashBoard.sToDate != "")
               {
                   DateTime dFromDate = DateTime.ParseExact(objDashBoard.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                   DateTime dToDate = DateTime.ParseExact(objDashBoard.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                   strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
               }
               strQry += " GROUP BY ED_FEEDERCODE,FD_FEEDER_NAME";
               return ObjCon.getDataTable(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStatusReportFeederWise");
                return dt;
            }
            finally
            {
                
            }
        }
    }
}

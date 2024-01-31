using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Globalization;

namespace IIITS.DTLMS.BL
{
    public class clsReports
    {
        public string sFromDate { get; set; }
        public string sTodate { get; set; }
        public string sTempFromDate { get; set; }
        public string sTempTodate { get; set; }
        public string sType { get; set; }
        public string sFailureType { get; set; }
        public string sGreaterVal { get; set; }
        public string sRepriername { get; set; }
        public string sCapacity { get; set; }
        public string sMake { get; set; }
        public string sOfficeCode { get; set; }
        public string sFeeder { get; set; }
        public string sSchemeType { get; set; }
        public string sFeederType { get; set; }
        public string sDtcCode { get; set; }
        public string sDtrCode { get; set; }
        public string sFailId { get; set; }
        public string sMonth { get; set; }
        public string sCurrentMonth { get; set; }
        public string sEmployeeCost { get; set; }
        public string sESI { get; set; }
        public string ServiceTax { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string DecomLabourCost { get; set; }
        public string sReportType { get; set; }
        public string sGuranteeType { get; set; }
        public string sSelectedFailureType { get; set; }
        public List<string> sGuarantyTypes { get; set; }
        public string sOldFeederCode { get; set; }
        public string sNewFeederCode { get; set; }
        public string sFeederBifurcationID { get; set; }
        public string sFinancialYear { get; set; }
        public string sDTCAddedThrough { get; set; }
        public string sQCApprovaltype { get; set; }
        public string sRepSupType { get; set; }
        public string sFromMonth { get; set; }
        public string sToMonth { get; set; }
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        #region Internal Application
        public DataTable EnumerationReport(string strFromdate, string strToDate)
        {

            DataTable dtStoreDetails = new DataTable();
            string strQry = string.Empty;
            try
            {

                DateTime dFromDate = DateTime.ParseExact(strFromdate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                DateTime dToDate = DateTime.ParseExact(strToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);


                strQry = "   SELECT (SELECT IU_FULLNAME from TBLINTERNALUSERS WHERE IU_ID=ED_OPERATOR1)ED_OPERATOR1,";
                strQry += " (SELECT IU_FULLNAME from TBLINTERNALUSERS WHERE IU_ID=ED_OPERATOR2)ED_OPERATOR2 , ";
                strQry += " NVL( SUM( CASE WHEN ED_ENUM_TYPE = 2 THEN 1 ELSE 0 END ),0) FIELD,";
                strQry += " NVL( SUM( CASE WHEN ED_ENUM_TYPE = 1 THEN 1 ELSE 0 END ),0) STORES,";
                strQry += " NVL( SUM( CASE WHEN ED_ENUM_TYPE = 3 THEN 1 ELSE 0 END ),0) REPAIRER, ";
                //strQry += " (select count(TO_CHAR(ED_APPROVED_BY)) from TBLENUMERATIONDETAILS where ED_APPROVED_BY<>1)PENDING_QC,";
                //strQry += " (select count(TO_CHAR(ED_STATUS_FLAG)) from TBLENUMERATIONDETAILS where ED_STATUS_FLAG=1)QC_DONE,";
                strQry += " (NVL( sum(CASE WHEN ED_STATUS_FLAG=0 THEN 1 ELSE 0 END ),0 ))PENDING_QC, ";
                strQry += " (NVL( sum(CASE WHEN ED_STATUS_FLAG=1 THEN 1 ELSE 0 END ),0 ))QC_DONE,";
                strQry += " COUNT(ED_ENUM_TYPE) TOTAL ";
                strQry += " FROM TBLENUMERATIONDETAILS where TO_CHAR(ED_CRON,'yyyyMMdd')";
                strQry += " BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "' GROUP BY ED_OPERATOR1,ED_OPERATOR2";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtStoreDetails.Load(dr);
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtStoreDetails;
            }
        }


        public DataTable PrintDetailedFieldReport(string strFromDate, string strToDate)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                DateTime dFromDate = DateTime.ParseExact(strFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dToDate = DateTime.ParseExact(strToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                strQry = "SELECT TO_CHAR(DTE_DTCCODE)DTE_DTCCODE,TO_CHAR(DTE_TC_CODE)DTE_TC_CODE,TO_CHAR(DTE_CAPACITY)DTE_CAPACITY,DTE_NAME,TM_NAME,(select CM_CIRCLE_NAME from TBLCIRCLE WHERE CM_CIRCLE_CODE=SUBSTR(ED_OFFICECODE,1,1)) AS Circle,";
                strQry += "(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(ED_OFFICECODE,1,2)) as Division,";
                strQry += "(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(ED_OFFICECODE,1,3)) as SubDivision";
                strQry += ",(SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(ED_OFFICECODE,1,4)) as Location   ";
                strQry += " FROM TBLDTCENUMERATION,TBLENUMERATIONDETAILS,TBLTRANSMAKES WHERE DTE_MAKE=TM_ID AND ED_ID=DTE_ED_ID AND  ED_ENUM_TYPE ='2' AND ";
                strQry += " TO_CHAR(DTE_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtDetailedReport.Load(dr);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetailedReport;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFromDate"></param>
        /// <param name="strToDate"></param>
        /// <returns></returns>
        public DataTable PrintDetailedStoreReport(string strFromDate, string strToDate)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                DateTime dFromDate = DateTime.ParseExact(strFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dToDate = DateTime.ParseExact(strToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                strQry = "SELECT TO_CHAR(DTE_TC_CODE)DTE_TC_CODE,TO_CHAR(DTE_CAPACITY)DTE_CAPACITY,";
                strQry += " TO_CHAR(DTE_TC_MANFDATE,'dd-MON-yyyy') DTE_TC_MANFDATE,TM_NAME,(select CM_CIRCLE_NAME from TBLCIRCLE WHERE CM_CIRCLE_CODE=SUBSTR(ED_OFFICECODE,1,1)) AS Circle,";
                strQry += "(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(ED_OFFICECODE,1,2)) as Division";
                strQry += " FROM TBLDTCENUMERATION,TBLENUMERATIONDETAILS,TBLTRANSMAKES WHERE DTE_MAKE=TM_ID AND ED_ID=DTE_ED_ID and  ED_ENUM_TYPE IN (1,3)";
                strQry += "  and TO_CHAR(DTE_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtDetailedReport.Load(dr);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetailedReport;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFromdate"></param>
        /// <param name="strToDate"></param>
        /// <returns></returns>
        public DataTable EnumReportLocationWise(string strFromdate, string strToDate)
        {
            DataTable dtStoreDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                DateTime dFromDate = DateTime.ParseExact(strFromdate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dToDate = DateTime.ParseExact(strToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                strQry = " SELECT (SELECT IU_FULLNAME from TBLINTERNALUSERS WHERE IU_ID=ED_OPERATOR1)ED_OPERATOR1,";
                strQry += " (SELECT IU_FULLNAME from TBLINTERNALUSERS WHERE IU_ID=ED_OPERATOR2)ED_OPERATOR2 , ";
                strQry += " NVL( SUM( CASE WHEN ED_ENUM_TYPE = 2 THEN 1 ELSE 0 END ),0) FIELD,";
                strQry += " NVL( SUM( CASE WHEN ED_ENUM_TYPE = 1 THEN 1 ELSE 0 END ),0) STORES,";
                strQry += "NVL( SUM( CASE WHEN ED_ENUM_TYPE = 3 THEN 1 ELSE 0 END ),0) REPAIRER, ";
                strQry += "  (NVL( sum(CASE WHEN ED_STATUS_FLAG=0 THEN 1 ELSE 0 END ),0 ))PENDING_QC, ";
                strQry += "  (NVL( sum(CASE WHEN ED_STATUS_FLAG=1 THEN 1 ELSE 0 END ),0 ))QC_DONE,  ";
                strQry += " COUNT(ED_ENUM_TYPE) TOTAL,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(ED_OFFICECODE,1,2)) as Division,";
                strQry += " (select CM_CIRCLE_NAME from TBLCIRCLE WHERE CM_CIRCLE_CODE=SUBSTR(ED_OFFICECODE,1,1)) AS Circle,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(ED_OFFICECODE,1,3)) as SubDivision,";
                strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(ED_OFFICECODE,1,4)) as Location";
                strQry += " FROM TBLENUMERATIONDETAILS where TO_CHAR(ED_CRON,'yyyyMMdd')";
                strQry += " BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "' GROUP BY ED_OPERATOR1,ED_OPERATOR2 ,ED_OFFICECODE";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtStoreDetails.Load(dr);
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtStoreDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFeederCode"></param>
        /// <param name="sOfficeCode"></param>
        /// <param name="sFromEnumDate"></param>
        /// <param name="sToEnumDate"></param>
        /// <returns></returns>
        public DataTable PrintFieldDetails(string sFeederCode, string sOfficeCode, string sFromEnumDate, string sToEnumDate)
        {
            DataTable dtStoreDetails = new DataTable("A");
            string strQry = string.Empty;
            try
            {
                strQry = "select DISTINCT DTE_DTCCODE,DTE_CESCCODE,DTE_IPCODE,DTE_NAME, (select TM_NAME from TBLTRANSMAKES where TM_ID=DTE_MAKE)DTE_MAKE,DTE_TC_CODE,DTE_TC_SLNO,DTE_CAPACITY,";
                strQry += "(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(ED_OFFICECODE,1,2)) as DIVISION ,";
                strQry += " FD_FEEDER_NAME as FEEDER,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(ED_OFFICECODE,1,3)) as SUBDIVISION,";
                strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(ED_OFFICECODE,1,4)) as SECTION ,TO_CHAR(DTE_TC_MANFDATE,'MM/YYYY') DTE_TC_MANFDATE,";
                strQry += " DTE_TANK_CAPACITY,DTE_TC_WEIGHT,TO_CHAR(ED_WELD_DATE,'DD/MM/YYYY') ED_WELD_DATE,ED_FEEDERCODE FROM ";
                strQry += " TBLDTCENUMERATION,TBLENUMERATIONDETAILS,TBLFEEDERMAST,TBLFEEDEROFFCODE where ED_ID= DTE_ED_ID ";
                strQry += " and ED_LOCTYPE='2' AND ED_STATUS_FLAG IN (0,2) AND  FD_FEEDER_CODE=ED_FEEDERCODE AND FD_FEEDER_ID=FDO_FEEDER_ID ";
                if (sFeederCode != "")
                {
                    strQry += " AND ED_FEEDERCODE Like '" + sFeederCode + "%' ";
                }
                strQry += "AND ED_OFFICECODE LIKE '" + sOfficeCode + "%'";
                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                strQry += " UNION ALL";
                strQry += " SELECT DISTINCT QAO_DTCCODE,QAO_CESCCODE,QAO_IPCODE,QAO_NAME,(select TM_NAME from TBLTRANSMAKES where TM_ID=QAO_MAKE)DTE_MAKE,QAO_TC_CODE,QAO_TC_SLNO,QAO_CAPACITY,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(QA_OFFICECODE,1,2)) as DIVISION ,";
                strQry += " FD_FEEDER_NAME as FEEDER,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(QA_OFFICECODE,1,3)) as SUBDIVISION,";
                strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(QA_OFFICECODE,1,4)) as SECTION ,TO_CHAR(QAO_TC_MANFDATE,'MM/YYYY') DTE_TC_MANFDATE, QAO_TANK_CAPACITY,";
                strQry += " QAO_TC_WEIGHT AS DTE_TC_WEIGHT,TO_CHAR(QA_WELD_DATE,'DD/MM/YYYY') ED_WELD_DATE,QA_FEEDERCODE ";
                strQry += " FROM TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS,TBLFEEDERMAST,TBLFEEDEROFFCODE WHERE QA_ID=QAO_QA_ID  ";
                strQry += " AND QA_LOCTYPE='2' AND QA_FEEDERCODE=FD_FEEDER_CODE AND FD_FEEDER_ID=FDO_FEEDER_ID  ";
                if (sFeederCode != "")
                {
                    strQry += " AND  QA_FEEDERCODE Like '" + sFeederCode + "%' ";
                }
                strQry += "AND QA_OFFICECODE LIKE '" + sOfficeCode + "%'";
                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(QA_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                strQry += " ORDER BY DTE_DTCCODE";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtStoreDetails.Load(dr);
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtStoreDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <param name="sFromEnumDate"></param>
        /// <param name="sToEnumDate"></param>
        /// <returns></returns>
        public DataTable PrintStoreDetails(string sOfficeCode, string sFromEnumDate, string sToEnumDate)
        {
            DataTable dtStoreDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " select (SELECT SM_NAME FROM TBLSTOREMAST WHERE ED_LOCNAME=SM_ID) SM_NAME,DECODE(ED_LOCTYPE,'1','STORE','3','REPAIRER','5','TRANSFORMER BANK') as ENUM_TYPE,TO_CHAR(DTE_TC_MANFDATE,'MM/YYYY') DTE_TC_MANFDATE,";
                strQry += " DTE_TANK_CAPACITY,DTE_TC_WEIGHT,TO_CHAR(DTE_TC_CODE)DTE_TC_CODE,DTE_TC_SLNO,TO_CHAR(DTE_CAPACITY)DTE_CAPACITY,";
                strQry += " (select TM_NAME from TBLTRANSMAKES where TM_ID=DTE_MAKE)DTE_MAKE,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(ED_OFFICECODE,1,2)) as DIVISION ,";
                strQry += "(select FD_FEEDER_NAME from TBLFEEDERMAST where FD_FEEDER_CODE=ED_FEEDERCODE)  as FEEDER,";
                strQry += "(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(ED_OFFICECODE,1,3)) as SUBDIVISION, ";
                strQry += "(SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(ED_OFFICECODE,1,4)) as SECTION,TO_CHAR(ED_WELD_DATE,'DD/MM/YYYY') ED_WELD_DATE,";
                strQry += " CASE ED_LOCTYPE WHEN 1 THEN (SELECT SM_NAME FROM TBLSTOREMAST WHERE ED_LOCNAME=SM_ID) WHEN 3 THEN(SELECT TR_NAME FROM TBLTRANSREPAIRER WHERE ED_LOCNAME=TR_ID) WHEN 5 THEN (SELECT SM_NAME FROM TBLSTOREMAST WHERE ED_LOCNAME=SM_ID) END  LOCNAME,";
                strQry += " (SELECT MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'TCT' AND MD_ID = DTE_TC_TYPE) AS TC_TYPE ";
                strQry += " FROM ";
                strQry += " TBLDTCENUMERATION,TBLENUMERATIONDETAILS where ED_ID= DTE_ED_ID  AND ED_OFFICECODE Like '" + sOfficeCode + "' and  ED_LOCTYPE IN ('1','3','5') AND  ED_STATUS_FLAG IN (0,2) ";
                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(ED_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                strQry += " UNION ALL";
                strQry += " SELECT (SELECT SM_NAME FROM TBLSTOREMAST WHERE QA_LOCNAME=SM_ID) SM_NAME,DECODE(QA_LOCTYPE,'1','STORE','3','REPAIRER','5','TRANSFORMER BANK') as ENUM_TYPE,TO_CHAR(QAO_TC_MANFDATE,'MM/YYYY') DTE_TC_MANFDATE, QAO_TANK_CAPACITY,";
                strQry += " QAO_TC_WEIGHT,TO_CHAR(QAO_TC_CODE) QAO_TC_CODE, QAO_TC_SLNO,TO_CHAR(QAO_CAPACITY) DTE_CAPACITY,";
                strQry += " (select TM_NAME from TBLTRANSMAKES where TM_ID=QAO_MAKE)DTE_MAKE,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(QA_OFFICECODE,1,2)) as DIVISION ,";
                strQry += " (select FD_FEEDER_NAME from TBLFEEDERMAST where FD_FEEDER_CODE=QA_FEEDERCODE)  as FEEDER,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(QA_OFFICECODE,1,3)) as SUBDIVISION,";
                strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(QA_OFFICECODE,1,4)) as SECTION ,";
                strQry += " TO_CHAR(QA_WELD_DATE,'DD/MM/YYYY') ED_WELD_DATE, ";
                strQry += " CASE QA_LOCTYPE WHEN 1 THEN (SELECT SM_NAME FROM TBLSTOREMAST WHERE QA_LOCNAME=SM_ID) WHEN 3 THEN(SELECT TR_NAME FROM TBLTRANSREPAIRER WHERE QA_LOCNAME=TR_ID) WHEN 5 THEN (SELECT SM_NAME FROM TBLSTOREMAST WHERE QA_LOCNAME=SM_ID) END  LOCNAME,";
                strQry += " (SELECT MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'TCT' AND MD_ID = QAO_TC_TYPE) AS TC_TYPE ";
                strQry += " FROM TBLQCAPPROVED,TBLQCAPPROVEDOBJECTS WHERE QA_ID=QAO_QA_ID and QA_OFFICECODE Like '" + sOfficeCode + "%' and QA_LOCTYPE IN ('1','3','5')  ";
                if (sFromEnumDate != "" && sToEnumDate != "")
                {
                    DateTime dFromDate = DateTime.ParseExact(sFromEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dToDate = DateTime.ParseExact(sToEnumDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(QA_CRON,'yyyyMMdd') BETWEEN '" + dFromDate.ToString("yyyyMMdd") + "' AND '" + dToDate.ToString("yyyyMMdd") + "'";
                }
                strQry += " ORDER BY DTE_TC_CODE";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtStoreDetails.Load(dr);
                return dtStoreDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtStoreDetails;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFailureId"></param>
        /// <returns></returns>
        public DataTable PrintEstimatedReport(string sFailureId)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationManager.AppSettings["EmployeeCost"];
            sESI = ConfigurationManager.AppSettings["ESI"];
            ServiceTax = ConfigurationManager.AppSettings["ServiceTax"];
            CGST = ConfigurationManager.AppSettings["CGST"];
            SGST = ConfigurationManager.AppSettings["SGST"];
            DecomLabourCost = ConfigurationManager.AppSettings["DecomLabourCost"];
            try
            {
                strQry = "SELECT DF_ENHANCE_CAPACITY FROM TBLDTCFAILURE WHERE DF_ID='" + sFailureId + "'";
                string sEnhanceCapacity = ObjCon.get_value(strQry);
                if (sEnhanceCapacity == "" || sEnhanceCapacity == null)
                {
                    strQry = "select DF_DTC_CODE,TO_CHAR(DF_EQUIPMENT_ID)DF_EQUIPMENT_ID,cast(DF_STATUS_FLAG as varchar(10))DF_STATUS_FLAG,TO_CHAR(DF_DATE,'dd/MM/yyyy')DF_DATE,DF_LOC_CODE,TO_CHAR(TC_CODE)DTR_CODE,";
                    strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) as SubDivision,";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) as Location,";
                    strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(DF_LOC_CODE,1,2)) as Division,";
                    strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE) DT_NAME,";
                    strQry += " 'No' AS Unit,'1' as Quantity,(select TO_CHAR(TC_CAPACITY) from TBLTCMASTER where TC_CODE=DF_EQUIPMENT_ID)Capacity,";
                    strQry += " TE_RATE as Price,1*TE_RATE AS TotalAmount,TE_COMMLABOUR as labourcharge,(TE_COMMLABOUR*'" + sEmployeeCost + "')/100 as EmployeeCost,(TE_COMMLABOUR*'" + sESI + "')/100 as ESI,(TE_COMMLABOUR*'" + SGST + "')/100 as SGST,(TE_COMMLABOUR*'" + CGST + "')/100 as CGST,(TE_COMMLABOUR*'" + ServiceTax + "')/100 as ServiceTax,";
                    strQry += " ((TE_RATE+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+TE_COMMLABOUR)/100)*2 as ContingencyCost,(TE_RATE+TE_COMMLABOUR+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+((TE_COMMLABOUR*'" + sESI + "')/100)+((TE_COMMLABOUR*'" + ServiceTax + "')/100)+((TE_RATE+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+TE_COMMLABOUR)/100)*2) as FinalTotal,EST_NO,TO_CHAR(DF_ENHANCE_CAPACITY) DF_ENHANCE_CAPACITY, ";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SDO_USERNAME,";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A'  AND US_ID=(SELECT DF_CRBY FROM TBLDTCFAILURE WHERE DF_ID='" + sFailureId + "')) SO_USERNAME ";
                    strQry += " FROM TBLDTCFAILURE,TBLITEMMASTER,TBLESTIMATION,TBLTCMASTER where DF_ID='" + sFailureId + "' AND EST_DF_ID=DF_ID ";
                    strQry += " AND TC_CODE=DF_EQUIPMENT_ID AND TC_CAPACITY=TE_CAPACITY AND NVL(TC_STAR_RATE,0)=NVL(TE_STAR_RATE,0)";
                }
                else
                {
                    strQry = "SELECT * FROM (SELECT DF_DTC_CODE,TO_CHAR(DF_EQUIPMENT_ID)DF_EQUIPMENT_ID,cast(DF_STATUS_FLAG as varchar(10))DF_STATUS_FLAG,TO_CHAR(DF_DATE,'dd/MM/yyyy')DF_DATE,DF_LOC_CODE,";
                    strQry += "(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) as SubDivision, ";
                    strQry += "(SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) as Location,(select TO_CHAR(TC_CAPACITY) from TBLTCMASTER where TC_CODE=DF_EQUIPMENT_ID)Capacity, ";
                    strQry += "(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(DF_LOC_CODE,1,2)) as Division, ";
                    strQry += "(SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE) DT_NAME, 'No' AS Unit,'1' as Quantity,TO_CHAR(DF_ENHANCE_CAPACITY)DF_ENHANCE_CAPACITY,";
                    strQry += "(SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A')";
                    strQry += "SDO_USERNAME, (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A' ";
                    strQry += "AND US_ID=(SELECT DF_CRBY FROM TBLDTCFAILURE WHERE DF_ID='" + sFailureId + "')) SO_USERNAME  FROM TBLDTCFAILURE WHERE DF_ID='" + sFailureId + "')A ";
                    strQry += "RIGHT JOIN (SELECT TE_RATE as Price,1*TE_RATE AS TotalAmount, TE_COMMLABOUR as labourcharge,(TE_COMMLABOUR*'" + sEmployeeCost + "')/100 as ";
                    strQry += "EmployeeCost,(TE_COMMLABOUR*'" + sESI + "')/100 as ESI,(TE_COMMLABOUR*'" + ServiceTax + "')/100 as ServiceTax, (TE_COMMLABOUR* '" + CGST + "')/100 as CGST ,(TE_COMMLABOUR*'" + SGST + "')/100 as SGST , (TE_RATE*2)/100 as ContingencyCost, ";
                    strQry += "(TE_RATE+TE_COMMLABOUR+(TE_RATE*2)/100+(TE_COMMLABOUR*10)/100) as FinalTotal,EST_NO, DF_ENHANCE_CAPACITY FROM TBLITEMMASTER,";
                    strQry += "TBLDTCFAILURE,TBLESTIMATION WHERE  TE_CAPACITY=DF_ENHANCE_CAPACITY AND DF_ID='" + sFailureId + "' AND DF_ID=EST_DF_ID AND TE_STAR_RATE IS NULL)B ON ";
                    strQry += "A.DF_ENHANCE_CAPACITY=B.DF_ENHANCE_CAPACITY";
                }
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtDetailedReport.Load(dr);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetailedReport;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable FailureAbstract(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtFailureAbstract = new DataTable();
            try
            {
                if (objReport.sReportType != "1")
                {
                    strQry = "SELECT CURRENT_MONTH,SELECTED_MONTH,PENDING_FOR_REPLACEMENT,DIV_NAME,Total_Fail_Pending,est_pend,WO_Pending,Indent_Pending,Invoice_Pending, ";
                    strQry += "Decommission_Pending,CR_RI_Pending,wo_office_code,CR_COMPLETED,to_number(TOTAL_DTC)TOTAL_DTC,'" + objReport.sReportType + "' as ReportType FROM (SELECT to_char(SYSDATE,'MONTH YYYY')as CURRENT_MONTH, ";
                    strQry += " to_char(to_date('" + objReport.sMonth + "','YYYY-MM'),'Month yyyy') as SELECTED_MONTH , to_number(nvl(est_pend,0)+nvl(WO_PENDING,0)+nvl(INDENT_PENDING,0)+nvl(INVOICE_PENDING,0)) as  PENDING_FOR_REPLACEMENT, ";
                    strQry += " to_number(nvl(Total_Fail_Pending,0))Total_Fail_Pending,to_number(nvl(est_pend,0))est_pend,to_number(WO_Pending)WO_Pending,to_number(Indent_Pending)Indent_Pending,to_number(nvl(Invoice_Pending,0))Invoice_Pending, ";
                    strQry += "to_number(Decommission_Pending)Decommission_Pending,to_number(CR_RI_Pending)CR_RI_Pending,wo_office_code,nvl(CR_COMPLETED,0)CR_COMPLETED FROM (SELECT WO_Pending,Indent_Pending,Invoice_Pending,Decommission_Pending, ";
                    strQry += "CR_RI_Pending,est_pend,wo_office_code,Total_Fail_Pending from (SELECT WO_Pending,Indent_Pending,est_pend,Invoice_Pending,Decommission_Pending, CR_RI_Pending,a.OFFCODE as wo_office_code from (SELECT substr(OFFCODE,0,2)OFFCODE, ";
                    strQry += "sum(CASE WHEN WORKORDER IS NULL AND FAILURE IS NOT NULL AND to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "'  THEN 1 ELSE 0 END)WO_Pending, sum(CASE WHEN INDENT IS NULL AND WORKORDER IS NOT NULL AND ";
                    strQry += " to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)Indent_Pending, sum(CASE WHEN to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' AND ";
                    strQry += " INVOICE IS NULL AND INDENT IS NOT NULL THEN 1 ELSE 0 END)Invoice_Pending,sum(CASE WHEN DECOMMISION IS NULL AND INVOICE IS NOT NULL AND to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' ";
                    strQry += " THEN 1 ELSE 0 END)Decommission_Pending, sum(CASE WHEN CRREPORT IS NULL AND DECOMMISION IS NOT NULL AND to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)CR_RI_Pending ";
                    strQry += " FROM WORKFLOWSTATUSDUMMY,VIEWPENDINGFAILURE WHERE DT_CODE=WO_DATA_ID AND OFFCODE like '%' AND WO_BO_ID <>10 GROUP BY ";
                    strQry += " substr(OFFCODE, 0, 2) ORDER BY substr(OFFCODE, 0, 2))a LEFT JOIN (SELECT substr(wo_office_code, 0, 2) wo_office_code, ";
                    strQry += " count(*) est_pend FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE '%'  AND WO_BO_ID = '9' AND TO_CHAR(WO_CR_ON, 'YYYY-MM')= '" + objReport.sMonth + "' ";
                    strQry += " AND WO_APPROVE_STATUS = '0' GROUP BY substr(wo_office_code, 0, 2) ORDER BY substr(wo_office_code, 0, 2))b  ON a.OFFCODE = b.wo_office_code)a ";
                    strQry += " LEFT JOIN (SELECT sum(CASE WHEN  DF_STATUS_FLAG IN (1,4) THEN 1 ELSE 0 END)Total_Fail_Pending, substr(DF_LOC_CODE, 0, 2)OFFCODE FROM TBLDTCFAILURE WHERE ";
                    strQry += " DF_STATUS_FLAG IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' AND DF_REPLACE_FLAG = '0'  GROUP BY substr(DF_LOC_CODE, 0, 2) ORDER BY  substr(DF_LOC_CODE, 0, 2))b ";
                    strQry += "on a.wo_office_code = b.OFFCODE)A LEFT  JOIN (SELECT substr(DF_LOC_CODE, 0, 2) OFFCODE, count(*) CR_COMPLETED FROM TBLDTCFAILURE WHERE  DF_REPLACE_FLAG='1' AND ";
                    strQry += "DF_STATUS_FLAG IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' GROUP BY substr(DF_LOC_CODE, 0, 2) )B ON A.WO_OFFICE_CODE=B.OFFCODE)A RIGHT JOIN ";
                    strQry += " (SELECT to_number(TOTAL_DTC)TOTAL_DTC,DT_DIV FROM(SELECT COUNT(DT_CODE)TOTAL_DTC,SUBSTR(DT_OM_SLNO,0,2)DT_DIV FROM TBLDTCMAST GROUP BY ";
                    strQry += " SUBSTR(DT_OM_SLNO,0,2)ORDER BY  SUBSTR(DT_OM_SLNO,0,2)))B ON B.DT_DIV=WO_OFFICE_CODE RIGHT JOIN  TBLDIVISION ON wo_office_code = DIV_CODE ";
                }
                else
                {
                    strQry = "SELECT CURRENT_MONTH,SELECTED_MONTH,PENDING_FOR_REPLACEMENT,DIV_NAME,Total_Fail_Pending,est_pend,WO_Pending,Indent_Pending,Invoice_Pending, ";
                    strQry += "Decommission_Pending,CR_RI_Pending,wo_office_code,CR_COMPLETED,to_number(TOTAL_DTC)TOTAL_DTC,'" + objReport.sReportType + "' as ReportType FROM (SELECT to_char(SYSDATE,'MONTH YYYY')as CURRENT_MONTH, ";
                    strQry += " to_char(to_date('" + objReport.sMonth + "','YYYY-MM'),'Month yyyy') as SELECTED_MONTH , to_number(nvl(est_pend,0)+nvl(WO_PENDING,0)+nvl(INDENT_PENDING,0)+nvl(INVOICE_PENDING,0)) as  PENDING_FOR_REPLACEMENT, ";
                    strQry += " to_number(nvl(Total_Fail_Pending,0))Total_Fail_Pending,to_number(nvl(est_pend,0))est_pend,to_number(WO_Pending)WO_Pending,to_number(Indent_Pending)Indent_Pending,to_number(nvl(Invoice_Pending,0))Invoice_Pending, ";
                    strQry += "to_number(Decommission_Pending)Decommission_Pending,to_number(CR_RI_Pending)CR_RI_Pending,wo_office_code,nvl(CR_COMPLETED,0)CR_COMPLETED FROM (SELECT WO_Pending,Indent_Pending,Invoice_Pending,Decommission_Pending, ";
                    strQry += "CR_RI_Pending,est_pend,wo_office_code,Total_Fail_Pending from (SELECT WO_Pending,Indent_Pending,est_pend,Invoice_Pending,Decommission_Pending, CR_RI_Pending,a.OFFCODE as wo_office_code from (SELECT substr(OFFCODE,0,2)OFFCODE, ";
                    strQry += "sum(CASE WHEN WORKORDER IS NULL AND FAILURE IS NOT NULL AND to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')<='" + objReport.sMonth + "'  THEN 1 ELSE 0 END)WO_Pending, sum(CASE WHEN INDENT IS NULL AND WORKORDER IS NOT NULL AND ";
                    strQry += " to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')<='" + objReport.sMonth + "' THEN 1 ELSE 0 END)Indent_Pending, sum(CASE WHEN to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')<='" + objReport.sMonth + "' AND ";
                    strQry += " INVOICE IS NULL AND INDENT IS NOT NULL THEN 1 ELSE 0 END)Invoice_Pending,sum(CASE WHEN DECOMMISION IS NULL AND INVOICE IS NOT NULL AND to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')<='" + objReport.sMonth + "' ";
                    strQry += " THEN 1 ELSE 0 END)Decommission_Pending, sum(CASE WHEN CRREPORT IS NULL AND DECOMMISION IS NOT NULL AND to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')<='" + objReport.sMonth + "' THEN 1 ELSE 0 END)CR_RI_Pending ";
                    strQry += " FROM WORKFLOWSTATUSDUMMY,VIEWPENDINGFAILURE WHERE DT_CODE=WO_DATA_ID AND OFFCODE like '%' AND WO_BO_ID <>10 GROUP BY ";
                    strQry += " substr(OFFCODE, 0, 2) ORDER BY substr(OFFCODE, 0, 2))a LEFT JOIN (SELECT substr(wo_office_code, 0, 2) wo_office_code, ";
                    strQry += " count(*) est_pend FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE '%'  AND WO_BO_ID = '9' AND TO_CHAR(WO_CR_ON, 'YYYY-MM')<= '" + objReport.sMonth + "' ";
                    strQry += " AND WO_APPROVE_STATUS = '0' GROUP BY substr(wo_office_code, 0, 2) ORDER BY substr(wo_office_code, 0, 2))b  ON a.OFFCODE = b.wo_office_code)a ";
                    strQry += " LEFT JOIN (SELECT sum(CASE WHEN  DF_STATUS_FLAG IN (1,4) THEN 1 ELSE 0 END)Total_Fail_Pending, substr(DF_LOC_CODE, 0, 2)OFFCODE FROM TBLDTCFAILURE WHERE ";
                    strQry += " DF_STATUS_FLAG IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')<= '" + objReport.sMonth + "' AND DF_REPLACE_FLAG = '0'  GROUP BY substr(DF_LOC_CODE, 0, 2) ORDER BY  substr(DF_LOC_CODE, 0, 2))b ";
                    strQry += "on a.wo_office_code = b.OFFCODE)A LEFT  JOIN (SELECT substr(DF_LOC_CODE, 0, 2) OFFCODE, count(*) CR_COMPLETED FROM TBLDTCFAILURE WHERE  DF_REPLACE_FLAG='1' AND ";
                    strQry += "DF_STATUS_FLAG IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')<= '" + objReport.sMonth + "' GROUP BY substr(DF_LOC_CODE, 0, 2) )B ON A.WO_OFFICE_CODE=B.OFFCODE)A RIGHT JOIN ";
                    strQry += " (SELECT to_number(TOTAL_DTC)TOTAL_DTC,DT_DIV FROM(SELECT COUNT(DT_CODE)TOTAL_DTC,SUBSTR(DT_OM_SLNO,0,2)DT_DIV FROM TBLDTCMAST GROUP BY ";
                    strQry += " SUBSTR(DT_OM_SLNO,0,2)ORDER BY  SUBSTR(DT_OM_SLNO,0,2)))B ON B.DT_DIV=WO_OFFICE_CODE RIGHT JOIN  TBLDIVISION ON wo_office_code = DIV_CODE ";
                }
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtFailureAbstract.Load(dr);
                return dtFailureAbstract;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtFailureAbstract;
            }
            finally
            {

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable FailMonthWiseAbstract(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtFailureMonthWiseAbstract = new DataTable();
            try
            {
                #region second time query
                //strQry = "SELECT SD_SUBDIV_CODE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE =SUBSTR";
                //strQry += "(SD_SUBDIV_CODE,0,1))CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE ";
                //strQry += "OFF_CODE =SUBSTR(SD_SUBDIV_CODE,0,2))DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES ";
                //strQry += "WHERE OFF_CODE =SD_SUBDIV_CODE) SUBDIVISION,CAPACITY,OB,CURRENT_MONTH,REPLACED_DURING_THIS_MONTH,TO_BE_REPLACE,";
                //strQry += "CR_COMPLETED FROM (SELECT ISUBDIV AS SD_SUBDIV_CODE,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY,";
                //strQry += "EST_PEND,OB,REPLACED_CURR_MONTH AS REPLACED_DURING_THIS_MONTH,TO_BE_REPLACE,CR_COMPLETE AS CR_COMPLETED,";
                //strQry += "NVL(TOTAL_FAIL_PENDING,0)CURRENT_MONTH FROM (SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,";
                //strQry += "CR_RI_PENDING,CAPACITY,EST_PEND,OB,REPLACED_CURR_MONTH,TO_BE_REPLACE,NVL(CR_COMPLETE,0)CR_COMPLETE FROM ";
                //strQry += "(SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY,EST_PEND,OB,REPLACED_CURR_MONTH,";
                //strQry += "NVL(TO_BE_REPLACE,0)TO_BE_REPLACE FROM (SELECT DISTINCT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,";
                //strQry += "CAPACITY,EST_PEND,OB,NVL(REPLACED_CURR_MONTH,0)REPLACED_CURR_MONTH FROM (SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,";
                //strQry += "INVOICE_PENDING,CR_RI_PENDING,CAPACITY,EST_PEND,NVL(OB,0)OB FROM (SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,";
                //strQry += "INVOICE_PENDING,CR_RI_PENDING,CAPACITY,nvl(EST_PEND,0)EST_PEND FROM(SELECT SUBDIV_CODE AS ISUBDIV,WO_PENDING,";
                //strQry += "INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY FROM (SELECT SUBSTR(OFFCODE,0,3) ISUBDIV, ";
                //strQry += "SUM(CASE WHEN WORKORDER IS NULL AND FAILURE IS NOT NULL AND ";
                //strQry += "to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)WO_PENDING, ";
                //strQry += "SUM(CASE WHEN INDENT IS NULL AND WORKORDER IS NOT NULL AND ";
                //strQry += "to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)INDENT_PENDING, ";
                //strQry += "SUM(CASE WHEN to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' AND INVOICE IS NULL AND INDENT IS NOT NULL THEN 1 ELSE 0 END)";
                //strQry += "INVOICE_PENDING,SUM(CASE WHEN CRREPORT IS NULL AND INVOICE IS NOT NULL AND ";
                //strQry += "to_char(TO_DATE(to_char(DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)CR_RI_PENDING ";
                //strQry += "FROM WORKFLOWSTATUS,VIEWPENDINGFAILURE WHERE WO_DATA_ID=DT_CODE AND OFFCODE LIKE '%' GROUP BY SUBSTR(OFFCODE,0,3) ";
                //strQry += "ORDER BY SUBSTR(OFFCODE,0,3))A RIGHT JOIN (SELECT SD_SUBDIV_CODE AS SUBDIV_CODE,MD_NAME AS CAPACITY FROM ";
                //strQry += "TBLSUBDIVMAST,TBLMASTERDATA WHERE MD_TYPE='C'  AND to_number(MD_NAME) <=500 ORDER BY to_number(MD_NAME))B ON ISUBDIV=SUBDIV_CODE ORDER BY ";
                //strQry += "ISUBDIV,to_number(CAPACITY))A LEFT JOIN (SELECT SUBSTR(WO_OFFICE_CODE, 0, 3) SUBDIV, COUNT(*) EST_PEND FROM ";
                //strQry += "TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE '%'  AND WO_BO_ID = '9' AND WO_APPROVE_STATUS = '0' GROUP BY ";
                //strQry += "SUBSTR(WO_OFFICE_CODE,0,3) ORDER BY SUBSTR(WO_OFFICE_CODE,0,3))B ON A.ISUBDIV = B.SUBDIV )A LEFT JOIN ";


                #region OB old
                //strQry += "(SELECT COUNT(*) AS OB,SUBSTR(WO_OFFCODE, 0, 3) AS SUBDIV, TC_CAPACITY FROM (SELECT  DF_DTC_CODE, ";
                //strQry += "SUBSTR(DF_LOC_CODE, 0, 3)OFFCODE,TC_CAPACITY FROM TBLDTCFAILURE,TBLTCMASTER WHERE DF_EQUIPMENT_ID =TC_CODE AND ";
                //strQry += "DF_STATUS_FLAG IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')< '" + objReport.sMonth + "' AND DF_REPLACE_FLAG='0' ORDER BY ";
                //strQry += "SUBSTR(DF_LOC_CODE,0,3))A RIGHT JOIN (SELECT WO_DATA_ID,OFFCODE AS WO_OFFCODE FROM WORKFLOWSTATUSDUMMY WHERE ";
                //strQry += "CRREPORT IS NULL AND TO_CHAR(WO_DF_DATE, 'YYYY-MM')< '" + objReport.sMonth + "' AND WO_BO_ID <>10  ORDER BY ";
                //strQry += "SUBSTR(OFFCODE,0,3)) ON DF_DTC_CODE =WO_DATA_ID GROUP BY SUBSTR(WO_OFFCODE,0,3),TC_CAPACITY  ORDER BY ";
                //strQry += "SUBSTR(WO_OFFCODE,0,3),TC_CAPACITY)B ";
                #endregion

                //strQry += "(SELECT COUNT(*) AS OB,SUBSTR(OFFCODE, 0, 3) AS SUBDIV,TC_CAPACITY FROM WORKFLOWSTATUSDUMMY,TBLDTCMAST,TBLTCMASTER WHERE  ";
                //strQry += "WO_DATA_ID =DT_CODE(+) AND TC_CODE=DT_TC_ID AND  DECOMMISION IS NULL  AND OFFCODE LIKE '%' AND WO_BO_ID <>10 AND ";
                //strQry += "TO_CHAR(WO_CR_ON, 'YYYY-MM')<> '" + objReport.sMonth + "' GROUP BY OFFCODE,TC_CAPACITY)B ";
                //strQry += "ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY=B.TC_CAPACITY)A LEFT JOIN ";
                //strQry += "(SELECT SUM(CASE WHEN TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' THEN 1 ELSE 0 END)";
                //strQry += "REPLACED_CURR_MONTH, SUBSTR(DF_LOC_CODE, 0, 3)SUBDIV FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,";
                //strQry += "TBLTCREPLACE WHERE DF_ID=WO_DF_ID AND WO_SLNO =TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO =TR_IN_NO AND  DF_STATUS_FLAG ";
                //strQry += "IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' GROUP BY SUBSTR(DF_LOC_CODE,0,3) ORDER BY ";
                //strQry += "SUBSTR(DF_LOC_CODE,0,3))B ON A.ISUBDIV=B.SUBDIV)A LEFT JOIN ";
                //strQry += "(SELECT COUNT(*) AS TO_BE_REPLACE,SUBSTR(OFFCODE, 0, 3) AS SUBDIV,TC_CAPACITY FROM WORKFLOWSTATUSDUMMY,TBLDTCMAST,";
                //strQry += "TBLTCMASTER WHERE  WO_DATA_ID=DT_CODE(+) AND TC_CODE=DT_TC_ID AND INVOICE IS NOT NULL AND DECOMMISION IS NULL  ";
                //strQry += "AND OFFCODE LIKE '%' AND WO_BO_ID <>10 AND TO_CHAR(WO_CR_ON, 'YYYY-MM')<= '" + objReport.sMonth + "' GROUP BY ";
                //strQry += "OFFCODE,TC_CAPACITY)B ";
                //strQry += "ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY=B.TC_CAPACITY)A ";
                //strQry += "LEFT JOIN (SELECT COUNT(DF_DTC_CODE) AS CR_COMPLETE,SUBSTR(DF_LOC_CODE, 0, 3)SUBDIV,TC_CAPACITY FROM TBLDTCFAILURE,";
                //strQry += "TBLTCMASTER WHERE TC_CODE=DF_EQUIPMENT_ID AND TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' AND ";
                //strQry += "DF_REPLACE_FLAG ='1' GROUP BY SUBSTR(DF_LOC_CODE,0,3),TC_CAPACITY ORDER BY SUBSTR(DF_LOC_CODE,0,3),TC_CAPACITY)B ";
                //strQry += "ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY = B.TC_CAPACITY)A LEFT JOIN (SELECT SUM(CASE WHEN  DF_STATUS_FLAG IN ";
                //strQry += "(1,4) THEN 1 ELSE 0 END)TOTAL_FAIL_PENDING,SUBSTR(DF_LOC_CODE, 0, 3)SUBDIV,TC_CAPACITY FROM TBLDTCFAILURE,TBLTCMASTER ";
                //strQry += "WHERE TC_CODE=DF_EQUIPMENT_ID AND DF_STATUS_FLAG IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' ";
                //strQry += "GROUP BY SUBSTR(DF_LOC_CODE, 0, 3),TC_CAPACITY ORDER BY  SUBSTR(DF_LOC_CODE, 0, 3),TC_CAPACITY)B ON ";
                //strQry += "A.ISUBDIV=B.SUBDIV AND A.CAPACITY=B.TC_CAPACITY)";
                #endregion
                //objReport.sReportType -> 1 All , 2 -> selected month 
                if (objReport.sReportType == "1")
                {
                    #region after adding OB+Current_month
                    strQry = "SELECT SD_SUBDIV_CODE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE =SUBSTR(SD_SUBDIV_CODE,0,1))";
                    strQry += "CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE =SUBSTR(SD_SUBDIV_CODE,0,2))DIVISION,";
                    strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE =SD_SUBDIV_CODE) SUBDIVISION,CAPACITY,OB,";
                    strQry += "CURRENT_MONTH,REPLACED_DURING_THIS_MONTH,TO_BE_REPLACE,CR_COMPLETED,(TO_BE_REPLACE+NVL(TO_BE_REPLACE2,0))TO_BE_REPLACE_ALL,";
                    strQry += "(REPLACED_DURING_THIS_MONTH+NVL(REPLACED_DURING_MONTH2,0))REPLACED_TILL_MONTH,'" + objReport.sReportType + "' as ReportType FROM (SELECT ISUBDIV AS SD_SUBDIV_CODE,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,";
                    strQry += "CR_RI_PENDING,CAPACITY,EST_PEND,OB,REPLACED_CURR_MONTH AS REPLACED_DURING_THIS_MONTH,TO_BE_REPLACE,CR_COMPLETE AS CR_COMPLETED,";
                    strQry += "NVL(TOTAL_FAIL_PENDING,0)CURRENT_MONTH FROM (SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY,";
                    strQry += "EST_PEND,OB,REPLACED_CURR_MONTH,TO_BE_REPLACE,NVL(CR_COMPLETE,0)CR_COMPLETE FROM (SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,";
                    strQry += "INVOICE_PENDING,CR_RI_PENDING,CAPACITY,EST_PEND,OB,REPLACED_CURR_MONTH,NVL(TO_BE_REPLACE,0)TO_BE_REPLACE FROM ";
                    strQry += "(SELECT DISTINCT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY,EST_PEND,OB,";
                    strQry += "NVL(REPLACED_CURR_MONTH,0)REPLACED_CURR_MONTH FROM (SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,";
                    strQry += "CAPACITY,EST_PEND,NVL(OB,0)OB FROM (SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY,";
                    strQry += "nvl(EST_PEND,0)EST_PEND FROM(SELECT SUBDIV_CODE AS ISUBDIV,nvl(WO_PENDING,0)WO_PENDING,nvl(INDENT_PENDING,0)INDENT_PENDING,";
                    strQry += "nvl(INVOICE_PENDING,0)INVOICE_PENDING,nvl(CR_RI_PENDING,0)CR_RI_PENDING,CAPACITY FROM (SELECT SUBSTR(OFFCODE,0,3) ISUBDIV,";
                    strQry += "TC_CAPACITY, SUM(CASE WHEN WORKORDER IS NULL AND FAILURE IS NOT NULL AND ";
                    strQry += "to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)WO_PENDING, ";
                    strQry += "SUM(CASE WHEN INDENT IS NULL AND WORKORDER IS NOT NULL AND ";
                    strQry += "to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)INDENT_PENDING, ";
                    strQry += "SUM(CASE WHEN to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' AND INVOICE IS NULL AND ";
                    strQry += "INDENT IS NOT NULL THEN 1 ELSE 0 END)INVOICE_PENDING,sum(CASE WHEN DECOMMISION IS NULL AND INVOICE IS NOT NULL AND ";
                    strQry += "to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)Decommission_Pending,";
                    strQry += "SUM(CASE WHEN CRREPORT IS NULL AND DECOMMISION IS NOT NULL AND ";
                    strQry += "to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')= '" + objReport.sMonth + "' THEN 1 ELSE 0 END)CR_RI_PENDING FROM ";
                    strQry += "WORKFLOWSTATUSDUMMY,VIEWPENDINGFAILURE C,TBLDTCFAILURE A,TBLTCMASTER B WHERE WO_DATA_ID=A.DF_DTC_CODE AND ";
                    strQry += "A.DF_EQUIPMENT_ID=B.TC_CODE AND WO_DATA_ID=DT_CODE AND WO_BO_ID <>10 AND OFFCODE LIKE '%' GROUP BY SUBSTR(OFFCODE,0,3),TC_CAPACITY ";
                    strQry += "ORDER BY SUBSTR(OFFCODE,0,3))A RIGHT JOIN (SELECT SD_SUBDIV_CODE AS SUBDIV_CODE,MD_NAME AS CAPACITY FROM TBLSUBDIVMAST,TBLMASTERDATA ";
                    strQry += "WHERE MD_TYPE='C'  AND to_number(MD_NAME) <=500 ORDER BY to_number(MD_NAME))B ON ISUBDIV=SUBDIV_CODE AND CAPACITY=TC_CAPACITY ORDER ";
                    strQry += "BY ISUBDIV,to_number(CAPACITY))A LEFT JOIN (SELECT SUBSTR(WO_OFFICE_CODE, 0, 3) SUBDIV, COUNT(*) EST_PEND FROM TBLWORKFLOWOBJECTS ";
                    strQry += "WHERE WO_REF_OFFCODE LIKE '%'  AND WO_BO_ID = '9' AND TO_CHAR(WO_CR_ON, 'YYYY-MM')= '" + objReport.sMonth + "' AND ";
                    strQry += "WO_APPROVE_STATUS = '0' GROUP BY SUBSTR(WO_OFFICE_CODE,0,3) ORDER BY SUBSTR(WO_OFFICE_CODE,0,3))B ON A.ISUBDIV = B.SUBDIV  )A ";
                    strQry += "LEFT JOIN (SELECT COUNT(*) AS OB,SUBSTR(OFFCODE, 0, 3) AS SUBDIV,TC_CAPACITY FROM WORKFLOWSTATUSDUMMY,TBLDTCMAST,TBLTCMASTER,";
                    strQry += "TBLDTCFAILURE WHERE WO_DATA_ID=DF_DTC_CODE(+) AND  WO_DATA_ID =DT_CODE(+) AND TC_CODE=DT_TC_ID AND  DECOMMISION IS NULL  AND ";
                    strQry += "OFFCODE LIKE '%' AND WO_BO_ID <>10 AND DF_REPLACE_FLAG='0' AND DT_TC_ID<>0 AND ";
                    strQry += "TO_CHAR(DF_DATE, 'YYYY-MM')< '" + objReport.sMonth + "' GROUP BY SUBSTR(OFFCODE, 0, 3),TC_CAPACITY)B ON A.ISUBDIV=B.SUBDIV AND ";
                    strQry += "A.CAPACITY=B.TC_CAPACITY)A LEFT JOIN (SELECT count(*)REPLACED_CURR_MONTH, SUBSTR(DF_LOC_CODE, 0, 3)SUBDIV,TC_CAPACITY FROM ";
                    strQry += "TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLTCMASTER WHERE DF_EQUIPMENT_ID=TC_CODE AND DF_ID=WO_DF_ID AND ";
                    strQry += "WO_SLNO =TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO =TR_IN_NO AND  DF_STATUS_FLAG IN (1,4) AND TR_RI_DATE IS NOT NULL AND ";
                    strQry += "to_char(DF_DATE,'YYYY-MM')= '" + objReport.sMonth + "' AND DF_REPLACE_FLAG='0'  GROUP BY SUBSTR(DF_LOC_CODE,0,3),TC_CAPACITY ORDER ";
                    strQry += "BY SUBSTR(DF_LOC_CODE,0,3))B ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY=tc_Capacity)A LEFT JOIN (SELECT COUNT(*) AS TO_BE_REPLACE,";
                    strQry += "SUBSTR(DF_LOC_CODE, 0, 3) AS SUBDIV,TC_CAPACITY FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLTCMASTER ";
                    strQry += "WHERE DF_EQUIPMENT_ID =TC_CODE AND DF_ID=WO_DF_ID(+) AND WO_SLNO =TI_WO_SLNO(+) AND TI_ID=IN_TI_NO(+) AND IN_NO =TR_IN_NO(+) AND  ";
                    strQry += "DF_STATUS_FLAG IN (1,4) AND TR_RI_DATE IS NULL AND to_char(DF_DATE,'YYYY-MM')= '" + objReport.sMonth + "' AND DF_REPLACE_FLAG ='0' ";
                    strQry += "GROUP BY SUBSTR(DF_LOC_CODE, 0, 3),TC_CAPACITY ORDER BY subdiv)B ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY=B.TC_CAPACITY)A LEFT JOIN ";
                    strQry += "(SELECT COUNT(DF_DTC_CODE) AS CR_COMPLETE,SUBSTR(DF_LOC_CODE, 0, 3)SUBDIV,TC_CAPACITY FROM TBLDTCFAILURE,TBLTCMASTER WHERE ";
                    strQry += "TC_CODE =DF_EQUIPMENT_ID AND TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' AND DF_STATUS_FLAG IN (1,4) AND ";
                    strQry += "DF_REPLACE_FLAG ='1' GROUP BY SUBSTR(DF_LOC_CODE,0,3),TC_CAPACITY ORDER BY SUBSTR(DF_LOC_CODE,0,3),TC_CAPACITY)B ON ";
                    strQry += "A.ISUBDIV=B.SUBDIV AND A.CAPACITY = B.TC_CAPACITY)A LEFT JOIN (SELECT SUM(CASE WHEN  DF_STATUS_FLAG IN (1,4) THEN 1 ELSE 0 END)";
                    strQry += "TOTAL_FAIL_PENDING,SUBSTR(DF_LOC_CODE, 0, 3)SUBDIV,TC_CAPACITY FROM TBLDTCFAILURE,TBLTCMASTER WHERE TC_CODE=DF_EQUIPMENT_ID ";
                    strQry += "AND DF_STATUS_FLAG IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' GROUP BY SUBSTR(DF_LOC_CODE, 0, 3),";
                    strQry += "TC_CAPACITY ORDER BY  SUBSTR(DF_LOC_CODE, 0, 3),TC_CAPACITY)B ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY=B.TC_CAPACITY)A LEFT JOIN ";
                    strQry += "(SELECT sum(CASE WHEN TR_RI_NO IS NULL THEN 1 ELSE 0 END) TO_BE_REPLACE2,sum(CASE WHEN TR_RI_NO IS NOT NULL THEN 1 ELSE 0 END) ";
                    strQry += "REPLACED_DURING_MONTH2,SUBSTR(OM_CODE, 0, 3)SUBDIV,TC_CAPACITY FROM VIEWPENDINGFAILURE,TBLDTCFAILURE,TBLTCMASTER WHERE ";
                    strQry += "DT_CODE =DF_DTC_CODE AND DF_EQUIPMENT_ID=TC_CODE AND DF_STATUS_FLAG IN (1,4) AND DF_REPLACE_FLAG='0'  AND  ";
                    strQry += "TO_CHAR(TBLDTCFAILURE.DF_DATE, 'YYYY-MM')< '" + objReport.sMonth + "' AND DT_CODE IN (SELECT DT_CODE FROM WORKFLOWSTATUSDUMMY,";
                    strQry += "TBLDTCMAST,TBLTCMASTER,TBLDTCFAILURE WHERE WO_DATA_ID=DF_DTC_CODE(+) AND  WO_DATA_ID =DT_CODE(+) AND TC_CODE=DT_TC_ID AND ";
                    strQry += "DT_TC_ID <>0 AND  DECOMMISION IS NULL  AND OFFCODE LIKE '%' AND WO_BO_ID <>10 AND DF_REPLACE_FLAG='0' AND  ";
                    strQry += "TO_CHAR(DF_DATE, 'YYYY-MM')< '" + objReport.sMonth + "') AND ROWNUM <(SELECT COUNT(*) FROM TBLDTCMAST) GROUP BY ";
                    strQry += "SUBSTR(OM_CODE, 0, 3),tc_capacity)B ON A.SD_SUBDIV_CODE=B.SUBDIV AND A.CAPACITY=B.TC_CAPACITY ORDER BY SD_SUBDIV_CODE";
                    #endregion
                }
                else
                {
                    #region  without adding OB+Current_month

                    strQry = "SELECT SD_SUBDIV_CODE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE =SUBSTR";
                    strQry += "(SD_SUBDIV_CODE,0,1))CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE =";
                    strQry += "SUBSTR(SD_SUBDIV_CODE,0,2))DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE ";
                    strQry += "OFF_CODE =SD_SUBDIV_CODE) SUBDIVISION,CAPACITY,OB,CURRENT_MONTH,REPLACED_DURING_THIS_MONTH,TO_BE_REPLACE,CR_COMPLETED,'" + objReport.sReportType + "' as ReportType  ";
                    strQry += "FROM (SELECT ISUBDIV AS SD_SUBDIV_CODE,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY,EST_PEND,";
                    strQry += "OB,REPLACED_CURR_MONTH AS REPLACED_DURING_THIS_MONTH,TO_BE_REPLACE,CR_COMPLETE AS CR_COMPLETED,";
                    strQry += "NVL(TOTAL_FAIL_PENDING,0)CURRENT_MONTH FROM (SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,";
                    strQry += "CAPACITY,EST_PEND,OB,REPLACED_CURR_MONTH,TO_BE_REPLACE,NVL(CR_COMPLETE,0)CR_COMPLETE FROM ";
                    strQry += "(SELECT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY,EST_PEND,OB,REPLACED_CURR_MONTH,";
                    strQry += "NVL(TO_BE_REPLACE,0)TO_BE_REPLACE FROM (SELECT DISTINCT ISUBDIV,WO_PENDING,INDENT_PENDING,INVOICE_PENDING,";
                    strQry += "CR_RI_PENDING,CAPACITY,EST_PEND,OB,NVL(REPLACED_CURR_MONTH,0)REPLACED_CURR_MONTH FROM (SELECT ISUBDIV,WO_PENDING,";
                    strQry += "INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY,EST_PEND,NVL(OB,0)OB FROM (SELECT ISUBDIV,WO_PENDING,";
                    strQry += "INDENT_PENDING,INVOICE_PENDING,CR_RI_PENDING,CAPACITY,nvl(EST_PEND,0)EST_PEND FROM(SELECT SUBDIV_CODE AS ISUBDIV,";
                    strQry += "nvl(WO_PENDING,0)WO_PENDING,nvl(INDENT_PENDING,0)INDENT_PENDING,nvl(INVOICE_PENDING,0)INVOICE_PENDING,";
                    strQry += "nvl(CR_RI_PENDING,0)CR_RI_PENDING,CAPACITY FROM (SELECT SUBSTR(OFFCODE,0,3) ISUBDIV,TC_CAPACITY, ";
                    strQry += "SUM(CASE WHEN WORKORDER IS NULL AND FAILURE IS NOT NULL AND to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')";
                    strQry += "='" + objReport.sMonth + "' THEN 1 ELSE 0 END)WO_PENDING, SUM(CASE WHEN INDENT IS NULL AND WORKORDER IS NOT NULL AND ";
                    strQry += "to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)INDENT_PENDING, ";
                    strQry += "SUM(CASE WHEN to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' AND INVOICE IS NULL AND ";
                    strQry += "INDENT IS NOT NULL THEN 1 ELSE 0 END)INVOICE_PENDING,sum(CASE WHEN DECOMMISION IS NULL AND INVOICE IS NOT NULL ";
                    strQry += "AND to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)";
                    strQry += "Decommission_Pending,SUM(CASE WHEN CRREPORT IS NULL AND DECOMMISION IS NOT NULL AND ";
                    strQry += "to_char(TO_DATE(to_char(C.DF_DATE),'DD-MM-YYYY'),'YYYY-MM')='" + objReport.sMonth + "' THEN 1 ELSE 0 END)CR_RI_PENDING ";
                    strQry += "FROM WORKFLOWSTATUSDUMMY,VIEWPENDINGFAILURE C,TBLDTCFAILURE A,TBLTCMASTER B WHERE WO_DATA_ID=A.DF_DTC_CODE AND ";
                    strQry += "A.DF_EQUIPMENT_ID=B.TC_CODE AND WO_DATA_ID=DT_CODE AND WO_BO_ID <>10 AND OFFCODE LIKE '%' GROUP BY ";
                    strQry += "SUBSTR(OFFCODE,0,3),TC_CAPACITY ORDER BY SUBSTR(OFFCODE,0,3))A RIGHT JOIN (SELECT SD_SUBDIV_CODE AS SUBDIV_CODE,";
                    strQry += "MD_NAME AS CAPACITY FROM TBLSUBDIVMAST,TBLMASTERDATA WHERE MD_TYPE='C'  AND to_number(MD_NAME) <=500 ORDER BY ";
                    strQry += "to_number(MD_NAME))B ON ISUBDIV=SUBDIV_CODE AND CAPACITY=TC_CAPACITY ORDER BY ISUBDIV,to_number(CAPACITY))A LEFT ";
                    strQry += "JOIN (SELECT SUBSTR(WO_OFFICE_CODE, 0, 3) SUBDIV, COUNT(*) EST_PEND FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE ";
                    strQry += "LIKE '%'  AND WO_BO_ID = '9' AND TO_CHAR(WO_CR_ON, 'YYYY-MM')= '" + objReport.sMonth + "' AND WO_APPROVE_STATUS = '0' ";
                    strQry += "GROUP BY SUBSTR(WO_OFFICE_CODE,0,3) ORDER BY SUBSTR(WO_OFFICE_CODE,0,3))B ON A.ISUBDIV = B.SUBDIV  )A LEFT JOIN ";
                    strQry += "(SELECT COUNT(*) AS OB,SUBSTR(OFFCODE, 0, 3) AS SUBDIV,TC_CAPACITY FROM WORKFLOWSTATUSDUMMY,TBLDTCMAST,TBLTCMASTER,";
                    strQry += "TBLDTCFAILURE WHERE WO_DATA_ID=DF_DTC_CODE(+) AND  WO_DATA_ID =DT_CODE(+) AND TC_CODE=DT_TC_ID AND  DECOMMISION IS NULL  ";
                    strQry += "AND OFFCODE LIKE '%' AND WO_BO_ID <>10 AND DF_REPLACE_FLAG='0' AND TO_CHAR(DF_DATE, 'YYYY-MM')< '" + objReport.sMonth + "' GROUP BY SUBSTR(OFFCODE, 0, 3),";
                    strQry += "TC_CAPACITY)B ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY=B.TC_CAPACITY)A LEFT JOIN (SELECT count(*)REPLACED_CURR_MONTH, ";
                    strQry += "SUBSTR(DF_LOC_CODE, 0, 3)SUBDIV,TC_CAPACITY FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,";
                    strQry += "TBLTCMASTER WHERE DF_EQUIPMENT_ID=TC_CODE AND DF_ID=WO_DF_ID AND WO_SLNO =TI_WO_SLNO AND TI_ID=IN_TI_NO AND ";
                    strQry += "IN_NO =TR_IN_NO AND  DF_STATUS_FLAG IN (1,4) AND TR_RI_DATE IS NOT NULL AND to_char(DF_DATE,'YYYY-MM')='" + objReport.sMonth + "' ";
                    strQry += "AND DF_REPLACE_FLAG='0'  GROUP BY SUBSTR(DF_LOC_CODE,0,3),TC_CAPACITY ORDER BY SUBSTR(DF_LOC_CODE,0,3))B ";
                    strQry += "ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY=tc_Capacity)A LEFT JOIN (SELECT COUNT(*) AS TO_BE_REPLACE,SUBSTR(DF_LOC_CODE, 0, 3) ";
                    strQry += "AS SUBDIV,TC_CAPACITY FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLTCMASTER WHERE ";
                    strQry += "DF_EQUIPMENT_ID =TC_CODE AND DF_ID=WO_DF_ID(+) AND WO_SLNO =TI_WO_SLNO(+) AND TI_ID=IN_TI_NO(+) AND IN_NO =TR_IN_NO(+) ";
                    strQry += "AND  DF_STATUS_FLAG IN (1,4) AND TR_RI_DATE IS NULL AND to_char(DF_DATE,'YYYY-MM')='" + objReport.sMonth + "' AND ";
                    strQry += "DF_REPLACE_FLAG ='0' GROUP BY SUBSTR(DF_LOC_CODE, 0, 3),TC_CAPACITY ORDER BY subdiv)B ON A.ISUBDIV=";
                    strQry += "B.SUBDIV AND A.CAPACITY=B.TC_CAPACITY)A LEFT JOIN (SELECT COUNT(DF_DTC_CODE) AS CR_COMPLETE,SUBSTR(DF_LOC_CODE, 0, 3)";
                    strQry += "SUBDIV,TC_CAPACITY FROM TBLDTCFAILURE,TBLTCMASTER WHERE TC_CODE=DF_EQUIPMENT_ID AND ";
                    strQry += "TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' AND DF_STATUS_FLAG IN (1,4) AND DF_REPLACE_FLAG ='1' GROUP BY SUBSTR(DF_LOC_CODE,0,3),";
                    strQry += "TC_CAPACITY ORDER BY SUBSTR(DF_LOC_CODE,0,3),TC_CAPACITY)B ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY = B.TC_CAPACITY)A ";
                    strQry += "LEFT JOIN (SELECT SUM(CASE WHEN  DF_STATUS_FLAG IN (1,4) THEN 1 ELSE 0 END)TOTAL_FAIL_PENDING,";
                    strQry += "SUBSTR(DF_LOC_CODE, 0, 3)SUBDIV,TC_CAPACITY FROM TBLDTCFAILURE,TBLTCMASTER WHERE TC_CODE=DF_EQUIPMENT_ID AND ";
                    strQry += "DF_STATUS_FLAG IN (1,4) AND TO_CHAR(DF_DATE, 'YYYY-MM')= '" + objReport.sMonth + "' GROUP BY SUBSTR(DF_LOC_CODE, 0, 3),";
                    strQry += "TC_CAPACITY ORDER BY  SUBSTR(DF_LOC_CODE, 0, 3),TC_CAPACITY)B ON A.ISUBDIV=B.SUBDIV AND A.CAPACITY=B.TC_CAPACITY) ";

                    #endregion without adding OB+Current_month
                }
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtFailureMonthWiseAbstract.Load(dr);
                return dtFailureMonthWiseAbstract;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtFailureMonthWiseAbstract;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable MisReplacableDTr(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtReplacableDTRs = new DataTable();
            try
            {
                strQry = "SELECT * FROM VIEWREPLACABLEDTR";
                dtReplacableDTRs = ObjCon.getDataTable(strQry);
                return dtReplacableDTRs;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtReplacableDTRs;
            }
        }



        #region EstComPrevSO
        public DataTable PrintEstimatedReportSO(string sDtrCode, string sWFObject, string sNewCap, string statusflag)
        {

            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
            sESI = ConfigurationSettings.AppSettings["ESI"];
            ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];
            CGST = ConfigurationSettings.AppSettings["CGST"];
            SGST = ConfigurationSettings.AppSettings["SGST"];

            DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];

            try
            {

                if (sNewCap == "")
                {
                    strQry = "select DISTINCT WO_DATA_ID as DF_DTC_CODE,'" + statusflag + "' as DF_STATUS_FLAG,('" + sDtrCode + "') as DF_EQUIPMENT_ID,TO_CHAR(WO_CR_ON,'dd/MM/yyyy')DF_DATE,WO_OFFICE_CODE,'' AS EST_NO,";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(WO_OFFICE_CODE,1,4)) as Location,";
                    strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(WO_OFFICE_CODE,1,3)) as SubDivision, ";
                    strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(WO_OFFICE_CODE,1,2)) as Division,";
                    strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=WO_DATA_ID) DT_NAME, 'No' AS Unit,'1' as Quantity,";
                    strQry += " (select TO_CHAR(TC_CAPACITY) from TBLTCMASTER where TC_CODE='" + sDtrCode + "')Capacity, TE_RATE as Price,1*TE_RATE AS TotalAmount,";
                    strQry += " TE_COMMLABOUR as labourcharge,(TE_COMMLABOUR*'" + sEmployeeCost + "')/100 as EmployeeCost,(TE_COMMLABOUR*'" + sESI + "')/100 as ESI,(TE_COMMLABOUR*'" + CGST + "')/100 as CGST,(TE_COMMLABOUR*'" + SGST + "')/100 as SGST, (TE_COMMLABOUR*'" + ServiceTax + "')/100 as ServiceTax, (TE_RATE*2)/100 as ContingencyCost,";
                    strQry += " (TE_RATE+TE_COMMLABOUR+(TE_RATE*2)/100+(TE_COMMLABOUR*10)/100) as FinalTotal,('" + sNewCap + "') as DF_ENHANCE_CAPACITY,";
                    strQry += "(SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=WO_OFFICE_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME";
                    strQry += " FROM TBLDTCFAILURE,TBLITEMMASTER,TBLWORKFLOWOBJECTS,TBLTCMASTER WHERE ";
                    strQry += " TC_CODE='" + sDtrCode + "' AND TC_CODE=DF_EQUIPMENT_ID(+) AND TC_CAPACITY=TE_CAPACITY AND NVL(TC_STAR_RATE,0)=NVL(TE_STAR_RATE,0) AND WO_ID='" + sWFObject + "'";
                }
                else
                {
                    strQry = "SELECT * FROM (SELECT DISTINCT WO_DATA_ID as DF_DTC_CODE ,'" + statusflag + "' as DF_STATUS_FLAG,'" + sDtrCode + "' as DF_EQUIPMENT_ID,TO_CHAR(WO_CR_ON,'dd/MM/yyyy')DF_DATE";
                    strQry += ",WO_OFFICE_CODE,'' AS EST_NO,(SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(WO_OFFICE_CODE,1,4)) as Location, ";
                    strQry += "(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(WO_OFFICE_CODE,1,3)) as SubDivision,  ";
                    strQry += "(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(WO_OFFICE_CODE,1,2)) as Division,(select TO_CHAR(TC_CAPACITY) from TBLTCMASTER where TC_CODE='" + sDtrCode + "')Capacity,";
                    strQry += "(SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=WO_DATA_ID) DT_NAME, 'No' AS Unit,'1' as Quantity,'" + sNewCap + "' as ";
                    strQry += " DF_ENHANCE_CAPACITY,(SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=WO_OFFICE_CODE AND US_ROLE_ID='4' AND US_MMS_ID IS NULL AND US_STATUS='A' ";
                    strQry += "AND ROWNUM='1')SO_USERNAME FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + sWFObject + "')A RIGHT JOIN ";
                    strQry += "(SELECT TE_RATE as Price,1*TE_RATE AS TotalAmount, TE_COMMLABOUR as labourcharge,(TE_COMMLABOUR*'" + sEmployeeCost + "')/100 as EmployeeCost,";
                    strQry += "(TE_COMMLABOUR*'" + sESI + "')/100 as ESI,(TE_COMMLABOUR*'" + CGST + "')/100 as CGST,(TE_COMMLABOUR*'" + SGST + "')/100 as SGST,(TE_COMMLABOUR*'" + ServiceTax + "')/100 as ServiceTax , (TE_RATE*2)/100 as ContingencyCost, ";
                    strQry += "(TE_RATE+TE_COMMLABOUR+(TE_RATE*2)/100+(TE_COMMLABOUR*10)/100) as FinalTotal,'" + sNewCap + "' as DF_ENHANCE_CAPACITY FROM ";
                    strQry += "TBLITEMMASTER WHERE  TE_CAPACITY='" + sNewCap + "' AND TE_STAR_RATE IS NULL)B ON A.DF_ENHANCE_CAPACITY=B.DF_ENHANCE_CAPACITY";
                }
                //OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtDetailedReport = ObjCon.getDataTable(strQry);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetailedReport;
            }
        }
        #endregion

        #region EstDecPrevSO
        public DataTable PrintDecomEstimationReportSO(string sDtrCode, string sWFObject, string sRes, string sNewCap, string sGuaranty, string statusflag)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationSettings.AppSettings["EmployeeCost"];
            sESI = ConfigurationSettings.AppSettings["ESI"];
            ServiceTax = ConfigurationSettings.AppSettings["ServiceTax"];
            DecomLabourCost = ConfigurationSettings.AppSettings["DecomLabourCost"];

            try
            {
                strQry = " SELECT WO_DATA_ID as DF_DTC_CODE,DT_NAME,('" + sRes.Replace("ç", ",") + "') AS DF_REASON,'" + statusflag + "' as DF_STATUS_FLAG, FD_FEEDER_NAME, TE_COMMLABOUR,(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST  WHERE SD_SUBDIV_CODE=SUBSTR(WO_OFFICE_CODE,1,3)) as SubDivision,(SELECT DIV_NAME FROM TBLDIVISION  WHERE DIV_CODE=SUBSTR(WO_OFFICE_CODE,1,2)) as Division,";
                strQry += " ('" + sDtrCode + "') AS DF_EQUIPMENT_ID,('" + sNewCap + "') as DF_ENHANCE_CAPACITY,to_char(WO_CR_ON,'dd/MM/yyyy')DF_DATE,WO_OFFICE_CODE,'' AS EST_NO, ";
                strQry += " (SELECT  to_char(TM_MAPPING_DATE,'dd/MON/yyyy') TM_MAPPING_DATE FROM TBLTRANSDTCMAPPING WHERE TM_TC_ID=TC_CODE AND TM_LIVE_FLAG='1' AND ROWNUM = 1) DTR_COMMISSION_DATE,to_char(DT_TRANS_COMMISION_DATE,'dd/MON/yyyy')DTC_COMMISSION_DATE, TE_RATE as Price , ";
                strQry += " TE_COMMLABOUR TE_DECOMLABOUR,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_CODE,TC_SLNO,'OLD' AS Rep, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TC_MAKE_ID=TM_ID)TM_NAME, ";
                strQry += "  DT_TOTAL_CON_KW,(1*TE_COMMLABOUR*'" + DecomLabourCost + "') LABOUR_COST,  (TE_COMMLABOUR*'" + sEmployeeCost + "')/100 as EmployeeCost,(TE_COMMLABOUR*'" + sESI + "')/100 as ESI,(TE_COMMLABOUR*'" + ServiceTax + "'*'" + DecomLabourCost + "')/100 as ServiceTax, (TE_COMMLABOUR*'" + CGST + "'*'" + DecomLabourCost + "')/100 as CGST, (TE_COMMLABOUR*'" + SGST + "'*'" + DecomLabourCost + "')/100 as SGST , ((((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+(TE_COMMLABOUR*'" + DecomLabourCost + "'))/100)*2 as ContingencyCost, ((TE_COMMLABOUR*'" + DecomLabourCost + "')+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+((TE_COMMLABOUR*'" + sESI + "')/100)+((TE_COMMLABOUR*'" + ServiceTax + "')/100)+((((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+(TE_COMMLABOUR*'" + DecomLabourCost + "'))/100)*2) as FinalTotal, ";
                strQry += " 'No' as Unit,'1' as Quantity,(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST  WHERE SD_SUBDIV_CODE=SUBSTR(WO_OFFICE_CODE,1,3)) as SubDivision, (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(WO_OFFICE_CODE,1,4)) as Location, ";
                strQry += " '" + sGuaranty + "' as  TR_GUARANTY,(1*TE_COMMLABOUR*'" + DecomLabourCost + "') LABOUR_COST,  ";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=WO_OFFICE_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME  from  TBLWORKFLOWOBJECTS, ";
                strQry += " TBLITEMMASTER,TBLTCMASTER,TBLDTCMAST,TBLFEEDERMAST WHERE WO_DATA_ID=DT_CODE AND  TC_CODE='" + sDtrCode + "' AND wo_id='" + sWFObject + "' ";
                strQry += "  AND TC_CAPACITY=TE_CAPACITY AND NVL(TC_STAR_RATE,0)=NVL(TE_STAR_RATE,0) AND FD_FEEDER_CODE=SUBSTR(WO_DATA_ID,0,4) ";

                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtDetailedReport.Load(dr);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetailedReport;
            }
        }
        #endregion

        #region EstSurvyPrevSO
        public DataTable PrintSurveyReportSO(DataTable dt, string sWoID, string sTCcode, string sNewCap)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                string Customername = string.Empty;
                string Customernumber = string.Empty;
                string Insulationnumber = string.Empty;
                string sDfType = Convert.ToString(dt.Rows[0]["DF_FAILURE_TYPE"]).Trim();
                string sDfHtBus = Convert.ToString(dt.Rows[0]["DF_HT_BUSING"]).Trim();
                string sDfLTBus = Convert.ToString(dt.Rows[0]["DF_LT_BUSING"]).Trim();
                string sDFHTRoad = Convert.ToString(dt.Rows[0]["DF_HT_BUSING_ROD"]).Trim();
                string sDFLTBusRoad = Convert.ToString(dt.Rows[0]["DF_LT_BUSING_ROD"]).Trim();
                string sDfBrea = Convert.ToString(dt.Rows[0]["DF_BREATHER"]).Trim();
                string sDFOilL = Convert.ToString(dt.Rows[0]["DF_OIL_LEVEL"]).Trim();
                string DFdrainV = Convert.ToString(dt.Rows[0]["DF_DRAIN_VALVE"]).Trim();
                string sDFOilQty = Convert.ToString(dt.Rows[0]["DF_OIL_QNTY"]).Trim();
                string DFTankCon = Convert.ToString(dt.Rows[0]["DF_TANK_CONDITION"]).Trim();
                string sDFExp = Convert.ToString(dt.Rows[0]["DF_EXPLOSION"]).Trim();
                string sDFKWHRead = Convert.ToString(dt.Rows[0]["DF_KWH_READING"]);

                if (dt.Columns.Contains("DF_CUSTOMER_NAME") && dt.Columns.Contains("DF_CUSTOMER_MOBILE") && dt.Columns.Contains("DF_NUMBER_OF_INSTALLATIONS"))
                {
                    Customername = Convert.ToString(dt.Rows[0]["DF_CUSTOMER_NAME"]);
                    Customernumber = Convert.ToString(dt.Rows[0]["DF_CUSTOMER_MOBILE"]);
                    Insulationnumber = Convert.ToString(dt.Rows[0]["DF_NUMBER_OF_INSTALLATIONS"]);
                }


                strQry = " SELECT DISTINCT ('" + sTCcode + "') AS DF_EQUIPMENT_ID,TO_CHAR(WO_CR_ON,'DD/MM/YYYY')DF_DATE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,('" + sNewCap + "') as DF_ENHANCE_CAPACITY, ";
                strQry += " TO_CHAR(TC_MANF_DATE,'DD/MM/YYYY')TC_MANF_DATE,SM_NAME,WO_OFFICE_CODE,TM_NAME,TC_SLNO,(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE ";
                strQry += " SD_SUBDIV_CODE=SUBSTR(WO_OFFICE_CODE,1,3)) AS SUBDIVISION,(SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(WO_OFFICE_CODE,1,4)) ";
                strQry += " AS LOCATION,(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(WO_OFFICE_CODE,1,2)) AS DIVISION,";
                strQry += " DECODE('" + sDfHtBus + "' , 1 , 'GOOD',  2 , 'BAD' , '')DF_HT_BUSING, ";
                strQry += " DECODE('" + sDfLTBus + "' ,1 , 'GOOD' , 2 , 'BAD' , '')DF_LT_BUSING, ";
                strQry += " DECODE('" + sDFHTRoad + "', 1 , 'GOOD' , 2 , 'BAD' , '')DF_HT_BUSING_ROD, ";
                strQry += " DECODE('" + sDFLTBusRoad + "',1 , 'GOOD' , 2 , 'BAD' , '')DF_LT_BUSING_ROD, ";
                strQry += " DECODE('" + sDfBrea + "',1,'YES',2,'NO','')DF_BREATHER, ";
                strQry += " DECODE('" + sDFOilL + "',1,'YES',2,'NO','')DF_OIL_LEVEL, ";
                strQry += " DECODE('" + DFdrainV + "',1,'YES',2,'NO','')DF_DRAIN_VALVE, ";
                strQry += "'" + sDFOilQty + "' AS DF_OIL_QNTY, ";
                strQry += " DECODE('" + DFTankCon + "' , 1 , 'GOOD' , 2 , 'BAD' , '')DF_TANK_CONDITION, ";
                strQry += " DECODE('" + sDFExp + "',1,'YES',2,'NO','')DF_EXAMPLING,'' AS EST_NO, ";
                strQry += "'" + Customername + "' AS DF_CUSTOMER_NAME, ";
                strQry += "'" + Customernumber + "' AS DF_CUSTOMER_MOBILE, ";
                strQry += "'" + Insulationnumber + "' AS DF_NUMBER_OF_INSTALLATIONS, ";

                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=WO_OFFICE_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME FROM ";
                strQry += " TBLTCMASTER,TBLTRANSMAKES,TBLSTOREMAST,TBLWORKFLOWOBJECTS WHERE TC_CODE='" + sTCcode + "' AND TM_ID=TC_MAKE_ID AND ";
                strQry += "TC_STORE_ID=SM_ID AND wo_id='" + sWoID + "'";

                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtDetailedReport.Load(dr);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetailedReport;

            }
            finally
            {

            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFailureId"></param>
        /// <returns></returns>
        public DataTable PrintWorkOrderReport(string sFailureId)
        {
            DataTable dtWorkOrderDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT  WO_SLNO,WO_NO,WO_DF_ID,TO_CHAR(WO_DATE,'dd/MM/yyyy')WO_DATE,TO_CHAR(WO_AMT)WO_AMT,WO_NO_DECOM,WO_ACC_CODE,WO_ACCCODE_DECOM,";
                strQry += "  WO_OFF_CODE,WO_CRBY,TO_CHAR(WO_CRON,'dd/MM/yyyy')WO_CRON,TO_CHAR(WO_DATE_DECOM,'dd/MM/yyyy')WO_DATE_DECOM,TO_CHAR(WO_AMT_DECOM)WO_AMT_DECOM,WO_ISSUED_BY,";
                strQry += " TO_CHAR(DF_DATE,'DD/MM/YYYY')DF_FAILED_DATE,WO_NEW_CAP,WO_REQUEST_LOC,DF_ENHANCE_CAPACITY,(SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE= WO_OFF_CODE AND US_ROLE_ID=7 AND US_MMS_ID IS NULL AND US_STATUS='A')AET_USERNAME,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE= WO_OFF_CODE AND US_ROLE_ID=2 AND US_MMS_ID IS NULL AND US_STATUS='A')STO_USERNAME,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE= WO_OFF_CODE AND US_ROLE_ID=6 AND US_MMS_ID IS NULL AND US_STATUS='A')AO_USERNAME,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE= WO_OFF_CODE AND US_ROLE_ID=3 AND US_MMS_ID IS NULL AND US_STATUS='A')DO_USERNAME";
                strQry += " FROM TBLWORKORDER,TBLDTCFAILURE WHERE DF_ID=WO_DF_ID AND DF_ID='" + sFailureId + "'";
                dtWorkOrderDetails = ObjCon.getDataTable(strQry);
                return dtWorkOrderDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtWorkOrderDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFailureId"></param>
        /// <returns></returns>
        public DataTable PrintDecomEstimatedReport(string sFailureId)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            sEmployeeCost = ConfigurationManager.AppSettings["EmployeeCost"];
            sESI = ConfigurationManager.AppSettings["ESI"];
            ServiceTax = ConfigurationManager.AppSettings["ServiceTax"];
            DecomLabourCost = ConfigurationManager.AppSettings["DecomLabourCost"];
            try
            {
                strQry = "SELECT DF_EQUIPMENT_ID FROM TBLDTCFAILURE WHERE DF_ID='" + sFailureId + "'";
                string tc_code = ObjCon.get_value(strQry);
                strQry = "SELECT RSM_GUARANTY_TYPE FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS WHERE RSM_ID=RSD_RSM_ID AND RSD_ID = (SELECT MAX(RSD_ID) FROM TBLREPAIRSENTDETAILS,TBLTCMASTER  WHERE RSD_TC_CODE= TC_CODE AND tc_code='" + tc_code + "')";
                // changed on 31-jan-2020
                // since the guaranty  type has to be taken from TBLDTCFAILURE Table and not from Repairer .
                string res = null;
                if (res == null || res == "")
                {
                    strQry = " SELECT DF_ID,DF_DTC_CODE,cast(DF_STATUS_FLAG as varchar(10))DF_STATUS_FLAG, FD_FEEDER_NAME,DT_NAME,TO_CHAR(TC_CODE)DTR_CODE,(SELECT DIV_NAME FROM TBLDIVISION  WHERE DIV_CODE=SUBSTR(DF_LOC_CODE,1,2)) as Division,";
                    strQry += " TO_CHAR(DF_EQUIPMENT_ID)DF_EQUIPMENT_ID,to_char(DF_DATE,'dd/MM/yyyy')DF_DATE,Replace(DF_REASON,'ç',',')DF_REASON,DF_LOC_CODE,TO_CHAR(DF_DTR_COMMISSION_DATE,'DD/MON/YYYY') AS DTR_COMMISSION_DATE,to_char(DT_TRANS_COMMISION_DATE,'dd/MON/yyyy')DTC_COMMISSION_DATE,";
                    strQry += " TE_RATE as Price ,(TE_COMMLABOUR*'" + DecomLabourCost + "') as TE_DECOMLABOUR,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_CODE,TC_SLNO,'OLD' AS Rep,TE_COMMLABOUR,";
                    strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TC_MAKE_ID=TM_ID)TM_NAME,DT_TOTAL_CON_KW,(TE_COMMLABOUR*'" + sEmployeeCost + "')/100 as EmployeeCost,";
                    strQry += " (TE_COMMLABOUR*'" + sESI + "')/100 as ESI,(TE_COMMLABOUR*'" + ServiceTax + "'*'" + DecomLabourCost + "')/100 as ServiceTax,  (TE_COMMLABOUR*'" + CGST + "'*'" + DecomLabourCost + "')/100 as CGST,  (TE_COMMLABOUR*'" + SGST + "'*'" + DecomLabourCost + "')/100 as SGST,    ((((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+(TE_COMMLABOUR*'" + DecomLabourCost + "'))/100)*2 as ContingencyCost, ((TE_COMMLABOUR*'" + DecomLabourCost + "')+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+((TE_COMMLABOUR*'" + sESI + "')/100)+((TE_COMMLABOUR*'" + ServiceTax + "')/100)+((((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+(TE_COMMLABOUR*'" + DecomLabourCost + "'))/100)*2) as FinalTotal,";
                    strQry += " 'No' as Unit,'1' as Quantity,(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST  WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) as SubDivision,";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) as Location,";
                    strQry += " EST_REPAIR AS TR_NAME,DF_GUARANTY_TYPE AS TR_GUARANTY";
                    strQry += " ,EST_NO,(1*TE_COMMLABOUR*'" + DecomLabourCost + "') LABOUR_COST,TO_CHAR(DF_ENHANCE_CAPACITY) DF_ENHANCE_CAPACITY, ";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SDO_USERNAME,";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A' AND US_ID=(SELECT DF_CRBY FROM TBLDTCFAILURE WHERE DF_ID='" + sFailureId + "')) SO_USERNAME ";
                    strQry += " from  TBLDTCFAILURE,TBLITEMMASTER,TBLTCMASTER,TBLDTCMAST,TBLESTIMATION,TBLFEEDERMAST WHERE DF_DTC_CODE=DT_CODE AND ";
                    strQry += " DF_EQUIPMENT_ID=TC_CODE AND DF_ID='" + sFailureId + "' AND EST_DF_ID=DF_ID AND TC_CAPACITY=TE_CAPACITY AND NVL(TC_STAR_RATE,0)=NVL(TE_STAR_RATE,0)";
                    strQry += " AND FD_FEEDER_CODE=SUBSTR(DF_DTC_CODE,0,4)";
                }
                else
                {
                    strQry = " SELECT DF_ID,DF_DTC_CODE,cast(DF_STATUS_FLAG as varchar(10))DF_STATUS_FLAG, FD_FEEDER_NAME,DT_NAME,TO_CHAR(TC_CODE)DTR_CODE,(SELECT DIV_NAME FROM TBLDIVISION  WHERE DIV_CODE=SUBSTR(DF_LOC_CODE,1,2)) as Division,";
                    strQry += " TO_CHAR(DF_EQUIPMENT_ID)DF_EQUIPMENT_ID,to_char(DF_DATE,'dd/MM/yyyy')DF_DATE,Replace(DF_REASON,'ç',',')DF_REASON,DF_LOC_CODE,TO_CHAR(DF_DTR_COMMISSION_DATE,'DD/MON/YYYY') AS DTR_COMMISSION_DATE,to_char(DT_TRANS_COMMISION_DATE,'dd/MON/yyyy')DTC_COMMISSION_DATE,";
                    strQry += " TE_RATE as Price ,(TE_COMMLABOUR*'" + DecomLabourCost + "') as TE_DECOMLABOUR,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_CODE,TC_SLNO,'OLD' AS Rep,TE_COMMLABOUR,";
                    strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TC_MAKE_ID=TM_ID)TM_NAME,DT_TOTAL_CON_KW,(TE_COMMLABOUR*'" + sEmployeeCost + "')/100 as EmployeeCost,";
                    strQry += " (TE_COMMLABOUR*'" + sESI + "')/100 as ESI,(TE_COMMLABOUR*'" + ServiceTax + "')/100 as ServiceTax,((((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+(TE_COMMLABOUR*'" + DecomLabourCost + "'))/100)*2 as ContingencyCost, ((TE_COMMLABOUR*'" + DecomLabourCost + "')+((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+((TE_COMMLABOUR*'" + sESI + "')/100)+((TE_COMMLABOUR*'" + ServiceTax + "')/100)+((((TE_COMMLABOUR*'" + sEmployeeCost + "')/100)+(TE_COMMLABOUR*'" + DecomLabourCost + "'))/100)*2) as FinalTotal,";
                    strQry += " 'No' as Unit,'1' as Quantity,(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST  WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) as SubDivision,";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) as Location,";
                    strQry += " EST_REPAIR AS TR_NAME, DF_GUARANTY_TYPE AS TR_GUARANTY";
                    strQry += " ,EST_NO,(1*TE_COMMLABOUR*'" + DecomLabourCost + "') LABOUR_COST,TO_CHAR(DF_ENHANCE_CAPACITY) DF_ENHANCE_CAPACITY, ";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SDO_USERNAME,";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A'  AND US_ID=(SELECT DF_CRBY FROM TBLDTCFAILURE WHERE DF_ID='" + sFailureId + "')) SO_USERNAME ";
                    strQry += " from  TBLDTCFAILURE,TBLITEMMASTER,TBLTCMASTER,TBLDTCMAST,TBLESTIMATION,TBLFEEDERMAST WHERE DF_DTC_CODE=DT_CODE AND ";
                    strQry += " DF_EQUIPMENT_ID=TC_CODE AND DF_ID='" + sFailureId + "' AND EST_DF_ID=DF_ID AND TC_CAPACITY=TE_CAPACITY AND NVL(TC_STAR_RATE,0)=NVL(TE_STAR_RATE,0)";
                    strQry += " AND FD_FEEDER_CODE=SUBSTR(DF_DTC_CODE,0,4)";
                }
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtDetailedReport.Load(dr);
                if (dtDetailedReport.Rows.Count > 0)
                {
                    if (String.IsNullOrEmpty(dtDetailedReport.Rows[0]["DTR_COMMISSION_DATE"].ToString()))
                    {
                        string df_id = dtDetailedReport.Rows[0]["DF_ID"].ToString();
                        string dtc_code = dtDetailedReport.Rows[0]["DF_DTC_CODE"].ToString();
                        strQry = " SELECT max(WO_WFO_ID) keep (dense_rank last ORDER BY wo_cr_on) AS WO_WFO_ID ";
                        strQry += " FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID='" + dtc_code + "' ";
                        strQry = " AND WO_RECORD_ID='" + df_id + "' AND WO_BO_ID='9' ";
                        string WFO_ID = ObjCon.get_value(strQry);
                        clsApproval objApproval = new clsApproval();
                        DataTable dtFailureDetails = new DataTable();
                        if (WFO_ID != "" && WFO_ID != null)
                        {
                            dtFailureDetails = objApproval.GetDatatableFromXML(WFO_ID);
                        }
                        if (dtFailureDetails.Rows.Count > 0)
                        {
                            if (dtFailureDetails.Columns.Contains("DTR_COMISSION_DATE"))
                            {
                                dtDetailedReport.Columns["DTR_COMMISSION_DATE"].ReadOnly = false;
                                dtDetailedReport.Rows[0]["DTR_COMMISSION_DATE"] = dtFailureDetails.Rows[0]["DTR_COMISSION_DATE"].ToString();
                            }
                            else
                            {
                                dtDetailedReport.Columns["DTR_COMMISSION_DATE"].ReadOnly = false;
                                dtDetailedReport.Rows[0]["DTR_COMMISSION_DATE"] = "";
                            }
                        }
                    }
                }
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetailedReport;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWoId"></param>
        /// <returns></returns>
        public DataTable PrintWorkOrderDetailsForNewDTC(string sWoId)
        {
            DataTable dtWODetails = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT WO_SLNO,WO_NO,TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE,TO_NUMBER(WO_AMT)WO_AMT,WO_ACC_CODE,WO_ISSUED_BY,WO_NEW_CAP,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CRBY=US_ID AND US_MMS_ID IS NULL AND US_STATUS='A') US_FULL_NAME,WO_REQUEST_LOC FROM TBLWORKORDER WHERE ";
                strQry += " WO_SLNO='" + sWoId + "' ";
                dtWODetails = ObjCon.getDataTable(strQry);
                return dtWODetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtWODetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFailureId"></param>
        /// <returns></returns>
        public DataTable PrintSurveyReport(string sFailureId)
        {
            DataTable dtDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT DISTINCT DF_EQUIPMENT_ID,TO_CHAR(DF_DATE,'DD/MM/YYYY')DF_DATE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,DF_ENHANCE_CAPACITY,TO_CHAR(TC_MANF_DATE,'DD/MM/YYYY')TC_MANF_DATE,SM_NAME,DF_LOC_CODE,TM_NAME,TC_SLNO,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) AS SUBDIVISION,";
                strQry += "(SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) AS LOCATION,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(DF_LOC_CODE,1,2)) AS DIVISION,EST_NO, ";
                strQry += " CASE DF_HT_BUSING  WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS DF_HT_BUSING,CASE DF_LT_BUSING  WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS DF_LT_BUSING,";
                strQry += "CASE DF_HT_BUSING_ROD  WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS DF_HT_BUSING_ROD,CASE DF_LT_BUSING_ROD  WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS DF_LT_BUSING_ROD,CASE DF_OIL_LEVEL  WHEN 1 THEN 'YES' WHEN 2 THEN 'NO' ELSE '' END AS DF_OIL_LEVEL,";
                strQry += " CASE DF_BREATHER  WHEN '1' THEN 'YES' WHEN '2' THEN 'NO' ELSE '' END AS DF_BREATHER,";
                strQry += "CASE DF_DRAIN_VALVE  WHEN 1 THEN 'YES' WHEN 2 THEN 'NO' ELSE '' END AS DF_DRAIN_VALVE,DF_OIL_QNTY,CASE DF_TANK_CONDITION WHEN 1 THEN 'GOOD' WHEN 2 THEN 'BAD' ELSE '' END AS DF_TANK_CONDITION,";
                strQry += "CASE DF_WHEEL WHEN 1 THEN 'YES' WHEN 2 THEN 'NO' ELSE '' END AS DF_WHEEL,CASE DF_EXPLOSION WHEN 1 THEN 'YES' WHEN 2 THEN 'NO' ELSE '' END AS DF_EXAMPLING, ";
                strQry += " EST_REPAIR AS TR_NAME, ";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SDO_USERNAME,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A'  AND US_ID=(SELECT DF_CRBY FROM TBLDTCFAILURE WHERE DF_ID='" + sFailureId + "')) SO_USERNAME,DF_CUSTOMER_NAME,DF_CUSTOMER_MOBILE,DF_NUMBER_OF_INSTALLATIONS ";
                strQry += " FROM TBLTCMASTER,TBLDTCFAILURE,TBLTRANSMAKES,TBLSTOREMAST,TBLESTIMATION WHERE TC_CODE=DF_EQUIPMENT_ID AND TM_ID=TC_MAKE_ID AND";
                strQry += "  TC_STORE_ID=SM_ID AND DF_EQUIPMENT_ID=TC_CODE AND DF_ID='" + sFailureId + "' AND EST_DF_ID=DF_ID ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtDetailedReport.Load(dr);
                return dtDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDetailedReport;
            }
        }
        #region New CR CODE
        /// <summary>
        /// Completion Report
        /// </summary>
        /// <param name="sDecommId"></param>
        /// <param name="sCRDate"></param>
        /// <returns></returns>
        public DataTable CompletionReport(string sDecommId, string sCRDate)
        {
            DataTable dtCompleteReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "  SELECT DISTINCT WO_NO,WO_NO_DECOM,TO_CHAR(WO_DATE,'DD-MM-YYYY') WO_DATE,TO_CHAR(WO_DTC_CAP)EST_FAULT_CAPACITY,DT_NAME,";
                strQry += " TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') AS TC_MANF_DATE,  TO_CHAR(IN_DATE,'DD-MON-YYYY') AS IN_DATE,TO_CHAR(TC_CODE)TC_CODE,TC_SLNO, CAST(DF_STATUS_FLAG AS VARCHAR(12)) DF_STATUS_FLAG ,";
                strQry += " CASE DF_FAILURE_TYPE WHEN 1 THEN 'FAILURE'  WHEN  2 THEN 'GOOD ENHANCEMENT'  WHEN 4 THEN 'FAILURE ENHANCEMENT'  END DF_FAILURE_TYPE , ";
                strQry += " TR_RV_NO ACK_NO, TO_CHAR(TR_RV_DATE,'DD-MON-YYYY') AS ACK_DATE,TO_CHAR(DF_DATE,'DD-MON-YYYY')DF_DATE,EST_NO,to_char(EST_CRON,'DD-MON-YYYY')EST_CRON,DF_DTC_CODE,";
                strQry += " TO_CHAR(WO_NEW_CAP)EST_REPLACE_CAPACITY,WO_DEVICE_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD-MON-YYYY')TI_INDENT_DATE,";
                strQry += " IN_MANUAL_INVNO AS IN_INV_NO,TO_CHAR(IN_DATE,'DD-MON-YYYY')IN_DATE,TR_RI_NO,TO_CHAR(TR_RI_DATE,'DD-MON-YYYY')TR_RI_DATE,";
                strQry += " TR_RV_NO,TO_CHAR(TR_RV_DATE,'DD-MON-YYYY')TR_RV_DATE,'" + sCRDate + "' TR_COMM_DATE,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(DF_LOC_CODE,1,2)) DIVISION,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) AS SUBDIVISION,";
                strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) AS SECTION,";
                strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) MAKE,TO_CHAR(DF_DTR_COMMISSION_DATE,'DD-MON-YYYY') AS DTR_COMMISSION_DATE,";
                strQry += "  WO_AMT AS EST_UNIT_PRICE,WO_AMT,TR_INVENTORY_QTY,TR_DECOM_INV_QTY,TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MON/YYYY')DTC_COMMISSION_DATE, ";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SDO_USERNAME,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME ";
                strQry += " FROM TBLTCMASTER,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCFAILURE,TBLDTCMAST,TBLTRANSDTCMAPPING,TBLESTIMATION WHERE  ";
                strQry += " TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND DF_ID=WO_DF_ID  AND DF_EQUIPMENT_ID = TC_CODE AND DT_CODE = DF_DTC_CODE ";
                strQry += " AND TM_DTC_ID=df_dtc_code AND EST_DF_ID=DF_ID AND TM_LIVE_FLAG='1' AND TR_ID='" + sDecommId + "'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtCompleteReport.Load(dr);
                return dtCompleteReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtCompleteReport;
            }
        }

        #endregion
        /// <summary>
        /// CR Details
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable CRDetails(clsReports objReport)
        {
            DataTable dtCompleteReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                //new one CR from main table 
                strQry = "  SELECT DISTINCT WO_NO,WO_NO_DECOM,DF_DTC_CODE,TO_CHAR(WO_DATE,'DD/MM/YYYY') WO_DATE,TO_CHAR(WO_DTC_CAP)EST_FAULT_CAPACITY,DT_NAME,";
                strQry += " TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') AS TC_MANF_DATE,  TO_CHAR(IN_DATE,'DD-MON-YYYY') AS IN_DATE,TO_CHAR(TC_CODE)TC_CODE,TC_SLNO, ";
                strQry += " CASE DF_FAILURE_TYPE WHEN 1 THEN 'FAILURE'  WHEN  2 THEN 'GOOD ENHANCEMENT'  WHEN 4 THEN 'FAILURE ENHANCEMENT'  END DF_FAILURE_TYPE , ";
                strQry += " TR_RV_NO ACK_NO, TO_CHAR(TR_RV_DATE,'DD-MON-YYYY') AS ACK_DATE,TO_CHAR(DF_DATE,'DD/MON/YYYY')DF_DATE,TO_CHAR(DF_DTR_COMMISSION_DATE,'DD/MON/YYYY') AS DTR_COMMISSION_DATE,";
                strQry += " TO_CHAR(WO_NEW_CAP)EST_REPLACE_CAPACITY,WO_DEVICE_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD/MON/YYYY')TI_INDENT_DATE,TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MON/YYYY')DTC_COMMISSION_DATE,";
                strQry += " IN_MANUAL_INVNO AS IN_INV_NO,TO_CHAR(IN_DATE,'DD/MON/YYYY')IN_DATE,TR_RI_NO,TO_CHAR(TR_RI_DATE,'DD/MON/YYYY')TR_RI_DATE,";
                strQry += " TR_RV_NO,TO_CHAR(TR_RV_DATE,'DD/MON/YYYY')TR_RV_DATE,TO_CHAR(TR_CR_DATE,'DD/MON/YYYY')TR_COMM_DATE,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(DF_LOC_CODE,1,2)) DIVISION,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) AS SUBDIVISION,";
                strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) AS SECTION,";
                strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) MAKE,TO_CHAR( EST_CRON,'DD-MON-YYYY')EST_CRON,";
                strQry += "  WO_AMT AS EST_UNIT_PRICE,WO_AMT,TR_INVENTORY_QTY,TR_DECOM_INV_QTY,TO_CHAR(EST_NO)EST_NO, ";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SDO_USERNAME,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME ";
                strQry += " FROM TBLTCMASTER,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCFAILURE,TBLDTCMAST,TBLESTIMATION WHERE DF_ID=EST_DF_ID AND  ";
                strQry += " TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND DF_ID=WO_DF_ID  AND DF_EQUIPMENT_ID = TC_CODE AND DT_CODE = DF_DTC_CODE AND DF_ID='" + Genaral.Decrypt(objReport.sFailId) + "'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtCompleteReport.Load(dr);
                return dtCompleteReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtCompleteReport;
            }
        }
        /// <summary>
        /// DTC Count Details
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable DTCCountDetails(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            string offCode = string.Empty;
            try
            {
                if (objReport.sOfficeCode == null || objReport.sOfficeCode == "")
                {
                    offCode = "";
                }
                else
                {
                    offCode = objReport.sOfficeCode;
                }
                strQry = "select nvl(TC_CAPACITY, 0 ) as TC_CAPACITY  , case when D.DCR_RANGE > 500 then  '>500' else D.DCR_RANGE end  DCR_RANEGE , ";
                strQry += "    FEEDER_NAME as FEEDER, FT_NAME  , (SELECT CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME   FROM TBLCIRCLE  ";
                strQry += "    WHERE CM_CIRCLE_CODE = SUBSTR(OFF_CODE, 0, 1)) CIRCLE ,  (SELECT DIV_CODE || '-' || DIV_NAME   FROM TBLDIVISION  ";
                strQry += "    WHERE DIV_CODE = SUBSTR(OFF_CODE, 0, 2)) DIVISION  ,    (SELECT SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME  FROM ";
                strQry += "   TBLSUBDIVMAST WHERE SD_SUBDIV_CODE = SUBSTR(OFF_CODE, 0, 3))  SUBDIVISION ,  (SELECT OM_CODE || '-' || OM_NAME ";
                strQry += "    FROM TBLOMSECMAST  WHERE OM_CODE = OFF_CODE)  SECTION from  (SELECT   count(TC_CAPACITY) TC_CAPACITY, MD_NAME, ";
                strQry += "     OFF_CODE, DCR_RANGE, FEEDER_NAME, FT_NAME    from(SELECT MD_NAME, OFF_CODE, DCR_RANGE  FROM TBLMASTERDATA, ";
                strQry += "    VIEW_ALL_OFFICES, TBLDTRCAPACITYRANGE   WHERE MD_TYPE = 'C' AND   LENGTH(OFF_CODE) = 4  AND MD_NAME = DCR_CAPACITY ";
                strQry += "   and OFF_CODE like '" + offCode + "%') A   left join(SELECT TC_CAPACITY, DT_OM_SLNO, DT_FDRSLNO, FD_FEEDER_CODE || '-' || FD_FEEDER_NAME ";
                strQry += "    as FEEDER_NAME, FT_NAME  FROM TBLDTCMAST, TBLTCMASTER, TBLFEEDERMAST, TBLFDRTYPE WHERE ";
                strQry += "    FT_ID = FD_FEEDER_TYPE AND DT_FDRSLNO = FD_FEEDER_CODE  and  DT_TC_ID = TC_CODE and DT_OM_SLNO LIKE '" + offCode + "%' and ";
                strQry += "    FD_FEEDER_CODE like '" + objReport.sFeeder + "%' and    TO_DATE(TO_CHAR(DT_CRON, 'yyyy/MM/dd')) >= TO_DATE('" + objReport.sFromDate + "', 'yyyy/MM/dd')  and ";
                strQry += "     TO_DATE(TO_CHAR(DT_CRON, 'yyyy/MM/dd')) <= to_date('" + objReport.sTodate + "', 'yyyy/MM/dd')) B   on B.TC_CAPACITY = A.MD_NAME and ";
                strQry += "    A.OFF_CODE = B.DT_OM_SLNO WHERE FEEDER_NAME is not null GROUP BY  MD_NAME, OFF_CODE, DCR_RANGE, FEEDER_NAME, ";
                strQry += "     FT_NAME ) C right join(SELECT distinct DCR_RANGE FROM TBLDTRCAPACITYRANGE) D on C.DCR_RANGE = D.DCR_RANGE ";
                strQry += "   ORDER BY OFF_CODE ,  feeder_name ";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIndentId"></param>
        /// <returns></returns>
        public DataTable IndentDetails(string strIndentId)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy') TI_INDENT_DATE,TO_CHAR(TC_CAPACITY) TC_CAPACITY,WO_NEW_CAP,";
                strQry += " TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE, (select SM_NAME from TBLSTOREMAST WHERE SM_ID=TI_STORE_ID) SM_NAME,";
                strQry += " (SELECT EST_UNIT_PRICE FROM TBLESTIMATION WHERE EST_DF_ID=WO_DF_ID) EST_UNIT_PRICE, WO_NO,WO_ACC_CODE,WO_AMT,";
                strQry += "  DF_DTC_CODE,cast(DF_STATUS_FLAG as varchar(10))DF_STATUS_FLAG, (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE) DT_NAME,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) AS SUBDIVISION,";
                strQry += "(SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) AS SECTION,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(DF_LOC_CODE,1,2)) AS DIVISION,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME ";
                strQry += " ,(SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME,DF_CUSTOMER_NAME,DF_CUSTOMER_MOBILE ,DF_NUMBER_OF_INSTALLATIONS FROM TBLTCMASTER,";
                strQry += " TBLWORKORDER,TBLINDENT,TBLDTCFAILURE where  TI_WO_SLNO=WO_SLNO and TI_ID='" + strIndentId + "' AND DF_ID=WO_DF_ID AND TC_CODE=DF_EQUIPMENT_ID ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtIndentDetails.Load(dr);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtIndentDetails;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sInvoiceId"></param>
        /// <param name="sOfficeCode"></param>
        /// <param name="sCapacity"></param>
        /// <returns></returns>
        public DataTable InvoiceReport(string sInvoiceId, string sOfficeCode, string sCapacity)
        {
            DataTable dtIndentDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                if (sOfficeCode.Length > 1)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 2);
                }
                strQry = "SELECT TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'dd/MM/yyyy') TI_INDENT_DATE,TO_CHAR(WO_DTC_CAP) TC_CAPACITY,WO_NEW_CAP,";
                strQry += " TO_CHAR(WO_DATE,'dd/MM/yyyy') WO_DATE, (select SM_NAME from TBLSTOREMAST WHERE SM_ID=TI_STORE_ID) SM_NAME,";
                strQry += " (SELECT EST_UNIT_PRICE FROM TBLESTIMATION WHERE EST_DF_ID=WO_DF_ID) EST_UNIT_PRICE, WO_NO,WO_ACC_CODE,WO_AMT,";
                strQry += "  DF_DTC_CODE, (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE) DT_NAME,IN_MANUAL_INVNO,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) AS SUBDIVISION,";
                strQry += "(SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) AS SECTION,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(DF_LOC_CODE,1,2)) AS DIVISION,";
                strQry += " IN_INV_NO,TO_CHAR(IN_DATE,'DD/MM/YYYY') IN_DATE,IN_AMT, ";
                strQry += " (SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_STORE_ID IN (SELECT SM_ID FROM TBLSTOREMAST WHERE  SM_OFF_CODE='" + sOfficeCode + "') AND TC_STATUS IN (1,2) ";
                strQry += " AND TC_CURRENT_LOCATION=1 AND TC_CAPACITY='" + sCapacity + "') STOCK_COUNT,";
                strQry += " TO_CHAR(TC_CODE) TC_CODE,TO_CHAR(TC_SLNO) TC_SLNO,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,2) AND US_ROLE_ID='2' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME";
                strQry += " FROM TBLTCMASTER,";
                strQry += " TBLWORKORDER,TBLINDENT,TBLDTCFAILURE,TBLDTCINVOICE,TBLTCDRAWN where  TI_WO_SLNO=WO_SLNO and TI_ID=IN_TI_NO ";
                strQry += " AND IN_NO='" + sInvoiceId + "' AND DF_ID=WO_DF_ID AND TD_TC_NO=TC_CODE AND TD_INV_NO=IN_NO ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtIndentDetails.Load(dr);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtIndentDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDecommId"></param>
        /// <returns></returns>
        public DataTable RIReport(string sDecommId)
        {
            DataTable dtRiDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                // case when TR_RV_NO is null then TR_OIL_QUNTY(DTLMS) else TR_OIL_QTY_BYSK(MMS) end as TR_OIL_QUNTY
                // if rv is done in mms then i am showing the mms  rv  oil quantity  else  dtlms oil quantity  .
                strQry = "select  TR_RI_NO,TO_CHAR(TR_RI_DATE,'DD-MON-YYYY')TR_RI_DATE,TO_CHAR(DF_DATE,'DD-MON-YYYY')DF_DATE, CASE  DF_STATUS_FLAG  WHEN 1 THEN 'FAILURE'  WHEN 4 THEN 'FAIL AND EHNC' WHEN 2 THEN 'GOOD EHNC' WHEN 5 THEN 'GOOD REDUCTION' END  AS STATUS , DF_GUARANTY_TYPE , ";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(DF_LOC_CODE,1,2)) DIVISION, ";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE =  SUBSTR(DF_LOC_CODE,1,3)) SUBDIVISION,";
                strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE = DF_LOC_CODE) SECTION,TR_MANUAL_ACKRV_NO,";
                strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID) MAKE, TC_SLNO, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') AS TC_MANF_DATE,TC_CODE,case when TR_RV_NO is null then TR_OIL_QUNTY else TR_OIL_QTY_BYSK end as TR_OIL_QUNTY,";
                strQry += " (SELECT TO_CHAR(TM_MAPPING_DATE,'DD-MON-YY') FROM TBLTRANSDTCMAPPING WHERE TM_ID = ";
                strQry += " (SELECT MAX(TM_ID) FROM TBLTRANSDTCMAPPING WHERE TM_TC_ID = TC_CODE)) AS DTRCOMMISIONDATE,";
                strQry += " TO_CHAR(TC_CAPACITY)TC_CAPACITY, (select SM_NAME from TBLSTOREMAST where SM_ID=TR_STORE_SLNO)SM_NAME,";
                strQry += " (SELECT WO_NO_DECOM FROM TBLWORKORDER WHERE WO_DF_ID=TD_DF_ID) WO_NO_DECOM,(SELECT TO_CHAR(WO_DATE,'DD-MON-YYYY') FROM TBLWORKORDER WHERE WO_DF_ID=TD_DF_ID)WO_DATE ,DF_DTC_CODE,";
                strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE) DTC_NAME,TR_OIL_QTY_BYSK,TR_RV_NO ACK_NO, TO_CHAR(TR_RV_DATE,'DD-MON-YYYY') AS ACK_DATE, ";
                strQry += " (SELECT EST_UNIT_PRICE FROM TBLESTIMATION WHERE EST_DF_ID=DF_ID) EST_UNIT_PRICE,";
                strQry += " (SELECT EST_NO FROM TBLESTIMATION WHERE EST_DF_ID=DF_ID) EST_NO, ";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SDO_USERNAME,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME, ";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,2) AND US_ROLE_ID='2' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,2) AND US_ROLE_ID='5' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SK_USERNAME";
                strQry += " FROM TBLTCMASTER,TBLTCREPLACE,TBLTCDRAWN,TBLDTCFAILURE WHERE ";
                strQry += " DF_EQUIPMENT_ID=TC_CODE AND TR_IN_NO=TD_INV_NO AND  TR_ID='" + sDecommId + "' AND  DF_ID=TD_DF_ID";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtRiDetails.Load(dr);
                return dtRiDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtRiDetails;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sInvoiceId"></param>
        /// <returns></returns>
        public DataTable LoadGatePass(string sInvoiceId)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT GP_VEHICLE_NO,GP_RECIEPIENT_NAME,GP_CHALLEN_NO,IN_INV_NO,TC_SLNO, TO_CHAR(IN_DATE,'DD-MON-YYYY') IN_DATE ,";
                strQry += " (select TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID )TM_NAME,TC_CODE,TO_CHAR(TC_CAPACITY) TC_CAPACITY, ";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(TC_LOCATION_ID,1,2) AND US_ROLE_ID='2' ";
                strQry += " AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME FROM TBLDTCINVOICE,TBLGATEPASS, ";
                strQry += " TBLTCDRAWN,TBLTCMASTER WHERE IN_INV_NO=GP_IN_NO AND IN_NO=TD_INV_NO AND TC_CODE=TD_TC_NO AND GP_IN_NO='" + sInvoiceId + "' ";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {

                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        #region cregisterabstractreport
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objreports"></param>
        /// <returns></returns>
        public DataTable PrintRegAbstact(clsReports objreports)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                #region Old CR Report
                //strQry = "SELECT (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES ";
                //strQry += " WHERE OFF_CODE=SUBSTR (DF_LOC_CODE,0,1))CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)";
                //strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(DF_LOC_CODE,0,2))DIVISION,";
                //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE ";
                //strQry += " OFF_CODE=SUBSTR(DF_LOC_CODE,0,3))SUBDIVISION, '" + objreports.sFromDate + "' as FROMDATE,'" + objreports.sTodate + "' as TODATE,TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate, ";
                //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)FROM VIEW_ALL_OFFICES WHERE";
                //strQry += " OFF_CODE=SUBSTR(DF_LOC_CODE,0,4))SECTION,(CASE WHEN TR_INVENTORY_QTY IS NULL THEN 'Crpending' WHEN TR_INVENTORY_QTY IS NOT NULL THEN 'Crcompleted' END)STATUS";
                //strQry += ",EST_NO,TO_CHAR( EST_CRON,'DD-MON-YYYY')EST_CRON ,WO_NO,WO_NO_DECOM ,";
                //strQry += " TO_CHAR( WO_DATE,'DD-MON-YYYY')WO_DATE,TO_CHAR( WO_DATE_DECOM,'DD-MON-YYYY')WO_DATE_DECOM,";
                //strQry += "  TO_CHAR(TC_CAPACITY)TC_CAPACITY,DF_REASON,IN_INV_NO,TO_CHAR( IN_DATE,'DD-MON-YYYY')IN_DATE,CASE WHEN DF_ENHANCE_CAPACITY IS NULL THEN TC_CAPACITY WHEN DF_ENHANCE_CAPACITY IS NOT NULL THEN DF_ENHANCE_CAPACITY END AS REPLACE_CAPACITY  ";
                //strQry += " FROM TBLTCMASTER INNER JOIN TBLDTCFAILURE ON DF_EQUIPMENT_ID=TC_CODE INNER  JOIN  TBLMASTERDATA  on DF_FAILURE_TYPE=MD_ID LEFT JOIN ";
                //strQry += " TBLESTIMATION ON DF_ID=EST_DF_ID LEFT JOIN TBLWORKORDER ON DF_ID=WO_DF_ID LEFT JOIN TBLINDENT";
                //strQry += " ON WO_SLNO=TI_WO_SLNO LEFT JOIN TBLDTCINVOICE ON IN_TI_NO=TI_ID LEFT JOIN ";
                //strQry += " TBLTCREPLACE ON TR_IN_NO=IN_NO WHERE  DF_LOC_CODE LIKE '" + objreports.sOfficeCode + "%' ";
                //strQry += " AND DF_STATUS_FLAG IN (1,4)  AND MD_TYPE='FT' AND  TR_RI_NO IS NOT NULL  AND ";
                //if (objreports.sTodate == null && (objreports.sFromDate != null))
                //{
                //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objreports.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";

                //}
                //if (objreports.sFromDate == null && (objreports.sTodate != null))
                //{
                //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objreports.sTodate + "' ";
                //}
                //if (objreports.sFromDate == null && objreports.sTodate == null)
                //{
                //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                //}
                //if (objreports.sFromDate != null && objreports.sTodate != null)
                //{
                //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objreports.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objreports.sTodate + "'  ";
                //}

                //strQry += "ORDER BY est_no";
                #endregion
                strQry = "SELECT (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES  WHERE OFF_CODE=SUBSTR (DF_LOC_CODE,0,1))CIRCLE,";
                strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(DF_LOC_CODE,0,2))DIVISION, ";
                strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE  OFF_CODE=SUBSTR(DF_LOC_CODE,0,3))SUBDIVISION, ";
                strQry += "'" + objreports.sFromDate + "' as FROMDATE,'" + objreports.sTodate + "' as TODATE,TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate,";
                strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(DF_LOC_CODE,0,4))SECTION,";
                strQry += "(CASE WHEN TR_INVENTORY_QTY IS NULL THEN 'Crpending' WHEN TR_INVENTORY_QTY IS NOT NULL THEN 'Crcompleted' END)STATUS,";
                strQry += "EST_NO,TO_CHAR( EST_CRON,'DD-MON-YYYY')EST_CRON ,WO_NO,WO_NO_DECOM , TO_CHAR( WO_DATE,'DD-MON-YYYY')WO_DATE,TO_CHAR";
                strQry += "( WO_DATE_DECOM,'DD-MON-YYYY')WO_DATE_DECOM,  TO_CHAR(TC_CAPACITY)TC_CAPACITY,DF_REASON,IN_INV_NO,";
                strQry += "TO_CHAR( IN_DATE,'DD-MON-YYYY')IN_DATE,CASE WHEN DF_ENHANCE_CAPACITY IS NULL THEN TC_CAPACITY WHEN DF_ENHANCE_CAPACITY ";
                strQry += "IS NOT NULL THEN DF_ENHANCE_CAPACITY END AS REPLACE_CAPACITY   FROM TBLTCMASTER,TBLDTCFAILURE,TBLMASTERDATA,TBLWORKORDER,";
                strQry += "TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCMAST,TBLESTIMATION where DF_ID=EST_DF_ID and DF_EQUIPMENT_ID=TC_CODE and ";
                strQry += "DF_DTC_CODE =DT_CODE and DF_REPLACE_FLAG=1 AND  TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') AND DF_LOC_CODE ";
                strQry += "LIKE '" + objreports.sOfficeCode + "%' AND DF_FAILURE_TYPE=MD_ID AND DF_ID=WO_DF_ID AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO ";
                strQry += "AND IN_NO=TR_IN_NO AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (1,4)";
                if (objreports.sTodate == null && (objreports.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objreports.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                }
                if (objreports.sFromDate == null && (objreports.sTodate != null))
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objreports.sTodate + "' ";
                }
                if (objreports.sFromDate == null && objreports.sTodate == null)
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objreports.sFromDate != null && objreports.sTodate != null)
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objreports.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objreports.sTodate + "'  ";
                }
                strQry += " UNION ALL ";
                strQry += "SELECT (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES  WHERE OFF_CODE=SUBSTR (DF_LOC_CODE,0,1))CIRCLE,";
                strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(DF_LOC_CODE,0,2))DIVISION, ";
                strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE  OFF_CODE=SUBSTR(DF_LOC_CODE,0,3))SUBDIVISION, ";
                strQry += "'" + objreports.sFromDate + "' as FROMDATE,'" + objreports.sTodate + "' as TODATE,TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate,";
                strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(DF_LOC_CODE,0,4))SECTION,";
                strQry += "(CASE WHEN TR_INVENTORY_QTY IS NULL THEN 'Crpending' WHEN TR_INVENTORY_QTY IS NOT NULL THEN 'Crcompleted' END)STATUS,";
                strQry += "EST_NO,TO_CHAR( EST_CRON,'DD-MON-YYYY')EST_CRON ,WO_NO,WO_NO_DECOM , TO_CHAR( WO_DATE,'DD-MON-YYYY')WO_DATE,TO_CHAR";
                strQry += "( WO_DATE_DECOM,'DD-MON-YYYY')WO_DATE_DECOM,  TO_CHAR(TC_CAPACITY)TC_CAPACITY,DF_REASON,IN_INV_NO,";
                strQry += "TO_CHAR( IN_DATE,'DD-MON-YYYY')IN_DATE,CASE WHEN DF_ENHANCE_CAPACITY IS NULL THEN TC_CAPACITY WHEN DF_ENHANCE_CAPACITY ";
                strQry += "IS NOT NULL THEN DF_ENHANCE_CAPACITY END AS REPLACE_CAPACITY   FROM TBLTCMASTER,TBLDTCFAILURE,TBLMASTERDATA,TBLWORKORDER,";
                strQry += "TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCMAST,TBLESTIMATION where DF_ID=EST_DF_ID and DF_EQUIPMENT_ID=TC_CODE and ";
                strQry += "DF_DTC_CODE =DT_CODE and DF_REPLACE_FLAG=0 AND  TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') AND DF_LOC_CODE ";
                strQry += "LIKE '" + objreports.sOfficeCode + "%' AND DF_FAILURE_TYPE=MD_ID AND DF_ID=WO_DF_ID(+) AND WO_SLNO=TI_WO_SLNO(+) AND TI_ID=IN_TI_NO(+) ";
                strQry += "AND IN_NO=TR_IN_NO(+) AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (1,4)";
                if (objreports.sTodate == null && (objreports.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objreports.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                }
                if (objreports.sFromDate == null && (objreports.sTodate != null))
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objreports.sTodate + "' ";
                }
                if (objreports.sFromDate == null && objreports.sTodate == null)
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objreports.sFromDate != null && objreports.sTodate != null)
                {
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objreports.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objreports.sTodate + "'  ";
                }
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <param name="dtccode"></param>
        /// <param name="oldDtccode"></param>
        /// <returns></returns>
        public DataTable CRDetails(clsReports objReport, string dtccode = "", string oldDtccode = "")
        {
            DataTable dtCompleteReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                if (dtccode.Length != 0 && oldDtccode.Length != 0)
                {
                    //new one CR from main table 
                    strQry = "  SELECT DISTINCT WO_NO,WO_NO_DECOM,DF_DTC_CODE,cast(DF_STATUS_FLAG as varchar(10))DF_STATUS_FLAG ,TO_CHAR(WO_DATE,'DD/MM/YYYY') WO_DATE,TO_CHAR(WO_DTC_CAP)EST_FAULT_CAPACITY,DT_NAME,";
                    strQry += " TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') AS TC_MANF_DATE,  TO_CHAR(IN_DATE,'DD-MON-YYYY') AS IN_DATE,TO_CHAR(TC_CODE)TC_CODE,TC_SLNO, ";
                    strQry += " CASE DF_FAILURE_TYPE WHEN 1 THEN 'FAILURE'  WHEN  2 THEN 'GOOD ENHANCEMENT'  WHEN 4 THEN 'FAILURE ENHANCEMENT'  END DF_FAILURE_TYPE , ";
                    strQry += " TR_RV_NO ACK_NO, TO_CHAR(TR_RV_DATE,'DD-MON-YYYY') AS ACK_DATE,TO_CHAR(DF_DATE,'DD/MON/YYYY')DF_DATE,TO_CHAR(DF_DTR_COMMISSION_DATE,'DD/MON/YYYY') AS DTR_COMMISSION_DATE,";
                    strQry += " TO_CHAR(WO_NEW_CAP)EST_REPLACE_CAPACITY,WO_DEVICE_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD/MON/YYYY')TI_INDENT_DATE,TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MON/YYYY')DTC_COMMISSION_DATE,";
                    strQry += " IN_MANUAL_INVNO AS IN_INV_NO,TO_CHAR(IN_DATE,'DD/MON/YYYY')IN_DATE,TR_RI_NO,TO_CHAR(TR_RI_DATE,'DD/MON/YYYY')TR_RI_DATE,";
                    strQry += " TR_RV_NO,TO_CHAR(TR_RV_DATE,'DD/MON/YYYY')TR_RV_DATE,TO_CHAR(TR_CR_DATE,'DD/MON/YYYY')TR_COMM_DATE,";
                    strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(DF_LOC_CODE,1,2)) DIVISION,";
                    strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) AS SUBDIVISION,";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) AS SECTION,";
                    strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) MAKE,TO_CHAR( EST_CRON,'DD-MON-YYYY')EST_CRON,";
                    strQry += "  WO_AMT AS EST_UNIT_PRICE,WO_AMT,TR_INVENTORY_QTY,TR_DECOM_INV_QTY,TO_CHAR(EST_NO)EST_NO, ";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SDO_USERNAME,";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME ";
                    strQry += " FROM TBLTCMASTER,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCFAILURE,TBLDTCMAST,TBLESTIMATION WHERE DF_ID=EST_DF_ID AND  ";
                    strQry += " TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND DF_ID=WO_DF_ID  AND DF_EQUIPMENT_ID = TC_CODE AND DT_CODE in('" + dtccode + "','" + oldDtccode + "' )   AND DF_ID='" + Genaral.Decrypt(objReport.sFailId) + "'";
                }
                else
                {
                    //new one CR from main table 
                    strQry = "  SELECT DISTINCT WO_NO,WO_NO_DECOM,DF_DTC_CODE,cast(DF_STATUS_FLAG as varchar(10))DF_STATUS_FLAG,TO_CHAR(WO_DATE,'DD/MM/YYYY') WO_DATE,TO_CHAR(WO_DTC_CAP)EST_FAULT_CAPACITY,DT_NAME,";
                    strQry += " TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY') AS TC_MANF_DATE,  TO_CHAR(IN_DATE,'DD-MON-YYYY') AS IN_DATE,TO_CHAR(TC_CODE)TC_CODE,TC_SLNO, ";
                    strQry += " CASE DF_FAILURE_TYPE WHEN 1 THEN 'FAILURE'  WHEN  2 THEN 'GOOD ENHANCEMENT'  WHEN 4 THEN 'FAILURE ENHANCEMENT'  END DF_FAILURE_TYPE , ";
                    strQry += " TR_RV_NO ACK_NO, TO_CHAR(TR_RV_DATE,'DD-MON-YYYY') AS ACK_DATE,TO_CHAR(DF_DATE,'DD/MON/YYYY')DF_DATE,TO_CHAR(DF_DTR_COMMISSION_DATE,'DD/MON/YYYY') AS DTR_COMMISSION_DATE,";
                    strQry += " TO_CHAR(WO_NEW_CAP)EST_REPLACE_CAPACITY,WO_DEVICE_ID,TI_INDENT_NO,TO_CHAR(TI_INDENT_DATE,'DD/MON/YYYY')TI_INDENT_DATE,TO_CHAR(DT_TRANS_COMMISION_DATE,'DD/MON/YYYY')DTC_COMMISSION_DATE,";
                    strQry += " IN_MANUAL_INVNO AS IN_INV_NO,TO_CHAR(IN_DATE,'DD/MON/YYYY')IN_DATE,TR_RI_NO,TO_CHAR(TR_RI_DATE,'DD/MON/YYYY')TR_RI_DATE,";
                    strQry += " TR_RV_NO,TO_CHAR(TR_RV_DATE,'DD/MON/YYYY')TR_RV_DATE,TO_CHAR(TR_CR_DATE,'DD/MON/YYYY')TR_COMM_DATE,";
                    strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(DF_LOC_CODE,1,2)) DIVISION,";
                    strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DF_LOC_CODE,1,3)) AS SUBDIVISION,";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE=SUBSTR(DF_LOC_CODE,1,4)) AS SECTION,";
                    strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) MAKE,TO_CHAR( EST_CRON,'DD-MON-YYYY')EST_CRON,";
                    strQry += "  WO_AMT AS EST_UNIT_PRICE,WO_AMT,TR_INVENTORY_QTY,TR_DECOM_INV_QTY,TO_CHAR(EST_NO)EST_NO, ";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(DF_LOC_CODE,1,3) AND US_ROLE_ID='1' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SDO_USERNAME,";
                    strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=DF_LOC_CODE AND US_ROLE_ID='4' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') SO_USERNAME ";
                    strQry += " FROM TBLTCMASTER,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCFAILURE,TBLDTCMAST,TBLESTIMATION WHERE DF_ID=EST_DF_ID AND  ";
                    strQry += " TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND DF_ID=WO_DF_ID  AND DF_EQUIPMENT_ID = TC_CODE AND DT_CODE = DF_DTC_CODE AND DF_ID='" + Genaral.Decrypt(objReport.sFailId) + "'";
                }
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtCompleteReport.Load(dr);
                return dtCompleteReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtCompleteReport;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sInvoiceNo"></param>
        /// <returns></returns>
        public DataTable PrintRepairGatePassReport(string sInvoiceNo)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "Select TO_CHAR(RSD_TC_CODE)RSD_TC_CODE,RSM_INV_NO,TO_CHAR(RSM_INV_DATE,'dd/MM/yyyy')RSM_INV_DATE,RSD_DELIVARY_DATE,GP_VEHICLE_NO,GP_RECIEPIENT_NAME,GP_CHALLEN_NO,";
                strQry += "TC_SLNO,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TM_NAME,DIV_NAME,TO_CHAR(TC_MANF_DATE,'dd/MM/yyyy')TC_MANF_DATE,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=RSM_DIV_CODE AND US_ROLE_ID='2' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME ";
                strQry += " FROM TBLGATEPASS,TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS,TBLTCMASTER,TBLTRANSMAKES,TBLDIVISION";
                strQry += " WHERE RSM_DIV_CODE=DIV_CODE and TM_ID=TC_MAKE_ID AND TC_CODE=RSD_TC_CODE AND RSD_RSM_ID=RSM_ID and RSM_INV_NO=GP_IN_NO and GP_IN_NO='" + sInvoiceNo + "' ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sInvoiceNo"></param>
        /// <returns></returns>
        public DataTable PrintScrapGatePass(string sInvoiceNo)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " select TC_SLNO,(select TM_NAME from TBLTRANSMAKES where TM_ID=TC_MAKE_ID)TM_NAME,TO_CHAR(TC_CODE) as RSD_TC_CODE,DIV_NAME,";
                strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(TC_MANF_DATE,'dd/MM/yyyy')TC_MANF_DATE,SO_INV_NO as RSM_INV_NO,GP_VEHICLE_NO,";
                strQry += " GP_CHALLEN_NO,GP_RECIEPIENT_NAME,To_char(SO_INV_DATE,'dd/MM/yyyy') as RSM_INV_DATE,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=ST_DIV_CODE AND US_ROLE_ID='2' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME ";
                strQry += " FROM TBLSCRAPTC,TBLSCRAPOBJECT,TBLTCMASTER,TBLGATEPASS,TBLDIVISION";
                strQry += " where SO_ST_ID=ST_ID and TC_CODE=SO_TC_CODE and SO_INV_NO=GP_IN_NO and ST_DIV_CODE=DIV_CODE and GP_IN_NO='" + sInvoiceNo + "' ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sInvoiceNo"></param>
        /// <returns></returns>
        public DataTable PrintStoreInvoiceGatePass(string sInvoiceNo)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT DISTINCT TC_SLNO,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TM_NAME,TO_CHAR(TC_CODE) AS RSD_TC_CODE, ";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION,TBLSTOREMAST WHERE DIV_CODE=SM_OFF_CODE AND SM_ID=SI_TO_STORE) DIV_NAME,";
                strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY,TO_CHAR(TC_MANF_DATE,'DD/MM/YYYY')TC_MANF_DATE,IS_NO AS RSM_INV_NO,GP_VEHICLE_NO,GP_CHALLEN_NO,";
                strQry += " GP_RECIEPIENT_NAME,TO_CHAR(IS_DATE,'DD/MM/YYYY') AS RSM_INV_DATE,IS_NO,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=ST_DIV_CODE AND US_ROLE_ID='5' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME,";
                strQry += " GP_IN_NO FROM TBLSTOREINVOICE,TBLSINVOICEOBJECTS,TBLTCMASTER,TBLGATEPASS,TBLSTOREINDENT,TBLSCRAPTC WHERE ";
                strQry += " IO_IS_ID=IS_ID AND TC_CODE=IO_TCCODE AND IS_NO=GP_IN_NO AND IS_SI_ID=SI_ID AND GP_IN_NO='" + sInvoiceNo + "'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// Print Scrap Invoice report
        /// </summary>
        /// <param name="sScrapId"></param>
        /// <returns></returns>
        public DataTable PrintScrapInvoicereport(string sScrapId)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "select ST_OM_NO ST_WO_NO,To_CHAR(ST_OM_DATE,'dd/MM/yyyy')ST_WO_DATE,TO_CHAR(TC_CODE) TC_CODE,SO_INV_NO,To_char(SO_INV_DATE,'dd/MM/yyyy')SO_INV_DATE,";
                strQry += " (select SM_NAME from TBLSTOREMAST where SM_ID=TC_STORE_ID)SM_NAME,";
                strQry += " (select TM_NAME from TBLTRANSMAKES where TC_MAKE_ID=TM_ID) TC_MAKE,TC_SLNO,TO_CHAR(TC_CAPACITY)TC_CAPACITY,";
                strQry += " TO_CHAR(TC_MANF_DATE,'dd/MM/yyyy') TC_MANF_DATE,DIV_NAME, ";
                strQry += " (SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_STORE_ID IN ";
                strQry += " (SELECT SM_ID FROM TBLSTOREMAST WHERE  SM_OFF_CODE=ST_DIV_CODE) AND TC_STATUS IN (1,2) ";
                strQry += "  AND TC_CURRENT_LOCATION=1 ) STOCK_COUNT,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=ST_DIV_CODE AND US_ROLE_ID='2' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME ";
                strQry += " from TBLSCRAPOBJECT,TBLSCRAPTC,TBLTCMASTER,TBLDIVISION where ST_ID=SO_ST_ID and TC_CODE=SO_TC_CODE ";
                strQry += " AND ST_ID='" + sScrapId + "' AND  ST_DIV_CODE=DIV_CODE";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }


        public DataTable PrintInterStoreInvoicereport(string sInvoiceNo)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                strQry = "SELECT TO_CHAR(TC_CODE) AS TC_CODE,TC_SLNO,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TM_NAME,";
                strQry += " TO_CHAR(TC_CAPACITY) TC_CAPACITY, ";
                strQry += " TO_CHAR(TC_MANF_DATE,'DD/MM/YYYY') MANF_DATE,TO_CHAR(IS_DATE,'DD/MM/YYYY') AS INV_DATE,IS_NO AS INV_NO,SI_NO AS INDENT_NO,TO_CHAR(SI_DATE,'DD/MM/YYYY') INDENT_DATE,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION,TBLSTOREMAST WHERE DIV_CODE=SM_OFF_CODE AND SM_ID=SI_TO_STORE) DIV_NAME,";
                strQry += " (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE) SM_NAME,";
                strQry += " (SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_STORE_ID IN ";
                strQry += " (SELECT SM_ID FROM TBLSTOREMAST WHERE  SM_ID=SI_TO_STORE) AND TC_STATUS IN (1,2) AND TC_CURRENT_LOCATION=1 ) STOCK_COUNT,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(TC_LOCATION_ID,1,2) AND US_ROLE_ID='2' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME ";
                strQry += " FROM TBLSTOREINVOICE,TBLSINVOICEOBJECTS,TBLTCMASTER,TBLSTOREINDENT WHERE ";
                strQry += " IO_IS_ID=IS_ID AND TC_CODE=IO_TCCODE AND IS_SI_ID=SI_ID AND IS_NO='" + sInvoiceNo + "'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;

            }
            finally
            {

            }
        }

        public DataTable PrintReceiveDTrReport(string sInvoiceId)
        {

            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {

                strQry = " SELECT distinct IS_NO,TO_CHAR(IS_DATE,'DD-MON-YYYY')IS_DATE,TO_CHAR(SI_DATE,'DD-MON-YYYY')SI_DATE,";
                strQry += " (select SM_NAME from TBLSTOREMAST where SM_ID=SI_FROM_STORE) AS SI_FROM_STORE,";
                strQry += " (select SM_NAME from TBLSTOREMAST where SM_ID= SI_TO_STORE) AS SI_TO_STORE,";
                strQry += " SI_NO,To_char(TC_CODE) as TC_CODE,To_char(TC_SLNO) as TC_SLNO,TO_CHAR(TC_CAPACITY) TC_CAPACITY,(SELECT TM_NAME FROM TBLTRANSMAKES";
                strQry += " WHERE TM_ID=TC_MAKE_ID) as TM_NAME,TC_MANF_DATE, ";
                strQry += "(SELECT DIV_NAME FROM TBLDIVISION,TBLSTOREMAST WHERE DIV_CODE=SM_OFF_CODE AND SM_ID=SI_TO_STORE) DIV_NAME,IS_RV_NO, ";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_OFFICE_CODE=SUBSTR(TC_LOCATION_ID,1,2) AND US_ROLE_ID='2' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A') STO_USERNAME ";
                strQry += " FROM TBLSTOREINVOICE,TBLSTOREINDENT,TBLSINVOICEOBJECTS,TBLSTOREMAST,TBLTCMASTER WHERE IS_SI_ID=SI_ID  AND ";
                strQry += " IO_IS_ID=IS_ID and SM_ID=SI_FROM_STORE AND IO_TCCODE=TC_CODE and IS_ID = '" + sInvoiceId + "'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;

            }
            finally
            {

            }
        }
        /// <summary>
        /// Print Store Indent Report
        /// </summary>
        /// <param name="sIndentId"></param>
        /// <returns></returns>
        public DataTable PrintStoreIndentReport(string sIndentId)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "select SI_NO,To_char(SI_DATE,'dd/MM/yyyy')SI_DATE,( select SM_NAME from TBLSTOREMAST where SM_ID=SI_FROM_STORE)SI_FROM_STORE,TO_CHAR(SO_CAPACITY)SO_CAPACITY,To_char(SO_QNTY)SO_QNTY,";
                strQry += " ( select SM_NAME from TBLSTOREMAST where SM_ID=SI_TO_STORE)SI_TO_STORE,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION,TBLSTOREMAST WHERE DIV_CODE=SM_OFF_CODE AND SM_ID=SI_FROM_STORE) DIV_NAME,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER,TBLSTOREMAST WHERE US_OFFICE_CODE=SM_OFF_CODE AND US_ROLE_ID='2' AND ROWNUM='1' AND US_MMS_ID IS NULL AND US_STATUS='A' AND SM_ID=SI_FROM_STORE) STO_USERNAME ";
                strQry += "  from TBLSINDENTOBJECTS,TBLSTOREINDENT where SI_ID=SO_SI_ID and SI_ID='" + sIndentId + "'";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// Print DTr Report
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public DataTable PrintDTrReport(clsReports objValue)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "select To_char(TC_CODE)TC_CODE,TC_SLNO,(select TM_NAME  from TBLTRANSMAKES ";
                strQry += " where TC_MAKE_ID=TM_ID)TC_MAKE_ID,To_char(TC_CAPACITY)TC_CAPACITY,To_char(TC_MANF_DATE,'dd-MON-yyyy')TC_MANF_DATE,";
                strQry += " (select MD_NAME from TBLMASTERDATA where MD_ID=TC_CURRENT_LOCATION and MD_TYPE='TCL') LOCATIONNAME ,  TC_OIL_CAPACITY , ";
                strQry += "  (select CM_CIRCLE_NAME from TBLCIRCLE WHERE CM_CIRCLE_CODE=SUBSTR(TC_LOCATION_ID,1,1)) AS CIRCLE,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(TC_LOCATION_ID,1,2)) as DIVISION ,";
                strQry += "(SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(TC_LOCATION_ID,1,3)) as SUBDIVISION, ";
                strQry += "(SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(TC_LOCATION_ID,1,4)) as SECTION ,  A.MD_NAME as   TC_STAR_RATE ";
                strQry += " FROM TBLTCMASTER  left join (SELECT MD_ID , MD_NAME FROM TBLMASTERDATA WHERE  MD_TYPE = 'SRT' )A on  A.MD_ID = TC_STAR_RATE  WHERE  TC_LOCATION_ID like '" + objValue.sOfficeCode + "%'  ";
                if (objValue.sCapacity != null)
                {
                    strQry += "  and  TC_CAPACITY='" + objValue.sCapacity + "'";
                }
                if (objValue.sMake != null)
                {
                    strQry += " and  TC_MAKE_ID='" + objValue.sMake + "' ";
                }
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// Print DTC CR Report
        /// </summary>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public DataTable PrintDTCCReport(clsReports objValue)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "select DT_CODE,DT_NAME, (select TM_NAME  from TBLTRANSMAKES  where TC_MAKE_ID=TM_ID)TC_MAKE_ID,";
                strQry += " to_char(TC_CODE)TC_CODE, To_char(TC_CAPACITY)TC_CAPACITY,";
                strQry += " (select CM_CIRCLE_NAME from TBLCIRCLE WHERE CM_CIRCLE_CODE=SUBSTR(DT_OM_SLNO,1,1)) AS CIRCLE,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(DT_OM_SLNO,1,2)) as DIVISION ,";
                strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DT_OM_SLNO,1,3)) as SUBDIVISION, ";
                strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(DT_OM_SLNO,1,4)) as SECTION,FD_FEEDER_NAME,FD_FEEDER_CODE,TC_SLNO,  A.MD_NAME as STAR_RATE,TO_CHAR(TC_MANF_DATE,'dd-mm-yyyy') TC_MANF_DATE ";
                strQry += " FROM TBLDTCMAST,TBLFEEDERMAST,TBLTCMASTER left  join   (SELECT MD_ID , MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'SRT' )A on A.MD_ID = TC_STAR_RATE"
                + " WHERE TC_CODE=DT_TC_ID AND FD_FEEDER_CODE=DT_FDRSLNO ";
                strQry += " and DT_OM_SLNO  like '" + objValue.sOfficeCode + "%' AND DT_TC_ID <>0  ";
                if (objValue.sFeeder != null)
                {
                    strQry += "  and  DT_FDRSLNO='" + objValue.sFeeder + "'  ";
                }
                if (objValue.sSchemeType != null)
                {
                    strQry += " and  DT_PROJECTTYPE='" + objValue.sSchemeType + "' ";
                }
                if (objValue.sCapacity != null)
                {
                    strQry += " AND TC_CAPACITY='" + objValue.sCapacity + "'";
                }
                strQry += "ORDER BY DT_CODE ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// Print Abstract Report
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintAbstractReport(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "  SELECT A.TC_CAPACITY,A.CIRCLE,A.DIV,A.SUBDIV,A.SECTION,A.TC_COUNT AS ONEWEEKCOUNT,B.TC_COUNT ";
                strQry += " AS FORTNIGHTCOUNT,C.TC_COUNT AS MONTHCOUNT , D.TC_COUNT AS MORETHENMONTHCOUNT FROM( SELECT B.OFF_CODE,";
                strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)AS OFC_NAME  FROM VIEW_ALL_OFFICES WHERE  OFF_CODE=SUBSTR(B.OFF_CODE,0,2)) ";
                strQry += " DIV,  (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE= substr(b.OFF_CODE,0,1))";
                strQry += " CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE= substr(b.OFF_CODE,0,4))";
                strQry += " SECTION,  NVL(TC_COUNT,0) TC_COUNT,MD_NAME AS TC_CAPACITY,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) SUBDIV FROM  ";
                strQry += " (SELECT SUM(CASE NVL(DF_ID,0) WHEN 0 THEN 0 ELSE 1 END) AS TC_COUNT,TC_CAPACITY, SUBSTR(OFF_CODE,0,3) OFF_CODE";
                strQry += " FROM VIEW_ALL_OFFICES LEFT JOIN TBLDTCFAILURE ON SUBSTR(DF_LOC_CODE,0,3)=OFF_CODE LEFT JOIN TBLTCMASTER ON ";
                strQry += " TC_CODE=DF_EQUIPMENT_ID WHERE LENGTH(OFF_CODE)=3 AND DF_DATE>=SYSDATE-7 AND DF_STATUS_FLAG<>2 AND DF_REPLACE_FLAG='0'  ";
                strQry += " GROUP BY SUBSTR(OFF_CODE,0,3),TC_CAPACITY)A RIGHT JOIN (SELECT MD_NAME,OFF_NAME,OFF_CODE FROM TBLMASTERDATA,";
                strQry += " VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=3 AND to_number(MD_NAME)<=500)B ON B.OFF_CODE=A.OFF_CODE ";
                strQry += " AND A.TC_CAPACITY=B.MD_NAME ORDER BY MD_NAME)A  INNER JOIN (SELECT (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)AS OFC_NAME";
                strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(B.OFF_CODE,0,2)) DIV,  NVL(TC_COUNT,0) TC_COUNT,MD_NAME AS TC_CAPACITY,";
                strQry += " SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) SUBDIV FROM (SELECT SUM(CASE NVL(DF_ID,0) WHEN 0 THEN 0 ELSE 1 END) AS TC_COUNT,";
                strQry += " TC_CAPACITY, SUBSTR(OFF_CODE,0,3) OFF_CODE FROM VIEW_ALL_OFFICES LEFT JOIN TBLDTCFAILURE ON SUBSTR(DF_LOC_CODE,0,3)=OFF_CODE ";
                strQry += " LEFT JOIN TBLTCMASTER ON TC_CODE=DF_EQUIPMENT_ID WHERE LENGTH(OFF_CODE)=3 AND DF_DATE>=(SYSDATE-14) AND DF_DATE<=SYSDATE-7";
                strQry += " AND DF_STATUS_FLAG<>2 AND DF_REPLACE_FLAG='0' GROUP BY SUBSTR(OFF_CODE,0,3),TC_CAPACITY)A RIGHT JOIN (SELECT MD_NAME,OFF_NAME,OFF_CODE  ";
                strQry += " FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=3 AND to_number(MD_NAME)<=500)B ON";
                strQry += " B.OFF_CODE=A.OFF_CODE AND A.TC_CAPACITY=B.MD_NAME ORDER BY MD_NAME)B ON A.DIV=B.DIV AND A.TC_CAPACITY=B.TC_CAPACITY AND";
                strQry += " A.SUBDIV=B.SUBDIV INNER JOIN  (SELECT (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)AS OFC_NAME  FROM VIEW_ALL_OFFICES";
                strQry += " WHERE OFF_CODE=SUBSTR(B.OFF_CODE,0,2)) DIV,  NVL(TC_COUNT,0) TC_COUNT,MD_NAME AS TC_CAPACITY,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)";
                strQry += " SUBDIV FROM (SELECT SUM(CASE NVL(DF_ID,0) WHEN 0 THEN 0 ELSE 1 END) AS TC_COUNT,TC_CAPACITY, SUBSTR(OFF_CODE,0,3) ";
                strQry += " OFF_CODE FROM VIEW_ALL_OFFICES LEFT JOIN TBLDTCFAILURE ON SUBSTR(DF_LOC_CODE,0,3)=OFF_CODE LEFT JOIN TBLTCMASTER ON ";
                strQry += " TC_CODE=DF_EQUIPMENT_ID WHERE LENGTH(OFF_CODE)=3 AND DF_DATE>=(SYSDATE-30) AND DF_DATE<=(SYSDATE-14) AND DF_STATUS_FLAG<>2 AND DF_REPLACE_FLAG='0' ";
                strQry += " GROUP BY SUBSTR(OFF_CODE,0,3),TC_CAPACITY)A RIGHT JOIN (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES";
                strQry += " WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=3 AND to_number(MD_NAME)<=500)B ON B.OFF_CODE=A.OFF_CODE AND A.TC_CAPACITY=B.MD_NAME ";
                strQry += " ORDER BY MD_NAME)C ON C.DIV=B.DIV AND C.TC_CAPACITY=C.TC_CAPACITY AND C.SUBDIV=B.SUBDIV AND A.DIV=C.DIV AND A.TC_CAPACITY=C.TC_CAPACITY";
                strQry += " AND A.SUBDIV=C.SUBDIV INNER JOIN  (SELECT (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)AS OFC_NAME  FROM VIEW_ALL_OFFICES ";
                strQry += " WHERE OFF_CODE=SUBSTR(B.OFF_CODE,0,2)) DIV,  NVL(TC_COUNT,0) TC_COUNT,MD_NAME AS TC_CAPACITY,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)";
                strQry += " SUBDIV FROM (SELECT SUM(CASE NVL(DF_ID,0) WHEN 0 THEN 0 ELSE 1 END) AS TC_COUNT,TC_CAPACITY, SUBSTR(OFF_CODE,0,3) OFF_CODE";
                strQry += " FROM VIEW_ALL_OFFICES LEFT JOIN TBLDTCFAILURE ON SUBSTR(DF_LOC_CODE,0,3)=OFF_CODE AND DF_REPLACE_FLAG='0' LEFT JOIN TBLTCMASTER ON TC_CODE=DF_EQUIPMENT_ID";
                strQry += " WHERE LENGTH(OFF_CODE)=3 AND DF_DATE<=(SYSDATE-30) AND DF_STATUS_FLAG<>2 AND DF_REPLACE_FLAG='0'  GROUP BY SUBSTR(OFF_CODE,0,3),TC_CAPACITY)A RIGHT JOIN ";
                strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=3 AND ";
                strQry += " to_number(MD_NAME)<=500)B ON B.OFF_CODE=A.OFF_CODE AND A.TC_CAPACITY=B.MD_NAME ORDER BY MD_NAME)D ON C.DIV=D.DIV AND ";
                strQry += " C.TC_CAPACITY=D.TC_CAPACITY AND C.SUBDIV=D.SUBDIV AND A.DIV=D.DIV AND A.TC_CAPACITY=D.TC_CAPACITY AND A.SUBDIV=D.SUBDIV ";
                strQry += " AND B.DIV=D.DIV AND B.TC_CAPACITY=D.TC_CAPACITY AND B.SUBDIV=D.SUBDIV AND OFF_CODE LIKE '" + objReport.sOfficeCode + "%' ORDER BY CIRCLE,DIV ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintAbstractReportTcFailedAtFSR(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT a.md_name as CAPACITY,substr(a.off_name,instr(a.off_name,':')+1)DIV,a.off_code,tc_failedbutnotreturned as FIELD_COUNT,tc_failedbutnotmapped as FIELD_COUNT_TOBEREPLACED,tcfailedbutinstore as STORE_COUNT,tcfailedbutinrepaircenter as REPAIRER_COUNT,  ";
                strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SUBSTR(B.OFF_CODE,0,1))CIRCLE from ";
                strQry += " (SELECT md_name,off_name,off_code,nvl(tc_failedbutnotreturned,0) tc_failedbutnotreturned from (SELECT tc_capacity,substr";
                strQry += "  (tc_location_id,0,2)tc_location_id ,count(tc_code) as tc_failedbutnotreturned from TBLTCMASTER inner join  TBLDTCFAILURE";
                strQry += "  on TC_CODE=DF_EQUIPMENT_ID INNER JOIN TBLWORKORDER on df_id=wo_df_id INNER JOIN TBLINDENT on wo_slno=TI_WO_SLNO INNER JOIN ";
                strQry += " TBLDTCINVOICE on in_ti_no=ti_id LEFT JOIN TBLTCREPLACE on IN_NO= TR_IN_NO WHERE tr_ri_no is NULL or tr_rv_no is null and tc_status='3'  GROUP BY";
                strQry += " tc_capacity,substr(tc_location_id,0,2))a RIGHT JOIN (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND to_number(MD_NAME)<=500 AND  LENGTH ";
                strQry += " (OFF_CODE)=2)b on MD_NAME=tc_capacity and tc_location_id=OFF_CODE)a INNER JOIN ";
                strQry += " (SELECT md_name,off_name,off_code,nvl(tc_failedbutnotmapped,0) tc_failedbutnotmapped from (SELECT tc_capacity,substr  ";
                strQry += " (tc_location_id,0,2)tc_location_id ,count(tc_code) as tc_failedbutnotmapped from TBLTCMASTER inner join  TBLDTCFAILURE  on  ";
                strQry += " TC_CODE=DF_EQUIPMENT_ID LEFT JOIN TBLWORKORDER on df_id=wo_df_id LEFT JOIN TBLINDENT on wo_slno=TI_WO_SLNO  left JOIN  ";
                strQry += " TBLDTCINVOICE on in_ti_no=ti_id  WHERE in_ti_no is NULL and tc_status='3' AND TC_CAPACITY<=500 GROUP BY tc_capacity,substr(tc_location_id,0,2))a RIGHT JOIN";
                strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=2)b";
                strQry += " on MD_NAME=tc_capacity and tc_location_id=OFF_CODE)b on a.md_name=b.md_name and a.off_name=b.off_name and a.off_code=b.off_code INNER JOIN ";
                strQry += " (SELECT md_name,off_name,off_code,nvl(tcfailedbutinstore,0) tcfailedbutinstore from (SELECT count(tc_code) ";
                strQry += " tcfailedbutinstore,substr(tc_location_id,0,2)tc_location_id,tc_capacity from TBLTCMASTER WHERE tc_status=3 and";
                strQry += " tc_current_location=1 GROUP BY substr(tc_location_id,0,2),tc_capacity )a RIGHT JOIN ";
                strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=2)b ";
                strQry += " on MD_NAME=tc_capacity and tc_location_id=OFF_CODE AND to_number(MD_NAME)<=500)c on a.md_name=c.md_name and a.off_name=c.off_name and a.off_code=c.off_code  INNER JOIN ";
                strQry += " (SELECT md_name,off_name,off_code,nvl(tcfailedbutinrepaircenter,0) tcfailedbutinrepaircenter from (SELECT count(tc_code)";
                strQry += " tcfailedbutinrepaircenter,substr(tc_location_id,0,2)tc_location_id,tc_capacity from TBLTCMASTER WHERE tc_status=3 and ";
                strQry += "tc_current_location=3 GROUP BY substr(tc_location_id,0,2),tc_capacity )a RIGHT JOIN ";
                strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=2)b ";
                strQry += "on MD_NAME=tc_capacity and tc_location_id=OFF_CODE AND to_number(MD_NAME)<=500)d on a.md_name=d.md_name and a.off_name=d.off_name and a.off_code=d.off_code AND a.OFF_CODE like '" + objReport.sOfficeCode + "%'  ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintRepairerTcCount(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                strQry = "SELECT MD_NAME AS TC_CAPACITY,TR_NAME AS REPAIRER,NVL(TC_COUNT,0) AS TC_COUNT FROM (SELECT TC_LOCATION_ID,TC_CAPACITY,NVL(COUNT(TC_CODE),0) TC_COUNT,RSM_SUPREP_ID FROM TBLTCMASTER INNER JOIN ";
                strQry += " TBLREPAIRSENTDETAILS ON RSD_TC_CODE=TC_CODE INNER JOIN TBLREPAIRSENTMASTER ON RSM_ID=RSD_RSM_ID WHERE  RSD_DELIVARY_DATE IS NULL AND RSM_SUPREP_TYPE=2 AND TC_LOCATION_ID  LIKE '" + objReport.sOfficeCode + "%' AND TC_STATUS='3' AND TC_CURRENT_LOCATION='3'";
                strQry += " GROUP BY TC_CAPACITY,RSM_SUPREP_ID,TC_LOCATION_ID ORDER BY RSM_SUPREP_ID)A RIGHT JOIN (SELECT TR_NAME,TR_ID,MD_NAME FROM TBLTRANSREPAIRER,TBLMASTERDATA WHERE UPPER(MD_TYPE)='C' ";
                strQry += " AND MD_NAME<=500)C ON  MD_NAME=TC_CAPACITY AND RSM_SUPREP_ID=TR_ID AND TC_LOCATION_ID LIKE '" + objReport.sOfficeCode + "%' ";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintCompletedRepairerTcCount(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                strQry = "SELECT MD_NAME AS TC_CAPACITY,TR_NAME AS REPAIRER,NVL(TC_COUNT,0) AS TC_COUNT FROM (SELECT TC_LOCATION_ID,TC_CAPACITY,NVL(COUNT(TC_CODE),0) TC_COUNT,RSM_SUPREP_ID FROM TBLTCMASTER INNER JOIN ";
                strQry += " TBLREPAIRSENTDETAILS ON RSD_TC_CODE=TC_CODE INNER JOIN TBLREPAIRSENTMASTER ON RSM_ID=RSD_RSM_ID WHERE  RSD_DELIVARY_DATE IS NOT NULL AND RSM_SUPREP_TYPE=2 ";
                strQry += " GROUP BY TC_CAPACITY,RSM_SUPREP_ID,TC_LOCATION_ID ORDER BY RSM_SUPREP_ID)A RIGHT JOIN (SELECT TR_NAME,TR_ID,MD_NAME FROM TBLTRANSREPAIRER,TBLMASTERDATA WHERE UPPER(MD_TYPE)='C' ";
                strQry += " AND MD_NAME<=500)C ON  MD_NAME=TC_CAPACITY AND RSM_SUPREP_ID=TR_ID AND TC_LOCATION_ID LIKE '" + objReport.sOfficeCode + "%' ";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable PrintAbstractReportWeekWise()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(b.OFF_CODE,0,2)) div,  NVL(TC_COUNT,0) TC_COUNT,MD_NAME AS ";
                strQry += " TC_CAPACITY,SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) subdiv FROM (SELECT sum(CASE NVL(DF_ID,0) WHEN 0 THEN 0 ELSE 1 END) AS TC_COUNT,TC_CAPACITY, SUBSTR(OFF_CODE,0,3) OFF_CODE FROM VIEW_ALL_OFFICES LEFT JOIN TBLDTCFAILURE ";
                strQry += "  ON SUBSTR(DF_LOC_CODE,0,3)=OFF_CODE LEFT JOIN TBLTCMASTER on TC_CODE=DF_EQUIPMENT_ID WHERE LENGTH(OFF_CODE)=3 DF_DATE>=SYSDATE-7   GROUP BY SUBSTR(OFF_CODE,0,3),TC_CAPACITY)a right JOIN ";
                strQry += " (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES where upper(MD_TYPE)='C' ";
                strQry += "  AND  LENGTH(OFF_CODE)=3)b on b.OFF_CODE=a.OFF_CODE and a.TC_CAPACITY=b.MD_NAME ORDER BY TC_COUNT,OFF_NAME ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dt;
            }
        }
        public DataTable PrintAbstractReportMonth()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT TC_CAPACITY,COUNT(*)TOTALFAILUIRE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) ";
                strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=DF_LOC_CODE) OFF_NAME  FROM TBLTCMASTER,TBLDTCFAILURE ";
                strQry += " WHERE TC_CODE=DF_EQUIPMENT_ID AND TO_CHAR(DF_DATE,'YYYY/MM')>=to_char(add_months(trunc(SYSDATE),-8),'YYYY/MM') ";
                strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM')<=TO_CHAR(SYSDATE,'YYYY/MM') GROUP BY TC_CAPACITY,DF_LOC_CODE ORDER BY TOTALFAILUIRE DESC";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        public List<string> PrintBlob(string sOfficeCode)
        {
            List<string> sImagePaths = new List<string>();
            try
            {

                string strQry = string.Empty;
                DataTable dt = new DataTable();
                DataTable dtRoles = new DataTable();
                strQry = "Select US_ID FROM TBLUSER WHERE US_OFFICE_CODE='" + sOfficeCode + "' OR US_OFFICE_CODE=SUBSTR('" + sOfficeCode + "',1,2) OR US_OFFICE_CODE=SUBSTR('" + sOfficeCode + "',1,3)";
                dt = ObjCon.getDataTable(strQry);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    strQry = "select DISTINCT US_SIGN_IMAGE,RO_NAME,RO_ID FROM TBLUSER,TBLROLES where US_ID='" + dt.Rows[j]["US_ID"] + "' and US_ROLE_ID=RO_ID";

                    dtRoles = ObjCon.getDataTable(strQry);
                    if (dtRoles.Rows[0]["US_SIGN_IMAGE"] != DBNull.Value)
                    {
                        byte[] myByteArray = (byte[])dtRoles.Rows[0]["US_SIGN_IMAGE"];
                        MemoryStream memStream = new MemoryStream(myByteArray);
                        Image img = System.Drawing.Image.FromStream(memStream);
                        img.Save(System.Web.HttpContext.Current.Server.MapPath("~/DTLMSDocs/") + "\\" + dt.Rows[j]["US_ID"] + ".Jpeg");
                        sImagePaths.Add(System.Web.HttpContext.Current.Server.MapPath("~/DTLMSDocs/") + "\\" + dt.Rows[j]["US_ID"] + ".Jpeg" + "~" + dtRoles.Rows[0]["RO_NAME"] + "~" + dtRoles.Rows[0]["RO_ID"]);
                    }
                }
                return sImagePaths;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return sImagePaths;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetRoles()
        {
            DataTable dtRoles = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "Select RO_ID,RO_NAME from TBLROLES";
                dtRoles = ObjCon.getDataTable(strQry);
                return dtRoles;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtRoles;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable TCFailReport(clsReports objReport)
        {
            DataTable dtFailureDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT * FROM(";
                if (objReport.sType == "1")
                {
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += "SELECT TO_CHAR(TC_CODE)TC_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,'' AS COMMISSION,'' AS DECOMMISSION,DF_DTC_CODE DT_CODE,";
                        strQry += " DF_LOC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,MD_NAME,'' as WO_NO,'' as TI_INDENT_NO,'' as IN_INV_NO,'' as TR_RI_NO, ";
                        strQry += "'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,";
                        strQry += "substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM  TBLFEEDERMAST WHERE ";
                        strQry += "FD_FEEDER_CODE =DT_FDRSLNO)FD_FEEDER_NAME,'' AS FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID,'' AS WO_DATE,'' AS TI_INDENT_DATE,'' AS IN_DATE,'' AS TR_RV_DATE,'' AS TR_RI_DATE, '" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                        strQry += "(SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  ";
                        strQry += "from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  ";
                        strQry += "from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2))  DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  ";
                        strQry += "from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION, (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  ";
                        strQry += "from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,TBLMASTERDATA,";
                        strQry += "TBLDTCMAST,WORKFLOWSTATUSDUMMY where  DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE ";
                        strQry += " AND DF_DTC_CODE=WO_DATA_ID   and DF_REPLACE_FLAG=0 AND DF_LOC_CODE LIKE ";
                        strQry += "'" + sOfficeCode + "%' AND DF_FAILURE_TYPE=MD_ID AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ")";
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                        if (objReport.sCapacity != null)
                        {
                            strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                        }
                        if (objReport.sGuranteeType != null)
                        {
                            strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                        }
                    }
                    else
                    {
                        strQry += "SELECT TO_CHAR(TC_CODE)TC_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,'' AS COMMISSION,'' AS DECOMMISSION,DF_DTC_CODE DT_CODE,DF_LOC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,MD_NAME,'' as WO_NO,'' as TI_INDENT_NO,'' as IN_INV_NO,'' as TR_RI_NO, ";
                        strQry += "'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                        strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,'' AS FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID,'' AS WO_DATE,'' AS TI_INDENT_DATE,'' AS IN_DATE,'' AS TR_RV_DATE, '' AS TR_RI_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                        strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                        strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                        strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                        strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,";
                        strQry += "TBLMASTERDATA,TBLDTCMAST,WORKFLOWSTATUSDUMMY where DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE AND DF_DTC_CODE=";
                        strQry += "WO_DATA_ID  and DF_REPLACE_FLAG=0 AND (DECOMMISION IS NULL ) AND ";
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                        strQry += " AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                        if (objReport.sMake != null)
                        {
                            strQry += "AND TC_MAKE_ID='" + objReport.sMake + "' ";
                        }
                        if (objReport.sFailureType != null)
                        {
                            strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "' ";
                        }
                        if (objReport.sCapacity != null)
                        {
                            strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                        }
                        if (objReport.sGuranteeType != null)
                        {
                            strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                        }
                        strQry += "AND DF_FAILURE_TYPE=MD_ID AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") UNION ALL ";
                        strQry += "SELECT TO_CHAR(DT_TC_ID) TC_CODE,to_char(wo_cr_on,'dd-MON-yyyy') DF_DATE,'' AS COMMISSION,'' AS DECOMMISSION,to_char(WO_DATA_ID) AS DT_CODE,OFFCODE AS ";
                        strQry += "DF_LOC_CODE,to_char(TC_CAPACITY) TC_CAPACITY,'' AS MD_NAME,'' AS FAILURE_TYPE,'' as WO_NO,'' as TI_INDENT_NO,'' as IN_INV_NO,'' as TR_RI_NO, '' AS FROMDATE,'' AS TODATE,";
                        strQry += "TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(WO_DATA_ID,0,4) FD_FEEDER_CODE, (SELECT FD_FEEDER_NAME FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE=";
                        strQry += "substr(WO_DATA_ID,0,4)) FD_FEEDER_NAME, (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID) TC_MAKE_ID,DT_NAME,'' AS WO_DATE,'' AS TI_INDENT_DATE,'' AS IN_DATE,'' AS TR_RV_DATE, '' AS TR_RI_DATE, '" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                        strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(OFFCODE,0,1)) CIRCLE,";
                        strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(OFFCODE,0,2))  DIVISION,";
                        strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(OFFCODE,0,3)) SUBDIVISION, ";
                        strQry += "(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(OFFCODE,0,4)) SECTION FROM WORKFLOWSTATUSDUMMY,";
                        strQry += "TBLDTCMAST,TBLTCMASTER WHERE DT_CODE=WO_DATA_ID AND TC_CODE=DT_TC_ID AND FAILURE IS NULL  AND OFFCODE LIKE '" + sOfficeCode + "%'";
                        if (objReport.sCapacity != null)
                        {
                            strQry += " AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                        }
                    }
                }
                if (objReport.sType == "2")
                {
                    strQry += "SELECT TO_CHAR(TC_CODE)TC_CODE,DF_DTC_CODE DT_CODE,WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,DF_LOC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,MD_NAME,WO_NO,'' as TI_INDENT_NO,'' as IN_INV_NO,'' as TR_RI_NO, '" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                    strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME, CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE ,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID,'' AS WO_DATE,'' AS TI_INDENT_DATE,'' AS IN_DATE,'' AS TR_RV_DATE, '' AS TR_RI_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                    strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                    strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                    strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from ";
                    strQry += "TBLTCMASTER,TBLDTCFAILURE,TBLMASTERDATA,TBLDTCMAST,TBLWORKORDER where  DF_EQUIPMENT_ID=TC_CODE ";
                    strQry += "and DF_DTC_CODE=DT_CODE and DF_REPLACE_FLAG=0 AND ";
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    }
                    strQry += " AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                    if (objReport.sMake != null)
                    {
                        strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if (objReport.sFailureType != null && objReport.sFailureType != "")
                    {
                        strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    }
                    if (objReport.sGuranteeType != null)
                    {
                        strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    }
                    strQry += "AND DF_FAILURE_TYPE=MD_ID AND DF_ID=WO_DF_ID(+) AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") AND DF_ID IS NOT NULL AND WO_SLNO IS NULL";
                }
                if (objReport.sType == "3")
                {
                    strQry += "SELECT TO_CHAR(TC_CODE)TC_CODE,DF_DTC_CODE DT_CODE,DF_LOC_CODE,WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,TC_CAPACITY,MD_NAME,'' as WO_NO,TI_INDENT_NO ,'' as IN_INV_NO,'' as TR_RI_NO,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                    strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID,'' AS TI_INDENT_DATE,'' AS IN_DATE,'' AS TR_RV_DATE, '' AS TR_RI_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                    if (objReport.sReportType == "3")
                    {
                        strQry += "TO_CHAR(WO_DATE,'DD-MON-YYYY')WO_DATE,";
                    }
                    else
                    {
                        strQry += "'' AS WO_DATE,";
                    }
                    strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                    strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                    strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,TBLDTCMAST,TBLMASTERDATA,TBLWORKORDER,TBLINDENT where  DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE and DF_REPLACE_FLAG=0 AND ";
                    if (objReport.sReportType == "3")
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(WO_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(WO_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(WO_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(WO_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(WO_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(WO_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    else
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    strQry += " AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                    if (objReport.sMake != null)
                    {
                        strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if (objReport.sFailureType != null && objReport.sFailureType != "")
                    {
                        strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    }
                    if (objReport.sGuranteeType != null)
                    {
                        strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    }
                    strQry += "AND DF_FAILURE_TYPE=MD_ID AND DF_ID=WO_DF_ID(+) AND WO_SLNO=TI_WO_SLNO(+) AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") AND WO_SLNO IS NOT NULL AND TI_ID IS NULL";
                }
                if (objReport.sType == "4")
                {
                    strQry += "SELECT TO_CHAR(TC_CODE)TC_CODE,DF_DTC_CODE DT_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION,DF_LOC_CODE,TC_CAPACITY,MD_NAME, WO_NO,'' as TI_INDENT_NO ,IN_INV_NO,'' as TR_RI_NO,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                    strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID,'' AS WO_DATE,'' AS IN_DATE,'' AS TR_RV_DATE, '' AS TR_RI_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                    if (objReport.sReportType == "3")
                    {
                        strQry += "TO_CHAR(TI_INDENT_DATE,'DD-MON-YYYY'),";
                    }
                    else
                    {
                        strQry += "'' AS TI_INDENT_DATE,";
                    }
                    strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                    strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                    strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,TBLDTCMAST,TBLMASTERDATA,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE where  DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE and DF_REPLACE_FLAG=0 AND ";
                    if (objReport.sReportType == "3")
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(TI_INDENT_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(TI_INDENT_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(TI_INDENT_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(TI_INDENT_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(TI_INDENT_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(TI_INDENT_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    else
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    strQry += " AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                    if (objReport.sMake != null)
                    {
                        strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if (objReport.sFailureType != null && objReport.sFailureType != "")
                    {
                        strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    }
                    if (objReport.sGuranteeType != null)
                    {
                        strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    }
                    strQry += "AND DF_FAILURE_TYPE=MD_ID AND DF_ID=WO_DF_ID(+) AND WO_SLNO=TI_WO_SLNO(+) AND TI_ID=IN_TI_NO(+) AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") AND TI_ID IS NOT NULL AND IN_NO IS NULL";
                }
                if (objReport.sType == "5")
                {
                    strQry += "SELECT TO_CHAR(TC_CODE)TC_CODE,DF_DTC_CODE DT_CODE,DF_LOC_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION,TC_CAPACITY,MD_NAME,'' as WO_NO,'' as TI_INDENT_NO ,'' as IN_INV_NO,TR_RI_NO,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                    strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID,'' AS WO_DATE,'' AS TI_INDENT_DATE,'' AS TR_RV_DATE, '' AS TR_RI_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                    if (objReport.sReportType == "3")
                    {
                        strQry += "TO_CHAR(IN_DATE,'DD-MON-YYYY')IN_DATE,";
                    }
                    else
                    {
                        strQry += "'' AS IN_DATE,";
                    }
                    strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                    strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                    strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,TBLDTCMAST,TBLMASTERDATA,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE where DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE and DF_REPLACE_FLAG=0 AND ";
                    if (objReport.sReportType == "3")
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(IN_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(IN_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(IN_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(IN_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(IN_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(IN_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    else
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    strQry += "AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                    if (objReport.sMake != null)
                    {
                        strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if (objReport.sFailureType != null && objReport.sFailureType != "")
                    {
                        strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    }
                    if (objReport.sGuranteeType != null)
                    {
                        strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    }
                    strQry += "AND DF_FAILURE_TYPE=MD_ID AND DF_ID=WO_DF_ID(+) AND WO_SLNO=TI_WO_SLNO(+) AND TI_ID=IN_TI_NO(+) AND IN_NO=TR_IN_NO(+) AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") AND IN_NO IS NOT NULL AND TR_ID IS NULL";
                }
                if (objReport.sType == "6")
                {
                    #region  old Query 
                    //strQry = "SELECT TO_CHAR(TC_CODE)TC_CODE,DF_DTC_CODE DT_CODE,DF_LOC_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION,TC_CAPACITY,MD_NAME,WO_NO as WO_NO,'' as TI_INDENT_NO ,'' as IN_INV_NO,'' as TR_RI_NO,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                    //strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID,WO_DATE AS WO_DATE,'' AS TI_INDENT_DATE,'' AS TR_RV_DATE,'' AS IN_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                    //if (objReport.sReportType == "3")
                    //{
                    //    strQry += "TO_CHAR(TR_RI_DATE,'DD-MON-YYYY')TR_RI_DATE,";
                    //}
                    //else
                    //{
                    //    strQry += "'' AS TR_RI_DATE,";
                    //}
                    //strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                    //strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                    //strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                    //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,TBLDTCMAST,TBLMASTERDATA,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE where DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE and DF_REPLACE_FLAG=0 AND ";
                    ////strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD') BETWEEN '" + sFromDate + "' AND '" + sTodate + "'
                    //if (objReport.sReportType == "3")
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(TR_RI_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(TR_RI_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";

                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(TR_RI_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(TR_RI_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        strQry += " TO_CHAR(TR_RI_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(TR_RI_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    //    }
                    //}
                    //else
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";

                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    //    }
                    //}



                    //strQry += "AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                    //if (objReport.sMake != null)
                    //{
                    //    strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    //}
                    //strQry += "AND DF_FAILURE_TYPE=MD_ID AND DF_ID=WO_DF_ID(+) AND WO_SLNO=TI_WO_SLNO(+) AND TI_ID=IN_TI_NO(+) AND IN_NO=TR_IN_NO(+) AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") AND IN_NO IS NOT NULL AND TR_ID IS NULL";

                    #endregion
                    strQry += " SELECT C.TC_CODE ,  D.DT_CODE ,D.DT_NAME ,  B.DF_LOC_CODE , B.DF_REPLACE_FLAG , TO_CHAR(B.DF_DATE,'dd-MON-yy')DF_DATE ,  E.WO_AMT AS COMMISSION,E.WO_AMT_DECOM AS DECOMMISSION , E.WO_NO  ";
                    strQry += ",  to_number(C.TC_CAPACITY) AS TC_CAPACITY , F.MD_NAME ,'' as TI_INDENT_NO ,'' as IN_INV_NO,'' AS TR_RI_NO , '" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY , substr(B.DF_DTC_CODE,0,4)FD_FEEDER_CODE , ";
                    strQry += " (SELECT DISTINCT FD_FEEDER_NAME FROM  TBLFEEDERMAST WHERE FD_FEEDER_CODE=substr(B.DF_DTC_CODE,0,4))FD_FEEDER_NAME , CASE B.DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE , ";
                    strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=C.TC_MAKE_ID)TC_MAKE_ID ,D.WO_DATE AS WO_DATE,'' AS TI_INDENT_DATE,'' AS IN_DATE, '' AS TR_RI_DATE , '" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY, ";
                    strQry += " (SELECT CM_CIRCLE_NAME FROM TBLCIRCLE WHERE CM_CIRCLE_CODE = substr(B.DF_LOC_CODE,0,1)) CIRCLE  ,D.DIV DIVISION, D.SUBDIVSION SUBDIVISION , D.OMSECTION SECTION";
                    strQry += "     FROM  MV_WORKFLOWSTATUSDUMMY A  , TBLDTCFAILURE B , TBLTCMASTER C , MV_VIEWPENDINGFAILURE D ,TBLWORKORDER E , TBLMASTERDATA F ";
                    strQry += " WHERE F.MD_ID = b.DF_FAILURE_TYPE and   B.DF_ID = E.WO_DF_ID and  A.WO_DATA_ID = D.DT_CODE AND D.DF_ID = B.DF_ID    AND B.DF_EQUIPMENT_ID = C.TC_CODE AND ";
                    if (objReport.sReportType == "3")
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    else
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(B.DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(B.DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(B.DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(B.DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(B.DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(B.DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    strQry += "AND A.RIAPPROVE IS  NULL AND a.DECOMMISION IS  NOT NULL AND   A.WO_BO_ID  <>10 AND   A.OFFCODE LIKE '" + sOfficeCode + "%' AND   F.MD_TYPE= 'FT' ";
                    if (objReport.sMake != null)
                    {
                        strQry += "AND .TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if (objReport.sFailureType != null && objReport.sFailureType != "")
                    {
                        strQry += "AND B.DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                }
                if (objReport.sType == "7")
                {
                    #region Refrence of Old queary
                    //strQry = "SELECT TO_CHAR(TC_CODE)TC_CODE,DF_DTC_CODE DT_CODE,DF_LOC_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION,";
                    //strQry +=" TC_CAPACITY,MD_NAME,WO_NO as WO_NO,'' as TI_INDENT_NO ,'' as IN_INV_NO,'' AS TR_RI_NO,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                    //strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID
                    //strQry += " ,WO_DATE AS WO_DATE,'' AS TI_INDENT_DATE,'' AS IN_DATE, '' AS TR_RI_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                    //if (objReport.sReportType == "3")
                    //{
                    //    strQry += "TO_CHAR(TR_RV_DATE,'DD-MON-YYYY')TR_RV_DATE,";
                    //}
                    //else
                    //{
                    //    strQry += "'' AS TR_RV_DATE,";
                    //}
                    //strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                    //strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                    //strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                    //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,TBLDTCMAST,
                    //strQry += " TBLMASTERDATA,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE where  DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE and DF_REPLACE_FLAG=0 AND ";
                    ////strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD') BETWEEN '" + sFromDate + "' AND '" + sTodate + "'
                    //if (objReport.sReportType == "3")
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        strQry += " TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    //    }
                    //}
                    //else
                    //{
                    //    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //    {
                    //        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                    //    }
                    //    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //    {
                    //        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //    }
                    //    if (objReport.sFromDate == null && objReport.sTodate == null)
                    //    {
                    //        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    //    }
                    //    if (objReport.sFromDate != null && objReport.sTodate != null)
                    //    {
                    //        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    //    }
                    //}
                    //strQry += "AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                    //if (objReport.sMake != null)
                    //{
                    //    strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    //}
                    //strQry += "AND DF_FAILURE_TYPE=MD_ID AND DF_ID=WO_DF_ID(+) AND WO_SLNO=TI_WO_SLNO(+) AND TI_ID=IN_TI_NO(+) AND IN_NO=TR_IN_NO(+) AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") AND TR_INVENTORY_QTY IS NULL";
                    #endregion
                    strQry += " SELECT C.TC_CODE ,  D.DT_CODE ,D.DT_NAME  ,  B.DF_LOC_CODE , B.DF_REPLACE_FLAG , TO_CHAR(B.DF_DATE,'dd-MON-yy')DF_DATE ,  E.WO_AMT AS COMMISSION,E.WO_AMT_DECOM AS DECOMMISSION , E.WO_NO  ";
                    strQry += ", to_number(C.TC_CAPACITY) AS TC_CAPACITY , F.MD_NAME ,'' as TI_INDENT_NO ,'' as IN_INV_NO,'' AS TR_RI_NO , '" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY , substr(B.DF_DTC_CODE,0,4)FD_FEEDER_CODE , ";
                    strQry += " (SELECT DISTINCT FD_FEEDER_NAME FROM  TBLFEEDERMAST WHERE FD_FEEDER_CODE=substr(B.DF_DTC_CODE,0,4))FD_FEEDER_NAME , CASE B.DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE , ";
                    strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=C.TC_MAKE_ID)TC_MAKE_ID ,D.WO_DATE AS WO_DATE,'' AS TI_INDENT_DATE,'' AS IN_DATE, '' AS TR_RI_DATE , '" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY, ";
                    strQry += " (SELECT CM_CIRCLE_NAME FROM TBLCIRCLE WHERE CM_CIRCLE_CODE = substr(B.DF_LOC_CODE,0,1)) CIRCLE  ,D.DIV DIVISION, D.SUBDIVSION SUBDIVISION , D.OMSECTION SECTION ";
                    strQry += " FROM  MV_WORKFLOWSTATUSDUMMY A  , TBLDTCFAILURE B , TBLTCMASTER C , MV_VIEWPENDINGFAILURE D ,TBLWORKORDER E , TBLMASTERDATA F ";
                    strQry += " WHERE F.MD_ID = b.DF_FAILURE_TYPE and   B.DF_ID = E.WO_DF_ID and  A.WO_DATA_ID = D.DT_CODE AND D.DF_ID = B.DF_ID    AND B.DF_EQUIPMENT_ID = C.TC_CODE AND ";
                    if (objReport.sReportType == "3")
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(D.TR_RI_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    else
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(B.DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(B.DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(B.DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(B.DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(B.DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(B.DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    strQry += "AND A.CRREPORT IS  NULL AND a.RIAPPROVE IS  NOT NULL AND   A.WO_BO_ID  <>10 AND   A.OFFCODE LIKE '" + sOfficeCode + "%' AND   F.MD_TYPE= 'FT' ";
                    if (objReport.sMake != null)
                    {
                        strQry += "AND .TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if ((objReport.sFailureType ?? "").Length>0)
                    {
                        strQry += "AND B.DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                }
                if (objReport.sType == "8")
                {
                    strQry += "SELECT TO_CHAR(TC_CODE)TC_CODE,DF_DTC_CODE DT_CODE,DF_LOC_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION,TC_CAPACITY,MD_NAME,WO_NO,'' as TI_INDENT_NO ,'' as IN_INV_NO,'' as TR_RI_NO,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                    strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID,WO_DATE AS WO_DATE,'' AS TI_INDENT_DATE,'' AS IN_DATE,'' AS TR_RV_DATE, '' AS TR_RI_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                    strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                    strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                    strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,TBLMASTERDATA,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCMAST where  DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE and DF_REPLACE_FLAG=1 AND ";
                    if (objReport.sReportType == "3")
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(TR_CR_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(TR_CR_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(TR_CR_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(TR_CR_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(TR_CR_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(TR_CR_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    else
                    {
                        if (objReport.sTodate == null && (objReport.sFromDate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                        }
                        if (objReport.sFromDate == null && (objReport.sTodate != null))
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                        }
                        if (objReport.sFromDate == null && objReport.sTodate == null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                        }
                        if (objReport.sFromDate != null && objReport.sTodate != null)
                        {
                            strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                        }
                    }
                    strQry += "AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                    if (objReport.sMake != null)
                    {
                        strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if ((objReport.sFailureType ?? "").Length > 0)
                    {
                        strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    }
                    if (objReport.sGuranteeType != null)
                    {
                        strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    }
                    strQry += "AND DF_FAILURE_TYPE=MD_ID AND DF_ID=TBLWORKORDER.WO_DF_ID AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO=TR_IN_NO AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ")";
                }
                if (objReport.sType == "9")
                {
                    strQry += "SELECT CASE WHEN DF_REPLACE_FLAG='0' THEN 'Pending' ELSE 'Completed' END STATUS,TO_CHAR(TC_CODE)TC_CODE,DF_DTC_CODE DT_CODE,DF_LOC_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION,TC_CAPACITY,MD_NAME,'' as WO_NO,'' as TI_INDENT_NO ,'' as IN_INV_NO,'' AS TR_RI_NO,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                    strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID,'' AS WO_DATE,'' AS TI_INDENT_DATE,'' AS IN_DATE, '' AS TR_RI_DATE,'' AS TR_RV_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                    strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                    strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                    strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from ";
                    strQry += " TBLTCMASTER left join  TBLDTCFAILURE on DF_EQUIPMENT_ID = TC_CODE  left join TBLDTCMAST on DT_CODE = DF_DTC_CODE  ";
                    strQry += " left join TBLMASTERDATA on DF_FAILURE_TYPE = MD_ID  left join TBLWORKORDER on DF_ID = WO_DF_ID ";
                    strQry += " left join TBLINDENT on TI_WO_SLNO = WO_SLNO left join  TBLDTCINVOICE on IN_TI_NO = TI_ID LEFT JOIN TBLTCDRAWN ON TD_DF_ID = DF_ID  LEFT JOIN  TBLTCREPLACE ON TR_IN_NO = IN_NO where    ";
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    }
                    strQry += "AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                    if (objReport.sMake != null)
                    {
                        strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if (objReport.sFailureType != null && objReport.sFailureType != "")
                    {
                        strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    }
                    if (objReport.sGuranteeType != null)
                    {
                        strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    }
                    strQry += "AND  MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") ";
                }
                // detailed report 
                if (objReport.sType == "10")
                {
                    // query to get records after Estimation is completed 
                    strQry = " SELECT    DT_CODE, DT_NAME , TC_CODE ,TO_CHAR(TC_MANF_DATE,'dd-MON-yyyy') TC_MANF_DATE , TC_CAPACITY , FD_FEEDER_CODE ,	FD_FEEDER_NAME  ,FC_NAME , (select FT_NAME from TBLFDRTYPE,TBLFEEDERMAST where  FT_ID=FD_FEEDER_TYPE and FD_FEEDER_CODE=DT_FDRSLNO AND ROWNUM=1)FEEDER_TYPE ,to_char(DF_LOC_CODE) as DF_LOC_CODE ,";
                    strQry += " TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE, MD_NAME ,  CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE, DF_GUARANTY_TYPE as Guaranty_Type ,  WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION, WO_NO,  WO_NO_DECOM ,  TO_CHAR(WO_DATE,'dd-MON-yy') WO_DATE ,  ";
                    strQry += " TI_INDENT_NO , TO_CHAR(TI_INDENT_DATE,'dd-MON-yy') TI_INDENT_DATE, IN_MANUAL_INVNO as IN_INV_NO, TO_CHAR(IN_DATE,'dd-MON-yy') IN_DATE , TD_TC_NO as Invoiced_DTr,  TR_RI_NO ,  TO_CHAR(TR_RI_DATE,'dd-MON-yy') TR_RI_DATE , TR_RV_NO  , TO_CHAR(TR_RV_DATE,'dd-MON-yy') TR_RV_DATE ,  TO_CHAR(TR_CR_DATE,'dd-MON-yy') TR_CR_DATE , ";
                    strQry += " CASE WHEN DF_REPLACE_FLAG='0' THEN 'Pending' ELSE 'Completed' END STATUS  ,";
                    strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID, '" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE, TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY , ";
                    strQry += " '" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY , ";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,1)) CIRCLE , ";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2))  DIVISION ,";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION , ";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION ";
                    strQry += " FROM TBLDTCFAILURE  left join TBLWORKORDER on DF_ID = WO_DF_ID left join TBLINDENT on WO_SLNO  = TI_WO_SLNO left join TBLDTCINVOICE on TI_ID = IN_TI_NO LEFT JOIN TBLTCDRAWN ON DF_ID = TD_DF_ID ";
                    strQry += "left join TBLTCREPLACE on TR_IN_NO = IN_NO left join TBLTCMASTER on DF_EQUIPMENT_ID = TC_CODE left join TBLDTCMAST on DT_CODE = DF_DTC_CODE ";
                    strQry += " left join TBLMASTERDATA on MD_ID = DF_FAILURE_TYPE left join  TBLFEEDERMAST on FD_FEEDER_CODE = DT_FDRSLNO  left join TBLFEEDERCATEGORY on FD_FC_ID = FC_ID  WHERE MD_TYPE = 'FT' AND  ";
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " TO_DATE(TO_CHAR(DF_DATE,'YYYY/MM/DD'),'YYYY/MM/DD')>= TO_DATE('" + objReport.sFromDate + "','YYYY/MM/DD') and TO_DATE(TO_CHAR(DF_DATE,'YYYY/MM/DD'),'YYYY/MM/DD')<=TO_DATE(TO_CHAR(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD')";
                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += " TO_DATE(TO_CHAR(DF_DATE,'YYYY/MM/DD'),'YYYY/MM/DD')<=TO_DATE('" + objReport.sTodate + "','YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += " TO_DATE(TO_CHAR(DF_DATE,'YYYY/MM/DD'),'YYYY/MM/DD')<=TO_DATE(TO_CHAR(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " TO_DATE(TO_CHAR(DF_DATE,'YYYY/MM/DD'),'YYYY/MM/DD')>= TO_DATE('" + objReport.sFromDate + "','YYYY/MM/DD') AND TO_DATE(TO_CHAR(DF_DATE,'YYYY/MM/DD'),'YYYY/MM/DD')<=TO_DATE('" + objReport.sTodate + "','YYYY/MM/DD')  ";
                    }
                    if (objReport.sMake != null)
                    {
                        strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if (objReport.sFailureType != null && objReport.sFailureType != "")
                    {
                        strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    }
                    if (objReport.sGuranteeType != null)
                    {
                        strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    }
                    strQry += " AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") AND DF_LOC_CODE like  '" + objReport.sOfficeCode + "%' ORDER BY DF_DATE ,DF_LOC_CODE  ";
                    dtFailureDetails = ObjCon.getDataTable(strQry);
                    // query to get the estimation records  
                    strQry = " SELECT WO_DATA_ID as DT_CODE,DT_NAME ,TO_CHAR(TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(),'dd-MM-yyyy HH24:mi:ss'),'dd-MON-yy') as DF_DATE, CASE XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_STATUS_FLAG/text()').getStringVal() WHEN '1' THEN 'FAIL' WHEN '2' THEN 'GD ENC' WHEN '4' THEN 'FAIL ENC' END FAILURE_TYPE, ";
                    strQry += " XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//GUARENTEE/text()').getStringVal() as GUARANTY_TYPE ,TO_CHAR( XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_LOC_CODE/text()').getStringVal()) ";
                    strQry += " As DF_LOC_CODE  ,MD_NAME,TC_CODE, TC_CAPACITY,FD_FEEDER_CODE,FD_FEEDER_NAME,FC_NAME  , '" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE, TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY , 'Pending'as STATUS,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID, '' AS REPORT_TYPE, FT_NAME as FEEDER_TYPE,'10' AS REPORT_CATEGORY, ";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(TO_CHAR( XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_LOC_CODE/text()').getStringVal()),0,1)) CIRCLE, (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) ";
                    strQry += " from VIEW_ALL_OFFICES where OFF_CODE=substr(TO_CHAR( XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_LOC_CODE/text()').getStringVal()),0,2)) DIVISION  , (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(TO_CHAR( XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_LOC_CODE/text()').getStringVal()),0,3)) SUBDIVISION , ";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(TO_CHAR( XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_LOC_CODE/text()').getStringVal()),0,4)) SECTION ";
                    strQry += " FROM TBLFDRTYPE,TBLWORKFLOWOBJECTS, TBLWFODATA, TBLTCMASTER, TBLDTCMAST, TBLFEEDERMAST, TBLFEEDERCATEGORY, TBLMASTERDATA WHERE  FT_ID=FD_FEEDER_TYPE and WO_WFO_ID = WFO_ID AND FD_FC_ID = FC_ID AND FD_FEEDER_CODE = DT_FDRSLNO AND to_char(MD_ID) = to_char(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_FAILURE_TYPE/text()').getStringVal())and MD_TYPE = 'FT'  AND  WO_DATA_ID = DT_CODE AND DT_TC_ID = TC_CODE AND WO_BO_ID IN(9) AND WO_RECORD_ID < 0  ";
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += "  AND TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss') >= to_date('" + objReport.sFromDate + "', 'yyyy-MM-dd')";
                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += " AND TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss') <= to_date('" + objReport.sTodate + "', 'yyyy-MM-dd')";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += "and TO_CHAR(TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss'), 'YYYY/MM/DD')<= TO_CHAR(SYSDATE, 'YYYY/MM/DD')";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " AND TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss') >= to_date('" + objReport.sFromDate + "', 'yyyy/MM/dd')   AND TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss') <= to_date('" + objReport.sTodate + "', 'yyyy/MM/dd')   ";
                    }
                    if (objReport.sMake != null)
                    {
                        strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if (objReport.sFailureType != null && objReport.sFailureType != "")
                    {
                        strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    }
                    if (objReport.sGuranteeType != null)
                    {
                      //  strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                        strQry += " AND TO_CHAR(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//GUARENTEE/text()').getStringVal()) ='" + objReport.sGuranteeType + "' ";
                    }
                    strQry += " AND TO_CHAR(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_LOC_CODE/text()').getStringVal()) like   '" + objReport.sOfficeCode + "%' ";
                    strQry += " AND TO_CHAR(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_STATUS_FLAG/text()').getStringVal()) IN (" + objReport.sSelectedFailureType + ") ";
                    strQry += " GROUP BY TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss'), DT_NAME, WO_DATA_ID, CASE XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_STATUS_FLAG/text()').getStringVal()";
                    strQry += "   WHEN '1' THEN 'FAIL' WHEN '2' THEN 'GD ENC' WHEN '4' THEN 'FAIL ENC' END, XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//GUARENTEE/text()').getStringVal(), TO_CHAR(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_LOC_CODE/text()').getStringVal()), ";
                    strQry += "FT_NAME ,TC_CODE, TC_CAPACITY, FD_FEEDER_CODE, FD_FEEDER_NAME, FC_NAME, MD_NAME, TC_MAKE_ID";
                    DataTable dt = new DataTable();
                    dt = ObjCon.getDataTable(strQry);
                    dtFailureDetails.Merge(dt);
                    return dtFailureDetails;
                    #region old query 

                    // old one  in which estimation count was not coming .
                    //strQry = " SELECT    DT_CODE, DT_NAME , TC_CODE  , TC_CAPACITY , FD_FEEDER_CODE ,	FD_FEEDER_NAME  ,FC_NAME , DF_LOC_CODE  ,";
                    //strQry += " TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE, MD_NAME ,  CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE, DF_GUARANTY_TYPE as Guaranty_Type ,  WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION, WO_NO,  WO_NO_DECOM ,  TO_CHAR(WO_DATE,'dd-MON-yy') WO_DATE ,  ";
                    //strQry += " TI_INDENT_NO , TO_CHAR(TI_INDENT_DATE,'dd-MON-yy') TI_INDENT_DATE ,  IN_INV_NO, TO_CHAR(IN_DATE,'dd-MON-yy') IN_DATE ,  TR_RI_NO ,  TO_CHAR(TR_RI_DATE,'dd-MON-yy') TR_RI_DATE , TR_RV_NO  , TO_CHAR(TR_RV_DATE,'dd-MON-yy') TR_RV_DATE ,  TO_CHAR(TR_CR_DATE,'dd-MON-yy') TR_CR_DATE , ";
                    //strQry += " CASE WHEN DF_REPLACE_FLAG='0' THEN 'Pending' ELSE 'Completed' END STATUS  ,";
                    //strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID, '" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE, TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY , ";
                    //strQry += " '" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY , ";
                    //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,1)) CIRCLE , ";
                    //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2))  DIVISION ,";
                    //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION , ";
                    //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION ";
                    //strQry += " FROM TBLDTCFAILURE  left join TBLWORKORDER on DF_ID = WO_DF_ID left join TBLINDENT on WO_SLNO  = TI_WO_SLNO left join TBLDTCINVOICE on TI_ID = IN_TI_NO ";
                    //strQry += "left join TBLTCREPLACE on TR_IN_NO = IN_NO left join TBLTCMASTER on DF_EQUIPMENT_ID = TC_CODE left join TBLDTCMAST on DT_CODE = DF_DTC_CODE ";
                    //strQry += " left join TBLMASTERDATA on MD_ID = DF_FAILURE_TYPE left join  TBLFEEDERMAST on FD_FEEDER_CODE = DT_FDRSLNO  left join TBLFEEDERCATEGORY on FD_FC_ID = FC_ID  WHERE MD_TYPE = 'FT' AND  ";


                    //if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //{
                    //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";

                    //}
                    //if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //{
                    //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //}
                    //if (objReport.sFromDate == null && objReport.sTodate == null)
                    //{
                    //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate != null && objReport.sTodate != null)
                    //{
                    //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    //}

                    //if (objReport.sMake != null)
                    //{
                    //    strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    //}

                    //strQry += " AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") AND DF_LOC_CODE like  '"+objReport.sOfficeCode+"%' ORDER BY DF_DATE ,DF_LOC_CODE  ";





                    //strQry = "SELECT CASE WHEN DF_REPLACE_FLAG='0' THEN 'Pending' ELSE 'Completed' END STATUS,TO_CHAR(TC_CODE)TC_CODE,DF_DTC_CODE DT_CODE,DF_LOC_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,WO_AMT AS COMMISSION,WO_AMT_DECOM AS DECOMMISSION,TC_CAPACITY,MD_NAME, WO_NO, TI_INDENT_NO , IN_INV_NO, TR_RI_NO,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME FROM ";
                    //strQry += " TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,CASE DF_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID, WO_DATE, TI_INDENT_DATE, IN_DATE,  TR_RI_DATE, TR_RV_DATE,'" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY,";
                    //strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr";
                    //strQry += "(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) ";
                    //strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION,";
                    //strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from ";
                    //strQry += " TBLTCMASTER left join  TBLDTCFAILURE on DF_EQUIPMENT_ID = TC_CODE  left join TBLDTCMAST on DT_CODE = DF_DTC_CODE  ";
                    //strQry += " left join TBLMASTERDATA on DF_FAILURE_TYPE = MD_ID  left join TBLWORKORDER on DF_ID = WO_DF_ID ";
                    //strQry += " left join TBLINDENT on TI_WO_SLNO = WO_SLNO left join  TBLDTCINVOICE on IN_TI_NO = TI_ID LEFT JOIN TBLTCDRAWN ON TD_DF_ID = DF_ID  LEFT JOIN  TBLTCREPLACE ON TR_IN_NO = IN_NO where    ";
                    //strQry += "  DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") AND   MD_TYPE='FT' AND   ";


                    ////TBLTCMASTER,TBLDTCFAILURE,TBLDTCMAST,TBLMASTERDATA,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE where  DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE AND ";
                    ////strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD') BETWEEN '" + sFromDate + "' AND '" + sTodate + "'


                    //if (objReport.sTodate == null && (objReport.sFromDate != null))
                    //{
                    //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";

                    //}
                    //if (objReport.sFromDate == null && (objReport.sTodate != null))
                    //{
                    //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    //}
                    //if (objReport.sFromDate == null && objReport.sTodate == null)
                    //{
                    //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    //}
                    //if (objReport.sFromDate != null && objReport.sTodate != null)
                    //{
                    //    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    //}


                    //strQry += "AND DF_LOC_CODE LIKE '" + sOfficeCode + "%'";
                    //if (objReport.sMake != null)
                    //{
                    //    strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    //}
                    //if (objReport.sFailureType != null)
                    //{
                    //    strQry += "AND DF_FAILURE_TYPE='" + objReport.sFailureType + "'";
                    //}
                    //if (objReport.sCapacity != null)
                    //{
                    //    strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    //}
                    //if (objReport.sGuranteeType != null)
                    //{
                    //    strQry += "AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "' ";
                    //}

                    //strQry += "AND  MD_TYPE='FT' AND DF_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") ";

                    #endregion
                }
                if (objReport.sType == "11")
                {
                    strQry += " SELECT  'Deleted' as STATUS,TO_CHAR(TC_CODE)TC_CODE ,DFT_WO_AMT AS COMMISSION,DFT_WO_DECOM AS DECOMMISSION,DT_CODE,DFT_OFFICE_CODE,TO_CHAR(DFT_DF_CRON,'dd-MM-yy')DF_DATE,TC_CAPACITY, ";
                    strQry += " MD_NAME,DFT_WO_NO as WO_NO,TO_CHAR(DFT_INDENT_NO) as TI_INDENT_NO ,TO_CHAR(DFT_INVOICE_NO)  as IN_INV_NO,TO_CHAR(DFT_RI_NO) AS TR_RI_NO,'" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE, ";
                    strQry += " TO_CHAR(SYSDATE,'DD/MM/YYYY')TODAY,substr(DFT_DTC_CODE,0,4)FD_FEEDER_CODE,(SELECT DISTINCT FD_FEEDER_NAME ";
                    strQry += " FROM  TBLFEEDERMAST WHERE FD_FEEDER_CODE=DT_FDRSLNO)FD_FEEDER_NAME,CASE DFT_STATUS_FLAG WHEN 1 THEN 'FAIL' WHEN 2 THEN ";
                    strQry += " 'GD ENC' WHEN 4 THEN 'FAIL ENC' END FAILURE_TYPE,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID, ";
                    strQry += " TO_CHAR(DFT_WO_CRON,'dd-MM-yy') AS WO_DATE,TO_CHAR(DFT_INDENT_CRON,'dd-MM-yy') AS TI_INDENT_DATE,TO_CHAR(DFT_INVOICE_CRON,'dd-MM-yy') ";
                    strQry += " AS IN_DATE,TO_CHAR(DFT_RI_CRON,'dd-MM-yy') AS TR_RI_DATE,TO_CHAR(DFT_RV_DATE,'dd-MM-yy') AS TR_RV_DATE,DFT_RV_NO as TR_RV_NO,TO_CHAR(DFT_DATE,'dd-MM-yy') AS DELETE_DATE,";
                    strQry += " '" + objReport.sReportType + "' AS REPORT_TYPE,'" + objReport.sType + "' AS REPORT_CATEGORY, (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DFT_DTC_CODE)DT_NAME,";
                    strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DFT_OFFICE_CODE,0,1)) ";
                    strQry += " CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DFT_OFFICE_CODE,0,2)) ";
                    strQry += " DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DFT_OFFICE_CODE,0,3)) ";
                    strQry += " SUBDIVISION, (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  from VIEW_ALL_OFFICES where OFF_CODE=substr(DFT_OFFICE_CODE,0,4))";
                    strQry += " SECTION from  TBLTCMASTER left join  TBLDELETEFAILURETRANSACTION on DFT_DTR_CODE = TC_CODE  left join TBLDTCMAST ";
                    strQry += " on DT_CODE = DFT_DTC_CODE   left join TBLMASTERDATA on DFT_FAIL_TYPE = MD_ID  where  ";
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " TO_CHAR(DFT_DF_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DFT_DF_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += " TO_CHAR(DFT_DF_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += " TO_CHAR(DFT_DF_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " TO_CHAR(DFT_DF_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DFT_DF_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    }
                    strQry += "AND DFT_OFFICE_CODE LIKE '" + sOfficeCode + "%'";
                    if (objReport.sMake != null)
                    {
                        strQry += "AND TC_MAKE_ID='" + objReport.sMake + "'";
                    }
                    if (objReport.sFailureType != null)
                    {
                        strQry += "AND DFT_FAIL_TYPE='" + objReport.sFailureType + "'";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += "AND TC_CAPACITY='" + objReport.sCapacity + "' ";
                    }
                    strQry += "AND  MD_TYPE='FT' AND DFT_STATUS_FLAG IN (" + objReport.sSelectedFailureType + ") ";
                }
                strQry += ")A  ORDER BY TO_DATE(DF_DATE,'DD-MM-YY')";
                dtFailureDetails = ObjCon.getDataTable(strQry);
                return dtFailureDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtFailureDetails;

            }
        }
        /// <summary>
        /// CR Abstract
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable CRAbstract(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable CRAbstract = new DataTable();
            try
            {
                strQry = "select to_char(TM_MAPPING_DATE,'dd-MON-yyyy')TM_MAPPING_DATE,(SELECT substr(off_name,instr(off_name,':')+1) FROM VIEW_ALL_OFFICES WHERE";
                strQry += " OFF_CODE=substr(df_loc_code,0,1))CIRCLE,(SELECT substr(off_name,instr(off_name,':')+1)";
                strQry += "FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,2))DIVISION,";
                strQry += "(SELECT substr(off_name,instr(off_name,':')+1) FROM VIEW_ALL_OFFICES WHERE ";
                strQry += "OFF_CODE=substr(df_loc_code,0,3))SUBDIVISION,(SELECT substr(off_name,instr(off_name,':')+1)";
                strQry += "FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,4))SECTION, '" + objReport.sFromDate + "' AS FROMDATE,";
                strQry += "(CASE WHEN DF_ENHANCE_CAPACITY IS NULL THEN 'FAILURE' WHEN DF_ENHANCE_CAPACITY IS ";
                strQry += "NOT NULL THEN 'ENHANCEMENT' END)NOMENCLATURE,DF_DTC_CODE,TO_CHAR(DF_EQUIPMENT_ID)DF_EQUIPMENT_ID, TO_CHAR( TR_COMM_DATE,'DD-MON-YYYY') as TR_COMM_DATE  , TO_CHAR(DF_DATE,'DD-MON-YYYY') as  FailureDate ,   WO_NO,";
                strQry += "WO_NO_DECOM ,TO_CHAR( WO_DATE,'DD-MON-YYYY')WO_DATE,EST_NO,'" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate,";
                strQry += "TO_CHAR( EST_CRON,'DD-MON-YYYY')EST_CRON, TI_INDENT_NO,";
                strQry += "TO_CHAR( TI_INDENT_DATE,'DD-MON-YYYY')TI_INDENT_DATE, IN_INV_NO,";
                strQry += "TO_CHAR( IN_DATE,'DD-MON-YYYY')IN_DATE,TR_RI_NO, TO_CHAR( TR_RI_DATE,'DD-MON-YYYY')TR_RI_DATE,";
                strQry += "TR_RV_NO,TO_CHAR( TR_RV_DATE,'DD-MON-YYYY')TR_RV_DATE  , TO_CHAR( TR_CR_DATE,'DD-MON-YYYY') as CR_DATE  ";
                strQry += "from TBLDTCFAILURE INNER JOIN TBLESTIMATION ON DF_ID=EST_DF_ID INNER JOIN TBLWORKORDER on df_id=WO_DF_ID ";
                strQry += "INNER JOIN TBLINDENT on wo_slno=ti_wo_slno INNER JOIN TBLDTCINVOICE on in_ti_no=ti_id ";
                strQry += "INNER JOIN TBLTCREPLACE on tr_in_no=in_no INNER JOIN TBLTCMASTER ON DF_EQUIPMENT_ID=TC_CODE INNER JOIN TBLDTCMAST ON ";
                strQry += "DF_DTC_CODE =DT_CODE INNER JOIN TBLTRANSDTCMAPPING ON TM_DTC_ID=df_dtc_code AND TM_LIVE_FLAG='1' where ";
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += "AND DF_LOC_CODE LIKE '" + sOfficeCode + "%' AND DF_REPLACE_FLAG=1 AND DF_STATUS_FLAG IN (1,4) ORDER BY CIRCLE,DIVISION  ";
                CRAbstract = ObjCon.getDataTable(strQry);
                return CRAbstract;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return CRAbstract;
            }
        }
        /// <summary>
        /// transformer performance
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PENDINGWTREPARIER(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtPendingwtreparier = new DataTable();
            try
            {
                strQry = " SELECT Cirlce,Circle_Code,Duration,Div_Code,Division_Name,Upto_25,from26_63,from64_100,from101_200, ";
                strQry += " from201_250,Above_250,FROMDATE,TODATE,currentdate,sum(Upto_25 + from26_63 + from64_100 + ";
                strQry += " from101_200 + from201_250 + Above_250)Total FROM( SELECT CM_CIRCLE_NAME as Cirlce, ";
                strQry += " CM_CIRCLE_CODE AS Circle_Code,Duration ,OFF_CODE AS Div_Code,OFF_NAME AS Division_Name,";
                strQry += " to_char(UPTO25) AS Upto_25, to_char(A2663) AS from26_63,to_char(B64100) AS from64_100,to_char(C101200) AS ";
                strQry += " from101_200, to_char(D201250) AS from201_250,to_char(ABOVE250)  AS Above_250, ";
                strQry += " '" + objReport.sTempFromDate + "' AS FROMDATE, '" + sTempTodate + "' AS TODATE, ";
                strQry += " TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate FROM TBLCIRCLE ,";
                strQry += " (SELECT md_name AS Duration,OFF_CODE,substr(OFF_NAME,instr(OFF_NAME,':')+1) as ";
                strQry += " OFF_NAME,UPTO25,A2663,B64100,C101200,D201250,ABOVE250,MD_ID FROM (SELECT *  FROM TBLMASTERDATA , ";
                strQry += " VIEW_ALL_OFFICES WHERE MD_TYPE='RD' AND LENGTH(OFF_CODE)=2 ORDER BY MD_ID)A LEFT JOIN ( ";
                strQry += " SELECT ";
                strQry += " CASE WHEN (ROUND(SYSDATE - RSM_ISSUE_DATE)) BETWEEN 0 AND 30 THEN 'WITHIN 30 DAYS' WHEN ";
                strQry += " (ROUND(SYSDATE - RSM_ISSUE_DATE)) BETWEEN 31 AND 60 THEN '30-60 DAYS' WHEN (ROUND(SYSDATE - RSM_ISSUE_DATE))";
                strQry += " BETWEEN 61 AND 90 THEN '60-90 DAYS' WHEN (ROUND(SYSDATE - RSM_ISSUE_DATE)) > 90 THEN 'MORE THAN 90' END AS ";
                strQry += " CONDITION1, SUM(CASE WHEN TC_CAPACITY BETWEEN 0 AND 25 THEN 1 ELSE 0 END) UPTO25, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 26 AND 63 THEN 1 ELSE 0 END) A2663, SUM(CASE WHEN TC_CAPACITY BETWEEN 64 ";
                strQry += " AND 100 THEN 1 ELSE 0 END)  B64100, SUM(CASE WHEN TC_CAPACITY BETWEEN 101 AND 200 THEN 1 ELSE 0 END) ";
                strQry += " C101200, SUM(CASE WHEN TC_CAPACITY BETWEEN 201 AND 250 THEN 1 ELSE 0 END) D201250, SUM(CASE WHEN TC_CAPACITY > 250 ";
                strQry += " THEN 1 ELSE 0 END) ABOVE250,RSM_DIV_CODE FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE ";
                strQry += " RSM_ID = RSD_RSM_ID AND   TC_CODE = RSD_TC_CODE AND RSD_DELIVARY_DATE IS  NULL ";
                if (objReport.sRepriername != null)
                {
                    strQry += "  AND RSM_SUPREP_ID='" + objReport.sRepriername + "'  ";
                }

                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " AND  TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " And TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";

                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " AND  TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == "" && objReport.sTodate == "")
                {
                    strQry += " AND TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if ((objReport.sFromDate != "") && (objReport.sTodate != ""))
                {
                    strQry += " AND TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                strQry += " GROUP BY CASE ";
                strQry += " WHEN (ROUND(SYSDATE - RSM_ISSUE_DATE)) BETWEEN 0 AND 30 THEN 'WITHIN 30 DAYS' ";
                strQry += " WHEN (ROUND(SYSDATE - RSM_ISSUE_DATE)) BETWEEN 31 AND 60 THEN '30-60 DAYS' ";
                strQry += " WHEN (ROUND(SYSDATE - RSM_ISSUE_DATE)) BETWEEN 61 AND 90 THEN '60-90 DAYS' ";
                strQry += " WHEN (ROUND(SYSDATE - RSM_ISSUE_DATE)) > 90 THEN 'MORE THAN 90' END,RSM_DIV_CODE)B ";
                strQry += " ON CONDITION1=MD_NAME AND OFF_CODE=RSM_DIV_CODE)d WHERE CM_CIRCLE_CODE = substr(D.OFF_CODE,0,1) ";
                strQry += " and  OFF_CODE like '" + objReport.sOfficeCode + "%') GROUP BY Cirlce,Circle_Code,Duration,Div_Code, ";
                strQry += " Division_Name, Upto_25,from26_63,from64_100,from101_200,from201_250, ";
                strQry += " Above_250,FROMDATE,TODATE,currentdate ORDER BY Div_Code";
                dtPendingwtreparier = ObjCon.getDataTable(strQry);
                //To Remove the section which doesn't have a single values in that section
                int globalindex = -1;
                int rowcount = dtPendingwtreparier.Rows.Count / 4;
                for (int j = 0; j < rowcount; j++)
                {
                    bool delete = true;
                    for (int i = 0; i < 4; i++)
                    {
                        globalindex++;

                        if (Convert.ToString(dtPendingwtreparier.Rows[globalindex][5]).Length > 0)
                        {
                            delete = false;

                        }
                    }
                    if (delete)
                    {
                        dtPendingwtreparier.Rows.RemoveAt(globalindex - 3);
                        globalindex--;
                        dtPendingwtreparier.Rows.RemoveAt(globalindex - 2);
                        globalindex--;
                        dtPendingwtreparier.Rows.RemoveAt(globalindex - 1);
                        globalindex--;
                        dtPendingwtreparier.Rows.RemoveAt(globalindex);
                        globalindex--;
                    }
                }
                return dtPendingwtreparier;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtPendingwtreparier;
            }
        }
        /// <summary>
        /// Transformer Wise Details
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable TransformerWiseDetails(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtTransformerWise = new DataTable();
            try
            {
                strQry = " SELECT (SELECT substr(OFF_NAME,instr(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE ";
                strQry += " off_code=substr(RSM_DIV_CODE,0,1)) CIRCLE,substr(OFF_NAME,instr(OFF_NAME,':')+1) as Division,CASE WHEN ";
                strQry += " IND_INSP_DATE IS NOT NULL THEN 'YES' ELSE 'NO'  END TESTING_COMPLETE,(SELECT TR_NAME FROM TBLTRANSREPAIRER ";
                strQry += "  WHERE TR_ID=rsm_suprep_id)REPAIRER_NAME,(SELECT TR_ADDRESS FROM TBLTRANSREPAIRER WHERE TR_ID=rsm_suprep_id)";
                strQry += "  REPAIRER_ADDRESS,TO_CHAR(RSM_ISSUE_DATE,'DD-Mon-YYYY') AS Issued_On,to_char(TC_CODE)TC_CODE,(SELECT TM_NAME FROM";
                strQry += " TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TM_NAME,to_char(TC_CAPACITY)TC_CAPACITY,RSM_GUARANTY_TYPE,RSM_PO_NO,";
                strQry += " TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY')RSM_PO_DATE,'" + objReport.sTempFromDate + "' AS FROMDATE, '" + sTempTodate + "' AS TODATE ,TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate,to_char(ROUND(SYSDATE - RSM_ISSUE_DATE)) AS Pending_Days FROM ";
                strQry += " (SELECT * FROM TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER,TBLTCMASTER,VIEW_ALL_OFFICES WHERE RSM_ID = RSD_RSM_ID";
                strQry += "  AND TC_CODE = RSD_TC_CODE AND RSM_DIV_CODE = off_code and RSD_DELIVARY_DATE IS NULL AND  RSM_DIV_CODE = off_code";
                strQry += "  and RSD_DELIVARY_DATE IS NULL AND  off_code like '" + objReport.sOfficeCode + "%'";
                if (objReport.sRepriername != null)
                {
                    strQry += "  AND RSM_SUPREP_ID='" + objReport.sRepriername + "'  ";
                }

                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";

                }
                if (objReport.sFromDate == "" && (objReport.sTodate != ""))
                {
                    strQry += " AND TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == "" && objReport.sTodate == "")
                {
                    strQry += " AND TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != "" && objReport.sTodate != "")
                {
                    strQry += " AND  TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                strQry += " AND  LENGTH(off_code)=2 ) A LEFT JOIN TBLINSPECTIONDETAILS ON RSD_ID=IND_RSD_ID ";
                strQry += " And nvl(IND_INSP_RESULT,0)=1 ORDER BY OFF_NAME,(SYSDATE - RSM_ISSUE_DATE) DESC ";
                dtTransformerWise = ObjCon.getDataTable(strQry);
                return dtTransformerWise;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtTransformerWise;
            }
        }
        /// <summary>
        /// completed
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable ReperierCompleted(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtreparierComplete = new DataTable();
            try
            {
                strQry = " SELECT Cirlce,Circle_Code,Duration,Div_Code,Division_Name,Upto_25,from26_63,from64_100, ";
                strQry += " from101_200,from201_250,Above_250,FROMDATE,TODATE,currentdate,sum(Upto_25 + from26_63 + ";
                strQry += " from64_100 + from101_200 + from201_250 + Above_250)Total FROM( ";
                strQry += " SELECT CM_CIRCLE_NAME as Cirlce,CM_CIRCLE_CODE AS Circle_Code, Duration ,OFF_CODE AS Div_Code, ";
                strQry += " OFF_NAME AS Division_Name,to_char(UPTO25) AS Upto_25,to_char(A2663)";
                strQry += " AS from26_63,to_char(B64100) AS from64_100,to_char(C101200) AS from101_200, ";
                strQry += " to_char(D201250) AS from201_250,to_char(ABOVE250) AS Above_250,'" + objReport.sFromDate + "' AS FROMDATE, ";
                strQry += " '" + objReport.sTodate + "' AS TODATE ,TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate FROM ";
                strQry += " TBLCIRCLE ,(SELECT md_name AS Duration,OFF_CODE,substr(OFF_NAME,instr(OFF_NAME,':')+1) as OFF_NAME, ";
                strQry += " UPTO25,A2663,B64100,C101200,D201250,ABOVE250,MD_ID FROM ";
                strQry += " (SELECT * FROM TBLMASTERDATA, VIEW_ALL_OFFICES WHERE MD_TYPE='RD' ";
                strQry += " AND LENGTH(OFF_CODE)=2 ORDER BY MD_ID )A LEFT JOIN (SELECT CASE ";
                strQry += " WHEN (ROUND(RSD_DELIVARY_DATE -  RSM_ISSUE_DATE)) BETWEEN 0 AND 30 THEN 'WITHIN 30 DAYS' ";
                strQry += " WHEN (ROUND(RSD_DELIVARY_DATE -  RSM_ISSUE_DATE)) BETWEEN 31 AND 60 THEN '30-60 DAYS' ";
                strQry += " WHEN (ROUND(RSD_DELIVARY_DATE -  RSM_ISSUE_DATE)) BETWEEN 61 AND 90 THEN '60-90 DAYS' ";
                strQry += " WHEN  (ROUND(RSD_DELIVARY_DATE -  RSM_ISSUE_DATE)) > 90 THEN 'MORE THAN 90' END AS CONDITION1, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 0 AND 25 THEN 1 ELSE 0 END)  UPTO25, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 26 AND 63 THEN 1 ELSE 0 END)  A2663, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 64 AND 100 THEN 1 ELSE 0 END)  B64100, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 101 AND 200 THEN 1 ELSE 0 END)  C101200, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 201 AND 250 THEN 1 ELSE 0 END)  D201250, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY > 250 THEN 1 ELSE 0 END)  ABOVE250,RSM_DIV_CODE ";
                strQry += " FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS,TBLTCMASTER ";
                strQry += " WHERE RSM_ID = RSD_RSM_ID AND   TC_CODE = RSD_TC_CODE AND RSD_DELIVARY_DATE IS NOT NULL ";
                if (objReport.sRepriername != null)
                {
                    strQry += "  AND RSM_SUPREP_ID='" + objReport.sRepriername + "'  ";
                }
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " And TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " AND TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " AND TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND  TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                strQry += " GROUP BY CASE ";
                strQry += " WHEN (ROUND(RSD_DELIVARY_DATE -  RSM_ISSUE_DATE)) BETWEEN 0 AND 30 THEN 'WITHIN 30 DAYS' ";
                strQry += " WHEN (ROUND(RSD_DELIVARY_DATE -  RSM_ISSUE_DATE)) BETWEEN 31 AND 60 THEN '30-60 DAYS' ";
                strQry += " WHEN (ROUND(RSD_DELIVARY_DATE -  RSM_ISSUE_DATE)) BETWEEN 61 AND 90 THEN '60-90 DAYS' ";
                strQry += " WHEN  (ROUND(RSD_DELIVARY_DATE -  RSM_ISSUE_DATE)) > 90 THEN 'MORE THAN 90' END, ";
                strQry += " RSM_DIV_CODE)B ON CONDITION1=MD_NAME AND ";
                strQry += " OFF_CODE=RSM_DIV_CODE)d WHERE CM_CIRCLE_CODE =  substr(D.OFF_CODE,0,1) ";
                strQry += " And  OFF_CODE like '" + objReport.sOfficeCode + "%' )";
                strQry += " GROUP BY Cirlce,Circle_Code,Duration,Div_Code,Division_Name,Upto_25,from26_63,from64_100, ";
                strQry += " from101_200,from201_250,Above_250,FROMDATE,TODATE,currentdate ORDER BY Div_Code";
                dtreparierComplete = ObjCon.getDataTable(strQry);
                int globalindex = -1;
                int rowcount = dtreparierComplete.Rows.Count / 4;
                for (int j = 0; j < rowcount; j++)
                {
                    bool delete = true;
                    for (int i = 0; i < 4; i++)
                    {
                        globalindex++;
                        if (Convert.ToString(dtreparierComplete.Rows[globalindex][5]).Length > 0)
                        {
                            delete = false;
                        }
                    }
                    if (delete)
                    {
                        dtreparierComplete.Rows.RemoveAt(globalindex - 3);
                        globalindex--;
                        dtreparierComplete.Rows.RemoveAt(globalindex - 2);
                        globalindex--;
                        dtreparierComplete.Rows.RemoveAt(globalindex - 1);
                        globalindex--;
                        dtreparierComplete.Rows.RemoveAt(globalindex);
                        globalindex--;
                    }
                }
                return dtreparierComplete;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtreparierComplete;
            }
        }
        /// <summary>
        /// Dtr Make Wise
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable Printdtrwise(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT CASE  WHEN ROWNUM <'" + objReport.sMake + "' THEN TM_NAME ELSE 'OTHERS' END TM_NAME, ";
                strQry += " TCCOUNT,FCOUNT,FAILUREPERCENTAGE, '" + objReport.sFromDate + "' AS FROMDATE, ";
                strQry += " '" + objReport.sTodate + "' AS TODATE, TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate, ";
                strQry += " ROWNUM FROM TBLTRANSMAKES A, (SELECT TC_MAKE_ID , COUNT(DISTINCT(TC_CODE)) TCCOUNT, ";
                strQry += " COUNT(DF_EQUIPMENT_ID) FCOUNT, ROUND ((COUNT(DF_EQUIPMENT_ID)/COUNT(DISTINCT(TC_CODE))) * 100) FAILUREPERCENTAGE ";
                strQry += " FROM TBLTCMASTER LEFT OUTER JOIN TBLDTCFAILURE ON DF_EQUIPMENT_ID = TC_CODE ";
                strQry += " AND  DF_LOC_CODE LIKE '" + objReport.sOfficeCode + "%' AND ";
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " WHERE NVL(TC_CODE,0)!=0 and DF_EQUIPMENT_ID!=0  GROUP BY TC_MAKE_ID ";
                strQry += " ORDER BY COUNT(DF_EQUIPMENT_ID) DESC) B WHERE A.TM_ID= B.TC_MAKE_ID ";
                strQry += " AND ROWNUM <='" + objReport.sMake + "' ORDER BY FCOUNT DESC ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtRDetailedReport.Load(dr);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtRDetailedReport;
            }
        }
        /// <summary>
        /// MakeWise Details
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintMakeWiseDetails(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT UPTO25,A2663,B64100, C101200, D201250, ABOVE250,(CONT_FAIL_ID) AS FCOUNT, ";
                strQry += " (TOTAL_TC_COUNT) AS TCOUNT,'' AS FROMDATE,'' AS TODATE, TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate, ";
                strQry += " (ROUND((NVL(CONT_FAIL_ID,0)/NVL(TOTAL_TC_COUNT,0))*100)) AS PER,TM_NAME as MakeName,";
                strQry += " CIRCLE,DIVISION,  SUBDIVISION,SECTION FROM TBLTRANSMAKES A, (SELECT SUM(CASE WHEN TC_CAPACITY  ";
                strQry += " BETWEEN 0 AND 25 THEN 1 ELSE 0 END) UPTO25, SUM(CASE WHEN TC_CAPACITY BETWEEN 26 AND 63 THEN 1 ELSE 0 END)  A2663, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 64 AND 100 THEN 1 ELSE 0 END) B64100, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 101 AND 200 THEN 1 ELSE 0 END) C101200, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 201 AND 250 THEN 1 ELSE 0 END) D201250, ";
                strQry += " SUM(CASE WHEN  TC_CAPACITY>250 THEN 1 ELSE 0 END ) ABOVE250, ";
                strQry += " TC_MAKE_ID, COUNT(DF_EQUIPMENT_ID) CONT_FAIL_ID, ";
                strQry += " (SELECT substr(off_name,instr(off_name,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,1))CIRCLE, ";
                strQry += " (SELECT substr(off_name,instr(off_name,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,2))DIVISION,";
                strQry += " (SELECT substr(off_name,instr(off_name,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,3))SUBDIVISION, ";
                strQry += " (SELECT substr(off_name,instr(off_name,':')+1)FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,4))SECTION, ";
                strQry += " COUNT(DISTINCT(TC_CODE)) TOTAL_TC_COUNT FROM TBLTCMASTER ";
                strQry += " LEFT JOIN TBLDTCFAILURE ON DF_EQUIPMENT_ID=TC_CODE INNER JOIN TBLTRANSMAKES ON ";
                strQry += " TM_ID = TC_MAKE_ID AND DF_LOC_CODE LIKE '" + objReport.sOfficeCode + "%' AND ";

                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                strQry += " WHERE NVL(DF_EQUIPMENT_ID,0)!=0 AND NVL(TC_CODE,0)!=0 GROUP BY TM_NAME,TC_MAKE_ID,df_loc_code)B ";
                strQry += " WHERE A.TM_ID= B.TC_MAKE_ID  ORDER BY CONT_FAIL_ID DESC ";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtRDetailedReport.Load(dr);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtRDetailedReport;
            }
        }
        /// <summary>
        /// DtrRepairerWise Details
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintDtrRepairerWise(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT TR_OFFICECODE,(SELECT substr(off_name,instr(off_name,':')+1) FROM VIEW_ALL_OFFICES ";
                strQry += " WHERE OFF_CODE=substr(TR_OFFICECODE,0,1))CIRCLE,'" + objReport.sFromDate + "' AS FROMDATE, ";
                strQry += " '" + objReport.sTodate + "' AS TODATE,TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate, ";
                strQry += " (SELECT substr(off_name,instr(off_name,':')+1)  FROM VIEW_ALL_OFFICES ";
                strQry += " WHERE OFF_CODE=substr(TR_OFFICECODE,0,2))DIVISION,TR_NAME, SUM(CASE WHEN TC_CAPACITY ";
                strQry += " BETWEEN 0 AND 25 THEN 1 ELSE 0 END)  UPTO25, SUM(CASE WHEN TC_CAPACITY BETWEEN 26 AND 63 THEN 1 ELSE 0 END) ";
                strQry += " A2663,  SUM(CASE WHEN TC_CAPACITY BETWEEN 64 AND 100 THEN 1 ELSE 0 END)  B64100, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY BETWEEN 101 AND 200  THEN 1 ";
                strQry += " ELSE 0 END)  C101200, SUM(CASE WHEN TC_CAPACITY BETWEEN 201 AND 250 THEN 1 ELSE 0 END) D201250, ";
                strQry += " SUM(CASE WHEN TC_CAPACITY>250 THEN 1 ELSE 0 END ) ABOVE250, ";
                strQry += " SUM(CASE WHEN (ROUND(SYSDATE -  RSM_ISSUE_DATE))  BETWEEN 0 AND 30 THEN  1 ELSE 0 END) WITHIN30,";
                strQry += " SUM(CASE WHEN (ROUND(SYSDATE -  RSM_ISSUE_DATE))  BETWEEN 31 AND 60 THEN  1 ELSE 0 END) WITHIN60,";
                strQry += " SUM(CASE WHEN (ROUND(SYSDATE -  RSM_ISSUE_DATE))  BETWEEN 61 AND 90 THEN  1 ELSE 0 END) WITHING90,";
                strQry += " SUM(CASE WHEN (ROUND(SYSDATE -  RSM_ISSUE_DATE))  > 91 THEN  1 ELSE 0 END) ABOVE90,COUNT(*) TOTAL  ";
                strQry += " FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS, TBLTRANSREPAIRER,TBLTCMASTER WHERE RSM_ID=RSD_RSM_ID ";
                strQry += " AND TR_ID=RSM_SUPREP_ID AND TR_OFFICECODE like '" + objReport.sOfficeCode + "%' AND ";
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " and TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " AND RSD_TC_CODE=TC_CODE GROUP BY TR_OFFICECODE,TR_NAME ORDER BY TR_OFFICECODE";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtRDetailedReport.Load(dr);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtRDetailedReport;

            }
        }
        /// <summary>
        /// RepairerWIse details
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintRePairerwise(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "  SELECT TR_NAME ,SUM(MORETHANONCE)MORETHANONCE,'" + objReport.sFromDate + "' AS FROMDATE, ";
                strQry += " '" + objReport.sTodate + "' AS TODATE, TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate ";
                strQry += " FROM (SELECT TR_ID,TR_NAME,RSD_TC_CODE,COUNT(*), CASE WHEN COUNT(*) > 1 THEN COUNT(*) ELSE 0 END MORETHANONCE, ";
                strQry += " CASE WHEN COUNT(*) = 1 THEN COUNT(*) ELSE 0 END ONLYONCE, ";
                strQry += " (SELECT substr(off_name,instr(off_name,':')+1) FROM VIEW_ALL_OFFICES WHERE ";
                strQry += " OFF_CODE=substr(TR_OFFICECODE,0,1))CIRCLE, (SELECT substr(off_name,instr(off_name,':')+1) FROM VIEW_ALL_OFFICES ";
                strQry += " WHERE OFF_CODE=substr(TR_OFFICECODE,0,2))DIVISION FROM TBLTRANSREPAIRER,TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS  ";
                strQry += " WHERE RSM_ID=RSD_RSM_ID  AND TR_ID=RSM_SUPREP_ID AND TR_OFFICECODE like '" + objReport.sOfficeCode + "%' AND ";
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " and TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " GROUP BY TR_NAME,RSD_TC_CODE,TR_ID,TR_OFFICECODE ";
                strQry += " ORDER BY TR_NAME,TR_ID) A WHERE MORETHANONCE!=0 GROUP BY TR_NAME";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtRDetailedReport.Load(dr);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtRDetailedReport;
            }
        }
        /// <summary>
        /// Print RepairerWise Others Details
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintRePairerOtherswise(clsReports objReport)
        {
            DataTable dtRDetailedReport = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT '" + objReport.sFromDate + "' AS FROMDATE,'" + objReport.sTodate + "' AS TODATE, ";
                strQry += " TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate,SUM(SUM(ONLYONCE))FAILURE FROM  (SELECT TR_NAME, ";
                strQry += " RSD_TC_CODE,COUNT(*), CASE WHEN COUNT(*) > 1 THEN COUNT(*) ELSE 0 END MORETHANONCE, ";
                strQry += " CASE WHEN COUNT(*) = 1 THEN COUNT(*) ELSE 0 END ONLYONCE, (SELECT substr(off_name,instr(off_name,':')+1) ";
                strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(TR_OFFICECODE,0,1)) as CIRCLE, (SELECT substr(off_name,instr(off_name,':')+1) ";
                strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(TR_OFFICECODE,0,2)) as DIVISION ";
                strQry += " FROM TBLTRANSREPAIRER,TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS  WHERE RSM_ID=RSD_RSM_ID  AND TR_ID=RSM_SUPREP_ID";
                strQry += " AND TR_OFFICECODE  like '" + objReport.sOfficeCode + "%' AND ";
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and ";
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND ";
                    strQry += " TO_CHAR(RSM_ISSUE_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                strQry += "  GROUP BY TR_NAME,RSD_TC_CODE,TR_OFFICECODE ORDER BY TR_NAME)A GROUP BY TR_NAME";
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dtRDetailedReport.Load(dr);
                return dtRDetailedReport;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                     ex.ToString(),
                     ex.Message,
                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name
                     );
                return dtRDetailedReport;
            }
        }
        /// <summary>
        /// Transformer Wise Details Completed
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable TransformerWiseDetailsCompleted(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable dtTransWisecompleted = new DataTable();
            try
            {
                strQry = " SELECT (SELECT substr(OFF_NAME,instr(OFF_NAME,':')+1) FROM ";
                strQry += " VIEW_ALL_OFFICES  WHERE off_code=substr(RSM_DIV_CODE,0,1)) CIRCLE, ";
                strQry += " substr(OFF_NAME,instr(OFF_NAME,':')+1) as Division,TO_CHAR(RSM_ISSUE_DATE,'DD-Mon-YY') AS Issued_On, ";
                strQry += " '" + objReport.sFromDate + "' AS FROMDATE, '" + objReport.sTodate + "' AS TODATE , ";
                strQry += " TO_CHAR(SYSDATE,'DD-MON-YYYY') as currentdate, TO_CHAR(RSD_DELIVARY_DATE,'DD-Mon-YY') AS Delivered_on, ";
                strQry += " to_char(ROUND(RSD_DELIVARY_DATE - RSM_ISSUE_DATE)) as TotalDays, (SELECT TR_NAME FROM TBLTRANSREPAIRER WHERE ";
                strQry += " TR_ID=rsm_suprep_id)REPAIRER_NAME, (SELECT TR_ADDRESS FROM TBLTRANSREPAIRER ";
                strQry += " WHERE TR_ID=rsm_suprep_id)REPAIRER_ADDRESS,to_char(TC_CODE) TC_CODE, ";
                strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TM_NAME, to_char(TC_CAPACITY)TC_CAPACITY, ";
                strQry += " RSM_GUARANTY_TYPE,RSM_PO_NO,TO_CHAR(RSM_PO_DATE,'DD-MON-YY')RSM_PO_DATE ";
                strQry += " FROM TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER,TBLTCMASTER, ";
                strQry += " VIEW_ALL_OFFICES WHERE RSM_ID = RSD_RSM_ID AND TC_CODE = ";
                strQry += " RSD_TC_CODE AND RSM_DIV_CODE = off_code and RSD_DELIVARY_DATE IS NOT NULL ";
                strQry += " AND LENGTH(off_code)=2 AND  off_code like '" + objReport.sOfficeCode + "%' ";
                if (objReport.sRepriername != null)
                {
                    strQry += "  AND RSM_SUPREP_ID='" + objReport.sRepriername + "'  ";
                }
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += "AND  TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += "AND  TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += "AND TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_DELIVARY_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += "  ORDER BY TC_CAPACITY,TM_NAME,(ROUND(RSM_ISSUE_DATE -  RSD_DELIVARY_DATE)) DESC";
                dtTransWisecompleted = ObjCon.getDataTable(strQry);
                return dtTransWisecompleted;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dtTransWisecompleted;
            }
        }
        /// <summary>
        /// Wo Reg Details
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable WoRegDetails(clsReports objReport)
        {
            string strQry = string.Empty;
            DataTable DtWoRegDetail = new DataTable();
            try
            {
                strQry = " select  (SELECT off_Code ||'-'|| substr(off_name,instr(off_name,':')+1) FROM ";
                strQry += " VIEW_ALL_OFFICES WHERE  OFF_CODE=substr (df_loc_code,0,1))CIRCLE, ";
                strQry += " (SELECT off_Code ||'-'|| substr(off_name,instr(off_name,':')+1)FROM  VIEW_ALL_OFFICES ";
                strQry += " WHERE OFF_CODE=substr(df_loc_code,0,2))DIVISION, ";
                strQry += " (SELECT off_Code ||'-'|| substr(off_name,instr(off_name,':')+1) FROM VIEW_ALL_OFFICES ";
                strQry += " WHERE OFF_CODE=substr(df_loc_code,0,3))SUBDIVISION, ";
                strQry += " CASE DF_STATUS_FLAG WHEN 1 THEN 'FAILURE' WHEN 2 THEN 'GOOD ENHANCEMENT' ";
                strQry += " WHEN 4 THEN 'FAILURE ENHANCEMENT' END FAILURE_TYPE , ";
                strQry += " '' as FROMDATE,(SELECT off_Code ||'-'|| substr(off_name,instr(off_name,':')+1) ";
                strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=substr(df_loc_code,0,4)) ";
                strQry += " SECTION,DF_DTC_CODE,TO_CHAR(DF_EQUIPMENT_ID)DF_EQUIPMENT_ID,TO_CHAR(EST_NO)EST_NO,'' AS TODATE,";
                strQry += " TO_CHAR(SYSDATE,'DD-MON-YYYY') as today,TO_CHAR( EST_CRON,'DD-MON-YYYY') EST_CRON , ";
                strQry += " (CASE WHEN TC_WARANTY_PERIOD IS NOT NULL THEN(SELECT CASE WHEN SYSDATE < TC_WARANTY_PERIOD ";
                strQry += " THEN MAX(RSD_GUARRENTY_TYPE) ELSE 'AGP' END FROM TBLREPAIRSENTDETAILS ";
                strQry += " WHERE RSD_WARENTY_PERIOD IS NOT NULL AND RSD_WARENTY_PERIOD IS NOT NULL AND RSD_TC_CODE=TC_CODE ";
                strQry += " ) ELSE '' END) RSM_GUARANTY_TYPE,WO_NO,WO_NO_DECOM , TO_CHAR( WO_DATE,'DD-MON-YYYY')WO_DATE,WO_AMT, ";
                strQry += " WO_AMT_DECOM, WO_ACC_CODE, WO_ACCCODE_DECOM, TO_CHAR( WO_DATE_DECOM, 'DD-MON-YYYY') WO_DATE_DECOM, ";
                strQry += " CASE WHEN DF_ENHANCE_CAPACITY IS NULL THEN TC_CAPACITY WHEN DF_ENHANCE_CAPACITY IS NOT NULL ";
                strQry += " THEN  TO_NUMBER(DF_ENHANCE_CAPACITY) END AS REPLACE_CAPACITY ,TO_CHAR(TC_CAPACITY)TC_CAPACITY,DF_REASON ";
                strQry += " from TBLTCMASTER INNER JOIN TBLDTCFAILURE  ON DF_EQUIPMENT_ID=TC_CODE ";
                strQry += " INNER JOIN TBLWORKORDER on df_id=WO_DF_ID INNER JOIN TBLESTIMATION ON DF_ID=EST_DF_ID ";
                strQry += " LEFT JOIN   TBLINDENT on wo_slno=ti_wo_slno LEFT JOIN TBLDTCINVOICE on in_ti_no=ti_id ";
                strQry += " LEFT JOIN TBLTCREPLACE ON tr_in_no=in_no INNER JOIN TBLDTCMAST ON dt_code=df_dtc_Code where ";
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(WO_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and ";
                    strQry += " TO_CHAR(WO_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(WO_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(WO_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(WO_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' ";
                    strQry += " AND TO_CHAR(WO_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                strQry += " AND DF_LOC_CODE LIKE '" + sOfficeCode + "%' ";
                strQry += " AND DF_STATUS_FLAG <>2 ORDER BY WO_DATE ";
                DtWoRegDetail = ObjCon.getDataTable(strQry);
                return DtWoRegDetail;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return DtWoRegDetail;
            }
        }
        /// <summary>
        /// DTC Added Report
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable DTCAddedReport(clsReports objReport)
        {
            DataTable dtDtcAddedDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                #region query for get dtc Added list based on conditions
                // if DTC Added through internal, external and so on ... 
                if (objReport.sDTCAddedThrough != "1")
                {
                    strQry = "select DT_CODE,DT_NAME, (select TM_NAME  from TBLTRANSMAKES  where TC_MAKE_ID=TM_ID)TC_MAKE_ID,";
                    strQry += " to_char(TC_CODE)TC_CODE, To_char(TC_CAPACITY)TC_CAPACITY,";
                    strQry += " (select CM_CIRCLE_NAME from TBLCIRCLE WHERE CM_CIRCLE_CODE=SUBSTR(DT_OM_SLNO,1,1)) AS CIRCLE,";
                    strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(DT_OM_SLNO,1,2)) as DIVISION ,";
                    strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(DT_OM_SLNO,1,3)) as SUBDIVISION, ";
                    strQry += " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(DT_OM_SLNO,1,4)) as SECTION,FD_FEEDER_NAME, ";
                    strQry += " FD_FEEDER_CODE,TC_SLNO,to_char(to_date('" + objReport.sFromDate + "','yyyy/MM/dd'),'dd-MON-yyyy') AS FROMDATE, ";
                    strQry += " to_char(to_date('" + objReport.sTodate + "','yyyy/MM/dd'),'dd-MON-yyyy') AS TODATE,TO_CHAR(SYSDATE,'dd-MON-yyyy')TODAY ";
                    strQry += " FROM  TBLDTCMAST,TBLTCMASTER,TBLFEEDERMAST ,TBLFDRTYPE  WHERE TC_CODE=DT_TC_ID AND FD_FEEDER_CODE=DT_FDRSLNO ";
                    strQry += " and DT_OM_SLNO  like '" + objReport.sOfficeCode + "%' AND DT_TC_ID <>0  AND  FD_FEEDER_TYPE =  FT_ID ";
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " AND  TO_CHAR(DT_CRON,'YYYY/MM/DD')>=to_char(to_date('" + objReport.sFromDate + "','yyyy/MM/dd'),'yyyy/MM/dd') ";
                        strQry += " and TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += "  AND TO_CHAR(DT_CRON,'YYYY/MM/DD')<=to_char(to_date('" + objReport.sTodate + "','yyyy/MM/dd'),'yyyy/MM/dd') ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += "  AND TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " AND  TO_CHAR(DT_CRON,'YYYY/MM/DD')>=to_char(to_date('" + objReport.sFromDate + "','yyyy/MM/dd'),'yyyy/MM/dd') ";
                        strQry += " AND TO_CHAR(DT_CRON,'YYYY/MM/DD')<=to_char(to_date('" + objReport.sTodate + "','yyyy/MM/dd'),'yyyy/MM/dd')  ";
                    }
                    if (objReport.sFeeder != null)
                    {
                        strQry += "  and  DT_FDRSLNO LIKE '" + objReport.sFeeder + "%'  ";
                    }
                    if (objReport.sSchemeType != null)
                    {
                        strQry += " and  DT_PROJECTTYPE='" + objReport.sSchemeType + "' ";
                    }
                    if (objReport.sFeederType != null)
                    {
                        strQry += " and  FT_ID='" + objReport.sFeederType + "' ";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += " AND TC_CAPACITY='" + objReport.sCapacity + "'";
                    }
                    strQry += "ORDER BY DT_CODE ";
                }
                // sDTCAddedThrough = 1 ; new dtc through mobile app 
                else
                {
                    strQry = " select DTE_DTCCODE as dt_Code ,DTE_NAME as dt_name, (select TM_NAME from TBLTRANSMAKES where DTE_MAKE =TM_ID)TC_MAKE_ID, ";
                    strQry += " to_char(DTE_TC_CODE )TC_CODE, To_char(DTE_CAPACITY)TC_CAPACITY, " +
                        " (select CM_CIRCLE_NAME from TBLCIRCLE WHERE CM_CIRCLE_CODE=SUBSTR(ED_OFFICECODE,1,1)) AS CIRCLE , " +
                        "  (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE=SUBSTR(ED_OFFICECODE,1,2)) as DIVISION , " +
                        "  (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE=SUBSTR(ED_OFFICECODE,1,3)) as SUBDIVISION, " +
                        " (SELECT OM_NAME FROM TBLOMSECMAST where OM_CODE=SUBSTR(ED_OFFICECODE,1,4)) as SECTION " +
                        "  ,FD_FEEDER_NAME,FD_FEEDER_CODE, DTE_TC_SLNO as tc_slno ," +
                        " to_char(to_date('" + objReport.sFromDate + "','yyyy/MM/dd'),'dd-MON-yyyy') AS FROMDATE,to_char(to_date('" + objReport.sTodate + "','yyyy/MM/dd'),'dd-MON-yyyy') AS TODATE, TO_CHAR(SYSDATE,'dd-MON-yyyy')TODAY   " +
                        "  FROM  TBLDTCENUMERATION left join  TBLENUMERATIONDETAILS on  ED_ID=DTE_ED_ID left  join   TBLFEEDERMAST on  FD_FEEDER_CODE= ED_FEEDERCODE " +
                        " left join  TBLFDRTYPE on   FD_FEEDER_TYPE =  FT_ID  left join     TBLDTCMAST on  DTE_DTCCODE=DT_CODE left join  TBLTCMASTER on  TC_CODE=DT_TC_ID     WHERE  " +
                       "  ED_OFFICECODE  like '" + objReport.sOfficeCode + "%'  AND  ED_ISNEWDTC='1'  AND ED_THROUGH='5' AND ED_RECORD_BY ='MOBILE-EXTERNAL'    ";
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " AND  TO_CHAR(ED_CRON,'YYYY/MM/DD')>=to_char(to_date('" + objReport.sFromDate + "','yyyy/MM/dd'),'yyyy/MM/dd') and TO_CHAR(ED_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";

                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += "  AND TO_CHAR(ED_CRON,'YYYY/MM/DD')<=to_char(to_date('" + objReport.sTodate + "','yyyy/MM/dd'),'yyyy/MM/dd') ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += "  AND TO_CHAR(ED_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " AND  TO_CHAR(ED_CRON,'YYYY/MM/DD')>=to_char(to_date('" + objReport.sFromDate + "','yyyy/MM/dd'),'yyyy/MM/dd') AND TO_CHAR(ED_CRON,'YYYY/MM/DD')<=to_char(to_date('" + objReport.sTodate + "','yyyy/MM/dd'),'yyyy/MM/dd')  ";
                    }
                    if (objReport.sFeeder != null)
                    {
                        strQry += "  and  ED_FEEDERCODE LIKE '" + objReport.sFeeder + "%'  ";
                    }
                    if (objReport.sSchemeType != null)
                    {
                        strQry += " and  DTE_PROJECTTYPE='" + objReport.sSchemeType + "' ";
                    }
                    if (objReport.sFeederType != null)
                    {
                        strQry += " and  FT_ID='" + objReport.sFeederType + "' ";
                    }
                    if (objReport.sCapacity != null)
                    {
                        strQry += " AND DTE_CAPACITY='" + objReport.sCapacity + "'";
                    }
                    if (objReport.sQCApprovaltype != "2")
                    {
                        strQry += " AND ED_STATUS_FLAG='" + objReport.sQCApprovaltype + "'";
                    }
                    strQry += "ORDER BY DT_CODE ";
                }
                #endregion
                dtDtcAddedDetails = ObjCon.getDataTable(strQry);
                return dtDtcAddedDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtDtcAddedDetails;
            }
        }
        /// <summary>
        /// frequent DTr Fail
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable FrequentTcFail(clsReports objReport)
        {
            DataTable dtFrequentTcFailureDetails = new DataTable();
            string strQry = string.Empty;
            string formattedfromdate = null;
            string formattedtodate = null;
            try
            {
                if (objReport.sFromDate != null)
                {
                    DateTime fromdate = Convert.ToDateTime(objReport.sFromDate);
                    formattedfromdate = fromdate.ToString("dd-MMM-yyyy");
                }
                if (objReport.sTodate != null)
                {
                    DateTime todate = Convert.ToDateTime(objReport.sTodate);
                    formattedtodate = todate.ToString("dd-MMM-yyyy");
                }
                strQry = "SELECT distinct TO_CHAR(TC_CODE)TC_CODE,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,DF_ID,TC_SLNO,DF_GUARANTY_TYPE,DF_DTC_CODE DT_CODE,";
                strQry += " DF_LOC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,MD_NAME, ";
                strQry += "TO_CHAR(SYSDATE,'DD-MON-YYYY') as TODAY, ";
                strQry += " '" + formattedfromdate + "' AS FROMDATE, ";
                strQry += " '" + formattedtodate + "' AS TODATE, ";
                strQry += " substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE,  ";
                strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID, ";
                strQry += " (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) ";
                strQry += " from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) ";
                strQry += " from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) ";
                strQry += " from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION, (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) ";
                strQry += " from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,TBLMASTERDATA,";
                strQry += " TBLDTCMAST,TBLTRANSMAKES where  DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE ";
                strQry += " AND DF_LOC_CODE LIKE ";
                strQry += "'" + objReport.sOfficeCode + "%' AND DF_FAILURE_TYPE=MD_ID AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (1,4) ";
                if (objReport.sGuranteeType != null)
                {
                    strQry += " AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "'  ";
                }
                if (objReport.sFailureType != null)
                {
                    strQry += " AND DF_FAILURE_TYPE='" + objReport.sFailureType + "' ";
                }
                strQry += " AND DF_DTC_CODE IN (SELECT DF_DTC_CODE FROM(SELECT DF_DTC_CODE,count(DF_DTC_CODE) ";
                strQry += " FROM TBLDTCFAILURE WHERE DF_STATUS_FLAG IN (1,4) AND DF_LOC_CODE LIKE '" + objReport.sOfficeCode + "%' AND ";
                string tempstrQry = string.Empty;
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    tempstrQry = " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    tempstrQry = " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')  ";
                    tempstrQry = "TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    tempstrQry = "TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'";
                }
                if (objReport.sGuranteeType != null)
                {
                    strQry += " AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "'  ";
                }
                if (objReport.sFailureType != null)
                {
                    strQry += " AND DF_FAILURE_TYPE='" + objReport.sFailureType + "' ";
                }
                if (objReport.sDtcCode != null)
                {
                    strQry += "AND DF_DTC_CODE='" + objReport.sDtcCode + "'";
                }
                if (objReport.sDtrCode != null)
                {
                    strQry += "AND DF_EQUIPMENT_ID='" + objReport.sDtrCode + "'";
                }
                strQry += " HAVING count(DF_DTC_CODE)>=2 GROUP BY DF_DTC_CODE)) AND " + tempstrQry + " ORDER BY DF_DTC_CODE , DF_ID  ";
                dtFrequentTcFailureDetails = ObjCon.getDataTable(strQry);
                return dtFrequentTcFailureDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtFrequentTcFailureDetails;
            }
        }
        /// <summary>
        ///  frequent DTR Fail
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable FrequentFailDTR(clsReports objReport)
        {
            DataTable dtFrequentTcFailureDetails = new DataTable();
            string strQry = string.Empty;
            string formattedfromdate = null;
            string formattedtodate = null;
            try
            {
                if (objReport.sFromDate != null)
                {
                    DateTime fromdate = Convert.ToDateTime(objReport.sFromDate);
                    formattedfromdate = fromdate.ToString("dd-MMM-yyyy");
                }
                if (objReport.sTodate != null)
                {
                    DateTime todate = Convert.ToDateTime(objReport.sTodate);
                    formattedtodate = todate.ToString("dd-MMM-yyyy");
                }
                strQry = " SELECT * FROM ( ";
                strQry += " SELECT DISTINCT TO_CHAR(TC_CODE)TC_CODE,  DF_ID , DF_DTC_CODE DT_CODE , TC_SLNO, ";
                strQry += " (SELECT TM_NAME FROM TBLTRANSMAKES WHERE TM_ID=TC_MAKE_ID)TC_MAKE_ID, ";
                strQry += " MD_NAME, DF_GUARANTY_TYPE, TO_CHAR(TC_CAPACITY)TC_CAPACITY,  TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,  DF_LOC_CODE, ";
                strQry += " TO_CHAR(SYSDATE,'DD-MON-YYYY') as TODAY,  '" + formattedfromdate + "' AS FROMDATE,  '" + formattedtodate + "' AS TODATE, ";
                strQry += " substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE, (SELECT DT_NAME FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE)DT_NAME, ";
                strQry += " (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  ";
                strQry += "from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,1)) CIRCLE,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  ";
                strQry += "from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,2)) DIVISION,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  ";
                strQry += "from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,3)) SUBDIVISION, (SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1)  ";
                strQry += "from VIEW_ALL_OFFICES where OFF_CODE=substr(DF_LOC_CODE,0,4)) SECTION from TBLTCMASTER,TBLDTCFAILURE,TBLMASTERDATA,";
                strQry += "TBLDTCMAST,TBLTRANSMAKES where  DF_EQUIPMENT_ID=TC_CODE and DF_DTC_CODE=DT_CODE ";
                strQry += " AND DF_LOC_CODE LIKE ";
                strQry += "'" + objReport.sOfficeCode + "%' AND DF_FAILURE_TYPE=MD_ID AND MD_TYPE='FT' AND DF_STATUS_FLAG IN (1,4) ";

                if (objReport.sGuranteeType != null)
                {
                    strQry += " AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "'  ";
                }
                if (objReport.sFailureType != null)
                {
                    strQry += " AND DF_FAILURE_TYPE='" + objReport.sFailureType + "' ";
                }
                strQry += " AND DF_EQUIPMENT_ID IN (SELECT DF_EQUIPMENT_ID FROM(SELECT DF_EQUIPMENT_ID,count(DF_EQUIPMENT_ID) ";
                strQry += " FROM TBLDTCFAILURE WHERE DF_STATUS_FLAG IN (1,4) AND DF_LOC_CODE LIKE '" + objReport.sOfficeCode + "%' AND ";
                string tempstrQry = string.Empty;
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    tempstrQry = " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    tempstrQry = " TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')  ";
                    tempstrQry = "TO_CHAR(DF_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                    tempstrQry = "TO_CHAR(DF_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DF_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'";
                }
                if (objReport.sGuranteeType != null)
                {
                    strQry += " AND DF_GUARANTY_TYPE='" + objReport.sGuranteeType + "'  ";
                }
                if (objReport.sFailureType != null)
                {
                    strQry += " AND DF_FAILURE_TYPE='" + objReport.sFailureType + "' ";
                }
                if (objReport.sDtcCode != null)
                {
                    strQry += "AND DF_DTC_CODE='" + objReport.sDtcCode + "'";
                }
                if (objReport.sDtrCode != null)
                {
                    strQry += "AND DF_EQUIPMENT_ID='" + objReport.sDtrCode + "'";
                }
                strQry += " HAVING count(DF_EQUIPMENT_ID)>=2 GROUP BY DF_EQUIPMENT_ID)) AND " + tempstrQry + " ORDER BY TC_CODE, DF_ID  ";
                strQry += ")A ORDER BY TO_DATE(DF_DATE,'dd-MON-yy')";
                dtFrequentTcFailureDetails = ObjCon.getDataTable(strQry);
                return dtFrequentTcFailureDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtFrequentTcFailureDetails;
            }
        }
        /// <summary>
        ///  Invoiced and RV dtr details along with work order details 
        /// </summary>
        /// <param name="objReport"></param>
        /// <author>Padma  </author>
        /// <returns></returns>
        public DataTable InvoicedandRVDetails(clsReports objReport)
        {
            DataTable dtFailureDetails = new DataTable();
            string strQry = string.Empty;
            string sReportType = string.Empty;
            try
            {
                if (objReport.sReportType == "1")
                {
                    sReportType = "Invoiced";
                }
                else
                {
                    sReportType = "RV";
                }

                strQry = " SELECT REPORT_TYPE, TR_CR_DATE, TC_CODE, INVOICED_DTR, TC_CAPACITY, INVOICED_DTR, TC_CAPACITY, ";
                strQry += " DF_GUARANTY_TYPE, DTC_CODE, DF_LOC_CODE, DF_REPLACE_FLAG, DF_DATE, COMMISSION, DECOMMISSION, ";
                strQry += " INV_NO, RV_NO, FROMDATE, TODATE, TODAY, FD_FEEDER_CODE, CIRCLE, DIVISION, SUBDIVISION, SECTION from( ";
                strQry += " SELECT '" + sReportType + "' as REPORT_TYPE ,   to_char(TR_CR_DATE,'dd-MON-yyyy')TR_CR_DATE,TO_CHAR(TC_CODE)TC_CODE,TO_CHAR(TD_TC_NO)INVOICED_DTR,TO_CHAR(TC_CAPACITY)TC_CAPACITY,DF_GUARANTY_TYPE , DF_DTC_CODE AS DTC_CODE,DF_LOC_CODE ,DF_REPLACE_FLAG ,TO_CHAR(DF_DATE,'dd-MON-yy')DF_DATE,";
                strQry += " WO_NO  ||' '|| TO_CHAR(WO_DATE,'dd-MON-yy') AS COMMISSION ,WO_NO_DECOM || ' ' ||TO_CHAR(WO_DATE_DECOM,'dd-MON-yy') AS DECOMMISSION,";
                strQry += " IN_MANUAL_INVNO || ' ' ||  TO_CHAR(IN_DATE,'dd-MON-yy') AS INV_NO  ,TR_MANUAL_ACKRV_NO  || ' ' || TO_CHAR(TR_RV_DATE,'dd-MON-yy') AS RV_NO,";
                strQry += " to_char(to_date('" + objReport.sFromDate + "','yyyy/MM/dd'),'dd-MON-yyyy') AS FROMDATE,to_char(to_date('" + objReport.sTodate + "','yyyy/MM/dd'),'dd-MON-yyyy') AS TODATE,TO_CHAR(SYSDATE,'dd-MON-yyyy')TODAY , substr(DF_DTC_CODE,0,4)FD_FEEDER_CODE ,";
                strQry += " (SELECT DISTINCT FD_FEEDER_NAME FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE=substr(DF_DTC_CODE,0,4))FD_FEEDER_NAME,";
                strQry += " (SELECT CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE WHERE CM_CIRCLE_CODE = substr(DF_LOC_CODE,0,1)) CIRCLE,";
                strQry += " (SELECT DIV_CODE || '-'|| DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = substr(DF_LOC_CODE,0,2)) DIVISION, (SELECT SD_SUBDIV_CODE ||'-'|| SD_SUBDIV_NAME FROM TBLSUBDIVMAST";
                strQry += " WHERE SD_SUBDIV_CODE = substr(DF_LOC_CODE,0,3)) SUBDIVISION,(SELECT OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_CODE = substr(DF_LOC_CODE,0,4)) SECTION FROM";
                strQry += " TBLDTCFAILURE LEFT JOIN   TBLWORKORDER on DF_ID = WO_DF_ID LEFT JOIN  TBLINDENT on WO_SLNO = TI_WO_SLNO LEFT JOIN TBLDTCINVOICE on TI_ID = IN_TI_NO";
                strQry += " LEFT JOIN TBLTCDRAWN ON  DF_ID =TD_DF_ID LEFT JOIN TBLTCMASTER ON DF_EQUIPMENT_ID = TC_CODE  LEFT JOIN TBLTCREPLACE ON IN_NO = TR_IN_NO  WHERE ";
                if (objReport.sReportType == "1")
                {
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " TO_CHAR(IN_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(IN_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += " TO_CHAR(IN_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += " TO_CHAR(IN_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " TO_CHAR(IN_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(IN_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'";
                    }
                }
                // Report Type  = RV 
                else if (objReport.sReportType == "2")
                {
                    if (objReport.sTodate == null && (objReport.sFromDate != null))
                    {
                        strQry += " TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                    }
                    if (objReport.sFromDate == null && (objReport.sTodate != null))
                    {
                        strQry += " TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                    }
                    if (objReport.sFromDate == null && objReport.sTodate == null)
                    {
                        strQry += " TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                    }
                    if (objReport.sFromDate != null && objReport.sTodate != null)
                    {
                        strQry += " TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(TR_RV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'";
                    }
                }
                strQry += " AND DF_LOC_CODE LIKE '" + sOfficeCode + "%' ";
                //ORDER BY CIRCLE,DIVISION,SUBDIVISION,SECTION";
                strQry += " )a ORDER BY a.CIRCLE,a.DIVISION,a.SUBDIVISION,a.SECTION ";
                dtFailureDetails = ObjCon.getDataTable(strQry);
                return dtFailureDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtFailureDetails;
            }
        }
        /// <summary>
        /// Mis Commission Pending
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable MisCommissionPending(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT * FROM VIEWMISCOMMISSIONPENDING ";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                    ex.ToString(),
                    ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name
                    );
                return dt;
            }
        }
        /// <summary>
        /// Mis Faulty Dtr
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable MisFaultyDtr(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "SELECT * FROM VIEWMISFAULTYDTR";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        //MOdified by Ramya.
        public DataTable GetReplacement(clsReports objReport)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable finaldt = new DataTable();


            try
            {
                //  SECTION_CODE,    SECTION_NAME,
                string strQry = "SELECT DIVISION_CODE , SUB_DIVISION_CODE ,SECTION_CODE, MD_NAME , DCR_RANGE ,STATUS , TC_CAPACITY , ";
                if (objReport.sReportType == "AGP")
                {
                    //strQry += " CASE GUARANTY_TYPE WHEN 'WGP' THEN 'WGP' WHEN 'WRGP' THEN 'WGP' WHEN 'AGP' THEN 'AGP' END GUARANTY_TYPE ";
                    strQry += " 'AGP' AS GUARANTY_TYPE ";
                }
                else if (objReport.sReportType == "WGP")
                {
                    strQry += " CASE GUARANTY_TYPE WHEN 'WGP' THEN 'WGP' WHEN 'WRGP' THEN 'WGP'  END GUARANTY_TYPE ";
                }
                else
                {
                    strQry += " 'TOTAL' AS GUARANTY_TYPE ";
                }
                // commented to check count miss match issue on 12-12-2022
                strQry += "  , DF_DATE , IN_DATE ,SECTION_NAME, SUB_DIVISION_NAME , DIVISION_NAME, CIRCLE_NAME FROM MV_VIEWMISFAILUREREPLACEMENT1 ";// MV_VIEWMISFAILUREREPLACEMENT";  VIEWMISFAILUREREPLACEMENT

                if (objReport.sReportType == "AGP")
                {
                    //strQry += " CASE GUARANTY_TYPE WHEN 'WGP' THEN 'WGP' WHEN 'WRGP' THEN 'WGP' WHEN 'AGP' THEN 'AGP' END GUARANTY_TYPE ";
                    strQry += " WHERE GUARANTY_TYPE = 'AGP'   ";
                }

                else if (objReport.sReportType == "WGP")
                {
                    //strQry += " CASE GUARANTY_TYPE WHEN 'WGP' THEN 'WGP' WHEN 'WRGP' THEN 'WGP' WHEN 'AGP' THEN 'AGP' END GUARANTY_TYPE ";
                    strQry += " WHERE GUARANTY_TYPE in ('WRGP','WGP')  ";
                }
                else
                {
                    strQry += " WHERE GUARANTY_TYPE in ('AGP','WRGP','WGP')  ";
                }
                dt = ObjCon.getDataTable(strQry);
                //  objReport.sGuranteeType = string.Join("/", objReport.sGuarantyTypes.ToArray());
                finaldt = dt.Copy();
                finaldt.Clear();


                DateTime StartDate = Convert.ToDateTime(objReport.sFromMonth);
                DateTime EndDate = Convert.ToDateTime(objReport.sToMonth);

                //OBCOUNT START.
                //while (StartDate <= EndDate)
                //{
                //string s = StartDate.ToString("MMM-yyyy");
                //var r=Convert.ToDateTime(StartDate.ToString("MMM-yyyy"));
                var obCountRows = (from failureReplacement in dt.AsEnumerable()
                                   where failureReplacement.Field<string>("STATUS").Equals("OBCOUNT")
                                   where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) < Convert.ToDateTime(StartDate.ToString("MMM-yyyy"))
                                   where (Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) >= Convert.ToDateTime(StartDate.ToString("MMM-yyyy")) || failureReplacement.Field<string>("IN_DATE") == null)
                                   orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                   select failureReplacement);

                if (obCountRows.Any())
                {
                    finaldt.Merge(obCountRows.CopyToDataTable<DataRow>());
                }
                StartDate = StartDate.AddMonths(1);
                // }
                //OBCOUNT END.


                StartDate = Convert.ToDateTime(objReport.sFromMonth);
                EndDate = Convert.ToDateTime(objReport.sToMonth);
                //TOBEREPLACED START
                // if the  selected month is same  
                //if (DateTime.ParseExact(objReport.sFromMonth, "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).Month == DateTime.Now.Month)
                //{
                //    var tobeReplacedRows = (from failureReplacement in dt.AsEnumerable()
                //                            where failureReplacement.Field<string>("STATUS").Equals("TOBEREPLACED")
                //                            where ((failureReplacement.Field<string>("IN_DATE") == null) || (Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) > Convert.ToDateTime(StartDate.ToString("MMM-yyyy"))))
                //                            //where failureReplacement.Field<Decimal>("DIVISION_CODE") == 22
                //                            orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                //                            select failureReplacement);
                //    if (tobeReplacedRows.Any())
                //    {
                //        if (finaldt.Rows.Count > 0)
                //        {
                //            finaldt.Merge(tobeReplacedRows.CopyToDataTable<DataRow>());
                //        }
                //        else
                //        {
                //            finaldt = tobeReplacedRows.CopyToDataTable<DataRow>();
                //        }
                //    }
                //}
                //else
                //{
                //while (StartDate <= EndDate)
                //{
                var tobeReplacedRows = (from failureReplacement in dt.AsEnumerable()
                                        where failureReplacement.Field<string>("STATUS").Equals("TOBEREPLACED")
                                        where (Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) > Convert.ToDateTime(EndDate.ToString("MMM-yyyy")) || failureReplacement.Field<string>("IN_DATE") == null)
                                        where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) <= Convert.ToDateTime(EndDate.ToString("MMM-yyyy"))
                                        orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                        select failureReplacement);

                if (tobeReplacedRows.Any())
                {
                    if (finaldt.Rows.Count > 0)
                    {
                        finaldt.Merge(tobeReplacedRows.CopyToDataTable<DataRow>());
                    }
                    else
                    {
                        finaldt = tobeReplacedRows.CopyToDataTable<DataRow>();
                    }
                }
                // StartDate = StartDate.AddMonths(1);
                //}
                // }
                //TOBEREPLACED END



                StartDate = Convert.ToDateTime(objReport.sFromMonth);
                EndDate = Convert.ToDateTime(objReport.sToMonth);
                //FAILED START
                while (StartDate <= EndDate)
                {
                    var failedDuringThisMonth = (from failureReplacement in dt.AsEnumerable()
                                                 where failureReplacement.Field<string>("STATUS").Equals("FAILED")
                                                 where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) == Convert.ToDateTime(StartDate.ToString("MMM-yyyy"))

                                                 orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                                 select failureReplacement);

                    if (failedDuringThisMonth.Any())
                    {
                        if (finaldt.Rows.Count > 0)
                        {
                            finaldt.Merge(failedDuringThisMonth.CopyToDataTable<DataRow>());
                        }
                        else
                        {
                            finaldt = failedDuringThisMonth.CopyToDataTable<DataRow>();
                        }
                    }
                    StartDate = StartDate.AddMonths(1);
                }
                //FAILED END


                StartDate = Convert.ToDateTime(objReport.sFromMonth);
                EndDate = Convert.ToDateTime(objReport.sToMonth);
                //REPLACED START
                DataTable emptydt = new DataTable();

                while (StartDate <= EndDate)
                {
                    var replacedDuringThisMonth = (from failureReplacement in dt.AsEnumerable()
                                                   where failureReplacement.Field<string>("STATUS").Equals("REPLACED")
                                                   where Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) == Convert.ToDateTime(StartDate.ToString("MMM-yyyy"))

                                                   orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                                   select failureReplacement);

                    //var replacedDuringThisMonth = (from failureReplacement in dt.AsEnumerable()
                    //                               where failureReplacement.Field<string>("STATUS").Equals("REPLACED")
                    //                               where ((Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) >=  Convert.ToDateTime(StartDate.ToString("MMM-yyyy"))) && (Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) <= Convert.ToDateTime(EndDate.ToString("MMM-yyyy"))) )
                    //                               where (Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) < Convert.ToDateTime(StartDate.ToString("MMM-yyyy")))
                    //                               orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                    //                               select failureReplacement);

                    if (replacedDuringThisMonth.Any())
                    {
                        if (finaldt.Rows.Count > 0)
                        {
                            finaldt.Merge(replacedDuringThisMonth.CopyToDataTable<DataRow>());
                        }
                        else
                        {
                            finaldt = replacedDuringThisMonth.CopyToDataTable<DataRow>();
                        }
                    }
                    StartDate = StartDate.AddMonths(1);

                    // cannot assign to null so assigned to some varible.
                    //emptydt.Rows.Add(replacedDuringThisMonth);
                    var finalDataRow = replacedDuringThisMonth;

                    if (finalDataRow.Any())
                    {
                        emptydt.Merge(finalDataRow.CopyToDataTable<DataRow>());
                    }

                }
                //REPLACED END


                DataTable dt3 = new DataTable();

                // division wise 
                if (objReport.sType == "2")
                {
                    //    if (objReport.sReportType == "AGP" || objReport.sReportType == "WGP")
                    //    {
                    //        finalDataRow = (from failureReplacement in finaldt.AsEnumerable()
                    //                           where objReport.sGuarantyTypes.Contains(failureReplacement.Field<string>("GUARANTY_TYPE"))
                    //                       // where failureReplacement.Field<string>("GUARANTY_TYPE").ToString().Contains(objReport.sReportType)
                    //                        where failureReplacement.Field<Decimal>("DIVISION_CODE").ToString().Contains(objReport.sOfficeCode)
                    //                        orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                    //                        select failureReplacement);
                    //    }

                    //    else

                    var finalDataRow2 = (from failureReplacement in finaldt.AsEnumerable()

                                         where failureReplacement.Field<Decimal>("DIVISION_CODE").ToString().Substring(0, 2).Contains(objReport.sOfficeCode)
                                         orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                         select failureReplacement);
                    if (finalDataRow2.Any())
                    {
                        emptydt = finalDataRow2.CopyToDataTable<DataRow>();
                    }

                    var groupbyDCR = from failureReplacement in emptydt.AsEnumerable()
                                     group failureReplacement by new
                                     {
                                         DCRRange = failureReplacement.Field<string>("DCR_RANGE"),
                                         Status = failureReplacement.Field<string>("STATUS"),
                                         circle = failureReplacement.Field<string>("CIRCLE_NAME"),
                                         subdivision = failureReplacement.Field<string>("SUB_DIVISION_NAME"),
                                         division = failureReplacement.Field<string>("DIVISION_NAME"),

                                         guarantyType = failureReplacement.Field<string>("GUARANTY_TYPE")
                                     } into g
                                     orderby g.Key.division

                                     select new
                                     {
                                         DCRRange = g.Key.DCRRange,
                                         Status = g.Key.Status,
                                         circle = g.Key.circle,
                                         subdivision = g.Key.subdivision,
                                         division = g.Key.division,

                                         guarantyType = g.Key.guarantyType,
                                         TotalMark = g.Count()
                                     };
                    dt3.Columns.Add("DCR_RANGE");
                    dt3.Columns.Add("STATUS");
                    dt3.Columns.Add("CIRCLE_NAME");
                    dt3.Columns.Add("DIVISION_NAME");
                    dt3.Columns.Add("GUARANTY_TYPE");
                    dt3.Columns.Add("TOTAL");
                    dt3.Columns.Add("SELECTED_GUARANTY_TYPE");
                    dt3.Columns.Add("FROM_DATE");
                    dt3.Columns.Add("TO_DATE");
                    foreach (var items in groupbyDCR)
                    {
                        dt3.Rows.Add(items.DCRRange, items.Status, items.circle, items.division, items.guarantyType, items.TotalMark, objReport.sGuranteeType, objReport.sFromMonth, objReport.sToMonth);
                    }

                    if (objReport.sReportType == "AGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS ,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME , DIV_CODE || '-' || DIV_NAME  AS DIVISION_NAME,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sFromMonth + "' FROM_DATE,'" + objReport.sToMonth + "'TO_DATE    FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += "  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) ";

                        strQry += "  C  cross join (SELECT  *  FROM tbldivision , TBLCIRCLE WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE ) D ";
                    }
                    else if (objReport.sReportType == "WGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME , DIV_CODE || '-' || DIV_NAME  AS DIVISION_NAME ,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sFromMonth + "' FROM_DATE,'" + objReport.sToMonth + "'TO_DATE    FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += " (SELECT 'WGP' GUARANTY_TYPE FROM DUAL)    ";

                        strQry += " C  cross join (SELECT  *  FROM tbldivision , TBLCIRCLE WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE ) D";
                    }
                    else
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME , DIV_CODE || '-' || DIV_NAME  AS DIVISION_NAME ,  'TOTAL' AS GUARANTY_TYPE ";
                        strQry += " ,'0' AS TOTAL ,'AGP/WGP/WRGP' AS SELECTED_GUARANTY_TYPE,'Jan-2019' ";
                        strQry += "SELECTED_MONTH    FROM  (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE ";
                        strQry += "WHERE DCR_RANGE < 500) A  CROSS JOIN    (SELECT * FROM  (SELECT 'OBCOUNT' ";
                        strQry += "STATUS  FROM DUAL)   union all  (SELECT 'REPLACED' STATUS FROM DUAL)  union ";
                        strQry += "all   (SELECT 'FAILED' STATUS FROM DUAL)    union all   (SELECT 'TOBEREPLACED' ";
                        strQry += "STATUS FROM DUAL)) B cross join (SELECT  *  FROM tbldivision , TBLCIRCLE WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE ) D";
                    }

                    dt3.Merge(ObjCon.getDataTable(strQry));
                    return dt3;

                }
                //sub division
                else if (objReport.sType == "3")
                //  else 
                {
                    var finalDataRow3 = (from failureReplacement in finaldt.AsEnumerable()

                                         where failureReplacement.Field<Decimal>("SUB_DIVISION_CODE").ToString().Substring(0, 3).Contains(objReport.sOfficeCode)
                                         orderby failureReplacement.Field<Decimal>("SUB_DIVISION_CODE").ToString()
                                         select failureReplacement);
                    if (finalDataRow3.Any())
                    {
                        emptydt = finalDataRow3.CopyToDataTable<DataRow>();
                    }

                    var groupbyDCR = from failureReplacement in emptydt.AsEnumerable()
                                     group failureReplacement by new
                                     {
                                         DCRRange = failureReplacement.Field<string>("DCR_RANGE"),
                                         Status = failureReplacement.Field<string>("STATUS"),
                                         circle = failureReplacement.Field<string>("CIRCLE_NAME"),
                                         division = failureReplacement.Field<string>("DIVISION_NAME"),
                                         SubDivision = failureReplacement.Field<string>("SUB_DIVISION_NAME"),
                                         guarantyType = failureReplacement.Field<string>("GUARANTY_TYPE")
                                     } into g

                                     orderby g.Key.SubDivision
                                     select new
                                     {
                                         DCRRange = g.Key.DCRRange,
                                         Status = g.Key.Status,
                                         circle = g.Key.circle,
                                         division = g.Key.division,
                                         SubDivision = g.Key.SubDivision,
                                         guarantyType = g.Key.guarantyType,
                                         TotalMark = g.Count()
                                     };
                    dt3.Columns.Add("DCR_RANGE");
                    dt3.Columns.Add("STATUS");
                    dt3.Columns.Add("CIRCLE_NAME");
                    dt3.Columns.Add("DIVISION_NAME");//sub division name has been changed to division name but the actual data is sub division name .
                    dt3.Columns.Add("GUARANTY_TYPE");
                    dt3.Columns.Add("TOTAL");
                    dt3.Columns.Add("SELECTED_GUARANTY_TYPE");
                    dt3.Columns.Add("FROM_DATE");
                    dt3.Columns.Add("TO_DATE");
                    foreach (var items in groupbyDCR)
                    {
                        dt3.Rows.Add(items.DCRRange, items.Status, items.circle, items.SubDivision, items.guarantyType, items.TotalMark, objReport.sGuranteeType, objReport.sMonth);
                    }

                    if (objReport.sReportType == "AGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS , CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME ,SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME  AS DIVISION_NAME ,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sFromMonth + "' FROM_DATE,'" + objReport.sToMonth + "'TO_DATE   FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += "  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) ";

                        strQry += "  C cross join (SELECT  * FROM tbldivision , TBLCIRCLE,TBLSUBDIVMAST,TBLOMSECMAST WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE and SD_DIV_CODE=DIV_CODE and SD_SUBDIV_CODE=OM_SUBDIV_CODE )D ";
                    }
                    else if (objReport.sReportType == "WGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS ,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME  ,SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME  AS DIVISION_NAME,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sFromMonth + "' FROM_DATE,'" + objReport.sToMonth + "'TO_DATE   FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += " (SELECT 'WGP' GUARANTY_TYPE FROM DUAL)    ";

                        strQry += " C cross join (SELECT  * FROM tbldivision , TBLCIRCLE,TBLSUBDIVMAST,TBLOMSECMAST WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE and SD_DIV_CODE=DIV_CODE and SD_SUBDIV_CODE=OM_SUBDIV_CODE )D ";
                    }
                    else
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS ,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME ,SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME  AS DIVISION_NAME,  'TOTAL' AS GUARANTY_TYPE ";
                        strQry += " ,'0' AS TOTAL ,'AGP/WGP/WRGP' AS SELECTED_GUARANTY_TYPE,'Jan-2019' ";
                        strQry += "SELECTED_MONTH    FROM  (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE ";
                        strQry += "WHERE DCR_RANGE < 500) A  CROSS JOIN    (SELECT * FROM  (SELECT 'OBCOUNT' ";
                        strQry += "STATUS  FROM DUAL)   union all  (SELECT 'REPLACED' STATUS FROM DUAL)  union ";
                        strQry += "all   (SELECT 'FAILED' STATUS FROM DUAL)    union all   (SELECT 'TOBEREPLACED' ";
                        strQry += "STATUS FROM DUAL)) B cross join (SELECT  * FROM tbldivision , TBLCIRCLE,TBLSUBDIVMAST,TBLOMSECMAST WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE and SD_DIV_CODE=DIV_CODE and SD_SUBDIV_CODE=OM_SUBDIV_CODE )D";
                    }

                    dt3.Merge(ObjCon.getDataTable(strQry));
                    return dt3;
                }
                // for section wise count
                else
                {
                    var finalDataRow2 = (from failureReplacement in finaldt.AsEnumerable()

                                         where failureReplacement.Field<Decimal>("SECTION_CODE").ToString().Substring(0, 4).Contains(objReport.sOfficeCode)
                                         orderby failureReplacement.Field<Decimal>("SECTION_CODE").ToString()
                                         select failureReplacement);
                    if (finalDataRow2.Any())
                    {
                        emptydt = finalDataRow2.CopyToDataTable<DataRow>();
                    }

                    var groupbyDCR = from failureReplacement in emptydt.AsEnumerable()
                                     group failureReplacement by new
                                     {
                                         DCRRange = failureReplacement.Field<string>("DCR_RANGE"),
                                         Status = failureReplacement.Field<string>("STATUS"),
                                         circle = failureReplacement.Field<string>("CIRCLE_NAME"),
                                         division = failureReplacement.Field<string>("DIVISION_NAME"),
                                         SubDivision = failureReplacement.Field<string>("SUB_DIVISION_NAME"),
                                         Section = failureReplacement.Field<string>("SECTION_NAME"),
                                         guarantyType = failureReplacement.Field<string>("GUARANTY_TYPE")
                                     } into g

                                     orderby g.Key.Section
                                     select new
                                     {
                                         DCRRange = g.Key.DCRRange,
                                         Status = g.Key.Status,
                                         circle = g.Key.circle,
                                         division = g.Key.division,
                                         SubDivision = g.Key.SubDivision,
                                         Section = g.Key.Section,
                                         guarantyType = g.Key.guarantyType,
                                         TotalMark = g.Count()
                                     };
                    dt3.Columns.Add("DCR_RANGE");
                    dt3.Columns.Add("STATUS");
                    dt3.Columns.Add("CIRCLE_NAME");
                    // dt3.Columns.Add("DIVISION_NAME");//sub division name has been changed to division name but the actual data is sub division name .
                    dt3.Columns.Add("DIVISION_NAME"); //section name has been changed to division name but the actual data is section name .
                    dt3.Columns.Add("GUARANTY_TYPE");
                    dt3.Columns.Add("TOTAL");
                    dt3.Columns.Add("SELECTED_GUARANTY_TYPE");
                    dt3.Columns.Add("FROM_DATE");
                    dt3.Columns.Add("TO_DATE");
                    foreach (var items in groupbyDCR)
                    {
                        dt3.Rows.Add(items.DCRRange, items.Status, items.circle, items.Section, items.guarantyType, items.TotalMark, objReport.sGuranteeType, objReport.sMonth);
                    }

                    if (objReport.sReportType == "AGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS ,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME ,OM_CODE || '-' || OM_NAME  AS DIVISION_NAME ,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sFromMonth + "' FROM_DATE,'" + objReport.sToMonth + "'TO_DATE   FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += "  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) ";

                        strQry += "  C cross join (SELECT  * FROM tbldivision , TBLCIRCLE,TBLSUBDIVMAST,TBLOMSECMAST WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE and SD_DIV_CODE=DIV_CODE and SD_SUBDIV_CODE=OM_SUBDIV_CODE )D ";
                    }
                    else if (objReport.sReportType == "WGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS ,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME  ,OM_CODE || '-' || OM_NAME  AS DIVISION_NAME,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sFromMonth + "' FROM_DATE,'" + objReport.sToMonth + "'TO_DATE   FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += " (SELECT 'WGP' GUARANTY_TYPE FROM DUAL)    ";

                        strQry += " C cross join (SELECT  * FROM tbldivision , TBLCIRCLE,TBLSUBDIVMAST,TBLOMSECMAST WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE and SD_DIV_CODE=DIV_CODE and SD_SUBDIV_CODE=OM_SUBDIV_CODE )D ";
                    }
                    else
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS ,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME  ,OM_CODE || '-' || OM_NAME  AS DIVISION_NAME,  'TOTAL' AS GUARANTY_TYPE ";
                        strQry += " ,'0' AS TOTAL ,'AGP/WGP/WRGP' AS SELECTED_GUARANTY_TYPE,'Jan-2019' ";
                        strQry += "SELECTED_MONTH    FROM  (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE ";
                        strQry += "WHERE DCR_RANGE < 500) A  CROSS JOIN    (SELECT * FROM  (SELECT 'OBCOUNT' ";
                        strQry += "STATUS  FROM DUAL)   union all  (SELECT 'REPLACED' STATUS FROM DUAL)  union ";
                        strQry += "all   (SELECT 'FAILED' STATUS FROM DUAL)    union all   (SELECT 'TOBEREPLACED' ";
                        strQry += "STATUS FROM DUAL)) B cross join (SELECT  * FROM tbldivision , TBLCIRCLE,TBLSUBDIVMAST,TBLOMSECMAST WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE and SD_DIV_CODE=DIV_CODE and SD_SUBDIV_CODE=OM_SUBDIV_CODE )D ";
                    }

                    dt3.Merge(ObjCon.getDataTable(strQry));
                    return dt3;
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
                throw ex;
            }


            return dt;
        }
        //MOdified by Ramya END.

        public DataTable GetMisFailureReplacementTable(clsReports objReport)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable finaldt = new DataTable();
            try
            {
                //string strQry = "SELECT * FROM VIEWMISFAILUREREPLACEMENT";
                string strQry = "SELECT DIVISION_CODE , SUB_DIVISION_CODE , MD_NAME , DCR_RANGE ,STATUS , TC_CAPACITY , ";

                if (objReport.sReportType == "AGP")
                {
                    //strQry += " CASE GUARANTY_TYPE WHEN 'WGP' THEN 'WGP' WHEN 'WRGP' THEN 'WGP' WHEN 'AGP' THEN 'AGP' END GUARANTY_TYPE ";
                    strQry += " 'AGP' AS GUARANTY_TYPE ";
                }
                else if (objReport.sReportType == "WGP")
                {
                    strQry += " CASE GUARANTY_TYPE WHEN 'WGP' THEN 'WGP' WHEN 'WRGP' THEN 'WGP'  END GUARANTY_TYPE ";
                }
                else
                {
                    strQry += " 'TOTAL' AS GUARANTY_TYPE ";
                }
                strQry += "  , DF_DATE , IN_DATE , SUB_DIVISION_NAME , DIVISION_NAME, CIRCLE_NAME FROM MV_VIEWMISFAILUREREPLACEMENT";
                if (objReport.sReportType == "AGP")
                {
                    //strQry += " CASE GUARANTY_TYPE WHEN 'WGP' THEN 'WGP' WHEN 'WRGP' THEN 'WGP' WHEN 'AGP' THEN 'AGP' END GUARANTY_TYPE ";
                    strQry += " WHERE GUARANTY_TYPE = 'AGP' ";
                }
                else if (objReport.sReportType == "WGP")
                {
                    //strQry += " CASE GUARANTY_TYPE WHEN 'WGP' THEN 'WGP' WHEN 'WRGP' THEN 'WGP' WHEN 'AGP' THEN 'AGP' END GUARANTY_TYPE ";
                    strQry += " WHERE GUARANTY_TYPE in ('WRGP','WGP')  ";
                }
                else
                {
                    strQry += " WHERE GUARANTY_TYPE in ('AGP','WRGP','WGP')  ";
                }


                dt = ObjCon.getDataTable(strQry);
                //  objReport.sGuranteeType = string.Join("/", objReport.sGuarantyTypes.ToArray());


                finaldt = dt.Copy();
                finaldt.Clear();

                //var dataRow01 = dt.AsEnumerable().Where(x => x.Field<decimal>("DIVISION_CODE")
                //                            .Where(x => x.Field<string>("STATUS") == "REPLACEDDURINGTHISMONTH");

                //var dataRow = (from failureReplacement in dt.AsEnumerable()
                //               where failureReplacement.Field<Decimal>("DIVISION_CODE").ToString().Contains("11")
                //              && failureReplacement.Field<string>("STATUS") == "REPLACEDDURINGTHISMONTH"
                //               select failureReplacement);


                //var dataRow = (from failureReplacement in dt.AsEnumerable()
                //               where failureReplacement.Field<Decimal>("DIVISION_CODE").ToString().Contains("")
                //              where failureReplacement.Field<string>("STATUS").Equals("FAILEDDURINGTHISMONTH")
                //              orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                //               select failureReplacement);

                //var  dataRow1 = (from failureReplacement in dt.AsEnumerable()
                //            where failureReplacement.Field<string>("STATUS").Equals("REPLACEDDURINGTHISMONTH")
                //            where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) <= Convert.ToDateTime(objReport.sMonth)
                //            orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                //            select failureReplacement);
                //dt1 = dataRow1.CopyToDataTable<DataRow>();



                //var dataRow1 = (from failureReplacement in dt.AsEnumerable()
                //                where failureReplacement.Field<Decimal>("DIVISION_CODE") == 11
                //                where failureReplacement.Field<string>("STATUS") == "TOBEREPLACED"
                //               select failureReplacement);

                //finaldt.Columns.Add("DIVISION_CODE", typeof(string));
                //finaldt.Columns.Add("SUB_DIVISION_CODE", typeof(string));
                //finaldt.Columns.Add("MD_NAME", typeof(Object));
                //finaldt.Columns.Add("DCR_RANGE", typeof(decimal));
                //finaldt.Columns.Add("STATUS", typeof(string));
                //finaldt.Columns.Add("TC_CAPACITY", typeof(decimal));
                //finaldt.Columns.Add("GUARANTY_TYPE", typeof(string));
                //finaldt.Columns.Add("DF_DATE", typeof(DateTime));
                //finaldt.Columns.Add("IN_DATE", typeof(DateTime));

                //   finaldt.Rows.Add(dataRow);


                //finaldt = dataRow.CopyToDataTable<DataRow>();
                // finaldt.Merge(dataRow1.CopyToDataTable<DataRow>());

                var obCountRows = (from failureReplacement in dt.AsEnumerable()
                                   where failureReplacement.Field<string>("STATUS").Equals("OBCOUNT")
                                   where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) < Convert.ToDateTime(objReport.sMonth)
                                   where (Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) >= Convert.ToDateTime(objReport.sMonth) || failureReplacement.Field<string>("IN_DATE") == null)
                                   orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                   select failureReplacement);

                if (obCountRows.Any())
                {
                    finaldt = obCountRows.CopyToDataTable<DataRow>();
                }

                // if the  selected month is same  
                if (DateTime.ParseExact(objReport.sMonth, "MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).Month == DateTime.Now.Month)
                {
                    var tobeReplacedRows = (from failureReplacement in dt.AsEnumerable()
                                            where failureReplacement.Field<string>("STATUS").Equals("TOBEREPLACED")
                                            where ((failureReplacement.Field<string>("IN_DATE") == null) || (Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) > Convert.ToDateTime(objReport.sMonth)))
                                            //where failureReplacement.Field<Decimal>("DIVISION_CODE") == 22
                                            orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                            select failureReplacement);

                    if (tobeReplacedRows.Any())
                    {
                        if (finaldt.Rows.Count > 0)
                        {
                            finaldt.Merge(tobeReplacedRows.CopyToDataTable<DataRow>());
                        }
                        else
                        {
                            finaldt = tobeReplacedRows.CopyToDataTable<DataRow>();
                        }
                    }
                }
                else
                {
                    var tobeReplacedRows = (from failureReplacement in dt.AsEnumerable()
                                            where failureReplacement.Field<string>("STATUS").Equals("TOBEREPLACED")
                                            where (Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) > Convert.ToDateTime(objReport.sMonth) || failureReplacement.Field<string>("IN_DATE") == null)
                                            where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) <= Convert.ToDateTime(objReport.sMonth)
                                            orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                            select failureReplacement);

                    if (tobeReplacedRows.Any())
                    {
                        if (finaldt.Rows.Count > 0)
                        {
                            finaldt.Merge(tobeReplacedRows.CopyToDataTable<DataRow>());
                        }
                        else
                        {
                            finaldt = tobeReplacedRows.CopyToDataTable<DataRow>();
                        }
                    }
                }


                var failedDuringThisMonth = (from failureReplacement in dt.AsEnumerable()
                                             where failureReplacement.Field<string>("STATUS").Equals("FAILED")
                                             where Convert.ToDateTime(failureReplacement.Field<string>("DF_DATE")) == Convert.ToDateTime(objReport.sMonth)
                                             orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                             select failureReplacement);

                if (failedDuringThisMonth.Any())
                {
                    if (finaldt.Rows.Count > 0)
                    {
                        finaldt.Merge(failedDuringThisMonth.CopyToDataTable<DataRow>());
                    }
                    else
                    {
                        finaldt = failedDuringThisMonth.CopyToDataTable<DataRow>();
                    }
                }

                var replacedDuringThisMonth = (from failureReplacement in dt.AsEnumerable()
                                               where failureReplacement.Field<string>("STATUS").Equals("REPLACED")
                                               where Convert.ToDateTime(failureReplacement.Field<string>("IN_DATE")) == Convert.ToDateTime(objReport.sMonth)
                                               orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                               select failureReplacement);

                if (replacedDuringThisMonth.Any())
                {
                    if (finaldt.Rows.Count > 0)
                    {
                        finaldt.Merge(replacedDuringThisMonth.CopyToDataTable<DataRow>());
                    }
                    else
                    {
                        finaldt = replacedDuringThisMonth.CopyToDataTable<DataRow>();
                    }
                }

                // cannot assign to null so assigned to some varible.
                var finalDataRow = replacedDuringThisMonth;


                DataTable dt3 = new DataTable();

                // division wise 
                if (objReport.sType == "2")
                {
                    //    if (objReport.sReportType == "AGP" || objReport.sReportType == "WGP")
                    //    {
                    //        finalDataRow = (from failureReplacement in finaldt.AsEnumerable()
                    //                           where objReport.sGuarantyTypes.Contains(failureReplacement.Field<string>("GUARANTY_TYPE"))
                    //                       // where failureReplacement.Field<string>("GUARANTY_TYPE").ToString().Contains(objReport.sReportType)
                    //                        where failureReplacement.Field<Decimal>("DIVISION_CODE").ToString().Contains(objReport.sOfficeCode)
                    //                        orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                    //                        select failureReplacement);
                    //    }

                    //    else

                    finalDataRow = (from failureReplacement in finaldt.AsEnumerable()

                                    where failureReplacement.Field<Decimal>("DIVISION_CODE").ToString().Substring(0, 2).Contains(objReport.sOfficeCode)
                                    orderby failureReplacement.Field<Decimal>("DIVISION_CODE").ToString()
                                    select failureReplacement);

                    dt1 = finalDataRow.CopyToDataTable<DataRow>();

                    var groupbyDCR = from failureReplacement in dt1.AsEnumerable()
                                     group failureReplacement by new
                                     {
                                         DCRRange = failureReplacement.Field<string>("DCR_RANGE"),
                                         Status = failureReplacement.Field<string>("STATUS"),
                                         circle = failureReplacement.Field<string>("CIRCLE_NAME"),
                                         subdivision = failureReplacement.Field<string>("SUB_DIVISION_NAME"),
                                         division = failureReplacement.Field<string>("DIVISION_NAME"),

                                         guarantyType = failureReplacement.Field<string>("GUARANTY_TYPE")
                                     } into g
                                     orderby g.Key.division

                                     select new
                                     {
                                         DCRRange = g.Key.DCRRange,
                                         Status = g.Key.Status,
                                         circle = g.Key.circle,
                                         subdivision = g.Key.subdivision,
                                         division = g.Key.division,

                                         guarantyType = g.Key.guarantyType,
                                         TotalMark = g.Count()
                                     };
                    dt3.Columns.Add("DCR_RANGE");
                    dt3.Columns.Add("STATUS");
                    dt3.Columns.Add("CIRCLE_NAME");
                    dt3.Columns.Add("DIVISION_NAME");
                    dt3.Columns.Add("GUARANTY_TYPE");
                    dt3.Columns.Add("TOTAL");
                    dt3.Columns.Add("SELECTED_GUARANTY_TYPE");
                    dt3.Columns.Add("SELECTED_MONTH");
                    foreach (var items in groupbyDCR)
                    {
                        dt3.Rows.Add(items.DCRRange, items.Status, items.circle, items.division, items.guarantyType, items.TotalMark, objReport.sGuranteeType, objReport.sMonth);
                    }

                    if (objReport.sReportType == "AGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS ,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME , DIV_CODE || '-' || DIV_NAME  AS DIVISION_NAME,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sMonth + "' SELECTED_MONTH    FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += "  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) ";

                        strQry += "  C  cross join (SELECT  *  FROM tbldivision , TBLCIRCLE WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE ) D ";
                    }
                    else if (objReport.sReportType == "WGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME , DIV_CODE || '-' || DIV_NAME  AS DIVISION_NAME ,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sMonth + "' SELECTED_MONTH    FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += " (SELECT 'WGP' GUARANTY_TYPE FROM DUAL)    ";

                        strQry += " C  cross join (SELECT  *  FROM tbldivision , TBLCIRCLE WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE ) D";
                    }
                    else
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS,CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME  AS CIRCLE_NAME , DIV_CODE || '-' || DIV_NAME  AS DIVISION_NAME ,  'TOTAL' AS GUARANTY_TYPE ";
                        strQry += " ,'0' AS TOTAL ,'AGP/WGP/WRGP' AS SELECTED_GUARANTY_TYPE,'Jan-2019' ";
                        strQry += "SELECTED_MONTH    FROM  (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE ";
                        strQry += "WHERE DCR_RANGE < 500) A  CROSS JOIN    (SELECT * FROM  (SELECT 'OBCOUNT' ";
                        strQry += "STATUS  FROM DUAL)   union all  (SELECT 'REPLACED' STATUS FROM DUAL)  union ";
                        strQry += "all   (SELECT 'FAILED' STATUS FROM DUAL)    union all   (SELECT 'TOBEREPLACED' ";
                        strQry += "STATUS FROM DUAL)) B cross join (SELECT  *  FROM tbldivision , TBLCIRCLE WHERE CM_CIRCLE_CODE = DIV_CICLE_CODE ) D";
                    }

                    dt3.Merge(ObjCon.getDataTable(strQry));
                    return dt3;

                }
                //else
                //{
                //    finalDataRow = (from failureReplacement in finaldt.AsEnumerable()
                //                    where objReport.sGuarantyTypes.Contains(failureReplacement.Field<string>("GUARANTY_TYPE"))
                //                    where failureReplacement.Field<Decimal>("SUB_DIVISION_CODE").ToString().Contains(objReport.sOfficeCode)
                //                    orderby failureReplacement.Field<Decimal>("SUB_DIVISION_CODE").ToString()
                //                    select failureReplacement);
                //    dt1 = finalDataRow.CopyToDataTable<DataRow>();

                //    var groupbyDCR = from failureReplacement in dt1.AsEnumerable()
                //                     group failureReplacement by new
                //                     {
                //                         DCRRange = failureReplacement.Field<string>("DCR_RANGE"),
                //                         Status = failureReplacement.Field<string>("STATUS"),
                //                         SubDivision = failureReplacement.Field<string>("SUB_DIVISION_NAME"),
                //                         guarantyType = failureReplacement.Field<string>("GUARANTY_TYPE")
                //                     } into g

                //                     orderby g.Key.SubDivision
                //                     select new
                //                     {
                //                         DCRRange = g.Key.DCRRange,
                //                         Status = g.Key.Status,
                //                         SubDivision = g.Key.SubDivision,
                //                         guarantyType = g.Key.guarantyType,
                //                         TotalMark = g.Count()
                //                     };
                //    dt3.Columns.Add("DCR_RANGE");
                //    dt3.Columns.Add("STATUS");
                //    dt3.Columns.Add("DIVISION_NAME");//sub division name has been changed to division name but the actual data is sub division name .
                //    dt3.Columns.Add("GUARANTY_TYPE");
                //    dt3.Columns.Add("TOTAL");
                //    dt3.Columns.Add("SELECTED_GUARANTY_TYPE");
                //    dt3.Columns.Add("SELECTED_MONTH");
                //    foreach (var items in groupbyDCR)
                //    {
                //        dt3.Rows.Add(items.DCRRange, items.Status, items.SubDivision, items.guarantyType, items.TotalMark, objReport.sGuranteeType, objReport.sMonth);
                //    }

                //    strQry = "SELECT DCR_RANGE ,STATUS , '' AS DIVISION_NAME ,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sMonth + "' SELECTED_MONTH    FROM ";
                //    strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                //    strQry += " CROSS JOIN   ";
                //    strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                //    strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                //    strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                //    strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                //    strQry += " CROSS JOIN ";
                //    strQry += " (SELECT * FROM (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all  ";
                //    strQry += " (SELECT 'WGP' GUARANTY_TYPE FROM DUAL)   ";
                //    strQry += " ) C ";

                //    dt3.Merge(ObjCon.getDataTable(strQry));
                //    return dt3;
                //}

                // sub division Wise
                else
                {



                    finalDataRow = (from failureReplacement in finaldt.AsEnumerable()

                                    where failureReplacement.Field<Decimal>("SUB_DIVISION_CODE").ToString().Substring(0, 2).Contains(objReport.sOfficeCode)
                                    orderby failureReplacement.Field<Decimal>("SUB_DIVISION_CODE").ToString()
                                    select failureReplacement);

                    dt1 = finalDataRow.CopyToDataTable<DataRow>();

                    var groupbyDCR = from failureReplacement in dt1.AsEnumerable()
                                     group failureReplacement by new
                                     {
                                         DCRRange = failureReplacement.Field<string>("DCR_RANGE"),
                                         Status = failureReplacement.Field<string>("STATUS"),
                                         circle = failureReplacement.Field<string>("CIRCLE_NAME"),
                                         SubDivision = failureReplacement.Field<string>("SUB_DIVISION_NAME"),
                                         guarantyType = failureReplacement.Field<string>("GUARANTY_TYPE")
                                     } into g

                                     orderby g.Key.SubDivision
                                     select new
                                     {
                                         DCRRange = g.Key.DCRRange,
                                         Status = g.Key.Status,
                                         circle = g.Key.circle,
                                         SubDivision = g.Key.SubDivision,
                                         guarantyType = g.Key.guarantyType,
                                         TotalMark = g.Count()
                                     };
                    dt3.Columns.Add("DCR_RANGE");
                    dt3.Columns.Add("STATUS");
                    dt3.Columns.Add("CIRCLE_NAME");
                    dt3.Columns.Add("DIVISION_NAME");//sub division name has been changed to division name but the actual data is sub division name .
                    dt3.Columns.Add("GUARANTY_TYPE");
                    dt3.Columns.Add("TOTAL");
                    dt3.Columns.Add("SELECTED_GUARANTY_TYPE");
                    dt3.Columns.Add("SELECTED_MONTH");
                    foreach (var items in groupbyDCR)
                    {
                        dt3.Rows.Add(items.DCRRange, items.Status, items.circle, items.SubDivision, items.guarantyType, items.TotalMark, objReport.sGuranteeType, objReport.sMonth);
                    }

                    if (objReport.sReportType == "AGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS , '' AS DIVISION_NAME ,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sMonth + "' SELECTED_MONTH    FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += "  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) ";

                        strQry += "  C ";
                    }
                    else if (objReport.sReportType == "WGP")
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS , '' AS DIVISION_NAME ,  GUARANTY_TYPE ,'0' AS TOTAL ,'" + objReport.sGuranteeType + "' AS SELECTED_GUARANTY_TYPE,'" + objReport.sMonth + "' SELECTED_MONTH    FROM ";
                        strQry += " (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE WHERE DCR_RANGE < 500) A ";
                        strQry += " CROSS JOIN   ";
                        strQry += " (SELECT * FROM  (SELECT 'OBCOUNT' STATUS  FROM DUAL)   union all ";
                        strQry += " (SELECT 'REPLACED' STATUS FROM DUAL)  union all  ";
                        strQry += " (SELECT 'FAILED' STATUS FROM DUAL)    union all  ";
                        strQry += " (SELECT 'TOBEREPLACED' STATUS FROM DUAL)) B ";
                        strQry += " CROSS JOIN ";
                        strQry += " (SELECT 'WGP' GUARANTY_TYPE FROM DUAL)    ";

                        strQry += " C ";
                    }
                    else
                    {
                        strQry = "SELECT DCR_RANGE ,STATUS , '' AS DIVISION_NAME ,  'TOTAL' AS GUARANTY_TYPE ";
                        strQry += " ,'0' AS TOTAL ,'AGP/WGP/WRGP' AS SELECTED_GUARANTY_TYPE,'Jan-2019' ";
                        strQry += "SELECTED_MONTH    FROM  (SELECT DISTINCT  DCR_RANGE  FROM TBLDTRCAPACITYRANGE ";
                        strQry += "WHERE DCR_RANGE < 500) A  CROSS JOIN    (SELECT * FROM  (SELECT 'OBCOUNT' ";
                        strQry += "STATUS  FROM DUAL)   union all  (SELECT 'REPLACED' STATUS FROM DUAL)  union ";
                        strQry += "all   (SELECT 'FAILED' STATUS FROM DUAL)    union all   (SELECT 'TOBEREPLACED' ";
                        strQry += "STATUS FROM DUAL)) B";
                    }

                    dt3.Merge(ObjCon.getDataTable(strQry));
                    return dt3;
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
                return dt;
            }
        }

        public DataTable MisRepairerStatus(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            string offCode = string.Empty;
            offCode = objReport.sOfficeCode;

            try
            {
                if (objReport.sCurrentMonth != "")
                {
                    strQry = " SELECT * FROM (";
                    strQry += "SELECT to_char( (to_date('" + objReport.sCurrentMonth + "','MMyyyy')),'MON yyyy') AS SELECTEDMONTH , '' AS SELECTEDYEAR, TC_CAPACITY,(SELECT DIV_CODE ||'-'|| DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE )OFF_CODE, ";
                    strQry += " (SELECT CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE )CIRCLE ,";
                    strQry += " (SELECT CM_CIRCLE_CODE FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE ) CIRCLE_CODE , ";
                    strQry += " (SELECT DIV_CODE FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE) DIVISION_CODE ";
                    strQry += " , MD_NAME ,STATUS,DCR_RANGE, case GUARANTY_TYPE when 'AGP' THEN 'AGP' when 'WGP' then 'WGP' when 'WRGP' then 'WGP' END GUARANTY_TYPE , TR_NAME FROM ";
                    // obcount
                    strQry += " (SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME,";
                    strQry += " 'OBCOUNT' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM (SELECT * FROM (SELECT OFF_CODE , MD_NAME, 'OBCOUNT' AS STATUS , DCR_RANGE FROM ";
                    strQry += " TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = ";
                    strQry += " DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' GUARANTY_TYPE FROM DUAL";
                    strQry += " ) UNION ALL (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY , (SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ,";
                    strQry += " TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND RSD_INV_DATE IS ";
                    strQry += " NOT NULL AND ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') < TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy') and (RSD_DELIVARY_DATE is NULL or TO_DATE(TO_CHAR( ";
                    strQry += " RSD_DELIVARY_DATE,'MMyyyy'),'MMyyyy') >= TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy') ) )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE )A ";

                    strQry += " UNION ALL ";
                    // issued
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'ISSUED' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM (SELECT ";
                    strQry += " * FROM (SELECT OFF_CODE , MD_NAME, 'ISSUED' AS STATUS , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE MD_TYPE ";
                    strQry += " = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL ";
                    strQry += " (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY , (SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS , ";
                    strQry += " TBLTCMASTER ,TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy') )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )B ";

                    strQry += " UNION ALL ";
                    // received
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'RECEIVED' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT OFF_CODE , MD_NAME, 'RECEIVED' AS STATUS , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY ,(SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER ,TBLINSPECTIONDETAILS ,TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND ";
                    strQry += " TO_DATE(TO_CHAR(RSD_DELIVARY_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy') AND IND_INSP_RESULT not in (0,4) AND IND_RSD_ID=RSD_ID and RSM_ID=RSD_RSM_ID )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )C ";


                    strQry += " UNION ALL ";
                    // WITHOUT REPAIR RETURNED
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'RECEIVED WITHOUT REPAIR' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT OFF_CODE , MD_NAME, 'RECEIVED WITHOUT REPAIR' AS STATUS , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY ,(SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER ,TBLINSPECTIONDETAILS ,TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND ";
                    strQry += " TO_DATE(TO_CHAR(RSD_DELIVARY_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy') AND IND_INSP_RESULT=4 AND IND_RSD_ID=RSD_ID and RSM_ID=RSD_RSM_ID )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )D ";

                    strQry += " UNION ALL ";

                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING' AS STATUS , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT OFF_CODE , MD_NAME, 'ISSUED' AS STATUS , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY ,(SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                    strQry += " ,TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') <= TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy') and (RSD_DELIVARY_DATE is NULL or TO_DATE(TO_CHAR( ";
                    strQry += " RSD_DELIVARY_DATE,'MMyyyy'),'MMyyyy') > TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy') ) )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                    strQry += " ) E ";
                    strQry += " ) F ) WHERE GUARANTY_TYPE IN (" + objReport.sGuranteeType + ") ";

                    if (offCode.Length == 1)
                    {
                        strQry += " AND CIRCLE_CODE = '" + offCode + "' ";
                    }
                    if (offCode.Length == 2)
                    {
                        strQry += " AND DIVISION_CODE = '" + offCode + "' ";
                    }

                }
                else
                {

                    string aprilMonth = "04" + objReport.sFinancialYear;
                    string marchmonth = string.Empty;
                    string financialYear = string.Empty;


                    DateTime dFromDate = DateTime.ParseExact(objReport.sFinancialYear.Replace('-', '/'), "yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dFromDate = dFromDate.AddYears(1);
                    marchmonth = Convert.ToString(dFromDate.Year);

                    financialYear = objReport.sFinancialYear + '-' + Convert.ToString(dFromDate.Year);
                    marchmonth = "03" + marchmonth;

                    strQry = " SELECT SELECTEDYEAR , SELECTEDMONTH , TC_CAPACITY , OFF_CODE , CIRCLE , STATUS , case GUARANTY_TYPE when 'AGP' THEN 'AGP' when 'WGP' then 'WGP' when 'WRGP' then 'WGP' END GUARANTY_TYPE ";
                    strQry += " , TR_NAME , DCR_RANGE FROM (";
                    strQry += "SELECT '" + financialYear + "' AS SELECTEDYEAR , '' AS SELECTEDMONTH , TC_CAPACITY,(SELECT DIV_CODE ||'-'|| DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE )OFF_CODE, ";
                    strQry += " (SELECT CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE )CIRCLE ,";
                    strQry += " (SELECT CM_CIRCLE_CODE FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE ) CIRCLE_CODE , ";
                    strQry += " (SELECT DIV_CODE FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE) DIVISION_CODE ";
                    strQry += " , MD_NAME ,STATUS,DCR_RANGE,GUARANTY_TYPE , TR_NAME FROM (SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME,";
                    strQry += " 'OBCOUNT' AS STATUS , DCR_RANGE , CASE GUARANTY_TYPE when 'AGP' THEN 'AGP' when 'WGP' then 'WGP' when 'WRGP' then 'WGP' END GUARANTY_TYPE ";
                    strQry += " , TR_NAME FROM (SELECT * FROM (SELECT OFF_CODE , MD_NAME, 'OBCOUNT' AS STATUS , DCR_RANGE FROM ";
                    strQry += " TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = ";
                    strQry += " DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' GUARANTY_TYPE FROM DUAL";
                    strQry += " ) UNION ALL (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY , (SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ,";
                    strQry += " TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND RSD_INV_DATE IS ";
                    strQry += " NOT NULL AND ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') < TO_DATE('" + aprilMonth + "','MMyyyy') and (RSD_DELIVARY_DATE is NULL or TO_DATE(TO_CHAR( ";
                    strQry += " RSD_DELIVARY_DATE,'MMyyyy'),'MMyyyy') >= TO_DATE('" + aprilMonth + "','MMyyyy') ) )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE )A ";

                    strQry += " UNION ALL ";

                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'ISSUED' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM (SELECT ";
                    strQry += " * FROM (SELECT OFF_CODE , MD_NAME, 'ISSUED' AS STATUS , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE MD_TYPE ";
                    strQry += " = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL ";
                    strQry += " (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY , (SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS , ";
                    strQry += " TBLTCMASTER ,TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN TO_DATE('" + aprilMonth + "','MMyyyy') AND TO_DATE('" + marchmonth + "','MMyyyy') )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )B ";

                    strQry += " UNION ALL ";

                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'RECEIVED' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT OFF_CODE , MD_NAME, 'ISSUED' AS STATUS , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY ,(SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER ,TBLINSPECTIONDETAILS , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND ";
                    strQry += " TO_DATE(TO_CHAR(RSD_DELIVARY_DATE,'MMyyyy'),'MMyyyy') BETWEEN TO_DATE('" + aprilMonth + "','MMyyyy') AND TO_DATE('" + marchmonth + "','MMyyyy') AND IND_INSP_RESULT not in (0,4) AND IND_RSD_ID=RSD_ID and RSM_ID=RSD_RSM_ID )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )C ";

                    strQry += " UNION ALL ";
                    // WITHOUT REPAIR RETURNED
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'RECEIVED WITHOUT REPAIR' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT OFF_CODE , MD_NAME, 'RECEIVED WITHOUT REPAIR' AS STATUS , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY ,(SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER ,TBLINSPECTIONDETAILS ,TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND ";
                    strQry += " TO_DATE(TO_CHAR(RSD_DELIVARY_DATE,'MMyyyy'),'MMyyyy') BETWEEN TO_DATE('" + aprilMonth + "','MMyyyy') AND TO_DATE('" + marchmonth + "','MMyyyy') AND IND_INSP_RESULT=4 AND IND_RSD_ID=RSD_ID and RSM_ID=RSD_RSM_ID )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )D ";

                    strQry += " UNION ALL ";

                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING' AS STATUS , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT OFF_CODE , MD_NAME, 'ISSUED' AS STATUS , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM (SELECT 'WGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) union all (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE , TC_CAPACITY ,(SELECT TR_NAME FROM TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                    strQry += " ,TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND ";
                    strQry += " ";
                    strQry += " RSD_INV_DATE IS not null and (RSD_DELIVARY_DATE is NULL ) )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE GROUP BY MD_NAME,DCR_RANGE,STATUS, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                    strQry += " ) E ";
                    strQry += " ) F ) WHERE GUARANTY_TYPE IN (" + objReport.sGuranteeType + ") ";

                    if (offCode.Length == 1)
                    {
                        strQry += " AND CIRCLE_CODE = '" + offCode + "' ";
                    }
                    if (offCode.Length == 2)
                    {
                        strQry += " AND DIVISION_CODE = '" + offCode + "' ";
                    }
                }
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        public DataTable MisRepairerPerformance(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            string offCode = string.Empty;
            offCode = objReport.sOfficeCode;
            try
            {
                if (objReport.sCurrentMonth != "")
                {
                    //for month wise Report
                    strQry = " SELECT * FROM (";
                    strQry += "SELECT ''  AS SELECTEDYEAR , to_char( (to_date('" + objReport.sCurrentMonth + "','MMyyyy')),'MON yyyy') AS SELECTEDMONTH ,  TC_CAPACITY,(SELECT DIV_CODE ||'-'|| DIV_NAME  FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE )OFF_CODE, ";
                    strQry += " (SELECT CM_CIRCLE_CODE  || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE )CIRCLE ,";
                    strQry += " (SELECT CM_CIRCLE_CODE  FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE  ) CIRCLE_CODE , ";
                    strQry += " (SELECT DIV_CODE   FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE) DIVISION_CODE ";
                    strQry += " , MD_NAME ,STATUS,DCR_RANGE,case GUARANTY_TYPE when 'AGP' THEN 'AGP' when 'WGP' then 'WGP' when 'WRGP' then 'WGP' END GUARANTY_TYPE , TR_NAME FROM (SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME,";
                    strQry += " 'ISSUED' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM (SELECT * FROM (SELECT  OFF_CODE , MD_NAME, DCR_RANGE FROM ";
                    strQry += " TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE  MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  =  ";
                    strQry += " DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL";
                    strQry += " 	) UNION ALL (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " 	LEFT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY , (SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ,";
                    strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE AND ";
                    strQry += "  TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy')  ";
                    strQry += "  )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME  ORDER BY OFF_CODE, DCR_RANGE )A ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'DELIVERED WITHIN 30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM (SELECT ";
                    strQry += "  * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE  MD_TYPE ";
                    strQry += " = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' GUARANTY_TYPE  FROM DUAL ";
                    strQry += " ) UNION ALL (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  ";
                    strQry += " (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A LEFT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY  ,  (SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME   FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS , ";
                    strQry += " TBLTCMASTER ,TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy') AND  (round(RSD_DELIVARY_DATE- RSD_INV_DATE ) <= 30 )  )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )B ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'DELIVERED AFTER 30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += "  LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME  FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy') AND (round(RSD_DELIVARY_DATE- RSD_INV_DATE ) > 30 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )C  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING WITHIN 30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME, DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += "  LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME  FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE )<= 30 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )D  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING AFTER 30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " 	) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME    FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                    strQry += " ,TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID  AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE ) > 30 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                    strQry += " ) E  ";
                    strQry += " UNION ALL ";
                    strQry += " select* from (SELECT round(sum(days) / sum(tot), 0) as TC_CAPACITY, OFF_CODE, '' MD_NAME, 'AVERAGE No. OF DAYS TAKEN BY REPAIRER' AS STATUS, '' DCR_RANGE, GUARANTY_TYPE, TR_NAME FROM(SELECT * FROM(SELECT  OFF_CODE  FROM TBLMASTERDATA, VIEW_ALL_OFFICES, TBLDTRCAPACITYRANGE WHERE  MD_TYPE = 'C'  ";
                    strQry += " AND DCR_CAPACITY < 500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY) CROSS JOIN(SELECT * FROM(SELECT 'WRGP'  GUARANTY_TYPE FROM DUAL) UNION ALL(SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL(SELECT 'AGP' GUARANTY_TYPE FROM DUAL))) A   LEFT JOIN(select RSM_GUARANTY_TYPE, COALESCE(RSD_RV_DATE - RSD_INV_DATE, 0) as Days, 1 as tot,";
                    strQry += " RSM_DIV_CODE, TR_NAME  from(SELECT RSM_GUARANTY_TYPE, RSD_INV_DATE, RSD_RV_DATE, RSD_TC_CODE, RSM_DIV_CODE, (SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME  FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS, TBLTCMASTER, TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND ";
                    strQry += " RSD_TC_CODE = TC_CODE  AND   TO_DATE(TO_CHAR(RSD_INV_DATE, 'MMyyyy'), 'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "', 'MMyyyy')  AND RSD_DELIVARY_DATE IS not NULL )   )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY   OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE)X ";
                    strQry += " ) F  ) ";
                    if (offCode.Length == 1)
                    {
                        strQry += " WHERE CIRCLE_CODE = '" + offCode + "' ";
                    }
                    if (offCode.Length == 2)
                    {
                        strQry += " WHERE DIVISION_CODE = '" + offCode + "' ";
                    }
                }
                else
                {
                    string aprilMonth = "04" + objReport.sFinancialYear;
                    string marchmonth = string.Empty;
                    string financialYear = string.Empty;
                    DateTime dFromDate = DateTime.ParseExact(objReport.sFinancialYear.Replace('-', '/'), "yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dFromDate = dFromDate.AddYears(1);
                    marchmonth = Convert.ToString(dFromDate.Year);
                    financialYear = objReport.sFinancialYear + '-' + Convert.ToString(dFromDate.Year);
                    marchmonth = "03" + marchmonth;
                    strQry = " SELECT * FROM (";
                    strQry += "SELECT '" + financialYear + "'  AS SELECTEDYEAR , '' AS SELECTEDMONTH ,  TC_CAPACITY,(SELECT DIV_CODE ||'-'|| DIV_NAME  FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE )OFF_CODE, ";
                    strQry += " (SELECT CM_CIRCLE_CODE  || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE )CIRCLE ,";
                    strQry += " (SELECT CM_CIRCLE_CODE  FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE  ) CIRCLE_CODE , ";
                    strQry += " (SELECT DIV_CODE   FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE) DIVISION_CODE ";
                    strQry += " , MD_NAME ,STATUS,DCR_RANGE,case GUARANTY_TYPE when 'AGP' THEN 'AGP' when 'WGP' then 'WGP' when 'WRGP' then 'WGP' END GUARANTY_TYPE , TR_NAME FROM (SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME,";
                    strQry += " 'ISSUED' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM (SELECT * FROM (SELECT  OFF_CODE , MD_NAME, DCR_RANGE FROM ";
                    strQry += " TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE  MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  =  ";
                    strQry += " DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' GUARANTY_TYPE FROM DUAL";
                    strQry += " 	) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " 	LEFT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY , (SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ,";
                    strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE AND ";
                    strQry += "  TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy')  ";
                    strQry += "  )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME  ORDER BY OFF_CODE, DCR_RANGE )A ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'DELIVERED WITHIN 30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM (SELECT ";
                    strQry += "  * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE  MD_TYPE ";
                    strQry += " = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' GUARANTY_TYPE  FROM DUAL ";
                    strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  ";
                    strQry += " (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A LEFT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY  ,  (SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME   FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS , ";
                    strQry += " TBLTCMASTER ,TBLTRANSREPAIRER A WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy') AND  (round(RSD_DELIVARY_DATE- RSD_INV_DATE ) <= 30 )  )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )B ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'DELIVERED AFTER 30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME , DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += "  LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME  FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy') AND (round(RSD_DELIVARY_DATE- RSD_INV_DATE ) > 30 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )C  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING WITHIN 30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME, DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += "  LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME  FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE )<= 30 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )D  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING AFTER 30DAYS' , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " GUARANTY_TYPE FROM DUAL) ";
                    strQry += " 	 UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME    FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                    strQry += " ,TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID  AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE ) > 30 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                    strQry += " ) E  ";
                    strQry += " UNION ALL ";
                    strQry += " select* from (SELECT round(sum(days) / sum(tot), 0) as TC_CAPACITY, OFF_CODE, '' MD_NAME, 'AVERAGE No. OF TIME TAKEN BY REPAIRER' AS STATUS, '' DCR_RANGE, GUARANTY_TYPE, TR_NAME FROM(SELECT * FROM(SELECT  OFF_CODE  FROM TBLMASTERDATA, VIEW_ALL_OFFICES, TBLDTRCAPACITYRANGE WHERE  MD_TYPE = 'C'  ";
                    strQry += " AND DCR_CAPACITY < 500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME = DCR_CAPACITY) CROSS JOIN(SELECT * FROM(SELECT 'WRGP'  GUARANTY_TYPE FROM DUAL) UNION ALL(SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL(SELECT 'AGP' GUARANTY_TYPE FROM DUAL))) A   LEFT JOIN(select RSM_GUARANTY_TYPE, COALESCE(RSD_RV_DATE - RSD_INV_DATE, 0) as Days, 1 as tot,";
                    strQry += " RSM_DIV_CODE, TR_NAME  from(SELECT RSM_GUARANTY_TYPE, RSD_INV_DATE, RSD_RV_DATE, RSD_TC_CODE, RSM_DIV_CODE, (SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID) TR_NAME  FROM TBLREPAIRSENTMASTER, TBLREPAIRSENTDETAILS, TBLTCMASTER, TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND ";
                    strQry += " RSD_TC_CODE = TC_CODE  AND   TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy') AND RSD_DELIVARY_DATE IS not NULL )   )B ON B.RSM_DIV_CODE = A.OFF_CODE AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY   OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE)X ";
                    strQry += " ) F  ) ";
                    if (offCode.Length == 1)
                    {
                        strQry += " WHERE CIRCLE_CODE = '" + offCode + "' ";
                    }
                    if (offCode.Length == 2)
                    {
                        strQry += " WHERE DIVISION_CODE = '" + offCode + "' ";
                    }
                }
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }

        public DataTable MisRepairerPending(clsReports objReport)
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            string offCode = string.Empty;
            offCode = objReport.sOfficeCode;
            try
            {
                if (objReport.sCurrentMonth != "")
                {
                    //for month wise Report
                    strQry = " SELECT * FROM (";
                    strQry += "SELECT ''  AS SELECTEDYEAR , to_char( (to_date('" + objReport.sCurrentMonth + "','MMyyyy')),'MON yyyy') AS SELECTEDMONTH ,  TC_CAPACITY,(SELECT DIV_CODE ||'-'|| DIV_NAME  FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE )OFF_CODE, ";
                    strQry += " (SELECT CM_CIRCLE_CODE  || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE )CIRCLE ,";
                    strQry += " (SELECT CM_CIRCLE_CODE  FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE  ) CIRCLE_CODE , ";
                    strQry += " (SELECT DIV_CODE   FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE) DIVISION_CODE ";
                    strQry += " , MD_NAME ,STATUS,DCR_RANGE,case GUARANTY_TYPE when 'AGP' THEN 'AGP' when 'WRGP' then 'WGRP' END GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM 0-30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME, DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " MD_TYPE = 'C'  AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += "  LEFT JOIN ";
                   // strQry += "  RIGHT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME  FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE )<= 30 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )D  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM 31-60DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " MD_TYPE = 'C' AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " 	) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN  ";
                    //strQry += " RIGHT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME    FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                    strQry += " ,TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID  AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE ) > 30 )AND  (round(SYSDATE- RSD_INV_DATE ) <= 60 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                    strQry += " ) E  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM 61-90DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " MD_TYPE = 'C'  AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " 	) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN  ";
                    //strQry += " RIGHT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME    FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                    strQry += " ,TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID  AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE ) > 60 )AND  (round(SYSDATE- RSD_INV_DATE ) <= 90 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                    strQry += " ) F  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM ABOVE 90DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " MD_TYPE = 'C' AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " 	) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN  ";
                   // strQry += " RIGHT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME    FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                    strQry += " ,TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID  AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') = TO_DATE('" + objReport.sCurrentMonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE ) > 90 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                    strQry += " ) G ";
                    strQry += " ) K where GUARANTY_TYPE!='WGP' ) ";
                    if (offCode.Length == 1)
                    {
                        strQry += " WHERE CIRCLE_CODE = '" + offCode + "' ";
                    }
                    if (offCode.Length == 2)
                    {
                        strQry += " WHERE DIVISION_CODE = '" + offCode + "' ";
                    }
                }
                else if (objReport.sFinancialYear != "")
                {
                    string aprilMonth = "04" + objReport.sFinancialYear;
                    string marchmonth = string.Empty;
                    string financialYear = string.Empty;
                    DateTime dFromDate = DateTime.ParseExact(objReport.sFinancialYear.Replace('-', '/'), "yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dFromDate = dFromDate.AddYears(1);
                    marchmonth = Convert.ToString(dFromDate.Year);
                    financialYear = objReport.sFinancialYear + '-' + Convert.ToString(dFromDate.Year);
                    marchmonth = "03" + marchmonth;
                    strQry = " SELECT * FROM (";
                    strQry += "SELECT '" + financialYear + "'  AS SELECTEDYEAR , '' AS SELECTEDMONTH ,  TC_CAPACITY,(SELECT DIV_CODE ||'-'|| DIV_NAME  FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE )OFF_CODE, ";
                    strQry += " (SELECT CM_CIRCLE_CODE  || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE )CIRCLE ,";
                    strQry += " (SELECT CM_CIRCLE_CODE  FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE  ) CIRCLE_CODE , ";
                    strQry += " (SELECT DIV_CODE   FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE) DIVISION_CODE ";
                    strQry += " , MD_NAME ,STATUS,DCR_RANGE,case GUARANTY_TYPE when 'AGP' THEN 'AGP' when 'WRGP' then 'WRGP' END GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM 0-30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME, DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " MD_TYPE = 'C' AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                     strQry += "  LEFT JOIN ";
                   // strQry += "  RIGHT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME  FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE )<= 30 )  )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )D  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM 31-60DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME, DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " MD_TYPE = 'C'  AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                    strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += "  LEFT JOIN ";
                  //  strQry += "  RIGHT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME  FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                    strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE )> 30 ) AND  (round(SYSDATE- RSD_INV_DATE ) <= 60 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                    strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                    strQry += " DCR_RANGE )D  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM 61-90DAYS' , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " MD_TYPE = 'C' AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " GUARANTY_TYPE FROM DUAL) ";
                    strQry += " 	 UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN  ";
                   // strQry += " RIGHT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME    FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                    strQry += " ,TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID  AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE ) > 60 )AND  (round(SYSDATE- RSD_INV_DATE ) <= 90 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                    strQry += " ) E  ";
                    strQry += " UNION ALL ";
                    strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM ABOVE 90DAYS' , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                    strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " MD_TYPE = 'C' AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " GUARANTY_TYPE FROM DUAL) ";
                    strQry += " 	 UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += " LEFT JOIN  ";
                   // strQry += " RIGHT JOIN  ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME    FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                    strQry += " ,TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID  AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE  AND  ";
                    strQry += " ";
                    strQry += " TO_DATE(TO_CHAR(RSD_INV_DATE,'MMyyyy'),'MMyyyy') BETWEEN  TO_DATE('" + aprilMonth + "','MMyyyy')  AND  TO_DATE('" + marchmonth + "','MMyyyy')  AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND (round(SYSDATE- RSD_INV_DATE ) > 90 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                    strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                    strQry += " ) R  ";
                    strQry += " ) F where GUARANTY_TYPE!='WGP' ) ";
                    if (offCode.Length == 1)
                    {
                        strQry += " WHERE CIRCLE_CODE = '" + offCode + "' ";
                    }
                    if (offCode.Length == 2)
                    {
                        strQry += " WHERE DIVISION_CODE = '" + offCode + "' ";
                    }
                }
                else
                {
                        strQry = " SELECT * FROM (";
                        strQry += "SELECT null  AS SELECTEDYEAR , null AS SELECTEDMONTH ,  TC_CAPACITY,(SELECT DIV_CODE ||'-'|| DIV_NAME  FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE )OFF_CODE, ";
                        strQry += " (SELECT CM_CIRCLE_CODE  || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE )CIRCLE ,";
                        strQry += " (SELECT CM_CIRCLE_CODE  FROM TBLCIRCLE WHERE SUBSTR(OFF_CODE,0,1) = CM_CIRCLE_CODE  ) CIRCLE_CODE , ";
                        strQry += " (SELECT DIV_CODE   FROM TBLDIVISION WHERE DIV_CODE = OFF_CODE) DIVISION_CODE ";
                        strQry += " , MD_NAME ,STATUS,DCR_RANGE,case GUARANTY_TYPE when 'AGP' THEN 'AGP' when 'WRGP' then 'WRGP' END GUARANTY_TYPE , TR_NAME FROM ( ";
                        strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM 0-30DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                        strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME, DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " MD_TYPE = 'C' AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                        strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    //strQry += "  RIGHT JOIN ";
                    strQry += "  LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME  FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                        strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE )<= 30 )  )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                        strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                        strQry += " DCR_RANGE )D  ";
                        strQry += " UNION ALL ";
                        strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM 31-60DAYS' AS STATUS , DCR_RANGE ,GUARANTY_TYPE, TR_NAME FROM ( ";
                        strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME, DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " MD_TYPE = 'C'  AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP' ";
                    strQry += " GUARANTY_TYPE FROM DUAL ";
                        strQry += " ) UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += "  LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME  FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER, ";
                        strQry += " TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID AND RSM_SUPREP_TYPE = 2 AND RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE )> 30 ) AND  (round(SYSDATE- RSD_INV_DATE ) <= 60 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B. ";
                        strQry += " TC_CAPACITY AND A.GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, ";
                        strQry += " DCR_RANGE )D  ";
                        strQry += " UNION ALL ";
                        strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM 61-90DAYS' , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                        strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " MD_TYPE = 'C'  AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                    strQry += " GUARANTY_TYPE FROM DUAL) ";
                        strQry += " UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                    strQry += "  LEFT JOIN ";
                    strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME    FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                        strQry += " ,TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID  AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  (round(SYSDATE- RSD_INV_DATE ) > 60 )AND  (round(SYSDATE- RSD_INV_DATE ) <= 90 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                        strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                        strQry += " ) E  ";
                        strQry += " UNION ALL ";
                        strQry += " SELECT * FROM (SELECT COUNT(TC_CAPACITY) TC_CAPACITY , OFF_CODE , MD_NAME, 'PENDING FROM ABOVE 90DAYS' , DCR_RANGE ,GUARANTY_TYPE , TR_NAME FROM ( ";
                        strQry += " SELECT * FROM (SELECT  OFF_CODE , MD_NAME,  DCR_RANGE FROM TBLMASTERDATA , VIEW_ALL_OFFICES , TBLDTRCAPACITYRANGE WHERE ";
                    //strQry += " MD_TYPE = 'C' AND DCR_CAPACITY <500 AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                        strQry += " MD_TYPE = 'C' AND LENGTH(OFF_CODE) = 2 AND MD_NAME  = DCR_CAPACITY ) CROSS JOIN (SELECT * FROM  (SELECT 'WRGP'  ";
                        strQry += " GUARANTY_TYPE FROM DUAL) ";
                        strQry += "  UNION ALL  (SELECT 'WGP' GUARANTY_TYPE FROM DUAL) UNION ALL  (SELECT 'AGP' GUARANTY_TYPE FROM DUAL) )) A ";
                        strQry += "  LEFT JOIN ";
                        strQry += " (SELECT RSM_GUARANTY_TYPE , RSD_TC_CODE , RSM_DIV_CODE  , TC_CAPACITY ,(SELECT TR_NAME FROM   TBLTRANSREPAIRER B WHERE A.TR_TR_ID = B.TR_ID)  TR_NAME    FROM TBLREPAIRSENTMASTER , TBLREPAIRSENTDETAILS ,TBLTCMASTER ";
                        strQry += " ,TBLTRANSREPAIRER A  WHERE TR_ID = RSM_SUPREP_ID  AND RSM_SUPREP_TYPE = 2 AND  RSM_ID = RSD_RSM_ID AND  RSD_TC_CODE = TC_CODE AND RSD_DELIVARY_DATE IS NULL and RSD_DELIVER_CHALLEN_NO IS NULL  AND RSD_RV_NO IS NULL  AND RSD_MMS_AUTO_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND (round(SYSDATE- RSD_INV_DATE ) > 90 ) )B   ON B.RSM_DIV_CODE = A.OFF_CODE AND A.MD_NAME = B.TC_CAPACITY AND A. ";
                        strQry += " GUARANTY_TYPE = B.RSM_GUARANTY_TYPE  GROUP BY MD_NAME,DCR_RANGE, OFF_CODE,GUARANTY_TYPE ,TR_NAME ORDER BY OFF_CODE, DCR_RANGE ";
                        strQry += " ) R  ";
                        strQry += " ) F where GUARANTY_TYPE!='WGP' ) ";
                        if (offCode.Length == 1)
                        {
                            strQry += " WHERE CIRCLE_CODE = '" + offCode + "' ";
                        }
                        if (offCode.Length == 2)
                        {
                            strQry += " WHERE DIVISION_CODE = '" + offCode + "' ";
                        }
                    }
                OleDbDataReader dr = ObjCon.Fetch(strQry);
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable PrintFeederBifurcationReport(clsReports objReport)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT";
                strQry += " (SELECT CM_CIRCLE_NAME  FROM TBLCIRCLE WHERE CM_CIRCLE_CODE = SUBSTR(DT_OM_SLNO, 0,1 )) CIRCLE , ";
                strQry += " (SELECT DIV_NAME  			FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(DT_OM_SLNO, 0,2 )) DIVISION , ";
                strQry += " (SELECT SD_SUBDIV_NAME	FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE =SUBSTR(DT_OM_SLNO, 0,3 )) SUBDIVISION , ";
                strQry += " (SELECT OM_NAME					FROM TBLOMSECMAST WHERE OM_CODE = DT_OM_SLNO) SECTION, ";
                strQry += "(SELECT FD_FEEDER_NAME  FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE = FBD_OLD_FEEDER_CODE ) FBD_OLD_FEEDER_NAME ,";
                strQry += " (SELECT FD_FEEDER_NAME  FROM TBLFEEDERMAST WHERE FD_FEEDER_CODE = FBD_NEW_FEEDER_CODE ) FBD_NEW_FEEDER_NAME , ";
                strQry += " (SELECT DT_NAME  FROM TBLDTCMAST WHERE DT_CODE = FBD_NEW_DTC_CODE )DTC_NAME , ";
                strQry += " DT_OM_SLNO, TO_CHAR(FB_OM_DATE,'DD-MON-YYYY')FB_OM_DATE  ,FBD_OLD_FEEDER_CODE , FBD_NEW_FEEDER_CODE,FBD_OLD_DTC_CODE , FBD_NEW_DTC_CODE ,FBD_DTR_CODE AS DTR_CODE, ED_ID, ";
                strQry += " (CASE WHEN ED_IS_INTERNAL_ENUM = 0 THEN 'PENDING' ELSE 'COMPLETED' end ) ENUMSTATUS ";
                strQry += " FROM TBLFEEDER_BIFURCATION_DETAILS left join TBLFEEDERBIFURCATION ON FB_ID = FBD_FB_ID  left join  TBLDTCMAST on FBD_NEW_DTC_CODE = DT_CODE ";
                strQry += " LEFT join TBLDTCENUMERATION  on DTE_DTCCODE = FBD_NEW_DTC_CODE LEFT JOIN TBLENUMERATIONDETAILS ON  ED_ID = DTE_ED_ID  WHERE ED_IS_FEEDER_BIFURCATION  = 1  ";
                if (!(objReport.sReportType == null || objReport.sReportType == ""))
                {
                    strQry += " AND ED_IS_INTERNAL_ENUM IN (" + objReport.sReportType + ") ";
                }
                if (!(objReport.sOfficeCode == null || objReport.sOfficeCode == ""))
                {
                    strQry += " AND DT_OM_SLNO LIKE '" + objReport.sOfficeCode + "%' ";
                }
                if (!(objReport.sOldFeederCode == null || objReport.sOldFeederCode == ""))
                {
                    strQry += " AND FBD_OLD_FEEDER_CODE = '" + objReport.sOldFeederCode + "'";
                }
                if (!(objReport.sNewFeederCode == null || objReport.sNewFeederCode == ""))
                {
                    strQry += " AND FBD_NEW_FEEDER_CODE = '" + objReport.sNewFeederCode + "'";
                }
                if (!(objReport.sFeederBifurcationID == null || objReport.sFeederBifurcationID == ""))
                {
                    strQry += " AND FB_ID = " + objReport.sFeederBifurcationID + "";
                }
                //date selection
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " AND TO_CHAR(FB_OM_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(FB_OM_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " AND TO_CHAR(FB_OM_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "' ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " AND TO_CHAR(FB_OM_DATE,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(FB_OM_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(FB_OM_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " ORDER BY DT_OM_SLNO,  FBD_OLD_DTC_CODE, FBD_NEW_DTC_CODE ";
                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        #region old guarenty type change query

        //strQry = " SELECT \"CIRCLE\", A.\"DIV_NAME\" as \"DIVISION\" ,REPAIRER_NAME,TC_CAPACITY,\"COUNT\" FROM  ";
        //strQry += " (SELECT   \"CIRCLE\", A.\"DIV_NAME\" ,\"COUNT\",REPAIRER_NAME,TC_CAPACITY FROM	 ( ";
        //strQry += " SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE   FROM \"TBLDIVISION\"   ";
        //strQry += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" where DIV_CODE like '" + objReport.sOfficeCode + "%')A ";
        //strQry += " LEFT JOIN (SELECT COUNT(TC_CAPACITY) AS \"COUNT\",DIV_CODE,DIV_NAME,TR_NAME AS REPAIRER_NAME, ";
        //strQry += " MD_NAME AS TC_CAPACITY   from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" ";
        //strQry += " on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
        //strQry += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
        //strQry += " INNER JOIN TBLMASTERDATA ON MD_NAME=TC_CAPACITY  RIGHT JOIN TBLDIVISION ON DIV_CODE=TC_LOCATION_ID ";
        //strQry += " WHERE RSM_GUARANTY_TYPE='AGP' and TC_STATUS=3 AND TC_CURRENT_LOCATION=3 AND RSD_RV_NO IS NULL ";
        //strQry += " AND RSD_RV_DATE IS NULL AND  TC_CAPACITY=25 AND    MD_TYPE='C' AND        ";
        //strQry += " TC_CODE IN (SELECT A.RSD_TC_CODE from  (SELECT max(RSD_RV_DATE) as RSD_RV_DATE,RSD_TC_CODE from ";
        //strQry += " \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER ";
        //strQry += " on RSD_RSM_ID=RSM_ID      GROUP BY   RSD_TC_CODE)A 	inner join ";
        //strQry += " (SELECT RSD_TC_CODE,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" ";
        //strQry += " on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER ";
        //strQry += " on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP' and TC_STATUS=3 ";
        //strQry += " AND TC_CURRENT_LOCATION=3 AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  )B ";
        //strQry += " on A.RSD_RV_DATE=B.RSD_RV_DATE and A.RSD_TC_CODE=B.RSD_TC_CODE)  and DIV_CODE like '" + objReport.sOfficeCode+"%' ";
        //if (objReport.sFromDate != null && objReport.sTodate != null)
        //{
        //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //}
        //strQry += " GROUP BY TC_CAPACITY,DIV_CODE,DIV_NAME,TR_NAME,MD_NAME)X ";
        //strQry += "    ON A.DIV_CODE=X.DIV_CODE ";

        //strQry += " UNION ALL ";

        //strQry += "   SELECT   \"CIRCLE\", A.\"DIV_NAME\", \"COUNT\", REPAIRER_NAME ,TC_CAPACITY  FROM	 ( ";
        //strQry += " SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE   FROM \"TBLDIVISION\"   ";
        //strQry += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" where DIV_CODE like '" + objReport.sOfficeCode + "%')A ";
        //strQry += " LEFT JOIN  (SELECT COUNT(TC_CAPACITY) AS \"COUNT\",DIV_CODE,DIV_NAME,TR_NAME AS REPAIRER_NAME, ";
        //strQry += " MD_NAME AS TC_CAPACITY   from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" on ";
        //strQry += " IND_RSD_ID =RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER on ";
        //strQry += " TC_CODE =RSD_TC_CODE INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   INNER JOIN TBLMASTERDATA ";
        //strQry += " ON MD_NAME=TC_CAPACITY  RIGHT JOIN TBLDIVISION ON DIV_CODE=TC_LOCATION_ID ";
        //strQry += " WHERE RSM_GUARANTY_TYPE='AGP' and TC_STATUS=3 AND TC_CURRENT_LOCATION=3 AND RSD_RV_NO IS NULL AND ";
        //strQry += " RSD_RV_DATE IS NULL AND  MD_TYPE='C'  AND  TC_CAPACITY=63 AND TC_CODE IN (SELECT A.RSD_TC_CODE ";
        //strQry += " from  (SELECT max(RSD_RV_DATE) as RSD_RV_DATE,RSD_TC_CODE from \"TBLREPAIRSENTDETAILS\" inner join ";
        //strQry += " \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSD_RSM_ID=RSM_ID     ";
        //strQry += " GROUP BY   RSD_TC_CODE)A	inner join 	(SELECT RSD_TC_CODE,RSD_RV_DATE,RSM_DIV_CODE from ";
        //strQry += " \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join ";
        //strQry += " TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER on TC_CODE=RSD_TC_CODE  ";
        //strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP' and TC_STATUS=3 AND TC_CURRENT_LOCATION=3 ";
        //strQry += " AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  )B ";
        //strQry += " on A.RSD_RV_DATE=B.RSD_RV_DATE and A.RSD_TC_CODE=B.RSD_TC_CODE) and  DIV_CODE like '" + objReport.sOfficeCode + "%' ";
        //if (objReport.sFromDate != null && objReport.sTodate != null)
        //{
        //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //}
        //strQry += " GROUP BY TC_CAPACITY,DIV_CODE,DIV_NAME,TR_NAME,MD_NAME)X 	ON A.DIV_CODE=X.DIV_CODE ";

        //strQry += " UNION ALL ";

        //strQry += " SELECT   \"CIRCLE\", A.\"DIV_NAME\", \"COUNT\",REPAIRER_NAME,TC_CAPACITY FROM	 ( ";
        //strQry += " SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE   FROM \"TBLDIVISION\"   ";
        //strQry += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" where DIV_CODE like '" + objReport.sOfficeCode + "%')A LEFT JOIN ";
        //strQry += " (SELECT COUNT(TC_CAPACITY) AS \"COUNT\",DIV_CODE,DIV_NAME,TR_NAME AS REPAIRER_NAME,MD_NAME AS TC_CAPACITY ";
        //strQry += " from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join ";
        //strQry += " TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER on TC_CODE=RSD_TC_CODE   ";
        //strQry += " INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID  INNER JOIN TBLMASTERDATA ON MD_NAME=TC_CAPACITY    ";
        //strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=TC_LOCATION_ID 	WHERE RSM_GUARANTY_TYPE='AGP' and TC_STATUS=3 ";
        //strQry += " AND TC_CURRENT_LOCATION=3 AND RSD_RV_NO IS NULL AND     MD_TYPE='C'       AND RSD_RV_DATE IS NULL AND ";
        //strQry += " TC_CAPACITY =100 AND TC_CODE IN (SELECT A.RSD_TC_CODE ";
        //strQry += " from  (SELECT max(RSD_RV_DATE) as RSD_RV_DATE,RSD_TC_CODE from \"TBLREPAIRSENTDETAILS\" inner join ";
        //strQry += " \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSD_RSM_ID=RSM_ID     ";
        //strQry += " GROUP BY   RSD_TC_CODE)A	inner join	(SELECT RSD_TC_CODE,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" ";
        //strQry += " inner join \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
        //strQry += " inner join TBLTCMASTER on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP' ";
        //strQry += " and TC_STATUS=3 AND TC_CURRENT_LOCATION=3 AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  )B ";
        //strQry += " on A.RSD_RV_DATE=B.RSD_RV_DATE and A.RSD_TC_CODE=B.RSD_TC_CODE) and  DIV_CODE like '" + objReport.sOfficeCode + "%'    ";
        //if (objReport.sFromDate != null && objReport.sTodate != null)
        //{
        //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //}
        //strQry += " GROUP BY TC_CAPACITY,DIV_CODE,DIV_NAME ,TR_NAME,MD_NAME)X ";
        //strQry += " ON A.DIV_CODE=X.DIV_CODE ";

        //strQry += " UNION ALL ";

        //strQry += " SELECT   \"CIRCLE\", A.\"DIV_NAME\", \"COUNT\",REPAIRER_NAME,TC_CAPACITY FROM	 ( ";
        //strQry += " SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE   FROM \"TBLDIVISION\"   ";
        //strQry += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" where DIV_CODE like '" + objReport.sOfficeCode + "%')A LEFT JOIN ";
        //strQry += " (SELECT COUNT(TC_CAPACITY) AS \"COUNT\",DIV_CODE,DIV_NAME,TR_NAME AS REPAIRER_NAME,MD_NAME AS TC_CAPACITY ";
        //strQry += " from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join ";
        //strQry += " TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER on TC_CODE=RSD_TC_CODE INNER JOIN ";
        //strQry += " TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID    INNER JOIN TBLMASTERDATA ON MD_NAME=TC_CAPACITY ";
        //strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=TC_LOCATION_ID ";
        //strQry += " WHERE RSM_GUARANTY_TYPE='AGP' and TC_STATUS=3 AND TC_CURRENT_LOCATION=3  AND     MD_TYPE='C' ";
        //strQry += " AND  RSD_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  TC_CAPACITY=250 AND TC_CODE IN (SELECT A.RSD_TC_CODE ";
        //strQry += "	from  (SELECT max(RSD_RV_DATE) as RSD_RV_DATE,RSD_TC_CODE from \"TBLREPAIRSENTDETAILS\" inner join ";
        //strQry += " \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSD_RSM_ID=RSM_ID ";
        //strQry += " GROUP BY   RSD_TC_CODE)A	inner join	(SELECT RSD_TC_CODE,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" ";
        //strQry += " inner join \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
        //strQry += " TBLTCMASTER on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP' ";
        //strQry += " and TC_STATUS=3 AND TC_CURRENT_LOCATION=3 AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  )B ";
        //strQry += " on A.RSD_RV_DATE=B.RSD_RV_DATE and A.RSD_TC_CODE=B.RSD_TC_CODE) and  DIV_CODE like '" + objReport.sOfficeCode + "%' ";
        //if (objReport.sFromDate != null && objReport.sTodate != null)
        //{
        //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //}
        //strQry += " GROUP BY TC_CAPACITY,DIV_CODE,DIV_NAME,TR_NAME,MD_NAME)X 	ON A.DIV_CODE=X.DIV_CODE ";

        //strQry += " UNION ALL ";

        //strQry += " SELECT   \"CIRCLE\", A.\"DIV_NAME\", \"COUNT\",REPAIRER_NAME,TC_CAPACITY FROM	 ( ";
        //strQry += " SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE   FROM \"TBLDIVISION\"   inner JOIN ";
        //strQry += " \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" where DIV_CODE like '" + objReport.sOfficeCode + "%')A LEFT JOIN ";
        //strQry += " (SELECT COUNT(TC_CAPACITY) AS \"COUNT\",DIV_CODE,DIV_NAME,TR_NAME AS REPAIRER_NAME,MD_NAME AS TC_CAPACITY ";
        //strQry += " from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER ";
        //strQry += " on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER on TC_CODE=RSD_TC_CODE INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID ";
        //strQry += " INNER JOIN TBLMASTERDATA ON MD_NAME=TC_CAPACITY  RIGHT JOIN TBLDIVISION ON DIV_CODE=TC_LOCATION_ID ";
        //strQry += " WHERE RSM_GUARANTY_TYPE='AGP' and TC_STATUS=3 AND TC_CURRENT_LOCATION=3  AND    MD_TYPE='C'      ";
        //strQry += " AND RSD_RV_NO IS NULL AND RSD_RV_DATE IS NULL AND  TC_CAPACITY=500 AND TC_CODE IN (SELECT A.RSD_TC_CODE ";
        //strQry += " from  (SELECT max(RSD_RV_DATE) as RSD_RV_DATE,RSD_TC_CODE from \"TBLREPAIRSENTDETAILS\" inner join ";
        //strQry += " \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSD_RSM_ID=RSM_ID   ";
        //strQry += " GROUP BY   RSD_TC_CODE)A 	inner join ";
        //strQry += " (SELECT RSD_TC_CODE,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" ";
        //strQry += " on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER on TC_CODE=RSD_TC_CODE ";
        //strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP' and TC_STATUS=3 AND TC_CURRENT_LOCATION=3 AND ";
        //strQry += " RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  )B	on A.RSD_RV_DATE=B.RSD_RV_DATE and ";
        //strQry += " A.RSD_TC_CODE=B.RSD_TC_CODE)  and  DIV_CODE like '" + objReport.sOfficeCode + "%'    ";
        //if (objReport.sFromDate != null && objReport.sTodate != null)
        //{
        //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //}
        //strQry += " GROUP BY TC_CAPACITY,DIV_CODE,DIV_NAME,TR_NAME,MD_NAME)X ";
        //strQry += " ON A.DIV_CODE=X.DIV_CODE ";

        //strQry += " UNION ALL";

        //strQry += " SELECT   \"CIRCLE\", A.\"DIV_NAME\", \"COUNT\",REPAIRER_NAME,TC_CAPACITY FROM	 ( ";
        //strQry += " SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE   FROM \"TBLDIVISION\"   inner JOIN \"TBLCIRCLE\" ON  ";
        //strQry += " \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" where DIV_CODE like '" + objReport.sOfficeCode + "%')A LEFT JOIN ";
        //strQry += " (SELECT COUNT(TC_CAPACITY) AS \"COUNT\",DIV_CODE,DIV_NAME,TR_NAME AS REPAIRER_NAME,MD_NAME AS TC_CAPACITY ";
        //strQry += " from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER ";
        //strQry += " on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID  ";
        //strQry += " INNER JOIN TBLMASTERDATA ON MD_NAME=TC_CAPACITY  RIGHT JOIN TBLDIVISION ON DIV_CODE=TC_LOCATION_ID ";
        //strQry += " WHERE RSM_GUARANTY_TYPE='AGP' and TC_STATUS=3 AND TC_CURRENT_LOCATION=3  AND    MD_TYPE='C'AND RSD_RV_NO IS NULL ";
        //strQry += " AND RSD_RV_DATE IS NULL AND  TC_CAPACITY>500 AND TC_CODE IN (SELECT A.RSD_TC_CODE ";
        //strQry += " from  (SELECT max(RSD_RV_DATE) as RSD_RV_DATE,RSD_TC_CODE from \"TBLREPAIRSENTDETAILS\" inner join ";
        //strQry += " \"TBLINSPECTIONDETAILS\" on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSD_RSM_ID=RSM_ID     ";
        //strQry += " GROUP BY   RSD_TC_CODE)A 	inner join ";
        //strQry += " (SELECT RSD_TC_CODE,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\" on ";
        //strQry += " IND_RSD_ID =RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER on TC_CODE=RSD_TC_CODE   ";
        //strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP' and TC_STATUS=3 AND TC_CURRENT_LOCATION=3 AND RSD_RV_NO IS NOT NULL ";
        //strQry += " AND RSD_RV_DATE IS NOT NULL  )B	on A.RSD_RV_DATE=B.RSD_RV_DATE and A.RSD_TC_CODE=B.RSD_TC_CODE) and  DIV_CODE like '" + objReport.sOfficeCode + "%' ";
        //if (objReport.sFromDate != null && objReport.sTodate != null)
        //{
        //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY/MM/DD')<='" + objReport.sTodate + "'  ";
        //}
        //strQry += " GROUP BY TC_CAPACITY,DIV_CODE,DIV_NAME,TR_NAME,MD_NAME)X ";
        //strQry += "	ON A.DIV_CODE=X.DIV_CODE)A  ORDER BY  DIV_NAME ";
        #endregion
               
        public DataTable GuarentyTypeChangedDetails(clsReports objReport)
        {
            DataTable SameRepairDt = new DataTable();
            DataTable DiffRepairDt = new DataTable();

            DataTable SameDiffRepairDt = new DataTable();


            try
            {
                string strQry = string.Empty;
                string strQry1 = string.Empty;

                #region REF QUERY
                //strQry = "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '25' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=25 ";
                //strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";

                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += " )A ";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=25)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '63' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%'";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=63 ";
                //strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += " )A";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=63)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '100' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%'";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=100  ";
                //strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += " )A";
                //strQry += " inner join ";
                //strQry += "(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=100)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM  ";
                //strQry += "(SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '250' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%'";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=250 and DIV_CODE like '" + objReport.sOfficeCode + "%' ";

                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "  )A ";

                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=250)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=500 and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "   )A ";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=500)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL  ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM  ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, 'Above500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" where DIV_CODE like '" + objReport.sOfficeCode + "%')A  LEFT JOIN  ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY>500 and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "  )A ";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY>500)B ";
                //strQry += "  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE";
                #endregion

                #region
                //Dtr Invoiced To Same Repairer
                //strQry = "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '25' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=25 ";
                //strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";

                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += " )A ";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=25)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME=B.TR_NAME GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '63' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%'";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=63 ";
                //strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += " )A";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=63)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  AND A.REPAIRER_NAME=B.TR_NAME GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '100' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%'";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=100  ";
                //strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += " )A";
                //strQry += " inner join ";
                //strQry += "(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE  INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=100)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID AND A.REPAIRER_NAME=B.TR_NAME  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM  ";
                //strQry += "(SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '250' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%'";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=250 and DIV_CODE like '" + objReport.sOfficeCode + "%' ";

                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "  )A ";

                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE  INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=250)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  AND A.REPAIRER_NAME=B.TR_NAME GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=500 and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "   )A ";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=500)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  AND A.REPAIRER_NAME=B.TR_NAME  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL  ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM  ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, 'Above500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" where DIV_CODE like '" + objReport.sOfficeCode + "%')A  LEFT JOIN  ";
                //strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY>500 and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "  )A ";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE  INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY>500)B ";
                //strQry += "  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  AND A.REPAIRER_NAME=B.TR_NAME  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE";

                //SameRepairDt = ObjCon.getDataTable(strQry);
                //return SameRepairDt;




                // Dtr invoiced to Diffrerent Repairer 
                //strQry1 = " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                //strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '25' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                //strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                //strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                //strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                //strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                //strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and TC_CAPACITY=25  ";
                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                //strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=25       ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                //strQry1 += " RIGHT JOIN ( ";
                //strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                //strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                //strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=25       ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                //strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=25                 ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                //strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                //strQry1 += " UNION ALL ";

                //strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                //strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '63' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                //strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                //strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                //strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                //strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                //strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'   and TC_CAPACITY=63  ";
                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                //strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=63       ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                //strQry1 += " RIGHT JOIN ( ";
                //strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                //strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                //strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=63       ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                //strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=63                 ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                //strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                //strQry1 += " UNION ALL ";

                //strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                //strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '100' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                //strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                //strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                //strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                //strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                //strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'   and TC_CAPACITY=100  ";

                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                //strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=100       ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                //strQry1 += " RIGHT JOIN ( ";
                //strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                //strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                //strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=100       ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                //strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=100                 ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                //strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                //strQry1 += " UNION ALL ";

                //strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                //strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '250' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                //strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                //strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                //strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                //strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                //strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'   and TC_CAPACITY=250  ";

                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                //strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=250       ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                //strQry1 += " RIGHT JOIN ( ";
                //strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                //strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                //strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=250       ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                //strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=250                 ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                //strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                //strQry1 += " UNION ALL ";

                //strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                //strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                //strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                //strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                //strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                //strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                //strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and TC_CAPACITY=500  ";

                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                //strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=500       ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                //strQry1 += " RIGHT JOIN ( ";
                //strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                //strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                //strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=500       ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                //strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=500                 ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                //strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                //strQry1 += " UNION ALL ";

                //strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                //strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, 'Above500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                //strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                //strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                //strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                //strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                //strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'   and TC_CAPACITY>500  ";

                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                //strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY>500       ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                //strQry1 += " RIGHT JOIN ( ";
                //strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                //strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                //strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                //strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                //strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY>500       ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                //strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                //strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                //strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY>500                 ";
                //strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                //strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                //strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";


                //DiffRepairDt = ObjCon.getDataTable(strQry1);
                //return DiffRepairDt;



                #endregion

                // dtr invoiced to same repairer
                strQry = " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '25' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  ";
                strQry += " \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A  ";
                strQry += " LEFT JOIN ";
                strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  ";
                strQry += " RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from ";
                strQry += " \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER ";
                strQry += " on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=25  and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " and IND_INSP_RESULT =1 )A ";
                strQry += " inner join ";
                strQry += " (SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL ";
                strQry += " AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=25)B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  ";
                strQry += " AND A.REPAIRER_NAME=B.TR_NAME GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  ";
                strQry += " GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE  ";
                strQry += "UNION ALL ";
                // dtr invoiced to diff dtr
                strQry += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT      \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '25' AS TC_CAPACITY    FROM \"TBLDIVISION\"     ";
                strQry += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A";
                strQry += " LEFT JOIN  ";
                strQry += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM  ";
                strQry += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM";
                strQry += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\"  ";
                strQry += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join";
                strQry += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID       ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=25  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " and  IND_INSP_RESULT=1     )A ";
                strQry += "inner join ";
                strQry += "(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER   on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL ";
                strQry += " and TC_CAPACITY=25     )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME  ";
                strQry += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ";
                strQry += " ON A.DIV_CODE=B.RSM_DIV_CODE   ";
                // 25 capacity details

                strQry += "UNION ALL ";

                strQry += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '63' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  ";
                strQry += " \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A  ";
                strQry += " LEFT JOIN ";
                strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  ";
                strQry += " RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from ";
                strQry += " \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER ";
                strQry += " on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=63  and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " and IND_INSP_RESULT =1 )A ";
                strQry += " inner join ";
                strQry += " (SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL ";
                strQry += " AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=63)B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  ";
                strQry += " AND A.REPAIRER_NAME=B.TR_NAME GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  ";
                strQry += " GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE  ";
                strQry += "UNION ALL ";
                // dtr invoiced to diff dtr
                strQry += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT      \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '63' AS TC_CAPACITY    FROM \"TBLDIVISION\"     ";
                strQry += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A";
                strQry += " LEFT JOIN  ";
                strQry += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM  ";
                strQry += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM";
                strQry += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\"  ";
                strQry += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join";
                strQry += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID       ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=63  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " and  IND_INSP_RESULT=1     )A ";
                strQry += "inner join ";
                strQry += "(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER   on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL ";
                strQry += " and TC_CAPACITY=63     )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME  ";
                strQry += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ";
                strQry += " ON A.DIV_CODE=B.RSM_DIV_CODE   ";
                // 63 capacity details

                strQry += "UNION ALL ";

                strQry += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '100' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  ";
                strQry += " \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A  ";
                strQry += " LEFT JOIN ";
                strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  ";
                strQry += " RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from ";
                strQry += " \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER ";
                strQry += " on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=100  ";
                strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " and IND_INSP_RESULT =1 )A ";
                strQry += " inner join ";
                strQry += " (SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL ";
                strQry += " AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=100)B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  ";
                strQry += " AND A.REPAIRER_NAME=B.TR_NAME GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  ";
                strQry += " GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE  ";
                strQry += "UNION ALL ";
                // dtr invoiced to diff dtr
                strQry += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT      \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '100' AS TC_CAPACITY    FROM \"TBLDIVISION\"     ";
                strQry += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A";
                strQry += " LEFT JOIN  ";
                strQry += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM  ";
                strQry += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM";
                strQry += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\"  ";
                strQry += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join";
                strQry += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID       ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                strQry += " and TC_CAPACITY=100  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " and  IND_INSP_RESULT=1     )A ";
                strQry += "inner join ";
                strQry += "(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER   on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL ";
                strQry += " and TC_CAPACITY=100     )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME  ";
                strQry += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ";
                strQry += " ON A.DIV_CODE=B.RSM_DIV_CODE   ";
                // 100 capacity details

                strQry += "UNION ALL ";

                strQry += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '250' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  ";
                strQry += " \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A  ";
                strQry += " LEFT JOIN ";
                strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  ";
                strQry += " RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from ";
                strQry += " \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER ";
                strQry += " on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=250  ";
                strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " and IND_INSP_RESULT =1 )A ";
                strQry += " inner join ";
                strQry += " (SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL ";
                strQry += " AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=250)B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  ";
                strQry += " AND A.REPAIRER_NAME=B.TR_NAME GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  ";
                strQry += " GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE  ";
                strQry += "UNION ALL ";
                // dtr invoiced to diff dtr
                strQry += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT      \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '250' AS TC_CAPACITY    FROM \"TBLDIVISION\"     ";
                strQry += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A";
                strQry += " LEFT JOIN  ";
                strQry += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM  ";
                strQry += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM";
                strQry += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\"  ";
                strQry += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join";
                strQry += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID       ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                strQry += " and TC_CAPACITY=250  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " and  IND_INSP_RESULT=1     )A ";
                strQry += "inner join ";
                strQry += "(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER   on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL ";
                strQry += " and TC_CAPACITY=250     )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME  ";
                strQry += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ";
                strQry += " ON A.DIV_CODE=B.RSM_DIV_CODE   ";
                // 250 capacity details
                strQry += "UNION ALL ";

                strQry += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  ";
                strQry += " \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A  ";
                strQry += " LEFT JOIN ";
                strQry += "(SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  ";
                strQry += " RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from ";
                strQry += " \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER ";
                strQry += " on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=500  ";
                strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }

                strQry += " and IND_INSP_RESULT =1 )A ";
                strQry += " inner join ";
                strQry += " (SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL ";
                strQry += " AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=500)B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  ";
                strQry += " AND A.REPAIRER_NAME=B.TR_NAME GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  ";
                strQry += " GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE  ";
                strQry += "UNION ALL ";
                // dtr invoiced to diff dtr
                strQry += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT      \"CM_CIRCLE_NAME\" as ";
                strQry += " \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '500' AS TC_CAPACITY    FROM \"TBLDIVISION\"     ";
                strQry += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '%'  )A";
                strQry += " LEFT JOIN  ";
                strQry += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM  ";
                strQry += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM";
                strQry += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\"  ";
                strQry += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join";
                strQry += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID       ";
                strQry += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                strQry += " and TC_CAPACITY=500  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry += " and  IND_INSP_RESULT=1     )A ";
                strQry += "inner join ";
                strQry += "(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID ";
                strQry += " inner join TBLTCMASTER   on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID   ";
                strQry += " where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL ";
                strQry += " and TC_CAPACITY=500     )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME  ";
                strQry += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ";
                strQry += " ON A.DIV_CODE=B.RSM_DIV_CODE   ";
                // 500 capacity details


                SameDiffRepairDt = ObjCon.getDataTable(strQry);
                return SameDiffRepairDt;



            }
            catch (Exception ex)
            {
                clsException.LogError(ex.ToString(), ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return SameDiffRepairDt;
            }
        }

        /// <returns></returns>
        public DataTable GuarentyTypeChangedDetailsDiffRpr(clsReports objReport)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry1 = string.Empty;
                #region REF QUERY
                //strQry = "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '25' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=25 ";
                //strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";

                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += " )A ";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=25)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '63' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%'";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=63 ";
                //strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += " )A";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=63)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '100' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%'";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=100  ";
                //strQry += " and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += " )A";
                //strQry += " inner join ";
                //strQry += "(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=100)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM  ";
                //strQry += "(SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '250' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%'";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=250 and DIV_CODE like '" + objReport.sOfficeCode + "%' ";

                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "  )A ";

                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=250)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" ";
                //strQry += " where DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //strQry += " )A  LEFT JOIN ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY=500 and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "   )A ";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY=500)B ";
                //strQry += " ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE ";
                //strQry += " UNION ALL  ";
                //strQry += "SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM  ";
                //strQry += "(  SELECT     \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, 'Above500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\" where DIV_CODE like '" + objReport.sOfficeCode + "%')A  LEFT JOIN  ";
                //strQry += "(SELECT  COUNT(\"RSD_TC_CODE\") AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   FROM ";
                //strQry += "(SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP' and TC_CAPACITY>500 and DIV_CODE like '" + objReport.sOfficeCode + "%' ";
                //if (objReport.sFromDate != null && objReport.sTodate != null)
                //{
                //    strQry += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                //}
                //strQry += "  )A ";
                //strQry += " inner join ";
                //strQry += "	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE from \"TBLREPAIRSENTDETAILS\" inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  on TC_CODE=RSD_TC_CODE      where IND_INSP_RESULT=4  and  RSM_GUARANTY_TYPE='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL and  TC_CAPACITY>500)B ";
                //strQry += "  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID  GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B ON A.DIV_CODE=B.RSM_DIV_CODE";
                #endregion
                // Dtr invoiced to Diffrerent Repairer 
                strQry1 = " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '25' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=25  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=25       ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                strQry1 += " RIGHT JOIN ( ";
                strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=25       ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=25                 ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                strQry1 += " UNION ALL ";

                strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '63' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=63  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=63       ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                strQry1 += " RIGHT JOIN ( ";
                strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=63       ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=63                 ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                strQry1 += " UNION ALL ";

                strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '100' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=100  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=100       ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                strQry1 += " RIGHT JOIN ( ";
                strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=100       ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=100                 ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                strQry1 += " UNION ALL ";

                strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '250' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=250  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=250       ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                strQry1 += " RIGHT JOIN ( ";
                strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=250       ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=250                 ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                strQry1 += " UNION ALL ";

                strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, '500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=500  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=500       ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                strQry1 += " RIGHT JOIN ( ";
                strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY=500       ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY=500                 ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";

                strQry1 += " UNION ALL ";

                strQry1 += " SELECT CIRCLE,DIV_NAME as DIVISION ,REPAIRER_NAME,TC_CAPACITY,COUNT FROM (  SELECT     ";
                strQry1 += " \"CM_CIRCLE_NAME\" as \"CIRCLE\",\"DIV_NAME\",DIV_CODE, 'Above500' AS TC_CAPACITY    FROM \"TBLDIVISION\"    ";
                strQry1 += " inner JOIN \"TBLCIRCLE\" ON  \"CM_CIRCLE_CODE\"=\"DIV_CICLE_CODE\"  where DIV_CODE like '" + objReport.sOfficeCode + "%'  )A  LEFT JOIN ";

                strQry1 += " (SELECT  COUNT,RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE FROM  (SELECT  B.COUNT,B.RSM_DIV_CODE,B.REPAIRER_NAME,B.RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME,  RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME   ";
                strQry1 += " FROM (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   ";
                strQry1 += " from \"TBLREPAIRSENTDETAILS\" LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on ";
                strQry1 += " RSM_ID =RSD_RSM_ID inner join  TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID     ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY>500  ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join ";
                strQry1 += " TBLTCMASTER  on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY>500       ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID    AND  A.REPAIRER_NAME=B.TR_NAME ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME)S  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE )A ";
                strQry1 += " RIGHT JOIN ( ";
                strQry1 += " SELECT  COUNT(DISTINCT RSD_ID) AS COUNT,RSM_DIV_CODE,REPAIRER_NAME, RSD_TC_CODE FROM ";
                strQry1 += " (SELECT  RSM_PO_NO,A.\"RSD_TC_CODE\",MIN(A.\"RSD_ID\") AS RSD_ID,A.RSM_DIV_CODE,A.REPAIRER_NAME FROM ";
                strQry1 += " (SELECT RSD_TC_CODE,RSD_ID ,RSD_RV_DATE,RSM_DIV_CODE ,TR_NAME AS REPAIRER_NAME   from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " LEFT join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join  ";
                strQry1 += " TBLTCMASTER on TC_CODE=RSD_TC_CODE  INNER JOIN TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      ";
                strQry1 += " RIGHT JOIN TBLDIVISION ON DIV_CODE=RSM_DIV_CODE  WHERE RSM_GUARANTY_TYPE='AGP'  and DIV_CODE like '" + objReport.sOfficeCode + "%' and TC_CAPACITY>500       ";
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry1 += " AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(RSD_INV_DATE,'YYYY-MM-DD')<='" + objReport.sTodate + "'  ";
                }
                strQry1 += "      )A  inner join 	(SELECT  RSM_PO_NO,RSD_TC_CODE,RSD_ID,RSD_RV_DATE,RSM_DIV_CODE,TR_NAME from \"TBLREPAIRSENTDETAILS\" ";
                strQry1 += " inner join \"TBLINSPECTIONDETAILS\"  on IND_RSD_ID=RSD_ID inner join TBLREPAIRSENTMASTER on RSM_ID=RSD_RSM_ID inner join TBLTCMASTER  ";
                strQry1 += " on TC_CODE=RSD_TC_CODE INNER JOIN  TBLTRANSREPAIRER ON TR_ID=RSM_SUPREP_ID      where IND_INSP_RESULT=4  and ";
                strQry1 += " RSM_GUARANTY_TYPE ='WRGP'  AND RSD_RV_NO IS NOT NULL AND RSD_RV_DATE IS NOT NULL  and TC_CAPACITY>500                 ";
                strQry1 += "    )B  ON  B.RSD_TC_CODE=A.RSD_TC_CODE and B.RSD_ID < A.RSD_ID   AND A.REPAIRER_NAME!=B.TR_NAME         ";
                strQry1 += " GROUP BY a.\"RSD_TC_CODE\",RSM_PO_NO,A.RSM_DIV_CODE,A.REPAIRER_NAME )P  GROUP BY RSM_DIV_CODE,REPAIRER_NAME,RSD_TC_CODE)B  ";
                strQry1 += " ON A.RSD_TC_CODE=B.RSD_TC_CODE where A.RSD_TC_CODE is null and B.RSD_TC_CODE is not null)B)B  ON A.DIV_CODE=B.RSM_DIV_CODE  ";


                dt = ObjCon.getDataTable(strQry1);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.ToString(), ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
        }                            /// <param name="objReport"></param>

        public DataTable PrintFeederBifurcationReportSO(clsReports objReport)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = "  SELECT  (SELECT  FD_FEEDER_CODE ||'-'|| FD_FEEDER_NAME  FROM TBLFEEDERMAST WHERE FBDS_OLD_FEEDER_CODE = FD_FEEDER_CODE ) as OLDFEEDERCODE  , " +
                  "  (SELECT  ST_STATION_CODE || '-' || ST_NAME  FROM TBLSTATION, TBLFEEDERMAST WHERE FD_ST_ID = ST_ID AND FD_FEEDER_CODE = FBDS_OLD_FEEDER_CODE) OLD_STATION ," +
                  " (SELECT  ST_STATION_CODE || '-' || ST_NAME  FROM TBLSTATION, TBLFEEDERMAST WHERE FD_ST_ID = ST_ID AND FD_FEEDER_CODE = FBDS_NEW_FEEDER_CODE) NEW_STATION ," +
                    " (SELECT  FD_FEEDER_CODE ||'-'|| FD_FEEDER_NAME  FROM TBLFEEDERMAST WHERE FBDS_NEW_FEEDER_CODE  = FD_FEEDER_CODE ) as NEWFEEDERCODE , " +
                    " (select  substr(OFF_NAME,instr(OFF_NAME,':',1,1) + 1) from VIEW_ALL_OFFICES WHERE OFF_CODE =  SUBSTR(FBS_SECTION_CODE, 0, 1)) as CIRCLE , " +
                    " (select  substr(OFF_NAME,instr(OFF_NAME,':',1,1) + 1) from VIEW_ALL_OFFICES WHERE OFF_CODE =  SUBSTR(FBS_SECTION_CODE, 0, 2)) as DIVISION ," +
                    " (select  substr(OFF_NAME,instr(OFF_NAME,':',1,1) + 1) from VIEW_ALL_OFFICES WHERE OFF_CODE =  SUBSTR(FBS_SECTION_CODE, 0, 3)) as SUBDIVISON ," +
                    " (select  substr(OFF_NAME,instr(OFF_NAME,':',1,1) + 1) from VIEW_ALL_OFFICES WHERE OFF_CODE =  FBS_SECTION_CODE ) as Section ," +
                    " to_char(FBDS_CRON  ,  'dd-MON-yyyy') as CREATEDON , FBDS_OLD_DTC_CODE  , FBDS_NEW_DTC_CODE  ,  FBDS_DTR_CODE as DTRCODE , DT_NAME , FBS_SECTION_CODE, " +
                    " (select US_FULL_NAME ||', Role: '||RO_NAME   FROM TBLUSER ,TBLROLES  WHERE US_ID = FBS_CR_BY AND US_ROLE_ID = RO_ID ) as CREATEDBY , " +
                    " (select US_FULL_NAME ||', Role: '||RO_NAME   FROM TBLUSER ,TBLROLES  WHERE US_ID = FBS_APP_BY AND US_ROLE_ID = RO_ID ) as APPROVEDBY , " +
                    " CASE FBS_STATUS WHEN 0 THEN  'PENDING' WHEN 1 THEN 'APPROVED' WHEN 2 THEN 'FEEDER BIFURCATED' END AS STATUS " +
                    " FROM TBLFEEDER_BFCN_DETAILS_SO  INNER join TBLFEEDERBIFURCATION_SO on  FBDS_FB_ID  = FBS_ID  " +
                    " inner join   TBLDTCMAST on  ( FBDS_OLD_DTC_CODE  = DT_CODE  ) or (FBDS_NEW_DTC_CODE   = DT_CODE)   WHERE FBS_ID = '" + objReport.sFeederBifurcationID + "'";

                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable CapacityAbstractDetails(clsReports objReport)
        {
            DataTable dtCapacityAbstractDetails = new DataTable();
            string strQry = string.Empty;
            string fromdate = objReport.sFromDate;
            string todate = objReport.sTodate;
            try
            {
                strQry = " SELECT ";
                strQry += " (SELECT CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME   FROM TBLCIRCLE   WHERE CM_CIRCLE_CODE = SUBSTR(OFF_CODE , 0,1)) CIRCLE , ";
                strQry += "   (SELECT DIV_CODE || '-' || DIV_NAME   FROM TBLDIVISION    WHERE DIV_CODE    = SUBSTR(OFF_CODE , 0,2)) DIVISION  , ";
                strQry += "  (SELECT SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME  FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE = SUBSTR(OFF_CODE , 0,3))  SUBDIVISION , ";
                strQry += "  TC_CAPACITY , OFF_CODE , MD_NAME ,DT_CRON ,case  when DCR_RANGE > 500 then '>500' else DCR_RANGE end AS DCR_RANGE  , ";
                strQry += " to_char(to_date('" + objReport.sFromDate + "','yyyy/MM/dd'),'dd/MON/yyyy') AS FROM_DATE , to_char(to_date('" + objReport.sTodate + "','yyyy/MM/dd'),'dd/MON/yyyy') AS TO_DATE ,TO_CHAR(SYSDATE,'DD/MON/YYYY') as CURRENT_DATE   FROM  ";
                strQry += " (SELECT  count(TC_CAPACITY) TC_CAPACITY  , MD_NAME , OFF_CODE ,  DCR_RANGE ,DT_CRON  ";
                strQry += "  FROM (SELECT MD_NAME ,OFF_CODE  ,DCR_RANGE  FROM TBLMASTERDATA ,VIEW_ALL_OFFICES ,TBLDTRCAPACITYRANGE   WHERE MD_TYPE = 'C' AND ";
                strQry += "   LENGTH(OFF_CODE) = 3  AND MD_NAME = DCR_CAPACITY  ) A ";
                strQry += " LEFT JOIN  ";
                strQry += " (SELECT TC_CAPACITY ,DT_OM_SLNO , DT_CRON FROM TBLDTCMAST ,TBLTCMASTER WHERE DT_TC_ID = TC_CODE AND ";
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ) B ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "' ) B ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')) B ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DT_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "' ) B ";
                }
                strQry += " on B.TC_CAPACITY  =  A.MD_NAME and SUBSTR(DT_OM_SLNO , 0,3) = OFF_CODE  group by ";
                strQry += "   MD_NAME ,DCR_RANGE ,DT_CRON ,OFF_CODE ) ORDER BY CIRCLE , DIVISION,SUBDIVISION  ";
                dtCapacityAbstractDetails = ObjCon.getDataTable(strQry);
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
            return dtCapacityAbstractDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable WorkwiseAbstractDetails(clsReports objReport)
        {
            DataTable dtWorkwiseAbstractDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " SELECT (SELECT CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME   FROM TBLCIRCLE   WHERE CM_CIRCLE_CODE = SUBSTR(OFF_CODE , 0,1)) CIRCLE ,";
                strQry += "  (SELECT DIV_CODE || '-' || DIV_NAME   FROM TBLDIVISION    WHERE DIV_CODE    = SUBSTR(OFF_CODE , 0,2)) DIVISION  , ";
                strQry += " 	DT_CODE,  OFF_CODE , DT_CRON , MD_NAME, TO_CHAR( (TO_DATE('" + objReport.sFromDate + "','yyyy/MM/dd')),'DD/MON/YYYY') AS FROM_DATE ,TO_CHAR( (TO_DATE('" + objReport.sTodate + "','yyyy/MM/dd')),'DD/MON/YYYY') AS TO_DATE,TO_CHAR(SYSDATE,'DD/MON/YYYY') as CURRENT_DATE  FROM (SELECT   COUNT(DT_CRON) DT_CODE  ,OFF_CODE ,DT_CRON ,MD_NAME ";
                strQry += " FROM (SELECT MD_NAME  ,  OFF_CODE     FROM TBLMASTERDATA ,VIEW_ALL_OFFICES    WHERE MD_TYPE = 'PT' AND LENGTH(OFF_CODE) = 2 ) A ";
                strQry += " LEFT JOIN ";
                strQry += " (SELECT  DT_OM_SLNO , DT_CRON ,MD_NAME as PROJECTYPE FROM TBLDTCMAST LEFT JOIN  TBLMASTERDATA ON  MD_ID = DT_PROJECTTYPE   WHERE MD_TYPE = 'PT' AND ";
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ) B ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "' ) B ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')) B ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DT_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "' ) B ";
                }
                strQry += " on B.PROJECTYPE  =  A.MD_NAME ";
                strQry += " and SUBSTR(DT_OM_SLNO , 0,2) = OFF_CODE GROUP BY OFF_CODE ,DT_CRON ,MD_NAME )  ";
                dtWorkwiseAbstractDetails = ObjCon.getDataTable(strQry);
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
            return dtWorkwiseAbstractDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objReport"></param>
        /// <returns></returns>
        public DataTable FeederOrCategoryAbstractDetails(clsReports objReport)
        {
            DataTable dtFeederOrCategoryAbstractDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = "  SELECT (SELECT CM_CIRCLE_CODE || '-' || CM_CIRCLE_NAME   FROM TBLCIRCLE   WHERE CM_CIRCLE_CODE = SUBSTR(OFF_CODE , 0,1)) CIRCLE ,";
                strQry += " (SELECT DIV_CODE || '-' || DIV_NAME   FROM TBLDIVISION    WHERE DIV_CODE    = SUBSTR(OFF_CODE , 0,2)) DIVISION  , ";
                strQry += " (SELECT SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME  FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE = SUBSTR(OFF_CODE , 0,3))  SUBDIVISION ,DT_CODE ,  FT_NAME ,DT_CRON , TO_CHAR( (TO_DATE('" + objReport.sFromDate + "','yyyy/MM/dd')),'DD/MON/YYYY') AS FROM_DATE ,TO_CHAR( (TO_DATE('" + objReport.sTodate + "','yyyy/MM/dd')),'DD/MON/YYYY') AS TO_DATE,TO_CHAR(SYSDATE,'DD/MON/YYYY') as CURRENT_DATE   FROM ";
                strQry += " ( SELECT COUNT(DT_CODE) DT_CODE , OFF_CODE , FT_NAME ,DT_CRON   FROM (SELECT OFF_CODE ,FT_NAME  FROM VIEW_ALL_OFFICES ,TBLFDRTYPE WHERE  LENGTH(OFF_CODE) = 3 )A LEFT JOIN ";
                strQry += " (SELECT  SUBSTR(DT_OM_SLNO , 0,3 ) DT_OM_SLNO ,DT_FDRSLNO ,DT_CODE  , FT_NAME as FEEDERTYPE   , DT_CRON FROM TBLDTCMAST ,TBLFEEDERMAST  ,TBLFDRTYPE   WHERE   FD_FEEDER_TYPE =  FT_ID  AND ";
                strQry += "  DT_FDRSLNO = FD_FEEDER_CODE AND   ";
                if (objReport.sTodate == null && (objReport.sFromDate != null))
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' and TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD') ) B ";
                }
                if (objReport.sFromDate == null && (objReport.sTodate != null))
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "' ) B ";
                }
                if (objReport.sFromDate == null && objReport.sTodate == null)
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')<=TO_CHAR(SYSDATE,'YYYY/MM/DD')) B ";
                }
                if (objReport.sFromDate != null && objReport.sTodate != null)
                {
                    strQry += " TO_CHAR(DT_CRON,'YYYY/MM/DD')>= '" + objReport.sFromDate + "' AND TO_CHAR(DT_CRON,'YYYY/MM/DD')<='" + objReport.sTodate + "' ) B ";
                }
                strQry += " ON A.OFF_CODE = B.DT_OM_SLNO AND A.FT_NAME = B.FEEDERTYPE GROUP BY OFF_CODE , FT_NAME , DT_CRON ) ";

                dtFeederOrCategoryAbstractDetails = ObjCon.getDataTable(strQry);
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
            return dtFeederOrCategoryAbstractDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Agencyid"></param>
        /// <param name="sOfficeCode"></param>
        /// <param name="po_no"></param>
        /// <returns></returns>
        public DataTable PrintPurchaseorderoil(string Agencyid, string sOfficeCode, string po_no)
        {
            DataTable dtagencydetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                strQry = " select AGENCY_NAME ,AGENCY_ADDRESS,DIVISION,\"Available_Amount\",\"Debit_Amount\",A.BT_ACC_CODE,(select sum(BT_DEBIT_AMNT) from TBLBUDGETTRANS where  BT_ACC_CODE =A.BT_ACC_CODE) AS \"Utilized_Amount\",\"Budget_SanctionAmt\"  from ( ";
                strQry += "   select RA_NAME AS AGENCY_NAME , RA_ADDRESS AS AGENCY_ADDRESS ,DIV_NAME as DIVISION, BT_AVL_AMNT AS \"Available_Amount\" ";
                strQry += " ,BT_DEBIT_AMNT  AS \"Debit_Amount\", BM_AMOUNT as \"Budget_SanctionAmt\",BT_ACC_CODE  ";
                strQry += "  from TBLREPAIRERAGENCYMASTER, TBLDIVISION,TBLOILSENTMASTER,TBLBUDGETTRANS ,TBLBUDGETMAST  ";
                strQry += "  where  OSD_OFFICE_CODE=DIV_CODE and  RA_ID= OSD_AGENCY and OSD_BT_ID=BT_ID and BT_BM_ID=BM_ID and RA_ID ='" + Agencyid + "' and DIV_CODE='" + sOfficeCode + "' and OSD_PO_NO='" + po_no + "' )A ";
                dtagencydetails = ObjCon.getDataTable(strQry);
                return dtagencydetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(
                   ex.ToString(),
                   ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name
                   );
                return dtagencydetails;
            }
        }
    }
}

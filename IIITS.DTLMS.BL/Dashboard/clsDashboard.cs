using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.BL
{
    public class clsDashboard
    {
        string strFormCode = "clsDashboard";
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        OleDbCommand oleDbCommand;
        public string sOfficeCode { get; set; }
        public string sOfficeName { get; set; }
        public string sBOId { get; set; }
        public string sRoleId { get; set; }

        public string sCapacity { get; set; }

        #region "DTC Failure Pending"

        public string GetEstimationPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT COUNT(*) FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE :OfficeCode ||'%'  AND WO_BO_ID='9' ";
                strQry += " AND WO_APPROVE_STATUS='0'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetEstimationPendingCount");
                return ex.Message;
            }
        }

        /// <summary>
        /// Get count of pending Work Order, Indent,Invoice(it includes Failure Entry,Enhancement, New DTC counts)
        /// </summary>
        /// <param name="objDashoard"></param>
        /// <returns></returns>
        public DataTable GetFailurePendingCount(clsDashboard objDashoard)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT WFOTABLE+AUTOTABLE  AS FAILURE_PENDING FROM ";
                //strQry += " (SELECT COUNT(*) WFOTABLE  FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE '" + objDashoard.sOfficeCode + "%' ";
                //strQry += " AND WO_BO_ID IN (" + objDashoard.sBOId + ") AND WO_APPROVE_STATUS='0') A,";
                //strQry += " (SELECT COUNT(*) AUTOTABLE FROM TBLWO_OBJECT_AUTO,TBLBO_FLOW_MASTER WHERE WOA_REF_OFFCODE LIKE '" + objDashoard.sOfficeCode + "%' AND ";
                //strQry += " WOA_INITIAL_ACTION_ID IS NULL AND BFM_ID=WOA_BFM_ID AND BFM_NEXT_BO_ID IN (" + objDashoard.sBOId + ")) B ";

                //return ObjCon.get_value(strQry);


                strQry = "SELECT NVL(SUM(CASE WHEN  (FAILURE IS NOT NULL AND WORKORDER IS NULL) THEN 1 ELSE 0 END ),0)  AS \"WORKORDER\", ";
                strQry += " NVL(SUM(CASE WHEN (INDENT IS NULL AND WORKORDER IS NOT NULL) THEN 1 ELSE 0 END ),0) as \"INDENT\" ,";
                strQry += " NVL(SUM(CASE WHEN  (INVOICE IS NULL AND INDENT IS NOT NULL) THEN 1 ELSE 0 END),0) AS \"INVOICE\" ,";
                strQry += " NVL(SUM(CASE WHEN  (DECOMMISION IS NULL AND INVOICE IS NOT NULL) THEN 1 ELSE 0 END),0) AS \"DECOMMISION\",";
                strQry += " NVL(SUM(CASE WHEN  (DECOMMISION IS NOT NULL AND RIAPPROVE IS NULL) THEN 1 ELSE 0 END),0) AS \"RI\",";
                strQry += " NVL(SUM(CASE WHEN  (CRREPORT is  NULL AND RIAPPROVE IS NOT NULL) THEN 1 ELSE 0 END),0) AS \"CR\"";
                strQry += " FROM MV_VIEWPENDINGFAILURE , MV_WORKFLOWSTATUSDUMMY WHERE WO_DATA_ID = DT_CODE AND  OFFCODE LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFailurePendingCount");
                return dt;
            }
        }


        /// <summary>
        /// Get count of pending Work Order
        /// </summary>
        /// <param name="objDashoard"></param>
        /// <returns></returns>
        public string GetWOPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT COUNT(*) FROM MV_VIEWPENDINGFAILURE , MV_WORKFLOWSTATUSDUMMY WHERE WO_DATA_ID = DT_CODE  AND  WORKORDER IS NULL  AND FAILURE IS NOT NULL AND WO_BO_ID <>10  AND  OFFCODE LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetWOPendingCount");
                return ex.Message;
            }
        }

        /// <summary>
        /// Get count of pending Indent
        /// </summary>
        /// <param name="objDashoard"></param>
        /// <returns></returns>
        public string GetIndentPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT COUNT(*) FROM  MV_VIEWPENDINGFAILURE , MV_WORKFLOWSTATUSDUMMY WHERE WO_DATA_ID = DT_CODE AND WO_BO_ID <> 10 AND INDENT IS NULL AND WORKORDER IS NOT NULL  AND  OFFCODE LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);

                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetIndentPendingCount");
                return ex.Message;
            }
        }


        /// <summary>
        /// Get count of pending Invoice
        /// </summary>
        /// <param name="objDashoard"></param>
        /// <returns></returns>
        public string GetInvoicePendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT COUNT(*) FROM  MV_VIEWPENDINGFAILURE , MV_WORKFLOWSTATUSDUMMY WHERE WO_DATA_ID = DT_CODE AND WO_BO_ID <> 10 AND INVOICE IS NULL AND INDENT IS NOT NULL AND OFFCODE LIKE :OfficeCode ||'%'";
                //strQry = "SELECT COUNT(*) FROM WORKFLOWSTATUS WHERE (INVOICE IS NULL OR DECOMMISION IS NULL) AND INDENT IS NOT NULL AND OFFCODE LIKE '" + objDashoard.sOfficeCode + "%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetInvoicePendingCount");
                return ex.Message;
            }
        }

        public DataTable GetReleasedGood(string value)
        {

            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "select TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE, SM_NAME AS DIVISION ,(SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME FROM ";
                strQry += " TBLTCMASTER , TBLSTOREMAST  WHERE SM_CODE = TC_LOCATION_ID AND TC_STATUS=11 AND TC_LIVE_FLAG = 1 AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetReleasedGood");
                return dt;
            }

        }

        public string GetDecommissionPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT COUNT(*) FROM  MV_VIEWPENDINGFAILURE , MV_WORKFLOWSTATUSDUMMY WHERE WO_DATA_ID = DT_CODE AND WO_BO_ID <> 10 AND DECOMMISION IS NULL AND INVOICE IS NOT NULL AND OFFCODE LIKE :OfficeCode ||'%'";
                //strQry = "SELECT COUNT(*) FROM WORKFLOWSTATUS WHERE (INVOICE IS NULL OR DECOMMISION IS NULL) AND INDENT IS NOT NULL AND OFFCODE LIKE '" + objDashoard.sOfficeCode + "%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetInvoicePendingCount");
                return ex.Message;
            }
        }

        /// <summary>
        /// Get count of pending RI
        /// </summary>
        /// <param name="objDashoard"></param>
        /// <returns></returns>
        public string GetRIPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT COUNT(*) FROM  MV_VIEWPENDINGFAILURE , MV_WORKFLOWSTATUSDUMMY WHERE WO_DATA_ID = DT_CODE AND WO_BO_ID <> 10 AND DECOMMISION IS NOT NULL AND RIAPPROVE IS NULL AND OFFCODE LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                //strQry = "SELECT COUNT(*) FROM WORKFLOWSTATUS WHERE (CRREPORT IS NULL)  AND INVOICE IS NOT NULL  AND OFFCODE LIKE '" + objDashoard.sOfficeCode + "%'";
                //strQry = "SELECT COUNT(*) FROM MV_WORKFLOWSTATUSDUMMY WHERE (RIAPPROVE IS NULL) AND (INVOICE IS NOT NULL AND DECOMMISION IS NOT NULL) AND WO_BO_ID<>10 AND OFFCODE LIKE '" + objDashoard.sOfficeCode + "%'";

                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRIPendingCount");
                return ex.Message;
            }
        }

        public string GetCRPendingCount(clsDashboard objDashoard)
        {
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM  MV_VIEWPENDINGFAILURE , MV_WORKFLOWSTATUSDUMMY WHERE WO_DATA_ID = DT_CODE AND WO_BO_ID <> 10 AND CRREPORT is  NULL AND RIAPPROVE IS NOT NULL  AND OFFCODE LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);

                //strQry = "SELECT COUNT(*) FROM WORKFLOWSTATUS WHERE (CRREPORT IS NULL)  AND INVOICE IS NOT NULL  AND OFFCODE LIKE '" + objDashoard.sOfficeCode + "%'";
                //strQry = "SELECT COUNT(*) FROM MV_WORKFLOWSTATUSDUMMY WHERE (CRREPORT IS NULL) AND (RIAPPROVE IS NOT NULL AND DECOMMISION IS NOT NULL) AND WO_BO_ID<>10 AND OFFCODE LIKE '" + objDashoard.sOfficeCode + "%'";

                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCRPendingCount");
                return ex.Message;
            }
        }

        public string GetTotalPendingCount(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT COUNT(*) FROM WORKFLOWSTATUS WHERE CRREPORT IS NULL AND OFFCODE LIKE '" + objDashoard.sOfficeCode + "%'";
                strQry = "SELECT SUM(CASE WHEN CRREPORT IS NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN CRREPORT IS NULL AND INVOICE IS NOT NULL AND ";
                strQry += " DECOMMISION IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN CRREPORT IS NULL AND DECOMMISION IS NULL AND INVOICE IS NOT NULL ";
                strQry += " THEN 1 ELSE 0 END) FROM MV_WORKFLOWSTATUSDUMMY WHERE OFFCODE LIKE :OfficeCode ||'%' AND WO_BO_ID<> 10";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTotalPendingCount");
                return ex.Message;
            }
        }

        #endregion

        #region "Faulty DTR"

        public string GetFaultyTCField(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                #region
                //old query for getting count (included CR Count as well)
                //strQry = "SELECT SUM( tc_failedbutnotreturned + tc_failedbutnotmapped)A FROM  (SELECT md_name,off_name,off_code,nvl(tc_failedbutnotreturned,0) tc_failedbutnotreturned ";
                //strQry += " from (SELECT tc_capacity,substr  (tc_location_id,0,2)tc_location_id ,count(tc_code) as tc_failedbutnotreturned from TBLTCMASTER inner join  TBLDTCFAILURE  ";
                //strQry += "on TC_CODE=DF_EQUIPMENT_ID INNER JOIN TBLWORKORDER on df_id=wo_df_id INNER JOIN TBLINDENT on wo_slno=TI_WO_SLNO INNER JOIN  TBLDTCINVOICE on in_ti_no=ti_id ";
                //strQry += "LEFT JOIN TBLTCREPLACE on IN_NO= TR_IN_NO WHERE tr_ri_no is NULL or tr_rv_no is null and tc_status='3'  GROUP BY tc_capacity,substr(tc_location_id,0,2))a ";
                //strQry += "RIGHT JOIN (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE UPPER(MD_TYPE)='C' AND  LENGTH  (OFF_CODE)=2)b on MD_NAME=tc_capacity ";
                //strQry += "and tc_location_id=OFF_CODE)a INNER JOIN  (SELECT md_name,off_name,off_code,nvl(tc_failedbutnotmapped,0) tc_failedbutnotmapped from (SELECT tc_capacity,";
                //strQry += "substr   (tc_location_id,0,2)tc_location_id ,count(tc_code) as tc_failedbutnotmapped from TBLTCMASTER inner join  TBLDTCFAILURE  on   TC_CODE=DF_EQUIPMENT_ID ";
                //strQry += "LEFT JOIN TBLWORKORDER on df_id=wo_df_id LEFT JOIN TBLINDENT on wo_slno=TI_WO_SLNO  left JOIN   TBLDTCINVOICE on in_ti_no=ti_id  WHERE in_ti_no is NULL ";
                //strQry += "and tc_status='3' GROUP BY tc_capacity,substr(tc_location_id,0,2))a RIGHT JOIN (SELECT MD_NAME,OFF_NAME,OFF_CODE  FROM TBLMASTERDATA,VIEW_ALL_OFFICES WHERE ";
                //strQry += "UPPER(MD_TYPE)='C' AND  LENGTH(OFF_CODE)=2)b on MD_NAME=tc_capacity and tc_location_id=OFF_CODE)b on a.md_name=b.md_name and a.off_name=b.off_name and ";
                //strQry += "a.off_code=b.off_code  AND a.OFF_CODE like '" + objDashoard.sOfficeCode + "%'";
                #endregion

                #region
                //new query for getting count (excluded CR/RI Count )
                //strQry = "SELECT sum(nvl(TOTAL_PENDING,0)-nvl(CR_PENDING,0)) FROM (SELECT COUNT(*)AS TOTAL_PENDING , OFFCODE FROM MV_WORKFLOWSTATUSDUMMY ";
                //strQry += " WHERE CRREPORT IS NULL AND OFFCODE LIKE '" + objDashoard.sOfficeCode + "%' AND WO_BO_ID<>10 GROUP BY OFFCODE)A left JOIN (SELECT COUNT(*) AS CR_PENDING, ";
                //strQry += "OFFCODE FROM MV_WORKFLOWSTATUSDUMMY WHERE(CRREPORT IS NULL)  AND(INVOICE IS NOT NULL AND DECOMMISION IS NOT NULL AND RIAPPROVE IS NOT NULL) AND ";
                //strQry += "OFFCODE LIKE '" + objDashoard.sOfficeCode + "%' AND WO_BO_ID<>10 GROUP BY OFFCODE)B ON A.OFFCODE = B.OFFCODE";
                #endregion

                #region new query for getting count (Considering only Decomm Count)
                //strQry = "SELECT COUNT(*)AS DECOMM_PENDING  FROM MV_WORKFLOWSTATUSDUMMY WHERE (INVOICE IS NOT NULL) AND (CRREPORT IS NULL AND DECOMMISION IS ";
                //strQry += " NULL AND WO_BO_ID<>10) AND OFFCODE LIKE :OfficeCode ||'%'";
                #endregion
                strQry = "SELECT COUNT(*)AS DECOMM_PENDING  FROM (SELECT WO_DATA_ID AS DT_CODE  FROM MV_WORKFLOWSTATUSDUMMY WHERE(INVOICE IS NOT NULL)" +
                    "AND(CRREPORT IS NULL AND DECOMMISION IS  NULL AND WO_BO_ID <> 10) AND OFFCODE LIKE :OfficeCode1 ||'%' UNION ALL select DT_CODE " +
                    "from MV_WORKFLOWSTATUSDUMMY A  , TBLDTCFAILURE B, TBLTCMASTER C, MV_VIEWPENDINGFAILURE D WHERE A.WO_DATA_ID = D.DT_CODE AND " +
                    "D.DF_ID = B.DF_ID    AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.RIAPPROVE IS  NULL AND a.DECOMMISION IS  NOT NULL  and " +
                    "D.RI_STATUS LIKE '%STORE KEEPER%' AND A.WO_BO_ID <> 10 AND   A.OFFCODE LIKE :OfficeCode ||'%') A";

                oleDbCommand.Parameters.AddWithValue("OfficeCode1", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);

                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyTCField");
                return ex.Message;
            }
        }

        public string GetFaultyTCStore(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                // strQry = "SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='1' AND TC_STATUS='3' AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";

                strQry = "SELECT COUNT(*)  FROM(SELECT CAST(TC_CODE AS INT) AS DT_CODE FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION = '1' AND TC_STATUS = '3' " +
                    "AND TC_LOCATION_ID LIKE :OfficeCode1||'%'  AND TC_LIVE_FLAG = 1 UNION ALL  select CAST(DTRCODE AS INT) AS DT_CODE from MV_WORKFLOWSTATUSDUMMY A " +
                    ", TBLDTCFAILURE B, TBLTCMASTER C, MV_VIEWPENDINGFAILURE D WHERE A.WO_DATA_ID = D.DT_CODE AND D.DF_ID = B.DF_ID  " +
                    "AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.RIAPPROVE IS  NULL AND a.DECOMMISION IS  NOT NULL  and D.RI_STATUS LIKE '%STORE OFFICER%'" +
                    "AND A.WO_BO_ID <> 10 AND   A.OFFCODE LIKE :OfficeCode||'%')A";
                oleDbCommand.Parameters.AddWithValue("OfficeCode1", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);

                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyTCStore");
                return ex.Message;
            }
        }

        public string GetFaultyTCRepair(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT COUNT(*) FROM TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE TC_CODE=RSD_TC_CODE ";
                //strQry += " AND RSD_DELIVARY_DATE IS NULL AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%'";

                //strQry = "SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='3' AND TC_STATUS='3' AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                strQry = "  SELECT  count(*) FROM  ( SELECT  TC_CODE,CASE WHEN RSM_SUPREP_TYPE = 2 THEN  ";
                strQry += " (SELECT TR_NAME FROM TBLTRANSREPAIRER WHERE TR_ID = RSM_SUPREP_ID)    WHEN RSM_SUPREP_TYPE = 1  ";
                strQry += "  THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TS_ID = RSM_SUPREP_ID) END SUPPLIER, ";
                strQry += " TO_CHAR(TC_CAPACITY)TC_CAPACITY,(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(TC_LOCATION_ID,1,2)) DIVISION ";
                strQry += " ,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE,RSM_PO_NO ,TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY') ";
                strQry += "RSM_PO_DATE , (SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME ";
                strQry += " FROM TBLTCMASTER , TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER  WHERE RSM_ID = RSD_RSM_ID and RSD_TC_CODE = TC_CODE and RSM_SUPREP_TYPE = 2 ";
                strQry += " and  RSD_INV_DATE IS not null  and RSD_DELIVARY_DATE is NULL   AND TC_STATUS='3' AND TC_LOCATION_ID LIKE :OfficeCode ||'%' ORDER BY tc_Code  ) ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyTCRepair");
                return ex.Message;
            }
        }


        public string GetFaultyTCSupplier(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT COUNT(*) FROM TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE TC_CODE=RSD_TC_CODE ";
                //strQry += " AND RSD_DELIVARY_DATE IS NULL AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%'";

                //strQry = "SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='3' AND TC_STATUS='3' AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                strQry = "  SELECT  count(*) FROM  ( SELECT  TC_CODE,CASE WHEN RSM_SUPREP_TYPE = 2 THEN  ";
                strQry += " (SELECT TR_NAME FROM TBLTRANSREPAIRER WHERE TR_ID = RSM_SUPREP_ID)    WHEN RSM_SUPREP_TYPE = 1  ";
                strQry += "  THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TS_ID = RSM_SUPREP_ID) END SUPPLIER, ";
                strQry += " TO_CHAR(TC_CAPACITY)TC_CAPACITY,(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(TC_LOCATION_ID,1,2)) DIVISION ";
                strQry += " ,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE,RSM_PO_NO ,TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY') ";
                strQry += "RSM_PO_DATE , (SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME ";
                strQry += " FROM TBLTCMASTER , TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER  WHERE RSM_ID = RSD_RSM_ID and RSD_TC_CODE = TC_CODE and RSM_SUPREP_TYPE = 1 ";
                strQry += " and  RSD_INV_DATE IS not null  and RSD_DELIVARY_DATE is NULL   AND TC_STATUS='3' AND TC_LOCATION_ID LIKE :OfficeCode ||'%' ORDER BY tc_Code  ) ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyTCRepair");
                return ex.Message;
            }
        }
        public string GetTotalFaultyTC(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT FIELDCOUNT+STORECOUNT+REPAIRCOUNT FROM ";
                //strQry += " (SELECT COUNT(*) FIELDCOUNT FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='2' AND TC_STATUS='3' AND TC_CODE<>0  ";
                //strQry += " AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%') A,";
                //strQry += " (SELECT COUNT(*) STORECOUNT FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='1' AND TC_STATUS='3' AND TC_CODE<>0 ";
                //strQry += " AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%') B,";
                //strQry += " (SELECT COUNT(*) REPAIRCOUNT FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='3' AND TC_STATUS='3' AND TC_CODE<>0 ";
                //strQry += " AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%') C";

                strQry = "SELECT FIELDCOUNT+STORECOUNT+REPAIRCOUNT FROM ";
                strQry += " (SELECT COUNT(*)AS FIELDCOUNT  FROM MV_WORKFLOWSTATUSDUMMY WHERE (INVOICE IS NOT NULL) AND (CRREPORT IS NULL AND ";
                strQry += " DECOMMISION IS NULL AND WO_BO_ID<>10) AND OFFCODE LIKE :OfficeCode ||'%') A,";
                strQry += " (SELECT COUNT(*) STORECOUNT FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='1' AND TC_STATUS='3' AND TC_CODE<>0 ";
                strQry += " AND TC_LOCATION_ID LIKE :OfficeCodes ||'%') B,";
                strQry += " (SELECT COUNT(*) REPAIRCOUNT FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='3' AND TC_STATUS='3' AND TC_CODE<>0 ";
                strQry += " AND TC_LOCATION_ID LIKE :OfficeCodeid ||'%') C";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCodes", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCodeid", objDashoard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTotalFaultyTC");
                return ex.Message;
            }
        }

        #endregion

        #region Inbox status

        public string GetPendingWorkflow(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT WFOTABLE+AUTOTABLE AS TOTAL_PENDING FROM ";
                strQry += " (SELECT COUNT(*) WFOTABLE  FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE :OfficeCodes ||'%'  ";
                strQry += " AND WO_APPROVE_STATUS='0' AND WO_NEXT_ROLE=:RoleId) A, ";
                strQry += "  (SELECT COUNT(*) AUTOTABLE FROM TBLWO_OBJECT_AUTO WHERE WOA_REF_OFFCODE LIKE :OfficeCode ||'%' ";
                strQry += " AND WOA_INITIAL_ACTION_ID IS NULL AND WOA_ROLE_ID=:RoleIds ) B ";
                oleDbCommand.Parameters.AddWithValue("OfficeCodes", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("RoleId", objDashoard.sRoleId);
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);

                oleDbCommand.Parameters.AddWithValue("RoleIds", objDashoard.sRoleId);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetPendingWorkflow");
                return ex.Message;
            }
        }

        public string GetApprovedWorkflow(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = " SELECT APPROVED+APPROVED_AUTO FROM ";
                strQry += "(SELECT COUNT(*) APPROVED  FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE :OfficeCode ||'%'  ";
                strQry += " AND WO_APPROVE_STATUS IN ('1','2') AND WO_NEXT_ROLE=:RoleId) A,";
                strQry += " (SELECT COUNT(*) APPROVED_AUTO  FROM TBLWO_OBJECT_AUTO WHERE WOA_REF_OFFCODE LIKE :OfficeCodes ||'%'";
                strQry += " AND WOA_INITIAL_ACTION_ID IS NOT NULL AND WOA_ROLE_ID=:RoleIds) B";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);

                oleDbCommand.Parameters.AddWithValue("RoleId", objDashoard.sRoleId);
                oleDbCommand.Parameters.AddWithValue("OfficeCodes", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("RoleIds", objDashoard.sRoleId);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetApprovedWorkflow");
                return ex.Message;
            }
        }

        public string GetRejectedWorkflow(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT COUNT(*) REJECTED  FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE :OfficeCode ||'%'  ";
                strQry += " AND WO_APPROVE_STATUS='3' AND WO_NEXT_ROLE=:RoleId";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("RoleId", objDashoard.sRoleId);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRejectedWorkflow");
                return ex.Message;
            }
        }

        public string GetTotalWorkflow(clsDashboard objDashoard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = "SELECT TOTAL_PENDING+APPROVED+REJECTED FROM ";
                strQry += " (SELECT A.WFOTABLE+B.AUTOTABLE AS TOTAL_PENDING FROM ";
                strQry += " (SELECT COUNT(*) WFOTABLE  FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE :OfficeCode ||'%'  ";
                strQry += " AND WO_APPROVE_STATUS='0' AND WO_NEXT_ROLE=:RoleId) A,";
                strQry += " (SELECT COUNT(*) AUTOTABLE FROM TBLWO_OBJECT_AUTO WHERE WOA_REF_OFFCODE LIKE :OfficeCodes ||'%' ";
                strQry += " AND WOA_INITIAL_ACTION_ID IS NULL AND WOA_ROLE_ID=:RoleIds ) B ) X,";
                strQry += " (SELECT APPROVED+APPROVED_AUTO APPROVED FROM ";
                strQry += "(SELECT COUNT(*) APPROVED  FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE :RefOfficeCode ||'%'  ";
                strQry += " AND WO_APPROVE_STATUS IN ('1','2') AND WO_NEXT_ROLE=:RoleIdsa) A,";
                strQry += " (SELECT COUNT(*) APPROVED_AUTO  FROM TBLWO_OBJECT_AUTO WHERE WOA_REF_OFFCODE LIKE :RefOfficeCodes ||'%'";
                strQry += " AND WOA_INITIAL_ACTION_ID IS NOT NULL AND WOA_ROLE_ID=:RoleIdqa) B) Y,";
                strQry += " (SELECT COUNT(*) REJECTED  FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE :RefOfficeCodesa ||'%' ";
                strQry += " AND WO_APPROVE_STATUS='3' AND WO_NEXT_ROLE=:RoleIdsd) Z";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("RoleId", objDashoard.sRoleId);
                oleDbCommand.Parameters.AddWithValue("OfficeCodes", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("RoleIds", objDashoard.sRoleId);
                oleDbCommand.Parameters.AddWithValue("RefOfficeCode", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("RoleIdsa", objDashoard.sRoleId);
                oleDbCommand.Parameters.AddWithValue("RefOfficeCodes", objDashoard.sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("RoleIdqa", objDashoard.sRoleId);
                oleDbCommand.Parameters.AddWithValue("RefOfficeCodesa", objDashoard.sOfficeCode);




                oleDbCommand.Parameters.AddWithValue("RoleIdsd", objDashoard.sRoleId);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRejectedWorkflow");
                return ex.Message;
            }
        }

        #endregion

        public DataTable LoadBarGraph(string sOfficeCode)
        {
            string subQuery = string.Empty;
            DataTable dtBarGraph = new DataTable();
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;

                strQry = "SELECT A.PRESENTMONTH , A.PRESENTYEAR , A.PREVIOUSYEAR , A.PREVIOUSMONTH AS PREVIOUSMONTH ,nvl(PRESENTCOUNT,0) as  PRESENTCOUNT , nvl(PREVIOUSCOUNT,0) as  PREVIOUSCOUNT FROM (select to_char(add_months(trunc(sysdate-1, 'yyyy'), level - 1), 'MON') PRESENTMONTH ,";

                strQry += "  to_char(add_months(trunc(sysdate, 'yyyy'), level - 1), 'YYYY')  PRESENTYEAR , ";
                strQry += " 	 to_char(add_months(trunc(sysdate, 'yyyy'), level - 1), 'MON') PREVIOUSMONTH, ";
                strQry += "  TO_CHAR(SYSDATE,'YYYY')-1  PREVIOUSYEAR, ";
                strQry += " TO_CHAR(ADD_MONTHS(TRUNC(SYSDATE, 'YYYY'), LEVEL - 1), 'MM') MON ";
                strQry += " from dual ";
                strQry += " connect by level <= 12 ) A left join ";
                strQry += " (SELECT sum(PRESENTCOUNT) PRESENTCOUNT, PRESENTYEAR, PRESENTMONTH   FROM((SELECT PRESENTYEAR, PRESENTMONTH, count(*) as PRESENTCOUNT     FROM(SELECT  WO_DATA_ID as DF_DTC_CODE, to_char(TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss'), 'YYYY') AS PRESENTYEAR, to_char(TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss'), 'MON') AS PRESENTMONTH, MAX(WFO_ID) KEEP(DENSE_RANK LAST ORDER BY WO_ID)   FROM TBLWORKFLOWOBJECTS, TBLWFODATA  WHERE ";
                strQry += " to_char(TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss'), 'YYYY') = to_char(SYSDATE, 'YYYY') AND ";
                strQry += " WO_WFO_ID = wfo_id AND   WO_BO_ID IN(9)   AND WO_RECORD_ID < 0 AND WO_REF_OFFCODE LIKE :OfficeCode ||'%'  GROUP BY WO_DATA_ID, TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss')) GROUP BY  PRESENTMONTH, PRESENTYEAR) ";
                strQry += " union all ";
                strQry += " (SELECT TO_CHAR(DF_DATE, 'YYYY') AS PRESENTYEAR, TO_CHAR(DF_DATE, 'MON') AS PRESENTMONTH, COUNT(DF_DTC_CODE)AS PRESENTCOUNT FROM  TBLDTCFAILURE WHERE TO_CHAR(DF_DATE, 'YYYY') = TO_CHAR(SYSDATE, 'YYYY') AND DF_LOC_CODE LIKE :OfficeCode1 ||'%' AND DF_STATUS_FLAG IN(1, 4) GROUP BY  TO_CHAR(DF_DATE, 'MON'), TO_CHAR(DF_DATE, 'YYYY'))) GROUP BY PRESENTYEAR , PRESENTMONTH ) B ";
                strQry += "    on A.PRESENTMONTH = B.PRESENTMONTH AND a.PRESENTYEAR = B.PRESENTYEAR LEFT JOIN ";
                strQry += " (SELECT  sum(PREVIOUSCOUNT) PREVIOUSCOUNT, PREVIOUSYEAR, PREVIOUSMONTH  FROM(SELECT PREVIOUSYEAR, PREVIOUSMONTH, count(*) as PREVIOUSCOUNT  FROM(SELECT  WO_DATA_ID as DF_DTC_CODE, to_char(TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss'), 'YYYY') AS PREVIOUSYEAR, to_char(TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss'), 'MON') AS PREVIOUSMONTH, MAX(WFO_ID) KEEP(DENSE_RANK LAST ORDER BY WO_ID)   FROM TBLWORKFLOWOBJECTS, TBLWFODATA  WHERE ";
                strQry += " to_char(TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss'), 'YYYY') = to_char(SYSDATE , 'YYYY')-1  AND ";
                strQry += " WO_WFO_ID = wfo_id AND   WO_BO_ID IN(9)   AND WO_RECORD_ID < 0 AND WO_REF_OFFCODE LIKE :OfficeCodes ||'%'  GROUP BY WO_DATA_ID, TO_DATE(XMLTYPE(TBLWFODATA.WFO_DATA).EXTRACT('//DF_DATE/text()').getStringVal(), 'dd-MM-yyyy HH24:mi:ss')) GROUP BY PREVIOUSMONTH, PREVIOUSYEAR ";
                strQry += " union all ";
                strQry += "  (SELECT TO_CHAR(DF_DATE, 'YYYY') AS PREVIOUSYEAR, TO_CHAR(DF_DATE, 'MON') AS PREVIOUSMONTH, COUNT(DF_DTC_CODE)AS PREVIOUSCOUNT FROM  TBLDTCFAILURE WHERE TO_CHAR(DF_DATE, 'YYYY') = TO_CHAR(SYSDATE, 'YYYY')-1  AND DF_LOC_CODE LIKE :OfficCodeWorkFlow ||'%' AND DF_STATUS_FLAG IN(1, 4) GROUP BY  TO_CHAR(DF_DATE, 'MON'), TO_CHAR(DF_DATE, 'YYYY'))) GROUP BY  PREVIOUSYEAR, PREVIOUSMONTH ) C ";
                strQry += " ON A.PRESENTMONTH = C.PREVIOUSMONTH AND C.PREVIOUSYEAR = A.PREVIOUSYEAR  ORDER BY MON ";



                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCode1", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCodes", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficCodeWorkFlow", sOfficeCode);
                dtBarGraph = ObjCon.getDataTable(strQry, oleDbCommand);
                return dtBarGraph;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadBarGraph");
                return dtBarGraph;
            }
        }

        #region View Failure Pending Details

        public DataTable LoadFailurePendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT * FROM MV_VIEWPENDINGFAILURE WHERE OM_CODE LIKE '" + sOfficeCode + "%' AND DT_CODE NOT IN (SELECT WO_DATA_ID FROM MV_WORKFLOWSTATUSDUMMY ";
                //strQry += " WHERE (CRREPORT IS NULL)  AND(INVOICE IS NOT NULL AND DECOMMISION IS NOT NULL AND WO_BO_ID<>10) AND OFFCODE LIKE '" + sOfficeCode + "%')";
                // old query 
                //strQry = "SELECT * FROM MV_VIEWPENDINGFAILURE WHERE OM_CODE LIKE '" + sOfficeCode + "%' AND DT_CODE NOT IN (SELECT WO_DATA_ID FROM MV_WORKFLOWSTATUSDUMMY ";
                //strQry += " WHERE (INVOICE IS NOT NULL) AND(CRREPORT IS NULL  AND WO_BO_ID <> 10) AND OFFCODE LIKE '" + sOfficeCode + "%')";

                // new query 

                strQry = " SELECT DT_CODE , DT_NAME ,DF_ID ,  DF_DATE , EST_NO , EST_CRON ,  FL_STATUS , WO_NO , WO_NO_DECOM, WO_DATE ,  WO_STATUS ,   TI_INDENT_NO , TI_INDENT_DATE, INDT_STATUS , IN_INV_NO,  IN_DATE, ";
                strQry += " INV_STATUS , TR_RI_NO ,  TR_RI_DATE , RI_STATUS, CR_STATUS  , GUARANTY_TYPE ,WO_NO , WO_DATE ,  DIV , SUBDIVSION,OMSECTION ,to_number(TC_CAPACITY) AS FAIL_CAPACITY , ";
                strQry += " CASE WHEN DF_ENHANCE_CAPACITY is  null THEN  (SELECT to_number(TC_CAPACITY) FROM TBLTCMASTER WHERE TC_CODE = DTRCODE) ";
                strQry += " ELSE TO_NUMBER(DF_ENHANCE_CAPACITY) ";
                strQry += " END INV_CAPACITY ";
                strQry += " FROM (SELECT * FROM MV_VIEWPENDINGFAILURE , TBLTCMASTER  WHERE TC_CODE = DTRCODE AND OM_CODE LIKE :OfficeCode ||'%' AND DT_CODE NOT IN  ";
                strQry += " (SELECT WO_DATA_ID FROM MV_WORKFLOWSTATUSDUMMY   WHERE (INVOICE IS NOT NULL) AND(CRREPORT IS NULL  AND WO_BO_ID <> 10) AND  OFFCODE LIKE :OfficeCodes ||'%')) ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCodes", sOfficeCode);

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFailurePendingDetails");
                return dt;
            }
        }

        #endregion

        #region DTC Failure Abstract

        public DataTable LoadDTCFailureAbstract(clsDashboard objDashboard)
        {
            string strQry = string.Empty;
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {

                strQry = " SELECT To_char(TC_CAPACITY)TC_CAPACITY, DF_LOC_CODE, (SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+1) AS OFFICENAME FROM VIEW_ALL_OFFICES WHERE DF_LOC_CODE=OFF_CODE) AS SECTION, COUNT(*) AS FAILURECOUNTOFYEAR,";
                strQry += "  SUM(CASE WHEN TO_CHAR(DF_DATE,'MONTH') IN  (SELECT MONTH FROM ";
                strQry += "  (SELECT TO_CHAR(ADD_MONTHS(TRUNC(SYSDATE, 'YYYY'), LEVEL - 1), 'MONTH') MONTH,";
                strQry += "  TO_CHAR(ADD_MONTHS(TRUNC(SYSDATE, 'YYYY'), LEVEL - 1), 'Q') QUARTER";
                strQry += "  FROM DUAL CONNECT BY LEVEL <= 12 )WHERE QUARTER = (select  to_char(sysdate,'Q') currentQuarter from dual)) THEN 1 ELSE 0 END ) CURRENTQUARTER,";
                strQry += "  SUM(CASE WHEN TO_CHAR(DF_DATE,'MONTH') = TO_CHAR(SYSDATE,'MONTH') THEN 1 ELSE 0 END) CURRENTMONTH,SUM(CASE WHEN TO_CHAR(DF_DATE,'MMYYYY') = TO_CHAR(ADD_MONTHS(SYSDATE,-1),'MMYYYY') THEN 1 ELSE 0 END) PREVIOUSMONTH ";
                strQry += "  FROM TBLDTCFAILURE, TBLTCMASTER WHERE DF_EQUIPMENT_ID = TC_CODE AND TO_CHAR(DF_DATE,'YYYY') = TO_CHAR(SYSDATE,'YYYY') ";
                strQry += " AND DF_LOC_CODE LIKE :OfficeCode ||'%' AND DF_STATUS_FLAG IN (1,4) ";

                if (objDashboard.sCapacity != null)
                {
                    strQry += "AND TC_CAPACITY = :Capacity ";
                    oleDbCommand.Parameters.AddWithValue("Capacity", objDashboard.sCapacity);
                }
                strQry += "  GROUP BY TC_CAPACITY, DF_LOC_CODE  ORDER BY TC_CAPACITY  ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashboard.sOfficeCode);

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDTCFailureAbstract");
                return dt;
            }
        }
        #endregion

        public DataTable LoadFaultyDTRDetails(string sOfficeCode)
        {

            DataTable dtCompleteDetails = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT TC_CODE,TM_NAME,TC_SLNO,TC_CAPACITY,DT_CODE,DT_NAME,TR_RI_NO,TR_RI_DATE,SM_NAME,SUP_REPNAME,RSM_ISSUE_DATE,SUP_INSP_DATE,INSP_BY FROM";
                //strQry += "(SELECT DISTINCT TC_CODE,TC_LOCATION_ID,TM_NAME,TC_SLNO,To_char(TC_CAPACITY)TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES TM WHERE TC_MAKE_ID=TM_ID and tc_status=3)A,";
                //strQry += "(SELECT DT_CODE,DT_NAME,DT_TC_ID FROM TBLDTCMAST,TBLTCMASTER WHERE TC_CURRENT_LOCATION=2 and tc_code=DT_TC_ID)B,";
                //strQry += "(SELECT DISTINCT SM.SM_NAME,TR_RI_NO,FAIL_TC_CODE,to_char(TR_RI_DATE,'DD-MON-YYYY')TR_RI_DATE FROM VIEWFAILTCCODE FT,TBLTCMASTER,TBLSTOREMAST SM  WHERE TC_CURRENT_LOCATION<>2";
                //strQry += " AND TC_CODE=FAIL_TC_CODE AND TC_STORE_ID=SM_ID)C,";
                //strQry += "(SELECT DISTINCT (CASE WHEN RSM_SUPREP_TYPE='2' THEN (SELECT TR_NAME FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN";
                //strQry += " RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END )SUP_REPNAME,RSD_TC_CODE,to_char(RSM_ISSUE_DATE,'DD-MON-YYYY')RSM_ISSUE_DATE,";
                //strQry += " RSM_ID,RSD_RSM_ID FROM TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER,TBLTCMASTER WHERE TC_CURRENT_LOCATION=3 AND";
                //strQry += " RSM_ID=RSD_RSM_ID AND TC_CODE=RSD_TC_CODE)D,";
                //strQry += "(SELECT TO_CHAR(IND_INSP_DATE,'DD-MON-YYYY') AS SUP_INSP_DATE,US_FULL_NAME AS INSP_BY,RSD_TC_CODE FROM";
                //strQry += " TBLREPAIRSENTDETAILS,TBLINSPECTIONDETAILS,TBLUSER,TBLTCMASTER WHERE TC_CURRENT_LOCATION=3 AND IND_INSP_BY=US_ID AND";
                //strQry += " IND_RSD_ID=RSD_ID AND TC_CODE=RSD_TC_CODE)E";
                //strQry += " WHERE B.DT_TC_ID(+)=A.TC_CODE AND A.TC_CODE=C.FAIL_TC_CODE(+) AND";
                //strQry += " A.TC_CODE=D.RSD_TC_CODE(+) AND E.RSD_TC_CODE(+)=A.TC_CODE and TC_LOCATION_ID LIKE '" + sOfficeCode + "%' ORDER BY TC_CODE";
                #region before query optimization
                //strQry = "SELECT TC_CODE,TM_NAME,TC_SLNO,TC_CAPACITY,DT_CODE,DT_NAME,TR_RI_NO,TR_RI_DATE,SM_NAME,SUP_REPNAME,RSM_ISSUE_DATE,";
                //strQry += " SUP_INSP_DATE,INSP_BY FROM(SELECT DISTINCT TC_CODE,TC_LOCATION_ID,TM_NAME,TC_SLNO,To_char(TC_CAPACITY)TC_CAPACITY FROM ";
                //strQry += " TBLTCMASTER,TBLTRANSMAKES TM WHERE TC_MAKE_ID=TM_ID and tc_status=3 )A,(SELECT DT_CODE,DT_NAME,DT_TC_ID FROM TBLDTCMAST,";
                //strQry += " TBLTCMASTER WHERE TC_CURRENT_LOCATION=2 and tc_code=DT_TC_ID AND TC_CODE<>0)B,(SELECT SM_NAME,TR_RI_NO,FAIL_TC_CODE,";
                //strQry += " TR_RI_DATE,tr_id FROM (SELECT DISTINCT SM.SM_NAME as SM_NAME,TR_RI_NO,FAIL_TC_CODE,to_char(TR_RI_DATE,'DD-MON-YYYY')";
                //strQry += " TR_RI_DATE,tr_id FROM VIEWFAILTCCODE FT,TBLTCMASTER,TBLSTOREMAST SM  WHERE TC_CURRENT_LOCATION<>2 AND ";
                //strQry += " TC_CODE =FAIL_TC_CODE AND TC_STORE_ID=SM_ID )A INNER JOIN (SELECT  max(tr_id)TR_IDD,FAIL_TC_CODE as  FAIL_TC_CODE1 ";
                //strQry += " FROM VIEWFAILTCCODE FT,TBLTCMASTER,TBLSTOREMAST SM  WHERE TC_CURRENT_LOCATION<>2 AND TC_CODE=FAIL_TC_CODE AND ";
                //strQry += " TC_STORE_ID =SM_ID GROUP BY FAIL_TC_CODE )B ON TR_IDD=tr_id )C,(SELECT DISTINCT (CASE WHEN RSM_SUPREP_TYPE='2' THEN ";
                //strQry += " (SELECT TR_NAME FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID=RSM_SUPREP_ID) WHEN RSM_SUPREP_TYPE='1' THEN (SELECT TS_NAME ";
                //strQry += " FROM TBLTRANSSUPPLIER  WHERE TS_ID=RSM_SUPREP_ID) END )SUP_REPNAME,RSD_TC_CODE ,to_char(RSM_ISSUE_DATE,'DD-MON-YYYY')";
                //strQry += " RSM_ISSUE_DATE, RSM_ID,RSD_RSM_ID FROM TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER,TBLTCMASTER WHERE TC_CURRENT_LOCATION=3";
                //strQry += " AND RSM_ID=RSD_RSM_ID AND TC_CODE=RSD_TC_CODE AND RSD_ID IN (SELECT RSD_ID FROM (SELECT DISTINCT RSD_TC_CODE,";
                //strQry += " max(RSD_ID)RSD_ID FROM TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER,TBLTCMASTER WHERE TC_CURRENT_LOCATION=3 AND";
                //strQry += " RSM_ID =RSD_RSM_ID AND TC_CODE=RSD_TC_CODE GROUP BY RSD_TC_CODE)))D,(SELECT TO_CHAR(IND_INSP_DATE,'DD-MON-YYYY')";
                //strQry += " AS SUP_INSP_DATE,US_FULL_NAME AS INSP_BY,RSD_TC_CODE FROM TBLREPAIRSENTDETAILS,TBLINSPECTIONDETAILS,TBLUSER,";
                //strQry += " TBLTCMASTER WHERE TC_CURRENT_LOCATION=3 AND IND_INSP_BY=US_ID AND IND_RSD_ID=RSD_ID AND TC_CODE=RSD_TC_CODE)E ";
                //strQry += " WHERE B.DT_TC_ID(+)=A.TC_CODE AND A.TC_CODE=C.FAIL_TC_CODE(+) AND A.TC_CODE=D.RSD_TC_CODE(+) AND";
                //strQry += " E.RSD_TC_CODE(+)=A.TC_CODE and TC_LOCATION_ID LIKE :OfficeCode ||'%' ORDER BY TC_CODE";
                #endregion

                strQry = " SELECT TC_CODE, TM_NAME, TC_SLNO, TC_CAPACITY, DT_CODE, DT_NAME, TR_RI_NO, TR_RI_DATE, SM_NAME, SUP_REPNAME, ";
                strQry += " RSM_ISSUE_DATE, SUP_INSP_DATE, INSP_BY From(SELECT DISTINCT TC_CODE, TC_LOCATION_ID, TM_NAME, TC_SLNO, ";
                strQry += " To_char(TC_CAPACITY)TC_CAPACITY FROM  TBLTCMASTER inner join TBLTRANSMAKES TM on TC_MAKE_ID = TM_ID ";
                strQry += " where tc_status = 3)A,(SELECT DT_CODE, DT_NAME, DT_TC_ID FROM TBLDTCMAST inner join TBLTCMASTER on TC_CODE= DT_TC_ID ";
                strQry += " WHERE TC_CURRENT_LOCATION = 2  AND TC_CODE<> 0)B,(SELECT SM_NAME, TR_RI_NO, FAIL_TC_CODE, TR_RI_DATE, TR_ID ";
                strQry += " FROM (SELECT DISTINCT VIEWFAILTCCODE.SM_NAME,VIEWFAILTCCODE.TR_RI_NO,FAIL_TC_CODE, ";
                strQry += " to_char(TBLTCREPLACE.TR_RI_DATE, 'DD-MON-YYYY') as TR_RI_DATE,TBLTCREPLACE.TR_ID ";
                strQry += " From VIEWFAILTCCODE inner join TBLTCMASTER on TC_CODE = FAIL_TC_CODE  ";
                strQry += " inner join TBLTCREPLACE on VIEWFAILTCCODE.TR_RI_NO = TBLTCREPLACE.TR_RI_NO WHERE TC_CURRENT_LOCATION<> 2)A ";
                strQry += " inner join(SELECT  MAX(TR_ID)TR_IDD, FAIL_TC_CODE as FAIL_TC_CODE1 From TBLTCREPLACE ";
                strQry += " inner join VIEWFAILTCCODE on TBLTCREPLACE.TR_RI_NO = VIEWFAILTCCODE.TR_RI_NO ";
                strQry += " inner join TBLTCMASTER on TC_CODE = FAIL_TC_CODE WHERE TC_CURRENT_LOCATION <> 2 GROUP BY FAIL_TC_CODE)B ";
                strQry += " ON TR_ID = TR_IDD )C,(SELECT DISTINCT(CASE WHEN RSM_SUPREP_TYPE = '2' THEN(SELECT TR_NAME ";
                strQry += " FROM TBLTRANSREPAIRER TR WHERE TR.TR_ID = RSM_SUPREP_ID) WHEN RSM_SUPREP_TYPE = '1' THEN(SELECT TS_NAME ";
                strQry += " FROM TBLTRANSSUPPLIER  WHERE TS_ID = RSM_SUPREP_ID) END)SUP_REPNAME,RSD_TC_CODE , ";
                strQry += " to_char(RSM_ISSUE_DATE, 'DD-MON-YYYY') RSM_ISSUE_DATE, RSM_ID,RSD_RSM_ID FROM TBLREPAIRSENTDETAILS ";
                strQry += " inner join TBLREPAIRSENTMASTER on RSD_RSM_ID = RSM_ID inner join TBLTCMASTER on RSD_TC_CODE = TC_CODE ";
                strQry += " WHERE TC_CURRENT_LOCATION = 3 AND RSD_ID IN(SELECT RSD_ID FROM(SELECT DISTINCT RSD_TC_CODE, max(RSD_ID)RSD_ID ";
                strQry += " from TBLREPAIRSENTDETAILS inner join TBLREPAIRSENTMASTER on RSD_RSM_ID = RSM_ID inner join TBLTCMASTER on RSD_TC_CODE = TC_CODE ";
                strQry += " WHERE TC_CURRENT_LOCATION = 3 GROUP BY RSD_TC_CODE)) )D,(Select TO_CHAR(IND_INSP_DATE, 'DD-MON-YYYY') AS SUP_INSP_DATE, ";
                strQry += " US_FULL_NAME AS INSP_BY, RSD_TC_CODE from TBLREPAIRSENTDETAILS inner join TBLTCMASTER on RSD_TC_CODE = TC_CODE ";
                strQry += " inner join TBLINSPECTIONDETAILS on RSD_ID = IND_RSD_ID inner join TBLUSER on IND_INSP_BY = US_ID WHERE TC_CURRENT_LOCATION = 3)E ";
                strQry += " WHERE B.DT_TC_ID(+) = A.TC_CODE AND A.TC_CODE = C.FAIL_TC_CODE(+) AND A.TC_CODE = D.RSD_TC_CODE(+) ";
                strQry += " AND E.RSD_TC_CODE(+) = A.TC_CODE and TC_LOCATION_ID LIKE: OfficeCode || '%' ORDER BY TC_CODE fetch first 100 rows only ";


                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dtCompleteDetails = ObjCon.getDataTable(strQry, oleDbCommand);
                return dtCompleteDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFaultyDTRDetails");
                return dtCompleteDetails;
            }
        }


        #region DTC Count

        public string GetTotalDTCCount(clsDashboard objDashoard)
        {
            try
            {
                oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT COUNT(*) FROM TBLDTCMAST WHERE DT_OM_SLNO LIKE :OfficeCode ||'%'  ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashoard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTotalDTCCount");
                return ex.Message;
            }
        }

        #endregion

        #region Approval Pending

        public DataTable LoadApprovalPendingDetails(string sOfficeCode, string sBOType)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry += " SELECT * FROM  (SELECT DT_CODE,DT_NAME,OMSECTION,CASE ";
                strQry += " WHEN FL_STATUS IS NOT NULL THEN 'FAILURE/ENHANCEMENT : ' || FL_STATUS ";
                strQry += " WHEN WO_STATUS IS NOT NULL THEN 'WORK ORDER : ' || WO_STATUS    ";
                strQry += " WHEN INDT_STATUS IS NOT NULL THEN 'INDENT : ' || INDT_STATUS ";
                strQry += " WHEN INV_STATUS IS NOT NULL THEN 'INVOICE : ' || INV_STATUS ";
                strQry += " WHEN DECOMM_STATUS IS NOT NULL THEN 'DECOMMISSION : ' || DECOMM_STATUS ";
                strQry += " WHEN RI_STATUS IS NOT NULL THEN 'RI APPROVE : ' || RI_STATUS ";
                strQry += " WHEN CR_STATUS IS NOT NULL THEN 'CR REPORT : ' || CR_STATUS ";
                strQry += " ELSE '' END STATUS FROM VIEWPENDINGAPPROVAL ";
                strQry += "  WHERE OM_CODE LIKE :OfficeCode ||'%') ";

                //FAILURE ENTRY
                if (sBOType == "9")
                {
                    strQry += " WHERE STATUS LIKE 'FAILURE%'";
                }

                //ENHANCEMENT ENTRY
                if (sBOType == "10")
                {
                    strQry += " WHERE STATUS LIKE 'FAILURE%' ";
                }

                //WORK ORDER
                if (sBOType == "11")
                {
                    strQry += " WHERE STATUS LIKE 'WORK ORDER%' ";
                }

                //INDENT
                if (sBOType == "12")
                {
                    strQry += " WHERE STATUS LIKE 'INDENT%'  ";
                }

                //INVOICE
                if (sBOType == "13")
                {
                    strQry += " WHERE STATUS LIKE 'INVOICE%' ";
                }

                //DECOMMISSION
                if (sBOType == "14")
                {
                    strQry += " WHERE STATUS LIKE 'DECOMMISSION%' ";
                }

                //RI
                if (sBOType == "15")
                {
                    strQry += " WHERE STATUS LIKE 'RI APPROVE%'  ";
                }

                //CR REPORT
                if (sBOType == "26")
                {
                    strQry += " WHERE STATUS LIKE 'CR%' ";
                }
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFailurePendingDetails");
                return dt;
            }
        }
        #endregion


        public DataTable LoadEstimationPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT DT_CODE,DT_NAME,SUBDIVSION,OMSECTION,EST_CRON,FL_STATUS FROM MV_VIEWPENDINGFAILURE,TBLWORKFLOWOBJECTS WHERE OM_CODE=WO_REF_OFFCODE AND WO_REF_OFFCODE LIKE '" + sOfficeCode + "%' AND WO_BO_ID='9' AND WO_DATA_ID=DT_CODE AND WO_APPROVE_STATUS='0'";

                strQry = "SELECT DT_CODE,DT_NAME,DIV,SUBDIVSION,OMSECTION,EST_CRON,FL_STATUS,DTRCODE,GUARANTY_TYPE,(SELECT TC_CAPACITY  FROM TBLTCMASTER WHERE DTRCODE = TC_CODE )FAILURECAPACITY , ";
                strQry += "  CASE FAILURETYPE  WHEN  '1' THEN 'FAILURE' WHEN '4' THEN  'FAILURE AND ENHANCEMENT' END  as FAILURETYPE ,";
                strQry += " CASE  WHEN COMMISSIONINGCAPACITY IS NULL THEN (SELECT TO_NUMBER(TC_CAPACITY)  FROM TBLTCMASTER WHERE DTRCODE = TC_CODE )  ";
                strQry += " ELSE TO_NUMBER(COMMISSIONINGCAPACITY) END AS INVOICECAPACITY  FROM ";
                strQry += " (SELECT DT_CODE,DT_NAME,DIV,SUBDIVSION,OMSECTION,EST_CRON,FL_STATUS , ";
                strQry += " (SELECT sys.xmltype(WFO_DATA).extract('TBLDTCFAILURE/TBLDTCFAILURE/DF_EQUIPMENT_ID/text()').getStringVal()   FROM TBLWFODATA WHERE WFO_ID = WO_WFO_ID ) AS DTRCODE , ";
                strQry += " (SELECT  sys.xmltype(WFO_DATA).extract('TBLDTCFAILURE/TBLDTCFAILURE/GUARENTEE/text()').getStringVal()  FROM TBLWFODATA WHERE WFO_ID = WO_WFO_ID) AS GUARANTY_TYPE , ";
                strQry += " (SELECT  sys.xmltype(WFO_DATA).extract('TBLDTCFAILURE/TBLDTCFAILURE/DF_STATUS_FLAG/text()').getStringVal()  FROM TBLWFODATA WHERE WFO_ID = WO_WFO_ID) AS  FAILURETYPE , ";
                strQry += " (SELECT SYS.XMLTYPE(WFO_DATA).EXTRACT('TBLDTCFAILURE/TBLDTCFAILURE/DF_ENHANCE_CAPACITY/text()').getStringVal() FROM TBLWFODATA WHERE WFO_ID = WO_WFO_ID) AS ";
                strQry += "  COMMISSIONINGCAPACITY ";
                strQry += " FROM MV_VIEWPENDINGFAILURE,TBLWORKFLOWOBJECTS WHERE OM_CODE=WO_REF_OFFCODE AND WO_REF_OFFCODE LIKE :OfficeCode ||'%' AND WO_BO_ID='9' AND WO_DATA_ID=DT_CODE ";
                strQry += " AND WO_APPROVE_STATUS='0') ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadEstimationPendingDetails");
                return dt;
            }
        }

        public DataTable LoadWorkorderPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT DT_CODE,DT_NAME,SUBDIVSION,OMSECTION,WO_DATE,WO_STATUS,WO_NO,WO_NEW_CAP FROM MV_WORKFLOWSTATUSDUMMY,MV_VIEWPENDINGFAILURE WHERE WORKORDER IS NULL AND FAILURE IS NOT NULL AND OFFCODE=OM_CODE AND WO_BO_ID<>10 AND OFFCODE LIKE '" + sOfficeCode + "%' AND DT_CODE=WO_DATA_ID";
                strQry = " SELECT D.DT_CODE , D.DT_NAME ,D.DIV, D.SUBDIVSION , D.OMSECTION ,D.WO_DATE , D.WO_NO, D.GUARANTY_TYPE,";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN 'FAILURE' WHEN 4 THEN 'FAILURE AND ENHANCEMENT' END  FAILURETYPE ,to_number(C.TC_CAPACITY) AS DECOMCAPACITY , ";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN  C.TC_CAPACITY WHEN 4 THEN B.DF_ENHANCE_CAPACITY END COMMCAPACITY ,d.WO_STATUS ";
                strQry += " FROM  MV_WORKFLOWSTATUSDUMMY A  , TBLDTCFAILURE B , TBLTCMASTER C , MV_VIEWPENDINGFAILURE D ";
                strQry += " WHERE A.WO_DATA_ID = D.DT_CODE AND D.DF_ID = B.DF_ID  AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.WORKORDER IS NULL AND a.FAILURE IS  NOT NULL ";
                strQry += " AND A.WO_BO_ID  <>10 AND A.OFFCODE LIKE :OfficeCode ||'%' ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadWorkorderPendingDetails");
                return dt;
            }
        }

        public DataTable LoadIndentPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT DT_CODE,DT_NAME,WO_NO,WO_DATE,WO_NEW_CAP,SUBDIVSION,OMSECTION,TI_INDENT_DATE,INDT_STATUS FROM MV_WORKFLOWSTATUSDUMMY,MV_VIEWPENDINGFAILURE WHERE INDENT IS NULL AND WORKORDER IS NOT NULL AND OFFCODE=OM_CODE AND WO_BO_ID<>10 AND OFFCODE LIKE '" + sOfficeCode + "%' AND DT_CODE=WO_DATA_ID";
                strQry = " SELECT D.DT_CODE , D.DT_NAME ,D.DIV, D.SUBDIVSION , D.OMSECTION ,D.WO_DATE ,D.GUARANTY_TYPE , D.WO_NO,D.WO_NO_DECOM ,D.TI_INDENT_DATE, D.INDT_STATUS ,";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN 'FAILURE' WHEN 4 THEN 'FAILURE AND ENHANCEMENT' END  FAILURETYPE ,to_number(C.TC_CAPACITY) AS DECOMCAPACITY ,";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN  C.TC_CAPACITY WHEN 4 THEN B.DF_ENHANCE_CAPACITY END COMMCAPACITY ,D.INDT_STATUS  ";
                strQry += "  FROM  MV_WORKFLOWSTATUSDUMMY A  , TBLDTCFAILURE B , TBLTCMASTER C , MV_VIEWPENDINGFAILURE D WHERE A.WO_DATA_ID = D.DT_CODE AND D.DF_ID = B.DF_ID ";
                strQry += "  AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.INDENT IS  NULL AND a.WORKORDER IS  NOT NULL AND A.WO_BO_ID  <>10 ";
                strQry += "  AND A.OFFCODE LIKE :OfficeCode ||'%' ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadWorkorderPendingDetails");
                return dt;
            }
        }

        public DataTable LoadInvoicePendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT DT_CODE,DT_NAME,WO_NO,WO_DATE,WO_NEW_CAP,SUBDIVSION,OMSECTION,IN_DATE,INV_STATUS FROM MV_WORKFLOWSTATUSDUMMY,MV_VIEWPENDINGFAILURE WHERE OFFCODE=OM_CODE AND INVOICE IS NULL AND INDENT IS NOT NULL AND WO_BO_ID<>10 AND OFFCODE LIKE '" + sOfficeCode + "%' AND DT_CODE=WO_DATA_ID";
                strQry = " SELECT D.DT_CODE , D.DT_NAME ,D.DIV, D.SUBDIVSION , D.OMSECTION ,D.GUARANTY_TYPE,D.WO_DATE , D.WO_NO,D.WO_NO_DECOM ,D.IN_DATE,";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN 'FAILURE' WHEN 4 THEN 'FAILURE AND ENHANCEMENT' END  FAILURETYPE , to_number(C.TC_CAPACITY) AS DECOMCAPACITY , ";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN  C.TC_CAPACITY WHEN 4 THEN B.DF_ENHANCE_CAPACITY END COMMCAPACITY ,D.INV_STATUS ";
                strQry += "  FROM  MV_WORKFLOWSTATUSDUMMY A  , TBLDTCFAILURE B , TBLTCMASTER C , MV_VIEWPENDINGFAILURE D WHERE A.WO_DATA_ID = D.DT_CODE ";
                strQry += " AND D.DF_ID = B.DF_ID  AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.INVOICE IS  NULL AND a.INDENT IS  NOT NULL ";
                // strQry += " AND A.WO_BO_ID  <>10 ";
                strQry += " AND A.OFFCODE LIKE :OfficeCode ||'%'  ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);

                //strQry = "SELECT DT_CODE,DT_NAME,SUBDIVSION,OMSECTION,IN_DATE,INV_STATUS FROM WORKFLOWSTATUS,MV_VIEWPENDINGFAILURE WHERE OFFCODE=OM_CODE AND (INVOICE IS NULL OR DECOMMISION IS NULL) AND INDENT IS NOT NULL AND OFFCODE LIKE '" + sOfficeCode + "%' AND DT_CODE=WO_DATA_ID";
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadWorkorderPendingDetails");
                return dt;
            }
        }

        public DataTable LoadDeCommissionPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = " SELECT D.DT_CODE , D.DT_NAME,B.DF_EQUIPMENT_ID as DTR_CODE ,D.DIV, D.SUBDIVSION , D.OMSECTION ,D.WO_DATE ,D.GUARANTY_TYPE ,D.WO_NO ,d.WO_NO_DECOM  , ";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN 'FAILURE' WHEN 4 THEN 'FAILURE AND ENHANCEMENT' END  FAILURETYPE , to_number(C.TC_CAPACITY) AS DECOMCAPACITY , ";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN  C.TC_CAPACITY WHEN 4 THEN B.DF_ENHANCE_CAPACITY END COMMCAPACITY ,D.DECOMM_STATUS ";
                strQry += "  FROM  MV_WORKFLOWSTATUSDUMMY A  , TBLDTCFAILURE B , TBLTCMASTER C , MV_VIEWPENDINGFAILURE D WHERE A.WO_DATA_ID = D.DT_CODE AND D.DF_ID = B.DF_ID ";
                strQry += "   AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.DECOMMISION IS  NULL AND a.INVOICE IS  NOT NULL AND A.WO_BO_ID  <>10 AND ";
                strQry += " A.OFFCODE LIKE :OfficeCode ||'%'  ";

                //strQry = "SELECT DT_CODE,DT_NAME,WO_NO_DECOM,WO_DATE_DECOM,WO_DTC_CAP,SUBDIVSION,OMSECTION,DECOMM_STATUS FROM MV_WORKFLOWSTATUSDUMMY,MV_VIEWPENDINGFAILURE WHERE OFFCODE=OM_CODE AND INVOICE IS NOT NULL AND DECOMMISION IS NULL AND WO_BO_ID<>10 AND OFFCODE LIKE '" + sOfficeCode + "%' AND DT_CODE=WO_DATA_ID";

                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadWorkorderPendingDetails");
                return dt;
            }
        }

        public DataTable LoadRIPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT DT_CODE,DT_NAME,SUBDIVSION,OMSECTION,TR_RI_DATE,RI_STATUS,CR_STATUS FROM WORKFLOWSTATUS,MV_VIEWPENDINGFAILURE WHERE OFFCODE=OM_CODE AND (RIAPPROVE IS NULL OR CRREPORT IS NULL )  AND INVOICE IS NOT NULL AND OFFCODE LIKE '" + sOfficeCode + "%' AND DT_CODE=WO_DATA_ID";

                //strQry = "SELECT DT_CODE,DT_NAME,WO_NO_DECOM,WO_DATE_DECOM,WO_DTC_CAP,SUBDIVSION,OMSECTION,TR_RI_DATE,RI_STATUS,CR_STATUS FROM MV_WORKFLOWSTATUSDUMMY,MV_VIEWPENDINGFAILURE WHERE OFFCODE=OM_CODE AND ";
                //strQry += "(RIAPPROVE IS NULL)  AND (INVOICE IS NOT NULL AND DECOMMISION IS NOT NULL) AND WO_BO_ID<>10 AND OFFCODE LIKE '" + sOfficeCode + "%' AND DT_CODE=WO_DATA_ID ";

                strQry = " SELECT D.DT_CODE , D.DT_NAME,B.DF_EQUIPMENT_ID as DTR_CODE ,D.DIV, D.SUBDIVSION , D.OMSECTION ,D.WO_DATE , D.WO_NO ,D.GUARANTY_TYPE,d.WO_NO_DECOM , D.TR_RI_DATE, ";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN 'FAILURE' WHEN 4 THEN 'FAILURE AND ENHANCEMENT' END  FAILURETYPE , to_number(C.TC_CAPACITY) AS DECOMCAPACITY , ";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN  C.TC_CAPACITY WHEN 4 THEN B.DF_ENHANCE_CAPACITY END COMMCAPACITY ,D.CR_STATUS ,D.RI_STATUS ";
                strQry += "  FROM  MV_WORKFLOWSTATUSDUMMY A  , TBLDTCFAILURE B , TBLTCMASTER C , MV_VIEWPENDINGFAILURE D WHERE A.WO_DATA_ID = D.DT_CODE AND D.DF_ID = B.DF_ID ";
                strQry += "   AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.RIAPPROVE IS  NULL AND a.DECOMMISION IS  NOT NULL AND A.WO_BO_ID  <>10 AND ";
                strQry += "  A.OFFCODE LIKE :OfficeCode ||'%' ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadWorkorderPendingDetails");
                return dt;
            }
        }

        public DataTable LoadCRPendingDetails(string sOfficeCode)
        {

            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT DT_CODE,DT_NAME,SUBDIVSION,OMSECTION,TR_RI_DATE,RI_STATUS,CR_STATUS FROM WORKFLOWSTATUS,MV_VIEWPENDINGFAILURE WHERE OFFCODE=OM_CODE AND (RIAPPROVE IS NULL OR CRREPORT IS NULL )  AND INVOICE IS NOT NULL AND OFFCODE LIKE '" + sOfficeCode + "%' AND DT_CODE=WO_DATA_ID";
                //strQry = "SELECT DT_CODE,DT_NAME,WO_NO_DECOM,WO_DATE_DECOM,WO_DTC_CAP,SUBDIVSION,OMSECTION,TR_RI_DATE,RI_STATUS,";
                //strQry += " CR_STATUS FROM MV_WORKFLOWSTATUSDUMMY,MV_VIEWPENDINGFAILURE WHERE OFFCODE=OM_CODE AND (CRREPORT IS NULL) AND";
                //strQry += " (RIAPPROVE IS NOT NULL AND DECOMMISION IS NOT NULL)  AND WO_BO_ID<>10 AND OFFCODE LIKE '" + sOfficeCode + "%' AND DT_CODE=WO_DATA_ID";

                strQry = " SELECT D.DT_CODE , D.DT_NAME,B.DF_EQUIPMENT_ID as DTR_CODE ,D.DIV, D.SUBDIVSION , D.OMSECTION,D.GUARANTY_TYPE ,D.WO_DATE , D.WO_NO ,d.WO_NO_DECOM ,D.TR_RI_DATE ,";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN 'FAILURE' WHEN 4 THEN 'FAILURE AND ENHANCEMENT' END  FAILURETYPE ,to_number(C.TC_CAPACITY) AS DECOMCAPACITY , ";
                strQry += " CASE B.DF_STATUS_FLAG WHEN 1 THEN  C.TC_CAPACITY WHEN 4 THEN B.DF_ENHANCE_CAPACITY END COMMCAPACITY ,D.CR_STATUS ,D.RI_STATUS";
                strQry += " FROM  MV_WORKFLOWSTATUSDUMMY A  , TBLDTCFAILURE B , TBLTCMASTER C , MV_VIEWPENDINGFAILURE D WHERE A.WO_DATA_ID = D.DT_CODE AND D.DF_ID = B.DF_ID ";
                strQry += "   AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.CRREPORT IS  NULL AND a.RIAPPROVE IS  NOT NULL AND ";
                strQry += "  A.WO_BO_ID  <>10 AND A.OFFCODE LIKE :OfficeCode ||'%'  ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCRPendingDetails");
                return dt;
            }
        }


        public DataTable LoadFailureDtrDetails(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                strQry = " SELECT TO_CHAR(TC_CODE)TC_CODE,TC_SLNO,TO_CHAR(TC_CAPACITY)TC_CAPACITY,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE=TC_LOCATION_ID)OFFNAME  FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='2' AND TC_STATUS='3'  AND TC_LOCATION_ID LIKE :OfficeCode ||'%'  ";
                strQry += " UNION SELECT TO_CHAR(TC_CODE)TC_CODE,TC_SLNO,TC_CODE(TC_CAPACITY)TC_CAPACITY,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE=TC_LOCATION_ID)OFFNAME  FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='1' AND TC_STATUS='3'  AND TC_LOCATION_ID LIKE :OfficeCodes ||'%'  ";
                strQry += " UNION SELECT TO_CHAR(TC_CODE,TC_SLNO,TC_CAPACITY,(SELECT SUBSTR(OFF_NAME,INSTR(OFF_NAME,':')+1) FROM VIEW_ALL_OFFICES WHERE OFF_CODE=TC_LOCATION_ID)OFFNAME   FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='3' AND TC_STATUS='3'  AND TC_LOCATION_ID LIKE :OfficeCodeid ||'%'  ORDER BY TC_CAPACITY";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCodes", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCodeid", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFailureDtrDetails");
                return dt;
            }
        }

        public string TotalRepairGoodTc(clsDashboard objDashboard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT COUNT(*) FROM TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE TC_CODE=RSD_TC_CODE ";
                //strQry += " AND RSD_DELIVARY_DATE IS NULL AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%'";

                strQry = "SELECT COUNT(*) from TBLTCMASTER WHERE TC_STATUS=2 AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashboard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyTCRepair");
                return ex.Message;
            }
        }

        public string BrandNewDTrCount(clsDashboard objDashboard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT COUNT(*) FROM TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE TC_CODE=RSD_TC_CODE ";
                //strQry += " AND RSD_DELIVARY_DATE IS NULL AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%'";

                strQry = "SELECT COUNT(*) from TBLTCMASTER WHERE TC_STATUS= 1 AND TC_LIVE_FLAG = 1 AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashboard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "BrandNewDTrCount");
                return ex.Message;
            }
        }

        public string ScrapDtrCount(clsDashboard objDashboard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT COUNT(*) FROM TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE TC_CODE=RSD_TC_CODE ";
                //strQry += " AND RSD_DELIVARY_DATE IS NULL AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%'";

                strQry = "SELECT COUNT(*) from TBLTCMASTER WHERE TC_STATUS= 4  AND TC_CURRENT_LOCATION=1  AND TC_LIVE_FLAG = 1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashboard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ScrapDtrCount");
                return ex.Message;
            }
        }

        public string NonrepairableCount(clsDashboard objDashboard)
        {
            try
            {
                string strQry = string.Empty;
                oleDbCommand = new OleDbCommand();
                //strQry = "SELECT COUNT(*) FROM TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE TC_CODE=RSD_TC_CODE ";
                //strQry += " AND RSD_DELIVARY_DATE IS NULL AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%'";

                strQry = "SELECT COUNT(*) from TBLTCMASTER WHERE TC_STATUS= 9 AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashboard.sOfficeCode);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "NonrepairableCount");
                return ex.Message;
            }
        }

        public string ReleasedGoodCount(clsDashboard objDashboard)
        {
            string strQry = string.Empty;
            oleDbCommand = new OleDbCommand();
            //strQry = "SELECT COUNT(*) FROM TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE TC_CODE=RSD_TC_CODE ";
            //strQry += " AND RSD_DELIVARY_DATE IS NULL AND TC_LOCATION_ID LIKE '" + objDashoard.sOfficeCode + "%'";

            strQry = "SELECT COUNT(*) from TBLTCMASTER WHERE TC_STATUS= 11 AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
            oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashboard.sOfficeCode);
            return ObjCon.get_value(strQry, oleDbCommand);
        }

        // repair good dtr 
        public DataTable TotalTcfailedview(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();

            try
            {
                string strQry = string.Empty;
                strQry = "select TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE, (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID = TC_STORE_ID ) DIVISION ,(SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME FROM ";
                strQry += " TBLTCMASTER WHERE TC_STATUS=2 AND TC_LIVE_FLAG = 1 AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "TotalTcfailedview");
                return dt;
            }
        }
        /// <summary>
        /// This method used to get total faulty dtr details 
        /// </summary>
        /// <param name="sOfficeCode"></param>
        /// <returns></returns>
        public DataTable GetTotalFaultyTCview(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;

                strQry = " SELECT TC_CODE,TC_CAPACITY,TC_SLNO,TC_MANF_DATE ,  TM_NAME,DIVISION FROM (SELECT TC_CODE, ";
                strQry += " TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO, TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE ,";
                strQry += " (SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME ,";
                strQry += " (SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(TC_LOCATION_ID,1,2)) DIVISION FROM ";
                strQry += " TBLTCMASTER WHERE TC_CODE IN ( SELECT DF_EQUIPMENT_ID AS TC_CODE FROM TBLDTCFAILURE WHERE ";
                strQry += " DF_DTC_CODE IN (SELECT WO_DATA_ID  FROM MV_WORKFLOWSTATUSDUMMY WHERE ";
                strQry += " (INVOICE IS NOT NULL) AND  (CRREPORT IS NULL AND DECOMMISION IS NULL AND WO_BO_ID<>10) AND OFFCODE ";
                strQry += " LIKE :OfficeCode ||'%') AND DF_REPLACE_FLAG='0'))  ";
                strQry += " UNION ALL (SELECT TC_CODE, TO_CHAR(TC_CAPACITY)TC_CAPACITY, TC_SLNO, ";
                strQry += " TO_CHAR(TC_MANF_DATE, 'DD-MON-YYYY')TC_MANF_DATE, TM_NAME, D.DIV AS  DIVISION FROM ";
                strQry += " MV_WORKFLOWSTATUSDUMMY A, TBLDTCFAILURE B, TBLTCMASTER C, MV_VIEWPENDINGFAILURE D, TBLTRANSMAKES, ";
                strQry += " TBLDTCINVOICE I, TBLTCREPLACE R   WHERE  R.TR_IN_NO = I.IN_NO and I.IN_INV_NO = D.IN_INV_NO and ";
                strQry += " A.WO_DATA_ID = D.DT_CODE AND  TM_ID = TC_MAKE_ID  AND D.DF_ID = B.DF_ID  AND B.DF_EQUIPMENT_ID = C.TC_CODE ";
                strQry += " AND  A.RIAPPROVE IS  NULL AND a.DECOMMISION IS  NOT NULL  and D.RI_STATUS LIKE '%STORE KEEPER%' ";
                strQry += " AND A.WO_BO_ID <> 10  AND   A.OFFCODE LIKE :Offcode ||'%')";
                strQry += " UNION ALL(SELECT TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO, ";
                strQry += " TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE,(SELECT TM_NAME  FROM TBLTRANSMAKES ";
                strQry += " WHERE TM_ID = TC_MAKE_ID ) TM_NAME , (SELECT DIV_NAME FROM TBLDIVISION WHERE ";
                strQry += " DIV_CODE = SUBSTR(TC_LOCATION_ID,1,2)) DIVISION FROM  TBLTCMASTER WHERE TC_CURRENT_LOCATION='1' ";
                strQry += " AND TC_STATUS='3' AND TC_CODE<>0 AND TC_LOCATION_ID LIKE :OfficeCodes ||'%')  ";
                strQry += "  UNION ALL(SELECT ";
                strQry += " TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO, ";
                strQry += " TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE,(SELECT TM_NAME FROM TBLTRANSMAKES";
                strQry += " WHERE TM_ID = TC_MAKE_ID ) TM_NAME ,(SELECT DIV_NAME FROM TBLDIVISION WHERE ";
                strQry += " DIV_CODE = SUBSTR(TC_LOCATION_ID,1,2)) DIVISION FROM  TBLTCMASTER WHERE ";
                strQry += " TC_CURRENT_LOCATION='3' AND TC_STATUS='3' AND TC_CODE<>0 AND ";
                strQry += " TC_LOCATION_ID LIKE :OfficeCodeid ||'%') ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCodes", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCodeid", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("Offcode", sOfficeCode);

                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTotalFaultyTCview");
                return dt;
            }
        }
        public DataTable GetFaultyTCFieldview(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {

                string strQry = string.Empty;

                #region  // old query
                //strQry = "SELECT TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE FROM TBLTCMASTER WHERE ";
                //strQry += " TC_CURRENT_LOCATION ='2' AND TC_STATUS='3' AND LENGTH(TC_LOCATION_ID)='4' AND TC_LOCATION_ID LIKE '" + sOfficeCode + "%'";

                //strQry = "SELECT TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(TC_LOCATION_ID,1,2)) DIVISION,";
                //strQry += " (SELECT SD_SUBDIV_NAME FROM TBLSUBDIVMAST WHERE SD_SUBDIV_CODE = SUBSTR(TC_LOCATION_ID,1,3)) SUBDIVISION, ";
                //strQry += " (SELECT OM_NAME FROM TBLOMSECMAST WHERE OM_CODE = TC_LOCATION_ID  ) SECTION,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE ,(SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME FROM TBLTCMASTER WHERE ";
                //strQry += " TC_CODE IN (SELECT DF_EQUIPMENT_ID FROM TBLDTCFAILURE WHERE DF_DTC_CODE IN(SELECT WO_DATA_ID FROM MV_WORKFLOWSTATUSDUMMY WHERE ";
                //strQry += "(INVOICE IS NOT NULL) AND (CRREPORT IS NULL AND DECOMMISION IS NULL AND WO_BO_ID<>10) AND OFFCODE LIKE :OfficeCode ||'%') AND DF_REPLACE_FLAG='0')";


                //strQry = " SELECT TC_CODE , TO_CHAR(TC_CAPACITY)TC_CAPACITY , TC_SLNO , TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE  , DF_GUARANTY_TYPE ,  DIV_NAME AS  DIVISION  ,  ";
                //strQry += " SD_SUBDIV_NAME AS SUBDIVISION , OM_NAME AS SECTION , TM_NAME      FROM TBLTCMASTER  INNER JOIN TBLOMSECMAST  ON  OM_CODE = TC_LOCATION_ID  INNER JOIN   ";
                //strQry += " TBLSUBDIVMAST ON SD_SUBDIV_CODE = SUBSTR(TC_LOCATION_ID,1,3)  INNER JOIN TBLDIVISION ON DIV_CODE = SUBSTR(TC_LOCATION_ID,1,2) INNER JOIN TBLTRANSMAKES  ON  TM_ID = ";
                //strQry += " TC_MAKE_ID  INNER JOIN TBLDTCFAILURE ON TC_CODE = DF_EQUIPMENT_ID INNER JOIN MV_WORKFLOWSTATUSDUMMY ON DF_DTC_CODE = WO_DATA_ID WHERE (INVOICE IS NOT NULL) AND ( ";
                //strQry += " CRREPORT IS NULL AND DECOMMISION IS NULL AND WO_BO_ID<>10) AND OFFCODE LIKE :OfficeCode ||'%' AND DF_REPLACE_FLAG = 0 ";



                //oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                //dt = ObjCon.getDataTable(strQry, oleDbCommand);
                //return dt;
                #endregion

                strQry = " SELECT * FROM (SELECT TC_CODE, TO_CHAR(TC_CAPACITY)TC_CAPACITY, TC_SLNO, TO_CHAR(TC_MANF_DATE, 'DD-MON-YYYY')TC_MANF_DATE,";
                strQry += "DF_GUARANTY_TYPE, DIV_NAME AS  DIVISION, SD_SUBDIV_NAME AS SUBDIVISION, OM_NAME AS SECTION, TM_NAME ,IN_MANUAL_INVNO,IN_DATE ,TR_RI_DATE FROM TBLTCMASTER  INNER JOIN ";
                strQry += "TBLOMSECMAST  ON  OM_CODE = TC_LOCATION_ID  INNER JOIN    TBLSUBDIVMAST ON SD_SUBDIV_CODE = SUBSTR(TC_LOCATION_ID, 1, 3)  INNER JOIN ";
                strQry += "TBLDIVISION ON DIV_CODE = SUBSTR(TC_LOCATION_ID, 1, 2) INNER JOIN TBLTRANSMAKES  ON  TM_ID = TC_MAKE_ID  INNER JOIN TBLDTCFAILURE ";
                strQry += "ON TC_CODE = DF_EQUIPMENT_ID INNER JOIN MV_WORKFLOWSTATUSDUMMY ON DF_DTC_CODE = WO_DATA_ID";
                //Added inner join by sandeep on 21-10-2022 Bescouse Of Decommision Date Enhancement
                strQry += " inner join MV_VIEWPENDINGFAILURE on DTRCODE=TC_CODE WHERE(INVOICE IS NOT NULL)";
                strQry += "AND(CRREPORT IS NULL AND DECOMMISION IS NULL AND WO_BO_ID <> 10) AND OFFCODE LIKE '" + sOfficeCode + "%' AND DF_REPLACE_FLAG = 0 UNION ALL ";
                strQry += "SELECT TC_CODE, TO_CHAR(TC_CAPACITY)TC_CAPACITY, TC_SLNO, TO_CHAR(TC_MANF_DATE, 'DD-MON-YYYY')TC_MANF_DATE, DF_GUARANTY_TYPE,";
                strQry += "D.DIV AS  DIVISION, D.SUBDIVSION AS SUBDIVISION, OMSECTION AS SECTION, TM_NAME , I.IN_MANUAL_INVNO,TO_CHAR(I.IN_DATE,'DD-MON-YYYY')IN_DATE,TO_CHAR(R.TR_RI_DATE,'DD-MON-YYYY')TR_RI_DATE FROM  MV_WORKFLOWSTATUSDUMMY A, TBLDTCFAILURE B,";
                strQry += "TBLTCMASTER C, MV_VIEWPENDINGFAILURE D, TBLTRANSMAKES,TBLDTCINVOICE I,TBLTCREPLACE R   WHERE  R.TR_IN_NO=I.IN_NO and I.IN_INV_NO=D.IN_INV_NO and A.WO_DATA_ID = D.DT_CODE AND  TM_ID = TC_MAKE_ID  AND D.DF_ID = B.DF_ID  ";
                strQry += "AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.RIAPPROVE IS  NULL AND a.DECOMMISION IS  NOT NULL  and D.RI_STATUS LIKE '%STORE KEEPER%' AND ";
                strQry += "A.WO_BO_ID <> 10 AND   A.OFFCODE LIKE '" + sOfficeCode + "%' )A";




                //strQry = " SELECT* FROM (SELECT TC_CODE, TO_CHAR(TC_CAPACITY)TC_CAPACITY, TC_SLNO, TO_CHAR(TC_MANF_DATE, 'DD-MON-YYYY')TC_MANF_DATE, ";
                //strQry += " DF_GUARANTY_TYPE, DIV_NAME AS  DIVISION, SD_SUBDIV_NAME AS SUBDIVISION, OM_NAME AS SECTION, TM_NAME, ";
                //strQry += " TO_CHAR(TR_RI_DATE, 'DD-MON-YYYY')TR_RI_DATE, DT_CODE, DF_DTC_CODE FROM TBLTCMASTER  INNER JOIN ";
                //strQry += " TBLOMSECMAST  ON  OM_CODE = TC_LOCATION_ID  INNER JOIN    TBLSUBDIVMAST ON SD_SUBDIV_CODE = SUBSTR(TC_LOCATION_ID, 1, 3) ";
                //strQry += "  INNER JOIN TBLDIVISION ON DIV_CODE = SUBSTR(TC_LOCATION_ID, 1, 2) INNER JOIN TBLTRANSMAKES  ON  TM_ID = TC_MAKE_ID  ";
                //strQry += " INNER JOIN TBLDTCFAILURE ON TC_CODE = DF_EQUIPMENT_ID INNER JOIN MV_WORKFLOWSTATUSDUMMY ON DF_DTC_CODE = WO_DATA_ID ";
                //strQry += " inner join MV_VIEWPENDINGFAILURE on DTRCODE = TC_CODE WHERE(INVOICE IS NOT NULL)AND(CRREPORT IS NULL AND ";
                //strQry += " DECOMMISION IS NULL AND WO_BO_ID <> 10) AND OFFCODE LIKE '" + sOfficeCode + "%' AND DF_REPLACE_FLAG = 0 ";

                //strQry += " UNION ALL SELECT TC_CODE, TO_CHAR(TC_CAPACITY)TC_CAPACITY, TC_SLNO, TO_CHAR(TC_MANF_DATE, 'DD-MON-YYYY')TC_MANF_DATE, ";
                //strQry += "  DF_GUARANTY_TYPE, D.DIV AS  DIVISION, D.SUBDIVSION AS SUBDIVISION, OMSECTION AS SECTION, TM_NAME, ";
                //strQry += " TO_CHAR(R.TR_RI_DATE, 'DD-MON-YYYY')TR_RI_DATE, DT_CODE, DF_DTC_CODE FROM  MV_WORKFLOWSTATUSDUMMY A, ";
                //strQry += "  TBLDTCFAILURE B, TBLTCMASTER C, MV_VIEWPENDINGFAILURE D, TBLTRANSMAKES, TBLDTCINVOICE I, TBLTCREPLACE R ";
                //strQry += " WHERE  R.TR_IN_NO = I.IN_NO and I.IN_INV_NO = D.IN_INV_NO and A.WO_DATA_ID = D.DT_CODE AND  TM_ID = TC_MAKE_ID  ";
                //strQry += " AND D.DF_ID = B.DF_ID  AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.RIAPPROVE IS  NULL AND a.DECOMMISION IS  NOT NULL ";
                //strQry += " and D.RI_STATUS LIKE '%STORE KEEPER%' AND A.WO_BO_ID <> 10 AND   A.OFFCODE LIKE '" + sOfficeCode + "%')A ";

                //strQry += " LEFT JOIN(Select DF_ID, DF_DTC_CODE, DF_EQUIPMENT_ID, TD_TC_NO, TD_INV_NO from TBLTCDRAWN inner join TBLDTCFAILURE ";
                //strQry += " ON TD_DF_ID = DF_ID)B On TD_TC_NO = TC_CODE AND B.DF_DTC_CODE = DT_CODE ";
                //strQry += " LEFT JOIN (SELECT IN_NO, IN_MANUAL_INVNO, TO_CHAR(IN_DATE,'DD-mm-YYYY')IN_DATE FROM TBLDTCINVOICE)C ";
                //strQry += " ON B.TD_INV_NO = C.IN_NO ";

                dt = ObjCon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyTCFieldview");
                return dt;
            }
        }
        public DataTable GetFaultyTCStoreview(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {
                //string strQry = string.Empty;
                //strQry = "SELECT TC_CODE,(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(TC_LOCATION_ID,1,2)) DIVISION, ";
                //strQry += " CASE TC_UPDATED_EVENT WHEN 'FAILURE ENTRY' THEN (SELECT DF_GUARANTY_TYPE FROM TBLDTCFAILURE WHERE DF_ID = TC_UPDATED_EVENT_ID ) ELSE '' END DF_GUARANTY_TYPE  , ";
                //strQry += " TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE ,(SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME ";
                //strQry += "FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION='1' AND TC_LIVE_FLAG = 1 AND TC_STATUS='3' AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                //oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                //dt = ObjCon.getDataTable(strQry, oleDbCommand);
                //return dt;
                string strQry = "SELECT * FROM (SELECT TC_CODE, CASE TC_UPDATED_EVENT WHEN 'FAILURE ENTRY' THEN(SELECT DF_GUARANTY_TYPE FROM TBLDTCFAILURE " +
                  "WHERE DF_ID = TC_UPDATED_EVENT_ID) ELSE '' END DF_GUARANTY_TYPE, TO_CHAR(TC_CAPACITY)TC_CAPACITY, TC_SLNO, TO_CHAR(TC_MANF_DATE, " +
                  "'DD-MON-YYYY')TC_MANF_DATE, (SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME,(SELECT DIV_NAME FROM TBLDIVISION " +
                  "WHERE DIV_CODE = SUBSTR(TC_LOCATION_ID, 1, 2)) DIVISION FROM TBLTCMASTER WHERE TC_CURRENT_LOCATION = '1' AND TC_LIVE_FLAG = 1 AND " +
                  "TC_STATUS = '3' AND TC_LOCATION_ID  LIKE :OfficeCode1 ||'%' UNION ALL SELECT TC_CODE, DF_GUARANTY_TYPE, TO_CHAR(TC_CAPACITY)TC_CAPACITY, " +
                  "TC_SLNO, TO_CHAR(TC_MANF_DATE, 'DD-MON-YYYY')TC_MANF_DATE, TM_NAME, D.DIV AS  DIVISION  FROM  MV_WORKFLOWSTATUSDUMMY A, TBLDTCFAILURE B, " +
                  "TBLTCMASTER C, MV_VIEWPENDINGFAILURE D, TBLTRANSMAKES  WHERE A.WO_DATA_ID = D.DT_CODE AND  TM_ID = TC_MAKE_ID  AND D.DF_ID = B.DF_ID " +
                  "AND B.DF_EQUIPMENT_ID = C.TC_CODE  AND  A.RIAPPROVE IS  NULL AND a.DECOMMISION IS  NOT NULL  and D.RI_STATUS LIKE '%STORE OFFICER%' AND " +
                  "A.WO_BO_ID <> 10 AND   A.OFFCODE  LIKE :OfficeCode ||'%') A";
                oleDbCommand.Parameters.AddWithValue("OfficeCode1", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyTCStoreview");
                return dt;
            }
        }
        public DataTable GetFaultyTCRepairview(string sOfficeCode, string repaireType)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT  TC_CODE,CASE WHEN RSM_SUPREP_TYPE = 2 THEN (SELECT TR_NAME FROM TBLTRANSREPAIRER WHERE TR_ID = RSM_SUPREP_ID)";
                strQry += " WHEN RSM_SUPREP_TYPE = 1 THEN (SELECT TS_NAME FROM TBLTRANSSUPPLIER WHERE TS_ID = RSM_SUPREP_ID) END SUPPLIER, ";
                strQry += " TO_CHAR(TC_CAPACITY)TC_CAPACITY,(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = SUBSTR(TC_LOCATION_ID,1,2)) DIVISION ";
                strQry += " ,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE,RSM_PO_NO ,TO_CHAR(RSM_PO_DATE,'DD-MON-YYYY')  RSM_PO_DATE , TO_CHAR(RSD_INV_DATE,'DD-MON-YYYY')  RSD_INV_DATE , ";
                strQry += " (SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME ";
                strQry += " FROM TBLTCMASTER , TBLREPAIRSENTDETAILS,TBLREPAIRSENTMASTER  WHERE RSM_ID = RSD_RSM_ID and RSD_TC_CODE = TC_CODE and RSM_SUPREP_TYPE = '" + repaireType + "' ";
                strQry += " and  RSD_INV_DATE IS not null  and RSD_DELIVARY_DATE is NULL   AND TC_STATUS='3' AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyTCRepairview");
                return dt;
            }
        }

        public DataTable GetStore_TcDetails(string sOfficeCode, string sWOslno)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                if (sOfficeCode.Length > 1)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 2);
                }
                strQry = "SELECT TC_CODE,TC_SLNO,TO_CHAR(TC_CAPACITY)TC_CAPACITY,CASE WHEN TC_STATUS=1 THEN 'BRAND NEW' WHEN TC_STATUS=2 THEN 'REPAIR GOOD' END ";
                strQry += " STATUS,SM_NAME FROM TBLTCMASTER,TBLSTOREMAST WHERE TC_LOCATION_ID=SM_CODE AND TC_LOCATION_ID=:OfficeCode ";
                strQry += " AND  TC_CAPACITY IN (SELECT WO_NEW_CAP FROM TBLWORKORDER WHERE WO_SLNO=:WOSLNO) AND TC_STATUS IN (1,2) AND TC_CURRENT_LOCATION=1";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                oleDbCommand.Parameters.AddWithValue("WOSLNO", sWOslno);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTransformerCount");
                return dt;
            }
        }


        // DTR at Store
        public DataTable GetBrandNew(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = " SELECT  TC_CODE ,TO_CHAR(TC_CAPACITY)TC_CAPACITY ,TC_SLNO ,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE , ";
                strQry += "SM_NAME AS DIVISION ,PO_NO , PO_DATE ,VM_NAME , (SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME ";
                strQry += "FROM  (SELECT * FROM TBLTCMASTER INNER JOIN TBLSTOREMAST ON SM_CODE = TC_LOCATION_ID   LEFT JOIN ";
                strQry += " (SELECT PO_NO ,TO_CHAR(PO_DATE,'DD-MON-YYYY')PO_DATE ,VM_NAME   FROM TBLPOMASTER , TBLVENDORMASTER ";
                strQry += " WHERE PO_SUPPLIER_ID = VM_MMS_ID ) ON TC_PO_NO = PO_NO WHERE TC_LIVE_FLAG = 1 AND TC_STATUS=1 AND  TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%') ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetBrandNew");
                return dt;
            }
        }

        public DataTable GetScrapStore(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "select TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE, SM_NAME AS DIVISION ,(SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME  FROM  ";
                strQry += " TBLTCMASTER , TBLSTOREMAST  WHERE SM_CODE = TC_LOCATION_ID AND  TC_LIVE_FLAG = 1 AND TC_STATUS=4 AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetScrapStore");
                return dt;
            }
        }

        public DataTable GetNonRepairable(string sOfficeCode)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "select TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,TC_SLNO,TO_CHAR(TC_MANF_DATE,'DD-MON-YYYY')TC_MANF_DATE, SM_NAME AS DIVISION ,(SELECT TM_NAME  FROM TBLTRANSMAKES WHERE TM_ID = TC_MAKE_ID ) TM_NAME FROM ";
                strQry += " TBLTCMASTER , TBLSTOREMAST  WHERE SM_CODE = TC_LOCATION_ID AND TC_STATUS=9 AND TC_LIVE_FLAG = 1 AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE :OfficeCode ||'%'";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetNonRepairable");
                return dt;
            }
        }

        public DataTable GetDTRCounts(clsDashboard objDashboard)
        {
            DataTable dt = new DataTable();
            oleDbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = " SELECT SUM(CASE WHEN (TC_STATUS= 2 AND TC_CURRENT_LOCATION=1) THEN 1 ELSE 0 END ) \"REPAIR GOOD\", ";
                strQry += " SUM(CASE WHEN (TC_STATUS= 1 AND TC_LIVE_FLAG = 1 AND TC_CURRENT_LOCATION=1) THEN 1 ELSE 0 END ) \"BRAND NEW\", ";
                strQry += " SUM(CASE WHEN (TC_STATUS= 4  AND TC_CURRENT_LOCATION=1) THEN 1 ELSE 0 END ) \"SCRAP\", ";
                strQry += " SUM(CASE WHEN (TC_STATUS= 9 AND TC_CURRENT_LOCATION=1)  THEN 1  ELSE 0 END ) \"NON REPAIRABLE\", ";
                strQry += " sum(CASE WHEN (TC_CURRENT_LOCATION='1' AND TC_STATUS='3') THEN 1 ELSE 0 END ) \"FAULTY STORE\", ";
                strQry += " SUM(CASE WHEN (TC_CURRENT_LOCATION='3' AND TC_STATUS='3') THEN 1 ELSE 0 END) \"REPAIRER\", ";
                strQry += " SUM(CASE WHEN (TC_CURRENT_LOCATION='1' AND TC_STATUS='11') THEN 1 ELSE 0 END) \"RELEASED GOOD\" ";
                strQry += " FROM TBLTCMASTER  WHERE TC_LOCATION_ID LIKE :OfficeCode ||'%' AND TC_LIVE_FLAG = 1 ";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", objDashboard.sOfficeCode);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTRCounts");
                return dt;
            }
        }

        public DataTable LoadOfficeDet(clsDashboard objStation)
        {
            oleDbCommand = new OleDbCommand();
            DataTable DtStationDet = new DataTable();
            try
            {

                string strQry = string.Empty;

                strQry = "select OFF_CODE,OFF_NAME FROM VIEW_ALL_OFFICES WHERE  OFF_NAME IS NOT NULL";
                if (objStation.sOfficeCode != "")
                {

                    strQry += " AND OFF_CODE LIKE :officeCode||'%' ";
                    oleDbCommand.Parameters.AddWithValue("officeCode", objStation.sOfficeCode);
                }
                if (objStation.sOfficeName != "")
                {
                    oleDbCommand = new OleDbCommand();
                    strQry += " AND UPPER(OFF_NAME) LIKE : officeName||'%' ";
                    oleDbCommand.Parameters.AddWithValue("officeName", objStation.sOfficeName.ToUpper());
                }
                strQry += " order by OFF_CODE";
                DtStationDet = ObjCon.getDataTable(strQry, oleDbCommand);

                return DtStationDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadOfficeDet");
                return DtStationDet;
            }
            finally
            {

            }

        }
    }
}

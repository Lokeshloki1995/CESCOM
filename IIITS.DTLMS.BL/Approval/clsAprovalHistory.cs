using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;

namespace IIITS.DTLMS.BL
{
    public class clsAprovalHistory
    {
        string strFormCode = "clsAprovalHistory";
        CustOledbConnection objcon = new CustOledbConnection(Constants.Password);

        public string sRecordId { get; set; }
        public string sBOId { get; set; }
        public string sDescription { get; set; }
        public string sStatus { get; set; }

        public string sDTCCode { get; set; }
        public string sDTCName { get; set; }
        public string sDTRCode { get; set; }

        public DataTable LoadApprovalHistory(string sRecordId, string sBOId)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry = " SELECT WO_ID, (SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CR_BY = US_ID) INITIATOR , TO_CHAR(WO_CR_ON,'DD-MON-YY HH:MI AM') WO_CR_ON, ";
                strQry += " (SELECT WO_USER_COMMENT  FROM TBLWORKFLOWOBJECTS A  WHERE A.WO_ID = B.WO_PREV_APPROVE_ID) WO_USER_COMMENT FROM TBLWORKFLOWOBJECTS B  ";
                strQry += "  WHERE WO_RECORD_ID='" + sRecordId + "' AND WO_BO_ID = '" + sBOId + "' ORDER BY WO_ID ";
                dt = objcon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadApprovalHistory");
                return dt;
            }
        }

        public clsAprovalHistory GetStatusofApproval(clsAprovalHistory objHistory)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                strQry = "SELECT STATUS,WO_DESCRIPTION FROM (";
                strQry += " SELECT CASE WHEN WO_APPROVE_STATUS = 1 THEN 'APPROVED' ELSE 'PENDING WITH ' || (SELECT RO_NAME FROM TBLROLES WHERE WO_NEXT_ROLE = RO_ID) || '-' ||";
                strQry += " (SELECT OFF_NAME FROM VIEW_ALL_OFFICES WHERE OFF_CODE = (SELECT WO_OFFICE_CODE FROM TBLWORKFLOWOBJECTS B WHERE A.WO_PREV_APPROVE_ID = B.WO_ID )) ";
                strQry += " END STATUS,WO_DESCRIPTION FROM TBLWORKFLOWOBJECTS A WHERE WO_RECORD_ID='" + objHistory.sRecordId + "' AND WO_BO_ID = '" + objHistory.sBOId + "' ORDER BY WO_ID DESC) WHERE ROWNUM = 1";
                dt=  objcon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    objHistory.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                    objHistory.sStatus = Convert.ToString(dt.Rows[0]["STATUS"]);
                }

                return objHistory;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStatusofApproval");
                return objHistory;
            }
        }

        public clsAprovalHistory GetDTCDetails(clsAprovalHistory objHistory)
        {
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                if (objHistory.sBOId == "9" || objHistory.sBOId == "10")
                {
                    strQry = "SELECT DISTINCT DT_NAME,DT_CODE,(SELECT TC_CODE FROM TBLTCMASTER WHERE TC_CODE=DT_TC_ID) TC_CODE ";
                    strQry += " FROM TBLDTCMAST,TBLWORKFLOWOBJECTS WHERE WO_DATA_ID=DT_CODE AND WO_RECORD_ID='" + objHistory.sRecordId + "' and WO_BO_ID='" + objHistory.sBOId + "'";
                }
                if (objHistory.sBOId == "11")
                {
                    strQry = "SELECT DISTINCT DT_NAME,DT_CODE,DF_EQUIPMENT_ID TC_CODE FROM TBLDTCMAST,TBLWORKFLOWOBJECTS,";
                    strQry += " TBLDTCFAILURE WHERE WO_DATA_ID=DF_ID AND WO_RECORD_ID='" + objHistory.sRecordId + "' and WO_BO_ID='" + objHistory.sBOId + "' AND DF_DTC_CODE=DT_CODE";
                }
                if (objHistory.sBOId == "12")
                {
                    strQry = "SELECT DISTINCT DT_NAME,DT_CODE,DF_EQUIPMENT_ID TC_CODE FROM TBLDTCMAST,TBLWORKFLOWOBJECTS,";
                    strQry += " TBLDTCFAILURE,TBLWORKORDER WHERE WO_DATA_ID=WO_SLNO AND WO_RECORD_ID='" + objHistory.sRecordId + "' ";
                    strQry += " and WO_BO_ID='" + objHistory.sBOId + "' AND DF_DTC_CODE=DT_CODE AND DF_ID=WO_DF_ID";
                }
                if (objHistory.sBOId == "13")
                {
                    strQry = "SELECT DISTINCT DT_NAME,DT_CODE,DF_EQUIPMENT_ID TC_CODE FROM TBLDTCMAST,TBLWORKFLOWOBJECTS,";
                    strQry += " TBLDTCFAILURE,TBLWORKORDER,TBLINDENT WHERE WO_DATA_ID=TI_ID AND WO_RECORD_ID='" + objHistory.sRecordId + "' ";
                    strQry += " and WO_BO_ID='" + objHistory.sBOId + "' AND DF_DTC_CODE=DT_CODE AND DF_ID=WO_DF_ID AND WO_SLNO=TI_WO_SLNO";
                }
                if (objHistory.sBOId == "14")
                {
                    strQry = "SELECT DISTINCT DT_NAME,DT_CODE,(SELECT TC_CODE FROM TBLTCMASTER WHERE TC_CODE=DT_TC_ID) TC_CODE ";
                    strQry += " FROM TBLDTCMAST,TBLWORKFLOWOBJECTS WHERE WO_DATA_ID=DT_CODE AND WO_RECORD_ID='" + objHistory.sRecordId + "' and WO_BO_ID='" + objHistory.sBOId + "'";
                }
                if (objHistory.sBOId == "15")
                {
                    strQry = "SELECT DISTINCT DT_NAME,DT_CODE,(SELECT TC_CODE FROM TBLTCMASTER WHERE TC_CODE=DT_TC_ID) TC_CODE ";
                    strQry += " FROM TBLDTCMAST,TBLWORKFLOWOBJECTS WHERE WO_DATA_ID=DT_CODE AND WO_RECORD_ID='" + objHistory.sRecordId + "' and WO_BO_ID='" + objHistory.sBOId + "'";
                }
                if (objHistory.sBOId == "26")
                {
                    strQry = " SELECT DISTINCT DT_NAME,DT_CODE,DF_EQUIPMENT_ID TC_CODE FROM TBLDTCMAST,TBLWORKFLOWOBJECTS, ";
                    strQry += " TBLDTCFAILURE,TBLDTCINVOICE,TBLTCREPLACE,TBLTCDRAWN WHERE WO_RECORD_ID=TR_ID AND WO_RECORD_ID='" + objHistory.sRecordId + "' ";
                    strQry += " and WO_BO_ID='" + objHistory.sBOId + "' AND DF_DTC_CODE=DT_CODE AND DF_ID=TD_DF_ID AND IN_NO=TD_INV_NO AND TR_IN_NO=IN_NO";
                }
                if(strQry!="")
                {
                    dt = objcon.getDataTable(strQry);
                }
                if (dt.Rows.Count > 0)
                {
                    objHistory.sDTCCode = Convert.ToString(dt.Rows[0]["DT_CODE"]);
                    objHistory.sDTCName = Convert.ToString(dt.Rows[0]["DT_NAME"]);
                    objHistory.sDTRCode = Convert.ToString(dt.Rows[0]["TC_CODE"]);
                }
                return objHistory;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCDetails");
                return objHistory;
            }
        }
    }
}

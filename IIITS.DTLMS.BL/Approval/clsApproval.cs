//**************************** Work Flow Concept******************************//
//*************************** Logic By : Ramesh Sir **************************//
//*************************** Code By : Priya *********************************//
//*************************** Last Update : 24/06/2016********************** //


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using IIITS.DTLMS.BL;
using System.Configuration;

namespace IIITS.DTLMS.BL
{
    public class clsApproval
    {
        string strFormCode = "clsApproval";
        public string sRoleId { get; set; }
        public string sBOId { get; set; }
        public string sAccessType { get; set; }
        public string sFormName { get; set; }

        public string sRefTypeforInbox { get; set; }

        public string sRecordId { get; set; }
        public string sOfficeCode { get; set; }
        public string sApproveStatus { get; set; }
        public string sPrevApproveId { get; set; }
        public string sClientIp { get; set; }
        public string sApproveComments { get; set; }
        public string sWFObjectId { get; set; }

        public string sQryValues { get; set; }
        public string sDescription { get; set; }
        public string sParameterValues { get; set; }
        public string sXMLdata { get; set; }
        public string sTableNames { get; set; }
        public string sColumnNames { get; set; }
        public string sColumnValues { get; set; }

        public string sCrby { get; set; }

        public string sMainTable { get; set; }
        public string sRefColumnName { get; set; }
        public string sApproveColumnName { get; set; }

        public string sBOFlowMasterId { get; set; }
        public string sPrevWFOId { get; set; }

        public string sWFAutoId { get; set; }
        public string sWFDataId { get; set; }
        public string sNewRecordId { get; set; }
        public string sWFInitialId { get; set; }
        public string sDataReferenceId { get; set; }
        public string sRefOfficeCode { get; set; }
        public string sMaxID { get; set; }

        public bool sIsOldPo { get; set; }
        public string sagency { get; set; }

        public string sFromDate { get; set; }
        public string sToDate { get; set; }
        public string sTI_ID { get; set; }
        public string sWO_ID { get; set; }



        string Intigration_crby;

        CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        OleDbCommand oledbCommand;
        //Approve Status
        // 0--->Pending    1---->Approved    2----> Modify and Approve   3--> Reject   4----> Abort



        /// <summary>
        /// To check Access of Forms Based on Roles
        /// </summary>
        /// <param name="objApproval">Role Id, Form Name</param>
        /// <returns></returns>
        public bool CheckAccessRights(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            string strQry = string.Empty;
            DataTable dt = new DataTable();

            try
            {
                if (Convert.ToString(ConfigurationManager.AppSettings["AccessRights"]).ToUpper().Equals("OFF"))
                {
                    return true;
                }

                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY
                strQry = "SELECT UR_ACCESSTYPE FROM TBLUSERROLEMAPPING WHERE UR_ROLEID='" + objApproval.sRoleId + "' AND UR_BOID IN ";
                strQry += " (SELECT BO_ID FROM TBLBUSINESSOBJECT WHERE UPPER(BO_FORMNAME)='" + objApproval.sFormName.Trim().ToUpper() + "') ";
                strQry += " AND UR_ACCESSTYPE IN (" + objApproval.sAccessType + ")  ORDER BY UR_ACCESSTYPE";
                dt = objcon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }


        /// <summary>
        /// Load Approval Inbox with Pending Details
        /// </summary>
        /// <param name="objApproval">Office Code,Role Id</param>
        /// <returns></returns>
        #region SRUJAN sIR qUERY
        //public DataTable LoadPendingApprovalInbox(clsApproval objApproval)
        //{

        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = "SELECT WO_ID,WO_RECORD_ID,WO_BO_ID,BO_NAME,USER_NAME,US_FULL_NAME AS CREATOR,CR_ON,STATUS,WO_APPROVE_STATUS,RO_NAME,CURRENT_STATUS,WO_DESCRIPTION,WOA_ID,WO_WFO_ID,WO_INITIAL_ID FROM (SELECT WO_ID,WO_RECORD_ID,WO_BO_ID,BO_NAME,(SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CR_BY=US_ID) USER_NAME,TO_CHAR(WO_CR_ON,'DD-MON-YYYY') CR_ON,";
        //        strQry += " DECODE( WO_APPROVE_STATUS,'0','PENDING','1','APPROVED','2','MODIFY AND APPROVE','3','REJECTED','OTHERS') STATUS,WO_APPROVE_STATUS, ";
        //        strQry += " (SELECT RO_NAME FROM TBLROLES WHERE WO_NEXT_ROLE=RO_ID) RO_NAME,'' AS CURRENT_STATUS,WO_DESCRIPTION,0 AS WOA_ID,WO_WFO_ID,WO_INITIAL_ID ";
        //        strQry += " FROM TBLWORKFLOWOBJECTS,TBLBUSINESSOBJECT WHERE WO_BO_ID=BO_ID AND WO_NEXT_ROLE='" + objApproval.sRoleId + "' ";
        //        strQry += " AND WO_APPROVE_STATUS='0' AND WO_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%'";

        //        if (objApproval.sBOId != null)
        //        {
        //            strQry += " AND WO_BO_ID='" + objApproval.sBOId + "' ";
        //        }
        //        if (objApproval.sCrby != null)
        //        {
        //            strQry += " AND WO_CR_BY='" + objApproval.sCrby + "' ";
        //        }
        //        if (objApproval.sFromDate != "")
        //        {
        //            DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')>='" + dFromDate.ToString("yyyyMMdd") + "'";

        //        }
        //        if (objApproval.sToDate != "")
        //        {
        //            DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')<='" + dToDate.ToString("yyyyMMdd") + "'";
        //        }

        //        if (objApproval.sFormName != "")
        //        {
        //            strQry += " AND UPPER(BO_NAME) LIKE '" + objApproval.sFormName.ToUpper() + "%' ";
        //        }
        //        if (objApproval.sDescription != "")
        //        {
        //            strQry += " AND UPPER(WO_DESCRIPTION) LIKE '%" + objApproval.sDescription.ToUpper() + "%' ";
        //        }

        //        strQry += " UNION ALL ";
        //        strQry += " SELECT WO_ID,WO_RECORD_ID,(SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID) AS WO_BO_ID,BO_NAME,(SELECT US_FULL_NAME ";
        //        strQry += "  FROM TBLUSER WHERE WOA_CRBY=US_ID) USER_NAME,TO_CHAR(WOA_CRON,'DD-MON-YYYY')CR_ON,'PENDING' AS STATUS,0 AS WO_APPROVE_STATUS, ";
        //        strQry += " (SELECT RO_NAME FROM TBLROLES WHERE WOA_ROLE_ID=RO_ID) RO_NAME,'' AS CURRENT_STATUS,WOA_DESCRIPTION AS WO_DESCRIPTION,WOA_ID,0 AS WO_WFO_ID, ";
        //        strQry += " WO_INITIAL_ID FROM TBLWO_OBJECT_AUTO,TBLBUSINESSOBJECT,TBLWORKFLOWOBJECTS WHERE ";
        //        strQry += " (SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID) = BO_ID AND ";
        //        strQry += " WOA_INITIAL_ACTION_ID IS NULL AND WOA_PREV_APPROVE_ID=WO_ID AND WOA_ROLE_ID='" + objApproval.sRoleId + "' AND WOA_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%'";
        //        //strQry += " WOA_INITIAL_ACTION_ID IS NULL AND WOA_FLAG IS NULL AND WOA_PREV_APPROVE_ID=WO_ID AND WOA_ROLE_ID='" + objApproval.sRoleId + "' AND WOA_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%'";


        //        if (objApproval.sBOId != null)
        //        {
        //            strQry += " AND (SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID)='" + objApproval.sBOId + "' ";
        //        }
        //        if (objApproval.sCrby != null)
        //        {
        //            strQry += " AND WO_CR_BY='" + objApproval.sCrby + "' ";
        //        }
        //        if (objApproval.sFromDate != "")
        //        {
        //            DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')>='" + dFromDate.ToString("yyyyMMdd") + "'";

        //        }
        //        if (objApproval.sToDate != "")
        //        {
        //            DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')<='" + dToDate.ToString("yyyyMMdd") + "'";
        //        }

        //        if (objApproval.sFormName != "")
        //        {
        //            strQry += " AND UPPER(BO_NAME) LIKE '" + objApproval.sFormName.ToUpper() + "%' ";
        //        }
        //        if (objApproval.sDescription != "")
        //        {
        //            strQry += " AND UPPER(WOA_DESCRIPTION) LIKE '%" + objApproval.sDescription.ToUpper() + "%' ";
        //        }

        //        strQry += " ORDER BY WO_ID DESC)A, VIEW_INITIATOR, TBLUSER  WHERE WO_INITIAL_ID = INITIAL_ID AND US_ID =  CRBY AND ROWNUM<500";

        //        return objcon.getDataTable(strQry);
        //        // AND WO_OFFICE_CODE LIKE '"+ objApproval.sOfficeCode +"%'
        //        //AND WOA_OFFICE_CODE LIKE '" + objApproval.sOfficeCode + "%'
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadPendingApprovalInbox");
        //        return dt;
        //    }
        //}
        #endregion

        public DataTable LoadPendingApprovalInbox(clsApproval objApproval)
        {

            DataTable dt = new DataTable();
            try
            {
                oledbCommand = new OleDbCommand();
                DateTime dToDate = new DateTime();
                DateTime dFromDate = new DateTime();
                string strQry = string.Empty;
                strQry = "SELECT WO_ID,WO_RECORD_ID,WO_BO_ID,BO_NAME,USER_NAME,(SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID=(SELECT max(B.WO_CR_BY) KEEP (DENSE_RANK LAST ORDER BY B.WO_ID DESC) FROM TBLWORKFLOWOBJECTS B WHERE B.WO_INITIAL_ID = A.WO_INITIAL_ID)) AS CREATOR,CR_ON,STATUS,WO_APPROVE_STATUS,RO_NAME,CURRENT_STATUS,WO_DESCRIPTION,WOA_ID,WO_WFO_ID,WO_INITIAL_ID FROM (SELECT WO_ID,WO_RECORD_ID,WO_BO_ID,BO_NAME,(SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CR_BY=US_ID) USER_NAME,TO_CHAR(WO_CR_ON,'DD-MON-YYYY') CR_ON,";
                strQry += " DECODE( WO_APPROVE_STATUS,'0','PENDING','1','APPROVED','2','MODIFY AND APPROVE','3','REJECTED','OTHERS') STATUS,WO_APPROVE_STATUS, ";
                strQry += " (SELECT RO_NAME FROM TBLROLES WHERE WO_NEXT_ROLE=RO_ID) RO_NAME,'' AS CURRENT_STATUS,WO_DESCRIPTION,0 AS WOA_ID,WO_WFO_ID,WO_INITIAL_ID ";
                strQry += " FROM TBLWORKFLOWOBJECTS,TBLBUSINESSOBJECT WHERE WO_BO_ID=BO_ID AND WO_NEXT_ROLE=:RoleId";
                strQry += " AND WO_APPROVE_STATUS='0' AND WO_REF_OFFCODE LIKE:OfficeCode||'%' ";

                oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                oledbCommand.Parameters.AddWithValue("OfficeCode", objApproval.sOfficeCode);

                if (objApproval.sBOId != null)
                {
                    strQry += " AND WO_BO_ID=:BOId";
                    oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                }
                if (objApproval.sCrby != null)
                {
                    strQry += " AND WO_CR_BY=:Crby";
                    oledbCommand.Parameters.AddWithValue("Crby", objApproval.sCrby);
                }
                if (objApproval.sFromDate != "")
                {
                    dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')>=:FromDate";
                    oledbCommand.Parameters.AddWithValue("FromDate", dFromDate.ToString("yyyyMMdd"));


                }
                if (objApproval.sToDate != "")
                {
                    dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')<=:ToDate";
                    oledbCommand.Parameters.AddWithValue("ToDate", dToDate.ToString("yyyyMMdd"));
                }

                if (objApproval.sFormName != "")
                {
                    strQry += " AND UPPER(BO_NAME) LIKE:FormName||'%' ";
                    oledbCommand.Parameters.AddWithValue("FormName", objApproval.sFormName.ToUpper());
                }
                if (objApproval.sDescription != "")
                {
                    strQry += " AND UPPER(WO_DESCRIPTION) LIKE '%'|| :Description ||'%' ";
                    oledbCommand.Parameters.AddWithValue("Description", objApproval.sDescription.ToUpper());
                }

                strQry += " UNION ALL ";
                strQry += " SELECT WO_ID,WO_RECORD_ID,(SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID) AS WO_BO_ID,BO_NAME,(SELECT US_FULL_NAME ";
                strQry += "  FROM TBLUSER WHERE WOA_CRBY=US_ID) USER_NAME,TO_CHAR(WOA_CRON,'DD-MON-YYYY')CR_ON,'PENDING' AS STATUS,0 AS WO_APPROVE_STATUS, ";
                strQry += " (SELECT RO_NAME FROM TBLROLES WHERE WOA_ROLE_ID=RO_ID) RO_NAME,'' AS CURRENT_STATUS,WOA_DESCRIPTION AS WO_DESCRIPTION,WOA_ID,0 AS WO_WFO_ID, ";
                strQry += " WO_INITIAL_ID FROM TBLWO_OBJECT_AUTO,TBLBUSINESSOBJECT,TBLWORKFLOWOBJECTS WHERE ";
                strQry += " (SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID) = BO_ID AND ";
                strQry += " WOA_INITIAL_ACTION_ID IS NULL AND WOA_PREV_APPROVE_ID=WO_ID AND WOA_ROLE_ID=:RoleId1 AND WOA_REF_OFFCODE LIKE:OfficeCode1||'%'";
                //strQry += " WOA_INITIAL_ACTION_ID IS NULL AND WOA_FLAG IS NULL AND WOA_PREV_APPROVE_ID=WO_ID AND WOA_ROLE_ID='" + objApproval.sRoleId + "' AND WOA_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%'";

                oledbCommand.Parameters.AddWithValue("RoleId1", objApproval.sRoleId);
                oledbCommand.Parameters.AddWithValue("OfficeCode1", objApproval.sOfficeCode);

                if (objApproval.sBOId != null)
                {
                    strQry += " AND (SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID)=:BOIds ";
                    oledbCommand.Parameters.AddWithValue("BOIds", objApproval.sBOId);
                }
                if (objApproval.sCrby != null)
                {
                    strQry += " AND WO_CR_BY=:Crbys ";
                    oledbCommand.Parameters.AddWithValue("Crbys", objApproval.sCrby);
                }
                if (objApproval.sFromDate != "")
                {
                    dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')>=:FromDates";
                    oledbCommand.Parameters.AddWithValue("FromDates", dFromDate.ToString("yyyyMMdd"));

                }
                if (objApproval.sToDate != "")
                {
                    dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')<=:ToDates";
                    oledbCommand.Parameters.AddWithValue("ToDates", dToDate.ToString("yyyyMMdd"));
                }

                if (objApproval.sFormName != "")
                {
                    strQry += " AND UPPER(BO_NAME) LIKE :FormNames||'%' ";
                    oledbCommand.Parameters.AddWithValue("FormNames", objApproval.sFormName.ToUpper());
                }
                if (objApproval.sDescription != "")
                {
                    strQry += " AND UPPER(WOA_DESCRIPTION) LIKE '%'||:Descriptions||'%' ";
                    oledbCommand.Parameters.AddWithValue("Descriptions", objApproval.sDescription.ToUpper());
                }

                strQry += " ORDER BY WO_ID DESC)A WHERE ROWNUM<500";

                return objcon.getDataTable(strQry, oledbCommand);

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
        }

        /// <summary>
        /// Load Approval Inbox with Already Approved Details
        /// </summary>
        /// <param name="objApproval">Office Code,Role Id</param>
        /// <returns></returns>
        #region old LoadAlreadyApprovedInbox Query 
        //public DataTable LoadAlreadyApprovedInbox(clsApproval objApproval)
        //{
        //    
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string strQry = string.Empty;
        //        strQry = " SELECT WO_ID,WO_RECORD_ID,WO_BO_ID,BO_NAME,USER_NAME,(SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID=(SELECT max(B.WO_CR_BY) KEEP (DENSE_RANK LAST ORDER BY B.WO_ID DESC) FROM TBLWORKFLOWOBJECTS B WHERE B.WO_INITIAL_ID = A.WO_INITIAL_ID)) AS CREATOR,CR_ON,STATUS,WO_APPROVE_STATUS,RO_NAME,NEXT_ROLE,CURRENT_STATUS,WO_DESCRIPTION,WOA_ID, WO_WFO_ID,WO_INITIAL_ID FROM (SELECT WO_ID,WO_RECORD_ID,WO_BO_ID,BO_NAME,(SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CR_BY=US_ID) USER_NAME,TO_CHAR(WO_CR_ON,'DD-MON-YYYY') CR_ON,";
        //        strQry += " DECODE( WO_APPROVE_STATUS,'0','PENDING','1','APPROVED','2','MODIFY AND APPROVE','3','REJECTED','OTHERS') STATUS,WO_APPROVE_STATUS ,";
        //        strQry += " (SELECT RO_NAME FROM TBLROLES WHERE WO_NEXT_ROLE=RO_ID) RO_NAME,";
        //        strQry += " (SELECT WO_NEXT_ROLE FROM TBLWORKFLOWOBJECTS WHERE WO_ID=(SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID= A.WO_RECORD_ID )) NEXT_ROLE,";
        //        strQry += " CASE (SELECT WO_NEXT_ROLE FROM TBLWORKFLOWOBJECTS WHERE WO_ID=(SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID= A.WO_RECORD_ID ))";
        //        strQry += " WHEN 0 THEN 'APPROVED' ELSE 'PENDING' END CURRENT_STATUS,WO_DESCRIPTION,0 AS WOA_ID,";
        //        //strQry += " (SELECT MAX(WO_WFO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%' AND WO_RECORD_ID=A.WO_RECORD_ID) ";
        //        strQry += " WO_WFO_ID,WO_INITIAL_ID ";
        //        strQry += " FROM TBLWORKFLOWOBJECTS A,TBLBUSINESSOBJECT B WHERE WO_BO_ID=BO_ID  AND WO_NEXT_ROLE='" + objApproval.sRoleId + "' ";
        //        strQry += "  AND WO_APPROVE_STATUS <> 0 AND WO_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%'";


        //        if (objApproval.sBOId != null)
        //        {
        //            strQry += " AND WO_BO_ID='" + objApproval.sBOId + "' ";
        //        }
        //        if (objApproval.sCrby != null)
        //        {
        //            strQry += " AND WO_CR_BY='" + objApproval.sCrby + "' ";
        //        }
        //        if (objApproval.sFromDate != "")
        //        {
        //            DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')>='" + dFromDate.ToString("yyyyMMdd") + "'";

        //        }
        //        if (objApproval.sToDate != "")
        //        {
        //            DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')<='" + dToDate.ToString("yyyyMMdd") + "'";
        //        }
        //        if (objApproval.sFormName != "")
        //        {
        //            strQry += " AND UPPER(BO_NAME) LIKE '" + objApproval.sFormName.ToUpper() + "%' ";
        //        }
        //        if (objApproval.sDescription != "")
        //        {
        //            strQry += " AND UPPER(WO_DESCRIPTION) LIKE '%" + objApproval.sDescription.ToUpper() + "%' ";
        //        }

        //        strQry += " UNION ALL  ";
        //        strQry += " SELECT WO_ID,WO_RECORD_ID,(SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID) AS WO_BO_ID,";
        //        strQry += " BO_NAME,'INITIATOR' AS USER_NAME,TO_CHAR(WOA_CRON,'DD-MON-YYYY')CR_ON,";
        //        strQry += " 'INITIATED' STATUS,WO_APPROVE_STATUS,";
        //        strQry += "  (SELECT RO_NAME FROM TBLROLES WHERE WOA_ROLE_ID=RO_ID) RO_NAME, ";
        //        strQry += " (SELECT WO_NEXT_ROLE FROM TBLWORKFLOWOBJECTS WHERE WO_ID=(SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE ";
        //        strQry += " WO_RECORD_ID= A.WO_RECORD_ID )) NEXT_ROLE,CASE (SELECT WO_NEXT_ROLE FROM TBLWORKFLOWOBJECTS WHERE WO_ID=(SELECT MAX(WO_ID) ";
        //        strQry += " FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID= A.WO_RECORD_ID )) WHEN 0 THEN 'APPROVED' ELSE 'PENDING' END CURRENT_STATUS,WOA_DESCRIPTION ";
        //        strQry += " AS WO_DESCRIPTION,0 AS WOA_ID,WO_WFO_ID,WO_INITIAL_ID  FROM TBLWO_OBJECT_AUTO,TBLBUSINESSOBJECT,TBLWORKFLOWOBJECTS A ";
        //        strQry += " WHERE  (SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID) = BO_ID AND  WOA_INITIAL_ACTION_ID IS NOT NULL ";
        //        strQry += " AND WOA_INITIAL_ACTION_ID=WO_ID AND WOA_ROLE_ID='" + objApproval.sRoleId + "' AND WOA_REF_OFFCODE LIKE '" + objApproval.sOfficeCode + "%' ";

        //        if (objApproval.sBOId != null)
        //        {
        //            strQry += " AND (SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID)='" + objApproval.sBOId + "' ";
        //        }
        //        if (objApproval.sCrby != null)
        //        {
        //            strQry += " AND WO_CR_BY='" + objApproval.sCrby + "' ";
        //        }
        //        if (objApproval.sFromDate != "")
        //        {
        //            DateTime dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')>='" + dFromDate.ToString("yyyyMMdd") + "'";

        //        }
        //        if (objApproval.sToDate != "")
        //        {
        //            DateTime dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')<='" + dToDate.ToString("yyyyMMdd") + "'";
        //        }

        //        if (objApproval.sFormName != "")
        //        {
        //            strQry += " AND UPPER(BO_NAME) LIKE '" + objApproval.sFormName.ToUpper() + "%' ";
        //        }
        //        if (objApproval.sDescription != "")
        //        {
        //            strQry += " AND UPPER(WOA_DESCRIPTION) LIKE '%" + objApproval.sDescription.ToUpper() + "%' ";
        //        }

        //        strQry += " ORDER BY WO_ID DESC)A WHERE ROWNUM<500";
        //        dt = objcon.getDataTable(strQry);
        //        
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAlreadyApprovedInbox");
        //        
        //        return dt;
        //    }
        //}
        #endregion

        #region New LoadAlreadyApprovedInbox Query

        public DataTable LoadAlreadyApprovedInbox(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            DataTable dt = new DataTable();
            try
            {
                DateTime dToDate = new DateTime();
                DateTime dFromDate = new DateTime();
                string strQry = string.Empty;
                strQry = " SELECT WO_ID, WO_RECORD_ID, WO_BO_ID, BO_NAME, USER_NAME, US_FULL_NAME AS CREATOR, CR_ON, STATUS, WO_APPROVE_STATUS, RO_NAME, ";
                strQry += " NEXT_ROLE, CURRENT_STATUS, WO_DESCRIPTION, WOA_ID, WO_WFO_ID, WO_INITIAL_ID FROM (SELECT WO_ID, WO_RECORD_ID, WO_BO_ID, BO_NAME, ";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CR_BY=US_ID) USER_NAME, TO_CHAR(WO_CR_ON, 'DD-MON-YYYY') CR_ON, DECODE";
                strQry += " ( WO_APPROVE_STATUS, '0', 'PENDING', '1', 'APPROVED', '2', 'MODIFY AND APPROVE', '3', 'REJECTED', 'OTHERS') STATUS, ";
                strQry += " WO_APPROVE_STATUS , (SELECT RO_NAME FROM TBLROLES WHERE WO_NEXT_ROLE=RO_ID) RO_NAME, C.NEXTROLE NEXT_ROLE, CASE C.NEXTROLE WHEN 0 ";
                strQry += " THEN 'APPROVED' ELSE 'PENDING' END CURRENT_STATUS, WO_DESCRIPTION, 0 AS WOA_ID, WO_WFO_ID, WO_INITIAL_ID FROM TBLWORKFLOWOBJECTS A, ";
                strQry += " TBLBUSINESSOBJECT B, VIEW_NEXTROLE C WHERE WO_RECORD_ID= C.RECORD_ID AND  WO_BO_ID=BO_ID AND WO_NEXT_ROLE=:RoleId ";
                strQry += "  AND WO_APPROVE_STATUS <> 0 AND WO_REF_OFFCODE LIKE:OfficeCode||'%'";

                oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                oledbCommand.Parameters.AddWithValue("OfficeCode", objApproval.sOfficeCode);
                if (objApproval.sBOId != null)
                {
                    strQry += " AND WO_BO_ID=:BOId ";
                    oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                }
                if (objApproval.sCrby != null)
                {
                    strQry += " AND WO_CR_BY=:Crby ";
                    oledbCommand.Parameters.AddWithValue("Crby", objApproval.sCrby);
                }
                if (objApproval.sFromDate != "")
                {
                    dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')>=:FromDate";
                    oledbCommand.Parameters.AddWithValue("FromDate", dFromDate.ToString("yyyyMMdd"));

                }
                if (objApproval.sToDate != "")
                {
                    dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')<=:ToDate";
                    oledbCommand.Parameters.AddWithValue("ToDate", dToDate.ToString("yyyyMMdd"));
                }
                if (objApproval.sFormName != "")
                {
                    strQry += " AND UPPER(BO_NAME) LIKE:FormName||'%' ";
                    oledbCommand.Parameters.AddWithValue("FormName", objApproval.sFormName.ToUpper());
                }
                if (objApproval.sDescription != "")
                {
                    strQry += " AND UPPER(WO_DESCRIPTION) LIKE '%'|| :Description ||'%' ";
                    oledbCommand.Parameters.AddWithValue("Description", objApproval.sDescription.ToUpper());
                }

                strQry += " UNION ALL  ";
                strQry += " SELECT WO_ID, WO_RECORD_ID, (SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID) AS WO_BO_ID, BO_NAME, ";
                strQry += " 'INITIATOR' AS USER_NAME, TO_CHAR(WOA_CRON, 'DD-MON-YYYY')CR_ON, 'INITIATED' STATUS, WO_APPROVE_STATUS, (SELECT RO_NAME FROM ";
                strQry += " TBLROLES WHERE WOA_ROLE_ID=RO_ID) RO_NAME, B.NEXTROLE NEXT_ROLE, CASE B.NEXTROLE WHEN 0 THEN 'APPROVED' ELSE 'PENDING' END ";
                strQry += " CURRENT_STATUS, WOA_DESCRIPTION AS WO_DESCRIPTION, 0 AS WOA_ID, WO_WFO_ID, WO_INITIAL_ID FROM TBLWO_OBJECT_AUTO, TBLBUSINESSOBJECT, ";
                strQry += " TBLWORKFLOWOBJECTS A, VIEW_NEXTROLE B WHERE A.WO_RECORD_ID= B.RECORD_ID AND (SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE ";
                strQry += " BFM_ID =WOA_BFM_ID) = BO_ID AND WOA_INITIAL_ACTION_ID IS NOT NULL AND WOA_INITIAL_ACTION_ID=WO_ID AND ";
                strQry += " WOA_ROLE_ID =:RoleId1 AND WOA_REF_OFFCODE LIKE :OfficeCode1||'%' ";


                oledbCommand.Parameters.AddWithValue("RoleId1", objApproval.sRoleId);
                oledbCommand.Parameters.AddWithValue("OfficeCode1", objApproval.sOfficeCode);
                if (objApproval.sBOId != null)
                {
                    strQry += " AND (SELECT BFM_NEXT_BO_ID FROM TBLBO_FLOW_MASTER WHERE BFM_ID=WOA_BFM_ID)=:BOIds";
                    oledbCommand.Parameters.AddWithValue("BOIds", objApproval.sBOId);
                }
                if (objApproval.sCrby != null)
                {
                    strQry += " AND WO_CR_BY=:Crbys";
                    oledbCommand.Parameters.AddWithValue("Crbys", objApproval.sCrby);
                }
                if (objApproval.sFromDate != "")
                {
                    dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')>=:FromDates";
                    oledbCommand.Parameters.AddWithValue("FromDates", dFromDate.ToString("yyyyMMdd"));

                }
                if (objApproval.sToDate != "")
                {
                    dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')<=:ToDates";
                    oledbCommand.Parameters.AddWithValue("ToDates", dToDate.ToString("yyyyMMdd"));
                }

                if (objApproval.sFormName != "")
                {
                    strQry += " AND UPPER(BO_NAME) LIKE:FormNames||'%' ";
                    oledbCommand.Parameters.AddWithValue("FormNames", objApproval.sFormName.ToUpper());
                }
                if (objApproval.sDescription != "")
                {
                    strQry += " AND UPPER(WOA_DESCRIPTION) LIKE '%'|| :Descriptions ||'%' ";
                    oledbCommand.Parameters.AddWithValue("Descriptions", objApproval.sDescription.ToUpper());
                }

                strQry += " ORDER BY WO_ID DESC)Z, VIEW_INITIATOR, TBLUSER WHERE WO_INITIAL_ID = INITIAL_ID ";
                strQry += " AND US_ID = CRBY ORDER BY WO_ID DESC  FETCH FIRST 500 ROWS ONLY";



                //oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                //oledbCommand.Parameters.AddWithValue("Crby", objApproval.sCrby);
                //oledbCommand.Parameters.AddWithValue("FromDate", dFromDate.ToString("yyyyMMdd"));
                //oledbCommand.Parameters.AddWithValue("ToDate", dToDate.ToString("yyyyMMdd"));
                //oledbCommand.Parameters.AddWithValue("FormName", objApproval.sFormName.ToUpper());
                //oledbCommand.Parameters.AddWithValue("Description", objApproval.sDescription.ToUpper());



                dt = objcon.getDataTable(strQry, oledbCommand);

                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAlreadyApprovedInbox");

                return dt;
            }
        }
        #endregion



        /// <summary>
        /// Load Approval Inbox with Already Approved Details
        /// </summary>
        /// <param name="objApproval">Office Code,Role Id</param>
        /// <returns></returns>
        public DataTable LoadRejectedApprovedInbox(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();

            DataTable dt = new DataTable();
            try
            {
                DateTime dToDate = new DateTime();
                DateTime dFromDate = new DateTime();
                string strQry = string.Empty;
                strQry = " SELECT * FROM (SELECT WO_ID,WO_RECORD_ID,WO_BO_ID,BO_NAME,(SELECT US_FULL_NAME FROM TBLUSER WHERE WO_CR_BY=US_ID) USER_NAME,TO_CHAR(WO_CR_ON,'DD-MON-YYYY') CR_ON,(SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID=(SELECT max(B.WO_CR_BY) KEEP (DENSE_RANK LAST ORDER BY B.WO_ID DESC) FROM TBLWORKFLOWOBJECTS B WHERE B.WO_INITIAL_ID = A.WO_INITIAL_ID)) AS CREATOR,";
                strQry += " DECODE( WO_APPROVE_STATUS,'0','PENDING','1','APPROVED','2','MODIFY AND APPROVE','3','REJECTED','OTHERS') STATUS,WO_APPROVE_STATUS ,";
                strQry += " (SELECT RO_NAME FROM TBLROLES WHERE WO_NEXT_ROLE=RO_ID) RO_NAME,";
                strQry += " (SELECT WO_NEXT_ROLE FROM TBLWORKFLOWOBJECTS WHERE WO_ID=(SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID= A.WO_RECORD_ID )) NEXT_ROLE,";
                strQry += " CASE (SELECT WO_NEXT_ROLE FROM TBLWORKFLOWOBJECTS WHERE WO_ID=(SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID= A.WO_RECORD_ID ))";
                strQry += " WHEN 0 THEN 'APPROVED' ELSE 'PENDING' END CURRENT_STATUS,WO_DESCRIPTION,0 AS WOA_ID,";
                strQry += " WO_WFO_ID,WO_INITIAL_ID ";
                strQry += " FROM TBLWORKFLOWOBJECTS A,TBLBUSINESSOBJECT B WHERE WO_BO_ID=BO_ID  AND WO_NEXT_ROLE=:RoleId";
                strQry += "  AND WO_APPROVE_STATUS ='3' AND WO_REF_OFFCODE LIKE:sOfficeCode||'%'";

                oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                oledbCommand.Parameters.AddWithValue("OfficeCode", objApproval.sOfficeCode);
                if (objApproval.sBOId != null)
                {
                    strQry += " AND WO_BO_ID=:BOId ";
                    oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                }
                if (objApproval.sCrby != null)
                {
                    strQry += " AND WO_CR_BY=:Crby ";
                    oledbCommand.Parameters.AddWithValue("Crby", objApproval.sCrby);
                }
                if (objApproval.sFromDate != "")
                {
                    dFromDate = DateTime.ParseExact(objApproval.sFromDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')>=:FromDate";
                    oledbCommand.Parameters.AddWithValue("FromDate", dFromDate.ToString("yyyyMMdd"));

                }
                if (objApproval.sToDate != "")
                {
                    dToDate = DateTime.ParseExact(objApproval.sToDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strQry += " AND TO_CHAR(WO_CR_ON,'YYYYMMDD')<=:ToDate";
                    oledbCommand.Parameters.AddWithValue("ToDate", dToDate.ToString("yyyyMMdd"));
                }
                if (objApproval.sFormName != "")
                {
                    strQry += " AND UPPER(BO_NAME) LIKE:FormName||'%' ";
                    oledbCommand.Parameters.AddWithValue("FormName", objApproval.sFormName.ToUpper());
                }
                if (objApproval.sDescription != "")
                {
                    strQry += " AND UPPER(WO_DESCRIPTION) LIKE: '%'|| Description||'%' ";
                    oledbCommand.Parameters.AddWithValue("Description", objApproval.sDescription.ToUpper());
                }

                strQry += " ORDER BY WO_ID DESC) WHERE ROWNUM<500";

                //oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                //oledbCommand.Parameters.AddWithValue("OfficeCode", objApproval.sOfficeCode);
                //oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                //oledbCommand.Parameters.AddWithValue("Crby", objApproval.sCrby);
                //oledbCommand.Parameters.AddWithValue("FromDate", dFromDate.ToString("yyyyMMdd"));
                //oledbCommand.Parameters.AddWithValue("ToDate", dToDate.ToString("yyyyMMdd"));
                //oledbCommand.Parameters.AddWithValue("FormName", objApproval.sFormName.ToUpper());
                //oledbCommand.Parameters.AddWithValue("Description", objApproval.sDescription.ToUpper());

                return objcon.getDataTable(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
        }

        /// <summary>
        /// Save Workflow Data to TBLWFODATA table like QueryValues,ParameterValues and XML format
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool SaveWorkFlowData(clsApproval objApproval)
        {

            try
            {
                string strQry = string.Empty;
                string sResult = string.Empty;
                if (objApproval.sTableNames != "" && objApproval.sTableNames != null)
                {
                    sResult = CreateXml(objApproval.sColumnNames, objApproval.sColumnValues, objApproval.sTableNames);
                    sResult = sResult.Replace("'", "''");
                }


                objApproval.sWFDataId = Convert.ToString(objcon.Get_max_no("WFO_ID", "TBLWFODATA"));
                strQry = "INSERT INTO TBLWFODATA (WFO_ID,WFO_QUERY_VALUES,WFO_PARAMETER,WFO_DATA,WFO_CR_BY) VALUES (";
                strQry += " '" + objApproval.sWFDataId + "','" + objApproval.sQryValues + "','" + objApproval.sParameterValues + "','" + sResult + "',";
                strQry += " '" + objApproval.sCrby + "')";
                objcon.Execute(strQry);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool SaveWorkFlowDatalatest(clsApproval objApproval, CustOledbConnection objconn)
        {

            try
            {
                string strQry = string.Empty;
                string sResult = string.Empty;
                if (objApproval.sTableNames != "" && objApproval.sTableNames != null)
                {
                    sResult = CreateXml(objApproval.sColumnNames, objApproval.sColumnValues, objApproval.sTableNames);
                    sResult = sResult.Replace("'", "''");
                }

                int Nooftimes = 0;
                Loop:
                objApproval.sWFDataId = Convert.ToString(objconn.Get_max_no("WFO_ID", "TBLWFODATA"));
                strQry = "INSERT INTO TBLWFODATA (WFO_ID,WFO_QUERY_VALUES,WFO_PARAMETER,WFO_DATA,WFO_CR_BY) VALUES (";
                strQry += " '" + objApproval.sWFDataId + "','" + objApproval.sQryValues + "','" + objApproval.sParameterValues + "','" + sResult + "',";
                strQry += " '" + objApproval.sCrby + "')";

                try
                {
                    objconn.Execute(strQry);
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(300);
                    Nooftimes++;
                    if (Nooftimes <= 3)
                    {
                        goto Loop;
                    }
                    else
                    {
                        throw ex;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }


     

        public bool SaveWorkFlowData1(clsApproval objApproval, CustOledbConnection objconn)
        {
            string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");

            if (!Directory.Exists(sFolderPath))
            {
                Directory.CreateDirectory(sFolderPath);

            }
            string sFileName = "Begintrans";
            string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            try
            {
                File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , SaveWorkFlowData1" + Environment.NewLine);

                string strQry = string.Empty;
                string sResult = string.Empty;
                if (objApproval.sTableNames != "" && objApproval.sTableNames != null)
                {
                    sResult = CreateXml(objApproval.sColumnNames, objApproval.sColumnValues, objApproval.sTableNames);
                    sResult = sResult.Replace("'", "''");
                }


                objApproval.sWFDataId = Convert.ToString(objconn.Get_max_no("WFO_ID", "TBLWFODATA"));
                strQry = "INSERT INTO TBLWFODATA (WFO_ID,WFO_QUERY_VALUES,WFO_PARAMETER,WFO_DATA,WFO_CR_BY) VALUES (";
                strQry += " '" + objApproval.sWFDataId + "','" + objApproval.sQryValues + "','" + objApproval.sParameterValues + "','" + sResult + "',";
                strQry += " '" + objApproval.sCrby + "')";
                objconn.Execute(strQry);
                File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , After execute Query" + Environment.NewLine);

                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        /// Generate Temporary Record Id to Insert in TBLWORKFLOWOBJECTS Table. i.e -1,-2 etc
        /// </summary>
        /// <returns></returns>
        public string GetRecordIdForWorkFlow()
        {

            try
            {
                string strQry = string.Empty;


                strQry = " SELECT  NVL(MIN(WO_RECORD_ID),0)-1 FROM TBLWORKFLOWOBJECTS";
                string sResult = objcon.get_value(strQry);
                if (Convert.ToInt32(sResult) >= 0)
                {
                    sResult = "-1";
                }

                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }

        public string GetRecordIdForWorkFlowlatest(CustOledbConnection objconn)
        {

            try
            {
                string strQry = string.Empty;

                strQry = " SELECT  NVL(MIN(WO_RECORD_ID),0)-1 FROM TBLWORKFLOWOBJECTS";
                string sResult = objconn.get_value(strQry);
                if (Convert.ToInt32(sResult) >= 0)
                {
                    sResult = "-1";
                }

                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }

        public string GetRecordIdForWorkFlow1(CustOledbConnection objconn)
        {
            string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");

            if (!Directory.Exists(sFolderPath))
            {
                Directory.CreateDirectory(sFolderPath);

            }
            string sFileName = "Begintrans";
            string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            try
            {
                string strQry = string.Empty;


                strQry = " SELECT  NVL(MIN(WO_RECORD_ID),0)-1 FROM TBLWORKFLOWOBJECTS";
                string sResult = objconn.get_value(strQry);
                if (Convert.ToInt32(sResult) >= 0)
                {
                    sResult = "-1";
                }
                File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss")
                    + " , GetRecordIdForWorkFlow1" + Environment.NewLine);

                return sResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }


        /// <summary>
        /// Save WorkFlow object like Who is Next Role to access the Bussiness Object.
        /// TBLWORKFLOWOBJECTS Table Contains  Transaction of WorkFlow based on Businens Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool SaveWorkflowObjects(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            string res = string.Empty;
            string sWFlowId = string.Empty;
            int start_point, end_point;
            try
            {
                string strQry = string.Empty;
                string sApproveResult = string.Empty;

                if (Convert.ToString(ConfigurationManager.AppSettings["Approval"]).ToUpper().Equals("OFF"))
                {
                    return false;
                }

                if (objApproval.sFormName != null && objApproval.sFormName != "")
                {
                    //To get Business Object Id

                    string qry = "SELECT BO_ID FROM TBLBUSINESSOBJECT WHERE UPPER(BO_FORMNAME)=:FormName";
                    oledbCommand.Parameters.AddWithValue("FormName", objApproval.sFormName.Trim().ToUpper());
                    objApproval.sBOId = objcon.get_value(qry, oledbCommand);
                }

                //Check Approval Exists
                bool bResult = CheckFormApprovalExists(objApproval);

                if (bResult == true)
                {
                    //To get Role Id with Priority Level                 
                    objApproval.sRoleId = GetRoleFromApprovePriority(objApproval);
                }
                else
                {
                    objApproval.sRoleId = "";

                }

                if (objApproval.sPrevApproveId == null)
                {
                    objApproval.sPrevApproveId = "0";
                }

                if (objApproval.sRoleId != "")
                {

                    // SaveWorkFlowData(objApproval);
                    objApproval.sPrevWFOId = objApproval.sWFObjectId;

                    int NoofTimes = 0;
                    Loop:
                    sWFlowId = Convert.ToString(objcon.Get_max_no("WO_ID", "TBLWORKFLOWOBJECTS"));

                    objApproval.sWFObjectId = sWFlowId;
                    strQry = "INSERT INTO TBLWORKFLOWOBJECTS (WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_NEXT_ROLE,WO_OFFICE_CODE,";
                    strQry += " WO_CLIENT_IP,WO_CR_BY,WO_DESCRIPTION,WO_WFO_ID,WO_DATA_ID,WO_REF_OFFCODE)";
                    strQry += " VALUES ('" + sWFlowId + "','" + objApproval.sBOId + "','" + objApproval.sRecordId + "','" + objApproval.sPrevApproveId + "',";
                    strQry += " '" + objApproval.sRoleId + "','" + objApproval.sOfficeCode + "','" + objApproval.sClientIp + "','" + objApproval.sCrby + "',";
                    strQry += " '" + objApproval.sDescription + "','" + objApproval.sWFDataId + "','" + objApproval.sDataReferenceId + "','" + objApproval.sRefOfficeCode + "')";

                    try
                    {
                        objcon.Execute(strQry);
                    }
                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(250);
                        NoofTimes++;
                        if (NoofTimes <= 5)
                        {
                            goto Loop;
                        }
                        else
                        {
                            throw ex;
                        }
                    }

                    //Insert Initial Id for WorkFlow
                    if (objApproval.sWFInitialId == "" || objApproval.sWFInitialId == null)
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + sWFlowId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objcon.Execute(strQry);
                    }
                    else
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + objApproval.sWFInitialId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objcon.Execute(strQry);
                    }

                    UpdateWFOAutoObject(objApproval);

                    //Saving SMS in Table to send to Next Role
                    SendSMStoRole(objApproval, "");
                    return true;
                }
                else
                {

                    if (objApproval.sPrevApproveId == "0" && objApproval.sRoleId == "")
                    {
                        objApproval.sWFDataId = "";
                        objApproval.sWFInitialId = "";
                    }


                    int NoofTimes = 0;
                    Loop:
                    sWFlowId = Convert.ToString(objcon.Get_max_no("WO_ID", "TBLWORKFLOWOBJECTS"));

                    strQry = "INSERT INTO TBLWORKFLOWOBJECTS (WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_NEXT_ROLE,WO_OFFICE_CODE,WO_CLIENT_IP,";
                    strQry += " WO_CR_BY,WO_APPROVE_STATUS,WO_DESCRIPTION,WO_WFO_ID,WO_DATA_ID,WO_REF_OFFCODE)";
                    strQry += " VALUES ('" + sWFlowId + "','" + objApproval.sBOId + "','" + objApproval.sRecordId + "','" + objApproval.sPrevApproveId + "',";
                    strQry += " '0','" + objApproval.sOfficeCode + "','" + objApproval.sClientIp + "','" + objApproval.sCrby + "',";
                    strQry += " '1','" + objApproval.sDescription + "','" + objApproval.sWFDataId + "','" + objApproval.sDataReferenceId + "','" + objApproval.sRefOfficeCode + "')";

                    try
                    {
                        objcon.Execute(strQry);
                    }
                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(250);
                        NoofTimes++;
                        if (NoofTimes <= 5)
                        {
                            goto Loop;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    //Insert Initial Id for WorkFlow
                    if (objApproval.sWFInitialId == "" || objApproval.sWFInitialId == null)
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + sWFlowId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objcon.Execute(strQry);
                    }
                    else
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + objApproval.sWFInitialId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objcon.Execute(strQry);
                    }

                    UpdateToMainTable(objApproval);
                    oledbCommand = new OleDbCommand();
                    if (objApproval.sBOId == "30")
                    {
                        strQry = "SELECT WFO_QUERY_VALUES FROM TBLWORKFLOWOBJECTS,TBLWFODATA WHERE WO_WFO_ID=WFO_ID AND WO_BO_ID='30' AND WO_ID=:WFObjectId";
                        oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                        string query = objcon.get_value(strQry, oledbCommand);

                        DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        bool result = objWCF.SaveTcDetails(query);
                        oledbCommand = new OleDbCommand();
                        if (result == false)
                        {
                            strQry = "SELECT WO_INITIAL_ID FROM TBLWORKFLOWOBJECTS,TBLWFODATA WHERE WO_WFO_ID=WFO_ID AND WO_BO_ID='30' AND WO_ID=:WFObjectId";
                            oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                            string sWO_ID = objcon.get_value(strQry, oledbCommand);

                            oledbCommand = new OleDbCommand();
                            start_point = query.IndexOf("(");
                            end_point = query.IndexOf(")");
                            strQry = "SELECT substr(WO_WFO_ID,'" + start_point + "','" + end_point + "' )WO_WFO_ID FROM TBLWORKFLOWOBJECTS,TBLWFODATA WHERE WO_WFO_ID=WFO_ID AND WO_BO_ID='30' AND WO_ID=:WFObjectId1";
                            oledbCommand.Parameters.AddWithValue("WFObjectId1", objApproval.sWFObjectId);
                            string query1 = objcon.get_value(strQry, oledbCommand);
                            strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_UPDATED_EVENT='' WHERE TC_CODE IN " + query1 + "";
                            objcon.Execute(strQry);
                            strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + sWFlowId + "'";
                            objcon.Execute(strQry);
                            strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_APPROVE_STATUS='0' WHERE WO_ID='" + sWO_ID + "'";
                            objcon.Execute(strQry);
                        }

                    }


                    string sPrevBoId = objApproval.sBOId;
                    string sResult = GetNextBOId(objApproval.sBOId);
                    objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    objApproval.sWFObjectId = sWFlowId;

                    if (sResult != "")
                    {
                        objApproval.sBOFlowMasterId = sResult.Split('~').GetValue(1).ToString();
                        objApproval.sBOId = sResult.Split('~').GetValue(0).ToString();

                        //To get Role Id with Priority Level to Create Form
                        objApproval.sRoleId = GetRoleFromApprovePriorityForBOCreate(objApproval);

                        bool resrult = SaveWFObjectAuto(objApproval);
                        if (resrult == false)
                        {
                            return false;
                        }
                    }

                    UpdateWFOAutoObject(objApproval);

                    //Saving SMS in Table to send to Next Role
                    SendSMStoRole(objApproval, sPrevBoId);

                    return true;

                }

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool SaveWorkflowObjectslatest(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();
            string res = string.Empty;
            string sWFlowId = string.Empty;
            int start_point, end_point;
            try
            {
                string strQry = string.Empty;
                string sApproveResult = string.Empty;

                if (Convert.ToString(ConfigurationManager.AppSettings["Approval"]).ToUpper().Equals("OFF"))
                {
                    return false;
                }

                if (objApproval.sFormName != null && objApproval.sFormName != "")
                {
                    //To get Business Object Id

                    string qry = "SELECT BO_ID FROM TBLBUSINESSOBJECT WHERE UPPER(BO_FORMNAME)='" + objApproval.sFormName.Trim().ToUpper() + "'";
                    // oledbCommand.Parameters.AddWithValue("FormName", objApproval.sFormName.Trim().ToUpper());
                    objApproval.sBOId = objconn.get_value(qry);
                }

                //Check Approval Exists
                bool bResult = CheckFormApprovalExistslatest(objApproval, objconn);

                if (bResult == true)
                {
                    //To get Role Id with Priority Level                 
                    objApproval.sRoleId = GetRoleFromApprovePrioritylatest(objApproval, objconn);
                }
                else
                {
                    objApproval.sRoleId = "";

                }

                if (objApproval.sPrevApproveId == null)
                {
                    objApproval.sPrevApproveId = "0";
                }

                if (objApproval.sRoleId != "")
                {

                    // SaveWorkFlowData(objApproval);
                    objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    sWFlowId = Convert.ToString(objconn.Get_max_no("WO_ID", "TBLWORKFLOWOBJECTS"));

                    objApproval.sWFObjectId = sWFlowId;
                    strQry = "INSERT INTO TBLWORKFLOWOBJECTS (WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_NEXT_ROLE,WO_OFFICE_CODE,";
                    strQry += " WO_CLIENT_IP,WO_CR_BY,WO_DESCRIPTION,WO_WFO_ID,WO_DATA_ID,WO_REF_OFFCODE)";
                    strQry += " VALUES ('" + sWFlowId + "','" + objApproval.sBOId + "','" + objApproval.sRecordId + "','" + objApproval.sPrevApproveId + "',";
                    strQry += " '" + objApproval.sRoleId + "','" + objApproval.sOfficeCode + "','" + objApproval.sClientIp + "','" + objApproval.sCrby + "',";
                    strQry += " '" + objApproval.sDescription + "','" + objApproval.sWFDataId + "','" + objApproval.sDataReferenceId + "','" + objApproval.sRefOfficeCode + "')";
                    objconn.Execute(strQry);
             
                    //Insert Initial Id for WorkFlow
                    if (objApproval.sWFInitialId == "" || objApproval.sWFInitialId == null)
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + sWFlowId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objconn.Execute(strQry);
                    }
                    else
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + objApproval.sWFInitialId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objconn.Execute(strQry);
                    }

                    UpdateWFOAutoObjectlatest(objApproval, objconn);
                    objconn.CommitTrans();
                    //Saving SMS in Table to send to Next Role
                    SendSMStoRole(objApproval, "");
                    return true;
                }
                else
                {

                    bool bApproveResult = objApproval.CheckDuplicateAtFinalApprove(objApproval, objconn);
                    if (bApproveResult == false)
                    {
                        return false;
                    }
                    if (objApproval.sPrevApproveId == "0" && objApproval.sRoleId == "")
                    {
                        objApproval.sWFDataId = "";
                        objApproval.sWFInitialId = "";
                    }

                    int NoofTimes = 0;
                    Loop:

                    sWFlowId = Convert.ToString(objconn.Get_max_no("WO_ID", "TBLWORKFLOWOBJECTS"));

                    strQry = "INSERT INTO TBLWORKFLOWOBJECTS (WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_NEXT_ROLE,WO_OFFICE_CODE,WO_CLIENT_IP,";
                    strQry += " WO_CR_BY,WO_APPROVE_STATUS,WO_DESCRIPTION,WO_WFO_ID,WO_DATA_ID,WO_REF_OFFCODE)";
                    strQry += " VALUES ('" + sWFlowId + "','" + objApproval.sBOId + "','" + objApproval.sRecordId + "','" + objApproval.sPrevApproveId + "',";
                    strQry += " '0','" + objApproval.sOfficeCode + "','" + objApproval.sClientIp + "','" + objApproval.sCrby + "',";
                    strQry += " '1','" + objApproval.sDescription + "','" + objApproval.sWFDataId + "','" + objApproval.sDataReferenceId + "','" + objApproval.sRefOfficeCode + "')";
                    try
                    {
                        objconn.Execute(strQry);
                    }
                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(500);
                        NoofTimes++;
                        if (NoofTimes <= 5)
                        {
                            goto Loop;
                        }
                        else
                        {
                            throw ex;
                        }
                    }



                    //Insert Initial Id for WorkFlow
                    if (objApproval.sWFInitialId == "" || objApproval.sWFInitialId == null)
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + sWFlowId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objconn.Execute(strQry);
                    }
                    else
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + objApproval.sWFInitialId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objconn.Execute(strQry);
                    }

                    UpdateToMainTablelatest(objApproval, objconn);
                    // objconn.CommitTrans();

                    oledbCommand = new OleDbCommand();
                    if (objApproval.sBOId == "30")
                    {
                        strQry = "SELECT WFO_QUERY_VALUES FROM TBLWORKFLOWOBJECTS,TBLWFODATA WHERE WO_WFO_ID=WFO_ID AND WO_BO_ID='30' AND WO_ID=:WFObjectId";
                        oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                        string query = objcon.get_value(strQry, oledbCommand);

                        DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        bool result = objWCF.SaveTcDetails(query);
                        oledbCommand = new OleDbCommand();
                        if (result == false)
                        {
                            strQry = "SELECT WO_INITIAL_ID FROM TBLWORKFLOWOBJECTS,TBLWFODATA WHERE WO_WFO_ID=WFO_ID AND WO_BO_ID='30' AND WO_ID=:WFObjectId";
                            oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                            string sWO_ID = objcon.get_value(strQry, oledbCommand);

                            oledbCommand = new OleDbCommand();
                            start_point = query.IndexOf("(");
                            end_point = query.IndexOf(")");
                            strQry = "SELECT substr(WO_WFO_ID,'" + start_point + "','" + end_point + "' )WO_WFO_ID FROM TBLWORKFLOWOBJECTS,TBLWFODATA WHERE WO_WFO_ID=WFO_ID AND WO_BO_ID='30' AND WO_ID=:WFObjectId1";
                            oledbCommand.Parameters.AddWithValue("WFObjectId1", objApproval.sWFObjectId);
                            string query1 = objcon.get_value(strQry, oledbCommand);
                            strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_UPDATED_EVENT='' WHERE TC_CODE IN " + query1 + "";
                            objcon.Execute(strQry);
                            strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + sWFlowId + "'";
                            objcon.Execute(strQry);
                            strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_APPROVE_STATUS='0' WHERE WO_ID='" + sWO_ID + "'";
                            objcon.Execute(strQry);
                        }

                    }

                    objconn.CommitTrans();
                    string sPrevBoId = objApproval.sBOId;
                    string sResult = GetNextBOId(objApproval.sBOId);
                    objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    objApproval.sWFObjectId = sWFlowId;

                    if (sResult != "")
                    {
                        objApproval.sBOFlowMasterId = sResult.Split('~').GetValue(1).ToString();
                        objApproval.sBOId = sResult.Split('~').GetValue(0).ToString();

                        //To get Role Id with Priority Level to Create Form
                        objApproval.sRoleId = GetRoleFromApprovePriorityForBOCreate(objApproval);

                        //bool resrult = SaveWFObjectAuto(objApproval);
                        bool resrult = SaveWFObjectAuto(objApproval);

                        if (resrult == false)
                        {
                            return false;
                        }
                    }

                    UpdateWFOAutoObject(objApproval);

                    //Saving SMS in Table to send to Next Role
                    SendSMStoRole(objApproval, sPrevBoId);

                    return true;

                }

            }
            catch (Exception ex)
            {
                objconn.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                //return false;
                throw ex;
            }
        }
        public bool SaveWorkflowObjects1(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();
            string res = string.Empty;
            string sWFlowId = string.Empty;
            int start_point, end_point;

            string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");

            if (!Directory.Exists(sFolderPath))
            {
                Directory.CreateDirectory(sFolderPath);

            }
            string sFileName = "Begintrans";
            string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            try
            {
                File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , SaveWorkflowObjects1" + Environment.NewLine);

                string strQry = string.Empty;
                string sApproveResult = string.Empty;

                if (Convert.ToString(ConfigurationManager.AppSettings["Approval"]).ToUpper().Equals("OFF"))
                {
                    return false;
                }

                if (objApproval.sFormName != null && objApproval.sFormName != "")
                {
                    //To get Business Object Id

                    // string qry = "SELECT BO_ID FROM TBLBUSINESSOBJECT WHERE UPPER(BO_FORMNAME)=:FormName";
                    //oledbCommand.Parameters.AddWithValue("FormName", objApproval.sFormName.Trim().ToUpper());
                    //objApproval.sBOId = objcon.get_value(qry, oledbCommand);

                    string qry = "SELECT BO_ID FROM TBLBUSINESSOBJECT WHERE UPPER(BO_FORMNAME)='" + objApproval.sFormName.Trim().ToUpper() + "' ";
                    objApproval.sBOId = objconn.get_value(qry);

                }

                //Check Approval Exists
                bool bResult = CheckFormApprovalExists1(objApproval, objconn);

                if (bResult == true)
                {
                    //To get Role Id with Priority Level                 
                    objApproval.sRoleId = GetRoleFromApprovePriority1(objApproval, objconn);
                }
                else
                {
                    objApproval.sRoleId = "";

                }

                if (objApproval.sPrevApproveId == null)
                {
                    objApproval.sPrevApproveId = "0";
                }

                if (objApproval.sRoleId != "")
                {
                    File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , sRoleId null" + Environment.NewLine);

                    // SaveWorkFlowData(objApproval);
                    objApproval.sPrevWFOId = objApproval.sWFObjectId;

                    sWFlowId = Convert.ToString(objconn.Get_max_no("WO_ID", "TBLWORKFLOWOBJECTS"));

                    objApproval.sWFObjectId = sWFlowId;
                    strQry = "INSERT INTO TBLWORKFLOWOBJECTS (WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_NEXT_ROLE,WO_OFFICE_CODE,";
                    strQry += " WO_CLIENT_IP,WO_CR_BY,WO_DESCRIPTION,WO_WFO_ID,WO_DATA_ID,WO_REF_OFFCODE)";
                    strQry += " VALUES ('" + sWFlowId + "','" + objApproval.sBOId + "','" + objApproval.sRecordId + "','" + objApproval.sPrevApproveId + "',";
                    strQry += " '" + objApproval.sRoleId + "','" + objApproval.sOfficeCode + "','" + objApproval.sClientIp + "','" + objApproval.sCrby + "',";
                    strQry += " '" + objApproval.sDescription + "','" + objApproval.sWFDataId + "','" + objApproval.sDataReferenceId + "','" + objApproval.sRefOfficeCode + "')";
                    objconn.Execute(strQry);

                    //Insert Initial Id for WorkFlow
                    if (objApproval.sWFInitialId == "" || objApproval.sWFInitialId == null)
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + sWFlowId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objconn.Execute(strQry);
                    }
                    else
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + objApproval.sWFInitialId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objconn.Execute(strQry);
                    }
                    File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , After Execute query" + Environment.NewLine);

                    UpdateWFOAutoObject1(objApproval, objconn);

                    //Saving SMS in Table to send to Next Role
                    SendSMStoRole(objApproval, "");
                    return true;
                }
                else
                {


                    if (objApproval.sPrevApproveId == "0" && objApproval.sRoleId == "")
                    {
                        objApproval.sWFDataId = "";
                        objApproval.sWFInitialId = "";
                    }

                    sWFlowId = Convert.ToString(objconn.Get_max_no("WO_ID", "TBLWORKFLOWOBJECTS"));

                    strQry = "INSERT INTO TBLWORKFLOWOBJECTS (WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_NEXT_ROLE,WO_OFFICE_CODE,WO_CLIENT_IP,";
                    strQry += " WO_CR_BY,WO_APPROVE_STATUS,WO_DESCRIPTION,WO_WFO_ID,WO_DATA_ID,WO_REF_OFFCODE)";
                    strQry += " VALUES ('" + sWFlowId + "','" + objApproval.sBOId + "','" + objApproval.sRecordId + "','" + objApproval.sPrevApproveId + "',";
                    strQry += " '0','" + objApproval.sOfficeCode + "','" + objApproval.sClientIp + "','" + objApproval.sCrby + "',";
                    strQry += " '1','" + objApproval.sDescription + "','" + objApproval.sWFDataId + "','" + objApproval.sDataReferenceId + "','" + objApproval.sRefOfficeCode + "')";
                    objconn.Execute(strQry);
                    //Insert Initial Id for WorkFlow
                    if (objApproval.sWFInitialId == "" || objApproval.sWFInitialId == null)
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + sWFlowId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objconn.Execute(strQry);
                    }
                    else
                    {
                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_INITIAL_ID='" + objApproval.sWFInitialId + "' WHERE WO_ID='" + sWFlowId + "'";
                        objconn.Execute(strQry);
                    }

                    UpdateToMainTable1(objApproval, objconn);
                    oledbCommand = new OleDbCommand();
                    if (objApproval.sBOId == "30")
                    {
                        strQry = "SELECT WFO_QUERY_VALUES FROM TBLWORKFLOWOBJECTS,TBLWFODATA WHERE WO_WFO_ID=WFO_ID AND WO_BO_ID='30' AND WO_ID=:WFObjectId";
                        oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                        string query = objconn.get_value(strQry, oledbCommand);

                        DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                        bool result = objWCF.SaveTcDetails(query);
                        oledbCommand = new OleDbCommand();
                        if (result == false)
                        {
                            strQry = "SELECT WO_INITIAL_ID FROM TBLWORKFLOWOBJECTS,TBLWFODATA WHERE WO_WFO_ID=WFO_ID AND WO_BO_ID='30' AND WO_ID=:WFObjectId";
                            oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                            string sWO_ID = objconn.get_value(strQry, oledbCommand);

                            oledbCommand = new OleDbCommand();
                            start_point = query.IndexOf("(");
                            end_point = query.IndexOf(")");
                            strQry = "SELECT substr(WO_WFO_ID,'" + start_point + "','" + end_point + "' )WO_WFO_ID FROM TBLWORKFLOWOBJECTS,TBLWFODATA WHERE WO_WFO_ID=WFO_ID AND WO_BO_ID='30' AND WO_ID=:WFObjectId1";
                            oledbCommand.Parameters.AddWithValue("WFObjectId1", objApproval.sWFObjectId);
                            string query1 = objconn.get_value(strQry, oledbCommand);
                            strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1,TC_UPDATED_EVENT='' WHERE TC_CODE IN " + query1 + "";
                            objconn.Execute(strQry);
                            strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + sWFlowId + "'";
                            objconn.Execute(strQry);
                            strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_APPROVE_STATUS='0' WHERE WO_ID='" + sWO_ID + "'";
                            objconn.Execute(strQry);
                        }

                    }

                    objconn.CommitTrans();
                    string sPrevBoId = objApproval.sBOId;
                    string sResult = GetNextBOId(objApproval.sBOId);
                    objApproval.sPrevWFOId = objApproval.sWFObjectId;
                    objApproval.sWFObjectId = sWFlowId;

                    if (sResult != "")
                    {
                        objApproval.sBOFlowMasterId = sResult.Split('~').GetValue(1).ToString();
                        objApproval.sBOId = sResult.Split('~').GetValue(0).ToString();

                        //To get Role Id with Priority Level to Create Form
                        objApproval.sRoleId = GetRoleFromApprovePriorityForBOCreate(objApproval);

                        bool resrult = SaveWFObjectAuto(objApproval);
                        if (resrult == false)
                        {
                            return false;
                        }
                    }

                    UpdateWFOAutoObject(objApproval);

                    //Saving SMS in Table to send to Next Role
                    SendSMStoRole(objApproval, sPrevBoId);

                    return true;

                }

            }
            catch (Exception ex)
            {
                //objconn.RollBack();               
                clsException.LogError(ex.StackTrace, ex.Message,
                                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;

            }
        }
        

        /// <summary>
        /// Get Approval Priority Role From TBLWORKFLOWMASTER Table 
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public string GetRoleFromApprovePriority(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                string sExistWFObject = string.Empty;


                strQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID =:BOIds AND WO_RECORD_ID=:RecordIds";
                oledbCommand.Parameters.AddWithValue("BOIds", objApproval.sBOId);
                oledbCommand.Parameters.AddWithValue("RecordIds", objApproval.sRecordId);
                sExistWFObject = objcon.get_value(strQry, oledbCommand);

                oledbCommand = new OleDbCommand();
                if (sExistWFObject == "" || objApproval.sIsOldPo == true)
                {
                    strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId AND WM_LEVEL = (";
                    strQry += " SELECT MIN(WM_LEVEL) FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId1 AND WM_LEVEL <> '1')";
                    oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                    oledbCommand.Parameters.AddWithValue("BOId1", objApproval.sBOId);
                }
                else if (objApproval.sApproveStatus == "3")
                {
                    strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId2 AND WM_LEVEL = (";
                    strQry += " SELECT MIN(WM_LEVEL) FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId3 AND WM_LEVEL = '1')";
                    oledbCommand.Parameters.AddWithValue("BOId2", objApproval.sBOId);
                    oledbCommand.Parameters.AddWithValue("BOId3", objApproval.sBOId);
                }
                else
                {
                    strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId4 AND WM_LEVEL = (";
                    strQry += " SELECT WM_LEVEL+1 FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId5 AND WM_ROLEID = ";
                    strQry += " (SELECT WO_NEXT_ROLE FROM TBLWORKFLOWOBJECTS WHERE WO_ID = ";
                    strQry += " (SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID =:BOId6 AND WO_RECORD_ID=:RecordId)))";
                    oledbCommand.Parameters.AddWithValue("BOId4", objApproval.sBOId);
                    oledbCommand.Parameters.AddWithValue("BOId5", objApproval.sBOId);
                    oledbCommand.Parameters.AddWithValue("BOId6", objApproval.sBOId);
                    oledbCommand.Parameters.AddWithValue("RecordId", objApproval.sRecordId);
                }
                return objcon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }

        public string GetRoleFromApprovePrioritylatest(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                string sExistWFObject = string.Empty;

                strQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID ='" + objApproval.sBOId + "' AND WO_RECORD_ID='" + objApproval.sRecordId + "'";
                //oledbCommand.Parameters.AddWithValue("BOIds", objApproval.sBOId);
                //oledbCommand.Parameters.AddWithValue("RecordIds", objApproval.sRecordId);
                sExistWFObject = objconn.get_value(strQry);


                oledbCommand = new OleDbCommand();
                if (sExistWFObject == "" || objApproval.sIsOldPo == true)
                {
                    strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = (";
                    strQry += " SELECT MIN(WM_LEVEL) FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL <> '1')";
                    //oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("BOId1", objApproval.sBOId);
                }
                else if (objApproval.sApproveStatus == "3")
                {
                    strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = (";
                    strQry += " SELECT MIN(WM_LEVEL) FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = '1')";
                    //oledbCommand.Parameters.AddWithValue("BOId2", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("BOId3", objApproval.sBOId);
                }
                else
                {
                    strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = (";
                    strQry += " SELECT WM_LEVEL+1 FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_ROLEID = ";
                    strQry += " (SELECT WO_NEXT_ROLE FROM TBLWORKFLOWOBJECTS WHERE WO_ID = ";
                    strQry += " (SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID ='" + objApproval.sBOId + "' AND WO_RECORD_ID='" + objApproval.sRecordId + "')))";
                    //oledbCommand.Parameters.AddWithValue("BOId4", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("BOId5", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("BOId6", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("RecordId", objApproval.sRecordId);
                }
                return objconn.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }

     
        public string GetRoleFromApprovePriority1(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                string sExistWFObject = string.Empty;

                //  strQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID =:BOIds AND WO_RECORD_ID=:RecordIds";
                //oledbCommand.Parameters.AddWithValue("BOIds", objApproval.sBOId);
                //oledbCommand.Parameters.AddWithValue("RecordIds", objApproval.sRecordId);
                //sExistWFObject = objcon.get_value(strQry, oledbCommand);

                strQry = "SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID ='" + objApproval.sBOId + "' AND WO_RECORD_ID='" + objApproval.sRecordId + "'";
                sExistWFObject = objconn.get_value(strQry);


                oledbCommand = new OleDbCommand();
                if (sExistWFObject == "" || objApproval.sIsOldPo == true)
                {
                    strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = (";
                    strQry += " SELECT MIN(WM_LEVEL) FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL <> '1')";
                    //oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("BOId1", objApproval.sBOId);
                }
                else if (objApproval.sApproveStatus == "3")
                {
                    strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = (";
                    strQry += " SELECT MIN(WM_LEVEL) FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = '1')";
                    //oledbCommand.Parameters.AddWithValue("BOId2", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("BOId3", objApproval.sBOId);
                }
                else
                {
                    strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = (";
                    strQry += " SELECT WM_LEVEL+1 FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_ROLEID = ";
                    strQry += " (SELECT WO_NEXT_ROLE FROM TBLWORKFLOWOBJECTS WHERE WO_ID = ";
                    strQry += " (SELECT MAX(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_BO_ID ='" + objApproval.sBOId + "' AND WO_RECORD_ID='" + objApproval.sRecordId + "')))";
                    //oledbCommand.Parameters.AddWithValue("BOId4", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("BOId5", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("BOId6", objApproval.sBOId);
                    //oledbCommand.Parameters.AddWithValue("RecordId", objApproval.sRecordId);
                }



                return objconn.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }

        /// <summary>
        /// Get Role Id of Form Creator from Bussiness Object Id
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public string GetRoleFromApprovePriorityForBOCreate(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;

                strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId AND WM_LEVEL = (";
                strQry += " SELECT MIN(WM_LEVEL) FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId1 AND WM_LEVEL = '1')";
                oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                oledbCommand.Parameters.AddWithValue("BOId1", objApproval.sBOId);
                // oledbCommand.Parameters.AddWithValue("RecordId", objApproval.sRecordId);

                return objcon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }

        public string GetRoleFromApprovePriorityForBOCreate1(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;

                strQry = "SELECT WM_ROLEID FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = (";
                strQry += " SELECT MIN(WM_LEVEL) FROM TBLWORKFLOWMASTER WHERE WM_BOID='" + objApproval.sBOId + "' AND WM_LEVEL = '1')";
                //oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                //oledbCommand.Parameters.AddWithValue("BOId1", objApproval.sBOId);
                // oledbCommand.Parameters.AddWithValue("RecordId", objApproval.sRecordId);

                return objconn.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }


        /// <summary>
        /// Check Approval Exists for Given Form/Bussiness Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool CheckFormApprovalExists(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT * FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId";
                oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                OleDbDataReader dr = objcon.Fetch(strQry, oledbCommand);
                if (dr.Read())
                {
                    dr.Close();
                    return true;
                }
                dr.Close();
                return false;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }


        public bool CheckFormApprovalExists1(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT * FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId";
                oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                OleDbDataReader dr = objconn.Fetch(strQry, oledbCommand);
                if (dr.Read())
                {
                    dr.Close();
                    return true;
                }
                dr.Close();
                return false;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool CheckFormApprovalExistslatest(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT * FROM TBLWORKFLOWMASTER WHERE WM_BOID=:BOId";
                oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                OleDbDataReader dr = objconn.Fetch(strQry, oledbCommand);
                if (dr.Read())
                {
                    dr.Close();
                    return true;
                }
                dr.Close();
                return false;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

   
        /// <summary>
        /// Approve WorkFlow Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool ApproveWFRequest(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();

            try
            {

                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sNextRole = string.Empty;
                string sApproveResult = string.Empty;

                if (objApproval.sWFAutoId != null)
                {
                    oledbCommand = new OleDbCommand();
                    if (objApproval.sWFAutoId == "0")
                    {
                        strQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID=:WFObjectId AND WO_APPROVE_STATUS<>0";
                        oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                        sApproveResult = objcon.get_value(strQry, oledbCommand);
                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                    else
                    {

                        strQry = "SELECT WOA_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_ID=:WFAutoId AND WOA_INITIAL_ACTION_ID IS NOT NULL";
                        oledbCommand.Parameters.AddWithValue("WFAutoId", objApproval.sWFAutoId);
                        sApproveResult = objcon.get_value(strQry, oledbCommand);
                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                }

                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_USER_COMMENT='" + objApproval.sApproveComments.Replace("'", "''") + "',WO_APPROVE_STATUS='" + objApproval.sApproveStatus + "',";
                strQry += " WO_APPROVED_BY='" + objApproval.sCrby + "' WHERE WO_ID='" + objApproval.sWFObjectId + "'";
                objcon.Execute(strQry);
                oledbCommand = new OleDbCommand();
                strQry = "SELECT WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_OFFICE_CODE,WO_CLIENT_IP,WO_NEXT_ROLE,WO_DESCRIPTION,";
                strQry += " WO_WFO_ID,WO_INITIAL_ID,WO_DATA_ID,WO_REF_OFFCODE FROM TBLWORKFLOWOBJECTS ";
                strQry += " WHERE WO_ID=:WFObjectId";

                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                dt = objcon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sBOId = Convert.ToString(dt.Rows[0]["WO_BO_ID"]);

                    if (objApproval.sRecordId == "" || objApproval.sRecordId == null)
                    {
                        objApproval.sRecordId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    }
                    if (objApproval.sWFDataId == "" || objApproval.sWFDataId == null)
                    {
                        objApproval.sWFDataId = Convert.ToString(dt.Rows[0]["WO_WFO_ID"]);
                    }

                    objApproval.sWFInitialId = Convert.ToString(dt.Rows[0]["WO_INITIAL_ID"]);
                    objApproval.sDataReferenceId = Convert.ToString(dt.Rows[0]["WO_DATA_ID"]);

                    if (objApproval.sRefOfficeCode == "" || objApproval.sRefOfficeCode == null)
                    {
                        objApproval.sRefOfficeCode = Convert.ToString(dt.Rows[0]["WO_REF_OFFCODE"]);
                    }

                    if (objApproval.sDescription == "" || objApproval.sDescription == null)
                    {
                        objApproval.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                    }
                    oledbCommand = new OleDbCommand();
                    sNextRole = Convert.ToString(dt.Rows[0]["WO_NEXT_ROLE"]);
                    if (sNextRole == "0")
                    {
                        strQry = "SELECT BFM_NEXT_BO_ID  FROM TBLWO_OBJECT_AUTO,TBLBO_FLOW_MASTER WHERE WOA_BFM_ID=BFM_ID AND WOA_PREV_APPROVE_ID=:WFObjectId";
                        oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                        string sResult = objcon.get_value(strQry, oledbCommand);
                        objApproval.sBOId = sResult;
                        objApproval.sPrevWFOId = objApproval.sWFObjectId;

                    }
                    else
                    {
                        objApproval.sPrevApproveId = Convert.ToString(dt.Rows[0]["WO_ID"]);
                    }

                }

                bool res = SaveWorkflowObjects(objApproval);
                if (res == false)
                {
                    return false;
                }
                UpdateWFOAutoObject(objApproval);
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool ApproveWFRequest1(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            CustOledbConnection objconn = new CustOledbConnection(Constants.Password);
            try
            {
                objconn.BeginTrans();
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sNextRole = string.Empty;
                string sApproveResult = string.Empty;

                if (objApproval.sWFAutoId != null)
                {
                    if (objApproval.sWFAutoId == "0")
                    {
                        strQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "' AND WO_APPROVE_STATUS<>0";
                        sApproveResult = objconn.get_value(strQry);

                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                    else
                    {

                        strQry = "SELECT WOA_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_ID='" + objApproval.sWFAutoId + "' AND WOA_INITIAL_ACTION_ID IS NOT NULL";
                        sApproveResult = objconn.get_value(strQry);
                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                }

                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_USER_COMMENT='" + objApproval.sApproveComments.Replace("'", "''") + "',WO_APPROVE_STATUS='" + objApproval.sApproveStatus + "',";
                strQry += " WO_APPROVED_BY='" + objApproval.sCrby + "' WHERE WO_ID='" + objApproval.sWFObjectId + "'";
                objconn.Execute(strQry);
                strQry = "SELECT WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_OFFICE_CODE,WO_CLIENT_IP,WO_NEXT_ROLE,WO_DESCRIPTION,";
                strQry += " WO_WFO_ID,WO_INITIAL_ID,WO_DATA_ID,WO_REF_OFFCODE FROM TBLWORKFLOWOBJECTS ";
                strQry += " WHERE WO_ID='" + objApproval.sWFObjectId + "'";
                OleDbDataReader dr = objconn.Fetch(strQry);
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sBOId = Convert.ToString(dt.Rows[0]["WO_BO_ID"]);

                    if (objApproval.sRecordId == "" || objApproval.sRecordId == null)
                    {
                        objApproval.sRecordId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    }
                    if (objApproval.sWFDataId == "" || objApproval.sWFDataId == null)
                    {
                        objApproval.sWFDataId = Convert.ToString(dt.Rows[0]["WO_WFO_ID"]);
                    }

                    objApproval.sWFInitialId = Convert.ToString(dt.Rows[0]["WO_INITIAL_ID"]);
                    objApproval.sDataReferenceId = Convert.ToString(dt.Rows[0]["WO_DATA_ID"]);

                    if (objApproval.sRefOfficeCode == "" || objApproval.sRefOfficeCode == null)
                    {
                        objApproval.sRefOfficeCode = Convert.ToString(dt.Rows[0]["WO_REF_OFFCODE"]);
                    }

                    if (objApproval.sDescription == "" || objApproval.sDescription == null)
                    {
                        objApproval.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                    }
                    sNextRole = Convert.ToString(dt.Rows[0]["WO_NEXT_ROLE"]);
                    if (sNextRole == "0")
                    {
                        strQry = "SELECT BFM_NEXT_BO_ID  FROM TBLWO_OBJECT_AUTO,TBLBO_FLOW_MASTER WHERE WOA_BFM_ID=BFM_ID AND WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";
                        string sResult = objconn.get_value(strQry);
                        objApproval.sBOId = sResult;
                        objApproval.sPrevWFOId = objApproval.sWFObjectId;

                    }
                    else
                    {
                        objApproval.sPrevApproveId = Convert.ToString(dt.Rows[0]["WO_ID"]);
                    }

                }
                bool res = SaveWorkflowObjectslatest(objApproval, objconn);

                if (res == false)
                {
                    return false;
                }
                UpdateWFOAutoObject(objApproval);

                return true;
            }
            catch (Exception ex)
            {
                objconn.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        /// Modify and Approve Workflow Object
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool ModifyApproveWFRequest(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();

            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sNextRole = string.Empty;


                strQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID=:WFObjectId AND WO_APPROVE_STATUS<>0";
                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                string sApproveResult = objcon.get_value(strQry, oledbCommand);
                if (sApproveResult != "")
                {
                    return false;
                }


                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_USER_COMMENT='" + objApproval.sApproveComments.Replace("'", "''") + "',WO_APPROVE_STATUS='" + objApproval.sApproveStatus + "',";
                strQry += " WO_APPROVED_BY='" + objApproval.sCrby + "' WHERE WO_ID='" + objApproval.sWFObjectId + "'";
                objcon.Execute(strQry);

                //objApproval.sDescription = objcon.get_value("SELECT WO_DESCRIPTION FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "'");
                oledbCommand = new OleDbCommand();
                strQry = "SELECT WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_OFFICE_CODE,WO_CLIENT_IP,WO_NEXT_ROLE,WO_DESCRIPTION,";
                strQry += " WO_WFO_ID,WO_INITIAL_ID,WO_DATA_ID,WO_REF_OFFCODE FROM TBLWORKFLOWOBJECTS ";
                strQry += " WHERE WO_ID=:WFObjectId";
                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                dt = objcon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sBOId = Convert.ToString(dt.Rows[0]["WO_BO_ID"]);
                    objApproval.sRecordId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    // objApproval.sWFDataId = Convert.ToString(dt.Rows[0]["WO_WFO_ID"]);
                    objApproval.sWFInitialId = Convert.ToString(dt.Rows[0]["WO_INITIAL_ID"]);
                    objApproval.sDataReferenceId = Convert.ToString(dt.Rows[0]["WO_DATA_ID"]);


                    if (objApproval.sRefOfficeCode == "" || objApproval.sRefOfficeCode == null)
                    {
                        objApproval.sRefOfficeCode = Convert.ToString(dt.Rows[0]["WO_REF_OFFCODE"]);
                    }

                    if (objApproval.sDescription == "" || objApproval.sDescription == null)
                    {
                        objApproval.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                    }
                    oledbCommand = new OleDbCommand();
                    sNextRole = Convert.ToString(dt.Rows[0]["WO_NEXT_ROLE"]);
                    if (sNextRole == "0")
                    {
                        //strQry = "SELECT BFM_NEXT_BO_ID || '~' || WOA_ROLE_ID FROM TBLWO_OBJECT_AUTO,TBLBO_FLOW_MASTER WHERE WOA_BFM_ID=BFM_ID AND WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";
                        strQry = "SELECT BFM_NEXT_BO_ID  FROM TBLWO_OBJECT_AUTO,TBLBO_FLOW_MASTER WHERE WOA_BFM_ID=BFM_ID AND WOA_PREV_APPROVE_ID=:WFObjectId";
                        oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                        string sResult = objcon.get_value(strQry, oledbCommand);
                        objApproval.sBOId = sResult;
                        //objApproval.sRoleId = sResult.Split('~').GetValue(1).ToString();

                        objApproval.sPrevWFOId = objApproval.sWFObjectId;

                    }
                    else
                    {
                        objApproval.sPrevApproveId = Convert.ToString(dt.Rows[0]["WO_ID"]);
                    }

                }

                bool res = SaveWorkflowObjects(objApproval);
                if (res == false)
                {
                    return false;
                }

                UpdateWFOAutoObject(objApproval);

                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool ModifyApproveWFRequest1(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            CustOledbConnection objconn = new CustOledbConnection(Constants.Password);

            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sNextRole = string.Empty;

                objconn.BeginTrans();
                strQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "' AND WO_APPROVE_STATUS<>0";
                //  oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                string sApproveResult = objconn.get_value(strQry);
                if (sApproveResult != "")
                {
                    return false;
                }


                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_USER_COMMENT='" + objApproval.sApproveComments.Replace("'", "''") + "',WO_APPROVE_STATUS='" + objApproval.sApproveStatus + "',";
                strQry += " WO_APPROVED_BY='" + objApproval.sCrby + "' WHERE WO_ID='" + objApproval.sWFObjectId + "'";
                objconn.Execute(strQry);

                //objApproval.sDescription = objcon.get_value("SELECT WO_DESCRIPTION FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "'");
                oledbCommand = new OleDbCommand();
                strQry = "SELECT WO_ID,WO_BO_ID,WO_RECORD_ID,WO_PREV_APPROVE_ID,WO_OFFICE_CODE,WO_CLIENT_IP,WO_NEXT_ROLE,WO_DESCRIPTION,";
                strQry += " WO_WFO_ID,WO_INITIAL_ID,WO_DATA_ID,WO_REF_OFFCODE FROM TBLWORKFLOWOBJECTS ";
                strQry += " WHERE WO_ID=:WFObjectId";
                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                dt = objcon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sBOId = Convert.ToString(dt.Rows[0]["WO_BO_ID"]);
                    objApproval.sRecordId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                    // objApproval.sWFDataId = Convert.ToString(dt.Rows[0]["WO_WFO_ID"]);
                    objApproval.sWFInitialId = Convert.ToString(dt.Rows[0]["WO_INITIAL_ID"]);
                    objApproval.sDataReferenceId = Convert.ToString(dt.Rows[0]["WO_DATA_ID"]);


                    if (objApproval.sRefOfficeCode == "" || objApproval.sRefOfficeCode == null)
                    {
                        objApproval.sRefOfficeCode = Convert.ToString(dt.Rows[0]["WO_REF_OFFCODE"]);
                    }

                    if (objApproval.sDescription == "" || objApproval.sDescription == null)
                    {
                        objApproval.sDescription = Convert.ToString(dt.Rows[0]["WO_DESCRIPTION"]);
                    }
                    oledbCommand = new OleDbCommand();
                    sNextRole = Convert.ToString(dt.Rows[0]["WO_NEXT_ROLE"]);
                    if (sNextRole == "0")
                    {
                        //strQry = "SELECT BFM_NEXT_BO_ID || '~' || WOA_ROLE_ID FROM TBLWO_OBJECT_AUTO,TBLBO_FLOW_MASTER WHERE WOA_BFM_ID=BFM_ID AND WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";
                        strQry = "SELECT BFM_NEXT_BO_ID  FROM TBLWO_OBJECT_AUTO,TBLBO_FLOW_MASTER WHERE WOA_BFM_ID=BFM_ID AND WOA_PREV_APPROVE_ID=:WFObjectId";
                        oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                        string sResult = objcon.get_value(strQry, oledbCommand);
                        objApproval.sBOId = sResult;
                        //objApproval.sRoleId = sResult.Split('~').GetValue(1).ToString();

                        objApproval.sPrevWFOId = objApproval.sWFObjectId;

                    }
                    else
                    {
                        objApproval.sPrevApproveId = Convert.ToString(dt.Rows[0]["WO_ID"]);
                    }

                }


                //  bool res = SaveWorkflowObjects1(objApproval, objconn);
                bool res = SaveWorkflowObjectslatest(objApproval, objconn);

                if (res == false)
                {
                    return false;
                }

                UpdateWFOAutoObject(objApproval);

                return true;
            }
            catch (Exception ex)
            {
                objconn.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                // return false;
                throw ex;
            }
        }

        #region Approval Column Update Concept

        public void UpdateApproveStatusinMainTable(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                strQry = "SELECT BO_MAIN_TABLE,BO_REF_COLUMN,BO_REF_APPROVE FROM TBLBUSINESSOBJECT WHERE BO_ID=:BOId";
                oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);
                dt = objcon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sMainTable = Convert.ToString(dt.Rows[0]["BO_MAIN_TABLE"]);
                    objApproval.sRefColumnName = Convert.ToString(dt.Rows[0]["BO_REF_COLUMN"]);
                    objApproval.sApproveColumnName = Convert.ToString(dt.Rows[0]["BO_REF_APPROVE"]);
                }

                if (objApproval.sMainTable != "")
                {
                    strQry = "UPDATE " + objApproval.sMainTable + "  SET   " + sApproveColumnName + "='1' WHERE ";
                    strQry += " " + objApproval.sRefColumnName + "='" + objApproval.sRecordId + "'";
                    objcon.Execute(strQry);
                }


            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateApproveStatusinMainTable");

            }
        }
        #endregion

        /// <summary>
        /// Update to Main Table by Fetching Queries from TBLWFODATA Table
        /// </summary>
        /// <param name="objApproval"></param>
        public void UpdateToMainTable(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();

            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                strQry = "SELECT WFO_QUERY_VALUES,WFO_PARAMETER FROM TBLWFODATA WHERE WFO_ID=:WFDataId";
                oledbCommand.Parameters.AddWithValue("WFDataId", objApproval.sWFDataId);
                dt = objcon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sColumnNames = Convert.ToString(dt.Rows[0]["WFO_QUERY_VALUES"]);
                    objApproval.sParameterValues = Convert.ToString(dt.Rows[0]["WFO_PARAMETER"]);

                    if (objApproval.sParameterValues != "")
                    {
                        string[] sParameterQueries = objApproval.sParameterValues.Split(';');
                        string sSecondRecordId = string.Empty;

                        for (int i = 0; i < sParameterQueries.Length; i++)
                        {
                            if (objApproval.sNewRecordId == null || objApproval.sNewRecordId == "")
                            {
                                if (sParameterQueries[i].ToString() != "")
                                {
                                    objApproval.sNewRecordId = objcon.get_value(sParameterQueries[i]);
                                    objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", objApproval.sNewRecordId);
                                }
                            }
                            else
                            {
                                sSecondRecordId = objcon.get_value(sParameterQueries[i]);
                                objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", sSecondRecordId);
                            }

                        }
                    }

                    string[] sQueries = objApproval.sColumnNames.Split(';');
                    for (int i = 0; i < sQueries.Length; i++)
                    {
                        if (sQueries[i].ToString() != "")
                        {
                            objcon.Execute(sQueries[i]);
                        }
                    }

                }

                if (objApproval.sNewRecordId != null && objApproval.sNewRecordId != "")
                {
                    UpdateWorkFlowRecordId(objApproval);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);

            }
        }


        public void UpdateToMainTable1(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();

            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                strQry = "SELECT WFO_QUERY_VALUES,WFO_PARAMETER FROM TBLWFODATA WHERE WFO_ID=:WFDataId";
                oledbCommand.Parameters.AddWithValue("WFDataId", objApproval.sWFDataId);
                dt = objcon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sColumnNames = Convert.ToString(dt.Rows[0]["WFO_QUERY_VALUES"]);
                    objApproval.sParameterValues = Convert.ToString(dt.Rows[0]["WFO_PARAMETER"]);

                    if (objApproval.sParameterValues != "")
                    {
                        string[] sParameterQueries = objApproval.sParameterValues.Split(';');
                        string sSecondRecordId = string.Empty;

                        for (int i = 0; i < sParameterQueries.Length; i++)
                        {
                            if (objApproval.sNewRecordId == null || objApproval.sNewRecordId == "")
                            {
                                if (sParameterQueries[i].ToString() != "")
                                {
                                    objApproval.sNewRecordId = objcon.get_value(sParameterQueries[i]);
                                    objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", objApproval.sNewRecordId);
                                }
                            }
                            else
                            {
                                sSecondRecordId = objcon.get_value(sParameterQueries[i]);
                                objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", sSecondRecordId);
                            }

                        }
                    }

                    string[] sQueries = objApproval.sColumnNames.Split(';');
                    for (int i = 0; i < sQueries.Length; i++)
                    {
                        if (sQueries[i].ToString() != "")
                        {
                            objconn.Execute(sQueries[i]);
                        }
                    }

                }

                if (objApproval.sNewRecordId != null && objApproval.sNewRecordId != "")
                {
                    UpdateWorkFlowRecordId1(objApproval, objconn);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }


        public void UpdateToMainTablelatest(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();

            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();

                strQry = "SELECT WFO_QUERY_VALUES,WFO_PARAMETER FROM TBLWFODATA WHERE WFO_ID='" + objApproval.sWFDataId + "'";
                //   oledbCommand.Parameters.AddWithValue("WFDataId", objApproval.sWFDataId);
                dt = objcon.getDataTable(strQry);
                if (dt.Rows.Count > 0)
                {
                    objApproval.sColumnNames = Convert.ToString(dt.Rows[0]["WFO_QUERY_VALUES"]);
                    objApproval.sParameterValues = Convert.ToString(dt.Rows[0]["WFO_PARAMETER"]);

                    if (objApproval.sParameterValues != "")
                    {
                        string[] sParameterQueries = objApproval.sParameterValues.Split(';');
                        string sSecondRecordId = string.Empty;

                        for (int i = 0; i < sParameterQueries.Length; i++)
                        {
                            if (objApproval.sNewRecordId == null || objApproval.sNewRecordId == "")
                            {
                                if (sParameterQueries[i].ToString() != "")
                                {
                                    objApproval.sNewRecordId = objconn.get_value(sParameterQueries[i]);
                                    objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", objApproval.sNewRecordId);
                                }
                            }
                            else
                            {
                                sSecondRecordId = objconn.get_value(sParameterQueries[i]);
                                objApproval.sColumnNames = objApproval.sColumnNames.Replace("{" + i + "}", sSecondRecordId);
                            }

                        }
                    }

                    string[] sQueries = objApproval.sColumnNames.Split(';');
                    for (int i = 0; i < sQueries.Length; i++)
                    {
                        if (sQueries[i].ToString() != "")
                        {
                            objconn.Execute(sQueries[i]);
                        }
                    }

                }

                if (objApproval.sNewRecordId != null && objApproval.sNewRecordId != "")
                {
                    UpdateWorkFlowRecordIdlatest(objApproval, objconn);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

       
        /// <summary>
        /// Get Form Name Using Bussiness Object Id
        /// </summary>
        /// <param name="sBOId"></param>
        /// <returns></returns>
        public string GetFormName(string sBOId)
        {
            oledbCommand = new OleDbCommand();

            try
            {
                string strQry = string.Empty;
                string BOId = sBOId;
                strQry = "SELECT BO_FORMNAME FROM TBLBUSINESSOBJECT WHERE BO_ID=:BOId";
                oledbCommand.Parameters.AddWithValue("BOId", BOId);
                return objcon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }

        /// <summary>
        /// Update Genrated/Actual Record Id to TBLWORKFLOWOBJECTS Table
        /// </summary>
        /// <param name="objApproval"></param>
        public void UpdateWorkFlowRecordId(clsApproval objApproval)
        {

            try
            {
                string strQry = string.Empty;
                if (objApproval.sNewRecordId == null || objApproval.sNewRecordId == "")
                {
                    return;
                }
                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + objApproval.sNewRecordId + "' WHERE ";
                strQry += " WO_BO_ID='" + objApproval.sBOId + "'AND WO_RECORD_ID='" + objApproval.sRecordId + "'";
                objcon.Execute(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);

            }
        }

        public void UpdateWorkFlowRecordId1(clsApproval objApproval, CustOledbConnection objconn)
        {

            try
            {
                string strQry = string.Empty;
                if (objApproval.sNewRecordId == null || objApproval.sNewRecordId == "")
                {
                    return;
                }
                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + objApproval.sNewRecordId + "' WHERE ";
                strQry += " WO_BO_ID='" + objApproval.sBOId + "'AND WO_RECORD_ID='" + objApproval.sRecordId + "'";
                objconn.Execute(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);

            }
        }

        public void UpdateWorkFlowRecordIdlatest(clsApproval objApproval, CustOledbConnection objconn)
        {

            try
            {
                string strQry = string.Empty;
                if (objApproval.sNewRecordId == null || objApproval.sNewRecordId == "")
                {
                    return;
                }
                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + objApproval.sNewRecordId + "' WHERE ";
                strQry += " WO_BO_ID='" + objApproval.sBOId + "'AND WO_RECORD_ID='" + objApproval.sRecordId + "'";
                objconn.Execute(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);

            }
        }

     
        #region XML Concepts

        public string DatatableToXML(DataTable dtforXML, string sTableName)
        {
            string xmlstr = string.Empty;
            DataTable dt = new DataTable(sTableName);
            try
            {
                dt = dtforXML;

                if (dt != null && dt.Rows.Count == 0)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dc.DataType = typeof(String);
                    }


                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dr.ItemArray.Count(); i++)
                    {
                        dr[i] = string.Empty;
                    }
                    dt.Rows.Add(dr);


                }
                DataSet ds = new DataSet(sTableName);
                ds.Tables.Add(dt);
                return ds.GetXml();

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DatatableToXML");
                return xmlstr;
            }

        }



        public void CreateXMLFile(clsApproval objApproval, DataTable dt)
        {
            try
            {
                //If System.IO.Directory.Exists(sDataPath & "\" & "XMLData" & "\" & sNamespace & "-" & iCompanyId) = False Then
                //     System.IO.Directory.CreateDirectory(sDataPath & "\" & "XMLData" & "\" & iCompanyId)
                // End If
                string sDirectory = System.Web.HttpContext.Current.Server.MapPath("XMLData");
                string sSubDirectory = System.Web.HttpContext.Current.Server.MapPath("XMLData" + "/" + objApproval.sCrby);
                string sPath = System.Web.HttpContext.Current.Server.MapPath("XMLData" + "/" + objApproval.sCrby + "/" + objApproval.sMainTable);

                if (!Directory.Exists(sDirectory))
                {
                    Directory.CreateDirectory(sDirectory);
                }

                if (!Directory.Exists(sSubDirectory))
                {
                    Directory.CreateDirectory(sSubDirectory);
                }

                if (File.Exists(sPath))
                {
                    File.Delete(sPath);
                }

                dt.WriteXml(sPath);

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CreateXMLFile");

            }
        }

        public void ReadXMLFile(clsApproval objApproval)
        {
            try
            {
                XmlReader objXmRdr;
                string sPath = System.Web.HttpContext.Current.Server.MapPath("XMLData" + "/" + objApproval.sCrby + "/" + objApproval.sMainTable);
                if (File.Exists(sPath))
                {
                    objXmRdr = XmlReader.Create(sPath);

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ReadXMLFile");

            }
        }

        #endregion


        /// <summary>
        /// Get Next Bussiness Object Id from TBLBO_FLOW_MASTER Table
        /// </summary>
        /// <param name="sBOId"></param>
        /// <returns></returns>
        public string GetNextBOId(string sBOId)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                string BOId = sBOId;
                strQry = "SELECT BFM_NEXT_BO_ID || '~' || BFM_ID  FROM TBLBO_FLOW_MASTER WHERE BFM_BO_ID=:BOId";
                oledbCommand.Parameters.AddWithValue("BOId", BOId);
                return objcon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }

        /// <summary>
        /// Save Initiation of Next Form for assigned Role
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool SaveWFObjectAuto(clsApproval objApproval)
        {
            oledbCommand = new OleDbCommand();
            Intigration_crby = objApproval.sCrby;
            string s = string.Empty;
            try
            {

                string strQry = string.Empty;

                if (objApproval.sBOId == "25" || objApproval.sBOId == "11")
                {
                    string qry = "SELECT DF_DTC_CODE FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE WHERE WO_ID=:WFObjectId AND DF_ID=WO_RECORD_ID";
                    oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                    string sRecordId = objcon.get_value(qry, oledbCommand);
                    objApproval.sDescription = "Work Order For DTC Code " + sRecordId;
                }
                if (objApproval.sBOId == "12")
                {
                    oledbCommand = new OleDbCommand();
                    string qry = "SELECT DF_DTC_CODE || '~' || WO_NO FROM TBLWORKFLOWOBJECTS,TBLWORKORDER,TBLDTCFAILURE WHERE WO_ID=:WFObjectId1 AND  WO_RECORD_ID=WO_SLNO AND DF_ID=WO_DF_ID";
                    oledbCommand.Parameters.AddWithValue("WFObjectId1", objApproval.sWFObjectId);
                    string sRecordId = objcon.get_value(qry, oledbCommand);
                    oledbCommand = new OleDbCommand();
                    if (sRecordId == "")
                    {
                        string qrys = "SELECT  WO_NO FROM TBLWORKFLOWOBJECTS,TBLWORKORDER WHERE WO_ID=:WFObjectId2 AND  WO_RECORD_ID=WO_SLNO";
                        oledbCommand.Parameters.AddWithValue("WFObjectId2", objApproval.sWFObjectId);
                        sRecordId = objcon.get_value(qrys, oledbCommand);

                        objApproval.sDescription = "Indent For New DTC Commission with WO No " + sRecordId;
                    }
                    else
                    {
                        objApproval.sDescription = "Indent For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " and WO No " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                }
                if (objApproval.sBOId == "13")
                {
                    oledbCommand = new OleDbCommand();

                    string record = "SELECT DF_DTC_CODE || '~' || TI_INDENT_NO || '~' || WO_NO || '~' || DT_NAME FROM TBLWORKFLOWOBJECTS,TBLINDENT,TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST WHERE WO_ID=:WFObjectId3 AND  WO_RECORD_ID=TI_ID AND DF_ID=WO_DF_ID AND TI_WO_SLNO=WO_SLNO AND DT_CODE=DF_DTC_CODE";
                    oledbCommand.Parameters.AddWithValue("WFObjectId3", objApproval.sWFObjectId);
                    string sRecordId = objcon.get_value(record, oledbCommand);
                    oledbCommand = new OleDbCommand();
                    if (sRecordId == "")
                    {
                        string sDescription = "SELECT TI_INDENT_NO || '~' || WO_NO FROM TBLWORKFLOWOBJECTS,TBLINDENT,TBLWORKORDER WHERE WO_ID=:WFObjectId4 AND  WO_RECORD_ID=TI_ID AND TI_WO_SLNO=WO_SLNO";
                        oledbCommand.Parameters.AddWithValue("WFObjectId4", objApproval.sWFObjectId);
                        sRecordId = objcon.get_value(sDescription, oledbCommand);
                        objApproval.sDescription = "Invoice For New DTC Commission with Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    else
                    {
                        objApproval.sDescription = "Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + ", DTC Name " + sRecordId.Split('~').GetValue(3).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString();
                    }
                }
                if (objApproval.sBOId == "14")
                {
                    oledbCommand = new OleDbCommand();
                    string qry = "SELECT DF_DTC_CODE FROM TBLTCDRAWN,TBLDTCFAILURE,TBLWORKFLOWOBJECTS WHERE WO_ID=:WFObjectId5 AND TD_DF_ID=DF_ID AND  WO_RECORD_ID=TD_INV_NO ";
                    oledbCommand.Parameters.AddWithValue("WFObjectId5", objApproval.sWFObjectId);
                    string sRecordId = objcon.get_value(qry, oledbCommand);

                    objApproval.sDescription = "Decommissioning For DTC Code " + sRecordId;
                    oledbCommand = new OleDbCommand();
                    if (sRecordId == "")
                    {
                        string RecordIdqry = "SELECT IN_INV_NO FROM TBLDTCINVOICE,TBLWORKFLOWOBJECTS WHERE WO_ID=:WFObjectId6 AND WO_RECORD_ID=IN_NO ";
                        oledbCommand.Parameters.AddWithValue("WFObjectId6", objApproval.sWFObjectId);
                        sRecordId = objcon.get_value(RecordIdqry, oledbCommand);
                        objApproval.sDescription = "Commissioning of DTC for the Invoice NO " + sRecordId;
                    }
                }
                if (objApproval.sBOId == "15")
                {
                    oledbCommand = new OleDbCommand();
                    string sQry = "SELECT  DF_EQUIPMENT_ID || '~' || WO_NO_DECOM || '~' || DF_DTC_CODE FROM TBLTCDRAWN,TBLDTCFAILURE,TBLWORKFLOWOBJECTS,TBLTCREPLACE,TBLWORKORDER WHERE WO_ID=:WFObjectId7 ";
                    sQry += " AND TD_DF_ID=DF_ID AND  WO_RECORD_ID=TR_ID AND TR_IN_NO=TD_INV_NO AND DF_ID=WO_DF_ID";
                    oledbCommand.Parameters.AddWithValue("WFObjectId7", objApproval.sWFObjectId);
                    string sRecordId = objcon.get_value(sQry, oledbCommand);
                    objApproval.sDescription = "RI Approval For DTC Code " + sRecordId.Split('~').GetValue(2).ToString() + " with DTr Code " + sRecordId.Split('~').GetValue(0).ToString() + " and Work Order NO " + sRecordId.Split('~').GetValue(1).ToString();

                }
                if (objApproval.sBOId == "26")
                {
                    oledbCommand = new OleDbCommand();
                    string sRecordIds = "SELECT DF_DTC_CODE FROM TBLTCDRAWN,TBLDTCFAILURE,TBLWORKFLOWOBJECTS,TBLTCREPLACE WHERE WO_ID=:WFObjectId8 AND TD_DF_ID=DF_ID AND  WO_RECORD_ID=TR_ID AND TR_IN_NO=TD_INV_NO";
                    oledbCommand.Parameters.AddWithValue("WFObjectId8", objApproval.sWFObjectId);
                    string sRecordId = objcon.get_value(sRecordIds, oledbCommand);

                    objApproval.sDescription = "Completion Report For DTC Code " + sRecordId;
                }
                if (objApproval.sBOId == "29")
                {
                    oledbCommand = new OleDbCommand();
                    string sRecordIds = "SELECT DF_DTC_CODE || '~' || TI_INDENT_NO || '~' || WO_NO FROM TBLWORKFLOWOBJECTS,TBLINDENT,TBLWORKORDER,TBLDTCFAILURE WHERE WO_ID=:WFObjectId9 AND  WO_RECORD_ID=TI_ID AND DF_ID=WO_DF_ID AND TI_WO_SLNO=WO_SLNO";
                    oledbCommand.Parameters.AddWithValue("WFObjectId9", objApproval.sWFObjectId);
                    string sRecordId = objcon.get_value(sRecordIds, oledbCommand);

                    if (sRecordId == "")
                    {
                        oledbCommand = new OleDbCommand();
                        string qrystr = "SELECT TI_INDENT_NO || '~' || WO_NO FROM TBLWORKFLOWOBJECTS,TBLINDENT,TBLWORKORDER WHERE WO_ID=:WFObjectId10 AND  WO_RECORD_ID=TI_ID  AND TI_WO_SLNO=WO_SLNO";
                        oledbCommand.Parameters.AddWithValue("WFObjectId10", objApproval.sWFObjectId);
                        sRecordId = objcon.get_value(qrystr, oledbCommand);
                        objApproval.sDescription = "Invoice For New DTC Commission with  Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    else
                    {
                        objApproval.sDescription = "Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " with WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString();
                    }

                }

                if (objApproval.sBOId == "24")
                {
                    oledbCommand = new OleDbCommand();
                    string sOfficeCode = string.Empty;
                    if (Convert.ToInt32(objApproval.sOfficeCode) > 1)
                    {
                        sOfficeCode = objApproval.sOfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        sOfficeCode = objApproval.sOfficeCode;
                    }
                    oledbCommand = new OleDbCommand();
                    string offcode = "SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_OFF_CODE=:OfficeCode";
                    oledbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                    string sStoreName = objcon.get_value(offcode, oledbCommand);
                    objApproval.sDescription = "Inter Store Indent Request for Specified Capacity Transformer From Store Name " + sStoreName;
                }
                if (objApproval.sBOId == "32")
                {
                    oledbCommand = new OleDbCommand();
                    string sino = "SELECT SI_NO ||'~' || IS_NO FROM TBLSTOREINDENT,TBLSTOREINVOICE WHERE IS_ID=:NewRecordId AND SI_ID=IS_SI_ID";
                    oledbCommand.Parameters.AddWithValue("NewRecordId", sNewRecordId);
                    string sResult = objcon.get_value(sino, oledbCommand);

                    objApproval.sDescription = "Response for Store Indent No " + sResult.Split('~').GetValue(0).ToString() + " with Store Invoice Number " + sResult.Split('~').GetValue(1).ToString();
                }
                int Nooftimes = 0;
                Loop:
                string sMaxNo = Convert.ToString(objcon.Get_max_no("WOA_ID", "TBLWO_OBJECT_AUTO"));

                strQry = "INSERT INTO TBLWO_OBJECT_AUTO (WOA_ID,WOA_BFM_ID,WOA_PREV_APPROVE_ID,WOA_ROLE_ID,WOA_OFFICE_CODE,";
                strQry += "WOA_CRBY,WOA_DESCRIPTION,WOA_REF_OFFCODE) VALUES ('" + sMaxNo + "','" + objApproval.sBOFlowMasterId + "','" + objApproval.sWFObjectId + "',";
                strQry += " '" + objApproval.sRoleId + "','" + objApproval.sOfficeCode + "','" + objApproval.sCrby + "','" + objApproval.sDescription + "','" + objApproval.sRefOfficeCode + "')";
                try
                {
                    objcon.Execute(strQry);
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(250);
                    Nooftimes++;
                    if (Nooftimes <= 3)
                    {
                        goto Loop;
                    }
                    else
                    {
                        throw ex;
                    }
                }

                #region WCF Methods
                if (objApproval.sBOId == "29")
                {
                    try
                    {
                        bool isSuccess;
                        strQry = string.Empty;
                        DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();

                        oledbCommand = new OleDbCommand();
                        strQry = " SELECT * FROM TBLWORKORDER,TBLDTCFAILURE,TBLTCMASTER,TBLINDENT,TBLESTIMATION,TBLSTOREMAST, ";
                        strQry += " TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO  WHERE DF_EQUIPMENT_ID=TC_CODE AND WO_DF_ID=DF_ID AND WO_SLNO=TI_WO_SLNO AND EST_DF_ID=DF_ID AND ";
                        strQry += " WO_ID=WOA_PREV_APPROVE_ID AND TI_ID=WO_RECORD_ID AND SM_ID=TC_STORE_ID AND WOA_PREV_APPROVE_ID=:WFObjectId11";
                        s = strQry;
                        DataTable dtIndentDetails = new DataTable("TableIndentDetails");
                        oledbCommand.Parameters.AddWithValue("WFObjectId11", objApproval.sWFObjectId);
                        dtIndentDetails = objcon.getDataTable(strQry, oledbCommand);

                        if (dtIndentDetails.Rows.Count > 0)
                        {
                            oledbCommand = new OleDbCommand();
                            strQry = " SELECT * FROM TBLWORKORDER,TBLDTCFAILURE,TBLTCMASTER,TBLINDENT,TBLESTIMATION,TBLSTOREMAST,TBLDTCMAST,TBLOMSECMAST,TBLSUBDIVMAST, ";
                            strQry += " TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO  WHERE SUBSTR(DF_LOC_CODE,0,3)  = SD_SUBDIV_CODE AND  OM_CODE = DF_LOC_CODE AND DT_CODE = DF_DTC_CODE AND DF_EQUIPMENT_ID=TC_CODE AND WO_DF_ID=DF_ID AND WO_SLNO=TI_WO_SLNO AND EST_DF_ID=DF_ID AND ";
                            strQry += " WO_ID=WOA_PREV_APPROVE_ID AND TI_ID=WO_RECORD_ID AND SM_CODE = SUBSTR(DT_OM_SLNO , 0, 2)  AND WOA_PREV_APPROVE_ID=:WFObjectId12";
                            oledbCommand.Parameters.AddWithValue("WFObjectId12", objApproval.sWFObjectId);
                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            strQry = " SELECT '' AS DF_ID,'' AS DF_EQUIPMENT_ID,'' AS EST_NO,'' AS EST_CRON,'' AS DF_STATUS_FLAG,'' AS DF_ENHANCE_CAPACITY,WO_NEW_CAP AS TC_CAPACITY,";
                            strQry += "'' AS TC_CODE,SM_MMS_STORE_ID,WO_SLNO,WO_NO,WO_NO_DECOM,WO_DATE,WO_DESCRIPTION,WO_NEW_CAP,WO_ACC_CODE,WO_PREV_APPROVE_ID,WO_CRON,";
                            strQry += "WO_CRBY,WO_OFF_CODE,TI_ID,TI_INDENT_NO,TI_INDENT_DATE,TI_CRBY,TI_ALERT_FLAG,WO_OFFICE_CODE,WO_CR_BY,WO_WFO_ID,";
                            strQry += "WO_DATA_ID,WO_REF_OFFCODE,WOA_ID,WOA_OFFICE_CODE,WOA_CRBY,WOA_DESCRIPTION,WO_REF_OFFCODE FROM TBLWORKORDER,TBLINDENT, ";
                            strQry += " TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO,TBLSTOREMAST  WHERE WO_SLNO=TI_WO_SLNO AND SUBSTR(WO_REQUEST_LOC,0,2)=SM_CODE AND  ";
                            strQry += " WO_ID=WOA_PREV_APPROVE_ID AND TI_ID=WO_RECORD_ID AND WOA_PREV_APPROVE_ID=:WFObjectIds";
                            oledbCommand.Parameters.AddWithValue("WFObjectIds", objApproval.sWFObjectId);
                        }
                        s = strQry;

                        dtIndentDetails = objcon.getDataTable(strQry, oledbCommand);

                        //DateTime dttime = Convert.ToDateTime(dtIndentDetails.Rows[0]["WO_CRON"].ToString());
                        DateTime dttime = Convert.ToDateTime(dtIndentDetails.Rows[0]["WO_DATE"].ToString());
                        DateTime dtHost = Convert.ToDateTime(ConfigurationManager.AppSettings["dHost"].ToString());
                        oledbCommand = new OleDbCommand();
                        if (dttime < dtHost)
                        {
                            isSuccess = objWcf.SaveDtlmsTackDetails(dtIndentDetails);
                            return true;
                        }
                        else
                        {

                            objApproval.sTI_ID = dtIndentDetails.Rows[0]["TI_ID"].ToString();
                            objApproval.sWO_ID = dtIndentDetails.Rows[0]["WO_PREV_APPROVE_ID"].ToString();
                            string sdtr_code = dtIndentDetails.Rows[0]["DF_EQUIPMENT_ID"].ToString();

                            string Scrby = dtIndentDetails.Rows[0]["WO_CR_BY"].ToString();
                            Intigration_crby = Scrby;

                            strQry = "SELECT US_MMS_ID FROM TBLUSER WHERE US_ID=:Scrby";
                            oledbCommand.Parameters.AddWithValue("Scrby", Scrby);
                            Scrby = objcon.get_value(strQry, oledbCommand);

                            if (Scrby == "" || Scrby == null)
                            {
                                dtIndentDetails.Columns.Remove("WO_CR_BY");
                                dtIndentDetails.Columns.Remove("WOA_CRBY");

                                dtIndentDetails.Columns.Add("WO_CR_BY", typeof(string));
                                dtIndentDetails.Columns.Add("WOA_CRBY", typeof(string));
                            }

                            dtIndentDetails.Rows[0]["WO_CR_BY"] = Scrby;
                            dtIndentDetails.Rows[0]["WOA_CRBY"] = Scrby;
                            oledbCommand = new OleDbCommand();
                            #region getting dtr Item Code 
                            strQry = "SELECT CASE WHEN (TC_STATUS=1) THEN ITM_BRAND_NEW WHEN (TC_STATUS=3 AND (WARRENTY_TYPE='AGP')) THEN ITM_FAULTY_AGP WHEN ";
                            strQry += "(TC_STATUS=3 AND (WARRENTY_TYPE='WGP' OR WARRENTY_TYPE='WRGP')) THEN ITM_FAULTY_WGP WHEN (TC_STATUS=2) THEN ITM_REPAIR_GOOD WHEN (TC_STATUS=3 AND ";
                            strQry += "(WARRENTY_TYPE='AGP') AND TC_MAKE_ID='98') THEN ITM_FAULTY_AGP_ABB_MAKE WHEN (TC_STATUS=2 AND TC_MAKE_ID='98')THEN ";
                            strQry += "ITM_REPAIRE_GOOD_ABB_MAKE WHEN TC_STATUS='4' THEN ITM_SCRAPE  END ITEM_CODE FROM (SELECT TC_CAPACITY,TC_STATUS,";
                            strQry += "ITM_FAULTY_AGP,ITM_FAULTY_WGP,ITM_REPAIR_GOOD,ITM_BRAND_NEW,ITM_FAULTY_AGP_ABB_MAKE,ITM_REPAIRE_GOOD_ABB_MAKE,ITM_SCRAPE,";
                            strQry += "TC_MAKE_ID,(CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN (SELECT RSD_GUARRENTY_TYPE FROM (SELECT RSD_GUARRENTY_TYPE,RSD_TC_CODE,DENSE_RANK() OVER ";
                            strQry += " (PARTITION BY RSD_TC_CODE ORDER BY RSD_ID) AS RANKED_RSD_ID  FROM TBLREPAIRSENTDETAILS)A WHERE RSD_TC_CODE=TC_CODE AND RSD_GUARRENTY_TYPE IS ";
                            strQry += " NOT NULL GROUP BY RSD_GUARRENTY_TYPE,RSD_TC_CODE HAVING MIN(RANKED_RSD_ID) IN (1)) WHEN DF_GUARANTY_TYPE IS NOT NULL THEN ";
                            strQry += "DF_GUARANTY_TYPE ELSE '' END)WARRENTY_TYPE FROM TBLTCMASTER,TBLITEMPRICEMASTER,TBLDTCFAILURE WHERE ";
                            strQry += "TC_CAPACITY =ITM_CAPACITY AND TC_CODE=DF_EQUIPMENT_ID AND DF_REPLACE_FLAG=0 AND TC_CODE=:sdtr_code1)";
                            oledbCommand.Parameters.AddWithValue("sdtr_code1", sdtr_code);
                            string sDtrItemCode = objcon.get_value(strQry, oledbCommand);

                            s = strQry;
                            if (sDtrItemCode != null || sDtrItemCode != "")
                            {

                                dtIndentDetails.Columns.Add("ITEM_CODE", typeof(string));
                                dtIndentDetails.Rows[0]["ITEM_CODE"] = sDtrItemCode;

                                CustOledbConnection objconn = new CustOledbConnection(Constants.Password);
                                objconn.BeginTrans();
                                isSuccess = objWcf.SaveIndentDetails(dtIndentDetails);
                                objconn.CommitTrans();

                            }
                            else
                            {
                                isSuccess = false;
                            }
                            #endregion
                            s = s + isSuccess;
                            if (isSuccess == false)
                            {
                                //  oledbCommand = new OleDb+Command();
                                strQry = "SELECT max(wo_id)+1 FROM TBLWORKFLOWOBJECTS";
                                sMaxID = objcon.get_value(strQry);
                                sMaxID = "-" + sMaxID;

                                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + sMaxID + "',WO_USER_COMMENT='',WO_APPROVED_BY='',WO_APPROVE_STATUS='0' WHERE WO_ID='" + objApproval.sPrevApproveId + "'";
                                objcon.Execute(strQry);

                                strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "'";
                                objcon.Execute(strQry);

                                strQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";
                                objcon.Execute(strQry);

                                strQry = "DELETE FROM TBLINDENT WHERE TI_ID='" + objApproval.sNewRecordId + "'";
                                objcon.Execute(strQry);

                                return false;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        strQry = "SELECT max(wo_id)+1 FROM TBLWORKFLOWOBJECTS";
                        sMaxID = objcon.get_value(strQry);
                        sMaxID = "-" + sMaxID;

                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + sMaxID + "',WO_USER_COMMENT='',WO_APPROVED_BY='',WO_APPROVE_STATUS='0' WHERE WO_ID='" + objApproval.sPrevApproveId + "'";
                        objcon.Execute(strQry);

                        strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "'";
                        objcon.Execute(strQry);

                        strQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";
                        objcon.Execute(strQry);

                        strQry = "DELETE FROM TBLINDENT WHERE TI_ID='" + objApproval.sNewRecordId + "'";
                        objcon.Execute(strQry);

                        clsException.Intigration_LogError(ex.StackTrace, ex.Message + s, strFormCode, "Nested_SaveWFObjectAuto", Intigration_crby);
                        return false;
                    }


                }

                if (objApproval.sBOId == "15")
                {
                    DataTable dt3 = new DataTable();
                    string stm_id = string.Empty;
                    string sSecondMax = string.Empty;
                    try
                    {
                        DataTable dt2 = new DataTable();
                        DataTable dt = new DataTable();
                        oledbCommand = new OleDbCommand();
                        bool IsSuccess = false;
                        strQry = " SELECT TR_IN_NO FROM TBLTCREPLACE WHERE TR_ID =(SELECT MAX(TR_ID) FROM TBLTCREPLACE )";
                        // oledbCommand.Parameters.AddWithValue("sdtr_code", sdtr_code);
                        string res = objcon.get_value(strQry);
                        oledbCommand = new OleDbCommand();
                        strQry = "SELECT TR_ID,WO_SLNO,to_char(IN_DATE,'yyyy-mm-dd')IN_DATE,to_char(TR_RI_DATE,'yyyy-mm-dd')TR_RI_DATE,TR_CRBY,TR_DESC,SM_MMS_STORE_ID AS TR_STORE_SLNO,TR_OIL_QUNTY,DF_STATUS_FLAG,DF_DTC_CODE,DF_EQUIPMENT_ID,IN_NO,DF_ID FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,";
                        strQry += "TBLTCREPLACE,TBLDTCFAILURE,TBLSTOREMAST WHERE SM_ID=TR_STORE_SLNO AND TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND WO_DF_ID=DF_ID AND TR_ID=:NewRecordId1";
                        oledbCommand.Parameters.AddWithValue("NewRecordId1", objApproval.sNewRecordId);
                        dt = objcon.getDataTable(strQry, oledbCommand);

                        string sdtc_code = dt.Rows[0]["DF_DTC_CODE"].ToString();
                        string sdtr_code = dt.Rows[0]["DF_EQUIPMENT_ID"].ToString();
                        oledbCommand = new OleDbCommand();
                        strQry = "SELECT * FROM TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO WHERE WO_ID=(SELECT max(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID=:TRID) AND WO_ID=WOA_PREV_APPROVE_ID";
                        oledbCommand.Parameters.AddWithValue("TRID", dt.Rows[0]["TR_ID"].ToString());
                        dt2 = objcon.getDataTable(strQry, oledbCommand);
                        oledbCommand = new OleDbCommand();
                        strQry = "SELECT * FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCFAILURE,TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO WHERE ";
                        strQry += " TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND WO_DF_ID=DF_ID AND TR_ID=:NewRecordId AND WO_RECORD_ID=TR_ID AND ";
                        strQry += " WO_ID=WOA_PREV_APPROVE_ID AND WO_DATA_ID=:sdtc_code";
                        oledbCommand.Parameters.AddWithValue("NewRecordId", objApproval.sNewRecordId);
                        oledbCommand.Parameters.AddWithValue("sdtc_code", sdtc_code);
                        dt3 = objcon.getDataTable(strQry, oledbCommand);

                        DateTime dtime = Convert.ToDateTime(dt3.Rows[0]["WO_DATE"].ToString());
                        DateTime dtHost = Convert.ToDateTime(ConfigurationManager.AppSettings["dHost"].ToString()); // to differentiate old record and new record
                        oledbCommand = new OleDbCommand();
                        if (dtime < dtHost)
                        {
                            DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();
                            IsSuccess = objWcf.SaveDtlmsTackDetails(dt3);
                            return true;
                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            strQry = "SELECT max(TM_ID) FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID=:sdtc_code AND TM_LIVE_FLAG=1 ";
                            oledbCommand.Parameters.AddWithValue("sdtc_code", sdtc_code);
                            stm_id = objcon.get_value(strQry, oledbCommand);

                            oledbCommand = new OleDbCommand();
                            strQry = "SELECT max(TM_ID) FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID=:sdtc_codes AND TM_ID <>:stm_ids AND TM_LIVE_FLAG=0 ";
                            oledbCommand.Parameters.AddWithValue("sdtc_codes", sdtc_code);
                            oledbCommand.Parameters.AddWithValue("stm_ids", stm_id);
                            sSecondMax = objcon.get_value(strQry, oledbCommand);

                            oledbCommand = new OleDbCommand();
                            #region getting dtr Item Code 
                            strQry = "SELECT CASE WHEN (TC_STATUS=1) THEN ITM_BRAND_NEW WHEN (TC_STATUS=3 AND (WARRENTY_TYPE='AGP')) THEN ITM_FAULTY_AGP WHEN ";
                            strQry += "(TC_STATUS=3 AND (WARRENTY_TYPE='WGP')) THEN ITM_FAULTY_WGP WHEN (TC_STATUS=2) THEN ITM_REPAIR_GOOD WHEN (TC_STATUS=3 AND ";
                            strQry += "(WARRENTY_TYPE='AGP') AND TC_MAKE_ID='98') THEN ITM_FAULTY_AGP_ABB_MAKE WHEN (TC_STATUS=2 AND TC_MAKE_ID='98')THEN ";
                            strQry += "ITM_REPAIRE_GOOD_ABB_MAKE WHEN TC_STATUS='4' THEN ITM_SCRAPE  END ITEM_CODE FROM (SELECT TC_CAPACITY,TC_STATUS,";
                            strQry += "ITM_FAULTY_AGP,ITM_FAULTY_WGP,ITM_REPAIR_GOOD,ITM_BRAND_NEW,ITM_FAULTY_AGP_ABB_MAKE,ITM_REPAIRE_GOOD_ABB_MAKE,ITM_SCRAPE,";
                            strQry += "TC_MAKE_ID,(CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN (SELECT RSD_GUARRENTY_TYPE FROM (SELECT RSD_ID,RSD_GUARRENTY_TYPE, ";
                            strQry += " row_number() over (order by RSD_ID desc) AS RNUM  FROM TBLREPAIRSENTDETAILS,TBLTCMASTER  WHERE  RSD_TC_CODE = TC_CODE AND RSD_GUARRENTY_TYPE ";
                            strQry += " IS NOT NULL) WHERE RNUM=1) WHEN DF_GUARANTY_TYPE IS NOT NULL THEN ";
                            strQry += "DF_GUARANTY_TYPE ELSE '' END) WARRENTY_TYPE FROM TBLTCMASTER,TBLITEMPRICEMASTER,TBLDTCFAILURE WHERE ";
                            strQry += "TC_CAPACITY =ITM_CAPACITY AND TC_CODE=DF_EQUIPMENT_ID AND DF_REPLACE_FLAG=0 AND TC_CODE=:sdtr_codes)";
                            oledbCommand.Parameters.AddWithValue("sdtr_codes", sdtr_code);
                            string sDtrItemCode = objcon.get_value(strQry, oledbCommand);
                            #endregion

                            DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();
                            DataTable RiDetails = new DataTable("Ridetails");

                            RiDetails.Columns.Add("FORMNAME", typeof(string));

                            #region RI Details
                            RiDetails.Columns.Add("TR_ID", typeof(string));
                            RiDetails.Columns.Add("WO_SLNO", typeof(string));
                            RiDetails.Columns.Add("TR_IN_NO", typeof(string));
                            RiDetails.Columns.Add("IN_DATE", typeof(string));
                            RiDetails.Columns.Add("TR_RI_DATE", typeof(string));
                            RiDetails.Columns.Add("TR_CRBY", typeof(string));
                            RiDetails.Columns.Add("TR_DESC", typeof(string));
                            RiDetails.Columns.Add("TR_STORE_SLNO", typeof(string));
                            RiDetails.Columns.Add("TR_OIL_QUNTY", typeof(string));
                            RiDetails.Columns.Add("DF_STATUS_FLAG", typeof(string));
                            RiDetails.Columns.Add("IN_NO", typeof(string));
                            RiDetails.Columns.Add("DF_ID", typeof(string));

                            #endregion

                            #region TBLWORKFLOWOBJECTS details

                            RiDetails.Columns.Add("WO_BO_ID", typeof(string));
                            RiDetails.Columns.Add("WO_RECORD_ID", typeof(string));
                            RiDetails.Columns.Add("WO_PREV_APPROVE_ID", typeof(string));
                            RiDetails.Columns.Add("WO_NEXT_ROLE", typeof(string));
                            RiDetails.Columns.Add("WO_OFFICE_CODE", typeof(string));
                            RiDetails.Columns.Add("WO_USER_COMMENT", typeof(string));
                            RiDetails.Columns.Add("WO_APPROVED_BY", typeof(string));
                            RiDetails.Columns.Add("WO_APPROVE_STATUS", typeof(string));
                            RiDetails.Columns.Add("WO_CR_BY", typeof(string));
                            RiDetails.Columns.Add("WO_CR_ON", typeof(string));
                            RiDetails.Columns.Add("WO_RECORD_BY", typeof(string));
                            RiDetails.Columns.Add("WO_DEVICE_ID", typeof(string));
                            RiDetails.Columns.Add("WO_DESCRIPTION", typeof(string));
                            RiDetails.Columns.Add("WO_WFO_ID", typeof(string));
                            RiDetails.Columns.Add("WO_INITIAL_ID", typeof(string));
                            RiDetails.Columns.Add("WO_DATA_ID", typeof(string));
                            RiDetails.Columns.Add("WO_REF_OFFCODE", typeof(string));

                            #endregion

                            #region TBLWO_OBJECT_AUTO details

                            RiDetails.Columns.Add("WOA_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_BFM_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_PREV_APPROVE_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_ROLE_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_OFFICE_CODE", typeof(string));
                            RiDetails.Columns.Add("WOA_INITIAL_ACTION_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_DESCRIPTION", typeof(string));
                            RiDetails.Columns.Add("WOA_CRBY", typeof(string));
                            RiDetails.Columns.Add("WOA_CRON", typeof(string));
                            RiDetails.Columns.Add("WOA_REF_OFFCODE", typeof(string));

                            RiDetails.Columns.Add("DTR_ITEM_CODE", typeof(string));
                            RiDetails.Columns.Add("DF_DTR_COMMISSION_DATE", typeof(string));
                            RiDetails.Columns.Add("DF_DATE", typeof(string));
                            #endregion
                            oledbCommand = new OleDbCommand();
                            string scrby = dt2.Rows[0]["WO_CR_BY"].ToString();
                            Intigration_crby = scrby;
                            strQry = "SELECT US_MMS_ID FROM TBLUSER WHERE US_ID=:scrby1";
                            oledbCommand.Parameters.AddWithValue("scrby1", scrby);
                            scrby = objcon.get_value(strQry, oledbCommand);

                            if (scrby == "" || scrby == null)
                            {
                                RiDetails.Columns.Remove("WO_CR_BY");
                                RiDetails.Columns.Remove("WOA_CRBY");

                                RiDetails.Columns.Add("WO_CR_BY", typeof(string));
                                RiDetails.Columns.Add("WOA_CRBY", typeof(string));
                            }

                            DataRow dtrow = RiDetails.NewRow();
                            dtrow["FORMNAME"] = "ReturnInvoice";

                            dtrow["TR_ID"] = Convert.ToString(dt.Rows[0]["TR_ID"]);
                            dtrow["WO_SLNO"] = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                            dtrow["TR_IN_NO"] = res;
                            dtrow["IN_DATE"] = Convert.ToString(dt.Rows[0]["IN_DATE"]);
                            dtrow["TR_RI_DATE"] = Convert.ToString(dt.Rows[0]["TR_RI_DATE"]);
                            dtrow["TR_CRBY"] = Convert.ToString(dt.Rows[0]["TR_CRBY"]);
                            dtrow["TR_DESC"] = Convert.ToString(dt.Rows[0]["TR_DESC"]);
                            dtrow["TR_STORE_SLNO"] = Convert.ToString(dt.Rows[0]["TR_STORE_SLNO"]);
                            dtrow["TR_OIL_QUNTY"] = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                            dtrow["DF_STATUS_FLAG"] = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                            dtrow["IN_NO"] = Convert.ToString(dt.Rows[0]["IN_NO"]);
                            dtrow["DF_ID"] = Convert.ToString(dt.Rows[0]["DF_ID"]);

                            dtrow["WO_BO_ID"] = Convert.ToString(dt2.Rows[0]["WO_BO_ID"]);
                            dtrow["WO_RECORD_ID"] = Convert.ToString(dt2.Rows[0]["WO_RECORD_ID"]);
                            dtrow["WO_PREV_APPROVE_ID"] = Convert.ToString(dt2.Rows[0]["WO_PREV_APPROVE_ID"]);
                            dtrow["WO_NEXT_ROLE"] = Convert.ToString(dt2.Rows[0]["WO_NEXT_ROLE"]);
                            dtrow["WO_OFFICE_CODE"] = Convert.ToString(dt2.Rows[0]["WO_OFFICE_CODE"]);
                            dtrow["WO_CR_ON"] = Convert.ToString(dt2.Rows[0]["WO_CR_ON"]);
                            dtrow["WO_USER_COMMENT"] = Convert.ToString(dt2.Rows[0]["WO_USER_COMMENT"]);
                            dtrow["WO_APPROVED_BY"] = Convert.ToString(dt2.Rows[0]["WO_APPROVED_BY"]);
                            dtrow["WO_APPROVE_STATUS"] = Convert.ToString(dt2.Rows[0]["WO_APPROVE_STATUS"]);
                            dtrow["WO_CR_BY"] = scrby;
                            dtrow["WO_RECORD_BY"] = Convert.ToString(dt2.Rows[0]["WO_RECORD_BY"]);
                            dtrow["WO_DEVICE_ID"] = Convert.ToString(dt2.Rows[0]["WO_DEVICE_ID"]);
                            dtrow["WO_DESCRIPTION"] = Convert.ToString(dt2.Rows[0]["WO_DESCRIPTION"]);
                            dtrow["WO_WFO_ID"] = Convert.ToString(dt2.Rows[0]["WO_WFO_ID"]);
                            dtrow["WO_INITIAL_ID"] = Convert.ToString(dt2.Rows[0]["WO_INITIAL_ID"]);
                            dtrow["WO_DATA_ID"] = Convert.ToString(dt.Rows[0]["TR_ID"]);
                            dtrow["WO_REF_OFFCODE"] = Convert.ToString(dt2.Rows[0]["WO_REF_OFFCODE"]);

                            dtrow["WOA_ID"] = Convert.ToString(dt2.Rows[0]["WOA_ID"]);
                            dtrow["WOA_BFM_ID"] = Convert.ToString(dt2.Rows[0]["WOA_BFM_ID"]);
                            dtrow["WOA_PREV_APPROVE_ID"] = Convert.ToString(dt2.Rows[0]["WOA_PREV_APPROVE_ID"]);
                            dtrow["WOA_ROLE_ID"] = Convert.ToString(dt2.Rows[0]["WOA_ROLE_ID"]);
                            dtrow["WOA_OFFICE_CODE"] = Convert.ToString(dt2.Rows[0]["WOA_OFFICE_CODE"]);
                            dtrow["WOA_INITIAL_ACTION_ID"] = Convert.ToString(dt2.Rows[0]["WOA_INITIAL_ACTION_ID"]);
                            dtrow["WOA_DESCRIPTION"] = Convert.ToString(dt2.Rows[0]["WOA_DESCRIPTION"]);
                            dtrow["WOA_CRBY"] = scrby;
                            dtrow["WOA_CRON"] = Convert.ToString(dt2.Rows[0]["WOA_CRON"]);
                            dtrow["WOA_REF_OFFCODE"] = Convert.ToString(dt2.Rows[0]["WOA_REF_OFFCODE"]);
                            DateTime tempDate;
                            if (dt3.Rows[0]["DF_DTR_COMMISSION_DATE"].ToString() != null)
                            {
                                tempDate = Convert.ToDateTime(Convert.ToString(dt3.Rows[0]["DF_DTR_COMMISSION_DATE"]).Replace('-', '/'));
                                DateTime dDtrCommissiondate = DateTime.Parse(tempDate.ToString());
                                string sDtrCommisiondate = Convert.ToDateTime(dDtrCommissiondate).ToString("MM/dd/yyyy");
                                dtrow["DF_DTR_COMMISSION_DATE"] = sDtrCommisiondate;
                            }
                            tempDate = Convert.ToDateTime(Convert.ToString(dt3.Rows[0]["DF_DATE"]).Replace('-', '/'));
                            DateTime dfailuredate = DateTime.Parse(tempDate.ToString());
                            string sfailuredate = Convert.ToDateTime(dfailuredate).ToString("MM/dd/yyyy");
                            dtrow["DF_DATE"] = sfailuredate;
                            dtrow["DTR_ITEM_CODE"] = sDtrItemCode;
                            RiDetails.Rows.Add(dtrow);
                            oledbCommand = new OleDbCommand();
                            IsSuccess = objWcf.SaveRVData(RiDetails);
                            if (IsSuccess == false)
                            {
                                strQry = "SELECT max(wo_id)+1 FROM TBLWORKFLOWOBJECTS";
                                sMaxID = objcon.get_value(strQry);
                                sMaxID = "-" + sMaxID;

                                strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID = '" + dt3.Rows[0]["WO_ID"].ToString() + "'";
                                objcon.Execute(strQry);

                                strQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_ID = '" + dt3.Rows[0]["WOA_ID"].ToString() + "'";
                                objcon.Execute(strQry);

                                strQry = "DELETE FROM TBLTCREPLACE WHERE TR_ID = '" + dt3.Rows[0]["TR_ID"].ToString() + "'";
                                objcon.Execute(strQry);

                                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + sMaxID + "', WO_USER_COMMENT = '', WO_APPROVED_BY = '', WO_APPROVE_STATUS = '0' WHERE WO_ID = '" + dt3.Rows[0]["WO_PREV_APPROVE_ID"].ToString() + "'";
                                objcon.Execute(strQry);

                                strQry = "DELETE TBLTRANSDTCMAPPING WHERE TM_ID='" + stm_id + "'";
                                objcon.Execute(strQry);

                                strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG=1 WHERE TM_ID='" + sSecondMax + "'";
                                objcon.Execute(strQry);

                                return false;
                            }
                        }


                    }
                    catch (Exception ex)
                    {

                        strQry = "SELECT max(wo_id)+1 FROM TBLWORKFLOWOBJECTS";
                        sMaxID = objcon.get_value(strQry);
                        sMaxID = "-" + sMaxID;

                        strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID = '" + dt3.Rows[0]["WO_ID"].ToString() + "'";
                        objcon.Execute(strQry);

                        strQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_ID = '" + dt3.Rows[0]["WOA_ID"].ToString() + "'";
                        objcon.Execute(strQry);

                        strQry = "DELETE FROM TBLTCREPLACE WHERE TR_ID = '" + dt3.Rows[0]["TR_ID"].ToString() + "'";
                        objcon.Execute(strQry);

                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + sMaxID + "', WO_USER_COMMENT = '', WO_APPROVED_BY = '', WO_APPROVE_STATUS = '0' WHERE WO_ID = '" + dt3.Rows[0]["WO_PREV_APPROVE_ID"].ToString() + "'";
                        objcon.Execute(strQry);

                        strQry = "DELETE TBLTRANSDTCMAPPING WHERE TM_ID='" + stm_id + "'";
                        objcon.Execute(strQry);

                        strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG=1 WHERE TM_ID='" + sSecondMax + "'";
                        objcon.Execute(strQry);

                        clsException.Intigration_LogError(ex.StackTrace, ex.Message, strFormCode, "Nested_SaveWFObjectAuto", Intigration_crby);

                        return false;
                    }

                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }


        public bool SaveWFObjectAuto1(clsApproval objApproval, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();
            Intigration_crby = objApproval.sCrby;
            string s = string.Empty;
            try
            {

                string strQry = string.Empty;

                string sMaxNo = Convert.ToString(objconn.Get_max_no("WOA_ID", "TBLWO_OBJECT_AUTO"));
                if (objApproval.sBOId == "25" || objApproval.sBOId == "11")
                {
                    string qry = "SELECT DF_DTC_CODE FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE WHERE WO_ID='" + objApproval.sWFObjectId + "' AND DF_ID=WO_RECORD_ID";
                    // oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                    string sRecordId = objconn.get_value(qry);
                    objApproval.sDescription = "Work Order For DTC Code " + sRecordId;
                }
                if (objApproval.sBOId == "12")
                {
                    oledbCommand = new OleDbCommand();
                    string qry = "SELECT DF_DTC_CODE || '~' || WO_NO FROM TBLWORKFLOWOBJECTS,TBLWORKORDER,TBLDTCFAILURE WHERE WO_ID='" + objApproval.sWFObjectId + "' AND  WO_RECORD_ID=WO_SLNO AND DF_ID=WO_DF_ID";
                    // oledbCommand.Parameters.AddWithValue("WFObjectId1", objApproval.sWFObjectId);
                    string sRecordId = objconn.get_value(qry);
                    oledbCommand = new OleDbCommand();
                    if (sRecordId == "")
                    {
                        string qrys = "SELECT  WO_NO FROM TBLWORKFLOWOBJECTS,TBLWORKORDER WHERE WO_ID='" + objApproval.sWFObjectId + "' AND  WO_RECORD_ID=WO_SLNO";
                        // oledbCommand.Parameters.AddWithValue("WFObjectId2", objApproval.sWFObjectId);
                        sRecordId = objconn.get_value(qrys);

                        objApproval.sDescription = "Indent For New DTC Commission with WO No " + sRecordId;
                    }
                    else
                    {
                        objApproval.sDescription = "Indent For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " and WO No " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                }
                if (objApproval.sBOId == "13")
                {
                    oledbCommand = new OleDbCommand();

                    string record = "SELECT DF_DTC_CODE || '~' || TI_INDENT_NO || '~' || WO_NO || '~' || DT_NAME FROM TBLWORKFLOWOBJECTS,TBLINDENT,TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST WHERE WO_ID='" + objApproval.sWFObjectId + "' AND  WO_RECORD_ID=TI_ID AND DF_ID=WO_DF_ID AND TI_WO_SLNO=WO_SLNO AND DT_CODE=DF_DTC_CODE";
                    // oledbCommand.Parameters.AddWithValue("WFObjectId3", objApproval.sWFObjectId);
                    string sRecordId = objconn.get_value(record);
                    oledbCommand = new OleDbCommand();
                    if (sRecordId == "")
                    {
                        string sDescription = "SELECT TI_INDENT_NO || '~' || WO_NO FROM TBLWORKFLOWOBJECTS,TBLINDENT,TBLWORKORDER WHERE WO_ID='" + objApproval.sWFObjectId + "' AND  WO_RECORD_ID=TI_ID AND TI_WO_SLNO=WO_SLNO";
                        // oledbCommand.Parameters.AddWithValue("WFObjectId4", objApproval.sWFObjectId);
                        sRecordId = objconn.get_value(sDescription);
                        objApproval.sDescription = "Invoice For New DTC Commission with Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    else
                    {
                        objApproval.sDescription = "Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + ", DTC Name " + sRecordId.Split('~').GetValue(3).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString();
                    }
                }
                if (objApproval.sBOId == "14")
                {
                    oledbCommand = new OleDbCommand();
                    string qry = "SELECT DF_DTC_CODE FROM TBLTCDRAWN,TBLDTCFAILURE,TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "' AND TD_DF_ID=DF_ID AND  WO_RECORD_ID=TD_INV_NO ";
                    // oledbCommand.Parameters.AddWithValue("WFObjectId5", objApproval.sWFObjectId);
                    string sRecordId = objconn.get_value(qry);

                    objApproval.sDescription = "Decommissioning For DTC Code " + sRecordId;
                    oledbCommand = new OleDbCommand();
                    if (sRecordId == "")
                    {
                        string RecordIdqry = "SELECT IN_INV_NO FROM TBLDTCINVOICE,TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "' AND WO_RECORD_ID=IN_NO ";
                        //oledbCommand.Parameters.AddWithValue("WFObjectId6", objApproval.sWFObjectId);
                        sRecordId = objconn.get_value(RecordIdqry);
                        objApproval.sDescription = "Commissioning of DTC for the Invoice NO " + sRecordId;
                    }
                }
                if (objApproval.sBOId == "15")
                {
                    oledbCommand = new OleDbCommand();
                    string sQry = "SELECT  DF_EQUIPMENT_ID || '~' || WO_NO_DECOM || '~' || DF_DTC_CODE FROM TBLTCDRAWN,TBLDTCFAILURE,TBLWORKFLOWOBJECTS,TBLTCREPLACE,TBLWORKORDER WHERE WO_ID='" + objApproval.sWFObjectId + "' ";
                    sQry += " AND TD_DF_ID=DF_ID AND  WO_RECORD_ID=TR_ID AND TR_IN_NO=TD_INV_NO AND DF_ID=WO_DF_ID";
                    //  oledbCommand.Parameters.AddWithValue("WFObjectId7", objApproval.sWFObjectId);
                    string sRecordId = objconn.get_value(sQry);
                    objApproval.sDescription = "RI Approval For DTC Code " + sRecordId.Split('~').GetValue(2).ToString() + " with DTr Code " + sRecordId.Split('~').GetValue(0).ToString() + " and Work Order NO " + sRecordId.Split('~').GetValue(1).ToString();

                }
                if (objApproval.sBOId == "26")
                {
                    oledbCommand = new OleDbCommand();
                    string sRecordIds = "SELECT DF_DTC_CODE FROM TBLTCDRAWN,TBLDTCFAILURE,TBLWORKFLOWOBJECTS,TBLTCREPLACE WHERE WO_ID='" + objApproval.sWFObjectId + "' AND TD_DF_ID=DF_ID AND  WO_RECORD_ID=TR_ID AND TR_IN_NO=TD_INV_NO";
                    // oledbCommand.Parameters.AddWithValue("WFObjectId8", objApproval.sWFObjectId);
                    string sRecordId = objconn.get_value(sRecordIds);

                    objApproval.sDescription = "Completion Report For DTC Code " + sRecordId;
                }
                if (objApproval.sBOId == "29")
                {
                    oledbCommand = new OleDbCommand();
                    string sRecordIds = "SELECT DF_DTC_CODE || '~' || TI_INDENT_NO || '~' || WO_NO FROM TBLWORKFLOWOBJECTS,TBLINDENT,TBLWORKORDER,TBLDTCFAILURE WHERE WO_ID='" + objApproval.sWFObjectId + "' AND  WO_RECORD_ID=TI_ID AND DF_ID=WO_DF_ID AND TI_WO_SLNO=WO_SLNO";
                    // oledbCommand.Parameters.AddWithValue("WFObjectId9", objApproval.sWFObjectId);
                    string sRecordId = objconn.get_value(sRecordIds);

                    if (sRecordId == "")
                    {
                        oledbCommand = new OleDbCommand();
                        string qrystr = "SELECT TI_INDENT_NO || '~' || WO_NO FROM TBLWORKFLOWOBJECTS,TBLINDENT,TBLWORKORDER WHERE WO_ID='" + objApproval.sWFObjectId + "' AND  WO_RECORD_ID=TI_ID  AND TI_WO_SLNO=WO_SLNO";
                        // oledbCommand.Parameters.AddWithValue("WFObjectId10", objApproval.sWFObjectId);
                        sRecordId = objconn.get_value(qrystr);
                        objApproval.sDescription = "Invoice For New DTC Commission with  Indent No " + sRecordId.Split('~').GetValue(0).ToString() + " and WorkOrder No " + sRecordId.Split('~').GetValue(1).ToString();
                    }
                    else
                    {
                        objApproval.sDescription = "Invoice For DTC Code " + sRecordId.Split('~').GetValue(0).ToString() + " with WorkOrder No " + sRecordId.Split('~').GetValue(2).ToString() + " and Indent No " + sRecordId.Split('~').GetValue(1).ToString();
                    }

                }

                if (objApproval.sBOId == "24")
                {
                    oledbCommand = new OleDbCommand();
                    string sOfficeCode = string.Empty;
                    if (Convert.ToInt32(objApproval.sOfficeCode) > 1)
                    {
                        sOfficeCode = objApproval.sOfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        sOfficeCode = objApproval.sOfficeCode;
                    }
                    oledbCommand = new OleDbCommand();
                    string offcode = "SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_OFF_CODE='" + sOfficeCode + "'";
                    // oledbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                    string sStoreName = objconn.get_value(offcode);
                    objApproval.sDescription = "Inter Store Indent Request for Specified Capacity Transformer From Store Name " + sStoreName;
                }
                if (objApproval.sBOId == "32")
                {
                    oledbCommand = new OleDbCommand();
                    string sino = "SELECT SI_NO ||'~' || IS_NO FROM TBLSTOREINDENT,TBLSTOREINVOICE WHERE IS_ID='" + sNewRecordId + "' AND SI_ID=IS_SI_ID";
                    // oledbCommand.Parameters.AddWithValue("NewRecordId", sNewRecordId);
                    string sResult = objconn.get_value(sino);

                    objApproval.sDescription = "Response for Store Indent No " + sResult.Split('~').GetValue(0).ToString() + " with Store Invoice Number " + sResult.Split('~').GetValue(1).ToString();
                }

                strQry = "INSERT INTO TBLWO_OBJECT_AUTO (WOA_ID,WOA_BFM_ID,WOA_PREV_APPROVE_ID,WOA_ROLE_ID,WOA_OFFICE_CODE,";
                strQry += "WOA_CRBY,WOA_DESCRIPTION,WOA_REF_OFFCODE) VALUES ('" + sMaxNo + "','" + objApproval.sBOFlowMasterId + "','" + objApproval.sWFObjectId + "',";
                strQry += " '" + objApproval.sRoleId + "','" + objApproval.sOfficeCode + "','" + objApproval.sCrby + "','" + objApproval.sDescription + "','" + objApproval.sRefOfficeCode + "')";
                objconn.Execute(strQry);

                #region WCF Methods
                if (objApproval.sBOId == "29")
                {
                    try
                    {
                        bool isSuccess;
                        strQry = string.Empty;
                        DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();

                        ///////////Update TBLWO_OBJECT_AUTO(WOA_FLAG) so (DTLMS) STO will not Get the PseudoIndent Record To Approve////////////
                        //strQry = "UPDATE TBLWO_OBJECT_AUTO SET WOA_FLAG='1' WHERE WOA_ID='" + sMaxNo + "'";
                        //objcon.Execute(strQry);
                        ///////////////////////////Update TBLWO_OBJECT_AUTO////////////////////////////////////

                        oledbCommand = new OleDbCommand();
                        strQry = " SELECT * FROM TBLWORKORDER,TBLDTCFAILURE,TBLTCMASTER,TBLINDENT,TBLESTIMATION,TBLSTOREMAST, ";
                        strQry += " TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO  WHERE DF_EQUIPMENT_ID=TC_CODE AND WO_DF_ID=DF_ID AND WO_SLNO=TI_WO_SLNO AND EST_DF_ID=DF_ID AND ";
                        strQry += " WO_ID=WOA_PREV_APPROVE_ID AND TI_ID=WO_RECORD_ID AND SM_ID=TC_STORE_ID AND WOA_PREV_APPROVE_ID=:WFObjectId11";
                        s = strQry;
                        DataTable dtIndentDetails = new DataTable("TableIndentDetails");
                        oledbCommand.Parameters.AddWithValue("WFObjectId11", objApproval.sWFObjectId);
                        dtIndentDetails = objcon.getDataTable(strQry, oledbCommand);

                        if (dtIndentDetails.Rows.Count > 0)
                        {
                            oledbCommand = new OleDbCommand();
                            strQry = " SELECT * FROM TBLWORKORDER,TBLDTCFAILURE,TBLTCMASTER,TBLINDENT,TBLESTIMATION,TBLSTOREMAST,TBLDTCMAST,TBLOMSECMAST,TBLSUBDIVMAST, ";
                            strQry += " TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO  WHERE SUBSTR(DF_LOC_CODE,0,3)  = SD_SUBDIV_CODE AND  OM_CODE = DF_LOC_CODE AND DT_CODE = DF_DTC_CODE AND DF_EQUIPMENT_ID=TC_CODE AND WO_DF_ID=DF_ID AND WO_SLNO=TI_WO_SLNO AND EST_DF_ID=DF_ID AND ";
                            strQry += " WO_ID=WOA_PREV_APPROVE_ID AND TI_ID=WO_RECORD_ID AND SM_CODE = SUBSTR(DT_OM_SLNO , 0, 2)  AND WOA_PREV_APPROVE_ID=:WFObjectId12";
                            oledbCommand.Parameters.AddWithValue("WFObjectId12", objApproval.sWFObjectId);
                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            strQry = " SELECT '' AS DF_ID,'' AS DF_EQUIPMENT_ID,'' AS EST_NO,'' AS EST_CRON,'' AS DF_STATUS_FLAG,'' AS DF_ENHANCE_CAPACITY,WO_NEW_CAP AS TC_CAPACITY,";
                            strQry += "'' AS TC_CODE,SM_MMS_STORE_ID,WO_SLNO,WO_NO,WO_NO_DECOM,WO_DATE,WO_DESCRIPTION,WO_NEW_CAP,WO_ACC_CODE,WO_PREV_APPROVE_ID,WO_CRON,";
                            strQry += "WO_CRBY,WO_OFF_CODE,TI_ID,TI_INDENT_NO,TI_INDENT_DATE,TI_CRBY,TI_ALERT_FLAG,WO_OFFICE_CODE,WO_CR_BY,WO_WFO_ID,";
                            strQry += "WO_DATA_ID,WO_REF_OFFCODE,WOA_ID,WOA_OFFICE_CODE,WOA_CRBY,WOA_DESCRIPTION,WO_REF_OFFCODE FROM TBLWORKORDER,TBLINDENT, ";
                            strQry += " TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO,TBLSTOREMAST  WHERE WO_SLNO=TI_WO_SLNO AND SUBSTR(WO_REQUEST_LOC,0,2)=SM_CODE AND  ";
                            strQry += " WO_ID=WOA_PREV_APPROVE_ID AND TI_ID=WO_RECORD_ID AND WOA_PREV_APPROVE_ID=:WFObjectIds";
                            oledbCommand.Parameters.AddWithValue("WFObjectIds", objApproval.sWFObjectId);
                        }
                        s = strQry;

                        dtIndentDetails = objcon.getDataTable(strQry, oledbCommand);

                        //DateTime dttime = Convert.ToDateTime(dtIndentDetails.Rows[0]["WO_CRON"].ToString());
                        DateTime dttime = Convert.ToDateTime(dtIndentDetails.Rows[0]["WO_DATE"].ToString());
                        DateTime dtHost = Convert.ToDateTime(ConfigurationSettings.AppSettings["dHost"].ToString());
                        oledbCommand = new OleDbCommand();
                        if (dttime < dtHost)
                        {
                            isSuccess = objWcf.SaveDtlmsTackDetails(dtIndentDetails);
                            return true;
                        }
                        else
                        {

                            objApproval.sTI_ID = dtIndentDetails.Rows[0]["TI_ID"].ToString();
                            objApproval.sWO_ID = dtIndentDetails.Rows[0]["WO_PREV_APPROVE_ID"].ToString();
                            string sdtr_code = dtIndentDetails.Rows[0]["DF_EQUIPMENT_ID"].ToString();

                            string Scrby = dtIndentDetails.Rows[0]["WO_CR_BY"].ToString();
                            Intigration_crby = Scrby;

                            strQry = "SELECT US_MMS_ID FROM TBLUSER WHERE US_ID='" + Scrby + "'";
                            // oledbCommand.Parameters.AddWithValue("Scrby", Scrby);
                            Scrby = objcon.get_value(strQry);

                            if (Scrby == "" || Scrby == null)
                            {
                                dtIndentDetails.Columns.Remove("WO_CR_BY");
                                dtIndentDetails.Columns.Remove("WOA_CRBY");

                                dtIndentDetails.Columns.Add("WO_CR_BY", typeof(string));
                                dtIndentDetails.Columns.Add("WOA_CRBY", typeof(string));
                            }

                            dtIndentDetails.Rows[0]["WO_CR_BY"] = Scrby;
                            dtIndentDetails.Rows[0]["WOA_CRBY"] = Scrby;
                            oledbCommand = new OleDbCommand();
                            #region getting dtr Item Code 
                            strQry = "SELECT CASE WHEN (TC_STATUS=1) THEN ITM_BRAND_NEW WHEN (TC_STATUS=3 AND (WARRENTY_TYPE='AGP')) THEN ITM_FAULTY_AGP WHEN ";
                            strQry += "(TC_STATUS=3 AND (WARRENTY_TYPE='WGP' OR WARRENTY_TYPE='WRGP')) THEN ITM_FAULTY_WGP WHEN (TC_STATUS=2) THEN ITM_REPAIR_GOOD WHEN (TC_STATUS=3 AND ";
                            strQry += "(WARRENTY_TYPE='AGP') AND TC_MAKE_ID='98') THEN ITM_FAULTY_AGP_ABB_MAKE WHEN (TC_STATUS=2 AND TC_MAKE_ID='98')THEN ";
                            strQry += "ITM_REPAIRE_GOOD_ABB_MAKE WHEN TC_STATUS='4' THEN ITM_SCRAPE  END ITEM_CODE FROM (SELECT TC_CAPACITY,TC_STATUS,";
                            strQry += "ITM_FAULTY_AGP,ITM_FAULTY_WGP,ITM_REPAIR_GOOD,ITM_BRAND_NEW,ITM_FAULTY_AGP_ABB_MAKE,ITM_REPAIRE_GOOD_ABB_MAKE,ITM_SCRAPE,";
                            strQry += "TC_MAKE_ID,(CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN (SELECT RSD_GUARRENTY_TYPE FROM (SELECT RSD_GUARRENTY_TYPE,RSD_TC_CODE,DENSE_RANK() OVER ";
                            strQry += " (PARTITION BY RSD_TC_CODE ORDER BY RSD_ID) AS RANKED_RSD_ID  FROM TBLREPAIRSENTDETAILS)A WHERE RSD_TC_CODE=TC_CODE AND RSD_GUARRENTY_TYPE IS ";
                            strQry += " NOT NULL GROUP BY RSD_GUARRENTY_TYPE,RSD_TC_CODE HAVING MIN(RANKED_RSD_ID) IN (1)) WHEN DF_GUARANTY_TYPE IS NOT NULL THEN ";
                            strQry += "DF_GUARANTY_TYPE ELSE '' END)WARRENTY_TYPE FROM TBLTCMASTER,TBLITEMPRICEMASTER,TBLDTCFAILURE WHERE ";
                            strQry += "TC_CAPACITY =ITM_CAPACITY AND TC_CODE=DF_EQUIPMENT_ID AND DF_REPLACE_FLAG=0 AND TC_CODE='" + sdtr_code + "')";
                            //  oledbCommand.Parameters.AddWithValue("sdtr_code1", sdtr_code);
                            string sDtrItemCode = objconn.get_value(strQry);

                            s = strQry;
                            if (sDtrItemCode != null || sDtrItemCode != "")
                            {
                                dtIndentDetails.Columns.Add("ITEM_CODE", typeof(string));
                                dtIndentDetails.Rows[0]["ITEM_CODE"] = sDtrItemCode;
                                isSuccess = objWcf.SaveIndentDetails(dtIndentDetails);
                            }
                            else
                            {
                                isSuccess = false;
                            }
                            #endregion
                            s = s + isSuccess;
                            if (isSuccess == false)
                            {
                                //  oledbCommand = new OleDb+Command();
                                strQry = "SELECT max(wo_id)+1 FROM TBLWORKFLOWOBJECTS";
                                sMaxID = objconn.get_value(strQry);
                                sMaxID = "-" + sMaxID;

                                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + sMaxID + "',WO_USER_COMMENT='',WO_APPROVED_BY='',WO_APPROVE_STATUS='0' WHERE WO_ID='" + objApproval.sPrevApproveId + "'";
                                objconn.Execute(strQry);

                                strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "'";
                                objconn.Execute(strQry);

                                strQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";
                                objconn.Execute(strQry);

                                strQry = "DELETE FROM TBLINDENT WHERE TI_ID='" + objApproval.sNewRecordId + "'";
                                objconn.Execute(strQry);

                                return false;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        strQry = "SELECT max(wo_id)+1 FROM TBLWORKFLOWOBJECTS";
                        sMaxID = objconn.get_value(strQry);
                        sMaxID = "-" + sMaxID;

                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + sMaxID + "',WO_USER_COMMENT='',WO_APPROVED_BY='',WO_APPROVE_STATUS='0' WHERE WO_ID='" + objApproval.sPrevApproveId + "'";
                        objconn.Execute(strQry);

                        strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId + "'";
                        objconn.Execute(strQry);

                        strQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_PREV_APPROVE_ID='" + objApproval.sWFObjectId + "'";
                        objconn.Execute(strQry);

                        strQry = "DELETE FROM TBLINDENT WHERE TI_ID='" + objApproval.sNewRecordId + "'";
                        objconn.Execute(strQry);

                        clsException.Intigration_LogError(ex.StackTrace, ex.Message + s, strFormCode, "Nested_SaveWFObjectAuto", Intigration_crby);
                        return false;
                    }


                }

                if (objApproval.sBOId == "15")
                {
                    DataTable dt3 = new DataTable();
                    string stm_id = string.Empty;
                    string sSecondMax = string.Empty;
                    try
                    {
                        DataTable dt2 = new DataTable();
                        DataTable dt = new DataTable();
                        oledbCommand = new OleDbCommand();
                        bool IsSuccess = false;
                        strQry = " SELECT TR_IN_NO FROM TBLTCREPLACE WHERE TR_ID =(SELECT MAX(TR_ID) FROM TBLTCREPLACE )";
                        // oledbCommand.Parameters.AddWithValue("sdtr_code", sdtr_code);
                        string res = objconn.get_value(strQry);
                        oledbCommand = new OleDbCommand();
                        strQry = "SELECT TR_ID,WO_SLNO,to_char(IN_DATE,'yyyy-mm-dd')IN_DATE,to_char(TR_RI_DATE,'yyyy-mm-dd')TR_RI_DATE,TR_CRBY,TR_DESC,SM_MMS_STORE_ID AS TR_STORE_SLNO,TR_OIL_QUNTY,DF_STATUS_FLAG,DF_DTC_CODE,DF_EQUIPMENT_ID,IN_NO,DF_ID FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,";
                        strQry += "TBLTCREPLACE,TBLDTCFAILURE,TBLSTOREMAST WHERE SM_ID=TR_STORE_SLNO AND TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND WO_DF_ID=DF_ID AND TR_ID=:NewRecordId1";
                        oledbCommand.Parameters.AddWithValue("NewRecordId1", objApproval.sNewRecordId);
                        dt = objcon.getDataTable(strQry, oledbCommand);

                        string sdtc_code = dt.Rows[0]["DF_DTC_CODE"].ToString();
                        string sdtr_code = dt.Rows[0]["DF_EQUIPMENT_ID"].ToString();
                        oledbCommand = new OleDbCommand();
                        strQry = "SELECT * FROM TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO WHERE WO_ID=(SELECT max(WO_ID) FROM TBLWORKFLOWOBJECTS WHERE WO_RECORD_ID=:TRID) AND WO_ID=WOA_PREV_APPROVE_ID";
                        oledbCommand.Parameters.AddWithValue("TRID", dt.Rows[0]["TR_ID"].ToString());
                        dt2 = objcon.getDataTable(strQry, oledbCommand);
                        oledbCommand = new OleDbCommand();
                        strQry = "SELECT * FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCFAILURE,TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO WHERE ";
                        strQry += " TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND WO_DF_ID=DF_ID AND TR_ID=:NewRecordId AND WO_RECORD_ID=TR_ID AND ";
                        strQry += " WO_ID=WOA_PREV_APPROVE_ID AND WO_DATA_ID=:sdtc_code";
                        //strQry = "SELECT * FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLDTCFAILURE,TBLWORKFLOWOBJECTS,TBLWO_OBJECT_AUTO WHERE ";
                        //strQry += " TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND WO_DF_ID=DF_ID AND DF_EQUIPMENT_ID='"+ sdtr_code + "' AND WO_RECORD_ID=TR_ID AND ";
                        //strQry += " WO_ID=WOA_PREV_APPROVE_ID AND WO_DATA_ID='" + sdtc_code + "' AND DF_REPLACE_FLAG = '0'";
                        oledbCommand.Parameters.AddWithValue("NewRecordId", objApproval.sNewRecordId);
                        oledbCommand.Parameters.AddWithValue("sdtc_code", sdtc_code);
                        dt3 = objcon.getDataTable(strQry, oledbCommand);

                        DateTime dtime = Convert.ToDateTime(dt3.Rows[0]["WO_DATE"].ToString());
                        DateTime dtHost = Convert.ToDateTime(ConfigurationSettings.AppSettings["dHost"].ToString()); // to differentiate old record and new record
                        oledbCommand = new OleDbCommand();
                        if (dtime < dtHost)
                        {
                            DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();
                            IsSuccess = objWcf.SaveDtlmsTackDetails(dt3);
                            return true;
                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            strQry = "SELECT max(TM_ID) FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID='" + sdtc_code + "' AND TM_LIVE_FLAG=1 ";
                            //  oledbCommand.Parameters.AddWithValue("sdtc_code", sdtc_code);
                            stm_id = objconn.get_value(strQry);

                            oledbCommand = new OleDbCommand();
                            strQry = "SELECT max(TM_ID) FROM TBLTRANSDTCMAPPING WHERE TM_DTC_ID='" + sdtc_code + "' AND TM_ID <>'" + stm_id + "' AND TM_LIVE_FLAG=0 ";
                            //oledbCommand.Parameters.AddWithValue("sdtc_codes", sdtc_code);
                            //oledbCommand.Parameters.AddWithValue("stm_ids", stm_id);
                            sSecondMax = objconn.get_value(strQry);

                            oledbCommand = new OleDbCommand();
                            #region getting dtr Item Code 
                            strQry = "SELECT CASE WHEN (TC_STATUS=1) THEN ITM_BRAND_NEW WHEN (TC_STATUS=3 AND (WARRENTY_TYPE='AGP')) THEN ITM_FAULTY_AGP WHEN ";
                            strQry += "(TC_STATUS=3 AND (WARRENTY_TYPE='WGP')) THEN ITM_FAULTY_WGP WHEN (TC_STATUS=2) THEN ITM_REPAIR_GOOD WHEN (TC_STATUS=3 AND ";
                            strQry += "(WARRENTY_TYPE='AGP') AND TC_MAKE_ID='98') THEN ITM_FAULTY_AGP_ABB_MAKE WHEN (TC_STATUS=2 AND TC_MAKE_ID='98')THEN ";
                            strQry += "ITM_REPAIRE_GOOD_ABB_MAKE WHEN TC_STATUS='4' THEN ITM_SCRAPE  END ITEM_CODE FROM (SELECT TC_CAPACITY,TC_STATUS,";
                            strQry += "ITM_FAULTY_AGP,ITM_FAULTY_WGP,ITM_REPAIR_GOOD,ITM_BRAND_NEW,ITM_FAULTY_AGP_ABB_MAKE,ITM_REPAIRE_GOOD_ABB_MAKE,ITM_SCRAPE,";
                            strQry += "TC_MAKE_ID,(CASE WHEN SYSDATE < TC_WARANTY_PERIOD THEN (SELECT RSD_GUARRENTY_TYPE FROM (SELECT RSD_ID,RSD_GUARRENTY_TYPE, ";
                            strQry += " row_number() over (order by RSD_ID desc) AS RNUM  FROM TBLREPAIRSENTDETAILS,TBLTCMASTER  WHERE  RSD_TC_CODE = TC_CODE AND RSD_GUARRENTY_TYPE ";
                            strQry += " IS NOT NULL) WHERE RNUM=1) WHEN DF_GUARANTY_TYPE IS NOT NULL THEN ";
                            strQry += "DF_GUARANTY_TYPE ELSE '' END) WARRENTY_TYPE FROM TBLTCMASTER,TBLITEMPRICEMASTER,TBLDTCFAILURE WHERE ";
                            strQry += "TC_CAPACITY =ITM_CAPACITY AND TC_CODE=DF_EQUIPMENT_ID AND DF_REPLACE_FLAG=0 AND TC_CODE='" + sdtr_code + "')";
                            // oledbCommand.Parameters.AddWithValue("sdtr_codes", sdtr_code);
                            string sDtrItemCode = objconn.get_value(strQry);
                            #endregion

                            DtmmsWebService.Service1Client objWcf = new DtmmsWebService.Service1Client();
                            DataTable RiDetails = new DataTable("Ridetails");

                            RiDetails.Columns.Add("FORMNAME", typeof(string));

                            #region RI Details
                            RiDetails.Columns.Add("TR_ID", typeof(string));
                            RiDetails.Columns.Add("WO_SLNO", typeof(string));
                            RiDetails.Columns.Add("TR_IN_NO", typeof(string));
                            RiDetails.Columns.Add("IN_DATE", typeof(string));
                            RiDetails.Columns.Add("TR_RI_DATE", typeof(string));
                            RiDetails.Columns.Add("TR_CRBY", typeof(string));
                            RiDetails.Columns.Add("TR_DESC", typeof(string));
                            RiDetails.Columns.Add("TR_STORE_SLNO", typeof(string));
                            RiDetails.Columns.Add("TR_OIL_QUNTY", typeof(string));
                            RiDetails.Columns.Add("DF_STATUS_FLAG", typeof(string));
                            RiDetails.Columns.Add("IN_NO", typeof(string));
                            RiDetails.Columns.Add("DF_ID", typeof(string));

                            #endregion

                            #region TBLWORKFLOWOBJECTS details

                            RiDetails.Columns.Add("WO_BO_ID", typeof(string));
                            RiDetails.Columns.Add("WO_RECORD_ID", typeof(string));
                            RiDetails.Columns.Add("WO_PREV_APPROVE_ID", typeof(string));
                            RiDetails.Columns.Add("WO_NEXT_ROLE", typeof(string));
                            RiDetails.Columns.Add("WO_OFFICE_CODE", typeof(string));
                            RiDetails.Columns.Add("WO_USER_COMMENT", typeof(string));
                            RiDetails.Columns.Add("WO_APPROVED_BY", typeof(string));
                            RiDetails.Columns.Add("WO_APPROVE_STATUS", typeof(string));
                            RiDetails.Columns.Add("WO_CR_BY", typeof(string));
                            RiDetails.Columns.Add("WO_CR_ON", typeof(string));
                            RiDetails.Columns.Add("WO_RECORD_BY", typeof(string));
                            RiDetails.Columns.Add("WO_DEVICE_ID", typeof(string));
                            RiDetails.Columns.Add("WO_DESCRIPTION", typeof(string));
                            RiDetails.Columns.Add("WO_WFO_ID", typeof(string));
                            RiDetails.Columns.Add("WO_INITIAL_ID", typeof(string));
                            RiDetails.Columns.Add("WO_DATA_ID", typeof(string));
                            RiDetails.Columns.Add("WO_REF_OFFCODE", typeof(string));

                            #endregion

                            #region TBLWO_OBJECT_AUTO details

                            RiDetails.Columns.Add("WOA_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_BFM_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_PREV_APPROVE_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_ROLE_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_OFFICE_CODE", typeof(string));
                            RiDetails.Columns.Add("WOA_INITIAL_ACTION_ID", typeof(string));
                            RiDetails.Columns.Add("WOA_DESCRIPTION", typeof(string));
                            RiDetails.Columns.Add("WOA_CRBY", typeof(string));
                            RiDetails.Columns.Add("WOA_CRON", typeof(string));
                            RiDetails.Columns.Add("WOA_REF_OFFCODE", typeof(string));

                            RiDetails.Columns.Add("DTR_ITEM_CODE", typeof(string));
                            RiDetails.Columns.Add("DF_DTR_COMMISSION_DATE", typeof(string));
                            RiDetails.Columns.Add("DF_DATE", typeof(string));
                            #endregion
                            oledbCommand = new OleDbCommand();
                            string scrby = dt2.Rows[0]["WO_CR_BY"].ToString();
                            Intigration_crby = scrby;
                            strQry = "SELECT US_MMS_ID FROM TBLUSER WHERE US_ID=:scrby1";
                            oledbCommand.Parameters.AddWithValue("scrby1", scrby);
                            scrby = objconn.get_value(strQry, oledbCommand);

                            if (scrby == "" || scrby == null)
                            {
                                RiDetails.Columns.Remove("WO_CR_BY");
                                RiDetails.Columns.Remove("WOA_CRBY");

                                RiDetails.Columns.Add("WO_CR_BY", typeof(string));
                                RiDetails.Columns.Add("WOA_CRBY", typeof(string));
                            }

                            DataRow dtrow = RiDetails.NewRow();
                            dtrow["FORMNAME"] = "ReturnInvoice";

                            dtrow["TR_ID"] = Convert.ToString(dt.Rows[0]["TR_ID"]);
                            dtrow["WO_SLNO"] = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                            dtrow["TR_IN_NO"] = res;
                            dtrow["IN_DATE"] = Convert.ToString(dt.Rows[0]["IN_DATE"]);
                            dtrow["TR_RI_DATE"] = Convert.ToString(dt.Rows[0]["TR_RI_DATE"]);
                            dtrow["TR_CRBY"] = Convert.ToString(dt.Rows[0]["TR_CRBY"]);
                            dtrow["TR_DESC"] = Convert.ToString(dt.Rows[0]["TR_DESC"]);
                            dtrow["TR_STORE_SLNO"] = Convert.ToString(dt.Rows[0]["TR_STORE_SLNO"]);
                            dtrow["TR_OIL_QUNTY"] = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                            dtrow["DF_STATUS_FLAG"] = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                            dtrow["IN_NO"] = Convert.ToString(dt.Rows[0]["IN_NO"]);
                            dtrow["DF_ID"] = Convert.ToString(dt.Rows[0]["DF_ID"]);

                            dtrow["WO_BO_ID"] = Convert.ToString(dt2.Rows[0]["WO_BO_ID"]);
                            dtrow["WO_RECORD_ID"] = Convert.ToString(dt2.Rows[0]["WO_RECORD_ID"]);
                            dtrow["WO_PREV_APPROVE_ID"] = Convert.ToString(dt2.Rows[0]["WO_PREV_APPROVE_ID"]);
                            dtrow["WO_NEXT_ROLE"] = Convert.ToString(dt2.Rows[0]["WO_NEXT_ROLE"]);
                            dtrow["WO_OFFICE_CODE"] = Convert.ToString(dt2.Rows[0]["WO_OFFICE_CODE"]);
                            dtrow["WO_CR_ON"] = Convert.ToString(dt2.Rows[0]["WO_CR_ON"]);
                            dtrow["WO_USER_COMMENT"] = Convert.ToString(dt2.Rows[0]["WO_USER_COMMENT"]);
                            dtrow["WO_APPROVED_BY"] = Convert.ToString(dt2.Rows[0]["WO_APPROVED_BY"]);
                            dtrow["WO_APPROVE_STATUS"] = Convert.ToString(dt2.Rows[0]["WO_APPROVE_STATUS"]);
                            dtrow["WO_CR_BY"] = scrby;
                            dtrow["WO_RECORD_BY"] = Convert.ToString(dt2.Rows[0]["WO_RECORD_BY"]);
                            dtrow["WO_DEVICE_ID"] = Convert.ToString(dt2.Rows[0]["WO_DEVICE_ID"]);
                            dtrow["WO_DESCRIPTION"] = Convert.ToString(dt2.Rows[0]["WO_DESCRIPTION"]);
                            dtrow["WO_WFO_ID"] = Convert.ToString(dt2.Rows[0]["WO_WFO_ID"]);
                            dtrow["WO_INITIAL_ID"] = Convert.ToString(dt2.Rows[0]["WO_INITIAL_ID"]);
                            dtrow["WO_DATA_ID"] = Convert.ToString(dt.Rows[0]["TR_ID"]);
                            dtrow["WO_REF_OFFCODE"] = Convert.ToString(dt2.Rows[0]["WO_REF_OFFCODE"]);

                            dtrow["WOA_ID"] = Convert.ToString(dt2.Rows[0]["WOA_ID"]);
                            dtrow["WOA_BFM_ID"] = Convert.ToString(dt2.Rows[0]["WOA_BFM_ID"]);
                            dtrow["WOA_PREV_APPROVE_ID"] = Convert.ToString(dt2.Rows[0]["WOA_PREV_APPROVE_ID"]);
                            dtrow["WOA_ROLE_ID"] = Convert.ToString(dt2.Rows[0]["WOA_ROLE_ID"]);
                            dtrow["WOA_OFFICE_CODE"] = Convert.ToString(dt2.Rows[0]["WOA_OFFICE_CODE"]);
                            dtrow["WOA_INITIAL_ACTION_ID"] = Convert.ToString(dt2.Rows[0]["WOA_INITIAL_ACTION_ID"]);
                            dtrow["WOA_DESCRIPTION"] = Convert.ToString(dt2.Rows[0]["WOA_DESCRIPTION"]);
                            dtrow["WOA_CRBY"] = scrby;
                            dtrow["WOA_CRON"] = Convert.ToString(dt2.Rows[0]["WOA_CRON"]);
                            dtrow["WOA_REF_OFFCODE"] = Convert.ToString(dt2.Rows[0]["WOA_REF_OFFCODE"]);
                            //
                            //////
                            ////////////
                            ///////////////////

                            //********//************//************ added   
                            DateTime tempDate;
                            if (dt3.Rows[0]["DF_DTR_COMMISSION_DATE"].ToString() != null)
                            {
                                //System.IO.File.AppendAllText("D:\\DateTime.txt", dt3.Rows[0]["DF_DTR_COMMISSION_DATE"].ToString());

                                tempDate = Convert.ToDateTime(Convert.ToString(dt3.Rows[0]["DF_DTR_COMMISSION_DATE"]).Replace('-', '/'));
                                //DateTime dDtrCommissiondate = DateTime.Parse(tempDate, "MM/dd/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                                DateTime dDtrCommissiondate = DateTime.Parse(tempDate.ToString());

                                //System.IO.File.AppendAllText("D:\\DateTime.txt","After Parsing" + dDtrCommissiondate.ToString());

                                //System.IO.File.AppendAllText("D:\\DateTime.txt", dDtrCommissiondate.ToString());
                                string sDtrCommisiondate = Convert.ToDateTime(dDtrCommissiondate).ToString("MM/dd/yyyy");

                                //  System.IO.File.AppendAllText("D:\\DateTime.txt", "After Converting to string in  dd/MM/yyyy format" + sDtrCommisiondate);

                                dtrow["DF_DTR_COMMISSION_DATE"] = sDtrCommisiondate;
                            }
                            //DateTime dfailuredate = DateTime.ParseExact(dt3.Rows[0]["DF_DATE"].ToString().Replace('-', '/'), "MM/dd/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                            tempDate = Convert.ToDateTime(Convert.ToString(dt3.Rows[0]["DF_DATE"]).Replace('-', '/'));
                            DateTime dfailuredate = DateTime.Parse(tempDate.ToString());
                            string sfailuredate = Convert.ToDateTime(dfailuredate).ToString("MM/dd/yyyy");
                            dtrow["DF_DATE"] = sfailuredate;
                            dtrow["DTR_ITEM_CODE"] = sDtrItemCode;
                            RiDetails.Rows.Add(dtrow);
                            oledbCommand = new OleDbCommand();
                            IsSuccess = objWcf.SaveRVData(RiDetails);
                            if (IsSuccess == false)
                            {
                                strQry = "SELECT max(wo_id)+1 FROM TBLWORKFLOWOBJECTS";
                                sMaxID = objconn.get_value(strQry);
                                sMaxID = "-" + sMaxID;

                                strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID = '" + dt3.Rows[0]["WO_ID"].ToString() + "'";
                                objcon.Execute(strQry);

                                strQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_ID = '" + dt3.Rows[0]["WOA_ID"].ToString() + "'";
                                objcon.Execute(strQry);

                                strQry = "DELETE FROM TBLTCREPLACE WHERE TR_ID = '" + dt3.Rows[0]["TR_ID"].ToString() + "'";
                                objcon.Execute(strQry);

                                strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + sMaxID + "', WO_USER_COMMENT = '', WO_APPROVED_BY = '', WO_APPROVE_STATUS = '0' WHERE WO_ID = '" + dt3.Rows[0]["WO_PREV_APPROVE_ID"].ToString() + "'";
                                objcon.Execute(strQry);

                                strQry = "DELETE TBLTRANSDTCMAPPING WHERE TM_ID='" + stm_id + "'";
                                objcon.Execute(strQry);

                                strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG=1 WHERE TM_ID='" + sSecondMax + "'";
                                objcon.Execute(strQry);

                                return false;
                            }
                        }


                    }
                    catch (Exception ex)
                    {

                        strQry = "SELECT max(wo_id)+1 FROM TBLWORKFLOWOBJECTS";
                        sMaxID = objcon.get_value(strQry);
                        sMaxID = "-" + sMaxID;

                        strQry = "DELETE FROM TBLWORKFLOWOBJECTS WHERE WO_ID = '" + dt3.Rows[0]["WO_ID"].ToString() + "'";
                        objcon.Execute(strQry);

                        strQry = "DELETE FROM TBLWO_OBJECT_AUTO WHERE WOA_ID = '" + dt3.Rows[0]["WOA_ID"].ToString() + "'";
                        objcon.Execute(strQry);

                        strQry = "DELETE FROM TBLTCREPLACE WHERE TR_ID = '" + dt3.Rows[0]["TR_ID"].ToString() + "'";
                        objcon.Execute(strQry);

                        strQry = "UPDATE TBLWORKFLOWOBJECTS SET WO_RECORD_ID='" + sMaxID + "', WO_USER_COMMENT = '', WO_APPROVED_BY = '', WO_APPROVE_STATUS = '0' WHERE WO_ID = '" + dt3.Rows[0]["WO_PREV_APPROVE_ID"].ToString() + "'";
                        objcon.Execute(strQry);

                        strQry = "DELETE TBLTRANSDTCMAPPING WHERE TM_ID='" + stm_id + "'";
                        objcon.Execute(strQry);

                        strQry = "UPDATE TBLTRANSDTCMAPPING SET TM_LIVE_FLAG=1 WHERE TM_ID='" + sSecondMax + "'";
                        objcon.Execute(strQry);

                        clsException.Intigration_LogError(ex.StackTrace, ex.Message, strFormCode, "Nested_SaveWFObjectAuto", Intigration_crby);

                        return false;
                    }

                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
                //return false;
            }
        }

        /// <summary>
        /// Update Initial Action Id from Workflow Object Id in TBLWO_OBJECT_AUTO Table
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool UpdateWFOAutoObject(clsApproval objApproval)
        {

            try
            {
                string strQry = string.Empty;
                string var = string.Empty;

                strQry = "UPDATE TBLWO_OBJECT_AUTO SET WOA_INITIAL_ACTION_ID='" + objApproval.sWFObjectId
                    + "' WHERE WOA_PREV_APPROVE_ID='" + objApproval.sPrevWFOId + "'";
                objcon.Execute(strQry);
                return true;

                if (objApproval.sWFObjectId == null || objApproval.sWFObjectId == "")
                {
                    try
                    {
                        var = "objApproval.sWFObjectId is getting null";
                    }
                    catch (Exception ex)
                    {
                        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Inside_UpdateWFOAutoObject " + var);
                        return false;
                    }
                }

                if (objApproval.sPrevWFOId == null || objApproval.sPrevWFOId == "")
                {
                    try
                    {
                        var = "objApproval.sPrevWFOId is getting null";
                    }
                    catch (Exception ex)
                    {
                        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Inside_UpdateWFOAutoObject " + var);
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }


        public bool UpdateWFOAutoObject1(clsApproval objApproval, CustOledbConnection objconn)
        {
            string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");

            if (!Directory.Exists(sFolderPath))
            {
                Directory.CreateDirectory(sFolderPath);

            }
            string sFileName = "Begintrans";
            string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            try
            {
                string strQry = string.Empty;
                string var = string.Empty;

                strQry = "UPDATE TBLWO_OBJECT_AUTO SET WOA_INITIAL_ACTION_ID='" + objApproval.sWFObjectId + "' WHERE WOA_PREV_APPROVE_ID='" + objApproval.sPrevWFOId + "'";
                objconn.Execute(strQry);
                File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , UpdateWFOAutoObject1 After query" + Environment.NewLine);

                return true;

                if (objApproval.sWFObjectId == null || objApproval.sWFObjectId == "")
                {
                    try
                    {
                        var = "objApproval.sWFObjectId is getting null";
                    }
                    catch (Exception ex)
                    {
                        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Inside_UpdateWFOAutoObject " + var);
                        return false;
                    }
                }

                if (objApproval.sPrevWFOId == null || objApproval.sPrevWFOId == "")
                {
                    try
                    {
                        var = "objApproval.sPrevWFOId is getting null";
                    }
                    catch (Exception ex)
                    {
                        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Inside_UpdateWFOAutoObject " + var);
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                // return false;
                throw ex;

            }
        }

        public bool UpdateWFOAutoObjectlatest(clsApproval objApproval, CustOledbConnection objconn)
        {

            try
            {
                string strQry = string.Empty;
                string var = string.Empty;

                strQry = "UPDATE TBLWO_OBJECT_AUTO SET WOA_INITIAL_ACTION_ID='" + objApproval.sWFObjectId + "' WHERE WOA_PREV_APPROVE_ID='" + objApproval.sPrevWFOId + "'";
                objconn.Execute(strQry);
                return true;

                if (objApproval.sWFObjectId == null || objApproval.sWFObjectId == "")
                {
                    try
                    {
                        var = "objApproval.sWFObjectId is getting null";
                    }
                    catch (Exception ex)
                    {
                        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Inside_UpdateWFOAutoObject " + var);
                        return false;
                    }
                }

                if (objApproval.sPrevWFOId == null || objApproval.sPrevWFOId == "")
                {
                    try
                    {
                        var = "objApproval.sPrevWFOId is getting null";
                    }
                    catch (Exception ex)
                    {
                        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Inside_UpdateWFOAutoObject " + var);
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

   

        /// <summary>
        /// To get Form Creator Access (If priority is 1, Consider as Form Creator)
        /// </summary>
        /// <param name="sBOId"></param>
        /// <param name="sRoleId"></param>
        /// <param name="sFormName"></param>
        /// <returns></returns>
        public string GetFormCreatorLevel(string sBOId, string sRoleId, string sFormName = "")
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                if (sBOId != "")
                {
                    strQry = "SELECT WM_LEVEL FROM TBLWORKFLOWMASTER WHERE WM_ROLEID=:RoleId AND WM_BOID=:BOId";
                    oledbCommand.Parameters.AddWithValue("RoleId", sRoleId);
                    oledbCommand.Parameters.AddWithValue("BOId", sBOId);
                }
                else
                {
                    strQry = "SELECT WM_LEVEL FROM TBLWORKFLOWMASTER WHERE WM_ROLEID=:RoleIds AND WM_BOID= ";
                    strQry += " (SELECT BO_ID FROM TBLBUSINESSOBJECT WHERE UPPER(BO_FORMNAME)=:FormName)";
                    oledbCommand.Parameters.AddWithValue("RoleIds", sRoleId);
                    oledbCommand.Parameters.AddWithValue("FormName", sFormName.Trim().ToUpper());
                }



                return objcon.get_value(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }


        /// <summary>
        /// Get Datatable from XML string
        /// </summary>
        /// <param name="sWFDataId"></param>
        /// <returns></returns>
        public DataTable GetDatatableFromXML(string sWFDataId)
        {
            DataSet ds = new DataSet();
            try
            {
                string sXMLResult = GetWFOData(sWFDataId);

                StringReader sReader = new StringReader(sXMLResult);
                try
                {
                    ds.ReadXml(sReader);
                }
                catch (Exception)
                {
                }

                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ds.Tables[0];
            }
        }

        public DataTable FailureCustomerdetails(string sFailureid)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQry = string.Empty;
                strQry += " SELECT DF_CUSTOMER_MOBILE,DF_CUSTOMER_NAME,DF_NUMBER_OF_INSTALLATIONS from TBLDTCFAILURE WHERE DF_ID=" + sFailureid + "";
                return objcon.getDataTable(strQry, oledbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dt;
            }
        }

        /// <summary>
        /// Get Datatable from Multiple XML string
        /// </summary>
        /// <param name="sWFDataId"></param>
        /// <returns></returns>
        public DataSet GetDatatableFromMultipleXML(string sWFDataId)
        {
            DataSet ds = new DataSet();
            DataSet dsResult = new DataSet();
            try
            {
                string sXMLResult = GetWFOData(sWFDataId);
                StringReader sReader = new StringReader(sXMLResult);
                dsResult.ReadXml(sReader);
                return dsResult;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return dsResult;
            }
        }


        /// <summary>
        /// Create Xml format using Table Name and Column Name with Values
        /// </summary>
        /// <param name="strColumns"></param>
        /// <param name="strParameters"></param>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public string CreateXml(string strColumns, string strParameters, string strTableName)
        {
            try
            {
                DataTable dtXmlContent = new DataTable();

                DataTable dtnew = new DataTable();

                DataSet ds;
                if (strTableName.Contains(";"))
                {
                    ds = new DataSet(strTableName.Split(';').GetValue(0).ToString());
                }
                else
                {
                    ds = new DataSet(strTableName);
                }

                string[] strArrColumns = strColumns.Split(';');
                string[] strArrParameters = strParameters.Split(';');
                string[] strTableNames = strTableName.Split(';');

                int k = 0;
                //DataRow dRow = dt.NewRow();
                for (int i = 0; i < strArrColumns.Length; i++)
                {
                    DataTable dt = new DataTable();
                    DataRow dRow = dt.NewRow();
                    string[] strdtColumns = strArrColumns[i].Split(',');
                    string[] strdtParametres = strArrParameters[i].Split(',');
                    dt.TableName = strTableNames[i];
                    //DataRow dRow1 = dtnew.NewRow();
                    for (int j = 0; j < strdtColumns.Length; j++)
                    {
                        dt.Columns.Add(strdtColumns[j]);
                        if (k < strdtParametres.Length)
                        {
                            string strColumnName = strdtParametres[k];
                            dRow[dt.Columns[j]] = strdtParametres[k];
                            if (dt.Rows.Count == 0)
                            {
                                dt.Rows.Add(dRow);
                            }
                            dt.AcceptChanges();
                            //i--;
                        }
                        k++;

                    }

                    k = 0;

                    ds.Merge(dt);
                    dt.Clear();

                }
                return ds.GetXml();
            }

            catch (Exception ex)
            {
                string strfailure = string.Empty;
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return strfailure;
            }
        }


        /// <summary>
        /// To get XML String from TBLWFODATA based on WFODataId corresponding to WorkflowObject Id from TBLWORKFLOWOBJECTS
        /// </summary>
        /// <param name="sWFDataId"></param>
        /// <returns></returns>
        public string GetWFOData(string sWFDataId)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT WFO_DATA FROM TBLWFODATA WHERE WFO_ID=:WFDataId";
                oledbCommand.Parameters.AddWithValue("WFDataId", sWFDataId);
                string sXMLResult = objcon.get_value(strQry, oledbCommand);
                return sXMLResult;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);
                return ex.Message;
            }
        }
        /// <summary>
        /// Check Data already exist and Waiting for Approval to Restrict duplicate Entry
        /// </summary>
        /// <param name="sDataReferenceId"></param>
        /// <param name="sBOId"></param>
        /// <returns></returns>
        public bool CheckAlreadyExistEntry(string sDataReferenceId, string sBOId)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                string sResult = string.Empty;
                strQry = "SELECT WO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_DATA_ID=:DataReferenceId AND WO_APPROVE_STATUS='0' AND WO_BO_ID=:BOId";
                oledbCommand.Parameters.AddWithValue("DataReferenceId", sDataReferenceId);
                oledbCommand.Parameters.AddWithValue("BOId", sBOId);
                sResult = objcon.get_value(strQry, oledbCommand);
                if (sResult != "")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public void SendSMStoRole(clsApproval objApproval, string sPreviousBoId)
        {
            oledbCommand = new OleDbCommand();

            string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");

            if (!Directory.Exists(sFolderPath))
            {
                Directory.CreateDirectory(sFolderPath);

            }
            string sFileName = "Begintrans";
            string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            try
            {

                File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , SendSMStoRole Start" + Environment.NewLine);

                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sOfficeCode = string.Empty;

                if (objApproval.sRoleId == "1")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 3);
                }
                if (objApproval.sRoleId == "2")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                if (objApproval.sRoleId == "3")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                if (objApproval.sRoleId == "4")
                {
                    sOfficeCode = objApproval.sRefOfficeCode;
                }
                if (objApproval.sRoleId == "5")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                if (objApproval.sRoleId == "6")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                if (objApproval.sRoleId == "7")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                strQry = "SELECT US_FULL_NAME,US_MOBILE_NO,US_EMAIL FROM TBLUSER WHERE US_ROLE_ID IN (" + objApproval.sRoleId + ") AND US_OFFICE_CODE='" + sOfficeCode + "' and US_STATUS='A' ";

                dt = objcon.getDataTable(strQry);

                string sSMSText = string.Empty;
                string sQry = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objComm = new clsCommunication();

                    oledbCommand = new OleDbCommand();
                    //Failure Entry
                    if (objApproval.sBOId == "9")
                    {
                        if (objApproval.sApproveStatus == "3")
                        {
                            strQry = "SELECT BO_NAME || '~' || RO_NAME FROM TBLWORKFLOWOBJECTS,TBLBUSINESSOBJECT,TBLUSER,TBLROLES WHERE WO_BO_ID=BO_ID AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID ";
                            strQry += " AND WO_ID=:WFObjectId AND WO_BO_ID=:BOId";
                            oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                            oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);

                            string sResult = objcon.get_value(strQry, oledbCommand);

                            objComm.sSMSkey = "SMStoReject";
                            objComm = objComm.GetsmsTempalte(objComm);
                            sSMSText = String.Format(objComm.sSMSTemplate,
                            sResult.Split('~').GetValue(0).ToString(), objApproval.sDataReferenceId, sResult.Split('~').GetValue(1).ToString());

                        }
                        else
                        {

                            objComm.sSMSkey = "SMStoFailureCreate";
                            objComm = objComm.GetsmsTempalte(objComm);
                            sSMSText = String.Format(objComm.sSMSTemplate,
                            objApproval.sDataReferenceId);
                        }

                    }

                    //Work Order Entry
                    if (objApproval.sBOId == "11")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "9")
                        {
                            objComm.sSMSkey = "SMStoFailure";
                            objComm = objComm.GetsmsTempalte(objComm);
                            sSMSText = String.Format(objComm.sSMSTemplate,
                            objApproval.sDataReferenceId);
                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                            sQry += " DF_ID =:DataReferenceId AND WO_DATA_ID=:DataReferenceId1 AND DF_DTC_CODE=DT_CODE AND ";
                            sQry += "WO_NEXT_ROLE =:RoleId AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID";
                            oledbCommand.Parameters.AddWithValue("DataReferenceId", objApproval.sDataReferenceId);
                            oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                            oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);

                            // oledbCommand.Parameters.AddWithValue("BOId", objApproval.sDataReferenceId);
                            string sResult = objcon.get_value(sQry, oledbCommand);

                            if (objApproval.sRoleId == "2")
                            {
                                objComm.sSMSkey = "SMStoWorkOrderCreate";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString());
                            }
                            else
                            {
                                oledbCommand = new OleDbCommand();
                                if (objApproval.sApproveStatus == "3")
                                {
                                    sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                    sQry += " DF_ID =:DataReferenceId AND WO_DATA_ID=:DataReferenceId1 AND DF_DTC_CODE=DT_CODE AND ";
                                    sQry += "WO_NEXT_ROLE =:RoleIds AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId";
                                    oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                                    oledbCommand.Parameters.AddWithValue("DataReferenceId", objApproval.sDataReferenceId);
                                    oledbCommand.Parameters.AddWithValue("RoleIds", objApproval.sRoleId);
                                    oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                                    sResult = objcon.get_value(sQry, oledbCommand);

                                    objComm.sSMSkey = "SMStoReject";
                                    objComm = objComm.GetsmsTempalte(objComm);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                    sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                                }
                                else
                                {
                                    objComm.sSMSkey = "SMStoWorkOrderApprover";
                                    objComm = objComm.GetsmsTempalte(objComm);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                    sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString(), sResult.Split('~').GetValue(2).ToString());
                                }

                            }
                        }

                    }

                    // Indent
                    if (objApproval.sBOId == "12")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "11")
                        {
                            sQry = "SELECT DT_CODE,DT_NAME,WO_NO FROM TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST WHERE WO_SLNO=:NewRecordId AND WO_DF_ID=DF_ID AND DF_DTC_CODE=DT_CODE";
                            oledbCommand.Parameters.AddWithValue("NewRecordId", objApproval.sNewRecordId);
                            dt = objcon.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoWorkOrder";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["DT_NAME"]));
                            }

                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            if (objApproval.sApproveStatus == "3")
                            {
                                sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                sQry += " DF_ID =:DataReferenceId AND WO_DATA_ID=:DataReferenceId1 AND DF_DTC_CODE=DT_CODE AND ";
                                sQry += "WO_NEXT_ROLE =:RoleId AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                                string sResult = objcon.get_value(sQry, oledbCommand);

                                objComm.sSMSkey = "SMStoReject";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                            }
                            else
                            {
                                sQry = "SELECT DT_CODE,DT_NAME,WO_NO FROM TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST WHERE WO_SLNO=:DataReferenceId1 AND WO_DF_ID=DF_ID AND DF_DTC_CODE=DT_CODE";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                                dt = objcon.getDataTable(sQry, oledbCommand);
                                if (dt.Rows.Count > 0)
                                {

                                    objComm.sSMSkey = "SMStoIndentCreate";
                                    objComm = objComm.GetsmsTempalte(objComm);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                    Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["DT_NAME"]));
                                }
                            }


                        }
                    }

                    // Invoice Creation Approval
                    if (objApproval.sBOId == "29")
                    {
                        if (sPreviousBoId == "12")
                        {
                            sQry = "SELECT DT_CODE,DT_NAME,WO_NO,TI_INDENT_NO FROM TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST,TBLINDENT WHERE ";
                            sQry += " TI_ID=:NewRecordId2 AND WO_DF_ID=DF_ID AND DF_DTC_CODE=DT_CODE AND WO_SLNO=TI_WO_SLNO";
                            oledbCommand.Parameters.AddWithValue("NewRecordId2", objApproval.sNewRecordId);
                            dt = objcon.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoIndent";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]));
                            }
                        }
                    }

                    // Invoice Creation
                    if (objApproval.sBOId == "13")
                    {
                        if (sPreviousBoId == "29")
                        {
                            sQry = "SELECT DT_CODE,DT_NAME,WO_NO,TI_INDENT_NO FROM TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST,TBLINDENT WHERE ";
                            sQry += " TI_ID=:RecordId1 AND WO_DF_ID=DF_ID AND DF_DTC_CODE=DT_CODE AND WO_SLNO=TI_WO_SLNO";
                            oledbCommand.Parameters.AddWithValue("RecordId1", objApproval.sRecordId);
                            dt = objcon.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoIndent";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]));
                            }
                        }

                    }

                    //Decommission
                    if (objApproval.sBOId == "14")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "13")
                        {
                            sQry = "SELECT TI_INDENT_NO,IN_INV_NO FROM TBLINDENT,TBLDTCINVOICE WHERE IN_TI_NO=TI_ID AND TI_ID=:DataReferenceId3";
                            oledbCommand.Parameters.AddWithValue("DataReferenceId3", objApproval.sDataReferenceId);
                            dt = objcon.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoInvoice";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]), Convert.ToString(dt.Rows[0]["IN_INV_NO"]));
                            }

                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            if (objApproval.sApproveStatus == "3")
                            {
                                sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                sQry += " DF_DTC_CODE =:DataReferenceId4 AND WO_DATA_ID=:DataReferenceId5 AND DF_DTC_CODE=DT_CODE AND ";
                                sQry += "WO_NEXT_ROLE =:RoleId3 AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId3";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId4", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("DataReferenceId5", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("RoleId3", objApproval.sRoleId);
                                oledbCommand.Parameters.AddWithValue("WFObjectId3", objApproval.sWFObjectId);
                                string sResult = objcon.get_value(sQry, oledbCommand);

                                objComm.sSMSkey = "SMStoReject";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                            }
                            else
                            {
                                sQry = "SELECT DF_DTC_CODE,DF_EQUIPMENT_ID FROM TBLDTCFAILURE WHERE  DF_DTC_CODE=:DataReferenceId6 ";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId6", objApproval.sDataReferenceId);

                                dt = objcon.getDataTable(sQry, oledbCommand);
                                if (dt.Rows.Count > 0)
                                {
                                    objComm.sSMSkey = "SMStoDecommCreate";
                                    objComm = objComm.GetsmsTempalte(objComm);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                    Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]));
                                }
                            }
                        }
                    }

                    // RI Acknoldgement
                    if (objApproval.sBOId == "15")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "14")
                        {
                            sQry = "SELECT IN_INV_NO,TR_RI_NO FROM TBLDTCINVOICE,TBLTCREPLACE WHERE TR_IN_NO=IN_NO AND TR_ID=:NewRecordId4";
                            oledbCommand.Parameters.AddWithValue("NewRecordId4", objApproval.sNewRecordId);
                            dt = objcon.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoDecomm";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                Convert.ToString(dt.Rows[0]["IN_INV_NO"]), Convert.ToString(dt.Rows[0]["TR_RI_NO"]));
                            }

                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            if (objApproval.sApproveStatus == "3")
                            {
                                sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                sQry += " DF_DTC_CODE =:DataReferenceId1 AND WO_DATA_ID=:DataReferenceId2 AND DF_DTC_CODE=DT_CODE AND ";
                                sQry += "WO_NEXT_ROLE =:RoleId AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("DataReferenceId2", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                                string sResult = objcon.get_value(sQry, oledbCommand);

                                objComm.sSMSkey = "SMStoReject";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                            }
                            else
                            {
                                sQry = "SELECT TR_RI_NO,DF_EQUIPMENT_ID FROM TBLTCREPLACE,TBLDTCFAILURE,TBLTCDRAWN WHERE DF_ID=TD_DF_ID AND ";
                                sQry += " TD_INV_NO=TR_IN_NO AND TR_ID=:RecordId";

                                oledbCommand.Parameters.AddWithValue("RecordId", objApproval.sRecordId);


                                dt = objcon.getDataTable(sQry, oledbCommand);
                                if (dt.Rows.Count > 0)
                                {
                                    objComm.sSMSkey = "SMStoRICreate";
                                    objComm = objComm.GetsmsTempalte(objComm);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                    Convert.ToString(dt.Rows[0]["TR_RI_NO"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]));
                                }
                            }

                        }
                    }


                    // Completion Report
                    if (objApproval.sBOId == "26")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "15")
                        {
                            sQry = "SELECT TR_RI_NO,DF_EQUIPMENT_ID FROM TBLTCREPLACE,TBLDTCFAILURE,TBLTCDRAWN WHERE DF_ID=TD_DF_ID AND ";
                            sQry += " TD_INV_NO=TR_IN_NO AND TR_ID=:RecordId";
                            oledbCommand.Parameters.AddWithValue("RecordId", objApproval.sRecordId);
                            dt = objcon.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                string qry = "SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_OFF_CODE=:OfficeCode";
                                oledbCommand.Parameters.AddWithValue("OfficeCode", objApproval.sOfficeCode.Substring(0, 2));
                                string sStoreName = objcon.get_value(qry, oledbCommand);

                                objComm.sSMSkey = "SMStoRI";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                Convert.ToString(dt.Rows[0]["TR_RI_NO"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]), sStoreName);
                            }

                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            if (objApproval.sApproveStatus == "3")
                            {
                                sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                sQry += " DF_DTC_CODE =:DataReferenceId3 AND WO_DATA_ID=:DataReferenceId4 AND DF_DTC_CODE=DT_CODE AND ";
                                sQry += "WO_NEXT_ROLE =:RoleId AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId3", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("DataReferenceId4", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);

                                string sResult = objcon.get_value(sQry);

                                objComm.sSMSkey = "SMStoReject";
                                objComm = objComm.GetsmsTempalte(objComm);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                            }
                            else
                            {
                                sQry = "SELECT TR_RI_NO,DF_EQUIPMENT_ID,TD_TC_NO  FROM TBLTCREPLACE,TBLDTCFAILURE,TBLTCDRAWN WHERE DF_ID=TD_DF_ID AND ";
                                sQry += " TD_INV_NO=TR_IN_NO AND TR_ID=:RecordId2";
                                oledbCommand.Parameters.AddWithValue("RecordId2", objApproval.sRecordId);
                                dt = objcon.getDataTable(sQry, oledbCommand);
                                oledbCommand = new OleDbCommand();
                                if (dt.Rows.Count > 0)
                                {
                                    string qry = "SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_OFF_CODE=:OfficeCode1";
                                    oledbCommand.Parameters.AddWithValue("OfficeCode1", objApproval.sOfficeCode.Substring(0, 2));
                                    string sStoreName = objcon.get_value(qry, oledbCommand);

                                    objComm.sSMSkey = "SMStoCRCreate";
                                    objComm = objComm.GetsmsTempalte(objComm);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                    Convert.ToString(dt.Rows[0]["TR_RI_NO"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]), Convert.ToString(dt.Rows[0]["TD_TC_NO"]));
                                }
                            }

                        }
                    }

                    if (sSMSText == "")
                    {

                    }
                    else
                    {
                        objComm.DumpSms(sMobileNo, sSMSText, objComm.sSMSTemplateID);
                    }
                    File.AppendAllText(sPath, Environment.NewLine + "Time --" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " , SendSMStoRole Start" + Environment.NewLine);

                }

            }
            catch (Exception ex)
            {
                //clsException.LogError(ex.StackTrace, ex.Message,
                //                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                //                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        public void SendSMStoRole1(clsApproval objApproval, string sPreviousBoId, CustOledbConnection objconn)
        {
            oledbCommand = new OleDbCommand();
            try
            {
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                string sOfficeCode = string.Empty;

                if (objApproval.sRoleId == "1")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 3);
                }
                if (objApproval.sRoleId == "2")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                if (objApproval.sRoleId == "3")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                if (objApproval.sRoleId == "4")
                {
                    sOfficeCode = objApproval.sRefOfficeCode;
                }
                if (objApproval.sRoleId == "5")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                if (objApproval.sRoleId == "6")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                if (objApproval.sRoleId == "7")
                {
                    sOfficeCode = objApproval.sRefOfficeCode.Substring(0, 2);
                }
                strQry = "SELECT US_FULL_NAME,US_MOBILE_NO,US_EMAIL FROM TBLUSER WHERE US_ROLE_ID IN ('" + objApproval.sRoleId + "') AND US_OFFICE_CODE='" + sOfficeCode + "' and US_STATUS='A' ";
                dt = objcon.getDataTable(strQry, oledbCommand);


                string sSMSText = string.Empty;
                string sQry = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sFullName = Convert.ToString(dt.Rows[i]["US_FULL_NAME"]);
                    string sMobileNo = Convert.ToString(dt.Rows[i]["US_MOBILE_NO"]);

                    clsCommunication objComm = new clsCommunication();

                    oledbCommand = new OleDbCommand();
                    //Failure Entry
                    if (objApproval.sBOId == "9")
                    {
                        if (objApproval.sApproveStatus == "3")
                        {
                            strQry = "SELECT BO_NAME || '~' || RO_NAME FROM TBLWORKFLOWOBJECTS,TBLBUSINESSOBJECT,TBLUSER,TBLROLES WHERE WO_BO_ID=BO_ID AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID ";
                            strQry += " AND WO_ID=:WFObjectId AND WO_BO_ID=:BOId";
                            oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                            oledbCommand.Parameters.AddWithValue("BOId", objApproval.sBOId);

                            string sResult = objconn.get_value(strQry, oledbCommand);

                            objComm.sSMSkey = "SMStoReject";
                            objComm = objComm.GetsmsTempalte1(objComm, objconn);
                            sSMSText = String.Format(objComm.sSMSTemplate,
                            sResult.Split('~').GetValue(0).ToString(), objApproval.sDataReferenceId, sResult.Split('~').GetValue(1).ToString());

                        }
                        else
                        {

                            objComm.sSMSkey = "SMStoFailureCreate";
                            objComm = objComm.GetsmsTempalte1(objComm, objconn);
                            sSMSText = String.Format(objComm.sSMSTemplate,
                            objApproval.sDataReferenceId);
                        }

                    }

                    //Work Order Entry
                    if (objApproval.sBOId == "11")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "9")
                        {
                            objComm.sSMSkey = "SMStoFailure";
                            objComm = objComm.GetsmsTempalte1(objComm, objconn);
                            sSMSText = String.Format(objComm.sSMSTemplate,
                            objApproval.sDataReferenceId);
                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                            sQry += " DF_ID =:DataReferenceId AND WO_DATA_ID=:DataReferenceId1 AND DF_DTC_CODE=DT_CODE AND ";
                            sQry += "WO_NEXT_ROLE =:RoleId AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID";
                            oledbCommand.Parameters.AddWithValue("DataReferenceId", objApproval.sDataReferenceId);
                            oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                            oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);

                            // oledbCommand.Parameters.AddWithValue("BOId", objApproval.sDataReferenceId);
                            string sResult = objconn.get_value(sQry, oledbCommand);

                            if (objApproval.sRoleId == "2")
                            {
                                objComm.sSMSkey = "SMStoWorkOrderCreate";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString());
                            }
                            else
                            {
                                oledbCommand = new OleDbCommand();
                                if (objApproval.sApproveStatus == "3")
                                {
                                    sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                    sQry += " DF_ID =:DataReferenceId AND WO_DATA_ID=:DataReferenceId1 AND DF_DTC_CODE=DT_CODE AND ";
                                    sQry += "WO_NEXT_ROLE =:RoleIds AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId";
                                    oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                                    oledbCommand.Parameters.AddWithValue("DataReferenceId", objApproval.sDataReferenceId);
                                    oledbCommand.Parameters.AddWithValue("RoleIds", objApproval.sRoleId);
                                    oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                                    sResult = objconn.get_value(sQry, oledbCommand);

                                    objComm.sSMSkey = "SMStoReject";
                                    objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                    sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                                }
                                else
                                {
                                    objComm.sSMSkey = "SMStoWorkOrderApprover";
                                    objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                    sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(1).ToString(), sResult.Split('~').GetValue(2).ToString());
                                }

                            }
                        }

                    }

                    // Indent
                    if (objApproval.sBOId == "12")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "11")
                        {
                            sQry = "SELECT DT_CODE,DT_NAME,WO_NO FROM TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST WHERE WO_SLNO=:NewRecordId AND WO_DF_ID=DF_ID AND DF_DTC_CODE=DT_CODE";
                            oledbCommand.Parameters.AddWithValue("NewRecordId", objApproval.sNewRecordId);
                            dt = objconn.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoWorkOrder";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                  Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["DT_NAME"]));
                            }

                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            if (objApproval.sApproveStatus == "3")
                            {
                                sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                sQry += " DF_ID =:DataReferenceId AND WO_DATA_ID=:DataReferenceId1 AND DF_DTC_CODE=DT_CODE AND ";
                                sQry += "WO_NEXT_ROLE =:RoleId AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                                string sResult = objconn.get_value(sQry, oledbCommand);

                                objComm.sSMSkey = "SMStoReject";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                            }
                            else
                            {
                                sQry = "SELECT DT_CODE,DT_NAME,WO_NO FROM TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST WHERE WO_SLNO=:DataReferenceId1 AND WO_DF_ID=DF_ID AND DF_DTC_CODE=DT_CODE";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                                dt = objconn.getDataTable(sQry, oledbCommand);
                                if (dt.Rows.Count > 0)
                                {

                                    objComm.sSMSkey = "SMStoIndentCreate";
                                    objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                      Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["DT_NAME"]));
                                }
                            }


                        }
                    }

                    // Invoice Creation Approval
                    if (objApproval.sBOId == "29")
                    {
                        if (sPreviousBoId == "12")
                        {
                            sQry = "SELECT DT_CODE,DT_NAME,WO_NO,TI_INDENT_NO FROM TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST,TBLINDENT WHERE ";
                            sQry += " TI_ID=:NewRecordId2 AND WO_DF_ID=DF_ID AND DF_DTC_CODE=DT_CODE AND WO_SLNO=TI_WO_SLNO";
                            oledbCommand.Parameters.AddWithValue("NewRecordId2", objApproval.sNewRecordId);
                            dt = objconn.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoIndent";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                  Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]));
                            }
                        }
                    }

                    // Invoice Creation
                    if (objApproval.sBOId == "13")
                    {
                        if (sPreviousBoId == "29")
                        {
                            sQry = "SELECT DT_CODE,DT_NAME,WO_NO,TI_INDENT_NO FROM TBLWORKORDER,TBLDTCFAILURE,TBLDTCMAST,TBLINDENT WHERE ";
                            sQry += " TI_ID=:RecordId1 AND WO_DF_ID=DF_ID AND DF_DTC_CODE=DT_CODE AND WO_SLNO=TI_WO_SLNO";
                            oledbCommand.Parameters.AddWithValue("RecordId1", objApproval.sRecordId);
                            dt = objconn.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoIndent";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                  Convert.ToString(dt.Rows[0]["DT_CODE"]), Convert.ToString(dt.Rows[0]["WO_NO"]), Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]));
                            }
                        }

                    }

                    //Decommission
                    if (objApproval.sBOId == "14")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "13")
                        {
                            sQry = "SELECT TI_INDENT_NO,IN_INV_NO FROM TBLINDENT,TBLDTCINVOICE WHERE IN_TI_NO=TI_ID AND TI_ID=:DataReferenceId3";
                            oledbCommand.Parameters.AddWithValue("DataReferenceId3", objApproval.sDataReferenceId);
                            dt = objcon.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoInvoice";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                   Convert.ToString(dt.Rows[0]["TI_INDENT_NO"]), Convert.ToString(dt.Rows[0]["IN_INV_NO"]));
                            }

                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            if (objApproval.sApproveStatus == "3")
                            {
                                sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                sQry += " DF_DTC_CODE =:DataReferenceId4 AND WO_DATA_ID=:DataReferenceId5 AND DF_DTC_CODE=DT_CODE AND ";
                                sQry += "WO_NEXT_ROLE =:RoleId3 AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId3";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId4", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("DataReferenceId5", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("RoleId3", objApproval.sRoleId);
                                oledbCommand.Parameters.AddWithValue("WFObjectId3", objApproval.sWFObjectId);
                                string sResult = objconn.get_value(sQry, oledbCommand);

                                objComm.sSMSkey = "SMStoReject";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                            }
                            else
                            {
                                sQry = "SELECT DF_DTC_CODE,DF_EQUIPMENT_ID FROM TBLDTCFAILURE WHERE  DF_DTC_CODE=:DataReferenceId6 ";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId6", objApproval.sDataReferenceId);

                                dt = objcon.getDataTable(sQry, oledbCommand);
                                if (dt.Rows.Count > 0)
                                {
                                    objComm.sSMSkey = "SMStoDecommCreate";
                                    objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                       Convert.ToString(dt.Rows[0]["DF_DTC_CODE"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]));
                                }
                            }
                        }
                    }

                    // RI Acknoldgement
                    if (objApproval.sBOId == "15")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "14")
                        {
                            sQry = "SELECT IN_INV_NO,TR_RI_NO FROM TBLDTCINVOICE,TBLTCREPLACE WHERE TR_IN_NO=IN_NO AND TR_ID=:NewRecordId4";
                            oledbCommand.Parameters.AddWithValue("NewRecordId4", objApproval.sNewRecordId);
                            dt = objconn.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                objComm.sSMSkey = "SMStoDecomm";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                   Convert.ToString(dt.Rows[0]["IN_INV_NO"]), Convert.ToString(dt.Rows[0]["TR_RI_NO"]));
                            }

                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            if (objApproval.sApproveStatus == "3")
                            {
                                sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                sQry += " DF_DTC_CODE =:DataReferenceId1 AND WO_DATA_ID=:DataReferenceId2 AND DF_DTC_CODE=DT_CODE AND ";
                                sQry += "WO_NEXT_ROLE =:RoleId AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId1", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("DataReferenceId2", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                                string sResult = objconn.get_value(sQry, oledbCommand);

                                objComm.sSMSkey = "SMStoReject";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                            }
                            else
                            {
                                sQry = "SELECT TR_RI_NO,DF_EQUIPMENT_ID FROM TBLTCREPLACE,TBLDTCFAILURE,TBLTCDRAWN WHERE DF_ID=TD_DF_ID AND ";
                                sQry += " TD_INV_NO=TR_IN_NO AND TR_ID=:RecordId";

                                oledbCommand.Parameters.AddWithValue("RecordId", objApproval.sRecordId);


                                dt = objconn.getDataTable(sQry, oledbCommand);
                                if (dt.Rows.Count > 0)
                                {
                                    objComm.sSMSkey = "SMStoRICreate";
                                    objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                       Convert.ToString(dt.Rows[0]["TR_RI_NO"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]));
                                }
                            }

                        }
                    }


                    // Completion Report
                    if (objApproval.sBOId == "26")
                    {
                        oledbCommand = new OleDbCommand();
                        if (sPreviousBoId == "15")
                        {
                            sQry = "SELECT TR_RI_NO,DF_EQUIPMENT_ID FROM TBLTCREPLACE,TBLDTCFAILURE,TBLTCDRAWN WHERE DF_ID=TD_DF_ID AND ";
                            sQry += " TD_INV_NO=TR_IN_NO AND TR_ID=:RecordId";
                            oledbCommand.Parameters.AddWithValue("RecordId", objApproval.sRecordId);
                            dt = objconn.getDataTable(sQry, oledbCommand);
                            if (dt.Rows.Count > 0)
                            {
                                string qry = "SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_OFF_CODE=:OfficeCode";
                                oledbCommand.Parameters.AddWithValue("OfficeCode", objApproval.sOfficeCode.Substring(0, 2));
                                string sStoreName = objconn.get_value(qry, oledbCommand);

                                objComm.sSMSkey = "SMStoRI";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                   Convert.ToString(dt.Rows[0]["TR_RI_NO"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]), sStoreName);
                            }

                        }
                        else
                        {
                            oledbCommand = new OleDbCommand();
                            if (objApproval.sApproveStatus == "3")
                            {
                                sQry = "SELECT DF_DTC_CODE || '~' || DT_NAME || '~' || RO_NAME || '~' || BO_NAME FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLDTCMAST,TBLUSER,TBLROLES,TBLBUSINESSOBJECT WHERE ";
                                sQry += " DF_DTC_CODE =:DataReferenceId3 AND WO_DATA_ID=:DataReferenceId4 AND DF_DTC_CODE=DT_CODE AND ";
                                sQry += "WO_NEXT_ROLE =:RoleId AND WO_CR_BY=US_ID AND RO_ID=US_ROLE_ID AND WO_BO_ID=BO_ID AND WO_ID=:WFObjectId";
                                oledbCommand.Parameters.AddWithValue("DataReferenceId3", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("DataReferenceId4", objApproval.sDataReferenceId);
                                oledbCommand.Parameters.AddWithValue("RoleId", objApproval.sRoleId);
                                oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);

                                string sResult = objconn.get_value(sQry);

                                objComm.sSMSkey = "SMStoReject";
                                objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                sSMSText = String.Format(objComm.sSMSTemplate,
                                sResult.Split('~').GetValue(3).ToString(), sResult.Split('~').GetValue(0).ToString(), sResult.Split('~').GetValue(2).ToString());

                            }
                            else
                            {
                                sQry = "SELECT TR_RI_NO,DF_EQUIPMENT_ID,TD_TC_NO  FROM TBLTCREPLACE,TBLDTCFAILURE,TBLTCDRAWN WHERE DF_ID=TD_DF_ID AND ";
                                sQry += " TD_INV_NO=TR_IN_NO AND TR_ID=:RecordId2";
                                oledbCommand.Parameters.AddWithValue("RecordId2", objApproval.sRecordId);
                                dt = objconn.getDataTable(sQry, oledbCommand);
                                oledbCommand = new OleDbCommand();
                                if (dt.Rows.Count > 0)
                                {
                                    string qry = "SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_OFF_CODE=:OfficeCode1";
                                    oledbCommand.Parameters.AddWithValue("OfficeCode1", objApproval.sOfficeCode.Substring(0, 2));
                                    string sStoreName = objconn.get_value(qry, oledbCommand);

                                    objComm.sSMSkey = "SMStoCRCreate";
                                    objComm = objComm.GetsmsTempalte1(objComm, objconn);
                                    sSMSText = String.Format(objComm.sSMSTemplate,
                                       Convert.ToString(dt.Rows[0]["TR_RI_NO"]), Convert.ToString(dt.Rows[0]["DF_EQUIPMENT_ID"]), Convert.ToString(dt.Rows[0]["TD_TC_NO"]));
                                }
                            }

                        }
                    }

                    if (sSMSText == "")
                    {

                    }
                    else
                    {
                        objComm.DumpSms1(sMobileNo, sSMSText, objComm.sSMSTemplateID, objconn);
                    }
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                  System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        /// <summary>
        /// Check for Duplicate Approval
        /// </summary>
        /// <param name="sBOId"></param>
        /// <param name="sRoleId"></param>
        /// <param name="sFormName"></param>
        /// <returns></returns>
        public bool CheckDuplicateApprove(clsApproval objApproval)
        {

            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                string sApproveResult = string.Empty;

                if (objApproval.sWFAutoId != null)
                {
                    if (objApproval.sWFAutoId == "0")
                    {
                        strQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID=:WFObjectId AND WO_APPROVE_STATUS<>0";
                        oledbCommand.Parameters.AddWithValue("WFObjectId", objApproval.sWFObjectId);
                        sApproveResult = objcon.get_value(strQry, oledbCommand);
                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                    else
                    {
                        strQry = "SELECT WOA_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_ID=:WFAutoId AND WOA_INITIAL_ACTION_ID IS NOT NULL";
                        oledbCommand.Parameters.AddWithValue("WFAutoId", objApproval.sWFAutoId);
                        sApproveResult = objcon.get_value(strQry, oledbCommand);
                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return true;
            }
        }

        public DateTime getWorkorder_CreatedDate(string record_id, string BO_ID)
        {
            oledbCommand = new OleDbCommand();
            string strQry = string.Empty;
            string sWOcreated_Date = string.Empty;
            try
            {
                if (BO_ID == "15")
                {
                    strQry = "SELECT TO_CHAR(WO_DATE,'yyyy-mm-dd')WO_DATE FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE WHERE ";
                    strQry += " DF_ID =WO_DF_ID AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO=TR_IN_NO AND TR_ID=:record_id";
                    oledbCommand.Parameters.AddWithValue("record_id", record_id);
                    sWOcreated_Date = objcon.get_value(strQry, oledbCommand);
                }
                else if (BO_ID == "29" || BO_ID == "13")
                {
                    strQry = "SELECT DF_ID FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT WHERE ";
                    strQry += " DF_ID =WO_DF_ID AND WO_SLNO=TI_WO_SLNO  AND TI_ID=:record_id1";
                    oledbCommand.Parameters.AddWithValue("record_id1", record_id);
                    string df_id = objcon.get_value(strQry, oledbCommand);
                    oledbCommand = new OleDbCommand();
                    if (df_id != "")
                    {

                        strQry = "SELECT TO_CHAR(WO_DATE,'yyyy-mm-dd')WO_DATE FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT WHERE ";
                        strQry += " DF_ID =WO_DF_ID AND WO_SLNO=TI_WO_SLNO  AND TI_ID=:record_id2";
                        oledbCommand.Parameters.AddWithValue("record_id2", record_id);
                    }
                    else
                    {
                        strQry = "SELECT TO_CHAR(WO_DATE,'yyyy-mm-dd')WO_DATE FROM TBLWORKORDER,TBLINDENT WHERE ";
                        strQry += " WO_SLNO=TI_WO_SLNO  AND TI_ID=:record_ids";
                        oledbCommand.Parameters.AddWithValue("record_ids", record_id);
                    }

                    sWOcreated_Date = objcon.get_value(strQry, oledbCommand);
                }
                if (sWOcreated_Date != "")
                {
                    return Convert.ToDateTime(sWOcreated_Date);
                }
                else
                {
                    DateTime date = DateTime.MinValue;
                    return Convert.ToDateTime(date);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return Convert.ToDateTime(sWOcreated_Date);
            }
        }
        /// <summary>
        /// Checking duplicate approve at final approver and using begin connection only for work order
        /// </summary>
        /// <param name="objApproval"></param>
        /// <returns></returns>
        public bool CheckDuplicateAtFinalApprove(clsApproval objApproval, CustOledbConnection objconn)
        {

            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                string sApproveResult = string.Empty;

                if (objApproval.sWFAutoId != null)
                {
                    if (objApproval.sWFAutoId == "0")
                    {
                        strQry = "SELECT WO_BO_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + objApproval.sWFObjectId
                            + "' AND WO_RECORD_ID > 0 AND WO_APPROVE_STATUS<>0 AND WO_BO_ID=11 ";
                        sApproveResult = objconn.get_value(strQry);
                        if (sApproveResult != "")
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return true;
            }
        }


      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data.OleDb;
using System.Configuration;
using System.IO;

namespace IIITS.DTLMS.BL
{
    public class clsCRReport : clsRIApproval
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsRIApproval";
        OleDbCommand oledbCommand;
        /// <summary>
        /// Get Details For CR
        /// </summary>
        /// <param name="objRIApproval"></param>
        /// <returns></returns>
        public clsRIApproval GetDetailsForCR(clsCRReport objRIApproval)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                string strQry = string.Empty;
                DataTable dt = new DataTable();
                strQry = " SELECT TR_APPROVE_REMARKS AS COMMENT_KEEPER,TR_OIL_QUNTY,WO_USER_COMMENT AS COMMENT_OFFICER,TO_CHAR(TR_APPROVED_DATE,'DD/MM/YYYY') TR_APPROVED_DATE,WO_NO,WO_NO_DECOM,TO_CHAR(WO_DATE,'dd/mm/yyyy')WO_DATE,";
                strQry += " (SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID=WO_CR_BY) AS STORE_OFFICER, (SELECT US_FULL_NAME FROM TBLUSER WHERE US_ID=TR_APPROVED_BY) AS ";
                strQry += " STORE_KEEPER,TD_DF_ID,TR_RI_NO,TO_CHAR(TR_RI_DATE,'DD-MON-YYYY') TR_RI_DATE,TR_INVENTORY_QTY,TR_DECOM_INV_QTY,TR_RV_NO , TO_CHAR(TR_RV_DATE,'dd/MM/yyyy')TR_RV_DATE ,IN_MANUAL_INVNO , TO_CHAR(IN_DATE ,'dd/MM/yyyy')IN_DATE  FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE,TBLWORKFLOWOBJECTS,TBLTCDRAWN WHERE TR_ID=WO_RECORD_ID AND WO_BO_ID='15' AND TR_IN_NO=IN_NO AND IN_TI_NO=TI_ID AND TI_WO_SLNO=WO_SLNO AND";
                strQry += " WO_NEXT_ROLE='2' AND WO_RECORD_ID=:sDecommId AND TD_INV_NO=TR_IN_NO";
                oledbCommand.Parameters.AddWithValue("sDecommId", sDecommId);
                dt = ObjCon.getDataTable(strQry, oledbCommand);
                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sWorkOrderDate = Convert.ToString(dt.Rows[0]["WO_DATE"]);
                    objRIApproval.sComWorkOrder = Convert.ToString(dt.Rows[0]["WO_NO"]);
                    objRIApproval.sDecomWorkOrder = Convert.ToString(dt.Rows[0]["WO_NO_DECOM"]);
                    objRIApproval.sStoreKeeperName = Convert.ToString(dt.Rows[0]["STORE_KEEPER"]);
                    objRIApproval.sStoreOfficerName = Convert.ToString(dt.Rows[0]["STORE_OFFICER"]);
                    objRIApproval.sCommentByStoreKeeper = Convert.ToString(dt.Rows[0]["COMMENT_KEEPER"]);
                    objRIApproval.sCommentByStoreOfficer = Convert.ToString(dt.Rows[0]["COMMENT_OFFICER"]);
                    objRIApproval.sOilQuantity = Convert.ToString(dt.Rows[0]["TR_OIL_QUNTY"]);
                    objRIApproval.sApprovedDate = Convert.ToString(dt.Rows[0]["TR_APPROVED_DATE"]);
                    objRIApproval.sFailureId = Convert.ToString(dt.Rows[0]["TD_DF_ID"]);
                    objRIApproval.sRINo = Convert.ToString(dt.Rows[0]["TR_RI_NO"]);
                    objRIApproval.sRIDate = Convert.ToString(dt.Rows[0]["TR_RI_DATE"]);
                    objRIApproval.sInventoryQty = Convert.ToString(dt.Rows[0]["TR_INVENTORY_QTY"]);
                    objRIApproval.sDecommInventoryQty = Convert.ToString(dt.Rows[0]["TR_DECOM_INV_QTY"]);
                    objRIApproval.sRVNo = Convert.ToString(dt.Rows[0]["TR_RV_NO"]);
                    objRIApproval.sRVDate = Convert.ToString(dt.Rows[0]["TR_RV_DATE"]);
                    objRIApproval.sManInvNumber = Convert.ToString(dt.Rows[0]["IN_MANUAL_INVNO"]);
                    objRIApproval.sInvDate = Convert.ToString(dt.Rows[0]["IN_DATE"]);
                    GetDTCTCDetailsFromFailure(objRIApproval);
                }
                return objRIApproval;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDetailsForCR");
                return objRIApproval;
            }
        }
        /// <summary>
        /// Save Completion Report
        /// </summary>
        /// <param name="objRI"></param>
        /// <returns></returns>
        public string[] SaveCompletionReport(clsRIApproval objRI)
        {
            string[] Arr = new string[2];
            string strQry = string.Empty;
            CustOledbConnection objconn = new CustOledbConnection(Constants.Password);
            try
            {
                #region Workflow

                oledbCommand = new OleDbCommand();
                string strQry1 = "UPDATE TBLDTCFAILURE SET DF_REPLACE_FLAG=1,DF_REP_DATE=SYSDATE WHERE DF_ID='" + objRI.sFailureId + "' ";
                strQry1 += " AND DF_REPLACE_FLAG=0";

                strQry1 = strQry1.Replace("'", "''");

                string strQry2 = "UPDATE TBLTCREPLACE SET TR_INVENTORY_QTY='" + objRI.sInventoryQty + "',";
                strQry2 += " TR_DECOM_INV_QTY='" + objRI.sDecommInventoryQty + "', ";
                strQry2 += " TR_CR_DATE = TO_DATE('" + objRI.sCrDate + "','dd/MM/yyyy') WHERE TR_ID='" + objRI.sDecommId + "' ";

                strQry2 = strQry2.Replace("'", "''");

                clsApproval objApproval = new clsApproval();
                objApproval.sFormName = objRI.sFormName;
                objApproval.sRecordId = objRI.sDecommId;
                objApproval.sOfficeCode = objRI.sOfficeCode;
                objApproval.sClientIp = objRI.sClientIP;
                objApproval.sCrby = objRI.sCrby;
                objApproval.sWFObjectId = objRI.sWFObjectId;
                objApproval.sWFAutoId = objRI.sWFAutoId;
                objApproval.sDataReferenceId = objRI.sDTCCode;
                objApproval.sQryValues = strQry1 + ";" + strQry2;

                objApproval.sDescription = "Completion Report For DTC Code " + objRI.sDTCCode;

                strQry = "SELECT DF_LOC_CODE FROM TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCREPLACE WHERE DF_ID=WO_DF_ID ";
                strQry += " AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO=TR_IN_NO AND TR_ID=:sDecommId";
                oledbCommand.Parameters.AddWithValue("sDecommId", objRI.sDecommId);
                objApproval.sRefOfficeCode = ObjCon.get_value(strQry, oledbCommand);
                //objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();


                objApproval.sColumnNames = "TR_ID,TR_INVENTORY_QTY,TR_DECOM_INV_QTY,TR_CR_DATE";
                objApproval.sColumnValues = "" + objRI.sDecommId + "," + objRI.sInventoryQty + "," + objRI.sDecommInventoryQty + "," + objRI.sCrDate + "";
                objApproval.sTableNames = "TBLTCREPLACE";

                bool bResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }

                //objApproval.SaveWorkflowObjects(objApproval);

                if (objRI.sActionType == "M")
                {
                    objApproval.SaveWorkFlowDatalatest(objApproval, objconn);
                    objRI.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objconn.BeginTrans();
                    objApproval.SaveWorkFlowDatalatest(objApproval, objconn);
                    objRI.sWFDataId = objApproval.sWFDataId;
                    //objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjectslatest(objApproval, objconn);
                }


                Arr[0] = "Approved Successfully";
                Arr[1] = "0";
                return Arr;
                #endregion
            }
            catch (Exception ex)
            {
                objconn.RollBack();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveCompletionReport");
                throw ex;
                // return Arr;
            }
        }
        #region WorkFlow XML
        /// <summary>
        /// Get CR Details From XML
        /// </summary>
        /// <param name="objRIApproval"></param>
        /// <returns></returns>
        public clsRIApproval GetCRDetailsFromXML(clsRIApproval objRIApproval)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();
                if (objRIApproval.sWFDataId != "" && objRIApproval.sWFDataId != null)
                {
                    dt = objApproval.GetDatatableFromXML(objRIApproval.sWFDataId);
                }
                if (dt.Rows.Count > 0)
                {
                    objRIApproval.sInventoryQty = Convert.ToString(dt.Rows[0]["TR_INVENTORY_QTY"]);
                    objRIApproval.sDecommInventoryQty = Convert.ToString(dt.Rows[0]["TR_DECOM_INV_QTY"]);
                    if (dt.Columns.Contains("TR_CR_DATE"))
                    {
                        objRIApproval.sCrDate = Convert.ToString(dt.Rows[0]["TR_CR_DATE"]);
                    }
                    else
                        objRIApproval.sCrDate = null;
                }
                return objRIApproval;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRIDetailsFromXML");
                return objRIApproval;
            }
        }
        #endregion
        /// <summary>
        /// Getting the Failed DTC Details  by passsing DTC Code or Work order number 
        /// Here DTC Can be bifurcated also .
        /// </summary>
        /// <param name="DtcCode"></param>
        /// <param name="sWorkOrderNumber"></param>
        /// <param name="sofficeCode"></param>
        /// <returns></returns>
        public DataTable GetCRDetails(string DtcCode = "", string sWorkOrderNumber = "", string sofficeCode = "")
        {
            DataTable dtCRDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                if (DtcCode.Length != 0)
                {
                    oledbCommand = new OleDbCommand();
                    strQry = " SELECT DF_ID, DF_DTC_CODE, DF_EQUIPMENT_ID, WO_NO, WO_NO_DECOM, TO_CHAR(WO_DATE,'DD-MM-YYYY')WO_DATE FROM TBLDTCFAILURE,TBLWORKORDER ,";
                    strQry += "TBLFEEDER_BIFURCATION_DETAILS WHERE DF_ID = WO_DF_ID AND((DF_DTC_CODE = FBD_OLD_DTC_CODE) or(DF_DTC_CODE = FBD_NEW_DTC_CODE)) and DF_REPLACE_FLAG = 1";
                    strQry += " and((FBD_OLD_DTC_CODE = '" + DtcCode + "') OR(FBD_NEW_DTC_CODE = '" + DtcCode + "')) and  DF_LOC_CODE LIKE '" + sofficeCode + "%'";
                    dtCRDetails = ObjCon.getDataTable(strQry, oledbCommand);
                    if (dtCRDetails.Rows.Count == 0)
                    {
                        strQry = "SELECT DF_ID,DF_DTC_CODE,DF_EQUIPMENT_ID,WO_NO,WO_NO_DECOM,TO_CHAR(WO_DATE,'DD-MM-YYYY')WO_DATE FROM TBLDTCFAILURE,TBLWORKORDER ";
                        strQry += " WHERE DF_ID=WO_DF_ID AND DF_REPLACE_FLAG=1 AND DF_DTC_CODE=:DtcCode";
                        oledbCommand.Parameters.AddWithValue("DtcCode", DtcCode);
                        dtCRDetails = ObjCon.getDataTable(strQry, oledbCommand);
                    }
                }
                else
                {
                    strQry = "SELECT DF_ID,DF_DTC_CODE,DF_EQUIPMENT_ID,WO_NO,WO_NO_DECOM,TO_CHAR(WO_DATE,'DD-MM-YYYY')WO_DATE FROM TBLDTCFAILURE,TBLWORKORDER ";
                    strQry += " WHERE DF_ID=WO_DF_ID AND DF_REPLACE_FLAG=1  AND  ( (WO_NO = '" + sWorkOrderNumber + "') or  (WO_NO_DECOM = '" + sWorkOrderNumber + "') ) AND WO_OFF_CODE = '" + sofficeCode + "'    ";
                    dtCRDetails = ObjCon.getDataTable(strQry);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCRDetails");
                return dtCRDetails;
            }
            return dtCRDetails;
        }
        /// <summary>
        /// Get Feeder Bifurcate DTC
        /// </summary>
        /// <param name="sDTC"></param>
        /// <returns></returns>
        public DataTable GetFeederBifurcateDTC(string sDTC)
        {
            DataTable dtCRDetails = new DataTable();
            string strQry = string.Empty;
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = "select FBD_NEW_DTC_CODE,FBD_OLD_DTC_CODE from TBLFEEDER_BIFURCATION_DETAILS where  FBD_OLD_DTC_CODE=:DtcCode ";
                oledbCommand.Parameters.AddWithValue("DtcCode", sDTC);
                dtCRDetails = ObjCon.getDataTable(strQry, oledbCommand);
                if (dtCRDetails.Rows.Count == 0)
                {
                    strQry = "select FBD_NEW_DTC_CODE,FBD_OLD_DTC_CODE from TBLFEEDER_BIFURCATION_DETAILS where  FBD_NEW_DTC_CODE=:DtcCode ";
                    dtCRDetails = ObjCon.getDataTable(strQry, oledbCommand);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "Genaral", "GetFeederBifurcateDTC");
                return dtCRDetails;
            }
            return dtCRDetails;
        }
        /// <summary>
        /// Function to update CR status in MMS after approval of DO in dtlms 
        /// </summary>
        /// <param name="objCrReport"></param>
        /// <returns></returns>
        public string[] UpdateCRStatusMMS(clsCRReport objCrReport)
        {
            String[] updateStatus = new String[2];
            string strQry = string.Empty;
            string sWorkOrderId;
            try
            {
                oledbCommand = new OleDbCommand();
                strQry = "SELECT WO_SLNO  from TBLDTCFAILURE , TBLWORKORDER where  WO_DF_ID = DF_ID AND DF_DTC_CODE  = :sDTCCode  AND WO_NO =:sComWorkOrder";
                oledbCommand.Parameters.AddWithValue("sDTCCode", objCrReport.sDTCCode);
                oledbCommand.Parameters.AddWithValue("sComWorkOrder", objCrReport.sComWorkOrder);
                sWorkOrderId = ObjCon.get_value(strQry, oledbCommand);
                if (sWorkOrderId == "")
                {
                    updateStatus[0] = "Didnt Get the Work order Id";
                    updateStatus[1] = "0";
                }
                else
                {
                    strQry = "UPDATE TBLWORKORDER SET wo_cr_sync_flag='1', WO_CR_FLAG  = '1' ,   WO_CR_DATE= '" + objCrReport.sCrDate + "' ,WO_CR_DESC= '" + objCrReport.sRemarks + "' WHERE WO_SOURCE_SLNO = '" + sWorkOrderId + "'";
                    DtmmsWebService.Service1Client objWCF = new DtmmsWebService.Service1Client();
                    //  string sPath = "E:\\CRSTATUS\\" + DateTime.Now.ToString("yyyyMMdd") + "-CRFile.txt";
                    //   string sPath = Convert.ToString(ConfigurationSettings.AppSettings["crpath"]);
                    string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
                    if (!Directory.Exists(sFolderPath))
                    {
                        Directory.CreateDirectory(sFolderPath);
                    }
                    string sFileName = "CRFile";
                    string sPath = sFolderPath + "//" + sFileName + DateTime.Now.ToString("yyyyMMdd") + "-CRFile.txt";
                    //  sPath = sPath + DateTime.Now.ToString("yyyyMMdd") + "-CRFile.txt";
                    if (objWCF.updateCrStatus(strQry))
                    {
                        System.IO.File.AppendAllText(sPath, " Updated WorkOrderID  : " + sWorkOrderId + " Time : " + DateTime.Now + Environment.NewLine);
                        System.IO.File.AppendAllText(sPath, " Query : " + strQry + " Time : " + DateTime.Now + Environment.NewLine);
                        updateStatus[0] = "Updated Successfully";
                        updateStatus[1] = "1";
                    }
                    else
                    {
                        System.IO.File.AppendAllText(sPath, " Rejected WorkOrderID  : " + sWorkOrderId + " Time : " + DateTime.Now + Environment.NewLine);
                        System.IO.File.AppendAllText(sPath, " Query : " + strQry + " Time : " + DateTime.Now + Environment.NewLine);
                    }
                }
                return updateStatus;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "UpdateCRStatusMMS");
                return updateStatus;
            }
        }
    }
}
